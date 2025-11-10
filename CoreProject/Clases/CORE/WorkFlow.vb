

'Imports CoreProject.MatrixModel
Public Class WorkFlow
#Region "Enumeradores"
    Enum Estados
        Creada = 1
        EnCurso = 2
        Asignada = 3
        Devuelta = 4
        Finalizada = 5
    End Enum
    Enum EstadosProcesados
        SinPlanear = 1
        FinalizaHoy = 2
        IniciaHoy = 3
        AunNoHaReportadoElInicio = 4
        Vencida = 5
        FinalizadaIncumplida = 6
        PorEjecutar = 7
        FinalizadaCumplida = 8
        Devuelta = 9
        EnCurso = 10
        NoAplica = 11
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

    Function obtenerXIdTrabajo(ByVal idTrabajo As Int64) As List(Of CORE_WorkFlow_Trabajos_Get_Result)
        Return CORE_WorkFlow_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, idTrabajo, Nothing, Nothing, Nothing, Nothing)
    End Function
    Function obtenerXId(ByVal id As Int64) As CORE_WorkFlow_Trabajos_Get_Result
        Return CORE_WorkFlow_Get(id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
    End Function

    Function obtenerTrabajosWorkFlow(ByVal idUnidadejecuta As Int64, ByVal todosCampos As String) As List(Of CORE_Trabajos_WithWorkFlow_Result)
        Return oMatrixContext.CORE_Trabajos_WithWorkFlow(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, todosCampos, Nothing, Nothing, idUnidadejecuta).ToList
    End Function
    Function obtenerXUsuarioAsignado(ByVal usuarioAsignado As Int64) As List(Of CORE_WorkFlow_Trabajos_Get_Result)
        Return CORE_WorkFlow_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, usuarioAsignado, Nothing, Nothing, Nothing, Nothing, Nothing)
    End Function

    Function obtenerXUsuarioAsignadoXEstado(ByVal usuarioAsignado As Int64, ByVal estado As Int16) As List(Of CORE_WorkFlow_Trabajos_Get_Result)
        Return CORE_WorkFlow_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, usuarioAsignado, Nothing, Nothing, Nothing, estado, Nothing)
    End Function

    Function obtenerXUsuarioAsignadoXIdTrabajoXEstado(ByVal usuarioAsignado As Int64, ByVal idTrabajo As Int64, ByVal estado As Int16) As List(Of CORE_WorkFlow_Trabajos_Get_Result)
        Return CORE_WorkFlow_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, usuarioAsignado, idTrabajo, Nothing, Nothing, estado, Nothing)
    End Function

    Function obtenerXUsuarioAsignadoXTodosCampos(ByVal usuarioAsignado As Int64, ByVal todosCampos As String) As List(Of CORE_WorkFlow_Trabajos_Get_Result)
        Return CORE_WorkFlow_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, usuarioAsignado, Nothing, todosCampos, Nothing, Nothing, Nothing)
    End Function

    Function obtenerXUsuarioAsignadoXIdTrabajoXTodosCampos(ByVal usuarioAsignado As Int64, ByVal idTrabajo As Int64, ByVal todosCampos As String) As List(Of CORE_WorkFlow_Trabajos_Get_Result)
        Return CORE_WorkFlow_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, usuarioAsignado, idTrabajo, todosCampos, Nothing, Nothing, Nothing)
    End Function
    Function obtenerXRoleEstimaXTrabajoId(ByVal idRoleEstima As Int16, ByVal idTrabajo As Int64) As List(Of CORE_WorkFlow_Trabajos_Get_Result)
        Return CORE_WorkFlow_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, idTrabajo, Nothing, idRoleEstima, Nothing, Nothing)
    End Function
    Function obtenerXUnidadEjecutaXTrabajoId(ByVal idUnidadEjecuta As Int16, ByVal idTrabajo As Int64) As List(Of CORE_WorkFlow_Trabajos_Get_Result)
        Return CORE_WorkFlow_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, idTrabajo, Nothing, Nothing, Nothing, idUnidadEjecuta)
    End Function

    Function DestinariosCorreosNotificacionFechas(ByVal TrabajoId As Int64, TareaId As Int64) As List(Of CORE_DestinatariosCorreosNotificacionFecha_Result)
        Return oMatrixContext.CORE_DestinatariosCorreosNotificacionFecha(TareaId, TrabajoId).ToList
    End Function

    Public Function ObtenerWorkFlowXIdTrabajoXIdTarea(ByVal idTrabajo As Int64, ByVal idTarea As Int64) As CORE_WorkFlow
        Return oMatrixContext.CORE_WorkFlow.Where(Function(x) x.CORE_Hilos.ContenedorId = idTrabajo And x.TareaId = idTarea).FirstOrDefault
    End Function

    Private Function CORE_WorkFlow_Get(ByVal id As Int64?, ByVal hiloId As Int64?, ByVal tareaId As Int64?, ByVal fIniP As Date?, ByVal fFinP As Date?, ByVal fIniR As Date?, ByVal fFinR As Date?, ByVal usuarioEstima As Int64?, ByVal usuarioAsignado As Int64?, ByVal trabajoId As Int64?, ByVal todosCampos As Global.System.String, ByVal idRolEstima As Int16?, ByVal Estado As Int16?, ByVal unidadEjecuta As Int64?) As List(Of CORE_WorkFlow_Trabajos_Get_Result)
        Return oMatrixContext.CORE_WorkFlow_Trabajos_Get(id, hiloId, tareaId, fIniP, fFinP, fIniR, fFinR, usuarioEstima, usuarioAsignado, trabajoId, todosCampos, idRolEstima, Estado, unidadEjecuta).ToList
    End Function
    Function obtenerUsuariosNotificacionTareas(ByVal idTrabajo As Int64, ByVal idTarea As Int64) As List(Of CORE_WorkFlow_TareasAsociadas_Get_Result)
        Return oMatrixContext.CORE_WorkFlow_TareasAsociadas_Get(idTrabajo, idTarea).ToList
    End Function
    Function obtenerUsuariosNotificacionTareaDevuelta(ByVal idTrabajo As Int64, ByVal idTarea As Int64) As List(Of CORE_WorkFlow_UsuariosANotificarTareaDevuelta_Get_Result)
        Return oMatrixContext.CORE_WorkFlow_UsuariosANotificarTareaDevuelta_Get(idTrabajo, idTarea).ToList
    End Function

    Function obtenerObservacionesXTarea(ByVal core_WorkFloId As Int64) As List(Of CORE_ObservacionesTareas_Get_Result)
        Return oMatrixContext.CORE_ObservacionesTareas_Get(core_WorkFloId).ToList
    End Function

    Function obtenerListaTrabajosTareas(ByVal coeId As Int64?, ByVal GerenteId As Int64?, ByVal proceso As Int64?, ByVal unidadesId As String, ByVal nombreTrabajo As String, ByVal estadoActual As EstadosProcesados?, ByVal paginaActual As Int64?, ByVal tamanoPagina As Int64?, anoRegistro As Decimal?, mesRegistro As Decimal?) As List(Of CORE_TrabajosTareas_Get_Result)
        paginaActual = If(paginaActual = 0, Nothing, paginaActual)
        tamanoPagina = If(tamanoPagina = 0, Nothing, tamanoPagina)
        Return oMatrixContext.CORE_TrabajosTareas_Get(coeId, GerenteId, proceso, unidadesId, nombreTrabajo, estadoActual, paginaActual, tamanoPagina, anoRegistro, mesRegistro).ToList
    End Function

    Function obtenerEstadosProcesados() As List(Of String)
        Dim tipo As Type = [Enum].GetUnderlyingType(EstadosProcesados.Devuelta.GetType())
        Dim nombres() As String = [Enum].GetNames(EstadosProcesados.Devuelta.GetType())
        Dim valores As Array = [Enum].GetValues(EstadosProcesados.Devuelta.GetType())
        Dim lst As New List(Of String)
        For i As Integer = 0 To nombres.Length - 1
            lst.Add(nombres(i) & "-" & Convert.ChangeType(valores.GetValue(i), tipo).ToString())
        Next
        Return lst
    End Function

    Function obtenerCantidadListaTrabajosTareas(ByVal coeId As Int64?, ByVal GerenteId As Int64?, ByVal proceso As Int64?, ByVal unidadesId As String, ByVal nombreTrabajo As String, ByVal estadoActual As EstadosProcesados?, anoRegistro As Int64?, mesRegistro As Int64?) As Int32?
        Return oMatrixContext.CORE_TrabajosTareas_Count_Get(coeId, GerenteId, proceso, unidadesId, nombreTrabajo, estadoActual, anoRegistro, mesRegistro).FirstOrDefault
    End Function

    Function ObtenerWorkflowXTrabajoXTarea(ByVal TrabajoId As Int64, ByVal TareaId As Int64?) As List(Of CORE_WorkFlow_GetXTrabajoXTarea_Result)
        Return oMatrixContext.CORE_WorkFlow_GetXTrabajoXTarea(TrabajoId, TareaId).ToList
    End Function

    Function ObtenerTareasxHiloid(ByVal Hiloid As Int64) As List(Of CORE_WorkFlow_GetxHiloid_Result)
        Return oMatrixContext.CORE_WorkFlow_GetxHiloid(Hiloid).ToList
    End Function

    Function ObtenerUsuariosAsignadosXProceso(ByVal proceso As Byte?) As List(Of CORE_UsuariosAsignadosXProceso_Result)
        Return oMatrixContext.CORE_UsuariosAsignadosXProceso(proceso).ToList
    End Function
