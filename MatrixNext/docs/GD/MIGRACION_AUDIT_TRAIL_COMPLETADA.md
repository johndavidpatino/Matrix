# Sprint 12.3.3: Audit Trail de Revisiones

**Ref**: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.3.3  
**Duración**: 8h (completado)  
**Estado**: ✅ COMPLETADO  

---

## 📋 Descripción

Implementación de Audit Trail (historial de revisiones) para solicitudes de documentos, permitiendo visualizar timeline completo de eventos: asignaciones, aprobaciones y rechazos con timestamps y comentarios.

---

## 🎯 Objetivos Alcanzados

✅ **DTOs extendidos** (SolicitudDocumentoDto.cs - +100 líneas):
- HistorialRevisionDto (15 propiedades + computed)
- TimelineSolicitudDto (6 propiedades + computed)

✅ **Adapter extendido** (SolicitudesAdapter.cs - +110 líneas):
- ObtenerHistorialRevisionesAsync
- ObtenerTimelineSolicitudAsync

✅ **Service extendido** (SolicitudesService.cs - +50 líneas):
- ObtenerHistorialRevisionesAsync
- ObtenerTimelineSolicitudAsync

✅ **Tabla reutilizada**: GD_Revisiones (ya existente)
- FechaAsignacion → Captura cuándo se asignó revisor
- FechaRevision → Captura cuándo aprobó/rechazó
- TipoRevision → 1=Pendiente, 2=Aprobado, 3=Rechazado
- ComentarioRevision → Razón del rechazo/aprobación

---

## 🏗️ Arquitectura del Audit Trail

### Flujo de Obtención de Historial

```
1. Usuario solicita ver historial de solicitud (modal)
   ↓
2. Controller.GetHistorial(idSolicitud)
   ↓
3. Service.ObtenerHistorialRevisionesAsync(idSolicitud)
   ↓
4. Adapter.ObtenerHistorialRevisionesAsync()
   a. Query JOIN GD_Revisiones + TH_Empleado
   b. SELECT:
      - IdRevision, IdRevisor, NombreRevisor, EmailRevisor
      - OrdenRevision, TipoRevision, TipoRevisionTexto
      - FechaAsignacion, FechaRevision, ComentarioRevision
      - Computed: Accion, AccionClass, AccionIcon, DiasTranscurridos
   c. ORDER BY OrdenRevision, FechaAsignacion
   ↓
5. Return List<HistorialRevisionDto> ordenado cronológicamente
```

### Flujo de Obtención de Timeline Completo

```
1. Usuario solicita ver timeline completo (modal con tabs)
   ↓
2. Controller.GetTimeline(idSolicitud)
   ↓
3. Service.ObtenerTimelineSolicitudAsync(idSolicitud)
   a. Obtener solicitud (FechaSolicitud, NumeroSolicitud, EstadoActual)
   b. Obtener historial de revisiones (eventos)
   c. Combinar en TimelineSolicitudDto
   d. Calcular UltimaActividad (MAX FechaRevision)
   ↓
4. Return TimelineSolicitudDto con:
   - Datos de solicitud (cabecera)
   - Lista de eventos ordenados (timeline visual)
   - Estadísticas (TotalEventos, UltimaActividad)
```

---

## 📊 Estructura de Datos

### HistorialRevisionDto

