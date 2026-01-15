# ANÁLISIS: PY_ControlCalidad - Migración WebMatrix → MatrixNext

**Fecha**: 2026-01-15  
**Sprint**: 12  
**Módulo**: PY_ControlCalidad (Control de Calidad de Proyectos)  
**Estado**: 📋 ANÁLISIS COMPLETADO - Listo para iniciar implementación  
**Prioridad**: 🟡 MEDIA-BAJA  
**Dependencias**: ✅ PY_Proyectos (COMPLETADO), OP_Cualitativo (COMPLETADO)

---

## 🎯 RESUMEN EJECUTIVO

El módulo **PY_ControlCalidad** gestiona la evaluación de calidad en proyectos cualitativos, específicamente:

- **Control de calidad en trabajo de campo** (encuestadores)
- **Evaluación de moderadoras** (moderadores de focus groups)
- **Evaluación de entrevistadoras** (entrevistadores en profundidad)
- **Control de calidad de transcripciones**
- **Informes de calidad**
- **Gestión de preguntas de evaluación** (maestro de preguntas para evaluaciones)

**Volumen**: 6 páginas WebForms → 6 MVC Controllers/Services/Adapters  
**Estimación**: 4-6 semanas (30-40 horas)  
**Complejidad**: ⭐⭐⭐ MEDIA (lógica CRUD simple, pero requiere validaciones)

---

## 📋 MÓDULO EN DETALLE

### Páginas WebForms a Migrar

| # | Página | Funcionalidad | Líneas VB | Complejidad | Estado |
|---|--------|---------------|-----------|------------|--------|
| 1 | `ControlCalidadCampo.aspx` | Control de calidad de encuestadores en campo | ~259 | ⭐⭐⭐ | ❌ |
| 2 | `EvaluacionModeradora.aspx` | Evaluación de moderadoras (focus groups) | ~277 | ⭐⭐⭐ | ❌ |
| 3 | `EvaluacionEntrevistadora.aspx` | Evaluación de entrevistadoras (in-depth) | ~TBD | ⭐⭐⭐ | ❌ |
| 4 | `ControlCalidadTranscripciones.aspx` | Control de calidad de transcripciones | ~262 | ⭐⭐⭐ | ❌ |
| 5 | `ControlCalidadInforme.aspx` | Control de calidad de informes finales | ~261 | ⭐⭐⭐ | ❌ |
| 6 | `Preguntas.aspx` | Maestro: Preguntas de evaluación | ~163 | ⭐⭐ | ❌ |

**Total Páginas**: 6  
**Total LOC (estimado)**: 1,500-1,800 líneas de código

---

## 🏗️ ARQUITECTURA ESPERADA

### Patrón MVC

```
HTTP Request (ej: /PY/ControlCalidad/Index)
    ↓
[ControlCalidadController.cs]  ← Coordina requests, valida headers, retorna View/JSON
    ↓
[ControlCalidadService.cs]    ← Lógica de negocio, validaciones, transformaciones
    ↓
[ControlCalidadAdapter.cs]    ← Acceso a datos (SP execution con Dapper)
    ↓
[Database] → PY_ControlCalidad, PY_DetalleControlCalidad (tablas SQL)
```

### Estructura de Carpetas

```
MatrixNext.Web/
├── Areas/
│   └── PY/
│       ├── Controllers/
│       │   ├── ControlCalidadController.cs        ← New (6 actions)
│       │   ├── PreguntasController.cs             ← New (CRUD preguntas)
│       │   └── [otros controllers existentes]
│       ├── Views/
│       │   ├── ControlCalidad/
│       │   │   ├── Index.cshtml
│       │   │   ├── _Form.cshtml (modal)
│       │   │   ├── _Detalles.cshtml (modal detalles)
│       │   │   └── [por tipo: Campo, Moderadora, etc]
│       │   ├── Preguntas/
│       │   │   ├── Index.cshtml
│       │   │   └── _Form.cshtml (modal)
│       │   └── [otros]
│       └── [otros]
├── Services/
│   └── PY/
│       ├── IControlCalidadService.cs              ← New
│       ├── ControlCalidadService.cs               ← New
│       ├── IPreguntasService.cs                   ← New
│       ├── PreguntasService.cs                    ← New
│       └── [otros]
└── Adapters/
    └── PY/
        ├── IControlCalidadAdapter.cs              ← New
        ├── ControlCalidadAdapter.cs               ← New
        ├── IPreguntasAdapter.cs                   ← New
        ├── PreguntasAdapter.cs                    ← New
        └── [otros]
```

