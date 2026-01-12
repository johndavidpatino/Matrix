# INVENTARIO Y MAPEO SPRINT 4 - TH EMPLEADOS/NÓMINA

## 📋 ALCANCE SPRINT 4

**Módulo**: TH_TalentoHumano (Empleados y Nómina)  
**Exclusiones**: Ausencias (ya completado - ver [RESUMEN_MIGRACION_AUSENCIAS.md](RESUMEN_MIGRACION_AUSENCIAS.md))  
**Prioridad**: Media  
**Fecha inicio**: 2026-01-11

---

## 📁 INVENTARIO LEGACY (WebMatrix/TH_TalentoHumano)

### Páginas principales Empleados (8 archivos .aspx)

| Archivo | Responsabilidad | WebMethods | Estado |
|---------|----------------|------------|--------|
| **EmpleadosAdmin.aspx** | Gestión completa empleados (RRHH) | 22 WebMethods | 🔴 Pendiente |
| **EmpleadoUpdate.aspx** | Autogestión datos empleado | 19 WebMethods | 🔴 Pendiente |
| **DesvinculacionesEmpleadosGestionRRHH.aspx** | Procesos de desvinculación RRHH | 5 WebMethods | 🔴 Pendiente |
| **DesvinculacionesEmpleadosGestionArea.aspx** | Evaluaciones desvinculación por área | 7 WebMethods | 🔴 Pendiente |
| **HojasVida.aspx** | Listado hojas de vida | Consulta | 🔴 Pendiente |
| **HojaVida.aspx** | Edición hoja de vida individual | 15+ WebMethods | 🔴 Pendiente |
| **ListadoHojasDeVida.aspx** | Reporte hojas de vida | Reporte | 🔴 Pendiente |
| **EnCasoEmergencia.aspx** | Contactos emergencia | Consulta | 🔴 Pendiente |

### Páginas Reportes Empleados (3 archivos .aspx)

| Archivo | Responsabilidad | Tipo | Estado |
|---------|----------------|------|--------|
| **EmpleadosReporteGeneral.aspx** | Reporte general empleados | Excel/PDF export | 🔴 Pendiente |
| **EmpleadosReporteDiligenciamiento.aspx** | Estado diligenciamiento datos | Reporte | 🔴 Pendiente |
| **ReporteCambiosContratacion.aspx** | Cambios contratación | Reporte | 🔴 Pendiente |

### Páginas excluidas (ya migradas o fuera de alcance)

- ✅ **AusenciasEquipo.aspx** - Migrado en Sprint Ausencias
- ✅ **SolicitudAusencia.aspx** - Migrado en Sprint Ausencias  
- ✅ **SolicitudAusenciaIncapacidades.aspx** - Migrado en Sprint Ausencias
- ✅ **GestionAusenciaRRHH.aspx** - Migrado en Sprint Ausencias
- 🔵 **Capacitacion.aspx** - Fuera de alcance Sprint 4 (prioridad baja)
- 🔵 **Contratistas.aspx** - Fuera de alcance Sprint 4 (prioridad baja)
- 🔵 **LogContratistas.aspx** - Fuera de alcance Sprint 4
- 🔵 **ConsultaLog.aspx** - Herramienta administrativa
- 🔵 **HWH*.aspx** (3 archivos) - Home Working Hours (módulo separado)
- 🔵 **Personas.aspx / Personas2.aspx** - Banco de hojas de vida externas (prioridad baja)

---

## 🔧 WEBMETHODS IDENTIFICADOS

### EmpleadosAdmin.aspx (22 WebMethods - RRHH Admin)

#### CRUD Empleado Principal
1. ✅ **save(...)** → `TH_Empleados_DatosGenerales_Add/Edit`
   - Parámetros: idPersonSelected, tipoIdentificacion, identificacion, foto, nombres, apellidos, edad, experiencia, nivelIngles, numeroCelular, correo, fechaEntrevista, observacion, keywords, ciudadResidencia
   - Retorna: Decimal (ID generado)

