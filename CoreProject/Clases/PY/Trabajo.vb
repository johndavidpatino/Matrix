
'Imports CoreProject.PY_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Trabajo
#Region "Variables Globales"
    Private oMatrixContext As PY_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New PY_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function DevolverTodos() As List(Of PY_Trabajo_Get_Result)
        Try
            Return PY_Trabajo_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxID(ByVal ID As Int64) As PY_Trabajo_Get_Result
        Try
            Dim oResult As List(Of PY_Trabajo_Get_Result)
            oResult = PY_Trabajo_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function PY_Trabajo_Get(ByVal ID As Int64, ByVal ProyectoID As Int64) As List(Of PY_Trabajo_Get_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Trabajo_Get_Result) = oMatrixContext.PY_Trabajo_Get(ID, ProyectoID)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Function obtenerAllTrabajos() As List(Of PY_Trabajos_Get_Result)
        Return PY_Trabajo_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)

    End Function
    Function obtenerPorBusqueda(ByVal Buscar As String) As List(Of PY_Trabajos_Get_Result)
        Return PY_Trabajo_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Buscar)
    End Function
    Function ObtenerTrabajosSinMetodologiaCampo() As List(Of PY_Trabajos_Get_WithoutMetodCampo_Estadistica_Result)
        Return oMatrixContext.PY_Trabajos_Get_WithoutMetodCampo_Estadistica(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).ToList
    End Function
    Function obtenerXIdProyeto(ByVal proyectoId As Int64) As List(Of PY_Trabajos_Get_Result)
        Return PY_Trabajo_Get(Nothing, proyectoId, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
    End Function
    Function obtenerXIdProyectoXIdTrabajo(ByVal idTrabajo As Int64, ByVal proyectoId As Int64) As List(Of PY_Trabajos_Get_Result)
        Return PY_Trabajo_Get(idTrabajo, proyectoId, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
    End Function
    Private Function PY_Trabajo_Get(ByVal id As Nullable(Of Global.System.Int64), ByVal proyectoId As Nullable(Of Global.System.Int64), ByVal OP_MetodologiaId As Nullable(Of Global.System.Int32), ByVal presupuestoId As Global.System.String, ByVal nombreTrabajo As Global.System.String, ByVal muestra As Nullable(Of Global.System.Int64), ByVal fechaTentativaInicioCampo As Nullable(Of Global.System.DateTime), ByVal fechaTentativaFinalizacion As Nullable(Of Global.System.DateTime), ByVal cOE As Nullable(Of Global.System.Int64), ByVal unidad As Nullable(Of Global.System.Int32), ByVal todosCampos As String) As List(Of PY_Trabajos_Get_Result)
        Return oMatrixContext.PY_Trabajos_Get(id, proyectoId, OP_MetodologiaId, presupuestoId, nombreTrabajo, muestra, fechaTentativaInicioCampo, fechaTentativaFinalizacion, cOE, unidad, Nothing, todosCampos).ToList
    End Function

    Function PY_Trabajos_COES() As List(Of PY_Trabajos_COES_Result)
        Return oMatrixContext.PY_Trabajos_COES().ToList
    End Function

    Function ObtenerTrabajosxCOE(ByVal COE As Int64?) As List(Of PY_Trabajos_Get_Result)
        Return oMatrixContext.PY_Trabajos_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, COE, Nothing, Nothing, Nothing).ToList
    End Function

    Public Function obtenerXIdProyectoXTodosCampos(ByVal proyectoId As Int64, ByVal todosCampos As String) As List(Of PY_Trabajos_Get_Result)
        Return PY_Trabajo_Get(Nothing, proyectoId, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, todosCampos)
    End Function
    Public Function obtenerXIdCOEXTodosCampos(ByVal coe As Int64, ByVal todosCampos As String) As List(Of PY_Trabajos_Get_Result)
        Return PY_Trabajo_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, coe, Nothing, todosCampos)
    End Function
    Public Function obtenerXIdUnidadXTodosCampos(ByVal unidad As Int64, ByVal todosCampos As String) As List(Of PY_Trabajos_Get_Result)
        Return PY_Trabajo_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, unidad, todosCampos)
    End Function
    Public Function obtenerXId(ByVal Id As Int64) As PY_Trabajos_Get_Result
        Return PY_Trabajo_Get(Id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
    End Function
    Public Function obtenerXCOE(ByVal Coe As Int64) As List(Of PY_Trabajos_Get_Result)
        Return PY_Trabajo_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Coe, Nothing, Nothing)
    End Function

    Public Function ObtenerXCoordinador(ByVal Coordinador As Int64) As List(Of PY_Trabajos_Get_Result)
        Return oMatrixContext.PY_Trabajos_Coordinador_Get(Coordinador).ToList
    End Function

    Public Function ObtenerInfoNuevoTrabajo(ByVal IdProyecto As Int64) As List(Of PY_InfoTrabajoCreacion_Result)
        Return oMatrixContext.PY_InfoTrabajoCreacion(IdProyecto).ToList
    End Function
    Public Function ObtenerTrabajoXJob(ByVal Job As String) As PY_Trabajo0
        Return oMatrixContext.PY_Trabajo.Where(Function(x) x.JobBook = Job).FirstOrDefault
    End Function
    Public Function ObtenerTrabajo(ByVal id As Int64) As PY_Trabajo0
        Return oMatrixContext.PY_Trabajo.Where(Function(x) x.id = id).FirstOrDefault
    End Function
    Public Function ObtenerTrabajoCuali(ByVal TrabajoId As Int64) As PY_TrabajoCuali
        Return oMatrixContext.PY_TrabajoCuali.Where(Function(x) x.TrabajoId = TrabajoId).FirstOrDefault
    End Function

    Public Function ObtenerInfoTrabajoCuali(ByVal TrabajoId As Int64) As PY_TrabajoCuali_Get_Result
        Return oMatrixContext.PY_TrabajoCuali_Get(TrabajoId).FirstOrDefault
    End Function

    Public Function ObtenerEstado(ByVal id As Int16) As String
        Return oMatrixContext.PY_EstadosTrabajo.Where(Function(x) x.id = id).FirstOrDefault.EstadoDesc
    End Function

    Public Function ListadoTrabajos(ByVal id As Int64?, Estado As Int32?, Nombre As String, JobBook As String, Proyecto As Int64?, Coe As Int64?, GerenteProyectos As Int64?, Unidad As Int32?, Gerencia As Int32?, Propuesta As Int64?, TodosCampos As String) As List(Of PY_Trabajos_GET_All_Result)
        Return oMatrixContext.PY_Trabajos_GET_All(id, Estado, Nombre, JobBook, Proyecto, Coe, GerenteProyectos, Unidad, Gerencia, Propuesta, TodosCampos).ToList
    End Function

    Public Function ListadoTrabajosCuali(ByVal id As Int64?, Estado As Int32?, Nombre As String, JobBook As String, Proyecto As Int64?, Coe As Int64?, GerenteProyectos As Int64?, Unidad As Int32?, Gerencia As Int32?, Propuesta As Int64?, TodosCampos As String) As List(Of PY_TrabajosCuali_GET_All_Result)
        Return oMatrixContext.PY_TrabajosCuali_GET_All(id, Estado, Nombre, JobBook, Proyecto, Coe, GerenteProyectos, Unidad, Gerencia, Propuesta, TodosCampos).ToList
    End Function

    Public Function existeNombreTrabajo(ByVal nombreTrabajo As String) As Boolean
        Return oMatrixContext.PY_Trabajo_NombreTrabajoYaExiste(nombreTrabajo).FirstOrDefault
    End Function

    Public Function obtenerListadoTrabajosCualitativos(ByVal id As Int64?, ByVal ProyectoId As Int64?, ByVal OP_MetodologiaId As Int32?, ByVal PresupuestoId As String, ByVal NombreTrabajo As String, ByVal Muestra As Int64?, ByVal FechaTentativaInicioCampo As Date?, ByVal FechaTentativaFinalizacion As Date?, ByVal COE As Int64?, ByVal Unidad As Int32?, ByVal JobBook As String, ByVal TipoProyectoId As Int16?, ByVal TodosCampos As String) As List(Of PY_Trabajos_Get_Cualitativos_Result)
        Return oMatrixContext.PY_Trabajos_Get_Cualitativos(id, ProyectoId, OP_MetodologiaId, PresupuestoId, NombreTrabajo, Muestra, FechaTentativaInicioCampo, FechaTentativaFinalizacion, COE, Unidad, JobBook, TipoProyectoId, TodosCampos).ToList
    End Function

    Public Function ObtenerTrabajosCualitativosxTrabajo(ByVal idTrabajo As Int64?, ByVal TipoProyectoId As Int64?) As List(Of PY_Trabajos_Get_Cualitativos_Result)
        Return obtenerListadoTrabajosCualitativos(idTrabajo, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, TipoProyectoId, Nothing)
    End Function

    Public Function ObtenerTrabajosCualitativosxCOE(ByVal COE As Int64?, ByVal TipoProyectoId As Int16?, ByVal todosCampos As String) As List(Of PY_Trabajos_Get_Cualitativos_Result)
        Return obtenerListadoTrabajosCualitativos(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, COE, Nothing, Nothing, TipoProyectoId, todosCampos)
    End Function

    Public Function ObtenerGerentesProyectoCuali() As List(Of PY_GerenteProyecto_Cuali_Result)
        Return oMatrixContext.PY_GerenteProyecto_Cuali().ToList
    End Function
    Public Function ObtenerTrabajosxGerente(ByVal gerente As Int64) As List(Of PY_TrabajosxProyectosxGerente_Result)
        Return oMatrixContext.PY_TrabajosxProyectosxGerente(gerente).ToList
    End Function

    Public Function ObtenerCoordinadorProyectoCuali() As List(Of PY_CoordinadorProyecto_Cuali_Result)
        Return oMatrixContext.PY_CoordinadorProyecto_Cuali().ToList
    End Function
    Public Function ObtenerTrabajosxCoordinador(ByVal coordinador As Int64) As List(Of PY_TrabajosxProyectosxCoordinador_Result)
        Return oMatrixContext.PY_TrabajosxProyectosxCoordinador(coordinador).ToList
    End Function

	Public Function ObtenerTrabajosConTareaEntregaDeProyectos() As List(Of PY_TrabajosConTareaEntregaDeProyectos_Result)
		Return oMatrixContext.PY_TrabajosConTareaEntregaDeProyectos.ToList
	End Function
