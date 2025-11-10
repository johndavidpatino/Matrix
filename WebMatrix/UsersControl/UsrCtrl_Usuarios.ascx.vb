Imports CoreProject

Partial Class UsrCtrl_Usuarios
    Inherits System.Web.UI.UserControl
    'variable privada que contiene la info de los usuarios
    Private _user As List(Of Usuarios_Result)

    'propiedad publica la cual devolvera la lista de usuarios
    Public Property Usuario As List(Of Usuarios_Result)
        Get
            Return Me._user
        End Get
        Set(value As List(Of Usuarios_Result))
            Me._user = value
        End Set
    End Property


#Region "Funciones"
    Public Function ConsultarUsuariosXNombre(ByVal Nombre As String) As List(Of Usuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Dim Info As List(Of Usuarios_Result)
        Try
            Info = Data.ConsultarUsuariosXNombre(Nombre)
            If Info.Count > 0 Then btnObtenerValores.Visible = True
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarUsuariosXUnidades(ByVal UnidadId As Integer) As List(Of Usuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Dim Info As List(Of Usuarios_Result)
        Try
            Info = Data.ConsultarUsuariosXUnidades(UnidadId)
            If Info.Count > 0 Then btnObtenerValores.Visible = True
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarUsuariosXGrupoUnidad(ByVal GrupoUnidadId As Integer) As List(Of Usuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Dim Info As List(Of Usuarios_Result)
        Try
            Info = Data.ConsultarUsuariosXGrupoUnidad(GrupoUnidadId)
            If Info.Count > 0 Then btnObtenerValores.Visible = True
            Return Info
            Return Data.ConsultarUsuariosXGrupoUnidad(GrupoUnidadId)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ConsultarUsuariosXRolesUsuarios(ByVal RolId As Integer) As List(Of Usuarios_Result)
        Dim Data As New Datos.ClsPermisosUsuarios
        Dim Info As List(Of Usuarios_Result)
        Try
            Info = Data.ConsultarUsuariosXRolesUsuarios(RolId)
            If Info.Count > 0 Then btnObtenerValores.Visible = True
            Return Info
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Procedimientos"
    Sub CargarGrid(ByVal filtro As Integer, ByVal valor As String)
        Select Case filtro
            Case 1
                'filtro por nombre
                Me.Usuario = ConsultarUsuariosXNombre(valor)
            Case 2
                'filtro por unidades
                Me.Usuario = ConsultarUsuariosXUnidades(CInt(valor))
            Case 3
                'filtro por grupo unidad
                Me.Usuario = ConsultarUsuariosXGrupoUnidad(CInt(valor))
            Case 4
                'filtro por roles
                Me.Usuario = ConsultarUsuariosXRolesUsuarios(CInt(valor))
        End Select
        Try
            gvUsuarios.DataSource = Me.Usuario
            gvUsuarios.DataBind()
        Catch ex As Exception
            gvUsuarios.DataSource = Nothing
            gvUsuarios.DataBind()
        End Try
    End Sub
#End Region

    ' esto no funciona en el user control :(
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        ' Create array of movies
        Dim Data As New Datos.ClsPermisosUsuarios
        'Dim loaddata() As String = Data.TodoslosUsuarios.ToArray
        Dim loaddata() As String
        Dim movies() As String = loaddata
        count = loaddata.Count
        ' Return matching movies
        Return (
            From m In movies
            Where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase)
            Select m).Take(count).ToArray()
    End Function

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

    End Sub


    Protected Sub btnObtenerValores_Click(sender As Object, e As System.EventArgs) Handles btnObtenerValores.Click
        Dim lista As New List(Of Usuarios_Result)
        Dim total As Integer = 0
        For Each row As GridViewRow In gvUsuarios.Rows
            Dim usuario As Usuarios_Result
            Dim check As CheckBox = TryCast(row.FindControl("chkSeleccion"), CheckBox)
            If check.Checked Then
                total = total + 1
                usuario = New Usuarios_Result
                usuario.id = gvUsuarios.DataKeys(row.RowIndex).Values("id")
                usuario.Nombres = gvUsuarios.DataKeys(row.RowIndex).Values("Nombres")
                usuario.Apellidos = gvUsuarios.DataKeys(row.RowIndex).Values("Apellidos")
                usuario.Activo = gvUsuarios.DataKeys(row.RowIndex).Values("Activo")
                usuario.Email = gvUsuarios.DataKeys(row.RowIndex).Values("Email")
                usuario.Usuario = gvUsuarios.DataKeys(row.RowIndex).Values("Usuario")
                lista.Add(usuario)
            End If
        Next
        Session("Usuarios") = lista
        If total = 0 Then
            lblRes.Text = "Por favor seleccione un Item"
        Else
            If total = 1 Then
                lblRes.Text = "ha seleccionado " & total & " Item"
            Else
                lblRes.Text = "ha seleccionado " & total & " Items"
            End If
        End If
    End Sub

End Class