
'Imports CoreProject.CU_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Propuesta
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

    Public Function ObtenerXIdGerenteCuentas(ByVal idGerenteCuentas As Int64) As List(Of CU_Propuestas_Get_Result)

        Return CU_Propuesta_Get(Nothing, Nothing, idGerenteCuentas, Nothing).ToList

    End Function

    Public Function ObtenerXIdGerenteCuentasXIdEstado(ByVal idGerenteCuentas As Int64, ByVal idEstadoPropuesta As Int16) As List(Of CU_Propuestas_Get_Result)

        Return CU_Propuesta_Get(Nothing, Nothing, idGerenteCuentas, idEstadoPropuesta).ToList

    End Function

    Public Function ObtenerXGerenteXPropuesta(ByVal idPropuesta As Int64?, ByVal idGerenteCuentas As Int64?, ByVal idEstado As Short?) As List(Of CU_Propuestas_Get_Result)
        Return CU_Propuesta_Get(idPropuesta, Nothing, idGerenteCuentas, idEstado).ToList
    End Function

    Public Function DevolverTodos() As List(Of CU_Propuestas_Get_Result)
        Try
            CU_Propuesta_Get(Nothing, Nothing, Nothing, Nothing)
            Return CU_Propuesta_Get(Nothing, Nothing, Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxBriefID(ByVal BriefID As Int64) As List(Of CU_Propuestas_Get_Result)
        Try
            Return CU_Propuesta_Get(Nothing, BriefID, Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxID(ByVal ID As Int64) As CU_Propuestas_Get_Result
        Try
            Dim oResult As List(Of CU_Propuestas_Get_Result)
            oResult = CU_Propuesta_Get(ID, Nothing, Nothing, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function CU_Propuesta_Get(ByVal ID As Int64?, ByVal BriefID As Int64?, ByVal idGerenteCuentas As Int64?, ByVal idEstadoPropuesta As Int16?) As List(Of CU_Propuestas_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_Propuestas_Get_Result) = oMatrixContext.CU_Propuestas_Get(ID, BriefID, idGerenteCuentas, idEstadoPropuesta)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function ObtenerPropuestaXID(ByVal id As Int64) As CU_Propuestas
        Return oMatrixContext.CU_Propuestas.Where(Function(x) x.Id = id).FirstOrDefault
    End Function
#End Region
#Region "Guardar"
    Public Function Guardar(ByVal ID As Int64?, ByVal Titulo As String, ByVal TipoId As Byte?, ByVal ProbabilidadID As Decimal?, ByVal FechaEnvio As Date?, ByVal EstadoID As Byte?, ByVal OrigenID As Byte?, ByVal FechaAprob As Date?, ByVal RazonNoAprob As Short?, ByVal FormaEnvio As String, ByVal Brief As Int64?, ByVal Tracking As Boolean?, ByVal JobBook As String, ByVal Internacional As Boolean?, ByVal anticipo As Int16, ByVal saldo As Int16, ByVal plazo As Int16, fechainiciocampo As Date?, ByVal RequestHabeasData As String) As Decimal
        Try
            Dim PropuestaID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.CU_Propuestas_Edit2(ID, Titulo, TipoId, ProbabilidadID, FechaEnvio, EstadoID, OrigenID, FechaAprob, RazonNoAprob, FormaEnvio, Brief, Tracking, JobBook, Internacional, anticipo, saldo, plazo, fechainiciocampo, RequestHabeasData)
                PropuestaID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.CU_Propuestas_Add2(Titulo, TipoId, ProbabilidadID, FechaEnvio, EstadoID, OrigenID, FechaAprob, RazonNoAprob, FormaEnvio, Brief, Tracking, JobBook, Internacional, anticipo, saldo, plazo, fechainiciocampo, RequestHabeasData)
                PropuestaID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return PropuestaID
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Sub ActualizarPropuesta_HabeasData(ByVal Id As Int64?, ByVal RequestHabeasData As String)
        oMatrixContext.CU_Propuestas_Edit_HabeasData(Id, RequestHabeasData)
    End Sub

    Public Function GuardarPropuesta(ByRef e As CU_Propuestas) As Long
        If e.Id = 0 Then
            oMatrixContext.CU_Propuestas.Add(e)
        End If
        oMatrixContext.SaveChanges()
        Return e.Id
    End Function
#End Region
#Region "Eliminar"
    Public Function Eliminar(ByVal ID As Int64) As Integer
        Try
            Return oMatrixContext.CU_Propuestas_Del(ID)
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
