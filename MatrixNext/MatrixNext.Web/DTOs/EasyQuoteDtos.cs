using System;
using System.Collections.Generic;

namespace MatrixNext.Web.DTOs
{
    // ==================== CREATE/UPDATE DTOs ====================

    public class EasyQuoteCreateDto
    {
        public string PropuestaNombre { get; set; }
        public string GrupoObjetivo { get; set; }
        public string Cliente { get; set; }
        public DateTime? FechaAprobacionEstimada { get; set; }
        public DateTime? FechaCampo { get; set; }
        public string ProbabilidadAprobacion { get; set; } = "Alta";
        public string SL { get; set; }
        public string MetodologiaSL { get; set; }
        public string RecordDetail { get; set; }
        public string CategoriaProducto { get; set; }
        public decimal? ValorProveedorExterno { get; set; }
        public decimal? ValorProveedorInternacional { get; set; }
        public decimal? ValorGMU { get; set; }
        public string Notas { get; set; }

        // Detalles asociados
        public EasyQuestionnaireDto Questionnaire { get; set; }
        public EasyMethodologyDto Methodology { get; set; }
        public List<EasySampleCityDto> SampleCities { get; set; } = new List<EasySampleCityDto>();
    }

    public class EasyQuoteUpdateDto : EasyQuoteCreateDto
    {
    }

    public class EasyQuestionnaireDto
    {
        public int DuracionMinutos { get; set; }
        public string PenetracionLabel { get; set; }
        public decimal? PenetracionValor { get; set; }
        public int PreguntasAbiertas { get; set; }
        public int PreguntasAbiertasMultiples { get; set; }
        public bool TopLine { get; set; }
        public string DataCleaning { get; set; } // Total, Parcial, No
        public bool ASCII { get; set; }
        public bool ScriptReclutamiento { get; set; }
        public bool Scripting { get; set; }
        public string TipoScript { get; set; } // Nuevo, Duplicado, Reutilizacion
        public bool Codificacion { get; set; }
        public bool Procesamiento { get; set; }
        public int NumProcesamientos { get; set; } = 1;
        public bool ProcesoEstadistico { get; set; }
        public string ClasePrueba { get; set; }
        public bool Refrigeracion { get; set; }
        public decimal? CompraProducto { get; set; }
        public string EtiquetadoTipo { get; set; }
        public bool Embalaje { get; set; }
        public int ProductosATestear { get; set; } = 1;
        public int ProductosPorRespondiente { get; set; } = 1;
    }

    public class EasyMethodologyDto
    {
        public string MetodologiaRecoleccion { get; set; }
        public string Tecnica1Tipo { get; set; }
        public bool Tecnica1Flag { get; set; }
        public string Tecnica2Tipo { get; set; }
        public bool Tecnica2Flag { get; set; }
        public string BaseDatos { get; set; }
        public string IncidenciaLabel { get; set; }
        public decimal? IncidenciaValor { get; set; }
    }

    public class EasySampleCityDto
    {
        public string Ciudad { get; set; }
        public bool Activa { get; set; } = true;
        public int MuestraTotal { get; set; }
        public int NSE1 { get; set; }
        public int NSE2 { get; set; }
        public int NSE3 { get; set; }
        public int NSE4 { get; set; }
        public int NSE5 { get; set; }
        public int NSE6 { get; set; }
        public decimal SobreMuestraPct { get; set; }
        public decimal? PesoProductoGramos { get; set; }
        public bool EnvioCiudades { get; set; }
    }

    // ==================== GET/LIST DTOs ====================

    public class EasyQuoteHeaderDto
    {
        public int Id { get; set; }
        public string PropuestaNombre { get; set; }
        public string Cliente { get; set; }
        public string SL { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }
    }

    public class EasyQuoteDetailDto
    {
        public int Id { get; set; }
        public string PropuestaNombre { get; set; }
        public string GrupoObjetivo { get; set; }
        public string Cliente { get; set; }
        public string SL { get; set; }
        public string MetodologiaSL { get; set; }
        public string RecordDetail { get; set; }
        public DateTime? FechaAprobacionEstimada { get; set; }
        public DateTime? FechaCampo { get; set; }
        public string ProbabilidadAprobacion { get; set; }
        public string Notas { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaModificacion { get; set; }

        // Detalles
        public EasyQuestionnaireDto Questionnaire { get; set; }
        public EasyMethodologyDto Methodology { get; set; }
        public List<EasySampleCityDto> SampleCities { get; set; }
    }

    public class EasyQuoteListDto
    {
        public int Id { get; set; }
        public string PropuestaNombre { get; set; }
        public string Cliente { get; set; }
        public string SL { get; set; }
        public string Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }

    // ==================== COST/CALCULATION DTOs ====================

    public class EasyCostResultDto
    {
        public int Id { get; set; }
        public int QuoteHeaderId { get; set; }
        public string Moneda { get; set; } = "COP";

        // Rubros
        public decimal CostoCampo { get; set; }
        public decimal CostoCalidad { get; set; }
        public decimal Viaticos { get; set; }
        public decimal Incentivos { get; set; }
        public decimal Insumos { get; set; }
        public decimal StaffOps { get; set; }
        public decimal Estadistica { get; set; }
        public decimal Scripting { get; set; }
        public decimal DataCleaning { get; set; }
        public decimal Procesamiento { get; set; }

        // Totales
        public decimal CostoDirectoTotal { get; set; }
        public decimal DirectCostOps { get; set; }

        // Márgenes
        public decimal GM { get; set; }
        public decimal PB_RMF { get; set; }
        public decimal OP { get; set; }
        public decimal PctOP { get; set; }

        public decimal AOTTotal { get; set; }
        public DateTime FechaCalculo { get; set; }
    }

    // ==================== MASTER DATA DTOs ====================

    public class EasyMasterPrecioDto
    {
        public int Id { get; set; }
        public string TipoMetodologia { get; set; }
        public string PenetracionRango { get; set; }
        public int DuracionMin { get; set; }
        public decimal ValorTotal { get; set; }
    }

    public class EasyMasterScriptProcDto
    {
        public int DuracionMin { get; set; }
        public decimal HorasScript { get; set; }
        public decimal HorasProc { get; set; }
        public decimal HorasHarmoni { get; set; }
        public decimal HorasGraficacion { get; set; }
    }

    public class EasyMasterValorHoraDto
    {
        public string Nivel { get; set; }
        public string Alternativa { get; set; }
        public decimal BaseCostRate { get; set; }
        public decimal LoadedCostRate { get; set; }
        public decimal BillingRate { get; set; }
    }

    public class EasyMasterCostInsumosDto
    {
        public int NSE { get; set; }
        public decimal Reclutamiento { get; set; }
        public decimal Obsequio { get; set; }
        public decimal Productividad { get; set; }
        public decimal TransporteEncuestador { get; set; }
    }

    public class EasyMasterRateEstadisticaDto
    {
        public int Id { get; set; }
        public string Categoria { get; set; }
        public string Servicio { get; set; }
        public decimal PrecioRef2024 { get; set; }
    }

    public class EasyMasterLocacionesDto
    {
        public int Id { get; set; }
        public string Ciudad { get; set; }
        public decimal TarifaBase { get; set; }
    }
}
