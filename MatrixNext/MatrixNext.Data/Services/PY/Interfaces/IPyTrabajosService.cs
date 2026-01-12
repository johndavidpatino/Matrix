using MatrixNext.Data.Adapters.PY.Models;
using System.Threading.Tasks;

namespace MatrixNext.Data.Services.PY.Interfaces
{
    /// <summary>
    /// Servicio de dominio para gestión de trabajos (proyectos) en modalidades cualitativas y cuantitativas.
    /// Orquesta flujos complejos como duplicación de trabajos, configuración de modalidades y orquestación general.
    /// </summary>
    public interface IPyTrabajosService
    {
        /// <summary>
        /// Duplica un trabajo completo incluyendo especificaciones, muestras, hilo de cuotas e información.
        /// Orquesta las fases: DuplicarEspecificaciones → DuplicarMuestra → DuplicarHilo → CopiarDocumentos.
        /// </summary>
        Task<DuplicarTrabajoResultDto> DuplicarTrabajoCompleto(DuplicarTrabajoInputDto input, string usuario);

        /// <summary>
        /// Obtiene configuración de un trabajo (modalidades, técnicas, líneas, etc).
        /// </summary>
        Task<TrabajoConfiguracionDto> ObtenerConfiguracionTrabajo(int trabajoId);

        /// <summary>
        /// Guarda cambios en configuración de trabajo (modalidades activas, técnicas habilitadas).
        /// </summary>
        Task<bool> GuardarConfiguracionTrabajo(TrabajoConfiguracionInputDto input, string usuario);

        /// <summary>
        /// Valida que un trabajo esté listo para iniciar (especificaciones completas, muestras validadas).
        /// </summary>
        Task<bool> ValidarTrabajoListo(int trabajoId);

        /// <summary>
        /// Obtiene estado actual del trabajo (avance de fases, completitud de información).
        /// </summary>
        Task<dynamic> ObtenerEstadoTrabajo(int trabajoId);

        /// <summary>
        /// Cierra un trabajo (marca como completado, archiva información).
        /// </summary>
        Task<bool> CerrarTrabajo(int trabajoId, string motivo, string usuario);
    }
}