2. ✅ **getEmpleados(...)** → `TH_Empleados_Get`
   - Filtros: id, nombres, apellidos, activo, areaServiceLine, cargo, sede
   - Retorna: List<TH_Empleados_Get_Result>

3. ✅ **getEmpleadoPorIdentificacion(...)** → `TH_Empleados_Get`
   - Filtro: identificacion (Long)
   - Retorna: TH_Empleados_Get_Result

#### Experiencia Laboral
4. ✅ **getExperienciasLaboralesPorIdentificacion(...)** → `TH_ExperienciaLaboral_Get`
5. ✅ **addExperienciaLaboral(...)** → `TH_ExperienciaLaboral_Add`
   - Parámetros: identificacion, empresa, fechaInicio, fechaFin, cargo, esInvestigacion
6. ✅ **deleteExperienciaLaboral(...)** → `TH_ExperienciaLaboral_Del`

#### Educación
7. ✅ **getEducacionPorIdentificacion(...)** → `TH_Educacion_Get`
8. ✅ **addEducacion(...)** → `TH_Educacion_Add`
   - Parámetros: identificacion, tipo, titulo, institucion, pais, ciudad, fechaInicio, fechaFin?, modalidad, estado
9. ✅ **deleteEducacion(...)** → `TH_Educacion_Del`

#### Hijos
10. ✅ **getHijosPorIdentificacion(...)** → `TH_Hijos_Get`
11. ✅ **addHijo(...)** → `TH_Hijos_Add`
    - Parámetros: personaId, nombres, apellidos, genero, fechaNacimiento
12. ✅ **deleteHijo(...)** → `TH_Hijos_Del`

#### Contactos Emergencia
13. ✅ **getContactosEmergenciaPorIdentificacion(...)** → `TH_ContactosEmergencia_Get`
14. ✅ **addContactoEmergencia(...)** → `TH_ContactosEmergencia_Add`
    - Parámetros: personaId, nombres, apellidos, parentesco, telefonoFijo?, telefonoCelular?
15. ✅ **deleteContactoEmergencia(...)** → `TH_ContactosEmergencia_Del`

#### Promociones
16. ✅ **getPromocionesPorIdentificacion(...)** → `TH_Promociones_Get`
17. ✅ **addPromocion(...)** → `TH_Promociones_Add`
    - Parámetros: personaId, nuevaAreaId, nuevaBandaId, nuevoCargoId, nuevoLevelId, fechaPromocion
18. ✅ **deletePromocion(...)** → `TH_Promociones_Del`

#### Salarios
19. ✅ **getSalariosPorIdentificacion(...)** → `TH_Salarios_Get`
20. ✅ **addSalario(...)** → `TH_Salarios_Add`
    - Parámetros: personaId, fechaAplicacion, motivoCambio?, salario (Decimal), tipo?
21. ✅ **deleteSalario(...)** → `TH_Salarios_Del`

#### Gestión Estado Empleado
22. ✅ **retirarEmpleado(...)** → SP custom (buscar en legacy)
    - Parámetros: identificacion, fechaRetiro, observacion
23. ✅ **reintegrarEmpleado(...)** → SP custom (buscar en legacy)
    - Parámetros: identificacion, fechaReintegro

#### Actualización Datos
24. ✅ **updateDatosGenerales(...)** → `TH_Empleados_DatosGenerales_Add/Edit`
    - Parámetros: esNuevo, id, tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, fotoBase64
25. ✅ **updateDatosLaborales(...)** → `TH_Empleados_DatosLaborales_Edit`
    - Parámetros: id, idIStaff, jefeInmediato, sede, correoIpsos, fechaIngreso, centroCosto, tipoContratoId, tiempoContratoId?, empresa, jobFunctionId, observaciones

### EmpleadoUpdate.aspx (19 WebMethods - Autogestión)

1-15. Mismos métodos que EmpleadosAdmin pero sin filtro identificacion (usa usuario autenticado)
16. ✅ **updateDatosPersonales(...)** → `TH_Empleados_DatosPersonales_Edit`
    - Parámetros: ciudadId, direccion, nseId, telefonoFijo?, telefonoCelular, emailPersonal, barrio, localidad, municipioNacimientoId, tallaCamisetaId

