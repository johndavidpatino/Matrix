'------------------------------------------------------------------------------
' <generado automáticamente>
'     Este código fue generado por una herramienta.
'
'     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
'     se vuelve a generar el código. 
' </generado automáticamente>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Partial Public Class AprobacionesFiltros
    
    '''<summary>
    '''Control lnkProyecto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lnkProyecto As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control upDatos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents upDatos As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control lblTextoTrabajo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTextoTrabajo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control lblTrabajo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblTrabajo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control hfIdProyecto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfIdProyecto As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfIdTrabajo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfIdTrabajo As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfIdFiltro.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfIdFiltro As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfIdRespuesta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfIdRespuesta As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfEstado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfEstado As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfPY.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfPY As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control btnImgExportarInforme.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnImgExportarInforme As Global.System.Web.UI.WebControls.ImageButton
    
    '''<summary>
    '''Control gvRespuestas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvRespuestas As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control upAprobacionFiltros.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents upAprobacionFiltros As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control pnlFiltros.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlFiltros As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control pnlAprobar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlAprobar As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control txtComentarios.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtComentarios As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control btnAprobar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnAprobar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnNoAprobar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnNoAprobar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control upReporteFiltros.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents upReporteFiltros As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control gvReporteFiltros.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvReporteFiltros As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control SqlDataSource1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents SqlDataSource1 As Global.System.Web.UI.WebControls.SqlDataSource
End Class
