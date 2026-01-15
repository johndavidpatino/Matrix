# SPRINT 10 - REPORTES (RP) - KICKOFF

**Fecha de inicio**: 2026-01-15  
**Duración estimada**: 2 semanas (1-2 sem flexible)  
**Esfuerzo**: 60 horas  
**Status**: 🟡 EN CURSO

---

## 🎯 OBJETIVO GENERAL

Completar la migración del módulo **RP_Reportes** (Reportes y Consultas) desde WebMatrix a MatrixNext.Web con:

- ✅ ReportesController completamente funcional
- ✅ Vistas Razor para todos los reportes
- ✅ Integración de datos desde múltiples módulos (EQ, CORE, TH, OP)
- ✅ Exportación a Excel/PDF
- ✅ Filtros dinámicos y búsqueda
- ✅ Performance optimizado para grandes datasets
- ✅ Build sin errores, 0 warnings

**Prioridad**: 🔴 ALTA  
**Dependencias**: CORE (Sprint 7 ✅), EQ (Sprint 8 ✅), TH (Sprint 5 ✅), OP (Sprint 6 ✅)

---

## 📋 TAREAS PRINCIPALES

### Tarea 1: Análisis de Reportes en WebMatrix (4h)
**Objetivo**: Identificar todos los reportes disponibles en WebMatrix y sus características

**Checklist**:
- [ ] Revisar WebMatrix/RP_Reportes (si existe)
- [ ] Buscar Stored Procedures de reportes en base de datos
- [ ] Identificar tipos de reportes: operacionales, analíticos, financieros
- [ ] Documentar filtros y parámetros por reporte
- [ ] Mapear integraciones con otros módulos

**Evidencia**: `MatrixNext/docs/RP/ANALISIS_RP_REPORTES.md`

### Tarea 2: Extender ReportesController (12h)
**Objetivo**: Implementar todos los endpoints de reportes

**Estructura esperada**:
```csharp
public class ReportesController : Controller
{
    // Reportes operacionales
    public async Task<IActionResult> ReporteCotizaciones()
    public async Task<IActionResult> ReporteProyectos()
    public async Task<IActionResult> ReporteProduccion()
    public async Task<IActionResult> ReporteTareas()
    public async Task<IActionResult> ReporteAusencias()
    
    // Exportación
    public async Task<IActionResult> ExportarExcel(string reportType)
    public async Task<IActionResult> ExportarPdf(string reportType)
    
    // Filtros
    public async Task<JsonResult> ObtenerFiltros(string reportType)
    public async Task<JsonResult> AplicarFiltros(FilterDto filtros)
}
```

**Checklist**:
- [ ] GET `/RP/Reportes/Cotizaciones` - Reporte de cotizaciones
- [ ] GET `/RP/Reportes/Proyectos` - Reporte de proyectos
- [ ] GET `/RP/Reportes/Produccion` - Reporte de producción
- [ ] GET `/RP/Reportes/Tareas` - Reporte de tareas CORE
- [ ] GET `/RP/Reportes/Ausencias` - Reporte de ausencias TH
- [ ] POST `/RP/Reportes/Filtros` - Aplicar filtros dinámicos
- [ ] GET `/RP/Reportes/ExportarExcel` - Export a Excel
- [ ] GET `/RP/Reportes/ExportarPdf` - Export a PDF

### Tarea 3: Crear ReportesService (10h)
**Objetivo**: Implementar lógica de negocio para cada reporte

**Métodos esperados**:
```csharp
public interface IReportesService
{
    Task<ReporteCotizacionesDto> ObtenerReporteCotizacionesAsync(FilterDto filtros);
    Task<ReporteProyectosDto> ObtenerReporteProyectosAsync(FilterDto filtros);
    Task<ReporteProduccionDto> ObtenerReporteProduccionAsync(FilterDto filtros);
    Task<ReporteTareasDto> ObtenerReporteTareasAsync(FilterDto filtros);
    Task<ReporteAusenciasDto> ObtenerReporteAusenciasAsync(FilterDto filtros);
    
    Task<byte[]> ExportarExcelAsync(string reportType, object datos);
    Task<byte[]> ExportarPdfAsync(string reportType, object datos);
}
```

**Integración de datos**:
- [ ] EQ (EqQuoteHeaders) - Cotizaciones
- [ ] PY (Proyectos, Trabajos) - Proyectos
- [ ] CORE (Tareas, WorkFlows) - Tareas
- [ ] TH (SolicitudAusencia) - Ausencias
- [ ] OP (Muestras, Cuali/Cuanti) - Producción

**Checklist**:
- [ ] ObtenerReporteCotizacionesAsync implementado
- [ ] ObtenerReporteProyectosAsync implementado
- [ ] ObtenerReporteProduccionAsync implementado
- [ ] ObtenerReporteTareasAsync implementado
- [ ] ObtenerReporteAusenciasAsync implementado
- [ ] ExportarExcelAsync implementado (usando EPPlus)
- [ ] ExportarPdfAsync implementado (usando iTextSharp o similar)