#End Region
#Region "Grabar"
    Function Editar(ByVal id As Int64, ByVal fIniP As Date?, ByVal fFinP As Date?, ByVal fIniR As Date?, ByVal fFinR As Date?, ByVal usuarioEstima As Int64?, ByVal usuarioAsignado As Int64?, ByVal ObservacionesPlaneacion As String) As Int64
        If id > 0 Then
            oMatrixContext.CORE_WorkFlow_Edit(id, fIniP, fFinP, fIniR, fFinR, usuarioEstima, usuarioAsignado, ObservacionesPlaneacion)
        End If
        Return id
    End Function
    Function GrabarHilo(ByVal tipoHilo As Int64, ByVal contenedorId As Int64) As Decimal
        Return oMatrixContext.CORE_Hilo_Add(tipoHilo, contenedorId).FirstOrDefault
    End Function

    Sub GrabarTareas(ByVal hiloId As Int64)
        oMatrixContext.CORE_WorkFlow_Add(hiloId)
    End Sub

    Sub CrearHiloCrearTareas(ByVal tipohilo As Int64, ByVal contenedorId As Int64)
        Using ts As New Transactions.TransactionScope
            Dim idHilo As Int64

            idHilo = GrabarHilo(tipohilo, contenedorId)
            GrabarTareas(idHilo)

            ts.Complete()

        End Using
    End Sub
    Function GuardarDuplicados(ByVal TareaId As Int64, ByVal hiloid As Int64, ByVal fIniP As Date?, ByVal fFinP As Date?, ByVal fIniR As Date?, ByVal fFinR As Date?, ByVal usuarioEstima As Int64?, ByVal usuarioAsignado As Int64?, ByVal Estado As Boolean, ByVal FultimoEstado As Date, ByVal ObPlaneacion As String, ByVal ObEjecucion As String, ByVal Descripcion As String) As Int64
        Return oMatrixContext.CORE_WorkFlow_AddDuplicados(TareaId, hiloid, fIniP, fFinP, fIniR, fFinP, usuarioEstima, usuarioAsignado, Estado, FultimoEstado, ObPlaneacion, ObEjecucion, Descripcion)
    End Function
    Public Sub GuardarWorkFlow(ByRef ent As CORE_WorkFlow)
        oMatrixContext.SaveChanges()
    End Sub
#End Region
End Class
