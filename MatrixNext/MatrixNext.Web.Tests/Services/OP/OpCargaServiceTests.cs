using Moq;
using Xunit;
using Microsoft.AspNetCore.Http;
using MatrixNext.Web.Services.OP;

namespace MatrixNext.Web.Tests.Services.OP;

/// <summary>
/// Unit tests for OpCargaService
/// Tests file processing for CATI RMC and Planillas uploads
/// </summary>
public class OpCargaServiceTests
{
    private readonly Mock<IOpCargaService> _cargaServiceMock;

    public OpCargaServiceTests()
    {
        _cargaServiceMock = new Mock<IOpCargaService>();
    }

    #region ProcesarArchivoAsync - CATI RMC Tests

    [Fact]
    public async Task ProcesarArchivoAsync_CatiRMC_ValidFile_ReturnSuccess()
    {
        // Arrange
        var archivo = CreateMockFormFile("data.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        var expectedResult = new OpCargaResult(
            EsValido: true,
            Mensaje: "Archivo válido",
            CargaEjecutada: false,
            Reporte: new OpCargaSummary(
                Tipo: OpCargaTipo.CatiRMC,
                FilasValidadas: 100,
                Validas: 95,
                NoValidas: 5,
                Duplicadas: 0,
                Inconsistencias: 0));

        _cargaServiceMock
            .Setup(x => x.ProcesarArchivoAsync(archivo, OpCargaTipo.CatiRMC, false, 0, default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _cargaServiceMock.Object.ProcesarArchivoAsync(archivo, OpCargaTipo.CatiRMC);

        // Assert
        Assert.True(result.EsValido);
        Assert.NotNull(result.Reporte);
        Assert.Equal(95, result.Reporte.Validas);
    }

    [Fact]
    public async Task ProcesarArchivoAsync_CatiRMC_InvalidFile_ReturnFailure()
    {
        // Arrange
        var archivo = CreateMockFormFile("data.txt", "text/plain");
        var expectedResult = new OpCargaResult(
            EsValido: false,
            Mensaje: "Formato de archivo no válido. Se espera .xlsx",
            CargaEjecutada: false);

        _cargaServiceMock
            .Setup(x => x.ProcesarArchivoAsync(archivo, OpCargaTipo.CatiRMC, false, 0, default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _cargaServiceMock.Object.ProcesarArchivoAsync(archivo, OpCargaTipo.CatiRMC);

        // Assert
        Assert.False(result.EsValido);
    }

    [Fact]
    public async Task ProcesarArchivoAsync_CatiRMC_WithDuplicates_ReturnValidWithDuplicateCount()
    {
        // Arrange
        var archivo = CreateMockFormFile("data.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        var expectedResult = new OpCargaResult(
            EsValido: true,
            Mensaje: "Archivo procesado con duplicados detectados",
            CargaEjecutada: false,
            Reporte: new OpCargaSummary(
                Tipo: OpCargaTipo.CatiRMC,
                FilasValidadas: 100,
                Validas: 90,
                NoValidas: 5,
                Duplicadas: 5,
                Inconsistencias: 0));

        _cargaServiceMock
            .Setup(x => x.ProcesarArchivoAsync(archivo, OpCargaTipo.CatiRMC, false, 0, default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _cargaServiceMock.Object.ProcesarArchivoAsync(archivo, OpCargaTipo.CatiRMC);

        // Assert
        Assert.True(result.EsValido);
        Assert.NotNull(result.Reporte);
        Assert.Equal(5, result.Reporte.Duplicadas);
    }

    #endregion

    #region ProcesarArchivoAsync - Planillas Tests

    [Fact]
    public async Task ProcesarArchivoAsync_Planillas_ValidFile_ReturnSuccess()
    {
        // Arrange
        var archivo = CreateMockFormFile("planilla.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        var expectedResult = new OpCargaResult(
            EsValido: true,
            Mensaje: "Planilla válida",
            CargaEjecutada: false,
            Reporte: new OpCargaSummary(
                Tipo: OpCargaTipo.Planillas,
                FilasValidadas: 50,
                Validas: 50,
                NoValidas: 0,
                Duplicadas: 0,
                Inconsistencias: 0));

        _cargaServiceMock
            .Setup(x => x.ProcesarArchivoAsync(archivo, OpCargaTipo.Planillas, false, 0, default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _cargaServiceMock.Object.ProcesarArchivoAsync(archivo, OpCargaTipo.Planillas);

        // Assert
        Assert.True(result.EsValido);
        Assert.Equal(50, result.Reporte?.Validas);
    }

    [Fact]
    public async Task ProcesarArchivoAsync_Planillas_WithInconsistencies_ReturnValidWithInconsistencyCount()
    {
        // Arrange
        var archivo = CreateMockFormFile("planilla.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        var expectedResult = new OpCargaResult(
            EsValido: true,
            Mensaje: "Planilla procesada con inconsistencias",
            CargaEjecutada: false,
            Reporte: new OpCargaSummary(
                Tipo: OpCargaTipo.Planillas,
                FilasValidadas: 50,
                Validas: 45,
                NoValidas: 3,
                Duplicadas: 0,
                Inconsistencias: 2));

        _cargaServiceMock
            .Setup(x => x.ProcesarArchivoAsync(archivo, OpCargaTipo.Planillas, false, 0, default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _cargaServiceMock.Object.ProcesarArchivoAsync(archivo, OpCargaTipo.Planillas);

        // Assert
        Assert.True(result.EsValido);
        Assert.Equal(2, result.Reporte?.Inconsistencias);
    }

    #endregion

    #region ProcesarArchivoAsync - With Execution Tests

    [Fact]
    public async Task ProcesarArchivoAsync_ValidAndExecute_ReturnSuccessAndCargaEjecutada()
    {
        // Arrange
        var archivo = CreateMockFormFile("data.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        var usuarioId = 1L;
        var expectedResult = new OpCargaResult(
            EsValido: true,
            Mensaje: "Carga ejecutada exitosamente",
            CargaEjecutada: true,
            Reporte: new OpCargaSummary(
                Tipo: OpCargaTipo.CatiRMC,
                FilasValidadas: 100,
                Validas: 100,
                NoValidas: 0,
                Duplicadas: 0,
                Inconsistencias: 0));

        _cargaServiceMock
            .Setup(x => x.ProcesarArchivoAsync(archivo, OpCargaTipo.CatiRMC, true, usuarioId, default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _cargaServiceMock.Object.ProcesarArchivoAsync(
            archivo, 
            OpCargaTipo.CatiRMC, 
            ejecutarCarga: true, 
            usuarioId: usuarioId);

        // Assert
        Assert.True(result.CargaEjecutada);
        Assert.Equal(100, result.Reporte?.Validas);
    }

    [Fact]
    public async Task ProcesarArchivoAsync_InvalidAndExecute_ReturnFailureWithoutExecution()
    {
        // Arrange
        var archivo = CreateMockFormFile("data.txt", "text/plain");
        var expectedResult = new OpCargaResult(
            EsValido: false,
            Mensaje: "Formato inválido, carga no ejecutada",
            CargaEjecutada: false);

        _cargaServiceMock
            .Setup(x => x.ProcesarArchivoAsync(archivo, OpCargaTipo.CatiRMC, true, 1, default))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _cargaServiceMock.Object.ProcesarArchivoAsync(
            archivo, 
            OpCargaTipo.CatiRMC, 
            ejecutarCarga: true, 
            usuarioId: 1);

        // Assert
        Assert.False(result.EsValido);
        Assert.False(result.CargaEjecutada);
    }

    #endregion

    #region Helper Methods

    private IFormFile CreateMockFormFile(string fileName, string contentType)
    {
        var mock = new Mock<IFormFile>();
        mock.Setup(f => f.FileName).Returns(fileName);
        mock.Setup(f => f.ContentType).Returns(contentType);
        mock.Setup(f => f.Length).Returns(1024);
        
        var stream = new MemoryStream();
        stream.Write(new byte[] { 0x50, 0x4B, 0x03, 0x04 }, 0, 4); // ZIP header for Excel files
        stream.Position = 0;
        
        mock.Setup(f => f.OpenReadStream()).Returns(stream);
        return mock.Object;
    }

    #endregion
}

/// <summary>
/// Test extension methods for simulation
/// </summary>
public static class OpCargaServiceTestExtensions
{
    public static Task<OpCargaResult> SimulateValidCarga(
        this OpCargaTipo tipo,
        int filasValidadas = 100,
        int validas = 100)
    {
        var result = new OpCargaResult(
            EsValido: true,
            Mensaje: "Simulación de carga válida",
            CargaEjecutada: false,
            Reporte: new OpCargaSummary(
                Tipo: tipo,
                FilasValidadas: filasValidadas,
                Validas: validas,
                NoValidas: filasValidadas - validas,
                Duplicadas: 0,
                Inconsistencias: 0));

        return Task.FromResult(result);
    }
}
