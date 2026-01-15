# Sprint 12.3.2: Aprobaciones y Rechazos Completos

**Ref**: BACKLOG_QA_MODULOS_PENDIENTES.md § Sprint 12.3.2  
**Duración**: 12h (completado)  
**Estado**: ✅ COMPLETADO  

---

## 📋 Descripción

Implementación de workflow completo de aprobaciones y rechazos de solicitudes de documentos con cambio automático de estado y notificaciones al solicitante.

---

## 🎯 Objetivos Alcanzados

✅ **DTOs extendidos** (SolicitudDocumentoDto.cs):
- AprobacionRevisionDto (8 propiedades)
- ResumenAprobacionDto (10 propiedades + 2 computed)

✅ **Adapter extendido** (SolicitudesAdapter.cs - +170 líneas):
- AprobarRevisionAsync
- RechazarRevisionAsync
- ObtenerResumenAprobacionAsync

✅ **Service extendido** (SolicitudesService.cs - +120 líneas):
- AprobarRevisionAsync (con cambio automático de estado)
- RechazarRevisionAsync (rechazo inmediato)
- ObtenerResumenAprobacionAsync
- Lógica de unanimidad vs mayoría

---

## 🏗️ Arquitectura del Workflow

### Flujo de Aprobación

```
1. Revisor aprueba revisión (btnAprobar_Click)
   ↓
2. Service.AprobarRevisionAsync(aprobacion)
   a. Validar IdRevision, IdRevisor
   b. Adapter.AprobarRevisionAsync() → SP: GD_Revisiones_Edit (TipoRevision=2)
   c. Actualizar ComentarioRevision (si existe)
   ↓
3. Adapter.ObtenerResumenAprobacionAsync(idSolicitud)
   a. Contar revisores por estado (Aprobado=2, Rechazado=3, Pendiente=1)
   b. Verificar RequiereAprobacionUnanimidad desde GD_ConfiguracionRevision
   c. Calcular EstadoFinal:
      - Si alguno rechazó → EstadoFinal = 3 (Rechazado)
      - Si todos aprobaron → EstadoFinal = 2 (Aprobado)
      - Si requiere unanimidad Y faltan revisores → EstadoFinal = 1 (Pendiente)
      - Si NO requiere unanimidad Y se alcanzó mayoría (50%+1) → EstadoFinal = 2
      - Caso contrario → EstadoFinal = 1 (Pendiente)
   ↓
4. Si EstadoFinal == 2 (Aprobado):
   a. Adapter.CambiarEstadoSolicitudAsync(idSolicitud, estado=2)
   b. Adapter.EnviarNotificacionRevisoresAsync(mensaje="Solicitud APROBADA")
   c. Log: "Solicitud aprobada automáticamente"
   ↓
5. Return mensaje con resumen
```

### Flujo de Rechazo

```
1. Revisor rechaza revisión con comentario obligatorio (btnRechazar_Click)
   ↓
2. Service.RechazarRevisionAsync(rechazo)
   a. Validar IdRevision, IdRevisor, ComentarioRevision NOT NULL
   b. Adapter.RechazarRevisionAsync() → SP: GD_Revisiones_Edit (TipoRevision=3)
   c. Actualizar ComentarioRevision (obligatorio)
   ↓
3. Cambio INMEDIATO de estado de solicitud:
   a. Adapter.CambiarEstadoSolicitudAsync(idSolicitud, estado=3)
   b. Adapter.EnviarNotificacionRevisoresAsync(mensaje="Solicitud RECHAZADA - Motivo: {comentario}")
   c. Log: "Solicitud rechazada automáticamente"
   ↓
4. Return mensaje confirmación
```

---

## 📊 Lógica de Agregación

### Reglas de Aprobación

| Escenario | Condición | Estado Final |
|-----------|-----------|--------------|
| **Rechazo** | Al menos 1 revisor rechazó | `3` (Rechazado) - Inmediato |
| **Unanimidad** | RequiereUnanimidad=true Y todos aprobaron | `2` (Aprobado) |
| **Unanimidad Pendiente** | RequiereUnanimidad=true Y faltan revisores | `1` (Pendiente) |
| **Mayoría** | RequiereUnanimidad=false Y aprobados >= (total/2)+1 | `2` (Aprobado) |
| **Mayoría Pendiente** | RequiereUnanimidad=false Y aprobados < mayoría | `1` (Pendiente) |

### Ejemplo con 5 Revisores (Unanimidad=false)

| Aprobados | Rechazados | Pendientes | Mayoría Necesaria | Estado Final |
|-----------|------------|------------|-------------------|--------------|
| 5 | 0 | 0 | 3 | `2` Aprobado |
| 3 | 0 | 2 | 3 | `2` Aprobado (mayoría alcanzada) |
| 2 | 0 | 3 | 3 | `1` Pendiente (faltan 1) |
| 2 | 1 | 2 | - | `3` Rechazado (alguno rechazó) |

### Ejemplo con 3 Revisores (Unanimidad=true)

| Aprobados | Rechazados | Pendientes | Estado Final |
|-----------|------------|------------|--------------|
| 3 | 0 | 0 | `2` Aprobado (todos) |
| 2 | 0 | 1 | `1` Pendiente (falta 1) |
| 2 | 1 | 0 | `3` Rechazado (alguno rechazó) |