---

## 💾 BASE DE DATOS - MAPEO EXACTO

### Tabla: `PY_ControlCalidad`

```sql
[Id] [bigint] IDENTITY(1,1) PRIMARY KEY
[TrabajoId] [bigint] NULL           -- FK a PY_Trabajo
[Evaluador] [varchar](100) NULL     -- Nombre del evaluador
[RolEvaluador] [varchar](100) NULL  -- Rol del evaluador (ej: "Supervisor")
[Persona] [bigint] NULL             -- FK a TH_Personas (analista responsable)
[Fecha] [date] NULL                 -- Fecha de evaluación
[TipoProceso] [bigint] NULL         -- Tipo de proceso (ej: 1=Campo, 2=Moderadora, 3=Entrevistadora, etc)
```

**Índices**: 
- PK: `PK_PY_ControlCalidad`
- FK: `FK_PY_ControlCalidad_TrabajoId → PY_Trabajo(Id)`
- FK: `FK_PY_ControlCalidad_Persona → TH_Personas(Id)`

**Constraint Auditoría**: 
- `RegistradoPor` (INT, FK → US_Usuarios)
- `FechaRegistro` (DATETIME)
- `ModificadoPor` (INT, FK → US_Usuarios)
- `FechaModificacion` (DATETIME)

### Tabla: `PY_DetalleControlCalidad`

```sql
[Id] [bigint] IDENTITY(1,1) PRIMARY KEY
[IdControlCalidad] [bigint] NULL    -- FK a PY_ControlCalidad
[IdPregunta] [bigint] NULL          -- FK a PY_Preguntas
[SI] [bit] NULL                     -- Respuesta binaria (1=Cumple, 0=No cumple)
[Comentarios] [varchar](max) NULL   -- Observaciones del evaluador
```

**Índices**:
- PK: `PK_PY_DetalleControlCalidad`
- FK: `FK_PY_DetalleControlCalidad_IdControlCalidad → PY_ControlCalidad(Id)`
- FK: `FK_PY_DetalleControlCalidad_IdPregunta → PY_Preguntas(Id)`

### Tabla: `PY_Preguntas` (Maestro - Ya Existe en CoreProject)

```sql
[IdPregunta] [bigint] IDENTITY(1,1) PRIMARY KEY
[IdProceso] [bigint] NULL           -- FK a PY_Tipos_Procesos (tipo de pregunta)
[Pregunta] [varchar](max) NULL      -- Texto de la pregunta
[Activa] [bit] DEFAULT 1            -- Flag de activo/inactivo
```

---

## 🔧 STORED PROCEDURES IDENTIFICADOS

### CRUD Principal

| SP | Acción | Ubicación | Parámetros | Retorna |
|----|--------|-----------|-----------|---------|
| `PY_ControlCalidad_Add` | Crear | SQL Server | TrabajoId, Evaluador, RolEvaluador, Persona, Fecha, TipoProceso | @Id (OUTPUT) |
| `PY_ControlCalidad_Edit` | Editar | SQL Server | Id, TrabajoId, Evaluador, RolEvaluador, Persona, Fecha, TipoProceso | — |
| `PY_ControlCalidad_Del` | Eliminar | SQL Server | IdControlCalidad | — |
| `PY_ControlCalidad_Get` | Consultar 1 | SQL Server | ID | PY_ControlCalidad_Get_Result |
| `PY_ControlCalidad_GetByTrabajo` | Consultar por Trabajo | SQL Server | TrabajoId, TipoProceso | LIST(PY_ControlCalidad_Get_Result) |

### Detalles

| SP | Acción | Ubicación | Parámetros | Retorna |
|----|--------|-----------|-----------|---------|
| `PY_DetalleControlCalidad_Add` | Crear Detalle | SQL Server | IdControlCalidad, IdPregunta, SI, Comentarios | @Id (OUTPUT) |
| `PY_DetalleControlCalidad_Get` | Consultar Detalles | SQL Server | IdControlCalidad | LIST(PY_DetalleControlCalidad_Get_Result) |
| `PY_DetalleControlCalidad_DelxIdControl` | Eliminar x Control | SQL Server | IdControlCalidad | — |

### Preguntas (Maestro)

