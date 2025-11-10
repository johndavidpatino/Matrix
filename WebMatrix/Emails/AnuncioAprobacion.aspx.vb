Imports CoreProject
Imports System.IO
Imports WebMatrix.EnviarCorreo

Public Class AnuncioAprobacion
    Inherits System.Web.UI.Page
#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim script As String = "setTimeout('window.close();',50)"
        'Dim page As Page = DirectCast(Context.Handler, Page)
        If Request.QueryString("idEstudio") IsNot Nothing Then
            Dim estudio As Int64 = Int64.Parse(Request.QueryString("idEstudio").ToString())
            CargarElemento(estudio)
            CargarPresupuestosXEstudio(estudio)
            GenerarHtml()
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        Else
            'ScriptManager.RegisterStartupScript(page, GetType(Page), "cerrar", script, True)
        End If

    End Sub
#End Region

#Region "Funciones y Metodos"
    Sub CargarElemento(ByVal EstudioId As Long)
        Dim oAnuncio As New CoreProject.AnuncioAprobacion
        Dim info = oAnuncio.DevolverxEstudio(EstudioId)
        lblCliente.Text = info.RazonSocial
        lblNombreEstudio.Text = info.Nombre
        lblSituacion.Text = info.Antecedentes
        lblComplicación.Text = info.Objetivos
        lblQuestion.Text = info.ActionStandars
        lblRecomendaciones.Text = info.Metodologia
        lblFechaInicio.Text = info.FechaInicio
        lblFechaInicioCampo.Text = info.FechaInicioCampo
        lblFechaFin.Text = info.FechaTerminacion
        lblFechaAprobacion.Text = info.FechaAprob
        lblValorEstudio.Text = FormatCurrency(info.Valor, 0, TriState.True, TriState.False, TriState.True)
        lblObservaciones.Text = info.Observaciones
        lblNumeroEstudio.Text = info.JobBook
        lblFormaPago.Text = "Anticipo " & info.Anticipo & "% - Saldo " & info.Saldo & "%"
        lblPlazoPago.Text = info.Plazo & " dias"
        lblAsunto.Text = lblAsunto.Text & " " & info.JobBook & " " & info.Nombre

        Dim oEstudio As New CoreProject.Estudio
        Dim infoEstudio = oEstudio.ObtenerXID(EstudioId)
        Dim oPropuesta As New CoreProject.Propuesta
        Dim infoPropuesta = oPropuesta.DevolverxID(infoEstudio.PropuestaId)
        Dim oBrief As New CoreProject.Brief
        Dim infoBrief = oBrief.DevolverxID(infoPropuesta.Brief)
        Dim oUsuarios As New US.Usuarios
        Me.lblGerenteCuentas.Text = oUsuarios.UsuarioGet(infoBrief.GerenteCuentas).Nombres & " " & oUsuarios.UsuarioGet(infoBrief.GerenteCuentas).Apellidos
    End Sub

    Sub CargarPresupuestosXEstudio(ByVal estudioID As Int64)
        Dim oPresupuesto As New Presupuesto
        Dim x = oPresupuesto.ObtenerMuestraParaAnuncioAprobacion(estudioID)

        gvPresupuestosAsignadosXEstudio.DataSource = x
        gvPresupuestosAsignadosXEstudio.DataBind()
    End Sub

    Function GridviewHtml() As String
        If gvPresupuestosAsignadosXEstudio.Rows.Count = 0 Then
            Return String.Empty
        End If
        Dim htmlTable As StringBuilder = New StringBuilder()
        Dim numberRows As Integer = gvPresupuestosAsignadosXEstudio.Rows.Count - 1
        Dim numberCols As Integer = gvPresupuestosAsignadosXEstudio.Columns.Count - 1
        Dim color As String = "#"
        htmlTable.Append("<table border=""1"" style=""color: " & color & "555; width:100%;font:12px/15px Arial, Helvetica, sans-serif;   border:1px solid #d3d3d3;    background:" & color & "fefefe;  margin:5% auto 0;/*Propiedad de centrado, borrar para dejarlo normal*/    -moz-border-radius:5px;    -webkit-border-radius:5px;	-ms-border-radius:5px;    border-radius:5px;    -moz-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);    -webkit-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);	-ms-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);	-o-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;text-align:center"">")

        ' Loop through each encabezado
        htmlTable.Append("<thead style=""font-weight:bold;"">")
        For j As Integer = 0 To numberCols
            ' Now loop through each row, getting the current columns value
            ' from each row
            htmlTable.Append("<th>")
            htmlTable.Append(gvPresupuestosAsignadosXEstudio.HeaderRow.Cells(j).Text)
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
                htmlTable.Append(gvPresupuestosAsignadosXEstudio.Rows(i).Cells(j).Text)
                htmlTable.Append("</td>")
            Next

            htmlTable.Append("</tr>")
        Next

        htmlTable.Append("</table>")

        ' To get the value of the StringBuilder, call ToString()
        Dim resultHtml = htmlTable.ToString()
        Return resultHtml
    End Function
    Sub GenerarHtml()
        Dim sb As New StringBuilder
        Dim tx As New StringWriter(sb)
        Dim hw As New HtmlTextWriter(tx)

        Me.pnlBody.RenderControl(hw)

        Dim contenido As String = sb.ToString
        contenido = contenido & "<br/>" & GridviewHtml()
        Dim destinatarios As New List(Of String)
        Dim o As New DestinatariosCorreos
        Dim estudio As Int64 = Int64.Parse(Request.QueryString("idEstudio").ToString())
        For i As Integer = 0 To o.DestinatariosAnuncioAprobacion(estudio).Count - 1
            destinatarios.Add(o.DestinatariosAnuncioAprobacion(estudio).Item(i).CORREO)
        Next
        If Request.QueryString("idGerenteProyectos") IsNot Nothing Then
            destinatarios.Clear()
            Dim oUsuarios As New US.Usuarios
            destinatarios.Add(oUsuarios.UsuarioGet(Request.QueryString("idGerenteProyectos")).Email)
        End If
        'destinatarios.Add("John.Patino@ipsos.com")
        'destinatarios.Add("Cesar.Verano@ipsos.com")
        'destinatarios.Add("sistemamatrixtempo@gmail.com")
        Dim oEnviarCorreo As New EnviarCorreo
        oEnviarCorreo.sendMail(destinatarios, Me.lblAsunto.Text, contenido)
    End Sub
#End Region
End Class