using MatrixNext.Data.Modules.TH.Ausencias.Adapters;
using MatrixNext.Data.Modules.TH.Ausencias.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MatrixNext.Data.Modules.TH.Ausencias.Services
{
    /// <summary>
    /// Service para gestión de ausencias
    /// Contiene lógica de negocio y orquestación
    /// </summary>
    public class AusenciaService
    {
        private readonly AusenciaDataAdapter _adapter;
        private readonly ILogger<AusenciaService> _logger;

        public AusenciaService(AusenciaDataAdapter adapter, ILogger<AusenciaService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #region CREAR SOLICITUD

        /// <summary>
        /// Crea una nueva solicitud de ausencia
        /// </summary>
        public (bool success, string message, long id) CrearSolicitud(long idEmpleado,
            SolicitudAusenciaFormViewModel modelo)
        {
            try
            {
                if (modelo.FechaInicio > modelo.FechaFin)
                    return (false, "La fecha de inicio no puede ser mayor que la fecha fin", 0);

                if (modelo.AprobadorId <= 0)
                    return (false, "Debe seleccionar un aprobador", 0);

                var validacion = _adapter.ValidarSolicitudAusencia(idEmpleado, modelo.FechaInicio, modelo.FechaFin, modelo.TipoAusencia);
                if (validacion != null && validacion.Result != 0)
                    return (false, validacion.MensajeResultado ?? "La solicitud no es válida", 0);

                var calculo = _adapter.CalcularDias(modelo.FechaInicio, modelo.FechaFin, false, idEmpleado);
                if (calculo == null)
                    return (false, "No fue posible calcular los días de ausencia", 0);

                var diasCalendario = (short)calculo.DiasCalendario;
                var diasLaborales = (byte)calculo.DiasLaborales;

                var observaciones = modelo.Observaciones ?? string.Empty;

                var id = _adapter.CrearSolicitudAusencia(
                    idEmpleado,
                    modelo.TipoAusencia,
                    modelo.FechaInicio,
                    modelo.FechaFin,
                    diasCalendario,
                    diasLaborales,
                    modelo.AprobadorId,
                    observaciones,
                    idEmpleado,
                    modelo.FiniCausacion,
                    modelo.FFinCausacion
                );

                _logger.LogInformation($"Solicitud de ausencia creada: ID={id}, Empleado={idEmpleado}");
                return (true, "Solicitud radicada correctamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando solicitud de ausencia");
                return (false, $"Error: {ex.Message}", 0);
            }
        }

        #endregion

        #region APROBAR/RECHAZAR

        /// <summary>
        /// Aprueba una solicitud de ausencia
        /// </summary>
        public (bool success, string message) AprobarSolicitud(long idSolicitud, long idAprobador,
            string? observaciones = null)
        {
            try
            {
                var solicitud = _adapter.ObtenerPorId(idSolicitud);
                if (solicitud == null)
                    return (false, "Solicitud no encontrada");

                var result = _adapter.AprobarSolicitud(idSolicitud, idAprobador, observaciones);
                if (!result)
                    return (false, "No se pudo aprobar la solicitud");

                _logger.LogInformation($"Solicitud aprobada: ID={idSolicitud}, AprobadorID={idAprobador}");
                return (true, "Solicitud aprobada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando solicitud");
                return (false, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Rechaza una solicitud de ausencia
        /// </summary>
        public (bool success, string message) RechazarSolicitud(long idSolicitud, long idAprobador,
            string? observaciones = null)
        {
            try
            {
                var solicitud = _adapter.ObtenerPorId(idSolicitud);
                if (solicitud == null)
                    return (false, "Solicitud no encontrada");

                var result = _adapter.RechazarSolicitud(idSolicitud, idAprobador, observaciones);
                if (!result)
                    return (false, "No se pudo rechazar la solicitud");

                _logger.LogInformation($"Solicitud rechazada: ID={idSolicitud}, AprobadorID={idAprobador}");
                return (true, "Solicitud rechazada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando solicitud");
                return (false, $"Error: {ex.Message}");
            }
        }

        #endregion

        #region OBTENER SOLICITUDES

        /// <summary>
        /// Obtiene solicitud por ID
        /// </summary>
        public (bool success, AusenciaViewModel? data) ObtenerPorId(long id)
        {
            try
            {
                var data = _adapter.ObtenerPorId(id);
                if (data == null)
                    return (false, null);

                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniéndosolicitud: {id}");
                return (false, null);
            }
        }

        /// <summary>
        /// Obtiene solicitudes del empleado
        /// </summary>
        public (bool success, string message, List<AusenciaViewModel> data) ObtenerSolicitudesEmpleado(long idEmpleado)
        {
            try
            {
                var data = _adapter.ObtenerSolicitudes(idEmpleado: idEmpleado);
                return (true, "", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo solicitudes del empleado: {idEmpleado}");
                return (false, ex.Message, new List<AusenciaViewModel>());
            }
        }

        /// <summary>
        /// Obtiene solicitudes pendientes de aprobación (Estado = 1 o 5)
        /// </summary>
        public (bool success, string message, List<ReporteSolicitudesPendientesViewModel> data) ObtenerSolicitudesPendientes()
        {
            try
            {
                var data = _adapter.ObtenerSolicitudesPendientes();
                return (true, "", data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo solicitudes pendientes");
                return (false, ex.Message, new List<ReporteSolicitudesPendientesViewModel>());
            }
        }

        #endregion

        #region INCAPACIDADES

        /// <summary>
        /// Crea un registro de incapacidad para una solicitud
        /// </summary>
        public (bool success, string message) CrearIncapacidad(long idSolicitud,
            IncapacidadViewModel modelo)
        {
            try
            {
                var result = _adapter.CrearIncapacidad(
                    idSolicitud,
                    modelo.EntidadConsulta,
                    modelo.IPS ?? string.Empty,
                    modelo.RegistroMedico ?? string.Empty,
                    modelo.TipoIncapacidad,
                    modelo.ClaseAusencia,
                    modelo.SOAT,
                    modelo.FechaAccidenteTrabajo,
                    modelo.Comentarios ?? string.Empty,
                    modelo.CIE ?? string.Empty
                );

                if (!result)
                    return (false, "No se pudo crear el registro de incapacidad");

                _logger.LogInformation($"Incapacidad creada para solicitud: {idSolicitud}");
                return (true, "Incapacidad registrada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando incapacidad");
                return (false, $"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene incapacidad de una solicitud
        /// </summary>
        public (bool success, IncapacidadViewModel? data) ObtenerIncapacidad(long idSolicitud)
        {
            try
            {
                var data = _adapter.ObtenerIncapacidadPorSolicitud(idSolicitud);
                return (data != null, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo incapacidad: {idSolicitud}");
                return (false, null);
            }
        }

        #endregion

        #region CATÁLOGOS

        /// <summary>
        /// Obtiene tipos de ausencia disponibles
        /// </summary>
        public (bool success, List<TipoAusenciaViewModel> data) ObtenerTiposAusencia()
        {
            try
            {
                var data = _adapter.ObtenerTiposAusencia();
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo tipos de ausencia");
                return (false, new List<TipoAusenciaViewModel>());
            }
        }

        /// <summary>
        /// Obtiene aprobadores disponibles
        /// </summary>
        public (bool success, List<AprobadorViewModel> data) ObtenerAprobadores()
        {
            try
            {
                var data = _adapter.ObtenerAprobadores();
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo aprobadores");
                return (false, new List<AprobadorViewModel>());
            }
        }

        #endregion

        #region OPERACIONES TH LEGADO

        public (bool success, CalculoDiasViewModel? data) CalcularDias(long idEmpleado, DateTime? inicio, DateTime? fin, bool incluirSabadoComoDiaLaboral = false)
        {
            try
            {
                var data = _adapter.CalcularDias(inicio, fin, incluirSabadoComoDiaLaboral, idEmpleado);
                return (data != null, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculando días de ausencia");
                return (false, null);
            }
        }

        public (bool success, ResultadoValidacionViewModel? data) ValidarSolicitudAusencia(long idEmpleado, DateTime? inicio, DateTime? fin, int? tipo)
        {
            try
            {
                var data = _adapter.ValidarSolicitudAusencia(idEmpleado, inicio, fin, tipo);
                return (data != null, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando solicitud de ausencia");
                return (false, null);
            }
        }

        public (bool success, string message) AnularSolicitud(long idSolicitud)
        {
            try
            {
                _adapter.AnularSolicitud(idSolicitud);
                return (true, "Solicitud anulada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error anulando solicitud");
                return (false, ex.Message);
            }
        }

        public (bool success, string message) CausarVacaciones(long idSolicitud)
        {
            try
            {
                _adapter.CausarVacaciones(idSolicitud);
                return (true, "Causación ejecutada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error causando vacaciones");
                return (false, ex.Message);
            }
        }

        public (bool success, DateTime diaRegreso) ObtenerDiaRegreso(DateTime fecha)
        {
            try
            {
                var dia = _adapter.ObtenerDiaRegreso(fecha);
                return (true, dia);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo día de regreso");
                return (false, default);
            }
        }

        public (bool success, List<PeriodoCausadoVacacionesViewModel> data) ObtenerCausacionVacaciones(long idSolicitud)
        {
            try
            {
                var data = _adapter.ObtenerCausacionVacaciones(idSolicitud);
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo causación de vacaciones");
                return (false, new List<PeriodoCausadoVacacionesViewModel>());
            }
        }

        #endregion

        #region REPORTES

        public (bool success, List<ReporteVacacionesViewModel> data) ReporteVacaciones()
        {
            try
            {
                var data = _adapter.ReporteVacaciones();
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reporte de vacaciones");
                return (false, new List<ReporteVacacionesViewModel>());
            }
        }

        public (bool success, List<ReporteBeneficiosViewModel> data) ReporteBeneficios(int anno)
        {
            try
            {
                var data = _adapter.ReporteBeneficios(anno);
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reporte de beneficios");
                return (false, new List<ReporteBeneficiosViewModel>());
            }
        }

        public (bool success, List<ReporteAusentismoViewModel> data) ReporteAusentismo(int anno)
        {
            try
            {
                var data = _adapter.ReporteAusentismo(anno);
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reporte de ausentismo");
                return (false, new List<ReporteAusentismoViewModel>());
            }
        }

        public (bool success, List<ReporteIncapacidadesViewModel> data) ReporteIncapacidades(int anno)
        {
            try
            {
                var data = _adapter.ReporteIncapacidades(anno);
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reporte de incapacidades");
                return (false, new List<ReporteIncapacidadesViewModel>());
            }
        }

        public (bool success, List<ReporteVacacionesDetalladoViewModel> data) ReporteVacacionesDetallado(int anno)
        {
            try
            {
                var data = _adapter.ReporteVacacionesDetallado(anno);
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reporte detallado de vacaciones");
                return (false, new List<ReporteVacacionesDetalladoViewModel>());
            }
        }

        public (bool success, List<ReporteVacacionesNominaViewModel> data) ReporteVacacionesNomina(int anno)
        {
            try
            {
                var data = _adapter.ReporteVacacionesNomina(anno);
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo reporte de vacaciones para nómina");
                return (false, new List<ReporteVacacionesNominaViewModel>());
            }
        }

        #endregion

        #region EQUIPO

        public (bool success, string message, List<AusenciaEquipoViewModel> data) ObtenerAusenciasEquipo(long idJefe, DateTime inicio, DateTime fin)
        {
            try
            {
                var data = _adapter.ObtenerAusenciasEquipo(idJefe, inicio, fin);
                return (true, string.Empty, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo ausencias del equipo");
                return (false, ex.Message, new List<AusenciaEquipoViewModel>());
            }
        }

        public (bool success, List<BeneficioPendienteViewModel> data) ObtenerBeneficiosPendientes(long idEmpleado)
        {
            try
            {
                var data = _adapter.ObtenerBeneficiosPendientes(idEmpleado);
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo beneficios pendientes");
                return (false, new List<BeneficioPendienteViewModel>());
            }
        }

        public (bool success, List<SubordinadoViewModel> data) ObtenerSubordinados(long idJefe)
        {
            try
            {
                var data = _adapter.ObtenerSubordinados(idJefe);
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo subordinados");
                return (false, new List<SubordinadoViewModel>());
            }
        }

        public (bool success, List<AusenciaEquipoViewModel> data) ObtenerPersonasConAusencias(long idJefe, string search)
        {
            try
            {
                var data = _adapter.ObtenerPersonasConAusencias(idJefe, search);
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo personas con ausencias");
                return (false, new List<AusenciaEquipoViewModel>());
            }
        }

        public (bool success, string message) AgregarSubordinado(long idJefe, long idSubordinado)
        {
            try
            {
                var (success, message) = _adapter.AgregarSubordinado(idJefe, idSubordinado);
                return (success, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error agregando subordinado");
                return (false, ex.Message);
            }
        }

        public (bool success, string message) RemoverSubordinado(long idSubordinado)
        {
            try
            {
                var (success, message) = _adapter.RemoverSubordinado(idSubordinado);
                return (success, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removiendo subordinado");
                return (false, ex.Message);
            }
        }

        #endregion
    }
}
