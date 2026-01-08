using System.Data;
using System.IO;
using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

/// <summary>
/// Implementation of export audit logging service
/// Tracks Excel exports for compliance and cleanup
/// Ref: S4-004
/// </summary>
public class OpExportesAuditoriaService : IOpExportesAuditoriaService
{
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpExportesAuditoriaService> _logger;

    public OpExportesAuditoriaService(MatrixDbContext dbContext, ILogger<OpExportesAuditoriaService> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<long> RegistrarExportacionAsync(
        long trabajoId,
        string tipo,
        long? usuario,
        string rutaArchivo,
        string nombreArchivo,
        long? tamanoBytes)
    {
        try
        {
            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            var idExporte = await connection.QueryFirstOrDefaultAsync<long>(
                @"INSERT INTO [dbo].[OP_ExportesAuditoria] 
                    ([TrabajoId], [Tipo], [Usuario], [RutaArchivo], [NombreArchivo], [TamanoBytes], [Exitoso], [FechaProgramadaLimpieza])
                  VALUES 
                    (@TrabajoId, @Tipo, @Usuario, @RutaArchivo, @NombreArchivo, @TamanoBytes, 1, DATEADD(DAY, 30, GETUTCDATE()))
                  SELECT CAST(SCOPE_IDENTITY() as bigint)",
                new
                {
                    TrabajoId = trabajoId,
                    Tipo = tipo,
                    Usuario = usuario,
                    RutaArchivo = rutaArchivo,
                    NombreArchivo = nombreArchivo,
                    TamanoBytes = tamanoBytes
                });

            _logger.LogInformation("Exportación registrada: IdExporte={IdExporte}, Tipo={Tipo}, Trabajo={TrabajoId}, Usuario={Usuario}",
                idExporte, tipo, trabajoId, usuario);

            return idExporte;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registrando exportación: Tipo={Tipo}, Trabajo={TrabajoId}", tipo, trabajoId);
            throw;
        }
    }

    public async Task<long> RegistrarErrorExportacionAsync(
        long trabajoId,
        string tipo,
        long? usuario,
        string mensajeError)
    {
        try
        {
            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            var idExporte = await connection.QueryFirstOrDefaultAsync<long>(
                @"INSERT INTO [dbo].[OP_ExportesAuditoria] 
                    ([TrabajoId], [Tipo], [Usuario], [RutaArchivo], [NombreArchivo], [Exitoso], [MensajeError])
                  VALUES 
                    (@TrabajoId, @Tipo, @Usuario, '', '', 0, @MensajeError)
                  SELECT CAST(SCOPE_IDENTITY() as bigint)",
                new
                {
                    TrabajoId = trabajoId,
                    Tipo = tipo,
                    Usuario = usuario,
                    MensajeError = mensajeError
                });

            _logger.LogWarning("Error de exportación registrado: IdExporte={IdExporte}, Tipo={Tipo}, Error={Error}",
                idExporte, tipo, mensajeError);

            return idExporte;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registrando fallo de exportación: Tipo={Tipo}", tipo);
            throw;
        }
    }

    public async Task<List<OpExportAuditoriaDto>> ObtenerExportacionesPorTrabajoAsync(long trabajoId)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var exportaciones = (await connection.QueryAsync<OpExportAuditoriaDto>(
            @"SELECT 
                [IdExporte], [TrabajoId], [Tipo], [Usuario], [FechaExportacion],
                [RutaArchivo], [NombreArchivo], [TamanoBytes], [Exitoso], [MensajeError],
                [FechaProgramadaLimpieza]
              FROM [dbo].[OP_ExportesAuditoria]
              WHERE [TrabajoId] = @TrabajoId
              ORDER BY [FechaExportacion] DESC",
            new { TrabajoId = trabajoId }))
            .ToList();

        return exportaciones;
    }

    public async Task<List<OpExportAuditoriaDto>> ObtenerExportacionesPorFechaAsync(DateTime desde, DateTime hasta)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var exportaciones = (await connection.QueryAsync<OpExportAuditoriaDto>(
            @"SELECT 
                [IdExporte], [TrabajoId], [Tipo], [Usuario], [FechaExportacion],
                [RutaArchivo], [NombreArchivo], [TamanoBytes], [Exitoso], [MensajeError],
                [FechaProgramadaLimpieza]
              FROM [dbo].[OP_ExportesAuditoria]
              WHERE [FechaExportacion] BETWEEN @Desde AND @Hasta
              ORDER BY [FechaExportacion] DESC",
            new { Desde = desde, Hasta = hasta }))
            .ToList();

