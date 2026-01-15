# PLAN DE MIGRACIÓN: PY_ControlCalidad

**Sprint**: 12  
**Módulo**: PY_ControlCalidad (Control de Calidad de Proyectos)  
**Fecha Inicio**: 2026-01-16  
**Fecha Entrega Estimada**: 2026-02-13  
**Esfuerzo**: 40 horas (5 días de 8h/día)  
**Estado**: 📋 PLAN DEFINIDO - Listo para kickoff

---

## 🎯 OBJETIVO

Migrar 100% del módulo **PY_ControlCalidad** de WebMatrix (.NET Framework 4.7) a **MatrixNext** (.NET 8 MVC), manteniendo paridad funcional completa.

**Resultados Esperados**:
- 6 MVC Controllers (1 principal + 1 maestro)
- 2 Services (lógica de negocio)
- 2 Adapters (acceso a datos)
- 8 Vistas Razor (Index + Modales)
- 2,300-2,500 LOC totales
- 0 errores en build
- 100% de QA funcional

---

## 📋 DESGLOSE POR ÉPICA

### ÉPICA 1: Infrastructure Base (Días 1-2, 8 horas)

**Objetivo**: Preparar base técnica (Adapters, DTOs, DbContext, DI)

#### Tarea 1.1: Verificar SP en SQL Server (1h)
- [ ] Conectar a SQL Server local
- [ ] Verificar existencia de SP:
  - `PY_ControlCalidad_Add`
  - `PY_ControlCalidad_Edit`
  - `PY_ControlCalidad_Del`
  - `PY_ControlCalidad_Get`
  - `PY_ControlCalidad_GetByTrabajo`
  - `PY_DetalleControlCalidad_Add`
  - `PY_DetalleControlCalidad_Get`
  - `PY_DetalleControlCalidad_DelxIdControl`
  - `PY_Preguntas_Get`
  - `PY_Preguntas_GetByTipo`
  - `PY_Preguntas_Add`
  - `PY_Preguntas_Edit`
  - `PY_Preguntas_Del`
- [ ] Documentar query exacta (parámetros, retorna)
- [ ] Si falta alguno, crear SP en script SQL

**Documentación**: `VERIFICACION_SP_PY_CONTROLCALIDAD.md`

#### Tarea 1.2: Crear DTOs (2h)
Ubicación: `MatrixNext.Web/DTOs/PY/ControlCalidad/`

**Archivos a crear**:
1. `ControlCalidadInputDto.cs` (30 LOC)
   ```csharp
   public long? TrabajoId { get; set; }
   [Required] public string Evaluador { get; set; }
   [Required] public string RolEvaluador { get; set; }
   [Required] public long PersonaId { get; set; }
   [Required] public DateTime Fecha { get; set; }
   [Required] public int TipoProceso { get; set; }
   public List<DetalleControlCalidadInputDto> Detalles { get; set; }
   ```

2. `ControlCalidadListDto.cs` (25 LOC)
   ```csharp
   public long Id { get; set; }
   public string Evaluador { get; set; }
   public string RolEvaluador { get; set; }
   public DateTime Fecha { get; set; }
   public string PersonaNombre { get; set; }
   public int TipoProceso { get; set; }
   public int DetallesCount { get; set; }
   ```

3. `ControlCalidadDetailDto.cs` (50 LOC)
   ```csharp
   public long Id { get; set; }
   public long TrabajoId { get; set; }
   public string Evaluador { get; set; }
   public string RolEvaluador { get; set; }
   public long PersonaId { get; set; }
   public DateTime Fecha { get; set; }
   public int TipoProceso { get; set; }
   public List<DetalleControlCalidadDetailDto> Detalles { get; set; }
   ```

4. `DetalleControlCalidadInputDto.cs` (20 LOC)
   ```csharp
   public long? IdPregunta { get; set; }
   public bool? Cumple { get; set; }
   public string Comentarios { get; set; }
   ```

5. `DetalleControlCalidadDetailDto.cs` (20 LOC)

6. `PreguntaInputDto.cs` (20 LOC)
   ```csharp
   [Required] public int IdProceso { get; set; }
   [Required] public string Pregunta { get; set; }
   public bool Activa { get; set; } = true;
   ```

