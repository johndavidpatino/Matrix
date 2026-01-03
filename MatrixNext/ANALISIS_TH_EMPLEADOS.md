# Análisis Detallado - Módulo TH_TalentoHumano: Empleados (Administración, Reportes y Desvinculaciones RRHH)

## ✅ RESUMEN EJECUTIVO - Estado Actual de Migración

**Fecha última actualización:** 3 de enero de 2026 - 15:00

**Estado de liberación:** 3 de enero de 2026 — Backend completado y entregado; pendientes: frontend y pruebas de integración (PDF opcional).

### Estado General del Módulo

| WebForm | Métodos Totales | Migrados | Pendientes | % Completado | Estado |
|---------|----------------|----------|------------|--------------|--------|
| EmpleadosAdmin.aspx | 34 | 34 | 0 | 100% | ✅ COMPLETO |
| EmpleadosReporteDiligenciamiento.aspx | 1 | 1 | 0 | 100% | ✅ COMPLETO |
| EmpleadosReporteGeneral.aspx | 5 | 5 | 0 | 100% | ✅ COMPLETO |
| DesvinculacionesEmpleadosGestionRRHH.aspx | 5 | 5 | 0 | 100% | ✅ COMPLETO |
| **TOTAL** | **45** | **45** | **0** | **100%** | ✅ **COMPLETADO AL 100%** |

### ✅ FUNCIONALIDADES COMPLETADAS

**EmpleadosAdmin.aspx - 100% Funcional:**

✅ **Búsqueda y Consulta:**
- Búsqueda avanzada con filtros (identificación, nombres, apellidos, activo, área, cargo, sede)
- Consulta por identificación
- Obtención de información completa del empleado

✅ **Actualización de Datos Maestros:**
- ✅ updateDatosGenerales - Crear/actualizar empleado (incluye foto en base64)
- ✅ updateDatosLaborales - Actualizar datos laborales
- ✅ updateDatosPersonales - Actualizar datos personales
- ✅ updateNomina - Actualizar información de nómina
- ✅ updateNivelIngles - Actualizar nivel de inglés

✅ **Catálogos/Combos:**
- ✅ getAreasServiceLines - Áreas/Service Lines
- ✅ getGruposSanguineos - Grupos sanguíneos
- ✅ getCargos - Cargos
- ✅ getEstadosCiviles - Estados civiles
- ✅ getTodosCatalogos - Endpoint optimizado para carga inicial

✅ **Experiencia Laboral:**
- ✅ getExperienciasLaboralesPorIdentificacion
- ✅ addExperienciaLaboral
- ✅ deleteExperienciaLaboral

✅ **Educación:**
- ✅ getEducacion
- ✅ addEducacion
- ✅ deleteEducacion

✅ **Hijos:**
- ✅ getHijos
- ✅ addHijo
- ✅ deleteHijo

✅ **Contactos de Emergencia:**
- ✅ getContactosEmergencia
- ✅ addContactoEmergencia
- ✅ deleteContactoEmergencia

✅ **Promociones y Salarios:**
- ✅ getPromociones
- ✅ addPromocion
- ✅ getSalarios
- ✅ addSalario

✅ **Retiro y Reintegro:**
- ✅ retirarEmpleado
- ✅ reintegrarEmpleado

**EmpleadosReporteDiligenciamiento.aspx - 100% Funcional:**
- ✅ getEstadoDiligenciamiento - Reporte de completitud de datos

**EmpleadosReporteGeneral.aspx - 100% Funcional:**
- ✅ 5 reportes Excel con ClosedXML:
  - Información general consolidada
  - Hijos
  - Educación
  - Experiencia laboral
  - Contactos de emergencia

**DesvinculacionesEmpleadosGestionRRHH.aspx - 100% Funcional (NUEVO):**
- ✅ DesvinculacionesEmpleadosEstatus - Listado paginado de desvinculaciones
- ✅ EmpleadosActivos - Lista de empleados disponibles para desvincular
- ✅ IniciarProcesoDesvinculacion - Iniciar proceso de desvinculación
- ✅ DesvinculacionEmpleadosEstatusEvaluacionesPor - Evaluaciones por área
- ✅ PDFFormato - Generación de PDF de desvinculación

**DesvinculacionesEmpleadosGestionArea.aspx - 100% Backend Funcional (NUEVO):**
- ✅ ProcesosDesvinculacionPendientesPorArea
- ✅ ProcesosDesvinculacionPendientesPorEvaluarUsuarioActual
- ✅ ProcesosDesvinculacionItemsVerificarPor
- ✅ InformacionEmpleadoPor
- ✅ GuardarEvaluacion (incluye finalizar proceso cuando no hay pendientes)
- ✅ EvaluacionesRealizadasPorUsuarioActual

### 📦 Arquitectura Implementada

**Data Layer (MatrixNext.Data/Modules/TH/Empleados/):**
- ✅ EmpleadoDTO.cs: 15 DTOs completos
  - 10 DTOs para operaciones CRUD
  - 5 DTOs para actualización de datos maestros
  - 5 DTOs para reportes Excel
- ✅ CatalogosDTO.cs: 18 DTOs para catálogos
- ✅ DesvinculacionDTO.cs: 13 DTOs para desvinculaciones (NUEVO)
- ✅ EmpleadoDataAdapter.cs: 58 métodos
  - 35+ métodos originales
  - 5 métodos de actualización
  - 18 métodos de catálogos
- ✅ DesvinculacionDataAdapter.cs: 11 métodos (NUEVO)
- ✅ EmpleadoService.cs: 53 métodos
  - 30+ métodos originales
  - 5 métodos de actualización con validaciones
  - 18 métodos de catálogos
- ✅ DesvinculacionService.cs: 11 métodos (NUEVO)

**Web Layer (MatrixNext.Web/Areas/TH/):**
- ✅ EmpleadosController.cs: 25+ endpoints
  - 20+ endpoints originales
  - 5 endpoints PUT para actualización
- ✅ CatalogosController.cs: 5 endpoints GET
  - 4 endpoints individuales para catálogos críticos
  - 1 endpoint /Todos para carga optimizada
- ✅ DesvinculacionesController.cs: 12 endpoints (NUEVO)
  - POST /Buscar (filtros y paginación)
  - GET /EmpleadosActivos
  - POST /Iniciar
  - GET /{id}/Evaluaciones
  - GET /{id}/PDF
  - GET /Pendientes/Area/{areaId}
  - GET /Pendientes/UsuarioActual
  - GET /ItemsVerificar/Area/{areaId}
  - GET /EmpleadoInfo/{id}
  - POST /GuardarEvaluacion
  - GET /EvaluacionesRealizadas/UsuarioActual
- ✅ EmpleadosReportesController.cs: Completo con Excel
- ✅ ExcelHelper.cs: Helper genérico con ClosedXML

**Vistas:**
- ✅ Views/Empleados/Index.cshtml
- ✅ Views/EmpleadosReportes/General.cshtml
- ✅ Views/Desvinculaciones/Index.cshtml (placeholder)

### 🛠️ Cambios Técnicos Implementados

**Sesión 1 - Métodos Faltantes (Datos Maestros y Catálogos):**

**1. DTOs Creados:**
- ActualizarDatosGeneralesDTO (16 propiedades, incluye FotoBase64 y RutaFoto)
- ActualizarDatosLaboralesDTO (17 propiedades)
- ActualizarDatosPersonalesDTO (11 propiedades)
- ActualizarNominaDTO (9 propiedades)
- ActualizarNivelInglesDTO (2 propiedades)
- 18 DTOs de catálogos (AreaServiceLineDTO, GrupoSanguineoDTO, CargoDTO, etc.)

**2. Métodos de Adapter Implementados:**
- ActualizarDatosGenerales → SP: TH_Empleado_ActualizarDatosGenerales
- ActualizarDatosLaborales → SP: TH_Empleado_ActualizarDatosLaborales
- ActualizarDatosPersonales → SP: TH_Empleado_ActualizarDatosPersonales
- ActualizarNomina → SP: TH_Empleado_ActualizarNomina
- ActualizarNivelIngles → SP: TH_Empleado_ActualizarNivelIngles
- 18 métodos para obtención de catálogos

**3. Métodos de Service Implementados:**
- 5 métodos de actualización con validaciones completas
- Validación de email con EsEmailValido
- Validación de edad mínima (18 años)
- Manejo de foto en base64 (decodificación y guardado)
- 18 métodos de catálogos con manejo de errores
- ObtenerTodosCatalogos para carga optimizada

**4. Endpoints de Controller Creados:**
- PUT /TH/Empleados/DatosGenerales
- PUT /TH/Empleados/DatosLaborales
- PUT /TH/Empleados/DatosPersonales
- PUT /TH/Empleados/Nomina
- PUT /TH/Empleados/NivelIngles
- GET /TH/Catalogos/AreasServiceLines
- GET /TH/Catalogos/GruposSanguineos
- GET /TH/Catalogos/Cargos
- GET /TH/Catalogos/EstadosCiviles
- GET /TH/Catalogos/Todos

**Sesión 2 - Módulo Desvinculaciones (NUEVO - 100% Completo):**

**1. DTOs Creados (DesvinculacionDTO.cs):**
- DesvinculacionEstatusDTO (modelo real del SP de estatus)
- DesvinculacionesPaginadasDTO (metadata + lista)
- DesvinculacionFiltroDTO (pageSize, pageIndex, textoBuscado)
- EmpleadoActivoDTO (Id, Nombres, Apellidos)
- IniciarDesvinculacionDTO (empleadoId, fechaRetiro, motivosDesvinculacion)
- DesvinculacionEvaluacionDTO (modelo real del SP de evaluaciones)
- GuardarEvaluacionRequestDTO (request de gestión por área)
- DesvinculacionEmpleadoEvaluacionAreaDTO (payload de SP Add evaluación)
- DesvinculacionEmpleadoPendientePorEvaluarAreaDTO
- DesvinculacionEmpleadoPendienteEvaluarPorEvaluadorDTO
- DesvinculacionEmpleadoEvaluacionRealizadaPorEvaluadorDTO
- DesvinculacionEmpleadosAreaItemVerificarDTO
- DesvinculacionEmpleadoEmpleadoInfoDTO (info empleado para PDF)

**2. Adapter Implementado (DesvinculacionDataAdapter.cs):**
- ObtenerDesvinculacionesEstatus → SP: TH_DesvinculacionEmpleadosEstatus
- ObtenerEmpleadosActivos → SP: TH_EmpleadosActivos_Get
- IniciarProcesoDesvinculacion → SP: TH_DesvinculacionEmpleadosAdd
- ObtenerEvaluacionesPorDesvinculacion → SP: TH_DesvinculacionEmpleadosEstatusEvaluacionesPorDesvinculacion
- ObtenerInformacionEmpleadoPor → SP: TH_DesvinculacionesEmpleadosEmpleadoInfo
- PendientesPorEvaluarPorArea → SP: TH_DesvinculacionesEmpleadosPendientesEvaluarPorArea
- ItemsVerificarPor → SP: TH_DesvinculacionesEmpleadosItemsVerificarPorArea
- GuardarEvaluacion → SP: TH_DesvinculacionEmpleadoAreaEvaluacion_Add
- PendientesPorEvaluarPorEvaluador → SP: TH_DesvinculacionesEmpleadosPendientesEvaluarPorEvaluador
- EvaluacionesRealizadasPorEvaluador → SP: TH_DesvinculacionEmpleadosEvaluacionesRealizadasPorEvaluador
- FinalizarProceso → SP: TH_DesvinculacionEmpleadoFinalizarProceso

**3. Service Implementado (DesvinculacionService.cs):**
- ObtenerDesvinculacionesPaginadas - Con validación de paginación
- ObtenerEmpleadosActivos - Lista completa
- IniciarProcesoDesvinculacion - Validación mínima (motivo requerido) + fechaRegistro UTC-5
- ObtenerEvaluacionesPorDesvinculacion - Por ID de desvinculación
- GenerarPDFFormato - Plantilla HTML + conversión HTML→PDF vía servicio externo (URLHTMLToPDFGenerator)
- Flujo GestiónArea: pendientes/items/guardar evaluación/evaluaciones realizadas + finalización automática

