Imports System.Data.Entity.Core.Objects
'Imports CoreProject.TH_Model
'Imports CoreProject

<Serializable()>
Public Class Cargos
#Region "Variables Globales"
	Private oMatrixContext As TH_Entities
#End Region
#Region "Enumeradores"
	Enum TiposCargos
		Supervisor = 72
		Encuestador = 73
		Verificador = 38
		Digitador = 75
		Codificador = 28
		DigitadorRMC = 74
		Critico = 38
	End Enum
#End Region
#Region "Constructores"
	'Constructor public 
	Public Sub New()
		oMatrixContext = New TH_Entities
	End Sub
#End Region
#Region "Obtener"
	Public Function DevolverTodos() As List(Of TH_Cargos_Get_Result)
		Try
			Return TH_Cargos_Get()
		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Function
	Private Function TH_Cargos_Get() As List(Of TH_Cargos_Get_Result)
		Try
			Dim oResult As ObjectResult(Of TH_Cargos_Get_Result) = oMatrixContext.TH_Cargos_Get()
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
End Class