7. `PreguntaListDto.cs` (15 LOC)
   ```csharp
   public long IdPregunta { get; set; }
   public int IdProceso { get; set; }
   public string Pregunta { get; set; }
   public bool Activa { get; set; }
   public string NombreProceso { get; set; }
   ```

**Total DTOs**: 200 LOC

#### Tarea 1.3: Crear Adapters (2h)
Ubicación: `MatrixNext.Infrastructure/Adapters/PY/`

**Archivo 1**: `IControlCalidadAdapter.cs` (60 LOC)
```csharp
public interface IControlCalidadAdapter
{
    Task<List<ControlCalidadListDto>> ObtenerPorTrabajoAsync(long trabajoId, int tipoProceso);
    Task<ControlCalidadDetailDto> ObtenerPorIdAsync(long id);
    Task<long> CrearAsync(ControlCalidadInputDto dto, int userId);
    Task EditarAsync(long id, ControlCalidadInputDto dto, int userId);
    Task EliminarAsync(long id);
    Task<List<DetalleControlCalidadDetailDto>> ObtenerDetallesAsync(long controlCalidadId);
    Task GuardarDetallesAsync(long controlCalidadId, List<DetalleControlCalidadInputDto> detalles, int userId);
}
```

**Archivo 2**: `ControlCalidadAdapter.cs` (180 LOC)
- Usar Dapper para ejecución de SP
- Implementar todas las operaciones CRUD
- Manejo de transacciones (Create + Detalles en 1 transacción)

**Archivo 3**: `IPreguntasAdapter.cs` (40 LOC)
```csharp
public interface IPreguntasAdapter
{
    Task<List<PreguntaListDto>> ObtenerTodasAsync();
    Task<List<PreguntaListDto>> ObtenerPorTipoAsync(int tipoProceso);
    Task<long> CrearAsync(PreguntaInputDto dto, int userId);
    Task EditarAsync(long id, PreguntaInputDto dto, int userId);
    Task ToggleActivoAsync(long id, int userId);
}
```

**Archivo 4**: `PreguntasAdapter.cs` (120 LOC)
- Usar Dapper
- Filtros por tipo de proceso
- Toggle de activo/inactivo

#### Tarea 1.4: DbContext y Mappings (1h)
- Verificar `PY_Entities` en EF Core mapea tablas
- Agregar si falta: `DbSet<PY_ControlCalidad>`, `DbSet<PY_DetalleControlCalidad>`
- Configurar FK si falta

#### Tarea 1.5: Registrar DI en Program.cs (1h)
```csharp
// Adapters
builder.Services.AddScoped<IControlCalidadAdapter, ControlCalidadAdapter>();
builder.Services.AddScoped<IPreguntasAdapter, PreguntasAdapter>();

// Services (ver Épica 2)
builder.Services.AddScoped<IControlCalidadService, ControlCalidadService>();
builder.Services.AddScoped<IPreguntasService, PreguntasService>();
```

**Entregable**: ✅ Infraestructura lista para servicios

---

### ÉPICA 2: Lógica de Negocio (Días 3-4, 10 horas)

**Objetivo**: Implementar Services con validaciones

#### Tarea 2.1: Crear IControlCalidadService (1h)
Ubicación: `MatrixNext.Core/Interfaces/`

```csharp
public interface IControlCalidadService
{
    Task<List<ControlCalidadListDto>> ObtenerPorTrabajoAsync(long trabajoId, int tipoProceso);
    Task<ControlCalidadDetailDto> ObtenerPorIdAsync(long id);
    Task<(bool success, string message, long id)> CrearAsync(ControlCalidadInputDto dto, int userId);
    Task<(bool success, string message)> EditarAsync(long id, ControlCalidadInputDto dto, int userId);
    Task<(bool success, string message)> EliminarAsync(long id, int userId);
    Task<List<PreguntaListDto>> ObtenerPreguntasActivasAsync(int tipoProceso);
}
```

#### Tarea 2.2: Implementar ControlCalidadService (4h)
Ubicación: `MatrixNext.Core/Services/PY/`

**Métodos**:
1. `ObtenerPorTrabajoAsync()` - Consultar todos los controles de un trabajo
2. `ObtenerPorIdAsync()` - Cargar 1 control con detalles
3. `CrearAsync()` - Validar + insertar (TRANSACCIÓN: PY_ControlCalidad + Detalles)
4. `EditarAsync()` - Validar + actualizar
5. `EliminarAsync()` - Eliminar (cascada a detalles)
6. `ObtenerPreguntasActivasAsync()` - Cargar preguntas por tipo

