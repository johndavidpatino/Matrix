# PLAN SPRINTS 1-6: FI Grupos 1-6

**Objetivo**: Documentación exhaustiva de cada FI grupo para implementación secuencial
**Total**: 6 sprints, 704 horas, 12 semanas (sin Pre-1; con Pre-1 = semanas 3-14)
**Estructura**: Sprint por grupo, dependencias claras, patrones compartidos

---

## ESTRUCTURA GENERAL POR SPRINT

Cada sprint FI sigue este patrón:

```
SPRINT X: Grupo Y (Z horas, N semanas)

1. Resumen ejecutivo
   - Páginas incluidas
   - Funcionalidad principal
   - Usuarios finales
   - Complejidad estimada

2. Análisis de páginas
   - Tabla con nombre, descripción, CRUD, complejidad, SP
   - Detalles de lógica

3. Patrones compartidos
   - Grid + filtros + export
   - Modales CRUD
   - SP utilizadas

4. Estructura de carpetas
   - Controllers, Services, Views, Data

5. Tareas de implementación
   - Tarea A: Models + DbContext
   - Tarea B: Controllers + Views
   - Tarea C: Services
   - Tarea D: Testing

6. Dependencias
   - Servicios consumidos
   - SP requeridas

7. Checklist de entrega
   - Compilación
   - Funcionalidad
   - Testing
   - Documentación

8. Timeline
   - Días 1-5 por tarea
```

---

# SPRINT 1: Grupo 1 - Control Presupuestos

**Duración**: 2 semanas (92 horas)  
**Semanas**: 3-4 (post-Pre-1)  
**Páginas**: 4  
**Patrón principal**: Grid + filtros + modal de detalles  
**Complejidad**: 🟠 Media

## 1.1 Resumen

**Funcionalidad**: Gestión de presupuestos y control de costos por trabajo

**Páginas**:
1. ControlPresupuestos.aspx - Listado y edición presupuestos
2. NominaDistribucionCostos.aspx - Distribución de costos salariales
3. AsignacionPresupuestos.aspx - Asignación de presupuesto a actividades
4. VerificacionPresupuestosRealizados.aspx - Verificación vs. realizado

**Usuarios**: Coordinadores, gerentes presupuestales

**Complejidad**: 🟠 Media (lógica de distribución, cálculos, validaciones)

## 1.2 Análisis de Páginas

| Página | Descripción | CRUD | Complejidad | SP Críticos |
|--------|-------------|------|-------------|-------------|
| ControlPresupuestos | Grid presupuestos, editar valores, validar totales | CRU | Media | CC_DetallePresupuesto*, CC_ObtenerPresupuestos |
| NominaDistribucionCostos | Distribuir costos entre centros de costo | R + Update | Media | CC_LiquidarPlanillas, CC_CuentasContables |
| AsignacionPresupuestos | Asignar presupuesto a actividades | CRU | Baja | CC_ActividadesPresupuestadas |
| VerificacionPresupuestosRealizados | Comparar presupuesto vs. realizado | R | Baja | CC_ConsecutivoCC*, CC_Produccion |

**Detalles**:

### ControlPresupuestos.aspx
- **Función**: Crear, leer, actualizar presupuestos
- **Entrada**: Filtro por período, trabajo, estado
- **Salida**: Tabla con presupuestos, totales por fila/columna
- **CRUD**:
  - Create: Nuevo presupuesto (modal)
  - Read: Grid de presupuestos
  - Update: Editar valores en modal
  - Delete: Anular (soft delete)
- **SP**: `CC_ObtenerPresupuestos`, `CC_DetallePresupuesto*` (CRUD), `CC_GuardarPresupuesto`
- **Validaciones**: 
  - Presupuesto no puede ser < 0
  - Total presupuesto = suma detalles
  - No permitir editar si está aprobado

### NominaDistribucionCostos.aspx
- **Función**: Distribuir costo salarial entre centros de costo
- **Entrada**: Período, empleado
- **Salida**: Tabla de distribución (% por centro)
- **Lógica**: 
  - Obtener liquidación del período
  - Permitir ajustar % distribución
  - Guardar en CC_DistribucionCostos
- **SP**: `CC_LiquidarPlanillas`, `CC_CuentasContables`

### AsignacionPresupuestos.aspx
- **Función**: Asignar presupuesto a actividades
- **Entrada**: Presupuesto, actividades
- **Salida**: Grid de actividades con presupuesto asignado
- **Lógica**:
  - Obtener presupuesto
  - Listar actividades disponibles
  - Asignar valores
  - Validar no sobreasignar

