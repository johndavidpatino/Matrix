# Mapa de Módulos para Migración WebMatrix → MatrixNext

⚠️ **IMPORTANTE**: Antes de migrar cualquier módulo, leer [DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md) que contiene las 15 reglas obligatorias para garantizar consistencia y calidad.

---

## ⚠️ CLASIFICACIÓN DE MÓDULOS - EVITAR TRABAJO DUPLICADO

> **CRÍTICO**: Este documento clasifica módulos en 4 categorías para evitar que el equipo de desarrollo itere sobre módulos ya completados:
> 
> - ✅ **COMPLETADOS (100%)**: NO TOCAR - Migración finalizada en Sprints 1-11
> - 🔍 **EN REVISIÓN/QA**: Solo verificar completitud, QA funcional, ajustes menores
> - 🚧 **PENDIENTES MIGRACIÓN**: Trabajo completo por iniciar (Sprints 12-19)
> - ⛔ **EXCLUIDOS**: NO migrar por decisión de negocio

**Dashboard Ejecutivo**: Ver [DASHBOARD_MIGRACION.md](MatrixNext/docs/GENERAL/DASHBOARD_MIGRACION.md) para métricas actualizadas.

---

## ✅ MÓDULOS COMPLETADOS (NO TOCAR - Sprints 1-11)

### 🔴 CRÍTICA (Ya Migrados 100%)

#### 1. **US_Usuarios** (14 páginas) ✅ SPRINT 1 COMPLETADO
- **Carpeta**: `WebMatrix/US_Usuarios/`
- **Contexto**: `US_Model` (CoreProject)
- **Páginas**:
  - Usuarios.aspx
  - CambioContrasena.aspx
  - Roles.aspx (x2)
  - Permisos.aspx
  - GrupoUnidad.aspx
  - GruposPermisos.aspx
  - RolesPermisos.aspx
  - RolesUsuarios.aspx
  - TipoGrupoUnidad.aspx
  - UsuariosUnidades.aspx
  - Feedback.aspx
  - SeguimientoFeedback.aspx
- **Dependencias**: Bajo (infraestructura solo)
- **Status**: ✅ COMPLETADO
- **Componentes migrados**:
  - CRUD usuarios completo
  - Cambio de contraseña
  - Asignaciones (Roles/Unidades/Permisos)
  - 14 páginas migradas
- **LOC migradas**: ~800 líneas

#### 2. **Home** (3 páginas)
- **Carpeta**: `WebMatrix/Home/`
- **Contexto**: `CORE_Model` + múltiples
- **Páginas**:
  - Home.aspx (dashboard principal)
  - Default.aspx (ALT: puede estar en raíz)
  - DefaultOLD.aspx
- **Status**: En progreso (Sprint 9) con controller HomeController, dashboard widgets y servicio de agregación ejecutándose desde `MatrixNext.Web`.
- **Evidencia MatrixNext**: `MatrixNext/MatrixNext.Web/Controllers/HomeController.cs`, `MatrixNext/MatrixNext.Web/Views/Home/Index.cshtml`, `MatrixNext/MatrixNext.Web/Services/Dashboard/DashboardService.cs` (carga tareas, proyectos, quotes, ausencias, documentos y métricas).
- **Dependencias**: ALTA (consume datos de múltiples módulos)
- **Status**: 🔄 DESPUÉS de US_Usuarios

---

### 🟠 ALTA (Prioritario)

#### 3. **PY_Proyectos** (18 páginas)
- **Carpeta**: `WebMatrix/PY_Proyectos/`
- **Contexto**: `PY_Model` (CoreProject)
- **Dependencias**: Medias (referencia Usuarios, Metodologías)
- **Volumen**: Grande pero bien estructurado
- **Status**: Código en marcha dentro del área `Areas/PY`, con controladores para proyectos, segmentación, sesiones y asignaciones en producción parcial.
- **Evidencia MatrixNext**: `MatrixNext/MatrixNext.Web/Areas/PY/Controllers` + `MatrixNext/MatrixNext.Web/Areas/PY/Views` conservan la estructura MVC y los documentos `MatrixNext/docs/PY/MIGRACION_PY_PROYECTOS.md`.

#### 4. **OP_Cuantitativo** (31 páginas)
- **Carpeta**: `WebMatrix/OP_Cuantitativo/`
- **Contexto**: `OP_Cuanti_Model` (CoreProject)
- **Páginas**: 31 páginas analizadas (excluye: Borrar.aspx, TraficoEncuestas.aspx)
- **Dependencias**: Altas (métodos, cálculos, variables)
- **Status**: ✅ ANÁLISIS COMPLETO
- **Documento**: [ANALISIS_OP_CUANTITATIVO.md](MatrixNext/docs/OP/ANALISIS_OP_CUANTITATIVO.md) (v1.1)
- **Estimación**: 330-435h (1:1) o 260-350h (optimizado)
- **Timeline**: 11-15 semanas (1:1) o 9-12 semanas (optimizado)
- **Status actualizado**: Estructura base lista en `MatrixNext.Web/Areas/OP`; controladores como `FichaCuantitativaController` y `ReportesController` ya existen y el backlog es de ajustes y QA por sprint.
- **Evidencia MatrixNext**: `MatrixNext/MatrixNext.Web/Areas/OP/Controllers/FichaCuantitativaController.cs`, `MatrixNext/MatrixNext.Web/Areas/OP/Views/FichaCuantitativa`, `MatrixNext/docs/OP/ANALISIS_OP_CUANTITATIVO.md`.
- **Riesgos identificados**: 14 (Session hardcoded, OleDb incompatibilidad, GridView complejidad)
- **Optimizaciones propuestas**: 7 consolidaciones (31→18 vistas, -42% código, 0% funcionalidad perdida)
- **Backlog**: 10 épicas definidas con t-shirt sizing
- **Próximo paso**: Decisión stakeholder sobre enfoque (1:1 vs optimizado vs híbrido)

