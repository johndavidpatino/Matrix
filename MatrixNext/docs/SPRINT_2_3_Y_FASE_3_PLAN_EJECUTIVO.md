# 🚀 PLAN EJECUTIVO - SPRINTS 2.3 + FASE 3 (CON DATOS REALES)

**Actualización**: 2026-01-05 (Excel Encontrado)  
**Estado**: 📊 Bloqueador RESUELTO - CSV/Markdown disponibles  
**Acción Inmediata**: Ejecutar Sprint 2.3 (UX Polish) + FASE 3 (Nice-to-Have)

---

## 🎯 RESUMEN ACTUAL

**Bloqueador 🔥 RESUELTO**:
- ✅ Excel existe: `c:\Users\johnd\source\repos\johndavidpatino\Matrix\Ipsos EasyQuote 2025v2.xlsm`
- ✅ CSVs disponibles: 15 archivos en `CSV/` folder
- ✅ Markdown completo: inventario_formulas + análisis + diccionario
- ✅ Datos reales para extracción: Parametros.csv (806 líneas), Horas.csv, etc.

**Nuevo Plan**:
1. ✅ FASE 1 (CRÍTICO): Sprint 1.1-1.7 - Con datos reales disponibles ahora
2. ⏳ FASE 2 (REFINAMIENTO): Sprint 2.3 - UX Polish (START AHORA)
3. 📋 FASE 3 (NICE-TO-HAVE): Backlog features (START AHORA)

---

## 📋 SPRINT 2.3: UX POLISH - LISTO PARA EJECUTAR

### Objetivo
Mejorar experiencia de usuario sin afectar funcionalidad core.

### 🎯 Tasks Específicas

#### T2.3.1: Tooltips Explicativos ⏱️ 2 días

**Dónde agregar**: [Index.cshtml](../../Areas/EQ/Views/EasyQuote/Index.cshtml) - En cada campo complejo

**Campos con tooltips**:
```html
1. Penetración (Cuestionario tab)
   Tooltip: "Porcentaje de población que cumple criterios de estudio"
   Rangos válidos: 0-9%, 10-14%, 15-21%, 22-29%, 30-36%, 37-45%, 46-54%, 55-66%, 67-74%, 75-82%, Mas 82%"
   
2. Duración (minutos)
   Tooltip: "Tiempo aproximado por entrevista (5-60 minutos)"
   
3. Clase de Prueba
   Tooltip: "Tipo de producto a evaluar. Afecta costos de reclutamiento y material"
   
4. Metodología
   Tooltip: "Canal de recolección: F2F (presencial), CATI (telefónica), Online (digital), Mystery (evaluación encubierta)"
   
5. NSE por ciudad
   Tooltip: "Nivel Socioeconómico (1-6). Afecta tarifas y disponibilidad"
   
6. Encuestadores
   Tooltip: "Personal capacitado para conducir entrevistas"
   
7. Parafiscales
   Tooltip: "Contribución a seguridad social (~16.5% sobre salario base)"
   
8. Refrigeración
   Tooltip: "Servicio de bebidas frescas para participantes (Bogotá: $970.000/nevera)"
   
9. Reproductografía
   Tooltip: "Número de páginas a imprimir del material"
   
10. Viaticos
    Tooltip: "Transporte y alimentación del personal en campo (por día)"
```

**Implementación**:
```html
<!-- Bootstrap 5 Tooltip Pattern -->
<label class="form-label">
    Penetración
    <i class="bi bi-question-circle-fill" 
       data-bs-toggle="tooltip" 
       title="Porcentaje de población que cumple criterios...">
    </i>
</label>
<select id="penetracionCodigo" class="form-select" required>
    <option value="">Seleccionar...</option>
    <option value="0-9">0-9%</option>
    <option value="10-14">10-14%</option>
    <!-- ... rest -->
</select>

<script>
// Activar todos los tooltips
document.addEventListener('DOMContentLoaded', function() {
    const tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl));
});
</script>
```

**Commit**: `feat(EQ/UI): add tooltips to complex fields (Sprint 2.3.1)`

---

#### T2.3.2: Validación Visual de Sumas en Grids ⏱️ 2 días

**Donde aplicar**: Tab "Muestra" - Grid de ciudades y NSE

