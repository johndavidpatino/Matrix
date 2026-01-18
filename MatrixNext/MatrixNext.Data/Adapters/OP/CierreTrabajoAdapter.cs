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
            
            // CORREGIDO: PY_Trabajos → PY_Trabajo, IdTrabajo → id, Estado → JobBk_Estado
            var result = await connection.QueryFirstOrDefaultAsync<CierreTrabajoDto>(
                @"SELECT 
                    id AS IdTrabajo,
                    JobBk_Estado AS EstadoAnterior,
                    'Cerrado' AS EstadoNuevo,
                    GETDATE() AS FechaCierre,
                    NULL AS Observaciones,
                    0 AS ValidacionDocumentosOk,
                    0 AS TotalDocumentosValidados
                FROM PY_Trabajo
                WHERE id = @IdTrabajo",
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

            // Obtener documentos escaneados para el trabajo (CORREGIDO: GD_EscanerDocumentos)
            var documentos = await connection.QueryAsync<dynamic>(
                @"SELECT 
                    COUNT(*) AS Total,
                    SUM(CASE WHEN Encontrado = 1 THEN 1 ELSE 0 END) AS Validados
                FROM GD_EscanerDocumentos
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
                MensajeError = "Error al validar documentos. Por favor intente nuevamente."
            };
        }
    }

    /// <summary>
    /// Cambia estado del trabajo a "Cerrado"
    /// NOTA: SP PY_Trabajos_UpdateEstado no existe - usar UPDATE directo
    /// </summary>
    public async Task<bool> CambiarEstadoACerradoAsync(long idTrabajo, string? observaciones, long usuarioId)
    {
        try
        {
            using var connection = _context.CreateConnection();

            // SP PY_Trabajos_UpdateEstado no existe - usar UPDATE directo
            var result = await connection.ExecuteAsync(
                @"UPDATE PY_Trabajo
                  SET Estado = @NuevoEstado,
                      Observaciones = @Observaciones,
                      ModificadoPor = @CerradoPor,
                      FechaModificacion = @FechaCierreDatos
                  WHERE id = @IdTrabajo",
                new
                {
                    IdTrabajo = idTrabajo,
                    NuevoEstado = (short)3, // 3 = Cerrado típicamente
                    Observaciones = observaciones,
                    CerradoPor = usuarioId,
                    FechaCierreDatos = DateTime.UtcNow
                }
            );

            _logger.LogInformation(
                "Estado del trabajo actualizado a Cerrado. IdTrabajo: {IdTrabajo}, Rows: {Rows}",
                idTrabajo, result);

            return result > 0;
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
                    CAST(t.id AS VARCHAR) AS NumeroTrabajo,
                    p.JobBook AS CodigoProyecto,
                    p.Nombre AS NombreProyecto
                FROM PY_Trabajo t
                INNER JOIN PY_Proyectos p ON t.ProyectoId = p.id
                WHERE t.id = @IdTrabajo",
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