#### 5. **OP_Cualitativo** (múltiples)
#### 5. **OP_Cualitativo** (múltiples) ✅ SPRINT 6 COMPLETADO
- **Carpeta**: `WebMatrix/OP_Cualitativo/`
- **Contexto**: `OP_Entities` (CoreProject)
- **Dependencias**: Altas (entrevistas, moderadores, sesiones)
- **Status Actual**: ✅ **SPRINT 6 COMPLETADO (100%)** - Sprint 6 entregó 6 fases:
  * Fase 1: Transcription (234 LOC)
  * Fase 2: Scheduling (456 LOC)  
  * Fase 3: Sample Management (312 LOC)
  * Fase 4: Calendar/Gantt (518 LOC)
  * Fase 5: Email/Notifications (703 LOC - IOpNotificacionService, OpNotificacionService, OpReminderBackgroundService)
  * Fase 6: Bulk Import (1,074 LOC - Excel/CSV validation, template generation, import history tracking)
- **LOC Total Sprint 6**: 3,297 líneas en 12 commits
- **Componentes Clave Entregados**:
  * `IOpNotificacionService` (39 LOC) - Interface con 6 métodos de notificación
  * `OpNotificacionService` (572 LOC) - Implementación con 6 templates HTML inline + Dapper queries
  * `OpReminderBackgroundService` (103 LOC) - Background service ejecutando cada 6 horas
  * `IOpBulkImportService` (34 LOC) - Interface para importación bulk
  * `OpBulkImportService` (365 LOC) - Validación Excel/CSV, Dapper insert, historial
  * Controllers: `CualitativoProgramacionController` (notificaciones), `CualitativoMuestraController` (bulk import)
  * Views: `_BulkImportModal.cshtml` (169 LOC), `_ImportHistorial.cshtml` (51 LOC), Index.cshtml actualizado
- **Evidencia MatrixNext**: `MatrixNext/MatrixNext.Web/Areas/OP/Controllers/`, `MatrixNext/MatrixNext.Web/Services/OP/`, `MatrixNext/MatrixNext.Web/Areas/OP/Views/CualitativoMuestra/`

#### 6. **FI_AdministrativoFinanciero + CC_FinzOpe** (28 págs FI + infraestructura CC)
- **Carpeta**: `WebMatrix/FI_AdministrativoFinanciero/` + `CoreProject/CC_FinzOpe`
- **Contexto**: `FI_Model` + `CC_FinzOpe` + `CAP` (CoreProject)
- **Dependencias**: ✅ CU_Cuentas (jobbooks), US_Usuarios, TH; 📋 CAP (costos)
- **Volumen**: Muy grande
- **Estado**: 🔄 En curso (CC_FinzOpe completado; FI Grupo 1-3 migrados: Control Presupuestos, Presupuestos Internos, Procesos Internos; análisis COMPLETO)
- **Evidencia MatrixNext**: `MatrixNext/MatrixNext.Web/Areas/CC/Controllers` y `MatrixNext/MatrixNext.Web/Areas/CC/Views` contienen las pantallas para Control Presupuestos, Presupuestos Internos y Procesos Internos; documentación complementaria en `MatrixNext/docs/FI_CC/`.
- **Documento**: [MIGRACION_FI_ADMINISTRATIVO.md](MIGRACION_FI_ADMINISTRATIVO.md)
- **Cambio CRÍTICO**: ✅ **CC_FinzOpe migra en Sprint Pre-1 como infraestructura (80h)**
  - Razón: CC_FinzOpe no es módulo independiente; es data core que FI consume vía SP
  - Sprint Pre-1 (80h): Migrar tablas, SP, DbContext wrapper
  - Sprints 1-6 (704h): FI Grupos 1-6 sobre CC_FinzOpe ya migrado
- **Alcance FI**: Excluye compras/OC/OS, radicación/aprobación de facturas
- **Grupos FI definidos** (✅ 100% COMPLETADOS):
  - Grupo 1: Control Presupuestos (4 páginas) - 92h ✅
  - Grupo 2: Presupuestos Internos (4 páginas) - 68h ✅
  - Grupo 3: Procesos Internos (6 páginas) - 132h ✅
  - Grupo 4: Reportes (4 páginas) - 72h ✅
  - Grupo 5: Producción (9 páginas) - 232h ✅
