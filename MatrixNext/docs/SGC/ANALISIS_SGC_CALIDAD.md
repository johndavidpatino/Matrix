# ANÁLISIS - MÓDULO SGC_CALIDAD (Sistema de Gestión de Calidad)

**Fecha Análisis**: 2026-01-15  
**Sprint Asignado**: Sprint 13  
**Estado**: 🔴 PENDIENTE MIGRACIÓN  
**Prioridad**: 🟡 MEDIA-BAJA  
**Dependencias**: US_Usuarios, GD_Documentos

---

## 📋 RESUMEN EJECUTIVO

**SGC_Calidad** es el módulo de Sistema de Gestión de Calidad que gestiona:
1. **Auditorías Internas** - Planificación, ejecución y reporte de auditorías
2. **Acciones de Mejora** - Registro y seguimiento de acciones correctivas
3. **Hallazgos** - Documentación de no conformidades

**Volumen**: 5 páginas ASP.NET + 2 repositorios Dapper + 8 Stored Procedures + 4 tablas SQL

---

## 🗂️ ESTRUCTURA WEBMATRIX

```
WebMatrix/SGC_Calidad/
├── SGC_Calidad_Home.aspx                    # Home/Dashboard
├── SGC_Calidad_Home.aspx.vb                # Código behind (vacío - solo carga)
│
├── AccionesMejora/
│   ├── SGC_AccionesMejora.aspx              # CRUD Acciones de Mejora
│   └── SGC_AccionesMejora.aspx.vb           # Lógica (295 LOC)
│
└── AuditoriasInternas/
    ├── Auditor.aspx                         # Listado/Gestión Auditorías
    ├── Auditor.aspx.vb                      # Lógica (193 LOC)
    ├── NuevaAuditoria.aspx                  # Crear Nueva Auditoría
    └── NuevaAuditoria.aspx.vb               # Lógica (123 LOC)
```

**Total LOC WebMatrix**: ~611 líneas (Code Behind)

---

## 🗄️ TABLAS SQL IDENTIFICADAS

| Tabla | Propósito | Campos Clave | Auditoría |
|-------|-----------|--------------|-----------|
| **SGC_AI_Auditorias** | Auditorías internas | Id, AuditorId, AreaAuditada, ProcesoAuditado, FechaLimiteAuditoria, SGC_AI_EstadoId | Sí |
| **SGC_AI_Auditorias_Auditados** | Personas auditadas | Id, SGC_AI_AuditoriaId, AuditadoId | No |
| **SGC_AI_Auditorias_Hallazgos** | No conformidades encontradas | Id, SGC_AI_AuditoriaId, Hallazgo, SGC_AI_TipoHallazgoId | No |
| **SGC_AI_Auditorias_InformeAuditor** | Informe post-auditoría | Id, SGC_AI_AuditoriaId, FechaAuditoria, Fortalezas, ArchivoInformeAuditoria* | Sí |
| **SGC_AI_Auditorias_EstadosLog** | Historial de estados | SGC_AI_AuditoriaId, SGC_AI_Estado | Sí |
| **ACM_AccionesMejora** | Acciones correctivas | Id, DescripcionAccion, FechaIncidente, UsuarioReporta, ProcesoId | Sí |

---

## 🔌 STORED PROCEDURES IDENTIFICADOS

### Auditorías Internas (SGC_AI)

| SP | Operación | Parámetros | Retorna |
|----|-----------|-----------|---------| 
| **SGC_AuditoriasInternas_Add** | Crear auditoría | @AuditorId, @AreaAuditada, @ProcesoAuditado, @FechaLimiteAuditoria, @TiposAuditoria, @NormativasAAuditar, @FechaRegistro, @UsuarioRegistraId | int (Id) |
| **SGC_AI_AuditoriasBy** | Listar con filtro | @AuditorId?, @EstadoId?, @AnoAuditoria?, @AuditadoId?, @pageSize, @pageIndex | List<SGC_AuditoriaInternaEntity> |
| **SGC_AI_AuditoriaInforme_Add** | Registrar informe auditor | @AuditoriaId, @FechaAuditoria, @Fortalezas, @Auditados, @Hallazgos (XML), @ArchivoInforme*, @UsuarioRegistra, @FechaRegistro | int (rows) |
| **SGC_AI_Auditorias_InformeAuditorByAuditoriaId** | Obtener informe | @AuditoriaId | SGC_AI_Auditorias_InformeAuditorEntity |
| **SGC_AI_Auditorias_InformeAuditor_AuditadosByAuditoriaId** | Obtener auditados | @AuditoriaId | List<SGC_AI_AuditadoResult> |
| **SGC_AI_Auditorias_InformeAuditor_HallazgosByAuditoriaId** | Obtener hallazgos | @AuditoriaId | List<SGC_AI_HallazgoResult> |

