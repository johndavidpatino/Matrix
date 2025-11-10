Imports CoreProject
Imports WebMatrix.Util
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO

Public Class FI_Gestion_Ordenes_Aprobacion
	Inherits System.Web.UI.Page
#Region "Enumerados"
	Enum eTipoOrden
		OrdenServicio = 1
		OrdenCompra = 2
		OrdenRequerimiento = 3
	End Enum
#End Region
	Sub AlertJS(ByVal mensaje As String)
		ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
	End Sub

	Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
		hfTipoOrden.Value = eTipoOrden.OrdenServicio
		Me.txtTitulo.Text = "Ordenes de Servicio"
		CargarOrdenes()
		gvAprobaciones.DataSource = ""
		gvAprobaciones.DataBind()
	End Sub

	Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
		hfTipoOrden.Value = eTipoOrden.OrdenCompra
		Me.txtTitulo.Text = "Ordenes de Compra"
		CargarOrdenes()
		gvAprobaciones.DataSource = ""
		gvAprobaciones.DataBind()
	End Sub

	Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
		hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento
		Me.txtTitulo.Text = "Ordenes de Requerimiento"
		CargarOrdenes()
		gvAprobaciones.DataSource = ""
		gvAprobaciones.DataBind()
	End Sub

	Sub CargarOrdenes()
		Dim o As New FI.Ordenes
		If Me.hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
			Me.gvOrdenes.DataSource = o.OrdenesServicioPendientesAprobacion(Session("IDUsuario").ToString)
		ElseIf Me.hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
			Me.gvOrdenes.DataSource = o.OrdenesCompraPendientesAprobacion(Session("IDUsuario").ToString)
		ElseIf Me.hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
			Me.gvOrdenes.DataSource = o.OrdenesRequerimientoPendientesAprobacion(Session("IDUsuario").ToString)
		End If
		Me.gvOrdenes.DataBind()
	End Sub

	Private Sub btnAprobar_Click(sender As Object, e As EventArgs) Handles btnAprobar.Click
		Dim o As New FI.Ordenes
		If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
			o.AprobarOS(hfId.Value, txtComentarios.Text, Session("IDUsuario").ToString, True)
		ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
			o.AprobarOC(hfId.Value, txtComentarios.Text, Session("IDUsuario").ToString, True)
		ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
			o.AprobarOR(hfId.Value, txtComentarios.Text, Session("IDUsuario").ToString, True)
		End If
		CargarOrdenes()
	End Sub

	Private Sub btnNoAprobar_Click(sender As Object, e As EventArgs) Handles btnNoAprobar.Click
		Dim o As New FI.Ordenes
		If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
			o.AprobarOS(hfId.Value, txtComentarios.Text, Session("IDUsuario").ToString, False)
		ElseIf hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
			o.AprobarOC(hfId.Value, txtComentarios.Text, Session("IDUsuario").ToString, False)
		ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
			o.AprobarOR(hfId.Value, txtComentarios.Text, Session("IDUsuario").ToString, False)
		End If
	End Sub
