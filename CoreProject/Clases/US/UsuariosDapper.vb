Imports System.Configuration
Imports System.Data.SqlClient
Imports CoreProject.US
Imports Dapper

Namespace Usuarios

    Public Class UsuariosDapper
#Region "Variables Globales"
        Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
#End Region

        Public Function UsuariosAsignadosAlPermiso(permisoId As Integer) As List(Of UsuarioAsignadoAlPermiso)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {Key .PermisoId = permisoId}

                Return db.Query(Of UsuarioAsignadoAlPermiso)("US_UsuariosAsignadosAlPermiso", commandType:=CommandType.StoredProcedure, param:=params).ToList()

            End Using
        End Function
        Public Function UsuariosxUnidadXrol(ByVal UnidadId As Integer, RolId As Integer) As List(Of Usuarios_Result)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {Key .Unidad = UnidadId, .Rol = RolId}

                Return db.Query(Of Usuarios_Result)("US_Usuarios_Get_xUnidadxRol", commandType:=CommandType.StoredProcedure, param:=params).ToList()

            End Using
        End Function
        Public Function UsuariosXRol(RolId As Integer)
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {Key .Rol = RolId}

                Return db.Query(Of Usuarios_Result)("US_Usuarios_Get_xRol", commandType:=CommandType.StoredProcedure, param:=params).ToList()

            End Using
        End Function
        Public Function UsuarioTieneRol(UsuarioId As Long, RolId As Integer) As Boolean
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim params = New With {
                    .RolId = RolId,
                    .UsuarioId = UsuarioId
                }

                Return db.ExecuteScalar(Of Boolean)("US_UsuarioTieneRol", commandType:=CommandType.StoredProcedure, param:=params)

            End Using
        End Function
        Public Class UsuarioAsignadoAlPermiso
            Public Property id As Long
            Public Property Nombres As String
            Public Property Apellidos As String
            Public Property Email As String
            Public ReadOnly Property NombreCompleto() As String
                Get
                    Return Me.Nombres & " " & Me.Apellidos
                End Get
            End Property
        End Class

    End Class
End Namespace