### VerificacionPresupuestosRealizados.aspx
- **Función**: Comparar presupuesto vs. realizado
- **Entrada**: Período
- **Salida**: Grid con columnas: Presupuesto, Realizado, Varianza, %
- **SP**: `CC_ResumenesdeProduccion` (o similar)
- **Lógica**: Read-only, solo reportes

## 1.3 Patrones Compartidos

### Patrón 1: Grid + Filtros + Export

**Componente**: `Components/FI/GridPresupuestosComponent.cs`

```html
<!-- Vista -->
<div class="card">
    <div class="card-header">
        <h3>Control de Presupuestos</h3>
        
        <!-- Filtros -->
        <div class="row">
            <div class="col-md-3">
                <label>Período</label>
                <input type="month" id="filtro-periodo" class="form-control">
            </div>
            <div class="col-md-3">
                <label>Trabajo</label>
                <select id="filtro-trabajo" class="form-control">
                    <option>Todos</option>
                </select>
            </div>
            <div class="col-md-3">
                <label>Estado</label>
                <select id="filtro-estado" class="form-control">
                    <option value="">Todos</option>
                    <option value="1">Borrador</option>
                    <option value="2">Aprobado</option>
                </select>
            </div>
            <div class="col-md-3" style="margin-top: 24px;">
                <button class="btn btn-primary" id="btn-buscar">Buscar</button>
                <button class="btn btn-secondary" id="btn-exportar">Exportar</button>
            </div>
        </div>
    </div>
    
    <div class="card-body">
        <!-- Grid con DataTables -->
        <table id="grid-presupuestos" class="table table-striped">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Período</th>
                    <th>Trabajo</th>
                    <th>Monto Presupuesto</th>
                    <th>Monto Realizado</th>
                    <th>Varianza</th>
                    <th>Estado</th>
                    <th>Acciones</th>
                </tr>
            </thead>
            <tbody></tbody>
        </table>
    </div>
</div>

<!-- Modal CRUD -->
<div class="modal fade" id="modalPresupuesto">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 id="modal-titulo">Nuevo Presupuesto</h5>
            </div>
            <div class="modal-body">
                <form id="form-presupuesto">
                    <div class="form-group">
                        <label>Período</label>
                        <input type="month" id="inp-periodo" class="form-control" required>
                    </div>
                    <div class="form-group">
                        <label>Trabajo</label>
                        <select id="inp-trabajo" class="form-control" required>
                            <option>Seleccionar...</option>
                        </select>
                    </div>
                    <div class="form-group">
                        <label>Monto Presupuesto</label>
                        <input type="number" id="inp-monto" class="form-control" 
                            step="0.01" required min="0">
                    </div>
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
                <button type="button" class="btn btn-primary" id="btn-guardar-presupuesto">Guardar</button>
            </div>
        </div>
    </div>
</div>
```

**JavaScript (Cliente)**:
```javascript
// Areas/FI/Views/ControlPresupuestos/Index.js

$(document).ready(function() {
    // Inicializar DataTable
    const table = $('#grid-presupuestos').DataTable({
        language: { url: '/lib/es.json' },
        pageLength: 10,
        processing: true,
        serverSide: true,
        ajax: {
            url: '/FI/ControlPresupuestos/GetPresupuestos',
            type: 'POST',
            data: function(d) {
                d.periodo = $('#filtro-periodo').val();
                d.trabajo = $('#filtro-trabajo').val();
                d.estado = $('#filtro-estado').val();
            }
        }
    });
    
    // Buscar
    $('#btn-buscar').click(function() {
        table.draw();
    });
    
    // Exportar
    $('#btn-exportar').click(function() {
        const periodo = $('#filtro-periodo').val();
        window.location = `/FI/ControlPresupuestos/Exportar?periodo=${periodo}`;
    });
    
    // Nuevo
    $('#btn-nuevo').click(function() {
        $('#form-presupuesto')[0].reset();
        $('#modal-titulo').text('Nuevo Presupuesto');
        $('#modalPresupuesto').modal('show');
    });
    
    // Guardar
    $('#btn-guardar-presupuesto').click(function() {
        const data = {
            idPresupuesto: $('#inp-id').val() || 0,
            periodo: $('#inp-periodo').val(),
            idTrabajo: $('#inp-trabajo').val(),
            monto: $('#inp-monto').val()
        };
        
        $.post('/FI/ControlPresupuestos/Guardar', data, function(result) {
            if (result.success) {
                alert('Guardado exitosamente');
                $('#modalPresupuesto').modal('hide');
                table.draw();
            } else {
                alert('Error: ' + result.message);
            }
        });
    });
});
```

### Patrón 2: Modal CRUD

