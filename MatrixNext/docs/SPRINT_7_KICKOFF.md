# Sprint 7: CORE Workflow/Tareas
**Duración**: 2 semanas (85 horas)  
**Prioridad**: 🔴 CRÍTICA (bloquea TH/OP/PY/GD)  
**Estado**: ⏳ IN PROGRESS

## Objetivo
Implementar la capa de gestión de tareas (WorkFlow) de CORE que permita:
1. CRUD completo de tareas con validación de estados
2. Asignación y escalada de tareas entre usuarios
3. Notificaciones en tiempo real para cambios de tareas
4. Auditoría y comentarios en tareas
5. Integración con módulos TH, OP, PY, GD, EQ

## Deliverables

### 1. **Core API Endpoints** (24h)
- `POST /api/CORE/tareas/crear` - Crear tarea
- `GET /api/CORE/tareas/{id}` - Obtener tarea
- `GET /api/CORE/tareas/usuario/{idUsuario}` - Listar tareas del usuario
- `PUT /api/CORE/tareas/{id}/actualizar` - Actualizar tarea
- `DELETE /api/CORE/tareas/{id}/anular` - Anular tarea
- `POST /api/CORE/tareas/{id}/asignar` - Asignar tarea
- `POST /api/CORE/tareas/{id}/escalar` - Escalar tarea
- `POST /api/CORE/tareas/{id}/cerrar` - Cerrar tarea

### 2. **Workflow Validation Service** (18h)
- Validación de transiciones de estado
- Validación de roles y permisos
- Reglas de negocio por módulo (TH/OP/PY/GD/EQ)

### 3. **Task Assignment & Escalation** (16h)
- Lógica de asignación automática
- Cadena de escalada
- Historial de asignaciones

### 4. **SignalR Real-time Notifications** (12h)
- Hub: `CoreNotificationsHub`
- Eventos: TaskCreated, TaskAssigned, TaskStateChanged, TaskEscalated, TaskClosed
- Subscriptions por usuario y rol

### 5. **Task Audit & Comments** (12h)
- `CoreTaskAudit` table - Auditoría de cambios
- `CoreTaskComments` table - Comentarios en tareas
- Endpoints para agregar comentarios

### 6. **Dashboard & UI** (15h)
- **Mis Tareas** - Tareas asignadas al usuario actual
- **Tareas Escaladas** - Tareas pendientes de escalar
- **Historial de Tareas** - Tareas completadas/anuladass
- Filtros: Estado, Prioridad, Módulo, Fecha

### 7. **JavaScript Modules** (8h)
- `core-tareas.js` - CRUD de tareas (420 LOC)
- `core-workflow.js` - Estado y transiciones (280 LOC)
- `core-notifications-client.js` - SignalR client (300 LOC)

## Architecture

```
Controllers/
  └── CoreController.cs (8 endpoints)

Services/
  ├── ICoreTaskService.cs / CoreTaskService.cs
  ├── ICoreWorkflowService.cs / CoreWorkflowService.cs
  ├── ICoreAssignmentService.cs / CoreAssignmentService.cs
  ├── ICoreNotificationService.cs / CoreNotificationService.cs
  └── ICoreAuditService.cs / CoreAuditService.cs

Hubs/
  └── CoreNotificationsHub.cs (SignalR)

Models/
  ├── WorkFlow.cs (exists)
  ├── WorkFlowUsuarioAsignado.cs (moved)
  ├── ObservacionTarea.cs (moved)
  ├── CoreTaskAudit.cs (new)
  └── CoreTaskComment.cs (new)

Views/
  ├── TareasIndex.cshtml
  ├── MisTareasPartial.cshtml
  ├── TareasEscaladasPartial.cshtml
  └── HistorialTareasPartial.cshtml

wwwroot/js/
  ├── core-tareas.js
  ├── core-workflow.js
  └── core-notifications-client.js
```

## Interfaz de Datos (Modelos Existentes)

### WorkFlow (ya existe)
- IdWorkFlow (PK)
- Descripcion
- Estado (Pendiente, EnProgreso, Completada, Anulada, Escalada)
- Prioridad (Baja, Normal, Alta, Urgente)
- IdUsuarioCreador
- IdUsuarioAsignado
- FechaCreacion
- FechaVencimiento
- FechaCierre
- Modulo (TH, OP, PY, GD, EQ)
- IdModulo (FK al módulo específico)

### WorkFlowUsuarioAsignado (moved to Web/Models/CORE)
- IdWorkFlowUsuario (PK)
- IdWorkFlow (FK)
- IdUsuario (FK)
- Rol (Ejecutor, Revisor, Supervisor)
- FechaAsignacion
- Activo

### ObservacionTarea (moved to Web/Models/CORE)
- IdObservacion (PK)
- IdWorkFlow (FK)
- IdUsuario (FK)
- Observacion
- TipoOperacion (Crear, Asignar, CambiarEstado, Anular, etc.)
- FechaHora

## Criterios de Aceptación
- ✅ 8 endpoints API funcionando correctamente
- ✅ Validación de transiciones de estado
- ✅ Asignación y escalada implementadas
- ✅ SignalR notificaciones en tiempo real
- ✅ UI completa (dashboard + views)
- ✅ 3 módulos JavaScript robustos
- ✅ Build limpio (0 errores)
- ✅ Integración con TH/OP/PY/GD validada

## Estimación de Horas
| Componente | Horas |
|-----------|-------|
| API Endpoints | 24 |
| Workflow Validation | 18 |
| Assignment & Escalation | 16 |
| SignalR Notifications | 12 |
| Audit & Comments | 12 |
| Dashboard & UI | 15 |
| JavaScript Modules | 8 |
| **TOTAL** | **85** |

## Dependencias
- Sprint 4: TH API (55 endpoints) ✅
- Sprint 5: TH Views/UI ✅
- Sprint 6: OP Complementos (Reports/Filters/Notifications) ✅
- Modelos CORE: ObservacionTarea, WorkFlowUsuarioAsignado ✅

## Bloqueadores Resueltos
- ✅ Directorio misplaced: `src/MatrixNext.Data/Models/CORE/` eliminado

## Status por Componente
| Item | Estado |
|------|--------|
| Estructura limpia | ✅ COMPLETADO |
| Modelos preparados | ✅ EN LUGAR CORRECTO |
| Controllers | ⏳ NOT STARTED |
| Services | ⏳ NOT STARTED |
| SignalR Hub | ⏳ NOT STARTED |
| Views | ⏳ NOT STARTED |
| JavaScript | ⏳ NOT STARTED |

---
**Próximo Paso**: Crear `CoreController.cs` con endpoints de tarea
