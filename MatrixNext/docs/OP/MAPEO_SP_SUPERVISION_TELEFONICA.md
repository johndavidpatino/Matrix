# Mapeo SP - Supervisión Telefónica (Sprint 12.1.10)

**Módulo**: OP (Operativo)  
**Funcionalidad**: Supervisión telefónica de operadores con checklist de evaluación  
**Fecha**: 2026-01-15  
**Estado**: ✅ Completado  
**Verificación**: CoreProject → MatrixNext

---

## 1. Stored Procedures Identificados

| SP Nombre | Parámetros | Uso |
|-----------|-----------|-----|
| `OP_SupervisionCampoTelefonico_Save` | @IdTrabajo, @IdOperador, @IdSupervisor, @NumeroEncuesta, @CalificacionTotal, @ResultadoSupervision, @Observaciones | Guarda supervisión |
| `OP_SupervisionCampoTelefonico_Get` | @IdTrabajo, @FechaInicio, @FechaFin | Consulta supervisiones |

---

## 2. Checklist de Evaluación (CRI)

Items estándar (ponderados):
- CRI1: Saludo y presentación (10%)
- CRI2: Lectura de preguntas completa (20%)
- CRI3: Captura de respuestas correcta (25%)
- CRI4: Manejo de objeciones (15%)
- CRI5: Cierre adecuado (10%)
- CRI6: Tiempo de llamada (10%)
- CRI7: Tono de voz apropiado (10%)

### Resultado Automático
- **Aprobado**: Calificación ≥ 80%
- **Observado**: Calificación 60-79%
- **Rechazado**: Calificación < 60%

---

## 3. Validaciones

- ✅ Permiso 157 (MyS/Call Center) requerido
- ✅ Checklist no vacío
- ✅ Número de encuesta requerido
- ✅ Operador ≠ Supervisor
- ✅ Catálogos filtrados por permisos

---

## 4. Catálogos

**Operadores**: Empleados con permiso 157 activos  
**Supervisores**: Empleados con permiso 100 o 135 activos

---

**Documento creado**: 2026-01-15  
**Completitud**: 100%  
**Listo para QA**: ✅ Sí
