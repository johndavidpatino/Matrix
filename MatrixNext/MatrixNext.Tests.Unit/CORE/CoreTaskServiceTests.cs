using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.DTOs;
using Xunit;

namespace MatrixNext.Tests.Unit.CORE
{
    public class CoreTaskServiceTests
    {
        private MatrixDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<MatrixDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new MatrixDbContext(options);
        }

        [Fact]
        public async Task CreateTaskAsync_CreatesWorkFlow_WithRequiredFields()
        {
            // Arrange
            using var db = CreateDbContext();
            var logger = LoggerFactory.Create(builder => builder.AddDebug()).CreateLogger<CoreTaskService>();
            var service = new CoreTaskService(db, logger);

            var dto = new CreateTaskDto
            {
                IdTrabajo = 1001,
                IdTarea = 2002,
                IdTipoHilo = 1,
                Prioridad = 2,
                Observaciones = "Test",
                FechaVencimiento = DateTime.UtcNow.AddDays(7)
            };

            // Act
            var result = await service.CreateTaskAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.Id > 0);
            Assert.Equal(dto.IdTrabajo, result.IdTrabajo);
            Assert.Equal(dto.IdTarea, result.IdTarea);
            Assert.Equal(dto.IdTipoHilo, result.IdTipoHilo);
            Assert.Equal("Creada", result.Estado);
            Assert.Equal(dto.Prioridad, result.Prioridad);
        }
    }
}
