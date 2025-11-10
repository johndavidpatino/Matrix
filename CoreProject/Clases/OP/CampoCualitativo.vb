
<Serializable()>
Public Class CampoCualitativo
#Region "Variables Globales"
    Private oMatrixContext As PY_CualiEntities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New PY_CualiEntities
    End Sub
#End Region

#Region "Obtener"
    Public Function ObtenerCampoCualiXId(ByVal id As Int64) As OP_CampoCuali
        Return oMatrixContext.OP_CampoCuali.Where(Function(x) x.id = id).FirstOrDefault
    End Function

    Public Function ObtenerCampoCualiList(ByVal SegmentoId As Int64) As List(Of OP_CampoCuali_Get_Result)
        Return oMatrixContext.OP_CampoCuali_Get(SegmentoId).ToList
    End Function

    Public Function ObtenerListadoCampoCuali(ByVal SegmentoId As Int64) As List(Of OP_ListadoCampoCualiXSegmento_Result)
        Return oMatrixContext.OP_ListadoCampoCualiXSegmento(SegmentoId).ToList
    End Function
    Public Function ObtenerModeradores() As List(Of US_UsuariosModeradoresCualitativos_Result)
        Return oMatrixContext.US_UsuariosModeradoresCualitativos().ToList
    End Function

    Public Function ObtenerReclutadores() As List(Of US_UsuariosReclutadoresCualitativos_Result)
        Return oMatrixContext.US_UsuariosReclutadoresCualitativos().ToList
    End Function

    Public Function ObtenerTranscriptores() As List(Of US_UsuariosTranscriptoresCualitativos_Result)
        Return oMatrixContext.US_UsuariosTranscriptoresCualitativos().ToList
    End Function

    Public Function ObtenerModeradorxId(ByVal id As Int64) As PY_SegmentosCuali_Moderadores
        Return oMatrixContext.PY_SegmentosCuali_Moderadores.Where(Function(x) x.id = id).FirstOrDefault
    End Function

    Public Function ObtenerReclutadorxId(ByVal id As Int64) As PY_SegmentosCuali_Reclutadores
        Return oMatrixContext.PY_SegmentosCuali_Reclutadores.Where(Function(x) x.id = id).FirstOrDefault
    End Function

    Public Function ObtenerModeradoresList(ByVal SegmentoId As Int64?) As List(Of PY_SegmentosCuali_Moderadores_Get_Result)
        Return oMatrixContext.PY_SegmentosCuali_Moderadores_Get(SegmentoId).ToList
    End Function

    Public Function ObtenerReclutadoresList(ByVal SegmentoId As Int64?) As List(Of PY_SegmentosCuali_Reclutadores_Get_Result)
        Return oMatrixContext.PY_SegmentosCuali_Reclutadores_Get(SegmentoId).ToList
    End Function

    'OP_MuestrasTrabajosCuali Inicio
    Public Function ObtenerMuestraCualixEstudioList(ByVal TrabajoId As Int64) As List(Of OP_MuestraTrabajosCuali)
        Return oMatrixContext.OP_MuestraTrabajosCuali.Where(Function(x) x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerMuestraCualixEstudioYCiudadList(ByVal TrabajoId As Int64, Ciudad As Int32) As List(Of OP_MuestraTrabajosCuali)
        Return oMatrixContext.OP_MuestraTrabajosCuali.Where(Function(x) x.TrabajoId = TrabajoId And x.CiudadId = Ciudad).ToList
    End Function

    Public Function ObtenerMuestraCualixId(ByVal idMuestra As Int64) As OP_MuestraTrabajosCuali
        Return oMatrixContext.OP_MuestraTrabajosCuali.Where(Function(x) x.Id = idMuestra).FirstOrDefault
    End Function

    'OP_MuestrasTrabajosCuali_Entrevistas Inicio
    Public Function ObtenerMuestraCualiEntrevistasxEstudioList(ByVal TrabajoId As Int64) As List(Of OP_MuestraTrabajosCuali_Entrevistas)
        Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerMuestraCualiEntrevistasxEstudioYCiudadList(ByVal TrabajoId As Int64, Ciudad As Int32) As List(Of OP_MuestraTrabajosCuali_Entrevistas)
        Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.TrabajoId = TrabajoId And x.CiudadId = Ciudad).ToList
    End Function

    Public Function ObtenerMuestraCualiEntrevistasSinModerador() As List(Of OP_MuestraTrabajosCuali_Entrevistas)
        Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.Moderador Is Nothing).ToList
    End Function
    Public Function ObtenerMuestraCualiEntrevistasxModerador(ByVal ModeradorId As Int64) As List(Of OP_MuestraTrabajosCuali_Entrevistas)
        Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.Moderador = ModeradorId).ToList
    End Function

    'Public Function ObtenerMuestraCualiEntrevistasSinModeradorTipoTecnica(ByVal TipoTecnicaId As Int32) As List(Of OP_MuestraTrabajosCuali_Entrevistas)
    '    Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.Moderador Is Nothing AndAlso x.PY_Trabajo.OP_Metodologias.OP_Tecnicas.TecTipo = TipoTecnicaId).ToList
    'End Function
    'Public Function ObtenerMuestraCualiEntrevistasxModeradorTipoTecnica(ByVal ModeradorId As Int64, ByVal TipoTecnicaId As Int32) As List(Of OP_MuestraTrabajosCuali_Entrevistas)
    '    Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.Moderador = ModeradorId AndAlso x.PY_Trabajo.OP_Metodologias.OP_Tecnicas.TecTipo = TipoTecnicaId).ToList
    'End Function

    Public Function ObtenerMuestraCualiEntrevistasxModeradoryTrabajo(ByVal ModeradorId As Int64, TrabajoId As Int64) As List(Of OP_MuestraTrabajosCuali_Entrevistas)
        Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.Moderador = ModeradorId And x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerMuestraCualiEntrevistasSinModeradorPorTrabajo(ByVal TrabajoId As Int64) As List(Of OP_MuestraTrabajosCuali_Entrevistas)
        Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.Moderador Is Nothing And x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerMuestraCualiEntrevistasxId(ByVal idMuestra As Int64) As OP_MuestraTrabajosCuali_Entrevistas
        Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.Id = idMuestra).FirstOrDefault
    End Function

    Public Function ObtenerMuestraCualiSesionxId(ByVal idMuestra As Int64) As OP_MuestraTrabajosCuali_Sesiones
        Return oMatrixContext.OP_MuestraTrabajosCuali_Sesiones.Where(Function(x) x.Id = idMuestra).FirstOrDefault
    End Function

    Public Function ObtenerMuestraCualiInHomexId(ByVal idMuestra As Int64) As OP_MuestraTrabajosCuali_InHome
        Return oMatrixContext.OP_MuestraTrabajosCuali_InHome.Where(Function(x) x.Id = idMuestra).FirstOrDefault
    End Function

    Public Function ObtenerMuestraCualiEntrevistasxTrabajoYCiudad(ByVal TrabajoId As Int64, Ciudad As Int32) As Double
        Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.TrabajoId = TrabajoId And x.CiudadId = Ciudad).FirstOrDefault.Cantidad
    End Function

    Public Function ObtenerMuestraCualiEntrevistasxEstudio(Muestraid As Int64) As OP_MuestraTrabajosCuali_Entrevistas
        Return oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.Id = Muestraid).FirstOrDefault
    End Function

    Public Function ObtenerEntrevistasxTrabajo(ByVal Trabajoid As Int64?) As List(Of OP_MuestraTrabajosCuali_EntrevistasGet_Result)
        Return oMatrixContext.OP_MuestraTrabajosCuali_EntrevistasGet(Nothing, Trabajoid).ToList
    End Function

    Public Function ObtenerEntrevistasxID(ByVal Id As Int64) As List(Of OP_MuestraTrabajosCuali_EntrevistasGet_Result)
        Return oMatrixContext.OP_MuestraTrabajosCuali_EntrevistasGet(Id, Nothing).ToList
    End Function

    Public Function ObtenerEntrevistasDistribucionxId(Id As Int64) As OP_EntrevistasCuali_Distribucion
        Return oMatrixContext.OP_EntrevistasCuali_Distribucion.Where(Function(x) x.Id = Id).FirstOrDefault
    End Function

    Public Function ObtenerEntrevistasDistribucionxTrabajo(ByVal IdTrabajo As Int64?) As List(Of OP_EntrevistasCuali_DistribucionGet_Result)
        Return oMatrixContext.OP_EntrevistasCuali_DistribucionGet(Nothing, Nothing, IdTrabajo).ToList
    End Function

    Public Function ObtenerEntrevistasDistribucionxIdEntrevista(ByVal IdEntrevista As Int64) As List(Of OP_EntrevistasCuali_DistribucionGet_Result)
        Return oMatrixContext.OP_EntrevistasCuali_DistribucionGet(Nothing, IdEntrevista, Nothing).ToList
    End Function

    Public Function ObtenerEntrevistasDistribucionxIdDistribucion(ByVal Id As Int64) As List(Of OP_EntrevistasCuali_DistribucionGet_Result)
        Return oMatrixContext.OP_EntrevistasCuali_DistribucionGet(Id, Nothing, Nothing).ToList
    End Function

    Public Function ObtenerMuestraCualiEntrevistasxSesion(IdSesion As Int64) As OP_MuestraTrabajosCuali_Sesiones
        Return oMatrixContext.OP_MuestraTrabajosCuali_Sesiones.Where(Function(x) x.Id = IdSesion).FirstOrDefault
    End Function

    Public Function ObtenerSesionesxTrabajo(ByVal Trabajoid As Int64?) As List(Of OP_MuestraTrabajosCuali_SesionesGet_Result)
        Return oMatrixContext.OP_MuestraTrabajosCuali_SesionesGet(Nothing, Trabajoid).ToList
    End Function

    Public Function ObtenerSesionesxID(ByVal Id As Int64) As List(Of OP_MuestraTrabajosCuali_SesionesGet_Result)
        Return oMatrixContext.OP_MuestraTrabajosCuali_SesionesGet(Id, Nothing).ToList
    End Function

    Public Function ObtenerMuestraCualiEntrevistasxInHome(IdSesion As Int64) As OP_MuestraTrabajosCuali_InHome
        Return oMatrixContext.OP_MuestraTrabajosCuali_InHome.Where(Function(x) x.Id = IdSesion).FirstOrDefault
    End Function

    Public Function ObtenerInHomexTrabajo(ByVal Trabajoid As Int64?) As List(Of OP_MuestraTrabajosCuali_InHomeGet_Result)
        Return oMatrixContext.OP_MuestraTrabajosCuali_InHomeGet(Nothing, Trabajoid).ToList
    End Function

    Public Function ObtenerInHomexID(ByVal Id As Int64) As List(Of OP_MuestraTrabajosCuali_InHomeGet_Result)
        Return oMatrixContext.OP_MuestraTrabajosCuali_InHomeGet(Id, Nothing).ToList
    End Function

    Public Function ObtenerLogEntrevistas(ByVal IdDistribucion As Int64) As List(Of OP_LogEntrevistasCuali_Get_Result)
        Return oMatrixContext.OP_LogEntrevistasCuali_Get(IdDistribucion).ToList
    End Function
    Public Function ObtenerLogSesiones(ByVal IdSesion As Int64) As List(Of OP_LogSesionesCuali_Get_Result)
        Return oMatrixContext.OP_LogSesionesCuali_Get(IdSesion).ToList
    End Function

    Public Function ObtenerLogInHome(ByVal IdSesion As Int64) As List(Of OP_LogInHomeCuali_Get_Result)
        Return oMatrixContext.OP_LogInHomeCuali_Get(IdSesion).ToList
    End Function

    Public Function ObtenerTipoPreguntaFiltro() As List(Of OP_TipoPregunta_Filtro)
        Return oMatrixContext.OP_TipoPregunta_Filtro.ToList()
    End Function

    Public Function ObtenerFiltroxId(ByVal Id As Int64) As OP_Filtros
        Return oMatrixContext.OP_Filtros.Where(Function(x) x.Id = Id).FirstOrDefault
    End Function

    Public Function ObtenerFiltroxTrabajoYTipo(ByVal IdTrabajo As Int64, ByVal TipoFiltro As Int32) As OP_Filtros
        Return oMatrixContext.OP_Filtros.Where(Function(x) x.IdTrabajo = IdTrabajo And x.TipoFiltro = TipoFiltro).FirstOrDefault
    End Function

    Public Function ObtenerPreguntasFitroxId(ByVal Id As Int64) As OP_Preguntas_Filtro
        Return oMatrixContext.OP_Preguntas_Filtro.Where(Function(x) x.Id = Id).FirstOrDefault
    End Function

    Public Function ObtenerListaPreguntasFiltro(ByVal IdFiltro As Int64?, ByVal TipoFiltro As Int32?, ByVal IdTrabajo As Int64?) As List(Of OP_Preguntas_Filtro_Get_Result)
        Return oMatrixContext.OP_Preguntas_Filtro_Get(IdFiltro, TipoFiltro, IdTrabajo).ToList()
    End Function

    Public Function ObtenerListaFiltros(ByVal IdFiltro As Int64?, ByVal TipoFiltro As Int32?, ByVal IdTrabajo As Int64?) As List(Of OP_Filtros_Get_Result)
        Return oMatrixContext.OP_Filtros_Get(IdFiltro, TipoFiltro, IdTrabajo).ToList()
    End Function

    Public Function ObtenerRespuestasMaestroFiltroReclutamiento(ByVal IdTrabajo As Int64?, ByVal Cedula As Int64?) As OP_Respuestas_FiltroReclutamiento_Maestro_Get_Result
        Return oMatrixContext.OP_Respuestas_FiltroReclutamiento_Maestro_Get(IdTrabajo, Cedula).FirstOrDefault
    End Function
    Public Function ObtenerRespuestasFiltroMaestroxId(ByVal Id As Int64) As OP_Respuestas_Filtro_Maestro
        Return oMatrixContext.OP_Respuestas_Filtro_Maestro.Where(Function(x) x.Id = Id).FirstOrDefault
    End Function
    Public Function ObtenerRespuestasFiltroMaestro(ByVal idFiltro As Int64?) As List(Of OP_Respuestas_Filtro_Maestro_Get_Result)
        Return oMatrixContext.OP_Respuestas_Filtro_Maestro_Get(idFiltro).ToList()
    End Function

    Public Function ObtenerRespuestasFiltroDetalle(ByVal idRespuesta As Int64?) As List(Of OP_Respuestas_Filtro_Detalle_Get_Result)
        Return oMatrixContext.OP_Respuestas_Filtro_Detalle_Get(idRespuesta).ToList()
    End Function

    Public Function ObtenerCiudadxPais(ByVal Ciudad As Int64?, ByVal Pais As Int64?) As List(Of TH_CiudadxPaisGet_Result)
        Dim oMatrixContext2 = New TH_Entities
        Return oMatrixContext2.TH_CiudadxPaisGet(Ciudad, Pais).ToList
    End Function

    Public Function ObtenerPais(ByVal Ciudad As Int64?, ByVal Pais As Int64?) As List(Of TH_PaisGet_Result)
        Dim oMatrixContext2 = New TH_Entities
        Return oMatrixContext2.TH_PaisGet(Ciudad, Pais).ToList
    End Function

    Public Function ObtenerProgramadosEntrevistadosCuali(Id As Int64?, TrabajoId As Int64?, EntrevistadoId As Int64?, Estado As Int64?, Fecha As Date?, Nombre As String, Documento As Int64?, Ciudad As String, Edad As Int64?, EstadoCivil As String, Genero As String, Estrato As Int64?, Barrio As String, NivelEscolaridad As String) As List(Of OP_Programados_Entrevistados_Cuali_Get_Result)
        Dim oMatrixContext1 = New OP_Entities
        Return oMatrixContext1.OP_Programados_Entrevistados_Cuali_Get(Id, TrabajoId, EntrevistadoId, Estado, Fecha, Nombre, Documento, Ciudad, Edad, EstadoCivil, Genero, Estrato, Barrio, NivelEscolaridad).ToList
    End Function

    Public Function ObtenerObsEntrevistadosCuali(TrabajoId As Int64?, EntrevistadoId As Int64?) As List(Of String)
        Dim oMatrixContext1 = New OP_Entities
        Return oMatrixContext1.OP_Obs_Entrevistados_Cuali_Get(TrabajoId, EntrevistadoId).ToList
    End Function

    Public Function ObtenerProgramadosEntrevistadosCualiXId(Id As Int64?) As OP_Programacion_Campo_Cuali
        If Id = 0 Then
            Return New OP_Programacion_Campo_Cuali
        Else
            Dim oMatrixContext1 = New OP_Entities
            If oMatrixContext1.OP_Programacion_Campo_Cuali.Where(Function(x) x.id = Id).ToList.Count > 0 Then
                Return oMatrixContext1.OP_Programacion_Campo_Cuali.Where(Function(x) x.id = Id).FirstOrDefault
            Else
                Return New OP_Programacion_Campo_Cuali
            End If
        End If
    End Function

    Public Function obtenerObservacionesCualixTrabajoxEntrevistado(idTrabajo As Int64?, Entrevistado As Int64?) As List(Of OP_Observaciones_Programacion_Campo_Cuali)
        If idTrabajo = 0 Or Entrevistado = 0 Then
            Return New List(Of OP_Observaciones_Programacion_Campo_Cuali)
        Else
            Dim oMatrixContext1 = New OP_Entities
            If oMatrixContext1.OP_Observaciones_Programacion_Campo_Cuali.Where(Function(x) x.TrabajoId = idTrabajo And x.EntrevistadoId = Entrevistado).ToList.Count > 0 Then
                Return oMatrixContext1.OP_Observaciones_Programacion_Campo_Cuali.Where(Function(x) x.TrabajoId = idTrabajo And x.EntrevistadoId = Entrevistado).ToList
            Else
                Return New List(Of OP_Observaciones_Programacion_Campo_Cuali)
            End If
        End If
    End Function
    Public Function obtenerModeradoresCuali() As List(Of OP_Moderadores_Cuali)
        Dim oMatrixContext1 = New OP_Entities
        If oMatrixContext1.OP_Moderadores_Cuali.ToList.Count > 0 Then
            Return oMatrixContext1.OP_Moderadores_Cuali.ToList.ToList
        Else
            Return New List(Of OP_Moderadores_Cuali)
        End If
    End Function
    Public Function obtenerReclutadoresCuali() As List(Of OP_Reclutadores_Cuali)
        Dim oMatrixContext1 = New OP_Entities
        If oMatrixContext1.OP_Reclutadores_Cuali.ToList.Count > 0 Then
            Return oMatrixContext1.OP_Reclutadores_Cuali.ToList.ToList
        Else
            Return New List(Of OP_Reclutadores_Cuali)
        End If
    End Function

    Public Function obtenerEntrevistadosCualixId(ByVal id As Int64) As OP_Entrevistados_Cuali_Get_Result
        Dim oMatrixContext1 = New OP_Entities
        Dim entrevistados = oMatrixContext1.OP_Entrevistados_Cuali_Get(id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
        If Not entrevistados Is Nothing Then
            Return entrevistados
        Else
            Return New OP_Entrevistados_Cuali_Get_Result
        End If
    End Function


#End Region
#Region "Guardar"
    Public Function GuardarCampo(ByVal Ent As OP_CampoCuali) As Int64
        If Ent.id = 0 Then
            oMatrixContext.OP_CampoCuali.Add(Ent)
            oMatrixContext.SaveChanges()
            Return Ent.id
        Else
            Dim e As New OP_CampoCuali
            e = ObtenerCampoCualiXId(Ent.id)
            e.id = Ent.id
            e.SegmentoId = Ent.SegmentoId
            e.Moderador = Ent.Moderador
            e.PlaneacionPor = Ent.PlaneacionPor
            e.Lugar = Ent.Lugar
            e.PersonaContaco = Ent.PersonaContaco
            e.DatosContacto = Ent.DatosContacto
            e.Direccion = Ent.Direccion
            e.Fecha = Ent.Fecha
            e.Hora = Ent.Hora
            e.ObservacionesPrevias = Ent.ObservacionesPrevias
            e.FechaReal = Ent.FechaReal
            e.HoraReal = Ent.HoraReal
            e.Asistentes = Ent.Asistentes
            e.AsistentesNoCumplen = Ent.AsistentesNoCumplen
            e.ObservacionesEjecucion = Ent.ObservacionesEjecucion
            e.EjecucionPor = Ent.EjecucionPor
            e.Caida = Ent.Caida
            e.Cancelada = Ent.Cancelada
            e.Ejecutada = Ent.Ejecutada
            oMatrixContext.SaveChanges()
            Return e.id
        End If
    End Function
    Public Function Duplicar(ByVal idCampo As Int64) As Decimal
        Return oMatrixContext.OP_CampoCualiDuplicar(idCampo)(0).Value
    End Function

    Public Function GuardarModerador(ByVal Ent As PY_SegmentosCuali_Moderadores) As Int64
        If Ent.id = 0 Then
            oMatrixContext.PY_SegmentosCuali_Moderadores.Add(Ent)
            oMatrixContext.SaveChanges()
            Return Ent.id
        Else
            Dim e As New PY_SegmentosCuali_Moderadores
            e = ObtenerModeradorxId(Ent.id)
            e.id = Ent.id
            e.TrabajoId = Ent.TrabajoId
            e.SegmentoId = Ent.SegmentoId
            e.Moderador = Ent.Moderador
            e.FechaAsignacion = Ent.FechaAsignacion
            oMatrixContext.SaveChanges()
            Return e.id
        End If
    End Function

    Public Function GuardarReclutador(ByVal Ent As PY_SegmentosCuali_Reclutadores) As Int64
        If Ent.id = 0 Then
            oMatrixContext.PY_SegmentosCuali_Reclutadores.Add(Ent)
            oMatrixContext.SaveChanges()
            Return Ent.id
        Else
            Dim e As New PY_SegmentosCuali_Reclutadores
            e = ObtenerReclutadorxId(Ent.id)
            e.id = Ent.id
            e.TrabajoId = Ent.TrabajoId
            e.SegmentoId = Ent.SegmentoId
            e.Reclutador = Ent.Reclutador
            e.FechaAsignacion = Ent.FechaAsignacion
            oMatrixContext.SaveChanges()
            Return e.id
        End If
    End Function

    Public Function GuardarReclutadorCuali(ByVal Ent As OP_Reclutadores_Cuali) As Int64
        Dim oMatrixContext1 = New OP_Entities
        If Ent.id = 0 Then
            oMatrixContext1.OP_Reclutadores_Cuali.Add(Ent)
            oMatrixContext1.SaveChanges()
            Return Ent.id
        Else
            Dim e As New OP_Reclutadores_Cuali
            e = obtenerReclutadoresCuali(Ent.id)
            e.id = Ent.id
            e.Nombre = Ent.Nombre
            oMatrixContext.SaveChanges()
            Return e.id
        End If
    End Function

    Public Function GuardarModeradorCuali(ByVal Ent As OP_Moderadores_Cuali) As Int64
        Dim oMatrixContext1 = New OP_Entities
        If Ent.id = 0 Then
            oMatrixContext1.OP_Moderadores_Cuali.Add(Ent)
            oMatrixContext1.SaveChanges()
            Return Ent.id
        Else
            Dim e As New OP_Moderadores_Cuali
            e = obtenerModeradoresCuali(Ent.id)
            e.id = Ent.id
            e.Nombre = Ent.Nombre
            oMatrixContext.SaveChanges()
            Return e.id
        End If
    End Function

    Public Sub GuardarMuestraXEstudio(ByVal Entidad As OP_MuestraTrabajosCuali_Entrevistas)
        If oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Where(Function(x) x.Id = Entidad.Id).ToList.Count = 0 Then
            oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Add(Entidad)
            oMatrixContext.SaveChanges()
        Else
            Dim E1 As New OP_MuestraTrabajosCuali_Entrevistas
            E1 = ObtenerMuestraCualiEntrevistasxId(Entidad.Id)
            E1.Descripcion = Entidad.Descripcion
            E1.CiudadId = Entidad.CiudadId
            E1.TrabajoId = Entidad.TrabajoId
            E1.Cantidad = Entidad.Cantidad
            E1.Moderador = Entidad.Moderador
            E1.FechaInicio = Entidad.FechaInicio
            E1.FechaFin = Entidad.FechaFin
            oMatrixContext.SaveChanges()
        End If
    End Sub

    Public Sub GuardarMuestra(ByVal Entidad As OP_MuestraTrabajosCuali)
        If oMatrixContext.OP_MuestraTrabajosCuali.Where(Function(x) x.Id = Entidad.Id).ToList.Count = 0 Then
            oMatrixContext.OP_MuestraTrabajosCuali.Add(Entidad)
            oMatrixContext.SaveChanges()
        Else
            Dim E1 As New OP_MuestraTrabajosCuali
            E1 = ObtenerMuestraCualixId(Entidad.Id)
            E1.CiudadId = Entidad.CiudadId
            E1.TrabajoId = Entidad.TrabajoId
            E1.Cantidad = Entidad.Cantidad
            E1.FechaInicio = Entidad.FechaInicio
            E1.FechaFin = Entidad.FechaFin
            E1.NumeroBackup = Entidad.NumeroBackup
            oMatrixContext.SaveChanges()
        End If
    End Sub

    Public Sub GuardarDistribucionEntrevistas_Log(ByVal Cantidad As Int32?, ByVal IdEntrevista As Int64?, ByVal TrabajoId As Int64?, ByVal GrupoObjetivo As String, ByVal CiudadId As Int32?, ByVal FechaInicio As Date?, ByVal FechaFin As Date?, ByVal Moderador As Int64?, ByVal Usuario As Int64?)
        oMatrixContext.OP_EntrevistasCuali_Distribucion_Add(Cantidad, IdEntrevista, TrabajoId, GrupoObjetivo, CiudadId, FechaInicio, FechaFin, Moderador, Usuario)
    End Sub

    Public Function GuardarDistribucionEntrevistas(ByRef ent As OP_EntrevistasCuali_Distribucion)
        Dim e As New OP_EntrevistasCuali_Distribucion

        If Not (ent.Id = 0) Then
            e = ObtenerEntrevistasDistribucionxId(ent.Id)
        End If
        e.IdEntrevista = ent.IdEntrevista
        e.Numero = ent.Numero
        e.TrabajoId = ent.TrabajoId
        e.GrupoObjetivo = ent.GrupoObjetivo
        e.CiudadId = ent.CiudadId
        e.Cantidad = ent.Cantidad
        e.FechaInicio = ent.FechaInicio
        e.FechaFin = ent.FechaFin
        e.Moderador = ent.Moderador
        e.Estado = ent.Estado
        If ent.Id = 0 Then
            oMatrixContext.OP_EntrevistasCuali_Distribucion.Add(e)
        End If
        oMatrixContext.SaveChanges()
        Return e.Id
    End Function


    Public Function GuardarMuestraXSesiones(ByRef ent As OP_MuestraTrabajosCuali_Sesiones)
        Dim e As New OP_MuestraTrabajosCuali_Sesiones

        If Not (ent.Id = 0) Then
            e = ObtenerMuestraCualiSesionxId(ent.Id)
        End If
        e.Numero = ent.Numero
        e.TrabajoId = ent.TrabajoId
        e.CiudadId = ent.CiudadId
        e.Cantidad = ent.Cantidad
        e.Fecha = ent.Fecha
        e.Hora = ent.Hora
        e.Moderador = ent.Moderador
        e.GrupoObjetivo = ent.GrupoObjetivo
        e.Caracteristicas = ent.Caracteristicas
        e.Estado = ent.Estado
        If ent.Id = 0 Then
            oMatrixContext.OP_MuestraTrabajosCuali_Sesiones.Add(e)
        End If
        oMatrixContext.SaveChanges()
        Return e.Id
    End Function

    Public Function GuardarMuestraXInHome(ByRef ent As OP_MuestraTrabajosCuali_InHome)
        Dim e As New OP_MuestraTrabajosCuali_InHome

        If Not (ent.Id = 0) Then
            e = ObtenerMuestraCualiInHomexId(ent.Id)
        End If
        e.Numero = ent.Numero
        e.TrabajoId = ent.TrabajoId
        e.CiudadId = ent.CiudadId
        e.Cantidad = ent.Cantidad
        e.Fecha = ent.Fecha
        e.Hora = ent.Hora
        e.Moderador = ent.Moderador
        e.GrupoObjetivo = ent.GrupoObjetivo
        e.Caracteristicas = ent.Caracteristicas
        e.Estado = ent.Estado
        If ent.Id = 0 Then
            oMatrixContext.OP_MuestraTrabajosCuali_InHome.Add(e)
        End If
        oMatrixContext.SaveChanges()
        Return e.Id
    End Function

    Public Sub GuardarLogEntrevistas(ByVal Entidad As OP_LogEntrevistasCuali)
        If oMatrixContext.OP_LogEntrevistasCuali.Where(Function(x) x.Id = Entidad.Id).ToList.Count = 0 Then
            oMatrixContext.OP_LogEntrevistasCuali.Add(Entidad)
            oMatrixContext.SaveChanges()
        End If
    End Sub

    Public Sub GuardarLogSesiones(ByVal Entidad As OP_LogSesionesCuali)
        If oMatrixContext.OP_LogSesionesCuali.Where(Function(x) x.Id = Entidad.Id).ToList.Count = 0 Then
            oMatrixContext.OP_LogSesionesCuali.Add(Entidad)
            oMatrixContext.SaveChanges()
        End If
    End Sub

    Public Sub GuardarLogInHome(ByVal Entidad As OP_LogInHomeCuali)
        If oMatrixContext.OP_LogInHomeCuali.Where(Function(x) x.Id = Entidad.Id).ToList.Count = 0 Then
            oMatrixContext.OP_LogInHomeCuali.Add(Entidad)
            oMatrixContext.SaveChanges()
        End If
    End Sub

    Public Sub GuardarProgramacionCampo(ByVal ProgramacionCampo As OP_Programacion_Campo_Cuali)
        Dim oMatrixContext1 = New OP_Entities
        If oMatrixContext1.OP_Programacion_Campo_Cuali.Where(Function(x) x.id = ProgramacionCampo.id).ToList.Count = 0 Then
            oMatrixContext1.OP_Programacion_Campo_Cuali.Add(ProgramacionCampo)
            oMatrixContext1.SaveChanges()
        Else
            Dim Campo = oMatrixContext1.OP_Programacion_Campo_Cuali.Where(Function(x) x.id = ProgramacionCampo.id).FirstOrDefault
            Campo.id = ProgramacionCampo.id
            Campo.TrabajoId = ProgramacionCampo.TrabajoId
            Campo.EntrevistadoId = ProgramacionCampo.EntrevistadoId
            Campo.Reclutador = ProgramacionCampo.Reclutador
            Campo.Audio = ProgramacionCampo.Audio
            Campo.Transcripcion = ProgramacionCampo.Transcripcion
            Campo.Estado = ProgramacionCampo.Estado
            Campo.Moderador = ProgramacionCampo.Moderador
            Campo.Fecha = ProgramacionCampo.Fecha
            Campo.Hora = ProgramacionCampo.Hora
            Campo.Filtro = ProgramacionCampo.Filtro
            Campo.Observaciones = ProgramacionCampo.Observaciones

            oMatrixContext1.SaveChanges()
        End If
    End Sub

    Public Sub GuardarLogProgramacionCampo(ByVal Entrevistados As OP_Log_Programacion_Campo_Cuali)
        Dim oMatrixContext1 = New OP_Entities
        If oMatrixContext1.OP_Programacion_Campo_Cuali.Where(Function(x) x.id = Entrevistados.id).ToList.Count = 0 Then
            oMatrixContext1.OP_Log_Programacion_Campo_Cuali.Add(Entrevistados)
        End If
        oMatrixContext1.SaveChanges()
    End Sub

    Public Sub ActualizarMuestraTrabajosCuali_Entrevistas(ByRef Entidad As OP_MuestraTrabajosCuali_Entrevistas)
        oMatrixContext.SaveChanges()
    End Sub

    Public Sub ActualizarEntrevistasDistribucion(ByRef Entidad As OP_EntrevistasCuali_Distribucion)
        oMatrixContext.SaveChanges()
    End Sub

    Public Sub ActualizarMuestraTrabajosCuali_Sesiones(ByRef Entidad As OP_MuestraTrabajosCuali_Sesiones)
        oMatrixContext.SaveChanges()
    End Sub

    Public Sub ActualizarMuestraTrabajosCuali_InHome(ByRef Entidad As OP_MuestraTrabajosCuali_InHome)
        oMatrixContext.SaveChanges()
    End Sub

    Public Sub ActualizarOrdenPreguntasFiltro(ByVal IdFiltro As Int64?, ByVal OrdenPregunta As Int32?)
        oMatrixContext.OP_Preguntas_Filtro_ActualizarOrden(IdFiltro, OrdenPregunta)
    End Sub

    Public Sub ReOrdenarPreguntasFiltro(ByVal IdFiltro As Int64?, ByVal OrdenAnterior As Int32?, ByVal OrdenActual As Int32?)
        oMatrixContext.OP_Preguntas_Filtro_ReOrdenar(IdFiltro, OrdenAnterior, OrdenActual)
    End Sub

    Public Function GuardarFiltros(ByRef ent As OP_Filtros)
        Dim e As New OP_Filtros

        If Not (ent.Id = 0) Then
            e = ObtenerFiltroxId(ent.Id)
        End If
        e.TipoFiltro = ent.TipoFiltro
        e.IdTrabajo = ent.IdTrabajo
        e.FechaInicio = ent.FechaInicio
        e.FechaFin = ent.FechaFin
        e.Activo = ent.Activo
        If ent.Id = 0 Then
            oMatrixContext.OP_Filtros.Add(e)
        End If
        oMatrixContext.SaveChanges()
        Return e.Id
    End Function

    Public Function GuardarPreguntasFiltro(ByRef ent As OP_Preguntas_Filtro)
        Dim e As New OP_Preguntas_Filtro

        If Not (ent.Id = 0) Then
            e = ObtenerPreguntasFitroxId(ent.Id)
        End If
        e.IdFiltro = ent.IdFiltro
        e.TipoPregunta = ent.TipoPregunta
        e.OrdenPregunta = ent.OrdenPregunta
        e.Textopregunta = ent.Textopregunta
        e.Respuestas = ent.Respuestas
        e.Obligatoria = ent.Obligatoria
        e.Fija = ent.Fija
        e.Campo = ent.Campo
        If ent.Id = 0 Then
            oMatrixContext.OP_Preguntas_Filtro.Add(e)
        End If
        oMatrixContext.SaveChanges()
        Return e.Id
    End Function

    Public Function ObtenerRespuestasFitroMaestroxId(ByVal Id As Int64) As OP_Respuestas_Filtro_Maestro
        Return oMatrixContext.OP_Respuestas_Filtro_Maestro.Where(Function(x) x.Id = Id).FirstOrDefault
    End Function

    Public Function GuardarRespuestasFiltroMaestro(ByRef ent As OP_Respuestas_Filtro_Maestro)
        Dim e As New OP_Respuestas_Filtro_Maestro

        If Not (ent.Id = 0) Then
            e = ObtenerRespuestasFitroMaestroxId(ent.Id)
        End If
        e.IdFiltro = ent.IdFiltro
        e.Fecha = ent.Fecha
        e.Nombre = ent.Nombre
        e.Cedula = ent.Cedula
        e.Celular = ent.Celular
        e.Direccion = ent.Direccion
        e.Ciudad = ent.Ciudad
        e.Barrio = ent.Barrio
        e.Edad = ent.Edad
        e.Estrato = ent.Estrato
        e.Reclutador = ent.Reclutador
        e.Estado = ent.Estado
        If ent.Id = 0 Then
            oMatrixContext.OP_Respuestas_Filtro_Maestro.Add(e)
        End If
        oMatrixContext.SaveChanges()
        Return e.Id
    End Function

    Public Function ObtenerRespuestasFitroDetallexId(ByVal Id As Int64) As OP_Respuestas_Filtro_Detalle
        Return oMatrixContext.OP_Respuestas_Filtro_Detalle.Where(Function(x) x.Id = Id).FirstOrDefault
    End Function

    Public Function GuardarRespuestasFiltroDetalle(ByRef ent As OP_Respuestas_Filtro_Detalle)
        Dim e As New OP_Respuestas_Filtro_Detalle

        If Not (ent.Id = 0) Then
            e = ObtenerRespuestasFitroDetallexId(ent.Id)
        End If
        e.IdRespuesta = ent.IdRespuesta
        e.IdPregunta = ent.IdPregunta
        e.Respuesta = ent.Respuesta
        If ent.Id = 0 Then
            oMatrixContext.OP_Respuestas_Filtro_Detalle.Add(e)
        End If
        oMatrixContext.SaveChanges()
        Return e.Id
    End Function

    Sub GuardarLogRespuestasFiltro(ByRef e As OP_LogRespuestas_Filtro)
        If e.id = 0 Then
            oMatrixContext.OP_LogRespuestas_Filtro.Add(e)
        End If
        oMatrixContext.SaveChanges()
    End Sub

    Public Sub CrearCopiaFiltros(ByVal Id As Int64?, ByVal IdFiltro As Int64?)
        oMatrixContext.OP_Filtros_CrearCopia(Id, IdFiltro)
    End Sub

    Public Function GuardarEntrevistado(ByRef Ent As OP_Entrevistados_Cuali) As Integer
        Dim oMatrixContext1 = New OP_Entities
        Dim id = Ent.Id
        If id = 0 Then
            oMatrixContext1.OP_Entrevistados_Cuali.Add(Ent)
            oMatrixContext1.SaveChanges()
            Return Ent.Id
        Else
            Try
                Dim e As New OP_Entrevistados_Cuali
                e = oMatrixContext1.OP_Entrevistados_Cuali.Where(Function(x) x.Id = id).FirstOrDefault
                e = emparejarEntrevistado(Ent, e)
                oMatrixContext1.SaveChanges()
                Return e.Id
            Catch ex As Exception
                Return 0
            End Try
        End If

    End Function

    Function emparejarEntrevistado(ent As OP_Entrevistados_Cuali_Get_Result) As OP_Entrevistados_Cuali
        Dim Entrevistado As New OP_Entrevistados_Cuali

        Entrevistado.Id = ent.Id
        Entrevistado.Nombre = ent.Nombre
        Entrevistado.Documento = ent.Documento
        Entrevistado.Telefono = ent.Telefono
        Entrevistado.Celular = ent.Celular
        Entrevistado.FechaNacimiento = ent.FechaNacimiento
        Entrevistado.Edad = ent.Edad
        Entrevistado.EstadoCivil = ent.EstadoCivil
        Entrevistado.Genero = ent.Genero
        Entrevistado.Ciudad = ent.Ciudad
        Entrevistado.Direccion = ent.Direccion
        Entrevistado.Barrio = ent.Barrio
        Entrevistado.Estrato = ent.Estrato
        Entrevistado.Correo = ent.Correo
        Entrevistado.NivelEscolaridad = ent.NivelEscolaridad
        Entrevistado.Perfil = ent.Perfil
        Entrevistado.FechaCreacion = ent.FechaCreacion
        Entrevistado.UsuarioCreacion = ent.UsuarioCreacion

        Return Entrevistado
    End Function

    Function emparejarEntrevistado(ent As OP_Entrevistados_Cuali, Entrevistado As OP_Entrevistados_Cuali) As OP_Entrevistados_Cuali
        Entrevistado.Id = ent.Id
        Entrevistado.Nombre = ent.Nombre
        Entrevistado.Documento = ent.Documento
        Entrevistado.Telefono = ent.Telefono
        Entrevistado.Celular = ent.Celular
        Entrevistado.FechaNacimiento = ent.FechaNacimiento
        Entrevistado.Edad = ent.Edad
        Entrevistado.EstadoCivil = ent.EstadoCivil
        Entrevistado.Genero = ent.Genero
        Entrevistado.Ciudad = ent.Ciudad
        Entrevistado.Direccion = ent.Direccion
        Entrevistado.Barrio = ent.Barrio
        Entrevistado.Estrato = ent.Estrato
        Entrevistado.Correo = ent.Correo
        Entrevistado.NivelEscolaridad = ent.NivelEscolaridad
        Entrevistado.Perfil = ent.Perfil
        Entrevistado.FechaCreacion = ent.FechaCreacion
        Entrevistado.UsuarioCreacion = ent.UsuarioCreacion

        Return Entrevistado
    End Function


    Public Function GuardarProgramarCampo(ByVal Ent As OP_Programacion_Campo_Cuali) As Int64
        Dim oMatrixContext1 = New OP_Entities
        If Ent.id = 0 Then
            oMatrixContext1.OP_Programacion_Campo_Cuali.Add(Ent)
            oMatrixContext1.SaveChanges()
            Return Ent.id
        Else
            Try
                Dim e As New OP_Programacion_Campo_Cuali
                e = oMatrixContext1.OP_Programacion_Campo_Cuali.Where(Function(x) x.id = Ent.id).FirstOrDefault
                e.id = Ent.id
                e.Fecha = Ent.Fecha
                e.Hora = Ent.Hora
                e.Estado = Ent.Estado
                e.Moderador = Ent.Moderador
                e.Filtro = Ent.Filtro
                oMatrixContext1.SaveChanges()
                Return e.id
            Catch ex As Exception
                Return 0
            End Try
        End If
    End Function

    Public Function GuardarObsProgramarCampo(ByVal Ent As OP_Programacion_Campo_Cuali, ByVal txtObservaciones As String, ByVal usuario As Int64) As Int64
        Dim oMatrixContext1 = New OP_Entities
        If Ent.id <> 0 Then
            Try
                Dim e As New OP_Programacion_Campo_Cuali
                e = oMatrixContext1.OP_Programacion_Campo_Cuali.Where(Function(x) x.id = Ent.id).FirstOrDefault
                Dim observaciones As New OP_Observaciones_Programacion_Campo_Cuali
                observaciones.EntrevistadoId = e.EntrevistadoId
                observaciones.TrabajoId = e.TrabajoId
                observaciones.EntrevistadoId = e.EntrevistadoId
                observaciones.Observador = txtObservaciones
                observaciones.UsuarioCreacion = usuario
                observaciones.FechaCreacion = DateTime.Now
                oMatrixContext1.OP_Observaciones_Programacion_Campo_Cuali.Add(observaciones)
                oMatrixContext1.SaveChanges()
                Return e.id
            Catch ex As Exception
                Return 0
            End Try
        Else
            Return 0
        End If
    End Function


#End Region
#Region "Eliminar"
    Public Function EliminarCampo(ByRef Ent As OP_CampoCuali) As Int32
        Dim e As New OP_CampoCuali
        e = ObtenerCampoCualiXId(Ent.id)
        Try
            oMatrixContext.OP_CampoCuali.Remove(e)
            oMatrixContext.SaveChanges()
            Return 0
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Function EliminarModerador(ByRef Ent As PY_SegmentosCuali_Moderadores) As Int32
        Dim e As New PY_SegmentosCuali_Moderadores
        e = ObtenerModeradorxId(Ent.id)
        Try
            oMatrixContext.PY_SegmentosCuali_Moderadores.Remove(e)
            oMatrixContext.SaveChanges()
            Return 0
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Function EliminarReclutador(ByRef Ent As PY_SegmentosCuali_Reclutadores) As Int32
        Dim e As New PY_SegmentosCuali_Reclutadores
        e = ObtenerReclutadorxId(Ent.id)
        Try
            oMatrixContext.PY_SegmentosCuali_Reclutadores.Remove(e)
            oMatrixContext.SaveChanges()
            Return 0
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Sub EliminarMuestraXEstudio(ByVal id As Int64)
        oMatrixContext.OP_MuestraTrabajosCuali_Entrevistas.Remove(ObtenerMuestraCualiEntrevistasxEstudio(id))
        oMatrixContext.SaveChanges()
    End Sub

    Public Sub EliminarMuestraXSesion(ByVal id As Int64)
        oMatrixContext.OP_MuestraTrabajosCuali_Sesiones.Remove(ObtenerMuestraCualiEntrevistasxSesion(id))
        oMatrixContext.SaveChanges()
    End Sub

    Public Sub EliminarMuestraXInHome(ByVal id As Int64)
        oMatrixContext.OP_MuestraTrabajosCuali_InHome.Remove(ObtenerMuestraCualiEntrevistasxInHome(id))
        oMatrixContext.SaveChanges()
    End Sub

    Public Sub EliminarPreguntaFitro(ByVal Id As Int64)
        oMatrixContext.OP_Preguntas_Filtro.Remove(ObtenerPreguntasFitroxId(Id))
        oMatrixContext.SaveChanges()
    End Sub

#End Region
End Class
