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


Partial Public Class PresupuestoInterno

	'''<summary>
	'''Control upDatos.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents upDatos As Global.System.Web.UI.UpdatePanel

	'''<summary>
	'''Control hfidpresupuesto.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents hfidpresupuesto As Global.System.Web.UI.WebControls.HiddenField

	'''<summary>
	'''Control hftipopresupuesto.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents hftipopresupuesto As Global.System.Web.UI.WebControls.HiddenField

	'''<summary>
	'''Control hfidTrabajo.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents hfidTrabajo As Global.System.Web.UI.WebControls.HiddenField

	'''<summary>
	'''Control hfIdMet.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents hfIdMet As Global.System.Web.UI.WebControls.HiddenField

	'''<summary>
	'''Control txtBuscar.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtBuscar As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control ddlTipoPresupuesto.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents ddlTipoPresupuesto As Global.System.Web.UI.WebControls.DropDownList

	'''<summary>
	'''Control ddlAño.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents ddlAño As Global.System.Web.UI.WebControls.DropDownList

	'''<summary>
	'''Control btbbuscar.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btbbuscar As Global.System.Web.UI.WebControls.Button

	'''<summary>
	'''Control btnCrear.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnCrear As Global.System.Web.UI.WebControls.Button

	'''<summary>
	'''Control gvMuestra.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents gvMuestra As Global.System.Web.UI.WebControls.GridView

	'''<summary>
	'''Control pnlDetail.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents pnlDetail As Global.System.Web.UI.WebControls.Panel

	'''<summary>
	'''Control lblObservacion.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblObservacion As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblMetodologia.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblMetodologia As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control TxtProductividad.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents TxtProductividad As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control txtMuestra.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtMuestra As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control btnCalcular.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnCalcular As Global.System.Web.UI.WebControls.Button

	'''<summary>
	'''Control btnAjustar.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnAjustar As Global.System.Web.UI.WebControls.Button

	'''<summary>
	'''Control btnAjustarSupervision.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnAjustarSupervision As Global.System.Web.UI.WebControls.Button

	'''<summary>
	'''Control gvCostos.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents gvCostos As Global.System.Web.UI.WebControls.GridView

	'''<summary>
	'''Control gvReclutamiento.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents gvReclutamiento As Global.System.Web.UI.WebControls.GridView

	'''<summary>
	'''Control txtObservaciones.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtObservaciones As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control btnSaveObservaciones.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnSaveObservaciones As Global.System.Web.UI.WebControls.Button

	'''<summary>
	'''Control upPresupuestos.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents upPresupuestos As Global.System.Web.UI.UpdatePanel

	'''<summary>
	'''Control txtValor.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtValor As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control btnGuardar.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnGuardar As Global.System.Web.UI.WebControls.Button

	'''<summary>
	'''Control upSupervision.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents upSupervision As Global.System.Web.UI.UpdatePanel

	'''<summary>
	'''Control txtValorSupervision.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtValorSupervision As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control rbTipoSupervision.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents rbTipoSupervision As Global.System.Web.UI.WebControls.RadioButtonList

	'''<summary>
	'''Control btnActualizarVrSupervision.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnActualizarVrSupervision As Global.System.Web.UI.WebControls.Button
End Class
