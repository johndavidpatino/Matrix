Imports CoreProject
Imports WebMatrix.Util

Public Class ProductividadRevisadaPMO
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(100, UsuarioID) = False Then
            Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        End If
        If Not IsPostBack Then
            CargarTrabajos()
            Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
            If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
            Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
            lblIni.Text = inicioCorteFecha.ToShortDateString()
            lblFin.Text = finCorteFecha.ToShortDateString()
        End If


    End Sub



    Sub EnviarEmailSolicitud(ByVal TrabajoId As Int64)
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/SolicitudPresupuesto.aspx?TrabajoId=" & TrabajoId)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Protected Sub ddlTrabajoSeleccionado_SelectedIndexChanged(sender As Object, e As EventArgs)
        CargarGrid()
    End Sub

    Sub CargarTrabajos()
        Dim op As New OP_CuantiDapper
        Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
        If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
        Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
        ddlTrabajoSeleccionado.DataSource = op.CuantiProdProductividadTrabajos_Get(inicioCorteFecha, finCorteFecha, Nothing, Session("IDUsuario").ToString, Nothing, Nothing, Nothing, True, True, Nothing)
        ddlTrabajoSeleccionado.DataTextField = "NombreTrabajo"
        ddlTrabajoSeleccionado.DataValueField = "TrabajoId"
        ddlTrabajoSeleccionado.DataBind()
        ddlTrabajoSeleccionado.Items.Insert(0, New ListItem With {.Text = "Seleccione un trabajo", .Value = 0})
    End Sub

    Sub CargarGrid()
        Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
        If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
        Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
        pnlResultados.Visible = True
        Dim op As New OP_CuantiDapper
        gvDataSearch.DataSource = op.CuantiProdProductividad_Get(inicioCorteFecha, finCorteFecha, Nothing, Session("IDUsuario").ToString, ddlTrabajoSeleccionado.SelectedValue, Nothing, Nothing, True, True, Nothing)
        gvDataSearch.DataBind()
    End Sub

    Protected Sub btnRechazar_Click(sender As Object, e As EventArgs)
        Dim op As New OP_CuantiDapper
        op.CuantiPlanillasTrabajosRemove(True, Nothing, Nothing, Nothing, ddlTrabajoSeleccionado.SelectedValue, Session("IDUsuario").ToString)
        CargarTrabajos()
        pnlResultados.Visible = False
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs)
        For Each row As GridViewRow In gvDataSearch.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If Not (IsNumeric(DirectCast(Me.gvDataSearch.Rows(row.RowIndex).FindControl("txtCantidad"), TextBox).Text)) Then
                    ShowNotification("Las cantidades deben ser numéricas", ShowNotifications.ErrorNotification)
                    Exit Sub
                End If
                If Int(DirectCast(Me.gvDataSearch.Rows(row.RowIndex).FindControl("txtCantidad"), TextBox).Text) > Int(row.Cells(4).Text) Then
                    ShowNotification("La cantidad autorizada no puede ser mayor a la original", ShowNotifications.ErrorNotification)
                End If
            End If
        Next
        Dim op As New TrabajoOPCuanti
        For Each row As GridViewRow In gvDataSearch.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim ent = op.ObtenerCCProduccionPST(gvDataSearch.DataKeys(row.RowIndex).Value)
                ent.FechaRevisaCoordinador = Date.UtcNow.AddHours(-5)
                ent.CoordinadorRevisa = Session("IDUsuario").ToString
                ent.CantidadCoordinador = DirectCast(Me.gvDataSearch.Rows(row.RowIndex).FindControl("txtCantidad"), TextBox).Text
            End If
        Next
        op.SaveChangesContext()
        CargarTrabajos()
        pnlResultados.Visible = False
    End Sub
End Class