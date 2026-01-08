using System.Data;
using System.IO;
using ClosedXML.Excel;
using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels.OP;
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
}