**Validaciones**:
- ✅ TrabajoId válido (existe en PY_Trabajo)
- ✅ Evaluador no vacío
- ✅ RolEvaluador no vacío
- ✅ Persona válida (existe en TH_Personas)
- ✅ Fecha válida (no futura)
- ✅ Al menos 1 pregunta respondida
- ✅ TipoProceso válido (enum)

**Logging**:
```csharp
_logger.LogInformation("ControlCalidad {Id} creado por usuario {UserId}", id, userId);
_logger.LogError(ex, "Error creando ControlCalidad para trabajo {TrabajoId}", dto.TrabajoId);
```

#### Tarea 2.3: Crear IPreguntasService (0.5h)
Ubicación: `MatrixNext.Core/Interfaces/`

```csharp
public interface IPreguntasService
{
    Task<List<PreguntaListDto>> ObtenerPorTipoAsync(int tipoProceso);
    Task<(bool success, string message, long id)> CrearAsync(PreguntaInputDto dto, int userId);
    Task<(bool success, string message)> EditarAsync(long id, PreguntaInputDto dto, int userId);
    Task<(bool success, string message)> ToggleActivoAsync(long id, int userId);
}
```

#### Tarea 2.4: Implementar PreguntasService (2h)
Ubicación: `MatrixNext.Core/Services/PY/`

**Métodos**:
1. `ObtenerPorTipoAsync()` - Listar preguntas activas por tipo
2. `CrearAsync()` - Validar + crear pregunta
3. `EditarAsync()` - Actualizar
4. `ToggleActivoAsync()` - Cambiar activo/inactivo

**Validaciones**:
- ✅ IdProceso válido
- ✅ Pregunta no vacía
- ✅ Pregunta no duplicada (por tipo)

#### Tarea 2.5: Crear métodos helper (2h)
- Helper: Validar TrabajoId existe
- Helper: Validar Persona existe
- Helper: Calcular % cumplimiento (detalles)
- Helper: Obtener nombre tipo proceso

**Entregable**: ✅ Servicios con lógica completa + logging

---

### ÉPICA 3: Controllers (Día 5, 6 horas)

**Objetivo**: Implementar endpoints REST

#### Tarea 3.1: Crear ControlCalidadController (4h)
Ubicación: `MatrixNext.Web/Areas/PY/Controllers/`

```csharp
[Area("PY")]
[Authorize]
[Route("api/py/controlcalidad")]
public class ControlCalidadController : Controller
{
    private readonly IControlCalidadService _service;
    
    // GET /py/controlcalidad/{tipoProceso}?trabajoId=X
    [HttpGet("{tipoProceso}")]
    public async Task<IActionResult> Index(int tipoProceso, long? trabajoId)
    {
        // Cargar controles del trabajo
        var controles = await _service.ObtenerPorTrabajoAsync(trabajoId ?? 0, tipoProceso);
        return Ok(controles);
    }
    
    // GET /py/controlcalidad/details/{id}
    [HttpGet("details/{id}")]
    public async Task<IActionResult> Details(long id)
    {
        var control = await _service.ObtenerPorIdAsync(id);
        if (control == null)
            return NotFound(new { success = false, message = "Control no encontrado" });
        
        return Ok(control);
    }
    
    // POST /py/controlcalidad/create
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] ControlCalidadInputDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = "Datos inválidos" });
        
        var (success, message, id) = await _service.CrearAsync(dto, User.GetUserId());
        return Json(new { success, message, id });
    }
    
    // POST /py/controlcalidad/edit/{id}
    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(long id, [FromBody] ControlCalidadInputDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = "Datos inválidos" });
        
        var (success, message) = await _service.EditarAsync(id, dto, User.GetUserId());
        return Json(new { success, message });
    }
    
    // POST /py/controlcalidad/delete/{id}
    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var (success, message) = await _service.EliminarAsync(id, User.GetUserId());
        return Json(new { success, message });
    }
    
    // GET /py/controlcalidad/preguntas/{tipoProceso}
    [HttpGet("preguntas/{tipoProceso}")]
    public async Task<IActionResult> ObtenerPreguntas(int tipoProceso)
    {
        var preguntas = await _service.ObtenerPreguntasActivasAsync(tipoProceso);
        return Ok(preguntas);
    }
}
```

