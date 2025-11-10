Imports WebMatrix.Util
Imports CoreProject
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports CoreProject.CC_FinzOpe

Public Class PresupuestosInternos
    Inherits System.Web.UI.Page
    Dim op As New PresupInt
    Dim Pr As New ProcesosInternos
#Region "Propiedades"

    Private _idUsuario As Int64
    Public Property idUsuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
        End Set
    End Property


#End Region

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

            Me.ddlTipoPresupuesto.DataSource = OP.LstTipoPresupuesto
            ddlTipoPresupuesto.DataValueField = "TipoPresupuesto"
            ddlTipoPresupuesto.DataTextField = "Descripcion"
			ddlTipoPresupuesto.DataBind()

			ddlTipoPresupuesto.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))

			If Request.QueryString("TrabajoId") IsNot Nothing Then
                hfIdTrabajo.Value = Request.QueryString("TrabajoId").ToString
                PresupuestosInternos(hfIdTrabajo.Value)
            End If

        End If
        'ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvMuestra_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMuestra.RowCommand
        If e.CommandName = "VerDetalle" Then
            hfidpresupuesto.Value = gvMuestra.DataKeys(CInt(e.CommandArgument))("PresupuestoId")
            cargarpresupuesto(hfidpresupuesto.Value)
            DetallePresupuesto(hfidpresupuesto.Value)
            CargarMetodologia(hfIdTrabajo.Value)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "Actualizar" Then
            hfidpresupuesto.Value = gvMuestra.DataKeys(CInt(e.CommandArgument))("PresupuestoId")
            Dim pr As New PresupInt
            Dim presup = pr.GetPresupuestoInterno(hfidpresupuesto.Value)
            presup.Descripcion = DirectCast(gvMuestra.Rows(e.CommandArgument).FindControl("txtDescripcion"), TextBox).Text
            pr.SavePresupuestoInterno(presup)
            'Detallepresupuestoget(gvMuestra.DataKeys(CInt(e.CommandArgument))("PresupuestoId"))
            'ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "Eliminar" Then
            hfidpresupuesto.Value = gvMuestra.DataKeys(CInt(e.CommandArgument))("PresupuestoId")
            Pr.EliminarPresupuesto(hfidpresupuesto.Value)
            ShowNotification("Presupuesto Eliminado", ShowNotifications.InfoNotification)
            PresupuestosInternos(hfIdTrabajo.Value)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End If
    End Sub
    Protected Sub btnCalcular_Click(sender As Object, e As EventArgs) Handles btnCalcular.Click
        Dim op As New PresupInt
        op.ActualizarProductividad(hfidpresupuesto.Value, TxtProductividad.Text, TxtMuestra.Text)

        calcular(hfidpresupuesto.Value)
        CalcularGrilla()
        CalcularTotalPResupuesto()
        cargarpresupuesto(hfidpresupuesto.Value)
        DetallePresupuesto(hfidpresupuesto.Value)
        CargarMetodologia(hfIdTrabajo.Value)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        PresupuestosInternos(hfIdTrabajo.Value)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnduplicar_Click(sender As Object, e As EventArgs) Handles btnduplicar.Click
        'DuplicarMuestra()
    End Sub
    Protected Sub btnAjustar_Click(sender As Object, e As EventArgs) Handles btnAjustar.Click
        Me.upPresupuestos.Update()
    End Sub

    Protected Sub btnAjustarSupervision_Click(sender As Object, e As EventArgs) Handles btnAjustarSupervision.Click
        Me.upSupervision.Update()
    End Sub
    Protected Sub btnCrear_Click(sender As Object, e As EventArgs) Handles btnCrear.Click
		Dim Presupuestoid As Int64
		Dim presupuestos = New IQ.Consultas()
		Dim DALTrabajo = New Trabajo

		If (ddlTipoPresupuesto.SelectedValue = "-1" OrElse ddlAño.SelectedValue = "-1" OrElse String.IsNullOrEmpty(ddlAño.SelectedValue)) Then
			ShowNotification("Debe seleccionar un tipo y un año", ShowNotifications.ErrorNotification)
			Exit Sub
		End If

		Dim trabajo = DALTrabajo.obtenerXId(hfIdTrabajo.Value)

		If presupuestos.ObtenerParametros(trabajo.IdPropuesta, trabajo.Alternativa, trabajo.MetCodigo, trabajo.Fase).Count < 1 Then
			ShowNotification("No se encontro un presupuesto asociado al trabajo/alternativa/metodología es posible que el trabajo este desactualizado con respecto a la propuesta, consulte con IT Desarrollo", ShowNotifications.ErrorNotification)
			Exit Sub
		End If


		Presupuestoid = guardarpresupuesto(hfIdTrabajo.Value, Session("IDUsuario"), ddlTipoPresupuesto.SelectedValue, ddlAño.SelectedValue)
        hfidpresupuesto.Value = Presupuestoid
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        'lblTotal.Visible = False
        cargarpresupuesto(hfidpresupuesto.Value)
        DetallePresupuesto(hfidpresupuesto.Value)
        CargarMetodologia(hfIdTrabajo.Value)
        PresupuestosInternos(hfIdTrabajo.Value)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If txtValor.Text <> "" Then
            actualizarvalorcontratista(hfidpresupuesto.Value, txtValor.Text)

            cargarpresupuesto(hfidpresupuesto.Value)
            DetallePresupuesto(hfidpresupuesto.Value)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            ShowNotification("Valor Actualizado", ShowNotifications.InfoNotification)
        Else
            ShowNotification("Ingrese Valor", ShowNotifications.InfoNotification)
        End If
    End Sub
    Protected Sub btnActualizarVrSupervision_Click(sender As Object, e As EventArgs) Handles btnActualizarVrSupervision.Click
        If txtValorSupervision.Text <> "" And rbTipoSupervision.SelectedIndex > -1 Then
            actualizarvalorindividualcontratista(hfidpresupuesto.Value, txtValorSupervision.Text, rbTipoSupervision.SelectedValue)
            cargarpresupuesto(hfidpresupuesto.Value)
            DetallePresupuesto(hfidpresupuesto.Value)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            ShowNotification("Valor Actualizado", ShowNotifications.InfoNotification)
        Else
            ShowNotification("Ingrese Valor y tipo de supervision", ShowNotifications.InfoNotification)
        End If
    End Sub
