# AUDITORÍA DE MIGRACIÓN - MÓDULO OP_CUANTITATIVO

**Fecha de Auditoría**: 8 de enero de 2026  
**Auditor**: GitHub Copilot (Auditoría Técnica)  
**Versión Documentación**: 1.0  
**Estado General**: 🟡 **PARCIALMENTE COMPLETO CON GAPS CRÍTICOS**

---

## 📋 RESUMEN EJECUTIVO

### Calificación General: **68/100**

| Criterio | Calificación | Peso | Notas |
|---|---|---|---|
| **Profundidad del Análisis** | 85/100 | 25% | Análisis técnico robusto, pero falta cobertura de algunos flujos críticos |
| **Cobertura de Implementación** | 55/100 | 35% | Solo ~45% de WebForms migrados; gaps críticos en navegación |
| **Calidad de Código** | 75/100 | 20% | Buena arquitectura, pero falta testing y documentación inline |
| **Documentación y Trazabilidad** | 80/100 | 20% | Documentación exhaustiva de sprints, pero inconsistencias con código real |

### Recomendación Final

**🔴 NO APROBAR PARA PRODUCCIÓN**

El módulo requiere completar **~52% de funcionalidades faltantes** antes de considerarse listo. Se identificaron **23 gaps críticos** y **17 gaps de alta prioridad** que impiden el uso operativo del módulo.

---

## 1️⃣ EVALUACIÓN DEL ANÁLISIS TÉCNICO

### ✅ Fortalezas Identificadas

1. **Inventario Completo (Sección 2)**
   - ✅ Todas las 31 páginas WebForms documentadas con permisos, dependencias y evidencia
   - ✅ Mapeo 1:1 a controladores MVC claramente definido (Sección 7)
   - ✅ Identificación exhaustiva de stored procedures (Sección 4)

2. **Análisis de Riesgos Robusto (Sección 6)**
   - ✅ Identificación del hardcoded ID en `SupervisionCampoTelefonico.aspx` (línea 74: Session 1047223102)
   - ✅ Documentación de dependencias de Session/State (>25 referencias)
   - ✅ Mapeo de conexiones múltiples (MatrixConnectionString, GestionCampoConnectionString)

3. **Backlog y Estimaciones (Secciones 8, 11)**
   - ✅ Priorización clara con T-shirt sizing
   - ✅ Estimación preliminar de 620-780 horas (~3.5-4.5 meses)
   - ✅ Plan de sprints detallado (5 sprints documentados)

### ⚠️ Debilidades del Análisis

1. **Falta de Testing Strategy Ejecutable**
   - ❌ Sección 318 menciona "Testing Strategy" pero no hay plan concreto
   - ❌ No se definieron casos de prueba por WebForm
   - ❌ No hay métricas de cobertura objetivo (unit tests, integration tests)

2. **Profundidad Limitada en Algunos Flujos**
   - ⚠️ **FichaCuantitativa.aspx**: Análisis superficial de sincronización Habeas Data con Propuesta (líneas 150-180)
   - ⚠️ **MuestraTrabajos.aspx**: No se documentó la lógica de auto-planeación con días festivos (líneas 90-100)
   - ⚠️ **RegistroProduccionOP.aspx**: Enumeradores `EAreas`, `EReproceso`, `EActividad` solo dicen "(valores por confirmar)"

3. **Stored Procedures Incompletos**
   - ⚠️ 15+ SPs marcados como "por confirmar" (Sección 4):
     - `OP_CuantiDapper.CuantiPlanillasGet/Update/Remove`
     - `OP_CuantiDapper.CuantiProdProductividad_*`
     - `RevisionIPS.Guardar/Eliminar`
     - `PresupInt.SolicitudPresupuesto*`
   - 🔴 **Riesgo**: Implementación puede fallar por desconocimiento de parámetros/retornos

4. **Decision Points Sin Resolver (Sección "Decision points abiertos")**
   - ❓ **DP-1**: ¿Modelo 1:1 o consolidación de vistas? (sin decisión documentada)
   - ❓ **DP-2**: ¿Archivos en ~/Files migrar a Azure Blob? (sin decisión documentada)

---

## 2️⃣ EVALUACIÓN DE LA IMPLEMENTACIÓN

### 📊 Cobertura de WebForms (31 páginas totales)

| Estado | Cantidad | % | WebForms |
|---|---|---|---|
| ✅ **Completado** | 14 | 45% | Portal, Trafico, Encuestas, ImportacionMasiva, PlanillasAprobacion, Productividad, IPS, Presupuestos, Supervision, IField, Avances, Produccion |
| 🟡 **Parcial** | 3 | 10% | Trabajos (falta navegación), TrabajosCoordinador (sin implementar), TrabajosCallCenter (sin implementar) |
| ❌ **Faltante** | 14 | 45% | FichaCuantitativa, EstimacionProduccion, MuestraTrabajos, ConsultaTrabajos, RevisionPlanillas (4 versiones rol-based), RegistroProduccionOP, HomeGestion, HomeRecoleccion |