#### Catálogos (Lookups)
17. ✅ **getAreasServiceLines()** → `TH_Area_Get`
18. ✅ **getGruposSanguineos()** → Tabla `TH_GruposSanguineos`
19. ✅ **getCargos()** → `TH_Cargos_Get`
20. ✅ **getEstadosCiviles()** → Tabla `TH_EstadosCiviles`
21. ✅ **getBandas()** → `TH_Bandas_Get`

### DesvinculacionesEmpleadosGestionRRHH.aspx (5 WebMethods)

1. ✅ **DesvinculacionesEmpleadosEstatus(...)** → SP custom con Dapper
   - Parámetros: pageSize, pageIndex, textoBuscado
   - Retorna: List<TH_DesvinculacionEmpleadosEstatus>

2. ✅ **EmpleadosActivos()** → Dapper query custom
   - Retorna: List<EmpleadosDapper.EmpleadosActivosoResult>

3. ✅ **IniciarProcesoDesvinculacion(...)** → SP custom
   - Parámetros: empleadoId, fechaRetiro, motivosDesvinculacion
   - Retorna: String (ID o mensaje)

4. ✅ **DesvinculacionEmpleadosEstatusEvaluacionesPor(...)** → SP custom
   - Parámetros: desvinculacionEmpleadoId
   - Retorna: IList<TH_DesvinculacionEmpleadosEstatusEvaluacionPorDesvinculacion>

5. ✅ **PDFFormato(...)** → Generación PDF
   - Parámetros: desvinculacionEmpleadoId
   - Retorna: String (URL o Base64)

### DesvinculacionesEmpleadosGestionArea.aspx (7 WebMethods)

1-7. WebMethods de evaluación por área/evaluador:
   - ProcesosDesvinculacionPendientesPorArea
   - ProcesosDesvinculacionPendientesPorEvaluarUsuarioActual
   - ProcesosDesvinculacionItemsVerificarPor
   - InformacionEmpleadoPor
   - GuardarEvaluacion
   - EvaluacionesRealizadasPorUsuarioActual
   - (Helper methods internos: Map, TieneEvaluacionesPendientes, FinalizarProcesoDesvinculacion)

---

## 📊 STORED PROCEDURES IDENTIFICADOS (CoreProject/TH_Model.Context1.vb)

### Empleados - CRUD Principal
| SP Name | Parámetros Clave | Retorno | Uso |
|---------|------------------|---------|-----|
| **TH_Empleados_Get** | id?, nombres?, apellidos?, activo?, serviceLive?, cargo?, sede? | ObjectResult<TH_Empleados_Get_Result> | Listado + búsqueda empleados |
| **TH_Empleados_DatosGenerales_Add** | id, tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, urlFoto, fechaCreacion, usuarioRegistro, fechaUltimaActualizacion | ObjectResult<Long?> | Crear empleado (retorna ID) |
| **TH_Empleados_DatosGenerales_Edit** | id, tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, urlFoto, fechaUltimaActualizacion | ObjectResult<Long?> | Actualizar datos generales |

### Empleados - Datos Personales/Laborales/Nómina
| SP Name | Parámetros Clave | Operación |
|---------|------------------|-----------|
| **TH_Empleados_DatosPersonales_Edit** | id, ciudadId, direccion, nseId, telefonoFijo?, telefonoCelular, emailPersonal, barrioResidencia, localidad, municipioNacimientoDivipolaId, tallaCamisetaId | Actualizar dirección/contacto |
| **TH_Empleados_DatosLaborales_Edit** | id, idIStaff, jefeInmediato, sede, correoIpsos, fechaIngreso, centroCostoId, tipoContratoId, tiempoContratoId?, empresa, jobFunctionId, observaciones | Actualizar datos trabajo |
| **TH_Empleados_Nomina_Edit** | id, bancoId, tipoCuentaId, numeroCuenta, fondoPensionesId, fondoCesantiasId, ePSId, cajaCompensacionId, aRLId | Actualizar datos nómina |
| **TH_Empleados_DatosLaborales_ActualizarSalario** | personaId, salario (Decimal), tipoSalarioId | Actualizar salario actual |
| **TH_Empleados_NivelIngles_Edit** | personaId, nivelInglesId | Actualizar nivel inglés |

