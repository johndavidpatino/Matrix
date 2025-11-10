Imports CoreProject
Imports WebMatrix.Util

Public Class EliminarCargueProduccion
    Inherits System.Web.UI.Page
    Dim Op As New ProcesosInternos

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Cargues(Nothing, Nothing, Nothing, Nothing)
            UsuariosCargues(Nothing)
        End If
    End Sub
    Sub Cargues(ByVal Id As Int64?, ByVal FechaIni As Date?, ByVal FechaFin As Date?, ByVal Usuario As Int64?)
        Dim OPD As New OP_CuantiDapper
        GvCargues.DataSource = OPD.CarguesProduccionGet(Id, FechaIni, FechaFin, Usuario)
        GvCargues.DataBind()
    End Sub
    Sub EliminarRegistros(ByVal Id As Int64)
        Op.EliminarCargueProduccion(Id)
    End Sub
    Private Sub GvCargues_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GvCargues.RowCommand
        If e.CommandName = "Eliminar" Then
            EliminarRegistros(Int64.Parse(Me.GvCargues.DataKeys(CInt(e.CommandArgument))("Id")))
            ShowNotification("Registros Eliminados", ShowNotifications.InfoNotification)
            Cargues(Nothing, Nothing, Nothing, Nothing)
        End If
    End Sub

    Private Sub GvCargues_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvCargues.PageIndexChanging
        GvCargues.PageIndex = e.NewPageIndex
        Cargues(Nothing, Nothing, Nothing, Nothing)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub UsuariosCargues(ByVal Usuario As Int64?)
        ddlUsuario.DataSource = Op.UsuariosCarguesProduccion(Usuario)
        ddlUsuario.DataValueField = "Id"
        ddlUsuario.DataTextField = "Usuario"
        ddlUsuario.DataBind()
        ddlUsuario.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        Cargues(Nothing, txtFechaInicio.Text, txtFechaFinalizacion.Text, Nothing)
    End Sub

    Private Sub ddlUsuario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUsuario.SelectedIndexChanged
        If txtFechaFinalizacion.Text = "" And txtFechaInicio.Text = "" Then
            Cargues(Nothing, Nothing, Nothing, ddlUsuario.SelectedValue)
        Else
            Cargues(Nothing, txtFechaInicio.Text, txtFechaFinalizacion.Text, ddlUsuario.SelectedValue)
        End If

    End Sub
End Class