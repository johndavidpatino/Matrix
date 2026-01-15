using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MatrixNext.Data.Models.OP;

namespace MatrixNext.Data.Adapters.OP
{
    /// <summary>
    /// Interfaz para acceso a datos de revisiones IPS por tarea
    /// </summary>
    public interface IIpsRevisionAdapter
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
        Task<long> CrearRevisionAsync(IpsRevisionCreateUpdateDto dto, long usuarioId);

        /// <summary>
        /// Actualiza una revisión IPS
        /// </summary>
        Task<bool> ActualizarRevisionAsync(IpsRevisionCreateUpdateDto dto, long usuarioId);

        /// <summary>
        /// Elimina una revisión IPS
        /// </summary>
        Task<bool> EliminarRevisionAsync(long revisionId, long usuarioId);
    }
}
