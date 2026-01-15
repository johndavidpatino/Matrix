# SPRINT 11 - DISCOVERY ANALYSIS

**Fecha**: 2026-01-15  
**Análisis**: OP_RO + OP_Trafico  
**Status**: 🟡 PRE-IMPLEMENTATION

---

## 🔍 DESCUBRIMIENTOS CLAVE

### OP_RO (Operational Review)

#### Controllers ✅
**Archivo**: `MatrixNext.Web/Areas/OP/Controllers/OP_ROController.cs`
- **LOC**: 93 líneas
- **Status**: Skeleton (TODO stubs)
- **Endpoints existentes**: 2
  - `GET /api/op/op_ro` - Listar revisiones
  - `GET /api/op/op_ro/{id}` - Obtener revisión

#### DTOs ✅
**Archivo**: `MatrixNext.Data/Models/OP_RO/OP_RODTO.cs`
- **LOC**: 261 líneas
- **Status**: Completamente definido
- **Tipos soportados**:
  - `OP_ROReviewDTO` - Revisión base
  - `OP_ROCuestionarioDTO` - Cuestionarios
  - `OP_ROInstructivoDTO` - Instructivos
  - `OP_ROMetodologiaDTO` - Metodologías
  - `OP_ROMaterialAyudaDTO` - Material de ayuda
  - Componentes anidados (Preguntas, Pasos, Fases, etc.)

#### Vistas ✅
**Ubicación**: `MatrixNext.Web/Areas/OP/Views/OP_RO/`
- Status: Carpeta existe (necesita crear .cshtml)

#### Services & Adapters ❌
- `IOP_ROService` - **NO EXISTE**
- `OP_ROService` - **NO EXISTE**
- `IOP_ROAdapter` - **NO EXISTE**
- `OP_ROAdapter` - **NO EXISTE**

**Acción**: CREAR en Sprint 11

#### WebMatrix Equivalente ✅
**Ubicación**: `WebMatrix/OP_RO/`
- **Archivos**: 12 (4 funcionalidades × 3 archivos cada una)
  ```
  Cuestionario.aspx(.designer.vb, .vb)
  Instructivo.aspx(.designer.vb, .vb)
  MaterialAyuda.aspx(.designer.vb, .vb)
  Metodologia.aspx(.designer.vb, .vb)
  ```

---

### OP_Trafico (Operational Traffic)

#### Controllers ✅
**Archivo**: `MatrixNext.Web/Areas/OP/Controllers/OP_TraficoController.cs`
- **LOC**: 93 líneas
- **Status**: Skeleton (TODO stubs)
- **Endpoints existentes**: 2
  - `GET /api/op/op_trafico` - Listar eventos
  - `GET /api/op/op_trafico/{id}` - Obtener evento

#### DTOs ✅
**Archivo**: `MatrixNext.Data/Models/OP_Trafico/OP_TraficoDTOS.cs`
- **LOC**: 347 líneas
- **Status**: Completamente definido
- **Tipos soportados**:
  - `OP_TraficoEventoDTO` - Evento base
  - `OP_TraficoCapturadoDTO` - Estado: Capturado
  - `OP_TraficoCriticadoDTO` - Estado: Criticado
  - `OP_TraficoVerificadoDTO` - Estado: Verificado
  - `OP_TraficoAnuladoDTO` - Estado: Anulado
  - Componentes anidados (Datos, Errores, Advertencias, Inconsistencias)

#### Vistas ✅
**Ubicación**: `MatrixNext.Web/Areas/OP/Views/OP_Trafico/`
- Status: Carpeta existe (necesita crear .cshtml)

#### Services & Adapters ❌
- `IOP_TraficoService` - **NO EXISTE**
- `OP_TraficoService` - **NO EXISTE**
- `IOP_TraficoAdapter` - **NO EXISTE**
- `OP_TraficoAdapter` - **NO EXISTE**

**Acción**: CREAR en Sprint 11