| Campo | Tipo | Descripción | Origen |
|-------|------|-------------|--------|
| **IdRevision** | long | PK de GD_Revisiones | DB |
| **IdSolicitud** | long | FK a solicitud | DB |
| **IdRevisor** | long | FK a TH_Empleado | DB |
| **NombreRevisor** | string | JOIN TH_Empleado.NombreCompleto | DB |
| **EmailRevisor** | string | JOIN TH_Empleado.Email | DB |
| **OrdenRevision** | int | Secuencia 1, 2, 3... | DB |
| **TipoRevision** | int | 1=Pendiente, 2=Aprobado, 3=Rechazado | DB |
| **TipoRevisionTexto** | string | CASE WHEN | Computed |
| **FechaAsignacion** | DateTime? | Cuándo se asignó | DB |
| **FechaRevision** | DateTime? | Cuándo revisó | DB |
| **ComentarioRevision** | string | Razón/comentario | DB |
| **Accion** | string | "Asignado", "Aprobado", "Rechazado" | Computed |
| **AccionClass** | string | "info", "success", "danger" | Computed (CSS) |
| **AccionIcon** | string | "fa-clock", "fa-check-circle", "fa-times-circle" | Computed (FA) |
| **DiasTranscurridos** | int | DATEDIFF(FechaAsignacion, FechaRevision o NOW) | Computed |

### TimelineSolicitudDto

| Campo | Tipo | Descripción |
|-------|------|-------------|
| **IdSolicitud** | long | ID de la solicitud |
| **NumeroSolicitud** | string | Número de solicitud (ej: SOL-2026-001) |
| **FechaSolicitud** | DateTime | Fecha de creación |
| **EstadoActual** | string | "Pendiente", "Aprobado", "Rechazado" |
| **Eventos** | List<HistorialRevisionDto> | Todos los eventos cronológicos |
| **TotalEventos** | int | Count de eventos (computed) |
| **UltimaActividad** | DateTime? | MAX FechaRevision (computed) |

---

## 🎨 Query SQL del Historial

```sql
SELECT 
    r.IdRevision,
    r.IdSolicitud,
    r.IdRevisor,
    e.NombreCompleto as NombreRevisor,
    e.Email as EmailRevisor,
    r.OrdenRevision,
    r.TipoRevision,
    CASE r.TipoRevision
        WHEN 1 THEN 'Pendiente'
        WHEN 2 THEN 'Aprobado'
        WHEN 3 THEN 'Rechazado'
        ELSE 'Desconocido'
    END as TipoRevisionTexto,
    r.FechaAsignacion,
    r.FechaRevision,
    r.ComentarioRevision,
    CASE 
        WHEN r.TipoRevision = 1 THEN 'Asignado'
        WHEN r.TipoRevision = 2 THEN 'Aprobado'
        WHEN r.TipoRevision = 3 THEN 'Rechazado'
        ELSE 'Asignado'
    END as Accion,
    CASE 
        WHEN r.TipoRevision = 1 THEN 'info'
        WHEN r.TipoRevision = 2 THEN 'success'
        WHEN r.TipoRevision = 3 THEN 'danger'
        ELSE 'secondary'
    END as AccionClass,
    CASE 
        WHEN r.TipoRevision = 1 THEN 'fa-clock'
        WHEN r.TipoRevision = 2 THEN 'fa-check-circle'
        WHEN r.TipoRevision = 3 THEN 'fa-times-circle'
        ELSE 'fa-user'
    END as AccionIcon,
    DATEDIFF(DAY, r.FechaAsignacion, ISNULL(r.FechaRevision, GETDATE())) as DiasTranscurridos
FROM GD_Revisiones r
LEFT JOIN TH_Empleado e ON e.IdEmpleado = r.IdRevisor
WHERE r.IdSolicitud = @IdSolicitud
ORDER BY r.OrdenRevision, r.FechaAsignacion
```

**Notas**:
- LEFT JOIN con TH_Empleado para obtener datos del revisor
- CASE WHEN para computed fields (Accion, AccionClass, AccionIcon)
- DATEDIFF para calcular días transcurridos (si aún pendiente, hasta hoy)
- ORDER BY para cronología correcta

---

## 🔐 Validaciones Implementadas

1. **IdSolicitud** > 0 → Requerido para obtener historial
2. **Logging** completo en cada operación (INFO, ERROR)
3. **Manejo de nulls**: TimelineSolicitudDto.UltimaActividad puede ser null si no hay revisiones
4. **Ordenamiento**: Eventos ordenados por OrdenRevision y FechaAsignacion

---

## 📦 Estadísticas