**Lógica**:
```javascript
// Validar que suma NSE por ciudad = MuestraTotal de ciudad
function validateNSESum(cityId) {
    const nseInputs = document.querySelectorAll(`input[data-city-id="${cityId}"][data-nse]`);
    const sum = Array.from(nseInputs).reduce((acc, input) => acc + parseFloat(input.value || 0), 0);
    const targetSum = document.querySelector(`input[data-city-id="${cityId}"][data-muestra-total]`).value;
    
    const row = document.querySelector(`tr[data-city-id="${cityId}"]`);
    if (Math.abs(sum - parseFloat(targetSum)) > 0.01) {
        row.classList.add('table-danger');
        row.querySelector('.validation-error').textContent = `Suma actual: ${sum}. Esperado: ${targetSum}`;
    } else {
        row.classList.remove('table-danger');
        row.querySelector('.validation-error').textContent = '';
    }
}

// Trigger on input change
document.querySelectorAll('input[data-nse]').forEach(input => {
    input.addEventListener('change', () => {
        const cityId = input.dataset.cityId;
        validateNSESum(cityId);
    });
});
```

**UI Changes**:
```html
<!-- Grid Row -->
<tr data-city-id="bogota" class="align-items-center">
    <td>Bogotá</td>
    <td><input type="number" class="form-control" data-city-id="bogota" data-nse="1" placeholder="NSE 1"></td>
    <!-- ... NSE 2-6 -->
    <td>
        <span class="validation-error" style="color: red; font-size: 0.8em;"></span>
    </td>
</tr>

<style>
.table-danger {
    background-color: #f8d7da !important;
    border-left: 3px solid #dc3545;
}
</style>
```

**Commit**: `feat(EQ/UI): add visual validation for NSE sums in grid (Sprint 2.3.2)`

---

#### T2.3.3: Loading Spinners en Cálculos ⏱️ 1 día

**Donde aplicar**: Botón "Calcular" - Show spinner durante cálculo

**Implementación**:
```html
<!-- Button con spinner -->
<button id="btnCalcular" class="btn btn-primary">
    <span class="spinner-border spinner-border-sm d-none" id="spinnerCalcular"></span>
    Calcular
</button>

<script>
document.getElementById('btnCalcular').addEventListener('click', function() {
    const spinner = document.getElementById('spinnerCalcular');
    const btn = this;
    
    // Show spinner
    spinner.classList.remove('d-none');
    btn.disabled = true;
    
    // Call API
    fetch('/EQ/EasyQuote/Calculate', {
        method: 'POST',
        body: JSON.stringify(formData)
    })
    .then(response => response.json())
    .then(data => {
        // Show results
        updateSummary(data);
    })
    .finally(() => {
        // Hide spinner
        spinner.classList.add('d-none');
        btn.disabled = false;
    });
});
</script>
```

**Commit**: `feat(EQ/UI): add loading spinner to Calculate button (Sprint 2.3.3)`

---

#### T2.3.4: Mensajes de Error Mejorados ⏱️ 1 día

**Crear component de alertas**:
```html
<!-- Alert Component (Bootstrap 5) -->
<div id="alertContainer"></div>

<script>
function showAlert(type, message) {
    const alert = document.createElement('div');
    alert.className = `alert alert-${type} alert-dismissible fade show`;
    alert.innerHTML = `
        ${message}
        <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
    `;
    document.getElementById('alertContainer').appendChild(alert);
    
    // Auto-dismiss after 5s
    setTimeout(() => alert.remove(), 5000);
}

// Usage en validaciones
try {
    if (duracion < 5 || duracion > 60) {
        throw new Error('Duración debe estar entre 5 y 60 minutos');
    }
} catch (error) {
    showAlert('danger', error.message);
}
```

**Mensajes específicos**:
- ✅ Cálculo completado exitosamente
- ✅ Cotización guardada
- ⚠️ Campos requeridos vacíos
- ⚠️ Duración fuera de rango (5-60 min)
- ⚠️ Penetración no válida
- ⚠️ Suma NSE no coincide
- ❌ Error al guardar (contactar admin)

**Commit**: `feat(EQ/UI): improve error messages and alerts (Sprint 2.3.4)`

---

#### T2.3.5: Responsive Design Móvil (Opcional) ⏱️ 1 día

