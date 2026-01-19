using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.US.Feedback.Adapters;

/// <summary>
/// Adaptador para Feedback
/// SP: CORE_Asunto_Get, CORE_Feedback_Add
/// Tabla: CORE_Retroalimentacion
/// Ref: CoreProject/Clases/CORE/Feedback.vb
/// </summary>
public interface IFeedbackAdapter
{
    Task<IEnumerable<Models.AsuntoDto>> ObtenerAsuntosAsync();
    Task EnviarFeedbackAsync(long idUsuario, int tipoMensaje, string mensaje);
    Task<IEnumerable<Models.FeedbackDto>> ObtenerFeedbackPendientesAsync();
    Task<IEnumerable<Models.FeedbackDto>> ObtenerFeedbackResueltosAsync();
    Task<Models.FeedbackDto?> ObtenerPorIdAsync(long id);
    Task ActualizarFeedbackAsync(long id, string? respuesta, bool solucionado, long usuarioResponde);
}

public class FeedbackAdapter : IFeedbackAdapter
{
    private readonly string _connectionString;
    private readonly ILogger<FeedbackAdapter> _logger;

    public FeedbackAdapter(string connectionString, ILogger<FeedbackAdapter> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<IEnumerable<Models.AsuntoDto>> ObtenerAsuntosAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<Models.AsuntoDto>(
                "CORE_Asunto_Get",
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener asuntos de feedback");
            throw;
        }
    }

    public async Task EnviarFeedbackAsync(long idUsuario, int tipoMensaje, string mensaje)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(
                "CORE_Feedback_Add",
                new { Usuario = idUsuario, Tipo_Mensaje = tipoMensaje, Mensaje = mensaje },
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Feedback enviado. Usuario: {Usuario}, Tipo: {Tipo}", idUsuario, tipoMensaje);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar feedback. Usuario: {Usuario}", idUsuario);
            throw;
        }
    }

    public async Task<IEnumerable<Models.FeedbackDto>> ObtenerFeedbackPendientesAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<Models.FeedbackDto>(
                @"SELECT r.id as Id, r.Tipo_Mensaje as TipoMensaje, a.tipo as TipoMensajeNombre,
                         r.Mensaje, r.Usuario as IdUsuarioEnvia, u.Usuario as NombreUsuarioEnvia,
                         r.Fecha as FechaEnvio, r.Solucionado, r.Respuesta,
                         r.UsuarioResponde, ur.Usuario as NombreUsuarioResponde, r.FechaSolucion
                  FROM CORE_Retroalimentacion r
                  LEFT JOIN CORE_Asunto a ON r.Tipo_Mensaje = a.id
                  LEFT JOIN US_Usuario u ON r.Usuario = u.Id
                  LEFT JOIN US_Usuario ur ON r.UsuarioResponde = ur.Id
                  WHERE r.Solucionado = 0 OR r.Solucionado IS NULL
                  ORDER BY r.Fecha DESC"
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener feedback pendientes");
            throw;
        }
    }

    public async Task<IEnumerable<Models.FeedbackDto>> ObtenerFeedbackResueltosAsync()
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryAsync<Models.FeedbackDto>(
                @"SELECT r.id as Id, r.Tipo_Mensaje as TipoMensaje, a.tipo as TipoMensajeNombre,
                         r.Mensaje, r.Usuario as IdUsuarioEnvia, u.Usuario as NombreUsuarioEnvia,
                         r.Fecha as FechaEnvio, r.Solucionado, r.Respuesta,
                         r.UsuarioResponde, ur.Usuario as NombreUsuarioResponde, r.FechaSolucion
                  FROM CORE_Retroalimentacion r
                  LEFT JOIN CORE_Asunto a ON r.Tipo_Mensaje = a.id
                  LEFT JOIN US_Usuario u ON r.Usuario = u.Id
                  LEFT JOIN US_Usuario ur ON r.UsuarioResponde = ur.Id
                  WHERE r.Solucionado = 1
                  ORDER BY r.FechaSolucion DESC"
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener feedback resueltos");
            throw;
        }
    }

    public async Task<Models.FeedbackDto?> ObtenerPorIdAsync(long id)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var result = await connection.QueryFirstOrDefaultAsync<Models.FeedbackDto>(
                @"SELECT r.id as Id, r.Tipo_Mensaje as TipoMensaje, a.tipo as TipoMensajeNombre,
                         r.Mensaje, r.Usuario as IdUsuarioEnvia, u.Usuario as NombreUsuarioEnvia,
                         r.Fecha as FechaEnvio, r.Solucionado, r.Respuesta,
                         r.UsuarioResponde, ur.Usuario as NombreUsuarioResponde, r.FechaSolucion
                  FROM CORE_Retroalimentacion r
                  LEFT JOIN CORE_Asunto a ON r.Tipo_Mensaje = a.id
                  LEFT JOIN US_Usuario u ON r.Usuario = u.Id
                  LEFT JOIN US_Usuario ur ON r.UsuarioResponde = ur.Id
                  WHERE r.id = @Id",
                new { Id = id }
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener feedback por Id: {Id}", id);
            throw;
        }
    }

    public async Task ActualizarFeedbackAsync(long id, string? respuesta, bool solucionado, long usuarioResponde)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await connection.ExecuteAsync(
                @"UPDATE CORE_Retroalimentacion 
                  SET Respuesta = @Respuesta, 
                      Solucionado = @Solucionado, 
                      UsuarioResponde = @UsuarioResponde,
                      FechaSolucion = CASE WHEN @Solucionado = 1 THEN GETDATE() ELSE FechaSolucion END
                  WHERE id = @Id",
                new { Id = id, Respuesta = respuesta, Solucionado = solucionado, UsuarioResponde = usuarioResponde }
            );

            _logger.LogInformation("Feedback actualizado. Id: {Id}, Solucionado: {Solucionado}", id, solucionado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar feedback. Id: {Id}", id);
            throw;
        }
    }
}
