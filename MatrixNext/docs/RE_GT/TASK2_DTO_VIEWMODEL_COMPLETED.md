# TASK 2: DTO + VIEWMODEL (COMPLETADA)

**Duración**: 1-2 horas  
**Fecha**: 2026-01-15  
**Archivos Creados**: 2 archivos (453 líneas)

---

## ✅ ARCHIVOS CREADOS

### 1. **TareasPorUnidadDto.cs** (156 líneas)

**Ubicación**: `MatrixNext.Web/DTOs/CORE/TareasPorUnidadDto.cs`

**Propiedades principales**:
```csharp
- IdWorkFlow: long (PK WorkFlow)
- IdTrabajo: long (FK Trabajo)
- JobBook: string
- NombreTrabajo: string
- Muestra: int
- NombreMetodologia: string
- NombreCOE: string
- NombreUnidad: string (Crítica, Verificación, Captura, etc)
- IdUnidad: int (5-14)
- Estado: string (Creada, EnProgreso, Completada, Anulada)
- Prioridad: int (1=Normal, 2=Alta, 3=Baja)
- FechaVencimiento: DateTime?
- UsuariosAsignados: int
- EsProyectoCualitativo: bool
```

**Propiedades Calculadas**:
```csharp
- EstadoDisplay: Display humanizado del estado
- PrioridadDisplay: Display humanizado de prioridad
- PrioridadCssClass: Clase Bootstrap para badge
- EstadoCssClass: Clase Bootstrap para badge
- EsUrgente: bool (vence en 3 días)
- EsVencida: bool
```

**Utilidad**: Mapeo 1:1 con resultado de SP `WorkFlow.obtenerTrabajosWorkFlow`

---

### 2. **TraficoTareasViewModel.cs** (297 líneas)

**Ubicación**: `MatrixNext.Web/ViewModels/CORE/TraficoTareasViewModel.cs`

#### A) **TraficoTareasViewModel** (Principal)

**Propiedades de Listado**:
```csharp
- Tareas: List<TareasPorUnidadDto> (listado principal)
- TotalRegistros: int
- PaginaActual: int (1-based)
- RegistrosPorPagina: int (default 25)
- TotalPaginas: int (calculated)
```

**Propiedades de Filtros**:
```csharp
- FiltroUnidad: int? (5-14)
- FiltroEstado: string? (Creada, EnProgreso, etc)
- FiltroPrioridad: int? (1=Normal, 2=Alta, 3=Baja)
- FiltroBusqueda: string? (nombre trabajo, jobbook)
```

**Propiedades de Contexto**:
```csharp
- UnidadesDisponibles: List<UnidadTraficoDto> (dropdown)
- IdUnidadActual: int (validación permisos)
- NombreUnidadActual: string?
- IdRolUsuario: int
- URLRetorno: URLRetornoEnum?
- IdTrabajoSeleccionado: long?
- NombreTrabajoSeleccionado: string?
- MostrarListado: bool (Accordion 0 vs 1)
```

**Propiedades Calculadas**:
```csharp
- MostrarPersonalAsignado: bool (solo unidades 11, 14)
- HayRegistros: bool
- TareasUrgentes: int
- TareasVencidas: int
- TareasEnProgreso: int
- TareasCompletadas: int
```

#### B) **UnidadTraficoDto**

**Propiedades**:
```csharp
- Id: int
- Nombre: string
- PermId: int (mapeo a permiso)
- GrupoOrigen: string ("Gestión" | "Recolección")

// Método helper:
- ObtenerUnidadesTrafico(): List<UnidadTraficoDto> (10 unidades)
```

#### C) **URLRetornoEnum**

```csharp
public enum URLRetornoEnum : int
{
    RE_GT_TraficoTareas_Scripting = 1,
    RE_GT_TraficoTareas_Pilotos = 2,
    RE_GT_TraficoTareas_Critica = 3,
    RE_GT_TraficoTareas_Verificacion = 4,
    RE_GT_TraficoTareas_Captura = 5,
    RE_GT_TraficoTareas_Codificacion = 6,
    RE_GT_TraficoTareas_Datacleaning = 7,
    RE_GT_TraficoTareas_Procesamiento = 8,
    RE_GT_TraficoTareas_Estadistica = 9,
    CORE_ListaTrabajosTareas = 10,
    RE_GT_TrabajosPorGerencia = 11,
    RE_GT_TraficoEncuestasRMC = 12,
    RE_GT_CallCenter = 13,
    Default = 0
}
```

#### D) **URLRetornoHelper**

**Método**: `ObtenerUrlRetorno(URLRetornoEnum?, baseUrl) → string?`

Mapea 14 casos de URLRetorno a URLs de retorno específicas:
- Gestión → HomeGestionTratamiento
- Recolección → HomeRecoleccion
- Estadística → ES_Estadistica/Default
- etc.

---

## 📋 CHECKLIST TASK 2

- [x] DTO TareasPorUnidadDto creado (156 líneas)
  - [x] Propiedades mapeadas 1:1 con SP
  - [x] Propiedades calculadas (Display, Css, Urgencia)
- [x] ViewModel TraficoTareasViewModel creado (297 líneas)
  - [x] Listado + paginación
  - [x] Filtros (Unidad, Estado, Prioridad, Búsqueda)
  - [x] Contexto (Unidades, Rol, URLRetorno)
  - [x] Propiedades calculadas (Urgentes, Vencidas, etc)
- [x] DTO UnidadTraficoDto creado (10 unidades)
  - [x] Método helper ObtenerUnidadesTrafico()
- [x] Enum URLRetornoEnum creado (14 casos)
- [x] Helper URLRetornoHelper creado
- [x] Archivo compilable (sin errores)

**TASK 2 COMPLETADA ✅** - Listo para TASK 3 (Service + Adapter)

---

**Documentación**: 2026-01-15 15:25 UTC  
**Próxima**: TASK 3 - Extender WorkFlowService + Adapter (1-2 horas)
