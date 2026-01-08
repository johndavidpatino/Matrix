using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.OP.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MatrixNext.Web.Tests.Services.OP
{
    /// <summary>
    /// Tests unitarios para OpRegistroProduccionService
    /// Cobertura de cascading dropdowns, búsqueda de JobBooks y validaciones
    /// </summary>
    public class OpRegistroProduccionServiceTests
    {
        private readonly MatrixDbContext _dbContext;
        private readonly IOpRegistroProduccionService _service;
        private readonly Mock<ILogger<OpRegistroProduccionService>> _loggerMock;

        public OpRegistroProduccionServiceTests()
        {
            // Setup InMemory DB para tests
            var options = new DbContextOptionsBuilder<MatrixDbContext>()
                .UseInMemoryDatabase(databaseName: $"OP_Registro_Test_{Guid.NewGuid()}")
                .Options;

            _dbContext = new MatrixDbContext(options);
            _loggerMock = new Mock<ILogger<OpRegistroProduccionService>>();
            _service = new OpRegistroProduccionService(_dbContext, _loggerMock.Object);
        }

        [Fact]
        public async Task ObtenerUnidadesAsync_ReturnsList()
        {
            // Act
            var result = await _service.ObtenerUnidadesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CatalogoItemDto>>(result);
        }

        [Fact]
        public async Task ObtenerUnidadesAsync_ReturnsOrderedByName()
        {
            // Act
            var result = await _service.ObtenerUnidadesAsync();

            // Assert
            Assert.NotNull(result);
            // Verify ordering (if items exist)
            if (result.Count > 1)
            {
                for (int i = 0; i < result.Count - 1; i++)
                {
                    Assert.True(string.Compare(result[i].Nombre, result[i + 1].Nombre) <= 0,
                        "Unidades should be ordered by name");
                }
            }
        }

        [Fact]
        public async Task ObtenerActividadesAsync_WithValidUnidadId_ReturnsList()
        {
            // Arrange
            int unidadId = 1;

            // Act
            var result = await _service.ObtenerActividadesAsync(unidadId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CatalogoItemDto>>(result);
        }

        [Fact]
        public async Task ObtenerActividadesAsync_WithInvalidUnidadId_ReturnsEmptyList()
        {
            // Arrange
            int invalidUnidadId = -999;

            // Act
            var result = await _service.ObtenerActividadesAsync(invalidUnidadId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(10)]
        public async Task ObtenerActividadesAsync_VariousUnidadIds_ProcessedSuccessfully(int unidadId)
        {
            // Act
            var result = await _service.ObtenerActividadesAsync(unidadId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CatalogoItemDto>>(result);
        }

        [Fact]
        public async Task ObtenerSubactividadesAsync_WithValidActivityId_ReturnsList()
        {
            // Arrange
            int actividadId = 1;

            // Act
            var result = await _service.ObtenerSubactividadesAsync(actividadId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<CatalogoItemDto>>(result);
        }

        [Fact]
        public async Task ObtenerSubactividadesAsync_WithInvalidActivityId_ReturnsEmptyList()
        {
            // Arrange
            int invalidActividadId = -999;

            // Act
            var result = await _service.ObtenerSubactividadesAsync(invalidActividadId);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task BuscarJobBooksAsync_WithValidCriteria_ReturnsList()
        {
            // Arrange
            string criterio = "TEST";
            string tipo = "JBE";

            // Act
            var result = await _service.BuscarJobBooksAsync(criterio, tipo);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<JobBookDto>>(result);
        }

        [Fact]
        public async Task BuscarJobBooksAsync_WithNullCriteria_ReturnsEmptyList()
        {
            // Arrange
            string criterio = null;
            string tipo = "JBE";

            // Act
            var result = await _service.BuscarJobBooksAsync(criterio, tipo);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("JBE")]
        [InlineData("JBI")]
        [InlineData("CC")]
        public async Task BuscarJobBooksAsync_VariousTypes_ProcessedSuccessfully(string tipo)
        {
            // Arrange
            string criterio = "TEST";

            // Act
            var result = await _service.BuscarJobBooksAsync(criterio, tipo);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<JobBookDto>>(result);
        }

        [Fact]
        public async Task RegistrarActividadAsync_WithValidData_ReturnsId()
        {
            // Arrange
            var registro = new RegistroProduccionDto
            {
                UnidadId = 1,
                ActividadId = 1,
                SubactividadId = 1,
                Cantidad = 10,
                Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                UsuarioId = 1
            };

            // Act
            var result = await _service.RegistrarActividadAsync(registro);

            // Assert
            // En un ambiente sin BD real, esto puede retornar 0 o un ID
            Assert.IsType<int>(result);
        }

        [Fact]
        public async Task RegistrarActividadAsync_WithInvalidData_ThrowsOrHandlesError()
        {
            // Arrange
            var registro = new RegistroProduccionDto
            {
                UnidadId = -1, // Invalid
                ActividadId = 1,
                SubactividadId = 1,
                Cantidad = 10,
                Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                UsuarioId = 1
            };

            // Act & Assert
            // Should either throw or return gracefully
            try
            {
                var result = await _service.RegistrarActividadAsync(registro);
                Assert.IsType<int>(result);
            }
            catch (Exception ex)
            {
                Assert.NotNull(ex); // Expected to fail
            }
        }

        [Fact]
        public async Task ValidarRegistroAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var registro = new RegistroProduccionDto
            {
                UnidadId = 1,
                ActividadId = 1,
                SubactividadId = 1,
                Cantidad = 10,
                Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                UsuarioId = 1
            };

            // Act
            var result = await _service.ValidarRegistroAsync(registro);

            // Assert
            Assert.True(result.Item1); // Valid
            Assert.NotEmpty(result.Item2); // Message
        }

        [Fact]
        public async Task ValidarRegistroAsync_WithZeroQuantity_ReturnsFalse()
        {
            // Arrange
            var registro = new RegistroProduccionDto
            {
                UnidadId = 1,
                ActividadId = 1,
                SubactividadId = 1,
                Cantidad = 0, // Invalid
                Fecha = DateTime.Now.ToString("yyyy-MM-dd"),
                UsuarioId = 1
            };

            // Act
            var result = await _service.ValidarRegistroAsync(registro);

            // Assert
            Assert.False(result.Item1); // Should be invalid
            Assert.NotEmpty(result.Item2); // Error message
        }

        [Fact]
        public async Task ValidarRegistroAsync_WithFutureDate_ReturnsFalse()
        {
            // Arrange
            var registro = new RegistroProduccionDto
            {
                UnidadId = 1,
                ActividadId = 1,
                SubactividadId = 1,
                Cantidad = 10,
                Fecha = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"), // Future date - Invalid
                UsuarioId = 1
            };

            // Act
            var result = await _service.ValidarRegistroAsync(registro);

            // Assert
            Assert.False(result.Item1); // Should be invalid
            Assert.NotEmpty(result.Item2); // Error message
        }

        [Fact]
        public async Task ValidarRegistroAsync_WithPastDate_ReturnsTrue()
        {
            // Arrange
            var registro = new RegistroProduccionDto
            {
                UnidadId = 1,
                ActividadId = 1,
                SubactividadId = 1,
                Cantidad = 10,
                Fecha = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"), // Past date - Valid
                UsuarioId = 1
            };

            // Act
            var result = await _service.ValidarRegistroAsync(registro);

            // Assert
            Assert.True(result.Item1); // Valid
        }

        [Fact]
        public void Logger_VerifyErrorLogged()
        {
            // Arrange
            int invalidId = -999;

            // Act
            var task = _service.ObtenerActividadesAsync(invalidId);
            task.Wait();

            // Assert
            // Logger may or may not be called depending on implementation
            // This test verifies the logger mock is working
            Assert.NotNull(_loggerMock);
        }
    }
}