**Ubicación**: `Components/FI/Shared/ModalCRUD.cshtml`

```html
<!-- Modal genérico reutilizable -->
<div class="modal fade" id="@Model.ModalId">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 id="modal-titulo">@Model.Title</h5>
                <button type="button" class="close" data-dismiss="modal">
                    <span>&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <form id="@Model.FormId" class="form-modal">
                    <!-- Campos generados por template -->
                    @foreach (var field in Model.Fields)
                    {
                        <div class="form-group">
                            <label>@field.Label</label>
                            <input type="@field.Type" 
                                id="inp-@field.Name" 
                                class="form-control" 
                                placeholder="@field.Placeholder"
                                @if (field.Required) { <text> required</text> }>
                        </div>
                    }
                </form>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">
                    Cancelar
                </button>
                <button type="button" class="btn btn-primary" id="btn-save-modal">
                    Guardar
                </button>
            </div>
        </div>
    </div>
</div>
```

## 1.4 Estructura de Carpetas

```
Areas/FI/
├── Controllers/
│   └── ControlPresupuestosController.cs
├── Services/
│   ├── IPresupuestoService.cs
│   └── PresupuestoService.cs
├── Data/
│   ├── Models/ (usar modelos compartidos con CC)
│   ├── DTOs/
│   │   ├── PresupuestoDTO.cs
│   │   ├── DetallePresupuestoDTO.cs
│   │   └── VerificacionPresupuestoDTO.cs
│   └── Adapters/
│       └── FIControlPresupuestosAdapter.cs
├── Views/
│   ├── ControlPresupuestos/
│   │   ├── Index.cshtml
│   │   ├── Index.js
│   │   ├── Detalles.cshtml
│   │   └── Detalles.js
│   └── Shared/
│       ├── ModalCRUD.cshtml
│       └── GridComponentes.cshtml
└── Components/
    └── FIGridComponent.cs
```

## 1.5 Tareas de Implementación

### Tarea 1.1: Models + DTOs (16 horas)

**Archivos**:
- `Areas/FI/Data/Models/FI_Presupuesto.cs` (mapear desde SQL)
- `Areas/FI/Data/Models/FI_DetallePresupuesto.cs`
- `Areas/FI/Data/DTOs/PresupuestoDTO.cs`
- `Areas/FI/Data/DTOs/DetallePresupuestoDTO.cs`

```csharp
// Areas/FI/Data/DTOs/PresupuestoDTO.cs

public class PresupuestoDTO
{
    public long Id { get; set; }
    public int Periodo { get; set; }
    public long IdTrabajo { get; set; }
    public string CodigoTrabajo { get; set; }
    public string NombreTrabajo { get; set; }
    public decimal MontoPresupuesto { get; set; }
    public decimal MontoRealizado { get; set; }
    public decimal Varianza => MontoRealizado - MontoPresupuesto;
    public byte Estado { get; set; }
    public string EstadoNombre { get; set; }
    public DateTime FechaRegistro { get; set; }
    public List<DetallePresupuestoDTO> Detalles { get; set; }
}

public class DetallePresupuestoDTO
{
    public long Id { get; set; }
    public long IdPresupuesto { get; set; }
    public long IdActividad { get; set; }
    public string CodigoActividad { get; set; }
    public string DescripcionActividad { get; set; }
    public decimal Cantidad { get; set; }
    public decimal ValorUnitario { get; set; }
    public decimal Subtotal => Cantidad * ValorUnitario;
}
```

### Tarea 1.2: Controller + Views (24 horas)

