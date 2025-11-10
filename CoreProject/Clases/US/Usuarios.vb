Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient
'Imports CoreProject.US_Model


Namespace US
    Public Class Usuarios
#Region "Variables Globales"
        Private oMatrixContext As US_Entities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New US_Entities
        End Sub
#End Region
        Public Function TodoslosUsuarios() As List(Of String)
            Dim PermisoUsuario = New US_Entities
            Dim arr As New List(Of String)
            Try
                Try
                    Dim List As ObjectResult(Of ConsultarUsuarios_Result) = PermisoUsuario.ConsultarUsuarios
                    For Each dr As ConsultarUsuarios_Result In List
                        arr.Add(dr.Nombre)
                    Next
                    Return arr
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function VerificarUsuario(ByVal usr As String, ByVal permiso As String) As Boolean
            Try
                Dim PermisoUsuario = New US_Entities
                PermisoUsuario.VerificarPermisoUsuario(permiso, usr)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function DevolverIdXNombreUsuario(ByVal nombreusr As String) As Int64
            Try
                If oMatrixContext.US_Usuarios.Where(Function(x) x.Usuario = nombreusr).ToList.Count = 0 Then
                    Return -1
                Else
                    Return oMatrixContext.US_Usuarios.Where(Function(x) x.Usuario = nombreusr).FirstOrDefault.id
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function DevolverUsuarioxUsuario(ByVal nombreusr As String) As US_Usuarios
            Return oMatrixContext.US_Usuarios.Where(Function(x) x.Usuario = nombreusr).FirstOrDefault
        End Function

        Public Function VerificarLogin(ByVal usr As String, ByVal password As String) As Long
            Try
                'Dim loginverif = New US_Entities
                'Dim val As Long = loginverif.US_GetLogin(usr, password)(0)
                'Return val

                If (oMatrixContext.US_Usuarios.Where(Function(x) x.Usuario = usr AndAlso x.Password = password)).ToList.Count > 0 Then
                    Return (oMatrixContext.US_Usuarios.Where(Function(x) x.Usuario = usr AndAlso x.Password = password)).ToList.Item(0).id
                Else
                    Return -1
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function VerificarLoginXId(ByVal usrid As Int64, ByVal password As String) As Long
            Try
                'Dim loginverif = New US_Entities
                'Dim val As Long = loginverif.US_GetLogin(usr, password)(0)
                'Return val

                If oMatrixContext.US_Usuarios.Where(Function(x) x.id = usrid AndAlso x.Password = password).ToList.Count > 0 Then
                    Return (oMatrixContext.US_Usuarios.Where(Function(x) x.id = usrid AndAlso x.Password = password)).ToList.Item(0).id
                Else
                    Return -1
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Function UsuariosxRol(ByVal Rolid As Int32) As List(Of Usuarios_Result)
            Return oMatrixContext.US_Usuarios_Get_xRol(Rolid).ToList
        End Function

        Public Function UsuariosxUnidad(ByVal Unidadid As Int32) As List(Of Usuarios_Result)
            Return oMatrixContext.US_Usuarios_Get_xUnidad(Unidadid).ToList
        End Function

        Public Function PasswordUsuario(ByVal idUsuario As Int64) As String
            Return oMatrixContext.US_Usuarios.Where(Function(x) x.id = idUsuario).FirstOrDefault.Password
        End Function

        Public Function UsuarioGet(ByVal UserId As Int64) As US_Usuarios
            Return oMatrixContext.US_Usuarios.Where(Function(x) x.id = UserId).FirstOrDefault
        End Function

        Public Function UsuarioCorreoPresupuestos(ByVal TrabajoId As Int64) As List(Of US_CorreosPresupuestos_Result)
            Return oMatrixContext.US_CorreosPresupuestos(TrabajoId).ToList
        End Function

        Public Function UsuariosxGrupoUnidadXrol(ByVal GrupoUnidadid As Int32, Rolid As Int32) As List(Of Usuarios_Result)
            Return oMatrixContext.US_Usuarios_Get_xGrupoUnidadxRol(Rolid, GrupoUnidadid).ToList
        End Function

        Public Function UsuariosXrolXPropuesta(Rolid As Int32, ByVal PropuestaId As Int64) As List(Of Usuarios_Result)
            Return oMatrixContext.US_Usuarios_Get_xRolxPropuesta(Rolid, PropuestaId).ToList
        End Function

        Public Function UsuariosxUnidadXrol(ByVal Unidadid As Int32, Rolid As Int32) As List(Of Usuarios_Result)
            Return oMatrixContext.US_Usuarios_Get_xUnidadxRol(Rolid, Unidadid).ToList
        End Function
        Public Sub actualizarpassword(ByVal password As String, ByVal idUsuario As Int64)
            Dim usuario As US_Usuarios
            usuario = oMatrixContext.US_Usuarios.Where(Function(x) x.id = idUsuario).FirstOrDefault
            usuario.Password = password
            oMatrixContext.SaveChanges()
        End Sub

        Public Function obtenerUsuarioXId(ByVal id As Int64) As US_Usuarios
            Return oMatrixContext.US_Usuarios.Where(Function(x) x.id = id).FirstOrDefault
        End Function

        Public Function obtener(id As Nullable(Of Long), usuario As String, nombres As String, apellidos As String, email As String, activo As Nullable(Of Boolean)) As List(Of US_UsuariosAnyParameter_Get_Result)
            Return oMatrixContext.US_UsuariosAnyParameter_Get(id, usuario, nombres, apellidos, email, activo).ToList
        End Function

    End Class
End Namespace
