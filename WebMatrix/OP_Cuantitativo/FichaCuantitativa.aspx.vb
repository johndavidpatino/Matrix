Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class FichaCuantitativa
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _jobbook As String
    Private VolverCoord As Boolean
    Public Property jobbook() As String
        Get
            Return _jobbook
        End Get
        Set(ByVal value As String)
            _jobbook = value
        End Set
    End Property
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("idtrabajo") IsNot Nothing Then
                Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
                hfidtrabajo.Value = idtrabajo
                CargarFichaCuantitativa()
                CargarHabeasData(hfidtrabajo.Value)
                If Not (hfidfichacuantitativa.Value = "") Then
                    CargarInfo(hfidfichacuantitativa.Value)
                End If
                If Request.QueryString("op") IsNot Nothing Then
                    btnGuardar.Visible = False
                    btnCancelar.Visible = False
                    btnEntrega.Visible = False
                    btnVolverOP.Visible = True
                End If
                If Request.QueryString("Coordinador") IsNot Nothing Then
                    VolverCoord = True
                Else
                    VolverCoord = False
                End If
            Else
                Response.Redirect("~/Home.aspx")
            End If
        End If
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        GuardarFichaCuantitativa()
        ActualizarHabeasData(hfidtrabajo.Value)
        ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
        'Limpiar()
        'CargarFichaCuantitativa()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnEntrega_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEntrega.Click
        If String.IsNullOrEmpty(hfidfichacuantitativa.Value) Then
            Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una entrega")
        End If
        Dim script As String = "window.open('../Emails/EntregaTrabajoCuantitativo.aspx?idFicha=" & Request.QueryString("idtrabajo").ToString & "','cal','width=400,height=250,left=270,top=180')"
        Dim page As Page = DirectCast(Context.Handler, Page)
        ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Dim idtrabajo As Int64 = Int64.Parse(hfidtrabajo.Value)
        Dim oTrabajo As New Trabajo
        Dim info = oTrabajo.DevolverxID(idtrabajo)
        Response.Redirect("../PY_Proyectos/Trabajos.aspx?ProyectoId=" & info.ProyectoId)
    End Sub
    Protected Sub btnVolverOP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolverOP.Click
        If Request.QueryString("Coordinador") IsNot Nothing Then
            If Request.QueryString("coordinador").ToString = 2 Then
                Response.Redirect("../OP_Cuantitativo/TrabajosCallCenter.aspx")
            Else

            End If

        Else
            Response.Redirect("../OP_Cuantitativo/Trabajos.aspx")
        End If
    End Sub
#End Region

