Imports System.IO
Imports CoreProject

Public Class EnvioAprobacionVacaciones
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim script As String = "setTimeout('window.close();',50)"
        Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idSolicitud") IsNot Nothing Then
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
        lblHWHId.Text = "Código Solicitud: " & idSolicitud
        lblTipoAusencia.Text = info.Tipo
        lblFini.Text = info.FInicio.Value
        lblFFin.Text = info.FFin.Value
        lblDiaRegreso.Text = o.ObtenerDiaRegreso(info.FFin)

        gvPeriodos.DataSource = o.GetCausacionPeriodosVAcaciones(idSolicitud)
        gvPeriodos.DataBind()

        Dim u As New US.Usuarios
        Dim destinatarios As New List(Of String)

        destinatarios.Add(u.obtenerUsuarioXId(info2.idEmpleado).Email)
        destinatarios.Add("Matrix@ipsos.com")
        destinatarios.Add("recursoshumanoscolombia@ipsos.com")

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString

        contenido = contenido & "<br/>" & GridviewHtml()

        contenido = contenido & "<br/><br/><br/><p>Este correo fue generado automáticamente y su contenido está sujeto a revisión y validación por parte de Recursos Humanos</p>"
        contenido = contenido & "<br/>"

        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub

    Function GridviewHtml() As String
        If gvPeriodos.Rows.Count = 0 Then
            Return String.Empty
        End If
        Dim htmlTable As StringBuilder = New StringBuilder()
        Dim numberRows As Integer = gvPeriodos.Rows.Count - 1
        Dim numberCols As Integer = gvPeriodos.Columns.Count - 1
        Dim color As String = "#"
        htmlTable.Append("<table border=""1"" style=""color: " & color & "555; width:100%;font:12px/15px Arial, Helvetica, sans-serif;   border:1px solid #d3d3d3;    background:" & color & "fefefe;  margin:5% auto 0;/*Propiedad de centrado, borrar para dejarlo normal*/    -moz-border-radius:5px;    -webkit-border-radius:5px;	-ms-border-radius:5px;    border-radius:5px;    -moz-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);    -webkit-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);	-ms-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);	-o-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;text-align:center"">")

        ' Loop through each encabezado
        htmlTable.Append("<thead style=""font-weight:bold;"">")
        For j As Integer = 0 To numberCols
            ' Now loop through each row, getting the current columns value
            ' from each row
            htmlTable.Append("<th>")
            htmlTable.Append(gvPeriodos.HeaderRow.Cells(j).Text)
            htmlTable.Append("</th>")
        Next
        htmlTable.Append("</thead>")
        ' Loop through each column first
        For i As Integer = 0 To numberRows
            htmlTable.Append("<tr>")

            ' Now loop through each row, getting the current columns value
            ' from each row
            For j As Integer = 0 To numberCols
                htmlTable.Append("<td>")
                htmlTable.Append(gvPeriodos.Rows(i).Cells(j).Text)
                htmlTable.Append("</td>")
            Next

            htmlTable.Append("</tr>")
        Next

        htmlTable.Append("</table>")

        ' To get the value of the StringBuilder, call ToString()
        Dim resultHtml = htmlTable.ToString()
        Return resultHtml
    End Function

#End Region


End Class