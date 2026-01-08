namespace MatrixNext.Web.Services.OP;

public interface IOpEncuestasService
{
    Task<bool> ActivarEncuestaAsync(long trabajoId, decimal numeroEncuesta, string observacion, long usuarioId);
    Task<bool> AnularEncuestaAsync(long trabajoId, decimal numeroEncuesta, string observacion);
}
