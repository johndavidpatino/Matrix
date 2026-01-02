# Plan de Migración - Módulo PY_Proyectos

**Módulo**: PY_Proyectos  
**Estado**: 🔜 Siguiente a Migrar (después de TH_Ausencias)  
**Prioridad**: 🟠 Alta  
**Complejidad**: 🟠 Media-Alta  

---

## 1. RESUMEN EJECUTIVO

**PY_Proyectos** es el módulo siguiente recomendado para migración dentro del roadmap. Se trata de la gestión central de proyectos en la organización, con 18 páginas .aspx bien estructuradas y dependencias relativamente bajas.

### Por qué PY_Proyectos después de TH_Ausencias:
1. **Dependencias claras**: Solo requiere US_Usuarios (ya migrado) y catálogos básicos
2. **Estructura CRUD simple**: Patrón repetible para acelerar aprendizaje
3. **Sin complejidades externas**: No depende de módulos pesados (FI, OP)
4. **Impacto alto**: Es funcionalidad central para operaciones
5. **Bajo riesgo**: Bien documentado en WebMatrix

---

## 2. ANÁLISIS DE ESTRUCTURA

### 2.1 Páginas Identificadas en WebMatrix

```
WebMatrix/PY_Proyectos/
├── Proyectos.aspx                      # CRUD Principal de Proyectos
├── ProyectosDetalle.aspx               # Vista detallada de proyecto
├── ProyectosActividades.aspx           # Gestión de actividades dentro de proyecto
├── ProyectosActivitiesGantt.aspx       # Diagrama Gantt (timeline)
├── ProyectosHitos.aspx                 # Hitos/milestones
├── ProyectosRecursos.aspx              # Asignación de recursos
├── ProyectosRiesgos.aspx               # Gestión de riesgos
├── ProyectosReporteMensual.aspx        # Reportes mensuales
├── ProyectosDocumentos.aspx            # Documentación del proyecto
├── ProyectosEquipo.aspx                # Equipo de proyecto
├── ProyectosPresupuesto.aspx           # Presupuesto y costos
├── ProyectosAcompañamiento.aspx        # Seguimiento/coaching
├── ProyectosOportunidades.aspx         # Registro de oportunidades
├── ProyectosDesviaciones.aspx          # Control de desviaciones
├── ProyectosEstado.aspx                # Estado del proyecto
├── ProyectosReporteFinal.aspx          # Reporte final de cierre
├── ProyectosComparativo.aspx           # Análisis comparativo
└── ProyectosExportacion.aspx           # Exportación de datos
```

### 2.2 Tablas Base de Datos Esperadas

