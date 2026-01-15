using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Web.ViewModels.CORE
{
    /// <summary>
    /// Resultado del SP CORE_Configuracion_DocumentosXTarea_Get
    /// Representa un documento disponible y si está asignado a la tarea
    /// </summary>
    public class DocumentoPorTareaVM
    {
        public long Id { get; set; }

        public long TareaId { get; set; }

        public long? IdDocumento { get; set; }

        [Required]
        public string Documento { get; set; } = string.Empty;

        public bool? Controlado { get; set; }
        public bool? Activo { get; set; }
        public string? Codigo { get; set; }
        public long? IdProceso { get; set; }
        public string? Responsable { get; set; }
        public string? URL { get; set; }
        public bool? EsOpcional { get; set; }
        public bool Asignado { get; set; }
    }
}
