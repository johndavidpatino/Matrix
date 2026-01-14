namespace MatrixNext.Data.Models.OP_Trafico
{
    /// <summary>
    /// DTO para Operational Traffic (OP_Trafico)
    /// Sprint 11B - Gestión de tráfico de datos
    /// Workflow: Capturado → Criticado → Verificado → Anulado
    /// Responsabilidades: Captura, Crítica, InicioTraficoEncuestas, RMC, TrabajosProyectos, Verificación
    /// </summary>
    /// 
    /// <summary>
    /// Información base de un evento de tráfico
    /// </summary>
    public class OP_TraficoEventoDTO
    {
        public int EventoId { get; set; }
        public string Codigo { get; set; }
        public string Tipo { get; set; } // Cuantitativo, Cualitativo, Mixto
        public string Descripcion { get; set; }
        public int EstudioId { get; set; }
        public DateTime FechaCaptura { get; set; }
        public int UsuarioCapturistaId { get; set; }
        public string UsuarioCapturista { get; set; }
        public string EstadoActual { get; set; } // Capturado, Criticado, Verificado, Anulado
        public int VersionEstado { get; set; }
        public DateTime? FechaUltimaTransicion { get; set; }
        public string UltimoUsuario { get; set; }
        public bool Disponible { get; set; }
    }

    /// <summary>
    /// Detalle de captura de datos
    /// Estado: Capturado
    /// </summary>
    public class OP_TraficoCapturadoDTO
    {
        public int CapturadoId { get; set; }
        public int EventoId { get; set; }
        public string Codigo { get; set; }
        public int NumeroEncuestas { get; set; }
        public int NumeroTrabajadores { get; set; }
        public DateTime FechaCaptura { get; set; }
        public int UsuarioCapturistaId { get; set; }
        public string UsuarioCapturista { get; set; }
        public string Observaciones { get; set; }
        public List<DatosCapturaDTO> DatosCapturados { get; set; } = new();
    }

    /// <summary>
    /// Información de crítica de datos
    /// Estado: Criticado
    /// </summary>
    public class OP_TraficoCriticadoDTO
    {
        public int CriticadoId { get; set; }
        public int EventoId { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaCritica { get; set; }
        public int UsuarioCriticoId { get; set; }
        public string UsuarioCritico { get; set; }
        public string Resultado { get; set; } // Aceptado, ConObservaciones, Rechazado
        public int NumeroErrores { get; set; }
        public int NumeroAdvertencias { get; set; }
        public List<ErrorCriticaDTO> Errores { get; set; } = new();
        public List<AdvertenciaCriticaDTO> Advertencias { get; set; } = new();
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// Información de verificación
    /// Estado: Verificado
    /// </summary>
    public class OP_TraficoVerificadoDTO
    {
        public int VerificadoId { get; set; }
        public int EventoId { get; set; }
        public string Codigo { get; set; }
        public DateTime FechaVerificacion { get; set; }
        public int UsuarioVerificadorId { get; set; }
        public string UsuarioVerificador { get; set; }
        public string Resultado { get; set; } // Aprobado, Rechazado
        public int NumeroInconsistencias { get; set; }
        public List<InconsistenciaDTO> Inconsistencias { get; set; } = new();
        public string Observaciones { get; set; }
        public DateTime? FechaAprobacionFinal { get; set; }
    }

    /// <summary>
    /// Información de anulación
    /// Estado: Anulado
    /// </summary>
    public class OP_TraficoAnuladoDTO
    {
        public int AnuladoId { get; set; }
        public int EventoId { get; set; }
        public DateTime FechaAnulacion { get; set; }
        public int UsuarioAnuladorId { get; set; }
        public string UsuarioAnulador { get; set; }
        public string MotivoAnulacion { get; set; }
        public string Observaciones { get; set; }
        public string EstadoAnterior { get; set; }
    }

    // ============================================
    // COMPONENTES ANIDADOS
    // ============================================

    /// <summary>
    /// Registro de datos capturados
    /// </summary>
    public class DatosCapturaDTO
    {
        public int DatoId { get; set; }
        public string Campo { get; set; }
        public string Valor { get; set; }
        public string Tipo { get; set; } // Numérico, Texto, Fecha
        public DateTime FechaCaptura { get; set; }
    }

    /// <summary>
    /// Error detectado en crítica
    /// </summary>
    public class ErrorCriticaDTO
    {
        public int ErrorId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Severidad { get; set; } // Crítica, Alta, Media, Baja
        public string Campo { get; set; }
        public string ValorActual { get; set; }
        public string ValorEsperado { get; set; }
    }

    /// <summary>
    /// Advertencia detectada en crítica
    /// </summary>
    public class AdvertenciaCriticaDTO
    {
        public int AdvertenciaId { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string Campo { get; set; }
        public string Observacion { get; set; }
    }

    /// <summary>
    /// Inconsistencia detectada en verificación
    /// </summary>
    public class InconsistenciaDTO
    {
        public int InconsistenciaId { get; set; }
        public string Tipo { get; set; } // Duplicado, Incoherencia, Falta, Exceso
        public string Descripcion { get; set; }
        public string DetallesCampo { get; set; }
        public DateTime FechaDeteccion { get; set; }
    }

    // ============================================
    // WORKFLOWS Y ACCIONES
    // ============================================

    /// <summary>
    /// DTO para registrar captura de datos
    /// Transición: → Capturado
    /// </summary>
    public class OP_TraficoCapturarDTO
    {
        public int EstudioId { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public int UsuarioCapturistaId { get; set; }
        public int NumeroEncuestas { get; set; }
        public int NumeroTrabajadores { get; set; }
        public List<DatosCapturaDTO> Datos { get; set; } = new();
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para registrar crítica
    /// Transición: Capturado → Criticado
    /// </summary>
    public class OP_TraficoCriticarDTO
    {
        public int EventoId { get; set; }
        public int UsuarioCriticoId { get; set; }
        public string Resultado { get; set; } // Aceptado, ConObservaciones, Rechazado
        public List<ErrorCriticaDTO> Errores { get; set; } = new();
        public List<AdvertenciaCriticaDTO> Advertencias { get; set; } = new();
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para registrar verificación
    /// Transición: Criticado → Verificado
    /// </summary>
    public class OP_TraficoVerificarDTO
    {
        public int EventoId { get; set; }
        public int UsuarioVerificadorId { get; set; }
        public string Resultado { get; set; } // Aprobado, Rechazado
        public List<InconsistenciaDTO> Inconsistencias { get; set; } = new();
        public string Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para anular evento
    /// Transición: [Cualquier estado] → Anulado
    /// </summary>
    public class OP_TraficoAnularDTO
    {
        public int EventoId { get; set; }
        public int UsuarioAnuladorId { get; set; }
        public string MotivoAnulacion { get; set; }
        public string Observaciones { get; set; }
    }

    // ============================================
    // FILTROS Y BÚSQUEDA
    // ============================================

    /// <summary>
    /// DTO para filtros en búsqueda de eventos
    /// </summary>
    public class OP_TraficoFiltrosDTO
    {
        public string Estado { get; set; } // Capturado, Criticado, Verificado, Anulado
        public string Tipo { get; set; } // Cuantitativo, Cualitativo
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? UsuarioId { get; set; }
        public int? EstudioId { get; set; }
        public string Codigo { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    /// <summary>
    /// DTO para resultado paginado
    /// </summary>
    public class OP_TraficoResultadoDTO
    {
        public List<OP_TraficoEventoDTO> Datos { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalPaginas { get; set; }
        public bool TienePaginas { get; set; }
    }

    // ============================================
    // DASHBOARDS Y REPORTES
    // ============================================

    /// <summary>
    /// DTO para resumen de tráfico
    /// Estadísticas por estado
    /// </summary>
    public class OP_TraficoDashboardDTO
    {
        public int TotalCapturados { get; set; }
        public int TotalCriticados { get; set; }
        public int TotalVerificados { get; set; }
        public int TotalAnulados { get; set; }
        public int TotalEventos { get; set; }
        public double PorcentajeCompletados { get; set; }
        public DateTime FechaActualizacion { get; set; }
        public List<EventoEstadoDTO> EventosPorEstado { get; set; } = new();
    }

    /// <summary>
    /// Resumen por estado
    /// </summary>
    public class EventoEstadoDTO
    {
        public string Estado { get; set; }
        public int Cantidad { get; set; }
        public double Porcentaje { get; set; }
    }

    // ============================================
    // HISTORIAL Y AUDITORÍA
    // ============================================

    /// <summary>
    /// Registro de transición de estado
    /// </summary>
    public class OP_TraficoHistorialDTO
    {
        public int HistorialId { get; set; }
        public int EventoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
        public string EstadoAnterior { get; set; }
        public string EstadoNuevo { get; set; }
        public string Accion { get; set; } // CAPTURA, CRÍTICA, VERIFICACIÓN, ANULACIÓN
        public string Detalles { get; set; }
    }

    // ============================================
    // CONSTANTES
    // ============================================

    public static class EstadosTrafico
    {
        public const string CAPTURADO = "Capturado";
        public const string CRITICADO = "Criticado";
        public const string VERIFICADO = "Verificado";
        public const string ANULADO = "Anulado";
    }

    public static class TiposTrafico
    {
        public const string CUANTITATIVO = "Cuantitativo";
        public const string CUALITATIVO = "Cualitativo";
        public const string MIXTO = "Mixto";
    }

    public static class ResultadosCritica
    {
        public const string ACEPTADO = "Aceptado";
        public const string CON_OBSERVACIONES = "ConObservaciones";
        public const string RECHAZADO = "Rechazado";
    }

    public static class ResultadosVerificacion
    {
        public const string APROBADO = "Aprobado";
        public const string RECHAZADO = "Rechazado";
    }

    public static class SeveridadesError
    {
        public const string CRÍTICA = "Crítica";
        public const string ALTA = "Alta";
        public const string MEDIA = "Media";
        public const string BAJA = "Baja";
    }

    public static class AccionesAuditoria
    {
        public const string CAPTURA = "CAPTURA";
        public const string CRÍTICA = "CRÍTICA";
        public const string VERIFICACIÓN = "VERIFICACIÓN";
        public const string ANULACIÓN = "ANULACIÓN";
    }
}