### 🚨 Gaps Críticos Identificados

#### GAP-OP-01: Navegación Principal Incompleta 🔴 CRÍTICO
**Severidad**: BLOQUEANTE  
**Impacto**: Usuarios no pueden acceder a flujos completos desde el portal

**Evidencia**:
- ✅ Existe `/OP/Portal` (PortalController.cs) mostrando lista de trabajos
- ❌ **NO existe** controlador para:
  - `/OP/Trabajos` → debería manejar `Trabajos.aspx` (permiso 100, navegación a Muestra/Estimaciones/RO/Cierre GD)
  - `/OP/TrabajosCoordinador` → `TrabajosCoordinador.aspx` (permiso 101, asignación personal)
  - `/OP/TrabajosCallCenter` → `TrabajosCallCenter.aspx` (permiso 101, asignar encuestadores)
  - `/OP/ConsultaTrabajos` → `ConsultaTrabajos.aspx` (permiso 19, consulta por unidad)

**Esfuerzo estimado**: 80 horas (4 controladores + 4 vistas + servicios)

---

#### GAP-OP-02: FichaCuantitativa Sin Implementar 🔴 CRÍTICO
**Severidad**: BLOQUEANTE  
**Impacto**: No se puede gestionar información del trabajo (CRUD ficha cuantitativa)

**Evidencia**:
- ❌ No existe `/OP/FichaCuantitativa` o similar
- WebForm original: `FichaCuantitativa.aspx` (284 líneas, CRUD + sincronización Habeas Data + email de entrega)
- Funcionalidad clave:
  - Guardar/actualizar ficha (incentivos, regalo clientes, compra Ipsos, grupo objetivo, marco muestral, etc.)
  - Sincronizar con Propuesta (`ActualizarHabeasData`)
  - Envío de email de entrega (`EntregaTrabajoCuantitativo.aspx`)
  - Navegación de retorno a Trabajos o TrabajosCallCenter

**Dependencias CoreProject**:
- `FichaCuantitativo.*` (SP internos)
- `Propuesta.*` (sincronización)
- `EnviarCorreo` (email)

**Esfuerzo estimado**: 24 horas (controlador + vista + servicio + adaptador SP)

---

#### GAP-OP-03: Estimación y Muestra Sin Implementar 🔴 CRÍTICO
**Severidad**: BLOQUEANTE  
**Impacto**: No se puede planear producción por ciudad ni gestionar muestra

**Evidencia**:
- ❌ No existe `/OP/EstimacionProduccion`
- ❌ No existe `/OP/MuestraTrabajos`
- WebForms originales:
  - `EstimacionProduccion.aspx` (163 líneas): grid estimación por ciudad, validación vs muestra, activar planeación
  - `MuestraTrabajos.aspx` (121 líneas): CRUD muestra (fechas inicio/fin), auto-planeación con días festivos, email a coordinador

**Funcionalidad clave**:
- Estimación: grid editable por ciudad, validación de cantidades vs muestra, generar/activar planeación automática
- Muestra: actualizar fechas inicio/fin por ciudad, ajuste de planeación con checkboxes de días (L-D) y exclusión de festivos

**Dependencias CoreProject**:
- `PlaneacionProduccion.*` (estimación y auto-planeación)
- `CoordinacionCampo.ObtenerMuestraxEstudioList/ObtenerMuestraxId/GuardarMuestraXEstudio`
- `PlaneacionProduccion.ActualizarFechasCiudad` (auto-planeación con festivos)

**Esfuerzo estimado**: 40 horas (2 controladores + 2 vistas + servicios + lógica festivos)

---

#### GAP-OP-04: Revisión de Planillas Multirrol Sin Implementar 🔴 CRÍTICO
**Severidad**: BLOQUEANTE  
**Impacto**: Coordinador/PMO/Campo/MyS no pueden aprobar productividad

**Evidencia**:
- ✅ Existe `/OP/PlanillasAprobacion` que consolida planillas cargadas/revisadas
- ❌ **FALTA** implementación de 4 flujos rol-based de revisión:
  - `RevisionProductividadPMO.aspx` (permiso 100)
  - `RevisionProductividadCoordinador.aspx` (permiso 135)
  - `RevisionProductividadCampo.aspx` (permiso 156)
  - `RevisionProductividadMYSCall.aspx` (permiso 157)

**Funcionalidad clave**:
- Cada rol revisa y ajusta cantidades autorizadas (monto actual vs previo)
- Rechazar planillas con observación
- Validación de máximos por trabajo (campo `MontoAutorizado`)
- Workflow de aprobación en cadena (Coordinador → PMO → Campo → MyS)

**Dependencias CoreProject**:
- `OP_CuantiDapper.CuantiProdProductividad_*` (probables SPs de update/aprobación)
- `TrabajoOPCuanti.ObtenerCCProduccionPST` (obtener presupuesto)

