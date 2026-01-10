using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.Models.ViewModels.GD
{
    public class MaestroDocumentoVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del documento es requerido")]
        [MaxLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El código del documento es requerido")]
        [MaxLength(50, ErrorMessage = "El código no puede exceder 50 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El proceso es requerido")]
        public int IdProceso { get; set; }

        [Required(ErrorMessage = "El responsable es requerido")]
        public int IdResponsable { get; set; }

        [Required(ErrorMessage = "El tipo de solicitud es requerido")]
        public int TipoSolicitud { get; set; }

        public bool Activo { get; set; } = true;

        public string ProcesoNombre { get; set; } = string.Empty;
        public string ResponsableNombre { get; set; } = string.Empty;
        public string TipoNombre { get; set; } = string.Empty;

        public DocumentoControlledVM ControlledDoc { get; set; } = new();
    }

    public class DocumentoControlledVM
    {
        public int Id { get; set; }
        public int IdMaestro { get; set; }

        [Required(ErrorMessage = "La ubicación es requerida")]
        [MaxLength(500)]
        public string Ubicacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El método de recuperación es requerido")]
        [MaxLength(100)]
        public string MetodoRecuperacion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El tiempo de retención es requerido")]
        [Range(1, 100, ErrorMessage = "El tiempo debe estar entre 1 y 100 años")]
        public int TiempoRetencion { get; set; }

        [Required(ErrorMessage = "La disposición final es requerida")]
        [MaxLength(200)]
        public string DisposicionFinal { get; set; } = string.Empty;

        public bool Activo { get; set; } = true;
        public DateTime? FechaRegistro { get; set; }
    }

    public class MaestroCreateVM : MaestroDocumentoVM
    {
        public List<TipoSolicitudViewModel> TiposSolicitud { get; set; } = new();
        public List<ProcesoViewModel> Procesos { get; set; } = new();
        public List<UsuarioViewModel> Usuarios { get; set; } = new();
    }

    public class MaestroUpdateVM : MaestroDocumentoVM
    {
        public List<TipoSolicitudViewModel> TiposSolicitud { get; set; } = new();
        public List<ProcesoViewModel> Procesos { get; set; } = new();
        public List<UsuarioViewModel> Usuarios { get; set; } = new();

        public int RegistradoPor { get; set; }
        public string RegistradoPorNombre { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public int? ModificadoPor { get; set; }
        public string ModificadoPorNombre { get; set; } = string.Empty;
        public DateTime? FechaModificacion { get; set; }
    }

    public class MaestroListVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Codigo { get; set; } = string.Empty;
        public string Proceso { get; set; } = string.Empty;
        public string Responsable { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
    }

    public class UsuarioViewModel
    {
        public int Id { get; set; }
        public string Usuario { get; set; } = string.Empty;
        public string Nombres { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool Activo { get; set; }

        public string NombreCompleto => string.Join(" ", new[] { Nombres, Apellidos }).Trim();
    }
}
