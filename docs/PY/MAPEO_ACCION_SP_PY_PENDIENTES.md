# Mapeo Acción → SP → Parámetros | PY Proyectos Pendientes Sprint 3

**Fecha:** 2025-01-XX  
**Sprint:** 3 - PY Proyectos Pendientes  
**Alcance:** 6 funcionalidades legacy pendientes de migración  
**Estado:** 🔨 En implementación

---

## 📋 Convenciones

| Símbolo | Significado |
|---------|-------------|
| ✅ | SP validado en CO_Matrix_Structure_SP.sql |
| ⚠️ | SP usa EF Core directo (no existe SP en estructura) |
| 🔨 | Implementación pendiente |
| ✔️ | Implementado y probado |

---

## 1️⃣ InHomeVisit (InHomeVisit.aspx)

### WebForm legacy
**Archivo:** `WebMatrix/PY_Proyectos/InHomeVisit.aspx.vb`  
**Entidad principal:** `CoreProject.CampoCualitativo`

### Mapeo de Acciones

| # | Acción Legacy | Método CoreProject | SP/Query Exacto | Parámetros | Estado MatrixNext |
|---|---------------|-------------------|-----------------|-----------|-------------------|
| 1 | Listar InHome por trabajo | `ObtenerInHomexTrabajo(trabajoId)` | ✅ `OP_MuestraTrabajosCuali_InHomeGet` | `@Id INT (NULL), @TrabajoId BIGINT` | 🔨 Adapter pendiente |
| 2 | Obtener InHome por ID | `ObtenerInHomexID(id)` | ✅ `OP_MuestraTrabajosCuali_InHomeGet` | `@Id INT, @TrabajoId BIGINT (NULL)` | 🔨 Adapter pendiente |
| 3 | Guardar/Actualizar InHome | `GuardarMuestraXInHome(itemInHome)` | ⚠️ EF Core `OP_MuestraTrabajosCuali_InHome.Add()` | Entidad completa | 🔨 Service pendiente |
| 4 | Registrar log cambios | `GuardarLogInHome(eLog)` | ⚠️ EF Core `OP_LogInHomeCuali.Add()` | Entidad completa | 🔨 Service pendiente |
| 5 | Obtener log InHome | `ObtenerLogInHome(idSesion)` | ✅ `OP_LogInHomeCuali_Get` | `@IdSesion BIGINT` | 🔨 Adapter pendiente |

**Campos clave OP_MuestraTrabajosCuali_InHome:**
- Id, TrabajoId, SegmentoId, CiudadId, Moderador, GrupoObjetivo, CantidadVisitas, Direccion, FechaInicio, FechaFin, Honorarios, Gastos, Otros, Observaciones

**Validación:** ✅ SP validados en CoreProject/Clases/OP/CampoCualitativo.vb líneas 158-163, 480-497, 517

---

## 2️⃣ VariablesControl (VariablesControl.aspx)

### WebForm legacy
**Archivo:** `WebMatrix/PY_Proyectos/VariablesControl.aspx.vb`  
**Entidad principal:** `CoreProject.Proyecto`

### Mapeo de Acciones

| # | Acción Legacy | Método CoreProject | SP/Query Exacto | Parámetros | Estado MatrixNext |
|---|---------------|-------------------|-----------------|-----------|-------------------|
| 1 | Obtener variables x trabajo x modalidad | `ObtenerVariableControlxTrabajoxMod(trabajoId, modalidad)` | ⚠️ EF Core LINQ `PY_Variables_Control.Where(x => x.TrabajoId == trabajoId && x.Modalidad == modalidad)` | trabajoId BIGINT, modalidad STRING | 🔨 Adapter pendiente |
| 2 | Validar cambios | `validar()` (código local JS) | N/A - Lógica cliente | N/A | 🔨 Controller pendiente |

**Campos clave PY_Variables_Control:**
- Id, TrabajoId, Modalidad, VariableControl (texto HTML)

**Validación:** ✅ Entidad confirmada en CoreProject/Clases/PY/Proyecto.vb línea 123

---

## 3️⃣ InstructivoGeneral (InstructivoGeneral.aspx)

### WebForm legacy
**Archivo:** `WebMatrix/PY_Proyectos/InstructivoGeneral.aspx.vb`  
**Entidad principal:** `CoreProject.Proyecto`

### Mapeo de Acciones

