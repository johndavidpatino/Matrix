using Dapper;
using MatrixNext.Data.Models.OP;
using MatrixNext.Web.Infrastructure.Data;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.OP;

/// <summary>
/// Implementación del adapter para notificaciones en OP
/// Acceso a BD para obtener destinatarios de emails
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.1.5
/// </summary>
public class NotificacionesOpAdapter : INotificacionesOpAdapter
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NotificacionesOpAdapter> _logger;

    public NotificacionesOpAdapter(
        ApplicationDbContext context,
        ILogger<NotificacionesOpAdapter> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene el coordinador asignado al trabajo
    /// </summary>
    public async Task<DestinatarioEmailDto?> ObtenerCoordinadorTrabajoAsync(long idTrabajo)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<DestinatarioEmailDto>(
                @"SELECT 
                    u.IdUsuario,
                    u.NombreCompleto,
                    u.Email AS EmailOrigen,
                    'Coordinador' AS Rol,
                    t.IdUnidad,
                    un.NombreUnidad
                FROM PY_Trabajos t
                LEFT JOIN TH_Usuario u ON t.IdCoordinador = u.IdUsuario
                LEFT JOIN CS_Unidad un ON t.IdUnidad = un.IdUnidad
                WHERE t.IdTrabajo = @IdTrabajo",
                new { IdTrabajo = idTrabajo }
            );

            if (result != null && !string.IsNullOrEmpty(result.EmailOrigen))
            {
                _logger.LogInformation(
                    "Coordinador obtenido para trabajo {IdTrabajo}: {Email}",
                    idTrabajo, result.EmailOrigen);
            }
            else
            {
                _logger.LogWarning("No se encontró coordinador o email para trabajo {IdTrabajo}", idTrabajo);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo coordinador para trabajo {IdTrabajo}", idTrabajo);
            return null;
        }
    }

    /// <summary>
    /// Obtiene usuarios COE (Centro de Operaciones Especializadas) por unidad
    /// </summary>
    public async Task<IEnumerable<DestinatarioEmailDto>> ObtenerCoeUnidadAsync(long? idUnidad = null)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@IdUnidad", idUnidad);
            parameters.Add("@Rol", "COE");

            // Buscar usuarios con rol COE en TH_UsuarioRol o similar
            var result = await connection.QueryAsync<DestinatarioEmailDto>(
                @"SELECT 
                    u.IdUsuario,
                    u.NombreCompleto,
                    u.Email AS EmailOrigen,
                    'COE' AS Rol,
                    u.IdUnidad,
                    un.NombreUnidad
                FROM TH_Usuario u
                INNER JOIN TH_UsuarioRol ur ON u.IdUsuario = ur.IdUsuario
                LEFT JOIN CS_Unidad un ON u.IdUnidad = un.IdUnidad
                WHERE ur.NombreRol LIKE '%COE%'
                  AND (@IdUnidad IS NULL OR u.IdUnidad = @IdUnidad)
                  AND u.Activo = 1",
                parameters
            );

            _logger.LogInformation(
                "Usuarios COE obtenidos. Unidad: {IdUnidad}, Total: {Total}",
                idUnidad, result.Count());

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo COEs. Unidad: {IdUnidad}", idUnidad);
            return Enumerable.Empty<DestinatarioEmailDto>();
        }
    }

    /// <summary>
    /// Obtiene el PMO (Project Manager Office) del trabajo
    /// </summary>
    public async Task<DestinatarioEmailDto?> ObtenerPmoTrabajoAsync(long idTrabajo)
    {
        try
        {
            using var connection = _context.CreateConnection();

            var result = await connection.QueryFirstOrDefaultAsync<DestinatarioEmailDto>(
                @"SELECT 
                    u.IdUsuario,
                    u.NombreCompleto,
                    u.Email AS EmailOrigen,
                    'PMO' AS Rol,
                    p.IdUnidad,
                    un.NombreUnidad
                FROM PY_Trabajos t
                INNER JOIN PY_Proyectos p ON t.IdProyecto = p.IdProyecto
                LEFT JOIN TH_Usuario u ON p.IdPmo = u.IdUsuario
                LEFT JOIN CS_Unidad un ON p.IdUnidad = un.IdUnidad
                WHERE t.IdTrabajo = @IdTrabajo",
                new { IdTrabajo = idTrabajo }
            );

            if (result != null && !string.IsNullOrEmpty(result.EmailOrigen))
            {
                _logger.LogInformation(
                    "PMO obtenido para trabajo {IdTrabajo}: {Email}",
                    idTrabajo, result.EmailOrigen);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo PMO para trabajo {IdTrabajo}", idTrabajo);
            return null;
        }
    }

    /// <summary>
    /// Obtiene todos los destinatarios recomendados (Coordinador + COE + PMO)
    /// Elimina duplicados por email
    /// </summary>
    public async Task<IEnumerable<DestinatarioEmailDto>> ObtenerDestinatariosAsync(long idTrabajo)
    {
        try
        {
            var destinatarios = new List<DestinatarioEmailDto>();

            // Obtener coordinador
            var coordinador = await ObtenerCoordinadorTrabajoAsync(idTrabajo);
            if (coordinador != null && !string.IsNullOrEmpty(coordinador.EmailOrigen))
            {
                destinatarios.Add(coordinador);
            }

            // Obtener unidad del trabajo para filtrar COE
            long? idUnidad = null;
            if (coordinador != null)
            {
                idUnidad = coordinador.IdUnidad;
            }

            // Obtener COEs
            var coes = await ObtenerCoeUnidadAsync(idUnidad);
            destinatarios.AddRange(coes);

            // Obtener PMO
            var pmo = await ObtenerPmoTrabajoAsync(idTrabajo);
            if (pmo != null && !string.IsNullOrEmpty(pmo.EmailOrigen))
            {
                destinatarios.Add(pmo);
            }

            // Eliminar duplicados por email
            var destinatarosUnicos = destinatarios
                .Where(d => !string.IsNullOrEmpty(d.EmailOrigen))
                .DistinctBy(d => d.EmailOrigen)
                .ToList();

            _logger.LogInformation(
                "Destinatarios obtenidos para trabajo {IdTrabajo}. Total: {Total}",
                idTrabajo, destinatarosUnicos.Count);

            return destinatarosUnicos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo destinatarios para trabajo {IdTrabajo}", idTrabajo);
            return Enumerable.Empty<DestinatarioEmailDto>();
        }
    }
}
