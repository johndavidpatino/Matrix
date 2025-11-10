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


Partial Public Class FI_Gestion_Ordenes_Aprobacion
    
    '''<summary>
    '''Control ToolkitScriptManager1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ToolkitScriptManager1 As Global.AjaxControlToolkit.ToolkitScriptManager
    
    '''<summary>
    '''Control upTarea.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents upTarea As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control hfId.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfId As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control hfTipoOrden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfTipoOrden As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control LinkButton1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents LinkButton1 As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control LinkButton2.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents LinkButton2 As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control LinkButton3.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents LinkButton3 As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control LinkButton7.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents LinkButton7 As Global.System.Web.UI.WebControls.LinkButton
    
    '''<summary>
    '''Control txtTitulo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtTitulo As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control gvOrdenes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvOrdenes As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control pnlAprobaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents pnlAprobaciones As Global.System.Web.UI.WebControls.Panel
    
    '''<summary>
    '''Control lblEstadoAutorizaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblEstadoAutorizaciones As Global.System.Web.UI.WebControls.Label
    
    '''<summary>
    '''Control gvAprobaciones.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvAprobaciones As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control lblAprobacionActual.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblAprobacionActual As Global.System.Web.UI.WebControls.Label
    
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
    '''Control UPanelSolicitantes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents UPanelSolicitantes As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control txtCedulaSolicitante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtCedulaSolicitante As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtNombreSolicitante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNombreSolicitante As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control btnBuscarSolicitante.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnBuscarSolicitante As Global.System.Web.UI.WebControls.Button
End Class
