using System;
using System.Collections.Generic;
using System.Linq;
using MatrixNext.Web.Areas.EQ.Models;
using MatrixNext.Web.Models.EQ;

namespace MatrixNext.Web.Services.EQ.Adapters
{
    /// <summary>
    /// Adaptador para convertir EqQuoteHeader (entidad EF) a EasyQuoteViewModel (DTO para cálculos)
    /// Necesario para que QuoteCalculator pueda procesar datos desde BD
    /// 
    /// Mapeo:
    /// - EqQuoteHeader → EQHeader
    /// - EqQuestionnaire → EQQuestionnaire
    /// - EqMethodology → EQMethodology
    /// - EqSampleCity → EQSampleCity
    /// - EqMystery → EQMysteryVisit
    /// - EqStaffSL → EQStaffSL
    /// - EqLogistica → EQLogistica (si existe)
    /// </summary>
    public class QuoteHeaderToViewModelAdapter
    {
        /// <summary>
        /// Convierte una entidad EqQuoteHeader completa a EasyQuoteViewModel para cálculos
        /// Asume que el header fue cargado con todos los Includes requeridos
        /// </summary>
        public EasyQuoteViewModel ToViewModel(EqQuoteHeader? header)
        {
            if (header == null)
                return new EasyQuoteViewModel();

            var vm = new EasyQuoteViewModel
            {
                Id = header.Id,
                Header = MapHeader(header),
                Questionnaire = MapQuestionnaire(header.Questionnaires?.FirstOrDefault()),
                Methodology = MapMethodology(header.Methodologies?.FirstOrDefault()),
                Logistica = MapLogistica(header),
                SampleCities = MapSampleCities(header.SampleCities),
                MysteryVisits = MapMysteryVisits(header.Mysteries),
                StaffSL = MapStaffSL(header.StaffSL)
            };

            return vm;
        }

        /// <summary>
        /// Mapea EqQuoteHeader → EQHeader (encabezado propuesta)
        /// </summary>
        private static EQHeader MapHeader(EqQuoteHeader? entity)
        {
            if (entity == null)
                return new EQHeader();

            return new EQHeader
            {
                Nombre = entity.PropuestaNombre ?? string.Empty,
                GrupoObjetivo = entity.GrupoObjetivo ?? string.Empty,
                Cliente = entity.Cliente ?? string.Empty,
                ProbAprobacion = entity.ProbabilidadAprobacion ?? "Alta",
                SL = entity.SL ?? string.Empty,
                MetodologiaSL = entity.MetodologiaSL ?? string.Empty,
                RecordDetail = entity.RecordDetail ?? string.Empty,
                CategoriaProducto = entity.CategoriaProducto ?? string.Empty,
                ValorProveedorExterno = entity.ValorProveedorExterno ?? 0m,
                ValorProveedorInternacional = entity.ValorProveedorInternacional ?? 0m,
                ValorGMU = entity.ValorGMU ?? 0m,
                FechaAprobacionEstimada = entity.FechaAprobacionEstimada,
                FechaCampo = entity.FechaCampo
            };
        }

        /// <summary>
        /// Mapea EqQuestionnaire → EQQuestionnaire (detalles cuestionario)
        /// </summary>
        private static EQQuestionnaire MapQuestionnaire(EqQuestionnaire? entity)
        {
            if (entity == null)
                return new EQQuestionnaire();

            return new EQQuestionnaire
            {
                DuracionMin = entity.DuracionMinutos > 0 ? entity.DuracionMinutos : 5,
                PenetracionCodigo = entity.PenetracionLabel ?? "MAS82",
                PregAbiertas = entity.PreguntasAbiertas,
                PregAbiertasMult = entity.PreguntasAbiertasMultiples,
                TopLine = entity.TopLine,
                DataCleaning = entity.DataCleaning ?? "Total",
                ASCIIFlag = entity.ASCII,
                ScriptReclutamiento = entity.ScriptReclutamiento,
                Scripting = entity.Scripting,
                ScriptingTipo = entity.TipoScript ?? "Nuevo",
                Codificacion = entity.Codificacion,
                Procesamiento = entity.Procesamiento,
                NumProcesamientos = entity.NumProcesamientos > 0 ? entity.NumProcesamientos : 1,
                ProcesoEstadistico = entity.ProcesoEstadistico,
                ClasePrueba = entity.ClasePrueba ?? string.Empty,
                Refrigeracion = entity.Refrigeracion,
                CompraProducto = entity.CompraProducto ?? 0m,
                EtiquetadoTipo = entity.EtiquetadoTipo ?? string.Empty,
                Embalaje = entity.Embalaje,
                ProductosTestear = entity.ProductosATestear > 0 ? entity.ProductosATestear : 1,
                ProductosPorResp = entity.ProductosPorRespondiente > 0 ? entity.ProductosPorRespondiente : 1,
                PatinadoresCiudad = entity.PatinadoresPorCiudad,
                Siembra = entity.Siembra,
                Harmoni = false, // NOTA: No existe en entidad actual, requiere migración si se necesita
                Graficacion = false, // NOTA: No existe en entidad actual, requiere migración si se necesita
                OtrosCostos = 0m // NOTA: No existe en entidad actual, requiere migración si se necesita
            };
        }