```csharp
// Areas/FI/Controllers/ControlPresupuestosController.cs

[Area("FI")]
[Route("FI/[controller]")]
[Authorize]
public class ControlPresupuestosController : Controller
{
    private readonly IPresupuestoService _service;
    private readonly ILogger<ControlPresupuestosController> _logger;
    
    public ControlPresupuestosController(IPresupuestoService service, 
        ILogger<ControlPresupuestosController> logger)
    {
        _service = service;
        _logger = logger;
    }
    
    // GET: /FI/ControlPresupuestos
    public IActionResult Index()
    {
        _logger.LogInformation("Acceder a ControlPresupuestos");
        return View();
    }
    
    // POST: /FI/ControlPresupuestos/GetPresupuestos (DataTable AJAX)
    [HttpPost]
    public IActionResult GetPresupuestos(DataTablesRequest request, 
        string periodo, string trabajo, string estado)
    {
        try
        {
            var data = _service.ObtenerPresupuestos(
                periodo, trabajo, estado, 
                request.Start, request.Length
            );
            
            return Json(new DataTablesResponse
            {
                Draw = request.Draw,
                RecordsTotal = data.Total,
                RecordsFiltered = data.Filtered,
                Data = data.Items
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en GetPresupuestos");
            return Json(new { error = ex.Message });
        }
    }
    
    // POST: /FI/ControlPresupuestos/Guardar
    [HttpPost]
    public IActionResult Guardar(PresupuestoDTO model)
    {
        try
        {
            // Validar
            if (model.MontoPresupuesto < 0)
                return Json(new { success = false, 
                    message = "Monto debe ser positivo" });
            
            // Guardar
            var result = _service.GuardarPresupuesto(model);
            
            return Json(new { success = true, message = "Guardado", data = result });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en Guardar");
            return Json(new { success = false, message = ex.Message });
        }
    }
    
    // POST: /FI/ControlPresupuestos/Eliminar
    [HttpPost]
    public IActionResult Eliminar(long id)
    {
        try
        {
            _service.EliminarPresupuesto(id);
            return Json(new { success = true, message = "Eliminado" });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
    }
    
    // GET: /FI/ControlPresupuestos/Exportar
    public IActionResult Exportar(string periodo)
    {
        try
        {
            var data = _service.ObtenerPresupuestos(periodo, "", "", 0, 10000);
            var excel = _service.ExportarExcel(data.Items);
            
            return File(excel, 
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Presupuestos_{periodo}.xlsx");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en Exportar");
            return BadRequest(ex.Message);
        }
    }
}
```

### Tarea 1.3: Service Layer (20 horas)

```csharp
// Areas/FI/Services/IPresupuestoService.cs

public interface IPresupuestoService
{
    // Read
    (List<PresupuestoDTO> Items, int Total, int Filtered) ObtenerPresupuestos(
        string periodo, string trabajo, string estado, int skip, int take);
    
    PresupuestoDTO ObtenerPresupuestoDetalle(long id);
    
    // Create/Update
    PresupuestoDTO GuardarPresupuesto(PresupuestoDTO model);
    
    // Delete
    void EliminarPresupuesto(long id);
    
    // Export
    byte[] ExportarExcel(List<PresupuestoDTO> data);
}

// Areas/FI/Services/PresupuestoService.cs

public class PresupuestoService : IPresupuestoService
{
    private readonly FIControlPresupuestosAdapter _adapter;
    private readonly ILogger<PresupuestoService> _logger;
    
    public PresupuestoService(FIControlPresupuestosAdapter adapter,
        ILogger<PresupuestoService> logger)
    {
        _adapter = adapter;
        _logger = logger;
    }
    
    public (List<PresupuestoDTO> Items, int Total, int Filtered) 
        ObtenerPresupuestos(string periodo, string trabajo, string estado, 
        int skip, int take)
    {
        _logger.LogInformation($"Obtener presupuestos: período={periodo}");
        
        var data = _adapter.ObtenerPresupuestos(periodo, trabajo, estado);
        var total = data.Count;
        var items = data.Skip(skip).Take(take).ToList();
        
        return (items, total, items.Count);
    }
    
    public PresupuestoDTO GuardarPresupuesto(PresupuestoDTO model)
    {
        // Validar
        if (model.MontoPresupuesto < 0)
            throw new ArgumentException("Monto debe ser positivo");
        
        // Validar suma detalles
        var sumaDetalles = model.Detalles?.Sum(d => d.Subtotal) ?? 0;
        if (sumaDetalles != model.MontoPresupuesto)
        {
            _logger.LogWarning(
                $"Suma detalles ({sumaDetalles}) != monto presupuesto ({model.MontoPresupuesto})"
            );
        }
        
        // Guardar
        var result = _adapter.GuardarPresupuesto(model);
        _logger.LogInformation($"Presupuesto {result.Id} guardado");
        return result;
    }
    
    public byte[] ExportarExcel(List<PresupuestoDTO> data)
    {
        _logger.LogInformation($"Exportar {data.Count} presupuestos a Excel");
        
        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Presupuestos");
        
        // Headers
        ws.Cell(1, 1).Value = "ID";
        ws.Cell(1, 2).Value = "Período";
        ws.Cell(1, 3).Value = "Trabajo";
        ws.Cell(1, 4).Value = "Monto Presupuesto";
        ws.Cell(1, 5).Value = "Monto Realizado";
        ws.Cell(1, 6).Value = "Varianza";
        
        // Datos
        int row = 2;
        foreach (var item in data)
        {
            ws.Cell(row, 1).Value = item.Id;
            ws.Cell(row, 2).Value = item.Periodo;
            ws.Cell(row, 3).Value = item.CodigoTrabajo;
            ws.Cell(row, 4).Value = item.MontoPresupuesto;
            ws.Cell(row, 5).Value = item.MontoRealizado;
            ws.Cell(row, 6).Value = item.Varianza;
            row++;
        }
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}
```

