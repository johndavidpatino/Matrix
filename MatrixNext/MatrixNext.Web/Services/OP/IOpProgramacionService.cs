using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Servicio para gestión de programación de campo cualitativo
/// Ref: ProgramacionCampo.aspx.vb (822 LOC)
/// Estados: Creado, Asignado, Confirmado, Ejecutado, Cancelado, NoAsistio, Reprogramado
/// </summary>
public interface IOpProgramacionService
{
    /// <summary>
    /// Obtener programaciones por trabajo
    /// Ref: ProgramacionCampo.aspx.vb líneas 45-89 (Page_Load)
    /// </summary>
    Task<(bool Success, List<ProgramacionCampoVm> Data, string Error)> ObtenerProgramacionesPorTrabajoAsync(
        long trabajoId, string? estado = null);

    /// <summary>
    /// Guardar programación (crear/editar)
    /// Ref: ProgramacionCampo.aspx.vb líneas 125-214 (btnSaveProgramar_Click)
    /// </summary>
    Task<(bool Success, long ProgramacionId, string Error)> GuardarProgramacionAsync(
        ProgramacionCampoVm programacion, long usuarioId);

    /// <summary>
    /// Cambiar estado de programación
    /// Ref: ProgramacionCampo.aspx.vb líneas 320-365 (CambiarEstado)
    /// </summary>
    Task<(bool Success, string Error)> CambiarEstadoProgramacionAsync(
        long programacionId, int nuevoEstado, long usuarioId, string? observaciones = null);

    /// <summary>
    /// Exportar programaciones a Excel
    /// Ref: ProgramacionCampo.aspx.vb líneas 520-618 (ExportarExcel con ClosedXML)
    /// </summary>
    Task<(bool Success, byte[] Data, string Error)> ExportarProgramacionesExcelAsync(
        long trabajoId, string? estado = null);

    /// <summary>
    /// Obtener entrevistados disponibles para programar
    /// Ref: ProgramacionCampo.aspx.vb líneas 220-287 (CargarEntrevistados)
    /// </summary>
    Task<(bool Success, List<EntrevistadoDisponibleVm> Data, string Error)> ObtenerEntrevistadosDisponiblesAsync(
        long trabajoId);

    /// <summary>
    /// Validar participantes seleccionados para una programación
    /// Reglas: existencia, disponibilidad, duplicados y estado futuro confirmado/ejecutado
    /// </summary>
    Task<(bool Success, List<ParticipanteValidacionVm> Data, string Error)> ValidarParticipantesAsync(
        long trabajoId, IEnumerable<long> idsParticipantes, DateTime? fechaProgramada = null);
}
