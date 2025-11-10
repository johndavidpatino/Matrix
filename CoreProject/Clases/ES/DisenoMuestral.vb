
'Imports CoreProject.ES_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class DisenoMuestral
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
    Public Function DevolverTodos() As List(Of ES_DisenoMuestral_Get_Result)
        Try
            Return ES_DisenoMuestral_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxID(ByVal ID As Int64) As ES_DisenoMuestral_Get_Result
        Try
            Dim oResult As List(Of ES_DisenoMuestral_Get_Result)
            oResult = ES_DisenoMuestral_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxIDBrief(ByVal briefID As Int64) As List(Of ES_DisenoMuestral_Get_Result)
        Try
            Return ES_DisenoMuestral_Get(Nothing, briefID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function ES_DisenoMuestral_Get(ByVal ID As Int64?, ByVal briefID As Int64?) As List(Of ES_DisenoMuestral_Get_Result)
        Try
            Dim oResult As ObjectResult(Of ES_DisenoMuestral_Get_Result) = oMatrixContext.ES_DisenoMuestral_Get(ID, briefID)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal BriefID As Int64?, ByVal Fecha As DateTime, ByVal MuestroProbabilistico As Boolean, ByVal Objetivo As Boolean,
                            ByVal Poblacion As Boolean, ByVal Mercado As Boolean, ByVal Marco As Boolean, ByVal Tecnica As Boolean, ByVal Diseno As Boolean, ByVal Tamano As Boolean, ByVal Fiabilidad As Boolean,
                            ByVal Desagregacion As Boolean, ByVal Fuente As Boolean, ByVal Ponderacion As Boolean,
                            ByVal ObjetivoT As String, ByVal PoblacionT As String, ByVal MercadoT As String, ByVal MarcoT As String, ByVal TecnicaT As String, ByVal DisenoT As String, ByVal TamanoT As String, ByVal FiabilidadT As String,
                            ByVal DesagregacionT As String, ByVal FuenteT As String, ByVal PonderacionT As String, ByVal Variable As Boolean, ByVal VariableT As String, ByVal Observaciones As String, ByVal ObservacionesT As String, ByVal NoVersion As Nullable(Of Integer)) As Decimal
        Try
            Dim DisenoID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.ES_DisenoMuestral_Edit(ID, BriefID, Fecha, MuestroProbabilistico, Objetivo, Poblacion, Mercado, Marco, Tecnica, Diseno, Tamano, Fiabilidad, Desagregacion, Fuente, Ponderacion, ObjetivoT,
                                                      PoblacionT, MercadoT, MarcoT, TecnicaT, DisenoT, TamanoT, FiabilidadT, DesagregacionT, FuenteT, PonderacionT, Variable, VariableT, Observaciones, ObservacionesT, NoVersion)
                DisenoID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.ES_DisenoMuestral_Add(BriefID, Fecha, MuestroProbabilistico, Objetivo, Poblacion, Mercado, Marco, Tecnica, Diseno, Tamano, Fiabilidad, Desagregacion, Fuente, Ponderacion, ObjetivoT,
                                                               PoblacionT, MercadoT, MarcoT, TecnicaT, DisenoT, TamanoT, FiabilidadT, DesagregacionT, FuenteT, PonderacionT, Variable, VariableT, Observaciones, ObservacionesT, NoVersion)
                DisenoID = Decimal.Parse(oResult.ToList().FirstOrDefault)
            End If

            Return DisenoID
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
            Return oMatrixContext.ES_DisenoMuestral_Del(ID)
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