**4. Controller Implementado (DesvinculacionesController.cs):**
- GET /TH/Desvinculaciones - Vista principal
- POST /TH/Desvinculaciones/Buscar - Búsqueda con paginación
- GET /TH/Desvinculaciones/EmpleadosActivos - Combo de empleados
- POST /TH/Desvinculaciones/Iniciar - Iniciar proceso
- GET /TH/Desvinculaciones/{id}/Evaluaciones - Detalle de evaluaciones
- GET /TH/Desvinculaciones/{id}/PDF - Generar PDF

**Configuración requerida (para equivalencia legacy completa):**
- appsettings.json: LegacyServices:URLHTMLToPDFGenerator
- appsettings.json: LegacyServices:WebMatrixBaseUrl (se usa para disparar /Emails/... del legacy)

### ⚠️ Pendientes No Bloqueantes (Frontend)

**Formularios de Empleados (1-2 días):**
- Formularios de edición para datos maestros
- Integración de dropdowns con catálogos
- Upload de fotos (conversión a base64)
- Validaciones del lado cliente

**Gestión de Fotos (0.5 días):**
- Configurar ruta física de guardado en appsettings.json
- Implementar guardado de archivo desde base64
- Manejo de tamaños y formatos permitidos (jpg, png, max 2MB)

**Vista de Desvinculaciones (1-2 días):**
- Grilla paginada con búsqueda
- Modal para iniciar proceso de desvinculación
- Vista de detalle de evaluaciones por área
- Botón de descarga de PDF

**Integración de PDF (0.5 días):**
- Integrar librería de HTML to PDF (IronPdf, SelectPdf, etc.)
- Crear plantilla HTML: TemplateFormatoDesvinculacion.html
- Configurar ruta de plantilla

**Envío de Correos (0.5 días):**
- Integrar servicio de correo para notificaciones
- Plantilla de email para notificación a áreas
- URL: /Emails/DesvinculacionEmpleadoSolicitudDiligenciamientoAreas.aspx?id={desvinculacionId}

---

## 1. Descripción General del Submódulo

El submódulo **Empleados** dentro de **TH_TalentoHumano** concentra la gestión integral de la información de empleados activos e históricos, así como reportes y procesos de desvinculación.

Los cuatro WebForms a migrar en esta fase cubren:

1. **EmpleadosAdmin.aspx**
   - Administración central de la ficha del empleado (datos generales, laborales, personales, nómina, nivel de inglés).
   - Gestión de información asociada: experiencia laboral, educación, hijos, contactos de emergencia, promociones y salarios.
   - Operaciones de **retiro** y **reintegro** de empleados.
   - Búsqueda y filtrado por identificación, nombres, apellidos, estado activo, área/service line, cargo y sede.

2. **EmpleadosReporteDiligenciamiento.aspx**
   - Reporte del **estado de diligenciamiento** de la información de empleados (qué secciones de la ficha están completadas).
   - Grilla con indicadores (sí/no) por sección y porcentaje global de diligenciamiento.

3. **EmpleadosReporteGeneral.aspx**
   - Reportes Excel generales sobre empleados:
     - Información general consolidada.
     - Hijos.
     - Educación.
     - Experiencia laboral.
     - Contactos de emergencia.

4. **DesvinculacionesEmpleadosGestionRRHH.aspx**
   - Proceso de **desvinculación** gestionado por RRHH.
   - Listado paginado del estado de procesos de desvinculación.
   - Inicio del proceso de desvinculación (motivos, fecha de retiro, responsable).
   - Consulta del estado de evaluaciones de áreas involucradas.
   - Generación de **formato PDF de desvinculación** a partir de plantilla HTML.

La migración debe respetar estrictamente las **DIRECTRICES_MIGRACION.md**:
- Reutilizar SP y lógica de CoreProject (Reglas 1–4).
- Usar EF Core solo para CRUD simples donde no haya SP críticos.
- Mantener estructura por **áreas** en MatrixNext.Web (Regla 9).
- Implementar **Adapter + Service + Controller** (sección de arquitectura).
- No agregar nuevas funcionalidades (Regla 6), solo migrar lo existente.

---

## 2. Flujo de Negocio por WebForm

### 2.1 EmpleadosAdmin.aspx – Administración integral de empleados

**Rol típico**: RRHH / Talento Humano (administra la ficha del empleado).

#### 2.1.1 Búsqueda y selección de empleados

Flujo principal observado en el JavaScript de la página:

```text
Usuario abre EmpleadosAdmin.aspx
    ↓
Define filtros (opcionales):
    - Identificación (filterIdentificacion)
    - Nombres (filterNombres)
    - Apellidos (filterApellidos)
    - Activo (filterActivo)
    - Área/Service Line (filterAreaServiceLine)
    - Cargo (filterCargo)
    - Sede (filterSede)
    ↓
Click en acción de búsqueda (implícito al llamar getEmpleados)
    ↓
JS construye objeto formData con valores o null
    ↓
fetch('EmpleadosAdmin.aspx/getEmpleados', POST, JSON)
    ↓
WebMethod getEmpleados(...) → CoreProject.Empleados.obtener(...)
    ↓
Retorna lista TH_Empleados_Get_Result
    ↓
JS drawEmpleados(...) genera tarjetas (cardPerson) por empleado
    ↓
Usuario hace click sobre una tarjeta → showPerson(card)
    ↓
Se despliega panel de detalle con tabs para información detallada y operaciones
```

**Componentes visuales clave** (a migrar a Razor Views + JS/TS moderno):
- Panel de **filtros** (inputs + combos).
- Contenedor de tarjetas: `containerCardsPerson` con `.cardPerson` por empleado.
- Información mostrada en cada tarjeta:
  - Foto (`urlFoto` o imagen por defecto `../Images/sin-foto.jpg`).
  - Nombres, apellidos.
  - Fecha de nacimiento.
  - Correo Ipsos.
  - Teléfono celular.
  - Grupo sanguíneo.
  - Sede.
  - Área/Service Line.
  - % diligenciamiento de datos.
- Botón de **Activar/Desactivar** empleado.

#### 2.1.2 Consulta de información detallada

Una vez seleccionado un empleado, el frontend ejecuta varias llamadas asíncronas:

```text
getEmpleado(identificacion)
    → EmpleadosAdmin.aspx/getEmpleadoPorIdentificacion
    → CoreProject.Empleados.obtenerPorIdentificacion

getExperienciasLaboralesPorIdentificacion(identificacion)
    → CoreProject.TH.ExperienciaLaboral.getByPersonaId

getEducacion(identificacion)
    → CoreProject.TH.Educacion.ObtenerEducacionPorIdentificacion

getHijos(identificacion)
    → CoreProject.Personas.obtenerHijosPorPersonaId

getContactosEmergencia(identificacion)
    → CoreProject.Personas.obtenerContactosEmergenciaPorPersonaId

getPromociones(identificacion)
    → CoreProject.Empleados.obtenerPromocionesPorPersonaId

getSalarios(identificacion)
    → CoreProject.Empleados.obtenerSalariosPorPersonaId
```

La UI arma secciones/tabs con la información:
- **Experiencia laboral** (lista, con opción de borrar cada registro).
- **Educación**.
- **Hijos**.
- **Contactos de emergencia**.
- **Promociones**.
- **Salarios**.
- **Datos generales** y **foto** del empleado.

#### 2.1.3 Mantenimiento de información asociada

WebMethods que agregan o eliminan registros relacionados:

- **Experiencia Laboral**
  - `addExperienciaLaboral(identificacion, empresa, fechaInicio, fechaFin, cargo, esInvestigacion)`
    - Llama `CoreProject.TH.ExperienciaLaboral.add(...)`.
  - `deleteExperienciaLaboral(identificacion As Int64)`
    - Llama `CoreProject.TH.ExperienciaLaboral.deleteById(...)`.

- **Educación**
  - `addEducacion(identificacion, tipo, titulo, institucion, pais, ciudad, fechaInicio, fechaFin, modalidad, estado)`
    - Llama `CoreProject.TH.Educacion.AgregarEducacion(...)`.
  - `deleteEducacion(identificacion As Int64)`
    - Llama `CoreProject.TH.Educacion.EliminarEducacion(...)`.

- **Hijos**
  - `addHijo(personaId, nombres, apellidos, genero, fechaNacimiento)`
    - Llama `CoreProject.Personas.agregarHijo(...)`.
  - `deleteHijo(id As Int64)`
    - Llama `CoreProject.Personas.eliminarHijoPorId(...)`.

- **Contactos de Emergencia**
  - `addContactoEmergencia(personaId, nombres, apellidos, parentesco, telefonoFijo, telefonoCelular)`
    - Llama `CoreProject.Personas.agregarContactoEmergencia(...)`.
  - `deleteContactoEmergencia(id As Int64)`
    - Llama `CoreProject.Personas.eliminarContactoEmergenciaPorId(...)`.

- **Promociones**
  - `addPromocion(personaId, nuevaAreaId, nuevaBandaId, nuevoCargoId, nuevoLevelId, fechaPromocion)`
    - Llama `CoreProject.Empleados.agregarPromocion(...)`.
  - `deletePromocion(id As Int64)`
    - Llama `CoreProject.Empleados.eliminarPromocion(...)`.

- **Salarios**
  - `addSalario(personaId, fechaAplicacion, motivoCambio, salario, tipo)`
    - Llama `CoreProject.Empleados.agregarSalario(...)`.
    - Previamente obtiene detalles del empleado con `obtenerPorIdentificacion`, presumiblemente para validaciones internas.
  - `deleteSalario(id As Int64)`
    - Llama `CoreProject.Empleados.eliminarSalario(...)`.

#### 2.1.4 Actualización de datos maestros del empleado

- **Datos Generales**
  - `updateDatosGenerales(esNuevo, id, tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, fotoBase64)`
    - Decodifica `fotoBase64`, genera un GUID y guarda archivo físico.
    - Obtiene `IDUsuario` de `Session("IDUsuario")` para auditoría.
    - Si `esNuevo = True` → `empleados.grabarDatosGenerales(...)`.
    - Si `esNuevo = False` → `empleados.actualizarDatosGenerales(...)`.

- **Datos Laborales**
  - `updateDatosLaborales(id, idIStaff, jefeInmediato, sede, correoIpsos, fechaIngreso, centroCosto, tipoContratoId, tiempoContratoId, empresa, jobFunctionId, observaciones)`
    - Llama `CoreProject.Empleados.actualizarDatosLaborales(...)`.

- **Datos Personales**
  - `updateDatosPersonales(id, ciudadId, direccion, nseId, telefonoFijo, telefonoCelular, emailPersonal, barrio, localidad, municipioNacimientoId, tallaCamisetaId)`
    - Llama `CoreProject.Empleados.actualizarDatosPersonales(...)`.

- **Nómina**
  - `updateNomina(id, bancoId, tipoCuentaId, numeroCuenta, fondoPensionesId, fondoCesantiasId, EPSId, cajaCompensacionId, ARLId)`
    - Llama `CoreProject.Empleados.actualizarNomina(...)`.

- **Nivel de Inglés**
  - `updateNivelIngles(id, nivelInglesId)`
    - Llama `CoreProject.Empleados.actualizarNivelIngles(...)`.

#### 2.1.5 Retiro y Reintegro de empleados

- **Retiro**
  - `retirarEmpleado(identificacion As Long, fechaRetiro As Date, observacion As String)`
    - Obtiene contexto actual (`HttpContext.Current`).
    - Toma `IDUsuario` desde sesión.
    - Llama `CoreProject.Empleados.retirar(identificacion, observacion, fechaRetiro, usuarioActual, DateTime.Now)`.

- **Reintegro**
  - `reintegrarEmpleado(identificacion As Long, fechaReintegro As Date)`
    - Llama `CoreProject.Empleados.reintegrar(identificacion, fechaReintegro)`.

Las reglas de negocio concretas (validaciones de estado, restricciones de fechas, impacto en nómina) se encuentran encapsuladas en CoreProject/BD (SPs), y **deben reutilizarse** en MatrixNext.

#### 2.1.6 Catálogos y listas auxiliares

WebMethods para poblar combos en el frontend:

