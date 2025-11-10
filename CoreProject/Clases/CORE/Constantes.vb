Public Class Constantes
#Region "Variables Globales"
    Private oMatrixContext As New CORE_Entities
#End Region
#Region "Enumerados"
    Enum EConstantes
        UsuarioArchivos = 1
        ContrasenaArchivos = 2
        ServidorRecuperacionArchivos = 3
        correosNotificarFeedbackMatrix = 7
    End Enum
#End Region
#Region "Metodos"
    Function obtenerXId(ByVal Id As EConstantes?) As f_Constantes_Get_Result
        Return oMatrixContext.f_Constantes_Get(Id).FirstOrDefault
    End Function
#End Region

End Class
