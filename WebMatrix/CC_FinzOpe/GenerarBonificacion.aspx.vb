Imports WebMatrix.Util
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports CoreProject
Imports CoreProject.CC_FinzOpe
Public Class GenerarBonificacion
    Inherits System.Web.UI.Page
    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click

        If Not (IsDate(txtFechaIngreso.Text)) Then
            ShowNotification("Escriba la fecha de Ingreso", ShowNotifications.ErrorNotification)
            txtFechaIngreso.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If Not (IsDate(txtFechaRetiro.Text)) Then
            ShowNotification("Escriba la fecha de Retiro", ShowNotifications.ErrorNotification)
            txtFechaRetiro.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If (CDate(txtFechaRetiro.Text)) < (CDate(txtFechaIngreso.Text)) Then
            ShowNotification("La fecha de retiro no puede ser menor a la fecha de ingreso", ShowNotifications.ErrorNotification)
            txtFechaRetiro.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If


        Dim opr As New PresupInt
        Dim Cedulas As List(Of CC_CedulasProduccion_Result)

        Dim ciudad As Int64? = Nothing
        Dim tipocontratacion As Int64? = Nothing
        Dim cedula As Int64? = Nothing
        If IsNumeric(TxtCedula.Text) Then cedula = TxtCedula.Text
        If IsNumeric(txtTipoContratacion.Text) Then tipocontratacion = txtTipoContratacion.Text
        If IsNumeric(txtCiudad.Text) Then ciudad = txtCiudad.Text

        Cedulas = opr.CedulasProduccion(CDate(txtFechaInicio.Text), CDate(txtFechaFinalizacion.Text), ciudad, tipocontratacion, cedula, Nothing).ToList

        For i = 0 To Cedulas.Count - 1
            Me.gvProduccion.DataSource = opr.ProduccionGetxFecha(CDate(txtFechaInicio.Text), CDate(txtFechaFinalizacion.Text), Cedulas.Item(i).PersonaId)
            Me.gvProduccion.DataBind()

            CrearPdf(Cedulas.Item(i).PersonaId)

            'PARAR
            'If i = 3 Then
            'i = 99999
            'End If
        Next i

        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Sub CrearPdf(ByVal Cedula As Int64)
        Dim total As Double
        Dim VrProvision As Double
        Dim trabajo1 As String = ""
        Dim job1 As String = ""
        Dim diastrabajados As New Double

        Dim op As New PresupInt

        Dim urlFija As String
        Dim Url As String

        Dim Consecutivo As List(Of CC_ConsecutivoCC_Result)
        Dim FecUltimaCC As List(Of CC_FechaUltimaCC_Result)


        'Dim Ancho As Single() = {Ssg(4), Ssg(9), Ssg(4), Ssg(4)}
        'Dim Url As String
        urlFija = "~/Cuentasdecobro/"
        Url = "..\Cuentasdecobro\"
        'Url = hfIdTrabajo.Value
        'HACK Deuda tecnica Aquí se deberia colocar la ruta raíz en un parametro en la base de datos
        urlFija = Server.MapPath(urlFija & "\")
        'Dim Persona As New CoreProject.TH_PersonasGET_Result Revisar Ajuste
        Dim Persona As New TH_PersonasGET_Result
        Dim Descuento As List(Of CC_DescuentosGet_Result)
        Dim WDescuento As Decimal

        'Dim oeTrabajo As List(Of PY_Trabajo0)
        Dim Col_TitlePage As New BaseColor(30, 172, 167)
        Dim Col_Title As New BaseColor(14, 1, 129)
        Dim Col_Title1 As New BaseColor(204, 232, 212)
        Dim Col_Title2 As New BaseColor(242, 249, 220)
        Dim Col_Title3 As New BaseColor(211, 211, 211)

        Dim font10 As New Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL)
        Dim font10B As New Font(Font.FontFamily.HELVETICA, 9, Font.BOLD)
        Dim font12 As New Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL)
        Dim font12B As New Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)
        Dim font8 As New Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL)
        Dim font8B As New Font(Font.FontFamily.HELVETICA, 8, Font.BOLD)
        Dim font7 As New Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL)
        Dim fontTitle As New Font(Font.FontFamily.HELVETICA, 9, Font.BOLD)
        Dim path As String = Server.MapPath("~/Files/")
        Dim tabla As New PdfPTable(4)
        Dim Ancho As Single() = {Ssg(4), Ssg(9), Ssg(4), Ssg(4)}
        tabla.SetWidths(Ancho)
        tabla.DefaultCell.Border = 0
        tabla.WidthPercentage = 95

        Dim PdfCell As New PdfPCell

        tabla = New PdfPTable(1)
        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        PdfCell.HorizontalAlignment = 3

        Dim pdfw As PdfWriter
        Dim documentoPDF As New Document(iTextSharp.text.PageSize.A4, 0, 0, 20, 20) 'Creamos el objeto documento PDF
        pdfw = PdfWriter.GetInstance(documentoPDF, New FileStream(urlFija & "\" & "CuentadeCobro-Bonificacion-" & Cedula & ".pdf", FileMode.Create))
        documentoPDF.Open()
        Consecutivo = op.ConsecutivoCC
        Descuento = op.Descuentos(Cedula, txtFechaInicio.Text, txtFechaFinalizacion.Text)
        WDescuento = 0
        If Descuento.Count > 0 Then
            WDescuento = Descuento(0).Total
        End If
        FecUltimaCC = op.FecchaUltimaCC(Cedula)
        Dim t As Integer
        t = FecUltimaCC.Count
        Persona = Informacionpersonas(Cedula)

        ' #### New Document
        tabla = New PdfPTable(2)
        Ancho = {Ssg(10), Ssg(11)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        Dim Imagen As iTextSharp.text.Image
        Imagen = iTextSharp.text.Image.GetInstance(path & "Ipsos.png")
        Imagen.ScalePercent(15)
        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        PdfCell.HorizontalAlignment = 1
        PdfCell.VerticalAlignment = 1
        PdfCell.Rowspan = 4
        PdfCell.AddElement(Imagen)
        tabla.AddCell(PdfCell)

        tabla.AddCell(NewPdfCell("", 3, False, False))
        tabla.AddCell(NewPdfCell("", 3, False, False))
        tabla.AddCell(NewPdfCell("", 3, False, False))
        tabla.AddCell(NewPdfCell("", 3, False, False))

        documentoPDF.Add(tabla)

        tabla = New PdfPTable(1)
        Ancho = {Ssg(21)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        Dim titulo As String = ""
        titulo = "DOCUMENTO SOPORTE EN ADQUISICIONES EFECTUADAS A NO OBLIGADOS A FACTURAR"

        Dim Parag As New Paragraph(titulo, fontTitle)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 5
        Parag.SpacingAfter = 7
        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 1, 1)
        PdfCell.HorizontalAlignment = PdfPCell.ALIGN_JUSTIFIED_ALL
        PdfCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)
        documentoPDF.Add(tabla)

        documentoPDF.Add(NewRenglonEnter())

        tabla = New PdfPTable(5)
        Ancho = {Ssg(2), Ssg(4), Ssg(9), Ssg(4), Ssg(2)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph("FECHA EXPEDICIÓN", font10B)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph("No. CONSECUTIVO", font10B)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        tabla.AddCell(PdfCell)

        Dim FechaFactura As Date = Date.Parse(Now()).ToString("dd/MM/yyyy")

        PdfCell = New PdfPCell
        Dim FechaElaboracion = DateTime.Parse(FechaFactura).ToString("dd/MM/yyyy")
        Parag = New Paragraph(FechaElaboracion, font10)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell

        Parag = New Paragraph("", font10)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        tabla.AddCell(PdfCell)

        documentoPDF.Add(tabla)
        documentoPDF.Add(NewRenglonEnter())

        tabla = New PdfPTable(2)
        CellBorders(PdfCell, 1, 1, 1, 1)
        Ancho = {Ssg(14), Ssg(7)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        Parag = New Paragraph("INFORMACIÓN DEL VENDEDOR O PRESTADOR DE SERVICIO", fontTitle)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.Colspan = 2
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("Nombre", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("Número de Identificación", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 1)
        Dim nombreProveedor = Persona.Nombres & " " & Persona.Apellidos
        Parag = New Paragraph(nombreProveedor, font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 1)
        Dim nit = Persona.id
        Parag = New Paragraph(nit, font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        documentoPDF.Add(tabla)

        tabla = New PdfPTable(3)
        CellBorders(PdfCell, 1, 1, 1, 1)
        Ancho = {Ssg(7), Ssg(7), Ssg(7)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("Dirección", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_LEFT
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("Teléfono", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_LEFT
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("Ciudad", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_LEFT
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 1)
        Dim direccion = Persona.Direccion
        Parag = New Paragraph(direccion, font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 1)
        Dim telefono = Persona.Telefono1
        Parag = New Paragraph(telefono, font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 1)
        'ciudad = Persona.Ciudad
        Parag = New Paragraph(Persona.Ciudad, font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        documentoPDF.Add(tabla)

        documentoPDF.Add(NewRenglonEnter())

        tabla = New PdfPTable(2)
        CellBorders(PdfCell, 1, 1, 1, 1)
        Ancho = {Ssg(14), Ssg(7)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        PdfCell = New PdfPCell
        Parag = New Paragraph("INFORMACIÓN DE LA EMPRESA", fontTitle)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.Colspan = 2
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("Nombre", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("Número de Identificación", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 1)
        Parag = New Paragraph("IPSOS NAPOLEÓN FRANCO & CIA SAS", font10B)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 1)
        Parag = New Paragraph("890.319.494      DV: 5", font10B)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        documentoPDF.Add(tabla)

        tabla = New PdfPTable(3)
        CellBorders(PdfCell, 1, 1, 1, 1)
        Ancho = {Ssg(7), Ssg(7), Ssg(7)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("Dirección", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_LEFT
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("Teléfono", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_LEFT
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("Ciudad", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_LEFT
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 1)
        Parag = New Paragraph("CL 74 11 81 P 5", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 1)
        Parag = New Paragraph("3769400", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 1)
        Parag = New Paragraph("BOGOTÁ", font10)
        'Parag.SpacingAfter = 2
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        documentoPDF.Add(tabla)
        documentoPDF.Add(NewRenglonEnter())

        tabla = New PdfPTable(5)
        CellBorders(PdfCell, 1, 1, 1, 1)
        Ancho = {Ssg(4), Ssg(5), Ssg(7), Ssg(2), Ssg(4)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        PdfCell = New PdfPCell
        Parag = New Paragraph("DETALLE DE LA OPERACIÓN", fontTitle)
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        Parag.SetLeading(0.5F, 1.0F)
        Parag.Alignment = Element.ALIGN_CENTER
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.Colspan = 6
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 1, 1)
        Parag = New Paragraph("JOB", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        'Parag.SpacingAfter = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 1, 1)
        Parag = New Paragraph("NOMBRE JOB", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        'Parag.SpacingAfter = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 1, 1)
        Parag = New Paragraph("CONCEPTO", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        'Parag.SpacingAfter = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 1, 1)
        Parag = New Paragraph("CANTIDAD", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        'Parag.SpacingAfter = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)


        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 1, 1)
        Parag = New Paragraph("VALOR TOTAL", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        'Parag.SpacingAfter = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        ' ### Espacio Requerimiento Servicio
        Dim aTable19 = New iTextSharp.text.pdf.PdfPTable(3)
        aTable19.DefaultCell.Border = BorderStyle.Double

        Dim CL3 = New PdfPCell(New Paragraph("Job-Estudio", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        CL3.HorizontalAlignment = 1

        Dim CL4 = New PdfPCell(New Paragraph("Trabajo", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        CL4.HorizontalAlignment = 1

        Dim CL5 = New PdfPCell(New Paragraph("Unitario", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        CL5.HorizontalAlignment = 1

        aTable19.AddCell(CL3)
        aTable19.AddCell(CL4)
        aTable19.AddCell(CL5)
        ' ### End Espacio Requerimiento Servicio

        '---INICIA DETALLE
        total = 0
        VrProvision = 0
        Dim WJob As String
        Dim WNTrabajo As String


        For x = 0 To gvProduccion.Rows.Count - 1

            WJob = gvProduccion.Rows(x).Cells(0).Text
            WNTrabajo = gvProduccion.Rows(x).Cells(2).Text


            PdfCell = New PdfPCell
            Parag = New Paragraph(WJob, font8)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            Parag = New Paragraph(WNTrabajo, font7)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            Dim conceptoTrabajo As String = "Trabajo Campo Nacional"
            Parag = New Paragraph(conceptoTrabajo, font7)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            Parag = New Paragraph(1, font8)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            VrProvision = gvProduccion.Rows(x).Cells(4).Text

            PdfCell = New PdfPCell
            Parag = New Paragraph(VrProvision.ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            total = total + VrProvision


            ' ### Para requerimiento de Servicio
            Dim CL9 = New PdfPCell(New Paragraph(WJob, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            CL9.HorizontalAlignment = 1
            Dim CL10 = New PdfPCell(New Paragraph(WNTrabajo, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            CL10.HorizontalAlignment = 1
            Dim CL11 = New PdfPCell(New Paragraph(VrProvision, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            CL11.HorizontalAlignment = 1
            aTable19.AddCell(CL9)
            aTable19.AddCell(CL10)
            aTable19.AddCell(CL11)
            ' ### End Req Servicio



        Next


        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        Parag = New Paragraph("", font8)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.Colspan = 2
        PdfCell.Rowspan = 5
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph("TOTAL", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        PdfCell.Colspan = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph(total.ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph("Retención en la Fuente", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        PdfCell.Colspan = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph("", font8)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph("Otros", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        PdfCell.Colspan = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph(WDescuento.ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph("Retención de ICA", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        PdfCell.Colspan = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        Dim sub1 As Double = 0

        If Persona.CiudadId = Divipola.ECiudades.Bogota Then
            sub1 = (total) * (9.66 / 1000)
        End If

        If sub1 > 0 Then
            PdfCell = New PdfPCell
            Parag = New Paragraph(sub1.ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)
        Else
            PdfCell = New PdfPCell
            Parag = New Paragraph("", font8)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)
        End If


        PdfCell = New PdfPCell
        Parag = New Paragraph("TOTAL A PAGAR", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        PdfCell.Colspan = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph((total - sub1 - WDescuento).ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph("Valor en Letras", font8B)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        PdfCell.Colspan = 2
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        Parag = New Paragraph("", font8)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.Colspan = 4
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        documentoPDF.Add(tabla)
        documentoPDF.Add(NewRenglonEnter())

        tabla = New PdfPTable(1)
        CellBorders(PdfCell, 1, 1, 1, 1)
        Ancho = {Ssg(22)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 1, 1)
        Parag = New Paragraph("OBSERVACIONES", fontTitle)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.BackgroundColor = Col_Title3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 1, 1)
        Parag = New Paragraph("     ", font8)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        PdfCell.AddElement(Parag)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        documentoPDF.Add(tabla)
        documentoPDF.Add(NewRenglonEnter())

        tabla = New PdfPTable(1)
        CellBorders(PdfCell, 0, 0, 0, 0)
        Ancho = {Ssg(22)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 0)
        Parag = New Paragraph("A este Documento se le aplican las normas relativas del Documento Soporte en adquisidores efectuadas a sujetos no obligados a facturar (Resolución 167 de 30 de diciembre de 2021). Número Autorización 18764058724745 aprobado en 20231026 prefijo DSE desde el nùmero 9501 al 16000 Vigencia: 12 meses.", fontTitle)
        Parag.Alignment = Element.ALIGN_CENTER
        Parag.SpacingBefore = 2
        Parag.SetLeading(0.5F, 1.0F)
        'PdfCell.AddElement(Parag)
        'Parag = New Paragraph("Rango de numeración DS 001 hasta DS 3.000", font8)
        'Parag.Alignment = Element.ALIGN_CENTER
        'Parag.SpacingAfter = 2
        'Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        documentoPDF.Add(tabla)
        documentoPDF.Add(NewRenglonEnter())

        tabla = New PdfPTable(4)
        CellBorders(PdfCell, 1, 1, 1, 1)
        Ancho = {Ssg(2), Ssg(6), Ssg(8), Ssg(6)}
        tabla.SetWidths(Ancho)
        tabla.WidthPercentage = 95

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 1)
        Parag = New Paragraph("FIRMA DEL VENDEDOR O PRESTADOR DE SERVICIO", font8B)
        Parag.Alignment = Element.ALIGN_LEFT
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.Colspan = 2
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 0, 0)
        Parag = New Paragraph("FIRMA EMISOR", font8B)
        Parag.Alignment = Element.ALIGN_LEFT
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        Parag.SetLeading(0.5F, 1.0F)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 1, 1, 1, 0)
        Parag = New Paragraph("SELLO RECIBIDO", font8B)
        Parag.Alignment = Element.ALIGN_LEFT
        Parag.SpacingBefore = 3
        Parag.SpacingAfter = 3
        Parag.SetLeading(0.5F, 1.0F)
        'PdfCell.Colspan = 2
        PdfCell.Rowspan = 4
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 1)
        Parag = New Paragraph("     ", font8)
        PdfCell.AddElement(Parag)
        PdfCell.AddElement(Parag)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 0)
        Parag = New Paragraph("     ", font8)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 0)
        Parag = New Paragraph("     ", font8)
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)


        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 0, 1)
        Parag = New Paragraph("Nombre", font10B)
        Parag.Alignment = Element.ALIGN_RIGHT
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)


        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 0, 0)
        Parag = New Paragraph(nombreProveedor, font10)
        PdfCell.AddElement(Parag)
        PdfCell.Colspan = 1
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 0)
        Parag = New Paragraph("IPSOS NAPOLEON FRANCO & CIA SAS", font10)
        PdfCell.AddElement(Parag)
        PdfCell.Colspan = 1
        PdfCell.Rowspan = 2
        tabla.AddCell(PdfCell)

        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 0, 1, 1)
        Parag = New Paragraph("C.C.", font10B)
        Parag.Alignment = Element.ALIGN_RIGHT
        Parag.SpacingAfter = 3
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)


        PdfCell = New PdfPCell
        CellBorders(PdfCell, 0, 1, 1, 0)
        Parag = New Paragraph(nit, font10)
        Parag.SpacingAfter = 3
        PdfCell.AddElement(Parag)
        PdfCell.Colspan = 1
        tabla.AddCell(PdfCell)

        documentoPDF.Add(tabla)
        ' #### End New Document


        'documentoPDF.NewPage()
        'Dim aTable = New iTextSharp.text.pdf.PdfPTable(2)
        'aTable.DefaultCell.Border = BorderStyle.None
        'Dim Imagen As iTextSharp.text.Image
        'Imagen = iTextSharp.text.Image.GetInstance(path & "logo-PST.png")
        'Imagen.ScalePercent(10)
        'Dim Img = New PdfPCell
        'Img.Border = 2
        'Img.AddElement(Imagen)
        'aTable.AddCell(Img)
        'aTable.AddCell(New Paragraph("DOCUMENTO EQUIVALENTE A LA FACTURA PARA REGIMEN SIMPLIFICADO Y PERSONAS NATURALES ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        'Dim aTable1 = New iTextSharp.text.pdf.PdfPTable(2)
        'Dim C1 = New PdfPCell(New Paragraph("FECHA EXPEDICION", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'C1.HorizontalAlignment = 1
        'Dim C2 = New PdfPCell(New Paragraph("No. CONSECUTIVO", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'C2.HorizontalAlignment = 1
        'aTable1.AddCell(C1)
        'aTable1.AddCell(C2)
        'Dim aTable2 = New iTextSharp.text.pdf.PdfPTable(2)
        'Dim C3 = New PdfPCell(New Paragraph("Año:" & Year(Now()) & "    Mes:" & Month(Now()) & "    Dia:" & Day(Now()), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'C3.HorizontalAlignment = 1
        'aTable2.AddCell(C3)
        'Dim Cons = New PdfPCell(New Paragraph(Consecutivo.Item(0).Numero + 1, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'Cons.HorizontalAlignment = 1
        'aTable2.AddCell(Cons)
        'Dim aTable3 = New iTextSharp.text.pdf.PdfPTable(1)
        'aTable3.DefaultCell.Border = BorderStyle.Double
        'Dim Cell1 = New PdfPCell(New Paragraph("INFORMACION DEL VENDEDOR O PRESTADOR DE SERVICIO", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        'Cell1.Colspan = 3
        ''Cell1.Border = 7
        'Cell1.HorizontalAlignment = 1
        'aTable3.AddCell(Cell1)
        'Dim aTable4 = New iTextSharp.text.pdf.PdfPTable(2)
        'aTable4.DefaultCell.Border = BorderStyle.Double
        'Dim C6 = New PdfPCell(New Paragraph("NOMBRE", FontFactory.GetFont(FontFactory.TIMES, 8, iTextSharp.text.Font.NORMAL)))
        'C6.HorizontalAlignment = 1
        'Dim C7 = New PdfPCell(New Paragraph("NUMERO DE IDENTIFICACION", FontFactory.GetFont(FontFactory.TIMES, 8, iTextSharp.text.Font.NORMAL)))
        'C7.HorizontalAlignment = 1
        'Dim C8 = New PdfPCell(New Paragraph(Persona.Nombres & " " & Persona.Apellidos, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'C8.HorizontalAlignment = 1
        'Dim C9 = New PdfPCell(New Paragraph(Persona.id, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'C9.HorizontalAlignment = 1
        'aTable4.AddCell(C6)
        'aTable4.AddCell(C7)
        'aTable4.AddCell(C8)
        'aTable4.AddCell(C9)
        'Dim aTable5 = New iTextSharp.text.pdf.PdfPTable(3)
        'aTable5.DefaultCell.Border = BorderStyle.Double
        'Dim C10 = New PdfPCell(New Paragraph("DIRECCION", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'C10.HorizontalAlignment = 1
        'Dim C11 = New PdfPCell(New Paragraph("TELEFONO", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'C11.HorizontalAlignment = 1
        'Dim C12 = New PdfPCell(New Paragraph("CIUDAD", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'C12.HorizontalAlignment = 1
        'Dim C13 = New PdfPCell(New Paragraph(Persona.Direccion, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'C13.HorizontalAlignment = 1
        'Dim C14 = New PdfPCell(New Paragraph(Persona.Telefono1 & " - " & Persona.Telefono2, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'C14.HorizontalAlignment = 1
        'Dim C15 = New PdfPCell(New Paragraph(Persona.Ciudad, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'C15.HorizontalAlignment = 1
        'aTable5.AddCell(C10)
        'aTable5.AddCell(C11)
        'aTable5.AddCell(C12)
        'aTable5.AddCell(C13)
        'aTable5.AddCell(C14)
        'aTable5.AddCell(C15)
        'Dim aTable6 = New iTextSharp.text.pdf.PdfPTable(1)
        'aTable6.DefaultCell.Border = BorderStyle.Double
        'Dim Cell2 = New PdfPCell(New Paragraph("INFORMACION DE LA EMPRESA", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        'Cell2.Colspan = 3
        ''Cell2.Border = 7
        'Cell2.HorizontalAlignment = 1
        'aTable6.AddCell(Cell2)
        'Dim aTable7 = New iTextSharp.text.pdf.PdfPTable(2)
        'Dim C16 = New PdfPCell(New Paragraph("NOMBRE", FontFactory.GetFont(FontFactory.TIMES, 8, iTextSharp.text.Font.NORMAL)))
        'C16.HorizontalAlignment = 1
        'Dim C17 = New PdfPCell(New Paragraph("NUMERO DE IDENTIFICACION", FontFactory.GetFont(FontFactory.TIMES, 8, iTextSharp.text.Font.NORMAL)))
        'C17.HorizontalAlignment = 1
        'Dim C18 = New PdfPCell(New Paragraph("IPSOS NAPOLEON FRANCO & CIA SAS", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'C18.HorizontalAlignment = 1
        'Dim C19 = New PdfPCell(New Paragraph("890.319.494          DV:5 ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'C19.HorizontalAlignment = 1
        'aTable7.AddCell(C16)
        'aTable7.AddCell(C17)
        'aTable7.AddCell(C18)
        'aTable7.AddCell(C19)
        'Dim aTable8 = New iTextSharp.text.pdf.PdfPTable(3)
        'Dim C20 = New PdfPCell(New Paragraph("DIRECCION", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'C20.HorizontalAlignment = 1
        'Dim C21 = New PdfPCell(New Paragraph("TELEFONO", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'C21.HorizontalAlignment = 1
        'Dim C22 = New PdfPCell(New Paragraph("CIUDAD", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'C22.HorizontalAlignment = 1
        'Dim C23 = New PdfPCell(New Paragraph("Calle 74   11 – 81 Piso 5", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        'C23.HorizontalAlignment = 1
        'Dim C24 = New PdfPCell(New Paragraph("+57 1 6286600", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        'C24.HorizontalAlignment = 1
        'Dim C25 = New PdfPCell(New Paragraph("Bogotá D.C.", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        'C25.HorizontalAlignment = 1
        'aTable8.AddCell(C20)
        'aTable8.AddCell(C21)
        'aTable8.AddCell(C22)
        'aTable8.AddCell(C23)
        'aTable8.AddCell(C24)
        'aTable8.AddCell(C25)
        'Dim aTable9 = New iTextSharp.text.pdf.PdfPTable(1)
        'aTable9.DefaultCell.Border = BorderStyle.Double
        'Dim Cell3 = New PdfPCell(New Paragraph("DETALLE DE LA OPERACIÓN", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        'Cell3.Colspan = 3
        'Cell3.HorizontalAlignment = 1
        'aTable9.AddCell(Cell3)
        'Dim aTable10 = New iTextSharp.text.pdf.PdfPTable(5)
        'aTable10.DefaultCell.Border = BorderStyle.Double
        'Dim CL1 = New PdfPCell(New Paragraph("Cantidad", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'CL1.HorizontalAlignment = 1
        'Dim CL2 = New PdfPCell(New Paragraph("Concepto", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'CL2.HorizontalAlignment = 1
        'Dim CL3 = New PdfPCell(New Paragraph("Job-Estudio", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'CL3.HorizontalAlignment = 1
        'Dim CL4 = New PdfPCell(New Paragraph("Trabajo", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'CL4.HorizontalAlignment = 1

        ''Dim CL5 = New PdfPCell(New Paragraph("Unitario", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        ''CL5.HorizontalAlignment = 1

        'Dim CL6 = New PdfPCell(New Paragraph("Total", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'CL6.HorizontalAlignment = 1
        'aTable10.AddCell(CL1)
        'aTable10.AddCell(CL2)
        'aTable10.AddCell(CL3)
        'aTable10.AddCell(CL4)
        '' aTable10.AddCell(CL5)
        'aTable10.AddCell(CL6)

        ''---INICIA DETALLE
        'total = 0
        'VrProvision = 0
        'Dim WJob As String
        'Dim WNTrabajo As String

        'For x = 0 To gvProduccion.Rows.Count - 1
        '    WJob = gvProduccion.Rows(x).Cells(0).Text
        '    WNTrabajo = gvProduccion.Rows(x).Cells(2).Text

        '    Dim CL7 = New PdfPCell(New Paragraph("1", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        '    CL7.HorizontalAlignment = 1
        '    CL7.FixedHeight = 5

        '    VrProvision = gvProduccion.Rows(x).Cells(4).Text

        '    Dim CL8 = New PdfPCell(New Paragraph("Trabajo Campo Nacional", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        '    CL8.HorizontalAlignment = 1
        '    Dim CL9 = New PdfPCell(New Paragraph(WJob, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        '    CL9.HorizontalAlignment = 1
        '    Dim CL10 = New PdfPCell(New Paragraph(WNTrabajo, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        '    CL10.HorizontalAlignment = 1

        '    'Dim CL11 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        '    'CL11.HorizontalAlignment = 1

        '    Dim CL12 = New PdfPCell(New Paragraph(FormatCurrency((VrProvision), 0), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        '    CL12.HorizontalAlignment = 2
        '    aTable10.AddCell(CL7)
        '    aTable10.AddCell(CL8)
        '    aTable10.AddCell(CL9)
        '    aTable10.AddCell(CL10)
        '    'aTable10.AddCell(CL11)
        '    aTable10.AddCell(CL12)

        '    total = total + VrProvision

        'Next

        'Dim At1 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'At1.HorizontalAlignment = 1
        'At1.FixedHeight = 5
        'Dim At2 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'At2.HorizontalAlignment = 1
        'Dim At3 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'At3.HorizontalAlignment = 1
        'Dim At4 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'At4.HorizontalAlignment = 1
        'Dim CL13 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'CL13.HorizontalAlignment = 1
        'Dim CL14 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'CL14.HorizontalAlignment = 1
        'Dim CL15 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'CL15.HorizontalAlignment = 1

        ''Dim CL16 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        ''CL16.HorizontalAlignment = 1

        'Dim CL17 = New PdfPCell(New Paragraph("Total", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'CL17.HorizontalAlignment = 1
        'Dim CL18 = New PdfPCell(New Paragraph(FormatCurrency(total, 0), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        'CL18.HorizontalAlignment = 2
        'aTable10.AddCell(CL13)
        'aTable10.AddCell(CL14)
        'aTable10.AddCell(CL15)
        '' aTable10.AddCell(CL16)
        'aTable10.AddCell(CL17)
        'aTable10.AddCell(CL18)
        'Dim aTable12 = New iTextSharp.text.pdf.PdfPTable(3)
        'Dim CL19 = New PdfPCell(New Paragraph("OBSERVACIONES", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'CL19.HorizontalAlignment = 2
        'Dim CL20 = New PdfPCell(New Paragraph("Retencion en la Fuente", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        'CL20.HorizontalAlignment = 2
        'Dim CL21 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'CL21.HorizontalAlignment = 2
        'aTable12.AddCell(CL19)
        'aTable12.AddCell(CL20)
        'aTable12.AddCell(CL21)
        'Dim CL22 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'CL22.HorizontalAlignment = 2
        'Dim CL23 = New PdfPCell(New Paragraph("Otros", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        'CL23.HorizontalAlignment = 2
        'Dim CL24 = New PdfPCell(New Paragraph(FormatCurrency(WDescuento, 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'CL24.HorizontalAlignment = 2
        'aTable12.AddCell(CL22)
        'aTable12.AddCell(CL23)
        'aTable12.AddCell(CL24)


        'If Persona.CiudadId = Divipola.ECiudades.Bogota Then
        '    Dim CL25 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        '    CL25.HorizontalAlignment = 2
        '    Dim CL26 = New PdfPCell(New Paragraph("Retencion de ICA", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        '    CL26.HorizontalAlignment = 2
        '    Dim CL27 = New PdfPCell(New Paragraph(FormatCurrency((total) * (9.66 / 1000), 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

        '    sub1 = (total) * (9.66 / 1000)

        '    CL27.HorizontalAlignment = 2
        '    aTable12.AddCell(CL25)
        '    aTable12.AddCell(CL26)
        '    aTable12.AddCell(CL27)

        'Else
        '    Dim CL25 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        '    CL25.HorizontalAlignment = 2
        '    Dim CL26 = New PdfPCell(New Paragraph("Retencion de ICA", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        '    CL26.HorizontalAlignment = 2
        '    Dim CL27 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        '    CL27.HorizontalAlignment = 2
        '    sub1 = 0
        '    aTable12.AddCell(CL25)
        '    aTable12.AddCell(CL26)
        '    aTable12.AddCell(CL27)
        'End If

        'Dim CL28 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'CL28.HorizontalAlignment = 2
        'Dim CL29 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        'CL29.HorizontalAlignment = 2
        'Dim CL30 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'CL30.HorizontalAlignment = 2
        'aTable12.AddCell(CL28)
        'aTable12.AddCell(CL29)
        'aTable12.AddCell(CL30)
        'aTable12.AddCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'aTable12.AddCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'aTable12.AddCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'Dim CL31 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'CL31.HorizontalAlignment = 2
        'Dim CL32 = New PdfPCell(New Paragraph("TOTAL A PAGAR", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        'CL32.HorizontalAlignment = 2
        'Dim CL33 = New PdfPCell(New Paragraph(FormatCurrency((total - sub1 - WDescuento), 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'CL33.HorizontalAlignment = 2
        'aTable12.AddCell(CL31)
        'aTable12.AddCell(CL32)
        'aTable12.AddCell(CL33)
        'Dim aTable11 = New iTextSharp.text.pdf.PdfPTable(1)
        'aTable11.DefaultCell.Border = BorderStyle.Double
        'Dim Cell4 = New PdfPCell(New Paragraph("Valor en letras: ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        'Cell4.Colspan = 3
        ''Cell4.Border = 7
        'Cell4.HorizontalAlignment = 0
        'aTable11.AddCell(Cell4)
        'Dim aTable13 = New iTextSharp.text.pdf.PdfPTable(1)
        'aTable13.DefaultCell.Border = Rectangle.NO_BORDER
        'Dim Cell5 = New PdfPCell(New Paragraph("Declaro que cumplo los requisitos Del Regimen Simplificado según articulo 511 E.T. No estoy Obligado a Expedir Factura", FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.NORMAL)))
        'Cell5.Colspan = 3
        ''Cell5.Border = 7
        'Cell5.HorizontalAlignment = 1
        'aTable13.AddCell(Cell5)
        'Dim aTable14 = New iTextSharp.text.pdf.PdfPTable(1)
        'aTable14.DefaultCell.Border = Rectangle.NO_BORDER
        'Dim Cell6 = New PdfPCell(New Paragraph("DOCUMENTO DE OBLIGATORIO CUMPLIMIENTO PARA OPERACIONES CON PERSONAS NATURALES NO COMERCIANTES NO INSCRITOS EN EL REGIMEN SIMPLIFICADO ART. 3 DEC. 522 DE 2003 ", FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.NORMAL)))
        'Cell6.Colspan = 3
        '' Cell6.Border = 7
        'Cell6.HorizontalAlignment = 1
        'aTable14.AddCell(Cell6)
        'Dim aTable15 = New iTextSharp.text.pdf.PdfPTable(2)
        'aTable15.DefaultCell.Border = Rectangle.NO_BORDER
        'aTable15.AddCell(New Paragraph("PERSONA NATURAL (Vendedor)", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        'Dim Imagen2 As iTextSharp.text.Image
        'Imagen2 = iTextSharp.text.Image.GetInstance(path & "logo-titulo.png")
        'Imagen2.ScalePercent(10)
        'Dim Img2 = New PdfPCell
        'Img2.Border = 0
        'Img2.AddElement(Imagen2)
        'aTable15.AddCell(Img2)
        'Dim aTable16 = New iTextSharp.text.pdf.PdfPTable(1)
        'aTable16.DefaultCell.Border = Rectangle.NO_BORDER
        'aTable16.AddCell(New Paragraph("Firma:_____________________________", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        'aTable16.AddCell(New Paragraph("NIT O C.C", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

        'documentoPDF.Add(aTable)
        'documentoPDF.Add(aTable1)
        'documentoPDF.Add(aTable2)
        'documentoPDF.Add(aTable3)
        'documentoPDF.Add(aTable4)
        'documentoPDF.Add(aTable5)
        'documentoPDF.Add(aTable6)
        'documentoPDF.Add(aTable7)
        'documentoPDF.Add(aTable8)
        'documentoPDF.Add(aTable9)
        'documentoPDF.Add(aTable10)
        'documentoPDF.Add(aTable12)
        'documentoPDF.Add(aTable11)
        'documentoPDF.Add(aTable13)
        'documentoPDF.Add(aTable14)
        'documentoPDF.Add(aTable15)
        'documentoPDF.Add(aTable16)

        documentoPDF.NewPage()
        Dim aTable55 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable55.DefaultCell.Border = iTextSharp.text.Rectangle.NO_BORDER
        aTable55.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        aTable55.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        aTable55.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        aTable55.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        Dim Cell55 = New PdfPCell(New Paragraph("CONTRATO DE TRANSACCION", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD)))
        Cell55.HorizontalAlignment = 1
        aTable55.AddCell(Cell55)
        'CellBorders(aTable.DefaultCell, 1, 1, 1, 1)
        Dim aTable155 = New iTextSharp.text.pdf.PdfPTable(1)
        Dim C155 = New PdfPCell(New Paragraph("Entre IPSOS NAPOLEON FRANCO & CIA S.A.S., sociedad con domicilio en Bogotá D.C., representada en este contrato por Clemencia Moreno Sánchez, quien ocupa el cargo de " & "Representante y Apoderado" & " y por tanto representante del empleador en los términos del artículo 32 del Código Sustantivo de Trabajo, facultado por el representante legal según poder conferido para estos efectos, domiciliado y residente en Bogotá, D.C.,  identificado como aparece al pie de su firma, quien para los efectos del presente documento se denominará LA EMPRESA y " & UCase(Persona.Nombres) & " " & UCase(Persona.Apellidos) & ", igualmente mayor de edad, domiciliado y residente en " & Persona.Ciudad & ", identificado como aparece al pie de su firma, actuando en su propio nombre, quien para los efectos del presente documento se denominará EL CONTRATISTA, se ha celebrado un contrato de transacción de carácter laboral conforme se deja constancia en las siguientes cláusulas:", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C155.HorizontalAlignment = 3
        aTable155.AddCell(C155)
        Dim C5055 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        aTable155.AddCell(C5055)
        aTable155.DefaultCell.Border = BorderStyle.NotSet
        Dim aTable255 = New iTextSharp.text.pdf.PdfPTable(1)

        Dim FechaRet As String

        If Persona.FechaRetiro Is Nothing Then
            FechaRet = FecUltimaCC.Item(t - 1).Fecha
        Else
            FechaRet = Date.UtcNow.AddHours(-5)
        End If

        Dim C355 = New PdfPCell(New Paragraph("PRIMERA: Las partes están de acuerdo en los siguientes hechos: EL CONTRATISTA prestó servicios técnicos de encuestador a favor de LA EMPRESA durante el periodo transcurrido entre el día " & txtFechaIngreso.Text & ", y el día " & txtFechaRetiro.Text & ". Para tal efecto acordaron que los servicios quedarían estructurados sobre la base de que no existiría relación laboral que implicara la existencia de un contrato de trabajo, suscribiendo documentos contractuales en ese sentido. Para cobrar sus servicios EL CONTRATISTA presentaba mensualmente cuentas de cobro. Su remuneración estuvo determinada por tarea o productividad estableciéndose una suma de dinero por encuesta efectivamente realizada. El vínculo finalizó por mutuo consentimiento de las partes. EL CONTRATISTA estuvo afiliado al sistema de seguridad social previsto para los trabajadores independientes.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C355.HorizontalAlignment = 3
        aTable255.DefaultCell.Border = BorderStyle.NotSet
        aTable255.AddCell(C355)
        Dim aTable355 = New iTextSharp.text.pdf.PdfPTable(1)
        Dim Cell155 = New PdfPCell(New Paragraph("SEGUNDA: Las partes reconocen que podría ser discutible la naturaleza del vínculo que existió entre ellas pues si bien para la EMPRESA es claro que dicho vínculo fue como lo pactaron por escrito las partes y que el CONTRATISTA no estuvo subordinado, este último  podría aducir que realmente hubo una relación laboral y consecuencialmente la posibilidad de que se causaran las prestaciones sociales, vacaciones y demás conceptos propios de un contrato de trabajo.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        aTable355.DefaultCell.Border = BorderStyle.NotSet
        aTable355.AddCell(Cell155)
        Dim aTable455 = New iTextSharp.text.pdf.PdfPTable(1)
        ' aTable4.DefaultCell.Border = BorderStyle.Double

        If Descuento.Count - 1 Then
            Dim C655 = New PdfPCell(New Paragraph("TERCERA: También las partes registran que la EMPRESA puede reclamar al CONTRATISTA recursos suministrados con ocasión de los servicios prestados por valor de $  " & "0" & " Dicha suma puede ser compensada frente a obligaciones reclamables por el CONTRATISTA.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            C655.HorizontalAlignment = 3
            aTable455.AddCell(C655)
            'C6.Border = BorderStyle.None
        Else
            Dim C655 = New PdfPCell(New Paragraph("TERCERA: También las partes registran que la EMPRESA puede reclamar al CONTRATISTA recursos suministrados con ocasión de los servicios prestados por valor de   " & FormatCurrency(Descuento.Item(0).Total, 0) & " Dicha suma puede ser compensada frente a obligaciones reclamables por el CONTRATISTA.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            C655.HorizontalAlignment = 3
            aTable455.AddCell(C655)
        End If

        Dim C755 = New PdfPCell(New Paragraph("CUARTA: Las partes de común acuerdo desean transar sus diferencias con el propósito de evitar litigios futuros entre ellas, acordando los siguientes aspectos:", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C755.HorizontalAlignment = 3
        aTable455.DefaultCell.Border = BorderStyle.NotSet
        aTable455.AddCell(C755)
        Dim aTable555 = New iTextSharp.text.pdf.PdfPTable(1)
        'aTable5.DefaultCell.Border = BorderStyle.Double
        Dim C1055 = New PdfPCell(New Paragraph("a) " & " " & " Para todos los efectos legales se entenderá que la vinculación finalizó por mutuo acuerdo de las partes.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C1055.HorizontalAlignment = 3
        'C10.Border = BorderStyle.None
        Dim C1155 = New PdfPCell(New Paragraph("b) " & " " & " EL CONTRATISTA recibirá la suma de " & FormatCurrency((total - sub1 - WDescuento), 0) & "", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C1155.HorizontalAlignment = 3
        'C11.Border = BorderStyle.None
        Dim C1255 = New PdfPCell(New Paragraph("c) " & " " & " El pago se realizará dentro de los 10 días siguientes a la fecha de suscripción del presente documento, mediante consignación o transferencia a la cuenta de ahorros del CONTRATISTA, Banco " & Persona.Banco & ", Cuentan Nº " & Persona.CuentaBanco & ".  ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C1255.HorizontalAlignment = 3
        'C12.Border = BorderStyle.None
        Dim C1355 = New PdfPCell(New Paragraph("d) " & " " & " Con el pago de las suma a que se refiere la presente transacción, LA EMPRESA quedará a paz y salvo con EL CONTRATISTA por cualquier  concepto  que pudiere reclamar derivado de los servicios prestados, particularmente salarios, cesantías, intereses a las cesantías, primas de servicio, vacaciones e  indemnizaciones de todo tipo así como aportes sistema de seguridad social integral.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C1355.HorizontalAlignment = 3
        'C13.Border = BorderStyle.None
        Dim C1455 = New PdfPCell(New Paragraph("e) " & " " & " Para todos los efectos legales EL CONTRATISTA declara y acepta que la EMPRESA actuó de buena fe durante la ejecución del contrato.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C1455.HorizontalAlignment = 3
        'C14.Border = BorderStyle.None
        aTable555.DefaultCell.Border = Rectangle.NO_BORDER
        aTable555.AddCell(C1055)
        aTable555.AddCell(C1155)
        aTable555.AddCell(C1255)
        aTable555.AddCell(C1355)
        aTable555.AddCell(C1455)
        Dim aTable655 = New iTextSharp.text.pdf.PdfPTable(1)
        'aTable6.DefaultCell.Border = BorderStyle.Double
        Dim Cell255 = New PdfPCell(New Paragraph("QUINTA:   Las partes se declaran enteradas de que la presente transacción produce efectos de cosa juzgada y que por tanto EL CONTRATISTA no podrá iniciar acciones administrativas o judiciales tendientes a reclamar los puntos transados.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        aTable655.DefaultCell.Border = Rectangle.NO_BORDER
        Cell255.Colspan = 3
        Cell255.HorizontalAlignment = 3
        aTable655.AddCell(Cell255)
        Dim aTable755 = New iTextSharp.text.pdf.PdfPTable(1)
        Dim C1655 = New PdfPCell(New Paragraph("SEXTA: Las partes dejan constancia que la presente transacción laboral ha sido realizada conforme lo autoriza el artículo 15 del Código Sustantivo del Trabajo en concordancia con el artículo 2.469 del Código Civil.  Para constancia se firma:" & Now().Date & "", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C1655.HorizontalAlignment = 3
        aTable755.DefaultCell.Border = Rectangle.NO_BORDER
        aTable755.AddCell(C1655)
        aTable755.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        aTable755.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        aTable755.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

        Dim aTable188 = New iTextSharp.text.pdf.PdfPTable(2)

        aTable188.DefaultCell.Border = Rectangle.NO_BORDER
        aTable188.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
        'Dim Imagen4 As iTextSharp.text.Image
        'Imagen4 = iTextSharp.text.Image.GetInstance(path & "Firma1.jpg")
        'Imagen4.ScalePercent(75)
        'Dim Img4 = New PdfPCell
        'Img4.Border = 0
        'Img4.AddElement(Imagen4)
        'aTable188.AddCell(Img4)

        Dim aTable855 = New iTextSharp.text.pdf.PdfPTable(2)
        aTable855.DefaultCell.Border = Rectangle.NO_BORDER
        Dim C2055 = New PdfPCell(New Paragraph("EL CONTRATISTA,", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C2055.HorizontalAlignment = 3
        Dim C2155 = New PdfPCell(New Paragraph("LA EMPRESA,", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C2155.HorizontalAlignment = 3
        Dim C2255 = New PdfPCell(New Paragraph(Persona.Nombres & " " & Persona.Apellidos, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C2255.HorizontalAlignment = 3
        Dim C2355 = New PdfPCell(New Paragraph("Clemencia Moreno Sánchez", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C2355.HorizontalAlignment = 3
        Dim C2455 = New PdfPCell(New Paragraph("CC " & Persona.id, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C2455.HorizontalAlignment = 3
        Dim C2555 = New PdfPCell(New Paragraph("CC 52829702 ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C2555.HorizontalAlignment = 3
        Dim C2755 = New PdfPCell(New Paragraph("Representante y Apoderado IPSOS NAPOLEON FRANCO & CIA S.AS.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        C2755.HorizontalAlignment = 3
        Dim Imagen55 As iTextSharp.text.Image
        Imagen55 = iTextSharp.text.Image.GetInstance(path & "Ipsos.png")
        Imagen55.ScalePercent(10)
        Dim Img55 = New PdfPCell
        'Img.Border = 0
        Img55.Border = BorderStyle.None
        Img55.AddElement(Imagen55)
        aTable855.AddCell(C2055)
        aTable855.AddCell(C2155)
        aTable855.AddCell(C2255)
        aTable855.AddCell(C2355)
        aTable855.AddCell(C2455)
        aTable855.AddCell(C2555)
        aTable855.AddCell(Img55)
        aTable855.AddCell(C2755)
        documentoPDF.Add(aTable55)
        documentoPDF.Add(aTable155)
        documentoPDF.Add(aTable255)
        documentoPDF.Add(aTable355)
        documentoPDF.Add(aTable455)
        documentoPDF.Add(aTable555)
        documentoPDF.Add(aTable655)
        documentoPDF.Add(aTable755)
        documentoPDF.Add(aTable188)
        documentoPDF.Add(aTable855)

        op.CuentasdeCobro(1, Persona.id, FormatCurrency(total - sub1), Session("IDUsuario"))

        'Añadimos los metadatos para el fichero PDF
        documentoPDF.AddAuthor(Session("IDUsuario").ToString)
        documentoPDF.AddTitle("Cuenta-Bono")
        documentoPDF.AddCreationDate()
        documentoPDF.Close() 'Cerramos el objeto documento, guardamos y creamos el PDF

        'Comprobamos si se ha creado el fichero PDF
        If System.IO.File.Exists(urlFija & "\" & "CuentadeCobro-Bonificacion-" & Cedula & ".pdf") Then
            'ShowNotification("Archivo Generado", ShowNotifications.InfoNotification)
            'Response.Redirect("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=3" & "&Id=" & Persona.id & "&TrabajoId=1")
            ShowWindows("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=3" & "&Id=" & Persona.id & "&TrabajoId=1")
        Else
            ShowNotification("El fichero PDF no se ha generado, compruebe que tiene permisos en la carpeta de destino.", ShowNotifications.InfoNotification)
        End If

    End Sub
    Public Function Informacionpersonas(ByVal Cedula As Int64)
        ' Dim e As New CoreProject.TH_PersonasGET_Result Revisar ajuste Cambio
        Dim e As New TH_PersonasGET_Result
        Dim o As New CoreProject.RegistroPersonas
        e = o.TH_PersonasGet(Cedula, Nothing).FirstOrDefault
        Return e
    End Function

    Private Sub GenerarBonificacion_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnGenerarPDF)
    End Sub

#Region "FuncionesPDF"
    Public Shared Function FNumero(ByVal Valor As Double) As String
        FNumero = FormatNumber(Valor, 0, , , TriState.UseDefault)
    End Function

    Sub CellBorders(ByRef Celda As PdfPCell, ByVal _Top As Single, ByVal _Rigth As Single, ByVal _Bottom As Single, ByVal _Left As Single)
        Celda.BorderWidthBottom = _Bottom - 0.5
        Celda.BorderWidthLeft = _Left - 0.5
        Celda.BorderWidthRight = _Rigth - 0.5
        Celda.BorderWidthTop = _Top - 0.5
        Dim color As New iTextSharp.text.BaseColor(0, 0, 0) '(100, 100, 100, 100) '
        Celda.BorderColor = color
    End Sub

    Public Shared Function Ssg(ByVal Unidad As Double) As Single
        Ssg = Math.Round((652 * Unidad) / 21.59, 0)
    End Function

    Function NewPdfCell(ByVal paragr As String, ByVal alineacion As Integer, Optional ByVal bold As Boolean = False, Optional ByVal bordes As Boolean = False, Optional ByVal ColSpan As Integer = 1) As PdfPCell
        Dim Font1 As iTextSharp.text.Font = FontFactory.GetFont("Century", 7, Font.BOLD)
        Dim Font2 As iTextSharp.text.Font = FontFactory.GetFont("Century", 7, Font.NORMAL)
        Dim PdfCell = New PdfPCell
        If bordes = True Then
            CellBorders(PdfCell, 1, 1, 1, 1)
        Else
            CellBorders(PdfCell, 0, 0, 0, 0)
        End If

        PdfCell.HorizontalAlignment = alineacion
        Dim f1 = New Font(Font1)
        Dim f2 = New Font(Font2)
        Dim Parag = New Paragraph(paragr, IIf(bold = True, f1, f2))
        Parag.Leading = 8
        Parag.Alignment = alineacion
        PdfCell.AddElement(Parag)
        PdfCell.PaddingBottom = 1.5
        PdfCell.Colspan = ColSpan
        NewPdfCell = PdfCell
    End Function

    Function NewPdfCellCh(ByVal paragr As String, ByVal alineacion As Integer, Optional ByVal bold As Boolean = False, Optional ByVal bordes As Boolean = False) As PdfPCell
        Dim Font1 As iTextSharp.text.Font = FontFactory.GetFont("Century", 6, Font.BOLD)
        Dim Font2 As iTextSharp.text.Font = FontFactory.GetFont("Century", 6.5, Font.NORMAL)
        Dim PdfCell = New PdfPCell
        If bordes = True Then
            CellBorders(PdfCell, 1, 1, 1, 1)
        Else
            CellBorders(PdfCell, 0, 0, 0, 0)
        End If

        PdfCell.HorizontalAlignment = alineacion
        Dim f1 = New Font(Font1)
        Dim f2 = New Font(Font2)
        Dim Parag = New Paragraph(paragr, IIf(bold = True, f1, f2))
        Parag.Leading = 6.5
        Parag.Alignment = alineacion
        PdfCell.AddElement(Parag)
        PdfCell.PaddingBottom = 1.5
        NewPdfCellCh = PdfCell
    End Function

    Function NewPdfCellGray(ByVal paragr As String, ByVal alineacion As Integer, Optional ByVal bold As Boolean = False, Optional ByVal bordes As Boolean = False) As PdfPCell
        Dim color As New iTextSharp.text.BaseColor(System.Drawing.Color.Navy)
        Dim Font1 As iTextSharp.text.Font = FontFactory.GetFont("Century", 9, Font.BOLD, color)
        Dim Font2 As iTextSharp.text.Font = FontFactory.GetFont("Century", 9, Font.NORMAL, color)
        Dim PdfCell = New PdfPCell
        If bordes = True Then
            CellBorders(PdfCell, 1, 1, 1, 1)
        Else
            CellBorders(PdfCell, 0, 0, 0, 0)
        End If

        PdfCell.HorizontalAlignment = alineacion
        Dim colorb As New iTextSharp.text.BaseColor(System.Drawing.Color.LightGray)
        PdfCell.BackgroundColor = colorb
        Dim f1 = New Font(Font1)
        Dim f2 = New Font(Font2)
        Dim Parag = New Paragraph(paragr, IIf(bold = True, f1, f2))
        Parag.Leading = 9.5
        Parag.Alignment = alineacion
        PdfCell.AddElement(Parag)
        NewPdfCellGray = PdfCell
    End Function

    Function NewPdfTitle(ByVal titulo As String) As PdfPTable
        Dim font As New Font(Font.FontFamily.HELVETICA, 8, 1, iTextSharp.text.BaseColor.BLACK)
        Dim CalFont1 As iTextSharp.text.Font = FontFactory.GetFont("Century", 8, Font.BOLD)
        Dim tabla = New PdfPTable(1)
        tabla.WidthPercentage = 100
        Dim PdfCell = New PdfPCell
        CellBorders(PdfCell, 1.5, 1.5, 1.5, 1.5)
        PdfCell.BorderColor = iTextSharp.text.BaseColor.BLACK
        font = New Font(CalFont1)
        Dim Parag = New Paragraph(titulo, font)
        Parag.Leading = 9
        Parag.Alignment = 2
        PdfCell.AddElement(Parag)
        tabla.AddCell(PdfCell)
        NewPdfTitle = tabla
    End Function

    Function NewRenglonEnter() As PdfPTable
        Dim tabla = New PdfPTable(1)
        tabla.WidthPercentage = 100
        Dim PdfCell = New PdfPCell
        Dim Parag = New Paragraph(" ")
        Parag.Leading = 10
        Parag.Alignment = 2
        PdfCell.AddElement(Parag)
        CellBorders(PdfCell, 0, 0, 0, 0)
        tabla.AddCell(PdfCell)
        NewRenglonEnter = tabla
    End Function

    Function EnterMin() As PdfPTable
        Dim tabla = New PdfPTable(1)
        tabla.WidthPercentage = 100
        Dim PdfCell = New PdfPCell
        Dim Parag = New Paragraph("")
        Parag.Leading = 1
        Parag.Alignment = 2
        PdfCell.AddElement(Parag)
        CellBorders(PdfCell, 0, 0, 0, 0)
        tabla.AddCell(PdfCell)
        EnterMin = tabla
    End Function

    Sub AddTable(ByRef tabla As PdfPTable, ByRef document As Document)
        document.Add(tabla)
        document.Add(EnterMin)
    End Sub

#End Region
End Class