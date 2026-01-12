using MatrixNext.Data.Adapters.PY.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY.Interfaces
{
    /// <summary>
    /// Servicio de dominio para gestión de visitas a domicilio (InHome) en modalidades cualitativas.
    /// Orquesta operaciones de lectura/escritura entre adapter de datos y lógica de negocio.
    /// </summary>
    public interface IPyInHomeVisitService
    {
        /// <summary>
        /// Obtiene todas las visitas InHome asociadas a un trabajo.
        /// </summary>
        Task<List<InHomeVisitDto>> ObtenerInHomesPorTrabajo(int trabajoId);

        /// <summary>
        /// Obtiene el historial de log para una visita InHome específica.
        /// </summary>
        Task<List<LogInHomeDto>> ObtenerLogInHome(int idInHome);

        /// <summary>
        /// Guarda una nueva visita InHome (creación de registro).
        /// </summary>
        Task<int> GuardarInHome(InHomeVisitInputDto input);

        /// <summary>
        /// Actualiza datos de una visita InHome existente.
        /// </summary>
        Task<bool> ActualizarInHome(InHomeVisitInputDto input);

        /// <summary>
        /// Registra evento en log de InHome (cambios de estado, observaciones, etc).
        /// </summary>
        Task<int> GuardarLogInHome(int idInHome, string descripcion, string usuario);
    }
}
