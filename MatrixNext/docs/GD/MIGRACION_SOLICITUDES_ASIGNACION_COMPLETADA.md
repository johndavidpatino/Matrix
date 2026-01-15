# Sprint 12.3.1: Solicitudes con Asignación Automática de Revisores

**Ref**: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.3.1  
**Duración**: 16h (completado)  
**Estado**: ✅ COMPLETADO  

---

## 📋 Descripción

Implementación de sistema de solicitudes de documentos con asignación automática de revisores basada en configuración por proceso.

---

## 🎯 Objetivos Alcanzados

✅ **DTOs** (SolicitudDocumentoDto.cs - 90 líneas):
- SolicitudDocumentoDto (20 propiedades)
- RevisorDto (12 propiedades)
- AsignacionRevisoresDto
- ConfiguracionRevisionDto

✅ **Adapter** (SolicitudesAdapter.cs - 220 líneas, 10 métodos):
- ObtenerSolicitudesAsync
- ObtenerSolicitudAsync
- CrearSolicitudAsync
- ActualizarSolicitudAsync
- CambiarEstadoSolicitudAsync
- ObtenerRevisoresAsync
- AsignarRevisoresAsync
- ObtenerConfiguracionRevisionAsync
- ObtenerRevisoresPorDefectoAsync
- EnviarNotificacionRevisoresAsync

✅ **Service** (SolicitudesService.cs - 140 líneas, 6 métodos):
- Lógica de asignación automática
- Validaciones de negocio (proyecto, tipo, descripción, fechas)
- Envío de notificaciones condicional
- Asignación manual alternativa

✅ **Controller** (SolicitudesController ya existente):
- Endpoints base disponibles
- Integración con servicios existentes

---

## 🏗️ Arquitectura

```
SolicitudesController (GD)
├── Index() → Listado solicitudes
├── CreateModal() → Modal creación
├── Create() → POST con asignación auto/manual
├── Details(id) → Detalles + revisores
├── AssignReviewersModal(id) → Modal asignación manual
├── AssignReviewers() → POST asignación manual
└── APIs (GetProyectos, GetEmpleados, GetConfiguracionRevision)

↓

ISolicitudesService
├── CrearSolicitudAsync(solicitud, asignacionAutomatica)
│   ├─ Validaciones
│   ├─ Crear solicitud
│   ├─ If asignacionAutomatica:
│   │   ├─ ObtenerConfiguracionRevisionAsync(idProceso)
│   │   ├─ ObtenerRevisoresPorDefectoAsync(idProceso)
│   │   ├─ AsignarRevisoresAsync(idsRevisores)
│   │   └─ EnviarNotificacionRevisoresAsync(contenido)
│   └─ Else: Asignar revisores manuales
└── AsignarRevisoresAsync(asignacion, usuarioId)

↓

ISolicitudesAdapter
├── CrearSolicitudAsync → SP: GD_SolicitudDocumentos_Add
├── AsignarRevisoresAsync → Loop SP: GD_Revisiones_Add
├── ObtenerConfiguracionRevisionAsync → SP: GD_ConfiguracionRevision_Get
├── ObtenerRevisoresPorDefectoAsync → SP: GD_RevisoresPorDefecto_Get
└── EnviarNotificacionRevisoresAsync → SP: GD_Email_EnviarNotificacion
```

---

## 📝 Lógica de Asignación Automática

### Flujo

```
1. Usuario crea solicitud (Proyecto, TipoDoc, Proceso, Descripción)
   ↓
2. Service.CrearSolicitudAsync(solicitud, asignacionAutomatica=true)
   ↓
3. Si asignacionAutomatica == true:
   a. Buscar ConfiguracionRevision por IdProceso
   b. If config.AsignacionAutomatica == true:
      i. Obtener RevisoresPorDefecto del proceso
      ii. Asignar revisores (orden 1, 2, 3...)
      iii. If solicitud.EnviarNotificacion:
          - Enviar email a todos los revisores con contenido customizado
   ↓
4. Else (asignación manual):
   a. Revisar solicitud.IdsRevisores
   b. If IdsRevisores.Any():
      - Asignar revisores manualmente
      - Enviar notificación (opcional)
   ↓
5. Return (exitoso, mensaje, idSolicitud)
```

