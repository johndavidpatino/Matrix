Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios

Public Class MetodologiaDeCampo
    Inherits System.Web.UI.Page

    Enum ETarea
        Metodologia = 23
    End Enum

#Region " Eventos del Control"
    Protected Sub chklista_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chklista.SelectedIndexChanged
        For Each chkitem As ListItem In chklista.Items
            Select Case chkitem.Value
                Case 1
                    If chkitem.Selected Then
                        pnlobjetivos.Visible = True
                    Else
                        pnlobjetivos.Visible = False
                    End If
                Case 2
                    If chkitem.Selected Then
                        pnlmercado.Visible = True
                    Else
                        pnlmercado.Visible = False
                    End If
                Case 3
                    If chkitem.Selected Then
                        pnlmarco.Visible = True
                    Else
                        pnlmarco.Visible = False
                    End If
                Case 4
                    If chkitem.Selected Then
                        pnltecnica.Visible = True
                    Else
                        pnltecnica.Visible = False
                    End If
                Case 5
                    If chkitem.Selected Then
                        pnldiseno.Visible = True
                    Else
                        pnldiseno.Visible = False
                    End If
                Case 6
                    If chkitem.Selected Then
                        pnlinstrucciones.Visible = True
                    Else
                        pnlinstrucciones.Visible = False
                    End If
                Case 7
                    If chkitem.Selected Then
                        pnldistribucion.Visible = True
                    Else
                        pnldistribucion.Visible = False
                    End If
                Case 8
                    If chkitem.Selected Then
                        pnlnivelconfianza.Visible = True
                    Else
                        pnlnivelconfianza.Visible = False
                    End If
                Case 9
                    If chkitem.Selected Then
                        pnlmargenerror.Visible = True
                    Else
                        pnlmargenerror.Visible = False
                    End If
                Case 10
                    If chkitem.Selected Then
                        pnldesagregacion.Visible = True
                    Else
                        pnldesagregacion.Visible = False
                    End If
                Case 11
                    If chkitem.Selected Then
                        pnlfuente.Visible = True
                    Else
                        pnlfuente.Visible = False
                    End If
                Case 12
                    If chkitem.Selected Then
                        pnlVariables.Visible = True
                    Else
                        pnlVariables.Visible = False
                    End If
                Case 13
                    If chkitem.Selected Then
                        pnltasa.Visible = True
                    Else
                        pnltasa.Visible = False
                    End If
                Case 14
                    If chkitem.Selected Then
                        pnlprocedimiento.Visible = True
                    Else
                        pnlprocedimiento.Visible = False
                    End If
            End Select
        Next
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub
    Protected Sub gvDatos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDatos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblobjetivos As Label = e.Row.FindControl("lblobjetivos")
            If (lblobjetivos.Text.Length > 25) Then
                lblobjetivos.Text = lblobjetivos.Text.Substring(0, 25) + "..."
            End If

            Dim lblmercado As Label = e.Row.FindControl("lblmercado")
            If (lblmercado.Text.Length > 25) Then
                lblmercado.Text = lblmercado.Text.Substring(0, 25) + "..."
            End If

            Dim lblmarco As Label = e.Row.FindControl("lblmarco")
            If (lblmarco.Text.Length > 25) Then
                lblmarco.Text = lblmarco.Text.Substring(0, 25) + "..."
            End If

            Dim lbltecnica As Label = e.Row.FindControl("lbltecnica")
            If (lbltecnica.Text.Length > 25) Then
                lbltecnica.Text = lbltecnica.Text.Substring(0, 25) + "..."
            End If

            Dim lbldiseno As Label = e.Row.FindControl("lbldiseno")
            If (lbldiseno.Text.Length > 25) Then
                lbldiseno.Text = lbldiseno.Text.Substring(0, 25) + "..."
            End If

            Dim lblinstrucciones As Label = e.Row.FindControl("lblinstrucciones")
            If (lblinstrucciones.Text.Length > 25) Then
                lblinstrucciones.Text = lblinstrucciones.Text.Substring(0, 25) + "..."
            End If

            Dim lbldistribucion As Label = e.Row.FindControl("lbldistribucion")
            If (lbldistribucion.Text.Length > 25) Then
                lbldistribucion.Text = lbldistribucion.Text.Substring(0, 25) + "..."
            End If

            Dim lblnivelconfianza As Label = e.Row.FindControl("lblnivelconfianza")
            If (lblnivelconfianza.Text.Length > 25) Then
                lblnivelconfianza.Text = lblnivelconfianza.Text.Substring(0, 25) + "..."
            End If

            Dim lblmargen As Label = e.Row.FindControl("lblmargen")
            If (lblmargen.Text.Length > 25) Then
                lblmargen.Text = lblmargen.Text.Substring(0, 25) + "..."
            End If

            Dim lbldesagregacion As Label = e.Row.FindControl("lbldesagregacion")
            If (lbldesagregacion.Text.Length > 25) Then
                lbldesagregacion.Text = lbldesagregacion.Text.Substring(0, 25) + "..."
            End If

            Dim lblfuente As Label = e.Row.FindControl("lblfuente")
            If (lblfuente.Text.Length > 25) Then
                lblfuente.Text = lblfuente.Text.Substring(0, 25) + "..."
            End If

            Dim lblvariables As Label = e.Row.FindControl("lblvariables")
            If (lblvariables.Text.Length > 25) Then
                lblvariables.Text = lblvariables.Text.Substring(0, 25) + "..."
            End If

            Dim lbltasa As Label = e.Row.FindControl("lbltasa")
            If (lbltasa.Text.Length > 25) Then
                lbltasa.Text = lbltasa.Text.Substring(0, 25) + "..."
            End If

            Dim lblprocedimiento As Label = e.Row.FindControl("lblprocedimiento")
            If (lblprocedimiento.Text.Length > 25) Then
                lblprocedimiento.Text = lblprocedimiento.Text.Substring(0, 25) + "..."
            End If

        End If
    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        CargaMetodologia()
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Duplicar"
                    Dim idMetodo As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfo(idMetodo)
                Case "Modificar"
                    Dim idMetodo As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfo(idMetodo)
                Case "Eliminar"
                    Dim idMetodo As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar(idMetodo)
                    CargaMetodologia()
                    ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
                    log(hfidmetodologia.Value, 4)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Request.QueryString("idtrabajo") IsNot Nothing Then
                Dim idtrabajo As Int64 = Int64.Parse(Request.QueryString("idtrabajo").ToString)
                hfidtrabajo.Value = idtrabajo
                CargaMetodologia()
            Else
                If Request.QueryString("pendientes") IsNot Nothing Then
                    CargarTrabajosSinMetodologia()
                Else
                    'CargarTrabajos()
                End If
            End If
        End If
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            Guardar()
            limpiar()
            CargaMetodologia()
            log(hfidmetodologia.Value, 2)
            cerrarTarea(Int64.Parse(hfidtrabajo.Value))
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(2, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargaMetodologia()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        limpiar()
        btnGuardar.Text = "Guardar"
        accordion2.Visible = True
        accordion1.Visible = False
        accordion0.Visible = False
        txtNombreEstudio.Focus()
    End Sub
    Private Sub gvTrabajos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos()
    End Sub
    Private Sub gvTrabajos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Actualizar" Then
            'cargarTrabajo(Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")))
            hfidtrabajo.Value = Int64.Parse(Me.gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id"))
            CargaMetodologia()
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "Tareas" Then
            'Response.Redirect("\CORE\Tareas.aspx" & "?IdTrabajo=" & gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id") & "&Coe=" & Session("IDUsuario").ToString)
        End If
    End Sub

    Sub cerrarTarea(idTrabajo As Int64)
        Dim daWorkFlow As New WorkFlow
        Dim oWorkFlow As CORE_WorkFlow
        Dim oLogWorkFlow As New LogWorkFlow

        oWorkFlow = daWorkFlow.ObtenerWorkFlowXIdTrabajoXIdTarea(idTrabajo, ETarea.Metodologia)
        oLogWorkFlow.CORE_Log_WorkFlow_Add(oWorkFlow.id, DateTime.UtcNow.AddHours(-5), Session("IDUsuario").ToString, LogWorkFlow.WorkFlowEstados.Finalizada, Nothing)

        oWorkFlow.FFinR = Date.UtcNow.AddHours(-5)
        oWorkFlow.Estado = LogWorkFlow.WorkFlowEstados.Finalizada
        oWorkFlow.FechaUltimoEstado = Date.UtcNow.AddHours(-5)

        daWorkFlow.GuardarWorkFlow(oWorkFlow)

    End Sub
#End Region
#Region "Funciones y Metodos"
    Public Sub limpiar()
        hfidmetodologia.Value = String.Empty
        txtBuscar.Text = String.Empty
        txtDesagregacion.Text = String.Empty
        txtDiseno.Text = String.Empty
		txtDistribucionDeMuestra.Content = String.Empty
		txtFecha.Text = String.Empty
		txtFecha.Text = String.Empty
		txtFecha.Text = String.Empty
		txtFuente.Text = String.Empty
        txtInstrucciones.Content = String.Empty
        txtMarcoMuestral.Text = String.Empty
        txtMargenError.Text = String.Empty
        txtMercado.Text = String.Empty
        txtNivelConfianza.Text = String.Empty
        txtNombreEstudio.Text = String.Empty
        txtObjetivos.Text = String.Empty
        txtprocedimiento.Text = String.Empty
        txtTasa.Text = String.Empty
        txtTecnica.Text = String.Empty
        txtVariables.Content = String.Empty
        pnldesagregacion.Visible = False
        pnldiseno.Visible = False
        pnldistribucion.Visible = False
        pnlfuente.Visible = False
        pnlinstrucciones.Visible = False
        pnlmarco.Visible = False
        pnlmargenerror.Visible = False
        pnlmercado.Visible = False
        pnlnivelconfianza.Visible = False
        pnlobjetivos.Visible = False
        pnlprocedimiento.Visible = False
        pnltasa.Visible = False
        pnltecnica.Visible = False
        pnlVariables.Visible = False
        For Each chkitem As ListItem In chklista.Items
            chkitem.Selected = False
        Next

    End Sub
    Sub CargarTrabajos()
        Dim oTrabajo As New Trabajo
        gvTrabajos.DataSource = oTrabajo.obtenerPorBusqueda(txtBusquedaTrabajo.Text)
        gvTrabajos.DataBind()
    End Sub

    Sub CargarTrabajosSinMetodologia()
        Dim oTrabajo As New Trabajo
        gvTrabajos.DataSource = oTrabajo.ObtenerTrabajosSinMetodologiaCampo()
        gvTrabajos.DataBind()
    End Sub
    Public Sub CargaMetodologia()
        Try
            Dim idtrabajo As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim oMetodologia As New MetodologiaCampo
            Dim listaMetodologia = (From lmetodo In oMetodologia.DevolverxIDTrabajo(idtrabajo)
                                    Select Id = lmetodo.Id,
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
                                    Procedimiento = lmetodo.ProcedimientoT).OrderBy(Function(m) m.Id)
            If (Not String.IsNullOrEmpty(txtBuscar.Text)) Then
                gvDatos.DataSource = listaMetodologia.Where(Function(c) (c.Objetivo.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Mercado.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Marco.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Tecnica.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Diseno.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Instrucciones.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Distribucion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.NivelConfianza.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.MargenError.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Desagregacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Fuente.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Variables.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Tasa.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Procedimiento.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList
            Else
                gvDatos.DataSource = listaMetodologia.ToList
            End If
            gvDatos.DataBind()
        Catch ex As Exception
            Throw ex
        End Try
        accordion1.Visible = True
        accordion0.Visible = False
        accordion2.Visible = False
    End Sub
    Public Sub CargarInfo(ByVal idMetodo As Int64)
        Try
            Dim oMetodo As New MetodologiaCampo
            Dim info = oMetodo.DevolverxID(idMetodo)
            hfidmetodologia.Value = idMetodo
            hfidtrabajo.Value = info.TrabajoId
            pnlVersion.Visible = True
            Dim versiones = oMetodo.ObtenerMetodologiaNumVersionesxTr(info.TrabajoId) + 1
            txtVersion.Text = versiones.ToString

            For Each chkitem As ListItem In chklista.Items
                Select Case chkitem.Value
                    Case 1
                        If info.Objetivo Then
                            chkitem.Selected = True
                            pnlobjetivos.Visible = True
                            txtObjetivos.Text = info.ObjetivoT
                        End If
                    Case 2
                        If info.Mercado Then
                            chkitem.Selected = True
                            pnlmercado.Visible = True
                            txtMercado.Text = info.MercadoT
                        End If
                    Case 3
                        If info.Marco Then
                            chkitem.Selected = True
                            pnlmarco.Visible = True
                            txtMarcoMuestral.Text = info.MarcoT
                        End If
                    Case 4
                        If info.Tecnica Then
                            chkitem.Selected = True
                            pnltecnica.Visible = True
                            txtTecnica.Text = info.TecnicaT
                        End If
                    Case 5
                        If info.Diseno Then
                            chkitem.Selected = True
                            pnldiseno.Visible = True
                            txtDiseno.Text = info.DisenoT
                        End If
                    Case 6
                        If info.Instrucciones Then
                            chkitem.Selected = True
                            pnlinstrucciones.Visible = True
                            txtInstrucciones.Content = info.InstruccionesT
                        End If
                    Case 7
                        If info.Distribucion Then
                            chkitem.Selected = True
                            pnldistribucion.Visible = True
							txtDistribucionDeMuestra.Content = info.DistribucionT
						End If
					Case 8
						If info.NivelConfianza Then
                            chkitem.Selected = True
                            pnlnivelconfianza.Visible = True
                            txtNivelConfianza.Text = info.NivelConfianzaT
                        End If
                    Case 9
                        If info.MargenError Then
                            chkitem.Selected = True
                            pnlmargenerror.Visible = True
                            txtMargenError.Text = info.MargenErrorT
                        End If
                    Case 10
                        If info.Desagregacion Then
                            chkitem.Selected = True
                            pnldesagregacion.Visible = True
                            txtDesagregacion.Text = info.DesagregacionT
                        End If
                    Case 11
                        If info.Fuente Then
                            chkitem.Selected = True
                            pnlfuente.Visible = True
                            txtFuente.Text = info.FuenteT
                        End If
                    Case 12
                        If info.Variables Then
                            chkitem.Selected = True
                            pnlVariables.Visible = True
                            txtVariables.Content = info.VariablesT
                        End If
                    Case 13
                        If info.Tasa Then
                            chkitem.Selected = True
                            pnltasa.Visible = True
                            txtTasa.Text = info.TasaT
                        End If
                    Case 14
                        If info.Procedimiento Then
                            chkitem.Selected = True
                            pnlprocedimiento.Visible = True
                            txtprocedimiento.Text = info.ProcedimientoT
                        End If
                End Select
            Next
            accordion1.Visible = False
            accordion0.Visible = False
            accordion2.Visible = True
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Eliminar(ByVal idMetodo As Int64)
        Try
            Dim oMetodo As New MetodologiaCampo
            oMetodo.Eliminar(idMetodo)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Guardar()
        Try
            Dim oMetodo As New MetodologiaCampo
            Dim idtrabajo As Int64 = Int64.Parse(hfidtrabajo.Value)
            Dim Objetivos, Mercado, Marco, Tecnica, Diseno, Instrucciones, Distribucion, NivelConfianza, MargenError, Desagregacion, Fuente, Variables, Tasa, Procedimiento As Boolean
            Dim ObjetivosT = "", MercadoT = "", MarcoT = "", TecnicaT = "", DisenoT = "", InstruccionesT = "", DistribucionT = "", NivelConfianzaT = "", MargenErrorT = "", DesagregacionT = "", FuenteT = "", VariablesT = "", TasaT = "", ProcedimientoT As String = ""
            Dim versiones = oMetodo.ObtenerMetodologiaNumVersionesxTr(idtrabajo) + 1
            txtVersion.Text = versiones.ToString
            Dim usuario = Session("IDUsuario").ToString

            For Each chkitem As ListItem In chklista.Items
                Select Case chkitem.Value
                    Case 1
                        If chkitem.Selected Then
                            Objetivos = True
                            ObjetivosT = txtObjetivos.Text.ToString
                        End If
                    Case 2
                        If chkitem.Selected Then
                            Mercado = True
                            MercadoT = txtMercado.Text.ToString
                        End If
                    Case 3
                        If chkitem.Selected Then
                            Marco = True
                            MarcoT = txtMarcoMuestral.Text.ToString
                        End If
                    Case 4
                        If chkitem.Selected Then
                            Tecnica = True
                            TecnicaT = txtTecnica.Text.ToString
                        End If
                    Case 5
                        If chkitem.Selected Then
                            Diseno = True
                            DisenoT = txtDiseno.Text.ToString
                        End If
                    Case 6
                        If chkitem.Selected Then
                            Instrucciones = True
                            InstruccionesT = txtInstrucciones.Content
                        End If
                    Case 7
                        If chkitem.Selected Then
                            Distribucion = True
							DistribucionT = txtDistribucionDeMuestra.Content
						End If
					Case 8
						If chkitem.Selected Then
                            NivelConfianza = True
                            NivelConfianzaT = txtNivelConfianza.Text.ToString
                        End If
                    Case 9
                        If chkitem.Selected Then
                            MargenError = True
                            MargenErrorT = txtMargenError.Text.ToString
                        End If
                    Case 10
                        If chkitem.Selected Then
                            Desagregacion = True
                            DesagregacionT = txtDesagregacion.Text.ToString
                        End If
                    Case 11
                        If chkitem.Selected Then
                            Fuente = True
                            FuenteT = txtFuente.Text.ToString
                        End If
                    Case 12
                        If chkitem.Selected Then
                            Variables = True
                            VariablesT = txtVariables.Content.ToString
                        End If
                    Case 13
                        If chkitem.Selected Then
                            Tasa = True
                            TasaT = txtTasa.Text.ToString
                        End If
                    Case 14
                        If chkitem.Selected Then
                            Procedimiento = True
                            ProcedimientoT = txtprocedimiento.Text.ToString
                        End If
                End Select
            Next

            Dim Nombre As String = String.Empty
            Dim idmetodo As Int64
            Dim fecha As DateTime = Date.UtcNow.AddHours(-5)

            If Not String.IsNullOrEmpty(txtNombreEstudio.Text) Then
                Nombre = txtNombreEstudio.Text
            End If

            If Not String.IsNullOrEmpty(hfidmetodologia.Value) Then
                idmetodo = Int64.Parse(hfidmetodologia.Value)
            End If

            If Not String.IsNullOrEmpty(txtFecha.Text) Then
                fecha = txtFecha.Text
            End If
            Dim flag As Boolean = False
            If idmetodo = 0 Then
                flag = True
            End If
            oMetodo.Guardar(idmetodo, idtrabajo, Nombre, fecha, Objetivos, Mercado, Marco, Tecnica, Diseno, Instrucciones, Distribucion, NivelConfianza, MargenError, Desagregacion, Fuente, Variables, Tasa, Procedimiento, ObjetivosT, MercadoT, MarcoT, TecnicaT, DisenoT, InstruccionesT, DistribucionT, NivelConfianzaT, MargenErrorT, DesagregacionT, FuenteT, VariablesT, TasaT, ProcedimientoT, versiones, usuario)

            'If flag = True Then
            EnviarEmail(idtrabajo)
            'End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub EnviarEmail(ByVal idtrabajo As Int64)
        Try
            If String.IsNullOrEmpty(idtrabajo) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar una metodología de campo")
            End If
            Dim script As String = "window.open('../Emails/MetodologiaDeCampo.aspx?idtrabajo=" & idtrabajo & "','cal','width=400,height=250,left=270,top=180')"
            Dim page As Page = DirectCast(Context.Handler, Page)
            ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
    End Sub
    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Dim log As New LogEjecucion
        log.Guardar(19, iddoc, Now(), Session("IDUsuario"), idaccion)

    End Sub

    Private Sub btnBuscarTrabajo_Click(sender As Object, e As EventArgs) Handles btnBuscarTrabajo.Click
        CargarTrabajos()
    End Sub

#End Region


End Class