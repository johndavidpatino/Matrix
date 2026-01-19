# 📊 INVENTARIO: TH_TalentoHumano WebMatrix vs MatrixNext

> **Fecha**: 2026-01-18  
> **Propósito**: Mapeo completo de páginas .aspx de WebMatrix → Controllers de MatrixNext  
> **Área**: TH (Talento Humano)

---

## 📈 RESUMEN EJECUTIVO

| Métrica | Cantidad |
|---------|----------|
| **Páginas WebMatrix** | 26 |
| **Controllers MatrixNext** | 7 + 3 API |
| **Páginas Migradas** | 14 ✅ |
| **Páginas Faltantes** | 12 ❌ |
| **Cobertura** | **54%** |

---

## 🗺️ TABLA DE MAPEO COMPLETO

### ✅ PÁGINAS MIGRADAS (14/26)

| # | WebMatrix (.aspx) | MatrixNext Controller | Actions Equivalentes | Estado |
|---|-------------------|----------------------|---------------------|--------|
| 1 | **SolicitudAusencia.aspx** | `AusenciasController.cs` | Index, Create, Aprobar, Rechazar, History, BeneficiosPendientes | ✅ Migrado |
| 2 | **SolicitudAusenciaIncapacidades.aspx** | `AusenciasController.cs` | CreateIncapacidad (parcial) | ✅ Parcial |
| 3 | **AusenciasEquipo.aspx** | `AusenciasEquipoController.cs` | Index, GetAusenciasEquipo, GetSubordinados, AddSubordinado, RemoveSubordinado | ✅ Migrado |
| 4 | **GestionAusenciaRRHH.aspx** | `GestionAusenciaController.cs` | Index, Aprobar, Rechazar, ReporteVacaciones, ReporteIncapacidades, ReporteBeneficios | ✅ Migrado |
| 5 | **EmpleadosAdmin.aspx** | `EmpleadosController.cs` | Index, Search, GetEmpleado, Update (datos generales, contacto, bancario, laboral) | ✅ Migrado |
| 6 | **EmpleadoUpdate.aspx** | `EmpleadosController.cs` | GetMisDatos, UpdateMisDatos (autoservicio empleado) | ✅ Migrado |
| 7 | **EmpleadosReporteDiligenciamiento.aspx** | `EmpleadosReportesController.cs` | Diligenciamiento, GetDiligenciamientoData | ✅ Migrado |
| 8 | **EmpleadosReporteGeneral.aspx** | `EmpleadosReportesController.cs` | General, DescargarInformacionGeneral, DescargarHijos, DescargarEducacion, etc. | ✅ Migrado |
| 9 | **DesvinculacionesEmpleadosGestionRRHH.aspx** | `DesvinculacionesController.cs` | Index, Buscar, IniciarProceso, GetEvaluaciones, GenerarPDF | ✅ Migrado |
| 10 | **DesvinculacionesEmpleadosGestionArea.aspx** | `DesvinculacionesController.cs` | GestionArea, EvaluarPorArea (integrado en mismo controller) | ✅ Integrado |
| 11 | **Default.aspx** | `HomeController.cs` (área TH) | Index (página de inicio del módulo TH) | ✅ No aplica |
| 12 | **Home.aspx** | `HomeController.cs` (área TH) | Index (verificación permiso 31) | ✅ No aplica |
| 13 | **EnCasoEmergencia.aspx** | `EmpleadosController.cs` | GetDatosEmergencia (endpoint de búsqueda) | ⚠️ Pendiente View |
| 14 | **ConsultaLog.aspx** | `EmpleadosController.cs` | GetLogPersonas (parcial, sin vista dedicada) | ⚠️ Pendiente View |

---

### ❌ PÁGINAS FALTANTES (12/26)

