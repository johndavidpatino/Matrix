# MAPA_DEPENDENCIAS_PY_CORE

**Fase 2: Mapa de Dependencias Detallado** - Validación de Ciclos e Integración

Documento generado: 6 enero 2026
Estatus: 🔄 EN CONSTRUCCIÓN

---

## 📊 Resumen Ejecutivo

Este documento mapea TODAS las dependencias inter-módulo identificadas en la validación de evidencias:
- **PY_Proyectos** → (llamadas a) CORE, CU_Cuentas, OP, US_Usuarios
- **CORE** → PY_Proyectos, CU_Cuentas, US_Usuarios, OP
- Identifica **ciclos potenciales** (riesgos de deadlock)
- Propone **orden de migración** (Phase 0 → Fases 1+)

---

## 1️⃣ MATRIZ DE DEPENDENCIAS DIRECTAS

### 1.1 PY_Proyectos → CORE

| Flujo | Método WebMatrix | Clase/SP | Parámetros | Tipo | Línea |
| --- | --- | --- | --- | --- | --- |
| **Crear trabajo cuanti** | Trabajos.aspx → Guardar() | WorkFlow.CrearHiloCrearTareas() | IdTrabajo, IdProyecto, IdEstudio | SP | 322 |
| **Crear trabajo cuali** | TrabajosCualitativos.aspx → Guardar() | WorkFlow.CrearHiloCrearTareas() | IdTrabajo, IdProyecto | SP | 322 |
| **Registrar tarea creada** | Trabajos/TrabajosCualitativos → Guardar() | LogWorkFlow.CORE_Log_WorkFlow_MasivoEstadoCreada_Add() | IdTrabajo, IdTarea | SP | 333 |
| **Duplicar trabajo** | Trabajos.aspx → gvTrabajos_RowCommand("Duplicar") | Py_TrabajoDuplicar (transactional) | IdTrabajo, NombreTrabajo, UsuarioId | SP | 395-410 |
| **Asignar responsables trabajo** | [Asignaciones → Guardar] | [CORE.AsignacionesTareas] | IdTrabajo, IdTarea, IdUsuario | ⚠️ POR CONFIRMAR | — |

### 1.2 CORE → PY_Proyectos

| Flujo | Método CORE | Clase/SP | Parámetros | Tipo | Riesgo |
| --- | --- | --- | --- | --- | --- |
| **Cambiar estado tarea** | Gestion-Tareas.aspx → btnCambiarEstado_Click() | [SP cambio estado] | IdTarea, NuevoEstado | SP | ⚠️ Actualiza estado trabajo PY? |
| **Devolver tarea** | Gestion-Tareas.aspx → [btnDevolver] | [SP devolver tarea] | IdTarea, IdTrabajo | SP | ⚠️ Puede cambiar estado trabajo PY |
| **Asignación múltiple** | AsignacionTareas.aspx → Guardar() | CORE_WorkFlow_UsuariosAsignados | IdTarea, ListaUsuarios | SP | ⚠️ Afecta trabajo PY |

**⚠️ POTENCIAL CICLO:** PY crea tareas en CORE; CORE cambia estado trabajo en PY → revisar flujo completo.

### 1.3 PY_Proyectos → CU_Cuentas

| Flujo | Método | Clase/SP | Parámetros | Tipo | Línea |
| --- | --- | --- | --- | --- | --- |
| **Obtener Brief proyecto** | PY_Proyectos.aspx → CargarInfoPropuesta() | Propuesta.DevolverxID() | IdPropuesta | Entity | 68-72 |
| **Obtener Estudio** | PY_Proyectos.aspx → gvProyectos_RowCommand("Informacion") | Estudio.ObtenerXID() | IdEstudio | Entity | 71 |
| **Brief contenido** | PY_Proyectos.aspx → CargarFrame() | Brief.ObtenerBriefXID() | IdBrief | Entity | [Implícito] |

**Dirección:** PY → CU (lectura pura, sin escritura)

### 1.4 PY_Proyectos → OP_Operaciones

