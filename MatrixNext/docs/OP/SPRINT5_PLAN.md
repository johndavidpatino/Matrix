# Sprint 5 - Presupuestos internos y utilidades

**Objetivo**: Finalizar los formularios de presupuestos internos, registrar producción y estandarizar iField/supervisión para cerrar la cobertura 100% del módulo OP_Cuantitativo.

## Flujo principal

| Área | Alcance | Dependencias Core | Resultado esperado |
|---|---|---|---|
| Presupuestos internos | Migrar SolicitudPresupuestoInterno.aspx y SolicitudPresupuestosInternos.aspx, manteniendo modos completo/simplificado y validaciones; PresupuestosController expone /OP/Presupuestos para ambos flujos. | PresupInt, EnviarCorreo, GD | La vista /OP/Presupuestos guarda solicitudes, evita duplicados y documenta observaciones; falta añadir notificaciones y el seguimiento en el tablero OP. |
| Registro de producción e iField | Reutilizar RegistroProduccionOP.aspx, RegistroProduccion.aspx e iFieldConfiguration.aspx mediante ProduccionController + OpProduccionService para combos (unidades, actividades, JB) y guardado de registros con OP_Produccion_Add. | RegistroProduccion, EAreas, EReproceso, EActividad, DALDAP.iFieldSettings | Vista /OP/Produccion con formulario completo, listado histórico y servicios Dapper para combos; pendiente la integración iField/LDAP y la vista de supervisión. |
| Supervisión de campo telefónico | Reproducir SupervisionCampoTelefonico.aspx, corregir id hardcode y aplicar permisos dinámicos. | SupervisionCampoTelefonico, PermisosService | Checklist actualizable con filtros por rol y validaciones de campo en la vista consolidada del portal OP. |

## Tareas iniciales (Sprint 5)

1. **Presupuestos**: el servicio PresupInt ya alimenta el formulario; confirmar envíos de correo y anexar notificaciones al portal.
2. **Registro de producción / iField**: ProduccionController y OpProduccionService ya cargan combos (unidades, actividades, JB) y guardan registros; se avanza en los mapeos iField/LDAP y el checklist de supervisión.
3. **Supervisión guía**: preparar un tab o vista en /OP/Supervision que recoja filtros por rol, el checklist telefónico y referencias al tablero actual de planillas.
4. **Seguimiento**: enlazar este plan con OP_CUANTITATIVO_AVANCE.md y la sección de decisiones, documentando bloqueos y avances en cada punto.

## Criterio de inicio

- Sprint 5 marcado como en progreso tan pronto se asignen responsables y se documenten los primeros artefactos.
- Documentación y checklist actualizados con nuevas pantallas (presupuestos, registro, iField, supervisión).
- Dependencias de CoreProject validadas y consumidas sin renombrar SP existentes.

*Nota: Se inicia Sprint 5 tras cerrar Sprint 4; el próximo checkpoint será la primera entrega funcional de presupuestos y del registro de producción.*