### Tarea 1.4: Adapter + Testing (16 horas)

```csharp
// Areas/FI/Data/Adapters/FIControlPresupuestosAdapter.cs

public class FIControlPresupuestosAdapter
{
    private readonly CCFinzOpeDataAdapter _ccAdapter;
    private readonly ILogger<FIControlPresupuestosAdapter> _logger;
    
    public FIControlPresupuestosAdapter(CCFinzOpeDataAdapter ccAdapter,
        ILogger<FIControlPresupuestosAdapter> logger)
    {
        _ccAdapter = ccAdapter;
        _logger = logger;
    }
    
    public List<PresupuestoDTO> ObtenerPresupuestos(string periodo, 
        string trabajo, string estado)
    {
        _logger.LogInformation(
            $"Adapter: obtener presupuestos (período={periodo})"
        );
        
        // Usar CC_FinzOpe
        var detalles = _ccAdapter.ObtenerDetallePresupuesto(
            DateTime.ParseExact(periodo, "yyyy-MM", CultureInfo.InvariantCulture)
        );
        
        // Mapear a PresupuestoDTO
        var presupuestos = detalles
            .GroupBy(d => d.IdPresupuesto)
            .Select(g => new PresupuestoDTO
            {
                Id = g.Key,
                MontoPresupuesto = g.Sum(x => x.Subtotal),
                Detalles = g.Select(d => new DetallePresupuestoDTO
                {
                    IdPresupuesto = d.IdPresupuesto,
                    IdActividad = d.IdActividad,
                    Cantidad = d.Cantidad,
                    ValorUnitario = d.ValorUnitario
                }).ToList()
            })
            .ToList();
        
        return presupuestos;
    }
    
    public PresupuestoDTO GuardarPresupuesto(PresupuestoDTO model)
    {
        // Insertar via CC_FinzOpe (usar transaction)
        // ...
        return model;
    }
}
```

**Testing**:
```csharp
// Areas/FI/Tests/ControlPresupuestosServiceTests.cs

[TestClass]
public class PresupuestoServiceTests
{
    private IPresupuestoService _service;
    private Mock<FIControlPresupuestosAdapter> _mockAdapter;
    
    [TestInitialize]
    public void Setup()
    {
        _mockAdapter = new Mock<FIControlPresupuestosAdapter>();
        var logger = new Mock<ILogger<PresupuestoService>>();
        _service = new PresupuestoService(_mockAdapter.Object, logger.Object);
    }
    
    [TestMethod]
    public void ObtenerPresupuestos_DebeRetornarLista()
    {
        // Arrange
        var expected = new List<PresupuestoDTO>
        {
            new PresupuestoDTO { Id = 1, MontoPresupuesto = 1000 }
        };
        _mockAdapter.Setup(x => x.ObtenerPresupuestos(It.IsAny<string>(), 
            It.IsAny<string>(), It.IsAny<string>()))
            .Returns(expected);
        
        // Act
        var (items, total, filtered) = _service.ObtenerPresupuestos("202401", "", "", 0, 10);
        
        // Assert
        Assert.AreEqual(1, items.Count);
        Assert.AreEqual(1000, items[0].MontoPresupuesto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void GuardarPresupuesto_MontoNegativo_DebeLanzarException()
    {
        // Arrange
        var model = new PresupuestoDTO { MontoPresupuesto = -100 };
        
        // Act
        _service.GuardarPresupuesto(model);
    }
}
```

## 1.6 Dependencias

### Servicios consumidos
- `CCFinzOpeService` (Adapter CC)
- `ILogger<T>` (Microsoft.Extensions.Logging)

### SP requeridas
- `CC_DetallePresupuesto` (lectura)
- `CC_GuardarPresupuesto` (creación/actualización)
- `CC_ObtenerPresupuestos` (lectura)
- `CC_LiquidarPlanillas` (para cálculo realizado)

## 1.7 Checklist de Entrega Sprint 1

- [ ] Compilación sin errores
- [ ] 0 warnings críticos
- [ ] ControlPresupuestosController responde GET/POST
- [ ] Index.cshtml muestra grid
- [ ] Filtros funcionan (período, trabajo, estado)
- [ ] Modal nuevo/editar abre y guarda
- [ ] Export a Excel funciona
- [ ] Service.ObtenerPresupuestos retorna datos
- [ ] Service.GuardarPresupuesto valida montos
- [ ] Adapter consulta CC_FinzOpe correctamente
- [ ] Tests unitarios pasan (80%+ cobertura)
- [ ] Documentación actualizada

