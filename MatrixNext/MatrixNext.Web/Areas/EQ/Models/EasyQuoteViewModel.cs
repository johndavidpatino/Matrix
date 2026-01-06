using System.Collections.Generic;

namespace MatrixNext.Web.Areas.EQ.Models
{
    public class EasyQuoteViewModel
    {
        public long? Id { get; set; }
        public EQHeader Header { get; set; } = new();
        public EQQuestionnaire Questionnaire { get; set; } = new();
        public EQMethodology Methodology { get; set; } = new();
        public EQLogistica Logistica { get; set; } = new();
        public List<EQSampleCity> SampleCities { get; set; } = new();
        public List<EQMysteryVisit> MysteryVisits { get; set; } = new();
        public List<EQStaffSL> StaffSL { get; set; } = new();
        public EQSummary Summary { get; set; } = new();
    }

    public class EQHeader
    {
        public string Nombre { get; set; } = string.Empty;
        public string GrupoObjetivo { get; set; } = string.Empty;
        public string Cliente { get; set; } = string.Empty;
        public string ProbAprobacion { get; set; } = "Alta";
        public string SL { get; set; } = string.Empty;
        public string MetodologiaSL { get; set; } = string.Empty;
        public string RecordDetail { get; set; } = string.Empty;
        public string CategoriaProducto { get; set; } = string.Empty;
        public decimal ValorProveedorExterno { get; set; }
        public decimal ValorProveedorInternacional { get; set; }
        public decimal ValorGMU { get; set; }
        public System.DateTime? FechaAprobacionEstimada { get; set; }
        public System.DateTime? FechaCampo { get; set; }
    }

    public class EQQuestionnaire
    {
        public int DuracionMin { get; set; }
        public string PenetracionCodigo { get; set; } = "MAS82";
        public int PregAbiertas { get; set; }
        public int PregAbiertasMult { get; set; }
        public bool TopLine { get; set; }
        public string DataCleaning { get; set; } = "Total";
        public bool ASCIIFlag { get; set; }
        public bool ScriptReclutamiento { get; set; }
        public bool Scripting { get; set; }
        public string ScriptingTipo { get; set; } = "Nuevo";
        public bool Codificacion { get; set; }
        public bool Procesamiento { get; set; }
        public int NumProcesamientos { get; set; } = 1;
        public bool ProcesoEstadistico { get; set; }
        public string ClasePrueba { get; set; } = string.Empty;
        public bool Refrigeracion { get; set; }
        public decimal CompraProducto { get; set; }
        public string EtiquetadoTipo { get; set; } = string.Empty;
        public bool Embalaje { get; set; }
        public int ProductosTestear { get; set; } = 1;
        public int ProductosPorResp { get; set; } = 1;
        public int PatinadoresCiudad { get; set; } = 0;
        public bool Siembra { get; set; }
        public bool Harmoni { get; set; }
        public bool Graficacion { get; set; }
        public decimal OtrosCostos { get; set; }
    }

    public class EQMethodology
    {
        public string MetodologiaRecoleccion { get; set; } = string.Empty;
        public string Tecnica1 { get; set; } = string.Empty;
        public string Tecnica2 { get; set; } = string.Empty;
        public string Tecnica3 { get; set; } = string.Empty;
        public string BaseDatos { get; set; } = string.Empty;
        public string IncidenciaLabel { get; set; } = string.Empty;
        public decimal? IncidenciaValor { get; set; }
        public decimal SobreMuestraPct { get; set; }
        public bool EnvioCiudades { get; set; }
        public decimal PesoProductoGr { get; set; }
    }

    public class EQSampleCity
    {
        public string Ciudad { get; set; } = string.Empty;
        public bool Activa { get; set; }
        public decimal MuestraTotal { get; set; }
        public decimal NSE1 { get; set; }
        public decimal NSE2 { get; set; }
        public decimal NSE3 { get; set; }
        public decimal NSE4 { get; set; }
        public decimal NSE5 { get; set; }
        public decimal NSE6 { get; set; }
    }

    public class EQMysteryVisit
    {
        public string TipoVisita { get; set; } = string.Empty;
        public string Complejidad { get; set; } = string.Empty;
        public int NumOlas { get; set; } = 1;
        public decimal? Desplazamientos { get; set; }
        public decimal? Tanqueos { get; set; }
        public decimal? Alertas { get; set; }
        public decimal? Edicion { get; set; }
        public decimal? AlquilerEquipos { get; set; }
        public decimal? CompraDispositivos { get; set; }
    }

    public class EQStaffSL
    {
        public string Nivel { get; set; } = string.Empty;
        public decimal HorasMinimas { get; set; }
        public decimal HorasPresup { get; set; }
        public decimal Tarifa { get; set; }
        public decimal Valor => HorasPresup * Tarifa;
    }

    public class EQSummary
    {
        public decimal CostoCampo { get; set; }
        public decimal CostoCalidad { get; set; }
        public decimal Viaticos { get; set; }
        public decimal Incentivos { get; set; }
        public decimal Insumos { get; set; }
        public decimal StaffOps { get; set; }
        public decimal StaffSL { get; set; }
        public decimal CompraProducto { get; set; }
        public decimal Tablets { get; set; }
        public decimal DirectCostOps { get; set; }
        public decimal GM { get; set; }
        public decimal PB_RMF { get; set; }
        public decimal ProfTime { get; set; }
        public decimal OP { get; set; }
        public decimal AOT { get; set; }
        public decimal PorcOP { get; set; }
    }

    public class EQLogistica
    {
        public int DiasSetup { get; set; } = 2;
        public int DiasCampo { get; set; }
        public int NumOlas { get; set; } = 1;
        public string ApoyoReclutamientoTipo { get; set; } = string.Empty;
        public bool TaxiParticipantes { get; set; }
        public bool EstudioNinos { get; set; }
        public int ReprografiaPaginas { get; set; }
        public decimal? ViaticasCampoOverride { get; set; }
        public decimal OtrosIncentivos { get; set; }
        public decimal? DimensionLargoCm { get; set; }
        public decimal? DimensionAnchoCm { get; set; }
        public decimal? DimensionAltoCm { get; set; }
    }
}
