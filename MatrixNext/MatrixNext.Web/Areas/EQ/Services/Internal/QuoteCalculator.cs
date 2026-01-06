using System;
using System.Linq;
using MatrixNext.Web.Areas.EQ.Models;
using MatrixNext.Web.Areas.EQ.Services.Masters;

namespace MatrixNext.Web.Areas.EQ.Services.Internal
{
    /// <summary>
    /// Motor de calculo basado en tablas maestras (sin hardcodes): campo, incentivos, insumos, staff OPS/SL, mystery y margenes basicos.
    /// </summary>
    public class QuoteCalculator
    {
        private readonly EasyQuoteMasterService _masters;

        public QuoteCalculator(EasyQuoteMasterService masters)
        {
            _masters = masters;
        }

        public EQSummary Calcular(EasyQuoteViewModel vm)
        {
            if (vm == null) return new EQSummary();
            var q = vm.Questionnaire ?? new EQQuestionnaire();

            var metodologia = vm.Methodology?.MetodologiaRecoleccion;
            if (string.IsNullOrWhiteSpace(metodologia)) metodologia = "F2F";
            var duracion = q.DuracionMin <= 0 ? 5 : q.DuracionMin;
            var penetracion = string.IsNullOrWhiteSpace(q.PenetracionCodigo) ? "MAS82" : q.PenetracionCodigo;

            decimal Total(Func<EQSampleCity, decimal> selector) => vm.SampleCities?.Where(c => c.Activa).Sum(selector) ?? 0;
            var totalMuestra = Total(c => c.MuestraTotal);
            var n1 = Total(c => c.NSE1);
            var n2 = Total(c => c.NSE2);
            var n3 = Total(c => c.NSE3);
            var n4 = Total(c => c.NSE4);
            var n5 = Total(c => c.NSE5);
            var n6 = Total(c => c.NSE6);

            // FORMULA 3 y 4: CATI/Online lookup (si metodologia es CATI o AUTO=Online)
            decimal valorEncuesta = 0m;
            if (string.Equals(metodologia, "CATI", StringComparison.OrdinalIgnoreCase))
            {
                valorEncuesta = _masters.GetPrecioEncuesta("CATI", penetracion, duracion) ?? 0;
            }
            else if (string.Equals(metodologia, "AUTO", StringComparison.OrdinalIgnoreCase) || string.Equals(metodologia, "Online", StringComparison.OrdinalIgnoreCase))
            {
                valorEncuesta = _masters.GetPrecioEncuesta("Online", penetracion, duracion) ?? 0;
            }
            else
            {
                valorEncuesta = _masters.GetPrecioEncuesta(metodologia, penetracion, duracion) ?? 0;
            }
            
            // FORMULA 2: Siembra factor (1 o 2 segun checkbox)
            var factorSiembra = q.Siembra ? 2m : 1m;
            
            // FORMULA 1: Parafiscales F2F (16.522% adicional si metodologia es F2F)
            var factorParafiscal = string.Equals(metodologia, "F2F", StringComparison.OrdinalIgnoreCase) ? 1.16522m : 1m;
            var costoCampo = valorEncuesta * totalMuestra * factorSiembra * factorParafiscal;

            // Productividad/dias de campo por ciudad (desde tabla eq_productividad_ciudad)
            decimal GetEnc(string ciudad)
            {
                var row = _masters.AllProductividad().FirstOrDefault(p => string.Equals(p.Ciudad, ciudad, StringComparison.OrdinalIgnoreCase));
                return row?.Encuestadores ?? 7;
            }
            decimal GetProd(string ciudad)
            {
                var row = _masters.AllProductividad().FirstOrDefault(p => string.Equals(p.Ciudad, ciudad, StringComparison.OrdinalIgnoreCase));
                return row?.Productividad ?? 4;
            }
            var diasCampo = 0m;
            foreach (var c in vm.SampleCities.Where(x => x.Activa))
            {
                var enc = GetEnc(c.Ciudad);
                var prod = GetProd(c.Ciudad);
                if (enc * prod > 0)
                {
                    var dias = Math.Ceiling((c.MuestraTotal) / (enc * prod));
                    if (dias > diasCampo) diasCampo = dias;
                }
            }
            if (diasCampo == 0 && totalMuestra > 0)
                diasCampo = Math.Ceiling(totalMuestra / (7 * 4));

            // FORMULA 5: Mystery completo (D73-D75: Edicion/AlquilerEquipo/CompraDispositivos)
            var costoMystery = 0m;
            if (vm.MysteryVisits != null)
            {
                foreach (var v in vm.MysteryVisits)
                {
                    if (string.IsNullOrWhiteSpace(v.TipoVisita)) continue;
                    var baseTarifa = _masters.GetMysteryTarifa(v.TipoVisita, v.Complejidad)?.VrUnitario ?? 0;
                    costoMystery += baseTarifa * Math.Max(1, v.NumOlas);
                    costoMystery += (v.Desplazamientos ?? 0) + (v.Tanqueos ?? 0) + (v.Alertas ?? 0);
                    // D73, D74, D75 - Edicion, Alquiler, Compra
                    costoMystery += (v.Edicion ?? 0) + (v.AlquilerEquipos ?? 0) + (v.CompraDispositivos ?? 0);
                }
            }

            // Reclutamiento e incentivos (valores por NSE)
            decimal Reclu(string nse) => _masters.GetCostoInsumo("Reclutamiento", nse);
            decimal Obs(string nse) => _masters.GetCostoInsumo("Obsequio", nse);
            var reclutamiento = Reclu("NSE1_2") * (n1 + n2)
                                + Reclu("NSE3") * n3
                                + Reclu("NSE4") * n4
                                + Reclu("NSE5_6") * (n5 + n6);
            var obsequios = Obs("NSE1_2") * (n1 + n2)
                            + Obs("NSE3") * n3
                            + Obs("NSE4") * n4
                            + Obs("NSE5_6") * (n5 + n6);
            var incentivos = obsequios / 0.93m; // 7% comision de bonos
            
            // FORMULA 6: Insumos prueba (ClasePrueba + ProductosTestear + CompraProducto)
            var insumosPrueba = 0m;
            if (!string.IsNullOrWhiteSpace(q.ClasePrueba) && !q.ClasePrueba.Equals("No aplica", StringComparison.OrdinalIgnoreCase))
            {
                var factorClase = _masters.GetFactorCodigo("CLASE_PRUEBA", q.ClasePrueba);
                insumosPrueba = (factorClase ?? 1m) * Math.Max(q.ProductosTestear, 1);
            }
            
            // FORMULA 7: Blind/rotulación (EtiquetadoTipo)
            var costoEtiquetado = 0m;
            if (!string.IsNullOrWhiteSpace(q.EtiquetadoTipo) && !q.EtiquetadoTipo.Equals("No", StringComparison.OrdinalIgnoreCase))
            {
                var factorEtiq = _masters.GetFactorCodigo("ETIQUETADO", q.EtiquetadoTipo);
                costoEtiquetado = (factorEtiq ?? 0) * Math.Max(q.ProductosTestear, 1) * Math.Max(q.ProductosPorResp, 1);
            }
            
            var insumos = reclutamiento + insumosPrueba + costoEtiquetado;

            // Staff OPS (scripting, procesamiento, datacleaning, toplines, harmoni, graficacion, ASCII, estadistica)
            var horas = _masters.GetHoras(duracion);
            decimal MultScript(string tipo) => tipo?.ToLowerInvariant() switch
            {
                "duplicado" => 4m,
                "reutilizacion" => 2m,
                _ => 1m
            };

            decimal TarifaOps(string actividadPrefix, decimal fallback)
            {
                var row = _masters.GetCostUnitario(actividadPrefix);
                return row?.Tarifa ?? fallback;
            }

            var tarifaOpsDefault = _masters.GetValorHoraOps("L6") ?? 54000m;
            var costoScripting = q.Scripting ? horas.HorasScript * MultScript(q.ScriptingTipo) * TarifaOps("Scripting", tarifaOpsDefault) : 0;
            var costoProc = q.Procesamiento ? horas.HorasProcesamiento * Math.Max(1, q.NumProcesamientos) * TarifaOps("Procesamiento", tarifaOpsDefault) : 0;
            var dcFactor = string.Equals(q.DataCleaning, "Parcial", StringComparison.OrdinalIgnoreCase) ? 0.6m :
                           string.Equals(q.DataCleaning, "No", StringComparison.OrdinalIgnoreCase) ? 0m : 1m;
            var costoDC = horas.HorasProcesamiento * dcFactor * TarifaOps("Datacleaning", tarifaOpsDefault);
            var costoTopline = q.TopLine ? horas.HorasGraficacion * TarifaOps("TopLines", tarifaOpsDefault) : 0;
            // FORMULA 17: Harmoni lookup (si checkbox activo)
            var costoHarmoni = q.Harmoni ? horas.HorasHarmoni * TarifaOps("Harmoni", tarifaOpsDefault) : 0;
            // FORMULA 18: Graficacion lookup (si checkbox activo)
            var costoGraf = q.Graficacion ? horas.HorasGraficacion * TarifaOps("Graficacion", tarifaOpsDefault) : 0;
            var costoAscii = q.ASCIIFlag ? (horas.HorasHarmoni > 0 ? horas.HorasHarmoni : 9m) * TarifaOps("Conversi", tarifaOpsDefault) : 0;
            var costoEstadistica = q.ProcesoEstadistico ? (_masters.GetRateEstadisticaDefault()?.PrecioReferencia ?? 0) : 0;
            var staffOps = costoScripting + costoProc + costoDC + costoTopline + costoHarmoni + costoGraf + costoAscii + costoEstadistica;

            // FORMULA 20: Staff SL con KEY lookup (SL|RecordDetail|MetodologiaSL)
            var staffSl = 0m;
            if (vm.StaffSL != null)
            {
                foreach (var s in vm.StaffSL)
                {
                    var tarifa = s.Tarifa > 0 ? s.Tarifa : (_masters.GetValorHoraOps(s.Nivel) ?? tarifaOpsDefault);
                    var horasMin = _masters.GetHorasMinimas(vm.Header?.SL, vm.Header?.RecordDetail, vm.Header?.MetodologiaSL, s.Nivel) ?? 0;
                    var horasReal = Math.Max(s.HorasPresup, horasMin);
                    staffSl += horasReal * tarifa;
                }
            }
            
            // FORMULA 14: Siembra telefónica (si ApoyoReclutamientoTipo)
            var costoSiembraTel = 0m;
            if (!string.IsNullOrWhiteSpace(vm.Logistica?.ApoyoReclutamientoTipo))
            {
                var factorApoyo = _masters.GetFactorCodigo("APOYO_RECLUTAMIENTO", vm.Logistica.ApoyoReclutamientoTipo) ?? 1m;
                costoSiembraTel = factorApoyo * totalMuestra;
            }
            
            // FORMULA 15: Tablets (PatinadoresCiudad * tarifaTablet)
            var costoTablets = 0m;
            if (q.PatinadoresCiudad > 0)
            {
                var tarifaTablet = _masters.GetMisc("COSTO_TABLET")?.ValorDecimal ?? 25000m;
                costoTablets = q.PatinadoresCiudad * tarifaTablet * (vm.SampleCities?.Count(x => x.Activa) ?? 1);
            }

            var compraProducto = q.CompraProducto;
            
            // FORMULA 8: Transporte niños (EstudioNinos checkbox = 15000)
            var transporteNinos = (vm.Logistica?.EstudioNinos ?? false) ? 15000m * totalMuestra : 0;
            
            // FORMULA 9: Transporte bebidas/producto (TaxiParticipantes = 28000)
            var transporteBebidas = (vm.Logistica?.TaxiParticipantes ?? false) ? 28000m * totalMuestra : 0;

            // Locaciones (tarifa gross * dias setup+campo)
            var costoLocaciones = 0m;
            foreach (var c in vm.SampleCities.Where(x => x.Activa))
            {
                var loc = _masters.GetLocacion(c.Ciudad);
                if (loc == null) continue;
                var dias = loc.DiasBase > 0 ? loc.DiasBase : diasCampo;
                if (dias == 0) dias = diasCampo;
                var tarifa = loc.TarifaConGross > 0 ? loc.TarifaConGross : loc.TarifaBase;
                costoLocaciones += tarifa * dias;
            }
            // FORMULA 11: Refrigeracion (factor 1.1 + nevera 970000)
            if (q.Refrigeracion)
            {
                var factorRef = _masters.GetMisc("FACTOR_REFRIGERACION")?.ValorDecimal ?? 1.1m;
                costoLocaciones *= factorRef;
                var costoNevera = _masters.GetMisc("COSTO_NEVERA")?.ValorDecimal ?? 970000m;
                costoLocaciones += costoNevera;
            }

            // FORMULA 10: Envios volumétrico (DimensionLargoCm * AnchoCm * AltoCm / 5000)
            var costoEnvio = 0m;
            if (vm.Methodology?.EnvioCiudades == true && vm.Methodology.PesoProductoGr > 0)
            {
                var pesoKgReal = vm.Methodology.PesoProductoGr / 1000m;
                var divisorVol = _masters.GetMisc("DIVISOR_VOLUMETRICO")?.ValorDecimal ?? (_masters.GetEnvioParam()?.DivisorVolumetrico ?? 5000m);
                
                var pesoVolumetrico = pesoKgReal;
                if (vm.Logistica?.DimensionLargoCm > 0 && vm.Logistica?.DimensionAnchoCm > 0 && vm.Logistica?.DimensionAltoCm > 0)
                {
                    pesoVolumetrico = (vm.Logistica.DimensionLargoCm ?? 0) * (vm.Logistica.DimensionAnchoCm ?? 0) * (vm.Logistica.DimensionAltoCm ?? 0) / divisorVol;
                }
                
                var pesoKg = Math.Max(pesoKgReal, pesoVolumetrico);
                var ciudadesActivas = vm.SampleCities?.Count(x => x.Activa) ?? 0;
                var envioParam = _masters.GetEnvioParam();
                var tipologia = ciudadesActivas <= 1 ? (envioParam?.TipologiaUrbano ?? "URBANO") : (envioParam?.TipologiaNacional ?? "NACIONAL");
                var tarifaEnv = _masters.GetEnvio(tipologia) ?? _masters.GetEnvio("URBANO");
                if (tarifaEnv != null && ciudadesActivas > 0)
                {
                    var adicionalKg = Math.Max(0m, pesoKg - 1m);
                    var seguro = Math.Max(tarifaEnv.ValorDeclaradoMin * tarifaEnv.SeguroPct, 0);
                    var costoUnit = tarifaEnv.KiloInicial + adicionalKg * tarifaEnv.KiloAdicional + seguro;
                    costoEnvio = costoUnit * ciudadesActivas;
                }
            }

            // Base de datos (parametrizado)
            var costoBaseDatos = 0m;
            if (!string.IsNullOrWhiteSpace(vm.Methodology?.BaseDatos) && !vm.Methodology.BaseDatos.Equals("No requiere", StringComparison.OrdinalIgnoreCase))
            {
                var val = _masters.GetBaseDatos(vm.Methodology.BaseDatos);
                if (val.HasValue) costoBaseDatos = val.Value;
            }

            // FORMULA 12: Reprografia (ReprografiaPaginas * factor)
            var costoReprografia = 0m;
            if ((vm.Logistica?.ReprografiaPaginas ?? 0) > 0)
            {
                var factorReprograf = _masters.GetMisc("COSTO_REPROGRAFIA_PAGINA")?.ValorDecimal ?? 50m;
                costoReprografia = (vm.Logistica?.ReprografiaPaginas ?? 0) * factorReprograf * totalMuestra;
            }
            
            // FORMULA 21: Viaticos diferenciados (override o calculados, + dias setup/campo)
            var viaticos = 0m;
            var diasSetup = vm.Logistica?.DiasSetup ?? 0;
            var diasViaticos = Math.Max(diasCampo, vm.Logistica?.DiasCampo ?? 0) + diasSetup;
            
            if (vm.Logistica?.ViaticasCampoOverride.HasValue == true)
            {
                viaticos = vm.Logistica.ViaticasCampoOverride.Value;
            }
            else
            {
                var tEnc = _masters.GetCostUnitario("Transportes PST Encuestadores");
                var tSup = _masters.GetCostUnitario("Transportes PST Supervisores");
                var totalEncuestadores = vm.SampleCities.Where(x=>x.Activa).Sum(x=>GetEnc(x.Ciudad));
                var totalSupervisores = vm.SampleCities.Any(x=>x.Activa) ? vm.SampleCities.Count(x=>x.Activa) * 1.75m : 0;
                if (tEnc != null) viaticos += (tEnc.Tarifa * totalEncuestadores) * diasViaticos;
                if (tSup != null) viaticos += (tSup.Tarifa * totalSupervisores) * diasViaticos;
            }

            // FORMULA 19: Codificacion completa (lookup tabla cod + cantidad + muestra)
            var costoCodif = 0m;
            if (q.Codificacion && (q.PregAbiertas > 0 || q.PregAbiertasMult > 0))
            {
                var cod = _masters.GetCodificacionDefault();
                if (cod != null)
                {
                    // Simple + Multiple * factor, segun cantidad de registros
                    var cantidadRegs = totalMuestra; // aproximacion
                    costoCodif = cod.ValorIpsos * (q.PregAbiertas + q.PregAbiertasMult * 1.5m) * (cantidadRegs > 0 ? cantidadRegs / 100m : 1m);
                }
            }

            // Proveedor externo / internacional
            var proveedores = (vm.Header?.ValorProveedorExterno ?? 0) + (vm.Header?.ValorProveedorInternacional ?? 0);
            
            // Otros costos varios
            var otrosCostos = q.OtrosCostos + (vm.Logistica?.OtrosIncentivos ?? 0);

            // FORMULA 13, 22-26: Margenes GM, PB+RMF, ProfTime, OP, %OP
            var directCost = costoCampo + costoMystery + incentivos + insumos + staffOps + staffSl + compraProducto + 
                             costoLocaciones + costoEnvio + proveedores + costoBaseDatos + viaticos + costoCodif + 
                             transporteNinos + transporteBebidas + costoReprografia + costoSiembraTel + costoTablets + otrosCostos;
            
            // FORMULA 13: Verificacion GM = 21.45% del directCost
            var gmOps = directCost * 0.2145m;
            
            // FORMULA 22: PB + RMF = -AOT * 4.3%
            var aot = directCost + gmOps;
            var pbRmf = -aot * 0.043m;
            
            // FORMULA 23: ProfTime = -staff_sl_total
            var profTime = -staffSl;
            
            // FORMULA 24: OP = GM + PB + RMF + ProfTime
            var op = gmOps + pbRmf + profTime;
            
            // FORMULA 25: %OP = (OP / AOT) * 100
            var porcOp = aot == 0 ? 0 : op / aot;

            return new EQSummary
            {
                CostoCampo = costoCampo + costoMystery,
                CostoCalidad = 0,
                Viaticos = viaticos,
                Incentivos = incentivos,
                Insumos = insumos,
                StaffOps = staffOps,
                StaffSL = staffSl,
                CompraProducto = compraProducto,
                Tablets = 0,
                DirectCostOps = directCost,
                GM = gmOps,
                PB_RMF = pbRmf,
                ProfTime = profTime,
                OP = op,
                AOT = aot,
                PorcOP = porcOp
            };
        }
    }
}
