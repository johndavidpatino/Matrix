Imports Microsoft.Practices.EnterpriseLibrary.Data
Imports Microsoft.Practices.ServiceLocation
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Threading
Imports System.Data.Common
Imports System.Data
Imports System.Data.SqlClient
Imports WebMatrix.Util
Public Class CargarErrores
    Inherits System.Web.UI.Page
    Private db As Database
    Dim Mensaje1 As String
    Dim Mensaje2 As String
    Public Sub New()
        db = DatabaseFactory.CreateDatabase()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    Protected Sub btnPasarServidor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPasarServidor.Click
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
                    If File.Exists(MapPath("~/Files/") & "ERRORES" & "." & Session("FILEEXT").ToString()) Then
                        File.Delete(MapPath("~/Files/") & "ERRORES" & "." & Session("FILEEXT").ToString())
                    End If

                    File.Move(MapPath("~/Files/") & Session("FILENAME").ToString(), MapPath("~/Files/") & "ERRORES" & "." & Session("FILEEXT").ToString())

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
    Protected Sub btnCargarDatos_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCargarDatos.Click
        Dim Errores As New Errores()
        Try
            If lstHoja.SelectedIndex > 0 And lstHoja.Items.Count > 0 Then
                If (File.Exists(Server.MapPath("~/Files/ERRORES" & "." & Session("FILEEXT").ToString))) Then
                    Dim MyConnection As System.Data.OleDb.OleDbConnection
                    Dim DtSet As System.Data.DataSet
                    Dim MyCommand As System.Data.OleDb.OleDbDataAdapter
                    MyConnection = New System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0; Data Source='" & Server.MapPath("~/Files/ERRORES" & "." & Session("FILEEXT").ToString) & "';Extended Properties='Excel 12.0;HDR=NO; IMEX=1'")
                    MyCommand = New System.Data.OleDb.OleDbDataAdapter("SELECT * FROM [" & lstHoja.SelectedValue & "]", MyConnection)

                    DtSet = New System.Data.DataSet

                    MyCommand.Fill(DtSet)

                    InsertarErrores(DtSet)

                    File.Delete(Server.MapPath("~/Files/ERRORES" & "." & Session("FILEEXT").ToString))

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
        MyConnection = New System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0; Data Source='" & Server.MapPath("~/Files/ERRORES" & "." & Session("FILEEXT").ToString) & "';Extended Properties='Excel 12.0;HDR=NO; IMEX=1'")
        Dim dt As DataTable = Nothing

        Try
            lstHoja.Items.Clear()