| SP | Acción | Ubicación | Parámetros | Retorna |
|----|--------|-----------|-----------|---------|
| `PY_Preguntas_Get` | Consultar Todas | SQL Server | (none) | LIST(PY_Preguntas_Result) |
| `PY_Preguntas_GetByTipo` | Por Tipo Proceso | SQL Server | IdTipoProceso | LIST(PY_Preguntas_Result) |
| `PY_Preguntas_Add` | Crear | SQL Server | IdProceso, Pregunta, Activa | @Id (OUTPUT) |
| `PY_Preguntas_Edit` | Editar | SQL Server | IdPregunta, IdProceso, Pregunta, Activa | — |
| `PY_Preguntas_Del` | Eliminar | SQL Server | IdPregunta | — |

---

## 📊 FUNCIONALIDADES DETALLADAS

### 1. **ControlCalidadCampo.aspx** → `ControlCalidadCampoController.cs`

**Propósito**: Evaluar calidad de encuestadores en trabajo de campo

**Flujo**:
1. Usuario abre página con `?idtrabajo=X`
2. Carga listado de evaluaciones existentes de ese trabajo
3. Puede crear nueva evaluación o editar existente
4. Formulario: `Evaluador`, `RolEvaluador`, `Persona (responsable)`, `Fecha`
5. Grid dinámico carga preguntas de tipo "ControlCalidadCampo"
6. Para c/pregunta: RadioButton (Cumple/No Cumple) + TextBox (Comentario)
7. Al guardar: Inserta PY_ControlCalidad + N registros en PY_DetalleControlCalidad

**Endpoints REST Esperados**:
```
GET  /PY/ControlCalidad/Campo                  → Index (listado)
GET  /PY/ControlCalidad/Campo/Create           → PartialView (_Form)
POST /PY/ControlCalidad/Campo/Create           → Guardar (JSON response)
GET  /PY/ControlCalidad/Campo/Edit/{id}        → PartialView (_Form con datos)
POST /PY/ControlCalidad/Campo/Edit/{id}        → Actualizar (JSON response)
POST /PY/ControlCalidad/Campo/Delete/{id}      → Eliminar (JSON response)
```

**Validaciones**:
- ✅ Evaluador: No vacío
- ✅ RolEvaluador: No vacío
- ✅ Persona: Debe seleccionarse
- ✅ Fecha: Formato válido
- ✅ Preguntas: Al menos 1 respuesta

**Componentes Reutilizables**:
- `_AjaxModal.cshtml` (Modal genérico)
- `_DatePicker.cshtml` (Selector fecha)
- `_SelectUser.cshtml` (Dropdown personas)
- `_Grid.cshtml` (Listado paginado)

---

### 2. **EvaluacionModeradora.aspx** → `EvaluacionModeradaController.cs`

**Propósito**: Evaluar calidad de moderadoras (focus groups)

**Flujo**: Idéntico a ControlCalidadCampo, pero:
- TipoProceso = "Moderadora"
- Preguntas específicas para evaluación de moderadoras

**Endpoints REST**:
```
GET  /PY/ControlCalidad/Moderadora             → Index
GET  /PY/ControlCalidad/Moderadora/Create      → PartialView
POST /PY/ControlCalidad/Moderadora/Create      → Guardar
POST /PY/ControlCalidad/Moderadora/Edit/{id}   → Actualizar
POST /PY/ControlCalidad/Moderadora/Delete/{id} → Eliminar
```

---

### 3. **EvaluacionEntrevistadora.aspx** → `EvaluacionEntrevistadoraController.cs`

**Propósito**: Evaluar calidad de entrevistadoras (in-depth interviews)

**Flujo**: Idéntico, pero TipoProceso = "Entrevistadora"

---

### 4. **ControlCalidadTranscripciones.aspx** → `ControlCalidadTranscripcionesController.cs`

**Propósito**: Control de calidad de transcripciones

**Diferencia**: Puede incluir campos adicionales (ej: % exactitud, errores encontrados)

---

### 5. **ControlCalidadInforme.aspx** → `ControlCalidadInformeController.cs`

**Propósito**: Control de calidad del informe final del proyecto

**Validaciones** (ejemplo):
- ✅ Estructura del informe completa
- ✅ Análisis incluido
- ✅ Conclusiones claras
- ✅ Recomendaciones fundamentadas

---

### 6. **Preguntas.aspx** → `PreguntasController.cs`

