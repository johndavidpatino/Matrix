Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class PreEntregaTrabajo
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idTrabajo") IsNot Nothing Then
            Dim estudio As Int64 = Int64.Parse(Request.QueryString("idTrabajo").ToString())
            CargarElemento(estudio)
            GenerarHtml()
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal TrabajoId As Long)
        Dim oPreEntrega As New CoreProject.PreEntregaTrabajo
        Dim info = oPreEntrega.DevolverPreEntregaxTrabajo(TrabajoId)
        lblNombreTrabajo.Text = info.NombreTrabajo
        lblJobBook.Text = info.JobBook
        lblMetodologia.Text = info.MetNombre
        lblFechaInicio.Text = info.FechaTentativaInicioCampo
        lblFechaFin.Text = info.FechaTentativaFinalizacion
        lblUnidad.Text = info.Unidad
        lblMuestra.Text = info.Muestra
        lblAsunto.Text = lblAsunto.Text & " " & info.JobBook & " " & info.NombreTrabajo

    End Sub

    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        Dim oUsuarios As New US.Usuarios
        Dim listUsuarios = oUsuarios.UsuariosxUnidad(40)

        Dim destinatarios As New List(Of String)

        For Each li As Usuarios_Result In listUsuarios
            destinatarios.Add(li.Email)
        Next
        
        destinatarios.Add("sistemamatrixtempo@gmail.com")
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class