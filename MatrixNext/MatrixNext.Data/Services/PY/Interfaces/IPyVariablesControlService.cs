using MatrixNext.Data.Adapters.PY.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY.Interfaces
{
    /// <summary>
    /// Servicio de dominio para gestión de variables de control por trabajo/modalidad.
    /// Orquesta validaciones de variables de control transversales.
    /// </summary>
    public interface IPyVariablesControlService
    {
        /// <summary>
        /// Obtiene todas las variables de control para un trabajo.
        /// </summary>
        Task<List<VariableControlDto>> ObtenerVariablesPorTrabajo(int trabajoId);

        /// <summary>
        /// Guarda una nueva variable de control.
        /// </summary>
        Task<int> GuardarVariableControl(VariableControlInputDto input);

        /// <summary>
        /// Valida que todas las variables de control requeridas estén completas.
        /// </summary>
        Task<bool> ValidarVariablesCompletadas(int trabajoId);
    }
}
