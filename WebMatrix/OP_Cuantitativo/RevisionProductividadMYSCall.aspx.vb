Imports CoreProject
Imports WebMatrix.Util

Public Class RevisionProductividadMYSCall
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(157, UsuarioID) = False Then
            Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        End If
        If Not IsPostBack Then
            CargarTrabajos()
            Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
            If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
            Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
            lblIni.Text = inicioCorteFecha.ToShortDateString()
            lblFin.Text = finCorteFecha.ToShortDateString()
            CargarStatus()
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
        ddlTrabajoSeleccionado.DataSource = op.CuantiProdProductividadTrabajos_Get(inicioCorteFecha, finCorteFecha, Nothing, Nothing, Nothing, 1, Nothing, False, Nothing, Nothing)
        ddlTrabajoSeleccionado.DataTextField = "NombreTrabajo"
        ddlTrabajoSeleccionado.DataValueField = "TrabajoId"
        ddlTrabajoSeleccionado.DataBind()
        ddlTrabajoSeleccionado.Items.Insert(0, New ListItem With {.Text = "Seleccione un trabajo", .Value = 0})
    End Sub

    Sub CargarStatus()
        Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
        If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
        Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
        Dim op As New OP_CuantiDapper
        gvEstatus.DataSource = op.CuantiProdProductividadStatus_Get(inicioCorteFecha, finCorteFecha, Nothing, Nothing, Nothing, 1, Nothing, False, Nothing)
        gvEstatus.DataBind()
    End Sub

    Sub CargarGrid()
        Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
        If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
        Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
        pnlResultados.Visible = True
        Dim op As New OP_CuantiDapper
        gvDataSearch.DataSource = op.CuantiProdProductividad_Get(inicioCorteFecha, finCorteFecha, Nothing, Nothing, ddlTrabajoSeleccionado.SelectedValue, 1, Nothing, False, Nothing, Nothing)
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
                    Exit Sub
                End If
            End If
        Next
        Dim op As New TrabajoOPCuanti
        For Each row As GridViewRow In gvDataSearch.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Dim ent = op.ObtenerCCProduccionPST(gvDataSearch.DataKeys(row.RowIndex).Value)
                ent.FechaRevisaJefe = Date.UtcNow.AddHours(-5)
                ent.JefeRevisa = Session("IDUsuario").ToString
                ent.CantidadJefe = DirectCast(Me.gvDataSearch.Rows(row.RowIndex).FindControl("txtCantidad"), TextBox).Text
                ent.ObservacionesJefe = DirectCast(Me.gvDataSearch.Rows(row.RowIndex).FindControl("txtObservaciones"), TextBox).Text
            End If
        Next
        op.SaveChangesContext()
        CargarTrabajos()
        pnlResultados.Visible = False
    End Sub
End Class