## 1.8 Timeline Detallado

```
Semana 3 (40 horas)
├─ Día 1 (8h): Tarea 1.1 - Models + DTOs
├─ Día 2-3 (16h): Tarea 1.2 - Controller + Views (Index.cshtml + JS)
├─ Día 4 (8h): Tarea 1.3 - Service (ObtenerPresupuestos)
└─ Día 5 (8h): Tarea 1.4 - Adapter + Testing (inicio)

Semana 4 (52 horas)
├─ Día 1 (8h): Tarea 1.4 - Adapter + Testing (final)
├─ Día 2-3 (16h): Tarea 1.2 - Views secundarias (Detalles.cshtml, etc.)
├─ Día 3-4 (16h): Tarea 1.3 - Service (Guardar, Eliminar, Export)
└─ Día 5 (12h): Testing, validación, documentación

TOTAL: 92 horas
```

---

# SPRINT 2: Grupo 2 - Presupuestos Internos

**Duración**: 1.5 semanas (68 horas)  
**Semanas**: 5  
**Páginas**: 4  
**Patrón principal**: Grid modal CRUD  
**Complejidad**: 🟠 Media

## 2.1 Resumen

**Funcionalidad**: Gestión de presupuestos internos de empresas/divisiones

**Páginas**:
1. PresupuestosInternosListado.aspx - Listado y CRUD
2. PresupuestosInternoDetalles.aspx - Detalles de presupuesto interno
3. DetallesPresupuestosInterno.aspx - Líneas de presupuesto
4. HistoricoPresupuestosInterno.aspx - Auditoría de cambios

**Usuarios**: Administradores, gerentes administrativos

## 2.2 Análisis de Páginas

| Página | CRUD | Complejidad | SP |
|--------|------|-------------|-----|
| PresupuestosInternosListado | CRUD | Baja | CC_PresupuestosInternosGet, CC_GuardarPresupuestoInterno |
| PresupuestosInternoDetalles | R + U | Baja | CC_PresupuestosInternoDetalle |
| DetallesPresupuestosInterno | CRUD | Baja | CC_DetallesPresupuestosInterno |
| HistoricoPresupuestosInterno | R | Muy baja | CC_HistoricoPresupuestosInterno |

## 2.3 Estructura y Tareas

Similar a Sprint 1, pero con menor complejidad.

**Tarea 2.1**: Models + DTOs (12 horas)
**Tarea 2.2**: Controller + Views (16 horas)
**Tarea 2.3**: Service (16 horas)
**Tarea 2.4**: Adapter + Testing (12 horas)

**Patrón compartido**: Grid + filtros (período, estado)

**SP críticos**: `CC_PresupuestosInternosGet`, `CC_GuardarPresupuestoInterno`

---

# SPRINT 3: Grupo 3 - Procesos Internos

**Duración**: 2.5 semanas (132 horas)  
**Semanas**: 6-7  
**Páginas**: 6  
**Patrón principal**: Grid + filtros, modales, auditoría  
**Complejidad**: 🔴 Alta (procesos complejos)

## 3.1 Resumen

**Funcionalidad**: Gestión de procesos internos, conteos, requerimientos

**Páginas**:
1. ConteoTrabajos.aspx - Registrar conteos de trabajo
2. ReporteConteoTrabajos.aspx - Reporte de conteos
3. RequerimientosEquipo.aspx - Generar requerimientos
4. ResumenProductividad.aspx - Resumen de productividad
5. ConsolidacionProduccion.aspx - Consolidar producción
6. CalculoJornadaLaboral.aspx - Cálculos de jornada

## 3.2 Análisis de Páginas

| Página | CRUD | Complejidad | SP Críticos |
|--------|------|-------------|-------------|
| ConteoTrabajos | CRU | Alta | CC_Conteos*, CC_ActividadesXTrabajo, CC_ConteosXIdGet |
| ReporteConteoTrabajos | R | Media | CC_ReporteConteoTrabajos |
| RequerimientosEquipo | CRUD | Alta | CC_GenerarRequerimientos, CC_MuestraGenerarRequerimiento |
| ResumenProductividad | R | Media | CC_ResumenesdeProduccion |
| ConsolidacionProduccion | U | Alta | CC_Produccion*, CC_ConsolidacionProduccion |
| CalculoJornadaLaboral | R + U | Alta | CC_CalculoJornada*, TH_Ausencia.CalculoDias |

**Detalles**:

