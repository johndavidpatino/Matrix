using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Servicio de dominio para TareasPrevias (precedencias)
    /// Valida ciclos acíclicos antes de crear relaciones
    /// </summary>
    public interface ITareasPreviasService
    {
        Task<ResultVM<TareaPrevia>> CrearAsync(TareaPrevia entity);
        Task<ResultVM<bool>> EliminarAsync(long id);
        Task<IEnumerable<TareaPrevia>> ObtenerPorTareaAsync(long idTarea);
        Task<IEnumerable<TareaPrevia>> ObtenerTodasAsync();
    }

    public class TareasPreviasService : ITareasPreviasService
    {
        private readonly MatrixDbContext _db;
        private readonly TareasPreviasDataAdapter _adapter;
        private readonly GrafoAciclicoService _grafo;
        private readonly IAuditoriaService _auditoria;

        public TareasPreviasService(
            MatrixDbContext db,
            TareasPreviasDataAdapter adapter,
            GrafoAciclicoService grafo,
            IAuditoriaService auditoria)
        {
            _db = db;
            _adapter = adapter;
            _grafo = grafo;
            _auditoria = auditoria;
        }

        public async Task<ResultVM<TareaPrevia>> CrearAsync(TareaPrevia entity)
        {
            // Validar que no sea autorreferencial
            if (entity.IdTarea == entity.IdTareaPreviaRequerida)
            {
                return ResultVM<TareaPrevia>.Fail("Una tarea no puede ser previa de sí misma");
            }

            // Validar grafo acíclico con datos reales de BD
            var actuales = await _adapter.ObtenerTodasAsync();
            var simuladas = actuales.ToList();
            simuladas.Add(new TareaPrevia
            {
                IdTarea = entity.IdTarea,
                IdTareaPreviaRequerida = entity.IdTareaPreviaRequerida,
                Orden = entity.Orden
            });

            var esAciclico = _grafo.ValidarNoCiclos(
                simuladas,
                getId: x => x.IdTarea,
                getIdPrevia: x => x.IdTareaPreviaRequerida
            );

            if (!esAciclico)
            {
                return ResultVM<TareaPrevia>.Fail(
                    "La relación crea un ciclo de dependencias. " +
                    $"Tarea {entity.IdTarea} → Previa {entity.IdTareaPreviaRequerida} genera ciclo."
                );
            }

            try
            {
                _db.TareasPrevias.Add(entity);
                await _db.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_TareasPrevias",
                    EntidadId = entity.Id,
                    Accion = "CREATE",
                    Detalles = $"Tarea={entity.IdTarea} requiere Previa={entity.IdTareaPreviaRequerida}, Orden={entity.Orden}"
                });

                return ResultVM<TareaPrevia>.Ok(entity, "Precedencia creada exitosamente");
            }
            catch (Exception ex)
            {
                return ResultVM<TareaPrevia>.Fail($"Error al crear precedencia: {ex.Message}");
            }
        }

        public async Task<ResultVM<bool>> EliminarAsync(long id)
        {
            var entity = await _db.TareasPrevias.FindAsync(id);
            if (entity == null)
            {
                return ResultVM<bool>.Fail("Precedencia no encontrada");
            }

            try
            {
                _db.TareasPrevias.Remove(entity);
                await _db.SaveChangesAsync();

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_TareasPrevias",
                    EntidadId = id,
                    Accion = "DELETE",
                    Detalles = $"Tarea={entity.IdTarea}, Previa={entity.IdTareaPreviaRequerida}"
                });

                return ResultVM<bool>.Ok(true, "Precedencia eliminada");
            }
            catch (Exception ex)
            {
                return ResultVM<bool>.Fail($"Error al eliminar: {ex.Message}");
            }
        }

        public async Task<IEnumerable<TareaPrevia>> ObtenerPorTareaAsync(long idTarea)
        {
            return await _adapter.ObtenerPorTareaAsync(idTarea);
        }

        public async Task<IEnumerable<TareaPrevia>> ObtenerTodasAsync()
        {
            return await _adapter.ObtenerTodasAsync();
        }
    }
}
