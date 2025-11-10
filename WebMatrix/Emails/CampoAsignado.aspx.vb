Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class CampoAsignado
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.QueryString("idTrabajo") IsNot Nothing Then
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

        lblAsunto.Text = "Matrix." & " " & info.JobBook & " ID: " & info.id & " " & info.NombreTrabajo & " - Campo Asignado"

        Me.lblTrabajo.Text = info.NombreTrabajo

        Dim oProyecto As New Proyecto
        Dim infop = oProyecto.obtenerXId(info.ProyectoId)

        Dim oUsuarios As New US.Usuarios
        Dim destinatarios As New List(Of String)

        lblCOE.Text = oUsuarios.UsuarioGet(info.COE).Nombres & " " & oUsuarios.UsuarioGet(info.COE).Apellidos

        Me.pnlBody.RenderControl(hw)
        destinatarios.Add(oUsuarios.UsuarioGet(info.COE).Email)
        destinatarios.Add(oUsuarios.UsuarioGet(infop.GerenteProyectos).Email)
        destinatarios.AddRange(oUsuarios.UsuariosxGrupoUnidadXrol(20, 9).Select(Function(x) x.Email).ToList)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>"


        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class