

'Imports CoreProject.MatrixModel

Public Class Feedback
#Region "Variables Globales"
    Private oMatrixContext As CORE_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CORE_Entities
    End Sub
#End Region
#Region "Metodos"
    Public Function ObtenerAsunto() As List(Of CORE_Asunto_Get_Result)
        Return oMatrixContext.CORE_Asunto_Get.ToList
    End Function

    Public Sub EnviarFeedBack(ByVal usuario As Long, ByVal tipo_mensaje As Integer, ByVal mensaje As String)
        oMatrixContext.CORE_Feedback_Add(usuario, tipo_mensaje, mensaje)
    End Sub

    Public Function ObtenerFeedbackResueltos() As List(Of CORE_Retroalimentacion)
        Return oMatrixContext.CORE_Retroalimentacion.Where(Function(x) x.Solucionado = True).ToList
    End Function

    Public Function ObtenerFeedbackPendientes() As List(Of CORE_Retroalimentacion)
        Return oMatrixContext.CORE_Retroalimentacion.Where(Function(x) x.Solucionado = False Or x.Solucionado Is Nothing).ToList
    End Function

    Public Function ObtenerFeedbackXId(ByVal id As Int64) As CORE_Retroalimentacion
        Return oMatrixContext.CORE_Retroalimentacion.Where(Function(x) x.id = id).FirstOrDefault
    End Function

    Public Sub ActualizarFeedback(ByVal Feedb As CORE_Retroalimentacion)
        Dim ent As New CORE_Retroalimentacion
        ent = ObtenerFeedbackXId(Feedb.id)
        ent.Respuesta = Feedb.Respuesta
        ent.FechaSolucion = Feedb.FechaSolucion
        ent.Solucionado = Feedb.Solucionado
        ent.UsuarioResponde = Feedb.UsuarioResponde
        oMatrixContext.SaveChanges()
    End Sub
#End Region
End Class
