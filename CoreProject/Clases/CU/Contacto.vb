
'Imports CoreProject.CU_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Contacto
#Region "Variables Globales"
    Private oMatrixContext As CU_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CU_Entities
    End Sub
#End Region

#Region "Obtener"
    Public Function DevolverTodos() As List(Of CU_Contactos_Get_Result)
        Try
            Return CU_Contactos_Get(Nothing, Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As CU_Contactos_Get_Result
        Try
            Dim oResult As List(Of CU_Contactos_Get_Result)
            oResult = CU_Contactos_Get(ID, Nothing, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxClienteID(ByVal ClienteID As Int64) As List(Of CU_Contactos_Get_Result)
        Try
            Return CU_Contactos_Get(Nothing, Nothing, ClienteID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function CU_Contactos_Get(ByVal id As Int64, ByVal Nombre As String, ByVal ClienteID As Int64) As List(Of CU_Contactos_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_Contactos_Get_Result) = oMatrixContext.CU_Contactos_Get(id, Nombre, ClienteID)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try

    End Function

#End Region
#Region "Guardar"
    Public Function Guardar(ByVal ID As Int64?,
                            ByVal Nombre As String,
                            ByVal Telefono As String,
                            ByVal Celular As String,
                            ByVal Email As String,
                            ByVal Cargo As String,
                            ByVal Activo As Boolean,
                            ByVal ClienteId As Int64?) As Decimal
        Try
            Dim ContactoID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.CU_Contactos_Edit(ID, Nombre, Telefono, Celular, Email, Cargo, Activo, ClienteId)
                ContactoID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.CU_Contactos_Add(Nombre, Telefono, Celular, Email, Cargo, Activo, ClienteId)
                ContactoID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return ContactoID
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
End Class
