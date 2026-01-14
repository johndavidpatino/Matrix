using MatrixNext.Data.Models.OP_RO;
using MatrixNext.Data.Services;
using MatrixNext.Data.Adapters.OP_RO;
using MatrixNext.Data.Services.Authorization;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.OP_RO
{
    /// <summary>
    /// Interfaz Service para Operational Review
    /// Orquesta workflow: Pendiente → Aprobado/Rechazado
    /// Gestiona: Cuestionarios, Instructivos, Metodologías, Materiales
    /// REGLA 6: Validaciones complejas
    /// REGLA 7: Transformación datos
    /// REGLA 8: Gestión errores
    /// </summary>
    public interface IOP_ROService
    {
        // ============================================
        // WORKFLOW DE REVISIÓN
        // ============================================

        /// <summary>
        /// Obtiene listado de revisiones con estado y filtros
        /// </summary>
        Task<ApiResponse<OP_ROResultadoDTO>> ObtenerRevisionesAsync(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Obtiene una revisión específica con historial
        /// </summary>
        Task<ApiResponse<OP_ROSolicitudRevisionDTO>> ObtenerRevisionDetalleAsync(int reviewId);

        /// <summary>
        /// Aprueba una revisión (cambia estado a Aprobado)
        /// State Machine: Pendiente/EnRevisión → Aprobado
        /// </summary>
        Task<ApiResponse<string>> AprobarRevisionAsync(OP_ROAprobarDTO aprobacion);

        /// <summary>
        /// Rechaza una revisión (cambia estado a Rechazado)
        /// State Machine: Pendiente/EnRevisión → Rechazado
        /// </summary>
        Task<ApiResponse<string>> RechazarRevisionAsync(OP_RORechazarDTO rechazo);

        // ============================================
        // GESTIÓN DE CUESTIONARIOS
        // ============================================

        /// <summary>
        /// Obtiene listado de cuestionarios disponibles
        /// </summary>
        Task<ApiResponse<List<OP_ROCuestionarioDTO>>> ObtenerCuestionariosAsync(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Obtiene cuestionario con preguntas
        /// </summary>
        Task<ApiResponse<OP_ROCuestionarioDTO>> ObtenerCuestionarioAsync(int cuestionarioId);

        /// <summary>
        /// Crea o actualiza cuestionario
        /// Genera nueva versión
        /// </summary>
        Task<ApiResponse<int>> GuardarCuestionarioAsync(OP_ROCuestionarioDTO cuestionario, int usuarioId);

        // ============================================
        // GESTIÓN DE INSTRUCTIVOS
        // ============================================

        /// <summary>
        /// Obtiene listado de instructivos
        /// </summary>
        Task<ApiResponse<List<OP_ROInstructivoDTO>>> ObtenerInstructivosAsync(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Obtiene instructivo con pasos
        /// </summary>
        Task<ApiResponse<OP_ROInstructivoDTO>> ObtenerInstructivoAsync(int instructivoId);

        /// <summary>
        /// Crea o actualiza instructivo
        /// </summary>
        Task<ApiResponse<int>> GuardarInstructivoAsync(OP_ROInstructivoDTO instructivo, int usuarioId);

        // ============================================
        // GESTIÓN DE METODOLOGÍAS
        // ============================================

        /// <summary>
        /// Obtiene listado de metodologías
        /// </summary>
        Task<ApiResponse<List<OP_ROMetodologiaDTO>>> ObtenerMetodologiasAsync(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Obtiene metodología con fases
        /// </summary>
        Task<ApiResponse<OP_ROMetodologiaDTO>> ObtenerMetodologiaAsync(int metodologiaId);

        /// <summary>
        /// Crea o actualiza metodología
        /// </summary>
        Task<ApiResponse<int>> GuardarMetodologiaAsync(OP_ROMetodologiaDTO metodologia, int usuarioId);

        // ============================================
        // GESTIÓN DE MATERIALES
        // ============================================

        /// <summary>
        /// Obtiene listado de materiales de ayuda
        /// </summary>
        Task<ApiResponse<List<OP_ROMaterialAyudaDTO>>> ObtenerMaterialesAsync(OP_ROFiltrosDTO filtros);

        /// <summary>
        /// Obtiene material específico
        /// </summary>
        Task<ApiResponse<OP_ROMaterialAyudaDTO>> ObtenerMaterialAsync(int materialId);

        /// <summary>
        /// Crea o actualiza material
        /// </summary>
        Task<ApiResponse<int>> GuardarMaterialAsync(OP_ROMaterialAyudaDTO material, int usuarioId);

        // ============================================
        // VALIDACIONES Y STATE MACHINE
        // ============================================

        /// <summary>
        /// Valida transición de estado según state machine
        /// Pendiente → [Aprobado, Rechazado, Cancelado]
        /// EnRevisión → [Aprobado, Rechazado]
        /// </summary>
        Task<bool> ValidarTransicionEstadoAsync(string estadoActual, string estadoNuevo, int usuarioId);

        /// <summary>
        /// Verifica permisos para accionar sobre revisión
        /// </summary>
        Task<bool> ValidarPermisoAsync(int reviewId, int usuarioId, string accion);
    }

    /// <summary>
    /// Implementación Service para Operational Review
    /// Implementa workflow de estado máquina
    /// </summary>
    public class OP_ROService : IOP_ROService
    {
        private readonly IOP_ROAdapter _adapter;
        private readonly IAuthorizationService _authService;
        private readonly ILogger<OP_ROService> _logger;

        public OP_ROService(
            IOP_ROAdapter adapter, 
            IAuthorizationService authService,
            ILogger<OP_ROService> logger)
        {
            _adapter = adapter;
            _authService = authService;
            _logger = logger;
        }

        // ============================================
        // WORKFLOW DE REVISIÓN
        // ============================================

        public async Task<ApiResponse<OP_ROResultadoDTO>> ObtenerRevisionesAsync(OP_ROFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_ROService] Obteniendo revisiones");

                _adapter.ValidarFiltros(filtros);

                var revisiones = await _adapter.GetRevisionesAsync(filtros);

                if (!revisiones.Any())
                    return ApiResponse<OP_ROResultadoDTO>.Ok(
                        new OP_ROResultadoDTO { Datos = new(), TotalRegistros = 0 },
                        "Sin revisiones para los filtros especificados");

                var totalRegistros = revisiones.Count;
                var totalPaginas = (int)Math.Ceiling((decimal)totalRegistros / filtros.PageSize);

                var resultado = new OP_ROResultadoDTO
                {
                    Datos = revisiones,
                    TotalRegistros = totalRegistros,
                    Pagina = filtros.PageNumber,
                    RegistrosPorPagina = filtros.PageSize,
                    TotalPaginas = totalPaginas,
                    TienePaginas = filtros.PageNumber < totalPaginas
                };

                return ApiResponse<OP_ROResultadoDTO>.Ok(resultado, "Revisiones obtenidas");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en ObtenerRevisiones");
                return ApiResponse<OP_ROResultadoDTO>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<OP_ROSolicitudRevisionDTO>> ObtenerRevisionDetalleAsync(int reviewId)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Obteniendo detalle revisión: {reviewId}");

                var revision = await _adapter.GetRevisionByIdAsync(reviewId);
                if (revision == null || revision.ReviewId == 0)
                    return ApiResponse<OP_ROSolicitudRevisionDTO>.NotFound("Revisión no encontrada");

                // Construir DTO de solicitud con historial
                var solicitud = new OP_ROSolicitudRevisionDTO
                {
                    SolicitudId = revision.ReviewId,
                    ReviewId = revision.ReviewId,
                    TipoRevision = revision.TipoRevision,
                    NombreDocumento = revision.NombreDocumento,
                    UsuarioSolicitanteId = revision.UsuarioCreadorId,
                    UsuarioSolicitante = revision.UsuarioCreador,
                    FechaSolicitud = revision.FechaCreacion,
                    EstadoActual = revision.Estado,
                    Historial = await _adapter.GetHistorialRevisionAsync(reviewId)
                };

                return ApiResponse<OP_ROSolicitudRevisionDTO>.Ok(solicitud, "Detalle obtenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en ObtenerRevisionDetalle");
                return ApiResponse<OP_ROSolicitudRevisionDTO>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> AprobarRevisionAsync(OP_ROAprobarDTO aprobacion)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Aprobando revisión: {aprobacion.ReviewId}");

                // REGLA 6: Validación
                if (aprobacion == null)
                    return ApiResponse<string>.BadRequest("Datos de aprobación inválidos");

                // Obtener revisión actual
                var revision = await _adapter.GetRevisionByIdAsync(aprobacion.ReviewId);
                if (revision == null || revision.ReviewId == 0)
                    return ApiResponse<string>.NotFound("Revisión no encontrada");

                // State Machine: Validar transición
                if (!await ValidarTransicionEstadoAsync(revision.Estado ?? string.Empty, EstadosRevision.APROBADO, aprobacion.UsuarioRevisorId))
                    return ApiResponse<string>.BadRequest($"No se puede pasar de {revision.Estado} a {EstadosRevision.APROBADO}");

                // Validar permisos
                if (!await ValidarPermisoAsync(aprobacion.ReviewId, aprobacion.UsuarioRevisorId, "APROBAR"))
                    return ApiResponse<string>.Unauthorized("No tiene permisos para aprobar esta revisión");

                // Ejecutar aprobación
                var resultado = await _adapter.AprobarRevisionAsync(aprobacion);
                if (!resultado)
                    return ApiResponse<string>.Error("Error al aprobar revisión");

                _logger.LogInformation($"[OP_ROService] Revisión {aprobacion.ReviewId} aprobada por usuario {aprobacion.UsuarioRevisorId}");

                return ApiResponse<string>.Ok($"Revisión #{aprobacion.ReviewId} aprobada", "Aprobación registrada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en AprobarRevision");
                return ApiResponse<string>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> RechazarRevisionAsync(OP_RORechazarDTO rechazo)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Rechazando revisión: {rechazo.ReviewId}");

                // REGLA 6: Validación
                if (rechazo == null || string.IsNullOrEmpty(rechazo.MotivoRechazo))
                    return ApiResponse<string>.BadRequest("Datos de rechazo inválidos - motivo requerido");

                // Obtener revisión actual
                var revision = await _adapter.GetRevisionByIdAsync(rechazo.ReviewId);
                if (revision == null || revision.ReviewId == 0)
                    return ApiResponse<string>.NotFound("Revisión no encontrada");

                // State Machine: Validar transición
                if (!await ValidarTransicionEstadoAsync(revision.Estado ?? string.Empty, EstadosRevision.RECHAZADO, rechazo.UsuarioRevisorId))
                    return ApiResponse<string>.BadRequest($"No se puede pasar de {revision.Estado} a {EstadosRevision.RECHAZADO}");

                // Validar permisos
                if (!await ValidarPermisoAsync(rechazo.ReviewId, rechazo.UsuarioRevisorId, "RECHAZAR"))
                    return ApiResponse<string>.Unauthorized("No tiene permisos para rechazar esta revisión");

                // Ejecutar rechazo
                var resultado = await _adapter.RechazarRevisionAsync(rechazo);
                if (!resultado)
                    return ApiResponse<string>.Error("Error al rechazar revisión");

                _logger.LogInformation($"[OP_ROService] Revisión {rechazo.ReviewId} rechazada: {rechazo.MotivoRechazo}");

                return ApiResponse<string>.Ok($"Revisión #{rechazo.ReviewId} rechazada", "Rechazo registrado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en RechazarRevision");
                return ApiResponse<string>.Error(ex.Message);
            }
        }

        // ============================================
        // GESTIÓN DE CUESTIONARIOS
        // ============================================

        public async Task<ApiResponse<List<OP_ROCuestionarioDTO>>> ObtenerCuestionariosAsync(OP_ROFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_ROService] Obteniendo cuestionarios");

                _adapter.ValidarFiltros(filtros);

                var cuestionarios = await _adapter.GetCuestionariosAsync(filtros);

                return ApiResponse<List<OP_ROCuestionarioDTO>>.Ok(
                    cuestionarios ?? new(),
                    $"{cuestionarios?.Count ?? 0} cuestionarios obtenidos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en ObtenerCuestionarios");
                return ApiResponse<List<OP_ROCuestionarioDTO>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<OP_ROCuestionarioDTO>> ObtenerCuestionarioAsync(int cuestionarioId)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Obteniendo cuestionario: {cuestionarioId}");

                var cuestionario = await _adapter.GetCuestionarioByIdAsync(cuestionarioId);

                if (cuestionario == null || cuestionario.CuestionarioId == 0)
                    return ApiResponse<OP_ROCuestionarioDTO>.NotFound("Cuestionario no encontrado");

                return ApiResponse<OP_ROCuestionarioDTO>.Ok(cuestionario, "Cuestionario obtenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en ObtenerCuestionario");
                return ApiResponse<OP_ROCuestionarioDTO>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> GuardarCuestionarioAsync(OP_ROCuestionarioDTO cuestionario, int usuarioId)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Guardando cuestionario: {cuestionario?.Titulo}");

                // REGLA 6: Validación
                if (cuestionario == null || string.IsNullOrEmpty(cuestionario.Titulo))
                    return ApiResponse<int>.BadRequest("Datos inválidos - título requerido", 0);

                _adapter.ValidarDatos(cuestionario);

                // REGLA 7: Transformación - generar nueva versión
                cuestionario.VersionId = cuestionario.VersionId + 1;

                // Guardar
                var id = await _adapter.SaveCuestionarioAsync(cuestionario);

                _logger.LogInformation($"[OP_ROService] Cuestionario guardado: {id}");

                return ApiResponse<int>.Ok(id, $"Cuestionario guardado - ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en GuardarCuestionario");
                return ApiResponse<int>.Error(ex.Message, 0);
            }
        }

        // ============================================
        // GESTIÓN DE INSTRUCTIVOS
        // ============================================

        public async Task<ApiResponse<List<OP_ROInstructivoDTO>>> ObtenerInstructivosAsync(OP_ROFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_ROService] Obteniendo instructivos");

                _adapter.ValidarFiltros(filtros);

                var instructivos = await _adapter.GetInstructivosAsync(filtros);

                return ApiResponse<List<OP_ROInstructivoDTO>>.Ok(
                    instructivos ?? new(),
                    $"{instructivos?.Count ?? 0} instructivos obtenidos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en ObtenerInstructivos");
                return ApiResponse<List<OP_ROInstructivoDTO>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<OP_ROInstructivoDTO>> ObtenerInstructivoAsync(int instructivoId)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Obteniendo instructivo: {instructivoId}");

                var instructivo = await _adapter.GetInstructivoByIdAsync(instructivoId);

                if (instructivo == null || instructivo.InstructivoId == 0)
                    return ApiResponse<OP_ROInstructivoDTO>.NotFound("Instructivo no encontrado");

                return ApiResponse<OP_ROInstructivoDTO>.Ok(instructivo, "Instructivo obtenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en ObtenerInstructivo");
                return ApiResponse<OP_ROInstructivoDTO>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> GuardarInstructivoAsync(OP_ROInstructivoDTO instructivo, int usuarioId)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Guardando instructivo: {instructivo?.Titulo}");

                if (instructivo == null || string.IsNullOrEmpty(instructivo.Titulo))
                    return ApiResponse<int>.BadRequest("Datos inválidos - título requerido", 0);

                _adapter.ValidarDatos(instructivo);

                instructivo.VersionId = instructivo.VersionId + 1;

                var id = await _adapter.SaveInstructivoAsync(instructivo);

                return ApiResponse<int>.Ok(id, $"Instructivo guardado - ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en GuardarInstructivo");
                return ApiResponse<int>.Error(ex.Message, 0);
            }
        }

        // ============================================
        // GESTIÓN DE METODOLOGÍAS
        // ============================================

        public async Task<ApiResponse<List<OP_ROMetodologiaDTO>>> ObtenerMetodologiasAsync(OP_ROFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_ROService] Obteniendo metodologías");

                _adapter.ValidarFiltros(filtros);

                var metodologias = await _adapter.GetMetodologiasAsync(filtros);

                return ApiResponse<List<OP_ROMetodologiaDTO>>.Ok(
                    metodologias ?? new(),
                    $"{metodologias?.Count ?? 0} metodologías obtenidas");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en ObtenerMetodologias");
                return ApiResponse<List<OP_ROMetodologiaDTO>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<OP_ROMetodologiaDTO>> ObtenerMetodologiaAsync(int metodologiaId)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Obteniendo metodología: {metodologiaId}");

                var metodologia = await _adapter.GetMetodologiaByIdAsync(metodologiaId);

                if (metodologia == null || metodologia.MetodologiaId == 0)
                    return ApiResponse<OP_ROMetodologiaDTO>.NotFound("Metodología no encontrada");

                return ApiResponse<OP_ROMetodologiaDTO>.Ok(metodologia, "Metodología obtenida");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en ObtenerMetodologia");
                return ApiResponse<OP_ROMetodologiaDTO>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> GuardarMetodologiaAsync(OP_ROMetodologiaDTO metodologia, int usuarioId)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Guardando metodología: {metodologia?.Nombre}");

                if (metodologia == null || string.IsNullOrEmpty(metodologia.Nombre))
                    return ApiResponse<int>.BadRequest("Datos inválidos - nombre requerido", 0);

                _adapter.ValidarDatos(metodologia);

                metodologia.VersionId = metodologia.VersionId + 1;

                var id = await _adapter.SaveMetodologiaAsync(metodologia);

                return ApiResponse<int>.Ok(id, $"Metodología guardada - ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en GuardarMetodologia");
                return ApiResponse<int>.Error(ex.Message, 0);
            }
        }

        // ============================================
        // GESTIÓN DE MATERIALES
        // ============================================

        public async Task<ApiResponse<List<OP_ROMaterialAyudaDTO>>> ObtenerMaterialesAsync(OP_ROFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_ROService] Obteniendo materiales");

                _adapter.ValidarFiltros(filtros);

                var materiales = await _adapter.GetMaterialesAsync(filtros);

                return ApiResponse<List<OP_ROMaterialAyudaDTO>>.Ok(
                    materiales ?? new(),
                    $"{materiales?.Count ?? 0} materiales obtenidos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en ObtenerMateriales");
                return ApiResponse<List<OP_ROMaterialAyudaDTO>>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<OP_ROMaterialAyudaDTO>> ObtenerMaterialAsync(int materialId)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Obteniendo material: {materialId}");

                var material = await _adapter.GetMaterialByIdAsync(materialId);

                if (material == null || material.MaterialId == 0)
                    return ApiResponse<OP_ROMaterialAyudaDTO>.NotFound("Material no encontrado");

                return ApiResponse<OP_ROMaterialAyudaDTO>.Ok(material, "Material obtenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en ObtenerMaterial");
                return ApiResponse<OP_ROMaterialAyudaDTO>.Error(ex.Message);
            }
        }

        public async Task<ApiResponse<int>> GuardarMaterialAsync(OP_ROMaterialAyudaDTO material, int usuarioId)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Guardando material: {material?.Titulo}");

                if (material == null || string.IsNullOrEmpty(material.Titulo))
                    return ApiResponse<int>.BadRequest("Datos inválidos - título requerido", 0);

                _adapter.ValidarDatos(material);

                material.VersionId = material.VersionId + 1;

                var id = await _adapter.SaveMaterialAsync(material);

                return ApiResponse<int>.Ok(id, $"Material guardado - ID: {id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_ROService] Error en GuardarMaterial");
                return ApiResponse<int>.Error(ex.Message, 0);
            }
        }

        // ============================================
        // VALIDACIONES Y STATE MACHINE
        // ============================================

        public async Task<bool> ValidarTransicionEstadoAsync(string estadoActual, string estadoNuevo, int usuarioId)
        {
            // STATE MACHINE: Validar transiciones permitidas
            // Pendiente → [AprobadoRechazadoCancelado]
            // EnRevisión → [Aprobado, Rechazado]
            // Aprobado → [Cancelado]
            // Rechazado → [Cancelado]

            _logger.LogInformation($"[OP_ROService] Validando transición: {estadoActual} → {estadoNuevo}");

            var transicionesValidas = new Dictionary<string, List<string>>
            {
                { EstadosRevision.PENDIENTE, new() { EstadosRevision.EN_REVISIÓN, EstadosRevision.APROBADO, EstadosRevision.RECHAZADO, EstadosRevision.CANCELADO } },
                { EstadosRevision.EN_REVISIÓN, new() { EstadosRevision.APROBADO, EstadosRevision.RECHAZADO, EstadosRevision.CANCELADO } },
                { EstadosRevision.APROBADO, new() { EstadosRevision.CANCELADO } },
                { EstadosRevision.RECHAZADO, new() { EstadosRevision.CANCELADO } },
                { EstadosRevision.CANCELADO, new() }
            };

            if (!transicionesValidas.ContainsKey(estadoActual))
            {
                _logger.LogWarning($"[OP_ROService] Estado actual no reconocido: {estadoActual}");
                return false;
            }

            var permitidas = transicionesValidas[estadoActual];
            var esValida = permitidas.Contains(estadoNuevo);

            _logger.LogInformation($"[OP_ROService] Transición {(esValida ? "VÁLIDA" : "INVÁLIDA")}");

            return await Task.FromResult(esValida);
        }

        public async Task<bool> ValidarPermisoAsync(int reviewId, int usuarioId, string accion)
        {
            try
            {
                _logger.LogInformation($"[OP_ROService] Validando permisos: usuario {usuarioId}, acción {accion}");
                return await _authService.ValidarPermisoAsync(usuarioId, "Revision", accion, reviewId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_ROService] Error validando permisos");
                return false;
            }
        }
    }
}