### ConteoTrabajos.aspx
- **Función**: Registrar cantidad de items procesados
- **Entrada**: Trabajo, actividad, cantidad, categoría
- **Validación**: 
  - Cantidad debe ser > 0
  - Validar si trabajo está activo
  - Verificar permisos por trabajo
- **SP**: `CC_Conteos.RegistrosConteo` (insert), `CC_ActividadesXTrabajo` (lectura)

### RequerimientosEquipo.aspx
- **Función**: Generar requerimientos de equipos/materiales
- **Proceso**: 
  - Obtener producción
  - Calcular necesidad de equipos
  - Generar requerimiento
- **SP**: `CC_GenerarRequerimientos`, `CC_MuestraGenerarRequerimiento`

### CalculoJornadaLaboral.aspx
- **Función**: Calcular horas trabajadas, descuentos por ausencias
- **Lógica**:
  - Obtener jornada base
  - Restar ausencias (usar TH_Ausencia)
  - Calcular extras si aplica
  - Guardar cálculo
- **SP**: `CC_CalculoJornada`, `TH_Ausencia.CalculoDias` (externa de TH)
- **Complejidad**: **🔴 CRÍTICA** - depende de cálculos correctos de ausencias

## 3.3 Estructura y Tareas

**Tarea 3.1**: Mapeo de SP externas (TH_Ausencia) (8 horas)
**Tarea 3.2**: Models + DTOs (16 horas)
**Tarea 3.3**: Controller + Views (28 horas)
**Tarea 3.4**: Service layer (32 horas)
**Tarea 3.5**: Adapter + Testing (20 horas)
**Tarea 3.6**: Validación cruzada (28 horas)

---

# SPRINT 4: Grupo 4 - Reportes

**Duración**: 1.5 semanas (72 horas)  
**Semanas**: 8  
**Páginas**: 4  
**Patrón principal**: Grid read-only + export  
**Complejidad**: 🟠 Media (queries complejas, permisos sensibles)

## 4.1 Resumen

**Funcionalidad**: Reportes financieros y operacionales

**Páginas**:
1. ReportePagos.aspx - Reporte de pagos procesados
2. ReporteActividadesProduccion.aspx - Detalles de actividades
3. ReporteContabilizacionPST.aspx - Contabilización de PST
4. ReporteVarianzasPresupuestarias.aspx - Análisis de varianzas

## 4.2 Análisis de Páginas

| Página | CRUD | Complejidad | SP |
|--------|------|-------------|-----|
| ReportePagos | R | Media | CC_ReportePagos, CC_LiquidarPlanillas |
| ReporteActividadesProduccion | R | Media | CC_ReporteActividadesProduccion |
| ReporteContabilizacionPST | R | Media | CC_ReporteContabilizacionPST, CC_ReportePagos |
| ReporteVarianzasPresupuestarias | R | Alta | CC_ResumenesdeProduccion, CC_ReportePagos |

**Notas**:
- ✅ Read-only (no CRUD)
- ✅ Requiere permisos por trabajo/período
- ✅ Filtros complejos (fecha, trabajo, empleado, estado)
- ✅ Export a Excel obligatorio
- 🔴 Sensibles: Datos de pagos, auditoría

## 4.3 Patrones

**Patrón**: Grid filtrado + Export + Charts (opcional)

```html
<div class="card">
    <div class="card-header">
        <h3>Reporte de Pagos</h3>
        <div class="filters">
            <!-- Filtros complejos -->
        </div>
    </div>
    <div class="card-body">
        <table id="grid-reportes"><!-- datos --></table>
        <button id="btn-exportar">Exportar Excel</button>
    </div>
</div>
```

---

# SPRINT 5: Grupo 5 - Producción

**Duración**: 4 semanas (232 horas) **← SPRINT MÁS CRÍTICO**  
**Semanas**: 9-12  
**Páginas**: 9  
**Patrón principal**: Grid CRUD completo, workflows complejos  
**Complejidad**: 🔴 MUY ALTA (liquidaciones, bonificaciones, contabilidad)

## 5.1 Resumen

**Funcionalidad**: Gestión de producción, liquidación, bonificaciones

**Páginas**:
1. RegistroProduccion.aspx - Registrar producción
2. LiquidarPlanillasActividades.aspx - Liquidar planillas
3. GenerarBonificacion.aspx - Generar bonificación
4. CargueDescuentosSS.aspx - Cargar descuentos seguridad social
5. LiquidarProductividadPST.aspx - Liquidar PST por productividad
6. AsignacionCostosPST.aspx - Asignar costos a PST
7. EstadoJobBooks.aspx - Cambiar estado de jobbooks
8. RevisarGeneracionBonificacion.aspx - Auditoría bonificaciones
9. AnulacionLiquidaciones.aspx - Reversar liquidaciones