#### Tarea 3.2: Crear PreguntasController (2h)
Ubicación: `MatrixNext.Web/Areas/PY/Controllers/`

```csharp
[Area("PY")]
[Authorize]
[Route("api/py/preguntas")]
public class PreguntasController : Controller
{
    private readonly IPreguntasService _service;
    
    // GET /py/preguntas/{tipoProceso}
    [HttpGet("{tipoProceso}")]
    public async Task<IActionResult> Index(int tipoProceso)
    {
        var preguntas = await _service.ObtenerPorTipoAsync(tipoProceso);
        return Ok(preguntas);
    }
    
    // POST /py/preguntas/create
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] PreguntaInputDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { success = false, message = "Datos inválidos" });
        
        var (success, message, id) = await _service.CrearAsync(dto, User.GetUserId());
        return Json(new { success, message, id });
    }
    
    // POST /py/preguntas/edit/{id}
    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(long id, [FromBody] PreguntaInputDto dto)
    {
        var (success, message) = await _service.EditarAsync(id, dto, User.GetUserId());
        return Json(new { success, message });
    }
    
    // POST /py/preguntas/toggle/{id}
    [HttpPost("toggle/{id}")]
    public async Task<IActionResult> Toggle(long id)
    {
        var (success, message) = await _service.ToggleActivoAsync(id, User.GetUserId());
        return Json(new { success, message });
    }
}
```

**Entregable**: ✅ 6 endpoints REST + error handling

---

### ÉPICA 4: Vistas y UI (Días 6-7, 10 horas)

**Objetivo**: Crear UI con AJAX-first, Modales, Grids

#### Tarea 4.1: Crear estructura de carpetas y vistas (1h)
```
Areas/PY/Views/
├── ControlCalidad/
│   ├── Index.cshtml
│   ├── _Form.cshtml
│   └── _DetallesGrid.cshtml
└── Preguntas/
    ├── Index.cshtml
    └── _Form.cshtml
```

#### Tarea 4.2: Vista Principal - ControlCalidad/Index.cshtml (3h)

**Contenido**:
```html
<!-- Header con filtros -->
<div class="card">
    <div class="card-header">
        <h5>Control de Calidad</h5>
        <button class="btn btn-primary" data-ajax-modal data-url="@Url.Action("Create")">
            Nueva Evaluación
        </button>
    </div>
    
    <!-- Grid con DataTables -->
    <div class="table-responsive">
        <table id="gridControlCalidad" class="table table-striped">
            <thead>
                <tr>
                    <th>Evaluador</th>
                    <th>Rol</th>
                    <th>Persona</th>
                    <th>Fecha</th>
                    <th>Preguntas</th>
                    <th>Acciones</th>
                </tr>
            </thead>
        </table>
    </div>
</div>

<!-- Modal genérico -->
<div id="modalContainer"></div>

<script>
    // Cargar grid con DataTables
    $('#gridControlCalidad').DataTable({
        ajax: {
            url: '@Url.Action("Index")',
            type: 'GET'
        },
        columns: [ ... ],
        columnDefs: [ ... ]
    });
    
    // Handlers AJAX modal
    $(document).on('click', '[data-ajax-modal]', function(e) {
        e.preventDefault();
        const url = $(this).data('url');
        $.get(url, function(html) {
            $('#modalContainer').html(html);
            $('#modalForm').modal('show');
        });
    });
</script>
```

**LOC**: 120 líneas

#### Tarea 4.3: Modal de Formulario - _Form.cshtml (4h)

**Contenido Principal**:
1. Header del modal
2. Campos input (Evaluador, RolEvaluador, PersonaId, Fecha)
3. Grid dinámico de preguntas (RadioButton Si/No + TextBox comentario)
4. Footer (Cancelar, Guardar)

