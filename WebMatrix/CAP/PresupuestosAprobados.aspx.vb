Imports WebMatrix.Util
Imports CoreProject


Public Class PresupuestosAprobados
    Inherits System.Web.UI.Page

    Dim _ControlCostos As New IQ.ControlCostos()

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim permisos As New Datos.ClsPermisosUsuarios
        If Not permisos.VerificarPermisoUsuario(158, Session("IDUsuario").ToString()) Then
            Response.Redirect("../Home/home.aspx")
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            CargarTecnicas()

            If Session("FiltrosPresupAprobados") IsNot Nothing Then
                txtIdPropuesta.Text = If(CType(Session("FiltrosPresupAprobados"), String).Split(";").GetValue(0) = "0", "", CType(Session("FiltrosPresupAprobados"), String).Split(";").GetValue(0))
                txtIdTrabajo.Text = If(CType(Session("FiltrosPresupAprobados"), String).Split(";").GetValue(1) = "0", "", CType(Session("FiltrosPresupAprobados"), String).Split(";").GetValue(1))
                lstTecnica.SelectedIndex = lstTecnica.Items.IndexOf(lstTecnica.Items.FindByValue(CType(Session("FiltrosPresupAprobados"), String).Split(";").GetValue(2)))
                txtJobBook.Text = If(CType(Session("FiltrosPresupAprobados"), String).Split(";").GetValue(3) = "0", "", CType(Session("FiltrosPresupAprobados"), String).Split(";").GetValue(3))
                CargarPresupuestosAprobados()
            End If

        End If
    End Sub
    Private Sub CargarPresupuestosAprobados()

        gvPresupAprobados.DataSource = _ControlCostos.ObtenerPresupuestosAprobados(If(txtIdPropuesta.Text.Trim = "", 0, CLng(txtIdPropuesta.Text.Trim)), If(txtIdTrabajo.Text.Trim = "", 0, CLng(txtIdTrabajo.Text.Trim)), txtJobBook.Text.Trim, lstTecnica.SelectedValue).Tables(0)
        gvPresupAprobados.DataBind()
        If Request.QueryString("OPT") = 1 Then
            gvPresupAprobados.Columns(12).Visible = True
            gvPresupAprobados.Columns(13).Visible = False
            gvPresupAprobados.Columns(14).Visible = False
        ElseIf Request.QueryString("OPT") = 2 Then
            gvPresupAprobados.Columns(12).Visible = False
            gvPresupAprobados.Columns(13).Visible = True
            gvPresupAprobados.Columns(14).Visible = True
        End If

        upGrilla.Update()
    End Sub
    Protected Sub gvPresupAprobados_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPresupAprobados.PageIndexChanging
        gvPresupAprobados.PageIndex = e.NewPageIndex
        CargarPresupuestosAprobados()
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Dim propuesta As Long
        Dim trabajo As Long
        Dim tecnica As Integer
        Dim jobbook As String
        propuesta = If(txtIdPropuesta.Text = String.Empty, 0, CLng(txtIdPropuesta.Text))
        trabajo = If(txtIdTrabajo.Text = String.Empty, 0, CLng(txtIdTrabajo.Text))
        jobbook = If(txtJobBook.Text = String.Empty, 0, txtJobBook.Text)
        tecnica = CInt(lstTecnica.SelectedValue)
        Session("FiltrosPresupAprobados") = propuesta.ToString() & ";" & trabajo.ToString() & ";" & tecnica.ToString() & ";" & jobbook
        CargarPresupuestosAprobados()
    End Sub

    Private Sub CargarTecnicas()
        lstTecnica.DataSource = _ControlCostos.ObtenerTecnicasCuali()
        lstTecnica.DataTextField = "TecNombre"
        lstTecnica.DataValueField = "TecCodigo"
        lstTecnica.DataBind()

    End Sub

    Protected Sub gvPresupAprobados_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPresupAprobados.RowCommand
        Dim p As New IQ_Parametros
        p.IdPropuesta = CLng(CType(gvPresupAprobados.Rows(e.CommandArgument).FindControl("Label5"), Label).Text)
        p.ParAlternativa = CInt(CType(gvPresupAprobados.Rows(e.CommandArgument).FindControl("Label1"), Label).Text)
        p.MetCodigo = CInt(CType(gvPresupAprobados.Rows(e.CommandArgument).FindControl("Label2"), Label).Text)
        p.ParNacional = CInt(CType(gvPresupAprobados.Rows(e.CommandArgument).FindControl("Label4"), Label).Text)
        Session("_PresupAprobados") = p
        Select Case e.CommandName

            Case "VER"

                Response.Redirect("ControlCostos.aspx?IdPropuesta=" & p.IdPropuesta & "&Alternativa=" & p.ParAlternativa & "&Metodologia=" & p.MetCodigo & "&Nacional=" & p.ParNacional & "&Back=1" & "&o_3453oioioioo_1133=0")
            Case "ACT"
                CargarActividades(p)
                txtNuevoCosto.Text = ""
                txtPorcentaje.Text = ""
                'txtCosto.Text = ""
                lblCostoActual.Text = ""
                lblCostoOperaconActual.Text = ""
                lblMargenOperacionesActual.Text = ""
                lblPorcActual.Text = ""

                lblCostoOperacon.Text = ""
                lblMargenOperaciones.Text = ""
                lblNuevoValor.Text = ""
                lblMargenOperaciones.Text = ""

                lbltecnica.Text = gvPresupAprobados.Rows(e.CommandArgument).Cells(2).Text
                lblmetodologia.Text = gvPresupAprobados.Rows(e.CommandArgument).Cells(4).Text
                lblalternativa.Text = CType(gvPresupAprobados.Rows(e.CommandArgument).FindControl("Label1"), Label).Text
                lblFase.Text = gvPresupAprobados.Rows(e.CommandArgument).Cells(7).Text
                lbljob.Text = gvPresupAprobados.Rows(e.CommandArgument).Cells(8).Text

                LinkButton1_ModalPopupExtender.Show()

            Case "VERP"
                Response.Redirect("ControlCostos.aspx?IdPropuesta=" & p.IdPropuesta & "&Alternativa=" & p.ParAlternativa & "&Metodologia=" & p.MetCodigo & "&Nacional=" & p.ParNacional & "&Back=1" & "&o_3453oioioioo_1133=0")
        End Select
    End Sub


    Private Sub CargarActividades(p As IQ_Parametros)
        lstActividad.DataSource = _ControlCostos.ObtenerActividadesPresupAprobados(p)
        lstActividad.DataValueField = "ActID"
        lstActividad.DataTextField = "ActNombre"
        lstActividad.DataBind()
    End Sub
    Protected Sub gvPresupAprobados_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPresupAprobados.RowCreated
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            CType(e.Row.Cells(9).FindControl("ImageButton1"), ImageButton).CommandArgument = e.Row.RowIndex.ToString()

        End If
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        LinkButton1_ModalPopupExtender.Hide()
    End Sub

    Protected Sub lstActividad_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstActividad.SelectedIndexChanged
        If lstActividad.SelectedIndex > 0 Then
            Dim act As New IQ_Actividades
            act.ID = CInt(lstActividad.SelectedValue)
            act.ActNombre = lstActividad.SelectedItem.Text
            Dim valoresActuales As IQ_AjusteCostoOperacionesObtenerDatos_Result
            valoresActuales = _ControlCostos.ObtenerValoresActualesAjustar(CType(Session("_PresupAprobados"), IQ_Parametros), CInt(lstActividad.SelectedValue))
            lblCostoActual.Text = valoresActuales.COLUMN1.Value.ToString("C0")
            lblCostoOperaconActual.Text = valoresActuales.COLUMN2.Value.ToString("C0")
            lblMargenOperacionesActual.Text = valoresActuales.COLUMN3.Value.ToString("C0")
            lblPorcActual.Text = valoresActuales.COLUMN4.Value.ToString("P2")
            'txtCosto.Text = _ControlCostos.ObtenerValorActividad(CType(Session("_PresupAprobados"), IQ_Parametros), act).ToString("N0")
        End If

    End Sub

    Protected Sub btnAplicar_Click(sender As Object, e As EventArgs) Handles btnAplicar.Click
        'If lstActividad.SelectedIndex > 0 Then

        Dim ajuste As IQ_AjusteCostoOperaciones_Result
        Dim afecta As Boolean
        Dim NuevoValor, Porcentaje, PorAfectaOperaciones As Decimal
        afecta = True
        NuevoValor = If(txtNuevoCosto.Text.Trim = String.Empty, 0, CDec(txtNuevoCosto.Text))
        Porcentaje = If(txtPorcentaje.Text.Trim = String.Empty, 0, CDec(txtPorcentaje.Text))
        PorAfectaOperaciones = If(txtPorcOperaciones.Text.Trim = String.Empty, 0, CDec(txtPorcOperaciones.Text))

        ajuste = _ControlCostos.ObtenerAjustesCostosOperaciones(CType(Session("_PresupAprobados"), IQ_Parametros), NuevoValor, Porcentaje, afecta, CInt(lstActividad.SelectedValue), PorAfectaOperaciones)
        lblNuevoValor.Text = ajuste.Column1.value.ToString("C0")
        lblCostoOperacon.Text = ajuste.Column2.Value.ToString("C0")
        lblMargenOperaciones.Text = ajuste.Column3.Value.ToString("C0")
        lblPorcentajeOperaciones.Text = ajuste.Column4.Value.ToString("P2")

        'Else
        '    ShowNotification("Seleccione la activdad", WebMatrix.ShowNotifications.ErrorNotification)
        'End If

    End Sub
End Class