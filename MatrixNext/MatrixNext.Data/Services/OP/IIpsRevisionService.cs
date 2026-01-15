using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Models.OP;

namespace MatrixNext.Data.Services.OP
{
    /// <summary>
    /// Interfaz para servicio de revisiones IPS por tarea
    /// </summary>
    public interface IIpsRevisionService
    {
        /// <summary>
        /// Obtiene las revisiones IPS para una tarea
        /// </summary>
        Task<IEnumerable<IpsRevisionDto>> ObtenerRevisionesAsync(long trabajoId);

        /// <summary>
        /// Obtiene una revisión IPS específica
        /// </summary>
        Task<IpsRevisionDto?> ObtenerRevisionAsync(long revisionId);

        /// <summary>
        /// Crea una nueva revisión IPS
        /// </summary>
        Task<(bool Success, string Message, long Id)> CrearRevisionAsync(IpsRevisionCreateUpdateDto dto, long usuarioId);

        /// <summary>
        /// Actualiza una revisión IPS
        /// </summary>
        Task<(bool Success, string Message)> ActualizarRevisionAsync(IpsRevisionCreateUpdateDto dto, long usuarioId);

        /// <summary>
        /// Elimina una revisión IPS
        /// </summary>
        Task<(bool Success, string Message)> EliminarRevisionAsync(long revisionId, long usuarioId);
    }
}
