using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.INV
{
    /// <summary>
    /// DTO para registrar legalizaciones de consumibles entregados.
    /// </summary>
    public class LegalizacionDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "El consumible es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "Seleccione un consumible válido")]
        public long IdConsumible { get; set; }

        public long UsuarioRegistra { get; set; }

        [Required(ErrorMessage = "El tipo de legalización es requerido")]
        [Range(1, short.MaxValue, ErrorMessage = "Seleccione un tipo de legalización válido")]
        public short TipoLegalizacion { get; set; }

        [Required(ErrorMessage = "El radicado es requerido")]
        [StringLength(100, ErrorMessage = "El radicado no puede exceder 100 caracteres")]
        public string Radicado { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El usuario responsable es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "Seleccione un usuario responsable válido")]
        public long IdUsuarioResponsable { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Las unidades deben ser mayor o igual a 0")]
        public int? Unidades { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "Las firmas deben ser mayor o igual a 0")]
        public long? Firmas { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "Las devoluciones deben ser mayor o igual a 0")]
        public long? Devoluciones { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "Las notas de crédito deben ser mayor o igual a 0")]
        public long? NotasCredito { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "El descuento de nómina debe ser mayor o igual a 0")]
        public long? DescuentoNomina { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "El valor legalizado debe ser mayor o igual a 0")]
        public long? ValorLegalizado { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "El pendiente debe ser mayor o igual a 0")]
        public long? Pendiente { get; set; }

        [StringLength(2000)]
        public string? Observaciones { get; set; }

        public bool Legalizado { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Seleccione un centro de costo")]
        public short? IdCentroCosto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una BU")]
        public int? IdBU { get; set; }

        public long? IdJobBook { get; set; }

        [StringLength(50)]
        public string? JobBookCodigo { get; set; }

        [StringLength(500)]
        public string? JobBookNombre { get; set; }

        [Range(0, long.MaxValue, ErrorMessage = "El valor de carrera debe ser mayor o igual a 0")]
        public long? ValorCarrera { get; set; }

        public bool Verificado { get; set; }
        public DateTime? FechaVerificacion { get; set; }
        public long? IdUsuarioVerifica { get; set; }

        // Propiedades adicionales para vista (denormalizadas)
        public long IdLegalizacion { get; set; }
        public DateTime FechaLegalizacion { get; set; }
        public long? Valor { get; set; } // Valor del consumible
        public string? NombreUsuario { get; set; } // Nombre del usuario que registra
    }
}