```html
<div class="modal-header">
    <h5 class="modal-title">Evaluación de Calidad</h5>
    <button type="button" class="close" data-dismiss="modal">&times;</button>
</div>

<form id="formControlCalidad" method="post" data-ajax-form>
    <div class="modal-body">
        <!-- Campos -->
        <div class="form-group">
            <label>Evaluador</label>
            <input type="text" name="Evaluador" class="form-control" required />
        </div>
        
        <div class="form-group">
            <label>Rol del Evaluador</label>
            <input type="text" name="RolEvaluador" class="form-control" required />
        </div>
        
        <div class="form-group">
            <label>Analista Responsable</label>
            <select name="PersonaId" class="form-control" id="ddlPersona" required></select>
        </div>
        
        <div class="form-group">
            <label>Fecha de Evaluación</label>
            <input type="date" name="Fecha" class="form-control" required />
        </div>
        
        <!-- Grid de preguntas dinámico -->
        <div class="form-group">
            <label>Preguntas de Evaluación</label>
            <div class="table-responsive">
                <table class="table table-sm" id="tblPreguntas">
                    <thead>
                        <tr>
                            <th>Pregunta</th>
                            <th>Cumple</th>
                            <th>Comentarios</th>
                        </tr>
                    </thead>
                    <tbody id="tbodyPreguntas"></tbody>
                </table>
            </div>
        </div>
    </div>
    
    <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
        <button type="submit" class="btn btn-primary">Guardar</button>
    </div>
</form>

<script>
    // Cargar personas en select
    $.get('@Url.Action("ObtenerPersonas", "Empleados")', function(personas) {
        $.each(personas, function(i, p) {
            $('#ddlPersona').append($('<option></option>').attr('value', p.id).text(p.nombre));
        });
    });
    
    // Cargar preguntas dinámicamente
    $.get('@Url.Action("ObtenerPreguntas", "ControlCalidad", new { tipoProceso = ViewBag.TipoProceso })', function(preguntas) {
        $.each(preguntas, function(i, p) {
            const row = $('<tr></tr>')
                .append($('<td></td>').text(p.pregunta))
                .append($('<td></td>').append(
                    $('<input type="radio" name="respuesta[' + p.idPregunta + ']" value="1" />').text('Sí '),
                    $('<input type="radio" name="respuesta[' + p.idPregunta + ']" value="0" />').text('No ')
                ))
                .append($('<td></td>').append(
                    $('<textarea name="comentario[' + p.idPregunta + ']" class="form-control" rows="2"></textarea>')
                ));
            
            $('#tbodyPreguntas').append(row);
        });
    });
    
    // Submit del formulario
    $(document).on('submit', '#formControlCalidad', function(e) {
        e.preventDefault();
        
        const data = {
            Evaluador: $('[name="Evaluador"]').val(),
            RolEvaluador: $('[name="RolEvaluador"]').val(),
            PersonaId: $('[name="PersonaId"]').val(),
            Fecha: $('[name="Fecha"]').val(),
            Detalles: [ /* mapear respuestas */ ]
        };
        
        $.ajax({
            url: '@Url.Action("Create")',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function(response) {
                if (response.success) {
                    showToast('Evaluación guardada', 'success');
                    $('#modalForm').modal('hide');
                    $('#gridControlCalidad').DataTable().ajax.reload();
                } else {
                    showToast(response.message, 'error');
                }
            }
        });
    });
</script>
```

**LOC**: 150 líneas

#### Tarea 4.4: Vista Preguntas/Index.cshtml (2h)

**Contenido similar a ControlCalidad**:
- Grid de preguntas
- Botón "Nueva Pregunta"
- Acciones: Editar, Toggle (activo/inactivo)

**LOC**: 80 líneas

#### Tarea 4.5: Modal Preguntas/_Form.cshtml (1h)

**Contenido**:
- Campos: IdProceso (select), Pregunta (textarea), Activa (checkbox)

**LOC**: 80 líneas

#### Tarea 4.6: Estilos CSS (1h)
Ubicación: `wwwroot/css/py-controlcalidad.css`

**Contenido**:
- Grid responsive
- Modal styling
- Tabla de preguntas (padding, borders)
- Estados (cumple/no cumple) con colores

**LOC**: 100 líneas

#### Tarea 4.7: Scripts AJAX/JS (2h)
Ubicación: `wwwroot/js/py-controlcalidad.js`

**Funciones**:
- `loadControlCalidad(trabajoId, tipoProceso)` - Cargar grid
- `openModalCreate(tipoProceso)` - Abrir modal crear
- `openModalEdit(id, tipoProceso)` - Cargar y abrir modal editar
- `deleteControlCalidad(id)` - Eliminar con confirmación
- `loadPreguntasByTipo(tipoProceso)` - Cargar preguntas dinámicamente
- `mapDetallesFromForm()` - Serializar detalles desde form