| # | Acción Legacy | Método CoreProject | SP/Query Exacto | Parámetros | Estado MatrixNext |
|---|---------------|-------------------|-----------------|-----------|-------------------|
| 1 | Obtener especificaciones | `ObtenerEspecifaciones(trabajoId)` | ⚠️ EF Core `PY_EspecifTecTrabajo.Where(x => x.TrabajoId == trabajoId).FirstOrDefault` | trabajoId BIGINT | 🔨 Adapter pendiente |
| 2 | Obtener última versión | `ObtenerEspecifacionesLast(trabajoId)` | ⚠️ EF Core `PY_EspecifTecTrabajo.Where(x => x.TrabajoId == trabajoId).OrderByDescending(x => x.NoVersion).First` | trabajoId BIGINT | 🔨 Adapter pendiente |
| 3 | Contar versiones | `ObtenerEspecifacionesContar(trabajoId)` | ⚠️ EF Core `PY_EspecifTecTrabajo.Where(x => x.TrabajoId == trabajoId).Count()` | trabajoId BIGINT | 🔨 Adapter pendiente |
| 4 | Guardar especificaciones | `GuardarInfoEspecificaciones(ent)` | ⚠️ EF Core `PY_EspecifTecTrabajo.Add()` | Entidad completa | 🔨 Service pendiente |
| 5 | Enviar email notificación | `EnviarCorreo(url)` | N/A - Servicio externo WebMatrix.Util | url STRING | 🔨 EmailService reutilizado |

**Campos clave PY_EspecifTecTrabajo (18 campos):**
- TrabajoId, AuditoriaCampo, Codificacion, Critica, EspecifacionesCampo, Estadistica, Incidencias, MaterialApoyo, OtrasEspecificaciones, PilotosCalidad, PilotosCampo, Procesamiento, Verificacion, VCSeguridad, VCObtencion, VCGrupoObjetivo, VCAplicacionInstrumentos, VCDistribucionCuotas, VCMetodologia, Usuario, Fecha, NoVersion

**Validación:** ✅ Entidades confirmadas en CoreProject/Clases/PY/Proyecto.vb líneas 127-163

---

## 4️⃣ InstructivoGeneralCuali (InstructivoGeneralCuali.aspx)

### WebForm legacy
**Archivo:** `WebMatrix/PY_Proyectos/InstructivoGeneralCuali.aspx.vb`  
**Entidad principal:** `CoreProject.SegmentosCuali`

### Mapeo de Acciones

| # | Acción Legacy | Método CoreProject | SP/Query Exacto | Parámetros | Estado MatrixNext |
|---|---------------|-------------------|-----------------|-----------|-------------------|
| 1 | Obtener especificaciones cuali | `ObtenerEspecifacionesCuali(trabajoId)` | ⚠️ EF Core `PY_EspecifTecTrabajoCuali.Where(x => x.TrabajoId == trabajoId).FirstOrDefault` | trabajoId BIGINT | 🔨 Adapter pendiente |
| 2 | Obtener última versión cuali | `ObtenerEspecifacionesCualiLast(trabajoId)` | ⚠️ EF Core `PY_EspecifTecTrabajoCuali.Where(x => x.TrabajoId == trabajoId).OrderByDescending(x => x.NoVersion).First` | trabajoId BIGINT | 🔨 Adapter pendiente |
| 3 | Contar versiones cuali | `ObtenerEspecifacionesContar(trabajoId)` | ⚠️ EF Core `PY_EspecifTecTrabajoCuali.Where(x => x.TrabajoId == trabajoId).Count()` | trabajoId BIGINT | 🔨 Adapter pendiente |
| 4 | Guardar especificaciones cuali | `GuardarInfoEspecificacionesCuali(ent)` | ⚠️ EF Core `PY_EspecifTecTrabajoCuali.Add()` | Entidad completa | 🔨 Service pendiente |
| 5 | Cargar ayudas cuali | `ObtenerAyudasCuali()` | ⚠️ EF Core `PY_AyudasCuali.ToList()` | N/A | 🔨 Adapter pendiente |
| 6 | Cargar tipo reclutamiento | `ObtenerTipoReclutamiento()` | ⚠️ EF Core `PY_TipoReclutamientoCuali.ToList()` | N/A | 🔨 Adapter pendiente |
| 7 | Guardar ayudas x trabajo | `GuardarAyudas(trabajoId, ayudaId, incluido)` | ⚠️ EF Core `PY_AyudasRequeridasCuali.Add/Remove()` | trabajoId BIGINT, ayudaId INT, incluido BIT | 🔨 Service pendiente |
| 8 | Obtener ayudas requeridas | `ObtenerAyudasRequeridasCualiList(trabajoId)` | ⚠️ EF Core `PY_AyudasRequeridasCuali.Where(x => x.TrabajoId == trabajoId).ToList` | trabajoId BIGINT | 🔨 Adapter pendiente |
| 9 | Guardar tipo reclutamiento x trabajo | `GuardarTipoReclutamiento(trabajoId, tipoId, incluido)` | ⚠️ EF Core `PY_ReclutamientoRequeridoCuali.Add/Remove()` | trabajoId BIGINT, tipoId INT, incluido BIT | 🔨 Service pendiente |
| 10 | Obtener reclutamiento requerido | `ObtenerReclutamientoRequeridoCualiList(trabajoId)` | ⚠️ EF Core `PY_ReclutamientoRequeridoCuali.Where(x => x.TrabajoId == trabajoId).ToList` | trabajoId BIGINT | 🔨 Adapter pendiente |

