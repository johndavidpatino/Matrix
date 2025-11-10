
'Imports CoreProject.ES_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class BriefDisenoMuestral
#Region "Variables Globales"
    Private oMatrixContext As ES_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New ES_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function DevolverTodos() As List(Of ES_BriefDisenoMuestral_Get_Result)
        Try
            Return ES_BriefDisenoMuestral_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxID(ByVal ID As Int64) As ES_BriefDisenoMuestral_Get_Result
        Try
            Dim oResult As List(Of ES_BriefDisenoMuestral_Get_Result)
            oResult = ES_BriefDisenoMuestral_Get(ID, Nothing)
            Return oResult.FirstOrDefault
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function ObtenerBriefXId(ByVal id As Int64) As List(Of ES_BriefDisenoMuestral)
        Dim brief As New ES_Entities
        Return brief.ES_BriefDisenoMuestral.Where(Function(x) x.id = id).ToList()
    End Function

    Public Function ObtenerBriefXPresupuesto(ByVal Propuestaid As Int64) As List(Of ES_BriefDisenoMuestral)
        Dim brief As New ES_Entities
        Return brief.ES_BriefDisenoMuestral.Where(Function(x) x.Propuestaid = Propuestaid).ToList()
    End Function

    Public Function DevolverxIDPropuesta(ByVal PropuestaID As Int64) As List(Of ES_BriefDisenoMuestral_Get_Result)
        Try
            Return ES_BriefDisenoMuestral_Get(Nothing, PropuestaID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function ES_BriefDisenoMuestral_Get(ByVal ID As Int64?, ByVal PropuestaID As Int64?) As List(Of ES_BriefDisenoMuestral_Get_Result)
        Try
            Dim oResult As ObjectResult(Of ES_BriefDisenoMuestral_Get_Result) = oMatrixContext.ES_BriefDisenoMuestral_Get(ID, PropuestaID)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal PropuestaID As Int64?, ByVal Fecha As DateTime, ByVal Objetivo As String, ByVal Poblacion As String, ByVal Capacidad As String, ByVal Metodologia As String, ByVal NivelesDesagregacion As String, ByVal PosiblesMarcos As String, ByVal Variable As String, Observaciones As String, NoVersion As Int64) As Decimal
        Try
            Dim BriefDisenoID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.ES_BriefDisenoMuestral_Edit(ID, PropuestaID, Fecha, Objetivo, Poblacion, Capacidad, Metodologia, NivelesDesagregacion, PosiblesMarcos, Variable, Observaciones, NoVersion)
                BriefDisenoID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.ES_BriefDisenoMuestral_Add(PropuestaID, Fecha, Objetivo, Poblacion, Capacidad, Metodologia, NivelesDesagregacion, PosiblesMarcos, Variable, Observaciones, NoVersion)
                BriefDisenoID = Decimal.Parse(oResult.ToList().LastOrDefault)
            End If

            Return BriefDisenoID
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
#Region "Eliminar"
    Public Function Eliminar(ByVal ID As Int64) As Integer
        Try
            Return oMatrixContext.ES_BriefDisenoMuestral_Del(ID)
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
