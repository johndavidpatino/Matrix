using Xunit;
using Moq;
using MatrixNext.Web.Services.CORE;
using MatrixNext.Web.Infrastructure.Data;
using MatrixNext.Web.Models.CORE;
using MatrixNext.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MatrixNext.Tests.CORE.Services
{
    /// <summary>
    /// Suite de pruebas unitarias para validación de precedencias en GestionTareasService
    /// Verifica que las tareas solo cambien de estado cuando sus dependencias están completas
    /// </summary>
    public class GestionTareasServiceTests
    {
        private readonly Mock<MatrixDbContext> _mockDbContext;
        private readonly Mock<IAuditoriaService> _mockAuditoria;
        private readonly GestionTareasService _service;

        public GestionTareasServiceTests()
        {
            _mockDbContext = new Mock<MatrixDbContext>();
            _mockAuditoria = new Mock<IAuditoriaService>();
            _service = new GestionTareasService(_mockDbContext.Object, _mockAuditoria.Object);
        }

        #region Pruebas de precedencias válidas

        [Fact]
        public async Task CambiarEstado_ConPrecedenciasCompletadas_DebePermitirCambio()
        {
            // Arrange
            long idWorkFlow = 1;
            long idUsuario = 100;
            const string nuevoEstado = "Completada";
            const string observacion = "Trabajo finalizado";

            var tarea = new WorkFlow 
            { 
                Id = idWorkFlow, 
                Estado = "EnProgreso",
                IdTrabajo = "TRB001",
                FechaCreacion = DateTime.UtcNow
            };

            var tareasAntecesor = new List<WorkFlow>
            {
                new WorkFlow { Id = 10, Estado = "Completada", IdTrabajo = "TRB001" },
                new WorkFlow { Id = 11, Estado = "Completada", IdTrabajo = "TRB001" }
            };

            // Mock de datos
            var tareaSet = new Mock<DbSet<WorkFlow>>();
            tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == idWorkFlow)))
                .ReturnsAsync(tarea);

            var precedenciasSet = new Mock<DbSet<TareaPrecedencia>>();
            precedenciasSet.Setup(s => s.Where(It.IsAny<System.Linq.Expressions.Expression<Func<TareaPrecedencia, bool>>>()))
                .Returns(new List<TareaPrecedencia>
                {
                    new TareaPrecedencia { IdTareaSiguiente = idWorkFlow, IdTareaAnterior = 10 },
                    new TareaPrecedencia { IdTareaSiguiente = idWorkFlow, IdTareaAnterior = 11 }
                }.AsQueryable());

            _mockDbContext.Setup(c => c.WorkFlows).Returns(tareaSet.Object);
            _mockDbContext.Setup(c => c.TareaPrecedencias).Returns(precedenciasSet.Object);

            // Act
            var resultado = await _service.CambiarEstado(idWorkFlow, nuevoEstado, idUsuario, observacion);

            // Assert
            Assert.True(resultado.IsSuccess);
            Assert.Equal("Estado actualizado correctamente", resultado.Message);
            Assert.True(resultado.Data);

            // Verificar que se registró en auditoría
            _mockAuditoria.Verify(
                a => a.LogearAsync(It.IsAny<AuditoriaVM>()), 
                Times.Once,
                "Debe registrar la auditoría del cambio de estado"
            );
        }

        [Fact]
        public async Task CambiarEstado_ConTareaPrecesorAnulada_DebePermitirCambio()
        {
            // Arrange
            long idWorkFlow = 1;
            long idUsuario = 100;
            const string nuevoEstado = "EnProgreso";

            var tarea = new WorkFlow 
            { 
                Id = idWorkFlow, 
                Estado = "Creada",
                IdTrabajo = "TRB001",
                FechaCreacion = DateTime.UtcNow
            };

            // Una tarea anulada se considera como completada para precedencias
            var tareaAnulada = new WorkFlow { Id = 10, Estado = "Anulada", IdTrabajo = "TRB001" };

            var tareaSet = new Mock<DbSet<WorkFlow>>();
            tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == idWorkFlow)))
                .ReturnsAsync(tarea);

            _mockDbContext.Setup(c => c.WorkFlows).Returns(tareaSet.Object);

            // Act
            var resultado = await _service.ValidarPrecedenciasCompletadas(idWorkFlow);

            // Assert
            // Las tareas anuladas cuentan como precedencias satisfechas
            Assert.True(resultado);
        }

        #endregion

        #region Pruebas de precedencias inválidas

        [Fact]
        public async Task CambiarEstado_ConPrecedenciaPendiente_DebeRechazarCambio()
        {
            // Arrange
            long idWorkFlow = 1;
            long idUsuario = 100;
            const string nuevoEstado = "Completada";
            const string observacion = "Intentando cambiar";

            var tarea = new WorkFlow 
            { 
                Id = idWorkFlow, 
                Estado = "EnProgreso",
                IdTrabajo = "TRB001",
                FechaCreacion = DateTime.UtcNow
            };

            // Una tarea antecesora en estado "EnProgreso" (no completada ni anulada)
            var tareaAntecesorPendiente = new WorkFlow 
            { 
                Id = 10, 
                Estado = "EnProgreso", 
                IdTrabajo = "TRB001" 
            };

            var tareaSet = new Mock<DbSet<WorkFlow>>();
            tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == idWorkFlow)))
                .ReturnsAsync(tarea);
            tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == 10)))
                .ReturnsAsync(tareaAntecesorPendiente);

            var precedenciasSet = new Mock<DbSet<TareaPrecedencia>>();
            precedenciasSet.Setup(s => s.Where(It.IsAny<System.Linq.Expressions.Expression<Func<TareaPrecedencia, bool>>>()))
                .Returns(new List<TareaPrecedencia>
                {
                    new TareaPrecedencia { IdTareaSiguiente = idWorkFlow, IdTareaAnterior = 10 }
                }.AsQueryable());

            _mockDbContext.Setup(c => c.WorkFlows).Returns(tareaSet.Object);
            _mockDbContext.Setup(c => c.TareaPrecedencias).Returns(precedenciasSet.Object);

            // Act
            var resultado = await _service.CambiarEstado(idWorkFlow, nuevoEstado, idUsuario, observacion);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Contains("precedencia", resultado.Message.ToLower());
            Assert.False(resultado.Data);

            // Verificar que NO se registró en auditoría (no se permitió el cambio)
            _mockAuditoria.Verify(
                a => a.LogearAsync(It.IsAny<AuditoriaVM>()), 
                Times.Never,
                "No debe registrar auditoría si el cambio es rechazado"
            );
        }

        [Fact]
        public async Task CambiarEstado_ConMultiplesPrecedenciasPendientes_DebeListarTodas()
        {
            // Arrange
            long idWorkFlow = 5;
            long idUsuario = 100;
            const string nuevoEstado = "Completada";

            var tarea = new WorkFlow 
            { 
                Id = idWorkFlow, 
                Estado = "EnProgreso",
                IdTrabajo = "TRB002",
                FechaCreacion = DateTime.UtcNow
            };

            // Múltiples antecesoras pendientes
            var antecesoras = new List<WorkFlow>
            {
                new WorkFlow { Id = 1, Estado = "Creada", IdTrabajo = "TRB002" },
                new WorkFlow { Id = 2, Estado = "EnProgreso", IdTrabajo = "TRB002" },
                new WorkFlow { Id = 3, Estado = "Creada", IdTrabajo = "TRB002" }
            };

            var tareaSet = new Mock<DbSet<WorkFlow>>();
            tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == idWorkFlow)))
                .ReturnsAsync(tarea);

            foreach (var antecesora in antecesoras)
            {
                tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == antecesora.Id)))
                    .ReturnsAsync(antecesora);
            }

            var precedenciasSet = new Mock<DbSet<TareaPrecedencia>>();
            precedenciasSet.Setup(s => s.Where(It.IsAny<System.Linq.Expressions.Expression<Func<TareaPrecedencia, bool>>>()))
                .Returns(new List<TareaPrecedencia>
                {
                    new TareaPrecedencia { IdTareaSiguiente = idWorkFlow, IdTareaAnterior = 1 },
                    new TareaPrecedencia { IdTareaSiguiente = idWorkFlow, IdTareaAnterior = 2 },
                    new TareaPrecedencia { IdTareaSiguiente = idWorkFlow, IdTareaAnterior = 3 }
                }.AsQueryable());

            _mockDbContext.Setup(c => c.WorkFlows).Returns(tareaSet.Object);
            _mockDbContext.Setup(c => c.TareaPrecedencias).Returns(precedenciasSet.Object);

            // Act
            var tareasPrevias = await _service.ObtenerTareasPrevias(idWorkFlow);

            // Assert
            Assert.NotNull(tareasPrevias);
            Assert.Equal(3, tareasPrevias.TareasPendientes);
            Assert.NotEmpty(tareasPrevias.Tareas);
        }

        #endregion

        #region Pruebas de anulación

        [Fact]
        public async Task AnularTarea_SiempreDebePermitirse_InclusoConPrecedenciasPendientes()
        {
            // Arrange
            long idWorkFlow = 1;
            long idUsuario = 100;
            const string motivo = "Cambio de requerimientos";

            var tarea = new WorkFlow 
            { 
                Id = idWorkFlow, 
                Estado = "EnProgreso",
                IdTrabajo = "TRB001",
                FechaCreacion = DateTime.UtcNow
            };

            var tareaSet = new Mock<DbSet<WorkFlow>>();
            tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == idWorkFlow)))
                .ReturnsAsync(tarea);

            _mockDbContext.Setup(c => c.WorkFlows).Returns(tareaSet.Object);

            // Act
            // La anulación debe permitirse incluso si hay precedencias
            var resultado = await _service.CambiarEstado(idWorkFlow, "Anulada", idUsuario, motivo);

            // Assert
            Assert.True(resultado.IsSuccess);

            // Verificar auditoría
            _mockAuditoria.Verify(
                a => a.LogearAsync(It.IsAny<AuditoriaVM>()), 
                Times.Once
            );
        }

        #endregion

        #region Pruebas de validación básica

        [Fact]
        public async Task ValidarPrecedenciasCompletadas_ConTareaInexistente_DebeRetornarFalso()
        {
            // Arrange
            long idWorkFlowInexistente = 9999;

            var tareaSet = new Mock<DbSet<WorkFlow>>();
            tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == idWorkFlowInexistente)))
                .ReturnsAsync((WorkFlow)null);

            _mockDbContext.Setup(c => c.WorkFlows).Returns(tareaSet.Object);

            // Act
            var resultado = await _service.ValidarPrecedenciasCompletadas(idWorkFlowInexistente);

            // Assert
            Assert.False(resultado);
        }

        [Fact]
        public async Task ValidarPrecedenciasCompletadas_ConTareaSinPrecedencias_DebeRetornarVerdadero()
        {
            // Arrange
            long idWorkFlow = 1;

            var tarea = new WorkFlow 
            { 
                Id = idWorkFlow, 
                Estado = "Creada",
                IdTrabajo = "TRB001",
                FechaCreacion = DateTime.UtcNow
            };

            var tareaSet = new Mock<DbSet<WorkFlow>>();
            tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == idWorkFlow)))
                .ReturnsAsync(tarea);

            var precedenciasSet = new Mock<DbSet<TareaPrecedencia>>();
            precedenciasSet.Setup(s => s.Where(It.IsAny<System.Linq.Expressions.Expression<Func<TareaPrecedencia, bool>>>()))
                .Returns(new List<TareaPrecedencia>().AsQueryable());

            _mockDbContext.Setup(c => c.WorkFlows).Returns(tareaSet.Object);
            _mockDbContext.Setup(c => c.TareaPrecedencias).Returns(precedenciasSet.Object);

            // Act
            var resultado = await _service.ValidarPrecedenciasCompletadas(idWorkFlow);

            // Assert
            Assert.True(resultado);
        }

        #endregion

        #region Pruebas de errores y excepciones

        [Fact]
        public async Task CambiarEstado_ConErrorEnBD_DebeRetornarError()
        {
            // Arrange
            long idWorkFlow = 1;
            long idUsuario = 100;
            const string nuevoEstado = "Completada";

            var tareaSet = new Mock<DbSet<WorkFlow>>();
            tareaSet.Setup(s => s.FindAsync(It.IsAny<long>()))
                .ThrowsAsync(new InvalidOperationException("Error de conexión"));

            _mockDbContext.Setup(c => c.WorkFlows).Returns(tareaSet.Object);

            // Act
            var resultado = await _service.CambiarEstado(idWorkFlow, nuevoEstado, idUsuario, null);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Contains("Error", resultado.Message);
        }

        [Fact]
        public async Task CambiarEstado_ConEstadoInvalido_DebeRechazar()
        {
            // Arrange
            long idWorkFlow = 1;
            long idUsuario = 100;
            const string estadoInvalido = "EstadoNoExistente";

            var tarea = new WorkFlow 
            { 
                Id = idWorkFlow, 
                Estado = "Creada",
                IdTrabajo = "TRB001",
                FechaCreacion = DateTime.UtcNow
            };

            var tareaSet = new Mock<DbSet<WorkFlow>>();
            tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == idWorkFlow)))
                .ReturnsAsync(tarea);

            _mockDbContext.Setup(c => c.WorkFlows).Returns(tareaSet.Object);

            // Act
            var resultado = await _service.CambiarEstado(idWorkFlow, estadoInvalido, idUsuario, null);

            // Assert
            Assert.False(resultado.IsSuccess);
            Assert.Contains("estado", resultado.Message.ToLower());
        }

        #endregion

        #region Pruebas de auditoría

        [Fact]
        public async Task CambiarEstado_DebeRegistrarAuditoriaConDetallesCompletos()
        {
            // Arrange
            long idWorkFlow = 1;
            long idUsuario = 100;
            const string nuevoEstado = "Completada";
            const string observacion = "Trabajo finalizado correctamente";

            var tarea = new WorkFlow 
            { 
                Id = idWorkFlow, 
                Estado = "EnProgreso",
                IdTrabajo = "TRB001",
                FechaCreacion = DateTime.UtcNow
            };

            var tareaSet = new Mock<DbSet<WorkFlow>>();
            tareaSet.Setup(s => s.FindAsync(It.Is<long>(x => x == idWorkFlow)))
                .ReturnsAsync(tarea);

            var precedenciasSet = new Mock<DbSet<TareaPrecedencia>>();
            precedenciasSet.Setup(s => s.Where(It.IsAny<System.Linq.Expressions.Expression<Func<TareaPrecedencia, bool>>>()))
                .Returns(new List<TareaPrecedencia>().AsQueryable());

            _mockDbContext.Setup(c => c.WorkFlows).Returns(tareaSet.Object);
            _mockDbContext.Setup(c => c.TareaPrecedencias).Returns(precedenciasSet.Object);

            AuditoriaVM auditoriaCapturada = null;
            _mockAuditoria.Setup(a => a.LogearAsync(It.IsAny<AuditoriaVM>()))
                .Callback<AuditoriaVM>(vm => auditoriaCapturada = vm)
                .Returns(Task.CompletedTask);

            // Act
            await _service.CambiarEstado(idWorkFlow, nuevoEstado, idUsuario, observacion);

            // Assert
            Assert.NotNull(auditoriaCapturada);
            Assert.Equal("WorkFlow", auditoriaCapturada.Entidad);
            Assert.Equal(idWorkFlow, auditoriaCapturada.EntidadId);
            Assert.Contains("CambiarEstado", auditoriaCapturada.Accion);
            Assert.Contains(nuevoEstado, auditoriaCapturada.Detalles);
        }

        #endregion
    }
}
