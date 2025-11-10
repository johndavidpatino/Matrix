
'Imports CoreProject

Public Class GestionTrabajosFin
#Region "VariableGlobal"
    Private oMatrixContext As CC_FinzOpe
#End Region

#Region "Constructor"
    Public Sub New()
        oMatrixContext = New CC_FinzOpe
    End Sub
#End Region

#Region "Obtener"
    Function ObtenerPreguntas(ByVal TrabajoId As Int64) As CC_Preguntas
        Return oMatrixContext.CC_Preguntas.Where(Function(x) x.TrabajoId = TrabajoId).FirstOrDefault
    End Function
    Sub GuardarPreguntas(ByVal Preguntas As CC_Preguntas)
        Dim info = ObtenerPreguntas(Preguntas.TrabajoId)
        If Not info Is Nothing Then
            oMatrixContext.SaveChanges()
        Else
            oMatrixContext.CC_Preguntas.Add(Preguntas)
            oMatrixContext.SaveChanges()
        End If
    End Sub

    Public Function ListadoTrabajoscc(ByVal id As Int64?, Estado As Int32?, Nombre As String, JobBook As String, Proyecto As Int64?, Coe As Int64?, GerenteProyectos As Int64?, Unidad As Int32?, Gerencia As Int32?, Propuesta As Int64?) As List(Of CC_Trabajos_Get_Result)
        Return oMatrixContext.CC_Trabajos_Get(id, Estado, Nombre, JobBook, Proyecto, Coe, GerenteProyectos, Unidad, Gerencia, Propuesta).ToList
    End Function

    Public Function ListadoTrabajosConteo(ByVal id As Int64?, Estado As Int32?, Nombre As String, JobBook As String, Proyecto As Int64?, Coe As Int64?, GerenteProyectos As Int64?, Unidad As Int32?, Gerencia As Int32?, Propuesta As Int64?)
        Return oMatrixContext.CC_TrabajosConteo(id, Estado, Nombre, JobBook, Proyecto, Coe, GerenteProyectos, Unidad, Gerencia, Propuesta).ToList
    End Function

    Public Function ReporteConteoTrabajos(ByVal fechaInicio As Date?, fechaFin As Date?) As List(Of REP_ConteoTrabajos_Result)
        Return oMatrixContext.REP_ConteoTrabajos(fechaInicio, fechaFin).ToList
    End Function


#End Region

End Class

