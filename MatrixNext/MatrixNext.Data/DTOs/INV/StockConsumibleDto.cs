using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.INV
{
    /// <summary>
    /// DTO para registrar movimientos de stock de consumibles (entrada/salida).
    /// </summary>
    public class StockConsumibleDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "El consumible es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "Seleccione un consumible válido")]
        public long IdConsumible { get; set; }

        public long? NumeroVale { get; set; }

        [Required(ErrorMessage = "La fecha es requerida")]
        public DateTime Fecha { get; set; }

        public long UsuarioRegistra { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es requerido")]
        [Range(1, short.MaxValue, ErrorMessage = "Seleccione un tipo de movimiento válido")]
        public short TipoMovimiento { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Seleccione un estado")]
        public short? Estado { get; set; }

        [Range(1, short.MaxValue, ErrorMessage = "Seleccione un centro de costo")]
        public short? IdCentroCosto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una BU")]
        public int? IdBU { get; set; }

        public long? IdTrabajo { get; set; }

        [StringLength(50)]
        public string? JobBookCodigo { get; set; }

        [StringLength(500)]
        public string? JobBookNombre { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Seleccione una cuenta contable")]
        public long? IdCuentaContable { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Seleccione una ciudad")]
        public long? IdCiudad { get; set; }

        [Required(ErrorMessage = "El valor es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "El valor debe ser mayor a 0")]
        public long Valor { get; set; }

        [Required(ErrorMessage = "El total es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "El total debe ser mayor a 0")]
        public long Total { get; set; }

        public long Disponible { get; set; }

        // Solo requerido para salidas (asignación a usuario)
        public long? IdUsuarioAsignado { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un tipo de cargo")]
        public int? TipoCargo { get; set; }

        [StringLength(2000)]
        public string? Observaciones { get; set; }
    }
}
