Imports CoreProject
Imports WebMatrix.Util

Public Class InstructivoGeneralCuali
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Session("TrabajoId") Is Nothing Then
                'Response.Redirect("Trabajos.aspx")
            Else
                cargarAyudas()
                cargarReclutamientos()
                hfTrabajoID.Value = Session("TrabajoId").ToString
                Dim o As New Proyecto

                hfversion.Value = 0
                CargarInfo()
                If hfversion.Value > 0 Then
                    pVersion.InnerText = "Versión " + Convert.ToString(hfversion.Value)
                End If

            End If
            txtModerador.Focus()
        End If
    End Sub

    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Dim ProyectoId = Session("ProyectoId")
        Response.Redirect("TrabajosCualitativos.aspx?ProyectoId=" + ProyectoId + "&TrabajoId=" + hfTrabajoID.Value + "&CargaT=1")
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim flag As Boolean = False
        Dim entAnt As New PY_EspecifTecTrabajoCuali
        entAnt = Session("ent")
        Dim o As New CoreProject.SegmentosCuali
        Dim ent As New PY_EspecifTecTrabajoCuali
        ent = o.ObtenerEspecifacionesCuali(hfTrabajoID.Value)
        If ent.TrabajoId = 0 Then flag = True
        ent.TrabajoId = hfTrabajoID.Value
        ent.Moderador = txtModerador.Text
        ent.EspecificacionesCampo = txtEspecificacionesCampo.Content
        ent.MaterialApoyo = txtMaterialApoyo.Text
        ent.Incidencias = txtIncidencias.Text
        ent.Auditoria = txtAuditoria.Text
        ent.VCSeguridad = txtVCSeguridad.Text
        ent.VCObtencion = txtVCObtencion.Text
        ent.VCGrupoObjetivo = txtVCGrupoObjetivo.Content
        ent.VCAplicacionInstrumentos = txtVCAplicacionInstrumentos.Text
        ent.VCDistribucionCuotas = txtVCDistribucionCuotas.Content
        ent.VCMetodologia = txtVCMetodologia.Text
        ent.Incentivos = rblIncentivos.SelectedValue
        ent.PresupuestoIncentivo = txtPresupuestoIncentivo.Text
        ent.DistribucionIncentivo = txtDistribucionIncentivo.Text
        ent.RegaloClientes = rblRegaloClientes.SelectedValue
        ent.CompraIpsos = rblCompraIpsos.SelectedValue
        ent.PresupuestoCompra = txtPresupuestoCompra.Text
        ent.DistribucionCompra = txtDistribucionCompra.Text
        ent.ExclusionesyRestricciones = txtExclusionesyRestricciones.Text
        ent.RecursosPropiedadesCliente = txtRecursosPropiedadesCliente.Text
        ent.HabeasData = txtHabeasData.Text
        ent.OtrasEspecificaciones = txtOtrasEspecificaciones.Content

        ent.Usuario = Session("IDUsuario").ToString
        ent.Fecha = DateTime.Now.ToString
        ent.NoVersion = hfversion.Value + 1

        If ent.NoVersion > 1 Then
            Dim cambios = 0
            If entAnt.Moderador <> ent.Moderador Then cambios = cambios + 1
            If entAnt.EspecificacionesCampo <> ent.EspecificacionesCampo Then cambios = cambios + 1
            If entAnt.MaterialApoyo <> ent.MaterialApoyo Then cambios = cambios + 1
            If entAnt.Incidencias <> ent.Incidencias Then cambios = cambios + 1
            If entAnt.Auditoria <> ent.Auditoria Then cambios = cambios + 1
            If entAnt.VCSeguridad <> ent.VCSeguridad Then cambios = cambios + 1
            If entAnt.VCObtencion <> ent.VCObtencion Then cambios = cambios + 1
            If entAnt.VCGrupoObjetivo <> ent.VCGrupoObjetivo Then cambios = cambios + 1
            If entAnt.VCAplicacionInstrumentos <> ent.VCAplicacionInstrumentos Then cambios = cambios + 1
            If entAnt.VCDistribucionCuotas <> ent.VCDistribucionCuotas Then cambios = cambios + 1
            If entAnt.VCMetodologia <> ent.VCMetodologia Then cambios = cambios + 1
            If entAnt.Incentivos <> ent.Incentivos Then cambios = cambios + 1
            If entAnt.PresupuestoIncentivo <> ent.PresupuestoIncentivo Then cambios = cambios + 1
            If entAnt.DistribucionIncentivo <> ent.DistribucionIncentivo Then cambios = cambios + 1
            If entAnt.RegaloClientes <> ent.RegaloClientes Then cambios = cambios + 1
            If entAnt.CompraIpsos <> ent.CompraIpsos Then cambios = cambios + 1
            If entAnt.PresupuestoCompra <> ent.PresupuestoCompra Then cambios = cambios + 1
            If entAnt.DistribucionCompra <> ent.DistribucionCompra Then cambios = cambios + 1
            If entAnt.ExclusionesyRestricciones <> ent.ExclusionesyRestricciones Then cambios = cambios + 1
            If entAnt.RecursosPropiedadesCliente <> ent.RecursosPropiedadesCliente Then cambios = cambios + 1
            If entAnt.HabeasData <> ent.HabeasData Then cambios = cambios + 1
            If entAnt.OtrasEspecificaciones <> ent.OtrasEspecificaciones Then cambios = cambios + 1

            If cambios = 0 Then
                ShowNotification("No hay cambios para guardar", ShowNotifications.ErrorNotification)
                Exit Sub
            End If
        End If

        Dim especificacionCuali = o.GuardarInfoEspecificacionesCuali(ent)
        If especificacionCuali = 0 Then
            ShowNotification("No se pudieron guardar estas especificaciones", ShowNotifications.ErrorNotification)
        Else
            hfversion.Value = ent.NoVersion
            GuardarAyudas()
            GuardarTipoReclutamiento()
            pVersion.InnerText = "Versión " + Convert.ToString(hfversion.Value)
            Session("ent") = ent

            'EnviarEmail(flag)
            ShowNotification("Nueva Versión guardada correctamente", ShowNotifications.InfoNotification)
            'Response.Redirect("TrabajosCualitativos.aspx")
        End If
    End Sub

    Sub CargarInfo()
        Dim o As New CoreProject.SegmentosCuali
        Dim ent As New PY_EspecifTecTrabajoCuali
        ent = o.ObtenerEspecifacionesCualiLast(hfTrabajoID.Value)
        If ent.TrabajoId = hfTrabajoID.Value Then
            hfTrabajoID.Value = ent.TrabajoId
            txtModerador.Text = ent.Moderador
            txtEspecificacionesCampo.Content = ent.EspecificacionesCampo
            txtMaterialApoyo.Text = ent.MaterialApoyo
            txtIncidencias.Text = ent.Incidencias
            txtAuditoria.Text = ent.Auditoria
            txtVCSeguridad.Text = ent.VCSeguridad
            txtVCObtencion.Text = ent.VCObtencion
            txtVCGrupoObjetivo.Content = ent.VCGrupoObjetivo
            txtVCAplicacionInstrumentos.Text = ent.VCAplicacionInstrumentos
            txtVCDistribucionCuotas.Content = ent.VCDistribucionCuotas
            txtVCMetodologia.Text = ent.VCMetodologia
            rblIncentivos.SelectedValue = ent.Incentivos
            If rblIncentivos.SelectedValue = 1 Then
                txtPresupuestoIncentivo.Enabled = True
                txtDistribucionIncentivo.Enabled = True
            Else
                txtPresupuestoIncentivo.Enabled = False
                txtDistribucionIncentivo.Enabled = False
            End If
            txtPresupuestoIncentivo.Text = ent.PresupuestoIncentivo
            txtDistribucionIncentivo.Text = ent.DistribucionIncentivo
            rblRegaloClientes.SelectedValue = ent.RegaloClientes
            rblCompraIpsos.SelectedValue = ent.CompraIpsos
            If rblCompraIpsos.SelectedValue = 1 Then
                txtPresupuestoCompra.Enabled = True
                txtDistribucionCompra.Enabled = True
            Else
                txtPresupuestoCompra.Enabled = False
                txtDistribucionCompra.Enabled = False
            End If
            txtPresupuestoCompra.Text = ent.PresupuestoCompra
            txtDistribucionCompra.Text = ent.DistribucionCompra
            txtExclusionesyRestricciones.Text = ent.ExclusionesyRestricciones
            txtRecursosPropiedadesCliente.Text = ent.RecursosPropiedadesCliente
            txtHabeasData.Text = ent.HabeasData
            txtOtrasEspecificaciones.Content = ent.OtrasEspecificaciones
            hfversion.Value = o.ObtenerEspecifacionesContar(hfTrabajoID.Value)

            ObtenerAyudas()
            ObtenerTipoReclutamiento()
        End If
        Session("ent") = ent
    End Sub

    Sub EnviarEmail(ByVal Nuevo As Boolean)
        Dim oEnviarCorreo As New EnviarCorreo
        If String.IsNullOrEmpty(hfTrabajoID.Value) Then
            Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una entrega")
        End If
        oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EntregaTrabajoCuantitativo.aspx?idFicha=" & hfTrabajoID.Value & "&nuevo=" & Nuevo & "&version=" & hfversion.Value)
    End Sub

    Sub cargarAyudas()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        Dim ayudas = oSegmentos.ObtenerAyudasCuali()
        chbAyudas.DataSource = ayudas
        chbAyudas.DataTextField = "Ayuda"
        chbAyudas.DataValueField = "id"
        chbAyudas.DataBind()
    End Sub

    Sub cargarReclutamientos()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        Dim reclutamiento = oSegmentos.ObtenerTipoReclutamiento()
        chbReclutamiento.DataSource = reclutamiento
        chbReclutamiento.DataTextField = "Tipo"
        chbReclutamiento.DataValueField = "id"
        chbReclutamiento.DataBind()
    End Sub

    Sub GuardarAyudas()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        For Each li As ListItem In chbAyudas.Items
            oSegmentos.GuardarAyudas(hfTrabajoID.Value, li.Value, li.Selected)
        Next
    End Sub

    Sub ObtenerAyudas()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        For i As Integer = 0 To oSegmentos.ObtenerAyudasRequeridasCualiList(hfTrabajoID.Value).ToList.Count - 1
            Dim val As Integer = oSegmentos.ObtenerAyudasRequeridasCualiList(hfTrabajoID.Value).Item(i).TipoAyuda
            For Each li As ListItem In chbAyudas.Items
                If li.Value = val Then
                    li.Selected = True
                    Exit For
                End If
            Next
        Next

    End Sub

    Sub GuardarTipoReclutamiento()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        For Each li As ListItem In chbReclutamiento.Items
            oSegmentos.GuardarTipoReclutamiento(hfTrabajoID.Value, li.Value, li.Selected)
        Next
    End Sub

    Sub ObtenerTipoReclutamiento()
        Dim oSegmentos As New CoreProject.SegmentosCuali
        For i As Integer = 0 To oSegmentos.ObtenerReclutamientoRequeridoCualiList(hfTrabajoID.Value).ToList.Count - 1
            Dim val As Integer = oSegmentos.ObtenerReclutamientoRequeridoCualiList(hfTrabajoID.Value).Item(i).TipoReclutamiento
            For Each li As ListItem In chbReclutamiento.Items
                If li.Value = val Then
                    li.Selected = True
                    Exit For
                End If
            Next
        Next

    End Sub

    Protected Sub rblIncentivos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblIncentivos.SelectedIndexChanged
        If rblIncentivos.SelectedValue = "1" Then
            txtPresupuestoIncentivo.Enabled = True
            txtDistribucionIncentivo.Enabled = True
        Else
            txtPresupuestoIncentivo.Enabled = False
            txtPresupuestoIncentivo.Text = ""
            txtDistribucionIncentivo.Enabled = False
            txtDistribucionIncentivo.Text = ""
        End If
    End Sub

    Protected Sub rblCompraIpsos_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles rblCompraIpsos.SelectedIndexChanged
        If rblCompraIpsos.SelectedValue = "1" Then
            txtPresupuestoCompra.Enabled = True
            txtDistribucionCompra.Enabled = True
        Else
            txtPresupuestoCompra.Enabled = False
            txtPresupuestoCompra.Text = ""
            txtDistribucionCompra.Enabled = False
            txtDistribucionCompra.Text = ""
        End If
    End Sub
End Class