| Flujo | Método | Clase/SP | Parámetros | Tipo | Línea |
| --- | --- | --- | --- | --- | --- |
| **Obtener metodología OP** | Trabajos.aspx → obtenerOPMetodologia() | MetodologiaOperaciones.obtenerXId() | IdMetodologia | Entity | 135 |
| **Guardar config trabajo** | Trabajos.aspx → Guardar() | TrabajoOPCuanti.GuardarTrabajoConfiguracion() | IdTrabajo, IdMetodologia | Method | 318 |
| **Crear muestra OP** | Trabajos.aspx → Guardar() | CoordinacionCampo.GuardarMuestraXEstudio() | IdEstudio, Muestra | Method | 328 |
| **Estimación automática** | Trabajos.aspx → Guardar() | PlaneacionProduccion.AgregarEstimacionAutomatica() | IdTrabajo | Method | 313 |
| **Estimar tráfico** | Trabajos.aspx → Guardar() | PlaneacionProduccion.GuardarEstimacionInicialOPTrafico() | IdTrabajo, Estimación | Method | 335 |
| **Obtener muestra cuali** | TrabajosCualitativos.aspx → CargarMuestra() | CoordinacionCampo.ObtenerMuestraxEstudioList() | IdEstudio | Method | [Implícito] |
| **Guardar muestra cuali** | TrabajosCualitativos.aspx → Guardar() | CampoCualitativo.GuardarMuestra() | IdTrabajoCuali, Muestra | Method | [Implícito] |
| **Duplicar muestra** | Trabajos.aspx → [Duplicar] | DuplicarMuestra() | IdTrabajo | Method | [Implícito] |

**Dirección:** PY → OP (bidireccional: PY crea trabajo OP, OP actualiza estado)

### 1.5 PY_Proyectos ↔ US_Usuarios

| Flujo | Método | Clase/SP | Parámetros | Tipo | Dirección |
| --- | --- | --- | --- | --- | --- |
| **Verificar permisos** | PY_Proyectos.aspx, Trabajos.aspx, Home.aspx → Page_Load() | Datos.ClsPermisosUsuarios.VerificarPermisoUsuario() | IdPermiso (24, 38, 97), IdUsuario | Method | PY→US |
| **Obtener gerentes proyecto cuali** | TrabajosCualitativos.aspx | Trabajo.ObtenerGerentesProyectoCuali() | IdTrabajoCuali | SP | PY→US |
| **Obtener coordinadores** | TrabajosCualitativos.aspx | Trabajo.ObtenerCoordinadorProyectoCuali() | IdTrabajoCuali | SP | PY→US |
| **Asignar responsable** | AsignacionProyectos.aspx → Guardar() | Proyecto.ActualizarGerente() → PY_Proyectos_EditGerentePY | IdProyecto, IdUsuario | SP | PY→US |

**Dirección:** PY → US (lectura+escritura de asignaciones)

### 1.6 CORE → US_Usuarios

| Flujo | Método | Clase/SP | Parámetros | Tipo | Dirección |
| --- | --- | --- | --- | --- | --- |
| **Obtener usuarios asignados** | Gestion-Tareas.aspx | CORE_WorkFlow_UsuariosAsignados_Get | IdTarea | SP | CORE→US |
| **Asignar tarea a usuario** | AsignacionTareas.aspx → Guardar() | [SP asignación] | IdTarea, IdUsuario | SP | CORE↔US |
| **Validar rol usuario tarea** | [Gestion-Tareas] | [Lógica seguridad] | IdUsuario, IdTarea | [Código] | CORE→US |

---

## 2️⃣ CICLOS POTENCIALES IDENTIFICADOS

### 2.1 Ciclo Sospechoso: PY ↔ CORE (Riesgo 🟠 Medio)

```
FLUJO CREAR TRABAJO CUANTITATIVO:
─────────────────────────────────

1. Usuario abre Trabajos.aspx
2. Llenar datos trabajo, btnGuardar_Click()

3. Trabajos.aspx.vb → Guardar() [línea 289]
   ├─ Trabajo.GuardarTrabajo() → SP PY_Trabajo_Add (crear trabajo)
   └─ WorkFlow.CrearHiloCrearTareas() → SP CORE_* (crear tareas)

4. CORE (workflow generado automáticamente):
   ├─ CORE_WorkFlow inserta nueva tarea
   ├─ CORE_WorkFlow_TareasPrevias valida precedencias
   └─ [¿Actualiza estado trabajo PY?]

5. Trabajos.aspx → lanzarTareas() [línea 322]
   └─ LogWorkFlow.CORE_Log_WorkFlow_MasivoEstadoCreada_Add()

PREGUNTA: ¿PY espera a CORE terminar antes de confirmar? ¿TransactionScope?
POTENCIAL RIESGO: Si CORE cambia estado trabajo PY durante creación → inconsistencia
```

### 2.2 Ciclo Confirmado: PY → CORE (una sola dirección)

**Validación:** WorkFlow.CrearHiloCrearTareas() no retorna estado trabajo; es llamada **fire-and-forget**.
- ✅ Bajo riesgo ciclo directo
- ⚠️ Riesgo integridad si CORE falla sin notificación