### Experiencia Laboral
| SP Name | Parámetros | Operación |
|---------|------------|-----------|
| **TH_ExperienciaLaboral_Get** | personaId (Long) | Listar experiencias |
| **TH_ExperienciaLaboral_Add** | personaId, empresa, fechaInicio, fechaFin, cargo, esInvestigacion | Agregar experiencia (retorna Decimal) |
| **TH_ExperienciaLaboral_Del** | id (Long) | Eliminar experiencia |
| **TH_ExperienciaLaboral_Edit** | id, hojaVidaId, empresa, telefono, inicio, finalizacion, actualmente, cargoId, nivelCargoId, paisId, ciudadId, direccion | Actualizar experiencia (legacy) |

### Educación
| SP Name | Parámetros | Operación |
|---------|------------|-----------|
| **TH_Educacion_Get** | personaId (Long) | Listar educación |
| **TH_Educacion_Add** | personaId, tipo, titulo, institucion, pais, ciudad, fechaInicio, fechaFin?, modalidad, estado | Agregar educación |
| **TH_Educacion_Del** | id (Long) | Eliminar educación |
| **TH_Educacion_Edit** | id, hojavidaId, nivelEstudioId, titulo, institucion, paisId, ciudadId, inicio, finalizacion?, estadoEducacionId | Actualizar educación (legacy) |

### Hijos
| SP Name | Parámetros | Operación |
|---------|------------|-----------|
| **TH_Hijos_Get** | personaId (Long) | Listar hijos |
| **TH_Hijos_Add** | personaId, nombres, apellidos, genero (Byte), fechaNacimiento | Agregar hijo |
| **TH_Hijos_Del** | id (Long) | Eliminar hijo |

### Contactos Emergencia
| SP Name | Parámetros | Operación |
|---------|------------|-----------|
| **TH_ContactosEmergencia_Get** | personaId (Long) | Listar contactos |
| **TH_ContactosEmergencia_Add** | personaId, nombres, apellidos, parentescoId (Byte), telefonoFijo?, telefonoCelular? | Agregar contacto |
| **TH_ContactosEmergencia_Del** | id (Long) | Eliminar contacto |

### Promociones
| SP Name | Parámetros | Operación |
|---------|------------|-----------|
| **TH_Promociones_Get** | personaId (Long) | Listar promociones |
| **TH_Promociones_Add** | personaId, nuevaAreaId, nuevaBandaId, nuevoCargoId, nuevoLevelId, fechaPromocion | Registrar promoción |
| **TH_Promociones_Del** | id (Long) | Eliminar promoción |

### Salarios
| SP Name | Parámetros | Operación |
|---------|------------|-----------|
| **TH_Salarios_Get** | personaId (Long) | Historial salarios |
| **TH_Salarios_Add** | personaId, fechaAplicacion, motivoCambioId?, tipo (Byte)?, salario (Decimal) | Registrar cambio salario |
| **TH_Salarios_Del** | id (Long) | Eliminar registro salario |

### Reportes (por identificar SPs exactos en SQL scripts)
- ❓ **TH_Empleados_Reporte_Info** (existe como Result class)
- ❓ **TH_Empleados_DatosEmergencia_Get** (existe como Result class)
- ❓ **TH_Empleados_EstadoDiligenciamientoDatos_Get** (existe como Result class)
- ❓ **TH_Desvinculacion_*** (múltiples SPs custom con Dapper - buscar en código legacy)

---

## 🗂️ TIPOS COMPLEJOS (DTOs en CoreProject)

### Result Classes (EF Generated)
- TH_Empleados_Get_Result
- TH_ExperienciaLaboral_Get_Result
- TH_Educacion_Get_Result
- TH_Hijos_Get_Result
- TH_ContactosEmergencia_Get_Result
- TH_Promociones_Get_Result
- TH_Salarios_Get_Result
- TH_Empleados_Reporte_Info_Result
- TH_Empleados_DatosEmergencia_Get_Result
- TH_Empleados_EstadoDiligenciamientoDatos_Get_Result

