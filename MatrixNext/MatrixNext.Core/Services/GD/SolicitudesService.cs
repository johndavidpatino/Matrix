/// <summary>
/// Service para lógica de negocio de Solicitudes de Documentos
/// Ref: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.3.1
/// </summary>
namespace MatrixNext.Core.Services.GD
{
    using MatrixNext.Core.DTOs.GD;
    using MatrixNext.Infrastructure.Adapters.GD;
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
                    return (false, "Descripción es obligatoria", 0);
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

                // Asignación de revisores
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
                                    $"Se le ha asignado la revisión de la solicitud {idSolicitud}. Por favor ingrese al sistema para revisar.";
                                
                                await _adapter.EnviarNotificacionRevisoresAsync(idSolicitud, contenido);
                            }

                            _logger.LogInformation("Asignación automática completada. IdSolicitud: {IdSolicitud}, Revisores: {Cantidad}",
                                idSolicitud, revisoresPorDefecto.Count);
                        }
                    }
                }
                else if (solicitud.IdsRevisores?.Any() == true)
                {
                    // Asignación manual
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
                    return (false, "Solicitud inválida");
                }

                if (string.IsNullOrWhiteSpace(solicitud.Descripcion))
                {
                    return (false, "Descripción es obligatoria");
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
                    return (false, "Solicitud inválida");
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

                // Enviar notificación
                if (asignacion.EnviarNotificacion)
                {
                    var contenido = asignacion.ContenidoEmail ?? 
                        $"Se le ha asignado la revisión de la solicitud {asignacion.IdSolicitud}. Por favor ingrese al sistema para revisar.";
                    
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
                _logger.LogError(ex, "Error obteniendo configuración. IdProceso: {IdProceso}", idProceso);
                return null;
            }
        }
    }
}
