using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.EQ;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace MatrixNext.Tests.Unit.EQ;

public class EqSeedServiceTests
{
    [Fact]
    public async Task SeedAllMasterTables_PopulatesAllTables_Successfully()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MatrixDbContext>()
            .UseInMemoryDatabase(databaseName: "EqSeedTest_" + Guid.NewGuid())
            .Options;

        using var context = new MatrixDbContext(options);
        var logger = Mock.Of<ILogger<EqSeedService>>();
        var seedService = new EqSeedService(context, logger);

        // Act
        var result = await seedService.SeedAllMasterTablesAsync();

        // Assert
        Assert.True(result.Success, $"Seed falló: {result.ErrorMessage}");
        Assert.Equal(6, result.TablasSeeded.Count); // 6 tablas maestras
        Assert.Empty(result.TablasSkipped);

        // Verify counts
        var status = await seedService.CheckMasterDataStatusAsync();
        Assert.True(status.AllTablesPopulated, "No todas las tablas fueron pobladas");
        
        // Verify specific counts
        Assert.True(status.PreciosCount > 0, "eq_param_precio no tiene datos");
        Assert.True(status.HorasCount > 0, "eq_param_script_proc no tiene datos");
        Assert.True(status.TarifasCount > 0, "eq_valor_hora_ops no tiene datos");
        Assert.True(status.CostosCount > 0, "eq_cost_insumos no tiene datos");
        Assert.True(status.RatesCount > 0, "eq_rate_estadistica no tiene datos");
        Assert.True(status.LocacionesCount > 0, "eq_locaciones no tiene datos");
        