### Tarea 4: Vistas Razor para Reportes (15h)
**Objetivo**: Crear vistas interactivas para visualizar y filtrar reportes

**Estructura esperada**:
```
MatrixNext/MatrixNext.Web/Areas/RP/Views/Reportes/
├── Index.cshtml (listado de reportes disponibles)
├── Cotizaciones.cshtml (reporte de cotizaciones)
├── Proyectos.cshtml (reporte de proyectos)
├── Produccion.cshtml (reporte de producción)
├── Tareas.cshtml (reporte de tareas)
├── Ausencias.cshtml (reporte de ausencias)
└── _Filtros.cshtml (partial para filtros reutilizable)
```

**Características por vista**:
- [ ] Tabla interactiva con paginación
- [ ] Columnas sortables
- [ ] Filtros dinámicos por rango de fechas, estado, usuario, etc.
- [ ] Búsqueda por texto libre
- [ ] Botones de exportación (Excel, PDF)
- [ ] Resumen de datos (totales, promedios)
- [ ] Responsive design

### Tarea 5: Sistema de Exportación (10h)
**Objetivo**: Implementar exportación a Excel y PDF

**Excel (usando EPPlus)**:
- [ ] Columnas con headers personalizados
- [ ] Formato (colores, bordes, fuentes)
- [ ] Datos con número de columnas y filas
- [ ] Fórmulas de suma/promedio automáticas
- [ ] Nombre del archivo con timestamp

**PDF (usando iTextSharp o Rotativa)**:
- [ ] Headers y footers personalizados
- [ ] Datos en tabla formateada
- [ ] Quebrado de página automático
- [ ] Orientación (portrait/landscape) adaptable

**Checklist**:
- [ ] NuGet: EPPlus instalado
- [ ] NuGet: iTextSharp o Rotativa instalado
- [ ] ExportarExcel genera archivo válido
- [ ] ExportarPdf genera archivo válido
- [ ] Download automático al cliente

### Tarea 6: Filtros y Búsqueda (8h)
**Objetivo**: Implementar sistema de filtros dinámicos

**Filtros disponibles**:
- [ ] Rango de fechas (fecha inicio, fecha fin)
- [ ] Estado (Activo, Inactivo, Completado, Cancelado)
- [ ] Usuario/Responsable (dropdown dinámico)
- [ ] Departamento/Unidad (dropdown)
- [ ] Búsqueda por nombre/descripción (text libre)
- [ ] Rango de valores (monto mínimo, máximo)

**Checklist**:
- [ ] FilterDto definido con todas las propiedades
- [ ] Validación de rangos de fechas
- [ ] Búsqueda case-insensitive
- [ ] Filtros combinables (AND logic)
- [ ] Reset de filtros funciona

### Tarea 7: Performance y Testing (6h)
**Objetivo**: Optimizar consultas y validar funcionamiento

**Optimizaciones**:
- [ ] Índices en BD para queries frecuentes
- [ ] Caching de datos con frecuencia de actualización > 1 hora
- [ ] Paginación (20-50 registros por página)
- [ ] Lazy-loading de detalles

**Testing**:
- [ ] Compilación sin errores (0 errores, 0 warnings)
- [ ] Cada reporte retorna datos correctos
- [ ] Filtros funcionan correctamente
- [ ] Exportación genera archivos válidos
- [ ] Performance < 3 segundos para 10k registros
- [ ] Responsive en mobile/tablet

**Checklist**:
- [ ] dotnet build exitoso
- [ ] Todas las vistas cargan sin errores
- [ ] Datos coinciden con WebMatrix
- [ ] Exportes se descargan sin corrupción

---

## 📊 DESGLOSE DE HORAS

| Tarea | Horas | % | Status |
|-------|-------|---|--------|
| 1. Análisis de Reportes | 4 | 6.7% | ⏳ |
| 2. ReportesController | 12 | 20% | ⏳ |
| 3. ReportesService | 10 | 16.7% | ⏳ |
| 4. Vistas Razor | 15 | 25% | ⏳ |
| 5. Exportación (Excel/PDF) | 10 | 16.7% | ⏳ |
| 6. Filtros y Búsqueda | 8 | 13.3% | ⏳ |
| 7. Performance/Testing | 6 | 10% | ⏳ |
| **TOTAL** | **60h** | **100%** | **⏳** |

**Distribución recomendada**:
- **Semana 1**: Tareas 1-4 (31h)
- **Semana 2**: Tareas 5-7 (29h)

---

## 🔧 REQUISITOS TÉCNICOS

### Dependencias NuGet a verificar/instalar:
```xml
<PackageReference Include="EPPlus" Version="7.0.0" />
<PackageReference Include="iTextSharp" Version="5.5.13.3" />
<!-- O alternativa: Rotativa para PDF -->
<PackageReference Include="Rotativa.Core" Version="2.0.0" />
```