**Propósito**: CRUD de maestro de preguntas

**Flujo**:
1. Listado de preguntas filtradas por Tipo de Proceso
2. Crear nueva pregunta
3. Editar pregunta existente
4. Activar/Desactivar pregunta

**Endpoints REST**:
```
GET  /PY/Preguntas                    → Index (listado)
GET  /PY/Preguntas/Create             → PartialView (_Form)
POST /PY/Preguntas/Create             → Guardar (JSON)
POST /PY/Preguntas/Edit/{id}          → Actualizar (JSON)
POST /PY/Preguntas/Toggle/{id}        → Activar/Desactivar (JSON)
```

---

## 📈 COMPLEJIDAD Y RIESGOS

### Complejidad por Componente

| Componente | Complejidad | Razón |
|------------|-------------|-------|
| ControlCalidadCampo | ⭐⭐⭐ | Grid dinámico con RadioButtons + validaciones |
| EvaluacionModeradora | ⭐⭐⭐ | Similar a ControlCalidadCampo |
| EvaluacionEntrevistadora | ⭐⭐⭐ | Similar a ControlCalidadCampo |
| ControlCalidadTranscripciones | ⭐⭐⭐ | Similar a ControlCalidadCampo |
| ControlCalidadInforme | ⭐⭐⭐ | Similar a ControlCalidadCampo |
| Preguntas (Maestro) | ⭐⭐ | CRUD simple |

**Total Complejidad Promedio**: ⭐⭐⭐ MEDIA

### Riesgos Identificados

| # | Riesgo | Probabilidad | Impacto | Mitigación |
|---|--------|-------------|--------|-----------|
| 1 | SP PY_ControlCalidad_Add puede no existir en BD | 🟡 Media | 🔴 Alto | Verificar/crear SP antes de implementar |
| 2 | Tablas PY_ControlCalidad puede no tener auditoría (RegistradoPor) | 🟡 Media | 🟠 Medio | Agregar columnas si falta |
| 3 | Grid dinámico con RadioButtons es complejo en Razor | 🟡 Media | 🟠 Medio | Usar EditorFor templating |
| 4 | Validación de preguntas activas en cliente puede desincronizarse | 🟢 Baja | 🟠 Medio | Validar en server siempre |
| 5 | Dependencia en PY_Trabajo (trabajo debe existir) | 🟢 Baja | 🔴 Alto | Validar FK antes de crear |

---

## 🎯 PLAN DE IMPLEMENTACIÓN (Detallado)

### Fase 1: Infraestructura Base (2-3 días)

**Tareas**:
1. ✅ Verificar/crear SP en SQL Server
2. ✅ Crear DbContext mappings (PY_ControlCalidad, PY_DetalleControlCalidad)
3. ✅ Crear DTOs (InputDto, OutputDto, ListDto)
4. ✅ Registrar DI en Program.cs

**Evidencia**:
- `MatrixNext/MatrixNext.Infrastructure/Adapters/PY/IControlCalidadAdapter.cs`
- `MatrixNext/MatrixNext.Infrastructure/Adapters/PY/ControlCalidadAdapter.cs`
- `MatrixNext/MatrixNext.Core/Interfaces/IPreguntasAdapter.cs`
- DTOs en `Dtos/` carpeta
- Program.cs actualizado

---

### Fase 2: Servicios de Lógica (3-4 días)

**Tareas**:
1. ✅ Implementar `IControlCalidadService` (6 métodos)
2. ✅ Implementar `IPreguntasService` (4 métodos)
3. ✅ Validaciones de negocio
4. ✅ Logging de auditoría

**Métodos ControlCalidadService**:
```csharp
Task<List<ControlCalidadListDto>> ObtenerPorTrabajoAsync(long trabajoId, int tipoProceso)
Task<ControlCalidadDetailDto> ObtenerPorIdAsync(long id)
Task<(bool success, string message, long id)> CrearAsync(ControlCalidadInputDto dto, int userId)
Task<(bool success, string message)> EditarAsync(long id, ControlCalidadInputDto dto, int userId)
Task<(bool success, string message)> EliminarAsync(long id, int userId)
Task<List<PreguntaDto>> ObtenerPreguntasActivasAsync(int tipoProceso)
```

