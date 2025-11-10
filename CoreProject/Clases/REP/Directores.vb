
'Imports CoreProject.Reportes
'Imports CoreProject.RPMatrixModel

Namespace Reportes
    <Serializable()>
    Public Class Directores
#Region "Variables Globales"
        Private oMatrixContext As RP_Entities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New RP_Entities
        End Sub
#End Region

#Region "Obtener"
        Function ObtenerListadoPropuestas(fechaini As Date, fechafin As Date, unidad As Int32) As List(Of REP_ListadoPropuestasDirector_Result)
            Return oMatrixContext.REP_ListadoPropuestasDirector(fechaini, fechafin, unidad).ToList
        End Function

        Function ObtenerListadoBrief(unidad As Int32) As List(Of REP_ListadoBriefDirector_Result)
            Return oMatrixContext.REP_ListadoBriefDirector(unidad).ToList
        End Function

        Function ObtenerAsignacionRecursosXUnidad(fechaini As Date, fechafin As Date, unidad As Int32) As List(Of REP_AsignacionRecursosXUnidad_Result)
            Return oMatrixContext.REP_AsignacionRecursosXUnidad(fechaini, fechafin, unidad).ToList
        End Function

		Function ObtenerListadoTrabajosCCT(ByVal cct As Int64?, gerentecuentas As Int64?, idtrabajo As Int64?, jobbook As String, nombretrabajo As String) As List(Of REP_ListadoTrabajosCCT_Result)
			Return oMatrixContext.REP_ListadoTrabajosCCT(cct, gerentecuentas, idtrabajo, jobbook, nombretrabajo).ToList
		End Function

		Function ObtenerListadoPlaneacionUnidad(ByVal GrupoUnidad As Integer?, ByVal Unidad As Integer?) As List(Of REP_ListadoPlaneacionUnidades_Result)
			Return oMatrixContext.REP_ListadoPlaneacionUnidades(GrupoUnidad, Unidad).ToList
		End Function
#End Region
	End Class
End Namespace

