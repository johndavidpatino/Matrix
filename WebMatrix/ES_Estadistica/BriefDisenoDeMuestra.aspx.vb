Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Public Class BriefDisenoDeMuestra
    Inherits System.Web.UI.Page
#Region " Eventos del Control"
    Protected Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("../CU_Cuentas/Propuestas.aspx?IdPropuesta=" & hfidpropuesta.Value)
    End Sub

    Protected Sub gvDatos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDatos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblobjetivos As Label = e.Row.FindControl("lblobjetivos")
            If (lblobjetivos.Text.Length > 25) Then
                lblobjetivos.Text = lblobjetivos.Text.Substring(0, 25) + "..."
            End If

            Dim lblpoblacion As Label = e.Row.FindControl("lblpoblacion")
            If (lblpoblacion.Text.Length > 25) Then
                lblpoblacion.Text = lblpoblacion.Text.Substring(0, 25) + "..."
            End If

            Dim lblcapacidad As Label = e.Row.FindControl("lblcapacidad")
            If (lblcapacidad.Text.Length > 25) Then
                lblcapacidad.Text = lblcapacidad.Text.Substring(0, 25) + "..."
            End If

            Dim lblmetodologia As Label = e.Row.FindControl("lblmetodologia")
            If (lblmetodologia.Text.Length > 25) Then
                lblmetodologia.Text = lblmetodologia.Text.Substring(0, 25) + "..."
            End If

            Dim lbldesagregacion As Label = e.Row.FindControl("lbldesagregacion")
            If (lbldesagregacion.Text.Length > 25) Then
                lbldesagregacion.Text = lbldesagregacion.Text.Substring(0, 25) + "..."
            End If

            Dim lblposiblesmarcos As Label = e.Row.FindControl("lblposiblesmarcos")
            If (lblposiblesmarcos.Text.Length > 25) Then
                lblposiblesmarcos.Text = lblposiblesmarcos.Text.Substring(0, 25) + "..."
            End If
        End If
    End Sub
    Protected Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging
        gvDatos.PageIndex = e.NewPageIndex
        If Not hfidpropuesta.Value = "" Then
            CargarBriefsDisenosPropuestas(hfidbrief.Value)
        Else
            CargarBriefsDisenos()
        End If

    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            If Session.Count = 0 Then
                Response.Redirect("../")
            End If
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(37, UsuarioID) = False Then
                Me.btnNuevo.Visible = False
            End If
            If Request.QueryString("idPropuesta") IsNot Nothing Then
                Dim idpropuesta As Int64 = Int64.Parse(Request.QueryString("idPropuesta").ToString)
                hfidpropuesta.Value = idpropuesta
                Me.btnVolver.Visible = True
                CargarBriefsDisenosPropuestas(idpropuesta)
                If Me.gvDatos.Rows.Count = 0 Then
                    If Request.QueryString("brieflist") IsNot Nothing Then
                        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
                    Else
                        Prellenado()
                        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                    End If
                End If
            Else
                Me.btnNuevo.Visible = False
                If Request.QueryString("pendientes") IsNot Nothing Then
                    CargarBriefsSinDiseno()
                End If

            End If
        End If
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            Guardar()
            If hfFlag.Value > 0 Then
                ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
                log(hfidbrief.Value, 2)
                'Limpiar()
                'CargarBriefsDisenos()
                'CargarBriefsDisenosPropuestas(hfidpropuesta.Value)
            End If
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Modificar"
                    Dim idBrief As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    CargarInfo(idBrief)
                    log(hfidbrief.Value, 3)
                Case "Eliminar"
                    Dim idBrief As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Eliminar(idBrief)
                    CargarBriefsDisenos()
                    ShowNotification("Registro Eliminado correctamente", ShowNotifications.InfoNotification)
                    log(hfidbrief.Value, 4)
                Case "Diseno"
                    Dim idBrief As Int32 = Int32.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Response.Redirect("DisenoDeMuestra.aspx?idbrief=" & idBrief.ToString)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Protected Sub btnNuevo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNuevo.Click
        Limpiar()
        accordion2.Visible = True
        accordion1.Visible = False
        Prellenado()
    End Sub

    Protected Sub btnBuscar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBuscar.Click
        Try
            CargarBriefsDisenos()
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Protected Sub btnDocumentos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDocumentos.Click
        If hfidbrief.Value <> "" Then
            Response.Redirect("\GD_Documentos\GD_Documentos.aspx?IdContenedor=" & hfidbrief.Value & "&IdDocumento=4")
        End If
    End Sub

    Private Sub gvVersionesBDM_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvVersionesBDM.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Me.gvVersionesBDM.DataKeys(e.Row.RowIndex)("NoVersion") = "1" Then
                e.Row.Cells(4).Visible = False
                e.Row.Cells(3).ColumnSpan = 2
            End If
        End If
    End Sub

    Private Sub gvVersionesBDM_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvVersionesBDM.RowCommand
        If e.CommandName = "Ver" Then
            pnlVersionesBDM.Visible = False
            pnlDetalleVerBDM.Visible = True
            pnlCompararBDM.Visible = False
            Dim id = gvVersionesBDM.DataKeys(CInt(e.CommandArgument))("id")
            Dim oBrief As New BriefDisenoMuestral
            Dim briefDiseño As List(Of ES_BriefDisenoMuestral) = oBrief.ObtenerBriefXId(id)

            llenarDetalleVersionBDM(briefDiseño.FirstOrDefault)
        ElseIf e.CommandName = "Comparar" Then
            pnlVersionesBDM.Visible = False
            pnlDetalleVerBDM.Visible = False
            pnlCompararBDM.Visible = True
            Dim id = gvVersionesBDM.DataKeys(CInt(e.CommandArgument))("id")
            Dim oBrief As New BriefDisenoMuestral
            Dim briefDiseño As List(Of ES_BriefDisenoMuestral) = oBrief.ObtenerBriefXPresupuesto(hfidpropuesta.Value)
            Dim brief1 As ES_BriefDisenoMuestral = oBrief.ObtenerBriefXId(id).FirstOrDefault()

            Dim numVersion = briefDiseño.Count - 1
            Dim versionActual = gvVersionesBDM.DataKeys(CInt(e.CommandArgument))("NoVersion")
            If brief1.NoVersion - 1 > 0 Then
                lblErrorVersionBDM.Text = ""
                Dim brief2 As ES_BriefDisenoMuestral = briefDiseño.Item(versionActual - 2)
                SubirDatosBDM(brief2, brief1)
            Else
                lblErrorVersionBDM.Text = "No hay versiones anteriores"
            End If

        End If
    End Sub

    Private Sub volverListadoVersBDM_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVersBDM.ServerClick
        lblErrorVersionBDM.Text = ""
        pnlVersionesBDM.Visible = True
        pnlDetalleVerBDM.Visible = False
        pnlCompararBDM.Visible = False
        LimpiarDetalleVersionBDM()
    End Sub

    Private Sub volverListadoVersBDM2_ServerClick(sender As Object, e As EventArgs) Handles volverListadoVersBDM2.ServerClick
        lblErrorVersionBDM.Text = ""
        pnlVersionesBDM.Visible = True
        pnlDetalleVerBDM.Visible = False
        pnlCompararBDM.Visible = False
        LimpiarDetalleVersionBDM()
    End Sub

