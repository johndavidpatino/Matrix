# Registro de Avances - Migracion OP_Cuantitativo

**Ultima actualizacion**: 2026-01-07

## Referencias inmediatas
- MatrixNext/docs/ANALISIS_OP_CUANTITATIVO.md (analisis 100% del modulo y backlog inicial).
- MatrixNext/DIRECTRICES_MIGRACION.md (reglas de nomenclatura, SP y EF Core).
- WebMatrix/OP_Cuantitativo (WebForms actuales a migrar).
- CoreProject/Clases/OP_Cuanti (servicios Dapper/EF y helpers de produccion, IPS, activaciones, trafico).
- CoreProject/OP_Cuanti_Model.edmx + OP_Cuanti2_Model (mapeo de entidades y stored procedures usados).

## Objetivo de este registro
Mantener un listado unico de todos los avances del modulo OP_Cuantitativo, apuntando a:

1. Cumplir la cobertura 100% de los WebForms (salvo los excluidos) y sus flujos derivados.
2. Documentar cada fase de configuracion, migracion y validacion con estatus y proximas acciones.
3. Asociar cada flujo a los servicios de datos en CoreProject para respetar las directrices.

## Fase 0 - Diagnostico (completado)
- Validacion del analisis tecnico y directorios del modulo (documentado en MatrixNext/docs/ANALISIS_OP_CUANTITATIVO.md).
- Reglas base confirmadas en DIRECTRICES_MIGRACION.md (nombres de SP, reutilizacion de CoreProject y uso de EF para inserciones simples).
- Inventario inicial de WebForms y dependencias (trabajos, trafico, planillas, IPS, productividad, presupuestos, encuestas, dashboard, supervisiones).

## Fase 1 - Configuracion base (completado)
- Crear infraestructura shared en MatrixNext (controladores, servicios, middlewares de autorizacion y notificaciones).
- Configurar appsettings.json con *MatrixConnectionString*, *GestionCampoConnectionString*, rutas de archivos y limites de carga/size.
- Asegurar que la capa de datos de CoreProject (OP_Cuanti, OP_Cuanti2, OP_CuantiDapper, Revisiones IPS, TrabajoOPCuanti, PlaneacionProduccion, RegistroProduccion) esta lista para ser consumida por MatrixNext.
- Identificar las stored procedures catalogadas en el analisis (CatiRMC_*, OP_CuantiProduccion*, OP_IPS*, OP_FichaCuantitativo_*, OP_Planeacion_*, etc.) y planear adaptadores Dapper.
- Poner un tablero vivo en MatrixNext.Web (`/OP/Avances`) y un Portal COE (`/OP/Portal`) que muestran el checklist junto a la lista de trabajos y las directrices de CoreProject.
- Publicar el portal de trÃ¡fico (`/OP/Trafico`) para validar la SP `OP_TraficoEncuestasCiudad` con los trabajos ya documentados y ofrecer un primer flujo 1:1.