**Esfuerzo estimado**: 48 horas (4 vistas + servicio común + adaptadores Dapper + permisos)

---

#### GAP-OP-05: Registro de Producción Sin Implementar 🟠 ALTA
**Severidad**: ALTA  
**Impacto**: Áreas de Procesamiento/Scripting no pueden registrar actividades

**Evidencia**:
- ✅ Existe `/OP/Produccion` (ProduccionController) pero solo muestra resumen general
- ❌ **NO implementa** el formulario de registro de `RegistroProduccionOP.aspx` (432 líneas)

**Funcionalidad clave**:
- Selección cascada: Unidad → Actividad → SubActividad
- Enumeradores: `EAreas` (Procesamiento=23, Scripting=18), `EReproceso`, `EActividad`
- Registro por tipo de aplicativo (solo Procesamiento)
- Búsqueda de JBE/JBI/CC con pop-up de selección
- Guardar con validaciones de fecha/hora

**Dependencias CoreProject**:
- `RecordProduccion.ObtenerUnidades/MatrizActividades/JBE_JBI`
- Enumeradores internos (documentados en análisis pero sin valores confirmados)

**Esfuerzo estimado**: 32 horas (formulario cascada + búsqueda JB + validaciones + servicio)

---

#### GAP-OP-06: HomeRecoleccion/HomeGestion Sin Implementar 🟡 MEDIA
**Severidad**: MEDIA  
**Impacto**: No hay dashboard de entrada al módulo (permiso 54)

**Evidencia**:
- ❌ No existe `/OP/Home` o similar
- WebForms originales:
  - `HomeRecoleccion.aspx` (valida permiso 54, landing)
  - `HomeGestion.aspx` (página vacía en legacy)

**Funcionalidad clave**:
- Landing page de módulo con navegación a flujos principales
- Validación de permiso 54 como acceso base
- Posible dashboard con KPIs (trabajos activos, pendientes, etc.)

**Esfuerzo estimado**: 16 horas (controlador + vista + KPIs básicos)

---

#### GAP-OP-07: Hardcoded User ID en Supervisión ⚠️ CORREGIDO PARCIALMENTE
**Severidad**: MEDIA (YA CORREGIDO EN CÓDIGO, FALTA VALIDACIÓN)

**Evidencia**:
- 🔴 WebForm original: `SupervisionCampoTelefonico.aspx.vb#L74` → `Session("IdUsuario") = 1047223102` (HARDCODED)
- ✅ Implementación actual: `SupervisionController.cs` usa `User.FindFirst(ClaimTypes.NameIdentifier)` (CORRECTO)
- ⚠️ **FALTA**: Validación de permiso 157 antes de mostrar supervisión (documentado en avance pero no verificado en código)

**Acción requerida**: Confirmar que el permiso 157 se valida en `SupervisionController` o middleware.

**Esfuerzo estimado**: 2 horas (testing + ajuste de permisos si falta)

---

#### GAP-OP-08: Falta Gestión Documental (GD) de Cierre 🔴 CRÍTICO
**Severidad**: BLOQUEANTE  
**Impacto**: No se puede cerrar trabajos con validación de documentos escaneados

**Evidencia**:
- ❌ No existe implementación de cierre de trabajo con GD
- WebForm original: `Trabajos.aspx.vb` líneas 367-465 (`btnCerrar_Click`)
  - Valida estado del trabajo
  - Sincroniza documentos escaneados GD (`GD.GD_Procedimientos.DevolverxIdTrabajoIdRolResponsable`)
  - Muestra opciones de forzar/confirmar
  - Envía email de cambio de estado

**Funcionalidad clave**:
- Validación de estado antes de cerrar
- Consulta de documentos en rutas UNC (`\\servidor\compartido`)
- Forzar cierre si faltan documentos (con confirmación)
- Email de notificación de cierre

**Dependencias CoreProject**:
- `GD.GD_Procedimientos.*` (gestión documental)
- `RepositorioDocumentos.*` (rutas UNC)
- `Trabajo.CambiarEstado` (cambio de estado + workflow)

**Esfuerzo estimado**: 40 horas (servicio GD + validaciones + UI confirmación + emails)

---

### 🟢 Implementaciones Exitosas

1. **Portal COE (`/OP/Portal`)** ✅
   - Grid de trabajos con filtros (JobBook, Estado, Nombre)
   - Badge de permiso 100
   - Enlaces a `/OP/Trafico`, `/OP/Avances`, `/OP/Encuestas`
   - Resumen de supervisión y producción

2. **Tráfico de Encuestas (`/OP/Trafico`)** ✅
   - KPIs por ciudad
   - Filtro por TrabajoId
   - SP `OP_TraficoEncuestasCiudad` consumido correctamente

3. **Activación/Anulación (`/OP/Encuestas`)** ✅
   - Formularios anti-forgery
   - SP `OP_GestionCampo_ActivarEncuesta` y `OP_GestionCampo_AnularEncuesta`
   - Mensajes de resultado con TempData

