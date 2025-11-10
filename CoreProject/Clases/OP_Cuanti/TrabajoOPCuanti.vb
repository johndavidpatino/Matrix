
'Imports CoreProject.OP_Cuanti_Model
<Serializable()>
Public Class TrabajoOPCuanti
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
    Public Function ObtenerTrabajoConfiguracion(ByVal TrabajoId As Int64) As OP_TrabajoConfiguracion
        Try
            Return oMatrixContext.OP_TrabajoConfiguracion.Where(Function(x) x.TrabajoId = TrabajoId).FirstOrDefault
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function ObtenerTrabajo(ByVal TrabajoId As Int64) As PY_Trabajo
        Try
            Return oMatrixContext.PY_Trabajo.Where(Function(x) x.id = TrabajoId).FirstOrDefault
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function ObtenerUnidadCritica(ByVal TrabajoId As Int64) As Int64
        Return ObtenerTrabajoConfiguracion(TrabajoId).UnidadCritica
    End Function
    Public Function GuardarTrabajoConfiguracion(E As OP_TrabajoConfiguracion) As OP_TrabajoConfiguracion
        If oMatrixContext.OP_TrabajoConfiguracion.Where(Function(x) x.TrabajoId = E.TrabajoId).ToList.Count = 0 Then
            E.PorcentajeVerificacion = 20
            oMatrixContext.OP_TrabajoConfiguracion.Add(E)
            oMatrixContext.SaveChanges()
            Return E
        Else
            Dim E1 As OP_TrabajoConfiguracion = ObtenerTrabajoConfiguracion(E.TrabajoId)
            E1.UnidadCritica = E.UnidadCritica
            If Not E.FechaFinalCampo Is Nothing Then E1.FechaFinalCampo = E.FechaFinalCampo
            If Not E.FechaInicioCampo Is Nothing Then E1.FechaInicioCampo = E.FechaInicioCampo
            E1.PorcentajeVerificacion = 20
            oMatrixContext.SaveChanges()
            Return E
        End If
    End Function

    Public Function ObtenerTecnicasRecoleccion() As List(Of OP_TipoRecoleccion)
        Return oMatrixContext.OP_TipoRecoleccion.ToList
    End Function

    Public Sub GuardarTipoRecoleccion(ByVal TrabajoId As Int64, TipoRecoleccion As Int16)
        Dim EntTraba As PY_Trabajo = ObtenerTrabajo(TrabajoId)
        EntTraba.TipoRecoleccionId = TipoRecoleccion
        oMatrixContext.SaveChanges()
    End Sub
    Public Function ObtenerOp_MuestraTrabajo(ByVal IdTrabajo As Int64) As List(Of Op_MuestraTrabajosGet_Result)
        Return oMatrixContext.Op_MuestraTrabajosGet(IdTrabajo).ToList
    End Function

    Public Function ObtenerCCProduccionPST(ByVal id As Int64) As CC_ProduccionCargaPST
        Return oMatrixContext.CC_ProduccionCargaPST.Where(Function(x) x.id = id).FirstOrDefault()
    End Function



#End Region

    Public Sub SaveChangesContext()
        oMatrixContext.SaveChanges()
    End Sub
End Class
