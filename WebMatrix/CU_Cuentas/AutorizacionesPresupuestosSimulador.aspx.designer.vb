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


Partial Public Class _AutorizacionesPresupuestosSimulador

	'''<summary>
	'''Control lblInfo.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblInfo As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control rbSearch.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents rbSearch As Global.System.Web.UI.WebControls.RadioButtonList

	'''<summary>
	'''Control txtPropuesta.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtPropuesta As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control txtFInicio.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtFInicio As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control txtFFin.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtFFin As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control ddlSL.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents ddlSL As Global.System.Web.UI.WebControls.DropDownList

	'''<summary>
	'''Control btnSearch.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnSearch As Global.System.Web.UI.WebControls.Button

	'''<summary>
	'''Control UPanelSearch.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents UPanelSearch As Global.System.Web.UI.UpdatePanel

	'''<summary>
	'''Control lkbModals.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lkbModals As Global.System.Web.UI.WebControls.LinkButton

	'''<summary>
	'''Control lblShowGMSimulator.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblShowGMSimulator As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control ModalPopupExtenderSimulator.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents ModalPopupExtenderSimulator As Global.AjaxControlToolkit.ModalPopupExtender

	'''<summary>
	'''Control gvDataSearch.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents gvDataSearch As Global.System.Web.UI.WebControls.GridView

	'''<summary>
	'''Control pnlGMSimulator.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents pnlGMSimulator As Global.System.Web.UI.WebControls.Panel

	'''<summary>
	'''Control UPanelSimulator.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents UPanelSimulator As Global.System.Web.UI.UpdatePanel

	'''<summary>
	'''Control hfIdSolicitud.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents hfIdSolicitud As Global.System.Web.UI.WebControls.HiddenField

	'''<summary>
	'''Control hfSimPropuesta.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents hfSimPropuesta As Global.System.Web.UI.WebControls.HiddenField

	'''<summary>
	'''Control hfSimAlternativa.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents hfSimAlternativa As Global.System.Web.UI.WebControls.HiddenField

	'''<summary>
	'''Control hfSimMetodologia.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents hfSimMetodologia As Global.System.Web.UI.WebControls.HiddenField

	'''<summary>
	'''Control hfSimFase.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents hfSimFase As Global.System.Web.UI.WebControls.HiddenField

	'''<summary>
	'''Control hfGMMin.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents hfGMMin As Global.System.Web.UI.WebControls.HiddenField

	'''<summary>
	'''Control lblSIMVrVenta.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMVrVenta As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMOtrosCostos.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMOtrosCostos As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMGM.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMGM As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMCargosGrupo.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMCargosGrupo As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMCostoOPS.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMCostoOPS As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMCostosDirectos.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMCostosDirectos As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMTargetProfessionalTime.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMTargetProfessionalTime As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMOP.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMOP As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMGMOPS.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMGMOPS As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMMargenBruto.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMMargenBruto As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMProfessionalTime.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMProfessionalTime As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control lblSIMOPPercent.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents lblSIMOPPercent As Global.System.Web.UI.WebControls.Label

	'''<summary>
	'''Control txtDiasCampo.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtDiasCampo As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control txtDiasDiseno.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtDiasDiseno As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control txtDiasProcesamiento.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtDiasProcesamiento As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control txtDiasInformes.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtDiasInformes As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control txtObservaciones.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtObservaciones As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control txtComentariosAprobacion.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents txtComentariosAprobacion As Global.System.Web.UI.WebControls.TextBox

	'''<summary>
	'''Control btnAprobarSolicitud.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnAprobarSolicitud As Global.System.Web.UI.WebControls.Button

	'''<summary>
	'''Control btnRechazarSolicitud.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnRechazarSolicitud As Global.System.Web.UI.WebControls.Button

	'''<summary>
	'''Control btnCancelSimulator.
	'''</summary>
	'''<remarks>
	'''Campo generado automáticamente.
	'''Para modificarlo, mueva la declaración del campo del archivo del diseñador al archivo de código subyacente.
	'''</remarks>
	Protected WithEvents btnCancelSimulator As Global.System.Web.UI.WebControls.Button
End Class
