Imports CoreProject

Public Class RolesUsuarios
    Inherits System.Web.UI.Page

    Public Function ObtenerUsuarios() As List(Of ObtenerUsuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Try
            Return Data.ObtenerUsuarios
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub

    Sub Limpiar()

    End Sub

    Function Validar() As Boolean
        Dim val As Boolean = True
        Return val
    End Function

    Public Function ConsultarRolPorUsuario(ByVal UsuarioId As String) As List(Of RolesUsuarios_Result)
        Dim Data As New US.RolesUsuarios
        Dim Info As List(Of RolesUsuarios_Result)
        Try
            Info = Data.ObtenerRolesUsuarios(Nothing, UsuarioId)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarUsuarioPorRol(ByVal RolId As String) As List(Of RolesUsuarios_Result)
        Dim Data As New US.RolesUsuarios
        Dim Info As List(Of RolesUsuarios_Result)
        Try
            Info = Data.ObtenerRolesUsuarios(RolId, Nothing)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = ConsultarUsuarioPorRol(Request.QueryString("IdRol"))
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("IdRol") IsNot Nothing Then
                Dim IdRol As Int64 = Int64.Parse(Request.QueryString("IdRol").ToString)
                CargarGrid(1)
                CargarCombo(ddlUsuarios, "Id", "Nombres", ObtenerUsuarios())
            Else
                Response.Redirect("Usuarios.aspx")
            End If
        End If
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        datos.Visible = True
        lista.Visible = False
        btnGuardar.Visible = True
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        If Validar() = True Then
            Dim US As New US.RolesUsuarios
            US.GuardarRolesUsuarios(ddlUsuarios.SelectedValue, Request.QueryString("IdRol"))
            lblResult.Text = "Usuario agregado correctamente"
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