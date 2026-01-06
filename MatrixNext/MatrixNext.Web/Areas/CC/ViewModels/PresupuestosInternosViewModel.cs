using MatrixNext.Data.Modules.CC.DTOs.PresupuestosInternos;

namespace MatrixNext.Web.Areas.CC.ViewModels
{
    /// <summary>
    /// ViewModel para Index de Presupuestos Internos
    /// </summary>
    public class PresupuestosInternosIndexViewModel
    {
        public int? PeriodoFiltro { get; set; }
        public string? CodigoEmpresaFiltro { get; set; }
        public byte? EstadoFiltro { get; set; }
    }

    /// <summary>
    /// ViewModel para creación/edición de presupuesto interno
    /// </summary>
    public class GuardarPresupuestoInternoViewModel
    {
        public long IdPresupuesto { get; set; }
        public int Periodo { get; set; }
        public string CodigoEmpresa { get; set; } = string.Empty;
        public string? CodigoDivision { get; set; }
        public decimal MontoTotal { get; set; }
        public string? Observaciones { get; set; }
        public List<DetallePresupuestoInternoViewModel> Detalles { get; set; } = new();
    }

    /// <summary>
    /// ViewModel para detalle de línea presupuestal
    /// </summary>
    public class DetallePresupuestoInternoViewModel
    {
        public long IdDetalle { get; set; }
        public string CodigoCentroCosto { get; set; } = string.Empty;
        public decimal MontoAsignado { get; set; }
        public string? Descripcion { get; set; }
    }

    /// <summary>
    /// ViewModel para aprobación
    /// </summary>
    public class AprobarPresupuestoViewModel
    {
        public long IdPresupuesto { get; set; }
        public string Usuario { get; set; } = string.Empty;
    }

    /// <summary>
    /// ViewModel para filtros de exportación
    /// </summary>
    public class ExportarPresupuestosViewModel
    {
        public int? Periodo { get; set; }
        public string? CodigoEmpresa { get; set; }
        public byte? Estado { get; set; }
    }
}