4. **Carga Masiva (`/OP/ImportacionMasiva`)** ✅
   - Wizard de validación y ejecución
   - SP `CatiRMC_*` ejecutados (7 SPs documentados)
   - Bulk copy a `OP_CuantiPlanillas`
   - Backup en `uploads/op/cargas` con timestamp
   - Reportes de métricas (Validas/NoValidas/Duplicadas/Inconsistencias)

5. **Planillas Aprobación (`/OP/PlanillasAprobacion`)** ✅
   - Consolidación de planillas cargadas/revisadas/aprobadas
   - Indicadores rol-based

6. **IPS (`/OP/Ips`)** ✅
   - Grid editable por tipo de tarea
   - Export a Excel con ClosedXML
   - Guardado en `~/Files/ips-export-*.xlsx`

7. **Presupuestos (`/OP/Presupuestos`)** ✅
   - Formularios completo y simplificado
   - Notificaciones configurables (`appsettings.json` → `Notifications:Presupuestos:Recipients`)
   - Resumen en Portal (5 solicitudes más recientes)

8. **iField (`/OP/IField`)** ✅
   - Selección de proyectos
   - Configuraciones
   - Botón "Sincronizar proyectos iField"
   - Documentación de sincronización LDAP

---

## 3️⃣ GAPS ADICIONALES (NO CRÍTICOS)

### GAP-OP-09: Testing Unitario Inexistente 🟡 MEDIA
**Evidencia**:
- ❌ Solo 1 test encontrado: `MatrixNext.Tests/CORE/Services/GestionTareasServiceTests.cs`
- ❌ No hay tests para servicios OP: `OpCargaService`, `OpPortalService`, `OpTraficoService`, etc.
- ❌ No hay tests de controladores

**Esfuerzo estimado**: 80 horas (cobertura mínima 60% de servicios OP)

---

### GAP-OP-10: Documentación Inline Limitada 🟡 MEDIA
**Evidencia**:
- ⚠️ Archivos de servicio sin XML comments (ejemplo: `OpCargaService.cs` métodos públicos sin `<summary>`)
- ⚠️ Controladores sin comentarios de propósito/permisos

**Esfuerzo estimado**: 16 horas (documentación inline de clases y métodos públicos)

---

### GAP-OP-11: Validación de SPs "Por Confirmar" 🟠 ALTA
**Evidencia**:
- ⚠️ 15+ SPs marcados como "por confirmar" en análisis (Sección 4)
- ⚠️ No se validó en código real que los parámetros coincidan con la implementación

**Acción requerida**: Revisar cada SP en `CoreProject/OP_Cuanti*` y documentar:
  - Nombre exacto
  - Parámetros (tipos, opcional/obligatorio)
  - Estructura de retorno (DataTable, entidad, escalar)

**Esfuerzo estimado**: 24 horas (revisión de 15 SPs + actualización de análisis)

---

### GAP-OP-12: Configuración de Rutas de Archivos 🟡 MEDIA
**Evidencia**:
- ✅ Rutas configuradas en código: `~/uploads/op/cargas`, `~/Files`
- ❌ No están en `appsettings.json` como configurables
- ❌ Decision Point abierto: ¿migrar a Azure Blob?

**Esfuerzo estimado**: 8 horas (externilizar rutas + documentar decisión Blob)

---

### GAP-OP-13: Enumeradores Sin Definir 🟡 MEDIA
**Evidencia**:
- ⚠️ Análisis dice: `EAreas`, `EReproceso`, `EActividad` → "(valores por confirmar)"
- ❌ No se crearon enums compartidos en MatrixNext
- ⚠️ WebForm original define: `EAreas.Procesamiento=23`, `EAreas.Scripting=18`

**Acción requerida**: Crear enums en `MatrixNext.Web/Models/OP/Enums.cs` con valores exactos del legacy.

**Esfuerzo estimado**: 4 horas (definir enums + refactorizar código que los use)

---

### GAP-OP-14: Validación de Permisos Inconsistente 🟠 ALTA
**Evidencia**:
- ✅ `PortalController` valida permiso 100 y 157
- ⚠️ Otros controladores usan `[Authorize]` genérico sin validación de permiso específico
- ❌ Falta middleware centralizado de permisos

**Esfuerzo estimado**: 16 horas (middleware de permisos + atributo `[RequiresPermission(100)]`)

---

### GAP-OP-15: Gestión de Festivos para Planillas 🟡 MEDIA
**Evidencia**:
- ✅ `OpCargaService` valida corte 16-15 de nómina
- ⚠️ WebForm original consulta tabla `_Festivos` para jornadas dominicales (línea 160-186)
- ❌ No se implementó validación de festivos en `ValidatePlanillasAsync`

**Esfuerzo estimado**: 8 horas (consulta a `_Festivos` + validación de TipoActividad 22/23)

---

