Imports CoreProject
Imports WebMatrix.Util

Public Class MuestraTrabajos
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _IDUsuario As Int64
    Public Property IDUsuario() As Int64
        Get
            Return _IDUsuario
        End Get
        Set(ByVal value As Int64)
            _IDUsuario = value
        End Set
    End Property
#End Region

#Region "Funciones y Métodos"
    Sub CargarMuestra(ByVal TrabajoId As Int64)
        Dim oCoorCampo As New CoordinacionCampo
        Dim listaMuestra = (From lmuestra In oCoorCampo.ObtenerMuestraxEstudioList(TrabajoId)
                            Select idMuestra = lmuestra.Id, departamento = lmuestra.C_Divipola.DivDeptoNombre, ciudad = lmuestra.C_Divipola.DivMuniNombre,
                            cantidad = lmuestra.Cantidad, FechaInicio = lmuestra.FechaInicio, FechaFin = lmuestra.FechaFin).OrderBy(Function(x) x.ciudad)
        If listaMuestra.Count = 0 Then
            Exit Sub
        End If
        'If Request.QueryString("COE") IsNot Nothing Then
        '    Me.gvMuestra.Columns(5).Visible = False
        '    Me.pnlEstimacion.Visible = False
        'Else
        '    Me.pnlEstimacion.Visible = True
        '    Me.gvMuestra.Columns(5).Visible = True
        'End If

        Me.pnlEstimacion.Visible = True
        Me.gvMuestra.Columns(5).Visible = True

        gvMuestra.DataSource = listaMuestra.ToList
        gvMuestra.DataBind()
    End Sub

    Sub EnviarEmail()
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            If String.IsNullOrEmpty(hfidTrabajo.Value) Then
                Throw New Exception("Debe elegir un estudio o guardarlo antes de enviar un anuncio de aprobación")
            End If
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/NuevoTrabajoCoordCampo.aspx?idTrabajo=" & hfidTrabajo.Value)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
        'Response.Redirect("../OP_Cuantitativo/Trabajos.aspx")
    End Sub
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("TrabajoId") IsNot Nothing Then
                Dim TrabajoId As Int64 = Int64.Parse(Request.QueryString("TrabajoId").ToString())
                hfidTrabajo.Value = TrabajoId
                CargarMuestra(TrabajoId)
                Try
                    Me.txtNombreTrabajo.Text = Session("NombreTrabajo").ToString
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub
    Private Sub gvMuestra_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvMuestra.PageIndexChanging
        gvMuestra.PageIndex = e.NewPageIndex
        CargarMuestra(hfidTrabajo.Value)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvMuestra_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMuestra.RowCommand
        If e.CommandName = "Actualizar" Then
            Dim idMuestra As Int64 = Int64.Parse(Me.gvMuestra.DataKeys(CInt(e.CommandArgument))("idMuestra"))
            Dim oCoorCampo As New CoordinacionCampo
            Dim ent As New OP_MuestraTrabajos
            ent = oCoorCampo.ObtenerMuestraxId(idMuestra)
            If IsDate(DirectCast(Me.gvMuestra.Rows(e.CommandArgument).FindControl("txtFInicio"), TextBox).Text) Then
                ent.FechaInicio = DirectCast(Me.gvMuestra.Rows(e.CommandArgument).FindControl("txtFInicio"), TextBox).Text
            End If
            If IsDate(DirectCast(Me.gvMuestra.Rows(e.CommandArgument).FindControl("txtFFin"), TextBox).Text) Then
                ent.FechaFin = DirectCast(Me.gvMuestra.Rows(e.CommandArgument).FindControl("txtFFin"), TextBox).Text
            End If
            oCoorCampo.GuardarMuestraXEstudio(ent)
            'oCoorCampo.GuardarFechasGeneralesTrabajo(hfidTrabajo.Value)
            Dim oPlaneacion As New PlaneacionProduccion
            oPlaneacion.ActualizarFechasCiudad(idMuestra, Me.chbDias.Items(0).Selected, Me.chbDias.Items(1).Selected, Me.chbDias.Items(2).Selected, Me.chbDias.Items(3).Selected, Me.chbDias.Items(4).Selected, Me.chbDias.Items(5).Selected, Me.chbDias.Items(6).Selected, chbFestivosExcluir.Checked)
            ShowNotification("Planeación automática ajustada correctamente", ShowNotifications.InfoNotification)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End If
    End Sub
    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        If Request.QueryString("COE") IsNot Nothing Then
            Response.Redirect("../OP_Cuantitativo/Trabajos.aspx")
        Else
            Response.Redirect("../RP_Reportes/TrabajosPorGerencia.aspx")
        End If

    End Sub

#End Region

    Private Sub btnCambiarGeneral_Click(sender As Object, e As System.EventArgs) Handles btnCambiarGeneral.Click
        If Not (IsDate(txtFechaInicioGeneral.Text)) Or Not (IsDate(txtFechaFinGeneral.Text)) Then
            ShowNotification("Requiere digitar las fechas generales", ShowNotifications.ErrorNotification)
            Exit Sub
            ActivateAccordion("0", EffectActivateAccordion.NoEffect)
        End If
        Dim o As New CoordinacionCampo
        o.GuardarFechasGenerales(hfidTrabajo.Value, txtFechaInicioGeneral.Text, txtFechaFinGeneral.Text)
        Dim op As New PlaneacionProduccion
        op.AgregarEstimacionAutomatica(hfidTrabajo.Value, Session("IDUsuario").ToString, Me.chbDias.Items(0).Selected, Me.chbDias.Items(1).Selected, Me.chbDias.Items(2).Selected, Me.chbDias.Items(3).Selected, Me.chbDias.Items(4).Selected, Me.chbDias.Items(5).Selected, Me.chbDias.Items(6).Selected, chbFestivosExcluir.Checked)
        CargarMuestra(hfidTrabajo.Value)
    End Sub
End Class