using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.Services.Shared;
using MatrixNext.Web.Services;
using Microsoft.Data.SqlClient;
using System.Data;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Implementación del servicio de programación de campo cualitativo
/// Ref: ProgramacionCampo.aspx.vb (822 LOC)
/// Strategy: Dapper para consultas complejas y SPs
/// </summary>
public class OpProgramacionService : IOpProgramacionService
{
    private readonly MatrixDbContext _context;
    private readonly string _connectionString;
    private readonly ILogger<OpProgramacionService> _logger;
    private readonly IExportService _exportService;
    private readonly IEmailQueueService _emailQueueService;

    public OpProgramacionService(
        MatrixDbContext context,
        IConfiguration configuration,
        ILogger<OpProgramacionService> logger,
        IExportService exportService,
        IEmailQueueService emailQueueService)
    {
        _context = context;
        _connectionString = configuration.GetConnectionString("MatrixDb")!;
        _logger = logger;
        _exportService = exportService;
        _emailQueueService = emailQueueService;
    }

    public async Task<(bool Success, List<ProgramacionCampoVm> Data, string Error)> ObtenerProgramacionesPorTrabajoAsync(
        long trabajoId, string? estado = null)
    {
        try
        {
            // Ref: ProgramacionCampo.aspx.vb líneas 45-89 (Page_Load, CargarProgramaciones)
            using var connection = new SqlConnection(_connectionString);

            var programaciones = await connection.QueryAsync<ProgramacionCampoVm>(
                @"SELECT 
                    p.Id,
                    p.TrabajoId,
                    t.NombreTrabajo AS TrabajoNombre,
                    p.EntrevistadoId,
                    COALESCE(e.Nombres + ' ' + e.Apellidos, '') AS EntrevistadoNombre,
                    e.Telefono AS EntrevistadoTelefono,
                    e.Direccion AS EntrevistadoDireccion,
                    p.Estado,
                    est.Descripcion AS EstadoDescripcion,
                    p.FechaProgramada,
                    p.HoraProgramada,
                    p.LugarCita,
                    p.DireccionCita,
                    p.EntrevistadorAsignadoId,
                    COALESCE(ent.Nombres + ' ' + ent.Apellidos, '') AS EntrevistadorAsignadoNombre,
                    p.Observaciones,
                    p.FechaCreacion,
                    p.CreadoPor,
                    p.FechaModificacion,
                    p.ModificadoPor
                  FROM OP_Programados_Entrevistados p
                  INNER JOIN PY_Trabajo t ON p.TrabajoId = t.Id
                  LEFT JOIN OP_MuestraTrabajos e ON p.EntrevistadoId = e.Id
                  LEFT JOIN OP_EstadosProgramacion est ON p.Estado = est.Id
                  LEFT JOIN US_Usuarios ent ON p.EntrevistadorAsignadoId = ent.id
                  WHERE p.TrabajoId = @TrabajoId
                    AND (@Estado IS NULL OR est.Descripcion = @Estado)
                  ORDER BY p.FechaProgramada DESC, p.Id DESC",
                new { TrabajoId = trabajoId, Estado = estado });

            // Mapear alias utilizados por las vistas
            var list = programaciones.ToList();
            foreach (var p in list)
            {
                p.ProgramacionId = p.Id;
                p.NombreTrabajo = string.IsNullOrEmpty(p.NombreTrabajo) ? p.TrabajoNombre : p.NombreTrabajo;
                p.NombreEntrevistado = string.IsNullOrEmpty(p.NombreEntrevistado) ? p.EntrevistadoNombre : p.NombreEntrevistado;
                p.EstadoId = p.EstadoId > 0 ? p.EstadoId : p.Estado;
                p.NombreEstado = string.IsNullOrEmpty(p.NombreEstado) ? p.EstadoDescripcion : p.NombreEstado;
            }

            return (true, list, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo programaciones trabajo {TrabajoId}", trabajoId);
            return (false, new List<ProgramacionCampoVm>(), ex.Message);
        }
    }

    public async Task<(bool Success, long ProgramacionId, string Error)> GuardarProgramacionAsync(
        ProgramacionCampoVm programacion, long usuarioId)
    {
        try
        {
            // Ref: ProgramacionCampo.aspx.vb líneas 125-214 (btnSaveProgramar_Click)
            // NOTA: OP_Programados_Entrevistados tabla no existe (solo existe SP de lectura)
            // Guardar programaciones requiere migración adicional de tabla base
            _logger.LogWarning("Guardar programación no disponible - tabla base no migrada");
            return (false, 0, "Funcionalidad de guardar programación no disponible - requerida migración DB");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error guardando programación");
            return (false, 0, ex.Message);
        }
    }

    public async Task<(bool Success, string Error)> CambiarEstadoProgramacionAsync(
        long programacionId, int nuevoEstado, long usuarioId, string? observaciones = null)
    {
        try
        {
            // Ref: ProgramacionCampo.aspx.vb líneas 320-365 (CambiarEstado)
            // NOTA: OP_Programados_Entrevistados tabla no existe para escribir
            _logger.LogWarning("Cambiar estado programación no disponible - tabla base no migrada");
            return (false, "Funcionalidad no disponible");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cambiando estado programación {ProgramacionId}", programacionId);
            return (false, ex.Message);
        }
    }

    public async Task<(bool Success, byte[] Data, string Error)> ExportarProgramacionesExcelAsync(
        long trabajoId, string? estado = null)
    {
        try
        {
            // Ref: ProgramacionCampo.aspx.vb líneas 520-618 (ExportarExcel con ClosedXML)
            var (success, programaciones, error) = await ObtenerProgramacionesPorTrabajoAsync(trabajoId, estado);

            if (!success)
                return (false, Array.Empty<byte>(), error);

            var data = programaciones.Select(p => new Dictionary<string, object?>
            {
                ["ID"] = p.ProgramacionId > 0 ? p.ProgramacionId : p.Id,
                ["Entrevistado"] = string.IsNullOrEmpty(p.NombreEntrevistado) ? p.EntrevistadoNombre : p.NombreEntrevistado,
                ["Teléfono"] = p.EntrevistadoTelefono,
                ["Dirección"] = p.EntrevistadoDireccion,
                ["Estado"] = string.IsNullOrEmpty(p.NombreEstado) ? p.EstadoDescripcion : p.NombreEstado,
                ["Fecha Programada"] = p.FechaProgramada?.ToString("dd/MM/yyyy"),
                ["Hora"] = p.HoraProgramada?.ToString(@"hh\:mm"),
                ["Lugar"] = p.LugarCita,
                ["Entrevistador"] = p.EntrevistadorAsignadoNombre,
                ["Observaciones"] = p.Observaciones
            }).ToList();

            var excelBytes = await _exportService.ExportarExcelAsync(
                data,
                $"Programaciones_Trabajo_{trabajoId}",
                "Programaciones");

            return (true, excelBytes, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exportando programaciones trabajo {TrabajoId}", trabajoId);
            return (false, Array.Empty<byte>(), ex.Message);
        }
    }

    public async Task<(bool Success, List<EntrevistadoDisponibleVm> Data, string Error)> ObtenerEntrevistadosDisponiblesAsync(
        long trabajoId)
    {
        try
        {
            // Ref: ProgramacionCampo.aspx.vb líneas 220-287 (CargarEntrevistados)
            using var connection = new SqlConnection(_connectionString);

            var entrevistados = await connection.QueryAsync<EntrevistadoDisponibleVm>(
                @"SELECT 
                    m.Id,
                    COALESCE(m.Nombres + ' ' + m.Apellidos, '') AS NombreCompleto,
                    m.Telefono,
                    m.Direccion,
                    c.Ciudad,
                    CASE WHEN EXISTS(
                        SELECT 1 FROM OP_Programados_Entrevistados p
                        WHERE p.EntrevistadoId = m.Id
                          AND p.Estado IN (3, 4) -- Confirmado o Ejecutado
                          AND p.FechaProgramada >= GETDATE()
                    ) THEN 0 ELSE 1 END AS EstaDisponible,
                    (SELECT COUNT(*) FROM OP_Programados_Entrevistados p WHERE p.EntrevistadoId = m.Id) AS CantidadProgramaciones,
                    (SELECT MAX(p.FechaProgramada) FROM OP_Programados_Entrevistados p WHERE p.EntrevistadoId = m.Id) AS UltimaProgramacion
                  FROM OP_MuestraTrabajos m
                  LEFT JOIN C_Ciudades c ON m.CiudadId = c.id
                  WHERE m.TrabajoId = @TrabajoId
                  ORDER BY m.Id",
                new { TrabajoId = trabajoId });

            return (true, entrevistados.ToList(), string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo entrevistados disponibles trabajo {TrabajoId}", trabajoId);
            return (false, new List<EntrevistadoDisponibleVm>(), ex.Message);
        }
    }

    public async Task<(bool Success, List<ParticipanteValidacionVm> Data, string Error)> ValidarParticipantesAsync(
        long trabajoId, IEnumerable<long> idsParticipantes, DateTime? fechaProgramada = null)
    {
        try
        {
            using var connection = new SqlConnection(_connectionString);

            // Obtener info base de los participantes seleccionados
            var participantes = (await connection.QueryAsync<EntrevistadoDisponibleVm>(
                @"SELECT 
                    m.Id,
                    COALESCE(m.Nombres + ' ' + m.Apellidos, '') AS NombreCompleto,
                    m.Telefono,
                    m.Direccion,
                    c.Ciudad,
                    CASE WHEN EXISTS(
                        SELECT 1 FROM OP_Programados_Entrevistados p
                        WHERE p.EntrevistadoId = m.Id
                          AND p.Estado IN (3, 4) -- Confirmado o Ejecutado
                          AND (
                               p.FechaProgramada >= GETDATE()
                               OR (@FechaProgramada IS NOT NULL AND CAST(p.FechaProgramada AS DATE) = CAST(@FechaProgramada AS DATE))
                          )
                    ) THEN 0 ELSE 1 END AS EstaDisponible,
                    (SELECT COUNT(*) FROM OP_Programados_Entrevistados p WHERE p.EntrevistadoId = m.Id) AS CantidadProgramaciones,
                    (SELECT MAX(p.FechaProgramada) FROM OP_Programados_Entrevistados p WHERE p.EntrevistadoId = m.Id) AS UltimaProgramacion
                  FROM OP_MuestraTrabajos m
                  LEFT JOIN C_Ciudades c ON m.CiudadId = c.id
                  WHERE m.TrabajoId = @TrabajoId AND m.Id IN @Ids
                  ORDER BY m.Id",
                new { TrabajoId = trabajoId, Ids = idsParticipantes, FechaProgramada = fechaProgramada }))
                .ToList();

            // Armar resultado de validación
            var resultados = new List<ParticipanteValidacionVm>();

            var idsSet = new HashSet<long>(idsParticipantes);
            foreach (var p in participantes)
            {
                var disponible = p.EstaDisponible;
                string? motivo = null;

                if (!idsSet.Contains(p.Id))
                {
                    disponible = false;
                    motivo = "Participante no incluido en la selección";
                }
                else if (!disponible)
                {
                    motivo = "Ya programado (confirmado/ejecutado) en fecha futura o misma fecha";
                }

                resultados.Add(new ParticipanteValidacionVm
                {
                    ParticipanteId = p.Id,
                    NombreCompleto = p.NombreCompleto,
                    Disponible = disponible,
                    MotivoNoValido = motivo,
                    ProgramacionesPrevias = p.CantidadProgramaciones,
                    UltimaProgramacion = p.UltimaProgramacion
                });
            }

            // Detectar duplicados en la lista seleccionada
            var duplicados = idsParticipantes
                .GroupBy(id => id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToHashSet();

            foreach (var dupId in duplicados)
            {
                var item = resultados.FirstOrDefault(r => r.ParticipanteId == dupId);
                if (item != null)
                {
                    item.Disponible = false;
                    item.MotivoNoValido = string.IsNullOrEmpty(item.MotivoNoValido)
                        ? "Participante duplicado en selección"
                        : item.MotivoNoValido + "; Participante duplicado";
                }
                else
                {
                    resultados.Add(new ParticipanteValidacionVm
                    {
                        ParticipanteId = dupId,
                        NombreCompleto = string.Empty,
                        Disponible = false,
                        MotivoNoValido = "Participante duplicado en selección",
                        ProgramacionesPrevias = 0,
                        UltimaProgramacion = null
                    });
                }
            }

            // Si algún participante de ids no existe en la tabla, agregamos entrada no válida
            var existentes = participantes.Select(p => p.Id).ToHashSet();
            foreach (var idSel in idsSet)
            {
                if (!existentes.Contains(idSel))
                {
                    resultados.Add(new ParticipanteValidacionVm
                    {
                        ParticipanteId = idSel,
                        NombreCompleto = string.Empty,
                        Disponible = false,
                        MotivoNoValido = "Participante no existe para el trabajo",
                        ProgramacionesPrevias = 0,
                        UltimaProgramacion = null
                    });
                }
            }

            return (true, resultados, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando participantes trabajo {TrabajoId}", trabajoId);
            return (false, new List<ParticipanteValidacionVm>(), ex.Message);
        }
    }
}
