Imports System.IO
Imports CoreProject

Public Class EnvioAprobacionAusencia
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idSolicitud") IsNot Nothing Then
            Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/TH_TalentoHumano/SolicitudAusencia.aspx?idHWH=" + Request.QueryString("idHWH")
            GenerarHtml()
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If
    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Dim idSolicitud As Int64 = Int64.Parse(Request.QueryString("idSolicitud").ToString())

        Dim o As New TH_Ausencia.DAL
        Dim oPersonas As New Personas
        Dim info = o.RegistrosAusencia(idSolicitud, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)(0)
        Dim info2 = o.GetSolicitudAusencia(idSolicitud)
        lblHWHId.Text = "Código Solicitud: " & info.ID
        lblUsuario.Text = info.EMPLEADO
        lblTipoAusencia.Text = info.Tipo
        lblFini.Text = info.FInicio.Value
        lblFFin.Text = info.FFin.Value

        Dim u As New US.Usuarios
        Dim destinatarios As New List(Of String)

		If info2.Estado = TH_Ausencia.DAL.ETipo.Graduacion Then
			Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/TH_TalentoHumano/GestionAusenciaRRHH.aspx?idHWH=" + Request.QueryString("idHWH")
		Else
			Dim JefeInmediato = Nothing
            JefeInmediato = u.obtenerUsuarioXId(info2.AprobadoPor)
            destinatarios.Add(JefeInmediato.Email)
        End If

		If info2.Tipo = TH_Ausencia.DAL.ETipo.IncapacidadMedica Then
			destinatarios.Add("alejandra.gonzalez@ipsos.com")
		End If
		destinatarios.Add("Matrix@ipsos.com")
        destinatarios.Add("recursoshumanoscolombia@ipsos.com")


        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub

#End Region


End Class