### 2.3 Ciclo Sospechoso: Cambio Estado Tarea CORE (Riesgo 🟠 Medio)

```
FLUJO CAMBIO ESTADO TAREA EN CORE:
──────────────────────────────────

1. Usuario en Gestion-Tareas.aspx → Cambiar estado tarea (ejm. Cerrar)
2. SP [cambio estado] actualiza CORE_WorkFlow.Estado = "Cerrado"
3. ¿Qué dispara después?
   ├─ ¿SP trigger que actualiza PY_Trabajo.Estado?
   ├─ ¿Notificación por email a gerente PY?
   ├─ ¿Log en CORE_ObservacionesTareas?
   └─ ¿Cambio estado en tablas relacionadas?

PREGUNTA: ¿Existe SP CORE que llama PY? ¿Transactional?
RIESGO: Si SP no es atómico → inconsistencia trabajo-tareas
```

### 2.4 Ciclo Duplicación: Py_TrabajoDuplicar (Riesgo 🔴 Alta)

```
FLUJO DUPLICAR TRABAJO:
──────────────────────

1. Usuario en Trabajos.aspx → Duplicar trabajo
2. SP Py_TrabajoDuplicar(@IdTrabajo, @NombreTrabajo, @UsuarioId):
   ├─ INSERT PY_Trabajo (clon) → IdTrabajo_Nuevo
   ├─ INSERT PY_Especificaciones (copiar de original)
   ├─ INSERT PY_Variables_Control (copiar)
   ├─ INSERT PY_SegmentosCuali (si aplica)
   ├─ ¿INSERT CORE_WorkFlow (crear tareas duplicadas)?
   └─ ¿INSERT OP_MuestraTrabajos (duplicar muestra)?

PREGUNTA: ¿SP Py_TrabajoDuplicar es atómico? ¿Clona tareas CORE?
RIESGO 🔴: Si SP falla a mitad → trabajo inconsistente con tareas huérfanas
MITIGACIÓN REQUERIDA: TransactionScope con rollback; validar tablas relacionadas
```

---

## 3️⃣ ORDEN RECOMENDADO DE MIGRACIÓN

### Fase 0️⃣ (Infraestructura) - Sin dependencias externas

```
PASO 1: Crear DbContext, Adapters base (NO SP)
  ├─ PY_Entities → EF Core (PY_Proyectos, PY_Trabajo, PY_Variables_Control, etc.)
  ├─ CORE_Entities → EF Core (CORE_Tareas, CORE_WorkFlow, etc.)
  ├─ CU_Entities → EF Core (Brief, Estudio, Propuesta)
  ├─ OP_Entities → EF Core (Metodologías, MuestraTrabajos, etc.)
  └─ US_Entities → EF Core (Permisos, Roles, Usuarios)

PASO 2: Publicar componentes compartidos
  ├─ UploadService (_UploadFrame.cshtml)
  ├─ GridService (_Grid.cshtml paginación)
  └─ PermisosService (VerificarPermisoUsuario)

PASO 3: Algoritmo validación ciclos CORE
  └─ GrafoAciclico.ValidarNoCiclos(List<TareaPrevia>)

Dependencias: NINGUNA EXTERNA
Duración: 1 semana @ 1 dev
```

### Fase 1️⃣ (CORE Catálogos) - Base para PY

```
PASO 1: TareasConfigController (CRUD plantillas tareas)
  ├─ CORE_Tareas
  ├─ CORE_TipoHilos (cached)
  └─ SP: CORE_Tareas_Get

PASO 2: TareasPreviasController (precedencias + ciclos)
  ├─ CORE_WorkFlow_TareasPrevias
  ├─ Llamar: GrafoAciclico.ValidarNoCiclos() ANTES de insertar
  └─ SP: CORE_WorkFlow_TareasPrevias_Get

PASO 3: HilosConfigController (mapeos tareas-hilo)
  └─ CORE_Configuracion_TareasXTipoHilo

PASO 4: DocumentosConfigController (reqs documentos por tarea)
  └─ CORE_Tareas_Documentos

Dependencias: Fase 0
Duración: 2 semanas
BLOQUEA: Fase 2 (PY Maestros)
```

### Fase 2️⃣ (PY Maestros) - Depende de CORE catálogos

