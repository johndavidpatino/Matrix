Imports CoreProject
Imports NoInkSoftware
Imports System.IO

Public Class InformacionGeneralCuali
    Inherits System.Web.UI.Page

    Dim ORep As New Reportes.ReportesGenerales
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Dim idPY As Int64? = Nothing, idTr As Int64? = Nothing
            If Request.QueryString("idPY") IsNot Nothing Then
                idPY = Request.QueryString("idPY").ToString
            Else
                idTr = Request.QueryString("idTr").ToString
            End If
            hfVolver.Value = Request.QueryString("URLBACK").ToString.Replace("|", "&")
            Dim ids = ORep.FormIDS(idPY, idTr)
            hfTrabajoID.Value = idTr
            hfProyectoID.Value = ids.PROYECTO
            CargarInfoPropuesta(idPY, idTr)
            CargarEspecificacionesTecnicas(ids.TRABAJO)
            CargarEspecificacionesProyecto(ids.PROYECTO)
            CargarFrame(ids.BRIEF)
            CargarMetodologiaCampo(ids.TRABAJO)

            CargarVersionesEspecificacionesCuentas(hfProyectoID.Value)
            CargarVersionesEspecificacionesTrabajo(hfTrabajoID.Value)
            CargarEsquemaAnalisis(hfProyectoID.Value)
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
            Dim ent As New PY_EspCuentasCuali
            ent = o.obtenerEspCuentasCuali(idProyecto)

            If Not IsNothing(ent.NoVersion) Then
                txtObservacionesCuali.Text = ent.BCPObservaciones
                chbBCPTecnicaCuali.SelectedValue = ent.BCPTecnica
                otraTecnica.Text = ent.BCPotraTecnica
                txtBCPIncentivosEspCuali.Text = ent.BCPIncentivosEsp
                txtBCPBDDEspCuali.Text = ent.BCPBDDEsp
                txtBCPProductoEspCuali.Text = ent.BCPProductoEsp
                chbBCPReclutamientoCuali.SelectedValue = ent.BCPReclutamiento
                txtBCPEspReclutamientoCuali.Text = ent.BCPEspReclutamiento
                chbBCPEspProductoCuali.Checked = ent.BCPEspProducto
                chbBCPMaterialEvalCuali.Checked = ent.BCPMaterialEval
                txtBCPObsProductoCuali.Text = ent.BCPObsProducto

                lblVersionEspC.InnerText = "Versión " + ent.NoVersion.ToString
            Else
                lblVersionEspC.InnerText = ""
            End If

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
    Sub CargarVersionesEspecificacionesTrabajo(ByVal idTr As Int64)
        Dim o As New CoreProject.SegmentosCuali
        Dim ent As New List(Of PY_EspecifTecTrabajoCuali)
        Try
            ent = o.ObtenerEspecifacionesCualiList(idTr)
            If ent.Count > 0 Then
                lblNumVersionEspecificacion.InnerText = "Versión No " + ent(0).NoVersion.ToString
            End If
            gvVersionesET.DataSource = ent
            gvVersionesET.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Sub CargarEspecificacionesTecnicas(idTr As Int64)
        Dim o As New CoreProject.SegmentosCuali
        Dim ent As New PY_EspecifTecTrabajoCuali
        Try
            ent = o.ObtenerEspecifacionesCualiLast(idTr)
            txtAuditoriaCampo.Text = ent.Auditoria
            txtModerador.Text = ent.Moderador
            lblEspecificacionesCampo.Text = ent.EspecificacionesCampo
            txtMaterialApoyo.Text = ent.MaterialApoyo
            txtIncidencias.Text = ent.Incidencias
            txtVCSeguridad.Text = ent.VCSeguridad
            txtVCObtencion.Text = ent.VCObtencion
            lblVCGrupoObjetivo.Text = ent.VCGrupoObjetivo
            txtVCAplicacionInstrumentos.Text = ent.VCAplicacionInstrumentos
            lblVCDistribucionCuotas.Text = ent.VCDistribucionCuotas
            txtVCMetodologia.Text = ent.VCMetodologia
            rblIncentivos.SelectedValue = ent.Incentivos
            txtPresupuestoIncentivo.Text = ent.PresupuestoIncentivo
            txtDistribucionIncentivo.Text = ent.DistribucionIncentivo
            rblRegaloClientes.SelectedValue = ent.RegaloClientes
            rblCompraIpsos.SelectedValue = ent.CompraIpsos
            txtPresupuestoCompra.Text = ent.PresupuestoCompra
            txtDistribucionCompra.Text = ent.DistribucionCompra
            txtExclusionesyRestricciones.Text = ent.ExclusionesyRestricciones
            txtRecursosPropiedadesCliente.Text = ent.RecursosPropiedadesCliente
            txtHabeasData.Text = ent.HabeasData
            lblOtrasEspecificaciones.Text = ent.OtrasEspecificaciones

            cargarAyudas()
            cargarReclutamientos()
            ObtenerAyudas()
            ObtenerTipoReclutamiento()
        Catch ex As Exception

        End Try
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
    Private Sub btnRegresar_Click(sender As Object, e As EventArgs) Handles btnRegresar.Click
        Response.Redirect(hfVolver.Value)
    End Sub

    Public Sub CargarMetodologiaCampo(ByVal IdTr As Int64)

        Try
            Dim idtrabajo As Int64 = Int64.Parse(IdTr)
            Dim oMetodologia As New MetodologiaCampo
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
            Dim idMetodo = listaMetodologia(0).Id
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

    Sub SubirDatosEC(ByVal ent As PY_EspCuentasCuali, ByVal ent2 As PY_EspCuentasCuali)
        lblVersionA.Text = "Versión " + ent.NoVersion.ToString
        lblVersionB.Text = "Versión " + ent2.NoVersion.ToString

        If ent.BCPObservaciones <> ent2.BCPObservaciones Then
            txtBCPObservacionesCam1.CssClass = "cambioVersion"
            txtBCPObservacionesCam2.CssClass = "cambioVersion1"
        Else
            txtBCPObservacionesCam1.CssClass = "versionIgual"
            txtBCPObservacionesCam2.CssClass = "versionIgual"
        End If
        txtBCPObservacionesCam1.Text = ent.BCPObservaciones
        txtBCPObservacionesCam2.Text = ent2.BCPObservaciones

        If ent.BCPTecnica <> ent2.BCPTecnica Then
            chbBCPTecnicaV1.CssClass = "cambioVersion"
            chbBCPTecnicaV2.CssClass = "cambioVersion1"
        Else
            chbBCPTecnicaV1.CssClass = "versionIgual"
            chbBCPTecnicaV2.CssClass = "versionIgual"
        End If

        Select Case ent.BCPTecnica
            Case 1
                chbBCPTecnicaV1.Text = "Entrevista"
            Case 2
                chbBCPTecnicaV1.Text = "Sesiones de grupo/Talleres"
            Case 3
                chbBCPTecnicaV1.Text = "Inmersiones"
            Case 4
                chbBCPTecnicaV1.Text = "Estudios online"
            Case 5
                chbBCPTecnicaV1.Text = "Otro"
            Case Else
                chbBCPTecnicaV1.Text = ""
        End Select

        Select Case ent2.BCPTecnica
            Case 1
                chbBCPTecnicaV2.Text = "Entrevista"
            Case 2
                chbBCPTecnicaV2.Text = "Sesiones de grupo/Talleres"
            Case 3
                chbBCPTecnicaV2.Text = "Inmersiones"
            Case 4
                chbBCPTecnicaV2.Text = "Estudios online"
            Case 5
                chbBCPTecnicaV2.Text = "Otro"
            Case Else
                chbBCPTecnicaV2.Text = ""
        End Select

        If ent.BCPotraTecnica <> ent2.BCPotraTecnica Then
            otraTecnicaV1.CssClass = "cambioVersion"
            otraTecnicaV2.CssClass = "cambioVersion1"
        Else
            otraTecnicaV1.CssClass = "versionIgual"
            otraTecnicaV2.CssClass = "versionIgual"
        End If
        otraTecnicaV1.Text = ent.BCPotraTecnica
        otraTecnicaV2.Text = ent2.BCPotraTecnica

        If ent.BCPIncentivosEsp <> ent2.BCPIncentivosEsp Then
            txtBCPIncentivosEspCam1.CssClass = "cambioVersion"
            txtBCPIncentivosEspCam2.CssClass = "cambioVersion1"
        Else
            txtBCPIncentivosEspCam1.CssClass = "versionIgual"
            txtBCPIncentivosEspCam2.CssClass = "versionIgual"
        End If
        txtBCPIncentivosEspCam1.Text = ent.BCPIncentivosEsp
        txtBCPIncentivosEspCam2.Text = ent2.BCPIncentivosEsp

        If ent.BCPBDDEsp <> ent2.BCPBDDEsp Then
            txtBCPBDDEspCam1.CssClass = "cambioVersion"
            txtBCPBDDEspCam2.CssClass = "cambioVersion1"
        Else
            txtBCPBDDEspCam1.CssClass = "versionIgual"
            txtBCPBDDEspCam2.CssClass = "versionIgual"
        End If
        txtBCPBDDEspCam1.Text = ent.BCPBDDEsp
        txtBCPBDDEspCam2.Text = ent2.BCPBDDEsp

        If ent.BCPProductoEsp <> ent2.BCPProductoEsp Then
            txtBCPProductoEspCam1.CssClass = "cambioVersion"
            txtBCPProductoEspCam2.CssClass = "cambioVersion1"
        Else
            txtBCPProductoEspCam1.CssClass = "versionIgual"
            txtBCPProductoEspCam2.CssClass = "versionIgual"
        End If
        txtBCPProductoEspCam1.Text = ent.BCPProductoEsp
        txtBCPProductoEspCam2.Text = ent2.BCPProductoEsp

        If ent.BCPReclutamiento <> ent2.BCPReclutamiento Then
            chbBCPReclutamientoV1.CssClass = "cambioVersion"
            chbBCPReclutamientoV2.CssClass = "cambioVersion1"
        Else
            chbBCPReclutamientoV1.CssClass = "versionIgual"
            chbBCPReclutamientoV2.CssClass = "versionIgual"
        End If

        Select Case ent.BCPReclutamiento
            Case 1
                chbBCPReclutamientoV1.Text = "Base de datos"
            Case 2
                chbBCPReclutamientoV1.Text = "Convencional"
            Case 3
                chbBCPReclutamientoV1.Text = "Referidos"
            Case 4
                chbBCPReclutamientoV1.Text = "En frío"
            Case Else
                chbBCPReclutamientoV1.Text = ""
        End Select

        Select Case ent2.BCPReclutamiento
            Case 1
                chbBCPReclutamientoV2.Text = "Base de datos"
            Case 2
                chbBCPReclutamientoV2.Text = "Convencional"
            Case 3
                chbBCPReclutamientoV2.Text = "Referidos"
            Case 4
                chbBCPReclutamientoV2.Text = "En frío"
            Case Else
                chbBCPReclutamientoV2.Text = ""
        End Select

        If ent.BCPEspReclutamiento <> ent2.BCPEspReclutamiento Then
            txtBCPEspReclutamientoCam1.CssClass = "cambioVersion"
            txtBCPEspReclutamientoCam2.CssClass = "cambioVersion1"
        Else
            txtBCPEspReclutamientoCam1.CssClass = "versionIgual"
            txtBCPEspReclutamientoCam2.CssClass = "versionIgual"
        End If
        txtBCPEspReclutamientoCam1.Text = ent.BCPEspReclutamiento
        txtBCPEspReclutamientoCam2.Text = ent2.BCPEspReclutamiento

        If ent.BCPEspProducto <> ent2.BCPEspProducto Then
            lblBCPEspProductoCam1.CssClass = "cambioVersion"
            lblBCPEspProductoCam2.CssClass = "cambioVersion1"
        Else
            lblBCPEspProductoCam1.CssClass = "versionIgual"
            lblBCPEspProductoCam2.CssClass = "versionIgual"
        End If
        If ent.BCPEspProducto Then lblBCPEspProductoCam1.Text = "Sí" Else lblBCPEspProductoCam1.Text = "No"
        If ent2.BCPEspProducto Then lblBCPEspProductoCam2.Text = "Sí" Else lblBCPEspProductoCam2.Text = "No"

        If ent.BCPMaterialEval <> ent2.BCPMaterialEval Then
            lblBCPMaterialEvalCam1.CssClass = "cambioVersion"
            lblBCPMaterialEvalCam2.CssClass = "cambioVersion1"
        Else
            lblBCPMaterialEvalCam1.CssClass = "versionIgual"
            lblBCPMaterialEvalCam2.CssClass = "versionIgual"
        End If
        If ent.BCPMaterialEval Then lblBCPMaterialEvalCam1.Text = "Sí" Else lblBCPMaterialEvalCam1.Text = "No"
        If ent2.BCPMaterialEval Then lblBCPMaterialEvalCam2.Text = "Sí" Else lblBCPMaterialEvalCam2.Text = "No"

        If ent.BCPObsProducto <> ent2.BCPObsProducto Then
            txtBCPObsProductoCam1.CssClass = "cambioVersion"
            txtBCPObsProductoCam2.CssClass = "cambioVersion1"
        Else
            txtBCPObsProductoCam1.CssClass = "versionIgual"
            txtBCPObsProductoCam2.CssClass = "versionIgual"
        End If
        txtBCPObsProductoCam1.Text = ent.BCPObsProducto
        txtBCPObsProductoCam2.Text = ent2.BCPObsProducto

    End Sub

    Sub SubirDatosET(ByVal ent As PY_EspecifTecTrabajoCuali, ByVal ent2 As PY_EspecifTecTrabajoCuali)
        lblVersionC.Text = "Versión " + ent.NoVersion.ToString
        lblVersionD.Text = "Versión " + ent2.NoVersion.ToString

        If ent.Moderador <> ent2.Moderador Then
            txtModerador1.CssClass = "cambioVersion"
            txtModerador2.CssClass = "cambioVersion1"
        Else
            txtModerador1.CssClass = "versionIgual"
            txtModerador2.CssClass = "versionIgual"
        End If
        txtModerador1.Text = ent.Moderador
        txtModerador2.Text = ent2.Moderador

        If ent.EspecificacionesCampo <> ent2.EspecificacionesCampo Then
            lblEspecificacionesCampo1.CssClass = "cambioVersion"
            lblEspecificacionesCampo2.CssClass = "cambioVersion1"
        Else
            lblEspecificacionesCampo1.CssClass = "versionIgual"
            lblEspecificacionesCampo2.CssClass = "versionIgual"
        End If
        lblEspecificacionesCampo1.Text = ent.EspecificacionesCampo
        lblEspecificacionesCampo2.Text = ent2.EspecificacionesCampo

        If ent.MaterialApoyo <> ent2.MaterialApoyo Then
            txtMaterialApoyo1.CssClass = "cambioVersion"
            txtMaterialApoyo2.CssClass = "cambioVersion1"
        Else
            txtMaterialApoyo1.CssClass = "versionIgual"
            txtMaterialApoyo2.CssClass = "versionIgual"
        End If
        txtMaterialApoyo1.Text = ent.MaterialApoyo
        txtMaterialApoyo2.Text = ent2.MaterialApoyo

        If ent.Incidencias <> ent2.Incidencias Then
            txtIncidencias1.CssClass = "cambioVersion"
            txtIncidencias2.CssClass = "cambioVersion1"
        Else
            txtIncidencias1.CssClass = "versionIgual"
            txtIncidencias2.CssClass = "versionIgual"
        End If
        txtIncidencias1.Text = ent.Incidencias
        txtIncidencias2.Text = ent2.Incidencias

        If ent.Auditoria <> ent2.Auditoria Then
            txtAuditoriaCampo1.CssClass = "cambioVersion"
            txtAuditoriaCampo2.CssClass = "cambioVersion1"
        Else
            txtAuditoriaCampo1.CssClass = "versionIgual"
            txtAuditoriaCampo2.CssClass = "versionIgual"
        End If
        txtAuditoriaCampo1.Text = ent.Auditoria
        txtAuditoriaCampo2.Text = ent2.Auditoria

        If ent.VCSeguridad <> ent2.VCSeguridad Then
            txtVCSeguridad1.CssClass = "cambioVersion"
            txtVCSeguridad2.CssClass = "cambioVersion1"
        Else
            txtVCSeguridad1.CssClass = "versionIgual"
            txtVCSeguridad2.CssClass = "versionIgual"
        End If
        txtVCSeguridad1.Text = ent.VCSeguridad
        txtVCSeguridad2.Text = ent2.VCSeguridad

        If ent.VCObtencion <> ent2.VCObtencion Then
            txtVCObtencion1.CssClass = "cambioVersion"
            txtVCObtencion2.CssClass = "cambioVersion1"
        Else
            txtVCObtencion1.CssClass = "versionIgual"
            txtVCObtencion2.CssClass = "versionIgual"
        End If
        txtVCObtencion1.Text = ent.VCObtencion
        txtVCObtencion2.Text = ent2.VCObtencion

        If ent.VCGrupoObjetivo <> ent2.VCGrupoObjetivo Then
            lblVCGrupoObjetivo1.CssClass = "cambioVersion"
            lblVCGrupoObjetivo2.CssClass = "cambioVersion1"
        Else
            lblVCGrupoObjetivo1.CssClass = "versionIgual"
            lblVCGrupoObjetivo2.CssClass = "versionIgual"
        End If
        lblVCGrupoObjetivo1.Text = ent.VCGrupoObjetivo
        lblVCGrupoObjetivo2.Text = ent2.VCGrupoObjetivo

        If ent.VCAplicacionInstrumentos <> ent2.VCAplicacionInstrumentos Then
            txtVCAplicacionInstrumentos1.CssClass = "cambioVersion"
            txtVCAplicacionInstrumentos2.CssClass = "cambioVersion1"
        Else
            txtVCAplicacionInstrumentos1.CssClass = "versionIgual"
            txtVCAplicacionInstrumentos2.CssClass = "versionIgual"
        End If
        txtVCAplicacionInstrumentos1.Text = ent.VCAplicacionInstrumentos
        txtVCAplicacionInstrumentos2.Text = ent2.VCAplicacionInstrumentos

        If ent.VCDistribucionCuotas <> ent2.VCDistribucionCuotas Then
            lblVCDistribucionCuotas1.CssClass = "cambioVersion"
            lblVCDistribucionCuotas2.CssClass = "cambioVersion1"
        Else
            lblVCDistribucionCuotas1.CssClass = "versionIgual"
            lblVCDistribucionCuotas2.CssClass = "versionIgual"
        End If
        lblVCDistribucionCuotas1.Text = ent.VCDistribucionCuotas
        lblVCDistribucionCuotas2.Text = ent2.VCDistribucionCuotas

        If ent.VCMetodologia <> ent2.VCMetodologia Then
            txtVCMetodologia1.CssClass = "cambioVersion"
            txtVCMetodologia2.CssClass = "cambioVersion1"
        Else
            txtVCMetodologia1.CssClass = "versionIgual"
            txtVCMetodologia2.CssClass = "versionIgual"
        End If
        txtVCMetodologia1.Text = ent.VCMetodologia
        txtVCMetodologia2.Text = ent2.VCMetodologia

        If ent.Incentivos <> ent2.Incentivos Then
            rblIncentivos1.CssClass = "cambioVersion"
            rblIncentivos2.CssClass = "cambioVersion1"
        Else
            rblIncentivos1.CssClass = "versionIgual"
            rblIncentivos2.CssClass = "versionIgual"
        End If
        If ent.Incentivos Then rblIncentivos1.Text = "Sí" Else rblIncentivos1.Text = "No"
        If ent2.Incentivos Then rblIncentivos2.Text = "Sí" Else rblIncentivos2.Text = "No"

        If ent.PresupuestoIncentivo <> ent2.PresupuestoIncentivo Then
            txtPresupuestoIncentivo1.CssClass = "cambioVersion"
            txtPresupuestoIncentivo2.CssClass = "cambioVersion1"
        Else
            txtPresupuestoIncentivo1.CssClass = "versionIgual"
            txtPresupuestoIncentivo2.CssClass = "versionIgual"
        End If
        txtPresupuestoIncentivo1.Text = ent.PresupuestoIncentivo
        txtPresupuestoIncentivo2.Text = ent2.PresupuestoIncentivo

        If ent.DistribucionIncentivo <> ent2.DistribucionIncentivo Then
            txtDistribucionIncentivo1.CssClass = "cambioVersion"
            txtDistribucionIncentivo2.CssClass = "cambioVersion1"
        Else
            txtDistribucionIncentivo1.CssClass = "versionIgual"
            txtDistribucionIncentivo2.CssClass = "versionIgual"
        End If
        txtDistribucionIncentivo1.Text = ent.DistribucionIncentivo
        txtDistribucionIncentivo2.Text = ent2.DistribucionIncentivo

        If ent.RegaloClientes <> ent2.RegaloClientes Then
            rblRegaloClientes1.CssClass = "cambioVersion"
            rblRegaloClientes2.CssClass = "cambioVersion1"
        Else
            rblRegaloClientes1.CssClass = "versionIgual"
            rblRegaloClientes2.CssClass = "versionIgual"
        End If
        If ent.RegaloClientes Then rblRegaloClientes1.Text = "Sí" Else rblRegaloClientes1.Text = "No"
        If ent2.RegaloClientes Then rblRegaloClientes2.Text = "Sí" Else rblRegaloClientes2.Text = "No"

        If ent.CompraIpsos <> ent2.CompraIpsos Then
            rblCompraIpsos1.CssClass = "cambioVersion"
            rblCompraIpsos2.CssClass = "cambioVersion1"
        Else
            rblCompraIpsos1.CssClass = "versionIgual"
            rblCompraIpsos2.CssClass = "versionIgual"
        End If
        If ent.CompraIpsos Then rblCompraIpsos1.Text = "Sí" Else rblCompraIpsos1.Text = "No"
        If ent2.CompraIpsos Then rblCompraIpsos2.Text = "Sí" Else rblCompraIpsos2.Text = "No"

        If ent.PresupuestoCompra <> ent2.PresupuestoCompra Then
            txtPresupuestoCompra1.CssClass = "cambioVersion"
            txtPresupuestoCompra2.CssClass = "cambioVersion1"
        Else
            txtPresupuestoCompra1.CssClass = "versionIgual"
            txtPresupuestoCompra2.CssClass = "versionIgual"
        End If
        txtPresupuestoCompra1.Text = ent.PresupuestoCompra
        txtPresupuestoCompra2.Text = ent2.PresupuestoCompra

        If ent.DistribucionCompra <> ent2.DistribucionCompra Then
            txtDistribucionCompra1.CssClass = "cambioVersion"
            txtDistribucionCompra2.CssClass = "cambioVersion1"
        Else
            txtDistribucionCompra1.CssClass = "versionIgual"
            txtDistribucionCompra2.CssClass = "versionIgual"
        End If
        txtDistribucionCompra1.Text = ent.DistribucionCompra
        txtDistribucionCompra2.Text = ent2.DistribucionCompra

        'If ent.Moderador <> ent2.Moderador Then
        '    txtModerador1.CssClass = "cambioVersion"
        '    txtModerador2.CssClass = "cambioVersion1"
        'Else
        '    txtModerador1.CssClass = "versionIgual"
        '    txtModerador2.CssClass = "versionIgual"
        'End If
        'txtModerador1.Text = ent.Moderador
        'txtModerador2.Text = ent2.Moderador

        'If ent.Moderador <> ent2.Moderador Then
        '    txtModerador1.CssClass = "cambioVersion"
        '    txtModerador2.CssClass = "cambioVersion1"
        'Else
        '    txtModerador1.CssClass = "versionIgual"
        '    txtModerador2.CssClass = "versionIgual"
        'End If
        'txtModerador1.Text = ent.Moderador
        'txtModerador2.Text = ent2.Moderador

        If ent.ExclusionesyRestricciones <> ent2.ExclusionesyRestricciones Then
            txtExclusionesyRestricciones1.CssClass = "cambioVersion"
            txtExclusionesyRestricciones2.CssClass = "cambioVersion1"
        Else
            txtExclusionesyRestricciones1.CssClass = "versionIgual"
            txtExclusionesyRestricciones2.CssClass = "versionIgual"
        End If
        txtExclusionesyRestricciones1.Text = ent.ExclusionesyRestricciones
        txtExclusionesyRestricciones2.Text = ent2.ExclusionesyRestricciones

        If ent.RecursosPropiedadesCliente <> ent2.RecursosPropiedadesCliente Then
            txtRecursosPropiedadesCliente1.CssClass = "cambioVersion"
            txtRecursosPropiedadesCliente2.CssClass = "cambioVersion1"
        Else
            txtRecursosPropiedadesCliente1.CssClass = "versionIgual"
            txtRecursosPropiedadesCliente2.CssClass = "versionIgual"
        End If
        txtRecursosPropiedadesCliente1.Text = ent.RecursosPropiedadesCliente
        txtRecursosPropiedadesCliente2.Text = ent2.RecursosPropiedadesCliente

        If ent.HabeasData <> ent2.HabeasData Then
            txtHabeasData1.CssClass = "cambioVersion"
            txtHabeasData2.CssClass = "cambioVersion1"
        Else
            txtHabeasData1.CssClass = "versionIgual"
            txtHabeasData2.CssClass = "versionIgual"
        End If
        txtHabeasData1.Text = ent.HabeasData
        txtHabeasData2.Text = ent2.HabeasData

        If ent.OtrasEspecificaciones <> ent2.OtrasEspecificaciones Then
            lblOtrasEspecificaciones1.CssClass = "cambioVersion"
            lblOtrasEspecificaciones2.CssClass = "cambioVersion1"
            pnlOtrasEspecificaciones1.CssClass = "cambioVersion"
            pnlOtrasEspecificaciones2.CssClass = "cambioVersion1"
        Else
            lblOtrasEspecificaciones1.CssClass = "versionIgual"
            lblOtrasEspecificaciones2.CssClass = "versionIgual"
            pnlOtrasEspecificaciones1.CssClass = "versionIgual"
            pnlOtrasEspecificaciones2.CssClass = "versionIgual"
        End If
        lblOtrasEspecificaciones1.Text = ent.OtrasEspecificaciones
        lblOtrasEspecificaciones2.Text = ent2.OtrasEspecificaciones

    End Sub

    Private Sub gvVersionesEC_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvVersionesEC.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Me.gvVersionesEC.DataKeys(e.Row.RowIndex)("NoVersion") = "1" Then
                e.Row.Cells(5).Visible = False
                e.Row.Cells(4).ColumnSpan = 2
            End If
        End If
    End Sub

    Sub CargarEsquemaAnalisis(ByVal IdProyecto As Int64)
        Dim oProyectos_Presupuestos As New Proyectos_Presupuestos
        Dim opI = oProyectos_Presupuestos.ObtenerProyecto(IdProyecto)

        txtA1.Text = opI.A1
        txtA2.Text = opI.A2
        txtA3.Text = opI.A3
        txtA4.Text = opI.A4
        txtA5.Text = opI.A5
        txtA6.Text = opI.A6
        txtA7.Text = opI.A7
        txtA8.Text = opI.A8
    End Sub

    Sub CargarVersionesEspecificacionesCuentas(ByVal IdProyecto As Int64)
        Dim o As New Proyecto
        Dim ent As New List(Of PY_EspCuentasCuali)
        pnlBriefCuentasProyectosCuali.Visible = True

        Try
            ent = o.ObtenerVerEspecifCuentaCuali(IdProyecto)
            If ent.Count > 0 Then
                lblNumVersionEspecificacion.InnerText = "Versión No " + ent(0).NoVersion.ToString
            End If
            gvVersionesEC.DataSource = ent
            gvVersionesEC.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub gvVersionesEC_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVersionesEC.RowCommand
        If e.CommandName = "Ver" Then
            pnlVersionesEC.Visible = False
            pnlDetalleVerEC.Visible = True
            pnlCompararEC.Visible = False
            Dim id = gvVersionesEC.DataKeys(CInt(e.CommandArgument))("id")
            Dim idPr = hfProyectoID.Value
            Dim o As New Proyecto
            Dim ent As New List(Of PY_EspCuentasCuali)

            ent = o.ObtenerEspecifCuentaCualixIdxPr(idPr, id)

            llenarDetalleVersionEC(ent(0))
        ElseIf e.CommandName = "Comparar" Then
            pnlVersionesEC.Visible = False
            pnlDetalleVerEC.Visible = False
            pnlCompararEC.Visible = True
            Dim id = gvVersionesEC.DataKeys(CInt(e.CommandArgument))("id")
            Dim idPr = hfProyectoID.Value
            Dim o As New Proyecto
            Dim ent As New PY_EspCuentasCuali
            Dim ent2 As New PY_EspCuentasCuali
            ent = o.ObtenerEspecifCuentaCualixId(id)

            Dim numVersion = o.ObtenerEspecifCuentaCualiContar(idPr)
            Dim versionActual = gvVersionesEC.DataKeys(CInt(e.CommandArgument))("NoVersion")
            If ent.NoVersion - 1 > 0 Then
                lblErrorVersionEC.Text = ""
                Dim Listent2 As List(Of PY_EspCuentasCuali) = o.ObtenerEspecifCuentaCualiList(idPr)
                ent2 = Listent2(versionActual - 2)
                SubirDatosEC(ent2, ent)
            Else
                lblErrorVersionEC.Text = "No hay versiones anteriores"
            End If

        End If
    End Sub

    Sub llenarDetalleVersionEC(ByVal ent As PY_EspCuentasCuali)
        txtBCPObservacionesVer.Text = ent.BCPObservaciones
        Dim BCPTecnica = ent.BCPTecnica
        Dim txtBCPTecnica = ""
        Select Case BCPTecnica
            Case 1
                txtBCPTecnica = "Entrevista"
            Case 2
                txtBCPTecnica = "Sesiones de grupo/Talleres"
            Case 3
                txtBCPTecnica = "Inmersiones"
            Case 4
                txtBCPTecnica = "Estudios online"
            Case 5
                txtBCPTecnica = "Otro"
            Case Else
                txtBCPTecnica = ""
        End Select
        chbBCPTecnicaVer.Text = txtBCPTecnica
        otraTecnicaVer.Text = ent.BCPotraTecnica
        txtBCPIncentivosEspVer.Text = ent.BCPIncentivosEsp
        txtBCPBDDEspVer.Text = ent.BCPBDDEsp
        txtBCPProductoEspVer.Text = ent.BCPProductoEsp
        Dim BCPReclutamiento = ent.BCPReclutamiento
        Dim txtBCPReclutamiento = ""
        Select Case BCPReclutamiento
            Case 1
                txtBCPReclutamiento = "Base de datos"
            Case 2
                txtBCPReclutamiento = "Convencional"
            Case 3
                txtBCPReclutamiento = "Referidos"
            Case 4
                txtBCPReclutamiento = "En frío"
            Case Else
                txtBCPReclutamiento = ""
        End Select
        chbBCPReclutamientoVer.Text = txtBCPReclutamiento
        txtBCPEspReclutamientoVer.Text = ent.BCPEspReclutamiento
        If ent.BCPEspProducto Then
            lblBCPEspProductoVer.Text = "Sí"
        Else
            lblBCPEspProductoVer.Text = "No"
        End If
        If ent.BCPMaterialEval Then
            lblBCPMaterialEvalVer.Text = "Sí"
        Else
            lblBCPMaterialEvalVer.Text = "No"
        End If
        txtBCPObsProductoVer.Text = ent.BCPObsProducto
    End Sub

    Sub llenarDetalleVersionET(ByVal ent As PY_EspecifTecTrabajoCuali)
        txtModeradorETVer.Text = ent.Moderador
        lblEspecificacionesCampoETVer.Text = ent.EspecificacionesCampo
        txtMaterialApoyoETVer.Text = ent.MaterialApoyo
        txtIncidenciasETVer.Text = ent.Incidencias
        txtAuditoriaCampoETVer.Text = ent.Auditoria
        txtVCSeguridadETVer.Text = ent.VCSeguridad
        txtVCObtencionETVer.Text = ent.VCObtencion
        lblVCGrupoObjetivoETVer.Text = ent.VCGrupoObjetivo
        txtVCAplicacionInstrumentosETVer.Text = ent.VCAplicacionInstrumentos
        lblVCDistribucionCuotasETVer.Text = ent.VCDistribucionCuotas
        txtVCMetodologiaETVer.Text = ent.VCMetodologia
        If ent.Incentivos = 1 Then
            rblIncentivosETVer.Text = "Sí"
        Else
            rblIncentivosETVer.Text = "No"
        End If
        txtPresupuestoIncentivoETVer.Text = ent.PresupuestoIncentivo
        txtDistribucionIncentivoETVer.Text = ent.DistribucionIncentivo
        If ent.RegaloClientes = 1 Then
            rblRegaloClientesETVer.Text = "Sí"
        Else
            rblRegaloClientesETVer.Text = "No"
        End If
        If ent.CompraIpsos = 1 Then
            rblCompraIpsosETVer.Text = "Sí"
        Else
            rblCompraIpsosETVer.Text = "No"
        End If
        txtPresupuestoCompraETVer.Text = ent.PresupuestoCompra
        txtDistribucionCompraETVer.Text = ent.DistribucionCompra
        txtExclusionesyRestriccionesETVer.Text = ent.ExclusionesyRestricciones
        txtRecursosPropiedadesClienteETVer.Text = ent.RecursosPropiedadesCliente
        txtHabeasDataETVer.Text = ent.HabeasData
        lblOtrasEspecificacionesETVer.Text = ent.OtrasEspecificaciones
    End Sub

    Sub LimpiarDetalleVersionEC()
        txtBCPObservacionesVer.Text = ""
        txtBCPIncentivosEspVer.Text = ""
        txtBCPBDDEspVer.Text = ""
        txtBCPProductoEspVer.Text = ""
        txtBCPEspReclutamientoVer.Text = ""
        lblBCPEspProductoVer.Text = ""
        lblBCPMaterialEvalVer.Text = ""
        txtBCPObsProductoVer.Text = ""
    End Sub

    Sub LimpiarDetalleVersionET()
        txtModeradorETVer.Text = ""
        lblEspecificacionesCampoETVer.Text = ""
        txtMaterialApoyoETVer.Text = ""
        txtIncidenciasETVer.Text = ""
        txtAuditoriaCampoETVer.Text = ""
        txtVCSeguridadETVer.Text = ""
        txtVCObtencionETVer.Text = ""
        lblVCGrupoObjetivoETVer.Text = ""
        txtVCAplicacionInstrumentosETVer.Text = ""
        lblVCDistribucionCuotasETVer.Text = ""
        txtVCMetodologiaETVer.Text = ""
        rblIncentivosETVer.Text = ""
        txtPresupuestoIncentivoETVer.Text = ""
        txtDistribucionIncentivoETVer.Text = ""
        rblRegaloClientesETVer.Text = ""
        rblCompraIpsosETVer.Text = ""
        txtPresupuestoCompraETVer.Text = ""
        txtDistribucionCompraETVer.Text = ""
        chbAyudasETVer.Text = ""
        chbReclutamientoETVer.Text = ""
        txtExclusionesyRestriccionesETVer.Text = ""
        txtRecursosPropiedadesClienteETVer.Text = ""
        txtHabeasDataETVer.Text = ""
        lblOtrasEspecificacionesETVer.Text = ""
    End Sub

    Private Sub volverListadoVersEC_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVersEC.ServerClick
        lblErrorVersionEC.Text = ""
        pnlVersionesEC.Visible = True
        pnlDetalleVerEC.Visible = False
        pnlCompararEC.Visible = False
        LimpiarDetalleVersionEC()
    End Sub

    Private Sub volverListadoVersEC2_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVersEC2.ServerClick
        lblErrorVersionEC.Text = ""
        pnlVersionesEC.Visible = True
        pnlDetalleVerEC.Visible = False
        pnlCompararEC.Visible = False
        LimpiarDetalleVersionEC()
    End Sub

    Private Sub volverListadoVersET_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVersET.ServerClick
        lblErrorVersionET.Text = ""
        pnlVersionesET.Visible = True
        pnlDetalleVerET.Visible = False
        pnlCompararET.Visible = False
        LimpiarDetalleVersionET()
    End Sub

    Private Sub volverListadoVersET2_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVersET2.ServerClick
        lblErrorVersionET.Text = ""
        pnlVersionesET.Visible = True
        pnlDetalleVerET.Visible = False
        pnlCompararET.Visible = False
        LimpiarDetalleVersionET()
    End Sub

    Private Sub gvVersionesET_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvVersionesET.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Me.gvVersionesET.DataKeys(e.Row.RowIndex)("NoVersion") = "1" Then
                e.Row.Cells(5).Visible = False
                e.Row.Cells(4).ColumnSpan = 2
            End If
        End If
    End Sub

    Private Sub gvVersionesET_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVersionesET.RowCommand
        If e.CommandName = "Ver" Then
            pnlVersionesET.Visible = False
            pnlDetalleVerET.Visible = True
            pnlCompararET.Visible = False
            Dim id = gvVersionesET.DataKeys(CInt(e.CommandArgument))("id")
            Dim idPr = hfProyectoID.Value
            Dim idTr = hfTrabajoID.Value
            Dim o As New CoreProject.SegmentosCuali
            Dim ent As New List(Of PY_EspecifTecTrabajoCuali)

            ent = o.ObtenerEspecifacionesCualixIdxTr(idTr, id)

            llenarDetalleVersionET(ent(0))
        ElseIf e.CommandName = "Comparar" Then
            pnlVersionesET.Visible = False
            pnlDetalleVerET.Visible = False
            pnlCompararET.Visible = True
            Dim id = gvVersionesET.DataKeys(CInt(e.CommandArgument))("id")
            Dim idPr = hfProyectoID.Value
            Dim idTr = hfTrabajoID.Value
            Dim o As New CoreProject.SegmentosCuali
            Dim ent As New PY_EspecifTecTrabajoCuali
            Dim ent2 As New PY_EspecifTecTrabajoCuali
            ent = o.ObtenerEspecifacionesCualixId(id)

            Dim numVersion = o.ObtenerEspecifacionesContar(idTr)
            Dim versionActual = gvVersionesET.DataKeys(CInt(e.CommandArgument))("NoVersion")
            If ent.NoVersion - 1 > 0 Then
                lblErrorVersionET.Text = ""
                Dim Listent2 As List(Of PY_EspecifTecTrabajoCuali) = o.ObtenerEspecifacionesCualiListAsc(idTr)
                ent2 = Listent2(versionActual - 2)
                SubirDatosET(ent2, ent)
            Else
                lblErrorVersionET.Text = "No hay versiones anteriores"
            End If

        End If
    End Sub
End Class


