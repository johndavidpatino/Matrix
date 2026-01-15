/// <summary>
/// DTOs para Solicitudes de Documentos (GD_SolicitudDocumentos)
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md Â§ Sprint 12.3.1
/// </summary>
namespace MatrixNext.Data.DTOs.GD
{
    using System;
    using System.Collections.Generic;

    public class SolicitudDocumentoDto
    {
        public long IdSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public long IdProyecto { get; set; }
        public string NombreProyecto { get; set; }
        public long IdTipoDocumento { get; set; }
        public string TipoDocumento { get; set; }
        public long IdProceso { get; set; }
        public string Proceso { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public DateTime? FechaRequerida { get; set; }
        public long IdSolicitante { get; set; }
        public string NombreSolicitante { get; set; }
        public long IdEstado { get; set; }
        public string Estado { get; set; }
        public string Observaciones { get; set; }
        public List<RevisorDto> Revisores { get; set; } = new();
        public List<long> IdsRevisores { get; set; } = new();
        public string ContenidoEmail { get; set; }
        public bool EnviarNotificacion { get; set; } = true;
        public DateTime FechaRegistro { get; set; }
        public long RegistradoPor { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public long? ModificadoPor { get; set; }
    }

    public class RevisorDto
    {
        public long IdRevision { get; set; }
        public long IdSolicitud { get; set; }
        public long IdRevisor { get; set; }
        public string NombreRevisor { get; set; }
        public string EmailRevisor { get; set; }
        public int OrdenRevision { get; set; }
        public long IdEstadoRevision { get; set; }
        public string EstadoRevision { get; set; }
        public DateTime? FechaRevision { get; set; }
        public string ComentarioRevision { get; set; }
        public bool Obligatorio { get; set; } = true;
        public DateTime FechaAsignacion { get; set; }
    }

    public class AsignacionRevisoresDto
    {
        public long IdSolicitud { get; set; }
        public List<long> IdsRevisores { get; set; } = new();
        public string ContenidoEmail { get; set; }
        public bool EnviarNotificacion { get; set; } = true;
    }

    public class ConfiguracionRevisionDto
    {
        public long IdConfiguracion { get; set; }
        public long IdProceso { get; set; }
        public string Proceso { get; set; }
        public List<long> RevisoresPorDefecto { get; set; } = new();
        public bool AsignacionAutomatica { get; set; }
        public int CantidadMinima { get; set; } = 1;
        public bool RequiereAprobacionUnanimidad { get; set; } = false;
    }

    /// <summary>
    /// DTO para aprobar o rechazar una revisiÃ³n
    /// Ref: Sprint 12.3.2 - Aprobaciones/Rechazos
    /// </summary>
    public class AprobacionRevisionDto
    {
        public long IdRevision { get; set; }
        public long IdSolicitud { get; set; }
        public long IdRevisor { get; set; }
        public int TipoRevision { get; set; } // 1=Pendiente, 2=Aprobado, 3=Rechazado
        public string ComentarioRevision { get; set; }
        public DateTime FechaRevision { get; set; } = DateTime.Now;
        public bool EnviarNotificacion { get; set; } = true;
    }

    /// <summary>
    /// DTO para resumen de aprobaciones de una solicitud
    /// </summary>
    public class ResumenAprobacionDto
    {
        public long IdSolicitud { get; set; }
        public int TotalRevisores { get; set; }
        public int RevisoresAprobados { get; set; }
        public int RevisoresRechazados { get; set; }
        public int RevisoresPendientes { get; set; }
        public bool RequiereUnanimidad { get; set; }
        public bool TodosAprobados => RevisoresAprobados == TotalRevisores;
        public bool AlgunoRechazo => RevisoresRechazados > 0;
        public long EstadoFinal { get; set; } // 2=Aprobado, 3=Rechazado, 1=Pendiente
        public string MensajeFinal { get; set; }
    }

    /// <summary>
    /// DTO para historial de revisiones (Audit Trail)
    /// Ref: Sprint 12.3.3 - Audit Trail de Revisiones
    /// </summary>
    public class HistorialRevisionDto
    {
        public long IdRevision { get; set; }
        public long IdSolicitud { get; set; }
        public long IdRevisor { get; set; }
        public string NombreRevisor { get; set; }
        public string EmailRevisor { get; set; }
        public int OrdenRevision { get; set; }
        public int TipoRevision { get; set; } // 1=Pendiente, 2=Aprobado, 3=Rechazado
        public string TipoRevisionTexto { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public DateTime? FechaRevision { get; set; }
        public string ComentarioRevision { get; set; }
        public string Accion { get; set; } // "Asignado", "Aprobado", "Rechazado"
        public string AccionClass { get; set; } // CSS class: "info", "success", "danger"
        public string AccionIcon { get; set; } // Font Awesome icon
        public int DiasTranscurridos { get; set; }
    }

    /// <summary>
    /// DTO para timeline completo de una solicitud
    /// </summary>
    public class TimelineSolicitudDto
    {
        public long IdSolicitud { get; set; }
        public string NumeroSolicitud { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string EstadoActual { get; set; }
        public List<HistorialRevisionDto> Eventos { get; set; } = new();
        public int TotalEventos => Eventos.Count;
        public DateTime? UltimaActividad => Eventos.OrderByDescending(e => e.FechaRevision ?? e.FechaAsignacion).FirstOrDefault()?.FechaRevision;
    }
}