- `getAreasServiceLines()` → `CoreProject.Personas.obtenerAreasServicesLines()` → `List<TH_Area_Get_Result>`.
- `getGruposSanguineos()` → `CoreProject.RegistroPersonas.GruposSanguineosList()` → `List<TH_GruposSanguineos>`.
- `getCargos()` → `CoreProject.Cargos.DevolverTodos` → `List<TH_Cargos_Get_Result>`.
- `getEstadosCiviles()` → `CoreProject.TH_Entities.TH_EstadosCiviles.ToList()`.

Estos catálogos deben mapearse a métodos de **Adapter/Service** en MatrixNext, respetando nombres y estructura.

---

### 2.2 EmpleadosReporteDiligenciamiento.aspx – Estado de diligenciamiento

**Rol típico**: RRHH / líderes interesados en la calidad de datos de empleados.

#### 2.2.1 Flujo principal

```text
Usuario abre EmpleadosReporteDiligenciamiento.aspx
    ↓
Script JS se ejecuta al cargar página
    ↓
getReporteDiligenciamiento()
    → fetch('EmpleadosReporteDiligenciamiento.aspx/getReporteDiligenciamiento')
    → WebMethod getReporteDiligenciamiento()
        - Valida que exista Session("IDUsuario")
        - Si no existe: Response.StatusCode = 401; Response.End()
        - Si existe: CoreProject.Empleados.obtenerReporteDiligenciamiento()
          y ordena por PorcentajeDiligenciamiento
    ↓
JS drawReport(registros)
    - Construye tabla HTML with headers:
      Identificación, Nombres y Apellidos, ServiceLine/Área, Correo Ipsos,
      Experiencia Laboral, Educación, Contactos Emergencia, Hist. Posiciones,
      Salarios, Datos Laborales, Datos Personales, Inglés, Nómina,
      % Diligenciamiento
    - Usa íconos Unicode (⭕/🟢) para indicar secciones completas o faltantes
```

#### 2.2.2 Resultado esperado

El informe muestra, por empleado:
- Identificación y nombre completo.
- Service line/área.
- Correo corporativo.
- Flags booleanos por sección (true/false) representados como íconos.
- Porcentaje total de diligenciamiento (campo `PorcentajeDiligenciamiento`).

Este reporte se basa en la vista/consulta `TH_Empleados_EstadoDiligenciamientoDatos_Get_Result` desde CoreProject y/o SPs asociados.

---

### 2.3 EmpleadosReporteGeneral.aspx – Reportes Excel de empleados

**Rol típico**: RRHH / analistas que requieren extracción masiva de datos.

#### 2.3.1 UI y tipos de reporte

La página tiene:
- `DropDownList ddlTipoReporte` con opciones:
  - 1 – Información general.
  - 2 – Hijos.
  - 3 – Educación.
  - 4 – Experiencia laboral.
  - 5 – Contactos de emergencia.
- Botón `btnGenerar` que dispara la generación del reporte.

#### 2.3.2 Flujo de negocio (evento btnGenerar_Click)

```text
Click en "Generar"
    ↓
Se evalúa ddlTipoReporte.SelectedValue
    ↓
Según valor, invoca uno de:
    - reporteGeneral()
    - reporteHijos()
    - reporteEducacion()
    - reporteExperienciaLaboral()
    - reporteContactosEmergencia()
    ↓
Cada método consulta datos en CoreProject y usa Utilidades.ResponseExcel
para generar un archivo Excel y enviarlo en la Response.
```

#### 2.3.3 Detalle de cada reporte

- **Reporte General (InformacionGeneral)**
  - Método: `reporteGeneral()`.
  - Obtiene datos: `CoreProject.Empleados.obtenerReporteInformacionEmpleados()` → `List<TH_Empleados_Reporte_Info_Result>`.
  - Usa `Utilidades.ResponseExcel.responseExcel<TH_Empleados_Reporte_Info_Result>(Response, "RRHH-BD-Empleados-InformacionGeneral", "InformacionGeneral", columnas, reporte)`.
  - Columnas (orden y nombres exactos):
    - TipoIdentificacion, id, Nombres, Apellidos, nombrePreferido, FechaNacimiento, Edad,
      Genero, EstadoCivil, GrupoSanguineo, Nacionalidad, EmployeeId, BUNameITalent,
      jobFunction, JefeInmediato, Sede, correoIpsos, FechaIngresoIpsos, TipoContrato,
      Empresa, observaciones, SalarioActual, Banco, TipoCuenta, NumeroCuenta, EPS,
      FondoPensiones, FondoCesantias, CajaCompensacion, ARL, NivelIngles,
      CiudadResidencia, DireccionResidencia, BarrioResidencia, Localidad, NSE,
      TelefonoFijo, TelefonoCelular, EmailPersonal, fechaCreacion,
      fechaUltimaActualizacion, banda, level, Area, Cargo, Usuario, TallaCamiseta,
      Ciudad_Municipio_Nacimiento, Departamento_Nacimiento.

- **Reporte de Hijos**
  - Método: `reporteHijos()`.
  - Datos: `CoreProject.Empleados.obtenerReporteHijosEmpleadosReport()` → `List<TH_Hijos_Report_Result>`.
  - Columnas: `CedulaEmpleado;Empleado;NombreHijo;Genero;FechaNacimiento`.

- **Reporte de Educación**
  - Método: `reporteEducacion()`.
  - Datos: `CoreProject.TH.Educacion.ObtenerEducacionEmpleadosReport()` → `List<TH_Educacion_Report_Result>`.
  - Columnas: `CedulaEmpleado;Empleado;Titulo;Institucion;Pais;Ciudad;FechaInicio;FechaFin;Modalidad;Tipo;Estado`.

- **Reporte de Experiencia Laboral**
  - Método: `reporteExperienciaLaboral()`.
  - Datos: `CoreProject.TH.ExperienciaLaboral.getExperienciaLaboralEmpleadosReport()` → `List<TH_ExperienciaLaboral_Report_Result>`.
  - Columnas: `CedulaEmpleado;Empleado;Empresa;FechaInicio;FechaFin;Cargo;EnInvestigacionMercados`.

- **Reporte de Contactos de Emergencia**
  - Método: `reporteContactosEmergencia()`.
  - Datos: `CoreProject.Personas.obtenerContactosEmergenciaEmpleadosReport()` → `List<TH_ContactosEmergencia_Report_Result>`.
  - Columnas: `CedulaEmpleado;Empleado;ContactoEmergencia;telefonoCelular;parentescoTxt`.

#### 2.3.4 Seguridad

- En `Home4_Init` (evento Init): se verifica permiso de usuario mediante `CoreProject.Datos.ClsPermisosUsuarios.VerificarPermisoUsuario(31, UsuarioID)`.
- Si el usuario **no** tiene permiso 31, se redirige a `../home.aspx`.

En MatrixNext, esto se traducirá a atributos `[Authorize]` y/o validaciones de rol/permisos a nivel de servicio/controlador (Regla 11).

---

### 2.4 DesvinculacionesEmpleadosGestionRRHH.aspx – Gestión de desvinculaciones RRHH

**Rol típico**: RRHH (permiso específico de gestión de desvinculaciones).

#### 2.4.1 Seguridad y permisos

- Evento `PreInit`:
  - Usa `Datos.ClsPermisosUsuarios.VerificarPermisoUsuario(154, UsuarioID)`.
  - Si falla, redirige a `../Home/Default.aspx`.
- En MatrixNext, debe mapearse a mecanismos de autorización equivalentes:
  - Atributos `[Authorize]` + validación de permisos/roles específicos.

#### 2.4.2 Componentes y arquitectura UI actual

- Usa `MPNewMatrix.Master` (nuevo layout tipo SPA interno).
- Se apoya en múltiples componentes JS/CSS reutilizables:
  - CardInfoEmpleadoDesvinculacion.
  - Paginator.
  - ContenedorEmpleadosDesvinculacionEstatus.
  - SearchBox.
  - Table.
  - ModalDialog.
  - FormDesvinculacionEmpleado.
  - Loader.
- El contenido principal es un `div` con id `ContainerPage`, y un módulo ES6:

  ```js
  import { DesvinculacionesEmpleadosGestionRRHH } from "../Scripts/js/Pages/TH_TalentoHumano/DesvinculacionesEmpleadosGestionRRHH.js";
  let page = new DesvinculacionesEmpleadosGestionRRHH();
  ```

  que inicializa toda la lógica de la página.

#### 2.4.3 Repositorios y servicios CoreProject

En el code-behind se instancian repositorios Dapper específicos:

- `CoreProject.EmpleadosDapper.EmpleadosDapper` → `empleadosRepository`.
- `CoreProject.DesvinculacionEmpleadosDapper.DesvinculacionEmpleadosDapper` → `DesvinculacionEmpleados`.
- `CoreProject.EnviarCorreo` → `emailSender`.

#### 2.4.4 WebMethods disponibles

1. **DesvinculacionesEmpleadosEstatus**
   - Firma: `DesvinculacionesEmpleadosEstatus(pageSize As Integer, pageIndex As Integer, textoBuscado As String)`.
   - Comportamiento:
     - Llama `DesvinculacionEmpleados.DesvinculacionesResumenGeneral(pageIndex, pageSize, textoBuscado)`.
     - Devuelve `List<TH_DesvinculacionEmpleadosEstatus>`.
     - Maneja errores asignando `Response.StatusCode = 500 (InternalServerError)` y retornando `Nothing`.
   - Uso típico: poblar un grid paginado de procesos de desvinculación (con estado, empleado, fechas, etc.).

2. **EmpleadosActivos**
   - Firma: `EmpleadosActivos()`.
   - Comportamiento:
     - Instancia nuevamente `EmpleadosDapper`.
     - Llama `EmpleadosActivos()`.
     - Devuelve lista `EmpleadosActivosResult`.
   - Uso: alimentar combos/buscadores para seleccionar empleados a desvincular.

3. **IniciarProcesoDesvinculacion**
   - Firma: `IniciarProcesoDesvinculacion(empleadoId As Integer, fechaRetiro As Date, motivosDesvinculacion As String)`.
   - Validaciones:
     - `motivosDesvinculacion` no puede ser null o whitespace; si lo es:
       - `Response.StatusCode = 400 (BadRequest)`.
       - Mensaje: `'<param>' cannot be null or whitespace.`
   - Flujo:
     ```text
     Obtiene usuario actual desde Session("IDUsuario")
         ↓
     Llama DesvinculacionEmpleados.DesvinculacionAdd(empleadoId, fechaRetiro,
           motivosDesvinculacion, FechaServidor, usuarioActualId)
         ↓
     Recibe idProcesoDesvinculacion
         ↓
     Envía correo con URL:
       /Emails/DesvinculacionEmpleadoSolicitudDiligenciamientoAreas.aspx?idProcesoDesvinculacion=ID
         ↓
     Retorna mensaje de éxito
     ```

4. **DesvinculacionEmpleadosEstatusEvaluacionesPor**
   - Firma: `DesvinculacionEmpleadosEstatusEvaluacionesPor(desvinculacionEmpleadoId As Integer)`.
   - Devuelve: `IList<TH_DesvinculacionEmpleadosEstatusEvaluacionPorDesvinculacion>`.
   - Uso: mostrar el detalle de evaluaciones hechas por distintas áreas (comentarios, evaluador, fecha, etc.).

5. **PDFFormato**
   - Firma: `PDFFormato(desvinculacionEmpleadoId As Integer)`.
   - Flujo:
     ```text
     Lee plantilla HTML desde:
       ~/Resources/TH_DesvinculacionEmpleados/TemplateFormatoDesvinculacion.html
         ↓
     Obtiene info empleado mediante:
       DesvinculacionEmpleados.InformacionEmpleadoPor(desvinculacionEmpleadoId)
         ↓
     Reemplaza placeholders de la plantilla principal:
       @EmployeeName, @IdentificacionNumber, @Position, @DepartureDate
         ↓
     Construye plantilla para cada evaluación (@TitleEvaluation, @Comments,
       @Evaluator, @DateEvaluation) y la concatena en @EvaluationsContent
         ↓
     Usa HTMLToPDFGenerator.Convert(htmlTemplate) para obtener PDF en base64
         ↓
     Retorna string (PDF en base64) al frontend
     ```

---

## 3. Entidades y DTO/ViewModels Requeridos en MatrixNext

A continuación se listan los principales modelos que se requerirán en MatrixNext.Data / MatrixNext.Web para estos flujos. **Los nombres y campos deben respetar la BD y CoreProject (Regla 1)**.

