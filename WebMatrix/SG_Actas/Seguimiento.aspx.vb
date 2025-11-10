Imports CoreProject

Public Class Seguimiento
    Inherits System.Web.UI.Page

    Function Validar() As Boolean
        Dim Val = True
        If txtSeguimiento.Text = String.Empty Then
            lblResult.Text = "Escriba la descripción del seguimiento"
            Val = False
        End If
        Return Val
    End Function

    Sub Limpiar()
        txtSeguimiento.Text = String.Empty
        chkCierra.Checked = False
    End Sub

    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub

    Public Function CargarGridSeguimientoXTarea(ByVal TareaId As Integer)
        Dim _sgActas As New SG.Seguimientos
        Dim list As List(Of SG_Seguimiento_Result)
        list = _sgActas.ConsultarSeguimientosXTarea(TareaId)
        Return list
    End Function

    Public Function ObtenerUsuarios() As List(Of ObtenerUsuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Try
            Return Data.ObtenerUsuarios
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("idTarea") IsNot Nothing Then
                Dim idTarea As Int64 = Int64.Parse(Request.QueryString("idTarea").ToString)
                gvDatos.DataSource = CargarGridSeguimientoXTarea(idTarea)
                gvDatos.DataBind()
                CargarCombo(ddUsuario, "Id", "Nombres", ObtenerUsuarios())
            Else
                Response.Redirect("Actas.aspx")
            End If
        End If
    End Sub

    Protected Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
            Select Case e.CommandName
                Case "Eliminar"
                    Dim PA As New SG.Seguimientos
                    PA.EliminarSeguimiento(Id)
                    gvDatos.DataSource = CargarGridSeguimientoXTarea(Request.QueryString("idTarea"))
                    gvDatos.DataBind()
                Case "Editar"
                    Session("SeguimientoId") = Id
                    txtSeguimiento.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("Seguimiento")
                    ddUsuario.SelectedValue = gvDatos.DataKeys(CInt(e.CommandArgument))("Usuario")
                    chkCierra.Checked = gvDatos.DataKeys(CInt(e.CommandArgument))("Cierra")
                    btnEditar.Visible = True
                    btnGuardar.Visible = False
                    lista_actas.Visible = False
                    gestion_seguimiento.Visible = True
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Validar() = True Then
            Try
                Dim SG As New SG.Seguimientos
                Dim SGT As New SG.Tareas
                SG.GuardarSeguimiento(SG.UltimoId, Request.QueryString("idTarea"), txtSeguimiento.Text, ddUsuario.SelectedValue, chkCierra.Checked, DateTime.Now)
                SGT.EditarEstado(Request.QueryString("idTarea"), 2)
                lblResult.Text = "Seguimiento guardado exitosamente"
                gvDatos.DataSource = CargarGridSeguimientoXTarea(Request.QueryString("idTarea"))
                gvDatos.DataBind()
                lista_actas.Visible = True
                gestion_seguimiento.Visible = False
                Limpiar()
            Catch ex As Exception
                lblResult.Text = ex.Message
            End Try
        End If
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If Validar() = True Then
            Try
                Dim SG As New SG.Seguimientos
                SG.EditarSeguimiento(Session("SeguimientoId"), Request.QueryString("idTarea"), txtSeguimiento.Text, ddUsuario.SelectedValue, chkCierra.Checked, DateTime.Now)
                lblResult.Text = "Seguimiento editado exitosamente"
                gvDatos.DataSource = CargarGridSeguimientoXTarea(Request.QueryString("idTarea"))
                gvDatos.DataBind()
                lista_actas.Visible = True
                gestion_seguimiento.Visible = False
                Limpiar()
            Catch ex As Exception
                lblResult.Text = ex.Message
            End Try
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        btnEditar.Visible = False
        btnGuardar.Visible = True
        lista_actas.Visible = False
        gestion_seguimiento.Visible = True
    End Sub
End Class