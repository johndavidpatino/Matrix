using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatrixNext.Data.Entities
{
    public class IQ_Parametros
    {
        public long IdPropuesta { get; set; }
        public int ParAlternativa { get; set; }
        public int MetCodigo { get; set; }
        public int ParNacional { get; set; }
        public int? TipoProyecto { get; set; }
        public string? ParNomPresupuesto { get; set; }
        public string? Pr_ProductCode { get; set; }
        public int? TecCodigo { get; set; }
        public int? ParTotalPreguntas { get; set; }
        public int? ParPaginasEncuesta { get; set; }
        public int? ParHorasEntrevista { get; set; }
        public int? ParNumAsistentesSesion { get; set; }
        public int? ParEncuestadoresPunto { get; set; }
        public double? ParProductividad { get; set; }
        public double? ParProductividadOriginal { get; set; }
        public int? ParContactosNoEfectivos { get; set; }
        public int? ParContactosNoEfectivosOriginales { get; set; }
        public int? ParTiempoEncuesta { get; set; }
        public decimal? Usuario { get; set; }
        public DateTime? ParFechaCreacion { get; set; }
        public decimal? ParValorDolar { get; set; }
        public bool? ParAprobado { get; set; }
        public DateTime? ParFechaAprobacion { get; set; }
        public bool? ParPresupuestoEnUso { get; set; }
        public decimal? ParUsuarioTieneUso { get; set; }
        public int? ParFactorAjustado { get; set; }
        public string? ParNumJobBook { get; set; }
        public int? ParNProcesosDC { get; set; }
        public int? ParNProcesosTopLines { get; set; }
        public int? ParNProcesosTablas { get; set; }
        public int? ParNProcesosBases { get; set; }
        public string? ParGrupoObjetivo { get; set; }
        public int? ParIncidencia { get; set; }
        public int? ParDiasEncuestador { get; set; }
        public int? ParDiasSupervisor { get; set; }
        public int? ParDiasCoordinador { get; set; }
        public int? ParUnidad { get; set; }
        public double? ParGrossMargin { get; set; }
        public decimal? ParValorVenta { get; set; }
        public decimal? ParCostoDirecto { get; set; }
        public decimal? ParActSubGasto { get; set; }
        public string? Pr_Offeringcode { get; set; }
        public decimal? ParActSubCosto { get; set; }
        public bool? ParUsaLista { get; set; }
        public bool? ParRevisado { get; set; }
        public decimal? ParRevisadoPor { get; set; }
        public DateTime? ParFechaRevision { get; set; }
        public bool? ParProbabilistico { get; set; }
        public string? ParDicultadTargetCualitativo { get; set; }
        public bool? ParViaticosReclutamiento { get; set; }
        public bool? ParViaticosModeracion { get; set; }
        public bool? ParViaticosInforme { get; set; }
        public bool? ParEditaVideo { get; set; }
        public bool? ParTransmiteInternet { get; set; }
        public bool? ParQAP { get; set; }
        public int? ParPorcentajeIntercep { get; set; }
        public int? ParPorcentajeRecluta { get; set; }
        public int? ParUnidadesProducto { get; set; }
        public decimal? ParValorUnitarioProd { get; set; }
        public int? ParTipoCLT { get; set; }
        public decimal? ParAlquilerEquipos { get; set; }
        public bool? ParApoyoLogistico { get; set; }
        public bool? ParAccesoInternet { get; set; }
        public string? ParObservaciones { get; set; }
        public bool? ParSubcontratar { get; set; }
        public int? ParPorcentajeSub { get; set; }
        public bool? ParUsaTablet { get; set; }
        public bool? ParUsaPapel { get; set; }
        public bool? ParDispPropio { get; set; }

        [Column("ParAñoSiguiente")]
        public bool? ParAnoSiguiente { get; set; }

        public byte? TipoPresupuesto { get; set; }
        public byte? Complejidad { get; set; }
        public bool? F2FVirtual { get; set; }
        public byte? ComplejidadCodificacion { get; set; }
        public bool? DPTransformacion { get; set; }
        public bool? DPUnificacion { get; set; }
        public byte? DPComplejidad { get; set; }
        public byte? DPPonderacion { get; set; }
        public bool? DPInInterna { get; set; }
        public bool? DPInCliente { get; set; }
        public bool? DPInPanel { get; set; }
        public bool? DPInExterno { get; set; }
        public bool? DPInGMU { get; set; }
        public bool? DPInOtro { get; set; }
        public bool? DPOutCliente { get; set; }
        public bool? DPOutWebDelivery { get; set; }
        public bool? DPOutExterno { get; set; }
        public bool? DPOutGMU { get; set; }
        public bool? DPOutOtro { get; set; }
        public byte? PTApoyosPunto { get; set; }
        public bool? PTCompra { get; set; }
        public bool? PTNeutralizador { get; set; }
        public byte? PTTipoProducto { get; set; }
        public byte? PTLotes { get; set; }
        public byte? PTVisitas { get; set; }
        public byte? PTCeldas { get; set; }
        public byte? PTProductosEvaluar { get; set; }
        public byte? DPComplejidadCuestionario { get; set; }
        public string? NoIQuote { get; set; }
        public double? OP { get; set; }

        public virtual ICollection<IQ_CostoActividades> IQ_CostoActividades { get; set; }
            = new HashSet<IQ_CostoActividades>();

        public virtual ICollection<IQ_Muestra_1> IQ_Muestra { get; set; }
            = new HashSet<IQ_Muestra_1>();

        public virtual ICollection<IQ_OpcionesAplicadas> IQ_OpcionesAplicadas { get; set; }
            = new HashSet<IQ_OpcionesAplicadas>();

        public virtual IQ_Preguntas? IQ_Preguntas { get; set; }

        public virtual ICollection<IQ_ProcesosPresupuesto> IQ_ProcesosPresupuesto { get; set; }
            = new HashSet<IQ_ProcesosPresupuesto>();
    }
}
