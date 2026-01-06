namespace MatrixNext.Data.Modules.CC.DTOs
{
    /// <summary>
    /// DTO for CC_FinzOpe Liquidation (Liquidación de Planillas)
    /// Maps result from CC_LiquidarPlanillas stored procedure
    /// </summary>
    public class CcLiquidacionDto
    {
        public int IdPeriodo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        
        public decimal TotalHoras { get; set; }
        public decimal TotalValor { get; set; }
        public decimal TotalBonificacion { get; set; }
        public decimal TotalDescuentos { get; set; }
        
        public decimal MontoNetoAPagar
        {
            get { return TotalValor + TotalBonificacion - TotalDescuentos; }
        }

        public List<CcProduccionDto> Producciones { get; set; } = new();
        public List<CcBonificacionDto> Bonificaciones { get; set; } = new();
        public List<CcDescuentoDto> Descuentos { get; set; } = new();
    }

    public class CcProduccionDto
    {
        public int IdProduccion { get; set; }
        public string? Cedula { get; set; }
        public string? NombrePersona { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Horas { get; set; }
        public decimal ValorProduccion { get; set; }
        public string? Descripcion { get; set; }
    }

    public class CcBonificacionDto
    {
        public int IdBonificacion { get; set; }
        public string? Cedula { get; set; }
        public decimal PorcentajeBonificacion { get; set; }
        public decimal ValorProduccion { get; set; }
        
        public decimal ValorBonificacion
        {
            get { return ValorProduccion * (PorcentajeBonificacion / 100); }
        }
    }

    public class CcDescuentoDto
    {
        public int IdDescuento { get; set; }
        public string? Cedula { get; set; }
        public string? TipoDescuento { get; set; }
        public decimal Valor { get; set; }
    }
}
