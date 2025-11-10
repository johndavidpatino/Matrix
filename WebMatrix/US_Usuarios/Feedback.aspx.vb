Imports WebMatrix.Util
Imports CoreProject

Public Class Feedback
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim oFeedback As New CoreProject.Feedback
            ddlAsunto.DataSource = oFeedback.ObtenerAsunto
            ddlAsunto.DataValueField = "id"
            ddlAsunto.DataTextField = "tipo"
            ddlAsunto.DataBind()
            ddlAsunto.Items.Insert(0, New ListItem("--Seleccione--", -1))
        End If
    End Sub

#Region "Eventos del Control"
    Protected Sub btnguardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnguardar.Click
        Dim oFeedback As New CoreProject.Feedback
        Dim daConstante As New Constantes
        Dim correosNotificar As String
        Try
            correosNotificar = daConstante.obtenerXId(Constantes.EConstantes.correosNotificarFeedbackMatrix).Valor
            oFeedback.EnviarFeedBack(Session("IDUsuario").ToString, Me.ddlAsunto.SelectedValue, Me.txtTextoMensaje.Text)
            Dim usuario As String = ""
            Dim o As New US.Usuarios
            usuario = o.UsuarioGet(Session("IDUsuario").ToString).Usuario
            Dim ec As New EnviarCorreo
            Dim li As New List(Of String)
            li.AddRange(correosNotificar.Split(",").ToList())
            ec.sendMail(li, "Feedback desde Matrix", Me.txtTextoMensaje.Text & ". - Enviado por " & usuario)
            ShowNotification("Mensaje enviado correctamente", ShowNotifications.InfoNotification)
            Me.pnlFeedback.Visible = False
            Me.Pnlenviado.Visible = True
            log(0, 2)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
        End Try
    End Sub
#End Region

#Region "Metodos"
    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(30, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
#End Region
End Class