### Catálogos (Tablas lookup)
- TH_Area (Areas/Service Lines)
- TH_GruposSanguineos
- TH_Cargos
- TH_EstadosCiviles
- TH_Bandas
- TH_Sedes
- TH_TiposContrato
- TH_TiemposContrato
- TH_Empresas
- TH_JobFunctions
- TH_Parentescos
- TH_MotivosCambioSalario
- TH_TiposSalario

---

## 📋 MAPEO ACCIÓN → SP → PARÁMETROS

### Flujo 1: Gestión Empleado (RRHH Admin)

| Acción UI | WebMethod | SP/Tabla | Parámetros Input | Retorno |
|-----------|-----------|----------|------------------|---------|
| **Buscar empleados** | getEmpleados() | TH_Empleados_Get | id?, nombres?, apellidos?, activo?, areaServiceLine?, cargo?, sede? | List<TH_Empleados_Get_Result> |
| **Crear empleado** | save() | TH_Empleados_DatosGenerales_Add | idPersonSelected, tipoIdentificacion, identificacion, foto, nombres, apellidos, ..., ciudadResidencia | Decimal (ID) |
| **Actualizar empleado** | updateDatosGenerales() | TH_Empleados_DatosGenerales_Edit | esNuevo, id, tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, fotoBase64 | Long? |
| **Actualizar datos laborales** | updateDatosLaborales() | TH_Empleados_DatosLaborales_Edit | id, idIStaff, jefeInmediato, sede, correoIpsos, fechaIngreso, centroCosto, tipoContratoId, tiempoContratoId?, empresa, jobFunctionId, observaciones | Integer |
| **Retirar empleado** | retirarEmpleado() | ❓ SP custom | identificacion, fechaRetiro, observacion | ❓ |
| **Reintegrar empleado** | reintegrarEmpleado() | ❓ SP custom | identificacion, fechaReintegro | ❓ |

### Flujo 2: Gestión Experiencia Laboral

| Acción UI | WebMethod | SP | Parámetros | Retorno |
|-----------|-----------|-----|-----------|---------|
| **Listar experiencias** | getExperienciasLaboralesPorIdentificacion() | TH_ExperienciaLaboral_Get | personaId (Long) | List<TH_ExperienciaLaboral_Get_Result> |
| **Agregar experiencia** | addExperienciaLaboral() | TH_ExperienciaLaboral_Add | personaId, empresa, fechaInicio, fechaFin, cargo, esInvestigacion | Decimal |
| **Eliminar experiencia** | deleteExperienciaLaboral() | TH_ExperienciaLaboral_Del | id (Long) | Integer |

### Flujo 3: Gestión Educación

| Acción UI | WebMethod | SP | Parámetros | Retorno |
|-----------|-----------|-----|-----------|---------|
| **Listar educación** | getEducacionPorIdentificacion() | TH_Educacion_Get | personaId (Long) | List<TH_Educacion_Get_Result> |
| **Agregar educación** | addEducacion() | TH_Educacion_Add | personaId, tipo, titulo, institucion, pais, ciudad, fechaInicio, fechaFin?, modalidad, estado | Integer |
| **Eliminar educación** | deleteEducacion() | TH_Educacion_Del | id (Long) | Integer |

### Flujo 4: Gestión Hijos

| Acción UI | WebMethod | SP | Parámetros | Retorno |
|-----------|-----------|-----|-----------|---------|
| **Listar hijos** | getHijosPorIdentificacion() | TH_Hijos_Get | personaId (Long) | List<TH_Hijos_Get_Result> |
| **Agregar hijo** | addHijo() | TH_Hijos_Add | personaId, nombres, apellidos, genero (Byte), fechaNacimiento | Integer |
| **Eliminar hijo** | deleteHijo() | TH_Hijos_Del | id (Long) | Integer |

### Flujo 5: Gestión Contactos Emergencia

