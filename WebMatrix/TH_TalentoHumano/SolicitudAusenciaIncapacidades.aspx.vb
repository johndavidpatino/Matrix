Imports CoreProject
Imports WebMatrix.Util
Imports System.IO
Imports System.Resources
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb
Imports System.Data

Public Class TH_SolicitudAusenciaIncapacidades
    Inherits System.Web.UI.Page

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Page.Form.Attributes.Add("enctype", "multipart/form-data")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarTiposSolicitudesAusencia()
            CargarAprobador()
        End If
        'Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        'smanager.RegisterPostBackControl(Me.btnGenerarPDF)
    End Sub

    Sub CargarTiposSolicitudesAusencia()
        Dim o As New TH_Ausencia.DAL
        ddlTipoSolicitud.DataSource = o.TiposSolicitudesAusencia
        ddlTipoSolicitud.DataTextField = "Tipo"
        ddlTipoSolicitud.DataValueField = "id"
        ddlTipoSolicitud.DataBind()
        ddlTipoSolicitud.Items.Insert(0, New ListItem With {.Value = "0", .Text = "-- Seleccione --"})
    End Sub

    Sub LoadHistorialAusencia()
        Dim o As New TH_Ausencia.DAL
        gvHistorialAusencia.DataSource = o.RegistrosAusencia(Nothing, Session("IDUsuario").ToString, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        gvHistorialAusencia.DataBind()
    End Sub

    Private Sub lbMenuHistorial_Click(sender As Object, e As EventArgs) Handles lbMenuHistorial.Click
        pnlHistorialAusencia.Visible = True
        pnlNuevaSolicitud.Visible = False
        pnlBeneficiosPendientes.Visible = False
        pnlAprobaciones.Visible = False
        LoadHistorialAusencia()
        liMenu3.Attributes.Add("class", "active")
        liMenu2.Attributes.Remove("class")
        liMenu1.Attributes.Remove("class")
        liMenu4.Attributes.Remove("class")
    End Sub


    Private Sub lbMenuNew_Click(sender As Object, e As EventArgs) Handles lbMenuNew.Click
        pnlHistorialAusencia.Visible = False
        pnlNuevaSolicitud.Visible = True
        pnlBeneficiosPendientes.Visible = False
        pnlAprobaciones.Visible = False
        liMenu1.Attributes.Add("class", "active")
        liMenu2.Attributes.Remove("class")
        liMenu3.Attributes.Remove("class")
        liMenu4.Attributes.Remove("class")
    End Sub

    Private Sub lbMenuPendientes_Click(sender As Object, e As EventArgs) Handles lbMenuPendientes.Click
        liMenu2.Attributes.Add("class", "active")
        liMenu1.Attributes.Remove("class")
        liMenu3.Attributes.Remove("class")
        liMenu4.Attributes.Remove("class")
        pnlHistorialAusencia.Visible = False
        pnlNuevaSolicitud.Visible = False
        pnlAprobaciones.Visible = False
        pnlBeneficiosPendientes.Visible = True
        Dim o As New TH_Ausencia.DAL
        gvBeneficiosPendientes.DataSource = o.ListadoBeneficiosPendientes(Session("IDUsuario").ToString)
        gvBeneficiosPendientes.DataBind()
    End Sub

    Private Sub lbMenuAprobaciones_Click(sender As Object, e As EventArgs) Handles lbMenuAprobaciones.Click
        liMenu4.Attributes.Add("class", "active")
        liMenu1.Attributes.Remove("class")
        liMenu2.Attributes.Remove("class")
        liMenu3.Attributes.Remove("class")
        pnlHistorialAusencia.Visible = False
        pnlNuevaSolicitud.Visible = False
        pnlAprobaciones.Visible = True
        pnlBeneficiosPendientes.Visible = False
        CargarAprobacionesPendientes()
    End Sub

    Sub CargarAprobacionesPendientes()
        Dim o As New TH_Ausencia.DAL
        gvAprobacionesPendientes.DataSource = o.RegistrosAusencia(Nothing, Nothing, Nothing, Nothing, Nothing, 1, Nothing, Nothing, Nothing, Nothing, Session("IDUsuario").ToString)
        gvAprobacionesPendientes.DataBind()
    End Sub

    Private Sub btnSubmitAusencia_Click(sender As Object, e As EventArgs) Handles btnSubmitAusencia.Click
        Dim o As New TH_Ausencia.DAL
        Dim empleado As New CoreProject.Empleados
        Dim tipoSalario As Short

        tipoSalario = empleado.obtenerPorIdentificacion(Session("IDUsuario").ToString).TipoSalario
        Dim diasCalculados = o.CalculoDias(txtFechaInicioSolicitud.Text, txtFechaFinSolicitud.Text, esSabadoDiaLaboral(tipoSalario), Session("IDUsuario").ToString)(0)

        If Not (IsDate(txtFechaInicioSolicitud.Text)) Then
            AlertJS("Debe diligenciar la fecha de inicio")
            txtFechaInicioSolicitud.Focus()
            Exit Sub
        End If
        If Not (IsDate(txtFechaFinSolicitud.Text)) Then
            AlertJS("Debe diligenciar la fecha de fin")
            txtFechaFinSolicitud.Focus()
            Exit Sub
        End If
        If ddlTipoSolicitud.SelectedValue = -1 Then
            AlertJS("Seleccione el tipo de ausencia")
            ddlTipoSolicitud.Focus()
            Exit Sub
        End If
        If CDate(txtFechaInicioSolicitud.Text).Date > CDate(txtFechaFinSolicitud.Text).Date Then
            AlertJS("El rango de fechas solicitado no es válido")
            Exit Sub
        End If
        Dim validacion = o.ValidarSolicitudAusencia(Session("IDUsuario").ToString, txtFechaInicioSolicitud.Text, txtFechaFinSolicitud.Text, ddlTipoSolicitud.SelectedValue)(0)
        If validacion.Result = 0 Then
            AlertJS(validacion.MessageResult)
            Exit Sub
        End If
        Dim ent As New CoreProject.TH_SolicitudAusencia
        ent.FInicio = txtFechaInicioSolicitud.Text
        ent.FFin = txtFechaFinSolicitud.Text
        ent.DiasCalendario = diasCalculados.DiasCalendario
        ent.DiasLaborales = diasCalculados.DiasLaborales
        ent.Estado = 1
        ent.Tipo = ddlTipoSolicitud.SelectedValue
        ent.FechaRegistro = Date.UtcNow.AddHours(-5)
        ent.RegistradoPor = Session("IDUsuario").ToString
        ent.idEmpleado = Session("IDUsuario").ToString
        ent.ObservacionesSolicitud = txtObservaciones.Text
        ent.AprobadoPor = ddlAprobador.SelectedValue
        o.AddSolicitudAusencia(ent)
        EnvioCorreo(ent.id)
        AlertJS("Se ha registrado su solicitud")
        Limpiar()
    End Sub

    Private Sub txtFechaInicioSolicitud_TextChanged(sender As Object, e As EventArgs) Handles txtFechaInicioSolicitud.TextChanged
        CalculoDias()
    End Sub

    Private Sub txtFechaFinSolicitud_TextChanged(sender As Object, e As EventArgs) Handles txtFechaFinSolicitud.TextChanged
        CalculoDias()
    End Sub
    Sub CalculoDias()
        If IsDate(txtFechaInicioSolicitud.Text) And IsDate(txtFechaFinSolicitud.Text) Then
            Dim o As New TH_Ausencia.DAL
            Dim empleado As New CoreProject.Empleados
            Dim tipoSalario As Short

            tipoSalario = empleado.obtenerPorIdentificacion(Session("IDUsuario").ToString).TipoSalario
            Dim info = o.CalculoDias(txtFechaInicioSolicitud.Text, txtFechaFinSolicitud.Text, esSabadoDiaLaboral(tipoSalario), Session("IDUsuario").ToString)(0)

            txtDiasCalendario.Text = info.DiasCalendario
            txtDiasLaborales.Text = info.DiasLaborales
            If ddlTipoSolicitud.SelectedValue = 10 Then
                If info.DiasCalendario >= 3 Then
                    lblAvisoIncapacidad3dias.Visible = True
                Else
                    lblAvisoIncapacidad3dias.Visible = False
                End If
            End If
        End If
    End Sub

    Private Sub ddlTipoSolicitud_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoSolicitud.SelectedIndexChanged
        If ddlTipoSolicitud.SelectedValue = 10 Then
            btnSubmitAusencia.Visible = False
            pnlIncapacidad.Visible = True
            lblAvisoIncapacidad.Visible = True
        Else
            btnSubmitAusencia.Visible = True
            pnlIncapacidad.Visible = False
            lblAvisoIncapacidad.Visible = False
            lblAvisoIncapacidad3dias.Visible = False
        End If
    End Sub

    Private Sub ddlIncapacidadClaseAusencia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlIncapacidadClaseAusencia.SelectedIndexChanged
        If ddlIncapacidadClaseAusencia.SelectedValue = 3 Then
            txtFechaAccidenteTrabajo.Enabled = True
        Else
            txtFechaAccidenteTrabajo.Enabled = False
            txtFechaAccidenteTrabajo.Text = ""
        End If
        If Not (ddlIncapacidadClaseAusencia.SelectedValue = 4) Then
            ddlIncapacidadSOAT.SelectedValue = 1
            ddlIncapacidadSOAT.Enabled = False
        Else
            ddlIncapacidadSOAT.Enabled = True
        End If
    End Sub



    Private Sub btnIncapacidadSubmit_Click(sender As Object, e As EventArgs) Handles btnIncapacidadSubmit.Click
        If Not (IsDate(txtFechaInicioSolicitud.Text)) Then
            AlertJS("Debe diligenciar la fecha de inicio")
            txtFechaInicioSolicitud.Focus()
            Exit Sub
        End If
        If Not (IsDate(txtFechaFinSolicitud.Text)) Then
            AlertJS("Debe diligenciar la fecha de fin")
            txtFechaFinSolicitud.Focus()
            Exit Sub
        End If
        If ddlTipoSolicitud.SelectedValue = -1 Then
            AlertJS("Seleccione el tipo de ausencia")
            ddlTipoSolicitud.Focus()
            Exit Sub
        End If
        If CDate(txtFechaInicioSolicitud.Text).Date > CDate(txtFechaFinSolicitud.Text).Date Then
            AlertJS("El rango de fechas solicitado no es válido")
            Exit Sub
        End If
        If ddlIncapacidadEntidad.SelectedValue = 0 Then
            AlertJS("Seleccione la entidad que lo atendió")
            ddlIncapacidadEntidad.Focus()
            Exit Sub
        End If
        If ddlIncapacidadClaseAusencia.SelectedValue = 0 Then
            AlertJS("Seleccione la Clase de ausencia")
            ddlIncapacidadClaseAusencia.Focus()
            Exit Sub
        End If
        If ddlIncapacidadClaseAusencia.SelectedValue = 4 And Not (IsDate(txtFechaAccidenteTrabajo.Text)) Then
            AlertJS("Debe escribir la fecha del accidente de trabajo")
            txtFechaAccidenteTrabajo.Focus()
            Exit Sub
        End If
        If lblDX.Text = "" Or txtCIE.Text = "" Then
            AlertJS("Debe escribir el CIE")
            txtCIE.Focus()
            Exit Sub
        End If
        Dim o As New TH_Ausencia.DAL
        Dim validacion = o.ValidarSolicitudAusencia(Session("IDUsuario").ToString, txtFechaInicioSolicitud.Text, txtFechaFinSolicitud.Text, ddlTipoSolicitud.SelectedValue)(0)
        If validacion.Result = 0 Then
            AlertJS(validacion.MessageResult)
            Exit Sub
        End If
        Dim ent As New CoreProject.TH_SolicitudAusencia
        ent.FInicio = txtFechaInicioSolicitud.Text
        ent.FFin = txtFechaFinSolicitud.Text
        ent.DiasCalendario = txtDiasCalendario.Text
        ent.DiasLaborales = txtDiasLaborales.Text
        ent.Estado = 1
        ent.Tipo = ddlTipoSolicitud.SelectedValue
        ent.FechaRegistro = Date.UtcNow.AddHours(-5)
        ent.RegistradoPor = Session("IDUsuario").ToString
        ent.idEmpleado = Session("IDUsuario").ToString
        ent.ObservacionesSolicitud = txtObservaciones.Text
        o.AddSolicitudAusencia(ent)

        Dim entI As New TH_Ausencia_Incapacidades
        entI.idSolicitudAusencia = ent.id
        entI.CIE = txtCIE.Text
        entI.ClaseAusencia = ddlIncapacidadClaseAusencia.SelectedValue
        entI.Comentarios = txtIncapacidadObservaciones.Text
        entI.EntidadConsulta = ddlIncapacidadEntidad.SelectedValue
        If IsDate(txtFechaAccidenteTrabajo.Text) Then entI.FechaAccidenteTrabajo = txtFechaAccidenteTrabajo.Text
        entI.IPS = txtIncapacidadIPS.Text
        entI.RegistroMedico = txtIncapacidadNoRegistroMedico.Text
        entI.SOAT = ddlIncapacidadSOAT.SelectedValue
        entI.TipoIncapacidad = ddlIncapacidadTipo.SelectedValue
        ent.AprobadoPor = ddlAprobador.SelectedValue
        o.AddIncapacidad(entI)
        EnvioCorreo(ent.id)
        AlertJS("Se ha registrado su solicitud")
        Limpiar()
        LimpiarIncapacidad()
    End Sub

    Sub Limpiar()
        txtFechaInicioSolicitud.Text = ""
        txtFechaFinSolicitud.Text = ""
        txtDiasCalendario.Text = ""
        txtDiasLaborales.Text = ""
        ddlTipoSolicitud.SelectedIndex = 0
        txtObservaciones.Text = ""
    End Sub
    Sub LimpiarIncapacidad()
        txtCIE.Text = ""
        ddlIncapacidadClaseAusencia.SelectedIndex = 0
        txtIncapacidadObservaciones.Text = ""
        ddlIncapacidadEntidad.SelectedIndex = 0
        txtFechaAccidenteTrabajo.Text = ""
        txtIncapacidadIPS.Text = ""
        txtIncapacidadNoRegistroMedico.Text = ""
        ddlIncapacidadSOAT.SelectedIndex = 0
        ddlIncapacidadTipo.SelectedIndex = 0
    End Sub
    Sub CargarAprobador()
        Dim o As New TH_Ausencia.DAL
        ddlAprobador.DataSource = o.ObtenerAprobadores
        ddlAprobador.DataTextField = "NombreCompleto"
        ddlAprobador.DataValueField = "id"
        ddlAprobador.DataBind()
        ddlAprobador.SelectedValue = o.ObtenerJefeInmediato(Session("IDUsuario").ToString)
    End Sub

    Sub EnvioCorreo(ByVal idSolicitud As Int64)
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EnvioAprobacionAusencia.aspx?&idSolicitud=" & idSolicitud.ToString)
    End Sub

    Private Sub gvAprobacionesPendientes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvAprobacionesPendientes.RowCommand
        Dim ids As Int64 = Int64.Parse(Me.gvAprobacionesPendientes.DataKeys(CInt(e.CommandArgument))("ID"))
        Dim o As New TH_Ausencia.DAL
        Dim ent = o.GetSolicitudAusencia(ids)
        If e.CommandName = "Aprobar" Then
            ent.FechaAprobacion = Date.UtcNow.AddHours(-5)
            ent.Estado = 5
            EnvioCorreo(ent.id)
        End If
        If e.CommandName = "Rechazar" Then
            ent.FechaAprobacion = Date.UtcNow.AddHours(-5)
            ent.Estado = 10
            EnvioCorreoRechazo(ent.id)
        End If
        o.SaveChangesSolicitud(ent)
        CargarAprobacionesPendientes()
    End Sub

    Private Sub txtCIE_TextChanged(sender As Object, e As EventArgs) Handles txtCIE.TextChanged
        Dim o As New TH_Ausencia.DAL
        Dim info = o.ListadoDXCIE(txtCIE.Text)
        If Not info.Count = 0 Then
            lblDX.Text = info(0).DX
        Else
            lblDX.Text = ""
        End If

    End Sub

    Sub EnvioCorreoRechazo(ByVal idSolicitud As Int64)
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EnvioDefinicionAusencia.aspx?&idSolicitud=" & idSolicitud.ToString)
    End Sub

    Private Sub gvHistorialAusencia_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvHistorialAusencia.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If IsDate(e.Row.Cells(0).Text) Then
                If (CDate(e.Row.Cells(0).Text).Date > Now.Date) And Not (e.Row.Cells(5).Text = "Anulado") Then
                    e.Row.Cells(8).Visible = True
                Else
                    e.Row.Cells(8).Visible = False
                End If
            End If
        End If
    End Sub

    Private Sub gvHistorialAusencia_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvHistorialAusencia.RowCommand
        Dim ids As Int64 = Int64.Parse(Me.gvHistorialAusencia.DataKeys(CInt(e.CommandArgument))("ID"))
        Dim o As New TH_Ausencia.DAL
        If e.CommandName = "Anular" Then
            o.AnularSolicitud(ids)
        End If
        LoadHistorialAusencia()
    End Sub

    Function esSabadoDiaLaboral(tipoSalario As CoreProject.Empleados.ETipoSalario) As Boolean
        Return tipoSalario = CoreProject.Empleados.ETipoSalario.Variable
    End Function
End Class