- **Esfuerzo completado**: 80 (CC Pre-1) + 596 (FI Grupos 1-5) = **676 horas** ✅
- **Total original**: 80 (CC Pre-1) + 612 (FI) + 92 (buffer) = 784 horas
- **Status FI**: 📊 **100% COMPLETO (5/5 grupos migrados, 28/29 páginas)**
- **Recomendación**: FI/CC está listo para producción. Evaluar siguientes módulos (TH, PY, OP)

---

### 🟡 MEDIA (Estratégica)

#### 7. **GD_Documentos**
- **Contexto**: `GD_Model`
- **Dependencias**: Medias
- **Status**: Controladores para documentos, repositorio y aprobaciones ya disponibles en MatrixNext; vistas asociadas generadas en el área `Areas/GD`.
- **Evidencia MatrixNext**: `MatrixNext/MatrixNext.Web/Areas/GD/Controllers/DocumentosMaestroController.cs`, `RepositorioController.cs`, `SolicitudesController.cs` y `MatrixNext/MatrixNext.Web/Areas/GD/Views/DocumentosMaestro`.

#### 8. **RP_Reportes**
- **Contexto**: `REP_Model`
- **Notas**: Consultas complejas → ideal para Dapper
- **Status**: Controlador `ReportesController` con los endpoints de generación/visualización ya funciona y provee vistas `Index/Generar/Detalle`.
- **Evidencia MatrixNext**: `MatrixNext/MatrixNext.Web/Areas/RP/Controllers/ReportesController.cs` + `MatrixNext/MatrixNext.Web/Areas/RP/Views/Reportes`.

#### 9. **TH_TalentoHumano** (28 páginas) ✅ SPRINT 4 COMPLETADO
- **Carpeta**: `WebMatrix/TH_TalentoHumano/`
- **Contexto**: `TH_Model` (CoreProject)
- **Dependencias**: Medias (empleados, usuarios, catálogos)
- **Estado**: 
  - ✅ **Ausencias** (4 páginas) - COMPLETADO
  - ✅ **Empleados API REST** (21 archivos, 2,750+ LOC) - SPRINT 4 COMPLETO
    * 6 Adapters (ThEmpleadosAdapter, ThExperienciaLaboralAdapter, ThEducacionAdapter, ThDatosComplementariosAdapter, ThDesvinculacionAdapter, ThCatalogosAdapter)
    * 3 Services (ThEmpleadosService, ThDesvinculacionService, ThCatalogosService)
    * 3 Controllers (EmpleadosController 37 endpoints, DesvinculacionesController 5 endpoints, CatalogosController 13 endpoints)
    * 30+ DTOs con Input/Output segregación
    * DI Registration (9 AddScoped en Program.cs)
    * 55 endpoints con [Authorize], ApiResponse<T>, ILogger<T>, validaciones
    * 35+ SPs documentados y consumidos
    * 0 errores de compilación
    * Documentación: INVENTARIO_MAPEO_TH + CIERRE_SPRINT_4
  - 🔄 **Nómina** (Views/UI - Sprint 5)
  - 🔄 **Otros** (pendiente)

**Evidencia MatrixNext**: `MatrixNext/MatrixNext.Web/Areas/TH/Controllers/AusenciasController.cs`, `DesvinculacionesController.cs`, `EmpleadosController.cs`, `GestionAusenciaController.cs` y las vistas bajo `MatrixNext/MatrixNext.Web/Areas/TH/Views` (Ausencias, GestionAusencia, Empleados).

**Análisis Detallado - GESTIÓN DE AUSENCIAS (4 páginas - Prioridad Alta dentro del módulo)**

##### A. **SolicitudAusencia.aspx** (Solicitud de Ausencias)
- **Funcionalidad**: Empleado solicita vacaciones, permisos, licencias sin remuneración
- **Vistas/Paneles**:
  1. "Nueva solicitud" - Formulario para crear ausencia
  2. "Historial" - Listado de solicitudes histórico
  3. "Beneficios pendientes" - Grid de días de vacación/permisos sin usar
  4. "Solicitudes por aprobar" - Si el usuario es aprobador
  5. "Ausencias del equipo" (link a página separada)
- **Lógica**:
  - Carga tipos de solicitud desde `TH_Ausencia.DAL.TiposSolicitudesAusencia`
  - Cálculo automático de días calendario vs. días laborales (considera si sábado es día laboral según tipo de salario)
  - Validación de rango de fechas y solapamiento de solicitudes previas
  - Transición de estado: 1 (Radicada) → 5 (Pendiente Aprobación) → 20 (Aprobada) / 10 (Rechazada)
  - Envío de emails de notificación tras crear solicitud
- **Componentes**:
  - DropDownList: TipoSolicitud, Aprobador
  - TextBox: FechaInicio, FechaFin, DiasCalendario (R/O), DiasLaborales (R/O), Observaciones
  - GridView: gvHistorialAusencia, gvBeneficiosPendientes, gvAprobacionesPendientes
  - Panel condicional: pnlIncapacidad (visible si rol RRHH)
