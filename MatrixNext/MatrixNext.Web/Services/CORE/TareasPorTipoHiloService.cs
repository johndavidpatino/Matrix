using MatrixNext.Web.ViewModels;
using MatrixNext.Web.ViewModels.CORE;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Servicio de dominio para asignar tareas a tipos de hilo (Configuracion_TareasXTipoHilo)
    /// </summary>
    public interface ITareasPorTipoHiloService
    {
        Task<ResultVM<IEnumerable<TareaPorTipoHiloVM>>> ObtenerAsync(long tipoHiloId, bool? asignada = null);
        Task<ResultVM<bool>> AsignarAsync(long tipoHiloId, long tareaId, long usuarioId);
        Task<ResultVM<bool>> DesasignarAsync(long tipoHiloId, long tareaId, long usuarioId);
    }

    public class TareasPorTipoHiloService : ITareasPorTipoHiloService
    {
        private readonly TareasPorTipoHiloDataAdapter _adapter;
        private readonly IAuditoriaService _auditoria;
        private readonly ILogger<TareasPorTipoHiloService> _logger;

        public TareasPorTipoHiloService(
            TareasPorTipoHiloDataAdapter adapter,
            IAuditoriaService auditoria,
            ILogger<TareasPorTipoHiloService> logger)
        {
            _adapter = adapter;
            _auditoria = auditoria;
            _logger = logger;
        }

        public async Task<ResultVM<IEnumerable<TareaPorTipoHiloVM>>> ObtenerAsync(long tipoHiloId, bool? asignada = null)
        {
            if (tipoHiloId <= 0)
            {
                return ResultVM<IEnumerable<TareaPorTipoHiloVM>>.Fail("El tipo de hilo es obligatorio");
            }

            var data = await _adapter.ObtenerAsync(tipoHiloId, asignada);
            return ResultVM<IEnumerable<TareaPorTipoHiloVM>>.Ok(data);
        }

        public async Task<ResultVM<bool>> AsignarAsync(long tipoHiloId, long tareaId, long usuarioId)
        {
            if (tipoHiloId <= 0 || tareaId <= 0)
            {
                return ResultVM<bool>.Fail("Datos inválidos para la asignación");
            }

            try
            {
                var inserted = await _adapter.AsignarAsync(tipoHiloId, tareaId);
                if (!inserted)
                {
                    return ResultVM<bool>.Ok(true, "La tarea ya estaba asignada al tipo de hilo");
                }

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_TipoHilo_Tareas",
                    EntidadId = tareaId,
                    Accion = "ASSIGN",
                    Detalles = $"Asignar TareaId={tareaId} a TipoHiloId={tipoHiloId}",
                    IdUsuario = usuarioId,
                    RutaArchivo = string.Empty
                });

                return ResultVM<bool>.Ok(true, "Tarea asignada al tipo de hilo");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error asignando tarea {TareaId} a tipo de hilo {TipoHiloId}", tareaId, tipoHiloId);
                return ResultVM<bool>.Fail("Error al asignar la tarea al tipo de hilo. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> DesasignarAsync(long tipoHiloId, long tareaId, long usuarioId)
        {
            if (tipoHiloId <= 0 || tareaId <= 0)
            {
                return ResultVM<bool>.Fail("Datos inválidos para la desasignación");
            }

            try
            {
                var deleted = await _adapter.DesasignarAsync(tipoHiloId, tareaId);
                if (!deleted)
                {
                    return ResultVM<bool>.Fail("La tarea no estaba asignada a este tipo de hilo");
                }

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_TipoHilo_Tareas",
                    EntidadId = tareaId,
                    Accion = "UNASSIGN",
                    Detalles = $"Desasignar TareaId={tareaId} de TipoHiloId={tipoHiloId}",
                    IdUsuario = usuarioId,
                    RutaArchivo = string.Empty
                });

                return ResultVM<bool>.Ok(true, "Tarea desasignada del tipo de hilo");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desasignando tarea {TareaId} del tipo de hilo {TipoHiloId}", tareaId, tipoHiloId);
                return ResultVM<bool>.Fail("Error al desasignar la tarea del tipo de hilo. Por favor intente nuevamente.");
            }
        }
    }
}
