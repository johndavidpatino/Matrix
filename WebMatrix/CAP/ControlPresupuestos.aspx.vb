Imports CoreProject

Public Class IngresarCostosAutorizados
    Inherits System.Web.UI.Page
    Dim _costos As New IQ.ControlCostos

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                CargarUnidades()
                CargarTecnicas()
                CargarMetodologias(CInt(lstTecnicas.SelectedValue))
                CargarPresupuestos(CInt(lstUnidades.SelectedValue), txtJobBook.Text, CInt(lstTecnicas.SelectedValue), CInt(lstMetodologias.SelectedValue))

            End If

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        CargarPresupuestos(CInt(lstUnidades.SelectedValue), txtJobBook.Text, CInt(lstTecnicas.SelectedValue), CInt(lstMetodologias.SelectedValue))
        upGrilla.Update()
    End Sub
    Private Sub CargarPresupuestos(ByVal unidad As Integer, ByVal jb As String, ByVal tec As Integer, ByVal met As Integer)

        gvPresupuestos.DataSource = _costos.ObtenerPresupuestos(unidad, jb, tec, met).Tables(0)
        gvPresupuestos.DataBind()
        upGrilla.Update()

    End Sub

    Private Sub CargarTecnicas()
        lstTecnicas.DataSource = _costos.ObtenerTecnicas()
        lstTecnicas.DataTextField = "TecNombre"
        lstTecnicas.DataValueField = "TecCodigo"
        lstTecnicas.DataBind()

    End Sub

    Private Sub CargarMetodologias(ByVal Tec As Integer)
        lstMetodologias.DataSource = _costos.ObtenerMetodolgias(Tec)
        lstMetodologias.DataTextField = "MetNombre"
        lstMetodologias.DataValueField = "MetCodigo"
        lstMetodologias.DataBind()
    End Sub


    Private Sub CargarUnidades()
        lstUnidades.DataSource = _costos.ObtenerUnidades()
        lstUnidades.DataTextField = "GrupoUnidad"
        lstUnidades.DataValueField = "CodContable"
        lstUnidades.DataBind()
    End Sub

    Protected Sub gvPresupuestos_PageIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Protected Sub gvPresupuestos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPresupuestos.PageIndexChanging
        gvPresupuestos.PageIndex = e.NewPageIndex
        CargarPresupuestos(CInt(lstUnidades.SelectedValue), txtJobBook.Text, CInt(lstTecnicas.SelectedValue), CInt(lstMetodologias.SelectedValue))

    End Sub



    Protected Sub gvPresupuestos_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPresupuestos.RowCreated

        If (e.Row.RowType = DataControlRowType.DataRow) Then
            CType(e.Row.Cells(10).FindControl("Button1"), Button).CommandArgument = e.Row.RowIndex.ToString()
            'e.Row.Cells(5).Visible = False
            'e.Row.Cells(7).Visible = False
        End If


    End Sub

    Protected Sub gvPresupuestos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPresupuestos.RowCommand

        Try
            Select Case e.CommandName
                Case "ADD"
                    Dim p As New IQ_Parametros
                    Dim Nombre, JobBook As String
                    p.IdPropuesta = CInt(gvPresupuestos.Rows(e.CommandArgument).Cells(0).Text)
                    p.ParAlternativa = CInt(gvPresupuestos.Rows(e.CommandArgument).Cells(2).Text)
                    p.MetCodigo = CInt(CType(gvPresupuestos.Rows(e.CommandArgument).FindControl("Label2"), Label).Text)
                    p.ParNacional = CInt(CType(gvPresupuestos.Rows(e.CommandArgument).FindControl("Label3"), Label).Text)
                    Nombre = CStr(gvPresupuestos.Rows(e.CommandArgument).Cells(1).Text)
                    JobBook = CStr(gvPresupuestos.Rows(e.CommandArgument).Cells(9).Text)
                    Response.Redirect("CostosAutorizados.aspx?IdPropuesta=" & p.IdPropuesta & "&Alternativa=" & p.ParAlternativa & "&Metodologia=" & p.MetCodigo & "&Fase=" & p.ParNacional & "&Nombre=" & Nombre & "&JobBook=" & JobBook)

            End Select
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lstTecnicas_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstTecnicas.SelectedIndexChanged
        If lstTecnicas.SelectedIndex > -1 Then

            CargarMetodologias(CInt(lstTecnicas.SelectedValue))
        End If
    End Sub
End Class