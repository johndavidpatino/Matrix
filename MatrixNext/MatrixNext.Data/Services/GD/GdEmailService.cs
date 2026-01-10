using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Services.GD.Interfaces;
using MatrixNext.Web.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    /// <summary>
    /// Servicio de notificaciones por email para Gestión Documental
    /// Usa IEmailQueueService para envío asíncrono sin bloquear request HTTP
    /// </summary>
    public class GdEmailService : IGdEmailService
    {
        private readonly IEmailQueueService _emailQueueService;
        private readonly IGdSolicitudesAdapter _solicitudesAdapter;
        private readonly ILogger<GdEmailService> _logger;
        private readonly IHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public GdEmailService(
            IEmailQueueService emailQueueService,
            IGdSolicitudesAdapter solicitudesAdapter,
            ILogger<GdEmailService> logger,
            IHostEnvironment env,
            IConfiguration config)
        {
            _emailQueueService = emailQueueService;
            _solicitudesAdapter = solicitudesAdapter;
            _logger = logger;
            _env = env;
            _config = config;
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
        }

        /// <summary>
        /// Envía notificación a revisores cuando se crea una solicitud
        /// REGLA 14: Async/await
        /// </summary>
        public async Task<(bool success, string message)> EnviarNotificacionSolicitud(int solicitudId)
        {
            try
            {
                _logger.LogInformation("Iniciando envío de notificación para solicitud {SolicitudId}", solicitudId);

                // 1. Obtener datos de solicitud
                var solicitud = await _solicitudesAdapter.ObtenerSolicitudById(solicitudId);
                if (solicitud == null)
                {
                    _logger.LogWarning("Solicitud {SolicitudId} no encontrada", solicitudId);
                    return (false, "Solicitud no encontrada");
                }

                // 2. Obtener revisores asignados
                var revisores = await _solicitudesAdapter.ObtenerRevisoresPendientes(solicitudId);
                if (revisores == null || !revisores.Any())
                {
                    _logger.LogWarning("No hay revisores asignados para solicitud {SolicitudId}", solicitudId);
                    return (false, "No hay revisores asignados");
                }

                // 3. Obtener template HTML
                var templatePath = Path.Combine(_env.ContentRootPath, "wwwroot", "EmailTemplates", "GD", "SolicitudCreada.html");
                if (!File.Exists(templatePath))
                {
                    _logger.LogError("Template de email no encontrado: {Path}", templatePath);
                    return (false, $"Template no encontrado: {templatePath}");
                }

                var templateHtml = await File.ReadAllTextAsync(templatePath);

                // 4. Preparar variables comunes del template
                var baseUrl = _config["AppSettings:BaseUrl"] ?? "https://matrix.local";
                var linkAprobacion = $"{baseUrl}/GD/Solicitudes/Detalle/{solicitudId}";

                // 5. Enviar email a cada revisor
                var emailsEnviados = 0;
                var errores = new List<string>();

                foreach (var revisor in revisores)
                {
                    try
                    {
                        // Obtener email del revisor desde BD
                        var emailRevisor = await ObtenerEmailUsuario(revisor.IdRevisor);
                        if (string.IsNullOrEmpty(emailRevisor))
                        {
                            _logger.LogWarning("Revisor {RevisorId} no tiene email configurado", revisor.IdRevisor);
                            errores.Add($"Revisor {revisor.NombreRevisor} sin email");
                            continue;
                        }

                        // Renderizar template con variables del revisor
                        var htmlBody = RenderTemplate(templateHtml, new
                        {
                            NombreRevisor = revisor.NombreRevisor,
                            NombreDocumento = solicitud.IdDocumento.ToString(), // TODO: Obtener nombre real del documento
                            Solicitante = solicitud.IdSolicitante.ToString(), // TODO: Obtener nombre real del solicitante
                            Area = solicitud.Area,
                            Razon = solicitud.Razon,
                            Descripcion = solicitud.Descripcion,
                            FechaSolicitud = solicitud.FechaRegistro.ToString("dd/MM/yyyy HH:mm"),
                            LinkAprobacion = linkAprobacion
                        });

                        // Encolar email (NO bloquea request)
                        await _emailQueueService.QueueEmailAsync(
                            destinatario: emailRevisor,
                            asunto: $"📋 Nueva Solicitud de Documento - {solicitud.IdDocumento}",
                            cuerpo: htmlBody,
                            esHtml: true
                        );

                        emailsEnviados++;
                        _logger.LogInformation("Email encolado para revisor {Revisor} ({Email})", revisor.NombreRevisor, emailRevisor);
                    }
                    catch (Exception exRevisor)
                    {
                        _logger.LogError(exRevisor, "Error al procesar email para revisor {RevisorId}", revisor.IdRevisor);
                        errores.Add($"Error al enviar a {revisor.NombreRevisor}: {exRevisor.Message}");
                    }
                }

                // 6. Resultado final
                if (emailsEnviados == 0)
                {
                    var mensajeError = errores.Any() ? string.Join("; ", errores) : "No se pudo enviar ningún email";
                    _logger.LogError("No se encolaron emails para solicitud {SolicitudId}: {Errores}", solicitudId, mensajeError);
                    return (false, mensajeError);
                }

                var mensaje = $"{emailsEnviados}/{revisores.Count} notificaciones encoladas correctamente";
                if (errores.Any())
                    mensaje += $". Errores: {string.Join("; ", errores)}";

                _logger.LogInformation("Notificaciones para solicitud {SolicitudId}: {Mensaje}", solicitudId, mensaje);
                return (true, mensaje);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificaciones para solicitud {SolicitudId}", solicitudId);
                return (false, $"Error al enviar notificaciones: {ex.Message}");
            }
        }

        /// <summary>
        /// Envía notificación de aprobación (EXCLUIDO - no existe en legacy)
        /// Placeholder para futuro
        /// </summary>
        public Task<(bool success, string message)> EnviarNotificacionAprobacion(int solicitudId)
        {
            _logger.LogInformation("EnviarNotificacionAprobacion llamado para solicitud {SolicitudId} (EXCLUIDO - no implementado en legacy)", solicitudId);
            return Task.FromResult((true, "Funcionalidad excluida - no existe en sistema legacy"));
        }

        /// <summary>
        /// Envía notificación de rechazo (EXCLUIDO - no existe en legacy)
        /// Placeholder para futuro
        /// </summary>
        public Task<(bool success, string message)> EnviarNotificacionRechazo(int solicitudId)
        {
            _logger.LogInformation("EnviarNotificacionRechazo llamado para solicitud {SolicitudId} (EXCLUIDO - no implementado en legacy)", solicitudId);
            return Task.FromResult((true, "Funcionalidad excluida - no existe en sistema legacy"));
        }

        /// <summary>
        /// Obtiene email de un usuario desde tabla US_Usuarios
        /// REGLA 14: Async/await
        /// </summary>
        private async Task<string> ObtenerEmailUsuario(int idUsuario)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();
                
                var email = await conn.QueryFirstOrDefaultAsync<string>(
                    "SELECT Email FROM US_Usuarios WHERE Id = @Id",
                    new { Id = idUsuario }
                );

                if (string.IsNullOrWhiteSpace(email))
                {
                    _logger.LogWarning("Usuario {IdUsuario} no tiene email configurado en BD", idUsuario);
                    return string.Empty;
                }

                return email;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener email de usuario {IdUsuario}", idUsuario);
                return string.Empty;
            }
        }

        /// <summary>
        /// Renderiza template HTML reemplazando variables {{Variable}}
        /// Método simple sin dependencias externas (NO usa RazorEngine completo)
        /// </summary>
        private string RenderTemplate(string template, object model)
        {
            var result = template;
            var properties = model.GetType().GetProperties();

            foreach (var prop in properties)
            {
                var placeholder = $"{{{{{prop.Name}}}}}"; // {{Variable}}
                var value = prop.GetValue(model)?.ToString() ?? string.Empty;
                result = result.Replace(placeholder, value);
            }

            return result;
        }
    }
}
