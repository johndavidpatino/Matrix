using Microsoft.EntityFrameworkCore;
using System.Reflection;
using MatrixNext.Web.ViewModels;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Implementación de IGridService con LINQ
    /// Ref: ESPECIFICACION_COMPONENTES_COMPARTIDOS.md § 2.2
    /// </summary>
    public class GridService : IGridService
    {
        private readonly ILogger<GridService> _logger;

        public GridService(ILogger<GridService> logger)
        {
            _logger = logger;
        }

        public async Task<PaginationResultVM<T>> PaginarAsync<T>(
            IQueryable<T> query,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Id",
            bool sortDescending = false
        ) where T : class
        {
            // Validar
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            try
            {
                // Contar total
                int total = await query.CountAsync();

                // Aplicar ordenamiento
                if (!string.IsNullOrEmpty(sortBy))
                {
                    var prop = typeof(T).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Public);
                    if (prop != null)
                    {
                        query = sortDescending
                            ? query.OrderByDescending(x => EF.Property<object>(x, prop.Name))
                            : query.OrderBy(x => EF.Property<object>(x, prop.Name));
                    }
                }

                // Aplicar paginación
                int offset = (pageNumber - 1) * pageSize;
                var items = await query
                    .Skip(offset)
                    .Take(pageSize)
                    .ToListAsync();

                return new PaginationResultVM<T>
                {
                    Items = items,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalCount = total,
                    SortBy = sortBy,
                    SortDescending = sortDescending
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error paginando {typeof(T).Name}");
                throw;
            }
        }
    }
}
