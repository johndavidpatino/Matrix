using MatrixNext.Data.Models.OP_Trafico;
using MatrixNext.Data.Services;
using MatrixNext.Data.Adapters.OP_Trafico;
using MatrixNext.Data.Services.Authorization;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.OP_Trafico
{
    /// <summary>
    /// Interfaz Service para Operational Traffic con State Machine
    /// Workflow: Capturado â†’ Criticado â†’ Verificado â†’ Anulado
    /// REGLA 6: Validaciones complejas
    /// REGLA 7: TransformaciÃ³n datos
    /// REGLA 8: GestiÃ³n errores
    /// REGLA 9: ValidaciÃ³n permisos
    /// </summary>
    public interface IOP_TraficoService
    {
        // ============================================
        // CONSULTAS GENERALES
        // ============================================

        /// <summary>
        /// Obtiene listado de eventos con estado actual
        /// </summary>
        Task<ApiResponse<OP_TraficoResultadoDTO>> ObtenerEventosAsync(OP_TraficoFiltrosDTO filtros);

        /// <summary>
        /// Obtiene evento con toda su informaciÃ³n y historial
        /// </summary>
        Task<ApiResponse<OP_TraficoEventoDTO>> ObtenerEventoDetalleAsync(int eventoId);

        // ============================================
        // TRANSICIONES DE ESTADO: STATE MACHINE
        // ============================================

        /// <summary>
        /// CAPTURA: Inicia flujo con captura de datos
        /// TransiciÃ³n: â†’ Capturado
        /// </summary>
        Task<ApiResponse<int>> CapturarAsync(OP_TraficoCapturarDTO captura);

        /// <summary>
        /// CRÃTICA: Realiza crÃ­tica de datos capturados
        /// TransiciÃ³n: Capturado â†’ Criticado
        /// </summary>
        Task<ApiResponse<string>> CriticarAsync(OP_TraficoCriticarDTO critica);

        /// <summary>
        /// VERIFICACIÃ“N: Realiza verificaciÃ³n final
        /// TransiciÃ³n: Criticado â†’ Verificado
        /// </summary>
        Task<ApiResponse<string>> VerificarAsync(OP_TraficoVerificarDTO verificacion);

        /// <summary>
        /// ANULACIÃ“N: Anula evento en cualquier estado
        /// TransiciÃ³n: [Cualquier] â†’ Anulado
        /// </summary>
        Task<ApiResponse<string>> AnularAsync(OP_TraficoAnularDTO anulacion);

        // ============================================
        // OBTENCIÃ“N DE DETALLES POR ESTADO
        // ============================================

        /// <summary>
        /// Obtiene detalles de captura
        /// </summary>
        Task<ApiResponse<OP_TraficoCapturadoDTO>> ObtenerCapturadoAsync(int eventoId);

        /// <summary>
        /// Obtiene detalles de crÃ­tica
        /// </summary>
        Task<ApiResponse<OP_TraficoCriticadoDTO>> ObtenerCriticadoAsync(int eventoId);

        /// <summary>
        /// Obtiene detalles de verificaciÃ³n
        /// </summary>
        Task<ApiResponse<OP_TraficoVerificadoDTO>> ObtenerVerificadoAsync(int eventoId);

        /// <summary>
        /// Obtiene detalles de anulaciÃ³n
        /// </summary>
        Task<ApiResponse<OP_TraficoAnuladoDTO>> ObtenerAnuladoAsync(int eventoId);

        // ============================================
        // HISTORIAL Y AUDITORÃA
        // ============================================

        /// <summary>
        /// Obtiene historial completo de transiciones
        /// </summary>
        Task<ApiResponse<List<OP_TraficoHistorialDTO>>> ObtenerHistorialAsync(int eventoId);

        // ============================================
        // DASHBOARDS Y REPORTES
        // ============================================

        /// <summary>
        /// Obtiene resumen de trÃ¡fico: estadÃ­sticas por estado
        /// </summary>
        Task<ApiResponse<OP_TraficoDashboardDTO>> ObtenerDashboardAsync(DateTime? fechaDesde = null, DateTime? fechaHasta = null);

        // ============================================
        // VALIDACIONES Y STATE MACHINE
        // ============================================

        /// <summary>
        /// Valida transiciÃ³n segÃºn state machine
        /// Capturado â†’ [Criticado, Anulado]
        /// Criticado â†’ [Verificado, Anulado]
        /// Verificado â†’ [Anulado]
        /// Anulado â†’ []
        /// </summary>
        Task<bool> ValidarTransicionEstadoAsync(string estadoActual, string estadoNuevo, int usuarioId);

        /// <summary>
        /// Valida permisos para realizar acciÃ³n
        /// </summary>
        Task<bool> ValidarPermisoAsync(int eventoId, int usuarioId, string accion);
    }

    /// <summary>
    /// ImplementaciÃ³n Service para Operational Traffic
    /// Implementa state machine de 4 estados
    /// </summary>
    public class OP_TraficoService : IOP_TraficoService
    {
        private readonly IOP_TraficoAdapter _adapter;
        private readonly IAuthorizationService _authService;
        private readonly ILogger<OP_TraficoService> _logger;

        public OP_TraficoService(
            IOP_TraficoAdapter adapter, 
            IAuthorizationService authService,
            ILogger<OP_TraficoService> logger)
        {
            _adapter = adapter;
            _authService = authService;
            _logger = logger;
        }

        // ============================================
        // CONSULTAS GENERALES
        // ============================================

        public async Task<ApiResponse<OP_TraficoResultadoDTO>> ObtenerEventosAsync(OP_TraficoFiltrosDTO filtros)
        {
            try
            {
                _logger.LogInformation("[OP_TraficoService] Obteniendo eventos");

                _adapter.ValidarFiltros(filtros);

                var eventos = await _adapter.GetEventosAsync(filtros);

                if (!eventos.Any())
                    return ApiResponse<OP_TraficoResultadoDTO>.Ok(
                        new OP_TraficoResultadoDTO { Datos = new(), TotalRegistros = 0 },
                        "Sin eventos para los filtros especificados");

                var totalRegistros = eventos.Count;
                var totalPaginas = (int)Math.Ceiling((decimal)totalRegistros / filtros.PageSize);

                var resultado = new OP_TraficoResultadoDTO
                {
                    Datos = eventos,
                    TotalRegistros = totalRegistros,
                    Pagina = filtros.PageNumber,
                    RegistrosPorPagina = filtros.PageSize,
                    TotalPaginas = totalPaginas,
                    TienePaginas = filtros.PageNumber < totalPaginas
                };

                return ApiResponse<OP_TraficoResultadoDTO>.Ok(resultado, "Eventos obtenidos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en ObtenerEventos");
                return ApiResponse<OP_TraficoResultadoDTO>.Error("Error al obtener eventos. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<OP_TraficoEventoDTO>> ObtenerEventoDetalleAsync(int eventoId)
        {
            try
            {
                _logger.LogInformation($"[OP_TraficoService] Obteniendo detalle evento: {eventoId}");

                var evento = await _adapter.GetEventoByIdAsync(eventoId);
                if (evento == null || evento.EventoId == 0)
                    return ApiResponse<OP_TraficoEventoDTO>.NotFound("Evento no encontrado");

                return ApiResponse<OP_TraficoEventoDTO>.Ok(evento, "Detalle obtenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en ObtenerEventoDetalle");
                return ApiResponse<OP_TraficoEventoDTO>.Error("Error al obtener detalle de evento. Por favor intente nuevamente.");
            }
        }

        // ============================================
        // TRANSICIONES DE ESTADO: STATE MACHINE
        // ============================================

        public async Task<ApiResponse<int>> CapturarAsync(OP_TraficoCapturarDTO captura)
        {
            try
            {
                _logger.LogInformation("[OP_TraficoService] Iniciando captura");

                // REGLA 6: ValidaciÃ³n
                if (captura == null || captura.EstudioId <= 0)
                    return ApiResponse<int>.BadRequest("Datos invÃ¡lidos", 0);

                _adapter.ValidarDatos(captura);

                // REGLA 9: Validar permisos
                if (!await ValidarPermisoAsync(0, captura.UsuarioCapturistaId, "CAPTURAR"))
                    return ApiResponse<int>.Unauthorized("No tiene permisos para capturar");

                // TransiciÃ³n: â†’ Capturado
                var eventoId = await _adapter.CapturarAsync(captura);

                _logger.LogInformation($"[OP_TraficoService] Evento capturado: {eventoId}");

                return ApiResponse<int>.Ok(eventoId, $"Captura registrada - ID: {eventoId}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en Capturar");
                return ApiResponse<int>.Error("Error al capturar. Por favor intente nuevamente.", 0);
            }
        }

        public async Task<ApiResponse<string>> CriticarAsync(OP_TraficoCriticarDTO critica)
        {
            try
            {
                _logger.LogInformation($"[OP_TraficoService] Criticando evento: {critica.EventoId}");

                // REGLA 6: ValidaciÃ³n
                if (critica == null || critica.EventoId <= 0)
                    return ApiResponse<string>.BadRequest("Datos invÃ¡lidos");

                _adapter.ValidarDatos(critica);

                // Obtener evento actual
                var evento = await _adapter.GetEventoByIdAsync(critica.EventoId);
                if (evento == null || evento.EventoId == 0)
                    return ApiResponse<string>.NotFound("Evento no encontrado");

                // STATE MACHINE: Validar transiciÃ³n Capturado â†’ Criticado
                if (!await ValidarTransicionEstadoAsync(evento.EstadoActual ?? string.Empty, EstadosTrafico.CRITICADO, critica.UsuarioCriticoId))
                    return ApiResponse<string>.BadRequest($"No se puede pasar de {evento.EstadoActual} a {EstadosTrafico.CRITICADO}");

                // REGLA 9: Validar permisos
                if (!await ValidarPermisoAsync(critica.EventoId, critica.UsuarioCriticoId, "CRITICAR"))
                    return ApiResponse<string>.Unauthorized("No tiene permisos para criticar");

                // Ejecutar crÃ­tica
                var resultado = await _adapter.CriticarAsync(critica);
                if (!resultado)
                    return ApiResponse<string>.Error("Error al criticar evento");

                _logger.LogInformation($"[OP_TraficoService] Evento {critica.EventoId} criticado");

                return ApiResponse<string>.Ok($"Evento #{critica.EventoId} criticado", "CrÃ­tica registrada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en Criticar");
                return ApiResponse<string>.Error("Error al criticar. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<string>> VerificarAsync(OP_TraficoVerificarDTO verificacion)
        {
            try
            {
                _logger.LogInformation($"[OP_TraficoService] Verificando evento: {verificacion.EventoId}");

                // REGLA 6: ValidaciÃ³n
                if (verificacion == null || verificacion.EventoId <= 0)
                    return ApiResponse<string>.BadRequest("Datos invÃ¡lidos");

                _adapter.ValidarDatos(verificacion);

                // Obtener evento actual
                var evento = await _adapter.GetEventoByIdAsync(verificacion.EventoId);
                if (evento == null || evento.EventoId == 0)
                    return ApiResponse<string>.NotFound("Evento no encontrado");

                // STATE MACHINE: Validar transiciÃ³n Criticado â†’ Verificado
                if (!await ValidarTransicionEstadoAsync(evento.EstadoActual ?? string.Empty, EstadosTrafico.VERIFICADO, verificacion.UsuarioVerificadorId))
                    return ApiResponse<string>.BadRequest($"No se puede pasar de {evento.EstadoActual} a {EstadosTrafico.VERIFICADO}");

                // REGLA 9: Validar permisos
                if (!await ValidarPermisoAsync(verificacion.EventoId, verificacion.UsuarioVerificadorId, "VERIFICAR"))
                    return ApiResponse<string>.Unauthorized("No tiene permisos para verificar");

                // Ejecutar verificaciÃ³n
                var resultado = await _adapter.VerificarAsync(verificacion);
                if (!resultado)
                    return ApiResponse<string>.Error("Error al verificar evento");

                _logger.LogInformation($"[OP_TraficoService] Evento {verificacion.EventoId} verificado");

                return ApiResponse<string>.Ok($"Evento #{verificacion.EventoId} verificado", "VerificaciÃ³n registrada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en Verificar");
                return ApiResponse<string>.Error("Error al verificar. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<string>> AnularAsync(OP_TraficoAnularDTO anulacion)
        {
            try
            {
                _logger.LogInformation($"[OP_TraficoService] Anulando evento: {anulacion.EventoId}");

                // REGLA 6: ValidaciÃ³n
                if (anulacion == null || anulacion.EventoId <= 0 || string.IsNullOrEmpty(anulacion.MotivoAnulacion))
                    return ApiResponse<string>.BadRequest("Datos invÃ¡lidos - motivo requerido");

                _adapter.ValidarDatos(anulacion);

                // Obtener evento actual
                var evento = await _adapter.GetEventoByIdAsync(anulacion.EventoId);
                if (evento == null || evento.EventoId == 0)
                    return ApiResponse<string>.NotFound("Evento no encontrado");

                // STATE MACHINE: Validar transiciÃ³n [Cualquier] â†’ Anulado
                if (!await ValidarTransicionEstadoAsync(evento.EstadoActual ?? string.Empty, EstadosTrafico.ANULADO, anulacion.UsuarioAnuladorId))
                    return ApiResponse<string>.BadRequest($"No se puede anular desde estado {evento.EstadoActual}");

                // REGLA 9: Validar permisos
                if (!await ValidarPermisoAsync(anulacion.EventoId, anulacion.UsuarioAnuladorId, "ANULAR"))
                    return ApiResponse<string>.Unauthorized("No tiene permisos para anular");

                // Ejecutar anulaciÃ³n
                var resultado = await _adapter.AnularAsync(anulacion);
                if (!resultado)
                    return ApiResponse<string>.Error("Error al anular evento");

                _logger.LogInformation($"[OP_TraficoService] Evento {anulacion.EventoId} anulado: {anulacion.MotivoAnulacion}");

                return ApiResponse<string>.Ok($"Evento #{anulacion.EventoId} anulado", "AnulaciÃ³n registrada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en Anular");
                return ApiResponse<string>.Error("Error al anular. Por favor intente nuevamente.");
            }
        }

        // ============================================
        // OBTENCIÃ“N DE DETALLES POR ESTADO
        // ============================================

        public async Task<ApiResponse<OP_TraficoCapturadoDTO>> ObtenerCapturadoAsync(int eventoId)
        {
            try
            {
                var capturado = await _adapter.GetCapturadoAsync(eventoId);
                if (capturado == null || capturado.CapturadoId == 0)
                    return ApiResponse<OP_TraficoCapturadoDTO>.NotFound("InformaciÃ³n de captura no encontrada");

                return ApiResponse<OP_TraficoCapturadoDTO>.Ok(capturado, "Detalle de captura obtenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en ObtenerCapturado");
                return ApiResponse<OP_TraficoCapturadoDTO>.Error("Error al obtener capturado. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<OP_TraficoCriticadoDTO>> ObtenerCriticadoAsync(int eventoId)
        {
            try
            {
                var criticado = await _adapter.GetCriticadoAsync(eventoId);
                if (criticado == null || criticado.CriticadoId == 0)
                    return ApiResponse<OP_TraficoCriticadoDTO>.NotFound("InformaciÃ³n de crÃ­tica no encontrada");

                return ApiResponse<OP_TraficoCriticadoDTO>.Ok(criticado, "Detalle de crÃ­tica obtenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en ObtenerCriticado");
                return ApiResponse<OP_TraficoCriticadoDTO>.Error("Error al obtener criticado. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<OP_TraficoVerificadoDTO>> ObtenerVerificadoAsync(int eventoId)
        {
            try
            {
                var verificado = await _adapter.GetVerificadoAsync(eventoId);
                if (verificado == null || verificado.VerificadoId == 0)
                    return ApiResponse<OP_TraficoVerificadoDTO>.NotFound("InformaciÃ³n de verificaciÃ³n no encontrada");

                return ApiResponse<OP_TraficoVerificadoDTO>.Ok(verificado, "Detalle de verificaciÃ³n obtenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en ObtenerVerificado");
                return ApiResponse<OP_TraficoVerificadoDTO>.Error("Error al obtener verificado. Por favor intente nuevamente.");
            }
        }

        public async Task<ApiResponse<OP_TraficoAnuladoDTO>> ObtenerAnuladoAsync(int eventoId)
        {
            try
            {
                var anulado = await _adapter.GetAnuladoAsync(eventoId);
                if (anulado == null || anulado.AnuladoId == 0)
                    return ApiResponse<OP_TraficoAnuladoDTO>.NotFound("InformaciÃ³n de anulaciÃ³n no encontrada");

                return ApiResponse<OP_TraficoAnuladoDTO>.Ok(anulado, "Detalle de anulaciÃ³n obtenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en ObtenerAnulado");
                return ApiResponse<OP_TraficoAnuladoDTO>.Error("Error al obtener anulado. Por favor intente nuevamente.");
            }
        }

        // ============================================
        // HISTORIAL Y AUDITORÃA
        // ============================================

        public async Task<ApiResponse<List<OP_TraficoHistorialDTO>>> ObtenerHistorialAsync(int eventoId)
        {
            try
            {
                _logger.LogInformation($"[OP_TraficoService] Obteniendo historial evento: {eventoId}");

                var historial = await _adapter.GetHistorialAsync(eventoId);

                return ApiResponse<List<OP_TraficoHistorialDTO>>.Ok(
                    historial ?? new(),
                    $"{historial?.Count ?? 0} registros de historial");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en ObtenerHistorial");
                return ApiResponse<List<OP_TraficoHistorialDTO>>.Error("Error al obtener historial. Por favor intente nuevamente.");
            }
        }

        // ============================================
        // DASHBOARDS Y REPORTES
        // ============================================

        public async Task<ApiResponse<OP_TraficoDashboardDTO>> ObtenerDashboardAsync(DateTime? fechaDesde = null, DateTime? fechaHasta = null)
        {
            try
            {
                _logger.LogInformation("[OP_TraficoService] Obteniendo dashboard");

                var dashboard = await _adapter.GetDashboardAsync(fechaDesde, fechaHasta);

                return ApiResponse<OP_TraficoDashboardDTO>.Ok(dashboard, "Dashboard obtenido");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[OP_TraficoService] Error en ObtenerDashboard");
                return ApiResponse<OP_TraficoDashboardDTO>.Error("Error al obtener dashboard. Por favor intente nuevamente.");
            }
        }

        // ============================================
        // VALIDACIONES Y STATE MACHINE
        // ============================================

        public async Task<bool> ValidarTransicionEstadoAsync(string estadoActual, string estadoNuevo, int usuarioId)
        {
            // STATE MACHINE: 4 estados
            // Capturado â†’ [Criticado, Anulado]
            // Criticado â†’ [Verificado, Anulado]
            // Verificado â†’ [Anulado]
            // Anulado â†’ []

            _logger.LogInformation($"[OP_TraficoService] Validando transiciÃ³n: {estadoActual} â†’ {estadoNuevo}");

            var transicionesValidas = new Dictionary<string, List<string>>
            {
                { EstadosTrafico.CAPTURADO, new() { EstadosTrafico.CRITICADO, EstadosTrafico.ANULADO } },
                { EstadosTrafico.CRITICADO, new() { EstadosTrafico.VERIFICADO, EstadosTrafico.ANULADO } },
                { EstadosTrafico.VERIFICADO, new() { EstadosTrafico.ANULADO } },
                { EstadosTrafico.ANULADO, new() }
            };

            if (!transicionesValidas.ContainsKey(estadoActual))
            {
                _logger.LogWarning($"[OP_TraficoService] Estado actual no reconocido: {estadoActual}");
                return false;
            }

            var permitidas = transicionesValidas[estadoActual];
            var esValida = permitidas.Contains(estadoNuevo);

            _logger.LogInformation($"[OP_TraficoService] TransiciÃ³n {(esValida ? "VÃLIDA" : "INVÃLIDA")}");

            return await Task.FromResult(esValida);
        }

        public async Task<bool> ValidarPermisoAsync(int eventoId, int usuarioId, string accion)
        {
            try
            {
                _logger.LogInformation($"[OP_TraficoService] Validando permisos: usuario {usuarioId}, acciÃ³n {accion}");
                return await _authService.ValidarPermisoAsync(usuarioId, "Trafico", accion, eventoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"[OP_TraficoService] Error validando permisos");
                return false;
            }
        }
    }
}

