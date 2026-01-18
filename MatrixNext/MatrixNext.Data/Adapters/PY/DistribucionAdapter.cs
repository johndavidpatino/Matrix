/// <summary>
/// Adapter para distribución de entrevistas, variables de control e InHome visits
/// 
/// IMPORTANTE - VALIDACIÓN DE BD 2025-01:
/// Las siguientes tablas NO EXISTEN en la BD CO_Matrix_Intranet:
/// - PY_DistribucionEntrevistas (especulativa)
/// - PY_Metodologias (especulativa) 
/// - OP_Unidades (especulativa)
/// - PY_CuotasDistribucion (especulativa)
/// - PY_VariablesControl (especulativa - existe PY_Variables_Control pero con estructura diferente)
/// - PY_InHomeVisit (especulativa)
/// - PY_Trabajos (especulativa - existe PY_Trabajo)
///
/// Tabla real existente: PY_Trabajo (columnas: id, ProyectoId, OP_MetodologiaId, PresupuestoId, 
///   NombreTrabajo, Muestra, FechaTentativaInicioCampo, etc.)
/// Tabla real existente: PY_Variables_Control (columnas: id, idTrabajo, idEvaluado, tipoEvaluado,
///   cumpleSeguridad, obsSeguridad, cumpleObtencion, etc. - para evaluación, no para variables de control)
///
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.2.1-12.2.3
/// </summary>
namespace MatrixNext.Data.Adapters.PY;

using Dapper;
using MatrixNext.Data.Models.PY;
using Microsoft.Extensions.Logging;
using System.Data;

public class DistribucionAdapter : IDistribucionAdapter
{
    private readonly IDbConnection _connection;
    private readonly ILogger<DistribucionAdapter> _logger;

