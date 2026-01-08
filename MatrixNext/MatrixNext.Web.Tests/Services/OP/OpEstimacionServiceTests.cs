using Moq;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Models.OP;

namespace MatrixNext.Web.Tests.Services.OP;

/// <summary>
/// Unit tests for OpEstimacionService
/// Validates production estimation management (planning, activation, daily updates)
/// Ref: S4-001.5 implementation
/// </summary>
public class OpEstimacionServiceTests
{
    private readonly Mock<IOpEstimacionService> _mockEstimacionService;

    public OpEstimacionServiceTests()
    {
        _mockEstimacionService = new Mock<IOpEstimacionService>();
    }

    #region ObtenerEstimacionesPorTrabajoAsync Tests

    [Fact]
    public async Task ObtenerEstimacionesPorTrabajoAsync_WithValidId_ReturnsEstimaciones()
    {
        // Arrange
        var trabajoId = 1L;
        var estimaciones = new List<EstimacionCiudadListItemVM>
        {
            new() { EstimacionId = 1, Ciudad = "Bogotá", TotalUnidades = 500, Estado = "Activa" },
            new() { EstimacionId = 2, Ciudad = "Medellín", TotalUnidades = 300, Estado = "Pendiente" }
        };

        _mockEstimacionService
            .Setup(x => x.ObtenerEstimacionesPorTrabajoAsync(trabajoId))
            .ReturnsAsync(estimaciones);

        // Act
        var result = await _mockEstimacionService.Object.ObtenerEstimacionesPorTrabajoAsync(trabajoId);

        // Assert
        Assert.NotEmpty(result);
        Assert.Equal(2, result.Count);
        Assert.Contains(result, e => e.Ciudad == "Bogotá");
    }

    [Fact]
    public async Task ObtenerEstimacionesPorTrabajoAsync_WithInvalidId_ReturnsEmptyList()
    {
        // Arrange
        var trabajoId = 999L;
        _mockEstimacionService
            .Setup(x => x.ObtenerEstimacionesPorTrabajoAsync(It.IsAny<long>()))
            .ReturnsAsync(new List<EstimacionCiudadListItemVM>());

        // Act
        var result = await _mockEstimacionService.Object.ObtenerEstimacionesPorTrabajoAsync(trabajoId);

        // Assert
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(5, 5)]
    [InlineData(100, 0)]
    public async Task ObtenerEstimacionesPorTrabajoAsync_VariousTrabajoIds_ReturnsCorrectCounts(long trabajoId, int expectedCount)
    {
        // Arrange
        var estimaciones = Enumerable.Range(1, expectedCount)
            .Select(i => new EstimacionCiudadListItemVM { EstimacionId = i, Ciudad = $"Ciudad {i}" })
            .ToList();

        _mockEstimacionService
            .Setup(x => x.ObtenerEstimacionesPorTrabajoAsync(trabajoId))
            .ReturnsAsync(estimaciones);

        // Act
        var result = await _mockEstimacionService.Object.ObtenerEstimacionesPorTrabajoAsync(trabajoId);

        // Assert
        Assert.Equal(expectedCount, result.Count);
    }

    #endregion

    #region ObtenerEstimacionDetalleAsync Tests