## Fase 2 - Migracion de flujos (por iniciar)
| Flujo | WebForms actuales | CoreProject clave | Estado | Proxima accion |
|---|---|---|---|---|
| Portal COE y navegacion general | Trabajos.aspx, TrabajosCoordinador.aspx, TrabajosCallCenter.aspx, ConsultaTrabajos.aspx | TrabajoOPCuanti, PlaneacionProduccion, CoordinacionCampoPersonal, GD, EnviarCorreo | Pendiente | Definir controladores/Servicios, crear vistas Razor/Blazor de lista de trabajos y navegacion. |
| Trafico de encuestas | TraficoEncuestas.aspx | TraficoEncuestas, CoordinacionCampo, OP_CuantiDapper (envio/recepcion) | En progreso | Nueva vista `/OP/Trafico` con filtros y KPIs basada en `OP_TraficoEncuestasCiudad`. |
| Importacion de datos CATI | ImportarDatos.aspx | CatiRMC_* SP, OP_Cuanti Modelo, ExcelValidationService (OpenXml) | Pendiente | Disenar wizard de carga con validaciones y adaptacion a Blob storage. |
| Importacion de planillas productividad | ImportarPlanillas.aspx | OP_CuantiDapper (planillas), OP_Cuanti.Modelo, _Festivos | Pendiente | Crear helper de ventana nomina (corte 16-15 + feriados) y servicio de carga con SqlBulkCopy. |
| Planillas y revisiones COE | PlanillasCargadas.aspx, RevisionPlanillas.aspx, PlanillasRevisadas.aspx | OP_CuantiDapper (CuantiPlanillas_GET/Update/Remove) | Pendiente | Consolidar vista con tabs y adaptador Dapper para aprobaciones. |
| Productividad por rol | ProductividadRevisada*.aspx, RevisionProductividad*.aspx | OP_CuantiDapper (CuantiProduccion*) | Pendiente | Implementar dashboards role-based y servicios de aprobacion (Coordinador, PMO, Campo, MyS/Call). |
| IPS y control de observaciones | IPS.aspx | RevisionIPS, EjecucionIPS, IPSClass | Pendiente | Migrar grid editable, validaciones por tipo de tarea y notificaciones (ClosedXML para export). |
| Presupuesto interno | SolicitudPresupuestoInterno.aspx, SolicitudPresupuestosInternos.aspx | PresupInt (CoreProject), EnviarCorreo | En progreso | /OP/Presupuestos guarda los modos completo/simplificado, dispara correos configurables (sección Notifications:Presupuestos:Recipients) y el portal OP lista las 5 solicitudes más recientes con badges; se mantiene la trazabilidad junto al checklist telefónico. |
| Activacion/Anulacion encuestas | ActivacionEncuestas.aspx, AnulacionEncuestas.aspx | ActivacionEncuestas, AnulacionEncuestas, OP_Cuanti_Model (OP_GestionCampo*) | Pendiente | Unificar vista + servicios que reusan stored procedures de CoreProject. |
| Dashboard y home | HomeGestion.aspx, HomeRecoleccion.aspx | Datos.ClsPermisosUsuarios, WorkFlow | Pendiente | Disenar SPA de dashboards con tabs por rol. |
| Produccion y registro | RegistroProduccionOP.aspx | RegistroProduccion, EAreas, EReproceso, EActividad | Pendiente | Exponer servicios de actividades, subactividades, reprocesos. |
| Supervision campo telefonico | SupervisionCampoTelefonico.aspx | SupervisionCampoTelefonico (CoreProject) | En progreso | Reemplazamos el ID hardcodeado (Session `1047223102`) por el usuario autenticado que valida permiso 157, y ahora el portal OP expone el resumen/alertas de supervisiÃ³n; sigue documentar los casos de alerta y cerrar feedback del equipo. |
| iField configuration | iFieldConfiguration.aspx | DALDAP.iFieldSettings | Completado | `/OP/IField` ofrece seleccion de proyectos, configuraciones y pendientes, incluye el botÃ³n â€œSincronizar proyectos iFieldâ€ y documenta la sincronizacion LDAP; solo resta la alerta automÃ¡tica por cambios. |

## Fase 2 - Sprints recomendados
La fase 2 la dividimos en sprints consecutivos para no perder de vista cada flujo y garantizar la cobertura total (100%) del mÃ³dulo.

| Sprint | DuraciÃ³n | Flujos centrales | Objetivo | Criterio de cierre |
|---|---|---|---|---|
| 1 | 1 semana | Portal COE y navegaciÃ³n general | Reproducir la grilla de Trabajos.aspx (permiso 100) con enlaces interflujo y sincronizaciÃ³n de sesiÃ³n | Vista funcional con grid, navegaciÃ³n a `/OP/Trafico` y `/OP/Avances`, pruebas de permisos 100 |
| 2 | 1 semana | TrÃ¡fico y Activaciones | Afinar `/OP/Trafico`, agregar accesos a Activation/Anulacion y preparar la base para ClosedXML | KPIs de ciudades, botones de acciÃ³n, y servicios listos para exportar y validar envÃ­os |
| 3 | 2 semanas | Cargas masivas (CATI + planillas) | Wizard `/OP/ImportacionMasiva` con validaciÃ³n y ejecuciÃ³n (`OpCargaService`); ahora ejecuta los SP `CatiRMC_*` y el bulk copy a `OP_CuantiPlanillas` | Validaciones y ejecuciÃ³n DB completadas, con reportes (`ResumenValidas/NoValidas/Duplicadas/Inconsistencias`) y copia de auditorÃ­a en uploads/op/cargas |
| 4 | 2 semanas | Planillas/Planillas Revisadas/Productividad/IPS | Consolida tab sets y grids para planillas, productividad y control IPS, con Dapper, notificaciones y validations comunes | Tabs listos, IPS editable y alertas con ClosedXML, correos listos y POV para coordinador/PMO/MyS/Call |
| 5 | 1 semana | Presupuestos internos + utilidades (RegistroProduccion, iField, SupervisiÃ³n) | Migrar formularios de presupuestos, registro de producciÃ³n e iField, corregir supervisiÃ³n con permiso dinÃ¡mico | Formularios funcionales, SPs invocados, ID hardcoded reemplazado y dependencias de CoreProject enlazadas |

