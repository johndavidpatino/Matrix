using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.DTOs.INV
{
    /// <summary>
    /// DTO para crear/editar registro de artículos en inventario.
    /// Soporta múltiples tipos: Computadores, Tablets, Celulares, Consumibles, Periféricos, Papelería, etc.
    /// </summary>
    public class RegistroArticuloDto
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "El tipo de artículo es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "Seleccione un tipo de artículo válido")]
        public long IdTipoArticulo { get; set; }

        [Required(ErrorMessage = "El artículo es requerido")]
        [Range(1, long.MaxValue, ErrorMessage = "Seleccione un artículo válido")]
        public long IdArticulo { get; set; }

        [Required(ErrorMessage = "La fecha de compra es requerida")]
        public DateTime? FechaCompra { get; set; }

        public long UsuarioRegistra { get; set; }

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

        [Range(0, long.MaxValue, ErrorMessage = "El valor debe ser mayor o igual a 0")]
        public long? ValorUnitario { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Seleccione un estado")]
        public long? IdEstado { get; set; }

        [StringLength(2000)]
        public string? Descripcion { get; set; }

        [StringLength(100)]
        public string? Symphony { get; set; }

        public long? IdFisico { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "Seleccione una sede")]
        public long? IdSede { get; set; }

        // === Campos específicos para COMPUTADORES ===
        public long? IdTipoComputador { get; set; }
        public short? PertenecePC { get; set; }

        [StringLength(100)]
        public string? Marca { get; set; }

        [StringLength(100)]
        public string? Modelo { get; set; }

        [StringLength(200)]
        public string? Procesador { get; set; }

        [StringLength(100)]
        public string? Memoria { get; set; }

        [StringLength(100)]
        public string? Almacenamiento { get; set; }

        [StringLength(100)]
        public string? SistemaOperativo { get; set; }

        [StringLength(200)]
        public string? Serial { get; set; }

        [StringLength(200)]
        public string? NombreEquipo { get; set; }

        [StringLength(200)]
        public string? Office { get; set; }

        [StringLength(500)]
        public string? Programas { get; set; }

        // === Campos específicos para SERVIDORES ===
        [StringLength(100)]
        public string? TipoServidor { get; set; }

        [StringLength(100)]
        public string? Raid { get; set; }

        // === Campos específicos para TABLETS ===
        public long? IdTablet { get; set; }
        public long? IdSTG { get; set; }

        [StringLength(50)]
        public string? TamanoPantalla { get; set; }

        // === Campos específicos para CELULARES ===
        public long? Chip { get; set; }
        public long? IMEI { get; set; }
        public long? Pertenece { get; set; }
        public long? Operador { get; set; }
        public long? NumeroCelular { get; set; }
        public int? CantidadMinutos { get; set; }

        // === Campos específicos para PERIFÉRICOS ===
        public long? IdTipoPeriferico { get; set; }

        // === Campos específicos para CONSUMIBLES/OBSEQUIOS ===
        public long? IdTipoProducto { get; set; }

        [StringLength(200)]
        public string? Producto { get; set; }

        public short? TipoObsequio { get; set; }
        public long? TipoBono { get; set; }

        // === Control de asignación ===
        public bool Asignado { get; set; }

        // === Campos para RENTAS ===
        public DateTime? FechaFinRenta { get; set; }

        // === Campos de proveedor ===
        public long? NumeroPV { get; set; }
        public long? ProveedorId { get; set; }

        // === Campos para PAPELERÍA ===
        [Range(1, long.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0")]
        public long? Cantidad { get; set; }

        public long? IdProductoPapeleria { get; set; }

        // Propiedades adicionales para vista (denormalizadas)
        public string? Placa { get; set; } // Placa del activo fijo
        public DateTime? FechaRegistro { get; set; }

        // Aliases de propiedades para compatibilidad con vistas
        public string? Ram => Memoria;
        public string? Disco => Almacenamiento;
        public string? VersionSO => SistemaOperativo;
        public string? Observaciones { get; set; }
        public DateTime? FechaModificacion { get; set; }
    }
}
