Namespace Reportes
	<Serializable()>
	Public Class ReportesGenerales
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
		Function PropuestaInfoGeneral(ByVal idPY As Int64?, idTr As Int64?) As List(Of REP_PYPropuesta_InfoGeneral_Result)
			Return oMatrixContext.REP_PYPropuesta_InfoGeneral(idPY, idTr).ToList
		End Function

		Function PropuestaInfoMuestra(ByVal idPY As Int64?, idTr As Int64?) As List(Of REP_PYPropuesta_InfoMuestra_Result)
			Return oMatrixContext.REP_PYPropuesta_InfoMuestra(idPY, idTr).ToList
		End Function

		Function PropuestaInfoPreguntas(ByVal idPY As Int64?, idTr As Int64?) As List(Of REP_PYPropuesta_InfoPreguntas_Result)
			Return oMatrixContext.REP_PYPropuesta_InfoPreguntas(idPY, idTr).ToList
		End Function

		Function FormIDS(ByVal idPY As Int64?, idTr As Int64?) As REP_PY_IDFORMS_Result
			Return oMatrixContext.REP_PY_IDFORMS(idPY, idTr).ToList.First
		End Function

#End Region
	End Class
End Namespace