**LOC**: 300 líneas

**Entregable**: ✅ UI completa con AJAX, Modales, Grids

---

### ÉPICA 5: Testing y QA (Día 8, 4 horas)

**Objetivo**: Validar 100% funcionalidad

#### Tarea 5.1: Testing Manual (2h)
- [ ] Crear nueva evaluación (todos los tipos: Campo, Moderadora, etc)
- [ ] Editar evaluación existente
- [ ] Eliminar evaluación (con confirmación)
- [ ] Búsqueda/Filtros funcionan
- [ ] Paginación del grid
- [ ] Manejo de errores (validaciones, BD offline)
- [ ] Auditoría: crear log de acciones

#### Tarea 5.2: QA de Seguridad (1h)
- [ ] `[Authorize]` aplicado en todos los controllers
- [ ] Solo usuarios autenticados pueden acceder
- [ ] Validación de permisos (ej: solo admin puede editar preguntas)

#### Tarea 5.3: Verificación de Datos (1h)
- [ ] Datos guardados correctamente en BD
- [ ] FK validadas (TrabajoId, PersonaId existen)
- [ ] Detalles (respuestas) guardadas correctamente
- [ ] Auditoría (RegistradoPor, FechaRegistro) completada

**Entregable**: ✅ Módulo validado al 100%

---

### ÉPICA 6: Documentación (Día 9, 2 horas)

#### Tarea 6.1: Completar MIGRACION_PY_CONTROLCALIDAD.md (1h)

**Contenido**:
- ✅ Checklist de implementación
- ✅ Páginas migradas (6/6)
- ✅ SP mapeados
- ✅ DTOs creados
- ✅ Testing realizado
- ✅ Problemas encontrados + soluciones

#### Tarea 6.2: Actualizar documentación relacionada (1h)
- [ ] Actualizar DASHBOARD_MIGRACION.md
- [ ] Actualizar MODULOS_MIGRACION.md
- [ ] Actualizar menú _Sidebar.cshtml (agregar link a ControlCalidad)
- [ ] Documentar en PLAN_EJECUCION_SPRINTS.md

**Entregable**: ✅ Documentación completa

---

## 📊 DESGLOSE HORARIO

| Épica | Tarea | Horas | Resultado |
|-------|-------|-------|-----------|
| 1 | Verificar SP | 1 | SP validados |
| 1 | DTOs | 2 | 200 LOC DTO |
| 1 | Adapters | 2 | 400 LOC Adapters |
| 1 | DbContext | 1 | DbContext actualizado |
| 1 | DI | 1 | Program.cs actualizado |
| 2 | Interfaces | 1.5 | 100 LOC interfaces |
| 2 | ControlCalidadService | 4 | 280 LOC service |
| 2 | PreguntasService | 2 | 200 LOC service |
| 2 | Helpers | 2 | 150 LOC helpers |
| 3 | ControlCalidadController | 4 | 250 LOC controller |
| 3 | PreguntasController | 2 | 150 LOC controller |
| 4 | Vistas | 10 | 600 LOC Razor |
| 4 | CSS | 1 | 100 LOC CSS |
| 4 | JS | 2 | 300 LOC JS |
| 5 | Testing Manual | 2 | 100% validado |
| 5 | QA Seguridad | 1 | ✅ Verificado |
| 5 | QA Datos | 1 | ✅ Verificado |
| 6 | MIGRACION.md | 1 | Documentación |
| 6 | Actualizar docs | 1 | Docs completas |

**TOTAL**: 40 horas (5 días x 8h)

---

## 🎯 CRITERIOS DE ACEPTACIÓN

### Build
- [ ] 0 errores de compilación
- [ ] 0 warnings críticos
- [ ] Intellisense funciona correctamente

### Funcionalidad
- [ ] CRUD completo de evaluaciones (Create, Read, Update, Delete)
- [ ] CRUD maestro de preguntas
- [ ] Todas las 6 páginas migradas
- [ ] Preguntas dinámicas cargadas por tipo
- [ ] Grid paginado y filtrable
- [ ] Modales abren/cierran correctamente

