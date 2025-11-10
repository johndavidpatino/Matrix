
'Imports CoreProject.CU_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Brief
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

    Function ObtenerBriefsxGrupoUnidad(ByVal TodosCampos As String, ByVal GrupoUnidad As Int32?) As List(Of CU_Brief_Get_Result)
        Return oMatrixContext.CU_Brief_Get(Nothing, Nothing, Nothing, TodosCampos, GrupoUnidad).ToList
    End Function

    Function ObtenerBriefs(ByVal ID As Int64?, ByVal ContactoID As Int64?, ByVal GerenteCuentas As Int64?, ByVal TodosCampos As String) As List(Of CU_Brief_Get_Result)
        Return oMatrixContext.CU_Brief_Get(ID, ContactoID, GerenteCuentas, TodosCampos, Nothing).ToList
    End Function

    Public Function DevolverTodos() As List(Of CU_Brief_Get_Result)
        Return oMatrixContext.CU_Brief_Get(Nothing, Nothing, Nothing, Nothing, Nothing).ToList
    End Function

    Public Function DevolverxID(ByVal ID As Int64) As CU_Brief_Get_Result
        Return oMatrixContext.CU_Brief_Get(ID, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
    End Function


    Public Function DevolverTiposBrief() As List(Of CU_TipoBrief_Get_Result)
        Try
            Return CU_TipoBrief_Get()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function CU_TipoBrief_Get() As List(Of CU_TipoBrief_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_TipoBrief_Get_Result) = oMatrixContext.CU_TipoBrief_Get()
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
    Public Function Guardar(ByVal ID As Int64?, Titulo As String, ByVal ContactoId As Int64?, ByVal TipoBrief As Byte?, ByVal Antecedentes As String, ByVal Objetivos As String, ByVal ActionStandar As String, ByVal Metodologia As String, ByVal TargetGroup As String, ByVal Tiempos As String, ByVal Presupuesto As String, ByVal Materiales As String, ByVal ResultadosAnteriores As String, ByVal FormatoInforme As String, ByVal Aprobaciones As String, ByVal Competencia As String, ByVal GerenteCuentas As Int64, ByVal Unidad As Int64) As Decimal
        Try
            Dim BriefID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.CU_Brief_Edit(ID, ContactoId, TipoBrief, Titulo, Antecedentes, Objetivos, ActionStandar, Metodologia, TargetGroup, Tiempos, Presupuesto, Materiales, ResultadosAnteriores, FormatoInforme, Aprobaciones, Competencia, Unidad)
                BriefID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.CU_Brief_Add(Titulo, ContactoId, TipoBrief, Antecedentes, Objetivos, ActionStandar, Metodologia, TargetGroup, Tiempos, Presupuesto, Materiales, ResultadosAnteriores, FormatoInforme, Aprobaciones, Competencia, GerenteCuentas, Unidad)
                BriefID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return BriefID
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function


    Public Function GuardarLogCambioGerente(ByVal IdBrief As Int64?, ByVal GerenteAnterior As Int64?, ByVal GerenteAsignado As Int64?, ByVal UsuarioRegistra As Int64?) As Decimal
        Return oMatrixContext.CU_Brief_LogCambioGerente_Add(IdBrief, GerenteAnterior, GerenteAsignado, UsuarioRegistra).FirstOrDefault
    End Function

#End Region
#Region "Eliminar"
    Public Function Eliminar(ByVal ID As Int64) As Integer
        Try
            Return oMatrixContext.CU_Brief_Del(ID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
#Region "Actualizar Viabilidad"
    Public Function ActualizarViabilidad(ByVal ID As Int64, ByVal Viabilidad As Boolean?, ByVal Usuario As Int64?) As Integer
        Try
            Return oMatrixContext.CU_Brief_Edit_Viabilidad(ID, Viabilidad, Usuario)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function ActualizarRazonViabilidad(ByVal ID As Int64, ByVal RazonNoViabilidad As String) As Integer
        Try
            Return oMatrixContext.CU_Brief_Edit_RazonNoViabilidad(ID, RazonNoViabilidad)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region

    Sub ActualizarGerenteCuentas(ByVal Id As Int64?, ByVal Gerente As Int64?)
        oMatrixContext.CU_Brief_Edit_GerenteCuentas(Id, Gerente)
    End Sub

    Public Function GuardarBrief(ByRef e As CU_Brief) As Long
		If e.Id = 0 Then
			oMatrixContext.CU_Brief.Add(e)
		End If
		oMatrixContext.SaveChanges()
		Return e.Id
	End Function

    Public Function ObtenerBriefXID(ByVal id As Int64) As CU_Brief
        Return oMatrixContext.CU_Brief.Where(Function(x) x.Id = id).FirstOrDefault
    End Function
End Class