| # | WebMatrix (.aspx) | Funcionalidad Principal | SP/DAL Usados | Prioridad | Esfuerzo |
|---|-------------------|------------------------|---------------|-----------|----------|
| 1 | **Capacitacion.aspx** | CRUD capacitaciones, planillas asistencia, PDF certificados | Capacitaciones, CargarCapacitaciones, AdicionarRefuerzo | 🔴 Alta | 8h |
| 2 | **Contratistas.aspx** | CRUD contratistas, servicios, clasificación | Contratista.ExisteContratista, GuardarContratista, ObtenerServicios | 🟠 Media | 6h |
| 3 | **LogContratistas.aspx** | Historial cambios contratistas (lectura) | Contratista.LogContratistasGet | 🟢 Baja | 2h |
| 4 | **HojasVida.aspx** | CRUD hojas de vida (reclutamiento), keywords, entrevistas | HojasVida.agregar, actualizarHojasVida, agregarKeyword, agregarEntrevista | 🔴 Alta | 8h |
| 5 | **HojaVida.aspx** | Formulario detallado de hoja de vida con múltiples tabs | TH.HojaVida, ExperienciaLaboral, Idiomas, Educacion | 🔴 Alta | 10h |
| 6 | **ListadoHojasDeVida.aspx** | Grid listado de hojas de vida con redirección a detalle | Personas.DevolverTodos | 🟠 Media | 3h |
| 7 | **Personas.aspx** | Registro completo de personas (formulario extenso) | RegistroPersonas, múltiples catálogos | 🔴 Alta | 12h |
| 8 | **Personas2.aspx** | Página vacía (sin lógica) | Ninguno | ⬜ Eliminar | 0h |
| 9 | **HWH.aspx** | Solicitud de días Easy Work (Teletrabajo) | TeleTrabajoC.BuscarXUsuarioXFecha, Guardar, validarHWH | 🟠 Media | 6h |
| 10 | **HWH-Admin.aspx** | Gestión/aprobación HWH por jefe directo | TeleTrabajoC.BuscarXjefeDirectoXEstadoXFechas, ActualizarGestion | 🟠 Media | 5h |
| 11 | **HWH-RH.aspx** | Panel RRHH para HWH, reportes Excel, gantt | TeleTrabajoC.BuscarJefes, exportarExcel | 🟠 Media | 5h |
| 12 | **ReporteCambiosContratacion.aspx** | Reporte cambios contratación (tipo log) | Personas.Th_REPCambiosContratacion | 🟢 Baja | 3h |

---

## 📋 DESCRIPCIÓN DETALLADA DE PÁGINAS FALTANTES

### 1. 🔴 Capacitacion.aspx (910 líneas)
**Funcionalidad**: Gestión completa de capacitaciones
- **Acciones**:
  - CargarCapacitaciones: Lista capacitaciones por trabajo
  - Guardar/Eliminar capacitaciones
  - Agregar/remover participantes
  - Generar planillas de asistencia PDF
  - Agregar refuerzos de capacitación
- **Permisos**: 86
- **SP identificados**: 
  - `Capacitaciones` (CoreProject)
  - `CargarCapacitaciones`
  - `AdicionarRefuerzo`
- **Dependencias**: iTextSharp (PDF), ClosedXML

---

### 2. 🟠 Contratistas.aspx (400 líneas)
**Funcionalidad**: CRUD de contratistas externos
- **Acciones**:
  - ObtenerContratistas: Listado con filtros
  - GuardarContratista: Crear/actualizar
  - ObtenerServicios: Servicios por contratista
  - Clasificación de contratistas
- **Permisos**: 131
- **SP identificados**:
  - `Contratista.ExisteContratista`
  - `Contratista.Guardar`
  - `Contratista.ObtenerServicios`

---

### 3. 🟢 LogContratistas.aspx (27 líneas)
**Funcionalidad**: Solo lectura - historial de contratistas
- **Acciones**:
  - CargarInformacion: Grid con filtros
  - Búsqueda por identificación/nombre
- **SP identificados**:
  - `Contratista.LogContratistasGet`

---

### 4. 🔴 HojasVida.aspx (122 líneas)
**Funcionalidad**: CRUD hojas de vida para reclutamiento
- **Acciones**:
  - savePerson: Crear/actualizar persona
  - addKeyword: Tags de búsqueda
  - addEntrevista: Registro de entrevistas
  - addExperienciaLaboral
  - obtenerHojasVida: Búsqueda con filtros
- **SP identificados**:
  - `HojasVida.agregar`
  - `HojasVida.actualizarHojasVida`
  - `HojasVida.agregarKeyword`
  - `HojasVida.obtenerHojasVida`

---

### 5. 🔴 HojaVida.aspx (789 líneas)
**Funcionalidad**: Formulario detallado multi-pestaña
- **Acciones**:
  - Datos personales
  - Experiencia laboral (CRUD)
  - Idiomas (CRUD)
  - Educación (CRUD)
  - Carga de foto
- **SP identificados**:
  - `TH.HojaVida.ObtenerHojaVidaIdentificacion`
  - `TH.ExperienciaLaboral`
  - `TH.Idiomas`
  - `TH.Educacion`

---

### 6. 🟠 ListadoHojasDeVida.aspx (35 líneas)
**Funcionalidad**: Grid de hojas de vida con link a detalle
- **Permisos**: 87
- **SP identificados**:
  - `Personas.DevolverTodos`

---

### 7. 🔴 Personas.aspx (592 líneas)
**Funcionalidad**: Registro completo de personas (formulario extenso)
- **Catálogos cargados**:
  - TipoIdentificacion, Sexo, EstadoCivil
  - NivelEducativo, GruposSanguineos
  - Ciudades, EstadoActual, Empresas
  - TiposContrato, TiposSalario, Bancos
  - Cargos, FondoPensiones, etc.
- **SP identificados**:
  - `RegistroPersonas` (múltiples métodos)

---

