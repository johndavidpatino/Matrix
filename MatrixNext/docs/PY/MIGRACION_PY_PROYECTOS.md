# Migración PY_Proyectos – Inventario y Plan (versión detallada)

**Propósito:** entregar a desarrollo un plan completo para migrar PY_Proyectos (WebMatrix → MatrixNext, ASP.NET Core 8) con paridad funcional, siguiendo DIRECTRICES_MIGRACION.md y manteniendo sincronía con DASHBOARD_MIGRACION y MODULOS_MIGRACION.

**Estado:** 🔍 Análisis ampliado, listo para ejecución por fases (alinear con FI/CC precedentes).

## Alcance, dependencias y exclusiones
- Cobertura: 18 páginas WebForms en WebMatrix/PY_Proyectos (cuanti + cuali).
- Dependencias críticas: US_Usuarios (roles/aprobadores), CU_Cuentas (JobBook/Brief/Estudios), CORE (workflow/tareas), catálogos PY/PY_Cuali, reportes PY, componente de carga de archivos (derivado de CU_Cuentas/Frame.aspx).
- Datos: contextos PY_Model y PY_Cuali en CoreProject (edmx + SP). Mantener nombres exactos de tablas/SP (Regla 1 y 4).
- Exclusiones: no agregar features nuevas; solo paridad. Cualquier nueva validación debe existir en legacy o ser aprobada.

## Inventario de vistas WebMatrix/PY_Proyectos
| Página (.aspx) | Funcionalidad principal | Acciones clave | Notas de migración |
| --- | --- | --- | --- |
| Default / NewDefault | Landing y acceso rápido a proyectos | Búsqueda, filtros, navegación a detalle | Consolidar en una vista Index Razor con filtros server-side + paginación. |
| Home | Dashboard módulo | Tarjetas/resúmenes, accesos directos | Reutilizar layout de Home área PY; métricas vía servicios. |
| PY_Proyectos | Maestro de proyectos | Alta/edición de proyecto, asignación de estado/tipo, vínculos a trabajos | Usa catálogos de tipos de proyecto y estados; validar SP de creación/edición. |
| Trabajos | Gestión trabajos cuantitativos | CRUD trabajos, vínculo a trabajos de campo, export | Requiere SP de trabajos y catálogos metodológicos. |
| TrabajosCualitativos | Gestión trabajos cualitativos | CRUD trabajos cuali, asignar moderadores/reclutadores | Depende de PY_Cuali.* entidades. |
| AsignacionProyectos | Asignar responsables a proyectos | Alta/edición de asignaciones, búsqueda por proyecto | Validar roles y permisos. |
| AsignacionesProyectos | Listado de asignaciones | Filtros por responsable, export | Reutilizar grid compartido. |
| REAsignacionProyectos | Reasignaciones | Cambiar responsables, auditoría | Garantizar trazabilidad y notificaciones. |
| DistribucionEntrevistas | Planeación entrevistas | Distribuir entrevistas por unidad/ciudad/metodología | Requiere SP de distribución y catálogos. |
| SegmentosCuali | Segmentación cualitativa | Alta/edición segmentos, cuotas | Depende de PY_SegmentosCuali.* entidades. |
| Sesiones | Programación de sesiones | CRUD sesiones cuali, horarios y recursos | Validar campos de lugar/fecha/estado. |
| InHomeVisit | Gestión visitas en casa | Programar visitas, seguimiento | Revisar SP InHome y bitácoras. |
| VariablesControl | Variables de control | Alta/edición de variables y rangos | Afecta reportes PY_Variables_Control*. |
| RegistroPlanillasCualitativo | Planillas de campo | Carga/registro de planillas, validaciones | Considerar carga de archivos si aplica. |
| InstructivoGeneral | Instructivos cuantitativos | Ver/descargar instructivos, quizá carga | Reutilizar componente de carga/descarga. |
| InstructivoGeneralCuali | Instructivos cualitativos | Similar al anterior para cuali | Compartir vistas parciales. |
| DuplicarTrabajos | Duplicación de trabajos | Clonar configuración/trabajo base | Asegurar copia de parámetros y catálogos. |
| Lista auxiliar: Trabajos (maestro) | Gestión general de trabajos | CRUD base, vínculo con tareas CORE | Mantener estados alineados con CORE. |

