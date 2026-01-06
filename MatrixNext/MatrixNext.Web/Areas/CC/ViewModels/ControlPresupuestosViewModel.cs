using MatrixNext.Data.Modules.CC.DTOs.ControlPresupuestos;

namespace MatrixNext.Web.Areas.CC.ViewModels
{
    /// <summary>
    /// ViewModel para Control de Presupuestos
    /// </summary>
    public class ControlPresupuestosViewModel
    {
        public List<PresupuestoDto> Presupuestos { get; set; } = new();
        public PresupuestoDto? PresupuestoSeleccionado { get; set; }
        public List<DetallePresupuestoDto> DetallesPresupuesto { get; set; } = new();
    }

    /// <summary>
    /// ViewModel para Verificación de Presupuestos
    /// </summary>
    public class VerificacionPresupuestosViewModel
    {
        public List<VerificacionPresupuestoDto> Verificaciones { get; set; } = new();
        public int? PeriodoFiltro { get; set; }
        public decimal TotalPresupuesto { get; set; }
        public decimal TotalRealizado { get; set; }
        public decimal TotalVarianza { get; set; }
    }

    /// <summary>
    /// ViewModel para Nómina y Distribución de Costos
    /// </summary>
    public class NominaDistribucionViewModel
    {
        public List<NominaDistribucionDto> Nominas { get; set; } = new();
        public int Periodo { get; set; }
        public decimal TotalLiquidado { get; set; }
    }

    /// <summary>
    /// ViewModel para Asignación de Presupuestos
    /// </summary>
    public class AsignacionPresupuestoViewModel
    {
        public long IdPresupuesto { get; set; }
        public List<AsignacionPresupuestoDto> Asignaciones { get; set; } = new();
        public decimal MontoTotalAsignado { get; set; }
        public decimal MontoTotalDisponible { get; set; }
        public decimal SaldoDisponible => MontoTotalDisponible - MontoTotalAsignado;
    }

    /// <summary>
    /// Request model para crear/editar presupuesto
    /// </summary>
    public class CrearPresupuestoRequest
    {
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public decimal MontoPresupuesto { get; set; }
        public byte Estado { get; set; }
    }

    /// <summary>
    /// Request model para crear distribución de costos
    /// </summary>
    public class CrearDistribucionRequest
    {
        public long IdEmpleado { get; set; }
        public long IdCentroCosto { get; set; }
        public decimal PorcentajeDistribucion { get; set; }
    }

    /// <summary>
    /// Request model para crear asignación de presupuesto
    /// </summary>
    public class CrearAsignacionRequest
    {
        public long IdPresupuesto { get; set; }
        public long IdActividad { get; set; }
        public decimal MontoAsignado { get; set; }
    }
}
