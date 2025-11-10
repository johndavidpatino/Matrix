Imports WebMatrix.Util
Imports CoreProject
Imports ClosedXML.Excel

Public Class ControlCostos
    Inherits System.Web.UI.Page

    Dim _ControlCostos As New IQ.ControlCostos()
    Dim _Presupuestado As Decimal = 0
    Dim _Presupuestado2 As Decimal = 0
    Dim _Autorizado As Decimal = 0
    Dim _Ejecutado As Decimal = 0
    Dim _DiferenciaPA As Decimal = 0
    Dim _Porcentaje1 As Decimal = 0
    Dim _DiferenciaPE As Decimal = 0
    Dim _Porcentaje2 As Decimal = 0
    Dim _TotalProduccion As Decimal = 0
    Dim _DiferenciaPP As Decimal = 0
    Dim _Porcentaje3 As Decimal = 0
    Dim _TotalViaticos As Decimal = 0
    Dim _TotalHoteles As Decimal = 0
    Dim _TotalTransporte As Decimal = 0
    Dim _TotalHoras As Integer = 0

 _
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Try

                Dim par As New IQ_Parametros()
                par.IdPropuesta = CLng(Request.QueryString("idPropuesta"))
                par.ParAlternativa = CInt(Request.QueryString("Alternativa"))
                par.MetCodigo = CInt(Request.QueryString("Metodologia"))
                par.ParNacional = CInt(Request.QueryString("Nacional"))

                Dim D As IQ_ObtenerDescPresupuesto_Result
                D = _ControlCostos.ObtenerDatosPresupuesto(par)
                lblFase.Text = D.FASE.ToUpper()
                lblMetodologia.Text = D.METODOLOGIA.ToUpper()
                lblTecnica.Text = D.TECNICA.ToUpper()
                lblIdPropuesta.Text = par.IdPropuesta.ToString()
                lblMuestra.Text = D.Muestra.ToString()

                gvControlCostos.DataSource = _ControlCostos.ObtenerCostos(par, 1)
                gvControlCostos.DataBind()

                gvDetallesOperaciones.DataSource = _ControlCostos.ObtenerCostos(par, 2)
                gvDetallesOperaciones.DataBind()

                gvViaticos.DataSource = _ControlCostos.ObtenerViaticos(par)
                gvViaticos.DataBind()


                Dim preg As IQ_Preguntas
                preg = _ControlCostos.ObtenerPreguntasPresupuesto(par)
                If (preg IsNot Nothing) Then
                    lblAbiertas.Text = preg.PregAbiertas.ToString()
                    lblAbiertasMult.Text = preg.PregAbiertasMultiples.ToString()
                    lblCerradas.Text = preg.PregCerradas.ToString()
                    lblCerradasMult.Text = preg.PregCerradasMultiples.ToString()
                    lblOtros.Text = preg.PregOtras.ToString()
                    lblDemograficos.Text = preg.PregDemograficos.ToString()
                End If

                If Request.QueryString("o_3453oioioioo_1133") IsNot Nothing Then
                    If Request.QueryString("o_3453oioioioo_1133") = 0 Then
                        Me.TabPanel2.Visible = True
                    Else
                        Me.TabPanel2.Visible = False
                    End If
                End If

            Catch ex As Exception
                ShowNotification(ex.Message, WebMatrix.ShowNotifications.ErrorNotification)
            End Try

        End If

    End Sub

    Protected Sub LKVOLVER_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LKVOLVER.Click
        Dim par As IQ_Parametros
        par = CType(Session("PARAMETROS"), IQ_Parametros)


        If Request.QueryString("Back") IsNot Nothing Then
            Response.Redirect("PresupuestosAprobados.aspx?OPT=1")
        Else
            If Request.QueryString("ACCION") IsNot Nothing Then
                Response.Redirect("Cap_Principal.aspx?IdPropuesta=" & par.IdPropuesta.ToString() & "&Alternativa=" & par.ParAlternativa.ToString() & "&ACCION=" & Request.QueryString("ACCION"))
            Else
                Response.Redirect("Cap_Principal.aspx?IdPropuesta=" & par.IdPropuesta.ToString() & "&Alternativa=" & par.ParAlternativa.ToString())
            End If
        End If



    End Sub

    Protected Sub gvControlCostos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvControlCostos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            _Presupuestado = _Presupuestado + CDec(e.Row.Cells(2).Text)
            e.Row.Cells(2).CssClass = "RightAlign"

            '_Autorizado = _Autorizado + CDec(e.Row.Cells(3).Text)
            '_DiferenciaPA = _DiferenciaPA + CDec(e.Row.Cells(4).Text)
            '_Porcentaje1 = _Porcentaje1 + CDec(e.Row.Cells(5).Text)
            '_TotalProduccion = _TotalProduccion + CDec(e.Row.Cells(6).Text)
            '_DiferenciaPP = _DiferenciaPP + CDec(e.Row.Cells(7).Text)
            '_Porcentaje3 = _Porcentaje3 + CDec(e.Row.Cells(8).Text)

            '_Ejecutado = _Ejecutado + CDec(e.Row.Cells(9).Text)
            '_DiferenciaPE = _DiferenciaPE + CDec(e.Row.Cells(10).Text)
            '_Porcentaje2 = _Porcentaje2 + CDec(e.Row.Cells(11).Text)
            e.Row.Cells(11).CssClass = "RightAlign"

            '_Ejecutado = _Ejecutado + CDec(e.Row.Cells(6).Text)
        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(1).Text = "TOTALES"
            e.Row.Cells(2).Text = _Presupuestado.ToString("C0")

            'e.Row.Cells(3).Text = _Autorizado.ToString("C0")
            'e.Row.Cells(4).Text = _DiferenciaPA.ToString("C0")
            ''e.Row.Cells(5).Text = _Porcentaje1.ToString("N")
            'e.Row.Cells(6).Text = _TotalProduccion.ToString("C0")
            'e.Row.Cells(7).Text = _DiferenciaPP.ToString("C0")
            ''e.Row.Cells(8).Text = _TotalProduccion.ToString("C0")
            'e.Row.Cells(9).Text = _Ejecutado.ToString("C0")
            'e.Row.Cells(10).Text = _DiferenciaPE.ToString("C0")
            '' e.Row.Cells(8).Text = _Porcentaje2.ToString("N")
            e.Row.Cells(2).CssClass = "RightAlign"


        End If


    End Sub

    Protected Sub btnExportar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportar.Click
        Dim excel As New List(Of Array)
        Dim Titulos As String
        If TabContainer1.ActiveTab.ID = "TabPanel3" Then
            Titulos = "CODIGO;CIUDAD;HOTELES;TRANSPORTE;TOTAL"
        Else
            If (Request.QueryString("o_3453oioioioo_1133").ToString = 1) = True Then
                Titulos = "ID;ACTIVIDAD;PRESUPUESTADO"
            Else
                Titulos = "ID;ACTIVIDAD;PRESUPUESTADO;UNIDADES;DESCRIPCION;HORAS"
            End If

        End If

        Dim par As New IQ_Parametros()
        par.IdPropuesta = CLng(Request.QueryString("idPropuesta"))
        par.ParAlternativa = CInt(Request.QueryString("Alternativa"))
        par.MetCodigo = CInt(Request.QueryString("Metodologia"))
        par.ParNacional = CInt(Request.QueryString("Nacional"))

        Dim DynamicColNames() As String
        Dim lstCambios As New List(Of IQ_ObtenerActControlCostos_Result)
        Dim lstViaticos As New List(Of IQ_ObtenerViaticosPresupuesto_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("Costos")


        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)

        If TabContainer1.ActiveTab.ID = "TabPanel1" Then
            lstCambios = _ControlCostos.ObtenerCostos(par, 1)
        ElseIf TabContainer1.ActiveTab.ID = "TabPanel3" Then
            lstViaticos = _ControlCostos.ObtenerViaticos(par)
        Else
            lstCambios = _ControlCostos.ObtenerCostos(par, 2)
        End If

        If TabContainer1.ActiveTab.ID = "TabPanel3" Then
            For Each s In lstViaticos
                excel.Add((s.CODIGO.ToString() & ";" & s.CIUDAD.ToString() & ";" & CDec(s.HOTELES).ToString("N2") & ";" & CDec(s.TRANSPORTE).ToString("N2") & ";" & CDec(s.TOTAL).ToString("N2")).Split(CChar(";")).ToArray())
            Next

            worksheet.Cell("A1").Value = excel
            worksheet.Range("C2:E100").DataType = XLCellValues.Number
            worksheet.Range("C2:E100").Style.NumberFormat.NumberFormatId = 4
        Else
            If (Request.QueryString("o_3453oioioioo_1133").ToString = 1) = True Then
                For Each x In lstCambios
                    excel.Add((x.ID.ToString() & ";" & x.ActNombre & ";" & CDec(x.PRESUPUESTADO).ToString("N2")).Split(CChar(";")).ToArray())
                Next

                worksheet.Cell("A1").Value = excel
                worksheet.Range("C2:C100").DataType = XLCellValues.Number
                worksheet.Range("C2:C100").Style.NumberFormat.NumberFormatId = 4
            Else
                For Each x In lstCambios
                    excel.Add((x.ID.ToString() & ";" & x.ActNombre & ";" & CDec(x.PRESUPUESTADO).ToString("N2") & ";" & CDec(x.UNIDADES).ToString("N0") & ";" & x.DESC_UNIDADES).Split(CChar(";") & ";" & CDec(x.HORAS).ToString("N0")).ToArray())
                Next
                worksheet.Cell("A1").Value = excel
                worksheet.Range("C2:C100").DataType = XLCellValues.Number
                worksheet.Range("C2:C100").Style.NumberFormat.NumberFormatId = 4

                worksheet.Range("F2:F100").DataType = XLCellValues.Number
                worksheet.Range("F2:F100").Style.NumberFormat.NumberFormatId = 4

            End If



        End If




        If TabContainer1.ActiveTab.ID = "TabPanel1" Then
            Crearexcel(workbook, "Detalles Costo unidad")
        ElseIf TabContainer1.ActiveTab.ID = "TabPanel3" Then
            Crearexcel(workbook, "Viaticos")
        Else
            Crearexcel(workbook, "Detalles costo operaciones")
        End If

    End Sub

    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Control_" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Protected Sub gvDetallesOperaciones_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDetallesOperaciones.RowDataBound


        If e.Row.RowType = DataControlRowType.DataRow Then
            _Presupuestado2 = _Presupuestado2 + CDec(e.Row.Cells(2).Text.Replace(".", ""))
            e.Row.Cells(2).CssClass = "RightAlign"
            e.Row.Cells(11).CssClass = "RightAlign"
            _TotalHoras = _TotalHoras + CInt(e.Row.Cells(13).Text)

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(1).Text = "TOTALES"
            e.Row.Cells(2).Text = _Presupuestado2.ToString("C0")
            e.Row.Cells(2).CssClass = "RightAlign"
            e.Row.Cells(13).Text = _TotalHoras.ToString("N0")

        End If

        If (Request.QueryString("o_3453oioioioo_1133").ToString = 1) = True Then
            e.Row.Cells(11).Visible = False
            e.Row.Cells(12).Visible = False
            e.Row.Cells(13).Visible = False
        End If
    End Sub

    Protected Sub gvViaticos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvViaticos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            _TotalViaticos = _TotalViaticos + CDec(e.Row.Cells(4).Text)
            _TotalHoteles = _TotalHoteles + CDec(e.Row.Cells(2).Text)
            _TotalTransporte = _TotalTransporte + CDec(e.Row.Cells(3).Text)
            e.Row.Cells(4).CssClass = "RightAlign"
            e.Row.Cells(2).CssClass = "RightAlign"
            e.Row.Cells(3).CssClass = "RightAlign"

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(1).Text = "TOTALES"
            e.Row.Cells(4).Text = _TotalViaticos.ToString("C0")
            e.Row.Cells(2).Text = _TotalHoteles.ToString("C0")
            e.Row.Cells(3).Text = _TotalTransporte.ToString("C0")

            e.Row.Cells(4).CssClass = "RightAlign"
            e.Row.Cells(2).CssClass = "RightAlign"
            e.Row.Cells(3).CssClass = "RightAlign"
        End If
    End Sub

    Private Function ObtenerControl(ByVal p As Control, ByVal name As String) As Control

        Dim c As Control
        For Each c In p.Controls

            If c.ID = name Then
                Return c
            Else
                If c.HasControls Then
                    ObtenerControl(c, name)
                    Return Nothing
                Else
                    Return Nothing
                End If
                Return Nothing
            End If
            Return Nothing
        Next
        Return Nothing
    End Function


    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'Dim c As Control
        'c = ObtenerControl(Me.mPanel, "TabPanel3")
        'c.Visible = False
    End Sub


End Class