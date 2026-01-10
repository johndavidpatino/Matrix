using System.Threading.Tasks;

namespace MatrixNext.Data.Services.GD.Interfaces
{
    public interface IGdEmailService
    {
        Task<(bool success, string message)> EnviarNotificacionAprobacion(int solicitudId);
        Task<(bool success, string message)> EnviarNotificacionRechazo(int solicitudId);
        Task<(bool success, string message)> EnviarNotificacionSolicitud(int solicitudId);
    }
}