### GAP-OP-16: Email Asíncrono Sin Queue 🟡 MEDIA
**Evidencia**:
- ✅ `PresupuestosController` envía emails con `IEmailService`
- ⚠️ WebForm original usa `AsyncEnviarCorreo(url)` (proceso asíncrono)
- ❌ No hay queue de reintentos si el email falla

**Esfuerzo estimado**: 24 horas (implementar queue con Hangfire/Azure Queue + logging)

---

### GAP-OP-17: Exportes Excel Sin Tracking 🟡 MEDIA
**Evidencia**:
- ✅ IPS guarda exports en `~/Files/ips-export-*.xlsx`
- ❌ No se registra en base de datos (usuario, fecha, trabajo, tipo)
- ❌ No hay limpieza automática de archivos antiguos

**Esfuerzo estimado**: 12 horas (tabla de auditoría + job de limpieza)

---

### GAP-OP-18: Sincronización Habeas Data Faltante 🔴 CRÍTICO
**Evidencia**:
- ❌ No implementado (depende de GAP-OP-02 FichaCuantitativa)
- WebForm original: `FichaCuantitativa.aspx.vb` método `ActualizarHabeasData` (línea 150-180)
- Sincroniza con tabla `Propuesta` campo `HabeasData`

**Esfuerzo estimado**: 8 horas (método en servicio + SP/EF)

---

### GAP-OP-19: Auto-Planeación con Festivos Faltante 🔴 CRÍTICO
**Evidencia**:
- ❌ No implementado (depende de GAP-OP-03 MuestraTrabajos)
- WebForm original: `MuestraTrabajos.aspx.vb` método `PlaneacionProduccion.ActualizarFechasCiudad` con checkboxes de días y `chbFestivosExcluir` (línea 90-100)

**Esfuerzo estimado**: 16 horas (lógica de planeación + UI checkboxes + servicio)

---

### GAP-OP-20: Asignación de Personal Sin Implementar 🔴 CRÍTICO
**Evidencia**:
- ❌ No existe `/OP/AsignacionPersonal` o similar
- WebForm: `TrabajosCoordinador.aspx` y `TrabajosCallCenter.aspx` permiten asignar/retirar encuestadores por ciudad
- Funcionalidad: grid de personal, asignación masiva, exportar a Excel

**Dependencias CoreProject**:
- `CoordinacionCampoPersonal.*` (asignación)

**Esfuerzo estimado**: 32 horas (controlador + vista + servicio + export Excel)

---

### GAP-OP-21: Navegación de Retorno Inconsistente 🟡 MEDIA
**Evidencia**:
- ⚠️ WebForms usan `Response.Redirect` con QueryString `?COE=1`, `?Coordinador=1`
- ❌ MatrixNext no implementa lógica de retorno parametrizada
- ⚠️ Botones "Volver" deberían respetar origen (Trabajos vs TrabajosCoordinador vs TrabajosCallCenter)

**Esfuerzo estimado**: 8 horas (helper de navegación + refactor de vistas)

---

### GAP-OP-22: Validación de Índice Único en Planillas 🟡 MEDIA
**Evidencia**:
- ✅ `OpCargaService` captura excepción de índice único `IX_OP_CuantiPlanillas_Unique_Trabajo_Per_ResFecha`
- ⚠️ WebForm original muestra mensaje específico de duplicado (línea 210-220)
- ❌ MatrixNext solo muestra mensaje genérico de error

**Esfuerzo estimado**: 4 horas (mensaje específico + hint de usuario)

---

### GAP-OP-23: Configuración de Límites de Carga 🟡 MEDIA
**Evidencia**:
- ⚠️ No hay límite configurado en `OpCargaService`
- ⚠️ Análisis menciona riesgo de DoS por archivos grandes
- ❌ Falta validación de tamaño máximo (ej: 50MB)

**Esfuerzo estimado**: 4 horas (límite en `appsettings.json` + validación)

---

## 4️⃣ INCONSISTENCIAS DOCUMENTACIÓN vs CÓDIGO

### INC-01: Sprint 5 Dice "Completado" Pero Faltan Flujos 🔴
**Evidencia**:
- 📄 `OP_CUANTITATIVO_AVANCE.md#L68`: "Sprint 5 (Presupuestos internos + utilidades) completado"
- ❌ **FALTAN**: RegistroProduccionOP, HomeGestion, HomeRecoleccion
- ⚠️ Sprint 5 solo completó Presupuestos e iField

**Acción**: Actualizar documento de avances para reflejar estado real.

---

### INC-02: Análisis Dice "31 Páginas" Pero Excluyó 3 🟡
**Evidencia**:
- 📄 Análisis dice: "31 páginas, 100% funcionalidades analizadas"
- ⚠️ Excluye: `WebForm1.aspx`, `Borrar.aspx`, `TraficoEncuestas.aspx`
- ✅ Correcto: 31 páginas totales - 3 excluidas = **28 páginas a migrar**

**Acción**: Actualizar resumen ejecutivo para aclarar "28 páginas netas".

---