#End Region

#Region "Funciones y Metodos"
    Public Sub Limpiar()
        hfidbrief.Value = String.Empty
        txtBuscar.Text = String.Empty
        txtCapacidad.Text = String.Empty
        txtDesagregacion.Text = String.Empty
        txtFecha.Text = String.Empty
        txtMarcos.Text = String.Empty
        txtMetodologia.Text = String.Empty
        txtObjetivos.Text = String.Empty
        txtPoblacion.Text = String.Empty
        txtVariables.Text = String.Empty
        txtObservaciones.Text = String.Empty
        hfVersion.Value = 0
    End Sub
    Public Sub CargarBriefsDisenos()
        Try
            Dim oBrief As New BriefDisenoMuestral
            Dim listabriefs = (From lbrief In oBrief.DevolverTodos()
                               Select Id = lbrief.id,
                               Propuesta = lbrief.Titulo,
                               Objetivo = lbrief.Objetivo,
                               Poblacion = lbrief.Poblacion,
                               Capacidad = lbrief.Capacidad,
                               Metodologia = lbrief.Metodologia,
                               Desagregacion = lbrief.NivelesDesagregacion,
                               PosiblesMarcos = lbrief.PosiblesMarcos,
                               Variable = lbrief.Variable,
                               Observaciones = lbrief.Observaciones,
                               Fecha = lbrief.Fecha).OrderBy(Function(b) b.Fecha)

            If (Not String.IsNullOrEmpty(txtBuscar.Text)) Then
                gvDatos.DataSource = listabriefs.Where(Function(c) (c.Objetivo.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Poblacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Capacidad.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Metodologia.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Desagregacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.PosiblesMarcos.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Variable.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Observaciones.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList
            Else
                gvDatos.DataSource = listabriefs.ToList
            End If
            gvDatos.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub CargarBriefsDisenosPropuestas(ByVal PropuestaId As Long)
        Try
            Dim oBrief As New BriefDisenoMuestral
            Dim listabriefs = (From lbrief In oBrief.DevolverxIDPropuesta(PropuestaId)
                               Select Id = lbrief.id,
                               Propuesta = lbrief.Titulo,
                               Objetivo = lbrief.Objetivo,
                               Poblacion = lbrief.Poblacion,
                               Capacidad = lbrief.Capacidad,
                               Metodologia = lbrief.Metodologia,
                               Desagregacion = lbrief.NivelesDesagregacion,
                               PosiblesMarcos = lbrief.PosiblesMarcos,
                               Fecha = lbrief.Fecha).OrderBy(Function(b) b.Fecha)

            If (Not String.IsNullOrEmpty(txtBuscar.Text)) Then
                gvDatos.DataSource = listabriefs.Where(Function(c) (c.Propuesta = PropuestaId)).ToList
            Else
                gvDatos.DataSource = listabriefs.ToList
            End If
            gvDatos.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub CargarBriefsSinDiseno()
        Try
            Dim oBrief As New BriefDisenoMuestral
            Dim oDiseno As New DisenoMuestral
            Dim listadiseno = (From ldiseno In oDiseno.DevolverTodos
                               Select briefid = ldiseno.BriefId)

            Dim listabriefs = (From lbrief In oBrief.DevolverTodos()
                               Select Id = lbrief.id,
                               Propuesta = lbrief.Titulo,
                               Objetivo = lbrief.Objetivo,
                               Poblacion = lbrief.Poblacion,
                               Capacidad = lbrief.Capacidad,
                               Metodologia = lbrief.Metodologia,
                               Desagregacion = lbrief.NivelesDesagregacion,
                               PosiblesMarcos = lbrief.PosiblesMarcos,
                               Fecha = lbrief.Fecha
                               Where Not (listadiseno.Contains(Id))).OrderBy(Function(b) b.Fecha)

            If (Not String.IsNullOrEmpty(txtBuscar.Text)) Then
                gvDatos.DataSource = listabriefs.Where(Function(c) (c.Objetivo.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Poblacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Capacidad.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Metodologia.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.Desagregacion.ToUpper.Contains(txtBuscar.Text.ToUpper) Or c.PosiblesMarcos.ToUpper.Contains(txtBuscar.Text.ToUpper))).ToList
            Else
                gvDatos.DataSource = listabriefs.ToList
            End If
            gvDatos.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub Guardar()
        Try
            Dim oBrief As New BriefDisenoMuestral
            Dim ID As Int64
            Dim fecha As String = ""
            Dim Objetivo As String = ""
            Dim Poblacion As String = ""
            Dim Capacidad As String = ""
            Dim Metodologia As String = ""
            Dim Desagregacion As String = ""
            Dim Marcos As String = ""
            Dim Variable As String = ""
            Dim Observaciones As String = ""

            If Not String.IsNullOrEmpty(hfidbrief.Value) Then
                ID = Int64.Parse(hfidbrief.Value)
            End If

            If Not String.IsNullOrEmpty(txtFecha.Text) Then
                fecha = txtFecha.Text.ToString()
            Else
                ShowNotification("Este campo es Obligatorio", ShowNotifications.ErrorNotification)
                txtFecha.Focus()
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(txtObjetivos.Text) Then
                Objetivo = txtObjetivos.Text.ToString()
            Else
                ShowNotification("Este campo es Obligatorio", ShowNotifications.ErrorNotification)
                txtObjetivos.Focus()
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(txtPoblacion.Text) Then
                Poblacion = txtPoblacion.Text.ToString
            Else
                ShowNotification("Este campo es Obligatorio", ShowNotifications.ErrorNotification)
                txtPoblacion.Focus()
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(txtCapacidad.Text) Then
                Capacidad = txtCapacidad.Text.ToString
            Else
                ShowNotification("Este campo es Obligatorio", ShowNotifications.ErrorNotification)
                txtCapacidad.Focus()
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(txtMetodologia.Text) Then
                Metodologia = txtMetodologia.Text.ToString
            Else
                ShowNotification("Este campo es Obligatorio", ShowNotifications.ErrorNotification)
                txtMetodologia.Focus()
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(txtDesagregacion.Text) Then
                Desagregacion = txtDesagregacion.Text.ToString
            Else
                ShowNotification("Este campo es Obligatorio", ShowNotifications.ErrorNotification)
                txtDesagregacion.Focus()
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(txtMarcos.Text) Then
                Marcos = txtMarcos.Text.ToString
            Else
                ShowNotification("Este campo es Obligatorio", ShowNotifications.ErrorNotification)
                txtMarcos.Focus()
                Exit Sub
            End If

            If Not String.IsNullOrEmpty(txtVariables.Text) Then
                Variable = txtVariables.Text.ToString
            Else
                ShowNotification("Este campo es Obligatorio", ShowNotifications.ErrorNotification)
                txtVariables.Focus()
                Exit Sub
            End If

            Observaciones = txtObservaciones.Text.ToString
            hfVersion.Value = hfVersion.Value + 1

            Dim flag As Boolean = False
            If ID = 0 Then
                flag = True
            End If
            hfidbrief.Value = oBrief.Guardar(Nothing, hfidpropuesta.Value, fecha, Objetivo, Poblacion, Capacidad, Metodologia, Desagregacion, Marcos, Variable, Observaciones, hfVersion.Value)
            lblVersionBriefMuestral.InnerText = "Versión " + hfVersion.Value.ToString

            If Convert.ToInt16(hfidbrief.Value) > 0 Then
                Dim oEnviarCorreo As New EnviarCorreo
                Try
                    If String.IsNullOrEmpty(hfidbrief.Value) Then
                        Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
                    End If
                    If flag = True Then
                        oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/BriefEstadistica.aspx?idBrief=" & hfidbrief.Value)
                    Else
                        oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/BriefEstadisticaCambio.aspx?idBrief=" & hfidbrief.Value)
                    End If
                    'Dim page As Page = DirectCast(Context.Handler, Page)
                    'ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
                Catch ex As Exception
                    ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
                End Try
                hfFlag.Value = 1
            Else
                hfFlag.Value = 0
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub CargarInfo(ByVal IdBrief As Int64)
        Try
            Limpiar()
            Dim oBrief As New BriefDisenoMuestral
            Dim info = oBrief.DevolverxID(IdBrief)
            If info Is Nothing Then
                ShowNotification("No hay información del Brief de Diseño Muestral indicado", ShowNotifications.ErrorNotification)
                Exit Sub
            Else
                hfidbrief.Value = info.id
                hfidpropuesta.Value = info.Propuestaid
                txtCapacidad.Text = info.Capacidad
                txtDesagregacion.Text = info.NivelesDesagregacion
                txtFecha.Text = info.Fecha
                txtMarcos.Text = info.PosiblesMarcos
                txtMetodologia.Text = info.Metodologia
                txtObjetivos.Text = info.Objetivo
                txtPoblacion.Text = info.Poblacion
                txtVariables.Text = info.Variable
                txtGerente.Text = info.Gerente
                txtObservaciones.Text = info.Observaciones
                hfVersion.Value = info.NoVersion
                If info.NoVersion > 0 Then
                    lblVersionBriefMuestral.InnerText = "Versión " + info.NoVersion.ToString
                End If
                accordion2.Visible = True
                accordion1.Visible = False
                CargarVersionesBDM(hfidpropuesta.Value)
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Sub Eliminar(ByVal idBrief As Int64)
        Try
            Dim oBrief As New BriefDisenoMuestral
            oBrief.Eliminar(idBrief)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Dim log As New LogEjecucion
        log.Guardar(17, iddoc, Now(), Session("IDUsuario"), idaccion)

    End Sub

    Sub Prellenado()
        If hfidpropuesta.Value = "" Then
            Exit Sub
        End If
        txtFecha.Text = Date.UtcNow.AddHours(-5).Date
        Me.txtFecha.Enabled = False
        Me.txtObjetivos.Focus()
        Dim oBrief As New Brief
        Dim oPropuesta As New Propuesta
        Dim oUsuario As New US.Usuarios
        Dim info = oBrief.DevolverxID(oPropuesta.DevolverxID(hfidpropuesta.Value).Brief)
        Dim gerente = oUsuario.UsuarioGet(info.GerenteCuentas)
        Me.txtObjetivos.Text = info.Objetivos
        Me.txtPoblacion.Text = info.TargetGroup
        Me.txtCapacidad.Text = info.Presupuestos
        Me.txtMetodologia.Text = info.Metodologia
        Me.txtGerente.Text = gerente.Nombres + " " + gerente.Apellidos
    End Sub

    Sub CargarVersionesBDM(ByVal PropuestaId As Int64)
        Dim oBrief As New BriefDisenoMuestral
        pnlVersionesBDM.Visible = True
        UPanelVersionesBDM.Update()

        Try
            Dim listabriefs = oBrief.ObtenerBriefXPresupuesto(PropuestaId).ToList().OrderByDescending(Function(x) x.id)
            gvVersionesBDM.DataSource = listabriefs
            gvVersionesBDM.DataBind()
        Catch ex As Exception

        End Try
    End Sub

    Sub LimpiarDetalleVersionBDM()
        'txtBCPObservacionesVer.Text = ""
        'txtBCPIncentivosEspVer.Text = ""
        'txtBCPBDDEspVer.Text = ""
        'txtBCPProductoEspVer.Text = ""
        'txtBCPEspReclutamientoVer.Text = ""
        'lblBCPEspProductoVer.Text = ""
        'lblBCPMaterialEvalVer.Text = ""
        'txtBCPObsProductoVer.Text = ""
    End Sub

    Sub llenarDetalleVersionBDM(ByVal brief As ES_BriefDisenoMuestral)
        txtDetFecha.text = brief.Fecha
        txtDetObjetivo.text = brief.Objetivo
        txtDetPoblacion.text = brief.Poblacion
        txtDetCapacidad.text = brief.Capacidad
        txtDetMetodologia.text = brief.Metodologia
        txtDetNivelesDesagregacion.text = brief.NivelesDesagregacion
        txtDetPosiblesMarcos.text = brief.PosiblesMarcos
        txtDetVariable.text = brief.Variable
        txtDetObservaciones.text = brief.Observaciones
        txtDetNoVersion.text = brief.NoVersion
    End Sub


    Sub SubirDatosBDM(ByVal ent As ES_BriefDisenoMuestral, ByVal ent2 As ES_BriefDisenoMuestral)
        lblVersionA.Text = "Versión " + ent.NoVersion.ToString
        lblVersionB.Text = "Versión " + ent2.NoVersion.ToString

        If ent.Fecha <> ent2.Fecha Then
            txtCompFecha1.CssClass = "cambioVersion"
            txtCompFecha2.CssClass = "cambioVersion1"
        Else
            txtCompFecha1.CssClass = "versionIgual"
            txtCompFecha2.CssClass = "versionIgual"
        End If
        txtCompFecha1.Text = ent.Fecha
        txtCompFecha2.Text = ent2.Fecha

        If ent.Objetivo <> ent2.Objetivo Then
            txtCompObjetivo1.CssClass = "cambioVersion"
            txtCompObjetivo2.CssClass = "cambioVersion1"
        Else
            txtCompObjetivo1.CssClass = "versionIgual"
            txtCompObjetivo2.CssClass = "versionIgual"
        End If
        txtCompObjetivo1.Text = ent.Objetivo
        txtCompObjetivo2.Text = ent2.Objetivo

        If ent.Poblacion <> ent2.Poblacion Then
            txtCompPoblacion1.CssClass = "cambioVersion"
            txtCompPoblacion2.CssClass = "cambioVersion1"
        Else
            txtCompPoblacion1.CssClass = "versionIgual"
            txtCompPoblacion2.CssClass = "versionIgual"
        End If
        txtCompPoblacion1.Text = ent.Poblacion
        txtCompPoblacion2.Text = ent2.Poblacion

        If ent.Capacidad <> ent2.Capacidad Then
            txtCompCapacidad1.CssClass = "cambioVersion"
            txtCompCapacidad2.CssClass = "cambioVersion1"
        Else
            txtCompCapacidad1.CssClass = "versionIgual"
            txtCompCapacidad2.CssClass = "versionIgual"
        End If
        txtCompCapacidad1.Text = ent.Capacidad
        txtCompCapacidad2.Text = ent2.Capacidad

        If ent.Metodologia <> ent2.Metodologia Then
            txtCompMetodologia1.CssClass = "cambioVersion"
            txtCompMetodologia2.CssClass = "cambioVersion1"
        Else
            txtCompMetodologia1.CssClass = "versionIgual"
            txtCompMetodologia2.CssClass = "versionIgual"
        End If
        txtCompMetodologia1.Text = ent.Metodologia
        txtCompMetodologia2.Text = ent2.Metodologia

        If ent.NivelesDesagregacion <> ent2.NivelesDesagregacion Then
            txtCompNivelesDesagregacion1.CssClass = "cambioVersion"
            txtCompNivelesDesagregacion2.CssClass = "cambioVersion1"
        Else
            txtCompNivelesDesagregacion1.CssClass = "versionIgual"
            txtCompNivelesDesagregacion2.CssClass = "versionIgual"
        End If
        txtCompNivelesDesagregacion1.Text = ent.NivelesDesagregacion
        txtCompNivelesDesagregacion2.Text = ent2.NivelesDesagregacion

        If ent.PosiblesMarcos <> ent2.PosiblesMarcos Then
            txtCompPosiblesMarcos1.CssClass = "cambioVersion"
            txtCompPosiblesMarcos2.CssClass = "cambioVersion1"
        Else
            txtCompPosiblesMarcos1.CssClass = "versionIgual"
            txtCompPosiblesMarcos2.CssClass = "versionIgual"
        End If
        txtCompPosiblesMarcos1.Text = ent.PosiblesMarcos
        txtCompPosiblesMarcos2.Text = ent2.PosiblesMarcos

        If ent.Variable <> ent2.Variable Then
            txtCompVariable1.CssClass = "cambioVersion"
            txtCompVariable2.CssClass = "cambioVersion1"
        Else
            txtCompVariable1.CssClass = "versionIgual"
            txtCompVariable2.CssClass = "versionIgual"
        End If
        txtCompVariable1.Text = ent.Variable
        txtCompVariable2.Text = ent2.Variable

        If ent.Observaciones <> ent2.Observaciones Then
            txtCompObservaciones1.CssClass = "cambioVersion"
            txtCompObservaciones2.CssClass = "cambioVersion1"
        Else
            txtCompObservaciones1.CssClass = "versionIgual"
            txtCompObservaciones2.CssClass = "versionIgual"
        End If
        txtCompObservaciones1.Text = ent.Observaciones
        txtCompObservaciones2.Text = ent2.Observaciones
    End Sub
#End Region
End Class