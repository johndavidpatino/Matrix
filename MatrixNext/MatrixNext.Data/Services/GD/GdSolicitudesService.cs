using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Adapters.GD.Models;
using MatrixNext.Data.Services.GD.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    public class GdSolicitudesService : IGdSolicitudesService
    {
        private readonly IGdSolicitudesAdapter _adapter;
        private readonly IGdEmailService _emailService;
        private readonly ILogger<GdSolicitudesService> _logger;

        public GdSolicitudesService(
            IGdSolicitudesAdapter adapter,
            IGdEmailService emailService,
            ILogger<GdSolicitudesService> logger)
        {
            _adapter = adapter;
            _emailService = emailService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene lista de solicitudes
        /// </summary>
        public async Task<(bool success, List<SolicitudListDto> data, string message)> ObtenerSolicitudes()
        {
            try
            {
                var solicitudes = await _adapter.ObtenerSolicitudes();
                return (true, solicitudes, "Solicitudes obtenidas correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener solicitudes");
                return (false, new List<SolicitudListDto>(), "Error al obtener solicitudes. Por favor intente nuevamente.");
            }
        }

        /// <summary>
        /// Obtiene una solicitud por ID
        /// </summary>
        public async Task<(bool success, SolicitudDocumentoDto? data, string message)> ObtenerSolicitudById(int id)
        {
            try
            {
                if (id <= 0)
                    return (false, null, "ID de solicitud invÃ¡lido");

                var solicitud = await _adapter.ObtenerSolicitudById(id);
                if (solicitud == null)
                    return (false, null, "Solicitud no encontrada");

                return (true, solicitud, "Solicitud obtenida correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener solicitud por ID {Id}", id);
                return (false, null, "Error al obtener la solicitud. Por favor intente nuevamente.");
            }
        }

        /// <summary>
        /// Crea una nueva solicitud con validaciones
        /// </summary>
        public async Task<(bool success, int id, string message)> CrearSolicitud(SolicitudCreateInputDto dto)
        {
            try
            {
                // REGLA 12: Validaciones de entrada
                if (dto == null)
                    return (false, 0, "Datos de solicitud invÃ¡lidos");

                if (dto.TipoSolicitud <= 0)
                    return (false, 0, "Tipo de solicitud no especificado");

                if (dto.IdDocumento <= 0)
                    return (false, 0, "Documento no especificado");

                if (dto.IdSolicitante <= 0)
                    return (false, 0, "Solicitante no especificado");

                if (string.IsNullOrWhiteSpace(dto.Area))
                    return (false, 0, "Ãrea es requerida");

                if (string.IsNullOrWhiteSpace(dto.Cargo))
                    return (false, 0, "Cargo es requerido");

                if (string.IsNullOrWhiteSpace(dto.Razon))
                    return (false, 0, "RazÃ³n de solicitud es requerida");

                if (string.IsNullOrWhiteSpace(dto.Descripcion))
                    return (false, 0, "DescripciÃ³n es requerida");

                // Mapear InputDto a DTO
                var documento = new SolicitudDocumentoDto
                {
                    TipoSolicitud = dto.TipoSolicitud,
                    IdDocumento = dto.IdDocumento,
                    IdSolicitante = dto.IdSolicitante,
                    Area = dto.Area,
                    Cargo = dto.Cargo,
                    Razon = dto.Razon,
                    Descripcion = dto.Descripcion,
                    IdEstado = dto.IdEstado ?? 1, // Estado por defecto: Pendiente
                    Comentarios = dto.Comentarios ?? string.Empty,
                    FechaRegistro = DateTime.Now,
                    AreaUso = dto.AreaUso,
                    SitioAcceso = dto.SitioAcceso,
                    NombreDocumento = dto.NombreDocumento ?? string.Empty,
                    Codigo = dto.Codigo ?? string.Empty
                };

                // REGLA 4: EjecuciÃ³n exacta del SP
                var id = await _adapter.CrearSolicitud(documento);
                
                if (id <= 0)
                    return (false, 0, "No se pudo crear la solicitud");

                _logger.LogInformation("Solicitud creada: {Id}", id);
                return (true, id, "Solicitud creada correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear solicitud");
                return (false, 0, "Error al crear la solicitud. Por favor intente nuevamente.");
            }
        }

        /// <summary>
        /// Asigna revisores a una solicitud
        /// Importante: recibe lista de IDs, NO usa Session
        /// </summary>
        public async Task<(bool success, string message)> AsignarRevisores(int idSolicitud, List<int> idRevisores)
        {
            try
            {
                // Validar entrada
                if (idSolicitud <= 0)
                    return (false, "ID de solicitud invÃ¡lido");

                if (idRevisores == null || idRevisores.Count == 0)
                    return (false, "Debe seleccionar al menos un revisor");

                // Verificar que solicitud existe
                var solicitud = await _adapter.ObtenerSolicitudById(idSolicitud);
                if (solicitud == null)
                    return (false, "Solicitud no encontrada");

                // Obtener ID documento controlado (asumiendo que coincide con IdDocumento)
                var idDocumentoControlado = solicitud.IdDocumento;

                // Crear revisiÃ³n para cada revisor
                var exitosas = 0;
                var errores = new List<string>();

                foreach (var idRevisor in idRevisores)
                {
                    try
                    {
                        var exito = await _adapter.CrearRevision(idSolicitud, idDocumentoControlado, idRevisor);
                        if (exito)
                            exitosas++;
                        else
                            errores.Add($"No se pudo asignar revisor ID {idRevisor}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error asignando revisor ID {Id} a solicitud {SolicitudId}", idRevisor, idSolicitud);
                        errores.Add($"Error asignando revisor ID {idRevisor}");
                    }
                }

                if (exitosas == 0)
                    return (false, string.Join("; ", errores));

                var mensaje = $"{exitosas} revisor(es) asignado(s) correctamente";
                if (errores.Count > 0)
                    mensaje += $"; {string.Join("; ", errores)}";

                _logger.LogInformation("Revisores asignados a solicitud {Id}: {Mensaje}", idSolicitud, mensaje);

                // Enviar notificaciones por email a revisores asignados (FASE 4 - Sprint 6)
                // IMPORTANTE: NO await - fire-and-forget para no bloquear request
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var (success, emailMessage) = await _emailService.EnviarNotificacionSolicitud(idSolicitud);
                        if (success)
                            _logger.LogInformation("Notificaciones enviadas para solicitud {Id}: {Mensaje}", idSolicitud, emailMessage);
                        else
                            _logger.LogWarning("Error enviando notificaciones para solicitud {Id}: {Mensaje}", idSolicitud, emailMessage);
                    }
                    catch (Exception exEmail)
                    {
                        _logger.LogError(exEmail, "ExcepciÃ³n al enviar notificaciones para solicitud {Id}", idSolicitud);
                    }
                });

                return (true, mensaje);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al asignar revisores a solicitud {Id}", idSolicitud);
                return (false, "Error al asignar revisores. Por favor intente nuevamente.");
            }
        }

        /// <summary>
        /// Obtiene datos para el formulario de creaciÃ³n (dropdowns, etc)
        /// </summary>
        public async Task<(bool success, SolicitudFormDataDto formData)> ObtenerFormData()
        {
            try
            {
                var formData = new SolicitudFormDataDto();

                // Cargar dropdowns en paralelo
                var tiposTask = _adapter.ObtenerTiposSolicitud();
                var documentosTask = _adapter.ObtenerDocumentos();
                var usuariosTask = _adapter.ObtenerUsuarios();
                var estadosTask = _adapter.ObtenerEstados();

                await Task.WhenAll(tiposTask, documentosTask, usuariosTask, estadosTask);

                formData.TiposSolicitud = (await tiposTask)
                    .Select(t => new SelectListItemDto { Id = t.Id, Nombre = t.Nombre })
                    .ToList();

                formData.Documentos = (await documentosTask)
                    .Select(d => new SelectListItemDto { Id = d.Id, Nombre = d.Nombre })
                    .ToList();

                formData.Usuarios = (await usuariosTask)
                    .Select(u => new SelectListItemDto { Id = u.Id, Nombre = $"{u.Nombre} ({u.Email})" })
                    .ToList();

                formData.Estados = (await estadosTask)
                    .Select(e => new SelectListItemDto { Id = e.Id, Nombre = e.Nombre })
                    .ToList();

                return (true, formData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener datos del formulario");
                return (false, new SolicitudFormDataDto());
            }
        }
    }
}

