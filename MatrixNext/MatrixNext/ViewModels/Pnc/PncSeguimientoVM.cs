using System.ComponentModel.DataAnnotations;

namespace MatrixNext.ViewModels.Pnc
{
    /// <summary>
    /// ViewModel para vista de seguimiento PNC
    /// Origen: SP PNC_Seguimiento_Get (sistema avanzado)
    /// Muestra estado general de PNC con KPIs
    /// </summary>
    public class PncSeguimientoVM
    {
        [Display(Name = "Total PNC Activos")]
        public int TotalPncActivos { get; set; }

        [Display(Name = "PNC Cerrados Este Mes")]
        public int PncCerradosEsteMes { get; set; }

        [Display(Name = "Acciones Vencidas")]
        public int AccionesVencidas { get; set; }

        [Display(Name = "Acciones Próximas a Vencer")]
        public int AccionesProximasVencer { get; set; }

        [Display(Name = "Promedio Días Cierre")]
        public double PromediosDiasCierre { get; set; }

        // Listado de PNC para seguimiento
        public List<PncSeguimientoItemVM> Items { get; set; } = new();
    }

    /// <summary>
    /// Item individual en el seguimiento
    /// </summary>
    public class PncSeguimientoItemVM
    {
        public int IdPNC { get; set; }
        public string JobBook { get; set; } = string.Empty;
        public string? NombreEstudio { get; set; }
        public DateTime FechaReclamo { get; set; }
        public int DiasAbierto { get; set; }
        public int TotalAcciones { get; set; }
        public int AccionesEjecutadas { get; set; }
        public int AccionesVencidas { get; set; }
        public string Estado { get; set; } = string.Empty;
        
        public double PorcentajeAvance
        {
            get
            {
                if (TotalAcciones == 0) return 0;
                return Math.Round((double)AccionesEjecutadas / TotalAcciones * 100, 2);
            }
        }

        public string ClaseEstado
        {
            get
            {
                if (AccionesVencidas > 0) return "danger";
                if (PorcentajeAvance == 100) return "success";
                if (PorcentajeAvance >= 50) return "warning";
                return "info";
            }
        }
    }
}