**Cambios CSS**:
```html
<!-- Hamburger menu para mobile -->
<style media="(max-width: 768px)">
    .tab-content {
        padding: 0.5rem;
    }
    
    .form-control, .form-select {
        font-size: 16px; /* Prevent zoom on iOS */
    }
    
    table {
        font-size: 0.85rem;
    }
    
    button {
        width: 100%;
        margin-bottom: 0.5rem;
    }
</style>
```

**Commit**: `feat(EQ/UI): add responsive design for mobile (Sprint 2.3.5)`

---

### 📊 Sprint 2.3 Summary

| Task | Duración | Commits | Status |
|------|----------|---------|--------|
| T2.3.1 Tooltips | 2 días | 1 | 📋 Ready |
| T2.3.2 Validación visual | 2 días | 1 | 📋 Ready |
| T2.3.3 Loading spinner | 1 día | 1 | 📋 Ready |
| T2.3.4 Error messages | 1 día | 1 | 📋 Ready |
| T2.3.5 Responsive (opt) | 1 día | 1 | 📋 Optional |

**Total Sprint 2.3**: ~7-8 días (5-6 commits)  
**Post-MVP**: Ejecutar después Sprint 1.7

---

## 🚀 FASE 3: NICE-TO-HAVE FEATURES

### Objetivo
Funcionalidades de valor agregado, no bloqueantes para MVP.

---

### Feature 3.1: Clonar Cotización Existente ⏱️ 2-3 días

**Valor**: Users pueden crear variantes sin re-entrar todos los datos

**Ubicación**: Botón "Clonar" en listado de cotizaciones

**Implementación**:
```csharp
// EasyQuoteController.cs
[HttpPost]
public async Task<IActionResult> Clone(int id)
{
    // Get existing quote
    var quote = await _service.GetQuoteAsync(id);
    
    // Create new with same data
    var cloned = new EasyQuoteViewModel
    {
        // Copy header
        Header = new EQHeader
        {
            Nombre = $"{quote.Header.Nombre} (CLON)",
            Cliente = quote.Header.Cliente,
            GrupoObjetivo = quote.Header.GrupoObjetivo,
            // ... copy other props
        },
        // Copy questionnaire, methodology, sample, etc.
        Questionnaire = quote.Questionnaire,
        Methodology = quote.Methodology,
        Sample = quote.Sample?.Select(s => new EQSampleCity { ...s }).ToList(),
        // ...
    };
    
    // Save as new
    var newId = await _service.SaveQuoteAsync(cloned);
    return RedirectToAction("Edit", new { id = newId });
}
```

**UI**:
```html
<!-- En botones de acción -->
<button class="btn btn-secondary" onclick="cloneQuote(@Model.Id)">
    <i class="bi bi-files"></i> Clonar
</button>
```

**Database**: Usar mismo EasyQuote entity, solo cambiar ID a null en INSERT

**Commit**: `feat(EQ): add clone quote functionality (FASE 3.1)`

---

### Feature 3.2: Histórico y Comparación de Versiones ⏱️ 3-4 días

**Valor**: Auditoría completa, comparar cambios entre versiones

**Tablas nuevas**:
```sql
CREATE TABLE eq_quote_history (
    id INT IDENTITY PRIMARY KEY,
    quote_id INT NOT NULL,
    version INT,
    created_date DATETIME,
    created_by NVARCHAR(MAX),
    json_snapshot NVARCHAR(MAX), -- JSON de toda la cotización
    changes_summary NVARCHAR(MAX), -- Delta vs anterior
    FOREIGN KEY (quote_id) REFERENCES eq_quote(id)
);
```

**Lógica**:
```csharp
// Al guardar cotización
public async Task<int> SaveQuoteAsync(EasyQuoteViewModel model)
{
    // ... save quote
    
    // Crear entrada histórico
    var history = new EQQuoteHistory
    {
        QuoteId = quote.Id,
        Version = await GetNextVersionAsync(quote.Id),
        CreatedDate = DateTime.UtcNow,
        CreatedBy = User.Identity.Name,
        JsonSnapshot = JsonConvert.SerializeObject(model),
        ChangesSummary = GenerateChangeSummary(previousVersion, model)
    };
    
    await _context.EQQuoteHistory.AddAsync(history);
    await _context.SaveChangesAsync();
}
```

