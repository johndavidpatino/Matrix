Imports WebMatrix.Util
Imports CoreProject
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO


Public Class OrdenesdeServicioF
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ObtenerContratistas(Nothing, Nothing)
            lblidtrabajo.Text = ""
            lbljob.Text = ""
            lblnombretrabajo.Text = ""
            CargarDepartamentos()
        End If
    End Sub
    Private Sub gvContratistas_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvContratistas.PageIndexChanging
        gvContratistas.PageIndex = e.NewPageIndex
        ObtenerContratistas(Nothing, Nothing)
    End Sub
    Private Sub gvContratistas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvContratistas.RowCommand
        If e.CommandName = "Orden" Then
            hfID.Value = Int64.Parse(Me.gvContratistas.DataKeys(CInt(e.CommandArgument))("Identificacion"))
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            cargarServiciosXContratista(hfID.Value)
            If Me.gvContratistas.DataKeys(CInt(e.CommandArgument))("Activo").ToString = "NO" Then
                lblContratistaInactivo.Visible = True
                btnagregar.Visible = False
                btngenerar.Visible = False
            Else
                lblContratistaInactivo.Visible = False
                btnagregar.Visible = True
                btngenerar.Visible = True
            End If
        End If

    End Sub
    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If txtIdentificacionBuscar.Text = "" And txtNombreBuscar.Text = "" Then
            ObtenerContratistas(Nothing, Nothing)
        ElseIf txtIdentificacionBuscar.Text = "" Then
            ObtenerContratistas(Nothing, txtNombreBuscar.Text)
            txtNombreBuscar.Text = String.Empty
        ElseIf txtNombreBuscar.Text = "" Then
            ObtenerContratistas(txtIdentificacionBuscar.Text, Nothing)
            txtIdentificacionBuscar.Text = String.Empty
        End If
    End Sub
    Protected Sub btnbuscartrabajo_Click(sender As Object, e As EventArgs) Handles btnbuscartrabajo.Click
        Dim oetrabajo As New PY_Trabajo0
        Dim op As New ProcesosInternos
        Dim Presupuestos As New List(Of CC_PresupuestosInternosGet_Result)
        If txtTrabajo.Text = "" Then
            ShowNotification("Ingrese id de Trabajo a buscar", ShowNotifications.InfoNotification)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            lblidtrabajo.Text = ""
            lbljob.Text = ""
            lblnombretrabajo.Text = ""
            txtsolicitado.Text = String.Empty
            Exit Sub
        Else
            oetrabajo = Obtenertrabajo(txtTrabajo.Text)
            If IsNothing(oetrabajo) Then
                ShowNotification("Trabajo No Encontrado", ShowNotifications.InfoNotification)
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                lblidtrabajo.Text = ""
                lbljob.Text = ""
                lblnombretrabajo.Text = ""
                txtsolicitado.Text = String.Empty
                Exit Sub
            Else
                Presupuestos = op.ObtenerPresupuestosAreasInternas(txtTrabajo.Text)
                If Presupuestos.Count < 0 Then
                    ShowNotification("Presupuesto no Encontrado!!!", ShowNotifications.InfoNotification)
                    ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                    Exit Sub
                End If
            End If
            lblidtrabajo.Text = oetrabajo.id
            lblnombretrabajo.Text = oetrabajo.NombreTrabajo
            lbljob.Text = oetrabajo.JobBook
            txtsolicitado.Text = If(oetrabajo.COE Is Nothing, "", op.InformacionCoe(oetrabajo.COE).Item(0).Nombre)
            ValorEncuesta(lblidtrabajo.Text)
            cargarGrillaOrdenes(Nothing, oetrabajo.id, hfID.Value)
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            End If

    End Sub
    Protected Sub btnagregar_Click(sender As Object, e As EventArgs) Handles btnagregar.Click
        If lblidtrabajo.Text = "" Then
            ShowNotification("Ingrese id de Trabajo a buscar", ShowNotifications.InfoNotification)
            Exit Sub
        Else
            lblPorcentaje.Visible = True
            txtPorcentaje.Visible = True
            btnPorcentaje.Visible = True
            AdicionarGv()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If

    End Sub

    Protected Sub btnPorcentaje_Click(sender As Object, e As EventArgs) Handles btnPorcentaje.Click
        Dim TOTALORDEN As Double

        lblActicipo.Visible = True
        lblVrAnticipo.Visible = True


        For x = 0 To GvDetalleOrden.Rows.Count - 1
            Dim CANTIDAD As Double
            Dim VRUNITARIO As Double
            Dim TOTALTRABAJO As Double

            CANTIDAD = GvDetalleOrden.Rows(x).Cells(6).Text
            VRUNITARIO = GvDetalleOrden.Rows(x).Cells(7).Text
            TOTALTRABAJO = CANTIDAD * VRUNITARIO

            TOTALORDEN = TOTALORDEN + TOTALTRABAJO
        Next

        If txtPorcentaje.Text > 0 Then

            hfPorcentaje.Value = txtPorcentaje.Text / 100
            hfVrAnticipo.Value = TOTALORDEN * hfPorcentaje.Value
            hfPagoFinal.Value = TOTALORDEN - hfVrAnticipo.Value

            lblVrAnticipo.InnerText = FormatCurrency(hfVrAnticipo.Value, 0)
        Else
            hfPorcentaje.Value = 0
            hfVrAnticipo.Value = 0
            hfPagoFinal.Value = 0
            lblVrAnticipo.InnerText = FormatCurrency(0, 0)

        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btngenerar_Click(sender As Object, e As EventArgs) Handles btngenerar.Click
        HfOrdenId.Value = GuardarOrden(hfID.Value, 0, Session("IDUsuario"), lblidtrabajo.Text, hfIdEvaluaFactura.Value, ddlDepartamento.SelectedValue, ddlCiudad.SelectedValue)
        CrearOrderdeServicio()
        lblidtrabajo.Text = ""
        lbljob.Text = ""
        lblnombretrabajo.Text = ""
        ddlServicio.ClearSelection()
        ddlDepartamento.ClearSelection()
        ddlCuentasContables.ClearSelection()
        ddlCiudad.ClearSelection()
        txtEvaluaFactura.Text = String.Empty
        txtcantidad.Text = String.Empty
        txtfechaduracion.Text = String.Empty
        txtTrabajo.Text = String.Empty
        txtvrunitario.Text = String.Empty
        txtIdentificacionBuscar.Text = String.Empty
        txtNombreBuscar.Text = String.Empty
        txtsolicitado.Text = String.Empty
        GvDetalleOrden.DataSource = Nothing
        GvDetalleOrden.DataBind()
        GvOrdenes.DataSource = Nothing
        GvOrdenes.DataBind()
        lblPorcentaje.Visible = False
        txtPorcentaje.Visible = False
        btnPorcentaje.Visible = False
        lblActicipo.Visible = False
        lblVrAnticipo.Visible = False
        txtPorcentaje.Text = String.Empty
        lblVrAnticipo.InnerText = ""
    End Sub
    Sub ValorEncuesta(ByVal Trabajoid As Int64)
        Dim op As New ProcesosInternos
        txtvrunitario.Text = FormatCurrency(op.ValorEncuestaContratista(Trabajoid), 0)
    End Sub
    Sub AdicionarGv()
        Dim miDataTable As New DataTable
        Dim dRow As DataRow
        Dim InfoContr As New TH_Contratistas
        Dim Cont As New CoreProject.Contratista
        Dim daProcesosInternos As New ProcesosInternos

        InfoContr = Cont.ObtenerContratista(hfID.Value)

        miDataTable.Columns.Add("Id")
        miDataTable.Columns.Add("NombreTrabajo")
        miDataTable.Columns.Add("Servicio")
        miDataTable.Columns.Add("CuentaContable")
        miDataTable.Columns.Add("ServicioId")
        miDataTable.Columns.Add("CuentaContableId")
        miDataTable.Columns.Add("NumeroCuenta")
        miDataTable.Columns.Add("Fecha")
        miDataTable.Columns.Add("Cantidad")
        miDataTable.Columns.Add("VrUnitario")
        miDataTable.Columns.Add("VrTotal")

        For i = 0 To GvDetalleOrden.Rows.Count - 1
            dRow = miDataTable.NewRow()
            Dim row As GridViewRow = GvDetalleOrden.Rows(i)
            dRow("Id") = row.Cells(0).Text
            dRow("NombreTrabajo") = Server.HtmlDecode(row.Cells(1).Text)
            dRow("Servicio") = row.Cells(2).Text
            dRow("CuentaContable") = Server.HtmlDecode(row.Cells(4).Text)
            dRow("ServicioId") = GvDetalleOrden.DataKeys(i)("ServicioId")
            dRow("CuentaContableId") = GvDetalleOrden.DataKeys(i)("CuentaContableId")
            dRow("NumeroCuenta") = Server.HtmlDecode(row.Cells(3).Text)
            dRow("Fecha") = row.Cells(5).Text
            dRow("Cantidad") = row.Cells(6).Text
            dRow("VrUnitario") = row.Cells(7).Text
            dRow("VrTotal") = row.Cells(8).Text

            miDataTable.Rows.Add(dRow)
        Next

        dRow = miDataTable.NewRow()
        dRow("Id") = lblidtrabajo.Text
        dRow("NombreTrabajo") = Server.HtmlDecode(lblnombretrabajo.Text) & "-" & lbljob.Text

        dRow("ServicioId") = ddlServicio.SelectedValue
        dRow("Servicio") = Server.HtmlDecode(ddlServicio.SelectedItem.ToString)

        Dim oCuentaContable = daProcesosInternos.CuentasContablesGet(Nothing, Nothing, ddlCuentasContables.SelectedValue, Nothing).FirstOrDefault

        dRow("CuentaContableId") = ddlCuentasContables.SelectedValue
        dRow("CuentaContable") = Server.HtmlDecode(ddlCuentasContables.SelectedItem.Text)
        dRow("NumeroCuenta") = oCuentaContable.NumeroCuenta

        dRow("Fecha") = Date.UtcNow.AddHours(-5)
        dRow("Cantidad") = txtcantidad.Text
        dRow("VrUnitario") = txtvrunitario.Text
        dRow("VrTotal") = CDbl(txtcantidad.Text) * CDbl(txtvrunitario.Text)
        hfidActividad.Value = Server.HtmlDecode(ddlServicio.SelectedItem.ToString)
        miDataTable.Rows.Add(dRow)
        GvDetalleOrden.DataSource = miDataTable
        GvDetalleOrden.DataBind()
    End Sub
    Sub ObtenerContratistas(ByVal Identificacion As Int64?, ByVal Nombre As String)
        Dim op As New CoreProject.Contratista
        gvContratistas.DataSource = op.ObtenerContratistas(Identificacion, Nombre, Nothing)
        gvContratistas.DataBind()
    End Sub
    Sub ObtenerInformacionContratista(ByVal Identificacion As Int64)
        Dim e As New TH_Contratistas
        Dim op As New CoreProject.Contratista
        e = op.ObtenerContratista(Identificacion)
    End Sub
    Public Function Obtenertrabajo(ByVal TrabajoId As Int64)
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo = oTrabajo.ObtenerTrabajo(TrabajoId)
        Return oeTrabajo
    End Function
    Sub cargarServiciosXContratista(ByVal idContratista As Int64)
        Dim op As New CC_FinzOpe
        Dim daContratista As New Contratista

        Dim lstServiciosContratista = daContratista.ObtenerServiciosContratista(idContratista)

        Me.ddlServicio.DataSource = lstServiciosContratista
        Me.ddlServicio.DataValueField = "ServicioId"
        Me.ddlServicio.DataTextField = "ServicioDescripcion"
        Me.ddlServicio.DataBind()

        ddlServicio.Items.Insert(0, New WebControls.ListItem("--Seleccione--", -1))
        ddlServicio.SelectedIndex = 0
    End Sub
    Public Function GuardarOrden(ByVal ContratistaId As Int64, ByVal Vrtotal As Double, ByVal UsuarioId As Int64, ByVal Trabajoid As Int64, SolicitadoPor As Int64, departamento As Int64, ciudad As Int64) As Decimal
        Dim OrdenId As Int64
        Dim op As New ProcesosInternos
        OrdenId = op.ordendeservicioadd(ContratistaId, Nothing, departamento, ciudad, Nothing, Nothing, Nothing, SolicitadoPor, Nothing, Trabajoid, Nothing, Nothing, Nothing, Nothing, Vrtotal, UsuarioId, Nothing, ProcesosInternos.EEstadosOrdenes.Generada, Nothing, Nothing, txtPorcentaje.Text, hfVrAnticipo.Value, hfPagoFinal.Value)
        actualizarEstadoOrden(OrdenId, ProcesosInternos.EEstadosOrdenes.Creada, Nothing, UsuarioId, DateTime.Now)
        actualizarEstadoOrden(OrdenId, ProcesosInternos.EEstadosOrdenes.Generada, Nothing, UsuarioId, DateTime.Now)
        Return OrdenId
    End Function
    Sub CrearOrderdeServicio()
        Dim TOTALORDEN As Double
        Dim op As New PresupInt
        Dim ot As New ProcesosInternos
        Dim InfoContr As New List(Of TH_ContratistasGet_Result)
        Dim Cont As New CoreProject.Contratista
        InfoContr = Cont.ObtenerContratistas(hfID.Value, Nothing, Nothing)
        Dim otrabajo As New Trabajo
        Dim t = otrabajo.ObtenerTrabajo(txtTrabajo.Text)
        Dim oInfo = Cont.ObtenerContratista(InfoContr(0).Identificacion)
        Dim clas = Cont.ObtenerClasificacion(oInfo.Clasificacion)

        Dim urlFija As String
        Dim Url As String
        Dim Consecutivo As List(Of CC_ConsecutivoOrdendeServicio_Result) ' Crear tabla Para Guardar Inf de la Ordenes
        Consecutivo = ot.ConsecutivoOrdendeServicio
        Dim Ordenes = ot.OrdenesdeServicioGet(Nothing, HfOrdenId.Value, Nothing).FirstOrDefault

        Dim path As String = Server.MapPath("~/Images/")

        urlFija = "~/OrdenesdeServicio/"
        Url = "..\OrdenesdeServicio\"
        urlFija = Server.MapPath(urlFija & "\")
        Dim Persona As New TH_PersonasGET_Result
        Dim pdfw As PdfWriter
        Dim documentoPDF As New Document(iTextSharp.text.PageSize.A4, 10, 10, 20, 20) 'Creamos el objeto documento PDF
        pdfw = PdfWriter.GetInstance(documentoPDF, New FileStream(urlFija & "\" & "OrdendeServicio-" & t.JobBook & "-" & Replace(Ordenes.Nombre, "&", "Y") & ".pdf", FileMode.Create))
        documentoPDF.Open()



        documentoPDF.NewPage()
        Dim aTable = New iTextSharp.text.pdf.PdfPTable(2)
        aTable.DefaultCell.Border = BorderStyle.None

        Dim Imagen As iTextSharp.text.Image
        Imagen = iTextSharp.text.Image.GetInstance(path & "logo-titulo2.png")
        Imagen.ScalePercent(10)
        Dim Img = New PdfPCell
        Img.Border = 2
        Img.AddElement(Imagen)
        aTable.AddCell(Img)
        aTable.AddCell(New Paragraph("Consecutivo N° " & HfOrdenId.Value, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))

        Dim aTable1 = New iTextSharp.text.pdf.PdfPTable(2)

        Dim C1 = New PdfPCell(New Paragraph("REQUERIMIENTO DE SERVICIOS TÉCNICOS", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        C1.HorizontalAlignment = 2
        Dim C2 = New PdfPCell(New Paragraph(lbljob.Text & " " & lblidtrabajo.Text, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.BOLD)))
        C2.HorizontalAlignment = 3
        aTable1.AddCell(C1)
        aTable1.AddCell(C2)



        Dim aTable3 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

        aTable3.DefaultCell.Border = BorderStyle.Double
        Dim Cell1 = New PdfPCell(New Paragraph("INFORMACION GENERAL", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        Cell1.Colspan = 3
        Cell1.HorizontalAlignment = 1
        aTable3.AddCell(Cell1)


        Dim aTable4 = New iTextSharp.text.pdf.PdfPTable(2)
        aTable4.DefaultCell.Border = BorderStyle.Double

        Dim C6 = New PdfPCell(New Paragraph("CONTRATISTA", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C6.HorizontalAlignment = 3
        Dim C7 = New PdfPCell(New Paragraph(Server.HtmlDecode(InfoContr.Item(0).Nombre), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C7.HorizontalAlignment = 3
        Dim C8 = New PdfPCell(New Paragraph("C.C./NIT", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C8.HorizontalAlignment = 3
        Dim C9 = New PdfPCell(New Paragraph(InfoContr.Item(0).Identificacion, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C9.HorizontalAlignment = 3

        Dim C10 = New PdfPCell(New Paragraph("DIRECCION", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C10.HorizontalAlignment = 3
        Dim C11 = New PdfPCell(New Paragraph(InfoContr.Item(0).Direccion, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C11.HorizontalAlignment = 3
        Dim C12 = New PdfPCell(New Paragraph("TELEFONO", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C12.HorizontalAlignment = 3
        Dim C13 = New PdfPCell(New Paragraph(InfoContr.Item(0).Telefono, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C13.HorizontalAlignment = 3
        Dim C14 = New PdfPCell(New Paragraph("CIUDAD", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C14.HorizontalAlignment = 3
        Dim C15 = New PdfPCell(New Paragraph(InfoContr.Item(0).Ciudad, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C15.HorizontalAlignment = 3

        Dim C16 = New PdfPCell(New Paragraph("CONTRATANTE", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C16.HorizontalAlignment = 3
        Dim C17 = New PdfPCell(New Paragraph("IPSOS-NAPOLEON FRANCO Y CIA. S.A.S (EN ADELANTE EL CONTRATANTE)", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C17.HorizontalAlignment = 3
        Dim C18 = New PdfPCell(New Paragraph("NIT", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C18.HorizontalAlignment = 3
        Dim C19 = New PdfPCell(New Paragraph("890.319.494          DV:5 ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C19.HorizontalAlignment = 3

        Dim C20 = New PdfPCell(New Paragraph("DIRECCION", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C20.HorizontalAlignment = 3
        Dim C21 = New PdfPCell(New Paragraph("Calle 74   11 – 81 Piso 5  BOGOTA D.C", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C21.HorizontalAlignment = 3
        Dim C22 = New PdfPCell(New Paragraph("TELEFONO", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        C22.HorizontalAlignment = 3
        Dim C23 = New PdfPCell(New Paragraph("6286600", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))


        aTable4.AddCell(C6)
        aTable4.AddCell(C7)
        aTable4.AddCell(C8)
        aTable4.AddCell(C9)

        aTable4.AddCell(C10)
        aTable4.AddCell(C11)
        aTable4.AddCell(C12)
        aTable4.AddCell(C13)
        aTable4.AddCell(C14)
        aTable4.AddCell(C15)

        aTable4.AddCell(C16)
        aTable4.AddCell(C17)
        aTable4.AddCell(C18)
        aTable4.AddCell(C19)

        aTable4.AddCell(C20)
        aTable4.AddCell(C21)
        aTable4.AddCell(C22)
        aTable4.AddCell(C23)


        Dim aTable5 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable5.DefaultCell.Border = BorderStyle.Double
        Dim Cell2 = New PdfPCell(New Paragraph("DESCRIPCION GENERAL DE LOS SERVICIOS:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        Cell2.Colspan = 3

        Cell2.HorizontalAlignment = 3
        aTable5.AddCell(Cell2)
        Dim descripcion As String = ""
        For x = 0 To GvDetalleOrden.Rows.Count - 1
            descripcion = descripcion & Server.HtmlDecode(GvDetalleOrden.Rows(x).Cells(4).Text) & "; "
        Next

        Dim aTable7 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable7.DefaultCell.Border = BorderStyle.Double
        Dim Cell4 = New PdfPCell(New Paragraph("El CONTRATISTA se obliga con el contratante, bajo su entera y absoluta responsabilidad, autonomía Técnica, Financiera y administrativa, a ejecutar la(s) siguiente(s) tarea(s): " & descripcion & " para el estudio relacionado  a continuación de conformidad con los requisitos metodológicos y requerimientos de la norma 20252:2019 y demás establecidos por el contratante. Dado que los servicios prestados se ejecutan de manera autónoma e independiente, no existirá vínculo laboral entre las partes.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        Cell4.Colspan = 3
        Cell4.HorizontalAlignment = 3
        aTable7.AddCell(Cell4)

        Cell4 = New PdfPCell(New Paragraph("Se hace entrega de Metodología, instrumentos y especificaciones de la prestación de servicio, por cada Estudio Y Job y su(s) respectiva(s) actividad(es),", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        Cell4.Colspan = 3
        Cell4.HorizontalAlignment = 3
        aTable7.AddCell(Cell4)

        Dim aTable8 = New iTextSharp.text.pdf.PdfPTable(6)
        aTable8.DefaultCell.Border = BorderStyle.Double

        Dim CL8 = New PdfPCell(New Paragraph("ESTUDIO Y JOB", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL8.HorizontalAlignment = 1
        Dim CL9 = New PdfPCell(New Paragraph("SERVICIO", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL9.HorizontalAlignment = 1
        Dim CL10 = New PdfPCell(New Paragraph("CUENTA CONTABLE", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL10.HorizontalAlignment = 1
        Dim CL11 = New PdfPCell(New Paragraph("CANTIDAD", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL11.HorizontalAlignment = 1
        Dim CL12 = New PdfPCell(New Paragraph("VALOR UNITARIO", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL12.HorizontalAlignment = 2
        Dim CL13 = New PdfPCell(New Paragraph("VALOR TOTAL", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL13.HorizontalAlignment = 2

        aTable8.AddCell(CL8)
        aTable8.AddCell(CL9)
        aTable8.AddCell(CL10)
        aTable8.AddCell(CL11)
        aTable8.AddCell(CL12)
        aTable8.AddCell(CL13)

        'Ciclo
        For x = 0 To GvDetalleOrden.Rows.Count - 1
            Dim CANTIDAD As Double
            Dim VRUNITARIO As Double
            Dim TOTALTRABAJO As Double


            Dim CL14 = New PdfPCell(New Paragraph(Server.HtmlDecode(GvDetalleOrden.Rows(x).Cells(1).Text), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL14.HorizontalAlignment = 1
            Dim CL15 = New PdfPCell(New Paragraph(Server.HtmlDecode(GvDetalleOrden.Rows(x).Cells(2).Text), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            C15.HorizontalAlignment = 1
            Dim CL16 = New PdfPCell(New Paragraph(Server.HtmlDecode(GvDetalleOrden.Rows(x).Cells(3).Text & " - " & GvDetalleOrden.Rows(x).Cells(4).Text), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL16.HorizontalAlignment = 1
            Dim CL17 = New PdfPCell(New Paragraph(GvDetalleOrden.Rows(x).Cells(6).Text, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL17.HorizontalAlignment = 1
            Dim CL18 = New PdfPCell(New Paragraph(FormatCurrency(GvDetalleOrden.Rows(x).Cells(7).Text, 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL18.HorizontalAlignment = 2
            CANTIDAD = GvDetalleOrden.Rows(x).Cells(6).Text
            VRUNITARIO = GvDetalleOrden.Rows(x).Cells(7).Text
            TOTALTRABAJO = CANTIDAD * VRUNITARIO

            Dim CL19 = New PdfPCell(New Paragraph(FormatCurrency(TOTALTRABAJO, 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            CL19.HorizontalAlignment = 2

            aTable8.AddCell(CL14)
            aTable8.AddCell(CL15)
            aTable8.AddCell(CL16)
            aTable8.AddCell(CL17)
            aTable8.AddCell(CL18)
            aTable8.AddCell(CL19)

            TOTALORDEN = TOTALORDEN + TOTALTRABAJO
            Dim cuentaId = CType(GvDetalleOrden.DataKeys(x)("CuentaContableId"), Long)
            Dim OrdenId = CType(HfOrdenId.Value, Int64)
            Dim servicio = CType(GvDetalleOrden.DataKeys(x)("ServicioId"), Long)
            ot.DetalleOrdendeServicioAdd(OrdenId, CANTIDAD, VRUNITARIO, TOTALORDEN, cuentaId, servicio)

        Next
        ot.ActualizarTotalOrden(TOTALORDEN, HfOrdenId.Value)
        Dim aTable9a = New iTextSharp.text.pdf.PdfPTable(2)

        Dim CL20a = New PdfPCell(New Paragraph("Porcentaje Anticipo:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL20a.HorizontalAlignment = 2
        Dim CL21a = New PdfPCell(New Paragraph(FormatPercent(hfPorcentaje.Value, 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL21a.HorizontalAlignment = 2
        aTable9a.AddCell(CL20a)
        aTable9a.AddCell(CL21a)

        Dim aTable9 = New iTextSharp.text.pdf.PdfPTable(2)

        Dim CL20 = New PdfPCell(New Paragraph("Valor Anticipo Pago parcial:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL20.HorizontalAlignment = 2
        Dim CL21 = New PdfPCell(New Paragraph(FormatCurrency(hfVrAnticipo.Value, 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL21.HorizontalAlignment = 2
        aTable9.AddCell(CL20)
        aTable9.AddCell(CL21)


        Dim aTable10 = New iTextSharp.text.pdf.PdfPTable(2)

        Dim CL22 = New PdfPCell(New Paragraph("Pago final del estudio:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL22.HorizontalAlignment = 2
        Dim CL23 = New PdfPCell(New Paragraph(FormatCurrency(hfPagoFinal.Value, 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL23.HorizontalAlignment = 2
        aTable10.AddCell(CL22)
        aTable10.AddCell(CL23)


        Dim aTable11 = New iTextSharp.text.pdf.PdfPTable(2)

        Dim CL24 = New PdfPCell(New Paragraph("Pago a cancelar en esta orden:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL24.HorizontalAlignment = 2
        Dim CL25 = New PdfPCell(New Paragraph(FormatCurrency(TOTALORDEN, 0), FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL25.HorizontalAlignment = 2


        aTable11.AddCell(CL24)
        aTable11.AddCell(CL25)



        Dim aTable12 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable12.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable12.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable12.DefaultCell.Border = BorderStyle.Double
        Dim CL26 = New PdfPCell(New Paragraph("VALOR DE ESTE REQUERIMIENTO Y FORMA DE PAGO", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL26.Colspan = 3

        CL26.HorizontalAlignment = 1
        aTable12.AddCell(CL26)


        Dim aTable13 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable13.DefaultCell.Border = BorderStyle.Double
        Dim CL27 = New PdfPCell(New Paragraph("El valor del requerimiento será de " & FormatCurrency(TOTALORDEN, 0) & " el cual será pagado a los " & clas.DiasPago & " siguientes contados a partir de la fecha de radicación de la cuenta de cobro, con el soporte respectivo del número de actividades realizadas de acuerdo con lo expresado en los términos de referencia. Esta suma únicamente se cancelará por tarea realizada. El presente requerimiento (RSE) se genera de acuerdo a lo contemplado en el contrato marco suscrito ante el contratista e Ipsos Napoleón Franco S.A.S", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL27.Colspan = 3

        CL27.HorizontalAlignment = 3
        aTable13.AddCell(CL27)


        Dim aTable14 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable14.DefaultCell.Border = BorderStyle.Double
        Dim CL28 = New PdfPCell(New Paragraph("DURACION DE LOS SERVICIOS", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL28.Colspan = 3

        CL28.HorizontalAlignment = 1
        aTable14.AddCell(CL28)


        Dim aTable15 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable15.DefaultCell.Border = BorderStyle.Double
        Dim CL29 = New PdfPCell(New Paragraph("La duracion del presente requerimiento será hasta el dia: " & txtfechaduracion.Text & " En constancia se firma en " & InfoContr.Item(0).Ciudad & " El dia " & Now() & " Aceptamos esta orden en los terminos y condiciones aquí estipulados La cuenta/Factura DEBE: a) hacerse a nombre de IPSOS - NAPOLEON FRANCO & CIA. S.A.S. - NIT. 890.319.494-5 b) cumplir con los requisitos establecidos en el Art. 617 del Estatuto Tributario; c) hacer referencia al número de este requerimiento;  d) radicarse, con el Vo. Bo. De la persona que recibe el servicio en la Recepción de la Compañía. Nos reservamos el derecho de recibir el servicio a que este requerimiento se refiere si no cumple con las especificaciones o la fecha de entrega estipuladas.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL29.Colspan = 3

        CL29.HorizontalAlignment = 3
        aTable15.AddCell(CL29)



        Dim aTable16 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable16.DefaultCell.Border = BorderStyle.Double
        Dim CL30 = New PdfPCell(New Paragraph("Las Cuentas de Cobro se deben radicar a más tardar Ocho (8) días de finalizados los procesos Internos, de no ser así Ipsos Napoleón Franco & CIA S A S no se hace responsable de dicho pago.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        CL30.Colspan = 3
        CL30.HorizontalAlignment = 3
        aTable16.AddCell(CL30)
        aTable16.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

        Dim aTable17 = New iTextSharp.text.pdf.PdfPTable(1)
        aTable17.DefaultCell.Border = BorderStyle.Double
        Dim CL31 = New PdfPCell(New Paragraph("Solicitador Por: " & txtsolicitado.Text, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))
        CL31.Colspan = 3

        CL31.HorizontalAlignment = 3
        aTable17.AddCell(CL31)

        Dim aTable18 = New iTextSharp.text.pdf.PdfPTable(2)
        aTable18.DefaultCell.Border = Rectangle.NO_BORDER
        aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
        aTable18.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

        Dim Cl32 = New PdfPCell(New Paragraph("EL CONTRATISTA,", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        Cl32.HorizontalAlignment = 3

        Dim Cl33 = New PdfPCell(New Paragraph("EL CONTRATANTE,", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        Cl33.HorizontalAlignment = 3

        Dim Cl34 = New PdfPCell(New Paragraph(InfoContr.Item(0).Nombre, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        Cl34.HorizontalAlignment = 3

        Dim Cl35 = New PdfPCell(New Paragraph("Pablo Nelson Ayala Beltrán", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        Cl35.HorizontalAlignment = 3

        Dim Cl36 = New PdfPCell(New Paragraph(InfoContr.Item(0).Identificacion, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        Cl36.HorizontalAlignment = 3

        Dim Cl37 = New PdfPCell(New Paragraph("CC 80778776", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
        Cl37.HorizontalAlignment = 3


        aTable18.AddCell(Cl32)
        aTable18.AddCell(Cl33)
        aTable18.AddCell(Cl34)
        aTable18.AddCell(Cl35)
        aTable18.AddCell(Cl36)
        aTable18.AddCell(Cl37)

        documentoPDF.Add(aTable)
        documentoPDF.Add(aTable1)
        documentoPDF.Add(aTable3)
        documentoPDF.Add(aTable4)
        documentoPDF.Add(aTable5)
        'documentoPDF.Add(aTable6)
        documentoPDF.Add(aTable7)
        documentoPDF.Add(aTable8)
        documentoPDF.Add(aTable9a)
        documentoPDF.Add(aTable9)
        documentoPDF.Add(aTable10)
        documentoPDF.Add(aTable11)
        documentoPDF.Add(aTable12)
        documentoPDF.Add(aTable13)
        documentoPDF.Add(aTable14)
        documentoPDF.Add(aTable15)
        documentoPDF.Add(aTable16)
        documentoPDF.Add(aTable17)
        documentoPDF.Add(aTable18)


        '----------------  ANEXO RSE ---------------------

        If descripcion.IndexOf("Trabajo de Campo Nacional") > -1 Or descripcion.IndexOf("Captura") > -1 Or descripcion.IndexOf("Codificación Externa") > -1 Or descripcion.IndexOf("Proc de información encuestas") > -1 Or descripcion.IndexOf("Otras asistencias") > -1 Or descripcion.IndexOf("Reclutamiento") > -1 Or descripcion.IndexOf("Encuesta especializada") > -1 OrElse descripcion.IndexOf("Transporte") > -1 OrElse descripcion.IndexOf("Taxis y Buses") > -1 OrElse descripcion.IndexOf("Salones y alimentación") > -1 OrElse descripcion.IndexOf("Salones y alimentación") > -1 OrElse descripcion.IndexOf("Casino y Restaurante") > -1 OrElse descripcion.IndexOf("Útiles Papelería y Fotocopias") > -1 OrElse descripcion.IndexOf("Alojamiento y Manutención") > -1 Then

            documentoPDF.NewPage()
            Dim aTable19 = New iTextSharp.text.pdf.PdfPTable(1)
            aTable19.DefaultCell.Border = Rectangle.NO_BORDER

            Dim Imagen1 As iTextSharp.text.Image
            Imagen1 = iTextSharp.text.Image.GetInstance(path & "logo-titulo2.png")
            Imagen1.ScalePercent(10)
            Dim Img1 = New PdfPCell
            Img1.Border = 2
            Img1.AddElement(Imagen1)
            aTable19.AddCell(Img1)
            aTable19.AddCell(New Paragraph("Consecutivo N° " & HfOrdenId.Value, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD)))

            Dim aTable20 = New iTextSharp.text.pdf.PdfPTable(1)
            aTable20.DefaultCell.Border = Rectangle.NO_BORDER

            Dim CARDP1 = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS TECNICOS RECOLECCION DE DATOS PRESENCIAL No. " & HfOrdenId.Value, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD))
            CARDP1.Alignment = 1

            Dim CACAP1 = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS TECNICOS CAPTURA DE DATOS No. " & HfOrdenId.Value, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD))
            CACAP1.Alignment = 1

            Dim CACOD1 = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS TECNICOS CODIFICACION No. " & HfOrdenId.Value, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD))
            CACOD1.Alignment = 1

            Dim CAPRO1 = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS TECNICOS PROCESAMIENTO DE DATOS No. " & HfOrdenId.Value, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD))
            CAPRO1.Alignment = 1

            Dim CARDT1 = New Paragraph("Anexo REQUERIMIENTO DE SERVICIOS TECNICOS RECOLECCION DE DATOS TELEFONICA No. " & HfOrdenId.Value, FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD))
            CARDT1.Alignment = 1


            Dim aTableE1 = New iTextSharp.text.pdf.PdfPTable(2)
            aTableE1.DefaultCell.Border = Rectangle.NO_BORDER
            aTableE1.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            aTableE1.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

            Dim CA2 = New Paragraph("En la ejecución del presente RSE las partes acuerdan dar cumplimiento a los siguientes aspectos:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CA2.Alignment = 3

            Dim aTableE2 = New iTextSharp.text.pdf.PdfPTable(2)
            aTableE2.DefaultCell.Border = Rectangle.NO_BORDER
            aTableE2.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            aTableE2.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

            Dim CA3 = New Paragraph("REQUERIMIENTOS DE CALIDAD:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD))
            CA3.Alignment = 3

            Dim CACAP23 = New Paragraph("El contratista se compromete a realizar sobre su equipo de capturadores una supervisión mínima de 10% sobre la producción de cada uno de los capturadores. Este porcentaje deberá ser seleccionado de manera aleatoria. Para tal efecto se utilizaran los formatos de supervisión establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CACAP23.Alignment = 3

            Dim CACOD23 = New Paragraph("El contratista se compromete a realizar sobre su equipo de codificadores una supervisión mínima de 10% sobre la producción de cada uno de los codificadores. Este porcentaje deberá ser seleccionado de manera aleatoria. Para tal efecto se utilizaran los formatos de supervisión establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CACOD23.Alignment = 3

            Dim CAPRO23 = New Paragraph("El contratista se compromete a realizar el control de calidad del 100% del producto o servicio contratado Para tal efecto se utilizarán los formatos establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CAPRO23.Alignment = 3

            Dim CARDP23 = New Paragraph("El contratista se compromete a realizar sobre su equipo de encuestadores una supervisión mínima de 30% sobre la producción de cada uno de los encuestadores. Para tal efecto se utilizaran los formatos de supervisión establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CARDP23.Alignment = 3

            Dim CARDT23 = New Paragraph("El contratista se compromete a realizar sobre su equipo de encuestadores una supervisión mínima de 10% sobre la producción de cada uno de los encuestadores. Para tal efecto se utilizaran los formatos de supervisión establecidos por LA CONTRATANTE, los cuales deberán ser entregados a esta última debidamente diligenciados.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CARDT23.Alignment = 3

            Dim aTableE3 = New iTextSharp.text.pdf.PdfPTable(2)
            aTableE3.DefaultCell.Border = Rectangle.NO_BORDER
            aTableE3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            aTableE3.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

            Dim CA24 = New Paragraph("PENALIDADES:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD))
            CA24.Alignment = 3

            Dim CA25 = New Paragraph("El incumplimiento por parte del contratista de cualquiera de sus obligaciones implicará las siguientes penalidades:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CA25.Alignment = 3

            Dim CA26 = New Paragraph("1. El incumplimiento en los requerimientos y las especificaciones de los proyectos de investigación  conforme a los documentos entregados por la CONTRATANTE - metodología, instructivos, u otros – (Numerales 5 y 6 de la cláusula sexta del contrato marco de prestación de servicios) implicará la anulación del servicio y/o encuesta con la consecuencia de no causar remuneración alguna. Adicionalmente se acuerda que si del total de servicio y/o encuestas requeridos al CONTRATISTA llegare a presentarse una anulación superior al 3% del total de los verificados por IPSOS, la remuneración general de esta Orden de servicio se disminuirá en 10%.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CA26.Alignment = 3

            Dim CA27 = New Paragraph("2. El incumplimiento de la obligación relacionada con la entrega de los servicios contratados en los tiempos o plazos establecidos en la presente orden producirá una penalización del 10% del valor de la RSE.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CA27.Alignment = 3

            Dim CA28 = New Paragraph("PARAGRAFO 1:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD))
            CA28.Alignment = 3
            Dim CA28B = New Paragraph("Para el pago de las penalidades anteriormente indicadas no será necesario requerimiento de ninguna índole al cual renuncian expresamente las partes.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CA28B.Alignment = 3

            Dim CA29 = New Paragraph("PARAGRAFO 2:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD))
            CA29.Alignment = 3
            Dim CA29B = New Paragraph("Las partes acuerdan que las penalidades anteriormente indicadas podrán descontarse de la presente RSE o de cualquier crédito que el contratista tenga a su favor en desarrollo del contrato marco de prestación de servicios celebrado entre las partes.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CA29B.Alignment = 3

            Dim aTableE4 = New iTextSharp.text.pdf.PdfPTable(2)
            aTableE4.DefaultCell.Border = Rectangle.NO_BORDER
            aTableE4.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            aTableE4.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

            Dim CA30 = New Paragraph("CONFIDENCIALIDAD:", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.BOLD))
            CA30.Alignment = 3

            Dim CA31 = New Paragraph("EL CONTRATISTA mantendrá la confidencialidad de la información recibida del CONTRATANTE para la ejecución y desarrollo de los servicios contratados. Se reitera en esta materia el estricto cumplimiento de la obligación consignada en la cláusula décima del contrato marco de prestación de servicios y el Acuerdo especial de confidencialidad suscrito por las partes. EL CONTRATISTA declara haber leído y tener perfectamente presente para la ejecución del presente RSE los documentos antes mencionados.", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL))
            CA31.Alignment = 3

            Dim aTableE5 = New iTextSharp.text.pdf.PdfPTable(2)
            aTableE5.DefaultCell.Border = Rectangle.NO_BORDER
            aTableE5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            aTableE5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            aTableE5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            aTableE5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            aTableE5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))
            aTableE5.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 9, iTextSharp.text.Font.NORMAL)))

            Dim CA32 = New PdfPCell(New Paragraph("Pablo Nelson Ayala Beltrán", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            CA32.HorizontalAlignment = 3

            Dim CA33 = New PdfPCell(New Paragraph(InfoContr.Item(0).Nombre, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            CA33.HorizontalAlignment = 3

            Dim CA34 = New PdfPCell(New Paragraph("C.C. 80778776", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            CA34.HorizontalAlignment = 3

            Dim CA35 = New PdfPCell(New Paragraph("C.C. " & InfoContr.Item(0).Identificacion, FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            CA34.HorizontalAlignment = 3

            Dim CA36 = New PdfPCell(New Paragraph("CONTRATANTE", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            CA34.HorizontalAlignment = 3

            Dim CA37 = New PdfPCell(New Paragraph("CONTRATISTA", FontFactory.GetFont(FontFactory.TIMES, 10, iTextSharp.text.Font.NORMAL)))
            CA37.HorizontalAlignment = 3

            aTableE5.AddCell(CA32)
            aTableE5.AddCell(CA33)
            aTableE5.AddCell(CA34)
            aTableE5.AddCell(CA35)
            aTableE5.AddCell(CA36)
            aTableE5.AddCell(CA37)

            Dim ImgCalidad As iTextSharp.text.Image
            ImgCalidad = iTextSharp.text.Image.GetInstance(path & "logo-titulo2.png")
            ImgCalidad.ScalePercent(10)
            Dim ImgCal = New PdfPCell
            ImgCal.Border = BorderStyle.None
            ImgCal.AddElement(ImgCalidad)

            documentoPDF.Add(aTable19)
            documentoPDF.Add(aTable20)

            If (descripcion.IndexOf("Trabajo de Campo Nacional") > -1 And descripcion.IndexOf("Supervisores de encuestas") > -1 And (t.OP_MetodologiaId <> 9 And t.OP_MetodologiaId <> 10)) Or (descripcion.IndexOf("Trabajo de Campo Nacional") > -1 Or descripcion.IndexOf("Encuesta especializada") > -1 And (t.OP_MetodologiaId = 4 Or t.OP_MetodologiaId = 5)) Or descripcion.IndexOf("Otras asistencias") > -1 Then
                documentoPDF.Add(CARDP1)
            End If

            If descripcion.IndexOf("Captura") > -1 Then
                documentoPDF.Add(CACAP1)
            End If

            If descripcion.IndexOf("Codificación Externa") > -1 Then
                documentoPDF.Add(CACOD1)
            End If

            If descripcion.IndexOf("Proc de información encuestas") > -1 Then
                documentoPDF.Add(CAPRO1)
            End If

            If descripcion.IndexOf("Trabajo de Campo Nacional") > -1 And descripcion.IndexOf("Supervisores de encuestas") > -1 And (t.OP_MetodologiaId = 9 Or t.OP_MetodologiaId = 10) Then
                documentoPDF.Add(CARDT1)
            End If

            documentoPDF.Add(aTableE1)
            documentoPDF.Add(CA2)
            documentoPDF.Add(aTableE2)
            documentoPDF.Add(CA3)


            If (descripcion.IndexOf("Trabajo de Campo Nacional") > -1 And descripcion.IndexOf("Supervisores de encuestas") > -1 And (t.OP_MetodologiaId <> 9 And t.OP_MetodologiaId <> 10)) Or (descripcion.IndexOf("Trabajo de Campo Nacional") > -1 Or descripcion.IndexOf("Encuesta especializada") > -1 And (t.OP_MetodologiaId = 4 Or t.OP_MetodologiaId = 5)) Or descripcion.IndexOf("Otras asistencias") > -1 Then
                documentoPDF.Add(CARDP23)
            End If

            If descripcion.IndexOf("Captura") > -1 Then
                documentoPDF.Add(CACAP23)
            End If

            If descripcion.IndexOf("Codificación Externa") > -1 Then
                documentoPDF.Add(CACOD23)
            End If

            If descripcion.IndexOf("Proc de información encuestas") > -1 Then
                documentoPDF.Add(CAPRO23)
            End If

            If descripcion.IndexOf("Trabajo de Campo Nacional") > -1 And descripcion.IndexOf("Supervisores de encuestas") > -1 And (t.OP_MetodologiaId = 9 Or t.OP_MetodologiaId = 10) Then
                documentoPDF.Add(CARDT23)
            End If


            documentoPDF.Add(aTableE3)
            documentoPDF.Add(CA24)
            documentoPDF.Add(CA25)
            documentoPDF.Add(CA26)
            documentoPDF.Add(CA27)
            documentoPDF.Add(CA28)
            documentoPDF.Add(CA28B)
            documentoPDF.Add(CA29)
            documentoPDF.Add(CA29B)
            documentoPDF.Add(aTableE4)
            documentoPDF.Add(CA30)
            documentoPDF.Add(CA31)
            documentoPDF.Add(aTableE5)

        End If
        '-----------------FINAL DE ANEXO RSE



        documentoPDF.NewPage()

        'Añadimos los metadatos para el fichero PDF
        documentoPDF.AddAuthor(Session("IDUsuario").ToString)
        documentoPDF.AddTitle("OrdendeServicio")
        documentoPDF.AddCreationDate()
        documentoPDF.Close() 'Cerramos el objeto documento, guardamos y creamos el PDF


        'Comprobamos si se ha creado el fichero PDF
        If System.IO.File.Exists(urlFija & "\" & "OrdendeServicio-" & t.JobBook & "-" & Replace(Ordenes.Nombre, "&", "Y") & ".pdf") Then
            'ShowNotification("Archivo Generado", ShowNotifications.InfoNotification)
            'Response.Redirect("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=4" & "&Id=" & InfoContr.Item(0).Identificacion& & "&TrabajoId=1")
            ShowWindows("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=4" & "&JobBook=" & t.JobBook & "&Contratista=" & Replace(Ordenes.Nombre, "&", "Y") & "&TrabajoId=1")
        Else

            ShowNotification("El fichero PDF no se ha generado, compruebe que tiene permisos en la carpeta de destino.", ShowNotifications.InfoNotification)
        End If

    End Sub
    Protected Sub GvDetalleOrden_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GvDetalleOrden.SelectedIndexChanged

    End Sub
    Sub CargarGridPersonas()
        Dim o As New CoreProject.US.Usuarios
        Dim cedula As Int64? = Nothing
        Dim nombre As String = Nothing
        If IsNumeric(txtCedulaSolicitante.Text) Then cedula = txtCedulaSolicitante.Text
        If Not txtNombreSolicitante.Text = "" Then nombre = txtNombreSolicitante.Text
        Me.gvSolicitantes.DataSource = o.obtener(cedula, Nothing, nombre, Nothing, Nothing, Nothing)
        Me.gvSolicitantes.DataBind()
    End Sub
    Private Sub gvSolicitantes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvSolicitantes.RowCommand
        If e.CommandName = "Seleccionar" Then
            txtEvaluaFactura.Text = HttpUtility.HtmlDecode(gvSolicitantes.Rows(e.CommandArgument).Cells(0).Text & " " & gvSolicitantes.Rows(e.CommandArgument).Cells(1).Text)
            hfIdEvaluaFactura.Value = Me.gvSolicitantes.DataKeys(CInt(e.CommandArgument))("id")
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub
    Private Sub btnBuscarSolicitante_Click(sender As Object, e As EventArgs) Handles btnBuscarSolicitante.Click
        CargarGridPersonas()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        UPanelSolicitantes.Update()
    End Sub
    Sub CargarDepartamentos()
        Dim oCoordCampo As New CoordinacionCampo
        Dim list = (From ldep In oCoordCampo.ObtenerDepartamentos()
                  Select iddep = ldep.DivDeptoMunicipio, nomdep = ldep.DivDeptoNombre).Distinct.ToList
        ddlDepartamento.DataSource = list
        ddlDepartamento.DataValueField = "iddep"
        ddlDepartamento.DataTextField = "nomdep"
        ddlDepartamento.DataBind()
        Me.ddlDepartamento.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
    Sub CargarCiudades()
        Dim oCoordCampo As New CoordinacionCampo
        Dim listciudades = (From ldep In oCoordCampo.ObtenerCiudades(ddlDepartamento.SelectedValue)
                  Select idmuni = ldep.DivMunicipio, nommuni = ldep.DivMuniNombre)
        ddlCiudad.DataSource = listciudades
        ddlCiudad.DataValueField = "idmuni"
        ddlCiudad.DataTextField = "nommuni"
        ddlCiudad.DataBind()
        Me.ddlCiudad.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub
    Private Sub ddlDepartamento_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlDepartamento.SelectedIndexChanged
        CargarCiudades()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub
    Sub cargarGrillaOrdenes(ByVal Id As Int64?, ByVal Trabajoid As Int64?, ByVal contratistaId As Int64?)
        Dim op As New ProcesosInternos
        GvOrdenes.DataSource = op.OrdenesdeServicioGet(Trabajoid, Id, contratistaId)
        GvOrdenes.DataBind()
    End Sub
    Sub actualizarEstadoOrden(ByVal idOrden As Int64, ByVal estado As ProcesosInternos.EEstadosOrdenes, ByVal observacion As String, ByVal usuarioId As Int64, ByVal fechaRegistro As DateTime)
        Dim op As New ProcesosInternos
        op.ActualizarOrdeneServicio(idOrden, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, estado, Nothing, Nothing, Nothing, Nothing, Nothing)
        op = New ProcesosInternos
        op.guardarLogEstadosOrdenesServicio(idOrden, observacion, fechaRegistro, usuarioId, estado)
    End Sub

    Protected Sub btnGuardarAnulacion_Click(sender As Object, e As EventArgs) Handles btnGuardarAnulacion.Click
        Dim op As New ProcesosInternos
        Dim os As CC_OrdenesdeServicioGet_Result
        os = op.OrdenesdeServicioGet(Nothing, HfOSAnulacionId.Value, Nothing).FirstOrDefault
        If os.Estado = ProcesosInternos.EEstadosOrdenes.Anular Then
            ShowNotification("Esta orden de servicio ya se encuentra anulada", ShowNotifications.InfoNotification)
        Else
            If op.RecepcionCuentasGet(Nothing, HfOSAnulacionId.Value).Count = 0 Then
                actualizarEstadoOrden(HfOSAnulacionId.Value, ProcesosInternos.EEstadosOrdenes.Anular, txtObservacionAnulacion.Text, Session("IDUsuario"), DateTime.Now)
                ShowNotification("Se ha anulado la orden satisfactoriamente", ShowNotifications.InfoNotification)
            Else
                ShowNotification("No es posible anular la orden porque ya tiene facturas radicadas", ShowNotifications.InfoNotification)
            End If
        End If
    End Sub

    Private Sub ddlServicio_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlServicio.SelectedIndexChanged
        Dim daContratista As New Contratista
        ddlCuentasContables.DataSource = daContratista.ObtenerCuentasContablesXServicio(ddlServicio.SelectedValue)
        ddlCuentasContables.DataValueField = "IdCuenta"
        ddlCuentasContables.DataTextField = "Descripcion"
        ddlCuentasContables.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    
End Class