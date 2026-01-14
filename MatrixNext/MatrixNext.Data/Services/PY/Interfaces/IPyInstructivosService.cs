using MatrixNext.Data.Adapters.PY.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY.Interfaces
{
    /// <summary>
    /// Servicio de dominio para gestión de especificaciones técnicas (instructivos).
    /// Orquesta creación, actualización y versionado de especificaciones cuantitativas, cualitativas y ayudas.
    /// Incluye integración de notificaciones por email para cambios de especificaciones.
    /// </summary>
    public interface IPyInstructivosService
    {
        /// <summary>
        /// Obtiene especificación técnica cuantitativa para un trabajo.
        /// </summary>
        Task<EspecificacionTecnicaDto?> ObtenerEspecificacionCuanti(int trabajoId);

        /// <summary>
        /// Obtiene especificación técnica cualitativa para un trabajo.
        /// </summary>
        Task<EspecificacionTecnicaCualiDto?> ObtenerEspecificacionCuali(int trabajoId);

        /// <summary>
        /// Guarda nueva especificación cuantitativa (crea versión inicial).
        /// Notifica por email a coordinadores si existen cambios respecto a versión anterior.
        /// </summary>
        Task<int> GuardarEspecificacionCuanti(EspecificacionTecnicaInputDto input, string usuario);

        /// <summary>
        /// Guarda nueva especificación cualitativa con sus ayudas y tipos de reclutamiento.
        /// Notifica cambios por email.
        /// </summary>
        Task<int> GuardarEspecificacionCuali(EspecificacionTecnicaCualiInputDto input, string usuario);

        /// <summary>
        /// Obtiene lista de ayudas cualitativas registradas para un trabajo.
        /// </summary>
        Task<List<AyudaCualiDto>> ObtenerAyudasCuali(int trabajoId);

        /// <summary>
        /// Guarda nueva ayuda cualitativa (material de apoyo en terreno).
        /// </summary>
        Task<int> GuardarAyudaCuali(AyudaCualiInputDto input);

        /// <summary>
        /// Obtiene tipos de reclutamiento especiales para modalidad cualitativa.
        /// </summary>
        Task<List<TipoReclutamientoCualiDto>> ObtenerTiposReclutamientoCuali(int trabajoId);

        /// <summary>
        /// Guarda tipo de reclutamiento cualitativo requerido.
        /// </summary>
        Task<int> GuardarTipoReclutamientoCuali(TipoReclutamientoCualiInputDto input);

        /// <summary>
        /// Obtiene historial de versiones para una especificación técnica.
        /// </summary>
        Task<List<dynamic>> ObtenerHistorialVersiones(int trabajoId, string tipoEspecificacion);

        /// <summary>
        /// Calcula cambios entre versiones y envía notificación.
        /// </summary>
        Task<bool> NotificarCambiosEspecificacion(int trabajoId, string tipoEspecificacion, string usuario);
    }
}
