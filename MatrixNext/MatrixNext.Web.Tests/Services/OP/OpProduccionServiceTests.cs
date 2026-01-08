using Moq;
using Xunit;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.ViewModels.OP;

namespace MatrixNext.Web.Tests.Services.OP;

/// <summary>
/// Unit tests for OpProduccionService
/// Tests production data entry and retrieval
/// </summary>
public class OpProduccionServiceTests
{
    private readonly Mock<IOpProduccionService> _produccionServiceMock;

    public OpProduccionServiceTests()
    {
        _produccionServiceMock = new Mock<IOpProduccionService>();
    }

    #region ObtenerUnidadesAsync Tests

    [Fact]
    public async Task ObtenerUnidadesAsync_ReturnsAllUnidades()
    {
        // Arrange
        var expectedUnidades = new List<UnidadDto>
        {
            new UnidadDto { Id = 1, Codigo = "UN01", Descripcion = "Unidad de Medida 1" },
            new UnidadDto { Id = 2, Codigo = "UN02", Descripcion = "Unidad de Medida 2" }
        };

        _produccionServiceMock
            .Setup(x => x.ObtenerUnidadesAsync(null, default))
            .ReturnsAsync(expectedUnidades);

        // Act
        var result = await _produccionServiceMock.Object.ObtenerUnidadesAsync(null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ObtenerUnidadesAsync_WithIdentificacion_ReturnsFiltered()
    {
        // Arrange
        var identificacion = 12345678L;
        var expectedUnidades = new List<UnidadDto>
        {
            new UnidadDto { Id = 1, Codigo = "UN01", Descripcion = "Unidad de Medida 1" }
        };

        _produccionServiceMock
            .Setup(x => x.ObtenerUnidadesAsync(identificacion, default))
            .ReturnsAsync(expectedUnidades);

        // Act
        var result = await _produccionServiceMock.Object.ObtenerUnidadesAsync(identificacion);

        // Assert
        Assert.Single(result);
    }

    #endregion

    #region ObtenerActividadesAsync Tests

    [Fact]
    public async Task ObtenerActividadesAsync_ReturnsAllActividades()
    {
        // Arrange
        var expectedActividades = new List<ActividadDto>
        {
            new ActividadDto { Id = 1, Codigo = "ACT01", Descripcion = "Actividad 1" },
            new ActividadDto { Id = 2, Codigo = "ACT02", Descripcion = "Actividad 2" }
        };

        _produccionServiceMock
            .Setup(x => x.ObtenerActividadesAsync(null, null, default))
            .ReturnsAsync(expectedActividades);

        // Act
        var result = await _produccionServiceMock.Object.ObtenerActividadesAsync(null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ObtenerActividadesAsync_WithUnidadFilter_ReturnsFiltered()
    {
        // Arrange
        var unidad = 1;
        var expectedActividades = new List<ActividadDto>
        {
            new ActividadDto { Id = 1, Codigo = "ACT01", Descripcion = "Actividad 1" }
        };

        _produccionServiceMock
            .Setup(x => x.ObtenerActividadesAsync(unidad, null, default))
            .ReturnsAsync(expectedActividades);

        // Act
        var result = await _produccionServiceMock.Object.ObtenerActividadesAsync(unidad, null);

        // Assert
        Assert.Single(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    public async Task ObtenerActividadesAsync_WithVariousUnidades_ReturnsValidResults(int unidad)
    {
        // Arrange
        _produccionServiceMock
            .Setup(x => x.ObtenerActividadesAsync(unidad, null, default))
            .ReturnsAsync(new List<ActividadDto>());

        // Act
        var result = await _produccionServiceMock.Object.ObtenerActividadesAsync(unidad, null);

        // Assert
        Assert.NotNull(result);
    }

    #endregion

    #region ObtenerJbeAsync Tests

    [Fact]
    public async Task ObtenerJbeAsync_WithTipo_ReturnsJbeList()
    {
        // Arrange
        var tipo = 1;
        var expectedJbes = new List<JbeDto>
        {
            new JbeDto { Id = 1, Codigo = "JBE01", Descripcion = "JBE 1" },
            new JbeDto { Id = 2, Codigo = "JBE02", Descripcion = "JBE 2" }
        };

        _produccionServiceMock
            .Setup(x => x.ObtenerJbeAsync(tipo, null, default))
            .ReturnsAsync(expectedJbes);

        // Act
        var result = await _produccionServiceMock.Object.ObtenerJbeAsync(tipo, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ObtenerJbeAsync_WithBusqueda_ReturnsFiltered()
    {
        // Arrange
        var tipo = 1;
        var busqueda = "JBE01";
        var expectedJbes = new List<JbeDto>
        {
            new JbeDto { Id = 1, Codigo = "JBE01", Descripcion = "JBE 1" }
        };

        _produccionServiceMock
            .Setup(x => x.ObtenerJbeAsync(tipo, busqueda, default))
            .ReturnsAsync(expectedJbes);

        // Act
        var result = await _produccionServiceMock.Object.ObtenerJbeAsync(tipo, busqueda);

        // Assert
        Assert.Single(result);
        Assert.Equal("JBE01", result[0].Codigo);
    }

    #endregion

    #region ObtenerProduccionAsync Tests

    [Fact]
    public async Task ObtenerProduccionAsync_WithDateRange_ReturnsProduccionRows()
    {
        // Arrange
        var fechaInicio = DateTime.Now.AddDays(-7);
        var fechaFin = DateTime.Now;
        var expectedProduccion = new List<ProduccionRowViewModel>
        {
            new ProduccionRowViewModel { Id = 1, Identificacion = "12345678", Unidad = 1, Actividad = 1, Cantidad = 5 },
            new ProduccionRowViewModel { Id = 2, Identificacion = "12345678", Unidad = 1, Actividad = 2, Cantidad = 3 }
        };

        _produccionServiceMock
            .Setup(x => x.ObtenerProduccionAsync(fechaInicio, fechaFin, null, null, default))
            .ReturnsAsync(expectedProduccion);

        // Act
        var result = await _produccionServiceMock.Object.ObtenerProduccionAsync(fechaInicio, fechaFin, null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ObtenerProduccionAsync_WithIdentificacion_ReturnsFiltered()
    {
        // Arrange
        var identificacion = "12345678";
        var expectedProduccion = new List<ProduccionRowViewModel>
        {
            new ProduccionRowViewModel { Id = 1, Identificacion = identificacion, Unidad = 1, Actividad = 1, Cantidad = 5 }
        };

        _produccionServiceMock
            .Setup(x => x.ObtenerProduccionAsync(null, null, identificacion, null, default))
            .ReturnsAsync(expectedProduccion);

        // Act
        var result = await _produccionServiceMock.Object.ObtenerProduccionAsync(null, null, identificacion, null);

        // Assert
        Assert.Single(result);
        Assert.Equal(identificacion, result[0].Identificacion);
    }

    [Fact]
    public async Task ObtenerProduccionAsync_NoData_ReturnsEmpty()
    {
        // Arrange
        _produccionServiceMock
            .Setup(x => x.ObtenerProduccionAsync(null, null, null, null, default))
            .ReturnsAsync(new List<ProduccionRowViewModel>());

        // Act
        var result = await _produccionServiceMock.Object.ObtenerProduccionAsync(null, null, null, null);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region GuardarRegistroAsync Tests

    [Fact]
    public async Task GuardarRegistroAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        var request = new GuardarRegistroRequest
        {
            Identificacion = "12345678",
            Unidad = 1,
            Actividad = 1,
            Cantidad = 5,
            Fecha = DateTime.Now
        };

        _produccionServiceMock
            .Setup(x => x.GuardarRegistroAsync(request, default))
            .ReturnsAsync(true);

        // Act
        var result = await _produccionServiceMock.Object.GuardarRegistroAsync(request);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GuardarRegistroAsync_WithZeroCantidad_ReturnsFalse()
    {
        // Arrange
        var request = new GuardarRegistroRequest
        {
            Identificacion = "12345678",
            Unidad = 1,
            Actividad = 1,
            Cantidad = 0,
            Fecha = DateTime.Now
        };

        _produccionServiceMock
            .Setup(x => x.GuardarRegistroAsync(request, default))
            .ReturnsAsync(false);

        // Act
        var result = await _produccionServiceMock.Object.GuardarRegistroAsync(request);

        // Assert
        Assert.False(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(10)]
    public async Task GuardarRegistroAsync_WithVariousCantidades_SavesSuccessfully(int cantidad)
    {
        // Arrange
        var request = new GuardarRegistroRequest
        {
            Identificacion = "12345678",
            Unidad = 1,
            Actividad = 1,
            Cantidad = cantidad,
            Fecha = DateTime.Now
        };

        _produccionServiceMock
            .Setup(x => x.GuardarRegistroAsync(request, default))
            .ReturnsAsync(true);

        // Act
        var result = await _produccionServiceMock.Object.GuardarRegistroAsync(request);

        // Assert
        Assert.True(result);
    }

    #endregion

    #region ObtenerResumenGeneralAsync Tests

    [Fact]
    public async Task ObtenerResumenGeneralAsync_ReturnsSummaryStats()
    {
        // Arrange
        var expectedSummary = new ProduccionSummary
        {
            TotalRegistros = 100,
            TotalCantidad = 250,
            PromedioActividad = 2.5m,
            FechaActualizacion = DateTime.Now
        };

        _produccionServiceMock
            .Setup(x => x.ObtenerResumenGeneralAsync(default))
            .ReturnsAsync(expectedSummary);

        // Act
        var result = await _produccionServiceMock.Object.ObtenerResumenGeneralAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(100, result.TotalRegistros);
        Assert.Equal(250, result.TotalCantidad);
    }

    #endregion
}

/// <summary>
/// Mock DTOs and models for testing
/// </summary>
public class UnidadDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}

public class ActividadDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}

public class JbeDto
{
    public int Id { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}

public class ProduccionRowViewModel
{
    public long Id { get; set; }
    public string Identificacion { get; set; } = string.Empty;
    public int Unidad { get; set; }
    public int Actividad { get; set; }
    public int Cantidad { get; set; }
}

public class GuardarRegistroRequest
{
    public string Identificacion { get; set; } = string.Empty;
    public int Unidad { get; set; }
    public int Actividad { get; set; }
    public int Cantidad { get; set; }
    public DateTime Fecha { get; set; }
}

public class ProduccionSummary
{
    public int TotalRegistros { get; set; }
    public int TotalCantidad { get; set; }
    public decimal PromedioActividad { get; set; }
    public DateTime FechaActualizacion { get; set; }
}
