# Mapeo acción→SP→parámetros — GD_Documentos Fases 1-4 (2026-01-11)

Fuentes revisadas: CoreProject/Clases/GD/GD_Procedimientos.vb, GD_Model.Context.vb/edmx, CSV previos (MAPEO_SP_GD.csv, MAPEO_SP_MAESTRO.csv, MAPEO_SP_REPOSITORIO.csv, MAPEO_SP_SOLICITUDES.csv), WebForms en WebMatrix/GD_Documentos. Alcance: infraestructura, catálogos, maestro/controlados, solicitudes, aprobaciones, repositorio (Fases 1-4). Fase 5 (PNC) se asume ya cerrada.

## Catálogos (Tipo Solicitud, Estado, Procesos)
| Acción legacy | WebForm/DataLayer | Adapter MatrixNext | SP exacto | Parámetros según CoreProject | Estado MatrixNext |
| --- | --- | --- | --- | --- | --- |
| Listar tipos | GD_TipoSolicitud.aspx | IGdCatalogosAdapter.ObtenerTipoSolicitudes | GD_TipoSolicitud_Get | sin params | Implementado |
| Crear tipo | GD_TipoSolicitud.aspx | IGdCatalogosAdapter.CrearTipoSolicitud | GD_TipoSolicitud_Add | @Tipo (nomTipoSol) | Implementado |
| Editar tipo | GD_TipoSolicitud.aspx | IGdCatalogosAdapter.ActualizarTipoSolicitud | GD_TipoSolicitud_Edit | @id, @Tipo | Implementado |
| Eliminar tipo | GD_TipoSolicitud.aspx | IGdCatalogosAdapter.EliminarTipoSolicitud | GD_TipoSolicitud_Del | @idTipoSol | Implementado |
| Listar estados | GD_EstadoSolicitud.aspx | IGdCatalogosAdapter.ObtenerEstadosSolicitud | GD_Estados_Get | sin params | Implementado |
| Crear estado | GD_EstadoSolicitud.aspx | IGdCatalogosAdapter.CrearEstadoSolicitud | GD_EstadoSolicitud_Add | @Estado (nomEstadoSol) | Implementado |
| Editar estado | GD_EstadoSolicitud.aspx | IGdCatalogosAdapter.ActualizarEstadoSolicitud | GD_EstadoSolicitud_Edit | @idEstadoSol, @Estado | Implementado |
| Eliminar estado | GD_EstadoSolicitud.aspx | IGdCatalogosAdapter.EliminarEstadoSolicitud | GD_EstadoSolicitud_Del | @idEstadoSol | Implementado |
| Listar procesos | GD_Procesos.aspx | IGdCatalogosAdapter.ObtenerProcesos | GD_Procesos_Get | sin params | Implementado |
| Crear proceso | GD_Procesos.aspx | IGdCatalogosAdapter.CrearProceso | GD_Procesos_Add | @Proceso (nomProceso) | Implementado |
| Editar proceso | GD_Procesos.aspx | IGdCatalogosAdapter.ActualizarProceso | GD_Procesos_Edit | @idProceso, @Proceso | Implementado |
| Eliminar proceso | GD_Procesos.aspx | IGdCatalogosAdapter.EliminarProceso | GD_Procesos_Del | @idProceso | Implementado |

## Maestro y documentos controlados
| Acción legacy | WebForm/DataLayer | Adapter MatrixNext | SP exacto | Parámetros según CoreProject | Estado MatrixNext |
| --- | --- | --- | --- | --- | --- |
| Listar maestros | GD_Maestro.aspx / GD_Documentos.aspx | IGdMaestroAdapter.ObtenerMaestros | GD_MaestroDocumentos_Get | sin params | Implementado |
| Obtener maestro por id | GD_Maestro.aspx | IGdMaestroAdapter.ObtenerMaestroById | GD_GD_MaestroDocumentos_Get2 | @IdDocumento (+ filtros opcionales null) | Implementado |
| Crear maestro + controlado | GD_Maestro.aspx | IGdMaestroAdapter.CrearMaestroConControlled | GD_MaestroDocumentos_Add2; GD_DocumentosControlados_Add | Add2: @doc, @controlado, @activo, @codigo, @idProc, @Responsable; Controlados_Add: @docId, @activo, @ubiArchivo, @metRecuperacion, @tiempoRetencion, @dispoFinal | Implementado |
| Actualizar controlado (constitución/actualización) | GD_Maestro.aspx | IGdMaestroAdapter.ActualizarMaestroConstitucion / ActualizarMaestroActualizacion | GD_DocumentosControlados_Add | @docId, @activo, @ubiArchivo, @metRecuperacion, @tiempoRetencion, @dispoFinal | Implementado (usa mismo SP como upsert) |
| Anular maestro | GD_Maestro.aspx | IGdMaestroAdapter.AnularMaestro | GD_DocumentosMaestros_Update | @docId | Implementado |
| Anular controlado | GD_Maestro.aspx | IGdMaestroAdapter.AnularControlado | GD_DocumentosControlados_Activo | @docId | Implementado |

