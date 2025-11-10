
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Cliente

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
    Public Function DevolverTodos() As List(Of CU_Cliente_Get_Result)
        Try
            Return CU_Cliente_Get(Nothing, Nothing, Nothing, Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxID(ByVal ID As Int64) As CU_Cliente_Get_Result
        Try
            Dim oResult As List(Of CU_Cliente_Get_Result)
            oResult = CU_Cliente_Get(ID, Nothing, Nothing, Nothing, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function CU_Cliente_Get(ByVal ID As Int64?, ByVal Nit As Decimal?, ByVal RazonSocial As String, ByVal Ciudad As Int32?, ByVal SectorID As String) As List(Of CU_Cliente_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_Cliente_Get_Result) = oMatrixContext.CU_Cliente_Get(ID, Nit, RazonSocial, Ciudad, SectorID)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal Nit As Decimal?, ByVal GrupoEconomico As String, ByVal RazonSocial As String,
                             ByVal Ciudad As Int32?, ByVal Apodo As String, ByVal TipoCliente As Int32?,
                             ByVal Direccion As String, ByVal Telefono As String, ByVal SectorID As String, ByVal Anticipo As Int16, ByVal SAldo As Int16, Plazo As Int16) As Decimal
        Try
            Dim ClienteID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.CU_Cliente_Edit(ID, Nit, GrupoEconomico, RazonSocial, Ciudad, Apodo, TipoCliente, Direccion, Telefono, SectorID, Anticipo, SAldo, Plazo)
                ClienteID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.CU_Cliente_Add(Nit, GrupoEconomico, RazonSocial, Ciudad, Apodo, TipoCliente, Direccion, Telefono, SectorID, Anticipo, SAldo, Plazo)
                ClienteID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return ClienteID
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
