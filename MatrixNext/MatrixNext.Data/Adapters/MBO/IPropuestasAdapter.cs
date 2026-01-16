using MatrixNext.Data.Models.MBO;

namespace MatrixNext.Data.Adapters.MBO;

/// <summary>
/// Interfaz para el adaptador de datos de Propuestas y Gestión MBO
/// Mapea stored procedures del sistema legacy WebMatrix
/// </summary>
public interface IPropuestasAdapter
{
    /// <summary>
    /// Obtiene el estado de propuestas creadas/enviadas por unidad
    /// SP: MBO_PropuestasCreadasEnviadasSinAnuncioActualizar
    /// </summary>
    /// <param name="sigla">Sigla de unidad ('9' para todas)</param>
    Task<IEnumerable<PropuestaEstadoDto>> ObtenerPropuestasCreadasEnviadasAsync(string sigla);

    /// <summary>
    /// Obtiene propuestas creadas/enviadas por gerente de cuentas
    /// SP: MBO_PropuestasCreadasEnviadasSinAnuncioGC
    /// </summary>
    /// <param name="sigla">Sigla de unidad ('9' para todas)</param>
    Task<IEnumerable<PropuestaPorGerenteDto>> ObtenerPropuestasPorGerenteAsync(string sigla);

    /// <summary>
    /// Obtiene propuestas con alta probabilidad que requieren actualización
    /// SP: MBO_PropuestasAltaProbabilidadPorActualizar
    /// </summary>
    /// <param name="sigla">Sigla de unidad ('9' para todas)</param>
    Task<IEnumerable<PropuestaAltaProbabilidadDto>> ObtenerPropuestasAltaProbabilidadAsync(string sigla);

    /// <summary>
    /// Obtiene propuestas con alta probabilidad por unidad específica
    /// SP: MBO_PropuestasAltaProbabilidadUnidad
    /// </summary>
    /// <param name="sigla">Sigla de unidad</param>
    Task<IEnumerable<PropuestaAltaProbabilidadDto>> ObtenerPropuestasAltaProbabilidadUnidadAsync(string sigla);

    /// <summary>
    /// Obtiene propuestas aprobadas sin trabajo asociado, agrupadas por unidad
    /// SP: MBO_PropuestasAprobadasSinTrabajoPorUnidad
    /// </summary>
    Task<IEnumerable<PropuestaSinTrabajoDto>> ObtenerPropuestasSinTrabajoPorUnidadAsync();

    /// <summary>
    /// Obtiene propuestas aprobadas sin trabajo, detalladas por unidad y metodología
    /// SP: MBO_PropuestasAprobadasSinTrabajoUnidadMetodo
    /// </summary>
    /// <param name="unidad">Unidad de negocio seleccionada</param>
    Task<IEnumerable<PropuestaSinTrabajoDto>> ObtenerPropuestasSinTrabajoPorMetodologiaAsync(string unidad);

    /// <summary>
    /// Obtiene listado completo de propuestas aprobadas sin trabajo
    /// SP: MBO_PropuestasAprobadasSinTrabajo
    /// </summary>
    Task<IEnumerable<PropuestaSinTrabajoDto>> ObtenerPropuestasSinTrabajoAsync();

    /// <summary>
    /// Obtiene datos de gestión general de Matrix (briefs, propuestas, presupuestos, etc.)
    /// SP: MBO_PGGestionMatrix
    /// </summary>
    Task<GestionMatrixDto?> ObtenerGestionMatrixAsync();

    /// <summary>
    /// Obtiene índices manuales de cuentas
    /// SP: MBO_PGIndicesManuales
    /// </summary>
    Task<IEnumerable<IndiceManualDto>> ObtenerIndicesManualesAsync();
}