### Acciones de Mejora (ACM)

| SP | Operación | Parámetros | Retorna |
|----|-----------|-----------|---------| 
| **ACM_AccionMejora_Add** | Crear acción | @DescripcionAccion, @FechaIncidente, @UsuarioReporta, @ProcesoId, ... | int (Id) |
| **ACM_AccionesMejora_Edit** | Actualizar acción | @Id, @DescripcionAccion, ... | int (rows) |

**Total SP**: 8 identificados ✅

---

## 🔐 USUARIOS Y ROLES

**Roles identificados** en WebMatrix:
- `ROL_CALIDAD = 45` - Gestor de Calidad (acceso total)
- `ROL_AUDITOR` - Auditor (solo sus auditorías)
- `ROL_AUDITADO` - Auditado (solo sus auditorías como auditado)

**Métodos de verificación**:
- `UsuarioTieneRolCalidad()` → Acceso a todas auditorías
- `UsuarioTieneRolAuditor()` → Solo auditorías asignadas como auditor
- Por defecto: Solo auditorías donde es auditado

---

## 📊 FLUJOS DE NEGOCIO

### 1. GESTIÓN DE AUDITORÍAS INTERNAS

```
┌─ Auditor (ROL_CALIDAD / ROL_AUDITOR)
│
├─ [1] Planificar Auditoría
│  ├─ Crear: SGC_AuditoriasInternas_Add
│  ├─ Datos: AreaAuditada, ProcesoAuditado, FechaLimite, Normativas, TiposAuditoria
│  └─ Estado: 20 (Creada)
│
├─ [2] Listar Auditorías
│  ├─ Filtrar: SGC_AI_AuditoriasBy
│  ├─ Por: EstadoId, AnoAuditoria, AuditorId (role-based)
│  └─ Paginación: pageIndex, pageSize
│
├─ [3] Ejecutar Auditoría
│  ├─ Cambio estado: 20 → 30 (Diligenciada)
│  ├─ Registrar: SGC_AI_AuditoriaInforme_Add
│  ├─ Datos: Fortalezas, Hallazgos (XML), Auditados, ArchivoInforme
│  └─ Log: SGC_AI_Auditorias_EstadosLog
│
└─ [4] Reportes
   └─ Recuperar: SGC_AI_Auditorias_InformeAuditorByAuditoriaId
      - Hallazgos, Auditados, Fortalezas, Archivo
```

**Estados Auditoría**:
- 20 = Creada
- 30 = Diligenciada por auditor (requiere aprobación)
- 40 = Aprobada
- 50 = Cerrada

### 2. GESTIÓN DE ACCIONES DE MEJORA

```
┌─ Usuario Calidad
│
├─ [1] Registrar Acción
│  ├─ Crear: ACM_AccionMejora_Add
│  ├─ Datos: DescripcionAccion, FechaIncidente, UsuarioReporta, ProcesoId
│  ├─ Relaciones: Causas (múltiples), PlanesAccion (múltiples)
│  └─ Fuentes: TipoFuente + FuenteId (auditoría, queja, etc.)
│
├─ [2] Agregar Detalles
│  ├─ Causas: AddCausas (subrututina)
│  ├─ PlanesAccion: AddPlanesAccion
│  └─ Fuentes: AddFuentes
│
├─ [3] Editar/Seguimiento
│  ├─ Actualizar: ACM_AccionesMejora_Edit
│  ├─ Estados: Abierta, En ejecución, Cerrada
│  └─ % Avance: Fechas planeado vs ejecutado
│
└─ [4] Eliminar
   ├─ Soft delete: IsDeleted = 1
   └─ Auditoría: Quién, cuándo, por qué
```

---

## 🎮 PÁGINAS WEBMATRIX → COMPONENTES MATRIXNEXT

| Página WebMatrix | Componentes MatrixNext | Acción Principal | Volumen |
|-----------------|----------------------|------------------|---------|
| SGC_Calidad_Home.aspx | Home/Dashboard SGC | Redireccionar a submodulos | 1 vista |
| SGC_AccionesMejora.aspx | Grid + Modal CRUD | Listar/Crear/Editar/Eliminar acciones | 1 controller + 2 servicios + grid |
| Auditor.aspx | Grid + Modal Nueva Auditoría + Informe | Listar/Crear/Diligenciar auditorías | 1 controller + 2 servicios + 2 grids |
| NuevaAuditoria.aspx | Modal Nueva Auditoría (reutilizable) | Crear + validar | 1 vista modal |

**Total Vistas**: 4 páginas → 3 vistas Razor (Home, Index AccionesMejora, Index Auditorías) + 3 modales

---

## 📦 ARTEFACTOS POR MIGRAR

### DTOs (Entity Models)

