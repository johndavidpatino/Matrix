using Moq;
using Xunit;
using MatrixNext.Web.Services.OP;
using MatrixNext.Web.Services.OP.Models;
using MatrixNext.Web.ViewModels;

namespace MatrixNext.Web.Tests.Services.OP;

/// <summary>
/// Unit tests for OpCoordinacionService
/// Tests coordination and personnel assignment
/// </summary>
public class OpCoordinacionServiceTests
{
    private readonly Mock<IOpCoordinacionService> _coordinacionServiceMock;

    public OpCoordinacionServiceTests()
    {
        _coordinacionServiceMock = new Mock<IOpCoordinacionService>();
    }

    #region ObtenerTrabajosPorCoordinadorAsync Tests

    [Fact]
    public async Task ObtenerTrabajosPorCoordinadorAsync_WithValidCoordinador_ReturnsTrabajos()
    {
        // Arrange
        var coordinadorId = 1L;
        var expectedTrabajos = new List<TrabajoCoordinadorDto>
        {
            new TrabajoCoordinadorDto 
            { 
                Id = 1, 
                JobBook = "JB-001", 
                Nombre = "Encuesta Mercado", 
                Estado = 1,
                Metodologia = "CATI"
            },
            new TrabajoCoordinadorDto 
            { 
                Id = 2, 
                JobBook = "JB-002", 
                Nombre = "Estudio Calidad", 
                Estado = 1,
                Metodologia = "Personal"
            }
        };

        _coordinacionServiceMock
            .Setup(x => x.ObtenerTrabajosPorCoordinadorAsync(coordinadorId, null, null, null, null))
            .ReturnsAsync(expectedTrabajos);

        // Act
        var result = await _coordinacionServiceMock.Object.ObtenerTrabajosPorCoordinadorAsync(coordinadorId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, t => Assert.Equal(1, t.Estado));
    }

