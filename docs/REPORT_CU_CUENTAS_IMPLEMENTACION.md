# 🔍 AUDITORÍA DE MIGRACIÓN: CU_CUENTAS (Fase 1)

**Fecha de Auditoría**: 2026-01-03  
**Auditor**: GitHub Copilot  
**Documentos Comparados**:
- 📘 **Fuente de Verdad**: `ANALISIS_CU_CUENTAS.md` (Plan de migración)
- 📗 **Implementación**: `REPORT_CU_CUENTAS_IMPLEMENTACION.md` (Lo ejecutado)

---

## 📊 RESUMEN EJECUTIVO

### Paridad General

| Aspecto | Planificado | Implementado | Paridad % | Estado |
|---------|-------------|--------------|-----------|--------|
| **Controllers** | 4 | 4 | 100% | ✅ COMPLETO |
| **Rutas MVC** | 22 | 18 | 82% | 🟠 PARCIAL |
| **Services** | 4 | 4 | 100% | ✅ COMPLETO |
| **DataAdapters** | 4 | 4 | 100% | ✅ COMPLETO |
| **ViewModels** | 20-25 | 15-20 | 75% | 🟠 PARCIAL |
| **Views (.cshtml)** | 15-20 | 12-15 | 70% | 🟠 PARCIAL |
| **Entidades EF** | 6 principales | 5 | 83% | 🟠 FALTA 1 |
| **Funcionalidad P0 (MVP)** | 18 tareas | 14 tareas | 78% | 🟠 PARCIAL |

**PARIDAD GLOBAL ESTIMADA**: **~80%** 🟠

### Riesgos Detectados

| Riesgo | Severidad | Descripción | Impacto |
|--------|-----------|-------------|---------|
| **SP `CU_Brief_Clone` confirmado** | 🟢 BAJO | SP de clonación identificado, requiere solo implementación de adapter | Funcionalidad desbloqueada, implementación directa |
| **Auto-creación de Propuesta no implementada** | 🔴 ALTO | Al guardar Brief, no se crea Propuesta automáticamente (análisis: Frame.aspx líneas 356-365) | Flujo de negocio incompleto |
| **Presupuestos aprobados no asignados** | 🟠 MEDIO | Modal de estudios no asigna presupuestos (análisis: `CU_Presupuestos.DevolverxIdPropuestaAprobados`) | Creación de estudios incompleta |
| **Documentos no migrados** | � BAJO | Componente Dropzone ya disponible, requiere integración | Carga de archivos con componente existente (P1-03, P1-09) - reducción de esfuerzo |
| **Emails no implementados** | 🟡 BAJO | No se envían emails al crear estudios (análisis: P1-10, P1-11) | Feature P1, no crítico para MVP |
| **Tabs de Brief no implementados** | 🟡 BAJO | Formulario Brief de 70+ campos sin organización en tabs (análisis: P1-13) | UX subóptima, no bloqueante |

### Hallazgos Clave

#### ✅ **Fortalezas (Implementado Correctamente)**
1. **Arquitectura base sólida**: Controllers, Services, Adapters creados según análisis
2. **QuillEditor integrado**: 4 campos HTML usan componente existente (análisis: sección 6 - riesgo mitigado)
3. **Session eliminado**: Implementación sin `Session("InfoJobBook")` (análisis: decisión técnica confirmada)
4. **Validaciones por estado**: Propuestas tienen validaciones dinámicas (análisis: sub-tabla validaciones)
5. **PY_Proyectos omitido**: No crea proyectos al guardar estudio (análisis: decisión técnica)

#### ❌ **Faltantes Críticos (P0)**
1. **ClonarBrief**: Ruta `/CU/Cuentas/Clonar` no funcional (análisis: tabla mapeo, Default.aspx → Clonar)
2. **Auto-creación Propuesta**: `BriefService.GuardarBrief()` no crea propuesta automática (análisis: Frame.aspx línea 356-365)
3. **Presupuestos en Estudios**: `EstudiosController.Crear` no valida/asigna presupuestos aprobados (análisis: Estudio.aspx líneas 111-149)
4. **JobBookContextBanner**: No existe componente de contexto superior (análisis: componente nuevo P0)

#### ⚠️ **Posibles Invenciones (No en Análisis)**
1. **Modal de Observaciones en implementación**: Análisis menciona "historial observaciones" pero no especifica modal separado para editar
2. **Export/Logging legacy mencionado como pendiente**: No está en análisis original (puede ser correcto, verificar código legacy)

---

## 🗂️ MATRIZ DE TRAZABILIDAD DETALLADA

### 1️⃣ DEFAULT.ASPX → CuentasController

| Funcionalidad (Análisis) | Ruta MVC (Análisis) | Implementado | Estado | Evidencia Análisis | Evidencia Reporte | Nota |
|---------------------------|---------------------|--------------|--------|-------------------|-------------------|------|
| Dashboard/búsqueda | `/CU/Cuentas` | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 1 | "Rutas: /CU/Cuentas (GET) Index" | Paridad 1:1 |
| Ejecutar búsqueda AJAX | `/CU/Cuentas/Buscar` (POST) | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 2 | "/CU/Cuentas/Buscar (POST parcial)" | Paridad 1:1 |
| Crear nuevo JobBook | `/CU/Brief` (GET) | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 3 | Redirige a Brief/Index | Limpiar contexto OK |
| Ver detalle JobBook | `/CU/Brief/{id}`, `/CU/Propuestas/{id}`, `/CU/Estudios/{id}` | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 4 | "/CU/Cuentas/Abrir (GET redireccion segun contexto)" | Lógica de redirección implementada |
| Modal clonar Brief | `/CU/Cuentas/Clonar` (GET + POST) | ❌ PARCIAL | 📝 TODO (P0) | Sección 4, tabla mapeo fila 5 | "/CU/Cuentas/Clonar (POST JSON, **pendiente por SP**)" | ✅ **READY**: SP confirmado como `CU_Brief_Clone`, requiere implementación de DataAdapter + Controller + Modal |

**Resumen Default.aspx**: 4/5 funcionalidades OK (80%)  
**Pendiente P0**: CloneBrief (SP `CU_Brief_Clone` confirmado, listo para implementar)

---

### 2️⃣ FRAME.ASPX → BriefController

| Funcionalidad (Análisis) | Ruta MVC (Análisis) | Implementado | Estado | Evidencia Análisis | Evidencia Reporte | Nota |
|---------------------------|---------------------|--------------|--------|-------------------|-------------------|------|
| Crear/Editar Brief (GET) | `/CU/Brief` o `/CU/Brief/{id}` | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 6 | "/CU/Brief (GET Index)" | Formulario con QuillEditor OK |
| Guardar Brief (POST) | `/CU/Brief/Guardar` | ✅ SÍ | 🟠 PARCIAL | Sección 4, tabla mapeo fila 7 | "/CU/Brief/Guardar (POST JSON)" | **FALTA**: Auto-crear Propuesta (análisis: Frame.aspx.vb líneas 356-365, método `SavePropuesta()`) |
| Marcar viabilidad OK/NO | `/CU/Brief/MarcarViabilidad` (POST) | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo filas 8-9 | "/CU/Brief/Viabilidad (POST JSON)" | Paridad 1:1 |
| Cargar documentos | `/CU/Brief/Documentos/{id}` (GET) | ❌ NO | 📝 TODO (P1) | Sección 4, tabla mapeo fila 10 | Reporte: "Sustituir UC_LoadFiles.ascx" | **P1-03**: Requiere FileUploadComponent (análisis: componente nuevo) |

**Resumen Frame.aspx**: 2.5/4 funcionalidades OK (62%)  
**Crítico**: Auto-creación de Propuesta faltante

#### 📌 Detalle: Auto-creación de Propuesta

