using MatrixNext.Data.Models.IT;
using MatrixNext.Data.Adapters.IT;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MatrixNext.Data.Services.IT;

public interface IITSyncService
{
    Task<IEnumerable<SyncPreguntaDto>> ObtenerPreguntasAsync(long trabajoId, decimal? sbjNum = null);
    Task<(bool success, string message)> ActualizarRespuestaAsync(SyncActualizarRespuestaDto dto, int usuarioId);
    Task<(bool success, string message)> QuitarPreguntasEntrenamientoAsync(long trabajoId, int usuarioId);
    Task<(bool success, string message)> ErrorTrabajoEspecializadoAsync(long trabajoId, int usuarioId);
    Task<(bool success, string message)> HabilitarSincronizacionAsync(long trabajoId, int usuarioId);
    Task<(bool success, string message)> HabilitarEncuestaPilotoAsync(decimal sbjNum, int usuarioId);
    Task<(bool success, string message)> EncuestaPilotoAsync(decimal sbjNum, int usuarioId);
}

public class ITSyncService : IITSyncService
{
    private readonly IITSyncAdapter _adapter;
    private readonly ILogger<ITSyncService> _logger;

    // Enums para auditoría (mapeados desde CoreProject)
    private const short TipoAccionActualizado = 2;
    private const short ModuloMatrixSoftSynActualizacionDatos = 6;
    private const short TablaRespuestas = 1;

    public ITSyncService(IITSyncAdapter adapter, ILogger<ITSyncService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }

    public async Task<IEnumerable<SyncPreguntaDto>> ObtenerPreguntasAsync(long trabajoId, decimal? sbjNum = null)
    {
        try
        {
            _logger.LogInformation("Obteniendo preguntas para trabajo {TrabajoId}, SbjNum {SbjNum}", trabajoId, sbjNum);
            return await _adapter.ObtenerPreguntasAsync(trabajoId, sbjNum);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo preguntas para trabajo {TrabajoId}", trabajoId);
            throw;
        }
    }

    public async Task<(bool success, string message)> ActualizarRespuestaAsync(SyncActualizarRespuestaDto dto, int usuarioId)
    {
        try
        {
            // Validar formato de fecha si el campo es Res_Fecha
            var valorFinal = dto.NuevoValor;
            if (dto.DCP.Equals("Res_Fecha", StringComparison.OrdinalIgnoreCase))
            {
                valorFinal = ConvertirFormatoFecha(dto.NuevoValor);
                if (string.IsNullOrEmpty(valorFinal))
                {
                    return (false, "El formato de fecha no es correcto. Use DD/MM/YYYY");
                }
            }

            // Actualizar pregunta
            await _adapter.ActualizarPreguntaAsync(dto.SbjNum, dto.DCP, valorFinal, dto.TrabajoId);

            // Obtener ID de registro para auditoría
            var idRegistro = await _adapter.ObtenerIdRegistroRespuestaAsync(dto.TrabajoId, dto.SbjNum);
            
            if (idRegistro.HasValue)
            {
                // Grabar auditoría
                await _adapter.GrabarAuditoriaAsync(
                    usuarioId,
                    TipoAccionActualizado,
                    ModuloMatrixSoftSynActualizacionDatos,
                    $"El nuevo valor es {dto.NuevoValor}",
                    DateTime.UtcNow.AddHours(-5), // Hora Colombia (UTC-5)
                    idRegistro.Value,
                    TablaRespuestas
                );
            }

            _logger.LogInformation(
                "Respuesta actualizada exitosamente. TrabajoId: {TrabajoId}, SbjNum: {SbjNum}, DCP: {DCP}, Usuario: {UsuarioId}",
                dto.TrabajoId, dto.SbjNum, dto.DCP, usuarioId
            );

            return (true, "Cambio realizado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando respuesta. TrabajoId: {TrabajoId}, SbjNum: {SbjNum}", dto.TrabajoId, dto.SbjNum);
            return (false, "Error al actualizar la respuesta");
        }
    }

