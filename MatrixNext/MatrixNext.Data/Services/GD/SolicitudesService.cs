/// <summary>
/// Service para lÃ³gica de negocio de Solicitudes de Documentos
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md Â§ Sprint 12.3.1
/// </summary>
namespace MatrixNext.Data.Services.GD
{
    using MatrixNext.Data.DTOs.GD;
    using MatrixNext.Data.Adapters.GD;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class SolicitudesService : ISolicitudesService
    {
        private readonly ISolicitudesAdapter _adapter;
        private readonly ILogger<SolicitudesService> _logger;

        public SolicitudesService(ISolicitudesAdapter adapter, ILogger<SolicitudesService> logger)
        {
            _adapter = adapter;
            _logger = logger;
        }

        public async Task<IEnumerable<SolicitudDocumentoDto>> ObtenerSolicitudesAsync(long? idProyecto = null, long? idEstado = null, long? idSolicitante = null)
        {
            try
            {
                return await _adapter.ObtenerSolicitudesAsync(idProyecto, idEstado, idSolicitante);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en servicio obteniendo solicitudes");
                throw;
            }
        }

        public async Task<SolicitudDocumentoDto> ObtenerSolicitudAsync(long idSolicitud)
        {
            try
            {
                return await _adapter.ObtenerSolicitudAsync(idSolicitud);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en servicio obteniendo solicitud. IdSolicitud: {IdSolicitud}", idSolicitud);
                throw;
            }
        }

        public async Task<(bool exitoso, string mensaje, long idSolicitud)> CrearSolicitudAsync(SolicitudDocumentoDto solicitud, bool asignacionAutomatica = true)
        {
            try
            {
                // Validaciones de negocio
                if (solicitud.IdProyecto <= 0)
                {
                    return (false, "Proyecto es obligatorio", 0);
                }

                if (solicitud.IdTipoDocumento <= 0)
                {
                    return (false, "Tipo de documento es obligatorio", 0);
                }

                if (string.IsNullOrWhiteSpace(solicitud.Descripcion))
                {
                    return (false, "DescripciÃ³n es obligatoria", 0);
                }

                if (solicitud.FechaRequerida.HasValue && solicitud.FechaRequerida.Value < DateTime.Now.Date)
                {
                    return (false, "Fecha requerida no puede ser en el pasado", 0);
                }

                // Crear solicitud
                var idSolicitud = await _adapter.CrearSolicitudAsync(solicitud);

                if (idSolicitud == 0)
                {
                    return (false, "Error al crear la solicitud", 0);
                }

                // AsignaciÃ³n de revisores
                if (asignacionAutomatica)
                {
                    var config = await _adapter.ObtenerConfiguracionRevisionAsync(solicitud.IdProceso);

                    if (config != null && config.AsignacionAutomatica)
                    {
                        var revisoresPorDefecto = await _adapter.ObtenerRevisoresPorDefectoAsync(solicitud.IdProceso);

                        if (revisoresPorDefecto.Any())
                        {
                            await _adapter.AsignarRevisoresAsync(idSolicitud, revisoresPorDefecto, solicitud.RegistradoPor);

                            if (solicitud.EnviarNotificacion)
                            {
                                var contenido = solicitud.ContenidoEmail ?? 
                                    $"Se le ha asignado la revisiÃ³n de la solicitud {idSolicitud}. Por favor ingrese al sistema para revisar.";
                                
                                await _adapter.EnviarNotificacionRevisoresAsync(idSolicitud, contenido);
                            }

                            _logger.LogInformation("AsignaciÃ³n automÃ¡tica completada. IdSolicitud: {IdSolicitud}, Revisores: {Cantidad}",
                                idSolicitud, revisoresPorDefecto.Count);
                        }
                    }
                }
                else if (solicitud.IdsRevisores?.Any() == true)
                {
                    // AsignaciÃ³n manual
                    await _adapter.AsignarRevisoresAsync(idSolicitud, solicitud.IdsRevisores, solicitud.RegistradoPor);

                    if (solicitud.EnviarNotificacion)
                    {
                        await _adapter.EnviarNotificacionRevisoresAsync(idSolicitud, solicitud.ContenidoEmail);
                    }
                }

                return (true, "Solicitud creada exitosamente", idSolicitud);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando solicitud. Proyecto: {IdProyecto}", solicitud.IdProyecto);
                return (false, "Error al crear la solicitud", 0);
            }
        }

        public async Task<(bool exitoso, string mensaje)> ActualizarSolicitudAsync(SolicitudDocumentoDto solicitud)
        {
            try
            {
                // Validaciones
                if (solicitud.IdSolicitud <= 0)
                {
                    return (false, "Solicitud invÃ¡lida");
                }

                if (string.IsNullOrWhiteSpace(solicitud.Descripcion))
                {
                    return (false, "DescripciÃ³n es obligatoria");
                }

                var actualizado = await _adapter.ActualizarSolicitudAsync(solicitud);

                if (actualizado)
                {
                    return (true, "Solicitud actualizada exitosamente");
                }
                else
                {
                    return (false, "Error al actualizar la solicitud");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando solicitud. IdSolicitud: {IdSolicitud}", solicitud.IdSolicitud);
                return (false, "Error al actualizar la solicitud");
            }
        }

        public async Task<(bool exitoso, string mensaje)> AsignarRevisoresAsync(AsignacionRevisoresDto asignacion, long usuarioId)
        {
            try
            {
                // Validaciones
                if (asignacion.IdSolicitud <= 0)
                {
                    return (false, "Solicitud invÃ¡lida");
                }

                if (!asignacion.IdsRevisores.Any())
                {
                    return (false, "Debe seleccionar al menos un revisor");
                }

                // Asignar revisores
                var asignado = await _adapter.AsignarRevisoresAsync(asignacion.IdSolicitud, asignacion.IdsRevisores, usuarioId);

                if (!asignado)
                {
                    return (false, "Error al asignar revisores");
                }

                // Enviar notificaciÃ³n
                if (asignacion.EnviarNotificacion)
                {
                    var contenido = asignacion.ContenidoEmail ?? 
                        $"Se le ha asignado la revisiÃ³n de la solicitud {asignacion.IdSolicitud}. Por favor ingrese al sistema para revisar.";
                    
                    await _adapter.EnviarNotificacionRevisoresAsync(asignacion.IdSolicitud, contenido);
                }

                return (true, $"{asignacion.IdsRevisores.Count} revisor(es) asignado(s) exitosamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error asignando revisores. IdSolicitud: {IdSolicitud}", asignacion.IdSolicitud);
                return (false, "Error al asignar revisores");
            }
        }

        public async Task<ConfiguracionRevisionDto> ObtenerConfiguracionRevisionAsync(long idProceso)
        {
            try
            {
                return await _adapter.ObtenerConfiguracionRevisionAsync(idProceso);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo configuraciÃ³n. IdProceso: {IdProceso}", idProceso);
                return null;
            }
        }

        public async Task<(bool exitoso, string mensaje)> AprobarRevisionAsync(AprobacionRevisionDto aprobacion)
        {
            try
            {
                // Validaciones
                if (aprobacion.IdRevision <= 0)
                    return (false, "Id de revisiÃ³n invÃ¡lido");

                if (aprobacion.IdRevisor <= 0)
                    return (false, "Revisor invÃ¡lido");

                // Aprobar revisiÃ³n
                aprobacion.TipoRevision = 2; // Aprobado
                aprobacion.FechaRevision = DateTime.Now;
                await _adapter.AprobarRevisionAsync(aprobacion);

                _logger.LogInformation("RevisiÃ³n {IdRevision} aprobada por usuario {IdRevisor}", 
                    aprobacion.IdRevision, aprobacion.IdRevisor);

                // Obtener resumen de aprobaciones
                var resumen = await _adapter.ObtenerResumenAprobacionAsync(aprobacion.IdSolicitud);

                // Cambiar estado de solicitud si todos aprobaron o se alcanzÃ³ mayorÃ­a
                if (resumen.EstadoFinal == 2) // Aprobado
                {
                    await _adapter.CambiarEstadoSolicitudAsync(
                        aprobacion.IdSolicitud, 
                        2, // Estado Aprobado
                        aprobacion.IdRevisor, 
                        resumen.MensajeFinal
                    );

                    // Enviar notificaciÃ³n al solicitante
                    if (aprobacion.EnviarNotificacion)
                    {
                        await _adapter.EnviarNotificacionRevisoresAsync(
                            aprobacion.IdSolicitud, 
                            $"Su solicitud ha sido APROBADA. {resumen.MensajeFinal}"
                        );
                    }

                    _logger.LogInformation("Solicitud {IdSolicitud} aprobada automÃ¡ticamente: {Mensaje}", 
                        aprobacion.IdSolicitud, resumen.MensajeFinal);
                    
                    return (true, $"AprobaciÃ³n registrada. {resumen.MensajeFinal}");
                }

                return (true, $"AprobaciÃ³n registrada. Pendiente: {resumen.RevisoresPendientes} revisor(es)");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error aprobando revisiÃ³n {IdRevision}", aprobacion.IdRevision);
                return (false, "Error al aprobar la revisiÃ³n");
            }
        }

        public async Task<(bool exitoso, string mensaje)> RechazarRevisionAsync(AprobacionRevisionDto rechazo)
        {
            try
            {
                // Validaciones
                if (rechazo.IdRevision <= 0)
                    return (false, "Id de revisiÃ³n invÃ¡lido");

                if (rechazo.IdRevisor <= 0)
                    return (false, "Revisor invÃ¡lido");

                if (string.IsNullOrWhiteSpace(rechazo.ComentarioRevision))
                    return (false, "El comentario es obligatorio para rechazos");

                // Rechazar revisiÃ³n
                rechazo.TipoRevision = 3; // Rechazado
                rechazo.FechaRevision = DateTime.Now;
                await _adapter.RechazarRevisionAsync(rechazo);

                _logger.LogInformation("RevisiÃ³n {IdRevision} rechazada por usuario {IdRevisor}", 
                    rechazo.IdRevision, rechazo.IdRevisor);

                // Cambiar estado de solicitud a Rechazado automÃ¡ticamente
                await _adapter.CambiarEstadoSolicitudAsync(
                    rechazo.IdSolicitud, 
                    3, // Estado Rechazado
                    rechazo.IdRevisor, 
                    $"Rechazado por revisor: {rechazo.ComentarioRevision}"
                );

                // Enviar notificaciÃ³n al solicitante
                if (rechazo.EnviarNotificacion)
                {
                    await _adapter.EnviarNotificacionRevisoresAsync(
                        rechazo.IdSolicitud, 
                        $"Su solicitud ha sido RECHAZADA. Motivo: {rechazo.ComentarioRevision}"
                    );
                }

                _logger.LogInformation("Solicitud {IdSolicitud} rechazada automÃ¡ticamente", rechazo.IdSolicitud);
                
                return (true, "Rechazo registrado. Solicitud marcada como Rechazada y solicitante notificado");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rechazando revisiÃ³n {IdRevision}", rechazo.IdRevision);
                return (false, "Error al rechazar la revisiÃ³n");
            }
        }

        public async Task<ResumenAprobacionDto> ObtenerResumenAprobacionAsync(long idSolicitud)
        {
            return await _adapter.ObtenerResumenAprobacionAsync(idSolicitud);
        }

        public async Task<IEnumerable<HistorialRevisionDto>> ObtenerHistorialRevisionesAsync(long idSolicitud)
        {
            try
            {
                var historial = await _adapter.ObtenerHistorialRevisionesAsync(idSolicitud);
                
                _logger.LogInformation("Historial de revisiones obtenido. IdSolicitud: {IdSolicitud}, Eventos: {Count}", 
                    idSolicitud, historial.Count());
                
                return historial;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo historial de revisiones. IdSolicitud: {IdSolicitud}", idSolicitud);
                throw;
            }
        }

        public async Task<TimelineSolicitudDto> ObtenerTimelineSolicitudAsync(long idSolicitud)
        {
            try
            {
                var timeline = await _adapter.ObtenerTimelineSolicitudAsync(idSolicitud);
                
                if (timeline == null)
                {
                    _logger.LogWarning("Timeline no encontrado. IdSolicitud: {IdSolicitud}", idSolicitud);
                    return null;
                }

                _logger.LogInformation("Timeline obtenido. IdSolicitud: {IdSolicitud}, Eventos: {Count}, Ãšltima actividad: {UltimaActividad}", 
                    idSolicitud, timeline.TotalEventos, timeline.UltimaActividad);
                
                return timeline;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo timeline. IdSolicitud: {IdSolicitud}", idSolicitud);
                throw;
            }
        }
    }
}

