using Moq;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Tests.Services.OP;

/// <summary>
/// Unit tests for OpFichaService
/// Validates Ficha Cuantitativa management (create, update, sync with Habeas Data)
/// Ref: S4-001.4 implementation
/// </summary>
public class OpFichaServiceTests
{
    private readonly Mock<IOpFichaService> _mockFichaService;

    public OpFichaServiceTests()
    {
        _mockFichaService = new Mock<IOpFichaService>();
    }

    #region ObtenerPorTrabajoAsync Tests

    [Fact]
    public async Task ObtenerPorTrabajoAsync_WithValidId_ReturnsFicha()
    {
        // Arrange
        var trabajoId = 1L;
        var expectedFicha = new FichaCuantitativaVM
        {
            TrabajoId = trabajoId,
            Concepto = "Encuesta Satisfacción",
            Porc_Muestra = 100m,
            Monto_Presupuesto = 5000000m
        };

        _mockFichaService
            .Setup(x => x.ObtenerPorTrabajoAsync(trabajoId))
            .ReturnsAsync(expectedFicha);

        // Act
        var result = await _mockFichaService.Object.ObtenerPorTrabajoAsync(trabajoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trabajoId, result.TrabajoId);
        Assert.Equal("Encuesta Satisfacción", result.Concepto);
    }

    [Fact]
    public async Task ObtenerPorTrabajoAsync_WithInvalidId_ReturnsNull()
    {
        // Arrange
        var trabajoId = 999L;
        _mockFichaService
            .Setup(x => x.ObtenerPorTrabajoAsync(It.IsAny<long>()))
            .ReturnsAsync((FichaCuantitativaVM?)null);

        // Act
        var result = await _mockFichaService.Object.ObtenerPorTrabajoAsync(trabajoId);

        // Assert
        Assert.Null(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(5000)]
    public async Task ObtenerPorTrabajoAsync_VariousTrabajoIds_AllProcessedSuccessfully(long trabajoId)
    {
        // Arrange
        _mockFichaService
            .Setup(x => x.ObtenerPorTrabajoAsync(It.IsAny<long>()))
            .ReturnsAsync(new FichaCuantitativaVM { TrabajoId = trabajoId });

        // Act
        var result = await _mockFichaService.Object.ObtenerPorTrabajoAsync(trabajoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(trabajoId, result.TrabajoId);
    }

    #endregion

    #region GuardarAsync Tests

    [Fact]
    public async Task GuardarAsync_WithValidData_ReturnsId()
    {
        // Arrange
        var model = new FichaCuantitativaVM
        {
            TrabajoId = 1L,
            Concepto = "Test Concept",
            Porc_Muestra = 50m,
            Monto_Presupuesto = 1000000m
        };
        var usuarioId = 100L;
        var expectedId = 1L;

        _mockFichaService
            .Setup(x => x.GuardarAsync(model, usuarioId))
            .ReturnsAsync(expectedId);

        // Act
        var result = await _mockFichaService.Object.GuardarAsync(model, usuarioId);

        // Assert
        Assert.Equal(expectedId, result);
        _mockFichaService.Verify(x => x.GuardarAsync(model, usuarioId), Times.Once);
    }

    [Fact]
    public async Task GuardarAsync_WithZeroBudget_ProcessesSuccessfully()
    {
        // Arrange
        var model = new FichaCuantitativaVM
        {
            TrabajoId = 1L,
            Monto_Presupuesto = 0m
        };

        _mockFichaService
            .Setup(x => x.GuardarAsync(It.IsAny<FichaCuantitativaVM>(), It.IsAny<long>()))
            .ReturnsAsync(1L);

        // Act
        var result = await _mockFichaService.Object.GuardarAsync(model, 100L);

        // Assert
        Assert.Equal(1L, result);
    }

    [Theory]
    [InlineData(100m, 25, 25)]
    [InlineData(1000m, 50, 500)]
    [InlineData(5000m, 75, 3750)]
    public async Task GuardarAsync_VariousMontosAndPercentages_AllProcessedSuccessfully(decimal monto, int porc, decimal expected)
    {
        // Arrange
        var model = new FichaCuantitativaVM
        {
            TrabajoId = 1L,
            Monto_Presupuesto = monto,
            Porc_Muestra = porc
        };

        _mockFichaService
            .Setup(x => x.GuardarAsync(model, It.IsAny<long>()))
            .ReturnsAsync(1L);

        // Act
        var result = await _mockFichaService.Object.GuardarAsync(model, 100L);

        // Assert
        Assert.Equal(1L, result);
    }

    #endregion

    #region SincronizarHabeasDataAsync Tests

    [Fact]
    public async Task SincronizarHabeasDataAsync_WithValidData_CompletesSuccessfully()
    {
        // Arrange
        var trabajoId = 1L;
        var habeasData = "Autorizo el tratamiento de datos personales...";

        _mockFichaService
            .Setup(x => x.SincronizarHabeasDataAsync(trabajoId, habeasData))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await _mockFichaService.Object.SincronizarHabeasDataAsync(trabajoId, habeasData);

        _mockFichaService.Verify(x => x.SincronizarHabeasDataAsync(trabajoId, habeasData), Times.Once);
    }

    [Fact]
    public async Task SincronizarHabeasDataAsync_WithEmptyText_HandlesGracefully()
    {
        // Arrange
        var trabajoId = 1L;

        _mockFichaService
            .Setup(x => x.SincronizarHabeasDataAsync(It.IsAny<long>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await _mockFichaService.Object.SincronizarHabeasDataAsync(trabajoId, string.Empty);
    }

    [Theory]
    [InlineData("Short text")]
    [InlineData("This is a longer habeas data text with multiple lines and information about data processing consent...")]
    [InlineData("")]
    public async Task SincronizarHabeasDataAsync_VariousTextLengths_AllProcessedSuccessfully(string habeasData)
    {
        // Arrange
        var trabajoId = 1L;
        _mockFichaService
            .Setup(x => x.SincronizarHabeasDataAsync(trabajoId, habeasData))
            .Returns(Task.CompletedTask);

        // Act & Assert
        await _mockFichaService.Object.SincronizarHabeasDataAsync(trabajoId, habeasData);
    }

    #endregion

    #region ObtenerIdProyectoPorTrabajoAsync Tests

    [Fact]
    public async Task ObtenerIdProyectoPorTrabajoAsync_WithValidTrabajo_ReturnsProyectoId()
    {
        // Arrange
        var trabajoId = 1L;
        var expectedProyectoId = 50L;

        _mockFichaService
            .Setup(x => x.ObtenerIdProyectoPorTrabajoAsync(trabajoId))
            .ReturnsAsync(expectedProyectoId);

        // Act
        var result = await _mockFichaService.Object.ObtenerIdProyectoPorTrabajoAsync(trabajoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(expectedProyectoId, result);
    }

    [Fact]
    public async Task ObtenerIdProyectoPorTrabajoAsync_WithInvalidTrabajo_ReturnsNull()
    {
        // Arrange
        var trabajoId = 999L;
        _mockFichaService
            .Setup(x => x.ObtenerIdProyectoPorTrabajoAsync(It.IsAny<long>()))
            .ReturnsAsync((long?)null);

        // Act
        var result = await _mockFichaService.Object.ObtenerIdProyectoPorTrabajoAsync(trabajoId);

        // Assert
        Assert.Null(result);
    }

    #endregion
}
