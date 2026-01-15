using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.ViewModels.CORE
{
    /// <summary>
    /// Resultado del SP CORE_Configuracion_TareasXTipoHilo_Get
    /// Representa una tarea y si está asignada a un tipo de hilo
    /// </summary>
    public class TareaPorTipoHiloVM
    {
        public long Id { get; set; }

        [Required]
        public string Tarea { get; set; } = string.Empty;

        public long? NoEmpiezaAntesDe { get; set; }
        public long? NoTerminaAntesDe { get; set; }
        public short? TiempoPromedioDias { get; set; }
        public bool? RequiereEstimacion { get; set; }
        public long? RolEstima { get; set; }
        public long? UnidadEjecuta { get; set; }
        public long? UnidadRecibe { get; set; }
        public long? RolEjecuta { get; set; }
        public bool? Visible { get; set; }
        public long? Orden { get; set; }

        // Campos de texto asociados en SP legacy
        public string? TextoRolEjecuta { get; set; }
        public string? TextoRolEstima { get; set; }
        public string? TextoUnidadEjecuta { get; set; }

        public bool Asignada { get; set; }

        // Alias para compatibilidad con vistas previas
        public string Nombre => Tarea;
    }
}