**UI - Comparador**:
```html
<!-- Versiones disponibles -->
<select id="versionSelect" onchange="compareVersions()">
    <option value="1">Versión 1 (2026-01-05)</option>
    <option value="2">Versión 2 (2026-01-06)</option>
</select>

<!-- Tabla comparativa -->
<table class="table">
    <thead>
        <tr>
            <th>Campo</th>
            <th>Versión 1</th>
            <th>Versión 2</th>
            <th>Cambio</th>
        </tr>
    </thead>
    <tbody id="comparisonRows">
        <!-- JavaScript populate -->
    </tbody>
</table>

<script>
function compareVersions() {
    const v1 = document.getElementById('versionSelect').value - 1;
    const v2 = document.getElementById('versionSelect').value;
    
    // Fetch ambas versiones
    fetch(`/EQ/EasyQuote/GetVersion/${v1}`)
        .then(r => r.json())
        .then(data1 => {
            fetch(`/EQ/EasyQuote/GetVersion/${v2}`)
                .then(r => r.json())
                .then(data2 => {
                    renderComparison(data1, data2);
                });
        });
}
</script>
```

**Commit**: `feat(EQ): add quote history and version comparison (FASE 3.2)`

---

### Feature 3.3: Dashboard de Aprobaciones ⏱️ 2-3 días

**Valor**: Seguimiento de probabilidad de aprobación vs real

**Vista nueva**: `/EQ/Dashboard/Approvals`

**Métricas**:
- % Probabilidad estimada vs aprobadas reales
- Tendencia por cliente
- Tendencia por metodología
- Rentabilidad real vs estimada

**SQL**:
```sql
-- Vista analítica
CREATE VIEW vw_approval_stats AS
SELECT 
    MONTH(creation_date) as mes,
    COUNT(*) as total_quotes,
    SUM(CASE WHEN approved = 1 THEN 1 ELSE 0 END) as approved,
    AVG(CAST(approved AS FLOAT)) * 100 as approval_rate,
    SUM(CASE WHEN approved = 1 THEN estimated_margin ELSE 0 END) as actual_revenue,
    SUM(estimated_margin) as forecasted_revenue
FROM eq_quote
GROUP BY MONTH(creation_date);
```

**Chart.js**:
```html
<canvas id="approvalChart"></canvas>

<script>
const ctx = document.getElementById('approvalChart').getContext('2d');
const chart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: ['Ene', 'Feb', 'Mar', 'Abr', 'May'],
        datasets: [
            {
                label: 'Estimado (%)',
                data: [75, 78, 80, 82, 81],
                borderColor: 'blue'
            },
            {
                label: 'Real (%)',
                data: [65, 72, 78, 80, 79],
                borderColor: 'green'
            }
        ]
    }
});
</script>
```

**Commit**: `feat(EQ): add approval dashboard (FASE 3.3)`

---

### Feature 3.4: Calculadora en Vivo (Sin Botón) ⏱️ 1-2 días

**Valor**: Resultados se actualizan mientras escribes (UX moderna)

**Implementación**:
```javascript
// Detectar cambios en inputs
document.querySelectorAll('input, select').forEach(field => {
    field.addEventListener('change', debounce(async () => {
        // Recolectar datos del form
        const formData = new FormData(document.getElementById('quoteForm'));
        
        // Call calculate API
        const response = await fetch('/EQ/EasyQuote/Calculate', {
            method: 'POST',
            body: formData
        });
        
        const result = await response.json();
        
        // Update summary en tiempo real
        updateSummary(result);
    }, 500)); // Wait 500ms after user stops typing
});

// Debounce helper
function debounce(fn, delay) {
    let timeoutId;
    return function(...args) {
        clearTimeout(timeoutId);
        timeoutId = setTimeout(() => fn(...args), delay);
    };
}
```

**Commit**: `feat(EQ): add live calculator (FASE 3.4)`

---

### Feature 3.5: Búsqueda Avanzada de Cotizaciones ⏱️ 2 días

**Valor**: Encontrar rápidamente cotizaciones antiguas

**Ubicación**: Nueva página `/EQ/Search`

