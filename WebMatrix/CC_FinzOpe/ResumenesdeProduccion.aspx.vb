Imports WebMatrix
Imports CoreProject
Imports WebMatrix.Util
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class ResumenesdeProduccion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnGenerarPDF_Click(sender As Object, e As EventArgs) Handles btnGenerarPDF.Click
        Dim op As New ProcesosInternos
        If txtFechaInicio.Text = "" Or txtFechaFinalizacion.Text = "" Then
            ShowNotification("Ingrese informacion en todos los campos", ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        Else
            Dim cedula As Long? = Nothing
            If IsNumeric(TxtCedula.Text) Then cedula = TxtCedula.Text
            If op.ValidarProduccion(cedula, txtFechaInicio.Text, txtFechaFinalizacion.Text) = True Then
                MainPDF(cedula, txtFechaInicio.Text, txtFechaFinalizacion.Text)
                ActivateAccordion(0, EffectActivateAccordion.NoEffect)
                ShowNotification("Archivo Generado", ShowNotifications.InfoNotification)

            Else
                ShowNotification("No se Encontro Produccion Revise la Informacion de la Busqueda", ShowNotifications.ErrorNotification)
                ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            End If
        End If
    End Sub

    Public Function Informacionpersonas(ByVal Cedula As Int64)
        Dim e As New TH_PersonasGET_Result
        Dim o As New CoreProject.RegistroPersonas
        e = o.TH_PersonasGet(Cedula, Nothing).FirstOrDefault
        Return e
    End Function

    Sub MainPDF(ByVal cedula As Long?, ByVal Fecini As Date, FecFin As Date)
        Dim ot As New ProcesosInternos

        Dim urlFija As String
        Dim Url As String
        Dim path As String = Server.MapPath("~/Images/")

        urlFija = "~/ResumenProduccion/"
        Url = "..\ResumenProduccion\"
        urlFija = Server.MapPath(urlFija & "\")
        Dim pdfw As PdfWriter
        Dim documentoPDF As New Document(iTextSharp.text.PageSize.A4.Rotate(), 0, 0, 20, 20) 'Creamos el objeto documento PDF
        Dim namefile As String = "\" & "ResumenProduccion-" & Now.Month & Now.Day & Now.Hour & Now.Minute & ".pdf"
        pdfw = PdfWriter.GetInstance(documentoPDF, New FileStream(urlFija & "\" & namefile, FileMode.Create))
        documentoPDF.Open()

        If cedula IsNot Nothing Then
            Dim produccion = ot.ProduccionResumen(cedula, Fecini, FecFin)
            CrearResumen(documentoPDF, cedula, produccion)
        Else
            Dim listadopersonas = ot.ProduccionResumenPersonasProcInt(Fecini, FecFin)
            For i As Integer = 0 To listadopersonas.Count - 1
                Dim produccion = ot.ProduccionResumen(listadopersonas(i).Value, Fecini, FecFin)
                CrearResumen(documentoPDF, listadopersonas(i).Value, produccion)
            Next
        End If



        'Añadimos los metadatos para el fichero PDF
        documentoPDF.AddAuthor(Session("IDUsuario").ToString)
        documentoPDF.AddTitle("ResumenProduccion")
        documentoPDF.AddCreationDate()
        documentoPDF.Close() 'Cerramos el objeto documento, guardamos y creamos el PDF


        'Comprobamos si se ha creado el fichero PDF
        If System.IO.File.Exists(urlFija & namefile) Then
            'Response.Redirect("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=5" & "&Id=" & Persona.id & "&TrabajoId=1")
            ShowWindows("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=7" & "&Id=" & namefile)
        Else

            ShowNotification("El fichero PDF no se ha generado, compruebe que tiene permisos en la carpeta de destino.", ShowNotifications.InfoNotification)
        End If


    End Sub
    Sub CrearResumen(documentoPDF As Document, Cedula As Long, Produccion As List(Of CC_ProduccionResumenXCedula_Result))
        Dim total As Double = 0
        Dim path As String = Server.MapPath("~/Images/")
        Dim Persona = Informacionpersonas(Cedula)
        documentoPDF.NewPage()
        Dim aTable = New iTextSharp.text.pdf.PdfPTable(2)
        aTable.DefaultCell.Border = BorderStyle.None

        Dim Imagen As iTextSharp.text.Image
        Imagen = iTextSharp.text.Image.GetInstance(path & "logo-titulo.png")
        Imagen.ScalePercent(10)
        Dim Img = New PdfPCell
        Img.Border = 2
        Img.AddElement(Imagen)
        aTable.AddCell(Img)
        aTable.AddCell(New Paragraph("Fecha Generacion " & Now(), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))

        Dim aTable1 = New iTextSharp.text.pdf.PdfPTable(1)

        Dim C1 = New PdfPCell(New Paragraph("IPSOS NAPOLEON FRANCO & CIA S.A.S. - RESUMEN DE PRODUCCION", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        C1.HorizontalAlignment = 3
        aTable1.AddCell(C1)




        Dim aTable3 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable3.DefaultCell.Border = BorderStyle.Double
        Dim Cell1 = New PdfPCell(New Paragraph("Empleado: " & Persona.id & "-" & Persona.Nombres & " " & Persona.Apellidos & "-" & Persona.Ciudad, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        Cell1.Colspan = 3
        Cell1.HorizontalAlignment = 3
        aTable3.AddCell(Cell1)

        Dim aTable5 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable5.DefaultCell.Border = BorderStyle.Double
        Dim Cell2 = New PdfPCell(New Paragraph("Periodo Fecha Inicial:" & txtFechaInicio.Text & " Fecha Final: " & txtFechaFinalizacion.Text, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        Cell2.Colspan = 3

        Cell2.HorizontalAlignment = 3
        aTable5.AddCell(Cell2)


        Dim aTable8 = New iTextSharp.text.pdf.PdfPTable(4)
        aTable8.DefaultCell.Border = BorderStyle.Double

        Dim CL8 = New PdfPCell(New Paragraph("CONCEPTO", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL8.HorizontalAlignment = 1
        Dim CL9 = New PdfPCell(New Paragraph("CANTIDAD", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL9.HorizontalAlignment = 1
        Dim CL10 = New PdfPCell(New Paragraph("VALOR UNITARIO", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL10.HorizontalAlignment = 1
        Dim CL11 = New PdfPCell(New Paragraph("TOTAL", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL11.HorizontalAlignment = 1


        aTable8.AddCell(CL8)
        aTable8.AddCell(CL9)
        aTable8.AddCell(CL10)
        aTable8.AddCell(CL11)
        Dim Ancho As Single() = {3.25F, 0.449999988F, 0.449999988F, 0.449999988F}

        'Ciclo
        For i = 0 To Produccion.Count - 1

            Dim CL14 = New PdfPCell(New Paragraph(Produccion.Item(i).Nombre & " - Trabajo: " & Produccion.Item(i).NombreTrabajo & "-" & Produccion.Item(i).TrabajoId & " - Job: " & Produccion.Item(i).JobBook, FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.NORMAL)))
            CL14.HorizontalAlignment = 3
            Dim CL15 = New PdfPCell(New Paragraph(Produccion.Item(i).Cantida, FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.NORMAL)))
            CL15.HorizontalAlignment = 1
            Dim CL16 = New PdfPCell(New Paragraph(FormatCurrency(Produccion.Item(i).VrUnitario, 0), FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.NORMAL)))
            CL16.HorizontalAlignment = 1
            Dim CL17 = New PdfPCell(New Paragraph(FormatCurrency(Produccion.Item(i).Total, 0), FontFactory.GetFont(FontFactory.TIMES, 7, iTextSharp.text.Font.NORMAL)))
            CL17.HorizontalAlignment = 1
            aTable8.SetWidths(Ancho)

            aTable8.AddCell(CL14)
            aTable8.AddCell(CL15)
            aTable8.AddCell(CL16)
            aTable8.AddCell(CL17)
            TOTAL = TOTAL + Produccion.Item(i).Total
        Next

        Dim aTable9 = New iTextSharp.text.pdf.PdfPTable(4)


        Dim CL22 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL22.HorizontalAlignment = 2
        Dim CL23 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL23.HorizontalAlignment = 2


        Dim CL24 = New PdfPCell(New Paragraph("TOTAL:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL24.HorizontalAlignment = 2
        Dim CL25 = New PdfPCell(New Paragraph(FormatCurrency(TOTAL, 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL25.HorizontalAlignment = 1
        aTable9.AddCell(CL22)
        aTable9.AddCell(CL23)
        aTable9.AddCell(CL24)
        aTable9.AddCell(CL25)




        Dim aTable18 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable18.DefaultCell.Border = Rectangle.NO_BORDER
        aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

        Dim Cl32 = New PdfPCell(New Paragraph(Persona.Nombres & " " & Persona.Apellidos, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        Cl32.HorizontalAlignment = 3

        Dim Cl34 = New PdfPCell(New Paragraph("CC " & Persona.id, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        Cl34.HorizontalAlignment = 3

        aTable18.AddCell(Cl32)
        aTable18.AddCell(Cl34)


        documentoPDF.Add(aTable)
        documentoPDF.Add(aTable1)
        documentoPDF.Add(aTable3)
        documentoPDF.Add(aTable5)
        documentoPDF.Add(aTable8)
        documentoPDF.Add(aTable9)
        documentoPDF.Add(aTable18)

    End Sub

    Private Sub btnEnviarEmail_Click(sender As Object, e As EventArgs) Handles btnEnviarEmail.Click
        Dim op As New ProcesosInternos
        If txtFechaInicio.Text = "" Or txtFechaFinalizacion.Text = "" Then
            ShowNotification("Ingrese informacion en todos los campos", ShowNotifications.ErrorNotification)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        Else
            Dim cedula As Long? = Nothing
            If IsNumeric(TxtCedula.Text) Then cedula = TxtCedula.Text
            If op.ValidarProduccion(cedula, txtFechaInicio.Text, txtFechaFinalizacion.Text) = True Then
				MainEmail(cedula, txtFechaInicio.Text, txtFechaFinalizacion.Text)
				ActivateAccordion(0, EffectActivateAccordion.NoEffect)
                ShowNotification("Archivo Generado", ShowNotifications.InfoNotification)

            Else
                ShowNotification("No se Encontro Produccion Revise la Informacion de la Busqueda", ShowNotifications.ErrorNotification)
                ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            End If
        End If
    End Sub

	Sub MainEmail(ByVal cedula As Long?, ByVal Fecini As Date, FecFin As Date)
		Dim ot As New ProcesosInternos
		Dim listadopersonas As New List(Of Long?)

		Dim urlFija As String
		Dim Url As String
		Dim path As String = Server.MapPath("~/Images/")

		urlFija = "~/ResumenProduccion/"
		Url = "..\ResumenProduccion\"
		urlFija = Server.MapPath(urlFija & "\")
		Dim pdfw As PdfWriter


		If cedula IsNot Nothing Then
            listadopersonas.Add(cedula)
        Else
			listadopersonas = ot.ProduccionResumenPersonasProcInt(Fecini, FecFin)
		End If
        For i As Integer = 0 To listadopersonas.Count - 1
            Dim produccion = ot.ProduccionResumen(listadopersonas(i).Value, Fecini, FecFin)

            Dim documentoPDF As New Document(iTextSharp.text.PageSize.A4.Rotate(), 0, 0, 20, 20) 'Creamos el objeto documento PDF
            Dim namefile As String = "\" & "ResumenProduccion-" & listadopersonas(i).Value.ToString & "-" & Now.Month & Now.Day & Now.Hour & Now.Minute & ".pdf"
            pdfw = PdfWriter.GetInstance(documentoPDF, New FileStream(urlFija & "\" & namefile, FileMode.Create))
            documentoPDF.Open()

            CrearResumen(documentoPDF, listadopersonas(i).Value, produccion)

            'Añadimos los metadatos para el fichero PDF
            documentoPDF.AddAuthor(Session("IDUsuario").ToString)
            documentoPDF.AddTitle("ResumenProduccion")
            documentoPDF.AddCreationDate()
            documentoPDF.Close() 'Cerramos el objeto documento, guardamos y creamos el PDF
            'ShowWindows("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=7" & "&Id=" & namefile)
            Dim oEnviarCorreo As New EnviarCorreo
            Dim oempleados As New CoreProject.Empleados
            Dim empleado = oempleados.obtenerPorIdentificacion(listadopersonas(i).Value)
            Dim contenido As String = ""
            contenido += "<p>Señor(a)</p>"
            contenido += "<p>" & empleado.Nombres & " " & empleado.Apellidos & "</p><br/><br/>"
            contenido += "<p>Por medio de la presente le enviamos la relación de la productividad reportada en el periodo de:</p>"
			contenido += "<p>Fecha inicial: " & Fecini.Date.ToString("d") & "</p>"
			contenido += "<p>Fecha final: " & FecFin.Date.ToString("d") & "</p><br/><br/><br/><br/>"
			contenido += "<p>Saludos</p><br/><br/><br/>"
			contenido += "<p>Yulieth Acosta Lopez</p>"
			contenido += "<p>Finanzas</p>"
			contenido += "<p>Ipsos</p>"
			Dim destinatarios As New List(Of String)
			Dim destinatarioscopia As New List(Of String)
			If empleado.correoIpsos IsNot Nothing Then
				destinatarios.Add(empleado.correoIpsos)
			Else
				destinatarios.Add(empleado.EmailPersonal)
            End If
            destinatarios.Add("matrix@ipsos.com")
            destinatarioscopia.Add("Yulieth.Acosta@ipsos.com")
            destinatarioscopia.Add("Juliana.Santos01@ipsos.com")
            Dim attachFile As New System.Net.Mail.Attachment(urlFija & "\" & namefile, System.Net.Mime.MediaTypeNames.Application.Pdf)
			oEnviarCorreo.sendMailWithAttachment(destinatarios, destinatarioscopia, "Reporte Productividad", contenido, attachFile)
        Next
        ShowNotification("Los correos han sido enviados", ShowNotifications.InfoNotification)
	End Sub
End Class