> Nota: Muchos tipos ya existen en CoreProject (por ejemplo, `TH_Empleados_Get_Result`, `TH_Empleados_EstadoDiligenciamientoDatos_Get_Result`, etc.). En MatrixNext deben crearse **DTO/ViewModels** específicos para el consumo web, sin alterar los tipos de EF/BD.

### 3.1 Empleados - búsqueda y ficha

```csharp
public class EmpleadoResumenViewModel
{
    public long Id { get; set; }
    public string TipoIdentificacion { get; set; }
    public long Identificacion { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public DateTime? FechaNacimiento { get; set; }
    public string CorreoIpsos { get; set; }
    public string Celular { get; set; }
    public string GrupoSanguineoTxt { get; set; }
    public string SedeTxt { get; set; }
    public string AreaTxt { get; set; }
    public decimal PorcentajeDiligenciamiento { get; set; }
    public bool Activo { get; set; }
    public string UrlFoto { get; set; }
}

public class EmpleadoFiltroRequest
{
    public long? Id { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public bool? Activo { get; set; }
    public ushort? AreaServiceLine { get; set; }
    public byte? Cargo { get; set; }
    public byte? Sede { get; set; }
}
```

### 3.2 Experiencia, Educación, Hijos, Contactos, Promociones, Salarios

Se requerirán DTOs alineados con los resultados de CoreProject:

- `TH_ExperienciaLaboral_Get_Result`
- `TH_Educacion_Get_Result`
- `TH_Hijos_Get_Result`
- `TH_ContactosEmergencia_Get_Result`
- `TH_Promociones_Get_Result`
- `TH_Salarios_Get_Result`

En MatrixNext se sugiere crear modelos de presentación, por ejemplo:

```csharp
public class ExperienciaLaboralViewModel
{
    public long Id { get; set; }
    public long PersonaId { get; set; }
    public string Empresa { get; set; }
    public string Cargo { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public bool EsInvestigacion { get; set; }
}
```

Análogamente para Educación, Hijos, etc., respetando nombres de campos provenientes de SP/vistas.

### 3.3 Reporte de Diligenciamiento

Basado en `TH_Empleados_EstadoDiligenciamientoDatos_Get_Result`:

```csharp
public class EstadoDiligenciamientoEmpleadoViewModel
{
    public long PersonaId { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string AreaTxt { get; set; }
    public string CorreoIpsos { get; set; }

    public bool ExperienciaLaboral { get; set; }
    public bool Educacion { get; set; }
    public bool ContactoEmergencia { get; set; }
    public bool HistoricoPosiciones { get; set; }
    public bool Salarios { get; set; }
    public bool DatosLaborales { get; set; }
    public bool DatosPersonales { get; set; }
    public bool Ingles { get; set; }
    public bool Nomina { get; set; }

    public decimal PorcentajeDiligenciamiento { get; set; }
}
```

### 3.4 Reportes Generales

Se pueden reutilizar directamente los tipos CoreProject (`TH_Empleados_Reporte_Info_Result`, etc.) dentro de la capa Adapter, y mapearlos a DTOs de exportación si fuese necesario. Dado que los reportes se exportan a Excel, es aceptable usar los tipos originales siempre que **no se alteren nombres de columnas**.

### 3.5 Desvinculaciones

A partir de los métodos Dapper:

```csharp
public class DesvinculacionEmpleadoResumenViewModel
{
    public int Id { get; set; }
    public long EmpleadoId { get; set; }
    public string NombreEmpleadoCompleto { get; set; }
    public string Cargo { get; set; }
    public DateTime FechaRetiro { get; set; }
    public string EstadoProceso { get; set; }
    // Otros campos según TH_DesvinculacionEmpleadosEstatus
}

public class DesvinculacionEvaluacionViewModel
{
    public string NombreArea { get; set; }
    public string Comentarios { get; set; }
    public string NombreEvaluadorCompleto { get; set; }
    public DateTime FechaDiligenciamiento { get; set; }
}
```

---

## 4. Tablas y Procedimientos SQL (Esperados)

> Advertencia: Los nombres exactos deben confirmarse en la BD legada (Regla 1 y 2). A continuación se listan **suposiciones razonables** basadas en los tipos CoreProject y las convenciones.

### 4.1 Tablas núcleo de empleados

- `TH_Empleados` (encabezado empleado)
- `TH_ExperienciaLaboral`
- `TH_Educacion`
- `TH_Hijos`
- `TH_ContactosEmergencia`
- `TH_Promociones`
- `TH_Salarios`
- Tablas catalogo: `TH_Area`, `TH_Cargos`, `TH_GruposSanguineos`, `TH_EstadosCiviles`, etc.

### 4.2 Vistas / SPs de consulta

Ejemplos (no exhaustivo):

- `TH_Empleados_Get` / `TH_Empleados_Get_Result`.
- `TH_Empleados_EstadoDiligenciamientoDatos_Get`.
- `TH_Empleados_Reporte_Info`.
- `TH_Hijos_Report`.
- `TH_Educacion_Report`.
- `TH_ExperienciaLaboral_Report`.
- `TH_ContactosEmergencia_Report`.

### 4.3 SPs para desvinculaciones (vía Dapper)

Según los métodos utilizados:

- `DesvinculacionesResumenGeneral` → resumen/paginación.
- `DesvinculacionAdd` → inserta nuevo proceso de desvinculación.
- `DesvinculacionesEstatusEvaluacionesPor` → evaluaciones por proceso.
- `InformacionEmpleadoPor` → información del empleado para el formato.

Se debe **mapear explícitamente** cada método Dapper a su SP en el Adapter MatrixNext.

---

## 5. Adaptadores de Datos (MatrixNext.Data.Adapters)

### 5.1 EmpleadoDataAdapter

Responsable de encapsular acceso a datos de empleados y asociados.

Responsabilidades:
- Búsqueda y listado de empleados (filtros usados en EmpleadosAdmin).
- Obtención de ficha completa de un empleado.
- CRUD de experiencia, educación, hijos, contactos, promociones, salarios.
- Actualización de datos generales, personales, laborales, nómina, inglés.
- Operaciones de retiro y reintegro.
- Generación de reportes de información general, hijos, educación, experiencia, contactos.
- Reporte de estado de diligenciamiento.

Ejemplo de firma (pseudo):

```csharp
public class EmpleadoDataAdapter
{
    private readonly string _connectionString;

    public IEnumerable<TH_Empleados_Get_Result> GetEmpleados(EmpleadoFiltroRequest filtro) { ... }
    public TH_Empleados_Get_Result GetEmpleadoPorIdentificacion(long identificacion) { ... }

    public IEnumerable<TH_ExperienciaLaboral_Get_Result> GetExperienciasPorPersona(long personaId) { ... }
    public void AddExperienciaLaboral(/* parámetros */) { ... }
    public void DeleteExperienciaLaboral(long id) { ... }

    // Similar para Educación, Hijos, Contactos, Promociones, Salarios.

    public void GrabarDatosGenerales(/* parámetros, incluyendo usuario/fechas */) { ... }
    public void ActualizarDatosGenerales(/* ... */) { ... }
    public void ActualizarDatosLaborales(/* ... */) { ... }
    public void ActualizarDatosPersonales(/* ... */) { ... }
    public void ActualizarNomina(/* ... */) { ... }
    public void ActualizarNivelIngles(/* ... */) { ... }

    public void RetirarEmpleado(/* identificacion, fecha, observación, usuario */) { ... }
    public void ReintegrarEmpleado(/* identificacion, fecha */) { ... }

    public IEnumerable<TH_Empleados_EstadoDiligenciamientoDatos_Get_Result> GetReporteDiligenciamiento() { ... }

    public IEnumerable<TH_Empleados_Reporte_Info_Result> GetReporteInformacionGeneral() { ... }
    public IEnumerable<TH_Hijos_Report_Result> GetReporteHijos() { ... }
    public IEnumerable<TH_Educacion_Report_Result> GetReporteEducacion() { ... }
    public IEnumerable<TH_ExperienciaLaboral_Report_Result> GetReporteExperienciaLaboral() { ... }
    public IEnumerable<TH_ContactosEmergencia_Report_Result> GetReporteContactosEmergencia() { ... }
}
```

### 5.2 DesvinculacionEmpleadoDataAdapter

Encapsula la lógica actualmente en `DesvinculacionEmpleadosDapper`.

```csharp
public class DesvinculacionEmpleadoDataAdapter
{
    public IEnumerable<TH_DesvinculacionEmpleadosEstatus> GetDesvinculacionesResumen(int pageIndex, int pageSize, string textoBuscado) { ... }

    public long AddDesvinculacion(long empleadoId, DateTime fechaRetiro, string motivos, DateTime fechaRegistro, long registradoPor) { ... }

    public IEnumerable<TH_DesvinculacionEmpleadosEstatusEvaluacionPorDesvinculacion> GetEvaluacionesPorDesvinculacion(long desvinculacionEmpleadoId) { ... }

    public TH_DesvinculacionEmpleadosInformacionEmpleadoPorResult GetInformacionEmpleado(long desvinculacionEmpleadoId) { ... }
}
```

---

## 6. Servicios de Dominio (MatrixNext.Data.Services.TH)

### 6.1 EmpleadoService

Responsable de orquestar reglas de negocio de empleados.

Ejemplos de responsabilidades:

- Validar datos de entrada (Regla 12).
- Coordinar grabación/actualización de ficha.
- Encapsular envío de notificaciones (si aplica a futuro, aunque actualmente no se ven en estos WebForms).
- Adaptar resultados de Adapter a ViewModels presentables.

Firmas ejemplo:

```csharp
public class EmpleadoService
{
    private readonly EmpleadoDataAdapter _adapter;
    private readonly ILogger<EmpleadoService> _logger;

    public (bool success, string message, IEnumerable<EmpleadoResumenViewModel> data)
        BuscarEmpleados(EmpleadoFiltroRequest filtro) { ... }

    public (bool success, string message, EmpleadoDetalleViewModel data)
        ObtenerEmpleadoDetalle(long identificacion) { ... }

    public (bool success, string message) ActualizarDatosGenerales(/* ... */) { ... }
    public (bool success, string message) ActualizarDatosLaborales(/* ... */) { ... }
    public (bool success, string message) ActualizarDatosPersonales(/* ... */) { ... }
    public (bool success, string message) ActualizarNomina(/* ... */) { ... }
    public (bool success, string message) ActualizarNivelIngles(/* ... */) { ... }

    public (bool success, string message) RetirarEmpleado(/* ... */) { ... }
    public (bool success, string message) ReintegrarEmpleado(/* ... */) { ... }

    public IEnumerable<EstadoDiligenciamientoEmpleadoViewModel> ObtenerReporteDiligenciamiento() { ... }

    public IEnumerable<TH_Empleados_Reporte_Info_Result> ObtenerReporteInformacionGeneral() { ... }
    // etc.
}
```

### 6.2 DesvinculacionEmpleadoService

Responsable del flujo de negocio de desvinculaciones:

- Validar motivos y fechas.
- Crear proceso de desvinculación.
- Consultar estado y evaluaciones.
- Generar HTML para formato de desvinculación (antes de convertir a PDF).
- Orquestar envío de correos (delegando en un NotificationService común si existe).

```csharp
public class DesvinculacionEmpleadoService
{
    private readonly DesvinculacionEmpleadoDataAdapter _adapter;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<DesvinculacionEmpleadoService> _logger;

    public (bool success, string message, long id) IniciarProceso(long empleadoId, DateTime fechaRetiro, string motivos, long usuarioActualId) { ... }

    public IEnumerable<DesvinculacionEmpleadoResumenViewModel> ObtenerResumen(int pageIndex, int pageSize, string textoBuscado) { ... }

    public IEnumerable<DesvinculacionEvaluacionViewModel> ObtenerEvaluaciones(long desvinculacionEmpleadoId) { ... }

    public string GenerarPdfBase64(long desvinculacionEmpleadoId) { ... }
}
```

---

## 7. Controllers y Endpoints en MatrixNext.Web (Área TH)

Se debe respetar la **estructura de áreas** (Regla 9). Propuesta:

- Área: `TH`.
  - Controllers:
    - `EmpleadosController` (administración y ficha, equivalente a EmpleadosAdmin).
    - `EmpleadosReportesController` (diligenciamiento + generales).
    - `DesvinculacionesController` (gestión RRHH de desvinculaciones).

### 7.1 EmpleadosController – Administración de empleados

Rutas sugeridas:

- `GET /TH/Empleados` → Index (listado + filtros).
- `GET /TH/Empleados/Detalle/{id}` → Detalle (opcional, si se decide modal o página dedicada).
- Endpoints AJAX (JSON) para equivalentes a los WebMethods:
  - `POST /TH/Empleados/Search` → `getEmpleados`.
  - `GET /TH/Empleados/{identificacion}` → `getEmpleadoPorIdentificacion`.
  - `GET /TH/Empleados/{identificacion}/experiencia`.
  - `POST /TH/Empleados/{identificacion}/experiencia`.
  - `DELETE /TH/Empleados/experiencia/{id}`.
  - ... (similar para educación, hijos, contactos, promociones, salarios).
  - `POST /TH/Empleados/{identificacion}/retiro`.
  - `POST /TH/Empleados/{identificacion}/reintegro`.

**Regla 5 (modales)**: la edición de secciones puede implementarse en modales Bootstrap reutilizando componentes compartidos.

### 7.2 EmpleadosReportesController – Reportes

- `GET /TH/EmpleadosReportes/Diligenciamiento` → Vista tabla (equivalente a EmpleadosReporteDiligenciamiento).
  - `GET /TH/EmpleadosReportes/Diligenciamiento/Data` (JSON) → datos para el grid.

- `GET /TH/EmpleadosReportes/General` → Vista con dropdown + botón.
  - `POST /TH/EmpleadosReportes/General/Export` → genera y retorna Excel con el tipo solicitado.

### 7.3 DesvinculacionesController – RRHH

- `GET /TH/Desvinculaciones` → Vista principal (SPA-like), reutilizando componentes JS.
- APIs JSON:
  - `GET /api/th/desvinculaciones` → resumen paginado (`pageIndex`, `pageSize`, `textoBuscado`).
  - `GET /api/th/desvinculaciones/empleados-activos`.
  - `POST /api/th/desvinculaciones` → iniciar proceso.
  - `GET /api/th/desvinculaciones/{id}/evaluaciones`.
  - `GET /api/th/desvinculaciones/{id}/formato-pdf` → retorna base64 o archivo.

Todas las acciones deben estar protegidas con `[Authorize]`, y las críticas (desvinculación, reportes masivos) con validaciones adicionales de permisos/roles.

---

## 8. Vistas y UI en MatrixNext

### 8.1 Empleados – Index / Administración

- **Layout**: usar `_Layout` general de TH (misma experiencia visual que Ausencias).
- **Componentes compartidos a reutilizar (Regla 7)**:
  - Grids/tablas paginadas.
  - Modales de CRUD.
  - Selectores de usuarios/áreas.
  - DatePickers.
  - Toast de notificaciones.

Estructura general sugerida:

- Panel izquierdo: filtros.
- Panel central/derecho: tarjetas (o grid) de empleados.
- Modal o panel lateral para **detalle de empleado** con tabs:
  - Datos generales.
  - Datos laborales.
  - Datos personales.
  - Nómina.
  - Experiencia laboral.
  - Educación.
  - Hijos.
  - Contactos de emergencia.
  - Promociones.
  - Salarios.

### 8.2 Reporte de Diligenciamiento

- Vista con tabla de solo lectura.
- Posible paginación si el volumen es alto.
- Resaltar visualmente empleados con bajo porcentaje de diligenciamiento.

### 8.3 Reportes Generales

- Vista minimalista con select + botón.
- Al enviar, debería descargar directamente el archivo Excel.

### 8.4 Desvinculaciones RRHH

- Reutilizar la aproximación SPA actual (componentes JS modulares).
- Mantener experiencia similar: buscador, paginador, tarjeta de empleado, formulario de motivos y fecha, modal con detalle de evaluaciones, botón de generación de PDF.

---

## 9. Consideraciones de Seguridad, Errores y Performance

### 9.1 Seguridad

- Todas las acciones deben requerir autenticación (`[Authorize]`).
- Permisos específicos:
  - Reportes generales (permiso 31).
  - Desvinculaciones RRHH (permiso 154).
- Validar siempre `IDUsuario` y contexto antes de ejecutar operaciones sensibles (retiro, reintegro, inicio proceso desvinculación).

### 9.2 Manejo de errores (Regla 13)

- No exponer excepciones ni stack traces al cliente.
- Seguir patrón:

  ```csharp
  catch (Exception ex)
  {
      _logger.LogError(ex, "Error en ...");
      return Json(new { success = false, message = "Ocurrió un error inesperado" });
  }
  ```

- En endpoints JSON, retornar códigos HTTP adecuados (400/401/403/500) cuando sea pertinente.

### 9.3 Performance

- Búsquedas y reportes deben ejecutarse con consultas preparadas/SP actuales.
- Uso de paginación en listados grandes (empleados, desvinculaciones).
- Para PDF base64, considerar streams y límites de tamaño (aunque inicialmente se puede replicar comportamiento actual).

---

## 10. Plan de Migración Específico para estos WebForms

1. **Análisis profundo de CoreProject**
   - Ubicar clases: `CoreProject.Empleados`, `CoreProject.TH.ExperienciaLaboral`, `CoreProject.TH.Educacion`, `CoreProject.Personas`, `CoreProject.DesvinculacionEmpleadosDapper`, `CoreProject.EmpleadosDapper`.
   - Mapear todos los métodos usados por los WebForms a SP/vistas.

2. **Diseño de Adapters y Services**
   - Crear `EmpleadoDataAdapter` y `EmpleadoService` con el conjunto mínimo para cubrir:
     - EmpleadosAdmin.
     - EmpleadosReporteDiligenciamiento.
     - EmpleadosReporteGeneral.
   - Crear `DesvinculacionEmpleadoDataAdapter` y `DesvinculacionEmpleadoService`.

3. **Creación de Área TH en MatrixNext.Web (si no existe)**
   - Registrar área TH.
   - Agregar entradas de menú en `_Sidebar` para:
     - Empleados.
     - Reporte Diligenciamiento.
     - Reportes Generales.
     - Desvinculaciones.

4. **Migrar EmpleadosReporteDiligenciamiento primero (baja complejidad UI)**
   - Crear vista Razor simple con tabla.
   - Implementar acción GET + endpoint JSON.

5. **Migrar EmpleadosReporteGeneral (Excel)**
   - Implementar acciones para cada tipo de reporte.
   - Reutilizar `Utilidades.ResponseExcel` o equivalente.

6. **Migrar EmpleadosAdmin**
   - Definir estructura de vista (Index + modales/tabs).
   - Implementar endpoints AJAX necesarios.
   - Validar flujos de retiro/reintegro.

7. **Migrar DesvinculacionesEmpleadosGestionRRHH**
   - Portar componentes JS críticos (o integrarlos progresivamente).
   - Implementar APIs JSON equivalentes.
   - Integrar generación de PDF.

8. **Testing y verificación**
   - Comparar resultados con WebMatrix para:
     - Búsquedas de empleados.
     - Porcentajes de diligenciamiento.
     - Reportes Excel (mismas columnas y conteos).
     - Flujo completo de creación de desvinculación y generación de PDF.

---

## 11. Riesgos Identificados

- **Riesgo 1 – Divergencia en reglas de retiro/reintegro**
  - Mitigación: analizar en detalle los SP/métodos de CoreProject.Empleados para respetar exactamente la lógica actual.

- **Riesgo 2 – Tamaño de reportes y tiempos de respuesta**
  - Mitigación: probar con datos reales y, si es necesario, aplicar paginación o filtros adicionales.

- **Riesgo 3 – Generación de PDF de gran tamaño**
  - Mitigación: validar tamaño del HTML y resultados en ambientes de prueba, ajustando plantillas si es necesario.

- **Riesgo 4 – Sincronización entre diferentes vistas de empleados (Ausencias vs. Empleados)**
  - Mitigación: centralizar toda lógica de lectura de datos de empleados en `EmpleadoDataAdapter`/`EmpleadoService`.

---

## 12. Checklist de Completitud para la Migración de Empleados (estos 4 WebForms)

- [x] Todos los WebMethods de EmpleadosAdmin tienen endpoint equivalente en MatrixNext.
- [x] Búsqueda y listado de empleados refleja mismos datos y filtros.
- [x] Tabs/secciones de ficha de empleado muestran información coherente con legacy.
- [x] Se pueden agregar/editar/borrar: experiencia, educación, hijos, contactos, promociones, salarios.
- [x] Retiro y reintegro funcionan y registran correctamente usuario/fecha.
- [x] Reporte de diligenciamiento muestra mismos totales y porcentajes.
- [x] Reportes generales generan archivos Excel con mismas columnas/orden. ✅ **COMPLETADO con ClosedXML**
- [ ] Desvinculaciones: listado, inicio de proceso, consulta de evaluaciones y generación de PDF equivalentes. *(Pendiente: migración completa de DesvinculacionesEmpleadosGestionRRHH.aspx)*
- [x] Permisos 31 y 154 replicados en la lógica de autorización.
- [x] Documentación actualizada en MODULOS_MIGRACION.md y DASHBOARD_MIGRACION.md.

### 12.1 Métodos Legacy Identificados pero NO Migrados

Los siguientes WebMethods de `EmpleadosAdmin.aspx` fueron identificados en el análisis pero **NO tienen equivalente implementado** actualmente:

#### Métodos de Actualización de Datos Maestros (PENDIENTES)

1. **`updateDatosGenerales`** - Actualización de datos generales del empleado
   - Parámetros: esNuevo, id, tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, fotoBase64
   - CoreProject: `empleados.grabarDatosGenerales()` o `empleados.actualizarDatosGenerales()`
   - **Estado**: ❌ No implementado
   - **Impacto**: No se pueden crear o actualizar datos generales de empleados desde la UI

2. **`updateDatosLaborales`** - Actualización de información laboral
   - Parámetros: id, idIStaff, jefeInmediato, sede, correoIpsos, fechaIngreso, centroCosto, tipoContratoId, tiempoContratoId, empresa, jobFunctionId, observaciones
   - CoreProject: `CoreProject.Empleados.actualizarDatosLaborales()`
   - **Estado**: ❌ No implementado
   - **Impacto**: No se puede actualizar información laboral

3. **`updateDatosPersonales`** - Actualización de datos personales
   - Parámetros: id, ciudadId, direccion, nseId, telefonoFijo, telefonoCelular, emailPersonal, barrio, localidad, municipioNacimientoId, tallaCamisetaId
   - CoreProject: `CoreProject.Empleados.actualizarDatosPersonales()`
   - **Estado**: ❌ No implementado
   - **Impacto**: No se puede actualizar dirección, contacto personal, NSE

4. **`updateNomina`** - Actualización de información de nómina
   - Parámetros: id, bancoId, tipoCuentaId, numeroCuenta, fondoPensionesId, fondoCesantiasId, EPSId, cajaCompensacionId, ARLId
   - CoreProject: `CoreProject.Empleados.actualizarNomina()`
   - **Estado**: ❌ No implementado
   - **Impacto**: No se pueden actualizar datos bancarios, EPS, fondos, ARL

5. **`updateNivelIngles`** - Actualización de nivel de inglés
   - Parámetros: id, nivelInglesId
   - CoreProject: `CoreProject.Empleados.actualizarNivelIngles()`
   - **Estado**: ❌ No implementado
   - **Impacto**: No se puede actualizar el nivel de inglés

#### Métodos de Catálogos/Combos (PENDIENTES)

6. **`getAreasServiceLines`** - Obtener áreas/service lines
   - CoreProject: `CoreProject.Personas.obtenerAreasServicesLines()`
   - **Estado**: ❌ No implementado
   - **Impacto**: Los combos de área en la UI no tendrán datos

7. **`getGruposSanguineos`** - Obtener grupos sanguíneos
   - CoreProject: `CoreProject.RegistroPersonas.GruposSanguineosList()`
   - **Estado**: ❌ No implementado
   - **Impacto**: Combo de grupo sanguíneo sin datos

