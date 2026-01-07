using MatrixNext.Web.ViewModels;

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
}
