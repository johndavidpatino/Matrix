# DIRECTRICES DE MIGRACIÓN - WebMatrix → MatrixNext

**Documento de Referencia Técnica**  
**Versión**: 1.0  
**Última Actualización**: 2026-01-02  
**Aplicable a**: Todos los módulos en migración

---

## 📋 ÍNDICE

1. [Reglas Core](#reglas-core)
2. [Arquitectura y Patrones](#arquitectura-y-patrones)
3. [Base de Datos](#base-de-datos)
4. [Controladores y Servicios](#controladores-y-servicios)
5. [Vistas y UI](#vistas-y-ui)
6. [Menú y Navegación](#menú-y-navegación)
7. [Testing y Validación](#testing-y-validación)
8. [Documentación](#documentación)

---

## 🎯 REGLAS CORE

### REGLA 1: Respetar Convenciones de Base de Datos

**Descripción**: Todos los nombres de procedimientos, tablas, columnas y tipos deben respetar exactamente la nomenclatura de la base de datos.

**Aplicación**:
- No cambiar nombres de SP ni de tablas
- Mantener prefijos existentes (TH_, US_, PY_, etc.)
- Respetar casing original (ej: `IdEmpleado`, `FechaInicio`)
- Validar en SQL Server antes de usar

**Ejemplo ✅ CORRECTO**:
```csharp
// Usar exactamente el nombre de la tabla
public class TH_SolicitudAusencia
{
    public long Id { get; set; }
    public long IdEmpleado { get; set; }  // Exacto como en BD
    public DateTime FInicio { get; set; }  // Sin cambiar a FechaInicio
}

// Usar exactamente el nombre del SP
connection.Query("TH_AUSENCIA_GET", parameters, 
    commandType: CommandType.StoredProcedure)
```

**Ejemplo ❌ INCORRECTO**:
```csharp
// NO renombrar
public class Ausencia { } // Debería ser TH_SolicitudAusencia
public long EmployeeId { get; set; } // Debería ser IdEmpleado
connection.Query("GetAusencia", ...) // Debería ser TH_AUSENCIA_GET
```

---

### REGLA 2: Analizar y Reutilizar Procedimientos Almacenados en CoreProject

**Descripción**: Antes de crear cualquier funcionalidad, investigar si ya existe un stored procedure en CoreProject (WebMatrix legacy) que ejecute esa lógica.

**Aplicación**:
1. Mapear todos los SP existentes del módulo
2. Documentar qué SP hace cada acción
3. Ejecutar el SP original en lugar de reimplementar
4. Crear adaptador que encapsule el SP
5. No duplicar lógica SQL

**Proceso**:
```
Paso 1: Analizar WebMatrix
└─ ¿Qué SP ejecuta en DataLayer? → TH_Ausencia.RegistrosAusencia

Paso 2: Validar en SQL Server
└─ EXEC TH_Ausencia.RegistrosAusencia @param1, @param2

Paso 3: Mapear en Adapter
└─ public List<...> ObtenerSolicitudes(...) 
     { return connection.Query("TH_Ausencia.RegistrosAusencia", ...) }

Paso 4: Exponer en Service
└─ public (bool, List<...>) ObtenerSolicitudes(...) { ... }

Paso 5: Usar en Controller
└─ var (success, data) = _service.ObtenerSolicitudes(...);
```

**Beneficios**:
- ✅ Sin duplicación de lógica
- ✅ Consistencia con WebMatrix
- ✅ Menos bugs (SP ya testado)
- ✅ Reversibilidad si es necesario

---

### REGLA 3: Utilizar EF para Inserciones y Actualizaciones

**Descripción**: Usar Entity Framework Core para operaciones INSERT y UPDATE simples. Reservar SP para lógica compleja.

**Aplicación**:

**✅ USAR EF CORE para**:
- INSERT nuevos registros
- UPDATE de campos simples
- DELETE de registros (si no hay triggers complejos)
- Operaciones que NO requieren lógica de negocio en SQL

**✅ USAR STORED PROCEDURES para**:
- Lógica compleja (validaciones, cálculos)
- Múltiples tablas (transacciones)
- Reportes con JOIN pesados
- Cálculos de auditoría

**Ejemplo ✅ EF PARA INSERT**:
```csharp
public long CrearSolicitudAusencia(long idEmpleado, byte tipo, DateTime fInicio, 
    DateTime fFin, short diasCalendario, byte diasLaborales, long aprobadorId, 
    string observaciones, long registradoPor)
{
    using var context = CreateContext();
    
    // Usar EF para INSERT simple
    var entity = new TH_SolicitudAusencia
    {
        IdEmpleado = idEmpleado,
        FInicio = fInicio,
        FFin = fFin,
        DiasCalendario = diasCalendario,
        DiasLaborales = diasLaborales,
        Tipo = tipo,
        Estado = 1,  // Radicada
        AprobadoPor = aprobadorId,
        RegistradoPor = registradoPor,
        FechaRegistro = DateTime.Now,
        ObservacionesSolicitud = observaciones ?? string.Empty,
        ObservacionesAprobacion = string.Empty
    };
    
    context.SolicitudesAusencia.Add(entity);
    context.SaveChanges();
    
    return entity.Id;
}
```

**Ejemplo ✅ SP PARA VALIDACIÓN COMPLEJA**:
```csharp
public ResultadoValidacionViewModel ValidarSolicitudAusencia(long idEmpleado, 
    DateTime fInicio, DateTime fFin, byte tipo)
{
    using var connection = new SqlConnection(_connectionString);
    
    var dp = new DynamicParameters();
    dp.Add("@idEmpleado", idEmpleado);
    dp.Add("@FInicio", fInicio);
    dp.Add("@FFin", fFin);
    dp.Add("@Tipo", tipo);
    
    // SP ejecuta: validar solapamiento, disponibilidad, etc.
    return connection.QueryFirstOrDefault<ResultadoValidacionViewModel>(
        "TH_Ausencia.ValidarSolicitudAusencia", dp, 
        commandType: CommandType.StoredProcedure);
}
```

---

### REGLA 4: Ejecutar Procedimientos Almacenados de Cada Acción

**Descripción**: Cada acción de WebMatrix ejecuta procedimientos específicos. Identificarlos y ejecutarlos en MatrixNext de la misma forma.

**Aplicación**:

**Mapeo Necesario**:
```
WebMatrix Action          →  SP Ejecutado              →  MatrixNext
═════════════════════════════════════════════════════════════════════
Crear Solicitud          →  TH_Ausencia.RegistrosAusencia (INSERT)
                            TH_Ausencia.CalculoDias
                            TH_Ausencia.ValidarSolicitud

Aprobar Solicitud        →  TH_Ausencia.RegistrosAusencia (UPDATE Estado=20)
                            TH_Ausencia.CausarVacaciones (si aplica)

Rechazar Solicitud       →  TH_Ausencia.RegistrosAusencia (UPDATE Estado=10)

Crear Incapacidad        →  TH_Ausencia_Incapacidades (INSERT)

Obtener Historial        →  TH_AUSENCIA_GET (SP legado)
                            o TH_Ausencia.RegistrosAusencia

Generar Reportes         →  TH_REP_Vacaciones, TH_REP_Beneficios, etc.
```

**Cómo Identificar SP**:
1. Abrir WebMatrix proyecto
2. Buscar clase DataLayer/DataAdapter del módulo
3. Notar qué SP se llama en cada método
4. Documentar exactamente el nombre del SP
5. Copiar la lógica de parámetros

**Ejemplo de Auditoría**:
```csharp
// En WebMatrix: Ausencias.aspx → btnAprobar_Click
// → AusenciaDataLayer.AprobarSolicitud(idSolicitud, idAprobador)
// → Ejecuta: TH_Ausencia.RegistrosAusencia (con @Accion='Aprobar')

// En MatrixNext: Hacer EXACTAMENTE lo mismo
public bool AprobarSolicitud(long idSolicitud, long aprobadorId, 
    string observacionesAprobacion = null)
{
    using var context = CreateContext();
    
    var entity = context.SolicitudesAusencia.FirstOrDefault(e => e.Id == idSolicitud);
    if (entity == null) return false;
    
    // Hacer lo mismo que el SP
    entity.Estado = 20;  // Aprobado
    entity.FechaAprobacion = DateTime.Now;
    entity.AprobadoPor = aprobadorId;
    entity.VoBo1 = aprobadorId;
    entity.FechaVoBo1 = DateTime.Now;
    entity.ObservacionesAprobacion = observacionesAprobacion ?? string.Empty;
    
    return context.SaveChanges() > 0;
}
```

---

### REGLA 5: Preferir Modales para Edición y Detalles

**Descripción**: Usar modales (Bootstrap Modal) en lugar de páginas separadas para editar, ver detalles, o eliminar registros.

**Aplicación**:

**Acciones que DEBEN ser Modal**:
- ✅ Editar registro (Create/Edit combined)
- ✅ Ver detalles ampliados
- ✅ Confirmar eliminación
- ✅ Cambiar estado (aprobar/rechazar)
- ✅ Agregar comentarios
- ✅ Seleccionar opciones secundarias

**Acciones que NO necesitan Modal**:
- ❌ Index/Listado (página principal)
- ❌ Dashboard/Summary
- ❌ Reportes complejos
- ❌ Navegación entre secciones

**Estructura Modal Estándar**:
```html
<!-- Modal para Editar/Crear -->
<div class="modal fade" id="modalEditar" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <!-- Header -->
            <div class="modal-header">
                <h5 class="modal-title">Editar Solicitud</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            
            <!-- Body - Formulario -->
            <div class="modal-body">
                <form id="formEditar">
                    <div class="mb-3">
                        <label class="form-label">Nombre</label>
                        <input type="text" class="form-control" id="nombre">
                    </div>
                    <!-- Más campos -->
                </form>
            </div>
            
            <!-- Footer - Acciones -->
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                    Cancelar
                </button>
                <button type="button" class="btn btn-primary" id="btnGuardar">
                    Guardar
                </button>
            </div>
        </div>
    </div>
</div>

<!-- Script para disparar modal -->
<script>
document.addEventListener('DOMContentLoaded', function() {
    const modal = new bootstrap.Modal(document.getElementById('modalEditar'));
    
    // Abrir modal al hacer click en botón Editar
    document.querySelectorAll('.btn-editar').forEach(btn => {
        btn.addEventListener('click', function() {
            const id = this.dataset.id;
            // Cargar datos via AJAX
            fetch(`/TH/Ausencias/GetDetails/${id}`)
                .then(r => r.json())
                .then(data => {
                    document.getElementById('nombre').value = data.nombre;
                    modal.show();
                });
        });
    });
    
    // Guardar cambios
    document.getElementById('btnGuardar').addEventListener('click', function() {
        const data = { /* recolectar datos del form */ };
        fetch('/TH/Ausencias/Update', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        })
        .then(r => r.json())
        .then(result => {
            if (result.success) {
                modal.hide();
                location.reload(); // O actualizar tabla vía AJAX
            }
        });
    });
});
</script>
```

---

### REGLA 6: Agregar Acciones Existentes, No Crear Nuevas

**Descripción**: Solo migrar acciones (botones, funcionalidades) que existan en WebMatrix. No agregar nuevas features durante la migración.

**Aplicación**:

**✅ HACER**:
- Crear/Editar/Leer/Eliminar (si existen en WebMatrix)
- Aprobar/Rechazar (si existen)
- Cambiar estado (si existen)
- Exportar/Reportes (si existen)
- Búsqueda/Filtros (si existen)

**❌ NO HACER**:
- Agregar nuevos campos que no estén en WebMatrix
- Crear nuevas acciones (ej: "DuplicarSolicitud")
- Cambiar flujo de negocio
- Agregar validaciones adicionales
- Implementar nuevos reportes

**Ejemplo ✅ CORRECTO**:
```csharp
// WebMatrix: SolicitudAusencia.aspx tiene botones:
// - Nueva (Create)
// - Editar
// - Eliminar
// - Ver Historial (Listado)

// MatrixNext: Implementar SOLO estos 4
public class AusenciasController : Controller
{
    [HttpGet("")]
    public IActionResult Index() { }  // ✅ Ver historial
    
    [HttpGet("Create")]
    public IActionResult Create() { }  // ✅ Nueva
    
    [HttpPost("Create")]
    public IActionResult Create(CrearRequest req) { }  // ✅ Guardar
    
    [HttpGet("Edit/{id}")]
    public IActionResult Edit(long id) { }  // ✅ Editar (modal)
    
    [HttpPost("Edit/{id}")]
    public IActionResult Edit(long id, EditRequest req) { }  // ✅ Guardar edición
    
    [HttpPost("Delete/{id}")]
    public IActionResult Delete(long id) { }  // ✅ Eliminar
    
    // ❌ NO hacer esto (no existe en WebMatrix)
    // public IActionResult Duplicate(long id) { }
}
```

---

### REGLA 7: Aprovechar Elementos Visuales Disponibles

**Descripción**: Usar componentes (controles, dropdowns, selectores) que ya existen en MatrixNext de otros módulos migrados.

**Aplicación**:

**Componentes Reutilizables**:
```
Componente               Ubicación                    Uso
═════════════════════════════════════════════════════════════════════
Modal CRUD              Views/Shared/_Modal*         Editar/Crear
DatePicker              Views/Shared/_DatePicker     Seleccionar fechas
Dropdown Usuarios       Views/Shared/_SelectUser     Seleccionar persona
Grid Paginado           Views/Shared/_Grid           Mostrar listados
Buscador                Views/Shared/_Search         Buscar registros
Confirmación Modal      Views/Shared/_Confirm        Confirmar acciones
Toast Notificaciones    Views/Shared/_Toast          Mostrar mensajes
Loading Spinner         Views/Shared/_Loading        Indicador de carga
Badge Estados           Views/Shared/_Badge          Mostrar estados
Sidebar Menú            Views/Shared/_Sidebar        Navegación
```

**Cómo Usar**:
```html
<!-- Usar partial compartido para DatePicker -->
@await Html.PartialAsync("_DatePicker", new DatePickerModel 
{
    FieldName = "FechaInicio",
    Label = "Fecha Inicio",
    Value = Model?.FechaInicio
})

<!-- Usar partial para Select Usuarios -->
@await Html.PartialAsync("_SelectUser", new SelectUserModel
{
    FieldName = "AprobadorId",
    Label = "Aprobador",
    SelectedValue = Model?.AprobadorId
})

<!-- Usar Grid parcial -->
@await Html.PartialAsync("_Grid", new GridModel
{
    Data = solicitudes,
    Columns = new[] { "Empleado", "Tipo", "Fecha Inicio", "Estado" },
    RowAction = "Editar"
})
```

---

### REGLA 8: Priorizar Detalle sobre Velocidad

**Descripción**: Es mejor migrar pocos webforms completamente que muchos webforms incompletos. Avanzar lentamente asegura calidad.

**Aplicación**:

**Patrón Iterativo Recomendado**:
```
Semana 1: Módulo COMPLETO (100% de 1-2 webforms)
├── Análisis exhaustivo
├── Mapeo de SP
├── Implementar CRUD perfecto
├── Documentar cada detalle
├── Testing funcional
└── ✅ LISTO PARA PRODUCCIÓN

Semana 2: Siguiente Módulo COMPLETO
├── Repetir proceso
├── Aplicar lecciones aprendidas
├── Menos problemas esta vez
└── ✅ LISTO PARA PRODUCCIÓN
```

**Vs. Patrón INCORRECTO**:
```
Intento Rápido (EVITAR):
├── Semana 1: Migrar 8 webforms (sin testear bien)
├── Semana 2: Semana 3: Bugs aparecen
├── Semana 3: Devolverse a arreglar problemas
├── Semana 4: Aún hay bugs
└── ❌ TIEMPO PERDIDO
```

**Checklist de Completitud**:
```
Para cada webform migrado:
□ Todos los campos presentes
□ Todos los botones implementados
□ Todas las validaciones funcionan
□ Todos los SP se ejecutan
□ Modales funcionan correctamente
□ Búsqueda/Filtros funcionan
□ Paginación funciona
□ Exportación funciona (si existe)
□ Error handling implementado
□ Logging implementado
□ Documentación completa
□ Testing funcional exitoso
□ Code review pasado
```

---

### REGLA 9: Mantener Estructura de Áreas

**Descripción**: Usar Areas para todos los módulos excepto funcionalidades globales (Login, Home, etc.).

**Aplicación**:

**Estructura Obligatoria**:
```
MatrixNext.Web/
├── Areas/
│   ├── TH/                              # Talento Humano
│   │   ├── Controllers/
│   │   │   ├── AusenciasController.cs
│   │   │   ├── EmpleadosController.cs
│   │   │   └── NominaController.cs
│   │   └── Views/
│   │       ├── Ausencias/
│   │       │   ├── Index.cshtml
│   │       │   ├── Create.cshtml
│   │       │   └── _Modal.cshtml
│   │       ├── Empleados/
│   │       └── Nomina/
│   │
│   ├── PY/                              # Proyectos
│   │   ├── Controllers/
│   │   └── Views/
│   │
│   └── [Otros módulos...]
│
├── Controllers/                         # Controllers GLOBALES SOLO
│   ├── HomeController.cs               # Dashboard principal
│   └── AccountController.cs            # Login (opcional, si existe)
│
├── Views/
│   ├── Home/
│   │   └── Index.cshtml                # Dashboard
│   └── Shared/                         # Componentes compartidos
│       ├── _Layout.cshtml
│       ├── _Sidebar.cshtml
│       ├── _Modal.cshtml
│       ├── _DatePicker.cshtml
│       └── [Otros componentes]
│
└── Program.cs                          # Configuración global
```

**Beneficios**:
- ✅ Escalabilidad (agregar módulos fácilmente)
- ✅ Equipos independientes (cada área por equipo)
- ✅ Evitar conflictos de nombres
- ✅ URLs claras (`/TH/Ausencias`, `/PY/Proyectos`)
- ✅ Mantenibilidad

**Registro en Program.cs**:
```csharp
// Program.cs
var builder = WebApplicationBuilder.CreateBuilder(args);

// Agregar Areas
builder.Services.AddControllersWithViews();

// Registrar módulos
builder.Services.AddTHModule();
builder.Services.AddPYModule();
builder.Services.AddUSModule();

var app = builder.Build();

// Configurar rutas
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
```

---

### REGLA 10: Crear Menú y Sidebar para Acceso

**Descripción**: Agregar entradas en el menú/sidebar para cada módulo y submodulo migrado.

**Aplicación**:

**Estructura de Menú**:
```
Home                          → /Home
├── Talento Humano            → #
│   ├── Ausencias             → /TH/Ausencias
│   │   ├── Nueva Solicitud   → /TH/Ausencias/Create
│   │   ├── Mis Solicitudes   → /TH/Ausencias
│   │   ├── Por Aprobar       → /TH/GestionAusencia
│   │   └── Equipo            → /TH/AusenciasEquipo
│   ├── Empleados             → /TH/Empleados
│   ├── Nómina                → /TH/Nomina
│   └── ...
│
├── Proyectos                 → #
│   ├── Gestión Proyectos     → /PY/Proyectos
│   ├── Actividades           → /PY/Actividades
│   ├── Hitos                 → /PY/Hitos
│   └── Reportes              → /PY/Reportes
│
├── Administración            → #
│   ├── Usuarios              → /US/Usuarios
│   ├── Roles                 → /US/Roles
│   ├── Permisos              → /US/Permisos
│   └── Grupos                → /US/Grupos
│
└── [Otros módulos...]
```

**Implementación en _Sidebar.cshtml**:
```html
<nav class="sidebar">
    <ul class="nav flex-column">
        <!-- Home -->
        <li class="nav-item">
            <a class="nav-link" href="/" title="Ir a inicio">
                <i class="fas fa-home"></i> Home
            </a>
        </li>
        
        <!-- Talento Humano (con submenu) -->
        <li class="nav-item">
            <a class="nav-link" data-bs-toggle="collapse" href="#thMenu" role="button">
                <i class="fas fa-users"></i> Talento Humano
                <span class="toggle-icon">▼</span>
            </a>
            <div class="collapse" id="thMenu">
                <ul class="nav flex-column ms-3">
                    <!-- Ausencias (con subsubmenu) -->
                    <li class="nav-item">
                        <a class="nav-link" data-bs-toggle="collapse" href="#ausenciasMenu">
                            <i class="fas fa-calendar-times"></i> Ausencias
                            <span class="toggle-icon">▼</span>
                        </a>
                        <div class="collapse" id="ausenciasMenu">
                            <ul class="nav flex-column ms-3">
                                <li class="nav-item">
                                    <a class="nav-link" href="/TH/Ausencias/Create">
                                        <i class="fas fa-plus"></i> Nueva Solicitud
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="/TH/Ausencias">
                                        <i class="fas fa-list"></i> Mis Solicitudes
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="/TH/GestionAusencia">
                                        <i class="fas fa-check"></i> Por Aprobar
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" href="/TH/AusenciasEquipo">
                                        <i class="fas fa-sitemap"></i> Equipo
                                    </a>
                                </li>
                            </ul>
                        </div>
                    </li>
                    
                    <!-- Empleados -->
                    <li class="nav-item">
                        <a class="nav-link" href="/TH/Empleados">
                            <i class="fas fa-id-card"></i> Empleados
                        </a>
                    </li>
                    
                    <!-- Nómina -->
                    <li class="nav-item">
                        <a class="nav-link" href="/TH/Nomina">
                            <i class="fas fa-coins"></i> Nómina
                        </a>
                    </li>
                </ul>
            </div>
        </li>
        
        <!-- Proyectos -->
        <li class="nav-item">
            <a class="nav-link" data-bs-toggle="collapse" href="#pyMenu">
                <i class="fas fa-project-diagram"></i> Proyectos
                <span class="toggle-icon">▼</span>
            </a>
            <div class="collapse" id="pyMenu">
                <ul class="nav flex-column ms-3">
                    <li class="nav-item">
                        <a class="nav-link" href="/PY/Proyectos">
                            <i class="fas fa-list"></i> Gestión
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/PY/Actividades">
                            <i class="fas fa-tasks"></i> Actividades
                        </a>
                    </li>
                </ul>
            </div>
        </li>
        
        <!-- Administración -->
        <li class="nav-item">
            <a class="nav-link" data-bs-toggle="collapse" href="#adminMenu">
                <i class="fas fa-cogs"></i> Administración
                <span class="toggle-icon">▼</span>
            </a>
            <div class="collapse" id="adminMenu">
                <ul class="nav flex-column ms-3">
                    <li class="nav-item">
                        <a class="nav-link" href="/US/Usuarios">
                            <i class="fas fa-user-tie"></i> Usuarios
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="/US/Roles">
                            <i class="fas fa-badge"></i> Roles
                        </a>
                    </li>
                </ul>
            </div>
        </li>
    </ul>
</nav>

<style>
.sidebar { /* estilos */ }
.nav-link { /* estilos */ }
.toggle-icon { /* estilos */ }
</style>
```

**Actualizaciones Necesarias al Agregar Módulos**:
```
1. Crear área con controllers/views
2. Agregar entrada en _Sidebar.cshtml
3. Registrar módulo en Program.cs (AddTHModule, etc.)
4. Documentar en este archivo
5. Commit a git
```

---

## 🏗️ ARQUITECTURA Y PATRONES

### PATRÓN: Adapter + Service + Controller

**Estructura Obligatoria**:
```
Request (HTTP)
    ↓
Controller (recibe request, valida, coordina)
    ↓
Service (lógica de negocio, transformación)
    ↓
DataAdapter (interactúa con BD)
    ↓
SQL (SP o EF)
    ↓
Response (JSON o View)
```

**Responsabilidades Claras**:

| Capa | Responsabilidad | Ejemplos |
|------|-----------------|----------|
| **Controller** | Recibir request, coordinar, retornar respuesta | Validar headers, autenticación, llamar service |
| **Service** | Lógica de negocio, validaciones | Calcular días, validar disponibilidad, logging |
| **Adapter** | Acceso a datos, mapeo | Ejecutar SP, EF CRUD, mapear resultados |
| **Database** | Almacenamiento, triggers, índices | Tablas, SP, vistas |

**Ejemplo Completo**:
```csharp
// === CONTROLLER ===
[Area("TH")]
[Route("TH/Ausencias")]
[Authorize]
public class AusenciasController : Controller
{
    private readonly AusenciaService _service;
    
    [HttpPost("Create")]
    public IActionResult Create([FromBody] CrearSolicitudRequest req)
    {
        // 1. Validar request
        if (!ModelState.IsValid)
            return Json(new { success = false, message = "Datos inválidos" });
        
        // 2. Obtener usuario actual
        var idUsuario = GetCurrentUserId();
        
        // 3. Llamar service
        var (success, message, id) = _service.CrearSolicitud(
            idUsuario,
            new SolicitudAusenciaFormViewModel
            {
                TipoAusencia = req.Tipo,
                FechaInicio = req.FechaInicio,
                FechaFin = req.FechaFin,
                AprobadorId = req.AprobadorId,
                Observaciones = req.Observaciones
            }
        );
        
        // 4. Retornar respuesta
        return Json(new { success, message, id });
    }
}

// === SERVICE ===
public class AusenciaService
{
    private readonly AusenciaDataAdapter _adapter;
    private readonly ILogger<AusenciaService> _logger;
    
    public (bool success, string message, long id) CrearSolicitud(
        long idEmpleado, SolicitudAusenciaFormViewModel modelo)
    {
        try
        {
            // 1. Validar fechas
            if (modelo.FechaInicio > modelo.FechaFin)
                return (false, "La fecha de inicio no puede ser mayor que la fecha fin", 0);
            
            // 2. Validar aprobador
            if (modelo.AprobadorId <= 0)
                return (false, "Debe seleccionar un aprobador", 0);
            
            // 3. Validar disponibilidad (via SP)
            var validacion = _adapter.ValidarSolicitudAusencia(
                idEmpleado, modelo.FechaInicio, modelo.FechaFin, modelo.TipoAusencia);
            if (validacion?.Result != 0)
                return (false, validacion?.MensajeResultado ?? "Solicitud no válida", 0);
            
            // 4. Calcular días (via SP)
            var calculo = _adapter.CalcularDias(
                modelo.FechaInicio, modelo.FechaFin, false, idEmpleado);
            if (calculo == null)
                return (false, "No fue posible calcular los días", 0);
            
            // 5. Crear solicitud (via EF)
            var id = _adapter.CrearSolicitudAusencia(
                idEmpleado,
                modelo.TipoAusencia,
                modelo.FechaInicio,
                modelo.FechaFin,
                (short)calculo.DiasCalendario,
                (byte)calculo.DiasLaborales,
                modelo.AprobadorId,
                modelo.Observaciones,
                idEmpleado
            );
            
            // 6. Logging
            _logger.LogInformation($"Solicitud creada: ID={id}, Empleado={idEmpleado}");
            
            // 7. Retornar éxito
            return (true, "Solicitud radicada correctamente", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creando solicitud");
            return (false, $"Error: {ex.Message}", 0);
        }
    }
}

// === DATA ADAPTER ===
public class AusenciaDataAdapter
{
    private readonly string _connectionString;
    
    // Inserción con EF
    public long CrearSolicitudAusencia(long idEmpleado, byte tipo, DateTime fInicio,
        DateTime fFin, short diasCalendario, byte diasLaborales, long aprobadorId,
        string observaciones, long registradoPor)
    {
        using var context = CreateContext();
        
        var entity = new TH_SolicitudAusencia
        {
            IdEmpleado = idEmpleado,
            FInicio = fInicio,
            FFin = fFin,
            DiasCalendario = diasCalendario,
            DiasLaborales = diasLaborales,
            Tipo = tipo,
            Estado = 1,
            AprobadoPor = aprobadorId,
            RegistradoPor = registradoPor,
            FechaRegistro = DateTime.Now,
            ObservacionesSolicitud = observaciones ?? string.Empty,
            ObservacionesAprobacion = string.Empty
        };
        
        context.SolicitudesAusencia.Add(entity);
        context.SaveChanges();
        
        return entity.Id;
    }
    
    // Validación con SP
    public ResultadoValidacionViewModel ValidarSolicitudAusencia(
        long idEmpleado, DateTime fInicio, DateTime fFin, byte tipo)
    {
        using var connection = new SqlConnection(_connectionString);
        
        var dp = new DynamicParameters();
        dp.Add("@idEmpleado", idEmpleado);
        dp.Add("@FInicio", fInicio);
        dp.Add("@FFin", fFin);
        dp.Add("@Tipo", tipo);
        
        return connection.QueryFirstOrDefault<ResultadoValidacionViewModel>(
            "TH_Ausencia.ValidarSolicitudAusencia", dp,
            commandType: CommandType.StoredProcedure);
    }
    
    // Cálculo con SP
    public CalculoDiasViewModel CalcularDias(DateTime? inicio, DateTime? fin,
        bool incluirSabadoComoDiaLaboral, long idEmpleado)
    {
        using var connection = new SqlConnection(_connectionString);
        
        var dp = new DynamicParameters();
        dp.Add("@FInicio", inicio);
        dp.Add("@FFin", fin);
        dp.Add("@incluyeSabado", incluirSabadoComoDiaLaboral);
        dp.Add("@idEmpleado", idEmpleado);
        
        return connection.QueryFirstOrDefault<CalculoDiasViewModel>(
            "TH_Ausencia.CalculoDias", dp,
            commandType: CommandType.StoredProcedure);
    }
}
```

---

## 💾 BASE DE DATOS

### Convenciones de Nombres

| Elemento | Formato | Ejemplo | Regla |
|----------|---------|---------|-------|
| **Tabla** | `[MODULO]_[Entidad]` | `TH_SolicitudAusencia` | PascalCase, con prefijo |
| **Columna** | `[NombreEnCamelCase]` | `IdEmpleado`, `FechaInicio` | Respetar casing original |
| **SP** | `[MODULO]_[Accion]` o `[MODULO].[Accion]` | `TH_AUSENCIA_GET` o `TH_Ausencia.RegistrosAusencia` | MAYÚSCULAS o [schema]. |
| **PK** | Siempre `Id` | `Id` | int o long |
| **FK** | `Id[Tabla]` | `IdEmpleado`, `IdSolicitud` | Referencia a tabla |
| **Auditoría** | `RegistradoPor`, `FechaRegistro`, `ModificadoPor`, `FechaModificacion` | - | En cada tabla |

### Mapeo EF Core

**Definición en DbContext**:
```csharp
public class MatrixDbContext : DbContext
{
    public MatrixDbContext(string connectionString) 
        : base(new DbContextOptionsBuilder<MatrixDbContext>()
            .UseSqlServer(connectionString)
            .Options)
    {
    }
    
    // TH_Ausencias
    public DbSet<TH_SolicitudAusencia> SolicitudesAusencia { get; set; }
    public DbSet<TH_Ausencia_Incapacidades> AusenciaIncapacidades { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Mapeo explícito si el nombre de tabla no sigue convención
        modelBuilder.Entity<TH_SolicitudAusencia>()
            .ToTable("TH_SolicitudAusencia");
        
        modelBuilder.Entity<TH_Ausencia_Incapacidades>()
            .ToTable("TH_Ausencia_Incapacidades");
    }
}
```

**Entidad EF**:
```csharp
public class TH_SolicitudAusencia
{
    [Key]
    public long Id { get; set; }
    
    public long IdEmpleado { get; set; }
    public DateTime? FiniCausacion { get; set; }
    public DateTime? FFinCausacion { get; set; }
    public DateTime? FInicio { get; set; }
    public DateTime? FFin { get; set; }
    public short DiasCalendario { get; set; }
    public byte DiasLaborales { get; set; }
    public byte Tipo { get; set; }
    public byte Estado { get; set; }
    public long? AprobadoPor { get; set; }
    public DateTime? FechaAprobacion { get; set; }
    public long? VoBo1 { get; set; }
    public DateTime? FechaVoBo1 { get; set; }
    public long RegistradoPor { get; set; }
    public DateTime FechaRegistro { get; set; }
    public string ObservacionesSolicitud { get; set; } = string.Empty;
    public string ObservacionesAprobacion { get; set; } = string.Empty;
}
```

---

## 🎮 CONTROLADORES Y SERVICIOS

### Estructura de Controller

```csharp
[Area("TH")]
[Route("TH/[controller]")]
[Authorize]  // Siempre requerir autenticación
public class AusenciasController : Controller
{
    private readonly AusenciaService _service;
    private readonly ILogger<AusenciasController> _logger;
    
    public AusenciasController(AusenciaService service, 
        ILogger<AusenciasController> logger)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    // Helper para obtener usuario actual
    private long GetCurrentUserId()
    {
        var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            ?? User?.FindFirst("Id")?.Value;
        if (long.TryParse(idClaim, out var id))
            return id;
        throw new InvalidOperationException("Id de usuario autenticado no disponible");
    }
    
    // DTOs para binding
    public class CrearSolicitudRequest
    {
        public long idEmpleado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public byte TipoAusencia { get; set; }
        public long AprobadorId { get; set; }
        public string Observaciones { get; set; }
    }
    
    // === GET: Index - Listado principal ===
    [HttpGet("")]
    public async Task<IActionResult> Index()
    {
        try
        {
            var idUsuario = GetCurrentUserId();
            var (success, msg, solicitudes) = await Task.FromResult(
                _service.ObtenerSolicitudesEmpleado(idUsuario));
            
            if (!success)
                ModelState.AddModelError("", msg);
            
            return View(solicitudes ?? new List<AusenciaViewModel>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en Index");
            return View(new List<AusenciaViewModel>());
        }
    }
    
    // === GET: Create - Mostrar formulario ===
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        try
        {
            // Cargar catálogos para dropdowns
            var (_, tipos) = await Task.FromResult(_service.ObtenerTiposAusencia());
            var (_, aprobadores) = await Task.FromResult(_service.ObtenerAprobadores());
            
            ViewBag.TiposAusencia = tipos ?? new List<TipoAusenciaViewModel>();
            ViewBag.Aprobadores = aprobadores ?? new List<AprobadorViewModel>();
            
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en Create GET");
            return RedirectToAction("Index");
        }
    }
    
    // === POST: Create - Guardar nueva solicitud ===
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CrearSolicitudRequest req)
    {
        try
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Datos inválidos" });
            
            var idUsuario = GetCurrentUserId();
            var (success, message, id) = await Task.FromResult(
                _service.CrearSolicitud(idUsuario, new SolicitudAusenciaFormViewModel
                {
                    TipoAusencia = req.TipoAusencia,
                    FechaInicio = req.FechaInicio,
                    FechaFin = req.FechaFin,
                    AprobadorId = req.AprobadorId,
                    Observaciones = req.Observaciones
                })
            );
            
            if (success)
                _logger.LogInformation($"Solicitud creada: ID={id}");
            else
                _logger.LogWarning($"Error creando solicitud: {message}");
            
            return Json(new { success, message, id });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en Create POST");
            return Json(new { success = false, message = ex.Message });
        }
    }
    
    // === Otros métodos (Edit, Details, Delete, etc.) ===
}
```

### Estructura de Service

```csharp
public class AusenciaService
{
    private readonly AusenciaDataAdapter _adapter;
    private readonly ILogger<AusenciaService> _logger;
    
    public AusenciaService(AusenciaDataAdapter adapter, 
        ILogger<AusenciaService> logger)
    {
        _adapter = adapter ?? throw new ArgumentNullException(nameof(adapter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    // Método típico: retorna tupla con (éxito, mensaje, datos)
    public (bool success, string message, List<AusenciaViewModel> data) 
        ObtenerSolicitudesEmpleado(long idEmpleado)
    {
        try
        {
            // Validar entrada
            if (idEmpleado <= 0)
                return (false, "ID de empleado inválido", null);
            
            // Llamar adapter
            var data = _adapter.ObtenerSolicitudes(idEmpleado: idEmpleado);
            
            // Logging
            _logger.LogInformation($"Solicitudes obtenidas: {data?.Count ?? 0} registros");
            
            // Retornar
            return (true, "", data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error obteniendo solicitudes del empleado {idEmpleado}");
            return (false, ex.Message, null);
        }
    }
}
```

---

## 🎨 VISTAS Y UI

### Estructura View

```html
@model List<AusenciaViewModel>
@{
    ViewData["Title"] = "Mis Solicitudes de Ausencia";
}

<div class="container-fluid">
    <!-- Header -->
    <div class="row mb-4">
        <div class="col-md-6">
            <h1>Mis Solicitudes de Ausencia</h1>
        </div>
        <div class="col-md-6 text-end">
            <button class="btn btn-primary" onclick="abrirModalCrear()">
                <i class="fas fa-plus"></i> Nueva Solicitud
            </button>
        </div>
    </div>
    
    <!-- Tabla de Solicitudes -->
    <div class="row">
        <div class="col-12">
            <div class="card">
                <div class="card-body">
                    @if (Model?.Any() == true)
                    {
                        <table class="table table-hover">
                            <thead>
                                <tr>
                                    <th>Tipo</th>
                                    <th>Fecha Inicio</th>
                                    <th>Fecha Fin</th>
                                    <th>Estado</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                @foreach (var item in Model)
                                {
                                    <tr>
                                        <td>@item.TipoNombre</td>
                                        <td>@item.FechaInicio?.ToString("dd/MM/yyyy")</td>
                                        <td>@item.FechaFin?.ToString("dd/MM/yyyy")</td>
                                        <td>
                                            <span class="badge bg-@GetEstadoBadge(item.Estado)">
                                                @item.EstadoNombre
                                            </span>
                                        </td>
                                        <td>
                                            <button class="btn btn-sm btn-info" 
                                                onclick="abrirModalDetalles(@item.Id)">
                                                <i class="fas fa-eye"></i> Ver
                                            </button>
                                            <button class="btn btn-sm btn-warning" 
                                                onclick="abrirModalEditar(@item.Id)">
                                                <i class="fas fa-edit"></i> Editar
                                            </button>
                                            <button class="btn btn-sm btn-danger" 
                                                onclick="confirmarEliminar(@item.Id)">
                                                <i class="fas fa-trash"></i> Eliminar
                                            </button>
                                        </td>
                                    </tr>
                                }
                            </tbody>
                        </table>
                    }
                    else
                    {
                        <div class="alert alert-info">
                            No tienes solicitudes registradas.
                        </div>
                    }
                </div>
            </div>
        </div>
    </div>
</div>

<!-- Modal para crear/editar -->
<div class="modal fade" id="modalFormulario" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Nueva Solicitud</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
            </div>
            <div class="modal-body">
                <form id="formSolicitud">
                    <div class="mb-3">
                        <label class="form-label">Tipo</label>
                        <select class="form-select" id="tipo" required></select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Fecha Inicio</label>
                        <input type="date" class="form-control" id="fechaInicio" required>
                    </div>
                    <div class="mb-3">
                        <label class="form-label">Fecha Fin</label>
                        <input type="date" class="form-control" id="fechaFin" required>
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">
                    Cancelar
                </button>
                <button type="button" class="btn btn-primary" onclick="guardarSolicitud()">
                    Guardar
                </button>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <script>
    const modal = new bootstrap.Modal(document.getElementById('modalFormulario'));
    
    function abrirModalCrear() {
        document.getElementById('formSolicitud').reset();
        modal.show();
    }
    
    function guardarSolicitud() {
        const data = {
            tipo: document.getElementById('tipo').value,
            fechaInicio: document.getElementById('fechaInicio').value,
            fechaFin: document.getElementById('fechaFin').value
        };
        
        fetch('/TH/Ausencias/Create', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        })
        .then(r => r.json())
        .then(result => {
            if (result.success) {
                alert('Solicitud creada correctamente');
                modal.hide();
                location.reload();
            } else {
                alert('Error: ' + result.message);
            }
        });
    }
    </script>
}
```

---

## 📋 MENÚ Y NAVEGACIÓN

### Actualización de Sidebar

**Archivos a Modificar**:
- `Views/Shared/_Sidebar.cshtml` - Agregar entradas de menú
- `wwwroot/css/sidebar.css` - Estilos si es necesario

**Proceso**:
1. Identificar dónde va el módulo en la jerarquía
2. Agregar `<li>` con enlace correcto
3. Incluir iconos FontAwesome consistentes
4. Probar que los enlaces funcionen
5. Validar que sea accesible con permisos del usuario

---

## ✔️ TESTING Y VALIDACIÓN

### Checklist Pre-Commit

Antes de commitear código, verificar:

```
□ Compilación sin errores
□ 0 warnings críticos (nullability aceptable)
□ Todos los métodos implementados
□ Todos los SP ejecutados correctamente
□ Modales funcionan
□ Búsqueda/filtros funcionan
□ Paginación funciona
□ Permisos [Authorize] aplicados
□ Logging en operaciones críticas
□ Manejo de excepciones completo
□ DI registrado en Program.cs
□ Menú actualizado en _Sidebar.cshtml
□ Documentación actualizada
□ Sin archivos sin usar
□ Sin TODO comentarios
```

### Testing Funcional Mínimo

Para cada vista, probar:
1. Acceso: ¿Puedo acceder con [Authorize]?
2. Crear: ¿Puedo crear nuevo registro?
3. Editar: ¿Puedo editar existente?
4. Eliminar: ¿Puedo eliminar con confirmación?
5. Búsqueda: ¿Funcionan filtros?
6. Paginación: ¿Se pagina correctamente?
7. Modal: ¿Se abre y cierra?
8. Error: ¿Qué pasa si hay error en BD?

---

## 📖 DOCUMENTACIÓN

### Documentación Mínima Requerida

Por cada módulo migrado:

1. **ANALISIS_[MODULO].md** (500+ líneas)
   - Descripción de módulo
   - Páginas a migrar
   - Procedimientos SQL
   - Flujos de negocio
   - Diagramas

2. **MIGRACION_[MODULO]_COMPLETADA.md**
   - Checklist de implementación
   - Componentes migrados
   - SP mapeados
   - Testing realizado

3. **Comentarios en Código**
   - Métodos complejos documentados
   - SP ejecutados documentados
   - Excepciones documentadas

### Plantilla Commit Git

```
[módulo]: [acción corta]

Descripción detallada:
- Qué se hizo
- Por qué
- Cómo se probó

Ejemplo:
feat(TH_Ausencias): implement solicitud creation flow
- Implement AusenciaDataAdapter with EF insert and SP validation
- Implement AusenciaService with business logic
- Implement AusenciasController with modal form
- Map TH_AUSENCIA_GET, TH_Ausencia.CalculoDias, TH_Ausencia.ValidarSolicitud
- Add Ausencias menu entry in sidebar
- Tested: create, edit, delete, list operations
```

---

## ⚠️ REGLAS ADICIONALES (AGREGADAS)

### REGLA 11: Validar Permisos de Usuario

**Descripción**: Siempre validar que el usuario autenticado tiene permisos para la acción.

**Aplicación**:
```csharp
[Authorize]  // Autenticación mínima
[Authorize(Roles = "RRHH")]  // Rol específico si es necesario

// O validar en servicio
if (idUsuario != solicitud.IdEmpleado && !esAprobador)
    return (false, "No tienes permisos para esta acción");
```

---

### REGLA 12: Validar Datos de Entrada

**Descripción**: Siempre validar que los datos recibidos sean válidos antes de procesarlos.

**Aplicación**:
```csharp
// En controller
if (!ModelState.IsValid)
    return Json(new { success = false, message = "Datos inválidos" });

// En service
if (modelo.FechaInicio > modelo.FechaFin)
    return (false, "Fecha inválida");

if (string.IsNullOrWhiteSpace(modelo.Nombre))
    return (false, "Nombre es requerido");
```

---

### REGLA 13: Manejar Errores Gracefully

**Descripción**: Nunca retornar stack trace al cliente. Retornar mensajes amigables.

**Aplicación**:
```csharp
// ❌ INCORRECTO
catch (Exception ex)
{
    return Json(new { error = ex.ToString() }); // Stack trace expuesto
}

// ✅ CORRECTO
catch (Exception ex)
{
    _logger.LogError(ex, "Error en operación"); // Log detalles
    return Json(new { success = false, message = "Ocurrió un error inesperado" }); // Mensaje genérico
}
```

---

### REGLA 14: Usar Async/Await en Controllers

**Descripción**: Usar async/await para operaciones de I/O (BD, APIs externas).

**Aplicación**:
```csharp
// ✅ CORRECTO
[HttpGet("")]
public async Task<IActionResult> Index()
{
    var (success, solicitudes) = await Task.FromResult(
        _service.ObtenerSolicitudes());
    return View(solicitudes);
}
```

---

### REGLA 15: Documentar Modificaciones en MODULOS_MIGRACION.md

**Descripción**: Mantener actualizado el documento maestro de migración con cada módulo completado.

**Aplicación**:
- Agregar estado ✅ COMPLETADO cuando termina módulo
- Especificar qué páginas se migraron
- Actualizar "Próximo a migrar"
- Incluir enlace a ANALISIS_[MODULO].md

---

## 🎯 RESUMEN DE REGLAS

| # | Regla | Prioridad | Aplicable |
|---|-------|-----------|-----------|
| 1 | Respetar nombres BD | 🔴 CRÍTICA | Siempre |
| 2 | Analizar SP en CoreProject | 🔴 CRÍTICA | Siempre |
| 3 | Usar EF para CRUD simple | 🟠 ALTA | Siempre |
| 4 | Ejecutar SP de WebMatrix | 🔴 CRÍTICA | Siempre |
| 5 | Preferir modales | 🟠 ALTA | UI |
| 6 | Agregar acciones existentes | 🔴 CRÍTICA | Features |
| 7 | Aprovechar componentes | 🟠 ALTA | UI |
| 8 | Priorizar detalle | 🔴 CRÍTICA | Proceso |
| 9 | Mantener áreas | 🟠 ALTA | Estructura |
| 10 | Crear menú de acceso | 🟠 ALTA | Navegación |
| 11 | Validar permisos | 🔴 CRÍTICA | Security |
| 12 | Validar entrada | 🔴 CRÍTICA | Data |
| 13 | Manejar errores | 🟠 ALTA | UX |
| 14 | Usar async/await | 🟠 ALTA | Performance |
| 15 | Documentar cambios | 🟠 ALTA | Tracking |

---

## 📝 CÓMO USAR ESTE DOCUMENTO

1. **Antes de migrar un módulo**: Leer todas las reglas
2. **Durante la migración**: Consultarlo como referencia
3. **Al completar**: Verificar contra checklist
4. **Para nuevos devs**: Es la guía de estándares

**Ubicación**: `MatrixNext/DIRECTRICES_MIGRACION.md`

**Actualizar cuando**:
- Se descubra nuevo patrón útil
- Se encuentre una regla inconsistente
- Se agregue nuevo estándar

---

**Documento Oficial**  
**Versión**: 1.0  
**Aprobado**: 2026-01-02  
**Revisión Siguiente**: Mensual o cuando se descubra inconsistencia