        // Expected counts
        Assert.Equal(396, status.PreciosCount); // 3 metodologías × 11 penetraciones × 12 duraciones
        Assert.Equal(12, status.HorasCount); // 12 duraciones
        Assert.Equal(8, status.TarifasCount); // L1-L8
        Assert.Equal(6, status.CostosCount); // NSE 1-6
        Assert.Equal(21, status.RatesCount); // Servicios estadísticos
        Assert.Equal(16, status.LocacionesCount); // Ciudades
    }

    [Fact]
    public async Task SeedAllMasterTables_WhenAlreadyPopulated_SkipsSeeding()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MatrixDbContext>()
            .UseInMemoryDatabase(databaseName: "EqSeedSkipTest_" + Guid.NewGuid())
            .Options;

        using var context = new MatrixDbContext(options);
        var logger = Mock.Of<ILogger<EqSeedService>>();
        var seedService = new EqSeedService(context, logger);

        // First seed
        await seedService.SeedAllMasterTablesAsync();

        // Act - Second seed without force
        var result = await seedService.SeedAllMasterTablesAsync(force: false);

        // Assert
        Assert.True(result.Success);
        Assert.Empty(result.TablasSeeded); // Should skip all
        Assert.Equal(6, result.TablasSkipped.Count);
    }

    [Fact]
    public async Task SeedAllMasterTables_WithForce_Reseeds()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MatrixDbContext>()
            .UseInMemoryDatabase(databaseName: "EqSeedForceTest_" + Guid.NewGuid())
            .Options;

        using var context = new MatrixDbContext(options);
        var logger = Mock.Of<ILogger<EqSeedService>>();
        var seedService = new EqSeedService(context, logger);

        // First seed
        await seedService.SeedAllMasterTablesAsync();
        var firstCount = await context.EqParamPrecios.CountAsync();

        // Act - Force reseed
        var result = await seedService.SeedAllMasterTablesAsync(force: true);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(6, result.TablasSeeded.Count);
        
        // Should have duplicates now (force doesn't clear first)
        var secondCount = await context.EqParamPrecios.CountAsync();
        Assert.True(secondCount >= firstCount);
    }

    [Fact]
    public void EqSeedData_GetPreciosMatriz_ReturnsCorrectCount()
    {
        // Act
        var precios = EqSeedData.GetPreciosMatriz();

        // Assert
        Assert.NotEmpty(precios);
        Assert.Equal(396, precios.Count); // 3 metodologías × 11 penetraciones × 12 duraciones
        
        // Verify methodologies
        var metodologias = precios.Select(p => p.TipoMetodologia).Distinct().ToList();
        Assert.Contains("F2F", metodologias);
        Assert.Contains("CATI", metodologias);
        Assert.Contains("ONLINE", metodologias);
        Assert.Equal(3, metodologias.Count);
        
        // Verify penetrations
        var penetraciones = precios.Select(p => p.PenetracionRango).Distinct().Count();
        Assert.Equal(11, penetraciones);
        
        // Verify durations
        var duraciones = precios.Select(p => p.DuracionMin).Distinct().Count();
        Assert.Equal(12, duraciones);
    }

    [Fact]
    public void EqSeedData_GetTarifasRecursos_ReturnsCorrectLevels()
    {
        // Act
        var tarifas = EqSeedData.GetTarifasRecursos();

        // Assert
        Assert.Equal(8, tarifas.Count); // L1-L8
        
        var niveles = tarifas.Select(t => t.Nivel).ToList();
        Assert.Contains("L1", niveles);
        Assert.Contains("L8", niveles);
        
        // Verify rates increase with level
        var orderedByLevel = tarifas.OrderBy(t => t.Nivel).ToList();
        for (int i = 1; i < orderedByLevel.Count; i++)
        {
            Assert.True(orderedByLevel[i].BillingRate > orderedByLevel[i - 1].BillingRate, 
                $"Billing rate should increase from {orderedByLevel[i - 1].Nivel} to {orderedByLevel[i].Nivel}");
        }
    }

    [Fact]
    public void EqSeedData_GetCostosInsumos_ReturnsSixNSE()
    {
        // Act
        var costos = EqSeedData.GetCostosInsumos();

        // Assert
        Assert.Equal(6, costos.Count); // NSE 1-6
        
        var nses = costos.Select(c => c.NSE).OrderBy(n => n).ToList();
        Assert.Equal(new[] { 1, 2, 3, 4, 5, 6 }, nses);
        
        // Verify productividad increases with NSE (higher NSE = higher productivity)
        var orderedByNSE = costos.OrderBy(c => c.NSE).ToList();
        for (int i = 1; i < orderedByNSE.Count; i++)
        {
            Assert.True(orderedByNSE[i].Productividad >= orderedByNSE[i - 1].Productividad, 
                $"Productividad should increase from NSE {orderedByNSE[i - 1].NSE} to {orderedByNSE[i].NSE}");
        }
    }

    [Fact]
    public void EqSeedData_GetRatesEstadistica_ReturnsMultipleServices()
    {
        // Act
        var rates = EqSeedData.GetRatesEstadistica();

        // Assert
        Assert.True(rates.Count >= 20, "Should have at least 20 statistical services");
        
        var categorias = rates.Select(r => r.Categoria).Distinct().ToList();
        Assert.Contains("Procesos Especiales", categorias);
        
        // Verify all have prices
        Assert.All(rates, r => Assert.True(r.PrecioRef2024 > 0, "Price should be > 0"));
    }

    [Fact]
    public void EqSeedData_GetLocaciones_ReturnsMainCities()
    {
        // Act
        var locaciones = EqSeedData.GetLocaciones();

        // Assert
        Assert.True(locaciones.Count >= 15, "Should have at least 15 cities");
        
        var ciudades = locaciones.Select(l => l.Ciudad).ToList();
        Assert.Contains("Bogotá", ciudades);
        Assert.Contains("Medellín", ciudades);
        Assert.Contains("Cali", ciudades);
        Assert.Contains("Barranquilla", ciudades);
        
        // Verify all have tariffs
        Assert.All(locaciones, l => Assert.True(l.TarifaBase > 0, "TarifaBase should be > 0"));
        Assert.All(locaciones, l => Assert.True(l.TarifaConGross > l.TarifaBase, "TarifaConGross should be > TarifaBase"));
    }
}