---

## 🔐 Validaciones Implementadas

### AprobacionRevisionDto

```csharp
1. IdRevision > 0 → "Id de revisión inválido"
2. IdRevisor > 0 → "Revisor inválido"
3. (Automático) TipoRevision = 2 (Service asigna)
4. (Automático) FechaRevision = DateTime.Now
```

### RechazarRevisionDto

```csharp
1. IdRevision > 0 → "Id de revisión inválido"
2. IdRevisor > 0 → "Revisor inválido"
3. ComentarioRevision NOT NULL → "El comentario es obligatorio para rechazos"
4. (Automático) TipoRevision = 3
5. (Automático) FechaRevision = DateTime.Now
```

---

## 📦 Stored Procedures Mapeados

| SP | Parámetros | Uso |
|----|------------|-----|
| **GD_Revisiones_Edit** | @IdRevision, @DocumentoId, @UsuarioId, @FechaAprobacion, @TipoRevision | Cambiar estado de revisión (2=Aprobado, 3=Rechazado) |
| **GD_SolicitudDocumentos_CambiarEstado** (reutilizado de 12.3.1) | @IdSolicitud, @IdEstado, @ModificadoPor, @Observaciones | Cambiar estado de solicitud tras aprobación/rechazo |
| **GD_Email_EnviarNotificacion** (reutilizado de 12.3.1) | @IdSolicitud, @Contenido, @TipoNotificacion | Notificar al solicitante |

**Nota**: Se reutilizan 2 SPs del Sprint 12.3.1. Solo se agrega uso de `GD_Revisiones_Edit` con TipoRevision=2 y 3.

---

## 📊 Estadísticas

| Métrica | Valor |
|---------|-------|
| **Líneas agregadas** | DTO (+60) + Adapter (+170) + Service (+120) = 350 LOC |
| **DTOs nuevos** | 2 (AprobacionRevisionDto, ResumenAprobacionDto) |
| **Métodos Adapter** | +3 (AprobarRevisionAsync, RechazarRevisionAsync, ObtenerResumenAprobacionAsync) |
| **Métodos Service** | +3 (AprobarRevisionAsync, RechazarRevisionAsync, ObtenerResumenAprobacionAsync) |
| **SPs mapeados** | 1 (GD_Revisiones_Edit con TipoRevision=2/3) |
| **Errores compilación** | 0 ✅ |

---

## ✅ Checklist Pre-Deploy

- [x] Compilación sin errores
- [x] DTOs con validaciones
- [x] Adapter con SP GD_Revisiones_Edit
- [x] Service con lógica de unanimidad/mayoría
- [x] Cambio automático de estado tras aprobación/rechazo
- [x] Notificación automática al solicitante
- [x] Logging completo (INFO, ERROR)
- [x] Comentario obligatorio para rechazos
- [x] Resumen de aprobaciones con computed properties

---

## 🔗 Integración

### Sprints Previos
- ✅ Sprint 12.3.1: Usa RevisorDto, AsignacionRevisoresDto, ConfiguracionRevisionDto
- ✅ Sprint 12.3.1: Reutiliza CambiarEstadoSolicitudAsync, EnviarNotificacionRevisoresAsync

### Próximos Sprints
- ⏳ Sprint 12.3.3: Audit Trail (historizar aprobaciones/rechazos con timestamp)
- ⏳ Sprint 12.3.4: Testing End-to-End (crear → asignar → aprobar → verificar estado final)

---

## 🧪 Casos de Prueba Sugeridos

### Caso 1: Aprobación Unánime (3 revisores, unanimidad=true)
```
1. Crear solicitud con IdProceso que tiene RequiereAprobacionUnanimidad=true
2. Asignar 3 revisores automáticamente
3. Revisor 1 aprueba → Estado solicitud = Pendiente
4. Revisor 2 aprueba → Estado solicitud = Pendiente
5. Revisor 3 aprueba → Estado solicitud = Aprobado ✅
6. Verificar notificación enviada al solicitante
```

### Caso 2: Rechazo Inmediato
```
1. Crear solicitud con 5 revisores
2. Revisor 1 aprueba → Estado = Pendiente
3. Revisor 2 aprueba → Estado = Pendiente
4. Revisor 3 rechaza con comentario "Falta documentación" → Estado = Rechazado ✅ (inmediato)
5. Verificar notificación con motivo de rechazo
6. Verificar que revisores 4 y 5 NO pueden aprobar (solicitud ya rechazada)
```

### Caso 3: Mayoría Simple (5 revisores, unanimidad=false)
```
1. Crear solicitud con RequiereAprobacionUnanimidad=false
2. Asignar 5 revisores
3. Revisor 1 aprueba → Estado = Pendiente (1/3 necesarios)
4. Revisor 2 aprueba → Estado = Pendiente (2/3 necesarios)
5. Revisor 3 aprueba → Estado = Aprobado ✅ (3/3 mayoría alcanzada)
6. Verificar que revisores 4 y 5 quedan pendientes pero solicitud ya aprobada
```

---

**Documento completado**: 2025-01-15  
**Última revisión**: Sprint 12.3.2  
**Estado de deploy**: LISTO PARA STAGING  
**Compilación**: ✅ Sin errores  
**Integración con 12.3.1**: ✅ Completada