        return exportaciones;
    }

    public async Task<List<OpExportAuditoriaDto>> ObtenerExportacionesPendienteLimpiezaAsync()
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var exportaciones = (await connection.QueryAsync<OpExportAuditoriaDto>(
            @"SELECT 
                [IdExporte], [TrabajoId], [Tipo], [Usuario], [FechaExportacion],
                [RutaArchivo], [NombreArchivo], [TamanoBytes], [Exitoso], [MensajeError],
                [FechaProgramadaLimpieza]
              FROM [dbo].[OP_ExportesAuditoria]
              WHERE [Limpiado] = 0 
                AND [Exitoso] = 1
                AND [FechaProgramadaLimpieza] <= GETUTCDATE()
              ORDER BY [FechaProgramadaLimpieza] ASC"))
            .ToList();

        return exportaciones;
    }

    public async Task<bool> LimpiarExportacionAsync(long idExporte)
    {
        try
        {
            await using var connection = _dbContext.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
            {
                await connection.OpenAsync();
            }

            // Get file path first
            var ruta = await connection.QueryFirstOrDefaultAsync<string>(
                "SELECT [RutaArchivo] FROM [dbo].[OP_ExportesAuditoria] WHERE [IdExporte] = @IdExporte",
                new { IdExporte = idExporte });

            if (!string.IsNullOrEmpty(ruta) && File.Exists(ruta))
            {
                try
                {
                    File.Delete(ruta);
                    _logger.LogInformation("Archivo de exportación eliminado: {RutaArchivo}", ruta);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error eliminando archivo: {RutaArchivo}", ruta);
                }
            }

            // Mark as cleaned in database
            var result = await connection.ExecuteAsync(
                @"UPDATE [dbo].[OP_ExportesAuditoria]
                  SET [Limpiado] = 1, [FechaLimpieza] = GETUTCDATE()
                  WHERE [IdExporte] = @IdExporte",
                new { IdExporte = idExporte });

            _logger.LogInformation("Exportación marcada como limpiada: IdExporte={IdExporte}", idExporte);
            return result > 0;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error limpiando exportación: IdExporte={IdExporte}", idExporte);
            throw;
        }
    }

    public async Task<int> LimpiarExportacionesAntiguasAsync(int diasRetension = 30)
    {
        try
        {
            var exportacionesPendientes = await ObtenerExportacionesPendienteLimpiezaAsync();

            int limpiadasCount = 0;
            foreach (var exportacion in exportacionesPendientes)
            {
                if (await LimpiarExportacionAsync(exportacion.IdExporte))
                {
                    limpiadasCount++;
                }
            }

            _logger.LogInformation("Limpieza completada: {Count} exportaciones eliminadas (retención: {Dias} días)",
                limpiadasCount, diasRetension);

            return limpiadasCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en limpieza de exportaciones antiguas");
            throw;
        }
    }

    public async Task<(int Total, int Exitosos, int Fallidos, long TamanoTotalBytes)> ObtenerEstadisticasAsync()
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync();
        }

        var stats = await connection.QueryFirstOrDefaultAsync<dynamic>(
            @"SELECT 
                COUNT(*) as Total,
                SUM(CASE WHEN [Exitoso] = 1 THEN 1 ELSE 0 END) as Exitosos,
                SUM(CASE WHEN [Exitoso] = 0 THEN 1 ELSE 0 END) as Fallidos,
                ISNULL(SUM([TamanoBytes]), 0) as TamanoTotalBytes
              FROM [dbo].[OP_ExportesAuditoria]");

        return (
            Total: (int)(stats?.Total ?? 0),
            Exitosos: (int)(stats?.Exitosos ?? 0),
            Fallidos: (int)(stats?.Fallidos ?? 0),
            TamanoTotalBytes: (long)(stats?.TamanoTotalBytes ?? 0)
        );
    }
}
