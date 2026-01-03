# Reporte Implementacion CU_CUENTAS (Fase 1)

## Archivos creados/modificados
- MatrixNext.Data/Entities/CU_Brief.cs
- MatrixNext.Data/Entities/CU_Propuestas.cs
- MatrixNext.Data/Entities/CU_Estudios.cs
- MatrixNext.Data/Entities/CU_Estudios_Presupuestos.cs
- MatrixNext.Data/Entities/CU_SeguimientoPropuestas.cs
- MatrixNext.Data/Modules/CU/Models/JobBookViewModels.cs
- MatrixNext.Data/Modules/CU/Models/PropuestaViewModels.cs
- MatrixNext.Data/Modules/CU/Models/EstudioViewModels.cs
- MatrixNext.Data/Modules/CU/Models/BriefViewModels.cs
- MatrixNext.Data/Adapters/CU/CuentaDataAdapter.cs
- MatrixNext.Data/Adapters/CU/PropuestaDataAdapter.cs
- MatrixNext.Data/Adapters/CU/EstudioDataAdapter.cs
- MatrixNext.Data/Adapters/CU/BriefDataAdapter.cs
- MatrixNext.Data/Services/CU/CuentaService.cs
- MatrixNext.Data/Services/CU/PropuestaService.cs
- MatrixNext.Data/Services/CU/EstudioService.cs
- MatrixNext.Data/Services/CU/BriefService.cs
- MatrixNext.Data/Modules/CU/ServiceCollectionExtensions.cs
- MatrixNext.Web/Areas/CU/Controllers/CuentasController.cs
- MatrixNext.Web/Areas/CU/Controllers/PropuestasController.cs
- MatrixNext.Web/Areas/CU/Controllers/EstudiosController.cs
- MatrixNext.Web/Areas/CU/Controllers/BriefController.cs
- MatrixNext.Web/Areas/CU/Views/Cuentas/Index.cshtml, _ResultadosGrid.cshtml
- MatrixNext.Web/Areas/CU/Views/Propuestas/Index.cshtml, _ModalCrear.cshtml, _ModalObservaciones.cshtml
- MatrixNext.Web/Areas/CU/Views/Estudios/Index.cshtml, _ModalCrear.cshtml
- MatrixNext.Web/Areas/CU/Views/Brief/Index.cshtml
- MatrixNext/Areas/CU/TODO_CU_CUENTAS.md
- MatrixNext/MatrixNext.Web/Program.cs
- MatrixNext/MatrixNext.Data/Entities/MatrixDbContext.cs
- MatrixNext/MatrixNext.Web/Areas/CU/Views/_ViewImports.cshtml, _ViewStart.cshtml

## Rutas MVC implementadas
- /CU/Cuentas (GET) Index; /CU/Cuentas/Buscar (POST parcial); /CU/Cuentas/Abrir (GET redireccion segun contexto); /CU/Cuentas/Clonar (POST JSON, pendiente por SP).
- /CU/Propuestas (GET Index con filtro); /CU/Propuestas/Crear (GET modal); /CU/Propuestas/Editar/{id} (GET modal); /CU/Propuestas/Guardar (POST JSON); /CU/Propuestas/Eliminar/{id} (POST JSON); /CU/Propuestas/Observaciones/{id} (GET modal); /CU/Propuestas/AgregarObservacion (POST JSON).
- /CU/Estudios (GET Index por propuesta); /CU/Estudios/Crear (GET modal); /CU/Estudios/Editar/{id} (GET modal); /CU/Estudios/Guardar (POST JSON).
- /CU/Brief (GET Index); /CU/Brief/Guardar (POST JSON); /CU/Brief/Viabilidad (POST JSON).

## Mapeo vs ANALISIS_CU_CUENTAS.md
- Default.aspx → OK: busqueda JobBooks via SP `CU_InfoGeneralJobBook_GET`, grid con Ver; PENDIENTE: clonar brief (falta SP `CloneBrief`).
- Propuestas.aspx → OK: listado y filtro por estado, modal create/edit con validaciones por estado, observaciones; PENDIENTE: integracion presupuestos/alternativas (Fase2), export/logging legacy, uso de JobBook masking avanzado.
- Estudio.aspx → OK: listado por propuesta y modal CRUD basico; PENDIENTE: validacion y asignacion de presupuestos aprobados, carga de documentos, creacion de proyectos PY y correos, cambio de alternativas.
- Frame.aspx → OK: formulario con Quill (4 campos HTML), validaciones base, cambio de viabilidad; PENDIENTE: auto-creacion de propuesta tras guardar brief, carga de archivos, tabs completos de 70+ campos (se exponen O*, D*, C*, M*, DI* pero sin logica extra).

## TODOs pendientes (ver MatrixNext/Areas/CU/TODO_CU_CUENTAS.md)
- Confirmar y mapear SP `CloneBrief` (Default.aspx.vb l.84-93).
- Asignacion de presupuestos aprobados al crear estudio (`CU_Presupuestos.DevolverxIdPropuestaAprobados`, `Estudios_Presupuestos.Grabar`).
- Sustituir `UC_LoadFiles.ascx` para carga de documentos en Brief/Estudio.
- Creacion de proyectos PY y correos asociados al guardar estudios.
- Auto-creacion de propuesta al guardar Brief (Frame.aspx.vb SavePropuesta).

## Pasos para probar manualmente
1. Ejecutar `dotnet build MatrixNext.sln` (ya verificado).
2. Navegar a `/CU/Cuentas`: probar busqueda por titulo/jobbook/id propuesta y navegar con boton “Ver”.
3. Desde resultados, ingresar a `/CU/Brief` y crear/editar brief; verificar validaciones y marcado de viabilidad.
4. Abrir `/CU/Propuestas` (idealmente con contexto de brief) y crear/editar propuestas validando reglas por estado; agregar observaciones.
5. Desde propuestas, abrir `/CU/Estudios?idPropuesta={id}` y crear/editar estudio (validacion basica de fechas y jobbook).
6. Revisar consola del navegador para solicitudes AJAX; confirmar mensajes en modales.