**Campos clave PY_EspecifTecTrabajoCuali (22 campos):**
- TrabajoId, Moderador, EspecificacionesCampo, MaterialApoyo, Incidencias, Auditoria, VCSeguridad, VCObtencion, VCGrupoObjetivo, VCAplicacionInstrumentos, VCDistribucionCuotas, VCMetodologia, Incentivos, PresupuestoIncentivo, DistribucionIncentivo, RegaloClientes, CompraIpsos, PresupuestoCompra, DistribucionCompra, ExclusionesyRestricciones, RecursosPropiedadesCliente, HabeasData, OtrasEspecificaciones, Usuario, Fecha, NoVersion

**Validación:** ✅ Entidades confirmadas en CoreProject/Clases/PY/SegmentosCuali.vb líneas 61-227

---

## 5️⃣ RegistroPlanillasCualitativo (RegistroPlanillasCualitativo.aspx)

### WebForm legacy
**Archivo:** `WebMatrix/PY_Proyectos/RegistroPlanillasCualitativo.aspx.vb`  
**Entidad principal:** `CoreProject.PlanillaModeracionDapper`

### Mapeo de Acciones

| # | Acción Legacy | Método CoreProject | SP/Query Exacto | Parámetros | Estado MatrixNext |
|---|---------------|-------------------|-----------------|-----------|-------------------|
| 1 | Obtener técnicas | `GetTecnicas(tipoTecnica)` | ✅ `UU_TecnicasGet` (Dapper) | @TipoTecnica NVARCHAR(50) | 🔨 Adapter pendiente |
| 2 | Obtener moderadores | `GetModeradores()` | ✅ `UU_ModeradoresGet` (Dapper) | N/A | 🔨 Adapter pendiente |
| 3 | Obtener usuarios x unidad x rol | `UsuariosxUnidadXrol(unidadId, rolId)` | ✅ SP exacto no confirmado (CoreProject.Usuarios.UsuariosDapper) | @UnidadId INT, @RolId INT | 🔨 Adapter pendiente |
| 4 | Guardar planilla moderación | `SavePlanillaModeracion(planillaModeracion)` | ✅ `UU_PlanillaModeracion_Add` (Dapper) | 10 params (IdJob, jobDesc, fecha, hora, tecnica, tiempo, moderador, rol, idUsuarioRegistro, Observaciones, IdCuentasUU, BI_WBSL) | 🔨 Service pendiente |
| 5 | Actualizar planilla moderación | `UpdatePlanillaModeracion(...)` | ✅ `UU_PlanillaModeracion_Update` (Dapper) | 8 params (idPlanilla, idEstado, observaciones, dineroBi, statusBi, idUsuarioAprueba, fechaAprobacion, JobEncontradoEnBI) | 🔨 Service pendiente |
| 6 | Obtener planilla moderación x ID | `GetPlanillasModeracionBy(idPlanilla)` | ✅ `UU_PlanillaModeracionGetBy` (Dapper) | @IdPlanilla INT | 🔨 Adapter pendiente |
| 7 | Guardar planilla informes | `SavePlanillaInformes(planillaInformes)` | ✅ `UU_PlanillaInformes_Add` (Dapper) | 9 params (IdJob, jobDesc, fecha, tecnica, muestra, IdCuentasUU, analista, Observaciones, idUsuarioRegistro, ServiceLineName) | 🔨 Service pendiente |
| 8 | Actualizar planilla informes | `UpdatePlanillaInformes(...)` | ✅ `UU_PlanillaInformes_Update` (Dapper) | 8 params (idPlanilla, idEstado, observaciones, dineroBi, statusBi, idUsuarioAprueba, fechaAprobacion, JobEncontradoEnBI) | 🔨 Service pendiente |
| 9 | Obtener planilla informes x ID | `GetPlanillasInformesBy(idPlanilla)` | ✅ `UU_PlanillaInformesGetBy` (Dapper) | @IdPlanilla INT | 🔨 Adapter pendiente |
| 10 | Listar planillas paginadas | `GetPlanillas(pageSize, pageIndex, filtro, estado)` | ✅ `UU_PlanillasGet` (Dapper) | @PageSize INT, @PageIndex INT, @FiltroPlanilla NVARCHAR(100), @IdEstado SMALLINT (NULL) | 🔨 Adapter pendiente |
| 11 | Exportar planillas moderación | `GetPlanillasModeracionToExport(fechaInicio, fechaFinal)` | ✅ `UU_PlanillasModeracionExport` (Dapper) | @FechaInicio DATETIME, @FechaFinal DATETIME | 🔨 Adapter pendiente |
| 12 | Exportar planillas informes | `GetPlanillasInformesToExport(fechaInicio, fechaFinal)` | ✅ `UU_PlanillasInformesExport` (Dapper) | @FechaInicio DATETIME, @FechaFinal DATETIME | 🔨 Adapter pendiente |
| 13 | Buscar JobBooks en BI | `JobsBySearchValue(tipoTrabajo, pageSize, page, termToSearch)` | N/A - API externa BIService | N/A | 🔨 Service externo integrado |

