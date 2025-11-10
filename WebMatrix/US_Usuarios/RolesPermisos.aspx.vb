Imports CoreProject

Public Class RolesPermisos
    Inherits System.Web.UI.Page

    Public Function ObtenerRol() As List(Of RolesCombo_Result)
        Dim Data As New US.Roles
        Try
            Return Data.ObtenerRolCombo
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

    Public Function Consultar(ByVal UsuarioId As String) As List(Of RolesPermisos_Result)
        Dim Data As New US.RolesPermisos
        Dim Info As List(Of RolesPermisos_Result)
        Try
            Info = Data.ObtenerRolesPermisos(UsuarioId)
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub CargarGrid(ByVal Opcion As Integer)
        Select Case Opcion
            Case 1
                Session("ListaCargada") = Consultar(Request.QueryString("IdPermiso"))
                gvDatos.DataSource = Session("ListaCargada")
                gvDatos.DataBind()
        End Select
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("IdPermiso") IsNot Nothing Then
                Dim IdPermiso As Int64 = Int64.Parse(Request.QueryString("IdPermiso").ToString)
                CargarGrid(1)
                CargarCombo(ddlRol, "Id", "Rol", ObtenerRol())
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
            Dim US As New US.RolesPermisos
            US.GuardarRolesPermisos(Request.QueryString("IdPermiso"), ddlRol.SelectedValue)
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

End Class