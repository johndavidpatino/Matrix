# REPORT_CU_PRESUPUESTO (Fase 2)

## Avance implementado
- Se agregaron entidades EF Core para CU_Presupuestos e IQ_* (Parametros, DatosGeneralesPresupuesto, Muestra, Preguntas, Procesos, CostoActividades, ControlCostos, Opciones*), habilitando consultas y upserts desde MatrixNext.
- Nuevo `PresupuestoDataAdapter` expone lectura de alternativas y guardado de datos generales con asignaci\u00f3n autom\u00e1tica de n\u00famero de alternativa.
- Nuevo `PresupuestoService` y `PresupuestoController` (Area CU) con vista `Index` para listar alternativas de una propuesta y modal de edici\u00f3n de datos generales.
- Documentados pendientes cr\u00edticos en `Areas/CU/TODO_CU_CUENTAS.md` (IQuote, muestra, JBI/JBE, simulador, importaciones).

## Archivos creados/modificados
- Entities: `MatrixNext.Data/Entities/CU_Presupuestos.cs`, `IQ_Parametros.cs`, `IQ_DatosGeneralesPresupuesto.cs`, `IQ_Muestra_1.cs`, `IQ_Preguntas.cs`, `IQ_ProcesosPresupuesto.cs`, `IQ_CostoActividades.cs`, `IQ_ControlCostos.cs`, `IQ_OpcionesAplicadas.cs`, `IQ_OpcionesTecnicas.cs`, `IQ_Procesos.cs`, `IQ_TecnicaProcesos.cs`, `MatrixNext.Data/Entities/CU_Propuestas.cs`, `MatrixNext.Data/Entities/MatrixDbContext.cs`.
- Data: `MatrixNext.Data/Modules/CU/Models/PresupuestoViewModels.cs`, `MatrixNext.Data/Adapters/CU/PresupuestoDataAdapter.cs`, `MatrixNext.Data/Services/CU/PresupuestoService.cs`, `MatrixNext.Data/Modules/CU/ServiceCollectionExtensions.cs`.
- Web: `MatrixNext.Web/Areas/CU/Controllers/PresupuestoController.cs`, `MatrixNext.Web/Areas/CU/Views/Presupuesto/Index.cshtml`, `MatrixNext.Web/Areas/CU/Views/Presupuesto/_ModalAlternativa.cshtml`.
- Docs: `MatrixNext/Areas/CU/TODO_CU_CUENTAS.md`, `MatrixNext/MODULOS_MIGRACION.md`, `MatrixNext/DASHBOARD_MIGRACION.md`, `MatrixNext/REPORT_CU_PRESUPUESTO.md`.

## Rutas MVC implementadas
- `/CU/Presupuesto` (GET): vista principal de alternativas (requiere contexto de propuesta).
- `/CU/Presupuesto/Alternativa?idPropuesta={id}&alternativa={opc}` (GET Partial): modal para crear/editar datos generales de alternativa.
- `/CU/Presupuesto/GuardarAlternativa` (POST): guarda datos generales de alternativa.

## Mapeo vs ANALISIS_CU_PRESUPUESTO.md
- Presupuesto.aspx (Listado de Alternativas / Panel Datos Generales) \u2192 **PARCIAL OK**: se lista por propuesta, modal para descripci\u00f3n y d\u00edas base. Falta validaci\u00f3n avanzada y estados OPS.
- Presupuesto.aspx (Modal Presupuesto completo, grids de presupuestos, muestra F2F/CATI/Online, actividades, an\u00e1lisis, horas, simulador, JBI/JBE, autorizaci\u00f3n GM, importaciones/duplicaciones) \u2192 **PENDIENTE** por falta de implementaci\u00f3n de motor IQuote y mapeo detallado (ver TODO).

## TODOs relevantes
- Formularios y c\u00e1lculos IQuote (guardar IQ_Parametros + tablas relacionadas). Evidencia: Presupuesto.aspx.vb `SavePresupuesto`, `Cotizador.General`.
- CRUD de presupuestos por t\u00e9cnica y grids asociados (F2F/CATI/Online, actividades, an\u00e1lisis, horas). Evidencia: tabla de mapeo secci\u00f3n 4 del an\u00e1lisis.
- Flujos de revisi\u00f3n/autorizaci\u00f3n GM y reportes JBI/JBE. Evidencia: comandos `ReviewP`, `JBIP`, `JBEP`, SP `CU_PresupuestosRevisionPorGerenteOperaciones`.
- Importar/duplicar alternativas y carga de muestra desde Excel (ClosedXML). Evidencia: botones `btnDuplicarAlternativa_Click`, `btnImportar_Click`, `btnLoadDataExcel_Click` en Presupuesto.aspx.vb.

## Pasos para probar
1. Ingresar a `/CU/Cuentas`, seleccionar un JobBook para cargar contexto y navegar a Presupuesto (usar redirect existente o URL directa `/CU/Presupuesto/{idPropuesta}`).
2. En la vista de Presupuesto, pulsar **Nueva alternativa** para abrir el modal y guardar; validar que la alternativa aparece en la tabla con el n\u00famero consecutivo.
3. Editar una alternativa existente desde el bot\u00f3n **Editar** y verificar persistencia de campos (descripci\u00f3n, observaciones, d\u00edas, mediciones).
4. Consultar tabla `IQ_DatosGeneralesPresupuesto` e `IQ_Parametros` (si aplica) para confirmar almacenamiento de datos generales.