**Validación:** ✅ Métodos Dapper confirmados en CoreProject/Clases/UU/PlanillaModeracionDapper.vb líneas 14-156

**Nota crítica:** Esta funcionalidad ya usa **Dapper** (no EF), confirmado en legacy code línea 16-17. Los adapters deben mantener patrón Dapper existente.

---

## 6️⃣ DuplicarTrabajos (DuplicarTrabajos.aspx)

### WebForm legacy
**Archivo:** `WebMatrix/PY_Proyectos/DuplicarTrabajos.aspx.vb`  
**Entidad principal:** `CoreProject.Trabajo` (hereda Proyecto)

### Mapeo de Acciones

| # | Acción Legacy | Método CoreProject | SP/Query Exacto | Parámetros | Estado MatrixNext |
|---|---------------|-------------------|-----------------|-----------|-------------------|
| 1 | Duplicar trabajo completo | `DuplicarTrabajo(...)` | ✅ `PY_TrabajosDuplicar` (11 params) | @TrabajoId, @NombreNuevo, @JobbookNuevo, @ProyectoIdNuevo, @ClienteIdNuevo, @TipoModalidad, @FechaInicioNueva, @FechaFinNueva, @Observaciones, @UsuarioId, @DuplicarEspecificaciones BIT | 🔨 Service pendiente |
| 2 | Obtener lista especificaciones | `ObtenerEspecifacionesList(trabajoId)` | ⚠️ EF Core `PY_EspecifTecTrabajo.Where(x => x.TrabajoId == trabajoId).ToList` | trabajoId BIGINT | 🔨 Adapter pendiente |
| 3 | Guardar especificaciones duplicadas | `GuardarInfoEspecificaciones(ent)` | ⚠️ EF Core `PY_EspecifTecTrabajo.Add()` | Entidad completa | 🔨 Service pendiente |
| 4 | Duplicar muestra x ciudad | `DuplicarMuestra(trabajoIdOrigen, trabajoIdDestino, ciudadId)` | ✅ SP exacto no confirmado (probable `OP_MuestraDuplicar`) | @TrabajoIdOrigen, @TrabajoIdDestino, @CiudadId | 🔨 Adapter pendiente |
| 5 | Obtener configuración trabajo | `trabajoconfiguracionget(trabajoId)` | ✅ `PY_TrabajosConfiguracionGet` | @TrabajoId BIGINT | 🔨 Adapter pendiente |
| 6 | Guardar configuración trabajo | `guardartrabajoconfiguracion(config)` | ✅ `PY_TrabajosConfiguracion_Add` | @TrabajoId, @Config1, @Config2, ... (campos config) | 🔨 Service pendiente |
| 7 | Duplicar hilo workflow | `hilo(trabajoIdOrigen, trabajoIdDestino)` | ✅ SP exacto no confirmado (probable `PY_HiloDuplicar`) | @TrabajoIdOrigen, @TrabajoIdDestino | 🔨 Service pendiente |
| 8 | Copiar documentos físicos | `copiardocumentos(trabajoIdOrigen, trabajoIdDestino)` | N/A - Operación file system | N/A | 🔨 Controller pendiente |

