using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.OP.Models;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio para gestión de trabajos OP Cuantitativo (Portal COE)
/// </summary>
/// <remarks>
/// Implementa funcionalidades de Trabajos.aspx.vb del legado WebMatrix
/// </remarks>
public class OpTrabajosService : IOpTrabajosService
{
    private readonly MatrixDbContext _db;
    private readonly ILogger<OpTrabajosService> _logger;

    public OpTrabajosService(MatrixDbContext db, ILogger<OpTrabajosService> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la configuración de un trabajo OP
    /// </summary>
    public async Task<TrabajoOpConfiguracion?> ObtenerConfiguracionAsync(long trabajoId)
    {
        try
        {
            // TODO: Implementar consulta a tabla OP_TrabajoConfiguracion
            // Por ahora retornamos null si no existe
            // Esto debe mapear al SP TrabajoOPCuanti.ObtenerTrabajoConfiguracion
            
            _logger.LogInformation("Obteniendo configuración para trabajo {TrabajoId}", trabajoId);
            
            // Query directa a la tabla (ajustar según estructura real de BD)
            var config = await _db.Database
                .SqlQueryRaw<TrabajoOpConfiguracion>(
                    "SELECT IdTrabajo, TipoRecoleccionId, FechaCreacion, CreadoPor, FechaActualizacion, ActualizadoPor " +
                    "FROM OP_TrabajoConfiguracion WHERE IdTrabajo = {0}", 
                    trabajoId)
                .FirstOrDefaultAsync();

            return config;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se encontró configuración para trabajo {TrabajoId}", trabajoId);
            return null;
        }
    }

    /// <summary>
    /// Guarda o actualiza la configuración de un trabajo OP
    /// </summary>
    public async Task<bool> GuardarConfiguracionAsync(long trabajoId, short tipoRecoleccionId, long usuarioId)
    {
        try
        {
            _logger.LogInformation("Guardando configuración para trabajo {TrabajoId}", trabajoId);

            // Verificar si existe configuración
            var existente = await ObtenerConfiguracionAsync(trabajoId);

            if (existente == null)
            {
                // INSERT
                await _db.Database.ExecuteSqlRawAsync(
                    "INSERT INTO OP_TrabajoConfiguracion (IdTrabajo, TipoRecoleccionId, FechaCreacion, CreadoPor) " +
                    "VALUES ({0}, {1}, GETDATE(), {2})",
                    trabajoId, tipoRecoleccionId, usuarioId);
            }
            else
            {
                // UPDATE
                await _db.Database.ExecuteSqlRawAsync(
                    "UPDATE OP_TrabajoConfiguracion " +
                    "SET TipoRecoleccionId = {0}, FechaActualizacion = GETDATE(), ActualizadoPor = {1} " +
                    "WHERE IdTrabajo = {2}",
                    tipoRecoleccionId, usuarioId, trabajoId);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al guardar configuración para trabajo {TrabajoId}", trabajoId);
            return false;
        }
    }

    /// <summary>
    /// Valida si un trabajo está en estado cerrado (10) o anulado (11)
    /// </summary>
    public async Task<bool> EstaTrabajoBloquadoAsync(long trabajoId)
    {
        try
        {
            var trabajo = await _db.Trabajos
                .AsNoTracking()
                .Where(t => t.Id == trabajoId)
                .Select(t => new { t.Estado })
                .FirstOrDefaultAsync();

            if (trabajo == null)
            {
                return true; // Si no existe, considerarlo bloqueado
            }

            // Estados de bloqueo: Cerrado=10, Anulado=11
            return trabajo.Estado == 10 || trabajo.Estado == 11;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar estado de trabajo {TrabajoId}", trabajoId);
            return true; // En caso de error, bloquear por seguridad
        }
    }

    /// <summary>
    /// Obtiene el ID de la ficha cuantitativa asociada a un trabajo
    /// </summary>
    public async Task<long?> ObtenerIdFichaCuantitativaAsync(long trabajoId)
    {
        try
        {
            // Mapea a FichaCuantitativo.DevolverxTrabajoID
            var result = await _db.Database
                .SqlQueryRaw<long?>(
                    "SELECT TOP 1 id FROM FichaCuantitativo WHERE IdTrabajo = {0}",
                    trabajoId)
                .FirstOrDefaultAsync();

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "No se encontró ficha cuantitativa para trabajo {TrabajoId}", trabajoId);
            return null;
        }
    }

    /// <summary>
    /// Verifica si existe estimación de producción para un trabajo
    /// </summary>
    public async Task<bool> TieneEstimacionAsync(long trabajoId)
    {
        try
        {
            // Mapea a PlaneacionProduccion.ObtenerEstimacionCiudadxTrabajoList
            var count = await _db.Database
                .SqlQueryRaw<int>(
                    "SELECT COUNT(*) as Value FROM PlaneacionProduccion_EstimacionCiudad WHERE IdTrabajo = {0}",
                    trabajoId)
                .FirstOrDefaultAsync();

            return count > 0;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error al verificar estimación para trabajo {TrabajoId}", trabajoId);
            return false;
        }
    }
}
