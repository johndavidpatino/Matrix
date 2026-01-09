using System.Data;
using System.IO;
using ClosedXML.Excel;
using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels.OP;
using MatrixNext.Web.Services.OP.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

public class OpIpsService : IOpIpsService
{
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpIpsService> _logger;
    private readonly IWebHostEnvironment _environment;
    private readonly IOpExportesAuditoriaService _exportAuditoriaService;

    public OpIpsService(
        MatrixDbContext dbContext,
        ILogger<OpIpsService> logger,
        IWebHostEnvironment environment,
        IOpExportesAuditoriaService exportAuditoriaService)
    {
        _dbContext = dbContext;
        _logger = logger;
        _environment = environment;
        _exportAuditoriaService = exportAuditoriaService;
    }

    public async Task<IpsRevisionViewModel> ObtenerRevisionesAsync(long? trabajoId, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var revisiones = (await connection.QueryAsync<IpsRevisionDto>(
            "OP_IPS_Revision_Get",
            new { ID = (long?)null, TrabajoID = trabajoId },
            commandType: CommandType.StoredProcedure))
            .Select(dto => new IpsRevisionRowViewModel
            {
                Id = dto.Id,
                TrabajoId = dto.TrabajoId,
                Trabajo = dto.JobBook ?? $"Trabajo {dto.TrabajoId}",
                Pregunta = dto.Pregunta,
                Observacion = dto.Observacion,
                DescripcionObservacion = dto.DescripcionObservacion,
                Instrumento = dto.Instrumento,
                Estado = dto.Estado,
                FechaHoraObservacion = dto.FechaHoraObservacion ?? DateTime.UtcNow
            })
            .ToList();

        return new IpsRevisionViewModel
        {
            TrabajoId = trabajoId,
            Revisiones = revisiones
        };
    }

    public async Task<bool> GuardarRevisionAsync(IpsRevisionUpdateModel model, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var result = await connection.ExecuteAsync(
            "OP_IPS_Revision_Edit",
            new
            {
                ID = model.Id,
                TrabajoId = model.TrabajoId,
                Pregunta = string.Empty,
                Observacion = model.Observacion,
                DescripcionObservacion = model.DescripcionObservacion,
                RespuestaProgramador = model.RespuestaProgramador,
                Rechazar = model.Rechazar,
                Estado = model.Estado,
                UsuarioProgramador = (long?)null,
                Instrumento = model.Instrumento,
                Version = string.Empty,
                Aplicativo = (int?)null,
                Proceso = (long?)null
            },
            commandType: CommandType.StoredProcedure);

        _logger.LogInformation("IPS {Id} actualizado", model.Id);
        return result > 0;
    }

    public async Task<IpsExportResult> ExportarRevisionesAsync(long? trabajoId, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync(cancellationToken);
            }

            var revisiones = (await connection.QueryAsync<IpsRevisionDto>(
                "OP_IPS_Revision_Get",
                new { ID = (long?)null, TrabajoID = trabajoId },
                commandType: CommandType.StoredProcedure)).ToList();

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("IPS");
            var table = worksheet.Cell(1, 1).InsertTable(revisiones.Select(r => new
            {
                r.TrabajoId,
                r.JobBook,
                r.Pregunta,
                r.Observacion,
                r.DescripcionObservacion,
                r.Estado,
                r.Instrumento,
                FechaHoraObservacion = r.FechaHoraObservacion?.ToString("s")
            }));

            worksheet.Columns().AdjustToContents();
            var filesRoot = Path.Combine(_environment.WebRootPath, "Files");
            Directory.CreateDirectory(filesRoot);

            var fileName = $"ips-export-{DateTime.UtcNow:yyyyMMddHHmmss}.xlsx";
            var filePath = Path.Combine(filesRoot, fileName);
            workbook.SaveAs(filePath);

            var relativePath = Path.GetRelativePath(_environment.WebRootPath, filePath).Replace(Path.DirectorySeparatorChar, '/');
            var publicPath = $"/{relativePath}";
            
            // Log export to audit table
            var tamanoBytes = new FileInfo(filePath).Length;
            await _exportAuditoriaService.RegistrarExportacionAsync(
                trabajoId ?? 0,
                "IPS",
                usuario: null, // TODO: Get current user ID from claims/session
                filePath,
                fileName,
                tamanoBytes);

