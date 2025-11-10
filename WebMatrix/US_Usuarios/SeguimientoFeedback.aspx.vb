Imports CoreProject
Imports WebMatrix.Util
Imports WebMatrix.EnviarCorreo

Public Class SeguimientoFeedback
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

    Private _idUsuario As Int64
    Public Property idUsuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
        End Set
    End Property




#End Region

#Region "Metodos"

    Sub CargarMensajesPendientes()
        Dim oFeedback As New CoreProject.Feedback
        Dim list = (From lst In oFeedback.ObtenerFeedbackPendientes
                    Select id = lst.id, fecha = lst.Fecha, tipo = lst.CORE_TipoRetroalimentacion.Tipo, usuario = lst.US_Usuarios.Nombres & " " & lst.US_Usuarios.Apellidos,
                        mensaje = lst.Mensaje)
        gvPendientes.DataSource = list.ToList
        gvPendientes.DataBind()
    End Sub

    Sub CargarMensajesResueltos()
        Dim oFeedback As New CoreProject.Feedback
        Dim list = (From lst In oFeedback.ObtenerFeedbackResueltos
                    Select id = lst.id, fecha = lst.Fecha, tipo = lst.CORE_TipoRetroalimentacion.Tipo, usuario = lst.US_Usuarios.Nombres & " " & lst.US_Usuarios.Apellidos,
                        mensaje = lst.Mensaje, respuesta = lst.Respuesta, usuarioresponde = lst.US_Usuarios1.Nombres & " " & lst.US_Usuarios1.Apellidos,
                        fechasolucion = lst.FechaSolucion)
        gvResueltos.DataSource = list.ToList
        gvResueltos.DataBind()
    End Sub

    Sub CargarFeedback(ByVal idf As Int64)
        Dim oFeedback As New CoreProject.Feedback
        Dim ent As New CORE_Retroalimentacion
        ent = oFeedback.ObtenerFeedbackXId(hfIdRespuesta.Value)
        Me.lblMensaje.Text = ent.Mensaje
    End Sub
#End Region

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            CargarMensajesPendientes()
            CargarMensajesResueltos()
        End If
    End Sub

    Sub Limpiar()
        Me.lblMensaje.Text = String.Empty
        Me.txtRespuesta.Text = String.Empty
        Me.chbSolucionado.Checked = False
    End Sub


    Protected Sub btnCancelar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelar.Click
        Limpiar()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub gvPendientes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvPendientes.PageIndexChanging
        gvPendientes.PageIndex = e.NewPageIndex
        CargarMensajesPendientes()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Private Sub gvPendientes_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPendientes.RowCommand
        If e.CommandName = "Responder" Then
            hfIdRespuesta.Value = Int64.Parse(Me.gvPendientes.DataKeys(CInt(e.CommandArgument))("Id"))
            CargarFeedback(hfIdRespuesta.Value)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

#End Region

    Protected Sub btnGrabar_Click(sender As Object, e As EventArgs) Handles btnGrabar.Click
        Dim oFeedback As New CoreProject.Feedback
        Dim ent As New CORE_Retroalimentacion
        ent = oFeedback.ObtenerFeedbackXId(hfIdRespuesta.Value)
        ent.Respuesta = txtRespuesta.Text
        ent.Solucionado = chbSolucionado.Checked
        ent.FechaSolucion = Date.UtcNow.AddHours(-5)
        ent.UsuarioResponde = Session("IDUsuario").ToString
        oFeedback.ActualizarFeedback(ent)
        CargarMensajesPendientes()
        CargarMensajesResueltos()
        Limpiar()
        Try
            Dim script As String = "window.open('../Emails/RespuestaFeedback.aspx?idFeedBack=" & hfIdRespuesta.Value & "','cal','width=400,height=250,left=270,top=180')"
            Dim page As Page = DirectCast(Context.Handler, Page)
            ScriptManager.RegisterStartupScript(page, GetType(Page), "Anuncio", script, True)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try

        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
End Class