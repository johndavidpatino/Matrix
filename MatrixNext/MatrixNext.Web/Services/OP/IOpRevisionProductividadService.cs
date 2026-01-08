using MatrixNext.Web.Models.OP.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Services.OP
{
    /// <summary>
    /// Interfaz para el servicio de revisión de productividad multirrol en OP.
    /// Proporciona métodos para validar y aprobar/rechazar planillas por diferentes roles.
    /// </summary>
    public interface IOpRevisionProductividadService
    {
        /// <summary>
        /// Obtiene las planillas de productividad para un trabajo según el rol del usuario.
        /// </summary>
        /// <param name="trabajoId">ID del trabajo a revisar</param>
        /// <param name="rol">Rol del usuario: PMO, Coordinador, Campo, MyS/Call</param>
        /// <returns>Lista de planillas con información de cantidades y montos</returns>
        Task<List<PlanillaProductividadDto>> ObtenerPlanillasPorRolAsync(int trabajoId, string rol);

        /// <summary>
        /// Aprueba una planilla de productividad con monto autorizado.
        /// </summary>
        /// <param name="planillaId">ID de la planilla a aprobar</param>
        /// <param name="montoAutorizado">Monto autorizado para esta revisión</param>
        /// <param name="usuarioId">ID del usuario que aprueba</param>
        /// <returns>Resultado de la aprobación (true si exitosa)</returns>
        Task<bool> AprobarPlanillaAsync(int planillaId, decimal montoAutorizado, int usuarioId);

        /// <summary>
        /// Rechaza una planilla de productividad con observaciones.
        /// </summary>
        /// <param name="planillaId">ID de la planilla a rechazar</param>
        /// <param name="observacion">Motivo del rechazo</param>
        /// <param name="usuarioId">ID del usuario que rechaza</param>
        /// <returns>Resultado del rechazo (true si exitosa)</returns>
        Task<bool> RechazarPlanillaAsync(int planillaId, string observacion, int usuarioId);

        /// <summary>
        /// Valida si el monto actual de la planilla es válido según máximos del trabajo.
        /// </summary>
        /// <param name="trabajoId">ID del trabajo</param>
        /// <param name="montoTotal">Monto total a validar</param>
        /// <returns>Validación result con mensaje si hay error</returns>
        Task<(bool Valid, string Message)> ValidarMontosPlanillaAsync(int trabajoId, decimal montoTotal);
    }
}
