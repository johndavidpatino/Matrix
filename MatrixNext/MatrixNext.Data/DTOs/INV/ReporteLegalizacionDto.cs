namespace MatrixNext.Data.DTOs.INV;

/// <summary>
/// DTO para reporte de legalizaciones de inventario
/// SP: INV_ReporteLegalizaciones
/// </summary>
public class ReporteLegalizacionDto
{
    public long Id { get; set; }
    public long? IdConsumible { get; set; }
    public string? Articulo { get; set; }
    public string? TipoProducto { get; set; }
    public string? Producto { get; set; }
    public string? TipoBono { get; set; }
    public DateTime? FechaEntrega { get; set; }
    public string? UsuarioAsignado { get; set; }
    public string? Cedula { get; set; }
    public string? TipoCargo { get; set; }
    public int? Unidades { get; set; }
    public decimal? ValorCarrera { get; set; }
    public decimal? ValorTotal { get; set; }
    public string? JobBookCodigo { get; set; }
    public string? JobBookNombre { get; set; }
    public string? BU { get; set; }
    public string? Observacion { get; set; }
    public int? Firmas { get; set; }
    public int? Devoluciones { get; set; }
    public decimal? NotasCredito { get; set; }
    public decimal? DescuentoNomina { get; set; }
    public bool? Legalizado { get; set; }
    public DateTime? FechaLegalizacion { get; set; }
}

/// <summary>
/// Filtros para reporte de legalizaciones
/// </summary>
public class ReporteLegalizacionFiltrosDto
{
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public long? UsuarioAsignado { get; set; }
    public long? Articulo { get; set; }
    public int? BU { get; set; }
    public string? JobBookCodigo { get; set; }
    public string? TodosCampos { get; set; }
    public int Pagina { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

/// <summary>
/// DTO para reporte de remanente de inventario
/// SP: INV_ReporteRemanente
/// </summary>
public class ReporteRemanenteDto
{
    public long? IdConsumible { get; set; }
    public string? Articulo { get; set; }
    public string? TipoProducto { get; set; }
    public string? Producto { get; set; }
    public string? TipoObsequio { get; set; }
    public string? EstadoProducto { get; set; }
    public string? TipoBono { get; set; }
    public DateTime? Fecha { get; set; }
    public string? JobBookCodigo { get; set; }
    public string? JobBookNombre { get; set; }
    public int? Total { get; set; }
    public int? Disponible { get; set; }
}

/// <summary>
/// Filtros para reporte de remanente
/// </summary>
public class ReporteRemanenteFiltrosDto
{
    public long? IdConsumible { get; set; }
    public long? Articulo { get; set; }
    public long? TipoProducto { get; set; }
    public string? JobBook { get; set; }
    public int Pagina { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}
