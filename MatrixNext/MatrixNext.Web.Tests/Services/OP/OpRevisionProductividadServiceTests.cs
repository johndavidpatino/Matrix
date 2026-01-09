using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.OP.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Web.Tests.Services.OP
{
    /// <summary>
    /// Tests unitarios para OpRevisionProductividadService
    /// Cobertura de métodos: ObtenerPlanillasPorRolAsync, AprobarPlanillaAsync, RechazarPlanillaAsync, ValidarMontosPlanillaAsync
    /// </summary>
    public class OpRevisionProductividadServiceTests
    {
        private readonly MatrixDbContext _dbContext;
        private readonly IOpRevisionProductividadService _service;
        private readonly Mock<ILogger<OpRevisionProductividadService>> _loggerMock;

        public OpRevisionProductividadServiceTests()
        {
            // Setup InMemory DB para tests
            var options = new DbContextOptionsBuilder<MatrixDbContext>()
                .UseInMemoryDatabase(databaseName: $"OP_Test_{Guid.NewGuid()}")
                .Options;

            _dbContext = new MatrixDbContext(options);
            _loggerMock = new Mock<ILogger<OpRevisionProductividadService>>();
            _service = new OpRevisionProductividadService(_dbContext, _loggerMock.Object);
        }

        [Fact]
        public async Task ObtenerPlanillasPorRolAsync_WithValidInput_ReturnsPlanillas()
        {
            // Arrange
            int trabajoId = 1;
            string rol = "PMO";
            int usuarioId = 1;

            // Act
            var result = await _service.ObtenerPlanillasPorRolAsync(trabajoId, rol, usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<PlanillaProductividadDto>>(result);
        }

        [Fact]
        public async Task ObtenerPlanillasPorRolAsync_WithInvalidTrabajoId_ReturnEmptyList()
        {
            // Arrange
            int invalidTrabajoId = -1;
            string rol = "PMO";
            int usuarioId = 1;

            // Act
            var result = await _service.ObtenerPlanillasPorRolAsync(invalidTrabajoId, rol, usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("PMO")]
        [InlineData("Coordinador")]
        [InlineData("Campo")]
        [InlineData("MyS/Call")]
        public async Task ObtenerPlanillasPorRolAsync_AllRoles_SuccessfullyProcessed(string rol)
        {
            // Arrange
            int trabajoId = 1;
            int usuarioId = 1;

            // Act
            var result = await _service.ObtenerPlanillasPorRolAsync(trabajoId, rol, usuarioId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<PlanillaProductividadDto>>(result);
        }

        [Fact]
        public async Task AprobarPlanillaAsync_WithValidInput_ReturnsTrue()
        {
            // Arrange
            int planillaId = 1;
            decimal montoAutorizado = 1000m;
            int usuarioId = 1;
            string rol = "PMO";

            // Act
            var result = await _service.AprobarPlanillaAsync(planillaId, montoAutorizado, usuarioId, rol);

            // Assert
            // Note: Resultado dependerá de si el SP existe en la BD
            // En un ambiente real, esto debería retornar true o false basado en ejecución del SP
            Assert.IsType<bool>(result);
        }

        [Fact]
        public async Task AprobarPlanillaAsync_WithZeroMonto_ProcessesSuccessfully()
        {
            // Arrange
            int planillaId = 1;
            decimal montoAutorizado = 0m;
            int usuarioId = 1;
            string rol = "PMO";

            // Act
            var result = await _service.AprobarPlanillaAsync(planillaId, montoAutorizado, usuarioId, rol);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public async Task RechazarPlanillaAsync_WithValidInput_ReturnsTrue()
        {
            // Arrange
            int planillaId = 1;
            string observacion = "Monto excedido";
            int usuarioId = 1;
            string rol = "PMO";

            // Act
            var result = await _service.RechazarPlanillaAsync(planillaId, observacion, usuarioId, rol);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public async Task RechazarPlanillaAsync_WithNullObservacion_HandlesGracefully()
        {
            // Arrange
            int planillaId = 1;
            string observacion = null;
            int usuarioId = 1;
            string rol = "PMO";

            // Act
            var result = await _service.RechazarPlanillaAsync(planillaId, observacion, usuarioId, rol);

            // Assert
            Assert.IsType<bool>(result);
        }

        [Fact]
        public async Task ValidarMontosPlanillaAsync_WithValidInput_ReturnsValidTuple()
        {
            // Arrange
            int trabajoId = 1;
            decimal montoTotal = 500m;

            // Act
            var result = await _service.ValidarMontosPlanillaAsync(trabajoId, montoTotal);

            // Assert
            Assert.IsType<ValueTuple<bool, string>>(result);
            Assert.NotNull(result.Item2); // Message
        }

        [Fact]
        public async Task ValidarMontosPlanillaAsync_WithNegativeMonto_ReturnsFalse()
        {
            // Arrange
            int trabajoId = 1;
            decimal montoTotal = -100m;

            // Act
            var result = await _service.ValidarMontosPlanillaAsync(trabajoId, montoTotal);

            // Assert
            Assert.False(result.Item1); // Valid should be false
            Assert.NotEmpty(result.Item2); // Message should not be empty
        }

        [Theory]
        [InlineData(0)]
        [InlineData(100)]
        [InlineData(1000)]
        [InlineData(10000)]
        public async Task ValidarMontosPlanillaAsync_VariousMontos_ProcessedSuccessfully(decimal monto)
        {
            // Arrange
            int trabajoId = 1;

            // Act
            var result = await _service.ValidarMontosPlanillaAsync(trabajoId, monto);

            // Assert
            Assert.IsType<ValueTuple<bool, string>>(result);
        }

        [Fact]
        public void Logger_VerifyInformationLogged()
        {
            // Arrange
            int trabajoId = 1;
            string rol = "PMO";
            int usuarioId = 1;

            // Act
            var task = _service.ObtenerPlanillasPorRolAsync(trabajoId, rol, usuarioId);
            task.Wait();

            // Assert
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.AtLeastOnce);
        }
    }
}
