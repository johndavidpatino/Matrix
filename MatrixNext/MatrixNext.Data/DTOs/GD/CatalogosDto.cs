using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
namespace MatrixNext.Data.DTOs.GD
{
    /// <summary>
    /// DTO para CatÃ¡logos del mÃ³dulo GD
    /// Sprint 12.3.8: CatÃ¡logos EdiciÃ³n con Datos
    /// SPs: GD_TipoSolicitud_*, GD_Estados_*, GD_Procesos_*
    /// </summary>
    /// 
    /// <summary>
    /// DTO para Tipo de Solicitud
    /// Tabla: GD_TipoSolicitud
    /// </summary>
    public class TipoSolicitudDto
    {
        [Display(Name = "ID")]
        public long IdTipoSolicitud { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder 150 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripciÃ³n no puede exceder 500 caracteres")]
        [Display(Name = "DescripciÃ³n")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Orden")]
        public int? Orden { get; set; }

        // AuditorÃ­a
        [Display(Name = "Registrado Por")]
        public long? RegistradoPor { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }

        [Display(Name = "Modificado Por")]
        public long? ModificadoPor { get; set; }

        [Display(Name = "Fecha ModificaciÃ³n")]
        public DateTime? FechaModificacion { get; set; }
    }

    /// <summary>
    /// DTO para Estado de Solicitud/PNC
    /// Tabla: GD_Estados
    /// </summary>
    public class EstadoDto
    {
        [Display(Name = "ID")]
        public long IdEstado { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(300, ErrorMessage = "La descripciÃ³n no puede exceder 300 caracteres")]
        [Display(Name = "DescripciÃ³n")]
        public string Descripcion { get; set; }

        [Display(Name = "MÃ³dulo")]
        public string Modulo { get; set; } // GD_Solicitud, PNC, etc.

        [Display(Name = "Color")]
        public string Color { get; set; } // CSS class: success, warning, danger, info

        [Display(Name = "Ãcono")]
        public string Icono { get; set; } // Font Awesome: check-circle, exclamation-circle, ban, etc.

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Orden")]
        public int? Orden { get; set; }

        // AuditorÃ­a
        [Display(Name = "Registrado Por")]
        public long? RegistradoPor { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }

        [Display(Name = "Modificado Por")]
        public long? ModificadoPor { get; set; }

        [Display(Name = "Fecha ModificaciÃ³n")]
        public DateTime? FechaModificacion { get; set; }
    }

    /// <summary>
    /// DTO para Proceso
    /// Tabla: GD_Procesos
    /// </summary>
    public class ProcesoDto
    {
        [Display(Name = "ID")]
        public long IdProceso { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(200, ErrorMessage = "El nombre no puede exceder 200 caracteres")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [StringLength(500, ErrorMessage = "La descripciÃ³n no puede exceder 500 caracteres")]
        [Display(Name = "DescripciÃ³n")]
        public string Descripcion { get; set; }

        [Display(Name = "CÃ³digo")]
        public string Codigo { get; set; }

        [Display(Name = "Responsable")]
        public long? IdResponsable { get; set; }

        [Display(Name = "Nombre Responsable")]
        public string NombreResponsable { get; set; }

        [Display(Name = "VersiÃ³n")]
        public decimal? Version { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Orden")]
        public int? Orden { get; set; }

        // AuditorÃ­a
        [Display(Name = "Registrado Por")]
        public long? RegistradoPor { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime? FechaRegistro { get; set; }

        [Display(Name = "Modificado Por")]
        public long? ModificadoPor { get; set; }

        [Display(Name = "Fecha ModificaciÃ³n")]
        public DateTime? FechaModificacion { get; set; }
    }

    /// <summary>
    /// DTO para Resumen de CatÃ¡logos
    /// </summary>
    public class CatalogosResumenDto
    {
        [Display(Name = "Total Tipos de Solicitud")]
        public int TotalTiposSolicitud { get; set; }

        [Display(Name = "Tipos Activos")]
        public int TiposActivos { get; set; }

        [Display(Name = "Total Estados")]
        public int TotalEstados { get; set; }

        [Display(Name = "Estados Activos")]
        public int EstadosActivos { get; set; }

        [Display(Name = "Total Procesos")]
        public int TotalProcesos { get; set; }

        [Display(Name = "Procesos Activos")]
        public int ProcesosActivos { get; set; }
    }
}

