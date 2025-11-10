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


Partial Public Class AvanceDeCampoDialog

    '''<summary>
    '''Control upDatos.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents upDatos As Global.System.Web.UI.UpdatePanel

    '''<summary>
    '''Control hfidTrabajo.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents hfidTrabajo As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control ChartEstado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ChartEstado As Global.System.Web.UI.DataVisualization.Charting.Chart

    '''<summary>
    '''Control lblVariacion.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblVariacion As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control lblNoData.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents lblNoData As Global.System.Web.UI.WebControls.Label

    '''<summary>
    '''Control gvAvanceCiudad.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvAvanceCiudad As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control ddVariables.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddVariables As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control gvAvanceCuotas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvAvanceCuotas As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control SqlDsAvanceCuotasPreg.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents SqlDsAvanceCuotasPreg As Global.System.Web.UI.WebControls.SqlDataSource

    '''<summary>
    '''Control SqlDsAvanceCuotas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents SqlDsAvanceCuotas As Global.System.Web.UI.WebControls.SqlDataSource

    '''<summary>
    '''Control ddColumna.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddColumna As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control ddFila.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddFila As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control gvAvanceCuotasCruce.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvAvanceCuotasCruce As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control SqlDsAvanceCruce.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents SqlDsAvanceCruce As Global.System.Web.UI.WebControls.SqlDataSource

    '''<summary>
    '''Control gvAvanceAreas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvAvanceAreas As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control gvRemanentes.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvRemanentes As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control gvAnuladas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvAnuladas As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control btnExportar.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents btnExportar As Global.System.Web.UI.WebControls.Button

    '''<summary>
    '''Control gvExportado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvExportado As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control SqlDsExportado.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents SqlDsExportado As Global.System.Web.UI.WebControls.SqlDataSource

    '''<summary>
    '''Control HiddenField1.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents HiddenField1 As Global.System.Web.UI.WebControls.HiddenField

    '''<summary>
    '''Control gvMatriz.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvMatriz As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control ddlAreasTrafico.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents ddlAreasTrafico As Global.System.Web.UI.WebControls.DropDownList

    '''<summary>
    '''Control gvTraficoAreas.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvTraficoAreas As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control gvEncuestadores.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents gvEncuestadores As Global.System.Web.UI.WebControls.GridView

    '''<summary>
    '''Control SqlDsEncuestadores.
    '''</summary>
    '''<remarks>
    '''Campo generado automáticamente.
    '''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
    '''</remarks>
    Protected WithEvents SqlDsEncuestadores As Global.System.Web.UI.WebControls.SqlDataSource
End Class