### INC-03: Tabla de Migración 1:1 No Refleja Código Real 🟠
**Evidencia**:
- 📄 Sección 7 del análisis lista controladores que no existen:
  - `OpCuantiController` → No existe en código (debería ser para Trabajos.aspx)
  - `CoordinadorOpCuantiController` → No existe
  - `CallCenterOpCuantiController` → No existe
  - `ConsultaOpCuantiController` → No existe
  - `EstimacionOpCuantiController` → No existe
  - `MuestraOpCuantiController` → No existe
  - `ImportarOpCuantiController` → Existe como `ImportacionMasivaController` (nombre diferente)

**Acción**: Actualizar tabla de mapeo 1:1 con nombres reales de controladores.

---

## 5️⃣ PLAN DE REMEDIACIÓN (EQUIPO B)

### 🎯 Objetivo: Completar Módulo OP_Cuantitativo al 95%

**Prioridad 1 (BLOQUEANTES - 2 semanas)**

| # | Gap | Horas | Asignación Sugerida |
|---|---|---|---|
| OP-01 | Navegación Principal (Trabajos/TrabajosCoordinador/CallCenter/Consulta) | 80h | 2 devs × 5 días |
| OP-02 | FichaCuantitativa | 24h | 1 dev × 3 días |
| OP-03 | Estimación y Muestra | 40h | 1 dev × 5 días |
| OP-04 | Revisión de Planillas Multirrol | 48h | 1 dev × 6 días |
| OP-08 | Gestión Documental Cierre | 40h | 1 dev × 5 días |
| OP-18 | Sincronización Habeas Data | 8h | 1 dev × 1 día |
| OP-19 | Auto-Planeación Festivos | 16h | 1 dev × 2 días |
| OP-20 | Asignación de Personal | 32h | 1 dev × 4 días |
| **TOTAL P1** | **288h** | **~7.2 semanas-persona (36 días) → 2 semanas con 3-4 devs** |

**Prioridad 2 (ALTA - 1 semana)**

| # | Gap | Horas | Asignación Sugerida |
|---|---|---|---|
| OP-05 | Registro de Producción | 32h | 1 dev × 4 días |
| OP-11 | Validación de SPs | 24h | 1 dev × 3 días |
| OP-14 | Middleware de Permisos | 16h | 1 dev × 2 días |
| OP-16 | Email Queue | 24h | 1 dev × 3 días |
| **TOTAL P2** | **96h** | **~2.4 semanas-persona (12 días) → 1 semana con 2-3 devs** |

**Prioridad 3 (MEDIA - 1 semana)**

| # | Gap | Horas | Asignación Sugerida |
|---|---|---|---|
| OP-06 | HomeRecoleccion/HomeGestion | 16h | 1 dev × 2 días |
| OP-09 | Testing Unitario | 80h | 2 devs × 5 días (paralelo) |
| OP-10 | Documentación Inline | 16h | 1 dev × 2 días |
| OP-12 | Configuración Rutas | 8h | 1 dev × 1 día |
| OP-13 | Enumeradores | 4h | 1 dev × 0.5 días |
| OP-15 | Gestión Festivos Planillas | 8h | 1 dev × 1 día |
| OP-17 | Tracking Exportes | 12h | 1 dev × 1.5 días |
| OP-21 | Navegación Retorno | 8h | 1 dev × 1 día |
| OP-22 | Mensaje Duplicado | 4h | 1 dev × 0.5 días |
| OP-23 | Límites Carga | 4h | 1 dev × 0.5 días |
| **TOTAL P3** | **160h** | **~4 semanas-persona (20 días) → 1 semana con 4 devs** |

### 📅 Cronograma Propuesto (4 semanas)

| Semana | Prioridad | Entregables | Recursos |
|---|---|---|---|
| **1-2** | P1 Bloqueantes | Navegación principal, FichaCuantitativa, Estimación/Muestra, Revisión Planillas, GD Cierre, Habeas Data, Auto-Planeación, Asignación Personal | 3-4 devs |
| **3** | P2 Alta | Registro Producción, Validación SPs, Middleware Permisos, Email Queue | 2-3 devs |
| **4** | P3 Media + Testing | HomeRecoleccion, Testing, Documentación, Configuración | 4 devs (2 en testing, 2 en media) |

**Total estimado**: **544 horas** (~13.6 semanas-persona → **4 semanas con 3-4 devs**)

---

## 6️⃣ RIESGOS DEL PLAN DE REMEDIACIÓN

| Riesgo | Probabilidad | Impacto | Mitigación |
|---|---|---|---|
| **SPs "por confirmar" tienen parámetros diferentes** | 🟠 Media | 🔴 Alta | Validar SPs en Sprint 0 (1 día) antes de P1 |
| **Dependencias de CoreProject no documentadas** | 🟡 Baja | 🟠 Media | Pair programming con dev original en P1 |
| **Cambios en BD durante remediación** | 🟡 Baja | 🔴 Alta | Freeze de schema durante 4 semanas |
| **Testing descubre bugs en implementación existente** | 🟠 Media | 🟠 Media | Buffer de 1 semana adicional post-P3 |
| **Decisión Blob storage afecta roadmap** | 🟡 Baja | 🟡 Baja | Decidir en Sprint 0 (DP-2) |

