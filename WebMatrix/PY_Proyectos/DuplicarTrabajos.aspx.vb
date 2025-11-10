Imports CoreProject
Imports WebMatrix.Util
Imports System.IO
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf


Public Class DuplicarTrabajos
    Inherits System.Web.UI.Page
#Region "Propiedades"

#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        btnDuplicar.Enabled = True
        Dim Trabajo As New PY_Trabajo0
        If Request.QueryString("trabajoId") IsNot Nothing Then
            hfidTrabajo.Value = Request.QueryString("trabajoId").ToString

        End If

        If Request.QueryString("proyectoId") IsNot Nothing Then
            hfidProyecto.Value = Request.QueryString("ProyectoId").ToString

        End If
        If Not IsPostBack Then
            Trabajo = Cargartrabajo(hfidTrabajo.Value)
            txtNombreTrabajo.Text = Trabajo.NombreTrabajo
            txtNoMedicion.Text = Trabajo.NoMedicion
            txtFechaTentativaInicioCampo.Text = Trabajo.FechaTentativaInicioCampo
            txtFechaTentativaFinalizacion.Text = Trabajo.FechaTentativaFinalizacion
        End If
    End Sub

    Protected Sub btnDuplicar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDuplicar.Click
        Dim IdNuevoTrabajo As Integer
        Dim fechainiori As Date
        Dim fechafinori As Date
        Dim fechaini As Date
        Dim fechafin As Date
        Dim oPlaneacion As New PlaneacionProduccion

        fechainiori = txtFechaTentativaInicioCampo.Text
        fechafinori = txtFechaTentativaFinalizacion.Text
        If CbkMes.Checked = True Then
            fechaini = DateAdd(DateInterval.Month, 1, fechainiori)
            fechafin = DateAdd(DateInterval.Month, 1, fechafinori)
            IdNuevoTrabajo = duplicartrabajo(txtNombreTrabajo.Text, fechaini, fechafin, txtNoMedicion.Text, hfidTrabajo.Value)
            TrabajoConfiguracion(IdNuevoTrabajo, fechaini, fechafin)
            muestra(IdNuevoTrabajo, fechaini, fechafin)
        Else
            IdNuevoTrabajo = duplicartrabajo(txtNombreTrabajo.Text, txtFechaTentativaInicioCampo.Text, txtFechaTentativaFinalizacion.Text, txtNoMedicion.Text, hfidTrabajo.Value)
            TrabajoConfiguracion(IdNuevoTrabajo, fechainiori, fechafinori)
            muestra(IdNuevoTrabajo, txtFechaTentativaInicioCampo.Text, txtFechaTentativaFinalizacion.Text)
        End If

        If CbkEspecificaciones.Checked = True And CbkMes.Checked = True Then
            'TrabajoConfiguracion(IdNuevoTrabajo, fechaini, fechafin)
            duplicarEspecificaciones(hfidTrabajo.Value, IdNuevoTrabajo)
            FichaCuanti(IdNuevoTrabajo)
        ElseIf CbkEspecificaciones.Checked = True And CbkMes.Checked = False Then
            ' TrabajoConfiguracion(IdNuevoTrabajo, fechainiori, fechafinori)
            duplicarEspecificaciones(hfidTrabajo.Value, IdNuevoTrabajo)
            FichaCuanti(IdNuevoTrabajo)
        End If
        'TrabajoConfiguracion(IdNuevoTrabajo, fechaini, fechafin)
        oPlaneacion.AgregarEstimacionAutomatica(IdNuevoTrabajo, Session("IDUsuario").ToString, True, True, True, True, True, True, False, False)
        hilo(IdNuevoTrabajo)
        If CbkDocumentos.Checked = True Then
            copiardocumentos(hfidTrabajo.Value, IdNuevoTrabajo)
        End If
        'EnviarPreentrega()
        'EnviarEmail(IdNuevoTrabajo)
        ShowNotification("Trabajo Duplicado", ShowNotifications.InfoNotification)
        btnDuplicar.Enabled = False
        log(Nothing, 8)
    End Sub

    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Dim o As New Trabajo
        Dim p As New Proyecto
        Dim info = o.ObtenerTrabajo(hfidTrabajo.Value)
        Dim proyecto = p.obtenerXId(info.ProyectoId)
        If proyecto.TipoProyectoId = 1 Then
            Response.Redirect("../Py_Proyectos/Trabajos.aspx?idtrabajo=" & hfidProyecto.Value)
        ElseIf proyecto.TipoProyectoId = 2 Then
            Response.Redirect("../Py_Proyectos/TrabajosCualitativos.aspx?idtrabajo=" & hfidProyecto.Value)
        End If

    End Sub

