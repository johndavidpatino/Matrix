Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class Emails_CambioEstadoTrabajo
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim script As String = "setTimeout('window.close();',50)"
        'Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idTrabajo") IsNot Nothing Then
            Dim estudio As Int64 = Int64.Parse(Request.QueryString("idTrabajo").ToString())
            CargarElemento(estudio)
            GenerarHtml()
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
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
        lblUnidad.Text = info.Unidad
        Dim oTrabajo As New Trabajo
        Dim infotrabajo = oTrabajo.ObtenerTrabajo(TrabajoId)
        lblAsunto.Text = "Matrix: " & oTrabajo.ObtenerEstado(infotrabajo.Estado) & " trabajo " & info.JobBook & " " & info.NombreTrabajo
        lblEstadoTrabajo.Text = oTrabajo.ObtenerEstado(infotrabajo.Estado)
        lblID.Text = TrabajoId

    End Sub

    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString

        Dim destinatarios As New List(Of String)

        Dim trabajo As Int64 = Int64.Parse(Request.QueryString("idTrabajo").ToString())

        Dim oUsuarios As New Reportes.DestinatariosCorreos
        Dim listUsuarios = oUsuarios.CorreosPorTrabajoGP(trabajo)

        For Each li As US_Correos_Result In listUsuarios
            destinatarios.Add(li.CORREO)
        Next
        Dim o As New DestinatariosCorreos
        For i As Integer = 0 To o.DestinatariosCreacionyEspecificacionesTrabajo(trabajo).Count - 1
            destinatarios.Add(o.DestinatariosCreacionyEspecificacionesTrabajo(trabajo).Item(i).CORREO)
        Next
        
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class