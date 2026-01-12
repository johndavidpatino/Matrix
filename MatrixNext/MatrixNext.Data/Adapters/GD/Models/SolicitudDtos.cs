using System;

namespace MatrixNext.Data.Adapters.GD.Models
{
    /// <summary>
    /// DTO base para solicitud de documento
    /// </summary>
    public class SolicitudDocumentoDto
    {
        public int Id { get; set; }
        public int TipoSolicitud { get; set; } // 1=Construcción, 2=Actualización, 3=Anulación
        public int IdDocumento { get; set; }
        public int IdSolicitante { get; set; }
        public string Area { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Razon { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int IdEstado { get; set; }
        public string Comentarios { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string? AreaUso { get; set; }
        public string? SitioAcceso { get; set; }
        public string? NombreDocumento { get; set; }
        public string? Codigo { get; set; }
    }

    /// <summary>
    /// DTO para input de creación de solicitud
    /// </summary>
    public class SolicitudCreateInputDto
    {
        public int TipoSolicitud { get; set; }
        public int IdDocumento { get; set; }
        public int IdSolicitante { get; set; }
        public string Area { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Razon { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int? IdEstado { get; set; }
        public string? Comentarios { get; set; }
        public string? AreaUso { get; set; }
        public string? SitioAcceso { get; set; }
        public string? NombreDocumento { get; set; }
        public string? Codigo { get; set; }
    }

    /// <summary>
    /// DTO para crear revisión (asignar revisor)
    /// </summary>
    public class RevisionDto
    {
        public int Id { get; set; }
        public int IdSolicitud { get; set; }
        public int IdDocumentoControlado { get; set; }
        public int IdRevisor { get; set; }
        public string NombreRevisor { get; set; } = string.Empty;
        public int TipoRevision { get; set; } // 1=Pendiente, 2=Completada
        public int Estado { get; set; } // 0=Pendiente, 1=Aprobado, 2=Rechazado
        public DateTime FechaAprobacion { get; set; }
        public string Comentarios { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para lista de solicitudes
    /// </summary>
    public class SolicitudListDto
    {
        public int Id { get; set; }
        public string NombreDocumento { get; set; } = string.Empty;
        public string TipoSolicitud { get; set; } = string.Empty;
        public string Solicitante { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int RevisoresPendientes { get; set; }
        public int RevisoresAprobados { get; set; }
        public DateTime FechaRegistro { get; set; }
    }

    /// <summary>
    /// DTO para revisiones pendientes de aprobación
    /// </summary>
    public class RevisionAprobacionDto
    {
        public int IdRevision { get; set; }
        public int DocumentoId { get; set; }
        public int UsuarioId { get; set; }
        public int TipoRevisionId { get; set; }
        public string TipoRevision { get; set; } = string.Empty;
        public int DocumentoControladoId { get; set; }
        public string NombreDocumento { get; set; } = string.Empty;
        public DateTime? FechaAprobacion { get; set; }
    }
}