#End Region
#Region "Metodos"
    Function duplicartrabajo(ByVal nombre As String, ByVal fechaini As Date, ByVal fechafin As Date, ByVal medicion As Integer, ByVal Idtrabajo As Integer)
        Dim InfoTrabajo As New PY_Trabajo0
        Dim trabajo As New Trabajo
        Dim idnuevotrabajo As Integer
        InfoTrabajo = Cargartrabajo(Idtrabajo)
        idnuevotrabajo = trabajo.DuplicarTrabajo(InfoTrabajo.ProyectoId, InfoTrabajo.OP_MetodologiaId, InfoTrabajo.PresupuestoId, nombre, InfoTrabajo.Muestra, fechaini, fechafin, Nothing, InfoTrabajo.Unidad, InfoTrabajo.JobBook, InfoTrabajo.TipoRecoleccionId, InfoTrabajo.Estado, InfoTrabajo.IdPropuesta, InfoTrabajo.Alternativa, InfoTrabajo.MetCodigo, InfoTrabajo.Fase, medicion)
        Return idnuevotrabajo
    End Function

    Sub duplicarEspecificaciones(ByVal IdTrabajoOld As Integer, ByVal IdTrabajoNew As Integer)
        Dim flag As Boolean = False
        Dim o As New Proyecto
        Dim ent As New List(Of PY_EspecifTecTrabajo)
        ent = o.ObtenerEspecifacionesList(IdTrabajoOld)
        For Each item As PY_EspecifTecTrabajo In ent
            item.TrabajoId = IdTrabajoNew
            o.GuardarInfoEspecificaciones(item)
        Next
    End Sub

    Sub muestra(ByVal Trabjoid As Integer, ByVal fechai As Date, ByVal fechaf As Date)
        Dim oCoorCampo As New CoordinacionCampo
        Dim Muestras As New Trabajo
        Dim listaMuestra = (From lmuestra In oCoorCampo.ObtenerMuestraxEstudioList(hfidTrabajo.Value)
                            Select idMuestra = lmuestra.Id, ciudad = lmuestra.CiudadId,
                            cantidad = lmuestra.Cantidad, coordinador = lmuestra.Coordinador, fechainicio = lmuestra.FechaInicio, fechafin = lmuestra.FechaFin).OrderBy(Function(x) x.ciudad)
        For Each result In listaMuestra
            Muestras.DuplicarMuestra(result.ciudad, Trabjoid, result.cantidad, result.coordinador, fechai, fechaf)
        Next
    End Sub
    Function Cargartrabajo(ByVal IdTrabjo As Integer)
        Dim trabajo As New Trabajo
        Dim ent As New PY_Trabajo0
        ent = trabajo.ObtenerTrabajo(hfidTrabajo.Value)
        Return ent
    End Function

    Sub TrabajoConfiguracion(ByVal Trabjoid As Integer, ByVal fechai As Date, ByVal fechaf As Date)
        Dim trabajo As New Trabajo
        Dim ent As List(Of OP_TrabajoConfiguracion_Get_Result)
        Dim TrabajoConfiguracion As New Trabajo
        ent = trabajo.trabajoconfiguracionget(hfidTrabajo.Value)
        If ent.Count = 0 Then
            ShowNotification("No Existe Configuracion del Trabajo para Duplicar", ShowNotifications.InfoNotification)
            Exit Sub
        End If
        TrabajoConfiguracion.guardartrabajoconfiguracion(Trabjoid, fechai, fechaf, ent(0).PorcentajeVerificacion, ent(0).UnidadCritica)
    End Sub

    Sub FichaCuanti(ByVal trabajoid As Integer)
        Dim Ficha As New FichaCuantitativo
        Dim EntFicha As List(Of OP_FichaCuantitativo_Get_Result)
        EntFicha = Ficha.DevolverxTrabajoID(hfidTrabajo.Value)
        If EntFicha.Count > 0 Then
            Ficha.Guardar(Nothing, trabajoid, EntFicha.Item(0).GrupoObjetivo, EntFicha.Item(0).CubrimientoGeografico, EntFicha.Item(0).MarcoMuestral, EntFicha.Item(0).DistribucionMuestra, EntFicha.Item(0).Cuotas, EntFicha.Item(0).NivelDesagregacionResultados, EntFicha.Item(0).Ponderacion, EntFicha.Item(0).RequerimientosEspeciales, EntFicha.Item(0).OtrasObservaciones, EntFicha.Item(0).IncentivoEconomico, EntFicha.Item(0).PresupuestoIncentivo, EntFicha.Item(0).RegalosCliente, EntFicha.Item(0).CompraIpsos, EntFicha.Item(0).Presupuesto)
        End If
    End Sub

    Sub hilo(ByVal trabajoid As Integer)
        Dim tareashilo As New WorkFlow
        Dim prueba As Int64
        Dim hilo As New Hilo
        Dim Id As List(Of CORE_HilosGetxContenedor_Result)
        Dim tareas As List(Of CORE_WorkFlow_GetxHiloid_Result)
        Dim CORE As New WorkFlow
        'Dim i As Integer
        Id = hilo.Obtenerhilo(hfidTrabajo.Value)
        tareas = CORE.ObtenerTareasxHiloid(Id.Item(0).ID)
        prueba = hilo.GuardarHilo(Id.Item(0).TIPOHILOID, trabajoid)
        tareashilo.GrabarTareas(prueba)
        'prueba = CORE.CrearHiloCrearTareas(Id.Item(0).TIPOHILOID, trabajoid)
        'For i = 0 To tareas.Count - 1
        'CORE.GuardarDuplicados(tareas.Item(i).TareaId, prueba, tareas.Item(i).FIniP, tareas.Item(i).FFinP, tareas.Item(i).FIniR, tareas.Item(i).FFinR, tareas.Item(i).UsuarioEstima, tareas.Item(i).UsuarioAsignado, tareas.Item(i).Estado, tareas.Item(i).FechaUltimoEstado, tareas.Item(i).ObservacionesPlaneacion, tareas.Item(i).ObservacionesEjecucion, tareas.Item(i).Descripcion)
        'Next
    End Sub

    Sub copiardocumentos(ByVal trabajo As Integer, ByVal idtrabajonuevo As Integer)
        Dim o As New RepositorioDocumentos
        Dim res As List(Of GD_RepositorioDocumentos)
        Dim url As String()
        Dim ruta1 As String
        Dim ruta2 As String
        Dim ruta3 As String
        Dim rutafisica As String
        Dim re As New GD_Documentos
        res = o.GD_RepositorioDocumentos_GetXtrabajo(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, hfidTrabajo.Value)
        If res.Count = 0 Then
            Exit Sub
        End If
        url = res(0).Url.Split(trabajo.ToString)
        Dim Nombres(200) As String
        Dim Carpetas(200) As String
        Dim Tem As String()
        Dim largo As Integer
        For i = 0 To res.Count - 1
            Tem = res(i).Url.Split("\")
            largo = Tem.Length
            Nombres(i) = Tem(largo - 1).ToString
            Carpetas(i) = Tem(largo - 2).ToString
        Next
        rutafisica = "C:\inetpub\wwwroot\Matrix\ArchivosCargados\"
        ruta1 = rutafisica & url(0).ToString & res(0).IdContenedor
        ruta2 = rutafisica & url(0).ToString & idtrabajonuevo
        My.Computer.FileSystem.CopyDirectory(ruta1, ruta2, True)
        ruta3 = url(0).ToString & idtrabajonuevo
        For i = 0 To res.Count - 1
            o.Grabar(res(i).Nombre, ruta3 & "\" & Carpetas(i) & "\" & Nombres(i), res(i).DocumentoId, res(i).Version, res(i).Fecha, res(i).Comentarios, res(i).UsuarioId, idtrabajonuevo)
        Next

    End Sub

    Sub EnviarEmail(ByVal idtrabajonuevo As Integer)
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NuevoTrabajoGerenciaOp.aspx?idTrabajo=" & idtrabajonuevo)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Sub EnviarPreentrega()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfidTrabajo.Value) Or (hfidTrabajo.Value = "0") Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una preentrega")
            End If
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/PreEntregaTrabajo.aspx?idTrabajo=" & hfidTrabajo.Value)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
#End Region


    Private Sub CbkMes_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CbkMes.CheckedChanged
        If CbkMes.Checked = True Then
            txtFechaTentativaFinalizacion.Enabled = False
            txtFechaTentativaInicioCampo.Enabled = False
        ElseIf CbkMes.Checked = False Then
            txtFechaTentativaFinalizacion.Enabled = True
            txtFechaTentativaInicioCampo.Enabled = True
        End If
    End Sub

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(38, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub


End Class