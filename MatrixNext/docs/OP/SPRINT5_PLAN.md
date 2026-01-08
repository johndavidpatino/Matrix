# Sprint 5 - Presupuestos internos y utilidades

**Objetivo**: Finalizar los formularios de presupuestos internos, registrar produccion y estandarizar iField/supervision para cerrar la cobertura 100% del modulo OP_Cuantitativo.

## Flujo principal

| Area | Alcance | Dependencias Core | Resultado esperado |
|---|---|---|---|
| Presupuestos internos | Migrar SolicitudPresupuestoInterno.aspx y SolicitudPresupuestosInternos.aspx, manteniendo modos completo/simplificado y validaciones; PresupuestosController expone /OP/Presupuestos para ambos flujos. | PresupInt, EnviarCorreo, GD | La vista /OP/Presupuestos guarda solicitudes, evita duplicados y documenta observaciones; falta agregar notificaciones y seguimiento en el portal OP. |
| Registro de produccion e iField | Reutilizar RegistroProduccionOP.aspx, RegistroProduccion.aspx e iFieldConfiguration.aspx con ProduccionController, OpProduccionService y /OP/IField para combos (unidades, actividades, JB) y guardado de registros con OP_Produccion_Add. | RegistroProduccion, EAreas, EReproceso, EActividad, DALDAP.iFieldSettings | Vista /OP/Produccion con formulario completo, listado historico y servicios Dapper para combos; /OP/IField organiza proyectos, configuraciones y pendientes, mientras se integra LDAP/iField y la vista de supervision. |
| Supervisión de campo telefónico | Reproducir `SupervisionCampoTelefonico.aspx` con permisos dinámicos y checklist; `/OP/Supervision` ofrece controles de CRI/COM/ACC y guardado con `OP_SupervisionCampoTelefonico_Add`. | `SupervisionCampoTelefonico`, `PermisosService`, `US_Usuarios` | Formulario con checklist, operadores y supervisores dinámicos, guardado por `OpSupervisionService` y checklist accesible desde el portal. |

## Tareas iniciales (Sprint 5)

1. **Presupuestos**: el servicio PresupInt alimenta el formulario, guarda solicitudes y ahora dispara correos configurables (sección Notifications:Presupuestos) cada vez que se completa o simplifica una solicitud; el portal OP lista las 5 solicitudes más recientes con badges y enlace al flujo para que el equipo las supervise.
2. **Registro de produccion / iField**: ProduccionController, OpProduccionService y /OP/IField ya cargan combos (unidades, actividades, JB), guardan registros y el portal visualiza un resumen global de producción (totales, hoy y última actualización) para vincularlo con las operaciones de iField.
3. **Supervision guia**: /OP/Supervision recapta filtros, operadores y checklist, usa OpSupervisionService para ejecutar OP_SupervisionCampoTelefonico_Add, reemplaza el Session( IdUsuario) hardcodeado por GetCurrentUserId(), exige permiso 157 en cada operación al guardar y alimenta el resumen de alertas que publica el portal.
4. **Portal operativo**: PortalController y OpPortalService consumen el resumen global de supervisión, producción y presupuestos, muestran badges de permisos 100/157 y exponen alertas telefónicas y solicitudes recientes dentro de /OP/Portal, guiando al equipo hacia cada flujo.
5. **Seguimiento**: vincular este plan con OP_CUANTITATIVO_AVANCE.md y la seccion de decisiones, documentando bloqueos y avances.

## Criterio de inicio

- Sprint 5 marcado como en progreso tan pronto se asignen responsables y se documenten los primeros artefactos.
- Documentacion y checklist actualizados con nuevas pantallas (presupuestos, registro, iField, supervision).
- Dependencias de CoreProject validadas y consumidas sin renombrar SP existentes.

*Nota: Se inicia Sprint 5 tras cerrar Sprint 4; proximo checkpoint sera la entrega funcional de presupuestos y del registro de produccion.*

### Estatus actual

- Sprint 5 completado con notificaciones, resumen de produccion y supervisión integrada en el portal.