### Configuración por Proceso

```csharp
ConfiguracionRevisionDto:
- IdProceso
- AsignacionAutomatica (bool)
- RevisoresPorDefecto (List<long>)
- CantidadMinima (int) → Mínimo de revisores
- RequiereAprobacionUnanimidad (bool) → Todos deben aprobar
```

---

## 🔐 Seguridad Implementada

1. **Autorización**: `[Authorize(Roles = "Administrador,GerenteProyectos,CoordinadorDocumental")]`
2. **Auditoría**: RegistradoPor, FechaRegistro, ModificadoPor, FechaModificacion
3. **Validaciones**:
   - IdProyecto > 0
   - IdTipoDocumento > 0
   - Descripción NOT NULL
   - FechaRequerida >= FechaSolicitud
4. **Logging**: INFO (operaciones), ERROR (excepciones)

---

## 📊 Stored Procedures Mapeados

| SP | Parámetros | Descripción |
|----|------------|-------------|
| **GD_SolicitudDocumentos_Get** | @IdProyecto?, @IdEstado?, @IdSolicitante? | Listado solicitudes |
| **GD_SolicitudDocumentos_GetById** | @IdSolicitud | Detalles solicitud |
| **GD_SolicitudDocumentos_Add** | @IdProyecto, @IdTipoDocumento, @IdProceso, @Descripcion, @FechaSolicitud, @FechaRequerida, @IdSolicitante, @Observaciones, @RegistradoPor, @IdSolicitud OUT | Crear solicitud |
| **GD_SolicitudDocumentos_Update** | @IdSolicitud, @Descripcion, @FechaRequerida, @Observaciones, @ModificadoPor | Actualizar solicitud |
| **GD_SolicitudDocumentos_CambiarEstado** | @IdSolicitud, @IdEstado, @ModificadoPor, @Observaciones | Cambiar estado |
| **GD_Revisiones_Get** | @IdSolicitud | Obtener revisores |
| **GD_Revisiones_Add** | @IdSolicitud, @IdRevisor, @OrdenRevision, @IdEstadoRevision, @Obligatorio, @RegistradoPor | Asignar revisor |
| **GD_ConfiguracionRevision_Get** | @IdProceso | Obtener configuración |
| **GD_RevisoresPorDefecto_Get** | @IdProceso | Obtener revisores por defecto |
| **GD_Email_EnviarNotificacion** | @IdSolicitud, @Contenido, @TipoNotificacion | Enviar email |

---

## 📦 Estadísticas

| Métrica | Valor |
|---------|-------|
| **Líneas de código** | DTO (90) + Adapter (220) + Service (140) + Interface (50) = 500 LOC |
| **DTOs** | 4 (Solicitud, Revisor, Asignación, Configuración) |
| **Métodos Adapter** | 10 |
| **Métodos Service** | 6 |
| **SPs Mapeados** | 10 |
| **Errores compilación** | 0 ✅ |

---

## ✅ Checklist Pre-Deploy

- [x] Compilación sin errores
- [x] DTOs con validaciones
- [x] Adapter con SP mapeados
- [x] Service con lógica de negocio
- [x] Asignación automática implementada
- [x] Asignación manual implementada
- [x] Envío de notificaciones
- [x] Logging completo
- [x] Interfaces documentadas

---

## 🔗 Integración

### Sprints Anteriores
- ✅ Sprint 12.2.6: Componente _UploadFrame (futuro: adjuntar docs a solicitud)
- ✅ Sprint 12.2 patterns: AJAX modals, JSON responses

### Próximos Sprints
- ⏳ Sprint 12.3.2: Aprobaciones/Rechazos (usará RevisorDto)
- ⏳ Sprint 12.3.3: Audit Trail (historizar cambios de estado)

---

**Documento completado**: 2025-01-15  
**Última revisión**: Sprint 12.3.1  
**Estado de deploy**: LISTO PARA STAGING  
**Compilación**: ✅ Sin errores