#End Region

#Region "Metodos"
    Private Function guardarpresupuesto(ByVal trabajoid As Int64, ByVal Usuarioid As Int64, ByVal Tipo As Int64, IdAñoValor As Int64)
        Dim idnuevotrabajo As Decimal
        idnuevotrabajo = op.GuardarPresupuestoInterno(trabajoid, Usuarioid, Tipo, IdAñoValor)
        ShowNotification("Presupuesto Creado", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        Return idnuevotrabajo
    End Function
    Sub PresupuestosInternos(ByVal IdTrabajo As Int64)
        Dim op As New PresupInt
        gvMuestra.DataSource = op.ObtenerPresupuestosInternos(IdTrabajo)
        gvMuestra.DataBind()
    End Sub
    Sub calcular(ByVal IdPresupuesto As Double)
        Dim op As New PresupInt
        Dim WAño As Integer
        Dim WTipo As Integer
        WAño = op.ObtenerAñoDelPresupuesto(IdPresupuesto)
        WTipo = op.ObtenerTipoDePresupuesto(IdPresupuesto)
        op.CalcularDetallePresupuesto(IdPresupuesto, WAño, WTipo, 1)
        DetallePresupuesto(IdPresupuesto)
        CalcularTotalPResupuesto()
    End Sub
    Sub CalcularTotalPResupuesto()
        Dim op As New PresupInt
        Dim Total As Double
        Total = 0
        For i = 0 To gvCostos.Rows.Count - 1
            If IsNumeric(gvCostos.Rows(i).Cells(7).Text) Then Total = Total + gvCostos.Rows(i).Cells(7).Text
        Next
        lblTotalPresupuesto.Visible = True
        lblTotalPresupuesto.Text = "Total Presupuesto: " & FormatCurrency(Total, 0)
        op.ActualizarTotal(hfidpresupuesto.Value, Total)
    End Sub
    Sub EliminarDetallePresupuesto(ByVal Id As Int64, ByVal IdPresupuesto As Int64)
        Dim op As New PresupInt
        op.EliminarDetallePresupuesto(Id, IdPresupuesto)
    End Sub
    Sub DetallePresupuesto(ByVal IdPresupuesto As Int64)
        Dim pr As New PresupInt
        gvCostos.DataSource = Nothing
        gvCostos.DataSource = pr.ObtenerDetallePresupuesto(IdPresupuesto)
        gvCostos.DataBind()


        If hftipopresupuesto.Value = 7 Then
            gvReclutamiento.Visible = True
            gvReclutamiento.DataSource = pr.ObtenerDetallePresupuestoReclutamiento(IdPresupuesto)
            gvReclutamiento.DataBind()
        Else
            gvReclutamiento.Visible = False
        End If


        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Sub CalcularGrilla()
        For Each row As GridViewRow In Me.gvCostos.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim id As Int64 = Me.gvCostos.DataKeys(row.RowIndex).Value
                Dim cant As Int32 = 0
                If row.FindControl("DiasHombre") IsNot Nothing Then
                    If IsNumeric(DirectCast(row.FindControl("DiasHombre"), TextBox).Text) Then
                        cant = DirectCast(row.FindControl("DiasHombre"), TextBox).Text
                    End If

                    Dim op As New PresupInt
                    op.ActualizarDetallePresupuesto(id, cant)
                End If
            End If
        Next
        'calcular(hfidpresupuesto.Value)
    End Sub
    Sub cargarpresupuesto(ByVal Id As Int64)
        Dim op As New PresupInt
        Dim Presupuesto As List(Of CC_PresupuestosInternosGetXId_Result)
        Presupuesto = op.ObtenerPresupuesto(hfidpresupuesto.Value)
        hftipopresupuesto.Value = Presupuesto(0).TipoPresupuesto
        Me.lblObservacion.Text = Presupuesto(0).Descripcion
        Me.TxtProductividad.Text = Presupuesto(0).Productividad
        Me.TxtMuestra.Text = Presupuesto(0).Muestra
        Dim presupint = op.GetPresupuestoInterno(Id)
        txtObservaciones.Text = presupint.Observacion
    End Sub
    Sub CargarMetodologia(ByVal Trabajoid As Int64)
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo = oTrabajo.ObtenerTrabajo(Trabajoid)
        Dim oTecnicas As New TecnicasRecol
        Dim oMetodologias As New MetodologiaOperaciones
        Dim metodologia = oMetodologias.obtenerXId(oeTrabajo.OP_MetodologiaId)
        Dim op As New PresupInt
        lblMetodologia.Text = oTecnicas.ObtenerTecnicaXId(metodologia.TecCodigo).TecNombre & " - " & metodologia.MetNombre
        hfIdMet.Value = metodologia.MetCodigo
    End Sub
    Sub actualizarvalorcontratista(ByVal presupuestoid As Int64, ByVal Valor As Double)
        Dim op As New PresupInt
        op.ActualizarValorContratista(presupuestoid, Valor)
    End Sub
    Sub actualizarvalorindividualcontratista(ByVal presupuestoid As Int64, ByVal Valor As Double, ByVal cargo As Integer)
        Dim op As New PresupInt
        op.ActualizarValoresSupervision(presupuestoid, Valor, cargo)
    End Sub
    Private Sub ddlTipoPresupuesto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoPresupuesto.SelectedIndexChanged
        Session("WTipo") = ddlTipoPresupuesto.SelectedValue
        Me.ddlAño.DataSource = op.LstObtenerAñoValor(Session("WTipo"))
        ddlAño.DataValueField = "Id"
        ddlAño.DataTextField = "ValorCargoAño"
		ddlAño.DataBind()

		ddlAño.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))

		ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
    Private Sub ddlAño_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlAño.SelectedIndexChanged
        Session("WIDAñoValor") = ddlAño.SelectedValue
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

#End Region

    Private Sub btbbuscar_Click(sender As Object, e As EventArgs) Handles btbbuscar.Click
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub gvReclutamiento_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        Dim Muestra As Integer = 0
        Dim TarifaPST As Integer = 0
        Dim TarifaContratista As Integer = 0
        Dim idRecord As Int64 = 0
        Muestra = DirectCast(gvReclutamiento.Rows(e.CommandArgument).FindControl("txtMuestra"), TextBox).Text
        TarifaContratista = DirectCast(gvReclutamiento.Rows(e.CommandArgument).FindControl("txtTarifaContratista"), TextBox).Text
        idRecord = gvReclutamiento.DataKeys(e.CommandArgument).Value
        Dim pr As New PresupInt
        pr.ActualizarPresupuestoReclutamiento(idRecord, Muestra, TarifaPST, TarifaContratista)
        DetallePresupuesto(hfidpresupuesto.Value)
    End Sub

    Protected Sub btnSaveObservaciones_Click(sender As Object, e As EventArgs)
        Dim pr As New PresupInt
        Dim presup = pr.GetPresupuestoInterno(hfidpresupuesto.Value)
        presup.Observacion = txtObservaciones.Text
        pr.SavePresupuestoInterno(presup)

    End Sub
End Class