using MatrixNext.Data.DTOs.RE_GT;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.RE_GT
{
    /// <summary>
    /// Servicio para gestión de cambios de JobBook Interno (JBI)
    /// </summary>
    public interface ICambioJBIService
    {
        /// <summary>
        /// Obtiene la lista de fases activas
        /// </summary>
        Task<IEnumerable<FaseDto>> ObtenerFasesAsync();

        /// <summary>
        /// Obtiene información de un trabajo por ID para validación
        /// </summary>
        Task<TrabajoInfoDto> ObtenerTrabajoAsync(int idTrabajo);

        /// <summary>
        /// Valida si la fase está creada en presupuestos
        /// </summary>
        Task<bool> ValidarFaseCreadaAsync(int idPropuesta, int alternativa, int idFase, string metCodigo);

        /// <summary>
        /// Realiza el cambio de JobBook Interno de un trabajo
        /// </summary>
        Task<(bool success, string message)> CambiarJBIAsync(CambioJBIDto dto, int usuarioId);
    }
}
