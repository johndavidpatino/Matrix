using MatrixNext.Data.ViewModels.MBO;
using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.Services.MBO;

/// <summary>
/// Servicio para lógica de negocio de AOT (Achievement of Tasks)
/// </summary>
public interface IAOTService
{
    /// <summary>
    /// Obtiene unidades disponibles para el usuario logueado
    /// </summary>
    Task<IEnumerable<UnidadUsuarioDto>> ObtenerUnidadesUsuarioAsync(int usuarioId);
    
    /// <summary>
    /// Obtiene datos completos para dashboard de Dirección
    /// Ejecuta múltiples SP en paralelo para optimizar rendimiento
    /// </summary>
    Task<AOTDireccionViewModel> ObtenerDatosDireccionAsync(int año, int mes, string sigla, int usuarioId);
    
    /// <summary>
    /// Obtiene datos completos para dashboard de Gerencia
    /// </summary>
    Task<AOTGerenciaViewModel> ObtenerDatosGerenciaAsync(int año, int mes, string sigla, int usuarioId);
    
    /// <summary>
    /// Obtiene datos de AOT desagregados por gerentes
    /// </summary>
    Task<AOTPorGerentesViewModel> ObtenerDatosPorGerentesAsync(int año, int mes, string sigla, int usuarioId);
    
    /// <summary>
    /// Obtiene datos de AOT para una unidad específica
    /// </summary>
    Task<AOTUnidadViewModel> ObtenerDatosUnidadAsync(int año, int mes, string sigla, int usuarioId);
}