#### WebMatrix Equivalente ✅
**Ubicación**: `WebMatrix/OP_Trafico/`
- **Archivos**: 18 (6 responsabilidades × 3 archivos cada una)
  ```
  Captura.aspx(.designer.vb, .vb)
  Critica.aspx(.designer.vb, .vb)
  InicioTraficoEncuestas.aspx(.designer.vb, .vb)
  RMC.aspx(.designer.vb, .vb)
  TrabajosProyectos.aspx(.designer.vb, .vb)
  Verificacion.aspx(.designer.vb, .vb)
  ```

---

## 📋 SCOPE VERIFICATION (Regla 6)

### OP_RO
| Acción | WebMatrix | Legacy | Scope |
|--------|-----------|--------|-------|
| Listar cuestionarios | ✅ Cuestionario.aspx | Sí | ✅ INCLUIR |
| Crear cuestionario | ✅ Cuestionario.aspx | Sí | ✅ INCLUIR |
| Editar cuestionario | ✅ Cuestionario.aspx | Sí | ✅ INCLUIR |
| Aprobar/Rechazar | ✅ (workflow) | Sí | ✅ INCLUIR |
| Listar instructivos | ✅ Instructivo.aspx | Sí | ✅ INCLUIR |
| Listar metodologías | ✅ Metodologia.aspx | Sí | ✅ INCLUIR |
| Listar material | ✅ MaterialAyuda.aspx | Sí | ✅ INCLUIR |

**Conclusión**: Todas las acciones existen en legacy → INCLUIR TODO

### OP_Trafico
| Acción | WebMatrix | Legacy | Scope |
|--------|-----------|--------|-------|
| Listar eventos | ✅ InicioTraficoEncuestas.aspx | Sí | ✅ INCLUIR |
| Capturar datos | ✅ Captura.aspx | Sí | ✅ INCLUIR |
| Criticar datos | ✅ Critica.aspx | Sí | ✅ INCLUIR |
| Verificar datos | ✅ Verificacion.aspx | Sí | ✅ INCLUIR |
| Gestionar RMC | ✅ RMC.aspx | Sí | ✅ INCLUIR |
| Trabajos/Proyectos | ✅ TrabajosProyectos.aspx | Sí | ✅ INCLUIR |
| Historial cambios | ✅ (embedded) | Sí | ✅ INCLUIR |

**Conclusión**: Todas las acciones existen en legacy → INCLUIR TODO

---

## 🏗️ ARQUITECTURA REQUERIDA

### OP_RO Implementation Plan

```
Controllers/OP_ROController.cs (93 LOC actual → expandir a 300-400 LOC)
  ├─ GetRevisiones() - Implementar
  ├─ GetRevision(id) - Implementar
  ├─ CreateRevision(dto) - AGREGAR
  ├─ UpdateRevision(id, dto) - AGREGAR
  ├─ DeleteRevision(id) - AGREGAR
  ├─ ApproveRevision(id, dto) - AGREGAR
  └─ RejectRevision(id, dto) - AGREGAR

Services/OP_ROService.cs (CREAR)
  ├─ GetRevisionsAsync(filtros)
  ├─ GetRevisionAsync(id)
  ├─ CreateRevisionAsync(dto)
  ├─ UpdateRevisionAsync(id, dto)
  ├─ DeleteRevisionAsync(id)
  ├─ ApproveRevisionAsync(id)
  └─ RejectRevisionAsync(id)

Adapters/OP_ROAdapter.cs (CREAR)
  ├─ GetRevisionsAsync(filtros) - SP: OP_RO_Get o similar
  ├─ GetRevisionAsync(id) - SP: OP_RO_GetById
  ├─ CreateRevisionAsync(dto) - SP: OP_RO_Insert
  ├─ UpdateRevisionAsync(dto) - SP: OP_RO_Update
  ├─ DeleteRevisionAsync(id) - SP: OP_RO_Delete
  ├─ ApproveRevisionAsync(id) - SP: OP_RO_Approve
  └─ RejectRevisionAsync(id) - SP: OP_RO_Reject
```

### OP_Trafico Implementation Plan

