using System;
using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Modules.TH.HWH.Models
{
    /// <summary>
    /// DTO para solicitud de Easy Work / Teletrabajo
    /// </summary>
    public class HWHDto
    {
        public long Id { get; set; }
        public long Usuario { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public DateTime FechaProgramada { get; set; }
        public string FechaProgramadaStr => FechaProgramada.ToString("dd/MM/yyyy");
        public int Estado { get; set; }
        public string NombreEstado { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? UsuarioGestion { get; set; }
        public string? ObservacionesGestion { get; set; }
        public long? JefeDirecto { get; set; }
        public string? NombreJefe { get; set; }
        
        /// <summary>
        /// Clase CSS para el badge del estado
        /// </summary>
        public string EstadoBadgeClass => Estado switch
        {
            1 => "badge-info",       // Pendiente
            2 => "badge-success",    // Aprobado
            3 => "badge-warning",    // Rechazado
            4 => "badge-danger",     // Anulado
            _ => "badge-secondary"
        };
        
        /// <summary>
        /// Indica si se puede anular la solicitud
        /// </summary>
        public bool PuedeAnular => Estado == 1 || Estado == 2;
        
        /// <summary>
        /// Indica si se puede aprobar/rechazar la solicitud
        /// </summary>
        public bool PuedeGestionar => Estado == 1;
    }
    
    /// <summary>
    /// DTO para crear una solicitud de Easy Work
    /// </summary>
    public class HWHCreateDto
    {
        [Required(ErrorMessage = "El documento del empleado es obligatorio")]
        public long Usuario { get; set; }
        
        [Required(ErrorMessage = "La fecha del Easy Work es obligatoria")]
        public DateTime FechaProgramada { get; set; }
        
        public string? Observaciones { get; set; }
    }
    
    /// <summary>
    /// DTO para gestionar (aprobar/rechazar/anular) una solicitud
    /// </summary>
    public class HWHGestionDto
    {
        [Required(ErrorMessage = "El ID de la solicitud es obligatorio")]
        public long Id { get; set; }
        
        [Required(ErrorMessage = "El nuevo estado es obligatorio")]
        public int Estado { get; set; }
        
        public string? Observaciones { get; set; }
    }
    
    /// <summary>
    /// DTO para búsqueda de solicitudes
    /// </summary>
    public class HWHBusquedaParams
    {
        public long? Id { get; set; }
        public long? Usuario { get; set; }
        public long? JefeDirecto { get; set; }
        public int? Estado { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
    
    /// <summary>
    /// DTO para vista de Gantt (calendario)
    /// </summary>
    public class HWHGanttDto
    {
        public long Id { get; set; }
        public long Usuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string FechaInicio { get; set; } = string.Empty;
        public string FechaFinal { get; set; } = string.Empty;
    }
    
    /// <summary>
    /// DTO para datos del Gantt completo
    /// </summary>
    public class HWHGanttResult
    {
        public List<HWHGanttSerie> Series { get; set; } = new();
        public DateTime FechaInicial { get; set; }
        public DateTime FechaFinal { get; set; }
    }
    
    /// <summary>
    /// DTO para serie individual del Gantt
    /// </summary>
    public class HWHGanttSerie
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Parent { get; set; } = "Listado_TeleTrabajo";
        public string FStart { get; set; } = string.Empty;
        public string FEnd { get; set; } = string.Empty;
        public string Owner { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
    }
    
    /// <summary>
    /// DTO para jefes que aprueban
    /// </summary>
    public class JefeAprobadorDto
    {
        public long Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Area { get; set; }
    }
    
    /// <summary>
    /// Estados de Easy Work
    /// </summary>
    public static class HWHEstados
    {
        public const int Pendiente = 1;
        public const int Aprobado = 2;
        public const int Rechazado = 3;
        public const int Anulado = 4;
        
        public static string ObtenerNombre(int estado) => estado switch
        {
            Pendiente => "Pendiente",
            Aprobado => "Aprobado",
            Rechazado => "Rechazado",
            Anulado => "Anulado",
            _ => "Desconocido"
        };
    }
}
