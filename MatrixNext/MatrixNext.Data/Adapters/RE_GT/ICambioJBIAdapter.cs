using MatrixNext.Core.DTOs.RE_GT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Adapters.RE_GT
{
    /// <summary>
    /// Adapter para acceso a datos de CambioJBI
    /// </summary>
    public interface ICambioJBIAdapter
    {
        /// <summary>
        /// Obtiene lista de fases activas desde BD
        /// </summary>
        Task<List<FaseDto>> ObtenerFasesAsync();

        /// <summary>
        /// Obtiene información de trabajo para validación
        /// </summary>
        Task<TrabajoInfoDto> ObtenerTrabajoAsync(int idTrabajo);

        /// <summary>
        /// Valida si fase existe en presupuestos
        /// </summary>
        Task<bool> ValidarFaseCreadaAsync(int idPropuesta, int alternativa, int idFase, string metCodigo);

        /// <summary>
        /// Ejecuta SP para cambiar JBI
        /// </summary>
        Task CambiarJBIAsync(CambioJBIDto dto);

        /// <summary>
        /// Guarda log de cambio de JBI
        /// </summary>
        Task GuardarLogCambioAsync(LogCambioJBIDto logDto);
    }
}
