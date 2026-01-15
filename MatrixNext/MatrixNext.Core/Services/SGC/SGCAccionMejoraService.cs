using Microsoft.Extensions.Logging;
using MatrixNext.Infrastructure.Adapters.SGC;
using MatrixNext.Infrastructure.DTOs.SGC;

namespace MatrixNext.Core.Services.SGC
{
    /// <summary>
    /// Implementación de servicio para Acciones de Mejora
    /// Contiene lógica de negocio, validaciones y seguimiento
    /// </summary>
    public class SGCAccionMejoraService : ISGCAccionMejoraService
    {
        private readonly ISGCAccionMejoraAdapter _adapter;
        private readonly ILogger<SGCAccionMejoraService> _logger;

        public SGCAccionMejoraService(ISGCAccionMejoraAdapter adapter, ILogger<SGCAccionMejoraService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        /// <summary>
        /// Crear nueva acción de mejora
        /// </summary>
        public async Task<(bool Success, string Message)> CreateAsync(SGCAccionMejoraCreateDto dto, long userId)
        {
            try
            {
                // Validar datos
                var (isValid, errorMessage) = await ValidateCreateAsync(dto);
                if (!isValid)
                {
                    _logger.LogWarning("Validación fallida al crear acción mejora. Usuario: {UserId}, Error: {Error}", userId, errorMessage);
                    return (false, errorMessage);
                }

                // Crear acción
                var accionId = await _adapter.CreateAsync(dto, userId);

                _logger.LogInformation("Acción mejora {AccionId} creada por usuario {UserId}", accionId, userId);
                return (true, "Acción de mejora creada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear acción mejora para usuario {UserId}", userId);
                return (false, "Error al crear la acción");
            }
        }

        /// <summary>
        /// Obtener acción de mejora por ID
        /// </summary>
        public async Task<SGCAccionMejoraDto> GetByIdAsync(int accionMejoraId)
        {
            try
            {
                var accion = await _adapter.GetByIdAsync(accionMejoraId);
                return accion;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener acción mejora {AccionId}", accionMejoraId);
                throw;
            }
        }

        /// <summary>
        /// Listar acciones de mejora con filtros
        /// </summary>
        public async Task<List<SGCAccionMejoraDto>> GetByFilterAsync(int? procesoId, long? usuarioResponsable, int pageSize, int pageIndex)
        {
            try
            {
                var acciones = await _adapter.GetByFilterAsync(procesoId, usuarioResponsable, null, pageSize, pageIndex);

                _logger.LogInformation("Acciones mejora obtenidas. Filtros: Proceso={Proceso}, Usuario={Usuario}, Página={Pagina}", 
                    procesoId, usuarioResponsable, pageIndex);

                return acciones;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar acciones mejora");
                throw;
            }
        }

        /// <summary>
        /// Actualizar acción de mejora
        /// </summary>
        public async Task<(bool Success, string Message)> UpdateAsync(SGCAccionMejoraUpdateDto dto, long userId)
        {
            try
            {
                var accion = await _adapter.GetByIdAsync(dto.AccionMejoraId);
                if (accion == null)
                    return (false, "Acción no encontrada");

                var result = await _adapter.UpdateAsync(dto, userId);

                _logger.LogInformation("Acción mejora {AccionId} actualizada por usuario {UserId}", dto.AccionMejoraId, userId);
                return (true, "Acción actualizada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar acción mejora {AccionId}", dto.AccionMejoraId);
                return (false, "Error al actualizar la acción");
            }
        }

        /// <summary>
        /// Eliminar acción de mejora (soft delete)
        /// </summary>
        public async Task<(bool Success, string Message)> DeleteAsync(int accionMejoraId, long userId)
        {
            try
            {
                var accion = await _adapter.GetByIdAsync(accionMejoraId);
                if (accion == null)
                    return (false, "Acción no encontrada");

                var result = await _adapter.DeleteAsync(accionMejoraId, userId);

                _logger.LogInformation("Acción mejora {AccionId} eliminada por usuario {UserId}", accionMejoraId, userId);
                return (true, "Acción eliminada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar acción mejora {AccionId}", accionMejoraId);
                return (false, "Error al eliminar la acción");
            }
        }

        /// <summary>
        /// Agregar causas a una acción
        /// </summary>
        public async Task<(bool Success, string Message)> AddCausasAsync(int accionMejoraId, List<SGCCausaCreateDto> causas, long userId)
        {
            try
            {
                if (!causas.Any())
                    return (false, "Debe agregar al menos una causa");

                var result = await _adapter.AddCausasAsync(accionMejoraId, causas);

                _logger.LogInformation("Se agregaron {Count} causas a acción mejora {AccionId} por usuario {UserId}", 
                    causas.Count, accionMejoraId, userId);

                return (true, "Causas agregadas exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar causas a acción mejora {AccionId}", accionMejoraId);
                return (false, "Error al agregar causas");
            }
        }

        /// <summary>
        /// Eliminar causa
        /// </summary>
        public async Task<(bool Success, string Message)> DeleteCausaAsync(int causaId, long userId)
        {
            try
            {
                var result = await _adapter.DeleteCausaAsync(causaId, userId);

                _logger.LogInformation("Causa {CausaId} eliminada por usuario {UserId}", causaId, userId);
                return (true, "Causa eliminada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar causa {CausaId}", causaId);
                return (false, "Error al eliminar la causa");
            }
        }

        /// <summary>
        /// Agregar planes de acción
        /// </summary>
        public async Task<(bool Success, string Message)> AddPlanesAccionAsync(int accionMejoraId, List<SGCPlanAccionCreateDto> planes, long userId)
        {
            try
            {
                if (!planes.Any())
                    return (false, "Debe agregar al menos un plan de acción");

                // Validar fechas planeadas
                foreach (var plan in planes)
                {
                    if (plan.FechaPlaneado < DateTime.Today)
                        return (false, "Fecha planeada no puede ser anterior a hoy");
                }

                var result = await _adapter.AddPlanesAccionAsync(accionMejoraId, planes);

                _logger.LogInformation("Se agregaron {Count} planes de acción a acción mejora {AccionId} por usuario {UserId}", 
                    planes.Count, accionMejoraId, userId);

                return (true, "Planes de acción agregados exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al agregar planes acción para acción mejora {AccionId}", accionMejoraId);
                return (false, "Error al agregar planes de acción");
            }
        }

        /// <summary>
        /// Actualizar plan de acción
        /// </summary>
        public async Task<(bool Success, string Message)> UpdatePlanAccionAsync(SGCPlanAccionUpdateDto dto, long userId)
        {
            try
            {
                if (dto.FechaPlaneado < DateTime.Today)
                    return (false, "Fecha planeada no puede ser anterior a hoy");

                var result = await _adapter.UpdatePlanAccionAsync(dto, userId);

                _logger.LogInformation("Plan acción {PlanId} actualizado por usuario {UserId}", dto.PlanAccionId, userId);
                return (true, "Plan actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar plan acción {PlanId}", dto.PlanAccionId);
                return (false, "Error al actualizar el plan");
            }
        }

        /// <summary>
        /// Eliminar plan de acción
        /// </summary>
        public async Task<(bool Success, string Message)> DeletePlanAccionAsync(int planAccionId, long userId)
        {
            try
            {
                var result = await _adapter.DeletePlanAccionAsync(planAccionId, userId);

                _logger.LogInformation("Plan acción {PlanId} eliminado por usuario {UserId}", planAccionId, userId);
                return (true, "Plan eliminado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar plan acción {PlanId}", planAccionId);
                return (false, "Error al eliminar el plan");
            }
        }

        /// <summary>
        /// Obtener planes vencidos para alertas
        /// </summary>
        public async Task<List<SGCPlanAccionDto>> GetPlanesAccionVencidosAsync()
        {
            try
            {
                // Esto podría ser implementado con una consulta SQL específica
                // Por ahora retorna lista vacía como placeholder
                _logger.LogInformation("Verificando planes de acción vencidos");
                return new List<SGCPlanAccionDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener planes vencidos");
                throw;
            }
        }

        /// <summary>
        /// Validar datos de creación de acción de mejora
        /// </summary>
        public async Task<(bool IsValid, string ErrorMessage)> ValidateCreateAsync(SGCAccionMejoraCreateDto dto)
        {
            // Validar campos requeridos
            if (string.IsNullOrWhiteSpace(dto.DescripcionAccion))
                return (false, "Descripción de acción es requerida");

            if (dto.DescripcionAccion.Length > 1000)
                return (false, "Descripción no puede exceder 1000 caracteres");

            if (dto.FechaIncidente > DateTime.Now)
                return (false, "Fecha de incidente no puede ser futura");

            if (dto.UsuarioReporta <= 0)
                return (false, "Usuario que reporta no es válido");

            if (dto.ProcesoId <= 0)
                return (false, "Proceso no es válido");

            if (dto.UsuarioResponsable <= 0)
                return (false, "Usuario responsable no es válido");

            return (true, "");
        }

        /// <summary>
        /// Obtener procesos disponibles
        /// </summary>
        public async Task<List<SGCProcesoDto>> GetProcesosAsync()
        {
            return await _adapter.GetProcesosAsync();
        }

        /// <summary>
        /// Obtener fuentes de no conformidad
        /// </summary>
        public async Task<List<SGCFuenteNoConformidadDto>> GetFuentesNoConformidadAsync()
        {
            return await _adapter.GetFuentesNoConformidadAsync();
        }

        /// <summary>
        /// Obtener fuentes específicas por tipo
        /// </summary>
        public async Task<List<SGCFuenteDto>> GetFuentesByTypeAsync(int fuenteNoConformidadId)
        {
            return await _adapter.GetFuentesByTypeAsync(fuenteNoConformidadId);
        }
    }
}