    public async Task<(bool success, string message)> QuitarPreguntasEntrenamientoAsync(long trabajoId, int usuarioId)
    {
        try
        {
            await _adapter.QuitarPreguntasEntrenamientoAsync(trabajoId);
            
            _logger.LogInformation(
                "Preguntas de entrenamiento quitadas del trabajo {TrabajoId} por usuario {UsuarioId}",
                trabajoId, usuarioId
            );

            return (true, "Cambio realizado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error quitando preguntas de entrenamiento. TrabajoId: {TrabajoId}", trabajoId);
            return (false, "Error al quitar preguntas de entrenamiento");
        }
    }

    public async Task<(bool success, string message)> ErrorTrabajoEspecializadoAsync(long trabajoId, int usuarioId)
    {
        try
        {
            await _adapter.ErrorTrabajoEspecializadoAsync(trabajoId);
            
            _logger.LogInformation(
                "Supervisión removida del trabajo especializado {TrabajoId} por usuario {UsuarioId}",
                trabajoId, usuarioId
            );

            return (true, "Cambio realizado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removiendo supervisión. TrabajoId: {TrabajoId}", trabajoId);
            return (false, "Error al remover supervisión del trabajo");
        }
    }

    public async Task<(bool success, string message)> HabilitarSincronizacionAsync(long trabajoId, int usuarioId)
    {
        try
        {
            await _adapter.HabilitarSincronizacionAsync(trabajoId);
            
            _logger.LogInformation(
                "Sincronización habilitada para trabajo {TrabajoId} por usuario {UsuarioId}",
                trabajoId, usuarioId
            );

            return (true, "Cambio realizado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error habilitando sincronización. TrabajoId: {TrabajoId}", trabajoId);
            return (false, "Error al habilitar sincronización");
        }
    }

    public async Task<(bool success, string message)> HabilitarEncuestaPilotoAsync(decimal sbjNum, int usuarioId)
    {
        try
        {
            await _adapter.HabilitarEncuestaPilotoAsync(sbjNum);
            
            _logger.LogInformation(
                "Encuesta piloto habilitada para SbjNum {SbjNum} por usuario {UsuarioId}",
                sbjNum, usuarioId
            );

            return (true, "Cambio realizado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error habilitando encuesta piloto. SbjNum: {SbjNum}", sbjNum);
            return (false, "Error al habilitar encuesta piloto");
        }
    }

    public async Task<(bool success, string message)> EncuestaPilotoAsync(decimal sbjNum, int usuarioId)
    {
        try
        {
            await _adapter.EncuestaPilotoAsync(sbjNum);
            
            _logger.LogInformation(
                "Encuesta marcada como piloto. SbjNum {SbjNum} por usuario {UsuarioId}",
                sbjNum, usuarioId
            );

            return (true, "Cambio realizado");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marcando encuesta como piloto. SbjNum: {SbjNum}", sbjNum);
            return (false, "Error al marcar encuesta como piloto");
        }
    }

    /// <summary>
    /// Convierte formato de fecha DD/MM/YYYY a MM/DD/YYYY
    /// </summary>
    private string ConvertirFormatoFecha(string fecha)
    {
        try
        {
            // Validar formato DD/MM/YYYY
            var regex = new Regex(@"^(\d{1,2})/(\d{1,2})/(\d{4})$");
            var match = regex.Match(fecha);

            if (!match.Success)
            {
                return string.Empty;
            }

            var dia = match.Groups[1].Value;
            var mes = match.Groups[2].Value;
            var anio = match.Groups[3].Value;

            // Validar que sea una fecha válida
            if (!DateTime.TryParseExact(
                fecha, 
                "dd/MM/yyyy", 
                CultureInfo.InvariantCulture, 
                DateTimeStyles.None, 
                out var fechaValida))
            {
                return string.Empty;
            }

            // Retornar en formato MM/DD/YYYY
            return $"{mes}/{dia}/{anio}";
        }
        catch
        {
            return string.Empty;
        }
    }
}
