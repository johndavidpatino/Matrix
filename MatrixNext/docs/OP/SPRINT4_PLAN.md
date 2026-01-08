# Sprint 4 - Planillas, Productividad e IPS

**Objetivo**: Completar los flujos de planillas (cargadas, revisión y aprobaciones), consolidar las vistas de productividad role-based y desplegar el control IPS editable/exportable unidos en la aplicación MatrixNext.Web, cumpliendo la cobertura 100% del módulo.

## Flujo principal

| Área | Alcance | Dependencias Core | Resultado esperado |
|---|---|---|---|
| Planillas cargadas/revisión/aprobación | Consolidar las 3 WebForms legacy (PlanillasCargadas.aspx, RevisionPlanillas.aspx, PlanillasRevisadas.aspx) en una única vista tabulada `PlanillasAprobacion/Index.cshtml` con estado `Pendiente`, `En revisión`, `Aprobadas` y acciones contextuales (rechazo/aceptación). | `OP_CuantiDapper` (`CuantiPlanillas_*`), `GD`, `EnviarCorreo` | Tab sets con grid reutilizable, operaciones de rechazo/aceptación via Dapper, notificaciones y validación de permisos 100/135; acciones invocan `OP_CuantiPlanillas_Update/Remove`. |
| Productividad por rol | Migrar vistas repetidas `ProductividadRevisada*` y `RevisionProductividad*` a un solo componente con parámetros de rol (PMO, Coordinador, Campo, MyS/Call). | `OP_CuantiDapper` (`CuantiProduccion*`), `TrabajoOPCuanti`, `PermisosService` | Grid role-based con columnas dinámicas, filtros por trabajo/corte 16-15, y acciones de aprobación/rechazo. |
| IPS y observaciones | Reproducir IPS.aspx grid editable con campos condicionales, exportes ClosedXML y notificaciones email. | `RevisionIPS`, `EjecucionIPS`, `ClosedXML`, `EnviarCorreo` | Vista con editor inline, guardado por tarea y exportación de Observaciones; alertas/queues y evidencia en doc. |

## Tareas clave (Sprint actual)

1. **Planillas**: escribir los endpoints `Aprobar`/`Rechazar` en `PlanillasAprobacionController`, reutilizar `OpGridComponent` y mirar `OP_CuantiPlanillas_Update/Remove` para cerrar el ciclo de aprobación.
2. **Productividad**: implementar `ProductividadController` y `RevisionProductividadController` con parámetros `rol`, `corte`, `trabajoId`; centralizar lógica de validación (corte 16-15, límites por rol) y usar DataGrid shared.
3. **IPS**: construir `IpsController` + view model, usar `RevisionIPS` y `EjecucionIPS` para persistir observaciones, preparar exportación ClosedXML y notificaciones en `NotificationService`.
4. **Permisos y notificaciones**: extender middleware para requerir códigos 100/135/157 en la misma vista (directiva Regla Core 1) y reutilizar `NotificationService`/`EmailService` para confirmaciones.
5. **Documentación y seguimiento**: registrar cada flujo en `SPRINT4_PLAN.md`, enlazar con `ANALISIS_OP_CUANTITATIVO.md`/`OP_CUANTITATIVO_AVANCE.md` y subir evidencia (capturas, logs) a `/OP/Avances`.

## Criterio de cierre

- Tab de planillas con acciones + notificaciones + datos de `OP_CuantiDapper`.
- Dashboard de productividad role-based con validaciones definidas.
- IPS editable con export y notificaciones, con pruebas manuales de cada acción.
- Documentación actualizada y checklist de sprint 4 en los documentos centrales.