```csharp
// Auditorías Internas
SGC_AuditoriaDto
SGC_AuditoriaInternaEntity (Read)
SGC_AuditoriaInformeDto
SGC_HallazgoDto
SGC_AuditadoDto

// Acciones de Mejora
SGC_AccionMejoraDto
SGC_CausaDto
SGC_PlanAccionDto
SGC_FuenteNoConformidadDto
```

**Total DTOs**: 9

### Adapters

```csharp
// Interfaces
ISGCAuditoriaAdapter
ISGCAccionMejoraAdapter

// Implementaciones (Dapper)
SGCAuditoriaAdapter    → SGC_AuditoriasInternasDapper
SGCAccionMejoraAdapter → AccionesMejoraDapper
```

**Total Adapters**: 2 interfaces + 2 implementaciones

### Services

```csharp
// Interfaces
ISGCAuditoriaService
ISGCAccionMejoraService

// Implementaciones
SGCAuditoriaService    → Lógica auditorías, validaciones, permisos
SGCAccionMejoraService → Lógica acciones, seguimiento
```

**Total Services**: 2 interfaces + 2 servicios

### Controllers (REST)

```csharp
// Area: SGC
AuditoriasController
AccionesMejoraController
```

**Total Controllers**: 2 (REST)

### Vistas Razor

```
Areas/SGC/Views/
├── Auditorias/
│   ├── Index.cshtml              # Listado con filtros
│   ├── _CreateEdit.cshtml        # Modal CRUD
│   └── _InformeAuditor.cshtml    # Modal Informe
│
└── AccionesMejora/
    ├── Index.cshtml              # Listado con filtros
    ├── _CreateEdit.cshtml        # Modal CRUD
    └── _Detalles.cshtml          # Modal Detalles
```

**Total Vistas**: 6 archivos Razor

### JavaScript / CSS

```
wwwroot/
├── js/
│   └── sgc-utilities.js          # Manejo modales, validaciones, filtros
│
└── css/
    └── sgc.css                   # Estilos customizados
```

**Total JS/CSS**: 2 archivos

---

## ✅ CHECKLIST DE MIGRACIÓN

### Fase 1: Setup (4h)
- [ ] Crear carpeta `Areas/SGC/` con estructura estándar
- [ ] Crear carpeta `MatrixNext/docs/SGC/` para documentación
- [ ] Registrar DI en `Program.cs` (ISGCAuditoriaAdapter, ISGCAccionMejoraAdapter, services)
- [ ] Crear archivo de migracion `MIGRACION_SGC_CALIDAD.md`

### Fase 2: Data Access (8h)
- [ ] Crear DTOs (9 clases)
- [ ] Crear Adapter Interfaces (2)
- [ ] Implementar Adapters sobre Dapper (2) - mapear a 8 SP exactos
- [ ] Crear DbContext mappings (si aplica EF Core)
- [ ] Verificar SP en SQL Server (validar nombres exactos)

### Fase 3: Business Logic (8h)
- [ ] Crear Service Interfaces (2)
- [ ] Implementar Services (2) - incluir validaciones, permisos, logging
- [ ] Implementar INotificacionService si hay alertas
- [ ] Testeo unitario de lógica

### Fase 4: Controllers REST (6h)
- [ ] Crear AuditoriasController (8-10 endpoints)
- [ ] Crear AccionesMejoraController (8-10 endpoints)
- [ ] Validar [Authorize] + permisos role-based
- [ ] Manejo de errores (try/catch → JSON response)

### Fase 5: Vistas (8h)
- [ ] Crear Index Auditorías (grid + filtros)
- [ ] Crear Modal Nueva Auditoría
- [ ] Crear Modal Informe Auditor
- [ ] Crear Index Acciones Mejora (grid + filtros)
- [ ] Crear Modal CRUD Acciones
- [ ] Crear Modal Detalles Acciones
- [ ] Estilos CSS

### Fase 6: Integración JS (4h)
- [ ] AJAX modales (open, submit, close)
- [ ] Validaciones cliente (fechas, campos requeridos)
- [ ] Filtros dinámicos (estado, año, usuario)
- [ ] Paginación
- [ ] Toasts notificaciones

### Fase 7: QA y Documentación (4h)
- [ ] Testing funcional completo (CRUD, filtros, permisos)
- [ ] Completar MIGRACION_SGC_CALIDAD.md
- [ ] Actualizar menú en `_Sidebar.cshtml`
- [ ] Verificar build sin errores

**Estimación Total**: 42 horas ≈ 1.5 semanas (1 dev full-time)

---

## 🔗 DEPENDENCIAS

### Módulos Requeridos ANTES de SGC

