
'Imports CoreProject.OP_Cuanti_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class FichaCuantitativo
#Region "Variables Globales"
    Private oMatrixContext As OP_Cuanti
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Cuanti
    End Sub
#End Region
#Region "Obtener"
    Public Function DevolverTodos() As List(Of OP_FichaCuantitativo_Get_Result)
        Try
            Return OP_FichaCuantitativo_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxTrabajoID(ByVal TrabajoID As Int64?) As List(Of OP_FichaCuantitativo_Get_Result)
        Try
            Return OP_FichaCuantitativo_Get(Nothing, TrabajoID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As OP_FichaCuantitativo_Get_Result
        Try
            Dim oResult As List(Of OP_FichaCuantitativo_Get_Result)
            oResult = OP_FichaCuantitativo_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function OP_FichaCuantitativo_Get(ByVal ID As Int64, ByVal TrabajoID As Int64) As List(Of OP_FichaCuantitativo_Get_Result)
        Try
            Dim oResult As ObjectResult(Of OP_FichaCuantitativo_Get_Result) = oMatrixContext.OP_FichaCuantitativo_Get(ID, TrabajoID)
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
    Public Function Guardar(ByVal ID As Int64?, ByVal TrabajoId As Int64?, ByVal GrupoObjetivo As String, ByVal CubrimientoGeografico As String, ByVal MarcoMuestral As String, ByVal DistribucionMuestra As String, ByVal Cuotas As String, ByVal NivelDesagregacionResultados As String, ByVal Ponderacion As String, ByVal RequerimientosEspeciales As String, ByVal OtrasObservaciones As String, ByVal IncentivoEconomico As Boolean?, ByVal PresupuestoIncentivo As Double?, ByVal RegalosCliente As Boolean?, ByVal CompraIpsos As Boolean?, ByVal Presupuesto As Double?) As Decimal
        Try
            Dim FichaCuantitativaID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.OP_FichaCuantitativo_Edit(ID, TrabajoId, GrupoObjetivo, CubrimientoGeografico, MarcoMuestral, DistribucionMuestra, Cuotas, NivelDesagregacionResultados, Ponderacion, RequerimientosEspeciales, OtrasObservaciones, IncentivoEconomico, PresupuestoIncentivo, RegalosCliente, CompraIpsos, Presupuesto)
                FichaCuantitativaID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.OP_FichaCuantitativo_Add(TrabajoId, GrupoObjetivo, CubrimientoGeografico, MarcoMuestral, DistribucionMuestra, Cuotas, NivelDesagregacionResultados, Ponderacion, RequerimientosEspeciales, OtrasObservaciones, IncentivoEconomico, PresupuestoIncentivo, RegalosCliente, CompraIpsos, Presupuesto)
                FichaCuantitativaID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return FichaCuantitativaID
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
            Return oMatrixContext.OP_FichaCuantitativo_Del(ID)
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
