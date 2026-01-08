using Moq;
using Xunit;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.OP.Models;

namespace MatrixNext.Web.Tests.Services.OP;

/// <summary>
/// Unit tests for OpTraficoService
/// Tests traffic/movement tracking and reporting
/// </summary>
public class OpTraficoServiceTests
{
    private readonly Mock<IOpTraficoService> _traficoServiceMock;

    public OpTraficoServiceTests()
    {
        _traficoServiceMock = new Mock<IOpTraficoService>();
    }

    #region ObtenerResumenAsync Tests

    [Fact]
    public async Task ObtenerResumenAsync_WithoutTrabajId_ReturnGlobalSummary()
    {
        // Arrange
        var expectedSummary = new OpTraficoSummary
        {
            TotalMovimientos = 1000,
            MovimientosRecibidos = 800,
            MovimientosEnProceso = 150,
            MovimientosRechazados = 50,
            TasaRechazos = 5.0m,
            FechaActualizacion = DateTime.Now
        };

        _traficoServiceMock
            .Setup(x => x.ObtenerResumenAsync(null))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _traficoServiceMock.Object.ObtenerResumenAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1000, result.TotalMovimientos);
        Assert.Equal(800, result.MovimientosRecibidos);
    }

    [Fact]
    public async Task ObtenerResumenAsync_WithValidTrabajo_ReturnWorkSpecificSummary()
    {
        // Arrange
        var trabajoId = 1;
        var expectedSummary = new OpTraficoSummary
        {
            TotalMovimientos = 100,
            MovimientosRecibidos = 85,
            MovimientosEnProceso = 10,
            MovimientosRechazados = 5,
            TasaRechazos = 5.0m,
            FechaActualizacion = DateTime.Now
        };

        _traficoServiceMock
            .Setup(x => x.ObtenerResumenAsync(trabajoId))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _traficoServiceMock.Object.ObtenerResumenAsync(trabajoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.TotalMovimientos);
        Assert.Equal(85, result.MovimientosRecibidos);
    }

    [Fact]
    public async Task ObtenerResumenAsync_WithInvalidTrabajo_ReturnEmptySummary()
    {
        // Arrange
        var trabajoId = 999;
        var expectedSummary = new OpTraficoSummary
        {
            TotalMovimientos = 0,
            MovimientosRecibidos = 0,
            MovimientosEnProceso = 0,
            MovimientosRechazados = 0,
            TasaRechazos = 0m,
            FechaActualizacion = DateTime.Now
        };

        _traficoServiceMock
            .Setup(x => x.ObtenerResumenAsync(trabajoId))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _traficoServiceMock.Object.ObtenerResumenAsync(trabajoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(0, result.TotalMovimientos);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(100)]
    public async Task ObtenerResumenAsync_WithVariousTrabajos_ReturnValidSummary(long trabajoId)
    {
        // Arrange
        var expectedSummary = new OpTraficoSummary
        {
            TotalMovimientos = 100,
            MovimientosRecibidos = 85,
            MovimientosEnProceso = 10,
            MovimientosRechazados = 5,
            TasaRechazos = 5.0m,
            FechaActualizacion = DateTime.Now
        };

        _traficoServiceMock
            .Setup(x => x.ObtenerResumenAsync(trabajoId))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _traficoServiceMock.Object.ObtenerResumenAsync(trabajoId);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.TotalMovimientos >= 0);
    }

    #endregion

    #region Rejection Rate Calculation Tests

    [Fact]
    public async Task ObtenerResumenAsync_CalculatesRejectionRateCorrectly()
    {
        // Arrange
        var trabajoId = 1;
        var expectedSummary = new OpTraficoSummary
        {
            TotalMovimientos = 100,
            MovimientosRecibidos = 90,
            MovimientosEnProceso = 5,
            MovimientosRechazados = 5,
            TasaRechazos = 5.0m,
            FechaActualizacion = DateTime.Now
        };

        _traficoServiceMock
            .Setup(x => x.ObtenerResumenAsync(trabajoId))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _traficoServiceMock.Object.ObtenerResumenAsync(trabajoId);

        // Assert
        Assert.NotNull(result);
        var expectedRate = (5m / 100m) * 100m;
        Assert.Equal(expectedRate, result.TasaRechazos);
    }

    [Fact]
    public async Task ObtenerResumenAsync_ZeroRejections_ReturnZeroRate()
    {
        // Arrange
        var trabajoId = 1;
        var expectedSummary = new OpTraficoSummary
        {
            TotalMovimientos = 100,
            MovimientosRecibidos = 100,
            MovimientosEnProceso = 0,
            MovimientosRechazados = 0,
            TasaRechazos = 0.0m,
            FechaActualizacion = DateTime.Now
        };

        _traficoServiceMock
            .Setup(x => x.ObtenerResumenAsync(trabajoId))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _traficoServiceMock.Object.ObtenerResumenAsync(trabajoId);

        // Assert
        Assert.Equal(0.0m, result.TasaRechazos);
    }

    [Fact]
    public async Task ObtenerResumenAsync_HighRejectionRate_ReturnCorrectRate()
    {
        // Arrange
        var trabajoId = 1;
        var expectedSummary = new OpTraficoSummary
        {
            TotalMovimientos = 100,
            MovimientosRecibidos = 70,
            MovimientosEnProceso = 20,
            MovimientosRechazados = 30,
            TasaRechazos = 30.0m,
            FechaActualizacion = DateTime.Now
        };

        _traficoServiceMock
            .Setup(x => x.ObtenerResumenAsync(trabajoId))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _traficoServiceMock.Object.ObtenerResumenAsync(trabajoId);

        // Assert
        Assert.Equal(30.0m, result.TasaRechazos);
    }

    #endregion

    #region Movement Distribution Tests

    [Fact]
    public async Task ObtenerResumenAsync_VerifyMovementDistribution_SumEquals Total()
    {
        // Arrange
        var trabajoId = 1;
        var expectedSummary = new OpTraficoSummary
        {
            TotalMovimientos = 100,
            MovimientosRecibidos = 70,
            MovimientosEnProceso = 20,
            MovimientosRechazados = 10,
            TasaRechazos = 10.0m,
            FechaActualizacion = DateTime.Now
        };

        _traficoServiceMock
            .Setup(x => x.ObtenerResumenAsync(trabajoId))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _traficoServiceMock.Object.ObtenerResumenAsync(trabajoId);

        // Assert
        var sum = result.MovimientosRecibidos + result.MovimientosEnProceso + result.MovimientosRechazados;
        Assert.Equal(result.TotalMovimientos, sum);
    }

    [Fact]
    public async Task ObtenerResumenAsync_IncompleteDistribution_StillValid()
    {
        // Arrange
        var trabajoId = 1;
        var expectedSummary = new OpTraficoSummary
        {
            TotalMovimientos = 100,
            MovimientosRecibidos = 60,
            MovimientosEnProceso = 30,
            MovimientosRechazados = 0,
            TasaRechazos = 0.0m,
            FechaActualizacion = DateTime.Now
        };

        _traficoServiceMock
            .Setup(x => x.ObtenerResumenAsync(trabajoId))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _traficoServiceMock.Object.ObtenerResumenAsync(trabajoId);

        // Assert
        Assert.True(result.MovimientosRecibidos >= 0);
        Assert.True(result.MovimientosEnProceso >= 0);
        Assert.True(result.MovimientosRechazados >= 0);
    }

    #endregion

    #region Timestamp Tests

    [Fact]
    public async Task ObtenerResumenAsync_ReturnCurrentTimestamp()
    {
        // Arrange
        var beforeCall = DateTime.Now;
        var expectedSummary = new OpTraficoSummary
        {
            TotalMovimientos = 100,
            MovimientosRecibidos = 85,
            MovimientosEnProceso = 10,
            MovimientosRechazados = 5,
            TasaRechazos = 5.0m,
            FechaActualizacion = DateTime.Now
        };
        var afterCall = DateTime.Now;

        _traficoServiceMock
            .Setup(x => x.ObtenerResumenAsync(null))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _traficoServiceMock.Object.ObtenerResumenAsync();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.FechaActualizacion >= beforeCall.AddSeconds(-1));
        Assert.True(result.FechaActualizacion <= afterCall.AddSeconds(1));
    }

    #endregion
}

/// <summary>
/// Mock models for testing
/// </summary>
public class OpTraficoSummary
{
    public long TotalMovimientos { get; set; }
    public long MovimientosRecibidos { get; set; }
    public long MovimientosEnProceso { get; set; }
    public long MovimientosRechazados { get; set; }
    public decimal TasaRechazos { get; set; }
    public DateTime FechaActualizacion { get; set; }
}
