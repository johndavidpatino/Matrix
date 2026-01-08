using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Services.OP;

public interface IOpProductividadService
{
    Task<ProductividadViewModel> ObtenerProductividadAsync(string rol, CancellationToken cancellationToken = default);
}
