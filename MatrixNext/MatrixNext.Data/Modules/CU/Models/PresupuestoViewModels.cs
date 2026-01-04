using System;
using System.Collections.Generic;

namespace MatrixNext.Data.Modules.CU.Models
{
    public class AlternativaViewModel
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public string? Descripcion { get; set; }
        public string? Observaciones { get; set; }
        public int DiasCampo { get; set; }
        public int? DiasDiseno { get; set; }
        public int? DiasInformes { get; set; }
        public int? DiasProcesamiento { get; set; }
        public int? NumeroMediciones { get; set; }
        public int? MesesMediciones { get; set; }
        public byte? TipoPresupuesto { get; set; }
        public string? NoIQuote { get; set; }
        public int? TotalPresupuestos { get; set; }
        public decimal? ValorTotal { get; set; }
    }

    public class PresupuestoIndexViewModel
    {
        public long IdPropuesta { get; set; }
        public JobBookContextViewModel? JobBookContext { get; set; }
        public List<AlternativaViewModel> Alternativas { get; set; } = new List<AlternativaViewModel>();
        public List<PresupuestoListItemViewModel> Presupuestos { get; set; } = new List<PresupuestoListItemViewModel>();
        public int? AlternativaSeleccionada { get; set; }
        public int? TecnicaSeleccionada { get; set; }
    }

    public class EditarAlternativaViewModel
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public string? Descripcion { get; set; }
        public string? Observaciones { get; set; }
        public int DiasCampo { get; set; }
        public int? DiasDiseno { get; set; }
        public int? DiasProcesamiento { get; set; }
        public int? DiasInformes { get; set; }
        public int? NumeroMediciones { get; set; }
        public int? MesesMediciones { get; set; }
        public byte? TipoPresupuesto { get; set; }
        public string? NoIQuote { get; set; }
    }

    /// <summary>
    /// ViewModel para el listado de presupuestos en el grid principal
    /// </summary>
    public class PresupuestoListItemViewModel
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int MetCodigo { get; set; }
        public string? MetodologiaNombre { get; set; }
        public int ParNacional { get; set; }
        public string? FaseNombre { get; set; }
        public int? TecCodigo { get; set; }
        public string? TecnicaNombre { get; set; }
        public int? TotalMuestra { get; set; }
        public decimal? ValorVenta { get; set; }
        public double? GrossMargin { get; set; }
        public bool? Revisado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? NoIQuote { get; set; }
        public string? JobBook { get; set; }
    }

    /// <summary>
    /// ViewModel completo para edición de presupuesto (formulario IQuote)
    /// </summary>
    public class EditarPresupuestoViewModel
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int MetCodigo { get; set; }
        public int ParNacional { get; set; }

        // Sección General
        public int? TecCodigo { get; set; }
        public int? TipoProyecto { get; set; }
        public string? ParGrupoObjetivo { get; set; }
        public int? ParIncidencia { get; set; }
        public double? ParProductividad { get; set; }
        public bool? ParProbabilistico { get; set; }
        public bool? F2FVirtual { get; set; }

        // Sección Cuestionario/Preguntas
        public int? PregCerradas { get; set; }
        public int? PregCerradasMultiples { get; set; }
        public int? PregAbiertas { get; set; }
        public int? PregAbiertasMultiples { get; set; }
        public int? PregOtras { get; set; }
        public int? PregDemograficos { get; set; }
        public int? ParTiempoEncuesta { get; set; }
        public byte? Complejidad { get; set; }
        public byte? DPComplejidadCuestionario { get; set; }

        // Sección Procesos
        public int? ParNProcesosDC { get; set; }
        public int? ParNProcesosTopLines { get; set; }
        public int? ParNProcesosTablas { get; set; }
        public int? ParNProcesosBases { get; set; }
        public byte? ComplejidadCodificacion { get; set; }

        // Sección Data Processing
        public bool? DPTransformacion { get; set; }
        public bool? DPUnificacion { get; set; }
        public byte? DPComplejidad { get; set; }
        public byte? DPPonderacion { get; set; }

        // Inputs Data Processing
        public bool? DPInInterna { get; set; }
        public bool? DPInCliente { get; set; }
        public bool? DPInPanel { get; set; }
        public bool? DPInExterno { get; set; }
        public bool? DPInGMU { get; set; }
        public bool? DPInOtro { get; set; }

        // Outputs Data Processing
        public bool? DPOutCliente { get; set; }
        public bool? DPOutWebDelivery { get; set; }
        public bool? DPOutExterno { get; set; }
        public bool? DPOutGMU { get; set; }
        public bool? DPOutOtro { get; set; }

        // Sección Product Testing
        public int? ParUnidadesProducto { get; set; }
        public decimal? ParValorUnitarioProd { get; set; }
        public byte? PTApoyosPunto { get; set; }
        public bool? PTCompra { get; set; }
        public bool? PTNeutralizador { get; set; }
        public byte? PTTipoProducto { get; set; }
        public byte? PTLotes { get; set; }
        public byte? PTVisitas { get; set; }
        public byte? PTCeldas { get; set; }
        public byte? PTProductosEvaluar { get; set; }

        // Sección CLT
        public int? ParTipoCLT { get; set; }
        public decimal? ParAlquilerEquipos { get; set; }
        public bool? ParApoyoLogistico { get; set; }
        public bool? ParAccesoInternet { get; set; }

        // Interceptación/Reclutamiento
        public int? ParPorcentajeIntercep { get; set; }
        public int? ParPorcentajeRecluta { get; set; }
        public int? ParEncuestadoresPunto { get; set; }

        // Otros
        public string? ParObservaciones { get; set; }
        public bool? ParUsaTablet { get; set; }
        public bool? ParUsaPapel { get; set; }
        public bool? ParDispPropio { get; set; }

        // Muestra
        public List<MuestraItemViewModel> Muestra { get; set; } = new List<MuestraItemViewModel>();

        // Procesos (relación N:N)
        public List<int> ProcesosCodigos { get; set; } = new List<int>();

        // Actividades Subcontratadas
        public List<ActividadSubcontratadaViewModel> ActividadesSubcontratadas { get; set; } = new List<ActividadSubcontratadaViewModel>();

        // Análisis Estadístico
        public List<AnalisisEstadisticoViewModel> AnalisisEstadisticos { get; set; } = new List<AnalisisEstadisticoViewModel>();

        // Horas Profesionales
        public List<HoraProfesionalViewModel> HorasProfesionales { get; set; } = new List<HoraProfesionalViewModel>();
    }

    public class MuestraItemViewModel
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int MetCodigo { get; set; }
        public int CiuCodigo { get; set; }
        public string? CiudadNombre { get; set; }
        public string? DepartamentoNombre { get; set; }
        public int MuIdentificador { get; set; }
        public string? IdentificadorNombre { get; set; } // NSE, Dificultad, etc.
        public int ParNacional { get; set; }
        public int? DeptCodigo { get; set; }
        public int MuCantidad { get; set; }
    }

    public class ActividadSubcontratadaViewModel
    {
        public int ActCodigo { get; set; }
        public string? ActividadNombre { get; set; }
        public decimal CaCosto { get; set; }
        public int? CaUnidades { get; set; }
        public string? CaDescripcionUnidades { get; set; }
        public int? Horas { get; set; }
    }

    public class AnalisisEstadisticoViewModel
    {
        public int Codigo { get; set; }
        public string? Nombre { get; set; }
        public int? Cantidad { get; set; }
    }

    public class HoraProfesionalViewModel
    {
        public int RolCodigo { get; set; }
        public string? RolNombre { get; set; }
        public int? Horas { get; set; }
        public string? Fase { get; set; }
    }

    /// <summary>
    /// ViewModel para el simulador de costos (cálculo completo)
    /// </summary>
    public class SimuladorCostosViewModel
    {
        public decimal CostoDirecto { get; set; }
        public double GrossMargin { get; set; }
        public decimal ValorVenta { get; set; }
        public decimal ActividadesSubcontratadas { get; set; }
        public int DiasCalculados { get; set; }
        public double ProductividadCalculada { get; set; }
        public int? TotalMuestra { get; set; }
        public int? DiasEstimados { get; set; }
        public List<DesgloseCostoViewModel> DesgloseCostos { get; set; } = new List<DesgloseCostoViewModel>();
    }

    public class DesgloseCostoViewModel
    {
        public string? Concepto { get; set; }
        public decimal Valor { get; set; }
        public double? Porcentaje { get; set; }
        public string? Categoria { get; set; } // Campo, Procesamiento, Honorarios, etc.
    }
}
