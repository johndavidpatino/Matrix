using MatrixNext.Web.Models.PY;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.PY
{
    /// <summary>
    /// Integración PY → CORE: crea trabajo y dispara creación de tareas CORE (placeholder hasta definir SP)
    /// </summary>
    public interface ITrabajosWorkFlowService
    {
        Task<ResultVM<Trabajo>> CrearTrabajoConWorkFlowAsync(Trabajo trabajo);
    }

    public class TrabajosWorkFlowService : ITrabajosWorkFlowService
    {
        private readonly ITrabajosService _trabajosService;
        private readonly IWorkFlowService _workFlowService;

        public TrabajosWorkFlowService(ITrabajosService trabajosService, IWorkFlowService workFlowService)
        {
            _trabajosService = trabajosService;
            _workFlowService = workFlowService;
        }

        public async Task<ResultVM<Trabajo>> CrearTrabajoConWorkFlowAsync(Trabajo trabajo)
        {
            // 1. Crear trabajo
            var result = await _trabajosService.CrearAsync(trabajo);
            if (!result.IsSuccess || result.Data == null)
            {
                return ResultVM<Trabajo>.Fail(result.Message, result.Errors);
            }

            // 2. Integración CORE: placeholder para hilo de creación de tareas
            // TODO: Llamar a SP/servicio CORE que genere WorkFlow inicial según configuración
            // Por ahora, solo retornamos el trabajo creado
            return ResultVM<Trabajo>.Ok(result.Data, "Trabajo creado y pendiente de crear tareas CORE");
        }
    }
}