| Métrica | Valor |
|---------|-------|
| **Líneas agregadas** | DTO (+100) + Adapter (+110) + Service (+50) = 260 LOC |
| **DTOs nuevos** | 2 (HistorialRevisionDto, TimelineSolicitudDto) |
| **Métodos Adapter** | +2 (ObtenerHistorialRevisionesAsync, ObtenerTimelineSolicitudAsync) |
| **Métodos Service** | +2 (ObtenerHistorialRevisionesAsync, ObtenerTimelineSolicitudAsync) |
| **Tablas reutilizadas** | 1 (GD_Revisiones - ya existente, NO se creó tabla nueva) |
| **JOINs** | 1 (GD_Revisiones ← TH_Empleado) |
| **Errores compilación** | 0 ✅ |

---

## ✅ Checklist Pre-Deploy

- [x] Compilación sin errores
- [x] DTOs con computed properties (AccionClass, AccionIcon, DiasTranscurridos)
- [x] Adapter con query optimizado (JOIN, CASE WHEN, ORDER BY)
- [x] Service con validaciones y logging
- [x] Timeline completo (solicitud + eventos)
- [x] Reutilización de tabla GD_Revisiones existente (NO se crea nueva tabla)
- [x] Logging completo (INFO, ERROR, WARNING)
- [x] Manejo de nulls (TimelineSolicitudDto.UltimaActividad)

---

## 🎨 Diseño de Vista (Propuesta)

### Modal de Historial (Timeline Vertical)

```html
<div class="modal" id="modalHistorial">
    <div class="modal-header">
        <h5>Historial de Solicitud: SOL-2026-001</h5>
        <span class="badge badge-success">Aprobado</span>
    </div>
    <div class="modal-body">
        <div class="timeline">
            <!-- Evento 1: Asignación -->
            <div class="timeline-item">
                <div class="timeline-marker bg-info">
                    <i class="fa fa-clock"></i>
                </div>
                <div class="timeline-content">
                    <h6>Asignado a Juan Pérez</h6>
                    <small class="text-muted">15/01/2026 09:30 AM</small>
                    <p>Orden de revisión: 1</p>
                </div>
            </div>

            <!-- Evento 2: Aprobación -->
            <div class="timeline-item">
                <div class="timeline-marker bg-success">
                    <i class="fa fa-check-circle"></i>
                </div>
                <div class="timeline-content">
                    <h6>Aprobado por Juan Pérez</h6>
                    <small class="text-muted">15/01/2026 11:45 AM</small>
                    <p class="text-muted">Comentario: "Aprobado, documentación completa"</p>
                    <span class="badge badge-light">2.25 horas</span>
                </div>
            </div>

            <!-- Evento 3: Rechazo -->
            <div class="timeline-item">
                <div class="timeline-marker bg-danger">
                    <i class="fa fa-times-circle"></i>
                </div>
                <div class="timeline-content">
                    <h6>Rechazado por María López</h6>
                    <small class="text-muted">15/01/2026 02:15 PM</small>
                    <p class="text-danger">Razón: "Falta firma en página 3"</p>
                    <span class="badge badge-light">4.75 horas</span>
                </div>
            </div>
        </div>
    </div>
    <div class="modal-footer">
        <p class="text-muted">
            Total eventos: 5 | Última actividad: 15/01/2026 02:15 PM
        </p>
    </div>
</div>
```

**CSS Bootstrap 5**:
- `.timeline-item`: Cada evento en el historial
- `.timeline-marker`: Círculo con ícono (bg-info, bg-success, bg-danger)
- `.timeline-content`: Detalles del evento
- `.badge`: Estado y duración

---

## 🔗 Integración

### Sprints Previos
- ✅ Sprint 12.3.1: Usa RevisorDto (FechaAsignacion, OrdenRevision)
- ✅ Sprint 12.3.2: Usa AprobacionRevisionDto (TipoRevision, FechaRevision, ComentarioRevision)

