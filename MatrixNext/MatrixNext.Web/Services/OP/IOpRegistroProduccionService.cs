using MatrixNext.Web.Models.OP.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Interfaz para el servicio de registro de actividades de producción en OP.
    /// Permite registrar actividades, búsqueda de JobBooks, y validaciones.
    /// </summary>
    public interface IOpRegistroProduccionService
    {
        /// <summary>
        /// Obtiene las unidades/áreas disponibles para registro de producción.
        /// </summary>
        /// <returns>Lista de unidades con ID y nombre</returns>
        Task<List<CatalogoItemDto>> ObtenerUnidadesAsync();

        /// <summary>
        /// Obtiene las actividades disponibles para una unidad específica.
        /// </summary>
        /// <param name="unidadId">ID de la unidad seleccionada</param>
        /// <returns>Lista de actividades cascada</returns>
        Task<List<CatalogoItemDto>> ObtenerActividadesAsync(int unidadId);

        /// <summary>
        /// Obtiene las subactividades para una actividad específica.
        /// </summary>
        /// <param name="actividadId">ID de la actividad seleccionada</param>
        /// <returns>Lista de subactividades cascada</returns>
        Task<List<CatalogoItemDto>> ObtenerSubactividadesAsync(int actividadId);

        /// <summary>
        /// Busca JobBooks por criterios (JBE/JBI/CC).
        /// </summary>
        /// <param name="criterio">Criterio de búsqueda (ej: "CALI-2024")</param>
        /// <param name="tipo">Tipo: "JBE", "JBI", o "CC"</param>
        /// <returns>Lista de JobBooks encontrados</returns>
        Task<List<JobBookDto>> BuscarJobBooksAsync(string criterio, string tipo);

        /// <summary>
        /// Registra una actividad de producción.
        /// </summary>
        /// <param name="registro">Datos del registro a guardar</param>
        /// <returns>ID del registro creado</returns>
        Task<int> RegistrarActividadAsync(RegistroProduccionDto registro);

        /// <summary>
        /// Valida los datos del registro antes de guardar.
        /// </summary>
        /// <param name="registro">Registro a validar</param>
        /// <returns>Tupla con validez y mensaje de error si aplica</returns>
        Task<(bool Valid, string Message)> ValidarRegistroAsync(RegistroProduccionDto registro);
    }
}