```
Tablas Principales (PY_Model en CoreProject):
├── PY_Proyecto                    # Encabezado proyecto
│   ├── ID (PK)
│   ├── Nombre
│   ├── Descripcion
│   ├── Responsable (FK US_Usuarios)
│   ├── Jefe (FK US_Usuarios)
│   ├── FechaInicio
│   ├── FechaFin
│   ├── Estado (1=Planeación, 2=Activo, 3=Cerrado)
│   ├── Metodologia (FK)
│   ├── ClienteInterno (FK CU_Cuentas)
│   ├── Presupuesto
│   └── ...
│
├── PY_Actividad                   # Actividades del proyecto
│   ├── ID (PK)
│   ├── IdProyecto (FK PY_Proyecto)
│   ├── Nombre
│   ├── Descripcion
│   ├── Responsable (FK US_Usuarios)
│   ├── FechaInicio
│   ├── FechaFin
│   ├── Duracion (días)
│   ├── Estado
│   ├── PercentComplete
│   └── ...
│
├── PY_Hito                        # Milestones/checkpoints
│   ├── ID (PK)
│   ├── IdProyecto (FK)
│   ├── Nombre
│   ├── FechaTarget
│   ├── Estado
│   └── ...
│
├── PY_Recurso                     # Asignación de recursos
│   ├── ID (PK)
│   ├── IdProyecto (FK)
│   ├── IdEmpleado (FK US_Usuarios)
│   ├── Rol
│   ├── PercentAsignacion
│   ├── FechaInicio
│   ├── FechaFin
│   └── ...
│
├── PY_Riesgo                      # Gestión de riesgos
│   ├── ID (PK)
│   ├── IdProyecto (FK)
│   ├── Descripcion
│   ├── Probabilidad
│   ├── Impacto
│   ├── PlanMitigacion
│   ├── Responsable
│   └── ...
│
├── PY_Desviacion                  # Desvíos respecto a lo planeado
│   ├── ID (PK)
│   ├── IdProyecto (FK)
│   ├── Tipo (Tiempo/Costo/Alcance/Calidad)
│   ├── Descripcion
│   ├── Impacto
│   ├── AccionCorrectiva
│   └── ...
│
├── PY_Oportunidad                 # Oportunidades de mejora/beneficio
│   ├── ID (PK)
│   ├── IdProyecto (FK)
│   ├── Descripcion
│   ├── ValorEstimado
│   ├── Responsable
│   └── ...
│
├── PY_Documento                   # Documentación
│   ├── ID (PK)
│   ├── IdProyecto (FK)
│   ├── Nombre
│   ├── Ruta (archivo)
│   ├── TipoDocumento
│   ├── FechaSubida
│   └── ...
│
├── PY_Presupuesto                 # Detalles de presupuesto
│   ├── ID (PK)
│   ├── IdProyecto (FK)
│   ├── Concepto
│   ├── MontoPlaneado
│   ├── MontoPagado
│   ├── MontoComprometido
│   └── ...
│
├── PY_Metodologia                 # Catálogo de metodologías
│   ├── ID (PK)
│   ├── Nombre (Waterfall, Agile, PMI, etc.)
│   └── ...
│
└── PY_Equipos                     # Equipo del proyecto
    ├── ID (PK)
    ├── IdProyecto (FK)
    ├── IdEmpleado (FK)
    ├── Rol
    └── ...
```

### 2.3 Procedimientos Almacenados Esperados

```sql
-- CRUD Proyectos
PY_Proyecto_Insert        -- Crear proyecto
PY_Proyecto_Update        -- Actualizar proyecto
PY_Proyecto_Delete        -- Eliminar proyecto
PY_Proyecto_GetById       -- Obtener por ID
PY_Proyecto_GetAll        -- Listar todos
PY_Proyecto_GetByUser     -- Proyectos del usuario
PY_Proyecto_GetActivos    -- Proyectos activos

-- CRUD Actividades
PY_Actividad_Insert       -- Crear actividad
PY_Actividad_Update       -- Actualizar actividad
PY_Actividad_Delete       -- Eliminar actividad
PY_Actividad_GetByProyecto -- Listar actividades de proyecto
PY_Actividad_UpdateProgress -- Actualizar % completado

-- CRUD Hitos
PY_Hito_Insert            -- Crear hito
PY_Hito_Update            -- Actualizar hito
PY_Hito_GetByProyecto     -- Listar hitos

-- CRUD Recursos
PY_Recurso_AsignarEmpleado   -- Asignar empleado
PY_Recurso_DesasignarEmpleado -- Desasignar
PY_Recurso_GetAsignados      -- Obtener asignados

-- CRUD Riesgos
PY_Riesgo_Insert          -- Crear riesgo
PY_Riesgo_Update          -- Actualizar riesgo
PY_Riesgo_GetByProyecto   -- Listar riesgos

-- Reportes
PY_REP_ProyectosActivos   -- Estado de proyectos
PY_REP_ProgressGantt      -- Datos para Gantt
PY_REP_RecursosAsignados  -- Utilización de recursos
PY_REP_RiesgosAbiertos    -- Riesgos sin resolver
PY_REP_RiesgosResueltos   -- Histórico de riesgos
PY_REP_Presupuesto        -- Análisis de presupuesto
PY_REP_DesviacionesAbiertas -- Desvíos activos
```

---

