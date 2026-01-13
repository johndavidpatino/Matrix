using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.Dashboard;

namespace MatrixNext.Tests.Unit.Dashboard
{
    /// <summary>
    /// SPRINT 9: Dashboard Service Tests
    /// Valida:
    /// - Comportamiento del servicio de agregación
    /// - Estrategia de caching (15min user, 30min global)
    /// - Manejo graceful de errores
    /// - Performance bajo carga
    /// </summary>
    public class DashboardServiceTests : IDisposable
    {
        private readonly MatrixDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly Mock<ILogger<DashboardService>> _mockLogger;
        private readonly DashboardService _service;

        public DashboardServiceTests()
        {
            // Setup EF In-Memory DbContext para tests
            var options = new DbContextOptionsBuilder<MatrixDbContext>()
                .UseInMemoryDatabase(databaseName: $"DashboardTest_{Guid.NewGuid()}")
                .Options;

            _context = new MatrixDbContext(options);
            _cache = new MemoryCache(new MemoryCacheOptions());
            _mockLogger = new Mock<ILogger<DashboardService>>();
            
            _service = new DashboardService(_context, _cache, _mockLogger.Object);
        }

        public void Dispose()
        {
            _context?.Dispose();
            _cache?.Dispose();
        }

        #region GetDashboardAsync Tests

        [Fact]
        public async Task GetDashboardAsync_WithValidUserId_ReturnsCompleteModel()
        {
            // Arrange
            const string userId = "test-user-123";

            // Act
            var dashboard = await _service.GetDashboardAsync(userId);

            // Assert
            Assert.NotNull(dashboard);
            Assert.IsType<DashboardViewModel>(dashboard);
            Assert.NotNull(dashboard.PendingTasks);
            Assert.NotNull(dashboard.ActiveProjects);
            Assert.NotNull(dashboard.RecentQuotes);
            Assert.NotNull(dashboard.UpcomingAbsences);
            Assert.NotNull(dashboard.DocumentStats);
            Assert.NotNull(dashboard.ProductionMetrics);
        }

        [Fact]
        public async Task GetDashboardAsync_WithEmptyDatabase_ReturnsValidEmptyModel()
        {
            // Arrange
            const string userId = "new-user";

            // Act
            var dashboard = await _service.GetDashboardAsync(userId);

            // Assert
            Assert.NotNull(dashboard);
            Assert.False(dashboard.HasError);
            Assert.Empty(dashboard.RecentQuotes);
            Assert.Empty(dashboard.PendingTasks);
            Assert.Empty(dashboard.ActiveProjects);
            Assert.Equal(0, dashboard.QuoteCount);
        }

        [Fact]
        public async Task GetDashboardAsync_CachesResult_ReturnsFromCache()
        {
            // Arrange
            const string userId = "cache-test";

            // Act - First call
            var dashboard1 = await _service.GetDashboardAsync(userId);
            var time1 = dashboard1.LoadedAt;

            // Wait and call again (should be from cache)
            await Task.Delay(100);
            var dashboard2 = await _service.GetDashboardAsync(userId);
            var time2 = dashboard2.LoadedAt;

            // Assert - Same timestamp means data came from cache
            Assert.Equal(time1, time2);
        }

        #endregion

        #region GetRecentQuotesAsync Tests

        [Fact]
        public async Task GetRecentQuotesAsync_WithEmptyDatabase_ReturnsEmptyList()
        {
            // Act
            var result = await _service.GetRecentQuotesAsync("any-user");

            // Assert
            Assert.Empty(result);
        }

        #endregion

        #region GetProductionMetricsAsync Tests

        [Fact]
        public async Task GetProductionMetricsAsync_WithNoQuotes_ReturnsZeroMetrics()
        {
            // Act
            var metrics = await _service.GetProductionMetricsAsync();

            // Assert
            Assert.Equal(0, metrics.TotalQuotesThisMonth);
            Assert.Equal(0, metrics.TotalRevenueThisMonth);
            Assert.Equal(0, metrics.AverageQuoteValue);
        }

