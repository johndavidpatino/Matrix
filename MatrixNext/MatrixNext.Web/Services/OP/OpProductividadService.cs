using System.Data;
using Dapper;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.ViewModels.OP;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.OP;

public class OpProductividadService : IOpProductividadService
{
    private readonly MatrixDbContext _dbContext;
    private readonly ILogger<OpProductividadService> _logger;

    public OpProductividadService(MatrixDbContext dbContext, ILogger<OpProductividadService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<ProductividadViewModel> ObtenerProductividadAsync(string rol, CancellationToken cancellationToken = default)
    {
        await using var connection = _dbContext.Database.GetDbConnection();
        if (connection.State != ConnectionState.Open)
        {
            await connection.OpenAsync(cancellationToken);
        }

        var registros = (await connection.QueryAsync<ProductividadRowDto>(
            "OP_CuantiProduccionProductividadTrabajos_GET",
            new { Revisado = (bool?)null, PMO = (long?)null, Fini = (DateTime?)null, Ffin = (DateTime?)null, TrabajoId = (long?)null },
            commandType: CommandType.StoredProcedure))
            .Take(20)
            .Select(dto => new ProductividadRowViewModel
            {
                TrabajoId = dto.TrabajoId,
                Trabajo = dto.NombreTrabajo,
                Ciudad = dto.Ciudad,
                Rol = dto.CargoMatrix ?? dto.Cargo.ToString(),
                Cantidad = dto.Cantidad,
                FechaEjecucion = dto.FechaEjecucion,
                Estado = dto.StatusPresupuesto ?? "Pendiente"
            })
            .ToList();

        _logger.LogDebug("Obtenidos {Count} registros de productividad para rol {Rol}", registros.Count, rol);

        return new ProductividadViewModel
        {
            RolActual = rol,
            RolesDisponibles = new[] { "PMO", "Coordinador", "Campo", "MyS/Call" },
            Registros = registros
        };
    }

    private sealed class ProductividadRowDto
    {
        public long TrabajoId { get; init; }
        public string NombreTrabajo { get; init; } = string.Empty;
        public string Ciudad { get; init; } = string.Empty;
        public string? CargoMatrix { get; init; }
        public int Cargo { get; init; }
        public int Cantidad { get; init; }
        public DateTime FechaEjecucion { get; init; }
        public string? StatusPresupuesto { get; init; }
    }
}
