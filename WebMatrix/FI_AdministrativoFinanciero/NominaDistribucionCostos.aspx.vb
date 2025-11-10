Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.ServiceLocation
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Threading
Imports System.Data.Common
Imports System.Data
Imports System.Data.SqlClient
Imports WebMatrix.Util
Imports ClosedXML.Excel
Public Class NominaDistribucionCostos
    Inherits System.Web.UI.Page
    Private db As Database
    Dim Mensaje1 As String
    Dim Mensaje2 As String
    Dim StrSql As String
    Dim DtSet As System.Data.DataSet
    Dim comando As DbCommand
    Dim Dr As SqlDataReader

    Dim DtSetErrores As System.Data.DataSet
    Dim DtSetDistribucion As System.Data.DataSet

    Dim WFechaInicial As Date
    Dim WFechaFinal As Date
    Dim WTipoNomina As String
    Dim WFilas As Integer
    Dim WTotalCosto As Double

    Public Sub New()
        db = DatabaseFactory.CreateDatabase()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Private Sub btnPasarServidor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPasarServidor.Click
        limpiar()
        If IsPostBack Then
            Dim fileOK As [Boolean] = False

            Dim path As [String] = Server.MapPath("~/Files/")

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
                    If File.Exists(MapPath("~/Files/") & "NOMINA" & "." & Session("FILEEXT").ToString()) Then
                        File.Delete(MapPath("~/Files/") & "NOMINA" & "." & Session("FILEEXT").ToString())
                    End If

                    File.Move(MapPath("~/Files/") & Session("FILENAME").ToString(), MapPath("~/Files/") & "NOMINA" & "." & Session("FILEEXT").ToString())

                    ObtenerHojasNombres()

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
    Private Sub btnCargarDatos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCargarDatos.Click
        Dim Errores As New Errores()
        Try
            If Session("WHoja") > 0 And Session("WNHojas") > 0 Then
                If (File.Exists(Server.MapPath("~/Files/NOMINA" & "." & Session("FILEEXT").ToString))) Then
                    CargarDataTable()

                    CargarCostos(DtSet)

                    File.Delete(Server.MapPath("~/Files/NOMINA" & "." & Session("FILEEXT").ToString))

                    ShowNotification("Datos cargados! - " & Mensaje2, WebMatrix.ShowNotifications.InfoNotification)

                Else
                    ShowNotification("El archivo no existe, probablemente no lo ha cargado!", WebMatrix.ShowNotifications.ErrorNotification)
                End If
            Else
                ShowNotification("Seleccione la hoja del archivo que contiene los datos!", WebMatrix.ShowNotifications.ErrorNotification)
            End If

        Catch ex As Exception
            ShowNotification(ex.Message.Replace("'", "").Replace("" & vbCrLf & "", "<br>"), WebMatrix.ShowNotifications.ErrorNotification)
        End Try
    End Sub
    Private Sub ObtenerHojasNombres()
        Dim MyConnection As System.Data.OleDb.OleDbConnection
        MyConnection = New System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0; Data Source='" & Server.MapPath("~/Files/NOMINA" & "." & Session("FILEEXT").ToString) & "';Extended Properties='Excel 12.0;HDR=NO; IMEX=1'")
        Dim dt As DataTable = Nothing

        Try
            lstHoja.Items.Clear()
