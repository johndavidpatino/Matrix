using MatrixNext.Data.DTOs.RP;

namespace MatrixNext.Data.Services.RP;

/// <summary>
/// Interfaz para reportes de avance de campo
/// </summary>
public interface IAvanceCampoService
{
    /// <summary>
    /// Obtiene avance general de campo para un trabajo
    /// SP: REP_AvanceCampoGeneral
    /// </summary>
    Task<AvanceCampoGeneralDto?> ObtenerAvanceGeneralAsync(long trabajoId);
    
    /// <summary>
    /// Obtiene avance por ciudad
    /// SP: REP_AvanceCampoxCiudad
    /// </summary>
    Task<List<AvanceCampoCiudadDto>> ObtenerAvancePorCiudadAsync(long trabajoId);
    
    /// <summary>
    /// Obtiene avance porcentual por áreas
    /// SP: REP_AvancePorcentualAreas
    /// </summary>
    Task<List<AvanceAreaDto>> ObtenerAvancePorAreasAsync(long trabajoId);
    
    /// <summary>
    /// Obtiene remanentes por áreas
    /// SP: REP_AvanceAreasRemanentes
    /// </summary>
    Task<List<AvanceRemanenteDto>> ObtenerRemanentesAsync(long trabajoId);
    
    /// <summary>
    /// Obtiene matriz de cumplimiento
    /// SP: REP_MatrizEstimacionCumplimiento
    /// </summary>
    Task<List<MatrizCumplimientoDto>> ObtenerMatrizCumplimientoAsync(long trabajoId);
    
    /// <summary>
    /// Obtiene encuestas anuladas
    /// </summary>
    Task<List<EncuestaAnuladaDto>> ObtenerEncuestasAnuladasAsync(long trabajoId);
    
    /// <summary>
    /// Verifica si el trabajo tiene datos de estimación
    /// </summary>
    Task<bool> TieneDatosEstimacionAsync(long trabajoId);
    
    /// <summary>
    /// Prepara el ViewModel completo para la vista
    /// </summary>
    Task<AvanceCampoViewModel> PrepararViewModelAsync(long trabajoId);
    
    /// <summary>
    /// Exporta avance de campo a Excel
    /// </summary>
    Task<byte[]> ExportarExcelAsync(long trabajoId);
}