            _logger.LogInformation("IPS export saved to {File}", filePath);
            return new IpsExportResult(filePath, publicPath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting IPS revisiones for trabajo {TrabajoId}", trabajoId);
            
            // Log error to audit table
            if (trabajoId.HasValue)
            {
                await _exportAuditoriaService.RegistrarErrorExportacionAsync(
                    trabajoId.Value,
                    "IPS",
                    usuario: null,
                    ex.Message);
            }

            throw;
        }
    }

    private sealed class IpsRevisionDto
    {
        public long Id { get; init; }
        public long TrabajoId { get; init; }
        public string? JobBook { get; init; }
        public string Pregunta { get; init; } = string.Empty;
        public string Observacion { get; init; } = string.Empty;
        public string DescripcionObservacion { get; init; } = string.Empty;
        public string Estado { get; init; } = string.Empty;
        public string Instrumento { get; init; } = string.Empty;
        public DateTime? FechaHoraObservacion { get; init; }
    }

    // ==================== NUEVOS MÉTODOS SPRINT 3 (OP-I01) ====================

    public async Task<(bool success, List<IpsRevisionVm> data, string error)> ObtenerRevisionesAsync(
        long? trabajoId, int? procesoId, string? metodo, string? userRol)
    {
        try
        {
            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            // Query SP OP_IPS_Procesos con filtros
            // Ref: IPSCuali.aspx.vb líneas 28-35 (SqlDataSource)
            var sql = @"
                SELECT 
                    r.Id,
                    r.IdProceso,
                    r.TrabajoId,
                    j.JobDesc AS TrabajoNombre,
                    r.ProcesoId,
                    p.Proceso AS ProcesoNombre,
                    r.TareaId,
                    r.TareaNombre,
                    r.EstadoWorkflow,
                    CASE r.EstadoWorkflow
                        WHEN 1 THEN 'Generado'
                        WHEN 2 THEN 'Notificado'
                        WHEN 3 THEN 'En Revisión'
                        WHEN 4 THEN 'Aprobado'
                        WHEN 5 THEN 'Rechazado'
                        ELSE 'Desconocido'
                    END AS EstadoWorkflowDescripcion,
                    r.FechaRevision,
                    r.RevisadoPor,
                    u.Nombre AS RevisadoPorNombre,
                    r.Observaciones,
                    r.RequiereAtencion,
                    r.FechaCreacion,
                    r.FechaModificacion
                FROM OP_IPS_Revisiones r
                LEFT JOIN PY_Trabajo j ON r.TrabajoId = j.IdJob
                LEFT JOIN OP_IPS_Procesos p ON r.ProcesoId = p.Id
                LEFT JOIN US_Usuarios u ON r.RevisadoPor = u.Id
                WHERE (@TrabajoId IS NULL OR r.TrabajoId = @TrabajoId)
                  AND (@ProcesoId IS NULL OR r.ProcesoId = @ProcesoId)
                  AND (@Metodo IS NULL OR j.MetodoRecoleccion = @Metodo)
                ORDER BY r.FechaCreacion DESC";

            var data = await connection.QueryAsync<IpsRevisionVm>(sql, new
            {
                TrabajoId = trabajoId,
                ProcesoId = procesoId,
                Metodo = metodo
            });

            return (true, data.AsList(), string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo revisiones IPS. Trabajo: {TrabajoId}", trabajoId);
            return (false, new List<IpsRevisionVm>(), "Error obteniendo revisiones IPS");
        }
    }

    public async Task<(bool success, List<ProcesoIpsVm> data, string error)> ObtenerProcesosAsync()
    {
        try
        {
            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            var sql = @"
                SELECT 
                    Id,
                    Proceso,
                    Descripcion,
                    Activo
                FROM OP_IPS_Procesos
                WHERE Activo = 1
                ORDER BY Proceso";

            var data = await connection.QueryAsync<ProcesoIpsVm>(sql);
            return (true, data.AsList(), string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo procesos IPS");
            return (false, new List<ProcesoIpsVm>(), "Error obteniendo procesos");
        }
    }

    public async Task<(bool success, string error)> NotificarProcesoAsync(long id, long usuarioId)
    {
        try
        {
            // Ref: IPSCuali.aspx.vb líneas 145-178 (btnNotificar_Click)
            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            var sql = @"
                UPDATE OP_IPS_Revisiones
                SET EstadoWorkflow = 2, -- Notificado
                    FechaRevision = GETDATE(),
                    RevisadoPor = @UsuarioId,
                    FechaModificacion = GETDATE()
                WHERE Id = @Id
                  AND EstadoWorkflow = 1"; // Solo si está en Generado

            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id, UsuarioId = usuarioId });

            if (rowsAffected == 0)
            {
                return (false, "Proceso no encontrado o ya fue notificado");
            }

            _logger.LogInformation("Proceso IPS {Id} notificado por usuario {UsuarioId}", id, usuarioId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notificando proceso IPS {Id}", id);
            return (false, "Error al notificar proceso");
        }
    }

    public async Task<(bool success, string error)> RechazarProcesoAsync(long id, long usuarioId, string observaciones)
    {
        try
        {
            // Ref: IPSCuali.aspx.vb líneas 180-215 (btnRechazar_Click)
            if (string.IsNullOrWhiteSpace(observaciones))
            {
                return (false, "Las observaciones son requeridas para rechazar");
            }

            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            var sql = @"
                UPDATE OP_IPS_Revisiones
                SET EstadoWorkflow = 5, -- Rechazado
                    FechaRevision = GETDATE(),
                    RevisadoPor = @UsuarioId,
                    Observaciones = @Observaciones,
                    FechaModificacion = GETDATE()
                WHERE Id = @Id
                  AND EstadoWorkflow IN (1, 2, 3)"; // Solo si no está Aprobado o Rechazado

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                Id = id,
                UsuarioId = usuarioId,
                Observaciones = observaciones
            });

            if (rowsAffected == 0)
            {
                return (false, "Proceso no encontrado o ya fue procesado");
            }

            _logger.LogInformation("Proceso IPS {Id} rechazado por usuario {UsuarioId}", id, usuarioId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rechazando proceso IPS {Id}", id);
            return (false, "Error al rechazar proceso");
        }
    }

    public async Task<(bool success, string error)> ActualizarEstadoAsync(
        long id, int nuevoEstado, long usuarioId, string? observaciones)
    {
        try
        {
            // Validar estado válido (1-5)
            if (nuevoEstado < 1 || nuevoEstado > 5)
            {
                return (false, "Estado inválido");
            }

            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            var sql = @"
                UPDATE OP_IPS_Revisiones
                SET EstadoWorkflow = @NuevoEstado,
                    FechaRevision = GETDATE(),
                    RevisadoPor = @UsuarioId,
                    Observaciones = COALESCE(@Observaciones, Observaciones),
                    FechaModificacion = GETDATE()
                WHERE Id = @Id";

            var rowsAffected = await connection.ExecuteAsync(sql, new
            {
                Id = id,
                NuevoEstado = nuevoEstado,
                UsuarioId = usuarioId,
                Observaciones = observaciones
            });

            if (rowsAffected == 0)
            {
                return (false, "Proceso no encontrado");
            }

            _logger.LogInformation("Proceso IPS {Id} actualizado a estado {Estado} por usuario {UsuarioId}",
                id, nuevoEstado, usuarioId);
            return (true, string.Empty);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error actualizando estado proceso IPS {Id}", id);
            return (false, "Error al actualizar estado");
        }
    }

    public async Task<byte[]> ExportarRevisionesExcelAsync(
        long? trabajoId, int? procesoId, string? metodo, string? userRol)
    {
        var (success, data, _) = await ObtenerRevisionesAsync(trabajoId, procesoId, metodo, userRol);

        if (!success || !data.Any())
        {
            return Array.Empty<byte>();
        }

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Revisiones IPS");

        // Headers
        worksheet.Cell(1, 1).Value = "ID";
        worksheet.Cell(1, 2).Value = "Trabajo";
        worksheet.Cell(1, 3).Value = "Proceso";
        worksheet.Cell(1, 4).Value = "Tarea";
        worksheet.Cell(1, 5).Value = "Estado";
        worksheet.Cell(1, 6).Value = "Fecha Revisión";
        worksheet.Cell(1, 7).Value = "Revisado Por";
        worksheet.Cell(1, 8).Value = "Observaciones";

        // Header style
        var headerRange = worksheet.Range(1, 1, 1, 8);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.LightBlue;

        // Data
        int row = 2;
        foreach (var revision in data)
        {
            worksheet.Cell(row, 1).Value = revision.Id;
            worksheet.Cell(row, 2).Value = revision.TrabajoNombre;
            worksheet.Cell(row, 3).Value = revision.ProcesoNombre;
            worksheet.Cell(row, 4).Value = revision.TareaNombre;
            worksheet.Cell(row, 5).Value = revision.EstadoWorkflowDescripcion;
            worksheet.Cell(row, 6).Value = revision.FechaRevision?.ToString("dd/MM/yyyy HH:mm") ?? "";
            worksheet.Cell(row, 7).Value = revision.RevisadoPorNombre ?? "";
            worksheet.Cell(row, 8).Value = revision.Observaciones ?? "";
            row++;
        }

        // Auto-fit columns
        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
