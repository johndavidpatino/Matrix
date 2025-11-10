
Imports System.Data.Entity.Core.Objects
'Imports CoreProject.TH_Model
'Imports CoreProject
Imports System.Data.SqlClient

Namespace TH
	Public Class ExperienciaLaboral

		Private _THcontext As TH_Entities

		Public Property THContext As TH_Entities
			Get
				If Me._THcontext Is Nothing Then
					Me._THcontext = New TH_Entities()
				End If
				Return Me._THcontext
			End Get
			Set(value As TH_Entities)
				Me._THcontext = value
			End Set
		End Property

		Public Function ObtenerExperienciaLaboralHVID(ByVal HojaVidaId As Integer) As List(Of TH_ExperienciaLaboral_Get_Result)
			Try
				Try
					Dim List As ObjectResult(Of TH_ExperienciaLaboral_Get_Result) = THContext.TH_ExperienciaLaboral_Get(HojaVidaId)
					Return List.ToList()
				Catch ex As SqlException
					Throw ex
				End Try
			Catch ex As Exception
				If IsNothing(ex.InnerException) Then
					Throw ex
				Else
					Throw ex.InnerException
				End If
			End Try
		End Function

		Public Sub EliminarExperienciaLaboral(ByVal id As Integer)
			THContext.TH_ExperienciaLaboral_Del(id)
		End Sub

		Public Function EditarExperienciaLaboral(Id As Integer, hojaVidaId As Integer, empresa As String, telefono As String, inicio As Date, finalizacion As Date, actualmente As Boolean, cargoId As Integer, nivelCargoId As Integer, paisId As Integer, ciudadId As Integer, direccion As String) As Integer
			Try
				Try
					Return THContext.TH_ExperienciaLaboral_Edit(Id, hojaVidaId, empresa, telefono, inicio, finalizacion, actualmente, cargoId, nivelCargoId, paisId, ciudadId, direccion)
				Catch ex As SqlException
					Throw ex
				End Try
			Catch ex As Exception
				If IsNothing(ex.InnerException) Then
					Throw ex
				Else
					Throw ex.InnerException
				End If
			End Try
		End Function

		Public Function add(empleadoId As Int64, empresa As String, fechaInicio As Date, fechaFin As Date, cargo As String, esInvestigacion As Boolean) As Decimal
			Return THContext.TH_ExperienciaLaboral_Add(empleadoId, empresa, fechaInicio, fechaFin, cargo, esInvestigacion).FirstOrDefault
		End Function
		Public Function getByPersonaId(personaId As Int64) As List(Of TH_ExperienciaLaboral_Get_Result)
			Return THContext.TH_ExperienciaLaboral_Get(personaId).ToList
		End Function
		Public Sub deleteById(ByVal id As Integer)
			THContext.TH_ExperienciaLaboral_Del(id)
		End Sub
        Public Function getExperienciaLaboralEmpleados() As IEnumerable(Of TH_ExperienciaLaboral_Get_Result)
            Return THContext.TH_ExperienciaLaboral_Get(Nothing).ToList
        End Function
        Public Function getExperienciaLaboralEmpleadosReport() As IEnumerable(Of TH_ExperienciaLaboral_Report_Result)
            Return THContext.TH_ExperienciaLaboral_Report(Nothing).ToList
        End Function
    End Class
End Namespace