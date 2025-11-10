Public Class PNCClass
    Enum EEstados
        enviado = 1
        actualizado = 2
        anulado = 3
        eliminado = 4
        aceptado = 5
        rechazado = 6
        causaRegistrada = 7
    End Enum
#Region "Variables Globales"
    Private oMatrixContext As PNCEntities
    Private oMatrixContextU As US_Entities
    Private oMatrixContextC As CU_Entities
    Private oMatrixContextP As PY_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New PNCEntities
        oMatrixContextU = New US_Entities
        oMatrixContextC = New CU_Entities
        oMatrixContextP = New PY_Entities
    End Sub
#End Region

#Region "Obtener"
    Public Function ExisteAccion(WIdpNC As Integer, WIdCausa As Integer, WTipoAccion As Integer) As Boolean
        Dim VAccion = (From a In oMatrixContext.PNC_ProductoNoConformeAcciones Where a.IdPNC = WIdpNC And a.IdCausa = WIdCausa And a.TipoAccion = WTipoAccion Select a.TipoAccion).ToList
        If VAccion.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function ExisteAccionInmediata(WIdpNC As Integer, WIdCausa As Integer) As Boolean
        Dim VAccion = (From a In oMatrixContext.PNC_ProductoNoConformeAcciones Where a.IdPNC = WIdpNC And a.IdCausa = WIdCausa And a.TipoAccion = 1 Select a.TipoAccion).ToList
        If VAccion.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function LstEstudios() As List(Of CU_Estudios)
        Return (From e In oMatrixContextC.CU_Estudios Order By e.Nombre Select e).ToList
    End Function
    Public Function LstTrabajos(VJobBook As String) As List(Of PY_Trabajo0)
        Dim VTrabajos = (From t In oMatrixContextP.PY_Trabajo Where t.JobBook.Contains(VJobBook) Select t).ToList
        Return VTrabajos
    End Function
    Public Function LstPNCTodos() As List(Of PNC_ObtenerProductoNoConformeTodos_Result)
        Return oMatrixContext.PNC_ObtenerProductoNoConformeTodos().ToList
    End Function
    Public Function LstPNC(VJobBook As String) As List(Of PNC_ObtenerProductoNoConforme_Result)
        Return oMatrixContext.PNC_ObtenerProductoNoConforme(VJobBook).ToList
    End Function
    Public Function LstPNCCausas(VIdPNC As Integer?) As List(Of PNC_VerProductoNoConformeCausas)
        Dim VPNCD1 = (From t In oMatrixContext.PNC_VerProductoNoConformeCausas Where t.IdPNC = VIdPNC Select t).ToList
        Return VPNCD1
    End Function
    Public Function LstPNCAcciones(VIdPNC As Integer, VIdCausa As Integer) As List(Of PNC_VerProductoNoConformeDetalle)
        Dim VPNCD1 = (From t In oMatrixContext.PNC_VerProductoNoConformeDetalle Where t.IdPNC = VIdPNC And t.IdCausa = VIdCausa Select t).ToList
        Return VPNCD1
    End Function
    Public Function LstFuente() As List(Of PNC_FuenteReclamo)
        Return oMatrixContext.PNC_FuenteReclamo.ToList
    End Function
    Public Function LstCategoria() As List(Of PNC_Categorias)
        Return oMatrixContext.PNC_Categorias.ToList
    End Function
    Public Function LstTipoAccion() As List(Of PNC_TiposDeAccion)
        Return oMatrixContext.PNC_TiposDeAccion.ToList
    End Function
    Public Function LstUsuarios() As List(Of PNC_VObtenerUsuarios)
        Return (From u In oMatrixContext.PNC_VObtenerUsuarios Order By u.Nombre Select u).ToList
    End Function
    Public Function ObtenerlstNombreEstudio(VJobBook As String) As List(Of CU_Estudios)
        Return (From c In oMatrixContextC.CU_Estudios Where c.JobBook.Contains(VJobBook.Substring(1, 9)) Select c).ToList
    End Function
    Public Function ObtenerIdEstudio(VJobBook As String) As Integer
        Dim VEstudio = (From c In oMatrixContextC.CU_Estudios Where c.JobBook.Contains(VJobBook.Substring(1, 9)) Select c.id).FirstOrDefault()
        Return VEstudio
    End Function
    Public Function ObtenerJobBook(IdEstudio As Long) As String
        Dim VJobBook = (From c In oMatrixContextC.CU_Estudios Where c.id = IdEstudio Select c.JobBook).FirstOrDefault()
        Return VJobBook
    End Function
    Public Function ObtenerNombreUnidad(IdEstudio As Int32) As String
        Dim VUnidad = (From u In oMatrixContext.PNC_VObtenerUnidad Where u.id = IdEstudio Select u.NombreUnidad).FirstOrDefault()
        Return VUnidad
    End Function
    Public Function ObtenerCodigoUnidad(IdEstudio As Int32) As Integer
        Dim VUnidad1 = (From c In oMatrixContext.PNC_VObtenerUnidad Where c.id = IdEstudio Select c.CodigoUnidad).FirstOrDefault()
        Return VUnidad1
    End Function
    Public Function ObtenerUsuario(IdUsuario As Integer) As String
        Dim VUsuario = (From t In oMatrixContextU.US_Usuarios Where t.id = IdUsuario Select t.Apellidos + " " + t.Nombres).FirstOrDefault()
        Return VUsuario
    End Function
    Public Function ObtenerNombreCliente(IdEstudio As Integer) As String
        Dim VCliente = (From t In oMatrixContext.PNC_VObtenerCliente Where t.IdEstudio = IdEstudio Select t.RazonSocial).FirstOrDefault()
        Return VCliente
    End Function
    Public Function ObtenerIdCliente(IdEstudio As Integer) As String
        Dim VCliente = (From t In oMatrixContext.PNC_VObtenerCliente Where t.IdEstudio = IdEstudio Select t.IdCliente).FirstOrDefault()
        Return VCliente
    End Function
    Public Function ObtenerFechaReclamo(IdPNC As Integer) As Date
        Return (From f In oMatrixContext.PNC_ProductoNoConforme Where f.Id = IdPNC Select f.FechaReclamo).FirstOrDefault()
    End Function
    Public Function ObtenerFuente(IdPNC As Integer) As Integer
        Return (From f In oMatrixContext.PNC_ProductoNoConforme Where f.Id = IdPNC Select f.FuenteReclamo).FirstOrDefault()
    End Function
    Public Function ObtenerCategoria(IdPNC As Integer) As Integer
        Return (From f In oMatrixContext.PNC_ProductoNoConforme Where f.Id = IdPNC Select f.Categoria).FirstOrDefault()
    End Function
    Public Function ObtenerTarea(IdPNC As Integer) As Integer
        Return (From f In oMatrixContext.PNC_ProductoNoConforme Where f.Id = IdPNC Select f.Tarea).FirstOrDefault()
    End Function
    Public Function ObtenerTipoAccion(Id As Integer) As Integer
        Return (From f In oMatrixContext.PNC_ProductoNoConformeAcciones Where f.Id = Id Select f.TipoAccion).FirstOrDefault()
    End Function
    Public Function ObtenerResponsableAccion(Id As Integer) As Integer
        Return (From f In oMatrixContext.PNC_ProductoNoConformeAcciones Where f.Id = Id Select f.IdResponsableAccion).FirstOrDefault()
    End Function
    Public Function ObtenerResponsableSeguir(Id As Integer) As Integer
        Return (From f In oMatrixContext.PNC_ProductoNoConformeAcciones Where f.Id = Id Select f.IdResponsableSeguimiento).FirstOrDefault()
    End Function
    Public Function GrabarRegistroPNC(WIdEstudio, WIdTrabajo, WJobBook, WFechaReclamo, WReporta, WUnidad, WCliente, WFuente, WCategoria, WTarea, WDescripcion, WCerrado, WFechaCierre, WUsuario, WFechaGrabacion, WFechaActualizacion) As Int64
        Dim PNCRegistro = New PNC_ProductoNoConforme()

        PNCRegistro.IdEstudio = CInt(WIdEstudio)
        PNCRegistro.IdTrabajo = WIdTrabajo
        PNCRegistro.JobBook = WJobBook
        If WFechaReclamo = "" Then
            PNCRegistro.FechaReclamo = Nothing
        Else
            PNCRegistro.FechaReclamo = Format(CDate(WFechaReclamo), "dd/MM/yyyy")
        End If
        PNCRegistro.IdReporta = CDec(WReporta)
        PNCRegistro.IdUnidad = CDec(WUnidad)
        PNCRegistro.IdClienteExterno = CDec(WCliente)
        PNCRegistro.FuenteReclamo = CInt(WFuente)
        PNCRegistro.Categoria = CInt(WCategoria)
        PNCRegistro.Tarea = CInt(WTarea)
        PNCRegistro.Descripcion = WDescripcion
        PNCRegistro.Cerrado = WCerrado
        PNCRegistro.FechaCierre = Nothing
        PNCRegistro.Usuario = WUsuario
        PNCRegistro.FechaGrabacion = Format(WFechaGrabacion, "dd/MM/yyyy")
        PNCRegistro.FechaActualizacion = Format(WFechaActualizacion, "dd/MM/yyyy")

        oMatrixContext.PNC_ProductoNoConforme.Add(PNCRegistro)
        oMatrixContext.SaveChanges()
        Return PNCRegistro.Id
    End Function
    Public Function GrabarRegistroPNCCausa(WIdPNC, WCausa)
        Dim PNCRegistroCausa = New PNC_ProductoNoConformeCausas()

        PNCRegistroCausa.IdPNC = WIdPNC
        PNCRegistroCausa.CausaRaiz = WCausa

        oMatrixContext.PNC_ProductoNoConformeCausas.Add(PNCRegistroCausa)
        oMatrixContext.SaveChanges()
        Return PNCRegistroCausa.Id

    End Function
    Public Function GrabarRegistroPNCAccion(WIdPNC, WCausa, WTipoAccion, WAccion, WFechaPlan, WFechaEjec, WResponsableAccion, WResponsableSeguir, WEvidencia)
        Dim PNCRegistroAccion = New PNC_ProductoNoConformeAcciones()

        PNCRegistroAccion.IdPNC = WIdPNC
        PNCRegistroAccion.IdCausa = WCausa
        PNCRegistroAccion.TipoAccion = CInt(WTipoAccion)
        PNCRegistroAccion.Accion = WAccion
        If WFechaPlan = "" Then
            PNCRegistroAccion.FechaPlaneada = Nothing
        Else
            PNCRegistroAccion.FechaPlaneada = Format(CDate(WFechaPlan), "dd/MM/yyyy")
        End If
        If WFechaEjec = "" Then
            PNCRegistroAccion.FechaEjecucion = Nothing
        Else
            PNCRegistroAccion.FechaEjecucion = Format(CDate(WFechaEjec), "dd/MM/yyyy")
        End If

        PNCRegistroAccion.IdResponsableAccion = CDec(WResponsableAccion)
        PNCRegistroAccion.IdResponsableSeguimiento = CDec(WResponsableSeguir)
        PNCRegistroAccion.EvidenciaCierre = WEvidencia

        oMatrixContext.PNC_ProductoNoConformeAcciones.Add(PNCRegistroAccion)
        oMatrixContext.SaveChanges()
        Return PNCRegistroAccion.Id

    End Function
    Public Sub ActualizarPNCdetalle(ByVal WId As Int32, WFechaEjecucion As Date, WEvidencia As String)
        Dim PNCRegistro = oMatrixContext.PNC_ProductoNoConformeAcciones.Where(Function(x) x.Id = WId).FirstOrDefault

        PNCRegistro.FechaEjecucion = WFechaEjecucion ' Format(CDate(WFechaEjecucion), "dd/MM/yyyy")
        PNCRegistro.EvidenciaCierre = WEvidencia
        oMatrixContext.SaveChanges()
    End Sub
    Public Function ObtenerPNCById(ByVal id As Int64) As PNC_GetById_Result
        Return oMatrixContext.PNC_GetById(id).FirstOrDefault
    End Function
    Public Function ObtenerPNCEmailReporte(ByVal id As Int64) As List(Of String)
        Return oMatrixContext.PNC_EmailNotificacionReporte(id).ToList
    End Function
    Public Function ObtenerPNCDetalles(ByVal id As Int64) As PNC_EmailAcciones_Result
        Return oMatrixContext.PNC_EmailAcciones(id).FirstOrDefault
    End Function
    Public Function ObtenerPNCDetallesEntidad(ByVal id As Int64) As PNC_ProductoNoConformeAcciones
        Return oMatrixContext.PNC_ProductoNoConformeAcciones.Where(Function(x) x.Id = id).FirstOrDefault
    End Function
    Public Function ObtenerPNCEntidad(ByVal id As Int64) As PNC_ProductoNoConforme
        Return oMatrixContext.PNC_ProductoNoConforme.Where(Function(x) x.Id = id).FirstOrDefault
    End Function
    Public Function obtenerPNCXEstado(ByVal Estado As Byte?) As List(Of PNC_Seguimiento_Get_Result)
        Return oMatrixContext.PNC_Seguimiento_Get(Estado).ToList
    End Function
    Public Function obtenerLstPNCAccionesXEstado(ByVal estado As Byte?) As List(Of PNC_ProductoNoConformeAcciones_Get_Result)
        Return oMatrixContext.PNC_ProductoNoConformeAcciones_Get(estado).ToList
    End Function
    Public Function obtenerLstPNCCausasXEstado(ByVal estado As Byte?) As List(Of PNC_ProductoNoConformeCausas_Get_Result)
        Return oMatrixContext.PNC_ProductoNoConformeCausas_Get(estado).ToList
    End Function

    Public Function obtenerProcesos() As List(Of PNC_Procesos)
        Return oMatrixContext.PNC_Procesos.ToList
    End Function

    Public Function obtenerProcedimientos() As List(Of PNC_Procedimientos)
        Return oMatrixContext.PNC_Procedimientos.ToList
    End Function
    Public Function obtenerCausaPorIdPNC(ByVal idPNC As Int64) As PNC_Causa_Get_Result
        Return oMatrixContext.PNC_Causa_Get(idPNC).FirstOrDefault
    End Function
    Public Function obtenerLogProducto(idProducto As Int64, estado As Byte) As PNC_Productos_Log_Get_Result
        Return oMatrixContext.PNC_Productos_Log_Get(idProducto, estado).FirstOrDefault
    End Function
