using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.OP.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Servicio para el registro de actividades de producción en OP.
    /// Implementa lógica de cascading dropdowns, búsqueda de JobBooks y validaciones.
    /// 
    /// Performance optimization (S4-006.3): Catalog caching
    /// - Unidades, Actividades, SubActividades now cached with 15-minute TTL
    /// - Expected improvement: 3 DB queries → 1 cached response
    /// - Response time: ~50ms → <5ms for cached data
    /// </summary>
    public class OpRegistroProduccionService : IOpRegistroProduccionService
    {
        private readonly MatrixDbContext _context;
        private readonly IOpCatalogCacheService _catalogCache;
        private readonly ILogger<OpRegistroProduccionService> _logger;

        public OpRegistroProduccionService(
            MatrixDbContext context,
            IOpCatalogCacheService catalogCache,
            ILogger<OpRegistroProduccionService> logger)
        {
            _context = context;
            _catalogCache = catalogCache;
            _logger = logger;
        }

        /// <inheritdoc />
        /// <remarks>
        /// Performance: Cached with 15-minute TTL (S4-006.3)
        /// First request: ~50ms (database query)
        /// Subsequent requests: <5ms (in-memory cache hit)
        /// </remarks>
        public async Task<List<CatalogoItemDto>> ObtenerUnidadesAsync()
        {
            try
            {
                // Use cache service (S4-006.3 optimization)
                var unidades = await _catalogCache.ObtenerUnidadesAsync();
                _logger.LogInformation("Obtenidas {Count} unidades para registro", unidades.Count);
                return unidades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo unidades");
                throw;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Performance: Cached with 15-minute TTL (S4-006.3)
        /// First request: ~50ms (database query)
        /// Subsequent requests: <5ms (in-memory cache hit)
        /// </remarks>
        public async Task<List<CatalogoItemDto>> ObtenerActividadesAsync(int unidadId)
        {
            try
            {
                // Use cache service (S4-006.3 optimization)
                var actividades = await _catalogCache.ObtenerActividadesAsync(unidadId);
                _logger.LogInformation("Obtenidas {Count} actividades para unidad {UnidadId}", actividades.Count, unidadId);
                return actividades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo actividades para unidad {UnidadId}", unidadId);
                throw;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// Performance: Cached with 15-minute TTL (S4-006.3)
        /// First request: ~50ms (database query)
        /// Subsequent requests: <5ms (in-memory cache hit)
        /// </remarks>
        public async Task<List<CatalogoItemDto>> ObtenerSubactividadesAsync(int actividadId)
        {
            try
            {
                // Use cache service (S4-006.3 optimization)
                var subactividades = await _catalogCache.ObtenerSubactividadesAsync(actividadId);
                _logger.LogInformation("Obtenidas {Count} subactividades para actividad {ActividadId}", subactividades.Count, actividadId);
                return subactividades;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo subactividades para actividad {ActividadId}", actividadId);
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<List<JobBookDto>> BuscarJobBooksAsync(string criterio, string tipo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(criterio))
                    return new List<JobBookDto>();

                var tipoNormalizado = (tipo ?? "JBE").Trim().ToUpperInvariant();
                var connectionString = _context.Database.GetConnectionString();
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    var criterioLike = $"%{criterio}%";
                    List<JobBookDto> resultado;

                    if (tipoNormalizado == "JBI")
                    {
                        resultado = (await connection.QueryAsync<JobBookDto>(
                            @"SELECT 
                                CAST(Id AS int) AS JobBookId,
                                JobBook AS Codigo,
                                NombreTrabajo AS Nombre,
                                'JBI' AS Tipo,
                                CAST(Id AS int) AS TrabajoId,
                                CAST(Estado AS varchar(10)) AS Estado
                              FROM PY_Trabajo
                              WHERE Estado IN (1,2,13,15)
                                AND JobBook IS NOT NULL
                                AND (
                                    @Criterio IS NULL
                                    OR JobBook LIKE @Criterio
                                    OR NombreTrabajo LIKE @Criterio
                                    OR CAST(Id AS varchar(10)) = @CriterioExacto
                                )
                              ORDER BY JobBook",
                            new { Criterio = criterioLike, CriterioExacto = criterio },
                            commandType: System.Data.CommandType.Text,
                            commandTimeout: 30)).ToList();
                    }
                    else if (tipoNormalizado == "CC")
                    {
                        resultado = new List<JobBookDto>
                        {
                            new JobBookDto
                            {
                                JobBookId = 0,
                                Codigo = "Unidad",
                                Nombre = "Unidad",
                                Tipo = "CC",
                                TrabajoId = 0,
                                Estado = string.Empty
                            }
                        };
                    }
                    else
                    {
                        resultado = (await connection.QueryAsync<JobBookDto>(
                            @"SELECT 
                                CAST(Id AS int) AS JobBookId,
                                JobBook AS Codigo,
                                Nombre,
                                'JBE' AS Tipo,
                                CAST(0 AS int) AS TrabajoId,
                                CAST(Estado AS varchar(10)) AS Estado
                              FROM PY_Proyectos
                              WHERE Estado IN (1,2,13,15)
                                AND JobBook IS NOT NULL
                                AND (
                                    @Criterio IS NULL
                                    OR JobBook LIKE @Criterio
                                    OR Nombre LIKE @Criterio
                                )
                              ORDER BY JobBook",
                            new { Criterio = criterioLike },
                            commandType: System.Data.CommandType.Text,
                            commandTimeout: 30)).ToList();
                    }

                    _logger.LogInformation("B??squeda de JobBooks: tipo={Tipo}, criterio={Criterio}, resultados={Count}", tipo, criterio, resultado.Count);
                    return resultado;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error buscando JobBooks con criterio {Criterio}", criterio);
                throw;
            }
        }

        public async Task<int> RegistrarActividadAsync(RegistroProduccionDto registro)
        {
            try
            {
                // Validar primero
                var (valido, mensaje) = await ValidarRegistroAsync(registro);
                if (!valido)
                    throw new InvalidOperationException($"Registro inv??lido: {mensaje}");

                var connectionString = _context.Database.GetConnectionString();
                using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
                {
                    int? trabajoId = null;
                    int? estudioId = null;

                    if (registro.JobBookId.HasValue && registro.JobBookId.Value > 0)
                    {
                        var foundTrabajo = await connection.ExecuteScalarAsync<int?>(
                            "SELECT TOP 1 Id FROM PY_Trabajo WHERE Id = @Id",
                            new { Id = registro.JobBookId.Value });

                        if (foundTrabajo.HasValue)
                        {
                            trabajoId = registro.JobBookId.Value;
                        }
                        else
                        {
                            var foundProyecto = await connection.ExecuteScalarAsync<int?>(
                                "SELECT TOP 1 Id FROM PY_Proyectos WHERE Id = @Id",
                                new { Id = registro.JobBookId.Value });

                            if (foundProyecto.HasValue)
                            {
                                estudioId = registro.JobBookId.Value;
                            }
                        }
                    }

                    TimeSpan? horaInicio = null;
                    if (!string.IsNullOrWhiteSpace(registro.HoraInicio) && TimeSpan.TryParse(registro.HoraInicio, out var horaInicioParsed))
                    {
                        horaInicio = horaInicioParsed;
                    }

                    TimeSpan? horaFin = null;
                    if (!string.IsNullOrWhiteSpace(registro.HoraFin) && TimeSpan.TryParse(registro.HoraFin, out var horaFinParsed))
                    {
                        horaFin = horaFinParsed;
                    }

                    var parameters = new DynamicParameters();
                    parameters.Add("@Actividad", registro.ActividadId);
                    parameters.Add("@SubActividad", registro.SubactividadId > 0 ? registro.SubactividadId : (int?)null);
                    parameters.Add("@Unidad", registro.UnidadId);
                    parameters.Add("@TrabajoId", trabajoId);
                    parameters.Add("@EstudioId", estudioId);
                    parameters.Add("@Fecha", DateTime.ParseExact(registro.Fecha, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
                    parameters.Add("@HoraInicio", horaInicio);
                    parameters.Add("@HoraFin", horaFin);
                    parameters.Add("@Cantidad", registro.Cantidad);
                    parameters.Add("@Observacion", string.IsNullOrWhiteSpace(registro.Observaciones) ? (object)DBNull.Value : registro.Observaciones);
                    parameters.Add("@Estado", (int?)null);
                    parameters.Add("@ValidadoPor", (long?)null);
                    parameters.Add("@PersonaId", (long?)registro.UsuarioId);
                    parameters.Add("@EsReproceso", (bool?)null);
                    parameters.Add("@CantidadEfectivas", (int?)null);
                    parameters.Add("@TipoReproceso", (byte?)null);
                    parameters.Add("@TipoAplicativoProceso", (byte?)null);
                    parameters.Add("@CantVarsScript", (int?)null);
                    parameters.Add("@CantVarsExport", (int?)null);

                    // Ejecutar SP OP_Produccion_Add
                    var idRegistroDecimal = await connection.ExecuteScalarAsync<decimal?>(
                        "OP_Produccion_Add",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure,
                        commandTimeout: 30
                    );

                    var idRegistro = idRegistroDecimal.HasValue ? (int)idRegistroDecimal.Value : 0;

                    _logger.LogInformation("Actividad de producci??n registrada: ID={IdRegistro}, Usuario={Usuario}", idRegistro, registro.UsuarioId);
                    return idRegistro;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registrando actividad de producci??n");
                throw;
            }
        }

        public async Task<(bool Valid, string Message)> ValidarRegistroAsync(RegistroProduccionDto registro)
        {
            try
            {
                if (registro == null)
                    return (false, "El registro no puede ser nulo");

                if (registro.UnidadId <= 0)
                    return (false, "Debe seleccionar una unidad");

                if (registro.ActividadId <= 0)
                    return (false, "Debe seleccionar una actividad");

                if (registro.SubactividadId <= 0)
                    return (false, "Debe seleccionar una subactividad");

                if (registro.Cantidad <= 0)
                    return (false, "La cantidad debe ser mayor a 0");

                if (string.IsNullOrWhiteSpace(registro.Fecha))
                    return (false, "Debe especificar la fecha del registro");

                if (!DateTime.TryParse(registro.Fecha, out var fecha))
                    return (false, "La fecha tiene formato inválido");

                if (fecha > DateTime.Now)
                    return (false, "No se puede registrar actividades en fechas futuras");

                return (true, "");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando registro de producción");
                throw;
            }
        }
    }
}
