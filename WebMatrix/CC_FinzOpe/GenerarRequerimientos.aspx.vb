Imports WebMatrix.Util
Imports iTextSharp
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports System.IO
Imports CoreProject

Public Class GenerarRequerimientos
    Inherits System.Web.UI.Page

#Region "Propiedades"

    Private _PresupuestoId As Int64
    Public Property PresupuestoId() As Int64
        Get
            Return _PresupuestoId
        End Get
        Set(ByVal value As Int64)
            _PresupuestoId = value
        End Set
    End Property
    Private _TrabajoId As Int16
    Public Property TrabajoId() As Int64
        Get
            Return _TrabajoId
        End Get
        Set(ByVal value As Int64)
            _TrabajoId = value
        End Set
    End Property
#End Region


#Region "Eventos"
    Private Sub PresupuestosInternos_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

            If permisos.VerificarPermisoUsuario(149, UsuarioID) = False Then
                Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
            End If

            CargarTrabajos(Session("IDUsuario").ToString)
            If Not Session("TrabajoId") = Nothing Then
                ' hfIdTrabajo.Value = Session("TrabajoId").ToString
            End If
        End If
        ' lbtnVolver.PostBackUrl = "~/OP_Cuantitativo/HomeRecoleccion.aspx"

    End Sub

    Private Sub gvTrabajos_PageIndexChanging(sender As Object, e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvTrabajos.PageIndexChanging
        gvTrabajos.PageIndex = e.NewPageIndex
        CargarTrabajos(Session("IDUsuario").ToString)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub


    Private Sub gvTrabajos_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvTrabajos.RowCommand
        If e.CommandName = "Ciudades" Then
            hfIdTrabajo.Value = gvTrabajos.DataKeys(CInt(e.CommandArgument))("Id")
            hfJobook.Value = gvTrabajos.Rows((e.CommandArgument)).Cells(1).Text
            hfnombretrabajo.Value = Server.HtmlDecode(gvTrabajos.Rows(CInt(e.CommandArgument)).Cells(2).Text)
            hfidFechaIni.Value = gvTrabajos.Rows(CInt(e.CommandArgument)).Cells(10).Text
            ' hfIdCoordinador.Value = gvTrabajos.Rows(CInt(e.CommandArgument)).Cells(6).Text
            ' hfidciudad.Value = gvTrabajos.Rows((e.CommandArgument)).Cells(3).Text
            'CargarPersonal(hfIdTrabajo.Value, hfidciudad.Value, 7) 'Recoradar Cambiar por 7
            'hfiCiudatexto.Value = gvTrabajos.Rows((e.CommandArgument)).Cells(4).Text
            cargarMuestra(hfIdTrabajo.Value)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)

        End If

    End Sub

    Private Sub btnGenerarRequisicion_Click(sender As Object, e As System.EventArgs) Handles btnGenerarRequisicion.Click
        CrearPDF()
    End Sub


    Private Sub gvmuestra_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvmuestra.RowCommand
        If e.CommandName = "PersonasAsigandas" Then
            CargarPersonal(hfIdTrabajo.Value, gvmuestra.Rows((e.CommandArgument)).Cells(2).Text, 7)
            ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

    Protected Sub btnrequisicion_Click(sender As Object, e As EventArgs) Handles btnrequisicion.Click
        Dim Lta As New List(Of CC_PersonalAsignadoGet_Result)
        Dim i As Integer = 0
        Dim op As New CC_FinzOpe

        For Each row As GridViewRow In Me.gvmuestra.Rows


            Dim elcheckbox As CheckBox = CType(row.FindControl("chkStatus"), CheckBox)
            If elcheckbox.Checked Then
                ' Dim Fila As GridViewRow = row
                Lta.AddRange(op.CC_PersonalAsignadoGet(hfIdTrabajo.Value, gvmuestra.Rows(i).Cells(2).Text, 7).ToList)

            End If
            i = i + 1
        Next
        PERSONAS(Lta)
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim oTrabajo As New GestionTrabajosFin
        Dim id As Int64? = Nothing
        Dim JobBook As String = Nothing
        Dim Nombre As String = Nothing
        Dim NoProp As Int64? = Nothing
        If Not (txtID.Text = "") Then id = txtID.Text
        If Not (txtJobBook.Text = "") Then JobBook = txtJobBook.Text
        If Not (txtNombreTrabajo.Text = "") Then Nombre = txtNombreTrabajo.Text
        If Not (txtNoPropuesta.Text = "") Then NoProp = txtNoPropuesta.Text
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajoscc(id, Nothing, Nombre, JobBook, Nothing, Nothing, Nothing, Nothing, Nothing, NoProp)
        gvTrabajos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