**Evidencia en Análisis**:
- Sección 3, Flujo 2, Paso 5: "Guardar nuevo Brief + crear Propuesta"
- Sección 4, tabla mapeo fila 7: "Crea Brief, auto-genera Propuesta con valores default, actualiza `TempData["JobBookContext"]`"
- Código legacy: `Frame.aspx.vb` líneas 356-365 → método `SavePropuesta()` crea propuesta con estado "Creada"

**Estado Implementación**: ❌ NO IMPLEMENTADO  
**Impacto**: Flujo de negocio roto (usuario debe crear propuesta manualmente)

**Acción Requerida**:
```csharp
// En BriefService.GuardarBrief(BriefViewModel model)
// Después de guardar Brief:
if (model.Id == 0) // Si es nuevo Brief
{
    var propuesta = new CU_Propuestas
    {
        Brief = briefGuardado.Id,
        Titulo = briefGuardado.Titulo,
        EstadoId = 1, // Creada
        TipoId = 1, // Valor default
        // ... otros campos con defaults
    };
    await _propuestaService.CrearPropuestaDesdeBreif(propuesta);
}
```

---

### 3️⃣ PROPUESTAS.ASPX → PropuestasController

| Funcionalidad (Análisis) | Ruta MVC (Análisis) | Implementado | Estado | Evidencia Análisis | Evidencia Reporte | Nota |
|---------------------------|---------------------|--------------|--------|-------------------|-------------------|------|
| Listar propuestas | `/CU/Propuestas` (GET) | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 11 | "/CU/Propuestas (GET Index con filtro)" | Paridad 1:1 |
| Filtrar por estado | `/CU/Propuestas?estadoId={id}` | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 12 | Implementado en Index | QueryString OK |
| Guardar propuesta | `/CU/Propuestas/Guardar` (POST) | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 13 | "/CU/Propuestas/Guardar (POST JSON)" | Validaciones por estado OK |
| Modal editar | `/CU/Propuestas/Editar/{id}` (GET) | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 14 | "/CU/Propuestas/Editar/{id} (GET modal)" | Paridad 1:1 |
| Eliminar propuesta | `/CU/Propuestas/Eliminar/{id}` (POST) | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 15 | "/CU/Propuestas/Eliminar/{id} (POST JSON)" | Análisis menciona agregar modal confirmación (legacy no lo tiene) |
| Ver detalles + observaciones | `/CU/Propuestas/Detalles/{id}` (GET) | ⚠️ PARCIAL | 🟠 DIFERENCIA | Sección 4, tabla mapeo fila 16 | Reporte no menciona esta ruta | **VERIFICAR**: ¿Se implementó en `/Observaciones/{id}`? |
| Agregar observación | `/CU/Propuestas/AgregarObservacion` (POST) | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 17 | "/CU/Propuestas/AgregarObservacion (POST JSON)" | Paridad 1:1 |
| Ir a estudios | `/CU/Estudios?idPropuesta={id}` | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 18 | Redirección con QueryString | Paridad 1:1 |

**Resumen Propuestas.aspx**: 7/8 funcionalidades OK (87%)  
**Pendiente**: Confirmar si modal Detalles está implementado

---

### 4️⃣ ESTUDIO.ASPX → EstudiosController

| Funcionalidad (Análisis) | Ruta MVC (Análisis) | Implementado | Estado | Evidencia Análisis | Evidencia Reporte | Nota |
|---------------------------|---------------------|--------------|--------|-------------------|-------------------|------|
| Listar estudios | `/CU/Estudios?idPropuesta={id}` (GET) | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 19 | "/CU/Estudios (GET Index por propuesta)" | Paridad 1:1 |
| Modal crear estudio | `/CU/Estudios/Crear` (GET) | ✅ SÍ | 🟠 PARCIAL | Sección 4, tabla mapeo fila 20 | "/CU/Estudios/Crear (GET modal)" | **FALTA**: Validar presupuestos aprobados antes de mostrar modal (análisis: Estudio.aspx líneas 111-149) |
| Guardar estudio | `/CU/Estudios/Guardar` (POST) | ✅ SÍ | 🟠 PARCIAL | Sección 4, tabla mapeo fila 21 | "/CU/Estudios/Guardar (POST JSON)" | **FALTA**: Asignar presupuestos aprobados (análisis: `CU_Presupuestos.DevolverxIdPropuestaAprobados`, `Estudios_Presupuestos.Grabar`) |
| Modal editar estudio | `/CU/Estudios/Editar/{id}` (GET) | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 22 | "/CU/Estudios/Editar/{id} (GET modal)" | Paridad 1:1 |
| Cargar documentos | `/CU/Estudios/Documentos/{id}` (GET) | ❌ NO | 📝 TODO (P1) | Sección 4, tabla mapeo fila 23 | Reporte: "carga de documentos" pendiente | **P1-09**: Requiere FileUploadComponent |

**Resumen Estudio.aspx**: 2/5 funcionalidades OK (40%)  
**Crítico**: Presupuestos aprobados no se asignan

#### 📌 Detalle: Presupuestos Aprobados en Estudios

**Evidencia en Análisis**:
- Sección 3, Sub-Flujo 4.2, Paso 1: `btnNew_Click` valida presupuestos aprobados (líneas 108-149)
  ```vb
  oPresupuesto.DevolverxIdPropuestaAprobados(hfPropuesta.Value)
  If countAprobados = 0 Then
      lblNoAprobados.Text = "No tiene ningún presupuesto aprobado..."
      Return
  End If
  ```
- Sección 3, Sub-Flujo 4.2, Paso 2: Usuario selecciona presupuesto(s) con RadioButton (líneas 151-166, método `ValidateSave`)
- Sección 5, SP tabla fila 11: `CU_Presupuestos.DevolverxIdPropuestaAprobados` retorna lista de presupuestos aprobados
- Sección 5, SP tabla fila 10: `CU_Estudios_Presupuestos.Grabar` asocia presupuesto a estudio

**Estado Implementación**: ❌ NO IMPLEMENTADO  
**Impacto**: No se puede asignar presupuesto aprobado al estudio (datos de negocio incompletos)

**Acción Requerida**:
1. En `EstudiosController.Crear(long idPropuesta)`: llamar `PresupuestoDataAdapter.ObtenerPresupuestosAprobados(idPropuesta)`
2. Pasar lista de presupuestos al modal `_ModalCrear.cshtml`
3. Renderizar RadioButtons/Checkboxes para seleccionar presupuesto(s)
4. En `EstudiosController.Guardar(EstudioViewModel model)`: validar `model.PresupuestosSeleccionados` no esté vacío
5. Guardar relación en `CU_Estudios_Presupuestos` via `EstudioDataAdapter`

---

### 5️⃣ ENTIDADES Y MODELOS

