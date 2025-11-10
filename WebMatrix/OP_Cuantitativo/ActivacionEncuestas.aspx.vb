Imports CoreProject
Imports WebMatrix.Util

Public Class ActivacionEncuestas
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _TrabajoId As Int64
    Public Property TrabajoId() As Int64
        Get
            Return _TrabajoId
        End Get
        Set(ByVal value As Int64)
            _TrabajoId = value
        End Set
    End Property
#End Region

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.QueryString("TrabajoId") Is Nothing Then
                hfidTrabajo.Value = Request.QueryString("TrabajoId").ToString
            End If
        End If
        CargarEncuestas(hfidTrabajo.Value)
    End Sub

    Protected Sub btnActivar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnActivar.Click
        Dim oAnulacionEncuestas As New CoreProject.AnulacionEncuestas
        Dim oActivacionEncuestas As New CoreProject.ActivacionEncuestas
        Dim Valor As Int16 = oAnulacionEncuestas.ExisteEncuestaAnulada(hfidTrabajo.Value, txtEncuesta.Text)
        If Valor = 1 Then
            oActivacionEncuestas.Eliminar(txtEncuesta.Text, hfidTrabajo.Value)
            oActivacionEncuestas.ActualizarGestionCampo(hfidTrabajo.Value, txtEncuesta.Text, txtObservacion.Text, CDec(Session("IDUsuario").ToString))
            log(txtEncuesta.Text, 3)
            limpiar()
            ShowNotification("Encuesta Activada", ShowNotifications.InfoNotification)
            CargarEncuestas(hfidTrabajo.Value)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            Exit Sub
        End If
        ShowNotification("Encuesta no Encontrada En listado de Anuladas", ShowNotifications.ErrorNotification)
    End Sub

    Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnVolver.Click
        Response.Redirect("ConsultaTrabajos.aspx")
    End Sub

    Private Sub ActivacionEncuestas_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(126, UsuarioID) = False Then
            Response.Redirect("ConsultaTrabajos.aspx")
        End If
    End Sub
#End Region

#Region "Metodos"
    Sub limpiar()
        txtEncuesta.Text = String.Empty
        txtObservacion.Text = String.Empty
    End Sub

    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(37, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub CargarEncuestas(ByVal TrabajoIdV As Int64)
        Dim oAnulacionEncuestas As New CoreProject.AnulacionEncuestas
        Dim listgv = (From list In oAnulacionEncuestas.EncuestasAnuladasListXTrabajo(TrabajoIdV)
                  Select id = list.id, NumeroEncuesta = list.NumeroEncuesta, Observacion = list.Observacion, Fecha = list.Fecha,
                  Usuario = list.TH_Personas.Nombres & " " & list.TH_Personas.Apellidos, Unidad = list.US_Unidades.Unidad)
        Me.gvEncuestas.DataSource = listgv
        Me.gvEncuestas.DataBind()
    End Sub
#End Region

End Class