### 8. ⬜ Personas2.aspx (9 líneas)
**Funcionalidad**: VACÍA - sin lógica implementada
- **Acción recomendada**: ELIMINAR

---

### 9-11. 🟠 HWH*.aspx (Easy Work / Teletrabajo)
**HWH.aspx** (486 líneas): Solicitud empleado
- Crear solicitud de día HWH
- Validar límites por quincena
- Enviar notificación por email

**HWH-Admin.aspx** (393 líneas): Gestión jefe directo
- Aprobar/Rechazar solicitudes
- Vista Gantt de equipo
- Filtros por estado/fecha

**HWH-RH.aspx** (343 líneas): Panel RRHH
- Búsqueda global
- Exportar Excel
- Vista consolidada

**SP identificados**:
- `TeleTrabajoC.BuscarXUsuarioXFecha`
- `TeleTrabajoC.BuscarXjefeDirectoXEstadoXFechas`
- `TeleTrabajoC.ActualizarGestion`
- `TeleTrabajoC.BuscarJefes`

---

### 12. 🟢 ReporteCambiosContratacion.aspx (70 líneas)
**Funcionalidad**: Reporte de cambios en contratación
- **Acciones**:
  - Consultar por fecha/cédula
  - Exportar Excel
- **SP identificados**:
  - `Personas.Th_REPCambiosContratacion`

---

## 🆕 FUNCIONALIDADES EXTRAS EN MATRIXNEXT

| Controller | Funcionalidad | ¿Existe en WebMatrix? |
|------------|--------------|----------------------|
| `CatalogosController.cs` | API centralizada de catálogos | ❌ No (estaban dispersos en cada página) |
| `Api/EmpleadosController.cs` | API REST para empleados | ❌ No (WebMethods en cada .aspx) |
| `Api/DesvinculacionesController.cs` | API REST para desvinculaciones | ❌ No |
| `Api/CatalogosController.cs` | API REST para catálogos | ❌ No |

**Nota**: MatrixNext centraliza endpoints API que en WebMatrix estaban dispersos como WebMethods en múltiples páginas.

---

## 📊 PLAN DE MIGRACIÓN SUGERIDO

### Sprint A: Alta Prioridad (30h)
1. **Capacitacion.aspx** → `CapacitacionesController.cs` (8h)
2. **HojasVida.aspx** → `HojasVidaController.cs` (8h)
3. **HojaVida.aspx** → Integrar en `HojasVidaController.cs` (10h)
4. **Personas.aspx** → `PersonasController.cs` (12h) ⚠️ Evaluar si fusionar con Empleados

### Sprint B: Media Prioridad (25h)
1. **Contratistas.aspx** → `ContratistasController.cs` (6h)
2. **HWH.aspx** → `TeletrabajoController.cs` (6h)
3. **HWH-Admin.aspx** → Integrar en `TeletrabajoController.cs` (5h)
4. **HWH-RH.aspx** → Integrar en `TeletrabajoController.cs` (5h)
5. **ListadoHojasDeVida.aspx** → Vista en `HojasVidaController.cs` (3h)

### Sprint C: Baja Prioridad (5h)
1. **ReporteCambiosContratacion.aspx** → `EmpleadosReportesController.cs` (3h)
2. **LogContratistas.aspx** → `ContratistasController.cs` (2h)

### No migrar
- **Personas2.aspx** → ELIMINAR (vacío)

---

## ✅ CHECKLIST DE VERIFICACIÓN

- [ ] Crear `CapacitacionesController.cs` + Service + Adapter
- [ ] Crear `HojasVidaController.cs` + Service + Adapter  
- [ ] Crear `ContratistasController.cs` + Service + Adapter
- [ ] Crear `TeletrabajoController.cs` + Service + Adapter
- [ ] Agregar vista `EnCasoEmergencia` (datos de emergencia)
- [ ] Agregar vista `ConsultaLog` (log de cambios personas)
- [ ] Verificar SP en `docs/SQL/CO_Matrix_SP_Names.csv`
- [ ] Actualizar `_Sidebar.cshtml` con nuevos menús
- [ ] Registrar servicios en `Program.cs`

---

## 📁 ARCHIVOS RELACIONADOS

### CoreProject (DAL Legacy)
- `CoreProject/Capacitaciones.vb`
- `CoreProject/Contratista.vb`
- `CoreProject/HojasVida.vb`
- `CoreProject/TeleTrabajoC.vb`
- `CoreProject/TH/HojaVida.vb`
- `CoreProject/RegistroPersonas.vb`

### WebMatrix
- `WebMatrix/TH_TalentoHumano/*.aspx.vb` (26 archivos)

### MatrixNext
- `MatrixNext.Web/Areas/TH/Controllers/` (7 controllers)
- `MatrixNext.Web/Areas/TH/Controllers/Api/` (3 API controllers)

---

**Documento generado**: 2026-01-18  
**Próxima revisión**: Al completar Sprint A
