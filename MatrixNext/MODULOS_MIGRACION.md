# Mapa de Módulos para Migración WebMatrix → MatrixNext

⚠️ **IMPORTANTE**: Antes de migrar cualquier módulo, leer [DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md) que contiene las 15 reglas obligatorias para garantizar consistencia y calidad.

---

## Módulos Identificados y Clasificados por Prioridad

### 🔴 CRÍTICA (Implementar primero)

#### 1. **US_Usuarios** (14 páginas)
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
- **Status**: 🔄 EN PROGRESO (modales y CambioContrasena implementados)

- **Notas de progreso**: Se agregaron entradas de menú y partials/modal para Create/Edit/Delete en `Views/Usuarios/` y se implementó la página `ChangePassword` (GET/POST) y el método `CambiarContrasena` en `UsuarioService`.

#### Asignaciones del usuario (Roles / Unidades / Permisos)
- Se añadieron métodos en `MatrixNext.Data.Adapters.UsuarioDataAdapter` para asignar/remover roles (`US_RolesUsuarios`), unidades (`US_UsuariosUnidades`) y permisos (`US_PermisosUsuarios`).
- Se expusieron operaciones en `MatrixNext.Data.Services.Usuarios.UsuarioService` para: obtener asignados/disponibles y asignar/remover.
- Se agregaron endpoints AJAX en `MatrixNext.Web.Controllers.Usuarios.UsuariosController` para invocar las operaciones desde la UI.
- El proyecto `MatrixNext.Web` compila correctamente tras los cambios (compilación exitosa con advertencias sobre nullable warnings en DTOs).

#### 2. **Home** (3 páginas)
- **Carpeta**: `WebMatrix/Home/`
- **Contexto**: `CORE_Model` + múltiples
- **Páginas**:
  - Home.aspx (dashboard principal)
  - Default.aspx (ALT: puede estar en raíz)
  - DefaultOLD.aspx
- **Dependencias**: ALTA (consume datos de múltiples módulos)
- **Status**: 🔄 DESPUÉS de US_Usuarios

---

### 🟠 ALTA (Prioritario)

#### 3. **PY_Proyectos** (18 páginas)
- **Carpeta**: `WebMatrix/PY_Proyectos/`
- **Contexto**: `PY_Model` (CoreProject)
- **Dependencias**: Medias (referencia Usuarios, Metodologías)
- **Volumen**: Grande pero bien estructurado

#### 4. **OP_Cuantitativo** (múltiples)
- **Carpeta**: `WebMatrix/OP_Cuantitativo/`
- **Contexto**: `OP_Cuanti_Model` (CoreProject)
- **Dependencias**: Altas (métodos, cálculos, variables)

#### 5. **OP_Cualitativo** (múltiples)
- **Carpeta**: `WebMatrix/OP_Cualitativo/`
- **Contexto**: `OP_Entities` (CoreProject)
- **Dependencias**: Altas (entrevistas, moderadores, sesiones)

#### 6. **FI_AdministrativoFinanciero** (21 páginas)
- **Carpeta**: `WebMatrix/FI_AdministrativoFinanciero/`
- **Contexto**: `FI_Model` (CoreProject)
- **Dependencias**: Altas (compras, órdenes, facturas)
- **Volumen**: Muy grande

---

### 🟡 MEDIA (Estratégica)

#### 7. **GD_Documentos**
- **Contexto**: `GD_Model`
- **Dependencias**: Medias

#### 8. **RP_Reportes**
- **Contexto**: `REP_Model`
- **Notas**: Consultas complejas → ideal para Dapper

#### 9. **TH_TalentoHumano** (28 páginas) ✅ EN PROGRESO
- **Carpeta**: `WebMatrix/TH_TalentoHumano/`
- **Contexto**: `TH_Model` (CoreProject)
- **Dependencias**: Medias (empleados, usuarios, catálogos)
- **Estado**: 
  - ✅ **Ausencias** (4 páginas) - COMPLETADO
  - 🔄 **Empleados** (pendiente)
  - 🔄 **Nómina** (pendiente)
  - 🔄 **Otros** (pendiente)

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

#### 10. **CU_Cuentas** (Clientes)
- **Contexto**: `CU_Model`
- **Dependencias**: Medias

#### 11. **CC_FinzOpe** (Financiera - Operacional)
- **Contexto**: `CC_FinzOpe`
- **Volumen**: Grande

---

### 🟢 BAJA (Complementaria)

