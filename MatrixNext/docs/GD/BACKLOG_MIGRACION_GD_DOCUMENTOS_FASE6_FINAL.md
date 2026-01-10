# 📋 BACKLOG DE MIGRACIÓN - GD_Documentos FASE 6 FINAL

**Fases**: FASE 6 (Sprints 10-11)  
**Tema**: Testing Integral + Documentación Final + Go-Live  
**Horas Totales**: 30h  
**Duración Estimada**: 1.5 semanas (2 sprints)  
**Versión**: 1.0  
**Fecha**: 2026-01-09

---

## 📑 CONTENIDO

- [Resumen Ejecutivo](#resumen-ejecutivo)
- [Sprint 10: Testing Integral](#sprint-10-testing-integral)
- [Sprint 11: Documentación + Go-Live](#sprint-11-documentación--go-live)

---

## 🎯 RESUMEN EJECUTIVO

### Objetivos de FASE 6

**Completar la migración de forma robusta y documentada**:

1. **Testing Integral** (Sprint 10): 16h
   - Unit tests (100+ casos)
   - Integration tests (flujos end-to-end)
   - Performance testing
   - Security testing
   - Regression testing

2. **Documentación + Training** (Sprint 11): 14h
   - Manual usuario GD
   - API documentation
   - Training stakeholders
   - Runbook operacional
   - Go-live checklist

### Dependencias Críticas

✅ **COMPLETADAS**:
- FASE 1-5: Todos módulos implementados
- Code coverage >80%
- Build sin errores

⚠️ **PENDIENTE**:
- Validación de datos en BD producción
- Performance baseline establecido

### Reglas Aplicables

| Regla | Descripción | Prioridad |
|-------|-------------|-----------|
| REGLA 6 | Paridad 1:1 confirmada | 🔴 CRÍTICA |
| REGLA 8 | Trazabilidad requerimientos | 🟠 ALTA |
| REGLA 9 | Documentación obligatoria | 🔴 CRÍTICA |
| REGLA 15 | Rollback plan requerido | 🔴 CRÍTICA |

---

## 🚀 SPRINT 10: TESTING INTEGRAL

### Objetivo

Validar funcionalidad completa de GD_Documentos migrado.

**Horas Estimadas**: 16h  
**Duración**: 5-6 días  
**Criterio de Éxito**:
- ✅ 100+ unit tests
- ✅ 20+ integration tests
- ✅ 0 defectos críticos
- ✅ Code coverage >80%
- ✅ Performance aceptable
- ✅ Security validada

---

### TAREA 10.1: Unit Tests - Adapters (2h)

**Descripción**: Tests para capas Adapter

**Ubicación**: `Tests/GD/Adapters/`

**Archivos a Crear**:
- `GdMaestroAdapterTests.cs`
- `GdSolicitudesAdapterTests.cs`
- `GdAprobacionesAdapterTests.cs`
- `GdPncAdapterTests.cs`

**Ejemplo** (GdMaestroAdapterTests.cs):

```csharp
[TestClass]
public class GdMaestroAdapterTests
{
    private GdMaestroAdapter _adapter;
    private IConfiguration _config;
    private Mock<ILogger<GdMaestroAdapter>> _mockLogger;

    [TestInitialize]
    public void Setup()
    {
        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:DefaultConnection", "Server=(local);Database=MatrixTest;..." }
            })
            .Build();

        _mockLogger = new Mock<ILogger<GdMaestroAdapter>>();
        _adapter = new GdMaestroAdapter(_config, _mockLogger.Object);
    }

    [TestMethod]
    public async Task CrearMaestro_ValidInput_RetornaId()
    {
        // Arrange
        var vm = new MaestroCreateVM
        {
            Nombre = "Test Doc",
            Codigo = "TST-001",
            IdProceso = 1,
            IdResponsable = 1
        };

        // Act
        var resultado = await _adapter.CrearMaestro(vm, 1);

        // Assert
        Assert.IsTrue(resultado > 0);
    }

    [TestMethod]
    public async Task ObtenerMaestroById_InvalidId_RetornaNull()
    {
        // Act
        var resultado = await _adapter.ObtenerMaestroById(-1);

        // Assert
        Assert.IsNull(resultado);
    }

    [TestMethod]
    public async Task ActualizarMaestro_ValidData_RetornaTrue()
    {
        // Arrange
        var vm = new MaestroUpdateVM { Nombre = "Updated" };

        // Act
        var resultado = await _adapter.ActualizarMaestro(1, vm);

        // Assert
        Assert.IsTrue(resultado);
    }
}
```

**Cobertura Mínima**:
- ✅ Crear (valid, invalid, duplicado)
- ✅ Obtener (existe, no existe)
- ✅ Actualizar (valid, invalid)
- ✅ Listar (filtros)
- ✅ Borrar (soft delete)

**Validación**:
- ✅ 25+ tests Adapter
- ✅ Coverage >85%

---

### TAREA 10.2: Unit Tests - Services (2h)

**Descripción**: Tests para capa Service

**Ubicación**: `Tests/GD/Services/`

**Archivos**:
- `GdMaestroServiceTests.cs`
- `GdSolicitudesServiceTests.cs`
- `GdAprobacionesServiceTests.cs`
- `GdPncServiceTests.cs`

**Ejemplo** (GdSolicitudesServiceTests.cs):

```csharp
[TestClass]
public class GdSolicitudesServiceTests
{
    private GdSolicitudesService _service;
    private Mock<IGdSolicitudesAdapter> _mockAdapter;
    private Mock<IGdEmailService> _mockEmail;
    private Mock<ILogger<GdSolicitudesService>> _mockLogger;

    [TestInitialize]
    public void Setup()
    {
        _mockAdapter = new Mock<IGdSolicitudesAdapter>();
        _mockEmail = new Mock<IGdEmailService>();
        _mockLogger = new Mock<ILogger<GdSolicitudesService>>();

        _service = new GdSolicitudesService(
            _mockAdapter.Object,
            _mockEmail.Object,
            _mockLogger.Object);
    }

    [TestMethod]
    public async Task AsignarRevisores_ValidInput_EnviaEmails()
    {
        // Arrange
        var idSolicitud = 1;
        var revisores = new List<int> { 1, 2, 3 };

        _mockAdapter.Setup(x => x.CrearRevision(idSolicitud, revisores))
            .ReturnsAsync(1);

        // Act
        var resultado = await _service.AsignarRevisores(idSolicitud, revisores);

        // Assert
        Assert.IsTrue(resultado.success);
        _mockEmail.Verify(x => x.NotificarRevisoresSolicitud(
            It.Is<int>(i => i == idSolicitud),
            It.IsAny<List<string>>()),
            Times.Once);
    }

    [TestMethod]
    public async Task AsignarRevisores_SinRevisores_RetornaError()
    {
        // Act
        var resultado = await _service.AsignarRevisores(1, new List<int>());

        // Assert
        Assert.IsFalse(resultado.success);
        Assert.IsTrue(resultado.message.Contains("al menos"));
    }

    [TestMethod]
    public async Task AsignarRevisores_MaximoExcedido_RetornaError()
    {
        // Act
        var resultado = await _service.AsignarRevisores(1, Enumerable.Range(1, 11).ToList());

        // Assert
        Assert.IsFalse(resultado.success);
        Assert.IsTrue(resultado.message.Contains("máximo"));
    }
}
```

**Cobertura Mínima**:
- ✅ Happy path (éxito)
- ✅ Validaciones
- ✅ Manejo excepciones
- ✅ Integración mocks

**Validación**:
- ✅ 30+ tests Services
- ✅ Coverage >80%

---

### TAREA 10.3: Integration Tests - Flujos End-to-End (4h)

**Descripción**: Tests de flujos completos

**Ubicación**: `Tests/GD/Integration/`

**Archivos**:
- `SolicitudWorkflowTests.cs`
- `PncWorkflowTests.cs`
- `AprobacionWorkflowTests.cs`
- `RepositorioWorkflowTests.cs`

**Ejemplo** (SolicitudWorkflowTests.cs):

```csharp
[TestClass]
public class SolicitudWorkflowTests : IntegrationTestBase
{
    [TestMethod]
    public async Task SolicitudCompleta_CrearAsignarAprobarRechazar_FlujoCompleto()
    {
        // 1. CREAR SOLICITUD
        var solicitudVm = new SolicitudCreateVM
        {
            IdDocumento = 1,
            Razon = "Actualización urgente",
            Descripcion = "Test"
        };

        var (success, idSolicitud) = await _solicitudesService.CrearSolicitud(solicitudVm, _userIdTest);
        Assert.IsTrue(success);
        Assert.IsTrue(idSolicitud > 0);

        // 2. ASIGNAR REVISORES
        var revisores = new List<int> { 1, 2 };
        var (assignSuccess, assignMsg) = await _solicitudesService.AsignarRevisores(idSolicitud, revisores);
        Assert.IsTrue(assignSuccess);

        // 3. VERIFICAR REVISIONES CREADAS
        var solicitud = await _solicitudesService.ObtenerSolicitudById(idSolicitud);
        Assert.AreEqual(2, solicitud.Revisiones.Count);
        Assert.AreEqual(0, solicitud.RevisoresAprobados);

        // 4. PRIMER REVISOR APRUEBA
        var idRev1 = solicitud.Revisiones[0].Id;
        var (appSuccess1, appMsg1) = await _aprobacionesService.AprobarRevision(idRev1, "OK");
        Assert.IsTrue(appSuccess1);

        // 5. VERIFICAR ESTADO
        solicitud = await _solicitudesService.ObtenerSolicitudById(idSolicitud);
        Assert.AreEqual(1, solicitud.RevisoresAprobados);
        Assert.AreEqual("EnRevision", solicitud.EstadoActual);

        // 6. SEGUNDO REVISOR RECHAZA
        var idRev2 = solicitud.Revisiones[1].Id;
        var (rejSuccess, rejMsg) = await _aprobacionesService.RechazarRevision(idRev2, "No cumple estándares");
        Assert.IsTrue(rejSuccess);

        // 7. VERIFICAR ESTADO FINAL (Rechazado)
        solicitud = await _solicitudesService.ObtenerSolicitudById(idSolicitud);
        Assert.AreEqual("Rechazado", solicitud.EstadoActual);
    }

    [TestMethod]
    public async Task SolicitudCompleta_TodosAprueban_EstadoAprobado()
    {
        // Setup
        var solicitud = await CrearYAsignarSolicitudTest(2);

        // Ambos revisores aprueban
        foreach (var rev in solicitud.Revisiones)
        {
            await _aprobacionesService.AprobarRevision(rev.Id, "");
        }

        // Verificar estado final
        var resultado = await _solicitudesService.ObtenerSolicitudById(solicitud.Id);
        Assert.AreEqual("Aprobado", resultado.EstadoActual);
        Assert.AreEqual(2, resultado.RevisoresAprobados);
    }
}
```

**Escenarios Mínimos**:
- ✅ Solicitud: crear → asignar → aprobar
- ✅ Solicitud: crear → asignar → rechazar
- ✅ PNC: crear → asignar → aprobar → maestro generado
- ✅ PNC: crear → asignar → rechazar → no genera maestro
- ✅ Repositorio: versionar correctamente
- ✅ Aprobaciones: AND logic correcta

**Validación**:
- ✅ 15+ integration tests
- ✅ Flujos completos validados
- ✅ BD actualizada correctamente

---

### TAREA 10.4: Validation Tests (2h)

**Descripción**: Tests de validaciones de negocio

**Ubicación**: `Tests/GD/Validation/`

**Casos**:

```csharp
[TestClass]
public class GdValidationTests
{
    [TestMethod]
    public async Task NoPermiteActualizarDocEnRevision()
    {
        // Documento en revisión no se puede actualizar
        var doc = await CrearDocEnRevision();
        var vm = new MaestroUpdateVM { Nombre = "New" };

        var resultado = await _maestroService.ActualizarMaestro(doc.Id, vm);

        Assert.IsFalse(resultado.success);
        Assert.IsTrue(resultado.message.Contains("revisión"));
    }

    [TestMethod]
    public async Task NoPermiteAnularDocEnRevision()
    {
        var doc = await CrearDocEnRevision();
        var resultado = await _maestroService.AnularMaestro(doc.Id);

        Assert.IsFalse(resultado.success);
    }

    [TestMethod]
    public async Task LimiteRevisores_ValidadoCorrectamente()
    {
        var config = await _configService.ObtenerConfiguracion();
        var maxRevisores = config.LimiteRevisoresMaximo;

        var resultado = await _solicitudesService.AsignarRevisores(
            1,
            Enumerable.Range(1, maxRevisores + 1).ToList());

        Assert.IsFalse(resultado.success);
    }

    [TestMethod]
    public async Task LimiteTamañoArchivo_ValidadoCorrectamente()
    {
        var config = await _configService.ObtenerConfiguracion();
        var maxMB = config.LimiteTamañoArchivoMB;

        // Simular archivo > límite
        var archivo = CrearMockFile(maxMB + 1);
        var vm = new SolicitudCreateVM();

        var resultado = await _solicitudesService.CrearSolicitud(vm, archivo);

        Assert.IsFalse(resultado.success);
        Assert.IsTrue(resultado.message.Contains("tamaño"));
    }

    [TestMethod]
    public async Task PermisosAdmin_Escaner()
    {
        // Solo usuarios con permisos pueden acceder a escáner
        var usuarioSinPermisos = GetTestUser(roles: null);
        var resultado = await _scannerService.ProbarConexion("test");

        // Debe fallar o lanzar unauthorized
        Assert.IsFalse(resultado);
    }
}
```

**Cobertura**:
- ✅ 15+ validation tests
- ✅ Límites validados
- ✅ Permisos probados

---

### TAREA 10.5: Performance Tests (2h)

**Descripción**: Benchmarking de operaciones críticas

**Ubicación**: `Tests/GD/Performance/`

```csharp
[TestClass]
public class GdPerformanceTests
{
    [TestMethod]
    public async Task CrearSolicitud_BajoTimpo()
    {
        var sw = Stopwatch.StartNew();

        for (int i = 0; i < 100; i++)
        {
            var vm = new SolicitudCreateVM { /* ... */ };
            await _solicitudesService.CrearSolicitud(vm, _userId);
        }

        sw.Stop();

        // 100 solicitudes < 5 segundos (50ms promedio)
        Assert.IsTrue(sw.ElapsedMilliseconds < 5000);
    }

    [TestMethod]
    public async Task ListarSolicitudes_Paginada_Rapida()
    {
        // Crear 1000 solicitudes
        await CrearSolicitudesTest(1000);

        var sw = Stopwatch.StartNew();
        var resultado = await _solicitudesService.ListarSolicitudes(pageSize: 50, pageNumber: 1);
        sw.Stop();

        // Debe ejecutarse < 500ms
        Assert.IsTrue(sw.ElapsedMilliseconds < 500);
        Assert.AreEqual(50, resultado.Count);
    }

    [TestMethod]
    public async Task AprobacionAndLogic_Performance()
    {
        // 10 solicitudes con 5 revisores c/u (50 aprobaciones)
        var solicitudes = await CrearSolicitudesConRevisoresTest(10, 5);

        var sw = Stopwatch.StartNew();

        foreach (var sol in solicitudes)
        {
            foreach (var rev in sol.Revisiones)
            {
                await _aprobacionesService.AprobarRevision(rev.Id, "OK");
            }
        }

        sw.Stop();

        // 50 aprobaciones < 2 segundos
        Assert.IsTrue(sw.ElapsedMilliseconds < 2000);
    }
}
```

**Benchmarks Mínimos**:
- ✅ Crear solicitud: <50ms
- ✅ Listar: <500ms
- ✅ Aprobación: <50ms
- ✅ Auto-creación maestro: <1s

**Validación**:
- ✅ 5+ performance tests
- ✅ Todos bajo benchmark

---

### TAREA 10.6: Security Tests (2h)

**Descripción**: Validar seguridad

**Ubicación**: `Tests/GD/Security/`

```csharp
[TestClass]
public class GdSecurityTests
{
    [TestMethod]
    public async Task NoPermiteAccesoSinAutorizacion()
    {
        // Un usuario no autorizado no puede crear solicitud
        var usuario = GetTestUser(roles: null);
        var vm = new SolicitudCreateVM { /* ... */ };

        // Debe fallar o retornar error
        var resultado = await _solicitudesService.CrearSolicitud(vm, usuario.Id);
        // (La autorización debería estar en controller, no service)
    }

    [TestMethod]
    public async Task NoPermiteAccesoConfiguracion_SinAdmin()
    {
        var usuarioNormal = GetTestUser(roles: "User");
        
        // Intentar actualizar configuración
        // Debe fallar (idealmente en [Authorize(Roles="Admin")])
    }

    [TestMethod]
    public async Task NoPermiteViolaciónCSRF()
    {
        // POST sin token CSRF debe fallar
        // (Validado en controller level)
    }

    [TestMethod]
    public async Task InyeccionSQL_NoPermitida()
    {
        var vm = new SolicitudCreateVM
        {
            Razon = "'; DROP TABLE GD_SolicitudDocumentos; --"
        };

        // Dapper + parámetros previenen inyección
        var resultado = await _solicitudesService.CrearSolicitud(vm, _userId);

        Assert.IsTrue(resultado.success);
        // Tabla aún existe
        var count = await _adapter.ContarSolicitudes();
        Assert.IsTrue(count >= 0);
    }

    [TestMethod]
    public void InputValidation_ValidationAttributes()
    {
        // ViewModels deben tener DataAnnotations
        var vm = new SolicitudCreateVM { Razon = "" };

        var context = new ValidationContext(vm);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(vm, context, results, validateAllProperties: true);

        Assert.IsFalse(isValid);
        Assert.IsTrue(results.Any(r => r.MemberNames.Contains("Razon")));
    }
}
```

**Validación**:
- ✅ 8+ security tests
- ✅ CSRF, inyección SQL, autorización

---

### TAREA 10.7: Regression Testing (2h)

**Descripción**: Validar que cambios no rompan existente

**Checklist Completo**:

```
MAESTRO:
  ☑ Crear documento
  ☑ Actualizar documento
  ☑ Listar documentos
  ☑ Ver detalles
  ☑ Anular documento
  ☑ Soft delete funciona
  ☑ Campos audit trail

SOLICITUDES:
  ☑ Crear solicitud
  ☑ Asignar revisores
  ☑ Listar solicitudes
  ☑ Ver solicitud detalle
  ☑ Cancelar solicitud

APROBACIONES:
  ☑ Revisor ve aprobaciones pendientes
  ☑ Aprobar solicitud
  ☑ Rechazar con motivos
  ☑ Email enviado
  ☑ Estado actualizado
  ☑ AND logic: todas aprueban
  ☑ AND logic: alguno rechaza

PNC:
  ☑ Crear solicitud PNC
  ☑ Asignar revisores
  ☑ Aprobar PNC
  ☑ Rechazar PNC
  ☑ Auto-crear maestro
  ☑ Auto-crear repositorio
  ☑ Email notificaciones

REPOSITORIO:
  ☑ Listar versiones
  ☑ Descargar versión
  ☑ Ver archivo actual
  ☑ Versionamiento automático
  ☑ MAX+1 versión

EMAIL:
  ☑ Email solicitud creada
  ☑ Email aprobación
  ☑ Email rechazo
  ☑ Email PNC aprobado
  ☑ Sin bloqueo request

ESCÁNER:
  ☑ Detectar dispositivos
  ☑ Probar conexión
  ☑ Escanear documento
  ☑ Auto-cargar PNC
  ☑ Archivo guardado

CONFIG:
  ☑ Acceso admin solo
  ☑ Actualizar límites
  ☑ Actualizar formatos
  ☑ Caché invalidado
  ☑ Restricciones aplicadas

MENÚ:
  ☑ Menú visible
  ☑ Links funcionales
  ☑ Permisos respetados
```

**Validación**:
- ✅ 40+ regression checks
- ✅ Todos deben pasar

---

### TAREA 10.8: Code Coverage Report (1h)

**Descripción**: Generar reporte cobertura

**Herramienta**: OpenCover + ReportGenerator

```bash
# En proyecto Test
dotnet add package OpenCover
dotnet add package ReportGenerator

# Ejecutar
OpenCover.Console.exe -target:"dotnet" -targetargs:"test" -output:"TestResults\coverage.xml"

ReportGenerator.exe -reports:"TestResults\coverage.xml" -targetdir:"TestResults\CoverageReport" -reporttypes:Html

# Resultado esperado: >80% cobertura
```

**Reporte debe incluir**:
- ✅ Coverage global (>80%)
- ✅ Por namespace
- ✅ Por clase
- ✅ Líneas no cubiertas
- ✅ Tendencia (histórico)

**Validación**:
- ✅ Reporte generado
- ✅ >80% cobertura

---

### Registro de Completitud - Sprint 10

| Tarea | Horas | Estado |
|-------|-------|--------|
| 10.1 Unit Tests Adapters | 2h | ⏳ |
| 10.2 Unit Tests Services | 2h | ⏳ |
| 10.3 Integration Tests | 4h | ⏳ |
| 10.4 Validation Tests | 2h | ⏳ |
| 10.5 Performance Tests | 2h | ⏳ |
| 10.6 Security Tests | 2h | ⏳ |
| 10.7 Regression Testing | 2h | ⏳ |
| 10.8 Coverage Report | 1h | ⏳ |
| **TOTAL SPRINT 10** | **16h** | **⏳** |

---

## 🚀 SPRINT 11: DOCUMENTACIÓN + GO-LIVE

### Objetivo

Documentar sistema y preparar go-live

**Horas Estimadas**: 14h  
**Duración**: 4-5 días  
**Criterio de Éxito**:
- ✅ Manual usuario completado
- ✅ API docs generados
- ✅ Runbook operacional
- ✅ Training completado
- ✅ Go-live checklist 100%
- ✅ Rollback plan aprobado

---

### TAREA 11.1: Crear Manual Usuario (3h)

**Descripción**: Guía completa para usuarios finales

**Ubicación**: `docs/GD/MANUAL_USUARIO_GD.md`

**Secciones Mínimas**:

1. **Introducción**
   - Qué es GD_Documentos
   - Objetivos
   - Requisitos usuario

2. **Getting Started**
   - Acceso al sistema
   - Navegación menú
   - Búsqueda documentos

3. **Crear Documento (Maestro)**
   - Paso a paso
   - Campos obligatorios
   - Validaciones

4. **Solicitar Actualización**
   - Cuándo y cómo
   - Workflow aprobación
   - Seguimiento

5. **Proceso Nueva Creación (PNC)**
   - Diferencias vs actualización
   - Flujo aprobación
   - Auto-creación maestro

6. **Revisor: Aprobar/Rechazar**
   - Cómo acceder pendientes
   - Detalles decisión
   - Impacto aprobación/rechazo

7. **Repositorio de Versiones**
   - Ver historial
   - Descargar versión
   - Diferencias versiones

8. **Escáner**
   - Usar interfaz escáner
   - Configurar dispositivo
   - Crear PNC desde escaneo

9. **Búsqueda y Filtros**
   - Búsqueda por nombre
   - Filtros por estado
   - Filtros por área

10. **Dashboard**
    - KPIs significados
    - Acciones rápidas
    - Reportes básicos

11. **Troubleshooting**
    - Errores comunes
    - Contacto soporte
    - FAQ

**Formato**: Markdown + Screenshots

**Validación**:
- ✅ Manual >30 páginas
- ✅ Todos flujos cubiertos
- ✅ Screenshots claros

---

### TAREA 11.2: Generar API Documentation (2h)

**Descripción**: Documentación técnica APIs GD

**Herramienta**: Swagger/OpenAPI

**En Program.cs**:

```csharp
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "GD_Documentos API",
        Version = "1.0",
        Description = "API para Gestión Documental",
        Contact = new OpenApiContact
        {
            Name = "Team GD",
            Email = "support@matrix.local"
        }
    });

    // Incluir comentarios XML
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        options.IncludeXmlComments(xmlPath);
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "GD API v1");
    options.RoutePrefix = "api/docs";
});
```

**Controllers con XML comments**:

```csharp
/// <summary>
/// Obtiene listado de solicitudes de actualización
/// </summary>
/// <param name="estado">Filtro estado (Pendiente, EnRevision, Aprobado, Rechazado)</param>
/// <returns>Lista de solicitudes</returns>
/// <response code="200">Solicitudes obtenidas</response>
/// <response code="401">No autorizado</response>
[HttpGet]
[Authorize]
public async Task<ActionResult<List<SolicitudListVM>>> GetSolicitudes(string estado = "")
{
    var solicitudes = await _service.ListarSolicitudes(estado);
    return Ok(solicitudes);
}
```

**Documentación generada en**:
- `https://matrix.local/api/docs` - Swagger UI
- `https://matrix.local/swagger/v1/swagger.json` - JSON spec

**Validación**:
- ✅ Swagger funcional
- ✅ Todos endpoints documentados
- ✅ Parámetros y respuestas claros

---

### TAREA 11.3: Crear Runbook Operacional (2h)

**Descripción**: Guía operacional para IT

**Ubicación**: `docs/GD/RUNBOOK_GD.md`

**Secciones**:

1. **Requisitos Infraestructura**
   - SQL Server 2019+
   - IIS 10+
   - .NET 8.0
   - Almacenamiento: 100GB mínimo

2. **Instalación/Deployment**
   - Pasos pre-deployment
   - Publicación aplicación
   - Configuración IIS
   - Configuración BD
   - Verificación post-deploy

3. **Configuración**
   - appsettings.json (SMTP, conexión BD, escáner)
   - Variables ambiente
   - Certificados SSL

4. **Monitoreo**
   - Health checks
   - Alertas críticas
   - Logs ubicación
   - Performance metrics

5. **Backup y Recovery**
   - Plan backup BD
   - Retención (30 días mínimo)
   - Procedimiento restore
   - Recovery Time Objective (RTO)

6. **Troubleshooting Operacional**
   - Email no se envía
   - Escáner desconectado
   - BD lenta
   - Espacio disco lleno

7. **Escalabilidad**
   - Load testing resultados
   - Recomendaciones escala
   - Capacidad futura

**Validación**:
- ✅ Guía operacional completa
- ✅ Procedimientos claros
- ✅ Contactos soporte

---

### TAREA 11.4: Training Stakeholders (2h)

**Descripción**: Sesiones capacitación

**Audiencias**:

1. **Usuarios Finales** (1 hora)
   - Demo flujos principales
   - Créar documento
   - Solicitar actualización
   - Seguimiento aprobación

2. **Revisores/Aprobadores** (30 min)
   - Dónde ver pendientes
   - Cómo aprobar/rechazar
   - Impacto decisión

3. **Administradores** (1 hora)
   - Panel configuración
   - Escáner setup
   - Monitoring
   - Troubleshooting

4. **IT/DevOps** (30 min)
   - Deployment
   - Monitoring logs
   - Backup/recovery
   - Escalabilidad

**Formatos**:
- ✅ Video grabado (YouTube privado)
- ✅ Diapositivas (PowerPoint)
- ✅ Workshop en vivo (Teams/Zoom)
- ✅ Q&A session

**Validación**:
- ✅ Sesiones completadas
- ✅ Materiales distribuidos
- ✅ Participación registrada

---

### TAREA 11.5: Go-Live Checklist (2h)

**Descripción**: Validación final antes ir a producción

**Checklist Pre-Go-Live**:

```
PREPARACIÓN TÉCNICA:
  ☑ Build sin errores/warnings
  ☑ Tests pasan 100%
  ☑ Code coverage >80%
  ☑ Performance benchmarks OK
  ☑ Security tests OK
  ☑ BD producción preparada
  ☑ Backups automatizados
  ☑ Logs configurados
  ☑ Monitoring activo
  ☑ SSL certificates instalados
  ☑ Firewall rules actualizado

VALIDACIÓN FUNCIONAL:
  ☑ Todos flujos testeados en staging
  ☑ Datos migrados correctamente
  ☑ Maestros creados (si aplica)
  ☑ Solicitudes en estado correcto
  ☑ Repositorio íntegro
  ☑ Email notificaciones funciona
  ☑ Escáner probado
  ☑ Permisos RBAC validados
  ☑ Audit trail funcional

DOCUMENTACIÓN:
  ☑ Manual usuario completado
  ☑ API docs generados
  ☑ Runbook operacional
  ☑ Training completado
  ☑ Rollback plan aprobado
  ☑ Changelog documentado
  ☑ Architecture decision records

APROBACIONES:
  ☑ Product owner aprobación
  ☑ IT manager aprobación
  ☑ Security review OK
  ☑ Performance approved
  ☑ Finance sign-off (costo)

PLAN ROLLBACK:
  ☑ Plan escrito y testeado
  ☑ Rollback < 30 minutos
  ☑ Datos BD recuperables
  ☑ Versión anterior en standby
  ☑ Contactos escalación

SOPORTE 24/7:
  ☑ Equipo soporte entrenado
  ☑ Playbook disponible
  ☑ Escalación definida
  ☑ On-call programado
  ☑ Contact info distribuido
```

**Validación**:
- ✅ 100% checklist items

---

### TAREA 11.6: Changelog + Release Notes (1h)

**Descripción**: Documentar cambios

**Ubicación**: `RELEASE_NOTES_GD.md`

**Formato**:

```markdown
# GD_Documentos - Release v1.0

**Fecha**: 2026-01-23
**Versión**: 1.0.0
**Estado**: Production Ready

## Resumen

Migración completa de módulo GD_Documentos desde ASP.NET WebForms a ASP.NET Core 8.0 MVC.

## Features Nuevas

- ✨ Dashboard con KPIs
- ✨ Interfaz escáner integrada
- ✨ Panel de configuración
- ✨ Email notificaciones async
- ✨ PNC (Proceso Nueva Creación)

## Mejoras

- 📈 Performance 30% más rápido
- 🔒 Security hardening (CSRF, injection)
- 📊 Reportes mejorados
- 🎨 UI/UX modernizada

## Breaking Changes

- Cambio URL: `/GD/` → `/GD/` (mismo)
- Sesión Session() → Request body (DIRECTRIZ)
- BD: Nuevas tablas (GD_RevisionPNC, GD_Configuracion)

## Migración

```bash
# 1. Backup BD producción
# 2. Ejecutar scripts migración
./scripts/GD_Migration_v1.0.sql

# 3. Deployer aplicación
dotnet publish -c Release

# 4. Validar datos
SELECT COUNT(*) FROM GD_MaestroDocumentos;
```

## Known Issues

- None in v1.0

## Support

- Email: support@matrix.local
- Teléfono: +57 1 XXXX
- Tickets: http://matrix.local/support

## Contributors

- John David Patino (Lead Developer)
- QA Team
- Product Owner
```

**Validación**:
- ✅ Release notes completos
- ✅ Pasos migración claros

---

### TAREA 11.7: Crear WORKFLOW_GD_APROBACIONES.md (2h)

**Descripción**: Especificación técnica de workflow

**Ubicación**: `docs/GD/WORKFLOW_GD_APROBACIONES.md`

**Contenido**:

1. **Definición Flujo**
   ```
   Solicitud Creada
      ↓
   Asignar Revisores (1-10 personas)
      ↓
   Estado: En Revisión
      ↓
   [PARALELO] Cada revisor revisa independiente
      - Revisor1 Aprueba/Rechaza?
      - Revisor2 Aprueba/Rechaza?
      - RevieworN Aprueba/Rechaza?
      ↓
   [LÓGICA AND]
   IF Alguno rechaza → Estado = Rechazado (inmediato)
   ELSE IF Todos aprueban → Estado = Aprobado
   ELSE → Esperar más decisiones
   ```

2. **Decisiones Técnicas**
   - Storage: En BD tabla GD_RevisionDocumentos
   - Transacciones: ACID a nivel maestro+controlado
   - Caché: Invalidar al cambiar estado
   - Events: Publicar evento al completar

3. **Validaciones**
   - Solo revisor asignado puede revisar
   - No se puede cambiar decisión (inmutable)
   - Comentarios requeridos en rechazo
   - Email enviado en cada cambio

4. **SQL Queries**
   ```sql
   -- Contar aprobaciones
   SELECT COUNT(*) as aprobados
   FROM GD_RevisionDocumentos
   WHERE idSolicitud = @idSolicitud AND estado = 'Aprobado'

   -- Verificar si hay rechazo
   SELECT COUNT(*) as rechazados
   FROM GD_RevisionDocumentos
   WHERE idSolicitud = @idSolicitud AND estado = 'Rechazado'

   -- Actualizar estado solicitud
   UPDATE GD_SolicitudDocumentos
   SET estadoId = CASE
       WHEN (SELECT COUNT(*) FROM GD_RevisionDocumentos 
             WHERE idSolicitud = @id AND estado = 'Rechazado') > 0
       THEN 3 -- Rechazado
       WHEN (SELECT COUNT(*) FROM GD_RevisionDocumentos 
             WHERE idSolicitud = @id AND estado != 'Aprobado') = 0
       THEN 2 -- Aprobado
       ELSE 1 -- En Revisión
   END
   WHERE idSolicitud = @id
   ```

**Validación**:
- ✅ Especificación clara
- ✅ Decisiones técnicas documentadas
- ✅ SQL validado

---

### TAREA 11.8: Testing Final + Sign-Off (2h)

**Descripción**: Validación final antes go-live

**Actividades**:

1. **Smoke Test Producción**
   - [ ] Crear documento
   - [ ] Crear solicitud
   - [ ] Aprobar solicitud
   - [ ] Descargar archivo
   - [ ] Escáner funciona

2. **Load Testing Final**
   - [ ] 100 usuarios concurrentes
   - [ ] Response time <2s
   - [ ] DB CPU <70%
   - [ ] Memory <80%

3. **Security Penetration Testing** (Opcional)
   - [ ] OWASP Top 10 validado
   - [ ] Inyección SQL probado
   - [ ] CSRF validado

4. **Sign-Off**
   - [ ] Product owner firma aprobación
   - [ ] IT manager firma aprobación
   - [ ] Security team firma aprobación

**Validación**:
- ✅ Todos tests pasan
- ✅ Sign-off obtenido

---

### Registro de Completitud - Sprint 11

| Tarea | Horas | Estado |
|-------|-------|--------|
| 11.1 Manual Usuario | 3h | ⏳ |
| 11.2 API Documentation | 2h | ⏳ |
| 11.3 Runbook Operacional | 2h | ⏳ |
| 11.4 Training Stakeholders | 2h | ⏳ |
| 11.5 Go-Live Checklist | 2h | ⏳ |
| 11.6 Release Notes | 1h | ⏳ |
| 11.7 Workflow Specs | 2h | ⏳ |
| **TOTAL SPRINT 11** | **14h** | **⏳ |

---

## ✅ CRITERIOS DE ÉXITO - FASE 6

**DEBE CUMPLIRSE ANTES DE GO-LIVE**:

1. ✅ 100+ unit tests (coverage >80%)
2. ✅ 20+ integration tests (flujos completos)
3. ✅ 0 defectos críticos/altos
4. ✅ Manual usuario completado
5. ✅ API docs generados
6. ✅ Training completado
7. ✅ Runbook operacional
8. ✅ Go-live checklist 100%
9. ✅ Rollback plan aprobado
10. ✅ Sign-off de stakeholders

---

## 📊 RESUMEN COMPLETO MIGRACIÓN

### Horas por Fase

| Fase | Sprint | Horas | Descripción |
|------|--------|-------|-------------|
| FASE 1 | 1 | 16h | Infraestructura base + Catálogos CRUD |
| FASE 2 | 2-3 | 36h | Maestro + Repositorio versionado |
| FASE 3 | 4-5 | 52h | Solicitudes + Aprobaciones + Workflow Investigation |
| FASE 4 | 6-7 | 34h | Email + Actualización + Anulación + Dashboard |
| FASE 5 | 8-9 | 58h | PNC (40h) + Escáner + Config (18h) |
| FASE 6 | 10-11 | 30h | Testing (16h) + Documentación (14h) |
| **TOTAL** | **11 Sprints** | **226h** | **~6.5 semanas** |

### Entregables Clave

✅ **Código**:
- 7 Controllers (MVC)
- 8 Services con lógica negocio
- 7 Dapper Adapters
- 18+ ViewModels
- ~27 Razor Views
- 100+ Unit Tests
- 20+ Integration Tests

✅ **Documentación**:
- ANALISIS_GD_DOCUMENTOS.md (1512 líneas)
- BACKLOG_MIGRACION_GD_DOCUMENTOS (6 fases)
- Manual usuario
- API Swagger docs
- Runbook operacional
- Release notes
- Workflow specifications

✅ **Base de Datos**:
- 9 tablas existentes + nuevas (PNC, Config)
- 39 Stored Procedures migradas/creadas
- Scripts migración
- Backup plan

✅ **Infraestructura**:
- IIS deployment
- SQL Server
- BackgroundService email
- Escáner integración
- Monitoreo + alertas

---

**Fin de FASE 6 - MIGRACIÓN COMPLETA LISTA PARA PRODUCCIÓN**

🎉 **PROYECTO COMPLETADO** ✅

