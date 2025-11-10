
'Imports CoreProject.ES_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class MetodologiaCampo
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
    Public Function DevolverTodos() As List(Of ES_MetodologiaCampo_Get_Result)
        Try
            Return ES_MetodologiaCampo_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxID(ByVal ID As Int64) As ES_MetodologiaCampo_Get_Result
        Try
            Dim oResult As List(Of ES_MetodologiaCampo_Get_Result)
            oResult = ES_MetodologiaCampo_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxIDTrabajo(ByVal TrabajoID As Int64) As List(Of ES_MetodologiaCampo_Get_Result)
        Try
            Return ES_MetodologiaCampo_Get(Nothing, TrabajoID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function ES_MetodologiaCampo_Get(ByVal ID As Int64?, ByVal TrabajoID As Int64?) As List(Of ES_MetodologiaCampo_Get_Result)
        Try
            Dim oResult As ObjectResult(Of ES_MetodologiaCampo_Get_Result) = oMatrixContext.ES_MetodologiaCampo_Get(ID, TrabajoID)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function ObtenerAprobacionMetXMetodologia(ByVal MetodologiaId As Int64) As List(Of ES_AprobacionMetodologia)
        Try
            Return oMatrixContext.ES_AprobacionMetodologia.Where(Function(x) x.MetodologiaId = MetodologiaId).ToList
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function ES_MetodologiaCampoXTr_Get(ByVal TrabajoID As Int64?) As List(Of ES_MetodologiaCampo_Get_Result)
        Try
            Dim oResult As ObjectResult(Of ES_MetodologiaCampo_Get_Result) = oMatrixContext.ES_MetodologiaCampo_Get(Nothing, TrabajoID)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function MetodologiaCampoTr(ByVal TrabajoID As Int64?) As List(Of ES_MetodologiaCampoTr_Result)
        Try
            Dim oResult As ObjectResult(Of ES_MetodologiaCampoTr_Result) = oMatrixContext.ES_MetodologiaCampoTr(TrabajoID)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function ObtenerMetodologiaXIdxTr(ByVal TrabajoID As Int64?, ByVal ID As Int64?) As List(Of ES_ObtenerMetodologiaXIdxTr_Result)
        Try
            Dim oResult As ObjectResult(Of ES_ObtenerMetodologiaXIdxTr_Result) = oMatrixContext.ES_ObtenerMetodologiaXIdxTr(TrabajoID, ID)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function ObtenerMetodologiaNumVersionesxTr(ByVal TrabajoID As Int64?) As Integer
        Try
            Return oMatrixContext.ES_MetodologiaCampo_NumVersiones(TrabajoID)(0).Value
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
    Public Function Guardar(ByVal ID As Int64?, ByVal TrabajoID As Int64?, ByVal NombreEstudio As String, ByVal Fecha As DateTime,
                            ByVal Objetivo As Boolean, ByVal Mercado As Boolean, ByVal Marco As Boolean, ByVal Tecnica As Boolean,
                            ByVal Diseno As Boolean, ByVal Instrucciones As Boolean, ByVal Distribucion As Boolean, ByVal NivelConfianza As Boolean, ByVal MargenError As Boolean,
                            ByVal Desagregacion As Boolean, ByVal Fuente As Boolean, ByVal Variables As Boolean, ByVal Tasa As Boolean, ByVal Procedimiento As Boolean,
                            ByVal ObjetivoT As String, ByVal MercadoT As String, ByVal MarcoT As String, ByVal TecnicaT As String, ByVal DisenoT As String,
                            ByVal InstruccionesT As String, ByVal DistribucionT As String, ByVal NivelConfianzaT As String, ByVal MargenErrorT As String, ByVal DesagregacionT As String,
                            ByVal FuenteT As String, ByVal VariablesT As String, ByVal TasaT As String, ByVal ProcedimientoT As String, ByVal version As Int64?, ByVal usuario As Int64?) As Decimal
        Try
            Dim MetodologiaID As Decimal = 0
            Dim oResult As ObjectResult(Of Decimal?)
            oResult = oMatrixContext.ES_MetodologiaCampo_Add(TrabajoID, NombreEstudio, Fecha, Objetivo, Mercado, Marco, Tecnica, Diseno, Instrucciones, Distribucion, NivelConfianza, MargenError,
                             Desagregacion, Fuente, Variables, Tasa, Procedimiento, ObjetivoT, MercadoT, MarcoT, TecnicaT, DisenoT, InstruccionesT, DistribucionT, NivelConfianzaT, MargenErrorT, DesagregacionT,
                             FuenteT, VariablesT, TasaT, ProcedimientoT, version, usuario)
            MetodologiaID = Decimal.Parse(oResult.ToList().Item(0))

            Return MetodologiaID
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function GuardarAprobacionMetodologia(ByVal Metodologia As ES_AprobacionMetodologia) As Decimal
        Try
            Dim MetodologiaID As Decimal = 0
            oMatrixContext.ES_AprobacionMetodologia.Add(Metodologia)
            oMatrixContext.SaveChanges()

            If Metodologia.id > 0 Then
                MetodologiaID = Convert.ToDecimal(Metodologia.id)
            End If

            Return MetodologiaID
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
            Return oMatrixContext.ES_MetodologiaCampo_Del(ID)
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