| Entidad/ViewModel (Análisis) | Implementado | Estado | Evidencia Análisis | Evidencia Reporte | Nota |
|------------------------------|--------------|--------|-------------------|-------------------|------|
| `CU_Brief` (entidad EF) | ✅ SÍ | ✅ OK | Sección 5, tabla "Tablas Identificadas" fila 1 | "MatrixNext.Data/Entities/CU_Brief.cs" | 70+ columnas confirmadas |
| `CU_Propuestas` (entidad EF) | ✅ SÍ | ✅ OK | Sección 5, tabla "Tablas Identificadas" fila 2 | "MatrixNext.Data/Entities/CU_Propuestas.cs" | Paridad 1:1 |
| `CU_Estudios` (entidad EF) | ✅ SÍ | ✅ OK | Sección 5, tabla "Tablas Identificadas" fila 3 | "MatrixNext.Data/Entities/CU_Estudios.cs" | Paridad 1:1 |
| `CU_Estudios_Presupuestos` (entidad EF) | ✅ SÍ | ✅ OK | Sección 5, tabla "Tablas Identificadas" fila 4 | "MatrixNext.Data/Entities/CU_Estudios_Presupuestos.cs" | Tabla de asociación N:M |
| `CU_SeguimientoPropuestas` (entidad EF) | ✅ SÍ | ✅ OK | Sección 5, tabla "Tablas Identificadas" fila 5 | "MatrixNext.Data/Entities/CU_SeguimientoPropuestas.cs" | Observaciones OK |
| `CU_Presupuestos` (entidad EF) | ❌ NO | ⚠️ VERIFICAR | Sección 5, tabla "Tablas Identificadas" fila 6 (nota: "Fuera de alcance Fase 1, se usa solo para consultas") | No mencionado en reporte | **VERIFICAR**: ¿Se creó entidad read-only o se usa SP directo? |
| `JobBookSearchViewModel` | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 1 | "JobBookViewModels.cs" | Modelo de búsqueda |
| `JobBookResultViewModel` | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 2 | "JobBookViewModels.cs" | Resultado de búsqueda |
| `BriefViewModel` | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 6 | "BriefViewModels.cs" | 70+ propiedades |
| `PropuestaViewModel` | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 13 | "PropuestaViewModels.cs" | Validaciones por estado |
| `EstudioViewModel` | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 21 | "EstudioViewModels.cs" | Modelo base |
| `ClonarBriefViewModel` | ⚠️ DESCONOCIDO | ⚠️ VERIFICAR | Sección 4, tabla mapeo fila 5 | No mencionado | ¿Se creó para modal clonar? |
| `PropuestaDetalleViewModel` | ⚠️ DESCONOCIDO | ⚠️ VERIFICAR | Sección 4, tabla mapeo fila 16 | No mencionado | ¿Se creó para modal detalles? |
| `ObservacionViewModel` | ⚠️ DESCONOCIDO | ⚠️ VERIFICAR | Sección 4, tabla mapeo fila 16-17 | No mencionado | ¿Se usa en modal observaciones? |

**Resumen Entidades**: 5/6 entidades EF OK (83%)  
**Resumen ViewModels**: 7/13 confirmados (54% - requiere verificación)

---

### 6️⃣ STORED PROCEDURES Y DATA ADAPTERS

| SP (Análisis) | Tecnología Decidida | Implementado | Estado | Evidencia Análisis | Evidencia Reporte | Nota |
|---------------|---------------------|--------------|--------|-------------------|-------------------|------|
| `CU_InfoGeneralJobBook_GET` | SP + Dapper | ✅ SÍ | ✅ OK | Sección 5, SP tabla fila 1 | "busqueda JobBooks via SP" en CuentaDataAdapter | Paridad 1:1 |
| `CU_Brief.Guardar` | EF Core | ✅ SÍ | ✅ OK | Sección 5, SP tabla fila 2 | BriefDataAdapter usa EF | Decisión técnica confirmada |
| `CU_Brief.ObtenerBriefXID` | EF Core | ✅ SÍ | ✅ OK | Sección 5, SP tabla fila 3 | BriefDataAdapter SELECT por PK | Paridad 1:1 |
| `CU_Brief_Clone` | SP + Dapper | ❌ NO | 📝 TODO (P0) | Sección 5, SP tabla fila 4 | "falta SP CloneBrief" | ✅ **CONFIRMADO**: SP existe como `CU_Brief_Clone`, requiere crear `BriefDataAdapter.ClonarBrief()` con Dapper |
| `CU_Propuestas.Guardar` | EF Core | ✅ SÍ | ✅ OK | Sección 5, SP tabla fila 5 | PropuestaDataAdapter usa EF | Decisión técnica confirmada |
| `CU_Propuestas_Get` | SP + Dapper | ✅ SÍ | ✅ OK | Sección 5, SP tabla fila 6 | PropuestaDataAdapter usa SP | JOIN con Brief OK |
| `CU_SeguimientoPropuestas.Guardar` | EF Core | ✅ SÍ | ✅ OK | Sección 5, SP tabla fila 7 | PropuestaDataAdapter INSERT observación | Paridad 1:1 |
| `CU_SeguimientoPropuestas_Get` | SP + Dapper | ⚠️ DESCONOCIDO | ⚠️ VERIFICAR | Sección 5, SP tabla fila 8 | No mencionado | ¿Se usa en modal observaciones? |
| `CU_Estudios.Guardar` | EF Core | ✅ SÍ | ✅ OK | Sección 5, SP tabla fila 9 | EstudioDataAdapter usa EF | Paridad 1:1 |
| `CU_Estudios_Get` | SP + Dapper | ✅ SÍ | ✅ OK | Sección 5, SP tabla fila 10 | EstudioDataAdapter usa SP | JOIN con Propuestas OK |
| `CU_Estudios_Presupuestos.Grabar` | EF Core | ❌ NO | 📝 TODO (P0) | Sección 5, SP tabla fila 11 | "asignacion presupuestos PENDIENTE" | **CRÍTICO**: Requiere implementar |
| `CU_Presupuestos.DevolverxIdPropuestaAprobados` | SP + Dapper | ❌ NO | 📝 TODO (P0) | Sección 5, SP tabla fila 12 | "asignacion presupuestos PENDIENTE" | **CRÍTICO**: Requiere implementar |
| `CU_Presupuestos.ObtenerPresupuestosAsignadosXEstudio` | SP + Dapper | ⚠️ DESCONOCIDO | ⚠️ VERIFICAR | Sección 5, SP tabla fila 13 | No mencionado | ¿Se usa en modal editar estudio? |

**Resumen SP/Adapters**: 8/13 confirmados OK (62%)  
**Crítico**: 2 SP de presupuestos faltan (P0)

---

### 7️⃣ COMPONENTES Y VISTAS

| Componente/Vista (Análisis) | Tipo | Implementado | Estado | Evidencia Análisis | Evidencia Reporte | Nota |
|-----------------------------|------|--------------|--------|-------------------|-------------------|------|
| `_QuillEditor.cshtml` | Existente | ✅ SÍ | ✅ OK | Sección 7, componentes existentes fila 1 | Usado en Brief (4 campos HTML) | Riesgo mitigado |
| `_Modal.cshtml` | Existente | ✅ SÍ | ✅ OK | Sección 7, componentes existentes fila 2 | Modales CRUD | Reutilizado |
| `_DatePicker.cshtml` | Existente | ✅ SÍ | ✅ OK | Sección 7, componentes existentes fila 3 | Campos de fecha | Reutilizado |
| `_Grid.cshtml` | Existente | ✅ SÍ | ✅ OK | Sección 7, componentes existentes fila 4 | Grids de búsqueda/listados | Reutilizado |
| `Dropzone` (componente) | Existente | ⚠️ PARCIAL | 🟠 INTEGRAR (P1) | Sección 7, componentes existentes | "Sustituir UC_LoadFiles.ascx PENDIENTE" | ✅ Componente ya existe, requiere integración en Brief/Estudios (P1-03, P1-09: 4h total vs 6h estimadas) |
| `ValidationHelpersJS` | Nuevo (P0) | ✅ SÍ | ✅ OK | Sección 7, componentes nuevos fila 3 | "validaciones dinamicas de Propuesta por estado" | Implementado (análisis: 4h estimadas) |
| Cuentas/Index.cshtml | Vista | ✅ SÍ | ✅ OK | Sección 4, estructura de carpetas | Reporte: "Views/Cuentas/Index.cshtml" | Paridad 1:1 |
| Cuentas/_ResultadosGrid.cshtml | Partial | ✅ SÍ | ✅ OK | Sección 4, tabla mapeo fila 2 | Reporte: "_ResultadosGrid.cshtml" | Grid AJAX OK |
| Cuentas/_ModalClonar.cshtml | Modal | ⚠️ DESCONOCIDO | ⚠️ VERIFICAR | Sección 4, estructura de carpetas línea 14 | No mencionado en reporte | ¿Se creó? |
| Brief/Index.cshtml | Vista | ✅ SÍ | ✅ OK | Sección 4, estructura de carpetas | Reporte: "Views/Brief/Index.cshtml" | Formulario con QuillEditor OK |
| Brief/_ModalViabilidad.cshtml | Modal | ⚠️ DESCONOCIDO | ⚠️ VERIFICAR | Sección 4, estructura de carpetas línea 18 | No mencionado | ¿Se usa modal o botones inline? |
| Propuestas/Index.cshtml | Vista | ✅ SÍ | ✅ OK | Sección 4, estructura de carpetas | Reporte: "Views/Propuestas/Index.cshtml" | Listado con filtros OK |
| Propuestas/_ModalCrear.cshtml | Modal | ✅ SÍ | ✅ OK | Sección 4, estructura de carpetas | Reporte: "_ModalCrear.cshtml" | Paridad 1:1 |
| Propuestas/_ModalObservaciones.cshtml | Modal | ✅ SÍ | ✅ OK | Sección 4, estructura de carpetas | Reporte: "_ModalObservaciones.cshtml" | Paridad 1:1 |
| Estudios/Index.cshtml | Vista | ✅ SÍ | ✅ OK | Sección 4, estructura de carpetas | Reporte: "Views/Estudios/Index.cshtml" | Listado de estudios OK |
| Estudios/_ModalCrear.cshtml | Modal | ✅ SÍ | ✅ OK | Sección 4, estructura de carpetas | Reporte: "_ModalCrear.cshtml" | **FALTA**: RadioButtons presupuestos |
| Estudios/_ModalPresupuestos.cshtml | Modal | ⚠️ DESCONOCIDO | ⚠️ VERIFICAR | Sección 4, estructura de carpetas línea 27 | No mencionado | ¿Se creó vista separada para presupuestos? |

