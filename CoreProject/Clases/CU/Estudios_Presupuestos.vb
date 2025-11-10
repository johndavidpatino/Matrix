
'Imports CoreProject.CU_Model

<Serializable()>
Public Class Estudios_Presupuestos
#Region "Variables Globales"
    Private oMatrixContext As CU_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CU_Entities
    End Sub
#End Region
#Region "Grabar"
    Function Grabar(ByVal estudio As CU_Estudios, ByVal PresupuestosAsignados As List(Of CU_Estudios_Presupuestos)) As Int64
        If estudio.id = 0 Then

            For Each i In PresupuestosAsignados
                estudio.CU_Estudios_Presupuestos.Add(New CU_Estudios_Presupuestos With {.PresupuestoId = i.PresupuestoId})
            Next

            oMatrixContext.CU_Estudios.Add(estudio)
            oMatrixContext.SaveChanges()
            Return estudio.id
        Else
            Return 0
        End If
    End Function

    Public Sub GrabarEstudiosPresupuestos(ByVal estudio As Int64, Presupuesto As Int64)
        If oMatrixContext.CU_Estudios_Presupuestos.Where(Function(x) x.PresupuestoId = Presupuesto And x.EstudioId = estudio).ToList.Count = 0 Then
            oMatrixContext.CU_Estudios_Presupuestos.Add(New CU_Estudios_Presupuestos With {.PresupuestoId = Presupuesto, .EstudioId = estudio})
        End If
        oMatrixContext.SaveChanges()
    End Sub
    Sub Grabar(ByVal estudio As CU_Estudios, ByVal PresupuestosEliminados As List(Of CU_Estudios_Presupuestos), ByVal PresupuestosNuevos As List(Of CU_Estudios_Presupuestos))
        Dim originalEstudio As CU_Estudios

        If estudio.id <> 0 Then
            originalEstudio = oMatrixContext.CU_Estudios.Where(Function(x) x.id = estudio.id).FirstOrDefault

            oMatrixContext.Entry(estudio).CurrentValues.SetValues(originalEstudio)


            If Not estudio Is Nothing Then

                For Each i In PresupuestosNuevos
                    oMatrixContext.CU_Estudios_Presupuestos.Add(New CU_Estudios_Presupuestos With {.EstudioId = estudio.id, .PresupuestoId = i.PresupuestoId})
                Next

                For Each i In PresupuestosEliminados
                    oMatrixContext.CU_Estudios_Presupuestos.Remove(oMatrixContext.CU_Estudios_Presupuestos.Where(Function(x) x.PresupuestoId = i.PresupuestoId AndAlso x.EstudioId = estudio.id).FirstOrDefault)
                Next

                oMatrixContext.SaveChanges()
            End If



        End If


    End Sub
#End Region


#Region "Obtener"
    Function ObtenerEstudios() As List(Of CU_Estudios)
        Return oMatrixContext.CU_Estudios.ToList
    End Function
#End Region
End Class
