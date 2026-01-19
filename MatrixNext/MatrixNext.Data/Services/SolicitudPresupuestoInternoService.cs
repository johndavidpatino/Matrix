using Dapper;
using MatrixNext.Data.Dtos;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Services;

/// <summary>
/// Servicio para gestión de solicitudes de presupuestos internos de trabajo
/// </summary>
public class SolicitudPresupuestoInternoService : ISolicitudPresupuestoInternoService
{
    private readonly string _connectionString;
    private readonly ILogger<SolicitudPresupuestoInternoService> _logger;

    public SolicitudPresupuestoInternoService(
        string connectionString,
        ILogger<SolicitudPresupuestoInternoService> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene ViewModel con información del trabajo para crear solicitud
    /// </summary>
    public async Task<SolicitudPresupuestoViewModel> PrepararSolicitudAsync(long trabajoId)
    {
        using var connection = new SqlConnection(_connectionString);
        
        // Obtener información del trabajo
        var sql = @"
            SELECT TOP 1
                t.IdTrabajo AS TrabajoId,
                t.JobBook,
                t.NombreTrabajo,
                m.Nombre AS Metodologia
            FROM PY_Trabajo t
            INNER JOIN PY_Metodologia m ON t.IdMetodologia = m.IdMetodologia
            WHERE t.IdTrabajo = @TrabajoId";
        
        var trabajo = await connection.QueryFirstOrDefaultAsync<dynamic>(sql, new { TrabajoId = trabajoId });
        
        if (trabajo == null)
        {
            _logger.LogWarning("Trabajo {TrabajoId} no encontrado", trabajoId);
            return new SolicitudPresupuestoViewModel
            {
                TrabajoId = trabajoId,
                JobBook = "N/A",
                NombreTrabajo = "Trabajo no encontrado",
                YaSolicitado = false
            };
        }
        
        // Verificar si ya existe solicitud
        var solicitudExistente = await ObtenerSolicitudPorTrabajoAsync(trabajoId);
        
        return new SolicitudPresupuestoViewModel
        {
            TrabajoId = trabajo.TrabajoId,
            JobBook = trabajo.JobBook ?? "N/A",
            NombreTrabajo = trabajo.NombreTrabajo ?? "Sin nombre",
            Metodologia = trabajo.Metodologia ?? "N/A",
            YaSolicitado = solicitudExistente != null,
            SolicitudExistente = solicitudExistente
        };
    }

    /// <summary>
    /// Crea nueva solicitud de presupuesto interno
    /// </summary>
    public async Task<(bool success, string message)> CrearSolicitudAsync(long trabajoId, string observacion, long usuarioId)
    {
        using var connection = new SqlConnection(_connectionString);
        
        try
        {
            // Validar que no exista solicitud previa
            var solicitudExistente = await ObtenerSolicitudPorTrabajoAsync(trabajoId);
            if (solicitudExistente != null)
            {
                _logger.LogWarning("Intento de crear solicitud duplicada para trabajo {TrabajoId} por usuario {UsuarioId}", 
                    trabajoId, usuarioId);
                return (false, "Ya existe una solicitud de presupuesto para este trabajo");
            }
            
            // Validar observación
            if (string.IsNullOrWhiteSpace(observacion))
            {
                return (false, "Debe escribir las especificaciones para la generación del presupuesto");
            }
            
            // Ejecutar SP para guardar solicitud
            var parameters = new DynamicParameters();
            parameters.Add("@Usuario", usuarioId);
            parameters.Add("@Fecha", DateTime.Now);
            parameters.Add("@TrabajoId", trabajoId);
            parameters.Add("@Observacion", observacion);
            
            await connection.ExecuteAsync(
                "CC_SolicitudPresupuestoInternoAdd",
                parameters,
                commandType: CommandType.StoredProcedure
            );
            
            _logger.LogInformation(
                "Solicitud de presupuesto creada para trabajo {TrabajoId} por usuario {UsuarioId}. Observacion: {Obs}",
                trabajoId, usuarioId, observacion);
            
            // TODO: Aquí se debería enviar email de notificación
            // await EnviarEmailSolicitudAsync(trabajoId);
            
            return (true, "Solicitud de presupuesto creada exitosamente");
        }
        catch (SqlException ex)
        {
            _logger.LogError(ex, "Error de BD al crear solicitud de presupuesto. TrabajoId: {TrabajoId}, UsuarioId: {UsuarioId}", 
                trabajoId, usuarioId);
            return (false, "Error al crear la solicitud. Por favor intente nuevamente.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al crear solicitud de presupuesto. TrabajoId: {TrabajoId}, UsuarioId: {UsuarioId}", 
                trabajoId, usuarioId);
            return (false, "Error inesperado. Contacte al administrador.");
        }
    }

    /// <summary>
    /// Obtiene solicitud existente para un trabajo
    /// </summary>
    public async Task<SolicitudPresupuestoInternoDto?> ObtenerSolicitudPorTrabajoAsync(long trabajoId)
    {
        using var connection = new SqlConnection(_connectionString);
        
        var parameters = new DynamicParameters();
        parameters.Add("@TrabajoId", trabajoId);
        
        var resultado = await connection.QueryAsync<SolicitudPresupuestoInternoDto>(
            "CC_SolicitudPresupuestoGet",
            parameters,
            commandType: CommandType.StoredProcedure
        );
        
        return resultado.FirstOrDefault();
    }
}