- **Data Access**:
  - Tabla: `TH_SolicitudAusencia` (idEmpleado, FInicio, FFin, Tipo, Estado, DiasCalendario, DiasLaborales, ObservacionesSolicitud, AprobadoPor, FechaAprobacion, VoBo1, FechaVoBo1)
  - Tabla: `TH_Ausencia_Incapacidades` (para tipo incapacidad)
  - Procedimientos: `TH_Ausencia.RegistrosAusencia`, `TH_Ausencia.CalculoDias`, `TH_Ausencia.ValidarSolicitudAusencia`, `TH_Ausencia.CausarVacaciones`
- **Complejidad**: ⭐⭐⭐ Media

##### B. **SolicitudAusenciaIncapacidades.aspx** (Solicitud de Incapacidades)
- **Funcionalidad**: Empleado solicita ausencia por incapacidad médica (enfermedad, accidente)
- **Vistas/Paneles**:
  1. "Nueva solicitud" - Formulario para incapacidad
  2. "Historial" - Listado de incapacidades registradas
  3. "Beneficios pendientes" - Incapacidades sin procesar
  4. "Solicitudes por aprobar" - Para aprobadores (RRHH/Coordinador)
- **Lógica**:
  - Similar a SolicitudAusencia pero con campos adicionales:
    - EntidadConsulta (EPS/IPS)
    - NoRegistroMedico
    - TipoIncapacidad (enfermedad general, accidente trabajo, maternidad, etc.)
    - ClaseAusencia
    - SOAT (si aplica)
    - FechaAccidenteTrabajo (si aplica)
    - DXAsociado (diagnóstico)
    - CIE (código diagnóstico)
    - Comentarios
  - Validaciones: puede requerir documento PDF/imagen con la incapacidad original
  - Estados: 1 (Radicada) → 5 (Pendiente Aprobación) → 20 (Aprobada) / 10 (Rechazada)
- **Componentes**:
  - DropDownLists: TipoSolicitud, Aprobador, EntidadConsulta, TipoIncapacidad
  - TextBoxes: FechaInicio, FechaFin, NoRegistroMedico, DXAsociado, CIE, Comentarios
  - FileUpload: Para documento de incapacidad
  - GridViews: gvHistorialAusencia, gvBeneficiosPendientes, gvAprobacionesPendientes
- **Data Access**:
  - Tabla: `TH_Ausencia_Incapacidades` (campos mencionados arriba)
  - Procedimientos: Similar a SolicitudAusencia
- **Complejidad**: ⭐⭐⭐ Media-Alta (incluye file upload)

##### C. **GestionAusenciaRRHH.aspx** (Gestión por RRHH)
- **Funcionalidad**: Personal de RRHH aprueba/rechaza solicitudes, genera reportes
- **Vistas/Paneles/Acordeones**:
  1. "Aprobaciones" - Grid de solicitudes pendientes de aprobación
     - Acciones: Aprobar (→ estado 20), Rechazar (→ estado 10)
     - Filtro por tipo de solicitud
  2. "Vacaciones" - Reporte de vacaciones (días disfrutados vs. pendientes)
     - Generación en Excel (ClosedXML)
     - Columnas: Identificacion, NombreEmpleado, AreaSL, FechaIngreso, DiasDisfrutados, DiasPendientes, UltimoPeriodoCausado, Observaciones, Estado
  3. "Beneficios" - Reporte de otros beneficios (permisos, licencias)
     - Excel con datos filtrados por año
  4. "Ausentismo" - Reporte de ausentismo general
     - Datos: Identificacion, NombreEmpleado, AreaSL, TipoAusentismo, FInicio, FFin, DiasCalendario, DiasLaborales, Estado
  5. "Incapacidades" - Reporte detallado de incapacidades
     - Datos: Identificacion, NombreEmpleado, AreaSL, FechaIngreso, EntidadConsulta, IPSPrestadora, NoRegistroMedico, TipoIncapacidad, ClaseAusencia, SOAT, FechaAccidenteTrabajo, Comentarios, DXAsociado, CIE, CategoriaDX, Estado
- **Lógica**:
  - Cargas asincrónicas de grids al cambiar filtros
  - Generación de Excel: títulos, datos en DataTable, descarga al cliente
  - Estados y transiciones: valida cambio de estado, envía correos de notificación
  - Si Tipo = Vacaciones (tipo 1), ejecuta procedimiento `CausarVacaciones` para descontar saldos
- **Componentes**:
  - DropDownLists: TipoSolicitud, Año (para reportes)
  - GridView: gvAprobacionesPendientes (con botones Aprobar/Rechazar)
  - Botones de reporte: btnReporteVacaciones, btnReporteBeneficios, btnReporteAusentismo, btnReporteIncapacidades, btnReporteNomina
  - UpdatePanels: para cargas parciales de grids
- **Data Access**:
  - Procedimientos: `TH_Ausencia.RegistrosAusencia` (filter by estado 5 = pendiente, estado 1 = radicada)
  - Procedimientos de reporte: `TH_Ausencia.ReporteVacaciones`, `TH_Ausencia.ReporteBeneficios`, `TH_Ausencia.ReporteAusentismo`, `TH_Ausencia.ReporteIncapacidades`
  - Genera emails vía `EnviarCorreo` y páginas Emails/EnvioDefinicionAusencia.aspx, Emails/EnvioAprobacionVacaciones.aspx
