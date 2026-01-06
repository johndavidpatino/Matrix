using MatrixNext.Data.Modules.CC.DTOs;

namespace MatrixNext.Web.Areas.CC.ViewModels
{
    /// <summary>
    /// FinzOpe main view model
    /// </summary>
    public class CcFinzOpeViewModel
    {
        // Liquidación
        public int IdPeriodo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public CcLiquidacionDto? LiquidacionResult { get; set; }

        // Bonificaciones
        public List<CcBonificacionDto>? Bonificaciones { get; set; }

        // Producción
        public decimal ProduccionTotal { get; set; }

        // Metadata
        public bool IsLoading { get; set; } = false;
        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }
    }

    /// <summary>
    /// Request model for liquidación
    /// </summary>
    public class LiquidacionRequest
    {
        public int IdPeriodo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }

    /// <summary>
    /// Request model for producción
    /// </summary>
    public class ProduccionRequest
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int? IdTrabajo { get; set; }
    }
}