**Métodos PreguntasService**:
```csharp
Task<List<PreguntaListDto>> ObtenerPorTipoAsync(int tipoProceso)
Task<(bool success, string message, long id)> CrearAsync(PreguntaInputDto dto, int userId)
Task<(bool success, string message)> EditarAsync(long id, PreguntaInputDto dto, int userId)
Task<(bool success, string message)> ToggleActivoAsync(long id, int userId)
```

---

### Fase 3: Controllers (2-3 días)

**Tareas**:
1. ✅ Crear `ControlCalidadController` (6 actions)
2. ✅ Crear `PreguntasController` (4 actions)
3. ✅ Validar `ModelState` y permisos `[Authorize]`
4. ✅ Manejar excepciones sin exponer stack traces

**Actions ControlCalidadController**:
```csharp
[HttpGet] Task<IActionResult> Index(long? trabajoId, int tipoProceso)
[HttpGet] Task<IActionResult> Create(int tipoProceso)
[HttpPost] Task<IActionResult> Create(ControlCalidadInputDto dto)
[HttpGet] Task<IActionResult> Edit(long id)
[HttpPost] Task<IActionResult> Edit(long id, ControlCalidadInputDto dto)
[HttpPost] Task<IActionResult> Delete(long id)
```

---

### Fase 4: Vistas (2-3 días)

**Tareas**:
1. ✅ Crear `Index.cshtml` (Listado con grid paginado)
2. ✅ Crear `_Form.cshtml` (Modal de creación/edición)
3. ✅ Crear `_Detalles.cshtml` (Modal con preguntas dinámicas)
4. ✅ Crear vista Preguntas (CRUD simple)
5. ✅ Scripts AJAX para modal + grid refresh

**Componentes Principales**:
- Modal genérico (Bootstrap)
- Grid con paginación (DataTables o similar)
- RadioButtonList dinámico (EditorTemplate)
- DatePicker
- Select con personas

---

### Fase 5: Testing y QA (1-2 días)

**Tareas**:
1. ✅ Crear (Validar campos y guardar correctamente)
2. ✅ Editar (Cargar y actualizar datos)
3. ✅ Eliminar (Con confirmación)
4. ✅ Búsqueda/Filtros
5. ✅ Paginación
6. ✅ Errores (BD offline, datos inválidos)
7. ✅ Auditoría (Log de creación/edición/eliminación)

---

## 📦 DEPENDENCIAS Y PREREQUISITOS

### Dependencias de Módulos
- ✅ **PY_Proyectos** (Must exist - for TrabajoId FK)
- ✅ **TH_TalentoHumano** (Must exist - for Persona FK)
- ✅ **US_Usuarios** (Must exist - for auditoría)