- **Complejidad**: ⭐⭐⭐⭐ Alta (múltiples paneles, reportes en Excel, flujo de aprobación)

##### D. **AusenciasEquipo.aspx** (Vista del Coordinador/Jefe)
- **Funcionalidad**: Coordinador ve ausencias de su equipo y gestiona dependencias
- **Métodos WebMethod**:
  1. `getAusenciasEquipo(jefeId, fInicio, fFin)` - Calendario de ausencias del equipo en rango
  2. `getBeneficiosPendientes(empleadoId)` - Beneficios sin usar de un empleado
  3. `getAusenciasSubordinados(jefeId)` - Lista de subordinados asignados al jefe
  4. `getAusenciasPersonas(jefeId, search)` - Búsqueda de personas con ausencias
  5. `removeAusenciasSubordinado(subordinadoId)` - Desasignar persona del jefe
  6. `addAusenciasSubordinado(jefeId, empleadoId)` - Asignar persona al jefe
- **Lógica**:
  - Valida que el usuario logueado tenga permisos (rol 55 = gestor de ausencias de equipo)
  - Si no tiene subordinados asignados, redirige a SolicitudAusencia.aspx
  - Interface: lista de personas, calendario/timeline de ausencias, opciones para agregar/remover personas
- **Data Access**:
  - Clase: `AusenciasEquipoDapper` (en CoreProject)
  - Métodos: `GetAusenciasEquipo`, `GetBeneficiosPendientes`, `GetAusenciasSubordinados`, `GetAusenciasPersonas`, `RemoveAusenciasSubordinado`, `AddAusenciasSubordinado`
- **Complejidad**: ⭐⭐⭐ Media (interfaz dinámica con WebMethods JSON)

---

##### Tablas SQL Identificadas (Ausencias)
- `TH_SolicitudAusencia` (PK: id, FK: idEmpleado, AprobadoPor)
- `TH_Ausencia_Incapacidades` (PK: id, FK: idSolicitudAusencia)
- Catálogos: TipoSolicitudAusencia, EntidadesConsulta, TiposIncapacidad, etc.

##### Relaciones de Datos
- 1 Empleado → N SolicitudAusencia
- 1 SolicitudAusencia → 0..1 Ausencia_Incapacidades
- 1 Empleado (Aprobador) ← N SolicitudAusencia (AprobadoPor)
- AusenciasEquipo: N Jefe → N Subordinados (tabla de relación)

---

**Recomendación**: Migrar Ausencias primero dentro de TH, luego pasar a EmpleadosAdmin (más complejo)

#### 10. **CU_Cuentas** (Clientes) ✅ SPRINT 2 COMPLETADO
- **Contexto**: `CU_Model`
- **Dependencias**: Medias
- **Estado**: ✅ COMPLETADO
  - ✅ Fase 1: Default.aspx (Búsqueda JobBooks), Brief/Frame.aspx, Propuestas.aspx, Estudio.aspx
  - ✅ Fase 2: Presupuesto.aspx completo (IQuote, alternativas, muestra, JBI/JBE, simulador, autorización GM)
- **Componentes migrados**:
  - Controllers: CuentasController, BriefController, PropuestasController, EstudiosController, PresupuestoController
  - Services: CuentasService, BriefService, PropuestasService (lógica de negocio)
  - Adapters: CuentasAdapter (Dapper, SP execution)
  - Views: 22 vistas Razor (Index, modales, grids, paneles)
- **LOC migradas**: ~3,500+ líneas

#### 11. **CC_FinzOpe + FI_Administrativo** ✅ SPRINT PRE-1 + 1-5 COMPLETADO
- **Carpeta**: `WebMatrix/FI_AdministrativoFinanciero/` + `CoreProject/CC_FinzOpe`
- **Contexto**: `FI_Model` + `CC_FinzOpe` + `CAP` (CoreProject)
- **Estado**: ✅ **100% COMPLETO (5/5 grupos migrados, 28/29 páginas)**
- **Evidencia MatrixNext**: `MatrixNext/MatrixNext.Web/Areas/CC/Controllers` y `MatrixNext/MatrixNext.Web/Areas/CC/Views`
- **Grupos Completados**:
  - Grupo 1: Control Presupuestos (4 páginas) - 92h ✅
  - Grupo 2: Presupuestos Internos (4 páginas) - 68h ✅
  - Grupo 3: Procesos Internos (6 páginas) - 132h ✅
  - Grupo 4: Reportes (4 páginas) - 72h ✅
  - Grupo 5: Producción (9 páginas) - 232h ✅
- **Esfuerzo completado**: 80 (CC Pre-1) + 596 (FI Grupos 1-5) = **676 horas** ✅
- **LOC migradas**: ~5,000 líneas

