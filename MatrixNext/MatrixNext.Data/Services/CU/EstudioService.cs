using System;
using System.Collections.Generic;
using MatrixNext.Data.Adapters.CU;
using MatrixNext.Data.Entities;
using MatrixNext.Data.Modules.CU.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.CU
{
    public class EstudioService
    {
        private readonly EstudioDataAdapter _adapter;
        private readonly ILogger<EstudioService> _logger;

        public EstudioService(EstudioDataAdapter adapter, ILogger<EstudioService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<EstudioListItemViewModel> Listar(long propuestaId, long usuarioId)
        {
            try
            {
                return _adapter.ObtenerListado(propuestaId, usuarioId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listando estudios");
                return new List<EstudioListItemViewModel>();
            }
        }

        public EstudioFormViewModel PrepararFormulario(long? idEstudio, long? idPropuesta)
        {
            var vm = new EstudioFormViewModel
            {
                DocumentosSoporte = _adapter.ObtenerDocumentosSoporte()
            };

            if (idEstudio.HasValue && idEstudio.Value > 0)
            {
                var entidad = _adapter.ObtenerEntidad(idEstudio.Value);
                if (entidad != null)
                {
                    vm.Estudio = MapEntidad(entidad);
                }
            }
            else if (idPropuesta.HasValue)
            {
                vm.Estudio.PropuestaId = idPropuesta.Value;
                vm.Estudio.Anticipo = 70;
                vm.Estudio.Saldo = 30;
                vm.Estudio.Plazo = 30;
                vm.Estudio.TiempoRetencionAnnos = 1;
                vm.Estudio.FechaInicio = DateTime.Now.Date;
            }

            return vm;
        }

        public (bool success, string message, long id) Guardar(EstudioViewModel model)
        {
            try
            {
                var validation = Validar(model);
                if (!string.IsNullOrEmpty(validation))
                {
                    return (false, validation, 0);
                }

                var entidad = model.Id > 0
                    ? _adapter.ObtenerEntidad(model.Id) ?? new CU_Estudios()
                    : new CU_Estudios();

                entidad.PropuestaId = model.PropuestaId;
                entidad.JobBook = model.JobBook;
                entidad.Nombre = model.Nombre;
                entidad.Valor = model.Valor;
                entidad.FechaInicio = model.FechaInicio;
                entidad.FechaTerminacion = model.FechaTerminacion;
                entidad.FechaInicioCampo = model.FechaInicioCampo;
                entidad.Anticipo = model.Anticipo ?? 70;
                entidad.Saldo = model.Saldo ?? 30;
                entidad.Plazo = model.Plazo ?? 30;
                entidad.DocumentoSoporte = model.DocumentoSoporte;
                entidad.TiempoRetencionAnnos = model.TiempoRetencionAnnos ?? 1;
                entidad.Observaciones = model.Observaciones;
                entidad.Estado = entidad.Estado == 0 ? (byte)1 : entidad.Estado;

                var id = _adapter.Guardar(entidad);
                return (true, "Estudio guardado correctamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando estudio");
                return (false, ex.Message, 0);
            }
        }

        private static EstudioViewModel MapEntidad(CU_Estudios entidad)
        {
            return new EstudioViewModel
            {
                Id = entidad.Id,
                PropuestaId = entidad.PropuestaId,
                JobBook = entidad.JobBook,
                Nombre = entidad.Nombre,
                Valor = entidad.Valor,
                FechaInicio = entidad.FechaInicio,
                FechaTerminacion = entidad.FechaTerminacion,
                FechaInicioCampo = entidad.FechaInicioCampo,
                Anticipo = entidad.Anticipo,
                Saldo = entidad.Saldo,
                Plazo = entidad.Plazo,
                DocumentoSoporte = entidad.DocumentoSoporte,
                TiempoRetencionAnnos = entidad.TiempoRetencionAnnos,
                Observaciones = entidad.Observaciones
            };
        }

        private string? Validar(EstudioViewModel model)
        {
            if (model == null) return "Modelo vacio";
            if (model.PropuestaId <= 0) return "Propuesta requerida";
            if (string.IsNullOrWhiteSpace(model.JobBook) || model.JobBook.EndsWith("00"))
                return "Debe escribir un JobBook valido";
            if (model.FechaInicioCampo == null) return "Digite la fecha de inicio de campo";
            if (model.FechaInicioCampo <= DateTime.Now.Date) return "La fecha de inicio de campo debe ser mayor a la fecha actual";
            if (model.FechaInicio == null) return "Fecha de inicio requerida";
            if (model.FechaTerminacion == null) return "Fecha de terminacion requerida";
            if (model.FechaTerminacion <= model.FechaInicio) return "La fecha de terminacion debe ser mayor a la fecha de inicio";
            if (model.DocumentoSoporte == null || model.DocumentoSoporte == 0) return "Seleccione documento soporte";

            return null;
        }
    }
}
