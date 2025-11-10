Imports CoreProject
Imports WebMatrix.Util

Public Class VariablesControl
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _proyectoId As Int64
    Public Property proyectoId() As Int64
        Get
            Return _proyectoId
        End Get
        Set(ByVal value As Int64)
            _proyectoId = value
        End Set
    End Property

    Private _trabajoId As Int64
    Public Property trabajoId() As Int64
        Get
            Return _trabajoId
        End Get
        Set(ByVal value As Int64)
            _trabajoId = value
        End Set
    End Property

    Private _modalidad As String
    Public Property modalidad() As String
        Get
            Return _modalidad
        End Get
        Set(ByVal value As String)
            _modalidad = value
        End Set
    End Property
#End Region
    Private Shared prevPage As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("modal") IsNot Nothing Then
            modalidad = Request.QueryString("modal")
            hfmodalidad.Value = modalidad
            Session("modal") = hfmodalidad.Value
        Else
            ShowNotification("No se encontró modalidad de calificación", ShowNotifications.ErrorNotification)
            prevPage = Request.Url.ToString().Replace(Request.Url.LocalPath.ToString() + Request.Url.Query.ToString(), "/Home/Default.aspx")
            Response.Redirect(prevPage)
        End If

        If (Not IsPostBack) Then
            If Not Request.UrlReferrer Is Nothing Then
                prevPage = Request.UrlReferrer.ToString()
            Else
                prevPage = Request.Url.ToString().Replace(Request.Url.LocalPath.ToString() + Request.Url.Query.ToString(), "")
                If hfmodalidad.Value.ToUpper = "COE" Then
                    prevPage = prevPage + "/PY_Proyectos/Home.aspx"
                ElseIf hfmodalidad.Value.ToUpper = "GP" Then
                    prevPage = prevPage + "/RE_GT/RecoleccionDeDatos.aspx"
                Else
                    prevPage = prevPage + "/Home/Default.aspx"
                End If
            End If

            If Request.QueryString("idTr") IsNot Nothing Then
                Long.TryParse(Request.QueryString("idTr"), trabajoId)
                hfIdTrabajo.Value = trabajoId
                Session("TrabajoId") = hfIdTrabajo.Value
            Else
                ShowNotification("No se encontró el trabajo", ShowNotifications.ErrorNotification)
                Response.Redirect(prevPage)
            End If

            Dim oTrabajo As New Trabajo
            Dim oeTrabajo As New PY_Trabajo0
            oeTrabajo = oTrabajo.ObtenerTrabajo(hfIdTrabajo.Value)

            Long.TryParse(oeTrabajo.ProyectoId, proyectoId)
            hfIdProyecto.Value = proyectoId
            Session("ProyectoId") = hfIdProyecto.Value

            Dim oProyecto As New Proyecto
            Dim oeProyecto As New PY_Proyectos_Get_Result
            oeProyecto = oProyecto.obtenerXId(hfIdProyecto.Value)
            Dim tipoProyecto = oeProyecto.TipoProyectoId

            If tipoProyecto = 1 Then
                pnlSeguridad.Visible = True
                pnlObtencion.Visible = True
                pnlObjetivo.Visible = True
                pnlAplicacion.Visible = True
                pnlDistribucion.Visible = True
                pnlCumplimiento.Visible = True
            ElseIf tipoProyecto = 2 Then
                pnlSeguridad.Visible = False
                pnlObtencion.Visible = False
                pnlObjetivo.Visible = True
                pnlAplicacion.Visible = True
                pnlDistribucion.Visible = True
                pnlCumplimiento.Visible = False
            End If

            txtTrabajoId.Text = oeTrabajo.id
            txtJobBook.Text = oeTrabajo.JobBook
            txtTrabajoNombre.Text = oeTrabajo.NombreTrabajo

            Dim oUsuario As New US.Usuarios
            Dim oeUsuarios As New US_Usuarios

            If hfmodalidad.Value.ToUpper = "COE" Then
                pnlCoe.Visible = True
                pnlGerente.Visible = False
                If oeTrabajo.COE Is Nothing Then
                    ShowNotification("No está asignado el COE", ShowNotifications.ErrorNotification)
                    Response.Redirect(prevPage)
                End If
                hfIdCOE.Value = oeTrabajo.COE
                hfIdGerente.Value = Nothing
                oeUsuarios = oUsuario.obtenerUsuarioXId(hfIdCOE.Value)
                txtCoe.Text = oeUsuarios.Nombres + " " + oeUsuarios.Apellidos
            ElseIf hfmodalidad.Value.ToUpper = "GP" Then
                pnlCoe.Visible = False
                pnlGerente.Visible = True
                If oeProyecto.GerenteProyectos Is Nothing Then
                    ShowNotification("No está asignado el Gerente de Proyecto", ShowNotifications.ErrorNotification)
                    Response.Redirect(prevPage)
                End If
                hfIdGerente.Value = oeProyecto.GerenteProyectos
                hfIdCOE.Value = Nothing
                oeUsuarios = oUsuario.obtenerUsuarioXId(hfIdGerente.Value)
                txtGerente.Text = oeUsuarios.Nombres + " " + oeUsuarios.Apellidos
            End If

            validar()
        End If
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        guardar()
        validar()
    End Sub

    Sub Limpiar()
        rblSeguridad.ClearSelection()
        txtSeguridad.Text = ""
        rblObtencion.ClearSelection()
        txtObtencion.Text = ""
        rblObjetivo.ClearSelection()
        txtObjetivo.Text = ""
        rblAplicacion.ClearSelection()
        txtAplicacion.Text = ""
        rblDistribucion.ClearSelection()
        txtDistribucion.Text = ""
        rblCumplimiento.ClearSelection()
        txtCumplimiento.Text = ""
    End Sub

    Sub llenarCampos(ByVal variable As PY_Variables_Control)
        rblSeguridad.SelectedValue = variable.cumpleSeguridad
        txtSeguridad.Text = variable.obsSeguridad
        rblObtencion.SelectedValue = variable.cumpleObtencion
        txtObtencion.Text = variable.obsObtencion
        rblObjetivo.SelectedValue = variable.cumpleObjetivo
        txtObjetivo.Text = variable.obsObjetivo
        rblAplicacion.SelectedValue = variable.cumpleAplicacion
        txtAplicacion.Text = variable.obsAplicacion
        rblDistribucion.SelectedValue = variable.cumpleDistribucion
        txtDistribucion.Text = variable.obsDistribucion
        rblCumplimiento.SelectedValue = variable.cumpleCumplimiento
        txtCumplimiento.Text = variable.obsCumplimiento
    End Sub

    Sub habilitarCampos(ByVal valor As Boolean)
        Dim valor2 = If(valor, False, True)
        rblSeguridad.Enabled = valor
        txtSeguridad.ReadOnly = valor2
        rblObtencion.Enabled = valor
        txtObtencion.ReadOnly = valor2
        rblObjetivo.Enabled = valor
        txtObjetivo.ReadOnly = valor2
        rblAplicacion.Enabled = valor
        txtAplicacion.ReadOnly = valor2
        rblDistribucion.Enabled = valor
        txtDistribucion.ReadOnly = valor2
        rblCumplimiento.Enabled = valor
        txtCumplimiento.ReadOnly = valor2

        btnGuardar.Visible = valor
    End Sub

    Sub validar()
        Dim o As New Proyecto
        Dim oVariable As New PY_Variables_Control
        Try
            oVariable = o.ObtenerVariableControlxTrabajoxMod(hfIdTrabajo.Value, hfmodalidad.Value).FirstOrDefault
            If Not oVariable Is Nothing Then
                Limpiar()
                llenarCampos(oVariable)
                habilitarCampos(False)
            Else
                habilitarCampos(True)
            End If
        Catch ex As Exception
            ShowNotification("No se puedo consultar Variables de Control. Error: " + ex.Message, ShowNotifications.ErrorNotification)
            Exit Sub
        End Try
    End Sub

    Sub guardar()
        Dim o As New Proyecto
        Dim oVariable As New PY_Variables_Control

        Try
            oVariable.idTrabajo = hfIdTrabajo.Value

            If hfmodalidad.Value.ToUpper = "GP" Then
                oVariable.idEvaluado = hfIdGerente.Value
            ElseIf hfmodalidad.Value.ToUpper = "COE" Then
                oVariable.idEvaluado = hfIdCOE.Value
            End If

            oVariable.tipoEvaluado = hfmodalidad.Value
            oVariable.cumpleSeguridad = rblSeguridad.SelectedValue
            oVariable.obsSeguridad = txtSeguridad.Text
            oVariable.cumpleObtencion = rblObtencion.SelectedValue
            oVariable.obsObtencion = txtObtencion.Text
            oVariable.cumpleObjetivo = rblObjetivo.SelectedValue
            oVariable.obsObjetivo = txtObjetivo.Text
            oVariable.cumpleAplicacion = rblAplicacion.SelectedValue
            oVariable.obsAplicacion = txtAplicacion.Text
            oVariable.cumpleDistribucion = rblDistribucion.SelectedValue
            oVariable.obsDistribucion = txtDistribucion.Text
            oVariable.cumpleCumplimiento = rblCumplimiento.SelectedValue
            oVariable.obsCumplimiento = txtCumplimiento.Text
            oVariable.usuario = Session("IDUsuario")
            oVariable.fechaCreacion = Date.UtcNow.AddHours(-5)

            o.guardarVariableControl(oVariable)

            ShowNotification("Evaluación Guardada", ShowNotifications.ErrorNotification)
        Catch ex As Exception
            ShowNotification("No se puedo guardar la Variable de Control. Error: " + ex.Message, ShowNotifications.ErrorNotification)
            Exit Sub
        End Try
    End Sub
End Class