# 📊 Sprint 4 - Progress Tracking

## Objetivo Sprint 4
Implementar Validación de Participantes en Programación de Campo (OP-P01), habilitando endpoint de validación AJAX y reglas básicas de negocio.

---

## ✅ Entregables (Parciales)

### Backend
- ✅ IOpProgramacionService: extendido con `ValidarParticipantesAsync(trabajoId, ids, fechaProgramada)`
- ✅ OpProgramacionService: implementación con Dapper (consultas a `OP_MuestraTrabajos` y `OP_Programados_Entrevistados`)
- ✅ Modelos: `ParticipanteValidacionVm` agregado en `ProgramacionIpsVms.cs`

### Controller
- ✅ `CualitativoProgramacionController`: acción AJAX `ValidateParticipants` (POST + CSRF) con DTO `ValidateParticipantsRequest`

### Views
- ⏳ Integración UI: modal de validación en `Edit.cshtml` con AJAX (pendiente)

### Build
- ✅ Build SUCCESS (22 warnings de nullability pre-existentes)

---

## 🔍 Detalles

### Reglas de Validación Implementadas
- Existencia del participante en `OP_MuestraTrabajos` por `TrabajoId`
- Disponibilidad: no programado con estado 3/4 (Confirmado/Ejecutado) en fecha futura o misma fecha
- Duplicados en selección
- Resumen por participante: `Disponible`, `MotivoNoValido`, `ProgramacionesPrevias`, `UltimaProgramacion`

### Endpoints
- POST `OP/Cualitativo/Programacion/ValidateParticipants`
  - Body: `{ ids: number[], fechaProgramada?: string }`
  - Respuesta: `{ success: boolean, data?: ParticipanteValidacionVm[], message?: string }`

---

## 📈 Próximos pasos
- [ ] Agregar modal de validación en `Views/CualitativoProgramacion/Edit.cshtml`
- [ ] Botón "Validar participantes" que llama al endpoint AJAX
- [ ] Mostrar tabla con columnas: Participante, Disponible, Motivo, ProgramacionesPrevias, ÚltimaProgramación
- [ ] Opcional: bloqueo de submit si hay no válidos

## Métricas
```
Código nuevo: ~160 LOC (servicio + controller + modelo)
Build:       SUCCESS (0 errores)
```