**Resumen Vistas**: 11/17 confirmadas OK (65%)  
**Pendiente**: Verificar modales y componentes no mencionados

---

## 📝 BACKLOG PRIORIZADO (ACCIONES RECOMENDADAS)

### 🔴 P0: CRÍTICO (Bloquea MVP)

| ID | Tarea | Descripción | Viabilidad | Dependencias | Estimación | Pasos Concretos |
|----|-------|-------------|------------|--------------|------------|-----------------|
| **P0-A1** | **Implementar auto-creación de Propuesta** | Al guardar Brief nuevo, crear Propuesta automáticamente con estado "Creada" | 🟢 ALTA | Ninguna (lógica simple) | 3h | 1. Modificar `BriefService.GuardarBrief()`<br>2. Detectar si `model.Id == 0` (nuevo)<br>3. Llamar `_propuestaService.CrearPropuestaDesdeBreif(briefId)`<br>4. Crear propuesta con valores default (estado=1, tipo=1)<br>5. Actualizar contexto JobBook<br>6. Agregar test unitario |
| **P0-A2** | **Implementar asignación de presupuestos aprobados** | Validar y asignar presupuestos al crear estudio | 🟢 ALTA | Requiere entidad `CU_Presupuestos` (verificar si existe) | 6h | 1. Crear/verificar `PresupuestoDataAdapter` con SP `DevolverxIdPropuestaAprobados`<br>2. Modificar `EstudiosController.Crear(idPropuesta)` para obtener presupuestos aprobados<br>3. Pasar lista a `_ModalCrear.cshtml`<br>4. Agregar RadioButtons/Checkboxes en modal<br>5. En `EstudioService.GuardarEstudio()`, validar presupuestos seleccionados<br>6. Guardar en `CU_Estudios_Presupuestos` con EF<br>7. Agregar validación: "Debe seleccionar al menos 1 presupuesto" |
| **P0-A3** | **Implementar CloneBrief con SP `CU_Brief_Clone`** | Crear DataAdapter, Controller action y modal para clonación | 🟢 ALTA | SP confirmado: `CU_Brief_Clone` | 3h | 1. ✅ SP confirmado: `CU_Brief_Clone`<br>2. Crear `BriefDataAdapter.ClonarBrief(idBrief, idUsuario, idUnidad, nuevoTitulo)` usando Dapper<br>3. Analizar parámetros del SP: `@IdBrief`, `@IdUsuario`, `@IdUnidad`, `@NuevoNombre`<br>4. Implementar `CuentasController.Clonar()` (GET modal + POST ejecución)<br>5. Crear `_ModalClonar.cshtml` con dropdown de unidades + input de título<br>6. Habilitar botón "Duplicar" en grid de resultados |

**Total P0**: **12 horas** (~1.5 días)  
*Reducción: CloneBrief 4h → 3h (SP confirmado)*

---

### 🟠 P1: IMPORTANTE (Mejora funcionalidad)

| ID | Tarea | Descripción | Viabilidad | Dependencias | Estimación | Pasos Concretos |
|----|-------|-------------|------------|--------------|------------|-----------------|
| **P1-A1** | **Integrar componente Dropzone existente** | Integrar componente Dropzone en Brief y Estudios | 🟢 ALTA | Componente Dropzone ya existe en MatrixNext | 4h | 1. ✅ Componente Dropzone ya disponible<br>2. Crear `DocumentoService` con métodos `Listar()`, `Guardar()`, `Eliminar()`<br>3. Implementar `BriefController.Documentos(id)` (GET modal + POST upload)<br>4. Implementar `EstudiosController.Documentos(id)` (GET modal + POST upload)<br>5. Crear modales `_ModalDocumentos.cshtml` reutilizando Dropzone<br>6. Configurar Dropzone para subir a `/CU/Brief/Documentos/{id}` y `/CU/Estudios/Documentos/{id}`<br>7. Agregar botones "Cargar Documentos" en vistas Brief/Index y Estudios/Index |
| **P1-A2** | **Verificar modal Detalles de Propuesta** | Confirmar si está implementado y crear si falta | 🟡 MEDIA | Requiere revisar código | 2h (si existe) / 5h (si falta) | 1. Buscar `PropuestasController.Detalles(id)`<br>2. Si existe: verificar que muestra historial de observaciones<br>3. Si NO existe: crear action + vista<br>4. Usar `SeguimientoService.ObtenerObservaciones(id)`<br>5. Renderizar timeline de observaciones<br>6. Agregar botón "Ver Detalles" en grid |
| **P1-A3** | **Refactorizar Brief en tabs** | Dividir formulario de 70+ campos en 4-5 tabs Bootstrap | 🟢 ALTA | Ninguna | 6h | 1. Crear partials: `_SeccionObjetivos.cshtml`, `_SeccionDiseño.cshtml`, `_SeccionContenido.cshtml`, `_SeccionMetodologia.cshtml`, `_SeccionDI.cshtml`<br>2. Implementar tabs Bootstrap en `Brief/Index.cshtml`<br>3. Mantener validaciones server-side en todas las tabs<br>4. Agregar navegación entre tabs con validación progresiva |
| **P1-A4** | **Implementar EmailService** | Crear servicio de envío de emails con templates Razor | 🟢 ALTA | Configurar SMTP en `appsettings.json` | 8h | 1. Crear `IEmailService` con método `EnviarAsync(destinatario, asunto, template, model)`<br>2. Implementar `EmailService` con `SmtpClient` o `SendGrid`<br>3. Crear templates Razor: `EmailAnuncioEstudio.cshtml`, `EmailJBI.cshtml`<br>4. Integrar en `EstudioService.GuardarEstudio()`<br>5. Configurar cola asíncrona (opcional: Hangfire)<br>6. Agregar logging de emails enviados |
| **P1-A5** | **Agregar paginación server-side** | Implementar paginación en grids de búsqueda/listados | 🟢 ALTA | Ninguna | 6h | 1. Modificar `CuentaDataAdapter.BuscarJobBooks()` para recibir `page`, `pageSize`<br>2. Actualizar SP `CU_InfoGeneralJobBook_GET` con `OFFSET/FETCH` (o paginación manual)<br>3. Repetir para `PropuestaDataAdapter`, `EstudioDataAdapter`<br>4. Implementar componente `_Pagination.cshtml`<br>5. Agregar AJAX para cambio de página sin reload |

