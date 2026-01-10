using System.Threading.Tasks;
using MatrixNext.Data.Services.GD.Interfaces;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Services.GD
{
    public class GdEmailService : IGdEmailService
    {
        private readonly ILogger<GdEmailService> _logger;

        public GdEmailService(ILogger<GdEmailService> logger)
        {
            _logger = logger;
        }

        public Task<(bool success, string message)> EnviarNotificacionAprobacion(int solicitudId)
        {
            _logger.LogInformation("EnviarNotificacionAprobacion: {SolicitudId}", solicitudId);
            return Task.FromResult((true, "Notificación de aprobación encolada"));
        }

        public Task<(bool success, string message)> EnviarNotificacionRechazo(int solicitudId)
        {
            _logger.LogInformation("EnviarNotificacionRechazo: {SolicitudId}", solicitudId);
            return Task.FromResult((true, "Notificación de rechazo encolada"));
        }

        public Task<(bool success, string message)> EnviarNotificacionSolicitud(int solicitudId)
        {
            _logger.LogInformation("EnviarNotificacionSolicitud: {SolicitudId}", solicitudId);
            return Task.FromResult((true, "Notificación de nueva solicitud encolada"));
        }
    }
}
