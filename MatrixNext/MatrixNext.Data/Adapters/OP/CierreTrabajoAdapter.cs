using Dapper;
using MatrixNext.Data.Models.OP;
using MatrixNext.Data.Context;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.OP;

/// <summary>
/// Implementación del adapter para cierre de trabajos
/// Maneja validación de documentos y cambio de estado
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.6
/// </summary>
public class CierreTrabajoAdapter : ICierreTrabajoAdapter
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CierreTrabajoAdapter> _logger;

    public CierreTrabajoAdapter(
        ApplicationDbContext context,
        ILogger<CierreTrabajoAdapter> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene información del trabajo para cierre
    /// </summary>
    public async Task<CierreTrabajoDto?> ObtenerTrabajoAsync(long idTrabajo)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<CierreTrabajoDto>(
                @"SELECT 
                    IdTrabajo,
                    Estado AS EstadoAnterior,
                    'Cerrado' AS EstadoNuevo,
                    GETDATE() AS FechaCierre,
                    NULL AS Observaciones,
                    0 AS ValidacionDocumentosOk,
                    0 AS TotalDocumentosValidados
                FROM PY_Trabajos
                WHERE IdTrabajo = @IdTrabajo",
                new { IdTrabajo = idTrabajo }
            );

            if (result != null)
            {
                _logger.LogInformation("Trabajo obtenido para cierre. IdTrabajo: {IdTrabajo}, Estado: {Estado}", 
                    idTrabajo, result.EstadoAnterior);
            }
            else
            {
                _logger.LogWarning("Trabajo no encontrado para cierre. IdTrabajo: {IdTrabajo}", idTrabajo);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo trabajo para cierre. IdTrabajo: {IdTrabajo}", idTrabajo);
            return null;
        }
    }

    /// <summary>
    /// Valida documentos escaneados en GD
    /// </summary>
    public async Task<ValidacionDocumentosDto> ValidarDocumentosAsync(long idTrabajo)
    {
        try
        {
            using var connection = _context.CreateConnection();

            // Obtener documentos escaneados para el trabajo
            var documentos = await connection.QueryAsync<dynamic>(
                @"SELECT 
                    COUNT(*) AS Total,
                    SUM(CASE WHEN Estado = 'Escaneado' THEN 1 ELSE 0 END) AS Validados
                FROM GD_DocumentosEscaneados
                WHERE IdTrabajo = @IdTrabajo",
                new { IdTrabajo = idTrabajo }
            );

            var doc = documentos.FirstOrDefault();
            var total = (int?)(doc?.Total) ?? 0;
            var validados = (int?)(doc?.Validados) ?? 0;

            var resultado = new ValidacionDocumentosDto
            {
                TotalDocumentos = total,
                DocumentosValidados = validados,
                EsValido = total > 0 && validados == total
            };

            if (!resultado.EsValido && total > 0)
            {
                resultado.ErroresValidacion.Add($"Documentos no escaneados: {total - validados} de {total}");
                resultado.MensajeError = "No todos los documentos han sido escaneados. No se puede cerrar el trabajo.";
            }
            else if (total == 0)
            {
                resultado.ErroresValidacion.Add("No hay documentos asociados al trabajo");
                resultado.MensajeError = "Asocie documentos antes de cerrar el trabajo.";
            }

            _logger.LogInformation(
                "Documentos validados para cierre. IdTrabajo: {IdTrabajo}, Validados: {Validados}/{Total}, EsValido: {EsValido}",
                idTrabajo, validados, total, resultado.EsValido);

            return resultado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando documentos para cierre. IdTrabajo: {IdTrabajo}", idTrabajo);
            return new ValidacionDocumentosDto
            {
                EsValido = false,
                MensajeError = "Error al validar documentos: " + ex.Message
            };
        }
    }

    /// <summary>
    /// Cambia estado del trabajo a "Cerrado"
    /// </summary>
    public async Task<bool> CambiarEstadoACerradoAsync(long idTrabajo, string? observaciones, long usuarioId)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", idTrabajo);
            parameters.Add("@NuevoEstado", "Cerrado");
            parameters.Add("@FechaCierre", DateTime.UtcNow);
            parameters.Add("@Observaciones", observaciones);
            parameters.Add("@CerradoPor", usuarioId);
            parameters.Add("@FechaCierreDatos", DateTime.UtcNow);

            // Intentar usar SP, si no existe usar query
            try
            {
                var result = await connection.ExecuteAsync(
                    "PY_Trabajos_UpdateEstado",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                _logger.LogInformation(
                    "Estado del trabajo actualizado a Cerrado. IdTrabajo: {IdTrabajo}, Rows: {Rows}",
                    idTrabajo, result);

                return result > 0;
            }
            catch (Exception spEx)
            {
                _logger.LogWarning(spEx, "SP PY_Trabajos_UpdateEstado no existe, usando query directa");

                // Fallback a query directa
                var result = await connection.ExecuteAsync(
                    @"UPDATE PY_Trabajos
                      SET Estado = @NuevoEstado,
                          FechaCierre = @FechaCierre,
                          Observaciones = @Observaciones,
                          ModificadoPor = @CerradoPor,
                          FechaModificacion = @FechaCierreDatos
                      WHERE IdTrabajo = @IdTrabajo",
                    parameters
                );

                _logger.LogInformation(
                    "Estado del trabajo actualizado vía query. IdTrabajo: {IdTrabajo}, Rows: {Rows}",
                    idTrabajo, result);

                return result > 0;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cambiando estado del trabajo a Cerrado. IdTrabajo: {IdTrabajo}", idTrabajo);
            return false;
        }
    }

    /// <summary>
    /// Obtiene datos del trabajo para notificación
    /// </summary>
    public async Task<(string NumeroTrabajo, string CodigoProyecto, string NombreProyecto)> ObtenerDatosTrabajoAsync(long idTrabajo)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                @"SELECT 
                    t.NumeroTrabajo,
                    p.CodigoProyecto,
                    p.NombreProyecto
                FROM PY_Trabajos t
                INNER JOIN PY_Proyectos p ON t.IdProyecto = p.IdProyecto
                WHERE t.IdTrabajo = @IdTrabajo",
                new { IdTrabajo = idTrabajo }
            );

            if (result != null)
            {
                return (
                    result.NumeroTrabajo ?? string.Empty,
                    result.CodigoProyecto ?? string.Empty,
                    result.NombreProyecto ?? string.Empty
                );
            }

            return (string.Empty, string.Empty, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo datos del trabajo. IdTrabajo: {IdTrabajo}", idTrabajo);
            return (string.Empty, string.Empty, string.Empty);
        }
    }
}
