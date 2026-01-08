using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Implementación del servicio de gestión documental para cierre de trabajos.
    /// </summary>
    public class OpGestionDocumentalService : IOpGestionDocumentalService
    {
        private readonly string? _connectionString;
        private readonly ILogger<OpGestionDocumentalService> _logger;

        public OpGestionDocumentalService(
            MatrixDbContext context,
            ILogger<OpGestionDocumentalService> logger)
        {
            _connectionString = context.Database.GetConnectionString();
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task<(bool TodosEncontrados, List<string> DocumentosFaltantes)> ValidarDocumentosEscaneadosAsync(
            long trabajoId,
            int rolResponsableCierre,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                _logger.LogWarning("No se encontró cadena de conexión MatrixDb para validar documentos GD");
                return (false, new List<string> { "Error de configuración" });
            }

            try
            {
                await using var connection = new SqlConnection(_connectionString);

                // Obtener documentos escaneados no encontrados (Encontrado = false)
                const string sql = @"
                    EXEC GD_EscanerDocumentos_Get 
                        @Id = NULL,
                        @IdTrabajo = @TrabajoId,
                        @IdDocumento = NULL,
                        @CodEncontrado = 0,
                        @RolResponsableCierre = @RolResponsableCierre";

                var documentosNoEncontrados = await connection.QueryAsync<DocumentoEscaneadoDto>(
                    sql,
                    new { TrabajoId = trabajoId, RolResponsableCierre = rolResponsableCierre },
                    commandTimeout: 30);

                var lista = documentosNoEncontrados.ToList();

                if (!lista.Any())
                {
                    _logger.LogInformation(
                        "Todos los documentos de cierre encontrados para trabajo {TrabajoId}",
                        trabajoId);
                    return (true, new List<string>());
                }

                var nombresDocumentos = lista
                    .Select(d => d.Documento ?? $"Documento ID {d.IdDocumento}")
                    .ToList();

                _logger.LogWarning(
                    "Faltan {Count} documentos para trabajo {TrabajoId}: {Documentos}",
                    lista.Count, trabajoId, string.Join(", ", nombresDocumentos));

                return (false, nombresDocumentos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar documentos escaneados para trabajo {TrabajoId}", trabajoId);
                return (false, new List<string> { "Error al validar documentos" });
            }
        }

        /// <inheritdoc />
        public async Task<List<DocumentoCierreVM>> ObtenerDocumentosFaltantesAsync(
            long trabajoId,
            int rolResponsableCierre,
            CancellationToken cancellationToken = default)
        {
            var resultado = new List<DocumentoCierreVM>();

            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                _logger.LogWarning("No se encontró cadena de conexión MatrixDb");
                return resultado;
            }

            try
            {
                await using var connection = new SqlConnection(_connectionString);

                // Obtener documentos de cierre con su estado de escaneo
                const string sql = @"
                    SELECT 
                        e.IdDocumento,
                        e.Documento,
                        e.CodEncontrado AS Encontrado,
                        e.FechaEscaneo,
                        e.Observacion,
                        c.URLOtroServidor AS RutaUNC
                    FROM GD.dbo.GD_EscanerDocumentos_Get(
                        NULL, @TrabajoId, NULL, 0, @RolResponsableCierre
                    ) e
                    LEFT JOIN CI.dbo.CI_DocumentosCierre_Get(@TrabajoId, @RolResponsableCierre) c
                        ON e.IdDocumento = c.IdDocumento
                    ORDER BY e.Documento";

                var documentos = await connection.QueryAsync<DocumentoEscaneadoDto>(
                    sql,
                    new { TrabajoId = trabajoId, RolResponsableCierre = rolResponsableCierre },
                    commandTimeout: 30);

                resultado = documentos.Select(d => new DocumentoCierreVM
                {
                    IdDocumento = d.IdDocumento ?? 0,
                    NombreDocumento = d.Documento ?? $"Documento {d.IdDocumento}",
                    Encontrado = d.CodEncontrado ?? false,
                    FechaEscaneo = d.FechaEscaneo,
                    Observacion = d.Observacion,
                    RutaUNC = d.URLOtroServidor
                }).ToList();

                _logger.LogInformation(
                    "Obtenidos {Count} documentos faltantes para trabajo {TrabajoId}",
                    resultado.Count, trabajoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener documentos faltantes para trabajo {TrabajoId}", trabajoId);
            }

            return resultado;
        }

        /// <inheritdoc />
        public async Task<Dictionary<string, bool>> ValidarRutasUNCAsync(
            List<string> rutas,
            CancellationToken cancellationToken = default)
        {
            var resultado = new Dictionary<string, bool>();

            if (rutas == null || !rutas.Any())
            {
                return resultado;
            }

            await Task.Run(() =>
            {
                foreach (var ruta in rutas)
                {
                    try
                    {
                        if (string.IsNullOrWhiteSpace(ruta))
                        {
                            resultado[ruta ?? "NULL"] = false;
                            continue;
                        }

                        // Validar si la ruta existe
                        var existe = Directory.Exists(ruta);
                        resultado[ruta] = existe;

                        _logger.LogDebug(
                            "Validación ruta UNC {Ruta}: {Resultado}",
                            ruta, existe ? "Accesible" : "No accesible");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error al validar ruta UNC {Ruta}", ruta);
                        resultado[ruta] = false;
                    }
                }
            }, cancellationToken);

            return resultado;
        }

        /// <inheritdoc />
        public async Task<int> SincronizarDocumentosEscaneadosAsync(
            long trabajoId,
            int rolResponsableCierre,
            string servidor,
            string unidad,
            string jbi,
            string nombreTrabajo,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                _logger.LogWarning("No se encontró cadena de conexión MatrixDb");
                return 0;
            }

            var documentosActualizados = 0;

            try
            {
                await using var connection = new SqlConnection(_connectionString);

                // 1. Obtener documentos ya escaneados
                var documentosEscaneados = await connection.QueryAsync<DocumentoEscaneadoDto>(
                    "EXEC GD_EscanerDocumentos_Get @Id=NULL, @IdTrabajo=@TrabajoId, @IdDocumento=NULL, @CodEncontrado=NULL, @RolResponsableCierre=NULL",
                    new { TrabajoId = trabajoId },
                    commandTimeout: 30);

                // 2. Obtener documentos de cierre requeridos
                var documentosCierre = await connection.QueryAsync<DocumentoCierreDto>(
                    "EXEC CI_DocumentosCierre_Get @IdTrabajo=@TrabajoId, @IdRolResponsable=@RolResponsableCierre",
                    new { TrabajoId = trabajoId, RolResponsableCierre = rolResponsableCierre },
                    commandTimeout: 30);

                var listaEscaneados = documentosEscaneados.ToList();
                var listaCierre = documentosCierre.ToList();

                // 3. Eliminar documentos escaneados que ya no son de cierre
                var idsEscaneados = listaEscaneados.Select(d => d.IdDocumento ?? 0).ToList();
                var idsCierre = listaCierre.Select(d => d.IdDocumento).ToList();
                var idsAEliminar = idsEscaneados.Except(idsCierre).ToList();

                foreach (var idDocumento in idsAEliminar)
                {
                    await connection.ExecuteAsync(
                        "EXEC GD_EscanerDocumentos_Del @IdTrabajo=@TrabajoId, @Id=NULL, @IdDocumento=@IdDocumento",
                        new { TrabajoId = trabajoId, IdDocumento = idDocumento },
                        commandTimeout: 30);
                }

                _logger.LogInformation(
                    "Eliminados {Count} documentos obsoletos de GD_EscanerDocumentos para trabajo {TrabajoId}",
                    idsAEliminar.Count, trabajoId);

                // 4. Escanear rutas UNC y actualizar registros
                // NOTA: Esto requiere acceso a red con credenciales configuradas
                // Por ahora solo actualizamos con lógica básica sin verificación UNC real
                // TODO: Implementar NetworkConnection con credenciales de appsettings

                foreach (var documento in listaCierre)
                {
                    var encontrado = documento.Cantidad > 0; // Lógica básica: si tiene cantidad, está encontrado

                    var documentoExistente = listaEscaneados.FirstOrDefault(e => e.IdDocumento == documento.IdDocumento);

                    if (documentoExistente != null)
                    {
                        // Actualizar documento existente
                        await connection.ExecuteAsync(
                            "EXEC GD_EscanerDocumentos_Edit @Id=@Id, @IdTrabajo=@TrabajoId, @IdDocumento=@IdDocumento, @Encontrado=@Encontrado, @Observacion=NULL",
                            new
                            {
                                Id = documentoExistente.Id,
                                TrabajoId = trabajoId,
                                IdDocumento = documento.IdDocumento,
                                Encontrado = encontrado
                            },
                            commandTimeout: 30);
                    }
                    else
                    {
                        // Insertar nuevo documento
                        await connection.ExecuteAsync(
                            "EXEC GD_EscanerDocumentos_Add @IdTrabajo=@TrabajoId, @IdDocumento=@IdDocumento, @Encontrado=@Encontrado",
                            new
                            {
                                TrabajoId = trabajoId,
                                IdDocumento = documento.IdDocumento,
                                Encontrado = encontrado
                            },
                            commandTimeout: 30);
                    }

                    documentosActualizados++;
                }

                _logger.LogInformation(
                    "Sincronizados {Count} documentos para trabajo {TrabajoId}",
                    documentosActualizados, trabajoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al sincronizar documentos escaneados para trabajo {TrabajoId}", trabajoId);
            }

            return documentosActualizados;
        }

        /// <summary>
        /// DTO interno para documento escaneado desde GD_EscanerDocumentos_Get
        /// </summary>
        private class DocumentoEscaneadoDto
        {
            public long Id { get; set; }
            public long? IdTrabajo { get; set; }
            public long? IdDocumento { get; set; }
            public string? Documento { get; set; }
            public bool? CodEncontrado { get; set; }
            public string? Encontrado { get; set; }
            public string? Responsable { get; set; }
            public DateTime? FechaEscaneo { get; set; }
            public string? Observacion { get; set; }
            public int? RolResponsableCierre { get; set; }
            public string? URLOtroServidor { get; set; }
        }

        /// <summary>
        /// DTO interno para documento de cierre desde CI_DocumentosCierre_Get
        /// </summary>
        private class DocumentoCierreDto
        {
            public long IdDocumento { get; set; }
            public string? Documento { get; set; }
            public int? Cantidad { get; set; }
            public string? URLOtroServidor { get; set; }
            public string? TipoArchivo { get; set; }
            public int? RolResponsableCierre { get; set; }
            public bool? Encontrado { get; set; }
            public DateTime? FechaEscaneo { get; set; }
            public string? Observacion { get; set; }
        }
    }
}
