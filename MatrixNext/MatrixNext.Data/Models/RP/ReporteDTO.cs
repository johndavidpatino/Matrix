namespace MatrixNext.Data.Models.RP
{
    /// <summary>
    /// DTO para listar reportes disponibles
    /// </summary>
    public class ReporteDTO
    {
        public int ReporteId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public string Categoria { get; set; }
        public DateTime? UltimaGeneracion { get; set; }
        public bool Disponible { get; set; }
    }

    /// <summary>
    /// Filtros avanzados para reportes
    /// </summary>
    public class ReporteFiltrosDTO
    {
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int? UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Estado { get; set; }
        public string Proyecto { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }

    /// <summary>
    /// Resultado genérico de reporte
    /// </summary>
    public class ReporteResultadoDTO
    {
        public List<Dictionary<string, object>> Datos { get; set; } = new();
        public int TotalRegistros { get; set; }
        public int Pagina { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalPaginas => (TotalRegistros + RegistrosPorPagina - 1) / RegistrosPorPagina;
        public bool TienePaginas => TotalPaginas > 1;
    }

    /// <summary>
    /// Modelo para exportación de reportes
    /// </summary>
    public class ReporteExportDTO
    {
        public string Nombre { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public string Usuario { get; set; }
        public byte[] Contenido { get; set; }
        public string ContentType { get; set; }
    }

    /// <summary>
    /// Categorías de reportes
    /// </summary>
    public static class CategoriasReporte
    {
        public const string INDICADORES = "Indicadores";
        public const string OPERACION = "Operación";
        public const string PLANEACION = "Planeación";
        public const string RECURSOS = "Recursos";
        public const string ESPECIALIZADOS = "Especializados";
    }

    /// <summary>
    /// Estados de reporte
    /// </summary>
    public static class EstadoReporte
    {
        public const string GENERANDO = "Generando";
        public const string LISTO = "Listo";
        public const string ERROR = "Error";
    }
}
