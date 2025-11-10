Imports CoreProject
Imports WebMatrix.Util

Public Class AnulacionEncuestas
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

    Private _UnidadId As Int32
    Public Property UnidadId() As Int32
        Get
            Return _UnidadId
        End Get
        Set(ByVal value As Int32)
            _UnidadId = value
        End Set
    End Property
#End Region
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Request.QueryString("TrabajoId") Is Nothing Then
                hfidTrabajo.Value = Request.QueryString("TrabajoId").ToString
                CargarEncuestasAnuladas(hfidTrabajo.Value)
            End If
            If Not Request.QueryString("IdUnidad") Is Nothing Then
                hfidUnidad.Value = Request.QueryString("IdUnidad").ToString
            End If
            Try
                Me.txtNombreTrabajo.Text = Session("NombreTrabajo").ToString
            Catch ex As Exception
            End Try
        End If
    End Sub
    Protected Sub btnAnular_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAnular.Click
        Dim oAnulacionEncuestas As New CoreProject.AnulacionEncuestas
        Dim Valor As Int16 = oAnulacionEncuestas.ExisteEncuestaAnulada(hfidTrabajo.Value, txtEncuesta.Text)
        If Valor = 1 Then
            ShowNotification("Esta encuesta ya ha sido anulada previamente", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        Valor = oAnulacionEncuestas.ExisteEncuestaAnuladaGC(hfidTrabajo.Value, txtEncuesta.Text)
        If Valor = 0 Then
            ShowNotification("Este número de encuesta no está digitado aún", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        oAnulacionEncuestas.grabar(hfidTrabajo.Value, txtEncuesta.Text, txtObservacion.Text, DateTime.UtcNow.AddHours(-5), Session("IDUsuario"), hfidUnidad.Value)
        oAnulacionEncuestas.AnularEncuestaGC(hfidTrabajo.Value, txtEncuesta.Text, txtObservacion.Text)
        Me.txtEncuesta.Text = ""
        Me.txtObservacion.Text = ""
        ShowNotification("Encuesta anulada", ShowNotifications.InfoNotification)
        CargarEncuestasAnuladas(hfidTrabajo.Value)
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("TraficoEncuestas.aspx?UnidadId=" & hfidUnidad.Value)
    End Sub
#End Region
#Region "Metodos"
    Sub CargarEncuestasAnuladas(ByVal TrabajoIdV As Int64)
        Dim oAnulacionEncuestas As New CoreProject.Reportes.AvanceCampo
        Dim listgv = (From list In oAnulacionEncuestas.ObtenerEncuestasAnuladas(TrabajoIdV)
                  Select id = list.id, NumeroEncuesta = list.NumeroEncuesta, Observacion = list.Observacion, Fecha = list.FechaAnulacion,
                  Usuario = list.AnuladoPor, Unidad = list.Unidad)
        Me.gvAnuladas.DataSource = listgv
        Me.gvAnuladas.DataBind()
    End Sub
#End Region


End Class