2:
            MyConnection.Open()
            dt = MyConnection.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, Nothing)

            Dim r As DataRow
            lstHoja.Items.Add(New ListItem("Seleccione...".ToString()))
            If dt IsNot Nothing Then
                For Each r In dt.Rows
                    lstHoja.Items.Add(New ListItem(r("TABLE_NAME").ToString(), r("TABLE_NAME").ToString()))
                Next
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            MyConnection.Close()
        End Try
    End Sub
    Private Sub CargarCostos(ByVal dt As DataSet)
        Dim WCedula As Decimal
        Dim WVrCosto As Double
        Dim WActividad As Integer
        Dim WNombre As String
        Dim WNTipoContratacion As Integer

        Dim WJobBook As String
        Dim WHoras As Double

        Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
        Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(sqlConnectionString)
        sqlConnection.Open()

        Dim transaccion As DbTransaction = sqlConnection.BeginTransaction()
        Dim i, er As Integer
        Dim WError As Boolean

        Dim RegistrosCargados As Integer
        Dim RegistrosNoCargados As Integer
        Dim RegistrosConError As Integer
        RegistrosCargados = 0
        RegistrosNoCargados = 0
        RegistrosConError = 0

        er = 0
        i = 0  'Leer fila de datos del archivo
        WFechaInicial = Convert.ToDateTime(dt.Tables(0).Rows(i).ItemArray.GetValue(4))
        WFechaFinal = Convert.ToDateTime(dt.Tables(0).Rows(i).ItemArray.GetValue(5))
        WTipoNomina = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(6))

        Session("FechaInicial") = WFechaInicial
        Session("FechaFinal") = WFechaFinal
        Session("TipoNomina") = WTipoNomina

        Session("Año") = Year(WFechaFinal)
        Session("Mes") = Month(WFechaFinal)

        If Session("TipoNomina") = "IPSOSFIJOSSALARIO" Then
            'BORRAR TABLA PRODUCCION FIJOS HORAS
            comando = db.GetStoredProcCommand("CC_NominaDistribucionBorrarProdFijos")
            db.ExecuteNonQuery(comando, transaccion)
            'Cargar Produccion de los fijos
            For i = 0 To dt.Tables(0).Rows.Count - 1  'Leer desde primera fila de datos de personas
                If (dt.Tables(0).Rows(i).ItemArray.GetValue(0) Is DBNull.Value) Then
                    i = dt.Tables(0).Rows.Count
                Else
                    WCedula = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(0))
                    WJobBook = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(1))
                    WActividad = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(2))
                    WHoras = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(3))
                    comando = db.GetStoredProcCommand("CC_NominaDistribucionInsertarProdFijos")
                    db.AddInParameter(comando, "Cedula", System.Data.DbType.Decimal, WCedula)
                    db.AddInParameter(comando, "JobBook", System.Data.DbType.String, WJobBook)
                    db.AddInParameter(comando, "Actividad", System.Data.DbType.Int32, WActividad)
                    db.AddInParameter(comando, "Horas", System.Data.DbType.Double, WHoras)
                    db.ExecuteNonQuery(comando, transaccion)

                    RegistrosCargados = RegistrosCargados + 1
                End If
            Next
            transaccion.Commit()
            Mensaje2 = Mensaje1 & " - Cargados: " & (RegistrosCargados).ToString()
        Else
            'BORRAR TABLA DE NOMINA DEl AÑO Y MES - TABLA DE ERRORES
            comando = db.GetStoredProcCommand("CC_NominaDistribucionBorrarMesErrores")
            db.ExecuteNonQuery(comando, transaccion)

            Try
                For i = 0 To dt.Tables(0).Rows.Count - 1  'Leer desde primera fila de datos de personas
                    If (dt.Tables(0).Rows(i).ItemArray.GetValue(0) Is DBNull.Value) Then
                        i = dt.Tables(0).Rows.Count
                    Else
                        'VALIDAR SI EXISTE PERSONA
                        WCedula = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(0))
                        WVrCosto = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(1))
                        WActividad = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(2))
                        WNTipoContratacion = 0
                        WNombre = "NO EXISTE"

                        comando = db.GetStoredProcCommand("CC_NominaDistribucionExisteCedula")
                        db.AddInParameter(comando, "Cedula", System.Data.DbType.Decimal, WCedula)
                        Dr = db.ExecuteReader(comando, transaccion)

                        WError = False

                        If Dr.Read Then
                            WNTipoContratacion = Dr.GetValue(0)
                            WNombre = Dr.GetValue(1)
                            Dr.Close()
                            If ((WTipoNomina = "1-IpsosProductividad" Or WTipoNomina = "3-IpsosFijosHoras" Or WTipoNomina = "4-IpsosFijosProduccion" Or
                                 WTipoNomina = "6-IpsosLiqFijosHorasyProduccion" Or WTipoNomina = "7-IpsosLiqProductividad") And
                                    (WNTipoContratacion = 1 Or WNTipoContratacion = 2 Or WNTipoContratacion = 5)) Or
                                ((WTipoNomina = "2-TemporalProductividad" Or WTipoNomina = "5-TemporalFijosProduccion") And
                                    (WNTipoContratacion = 3 Or WNTipoContratacion = 6)) Then
                            Else
                                WError = True
                                RegistrosConError = RegistrosConError + 1
                                Dr.Close()
                            End If
                            comando = db.GetStoredProcCommand("CC_NominaDistribucionInsertarRegistros")
                            db.AddInParameter(comando, "Año", System.Data.DbType.Int32, Session("Año"))
                            db.AddInParameter(comando, "Mes", System.Data.DbType.Int32, Session("Mes"))
                            db.AddInParameter(comando, "TipoNomina", System.Data.DbType.String, WTipoNomina)
                            db.AddInParameter(comando, "TipoContratacion", System.Data.DbType.Int32, WNTipoContratacion)
                            db.AddInParameter(comando, "Cedula", System.Data.DbType.Decimal, WCedula)
                            db.AddInParameter(comando, "VrCosto", System.Data.DbType.Double, WVrCosto)
                            db.AddInParameter(comando, "Actividad", System.Data.DbType.Int32, WActividad)
                            db.ExecuteNonQuery(comando, transaccion)

                            RegistrosCargados = RegistrosCargados + 1
                        Else
                            WError = True
                            RegistrosNoCargados = RegistrosNoCargados + 1
                            Dr.Close()
                        End If
                        If WError = True Then
                            comando = db.GetStoredProcCommand("CC_NominaDistribucionInsertarRegistrosErrores")
                            db.AddInParameter(comando, "Año", System.Data.DbType.Int32, Session("Año"))
                            db.AddInParameter(comando, "Mes", System.Data.DbType.Int32, Session("Mes"))
                            db.AddInParameter(comando, "TipoNomina", System.Data.DbType.String, WTipoNomina)
                            db.AddInParameter(comando, "TipoContratacion", System.Data.DbType.Int32, WNTipoContratacion)
                            db.AddInParameter(comando, "Cedula", System.Data.DbType.Decimal, WCedula)
                            db.AddInParameter(comando, "Nombre", System.Data.DbType.String, WNombre)
                            db.AddInParameter(comando, "VrCosto", System.Data.DbType.Double, WVrCosto)
                            db.ExecuteNonQuery(comando, transaccion)
                            Dr.Close()

                            WError = False
                        End If

                    End If
                Next
                transaccion.Commit()

                StrSql = "SELECT * FROM CC_NominaDistribucionLiquidacionErrores"
                Dim Mycommand As New SqlCommand(StrSql, sqlConnection)
                Dim ErroresAdapter As New SqlDataAdapter
                ErroresAdapter.SelectCommand = Mycommand
                DtSetErrores = New System.Data.DataSet
                ErroresAdapter.Fill(DtSetErrores, "Errores")
                GVErrores.DataSource = DtSetErrores
                GVErrores.DataBind()

                Mensaje2 = Mensaje1 & " - Cargados: " & (RegistrosCargados).ToString() & " - " & "No existen: " & (RegistrosNoCargados).ToString() & "Errores: " & (RegistrosConError).ToString() & "     "

            Catch ex As Exception
                transaccion.Rollback()
                ShowNotification("Linea : " + (i + 1).ToString() + " - " + ex.Message.Replace("{", "").Replace("}", ""), WebMatrix.ShowNotifications.InfoNotification)
                'Throw New Exception("Linea : " + (i + 1).ToString() + " - " + ex.Message.Replace("{", "").Replace("}", "")) 
            Finally
                If (sqlConnection.State = ConnectionState.Open) Then
                    sqlConnection.Close()
                End If
            End Try

        End If

    End Sub
    Private Sub ForzarPostback()
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "jsKeys", "javascript:Forzar();", True)
        'ClientScript.RegisterClientScriptBlock(GetType(Page), "jskeys", "javascript:Forzar();", True)
    End Sub
    Private Sub lstHoja_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstHoja.SelectedIndexChanged
        Session("WHoja") = lstHoja.SelectedIndex
        Session("WNHojas") = lstHoja.Items.Count
        Session("WNombreHoja") = lstHoja.SelectedValue

        CargarDataTable()

        'LEER DATOS DE LA HOJA
        Dim connection As DbConnection = db.CreateConnection()
        connection.Open()

        txtFechaInicial.Text = DtSet.Tables(0).Rows(0).ItemArray.GetValue(4)
        txtFechaFinal.Text = DtSet.Tables(0).Rows(0).ItemArray.GetValue(5)
        txtTipoContratacion.Text = CStr(DtSet.Tables(0).Rows(0).ItemArray.GetValue(6))
        txtCostoADistribuir.Text = CDbl(DtSet.Tables(0).Rows(0).ItemArray.GetValue(7))

        'ForzarPostback()

        connection.Close()

    End Sub
    Private Sub CargarDataTable()
        Dim MyConnection As System.Data.OleDb.OleDbConnection

        Dim MyCommand As System.Data.OleDb.OleDbDataAdapter

        If Session("FILEEXT") = ".xls" Then
            'connstr = "Provider=Microsoft.Jet.OLEDB.4.0;" & "Data Source=" & Path & ";" & "Extended Properties='Excel 8.0'"
            MyConnection = New System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0; Data Source='" & Server.MapPath("~/Files/NOMINA" & "." & Session("FILEEXT").ToString) & "';Extended Properties='Excel 8.0'")
        Else
            MyConnection = New System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0; Data Source='" & Server.MapPath("~/Files/NOMINA" & "." & Session("FILEEXT").ToString) & "';Extended Properties='Excel 12.0;HDR=YES; IMEX=1'")
        End If

        Dim WNombreHoja As String
        WNombreHoja = Session("WNombreHoja")

        MyCommand = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" & WNombreHoja & "]", MyConnection)
        DtSet = New System.Data.DataSet
        MyCommand.Fill(DtSet)
    End Sub

    Protected Sub btnDistribucion_Click(sender As Object, e As EventArgs) Handles btnDistribucion.Click
        Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
        Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(sqlConnectionString)
        sqlConnection.Open()
        Dim transaccion As DbTransaction = SqlConnection.BeginTransaction()

        'DISTRIBUCION
        If Session("TipoNomina") = "1-IpsosProductividad" Then
            comando = db.GetStoredProcCommand("CC_NominaDistribucion1-IpsosProductividad")
        End If
        If Session("TipoNomina") = "2-TemporalProductividad" Then
            comando = db.GetStoredProcCommand("CC_NominaDistribucion2-TemporalProductividad")
        End If
        If Session("TipoNomina") = "3-IpsosFijosHoras" Then
            comando = db.GetStoredProcCommand("CC_NominaDistribucion3-IpsosFijosHoras")
        End If
        If Session("TipoNomina") = "4-IpsosFijosProduccion" Then
            comando = db.GetStoredProcCommand("CC_NominaDistribucion4-Ipsos5-Temporal-FijosProduccion")
        End If
        If Session("TipoNomina") = "5-TemporalFijosProduccion" Then
            comando = db.GetStoredProcCommand("CC_NominaDistribucion4-Ipsos5-Temporal-FijosProduccion")
        End If
        If Session("TipoNomina") = "6-IpsosLiqFijosHorasyProduccion" Then
            comando = db.GetStoredProcCommand("CC_NominaDistribucion6-IpsosLiqFijosHorasyProduccion7-Productividad")
        End If
        If Session("TipoNomina") = "7-IpsosLiqProductividad" Then
            comando = db.GetStoredProcCommand("CC_NominaDistribucion6-IpsosLiqFijosHorasyProduccion7-Productividad")
        End If

        db.AddInParameter(comando, "FechaInicial", System.Data.DbType.Date, Session("FechaInicial"))
        db.AddInParameter(comando, "FechaFinal", System.Data.DbType.Date, Session("FechaFinal"))
        db.AddInParameter(comando, "TipoNomina", System.Data.DbType.String, Session("TipoNomina"))
        db.ExecuteNonQuery(comando, transaccion)

        'INSERTAR HISTORICOS - DETALLE y REPORTE DEL MES
        comando = db.GetStoredProcCommand("CC_NominaDistribucionInsertarHistoricos")
        db.AddInParameter(comando, "Año", System.Data.DbType.Int32, Session("Año"))
        db.AddInParameter(comando, "Mes", System.Data.DbType.Int32, Session("Mes"))
        db.AddInParameter(comando, "TipoNomina", System.Data.DbType.String, Session("TipoNomina"))
        db.ExecuteNonQuery(comando, transaccion)

        transaccion.Commit()

        MostrarDistribucion()

        sqlConnection.Close()
    End Sub
    Protected Sub GVDistribucion_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GVDistribucion.PageIndexChanging
        GVDistribucion.PageIndex = e.NewPageIndex
        MostrarDistribucion()
    End Sub
    Private Sub MostrarDistribucion()
        Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
        Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(sqlConnectionString)
        sqlConnection.Open()

        'MOSTRAR  DISTRIBUCION CALCULADA
        StrSql = "SELECT * FROM CC_NominaDistribucionReporte"
        Dim Mycommand As New SqlCommand(StrSql, SqlConnection)
        Dim DistribucionAdapter As New SqlDataAdapter
        DistribucionAdapter.SelectCommand = Mycommand
        DtSetDistribucion = New System.Data.DataSet
        DistribucionAdapter.Fill(DtSetDistribucion, "Distribucion")
        GVDistribucion.DataSource = DtSetDistribucion
        GVDistribucion.DataBind()

        With DtSetDistribucion.Tables(0)
            WTotalCosto = .Compute("Sum(VrCosto)", "")
        End With

        txtVrDistribuido.Text = WTotalCosto

        sqlConnection.Close()
    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
        Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(sqlConnectionString)
        sqlConnection.Open()
        StrSql = "SELECT * FROM CC_NominaDistribucionReporte"
        Dim Mycommand As New SqlCommand(StrSql, SqlConnection)
        Dim DistribucionAdapter As New SqlDataAdapter
        DistribucionAdapter.SelectCommand = Mycommand
        DtSetDistribucion = New System.Data.DataSet
        DistribucionAdapter.Fill(DtSetDistribucion, "Distribucion")

        Dim WArchivo As String
        WArchivo = "NominaDistribucion" & "-" & Session("TipoNomina") & "-" & Session("Año") & "-" & Session("Mes") & ".xlsx"
        Dim wb = New XLWorkbook()
        wb.Worksheets.Add(DtSetDistribucion.Tables(0))
        Crearexcel(wb, WArchivo)

        sqlConnection.Close()
    End Sub
    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=" & name)

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub
    Private Sub limpiar()
        txtFechaInicial.Text = Nothing
        txtFechaFinal.Text = Nothing
        txtTipoContratacion.Text = Nothing
        GVErrores.DataSource = Nothing
        GVErrores.DataBind()
        GVDistribucion.DataSource = Nothing
        GVDistribucion.DataBind()
        txtCostoADistribuir.Text = Nothing
        txtVrDistribuido.Text = Nothing
    End Sub
End Class