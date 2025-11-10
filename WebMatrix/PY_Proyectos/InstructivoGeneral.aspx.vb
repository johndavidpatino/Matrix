Imports CoreProject
Public Class InstructivoGeneral
	Inherits System.Web.UI.Page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		If Not IsPostBack Then
			If Session("TrabajoId") Is Nothing Then
				'Response.Redirect("Trabajos.aspx")
			Else
				hfTrabajoID.Value = Session("TrabajoId").ToString
				Dim o As New Proyecto

				If Session("duplicar") Is Nothing Then
					pVersion.InnerText = "Versión 1"
					hfversion.Value = 1
				ElseIf (Session("duplicar").ToString = "1") Then
					CargarInfo()
					cambiarCampos(True)
					pVersion.InnerText = "Versión " + Convert.ToString(o.ObtenerEspecifacionesContar(hfTrabajoID.Value))
					hfversion.Value = Convert.ToString(o.ObtenerEspecifacionesContar(hfTrabajoID.Value))
				ElseIf (Session("duplicar").ToString = "0") Then
					CargarInfo()
					cambiarCampos(False)
					pVersion.InnerText = "Versión " + Convert.ToString(o.ObtenerEspecifacionesContar(hfTrabajoID.Value) + 1)
					hfversion.Value = Convert.ToString(o.ObtenerEspecifacionesContar(hfTrabajoID.Value) + 1)
				End If
			End If
			txtEspecificacionesCampo.Focus()
		End If
	End Sub

	Sub cambiarCampos(ByVal bool As Boolean)
		If bool = False Then
			txtVC_DistribucionCuotas.Visible = True
			lblVC_DistribucionCuotas.Visible = False
			txtEstadistica.Visible = True
			lblEstadistica.Visible = False
			txtCritica.Visible = True
			lblCritica.Visible = False
			txtVerificacion.Visible = True
			lblVerificacion.Visible = False
			txtProcesamiento.Visible = True
			lblProcesamiento.Visible = False
			txtEspecificacionesCampo.Visible = True
			lblEspecificacionesCampo.Visible = False
			txtMaterialApoyo.Visible = True
			lblMaterialApoyo.Visible = False
			txtIncidencias.Visible = True
			lblIncidencias.Visible = False
			txtPilotos.Visible = True
			lblPilotos.Visible = False
			txtAuditoriaCampo.Visible = True
			lblAuditoriaCampo.Visible = False
			txtPilotosCalidad.Visible = True
			lblPilotosCalidad.Visible = False
			txtCodificacion.Visible = True
			lblCodificacion.Visible = False
			txtVCSeguridad.Visible = True
			lblVCSeguridad.Visible = False
			txtVCObtencion.Visible = True
			lblVCObtencion.Visible = False
			txtVCGrupoObjetivo.Visible = True
			lblVCGrupoObjetivo.Visible = False
			txtVCAplicacionInstrumentos.Visible = True
			lblVCAplicacionInstrumentos.Visible = False
			txtVCMetodologia.Visible = True
			lblVCMetodologia.Visible = False
			txtOtrasEspecificaciones.Visible = True
			lblOtrasEspecificaciones.Visible = False

			btnDuplicar.Visible = False
			btnGuardar.Visible = True
		Else
			txtVC_DistribucionCuotas.Visible = False
			lblVC_DistribucionCuotas.Visible = True
			txtEstadistica.Visible = False
			lblEstadistica.Visible = True
			txtCritica.Visible = False
			lblCritica.Visible = True
			txtVerificacion.Visible = False
			lblVerificacion.Visible = True
			txtProcesamiento.Visible = False
			lblProcesamiento.Visible = True
			txtEspecificacionesCampo.Visible = False
			lblEspecificacionesCampo.Visible = True
			txtMaterialApoyo.Visible = False
			lblMaterialApoyo.Visible = True
			txtIncidencias.Visible = False
			lblIncidencias.Visible = True
			txtPilotos.Visible = False
			lblPilotos.Visible = True
			txtAuditoriaCampo.Visible = False
			lblAuditoriaCampo.Visible = True
			txtPilotosCalidad.Visible = False
			lblPilotosCalidad.Visible = True
			txtCodificacion.Visible = False
			lblCodificacion.Visible = True
			txtVCSeguridad.Visible = False
			lblVCSeguridad.Visible = True
			txtVCObtencion.Visible = False
			lblVCObtencion.Visible = True
			txtVCGrupoObjetivo.Visible = False
			lblVCGrupoObjetivo.Visible = True
			txtVCAplicacionInstrumentos.Visible = False
			lblVCAplicacionInstrumentos.Visible = True
			txtVCMetodologia.Visible = False
			lblVCMetodologia.Visible = True
			txtOtrasEspecificaciones.Visible = False
			lblOtrasEspecificaciones.Visible = True

			btnDuplicar.Visible = True
			btnGuardar.Visible = False
		End If
	End Sub

	Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
		Response.Redirect("Trabajos.aspx")
	End Sub

	Private Sub btnDuplicar_Click(sender As Object, e As EventArgs) Handles btnDuplicar.Click
		Session("duplicar") = 0
		Response.Redirect("InstructivoGeneral.aspx")
	End Sub

	Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
		Dim flag As Boolean = False
		Dim o As New Proyecto
		Dim ent As New PY_EspecifTecTrabajo
		ent = o.ObtenerEspecifaciones(hfTrabajoID.Value)
		If ent.TrabajoId = 0 Then flag = True
		ent.TrabajoId = hfTrabajoID.Value
		ent.AuditoriaCampo = txtAuditoriaCampo.Content
		ent.Codificacion = txtCodificacion.Content
		ent.Critica = txtCritica.Content
		ent.EspecifacionesCampo = txtEspecificacionesCampo.Content
		ent.Estadistica = txtEstadistica.Content
		ent.Incidencias = txtIncidencias.Content
		ent.MaterialApoyo = txtMaterialApoyo.Content
		ent.OtrasEspecificaciones = txtOtrasEspecificaciones.Content
		ent.PilotosCalidad = txtPilotosCalidad.Content
		ent.PilotosCampo = txtPilotos.Content
		ent.Procesamiento = txtProcesamiento.Content
		ent.Verificacion = txtVerificacion.Content
		ent.VCSeguridad = txtVCSeguridad.Content
		ent.VCObtencion = txtVCObtencion.Content
		ent.VCGrupoObjetivo = txtVCGrupoObjetivo.Content
		ent.VCAplicacionInstrumentos = txtVCAplicacionInstrumentos.Content
		ent.VCDistribucionCuotas = txtVC_DistribucionCuotas.Content
		ent.VCMetodologia = txtVCMetodologia.Content
		ent.Usuario = Session("IDUsuario").ToString
		ent.Fecha = DateTime.Now.ToString
		Dim version = pVersion.InnerText
		ent.NoVersion = hfversion.Value

		o.GuardarInfoEspecificaciones(ent)
		EnviarEmail(flag)
		Response.Redirect("Trabajos.aspx")
	End Sub

	Sub CargarInfo()
		Dim o As New Proyecto
		Dim ent As New PY_EspecifTecTrabajo
		ent = o.ObtenerEspecifacionesLast(hfTrabajoID.Value)
		If ent.TrabajoId = hfTrabajoID.Value Then
			lblAuditoriaCampo.Text = ent.AuditoriaCampo
			txtAuditoriaCampo.Content = ent.AuditoriaCampo
			lblCodificacion.Text = ent.Codificacion
			txtCodificacion.Content = ent.Codificacion
			lblCritica.Text = ent.Critica
			txtCritica.Content = ent.Critica
			lblEspecificacionesCampo.Text = ent.EspecifacionesCampo
			txtEspecificacionesCampo.Content = ent.EspecifacionesCampo
			lblEstadistica.Text = ent.Estadistica
			txtEstadistica.Content = ent.Estadistica
			lblIncidencias.Text = ent.Incidencias
			txtIncidencias.Content = ent.Incidencias
			lblMaterialApoyo.Text = ent.MaterialApoyo
			txtMaterialApoyo.Content = ent.MaterialApoyo
			lblOtrasEspecificaciones.Text = ent.OtrasEspecificaciones
			txtOtrasEspecificaciones.Content = ent.OtrasEspecificaciones
			lblPilotosCalidad.Text = ent.PilotosCalidad
			txtPilotosCalidad.Content = ent.PilotosCalidad
			lblPilotos.Text = ent.PilotosCampo
			txtPilotos.Content = ent.PilotosCampo
			lblProcesamiento.Text = ent.Procesamiento
			txtProcesamiento.Content = ent.Procesamiento
			lblVerificacion.Text = ent.Verificacion
			txtVerificacion.Content = ent.Verificacion
			lblVCSeguridad.Text = ent.VCSeguridad
			txtVCSeguridad.Content = ent.VCSeguridad
			lblVCObtencion.Text = ent.VCObtencion
			txtVCObtencion.Content = ent.VCObtencion
			lblVCGrupoObjetivo.Text = ent.VCGrupoObjetivo
			txtVCGrupoObjetivo.Content = ent.VCGrupoObjetivo
			lblVCAplicacionInstrumentos.Text = ent.VCAplicacionInstrumentos
			txtVCAplicacionInstrumentos.Content = ent.VCAplicacionInstrumentos
			lblVC_DistribucionCuotas.Text = ent.VCDistribucionCuotas
			txtVC_DistribucionCuotas.Content = ent.VCDistribucionCuotas
			lblVCMetodologia.Text = ent.VCMetodologia
			txtVCMetodologia.Content = ent.VCMetodologia
			lblOtrasEspecificaciones.Text = ent.OtrasEspecificaciones
			txtOtrasEspecificaciones.Content = ent.OtrasEspecificaciones
		End If
	End Sub

	Sub EnviarEmail(ByVal Nuevo As Boolean)
		Dim oEnviarCorreo As New EnviarCorreo
		If String.IsNullOrEmpty(hfTrabajoID.Value) Then
			Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una entrega")
		End If
		oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EntregaTrabajoCuantitativo.aspx?idFicha=" & hfTrabajoID.Value & "&nuevo=" & Nuevo & "&version=" & hfversion.Value)
	End Sub
End Class