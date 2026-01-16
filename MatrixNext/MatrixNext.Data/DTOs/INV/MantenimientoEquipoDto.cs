using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.INV
{
    /// <summary>
    /// DTO para registrar mantenimientos de equipos (preventivos/correctivos).
    /// </summary>
    public class MantenimientoEquipoDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "El activo fijo es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "Seleccione un activo fijo válido")]
        public long IdActivoFijo { get; set; }

        public long UsuarioRegistra { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El tipo de mantenimiento es requerido")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de mantenimiento válido")]
        public int TipoMantenimiento { get; set; }

        [Required(ErrorMessage = "El usuario responsable es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "Seleccione un usuario responsable válido")]
        public long IdUsuarioResponsable { get; set; }

        [Required(ErrorMessage = "Las observaciones son requeridas")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Las observaciones deben tener entre 10 y 2000 caracteres")]
        public string Observaciones { get; set; } = string.Empty;

        // Propiedades adicionales para vista (denormalizadas)
        public long IdMantenimiento { get; set; }
        public string? PlacaActivo { get; set; }
        public string? MarcaModelo { get; set; }
        public string? TipoArticulo { get; set; }
        public string? NombreRegistrador { get; set; }
    }
}
