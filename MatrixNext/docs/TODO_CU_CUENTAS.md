# TODO CU_CUENTAS (Fase 1)

Registra aqui evidencias pendientes o funcionalidad no implementada por falta de informacion. Formato recomendado:
- Que falta
- Donde buscar (archivo legacy / metodo / SP)
- Impacto

- Confirmar SP/metodo `CloneBrief` usado por `CU_JobBook.DAL.CloneBrief` (WebMatrix/CU_Cuentas/Default.aspx.vb l.84-93). Impacto: clonacion de briefs no migrada.
- Asignacion de presupuestos aprobados al crear estudio (legacy `CU_Presupuestos.DevolverxIdPropuestaAprobados`, `Estudios_Presupuestos.Grabar`). Revisar WebMatrix/CU_Cuentas/Estudio.aspx.vb (btnNew_Click, btnSave_Click). Impacto: no se valida presupuesto ni se asocian presupuestos al estudio.
- Carga de documentos (sustituir `UC_LoadFiles.ascx` en Estudio/Frame). Revisar WebMatrix/CU_Cuentas/Estudio.aspx.vb metodo `LoadFiles_Click` y componente `UC_LoadFiles.ascx`. Impacto: usuarios no pueden adjuntar soporte en estudios/briefs.
- Creacion de proyectos (PY_Proyectos) al guardar estudios (`Estudio.aspx.vb btnSave_Click`). Impacto: no se generan proyectos ni se envian correos asociados (AnuncioAprobacion*, NuevoProyecto*).
- Auto-creacion de propuesta al guardar Brief (Frame.aspx.vb SavePropuesta). Revisar llamada `SavePropuesta()` tras guardar brief y mapear parametros a PropuestaService. Impacto: tras crear un brief no se genera la propuesta inicial automaticamente.
- Migracion completa de Presupuesto.aspx (Fase 2) pendiente: falta formulario de presupuesto, calculadora IQuote y CRUD de presupuestos por tecnica. Revisar WebMatrix/CU_Cuentas/Presupuesto.aspx(.vb) metodos `SavePresupuesto`, `CargarPresupuestos` y clase `CoreProject.Cotizador.General`. Impacto: solo se guardan datos generales de alternativas; no se pueden crear/editar presupuestos.
- Gestion de muestra (F2F/CATI/Online) no implementada. Evidencia en Presupuesto.aspx.vb (`GetMuestraF2F`, `GetMuestraCati`, `GetMuestraOnline`, tabla IQ_Muestra_1). Impacto: no se capturan distribuciones de muestra ni se recalculan totales.
- JobBook Interno/Externo, Simulador y Ajuste de GM no migrados. Revisar Presupuesto.aspx.vb (comandos `JBIP`, `JBEP`, `SimulatorP`, `AjustarGM`) y SP `CU_PresupuestosRevisionPorGerenteOperaciones`. Impacto: gerencia de operaciones no puede revisar/aprobar presupuestos ni generar reportes JBI/JBE.
- Importaciones/duplicaciones pendientes: copiar alternativa/presupuesto y carga de muestra desde Excel (ClosedXML). Revisar Presupuesto.aspx.vb (`btnDuplicarAlternativa_Click`, `btnImportar_Click`, `btnLoadDataExcel_Click`). Impacto: usuarios no pueden reutilizar presupuestos existentes ni acelerar carga de datos.