2:
            MyConnection.Open()
            dt = MyConnection.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, Nothing)

            Dim r As DataRow
            lstHoja.Items.Add(New ListItem("Seleccione...", "Seleccione..."))
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
    Private Sub InsertarErrores(ByVal dt As DataSet)
        Dim Er As New VOErrores()
        Dim connection As DbConnection = db.CreateConnection()
        connection.Open()
        Dim transaccion As DbTransaction = connection.BeginTransaction()
        Dim i As Integer

        Dim comando As DbCommand
        Dim Dr As SqlDataReader

        Dim EncuestasCargadas As Integer
        Dim EncuestasNoCargadas As Integer
        EncuestasCargadas = 0
        EncuestasNoCargadas = 0

        i = 5  'Leer primera fila de datos
        Er.Estudio = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(3))
        Er.Mes = Convert.ToInt32(dt.Tables(0).Rows(i).ItemArray.GetValue(1))
        Mensaje1 = "Estudio : " & Er.Estudio.ToString() & " - " & "MES : " & Er.Mes.ToString()
        Try
            For i = 5 To dt.Tables(0).Rows.Count - 1
                'If i = 31 Then
                ' MsgBox("PARAR")
                'End If

                If (dt.Tables(0).Rows(i).ItemArray.GetValue(0) Is DBNull.Value) Then
                    i = dt.Tables(0).Rows.Count
                Else
                    'VALIDAR SI EXISTE ESTE ESTUDIO y ESTA ENCUESTA - No la incluye
                    Er.Estudio = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(3))
                    Er.Encuesta = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(6))

                    comando = db.GetStoredProcCommand("MBO_OPCampoExisteEstudioEncuesta")
                    db.AddInParameter(comando, "Estudio", System.Data.DbType.Decimal, Er.Estudio)
                    db.AddInParameter(comando, "Encuesta", System.Data.DbType.Decimal, Er.Encuesta)
                    Dr = db.ExecuteReader(comando, transaccion)

                    If Not Dr.Read Then
                        Dr.Close()

                        Er.Año = Convert.ToInt32(dt.Tables(0).Rows(i).ItemArray.GetValue(0))
                        Er.Mes = Convert.ToInt32(dt.Tables(0).Rows(i).ItemArray.GetValue(1))
                        Er.Unidad = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(2))

                        Er.Ciudad = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(7))
                        Er.Encuestador = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(8))
                        Er.Supervisor = CDec(dt.Tables(0).Rows(i).ItemArray.GetValue(9))
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(10) Is DBNull.Value) Then
                            Er.Error1 = 0
                        Else
                            Er.Error1 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(10))
                        End If
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(12) Is DBNull.Value) Then
                            Er.Pregunta1 = ""
                        Else
                            Er.Pregunta1 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(12))
                        End If
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(13) Is DBNull.Value) Then
                            Er.Error2 = 0
                        Else
                            Er.Error2 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(13))
                        End If
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(15) Is DBNull.Value) Then
                            Er.Pregunta2 = ""
                        Else
                            Er.Pregunta2 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(15))
                        End If
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(16) Is DBNull.Value) Then
                            Er.Error3 = 0
                        Else
                            Er.Error3 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(16))
                        End If
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(18) Is DBNull.Value) Then
                            Er.Pregunta3 = ""
                        Else
                            Er.Pregunta3 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(18))
                        End If

                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(19) Is DBNull.Value) Then
                            Er.Error4 = 0
                        Else
                            Er.Error4 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(19))
                        End If
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(21) Is DBNull.Value) Then
                            Er.Pregunta4 = ""
                        Else
                            Er.Pregunta4 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(21))
                        End If

                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(22) Is DBNull.Value) Then
                            Er.Error5 = 0
                        Else
                            Er.Error5 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(22))
                        End If
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(24) Is DBNull.Value) Then
                            Er.Pregunta5 = ""
                        Else
                            Er.Pregunta5 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(24))
                        End If

                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(25) Is DBNull.Value) Then
                            Er.Error6 = 0
                        Else
                            Er.Error6 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(25))
                        End If
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(27) Is DBNull.Value) Then
                            Er.Pregunta6 = ""
                        Else
                            Er.Pregunta6 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(27))
                        End If

                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(28) Is DBNull.Value) Then
                            Er.Error7 = 0
                        Else
                            Er.Error7 = CInt(dt.Tables(0).Rows(i).ItemArray.GetValue(28))
                        End If
                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(30) Is DBNull.Value) Then
                            Er.Pregunta7 = ""
                        Else
                            Er.Pregunta7 = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(30))
                        End If

                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(31) Is DBNull.Value) Then
                            Er.Disp1 = 0
                        Else
                            Er.Disp1 = Convert.ToInt32(dt.Tables(0).Rows(i).ItemArray.GetValue(31))
                        End If

                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(33) Is DBNull.Value) Then
                            Er.Disp2 = 0
                        Else
                            Er.Disp2 = Convert.ToInt32(dt.Tables(0).Rows(i).ItemArray.GetValue(33))
                        End If

                        If (dt.Tables(0).Rows(i).ItemArray.GetValue(35) Is DBNull.Value) Then
                            Er.FechaRecepcion = ""
                        Else
                            Er.FechaRecepcion = CStr(dt.Tables(0).Rows(i).ItemArray.GetValue(35))
                        End If

                        comando = db.GetStoredProcCommand("MBO_OPCampoInsertarErrores")
                        db.AddInParameter(comando, "Año", System.Data.DbType.Int32, Er.Año)
                        db.AddInParameter(comando, "Mes", System.Data.DbType.Int32, Er.Mes)
                        db.AddInParameter(comando, "Unidad", System.Data.DbType.[String], Er.Unidad)
                        db.AddInParameter(comando, "Estudio", System.Data.DbType.Decimal, Er.Estudio)
                        db.AddInParameter(comando, "Encuesta", System.Data.DbType.Decimal, Er.Encuesta)
                        db.AddInParameter(comando, "Ciudad", System.Data.DbType.Decimal, Er.Ciudad)
                        db.AddInParameter(comando, "Encuestador", System.Data.DbType.Decimal, Er.Encuestador)
                        db.AddInParameter(comando, "Supervisor", System.Data.DbType.Decimal, Er.Supervisor)
                        db.AddInParameter(comando, "Error1", System.Data.DbType.Int32, Er.Error1)
                        db.AddInParameter(comando, "Pregunta1", System.Data.DbType.[String], Er.Pregunta1)
                        db.AddInParameter(comando, "Error2", System.Data.DbType.Int32, Er.Error2)
                        db.AddInParameter(comando, "Pregunta2", System.Data.DbType.[String], Er.Pregunta2)
                        db.AddInParameter(comando, "Error3", System.Data.DbType.Int32, Er.Error3)
                        db.AddInParameter(comando, "Pregunta3", System.Data.DbType.[String], Er.Pregunta3)
                        db.AddInParameter(comando, "Error4", System.Data.DbType.Int32, Er.Error4)
                        db.AddInParameter(comando, "Pregunta4", System.Data.DbType.[String], Er.Pregunta4)
                        db.AddInParameter(comando, "Error5", System.Data.DbType.Int32, Er.Error5)
                        db.AddInParameter(comando, "Pregunta5", System.Data.DbType.[String], Er.Pregunta5)
                        db.AddInParameter(comando, "Error6", System.Data.DbType.Int32, Er.Error6)
                        db.AddInParameter(comando, "Pregunta6", System.Data.DbType.[String], Er.Pregunta6)
                        db.AddInParameter(comando, "Error7", System.Data.DbType.Int32, Er.Error7)
                        db.AddInParameter(comando, "Pregunta7", System.Data.DbType.[String], Er.Pregunta7)
                        db.AddInParameter(comando, "Disp1", System.Data.DbType.Int32, Er.Disp1)
                        db.AddInParameter(comando, "Disp2", System.Data.DbType.Int32, Er.Disp2)
                        db.AddInParameter(comando, "FechaRecepcion", System.Data.DbType.[String], Er.FechaRecepcion)

                        db.ExecuteNonQuery(comando, transaccion)
                        EncuestasCargadas = EncuestasCargadas + 1
                    Else
                        EncuestasNoCargadas = EncuestasNoCargadas + 1
                        Dr.Close()
                    End If
                End If
            Next
            transaccion.Commit()
            Mensaje2 = Mensaje1 & " - Cargadas : " & (EncuestasCargadas).ToString() & " - " & "Ya existían : " & (EncuestasNoCargadas).ToString() & "     "

        Catch ex As Exception
            transaccion.Rollback()
            ShowNotification("Linea : " + (i + 1).ToString() + " - " + ex.Message.Replace("{", "").Replace("}", ""), WebMatrix.ShowNotifications.InfoNotification)
            'Throw New Exception("Linea : " + (i + 1).ToString() + " - " + ex.Message.Replace("{", "").Replace("}", ""))
        Finally
            If (connection.State = ConnectionState.Open) Then
                connection.Close()
            End If
        End Try

    End Sub
End Class