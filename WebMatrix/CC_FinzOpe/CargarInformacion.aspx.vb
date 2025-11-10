Imports System.Data.OleDb
Imports System.IO
Imports CoreProject
Imports WebMatrix.Util
Imports System.Data.Common
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports System.Resources
Imports System.Globalization
Imports System.Threading
Imports ClosedXML
Imports ClosedXML.Excel
Imports System.Linq

Public Class CargarInformacion
    Inherits System.Web.UI.Page

    Enum eTipoCargue
        PorDías = 1
        PorProductividad = 2
    End Enum
    Enum eTipoArchivo
        xls = 1
        xlsx = 2
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(132, UsuarioID) = False Then
            Response.Redirect("../FI_AdministrativoFinanciero/Default.aspx")
        End If
        If Not IsPostBack Then
            Page.Form.Attributes.Add("enctype", "multipart/form-data")
            ddlTipo.SelectedValue = 1
            LblCantidad.Text = ""
            LblSuma.Text = ""
            LblCantidad.Visible = False
            LblSuma.Visible = False
            BorrarDatosProduccionTemp()
            chkSoloValida.Checked = True
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnPasarServidor)
    End Sub
    Protected Sub btnPasarServidor_Click(sender As Object, e As EventArgs) Handles btnPasarServidor.Click
        If IsPostBack Then
            Dim fileOK As [Boolean] = False

            Dim path As [String] = Server.MapPath("~/Files/")

            If FileUpload1.HasFile Then
                'La carpeta donde voy a almacer el archivo
                hfNombreArchivo.Value = FileUpload1.FileName
                Dim fileload As New System.IO.FileInfo(FileUpload1.FileName)
                'Verifica que las extensiones sean las permitidas, dependiendo de la extensión llama la función
                If fileload.Extension = ".xls" Then
                    FileUpload1.SaveAs(path & "PRODUCTIVIDAD.xls")
                    hfTypeFile.Value = 0

                    fileOK = True
                    ReadExcel(0, path & "PRODUCTIVIDAD.xls")
                ElseIf fileload.Extension = ".xlsx" Then
                    FileUpload1.SaveAs(path & "PRODUCTIVIDAD.xlsx")
                    hfTypeFile.Value = 1
                    fileOK = True
                    ReadExcel(1, path & "PRODUCTIVIDAD.xlsx")
                Else
                    FileUpload1.Visible = True
                    Exit Sub
                End If
            End If
        End If

    End Sub
    Sub ReadExcel(ByVal typefile As Int16, ByVal path As String)
        Dim connstr As String = ""
        'Dependiendo del tipo de archivo configura la cadena de conexión
        If typefile = 0 Then
            connstr = "Provider=Microsoft.Jet.OLEDB.4.0;" & "Data Source=" & path & ";" & "Extended Properties='Excel 8.0'"
        ElseIf typefile = 1 Then
            connstr = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & path & ";" & "Extended Properties='Excel 12.0'"
        End If
        'Gestion la cadena de conexión
        Using cnn As New OleDbConnection(connstr)
            'Abre la conexión
            cnn.Open()

            'Crea un datatable que lee y lista las hojas del archivo
            Dim tables As DataTable = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})

            'Muestra el listado de hojas
            'pnlSheets.Visible = True

            'Recorre el datable Tables y agrega al listbox los nombres de las hojas
            lsthojas.Items.Clear()
            For Each row As DataRow In tables.Rows
                lsthojas.Items.Add(row.Item("Table_Name").ToString.Replace("$", "").Replace("'", ""))
            Next
            cnn.Close()
        End Using
    End Sub
    Protected Sub btnCargarDatos_Click(sender As Object, e As EventArgs) Handles btnCargarDatos.Click
        Dim CargueId As Int64
        Dim daPI As New ProcesosInternos
        'En general configura la cadena de conexión nuevamente y envía como parámetro el nombre de la hoja seleccionada
        Dim path As String = Server.MapPath("~/Files/")
        Dim lstErrores As List(Of CC_ProduccionValidarCargueDiferenteEncuestas_Result)
        If ddlTipoCargue.SelectedValue = eTipoCargue.PorProductividad Then
            Dim connstr As String = ""
            If hfTypeFile.Value = 0 Then
                connstr = "Provider=Microsoft.Jet.OLEDB.4.0;" & "Data Source=" & path & "PRODUCTIVIDAD.xls" & ";" & "Extended Properties='Excel 8.0'"
            ElseIf hfTypeFile.Value = 1 Then
                connstr = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & path & "PRODUCTIVIDAD.xlsx" & ";" & "Extended Properties='Excel 12.0'"
            End If
            'El objeto System.IO.FileInfo guarda todas las propiedades del archivo, nombre, extensión, tamaño, etc.
            LoadRecords(lsthojas.SelectedValue.Replace("$", "").Replace("'", ""), connstr, New System.IO.FileInfo(path & "PRODUCTIVIDAD.xls"))
        Else
            Dim tipoArchivo As eTipoArchivo = obtenerTipoArchivo(hfNombreArchivo.Value)
            Dim nombreArchivo As String = "Productividad" & If(tipoArchivo = eTipoArchivo.xls, ".xls", ".xlsx")
            Dim cadenaConexion As String = obtenerCadenaConexion(tipoArchivo, nombreArchivo, path)
            Dim dtDatos As DataTable
            Try
                dtDatos = obtenerDataTableDesdeExcel(lsthojas.SelectedItem.Text, cadenaConexion)
                If validarArchivoPorDiasColumnasDiferentes(dtDatos).Count = 0 Then
                    eliminarProduccionTempDiferenteEncuestas()
                    CopiarToSql(dtDatos, "CC_Produccion_CargueTemporal")
                    lstErrores = validacionesArchivoPorDias()
                    If lstErrores.Count > 0 Then
                        Dim Excel = construirExcel(lstErrores)
                        Excel.SaveAs(path & "ErroresCargue.xlsx")
                        Excel.Dispose()
                        hlErrores.Visible = True
                        hlErrores.NavigateUrl = "~/Files/ErroresCargue.xlsx"
                    Else
                        hlErrores.Visible = False
                        If chkSoloValida.Checked = False Then
                            CargueId = daPI.CargueProduccionAdd(Now(), 1, Session("IDUsuario"))
                            daPI.ActualizarIdCargue(CargueId)
                            CargueTemporalDiferenteEncuestasPasarAFinal()
                            ShowNotification("Se ha cargado el archivo correctamente", ShowNotifications.InfoNotification)
                        Else
                            ShowNotification("El archivo no tiene errores", ShowNotifications.InfoNotification)
                        End If
                    End If
                Else
                    ShowNotification("Las columnas no cumplen con el formato del archivo requerido", ShowNotifications.InfoNotification)
                End If
            Catch ex As Exception
				ShowNotification("Ha ocurrido un error - " & ex.Message, ShowNotifications.InfoNotification)
			End Try
        End If
    End Sub
    Sub LoadRecords(ByVal NameSheet As String, ByVal connstr As String, fileloadinfo As System.IO.FileInfo)
        'Me.pnlLoadFile.Visible = False
        'La instrucción SQL para leer los datos de la hoja. 
        Dim sqlcmd As String = "SELECT * FROM [" & NameSheet & "$A1:U20000] WHERE IdTrabajo <> NULL"
        Dim dt As DataTable = New DataTable
        'Abre la cadena de conexión que fue enviada como parámetro
        Using conn As OleDbConnection = New OleDbConnection(connstr)
            'Dim tables As DataTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
            'Ejecuta un dataaapter para abrir la base y ejecutar el comando
            Using da As OleDbDataAdapter = New OleDbDataAdapter(sqlcmd, conn)
                'Llena el objeto dt que es un datatable con la información de la hoja
                da.Fill(dt)
            End Using
        End Using
        ' Verifica q
        If dt.Rows.Count = 0 Then
            AlertJs("No se Encontraron Registros")
            ' Me.pnlLoadFile.Visible = True
            Exit Sub
        Else
            'lblRecordsSum.Text = dt.Rows.Count
        End If

        'Columnas que debe contener la plantilla
        Dim MiVectorColumnas(21) As String
        MiVectorColumnas(0) = "Linea"
        MiVectorColumnas(1) = "IdTrabajo"
        MiVectorColumnas(2) = "JobBook"
        MiVectorColumnas(3) = "NombredelEstudio"
        MiVectorColumnas(4) = "Cedula"
        MiVectorColumnas(5) = "Fecha"
        MiVectorColumnas(6) = "Cantidad"
        MiVectorColumnas(7) = "Cargo"
        MiVectorColumnas(8) = "Ciudad"
        MiVectorColumnas(9) = "Condiciones"
        MiVectorColumnas(10) = "P1"
        MiVectorColumnas(11) = "V1"
        MiVectorColumnas(12) = "P2"
        MiVectorColumnas(13) = "V2"
        MiVectorColumnas(14) = "P3"
        MiVectorColumnas(15) = "V3"
        MiVectorColumnas(16) = "P4"
        MiVectorColumnas(17) = "V4"
        MiVectorColumnas(18) = "P5"
        MiVectorColumnas(19) = "V5"
        MiVectorColumnas(20) = "Observaciones"

        'Verificar las columnas en el excel
        If dt.Columns.Count < 21 Then
            AlertJs("Revise La Estructura del Archivo")
            Exit Sub
        End If

        Dim MiError As Integer
        MiError = 0

        If dt.Columns(0).ColumnName <> MiVectorColumnas(0) Then
            AlertJs("Revise Columnas del Archivo")
            Exit Sub
        End If

        For i = 1 To 20
            If dt.Columns.Count > i And dt.Columns(i).ColumnName <> MiVectorColumnas(i) Then
                MiError = MiError + 1
            End If
        Next

        If MiError > 0 Then
            AlertJs("Las columnas del archivo no coinciden con la plantilla")
            Exit Sub
        Else
            BorrarDatosProduccionTemp()
            CopiarToSql(dt, "CC_ProduccionTemp")
            ValidarDatos()
        End If

    End Sub
    Sub CopiarToSql(ByRef MyDataTable As DataTable, ByVal TablaDestino As String)
        Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString

        Using sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(sqlConnectionString)
            sqlConnection.Open()
            Using bulkcopy As New SqlBulkCopy(sqlConnection)
                bulkcopy.DestinationTableName = TablaDestino
                Try
                    bulkcopy.BulkCopyTimeout = 0
                    bulkcopy.WriteToServer(MyDataTable)

                Catch ex As Exception
                    AlertJs("Error: " & ex.Message.ToString)
                End Try
            End Using
        End Using

    End Sub
    Sub AlertJs(ByVal message As String)
        ClientScript.RegisterStartupScript(Me.GetType(), "AlertScript", "alert('" & message & "');", True)
    End Sub
    Sub ValidarDatos()
        Dim op As New ProcesosInternos
        Dim WErrores As New List(Of CC_ObtenerProduccionErroresCargueDatos_Result)
        If op.ValidarDatosCargue(CDate(txtFechaInicio.Text), CDate(txtFechaFinalizacion.Text), ddlTipo.SelectedValue) = True Then
            AlertJs("Se Encontraron Errores en el Archivo")
            WErrores = op.ProduccionErroresCargueDatos()
        End If
        If WErrores.Count > 0 Then
            AlertJs("Se Encontraron Errores en el Archivo")
            Dim excel As New List(Of Array)
            Dim Titulos As String = "Linea;Trabajo;JobBook;NTrabajo;Cedula;Error"
            Dim DynamicColNames() As String
            Dim workbook = New XLWorkbook()
            Dim worksheet = workbook.Worksheets.Add("Errores")
            Dim path As String = Server.MapPath("~/Files/")

            DynamicColNames = Titulos.Split(CChar(";"))
            excel.Add(DynamicColNames)

            Dim DatosErrores() As String
            Dim WDatos As String

            For I = 0 To WErrores.Count - 1
                WDatos = CStr(WErrores.ToList.Item(I).Linea) & ";" & CStr(WErrores.ToList.Item(I).Idtrabajo) & ";" & WErrores.ToList.Item(I).JobBook & ";" & WErrores.ToList.Item(I).NombreTrabajo & ";" & CStr(WErrores.ToList.Item(I).Cedula) & ";" & WErrores.ToList.Item(I).Mensaje
                DatosErrores = WDatos.Split(CChar(";"))
                excel.Add(DatosErrores)
            Next
            worksheet.Cell("A1").Value = excel
            workbook.SaveAs(path & "ErroresCargue.xlsx")
            hlErrores.Visible = True
            workbook.Dispose()
        Else
            hlErrores.Visible = False
            gvHorasPorDia.DataSource = op.ValidarHorasPorDia()
            gvHorasPorDia.DataBind()
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            If chkSoloValida.Checked = True Then
                CifrasDeControl()
            Else
                Dim Datos As List(Of CC_ProduccionTempGet_Result)
                Dim cargueId As Int64
                Dim daPI As New ProcesosInternos
                Datos = op.ProduccionTemp()
                cargueId = daPI.CargueProduccionAdd(Now(), Datos.Count, Session("IDUsuario"))
                daPI.ActualizarIdCargue(cargueId)
                GuardarProduccion(Datos, cargueId)
                CifrasDeControl()
            End If
        End If

    End Sub
    Sub CifrasDeControl()
        Dim suma As Double = 0
        Dim op As New ProcesosInternos
        Dim Datos As List(Of CC_ProduccionTempGet_Result)
        Datos = op.ProduccionTemp()
        LblCantidad.Visible = True
        LblSuma.Visible = True
        If chkSoloValida.Checked = True Then
            LblCantidad.Text = "REGISTROS VALIDADOS: " & Datos.Count
            For i = 0 To Datos.Count - 1
                suma = Datos.Item(i).Cantidad + suma
            Next
            LblSuma.Text = "Total de Cantidad: " & suma
        Else
            LblCantidad.Text = "REGISTROS CARGADOS: " & Datos.Count
            For i = 0 To Datos.Count - 1
                suma = Datos.Item(i).Cantidad + suma
            Next
            LblSuma.Text = "Total de Cantidad: " & suma

            LblIdInicial.Text = "Id INICIAL: " & Session("WUltimoIdInicial")
            LblIdFinal.Text = "Id FINAL: " & Session("WUltimoIdFinal")

        End If
    End Sub
    Sub BorrarDatosProduccionTemp()
        Dim op As New ProcesosInternos
        op.EliminarProduccionTemp()
    End Sub
    Sub GuardarProduccion(ByVal Datos As List(Of CC_ProduccionTempGet_Result), ByVal cargueId As Int64)
        Dim op As New ProcesosInternos
        Datos = op.ProduccionTemp()
        Dim ValorUnitario As Double
        Dim diastrabajados As List(Of CC_ProduccionDiasTrabajados_Result)
        Dim Total As Double
        Dim WTipoContratacion As Integer

        Session("WUltimoIdInicial") = op.ObtenerUltimoIdProduccion() + 1

        For i = 0 To Datos.Count - 1
            If Datos(i).Cargo = 5400 Then
                ValorUnitario = op.ProduccionValorEncuesta(Datos(i).IdTrabajo, Datos(i).Cargo, Datos(i).Condiciones, Datos(i).P1, Datos(i).V1, Datos(i).P2, Datos(i).V2,
                                                            Datos(i).P3, Datos(i).V3, Datos(i).P4, Datos(i).V4, Datos(i).P5, Datos(i).V5)
            ElseIf Datos(i).Cargo = 5401 Then
                ValorUnitario = op.ProduccionValorEncuesta(Datos(i).IdTrabajo, Datos(i).Cargo, Datos(i).Condiciones, Datos(i).P1, Datos(i).V1, Datos(i).P2, Datos(i).V2,
                                                             Datos(i).P3, Datos(i).V3, Datos(i).P4, Datos(i).V4, Datos(i).P5, Datos(i).V5)
            Else
                ValorUnitario = op.ProduccionValorEncuesta(Datos(i).IdTrabajo, Datos(i).Cargo, Datos(i).Condiciones, Datos(i).P1, Datos(i).V1, Datos(i).P2, Datos(i).V2,
                                                            Datos(i).P3, Datos(i).V3, Datos(i).P4, Datos(i).V4, Datos(i).P5, Datos(i).V5)
            End If

            diastrabajados = op.DiasTrabajados(Datos(i).Cedula)
            Total = ValorUnitario * Datos(i).Cantidad
            If ddlTipo.SelectedValue = 1 Then
                WTipoContratacion = 7
            Else
                WTipoContratacion = 2
            End If
            op.ProduccionAdd(Datos(i).Cedula, Datos(i).IdTrabajo, Datos(i).Cantidad, ValorUnitario, Total, Datos(i).Fecha, Session("IDUsuario"), Datos(i).Ciudad, diastrabajados(0).DIASTRABAJADOS, Datos(i).Cargo, WTipoContratacion, cargueId)
        Next
        Session("WUltimoIdFinal") = op.ObtenerUltimoIdProduccion()

    End Sub
    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

        Using memoryStream = New System.IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub
    Protected Sub btnActualizarDiasTrabajados_Click(sender As Object, e As EventArgs) Handles btnActualizarDiasTrabajados.Click
        Dim esValida As Boolean = True
        Dim daProcesosInternos As New ProcesosInternos
        If String.IsNullOrEmpty(txtFechaInicio.Text) Then
            AlertJs("Debe seleccionar la fecha inicial")
            esValida = False
        End If
        If String.IsNullOrEmpty(txtFechaFinalizacion.Text) Then
            AlertJs("Debe seleccionar la fecha final")
            esValida = False
        End If
        If esValida Then
            Try
                daProcesosInternos.actualizarDiasTrabajados(txtFechaInicio.Text, txtFechaFinalizacion.Text)
                ShowNotification("Se han actualizado correctamente los días trabajados", ShowNotifications.InfoNotification)
            Catch ex As Exception
                ShowNotification("A ocurrido un error al intentar actualizas las fechas - " & ex.Message, ShowNotifications.ErrorNotification)
            End Try
        End If
    End Sub
    Function obtenerDataTableDesdeExcel(ByVal nombreHoja As String, ByVal ubicacion As String) As DataTable
        Dim sqlcmd As String = "SELECT * FROM [" & nombreHoja & "$A1:U20000] WHERE TrabajoId <> NULL"
        Dim dt As DataTable = New DataTable
        Using conn As OleDbConnection = New OleDbConnection(ubicacion)
            Using da As OleDbDataAdapter = New OleDbDataAdapter(sqlcmd, conn)
                da.Fill(dt)
            End Using
        End Using
        Return dt
    End Function
    Function obtenerHojasExcel(ByVal tipoArchivo As eTipoArchivo, ByVal cadenaConexion As String) As List(Of String)
        Dim lstHojasExcel As New List(Of String)
        Using cnn As New OleDbConnection(cadenaConexion)
            cnn.Open()
            Dim tables As DataTable = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})
            For Each row As DataRow In tables.Rows
                lstHojasExcel.Add(row.Item("Table_Name").ToString.Replace("$", "").Replace("'", ""))
            Next
        End Using
        Return lstHojasExcel
    End Function
    Function validacionesArchivoPorDias() As List(Of CC_ProduccionValidarCargueDiferenteEncuestas_Result)
        Dim daPI As New ProcesosInternos
        Return daPI.validarCargueDiferenteEncuestas(Date.Parse(txtFechaInicio.Text), Date.Parse(txtFechaFinalizacion.Text))
    End Function
    Function validarArchivoPorDiasColumnasDiferentes(ByVal dt As DataTable) As List(Of String)
        Dim colsArchivo = (From x In dt.Columns.Cast(Of DataColumn)()
                           Select x.ColumnName).ToList
        Dim ColsPermitidas = New List(Of String)({"Fila", "PersonaId", "TrabajoId", "Cantida", "VrUnitario", "Total", "Fecha", "UsuarioId", "CiudadId", "DiasTrabajados", "PresupuestoId", "TipoContratacion", "FechaLiquidacion", "VrProvisionBono"})

        Dim diferencia = colsArchivo.Except(ColsPermitidas).ToList

        Return diferencia
    End Function
    Function guardarArchivoServidor(ByVal nombreArchivo As String, ByVal path As String, ByVal controlFile As FileUpload) As Boolean
        controlFile.SaveAs(path & nombreArchivo)
    End Function
    Function obtenerCadenaConexion(ByVal tipoArchivo As eTipoArchivo, ByVal nombreArchivo As String, ByVal path As String) As String
        Dim cadenaConexion As String = ""
        If tipoArchivo = eTipoArchivo.xls Then
            cadenaConexion = "Provider=Microsoft.Jet.OLEDB.4.0;" & "Data Source=" & path & nombreArchivo & ";" & "Extended Properties='Excel 8.0'"
        ElseIf tipoArchivo = eTipoArchivo.xlsx Then
            cadenaConexion = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & path & nombreArchivo & ";" & "Extended Properties='Excel 12.0'"
        End If
        Return cadenaConexion
    End Function
    Function obtenerTipoArchivo(ByVal nombreArchivo As String) As eTipoArchivo
        Dim fileload As New System.IO.FileInfo(nombreArchivo)
        Dim tipoArchivo As eTipoArchivo
        Select Case fileload.Extension
            Case ".xls"
                tipoArchivo = eTipoArchivo.xls
            Case ".xlsx"
                tipoArchivo = eTipoArchivo.xlsx
        End Select
        Return tipoArchivo
    End Function
    Private Sub ddlTipoCargue_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoCargue.SelectedIndexChanged
        If ddlTipoCargue.SelectedValue = eTipoCargue.PorDías Then
            ddlTipo.Items.Add(New ListItem("PST", 1))
        Else
            ddlTipo.Items.Add(New ListItem("PST", 1))
            ddlTipo.Items.Add(New ListItem("Internos", 2))
        End If
    End Sub
    Function construirExcel(ByVal datos As List(Of CC_ProduccionValidarCargueDiferenteEncuestas_Result)) As XLWorkbook
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("Errores")
        worksheet.Cell(1, 1).Value = "Fila"
        worksheet.Cell(1, 2).Value = "Mensaje"
        worksheet.Cell(2, 1).Value = datos
        Return workbook
    End Function

    Sub eliminarProduccionTempDiferenteEncuestas()
        Dim daPI As New ProcesosInternos
        daPI.eliminarProduccionTemporalDiferenteEncuestas()
    End Sub

    Sub CargueTemporalDiferenteEncuestasPasarAFinal()
        Dim daPI As New ProcesosInternos
        daPI.CargueTemporalDiferenteEncuestasPasarAFinal()
    End Sub
End Class