using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

public class OpProduccionService : IOpProduccionService
{
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpProduccionService> _logger;

    public OpProduccionService(MatrixDbContext dbContext, ILogger<OpProduccionService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<IReadOnlyList<UnidadDto>> ObtenerUnidadesAsync(long? identificacion, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var unidades = await connection.QueryAsync<UnidadDto>(
            "OP_UnidadesProduccionGet",
            new { identificacion },
            commandType: System.Data.CommandType.StoredProcedure);

        return unidades.ToList();
    }

    public async Task<IReadOnlyList<ActividadDto>> ObtenerActividadesAsync(int? unidad, int? actividad, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var actividades = await connection.QueryAsync<ActividadDto>(
            "OP_ActividadesProduccionGet",
            new { Unidadid = unidad, Actividad = actividad, SubActividad = (int?)null, Activa = true },
            commandType: System.Data.CommandType.StoredProcedure);

        return actividades.ToList();
    }

    public async Task<IReadOnlyList<JbeDto>> ObtenerJbeAsync(int tipo, string? busqueda, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var jbes = await connection.QueryAsync<JbeDto>(
            "OP_JBE_JBI_CC_Get",
            new { tipo, busqueda },
            commandType: System.Data.CommandType.StoredProcedure);

        return jbes.ToList();
    }

    public async Task<IReadOnlyList<ProduccionRowViewModel>> ObtenerProduccionAsync(DateTime? fechaInicio, DateTime? fechaFin, string? identificacion, int? unidad, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var registros = await connection.QueryAsync<ProduccionRecord>(
            "OP_Produccion_Get",
            new { fechaInicio, fechaFin, personaId = identificacion, id = (int?)null, unidad },
            commandType: System.Data.CommandType.StoredProcedure);

        return registros.Select(r => new ProduccionRowViewModel
        {
            Id = r.Id,
            Area = r.Area,
            Actividad = r.Actividad,
            SubActividad = r.SubActividad,
            Fecha = r.Fecha.HasValue ? r.Fecha.Value.ToString("dd/MM/yyyy") : string.Empty,
            HoraInicio = r.HoraInicio?.ToString(@"hh\:mm") ?? string.Empty,
            HoraFin = r.HoraFin?.ToString(@"hh\:mm") ?? string.Empty,
            CantidadGeneral = r.CantidadGeneral,
            CantidadEfectivas = r.CantidadEfectivas,
            EsReproceso = r.EsReproceso == "Si",
            Observacion = r.Observacion ?? string.Empty
        }).ToList();
    }

    public async Task<bool> GuardarRegistroAsync(GuardarRegistroRequest request, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != System.Data.ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var rows = await connection.ExecuteAsync(
            "OP_Produccion_Add",
            new
            {
                actividad = request.Actividad,
                subActividad = request.SubActividad,
                unidad = request.Unidad,
                trabajoId = request.TrabajoId,
                estudioId = (int?)null,
                fecha = request.Fecha,
                horaInicio = request.HoraInicio,
                horaFin = request.HoraFin,
                cantidad = request.CantidadGeneral,
                observacion = request.Observacion,
                estado = (int?)null,
                validadoPor = (long?)null,
                personaId = request.UsuarioId,
                esReproceso = request.EsReproceso,
                cantidadEfectivas = request.CantidadEfectivas,
                tipoReproceso = request.TipoReproceso,
                tipoAplicativoProceso = request.TipoAplicativoProceso,
                cantVarsScript = request.CantVarsScript,
                cantVarsExport = request.CantVarsExport
            },
            commandType: System.Data.CommandType.StoredProcedure);

        _logger.LogInformation("Registro de producción guardado para trabajo {TrabajoId}", request.TrabajoId);
        return rows > 0;
    }

    private sealed class ProduccionRecord
    {
        public int Id { get; init; }
        public string Area { get; init; } = string.Empty;
        public string Actividad { get; init; } = string.Empty;
        public string? SubActividad { get; init; }
        public DateTime? Fecha { get; init; }
        public TimeSpan? HoraInicio { get; init; }
        public TimeSpan? HoraFin { get; init; }
        public int? CantidadGeneral { get; init; }
        public int? CantidadEfectivas { get; init; }
        public string? EsReproceso { get; init; }
        public string? Observacion { get; init; }
    }
}