```
PASO 1: ProyectosController (CRUD maestro)
  ├─ PY_Proyectos (EF Core)
  └─ SP: PY_Proyectos_Get

PASO 2: TrabajosController (CRUD cuantitativos)
  ├─ PY_Trabajo (EF Core)
  ├─ Llamar: WorkFlow.CrearHiloCrearTareas() en Guardar()
  └─ SP: PY_Trabajos_GET_All, PY_Trabajo_Add/Edit

PASO 3: TrabajosCualiController (CRUD cualitativos)
  ├─ PY_TrabajoCuali (EF Core)
  ├─ CampoCualitativo.GuardarMuestra()
  └─ SP: PY_TrabajosCuali_GET_All, PY_TrabajoCuali_Get

PASO 4: HomeController (dashboard PY)
  └─ Agregaciones de trabajos

Dependencias: Fase 0, Fase 1 (CORE tareas creadas por WorkFlow)
Duración: 3 semanas
BLOQUEA: Fase 3 (operación)
RIESGO 🟠: Integración PY→CORE requiere transactionalidad
```

### Fase 3️⃣ (CORE Operación) - Paralelo con Fase 2

```
PASO 1: AsignacionesController (asignar responsables a tareas)
  ├─ CORE_WorkFlow_UsuariosAsignados (N:N)
  └─ SP: [Asignación]

PASO 2: GestionTareasController (cambios estado + auditoría)
  ├─ CORE_WorkFlow.Estado UPDATE
  ├─ CORE_ObservacionesTareas INSERT (auditoría)
  ├─ VALIDAR: Precedencias (GrafoAciclico.PermiteTransicion)
  └─ SP: [Cambio estado + auditoria]

PASO 3: TraficoController (cola/prioridades)
  ├─ CORE_WorkFlow filtrado por estado
  └─ Actualizar prioridades

Dependencias: Fase 0, Fase 1 (tareas existen), Fase 2 (trabajos existen)
Duración: 2 semanas
RIESGO 🟠: Cambios estado afectan trabajos PY
```

### Fases 4️⃣+ (Soporte)

```
FASE 4: Cuali (SegmentosCuali, Sesiones, InHomeVisit) → 80-120h
FASE 5: Asignaciones/Reasignaciones PY → 60h
FASE 6: Reportes + Documentos → 100h
FASE 7: Testing end-to-end + estabilización → 200h

Total: ~1,200 horas
```

---

## 4️⃣ VALIDACIÓN CICLOS: PREGUNTAS CRÍTICAS

Para resolver riesgos 🟠, necesitamos CONFIRMAR EN LEGACY:

### Pregunta 1: ¿Py_TrabajoDuplicar clona tareas CORE?

```sql
-- VALIDAR EN SQL SERVER:
-- ¿Esta SP tiene esta lógica?

CREATE PROCEDURE Py_TrabajoDuplicar
  @IdTrabajo BIGINT,
  @NombreTrabajo NVARCHAR(MAX),
  @UsuarioId BIGINT
AS
BEGIN TRANSACTION
  -- 1. Clonar trabajo
  INSERT INTO PY_Trabajo (...) SELECT ... FROM PY_Trabajo WHERE Id = @IdTrabajo
  DECLARE @IdTrabajo_Nuevo BIGINT = @@IDENTITY
  
  -- 2. ¿Clonar tareas CORE?
  IF EXISTS (SELECT 1 FROM CORE_WorkFlow WHERE IdTrabajo = @IdTrabajo)
  BEGIN
    INSERT INTO CORE_WorkFlow (IdTrabajo, IdTarea, ...)
    SELECT @IdTrabajo_Nuevo, IdTarea, ... FROM CORE_WorkFlow WHERE IdTrabajo = @IdTrabajo
  END
  
  -- 3. ¿Clonar muestra OP?
  IF EXISTS (SELECT 1 FROM OP_MuestraTrabajos WHERE IdTrabajo = @IdTrabajo)
  BEGIN
    INSERT INTO OP_MuestraTrabajos (...)
    SELECT ... FROM OP_MuestraTrabajos WHERE IdTrabajo = @IdTrabajo
  END
  
COMMIT TRANSACTION
RETURN @IdTrabajo_Nuevo
```

**Acción:** Revisar SQL en BD legacy → documentar exactamente qué clona.

### Pregunta 2: ¿Cambio estado CORE dispara trigger en PY?

```sql
-- VALIDAR EN SQL SERVER:
-- ¿Existe trigger en CORE_WorkFlow.Estado?

CREATE TRIGGER trg_CORE_WorkFlow_CambioEstado
ON CORE_WorkFlow
AFTER UPDATE
AS
BEGIN
  -- ¿Actualiza PY_Trabajo.Estado?
  IF UPDATE(Estado)
  BEGIN
    UPDATE PY_Trabajo
    SET Estado = NEW_Estado
    WHERE Id IN (SELECT DISTINCT IdTrabajo FROM inserted)
  END
END
```

