Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo


Public Class NuevoTrabajoCoordCampoOpMail
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("idTrabajo") IsNot Nothing Then
            Me.hplLink.NavigateUrl = WebMatrix.Util.obtenerUrlRaiz() & "/RE_GT/AsignacionCoordinador.aspx"
            GenerarHtml()
        Else
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal EstudioId As Long)
        Dim oAnuncio As New CoreProject.AnuncioAprobacion
        lblAsunto.Text = lblAsunto.Text

    End Sub
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)



        Dim estudio As Int64 = Int64.Parse(Request.QueryString("idTrabajo").ToString())
        Dim oTrabajo As New Trabajo
        Dim info = oTrabajo.obtenerXId(estudio)

        Me.lblTrabajo.Text = info.NombreTrabajo

        Dim oUsuarios As New US.Usuarios

        Dim listUsuarios = oUsuarios.UsuariosxRol(ListaRoles.CoordinadorNacional_RegionaldeCampo)

        Dim destinatarios As New List(Of String)

        For Each li As Usuarios_Result In listUsuarios
            destinatarios.Add(li.Email)
        Next

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"

        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("Cesar.Verano@ipsos.com")
        
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class