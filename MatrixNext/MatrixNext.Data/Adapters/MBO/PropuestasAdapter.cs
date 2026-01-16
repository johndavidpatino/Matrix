using System.Data;
using Dapper;
using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.Adapters.MBO;

/// <summary>
/// Implementación del adaptador de datos de Propuestas y Gestión MBO
/// Ejecuta stored procedures del sistema legacy WebMatrix
/// </summary>
public class PropuestasAdapter : IPropuestasAdapter
{
    private readonly IDbConnection _connection;

    public PropuestasAdapter(IDbConnection connection)
    {
        _connection = connection;
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PropuestaEstadoDto>> ObtenerPropuestasCreadasEnviadasAsync(string sigla)
    {
        return await _connection.QueryAsync<PropuestaEstadoDto>(
            "MBO_PropuestasCreadasEnviadasSinAnuncioActualizar",
            new { Sigla = sigla },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PropuestaPorGerenteDto>> ObtenerPropuestasPorGerenteAsync(string sigla)
    {
        return await _connection.QueryAsync<PropuestaPorGerenteDto>(
            "MBO_PropuestasCreadasEnviadasSinAnuncioGC",
            new { Sigla = sigla },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PropuestaAltaProbabilidadDto>> ObtenerPropuestasAltaProbabilidadAsync(string sigla)
    {
        return await _connection.QueryAsync<PropuestaAltaProbabilidadDto>(
            "MBO_PropuestasAltaProbabilidadPorActualizar",
            new { Sigla = sigla },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PropuestaAltaProbabilidadDto>> ObtenerPropuestasAltaProbabilidadUnidadAsync(string sigla)
    {
        return await _connection.QueryAsync<PropuestaAltaProbabilidadDto>(
            "MBO_PropuestasAltaProbabilidadUnidad",
            new { Sigla = sigla },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PropuestaSinTrabajoDto>> ObtenerPropuestasSinTrabajoPorUnidadAsync()
    {
        return await _connection.QueryAsync<PropuestaSinTrabajoDto>(
            "MBO_PropuestasAprobadasSinTrabajoPorUnidad",
            commandType: CommandType.StoredProcedure
        );
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PropuestaSinTrabajoDto>> ObtenerPropuestasSinTrabajoPorMetodologiaAsync(string unidad)
    {
        return await _connection.QueryAsync<PropuestaSinTrabajoDto>(
            "MBO_PropuestasAprobadasSinTrabajoUnidadMetodo",
            new { Unidad = unidad },
            commandType: CommandType.StoredProcedure
        );
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<PropuestaSinTrabajoDto>> ObtenerPropuestasSinTrabajoAsync()
    {
        return await _connection.QueryAsync<PropuestaSinTrabajoDto>(
            "MBO_PropuestasAprobadasSinTrabajo",
            commandType: CommandType.StoredProcedure
        );
    }

    /// <inheritdoc/>
    public async Task<GestionMatrixDto?> ObtenerGestionMatrixAsync()
    {
        var result = await _connection.QueryAsync<GestionMatrixDto>(
            "MBO_PGGestionMatrix",
            commandType: CommandType.StoredProcedure
        );

        return result.FirstOrDefault();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<IndiceManualDto>> ObtenerIndicesManualesAsync()
    {
        return await _connection.QueryAsync<IndiceManualDto>(
            "MBO_PGIndicesManuales",
            commandType: CommandType.StoredProcedure
        );
    }
}