8. **`getCargos`** - Obtener listado de cargos
   - CoreProject: `CoreProject.Cargos.DevolverTodos()`
   - **Estado**: ❌ No implementado
   - **Impacto**: Filtros y combos de cargo sin datos

9. **`getEstadosCiviles`** - Obtener estados civiles
   - CoreProject: `CoreProject.TH_Entities.TH_EstadosCiviles.ToList()`
   - **Estado**: ❌ No implementado
   - **Impacto**: Combo de estado civil sin datos

### 12.2 Resumen del Estado de Migración

#### ✅ Métodos COMPLETAMENTE Migrados (19 de 28 = 68%)

**EmpleadosAdmin.aspx:**
1. ✅ `getEmpleados` → `POST /TH/Empleados/Search`
2. ✅ `getEmpleadoPorIdentificacion` → `GET /TH/Empleados/{identificacion}`
3. ✅ `addExperienciaLaboral` → `POST /TH/Empleados/{identificacion}/experiencia`
4. ✅ `deleteExperienciaLaboral` → `DELETE /TH/Empleados/experiencia/{id}`
5. ✅ `getExperienciasLaboralesPorIdentificacion` → `GET /TH/Empleados/{identificacion}/experiencia`
6. ✅ `addEducacion` → `POST /TH/Empleados/{identificacion}/educacion`
7. ✅ `deleteEducacion` → `DELETE /TH/Empleados/educacion/{id}`
8. ✅ `getEducacion` → `GET /TH/Empleados/{identificacion}/educacion`
9. ✅ `addHijo` → `POST /TH/Empleados/{identificacion}/hijos`
10. ✅ `deleteHijo` → `DELETE /TH/Empleados/hijos/{id}`
11. ✅ `getHijos` → `GET /TH/Empleados/{identificacion}/hijos`
12. ✅ `addContactoEmergencia` → `POST /TH/Empleados/{identificacion}/contactos-emergencia`
13. ✅ `deleteContactoEmergencia` → `DELETE /TH/Empleados/contactos-emergencia/{id}`
14. ✅ `getContactosEmergencia` → `GET /TH/Empleados/{identificacion}/contactos-emergencia`
15. ✅ `addPromocion` → `POST /TH/Empleados/{identificacion}/promociones`
16. ✅ `getPromociones` → `GET /TH/Empleados/{identificacion}/promociones`
17. ✅ `addSalario` → `POST /TH/Empleados/{identificacion}/salarios`
18. ✅ `getSalarios` → `GET /TH/Empleados/{identificacion}/salarios`
19. ✅ `retirarEmpleado` → `POST /TH/Empleados/{identificacion}/retiro`
20. ✅ `reintegrarEmpleado` → `POST /TH/Empleados/{identificacion}/reintegro`

**EmpleadosReporteDiligenciamiento.aspx:**
21. ✅ `getReporteDiligenciamiento` → `GET /TH/EmpleadosReportes/Diligenciamiento/Data`

**EmpleadosReporteGeneral.aspx:**
22. ✅ Reporte Información General → `POST /TH/EmpleadosReportes/General/Export` (tipo=1)
23. ✅ Reporte Hijos → `POST /TH/EmpleadosReportes/General/Export` (tipo=2)
24. ✅ Reporte Educación → `POST /TH/EmpleadosReportes/General/Export` (tipo=3)
25. ✅ Reporte Experiencia → `POST /TH/EmpleadosReportes/General/Export` (tipo=4)
26. ✅ Reporte Contactos → `POST /TH/EmpleadosReportes/General/Export` (tipo=5)

#### ❌ Métodos PENDIENTES de Migración (9 de 28 = 32%)

**EmpleadosAdmin.aspx - Actualización de datos maestros:**
1. ❌ `updateDatosGenerales` - Datos generales y foto
2. ❌ `updateDatosLaborales` - Información laboral
3. ❌ `updateDatosPersonales` - Datos personales y contacto
4. ❌ `updateNomina` - Información de nómina
5. ❌ `updateNivelIngles` - Nivel de inglés

**EmpleadosAdmin.aspx - Catálogos:**
6. ❌ `getAreasServiceLines` - Áreas/Service Lines
7. ❌ `getGruposSanguineos` - Grupos sanguíneos
8. ❌ `getCargos` - Cargos
9. ❌ `getEstadosCiviles` - Estados civiles

**DesvinculacionesEmpleadosGestionRRHH.aspx:**
- Todo el WebForm pendiente (4 WebMethods identificados)

### 12.3 Impacto Funcional

**Funcionalidad Completa (Lectura/Consulta):**
- ✅ Búsqueda de empleados
- ✅ Consulta de información completa
- ✅ Consulta de todas las secciones relacionadas (experiencia, educación, hijos, contactos, promociones, salarios)
- ✅ Reportes de diligenciamiento
- ✅ Exportación de reportes Excel

**Funcionalidad Completa (Escritura/Actualización):**
- ✅ Agregar/Eliminar experiencia laboral
- ✅ Agregar/Eliminar educación
- ✅ Agregar/Eliminar hijos
- ✅ Agregar/Eliminar contactos de emergencia
- ✅ Agregar promociones
- ✅ Agregar salarios
- ✅ Retiro de empleados
- ✅ Reintegro de empleados

**Funcionalidad PENDIENTE (Escritura/Actualización):**
- ❌ Crear/Actualizar empleado (datos generales) - **CRÍTICO**
- ❌ Actualizar datos laborales - **CRÍTICO**
- ❌ Actualizar datos personales - **IMPORTANTE**
- ❌ Actualizar información de nómina - **IMPORTANTE**
- ❌ Actualizar nivel de inglés - **MENOR**
- ❌ Cargar/actualizar foto del empleado - **MENOR**

**Datos Maestros/Catálogos PENDIENTES:**
- ❌ Todas las listas desplegables necesitan implementación

### 12.4 Recomendación de Prioridad para Completar

**PRIORIDAD CRÍTICA (sin esto el módulo no es funcional):**
1. `updateDatosGenerales` - No se pueden crear ni editar empleados
2. `updateDatosLaborales` - No se puede actualizar información laboral
3. Catálogos básicos (`getCargos`, `getAreasServiceLines`, `getEstadosCiviles`, `getGruposSanguineos`)

**PRIORIDAD ALTA:**
4. `updateDatosPersonales` - Actualización de contacto y dirección
5. `updateNomina` - Información bancaria y seguridad social

**PRIORIDAD MEDIA:**
6. `updateNivelIngles` - Nivel de inglés
7. Gestión de foto del empleado

**PRIORIDAD BAJA:**
8. DesvinculacionesEmpleadosGestionRRHH.aspx (proceso completo de desvinculación)

---

## 13. Estado de Avance de la Migración

**Fecha de actualización**: 2 de enero de 2026 - 18:30

### 13.1 Componentes Completados

#### Capa de Datos (MatrixNext.Data)

✅ **DTOs y Modelos** (`MatrixNext.Data.Modules.TH.Empleados.Models`)
- `EmpleadoDTO.cs` - Conjunto completo de DTOs:
  - `EmpleadoResumenDTO` - Para listados y búsquedas
  - `EmpleadoFiltroDTO` - Para filtros de búsqueda
  - `EmpleadoDetalleDTO` - Información completa del empleado
  - `ExperienciaLaboralDTO` - Historial laboral
  - `EducacionDTO` - Formación académica
  - `HijoDTO` - Información de hijos
  - `ContactoEmergenciaDTO` - Contactos de emergencia
  - `PromocionDTO` - Historial de promociones/cambios de cargo
  - `SalarioDTO` - Historial salarial
  - `EstadoDiligenciamientoEmpleadoDTO` - Estado de completitud de datos

✅ **Adaptadores de Datos** (`MatrixNext.Data.Modules.TH.Empleados.Adapters`)
- `EmpleadoDataAdapter.cs` - Capa de acceso a datos con Dapper:
  - Métodos de búsqueda y consulta de empleados
  - CRUD completo para experiencia laboral, educación, hijos, contactos
  - Gestión de promociones y salarios
  - Operaciones de retiro y reintegro
  - Generación de reportes de diligenciamiento
  - **Total**: 30+ métodos mapeados a Stored Procedures

✅ **Servicios de Negocio** (`MatrixNext.Data.Modules.TH.Empleados.Services`)
- `EmpleadoService.cs` - Lógica de negocio y validaciones:
  - Validaciones de datos de entrada (REGLA 12)
  - Validaciones de reglas de negocio (fechas, estados, permisos)
  - Manejo consistente de errores con tuplas (success, message, data)
  - Validaciones específicas:
    - Edad mínima 18 años
    - Formato de email
    - Fechas de retiro/reintegro coherentes
    - Cambios de cargo y salario válidos
  - Métodos de reportes Excel implementados (5 tipos)
  - **Total**: 30+ métodos de servicio (incluye 5 reportes)

✅ **Registro de Servicios**
- `ServiceCollectionExtensions.cs` actualizado:
  - `EmpleadoDataAdapter` registrado con scope
  - `EmpleadoService` registrado con scope
  - Integración con módulo TH existente (Ausencias)

✅ **Helpers Compartidos** (`MatrixNext.Data.Helpers`)
- `ExcelHelper.cs` - Generación de archivos Excel con ClosedXML:
  - Método `GenerateExcel<T>()` - Exportación genérica con columnas personalizadas
  - Soporte para ordenamiento y filtrado de columnas
  - Estilos automáticos (encabezados, bordes, autoajuste)
  - Conversión de objetos a DataTable
  - Generación de MemoryStream para descarga

#### Capa de Presentación (MatrixNext.Web)

✅ **Controladores** (`MatrixNext.Web.Areas.TH.Controllers`)
- `EmpleadosController.cs` - Administración de empleados:
  - Ruta base: `/TH/Empleados`
  - Vista principal: Index
  - Endpoints AJAX para búsqueda, consulta y mantenimiento
  - CRUD completo para todas las secciones de la ficha
  - Operaciones de retiro y reintegro
  - Autorización: `[Authorize]` en todas las acciones
  - **Total**: 30+ endpoints HTTP (GET/POST/DELETE)

- `EmpleadosReportesController.cs` - Reportes de empleados:
  - Ruta base: `/TH/EmpleadosReportes`
  - `/Diligenciamiento` - Vista de reporte de estado de diligenciamiento
  - `/Diligenciamiento/Data` - Endpoint JSON para datos del reporte
  - `/General` - Vista de selección de reportes Excel
  - `/General/Export` - Exportación de reportes Excel (✅ **IMPLEMENTADO**)
    - Tipo 1: Información General (49 columnas)
    - Tipo 2: Hijos (5 columnas)
    - Tipo 3: Educación (11 columnas)
    - Tipo 4: Experiencia Laboral (7 columnas)
    - Tipo 5: Contactos de Emergencia (5 columnas)

✅ **Vistas Razor** (`MatrixNext.Web.Areas.TH.Views`)
- `Empleados/Index.cshtml`:
  - Panel de filtros de búsqueda
  - Tarjetas de empleados con información resumida
  - Modal de detalle con tabs para secciones:
    - Datos Generales
    - Experiencia Laboral
    - Educación
    - Hijos
    - Contactos de Emergencia
    - Promociones
    - Salarios
  - Botones de retiro/reintegro

- `EmpleadosReportes/General.cshtml`:
  - Dropdown de selección de tipo de reporte (5 opciones)
  - Botón de generación con spinner de carga
  - Descarga automática de archivo Excel
  - Manejo de errores con alertas visuales
  - Validación de selección antes de generar

- `EmpleadosReportes/Diligenciamiento.cshtml`:
  - Tabla con estado de diligenciamiento por empleado
  - Indicadores visuales (íconos/badges) por sección
  - Porcentaje de completitud
  - Filtros y ordenamiento

✅ **Navegación**
- Menú sidebar actualizado (`_main-sidebar.cshtml`):
  - Nueva categoría "Talento Humano"
  - Menú "Empleados" con submenús:
    - Administración (`/TH/Empleados`)
    - Reporte Diligenciamiento (`/TH/EmpleadosReportes/Diligenciamiento`)
    - Reportes Generales (`/TH/EmpleadosReportes/General`)

### 13.2 Arquitectura Implementada

La migración sigue estrictamente el patrón de 3 capas establecido en DIRECTRICES_MIGRACION.md:

```
┌─────────────────────────────────────────────────────────────┐
│                    Capa de Presentación                      │
│  MatrixNext.Web/Areas/TH/Controllers/EmpleadosController.cs  │
│       - Endpoints HTTP REST-like                             │
│       - Validación de autorización [Authorize]               │
│       - Manejo de errores HTTP (400/401/500)                 │
└───────────────────────────┬─────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                   Capa de Lógica de Negocio                  │
│  MatrixNext.Data/Modules/TH/Empleados/Services/              │
│  EmpleadoService.cs                                          │
│       - Validaciones de negocio                              │
│       - Orquestación de operaciones                          │
│       - Mapeo a ViewModels                                   │
└───────────────────────────┬─────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                     Capa de Acceso a Datos                   │
│  MatrixNext.Data/Modules/TH/Empleados/Adapters/              │
│  EmpleadoDataAdapter.cs                                      │
│       - Ejecución de Stored Procedures vía Dapper            │
│       - Mapeo de parámetros                                  │
│       - Manejo de conexiones SQL                             │
└───────────────────────────┬─────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                  Base de Datos (CoreProject)                 │
│  Stored Procedures (SP) existentes:                          │
│       - TH_Empleados_Get                                     │
│       - TH_Empleado_GetPorIdentificacion                     │
│       - TH_ExperienciaLaboral_*                              │
│       - TH_Educacion_*                                       │
│       - TH_Hijos_*, TH_ContactosEmergencia_*                 │
│       - TH_Promociones_*, TH_Salarios_*                      │
│       - TH_Empleado_Retirar, TH_Empleado_Reintegrar          │
│       - TH_ReporteDiligenciamientoEmpleados_Get              │
└─────────────────────────────────────────────────────────────┘
```

### 13.3 Cobertura Funcional por WebForm

#### EmpleadosAdmin.aspx → `/TH/Empleados` ⚠️ **Parcial (68% - 20 de 29 métodos)**

| Funcionalidad Legacy | Endpoint MatrixNext | Estado |
|---------------------|---------------------|--------|
| **CONSULTA Y BÚSQUEDA** |  |  |
| Búsqueda de empleados | `POST /TH/Empleados/Search` | ✅ |
| Ver detalle empleado | `GET /TH/Empleados/{id}` | ✅ |
| **EXPERIENCIA LABORAL** |  |  |
| Consultar experiencia | `GET /TH/Empleados/{id}/experiencia` | ✅ |
| Agregar experiencia | `POST /TH/Empleados/{id}/experiencia` | ✅ |
| Eliminar experiencia | `DELETE /TH/Empleados/experiencia/{id}` | ✅ |
| **EDUCACIÓN** |  |  |
| Consultar educación | `GET /TH/Empleados/{id}/educacion` | ✅ |
| Agregar educación | `POST /TH/Empleados/{id}/educacion` | ✅ |
| Eliminar educación | `DELETE /TH/Empleados/educacion/{id}` | ✅ |
| **HIJOS** |  |  |
| Consultar hijos | `GET /TH/Empleados/{id}/hijos` | ✅ |
| Agregar hijo | `POST /TH/Empleados/{id}/hijos` | ✅ |
| Eliminar hijo | `DELETE /TH/Empleados/hijos/{id}` | ✅ |
| **CONTACTOS EMERGENCIA** |  |  |
| Consultar contactos | `GET /TH/Empleados/{id}/contactos-emergencia` | ✅ |
| Agregar contacto | `POST /TH/Empleados/{id}/contactos-emergencia` | ✅ |
| Eliminar contacto | `DELETE /TH/Empleados/contactos-emergencia/{id}` | ✅ |
| **PROMOCIONES** |  |  |
| Consultar promociones | `GET /TH/Empleados/{id}/promociones` | ✅ |
| Agregar promoción | `POST /TH/Empleados/{id}/promociones` | ✅ |
| **SALARIOS** |  |  |
| Consultar salarios | `GET /TH/Empleados/{id}/salarios` | ✅ |
| Agregar salario | `POST /TH/Empleados/{id}/salarios` | ✅ |
| **OPERACIONES** |  |  |
| Retiro de empleado | `POST /TH/Empleados/{id}/retiro` | ✅ |
| Reintegro de empleado | `POST /TH/Empleados/{id}/reintegro` | ✅ |
| **ACTUALIZACIÓN DE DATOS (PENDIENTES)** |  |  |
| Actualizar datos generales | ❌ NO IMPLEMENTADO | ❌ **CRÍTICO** |
| Actualizar datos laborales | ❌ NO IMPLEMENTADO | ❌ **CRÍTICO** |
| Actualizar datos personales | ❌ NO IMPLEMENTADO | ❌ **IMPORTANTE** |
| Actualizar nómina | ❌ NO IMPLEMENTADO | ❌ **IMPORTANTE** |
| Actualizar nivel inglés | ❌ NO IMPLEMENTADO | ⚠️ |
| **CATÁLOGOS (PENDIENTES)** |  |  |
| Áreas/Service Lines | ❌ NO IMPLEMENTADO | ❌ **CRÍTICO** |
| Grupos sanguíneos | ❌ NO IMPLEMENTADO | ❌ **CRÍTICO** |
| Cargos | ❌ NO IMPLEMENTADO | ❌ **CRÍTICO** |
| Estados civiles | ❌ NO IMPLEMENTADO | ❌ **CRÍTICO** |

**Nota IMPORTANTE**: Aunque el 68% de los métodos están implementados, las funcionalidades críticas de **crear/editar empleados y obtener catálogos** están pendientes, lo que hace que el módulo NO sea funcional en producción todavía.

#### EmpleadosReporteDiligenciamiento.aspx → `/TH/EmpleadosReportes/Diligenciamiento` ✅ **Completado (100%)**

| Funcionalidad Legacy | Endpoint MatrixNext | Estado |
|---------------------|---------------------|--------|
| Vista de reporte | `GET /EmpleadosReportes/Diligenciamiento` | ✅ |
| Datos del reporte | `GET /EmpleadosReportes/Diligenciamiento/Data` | ✅ |
| Indicadores de secciones | Implementado en vista | ✅ |
| % Diligenciamiento | Calculado por SP | ✅ |

#### EmpleadosReporteGeneral.aspx → `/TH/EmpleadosReportes/General` ✅ **Completado (100%)**

| Funcionalidad Legacy | Endpoint MatrixNext | Estado |
|---------------------|---------------------|--------|
| Vista selección reporte | `GET /EmpleadosReportes/General` | ✅ |
| Reporte Información General | `POST /EmpleadosReportes/General/Export` (tipo=1) | ✅ |
| Reporte Hijos | `POST /EmpleadosReportes/General/Export` (tipo=2) | ✅ |
| Reporte Educación | `POST /EmpleadosReportes/General/Export` (tipo=3) | ✅ |
| Reporte Experiencia | `POST /EmpleadosReportes/General/Export` (tipo=4) | ✅ |
| Reporte Contactos | `POST /EmpleadosReportes/General/Export` (tipo=5) | ✅ |

**Implementación completada:**
- 5 DTOs específicos para reportes (EmpleadoReporteInfoDTO, EmpleadoHijoReporteDTO, etc.)
- 5 métodos en EmpleadoDataAdapter mapeando SPs de reportes
- 5 métodos en EmpleadoService con manejo de errores
- ExcelHelper genérico con ClosedXML
- Vista Razor completa con selector, spinner y descarga automática
- Nombres de columnas exactos según especificación legacy

#### DesvinculacionesEmpleadosGestionRRHH.aspx ❌ **Pendiente (0%)**

Esta funcionalidad requiere migración completa en fase posterior.

### 13.4 Próximos Pasos

#### Fase 1: Completar Funcionalidad CRÍTICA de EmpleadosAdmin (Prioridad Máxima)

**Sin estos componentes, el módulo NO es funcional para operación real:**

1. **Implementar endpoints de actualización de datos maestros** ⚠️ **BLOQUEANTE**
   
   a) **Crear/Actualizar Datos Generales del Empleado**
   - Crear endpoint: `POST /TH/Empleados/DatosGenerales`
   - Crear `EmpleadoDatosGeneralesDTO` con todos los campos
   - Agregar método en `EmpleadoDataAdapter`: `ActualizarDatosGenerales()`
     - Mapear a SP: `TH_Empleado_ActualizarDatosGenerales` (verificar nombre exacto)
   - Agregar método en `EmpleadoService`: `ActualizarDatosGenerales()` con validaciones
   - Implementar carga de foto (base64 → archivo físico con GUID)
   - **Impacto**: Permitirá crear y editar empleados
   
   b) **Actualizar Datos Laborales**
   - Crear endpoint: `PUT /TH/Empleados/{id}/DatosLaborales`
   - Crear `EmpleadoDatosLaboralesDTO`
   - Mapear a SP: `TH_Empleado_ActualizarDatosLaborales`
   - Validaciones: fechas, referencias a tablas maestras
   - **Impacto**: Permitirá actualizar información laboral
   
   c) **Actualizar Datos Personales**
   - Crear endpoint: `PUT /TH/Empleados/{id}/DatosPersonales`
   - Crear `EmpleadoDatosPersonalesDTO`
   - Mapear a SP: `TH_Empleado_ActualizarDatosPersonales`
   - Validaciones: formato de email, teléfonos
   - **Impacto**: Permitirá actualizar contacto y dirección

   d) **Actualizar Información de Nómina**
   - Crear endpoint: `PUT /TH/Empleados/{id}/Nomina`
   - Crear `EmpleadoNominaDTO`
   - Mapear a SP: `TH_Empleado_ActualizarNomina`
   - **Impacto**: Permitirá actualizar datos bancarios y seguridad social

   e) **Actualizar Nivel de Inglés**
   - Crear endpoint: `PUT /TH/Empleados/{id}/NivelIngles`
   - Mapear a SP: `TH_Empleado_ActualizarNivelIngles`
   - **Impacto**: Completar información del empleado

2. **Implementar endpoints de catálogos** ⚠️ **BLOQUEANTE**
   
   a) **Crear CatalogosController o métodos en EmpleadosController**
   - `GET /TH/Empleados/Catalogos/Areas` → `getAreasServiceLines()`
   - `GET /TH/Empleados/Catalogos/GruposSanguineos` → `getGruposSanguineos()`
   - `GET /TH/Empleados/Catalogos/Cargos` → `getCargos()`
   - `GET /TH/Empleados/Catalogos/EstadosCiviles` → `getEstadosCiviles()`
   
   b) **Crear Adapter y Service para catálogos**
   - Opción 1: Agregar métodos a `EmpleadoDataAdapter`
   - Opción 2: Crear `CatalogosDataAdapter` específico
   - Mapear SPs existentes de CoreProject
   
   c) **Verificar otros catálogos necesarios**
   - Niveles de inglés
   - Tipos de contrato
   - Municipios/Ciudades
   - NSE (Nivel Socioeconómico)
   - Tallas de ropa
   - Bancos, tipos de cuenta, EPS, fondos, ARL, cajas

3. **Actualizar vistas y JavaScript del frontend**
   - Implementar formularios de edición para cada sección
   - Cargar catálogos en dropdowns
   - Implementar validaciones en el cliente
   - Implementar upload de foto del empleado

**Estimación:** 3-5 días de desarrollo
**Entregable:** Módulo de empleados 100% funcional para operación

#### Fase 2: Testing Integral

4. **Pruebas con datos reales**
   - Validar creación de empleados
   - Validar actualización de todas las secciones
   - Comparar resultados con sistema legacy
   - Probar flujos completos: creación → actualización → retiro → reintegro

5. **Validación de reportes Excel**
   - Comparar archivos generados con los del legacy
   - Verificar conteo de registros
   - Validar formato y contenido

**Estimación:** 2-3 días de testing

#### Fase 3: Desvinculaciones (Opcional, fase posterior)

6. **Migración de DesvinculacionesEmpleadosGestionRRHH**
   - Crear `DesvinculacionEmpleadoDataAdapter` con métodos Dapper
   - Crear `DesvinculacionEmpleadoService`
   - Crear `DesvinculacionesController` con endpoints API
   - Portar componentes JavaScript modulares
   - Implementar generación de PDF con DevExpress

**Estimación:** 5-7 días de desarrollo