| Acción UI | WebMethod | SP | Parámetros | Retorno |
|-----------|-----------|-----|-----------|---------|
| **Listar contactos** | getContactosEmergenciaPorIdentificacion() | TH_ContactosEmergencia_Get | personaId (Long) | List<TH_ContactosEmergencia_Get_Result> |
| **Agregar contacto** | addContactoEmergencia() | TH_ContactosEmergencia_Add | personaId, nombres, apellidos, parentescoId (Byte), telefonoFijo?, telefonoCelular? | Integer |
| **Eliminar contacto** | deleteContactoEmergencia() | TH_ContactosEmergencia_Del | id (Long) | Integer |

### Flujo 6: Gestión Promociones

| Acción UI | WebMethod | SP | Parámetros | Retorno |
|-----------|-----------|-----|-----------|---------|
| **Listar promociones** | getPromocionesPorIdentificacion() | TH_Promociones_Get | personaId (Long) | List<TH_Promociones_Get_Result> |
| **Registrar promoción** | addPromocion() | TH_Promociones_Add | personaId, nuevaAreaId, nuevaBandaId, nuevoCargoId, nuevoLevelId, fechaPromocion | Integer |
| **Eliminar promoción** | deletePromocion() | TH_Promociones_Del | id (Long) | Integer |

### Flujo 7: Gestión Salarios

| Acción UI | WebMethod | SP | Parámetros | Retorno |
|-----------|-----------|-----|-----------|---------|
| **Historial salarios** | getSalariosPorIdentificacion() | TH_Salarios_Get | personaId (Long) | List<TH_Salarios_Get_Result> |
| **Registrar salario** | addSalario() | TH_Salarios_Add | personaId, fechaAplicacion, motivoCambioId?, tipo (Byte)?, salario (Decimal) | Integer |
| **Eliminar salario** | deleteSalario() | TH_Salarios_Del | id (Long) | Integer |

### Flujo 8: Autogestión Empleado (EmpleadoUpdate)

| Acción UI | WebMethod | SP | Observación |
|-----------|-----------|-----|-------------|
| **Ver datos propios** | getEmpleado() | TH_Empleados_Get | Usa ID usuario autenticado |
| **Actualizar datos personales** | updateDatosPersonales() | TH_Empleados_DatosPersonales_Edit | ciudadId, direccion, nseId, telefonoFijo?, telefonoCelular, emailPersonal, barrio, localidad, municipioNacimientoId, tallaCamisetaId |
| **Actualizar experiencia/educación/etc** | (mismos métodos que Admin) | (mismos SPs) | Validar que ID empleado == usuario autenticado |

### Flujo 9: Desvinculaciones RRHH

| Acción UI | WebMethod | SP/Query | Observación |
|-----------|-----------|----------|-------------|
| **Listar desvinculaciones** | DesvinculacionesEmpleadosEstatus() | ❓ Dapper query custom | Paginado (pageSize, pageIndex, textoBuscado) |
| **Iniciar desvinculación** | IniciarProcesoDesvinculacion() | ❓ SP custom | empleadoId, fechaRetiro, motivosDesvinculacion |
| **Ver evaluaciones** | DesvinculacionEmpleadosEstatusEvaluacionesPor() | ❓ SP custom | desvinculacionEmpleadoId |
| **Generar PDF** | PDFFormato() | PDF generator | desvinculacionEmpleadoId |

### Flujo 10: Evaluaciones Desvinculación (Áreas)

| Acción UI | WebMethod | SP/Query | Observación |
|-----------|-----------|----------|-------------|
| **Pendientes por área** | ProcesosDesvinculacionPendientesPorArea() | ❓ Custom | AreaId |
| **Pendientes por evaluador** | ProcesosDesvinculacionPendientesPorEvaluarUsuarioActual() | ❓ Custom | Usuario autenticado |
| **Ítems verificar** | ProcesosDesvinculacionItemsVerificarPor() | ❓ Custom | AreaId |
| **Info empleado** | InformacionEmpleadoPor() | ❓ Custom | DesvinculacionEmpleadoId |
| **Guardar evaluación** | GuardarEvaluacion() | ❓ SP custom | DesvinculacionEmpleadoEvaluacionModel |
| **Evaluaciones realizadas** | EvaluacionesRealizadasPorUsuarioActual() | ❓ Custom | Usuario autenticado |

