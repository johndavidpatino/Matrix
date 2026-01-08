# Sprint 5 - Presupuestos internos y utilidades

**Objetivo**: Finalizar los formularios de presupuestos internos, registrar producción y estandarizar iField/supervisión para cerrar la cobertura 100% del módulo OP_Cuantitativo.

## Flujo principal

| Área | Alcance | Dependencias Core | Resultado esperado |
|---|---|---|---|
| Presupuestos internos | Migrar `SolicitudPresupuestoInterno.aspx` y `SolicitudPresupuestosInternos.aspx` manteniendo los modos completo/simplificado, validaciones y envíos de correo; `PresupuestosController` expone `GET /OP/Presupuestos` con ambas opciones. | `PresupInt`, `EnviarCorreo`, `GD` | Solo queda pulir notificaciones y seguimiento en portal: la vista `/OP/Presupuestos` ya guarda solicitudes, valida duplicados y registra observaciones. |
| Registro de producción y iField | Reutilizar `RegistroProduccionOP.aspx`, `RegistroProduccion.aspx` y `iFieldConfiguration.aspx` para exponer servicios de actividades, subactividades e iField. | `RegistroProduccion`, `EAreas`, `EReproceso`, `EActividad`, `DALDAP.iFieldSettings` | Formularios que consuman servicios EF/Dapper, actualicen configuraciones LDAP y registren operaciones de producción. |
| Supervisión de campo telefonico | Reproducir `SupervisionCampoTelefonico.aspx`, corregir id hardcode y aplicar permisos dinámicos. | `SupervisionCampoTelefonico`, `PermisosService` | Checklist actualizable, filtros por rol y validaciones de campo en la vista consolidada del portal OP. |

## Tareas iniciales (Sprint 5)

1. **Presupuestos**: adaptar el servicio `PresupInt` para servir el formulario desde MatrixNext, reutilizar componentes de validación y disparar notificaciones vía `NotificationService`.
2. **Registro de producción / iField**: mapear `RegistroProduccion` y `iFieldConfiguration` a nuevos ViewModels/Controllers, reciclar campos base (activaciones, re-procesos) y revisar la sincronización LDAP.
3. **Supervisión guía**: construir un tab dentro de `/OP/PlanillasAprobacion` o una nueva vista en `/OP/Supervision` con permisos dinámicos y la lista de chequeo de campo telefónico.
4. **Seguimiento**: enlazar este plan con `OP_CUANTITATIVO_AVANCE.md` y la sección de decisiones, actualizando controles conforme avance el sprint.

## Criterio de inicio

- Sprint 5 marcado como en progreso tan pronto se asignen responsables y se documenten los primeros artefactos.
- Documentación y checklist actualizados con nuevas pantallas (presupuestos, registro, iField, supervisión).
- Dependencias de CoreProject validadas y consumidas sin renombrar SP existentes.

*Nota: Se inicia Sprint 5 tras cerrar Sprint 4; el próximo checkpoint será la primera entrega funcional de presupuestos y registro de producción.*
