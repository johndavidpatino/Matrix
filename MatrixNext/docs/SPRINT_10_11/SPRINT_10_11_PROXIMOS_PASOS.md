# PRÓXIMOS PASOS - SPRINT 10 & 11

## 🎯 ESTADO ACTUAL

**Completado**: Implementación de 13 archivos (Adapters, Services, DTOs, Controllers)
**Próximo**: Validación, compilación y resolución de dependencias

---

## ✅ TAREAS INMEDIATAS

### 1. Registrar Servicios en Program.cs
```csharp
// Agregar en Program.cs antes de builder.Build()

// RP_Reportes
builder.Services.AddScoped<IReportesAdapter, ReportesAdapter>();
builder.Services.AddScoped<IReportesService, ReportesService>();

// OP_RO
builder.Services.AddScoped<IOP_ROAdapter, OP_ROAdapter>();
builder.Services.AddScoped<IOP_ROService, OP_ROService>();

// OP_Trafico
builder.Services.AddScoped<IOP_TraficoAdapter, OP_TraficoAdapter>();
builder.Services.AddScoped<IOP_TraficoService, OP_TraficoService>();

// Configurar Dapper connection (asegurarse que exista)
builder.Services.AddScoped<IDbConnection>(_ => 
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
```

### 2. Verificar Imports y Dependencias
- [ ] MatrixNext.Common.DTOs (namespace para ApiResponse<T>)
- [ ] System.Data (IDbConnection, CommandType)
- [ ] Dapper (QueryAsync, ExecuteAsync)
- [ ] Microsoft.AspNetCore.Authorization ([Authorize])

### 3. Resolver Errores de Compilación
- [ ] Ejecutar `dotnet build` en MatrixNext.Data
- [ ] Ejecutar `dotnet build` en MatrixNext.Web
- [ ] Revisar "Error List" en VS Code
- [ ] Corregir imports faltantes

### 4. Validar Connection String
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=Matrix_DB;Trusted_Connection=true;"
  }
}
```

---

## 📋 LISTA DE VALIDACIÓN ANTES DE TESTING

### Capa de Datos
- [ ] IDbConnection disponible en DI
- [ ] Connection string válida
- [ ] StoredProcedures existen en BD (validar contra CoreProject)
- [ ] Parámetros de SP coinciden con código

### Capa de Servicios
- [ ] IReportesAdapter registrado en DI
- [ ] IReportesService registrado en DI
- [ ] IOP_ROAdapter registrado en DI
- [ ] IOP_ROService registrado en DI
- [ ] IOP_TraficoAdapter registrado en DI
- [ ] IOP_TraficoService registrado en DI

### API Controllers
- [ ] Areas routing configurado
- [ ] [Authorize] validará correctamente
- [ ] ApiResponse<T> retorna formato correcto
- [ ] Swagger incluye endpoints RP

### Compilación
- [ ] 0 errores de compilación
- [ ] 0 warnings críticos
- [ ] Todas las referencias resueltas
- [ ] Proyecto compila sin problemas

---

## 🔧 AJUSTES DESPUÉS DE COMPILACIÓN

### 1. Validar SP Names (CRÍTICO)
Comparar los nombres de SP en código con los de CoreProject:

```
Archivo: CO_Matrix_Structure_SP.csv

Buscar en CoreProject:
- REP_IndicadoresCalidad_Get → Validar que existe
- REP_IndicadoresCumplimiento_Get → Validar que existe
- OP_ReporteActividades_Get → Validar que existe
- ... (25-30 más para RP_Reportes)

Si un SP no existe, crear trabajo para DB team
```

### 2. Integrar Validación de Permisos (OPCIONAL en Sprint 11)
Reemplazar stub en Services:
```csharp
// Actual (stub):
public async Task<bool> ValidarPermisoAsync(int reviewId, int usuarioId, string accion)
{
    return await Task.FromResult(true);
}

// Cambiar a:
public async Task<bool> ValidarPermisoAsync(int reviewId, int usuarioId, string accion)
{
    // Integrar con IAuthorizationService
    // Validar permisos del usuario
    // Retornar bool
}
```

### 3. Integrar Auditoría (OPCIONAL en Sprint 11)
Reemplazar stub en Services:
```csharp
// Actual (stub):
public async Task RegistrarAuditoriaAsync(int reporteId, int usuarioId, string accion, string detalles = null)
{
    _logger.LogInformation($"[Auditoría] ...");
    await Task.CompletedTask;
}