#### 12. **OP_Cualitativo** ✅ SPRINT 6 COMPLETADO
- **Carpeta**: `WebMatrix/OP_Cualitativo/`
- **Contexto**: `OP_Entities` (CoreProject)
- **Estado**: ✅ **SPRINT 6 COMPLETADO (100%)** - 6 fases entregadas
- **Componentes**: Transcription, Scheduling, Sample Management, Calendar/Gantt, Email/Notifications, Bulk Import
- **LOC Total**: 3,297 líneas en 12 commits
- **Evidencia MatrixNext**: `Areas/OP/Controllers/Cualitativo*`, `Services/OP/`, `Areas/OP/Views/CualitativoMuestra/`

#### 13. **CORE (Workflow)** ✅ SPRINT 7 COMPLETADO
- **Contexto**: `CORE_Model`
- **Estado**: ✅ **SPRINT 7 COMPLETADO** - Máquina de estados + UI runtime + SignalR + reportes
- **Evidencia MatrixNext**: `Areas/CORE/Controllers` (`WorkFlowController`, `GestionTareasController`, `IndicadoresController`)
- **LOC migradas**: ~4,000 líneas

#### 14. **EQ (EasyQuote)** ✅ SPRINT 8 COMPLETADO
- **Contexto**: `EQ_Model`
- **Estado**: ✅ **SPRINT 8 COMPLETADO** - Motor de cálculos (26 fórmulas), Seeds (600+ registros), EasyCostService completo
- **Evidencia MatrixNext**: `Areas/EQ/Controllers` (`EasyQuoteController`, `MaestrasAdminController`, `EasyQuoteSeedController`)
- **LOC migradas**: ~3,500 líneas

#### 15. **Home Dashboard** ✅ SPRINT 9 COMPLETADO
- **Carpeta**: `WebMatrix/Home/`
- **Contexto**: `CORE_Model` + múltiples
- **Estado**: ✅ **SPRINT 9 COMPLETADO** - HomeController + DashboardService (7/7 métodos) + dashboard.js + dashboard.css
- **Evidencia MatrixNext**: `Controllers/HomeController.cs`, `Services/Dashboard/DashboardService.cs`, `Views/Home/Index.cshtml`
- **LOC migradas**: ~1,500 líneas

#### 16. **RP_Reportes** ✅ SPRINT 10 COMPLETADO
- **Contexto**: `REP_Model`
- **Estado**: ✅ **SPRINT 10 COMPLETADO** - 12 SP mapeados, Excel export con ClosedXML, paridad WebMatrix
- **Evidencia MatrixNext**: `Areas/RP/Controllers/ReportesController.cs` (334 LOC), ReportesService (436 LOC), ReportesAdapter (449 LOC)
- **LOC migradas**: 1,219 líneas

#### 17. **OP_RO** (Revisión Operacional) ✅ SPRINT 11 COMPLETADO
- **Estado**: ✅ **SPRINT 11 COMPLETADO** - 11 endpoints REST, 20 SP mapeados, máquina de estados
- **Evidencia MatrixNext**: `Areas/OP/Controllers/OP_ROController.cs` (479 LOC), OP_ROService (644 LOC), OP_ROAdapter (622 LOC)
- **LOC migradas**: 1,745 líneas

#### 18. **OP_Trafico** ✅ SPRINT 11 COMPLETADO
- **Estado**: ✅ **SPRINT 11 COMPLETADO** - 8 endpoints REST, 17 SP mapeados, máquina de estados (4 estados)
- **Evidencia MatrixNext**: `Areas/OP/Controllers/OP_TraficoController.cs` (437 LOC), OP_TraficoService (526 LOC), OP_TraficoAdapter (536 LOC)
- **LOC migradas**: 1,499 líneas

---

## 🔍 MÓDULOS EN REVISIÓN/QA (Solo Verificar Completitud)

> **⚠️ ADVERTENCIA**: Estos módulos tienen código existente en MatrixNext pero requieren verificación al 100% contra WebMatrix.  
> **NO iniciar migración desde cero** - Auditar y completar features faltantes solamente.

### 🟠 ALTA (Revisar Primero)

#### 19. **OP_Cuantitativo** (31 páginas) 🔍 EN REVISIÓN
- **Carpeta**: `WebMatrix/OP_Cuantitativo/`
- **Contexto**: `OP_Cuanti_Model` (CoreProject)
- **Estado Actual**: Estructura base existe, verificar completitud
- **Evidencia MatrixNext**: `Areas/OP/Controllers/FichaCuantitativaController.cs`, `Areas/OP/Views/FichaCuantitativa`
- **Documento Análisis**: [ANALISIS_OP_CUANTITATIVO.md](MatrixNext/docs/OP/ANALISIS_OP_CUANTITATIVO.md) (v1.1)
- **Tareas Revisión**:
  - [ ] Auditar 31 páginas WebMatrix vs código MatrixNext
  - [ ] Verificar SP mapeados (ver análisis para lista completa)
  - [ ] Completar controllers/services faltantes
  - [ ] QA funcional completo
- **Estimación Completitud**: TBD (requiere auditoría)

