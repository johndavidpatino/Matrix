Imports CoreProject
Imports WebMatrix.Util
Imports iTextSharp
Imports iTextSharp.text.pdf
Imports System.IO
Imports iTextSharp.text
Imports System.Resources
Imports System.Globalization
Imports System.Threading
Public Class CuentasdeCobro
    Inherits System.Web.UI.Page
    Dim GTotalProduccion As Double = 0

    Protected Sub btnCargarFiltros_Click(sender As Object, e As EventArgs) Handles btnCargarFiltros.Click
        cargarCiudadesConProduccion(txtFechaInicio.Text, txtFechaFinalizacion.Text)
        cargarTiposEncuestadorConProduccion(txtFechaInicio.Text, txtFechaFinalizacion.Text)
        cargarCedulasConProduccion(txtFechaInicio.Text, txtFechaFinalizacion.Text)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        Dim tipoEncuestador As PresupInt.ETipoEncuestador?
        Dim ciudad As Long?
        Dim cedula As Long?
        If ddlTiposEncuestador.SelectedValue <> "-1" AndAlso ddlTiposEncuestador.SelectedValue <> "7" Then tipoEncuestador = ddlTiposEncuestador.SelectedValue
        If ddlciudades.SelectedValue <> "-1" Then ciudad = ddlciudades.SelectedValue
        If ddlCedulas.SelectedValue <> "-1" Then cedula = ddlCedulas.SelectedValue

        gvProduccion.Caption = "Producción " & ddlciudades.SelectedItem.Text & ", de " & txtFechaInicio.Text & " a " & txtFechaFinalizacion.Text
        produccion(txtFechaInicio.Text, txtFechaFinalizacion.Text, cedula, ciudad, tipoEncuestador)
    End Sub
    Protected Sub btnCuentas_Click(sender As Object, e As EventArgs) Handles btnCuentas.Click
        If Not (IsDate(txtFechaFactura.Text)) Then
            ShowNotification("Debe seleccionar la fecha de facturación", ShowNotifications.ErrorNotification)
            ActivateAccordion(0)
            Exit Sub
        End If
        Dim worker As New BackgroundWorker()
        Dim GuidProceso As String = Guid.NewGuid.ToString
        Dim Cedulas As List(Of CC_CedulasProduccion_Result)
        Dim fecIni As Date
        Dim fecFin As Date
        Dim tipoEncuestador As PresupInt.ETipoEncuestador?
        Dim cedulaConProduccion As Long?
        Dim ciudadConProduccion As Long?
        AddHandler worker.DoWork, AddressOf worker_DoWork

        fecIni = Date.Parse(txtFechaInicio.Text).ToString("dd/MM/yyyy")
        fecFin = Date.Parse(txtFechaFinalizacion.Text).ToString("dd/MM/yyyy")

        Dim opr As New PresupInt

        If ddlTiposEncuestador.SelectedValue <> "-1" AndAlso ddlTiposEncuestador.SelectedValue <> "7" Then tipoEncuestador = ddlTiposEncuestador.SelectedValue
        If ddlCedulas.SelectedValue <> "-1" Then cedulaConProduccion = ddlCedulas.SelectedValue
        If ddlciudades.SelectedValue <> "-1" Then ciudadConProduccion = ddlciudades.SelectedValue

        Cedulas = opr.CedulasProduccion(fecIni, fecFin, ciudadConProduccion, 7, cedulaConProduccion, tipoEncuestador).ToList

        'CrearPdf(Cedulas, 0, GuidProceso, fecIni, fecFin, tipoEncuestador, ciudadConProduccion)
        lblGTotalProduccion.Text = GTotalProduccion

        Session("worker") = worker
        Session("Guid") = GuidProceso

        worker.RunWorker({Cedulas, GuidProceso, fecIni, fecFin, tipoEncuestador, ciudadConProduccion})

        lblCantidadOrdenesGenerar.Text = Cedulas.Count
        lblProgreso.Text = "0"
        Timer1.Enabled = True
        btnCuentas.Enabled = False
        lblEstadp.Text = "Iniciado"
    End Sub

    Public Function Informacionpersonas(ByVal Cedula As Int64)
        ' Dim e As New CoreProject.TH_PersonasGET_Result Revisar ajuste Cambio
        Dim e As New TH_PersonasGET_Result
        Dim o As New CoreProject.RegistroPersonas
        e = o.TH_PersonasGet(Cedula, Nothing).FirstOrDefault
        Return e
    End Function

    Public Function InformacionTrabajo(ByVal Id As Int64)
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo = oTrabajo.ObtenerTrabajo(Id)
        Return oeTrabajo
    End Function
    Sub cargarCiudadesConProduccion(ByVal FecIni As Date, ByVal FecFin As Date)
        Dim op As New PresupInt
        ddlciudades.DataSource = op.ProduccionCiudades(FecIni, FecFin)
        ddlciudades.DataValueField = "CiudadId"
        ddlciudades.DataTextField = "DivMuniNombre"
        ddlciudades.DataBind()
        ddlciudades.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
    End Sub
    Sub cargarTiposEncuestadorConProduccion(fechaInicio As Date, fechaFin As Date)
        Dim DA As New PresupInt
        Dim lst As New List(Of CC_ProduccionTiposEncuestador_Result)
        lst = DA.tiposEncuestadorConProduccion(fechaInicio, fechaFin)
        ddlTiposEncuestador.DataSource = lst
        ddlTiposEncuestador.DataValueField = "Id"
        ddlTiposEncuestador.DataTextField = "Tipo"
        ddlTiposEncuestador.DataBind()
        ddlTiposEncuestador.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
    End Sub
    Sub cargarCedulasConProduccion(fechaInicio As Date, fechaFin As Date)
        Dim DA As New PresupInt
        Dim lst As New List(Of CC_CedulasProduccion_Result)
        lst = DA.CedulasProduccion(fechaInicio, fechaFin, Nothing, 7, Nothing, Nothing)
        Dim lstDepurada = lst.GroupBy(Function(x) New With {Key x.PersonaId, Key .Nombres = (x.PersonaId & "-" & x.Nombres)}).Select(Function(x) x.Key).ToList
        ddlCedulas.DataSource = lstDepurada
        ddlCedulas.DataValueField = "personaId"
        ddlCedulas.DataTextField = "Nombres"
        ddlCedulas.DataBind()
        ddlCedulas.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
    End Sub
    Sub produccion(ByVal Fecini As Date, ByVal Fecfin As Date, ByVal Cedula As Int64?, ByVal Ciudad As Int64?, tipoEncuestador As PresupInt.ETipoEncuestador?)
        Dim po As New PresupInt
        Me.gvProduccion.DataSource = po.ProduccionXCiudad(Fecini, Fecfin, Cedula, Ciudad, tipoEncuestador)
        Me.gvProduccion.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub


    Sub CrearPdf(ByVal Cedulas As List(Of CC_CedulasProduccion_Result), ByRef progress As Integer, ByVal Guid As String, ByVal Fecini As Date, ByVal Fecfin As Date, tipoEncuestador As PresupInt.ETipoEncuestador?, ciudad As Long?)
        Dim total As Double
        Dim otros As Double
        Dim vrICA As Double
        Dim cantidad As Double
        Dim Mayor As Double = 0
        Dim trabajo1 As String = ""
        Dim job1 As String = ""
        Dim diastrabajados As Double
        Dim op As New PresupInt
        Dim Consecutivo As List(Of CC_ConsecutivoCC_Result)

        Dim urlFija As String
        Dim Url As String

        'Dim path As String = Server.MapPath("~/Images/")

        'Dim Ancho As Single() = {Ssg(4), Ssg(9), Ssg(4), Ssg(4)}
        'Dim Url As String
        urlFija = "~/Cuentasdecobro/"
        Url = "..\Cuentasdecobro\"
        'Url = hfIdTrabajo.Value
        'HACK Deuda tecnica Aquí se deberia colocar la ruta raíz en un parametro en la base de datos
        urlFija = Server.MapPath(urlFija & "\")
        'Dim Persona As New CoreProject.TH_PersonasGET_Result Revisar Ajuste
        Dim Persona As New TH_PersonasGET_Result
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
        pdfw = PdfWriter.GetInstance(documentoPDF, New FileStream(urlFija & "\" & "CuentasdeCobro-" & Guid & ".pdf", FileMode.Create))
        documentoPDF.Open()

        For i = 0 To Cedulas.Count - 1
            Dim po As New PresupInt
            Dim pi As New ProcesosInternos
            Dim o = po.ProduccionXCiudad(Fecini, Fecfin, Cedulas(i).PersonaId, ciudad, tipoEncuestador)

            progress = i + 1

            If o.Count > 0 Then
                Persona = Informacionpersonas(Cedulas.Item(i).PersonaId)
                Consecutivo = op.ConsecutivoCC
                'oeTrabajo = InformacionTrabajo(gvProduccion.Rows(i).Cells(1).Text)
                If Consecutivo.Item(0).Numero > 0 Then
                    pi.ActualizarProduccion(Fecini, Fecfin, Cedulas(i).PersonaId, Nothing, Consecutivo.Item(0).Numero + 1)
                End If

                documentoPDF.NewPage()

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

                Dim FechaFactura As Date = Date.Parse(txtFechaFactura.Text).ToString("dd/MM/yyyy")

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

                tabla = New PdfPTable(6)
                CellBorders(PdfCell, 1, 1, 1, 1)
                Ancho = {Ssg(3), Ssg(5), Ssg(6), Ssg(2), Ssg(3), Ssg(3)}
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
                Parag = New Paragraph("VALOR UNITARIO", font8B)
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
                total = 0
                cantidad = 0
                otros = 0
                Mayor = 0

                For x = 0 To gvProduccion.Rows.Count - 1

                    Dim oTrabajo As New Trabajo
                    Dim oeTrabajo = oTrabajo.ObtenerTrabajo(gvProduccion.Rows(x).Cells(1).Text)


                    If gvProduccion.Rows(x).Cells(0).Text = Persona.id Then
                        cantidad = gvProduccion.Rows(x).Cells(2).Text
                        If cantidad > Mayor Then
                            Mayor = cantidad
                            job1 = oeTrabajo.JobBook
                            trabajo1 = oeTrabajo.NombreTrabajo
                        Else
                            cantidad = 0
                        End If

                        PdfCell = New PdfPCell
                        Parag = New Paragraph(oeTrabajo.JobBook, font8)
                        Parag.Alignment = Element.ALIGN_CENTER
                        Parag.SetLeading(0.5F, 1.0F)
                        PdfCell.AddElement(Parag)
                        tabla.AddCell(PdfCell)

                        PdfCell = New PdfPCell
                        Parag = New Paragraph(oeTrabajo.NombreTrabajo, font7)
                        Parag.Alignment = Element.ALIGN_CENTER
                        Parag.SetLeading(0.5F, 1.0F)
                        PdfCell.AddElement(Parag)
                        tabla.AddCell(PdfCell)

                        PdfCell = New PdfPCell
                        Dim conceptoTrabajo As String = "Trabajo Campo Nacional"
                        If oeTrabajo.PresupuestoId = -100 Then
                            conceptoTrabajo = "Procesamiento de información encuestas"
                        End If
                        Parag = New Paragraph(conceptoTrabajo, font7)
                        Parag.Alignment = Element.ALIGN_CENTER
                        Parag.SetLeading(0.5F, 1.0F)
                        PdfCell.AddElement(Parag)
                        tabla.AddCell(PdfCell)

                        PdfCell = New PdfPCell
                        Parag = New Paragraph(gvProduccion.Rows(x).Cells(2).Text, font8)
                        Parag.Alignment = Element.ALIGN_CENTER
                        Parag.SetLeading(0.5F, 1.0F)
                        PdfCell.AddElement(Parag)
                        tabla.AddCell(PdfCell)

                        PdfCell = New PdfPCell
                        'Parag = New Paragraph(String.Format("{0:C0}", Double.Parse(gvProduccion.Rows(x).Cells(3).Text)), font8)
                        Parag = New Paragraph(Double.Parse(gvProduccion.Rows(x).Cells(3).Text).ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
                        Parag.Alignment = Element.ALIGN_CENTER
                        Parag.SetLeading(0.5F, 1.0F)
                        PdfCell.AddElement(Parag)
                        tabla.AddCell(PdfCell)

                        PdfCell = New PdfPCell
                        'Parag = New Paragraph(String.Format("{0:C0}", Double.Parse(gvProduccion.Rows(x).Cells(4).Text)), font8)
                        Parag = New Paragraph(Double.Parse(gvProduccion.Rows(x).Cells(4).Text).ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
                        Parag.Alignment = Element.ALIGN_CENTER
                        Parag.SetLeading(0.5F, 1.0F)
                        PdfCell.AddElement(Parag)
                        tabla.AddCell(PdfCell)

                        total = total + gvProduccion.Rows(x).Cells(4).Text
                        GTotalProduccion = GTotalProduccion + gvProduccion.Rows(x).Cells(4).Text  'Para total de control del proceso
                        diastrabajados = gvProduccion.Rows(x).Cells(8).Text
                        otros = gvProduccion.Rows(x).Cells(9).Text
                        vrICA = gvProduccion.Rows(x).Cells(10).Text

                        ' ### Para requerimiento de Servicio
                        Dim CL9 = New PdfPCell(New Paragraph(oeTrabajo.JobBook, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                        CL9.HorizontalAlignment = 1
                        Dim CL10 = New PdfPCell(New Paragraph(oeTrabajo.NombreTrabajo, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                        CL10.HorizontalAlignment = 1
                        Dim CL11 = New PdfPCell(New Paragraph(gvProduccion.Rows(x).Cells(3).Text, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                        CL11.HorizontalAlignment = 1
                        aTable19.AddCell(CL9)
                        aTable19.AddCell(CL10)
                        aTable19.AddCell(CL11)
                        ' ### End Req Servicio
                    End If


                Next

                Dim Valor As Double = 0
                Dim totalauxilio As Double = 0
                Dim oIquoteGeneral As New IQ.IquoteGeneral

                '## Se valida que el tipo de encuestador no sea Mistery para agregar item de transporte
                '## Para item de transporte
                '## tipoEncuestador 6 = Mistery
                If tipoEncuestador <> 6 Then
                    PdfCell = New PdfPCell
                    Parag = New Paragraph(job1, font8)
                    Parag.Alignment = Element.ALIGN_CENTER
                    Parag.SetLeading(0.5F, 1.0F)
                    PdfCell.AddElement(Parag)
                    tabla.AddCell(PdfCell)

                    PdfCell = New PdfPCell
                    Parag = New Paragraph(trabajo1, font7)
                    Parag.Alignment = Element.ALIGN_CENTER
                    Parag.SetLeading(0.5F, 1.0F)
                    PdfCell.AddElement(Parag)
                    tabla.AddCell(PdfCell)

                    PdfCell = New PdfPCell
                    Parag = New Paragraph("Transporte trabajo campo nacional", font7)
                    Parag.Alignment = Element.ALIGN_CENTER
                    Parag.SetLeading(0.5F, 1.0F)
                    PdfCell.AddElement(Parag)
                    tabla.AddCell(PdfCell)

                    PdfCell = New PdfPCell
                    Parag = New Paragraph(diastrabajados, font8)
                    Parag.Alignment = Element.ALIGN_CENTER
                    Parag.SetLeading(0.5F, 1.0F)
                    PdfCell.AddElement(Parag)
                    tabla.AddCell(PdfCell)


                    Dim e = (From x In o
                             Where x.OP_MetodologiaId <> MetodologiaOperaciones.EMetodologiasOperaciones.Especializada AndAlso x.OP_MetodologiaId <> MetodologiaOperaciones.EMetodologiasOperaciones.MysteryShopper
                             Group By x.Cargo Into Group
                             Select New With {
                                        .Cargo = Cargo,
                                        .cantidad = (From y In Group
                                                     Select y.Cantida).Sum
                                    }).OrderByDescending(Function(x) x.cantidad).ToList


                    If e.Count > 0 Then
                        If e(0).Cargo = 5401 Then
                            Valor = oIquoteGeneral.obtenerValorTransporteSupervisorPST
                            totalauxilio = Valor * diastrabajados
                            pi.CuentaCobroDetalle(Consecutivo.Item(0).Numero + 1, e(0).Cargo, totalauxilio)
                        Else
                            Valor = oIquoteGeneral.obtenerValorTransporteEncuestadorPST
                            totalauxilio = Valor * diastrabajados
                        End If
                    End If

                    PdfCell = New PdfPCell
                    Parag = New Paragraph(Valor.ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
                    Parag.Alignment = Element.ALIGN_CENTER
                    Parag.SetLeading(0.5F, 1.0F)
                    PdfCell.AddElement(Parag)
                    tabla.AddCell(PdfCell)

                    PdfCell = New PdfPCell
                    Parag = New Paragraph(totalauxilio.ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
                    Parag.Alignment = Element.ALIGN_CENTER
                    Parag.SetLeading(0.5F, 1.0F)
                    PdfCell.AddElement(Parag)
                    tabla.AddCell(PdfCell)
                End If

                PdfCell = New PdfPCell
                CellBorders(PdfCell, 0, 0, 0, 0)
                Parag = New Paragraph("", font8)
                Parag.Alignment = Element.ALIGN_CENTER
                Parag.SetLeading(0.5F, 1.0F)
                PdfCell.Colspan = 3
                PdfCell.Rowspan = 6
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
                Parag = New Paragraph((total + totalauxilio).ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
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
                Parag = New Paragraph("Retención de IVA", font8B)
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

                'If Persona.CiudadId = Divipola.ECiudades.Bogota Then
                '    sub1 = (total + totalauxilio) * (9.66 / 1000)
                'End If

                If vrICA > 0 Then
                    PdfCell = New PdfPCell
                    Parag = New Paragraph(vrICA.ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
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
                Parag = New Paragraph(otros.ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
                Parag.Alignment = Element.ALIGN_CENTER
                Parag.SetLeading(0.5F, 1.0F)
                PdfCell.AddElement(Parag)
                tabla.AddCell(PdfCell)

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
                Parag = New Paragraph(((total + totalauxilio) - otros - sub1).ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), font8)
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
                Parag = New Paragraph("A este Documento se le aplican las normas relativas del Documento Soporte en adquisidores efectuadas a sujetos no obligados a facturar (Resolución 167 de 30 de diciembre de 2021). Número Autorización 18764082534020 aprobado en 20241030 prefijo DSE desde el nùmero 12171 al 16000 Vigencia: 12 meses.", fontTitle)
                Parag.Alignment = Element.ALIGN_CENTER
                Parag.SpacingBefore = 2
                Parag.SetLeading(0.5F, 1.0F)
                PdfCell.AddElement(Parag)
                'Parag = New Paragraph("Rango de numeración DS 001 hasta DS 3.000", font8)
                'Parag.Alignment = Element.ALIGN_CENTER
                'Parag.SpacingAfter = 2
                'Parag.SetLeading(0.5F, 1.0F)
                'PdfCell.AddElement(Parag)
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

                'Dim aTable10 = New iTextSharp.text.pdf.PdfPTable(6)
                'aTable10.DefaultCell.Border = BorderStyle.Double
                'Dim aTable19 = New iTextSharp.text.pdf.PdfPTable(3)
                'aTable19.DefaultCell.Border = BorderStyle.Double

                'Dim CL1 = New PdfPCell(New Paragraph("Cantidad", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL1.HorizontalAlignment = 1

                'Dim CL2 = New PdfPCell(New Paragraph("Concepto", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL2.HorizontalAlignment = 1

                'Dim CL3 = New PdfPCell(New Paragraph("Job-Estudio", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL3.HorizontalAlignment = 1

                'Dim CL4 = New PdfPCell(New Paragraph("Trabajo", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL4.HorizontalAlignment = 1

                'Dim CL5 = New PdfPCell(New Paragraph("Unitario", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL5.HorizontalAlignment = 1

                'Dim CL6 = New PdfPCell(New Paragraph("Total", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL6.HorizontalAlignment = 1
                'aTable10.AddCell(CL1)
                'aTable10.AddCell(CL2)
                'aTable10.AddCell(CL3)
                'aTable10.AddCell(CL4)
                'aTable10.AddCell(CL5)
                'aTable10.AddCell(CL6)

                'aTable19.AddCell(CL3)
                'aTable19.AddCell(CL4)
                'aTable19.AddCell(CL5)

                ''Ciclo

                'total = 0
                'cantidad = 0
                'otros = 0
                'Mayor = 0
                'For x = 0 To gvProduccion.Rows.Count - 1
                '    Dim oTrabajo As New Trabajo
                '    Dim oeTrabajo = oTrabajo.ObtenerTrabajo(gvProduccion.Rows(x).Cells(1).Text)

                '    If gvProduccion.Rows(x).Cells(0).Text = Persona.id Then
                '        Dim CL7 = New PdfPCell(New Paragraph(gvProduccion.Rows(x).Cells(2).Text, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        CL7.HorizontalAlignment = 1
                '        CL7.FixedHeight = 5

                '        cantidad = gvProduccion.Rows(x).Cells(2).Text
                '        If cantidad > Mayor Then
                '            Mayor = cantidad
                '            job1 = oeTrabajo.JobBook
                '            trabajo1 = oeTrabajo.NombreTrabajo
                '        Else
                '            cantidad = 0

                '        End If


                '        Dim CL8 = New PdfPCell(New Paragraph("Trabajo Campo Nacional", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        CL8.HorizontalAlignment = 1

                '        Dim CL8_2 = New PdfPCell(New Paragraph("Procesamiento de información encuestas", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        CL8_2.HorizontalAlignment = 1

                '        Dim CL9 = New PdfPCell(New Paragraph(oeTrabajo.JobBook, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        CL9.HorizontalAlignment = 1

                '        Dim CL10 = New PdfPCell(New Paragraph(oeTrabajo.NombreTrabajo, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        CL10.HorizontalAlignment = 1

                '        Dim CL11 = New PdfPCell(New Paragraph(gvProduccion.Rows(x).Cells(3).Text, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        CL11.HorizontalAlignment = 1

                '        Dim CL12 = New PdfPCell(New Paragraph(gvProduccion.Rows(x).Cells(4).Text, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        CL12.HorizontalAlignment = 2
                '        aTable10.AddCell(CL7)
                '        If oeTrabajo.PresupuestoId = -100 Then
                '            aTable10.AddCell(CL8_2)
                '        Else
                '            aTable10.AddCell(CL8)
                '        End If

                '        aTable10.AddCell(CL9)
                '        aTable10.AddCell(CL10)
                '        aTable10.AddCell(CL11)
                '        aTable10.AddCell(CL12)

                '        aTable19.AddCell(CL9)
                '        aTable19.AddCell(CL10)
                '        aTable19.AddCell(CL11)


                '        total = total + gvProduccion.Rows(x).Cells(4).Text
                '        GTotalProduccion = GTotalProduccion + gvProduccion.Rows(x).Cells(4).Text  'Para total de control del proceso
                '        diastrabajados = gvProduccion.Rows(x).Cells(8).Text
                '        otros = gvProduccion.Rows(x).Cells(9).Text
                '    End If

                'Next

                'Dim At1 = New PdfPCell(New Paragraph(diastrabajados, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'At1.HorizontalAlignment = 1
                'At1.FixedHeight = 5

                'Dim At2 = New PdfPCell(New Paragraph("Transporte trabajo campo nacional", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'At2.HorizontalAlignment = 1

                'Dim At2_2_Verificar = New PdfPCell(New Paragraph("Transporte trabajo campo nacional", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'At2_2_Verificar.HorizontalAlignment = 1

                'Dim At3 = New PdfPCell(New Paragraph(job1, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'At3.HorizontalAlignment = 1

                'Dim At4 = New PdfPCell(New Paragraph(trabajo1, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'At4.HorizontalAlignment = 1

                'Dim e = (From x In o
                '         Where x.OP_MetodologiaId <> MetodologiaOperaciones.EMetodologiasOperaciones.Especializada AndAlso x.OP_MetodologiaId <> MetodologiaOperaciones.EMetodologiasOperaciones.MysteryShopper
                '         Group By x.Cargo Into Group
                '         Select New With {
                '                        .Cargo = Cargo,
                '                        .cantidad = (From y In Group
                '                                     Select y.Cantida).Sum
                '                    }).OrderByDescending(Function(x) x.cantidad).ToList
                'Dim Valor As Double = 0
                'Dim totalauxilio As Double = 0
                'Dim oIquoteGeneral As New IQ.IquoteGeneral

                'If e.Count > 0 Then
                '    If e(0).Cargo = 5401 Then
                '        Valor = oIquoteGeneral.obtenerValorTransporteSupervisorPST
                '        totalauxilio = Valor * diastrabajados
                '        Dim At5 = New PdfPCell(New Paragraph(Valor, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        At5.HorizontalAlignment = 1
                '        Dim At6 = New PdfPCell(New Paragraph(totalauxilio, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        At6.HorizontalAlignment = 2

                '        aTable10.AddCell(At1)
                '        aTable10.AddCell(At2)
                '        aTable10.AddCell(At3)
                '        aTable10.AddCell(At4)
                '        aTable10.AddCell(At5)
                '        aTable10.AddCell(At6)
                '        pi.CuentaCobroDetalle(Consecutivo.Item(0).Numero + 1, e(0).Cargo, totalauxilio)

                '    Else
                '        Valor = oIquoteGeneral.obtenerValorTransporteEncuestadorPST
                '        totalauxilio = Valor * diastrabajados
                '        Dim At5 = New PdfPCell(New Paragraph(Valor, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        At5.HorizontalAlignment = 1

                '        Dim At6 = New PdfPCell(New Paragraph(totalauxilio, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                '        At6.HorizontalAlignment = 2
                '        aTable10.AddCell(At1)
                '        aTable10.AddCell(At2)
                '        aTable10.AddCell(At3)
                '        aTable10.AddCell(At4)
                '        aTable10.AddCell(At5)
                '        aTable10.AddCell(At6)
                '        pi.CuentaCobroDetalle(Consecutivo.Item(0).Numero + 1, e(0).Cargo, totalauxilio)

                '    End If
                'End If

                'Dim CL13 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL13.HorizontalAlignment = 1

                'Dim CL14 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL14.HorizontalAlignment = 1

                'Dim CL15 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL15.HorizontalAlignment = 1

                'Dim CL16 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL16.HorizontalAlignment = 1

                'Dim CL17 = New PdfPCell(New Paragraph("Total", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL17.HorizontalAlignment = 1

                'Dim CL18 = New PdfPCell(New Paragraph((total + totalauxilio).ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                'CL18.HorizontalAlignment = 2
                'aTable10.AddCell(CL13)
                'aTable10.AddCell(CL14)
                'aTable10.AddCell(CL15)
                'aTable10.AddCell(CL16)
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

                'Dim CL23 = New PdfPCell(New Paragraph("Retencion de IVA", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
                'CL23.HorizontalAlignment = 2

                'Dim CL24 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
                'CL24.HorizontalAlignment = 2
                'aTable12.AddCell(CL22)
                'aTable12.AddCell(CL23)
                'aTable12.AddCell(CL24)
                'Dim sub1 As Double
                'If Persona.CiudadId = Divipola.ECiudades.Bogota Then

                '    Dim CL25 = New PdfPCell(New Paragraph("", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
                '    CL25.HorizontalAlignment = 2

                '    Dim CL26 = New PdfPCell(New Paragraph("Retencion de ICA", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
                '    CL26.HorizontalAlignment = 2

                '    Dim CL27 = New PdfPCell(New Paragraph(((total + totalauxilio) * (9.66 / 1000)).ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))


                '    sub1 = (total + totalauxilio) * (9.66 / 1000)
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

                'Dim CL29 = New PdfPCell(New Paragraph("Otros", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
                'CL29.HorizontalAlignment = 2

                'Dim CL30 = New PdfPCell(New Paragraph(otros.ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
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

                'Dim CL33 = New PdfPCell(New Paragraph(((total + totalauxilio) - otros - sub1).ToString("C0", Globalization.CultureInfo.GetCultureInfo("es-CO")), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
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


                op.CuentasdeCobro(i, Persona.id, total + totalauxilio, Session("IDUsuario"))

                'Nueva Hoja para la cuenta con requerimiento. REQUERIMIENTO!!!
                documentoPDF.NewPage()

                Dim aTable17 = New iTextSharp.text.pdf.PdfPTable(2)
                aTable17.DefaultCell.Border = Rectangle.NO_BORDER

                Dim Cell = New PdfPCell(New Paragraph("REQUERIMIENTO DE SERVICIOS ESPECIFICOS", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD)))
                Cell.Colspan = 3
                Cell.Border = 0
                Cell.HorizontalAlignment = 1
                aTable17.AddCell(Cell)

                aTable17.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable17.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))

                aTable17.AddCell(New Paragraph("CIUDAD: " & Persona.Ciudad, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable17.AddCell(New Paragraph("FECHA:  " & FechaFactura, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))

                aTable17.AddCell(New Paragraph("CONTRATANTE: ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable17.AddCell(New Paragraph("IPSOS NAPOLEON FRANCO & CIA S.A.S.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))

                aTable17.AddCell(New Paragraph("IDENTIFICACION: ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable17.AddCell(New Paragraph("NIT: 890.319.494-5", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))

                aTable17.AddCell(New Paragraph("CONTRATISTA:  ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable17.AddCell(New Paragraph(Persona.Nombres & " " & Persona.Apellidos, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable17.AddCell(New Paragraph("IDENTIFICACION: C.C. Nº  ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable17.AddCell(New Paragraph(Persona.id, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))

                aTable17.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable17.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))


                Dim Cell22 = New PdfPCell(New Paragraph("Objeto del requerimiento: Trabajo nacional de campo ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                Cell22.Colspan = 3
                Cell22.Border = 0
                Cell22.HorizontalAlignment = 0
                aTable17.AddCell(Cell22)


                Dim Cell23 = New PdfPCell(New Paragraph("Por la presente IPSOS NAPOLEÓN FRANCO & CIA S.A.S., en desarrollo del contrato de prestación de servicios técnicos actualmente vigente entre las partes procede a suministrar al CONTRATISTA información sobre encuestas a realizarse por parte de este último: ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                Cell23.Colspan = 3
                Cell23.Border = 0
                Cell23.HorizontalAlignment = 0
                aTable17.AddCell(Cell23)

                Dim TextNew = New PdfPCell(New Paragraph("El contratista del servicio se obliga con el contratante, bajo su entera y absoluta responsabilidad, autonomía Técnica, Financiera y administrativa, a ejecutar la(s) tarea(s) para el(los) estudio(s) relacionado(s) de conformidad con los requisitos metodológicos y requerimientos de la norma 20252:2019 y demás establecidos por el contratante. Dado que los servicios prestados se ejecutan de manera autónoma e independiente, no existirá vínculo laboral entre las partes.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                TextNew.Colspan = 2
                TextNew.Border = 0
                TextNew.HorizontalAlignment = 0

                Dim textNewEmergenciaSanitaria = New PdfPCell()
                textNewEmergenciaSanitaria.AddElement(New Paragraph("OBLIGACIONES RELACIONADAS CON LA EMERGENCIA SANITARIA: En la ejecución de este REQUERIMIENTO DE SERVICIOS ESPECIFICOS (RSE) EL CONTRATISTA tendrá las siguientes obligaciones:", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))

                Dim lista = New List(True)
                lista.Add(New ListItem("Asistir a la capacitación de bioseguridad que programe IPSOS y cumplir con el protocolo de bioseguridad expuesto y entregado en dicha capacitación conforme a lo indicado en los numerales 4 y 5 del artículo 2.2.4.2.2.16 del Decreto 1072/2015", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                lista.Add(New ListItem("Cumplir con las medidas de verificación del protocolo de bioseguridad que IPSOS indique en la respectiva capacitación.", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                lista.Add(New ListItem("Avisar a IPSOS de inmediato si tiene síntomas de COVID-19 tales como la fiebre, la tos seca y el cansancio, dolores y molestias, la congestión nasal, el dolor de cabeza, la conjuntivitis, el dolor de garganta, la diarrea, la pérdida del gusto o el olfato y las erupciones cutáneas o cambios de color en los dedos de las manos o los pies, etc. (numeral 3 del artículo 2.2.4.2.2.16 del Decreto 1072/2015)", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                lista.Add(New ListItem("Cumplir con la normatividad vigente expedida por el Gobierno Nacional, Departamental o Distrital/Municipal en relación con la emergencia sanitaria", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                textNewEmergenciaSanitaria.AddElement(lista)
                textNewEmergenciaSanitaria.Colspan = 2
                textNewEmergenciaSanitaria.Border = 0
                textNewEmergenciaSanitaria.HorizontalAlignment = 0


                Dim aTable18 = New iTextSharp.text.pdf.PdfPTable(2)
                aTable18.DefaultCell.Border = Rectangle.NO_BORDER
                aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(TextNew)
                aTable18.AddCell(textNewEmergenciaSanitaria)
                aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))

                Dim Cell33 = New PdfPCell(New Paragraph("Autorizo, a que me sea descontado de la cuenta de cobro respectiva, el valor que adeude por seguridad social", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                Cell33.Colspan = 3 '
                Cell33.Border = 0
                Cell33.HorizontalAlignment = 0
                aTable18.AddCell(Cell33)

                aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph("Firma Ordenante: ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph("Firma Contratista: ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))



                Dim Imagen4 As iTextSharp.text.Image
                Imagen4 = iTextSharp.text.Image.GetInstance(path & "FirmaOrdenante.png")
                Imagen4.ScalePercent(75)
                Dim Img4 = New PdfPCell
                Img4.Border = 0
                Img4.AddElement(Imagen4)
                aTable18.AddCell(Img4)

                aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))


                aTable18.AddCell(New Paragraph("Clemencia Moreno Sánchez", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph(Persona.Nombres & " " & Persona.Apellidos, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph("Contratante", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph("Contratista", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph("Cedula: 52829702", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph("Cedula: " & Persona.id, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))

                aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
                aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))

                documentoPDF.Add(aTable17)
                documentoPDF.Add(aTable19)
                documentoPDF.Add(aTable18)

            End If

        Next
        'Añadimos los metadatos para el fichero PDF
        documentoPDF.AddAuthor(Session("IDUsuario").ToString)
        documentoPDF.AddTitle("Cuenta de Cobro")
        documentoPDF.AddCreationDate()
        documentoPDF.Close() 'Cerramos el objeto documento, guardamos y creamos el PDF


    End Sub

#Region "Eventos"
    Protected Sub Timer1_Tick(ByVal sender As Object, ByVal e As EventArgs)
        ' Show the progress of current operation.
        Dim worker As BackgroundWorker = DirectCast(Session("worker"), BackgroundWorker)
        Dim Guid As String = DirectCast(Session("Guid"), String)
        If worker IsNot Nothing Then
            ' Display the progress of the operation.
            If CInt(lblProgreso.Text) > worker.Progress Then
                lblProgreso.Text = lblCantidadOrdenesGenerar.Text
            Else
                lblProgreso.Text = worker.Progress.ToString()
            End If
            btnCuentas.Enabled = Not worker.IsRunning
            Timer1.Enabled = worker.IsRunning
            lblEstadp.Text = "En Curso"
            lblMensaje.Text = DirectCast(worker.Result, String)
            ' Display the result when the operation completed.
            If Not worker.IsRunning Then
                lblEstadp.Text = "Finalizado"
                phArchivos.Controls.Add(crearTablaArchivosDescarga(ddlciudades.SelectedValue, Guid))
            End If
        End If
    End Sub
#End Region
#Region "Metodos"
    Function crearTablaArchivosDescarga(ByVal ciudadId As Integer, ByVal guid As String) As HtmlTable
        Dim tabla As New HtmlTable
        tabla.Border = 1
        tabla.Rows.Add(New HtmlTableRow)
        tabla.Rows(0).Cells.Add(New HtmlTableCell With {.InnerText = "Ciudad"})
        tabla.Rows(0).Cells.Add(New HtmlTableCell With {.InnerText = "Url"})
        tabla.Rows.Add(New HtmlTableRow)
        tabla.Rows(1).Cells.Add(New HtmlTableCell With {.InnerText = ddlciudades.SelectedItem.Text})
        tabla.Rows(1).Cells.Add(New HtmlTableCell)
        tabla.Rows(1).Cells(1).Controls.Add(New HtmlAnchor With {.HRef = "~/Cuentasdecobro/CuentasdeCobro-" & guid & ".pdf", .InnerText = "Descargar", .Target = "_blank"})
        tabla.Rows.Add(New HtmlTableRow)

        Return tabla
    End Function
    Sub worker_DoWork(ByRef progress As Integer, ByRef result As Object, ByVal ParamArray arguments As Object())
        Try
            CrearPdf(CType(arguments(0), List(Of CC_CedulasProduccion_Result)), progress, CType(arguments(1), String), CType(arguments(2), Date), CType(arguments(3), Date), CType(arguments(4), PresupInt.ETipoEncuestador?), CType(arguments(5), Long?))
            lblGTotalProduccion.Text = GTotalProduccion
            result = "Finalizo correctamente"
        Catch ex As Exception
            result = "A ocurrido un error al tratar de exportar los datos - " & ex.Message
        End Try
    End Sub
#End Region

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

    Private Sub CuentasdeCobro_Load(sender As Object, e As EventArgs) Handles Me.Load
        'Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        'smanager.RegisterPostBackControl(Me.btnCuentas)
    End Sub

    Private Sub CuentasdeCobro_Init(sender As Object, e As EventArgs) Handles Me.Init
        Page.Form.Attributes.Add("enctype", "multipart/form-data")
    End Sub

#End Region

End Class