---

## 🎯 ENTREGABLES SPRINT 4

### Fase 1: Adapters (Semana 1, Día 1-4)
- [ ] **ThEmpleadosAdapter.cs**: CRUD empleados con SPs identificados (35+ métodos)
- [ ] **ThExperienciaLaboralAdapter.cs**: Gestión experiencias laborales (3 métodos Get/Add/Del)
- [ ] **ThEducacionAdapter.cs**: Gestión educación (3 métodos Get/Add/Del)
- [ ] **ThDatosComplementariosAdapter.cs**: Hijos, contactos emergencia, promociones, salarios (12 métodos)
- [ ] **ThDesvinculacionAdapter.cs**: Procesos desvinculación (10+ métodos custom con Dapper)
- [ ] **ThCatalogosAdapter.cs**: Lookups (Areas, Cargos, Bandas, Estados Civiles, etc. - ~12 métodos)
- [ ] **DTOs**: Crear modelos Input/Output para todas las operaciones (estimado 30+ DTOs)

### Fase 2: Services (Semana 1, Día 3-6)
- [ ] **IThEmpleadosService.cs / ThEmpleadosService.cs**: Lógica negocio empleados (20+ métodos)
- [ ] **IThDesvinculacionService.cs / ThDesvinculacionService.cs**: Lógica desvinculaciones (8+ métodos)
- [ ] **IThCatalogosService.cs / ThCatalogosService.cs**: Servicios de catálogos (12+ métodos)
- [ ] Validaciones: Validar campos requeridos, formato identificación, rangos fecha, permisos RRHH vs autogestión

### Fase 3: Controllers + Views (Semana 2, Día 1-6)
- [ ] **EmpleadosController.cs**: 25+ endpoints API REST con [Authorize]
  * GET /api/th/empleados (listado con filtros)
  * GET /api/th/empleados/{id} (detalle empleado)
  * POST /api/th/empleados (crear empleado)
  * PUT /api/th/empleados/{id}/datos-generales (actualizar datos)
  * PUT /api/th/empleados/{id}/datos-laborales
  * PUT /api/th/empleados/{id}/datos-personales
  * PUT /api/th/empleados/{id}/nomina
  * POST /api/th/empleados/{id}/retirar
  * POST /api/th/empleados/{id}/reintegrar
  * GET /api/th/empleados/{id}/experiencias (CRUD nested resources)
  * POST/DELETE /api/th/empleados/{id}/experiencias
  * GET /api/th/empleados/{id}/educacion (CRUD nested resources)
  * POST/DELETE /api/th/empleados/{id}/educacion
  * GET /api/th/empleados/{id}/hijos (CRUD nested resources)
  * POST/DELETE /api/th/empleados/{id}/hijos
  * GET /api/th/empleados/{id}/contactos-emergencia (CRUD nested resources)
  * POST/DELETE /api/th/empleados/{id}/contactos-emergencia
  * GET /api/th/empleados/{id}/promociones (CRUD nested resources)
  * POST/DELETE /api/th/empleados/{id}/promociones
  * GET /api/th/empleados/{id}/salarios (CRUD nested resources)
  * POST/DELETE /api/th/empleados/{id}/salarios
  
- [ ] **DesvinculacionController.cs**: 8+ endpoints desvinculaciones
  * GET /api/th/desvinculacion (listado paginado)
  * POST /api/th/desvinculacion (iniciar proceso)
  * GET /api/th/desvinculacion/{id}/evaluaciones
  * POST /api/th/desvinculacion/{id}/evaluar (guardar evaluación área)
  * GET /api/th/desvinculacion/{id}/pdf (generar PDF)
  * GET /api/th/desvinculacion/pendientes-area/{areaId}
  * GET /api/th/desvinculacion/pendientes-usuario
  * GET /api/th/desvinculacion/realizadas-usuario

