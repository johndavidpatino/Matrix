Imports CoreProject
Imports NoInkSoftware
Imports System.Globalization
Imports System.IO
Imports WebMatrix.Util

Public Class InformacionGeneral
	Inherits System.Web.UI.Page

	Enum ETarea
		Metodologia = 23
	End Enum

	Dim ORep As New Reportes.ReportesGenerales
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then

			Dim idPY As Int64? = Nothing, idTr As Int64? = Nothing
			If Request.QueryString("idPY") IsNot Nothing Then
				idPY = Request.QueryString("idPY").ToString
			Else
				idTr = Request.QueryString("idTr").ToString
			End If
			hfTrabajoId.Value = Convert.ToInt64(idTr.ToString)
			hfVolver.Value = Request.QueryString("URLBACK").ToString.Replace("|", "&")
			Dim ids = ORep.FormIDS(idPY, idTr)
			CargarInfoPropuesta(idPY, idTr)
			CargarEspecificacionesTecnicas(ids.TRABAJO)
			CargarEspecificacionesProyecto(ids.PROYECTO)
			CargarEsquemaAnalisis(ids.PROYECTO)
			CargarFrame(ids.BRIEF)
			CargarMetodologiaCampo(ids.TRABAJO)

			CargarVersionesMetodologias(idTr)
			CargarVersionesEspecificaciones(idTr)
			CargarProcesos(idTr)
			CargarNoProcesos(idTr)
			CargarProcesosEstadistica(idTr)
			pnlCotizacionOPS.Visible = False
			If Request.QueryString("URLBACK").ToString.ToLower.Contains("uantitativo") Then
				Dim permisos As New Datos.ClsPermisosUsuarios
				Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
				If permisos.VerificarRolUsuario(ListaRoles.COE, UsuarioID) = True Then
					pnlCotizacionOPS.Visible = True
					CargarDatosPresupuesto(idTr)
				End If
			End If

		End If
		Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
		smanager.RegisterPostBackControl(Me.btnExportarEspecificacionesWord)
	End Sub
	Sub CargarInfoPropuesta(idPY As Int64?, idTr As Int64?)
		'Dim o As New Reportes.ReportesGenerales
		Me.gvPropuestaInfoGeneral.DataSource = ORep.PropuestaInfoGeneral(idPY, idTr)
		Me.gvPropuestaPreguntas.DataSource = ORep.PropuestaInfoPreguntas(idPY, idTr)
		Me.gvPropuestaMuestra.DataSource = ORep.PropuestaInfoMuestra(idPY, idTr)
		gvPropuestaInfoGeneral.DataBind()
		gvPropuestaPreguntas.DataBind()
		gvPropuestaMuestra.DataBind()
	End Sub

	Sub CargarEspecificacionesProyecto(idProyecto As Int64)
		Dim o As New Proyecto
		Try
			If Session("TipoProyectoId") = 1 Then
				pnlCuanti.Visible = True
				pnlCuali.Visible = False
				Dim ent As New PY_EspCuentasCuanti
				ent = o.obtenerEspCuentasCuanti(idProyecto)
				txtBCPObservaciones.Text = ent.BCPObservaciones
				chbBCPMedicion.SelectedValue = ent.BCPMedicion
				txtBCPOlas.Text = ent.BCPOlas
				chbBCPPilotos.Checked = ent.BCPPilotos
				txtBCPPilotosEspecificaciones.Text = ent.BCPPilotosEspecificaciones
				txtBCPIncentivosEspecificaciones.Text = ent.BCPIncentivosEspecificaciones
				txtBCPBDDEspecificaciones.Text = ent.BCPBDDEspecificaciones
				txtBCPProductoEspecificaciones.Text = ent.BCPProductoEspecificaciones
				txtBCPFechaBDD.Text = If(ent.BCPFechaBDD Is Nothing, "", ent.BCPFechaBDD)
				txtBCPFechaConceptos.Text = If(ent.BCPFechaConceptos Is Nothing, "", ent.BCPFechaConceptos)
				txtBCPFechaCuestionario.Text = If(ent.BCPFechaCuestionario Is Nothing, "", ent.BCPFechaCuestionario)
				txtBCPFechaInicioCampo.Text = If(ent.BCPFechaInicioCampo Is Nothing, "", ent.BCPFechaInicioCampo)
				txtBCPFechaInformeCuentas.Text = If(ent.BCPFechaInformeCuentas Is Nothing, "", ent.BCPFechaInformeCuentas)
				txtBCPFechaInformeCliente.Text = If(ent.BCPFechaInformeCliente Is Nothing, "", ent.BCPFechaInformeCliente)
				lblVersionEspCuanti.InnerText = "Versión " + ent.NoVersion.ToString
			ElseIf Session("TipoProyectoId") = 2 Then

			End If
		Catch ex As Exception

		End Try
	End Sub

	Sub CargarEsquemaAnalisis(idProyecto As Int64)
		Dim oProyectos_Presupuestos As New Proyectos_Presupuestos
		Dim opI = oProyectos_Presupuestos.ObtenerProyecto(idProyecto)
		Try
			txtA1.Text = opI.A1
			txtA2.Text = opI.A2
			txtA3.Text = opI.A3
			txtA4.Text = opI.A4
			txtA5.Text = opI.A5
			txtA6.Text = opI.A6
			txtA7.Text = opI.A7
			txtA8.Text = opI.A8
		Catch ex As Exception

		End Try
	End Sub

	Public Sub CargarFrame(ByVal idbrief As Int64)
		Try
			Dim oBrief As New Brief
			Dim info = oBrief.ObtenerBriefXID(idbrief)
			txtO1.Text = info.O1
			txtO2.Text = info.O2
			txtO3.Text = info.O3
			txtO4.Text = info.O4
			txtO5.Text = info.O5
			txtO6.Text = info.O6
			txtO7.Text = info.O7
			txtD1.Text = info.D1
			txtD2.Text = info.D2
			txtD3.Text = info.D3
			txtC1.Text = info.C1
			txtC2.Text = info.C2
			txtC3.Text = info.C3
			txtC4.Text = info.C4
			txtC5.Text = info.C5
			txtM1.Text = info.M1
			txtM2.Text = info.M2
			txtM3.Text = info.M3
			txtDI1.Text = info.DI1
			txtDI2.Text = info.DI2
			txtDI3.Text = info.DI3
			txtDI4.Text = info.DI4
			txtDI5.Text = info.DI5
			txtDI6.Text = info.DI6
			txtDI7.Text = info.DI7
			txtDI8.Text = info.DI8
			txtDI9.Text = info.DI9
			txtDI10.Text = info.DI10
			txtDI11.Text = info.DI11
			txtDI12.Text = info.DI12
			txtDI13.Text = info.DI13
			txtDI14.Text = info.DI14
			txtDI15.Text = info.DI15
			txtDI16.Text = info.DI16
			txtDI17.Text = info.DI17
			txtDI18.Text = info.DI18
		Catch ex As Exception
			Throw ex
		End Try
	End Sub

	Sub CargarEspecificacionesTecnicas(idTr As Int64)
		Dim o As New Proyecto
		Dim ent As New PY_EspecifTecTrabajo
		Try
			ent = o.ObtenerEspecifacionesLast(idTr)
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

	Sub CargarVersionesEspecificaciones(ByVal idTr As Int64)
		Dim o As New Proyecto
		Dim ent As New List(Of PY_ObtenerVerEspecifTecTr_Result)

		Try
			ent = o.ObtenerVerEspecifTecTr(idTr)
			lblNumVersionEspecificacion.InnerText = "Versión No " + ent(0).NoVersion.ToString
			gvVersionesE.DataSource = ent
			gvVersionesE.DataBind()
		Catch ex As Exception

		End Try
	End Sub

	Sub CargarDatosPresupuesto(ByVal idTr As Int64)
		Dim oTrabajo As New Trabajo
		Dim infoT = oTrabajo.obtenerXId(idTr)

		Dim oCot As New CoreProject.Cotizador.General
		gvDetallesOperaciones.DataSource = oCot.GetCostos(infoT.IdPropuesta, infoT.Alternativa, infoT.MetCodigo, infoT.Fase, 2)
		gvDetallesOperaciones.DataBind()

		lblObservacionesPresupuesto.Text = oCot.GetPresupuesto(infoT.IdPropuesta, infoT.Alternativa, infoT.MetCodigo, infoT.Fase).ParObservaciones
	End Sub

	Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
		Response.Redirect(hfVolver.Value)
	End Sub

	Function CargarMetodologiaGPflag(IdTr As Int64, oWorkFlow As CORE_WorkFlow, versiones As List(Of ES_MetodologiaCampoTr_Result)) As Boolean
		Dim oTrabajo As New Trabajo
		Dim oMetodologia As New MetodologiaCampo
		Dim flag = False

		Dim FechaInicioAprobacion = Date.ParseExact("20190729", "yyyyMMdd", CultureInfo.InvariantCulture)
		If oWorkFlow.Estado = 5 And oWorkFlow.FFinR < FechaInicioAprobacion Then
			flag = True
			btnAprobarMetodologiaModal.Visible = False
		End If

		If versiones.Count > 0 Then
			Dim metodo = oMetodologia.ObtenerAprobacionMetXMetodologia(versiones.FirstOrDefault.id)
			If metodo.Count > 0 Then
				For Each m In metodo
					If m.Aprobado = 1 Then
						flag = True
						btnAprobarMetodologiaModal.Visible = False
					End If
				Next
			Else
				Dim coe = oTrabajo.DevolverxID(IdTr).COE
				If Session("IDUsuario") <> coe Then
					flag = True
				End If
			End If
		End If
		Return flag
	End Function
	Public Sub CargarMetodologiaCampo(ByVal IdTr As Int64)
		Try
			Dim daWorkFlow As New WorkFlow
			Dim oWorkFlow As CORE_WorkFlow = daWorkFlow.ObtenerWorkFlowXIdTrabajoXIdTarea(IdTr, ETarea.Metodologia)
			Dim oMetodologia As New MetodologiaCampo
			Dim oTrabajo As New Trabajo
			Dim versiones = oMetodologia.MetodologiaCampoTr(IdTr)

			Dim flag = CargarMetodologiaGPflag(IdTr, oWorkFlow, versiones)

			If flag = False Then
				btnAprobarMetodologiaModal.Visible = False
			Else
				btnAprobarMetodologiaModal.Visible = True
			End If

			If flag Then
				Dim idtrabajo As Int64 = Int64.Parse(IdTr)
				Dim listaMetodologia = (From lmetodo In oMetodologia.DevolverxIDTrabajo(idtrabajo)
										Select Id = lmetodo.id,
										Fecha = lmetodo.Fecha,
										Objetivo = lmetodo.ObjetivoT,
										Mercado = lmetodo.MercadoT,
										Marco = lmetodo.MarcoT,
										Tecnica = lmetodo.TecnicaT,
										Diseno = lmetodo.DisenoT,
										Instrucciones = lmetodo.InstruccionesT,
										Distribucion = lmetodo.DistribucionT,
										NivelConfianza = lmetodo.NivelConfianzaT,
										MargenError = lmetodo.MargenErrorT,
										Desagregacion = lmetodo.DesagregacionT,
										Fuente = lmetodo.FuenteT,
										Variables = lmetodo.VariablesT,
										Tasa = lmetodo.TasaT,
										Procedimiento = lmetodo.ProcedimientoT).OrderByDescending(Function(m) m.Id)

				If listaMetodologia.Count > 0 Then
					Dim idMetodo = listaMetodologia.FirstOrDefault.Id
					hfMetodologiaId.Value = Convert.ToInt64(idMetodo)
					Dim oMetodo As New MetodologiaCampo
					Dim info = oMetodo.DevolverxID(idMetodo)

					If info.Objetivo Then
						pnlobjetivos.Visible = True
						txtObjetivos.Text = info.ObjetivoT
					End If

					If info.Mercado Then
						pnlmercado.Visible = True
						txtMercado.Text = info.MercadoT
					End If

					If info.Marco Then
						pnlmarco.Visible = True
						txtMarcoMuestral.Text = info.MarcoT
					End If
					If info.Tecnica Then
						pnltecnica.Visible = True
						txtTecnica.Text = info.TecnicaT
					End If
					If info.Diseno Then
						pnldiseno.Visible = True
						txtDiseno.Text = info.DisenoT
					End If
					If info.Instrucciones Then
						pnlinstrucciones.Visible = True
						txtInstrucciones.Text = info.InstruccionesT
					End If
					If info.Distribucion Then
						pnldistribucion.Visible = True
						txtDistribucion.Text = info.DistribucionT
					End If
					If info.NivelConfianza Then
						pnlnivelconfianza.Visible = True
						txtNivelConfianza.Text = info.NivelConfianzaT
					End If
					If info.MargenError Then
						pnlmargenerror.Visible = True
						txtMargenError.Text = info.MargenErrorT
					End If
					If info.Desagregacion Then
						pnldesagregacion.Visible = True
						txtDesagregacion.Text = info.DesagregacionT
					End If
					If info.Fuente Then
						pnlfuente.Visible = True
						txtFuente.Text = info.FuenteT
					End If
					If info.Variables Then
						pnlVariables.Visible = True
						txtVariables.Text = info.VariablesT
					End If
					If info.Tasa Then
						pnltasa.Visible = True
						txtTasa.Text = info.TasaT
					End If
					If info.Procedimiento Then
						pnlprocedimiento.Visible = True
						txtprocedimiento.Text = info.ProcedimientoT
					End If

					'Dim oMetodologiaAp As New MetodologiaCampo
					'Dim Metodo = oMetodologiaAp.ObtenerAprobacionMetXMetodologia(idMetodo)
				Else
					btnAprobarMetodologiaModal.Visible = False
				End If
			End If
		Catch ex As Exception

		End Try
	End Sub

	Sub CargarVersionesMetodologias(ByVal idTr As Int64)
		Try

			Dim daWorkFlow As New WorkFlow
			Dim oWorkFlow As CORE_WorkFlow = daWorkFlow.ObtenerWorkFlowXIdTrabajoXIdTarea(idTr, ETarea.Metodologia)
			Dim oMetodologia As New MetodologiaCampo
			Dim oTrabajo As New Trabajo
			Dim versiones = oMetodologia.MetodologiaCampoTr(idTr)

			Dim flag = CargarMetodologiaGPflag(idTr, oWorkFlow, versiones)

			If flag Then
				lblNumVersionMetodologia.InnerText = "Versión No " + versiones.FirstOrDefault.NoVersion.ToString
				gvVersionesM.DataSource = versiones
				gvVersionesM.DataBind()
			End If
		Catch ex As Exception

		End Try

	End Sub

	Protected Sub btnExportarEspecificacionesWord_Click(sender As Object, e As EventArgs) Handles btnExportarEspecificacionesWord.Click
		Dim URL As String
		Dim RutaFisica As String
		Dim idTr = Request.QueryString("idTr").ToString
		Dim nombre As String = "Especificaciones_" + idTr.ToString + ".docx"
		URL = Path.Combine("~/FILES/Especificaciones/")
		RutaFisica = MapPath(URL)
		IO.Directory.CreateDirectory(RutaFisica)
		URL = Path.Combine(Server.MapPath(URL + "/" + nombre))
		pnlModalAprobar.Visible = False
		Dim data As String = ""
		data = "<html>
                    <head>
                        <meta http-equiv='Content-Type' content='text/html; charset=utf-8'/>
                    </head>
                    <body>
                        <h2>Metodología de Campo</h2>"

		Dim sb As StringBuilder = New StringBuilder()
		Dim tw As StringWriter = New StringWriter(sb)
		Dim hw As HtmlTextWriter = New HtmlTextWriter(tw)
		pnlMetodologia.RenderControl(hw)
		data += sb.ToString()
		data += "<h2>Especificaciones Técnicas del Trabajo</h2>"
		sb = New StringBuilder()
		tw = New StringWriter(sb)
		hw = New HtmlTextWriter(tw)
		pnlEspecificacionesTecnicas.RenderControl(hw)
		data += sb.ToString()
		data += "   </body>
                </html>"

		Dim nuevoArchivo As HTMLtoDOCX = New HTMLtoDOCX()
		pnlModalAprobar.Visible = True
		Try
			nuevoArchivo.CreateFileFromHTML(data, URL)
			Response.Redirect("~/FILES/Especificaciones/" + nombre)
		Catch ex As Exception
			If ex.Message.Contains("El proceso no puede obtener acceso al archivo") Then
				Console.Write("El archivo no se puede crear porque ya existe y no se puede reemplazar. Puede que esté abierto.")
			Else
				Console.Write("Se ha presentado un error: " + ex.Message)
			End If
		End Try

	End Sub

	Private Sub gvVersionesE_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVersionesE.RowCommand
		If e.CommandName = "Ver" Then
			pnlVersionesE.Visible = False
			pnlDetalleVerE.Visible = True
			pnlCompararE.Visible = False
			Dim id = gvVersionesE.DataKeys(CInt(e.CommandArgument))("id")
			Dim idTr = Request.QueryString("idTr").ToString
			Dim o As New Proyecto
			Dim ent As New List(Of PY_ObtenerEspecifXIdxTr_Result)

			ent = o.ObtenerEspecifXIdxTr(idTr, id)

			llenarDetalleVersionE(ent(0))
		ElseIf e.CommandName = "Comparar" Then
			pnlVersionesE.Visible = False
			pnlDetalleVerE.Visible = False
			pnlCompararE.Visible = True
			Dim id = gvVersionesE.DataKeys(CInt(e.CommandArgument))("id")
			Dim idTr = Request.QueryString("idTr").ToString
			Dim o As New Proyecto
			Dim ent As New PY_EspecifTecTrabajo
			Dim ent2 As New PY_EspecifTecTrabajo
			Dim Listent = New List(Of PY_EspecifTecTrabajo)
			Listent = o.ObtenerEspecifacionesId(id)
			ent = Listent(0)

			Dim numVersion = o.ObtenerEspecifacionesContar(idTr)
			Dim versionActual = gvVersionesE.DataKeys(CInt(e.CommandArgument))("NoVersion")
			If ent.NoVersion - 1 > 0 Then
				lblErrorVersionE.Text = ""
				Dim Listent2 = o.ObtenerEspecifacionesList(idTr)
				ent2 = Listent2(versionActual - 2)
				SubirDatosE(ent2, ent)
			Else
				lblErrorVersionE.Text = "No hay versiones anteriores"
			End If

		End If
	End Sub

	Private Sub volverListadoVersE_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVersE.ServerClick
		lblErrorVersionE.Text = ""
		pnlVersionesE.Visible = True
		pnlDetalleVerE.Visible = False
		pnlCompararE.Visible = False
		LimpiarDetalleVersionE()
	End Sub
	Private Sub volverListadoVersE2_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVersE2.ServerClick
		lblErrorVersionE.Text = ""
		pnlVersionesE.Visible = True
		pnlDetalleVerE.Visible = False
		pnlCompararE.Visible = False
		LimpiarDetalleVersionE()
	End Sub

	Sub llenarDetalleVersionE(ByVal ent As PY_ObtenerEspecifXIdxTr_Result)
		txtEspecificacionesCampoVer.Text = ent.EspecifacionesCampo
		txtMaterialApoyoVer.Text = ent.MaterialApoyo
		txtIncidenciasVer.Text = ent.Incidencias
		txtPilotoCampoVer.Text = ent.PilotosCampo
		txtAuditoriaVer.Text = ent.AuditoriaCampo
		txtVerificacionVer.Text = ent.Verificacion
		txtCriticaVer.Text = ent.Critica
		txtPilotoCampoCalidadVer.Text = ent.PilotosCalidad
		txtCodificacionVer.Text = ent.Codificacion
		txtProcesamientoVer.Text = ent.Procesamiento
		txtSeguridadVer.Text = ent.VCSeguridad
		txtObtencionEntrevistadosVer.Text = ent.VCObtencion
		txtGrupoObjetivoVer.Text = ent.VCGrupoObjetivo
		txtAplicacionInstrumentosVer.Text = ent.VCAplicacionInstrumentos
		txtDistribucionVer.Text = ent.VCDistribucionCuotas
		txtCumplimientoMetodologiaVer.Text = ent.VCMetodologia
		txtEstadisticaVer.Text = ent.Estadistica
		txtOtrasEspecificacionesVer.Text = ent.OtrasEspecificaciones
	End Sub

	Sub SubirDatosE(ByVal ent As PY_EspecifTecTrabajo, ByVal ent2 As PY_EspecifTecTrabajo)
		lblVersionA.Text = "Versión " + ent.NoVersion.ToString
		lblVersionB.Text = "Versión " + ent2.NoVersion.ToString

		If ent.EspecifacionesCampo <> ent2.EspecifacionesCampo Then
			txtEspCam1.CssClass = "cambioVersion lblScrolllbl"
			txtEspCam2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtEspCam1.CssClass = "versionIgual lblScrolllbl"
			txtEspCam2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtEspCam1.Text = ent.EspecifacionesCampo
		txtEspCam2.Text = ent2.EspecifacionesCampo

		If ent.MaterialApoyo <> ent2.MaterialApoyo Then
			txtMatApo1.CssClass = "cambioVersion lblScrolllbl"
			txtMatApo2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtMatApo1.CssClass = "versionIgual lblScrolllbl"
			txtMatApo2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtMatApo1.Text = ent.MaterialApoyo
		txtMatApo2.Text = ent2.MaterialApoyo

		If ent.Incidencias <> ent2.Incidencias Then
			txtInc1.CssClass = "cambioVersion lblScrolllbl"
			txtInc2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtInc1.CssClass = "versionIgual lblScrolllbl"
			txtInc2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtInc1.Text = ent.Incidencias
		txtInc2.Text = ent2.Incidencias

		If ent.PilotosCampo <> ent2.PilotosCampo Then
			txtpilCam1.CssClass = "cambioVersion lblScrolllbl"
			txtpilCam2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtpilCam1.CssClass = "versionIgual lblScrolllbl"
			txtpilCam2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtpilCam1.Text = ent.PilotosCampo
		txtpilCam2.Text = ent2.PilotosCampo

		If ent.AuditoriaCampo <> ent2.AuditoriaCampo Then
			txtAudCam1.CssClass = "cambioVersion lblScrolllbl"
			txtAudCam2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtAudCam1.CssClass = "versionIgual lblScrolllbl"
			txtAudCam2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtAudCam1.Text = ent.AuditoriaCampo
		txtAudCam2.Text = ent2.AuditoriaCampo

		If ent.Verificacion <> ent2.Verificacion Then
			txtVer1.CssClass = "cambioVersion lblScrolllbl"
			txtVer2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtVer1.CssClass = "versionIgual lblScrolllbl"
			txtVer2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtVer1.Text = ent.Verificacion
		txtVer2.Text = ent2.Verificacion

		If ent.Critica <> ent2.Critica Then
			txtcri1.CssClass = "cambioVersion lblScrolllbl"
			txtcri2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtcri1.CssClass = "versionIgual lblScrolllbl"
			txtcri2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtcri1.Text = ent.Critica
		txtcri2.Text = ent2.Critica

		If ent.PilotosCalidad <> ent2.PilotosCalidad Then
			txtpilCal1.CssClass = "cambioVersion lblScrolllbl"
			txtpilCal2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtpilCal1.CssClass = "versionIgual lblScrolllbl"
			txtpilCal2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtpilCal1.Text = ent.PilotosCalidad
		txtpilCal2.Text = ent2.PilotosCalidad

		If ent.Codificacion <> ent2.Codificacion Then
			txtCod1.CssClass = "cambioVersion lblScrolllbl"
			txtCod2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtCod1.CssClass = "versionIgual lblScrolllbl"
			txtCod2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtCod1.Text = ent.Codificacion
		txtCod2.Text = ent2.Codificacion

		If ent.Procesamiento <> ent2.Procesamiento Then
			txtPro1.CssClass = "cambioVersion lblScrolllbl"
			txtPro2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtPro1.CssClass = "versionIgual lblScrolllbl"
			txtPro2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtPro1.Text = ent.Procesamiento
		txtPro2.Text = ent2.Procesamiento

		If ent.VCSeguridad <> ent2.VCSeguridad Then
			txtSegcon1.CssClass = "cambioVersion lblScrolllbl"
			txtSegcon2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtSegcon1.CssClass = "versionIgual lblScrolllbl"
			txtSegcon2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtSegcon1.Text = ent.VCSeguridad
		txtSegcon2.Text = ent2.VCSeguridad

		If ent.VCObtencion <> ent2.VCObtencion Then
			txtObt1.CssClass = "cambioVersion lblScrolllbl"
			txtObt2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtObt1.CssClass = "versionIgual lblScrolllbl"
			txtObt2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtObt1.Text = ent.VCObtencion
		txtObt2.Text = ent2.VCObtencion

		If ent.VCGrupoObjetivo <> ent2.VCGrupoObjetivo Then
			txtgruObj1.CssClass = "cambioVersion lblScrolllbl"
			txtgruObj2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtgruObj1.CssClass = "versionIgual lblScrolllbl"
			txtgruObj2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtgruObj1.Text = ent.VCGrupoObjetivo
		txtgruObj2.Text = ent2.VCGrupoObjetivo

		If ent.VCAplicacionInstrumentos <> ent2.VCAplicacionInstrumentos Then
			txtInst1.CssClass = "cambioVersion lblScrolllbl"
			txtInst2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtInst1.CssClass = "versionIgual lblScrolllbl"
			txtInst2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtInst1.Text = ent.VCAplicacionInstrumentos
		txtInst2.Text = ent2.VCAplicacionInstrumentos

		If ent.VCDistribucionCuotas <> ent2.VCDistribucionCuotas Then
			txtDis1.CssClass = "cambioVersion lblScrolllbl"
			txtDis2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtDis1.CssClass = "versionIgual lblScrolllbl"
			txtDis2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtDis1.Text = ent.VCDistribucionCuotas
		txtDis2.Text = ent2.VCDistribucionCuotas

		If ent.VCMetodologia <> ent2.VCMetodologia Then
			txtCumMet1.CssClass = "cambioVersion lblScrolllbl"
			txtCumMet2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtCumMet1.CssClass = "versionIgual lblScrolllbl"
			txtCumMet2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtCumMet1.Text = ent.VCMetodologia
		txtCumMet2.Text = ent2.VCMetodologia

		If ent.Estadistica <> ent2.Estadistica Then
			txtEst1.CssClass = "cambioVersion lblScrolllbl"
			txtEst2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtEst1.CssClass = "versionIgual lblScrolllbl"
			txtEst2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtEst1.Text = ent.Estadistica
		txtEst2.Text = ent2.Estadistica

		If ent.OtrasEspecificaciones <> ent2.OtrasEspecificaciones Then
			txtOtrEsp1.CssClass = "cambioVersion lblScrolllbl"
			txtOtrEsp2.CssClass = "cambioVersion1 lblScrolllbl"
		Else
			txtOtrEsp1.CssClass = "versionIgual lblScrolllbl"
			txtOtrEsp2.CssClass = "versionIgual lblScrolllbl"
		End If
		txtOtrEsp1.Text = ent.EspecifacionesCampo
		txtOtrEsp2.Text = ent2.EspecifacionesCampo
	End Sub

	Sub SubirDatosM(ByVal ent As ES_MetodologiaCampo_Get_Result, ByVal ent2 As ES_MetodologiaCampo_Get_Result)
		lblVersionC.Text = "Versión " + ent.NoVersion.ToString
		lblVersionD.Text = "Versión " + ent2.NoVersion.ToString

		If ent.ObjetivoT <> ent2.ObjetivoT Then
			txtGruObjM1.CssClass = "cambioVersion"
			txtGruObjM2.CssClass = "cambioVersion1"
		Else
			txtGruObjM1.CssClass = "versionIgual"
			txtGruObjM2.CssClass = "versionIgual"
		End If
		txtGruObjM1.Text = ent.ObjetivoT
		txtGruObjM2.Text = ent2.ObjetivoT

		If ent.MercadoT <> ent2.MercadoT Then
			txtMerCub1.CssClass = "cambioVersion"
			txtMerCub2.CssClass = "cambioVersion1"
		Else
			txtMerCub1.CssClass = "versionIgual"
			txtMerCub2.CssClass = "versionIgual"
		End If
		txtMerCub1.Text = ent.MercadoT
		txtMerCub2.Text = ent2.MercadoT

		If ent.MarcoT <> ent2.MarcoT Then
			txtMarMues1.CssClass = "cambioVersion"
			txtMarMues2.CssClass = "cambioVersion1"
		Else
			txtMarMues1.CssClass = "versionIgual"
			txtMarMues2.CssClass = "versionIgual"
		End If
		txtMarMues1.Text = ent.MarcoT
		txtMarMues2.Text = ent2.MarcoT

		If ent.TecnicaT <> ent2.TecnicaT Then
			txtTec1.CssClass = "cambioVersion"
			txtTec2.CssClass = "cambioVersion1"
		Else
			txtTec1.CssClass = "versionIgual"
			txtTec2.CssClass = "versionIgual"
		End If
		txtTec1.Text = ent.TecnicaT
		txtTec2.Text = ent2.TecnicaT

		If ent.DisenoT <> ent2.DisenoT Then
			txtDisMues1.CssClass = "cambioVersion"
			txtDisMues2.CssClass = "cambioVersion1"
		Else
			txtDisMues1.CssClass = "versionIgual"
			txtDisMues2.CssClass = "versionIgual"
		End If
		txtDisMues1.Text = ent.DisenoT
		txtDisMues2.Text = ent2.DisenoT

		If ent.InstruccionesT <> ent2.InstruccionesT Then
			txtInstRec1.CssClass = "cambioVersion"
			txtInstRec2.CssClass = "cambioVersion1"
		Else
			txtInstRec1.CssClass = "versionIgual"
			txtInstRec2.CssClass = "versionIgual"
		End If
		txtInstRec1.Text = ent.InstruccionesT
		txtInstRec2.Text = ent2.InstruccionesT

		If ent.DistribucionT <> ent2.DistribucionT Then
			divDistriMues1.Attributes.Add("class", "cambioVersion lblScrolllbl")
			divDistriMues2.Attributes.Add("class", "cambioVersion1 lblScrolllbl")
		Else
			divDistriMues1.Attributes.Add("class", "versionIgual lblScrolllbl")
			divDistriMues2.Attributes.Add("class", "versionIgual lblScrolllbl")
		End If
		txtDistriMues1.Text = ent.DistribucionT
		txtDistriMues2.Text = ent2.DistribucionT


		If ent.NivelConfianzaT <> ent2.NivelConfianzaT Then
			txtNiv1.CssClass = "cambioVersion"
			txtNiv2.CssClass = "cambioVersion1"
		Else
			txtNiv1.CssClass = "versionIgual"
			txtNiv2.CssClass = "versionIgual"
		End If
		txtNiv1.Text = ent.NivelConfianzaT
		txtNiv2.Text = ent2.NivelConfianzaT

		If ent.MargenErrorT <> ent2.MargenErrorT Then
			txtMarErr1.CssClass = "cambioVersion"
			txtMarErr2.CssClass = "cambioVersion1"
		Else
			txtMarErr1.CssClass = "versionIgual"
			txtMarErr2.CssClass = "versionIgual"
		End If
		txtMarErr1.Text = ent.MargenErrorT
		txtMarErr2.Text = ent2.MargenErrorT

		If ent.DesagregacionT <> ent2.DesagregacionT Then
			txtDesBas1.CssClass = "cambioVersion"
			txtDesBas2.CssClass = "cambioVersion1"
		Else
			txtDesBas1.CssClass = "versionIgual"
			txtDesBas2.CssClass = "versionIgual"
		End If
		txtDesBas1.Text = ent.DesagregacionT
		txtDesBas2.Text = ent2.DesagregacionT

		If ent.FuenteT <> ent2.FuenteT Then
			txtFue1.CssClass = "cambioVersion"
			txtFue2.CssClass = "cambioVersion1"
		Else
			txtFue1.CssClass = "versionIgual"
			txtFue2.CssClass = "versionIgual"
		End If
		txtFue1.Text = ent.FuenteT
		txtFue2.Text = ent2.FuenteT

		If ent.VariablesT <> ent2.VariablesT Then
			txtVar1.CssClass = "cambioVersion"
			txtVar2.CssClass = "cambioVersion1"
		Else
			txtVar1.CssClass = "versionIgual"
			txtVar2.CssClass = "versionIgual"
		End If
		txtVar1.Text = ent.VariablesT
		txtVar2.Text = ent2.VariablesT

		If ent.TasaT <> ent2.TasaT Then
			txtTas1.CssClass = "cambioVersion"
			txtTas2.CssClass = "cambioVersion1"
		Else
			txtTas1.CssClass = "versionIgual"
			txtTas2.CssClass = "versionIgual"
		End If
		txtTas1.Text = ent.TasaT
		txtTas2.Text = ent2.TasaT

		If ent.ProcedimientoT <> ent2.ProcedimientoT Then
			txtProImp1.CssClass = "cambioVersion"
			txtProImp2.CssClass = "cambioVersion1"
		Else
			txtProImp1.CssClass = "versionIgual"
			txtProImp2.CssClass = "versionIgual"
		End If
		txtProImp1.Text = ent.ProcedimientoT
		txtProImp2.Text = ent2.ProcedimientoT


	End Sub

	Sub LimpiarDetalleVersionE()
		txtEspecificacionesCampoVer.Text = ""
		txtMaterialApoyoVer.Text = ""
		txtIncidenciasVer.Text = ""
		txtPilotoCampoVer.Text = ""
		txtAuditoriaVer.Text = ""
		txtVerificacionVer.Text = ""
		txtCriticaVer.Text = ""
		txtPilotoCampoCalidadVer.Text = ""
		txtCodificacionVer.Text = ""
		txtProcesamientoVer.Text = ""
		txtSeguridadVer.Text = ""
		txtObtencionEntrevistadosVer.Text = ""
		txtGrupoObjetivoVer.Text = ""
		txtAplicacionInstrumentosVer.Text = ""
		txtDistribucionVer.Text = ""
		txtCumplimientoMetodologiaVer.Text = ""
		txtEstadisticaVer.Text = ""
		txtOtrasEspecificacionesVer.Text = ""
	End Sub

	Private Sub gvVersionesM_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVersionesM.RowCommand
		If e.CommandName = "Ver" Then
			pnlVersionesM.Visible = False
			pnlDetalleVerM.Visible = True
			Dim id = gvVersionesM.DataKeys(CInt(e.CommandArgument))("id")
			Dim idTr = Request.QueryString("idTr").ToString
			Dim o As New MetodologiaCampo
			Dim ent As New List(Of ES_ObtenerMetodologiaXIdxTr_Result)

			ent = o.ObtenerMetodologiaXIdxTr(idTr, id)
			llenarDetalleVersionM(ent(0))
		ElseIf e.CommandName = "Comparar" Then
			pnlVersionesM.Visible = False
			pnlDetalleVerM.Visible = False
			pnlCompararM.Visible = True
			Dim id = gvVersionesM.DataKeys(CInt(e.CommandArgument))("id")
			Dim idTr = Request.QueryString("idTr").ToString
			Dim o As New MetodologiaCampo
			Dim ent As New ES_MetodologiaCampo_Get_Result
			Dim ent2 As New ES_MetodologiaCampo_Get_Result
			ent = o.DevolverxID(id)

			Dim numVersion = o.ObtenerMetodologiaNumVersionesxTr(idTr)
			Dim versionActual = gvVersionesM.DataKeys(CInt(e.CommandArgument))("NoVersion")
			If ent.NoVersion - 1 > 0 Then
				lblErrorVersionM.Text = ""
				Dim Listent = o.ES_MetodologiaCampoXTr_Get(idTr)
				ent2 = Listent(versionActual - 2)
				SubirDatosM(ent2, ent)
			Else
				lblErrorVersionM.Text = "No hay versiones anteriores"
			End If


		End If
	End Sub

	Private Sub volverListadoVersM_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVersM.ServerClick
		lblErrorVersionM.Text = ""
		pnlVersionesM.Visible = True
		pnlDetalleVerM.Visible = False
		pnlCompararM.Visible = False
		LimpiarDetalleVersionM()
	End Sub

	Private Sub volverListadoVersM2_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVersM2.ServerClick
		lblErrorVersionM.Text = ""
		pnlVersionesM.Visible = True
		pnlDetalleVerM.Visible = False
		pnlCompararM.Visible = False
		LimpiarDetalleVersionM()
	End Sub

	Sub llenarDetalleVersionM(ByVal ent As ES_ObtenerMetodologiaXIdxTr_Result)
		txtGrupoObjetivoMVer.Text = ent.ObjetivoT
		txtMercadoCubrimientoVer.Text = ent.MercadoT
		txtMarcoMuestralVer.Text = ent.MarcoT
		txtTecnicaVer.Text = ent.TecnicaT
		txtDisenoMuestralVer.Text = ent.DisenoT
		txtInstruccionesRecoleccionVer.Text = ent.InstruccionesT
		txtDistribucionMuestraMVer.Text = ent.DistribucionT
		txtNivelConfianzaVer.Text = ent.NivelConfianzaT
		txtMargenErrorEsperadoVer.Text = ent.MargenErrorT
		txtDesagregacionVer.Text = ent.DesagregacionT
		txtFuenteDistribucionVer.Text = ent.FuenteT
		txtVariablePonderacionVer.Text = ent.VariablesT
		txtTasaRespuesta.Text = ent.TasaT
		txtProcedimientoImputacion.Text = ent.ProcedimientoT

		txtGrupoObjetivoMVer.ReadOnly = True
		txtMercadoCubrimientoVer.ReadOnly = True
		txtMarcoMuestralVer.ReadOnly = True
		txtTecnicaVer.ReadOnly = True
		txtDisenoMuestralVer.ReadOnly = True
		txtNivelConfianzaVer.ReadOnly = True
		txtMargenErrorEsperadoVer.ReadOnly = True
		txtDesagregacionVer.ReadOnly = True
		txtFuenteDistribucionVer.ReadOnly = True
		txtVariablePonderacionVer.ReadOnly = True
		txtTasaRespuesta.ReadOnly = True
		txtProcedimientoImputacion.ReadOnly = True
	End Sub

	Sub LimpiarDetalleVersionM()
		txtGrupoObjetivoMVer.Text = ""
		txtMercadoCubrimientoVer.Text = ""
		txtMarcoMuestralVer.Text = ""
		txtTecnicaVer.Text = ""
		txtDisenoMuestralVer.Text = ""
		txtInstruccionesRecoleccionVer.Text = ""
		txtDistribucionMuestraMVer.Text = ""
		txtNivelConfianzaVer.Text = ""
		txtMargenErrorEsperadoVer.Text = ""
		txtDesagregacionVer.Text = ""
		txtFuenteDistribucionVer.Text = ""
		txtVariablePonderacionVer.Text = ""
		txtTasaRespuesta.Text = ""
		txtProcedimientoImputacion.Text = ""

		txtGrupoObjetivoMVer.ReadOnly = False
		txtMercadoCubrimientoVer.ReadOnly = False
		txtMarcoMuestralVer.ReadOnly = False
		txtTecnicaVer.ReadOnly = False
		txtDisenoMuestralVer.ReadOnly = False
		txtNivelConfianzaVer.ReadOnly = False
		txtMargenErrorEsperadoVer.ReadOnly = False
		txtDesagregacionVer.ReadOnly = False
		txtFuenteDistribucionVer.ReadOnly = False
		txtVariablePonderacionVer.ReadOnly = False
		txtTasaRespuesta.ReadOnly = False
		txtProcedimientoImputacion.ReadOnly = False

	End Sub

	Private Sub gvVersionesE_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvVersionesE.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			If Me.gvVersionesE.DataKeys(e.Row.RowIndex)("NoVersion") = "1" Then
				e.Row.Cells(5).Visible = False
				e.Row.Cells(4).ColumnSpan = 2
			End If
		End If
	End Sub

	Private Sub gvVersionesM_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvVersionesM.RowDataBound
		If e.Row.RowType = DataControlRowType.DataRow Then
			If Me.gvVersionesM.DataKeys(e.Row.RowIndex)("NoVersion") = "1" Then
				e.Row.Cells(5).Visible = False
				e.Row.Cells(4).ColumnSpan = 2
			End If
		End If
	End Sub

	Private Sub btnAprobarMetodologia_Click(sender As Object, e As EventArgs) Handles btnAprobarMetodologia.Click
		Dim oMetodologia As New MetodologiaCampo

		Try
			'Dim Metodo = oMetodologia.ObtenerAprobacionMetXMetodologia(hfMetodologiaId.Value)
			Dim AprobacionMetodologia As New ES_AprobacionMetodologia()
			AprobacionMetodologia.MetodologiaId = hfMetodologiaId.Value
			AprobacionMetodologia.TrabajoId = hfTrabajoId.Value
			AprobacionMetodologia.Observación = txtObsAprobarMetodologia.Text
			AprobacionMetodologia.Aprobado = 1
			AprobacionMetodologia.Usuario = Session("IDUsuario")
			AprobacionMetodologia.Fecha = Date.UtcNow.AddHours(-5)

			Dim aprobado = oMetodologia.GuardarAprobacionMetodologia(AprobacionMetodologia)
			If aprobado > 0 Then
				'ShowNotification("Metodología Aprobada", ShowNotifications.InfoNotification)
				ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cerrarAprobarMetodologia", "<script>cerrarAprobarMetodologia()</script>", False)
				btnAprobarMetodologiaModal.Visible = False
			End If
		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Sub

	Private Sub btnRechazarMetodologia_Click(sender As Object, e As EventArgs) Handles btnRechazarMetodologia.Click
		Dim oMetodologia As New MetodologiaCampo
		If txtObsAprobarMetodologia.Text.Count() <= 0 Then
			ShowNotification("Debe indicar por qué está rechazando la metodología.", ShowNotifications.InfoNotification)
			Exit Sub
		End If

		Try
			Dim AprobacionMetodologia As New ES_AprobacionMetodologia()
			AprobacionMetodologia.MetodologiaId = hfMetodologiaId.Value
			AprobacionMetodologia.TrabajoId = hfTrabajoId.Value
			AprobacionMetodologia.Observación = txtObsAprobarMetodologia.Text
			AprobacionMetodologia.Aprobado = 0
			AprobacionMetodologia.Usuario = Session("IDUsuario")
			AprobacionMetodologia.Fecha = Date.UtcNow.AddHours(-5)

			Dim aprobado = oMetodologia.GuardarAprobacionMetodologia(AprobacionMetodologia)
			If aprobado > 0 Then
				ScriptManager.RegisterStartupScript(Page, Me.GetType(), "cerrarAprobarMetodologia", "<script>cerrarAprobarMetodologia()</script>", False)
				btnAprobarMetodologiaModal.Visible = False
				EnviarEmailNoAprobacionMetodologia(AprobacionMetodologia.MetodologiaId, hfTrabajoId.Value)
			End If
		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Sub


	Sub EnviarEmailNoAprobacionMetodologia(ByVal IdAprobacionMetodologia As Int64, ByVal idtrabajo As Int64)
		Dim oEnviarCorreo As New EnviarCorreo
		Try
			If String.IsNullOrEmpty(IdAprobacionMetodologia) Then
				Throw New Exception("No se puede rechazar sin metodología")
			End If
			Dim script As String = ""
			oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/RechazarMetodologia.aspx?IdAprobacionMetodologia=" & IdAprobacionMetodologia & "&idtrabajo=" & idtrabajo)
			ShowNotification("Notificación enviada correctamente", ShowNotifications.InfoNotification)
			ActivateAccordion(0, EffectActivateAccordion.NoEffect)
		Catch ex As Exception
			ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
			ActivateAccordion(0, EffectActivateAccordion.NoEffect)
		End Try
	End Sub

	Sub CargarProcesos(idTr As Int64?)
		Dim oData As New Cotizador.GeneralDapper
		Dim dataGet = oData.GetProcesos(idTr)
		Dim listTxt As String = "<p>"
		Dim ParNacional As String = ""
		For Each item In dataGet
			If Not (item.DescFase = ParNacional) Then
				ParNacional = item.DescFase
				listTxt += "<br/> <b>" + item.DescFase + "</b>: "
				listTxt += item.ProcDescripcion + " | "
			Else
				listTxt += item.ProcDescripcion + " | "
			End If
		Next
		listTxt += "</p>"
		literalProcesos.Text = listTxt.Remove(3, 5)
	End Sub

	Sub CargarNoProcesos(idTr As Int64?)
		Dim oData As New Cotizador.GeneralDapper
		Dim dataGet = oData.GetNoProcesos(idTr)
		gvNoProcesos.DataSource = dataGet
		gvNoProcesos.DataBind()
	End Sub

	Sub CargarProcesosEstadistica(idTr As Int64?)
		Dim oData As New Cotizador.GeneralDapper
		Dim dataGet = oData.GetProcesosEstadistica(idTr)
		gvProcesosEstadistica.DataSource = dataGet
		gvProcesosEstadistica.DataBind()
	End Sub
End Class


