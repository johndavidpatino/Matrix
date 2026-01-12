using System.ComponentModel.DataAnnotations;

namespace MatrixNext.Data.Models.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para vista completa del PNC (maestro + causas + acciones)
    /// Usado en detalle y seguimiento
    /// Origen: Multiple tablas (PNC_ProductoNoConforme + Causas + Acciones)
    /// </summary>
    public class ProductoNoConformeDetalleVM
    {
        // Datos maestro PNC
        public ProductoNoConformeVM Pnc { get; set; } = new();

        // Causas con acciones anidadas
        public List<ProductoNoConformeCausaDetalleVM> Causas { get; set; } = new();

        // Propiedades calculadas
        public int TotalCausas => Causas?.Count ?? 0;
        
        public int TotalAcciones => Causas?.Sum(c => c.Acciones?.Count ?? 0) ?? 0;
        
        public int AccionesPendientes => Causas?
            .SelectMany(c => c.Acciones ?? new List<ProductoNoConformeAccionVM>())
            .Count(a => !a.FechaEjecucion.HasValue) ?? 0;

        public int AccionesVencidas => Causas?
            .SelectMany(c => c.Acciones ?? new List<ProductoNoConformeAccionVM>())
            .Count(a => a.EstaVencida) ?? 0;

        public bool PuedeSerCerrado => TotalCausas > 0 
            && TotalAcciones > 0 
            && AccionesPendientes == 0 
            && !Pnc.Cerrado;

        public double PorcentajeAvance
        {
            get
            {
                if (TotalAcciones == 0) return 0;
                var ejecutadas = TotalAcciones - AccionesPendientes;
                return Math.Round((double)ejecutadas / TotalAcciones * 100, 2);
            }
        }
    }

    /// <summary>
    /// ViewModel para causa con sus acciones
    /// </summary>
    public class ProductoNoConformeCausaDetalleVM
    {
        public int Id { get; set; }
        public int IdPNC { get; set; }
        public string CausaRaiz { get; set; } = string.Empty;
        public List<ProductoNoConformeAccionVM> Acciones { get; set; } = new();

        // Validaciones
        public bool TieneAccionInmediata => Acciones?.Any(a => a.TipoAccion == (int)TipoAccionEnum.Inmediata) ?? false;
        public int AccionesPendientes => Acciones?.Count(a => !a.FechaEjecucion.HasValue) ?? 0;
    }
}