#Region "Funciones y Metodos"
    Public Sub Limpiar()
        rblIncentivos.SelectedValue = "0"
        rblRegaloClientes.SelectedValue = "0"
        rblCompraIpsos.SelectedValue = "0"
        hfidfichacuantitativa.Value = String.Empty
        txtGrupoObjetivo.Text = String.Empty
        txtCubrimientoGeografico.Text = String.Empty
        txtMarcoMuestral.Text = String.Empty
        txtDistribucionMuestral.Content = String.Empty
        txtCuotas.Text = String.Empty
        txtNivelDesagregacionResultados.Text = String.Empty
        txtPonderacion.Text = String.Empty
        txtRequerimientosEspeciales.Text = String.Empty
        txtOtrasObservaciones.Text = String.Empty
        txtHabeasData.Text = String.Empty

    End Sub
    Public Function CargarFichaCuantitativa() As Long
        Dim idtrabajo As Int64 = Int64.Parse(hfidtrabajo.Value)
        Dim oTrabajo As New Trabajo
        Dim info = oTrabajo.DevolverxID(idtrabajo)
        txtJobBook.Text = info.JobBook
        txtNombreProyecto.Text = info.NombreTrabajo

        Dim oFichaCuantitativa As New FichaCuantitativo
        Dim lfichaCuantitativa = (From lficha In oFichaCuantitativa.DevolverxTrabajoID(idtrabajo)
                                  Select Id = lficha.id,
                                  JobBook = lficha.JobBook,
                                  NombreTrabajo = lficha.NombreTrabajo, GrupoObjetivo = lficha.GrupoObjetivo, CubrimientoGeografico = lficha.CubrimientoGeografico, MarcoMuestral = lficha.MarcoMuestral,
                                  DistribucionMuestra = lficha.DistribucionMuestra, Cuotas = lficha.Cuotas, NivelDesagregacionResultados = lficha.NivelDesagregacionResultados, Ponderacion = lficha.Ponderacion, RequerimientosEspeciales = lficha.RequerimientosEspeciales, OtrasObservaciones = lficha.OtrasObservaciones,
                                  IncentivoEconomico = lficha.IncentivoEconomico,
                                  PresupuestoIncentivo = lficha.PresupuestoIncentivo,
                                  RegalosCliente = lficha.RegalosCliente,
                                  CompraIpsos = lficha.CompraIpsos,
                                  Presupuesto = lficha.Presupuesto).OrderBy(Function(f) f.Id)
        If lfichaCuantitativa.ToList.Count = 0 Then
            Return Nothing
        Else
            hfidfichacuantitativa.Value = lfichaCuantitativa.ToList.Item(0).Id
            Return lfichaCuantitativa.ToList.Item(0).Id
        End If
    End Function
    Public Sub CargarInfo(ByVal idFichaCuantitativa As Int64)

        Dim oFichaCuantitativo As New FichaCuantitativo
        Dim info = oFichaCuantitativo.DevolverxID(idFichaCuantitativa)
        hfidfichacuantitativa.Value = idFichaCuantitativa

        If info.RegalosCliente Then
            rblRegaloClientes.SelectedValue = "1"
        Else
            rblRegaloClientes.SelectedValue = "0"
        End If

        If info.CompraIpsos Then
            rblCompraIpsos.SelectedValue = "1"
        Else
            rblCompraIpsos.SelectedValue = "0"
        End If


        txtGrupoObjetivo.Text = info.GrupoObjetivo
        If txtGrupoObjetivo.Text = "" Then
            Dim oTrabajo As New Trabajo
            Dim infoT = oTrabajo.DevolverxID(info.TrabajoId)
            Dim oProyecto As New Proyecto
            Dim infoP = oProyecto.obtenerXId(infoT.ProyectoId)
            Dim oEstudio As New Estudio
            Dim infoE = oEstudio.ObtenerXID(infoP.EstudioId)
            Dim oPropuesta As New Propuesta
            Dim infoPr = oPropuesta.DevolverxID(infoE.PropuestaId)
            Dim oBrief As New Brief
            Dim infoB = oBrief.DevolverxID(infoPr.Brief)
            Me.txtGrupoObjetivo.Text = infoB.TargetGroup
        End If
        txtCubrimientoGeografico.Text = info.CubrimientoGeografico
        txtMarcoMuestral.Text = info.MarcoMuestral
        txtDistribucionMuestral.Content = info.DistribucionMuestra
        txtCuotas.Text = info.Cuotas
        txtNivelDesagregacionResultados.Text = info.NivelDesagregacionResultados
        txtPonderacion.Text = info.Ponderacion
        txtRequerimientosEspeciales.Text = info.RequerimientosEspeciales
        txtOtrasObservaciones.Text = info.OtrasObservaciones

        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Public Sub CargarHabeasData(ByVal idTrabajo As Int64)
        Dim oTrabajo As New Trabajo
        Dim oPropuesta As New Propuesta
        Dim trabajo = oTrabajo.obtenerXId(idTrabajo)
        Dim idPropuesta = trabajo.IdPropuesta
        Dim info = oPropuesta.DevolverxID(idPropuesta)

        If info.RequestHabeasData IsNot Nothing Then
            txtHabeasData.Text = info.RequestHabeasData
        End If

    End Sub
    Public Sub GuardarFichaCuantitativa()
        Dim TrabajoId, FichaCuantitativaId As Int64
        Dim GrupoObjetivo = "", CubrimientoGeografico = "", MarcoMuestral = "", DistribucionMuestra = "", Cuotas = "", NivelDesagregacionResultados = "", Ponderacion = "", RequerimientosEspeciales = "", OtrasObservaciones As String = ""
        Dim IncentivoEconomico, RegalosCliente, CompraIpsos As Boolean
        Dim PresupuestoIncentivo = 0, Presupuesto As Double = 0

        TrabajoId = Int64.Parse(hfidtrabajo.Value)

        If Not String.IsNullOrEmpty(hfidfichacuantitativa.Value) Then
            FichaCuantitativaId = Int64.Parse(hfidfichacuantitativa.Value)
        End If

        If Not String.IsNullOrEmpty(txtGrupoObjetivo.Text) Then
            GrupoObjetivo = txtGrupoObjetivo.Text
        End If

        If Not String.IsNullOrEmpty(txtCubrimientoGeografico.Text) Then
            CubrimientoGeografico = txtCubrimientoGeografico.Text
        End If

        If Not String.IsNullOrEmpty(txtMarcoMuestral.Text) Then
            MarcoMuestral = txtMarcoMuestral.Text
        End If

        If Not String.IsNullOrEmpty(txtDistribucionMuestral.Content) Then
            DistribucionMuestra = txtDistribucionMuestral.Content
        End If

        If Not String.IsNullOrEmpty(txtCuotas.Text) Then
            Cuotas = txtCuotas.Text
        End If

        If Not String.IsNullOrEmpty(txtNivelDesagregacionResultados.Text) Then
            NivelDesagregacionResultados = txtNivelDesagregacionResultados.Text
        End If

        If Not String.IsNullOrEmpty(txtPonderacion.Text) Then
            Ponderacion = txtPonderacion.Text
        End If

        If Not String.IsNullOrEmpty(txtRequerimientosEspeciales.Text) Then
            RequerimientosEspeciales = txtRequerimientosEspeciales.Text
        End If

        If Not String.IsNullOrEmpty(txtOtrasObservaciones.Text) Then
            OtrasObservaciones = txtOtrasObservaciones.Text
        End If

        Select Case rblIncentivos.SelectedValue
            Case "0"
                IncentivoEconomico = False
            Case "1"
                IncentivoEconomico = True
        End Select

        Select Case rblRegaloClientes.SelectedValue
            Case "0"
                RegalosCliente = False
            Case "1"
                RegalosCliente = True
        End Select

        Select Case rblCompraIpsos.SelectedValue
            Case "0"
                CompraIpsos = False
            Case "1"
                CompraIpsos = True
        End Select

        Dim oFichaCuantitativo As New FichaCuantitativo
        If hfidfichacuantitativa.Value = "" Then
            hfidfichacuantitativa.Value = oFichaCuantitativo.Guardar(FichaCuantitativaId, TrabajoId, GrupoObjetivo, CubrimientoGeografico, MarcoMuestral, DistribucionMuestra, Cuotas, NivelDesagregacionResultados, Ponderacion, RequerimientosEspeciales, OtrasObservaciones, IncentivoEconomico, PresupuestoIncentivo, RegalosCliente, CompraIpsos, Presupuesto)
            EnviarEmail(True)
        Else
            oFichaCuantitativo.Guardar(FichaCuantitativaId, TrabajoId, GrupoObjetivo, CubrimientoGeografico, MarcoMuestral, DistribucionMuestra, Cuotas, NivelDesagregacionResultados, Ponderacion, RequerimientosEspeciales, OtrasObservaciones, IncentivoEconomico, PresupuestoIncentivo, RegalosCliente, CompraIpsos, Presupuesto)
            EnviarEmail(False)
        End If
    End Sub

    Sub ActualizarHabeasData(ByVal idTrabajo As Int64)
        Dim oTrabajo As New Trabajo
        Dim oPropuesta As New Propuesta
        Dim trabajo = oTrabajo.obtenerXId(idTrabajo)
        Dim idPropuesta = trabajo.IdPropuesta

        If Not txtHabeasData.Text = "" Then
            oPropuesta.ActualizarPropuesta_HabeasData(idPropuesta, txtHabeasData.Text)
        End If
    End Sub

    Sub EnviarEmail(ByVal Nuevo As Boolean)
        Dim oEnviarCorreo As New EnviarCorreo
        If String.IsNullOrEmpty(hfidfichacuantitativa.Value) Then
            Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una entrega")
        End If
        oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EntregaTrabajoCuantitativo.aspx?idFicha=" & Request.QueryString("idtrabajo").ToString & "&nuevo=" & Nuevo)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub
#End Region

End Class