```
Controllers/OP_TraficoController.cs (93 LOC actual → expandir a 400-500 LOC)
  ├─ GetEventos() - Implementar
  ├─ GetEvento(id) - Implementar
  ├─ CaptureData(dto) - AGREGAR
  ├─ CriticsData(id, dto) - AGREGAR
  ├─ VerifyData(id, dto) - AGREGAR
  ├─ AnnulEvent(id, dto) - AGREGAR
  └─ GetHistorial(id) - AGREGAR

Services/OP_TraficoService.cs (CREAR)
  ├─ GetEventosAsync(filtros)
  ├─ GetEventoAsync(id)
  ├─ CaptureDataAsync(dto)
  ├─ CriticsDataAsync(id, dto)
  ├─ VerifyDataAsync(id, dto)
  ├─ AnnulEventAsync(id, dto)
  └─ GetHistorialAsync(id)

Adapters/OP_TraficoAdapter.cs (CREAR)
  ├─ GetEventosAsync(filtros) - SP: OP_Trafico_Get
  ├─ GetEventoAsync(id) - SP: OP_Trafico_GetById
  ├─ InsertEventoAsync(dto) - SP: OP_Trafico_Insert
  ├─ UpdateCapturadoAsync(dto) - SP: OP_Trafico_Captura_Update
  ├─ UpdateCriticadoAsync(dto) - SP: OP_Trafico_Critica_Update
  ├─ UpdateVerificadoAsync(dto) - SP: OP_Trafico_Verificacion_Update
  ├─ UpdateAnuladoAsync(dto) - SP: OP_Trafico_Anula_Update
  └─ GetHistorialAsync(id) - SP: OP_Trafico_Historial_Get
```

---

## 🔗 STORED PROCEDURES REQUERIDOS

**Necesario buscar en `MatrixNext/docs/SQL/`**:

### OP_RO SPs
- `OP_RO_Get` - Listar revisiones
- `OP_RO_GetById` - Obtener por ID
- `OP_RO_Insert` - Crear
- `OP_RO_Update` - Editar
- `OP_RO_Delete` - Eliminar
- `OP_RO_Approve` - Aprobar
- `OP_RO_Reject` - Rechazar

### OP_Trafico SPs
- `OP_Trafico_Get` - Listar eventos
- `OP_Trafico_GetById` - Obtener por ID
- `OP_Trafico_Insert` - Crear evento
- `OP_Trafico_Captura_Update` - Actualizar captura
- `OP_Trafico_Critica_Update` - Actualizar crítica
- `OP_Trafico_Verificacion_Update` - Actualizar verificación
- `OP_Trafico_Anula_Update` - Anular evento
- `OP_Trafico_Historial_Get` - Obtener historial

---

## ✅ BUILD STATUS

```
✅ Controllers existen (skeleton)
✅ DTOs completamente definidos
✅ Vistas carpetas existen
✅ Compilación: 0 Errores (actual)
```

---

## 📊 ESTIMACIONES

### OP_RO
- Create Services & Adapters: 8h
- Implement 7 endpoints: 12h
- Implement Vistas (4 tipos): 10h
- Testing: 5h
- **Subtotal OP_RO: 35h**

### OP_Trafico
- Create Services & Adapters: 10h
- Implement 7 endpoints: 14h
- Implement Vistas (6 tipos): 12h
- Testing: 6h
- **Subtotal OP_Trafico: 42h**

### Total Sprint 11
- **OP_RO + OP_Trafico: 77h** (vs 80h estimated)

---

## 🚀 PRÓXIMOS PASOS

1. **Buscar SPs** en `MatrixNext/docs/SQL/CO_Matrix_SP_Names.csv`
2. **Leer code-behind** de páginas WebMatrix para entender lógica
3. **Crear Services**:
   - `IOP_ROService` + `OP_ROService`
   - `IOP_TraficoService` + `OP_TraficoService`
4. **Crear Adapters**:
   - `IOP_ROAdapter` + `OP_ROAdapter`
   - `IOP_TraficoAdapter` + `OP_TraficoAdapter`
5. **Expandir Controllers** con métodos reales
6. **Crear Vistas** Razor (Index, Create, Edit, Details)
7. **Testing** manual
8. **Documentación** Sprint 11 COMPLETADO

---

**Status**: 🟡 Ready for implementation  
**Blocker**: Necesita confirmación de nombre exacto de SPs  
**Action**: Comenzar búsqueda de SPs mañana

---

*Generado: 2026-01-15 14:50 UTC*  
*Sprint 11 Discovery - Analysis Complete*