#End Region

#Region "Metodos"
    Sub CargarTrabajos(ByVal Coe As Int64)
        Dim oTrabajo As New GestionTrabajosFin
        gvTrabajos.DataSource = oTrabajo.ListadoTrabajoscc(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
        gvTrabajos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Sub cargarMuestra(ByVal Trabajoid As Int64)
        Dim opr As New PresupInt
        Me.gvmuestra.DataSource = opr.ObtenerMuestraTrabajo(Trabajoid)
        Me.gvmuestra.DataBind()

    End Sub
    Sub CargarPersonal(ByVal trabajoid As Int64?, ByVal Ciudad As Int64?, ByVal contratacion As Int64?)
        Dim op As New PresupInt
        gvPersonalAsignado.DataSource = op.PersonalAsginado(trabajoid, Ciudad, contratacion)
        gvPersonalAsignado.DataBind()
        ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
    End Sub
    Sub PERSONAS(ByVal LISTA As List(Of CC_PersonalAsignadoGet_Result))
        gvPersonalAsignado.DataSource = LISTA
        gvPersonalAsignado.DataBind()
        ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
    End Sub

    Function ValorUnitario(ByVal Trabajoid As Int64, ByVal Tipo As Int64)
        Dim op As New PresupInt
        Dim ent As New CC_ValorEncuestaGet_Result
        ent = op.ValorEncuesta(Trabajoid, Tipo).FirstOrDefault

        Return ent
    End Function

    Function NombreCoordinador(ByVal Trabajoid As Int64, ByVal Ciudadid As Int64)
        Dim op As New PresupInt
        Dim ent As New CC_NombreCoordinadorGet_Result
        ent = op.ObtenerNombreCoordinador(Trabajoid, Ciudadid).FirstOrDefault
        Return ent
    End Function

    Sub CrearPDF()
        Dim cc As New PresupInt
        Dim Vr As New CC_ValorEncuestaGet_Result
        Dim valor As New Double
        Dim urlFija As String
        Dim Url As String
        Dim Ent As New CC_ValorEncuestaGet_Result
        Dim nombre As CC_NombreCoordinadorGet_Result
        Dim path As String = Server.MapPath("~/Images/")
        Dim VRENC As New CC_ValorEncuestaGet_Result
        Dim VRSUP As New CC_ValorEncuestaGet_Result
        Dim ValorEsp As New Double
        'Dim Url As String
        urlFija = "~/Requisiciones/"
        Url = "..\Requisiciones\"
        'Url = hfIdTrabajo.Value
        'HACK Deuda tecnica Aquí se deberia colocar la ruta raíz en un parametro en la base de datos
        urlFija = Server.MapPath(urlFija & "\")
        CargarMetodologia(hfIdTrabajo.Value)

        Dim pdfw As PdfWriter
        Dim documentoPDF As New Document(iTextSharp.text.PageSize.A4, 0, 0, 20, 20) 'Creamos el objeto documento PDF
        pdfw = PdfWriter.GetInstance(documentoPDF, New FileStream(urlFija & "\" & hfIdTrabajo.Value & "-" & Now.Year & "-" & Now.Month & "-" & Now.Day & ".pdf", FileMode.Create))
        documentoPDF.Open()





        For i = 0 To gvPersonalAsignado.Rows.Count - 1

            'If hfidMet.Value = 130 Then
            '    VRENC = ValorUnitario(hfIdTrabajo.Value, 3)
            '    VRSUP = ValorUnitario(hfIdTrabajo.Value, 6)
            '    ValorEsp = VRENC.Ajustado + VRSUP.Ajustado
            'Else
            If gvPersonalAsignado.Rows(i).Cells(2).Text = "Encuestador" Then
                Ent = ValorUnitario(hfIdTrabajo.Value, 3)
            ElseIf gvPersonalAsignado.Rows(i).Cells(2).Text = "Supervisor" Then
                'Vr = ValorUnitario(hfIdTrabajo.Value, 3)
                Ent = ValorUnitario(hfIdTrabajo.Value, 6)
                'valor = FormatCurrency(Vr.Ajustado, 0)
            End If
            ' End If

            nombre = NombreCoordinador(hfIdTrabajo.Value, gvPersonalAsignado.Rows(i).Cells(6).Text)
            documentoPDF.NewPage()
            Dim aTable = New iTextSharp.text.pdf.PdfPTable(2)
            aTable.DefaultCell.Border = Rectangle.NO_BORDER

            Dim Cell = New PdfPCell(New Paragraph("REQUERIMIENTO DE SERVICIOS TECNICOS", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.BOLD)))
            Cell.Colspan = 3
            Cell.Border = 0
            Cell.HorizontalAlignment = 1
            aTable.AddCell(Cell)

            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

            aTable.AddCell(New Paragraph("CIUDAD: " & gvPersonalAsignado.Rows(i).Cells(5).Text, FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("FECHA:  " & Now(), FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

            aTable.AddCell(New Paragraph("CONTRATANTE: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("IPSOS NAPOLEON FRANCO & CIA S.A.S.", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

            aTable.AddCell(New Paragraph("IDENTIFICACION: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("NIT: 890.319.494-5", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

            aTable.AddCell(New Paragraph("CONTRATISTA:  ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(Server.HtmlDecode(gvPersonalAsignado.Rows(i).Cells(1).Text), FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("IDENTIFICACION: C.C. Nº  ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(gvPersonalAsignado.Rows(i).Cells(0).Text, FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

            Dim Cell2 = New PdfPCell(New Paragraph("Por la presente IPSOS NAPOLEÓN FRANCO & CIA S.A.S., en desarrollo del contrato de prestación de servicios técnicos actualmente vigente entre las partes procede a suministrar al CONTRATISTA información sobre encuestas a realizarse por parte de este último: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            Cell2.Colspan = 3
            Cell2.Border = 0
            Cell2.HorizontalAlignment = 0
            aTable.AddCell(Cell2)

            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

            'Dim val As String
            'If valor = 0 Then
            '    val = ""
            'Else
            '    val = CStr(valor)
            'End If

            'If ValorEsp = 0 Then
            '    'ValorEsp = FormatCurrency(Ent.Ajustado, 0)
            'End If


            aTable.AddCell(New Paragraph("1º Nombre e identificación del estudio: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(hfIdTrabajo.Value & " / " & hfJobook.Value & " / " & hfnombretrabajo.Value, FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("2º Objeto del requerimiento: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("Trabajo nacional de campo", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("3º Valor a reconocer por encuesta: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(FormatCurrency(Ent.Ajustado, 0), FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("4º Fecha de iniciación de encuestas: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(hfidFechaIni.Value, FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("5º Lugar para realización de encuestas: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(nombre.Ciudad, FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("6º Coordinador asignado por la empresa y encargado de recibir documentos: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(nombre.Nombre, FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("Firma Ordenante: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("Firma Contratista: ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))



            Dim Imagen As iTextSharp.text.Image
            Imagen = iTextSharp.text.Image.GetInstance(path & "Firma.jpg")
            Imagen.ScalePercent(75)
            Dim Img = New PdfPCell
            Img.Border = 0
            Img.AddElement(Imagen)
            aTable.AddCell(Img)

            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))


            aTable.AddCell(New Paragraph("Alexandra Vargas Lopez", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(Server.HtmlDecode(gvPersonalAsignado.Rows(i).Cells(1).Text), FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("Contratante", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("Contratista", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            'aTable.AddCell(New Paragraph(gvPersonalAsignado.Rows(i).Cells(2).Text, FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("Cedula: 51787080", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph("Cedula: " & gvPersonalAsignado.Rows(i).Cells(0).Text, FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            aTable.AddCell(New Paragraph(" ", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))

            Dim Cell3 = New PdfPCell(New Paragraph("Autorizo, a que me sea descontado de la cuenta de cobro respectiva, el valor que adeude por seguridad social", FontFactory.GetFont(FontFactory.TIMES, 11, iTextSharp.text.Font.NORMAL)))
            Cell3.Colspan = 3 '
            Cell3.Border = 0
            Cell3.HorizontalAlignment = 0
            aTable.AddCell(Cell3)

            documentoPDF.Add(aTable) 'Agregamos la Tabla al Documento
            cc.Requisiciones(gvPersonalAsignado.Rows(i).Cells(0).Text, hfIdTrabajo.Value, Ent.Ajustado, nombre.CiudadId, CInt(Session("IDUsuario")))

        Next
        'Añadimos los metadatos para el fichero PDF
        documentoPDF.AddAuthor(Session("IDUsuario").ToString)
        documentoPDF.AddTitle("RequerimientoServicios")
        documentoPDF.AddCreationDate()
        documentoPDF.Close() 'Cerramos el objeto documento, guardamos y creamos el PDF


        'Comprobamos si se ha creado el fichero PDF
        If System.IO.File.Exists(urlFija & "\" & hfIdTrabajo.Value & "-" & Now.Year & "-" & Now.Month & "-" & Now.Day & ".pdf") Then

            'Response.Redirect("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=1" & "&Id=2" & "&Trabajoid=" & hfIdTrabajo.Value)
            ShowWindows("../CC_FinzOpe/ListadodeRequerimientos.aspx?Tipo=1" & "&Id=2" & "&Trabajoid=" & hfIdTrabajo.Value)
            ' ShowNotification("Requisicion Generada", ShowNotifications.InfoNotification)
            'DescargarPDF(urlFija & "\" & hfIdTrabajo.Value & "-" & hfidmuestra.Value & ".pdf")
            'DescargarPDF(hfIdTrabajo.Value & "-" & hfidmuestra.Value & ".pdf", Url)
            ' Dim sUrl As String = "/paginaNuevaaspx?Parametro=" & parametro
            'Descargar archivo en la ruta descrita en path


        Else
            'MsgBox("El fichero PDF no se ha generado, " + _
            '       "compruebe que tiene permisos en la carpeta de destino.",
            '       MsgBoxStyle.Exclamation + MsgBoxStyle.OkOnly)

            ShowNotification("El fichero PDF no se ha generado, compruebe que tiene permisos en la carpeta de destino.", ShowNotifications.InfoNotification)
        End If
        ActivateAccordion(3, EffectActivateAccordion.NoEffect)
    End Sub
    Sub CargarMetodologia(ByVal Trabajoid As Int64)
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo = oTrabajo.ObtenerTrabajo(Trabajoid)
        Dim oMetodologias As New MetodologiaOperaciones
        Dim op As New PresupInt
        hfidMet.Value = oMetodologias.obtenerXId(oeTrabajo.OP_MetodologiaId).MetCodigo
    End Sub

    Sub RegistroRequisiciones()


    End Sub
#End Region



End Class