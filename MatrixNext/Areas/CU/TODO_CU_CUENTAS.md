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