#End Region

    Function grabarProducto(asociadoA As Int16, estudioId As Long?, trabajoId As Long?, proceso As Int16, procedimiento As Int16, unidad As Int16, personaIdentifica As Long, fechaReclamo As Date, fuente As Int16, categoria As Int16, tarea As Int16?, responsable As Long, informarA As Long, descripcion As String, estado As EEstados, fechaCreacion As DateTime, usuario As Long) As Decimal
        Return oMatrixContext.PNC_Productos_Add(asociadoA, estudioId, trabajoId, proceso, procedimiento, unidad, personaIdentifica, fechaReclamo, fuente, categoria, tarea, responsable, informarA, descripcion, estado, fechaCreacion, usuario).FirstOrDefault
    End Function
    Function grabarLog(idProducto As Decimal, estado As EEstados, fecha As DateTime, usuario As Decimal, observaciones As String) As Decimal
        Return oMatrixContext.PNC_Productos_Log_Estado_Add(idProducto, estado, fecha, usuario, observaciones).FirstOrDefault
    End Function
    Function obtenerProductos(id As Decimal?, responsable As Decimal?, estado As EEstados?, usuarioRegistra As Decimal?) As List(Of PNC_Productos_Get_Result)
        Return oMatrixContext.PNC_Productos_Get(id, responsable, estado, usuarioRegistra).ToList
    End Function
    Function grabarCausa(productoId As Decimal, causa As String, correccion As String, fechaEstimadaCierre As Date, usuario As Decimal, fechaCreacion As DateTime) As Decimal
        Return oMatrixContext.PNC_Productos_Causas_Add(productoId, causa, correccion, fechaEstimadaCierre, usuario, fechaCreacion).FirstOrDefault
    End Function
    Sub actualizarEstadoProducto(idProducto As Decimal, estado As EEstados)
        oMatrixContext.PNC_Producto_UpdateEstado(idProducto, estado)
    End Sub
    Function obtenerUsuariosNotificar(idPNC As Decimal) As List(Of String)
        Return oMatrixContext.PNC_Productos_CorreosNotificar(idPNC).ToList
    End Function

End Class