**Total P1**: **31 horas** (~3.9 días)  
*Reducción: FileUpload 6h → 4h (componente Dropzone existente)*

---

### 🟡 P2: DESEABLE (Mejoras post-MVP)

| ID | Tarea | Descripción | Viabilidad | Estimación |
|----|-------|-------------|------------|------------|
| **P2-A1** | **Implementar AuditService** | Logging automático de cambios en Brief/Propuesta/Estudio | 🟢 ALTA | 8h |
| **P2-A2** | **Optimización de queries** | Profiling y agregar índices en columnas de búsqueda | 🟢 ALTA | 6h |
| **P2-A3** | **Testing de integración** | Tests automatizados de flujos completos | 🟢 ALTA | 12h |
| **P2-A4** | **MaskedInputComponent** | Componente Razor para inputs con máscara (JobBook, fechas) | 🟢 ALTA | 3h |
| **P2-A5** | **Validar campos obsoletos en Brief** | Consultar con negocio si O1-O7, DI1-DI18 se usan o eliminar | 🟡 MEDIA | 4h |

**Total P2**: **33 horas** (~4.1 días)

---

## 🎯 RESUMEN DE ACCIONES INMEDIATAS

### Para alcanzar MVP funcional (P0)

1. **BriefService** → Agregar auto-creación de Propuesta (3h)
2. **EstudiosController + Service** → Implementar presupuestos aprobados (6h)
3. **CuentasController** → Implementar CloneBrief con SP `CU_Brief_Clone` (3h) ✅ *SP confirmado*

**Total P0**: **12 horas** (~1.5 días de trabajo)  
**Componentes confirmados**: SP `CU_Brief_Clone`, Dropzone existente

### Validaciones requeridas (urgente)

1. ✅ **Verificar si `CU_Presupuestos` existe** como entidad EF o solo se usa vía SP
2. ✅ ~~**Confirmar existencia de SP `CloneBrief`**~~ → **CONFIRMADO**: `CU_Brief_Clone`
3. ✅ **Revisar código de modales** no mencionados en reporte (_ModalClonar, _ModalViabilidad, _ModalPresupuestos)
4. ✅ **Validar ViewModels** no confirmados (ClonarBriefViewModel, ObservacionViewModel, etc.)

### Componentes confirmados disponibles

1. ✅ **SP `CU_Brief_Clone`**: Procedimiento almacenado para clonación de Brief
2. ✅ **Componente Dropzone**: Ya disponible en MatrixNext para carga de archivos

---

## 📊 MÉTRICAS FINALES

### Tareas del Backlog (Análisis Sección 8)

| Prioridad | Planificado (Análisis) | Implementado | Pendiente Ajustado | % Completado |
|-----------|------------------------|--------------|-----------|------------|
| **P0** (18 tareas) | 106h | ~94h | ~12h | 89% |
| **P1** (16 tareas) | 91h | ~0h | ~31h (ajustado) | 0% |
| **P2** (10 tareas) | 55h | ~0h | ~55h | 0% |
| **TOTAL** | 252h | ~94h | ~98h (ajustado) | 49% |

*Ajustes: CloneBrief -1h (SP confirmado), FileUpload -2h (Dropzone existente), Presupuestos +1h estimación conservadora*

### Estado de Migración

| Componente | Completado | En Progreso | Pendiente | Bloqueado |
|------------|------------|-------------|-----------|-----------|
| Controllers | 4/4 (100%) | 0 | 0 | 0 |
| Services | 4/4 (100%) | 0 | 0 | 0 |
| DataAdapters | 4/4 (100%) | 0 | 0 | 0 |
| Rutas/Actions | 18/22 (82%) | 0 | 4 (ready) | 0 |
| Entidades EF | 5/6 (83%) | 0 | 1 | 0 |
| ViewModels | 7/13 (54%) | 0 | 6 | 0 |
| Vistas/Modales | 11/17 (65%) | 0 | 6 | 0 |
| Componentes | 6/10 (60%) | 0 | 4 | 0 |

*Componentes existentes confirmados: QuillEditor, Modal, DatePicker, Grid, ValidationHelpersJS, **Dropzone***

**PARIDAD GLOBAL AJUSTADA**: **~80-85%** 🟢  
**ESTADO**: **MVP CASI COMPLETO** - Requiere 12h de trabajo P0 para completar flujos críticos  
**Desbloqueado**: SP `CU_Brief_Clone` confirmado, componente Dropzone disponible

---

**FIN DE AUDITORÍA**

---

## 📋 LISTA CONSOLIDADA DE TODOs PENDIENTES

### 🔴 P0: CRÍTICO PARA MVP (12 horas - ~1.5 días)

#### TODO-P0-01: Auto-creación de Propuesta al guardar Brief nuevo
**Ubicación**: `MatrixNext.Data/Services/CU/BriefService.cs`  
**Descripción**: Cuando se guarda un Brief nuevo (`Id == 0`), crear automáticamente una Propuesta con estado "Creada"  
**Evidencia**: Análisis Frame.aspx.vb líneas 356-365, método `SavePropuesta()`  
**Estimación**: 3 horas  
**Pasos**:
```csharp
// En BriefService.GuardarBrief(BriefViewModel model)
public async Task<long> GuardarBrief(BriefViewModel model)
{
    var brief = _mapper.Map<CU_Brief>(model);
    bool esNuevo = model.Id == 0;
    
    var idBrief = await _briefDataAdapter.GuardarBrief(brief);
    
    // TODO-P0-01: Auto-crear propuesta si es nuevo Brief
    if (esNuevo)
    {
        var propuesta = new CU_Propuestas
        {
            Brief = idBrief,
            Titulo = brief.Titulo,
            EstadoId = 1, // Creada
            TipoId = 1, // Valor default
            ProbabilidadId = 0.25M, // 25% inicial
            Internacional = false,
            Tracking = true,
            // ... otros campos con valores default
        };
        await _propuestaService.CrearPropuestaDesdeBreif(propuesta);
    }
    
    return idBrief;
}
```

---

#### TODO-P0-02: Asignación de presupuestos aprobados al crear estudio
**Ubicación**: 
- `MatrixNext.Data/Adapters/CU/PresupuestoDataAdapter.cs` (crear si no existe)
- `MatrixNext.Web/Areas/CU/Controllers/EstudiosController.cs`
- `MatrixNext.Web/Areas/CU/Views/Estudios/_ModalCrear.cshtml`
- `MatrixNext.Data/Services/CU/EstudioService.cs`

**Descripción**: Validar presupuestos aprobados antes de mostrar modal crear estudio, permitir selección, y guardar relación en `CU_Estudios_Presupuestos`  
**Evidencia**: Análisis Estudio.aspx líneas 111-149, SP `CU_Presupuestos.DevolverxIdPropuestaAprobados`  
**Estimación**: 6 horas  

**Pasos**:

1. **Crear/verificar PresupuestoDataAdapter** (1h):
```csharp
// MatrixNext.Data/Adapters/CU/PresupuestoDataAdapter.cs
public class PresupuestoDataAdapter
{
    private readonly IDbConnection _db;
    
    public async Task<IEnumerable<PresupuestoAprobadoDto>> ObtenerPresupuestosAprobados(long idPropuesta)
    {
        var sql = "EXEC CU_Presupuestos.DevolverxIdPropuestaAprobados @IdPropuesta";
        return await _db.QueryAsync<PresupuestoAprobadoDto>(sql, new { IdPropuesta = idPropuesta });
    }
}
```

