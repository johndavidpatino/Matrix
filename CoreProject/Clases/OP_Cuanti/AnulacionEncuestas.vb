
'Imports CoreProject.OP_Cuanti_Model

<Serializable()>
Public Class AnulacionEncuestas
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
    Public Function EncuestasAnuladasListXTrabajo(ByVal TrabajoId As Int64) As List(Of OP_EncuestasAnuladas)
        Return oMatrixContext.OP_EncuestasAnuladas.Where(Function(x) x.TrabajoId = TrabajoId).ToList
    End Function
#End Region
#Region "Grabar"
    Function grabar(ByVal trabajoId As Int64, ByVal numeroEncuesta As Int64, ByVal observacion As String, ByVal fecha As DateTime, ByVal usuarioId As Int64, ByVal unidadId As Int64) As Int64
        Dim oeEncuestasAnuladas As New OP_EncuestasAnuladas
        oeEncuestasAnuladas.TrabajoId = trabajoId
        oeEncuestasAnuladas.NumeroEncuesta = numeroEncuesta
        oeEncuestasAnuladas.Observacion = observacion
        oeEncuestasAnuladas.Fecha = fecha
        oeEncuestasAnuladas.UsuarioId = usuarioId
        oeEncuestasAnuladas.UnidadId = unidadId
        oMatrixContext.OP_EncuestasAnuladas.Add(oeEncuestasAnuladas)
        oMatrixContext.SaveChanges()
        Return oeEncuestasAnuladas.id
    End Function

    Public Function ExisteEncuestaAnulada(ByVal TrabajoId As Int64, ByVal NoEncuesta As Int64) As Int16
        Return oMatrixContext.OP_ExisteEncuestaAnulada(TrabajoId, NoEncuesta)(0).Value
    End Function

    Public Function ExisteEncuestaAnuladaGC(ByVal TrabajoId As Int64, ByVal NoEncuesta As Int64) As Int16
        Return oMatrixContext.OP_GestionCampo_ExisteEncuesta(TrabajoId, NoEncuesta)(0).Value
    End Function

    Public Sub AnularEncuestaGC(ByVal TrabajoId As Int64, ByVal NoEncuesta As Int64, observacion As String)
        oMatrixContext.OP_GestionCampo_AnularEncuesta(TrabajoId, NoEncuesta, observacion)
    End Sub
#End Region
End Class
