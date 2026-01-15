# Mapeo SP - Tráfico de Encuestas Completo (Sprint 12.1.9)

**Módulo**: OP (Operativo)  
**Funcionalidad**: Gestión completa de tráfico de encuestas entre unidades  
**Fecha**: 2026-01-15  
**Estado**: ✅ Completado  
**Verificación**: CoreProject → MatrixNext

---

## 1. Stored Procedures Identificados

| SP Nombre | Parámetros | Uso |
|-----------|-----------|-----|
| `OP_TraficoEncuestas_Enviar` | @IdTrabajo, @IdUnidadOrigen, @IdUnidadDestino, @Cantidad, @Ciudad, @Observaciones, @EnviadoPor | Registra envío |
| `OP_TraficoEncuestas_Recibir` | @IdMovimiento, @CantidadRecibida, @RecibidoPor, @ObservacionesDiscrepancia | Registra recepción |
| `OP_TraficoEncuestas_PersonalAsignar` | @IdMovimiento, @IdEmpleado, @Cargo, @CantidadAsignada, @AsignadoPor | Asigna personal |

---

## 2. Validaciones Implementadas

### Envío
- ✅ Permiso 117/118/119/120 según unidad
- ✅ Cantidad disponible en unidad origen
- ✅ Ciudad requerida para RMC (unidad 119/120)

### Recepción
- ✅ Estado "EnTransito" válido
- ✅ Observaciones si hay discrepancia
- ✅ Actualización automática de estado según cantidad

### Personal
- ✅ Cargos válidos: Encuestador, Supervisor, Crítico, Digitador, RMC
- ✅ Cantidad positiva

---

## 3. Flujo Completo

1. **Envío**: Unidad Verificación → Unidad Captura (cantidad: 500)
2. **Recepción**: Unidad Captura recibe 495 → Discrepancia: 5
3. **Devolución**: Unidad Captura devuelve 10 mal llenadas → Unidad Verificación
4. **Asignación**: Personal asignado a movimiento específico

---

**Documento creado**: 2026-01-15  
**Completitud**: 100%  
**Listo para QA**: ✅ Sí
