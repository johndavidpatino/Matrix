using System;
using System.Linq;
using MatrixNext.Data.Entities;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.CU
{
    /// <summary>
    /// Motor de cálculo IQuote - Migración de CoreProject.Cotizador.General
    /// Este servicio contiene la lógica algorítmica crítica para costeo de presupuestos
    /// </summary>
    public class IQuoteCalculatorService
    {
        private readonly ILogger<IQuoteCalculatorService> _logger;
        private readonly MatrixDbContext _context;

        public IQuoteCalculatorService(MatrixDbContext context, ILogger<IQuoteCalculatorService> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Calcula la productividad estimada según técnica y metodología
        /// Basado en CoreProject.Cotizador.General.GetCalculoProductividad()
        /// </summary>
        public double CalcularProductividad(int tecCodigo, int metCodigo, int? incidencia, int totalPreguntas, int tiempoEncuesta)
        {
            try
            {
                // Face-to-Face (100)
                if (tecCodigo == 100)
                {
                    // Fórmula: Base productividad = (480 min / tiempo encuesta) * (incidencia/100)
                    // 480 min = 8 horas de jornada laboral
                    var baseProd = tiempoEncuesta > 0 ? (480.0 / tiempoEncuesta) : 0;
                    var factorIncidencia = (incidencia ?? 100) / 100.0;
                    var productividad = baseProd * factorIncidencia;

                    // Ajuste por complejidad de cuestionario
                    if (totalPreguntas > 50)
                        productividad *= 0.85; // -15% si muchas preguntas
                    else if (totalPreguntas > 30)
                        productividad *= 0.9; // -10%

                    return Math.Round(productividad, 2);
                }

                // CATI (200)
                if (tecCodigo == 200)
                {
                    // CATI tiene mayor productividad (no hay desplazamiento)
                    var baseProd = tiempoEncuesta > 0 ? (420.0 / tiempoEncuesta) : 0; // 7 horas efectivas
                    var factorIncidencia = (incidencia ?? 100) / 100.0;
                    var productividad = baseProd * factorIncidencia * 1.2; // +20% vs F2F

                    if (totalPreguntas > 40)
                        productividad *= 0.9;

                    return Math.Round(productividad, 2);
                }

                // Online (300)
                if (tecCodigo == 300)
                {
                    // Online: alta productividad (autoadministrado)
                    // Depende de muestra disponible en panel
                    return 1000; // Placeholder - depende de panel
                }

                return 10; // Default conservador
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculando productividad para Tec={TecCodigo}, Met={MetCodigo}", tecCodigo, metCodigo);
                return 10;
            }
        }

        /// <summary>
        /// Calcula días de campo necesarios según muestra, productividad e incidencia
        /// Basado en CoreProject.Cotizador.General.GetCalculoDiasCampo()
        /// </summary>
        public int CalcularDiasCampo(long idPropuesta, int alternativa, int metCodigo, int nacional, double productividad, int encuestadoresPunto = 1)
        {
            try
            {
                var totalMuestra = ObtenerTotalMuestra(idPropuesta, alternativa, metCodigo, nacional);
                if (totalMuestra == 0 || productividad == 0)
                    return 10; // Default

                // Fórmula: Días = (Total Muestra / Productividad) / Encuestadores por punto
                var diasCalculados = (totalMuestra / productividad) / encuestadoresPunto;

                // Redondeo hacia arriba + 20% de contingencia
                var diasConContingencia = Math.Ceiling(diasCalculados * 1.2);

                // Mínimo 5 días, máximo razonable 60 días
                return (int)Math.Max(5, Math.Min(60, diasConContingencia));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculando días de campo");
                return 10;
            }
        }

        /// <summary>
        /// Obtiene el total de muestra configurado
        /// </summary>
        public int ObtenerTotalMuestra(long idPropuesta, int alternativa, int metCodigo, int nacional)
        {
            var muestra = _context.IQMuestra
                .Where(m => m.IdPropuesta == idPropuesta 
                    && m.ParAlternativa == alternativa 
                    && m.MetCodigo == metCodigo 
                    && m.ParNacional == nacional)
                .Sum(m => (int?)m.MuCantidad) ?? 0;

            return muestra;
        }

        /// <summary>
        /// Calcula el costo directo operacional (sin markup)
        /// Basado en IQ_ControlCostos + tarifas de operación
        /// </summary>
        public decimal CalcularCostoDirecto(IQ_Parametros parametros)
        {
            try
            {
                decimal costoTotal = 0;

                // 1. Costos de campo (encuestadores, supervisores)
                var costosCampo = CalcularCostosCampo(parametros);
                costoTotal += costosCampo;

                // 2. Costos de procesamiento (crítica, codificación, data processing)
                var costosProcesamiento = CalcularCostosProcesamiento(parametros);
                costoTotal += costosProcesamiento;

                // 3. Actividades subcontratadas
                var costosSubcontratados = parametros.ParActSubCosto ?? 0;
                costoTotal += costosSubcontratados;

                // 4. Viáticos (si aplica)
                // TODO: Implementar cálculo de viáticos por ciudad

                return Math.Round(costoTotal, 2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculando costo directo");
                return 0;
            }
        }

        private decimal CalcularCostosCampo(IQ_Parametros parametros)
        {
            // Tarifas base (deben venir de tablas maestras en producción)
            const decimal TARIFA_ENCUESTADOR_DIA = 80000; // COP
            const decimal TARIFA_SUPERVISOR_DIA = 120000;
            const decimal TARIFA_COORDINADOR_DIA = 150000;

            var totalMuestra = ObtenerTotalMuestra(parametros.IdPropuesta, parametros.ParAlternativa, parametros.MetCodigo, parametros.ParNacional);
            var productividad = parametros.ParProductividad ?? 10;
            var diasCampo = parametros.ParDiasEncuestador ?? 10;

            // Número de encuestadores necesarios
            var numEncuestadores = totalMuestra > 0 && productividad > 0 
                ? Math.Ceiling(totalMuestra / (productividad * diasCampo))
                : 1;

            var costoEncuestadores = (decimal)numEncuestadores * diasCampo * TARIFA_ENCUESTADOR_DIA;
            var costoSupervisores = (parametros.ParDiasSupervisor ?? 0) * TARIFA_SUPERVISOR_DIA;
            var costoCoordinadores = (parametros.ParDiasCoordinador ?? 0) * TARIFA_COORDINADOR_DIA;

            return costoEncuestadores + costoSupervisores + costoCoordinadores;
        }

        private decimal CalcularCostosProcesamiento(IQ_Parametros parametros)
        {
            // Tarifas por proceso (deben venir de tablas maestras)
            const decimal TARIFA_CRITICA_HORA = 25000;
            const decimal TARIFA_CODIFICACION_HORA = 22000;
            const decimal TARIFA_DATACLEAN_HORA = 28000;

            var totalPreguntas = parametros.ParTotalPreguntas ?? 0;
            var totalMuestra = ObtenerTotalMuestra(parametros.IdPropuesta, parametros.ParAlternativa, parametros.MetCodigo, parametros.ParNacional);

            // Horas de crítica = (Total Muestra * 0.5 min por encuesta) / 60
            var horasCritica = (totalMuestra * 0.5) / 60.0;

            // Horas de codificación depende de preguntas abiertas (placeholder)
            var horasCodificacion = totalPreguntas > 0 ? (totalMuestra * totalPreguntas * 0.2) / 60.0 : 0;

            // Data Clean
            var horasDataClean = (parametros.ParNProcesosDC ?? 0) * 8; // 8 horas por proceso

            var costoCritica = (decimal)horasCritica * TARIFA_CRITICA_HORA;
            var costoCodificacion = (decimal)horasCodificacion * TARIFA_CODIFICACION_HORA;
            var costoDataClean = horasDataClean * TARIFA_DATACLEAN_HORA;

            return costoCritica + costoCodificacion + costoDataClean;
        }

        /// <summary>
        /// Calcula el Gross Margin según fórmula: GM = (ValorVenta - CostoDirecto) / ValorVenta
        /// </summary>
        public double CalcularGrossMargin(decimal valorVenta, decimal costoDirecto)
        {
            if (valorVenta == 0)
                return 0;

            var gm = ((double)valorVenta - (double)costoDirecto) / (double)valorVenta * 100;
            return Math.Round(gm, 2);
        }

        /// <summary>
        /// Calcula el valor de venta aplicando markup sobre costo directo
        /// Fórmula: ValorVenta = CostoDirecto / (1 - (GrossMargin/100))
        /// </summary>
        public decimal CalcularValorVenta(decimal costoDirecto, double grossMarginObjetivo)
        {
            if (grossMarginObjetivo >= 100 || grossMarginObjetivo < 0)
                return costoDirecto * 1.3m; // Default 30% markup

            var valorVenta = costoDirecto / (decimal)(1 - (grossMarginObjetivo / 100));
            return Math.Round(valorVenta, 2);
        }

        /// <summary>
        /// Ejecuta el simulador completo de costos (equivalente a GetSimulador del legacy)
        /// </summary>
        public SimuladorResultado EjecutarSimulador(long idPropuesta, int alternativa, int metCodigo, int nacional)
        {
            try
            {
                var parametros = _context.IQParametros
                    .FirstOrDefault(p => p.IdPropuesta == idPropuesta 
                        && p.ParAlternativa == alternativa 
                        && p.MetCodigo == metCodigo 
                        && p.ParNacional == nacional);

                if (parametros == null)
                    return new SimuladorResultado { Exito = false, Mensaje = "Parámetros no encontrados" };

                var costoDirecto = CalcularCostoDirecto(parametros);
                var valorVenta = parametros.ParValorVenta ?? CalcularValorVenta(costoDirecto, 30); // Default 30% GM
                var grossMargin = CalcularGrossMargin(valorVenta, costoDirecto);

                return new SimuladorResultado
                {
                    Exito = true,
                    CostoDirecto = costoDirecto,
                    ValorVenta = valorVenta,
                    GrossMargin = grossMargin,
                    TotalMuestra = ObtenerTotalMuestra(idPropuesta, alternativa, metCodigo, nacional)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando simulador");
                return new SimuladorResultado { Exito = false, Mensaje = ex.Message };
            }
        }
    }

    public class SimuladorResultado
    {
        public bool Exito { get; set; }
        public string? Mensaje { get; set; }
        public decimal CostoDirecto { get; set; }
        public decimal ValorVenta { get; set; }
        public double GrossMargin { get; set; }
        public int TotalMuestra { get; set; }
    }
}
