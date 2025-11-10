Imports CoreProject

Public Class GruposPermisos
    Inherits System.Web.UI.Page

    Sub Limpiar()
        txtNombre.Text = String.Empty
        txtId.Text = String.Empty
    End Sub

    Function Validar() As Boolean
        Dim val As Boolean = True
        If txtNombre.Text = String.Empty Then
            lblResult.Text = "Especifique el nombre"
            val = False
        End If
        If txtId.Text = String.Empty Then
            lblResult.Text = "Especifique el id"
            val = False
        End If
        Return val
    End Function

    Public Function ConsultarGP(ByVal Nombre As String) As List(Of GruposPermisos_Result)
        Dim Data As New US.GrupoPermisos
        Dim Info As List(Of GruposPermisos_Result)
        Try
            Info = Data.ObtenerGrupoPermiso(Nombre)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = ConsultarGP(txtBuscar.Text)
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(90, UsuarioID) = False Then
            Response.Redirect("../TH_TalentoHumano/Default.aspx")
        End If
        If Not IsPostBack Then
            CargarGrid(1)
        End If
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarGrid(1)
    End Sub

    Protected Sub gvDatos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Editar"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    txtId.Text = Id
                    txtNombre.Text = gvDatos.DataKeys(CInt(e.CommandArgument))("GrupoPermisos")
                    datos.Visible = True
                    btnEditar.Visible = True
                    btnGuardar.Visible = False
                    lista.Visible = False
                Case "Eliminar"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Dim US As New US.GrupoPermisos
                    US.EliminarGrupoPermiso(Id)
                    CargarGrid(1)
                Case "Permisos"
                    Dim Id As Int32 = Int32.Parse(gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Response.Redirect("Permisos.aspx?IdGrupoPermiso=" & Id.ToString)
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        datos.Visible = True
        lista.Visible = False
        btnEditar.Visible = False
        btnGuardar.Visible = True
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Validar() = True Then
            Dim US As New US.GrupoPermisos
            US.GuardarGrupoPermiso(txtId.Text, txtNombre.Text)
            lblResult.Text = "GP agregada correctamente"
            Limpiar()
            CargarGrid(1)
            lista.Visible = True
            datos.Visible = False
        End If
    End Sub

    Protected Sub btnEditar_Click(sender As Object, e As EventArgs) Handles btnEditar.Click
        If Validar() = True Then
            Dim US As New US.GrupoPermisos
            US.EditarGrupoPermiso(txtId.Text, txtNombre.Text)
            lblResult.Text = "GP editada correctamente"
            Limpiar()
            CargarGrid(1)
            lista.Visible = True
            datos.Visible = False
        End If
    End Sub

    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        gvDatos.DataSource = Session("ListaCargada")
        gvDatos.DataBind()
    End Sub

End Class