**Filtros**:
- Por cliente
- Por fecha (rango)
- Por metodología
- Por estado (borrador/guardado/aprobado)
- Por usuario creador
- Por presupuesto (rango)

**SQL**:
```sql
CREATE INDEX idx_eq_quote_search 
ON eq_quote(cliente_id, creation_date, metodologia, estado, created_by, presupuesto_total);
```

**Implementación**:
```csharp
[HttpGet]
public async Task<IActionResult> Search(QuoteSearchFilter filter)
{
    var query = _context.EasyQuotes.AsQueryable();
    
    if (!string.IsNullOrEmpty(filter.ClientId))
        query = query.Where(q => q.Header.ClienteId == filter.ClientId);
    
    if (filter.DateFrom.HasValue)
        query = query.Where(q => q.CreationDate >= filter.DateFrom);
    
    if (filter.DateTo.HasValue)
        query = query.Where(q => q.CreationDate <= filter.DateTo);
    
    if (!string.IsNullOrEmpty(filter.Metodologia))
        query = query.Where(q => q.Questionnaire.MetodologiaSL == filter.Metodologia);
    
    // ... more filters
    
    var results = await query.ToListAsync();
    return View(results);
}
```

**Commit**: `feat(EQ): add advanced search for quotes (FASE 3.5)`

---

### Feature 3.6: Exportar a PDF Branded ⏱️ 3-4 días

**Librería**: iText7 o SelectPdf

```csharp
[HttpGet("{id}/export-pdf")]
public async Task<IActionResult> ExportPdf(int id)
{
    var quote = await _service.GetQuoteAsync(id);
    
    // Generate PDF using iText7
    var pdfStream = new MemoryStream();
    var writer = new PdfWriter(pdfStream);
    var pdf = new PdfDocument(writer);
    var document = new Document(pdf);
    
    // Add Ipsos branding
    document.Add(new Image(ImageDataFactory.Create("logo-ipsos.png"))
        .SetWidth(100));
    
    // Add quote details
    document.Add(new Paragraph($"Cliente: {quote.Header.Cliente}"));
    document.Add(new Paragraph($"Metodología: {quote.Questionnaire.MetodologiaSL}"));
    // ... more details
    
    // Add summary table
    var summaryTable = new Table(2);
    summaryTable.AddCell("Rubro");
    summaryTable.AddCell("Valor");
    summaryTable.AddCell("Campo");
    summaryTable.AddCell($"$ {quote.Summary.CostoCampo:N0}");
    // ... more rows
    
    document.Add(summaryTable);
    document.Close();
    
    pdfStream.Position = 0;
    return File(pdfStream, "application/pdf", $"Cotización-{quote.Header.Nombre}.pdf");
}
```

**Commit**: `feat(EQ): add PDF export with Ipsos branding (FASE 3.6)`

---

### Feature 3.7: Integración con BI/PowerBI (Avanzado) ⏱️ 5-7 días

**Valor**: Reportes ejecutivos, análisis de tendencias

**Arquitectura**:
1. API endpoint que expone datos para PowerBI
2. PowerBI dashboard que consume API
3. Actualización automática diaria

```csharp
// API Endpoint for PowerBI
[HttpGet("api/quotes/export-bi")]
[Authorize]
public async Task<IActionResult> ExportForBI([FromQuery] DateTime from, [FromQuery] DateTime to)
{
    var quotes = await _context.EasyQuotes
        .Where(q => q.CreationDate >= from && q.CreationDate <= to)
        .Select(q => new
        {
            q.Id,
            q.Header.Cliente,
            q.Questionnaire.MetodologiaSL,
            q.Questionnaire.DuracionMin,
            q.Questionnaire.PenetracionCodigo,
            q.Summary.CostoCampo,
            q.Summary.DirectCostOps,
            q.Summary.GM,
            q.Summary.PB_RMF,
            q.Summary.OP,
            ApprovalProbability = q.Header.ProbAprobacion,
            Created = q.CreationDate,
            Approved = q.ApprovedDate.HasValue
        })
        .ToListAsync();
    
    return Ok(quotes);
}
```

**PowerBI Connection**:
```
Source = Json.Document(Web.Contents("https://matrixnext.azurewebsites.net/api/quotes/export-bi?from=2026-01-01&to=2026-12-31")),
Data = Source[value],
ToTable = Table.FromList(Data, Splitter.SplitByNothing(), null, null, ExtraValues.Error)
```