## Solicitudes y workflow inicial
| Acción legacy | WebForm/DataLayer | Adapter MatrixNext | SP exacto | Parámetros según CoreProject | Estado MatrixNext |
| --- | --- | --- | --- | --- | --- |
| Listar/obtener solicitudes | GD_SolicitudDocumentos.aspx | IGdSolicitudesAdapter.ObtenerSolicitudes / ObtenerSolicitudById | GD_SolDocumentos_Get | @Id opcional | Implementado |
| Crear solicitud | GD_SolicitudDocumentos.aspx | IGdSolicitudesAdapter.CrearSolicitud | GD_SolDocumentos_Add | @FechaSolicitud, @IdSolicitante, @area, @cargo, @IdTipo, @IdDocumento, @NombreDocumento, @Codigo, @AreaUso, @SitioAcceso, @RazonSolicitud, @DescripcionSolicitud, @IdEstado, @FechaEstado, @Comentarios, @Modificacion | Implementado |
| Asignar revisores | GD_SolicitudDocumentos.aspx | IGdSolicitudesAdapter.CrearRevision | GD_Revisiones_Add | @DocumentoId, @UsuarioId, @FechaAprobacion, @TipoRevision | Implementado |
| Listar estados (dropdown) | GD_SolicitudDocumentos.aspx | IGdSolicitudesAdapter.ObtenerEstados | GD_EstadoSolicitud_Get_F | sin params | Implementado |
| Listar tipos (dropdown) | GD_SolicitudDocumentos.aspx | IGdSolicitudesAdapter.ObtenerTiposSolicitud | GD_TipoSolicitud_Get | sin params | Implementado |
| Listar documentos (dropdown) | GD_SolicitudDocumentos.aspx | IGdSolicitudesAdapter.ObtenerDocumentos | GD_MaestroDocumentos_Get | sin params | Implementado |
| Listar usuarios (revisores) | GD_SolicitudDocumentos.aspx | IGdSolicitudesAdapter.ObtenerUsuarios | GD_US_Usuarios_Get | sin params | Implementado |

## Aprobaciones / Revisiones (workflow)
| Acción legacy | WebForm/DataLayer | Adapter MatrixNext | SP exacto | Parámetros según CoreProject | Estado MatrixNext |
| --- | --- | --- | --- | --- | --- |
| Listar revisiones pendientes/aprobar | GD_Aprobaciones.aspx / Aprobacion.aspx / Revision.aspx | IGdAprobacionesAdapter.RevisionesGetRev | GD_Revisiones_GetRev | @Usuario | Implementado |
| Editar revisión (aprobar) | GD_Aprobaciones.aspx / Revision.aspx | IGdAprobacionesAdapter.RevisionesEdit | GD_Revisiones_Edit | @IdRevision, @DocumentoId, @UsuarioId, @FechaAprobacion, @TipoRevision (3 = aprobado) | Implementado (aprobación). Rechazo no evidenciado en scripts; pendiente confirmación si se requiere valor de TipoRevision/estado para rechazar. |
| Listar revisiones de solicitud | GD_Aprobaciones.aspx / Revision.aspx | IGdSolicitudesAdapter.ObtenerRevisoresPendientes / ObtenerRevisoresAprobados / ObtenerTotalRevisores | GD_Revisiones_Get | @SolicitudId | Implementado |
| WebForm legado mención "SolicitudDocumentos_Update" | (comentado en adapter) | — | — | No existe en CoreProject; no usar hasta confirmación en scripts SQL | No implementado (intencional) |

## Repositorio / Escáner
| Acción legacy | WebForm/DataLayer | Adapter MatrixNext | SP exacto | Parámetros según CoreProject | Estado MatrixNext |
| --- | --- | --- | --- | --- | --- |
| Listar documentos por contenedor/trabajo | GD_RepositorioDocumentos.aspx / GD_SeguimientoPNC.aspx | IGdRepositorioAdapter.ObtenerDocumentos / ObtenerDocumentosContenedor | GD_RepositorioDocumentos_GetXTrabajo; GD_RepositorioDocumentos_Get | GetXTrabajo: IdContenedor (otros null); Get: @Id opcional/@IdContenedor opcional | Implementado |
| Obtener documento por id | GD_RepositorioDocumentos.aspx | IGdRepositorioAdapter.ObtenerDocumentoById | GD_RepositorioDocumentos_Get | @Id | Implementado |
| Guardar documento (upload) | GD_RepositorioDocumentos.aspx | IGdRepositorioAdapter.GuardarDocumento | GD_GD_RepositorioDocumentos_Add | @Nombre, @Url, @DocumentoId, @Version, @Fecha, @Comentarios, @UsuarioId, @IdContenedor | Implementado |
| Eliminar documento/escáner | GD_SeguimientoPNC.aspx | IGdRepositorioAdapter.EliminarDocumento | GD_EscanerDocumentos_Del | @Id, @IdTrabajo=null, @IdDocumento=null | Implementado |
| Próxima versión | — | IGdRepositorioAdapter.ObtenerProximaVersion | (query directa sobre GD_RepositorioDocumentos) | @IdContenedor, @IdDocumento | Implementado |

