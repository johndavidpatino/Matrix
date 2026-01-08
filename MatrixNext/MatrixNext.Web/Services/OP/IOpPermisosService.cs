namespace MatrixNext.Web.Services.OP;

public interface IOpPermisosService
{
    Task<bool> TienePermisoAsync(long usuarioId, int permisoCodigo);
}
