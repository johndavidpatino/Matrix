Imports WebMatrix.Util
Imports CoreProject

Public Class ListadoBrief
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarGruposUnidad()
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
            If permisos.VerificarPermisoUsuario(16, UsuarioID) = False Then
                Response.Redirect("../Home.aspx")
            End If
        End If
    End Sub


#End Region

#Region "Metodos"
    Sub CargarGruposUnidad()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGrupoUnidades.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(1)
        ddlGrupoUnidades.DataValueField = "id"
        ddlGrupoUnidades.DataTextField = "GrupoUnidad"
        ddlGrupoUnidades.DataBind()
        ddlGrupoUnidades.Items.Insert(0, New ListItem With {.Text = "--Seleccione--", .Value = -1})
    End Sub

    Sub CargarBrief(grupounidad As Int32)
        Dim o As New Reportes.Directores
        Me.gvDatos.DataSource = o.ObtenerListadoBrief(grupounidad)
        Me.gvDatos.DataBind()
    End Sub
#End Region


    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If Me.ddlGrupoUnidades.SelectedValue = 0 Or Me.ddlGrupoUnidades.SelectedValue = -1 Then
            ShowNotification("Seleccione primero la unidad antes de continuar", ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        CargarBrief(ddlGrupoUnidades.SelectedValue)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "SI"
                    Dim Viable As Boolean = True
                    Dim idBrief As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    Dim oBrief As New Brief
                    oBrief.ActualizarViabilidad(idBrief, Viable, Session("IDUsuario").ToString)
                    CargarBrief(ddlGrupoUnidades.SelectedValue)
                Case "NO"
                    Dim Viable As Boolean = False
                    Dim idBrief As Int64 = Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument))("Id"))
                    hfidbriefnoviab.Value = idBrief
                    'Dim oBrief As New Brief
                    'oBrief.ActualizarViabilidad(idBrief, Viable, Session("IDUsuario").ToString)
                    'CargarBrief(ddlGrupoUnidades.SelectedValue)
            End Select
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End Try
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnGuardarRazonViabilidad_Click(sender As Object, e As System.EventArgs) Handles btnGuardarRazonViabilidad.Click
        If Me.txtRazonNoViabilidad.Text = "" Then
            ShowNotification("No podrá continuar hasta que no escriba la razón de no viabilidad", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        Dim Viable As Boolean = False
        Dim idBrief As Int64 = hfidbriefnoviab.Value
        Dim oBrief As New Brief
        oBrief.ActualizarViabilidad(idBrief, Viable, Session("IDUsuario").ToString)
        oBrief.ActualizarRazonViabilidad(idBrief, txtRazonNoViabilidad.Text)
        CargarBrief(ddlGrupoUnidades.SelectedValue)
        EnviarEmail(idBrief)
    End Sub
    Public Sub EnviarEmail(ByVal idBrief As Int32)
        Try
            ''Info Brief
            Dim oBrief As New Brief
            Dim infobrief = oBrief.DevolverxID(idBrief)

            ''Info Propuesta
            Dim oPropuesta As New Propuesta
            ''Dim infopropuesta = oPropuesta.DevolverxBriefID(infobrief.Id)
            Dim otexto As New StringBuilder
            Dim destinatarios As New List(Of String)
            'otexto.AppendLine("<br/> <p> <img alt='' src='../Images/logo-titulo.png' width='217px' height='52px' /></p>")
            otexto.AppendLine("<br/>Bogotá, " & Day(Now.Date).ToString() & " de " & MonthName(Now.Month) & " de " & Now.Year)
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Doctor")
            otexto.AppendLine("<br/><b>" & infobrief.Nombre & "</b>")
            otexto.AppendLine("<br/>La Ciudad")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Estimado(a) Señor(a):")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Agradecemos su amable invitación para presentar una propuesta sobre ")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Desafortunadamente nuestros actuales compromisos con clientes permanentes comprometen los recursos disponibles en diseño, ejecución y análisis, y nos impiden, en estos momentos, atender a su amable solicitud.")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Confiando en un futuro construir una conjunta y benéfica relación,")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Reciban un atento saludo,")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>")
            otexto.AppendLine("<br/>Nombre Gerente de Cuentas")
            otexto.AppendLine("<br/>CCCCCCC")
            otexto.AppendLine("<br/>Ipsos-Napoleón Franco")
            otexto.AppendLine("<br/>Calle 74 No. 11 – 81 Piso 5")
            otexto.AppendLine("<br/>Tels: 3769400 – Fax 525")
            otexto.AppendLine("<br/>E-mail: nfranco@ipsos.com.co - www.ipsos.com.co")

            Dim oEnviarCorreo As New EnviarCorreo
            oEnviarCorreo.sendMail(destinatarios, "prueba", otexto.ToString)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class