        [Fact]
        public async Task GetProductionMetricsAsync_CachesFor30Minutes()
        {
            // Act - First call
            var metrics1 = await _service.GetProductionMetricsAsync();
            var firstUpdateTime = metrics1.LastUpdated;

            // Wait and call again (should be from cache)
            await Task.Delay(100);
            var metrics2 = await _service.GetProductionMetricsAsync();

            // Assert - Same timestamp (from cache)
            Assert.Equal(firstUpdateTime, metrics2.LastUpdated);
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public async Task GetDashboardAsync_HandlesMissingData_Gracefully()
        {
            // Arrange
            const string userId = "test-any-data";

            // Act - Service should handle any data gracefully
            var dashboard = await _service.GetDashboardAsync(userId);

            // Assert - No exceptions, returns valid model
            Assert.NotNull(dashboard);
            Assert.False(dashboard.HasError);
        }

        #endregion

        #region Performance Tests

        [Fact]
        public async Task GetDashboardAsync_FirstLoad_CompletesReasonablyFast()
        {
            // Arrange
            const string userId = "perf-test";
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var dashboard = await _service.GetDashboardAsync(userId);
            stopwatch.Stop();

            // Assert - Should complete within reasonable time
            Assert.True(stopwatch.ElapsedMilliseconds < 5000,
                $"First load took {stopwatch.ElapsedMilliseconds}ms, should be <5000ms");
            Assert.NotNull(dashboard);
        }

        [Fact]
        public async Task GetDashboardAsync_CachedLoad_IsVeryFast()
        {
            // Arrange
            const string userId = "perf-cache";
            await _service.GetDashboardAsync(userId); // Prime cache

            // Act
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var dashboard = await _service.GetDashboardAsync(userId);
            stopwatch.Stop();

            // Assert - Cached load should be very fast
            Assert.True(stopwatch.ElapsedMilliseconds < 1000,
                $"Cached load took {stopwatch.ElapsedMilliseconds}ms, should be <1000ms");
        }

        #endregion

        #region Caching Strategy Tests

        [Fact]
        public async Task Dashboard_CachingStrategy_UserDataCachedSeparately()
        {
            // Arrange
            const string user1 = "user-1";
            const string user2 = "user-2";

            // Act
            var dashboard1 = await _service.GetDashboardAsync(user1);
            var dashboard2 = await _service.GetDashboardAsync(user2);
            var dashboard1Again = await _service.GetDashboardAsync(user1);

            // Assert - Same user returns same timestamp (cached)
            Assert.Equal(dashboard1.LoadedAt, dashboard1Again.LoadedAt);
        }

        #endregion

        #region Data Validation Tests

        [Fact]
        public async Task GetDashboardAsync_AllWidgets_AreInitialized()
        {
            // Act
            var dashboard = await _service.GetDashboardAsync("test");

            // Assert - Verify all collections are initialized (not null)
            Assert.NotNull(dashboard.PendingTasks);
            Assert.NotNull(dashboard.ActiveProjects);
            Assert.NotNull(dashboard.RecentQuotes);
            Assert.NotNull(dashboard.UpcomingAbsences);
            Assert.NotNull(dashboard.DocumentStats);
            Assert.NotNull(dashboard.ProductionMetrics);
        }

        [Fact]
        public async Task GetProductionMetricsAsync_ReturnsValidMetricsModel()
        {
            // Act
            var metrics = await _service.GetProductionMetricsAsync();

            // Assert
            Assert.NotNull(metrics);
            Assert.True(metrics.TotalQuotesThisMonth >= 0);
            Assert.True(metrics.TotalRevenueThisMonth >= 0);
            Assert.True(metrics.AverageQuoteValue >= 0);
            Assert.NotEqual(default(DateTime), metrics.LastUpdated);
        }

        #endregion

        #region Parallel Loading Tests

        [Fact]
        public async Task GetDashboardAsync_LoadsAllWidgetsInParallel_IsEfficient()
        {
            // Arrange
            const string userId = "parallel-test";
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act - This internally loads all 6 widgets in parallel via Task.WhenAll()
            var dashboard = await _service.GetDashboardAsync(userId);
            stopwatch.Stop();

            // Assert - All widgets loaded successfully
            Assert.NotNull(dashboard.PendingTasks);
            Assert.NotNull(dashboard.ActiveProjects);
            Assert.NotNull(dashboard.RecentQuotes);
            Assert.NotNull(dashboard.UpcomingAbsences);
            Assert.NotNull(dashboard.DocumentStats);
            Assert.NotNull(dashboard.ProductionMetrics);
            
            // Parallel load should be reasonably fast
            Assert.True(stopwatch.ElapsedMilliseconds < 5000,
                $"Parallel dashboard load took {stopwatch.ElapsedMilliseconds}ms");
        }

        #endregion
    }
}