- [ ] **ThCatalogosController.cs**: 12+ endpoints catálogos
  * GET /api/th/catalogos/areas
  * GET /api/th/catalogos/cargos
  * GET /api/th/catalogos/bandas
  * GET /api/th/catalogos/estados-civiles
  * GET /api/th/catalogos/grupos-sanguineos
  * GET /api/th/catalogos/sedes
  * GET /api/th/catalogos/tipos-contrato
  * GET /api/th/catalogos/empresas
  * (etc.)

- [ ] **Views Razor/AJAX**: Modales Bootstrap para CRUD empleados con tabs (Datos Generales, Laborales, Personales, Nómina, Experiencia, Educación, Hijos, Contactos, Promociones, Salarios)
- [ ] **Views Desvinculación**: Formulario evaluación con checklist dinámico, generación PDF

### Fase 4: QA + Cierre (Semana 2, Día 6-7)
- [ ] Pruebas funcionales: Crear/editar/eliminar empleados, nested resources, retirar/reintegrar
- [ ] Pruebas desvinculaciones: Iniciar proceso, evaluar, generar PDF
- [ ] Validar paridad con legacy (todos los campos mapeados, reglas de negocio respetadas)
- [ ] Verificar permisos: RRHH vs empleados autogestión
- [ ] Pruebas de reportes: Exportes Excel/PDF
- [ ] **MIGRACION_TH_TALENTOHUMANO_COMPLETADA.md**: Documento de cierre

---

## ⚠️ GAPS IDENTIFICADOS / PENDIENTES

1. **SPs desvinculación**: Los SPs exactos para desvinculaciones no están documentados en TH_Model.Context1.vb. Necesitan búsqueda manual en:
   - CoreProject/TH_Model.vb (buscar Dapper queries)
   - SQL scripts en `docs/SQL/CO_Matrix_Structure_SP.sql`
   - Buscar por pattern: `TH_Desvinculacion_*`, `TH_DesvinculacionEmpleados_*`

2. **Reportes**: Determinar si EmpleadosReporteGeneral.aspx usa Crystal Reports o exportes custom (Excel/PDF)

3. **Upload fotos**: Validar si `fotoBase64` se guarda directo en DB o se usa IUploadService para almacenar en disco/Azure Blob

4. **Validación identificación**: Confirmar lógica validación cédula (dígito verificación) si aplica

5. **Nómina integración**: Verificar si hay integración con sistema externo de nómina (probable exportación a archivos planos)

6. **Hoja de vida externa (Personas.aspx/Personas2.aspx)**: Decidir si migrar o excluir (prioridad baja según backlog)

---

## 📝 NOTAS TÉCNICAS

### Tipos de datos importantes
- **Salario**: `Decimal` (SQL: money/decimal(18,2)) - mantener precisión
- **Fechas**: `Date`/`DateTime` (SQL: date/datetime) - timezone UTC en ASP.NET Core
- **IDs grandes**: `Long` (Int64) para personaId, empleadoId
- **Catálogos**: `Byte`, `UShort`, `Short` dependiendo del catálogo

### Validaciones críticas
- Identificación única (constraint en BD)
- Fecha nacimiento < Fecha ingreso
- Salarios > 0
- Estados empleado: Activo/Retirado/Reintegrado (state machine validation)
- Fechas experiencia/educación: FechaInicio < FechaFin
- Permisos RRHH vs empleado normal (AuthorizationPolicies)

### Consideraciones arquitectura
- Nested resources para experiencias/educación/etc (REST pattern: `/empleados/{id}/experiencias`)
- Paginación en listado empleados (DataTables server-side)
- Búsqueda full-text (considerar índices SQL o ElasticSearch futuro)
- Auditoría: `usuarioRegistro`, `fechaCreacion`, `fechaUltimaActualizacion` en todas las tablas
- Foto empleado: Implementar con `IUploadService` (formato: base64 → save as file → store URL en BD)

---

**Estado**: 🔴 Inventario completado, listo para iniciar implementación  
**Próximo paso**: Crear adapters TH (Fase 1 - Día 1-4)  
**Última actualización**: 2026-01-11
