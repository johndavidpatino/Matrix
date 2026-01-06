using System.Collections.Generic;
using MatrixNext.Web.Areas.EQ.Services.Masters;

namespace MatrixNext.Web.Areas.EQ.Models
{
    public class EasyQuoteAdminViewModel
    {
        public List<EasyQuoteMasterService.PrecioRow> Precios { get; set; } = new();
        public List<EasyQuoteMasterService.HorasRow> Horas { get; set; } = new();
        public List<EasyQuoteMasterService.ValorHoraRow> ValorHoraOps { get; set; } = new();
        public List<EasyQuoteMasterService.CostInsumoRow> CostInsumos { get; set; } = new();
        public List<EasyQuoteMasterService.LocacionRow> Locaciones { get; set; } = new();
        public List<EasyQuoteMasterService.EnvioTarifaRow> Envios { get; set; } = new();
        public List<EasyQuoteMasterService.CodificacionRow> Codificacion { get; set; } = new();
        public List<EasyQuoteMasterService.MysteryTarifaRow> Mystery { get; set; } = new();
        public List<EasyQuoteMasterService.CostUnitarioOpsRow> CostUnitarios { get; set; } = new();
        public List<EasyQuoteMasterService.RateEstadisticaRow> Estadistica { get; set; } = new();
        public List<EasyQuoteMasterService.ParamMiscRow> ParamMisc { get; set; } = new();
        public EasyQuoteMasterService.EnvioParamRow EnvioParam { get; set; } = new();
        public List<EasyQuoteMasterService.ProductividadCiudadRow> Productividad { get; set; } = new();
        public List<EasyQuoteMasterService.BaseDatosRow> BaseDatos { get; set; } = new();
    }
}
