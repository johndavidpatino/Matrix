using MatrixNext.Web.ViewModels;
using MatrixNext.Web.ViewModels.CORE;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.CORE
{
    /// <summary>
    /// Servicio de dominio para documentos requeridos por tarea (Configuracion_Tareas_Documentos)
    /// </summary>
    public interface ITareasDocumentosService
    {
        Task<ResultVM<IEnumerable<DocumentoPorTareaVM>>> ObtenerAsync(long tareaId, short tipoDocumentoTareaId, bool? asignado = null);
        Task<ResultVM<bool>> AsignarAsync(long tareaId, long documentoId, short tipoDocumentoTareaId, bool esOpcional, long usuarioId);
        Task<ResultVM<bool>> DesasignarAsync(long tareaId, long documentoId, short tipoDocumentoTareaId, long usuarioId);
    }

    public class TareasDocumentosService : ITareasDocumentosService
    {
        private readonly TareasDocumentosDataAdapter _adapter;
        private readonly IAuditoriaService _auditoria;
        private readonly ILogger<TareasDocumentosService> _logger;

        public TareasDocumentosService(
            TareasDocumentosDataAdapter adapter,
            IAuditoriaService auditoria,
            ILogger<TareasDocumentosService> logger)
        {
            _adapter = adapter;
            _auditoria = auditoria;
            _logger = logger;
        }

        public async Task<ResultVM<IEnumerable<DocumentoPorTareaVM>>> ObtenerAsync(long tareaId, short tipoDocumentoTareaId, bool? asignado = null)
        {
            if (tareaId <= 0)
            {
                return ResultVM<IEnumerable<DocumentoPorTareaVM>>.Fail("La tarea es obligatoria");
            }

            var data = await _adapter.ObtenerAsync(tareaId, tipoDocumentoTareaId, asignado);
            return ResultVM<IEnumerable<DocumentoPorTareaVM>>.Ok(data);
        }

        public async Task<ResultVM<bool>> AsignarAsync(long tareaId, long documentoId, short tipoDocumentoTareaId, bool esOpcional, long usuarioId)
        {
            if (tareaId <= 0 || documentoId <= 0)
            {
                return ResultVM<bool>.Fail("Datos inválidos para la asignación");
            }

            try
            {
                var inserted = await _adapter.AsignarAsync(tareaId, documentoId, tipoDocumentoTareaId, esOpcional);
                if (!inserted)
                {
                    return ResultVM<bool>.Ok(true, "El documento ya estaba asignado a la tarea");
                }

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_Tareas_Documentos",
                    EntidadId = documentoId,
                    Accion = "ASSIGN",
                    Detalles = $"Asignar DocumentoId={documentoId} a TareaId={tareaId} (TipoDocumentoTareaId={tipoDocumentoTareaId}, Opcional={esOpcional})",
                    IdUsuario = usuarioId,
                    RutaArchivo = string.Empty
                });

                return ResultVM<bool>.Ok(true, "Documento asignado a la tarea");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error asignando documento {DocumentoId} a tarea {TareaId}", documentoId, tareaId);
                return ResultVM<bool>.Fail("Error al asignar el documento. Por favor intente nuevamente.");
            }
        }

        public async Task<ResultVM<bool>> DesasignarAsync(long tareaId, long documentoId, short tipoDocumentoTareaId, long usuarioId)
        {
            if (tareaId <= 0 || documentoId <= 0)
            {
                return ResultVM<bool>.Fail("Datos inválidos para la desasignación");
            }

            try
            {
                var deleted = await _adapter.DesasignarAsync(tareaId, documentoId, tipoDocumentoTareaId);
                if (!deleted)
                {
                    return ResultVM<bool>.Fail("El documento no estaba asignado a esta tarea");
                }

                await _auditoria.LogearAsync(new AuditoriaVM
                {
                    Entidad = "CORE_Tareas_Documentos",
                    EntidadId = documentoId,
                    Accion = "UNASSIGN",
                    Detalles = $"Desasignar DocumentoId={documentoId} de TareaId={tareaId} (TipoDocumentoTareaId={tipoDocumentoTareaId})",
                    IdUsuario = usuarioId,
                    RutaArchivo = string.Empty
                });

                return ResultVM<bool>.Ok(true, "Documento desasignado de la tarea");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desasignando documento {DocumentoId} de tarea {TareaId}", documentoId, tareaId);
                return ResultVM<bool>.Fail("Error al desasignar el documento. Por favor intente nuevamente.");
            }
        }
    }
}
