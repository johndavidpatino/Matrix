using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Servicio para gestionar festivos y días no laborables.
    /// Utilizado para validación de planillas dominicales (TipoActividad 22/23)
    /// y exclusión de festivos en auto-planeación de producción.
    /// </summary>
    public interface IOpFestivosService
    {
        /// <summary>
        /// Obtiene la lista de festivos en un rango de fechas.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio del rango (inclusive).</param>
        /// <param name="fechaFin">Fecha de fin del rango (inclusive).</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>
        /// Lista de fechas festivas en el rango especificado.
        /// Retorna lista vacía si no hay festivos o si ocurre un error.
        /// </returns>
        /// <remarks>
        /// Consulta la tabla _Festivos de la base de datos.
        /// Utilizado en:
        /// - Auto-planeación de estimación (EstimacionProduccionController)
        /// - Auto-planeación de muestra (MuestraTrabajosController)
        /// - Validación de planillas dominicales (OpCargaService)
        /// </remarks>
        Task<List<DateOnly>> ObtenerFestivosEnRangoAsync(
            DateOnly fechaInicio,
            DateOnly fechaFin,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Verifica si una fecha específica es festivo.
        /// </summary>
        /// <param name="fecha">Fecha a verificar.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>
        /// <c>true</c> si la fecha es festivo; <c>false</c> en caso contrario.
        /// </returns>
        /// <remarks>
        /// Internamente consulta los festivos del año de la fecha especificada
        /// y verifica si la fecha está en el conjunto de festivos.
        /// Implementa caché en memoria para evitar consultas repetidas.
        /// </remarks>
        Task<bool> EsDiaFestivoAsync(
            DateOnly fecha,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Obtiene todos los festivos de un año específico.
        /// </summary>
        /// <param name="año">Año para el cual obtener los festivos.</param>
        /// <param name="cancellationToken">Token de cancelación.</param>
        /// <returns>
        /// HashSet con todas las fechas festivas del año especificado.
        /// Retorna HashSet vacío si no hay festivos o si ocurre un error.
        /// </returns>
        /// <remarks>
        /// Utilizado internamente por <see cref="EsDiaFestivoAsync"/> para caché.
        /// Puede ser usado directamente para cargar calendarios completos.
        /// </remarks>
        Task<HashSet<DateOnly>> ObtenerFestivosPorAñoAsync(
            int año,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Limpia el caché interno de festivos.
        /// </summary>
        /// <remarks>
        /// Útil para forzar recarga después de modificaciones en la tabla _Festivos.
        /// El caché se limpia automáticamente en cambio de año.
        /// </remarks>
        void LimpiarCache();
    }
}
