using MatrixNext.Web.Services.PY;

namespace MatrixNext.Web.Services.OP;

public class OpPermisosService : IOpPermisosService
{
    private readonly IPYPermisosService _permisosService;
    private readonly ILogger<OpPermisosService> _logger;

    public OpPermisosService(IPYPermisosService permisosService, ILogger<OpPermisosService> logger)
    {
        _permisosService = permisosService;
        _logger = logger;
    }

    public async Task<bool> TienePermisoAsync(long usuarioId, int permisoCodigo)
    {
        var permisos = await _permisosService.ObtenerPermisosUsuarioAsync(usuarioId);
        var result = permisos.Contains(permisoCodigo);
        _logger.LogDebug("Usuario {usuarioId} permiso {permiso} -> {result}", usuarioId, permisoCodigo, result);
        return result;
    }
}
