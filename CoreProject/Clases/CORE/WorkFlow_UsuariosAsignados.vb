

'Imports CoreProject.MatrixModel

Public Class WorkFlow_UsuariosAsignados
#Region "Variables Globales"
    Private oMatrixContext As CORE_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CORE_Entities
    End Sub
#End Region
#Region "Grabar"
    Sub grabar(ByVal WorkFlowId As Int64, ByVal UsuarioId As Int64, ByVal FechaAsignacion As Date)
        Dim oeCORE_WorlFlow_UsuariosAsignados As New CORE_WorkFlow_UsuariosAsignados
        oeCORE_WorlFlow_UsuariosAsignados.WorkFlowId = WorkFlowId
        oeCORE_WorlFlow_UsuariosAsignados.UsuarioId = UsuarioId
        oeCORE_WorlFlow_UsuariosAsignados.FechaAsignacion = FechaAsignacion
        oMatrixContext.CORE_WorkFlow_UsuariosAsignados.Add(oeCORE_WorlFlow_UsuariosAsignados)
        oMatrixContext.SaveChanges()
    End Sub
#End Region
#Region "Obtener"
    Function obtenerUsuariosXWorkFlowIdXEstadoXRolId(ByVal pWorkFlowId As Int64?, ByVal pAsignado As Boolean, ByVal pRolId As Int64) As List(Of CORE_WorkFlow_UsuariosAsignados_Get_Result)
        Return oMatrixContext.CORE_WorkFlow_UsuariosAsignados_Get(pWorkFlowId, pAsignado, pRolId).ToList
    End Function

    Function obtener(ByVal idtrabajo As Int64, ByVal rolid As Int64) As CORE_obtenerusuariosasignados_get_Result
        Return oMatrixContext.CORE_obtenerusuariosasignados_get(idtrabajo, rolid).FirstOrDefault
    End Function

#End Region
#Region "Eliminar"
    Sub eliminar(ByVal id As Int64)
        oMatrixContext.CORE_WorkFlow_UsuariosAsignados.Remove(oMatrixContext.CORE_WorkFlow_UsuariosAsignados.Where(Function(x) x.id = id).FirstOrDefault)
        oMatrixContext.SaveChanges()
    End Sub
#End Region
End Class