        /// <summary>
        /// Mapea EqMethodology → EQMethodology (metodologias recolección)
        /// </summary>
        private static EQMethodology MapMethodology(EqMethodology? entity)
        {
            if (entity == null)
                return new EQMethodology();

            return new EQMethodology
            {
                MetodologiaRecoleccion = entity.MetodologiaRecoleccion ?? string.Empty,
                Tecnica1 = entity.Tecnica1Tipo ?? string.Empty,
                Tecnica2 = entity.Tecnica2Tipo ?? string.Empty,
                Tecnica3 = entity.Tecnica3Tipo ?? string.Empty,
                BaseDatos = entity.BaseDatos ?? string.Empty,
                IncidenciaLabel = entity.IncidenciaLabel ?? string.Empty,
                IncidenciaValor = entity.IncidenciaValor,
                SobreMuestraPct = 0m, // No existe en entidad, requiere migración
                EnvioCiudades = false, // No existe en entidad, requiere migración
                PesoProductoGr = 0m // No existe en entidad, requiere migración
            };
        }

        /// <summary>
        /// Mapea colección EqSampleCity → List<EQSampleCity> (ciudades de muestra)
        /// </summary>
        private static List<EQSampleCity> MapSampleCities(ICollection<EqSampleCity>? entities)
        {
            if (entities == null || entities.Count == 0)
                return new List<EQSampleCity>();

            var sampleCities = entities!;
            return sampleCities.Select(e => new EQSampleCity
            {
                Ciudad = e.Ciudad ?? string.Empty,
                Activa = e.Activa,
                MuestraTotal = e.MuestraTotal,
                NSE1 = e.NSE1,
                NSE2 = e.NSE2,
                NSE3 = e.NSE3,
                NSE4 = e.NSE4,
                NSE5 = e.NSE5,
                NSE6 = e.NSE6
            }).ToList();
        }

        /// <summary>
        /// Mapea colección EqMystery → List<EQMysteryVisit> (visitas mystery shopper)
        /// </summary>
        private static List<EQMysteryVisit> MapMysteryVisits(ICollection<EqMystery>? entities)
        {
            if (entities == null || entities.Count == 0)
                return new List<EQMysteryVisit>();

            var mysteries = entities!;
            return mysteries.Select(e => new EQMysteryVisit
            {
                TipoVisita = e.TipoVisita.ToString() ?? string.Empty, // TipoVisita es int (1,2,3)
                Complejidad = e.Complejidad ?? string.Empty,
                NumOlas = e.NumOlas > 0 ? e.NumOlas : 1,
                Desplazamientos = e.Desplazamientos,
                Tanqueos = e.Tanques, // Nombre correcto: Tanques no Tanqueos
                Alertas = e.Alertas,
                Edicion = e.EdicionVideo, // Nombre correcto: EdicionVideo no Edicion
                AlquilerEquipos = e.AlquilerEquipos,
                CompraDispositivos = e.CompraDispositivos
            }).ToList();
        }

        /// <summary>
        /// Mapea colección EqStaffSL → List<EQStaffSL> (staff SL asignado)
        /// </summary>
        private static List<EQStaffSL> MapStaffSL(ICollection<EqStaffSL>? entities)
        {
            if (entities == null || entities.Count == 0)
                return new List<EQStaffSL>();

            var staffList = entities!;
            return staffList.Select(e => new EQStaffSL
            {
                Nivel = e.Nivel ?? string.Empty,
                HorasMinimas = e.HorasMinimas,
                HorasPresup = e.HorasPresupuestadas, // Nombre correcto: HorasPresupuestadas
                Tarifa = e.TarifaNivel // Nombre correcto: TarifaNivel
            }).ToList();
        }

        /// <summary>
        /// Mapea EqQuoteHeader → EQLogistica (datos de logística)
        /// NOTA: Estos datos actualmente están dispersos en la entidad principal y requerimientos
        /// Se mapean valores por defecto o desde propiedades disponibles
        /// </summary>
        private static EQLogistica MapLogistica(EqQuoteHeader? header)
        {
            if (header == null)
                return new EQLogistica();

            // TODO: Una vez que EqLogistica se integre como tabla separada o propiedades,
            // actualizar este mapeo con valores reales de BD
            return new EQLogistica
            {
                DiasSetup = 2,
                DiasCampo = 0,
                NumOlas = 1,
                ApoyoReclutamientoTipo = string.Empty,
                TaxiParticipantes = false,
                EstudioNinos = false,
                ReprografiaPaginas = 0,
                ViaticasCampoOverride = null,
                OtrosIncentivos = 0m,
                DimensionLargoCm = null,
                DimensionAnchoCm = null,
                DimensionAltoCm = null
            };
        }

        /// <summary>
        /// Mapea EasyQuoteViewModel → EqQuoteHeader (operación inversa)
        /// Usado para guardar cambios de la quote
        /// </summary>
        public EqQuoteHeader ToEntity(EasyQuoteViewModel vm)
        {
            if (vm == null)
                return new EqQuoteHeader();

            var entity = new EqQuoteHeader
            {
                Id = (int)vm.Id,
                PropuestaNombre = vm.Header?.Nombre ?? string.Empty,
                GrupoObjetivo = vm.Header?.GrupoObjetivo ?? string.Empty,
                Cliente = vm.Header?.Cliente ?? string.Empty,
                ProbabilidadAprobacion = vm.Header?.ProbAprobacion ?? string.Empty,
                SL = vm.Header?.SL ?? string.Empty,
                MetodologiaSL = vm.Header?.MetodologiaSL ?? string.Empty,
                RecordDetail = vm.Header?.RecordDetail ?? string.Empty,
                CategoriaProducto = vm.Header?.CategoriaProducto ?? string.Empty,
                ValorProveedorExterno = vm.Header?.ValorProveedorExterno ?? 0m,
                ValorProveedorInternacional = vm.Header?.ValorProveedorInternacional ?? 0m,
                ValorGMU = vm.Header?.ValorGMU ?? 0m,
                FechaAprobacionEstimada = vm.Header?.FechaAprobacionEstimada,
                FechaCampo = vm.Header?.FechaCampo
            };

            return entity;
        }
    }
}