    public DistribucionAdapter(IDbConnection connection, ILogger<DistribucionAdapter> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    // ===== SPRINT 12.2.1: Distribución de Entrevistas =====
    // NOTA: La tabla PY_DistribucionEntrevistas NO EXISTE - pendiente definir estructura
    
    public Task<List<DistribucionEntrevistaDto>> ObtenerDistribucionesAsync(long idTrabajo)
    {
        // Tabla PY_DistribucionEntrevistas NO EXISTE en la BD
        _logger.LogWarning("ObtenerDistribucionesAsync: Tabla PY_DistribucionEntrevistas no existe en BD");
        throw new NotImplementedException(
            "La tabla PY_DistribucionEntrevistas no existe en la BD CO_Matrix_Intranet. " +
            "Pendiente definir estructura de datos para distribución de entrevistas.");
    }

    public Task<ResumenDistribucionDto> ObtenerResumenAsync(long idTrabajo)
    {
        // Tabla PY_DistribucionEntrevistas y columna TotalMuestra en PY_Trabajos NO EXISTEN
        _logger.LogWarning("ObtenerResumenAsync: Tablas/columnas requeridas no existen en BD");
        throw new NotImplementedException(
            "La tabla PY_DistribucionEntrevistas y columna PY_Trabajos.TotalMuestra no existen. " +
            "La tabla real es PY_Trabajo con columna 'Muestra'.");
    }

    public Task<bool> DistribuirPorUnidadAsync(DistribuirPorUnidadDto distribucion)
    {
        // Tabla PY_DistribucionEntrevistas NO EXISTE
        _logger.LogWarning("DistribuirPorUnidadAsync: Tabla PY_DistribucionEntrevistas no existe");
        throw new NotImplementedException(
            "La tabla PY_DistribucionEntrevistas no existe en la BD. " +
            "Pendiente crear tabla y SP correspondientes.");
    }

    public Task<List<CuotaDistribucionDto>> ObtenerCuotasAsync(long idDistribucion)
    {
        // Tabla PY_CuotasDistribucion NO EXISTE
        _logger.LogWarning("ObtenerCuotasAsync: Tabla PY_CuotasDistribucion no existe");
        throw new NotImplementedException(
            "La tabla PY_CuotasDistribucion no existe en la BD CO_Matrix_Intranet.");
    }

    /// <summary>
    /// Valida que la suma de distribución coincida con la muestra del trabajo.
    /// NOTA: Usa tabla real PY_Trabajo con columna 'Muestra' (no TotalMuestra).
    /// </summary>
    public async Task<bool> ValidarSumaDistribucionAsync(long idTrabajo, int sumaDistribucion)
    {
        try
        {
            // Usar tabla correcta: PY_Trabajo (no PY_Trabajos) y columna 'Muestra' (no TotalMuestra)
            var totalMuestra = await _connection.ExecuteScalarAsync<int?>(
                "SELECT Muestra FROM PY_Trabajo WHERE id = @IdTrabajo",
                new { IdTrabajo = idTrabajo });

            if (!totalMuestra.HasValue)
            {
                _logger.LogWarning("Trabajo {IdTrabajo} no encontrado o sin muestra definida", idTrabajo);
                return false;
            }

            var esValido = sumaDistribucion == totalMuestra.Value;
            _logger.LogInformation("Validación suma distribución: {Suma} == {Total} = {Resultado}",
                sumaDistribucion, totalMuestra.Value, esValido);
            return esValido;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando suma distribución trabajo {IdTrabajo}", idTrabajo);
            return false;
        }
    }

    // ===== SPRINT 12.2.2: Variables de Control =====
    // NOTA: Existe PY_Variables_Control pero es para evaluación/cumplimiento, 
    // NO para definir variables de control del trabajo
    
    public Task<List<VariableControlDto>> ObtenerVariablesControlAsync(long idTrabajo)
    {
        // La tabla PY_VariablesControl (sin guión bajo) NO EXISTE
        // PY_Variables_Control existe pero tiene estructura diferente (para evaluación)
        _logger.LogWarning("ObtenerVariablesControlAsync: Tabla PY_VariablesControl no existe");
        throw new NotImplementedException(
            "La tabla PY_VariablesControl no existe. Existe PY_Variables_Control pero " +
            "tiene estructura diferente (cumpleSeguridad, cumpleObtencion, etc. - es para evaluación).");
    }

    public Task<long> CrearVariableControlAsync(VariableControlDto variable)
    {
        _logger.LogWarning("CrearVariableControlAsync: Tabla PY_VariablesControl no existe");
        throw new NotImplementedException(
            "La tabla PY_VariablesControl no existe en la BD. " +
            "Pendiente definir estructura para variables de control de trabajos.");
    }

    public Task<bool> ActualizarVariableControlAsync(VariableControlDto variable)
    {
        _logger.LogWarning("ActualizarVariableControlAsync: Tabla PY_VariablesControl no existe");
        throw new NotImplementedException("La tabla PY_VariablesControl no existe en la BD.");
    }

    public Task<bool> EliminarVariableControlAsync(long idVariable)
    {
        _logger.LogWarning("EliminarVariableControlAsync: Tabla PY_VariablesControl no existe");
        throw new NotImplementedException("La tabla PY_VariablesControl no existe en la BD.");
    }

    // ===== SPRINT 12.2.3: InHome Visit =====
    // NOTA: La tabla PY_InHomeVisit NO EXISTE en la BD
    
    public Task<List<InHomeVisitDto>> ObtenerInHomeVisitsAsync(long idTrabajo)
    {
        _logger.LogWarning("ObtenerInHomeVisitsAsync: Tabla PY_InHomeVisit no existe");
        throw new NotImplementedException(
            "La tabla PY_InHomeVisit no existe en la BD CO_Matrix_Intranet. " +
            "Pendiente crear tabla para registro de InHome visits.");
    }

    public Task<long> CrearInHomeVisitAsync(InHomeVisitDto visita)
    {
        _logger.LogWarning("CrearInHomeVisitAsync: Tabla PY_InHomeVisit no existe");
        throw new NotImplementedException("La tabla PY_InHomeVisit no existe en la BD.");
    }

    public Task<bool> ActualizarInHomeVisitAsync(InHomeVisitDto visita)
    {
        _logger.LogWarning("ActualizarInHomeVisitAsync: Tabla PY_InHomeVisit no existe");
        throw new NotImplementedException("La tabla PY_InHomeVisit no existe en la BD.");
    }

    public Task<bool> CambiarEstadoVisitaAsync(long idVisita, string nuevoEstado, long usuarioId)
    {
        _logger.LogWarning("CambiarEstadoVisitaAsync: Tabla PY_InHomeVisit no existe");
        throw new NotImplementedException("La tabla PY_InHomeVisit no existe en la BD.");
    }
}
