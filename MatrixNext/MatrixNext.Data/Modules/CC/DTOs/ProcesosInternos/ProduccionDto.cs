namespace MatrixNext.Data.Modules.CC.DTOs.ProcesosInternos
{
    /// <summary>
    /// DTO para Producción (consolidación)
    /// </summary>
    public class ProduccionDto
    {
        public long IdProduccion { get; set; }
        public long IdTrabajo { get; set; }
        public string? CodigoTrabajo { get; set; }
        public string? NombreTrabajo { get; set; }
        public int Periodo { get; set; }
        public DateTime FechaProduccion { get; set; }
        public int CantidadProducida { get; set; }
        public int CantidadConsolidada { get; set; }
        public bool EstaConsolidado => CantidadConsolidada > 0;
        public string? UsuarioConsolida { get; set; }
        public DateTime? FechaConsolidacion { get; set; }
        public byte Estado { get; set; }
    }

    /// <summary>
    /// DTO para consolidar producción
    /// </summary>
    public class ConsolidarProduccionRequest
    {
        public long IdProduccion { get; set; }
        public int CantidadConsolidada { get; set; }
        public string UsuarioConsolida { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
    }

    /// <summary>
    /// DTO para resumen de consolidación
    /// </summary>
    public class ResumenConsolidacionDto
    {
        public int TotalRegistros { get; set; }
        public int TotalConsolidados { get; set; }
        public int TotalPendientes => TotalRegistros - TotalConsolidados;
        public decimal PorcentajeConsolidado => TotalRegistros > 0 
            ? Math.Round((decimal)TotalConsolidados / TotalRegistros * 100, 2) 
            : 0;
        public int TotalCantidadProducida { get; set; }
        public int TotalCantidadConsolidada { get; set; }
    }
}
