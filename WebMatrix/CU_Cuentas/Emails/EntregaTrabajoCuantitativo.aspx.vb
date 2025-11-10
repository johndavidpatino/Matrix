Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class EntregaTrabajoCuantitativo
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idFicha") IsNot Nothing Then
            Dim estudio As Int64 = Int64.Parse(Request.QueryString("idFicha").ToString())
            CargarElemento(estudio)
            If Request.QueryString("notclose") Is Nothing Then
                GenerarHtml()
                ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
            End If
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal FichaId As Long)
        Dim oEntrega As New CoreProject.EntregasTrabajos
        Dim info = oEntrega.DevolverPreEntregaxTrabajo(FichaId)
        lblNombreTrabajo.Text = info.NombreTrabajo
        lblJobBook.Text = info.JobBook
        lblMetodologia.Text = info.MetNombre
        lblFechaInicio.Text = info.FechaTentativaInicioCampo
        lblFechaFin.Text = info.FechaTentativaFinalizacion
        lblUnidad.Text = info.Unidad
        lblMuestra.Text = info.Muestra
        lblGrupoObjetivo.Text = info.GrupoObjetivo
        lblCubrimientoGeografico.Text = info.CubrimientoGeografico
        lblMarcoMuestral.Text = info.MarcoMuestral
        lblDistribucionMuestra.Text = info.DistribucionMuestra
        lblCuotas.Text = info.Cuotas
        lblDesagregacionResultados.Text = info.NivelDesagregacionResultados
        lblPonderacion.Text = info.Ponderacion
        lblRequerimientosEspeciales.Text = info.RequerimientosEspeciales
        lblOtrasObservaciones.Text = info.OtrasObservaciones
        lblIncentivoEconimico.Text = info.IncentivoEconomico
        lblPresupuestoIncentivo.Text = info.PresupuestoIncentivo
        lblRegalosCliente.Text = info.RegalosCliente
        lblCompraIpsos.Text = info.CompraIpsos
        lblPresupuesto.Text = info.Presupuesto
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