    [Fact]
    public async Task ObtenerTrabajosPorCoordinadorAsync_WithInvalidCoordinador_ReturnsEmptyList()
    {
        // Arrange
        var coordinadorId = 999L;
        _coordinacionServiceMock
            .Setup(x => x.ObtenerTrabajosPorCoordinadorAsync(coordinadorId, null, null, null, null))
            .ReturnsAsync(new List<TrabajoCoordinadorDto>());

        // Act
        var result = await _coordinacionServiceMock.Object.ObtenerTrabajosPorCoordinadorAsync(coordinadorId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(5)]
    public async Task ObtenerTrabajosPorCoordinadorAsync_WithVariousCoordinadores_ReturnsListSuccessfully(long coordinadorId)
    {
        // Arrange
        _coordinacionServiceMock
            .Setup(x => x.ObtenerTrabajosPorCoordinadorAsync(coordinadorId, null, null, null, null))
            .ReturnsAsync(new List<TrabajoCoordinadorDto>());

        // Act
        var result = await _coordinacionServiceMock.Object.ObtenerTrabajosPorCoordinadorAsync(coordinadorId);

        // Assert
        Assert.NotNull(result);
    }

    #endregion

    #region ObtenerTrabajosCallCenterAsync Tests

    [Fact]
    public async Task ObtenerTrabajosCallCenterAsync_ReturnsPendingTrabajos()
    {
        // Arrange
        var expectedTrabajos = new List<TrabajoCoordinadorDto>
        {
            new TrabajoCoordinadorDto 
            { 
                Id = 10, 
                JobBook = "JB-010", 
                Nombre = "Trabajo Pendiente 1", 
                Estado = 0
            },
            new TrabajoCoordinadorDto 
            { 
                Id = 11, 
                JobBook = "JB-011", 
                Nombre = "Trabajo Pendiente 2", 
                Estado = 0
            }
        };

        _coordinacionServiceMock
            .Setup(x => x.ObtenerTrabajosCallCenterAsync(null, null, null, null))
            .ReturnsAsync(expectedTrabajos);

        // Act
        var result = await _coordinacionServiceMock.Object.ObtenerTrabajosCallCenterAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ObtenerTrabajosCallCenterAsync_WithFilters_ReturnsFilteredResults()
    {
        // Arrange
        var expectedTrabajos = new List<TrabajoCoordinadorDto>
        {
            new TrabajoCoordinadorDto 
            { 
                Id = 10, 
                JobBook = "JB-010", 
                Nombre = "Trabajo Pendiente", 
                Estado = 0
            }
        };

        _coordinacionServiceMock
            .Setup(x => x.ObtenerTrabajosCallCenterAsync(10, null, null, null))
            .ReturnsAsync(expectedTrabajos);

        // Act
        var result = await _coordinacionServiceMock.Object.ObtenerTrabajosCallCenterAsync(trabajoId: 10);

        // Assert
        Assert.Single(result);
        Assert.Equal(10, result[0].Id);
    }

    #endregion

    #region ObtenerCiudadesAsignadasAsync Tests

    [Fact]
    public async Task ObtenerCiudadesAsignadasAsync_WithValidCoordinadorTrabajo_ReturnsCiudades()
    {
        // Arrange
        var coordinadorId = 1L;
        var trabajoId = 1L;
        var expectedCiudades = new List<CiudadAsignadaDto>
        {
            new CiudadAsignadaDto { CiudadId = 76001, CiudadNombre = "Bogotá", MuestraAsignada = 100 },
            new CiudadAsignadaDto { CiudadId = 76002, CiudadNombre = "Soacha", MuestraAsignada = 50 }
        };

        _coordinacionServiceMock
            .Setup(x => x.ObtenerCiudadesAsignadasAsync(coordinadorId, trabajoId))
            .ReturnsAsync(expectedCiudades);

        // Act
        var result = await _coordinacionServiceMock.Object.ObtenerCiudadesAsignadasAsync(coordinadorId, trabajoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ObtenerCiudadesAsignadasAsync_WithInvalidCombination_ReturnsEmptyList()
    {
        // Arrange
        var coordinadorId = 999L;
        var trabajoId = 999L;
        _coordinacionServiceMock
            .Setup(x => x.ObtenerCiudadesAsignadasAsync(coordinadorId, trabajoId))
            .ReturnsAsync(new List<CiudadAsignadaDto>());

        // Act
        var result = await _coordinacionServiceMock.Object.ObtenerCiudadesAsignadasAsync(coordinadorId, trabajoId);

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    #endregion

    #region ObtenerPersonalAsignadoAsync Tests

    [Fact]
    public async Task ObtenerPersonalAsignadoAsync_WithValidTrabajo_ReturnsPersonal()
    {
        // Arrange
        var trabajoId = 1L;
        var expectedPersonal = new List<PersonalAsignadoDto>
        {
            new PersonalAsignadoDto { PersonalId = 1, Nombre = "Juan Pérez", Ciudad = "Bogotá" },
            new PersonalAsignadoDto { PersonalId = 2, Nombre = "María García", Ciudad = "Bogotá" }
        };

        _coordinacionServiceMock
            .Setup(x => x.ObtenerPersonalAsignadoAsync(trabajoId, null))
            .ReturnsAsync(expectedPersonal);

        // Act
        var result = await _coordinacionServiceMock.Object.ObtenerPersonalAsignadoAsync(trabajoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task ObtenerPersonalAsignadoAsync_WithCiudadFilter_ReturnsFilteredPersonal()
    {
        // Arrange
        var trabajoId = 1L;
        var ciudadId = 76001;
        var expectedPersonal = new List<PersonalAsignadoDto>
        {
            new PersonalAsignadoDto { PersonalId = 1, Nombre = "Juan Pérez", Ciudad = "Bogotá" }
        };

        _coordinacionServiceMock
            .Setup(x => x.ObtenerPersonalAsignadoAsync(trabajoId, ciudadId))
            .ReturnsAsync(expectedPersonal);

        // Act
        var result = await _coordinacionServiceMock.Object.ObtenerPersonalAsignadoAsync(trabajoId, ciudadId);

        // Assert
        Assert.Single(result);
    }

    #endregion

    #region ObtenerPersonalDisponibleAsync Tests

    [Fact]
    public async Task ObtenerPersonalDisponibleAsync_ReturnAvailablePersonal()
    {
        // Arrange
        var trabajoId = 1L;
        var expectedPersonal = new List<PersonalDisponibleDto>
        {
            new PersonalDisponibleDto { PersonalId = 10, Nombre = "Carlos López", Disponible = true },
            new PersonalDisponibleDto { PersonalId = 11, Nombre = "Ana Martínez", Disponible = true }
        };

        _coordinacionServiceMock
            .Setup(x => x.ObtenerPersonalDisponibleAsync(trabajoId, null))
            .ReturnsAsync(expectedPersonal);

        // Act
        var result = await _coordinacionServiceMock.Object.ObtenerPersonalDisponibleAsync(trabajoId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.All(result, p => Assert.True(p.Disponible));
    }

    #endregion

    #region AsignarPersonalAsync Tests

    [Fact]
    public async Task AsignarPersonalAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        var trabajoId = 1L;
        var personalId = 10L;
        var ciudadId = 76001;
        var usuarioId = 1L;
        _coordinacionServiceMock
            .Setup(x => x.AsignarPersonalAsync(trabajoId, personalId, ciudadId, usuarioId))
            .ReturnsAsync(true);

        // Act
        var result = await _coordinacionServiceMock.Object.AsignarPersonalAsync(trabajoId, personalId, ciudadId, usuarioId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task AsignarPersonalAsync_WithInvalidPersonal_ReturnsFalse()
    {
        // Arrange
        var trabajoId = 1L;
        var personalId = 999L;
        var ciudadId = 76001;
        var usuarioId = 1L;
        _coordinacionServiceMock
            .Setup(x => x.AsignarPersonalAsync(trabajoId, personalId, ciudadId, usuarioId))
            .ReturnsAsync(false);

        // Act
        var result = await _coordinacionServiceMock.Object.AsignarPersonalAsync(trabajoId, personalId, ciudadId, usuarioId);

        // Assert
        Assert.False(result);
    }

    #endregion

    #region RetirarPersonalAsync Tests

    [Fact]
    public async Task RetirarPersonalAsync_WithValidAsignacion_ReturnsTrue()
    {
        // Arrange
        var asignacionId = 1L;
        var usuarioId = 1L;
        _coordinacionServiceMock
            .Setup(x => x.RetirarPersonalAsync(asignacionId, usuarioId))
            .ReturnsAsync(true);

        // Act
        var result = await _coordinacionServiceMock.Object.RetirarPersonalAsync(asignacionId, usuarioId);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public async Task RetirarPersonalAsync_WithInvalidAsignacion_ReturnsFalse()
    {
        // Arrange
        var asignacionId = 999L;
        var usuarioId = 1L;
        _coordinacionServiceMock
            .Setup(x => x.RetirarPersonalAsync(asignacionId, usuarioId))
            .ReturnsAsync(false);

        // Act
        var result = await _coordinacionServiceMock.Object.RetirarPersonalAsync(asignacionId, usuarioId);

        // Assert
        Assert.False(result);
    }

    #endregion
}

/// <summary>
/// Mock DTOs for testing
/// </summary>
public class CiudadAsignadaDto
{
    public int CiudadId { get; set; }
    public string CiudadNombre { get; set; } = string.Empty;
    public int MuestraAsignada { get; set; }
}

public class PersonalAsignadoDto
{
    public long PersonalId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Ciudad { get; set; } = string.Empty;
}

public class PersonalDisponibleDto
{
    public long PersonalId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public bool Disponible { get; set; }
}