**Acción:** Buscar triggers en BD legacy → documentar cadena de actualización.

### Pregunta 3: ¿WorkFlow.CrearHiloCrearTareas() es síncrono o asíncrono?

**Validación en WebMatrix:**
- Si async → necesita callback/evento cuando CORE termina
- Si sync → Trabajos.aspx.vb espera a que tareas se creen
- Si fire-and-forget → posible inconsistencia

**Acción:** Revisar Trabajos.aspx.vb línea 322 → ¿Qué ejecuta después de CrearHiloCrearTareas()?

---

## 5️⃣ MATRIZ DE DEPENDENCIAS POR COMPONENTE

### 5.1 PY.ProyectosController (nueva)

**Dependencias INTERNAS (en PY):**
- ProyectoService
- Proyecto.vb (CoreProject)

**Dependencias EXTERNAS:**
- CU_Cuentas (lectura Brief/Estudio)
- US_Usuarios (lectura permisos)
- UploadService (compartido)

**Bloqueadores:**
- ✅ Ninguno (CORE no requerido para CRUD básico)

### 5.2 PY.TrabajosController (nueva)

**Dependencias INTERNAS:**
- TrabajoService
- Trabajo.vb (CoreProject)

**Dependencias EXTERNAS:**
- CU_Cuentas (lectura Propuesta/Estudio)
- OP_Operaciones (metodologías, muestras)
- CORE (crear tareas via WorkFlow)
- US_Usuarios (permisos + asignaciones)
- UploadService (compartido)

**Bloqueadores:**
- 🔴 CORE.Tareas debe estar migrado PRIMERO (Fase 1)
- 🔴 Validar transactionalidad PY→CORE

### 5.3 CORE.TareasPreviasController (nueva)

**Dependencias INTERNAS:**
- TareasPreviasService
- CORE.WorkFlow_TareasPrevias (Entity)
- GrafoAciclico (validación ciclos)

**Dependencias EXTERNAS:**
- CORE.TareasConfigController (debe existir primero)
- US_Usuarios (permisos)

**Bloqueadores:**
- ✅ Ninguno (CORE es independiente)

### 5.4 CORE.GestionTareasController (nueva)

**Dependencias INTERNAS:**
- GestionTareasService
- CORE.WorkFlow (Entity)
- CORE.ObservacionesTareas (auditoría)
- GrafoAciclico (validar precedencias)

**Dependencias EXTERNAS:**
- PY_Proyectos (¿cambio estado trabajo?) 🟠
- US_Usuarios (permisos + responsables)
- Email (notificaciones)

**Bloqueadores:**
- 🟠 Necesita confirmar si cambio estado CORE → cambio estado PY
- ⚠️ Si usa SP trigger → requiere validación en DB

---

## 6️⃣ CONCLUSIONES Y RECOMENDACIONES

### 6.1 Ciclos Confirmados SIN RIESGO ✅

- ✅ **PY → CORE (crear tareas):** No hay retorno; es **unidireccional**
- ✅ **PY → CU_Cuentas (lectura):** No hay escritura; es **lectura pura**
- ✅ **PY → OP (crear trabajo):** Bidireccional pero **sin ciclo directo**

### 6.2 Ciclos PENDIENTES DE VALIDACIÓN 🟠

- 🟠 **CORE → PY (cambio estado):** ¿Trigger en BD? ¿Transactional?
- 🟠 **Py_TrabajoDuplicar (clonación):** ¿Qué tablas se clonan? ¿Atómico?
- 🟠 **Asignaciones PY ↔ CORE:** ¿Sincronización automática?

### 6.3 Orden de Migración RECOMENDADO

1. **Fase 0:** Infraestructura (DI, DbContext, componentes)
2. **Fase 1:** CORE Catálogos (Tareas config, Precedencias, Hilos)
3. **Fase 2:** PY Maestros (Proyectos, Trabajos) + integración WorkFlow
4. **Fase 3:** CORE Operación (Asignaciones, Cambios estado, Auditoría)
5. **Fases 4+:** Soporte, Reportes, Testing

### 6.4 Próximas Acciones

- [ ] **VALIDAR SP LEGACY** (Py_TrabajoDuplicar, cambio estado, triggers)
- [ ] **CONFIRMAR flujo WorkFlow** (síncrono vs asíncrono)
- [ ] **DISEÑAR transacción PY-CORE** (incluir en Service layer)
- [ ] **DOCUMENTAR permisos** (exactamente qué rol accede a qué)

---

**Fase 2 completada.** Listo para Fase 3: Matriz de Permisos/Roles.
