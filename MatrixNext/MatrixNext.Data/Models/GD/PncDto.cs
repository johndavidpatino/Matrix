using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Models.GD
{
    /// <summary>
    /// DTO para Producto No Conforme (PNC_Productos)
    /// SP: PNC_Productos_Get, PNC_Productos_Add
    /// </summary>
    public class PncDto
    {
        public long Id { get; set; }
        
        /// <summary>
        /// 1=JBE (Proyecto), 2=JBI (Trabajo), 3=Actividad
        /// </summary>
        public byte AsociadoA { get; set; }
        public string? AsociadoADescripcion { get; set; }
        
        public long? ProyectoId { get; set; }
        public long? TrabajoId { get; set; }
        public string? ProyectoTrabajo { get; set; }
        
        public byte Proceso { get; set; }
        public string? ProcesoDescripcion { get; set; }
        
        public byte Procedimiento { get; set; }
        public string? ProcedimientoDescripcion { get; set; }
        
        public byte Unidad { get; set; }
        public string? UnidadDescripcion { get; set; }
        
        public long PersonaIdentifica { get; set; }
        public string? PersonaIdentificaNombre { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime FechaReclamo { get; set; }
        
        public int Fuente { get; set; }
        public string? FuenteDescripcion { get; set; }
        
        public int Categoria { get; set; }
        public string? CategoriaDescripcion { get; set; }
        
        public long? Tarea { get; set; }
        public string? TareaDescripcion { get; set; }
        
        public long Responsable { get; set; }
        public string? ResponsableNombre { get; set; }
        
        public long InformarA { get; set; }
        public string? InformarANombre { get; set; }
        
        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; } = string.Empty;
        
        /// <summary>
        /// Estado del PNC: 1=Abierto, 2=Cerrado, etc.
        /// </summary>
        public byte Estado { get; set; }
        public string? EstadoDescripcion { get; set; }
        
        public DateTime FechaCreacion { get; set; }
        public long Usuario { get; set; }
    }

    /// <summary>
    /// DTO para crear un nuevo PNC
    /// SP: PNC_Productos_Add
    /// </summary>
    public class PncCrearDto
    {
        [Required]
        public byte AsociadoA { get; set; }
        
        public long? ProyectoId { get; set; }
        public long? TrabajoId { get; set; }
        
        [Required]
        public byte Proceso { get; set; }
        
        [Required]
        public byte Procedimiento { get; set; }
        
        [Required]
        public byte Unidad { get; set; }
        
        [Required]
        public long PersonaIdentifica { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaReclamo { get; set; }
        
        [Required]
        public int Fuente { get; set; }
        
        [Required]
        public int Categoria { get; set; }
        
        public long? Tarea { get; set; }
        
        [Required]
        public long Responsable { get; set; }
        
        [Required]
        public long InformarA { get; set; }
        
        [Required(ErrorMessage = "La descripción es requerida")]
        public string Descripcion { get; set; } = string.Empty;
    }

    /// <summary>
    /// DTO para Causa de PNC (PNC_Productos_Causas)
    /// SP: PNC_Productos_Causas_Add, PNC_ProductoNoConformeCausas_Get
    /// </summary>
    public class PncCausaDto
    {
        public long Id { get; set; }
        public long ProductoId { get; set; }
        
        [Required(ErrorMessage = "La causa es requerida")]
        public string Causa { get; set; } = string.Empty;
        
        public string? Correccion { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? FechaEstimadaCierre { get; set; }
        
        public long Usuario { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    /// <summary>
    /// DTO para Acción de PNC
    /// SP: PNC_ProductoNoConformeAcciones_Get
    /// </summary>
    public class PncAccionDto
    {
        public long Id { get; set; }
        public long ProductoId { get; set; }
        public long CausaId { get; set; }
        
        [Required(ErrorMessage = "La acción es requerida")]
        public string Accion { get; set; } = string.Empty;
        
        public string? Responsable { get; set; }
        public long? ResponsableId { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? FechaCompromiso { get; set; }
        
        [DataType(DataType.Date)]
        public DateTime? FechaCierre { get; set; }
        
        public string? Observacion { get; set; }
        public byte Estado { get; set; }
        public long Usuario { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    /// <summary>
    /// DTO para log de estados del PNC
    /// SP: PNC_Productos_Log_Get, PNC_Productos_Log_Estado_Add
    /// </summary>
    public class PncLogEstadoDto
    {
        public long Id { get; set; }
        public long IdProducto { get; set; }
        public byte Estado { get; set; }
        public string? EstadoDescripcion { get; set; }
        public DateTime Fecha { get; set; }
        public long Usuario { get; set; }
        public string? UsuarioNombre { get; set; }
        public string? Observacion { get; set; }
    }

    /// <summary>
    /// DTO para seguimiento de PNC
    /// SP: PNC_Seguimiento_Get
    /// </summary>
    public class PncSeguimientoDto
    {
        public long Id { get; set; }
        public long? IdEstudio { get; set; }
        public string? NombreEstudio { get; set; }
        public long? IdTrabajo { get; set; }
        public string? NombreTrabajo { get; set; }
        public string? JobBook { get; set; }
        public DateTime FechaReclamo { get; set; }
        public long IdReporta { get; set; }
        public string? Reporta { get; set; }
        public int IdUnidad { get; set; }
        public string? Unidad { get; set; }
        public long IdClienteExterno { get; set; }
        public string? Cliente { get; set; }
        public int IdFuenteReclamo { get; set; }
        public string? FuenteReclamo { get; set; }
        public int IdCategoria { get; set; }
        public string? Categoria { get; set; }
        public long? IdTarea { get; set; }
        public string? Tarea { get; set; }
        public string? Descripcion { get; set; }
        public int? CantidadCausas { get; set; }
        public int? CantidadAcciones { get; set; }
        
        /// <summary>
        /// Estado calculado: Cerrado, No tiene causas, No tiene acciones, Gestionado
        /// </summary>
        public string Estado { get; set; } = string.Empty;
    }

    /// <summary>
    /// Parámetros de búsqueda para PNC
    /// </summary>
    public class PncBusquedaParams
    {
        public long? Id { get; set; }
        public long? Responsable { get; set; }
        public byte? Estado { get; set; }
        public long? UsuarioRegistra { get; set; }
    }

    /// <summary>
    /// ViewModel para la vista Index de PNC
    /// </summary>
    public class PncIndexViewModel
    {
        public IEnumerable<PncDto> Productos { get; set; } = new List<PncDto>();
        public IEnumerable<PncSeguimientoDto> Seguimiento { get; set; } = new List<PncSeguimientoDto>();
        public PncBusquedaParams Filtros { get; set; } = new PncBusquedaParams();
        
        // Catálogos para filtros
        public IEnumerable<CatalogoItem> Estados { get; set; } = new List<CatalogoItem>();
        public IEnumerable<CatalogoItem> Procesos { get; set; } = new List<CatalogoItem>();
        public IEnumerable<CatalogoItem> Categorias { get; set; } = new List<CatalogoItem>();
        public IEnumerable<CatalogoItem> Fuentes { get; set; } = new List<CatalogoItem>();
    }

    /// <summary>
    /// Item genérico para catálogos
    /// </summary>
    public class CatalogoItem
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }

    /// <summary>
    /// ViewModel para detalle de PNC
    /// </summary>
    public class PncDetalleViewModel
    {
        public PncDto Producto { get; set; } = new PncDto();
        public IEnumerable<PncCausaDto> Causas { get; set; } = new List<PncCausaDto>();
        public IEnumerable<PncAccionDto> Acciones { get; set; } = new List<PncAccionDto>();
        public IEnumerable<PncLogEstadoDto> HistorialEstados { get; set; } = new List<PncLogEstadoDto>();
    }
}