2. **Modificar EstudiosController.Crear** (1h):
```csharp
[HttpGet]
public async Task<IActionResult> Crear(long idPropuesta)
{
    var presupuestos = await _presupuestoDataAdapter.ObtenerPresupuestosAprobados(idPropuesta);
    
    if (!presupuestos.Any())
    {
        return Json(new { success = false, message = "No tiene ningún presupuesto aprobado para crear estudio" });
    }
    
    var model = new CrearEstudioViewModel
    {
        IdPropuesta = idPropuesta,
        PresupuestosAprobados = presupuestos.ToList()
    };
    
    return PartialView("_ModalCrear", model);
}
```

3. **Actualizar vista _ModalCrear.cshtml** (2h):
```html
<!-- Agregar sección de selección de presupuestos -->
<div class="form-group">
    <label>Presupuestos Aprobados (seleccione al menos uno)</label>
    @foreach (var presupuesto in Model.PresupuestosAprobados)
    {
        <div class="form-check">
            <input class="form-check-input" type="checkbox" name="PresupuestosSeleccionados" 
                   value="@presupuesto.Id" id="presupuesto_@presupuesto.Id">
            <label class="form-check-label" for="presupuesto_@presupuesto.Id">
                Alternativa @presupuesto.Alternativa - $@presupuesto.Valor.ToString("N0")
            </label>
        </div>
    }
</div>
```

4. **Modificar EstudioService.GuardarEstudio** (2h):
```csharp
public async Task<long> GuardarEstudio(EstudioViewModel model)
{
    // Validar presupuestos seleccionados
    if (!model.PresupuestosSeleccionados?.Any() ?? true)
    {
        throw new ValidationException("Debe seleccionar al menos un presupuesto aprobado");
    }
    
    var estudio = _mapper.Map<CU_Estudios>(model);
    var idEstudio = await _estudioDataAdapter.GuardarEstudio(estudio);
    
    // TODO-P0-02: Guardar relación con presupuestos
    foreach (var idPresupuesto in model.PresupuestosSeleccionados)
    {
        await _context.CU_Estudios_Presupuestos.AddAsync(new CU_Estudios_Presupuestos
        {
            EstudioId = idEstudio,
            PresupuestoId = idPresupuesto
        });
    }
    await _context.SaveChangesAsync();
    
    return idEstudio;
}
```

---

#### TODO-P0-03: Implementar clonación de Brief con SP CU_Brief_Clone
**Ubicación**:
- `MatrixNext.Data/Adapters/CU/BriefDataAdapter.cs`
- `MatrixNext.Web/Areas/CU/Controllers/CuentasController.cs`
- `MatrixNext.Web/Areas/CU/Views/Cuentas/_ModalClonar.cshtml` (crear)

**Descripción**: Implementar funcionalidad de clonación de Brief a otra unidad usando SP `CU_Brief_Clone`  
**Evidencia**: Default.aspx.vb línea 84-93, SP confirmado como `CU_Brief_Clone`  
**Estimación**: 3 horas  

**Pasos**:

1. **Agregar método en BriefDataAdapter** (1h):
```csharp
// MatrixNext.Data/Adapters/CU/BriefDataAdapter.cs
public async Task<long> ClonarBrief(long idBrief, long idUsuario, int idUnidad, string nuevoTitulo)
{
    using (var connection = _dbContext.Database.GetDbConnection())
    {
        var parameters = new
        {
            IdBrief = idBrief,
            IdUsuario = idUsuario,
            IdUnidad = idUnidad,
            NuevoNombre = nuevoTitulo
        };
        
        // SP retorna el ID del nuevo Brief clonado
        var result = await connection.ExecuteScalarAsync<long>(
            "CU_Brief_Clone", 
            parameters, 
            commandType: CommandType.StoredProcedure
        );
        
        return result;
    }
}
```

2. **Crear actions en CuentasController** (1h):
```csharp
[HttpGet]
public async Task<IActionResult> MostrarModalClonar(long idBrief)
{
    var unidades = await _unidadService.ObtenerTodasLasUnidades();
    var model = new ClonarBriefViewModel
    {
        IdBrief = idBrief,
        Unidades = unidades
    };
    return PartialView("_ModalClonar", model);
}

[HttpPost]
public async Task<IActionResult> Clonar(long idBrief, int idUnidad, string nuevoTitulo)
{
    try
    {
        var idUsuario = long.Parse(User.FindFirst("UserId")?.Value ?? "0");
        var nuevoIdBrief = await _briefDataAdapter.ClonarBrief(idBrief, idUsuario, idUnidad, nuevoTitulo);
        
        return Json(new { success = true, message = "Brief clonado exitosamente", id = nuevoIdBrief });
    }
    catch (Exception ex)
    {
        return Json(new { success = false, message = ex.Message });
    }
}
```

3. **Crear vista _ModalClonar.cshtml** (1h):
```html
@model ClonarBriefViewModel

<div class="modal-header">
    <h5 class="modal-title">Clonar Brief a otra unidad</h5>
    <button type="button" class="close" data-dismiss="modal">&times;</button>
</div>
<div class="modal-body">
    <form id="formClonarBrief">
        <input type="hidden" name="idBrief" value="@Model.IdBrief" />
        
        <div class="form-group">
            <label for="ddlUnidades">Unidad destino</label>
            <select class="form-control" id="ddlUnidades" name="idUnidad" required>
                <option value="">Seleccione...</option>
                @foreach (var unidad in Model.Unidades)
                {
                    <option value="@unidad.Id">@unidad.Nombre</option>
                }
            </select>
        </div>
        
        <div class="form-group">
            <label for="txtNuevoTitulo">Nuevo título</label>
            <input type="text" class="form-control" id="txtNuevoTitulo" name="nuevoTitulo" 
                   maxlength="200" required />
        </div>
    </form>
</div>
<div class="modal-footer">
    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
    <button type="button" class="btn btn-primary" onclick="clonarBrief()">Clonar</button>
</div>

<script>
function clonarBrief() {
    var form = $('#formClonarBrief');
    if (!form[0].checkValidity()) {
        form[0].reportValidity();
        return;
    }
    
    $.post('@Url.Action("Clonar", "Cuentas")', form.serialize())
        .done(function(result) {
            if (result.success) {
                showNotification('success', result.message);
                $('#modalClonar').modal('hide');
                // Opcional: redirigir al Brief clonado
                // window.location.href = '@Url.Action("Index", "Brief")/' + result.id;
            } else {
                showNotification('error', result.message);
            }
        });
}
</script>
```

---

### 🟠 P1: IMPORTANTE (31 horas - ~4 días)

#### TODO-P1-01: Integrar componente Dropzone para carga de documentos
**Ubicación**:
- `MatrixNext.Data/Services/CU/DocumentoService.cs` (crear)
- `MatrixNext.Web/Areas/CU/Controllers/BriefController.cs`
- `MatrixNext.Web/Areas/CU/Controllers/EstudiosController.cs`
- `MatrixNext.Web/Areas/CU/Views/Shared/_ModalDocumentos.cshtml` (crear)

**Descripción**: Integrar componente Dropzone existente para reemplazar `UC_LoadFiles.ascx` en Brief y Estudios  
**Estimación**: 4 horas (reducido de 6h por componente existente)  

**Pasos**:

1. **Crear DocumentoService** (1h):
```csharp
public interface IDocumentoService
{
    Task<IEnumerable<DocumentoDto>> Listar(long contenedorId, byte documentoId);
    Task<long> Guardar(DocumentoUploadDto documento);
    Task Eliminar(long id);
}

public class DocumentoService : IDocumentoService
{
    // Implementar métodos usando tabla de documentos (verificar nombre de tabla)
    // Probablemente: CU_Documentos o tabla compartida de documentos
}
```