## 3. FASE 1: ESTRUCTURA BASE (Semana 1)

### 3.1 Crear Carpetas y Archivos Base

```powershell
# En MatrixNext.Data/Modules/PY/Proyectos/
mkdir Models
mkdir Adapters
mkdir Services

# En MatrixNext.Web/Areas/PY/
mkdir Controllers
mkdir Views/Proyectos
mkdir Views/Actividades
mkdir Views/Recursos
mkdir Views/Riesgos
```

### 3.2 Crear ViewModels Base

```csharp
// MatrixNext.Data/Modules/PY/Proyectos/Models/

// 1. ProyectoViewModel.cs
public class ProyectoViewModel
{
    public long Id { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public long ResponsableId { get; set; }
    public string NombreResponsable { get; set; }
    public long JefeId { get; set; }
    public string NombreJefe { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public byte? Estado { get; set; }
    public string EstadoNombre { get; set; }
    public long? MetodologiaId { get; set; }
    public string MetodologiaNombre { get; set; }
    public decimal Presupuesto { get; set; }
    public decimal PresupuestoUtilizado { get; set; }
    public byte PercentComplete { get; set; }
    public DateTime FechaRegistro { get; set; }
}

// 2. ActividadViewModel.cs
public class ActividadViewModel
{
    public long Id { get; set; }
    public long IdProyecto { get; set; }
    public string NombreProyecto { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public long ResponsableId { get; set; }
    public string NombreResponsable { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
    public int Duracion { get; set; }
    public byte Estado { get; set; }
    public byte PercentComplete { get; set; }
    public int Secuencia { get; set; }
}

// 3. HitoViewModel.cs
public class HitoViewModel
{
    public long Id { get; set; }
    public long IdProyecto { get; set; }
    public string Nombre { get; set; }
    public DateTime? FechaTarget { get; set; }
    public DateTime? FechaReal { get; set; }
    public byte Estado { get; set; }
    public string Descripcion { get; set; }
}

// 4. RecursoViewModel.cs
public class RecursoViewModel
{
    public long Id { get; set; }
    public long IdProyecto { get; set; }
    public long IdEmpleado { get; set; }
    public string NombreEmpleado { get; set; }
    public string Rol { get; set; }
    public byte PercentAsignacion { get; set; }
    public DateTime? FechaInicio { get; set; }
    public DateTime? FechaFin { get; set; }
}

// 5. RiesgoViewModel.cs
public class RiesgoViewModel
{
    public long Id { get; set; }
    public long IdProyecto { get; set; }
    public string Descripcion { get; set; }
    public byte Probabilidad { get; set; }
    public byte Impacto { get; set; }
    public byte Criticidad { get; set; }
    public string PlanMitigacion { get; set; }
    public long ResponsableId { get; set; }
    public string NombreResponsable { get; set; }
    public byte Estado { get; set; }
}
```

### 3.3 Crear DataAdapter Base