#End Region
#Region "Guardar"
	Public Function GuardarTrabajo(ByRef ent As PY_Trabajo0) As Int64
        Dim e As New PY_Trabajo0

        If Not (ent.id = 0) Then
            e = ObtenerTrabajo(ent.id)
        End If
        e.ProyectoId = ent.ProyectoId
        e.OP_MetodologiaId = ent.OP_MetodologiaId
        e.PresupuestoId = ent.PresupuestoId
        e.NombreTrabajo = ent.NombreTrabajo
        e.Muestra = ent.Muestra
        e.FechaTentativaInicioCampo = ent.FechaTentativaInicioCampo
        e.FechaTentativaFinalizacion = ent.FechaTentativaFinalizacion
        e.COE = ent.COE
        e.Unidad = ent.Unidad
        e.TipoRecoleccionId = ent.TipoRecoleccionId
        e.Estado = ent.Estado
        e.IdPropuesta = ent.IdPropuesta
        e.Alternativa = ent.Alternativa
        e.MetCodigo = ent.MetCodigo
        e.Fase = ent.Fase
        e.NoMedicion = ent.NoMedicion
        e.JobBook = ent.JobBook
        e.COE = ent.COE
        If ent.id = 0 Then
            oMatrixContext.PY_Trabajo.Add(e)
        End If
        Try
            oMatrixContext.SaveChanges()
        Catch ex As Exception
            Dim error1 = ex.Message
            Return 0
        End Try

        Return e.id
    End Function

    Public Function GuardarTrabajoCuali(ByRef ent As PY_TrabajoCuali) As Int64
        Dim e As New PY_TrabajoCuali

        If Not (ent.TrabajoId = 0) Then
            e = ObtenerTrabajoCuali(ent.TrabajoId)
            If e Is Nothing Then
                e = New PY_TrabajoCuali
            End If
        End If
        e.TrabajoId = ent.TrabajoId
        e.IncentivoEconomico = ent.IncentivoEconomico
        e.PresupuestoIncentivo = ent.PresupuestoIncentivo
        e.DistribucionIncentivo = ent.DistribucionIncentivo
        e.RegalosCliente = ent.RegalosCliente
        e.CompraIpsos = ent.CompraIpsos
        e.PresupuestoCompra = ent.PresupuestoCompra
        e.DistribucionCompra = ent.DistribucionCompra
        e.ExclusionesRestricciones = ent.ExclusionesRestricciones
        e.RecursosPropiedadCliente = ent.RecursosPropiedadCliente
        e.Backups = ent.Backups

        If e.id = 0 Then
            oMatrixContext.PY_TrabajoCuali.Add(e)
        End If
        oMatrixContext.SaveChanges()
        Return e.TrabajoId
    End Function

    Public Function Guardar(ByVal ID As Int64?, ByVal ProyectoId As Int64?, ByVal oP_MetodologiaId As Integer?, ByVal presupuestoId As Integer?, ByVal nombreTrabajo As String, ByVal Muestra As Int64?, ByVal FechaTentativaInicioCampo As Date?, ByVal FechaTentativaFinalizacion As Date?, ByVal COE As Int64?, ByVal Unidad As Int32?, ByVal JobBook As String, ByVal TipoRecoleccion As Int16?, ByVal Estado As Int16?) As Decimal
        Try
            Dim TrabajoID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.PY_Trabajo_Edit(ID, ProyectoId, oP_MetodologiaId, presupuestoId, nombreTrabajo, Muestra, FechaTentativaInicioCampo, FechaTentativaFinalizacion, COE, Unidad, JobBook, TipoRecoleccion, Estado)
                TrabajoID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.PY_Trabajo_Add(ProyectoId, oP_MetodologiaId, presupuestoId, nombreTrabajo, Muestra, FechaTentativaInicioCampo, FechaTentativaFinalizacion, COE, Unidad)
                TrabajoID = Decimal.Parse(oResult(0))
            End If

            Return TrabajoID
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Sub CambioEstado(ByVal TrabajoId As Int64, Estado As Int16, Observaciones As String, Usuario As Int64)
        Dim trabajo = ObtenerTrabajo(TrabajoId)
        trabajo.Estado = Estado
        Dim ent As New TR_CambioEstados
        ent.Estado = Estado
        ent.Observaciones = Observaciones
        ent.Fecha = Now
        ent.CerradoPor = Usuario
        ent.Trabajo = TrabajoId
        oMatrixContext.TR_CambioEstados.Add(ent)
        oMatrixContext.SaveChanges()
    End Sub

    Public Sub CambioEstadoCualitativo(ByVal TrabajoId As Int64, Estado As Int16, Observaciones As String, Usuario As Int64)
        Dim trabajo = ObtenerTrabajo(TrabajoId)
        trabajo.Estado = Estado
        Dim ent As New TR_CambioEstados
        ent.Estado = Estado
        ent.Observaciones = Observaciones
        ent.Fecha = Now
        ent.CerradoPor = Usuario
        ent.Trabajo = TrabajoId
        oMatrixContext.TR_CambioEstados.Add(ent)
        oMatrixContext.SaveChanges()
    End Sub

    Public Function DuplicarTrabajo(ByVal ProyectoId As Int64?, ByVal oP_MetodologiaId As Integer?, ByVal presupuestoId As Integer?, ByVal nombreTrabajo As String, ByVal Muestra As Int64?, ByVal FechaTentativaInicioCampo As Date?, ByVal FechaTentativaFinalizacion As Date?, ByVal COE As Int64?, ByVal Unidad As Int32?, ByVal JobBook As String, ByVal TipoRecoleccion As Int16?, ByVal Estado As Int16?, ByVal IdPropuesta As Int64?, ByVal Alternativa As Int32?, ByVal MetCodigo As Int32?, ByVal Fase As Int32?, ByVal NumeroMedicion As Int32?) As Decimal
        Dim TrabajoID As Decimal = 0
        Dim oResult As ObjectResult(Of Decimal?)
        oResult = oMatrixContext.Py_TrabajoDuplicar(ProyectoId, oP_MetodologiaId, presupuestoId, nombreTrabajo, Muestra, FechaTentativaInicioCampo, FechaTentativaFinalizacion, COE, Unidad, JobBook, TipoRecoleccion, Estado, IdPropuesta, Alternativa, MetCodigo, Fase, NumeroMedicion)
        TrabajoID = Decimal.Parse(oResult(0))
        Return TrabajoID

    End Function

    Public Sub DuplicarMuestra(ByVal Ciudad As Integer, ByVal TrabajoId As Int64, ByVal Cantidad As Integer, ByVal Coordinador As Int64?, ByVal FechaInicio As Date?, ByVal FechaFin As Date?)
        oMatrixContext.OP_MuestraTrabajosDuplicar(Ciudad, TrabajoId, Cantidad, Coordinador, FechaInicio, FechaFin)
    End Sub

    Function trabajoconfiguracionget(ByVal trabajoid As Int64) As List(Of OP_TrabajoConfiguracion_Get_Result)
        Return oMatrixContext.OP_TrabajoConfiguracion_Get(trabajoid).ToList
    End Function

    Public Sub guardartrabajoconfiguracion(ByVal TrabajoId As Int64, ByVal fechaini As Date, ByVal fechafin As Date, ByVal porcentajeverificacion As Int64, ByVal unidadcritica As Int64)
        oMatrixContext.OP_TrabajoConfiguracion_Add(TrabajoId, fechaini, fechafin, porcentajeverificacion, unidadcritica)
    End Sub
#End Region
#Region "Eliminar"
    Public Function Eliminar(ByVal ID As Int64) As Integer
        Try
            Return oMatrixContext.PY_Trabajo_Del(ID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
End Class
