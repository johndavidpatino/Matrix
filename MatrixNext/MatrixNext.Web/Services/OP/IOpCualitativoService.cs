using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio principal para gestión de trabajos cualitativos (COE)
/// Ref: ANALISIS_OP_CUALITATIVO_FASE1.md § 1.1-1.2
/// Tareas: OP-C01 (Trabajos)
/// </summary>
public interface IOpCualitativoService
{
    /// <summary>
    /// Obtiene trabajos cualitativos filtrados por coordinador (Permiso 42)
    /// Ref: Trabajos.aspx.vb líneas 21-47
    /// </summary>
    Task<(bool Success, List<TrabajoCualitativoVm> Data, string Error)> ObtenerTrabajosPorCoordinadorAsync(
        long usuarioId, long? coeId = null);

    /// <summary>
    /// Obtiene trabajos cualitativos por COE (sin filtro coordinador)
    /// Ref: Trabajos.aspx.vb líneas 26-45 (Permiso 148)
    /// </summary>
    Task<(bool Success, List<TrabajoCualitativoVm> Data, string Error)> ObtenerTrabajosPorCoeAsync(
        long? coeId = null, int? tipo = null, string estado = null);

    /// <summary>
    /// Obtiene configuración actual de un trabajo cualitativo
    /// Ref: Trabajos.aspx.vb líneas 145-167 (CargarConfiguracion)
    /// </summary>
    Task<(bool Success, ConfiguracionTrabajoVm Data, string Error)> ObtenerConfiguracionTrabajoAsync(
        long trabajoId);

    /// <summary>
    /// Guarda configuración de fechas y tipo de recolección
    /// Ref: Trabajos.aspx.vb líneas 171-195 (btnGuardarConfiguracion_Click)
    /// </summary>
    Task<(bool Success, string Error)> GuardarConfiguracionTrabajoAsync(
        long trabajoId, ConfiguracionTrabajoVm configuracion, long usuarioId);

    /// <summary>
    /// Valida permisos específicos de usuario para acciones en COE
    /// Ref: Trabajos.aspx.vb línea 26 (VerificarPermisoUsuario 42, 148)
    /// </summary>
    Task<bool> ValidarPermisoCoordinadorAsync(long usuarioId, int permisoId);
}