2. **Agregar actions en controllers** (1h):
```csharp
// BriefController.cs
[HttpGet]
public async Task<IActionResult> Documentos(long id)
{
    var documentos = await _documentoService.Listar(id, 1); // DocumentoId=1 para Brief
    return PartialView("_ModalDocumentos", new DocumentosViewModel 
    { 
        ContenedorId = id, 
        DocumentoId = 1,
        Documentos = documentos 
    });
}

[HttpPost]
public async Task<IActionResult> SubirDocumento(long id, IFormFile file)
{
    var dto = new DocumentoUploadDto
    {
        ContenedorId = id,
        DocumentoId = 1,
        Archivo = file,
        UsuarioId = long.Parse(User.FindFirst("UserId")?.Value ?? "0")
    };
    
    var idDocumento = await _documentoService.Guardar(dto);
    return Json(new { success = true, id = idDocumento });
}

// EstudiosController.cs - mismo patrón con DocumentoId=2
```

3. **Crear vista _ModalDocumentos.cshtml con Dropzone** (2h):
```html
@model DocumentosViewModel

<div class="modal-header">
    <h5 class="modal-title">Documentos adjuntos</h5>
    <button type="button" class="close" data-dismiss="modal">&times;</button>
</div>
<div class="modal-body">
    <!-- Zona Dropzone -->
    <form action="@Url.Action("SubirDocumento")/@Model.ContenedorId" 
          class="dropzone" id="dropzoneDocumentos">
    </form>
    
    <!-- Listado de documentos existentes -->
    <div class="mt-3">
        <h6>Documentos cargados</h6>
        <table class="table table-sm">
            <tbody>
                @foreach (var doc in Model.Documentos)
                {
                    <tr>
                        <td>@doc.NombreArchivo</td>
                        <td>@doc.FechaCarga.ToString("dd/MM/yyyy")</td>
                        <td>
                            <button class="btn btn-sm btn-danger" 
                                    onclick="eliminarDocumento(@doc.Id)">
                                Eliminar
                            </button>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    </div>
</div>

<script>
// Configurar Dropzone (componente ya existe en MatrixNext)
Dropzone.options.dropzoneDocumentos = {
    paramName: "file",
    maxFilesize: 10, // MB
    acceptedFiles: ".pdf,.doc,.docx,.xls,.xlsx",
    success: function(file, response) {
        if (response.success) {
            showNotification('success', 'Documento cargado exitosamente');
            // Recargar modal o agregar fila a tabla
        }
    }
};
</script>
```

---

#### TODO-P1-02: Verificar e implementar modal Detalles de Propuesta
**Ubicación**: `MatrixNext.Web/Areas/CU/Controllers/PropuestasController.cs`  
**Estimación**: 2-5 horas (depende si existe)  
**Acción**: Buscar `PropuestasController.Detalles(id)` y verificar si muestra historial de observaciones. Si no existe, crear action + vista con timeline de observaciones usando `SeguimientoService.ObtenerObservaciones(id)`.

---

#### TODO-P1-03: Refactorizar formulario Brief en tabs Bootstrap
**Ubicación**: `MatrixNext.Web/Areas/CU/Views/Brief/Index.cshtml`  
**Estimación**: 6 horas  
**Acción**: Dividir formulario de 70+ campos en 4-5 tabs: Objetivos (O1-O7), Diseño (D1-D3), Contenido (C1-C5), Metodología (M1-M3), DI (DI1-DI18). Mantener validaciones server-side y agregar navegación progresiva.

---

#### TODO-P1-04: Implementar EmailService
**Ubicación**: 
- `MatrixNext.Data/Services/CU/EmailService.cs` (crear)
- `MatrixNext.Data/Services/CU/EstudioService.cs` (integrar)

**Estimación**: 8 horas  
**Acción**: Crear `IEmailService` con templates Razor (`EmailAnuncioEstudio.cshtml`, `EmailJBI.cshtml`). Configurar SMTP en `appsettings.json`. Integrar en `EstudioService.GuardarEstudio()` para enviar emails al crear estudio. Considerar cola asíncrona con Hangfire.

---

#### TODO-P1-05: Implementar paginación server-side
**Ubicación**: 
- `MatrixNext.Data/Adapters/CU/CuentaDataAdapter.cs`
- `MatrixNext.Data/Adapters/CU/PropuestaDataAdapter.cs`
- `MatrixNext.Data/Adapters/CU/EstudioDataAdapter.cs`

**Estimación**: 6 horas  
**Acción**: Modificar métodos de búsqueda para recibir `page`, `pageSize`. Actualizar SP con `OFFSET/FETCH`. Crear componente `_Pagination.cshtml` reutilizable. Implementar AJAX para cambio de página sin reload.

---

### 🟡 P2: DESEABLE (33 horas - ~4 días)

#### TODO-P2-01: Implementar AuditService
**Estimación**: 8 horas  
**Acción**: Crear interceptor de EF Core (`SaveChangesInterceptor`) para logging automático de cambios en Brief/Propuesta/Estudio. Registrar usuario, fecha, acción (INSERT/UPDATE/DELETE), valores anteriores y nuevos.

#### TODO-P2-02: Optimización de queries
**Estimación**: 6 horas  
**Acción**: Ejecutar profiling en BD. Agregar índices en columnas de búsqueda: `CU_Brief.Titulo`, `CU_Brief.Cliente`, `CU_Propuestas.JobBook`, `CU_Estudios.JobBook`. Verificar execution plans de SP `CU_InfoGeneralJobBook_GET`.

#### TODO-P2-03: Testing de integración
**Estimación**: 12 horas  
**Acción**: Crear tests automatizados de flujos completos: 1) Crear Brief → Auto-crear Propuesta, 2) Aprobar presupuesto → Crear estudio → Asignar presupuesto, 3) Clonar Brief → Verificar datos. Usar xUnit + Moq.

#### TODO-P2-04: MaskedInputComponent
**Estimación**: 3 horas  
**Acción**: Crear componente Razor con InputMask.js para inputs con máscara: JobBook (`99-999999` o `99-999999-99`), fechas (`dd/MM/yyyy`). Helper: `@Html.MaskedInputFor(m => m.JobBook, "99-999999")`.

#### TODO-P2-05: Validar campos obsoletos en Brief
**Estimación**: 4 horas  
**Acción**: Consultar con Gerente de Cuentas si campos O1-O7, D1-D3, C1-C5, M1-M3, DI1-DI18 se usan actualmente. Considerar eliminar campos obsoletos (requiere migración de datos y actualización de SP).

---

### ⚠️ VALIDACIONES PENDIENTES (Urgentes)

1. **CU_Presupuestos**: Verificar si existe entidad EF o solo se usa vía SP
   - **Acción**: Buscar archivo `CU_Presupuestos.cs` en `MatrixNext.Data/Entities/`
   - **Si NO existe**: Crear entidad read-only para consultas

2. **Modales no confirmados**: Verificar existencia de:
   - `_ModalClonar.cshtml` → **TODO-P0-03** lo crea
   - `_ModalViabilidad.cshtml` → Verificar si viabilidad se marca con modal o botones inline
   - `_ModalPresupuestos.cshtml` → Verificar si se creó vista separada para presupuestos en estudios

3. **ViewModels no confirmados**:
   - `ClonarBriefViewModel` → **TODO-P0-03** lo requiere
   - `ObservacionViewModel` → Verificar si se usa en modal observaciones
   - `PropuestaDetalleViewModel` → **TODO-P1-02** lo requiere

4. **Verificar SP CU_SeguimientoPropuestas_Get**:
   - **Acción**: Confirmar que se usa en modal observaciones para obtener historial
   - **Ubicación**: `PropuestaDataAdapter` (verificar si método existe)

---

### 📦 COMPONENTES Y RECURSOS CONFIRMADOS DISPONIBLES

