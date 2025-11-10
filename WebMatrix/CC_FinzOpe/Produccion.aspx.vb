Imports CoreProject
'Imports CoreProject.CC_FinanzasOp
Imports WebMatrix.Util
Imports iTextSharp
Imports iTextSharp.text.pdf
Imports System.IO
Imports iTextSharp.text
Imports ClosedXML
Imports ClosedXML.Excel
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.ServiceLocation



Public Class Produccion
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
            smanager.RegisterPostBackControl(Me.btnexportar)
            smanager.RegisterPostBackControl(Me.btnnomina)
        End If

    End Sub

    Protected Sub btnConsultar_Click(sender As Object, e As EventArgs) Handles btnConsultar.Click
        produccion(txtFechaInicio.Text, txtFechaFinalizacion.Text, Nothing)
        'CargarDatos(txtFechaInicio.Text, txtFechaFinalizacion.Text)
    End Sub

    Protected Sub btnCuentas_Click(sender As Object, e As EventArgs) Handles btnCuentas.Click
        Response.Redirect("../CC_FinzOpe/CuentasdeCobro.aspx")
        'CrearPdf()
    End Sub
    Sub CargarDatos(ByVal Fecini As Date, ByVal Fecfin As Date)
        Dim op As New PresupInt
        Me.gvProduccion.DataSource = op.ProducccionGet(Fecini, Fecfin)
        Me.gvProduccion.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Sub produccion(ByVal Fecini As Date, ByVal Fecfin As Date, ByVal Cedula As Int64?)
        Dim po As New PresupInt
        Me.gvProduccion.DataSource = po.ProduccionGetxFecha(Fecini, Fecfin, Cedula)
        Me.gvProduccion.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub
    Sub CellBorders(ByRef Celda As PdfPCell, ByVal _Top As Single, ByVal _Rigth As Single, ByVal _Bottom As Single, ByVal _Left As Single)
        Celda.BorderWidthBottom = 1
        Celda.BorderWidthLeft = 1
        Celda.BorderWidthRight = 1
        Celda.BorderWidthTop = 1
        Dim color As New iTextSharp.text.BaseColor(0, 0, 0) '(100, 100, 100, 100) '
        Celda.BorderColor = color
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
    Protected Sub btnexportar_Click(sender As Object, e As EventArgs) Handles btnexportar.Click
        'Exportar(txtFechaInicio.Text, txtFechaFinalizacion.Text)
        ExportarProductividad(txtFechaInicio.Text, txtFechaFinalizacion.Text)
    End Sub
    Protected Sub Exportar(ByVal FecIni As Date, ByVal FecFin As Date)
        Dim excel As New List(Of Array)
        Dim Titulos As String = "Estudio;JobBook;Cedula;Fecha;Total;Ciudad;Cargo;Estado;FechaCargue;Usuario;TipoContrataciónMatrix;CargoMatrix;NSE;MUESTRA;Orden;Observacion"
        Dim DynamicColNames() As String
        Dim lstCambios As List(Of CC_ProduccionXFechas_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("Produccion")

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        Dim op As New PresupInt
        lstCambios = op.ProducccionGet(FecIni, FecFin)
        For Each x In lstCambios
            excel.Add((x.Estudio & ";" & x.JobBook & ";" & x.Cedula & ";" & x.Fecha & ";" & x.Total & ";" & x.Ciudad & ";" & x.Cargo & ";" & x.Estado & ";" & x.FechaCargue & ";" & x.Usuario & ";" & x.TipoContrataciónMatrix & ";" & x.CargoMatrix & ";" & x.NSE & ";" & x.MUESTRA & ";" & x.Orden & ";" & x.Observacion).Split(CChar(";")).ToArray())
        Next
        worksheet.Cell("A1").Value = excel
        Crearexcel(workbook, "Produccion -" & Now.Year & "-" & Now.Month & "-" & Now.Day)
    End Sub
    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

        Using memoryStream = New System.IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub
    Private Sub btnSinPresupuesto_Click(sender As Object, e As EventArgs) Handles btnSinPresupuesto.Click
        Response.Redirect("TrabajosSinPresupuesto.aspx", True)
    End Sub
    Protected Sub btnCargar_Click(sender As Object, e As EventArgs) Handles btnCargar.Click
        ' Me.upCargar.Update()
        'ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        Response.Redirect("../CC_FinzOpe/CargarInformacion.aspx")
    End Sub

    Protected Sub btnPasarServidor_Click(sender As Object, e As EventArgs) Handles btnPasarServidor.Click
        If IsPostBack Then
            Dim fileOK As [Boolean] = False

            Dim path As [String] = Server.MapPath("~/Excel/")

            If FileUpload1.HasFile Then
                Dim fileExtension As [String] = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower()
                Dim allowedExtensions As [String]() = {".xls", ".xlsx"}
                For i As Integer = 0 To allowedExtensions.Length - 1
                    If fileExtension = allowedExtensions(i) Then
                        fileOK = True
                    End If
                Next
            End If

            If fileOK Then
                Try
                    FileUpload1.PostedFile.SaveAs(path + FileUpload1.FileName)
                    'Guardamos el nombre del archivo que se subio al servidor para renombrarlo de forma standar y luego poder subirlo a la BD
                    Session("FILENAME") = FileUpload1.FileName
                    Session("FILEEXT") = System.IO.Path.GetExtension(FileUpload1.FileName).ToLower()

                    'Cambiamos de nombre el archivo para luego congerlo y cargarlo a la bd
                    If File.Exists(MapPath("~/Files/") & "ERRORES" & "." & Session("FILEEXT").ToString()) Then
                        File.Delete(MapPath("~/Files/") & "ERRORES" & "." & Session("FILEEXT").ToString())
                    End If

                    File.Move(MapPath("~/Files/") & Session("FILENAME").ToString(), MapPath("~/Files/") & "ERRORES" & "." & Session("FILEEXT").ToString())

                    ' ObtenerHojasNombres()

                    ShowNotification("Archivo en el servidor - LISTO!", WebMatrix.ShowNotifications.InfoNotification)
                Catch ex As Exception
                    ShowNotification(ex.Message.Replace("'", "").Replace("" & vbCrLf & "", "<br>"), WebMatrix.ShowNotifications.ErrorNotification)

                    'Notificacion.ShowNotification(Me, ex.Message.Replace("'", "").Replace("" & vbCrLf & "", "<br>"), Me.btnPasarServidor, Nothing)
                End Try
            Else
                ShowNotification("No se aceptan archivos de este tipo!", WebMatrix.ShowNotifications.ErrorNotification)
            End If
        End If
    End Sub

    Protected Sub btnCargarDatos_Click(sender As Object, e As EventArgs) Handles btnCargarDatos.Click
        Dim Errores As New Errores()
        Try
            ' If lstHoja.SelectedIndex > 0 And lstHoja.Items.Count > 0 Then
            If (File.Exists(Server.MapPath("~/Files/ERRORES" & "." & Session("FILEEXT").ToString))) Then
                Dim MyConnection As System.Data.OleDb.OleDbConnection
                Dim DtSet As System.Data.DataSet
                Dim MyCommand As New System.Data.OleDb.OleDbDataAdapter
                MyConnection = New System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0; Data Source='" & Server.MapPath("~/Files/ERRORES" & "." & Session("FILEEXT").ToString) & "';Extended Properties='Excel 12.0;HDR=NO; IMEX=1'")
                'MyCommand = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM , MyConnection)

                DtSet = New System.Data.DataSet

                MyCommand.Fill(DtSet)

                'InsertarErrores(DtSet)

                File.Delete(Server.MapPath("~/Files/ERRORES" & "." & Session("FILEEXT").ToString))

                ' ShowNotification("Datos cargados! - " & Mensaje2, WebMatrix.ShowNotifications.InfoNotification)

            Else
                ShowNotification("El archivo no existe, probablemente no lo ha cargado!", WebMatrix.ShowNotifications.ErrorNotification)
            End If
            'Else
            ShowNotification("Seleccione la hoja del archivo que contiene los datos!", WebMatrix.ShowNotifications.ErrorNotification)
            ' End If

        Catch ex As Exception
            ShowNotification(ex.Message.Replace("'", "").Replace("" & vbCrLf & "", "<br>"), WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub

    Sub ExportarProductividad(ByVal Fecini As Date, ByVal FecFin As Date)
        Dim excel As New List(Of Array)
        Dim Titulos As String = "Estudio;JobBook;NombreTrabajo;Cedula;Fecha;Total;Ciudad;Cargo;Estado;FechaCargue;Usuario;TipoContrataciónMatrix;CargoMatrix;VrUnitario;ValorPagar;Orden;Observacion"
        Dim DynamicColNames() As String
        Dim lstCambios As List(Of CC_ProduccionExport_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("Produccion")

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        Dim op As New PresupInt
        lstCambios = op.ProduccionExport(Fecini, FecFin)
        For Each x In lstCambios

            excel.Add((x.Estudio & ";" & x.JobBook & ";" & x.NombreTrabajo & ";" & x.Cedula & ";" & x.Fecha & ";" & x.Total & ";" & x.Ciudad & ";" & x.Cargo & ";" & x.Estado & ";" & x.FechaCargue & ";" & x.Usuario & ";" & x.TipoContrataciónMatrix & ";" & x.CargoMatrix & ";" & x.VrUnitario & ";" & x.ValorPagar & ";" & x.Orden & ";" & x.Observacion).Split(CChar(";")).ToArray())
        Next
        worksheet.Cell("A1").Value = excel
        Crearexcel(workbook, "Produccion -" & Now.Year & "-" & Now.Month & "-" & Now.Day)
    End Sub

    Sub ArchivoNomina(ByVal Fecini As Date, ByVal FecFin As Date)
        Dim excel As New List(Of Array)
        Dim Titulos As String = "Cedula;Nombre;VrBase;FechaIngreso;Tipo;DiasTrabajados"
        Dim DynamicColNames() As String
        Dim lstdatos As List(Of CC_ArchivoNomina_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("ArchivoNomina")

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        Dim op As New PresupInt
        lstdatos = op.ArchivoNomina(Fecini, FecFin)

        For Each x In lstdatos
            excel.Add((x.Cedula & ";" & x.Nombre & ";" & x.VrBase & ";" & x.FechaIngreso & ";" & x.Tipo & ";" & x.DiasTrabajados).Split(CChar(";")).ToArray())
        Next

        worksheet.Cell("A1").Value = excel

        worksheet.Range("A2:A" & lstdatos.Count + 1).DataType = XLCellValues.Number
        worksheet.Range("A2:A" & lstdatos.Count + 1).Style.NumberFormat.NumberFormatId = 1
        worksheet.Range("C2:C" & lstdatos.Count + 1).DataType = XLCellValues.Number
        worksheet.Range("C2:C" & lstdatos.Count + 1).Style.NumberFormat.NumberFormatId = 1
        worksheet.Range("F2:F" & lstdatos.Count + 1).DataType = XLCellValues.Number
        worksheet.Range("F2:F" & lstdatos.Count + 1).Style.NumberFormat.NumberFormatId = 1
        worksheet.Range("D2:D" & lstdatos.Count + 1).DataType = XLCellValues.DateTime

        Crearexcel(workbook, "ArchivoNomina -" & Now.Year & "-" & Now.Month & "-" & Now.Day)
    End Sub
    Protected Sub btnnomina_Click(sender As Object, e As EventArgs) Handles btnnomina.Click
        ArchivoNomina(txtFechaInicio.Text, txtFechaFinalizacion.Text)
    End Sub
    Protected Sub BtnBonificacion_Click(sender As Object, e As EventArgs) Handles BtnBonificacion.Click
        'PdfBonificacion()
        'ShowNotification("Requisicion Generada", ShowNotifications.InfoNotification)
    End Sub

    Protected Sub BtnEliminar_Click(sender As Object, e As EventArgs) Handles BtnEliminar.Click
        Response.Redirect("../CC_FinzOpe/EliminarCargueProduccion.aspx")
    End Sub
End Class