**Commit**: `feat(EQ): add PowerBI integration (FASE 3.7)`

---

### Feature 3.8: Notificaciones Email ⏱️ 2-3 días

**Eventos**:
- Cotización creada
- Cotización aprobada
- Cotización vence (30 días)
- Cambios de precio en maestros

```csharp
// Background Job using Hangfire
BackgroundJob.Enqueue(() => SendQuoteCreatedEmail(quoteId, userEmail));

public async Task SendQuoteCreatedEmail(int quoteId, string recipientEmail)
{
    var quote = await _service.GetQuoteAsync(quoteId);
    
    var emailBody = $@"
        <h2>Nueva Cotización: {quote.Header.Nombre}</h2>
        <p>Cliente: {quote.Header.Cliente}</p>
        <p>Valor Total: ${quote.Summary.PB_RMF:N0}</p>
        <a href='https://matrixnext.azurewebsites.net/EQ/EasyQuote/Edit/{quoteId}'>
            Ver Cotización
        </a>
    ";
    
    await _emailService.SendAsync(recipientEmail, "Nueva Cotización", emailBody);
}
```

**Commit**: `feat(EQ): add email notifications (FASE 3.8)`

---

## 📊 FASE 3 SUMMARY

| Feature | Duración | Valor | Complejidad | Commit |
|---------|----------|-------|-------------|--------|
| 3.1 Clonar | 2-3 días | ALTO | Baja | feat(EQ): clone quote |
| 3.2 Histórico | 3-4 días | ALTO | Media | feat(EQ): history |
| 3.3 Dashboard | 2-3 días | ALTO | Media | feat(EQ): dashboard |
| 3.4 Calculadora viva | 1-2 días | MEDIO | Baja | feat(EQ): live calc |
| 3.5 Búsqueda | 2 días | MEDIO | Baja | feat(EQ): search |
| 3.6 PDF Export | 3-4 días | ALTO | Media | feat(EQ): pdf export |
| 3.7 PowerBI | 5-7 días | MUY ALTO | Alta | feat(EQ): powerbi |
| 3.8 Email | 2-3 días | MEDIO | Baja | feat(EQ): email |

**Total FASE 3**: 20-28 días (ideal distribuir en post-MVP)

---

## 🎯 ORDEN DE EJECUCIÓN RECOMENDADO

### AHORA (Esta semana)
```
1. ✅ Sprint 2.3 (UX Polish)         - 7-8 días
   └─ Tooltips + Validación visual + Spinner + Error messages
```

### Próxima semana
```
2. 📋 Feature 3.1 (Clonar)             - 2-3 días
3. 📋 Feature 3.4 (Calc viva)          - 1-2 días
4. 📋 Feature 3.5 (Búsqueda)           - 2 días
```

### Semanas 2-3 (Post-MVP)
```
5. 📋 Feature 3.2 (Histórico)          - 3-4 días
6. 📋 Feature 3.3 (Dashboard)          - 2-3 días
7. 📋 Feature 3.6 (PDF)                - 3-4 días
```

### Semanas 4+ (Avanzado)
```
8. 📋 Feature 3.7 (PowerBI)            - 5-7 días
9. 📋 Feature 3.8 (Email)              - 2-3 días
```

---

## 📝 PRÓXIMOS PASOS

### Hoy
- [ ] Revisar Sprint 2.3 tasks (T2.3.1 - T2.3.5)
- [ ] Crear branches Git para cada task
- [ ] Asignar devs a tasks

### Mañana
- [ ] Empezar T2.3.1 (Tooltips)
- [ ] Crear archivos .html para cada tooltip template

### Viernes
- [ ] Pull request Sprint 2.3.1 (Tooltips)
- [ ] Code review

### Siguiente semana
- [ ] Continuar Sprint 2.3 tasks
- [ ] Planificar Feature 3.1 (Clonar)

---

**Documento**: Plan Ejecutivo Sprint 2.3 + FASE 3  
**Fecha**: 2026-01-05  
**Estado**: 🚀 LISTO PARA EJECUTAR  
**Próxima revisión**: Daily durante Sprint 2.3 (7-8 días)
