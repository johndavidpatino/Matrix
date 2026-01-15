using System.Data;
using Dapper;
using MatrixNext.Data.Models.MBO;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Adapters.MBO;

/// <summary>
/// Implementación del adapter para acceso a datos de AOT
/// Ejecuta stored procedures de MBO usando Dapper
/// </summary>
public class AOTAdapter : IAOTAdapter
{
    private readonly IDbConnection _connection;
    private readonly ILogger<AOTAdapter> _logger;

    public AOTAdapter(IDbConnection connection, ILogger<AOTAdapter> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task<IEnumerable<UnidadUsuarioDto>> ObtenerUnidadesUsuarioAsync(int usuarioId)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", usuarioId);

            var result = await _connection.QueryAsync<UnidadUsuarioDto>(
                "MBO_ObtenerUnidadesUsuario",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo unidades para usuario {UsuarioId}", usuarioId);
            throw;
        }
    }

    public async Task<AOTBudgetEjecucionDto?> ObtenerBudgetEjecucionAsync(int año, int mes, string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryFirstOrDefaultAsync<AOTBudgetEjecucionDto>(
                "MBO_PGAOTBudgetEjecucionAñoMes",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo budget/ejecución AOT. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                año, mes, sigla);
            throw;
        }
    }

    public async Task<AOTMetaTotalDto?> ObtenerMetaTotalAsync(string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryFirstOrDefaultAsync<AOTMetaTotalDto>(
                "MBO_PGAOTBudgetMetaTotal",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo meta total AOT. Sigla: {Sigla}", sigla);
            throw;
        }
    }

    public async Task<AOTEjecucionTotalDto?> ObtenerEjecucionTotalAsync(int año, int mes, string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryFirstOrDefaultAsync<AOTEjecucionTotalDto>(
                "MBO_PGAOTEjecucionTotal",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo ejecución total AOT. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                año, mes, sigla);
            throw;
        }
    }

    public async Task<IEnumerable<AOTUnidadDto>> ObtenerBudgetPorUnidadAsync(int año, int mes, string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryAsync<AOTUnidadDto>(
                "MBO_PGAOTBudgetEjecucionUnidad",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo budget por unidad AOT. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                año, mes, sigla);
            throw;
        }
    }

    public async Task<AOTAcquisitionDto?> ObtenerAOTAcquisitionAsync(string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryFirstOrDefaultAsync<AOTAcquisitionDto>(
                "MBO_AOTAcquisition",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo AOT Acquisition. Sigla: {Sigla}", sigla);
            throw;
        }
    }

    public async Task<IEnumerable<AOTGerenteDto>> ObtenerAOTPorGerenteAsync(int año, int mes, string sigla)
    {
        try
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);
            parameters.Add("@Sigla", sigla);

            var result = await _connection.QueryAsync<AOTGerenteDto>(
                "MBO_PGAOTPorUnidadGerente",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo AOT por gerente. Año: {Año}, Mes: {Mes}, Sigla: {Sigla}", 
                año, mes, sigla);
            throw;
        }
    }
}
