

'Imports CoreProject.TH_Model
'Imports CoreProject
Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

Namespace TH
	Public Class Educacion

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

		Public Function ObtenerEducacionPorIdentificacion(ByVal identificacion As Long) As List(Of TH_Educacion_Get_Result)
			Return THContext.TH_Educacion_Get(identificacion).ToList
		End Function
        Public Function ObtenerEducacionEmpleados() As List(Of TH_Educacion_Get_Result)
            Return THContext.TH_Educacion_Get(Nothing).ToList
        End Function
        Public Function ObtenerEducacionEmpleadosReport() As List(Of TH_Educacion_Report_Result)
            Return THContext.TH_Educacion_Report(Nothing).ToList
        End Function

        Public Sub EliminarEducacion(ByVal id As Integer)
			THContext.TH_Educacion_Del(id)
		End Sub

		Public Function EditarEducacion(Id As Integer, hojavidaId As Integer, nivelEstudioId As Integer, titulo As String, institucion As String, paisId As Integer, ciudadId As Integer, inicio As Date, finalizacion As Date, estadoEducacionId As Integer) As Integer
			Try
				Try
					Return THContext.TH_Educacion_Edit(Id, hojavidaId, nivelEstudioId, titulo, institucion, paisId, ciudadId, inicio, finalizacion, estadoEducacionId)
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

		Public Function AgregarEducacion(identificacion As Int64, tipo As UShort, titulo As String, institucion As String, pais As String, ciudad As String, fechaInicio As Date, fechaFin As Date?, modalidad As UShort, estado As UShort) As Integer
			Return THContext.TH_Educacion_Add(identificacion, tipo, titulo, institucion, pais, ciudad, fechaInicio, fechaFin, modalidad, estado)
		End Function
		Function obtenerTiposEducacion() As List(Of TH_TiposEducacion_Get_Result)
			Return THContext.TH_TiposEducacion_Get().ToList
		End Function
		Function obtenerModalidadesEducacion() As List(Of TH_ModalidadesEducacion_Get_Result)
			Return THContext.TH_ModalidadesEducacion_Get().ToList
		End Function
		Function obtenerEstadosEducacion() As List(Of TH_EstadoEducacion_Get_Result)
			Return THContext.TH_EstadoEducacion_Get.ToList
		End Function
	End Class
End Namespace