1. ✅ **US_Usuarios** - Para roles (ROL_CALIDAD, ROL_AUDITOR) y auditoría
2. ✅ **GD_Documentos** - Para almacenar informes auditoría (archivos)
3. ✅ **TH_TalentoHumano** - Para lista empleados (auditados, auditores)

**Estado**: Todas disponibles en MatrixNext ✅

### Integraciones con Otros Módulos

- Usuarios de **TH** para dropdown auditores/auditados
- Procesos de **QMS** (si existe) para selector de procesos
- Notificaciones de **CORE** para alertas de auditoría

---

## 🎯 ORDEN RECOMENDADO DE IMPLEMENTACIÓN

```
Sprint 13 (4 semanas)
│
├─ Week 1: Fase 1-2 (Setup + Data Access)
├─ Week 2: Fase 3-4 (Services + Controllers)
├─ Week 3: Fase 5 (Vistas Razor)
├─ Week 4: Fase 6-7 (JS + QA)
│
└─ Entregable: SGC_Calidad 100% funcional + Documentación
```

---

## 📝 ENDPOINTS REST PRELIMINARES

### Auditorías Internas

```http
GET    /api/sgc/auditorias                    # Listar (filtros)
GET    /api/sgc/auditorias/{id}               # Detalle
POST   /api/sgc/auditorias                    # Crear
PUT    /api/sgc/auditorias/{id}               # Actualizar estado
GET    /api/sgc/auditorias/{id}/informe       # Obtener informe
POST   /api/sgc/auditorias/{id}/informe       # Registrar informe
DELETE /api/sgc/auditorias/{id}               # Eliminar (soft)
```

### Acciones de Mejora

```http
GET    /api/sgc/acciones-mejora               # Listar (filtros)
GET    /api/sgc/acciones-mejora/{id}          # Detalle completo
POST   /api/sgc/acciones-mejora               # Crear
PUT    /api/sgc/acciones-mejora/{id}          # Actualizar
DELETE /api/sgc/acciones-mejora/{id}          # Eliminar (soft)
GET    /api/sgc/acciones-mejora/tipos-fuente  # Catálogos
```

---

## ⚠️ RIESGOS Y CONSIDERACIONES

| Riesgo | Probabilidad | Impacto | Mitigación |
|--------|------------|--------|-----------|
| Archivos (PDF/DOC) informe auditor no se carga | Media | Alto | Usar GD_Documentos; path de upload validado |
| Validación de fechas (fecha límite ≠ fecha real) | Media | Medio | Validar en Service; mensaje claro al usuario |
| Permisos rol-based incorrectos | Alta | Alto | Test exhaustivo de roles; matriz de permisos |
| XML en Hallazgos (string) vs. estructura fuerte | Media | Medio | Parsear XML; crear entidad Hallazgo fuerte |
| Performance grid 1000+ auditorías | Baja | Medio | Paginación, índices SQL, caché |

---

## 🎓 GUÍA RÁPIDA PARA DEV

### Paso 1: Copiar Dapper Adapters
```vb
' CoreProject/Clases/SGC/SGC_AuditoriasInternasDapper.vb (187 líneas)
' CoreProject/Clases/REP/AccionesMejoraDapper.vb (450 líneas)
→ Traducir a C# / Implementar en MatrixNext
```

### Paso 2: Crear DTOs
Usar `SGC_AuditoriaInternaEntity`, `SGC_AccionMejoraDto` como base

### Paso 3: Mapear SP
- 8 SP SGC_AI_* → Métodos en Adapter
- Verificar nombres exactos en `CO_Matrix_SP_Names.csv`

### Paso 4: Implementar Service
- Lógica: Validar rol, verificar auditoría activa
- Logging: CRUD + cambios estado
- Notificaciones: Auditoría creada, informe enviado

### Paso 5: Crear Controllers
- GET con filtros (estado, año, usuario)
- POST con validaciones ModelState
- Manejo de archivos si es necesario

### Paso 6: Vistas Razor
- Grid paginado con Bootstrap
- Modales AJAX reutilizables
- Toasts para feedback

---

## 📚 REFERENCIAS

- **WebMatrix**: `WebMatrix/SGC_Calidad/`
- **CoreProject**: 
  - `CoreProject/Clases/SGC/SGC_AuditoriasInternasDapper.vb`
  - `CoreProject/Clases/REP/AccionesMejoraDapper.vb`
- **SQL**: `MatrixNext/docs/SQL/CO_Matrix_Structure_Tables.sql` (línea 11650+)
- **SP**: `MatrixNext/docs/SQL/CO_Matrix_SP_Names.csv` (líneas 8-9, 1198-1203)

---

**Documento**: ANALISIS_SGC_CALIDAD.md  
**Versión**: 1.0  
**Fecha**: 2026-01-15  
**Estado**: ✅ LISTO PARA INICIAR MIGRACIÓN SPRINT 13