### Próximos Sprints
- ⏳ Sprint 12.3.4: Testing End-to-End (validar historial en casos de prueba)
- ⏳ Controller: Endpoints para GET /Solicitudes/GetHistorial/{id} y GET /Solicitudes/GetTimeline/{id}
- ⏳ View: Modal _HistorialModal.cshtml con timeline vertical Bootstrap

---

## 🧪 Casos de Prueba Sugeridos

### Caso 1: Timeline con 3 Revisores (todos aprobaron)
```
1. Crear solicitud SOL-2026-001
2. Asignar 3 revisores (Juan, María, Pedro)
3. Juan aprueba → Verificar evento en historial
4. María aprueba → Verificar evento
5. Pedro aprueba → Verificar evento + solicitud aprobada
6. Obtener timeline → Verificar 3 eventos + UltimaActividad = FechaRevision de Pedro
7. Verificar DiasTranscurridos calculado correctamente
```

### Caso 2: Timeline con Rechazo
```
1. Crear solicitud SOL-2026-002
2. Asignar 2 revisores
3. Revisor 1 rechaza con comentario → Verificar evento con Accion="Rechazado", AccionClass="danger"
4. Obtener timeline → Verificar ComentarioRevision visible
5. Verificar solicitud estado = Rechazado
```

### Caso 3: Timeline con Revisores Pendientes
```
1. Crear solicitud SOL-2026-003
2. Asignar 5 revisores
3. Revisor 1 aprueba → 1 evento
4. Revisor 2 aprueba → 2 eventos
5. Revisores 3, 4, 5 aún pendientes
6. Obtener timeline → Verificar 5 eventos (2 aprobados, 3 asignados pendientes)
7. Verificar DiasTranscurridos para pendientes = DATEDIFF(FechaAsignacion, GETDATE())
```

---

## 📊 Ejemplo de JSON Response

### GET /Solicitudes/GetHistorial/123

```json
[
  {
    "idRevision": 1,
    "idSolicitud": 123,
    "idRevisor": 45,
    "nombreRevisor": "Juan Pérez",
    "emailRevisor": "juan.perez@company.com",
    "ordenRevision": 1,
    "tipoRevision": 2,
    "tipoRevisionTexto": "Aprobado",
    "fechaAsignacion": "2026-01-15T09:30:00",
    "fechaRevision": "2026-01-15T11:45:00",
    "comentarioRevision": "Aprobado, documentación completa",
    "accion": "Aprobado",
    "accionClass": "success",
    "accionIcon": "fa-check-circle",
    "diasTranscurridos": 0
  },
  {
    "idRevision": 2,
    "idSolicitud": 123,
    "idRevisor": 67,
    "nombreRevisor": "María López",
    "emailRevisor": "maria.lopez@company.com",
    "ordenRevision": 2,
    "tipoRevision": 1,
    "tipoRevisionTexto": "Pendiente",
    "fechaAsignacion": "2026-01-15T09:30:00",
    "fechaRevision": null,
    "comentarioRevision": null,
    "accion": "Asignado",
    "accionClass": "info",
    "accionIcon": "fa-clock",
    "diasTranscurridos": 1
  }
]
```

### GET /Solicitudes/GetTimeline/123

```json
{
  "idSolicitud": 123,
  "numeroSolicitud": "SOL-2026-001",
  "fechaSolicitud": "2026-01-15T09:00:00",
  "estadoActual": "Pendiente",
  "eventos": [ /* array de HistorialRevisionDto */ ],
  "totalEventos": 5,
  "ultimaActividad": "2026-01-15T11:45:00"
}
```

---

**Documento completado**: 2025-01-15  
**Última revisión**: Sprint 12.3.3  
**Estado de deploy**: LISTO PARA STAGING  
**Compilación**: ✅ Sin errores  
**Integración con 12.3.1 y 12.3.2**: ✅ Completada  
**Tabla nueva creada**: ❌ NO (reutiliza GD_Revisiones existente)
