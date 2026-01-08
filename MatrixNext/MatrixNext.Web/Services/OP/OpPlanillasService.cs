using System.Data;
using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

public class OpPlanillasService : IOpPlanillasService
{
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpPlanillasService> _logger;

    private const string PlanillasSp = "OP_CuantiPlanillas_GET";

    public OpPlanillasService(MatrixDbContext dbContext, ILogger<OpPlanillasService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<PlanillasAprobacionViewModel> ObtenerPlanillasAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var planillas = (await connection.QueryAsync<PlanillaDto>(
            PlanillasSp,
            new
            {
                Revisado = (bool?)null,
                PMO = (long?)null,
                Fini = (DateTime?)null,
                Ffin = (DateTime?)null,
                TrabajoId = (long?)null,
                Coordinador = (long?)null
            },
            commandType: CommandType.StoredProcedure)).ToList();

        var total = planillas.Count;
        var aprobadas = planillas.Count(p => p.Revisado);
        var pendientes = total - aprobadas;
        var enRevision = planillas.Count(p => !p.Revisado && !string.IsNullOrWhiteSpace(p.UsuarioCarga));

        var tabs = new List<PlanillaStatusViewModel>
        {
            new()
            {
                Title = "Pendientes",
                Count = pendientes,
                Description = "Planillas cargadas que requieren validación del COE",
                Badge = "warning",
                TabId = "pendientes"
            },
            new()
            {
                Title = "En revisión",
                Count = enRevision,
                Description = "Planillas con comentarios y en manos de coordinadores",
                Badge = "primary",
                TabId = "revision"
            },
            new()
            {
                Title = "Aprobadas",
                Count = aprobadas,
                Description = "Planillas ya aprobadas por COE y PMO",
                Badge = "success",
                TabId = "aprobadas"
            }
        };

        var planillaRows = planillas
            .OrderByDescending(p => p.FechaCarga ?? DateTime.UtcNow)
            .Take(25)
            .Select(p => new PlanillaRowViewModel
            {
                TrabajoId = p.TrabajoId,
                TrabajoNombre = p.NombreTrabajo,
                Responsable = p.NombrePersona ?? p.PMO ?? "Sin asignar",
                Estado = p.Revisado ? "Aprobada" : "Pendiente",
                FechaCarga = p.FechaCarga ?? DateTime.UtcNow,
                Cantidad = p.Cantidad,
                Observaciones = p.TipoActividadDescripcion ?? string.Empty
            })
            .ToList();

        var totalIps = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM OP_IPS_Revision");
        var pendientesIps = await connection.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM OP_IPS_Revision WHERE Estado = 0");
        var ipsSummary = new IpsSummaryViewModel
        {
            Pendientes = pendientesIps,
            Atendidas = Math.Max(totalIps - pendientesIps, 0),
            UltimaActualizacion = DateTime.UtcNow.ToString("g"),
            Comentario = "Datos propios de OP_IPS_Revision"
        };

        _logger.LogDebug("Planillas: {Total}, aprobadas {Aprobadas}, IPS pendientes {PendientesIps}", total, aprobadas, pendientesIps);

        return new PlanillasAprobacionViewModel
        {
            StatusTabs = tabs,
            Planillas = planillaRows,
            Productivity = new ProductivitySummaryViewModel
            {
                Rol = "PMO",
                Corte = "Corte 16-15",
                TotalAprobadas = aprobadas,
                TotalPendientes = pendientes,
                Nota = "Basado en OP_CuantiPlanillas_GET"
            },
            Ips = ipsSummary
        };
    }

    private sealed class PlanillaDto
    {
        public long TrabajoId { get; init; }
        public string NombreTrabajo { get; init; } = string.Empty;
        public string? NombrePersona { get; init; }
        public string? PMO { get; init; }
        public string? UsuarioCarga { get; init; }
        public bool Revisado { get; init; }
        public DateTime? FechaCarga { get; init; }
        public int Cantidad { get; init; }
        public string? TipoActividadDescripcion { get; init; }
    }
}
