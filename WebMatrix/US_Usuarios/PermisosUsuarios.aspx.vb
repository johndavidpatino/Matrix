Imports CoreProject

Public Class PermisosUsuarios
    Inherits System.Web.UI.Page

    Public Function ObtenerGrupoPermiso() As List(Of GruposPermisosCombo_Result)
        Dim Data As New US.GrupoPermisos
        Try
            Return Data.ObtenerGrupoPermisoCombo
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerPermiso(ByVal Grupo As Integer) As List(Of PermisosCombo_Result)
        Dim Data As New US.Permisos
        Try
            Return Data.ObtenerPermisoCombo(Grupo)
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

    Public Function Consultar(ByVal UsuarioId As String) As List(Of PermisosUsuarios_Result)
        Dim Data As New US.PermisosUsuarios
        Dim Info As List(Of PermisosUsuarios_Result)
        Try
            Info = Data.ObtenerPermisosUsuario(UsuarioId)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = Consultar(Request.QueryString("IdUsuario"))
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("IdUsuario") IsNot Nothing Then
                Dim IdUsuario As Int64 = Int64.Parse(Request.QueryString("IdUsuario").ToString)
                CargarGrid(1)
                CargarCombo(ddlGrupoPermiso, "Id", "GrupoPermisos", ObtenerGrupoPermiso())
                CargarCombo(ddlPermiso, "Id", "Permiso", ObtenerPermiso(ddlGrupoPermiso.SelectedValue))
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
            Dim US As New US.PermisosUsuarios
            US.GuardarPermisosUsuario(Request.QueryString("IdUsuario"), ddlPermiso.SelectedValue)
            lblResult.Text = "Permiso agregado correctamente"
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

    Protected Sub ddlGrupoPermiso_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlGrupoPermiso.SelectedIndexChanged
        CargarCombo(ddlPermiso, "Id", "Permiso", ObtenerPermiso(ddlGrupoPermiso.SelectedValue))
    End Sub
End Class