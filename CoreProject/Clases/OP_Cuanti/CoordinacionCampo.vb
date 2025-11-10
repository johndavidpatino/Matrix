
'Imports CoreProject.OP_Cuanti_Model

<Serializable()>
Public Class CoordinacionCampo
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
    Public Function ObtenerMuestraxEstudioList(ByVal TrabajoId As Int64) As List(Of OP_MuestraTrabajos)
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerMuestraxEstudioYCiudadList(ByVal TrabajoId As Int64, Ciudad As Int32) As List(Of OP_MuestraTrabajos)
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.TrabajoId = TrabajoId And x.CiudadId = Ciudad).ToList
    End Function

    Public Function ObtenerMuestraSinCoordinador() As List(Of OP_MuestraTrabajos)
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.Coordinador Is Nothing).ToList
    End Function
    Public Function ObtenerMuestraxCoordinador(ByVal CoordinadorId As Int64) As List(Of OP_MuestraTrabajos)
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.Coordinador = CoordinadorId).ToList
    End Function

    Public Function ObtenerMuestraSinCoordinadorTipoTecnica(ByVal TipoTecnicaId As Int32) As List(Of OP_MuestraTrabajos)
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.Coordinador Is Nothing AndAlso x.PY_Trabajo.OP_Metodologias.OP_Tecnicas.TecTipo = TipoTecnicaId).ToList
    End Function
    Public Function ObtenerMuestraxCoordinadorTipoTecnica(ByVal CoordinadorId As Int64, ByVal TipoTecnicaId As Int32) As List(Of OP_MuestraTrabajos)
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.Coordinador = CoordinadorId AndAlso x.PY_Trabajo.OP_Metodologias.OP_Tecnicas.TecTipo = TipoTecnicaId).ToList
    End Function

    Public Function ObtenerMuestraxCoordinadoryTrabajo(ByVal CoordinadorId As Int64, TrabajoId As Int64) As List(Of OP_MuestraTrabajos)
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.Coordinador = CoordinadorId And x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerMuestraSinCoordinadorPorTrabajo(ByVal TrabajoId As Int64) As List(Of OP_MuestraTrabajos)
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.Coordinador Is Nothing And x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerMuestraxId(ByVal idMuestra As Int64) As OP_MuestraTrabajos
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.Id = idMuestra).FirstOrDefault
    End Function

    Public Function ObtenerMuestraxTrabajoYCiudad(ByVal TrabajoId As Int64, Ciudad As Int32) As Double
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.TrabajoId = TrabajoId And x.CiudadId = Ciudad).FirstOrDefault.Cantidad
    End Function

    Public Function ObtenerMuestraxEstudio(Muestraid As Int64) As OP_MuestraTrabajos
        Return oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.Id = Muestraid).FirstOrDefault
    End Function

    Public Function ObtenerDepartamentos() As List(Of C_Divipola)
        Return oMatrixContext.C_Divipola.Distinct.ToList
    End Function

    Public Function ObtenerCiudades(ByVal DepartamentoId As Int64) As List(Of C_Divipola)
        Return oMatrixContext.C_Divipola.Where(Function(x) x.DivDeptoMunicipio = DepartamentoId).ToList
    End Function

    Public Function ObtenerEnviosList(ByVal TrabajoId As Int64, ByVal CiudadId As Int32) As List(Of OP_EnvioPaquetesEncuestas)
        Return oMatrixContext.OP_EnvioPaquetesEncuestas.Where(Function(x) x.TrabajoId = TrabajoId And x.CiudadId = CiudadId).ToList
    End Function
    Public Function ObtenerEnvios(ByVal EnvioId As Int64) As OP_EnvioPaquetesEncuestas
        Return oMatrixContext.OP_EnvioPaquetesEncuestas.Where(Function(x) x.id = EnvioId).FirstOrDefault
    End Function
#End Region

#Region "Guardar"
    Public Sub GuardarFechasGenerales(ByVal TrabajoId As Int64, ByVal Finicio As Date, Ffin As Date)
        oMatrixContext.OP_Planeacion_CambioFechasGeneralesCampo(TrabajoId, Finicio, Ffin)
    End Sub
    Public Sub GuardarFechasGeneralesTrabajo(ByVal TrabajoId As Int64)
        oMatrixContext.OP_Planeacion_CambioFechasGeneralesCampoAutomatico(TrabajoId)
    End Sub
    Public Sub GuardarMuestraXEstudio(ByVal Entidad As OP_MuestraTrabajos)
        If oMatrixContext.OP_MuestraTrabajos.Where(Function(x) x.Id = Entidad.Id).ToList.Count = 0 Then
            oMatrixContext.OP_MuestraTrabajos.Add(Entidad)
            oMatrixContext.SaveChanges()
        Else
            Dim E1 As New OP_MuestraTrabajos
            E1 = ObtenerMuestraxId(Entidad.Id)
            E1.Cantidad = Entidad.Cantidad
            E1.Coordinador = Entidad.Coordinador
            E1.CiudadId = Entidad.CiudadId
            E1.TrabajoId = Entidad.TrabajoId
            oMatrixContext.SaveChanges()
        End If

    End Sub

    Public Sub GuardarEnvioPaquetes(ByVal Entidad As OP_EnvioPaquetesEncuestas)
        If oMatrixContext.OP_EnvioPaquetesEncuestas.Where(Function(x) x.id = Entidad.id).ToList.Count = 0 Then
            oMatrixContext.OP_EnvioPaquetesEncuestas.Add(Entidad)
            oMatrixContext.SaveChanges()
        Else
            Dim E1 As New OP_EnvioPaquetesEncuestas
            E1 = ObtenerEnvios(Entidad.id)
            E1.TrabajoId = Entidad.TrabajoId
            E1.CiudadId = Entidad.CiudadId
            E1.Guia = Entidad.Guia
            E1.Courier = Entidad.Courier
            E1.FechaEnvio = Entidad.FechaEnvio
            E1.Cantidad = Entidad.Cantidad
            E1.FechaRegistro = Entidad.FechaRegistro
            E1.Usuario = Entidad.Usuario
            oMatrixContext.SaveChanges()
        End If

    End Sub

    Public Sub ActualizarMuestra(ByRef Entidad As OP_MuestraTrabajos)
        oMatrixContext.SaveChanges()
    End Sub
#End Region

#Region "Eliminar"
    Public Sub EliminarMuestraXEstudio(ByVal id As Int64)
        oMatrixContext.OP_MuestraTrabajos.Remove(ObtenerMuestraxEstudio(id))
        oMatrixContext.SaveChanges()
    End Sub
    Public Sub EliminarEnvioPaquete(ByVal id As Int64)
        oMatrixContext.OP_EnvioPaquetesEncuestas.Remove(ObtenerEnvios(id))
        oMatrixContext.SaveChanges()
    End Sub
#End Region
End Class
