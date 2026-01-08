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
- Publicar el portal de tráfico (`/OP/Trafico`) para validar la SP `OP_TraficoEncuestasCiudad` con los trabajos ya documentados y ofrecer un primer flujo 1:1.

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
| Presupuesto interno | SolicitudPresupuestoInterno.aspx, SolicitudPresupuestosInternos.aspx | PresupInt (CoreProject), EnviarCorreo | Pendiente | Crear formulario adaptativo (modo completo/simplificado) y logica de correo. |
| Activacion/Anulacion encuestas | ActivacionEncuestas.aspx, AnulacionEncuestas.aspx | ActivacionEncuestas, AnulacionEncuestas, OP_Cuanti_Model (OP_GestionCampo*) | Pendiente | Unificar vista + servicios que reusan stored procedures de CoreProject. |
| Dashboard y home | HomeGestion.aspx, HomeRecoleccion.aspx | Datos.ClsPermisosUsuarios, WorkFlow | Pendiente | Disenar SPA de dashboards con tabs por rol. |
| Produccion y registro | RegistroProduccionOP.aspx | RegistroProduccion, EAreas, EReproceso, EActividad | Pendiente | Exponer servicios de actividades, subactividades, reprocesos. |
| Supervision campo telefonico | SupervisionCampoTelefonico.aspx | SupervisionCampoTelefonico (CoreProject) | Pendiente | Corregir id hardcoded y migrar checklist/campos. |
| iField configuration | iFieldConfiguration.aspx | DALDAP.iFieldSettings | Pendiente | Validar integracion LDAP y sincronizacion de variables. |

## Fase 2 - Sprints recomendados
La fase 2 la dividimos en sprints consecutivos para no perder de vista cada flujo y garantizar la cobertura total (100%) del módulo.

| Sprint | Duración | Flujos centrales | Objetivo | Criterio de cierre |
|---|---|---|---|---|
| 1 | 1 semana | Portal COE y navegación general | Reproducir la grilla de Trabajos.aspx (permiso 100) con enlaces interflujo y sincronización de sesión | Vista funcional con grid, navegación a `/OP/Trafico` y `/OP/Avances`, pruebas de permisos 100 |
| 2 | 1 semana | Tráfico y Activaciones | Afinar `/OP/Trafico`, agregar accesos a Activation/Anulacion y preparar la base para ClosedXML | KPIs de ciudades, botones de acción, y servicios listos para exportar y validar envíos |
| 3 | 2 semanas | Cargas masivas (ImportarDatos + ImportarPlanillas) | Wizard unificado con validaciones y bulk copy reutilizando `CatiRMC_*` y `OP_CuantiPlanillas` | Flujo de carga con resumen, archivos en blob configurable y registros en `RespuestasCatiRMCtmp`/`OP_CuantiPlanillas` |
| 4 | 2 semanas | Planillas/Planillas Revisadas/Productividad/IPS | Consolida tab sets y grids para planillas, productividad y control IPS, con Dapper, notificaciones y validations comunes | Tabs listos, IPS editable y alertas con ClosedXML, correos listos y POV para coordinador/PMO/MyS/Call |
| 5 | 1 semana | Presupuestos internos + utilidades (RegistroProduccion, iField, Supervisión) | Migrar formularios de presupuestos, registro de producción e iField, corregir supervisión con permiso dinámico | Formularios funcionales, SPs invocados, ID hardcoded reemplazado y dependencias de CoreProject enlazadas |

Cada sprint se documentará aquí y en `ANALISIS_OP_CUANTITATIVO.md` para reflejar bloqueos, decisiones y avances. Los criterios se revisarán en las reuniones de seguimiento y el backlog se actualizará si cambian las prioridades o aparecen dependencias nuevas.

## Sprint actual
- Sprint 1 (Portal COE) en curso: la grilla ya lista permite filtrar por JobBook/Estado y enlazar a `/OP/Trafico` y `/OP/Avances`; se sigue afinando la validación de permisos 100 y las acciones contextuales para navegación a los demás flujos.

> Este es el checklist base; cada entrada se actualizara con notas y tiempos una vez arranquemos la implementacion de la vista/servicio correspondiente. Se debe asegurar que las dependencias de CoreProject (stored procedures y contexts) se consumen sin cambiar nombres, siguiendo la Regla Core 1 de las directrices.

## Validacion y seguimiento
- Planear spiking con OpenXml y posible Blob storage antes de la migracion de cargas masivas (ImportarDatos y Planillas).
- Registrar avances, bloqueos y decisiones en este documento; si se requiere confirmacion se solicitara en espanol (tal como lo solicita la directiva).
- Cada vez que se cierre un flujo o se habilite un sprint, se anadira una nueva entrada con fecha y responsable.

## Decision points abiertos
1. Se mantiene un modelo 1:1 o se aplica la consolidacion de vistas optimizadas para productividades, planillas y encuestas (ver seccion de propuestas en ANALISIS_OP_CUANTITATIVO.md)? 
2. Confirmar si los archivos en ~/Files se deben migrar a Azure Blob o permanecer en los recursos de MatrixNext con un path configurable.