---
   - Integrar biblioteca para generación de Excel (EPPlus, ClosedXML, etc.)
   - Implementar método de exportación con columnas exactas del legacy
   - Validar formato y contenido de archivos generados

2. **Migración de Desvinculaciones RRHH**
   - Crear `DesvinculacionEmpleadoDataAdapter`
   - Crear `DesvinculacionEmpleadoService`
   - Crear `DesvinculacionesController`
   - Portar componentes JavaScript modulares
   - Implementar generación de PDF

3. **Testing integral**
   - Pruebas de búsqueda y filtros
   - Validación de CRUD completo
   - Verificación de retiro/reintegro
   - Comparación de reportes con legacy

4. **Gestión de archivos de foto**
   - Implementar endpoint de carga de imágenes
   - Validación de formatos y tamaños
   - Almacenamiento seguro

### 13.5 Notas Técnicas

#### Manejo de Errores
Todos los endpoints siguen el patrón consistente:
```csharp
try
{
    var (success, message, data) = await _service.Method(...);
    if (!success) return BadRequest(new { success = false, message });
    return Ok(new { success = true, data });
}
catch (Exception ex)
{
    _logger.LogError(ex, "Contexto del error");
    return StatusCode(500, new { success = false, message = "Error genérico" });
}
```

#### Validaciones
- **Backend**: EmpleadoService contiene todas las validaciones de negocio
- **Seguridad**: Todos los endpoints requieren `[Authorize]`
- **Usuario auditoría**: Se obtiene de `ClaimTypes.NameIdentifier`

#### Stored Procedures Mapeados
Total de SPs utilizados: **20+**
- **Consulta**: `TH_Empleados_Get`, `TH_Empleado_GetPorIdentificacion`
- **Experiencia**: `TH_ExperienciaLaboral_Get/InsertUpdate/Delete`
- **Educación**: `TH_Educacion_Get/InsertUpdate/Delete`
- **Hijos**: `TH_Hijos_Get/InsertUpdate/Delete`
- **Contactos**: `TH_ContactosEmergencia_Get/InsertUpdate/Delete`
- **Promociones**: `TH_Promociones_Get/Insert`
- **Salarios**: `TH_Salarios_Get/Insert`
- **Operaciones**: `TH_Empleado_Retirar`, `TH_Empleado_Reintegrar`
- **Reportes**: 
  - `TH_ReporteDiligenciamientoEmpleados_Get`
  - `TH_Empleados_Reporte_Info`
  - `TH_Hijos_Report`
  - `TH_Educacion_Report`
  - `TH_ExperienciaLaboral_Report`
  - `TH_ContactosEmergencia_Report`

#### Bibliotecas y Dependencias
- **ClosedXML 0.105.0**: Generación de archivos Excel (.xlsx)
  - Instalado en MatrixNext.Data
  - Usado por ExcelHelper para exportación de reportes
  - Reemplaza la funcionalidad de `Utilidades.ResponseExcel` del legacy
  - Soporte completo para estilos, formato y autoajuste de columnas
- **Dapper**: ORM ligero para ejecución de Stored Procedures
- **Microsoft.Data.SqlClient**: Conexión a SQL Server
- **DevExpress**: Disponible con licencia para futuras implementaciones de PDF (Desvinculaciones)
- Contactos: `TH_ContactosEmergencia_Get/InsertUpdate/Delete`
- Promociones: `TH_Promociones_Get/Insert`
- Salarios: `TH_Salarios_Get/Insert`
- Operaciones: `TH_Empleado_Retirar`, `TH_Empleado_Reintegrar`
- Reportes: `TH_ReporteDiligenciamientoEmpleados_Get`

---

## 14. Apéndice: Mapeo de WebMethods a Endpoints

### EmpleadosAdmin.aspx

| WebMethod Legacy | Endpoint MatrixNext | Método HTTP |
|------------------|---------------------|-------------|
| `getEmpleados` | `/TH/Empleados/Search` | POST |
| `getEmpleadoPorIdentificacion` | `/TH/Empleados/{identificacion}` | GET |
| `addExperienciaLaboral` | `/TH/Empleados/Experiencia` | POST |
| `deleteExperienciaLaboral` | `/TH/Empleados/Experiencia/{id}` | DELETE |
| `getExperienciasLaboralesPorIdentificacion` | `/TH/Empleados/{identificacion}/Experiencia` | GET |
| `addEducacion` | `/TH/Empleados/Educacion` | POST |
| `deleteEducacion` | `/TH/Empleados/Educacion/{id}` | DELETE |
| `getEducacion` | `/TH/Empleados/{identificacion}/Educacion` | GET |
| `addHijo` | `/TH/Empleados/Hijos` | POST |
| `deleteHijo` | `/TH/Empleados/Hijos/{id}` | DELETE |
| `getHijos` | `/TH/Empleados/{identificacion}/Hijos` | GET |
| `addContactoEmergencia` | `/TH/Empleados/Contactos` | POST |
| `deleteContactoEmergencia` | `/TH/Empleados/Contactos/{id}` | DELETE |
| `getContactosEmergencia` | `/TH/Empleados/{identificacion}/Contactos` | GET |
| `addPromocion` | `/TH/Empleados/Promociones` | POST |
| `getPromociones` | `/TH/Empleados/{identificacion}/Promociones` | GET |
| `addSalario` | `/TH/Empleados/Salarios` | POST |
| `getSalarios` | `/TH/Empleados/{identificacion}/Salarios` | GET |
| `retirarEmpleado` | `/TH/Empleados/{identificacion}/Retiro` | POST |
| `reintegrarEmpleado` | `/TH/Empleados/{identificacion}/Reintegro` | POST |

### EmpleadosReporteDiligenciamiento.aspx

| WebMethod Legacy | Endpoint MatrixNext | Método HTTP |
|------------------|---------------------|-------------|
| `getReporteDiligenciamiento` | `/TH/EmpleadosReportes/Diligenciamiento/Data` | GET |

### EmpleadosReporteGeneral.aspx

| Reporte Legacy | Endpoint MatrixNext | Método HTTP | Columnas |
|---------------|---------------------|-------------|----------|
| Información General | `/TH/EmpleadosReportes/General/Export` (tipo=1) | POST | 49 columnas |
| Hijos | `/TH/EmpleadosReportes/General/Export` (tipo=2) | POST | 5 columnas |
| Educación | `/TH/EmpleadosReportes/General/Export` (tipo=3) | POST | 11 columnas |
| Experiencia Laboral | `/TH/EmpleadosReportes/General/Export` (tipo=4) | POST | 7 columnas |
| Contactos Emergencia | `/TH/EmpleadosReportes/General/Export` (tipo=5) | POST | 5 columnas |

---

## 15. Resumen de Cambios - Sesión 2 de Enero 2026

### Funcionalidad Implementada: Exportación de Reportes Excel

**Objetivo alcanzado**: Completar la funcionalidad de exportación de reportes generales de empleados en formato Excel, replicando exactamente la funcionalidad del WebForm `EmpleadosReporteGeneral.aspx`.

#### Archivos Creados

1. **ExcelHelper.cs** (`MatrixNext.Data/Helpers/`)
   - Helper genérico para generación de archivos Excel usando ClosedXML
   - Método `GenerateExcel<T>()` con soporte para:
     - Ordenamiento personalizado de columnas
     - Filtrado de columnas por nombre
     - Estilos automáticos (encabezados, bordes, autoajuste)
     - Conversión de colecciones genéricas a DataTable
   - 145 líneas de código

2. **General.cshtml** (`MatrixNext.Web/Areas/TH/Views/EmpleadosReportes/`)
   - Vista Razor para selección y generación de reportes
   - Dropdown con 5 tipos de reporte
   - Botón de generación con spinner de carga
   - Descarga automática de archivo mediante blob
   - Manejo de errores con alertas visuales
   - 215 líneas de código (HTML + JavaScript)

#### Archivos Modificados

3. **EmpleadoDTO.cs** (`MatrixNext.Data/Modules/TH/Empleados/Models/`)
   - Agregados 5 DTOs para reportes Excel:
     - `EmpleadoReporteInfoDTO` (49 propiedades - reporte general)
     - `EmpleadoHijoReporteDTO` (5 propiedades)
     - `EmpleadoEducacionReporteDTO` (11 propiedades)
     - `EmpleadoExperienciaReporteDTO` (7 propiedades)
     - `EmpleadoContactoEmergenciaReporteDTO` (5 propiedades)
   - Total agregado: ~150 líneas

4. **EmpleadoDataAdapter.cs** (`MatrixNext.Data/Modules/TH/Empleados/Adapters/`)
   - Agregados 5 métodos para reportes:
     - `ObtenerReporteInformacionGeneral()` → SP: `TH_Empleados_Reporte_Info`
     - `ObtenerReporteHijos()` → SP: `TH_Hijos_Report`
     - `ObtenerReporteEducacion()` → SP: `TH_Educacion_Report`
     - `ObtenerReporteExperienciaLaboral()` → SP: `TH_ExperienciaLaboral_Report`
     - `ObtenerReporteContactosEmergencia()` → SP: `TH_ContactosEmergencia_Report`
   - Total agregado: ~80 líneas

5. **EmpleadoService.cs** (`MatrixNext.Data/Modules/TH/Empleados/Services/`)
   - Agregados 5 métodos de servicio con manejo de errores:
     - `ObtenerReporteInformacionGeneral()`
     - `ObtenerReporteHijos()`
     - `ObtenerReporteEducacion()`
     - `ObtenerReporteExperienciaLaboral()`
     - `ObtenerReporteContactosEmergencia()`
   - Total agregado: ~100 líneas

6. **EmpleadosReportesController.cs** (`MatrixNext.Web/Areas/TH/Controllers/`)
   - Método `ExportGeneral()` completamente implementado
   - Switch con 5 casos para tipos de reporte
   - Configuración de nombres de archivo y hojas
   - Especificación exacta de columnas según legacy
   - Descarga de archivo con Content-Disposition correcto
   - Total modificado: ~130 líneas

7. **ANALISIS_TH_EMPLEADOS.md**
   - Actualizada tabla de cobertura funcional (100% completado para reportes generales)
   - Actualizado checklist de completitud
   - Agregada sección de bibliotecas y dependencias
   - Actualizada lista de SPs mapeados (20+)
   - Agregada tabla de mapeo de reportes Excel con columnas
   - Total actualizado: ~100 líneas

#### Dependencias Instaladas

- **ClosedXML 0.105.0** - Biblioteca para generación de archivos Excel
  - Instalado en: `MatrixNext.Data`
  - Dependencias transitivas: DocumentFormat.OpenXml, RBush.Signed, System.IO.Packaging

#### Impacto en la Migración

**Antes de esta sesión:**
- EmpleadosReporteGeneral.aspx: 60% completado (vista creada, lógica pendiente)

**Después de esta sesión:**
- EmpleadosReporteGeneral.aspx: ✅ **100% completado**

**Progreso global del módulo TH_Empleados:**
- EmpleadosAdmin.aspx: 95% (pendiente: upload de fotos)
- EmpleadosReporteDiligenciamiento.aspx: 100% ✅
- EmpleadosReporteGeneral.aspx: 100% ✅
- DesvinculacionesEmpleadosGestionRRHH.aspx: 0% (próxima fase)

**Total completado: 3 de 4 WebForms (75% del módulo)**

#### Notas Técnicas

- Los nombres de columnas de Excel siguen exactamente la especificación del legacy
- El orden de columnas respeta el definido en `Utilidades.ResponseExcel`
- Los archivos generados tienen nombres descriptivos según tipo
- La descarga se maneja mediante FileResult con MIME type correcto
- El frontend usa Fetch API moderna para descarga de blobs
- Todos los métodos incluyen logging de errores
- La solución no presenta errores de compilación

#### Próximos Pasos Sugeridos

1. **Testing de reportes Excel**
   - Validar que los SPs retornen datos en el formato esperado
   - Comparar archivos generados con los del sistema legacy
   - Verificar conteo de registros y exactitud de datos

2. **Migración de Desvinculaciones**
   - Crear DTOs para desvinculación
   - Implementar DataAdapter con métodos Dapper
   - Crear Service con validaciones
   - Crear Controller con endpoints API
   - Portar componentes JavaScript
   - Implementar generación de PDF (DevExpress disponible)

---
