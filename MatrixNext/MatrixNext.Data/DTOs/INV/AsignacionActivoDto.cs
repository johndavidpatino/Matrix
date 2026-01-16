using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.INV
{
    /// <summary>
    /// DTO para asignar activos fijos (tablets, computadores, periféricos) a empleados.
    /// </summary>
    public class AsignacionActivoDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "El activo fijo es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "Seleccione un activo fijo válido")]
        public long IdActivoFijo { get; set; }

        public long UsuarioRegistra { get; set; }

        [Required(ErrorMessage = "La fecha de asignación es requerida")]
        public DateTime FechaAsignacion { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Seleccione un centro de costo")]
        public short? IdCentroCosto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una BU")]
        public int? IdBU { get; set; }

        public long? IdTrabajo { get; set; }

        [StringLength(50)]
        public string? JobBookCodigo { get; set; }

        [StringLength(500)]
        public string? JobBookNombre { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Seleccione una ciudad")]
        public long? IdCiudad { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Seleccione el estado de la tablet")]
        public long? IdEstadoTablet { get; set; }

        [Required(ErrorMessage = "El usuario asignado es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "Seleccione un usuario válido")]
        public long IdUsuarioAsignado { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Seleccione el tipo de cargo")]
        public short? TipoCargo { get; set; }

        [StringLength(200)]
        public string? Cargo { get; set; }

        [StringLength(2000)]
        public string? Observacion { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una sede")]
        public int? IdSede { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Seleccione el tipo de grupo/unidad")]
        public short? TipoGrupoUnidad { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un grupo/unidad")]
        public int? IdGrupoUnidad { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una unidad")]
        public int? IdUnidad { get; set; }

        // Propiedades adicionales para vista (denormalizadas)
        public long IdAsignacion { get; set; }
        public string? PlacaActivo { get; set; }
        public string? TipoArticulo { get; set; }
        public DateTime? FechaDevolucion { get; set; }
        public string? MarcaModelo { get; set; }
        public string? SerialActivo { get; set; }
        public string? NombreUsuario { get; set; }
        public string? NombreRegistrador { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public string? Observaciones => Observacion; // Alias para vistas
    }
}
