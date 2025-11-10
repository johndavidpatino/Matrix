Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class EntregaTrabajoEntrevistas
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idFicha") IsNot Nothing Then
            Dim estudio As Int64 = Int64.Parse(Request.QueryString("idFicha").ToString())
            CargarElemento(estudio)
            GenerarHtml()
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal FichaId As Long)
        Dim oEntrega As New CoreProject.EntregasTrabajos
        Dim info = oEntrega.DevolverPreEntregaxEntrevistas(FichaId)
        lblNombreTrabajo.Text = info.NombreTrabajo
        lblJobBook.Text = info.JobBook
        lblMetodologia.Text = info.MetNombre
        lblFechaInicio.Text = info.FechaTentativaInicioCampo
        lblFechaFin.Text = info.FechaTentativaFinalizacion
        lblUnidad.Text = info.Unidad
        lblMuestra.Text = info.Muestra

        lblGrupoObjetivo.Text = info.GrupoObjetivo
        lblCaracteristicasEspeciales.Text = info.CaracteristicasEspeciales
        lblComentarios.Text = info.Comentarios
        lblMetodoAceptableReclutamiento.Text = info.MetodoAceptableReclutamiento
        lblExclusionesYRestriccionesEspecificas.Text = info.ExclusionesYRestriccionesEspecificas
        lblRecursosPropiedadCliente.Text = info.RecursosPropiedadCliente
        lblObservaciones.Text = info.Observaciones

        lblCantidadRequerida.Text = info.CantidadRequerida
        lblAsistentesRequeridos.Text = info.AsistentesRequeridos
        lblFlashReport.Text = info.FlashReport
        lblFlashReportEscrito.Text = info.FlashReportEscrito
        lblFlashReportVerbal.Text = info.FlashReportVerbal
        lblCallCenter.Text = info.CallCenter
        lblSacaCita.Text = info.SacaCita
        lblTranscripcion.Text = info.Transcripcion
        lblGrabacion.Text = info.Grabacion

        lblIncentivoEconomico.Text = info.IncentivoEconomico
        lblPresupuestoIncentivo.Text = info.PresupuestoIncentivo
        lblDescripcionIncentivo.Text = info.DescripcionIncentivo
        lblRegalosCliente.Text = info.RegalosCliente
        lblCompraIpsos.Text = info.CompraIpsos
        lblPresupuesto.Text = info.Presupuesto

        lblCircuitoCerrado.Text = info.lblCircuitoCerrado
        lblFilmacionFija.Text = info.lblFilmacionFija
        lblCamaraFotografica.Text = info.lblCamaraFotografica
        lblTV_DVD.Text = info.lblTV_DVD
        lblFilmacionActiva.Text = info.lblFilmacionActiva
        lblVideoBeam_Laptop.Text = info.lblVideoBeam_Laptop
        lblEntregaFiltroAsistentes.Text = info.lblEntregaFiltroAsistentes
        lblEntregaFiltroReclutamiento.Text = info.lblEntregaFiltroReclutamiento
        lblEntregaCartaInvitacion.Text = info.lblEntregaCartaInvitacion
        lblEntregaFaxConfirmacion.Text = info.lblEntregaFaxConfirmacion


        lblAsunto.Text = lblAsunto.Text & " " & info.JobBook & " " & info.NombreTrabajo

    End Sub

    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        Dim destinatarios As New List(Of String)
        destinatarios.Add("John.Patino@ipsos.com")
        destinatarios.Add("Cesar.Verano@ipsos.com")
        destinatarios.Add("sistemamatrixtempo@gmail.com")
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region

End Class