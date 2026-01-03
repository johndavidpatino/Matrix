using System;
using System.Collections.Generic;
using System.Linq;
using MatrixNext.Data.Adapters.CU;
using MatrixNext.Data.Entities;
using MatrixNext.Data.Modules.CU.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.CU
{
    public class PropuestaService
    {
        private readonly PropuestaDataAdapter _adapter;
        private readonly ILogger<PropuestaService> _logger;

        public PropuestaService(PropuestaDataAdapter adapter, ILogger<PropuestaService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<PropuestaListItemViewModel> ObtenerListado(long userId, byte? estadoId = null)
        {
            try
            {
                return _adapter.ObtenerListado(userId, estadoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo propuestas");
                return new List<PropuestaListItemViewModel>();
            }
        }

        public IEnumerable<CatalogoItem<byte>> ObtenerEstados()
        {
            try { return _adapter.ObtenerEstados(); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo estados de propuesta");
                return Array.Empty<CatalogoItem<byte>>();
            }
        }

        public IEnumerable<CatalogoItem<decimal>> ObtenerProbabilidades()
        {
            try { return _adapter.ObtenerProbabilidades(); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo probabilidades");
                return Array.Empty<CatalogoItem<decimal>>();
            }
        }

        public IEnumerable<CatalogoItem<short>> ObtenerRazones()
        {
            try { return _adapter.ObtenerRazones(); }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo razones de no aprobacion");
                return Array.Empty<CatalogoItem<short>>();
            }
        }

        public PropuestaFormViewModel PrepararFormulario(long? idPropuesta, long? idBrief)
        {
            var form = new PropuestaFormViewModel
            {
                Estados = ObtenerEstados(),
                Probabilidades = ObtenerProbabilidades(),
                Razones = ObtenerRazones()
            };

            if (idPropuesta.HasValue && idPropuesta.Value > 0)
            {
                var entidad = _adapter.ObtenerEntidadPorId(idPropuesta.Value);
                if (entidad != null)
                {
                    form.Propuesta = MapEntidadAViewModel(entidad);
                }
            }
            else if (idBrief.HasValue)
            {
                form.Propuesta.BriefId = idBrief.Value;
            }

            if (form.Propuesta.Anticipo == null) form.Propuesta.Anticipo = 70;
            if (form.Propuesta.Saldo == null) form.Propuesta.Saldo = 30;
            if (form.Propuesta.Plazo == null) form.Propuesta.Plazo = 30;
            if (form.Propuesta.EstadoId == null) form.Propuesta.EstadoId = 1;

            return form;
        }

        public (bool success, string message, long id) Guardar(PropuestaViewModel model)
        {
            try
            {
                var validacion = Validar(model);
                if (!string.IsNullOrEmpty(validacion))
                {
                    return (false, validacion, 0);
                }

                var entidad = model.Id > 0
                    ? _adapter.ObtenerEntidadPorId(model.Id) ?? new CU_Propuestas()
                    : new CU_Propuestas();

                entidad.Titulo = model.Titulo?.Trim();
                entidad.Brief = model.BriefId;
                entidad.TipoId = 1; // legacy fija el tipo en 1
                entidad.ProbabilidadId = model.ProbabilidadId;
                entidad.EstadoId = model.EstadoId;
                entidad.FechaEnvio = model.FechaEnvio;
                entidad.FechaAprob = model.FechaAprob;
                entidad.RazonNoAprobId = model.RazonNoAprobId;
                entidad.JobBook = model.JobBook;
                entidad.Internacional = model.Internacional;
                entidad.Anticipo = model.Anticipo ?? 70;
                entidad.Saldo = model.Saldo ?? 30;
                entidad.Plazo = model.Plazo ?? 30;
                entidad.FechaInicioCampo = model.FechaInicioCampo;
                entidad.RequestHabeasData = model.RequestHabeasData;
                entidad.Tracking = model.Tracking;
                entidad.OrigenId = entidad.OrigenId == 0 ? (byte)1 : entidad.OrigenId;

                var id = _adapter.Guardar(entidad);
                return (true, "Propuesta guardada correctamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando propuesta");
                return (false, ex.Message, 0);
            }
        }

        public (bool success, string message) Eliminar(long id)
        {
            try
            {
                var ok = _adapter.Eliminar(id);
                return ok ? (true, "Registro eliminado") : (false, "No se encontro la propuesta");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando propuesta");
                return (false, ex.Message);
            }
        }

        public List<ObservacionViewModel> ObtenerObservaciones(long propuestaId)
        {
            try
            {
                return _adapter.ObtenerObservaciones(propuestaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo observaciones");
                return new List<ObservacionViewModel>();
            }
        }

        public (bool success, string message) AgregarObservacion(long propuestaId, long usuarioId, string observacion)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(observacion))
                {
                    return (false, "La observacion es requerida");
                }

                _adapter.GuardarObservacion(propuestaId, usuarioId, observacion.Trim());
                return (true, "Observacion guardada");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando observacion");
                return (false, ex.Message);
            }
        }

        private static PropuestaViewModel MapEntidadAViewModel(CU_Propuestas entidad)
        {
            return new PropuestaViewModel
            {
                Id = entidad.Id,
                BriefId = entidad.Brief,
                Titulo = entidad.Titulo,
                ProbabilidadId = entidad.ProbabilidadId,
                EstadoId = entidad.EstadoId,
                FechaEnvio = entidad.FechaEnvio,
                FechaAprob = entidad.FechaAprob,
                RazonNoAprobId = entidad.RazonNoAprobId,
                JobBook = entidad.JobBook,
                Internacional = entidad.Internacional ?? false,
                Tracking = entidad.Tracking ?? false,
                Anticipo = entidad.Anticipo,
                Saldo = entidad.Saldo,
                Plazo = entidad.Plazo,
                FechaInicioCampo = entidad.FechaInicioCampo,
                RequestHabeasData = entidad.RequestHabeasData
            };
        }

        private string? Validar(PropuestaViewModel model)
        {
            if (model == null) return "Modelo vacio";
            if (model.BriefId <= 0) return "Debe seleccionar un brief";
            if (string.IsNullOrWhiteSpace(model.Titulo)) return "El titulo es requerido";
            if (model.ProbabilidadId == null) return "Seleccione la probabilidad";
            if (model.EstadoId == null || model.EstadoId == 0) return "Seleccione el estado de la propuesta";
            if (string.IsNullOrWhiteSpace(model.RequestHabeasData)) return "Debe ingresar los requerimientos de Habeas Data";

            // Validaciones por estado
            switch (model.EstadoId)
            {
                case 2: // Enviada
                    if (model.FechaEnvio == null) return "Digite la fecha de envio";
                    break;
                case 3: // Vendida
                    if (model.FechaEnvio == null) return "Digite la fecha de envio";
                    if (model.FechaAprob == null) return "Digite la fecha de aprobacion";
                    if (string.IsNullOrWhiteSpace(model.JobBook) || !(model.JobBook!.Length == 9 || model.JobBook.Length == 12))
                        return "Debe escribir el JobBook (9 o 12 caracteres)";
                    break;
                case 4: // Perdida
                    if (model.FechaAprob == null) return "Digite la fecha de no aprobacion";
                    if (model.RazonNoAprobId == null) return "Seleccione la razon de no aprobacion";
                    break;
            }

            // Fecha de inicio campo requerida para estados distintos a perdida
            if (model.EstadoId != 4)
            {
                if (model.FechaInicioCampo == null) return "Digite la fecha de inicio de campo";
                if (model.FechaInicioCampo <= DateTime.Now.Date) return "La fecha de inicio de campo debe ser mayor a la fecha actual";
            }

            return null;
        }
    }
}
