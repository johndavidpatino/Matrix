using System.Collections.Generic;

namespace MatrixNext.Web.Models.ViewModels.GD
{
    public class CatalogosResponseVM
    {
        public List<TipoSolicitudViewModel> Tipos { get; set; } = new();
        public List<EstadoSolicitudViewModel> Estados { get; set; } = new();
        public List<ProcesoViewModel> Procesos { get; set; } = new();
    }
}