## Observaciones
- No se encontró `SolicitudDocumentos_Update` en CoreProject; cualquier actualización de solicitud debe revisar scripts SQL (`CO_Matrix_Structure_SP.sql`). Mantener pendiente hasta confirmación con DBA.
- Parámetros deben validarse contra `CO_Matrix_Structure_SP.sql`/`CO_Matrix_Structure_Tables.sql` antes de cambios; este mapeo se basa en CoreProject.
- PNC (Fase 5) ya migrado; no duplicar SP ni flows.
- Todos los adapters actuales ya usan estos SP; las acciones faltantes a revisar son gaps de UI/flujo, no de acceso a datos.

## Validación de tipos contra `CO_Matrix_Structure_SP.sql` (2026-01-11)
- GD_DocumentosControlados_Add(@docId bigint, @activo bit, @ubiArchivo varchar(250), @metRecuperacion varchar(50), @tiempoRetencion varchar(50), @dispoFinal varchar(50)). Adapter coincide (strings/bit). Upsert por versión.
- GD_MaestroDocumentos_Add2(@doc varchar(250), @controlado bit, @activo bit, @codigo varchar(100), @idProc smallint, @Responsable varchar(max)). Adapter envía Responsable.ToString(); validar largo si llega GUID.
- GD_DocumentosMaestros_Update(@docId bigint) y GD_DocumentosControlados_Activo(@docId bigint) usados para anular; tipos alineados.
- GD_SolDocumentos_Add: fechas datetime; @Solicitante bigint; @Area/@Cargo varchar(100); @TipoId int; @DocumentoId bigint; @NombreDocumento varchar(250); @Codigo varchar(100); @AreaUso/@SitioAcceso varchar(250); @RazonSolicitud/@DescripcionSolicitud/@Comentarios/@Modificacion varchar(max implícito en script); @EstadoId tinyint; @FechaEstado datetime. Adapter usa strings/nullables; alinear longitudes si se agregan validaciones.
- GD_Revisiones_Add/Edit/Get/GetRev: @DocumentoId int, @UsuarioId int, @FechaAprobacion datetime, @TipoRevision int, @IdRevision int (solo Edit). Adapters usan int y DateTime; sin desajustes.
- GD_RepositorioDocumentos_GetXTrabajo: parámetros opcionales (bigint/varchar(max)/float/datetime/bit). Adapter pasa solo IdContenedor y null resto: válido. Considerar @esRecuperacion bit para filtros futuros.
- GD_EscanerDocumentos_Del: @IdTrabajo bigint, @Id bigint, @IdDocumento bigint. Adapter envía Id y nulls: válido.

## Sprint 1 — Estado de implementación (Cierre 2026-01-11)
✅ **Catalogos (TipoSolicitud, EstadoSolicitud, Procesos)**: Adapters/Services/Controllers/Views completos; CRUD funcional.  
✅ **Solicitudes**: Create (GD_SolDocumentos_Add con parámetros NombreDocumento/Codigo alineados al SP signature), List, Assign-Reviewers (GD_Revisiones_Add); ViewModels ajustados; modales implementados.  
✅ **Aprobaciones**: List pending reviews (GD_Revisiones_GetRev), Approve action (GD_Revisiones_Edit con TipoRevision=3); controller/view funcionando.  
✅ **Build limpio**: sin errores de compilación; warnings Azure.Identity suprimidos con NoWarn.  
⏸️ **Pendiente**: QA funcional sobre staging (create/edit/delete catalogos, crear solicitud, asignar revisores, aprobar); Sprint 2 (maestro/workflow avanzado).

## Sprint 2 — Estado de implementación (Cierre 2026-01-11)
✅ **Maestro (Construcción/Actualización/Anulación)**: GdMaestroAdapter con GD_MaestroDocumentos_Add2 (construcción + controlado), GD_DocumentosControlados_Add (actualización/constitución vía upsert), GD_DocumentosMaestros_Update (anular maestro), GD_DocumentosControlados_Activo (anular controlado); DocumentosMaestroController con Create/Edit/Delete; modales funcionando; lógica condicional por TipoSolicitud (1=Construcción, 2=Actualización, 3=Anulación).  
✅ **Workflow completo**: AprobacionesController (GD_Revisiones_GetRev para pendientes, GD_Revisiones_Edit con TipoRevision=3 para aprobar); SolicitudesController asigna revisores (GD_Revisiones_Add); email service integrado.  
✅ **Permisos**: Todos controllers GD con `[Authorize]` (CatalogosController, SolicitudesController, AprobacionesController, DocumentosMaestroController, RepositorioController, PncController, DashboardController).  
✅ **Build limpio**: sin errores de compilación.  
⏸️ **Pendiente QA staging**: Maestro (crear construcción con campos Nombre/Codigo/Proceso/Responsable/Ubicacion/MetodoRecuperacion/TiempoRetencion/DisposicionFinal; actualizar documento existente vía tipo 2; anular vía tipo 3); Workflow (crear solicitud, asignar N revisores, aprobar como revisor, verificar email); permisos (acceso restringido sin auth).