#### 20. **PY_Proyectos** (18 páginas) 🔍 EN REVISIÓN
- **Carpeta**: `WebMatrix/PY_Proyectos/`
- **Contexto**: `PY_Model` (CoreProject)
- **Estado Actual**: Controllers parciales (proyectos, segmentación, sesiones)
- **Evidencia MatrixNext**: `Areas/PY/Controllers`, `Areas/PY/Views`, `docs/PY/MIGRACION_PY_PROYECTOS.md`
- **Tareas Revisión**:
  - [ ] Auditar 18 páginas WebMatrix vs MatrixNext
  - [ ] Completar módulos: Asignaciones, Reportes
  - [ ] Validar integraciones con TH, OP
  - [ ] QA funcional completo
- **Estimación Completitud**: TBD (requiere auditoría)

### 🟡 MEDIA (Revisar Después)

#### 21. **GD_Documentos** 🔍 EN REVISIÓN
- **Contexto**: `GD_Model`
- **Estado Actual**: Controllers (DocumentosMaestro, Repositorio, Solicitudes) + Views
- **Evidencia MatrixNext**: `Areas/GD/Controllers`, `Areas/GD/Views`
- **Tareas Revisión**:
  - [ ] Verificar workflows aprobación (estados, transiciones)
  - [ ] Validar integración filesystem (upload/download)
  - [ ] Completar vistas faltantes (si existen)
  - [ ] QA funcional completo
- **Estimación Completitud**: TBD (requiere auditoría)

---

## 🚧 MÓDULOS PENDIENTES MIGRACIÓN (Sprints 12-19)

> **✅ ACCIÓN**: Estos módulos NO tienen código en MatrixNext - Iniciar migración completa siguiendo [DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md)

### 🟡 BAJA (Complementaria)

#### 22. **PY_ControlCalidad** 🔴 PENDIENTE
- **Carpeta**: `WebMatrix/PY_ControlCalidad`
- **Prioridad**: 🟡 MEDIA-BAJA
- **Sprint Sugerido**: Sprint 12
- **Dependencias**: PY_Proyectos (100%)
- **Estimación**: 3-4 semanas
- **Estado**: ❌ NO INICIADO

#### 23. **SGC_Calidad** 🔴 PENDIENTE
- **Carpeta**: ver `MatrixNext/docs/GENERAL/SGC_Calidad.md`
- **Prioridad**: 🟡 MEDIA-BAJA
- **Sprint Sugerido**: Sprint 13
- **Dependencias**: US_Usuarios, GD_Documentos
- **Estimación**: 2-3 semanas
- **Estado**: ❌ NO INICIADO

#### 24. **ES_Estadistica** 🔴 PENDIENTE
- **Carpeta**: `WebMatrix/ES_Estadistica`
- **Prioridad**: 🟡 BAJA
- **Sprint Sugerido**: Sprint 14
- **Dependencias**: Múltiples módulos (reportes)
- **Estimación**: 2-3 semanas
- **Estado**: ❌ NO INICIADO

#### 25. **IT** 🔴 PENDIENTE
- **Carpeta**: `WebMatrix/IT`
- **Prioridad**: 🟡 BAJA
- **Sprint Sugerido**: Sprint 15
- **Dependencias**: US_Usuarios
- **Estimación**: 1-2 semanas
- **Estado**: ❌ NO INICIADO

#### 26. **Inventario** 🔴 PENDIENTE
- **Carpeta**: `WebMatrix/Inventario`
- **Prioridad**: 🟡 BAJA
- **Sprint Sugerido**: Sprint 20
- **Dependencias**: Verificar
- **Estimación**: 1-2 semanas
- **Estado**: ❌ NO INICIADO

---

## 📊 RESUMEN EJECUTIVO

### Progreso General

| Categoría | Cantidad | Porcentaje | Indicador |
| --- | --- | --- | --- |
| **Completados** | 18 módulos | 58% | ✅ |
| **En Revisión/QA** | 3 módulos | 10% | 🔍 |
| **Pendientes Migración** | 8 módulos | 26% | 🔴 |
| **Excluidos** | 2 módulos | 6% | ⛔ |
| **TOTAL** | 31 módulos | 100% | - |

### LOC Migradas

- **Total Completado**: ~28,810 LOC (Sprints 1-11)
- **En Revisión**: TBD (auditoría pendiente)
- **Pendiente**: TBD (estimación por módulo)

### Timeline

- **Sprints Completados**: 1-11 (2026-01-15) ✅
- **Fase Actual**: Revisión/QA (Sprint 12 Parte 1)
- **Sprints Futuros**: 12-19 (Migración nuevos módulos)
- **Hito Crítico**: 2026-01-15 = Fin Sprints 5-11 (77 días adelantado) ✅

---

## 🎯 PRÓXIMOS PASOS INMEDIATOS

### Prioridad 1: Cerrar Módulos en Revisión (Sprint 12 Parte 1)

1. **OP_Cuantitativo** - Auditar 31 páginas vs MatrixNext
2. **PY_Proyectos** - Auditar 18 páginas vs MatrixNext
3. **GD_Documentos** - Verificar workflows + filesystem

**Objetivo**: Módulos al 100% funcional antes de iniciar nuevas migraciones.

### Prioridad 2: Planificar Sprints 12-19 (Nuevos Módulos)

