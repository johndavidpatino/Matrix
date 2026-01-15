using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.Adapters.MBO;

/// <summary>
/// Adapter para acceso a datos de AOT (Achievement of Tasks)
/// </summary>
public interface IAOTAdapter
{
    /// <summary>
    /// Obtiene unidades asociadas a un usuario
    /// SP: MBO_ObtenerUnidadesUsuario
    /// </summary>
    Task<IEnumerable<UnidadUsuarioDto>> ObtenerUnidadesUsuarioAsync(int usuarioId);
    
    /// <summary>
    /// Obtiene datos de Budget y Ejecución AOT por año/mes
    /// SP: MBO_PGAOTBudgetEjecucionAñoMes
    /// </summary>
    Task<AOTBudgetEjecucionDto?> ObtenerBudgetEjecucionAsync(int año, int mes, string sigla);
    
    /// <summary>
    /// Obtiene meta total anual
    /// SP: MBO_PGAOTBudgetMetaTotal
    /// </summary>
    Task<AOTMetaTotalDto?> ObtenerMetaTotalAsync(string sigla);
    
    /// <summary>
    /// Obtiene ejecución total acumulada
    /// SP: MBO_PGAOTEjecucionTotal
    /// </summary>
    Task<AOTEjecucionTotalDto?> ObtenerEjecucionTotalAsync(int año, int mes, string sigla);
    
    /// <summary>
    /// Obtiene Budget y Ejecución desagregado por unidad
    /// SP: MBO_PGAOTBudgetEjecucionUnidad
    /// </summary>
    Task<IEnumerable<AOTUnidadDto>> ObtenerBudgetPorUnidadAsync(int año, int mes, string sigla);
    
    /// <summary>
    /// Obtiene datos de adquisición AOT
    /// SP: MBO_AOTAcquisition
    /// </summary>
    Task<AOTAcquisitionDto?> ObtenerAOTAcquisitionAsync(string sigla);
    
    /// <summary>
    /// Obtiene AOT desagregado por unidad y gerente
    /// SP: MBO_PGAOTPorUnidadGerente
    /// </summary>
    Task<IEnumerable<AOTGerenteDto>> ObtenerAOTPorGerenteAsync(int año, int mes, string sigla);
}