Cada sprint se documentarÃ¡ aquÃ­ y en `ANALISIS_OP_CUANTITATIVO.md` para reflejar bloqueos, decisiones y avances. Los criterios se revisarÃ¡n en las reuniones de seguimiento y el backlog se actualizarÃ¡ si cambian las prioridades o aparecen dependencias nuevas.

## Sprint actual
- Sprint 1 (Portal COE) completado: la grilla filtra por JobBook/Estado, muestra el estado del permiso 100, enlaza a `/OP/Trafico`, `/OP/Avances` y `/OP/Encuestas`, y exhibe el badge del permiso 100; las acciones contextuales estÃ¡n listas.
- Sprint 2 (TrÃ¡fico + Activaciones) completado: `/OP/Trafico` mantiene KPIs y agregamos `/OP/Encuestas`, una vista con formularios que ejecutan `OP_GestionCampo_ActivarEncuesta` y `OP_GestionCampo_AnularEncuesta` con formularios anti-forgery y mensajes de resultado, cerrando el sprint 2 con data real sobre activaciones/anulaciones.
- Sprint 3 (Cargas masivas) completado: el wizard `/OP/ImportacionMasiva` valida, ejecuta los SP `CatiRMC_*` y dispara el bulk copy a `OP_CuantiPlanillas`, reporta mÃ©tricas (`ResumenValidas/NoValidas/Duplicadas/Inconsistencias`) y guarda el backup en uploads/op/cargas, cerrando el sprint con trazabilidad completa.
- El portal `/OP/Portal` ahora consume el resumen global de supervisión (total, hoy, alertas) y comunica si el permiso 157 está habilitado para dar seguimiento a los checklist telefónicos.
- Sprint 4 (Planillas/Productividad/IPS) completado: `/OP/PlanillasAprobacion` consolida las planillas cargadas/revisadas/aprobadas con indicadores rol-based y control IPS, el endpoint `/OP/Productividad` redirige a la vista unificada y los exports IPS quedan guardados en `~/Files/ips-export-*.xlsx` antes de descargarse, cumpliendo todos los criterios de este sprint.
- Sprint 5 (Presupuestos internos + utilidades) completado: los flujos de presupuestos guardan solicitudes completas y simplificadas, disparan notificaciones configurables, y el portal OP ya despliega el resumen de producción más reciente junto con las alertas y solicitudes telefónicas.
- Sprint 5 (Presupuestos internos + utilidades) completado: la documentación en docs/OP/SPRINT5_PLAN.md detalla las alertas de presupuestos, los resúmenes de producción y la integración con iField/supervisión, para cerrar la trazabilidad del módulo.

> Este es el checklist base; cada entrada se actualizara con notas y tiempos una vez arranquemos la implementacion de la vista/servicio correspondiente. Se debe asegurar que las dependencias de CoreProject (stored procedures y contexts) se consumen sin cambiar nombres, siguiendo la Regla Core 1 de las directrices.

## Validacion y seguimiento
- Configurar Notifications:Presupuestos:Recipients para recibir correos tras cada guardado y mostrar en /OP/Portal las 5 solicitudes de presupuesto recientes junto al resumen de producción (totales, hoy y última actualización) y las alertas telefónicas.
- Planear spiking con OpenXml y posible Blob storage antes de la migracion de cargas masivas (ImportarDatos y Planillas).
- Registrar cada ejecuciÃ³n del wizard `/OP/ImportacionMasiva`: la interfaz ahora muestra el tracking de SP `CatiRMC_*` (vÃ¡lidas/no vÃ¡lidas/duplicadas/inconsistencias) y el backup en `uploads/op/cargas`, conectando resultados con los criterios del sprint 3.
- Definir las stories de Sprint 4, incluyendo la vista `PlanillasAprobacion`, el grid comÃºn para productividad (renderizado por rol) y el mÃ³dulo IPS editable, para poder medir el avance sobre esa fase.
- Registrar avances, bloqueos y decisiones en este documento; si se requiere confirmacion se solicitara en espanol (tal como lo solicita la directiva).
- Cada vez que se cierre un flujo o se habilite un sprint, se anadira una nueva entrada con fecha y responsable.
- Documentar la carpeta `~/Files` (expuesta como `/Files`) como el repositorio oficial de exportes IPS generados por el sprint 4, incluyendo nombre, fecha y enlace generado para mantener trazabilidad en los registros.

## Decision points abiertos
1. Se mantiene un modelo 1:1 o se aplica la consolidacion de vistas optimizadas para productividades, planillas y encuestas (ver seccion de propuestas en ANALISIS_OP_CUANTITATIVO.md)? 
2. Confirmar si los archivos en ~/Files se deben migrar a Azure Blob o permanecer en los recursos de MatrixNext con un path configurable.
