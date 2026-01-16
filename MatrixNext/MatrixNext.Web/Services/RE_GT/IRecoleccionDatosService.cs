// MatrixNext.Web/Services/RE_GT/IRecoleccionDatosService.cs

using MatrixNext.Core.DTOs.RE_GT;

namespace MatrixNext.Web.Services.RE_GT
{
    /// <summary>
    /// Interfaz para servicio de Recolección de Datos
    /// Landing page de navegación a operaciones de recolección
    /// </summary>
    public interface IRecoleccionDatosService
    {
        /// <summary>
        /// Obtiene la estructura de menú para la página de Recolección de Datos
        /// </summary>
        Task<RecoleccionDatosMenuDto> ObtenerMenuRecoleccionAsync();

        /// <summary>
        /// Obtiene la estructura de menú para Gestión y Tratamiento
        /// </summary>
        Task<GestionTratamientoDatosMenuDto> ObtenerMenuGestionTratamientoAsync();
    }
}
