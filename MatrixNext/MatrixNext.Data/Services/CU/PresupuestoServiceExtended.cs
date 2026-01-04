using System;
using System.Collections.Generic;
using MatrixNext.Data.Adapters.CU;
using MatrixNext.Data.Modules.CU.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.CU
{
    public class PresupuestoServiceExtended
    {
        private readonly PresupuestoDataAdapter _adapter;
        private readonly IQuoteCalculatorService _calculator;
        private readonly ILogger<PresupuestoServiceExtended> _logger;

        public PresupuestoServiceExtended(PresupuestoDataAdapter adapter, IQuoteCalculatorService calculator, ILogger<PresupuestoServiceExtended> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _calculator = calculator ?? throw new ArgumentNullException(nameof(calculator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public PresupuestoIndexViewModel PrepararIndex(long propuestaId, JobBookContextViewModel? contexto = null)
        {
            return new PresupuestoIndexViewModel
            {
                IdPropuesta = propuestaId,
                JobBookContext = contexto,
                Alternativas = _adapter.ObtenerAlternativas(propuestaId)
            };
        }

        public EditarAlternativaViewModel PrepararDatosGenerales(long propuestaId, int? alternativaId = null)
        {
            if (alternativaId.HasValue)
            {
                var existente = _adapter.ObtenerDatosGenerales(propuestaId, alternativaId.Value);
                if (existente != null)
                {
                    return existente;
                }
            }

            var siguiente = _adapter.CalcularSiguienteAlternativa(propuestaId);
            return new EditarAlternativaViewModel
            {
                IdPropuesta = propuestaId,
                ParAlternativa = siguiente,
                DiasCampo = 10,
                DiasDiseno = 5,
                DiasProcesamiento = 7,
                DiasInformes = 3,
                NumeroMediciones = 1,
                MesesMediciones = 1,
                TipoPresupuesto = 1
            };
        }

        public (bool success, string message, int alternativa) GuardarDatosGenerales(EditarAlternativaViewModel model)
        {
            try
            {
                var validacion = ValidarAlternativa(model);
                if (!string.IsNullOrEmpty(validacion))
                {
                    return (false, validacion!, model.ParAlternativa);
                }

                return _adapter.GuardarDatosGenerales(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando alternativa {Alternativa} de propuesta {Propuesta}", model.ParAlternativa, model.IdPropuesta);
                return (false, "Ocurrió un error guardando la alternativa", model.ParAlternativa);
            }
        }

        public List<PresupuestoListItemViewModel> ObtenerPresupuestos(long propuestaId, int alternativa, int? tecnicaFiltro = null)
        {
            return _adapter.ObtenerPresupuestos(propuestaId, alternativa, tecnicaFiltro);
        }

        public EditarPresupuestoViewModel PrepararNuevoPresupuesto(long propuestaId, int alternativa, int tecCodigo, int metCodigo, int parNacional)
        {
            return new EditarPresupuestoViewModel
            {
                IdPropuesta = propuestaId,
                ParAlternativa = alternativa,
                MetCodigo = metCodigo,
                ParNacional = parNacional,
                TecCodigo = tecCodigo,
                PregDemograficos = 15,
                ParUsaTablet = true,
                ParUsaPapel = false
            };
        }

        public EditarPresupuestoViewModel? ObtenerPresupuesto(long propuestaId, int alternativa, int metCodigo, int nacional)
        {
            return _adapter.ObtenerPresupuesto(propuestaId, alternativa, metCodigo, nacional);
        }

        public (bool success, string message) GuardarPresupuesto(EditarPresupuestoViewModel model, long usuarioId)
        {
            try
            {
                var validacion = ValidarPresupuesto(model);
                if (!string.IsNullOrEmpty(validacion))
                {
                    return (false, validacion!);
                }

                var resultado = _adapter.GuardarPresupuesto(model, usuarioId);
                if (!resultado.success)
                    return resultado;

                var totalMuestra = _calculator.ObtenerTotalMuestra(model.IdPropuesta, model.ParAlternativa, model.MetCodigo, model.ParNacional);
                if (totalMuestra > 0)
                {
                    EjecutarCalculosAutomaticos(model);
                }

                return (true, "Presupuesto guardado y calculado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando presupuesto");
                return (false, $"Error: {ex.Message}");
            }
        }

        private void EjecutarCalculosAutomaticos(EditarPresupuestoViewModel model)
        {
            if (!model.ParProductividad.HasValue || model.ParProductividad == 0)
            {
                var totalPreguntas = (model.PregCerradas ?? 0) + (model.PregCerradasMultiples ?? 0) 
                    + (model.PregAbiertas ?? 0) + (model.PregAbiertasMultiples ?? 0) + (model.PregOtras ?? 0);

                model.ParProductividad = _calculator.CalcularProductividad(
                    model.TecCodigo ?? 100, 
                    model.MetCodigo, 
                    model.ParIncidencia, 
                    totalPreguntas, 
                    model.ParTiempoEncuesta ?? 20
                );
            }

            var simulador = _calculator.EjecutarSimulador(model.IdPropuesta, model.ParAlternativa, model.MetCodigo, model.ParNacional);
            if (simulador.Exito)
            {
                _logger.LogInformation("Simulador ejecutado: CostoDirecto={Costo}, ValorVenta={Valor}, GM={GM}%", 
                    simulador.CostoDirecto, simulador.ValorVenta, simulador.GrossMargin);
            }
        }

        public (bool success, string message) AgregarMuestra(MuestraItemViewModel muestra)
        {
            return _adapter.AgregarMuestra(muestra);
        }

        public (bool success, string message) EliminarMuestra(long idPropuesta, int alternativa, int metCodigo, int ciuCodigo, int muIdentificador, int nacional)
        {
            return _adapter.EliminarMuestra(idPropuesta, alternativa, metCodigo, ciuCodigo, muIdentificador, nacional);
        }

        public (bool success, string message) EliminarPresupuesto(long idPropuesta, int alternativa, int metCodigo, int nacional)
        {
            return _adapter.EliminarPresupuesto(idPropuesta, alternativa, metCodigo, nacional);
        }

        private static string? ValidarAlternativa(EditarAlternativaViewModel model)
        {
            if (model == null) return "Modelo vacío";
            if (model.IdPropuesta <= 0) return "Id de propuesta inválido";
            if (string.IsNullOrWhiteSpace(model.Descripcion)) return "La descripción es requerida";
            if (model.DiasCampo <= 0) return "Los días de campo deben ser mayores a cero";
            if (model.DiasDiseno.HasValue && model.DiasDiseno < 0) return "Los días de diseño no pueden ser negativos";
            if (model.DiasProcesamiento.HasValue && model.DiasProcesamiento < 0) return "Los días de procesamiento no pueden ser negativos";
            if (model.DiasInformes.HasValue && model.DiasInformes < 0) return "Los días de informes no pueden ser negativos";

            return null;
        }

        private static string? ValidarPresupuesto(EditarPresupuestoViewModel model)
        {
            if (model == null) return "Modelo vacío";
            if (model.IdPropuesta <= 0) return "Id de propuesta inválido";
            if (model.MetCodigo <= 0) return "Debe seleccionar una metodología";
            if (!model.TecCodigo.HasValue || model.TecCodigo == 0) return "Debe seleccionar una técnica";
            if (string.IsNullOrWhiteSpace(model.ParGrupoObjetivo) || model.ParGrupoObjetivo.Length < 3) 
                return "Grupo objetivo es requerido (mínimo 3 caracteres)";

            var totalPreguntas = (model.PregCerradas ?? 0) + (model.PregCerradasMultiples ?? 0) 
                + (model.PregAbiertas ?? 0) + (model.PregAbiertasMultiples ?? 0) + (model.PregOtras ?? 0);

            if (totalPreguntas == 0)
                return "Debe ingresar al menos una pregunta";

            if ((model.TecCodigo == 100 || model.TecCodigo == 200) && (!model.ParIncidencia.HasValue || model.ParIncidencia == 0))
                return "Incidencia es requerida para Face-to-Face y CATI";

            return null;
        }
    }
}
