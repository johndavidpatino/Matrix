# Sprint 5 - Presupuestos internos y utilidades

**Objetivo**: Finalizar los formularios de presupuestos internos, registrar produccion y estandarizar iField/supervision para cerrar la cobertura 100% del modulo OP_Cuantitativo.

## Flujo principal

| Area | Alcance | Dependencias Core | Resultado esperado |
|---|---|---|---|
| Presupuestos internos | Migrar SolicitudPresupuestoInterno.aspx y SolicitudPresupuestosInternos.aspx, manteniendo modos completo/simplificado y validaciones; PresupuestosController expone /OP/Presupuestos para ambos flujos. | PresupInt, EnviarCorreo, GD | La vista /OP/Presupuestos guarda solicitudes, evita duplicados y documenta observaciones; falta agregar notificaciones y seguimiento en el portal OP. |
| Registro de produccion e iField | Reutilizar RegistroProduccionOP.aspx, RegistroProduccion.aspx e iFieldConfiguration.aspx con ProduccionController, OpProduccionService y /OP/IField para combos (unidades, actividades, JB) y guardado de registros con OP_Produccion_Add. | RegistroProduccion, EAreas, EReproceso, EActividad, DALDAP.iFieldSettings | Vista /OP/Produccion con formulario completo, listado historico y servicios Dapper para combos; /OP/IField organiza proyectos, configuraciones y pendientes, mientras se integra LDAP/iField y la vista de supervision. |
| Supervision de campo telefonico | Reproducir SupervisionCampoTelefonico.aspx, corregir id hardcode y aplicar permisos dinamicos. | SupervisionCampoTelefonico, PermisosService | Checklist actualizable con filtros por rol y validaciones de campo en el portal OP. |

## Tareas iniciales (Sprint 5)

1. **Presupuestos**: el servicio PresupInt alimenta el formulario y guarda solicitudes; confirmar envios de correo y anexar notificaciones en el portal.
2. **Registro de produccion / iField**: ProduccionController, OpProduccionService y /OP/IField ya cargan combos (unidades, actividades, JB) y guardan registros; avanzar en los mapeos iField/LDAP y el checklist de supervision telefonica.
3. **Supervision guia**: preparar un tab o vista en /OP/Supervision que recoja filtros por rol, el checklist y referencias al tablero de planillas.
4. **Seguimiento**: vincular este plan con OP_CUANTITATIVO_AVANCE.md y la seccion de decisiones, documentando bloqueos y avances.

## Criterio de inicio

- Sprint 5 marcado como en progreso tan pronto se asignen responsables y se documenten los primeros artefactos.
- Documentacion y checklist actualizados con nuevas pantallas (presupuestos, registro, iField, supervision).
- Dependencias de CoreProject validadas y consumidas sin renombrar SP existentes.

*Nota: Se inicia Sprint 5 tras cerrar Sprint 4; proximo checkpoint sera la entrega funcional de presupuestos y del registro de produccion.*
