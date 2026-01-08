using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Services.OP;

public interface IOpPortalService
{
    Task<OpPortalSnapshot> ObtenerPortalAsync(FiltrosVM filtros, long? idProyecto = null);
}
