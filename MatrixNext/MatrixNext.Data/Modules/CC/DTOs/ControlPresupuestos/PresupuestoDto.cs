namespace MatrixNext.Data.Modules.CC.DTOs.ControlPresupuestos
{
    /// <summary>
    /// DTO para Presupuestos (Control Presupuestos)
    /// </summary>
    public class PresupuestoDto
    {
        public long IdPresupuesto { get; set; }
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public string? CodigoTrabajo { get; set; }
        public string? NombreTrabajo { get; set; }
        public decimal MontoPresupuesto { get; set; }
        public decimal MontoRealizado { get; set; }
        public decimal Varianza => MontoRealizado - MontoPresupuesto;
        public byte Estado { get; set; }
        public string? EstadoNombre { get; set; }
        public DateTime FechaRegistro { get; set; }
        public List<DetallePresupuestoDto> Detalles { get; set; } = new();
    }

    /// <summary>
    /// DTO para Detalle de Presupuesto
    /// </summary>
    public class DetallePresupuestoDto
    {
        public long IdDetallePresupuesto { get; set; }
        public long IdPresupuesto { get; set; }
        public long IdActividad { get; set; }
        public string? CodigoActividad { get; set; }
        public string? DescripcionActividad { get; set; }
        public decimal Cantidad { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal Subtotal => Cantidad * ValorUnitario;
    }

    /// <summary>
    /// DTO para Verificación de Presupuesto vs. Realizado
    /// </summary>
    public class VerificacionPresupuestoDto
    {
        public long IdPresupuesto { get; set; }
        public int Periodo { get; set; }
        public long IdTrabajo { get; set; }
        public string? NombreTrabajo { get; set; }
        public decimal MontoPresupuesto { get; set; }
        public decimal MontoRealizado { get; set; }
        public decimal Varianza { get; set; }
        public decimal PorcentajeVarianza { get; set; }
        public string? Estado { get; set; }
    }

    /// <summary>
    /// DTO para Nomina y Distribución de Costos
    /// </summary>
    public class NominaDistribucionDto
    {
        public long IdEmpleado { get; set; }
        public string? NombreEmpleado { get; set; }
        public string? Cedula { get; set; }
        public int Periodo { get; set; }
        public decimal ValorLiquidado { get; set; }
        public List<DistribucionPorCentroDto> Distribuciones { get; set; } = new();
    }

    /// <summary>
    /// DTO para Distribución por Centro de Costo
    /// </summary>
    public class DistribucionPorCentroDto
    {
        public long IdDistribucion { get; set; }
        public long IdCentroCosto { get; set; }
        public string? CodigoCentroCosto { get; set; }
        public string? NombreCentroCosto { get; set; }
        public decimal PorcentajeDistribucion { get; set; }
        public decimal ValorDistribuido { get; set; }
    }

    /// <summary>
    /// DTO para Asignación de Presupuesto a Actividades
    /// </summary>
    public class AsignacionPresupuestoDto
    {
        public long IdAsignacion { get; set; }
        public long IdPresupuesto { get; set; }
        public long IdActividad { get; set; }
        public string? CodigoActividad { get; set; }
        public string? DescripcionActividad { get; set; }
        public decimal MontoAsignado { get; set; }
        public decimal MontoUtilizado { get; set; }
        public decimal SaldoDisponible => MontoAsignado - MontoUtilizado;
        public byte Estado { get; set; }
    }
}