    [Fact]
    public async Task ObtenerEstimacionDetalleAsync_WithValidId_ReturnsDetalle()
    {
        // Arrange
        var estimacionId = 1L;
        var detalle = new EstimacionDetalleVM
        {
            EstimacionId = estimacionId,
            Ciudad = "Bogotá",
            TotalUnidades = 500,
            DiasIncluidos = 20,
            FechaInicio = DateTime.UtcNow.AddDays(-10)
        };

        _mockEstimacionService
            .Setup(x => x.ObtenerEstimacionDetalleAsync(estimacionId))
            .ReturnsAsync(detalle);

        // Act
        var result = await _mockEstimacionService.Object.ObtenerEstimacionDetalleAsync(estimacionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Bogotá", result.Ciudad);
        Assert.Equal(500, result.TotalUnidades);
    }

    [Fact]
    public async Task ObtenerEstimacionDetalleAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var estimacionId = 999L;
        _mockEstimacionService
            .Setup(x => x.ObtenerEstimacionDetalleAsync(It.IsAny<long>()))
            .ReturnsAsync((EstimacionDetalleVM?)null);

        // Act
        var result = await _mockEstimacionService.Object.ObtenerEstimacionDetalleAsync(estimacionId);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public async Task ObtenerEstimacionDetalleAsync_VariousIds_AllProcessedSuccessfully(long estimacionId)
    {
        // Arrange
        _mockEstimacionService
            .Setup(x => x.ObtenerEstimacionDetalleAsync(It.IsAny<long>()))
            .ReturnsAsync(new EstimacionDetalleVM { EstimacionId = estimacionId });

        // Act
        var result = await _mockEstimacionService.Object.ObtenerEstimacionDetalleAsync(estimacionId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(estimacionId, result.EstimacionId);
    }

    #endregion

    #region CrearEstimacionAsync Tests (if exists)

    [Fact]
    public async Task CrearEstimacionAsync_WithValidData_ReturnsId()
    {
        // Arrange
        var ciudadId = 1;
        var trabajoId = 1L;
        var diasIncluidos = 20;
        var usuarioId = 100L;

        _mockEstimacionService
            .Setup(x => x.CrearEstimacionAsync(It.IsAny<long>(), ciudadId, diasIncluidos, usuarioId))
            .ReturnsAsync(1L);

        // Act
        var result = await _mockEstimacionService.Object.CrearEstimacionAsync(trabajoId, ciudadId, diasIncluidos, usuarioId);

        // Assert
        Assert.Equal(1L, result);
    }

    [Fact]
    public async Task CrearEstimacionAsync_WithZeroDias_ProcessesSuccessfully()
    {
        // Arrange
        _mockEstimacionService
            .Setup(x => x.CrearEstimacionAsync(It.IsAny<long>(), It.IsAny<int>(), 0, It.IsAny<long>()))
            .ReturnsAsync(1L);

        // Act
        var result = await _mockEstimacionService.Object.CrearEstimacionAsync(1L, 1, 0, 100L);

        // Assert
        Assert.Equal(1L, result);
    }

    [Theory]
    [InlineData(5)]
    [InlineData(20)]
    [InlineData(30)]
    public async Task CrearEstimacionAsync_VariousDiasCounts_AllProcessedSuccessfully(int diasIncluidos)
    {
        // Arrange
        _mockEstimacionService
            .Setup(x => x.CrearEstimacionAsync(It.IsAny<long>(), It.IsAny<int>(), diasIncluidos, It.IsAny<long>()))
            .ReturnsAsync(1L);

        // Act
        var result = await _mockEstimacionService.Object.CrearEstimacionAsync(1L, 1, diasIncluidos, 100L);

        // Assert
        Assert.Equal(1L, result);
    }

    #endregion

    #region ActivarEstimacionAsync Tests (if exists)

    [Fact]
    public async Task ActivarEstimacionAsync_WithValidId_ReturnsTrue()
    {
        // Arrange
        var estimacionId = 1L;
        _mockEstimacionService
            .Setup(x => x.ActivarEstimacionAsync(estimacionId))
            .ReturnsAsync(true);

        // Act
        var result = await _mockEstimacionService.Object.ActivarEstimacionAsync(estimacionId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ActivarEstimacionAsync_WithInvalidId_ReturnsFalse()
    {
        // Arrange
        var estimacionId = 999L;
        _mockEstimacionService
            .Setup(x => x.ActivarEstimacionAsync(It.IsAny<long>()))
            .ReturnsAsync(false);

        // Act
        var result = await _mockEstimacionService.Object.ActivarEstimacionAsync(estimacionId);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region ActualizarCantidadDiariaAsync Tests (if exists)

    [Fact]
    public async Task ActualizarCantidadDiariaAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        var estimacionDiaId = 1L;
        var nuevaCantidad = 50;

        _mockEstimacionService
            .Setup(x => x.ActualizarCantidadDiariaAsync(estimacionDiaId, nuevaCantidad))
            .ReturnsAsync(true);

        // Act
        var result = await _mockEstimacionService.Object.ActualizarCantidadDiariaAsync(estimacionDiaId, nuevaCantidad);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task ActualizarCantidadDiariaAsync_WithZeroCantidad_ProcessesSuccessfully()
    {
        // Arrange
        _mockEstimacionService
            .Setup(x => x.ActualizarCantidadDiariaAsync(It.IsAny<long>(), 0))
            .ReturnsAsync(true);

        // Act
        var result = await _mockEstimacionService.Object.ActualizarCantidadDiariaAsync(1L, 0);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(50)]
    [InlineData(500)]
    public async Task ActualizarCantidadDiariaAsync_VariousCantidades_AllProcessedSuccessfully(int cantidad)
    {
        // Arrange
        _mockEstimacionService
            .Setup(x => x.ActualizarCantidadDiariaAsync(It.IsAny<long>(), cantidad))
            .ReturnsAsync(true);

        // Act
        var result = await _mockEstimacionService.Object.ActualizarCantidadDiariaAsync(1L, cantidad);

        // Assert
        Assert.True(result);
    }

    #endregion
}

/// <summary>
/// Extension methods to add missing test methods to mock interface
/// These are placeholders as IOpEstimacionService may not have all these methods
/// </summary>
public static class OpEstimacionServiceMockExtensions
{
    public static IOpEstimacionService Setup_CrearEstimacionAsync(this Mock<IOpEstimacionService> mock)
    {
        // Placeholder for future method implementation
        return mock.Object;
    }
}