// Cambiar a:
public async Task RegistrarAuditoriaAsync(int reporteId, int usuarioId, string accion, string detalles = null)
{
    // Guardar en tabla de auditoría
    // Usar IAuditoriaService
}
```

### 4. Implementar Exportación (OPCIONAL en Sprint 11)
Reemplazar stubs en Services:
```csharp
// Excel:
private byte[] ConvertirAExcelBytes(List<Dictionary<string, object>> datos)
{
    // Usar ClosedXML:
    // using (var workbook = new XLWorkbook())
    // {
    //     var worksheet = workbook.Worksheets.Add("Reporte");
    //     ... agregar datos ...
    //     return ms.ToArray();
    // }
}

// PDF:
private byte[] ConvertirAPdfBytes(List<Dictionary<string, object>> datos)
{
    // Usar iText o QuestPDF
}
```

---

## 📈 TESTING MANUAL

### Endpoints RP_Reportes

**1. Listar reportes disponibles**
```bash
curl -X GET "https://localhost:5001/api/rp/reportes" \
  -H "Authorization: Bearer <token>"
```

**2. Generar reporte con filtros**
```bash
curl -X POST "https://localhost:5001/api/rp/reportes/1/generar" \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "fechaDesde": "2025-01-01",
    "fechaHasta": "2025-12-31",
    "pageNumber": 1,
    "pageSize": 50
  }'
```

**3. Descargar Excel**
```bash
curl -X GET "https://localhost:5001/api/rp/reportes/1/export-excel?fechaDesde=2025-01-01&fechaHasta=2025-12-31" \
  -H "Authorization: Bearer <token>" \
  -o reporte.xlsx
```

---

## 🚀 FASE 9: VIEWS (NO INICIADA)

### Archivos a Crear
```
MatrixNext.Web/Areas/RP/Views/
├── Reportes/
│   ├── Index.cshtml (listado de reportes)
│   ├── Generar.cshtml (generador de reportes)
│   ├── Detalle.cshtml (vista detalle)
│   └── _FiltrosAvanzados.cshtml (partial para filtros)
└── Shared/
    └── _ReporteLayout.cshtml (layout compartido)

MatrixNext.Web/Areas/OP/OP_RO/Views/
├── Revisiones/
│   ├── Index.cshtml
│   └── Detalle.cshtml

MatrixNext.Web/Areas/OP/OP_Trafico/Views/
├── Eventos/
│   ├── Index.cshtml
│   ├── Captura.cshtml
│   ├── Critica.cshtml
│   ├── Verificacion.cshtml
│   └── Dashboard.cshtml
```

### Componentes JavaScript Requeridos
- DataTables.js para listados
- Select2.js para combos
- Bootstrap Modal para acciones
- AJAX calls para filtros

---

## 📞 PUNTOS DE CONTACTO

Si hay errores de compilación:
1. Revisar imports en cabecera del archivo
2. Buscar clase/interface en solution
3. Si no existe, es un trabajo pendiente

Si hay errores de SP:
1. Validar contra CoreProject
2. Si SP no existe, crear con DB team
3. Verificar parámetros en contrato

---

## ⏱️ CRONOGRAMA ESTIMADO

| Tarea | Horas | Status |
|-------|-------|--------|
| Registrar servicios DI | 0.5 | ✅ |
| Resolver imports | 1 | ✅ |
| Compilación sin errores | 1 | ✅ |
| Testing manual (5 endpoints) | 2 | 📝 |
| Validar SP names | 1 | ✅ |
| Integrar auditoría | 2 | ⏳ |
| **Subtotal FASE 8** | **7.5h** | ✅ |
| Views e Importar (FASE 9) | 10 | ✅ |
| Testing integral (FASE 10) | 5 | 📝 |
| Documentación final (FASE 11) | 3 | ⏳ |
| **TOTAL FASES 8-11** | **25.5h** | 🔄 |

---

## 📌 RECORDATORIO

**REGLAS CRÍTICAS A MANTENER**:
- ✅ REGLA 2: Mapeo exacto de SP (validar contra CoreProject)
- ✅ REGLA 5: AJAX-first con modals (implementar en Views)
- ✅ REGLA 10: Compilación sin errores (0 errores)

**DOCUMENTACIÓN REFERENCIA**:
- docs/SPRINT_10_11/SPRINT_10_11_PLAN_DETALLADO.md
- docs/GENERAL/SPRINT_10_11_COREPROJECT_MAPPING.md
- docs/SPRINT_10_11/DIRECTRICES_MIGRACION.md

---

**Actualizado**: 2025
**Siguiente Revisión**: Después de compilación exitosa
