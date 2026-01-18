using Dapper;
using MatrixNext.Data.Models.IT;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.IT;

public interface IITSyncAdapter
{
    Task<IEnumerable<SyncPreguntaDto>> ObtenerPreguntasAsync(long? trabajoId, decimal? sbjNum);
    Task ActualizarPreguntaAsync(decimal sbjNum, string dcp, string valor, decimal eId);
    Task<decimal?> ObtenerIdRegistroRespuestaAsync(decimal eId, decimal numeroEncuesta);
    Task QuitarPreguntasEntrenamientoAsync(long trabajoId);
    Task ErrorTrabajoEspecializadoAsync(long trabajoId);
    Task HabilitarSincronizacionAsync(long trabajoId);
    Task HabilitarEncuestaPilotoAsync(decimal sbjNum);
    Task EncuestaPilotoAsync(decimal sbjNum);
    Task GrabarAuditoriaAsync(decimal usuarioId, short tipoAccion, short modulo, string descripcion, DateTime fecha, decimal idRegistro, short tabla);
}

/// <summary>
/// Adapter para sincronización IT
/// NOTA: Todos los SP de este módulo NO EXISTEN en la BD legacy (CO_Matrix_Intranet)
/// Los métodos retornan valores vacíos/default hasta que se creen los SP correspondientes
/// </summary>
public class ITSyncAdapter : IITSyncAdapter
{
    private readonly IDbConnection _connection;
    private readonly ILogger<ITSyncAdapter> _logger;

    public ITSyncAdapter(IDbConnection connection, ILogger<ITSyncAdapter> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    /// <summary>
    /// STUB: SP Sync_Preguntas_Get no existe en BD legacy
    /// </summary>
    public Task<IEnumerable<SyncPreguntaDto>> ObtenerPreguntasAsync(long? trabajoId, decimal? sbjNum)
    {
        _logger.LogWarning("[IT] ObtenerPreguntasAsync: SP 'Sync_Preguntas_Get' no existe en BD legacy. Retornando lista vacía. TrabajoId={TrabajoId}, SbjNum={SbjNum}", trabajoId, sbjNum);
        return Task.FromResult<IEnumerable<SyncPreguntaDto>>(new List<SyncPreguntaDto>());
    }

    /// <summary>
    /// STUB: SP Sync_Preguntas_UpdateInfo no existe en BD legacy
    /// </summary>
    public Task ActualizarPreguntaAsync(decimal sbjNum, string dcp, string valor, decimal eId)
    {
        _logger.LogWarning("[IT] ActualizarPreguntaAsync: SP 'Sync_Preguntas_UpdateInfo' no existe en BD legacy. Operación ignorada. SbjNum={SbjNum}, DCP={DCP}, EId={EId}", sbjNum, dcp, eId);
        return Task.CompletedTask;
    }

    /// <summary>
    /// STUB: SP obtenerRespuestaIdRegistroXIdTrabajoNumeroEncuesta no existe en BD legacy
    /// </summary>
    public Task<decimal?> ObtenerIdRegistroRespuestaAsync(decimal eId, decimal numeroEncuesta)
    {
        _logger.LogWarning("[IT] ObtenerIdRegistroRespuestaAsync: SP 'obtenerRespuestaIdRegistroXIdTrabajoNumeroEncuesta' no existe en BD legacy. Retornando null. EId={EId}, NumeroEncuesta={NumeroEncuesta}", eId, numeroEncuesta);
        return Task.FromResult<decimal?>(null);
    }

    /// <summary>
    /// STUB: SP Sync_EncuestasEntrenamiento no existe en BD legacy
    /// </summary>
    public Task QuitarPreguntasEntrenamientoAsync(long trabajoId)
    {
        _logger.LogWarning("[IT] QuitarPreguntasEntrenamientoAsync: SP 'Sync_EncuestasEntrenamiento' no existe en BD legacy. Operación ignorada. TrabajoId={TrabajoId}", trabajoId);
        return Task.CompletedTask;
    }

    /// <summary>
    /// STUB: SP Sync_ErrorTrabajoEspecializado no existe en BD legacy
    /// </summary>
    public Task ErrorTrabajoEspecializadoAsync(long trabajoId)
    {
        _logger.LogWarning("[IT] ErrorTrabajoEspecializadoAsync: SP 'Sync_ErrorTrabajoEspecializado' no existe en BD legacy. Operación ignorada. TrabajoId={TrabajoId}", trabajoId);
        return Task.CompletedTask;
    }

    /// <summary>
    /// STUB: SP Sync_HabilitarSincronizacionEstudio no existe en BD legacy
    /// </summary>
    public Task HabilitarSincronizacionAsync(long trabajoId)
    {
        _logger.LogWarning("[IT] HabilitarSincronizacionAsync: SP 'Sync_HabilitarSincronizacionEstudio' no existe en BD legacy. Operación ignorada. TrabajoId={TrabajoId}", trabajoId);
        return Task.CompletedTask;
    }

    /// <summary>
    /// STUB: SP Sync_HabilitarEncuestasPiloto no existe en BD legacy
    /// </summary>
    public Task HabilitarEncuestaPilotoAsync(decimal sbjNum)
    {
        _logger.LogWarning("[IT] HabilitarEncuestaPilotoAsync: SP 'Sync_HabilitarEncuestasPiloto' no existe en BD legacy. Operación ignorada. SbjNum={SbjNum}", sbjNum);
        return Task.CompletedTask;
    }

    /// <summary>
    /// STUB: SP Sync_EncuestaPiloto no existe en BD legacy
    /// </summary>
    public Task EncuestaPilotoAsync(decimal sbjNum)
    {
        _logger.LogWarning("[IT] EncuestaPilotoAsync: SP 'Sync_EncuestaPiloto' no existe en BD legacy. Operación ignorada. SbjNum={SbjNum}", sbjNum);
        return Task.CompletedTask;
    }

    /// <summary>
    /// STUB: SP GrabarAuditoria no existe en BD legacy
    /// </summary>
    public Task GrabarAuditoriaAsync(decimal usuarioId, short tipoAccion, short modulo, string descripcion, DateTime fecha, decimal idRegistro, short tabla)
    {
        _logger.LogWarning("[IT] GrabarAuditoriaAsync: SP 'GrabarAuditoria' no existe en BD legacy. Operación ignorada. UsuarioId={UsuarioId}, TipoAccion={TipoAccion}, Modulo={Modulo}", usuarioId, tipoAccion, modulo);
        return Task.CompletedTask;
    }
}