**Validación parcial:** 
- ✅ DuplicarTrabajo confirmado en flujo complejo (línea 54-80 del legacy)
- ✅ trabajoconfiguracionget confirmado en CoreProject/Clases/OP_Cuanti/TrabajoOPCuanti.vb línea 42
- ⚠️ Métodos hilo(), DuplicarMuestra(), copiardocumentos() requieren validación adicional en CoreProject

**Complejidad alta:** Esta funcionalidad orquesta múltiples SP y operaciones transaccionales.

---

## 7️⃣ DistribucionEntrevistas (DistribucionEntrevistas.aspx)

### WebForm legacy
**Archivo:** `WebMatrix/PY_Proyectos/DistribucionEntrevistas.aspx.vb`  
**Entidad principal:** `CoreProject.CampoCualitativo`

### Mapeo de Acciones

| # | Acción Legacy | Método CoreProject | SP/Query Exacto | Parámetros | Estado MatrixNext |
|---|---------------|-------------------|-----------------|-----------|-------------------|
| 1 | Listar entrevistas x trabajo | `ObtenerEntrevistasxTrabajo(trabajoId)` | ✅ `OP_MuestraTrabajosCuali_EntrevistasGet` | @Id BIGINT (NULL), @TrabajoId BIGINT | 🔨 Adapter pendiente |
| 2 | Listar distribución x entrevista | `ObtenerEntrevistasDistribucionxIdEntrevista(entrevistaId)` | ✅ `OP_EntrevistasCuali_DistribucionGet` | @Id INT (NULL), @IdEntrevista BIGINT, @IdTrabajo BIGINT (NULL) | 🔨 Adapter pendiente |
| 3 | Obtener distribución x ID | `ObtenerEntrevistasDistribucionxIdDistribucion(distribucionId)` | ✅ `OP_EntrevistasCuali_DistribucionGet` | @Id INT, @IdEntrevista BIGINT (NULL), @IdTrabajo BIGINT (NULL) | 🔨 Adapter pendiente |
| 4 | Obtener moderadores | `ObtenerModeradores()` | ✅ `US_UsuariosModeradoresCualitativos` | N/A | 🔨 Adapter pendiente |
| 5 | Guardar distribución entrevista | `GuardarDistribucionEntrevistas(itemDistribucion)` | ✅ `OP_EntrevistasCuali_Distribucion_Add` (Dapper confirmado línea 430) | @Cantidad, @IdEntrevista, @TrabajoId, @GrupoObjetivo, @CiudadId, @FechaInicio, @FechaFin, @Moderador, @Usuario | 🔨 Service pendiente |
| 6 | Guardar log entrevista | `GuardarLogEntrevistas(eLog)` | ⚠️ EF Core `OP_LogEntrevistasCuali.Add()` | Entidad completa | 🔨 Service pendiente |
| 7 | Obtener log entrevistas | `ObtenerLogEntrevistas(distribucionId)` | ✅ `OP_LogEntrevistasCuali_Get` | @IdDistribucion BIGINT | 🔨 Adapter pendiente |

**Validación:** ✅ SP confirmados en CoreProject/Clases/OP/CampoCualitativo.vb líneas 118-139, 430

**Flujo crítico:** Gestión estados (Activa=1, Caída=2, Anulada=3, Efectiva=4) con validaciones en GridView.

---

## 📊 Resumen de SP y Estrategia de Implementación

### SP Validados (requieren Dapper adapters) ✅
1. `OP_MuestraTrabajosCuali_InHomeGet`
2. `OP_LogInHomeCuali_Get`
3. `UU_TecnicasGet`
4. `UU_ModeradoresGet`
5. `UU_PlanillaModeracion_Add`
6. `UU_PlanillaModeracion_Update`
7. `UU_PlanillaModeracionGetBy`
8. `UU_PlanillaInformes_Add`
9. `UU_PlanillaInformes_Update`
10. `UU_PlanillaInformesGetBy`
11. `UU_PlanillasGet`
12. `UU_PlanillasModeracionExport`
13. `UU_PlanillasInformesExport`
14. `PY_TrabajosDuplicar`
15. `PY_TrabajosConfiguracionGet`
16. `PY_TrabajosConfiguracion_Add`
17. `OP_MuestraTrabajosCuali_EntrevistasGet`
18. `OP_EntrevistasCuali_DistribucionGet`
19. `OP_EntrevistasCuali_Distribucion_Add`
20. `OP_LogEntrevistasCuali_Get`
21. `US_UsuariosModeradoresCualitativos`