| Componente | Estado | Ubicación | Uso |
|------------|--------|-----------|-----|
| **SP `CU_Brief_Clone`** | ✅ Confirmado | Base de datos | Clonación de Brief (TODO-P0-03) |
| **Componente Dropzone** | ✅ Disponible | MatrixNext existente | Carga de documentos (TODO-P1-01) |
| **QuillEditor** | ✅ Implementado | `Views/Shared/_QuillEditor.cshtml` | 4 campos HTML en Brief ✅ |
| **_Modal.cshtml** | ✅ Implementado | `Views/Shared/` | Modales CRUD ✅ |
| **_DatePicker.cshtml** | ✅ Implementado | `Views/Shared/` | Campos de fecha ✅ |
| **_Grid.cshtml** | ✅ Implementado | `Views/Shared/` | Grids de búsqueda ✅ |
| **ValidationHelpersJS** | ✅ Implementado | Propuestas | Validaciones dinámicas ✅ |

---

### 📈 PROGRESO Y PRÓXIMOS PASOS

**Estado Actual**: MVP al 89% (94h completadas de 106h)  
**Trabajo Restante P0**: 12 horas (~1.5 días)  
**Bloqueos**: ❌ Ninguno (SP confirmado, componentes disponibles)

**Orden de Ejecución Recomendado**:
1. **TODO-P0-01** (3h) → Auto-creación Propuesta *(flujo de negocio crítico)*
2. **TODO-P0-02** (6h) → Presupuestos en Estudios *(datos de negocio críticos)*
3. **TODO-P0-03** (3h) → CloneBrief *(funcionalidad secundaria P0)*
4. **Validaciones pendientes** (2h) → Verificar entidades/modales/ViewModels
5. **Testing manual** (4h) → Probar flujo completo Default → Brief → Propuesta → Estudio
6. **TODO-P1** (según prioridad de negocio)

**Fecha estimada MVP completo**: **+2 días** desde inicio de trabajo P0

---

## REPORTE ORIGINAL DE IMPLEMENTACIÓN

## Archivos creados/modificados
- MatrixNext.Data/Entities/CU_Brief.cs
- MatrixNext.Data/Entities/CU_Propuestas.cs
- MatrixNext.Data/Entities/CU_Estudios.cs
- MatrixNext.Data/Entities/CU_Estudios_Presupuestos.cs
- MatrixNext.Data/Entities/CU_SeguimientoPropuestas.cs
- MatrixNext.Data/Modules/CU/Models/JobBookViewModels.cs
- MatrixNext.Data/Modules/CU/Models/PropuestaViewModels.cs
- MatrixNext.Data/Modules/CU/Models/EstudioViewModels.cs
- MatrixNext.Data/Modules/CU/Models/BriefViewModels.cs
- MatrixNext.Data/Adapters/CU/CuentaDataAdapter.cs
- MatrixNext.Data/Adapters/CU/PropuestaDataAdapter.cs
- MatrixNext.Data/Adapters/CU/EstudioDataAdapter.cs
- MatrixNext.Data/Adapters/CU/BriefDataAdapter.cs
- MatrixNext.Data/Services/CU/CuentaService.cs
- MatrixNext.Data/Services/CU/PropuestaService.cs
- MatrixNext.Data/Services/CU/EstudioService.cs
- MatrixNext.Data/Services/CU/BriefService.cs
- MatrixNext.Data/Modules/CU/ServiceCollectionExtensions.cs
- MatrixNext.Web/Areas/CU/Controllers/CuentasController.cs
- MatrixNext.Web/Areas/CU/Controllers/PropuestasController.cs
- MatrixNext.Web/Areas/CU/Controllers/EstudiosController.cs
- MatrixNext.Web/Areas/CU/Controllers/BriefController.cs
- MatrixNext.Web/Areas/CU/Views/Cuentas/Index.cshtml, _ResultadosGrid.cshtml
- MatrixNext.Web/Areas/CU/Views/Propuestas/Index.cshtml, _ModalCrear.cshtml, _ModalObservaciones.cshtml
- MatrixNext.Web/Areas/CU/Views/Estudios/Index.cshtml, _ModalCrear.cshtml
- MatrixNext.Web/Areas/CU/Views/Brief/Index.cshtml
- MatrixNext/Areas/CU/TODO_CU_CUENTAS.md
- MatrixNext/MatrixNext.Web/Program.cs
- MatrixNext/MatrixNext.Data/Entities/MatrixDbContext.cs
- MatrixNext/MatrixNext.Web/Areas/CU/Views/_ViewImports.cshtml, _ViewStart.cshtml

## Rutas MVC implementadas
- /CU/Cuentas (GET) Index; /CU/Cuentas/Buscar (POST parcial); /CU/Cuentas/Abrir (GET redireccion segun contexto); /CU/Cuentas/Clonar (POST JSON, pendiente por SP).
- /CU/Propuestas (GET Index con filtro); /CU/Propuestas/Crear (GET modal); /CU/Propuestas/Editar/{id} (GET modal); /CU/Propuestas/Guardar (POST JSON); /CU/Propuestas/Eliminar/{id} (POST JSON); /CU/Propuestas/Observaciones/{id} (GET modal); /CU/Propuestas/AgregarObservacion (POST JSON).
- /CU/Estudios (GET Index por propuesta); /CU/Estudios/Crear (GET modal); /CU/Estudios/Editar/{id} (GET modal); /CU/Estudios/Guardar (POST JSON).
- /CU/Brief (GET Index); /CU/Brief/Guardar (POST JSON); /CU/Brief/Viabilidad (POST JSON).

## Mapeo vs ANALISIS_CU_CUENTAS.md
- Default.aspx → OK: busqueda JobBooks via SP `CU_InfoGeneralJobBook_GET`, grid con Ver; PENDIENTE: clonar brief (falta SP `CloneBrief`).
- Propuestas.aspx → OK: listado y filtro por estado, modal create/edit con validaciones por estado, observaciones; PENDIENTE: integracion presupuestos/alternativas (Fase2), export/logging legacy, uso de JobBook masking avanzado.
- Estudio.aspx → OK: listado por propuesta y modal CRUD basico; PENDIENTE: validacion y asignacion de presupuestos aprobados, carga de documentos, creacion de proyectos PY y correos, cambio de alternativas.
- Frame.aspx → OK: formulario con Quill (4 campos HTML), validaciones base, cambio de viabilidad; PENDIENTE: auto-creacion de propuesta tras guardar brief, carga de archivos, tabs completos de 70+ campos (se exponen O*, D*, C*, M*, DI* pero sin logica extra).

## TODOs pendientes (ver MatrixNext/Areas/CU/TODO_CU_CUENTAS.md)
- Confirmar y mapear SP `CloneBrief` (Default.aspx.vb l.84-93).
- Asignacion de presupuestos aprobados al crear estudio (`CU_Presupuestos.DevolverxIdPropuestaAprobados`, `Estudios_Presupuestos.Grabar`).
- Sustituir `UC_LoadFiles.ascx` para carga de documentos en Brief/Estudio.
- Creacion de proyectos PY y correos asociados al guardar estudios.
- Auto-creacion de propuesta al guardar Brief (Frame.aspx.vb SavePropuesta).

## Pasos para probar manualmente
1. Ejecutar `dotnet build MatrixNext.sln` (ya verificado).
2. Navegar a `/CU/Cuentas`: probar busqueda por titulo/jobbook/id propuesta y navegar con boton “Ver”.
3. Desde resultados, ingresar a `/CU/Brief` y crear/editar brief; verificar validaciones y marcado de viabilidad.
4. Abrir `/CU/Propuestas` (idealmente con contexto de brief) y crear/editar propuestas validando reglas por estado; agregar observaciones.
5. Desde propuestas, abrir `/CU/Estudios?idPropuesta={id}` y crear/editar estudio (validacion basica de fechas y jobbook).
6. Revisar consola del navegador para solicitudes AJAX; confirmar mensajes en modales.

