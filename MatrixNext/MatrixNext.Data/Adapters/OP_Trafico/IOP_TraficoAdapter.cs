using Dapper;
using MatrixNext.Data.Models.OP_Trafico;
using System.Data;

namespace MatrixNext.Data.Adapters.OP_Trafico
{
    /// <summary>
    /// Interfaz Adapter para Operational Traffic
    /// Responsable: Ejecutar SP para gestión de tráfico de datos
    /// Workflow: Capturado → Criticado → Verificado → Anulado
    /// REGLA 2: Mapeo exacto de SP según CoreProject
    /// REGLA 4: Ejecución de SP contra BD
    /// </summary>
    public interface IOP_TraficoAdapter
    {
        // ============================================
        // CONSULTAS GENERALES
        // ============================================

        /// <summary>
        /// Obtiene listado de eventos con filtros
        /// SP: OP_Trafico_Eventos_Get
        /// </summary>
        Task<List<OP_TraficoEventoDTO>> GetEventosAsync(OP_TraficoFiltrosDTO filtros);

        /// <summary>
        /// Obtiene un evento específico por ID
        /// SP: OP_Trafico_Evento_GetById
        /// </summary>
        Task<OP_TraficoEventoDTO> GetEventoByIdAsync(int eventoId);

        // ============================================
        // ESTADO: CAPTURADO
        // ============================================

        /// <summary>
        /// Obtiene información de captura
        /// SP: OP_Trafico_Capturado_GetById
        /// </summary>
        Task<OP_TraficoCapturadoDTO> GetCapturadoAsync(int eventoId);

        /// <summary>
        /// Registra nueva captura
        /// SP: OP_Trafico_Capturado_Save
        /// Transición: → Capturado
        /// </summary>
        Task<int> CapturarAsync(OP_TraficoCapturarDTO captura);

        // ============================================
        // ESTADO: CRITICADO
        // ============================================

        /// <summary>
        /// Obtiene información de crítica
        /// SP: OP_Trafico_Criticado_GetById
        /// </summary>
        Task<OP_TraficoCriticadoDTO> GetCriticadoAsync(int eventoId);

        /// <summary>
        /// Registra crítica de datos
        /// SP: OP_Trafico_Criticado_Save
        /// Transición: Capturado → Criticado
        /// </summary>
        Task<bool> CriticarAsync(OP_TraficoCriticarDTO critica);

        // ============================================
        // ESTADO: VERIFICADO
        // ============================================

        /// <summary>
        /// Obtiene información de verificación
        /// SP: OP_Trafico_Verificado_GetById
        /// </summary>
        Task<OP_TraficoVerificadoDTO> GetVerificadoAsync(int eventoId);

        /// <summary>
        /// Registra verificación
        /// SP: OP_Trafico_Verificado_Save
        /// Transición: Criticado → Verificado
        /// </summary>
        Task<bool> VerificarAsync(OP_TraficoVerificarDTO verificacion);

        // ============================================
        // ESTADO: ANULADO
        // ============================================

        /// <summary>
        /// Obtiene información de anulación
        /// SP: OP_Trafico_Anulado_GetById
        /// </summary>
        Task<OP_TraficoAnuladoDTO> GetAnuladoAsync(int eventoId);

        /// <summary>
        /// Registra anulación de evento
        /// SP: OP_Trafico_Anulado_Save
        /// Transición: [Cualquier] → Anulado
        /// </summary>
        Task<bool> AnularAsync(OP_TraficoAnularDTO anulacion);

        // ============================================
        // HISTORIAL Y AUDITORÍA
        // ============================================

        /// <summary>
        /// Obtiene historial de transiciones
        /// SP: OP_Trafico_Evento_Historial_Get
        /// </summary>
        Task<List<OP_TraficoHistorialDTO>> GetHistorialAsync(int eventoId);

        // ============================================
        // DASHBOARDS Y REPORTES
        // ============================================

        /// <summary>
        /// Obtiene resumen de tráfico
        /// SP: OP_Trafico_Dashboard_Get
        /// Estadísticas por estado
        /// </summary>
        Task<OP_TraficoDashboardDTO> GetDashboardAsync(DateTime? fechaDesde = null, DateTime? fechaHasta = null);

        /// <summary>
        /// Obtiene estadísticas por estado
        /// SP: OP_Trafico_EstadisticasEstado_Get
        /// </summary>
        Task<List<EventoEstadoDTO>> GetEstadisticasEstadoAsync();

        // ============================================
        // VALIDACIONES
        // ============================================

        /// <summary>
        /// Valida parámetros de filtros
        /// </summary>
        void ValidarFiltros(OP_TraficoFiltrosDTO filtros);

        /// <summary>
        /// Valida datos según tipo de transición
        /// </summary>
        void ValidarDatos(object dto);

        /// <summary>
        /// Valida si es posible realizar transición de estado
        /// </summary>
        Task<bool> ValidarTransicionAsync(int eventoId, string estadoActual, string estadoNuevo);
    }
}
