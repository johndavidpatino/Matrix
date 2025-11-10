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


Partial Public Class DetalleRadicacion
    
    '''<summary>
    '''Control upBuscar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents upBuscar As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control txtIdOrden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtIdOrden As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control ddlTipoOrden.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlTipoOrden As Global.System.Web.UI.WebControls.DropDownList
    
    '''<summary>
    '''Control txtProveedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtProveedor As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control hfProveedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfProveedor As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control btnSearchProveedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnSearchProveedor As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control btnLimpiar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnLimpiar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control txtIdTrabajo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtIdTrabajo As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtOrdenFechaInicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtOrdenFechaInicio As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtOrdenFechaFin.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtOrdenFechaFin As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtFacturaConsecutivo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFacturaConsecutivo As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtFacturaFechaInicio.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFacturaFechaInicio As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtFacturaFechaFin.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtFacturaFechaFin As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control btnBuscar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnBuscar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control hfNumRequerimiento.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfNumRequerimiento As Global.System.Web.UI.WebControls.HiddenField
    
    '''<summary>
    '''Control btnExportar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnExportar As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control upOrdenesFacturas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents upOrdenesFacturas As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control gvDatos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvDatos As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control UPanelProveedores.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents UPanelProveedores As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control txtNitProveedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNitProveedor As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtNombreProveedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNombreProveedor As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control btnBuscarProveedor.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnBuscarProveedor As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control gvProveedores.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvProveedores As Global.System.Web.UI.WebControls.GridView
    
    '''<summary>
    '''Control UPanelCentroCosto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents UPanelCentroCosto As Global.System.Web.UI.UpdatePanel
    
    '''<summary>
    '''Control txtNumeroCuenta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtNumeroCuenta As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control txtDescripcionCuenta.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents txtDescripcionCuenta As Global.System.Web.UI.WebControls.TextBox
    
    '''<summary>
    '''Control btnBuscarCentroCosto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnBuscarCentroCosto As Global.System.Web.UI.WebControls.Button
    
    '''<summary>
    '''Control gvCentroCosto.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvCentroCosto As Global.System.Web.UI.WebControls.GridView
End Class