## Mapa de migración (fases sugeridas)
- **Fase 0 – Base técnica (1-2 días):** crear área PY y registrar DI; armar adapters/services/viewmodels base; publicar componente de carga de archivos compartido (ver abajo); definir rutas y layout.
- **Fase 1 – Listados y maestro (Default/Home, PY_Proyectos, Trabajos):** filtros+paginación, creación/edición de proyectos y trabajos; catálogos de tipos/estados/metodologías; respetar SP de creación/edición cuando exista lógica.
- **Fase 2 – Asignaciones y distribución:** AsignacionProyectos, AsignacionesProyectos, REAsignacionProyectos, DistribucionEntrevistas; permisos de rol, trazabilidad de reasignaciones, notificaciones si existen en legacy.
- **Fase 3 – Cuali:** TrabajosCualitativos, SegmentosCuali, Sesiones, InHomeVisit; cuotas por segmento, sesiones y visitas; validar catálogos PY_Cuali.*.
- **Fase 4 – Soporte y documentación:** InstructivoGeneral, InstructivoGeneralCuali, RegistroPlanillasCualitativo, VariablesControl, DuplicarTrabajos; cargas/descargas, duplicación de trabajos, variables de control ligadas a reportes.
- **Cierre:** checklist de paridad, pruebas funcionales, y actualización de [DASHBOARD_MIGRACION.md](../DASHBOARD_MIGRACION.md) y [MODULOS_MIGRACION.md](../MODULOS_MIGRACION.md) al finalizar cada fase.