#Region "PDF"

	Sub ConstruirPDFOs(ByVal id As Long, ByVal document As Document)
		Dim font9 As New Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL)
		Dim font9B As New Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD)
		Dim path As String = Server.MapPath("~/Files/")
		Dim tabla As New PdfPTable(4)
		Dim Ancho As Single() = {Ssg(4), Ssg(9), Ssg(4), Ssg(4)}
		tabla.SetWidths(Ancho)
		tabla.DefaultCell.Border = 0
		tabla.WidthPercentage = 100

		Dim Col_Title1 As New iTextSharp.text.BaseColor(204, 232, 212)
		Dim Col_Title2 As New iTextSharp.text.BaseColor(242, 249, 220)


		Dim PdfCell As New PdfPCell

		tabla = New PdfPTable(1)
		PdfCell = New PdfPCell
		CellBorders(PdfCell, 0, 0, 0, 0)
		PdfCell.HorizontalAlignment = 3

		Try


			Dim titulo As String = ""
			titulo = "ORDEN DE SERVICIO No. " & id

			Dim Parag As New Paragraph(titulo, font9)
			'Parag.Alignment = 1
			'PdfCell.AddElement(Parag)
			'tabla.AddCell(PdfCell)
			'document.Add(tabla)

			tabla = New PdfPTable(2)
			Ancho = {Ssg(10), Ssg(11)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100

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

			tabla.AddCell(NewPdfCell("Bogotá: Calle 74 No. 11-81 - PBX: 376 9400 - Fax: 376 9400 Ext: 525", 3, False, False))
			tabla.AddCell(NewPdfCell("Medellín: Carrera 36 No. 7-46 - PBX: (4) 312 2090", 3, False, False))
			tabla.AddCell(NewPdfCell("Cali: Calle 7 Sur No. 38A-11 - Teléfono: (2) 514 0017", 3, False, False))
			tabla.AddCell(NewPdfCell("Nit: 890.319.494-5 | Página web: www.ipsos.com.co", 3, False, False))

			document.Add(tabla)

			tabla = New PdfPTable(1)
			Ancho = {Ssg(21)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100
			tabla.AddCell(NewPdfCellGray(titulo, 1, True, True))
			document.Add(tabla)

			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())

			Dim o As New FI.Ordenes
			Dim info = o.ObtenerOrdenesDeServicio(id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
			Dim op As New Contratista
			Dim infop = op.ObtenerContratista(info.ProveedorId)
			tabla = New PdfPTable(5)
			Ancho = {Ssg(3), Ssg(7), Ssg(1), Ssg(3), Ssg(7)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100


			tabla.AddCell(NewPdfCell("Proveedor", 3, True, True))
			tabla.AddCell(NewPdfCell(info.Proveedor, 3, False, True))
			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			PdfCell.HorizontalAlignment = 1
			PdfCell.VerticalAlignment = 1
			PdfCell.Rowspan = 5
			tabla.AddCell(PdfCell)

			tabla.AddCell(NewPdfCell("Ciudad y Fecha", 3, True, True))
			tabla.AddCell(NewPdfCell(info.Ciudad & " - " & info.Fecha, 3, False, True))

			tabla.AddCell(NewPdfCell("Nit", 3, True, True))
			tabla.AddCell(NewPdfCell(info.ProveedorId, 3, False, True))
			tabla.AddCell(NewPdfCell("Beneficiario", 3, True, True))
			tabla.AddCell(NewPdfCell(info.Beneficiario, 3, False, True))

			tabla.AddCell(NewPdfCell("Direccion", 3, True, True))
			tabla.AddCell(NewPdfCell(infop.Direccion, 3, False, True))
			tabla.AddCell(NewPdfCell("Fecha de entrega", 3, True, True))
			tabla.AddCell(NewPdfCell(info.FechaEntrega, 3, False, True))

			tabla.AddCell(NewPdfCell("Teléfono", 3, True, True))
			tabla.AddCell(NewPdfCell(infop.Telefono, 3, False, True))
			tabla.AddCell(NewPdfCell("Forma de pago", 3, True, True))
			tabla.AddCell(NewPdfCell(info.FormaPago, 3, False, True))

			tabla.AddCell(NewPdfCell("CC / JB", 3, True, True))
			tabla.AddCell(NewPdfCell(info.JOBBOOK, 3, False, True))
			tabla.AddCell(NewPdfCell("Descripción CC / JB", 3, True, True))
			tabla.AddCell(NewPdfCell(info.CARGADOA, 3, False, True))

			document.Add(tabla)

			document.Add(NewRenglonEnter())

			Dim infoD = o.obtenerDetalle(eTipoOrden.OrdenServicio, id)

			tabla = New PdfPTable(6)
			Ancho = {Ssg(2), Ssg(2), Ssg(7), Ssg(2), Ssg(3), Ssg(3)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100
			tabla.AddCell(NewPdfCellGray("Cta Contable", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Fecha", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Descripción", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Cantidad", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Vr Unitario", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Vr Total", 3, True, True))

			For Each el As FI_OrdenDetalle_Get_Result In infoD
				tabla.AddCell(NewPdfCell(el.NumeroCuenta & " - " & el.NombreCuenta, 3, False, True))
				tabla.AddCell(NewPdfCell(el.Fecha, 3, False, True))
				tabla.AddCell(NewPdfCell(el.Descripcion, 3, False, True))
				tabla.AddCell(NewPdfCell(el.Cantidad, 3, False, True))
				tabla.AddCell(NewPdfCell(String.Format("{0:C0}", el.VrUnitario), 3, False, True))
				tabla.AddCell(NewPdfCell(String.Format("{0:C0}", el.VrTotal), 3, False, True))
			Next

			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("Subtotal", 3, True, True))
			tabla.AddCell(NewPdfCell(String.Format("{0:C0}", info.SUBTOTAL), 3, True, True))

			document.Add(tabla)

			document.Add(NewRenglonEnter)
			document.Add(NewRenglonEnter)
			Parag = New Paragraph("La factura DEBE :  a) hacerse a nombre de IPSOS - NAPOLEON FRANCO & CIA. S.A.S. - NIT. 890.319.494-5 b) cumplir con los requisitos establecidos en el Art. 617 del Estatuto Tributario; c)Adjuntar y hacer referencia al número de esta orden;  d) radicarse con el Vo. Bo. de la persona que recibe la mercancía, en la Recepción de la Compañía. Nos reservamos el derecho de recibir la mercancía a que esta orden se refiere si no cumple con las especificaciones o la fecha de entrega estipuladas. Las Cuentas de Cobro se deben radicar a más tardar Ocho (8) días de finalizados los procesos Internos, de no ser así Ipsos Napoleón Franco & CIA S A S no se hace responsable de dicho pago.", font9)
			Parag.Alignment = 4
			document.Add(Parag)
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())

			Parag = New Paragraph("El CONTRATISTA se obliga con el contratante,  bajo su entera y absoluta responsabilidad, autonomía técnica, financiera y administrativa" _
								  & ", a ejecutar las tareas contratadas para los estudios  relacionados de conformidad con los requisitos metodológicos y requerimientos de " _
								  & "las de las Normas 20252:2019 y 9001:2015 y demás establecidos por el contratante. Dado que los servicios prestados se ejecutan de manera autónoma e independiente," _
								  & "no existirá vínculo laboral entre las partes.Se hace entrega de metodología, instrumentos y/o especificaciones de la prestación del servicio" _
								  & " y sus respectivas  actividades.", font9)
			Parag.Alignment = 4
			document.Add(Parag)
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())

			Parag = New Paragraph("CONFIDENCIALIDAD:", font9B)
			Parag.Alignment = 4
			document.Add(Parag)
			document.Add(NewRenglonEnter())

			Parag = New Paragraph("EL CONTRATISTA mantendrá la confidencialidad de la información recibida del CONTRATANTE para la ejecución y desarrollo de los servicios" _
								  & " contratados. Se reitera en esta materia el estricto cumplimiento de la obligación consignada en la cláusula décima del contrato marco " _
								  & "de prestación de servicios y el Acuerdo especial de confidencialidad suscrito por las partes. EL CONTRATISTA declara haber leído y " _
								  & "tener perfectamente presente para la ejecución del presente RSE los documentos antes mencionados.", font9)
			Parag.Alignment = 4
			document.Add(Parag)

			Parag = New Paragraph()
			Parag.SpacingBefore = 5
			Parag.Add(New Chunk("OBLIGACIONES RELACIONADAS CON LA EMERGENCIA SANITARIA:", font9B))
			Parag.Add(New Chunk("En la ejecución de esta ORDEN DE SERVICIO EL CONTRATISTA tendrá las siguientes obligaciones:", font9))
			Parag.SetLeading(0.2F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)


			Parag = New Paragraph()
			Dim Lista = New List(True)
			Lista.Add(New ListItem("Asistir a la capacitación de bioseguridad que programe IPSOS y cumplir con el protocolo de bioseguridad expuesto y entregado en dicha capacitación conforme a lo indicado en los numerales 4 y 5 del artículo 2.2.4.2.2.16 del Decreto 1072/2015.", font9))
			Lista.Add(New ListItem("Cumplir con las medidas de verificación del protocolo de bioseguridad que IPSOS indique en la respectiva capacitación.", font9))
			Lista.Add(New ListItem("Avisar a IPSOS de inmediato si tiene síntomas de COVID-19 tales como la fiebre, la tos seca y el cansancio, dolores y molestias, la congestión nasal, el dolor de cabeza, la conjuntivitis, el dolor de garganta, la diarrea, la pérdida del gusto o el olfato y las erupciones cutáneas o cambios de color en los dedos de las manos o los pies, etc. (numeral 3 del artículo 2.2.4.2.2.16 del Decreto 1072/2015)", font9))
			Lista.Add(New ListItem("Cumplir con la normatividad vigente expedida por el Gobierno Nacional, Departamental o Distrital/Municipal en relación con la emergencia sanitaria.", font9))
			Lista.Add(New ListItem("Asegurarse que el personal que EL CONTRATISTA haya contratado para los servicios cumpla en su totalidad las disposiciones de esta cláusula.", font9))
			Lista.Add(New ListItem("Cumplir con la regulación aplicable respecto de la emergencia sanitaria según le aplique y contar con su respectivo protocolo de bioseguridad y habilitación de la alcaldía, en caso de ser requisito para operar.", font9))
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			Parag.Add(Lista)
			document.Add(Parag)

			Parag = New Paragraph()
			Parag.SpacingBefore = 5
			Parag.Add(New Chunk("OBLIGACIONES RELACIONADAS CON EL REGLAMENTO UNICO DEL SECTOR TRABAJO:", font9B))
			Parag.Add(New Chunk("En la ejecución de esta ORDEN DE SERVICIO EL CONTRATISTA tendrá las siguientes obligaciones:", font9))
			Dim Lista2 = New List(False, 10.0F)
			Lista2.SetListSymbol("•")
			Lista2.Add(New ListItem("Cumplir con la obligación de afiliación al Sistema general del riesgo laboral", font9))
			Parag.Add(Lista2)
			''Parag.Add(New Chunk(Chunk.NEWLINE))
			Parag.Add(New ListItem("Para los efectos del sistema de gestión de la seguridad y salud en el trabajo los proveedores y contratistas deben cumplir frente a sus trabajadores o subcontratistas con la obligación anteriormente mencionada", font9))
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			document.Add(NewRenglonEnter())

			Parag = New Paragraph("Aceptamos esta orden en los términos y condiciones aquí estipulados", font9)
			Parag.Alignment = 3
			document.Add(Parag)
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())
			tabla = New PdfPTable(5)
			Ancho = {Ssg(6), Ssg(1.5), Ssg(6), Ssg(1.5), Ssg(6)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100

			tabla.AddCell(NewPdfCell("", 2, True, True))
			tabla.AddCell(NewPdfCell("", 2, True, False))
			tabla.AddCell(NewPdfCell("", 2, True, True))
			tabla.AddCell(NewPdfCell("", 2, True, False))
			tabla.AddCell(NewPdfCell("", 2, True, True))

			tabla.AddCell(NewPdfCell("NOMBRE CONTRATISTA", 1, True, False))
			tabla.AddCell(NewPdfCell("", 1, True, False))
			tabla.AddCell(NewPdfCell("IDENTIFICACIÓN", 1, True, False))
			tabla.AddCell(NewPdfCell("", 1, True, False))
			tabla.AddCell(NewPdfCell("FECHA", 1, True, False))

			document.Add(tabla)

		Catch ex As Exception
			document.CloseDocument()
			document.Close()
			Exit Sub
		Finally
			document.CloseDocument()
			document.Close()
		End Try

	End Sub

	Sub ConstruirPDFOc(ByVal id As Long, ByVal document As Document)
		Dim font9 As New Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL)
		Dim font9B As New Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD)
		Dim path As String = Server.MapPath("~/Files/")
		Dim tabla As New PdfPTable(4)
		Dim Ancho As Single() = {Ssg(4), Ssg(9), Ssg(4), Ssg(4)}
		tabla.SetWidths(Ancho)
		tabla.DefaultCell.Border = 0
		tabla.WidthPercentage = 100

		Dim Col_Title1 As New iTextSharp.text.BaseColor(204, 232, 212)
		Dim Col_Title2 As New iTextSharp.text.BaseColor(242, 249, 220)


		Dim PdfCell As New PdfPCell

		tabla = New PdfPTable(1)
		PdfCell = New PdfPCell
		CellBorders(PdfCell, 0, 0, 0, 0)
		PdfCell.HorizontalAlignment = 3

		Try


			Dim titulo As String = ""
			titulo = "ORDEN DE COMPRA No. " & id

			Dim Parag As New Paragraph(titulo, font9)
			'Parag.Alignment = 1
			'PdfCell.AddElement(Parag)
			'tabla.AddCell(PdfCell)
			'document.Add(tabla)

			tabla = New PdfPTable(2)
			Ancho = {Ssg(10), Ssg(11)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100

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

			tabla.AddCell(NewPdfCell("Bogotá: Calle 74 No. 11-81 - PBX: 376 9400 - Fax: 376 9400 Ext: 525", 3, False, False))
			tabla.AddCell(NewPdfCell("Medellín: Carrera 36 No. 7-46 - PBX: (4) 312 2090", 3, False, False))
			tabla.AddCell(NewPdfCell("Cali: Calle 7 Sur No. 38A-11 - Teléfono: (2) 514 0017", 3, False, False))
			tabla.AddCell(NewPdfCell("Nit: 890.319.494-5 | Página web: www.ipsos.com.co", 3, False, False))

			document.Add(tabla)

			tabla = New PdfPTable(1)
			Ancho = {Ssg(21)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100
			tabla.AddCell(NewPdfCellGray(titulo, 1, True, True))
			document.Add(tabla)

			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())

			Dim o As New FI.Ordenes
			Dim info = o.ObtenerOrdenesDeCompra(id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
			Dim op As New Contratista
			Dim infop = op.ObtenerContratista(info.ProveedorId)
			tabla = New PdfPTable(5)
			Ancho = {Ssg(3), Ssg(7), Ssg(1), Ssg(3), Ssg(7)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100


			tabla.AddCell(NewPdfCell("Proveedor", 3, True, True))
			tabla.AddCell(NewPdfCell(info.Proveedor, 3, False, True))
			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			PdfCell.HorizontalAlignment = 1
			PdfCell.VerticalAlignment = 1
			PdfCell.Rowspan = 5
			tabla.AddCell(PdfCell)

			tabla.AddCell(NewPdfCell("Ciudad y Fecha", 3, True, True))
			tabla.AddCell(NewPdfCell(info.Ciudad & " - " & info.Fecha, 3, False, True))

			tabla.AddCell(NewPdfCell("Nit", 3, True, True))
			tabla.AddCell(NewPdfCell(info.ProveedorId, 3, False, True))
			tabla.AddCell(NewPdfCell("Beneficiario", 3, True, True))
			tabla.AddCell(NewPdfCell(info.Beneficiario, 3, False, True))

			tabla.AddCell(NewPdfCell("Direccion", 3, True, True))
			tabla.AddCell(NewPdfCell(infop.Direccion, 3, False, True))
			tabla.AddCell(NewPdfCell("Fecha de entrega", 3, True, True))
			tabla.AddCell(NewPdfCell(info.FechaEntrega, 3, False, True))

			tabla.AddCell(NewPdfCell("Teléfono", 3, True, True))
			tabla.AddCell(NewPdfCell(infop.Telefono, 3, False, True))
			tabla.AddCell(NewPdfCell("Forma de pago", 3, True, True))
			tabla.AddCell(NewPdfCell(info.FormaPago, 3, False, True))

			tabla.AddCell(NewPdfCell("CC / JB", 3, True, True))
			tabla.AddCell(NewPdfCell(info.JOBBOOK, 3, False, True))
			tabla.AddCell(NewPdfCell("Descripción CC / JB", 3, True, True))
			tabla.AddCell(NewPdfCell(info.CARGADOA, 3, False, True))

			document.Add(tabla)

			document.Add(NewRenglonEnter())

			Dim infoD = o.obtenerDetalle(eTipoOrden.OrdenCompra, id)

			tabla = New PdfPTable(6)
			Ancho = {Ssg(2), Ssg(2), Ssg(7), Ssg(2), Ssg(3), Ssg(3)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100
			tabla.AddCell(NewPdfCellGray("Cta Contable", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Fecha", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Descripción", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Cantidad", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Vr Unitario", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Vr Total", 3, True, True))

			For Each el As FI_OrdenDetalle_Get_Result In infoD
				tabla.AddCell(NewPdfCell(el.NumeroCuenta & " - " & el.NombreCuenta, 3, False, True))
				tabla.AddCell(NewPdfCell(el.Fecha, 3, False, True))
				tabla.AddCell(NewPdfCell(el.Descripcion, 3, False, True))
				tabla.AddCell(NewPdfCell(el.Cantidad, 3, False, True))
				tabla.AddCell(NewPdfCell(String.Format("{0:C0}", el.VrUnitario), 3, False, True))
				tabla.AddCell(NewPdfCell(String.Format("{0:C0}", el.VrTotal), 3, False, True))
			Next

			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("Subtotal", 3, True, True))
			tabla.AddCell(NewPdfCell(String.Format("{0:C0}", info.SUBTOTAL), 3, True, True))

			document.Add(tabla)

			document.Add(NewRenglonEnter)
			document.Add(NewRenglonEnter)
			Parag = New Paragraph("La factura DEBE :  a) hacerse a nombre de IPSOS - NAPOLEON FRANCO & CIA. S.A.S. - NIT. 890.319.494-5 b) cumplir con los requisitos establecidos en el Art. 617 del Estatuto Tributario; c)Adjuntar y hacer referencia al número de esta orden;  d) radicarse con el Vo. Bo. de la persona que recibe la mercancía, en la Recepción de la Compañía. Nos reservamos el derecho de recibir la mercancía a que esta orden se refiere si no cumple con las especificaciones o la fecha de entrega estipuladas. Las Cuentas de Cobro se deben radicar a más tardar Ocho (8) días de finalizados los procesos Internos, de no ser así Ipsos Napoleón Franco & CIA S A S no se hace responsable de dicho pago.", font9)
			Parag.Alignment = 4
			document.Add(Parag)
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())

			Parag = New Paragraph("El CONTRATISTA se obliga con el contratante,  bajo su entera y absoluta responsabilidad, autonomía técnica, financiera y administrativa" _
					  & ", a ejecutar las tareas contratadas para los estudios  relacionados de conformidad con los requisitos metodológicos y requerimientos de " _
					  & "las de las Normas 20252:2019 y 9001:2015 y demás establecidos por el contratante. Dado que los servicios prestados se ejecutan de manera autónoma e independiente," _
					  & "no existirá vínculo laboral entre las partes.Se hace entrega de metodología, instrumentos y/o especificaciones de la prestación del servicio" _
					  & " y sus respectivas  actividades.", font9)
			Parag.Alignment = 4
			document.Add(Parag)
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())

			Parag = New Paragraph("CONFIDENCIALIDAD:", font9B)
			Parag.Alignment = 4
			document.Add(Parag)
			document.Add(NewRenglonEnter())

			Parag = New Paragraph("EL CONTRATISTA mantendrá la confidencialidad de la información recibida del CONTRATANTE para la ejecución y desarrollo de los servicios" _
								  & " contratados. Se reitera en esta materia el estricto cumplimiento de la obligación consignada en la cláusula décima del contrato marco " _
								  & "de prestación de servicios y el Acuerdo especial de confidencialidad suscrito por las partes. EL CONTRATISTA declara haber leído y " _
								  & "tener perfectamente presente para la ejecución del presente RSE los documentos antes mencionados.", font9)
			Parag.Alignment = 4
			document.Add(Parag)
			document.Add(NewRenglonEnter())

			Parag = New Paragraph("Aceptamos esta orden en los términos y condiciones aquí estipulados", font9)
			Parag.Alignment = 3
			document.Add(Parag)
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())
			tabla = New PdfPTable(5)
			Ancho = {Ssg(6), Ssg(1.5), Ssg(6), Ssg(1.5), Ssg(6)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100

			tabla.AddCell(NewPdfCell("", 2, True, True))
			tabla.AddCell(NewPdfCell("", 2, True, False))
			tabla.AddCell(NewPdfCell("", 2, True, True))
			tabla.AddCell(NewPdfCell("", 2, True, False))
			tabla.AddCell(NewPdfCell("", 2, True, True))

			tabla.AddCell(NewPdfCell("NOMBRE CONTRATISTA", 1, True, False))
			tabla.AddCell(NewPdfCell("", 1, True, False))
			tabla.AddCell(NewPdfCell("IDENTIFICACIÓN", 1, True, False))
			tabla.AddCell(NewPdfCell("", 1, True, False))
			tabla.AddCell(NewPdfCell("FECHA", 1, True, False))

			document.Add(tabla)

		Catch ex As Exception
			document.CloseDocument()
			document.Close()
			Exit Sub
		Finally
			document.CloseDocument()
			document.Close()
		End Try

	End Sub

    Sub ConstruirPDFDocEqu(ByVal id As Long, ByVal documentoPDF As Document)
        Dim Col_TitlePage As New BaseColor(30, 172, 167)
        Dim Col_Title As New BaseColor(14, 1, 129)
        Dim Col_Title1 As New BaseColor(204, 232, 212)
        Dim Col_Title2 As New BaseColor(242, 249, 220)
        Dim Col_Title3 As New BaseColor(211, 211, 211)

        Dim font10 As New Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL)
        Dim font10B As New Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)
        Dim font12 As New Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL)
        Dim font12B As New Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)
        Dim font8 As New Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL)
        Dim font8B As New Font(Font.FontFamily.HELVETICA, 8, Font.BOLD)
        Dim font7 As New Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL)
        Dim fontTitle As New Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)
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

        Try
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

            Dim o As New FI.Ordenes
            Dim info = o.ObtenerOrdenesDeRequerimiento(id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
            Dim op As New Contratista
            Dim infoC = op.ObtenerContratista(info.ProveedorId)
            Dim pro As New Proyecto
            Dim otrabajo As New Trabajo
            Dim t As New PY_Trabajo0
            t = otrabajo.ObtenerTrabajo(info.JOBBOOK)
            Dim clas = op.ObtenerClasificacion(infoC.Clasificacion)

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

            PdfCell = New PdfPCell
            Dim FechaElaboracion = DateTime.Parse(info.Fecha).ToString("dd/MM/yyyy")
            Parag = New Paragraph(FechaElaboracion, font10)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 0, 0, 0)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            Dim IdInfo = info.ID
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
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 1, 1, 0, 1)
            Parag = New Paragraph("Número de Identificación", font10)
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 1, 1, 1)
            Dim nombreProveedor = info.Proveedor
            Parag = New Paragraph(nombreProveedor, font10)
            Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 1, 1, 1)
            Dim nit = info.ProveedorId
            Parag = New Paragraph(nit, font10)
            Parag.SpacingAfter = 2
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

			'         documentoPDF.Add(tabla)


			'tabla = New PdfPTable(2)
			'CellBorders(PdfCell, 1, 1, 1, 1)
			'         Ancho = {Ssg(15), Ssg(6)}
			'         tabla.SetWidths(Ancho)
			'         tabla.WidthPercentage = 95

			PdfCell = New PdfPCell
            CellBorders(PdfCell, 1, 1, 0, 1)
            Parag = New Paragraph("Dirección", font10)
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_LEFT
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 0, 1)
			Parag = New Paragraph("Ciudad", font10)
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 1, 1, 1)
			Dim direccion = infoC.Direccion
			Parag = New Paragraph(direccion, font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 1, 1, 1)
			Dim ciudad = info.Ciudad
			Parag = New Paragraph(ciudad, font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 0, 1)
			Parag = New Paragraph("Correo electrónico", font10)
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 0, 1)
            Parag = New Paragraph("Teléfono", font10)
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_LEFT
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 1, 1, 1)
			Dim correoelec = infoC.Email
			Parag = New Paragraph(correoelec, font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 1, 1, 1)
            Dim telefono = infoC.Telefono
            Parag = New Paragraph(telefono, font10)
            Parag.SpacingAfter = 2
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
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 1, 1, 0, 1)
            Parag = New Paragraph("Número de Identificación", font10)
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 1, 1, 1)
            Parag = New Paragraph("IPSOS NAPOLEÓN FRANCO & CIA SAS", font10B)
            Parag.SpacingAfter = 2
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 1, 1, 1)
            Parag = New Paragraph("890.319.494      DV: 5", font10B)
            Parag.SpacingAfter = 2
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
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_LEFT
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 1, 1, 0, 1)
            Parag = New Paragraph("Teléfono", font10)
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_LEFT
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 1, 1, 0, 1)
            Parag = New Paragraph("Ciudad", font10)
			Parag.SpacingAfter = 1
			Parag.Alignment = Element.ALIGN_LEFT
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 1, 1, 1)
            Parag = New Paragraph("CL 74 11 81 P 5", font10)
            Parag.SpacingAfter = 2
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 1, 1, 1)
            Parag = New Paragraph("3769400", font10)
            Parag.SpacingAfter = 2
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 1, 1, 1)
            Parag = New Paragraph("BOGOTÁ", font10)
            Parag.SpacingAfter = 2
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
            Parag.SpacingAfter = 2
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.BackgroundColor = Col_Title3
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 1, 1, 1, 1)
            Parag = New Paragraph("NOMBRE JOB", font8B)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SpacingAfter = 2
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.BackgroundColor = Col_Title3
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 1, 1, 1, 1)
            Parag = New Paragraph("CONCEPTO", font8B)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SpacingAfter = 2
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.BackgroundColor = Col_Title3
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 1, 1, 1, 1)
            Parag = New Paragraph("CANTIDAD", font8B)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SpacingAfter = 2
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.BackgroundColor = Col_Title3
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 1, 1, 1, 1)
            Parag = New Paragraph("VALOR UNITARIO", font8B)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SpacingAfter = 2
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.BackgroundColor = Col_Title3
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 1, 1, 1, 1)
            Parag = New Paragraph("VALOR TOTAL", font8B)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SpacingAfter = 2
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.BackgroundColor = Col_Title3
            PdfCell.AddElement(Parag)
            tabla.AddCell(PdfCell)

            Dim infoD = o.obtenerDetalle(eTipoOrden.OrdenRequerimiento, id)

            For Each el As FI_OrdenDetalle_Get_Result In infoD

                PdfCell = New PdfPCell
                Parag = New Paragraph(t.JobBook, font8)
                Parag.Alignment = Element.ALIGN_CENTER
                Parag.SetLeading(0.5F, 1.0F)
                PdfCell.AddElement(Parag)
                tabla.AddCell(PdfCell)

                PdfCell = New PdfPCell
                Parag = New Paragraph(info.CARGADOA, font7)
                Parag.Alignment = Element.ALIGN_CENTER
                Parag.SetLeading(0.5F, 1.0F)
                PdfCell.AddElement(Parag)
                tabla.AddCell(PdfCell)

                PdfCell = New PdfPCell
                Parag = New Paragraph("Servicios Generales (" + el.Descripcion.Trim + ")", font7)
                Parag.Alignment = Element.ALIGN_CENTER
                Parag.SetLeading(0.5F, 1.0F)
                PdfCell.AddElement(Parag)
                tabla.AddCell(PdfCell)

                PdfCell = New PdfPCell
                Parag = New Paragraph(el.Cantidad, font8)
                Parag.Alignment = Element.ALIGN_CENTER
                Parag.SetLeading(0.5F, 1.0F)
                PdfCell.AddElement(Parag)
                tabla.AddCell(PdfCell)

                PdfCell = New PdfPCell
                Parag = New Paragraph(String.Format("{0:C0}", el.VrUnitario), font8)
                Parag.Alignment = Element.ALIGN_CENTER
                Parag.SetLeading(0.5F, 1.0F)
                PdfCell.AddElement(Parag)
                tabla.AddCell(PdfCell)

                PdfCell = New PdfPCell
                Parag = New Paragraph(String.Format("{0:C0}", el.VrTotal), font8)
                Parag.Alignment = Element.ALIGN_CENTER
                Parag.SetLeading(0.5F, 1.0F)
                PdfCell.AddElement(Parag)
                tabla.AddCell(PdfCell)

            Next

            PdfCell = New PdfPCell
            CellBorders(PdfCell, 0, 0, 0, 0)
            Parag = New Paragraph("", font8)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
            PdfCell.Colspan = 3
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
            Parag = New Paragraph(String.Format("{0:C0}", info.SUBTOTAL), font8)
            Parag.Alignment = Element.ALIGN_CENTER
            Parag.SetLeading(0.5F, 1.0F)
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
			Parag = New Paragraph("A este Documento se le aplican las normas relativas del Documento Soporte en adquisidores efectuadas a sujetos no obligados a facturar (Resolución 167 de 30 de diciembre de 2021). Número Autorización 18764082534020 aprobado en 20241030 prefijo DSE desde el nùmero 12171 al 16000 Vigencia: 12 meses.", font8)
			Parag.Alignment = Element.ALIGN_CENTER
            Parag.SpacingBefore = 2
            Parag.SetLeading(0.5F, 1.0F)
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
        Catch ex As Exception
            documentoPDF.CloseDocument()
            documentoPDF.Close()
            Exit Sub
        Finally
            documentoPDF.CloseDocument()
            documentoPDF.Close()
        End Try

	End Sub

	Sub ConstruirPDFNotaCredito(ByVal id As Long, ByVal document As Document)
		Dim Col_TitlePage As New BaseColor(30, 172, 167)
		Dim Col_Title As New BaseColor(14, 1, 129)
		Dim Col_Title1 As New BaseColor(204, 232, 212)
		Dim Col_Title2 As New BaseColor(242, 249, 220)
		Dim Col_Title3 As New BaseColor(211, 211, 211)

		Dim font10 As New Font(Font.FontFamily.HELVETICA, 10, Font.NORMAL)
		Dim font10B As New Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)
		Dim font12 As New Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL)
		Dim font12B As New Font(Font.FontFamily.HELVETICA, 12, Font.BOLD)
		Dim font8 As New Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL)
		Dim font8B As New Font(Font.FontFamily.HELVETICA, 8, Font.BOLD)
		Dim font7 As New Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL)
		Dim fontTitle As New Font(Font.FontFamily.HELVETICA, 10, Font.BOLD)
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

		Try
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

			tabla.AddCell(NewPdfCell("Bogotá: Calle 74 No. 11-81 - PBX: 376 9400 - Fax: 376 9400 Ext: 525", 3, False, False))
			tabla.AddCell(NewPdfCell("Medellín: Carrera 36 No. 7-46 - PBX: (4) 312 2090", 3, False, False))
			tabla.AddCell(NewPdfCell("Cali: Calle 7 Sur No. 38A-11 - Teléfono: (2) 514 0017", 3, False, False))
			tabla.AddCell(NewPdfCell("Nit: 890.319.494-5 | Página web: www.ipsos.com.co", 3, False, False))

			document.Add(tabla)

			tabla = New PdfPTable(1)
			Ancho = {Ssg(21)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 95

			Dim titulo As String = ""
			titulo = "NOTA CRÉDITO"

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
			document.Add(tabla)

			document.Add(NewRenglonEnter())

			Dim o As New FI.Ordenes
			Dim info = o.ObtenerOrdenesDeRequerimiento(id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
			Dim op As New Contratista
			Dim infoC = op.ObtenerContratista(info.ProveedorId)
			Dim pro As New Proyecto
			Dim otrabajo As New Trabajo
			Dim t As New PY_Trabajo0
			t = otrabajo.ObtenerTrabajo(info.JOBBOOK)
			Dim clas = op.ObtenerClasificacion(infoC.Clasificacion)

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
			Parag = New Paragraph("Nota Crédito No.", font10B)
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

			PdfCell = New PdfPCell
			Dim FechaElaboracion = DateTime.Parse(info.Fecha).ToString("dd/MM/yyyy")
			Parag = New Paragraph(FechaElaboracion, font10)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			Dim IdInfo = info.ID
			Parag = New Paragraph(IdInfo, font10)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			tabla.AddCell(PdfCell)

			document.Add(tabla)
			document.Add(NewRenglonEnter())

			tabla = New PdfPTable(2)
			CellBorders(PdfCell, 1, 1, 1, 1)
			Ancho = {Ssg(12), Ssg(9)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 95

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 0, 0, 0)
			Parag = New Paragraph("COMPRADOR", fontTitle)
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SpacingBefore = 3
			Parag.SpacingAfter = 3
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.Colspan = 2
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("Nombre:  IPSOS NAPOLEÓN FRANCO & CIA SAS", font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("Número de Identificación:    890.319.494 DV: 5", font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("Dirección:   CL 74 11 81 P 5", font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("Ciudad:  BOGOTÁ", font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("Teléfono:    3769400", font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			PdfCell.Colspan = 2
			tabla.AddCell(PdfCell)

			document.Add(tabla)
			document.Add(NewRenglonEnter())

			tabla = New PdfPTable(1)
			CellBorders(PdfCell, 1, 1, 1, 1)
			Ancho = {Ssg(21)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 95

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 0, 0, 0)
			Parag = New Paragraph("PROVEEDOR", fontTitle)
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SpacingBefore = 3
			Parag.SpacingAfter = 3
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.Colspan = 2
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			document.Add(tabla)
			document.Add(NewRenglonEnter())

			tabla = New PdfPTable(2)
			CellBorders(PdfCell, 1, 1, 1, 1)
			Ancho = {Ssg(12), Ssg(9)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 95
			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Dim nombreProveedor = info.Proveedor
			Parag = New Paragraph("Nombre:  " + nombreProveedor.ToString, font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Dim nit = info.ProveedorId
			Parag = New Paragraph("Número de Identificación:  " + nit.ToString, font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Dim direccion = infoC.Direccion
			Parag = New Paragraph("Dirección:  " + direccion.ToString, font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Dim ciudad = info.Ciudad
			Parag = New Paragraph("Ciudad:  " + ciudad.ToString, font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			PdfCell.Colspan = 2
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Dim telefono = infoC.Telefono
			Parag = New Paragraph("Teléfono:  " + telefono.ToString, font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Dim email = infoC.Email
			Parag = New Paragraph("Correo:  " + email.ToString, font10)
			Parag.SpacingAfter = 2
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			document.Add(tabla)
			document.Add(NewRenglonEnter())

			tabla = New PdfPTable(1)
			CellBorders(PdfCell, 1, 1, 0, 1)
			Ancho = {Ssg(21)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 95

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 1, 1)
			Parag = New Paragraph("CONCEPTO", font8B)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SpacingBefore = 3
			Parag.SpacingAfter = 3
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("(Descripcion de los productos vendidos o servicios prestados)", font8)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SpacingBefore = 3
			Parag.SpacingAfter = 3
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			document.Add(tabla)
			document.Add(NewRenglonEnter())

			tabla = New PdfPTable(7)
			CellBorders(PdfCell, 1, 1, 1, 1)
			Ancho = {Ssg(1), Ssg(4), Ssg(3), Ssg(4), Ssg(4), Ssg(2), Ssg(2)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 95

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 1, 1)
			Parag = New Paragraph("CANT", font8B)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SpacingAfter = 2
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.BackgroundColor = Col_Title3
			PdfCell.MinimumHeight = 20
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 1, 1)
			Parag = New Paragraph("CONCEPTO", font8B)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SpacingAfter = 2
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.BackgroundColor = Col_Title3
			PdfCell.MinimumHeight = 20
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 1, 1)
			Parag = New Paragraph("JOB", font8B)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SpacingAfter = 2
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.BackgroundColor = Col_Title3
			PdfCell.MinimumHeight = 20
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 1, 1)
			Parag = New Paragraph("NOMBRE JOB", font8B)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SpacingAfter = 2
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.BackgroundColor = Col_Title3
			PdfCell.MinimumHeight = 20
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 1, 1)
			Parag = New Paragraph("CUENTA CONTABLE", font8B)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SpacingAfter = 2
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.BackgroundColor = Col_Title3
			PdfCell.MinimumHeight = 20
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 1, 1)
			Parag = New Paragraph("VALOR UNITARIO", font8B)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SpacingAfter = 2
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.BackgroundColor = Col_Title3
			PdfCell.MinimumHeight = 20
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 1, 1)
			Parag = New Paragraph("VALOR TOTAL", font8B)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SpacingAfter = 2
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.BackgroundColor = Col_Title3
			PdfCell.MinimumHeight = 20
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			Dim infoD = o.obtenerDetalle(eTipoOrden.OrdenRequerimiento, id)

			For Each el As FI_OrdenDetalle_Get_Result In infoD
				PdfCell = New PdfPCell
				Parag = New Paragraph(el.Cantidad, font8)
				Parag.Alignment = Element.ALIGN_CENTER
				Parag.SetLeading(0.5F, 1.0F)
				PdfCell.MinimumHeight = 20
				PdfCell.AddElement(Parag)
				tabla.AddCell(PdfCell)

				PdfCell = New PdfPCell
				Parag = New Paragraph(el.Descripcion.Trim, font7)
				Parag.Alignment = Element.ALIGN_CENTER
				Parag.SetLeading(0.5F, 1.0F)
				PdfCell.MinimumHeight = 20
				PdfCell.AddElement(Parag)
				tabla.AddCell(PdfCell)

				PdfCell = New PdfPCell
				Parag = New Paragraph(t.JobBook, font8)
				Parag.Alignment = Element.ALIGN_CENTER
				Parag.SetLeading(0.5F, 1.0F)
				PdfCell.MinimumHeight = 20
				PdfCell.AddElement(Parag)
				tabla.AddCell(PdfCell)

				PdfCell = New PdfPCell
				Parag = New Paragraph(info.CARGADOA, font7)
				Parag.Alignment = Element.ALIGN_CENTER
				Parag.SetLeading(0.5F, 1.0F)
				PdfCell.MinimumHeight = 20
				PdfCell.AddElement(Parag)
				tabla.AddCell(PdfCell)

				PdfCell = New PdfPCell
				Parag = New Paragraph(el.NumeroCuenta.ToString + " - " + el.NombreCuenta.Trim, font7)
				Parag.Alignment = Element.ALIGN_CENTER
				Parag.SetLeading(0.5F, 1.0F)
				PdfCell.MinimumHeight = 20
				PdfCell.AddElement(Parag)
				tabla.AddCell(PdfCell)

				PdfCell = New PdfPCell
				Parag = New Paragraph(String.Format("{0:C0}", el.VrUnitario), font8)
				Parag.Alignment = Element.ALIGN_CENTER
				Parag.SetLeading(0.5F, 1.0F)
				PdfCell.MinimumHeight = 20
				PdfCell.AddElement(Parag)
				tabla.AddCell(PdfCell)

				PdfCell = New PdfPCell
				Parag = New Paragraph(String.Format("{0:C0}", el.VrTotal), font8)
				Parag.Alignment = Element.ALIGN_CENTER
				Parag.SetLeading(0.5F, 1.0F)
				PdfCell.MinimumHeight = 20
				PdfCell.AddElement(Parag)
				tabla.AddCell(PdfCell)
			Next

			Dim filas = If(infoD.Count > 6, 1, 6 - infoD.Count)

			For i = 1 To filas
				For j = 1 To 7
					PdfCell = New PdfPCell
					CellBorders(PdfCell, 1, 1, 1, 1)
					Parag = New Paragraph(" ", font8)
					Parag.Alignment = Element.ALIGN_CENTER
					Parag.SetLeading(0.5F, 1.0F)
					PdfCell.MinimumHeight = 20
					PdfCell.AddElement(Parag)
					tabla.AddCell(PdfCell)
				Next
			Next



			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("", font8)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.MinimumHeight = 20
			PdfCell.Colspan = 4
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 1, 1, 1)
			Parag = New Paragraph("TOTAL", font8B)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SpacingBefore = 3
			Parag.SpacingAfter = 3
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.BackgroundColor = Col_Title3
			PdfCell.MinimumHeight = 20
			PdfCell.AddElement(Parag)
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			Parag = New Paragraph(String.Format("{0:C0}", info.SUBTOTAL), font8)
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			PdfCell.MinimumHeight = 20
			PdfCell.Colspan = 2
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			PdfCell.MinimumHeight = 100
			Parag = New Paragraph("", fontTitle)
			PdfCell.AddElement(Parag)
			PdfCell.MinimumHeight = 100
			PdfCell.Colspan = 7
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("     ", font8)
			PdfCell.AddElement(Parag)
			PdfCell.Colspan = 4
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 1, 0, 0, 0)
			Parag = New Paragraph("Firma Beneficiario", font10)
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SpacingBefore = 3
			Parag.SpacingAfter = 3
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			PdfCell.Colspan = 3
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("     ", font8)
			PdfCell.AddElement(Parag)
			PdfCell.Colspan = 4
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("Nombre: " + nombreProveedor, font10)
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SpacingBefore = 3
			Parag.SpacingAfter = 3
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			PdfCell.Colspan = 3
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("     ", font8)
			PdfCell.AddElement(Parag)
			PdfCell.Colspan = 4
			tabla.AddCell(PdfCell)

			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			Parag = New Paragraph("C.C." + nit.ToString, font10)
			Parag.Alignment = Element.ALIGN_LEFT
			Parag.SpacingBefore = 3
			Parag.SpacingAfter = 3
			Parag.SetLeading(0.5F, 1.0F)
			PdfCell.AddElement(Parag)
			PdfCell.Colspan = 3
			tabla.AddCell(PdfCell)

			document.Add(tabla)
		Catch ex As Exception
			document.CloseDocument()
			document.Close()
			Exit Sub
		Finally
			document.CloseDocument()
			document.Close()
		End Try

	End Sub

	Sub ConstruirPDFOr(ByVal id As Long, ByVal document As Document)
		Dim font8 As New Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL)
		Dim font8B As New Font(Font.FontFamily.HELVETICA, 8, Font.BOLD)
		Dim fontC As New Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL)
		Dim fontD As New Font(Font.FontFamily.HELVETICA, 7, Font.BOLD)
		Dim path As String = Server.MapPath("~/Files/")
		Dim tabla As New PdfPTable(4)
		Dim Ancho As Single() = {Ssg(4), Ssg(9), Ssg(4), Ssg(4)}
		tabla.SetWidths(Ancho)
		tabla.DefaultCell.Border = 0
		tabla.WidthPercentage = 100

		Dim Col_Title1 As New iTextSharp.text.BaseColor(204, 232, 212)
		Dim Col_Title2 As New iTextSharp.text.BaseColor(242, 249, 220)


		Dim PdfCell As New PdfPCell

		tabla = New PdfPTable(1)
		PdfCell = New PdfPCell
		CellBorders(PdfCell, 0, 0, 0, 0)
		PdfCell.HorizontalAlignment = 3

		Try
			Dim titulo As String = ""
			titulo = "REQUERIMIENTO DE SERVICIO No. " & id

			Dim Parag As New Paragraph(titulo, font8)

			tabla = New PdfPTable(2)
			Ancho = {Ssg(10), Ssg(11)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100

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

			tabla.AddCell(NewPdfCell("Bogotá: Calle 74 No. 11-81 - PBX: 376 9400 - Fax: 376 9400 Ext: 525", 3, False, False))
			tabla.AddCell(NewPdfCell("Medellín: Carrera 36 No. 7-46 - PBX: (4) 312 2090", 3, False, False))
			tabla.AddCell(NewPdfCell("Cali: Calle 7 Sur No. 38A-11 - Teléfono: (2) 514 0017", 3, False, False))
			tabla.AddCell(NewPdfCell("Nit: 890.319.494-5 | Página web: www.ipsos.com.co", 3, False, False))

			document.Add(tabla)

			tabla = New PdfPTable(1)
			Ancho = {Ssg(21)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100
			tabla.AddCell(NewPdfCellGray(titulo, 1, True, True))
			document.Add(tabla)

			document.Add(NewRenglonEnter())

			Dim o As New FI.Ordenes
			Dim info = o.ObtenerOrdenesDeRequerimiento(id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
			Dim op As New Contratista
			Dim infoC = op.ObtenerContratista(info.ProveedorId)
			Dim pro As New Proyecto
			Dim otrabajo As New Trabajo
			Dim t As New PY_Trabajo0
			t = otrabajo.ObtenerTrabajo(info.JOBBOOK)
			Dim clas = op.ObtenerClasificacion(infoC.Clasificacion)

			tabla = New PdfPTable(5)
			Ancho = {Ssg(3), Ssg(7), Ssg(1), Ssg(3), Ssg(7)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100


			tabla.AddCell(NewPdfCell("Contratista", 3, True, True))
			tabla.AddCell(NewPdfCell(info.Proveedor, 3, False, True))
			PdfCell = New PdfPCell
			CellBorders(PdfCell, 0, 0, 0, 0)
			PdfCell.HorizontalAlignment = 1
			PdfCell.VerticalAlignment = 1
			PdfCell.Rowspan = 5
			tabla.AddCell(PdfCell)

			tabla.AddCell(NewPdfCell("Ciudad y Fecha", 3, True, True))
			tabla.AddCell(NewPdfCell(info.Ciudad & " - " & info.Fecha, 3, False, True))

			tabla.AddCell(NewPdfCell("CC / Nit", 3, True, True))
			tabla.AddCell(NewPdfCell(info.ProveedorId, 3, False, True))
			tabla.AddCell(NewPdfCell("Solicitante", 3, True, True))
			tabla.AddCell(NewPdfCell(info.NombreSolicitud, 3, False, True))

			tabla.AddCell(NewPdfCell("Direccion", 3, True, True))
			tabla.AddCell(NewPdfCell(infoC.Direccion, 3, False, True))
			tabla.AddCell(NewPdfCell("Fecha de entrega", 3, True, True))
			tabla.AddCell(NewPdfCell(info.FechaEntrega, 3, False, True))

			tabla.AddCell(NewPdfCell("Teléfono", 3, True, True))
			tabla.AddCell(NewPdfCell(infoC.Telefono, 3, False, True))
			tabla.AddCell(NewPdfCell("Forma de pago", 3, True, True))
			tabla.AddCell(NewPdfCell(info.FormaPago, 3, False, True))

			tabla.AddCell(NewPdfCell("CC / JB", 3, True, True))
			tabla.AddCell(NewPdfCell("ID: " + info.JOBBOOK + "  /  JBI: " + t.JobBook, 3, False, True))
			tabla.AddCell(NewPdfCell("Descripción CC / JB", 3, True, True))
			tabla.AddCell(NewPdfCell(info.CARGADOA, 3, False, True))

			document.Add(tabla)

			Dim infoD = o.obtenerDetalle(eTipoOrden.OrdenRequerimiento, id)

			tabla = New PdfPTable(6)
			Ancho = {Ssg(5), Ssg(2), Ssg(6), Ssg(2), Ssg(2), Ssg(2)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 100
			tabla.AddCell(NewPdfCellGray("Cta Contable", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Fecha", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Descripción", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Cantidad", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Vr Unitario", 3, True, True))
			tabla.AddCell(NewPdfCellGray("Vr Total", 3, True, True))

			Dim descripcion As String = ""
			For Each el As FI_OrdenDetalle_Get_Result In infoD
				tabla.AddCell(NewPdfCell(el.NumeroCuenta & " - " & el.NombreCuenta, 3, False, True))
				tabla.AddCell(NewPdfCell(el.Fecha, 3, False, True))
				tabla.AddCell(NewPdfCell(el.Descripcion, 3, False, True))
				tabla.AddCell(NewPdfCell(el.Cantidad, 3, False, True))
				tabla.AddCell(NewPdfCell(String.Format("{0:C0}", el.VrUnitario), 3, False, True))
				tabla.AddCell(NewPdfCell(String.Format("{0:C0}", el.VrTotal), 3, False, True))
				descripcion = descripcion & Server.HtmlDecode(el.NombreCuenta) & "; "
			Next

			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("", 3, False, False))
			tabla.AddCell(NewPdfCell("Subtotal", 3, True, True))
			tabla.AddCell(NewPdfCell(String.Format("{0:C0}", info.SUBTOTAL), 3, True, True))

			Parag = New Paragraph("“"Por la presente, en desarrollo del contrato de prestación de servicios actualmente vigente entre las partes, las partes acuerdan que el CONTRATISTA prestará los servicios especificados en este documento”"", fontC)
			Parag.SpacingBefore = 5
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			Parag.SetLeading(0.5F, 1.0F)
			document.Add(Parag)

			Parag = New Paragraph("DESCRIPCION GENERAL DE LOS SERVICIOS:", fontD)
			Parag.SpacingBefore = 5
			Parag.SpacingAfter = 3
			Parag.Alignment = Element.ALIGN_CENTER
			Parag.SetLeading(1.3F, 1.0F)
			document.Add(Parag)

			Parag = New Paragraph("El CONTRATISTA se obliga con el contratante, bajo su entera y absoluta responsabilidad, autonomía Técnica, Financiera y administrativa, a ejecutar la(s) siguiente(s) tarea(s): " & descripcion & " para el estudio relacionado  a continuación de conformidad con los requisitos metodológicos y requerimientos de la norma 20252:2019 y demás establecidos por el contratante. Dado que los servicios prestados se ejecutan de manera autónoma e independiente, no existirá vínculo laboral entre las partes.", fontC)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			Parag.SetLeading(0.5F, 1.0F)
			document.Add(Parag)

			Parag = New Paragraph("Se hace entrega de Metodología, instrumentos y especificaciones de la prestación de servicio, por cada Estudio Y Job y su(s) respectiva(s) actividad(es),", fontC)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			Parag.SetLeading(0.5F, 1.0F)
			document.Add(Parag)
			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())
			document.Add(tabla)

			Parag = New Paragraph("VALOR DE ESTE REQUERIMIENTO Y FORMA DE PAGO:", fontD)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.SpacingBefore = 5
			Parag.SpacingAfter = 3
			Parag.Alignment = Element.ALIGN_CENTER
			document.Add(Parag)

			Dim totalorden = info.SUBTOTAL
			Parag = New Paragraph("El valor del requerimiento será de " & FormatCurrency(totalorden, 0) & " el cual será pagado a los " & info.FormaPago & " siguientes contados a partir de la fecha de radicación de la cuenta de cobro, con el soporte respectivo del número de actividades realizadas de acuerdo con lo expresado en los términos de referencia. Esta suma únicamente se cancelará por tarea realizada. El presente requerimiento (RSE) se genera de acuerdo a lo contemplado en el contrato marco suscrito ante el contratista e Ipsos Napoleón Franco S.A.S", fontC)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			document.Add(NewRenglonEnter())

			Parag = New Paragraph("DURACION DE LOS SERVICIOS:", fontD)
			Parag.SpacingBefore = 5
			Parag.SpacingAfter = 3
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_CENTER
			document.Add(Parag)

			Parag = New Paragraph("La duracion del presente requerimiento será hasta el dia: " & info.FechaEntrega & " En constancia se firma en " & info.Ciudad & " el dia " & info.Fecha & ". Aceptamos esta orden en los términos y condiciones aquí estipulados La cuenta/Factura DEBE: a) hacerse a nombre de IPSOS - NAPOLEON FRANCO & CIA. S.A.S. - NIT. 890.319.494-5 b) cumplir con los requisitos establecidos en el Art. 617 del Estatuto Tributario; c) hacer referencia al número de este requerimiento;  d) radicarse, con el Vo. Bo. De la persona que recibe el servicio en la Recepción de la Compañía. Nos reservamos el derecho de recibir el servicio a que este requerimiento se refiere si no cumple con las especificaciones o la fecha de entrega estipuladas.", fontC)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			Parag = New Paragraph("Las Cuentas de Cobro se deben radicar a más tardar Ocho (8) días de finalizados los procesos Internos, de no ser así Ipsos Napoleón Franco & CIA S A S no se hace responsable de dicho pago.", fontC)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			descripcion = descripcion.Replace("; ", "")
			Dim ParagCumplimiento = New Paragraph("En la ejecución del presente RSE las partes acuerdan dar cumplimiento a los siguientes aspectos:", fontC)
			ParagCumplimiento.Alignment = Element.ALIGN_JUSTIFIED

			If (descripcion.IndexOf("Trabajo de Campo Nacional") > -1 And descripcion.IndexOf("Supervisores de encuestas") > -1 And (t.OP_MetodologiaId <> 9 And t.OP_MetodologiaId <> 10)) Or (descripcion.IndexOf("Trabajo de Campo Nacional") > -1 Or descripcion.IndexOf("Encuesta especializada") > -1 And (t.OP_MetodologiaId = 4 Or t.OP_MetodologiaId = 5)) Or descripcion.IndexOf("Otras asistencias") > -1 Then
				Parag = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS TECNICOS RECOLECCION DE DATOS PRESENCIAL No. " & id, fontD)
				Parag.SpacingBefore = 5
				Parag.SpacingAfter = 3
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_CENTER
				document.Add(Parag)
				document.Add(ParagCumplimiento)
				document.Add(NewRenglonEnter())
				Parag = New Paragraph("REQUERIMIENTOS DE CALIDAD:", fontD)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
				Parag = New Paragraph("El contratista se compromete a realizar sobre su equipo de encuestadores una supervisión mínima de 30% sobre la producción de cada uno de los encuestadores. Para tal efecto se utilizaran los formatos de supervisión establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", fontC)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
			ElseIf descripcion.IndexOf("Captura") > -1 Then
				Parag = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS TECNICOS CAPTURA DE DATOS No. " & id, fontC)
				Parag.SpacingBefore = 5
				Parag.SpacingAfter = 3
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_CENTER
				document.Add(Parag)
				document.Add(ParagCumplimiento)
				document.Add(NewRenglonEnter())
				Parag = New Paragraph("REQUERIMIENTOS DE CALIDAD:", fontD)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
				Parag = New Paragraph("El contratista se compromete a realizar sobre su equipo de capturadores una supervisión mínima de 10% sobre la producción de cada uno de los capturadores. Este porcentaje deberá ser seleccionado de manera aleatoria. Para tal efecto se utilizaran los formatos de supervisión establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", fontC)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
			ElseIf descripcion.IndexOf("Codificación Externa") > -1 Then
				Parag = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS TECNICOS CODIFICACION No. " & id, fontD)
				Parag.SpacingBefore = 5
				Parag.SpacingAfter = 3
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_CENTER
				document.Add(Parag)
				document.Add(ParagCumplimiento)
				document.Add(NewRenglonEnter())
				Parag = New Paragraph("REQUERIMIENTOS DE CALIDAD:", fontD)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
				Parag = New Paragraph("El contratista se compromete a realizar sobre su equipo de codificadores una supervisión mínima de 10% sobre la producción de cada uno de los codificadores. Este porcentaje deberá ser seleccionado de manera aleatoria. Para tal efecto se utilizaran los formatos de supervisión establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", fontC)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
			ElseIf descripcion.IndexOf("Proc de información encuestas") > -1 Then
				Parag = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS TECNICOS PROCESAMIENTO DE DATOS No. " & id, fontD)
				Parag.SpacingBefore = 5
				Parag.SpacingAfter = 3
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_CENTER
				document.Add(Parag)
				document.Add(ParagCumplimiento)
				document.Add(NewRenglonEnter())
				Parag = New Paragraph("REQUERIMIENTOS DE CALIDAD:", fontD)
				Parag.SpacingBefore = 5
				Parag.SpacingAfter = 3
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
				Parag = New Paragraph("El contratista se compromete a realizar el control de calidad del 100% del producto o servicio contratado Para tal efecto se utilizarán los formatos establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", fontC)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
			ElseIf descripcion.IndexOf("Trabajo de Campo Nacional") > -1 And descripcion.IndexOf("Supervisores de encuestas") > -1 And (t.OP_MetodologiaId = 9 Or t.OP_MetodologiaId = 10) Then
				Parag = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS TECNICOS RECOLECCION DE DATOS TELEFONICA No. " & id, fontD)
				Parag.SpacingBefore = 5
				Parag.SpacingAfter = 3
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_CENTER
				document.Add(Parag)
				document.Add(ParagCumplimiento)
				document.Add(NewRenglonEnter())
				Parag = New Paragraph("REQUERIMIENTOS DE CALIDAD:", fontD)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
				Parag = New Paragraph("El contratista se compromete a realizar sobre su equipo de encuestadores una supervisión mínima de 10% sobre la producción de cada uno de los encuestadores. Para tal efecto se utilizaran los formatos de supervisión establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", fontC)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
			Else
				Parag = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS No. " & id, fontD)
				Parag.SpacingBefore = 5
				Parag.SpacingAfter = 3
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_CENTER
				document.Add(Parag)
				document.Add(ParagCumplimiento)
				document.Add(NewRenglonEnter())
				Parag = New Paragraph("REQUERIMIENTOS DE CALIDAD:", fontD)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
				Parag = New Paragraph("El contratista se compromete a realizar el control de calidad del 100% del producto o servicio contratado Para tal efecto se utilizarán los formatos establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", fontC)
				Parag.SetLeading(0.5F, 1.0F)
				Parag.Alignment = Element.ALIGN_JUSTIFIED
				document.Add(Parag)
			End If

			document.Add(NewRenglonEnter())
			Parag = New Paragraph("PENALIDADES:", fontD)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			Parag = New Paragraph("El incumplimiento por parte del contratista de cualquiera de sus obligaciones implicará las siguientes penalidades: ", fontC)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			Parag = New Paragraph("1. El incumplimiento en los requerimientos y las especificaciones de los proyectos de investigación  conforme a los documentos entregados por la CONTRATANTE - metodología, instructivos, u otros – (Numerales 5 y 6 de la cláusula sexta del contrato marco de prestación de servicios) implicará la anulación del servicio y/o encuesta con la consecuencia de no causar remuneración alguna. Adicionalmente se acuerda que si del total de servicio y/o encuestas requeridos al CONTRATISTA llegare a presentarse una anulación superior al 3% del total de los verificados por IPSOS, la remuneración general de esta Orden de servicio se disminuirá en 10%. ", fontC)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			Parag = New Paragraph("2. El incumplimiento de la obligación relacionada con la entrega de los servicios contratados en los tiempos o plazos establecidos en la presente orden producirá una penalización del 10% del valor de la RSE.", fontC)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			document.Add(NewRenglonEnter())

			Parag = New Paragraph("PARAGRAFO 1: Para el pago de las penalidades anteriormente indicadas no será necesario requerimiento de ninguna índole al cual renuncian expresamente las partes.", fontD)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			document.Add(NewRenglonEnter())

			Parag = New Paragraph("PARAGRAFO 2: Las partes acuerdan que las penalidades anteriormente indicadas podrán descontarse de la presente RSE o de cualquier crédito que el contratista tenga a su favor en desarrollo del contrato marco de prestación de servicios celebrado entre las partes.", fontD)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			document.Add(NewRenglonEnter())

			Parag = New Paragraph("CONFIDENCIALIDAD:", fontD)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			Parag = New Paragraph("EL CONTRATISTA mantendrá la confidencialidad de la información recibida del CONTRATANTE para la ejecución y desarrollo de los servicios contratados. Se reitera en esta materia el estricto cumplimiento de la obligación consignada en la cláusula décima del contrato marco de prestación de servicios y el Acuerdo especial de confidencialidad suscrito por las partes. EL CONTRATISTA declara haber leído y tener perfectamente presente para la ejecución del presente RSE los documentos antes mencionados.", fontC)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			Parag = New Paragraph()
			Parag.SpacingBefore = 5
			Parag.Add(New Chunk("OBLIGACIONES RELACIONADAS CON LA EMERGENCIA SANITARIA:", fontD))
			Parag.Add(New Chunk("En la ejecución de este REQUERIMIENTO DE SERVICIOS ESPECIFICOS (RSE) EL CONTRATISTA tendrá las siguientes obligaciones:", fontC))
			Parag.SetLeading(0.2F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)


			Parag = New Paragraph()
			Dim Lista = New List(True)
			Lista.Add(New ListItem("Asistir a la capacitación de bioseguridad que programe IPSOS y cumplir con el protocolo de bioseguridad expuesto y entregado en dicha capacitación conforme a lo indicado en los numerales 4 y 5 del artículo 2.2.4.2.2.16 del Decreto 1072/2015.", fontC))
			Lista.Add(New ListItem("Cumplir con las medidas de verificación del protocolo de bioseguridad que IPSOS indique en la respectiva capacitación.", fontC))
			Lista.Add(New ListItem("Avisar a IPSOS de inmediato si tiene síntomas de COVID-19 tales como la fiebre, la tos seca y el cansancio, dolores y molestias, la congestión nasal, el dolor de cabeza, la conjuntivitis, el dolor de garganta, la diarrea, la pérdida del gusto o el olfato y las erupciones cutáneas o cambios de color en los dedos de las manos o los pies, etc. (numeral 3 del artículo 2.2.4.2.2.16 del Decreto 1072/2015)", fontC))
			Lista.Add(New ListItem("Cumplir con la normatividad vigente expedida por el Gobierno Nacional, Departamental o Distrital/Municipal en relación con la emergencia sanitaria.", fontC))
			Lista.Add(New ListItem("Asegurarse que el personal que EL CONTRATISTA haya contratado para los servicios cumpla en su totalidad las disposiciones de esta cláusula.", fontC))
			Lista.Add(New ListItem("Cumplir con la regulación aplicable respecto de la emergencia sanitaria según le aplique y contar con su respectivo protocolo de bioseguridad y habilitación de la alcaldía, en caso de ser requisito para operar.", fontC))
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			Parag.Add(Lista)
			document.Add(Parag)

			Parag = New Paragraph()
			Parag.SpacingBefore = 5
			Parag.Add(New Chunk("OBLIGACIONES RELACIONADAS CON EL REGLAMENTO UNICO DEL SECTOR TRABAJO:", fontD))
			Parag.Add(New Chunk("En la ejecución de este  REQUERIMIENTO DE SERVICIOS ESPECIFICOS (RSE) EL CONTRATISTA tendrá las siguientes obligaciones:", fontC))
			Dim Lista2 = New List(False, 10.0F)
			Lista2.SetListSymbol("•")
			Lista2.Add(New ListItem("Cumplir con la obligación de afiliación al Sistema general del riesgo laboral", fontC))
			Parag.Add(Lista2)
			''Parag.Add(New Chunk(Chunk.NEWLINE))
			Parag.Add(New ListItem("Para los efectos del sistema de gestión de la seguridad y salud en el trabajo los proveedores y contratistas deben cumplir frente a sus trabajadores o subcontratistas con la obligación anteriormente mencionada", fontC))
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			document.Add(NewRenglonEnter())

			Parag = New Paragraph("ACEPTAMOS ESTE REQUERIMIENTO EN LOS TÉRMINOS Y CONDICIONES AQUÍ ESTIPULADOS", fontD)
			Parag.SetLeading(0.5F, 1.0F)
			Parag.Alignment = Element.ALIGN_JUSTIFIED
			document.Add(Parag)

			document.Add(NewRenglonEnter())

			Parag = New Paragraph("", font8)
			Parag.Alignment = 3
			document.Add(Parag)
			tabla = New PdfPTable(3)
			Ancho = {Ssg(2.5), Ssg(6), Ssg(2)}
			tabla.SetWidths(Ancho)
			tabla.WidthPercentage = 50
			Dim lblContratista = New PdfPCell(New Paragraph("CONTRATISTA", font8B))
			lblContratista.HorizontalAlignment = HorizontalAlign.Left
			lblContratista.Border = BorderStyle.None

			Dim nombreContratista = New PdfPCell(New Paragraph(infoC.Nombre, font8B))
			nombreContratista.HorizontalAlignment = HorizontalAlign.Left
			nombreContratista.BorderColor = New BaseColor(255, 255, 255)

			Dim idContratista = "C.C. " + infoC.Identificacion.ToString
			Dim documentoContratista = New PdfPCell(New Paragraph(idContratista, font8B))
			documentoContratista.HorizontalAlignment = HorizontalAlign.Left
			documentoContratista.BorderColor = New BaseColor(255, 255, 255)

			document.Add(NewRenglonEnter())
			document.Add(NewRenglonEnter())

			tabla.AddCell(NewPdfCell("", 1, True, False))
			tabla.AddCell(NewPdfCell("", 1, True, True))
			tabla.AddCell(NewPdfCell("", 1, True, False))

			tabla.AddCell(NewPdfCell("", 1, True, False))
			tabla.AddCell(lblContratista)
			tabla.AddCell(NewPdfCell("", 1, True, False))

			tabla.AddCell(NewPdfCell("", 1, True, False))
			tabla.AddCell(nombreContratista)
			tabla.AddCell(NewPdfCell("", 2, True, False))

			tabla.AddCell(NewPdfCell("", 1, True, False))
			tabla.AddCell(documentoContratista)
			tabla.AddCell(NewPdfCell("", 2, True, False))

			document.Add(tabla)

		Catch ex As Exception
			document.CloseDocument()
			document.Close()
			Exit Sub
		Finally
			document.CloseDocument()
			document.Close()
		End Try

	End Sub

	Sub DescargarPDF(ByVal namefile As String)
		Dim urlFija As String
		urlFija = "\Files"
		'HACK Deuda tecnica Aquí se deberia colocar la ruta raíz en un parametro en la base de datos

		urlFija = Server.MapPath(urlFija & "\" & namefile)

		Dim path As New FileInfo(urlFija)

		Response.Clear()
		'Response.ClearContent()
		'Response.ClearHeaders()
		Response.AddHeader("Content-Disposition", "attachment; filename=" & namefile)
		Response.ContentType = "application/octet-stream"
		Response.WriteFile(urlFija)
		Response.End()
	End Sub

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

	Private Sub gvOrdenes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvOrdenes.RowCommand
		Dim idOrden As Int64
		idOrden = Me.gvOrdenes.DataKeys(CInt(e.CommandArgument))("Id")
		hfId.Value = idOrden
		construirPDF(hfTipoOrden.Value, idOrden)
		cargarAprobaciones()
		ScriptManager.RegisterStartupScript(Me, Me.GetType, "visualiza", "cargarOrden(" & idOrden & ");", True)
	End Sub

	Public Sub construirPDF(tipoOrden As eTipoOrden, idOrden As Long)
		Dim document As New Document(PageSize.LETTER, Ssg(0.5), Ssg(0.5), Ssg(0.5), Ssg(0.5))
		Dim path As String = Server.MapPath("~/Files/")
		Dim nombrepdf As String = ""
		Select Case tipoOrden
			Case eTipoOrden.OrdenCompra
				nombrepdf = "ORDENCOMPRA-" & idOrden & ".pdf"
				PdfWriter.GetInstance(document, New FileStream(path & nombrepdf, FileMode.Create))
				document.Open()
				ConstruirPDFOc(idOrden, document)
			Case eTipoOrden.OrdenServicio
				nombrepdf = "ORDENSERVICIO-" & idOrden & ".pdf"
				PdfWriter.GetInstance(document, New FileStream(path & nombrepdf, FileMode.Create))
				document.Open()
				ConstruirPDFOs(idOrden, document)
			Case eTipoOrden.OrdenRequerimiento
				nombrepdf = "ORDENREQUERIMIENTO-" & idOrden & ".pdf"
				PdfWriter.GetInstance(document, New FileStream(path & nombrepdf, FileMode.Create))
				document.Open()
				ConstruirPDFOr(idOrden, document)
		End Select
	End Sub

	Public Sub construirDocEqui(idOrden As Long)
		Dim document As New Document(PageSize.LETTER, Ssg(0.5), Ssg(0.5), Ssg(0.5), Ssg(0.5))
		Dim path As String = Server.MapPath("~/Files/")
		Dim nombrepdf As String = ""
		nombrepdf = "DOCUMENTOEQUIVALENTE-" & idOrden & ".pdf"
		PdfWriter.GetInstance(document, New FileStream(path & nombrepdf, FileMode.Create))
		document.Open()
		ConstruirPDFDocEqu(idOrden, document)
	End Sub

	Public Sub construirNotaCredito(idOrden As Long)
		Dim document As New Document(PageSize.LETTER, Ssg(0.5), Ssg(0.5), Ssg(0.5), Ssg(0.5))
		Dim path As String = Server.MapPath("~/Files/")
		Dim nombrepdf As String = ""
		nombrepdf = "NOTACREDITO-" & idOrden & ".pdf"
		PdfWriter.GetInstance(document, New FileStream(path & nombrepdf, FileMode.Create))
		document.Open()
		ConstruirPDFNotaCredito(idOrden, document)
	End Sub

	Sub cargarAprobaciones()
		Dim daOrdenes As New FI.Ordenes
		If hfTipoOrden.Value = eTipoOrden.OrdenServicio Then
			gvAprobaciones.DataSource = daOrdenes.ObtenerLogAprobacionesOrdenServicio(hfId.Value)
			gvAprobaciones.DataBind()
		ElseIf hfTipoOrden.Value = eTipoOrden.OrdenCompra Then
			gvAprobaciones.DataSource = daOrdenes.ObtenerLogAprobacionesOrdenCompra(hfId.Value)
			gvAprobaciones.DataBind()
		ElseIf hfTipoOrden.Value = eTipoOrden.OrdenRequerimiento Then
			gvAprobaciones.DataSource = daOrdenes.ObtenerLogAprobacionesOrdenRequerimiento(hfId.Value)
			gvAprobaciones.DataBind()
		End If
	End Sub
End Class
