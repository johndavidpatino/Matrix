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
            // NOTA: SP MBO_PGAOTBudgetEjecucionAñoMes solo acepta @Año (int)
            // Los parámetros @Mes y @Sigla no existen en el SP
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);

            var result = await _connection.QueryFirstOrDefaultAsync<AOTBudgetEjecucionDto>(
                "MBO_PGAOTBudgetEjecucionAñoMes",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo budget/ejecución AOT. Año: {Año}", año);
            throw;
        }
    }

    public async Task<AOTMetaTotalDto?> ObtenerMetaTotalAsync(string sigla)
    {
        try
        {
            // NOTA: SP MBO_PGAOTBudgetMetaTotal acepta @Año (int), no @Sigla
            // Usando año actual como default
            var parameters = new DynamicParameters();
            parameters.Add("@Año", DateTime.Now.Year);

            var result = await _connection.QueryFirstOrDefaultAsync<AOTMetaTotalDto>(
                "MBO_PGAOTBudgetMetaTotal",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo meta total AOT");
            throw;
        }
    }

    public async Task<AOTEjecucionTotalDto?> ObtenerEjecucionTotalAsync(int año, int mes, string sigla)
    {
        try
        {
            // NOTA: SP MBO_PGAOTEjecucionTotal acepta @Año y @Mes (int), no @Sigla
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@Mes", mes);

            var result = await _connection.QueryFirstOrDefaultAsync<AOTEjecucionTotalDto>(
                "MBO_PGAOTEjecucionTotal",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo ejecución total AOT. Año: {Año}, Mes: {Mes}", año, mes);
            throw;
        }
    }

    public async Task<IEnumerable<AOTUnidadDto>> ObtenerBudgetPorUnidadAsync(int año, int mes, string sigla)
    {
        try
        {
            // NOTA: SP MBO_PGAOTBudgetEjecucionUnidad acepta @Año, @MesInicial, @MesHasta (int)
            // Usando @Mes como ambos límites para consulta de un solo mes
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@MesInicial", mes);
            parameters.Add("@MesHasta", mes);

            var result = await _connection.QueryAsync<AOTUnidadDto>(
                "MBO_PGAOTBudgetEjecucionUnidad",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo budget por unidad AOT. Año: {Año}, Mes: {Mes}", año, mes);
            throw;
        }
    }

    public async Task<AOTAcquisitionDto?> ObtenerAOTAcquisitionAsync(string sigla)
    {
        try
        {
            // NOTA: SP MBO_AOTAcquisition acepta @Año y @Mes (int), no @Sigla
            // Usando año y mes actual como default
            var parameters = new DynamicParameters();
            parameters.Add("@Año", DateTime.Now.Year);
            parameters.Add("@Mes", DateTime.Now.Month);

            var result = await _connection.QueryFirstOrDefaultAsync<AOTAcquisitionDto>(
                "MBO_AOTAcquisition",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo AOT Acquisition");
            throw;
        }
    }

    public async Task<IEnumerable<AOTGerenteDto>> ObtenerAOTPorGerenteAsync(int año, int mes, string sigla)
    {
        try
        {
            // NOTA: SP MBO_PGAOTPorUnidadGerente acepta @Año, @MesInicial, @MesHasta (int), @Unidad (varchar)
            var parameters = new DynamicParameters();
            parameters.Add("@Año", año);
            parameters.Add("@MesInicial", mes);
            parameters.Add("@MesHasta", mes);
            parameters.Add("@Unidad", sigla);

            var result = await _connection.QueryAsync<AOTGerenteDto>(
                "MBO_PGAOTPorUnidadGerente",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo AOT por gerente. Año: {Año}, Mes: {Mes}, Unidad: {Unidad}", 
                año, mes, sigla);
            throw;
        }
    }
}