### Dependencias Técnicas
- ✅ .NET 8 (C# 12)
- ✅ Entity Framework Core 8
- ✅ Dapper 2.x
- ✅ Bootstrap 5
- ✅ jQuery 3.6+
- ✅ DataTables (opcional, para grid paginado)

### BD Prerequisites
- ✅ Tabla `PY_ControlCalidad` debe existir
- ✅ Tabla `PY_DetalleControlCalidad` debe existir
- ✅ Tabla `PY_Preguntas` debe existir (ya existe)
- ✅ SP `PY_ControlCalidad_*` deben existir
- ✅ FK a `PY_Trabajo` y `TH_Personas`

---

## 📋 CHECKLIST PRE-IMPLEMENTACIÓN

Antes de iniciar desarrollo, verificar:

- [ ] ✅ SP `PY_ControlCalidad_Get`, `_Add`, `_Edit`, `_Del` existen en BD
- [ ] ✅ SP `PY_DetalleControlCalidad_Get`, `_Add`, `_Del*` existen en BD
- [ ] ✅ Tabla `PY_ControlCalidad` con auditoría (RegistradoPor, FechaRegistro)
- [ ] ✅ Tabla `PY_DetalleControlCalidad` con FK correctas
- [ ] ✅ Tabla `PY_Preguntas` tiene columna `Activa`
- [ ] ✅ Enum `TipoProceso` incluye: ControlCalidadCampo, Moderadora, Entrevistadora, Transcripciones, Informe
- [ ] ✅ DbContext (PY_Entities) mapea ambas tablas
- [ ] ✅ PY_Proyectos está 100% funcional (para FK)
- [ ] ✅ TH_TalentoHumano está 100% funcional (para FK)

---

## 🗂️ FICHEROS A CREAR

### Backend

```
MatrixNext.Infrastructure/
├── Adapters/PY/
│   ├── IControlCalidadAdapter.cs           (60 LOC)
│   ├── ControlCalidadAdapter.cs            (180 LOC)
│   ├── IPreguntasAdapter.cs                (40 LOC)
│   └── PreguntasAdapter.cs                 (120 LOC)

MatrixNext.Core/
├── Interfaces/
│   ├── IControlCalidadService.cs           (50 LOC)
│   └── IPreguntasService.cs                (40 LOC)
├── Services/PY/
│   ├── ControlCalidadService.cs            (280 LOC)
│   └── PreguntasService.cs                 (200 LOC)

MatrixNext.Web/
├── Areas/PY/Controllers/
│   ├── ControlCalidadController.cs         (250 LOC)
│   └── PreguntasController.cs              (150 LOC)
├── DTOs/
│   ├── ControlCalidadInputDto.cs           (30 LOC)
│   ├── ControlCalidadListDto.cs            (25 LOC)
│   ├── ControlCalidadDetailDto.cs          (50 LOC)
│   ├── PreguntaInputDto.cs                 (20 LOC)
│   └── PreguntaListDto.cs                  (15 LOC)
```

### Frontend

```
MatrixNext.Web/
├── Areas/PY/Views/
│   ├── ControlCalidad/
│   │   ├── Index.cshtml                    (120 LOC)
│   │   ├── _Form.cshtml                    (150 LOC)
│   │   └── _DetallesGrid.cshtml            (200 LOC)
│   └── Preguntas/
│       ├── Index.cshtml                    (80 LOC)
│       └── _Form.cshtml                    (80 LOC)
├── wwwroot/js/
│   └── py-controlcalidad.js                (300 LOC)
├── wwwroot/css/
│   └── py-controlcalidad.css               (100 LOC)
```

**Total LOC Esperadas**: 2,300-2,500 líneas (3-4x líneas WebForms)

---

## 📊 TIMELINE ESTIMADO

### Sprint 12 - Parte B (Después de GD/PY completados)

| Día | Fase | Tareas | Horas | Entregable |
|-----|------|--------|-------|-----------|
| Día 1-2 | Infra | SP verificación, DbContext, DTOs, DI | 8h | Infrastructure lista |
| Día 3-4 | Services | IControlCalidadService, IPreguntasService | 10h | Servicios + tests |
| Día 5 | Controllers | ControlCalidadController, PreguntasController | 6h | Controllers básicos |
| Día 6-7 | Vistas | Index, _Form, _DetallesGrid | 10h | UI completa |
| Día 8 | Testing | QA funcional, edge cases | 4h | Módulo validado |
| Día 9 | Documentación | MIGRACION completada, actualizaciones | 2h | Docs finalizadas |

**Total**: 40 horas = 5 días de 8h/día

---

## 📖 DOCUMENTACIÓN DE REFERENCIA

### Documentos Relacionados
- [DIRECTRICES_MIGRACION.md](../../DIRECTRICES_MIGRACION.md) - 15 reglas obligatorias
- [MODULOS_MIGRACION.md](../../MODULOS_MIGRACION.md) - Estado general de módulos
- [MIGRACION_PY_PROYECTOS.md](MIGRACION_PY_PROYECTOS.md) - Patrón PY (dependencia)
- [SPRINT_12_3_COMPLETADO.md](../../GENERAL/SPRINT_12_3_COMPLETADO.md) - Último sprint completado

### Ejemplos de Código
- [TH_AusenciasService.cs](../../TH/) - Patrón Service (referencia)
- [OP_CualitativoAdapter.cs](../../OP/) - Patrón Adapter (referencia)
- [RP_ReportesController.cs](../../RP/) - Patrón Controller (referencia)

---

## ✅ ESTADO FINAL DE ANÁLISIS

**Análisis Completado**: ✅  
**Riesgos Identificados**: 5 (todos mitigables)  
**Prioridad**: 🟡 MEDIA-BAJA (después de PY_Proyectos)  
**Listo para Implementación**: ✅ SÍ

**Próximo Paso**: Crear infraestructura (Adapters, DTOs, SP verification)

---

**Documento**: ANALISIS_PY_CONTROLCALIDAD.md  
**Versión**: 1.0  
**Fecha Actualización**: 2026-01-15  
**Autor**: GitHub Copilot  
**Revisor**: [Pendiente]
