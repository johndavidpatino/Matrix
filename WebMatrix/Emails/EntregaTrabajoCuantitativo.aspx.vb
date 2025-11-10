Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class EntregaTrabajoCuantitativo
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim script As String = "setTimeout('window.close();',50)"
        'Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idFicha") IsNot Nothing Then
            Dim estudio As Int64 = Int64.Parse(Request.QueryString("idFicha").ToString())
            Dim version As Int64 = Int64.Parse(Request.QueryString("version").ToString())
            If version = Nothing Or version = 0 Then
                version = 1
            End If
            CargarElemento(estudio, version)
            If Request.QueryString("notclose") Is Nothing Then
                GenerarHtml()
                'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
            End If
        Else
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal FichaId As Long, ByVal version As Int32)
        Dim o As New Proyecto
        Dim ent As New PY_EspecifTecTrabajo
        Dim oT As New Trabajo
        Dim infoT = oT.ObtenerTrabajo(FichaId)
        lblAsunto.Text = "Matrix." & " " & infoT.JobBook & " ID: " & infoT.id & " " & infoT.NombreTrabajo & " - Especificaciones del trabajo"
        Try
            ent = o.ObtenerEspecifacionesxVersion(FichaId, version)
            txtAuditoriaCampo.Text = ent.AuditoriaCampo
            txtCodificacion.Text = ent.Codificacion
            txtCritica.Text = ent.Critica
            txtEspecificacionesCampo.Text = ent.EspecifacionesCampo
            txtEstadistica.Text = ent.Estadistica
            txtIncidencias.Text = ent.Incidencias
            txtMaterialApoyo.Text = ent.MaterialApoyo
            txtOtrasEspecificaciones.Text = ent.OtrasEspecificaciones
            txtPilotosCalidad.Text = ent.PilotosCalidad
            txtPilotos.Text = ent.PilotosCampo
            txtProcesamiento.Text = ent.Procesamiento
            txtVerificacion.Text = ent.Verificacion
            txtVCSeguridad.Text = ent.VCSeguridad
            txtVCObtencion.Text = ent.VCObtencion
            txtVCGrupoObjetivo.Text = ent.VCGrupoObjetivo
            txtVCAplicacionInstrumentos.Text = ent.VCAplicacionInstrumentos
            txtVCDistribucionCuotas.Text = ent.VCDistribucionCuotas
            txtVCMetodologia.Text = ent.VCMetodologia
        Catch ex As Exception

        End Try
    End Sub

    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        Dim destinatarios As New List(Of String)
        Dim trabajo As Int64 = Int64.Parse(Request.QueryString("idFicha").ToString())
        Dim oEntrega As New CoreProject.EntregasTrabajos
		'Dim info = oEntrega.DevolverPreEntregaxTrabajo(trabajo)
		Dim o As New DestinatariosCorreos
        Dim oUsuarios As New US.Usuarios
        If Boolean.Parse(Request.QueryString("nuevo").ToString()) = False Then
            lblAsunto.Text = lblAsunto.Text & " - Nueva versión"
        End If
		'destinatarios.Add(oUsuarios.UsuarioGet(info.GerenteProyectos).Email)
		For i As Integer = 0 To o.DestinatariosCreacionyEspecificacionesTrabajo(trabajo).Count - 1
            destinatarios.Add(o.DestinatariosCreacionyEspecificacionesTrabajo(trabajo).Item(i).CORREO)
        Next
        
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region

End Class