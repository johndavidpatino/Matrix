namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Interface para servicio de paginación y filtrado
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 2
    /// </summary>
    public interface IGridService
    {
        /// <summary>
        /// Ejecuta una query con paginación, filtros y ordenamiento
        /// </summary>
        Task<PaginationResultVM<T>> PaginarAsync<T>(
            IQueryable<T> query,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Id",
            bool sortDescending = false
        ) where T : class;
    }

    public class PaginationResultVM<T> where T : class
    {
        public List<T> Items { get; set; } = new();
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public string SortBy { get; set; } = "Id";
        public bool SortDescending { get; set; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