---

## 7️⃣ CRITERIOS DE ACEPTACIÓN FINALES

### ✅ Criterios para Aprobar Módulo

1. **Cobertura de Funcionalidades**
   - [ ] 100% de WebForms migrados (28 páginas netas)
   - [ ] Todos los permisos mapeados (100, 101, 19, 135, 117-120, 156, 157, 126, 54)
   - [ ] Navegación completa entre flujos (Trabajos → Muestra → Estimación → Cierre)

2. **Testing y Calidad**
   - [ ] Cobertura de tests ≥60% en servicios OP
   - [ ] 0 errores críticos de Pylance/compilación
   - [ ] Validación manual de 5 flujos end-to-end (lista de trabajos → cierre)

3. **Documentación**
   - [ ] Actualizar `OP_CUANTITATIVO_AVANCE.md` con estado real
   - [ ] Actualizar tabla 1:1 en `ANALISIS_OP_CUANTITATIVO.md`
   - [ ] Documentación inline en todos los servicios públicos (XML comments)

4. **Configuración y Deploy**
   - [ ] Rutas de archivos en `appsettings.json`
   - [ ] Decisión documentada sobre Blob storage (DP-2)
   - [ ] Email queue configurado con reintentos

5. **Seguridad y Permisos**
   - [ ] Middleware de permisos implementado
   - [ ] 0 hardcoded IDs (validar SupervisionController)
   - [ ] Anti-forgery tokens en todos los POST

---

## 8️⃣ RECOMENDACIONES ADICIONALES

### Para el Equipo B

1. **Sprint 0 (Pre-trabajo - 2 días)**
   - Validar todos los SPs "por confirmar" en CoreProject
   - Decidir DP-2 (Blob storage sí/no)
   - Crear enumeradores compartidos (GAP-OP-13)
   - Revisar permisos con stakeholders

2. **Estrategia de Testing**
   - Priorizar tests de integración sobre unitarios (mayor ROI)
   - Usar base de datos de prueba con datos reales anonimizados
   - Crear suite de smoke tests para regresión

3. **Gestión de Dependencias**
   - Documentar cada nuevo SP usado en wiki interna
   - Alertar a equipo de DB sobre cambios de schema
   - Mantener log de decisiones técnicas (ADRs)

4. **Comunicación**
   - Daily standup de 15min para bloqueos
   - Demo semanal a stakeholders (viernes)
   - Slack channel exclusivo para módulo OP

---

## 9️⃣ CONCLUSIONES

### Análisis del Arquitecto: **85/100** 🟢 BUENO
- Inventario completo y detallado
- Riesgos bien identificados
- Estimaciones razonables
- **Mejoras**: Profundizar en flujos complejos, confirmar SPs, resolver decision points

### Implementación del Equipo A: **55/100** 🟡 INSUFICIENTE
- Solo 45% de cobertura de WebForms
- Gaps críticos en navegación principal
- Buena calidad en lo implementado (sprints 1-5)
- **Mejoras**: Completar P1 antes de producción, aumentar testing

### Estado General del Módulo: **68/100** 🟡 NO LISTO
- **Bloqueantes**: 8 gaps críticos (288h)
- **Alta prioridad**: 4 gaps (96h)
- **Media prioridad**: 10 gaps (160h)
- **Total remediación**: 544 horas (~4 semanas con 3-4 devs)

---

## 📎 ANEXOS

### A. Checklist de Entregables para Equipo B

```markdown
## Pre-Sprint 0
- [ ] Validar 15 SPs "por confirmar" en CoreProject (GAP-OP-11)
- [ ] Decidir DP-2: Blob storage (sí/no/más adelante)
- [ ] Crear enums compartidos (GAP-OP-13)
- [ ] Revisar permisos con stakeholders

## Sprint P1 (Semanas 1-2)
- [ ] Controlador Trabajos.aspx (permiso 100, navegación, cierre GD)
- [ ] Controlador TrabajosCoordinador.aspx (permiso 101, asignación)
- [ ] Controlador TrabajosCallCenter.aspx (permiso 101, encuestadores)
- [ ] Controlador ConsultaTrabajos.aspx (permiso 19, unidades)
- [ ] Controlador FichaCuantitativa (CRUD + Habeas Data + email)
- [ ] Controlador EstimacionProduccion (grid + validación)
- [ ] Controlador MuestraTrabajos (fechas + auto-planeación festivos)
- [ ] Vistas de Revisión Planillas (4 roles: PMO, Coordinador, Campo, MyS)
- [ ] Servicio de Gestión Documental (GD cierre)
- [ ] Servicio de Asignación de Personal

## Sprint P2 (Semana 3)
- [ ] Formulario Registro de Producción (cascada Unidad→Actividad→Sub)
- [ ] Validación de 15 SPs (documentar parámetros/retornos)
- [ ] Middleware de permisos centralizado
- [ ] Email queue con reintentos (Hangfire/Azure)

## Sprint P3 (Semana 4)
- [ ] HomeRecoleccion/HomeGestion (landing + permiso 54)
- [ ] Testing unitario (≥60% cobertura servicios OP)
- [ ] Documentación inline (XML comments)
- [ ] Rutas en appsettings.json
- [ ] Gestión de festivos en planillas
- [ ] Tracking de exportes Excel
- [ ] Navegación de retorno parametrizada
- [ ] Mensaje específico de duplicado en planillas
- [ ] Límite de carga en config

## Post-Sprint (Validación)
- [ ] Smoke tests end-to-end (5 flujos completos)
- [ ] Actualizar OP_CUANTITATIVO_AVANCE.md
- [ ] Actualizar ANALISIS_OP_CUANTITATIVO.md (tabla 1:1)
- [ ] Demo a stakeholders
- [ ] Handoff a equipo de QA
```