## 5.2 Análisis Crítico

| Página | CRUD | Complejidad | SP CRÍTICOS | Estado Requerido |
|--------|------|-------------|-------------|------------------|
| RegistroProduccion | CRUD | Alta | CC_Produccion.RegistrosProduccion | Mitad hábil |
| LiquidarPlanillasActividades | U + Create | MUY ALTA | CC_LiquidarPlanillas | Fin mes |
| GenerarBonificacion | Create | MUY ALTA | CC_GenerarBonificacion | Fin período |
| CargueDescuentosSS | U | Media | CC_CargueDescuentosSS | Post-liquidación |
| LiquidarProductividadPST | Create | MUY ALTA | CC_LiquidarProductividadPST | Fin período |
| AsignacionCostosPST | U | Media | CC_AsignacionCostosPST | Post-PST |
| EstadoJobBooks | U | Baja | CC_EstadoJobBooks | Auditoría |
| RevisarGeneracionBonificacion | R | Media | SP_ReporteBonificacion | Auditoría |
| AnulacionLiquidaciones | U + Create | Alta | CC_RevertirLiquidacion | Auditoría |

### ⚠️ RIESGOS CRÍTICOS SPRINT 5:

1. **🔴 RIESGO 1: Liquidación incorrecta**
   - Impacto: Empleados cobran mal
   - Mitigación: Testing exhaustivo de CC_LiquidarPlanillas antes de Sprint 5
   - Validación: Reconciliación con nómina original

2. **🔴 RIESGO 2: Pérdida de data**
   - Impacto: Datos históricos destruidos
   - Mitigación: Backup pre-migración, migration en DB staging primero
   - Rollback: Script de reversión preparado

3. **🔴 RIESGO 3: Performance bajo carga**
   - Impacto: Timeouts en liquidación mensual
   - Mitigación: Optimizar índices de CC_Produccion, CC_PrestacionServicios
   - Testing: Load test con 100k+ registros

4. **🟠 RIESGO 4: Integridad de SP**
   - Impacto: Cálculos incorrectos
   - Mitigación: Validar lógica de negocio con Nómina
   - Testing: Casos reales de empleados del sistema actual

## 5.3 Estructura y Tareas

**Tarea 5.1**: Validación de SP críticos (20 horas)
- Verificar CC_LiquidarPlanillas con datos reales
- Verificar CC_GenerarBonificacion
- Documentar casos de borde

**Tarea 5.2**: Models + DTOs (20 horas)

**Tarea 5.3**: Controllers + Views (48 horas)
- RegistroProduccion: Grid CRUD completo
- Liquidaciones: Wizards de varias páginas
- Auditoría: Grillas de historial

**Tarea 5.4**: Service layer (64 horas)
- Lógica de liquidación
- Validaciones de negocio
- Workflows complejos

**Tarea 5.5**: Adapter + Testing (64 horas)
- Dapper queries por SP
- Testing: Mínimo 90% cobertura
- Performance testing

**Tarea 5.6**: Validación + UAT (16 horas)
- Reconciliación con datos históricos
- UAT con equipo de Nómina
- Documentación

---

# SPRINT 6: Grupo 6 - Inventario

**Duración**: 1 semana (16 horas)  
**Semanas**: 13  
**Páginas**: 1  
**Patrón principal**: Grid CRUD simple  
**Complejidad**: 🟡 Baja

## 6.1 Resumen

**Funcionalidad**: Gestión básica de inventario

**Página**:
1. InventarioProductos.aspx - CRUD de productos/SKU

---

## 6.2 Timeline Global (Sprints 1-6)

```
SEMANA 1-2: Sprint Pre-1 (CC_FinzOpe) [80 horas]
SEMANA 3-4: Sprint 1 (Control Presupuestos) [92 horas]
SEMANA 5: Sprint 2 (Presupuestos Internos) [68 horas]
SEMANA 6-7: Sprint 3 (Procesos Internos) [132 horas]
SEMANA 8: Sprint 4 (Reportes) [72 horas]
SEMANA 9-12: Sprint 5 (Producción) [232 horas] ← CRÍTICO
SEMANA 13: Sprint 6 (Inventario) [16 horas]

TOTAL: 13 semanas, 784 horas (@ 1 dev, 80h/sem) o 8-9 semanas (@ 2 devs)
```

---

**Documento**: PLAN_SPRINTS_1_6_FI.md  
**Versión**: 1.0  
**Estado**: 📋 Documentación base completada

