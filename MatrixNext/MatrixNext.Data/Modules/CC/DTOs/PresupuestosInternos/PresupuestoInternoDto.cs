namespace MatrixNext.Data.Modules.CC.DTOs.PresupuestosInternos
{
    /// <summary>
    /// DTO para Presupuesto Interno
    /// </summary>
    public class PresupuestoInternoDto
    {
        public long IdPresupuestoInterno { get; set; }
        public int Periodo { get; set; }
        public string? CodigoEmpresa { get; set; }
        public string? NombreEmpresa { get; set; }
        public string? Division { get; set; }
        public decimal MontoTotal { get; set; }
        public decimal MontoUtilizado { get; set; }
        public decimal SaldoDisponible => MontoTotal - MontoUtilizado;
        public byte Estado { get; set; }
        public string? EstadoNombre { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaAprobacion { get; set; }
        public string? UsuarioCreacion { get; set; }
        public string? UsuarioAprobacion { get; set; }
        public List<DetallePresupuestoInternoDto> Detalles { get; set; } = new();
    }

    /// <summary>
    /// DTO para Detalle de Presupuesto Interno
    /// </summary>
    public class DetallePresupuestoInternoDto
    {
        public long IdDetalle { get; set; }
        public long IdPresupuestoInterno { get; set; }
        public string? CodigoLinea { get; set; }
        public string? DescripcionLinea { get; set; }
        public decimal MontoAsignado { get; set; }
        public decimal MontoEjecutado { get; set; }
        public decimal SaldoLinea => MontoAsignado - MontoEjecutado;
        public string? CentroCosto { get; set; }
        public string? CuentaContable { get; set; }
        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para Histórico de Cambios en Presupuesto Interno
    /// </summary>
    public class HistoricoPresupuestoInternoDto
    {
        public long IdHistorico { get; set; }
        public long IdPresupuestoInterno { get; set; }
        public DateTime FechaCambio { get; set; }
        public string? UsuarioCambio { get; set; }
        public string? TipoCambio { get; set; }
        public string? CampoModificado { get; set; }
        public string? ValorAnterior { get; set; }
        public string? ValorNuevo { get; set; }
        public string? Motivo { get; set; }
    }

    /// <summary>
    /// DTO para resumen de Presupuestos Internos
    /// </summary>
    public class ResumenPresupuestoInternoDto
    {
        public int Periodo { get; set; }
        public string? Empresa { get; set; }
        public int TotalPresupuestos { get; set; }
        public decimal MontoTotalAsignado { get; set; }
        public decimal MontoTotalUtilizado { get; set; }
        public decimal SaldoTotal { get; set; }
        public decimal PorcentajeEjecucion { get; set; }
        public int PresupuestosActivos { get; set; }
        public int PresupuestosCerrados { get; set; }
    }

    /// <summary>
    /// Request para crear/actualizar presupuesto interno
    /// </summary>
    public class GuardarPresupuestoInternoRequest
    {
        public long IdPresupuestoInterno { get; set; }
        public int Periodo { get; set; }
        public string? CodigoEmpresa { get; set; }
        public string? Division { get; set; }
        public decimal MontoTotal { get; set; }
        public byte Estado { get; set; }
        public List<DetallePresupuestoInternoDto> Detalles { get; set; } = new();
    }
}