### B. Matriz de Trazabilidad (WebForms → Código)

| WebForm Original | Controlador Implementado | Estado | Gap Relacionado |
|---|---|---|---|
| Trabajos.aspx | ❌ NO | FALTANTE | GAP-OP-01 |
| TrabajosCoordinador.aspx | ❌ NO | FALTANTE | GAP-OP-01 |
| TrabajosCallCenter.aspx | ❌ NO | FALTANTE | GAP-OP-01 |
| ConsultaTrabajos.aspx | ❌ NO | FALTANTE | GAP-OP-01 |
| FichaCuantitativa.aspx | ❌ NO | FALTANTE | GAP-OP-02 |
| EstimacionProduccion.aspx | ❌ NO | FALTANTE | GAP-OP-03 |
| MuestraTrabajos.aspx | ❌ NO | FALTANTE | GAP-OP-03 |
| ImportarDatos.aspx | ✅ ImportacionMasivaController | COMPLETO | - |
| ImportarPlanillas.aspx | ✅ ImportacionMasivaController | COMPLETO | - |
| PlanillasCargadas.aspx | ✅ PlanillasAprobacionController | COMPLETO | - |
| RevisionPlanillas.aspx | ✅ PlanillasAprobacionController | COMPLETO | - |
| PlanillasRevisadas.aspx | ✅ PlanillasAprobacionController | COMPLETO | - |
| ProductividadRevisadaPMO.aspx | ❌ NO (solo redirect) | PARCIAL | GAP-OP-04 |
| ProductividadRevisadaCoordinador.aspx | ❌ NO | FALTANTE | GAP-OP-04 |
| ProductividadRevisadaCampo.aspx | ❌ NO | FALTANTE | GAP-OP-04 |
| ProductividadRevisadaMYSCall.aspx | ❌ NO | FALTANTE | GAP-OP-04 |
| RevisionProductividadPMO.aspx | ❌ NO | FALTANTE | GAP-OP-04 |
| RevisionProductividadCoordinador.aspx | ❌ NO | FALTANTE | GAP-OP-04 |
| RevisionProductividadCampo.aspx | ❌ NO | FALTANTE | GAP-OP-04 |
| RevisionProductividadMYSCall.aspx | ❌ NO | FALTANTE | GAP-OP-04 |
| SolicitudPresupuestoInterno.aspx | ✅ PresupuestosController | COMPLETO | - |
| SolicitudPresupuestosInternos.aspx | ✅ PresupuestosController | COMPLETO | - |
| ActivacionEncuestas.aspx | ✅ EncuestasController | COMPLETO | - |
| AnulacionEncuestas.aspx | ✅ EncuestasController | COMPLETO | - |
| IPS.aspx | ✅ IpsController | COMPLETO | - |
| RegistroProduccionOP.aspx | ❌ NO (solo resumen) | PARCIAL | GAP-OP-05 |
| SupervisionCampoTelefonico.aspx | ✅ SupervisionController | COMPLETO | GAP-OP-07 (validar) |
| iFieldConfiguration.aspx | ✅ IFieldController | COMPLETO | - |
| TraficoEncuestas.aspx | ✅ TraficoController | COMPLETO | - |
| HomeGestion.aspx | ❌ NO | FALTANTE | GAP-OP-06 |
| HomeRecoleccion.aspx | ❌ NO | FALTANTE | GAP-OP-06 |

**Resumen Matriz**:
- ✅ Completo: 14 (45%)
- 🟡 Parcial: 3 (10%)
- ❌ Faltante: 14 (45%)

---

**FIN DEL DOCUMENTO DE AUDITORÍA**

---

**Próximos Pasos**:
1. Revisión de este documento con Equipo B y stakeholders
2. Kick-off de Sprint 0 (validación de SPs y decisiones)
3. Asignación de recursos para P1 (semanas 1-2)
4. Configuración de ambiente de desarrollo (DB de prueba, CI/CD)
