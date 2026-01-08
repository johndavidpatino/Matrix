using System.Data;
using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

public class OpPresupuestosService : IOpPresupuestosService
{
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpPresupuestosService> _logger;

    public OpPresupuestosService(MatrixDbContext dbContext, ILogger<OpPresupuestosService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<SolicitudPresupuestoState?> ObtenerEstadoAsync(long trabajoId, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var registro = await connection.QueryFirstOrDefaultAsync<PresupuestoRecord>(
            "CC_SolicitudPresupuestoGet",
            new { TrabajoId = trabajoId },
            commandType: CommandType.StoredProcedure);

        if (registro == null)
        {
            return new SolicitudPresupuestoState(trabajoId, false, string.Empty);
        }

        return new SolicitudPresupuestoState(trabajoId, true, registro.Observacion ?? string.Empty);
    }

    public async Task<bool> GuardarSolicitudCompletaAsync(SolicitudPresupuestoCompletoRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        await connection.ExecuteAsync(
            "CC_SolicitudPresupuestoInternoAddMod",
            new { TrabajoId = request.TrabajoId },
            commandType: CommandType.StoredProcedure);

        const string deleteSql = "DELETE FROM dbo.CC_SolicitudPresupuesto WHERE TrabajoId = @TrabajoId";
        await connection.ExecuteAsync(deleteSql, new { request.TrabajoId });

        const string insertSql = @"
INSERT INTO dbo.CC_SolicitudPresupuesto
(TrabajoId, UsuarioId, Observacion, Muestra, Encuesta, Agendamiento, Jornada, Reclutamiento, NSE1y2, NSE3y4, NSE5y6, General, VrSugeridoContratista, Fecha)
VALUES
(@TrabajoId, @UsuarioId, @Observacion, @Muestra, @Encuesta, @Agendamiento, @Jornada, @Reclutamiento, @NSE1y2, @NSE3y4, @NSE5y6, @General, @VrSugeridoContratista, @Fecha);";

        var filas = await connection.ExecuteAsync(insertSql, new
        {
            request.TrabajoId,
            request.UsuarioId,
            Observacion = request.Observacion,
            request.Muestra,
            Encuesta = request.Encuesta ? 1 : 0,
            Agendamiento = request.Agendamiento ? 1 : 0,
            Jornada = request.Jornada ? 1 : 0,
            Reclutamiento = request.Reclutamiento ? 1 : 0,
            request.NSE1y2,
            request.NSE3y4,
            request.NSE5y6,
            request.General,
            VrSugeridoContratista = request.VrSugeridoContratista,
            Fecha = DateTime.UtcNow
        });

        _logger.LogInformation("Solicitud de presupuesto completo guardada para trabajo {TrabajoId} por usuario {UsuarioId}", request.TrabajoId, request.UsuarioId);
        return filas > 0;
    }

    public async Task<bool> GuardarSolicitudSimplificadaAsync(SolicitudPresupuestoSimplificadoRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        await connection.ExecuteAsync(
            "CC_SolicitudPresupuestoInternoAddMod",
            new { TrabajoId = request.TrabajoId },
            commandType: CommandType.StoredProcedure);

        const string deleteSql = "DELETE FROM dbo.CC_SolicitudPresupuesto WHERE TrabajoId = @TrabajoId";
        await connection.ExecuteAsync(deleteSql, new { request.TrabajoId });

        const string insertSql = @"
INSERT INTO dbo.CC_SolicitudPresupuesto
(TrabajoId, UsuarioId, Observacion, Fecha)
VALUES
(@TrabajoId, @UsuarioId, @Observacion, @Fecha);";

        var filas = await connection.ExecuteAsync(insertSql, new
        {
            request.TrabajoId,
            request.UsuarioId,
            Observacion = request.Observacion,
            Fecha = DateTime.UtcNow
        });

        _logger.LogInformation("Solicitud simplificada guardada para trabajo {TrabajoId}", request.TrabajoId);
        return filas > 0;
    }

    private sealed class PresupuestoRecord
    {
        public long Id { get; init; }
        public string? Observacion { get; init; }
    }
}