```csharp
// MatrixNext.Data/Modules/PY/Proyectos/Adapters/ProyectoDataAdapter.cs

using System;
using System.Collections.Generic;
using Dapper;
using MatrixNext.Data.Entities;
using MatrixNext.Data.Modules.PY.Proyectos.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace MatrixNext.Data.Modules.PY.Proyectos.Adapters
{
    public class ProyectoDataAdapter
    {
        private readonly string _connectionString;

        public ProyectoDataAdapter(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("MatrixDb") ??
                throw new ArgumentNullException(nameof(configuration), "MatrixDb connection string not found");
        }

        #region INSERT - Proyectos

        public long CrearProyecto(string nombre, string descripcion, long responsableId, 
            long jefeId, DateTime? fechaInicio, DateTime? fechaFin, long? metodologiaId, decimal presupuesto)
        {
            using var context = new MatrixDbContext(_connectionString);
            
            // Implementar INSERT via EF Core o SP
            // Retornar ID generado
            
            throw new NotImplementedException();
        }

        #endregion

        #region UPDATE - Proyectos

        public bool ActualizarProyecto(long id, string nombre, string descripcion, 
            long responsableId, long jefeId, byte? estado, byte percentComplete)
        {
            // Implementar UPDATE
            throw new NotImplementedException();
        }

        #endregion

        #region SELECT - Proyectos

        public ProyectoViewModel ObtenerPorId(long id)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var dp = new DynamicParameters();
            dp.Add("@id", id);
            
            return connection.QueryFirstOrDefault<ProyectoViewModel>(
                "PY_Proyecto_GetById", dp, commandType: CommandType.StoredProcedure);
        }

        public List<ProyectoViewModel> ObtenerTodos()
        {
            using var connection = new SqlConnection(_connectionString);
            
            return connection.Query<ProyectoViewModel>(
                "PY_Proyecto_GetAll", commandType: CommandType.StoredProcedure).ToList();
        }

        public List<ProyectoViewModel> ObtenerActivos()
        {
            using var connection = new SqlConnection(_connectionString);
            
            return connection.Query<ProyectoViewModel>(
                "PY_Proyecto_GetActivos", commandType: CommandType.StoredProcedure).ToList();
        }

        public List<ProyectoViewModel> ObtenerPorResponsable(long idResponsable)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var dp = new DynamicParameters();
            dp.Add("@idResponsable", idResponsable);
            
            return connection.Query<ProyectoViewModel>(
                "PY_Proyecto_GetByUser", dp, commandType: CommandType.StoredProcedure).ToList();
        }

        #endregion

        #region ACTIVIDADES

        public List<ActividadViewModel> ObtenerActividadesPorProyecto(long idProyecto)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var dp = new DynamicParameters();
            dp.Add("@idProyecto", idProyecto);
            
            return connection.Query<ActividadViewModel>(
                "PY_Actividad_GetByProyecto", dp, commandType: CommandType.StoredProcedure).ToList();
        }

        #endregion

        #region RECURSOS

        public List<RecursoViewModel> ObtenerRecursosPorProyecto(long idProyecto)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var dp = new DynamicParameters();
            dp.Add("@idProyecto", idProyecto);
            
            return connection.Query<RecursoViewModel>(
                "PY_Recurso_GetByProyecto", dp, commandType: CommandType.StoredProcedure).ToList();
        }

        #endregion

        #region RIESGOS

        public List<RiesgoViewModel> ObtenerRiesgosPorProyecto(long idProyecto)
        {
            using var connection = new SqlConnection(_connectionString);
            
            var dp = new DynamicParameters();
            dp.Add("@idProyecto", idProyecto);
            
            return connection.Query<RiesgoViewModel>(
                "PY_Riesgo_GetByProyecto", dp, commandType: CommandType.StoredProcedure).ToList();
        }

        #endregion
    }
}
```

### 3.4 Crear Service Base

```csharp
// MatrixNext.Data/Modules/PY/Proyectos/Services/ProyectoService.cs

using System;
using System.Collections.Generic;
using MatrixNext.Data.Modules.PY.Proyectos.Adapters;
using MatrixNext.Data.Modules.PY.Proyectos.Models;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Data.Modules.PY.Proyectos.Services
{
    public class ProyectoService
    {
        private readonly ProyectoDataAdapter _adapter;
        private readonly ILogger<ProyectoService> _logger;

        public ProyectoService(ProyectoDataAdapter adapter, ILogger<ProyectoService> logger)
        {
            _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public (bool success, string message, long id) CrearProyecto(string nombre, 
            string descripcion, long responsableId, long jefeId, DateTime? fechaInicio, 
            DateTime? fechaFin, long? metodologiaId, decimal presupuesto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(nombre))
                    return (false, "El nombre del proyecto es requerido", 0);

                if (responsableId <= 0 || jefeId <= 0)
                    return (false, "Responsable y Jefe son requeridos", 0);

                if (fechaInicio > fechaFin)
                    return (false, "Fecha de inicio no puede ser mayor que fecha fin", 0);

                var id = _adapter.CrearProyecto(nombre, descripcion, responsableId, jefeId, 
                    fechaInicio, fechaFin, metodologiaId, presupuesto);

                _logger.LogInformation($"Proyecto creado: ID={id}, Nombre={nombre}");
                return (true, "Proyecto creado correctamente", id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando proyecto");
                return (false, $"Error: {ex.Message}", 0);
            }
        }

        public (bool success, ProyectoViewModel data) ObtenerPorId(long id)
        {
            try
            {
                var data = _adapter.ObtenerPorId(id);
                return (data != null, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error obteniendo proyecto: {id}");
                return (false, null);
            }
        }

        public (bool success, List<ProyectoViewModel> data) ObtenerActivos()
        {
            try
            {
                var data = _adapter.ObtenerActivos();
                return (true, data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo proyectos activos");
                return (false, new List<ProyectoViewModel>());
            }
        }
    }
}
```

