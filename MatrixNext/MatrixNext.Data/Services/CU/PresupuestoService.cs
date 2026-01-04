using System;
using MatrixNext.Data.Adapters.CU;
using MatrixNext.Data.Modules.CU.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.CU
{
    public class PresupuestoService
    {
        private readonly PresupuestoDataAdapter _adapter;
        private readonly ILogger<PresupuestoService> _logger;

        public PresupuestoService(PresupuestoDataAdapter adapter, ILogger<PresupuestoService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
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
                var validacion = Validar(model);
                if (!string.IsNullOrEmpty(validacion))
                {
                    return (false, validacion!, model.ParAlternativa);
                }

                return _adapter.GuardarDatosGenerales(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando alternativa {Alternativa} de propuesta {Propuesta}", model.ParAlternativa, model.IdPropuesta);
                return (false, "Ocurri\u00f3 un error guardando la alternativa", model.ParAlternativa);
            }
        }

        private static string? Validar(EditarAlternativaViewModel model)
        {
            if (model == null) return "Modelo vac\u00edo";
            if (model.IdPropuesta <= 0) return "Id de propuesta inv\u00e1lido";
            if (string.IsNullOrWhiteSpace(model.Descripcion)) return "La descripci\u00f3n es requerida";
            if (model.DiasCampo <= 0) return "Los d\u00edas de campo deben ser mayores a cero";
            if (model.DiasDiseno.HasValue && model.DiasDiseno < 0) return "Los d\u00edas de dise\u00f1o no pueden ser negativos";
            if (model.DiasProcesamiento.HasValue && model.DiasProcesamiento < 0) return "Los d\u00edas de procesamiento no pueden ser negativos";
            if (model.DiasInformes.HasValue && model.DiasInformes < 0) return "Los d\u00edas de informes no pueden ser negativos";
            if (model.NumeroMediciones.HasValue && model.NumeroMediciones < 0) return "Las mediciones deben ser iguales o mayores a cero";
            if (model.MesesMediciones.HasValue && model.MesesMediciones < 0) return "Los meses entre mediciones deben ser iguales o mayores a cero";

            return null;
        }
    }
}
