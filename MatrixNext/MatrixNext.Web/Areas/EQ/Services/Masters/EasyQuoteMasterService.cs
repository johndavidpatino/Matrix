using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace MatrixNext.Web.Areas.EQ.Services.Masters
{
    /// <summary>
    /// Servicio ligero de cache para tablas maestras de EasyQuote (precios, horas, insumos, tarifas).
    /// Carga una sola vez por request para alimentar el motor de cálculo sin reconsultar la BD.
    /// </summary>
    public class EasyQuoteMasterService
    {
        private readonly string _connString;
        private MasterCache? _cache;

        public EasyQuoteMasterService(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        }

        private MasterCache Cache
        {
            get
            {
                if (_cache != null) return _cache;
                using var conn = new SqlConnection(_connString);
                _cache = new MasterCache
                {
                    Precios = conn.Query<PrecioRow>("SELECT MetodologiaCodigo, PenetracionCodigo, DuracionMin, ValorTotal FROM eq_param_precio").ToList(),
                    HorasScriptProc = conn.Query<HorasRow>("SELECT DuracionMin, HorasScript, HorasProcesamiento, HorasHarmoni, HorasGraficacion FROM eq_param_script_proc").ToList(),
                    ValorHoraOps = conn.Query<ValorHoraRow>("SELECT Nivel, Variante, ValorHora FROM eq_valor_hora_ops").ToList(),
                    CostInsumos = conn.Query<CostInsumoRow>("SELECT NSE, Tipo, ValorUnitario FROM eq_cost_insumos").ToList(),
                    Locaciones = conn.Query<LocacionRow>("SELECT Ciudad, TarifaBase, TarifaConGross, DiasBase FROM eq_locaciones").ToList(),
                    EnvioTarifas = conn.Query<EnvioTarifaRow>("SELECT Tipologia, KiloInicial, KiloAdicional, SeguroPct, ValorDeclaradoMin FROM eq_envio_tarifa").ToList(),
                    Codificacion = conn.Query<CodificacionRow>("SELECT Escenario, Registros, PregAbiertas, PregAbiertasMult, Dias, Horas, ValorIpsos FROM eq_codificacion_param").ToList(),
                    MysteryTarifa = conn.Query<MysteryTarifaRow>("SELECT TipoVisita, Complejidad, VrUnitario, OlasDefault FROM eq_tarifa_mystery").ToList(),
                    CostUnitarioOps = conn.Query<CostUnitarioOpsRow>("SELECT CodMatrix, Actividad, Tarifa, Unidad, Horas FROM eq_cost_unitario_ops").ToList(),
                    RateEstadistica = conn.Query<RateEstadisticaRow>("SELECT Categoria, Servicio, HorasEstimadas, PrecioReferencia, FactorEscala FROM eq_rate_estadistica").ToList(),
                    ProductividadCiudad = conn.Query<ProductividadCiudadRow>("SELECT Ciudad, Encuestadores, Productividad FROM eq_productividad_ciudad").ToList(),
                    ParamMisc = conn.Query<ParamMiscRow>("SELECT Clave, ValorDecimal, ValorTexto FROM eq_param_misc").ToList(),
                    EnvioParam = conn.QueryFirstOrDefault<EnvioParamRow>("SELECT TOP 1 * FROM eq_envio_param"),
                    BaseDatos = conn.Query<BaseDatosRow>("SELECT Tipo, Valor FROM eq_cost_base_datos").ToList(),
                    Factores = conn.Query<FactorRow>("SELECT Tipo, Codigo, Descripcion, Factor, Orden, Activo FROM eq_param_factores WHERE Activo = 1").ToList(),
                    RateHoras = conn.Query<RateHoraRow>("SELECT [Key], SL, RecordDetail, MetodologiaSL, HorasL3, HorasL4, HorasL5, HorasL6, HorasL7 FROM eq_rate_horas").ToList()
                };
                return _cache;
            }
        }

        public void Reset() => _cache = null;

        // Exposiciones completas para vistas de administracion
        public List<PrecioRow> AllPrecios() => Cache.Precios;
        public List<HorasRow> AllHoras() => Cache.HorasScriptProc;
        public List<ValorHoraRow> AllValorHora() => Cache.ValorHoraOps;
        public List<CostInsumoRow> AllCostInsumos() => Cache.CostInsumos;
        public List<LocacionRow> AllLocaciones() => Cache.Locaciones;
        public List<EnvioTarifaRow> AllEnvios() => Cache.EnvioTarifas;
        public List<CodificacionRow> AllCodificacion() => Cache.Codificacion;
        public List<MysteryTarifaRow> AllMystery() => Cache.MysteryTarifa;
        public List<CostUnitarioOpsRow> AllCostUnitarios() => Cache.CostUnitarioOps;
        public List<RateEstadisticaRow> AllEstadistica() => Cache.RateEstadistica;
        public List<ProductividadCiudadRow> AllProductividad() => Cache.ProductividadCiudad;
        public List<FactorRow> AllFactores() => Cache.Factores;
        public List<RateHoraRow> AllRateHoras() => Cache.RateHoras;
        public ParamMiscRow GetMisc(string clave) => Cache.ParamMisc.FirstOrDefault(p => string.Equals(p.Clave, clave, StringComparison.OrdinalIgnoreCase));
        public EnvioParamRow GetEnvioParam() => Cache.EnvioParam;
        public decimal? GetBaseDatos(string tipo) => Cache.BaseDatos.FirstOrDefault(b => string.Equals(b.Tipo, tipo, StringComparison.OrdinalIgnoreCase))?.Valor;
        public List<ParamMiscRow> AllMisc() => Cache.ParamMisc;
        public List<BaseDatosRow> AllBaseDatos() => Cache.BaseDatos;
        
        public decimal? GetFactorCodigo(string tipo, string codigo)
        {
            return Cache.Factores.FirstOrDefault(f => 
                string.Equals(f.Tipo, tipo, StringComparison.OrdinalIgnoreCase) && 
                string.Equals(f.Codigo, codigo, StringComparison.OrdinalIgnoreCase))?.Factor;
        }
        
        public decimal? GetHorasMinimas(string sl, string recordDetail, string metodologiaSL, string nivel)
        {
            var key = $"{sl}|{recordDetail}|{metodologiaSL}";
            var row = Cache.RateHoras.FirstOrDefault(r => string.Equals(r.Key, key, StringComparison.OrdinalIgnoreCase));
            if (row == null) return null;
            
            return nivel?.ToUpperInvariant() switch
            {
                "L3" => row.HorasL3,
                "L4" => row.HorasL4,
                "L5" => row.HorasL5,
                "L6" => row.HorasL6,
                "L7" => row.HorasL7,
                _ => null
            };
        }

        public decimal? GetPrecioEncuesta(string metodologia, string penetracion, int duracionMin, DateTime? fechaLookup = null)
        {
            // Sprint 2.1: Si se proporciona fecha, usar datos vigentes a esa fecha
            // Por ahora usa el cache actual (vigentes hoy)
            // Nota: Para lookups por fecha histórica, sería necesario cargar datos históricos
            
            var precios = Cache.Precios
                .Where(p => string.Equals(p.MetodologiaCodigo, metodologia, StringComparison.OrdinalIgnoreCase)
                         && string.Equals(p.PenetracionCodigo, penetracion, StringComparison.OrdinalIgnoreCase))
                .OrderBy(p => p.DuracionMin)
                .ToList();

            if (!precios.Any()) return null;
            // buscar duracion exacta; si no existe, usar la mas cercana inferior o la minima disponible
            var exact = precios.FirstOrDefault(p => p.DuracionMin == duracionMin);
            if (exact != null) return exact.ValorTotal;
            var lower = precios.LastOrDefault(p => p.DuracionMin <= duracionMin);
            return (lower ?? precios.First()).ValorTotal;
        }

        public HorasRow GetHoras(int duracionMin)
        {
            var ordered = Cache.HorasScriptProc.OrderBy(h => h.DuracionMin).ToList();
            var exact = ordered.FirstOrDefault(h => h.DuracionMin == duracionMin);
            if (exact != null) return exact;
            return ordered.LastOrDefault(h => h.DuracionMin <= duracionMin) ?? ordered.LastOrDefault();
        }

        public decimal? GetValorHoraOps(string nivel)
        {
            return Cache.ValorHoraOps.FirstOrDefault(v => string.Equals(v.Nivel, nivel, StringComparison.OrdinalIgnoreCase))?.ValorHora;
        }

        public decimal GetCostoInsumo(string tipo, string nseCodigo)
        {
            return Cache.CostInsumos.FirstOrDefault(c =>
                string.Equals(c.Tipo, tipo, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(c.NSE, nseCodigo, StringComparison.OrdinalIgnoreCase))?.ValorUnitario ?? 0;
        }

        public LocacionRow GetLocacion(string ciudad) =>
            Cache.Locaciones.FirstOrDefault(l => string.Equals(l.Ciudad, ciudad, StringComparison.OrdinalIgnoreCase));

        public EnvioTarifaRow GetEnvio(string tipologia) =>
            Cache.EnvioTarifas.FirstOrDefault(e => string.Equals(e.Tipologia, tipologia, StringComparison.OrdinalIgnoreCase));

        public CodificacionRow GetCodificacionDefault() => Cache.Codificacion.FirstOrDefault();

        public MysteryTarifaRow GetMysteryTarifa(string tipo, string complejidad) =>
            Cache.MysteryTarifa.FirstOrDefault(m =>
                string.Equals(m.TipoVisita, tipo, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(m.Complejidad, complejidad, StringComparison.OrdinalIgnoreCase));

        public CostUnitarioOpsRow GetCostUnitario(string actividadStartsWith)
        {
            return Cache.CostUnitarioOps.FirstOrDefault(c =>
                c.Actividad.StartsWith(actividadStartsWith, StringComparison.OrdinalIgnoreCase));
        }

        public RateEstadisticaRow GetRateEstadisticaDefault() =>
            Cache.RateEstadistica.FirstOrDefault();

        private class MasterCache
        {
            public List<PrecioRow> Precios { get; set; } = new();
            public List<HorasRow> HorasScriptProc { get; set; } = new();
            public List<ValorHoraRow> ValorHoraOps { get; set; } = new();
            public List<CostInsumoRow> CostInsumos { get; set; } = new();
            public List<LocacionRow> Locaciones { get; set; } = new();
            public List<EnvioTarifaRow> EnvioTarifas { get; set; } = new();
            public List<CodificacionRow> Codificacion { get; set; } = new();
            public List<MysteryTarifaRow> MysteryTarifa { get; set; } = new();
            public List<CostUnitarioOpsRow> CostUnitarioOps { get; set; } = new();
            public List<RateEstadisticaRow> RateEstadistica { get; set; } = new();
            public List<ProductividadCiudadRow> ProductividadCiudad { get; set; } = new();
            public List<ParamMiscRow> ParamMisc { get; set; } = new();
            public EnvioParamRow EnvioParam { get; set; }
            public List<BaseDatosRow> BaseDatos { get; set; } = new();
            public List<FactorRow> Factores { get; set; } = new();
            public List<RateHoraRow> RateHoras { get; set; } = new();
        }

        public class PrecioRow
        {
            public string MetodologiaCodigo { get; set; } = string.Empty;
            public string PenetracionCodigo { get; set; } = string.Empty;
            public int DuracionMin { get; set; }
            public decimal ValorTotal { get; set; }
        }

        public class HorasRow
        {
            public int DuracionMin { get; set; }
            public decimal HorasScript { get; set; }
            public decimal HorasProcesamiento { get; set; }
            public decimal HorasHarmoni { get; set; }
            public decimal HorasGraficacion { get; set; }
        }

        public class ValorHoraRow
        {
            public string Nivel { get; set; } = string.Empty;
            public string Variante { get; set; } = string.Empty;
            public decimal ValorHora { get; set; }
        }

        public class CostInsumoRow
        {
            public string NSE { get; set; } = string.Empty;
            public string Tipo { get; set; } = string.Empty;
            public decimal ValorUnitario { get; set; }
        }

        public class LocacionRow
        {
            public string Ciudad { get; set; } = string.Empty;
            public decimal TarifaBase { get; set; }
            public decimal TarifaConGross { get; set; }
            public decimal DiasBase { get; set; }
        }

        public class EnvioTarifaRow
        {
            public string Tipologia { get; set; } = string.Empty;
            public decimal KiloInicial { get; set; }
            public decimal KiloAdicional { get; set; }
            public decimal SeguroPct { get; set; }
            public decimal ValorDeclaradoMin { get; set; }
        }

        public class EnvioParamRow
        {
            public int Id { get; set; }
            public decimal DivisorVolumetrico { get; set; }
            public string TipologiaUrbano { get; set; } = string.Empty;
            public string TipologiaNacional { get; set; } = string.Empty;
        }

        public class ParamMiscRow
        {
            public string Clave { get; set; } = string.Empty;
            public decimal? ValorDecimal { get; set; }
            public string ValorTexto { get; set; } = string.Empty;
        }

        public class BaseDatosRow
        {
            public string Tipo { get; set; } = string.Empty;
            public decimal Valor { get; set; }
        }

        public class CodificacionRow
        {
            public string Escenario { get; set; } = string.Empty;
            public int Registros { get; set; }
            public int PregAbiertas { get; set; }
            public int PregAbiertasMult { get; set; }
            public decimal Dias { get; set; }
            public decimal Horas { get; set; }
            public decimal ValorIpsos { get; set; }
        }

        public class MysteryTarifaRow
        {
            public string TipoVisita { get; set; } = string.Empty;
            public string Complejidad { get; set; } = string.Empty;
            public decimal VrUnitario { get; set; }
            public int OlasDefault { get; set; }
        }

        public class ProductividadCiudadRow
        {
            public string Ciudad { get; set; } = string.Empty;
            public decimal Encuestadores { get; set; }
            public decimal Productividad { get; set; }
        }

        public class CostUnitarioOpsRow
        {
            public int CodMatrix { get; set; }
            public string Actividad { get; set; } = string.Empty;
            public decimal Tarifa { get; set; }
            public string Unidad { get; set; } = string.Empty;
            public decimal? Horas { get; set; }
        }

        public class RateEstadisticaRow
        {
            public string Categoria { get; set; } = string.Empty;
            public string Servicio { get; set; } = string.Empty;
            public decimal HorasEstimadas { get; set; }
            public decimal PrecioReferencia { get; set; }
            public decimal FactorEscala { get; set; }
        }

        public class FactorRow
        {
            public int Id { get; set; }
            public string Tipo { get; set; } = string.Empty;
            public string Codigo { get; set; } = string.Empty;
            public string Descripcion { get; set; } = string.Empty;
            public decimal Factor { get; set; }
            public int Orden { get; set; }
            public bool Activo { get; set; }
        }

        public class RateHoraRow
        {
            public int Id { get; set; }
            public string Key { get; set; } = string.Empty;
            public string SL { get; set; } = string.Empty;
            public string RecordDetail { get; set; } = string.Empty;
            public string MetodologiaSL { get; set; } = string.Empty;
            public decimal HorasL3 { get; set; }
            public decimal HorasL4 { get; set; }
            public decimal HorasL5 { get; set; }
            public decimal HorasL6 { get; set; }
            public decimal HorasL7 { get; set; }
        }
    }
}
