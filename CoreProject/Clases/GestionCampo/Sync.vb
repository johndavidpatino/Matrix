Namespace GestionCampo
    Public Class Sync
#Region "Variables Globales"
        Private oMatrixContext As GCEntities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New GCEntities
        End Sub
#End Region
        Enum ETipoAccion
            actualizado = 2
        End Enum
        Enum EModulo
            Matrix_SoftSyn_ActualizaciónDatos = 6
        End Enum
        Enum ETabla
            Respuestas = 1
        End Enum
#Region "Obtener"
        Public Function PreguntasGet(ByVal TrabajoId As Int64?, SubjNum As Decimal?) As List(Of Sync_Preguntas_Get_Result)
            oMatrixContext.Database.CommandTimeout = 60
            Return oMatrixContext.Sync_Preguntas_Get(TrabajoId, SubjNum).ToList
        End Function
        Public Sub QuitarPreguntasEntrenamiento(ByVal TrabajoId As Int64)
            oMatrixContext.Sync_EncuestasEntrenamiento(TrabajoId)
        End Sub
        Public Sub ErrorTrabajoEspecializado(ByVal TrabajoId As Int64)
            oMatrixContext.Sync_ErrorTrabajoEspecializado(TrabajoId)
        End Sub
        Public Sub HabilitarSincronizacion(ByVal TrabajoId As Int64)
            oMatrixContext.Sync_HabilitarSincronizacionEstudio(TrabajoId)
        End Sub
        Public Sub ActualizarPregunta(ByVal SubjNum As Decimal, valor As String, dcp As String, ByVal e_id As Decimal)
            oMatrixContext.Sync_Preguntas_UpdateInfo(SubjNum, dcp, valor, e_id)
        End Sub
        Public Sub HabilitarEncuestaPiloto(ByVal SubjNum As Decimal)
            oMatrixContext.Sync_HabilitarEncuestasPiloto(SubjNum)
        End Sub
        Public Sub EncuestaPiloto(ByVal SubjNum As Decimal)
            oMatrixContext.Sync_EncuestaPiloto(SubjNum)
        End Sub
        Public Function obtenerIdRegistroRespuestas(idTrabajo As Decimal, numeroEncuesta As Decimal) As Decimal
            Return oMatrixContext.obtenerRespuestaIdRegistroXIdTrabajoNumeroEncuesta(idTrabajo, numeroEncuesta).FirstOrDefault
        End Function
#End Region
        Public Sub grabarAuditoria(idUsuario As Decimal, tipoAcction As ETipoAccion, modulo As EModulo, descripcion As String, fecha As DateTime, idRegistro As Decimal, tabla As ETabla)
            Dim oParameter As New Entity.Core.Objects.ObjectParameter("A_Id", System.DBNull.Value)
            oMatrixContext.GrabarAuditoria(oParameter, idUsuario, tipoAcction, modulo, descripcion, fecha, idRegistro, tabla)
        End Sub

        Public Function ObtenerResultadoVerificacion() As List(Of TiposResultadosVerificacionCampo)
            Return oMatrixContext.TiposResultadosVerificacionCampo.ToList()
        End Function

    End Class
End Namespace