using Xunit;
using Moq;
using MatrixNext.Data.Adapters.GD;
using MatrixNext.Data.Adapters.GD.Models;
using MatrixNext.Data.Services.GD;
using MatrixNext.Data.Services.GD.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Tests.GD.Services
{
    /// <summary>
    /// Suite de pruebas unitarias para GdSolicitudesService
    /// Valida lógica de negocio: REGLA 12 (validaciones), manejo de errores, async operations
    /// </summary>
    public class GdSolicitudesServiceTests
    {
        private readonly Mock<IGdSolicitudesAdapter> _mockAdapter;
        private readonly Mock<ILogger<GdSolicitudesService>> _mockLogger;
        private readonly IGdSolicitudesService _service;

        public GdSolicitudesServiceTests()
        {
            _mockAdapter = new Mock<IGdSolicitudesAdapter>();
            _mockLogger = new Mock<ILogger<GdSolicitudesService>>();
            _service = new GdSolicitudesService(_mockAdapter.Object, _mockLogger.Object);
        }

        #region ObtenerSolicitudes Tests

        [Fact]
        public async Task ObtenerSolicitudes_WithValidData_ReturnsSuccess()
        {
            // Arrange
            var solicitudes = new List<SolicitudListDto>
            {
                new SolicitudListDto 
                { 
                    Id = 1, 
                    NombreDocumento = "Documento 1", 
                    TipoSolicitud = "Lectura",
                    Solicitante = "Juan Pérez",
                    Estado = "Pendiente",
                    RevisoresPendientes = 2,
                    RevisoresAprobados = 0,
                    FechaRegistro = DateTime.Now.AddDays(-1)
                },
                new SolicitudListDto 
                { 
                    Id = 2, 
                    NombreDocumento = "Documento 2", 
                    TipoSolicitud = "Modificación",
                    Solicitante = "María García",
                    Estado = "En Revisión",
                    RevisoresPendientes = 1,
                    RevisoresAprobados = 1,
                    FechaRegistro = DateTime.Now.AddDays(-2)
                }
            };

            _mockAdapter.Setup(a => a.ObtenerSolicitudes())
                .ReturnsAsync(solicitudes);

            // Act
            var (success, data, message) = await _service.ObtenerSolicitudes();

            // Assert
            Assert.True(success);
            Assert.NotNull(data);
            Assert.Equal(2, data.Count);
            Assert.Equal("Documento 1", data[0].NombreDocumento);
            Assert.Contains("correctamente", message, StringComparison.OrdinalIgnoreCase);
            _mockAdapter.Verify(a => a.ObtenerSolicitudes(), Times.Once);
        }

        [Fact]
        public async Task ObtenerSolicitudes_WithEmptyList_ReturnsEmptyList()
        {
            // Arrange
            _mockAdapter.Setup(a => a.ObtenerSolicitudes())
                .ReturnsAsync(new List<SolicitudListDto>());

            // Act
            var (success, data, message) = await _service.ObtenerSolicitudes();

            // Assert
            Assert.True(success);
            Assert.Empty(data);
        }

        [Fact]
        public async Task ObtenerSolicitudes_WithException_ReturnsFail()
        {
            // Arrange
            _mockAdapter.Setup(a => a.ObtenerSolicitudes())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var (success, data, message) = await _service.ObtenerSolicitudes();

            // Assert
            Assert.False(success);
            Assert.NotNull(data); // Service returns empty list, not null
            Assert.Empty(data);
            Assert.Contains("error", message, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region ObtenerSolicitudById Tests

        [Fact]
        public async Task ObtenerSolicitudById_WithValidId_ReturnsDto()
        {
            // Arrange
            int idSolicitud = 1;
            var solicitud = new SolicitudDocumentoDto
            {
                Id = idSolicitud,
                TipoSolicitud = 1,
                IdDocumento = 5,
                IdSolicitante = 100,
                Area = "Sistemas",
                Cargo = "Analista",
                Razon = "Auditoría interna",
                Descripcion = "Revisar documentos de seguridad",
                IdEstado = 1,
                Comentarios = "Urgente",
                FechaRegistro = DateTime.Now.AddDays(-1)
            };

            _mockAdapter.Setup(a => a.ObtenerSolicitudById(idSolicitud))
                .ReturnsAsync(solicitud);

            // Act
            var (success, data, message) = await _service.ObtenerSolicitudById(idSolicitud);

            // Assert
            Assert.True(success);
            Assert.NotNull(data);
            Assert.Equal(idSolicitud, data.Id);
            Assert.Equal("Sistemas", data.Area);
        }

        [Fact]
        public async Task ObtenerSolicitudById_WithNullResult_ReturnsFail()
        {
            // Arrange
            int idSolicitud = 999;
            _mockAdapter.Setup(a => a.ObtenerSolicitudById(idSolicitud))
                .ReturnsAsync((SolicitudDocumentoDto?)null);

            // Act
            var (success, data, message) = await _service.ObtenerSolicitudById(idSolicitud);

            // Assert
            Assert.False(success);
            Assert.Null(data);
            Assert.Contains("no encontrada", message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task ObtenerSolicitudById_WithInvalidId_ReturnsFail()
        {
            // Arrange
            int invalidId = -1;

            // Act
            var (success, data, message) = await _service.ObtenerSolicitudById(invalidId);

            // Assert
            Assert.False(success);
            Assert.Null(data);
            Assert.Contains("válido", message, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region CrearSolicitud Tests

        [Fact]
        public async Task CrearSolicitud_WithValidInput_ReturnsNewId()
        {
            // Arrange
            var inputDto = new SolicitudCreateInputDto
            {
                TipoSolicitud = 1,
                IdDocumento = 5,
                IdSolicitante = 100,
                Area = "Sistemas",
                Cargo = "Analista",
                Razon = "Auditoría",
                Descripcion = "Revisar documentos de seguridad",
                IdEstado = 1
            };

            int newId = 42;
            _mockAdapter.Setup(a => a.CrearSolicitud(It.IsAny<SolicitudDocumentoDto>()))
                .ReturnsAsync(newId);

            // Act
            var (success, id, message) = await _service.CrearSolicitud(inputDto);

            // Assert
            Assert.True(success);
            Assert.Equal(newId, id);
            Assert.Contains("creada", message, StringComparison.OrdinalIgnoreCase);
            _mockAdapter.Verify(a => a.CrearSolicitud(It.IsAny<SolicitudDocumentoDto>()), Times.Once);
        }

        [Fact]
        public async Task CrearSolicitud_WithMissingRequiredField_ReturnsFail()
        {
            // Arrange
            var inputDto = new SolicitudCreateInputDto
            {
                TipoSolicitud = 0, // Invalid
                IdDocumento = 5,
                IdSolicitante = 100,
                Area = "Sistemas",
                Cargo = "Analista",
                Razon = "Auditoría",
                Descripcion = "Revisar documentos de seguridad",
                IdEstado = 1
            };

            // Act
            var (success, id, message) = await _service.CrearSolicitud(inputDto);

            // Assert
            Assert.False(success);
            Assert.Equal(0, id);
            Assert.Contains("Tipo de solicitud", message, StringComparison.OrdinalIgnoreCase);
            _mockAdapter.Verify(a => a.CrearSolicitud(It.IsAny<SolicitudDocumentoDto>()), Times.Never);
        }

        [Fact]
        public async Task CrearSolicitud_WithEmptyDescription_ReturnsFail()
        {
            // Arrange
            var inputDto = new SolicitudCreateInputDto
            {
                TipoSolicitud = 1,
                IdDocumento = 5,
                IdSolicitante = 100,
                Area = "Sistemas",
                Cargo = "Analista",
                Razon = "Auditoría",
                Descripcion = "", // Empty
                IdEstado = 1
            };

            // Act
            var (success, id, message) = await _service.CrearSolicitud(inputDto);

            // Assert
            Assert.False(success);
            Assert.Equal(0, id);
        }

        [Fact]
        public async Task CrearSolicitud_WithAdapterException_ReturnsFail()
        {
            // Arrange
            var inputDto = new SolicitudCreateInputDto
            {
                TipoSolicitud = 1,
                IdDocumento = 5,
                IdSolicitante = 100,
                Area = "Sistemas",
                Cargo = "Analista",
                Razon = "Auditoría",
                Descripcion = "Revisar documentos",
                IdEstado = 1
            };

            _mockAdapter.Setup(a => a.CrearSolicitud(It.IsAny<SolicitudDocumentoDto>()))
                .ThrowsAsync(new Exception("SP execution error"));

            // Act
            var (success, id, message) = await _service.CrearSolicitud(inputDto);

            // Assert
            Assert.False(success);
            Assert.Equal(0, id);
            Assert.Contains("error", message, StringComparison.OrdinalIgnoreCase);
        }

        #endregion

        #region AsignarRevisores Tests

        [Fact]
        public async Task AsignarRevisores_WithValidRevisores_ReturnsSuccess()
        {
            // Arrange
            int idSolicitud = 1;
            var idRevisores = new List<int> { 10, 20, 30 };
            var solicitud = new SolicitudDocumentoDto { Id = idSolicitud };

            _mockAdapter.Setup(a => a.ObtenerSolicitudById(idSolicitud))
                .ReturnsAsync(solicitud);

            _mockAdapter.Setup(a => a.CrearRevision(idSolicitud, It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(true);

            // Act
            var (success, message) = await _service.AsignarRevisores(idSolicitud, idRevisores);

            // Assert
            Assert.True(success);
            Assert.Contains("3", message);
            Assert.Contains("asignado", message, StringComparison.OrdinalIgnoreCase);
            _mockAdapter.Verify(a => a.CrearRevision(idSolicitud, It.IsAny<int>(), It.IsAny<int>()), Times.Exactly(3));
        }

        [Fact]
        public async Task AsignarRevisores_WithInvalidSolicitudId_ReturnsFail()
        {
            // Arrange
            int invalidId = -1;
            var idRevisores = new List<int> { 10 };

            // Act
            var (success, message) = await _service.AsignarRevisores(invalidId, idRevisores);

            // Assert
            Assert.False(success);
            Assert.Contains("válido", message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AsignarRevisores_WithEmptyRevisoresList_ReturnsFail()
        {
            // Arrange
            int idSolicitud = 1;
            var idRevisores = new List<int>();

            // Act
            var (success, message) = await _service.AsignarRevisores(idSolicitud, idRevisores);

            // Assert
            Assert.False(success);
            Assert.Contains("revisor", message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AsignarRevisores_WithNonexistentSolicitud_ReturnsFail()
        {
            // Arrange
            int idSolicitud = 999;
            var idRevisores = new List<int> { 10 };

            _mockAdapter.Setup(a => a.ObtenerSolicitudById(idSolicitud))
                .ReturnsAsync((SolicitudDocumentoDto?)null);

            // Act
            var (success, message) = await _service.AsignarRevisores(idSolicitud, idRevisores);

            // Assert
            Assert.False(success);
            Assert.Contains("no encontrada", message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public async Task AsignarRevisores_WithPartialFailures_ReturnsPartialSuccess()
        {
            // Arrange
            int idSolicitud = 1;
            var idRevisores = new List<int> { 10, 20, 30 };
            var solicitud = new SolicitudDocumentoDto { Id = idSolicitud };

            _mockAdapter.Setup(a => a.ObtenerSolicitudById(idSolicitud))
                .ReturnsAsync(solicitud);

            _mockAdapter.Setup(a => a.CrearRevision(idSolicitud, It.IsAny<int>(), 10))
                .ReturnsAsync(true);

            _mockAdapter.Setup(a => a.CrearRevision(idSolicitud, It.IsAny<int>(), 20))
                .ThrowsAsync(new Exception("Duplicate revision"));

            _mockAdapter.Setup(a => a.CrearRevision(idSolicitud, It.IsAny<int>(), 30))
                .ReturnsAsync(true);

            // Act
            var (success, message) = await _service.AsignarRevisores(idSolicitud, idRevisores);

            // Assert
            Assert.True(success); // 2 of 3 succeeded
            Assert.Contains("2 revisor(es) asignado(s) correctamente", message);
        }

        #endregion

        #region ObtenerFormData Tests

        [Fact]
        public async Task ObtenerFormData_WithAllDropdowns_ReturnsCompleteData()
        {
            // Arrange
            var tipos = new List<TipoSolicitudDto> 
            { 
                new TipoSolicitudDto { Id = 1, Nombre = "Lectura" },
                new TipoSolicitudDto { Id = 2, Nombre = "Modificación" }
            };

            var documentos = new List<MaestroListDto>
            {
                new MaestroListDto { Id = 1, Nombre = "Doc1" }
            };

            var usuarios = new List<UsuarioDto>
            {
                new UsuarioDto { Id = 10, Nombre = "Juan", Email = "juan@test.com" }
            };

            var estados = new List<EstadoSolicitudDto>
            {
                new EstadoSolicitudDto { Id = 1, Nombre = "Pendiente" }
            };

            _mockAdapter.Setup(a => a.ObtenerTiposSolicitud())
                .ReturnsAsync(tipos);

            _mockAdapter.Setup(a => a.ObtenerDocumentos())
                .ReturnsAsync(documentos);

            _mockAdapter.Setup(a => a.ObtenerUsuarios())
                .ReturnsAsync(usuarios);

            _mockAdapter.Setup(a => a.ObtenerEstados())
                .ReturnsAsync(estados);

            // Act
            var (success, data) = await _service.ObtenerFormData();

            // Assert
            Assert.True(success);
            Assert.NotNull(data);
            Assert.Equal(2, data.TiposSolicitud.Count);
            Assert.Single(data.Documentos);
            Assert.Single(data.Usuarios);
            Assert.Single(data.Estados);
        }

        [Fact]
        public async Task ObtenerFormData_WithException_ReturnsEmptyData()
        {
            // Arrange
            _mockAdapter.Setup(a => a.ObtenerTiposSolicitud())
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var (success, data) = await _service.ObtenerFormData();

            // Assert
            Assert.False(success);
            Assert.NotNull(data);
            Assert.Empty(data.TiposSolicitud);
        }

        #endregion
    }
}
