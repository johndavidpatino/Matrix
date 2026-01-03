using System;
using System.Collections.Generic;
using System.Linq;
using MatrixNext.Data.Adapters.CU;
using MatrixNext.Data.Entities;
using MatrixNext.Data.Modules.CU.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.CU
{
    public class EstudioService
    {
        private readonly EstudioDataAdapter _adapter;
        private readonly PresupuestoDataAdapter _presupuestoAdapter;
        private readonly ILogger<EstudioService> _logger;

        public EstudioService(EstudioDataAdapter adapter, PresupuestoDataAdapter presupuestoAdapter, ILogger<EstudioService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _presupuestoAdapter = presupuestoAdapter ?? throw new ArgumentNullException(nameof(presupuestoAdapter));
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
                    
                    // TODO-P0-02: Cargar presupuestos asignados si es edición
                    try
                    {
                        var presupuestosAsignados = _presupuestoAdapter.ObtenerPresupuestosAsignadosXEstudio(idEstudio.Value);
                        vm.Estudio.PresupuestosSeleccionados = presupuestosAsignados.Select(p => p.Id).ToList();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Error cargando presupuestos asignados al estudio {IdEstudio}", idEstudio.Value);
                    }
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
                
                // TODO-P0-02: Obtener presupuestos aprobados de la propuesta
                try
                {
                    vm.PresupuestosAprobados = _presupuestoAdapter.ObtenerPresupuestosAprobados(idPropuesta.Value);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Error obteniendo presupuestos aprobados para propuesta {IdPropuesta}", idPropuesta.Value);
                    vm.PresupuestosAprobados = new List<PresupuestoAprobadoViewModel>();
                }
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
                
                // TODO-P0-02: Asignar presupuestos aprobados al estudio
                if (model.PresupuestosSeleccionados != null && model.PresupuestosSeleccionados.Any())
                {
                    try
                    {
                        _presupuestoAdapter.AsignarPresupuestosAEstudio(id, model.PresupuestosSeleccionados);
                        _logger.LogInformation("Asignados {Count} presupuestos al estudio {IdEstudio}", 
                            model.PresupuestosSeleccionados.Count, id);
                    }
                    catch (Exception exPresu)
                    {
                        _logger.LogError(exPresu, "Error asignando presupuestos al estudio {IdEstudio}", id);
                        // No falla la operación, solo registra el error
                    }
                }
                
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
            
            // TODO-P0-02: Validar que se seleccionó al menos un presupuesto
            if (model.PresupuestosSeleccionados == null || !model.PresupuestosSeleccionados.Any())
                return "Debe seleccionar al menos un presupuesto aprobado";
            
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
