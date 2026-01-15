using Dapper;
using MatrixNext.Data.Models.MBO;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System.Data;

namespace MatrixNext.Data.Adapters.MBO;

/// <summary>
/// Adapter para acceso a datos del módulo Campo (MBO)
/// </summary>
public class CampoAdapter : ICampoAdapter
{
    private readonly IDbConnection _connection;
    private readonly ILogger<CampoAdapter> _logger;

    public CampoAdapter(IDbConnection connection, ILogger<CampoAdapter> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<CampoEncuestaDto?> ObtenerEncuestasRealizadasAsync(int año, int mes, string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryFirstOrDefaultAsync<CampoEncuestaDto>(
                "MBO_CampoEncuestasRealizadas",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo encuestas realizadas. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                año, mes, sigla);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<CampoCalidadDto?> ObtenerCalidadGeneralAsync(int año, int mes, string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryFirstOrDefaultAsync<CampoCalidadDto>(
                "MBO_CampoCalidadGeneral",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo calidad general. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                año, mes, sigla);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CampoCiudadDto>> ObtenerCalidadPorCiudadAsync(int año, int mes, string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryAsync<CampoCiudadDto>(
                "MBO_CampoCalidadPorCiudad",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo calidad por ciudad. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                año, mes, sigla);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CampoEncuestadorDto>> ObtenerCalidadPorEncuestadorAsync(int año, int mes, string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryAsync<CampoEncuestadorDto>(
                "MBO_CampoCalidadPorEncuestador",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo calidad por encuestador. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                año, mes, sigla);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CampoErrorDto>> ObtenerErroresAsync(int año, int mes, string? sigla = null, int? idTrabajo = null, int? idEncuestador = null)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);
            parameters.Add("@Sigla", sigla);
            parameters.Add("@IdTrabajo", idTrabajo);
            parameters.Add("@IdEncuestador", idEncuestador);

            var result = await _connection.QueryAsync<CampoErrorDto>(
                "MBO_CampoErroresGet",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo errores de campo. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                año, mes, sigla);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<CampoErrorDto?> ObtenerErrorPorIdAsync(int idError)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdError", idError);

            var result = await _connection.QueryFirstOrDefaultAsync<CampoErrorDto>(
                "MBO_CampoErroresGet",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo error por ID. IdError: {IdError}", idError);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<int> InsertarErrorAsync(CampoErrorDto error, int usuarioId)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdTrabajo", error.IdTrabajo);
            parameters.Add("@IdEncuestador", error.IdEncuestador);
            parameters.Add("@IdCiudad", error.IdCiudad);
            parameters.Add("@FechaEncuesta", error.FechaEncuesta);
            parameters.Add("@NumeroEncuesta", error.NumeroEncuesta);
            parameters.Add("@IdTipoError", error.IdTipoError);
            parameters.Add("@Observaciones", error.Observaciones);
            parameters.Add("@AccionCorrectiva", error.AccionCorrectiva);
            parameters.Add("@Estado", error.Estado);
            parameters.Add("@RegistradoPor", usuarioId);
            parameters.Add("@IdError", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await _connection.ExecuteAsync(
                "MBO_CampoErroresInsert",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var idErrorGenerado = parameters.Get<int>("@IdError");
            _logger.LogInformation("Error de campo insertado. IdError: {IdError}, Usuario: {UsuarioId}", 
                idErrorGenerado, usuarioId);

            return idErrorGenerado;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error insertando error de campo. Usuario: {UsuarioId}", usuarioId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> ActualizarErrorAsync(CampoErrorDto error, int usuarioId)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdError", error.IdError);
            parameters.Add("@IdTrabajo", error.IdTrabajo);
            parameters.Add("@IdEncuestador", error.IdEncuestador);
            parameters.Add("@IdCiudad", error.IdCiudad);
            parameters.Add("@FechaEncuesta", error.FechaEncuesta);
            parameters.Add("@NumeroEncuesta", error.NumeroEncuesta);
            parameters.Add("@IdTipoError", error.IdTipoError);
            parameters.Add("@Observaciones", error.Observaciones);
            parameters.Add("@AccionCorrectiva", error.AccionCorrectiva);
            parameters.Add("@Estado", error.Estado);
            parameters.Add("@ModificadoPor", usuarioId);

            var rowsAffected = await _connection.ExecuteAsync(
                "MBO_CampoErroresUpdate",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Error de campo actualizado. IdError: {IdError}, Usuario: {UsuarioId}", 
                error.IdError, usuarioId);

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando error de campo. IdError: {IdError}, Usuario: {UsuarioId}", 
                error.IdError, usuarioId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> EliminarErrorAsync(int idError, int usuarioId)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdError", idError);
            parameters.Add("@ModificadoPor", usuarioId);

            var rowsAffected = await _connection.ExecuteAsync(
                "MBO_CampoErroresDelete",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            _logger.LogInformation("Error de campo eliminado. IdError: {IdError}, Usuario: {UsuarioId}", 
                idError, usuarioId);

            return rowsAffected > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error eliminando error de campo. IdError: {IdError}, Usuario: {UsuarioId}", 
                idError, usuarioId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<CampoTipoErrorDto>> ObtenerTiposErrorAsync(bool soloActivos = true)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@SoloActivos", soloActivos);

            var result = await _connection.QueryAsync<CampoTipoErrorDto>(
                "MBO_CampoTiposErrorGet",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo tipos de error");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<dynamic>> ObtenerCiudadesAsync(string? sigla = null)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryAsync(
                "MBO_CampoCiudadesGet",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo ciudades. Sigla: {Sigla}", sigla);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<dynamic>> ObtenerEncuestadoresAsync(string? sigla = null, int? idCiudad = null)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Sigla", sigla);
            parameters.Add("@IdCiudad", idCiudad);

            var result = await _connection.QueryAsync(
                "MBO_CampoEncuestadoresGet",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo encuestadores. Sigla: {Sigla}, IdCiudad: {IdCiudad}", 
                sigla, idCiudad);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<(int insertados, int errores, string mensaje)> CargarErroresExcelAsync(IEnumerable<CampoErrorDto> errores, int usuarioId)
    {
        try
        {
            // Convertir lista de errores a DataTable para TVP (Table-Valued Parameter)
            var dt = new DataTable();
            dt.Columns.Add("IdTrabajo", typeof(int));
            dt.Columns.Add("IdEncuestador", typeof(int));
            dt.Columns.Add("IdCiudad", typeof(int));
            dt.Columns.Add("FechaEncuesta", typeof(DateTime));
            dt.Columns.Add("NumeroEncuesta", typeof(string));
            dt.Columns.Add("IdTipoError", typeof(int));
            dt.Columns.Add("Observaciones", typeof(string));
            dt.Columns.Add("AccionCorrectiva", typeof(string));
            dt.Columns.Add("Estado", typeof(string));

            foreach (var error in errores)
            {
                dt.Rows.Add(
                    error.IdTrabajo,
                    error.IdEncuestador,
                    error.IdCiudad,
                    error.FechaEncuesta,
                    error.NumeroEncuesta,
                    error.IdTipoError,
                    error.Observaciones ?? (object)DBNull.Value,
                    error.AccionCorrectiva ?? (object)DBNull.Value,
                    error.Estado
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Errores", dt.AsTableValuedParameter("MBO_CampoErroresType"));
            parameters.Add("@RegistradoPor", usuarioId);
            parameters.Add("@Insertados", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@ErroresCarga", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@Mensaje", dbType: DbType.String, size: 500, direction: ParameterDirection.Output);

            await _connection.ExecuteAsync(
                "MBO_CampoCargarErroresExcel",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var insertados = parameters.Get<int>("@Insertados");
            var erroresCarga = parameters.Get<int>("@ErroresCarga");
            var mensaje = parameters.Get<string>("@Mensaje");

            _logger.LogInformation("Carga masiva de errores Excel. Insertados: {Insertados}, Errores: {Errores}, Usuario: {UsuarioId}", 
                insertados, erroresCarga, usuarioId);

            return (insertados, erroresCarga, mensaje);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en carga masiva de errores Excel. Usuario: {UsuarioId}", usuarioId);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<string>> ValidarErroresAsync(IEnumerable<CampoErrorDto> errores)
    {
        try
        {
            var dt = new DataTable();
            dt.Columns.Add("IdTrabajo", typeof(int));
            dt.Columns.Add("IdEncuestador", typeof(int));
            dt.Columns.Add("IdCiudad", typeof(int));
            dt.Columns.Add("FechaEncuesta", typeof(DateTime));
            dt.Columns.Add("NumeroEncuesta", typeof(string));
            dt.Columns.Add("IdTipoError", typeof(int));

            foreach (var error in errores)
            {
                dt.Rows.Add(
                    error.IdTrabajo,
                    error.IdEncuestador,
                    error.IdCiudad,
                    error.FechaEncuesta,
                    error.NumeroEncuesta,
                    error.IdTipoError
                );
            }

            var parameters = new DynamicParameters();
            parameters.Add("@Errores", dt.AsTableValuedParameter("MBO_CampoErroresType"));

            var result = await _connection.QueryAsync<string>(
                "MBO_CampoValidarErrores",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando errores de campo");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<CampoEstadisticaDto?> ObtenerEstadisticasAsync(int año, int mes, string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryFirstOrDefaultAsync<CampoEstadisticaDto>(
                "MBO_CampoEstadisticasEncuestas",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo estadísticas de campo. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                año, mes, sigla);
            throw;
        }
    }
}