### Seguridad
- [ ] `[Authorize]` en todos los controllers
- [ ] Solo usuarios autenticados acceden
- [ ] Validaciones en server (no solo client)
- [ ] No se exponen stack traces

### Datos
- [ ] SP ejecutados correctamente
- [ ] FK validadas
- [ ] Auditoría registrada
- [ ] Transacciones funcionan

### Documentación
- [ ] MIGRACION_PY_CONTROLCALIDAD.md completa
- [ ] DASHBOARD actualizado
- [ ] Menú actualizado
- [ ] Sin código comentado

---

## 🚨 RIESGOS Y MITIGACIÓN

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|-------------|--------|-----------|
| SP no existen en BD | 🟡 Media | 🔴 Alto | ✅ Verificar antes (Tarea 1.1) |
| FK a PY_Trabajo no valida | 🟡 Media | 🔴 Alto | ✅ Validar en service |
| Grid dinámico complejo en Razor | 🟡 Media | 🟠 Medio | ✅ Usar EditorTemplate |
| Auditoría incompleta | 🟢 Baja | 🟠 Medio | ✅ Testing automatiza verificación |
| Performance: Grid con 1000+ filas | 🟢 Baja | 🟠 Medio | ✅ Agregar paginación servidor |

---

## 📌 PREREQUISITOS

- [ ] ✅ PY_Proyectos está 100% funcional
- [ ] ✅ TH_TalentoHumano está 100% funcional
- [ ] ✅ BD: SP existe o se crearán
- [ ] ✅ BD: FK a PY_Trabajo y TH_Personas existen
- [ ] ✅ DbContext (PY_Entities) actualizado
- [ ] ✅ Enum TipoProceso incluye tipos necesarios

---

## ✅ CHECKLIST INICIO

Antes de comenzar la implementación, verificar:

- [ ] Análisis completado (ANALISIS_PY_CONTROLCALIDAD.md)
- [ ] Todas las dependencias disponibles (PY_Proyectos, TH_TalentoHumano)
- [ ] BD verificada (SP, FK, tablas)
- [ ] Team sincronizado en reunión kickoff
- [ ] Rama `feature/py-controlcalidad` creada
- [ ] Equipos asignados:
  - [ ] Backend (Adapters, Services, Controllers): Dev 1
  - [ ] Frontend (Views, JS, CSS): Dev 2
  - [ ] QA (Testing, Documentación): Dev 3

---

## 📅 CRONOGRAMA DETALLADO

### Semana 1 (Lunes-Viernes)

- **Lunes (Día 1)**: Épica 1 (Infra base) → 8h
- **Martes (Día 2)**: Épica 2.1-2.2 (ControlCalidadService) → 8h
- **Miércoles (Día 3)**: Épica 2.3-2.5 (PreguntasService + Helpers) → 8h
- **Jueves (Día 4)**: Épica 3 (Controllers) → 8h
- **Viernes (Día 5)**: Épica 4.1-4.7 (Vistas) → 8h

### Semana 2 (Lunes-Miércoles)

- **Lunes (Día 6)**: Épica 4 Continuación (Vistas avanzadas) → 8h
- **Martes (Día 7)**: Épica 5 (QA) → 4h + Épica 6 (Docs) → 4h
- **Miércoles**: Buffer / Ajustes / Revisión final

**Total**: ~40-45 horas efectivas

---

## 🎁 ENTREGABLES FINALES

1. ✅ 2 Controllers (ControlCalidad, Preguntas)
2. ✅ 2 Services (ControlCalidadService, PreguntasService)
3. ✅ 2 Adapters (ControlCalidadAdapter, PreguntasAdapter)
4. ✅ 7 DTOs (Input, List, Detail)
5. ✅ 6 Vistas Razor (Index + Modales)
6. ✅ JS + CSS (AJAX, Modales, Grid)
7. ✅ MIGRACION_PY_CONTROLCALIDAD.md completa
8. ✅ Documentación actualizada
9. ✅ Build 0 errores
10. ✅ QA 100% completado

---

**Plan Aprobado**: ✅  
**Listo para Kickoff**: ✅  
**Fecha Inicio**: 2026-01-16  
**Fecha Entrega**: 2026-02-13  

---

**Documento**: PLAN_MIGRACION_PY_CONTROLCALIDAD.md  
**Versión**: 1.0  
**Fecha**: 2026-01-15  
**Autor**: GitHub Copilot
