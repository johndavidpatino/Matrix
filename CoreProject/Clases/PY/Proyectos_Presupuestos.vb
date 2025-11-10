
'Imports CoreProject.PY_Model

<Serializable()>
Public Class Proyectos_Presupuestos
#Region "Variables Globales"
    Private oMatrixContext As PY_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New PY_Entities
    End Sub
#End Region
#Region "Grabar"
    Function Grabar(ByVal proyecto As PY_Proyectos_Get_Result, ByVal presupuestos As List(Of PY_Proyecto_Presupuesto_Get_Result)) As Int64
        Dim oProyecto As New Proyecto
        Dim idProyecto As Decimal

		idProyecto = oProyecto.Grabar(proyecto.JobBook, proyecto.Nombre, proyecto.UnidadId, Nothing, proyecto.EstudioId, proyecto.TipoProyectoId, Nothing, proyecto.A1, proyecto.A2, proyecto.A3, proyecto.A4, proyecto.A5, proyecto.A6, proyecto.A7)

		For Each Presupuesto In presupuestos
            Grabar(Presupuesto.PresupuestoId, idProyecto)
        Next

        Return idProyecto
    End Function
    Sub Grabar(ByVal presupuestoId As Long, ByVal proyectoId As Long)
        oMatrixContext.PY_Proyecto_Presupuesto_Add(presupuestoId, proyectoId)
    End Sub
    Sub Grabar(ByVal proyecto As PY_Proyectos_Get_Result, ByVal presupuestosNuevos As List(Of PY_Proyecto_Presupuesto_Get_Result), ByVal presupuestosEliminados As List(Of PY_Proyecto_Presupuesto_Get_Result))
        Dim oProyecto As New Proyecto
        Dim idProyecto As Decimal

		idProyecto = oProyecto.Grabar(proyecto.JobBook, proyecto.Nombre, proyecto.UnidadId, Nothing, proyecto.EstudioId, proyecto.TipoProyectoId, proyecto.id, proyecto.A1, proyecto.A2, proyecto.A3, proyecto.A4, proyecto.A5, proyecto.A6, proyecto.A7)

		For Each Presupuesto In presupuestosNuevos
            Grabar(Presupuesto.PresupuestoId, idProyecto)
        Next

        For Each Presupuesto In presupuestosEliminados
            eliminar(Presupuesto.PresupuestoId, idProyecto)
        Next


    End Sub

	Sub GrabarSoloProyecto(ByVal proyecto As PY_Proyectos_Get_Result)
		Dim oProyecto As New Proyecto
		Dim idProyecto As Decimal

		idProyecto = oProyecto.Grabar(proyecto.JobBook, proyecto.Nombre, proyecto.UnidadId, Nothing, proyecto.EstudioId, proyecto.TipoProyectoId, proyecto.id, proyecto.A1, proyecto.A2, proyecto.A3, proyecto.A4, proyecto.A5, proyecto.A6, proyecto.A7)


	End Sub
	Function ObtenerProyecto(ByVal id As Int64) As PY_Proyectos
		Return oMatrixContext.PY_Proyectos.Where(Function(x) x.id = id).FirstOrDefault
	End Function
	Sub GuardarProyecto(ByRef ent As PY_Proyectos)
		oMatrixContext.SaveChanges()
	End Sub
#End Region
#Region "Eliminar"
	Sub eliminar(ByVal presupuestoId As Long, ByVal proyectoId As Long)
        oMatrixContext.PY_Proyecto_Presupuesto_Del(presupuestoId, proyectoId)
    End Sub
#End Region
End Class