**Orden sugerido** (por prioridad operativa):
1. Sprint 12: PY_ControlCalidad (3-4 sem)
2. Sprint 13: SGC_Calidad (2-3 sem)
3. Sprint 14: ES_Estadistica (2-3 sem)
4. Sprint 15: IT (1-2 sem)
5. Sprint 16-17: MBO (3 variantes, 4-6 sem)
6. Sprint 18: ResumenProduccion (2-3 sem)
7. Sprint 19: RE_GT + PC_PropiedadCliente (2-4 sem)

**Estimación Total**: 16-24 semanas

---

## Patrón de Migración por Módulo
- **Sprint Sugerido**: Sprint 16-17
- **Dependencias**: TH, PY
- **Estimación**: 4-6 semanas (3 módulos)
- **Estado**: ❌ NO INICIADO

#### 27. **ResumenProduccion** 🔴 PENDIENTE
- **Carpeta**: `WebMatrix/ResumenProduccion`
- **Prioridad**: 🟡 BAJA
- **Sprint Sugerido**: Sprint 18
- **Dependencias**: OP, CC
- **Estimación**: 2-3 semanas
- **Estado**: ❌ NO INICIADO

#### 28. **RE_GT** 🔴 PENDIENTE
- **Carpeta**: `WebMatrix/RE_GT`
- **Prioridad**: 🟡 BAJA
- **Sprint Sugerido**: Sprint 19
- **Dependencias**: TBD
- **Estimación**: 1-2 semanas
- **Estado**: ❌ NO INICIADO

#### 29. **PC_PropiedadCliente** 🔴 PENDIENTE
- **Carpeta**: `WebMatrix/PC_PropiedadCliente`
- **Prioridad**: 🟡 BAJA
- **Sprint Sugerido**: Sprint 19
- **Dependencias**: CU_Cuentas
- **Estimación**: 1-2 semanas
- **Estado**: ❌ NO INICIADO

---

## ⛔ MÓDULOS EXCLUIDOS (NO MIGRAR)

> **DECISIÓN DE NEGOCIO**: Los siguientes módulos NO se migrarán por razones operativas/estratégicas.

### Excluidos Permanentemente

#### 30. **Centro_Informacion** ⛔ EXCLUIDO
- **Carpeta**: `WebMatrix/Centro_Informacion`
- **Razón**: Excluido por decisión del usuario/negocio
- **Estado**: ⛔ NO MIGRAR


```
Módulo WebMatrix (ej: US_Usuarios/)
│
├── 14 páginas .aspx.vb
│
└── MatrixNext → Controllers + Views + Services
    │
    ├── Controllers/ (Coordinar, validar, retornar respuestas)
    ├── Services/ (Lógica de negocio)
    ├── Adapters/ (Acceso a datos, SP execution con Dapper)
    └── Views/ (Razor, Bootstrap, Ajax-first)
```

---

## 📖 DOCUMENTACIÓN DE REFERENCIA

### Documentos Maestros

| Documento | Propósito | Cuándo Usar |
|-----------|-----------|-------------|
| [DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md) | 15 reglas obligatorias | **LEER ANTES DE CUALQUIER MIGRACIÓN** |
| [DASHBOARD_MIGRACION.md](MatrixNext/docs/GENERAL/DASHBOARD_MIGRACION.md) | Estado actual, métricas, clasificación módulos | Referencia ejecutiva diaria |
| [PLAN_MIGRACION_PY_PROYECTOS.md](PLAN_MIGRACION_PY_PROYECTOS.md) | Plan detallado módulo PY | Plantilla para nuevos módulos |
| [ANALISIS_OP_CUANTITATIVO.md](MatrixNext/docs/OP/ANALISIS_OP_CUANTITATIVO.md) | Análisis completo con optimizaciones | Referencia módulos grandes |
| [VERIFICACION_AUSENCIAS_MIGRACION.md](VERIFICACION_AUSENCIAS_MIGRACION.md) | Caso de estudio: verificación TH_Ausencias | Referencia de calidad |
| [RESUMEN_MIGRACION_AUSENCIAS.md](MatrixNext/docs/TH/RESUMEN_MIGRACION_AUSENCIAS.md) | Resumen ejecutivo TH_Ausencias | Stakeholders/Gerencia |
| [SGC_Calidad.md](MatrixNext/docs/GENERAL/SGC_Calidad.md) | Roadmap SGC_Calidad | Sprint 13 (futuro) |

### Cómo Usar Documentación

1. **Iniciando módulo**: Leer [DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md) completo
2. **Planificando**: Usar PLAN_MIGRACION_PY_PROYECTOS.md como plantilla
3. **Verificando**: Comparar contra VERIFICACION_AUSENCIAS_MIGRACION.md
4. **Reportando**: Actualizar [DASHBOARD_MIGRACION.md](MatrixNext/docs/GENERAL/DASHBOARD_MIGRACION.md)

---

**Última actualización**: 2026-01-15  
**Próxima revisión**: Fin Sprint 12 Parte 1 (Revisión/QA módulos parciales)  
**Contacto**: Equipo de desarrollo MatrixNext