### 3.5 Crear Controller Base

```csharp
// MatrixNext.Web/Areas/PY/Controllers/ProyectosController.cs

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using MatrixNext.Data.Modules.PY.Proyectos.Models;
using MatrixNext.Data.Modules.PY.Proyectos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Areas.PY.Controllers
{
    [Area("PY")]
    [Route("PY/Proyectos")]
    [Authorize]
    public class ProyectosController : Controller
    {
        private readonly ProyectoService _proyectoService;
        private readonly ILogger<ProyectosController> _logger;

        public ProyectosController(ProyectoService proyectoService, ILogger<ProyectosController> logger)
        {
            _proyectoService = proyectoService ?? throw new ArgumentNullException(nameof(proyectoService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private long GetCurrentUserId()
        {
            var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? User?.FindFirst("Id")?.Value;
            if (long.TryParse(idClaim, out var id))
            {
                return id;
            }
            throw new InvalidOperationException("Id de usuario autenticado no disponible");
        }

        // DTOs
        public class CrearProyectoRequest
        {
            public string Nombre { get; set; }
            public string Descripcion { get; set; }
            public long ResponsableId { get; set; }
            public long JefeId { get; set; }
            public DateTime? FechaInicio { get; set; }
            public DateTime? FechaFin { get; set; }
            public long? MetodologiaId { get; set; }
            public decimal Presupuesto { get; set; }
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var (success, proyectos) = await Task.FromResult(
                    _proyectoService.ObtenerActivos()
                );

                return View(proyectos ?? new List<ProyectoViewModel>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading proyectos");
                return View(new List<ProyectoViewModel>());
            }
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View(new CrearProyectoRequest());
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CrearProyectoRequest request)
        {
            try
            {
                var (success, message, id) = await Task.FromResult(
                    _proyectoService.CrearProyecto(
                        request.Nombre,
                        request.Descripcion,
                        request.ResponsableId,
                        request.JefeId,
                        request.FechaInicio,
                        request.FechaFin,
                        request.MetodologiaId,
                        request.Presupuesto
                    )
                );

                return Json(new { success, message, id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating proyecto");
                return Json(new { success = false, message = ex.Message, id = 0L });
            }
        }
    }
}
```

### 3.6 Registrar DI

```csharp
// En ServiceCollectionExtensions.cs

public static IServiceCollection AddPYModule(this IServiceCollection services)
{
    services.AddScoped<ProyectoDataAdapter>();
    services.AddScoped<ProyectoService>();
    // Agregar más adapters y services conforme se avance
    return services;
}
```

```csharp
// En Program.cs
builder.Services.AddPYModule();
```

---

## 4. FASE 2: CRUD COMPLETO (Semanas 2-3)

### 4.1 Completar AusenciaDataAdapter con Métodos CRUD
- [ ] CrearActividad, ActualizarActividad, EliminarActividad
- [ ] CrearHito, ActualizarHito, EliminarHito
- [ ] AsignarRecurso, DesasignarRecurso
- [ ] CrearRiesgo, ActualizarRiesgo, EliminarRiesgo
- [ ] CrearDesviacion, ActualizarDesviacion

