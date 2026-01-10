using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MatrixNext.Data.Adapters.GD.Models;

namespace MatrixNext.Web.Models.ViewModels.GD
{
    /// <summary>
    /// ViewModel base para solicitud de documento
    /// </summary>
    public class SolicitudDocumentoVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El tipo de solicitud es requerido")]
        public int TipoSolicitud { get; set; } // 1=Construcción, 2=Actualización, 3=Anulación

        [Required(ErrorMessage = "El documento es requerido")]
        public int IdDocumento { get; set; }

        [Required(ErrorMessage = "El solicitante es requerido")]
        public int IdSolicitante { get; set; }

        [Required(ErrorMessage = "El área es requerida")]
        [MaxLength(100)]
        public string Area { get; set; } = string.Empty;

        [Required(ErrorMessage = "El cargo es requerido")]
        [MaxLength(100)]
        public string Cargo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La razón es requerida")]
        [MaxLength(500)]
        public string Razon { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(1000)]
        public string Descripcion { get; set; } = string.Empty;

        public int? IdEstado { get; set; } = 1; // Por defecto: En Revisión

        [MaxLength(500)]
        public string Comentarios { get; set; } = string.Empty;

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        // Para visualización
        public string NombreDocumento { get; set; } = string.Empty;
        public string NombreSolicitante { get; set; } = string.Empty;
        public string NombreEstado { get; set; } = string.Empty;
        public string NombreTipo { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel para crear solicitud (con dropdowns)
    /// </summary>
    public class SolicitudCreateVM : SolicitudDocumentoVM
    {
        public List<SelectListItemDto> TiposSolicitud { get; set; } = new();
        public List<SelectListItemDto> Documentos { get; set; } = new();
        public List<SelectListItemDto> Usuarios { get; set; } = new();
        public List<SelectListItemDto> Estados { get; set; } = new();

        // Campos condicionales según tipo
        public string? AreaUso { get; set; }
        public string? SitioAcceso { get; set; }
    }

    /// <summary>
    /// ViewModel para asignar revisores a una solicitud
    /// </summary>
    public class AsignReviewersVM
    {
        public int IdSolicitud { get; set; }

        [Required(ErrorMessage = "Debe seleccionar al menos un revisor")]
        public List<int> IdRevisores { get; set; } = new();

        public List<SelectListItemDto> RevisoresDisponibles { get; set; } = new();

        public string NombreSolicitud { get; set; } = string.Empty;
        public string NombreDocumento { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel para listar solicitudes en tabla (contenedor)
    /// </summary>
    public class SolicitudListVM
    {
        public List<SolicitudListItemVM> Solicitudes { get; set; } = new();
    }

    /// <summary>
    /// ViewModel para item de solicitud en tabla
    /// </summary>
    public class SolicitudListItemVM
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
}