12. **OP_RO** (Revisión Operacional)
13. **OP_Trafico**
14. **PY_ControlCalidad**
15. **PY_Adquisiciones**
16. **PNC** (Producto No Conforme)
17. **SG_Actas** (Seguimiento - Actas)
18. **ES_Estadistica**
19. **Centro_Informacion**
20. **Inventario**
21. **IT**
22. **MBO** (Objetivos)
23. **ResumenProduccion**
24. **RE_GT**
25. **PC_PropiedadCliente**
26. Otros (Account, Controls, etc.)

---

## Patrón de Migración por Módulo

```
Módulo WebMatrix (ej: US_Usuarios/)
│
├── 14 páginas .aspx.vb
│
└── MatrixNext → Controllers + Views + Services
    │
    ├── Controllers/
    │   ├── UsuariosController.cs
    │   ├── RolesController.cs
    │   ├── PermisosController.cs
    │   └── GrupoUnidadController.cs
    │
    ├── Views/
    │   ├── Usuarios/
    │   │   ├── Index.cshtml
    │   │   ├── Create.cshtml
    │   │   ├── Edit.cshtml
    │   │   └── Delete.cshtml
    │   ├── Roles/
    │   │   └── [idem estructura]
    │   ├── Permisos/
    │   │   └── [idem estructura]
    │   └── GrupoUnidad/
    │       └── [idem estructura]
    │
    └── Data/Services/
        ├── UsuarioService.cs
        ├── RolService.cs
        ├── PermisosService.cs
        └── GrupoUnidadService.cs
```

---

## Cronología Recomendada

| Fase | Semana | Módulo | Entregables |
|------|--------|--------|-------------|
| 0 | 1 | Login (✅ HECHO) | LoginController, autenticación |
| 1 | 2-3 | US_Usuarios | 14 páginas migradas, adaptador CoreProject |
| 2 | 4 | Home | Dashboard funcional |
| 3 | 5-7 | PY_Proyectos | 18 páginas, gestión completa |
| 4 | 8-9 | OP_Cuantitativo | Operaciones cuantitativos |
| 5 | 10-11 | OP_Cualitativo | Operaciones cualitativos |
| 6 | 12-15 | FI_Administrativo | 21 páginas finanzas |
| 7+ | 16+ | Módulos restantes | Por prioridad operativa |

---

## Decisiones Clave

✅ **SIN coexistencia**: Eliminar WebMatrix completamente al terminar cada módulo  
✅ **Adaptar CoreProject**: Crear wrappers que encapsulen contextos EF6  
✅ **Testing exhaustivo**: Validar funcionalidad antes de eliminar legacy  
✅ **Migraciones EF Core**: Solo para nuevas features, no for legacy  
✅ **Dapper para consultas**: Mantener para SP complejas  

---

## Próximo Paso Concreto

**Crear estructura base para US_Usuarios:**

```bash
# En MatrixNext.Web
mkdir Controllers/US
mkdir Views/US

# En MatrixNext.Data
mkdir Models/US
mkdir Services/US
mkdir Adapters
```

**Luego**: Migrar primera página (Usuarios.aspx) como demo del patrón
---

## 📖 DOCUMENTACIÓN DE REFERENCIA

### Documentos Maestros

| Documento | Propósito | Aplicable |
|-----------|-----------|-----------|
| [DIRECTRICES_MIGRACION.md](DIRECTRICES_MIGRACION.md) | 15 reglas obligatorias para todas las migraciones | **LEER ANTES DE CUALQUIER MÓDULO** |
| [VERIFICACION_AUSENCIAS_MIGRACION.md](VERIFICACION_AUSENCIAS_MIGRACION.md) | Caso de estudio: verificación completa de TH_Ausencias | Referencia de calidad |
| [PLAN_MIGRACION_PY_PROYECTOS.md](PLAN_MIGRACION_PY_PROYECTOS.md) | Plan detallado para próximo módulo | Plantilla para futuros módulos |
| [DASHBOARD_MIGRACION.md](DASHBOARD_MIGRACION.md) | Estado, métricas, timeline de todo el esfuerzo | Actualizar semanalmente |
| [RESUMEN_MIGRACION_AUSENCIAS.md](RESUMEN_MIGRACION_AUSENCIAS.md) | Resumen ejecutivo de TH_Ausencias | Stakeholders/Gerencia |

### Cómo Usar Documentación

1. **Iniciando módulo**: Leer DIRECTRICES_MIGRACION.md completo
2. **Planificando**: Usar PLAN_MIGRACION_PY_PROYECTOS.md como plantilla
3. **Verificando**: Comparar contra VERIFICACION_AUSENCIAS_MIGRACION.md
4. **Reportando**: Actualizar DASHBOARD_MIGRACION.md