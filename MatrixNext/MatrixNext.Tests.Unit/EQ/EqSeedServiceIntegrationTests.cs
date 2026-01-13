using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.EQ;
using Microsoft.Extensions.Logging;
using Moq;

namespace MatrixNext.Tests.Unit.EQ
{
    /// <summary>
    /// Test de integración para EqSeedService
    /// Valida que el seed se ejecuta correctamente y popula maestras
    /// </summary>
    public class EqSeedServiceIntegrationTests
    {
        private ILogger<EqSeedService> GetMockLogger()
        {
            // Crear un mock logger para tests
            var mockLogger = new Mock<ILogger<EqSeedService>>();
            return mockLogger.Object;
        }
        /// <summary>
        /// Test 1: EqSeedService carga todos los maestros correctamente
        /// </summary>
        [Fact]
        public async Task SeedService_SeedAllMasterTablesAsync_PopulatesAllTables()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<MatrixDbContext>()
                .UseInMemoryDatabase(databaseName: $"SeedTest_{Guid.NewGuid()}")
                .Options;

            using (var context = new MatrixDbContext(dbOptions))
            {
                var seedService = new EqSeedService(context, GetMockLogger());

                // Act
                var result = await seedService.SeedAllMasterTablesAsync(force: true);

                // Assert
                Assert.True(result.Success);
                Assert.Equal(6, result.TablasSeeded.Count); // 6 maestro tables
                Assert.Empty(result.TablasSkipped ?? new List<string>());
                
                // Verificar que se sembraron datos en todas las tablas
                Assert.True(context.EqParamPrecios.Any(), "EqParamPrecios should be populated");
                Assert.True(context.EqParamScriptProcs.Any(), "EqParamScriptProcs should be populated");
                Assert.True(context.EqValorHoraOps.Any(), "EqValorHoraOps should be populated");
                Assert.True(context.EqCostInsumos.Any(), "EqCostInsumos should be populated");
                Assert.True(context.EqRateEstadisticas.Any(), "EqRateEstadisticas should be populated");
                Assert.True(context.EqLocaciones.Any(), "EqLocaciones should be populated");
            }
        }

        /// <summary>
        /// Test 2: EqSeedService respeta el flag force=false (no re-siembra si existe datos)
        /// </summary>
        [Fact]
        public async Task SeedService_WithForceFalse_SkipsSeedingIfDataExists()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<MatrixDbContext>()
                .UseInMemoryDatabase(databaseName: $"SeedTest_{Guid.NewGuid()}")
                .Options;

            using (var context = new MatrixDbContext(dbOptions))
            {
                var seedService = new EqSeedService(context, GetMockLogger());

                // Act - First seed con force=true
                var result1 = await seedService.SeedAllMasterTablesAsync(force: true);
                var countAfterFirst = context.EqParamPrecios.Count();

                // Act - Second seed con force=false (debe skipear)
                var result2 = await seedService.SeedAllMasterTablesAsync(force: false);
                var countAfterSecond = context.EqParamPrecios.Count();

                // Assert
                Assert.True(result1.Success);
                Assert.True(result2.Success);
                Assert.Equal(6, result1.TablasSeeded.Count);
                Assert.Equal(0, result2.TablasSeeded.Count); // Segunda llamada debe skipear
                Assert.Equal(6, result2.TablasSkipped.Count); // Todas las 6 tablas fueron skippeadas
                Assert.Equal(countAfterFirst, countAfterSecond); // El count es igual
            }
        }

        /// <summary>
        /// Test 3: EqSeedService.CheckMasterDataStatusAsync retorna estado correcto
        /// </summary>
        [Fact]
        public async Task SeedService_CheckMasterDataStatusAsync_ReturnsCorrectStatus()
        {
            // Arrange
            var dbOptions = new DbContextOptionsBuilder<MatrixDbContext>()
                .UseInMemoryDatabase(databaseName: $"SeedTest_{Guid.NewGuid()}")
                .Options;

            using (var context = new MatrixDbContext(dbOptions))
            {
                var seedService = new EqSeedService(context, GetMockLogger());

                // Act - Seed data first
                await seedService.SeedAllMasterTablesAsync(force: true);
                
                // Act - Check status
                var status = await seedService.CheckMasterDataStatusAsync();

                // Assert
                Assert.NotNull(status);
                Assert.True(status.AllTablesPopulated, "All maestro tables should be populated");
                Assert.True(status.PreciosCount > 0);
                Assert.True(status.HorasCount > 0);
                Assert.True(status.TarifasCount > 0);
                Assert.True(status.CostosCount > 0);
                Assert.True(status.RatesCount > 0);
                Assert.True(status.LocacionesCount > 0);
            }
        }
    }
}
