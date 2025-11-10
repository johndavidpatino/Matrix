

'Imports CoreProject.MatrixModel

Public Class LogWorkFlow
#Region "Enumerados"
    Enum WorkFlowEstados
        Creada = 1
        EnCurso = 2
        Asignada = 3
        Devuelta = 4
        Finalizada = 5
        NoAplica = 6
    End Enum
#End Region
#Region "Variables Globales"
    Private oMatrixContext As CORE_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CORE_Entities
    End Sub
#End Region
#Region "Obtener"
    Function obtenerCantidadTareasXTrabajo(ByVal idUsuario As Int64) As List(Of CORE_obtenerCantidadTareasXTrabajo_Result)
        Return oMatrixContext.CORE_obtenerCantidadTareasXTrabajo(idUsuario).ToList
    End Function
#End Region
#Region "Grabar"
    ''' <summary>
    ''' Cambia el estado de las tareas dependientes a una tarea y que se debean finalizar automaticamente
    ''' </summary>
    ''' <param name="idTarea">Id de la tarea especifica</param>
    ''' <param name="idTrabajo">Id del trabajo</param>
    ''' <param name="idUsuario">Id del usuario</param>
    ''' <remarks></remarks>
    Sub CORE_TareasDependientes_Finalizar(ByVal idTarea As Int64, ByVal idTrabajo As Int64, ByVal idUsuario As Int64)
        oMatrixContext.CORE_TareasDependientes_Finalizar(idTarea, idTrabajo, idUsuario)
    End Sub

    Sub CORE_Log_WorkFlow_MasivoEstadoCreada_Add(ByVal trabajoId As Int64, ByVal fechaRegistro As Date, ByVal usuario As Int64, ByVal tipoHiloId As Int64)
        oMatrixContext.CORE_Log_WorkFlow_MasivoEstadoCreada_Add(trabajoId, fechaRegistro, usuario, tipoHiloId)
    End Sub
    Function CORE_Log_WorkFlow_Add(ByVal core_WorkFlow_Id As Int64, ByVal fechaRegistro As DateTime, ByVal usuario As Int64, ByVal estadoWorkFlow_Id As WorkFlowEstados, ByVal observacion As String) As Decimal
        Return oMatrixContext.CORE_Log_WorkFlow_Add(core_WorkFlow_Id, fechaRegistro, usuario, estadoWorkFlow_Id, observacion)(0)
    End Function

#End Region
End Class
