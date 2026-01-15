using Microsoft.Extensions.Logging;
using MatrixNext.Infrastructure.Adapters.SGC;
using MatrixNext.Infrastructure.DTOs.SGC;

namespace MatrixNext.Core.Services.SGC
{
    /// <summary>
    /// Implementación de servicio para Auditorías Internas
    /// Contiene lógica de negocio, validaciones, permisos y logging
    /// </summary>
    public class SGCAuditoriaService : ISGCAuditoriaService
    {
        private readonly ISGCAuditoriaAdapter _adapter;
        private readonly ILogger<SGCAuditoriaService> _logger;

        public SGCAuditoriaService(ISGCAuditoriaAdapter adapter, ILogger<SGCAuditoriaService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        /// <summary>
        /// Crear nueva auditoría con validaciones
        /// </summary>
        public async Task<(bool Success, string Message)> CreateAsync(SGCAuditoriaCreateDto dto, long userId)
        {
            try
            {
                // Validar datos de entrada
                var (isValid, errorMessage) = await ValidateCreateAsync(dto);
                if (!isValid)
                {
                    _logger.LogWarning("Validación fallida al crear auditoría. Usuario: {UserId}, Error: {Error}", userId, errorMessage);
                    return (false, errorMessage);
                }

                // Crear auditoría
                var auditoriaId = await _adapter.CreateAsync(dto, userId);

                _logger.LogInformation("Auditoría {AuditoriaId} creada por usuario {UserId}", auditoriaId, userId);
                return (true, "Auditoría creada exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear auditoría para usuario {UserId}", userId);
                return (false, "Error al crear la auditoría. Intente nuevamente.");
            }
        }

        /// <summary>
        /// Obtener auditoría por ID
        /// </summary>
        public async Task<SGCAuditoriaDto> GetByIdAsync(int auditoriaId)
        {
            try
            {
                var auditoria = await _adapter.GetByIdAsync(auditoriaId);
                return auditoria;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener auditoría {AuditoriaId}", auditoriaId);
                throw;
            }
        }

        /// <summary>
        /// Listar auditorías con filtros según rol del usuario
        /// </summary>
        public async Task<List<SGCAuditoriaDto>> GetByFilterAsync(byte? estadoId, int? anoAuditoria, int pageSize, int pageIndex, long userId, byte userRoleId)
        {
            try
            {
                long? auditorId = null;
                long? auditadoId = null;

                // Aplicar lógica de permisos por rol
                const byte ROL_CALIDAD = 45;  // Acceso total
                // ROL_AUDITOR: solo sus auditorías como auditor
                // ROL_AUDITADO: solo sus auditorías como auditado (por verificar)

                if (userRoleId != ROL_CALIDAD)
                {
                    // Si no es calidad, filtrar por usuario (auditor o auditado)
                    auditorId = userId;
                }

                var auditorias = await _adapter.GetByFilterAsync(estadoId, auditorId, anoAuditoria, auditadoId, pageSize, pageIndex);
                
                _logger.LogInformation("Auditorías obtenidas para usuario {UserId}, Filtros: Estado={Estado}, Año={Ano}, Página={Pagina}", 
                    userId, estadoId, anoAuditoria, pageIndex);

                return auditorias;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al filtrar auditorías para usuario {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// Actualizar estado de auditoría
        /// </summary>
        public async Task<(bool Success, string Message)> UpdateEstadoAsync(int auditoriaId, byte nuevoEstadoId, long userId)
        {
            try
            {
                var auditoria = await _adapter.GetByIdAsync(auditoriaId);
                if (auditoria == null)
                    return (false, "Auditoría no encontrada");

                // Validar transición de estado
                if (!IsValidStateTransition(auditoria.SGC_AI_EstadoId, nuevoEstadoId))
                    return (false, "Transición de estado no válida");

                var result = await _adapter.UpdateEstadoAsync(auditoriaId, nuevoEstadoId, userId);

                _logger.LogInformation("Auditoría {AuditoriaId} cambió de estado {OldState} a {NewState} por usuario {UserId}", 
                    auditoriaId, auditoria.SGC_AI_EstadoId, nuevoEstadoId, userId);

                return (true, "Estado actualizado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar estado auditoría {AuditoriaId}", auditoriaId);
                return (false, "Error al actualizar el estado");
            }
        }

        /// <summary>
        /// Crear informe del auditor
        /// </summary>
        public async Task<(bool Success, string Message)> CreateInformeAsync(SGCAuditoriaInformeCreateDto dto, long userId)
        {
            try
            {
                // Validar informe
                var (isValid, errorMessage) = await ValidateInformeAsync(dto);
                if (!isValid)
                {
                    _logger.LogWarning("Validación fallida al crear informe. Auditoría: {AuditoriaId}, Error: {Error}", dto.AuditoriaId, errorMessage);
                    return (false, errorMessage);
                }

                var result = await _adapter.CreateInformeAsync(dto, userId);

                _logger.LogInformation("Informe para auditoría {AuditoriaId} creado por usuario {UserId}", dto.AuditoriaId, userId);
                return (true, "Informe registrado exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear informe para auditoría {AuditoriaId}", dto.AuditoriaId);
                return (false, "Error al registrar el informe");
            }
        }

        /// <summary>
        /// Obtener informe auditor
        /// </summary>
        public async Task<SGCAuditoriaInformeDto> GetInformeByIdAsync(int auditoriaId)
        {
            try
            {
                var informe = await _adapter.GetInformeByIdAsync(auditoriaId);
                return informe;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener informe para auditoría {AuditoriaId}", auditoriaId);
                throw;
            }
        }

        /// <summary>
        /// Validar datos de creación de auditoría
        /// </summary>
        public async Task<(bool IsValid, string ErrorMessage)> ValidateCreateAsync(SGCAuditoriaCreateDto dto)
        {
            // Validar campos requeridos
            if (dto.AuditorId <= 0)
                return (false, "Auditor no válido");

            if (string.IsNullOrWhiteSpace(dto.AreaAuditada))
                return (false, "Área auditada es requerida");

            if (dto.AreaAuditada.Length > 500)
                return (false, "Área auditada no puede exceder 500 caracteres");

            if (string.IsNullOrWhiteSpace(dto.ProcesoAuditado))
                return (false, "Proceso auditado es requerido");

            if (dto.ProcesoAuditado.Length > 500)
                return (false, "Proceso auditado no puede exceder 500 caracteres");

            // Validar fecha límite (no puede ser menor a hoy)
            if (dto.FechaLimiteAuditoria < DateTime.Today)
                return (false, "Fecha límite no puede ser anterior a hoy");

            if (!dto.NormativasAAuditar.Any())
                return (false, "Debe seleccionar al menos una normativa");

            if (!dto.TiposAuditoria.Any())
                return (false, "Debe seleccionar al menos un tipo de auditoría");

            return (true, "");
        }

        /// <summary>
        /// Validar datos de informe auditor
        /// </summary>
        public async Task<(bool IsValid, string ErrorMessage)> ValidateInformeAsync(SGCAuditoriaInformeCreateDto dto)
        {
            if (dto.AuditoriaId <= 0)
                return (false, "Auditoría no válida");

            if (string.IsNullOrWhiteSpace(dto.Fortalezas))
                return (false, "Las fortalezas son requeridas");

            if (dto.Fortalezas.Length > 2000)
                return (false, "Fortalezas no pueden exceder 2000 caracteres");

            if (!dto.AuditadosIds.Any())
                return (false, "Debe registrar al menos un auditado");

            if (!dto.Hallazgos.Any())
                return (false, "Debe registrar al menos un hallazgo");

            foreach (var hallazgo in dto.Hallazgos)
            {
                if (string.IsNullOrWhiteSpace(hallazgo.Hallazgo))
                    return (false, "Descripción de hallazgo no puede estar vacía");

                if (hallazgo.TipoHallazgoId <= 0)
                    return (false, "Tipo de hallazgo no válido");
            }

            return (true, "");
        }

        /// <summary>
        /// Obtener catálogo de normativas
        /// </summary>
        public async Task<List<SGCNormativaDto>> GetNormativasAsync()
        {
            return await _adapter.GetNormativasAsync();
        }

        /// <summary>
        /// Obtener catálogo de tipos de auditoría
        /// </summary>
        public async Task<List<SGCTipoAuditoriaDto>> GetTiposAuditoriaAsync()
        {
            return await _adapter.GetTiposAuditoriaAsync();
        }

        /// <summary>
        /// Obtener catálogo de tipos de hallazgo
        /// </summary>
        public async Task<List<SGCTipoHallazgoDto>> GetTiposHallazgoAsync()
        {
            return await _adapter.GetTiposHallazgoAsync();
        }

        /// <summary>
        /// Obtener catálogo de estados
        /// </summary>
        public async Task<List<SGCEstadoAuditoriaDto>> GetEstadosAsync()
        {
            return await _adapter.GetEstadosAsync();
        }

        /// <summary>
        /// Validar transiciones de estado válidas
        /// </summary>
        private bool IsValidStateTransition(byte currentState, byte newState)
        {
            // Estados: 20=Creada, 30=Diligenciada, 40=Aprobada, 50=Cerrada
            return (currentState, newState) switch
            {
                (20, 30) => true,  // Creada → Diligenciada
                (30, 40) => true,  // Diligenciada → Aprobada
                (40, 50) => true,  // Aprobada → Cerrada
                (30, 20) => true,  // Diligenciada → Creada (revertir)
                (40, 30) => true,  // Aprobada → Diligenciada (revertir)
                _ => false
            };
        }
    }
}