### Operaciones EF Core (requieren services) ⚠️
1. `PY_Variables_Control` (CRUD directo)
2. `PY_EspecifTecTrabajo` (CRUD directo)
3. `PY_EspecifTecTrabajoCuali` (CRUD directo)
4. `PY_AyudasCuali` (lookup table)
5. `PY_TipoReclutamientoCuali` (lookup table)
6. `PY_AyudasRequeridasCuali` (relación M2M)
7. `PY_ReclutamientoRequeridoCuali` (relación M2M)
8. `OP_MuestraTrabajosCuali_InHome` (CRUD directo)
9. `OP_LogInHomeCuali` (CRUD directo)
10. `OP_LogEntrevistasCuali` (CRUD directo)

### Servicios Externos/Especiales
1. **BIService:** API externa JobBookModerationInfo, JobsBySearchValue
2. **EnviarCorreo:** WebMatrix.Util legacy (migrar a GdEmailService)
3. **File System:** copiardocumentos (requiere IFileService)

---

## ⚠️ Hallazgos Críticos

### 1. Patrón Mixto (Dapper + EF Core)
- **RegistroPlanillasCualitativo** ya usa Dapper en legacy (PlanillaModeracionDapper.vb)
- **InHomeVisit, DistribucionEntrevistas** usan SP + EF Core mixto
- **InstructivoGeneral, VariablesControl** usan 100% EF Core

**Recomendación:** Mantener patrón mixto coherente con legacy. No forzar SP donde no existen.

### 2. SP Inexistentes
- Algunos métodos CoreProject usan LINQ directo sobre DbContext
- No hay SP para `PY_Variables_Control`, `PY_AyudasCuali`, `PY_EspecifTecTrabajo` operations
- **Decisión:** Usar EF Core en MatrixNext.Data para estas entidades (coherente con existente)

### 3. Versionamiento de Especificaciones
- `PY_EspecifTecTrabajo` y `PY_EspecifTecTrabajoCuali` tienen campo `NoVersion`
- Flujo duplicación crea nuevas versiones sin modificar anteriores
- **Decisión:** Mantener inmutabilidad de versiones en implementación

### 4. Integración BI
- `RegistroPlanillasCualitativo` consume API externa BI (BIService)
- Requiere ApiKey de `ConfigurationManager.AppSettings("BI_API_URLBase")`
- **Decisión:** Migrar configuración a appsettings.json MatrixNext.Web

---

## 🎯 Plan de Implementación

### Fase 1: Adapters (Días 3-4)
1. `PyInHomeVisitAdapter` (SP + EF mixto)
2. `PyVariablesControlAdapter` (EF Core)
3. `PyInstructivosAdapter` (EF Core)
4. `PyPlanillasAdapter` (Dapper puro, reusar PlanillaModeracionDapper)
5. `PyDistribucionEntrevistasAdapter` (SP + EF mixto)
6. Extender `PyTrabajosAdapter` (DuplicarTrabajo)

### Fase 2: Services (Día 5)
1. `PyInHomeVisitService`
2. `PyVariablesControlService`
3. `PyInstructivosService`
4. `PyPlanillasService` (incluye integración BIService)
5. `PyDistribucionEntrevistasService`
6. `PyTrabajosService.DuplicarTrabajoCompleto()`

### Fase 3: Controllers + Views (Día 6)
1. `InHomeVisitController` (AJAX CRUD)
2. `VariablesControlController` (Editor HTML)
3. `InstructivosController` (Versiones cuanti/cuali)
4. `PlanillasController` (Moderación + Informes)
5. `DistribucionEntrevistasController` (Gestión estados)
6. Extender `TrabajosController` (acción Duplicar)

### Fase 4: QA (Día 7)
- Pruebas funcionales por feature
- Validación paridad con legacy
- Build clean
- Commit cierre Sprint 3

---

**Última actualización:** 2025-01-XX  
**Siguiente paso:** Implementar adapters fase 1