### DbContext y Entidades requeridas:
- ✅ MatrixDbContext (web context)
- ✅ DbSet<EqQuoteHeader> para cotizaciones
- ✅ DbSet<Proyecto> y DbSet<Trabajo> para proyectos
- ✅ DbSet<WorkFlow> y DbSet<Tarea> para tareas
- ✅ MatrixNext.Data.Context.MatrixDbContext
- ✅ DbSet<TH_SolicitudAusencia> para ausencias
- ✅ Entidades OP (Muestras, sesiones cuali/cuanti)

### Arquitectura esperada:
```
Controller → Service → Adapter → DbContext/BD
    ↓
    ├─ GET Index → lista de reportes disponibles
    ├─ GET [ReportName] → carga reporte con filtros
    ├─ POST Filtros → aplica filtros dinámicos
    ├─ GET ExportarExcel → descarga Excel
    └─ GET ExportarPdf → descarga PDF
```

---

## ✅ CRITERIOS DE ACEPTACIÓN

### Code Quality:
- ✅ Compilación sin errores
- ✅ Compilación sin warnings
- ✅ Naming conventions seguidas (PascalCase, _camelCase privado)
- ✅ Métodos tienen <summary> comentados
- ✅ DI registration correcto en Program.cs

### Functionality:
- ✅ 5 reportes principales implementados
- ✅ Filtros funcionan en todos los reportes
- ✅ Búsqueda por texto libre funciona
- ✅ Exportación a Excel genera archivo válido
- ✅ Exportación a PDF genera archivo válido
- ✅ Datos coinciden con WebMatrix
- ✅ Paginación funciona correctamente

### Performance:
- ✅ Carga de reporte < 3 segundos (10k registros)
- ✅ Exportación < 5 segundos (10k registros)
- ✅ Queries optimizadas con índices
- ✅ Caching implementado donde corresponde

### UX/UI:
- ✅ Interfaces responsive (mobile/tablet/desktop)
- ✅ Tooltips en filtros
- ✅ Feedback visual de carga (spinner)
- ✅ Mensajes de error amigables
- ✅ Botones de acción claros

### Documentation:
- ✅ ANALISIS_RP_REPORTES.md completado
- ✅ Código comentado en funciones complejas
- ✅ README actualizado
- ✅ Git commits con mensajes descriptivos

---

## 🚀 HITO CRÍTICO

**Objetivo**: RP_Reportes 100% completo y funcional  
**Fecha de cierre**: 2026-01-29 (2 semanas)  
**Build status**: ✅ 0 errores, 0 warnings  
**Tests**: ✅ Todos los reportes funcionales  
**Documentación**: ✅ Completa

---

## 📝 NOTAS DE DESARROLLO

### Decisiones arquitectónicas:
1. **Excel vs CSV**: Usar EPPlus para Excel (mejor formato, estilos, fórmulas)
2. **PDF**: Evaluar entre iTextSharp (más control) vs Rotativa (simplificado)
3. **Caching**: Reportes se cachean por 1-2 horas (datos no cambian tan frecuentemente)
4. **Paginación**: 50 registros por página (balance entre performance y UX)

### Riesgos potenciales:
- 🟡 Integración entre múltiples DbContext (Web context vs Data context)
- 🟡 Performance con queries grandes (> 100k registros)
- 🟡 Compatibilidad de exportes en navegadores antiguos
- 🟡 Permisos/seguridad (algunos reportes pueden ser sensibles)

### Mitigation:
- Usar vistas/proyecciones en lugar de queries directas complejas
- Implementar índices en BD
- Testing exhaustivo con datos reales
- Validar permisos con [Authorize] y roles

---

## 📖 REFERENCIAS

- `MatrixNext/docs/GENERAL/MIGRACION_ESPECIFICACIONES.md` - Estándares de migración
- `MatrixNext/docs/GENERAL/PLAN_IMPLEMENTACION_SPRINTS.md` - Roadmap general
- WebMatrix/RP_Reportes - Fuente de referencia (si existe)
- SQL Server: `MatrixNext/docs/SQL/CO_Matrix_Structure_SP.sql` - Stored Procedures

---

## 📅 CALENDARIO TENTATIVO

| Fecha | Hito | Status |
|-------|------|--------|
| 2026-01-15 | Sprint 10 Kickoff | ✅ Hoy |
| 2026-01-16 → 2026-01-17 | Análisis + Controller (Tareas 1-2) | ⏳ |
| 2026-01-20 → 2026-01-22 | Service + Vistas (Tareas 3-4) | ⏳ |
| 2026-01-23 → 2026-01-24 | Exportación + Filtros (Tareas 5-6) | ⏳ |
| 2026-01-27 → 2026-01-28 | Testing + Optimizaciones (Tarea 7) | ⏳ |
| 2026-01-29 | Cierre Sprint 10 | ⏳ |

---

**Documento generado**: 2026-01-15 23:50  
**Status**: 🟡 EN CURSO  
**Siguiente**: Sprint 11 (OP_RO + OP_Trafico) - Estimado 2026-02-01+
