
'Imports CoreProject.OP_Model

<Serializable()>
Public Class GestionTrabajosOP
#Region "Variables Globales"
    Private oMatrixContext As OP_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function ListaTrabajos(Id As Int64?, Nombre As String, JobBook As String, Proyecto As Int64?, COE As Int64?, GerenteCuentas As Int64?, Unidad As Int64?, Gerencia As Int64?, Propuesta As Int64?, Estado As Int32?) As List(Of OP_Trabajos_Get_Result)
        Return oMatrixContext.OP_Trabajos_Get(Id, Estado, Nombre, JobBook, Proyecto, COE, GerenteCuentas, Unidad, Gerencia, Propuesta).ToList
    End Function

    Public Function MuestraXTrabajo(IdMuestra As Int64?, IdTrabajo As Int64?) As List(Of Op_MuestraTrabajos_Get_Result)
        Return oMatrixContext.Op_MuestraTrabajosGet(IdMuestra, IdTrabajo).ToList
    End Function

    Public Function ListaTrabajosXCoordinador(Id As Int64?, Nombre As String, JobBook As String, Proyecto As Int64?, COE As Int64?, GerenteCuentas As Int64?, Unidad As Int64?, Gerencia As Int64?, Propuesta As Int64?, Estado As Int32?, Coordinador As Int64?) As List(Of OP_Trabajos_xCoordinador_Get_Result)
        Return oMatrixContext.OP_Trabajos_xCoordinador_Get(Id, Estado, Nombre, JobBook, Proyecto, COE, GerenteCuentas, Unidad, Gerencia, Propuesta, Coordinador).ToList
    End Function

    Public Function ListaTrabajosCallCenter(Id As Int64?, Nombre As String, JobBook As String, Proyecto As Int64?, COE As Int64?, GerenteCuentas As Int64?, Unidad As Int64?, Gerencia As Int64?, Propuesta As Int64?, Estado As Int32?, Coordinador As Int64?) As List(Of OP_Trabajos_CallCenter_Get_Result)
        Return oMatrixContext.OP_Trabajos_CallCenter_Get(Id, Estado, Nombre, JobBook, Proyecto, COE, GerenteCuentas, Unidad, Gerencia, Propuesta, Coordinador).ToList
    End Function
#End Region

End Class
