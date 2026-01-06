namespace MatrixNext.Web.ViewModels
{
    /// <summary>
    /// ViewModels base compartidas
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 5
    /// </summary>

    /// <summary>
    /// Resultado de paginación genérico para Grid
    /// </summary>
    public class PaginationResultVM<T>
    {
        public List<T> Items { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int TotalRecords => TotalCount;
    }

    /// <summary>
    /// Base para todos los ViewModels
    /// </summary>
    public class BaseVM
    {
        public long Id { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        public DateTime FechaModificacion { get; set; } = DateTime.UtcNow;
        public long UsuarioCreacion { get; set; }
        public long UsuarioModificacion { get; set; }
        public bool Activo { get; set; } = true;
    }

    /// <summary>
    /// Respuesta estándar para APIs/acciones
    /// </summary>
    public class ResultVM
    {
        public bool Exitoso { get; set; } = true;
        public string? Mensaje { get; set; }
        public List<ErrorVM> Errores { get; set; } = new();
        public object? Datos { get; set; }

        public static ResultVM Exito(string mensaje = "Operación exitosa", object? datos = null)
        {
            return new ResultVM { Exitoso = true, Mensaje = mensaje, Datos = datos };
        }

        public static ResultVM Error(string mensaje, List<ErrorVM>? errores = null)
        {
            return new ResultVM { Exitoso = false, Mensaje = mensaje, Errores = errores ?? new() };
        }
    }

    /// <summary>
    /// Error en respuesta
    /// </summary>
    public class ErrorVM
    {
        public string Campo { get; set; }
        public string Mensaje { get; set; }
    }

    /// <summary>
    /// Filtros comunes para búsquedas
    /// </summary>
    public class FiltrosVM
    {
        public string Busqueda { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int Estado { get; set; } = -1; // -1 = todos
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortBy { get; set; } = "FechaCreacion";
        public bool SortDescending { get; set; } = true;
    }
}
