using Dapper;
using MatrixNext.Data.Models.IT;
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

public class ITSyncAdapter : IITSyncAdapter
{
    private readonly IDbConnection _connection;

    public ITSyncAdapter(IDbConnection connection)
    {
        _connection = connection;
    }

    public async Task<IEnumerable<SyncPreguntaDto>> ObtenerPreguntasAsync(long? trabajoId, decimal? sbjNum)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TrabajoId", trabajoId, DbType.Int64);
        parameters.Add("@SbjNum", sbjNum, DbType.Decimal);

        return await _connection.QueryAsync<SyncPreguntaDto>(
            "Sync_Preguntas_Get",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task ActualizarPreguntaAsync(decimal sbjNum, string dcp, string valor, decimal eId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@SbjNum", sbjNum, DbType.Decimal);
        parameters.Add("@DCP", dcp, DbType.String, size: 50);
        parameters.Add("@valor", valor, DbType.String);
        parameters.Add("@e_Id", eId, DbType.Decimal);

        await _connection.ExecuteAsync(
            "Sync_Preguntas_UpdateInfo",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task<decimal?> ObtenerIdRegistroRespuestaAsync(decimal eId, decimal numeroEncuesta)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@E_Id", eId, DbType.Decimal);
        parameters.Add("@numeroEncuesta", numeroEncuesta, DbType.Decimal);

        var result = await _connection.QueryFirstOrDefaultAsync<decimal?>(
            "obtenerRespuestaIdRegistroXIdTrabajoNumeroEncuesta",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return result;
    }

    public async Task QuitarPreguntasEntrenamientoAsync(long trabajoId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TrabajoId", trabajoId, DbType.Int64);

        await _connection.ExecuteAsync(
            "Sync_EncuestasEntrenamiento",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task ErrorTrabajoEspecializadoAsync(long trabajoId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TrabajoId", trabajoId, DbType.Int64);

        await _connection.ExecuteAsync(
            "Sync_ErrorTrabajoEspecializado",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task HabilitarSincronizacionAsync(long trabajoId)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@TrabajoId", trabajoId, DbType.Int64);

        await _connection.ExecuteAsync(
            "Sync_HabilitarSincronizacionEstudio",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task HabilitarEncuestaPilotoAsync(decimal sbjNum)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@sbjNum", sbjNum, DbType.Decimal);

        await _connection.ExecuteAsync(
            "Sync_HabilitarEncuestasPiloto",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task EncuestaPilotoAsync(decimal sbjNum)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@sbjNum", sbjNum, DbType.Decimal);

        await _connection.ExecuteAsync(
            "Sync_EncuestaPiloto",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }

    public async Task GrabarAuditoriaAsync(decimal usuarioId, short tipoAccion, short modulo, string descripcion, DateTime fecha, decimal idRegistro, short tabla)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@A_Id", dbType: DbType.Decimal, direction: ParameterDirection.InputOutput);
        parameters.Add("@Usu_Id", usuarioId, DbType.Decimal);
        parameters.Add("@TA_Id", tipoAccion, DbType.Int16);
        parameters.Add("@Mod_Id", modulo, DbType.Int16);
        parameters.Add("@A_Descripcion", descripcion, DbType.String);
        parameters.Add("@A_Fecha", fecha, DbType.DateTime);
        parameters.Add("@Id_Reg", idRegistro, DbType.Decimal);
        parameters.Add("@T_Id", tabla, DbType.Int16);

        await _connection.ExecuteAsync(
            "GrabarAuditoria",
            parameters,
            commandType: CommandType.StoredProcedure
        );
    }
}