## Capa de datos (CoreProject) a migrar
- **Contextos:** PY_Model (cuantitativo) y PY_Cuali (cualitativo) desde CoreProject/*.edmx.
- **Entidades clave (ejemplos):** PY_Proyectos, PY_Trabajo*, PY_Especificaciones*, PY_Variables_Control, PY_SegmentosCuali*, PY_Sesiones*, PY_InHomeVisit*, PY_TiposProyectos_Get_Result, PY_Trabajos_GET_All_Result, PY_TrabajosCuali_*.
- **Stored Procedures/SP wrappers:** validar todos los tipos *_Get_Result y clases asociadas; mapear acciones CRUD vs consultas; registrar nombres exactos (Regla 1 y 4).
- **Estrategia acceso datos:**
  - EF Core para inserts/updates simples (creación/edición de proyectos, trabajos, segmentos).
  - Dapper para SP complejas (distribución entrevistas, duplicar trabajos, reportes, cuotas).
- **Tareas iniciales:**
  - Generar scaffolding de modelos en MatrixNext.Data (namespace MatrixNext.Data.PY) respetando nombres.
  - Crear adapters por tema: ProyectoDataAdapter, TrabajoDataAdapter, CualiDataAdapter, AsignacionDataAdapter, DistribucionDataAdapter, VariablesControlDataAdapter.
  - Servicios: ProyectoService, TrabajoService, CualiService, AsignacionService, DistribucionService, VariablesControlService.

### Inventario mínimo de SP/clases a validar (prioridad alta)
- Creación/edición: PY_Proyectos (crea/edita proyecto), PY_Trabajo* (cuanti), PY_TrabajoCuali*, PY_Especificaciones*, PY_Variables_Control.
- Listados/reportes: PY_Proyectos_Get_Result, PY_Trabajos_GET_All_Result, PY_Trabajos_Get_Result, PY_Trabajos_Get_WithoutMetodCampo_Estadistica_Result, PY_SegmentosCuali_Get_Result, PY_Trabajo_Entrega_* (entregables).
- Distribución y asignaciones: PY_TrabajosxProyectosxCoordinador_Result, PY_TrabajosxProyectosxGerente_Result, PY_InfoTrabajoCreacion_Result, PY_CoordinadorProyecto_Cuali_Result, PY_GerenteProyecto_Cuali_Result.
- Duplicación y soporte: revisar SP usados en DuplicarTrabajos.aspx y RegistroPlanillasCualitativo.aspx (buscar en code-behind), más InstructivoGeneral*.
- Catálogos: PY_TiposProyectos_Get_Result, PY_Tipos_Procesos_Get_Result, metodologías y segmentos (PY_SegmentosCuali_*, PY_TipoReclutamientoCuali).

## Integración y arquitectura objetivo en MatrixNext
- **Área PY (MatrixNext.Web/Areas/PY):** controllers Proyectos, Trabajos, TrabajosCuali, Asignaciones, Distribucion, SegmentosCuali, Sesiones, InHomeVisit, VariablesControl, Instructivos.
- **Views:** Razor por página; modales para CRUD (Regla 5); partials compartidos (grid, datepicker, select user, upload).
- **DI:** registrar servicios/adapters en Program.cs vía AddPYModule.
- **Routing:** {area:exists}/{controller=Proyectos}/{action=Index}/{id?}.
- **Autorización:** [Authorize] y roles por acción; validar permisos de asignación y descarga de archivos.

## Arquitectura objetivo en MatrixNext
- **Área PY (MatrixNext.Web/Areas/PY):**
  - Controllers: ProyectosController, TrabajosController, TrabajosCualiController, AsignacionesController, DistribucionController, SegmentosCualiController, SesionesController, InHomeVisitController, VariablesControlController, InstructivosController.
  - Views: Razor por página original, usar modales para CRUD (Regla 5), partials reutilizables (grid, datepicker, select user, upload).
  - Autorización: [Authorize] y roles específicos si aplica (Regla 11).
- **DI:** registrar servicios/adapters en Program.cs siguiendo patrón AddPYModule.
- **Routing:** usar pattern {area:exists}/{controller=Proyectos}/{action=Index}/{id?}.

## Componente reutilizable de carga de archivos
- Origen: lógica de carga en WebMatrix/CU_Cuentas/Frame.aspx.
- Objetivo: crear parcial Razor y helper JS compartido (Views/Shared/_UploadFrame.cshtml) con soporte para:
  - Llaves: IdTrabajo/IdProyecto, tipoDocumento, nombre visible, peso máximo.
  - Persistencia: reutilizar SP/tabla existente; exponer en servicios adaptadores.
  - Validaciones: tamaño/extensiones, manejo de duplicados, logs.
- Reutilización: InstructivoGeneral, InstructivoGeneralCuali, RegistroPlanillasCualitativo, adjuntos de proyectos.

### Consideraciones de implementación
- Backend: endpoint único (ej. UploadController) que recibe IdTrabajo/IdProyecto + tipoDocumento; guardar metadata y archivo; log de usuario/fecha.
- Frontend: partial con input file + hidden keys; callbacks JS para refrescar grillas de adjuntos; mensajes de éxito/error consistentes.
- Seguridad: validar permisos antes de aceptar carga/descarga; tamaño máx configurable en appsettings.
- Pruebas: cargar, reemplazar, descargar, eliminar; validar extensiones y duplicados.

## Checklist de paridad (usar en cierre de cada fase)
- [ ] Cada vista WebForms tiene su Razor equivalente con mismas acciones y controles.
- [ ] SP legacy identificados, parámetros iguales y ejecutados desde adapters.
- [ ] Catálogos cargan desde CoreProject sin renombrar campos.
- [ ] Modales de creación/edición y confirmaciones de eliminación funcionan.
- [ ] Filtros y paginación replican comportamiento (incluye búsqueda server-side).
- [ ] Exportaciones/descargas operativas (instructivos, planillas, reportes).
- [ ] DuplicarTrabajos preserva claves y relaciones.
- [ ] Roles/permisos aplicados según US_Usuarios.
- [ ] Logging y manejo de errores homogéneo.
- [ ] Documentación y dashboard actualizados tras cada fase.

## Pruebas mínimas por vista
- Acceso autorizado, carga de catálogos.
- Crear/editar/eliminar/duplicar (según aplique) con validaciones de fechas y estados.
- Filtros y paginación.
- Exportar/cargar archivos.
- Integración con CORE (tareas asociadas) cuando aplique.

## Riesgos y mitigación (rápido)
- **SP no identificados en code-behind:** mapear todos los CommandText en los .vb antes de iniciar; usar Regla 4.
- **Catálogos incompletos:** validar listas en PY_Model/PY_Cuali antes de exponer vistas; fallback a combos de US/CU si aplica.
- **Duplicación de trabajos:** asegurar transacción y copia de claves/relaciones; prever rollback si falla parte del clon.
- **Archivos:** tamaño/extensiones y permisos; registrar descargas (auditoría ligera).
- **Performance en listados/reportes:** usar Dapper + paginación server-side; evitar cargas completas.

## Entregables por fase
- Código: controllers, services, adapters, viewmodels, views Razor, partials compartidos.
- Config: registro de DI y rutas; appsettings para límites de carga.
- Docs: actualizar este archivo, DASHBOARD_MIGRACION y MODULOS_MIGRACION al cerrar cada fase.
- QA: checklist de paridad firmado; evidencias de pruebas (capturas o lista de casos ejecutados).
