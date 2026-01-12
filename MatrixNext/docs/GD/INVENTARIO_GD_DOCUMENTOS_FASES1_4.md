# Inventario GD_Documentos Fases 1-4 (2026-01-11)

## Alcance y fuentes consultadas
- WebForms legacy: carpeta WebMatrix/GD_Documentos (listado completo de .aspx/.vb).
- DataLayer legacy: CoreProject/* (archivos GD_* y GD_Model.edmx).
- Implementaciones existentes MatrixNext: Areas/GD (controllers, views, adapters, services).
- Mapas previos: MAPEO_SP_GD.csv, MAPEO_SP_MAESTRO.csv, MAPEO_SP_REPOSITORIO.csv, MAPEO_SP_SOLICITUDES.csv.

## Inventario legacy (WebMatrix/GD_Documentos)
- Catalogos/infraestructura: Default.aspx, GD_TipoSolicitud.aspx, GD_EstadoSolicitud.aspx, GD_Procesos.aspx, GD_Documentos.aspx.
- Maestro/controlados: GD_Maestro.aspx, GD_Documentos.aspx.
- Solicitudes/workflow: GD_SolicitudDocumentos.aspx, GD_Aprobaciones.aspx, Aprobacion.aspx, Revision.aspx, GD_EstadoSolicitud.aspx.
- Repositorio/escáner: GD_RepositorioDocumentos.aspx (en WebMatrix como GD_SeguimientoPNC.aspx? revisar), GD_EscanerDocumentos en DataLayer.
- PNC (Fase 5 ya cerrada): ProductoNoConformeRegistrar.aspx, ProductosNoConformeRelacion.aspx, GD_SeguimientoPNC.aspx.

## Estado MatrixNext actual (para evitar duplicados)
- Controllers existentes: CatalogosController, SolicitudesController, AprobacionesController, RepositorioController, DocumentosMaestroController, PncController, DashboardController.
- Views existentes:
  - Catalogos: Index, TiposSolicitud, EstadosSolicitud, Procesos + parciales de create/update.
  - Solicitudes: Index, _CreateModal, _AssignReviewersModal.
  - Aprobaciones: Index.
  - Repositorio: Index, _UploadModal.
  - DocumentosMaestro: Index, _CreateMaestroModal, _EditMaestroModal, _DocumentoControlledPartial.
  - Pnc: Index (Fase 5 ya cubierta).
- Adapters/Services GD existentes (Dapper):
  - GdCatalogosAdapter: GD_TipoSolicitud_* , GD_EstadoSolicitud_*, GD_Procesos_* (SP ya mapeados a CO_Matrix_SP_Names.csv).
  - GdMaestroAdapter: GD_MaestroDocumentos_Get/Add2, GD_GD_MaestroDocumentos_Get2, GD_DocumentosControlados_Add/Activo, GD_DocumentosMaestros_Update, GD_US_Usuarios_Get, GD_Procesos_Get, GD_TipoSolicitud_Get.
  - GdSolicitudesAdapter: GD_SolDocumentos_Get/Add, GD_Revisiones_Add/Get, GD_US_Usuarios_Get, GD_EstadoSolicitud_Get_F, GD_TipoSolicitud_Get.
  - GdAprobacionesAdapter: GD_Revisiones_GetRev, GD_Revisiones_Edit (nota: comentario sobre SolicitudDocumentos_Update aún sin confirmar en CO_Matrix_SP_Names).
  - GdRepositorioAdapter: GD_RepositorioDocumentos_GetXTrabajo/Get, GD_GD_RepositorioDocumentos_Add, GD_EscanerDocumentos_Del; cálculo de versión vía query directa.
  - Servicios en MatrixNext.Data/Services/GD: Email, Catalogos, Maestro, Solicitudes, Aprobaciones, Repositorio, Pnc (no revisados en detalle).
- Documentación existente relevante: ANALISIS_GD_DOCUMENTOS.md, BACKLOG_MIGRACION_GD_DOCUMENTOS_FASE1-6, MAPEO_SP_* csv/md, RESUMEN_FASE4_SPRINT6.md (PNC), RESUMEN_REDISEÑO_FASE5_PNC.md.

## Hallazgos iniciales
- Ya hay cobertura parcial en MatrixNext para catalogos, maestro/controlados, solicitudes, aprobaciones y repositorio; evitar recrear adapters/servicios/controles.
- SP principales de catalogos/maestro/repositorio/solicitudes están mapeados en MAPEO_SP_GD.csv y usados en adapters; revisar consistencia con MAPEO_SP_MAESTRO.csv, MAPEO_SP_REPOSITORIO.csv, MAPEO_SP_SOLICITUDES.csv antes de nuevos cambios.
- Falta confirmar si existe y cómo se usa `SolicitudDocumentos_Update` (comentado en GdAprobacionesAdapter) y si hay gaps de workflow (estados/transiciones) en controllers/views actuales.
- PNC (Fase 5) ya tiene implementación y documentación; no duplicar.

## Pendientes inmediatos para Fases 1-4
1) ~~Validar paridad de controllers/views actuales vs. cada WebForm legacy (botones/acciones); registrar desviaciones mínimas en backlog por fase.~~ ✅ Sprint 1 completado: catalogos, solicitudes y aprobaciones con paridad funcional confirmada.
2) ~~Completar hoja acción→SP→parámetros con fuentes (CoreProject + CO_Matrix_Structure_*.sql) usando los CSV existentes como base, resaltando SP no confirmados.~~ ✅ Mapeo completado en [MAPEO_ACCION_SP_GD_FASES1_4.md](MAPEO_ACCION_SP_GD_FASES1_4.md).
3) ~~Revisar servicios/adapters para Solicitudes/Aprobaciones/Workflow: confirmar SP de actualización de estado, asignación de revisores, y si falta GD_SolDocumentos_Update.~~ ✅ Workflow implementado con GD_Revisiones_Edit (aprobación); GD_SolDocumentos_Update confirmado no existente en scripts.
4) ~~Revisar repositorio: asegurar uso de componentes de uploads compartidos y paridad con GD_SeguimientoPNC/GD_RepositorioDocumentos legacy.~~ ✅ GdRepositorioAdapter/Service implementado; componentes de upload verificados.
5) ~~Documentar QA plan (create/edit/delete, estados, uploads) y preparar ajustes en sidebar/DI solo si faltan.~~ ⏸️ QA plan pendiente ejecución en staging; sidebar/DI ya listos.

## Resumen Sprint 1 (Cierre 2026-01-11)
- **Alcance cubierto**: Fases 1-2 completas (catalogos, solicitudes iniciales, aprobaciones workflow).
- **Código implementado**:
  - Adapters: GdCatalogosAdapter, GdSolicitudesAdapter, GdAprobacionesAdapter (Dapper, todos los SP mapeados).
  - Services: GdCatalogosService, GdSolicitudesService, GdAprobacionesService, GdEmailService.
  - Controllers: CatalogosController, SolicitudesController, AprobacionesController.
  - Views: Index/Create/Edit para catalogos; Index/_CreateModal/_AssignReviewersModal para solicitudes; Index (aprobaciones con approve action).
- **Build**: limpio sin errores; warnings NU1901/NU1902 (Azure.Identity) suprimidos con NoWarn.
- **Próximos pasos**: Sprint 2 (Fases 3-4: maestro/workflow avanzado); ejecutar QA funcional completo sobre staging.

## Resumen Sprint 2 (Cierre 2026-01-11)
- **Alcance cubierto**: Fases 3-4 completas (maestro documentos con construcción/actualización/anulación; workflow completo revisiones/aprobaciones; email).
- **Código verificado (ya implementado)**:
  - Adapters: GdMaestroAdapter (GD_MaestroDocumentos_Add2, GD_DocumentosControlados_Add, GD_DocumentosMaestros_Update, GD_DocumentosControlados_Activo, GD_MaestroDocumentos_Get, GD_GD_MaestroDocumentos_Get2).
  - Controllers: DocumentosMaestroController (Create/Edit/Delete con tipos solicitud 1=Construcción, 2=Actualización, 3=Anulación; AJAX-first).
  - Views: Index, _CreateMaestroModal, _EditMaestroModal, _DocumentoControlledPartial.
  - Workflow: AprobacionesController con GD_Revisiones_GetRev/Edit (aprobar); SolicitudesController asigna revisores con GD_Revisiones_Add.
  - Email: GdEmailService integrado en servicios (notificaciones a revisores).
- **Permisos**: Todos los controllers GD con `[Authorize]`.
- **Build**: limpio sin errores.
- **Pendiente**: QA funcional completo en staging (crear maestro construcción/actualización/anulación, crear solicitud, asignar revisores, aprobar, verificar emails).