### 4.2 Completar ProyectoService
- [ ] Métodos para todas las operaciones del adapter
- [ ] Validaciones de negocio
- [ ] Logging exhaustivo

### 4.3 Completar Controllers
- [ ] ProyectosController (CRUD completo)
- [ ] ActividadesController (CRUD dentro de proyecto)
- [ ] HitosController (CRUD hitos)
- [ ] RecursosController (Asignación/desasignación)
- [ ] RiesgosController (CRUD riesgos)

### 4.4 Crear Views
- [ ] Proyectos/Index.cshtml
- [ ] Proyectos/Create.cshtml
- [ ] Proyectos/Edit.cshtml
- [ ] Proyectos/Details.cshtml
- [ ] Proyectos/Delete.cshtml
- [ ] Actividades/Index.cshtml (dentro de Details del proyecto)
- [ ] Actividades/Create.cshtml
- [ ] etc.

---

## 5. FASE 3: INTEGRACIONES COMPLEJAS (Semana 4)

### 5.1 Gantt Chart
- [ ] Implementar endpoints para obtener datos (fechas, duraciones, precedencias)
- [ ] Integrar librería Gantt (ej: DHTMLX Gantt, FusionCharts, o Syncfusion)
- [ ] Vista Gantt.cshtml

### 5.2 Reportes
- [ ] ReporteProyectosActivos
- [ ] ReporteProgressGantt
- [ ] ReporteRecursosAsignados
- [ ] ReporteRiesgosAbiertos
- [ ] ReportePresupuesto
- [ ] ReporteDesviacionesAbiertas

### 5.3 Exportación
- [ ] Exportar a Excel (proyectos, actividades, riesgos)
- [ ] Exportar a PDF (reportes)

---

## 6. ESTIMACIÓN DE ESFUERZO

| Fase | Tarea | Horas | Semanas |
|------|-------|-------|---------|
| 1 | Estructura Base | 16 | 0.5 |
| 2 | CRUD Proyectos | 24 | 1 |
| 2 | CRUD Actividades | 16 | 0.5 |
| 2 | CRUD Hitos/Recursos/Riesgos | 24 | 1 |
| 2 | Views y Formularios | 20 | 1 |
| 3 | Gantt + Reportes | 32 | 1.5 |
| 3 | Exportación + Testing | 16 | 0.5 |
| **Total** | | **148** | **5-6** |

---

## 7. RIESGOS Y MITIGACIONES

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|--------------|---------|-----------|
| Procedimientos SQL incompletos | Media | Alto | Validar BD antes de iniciar |
| Gantt complejo de integrar | Media | Medio | Investigar librerías con anticipación |
| Tablas con relaciones circulares | Baja | Alto | Mapeo EF Core claro desde el inicio |
| Performance en reportes pesados | Media | Medio | Implementar índices, usar Dapper |
| Cambios en requisitos | Alta | Medio | Validar funcionalidad existente en WebMatrix |

---

## 8. CRITERIOS DE ÉXITO

- ✅ CRUD completo de Proyectos, Actividades, Hitos, Recursos, Riesgos
- ✅ Compilación sin errores
- ✅ Routing correcto en `/PY/*`
- ✅ DI registrada correctamente
- ✅ 5 reportes principales funcionando
- ✅ Gantt chart renderizándose
- ✅ Exportación a Excel funcionando
- ✅ Integración con US_Usuarios validada
- ✅ Testing funcional en ambiente test

---

## 9. SIGUIENTE DESPUÉS DE PY_PROYECTOS

Recomendación: **OP_Cuantitativo** (Operaciones - Cuantitativo)
- Complejidad similar
- Dependencia de PY_Proyectos (seguimiento de objetivos)
- Impacto operativo alto

---

**Documento Preparado**: 2024-01-XX  
**Estado**: Listo para Implementación  
**Próxima Acción**: Crear estructura base de PY_Proyectos en MatrixNext.Data y MatrixNext.Web
