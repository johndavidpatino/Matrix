using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace MatrixNext.Web.Areas.SGC
{
    /// <summary>
    /// Registro del área SGC (Sistema de Gestión de Calidad)
    /// Gestión de Auditorías Internas y Acciones de Mejora
    /// </summary>
    public class SGCAreaRegistration : IAreaModelConvention
    {
        public void Apply(AreaModel model)
        {
            if (model.AreaName != "SGC")
                return;

            model.Route.DataTokens["area"] = "SGC";
        }
    }
}
