Imports System.Resources
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient
Imports CoreProject
Imports System.IO
Imports DocumentFormat.OpenXml.Spreadsheet
Imports WebMatrix.ImportarDatos
Imports ClosedXML.Excel

Public Class ImportarPlanillas
    Inherits System.Web.UI.Page

    Private ReadOnly Headers As List(Of String) = New List(Of String)({"TrabajoId", "Per_NumIdentificacionEncu", "Res_Ciudad", "Res_Fecha", "TipoActividad", "FechaCarga", "Revisado", "RevisadoPor", "FechaRevision"})
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(135, UsuarioID) = False Then
            Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        End If

    End Sub
    Protected Sub btnUploadFile_Click(sender As Object, e As EventArgs) Handles btnUploadFile.Click

        If Not FileUpData.HasFile Then
            Exit Sub
        End If

        Dim ext = IO.Path.GetExtension(FileUpData.FileName)
        Dim connstr As String

        If ext <> ".xls" And ext <> ".xlsx" Then
            Exit Sub
        End If

        Dim fileName = Guid.NewGuid().ToString() & ext

        Dim tempFilesDirectory As String = Server.MapPath("~/Files/Temp/ImportarPlantillas")

        If Not Directory.Exists(tempFilesDirectory) Then
            Directory.CreateDirectory(tempFilesDirectory)
        End If

        If ext = ".xls" Then
            connstr = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES;'"
        ElseIf ext = ".xlsx" Then
            connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES;'"
        End If

        If String.IsNullOrEmpty(connstr) Then
            Exit Sub
        End If
        Dim dt As DataTable = New DataTable()

        Dim tempFilePath = IO.Path.Combine(tempFilesDirectory, fileName)

        Using fileStream As Stream = FileUpData.PostedFile.InputStream
            Using memoryStream As MemoryStream = New MemoryStream()
                fileStream.CopyTo(memoryStream)
                memoryStream.Position = 0


                IO.File.WriteAllBytes(tempFilePath, memoryStream.ToArray())

                Using connExcel As OleDbConnection = New OleDbConnection(String.Format(connstr, tempFilePath))
                    Using cmdExcel As OleDbCommand = New OleDbCommand()
                        Using oda As OleDbDataAdapter = New OleDbDataAdapter()
                            cmdExcel.Connection = connExcel

                            connExcel.Open()
                            Dim dtExcelSchema As DataTable

                            dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, Nothing)

                            Dim sheetName As String = dtExcelSchema.Rows(0).Item("TABLE_NAME").ToString()

                            connExcel.Close()

                            connExcel.Open()
                            cmdExcel.CommandText = "SELECT * From [" + sheetName + "]"
                            oda.SelectCommand = cmdExcel
                            oda.Fill(dt)
                            connExcel.Close()

                        End Using
                    End Using
                End Using
            End Using
        End Using

        If dt.Rows.Count = 0 Then
            AlertJs("Error: No sé encontro ningún registro")
            Exit Sub
        End If

        dt.Columns.Add("SubidoPor")
        dt.Columns.Add("FechaCarga")
        dt.Columns.Add("Revisado")
        dt.Columns.Add("RevisadoPor")
        dt.Columns.Add("FechaRevision")

        'Validar estructura Excel
        Dim validHeaders = ValidateHeaders(dt, Headers)

        If validHeaders = False Then
            AlertJs("Error: La estructura de la plantilla no es correcta")
            Exit Sub
        End If

        'Validar tipo de actividad
        Dim validActivityType = ValidarValoresTipoActividad(dt)

        If validActivityType = False Then
            AlertJs("Error: Algunos tipos de actividad no son los correctos")
            Exit Sub
        End If

        Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString

        Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(sqlConnectionString)

        sqlConnection.Open()

        Dim festivosDt As DataTable = New DataTable()

        Using conn As New SqlConnection(sqlConnectionString)
            Dim query As String = "SELECT * FROM _Festivos"
            Using cmd As New SqlCommand(query, conn)
                Using adapter As New SqlDataAdapter(cmd)
                    adapter.Fill(festivosDt)
                End Using
            End Using
        End Using

        For Each row As DataRow In dt.Rows
            row("FechaCarga") = Date.UtcNow.AddHours(-5)
            row("Revisado") = False
            row("SubidoPor") = Session("IDUsuario").ToString
        Next

        Dim dtCollection() = dt.Select()
        Dim today = DateTime.Now()
        For Each row As DataRow In dtCollection

            Dim tipoActividad = Int32.Parse(row("TipoActividad"))
            Dim idEncuestador = row("Per_NumIdentificacionEncu")
            Dim trabajoId = row("TrabajoId")
            Dim cantidad = row("Cantidad")
            Dim resFechaString = row("Res_Fecha")
            Dim resFecha = DateTime.Parse(resFechaString)
            Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
            If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
            Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)

            If resFecha.Date > finCorteFecha Or resFecha.Date < inicioCorteFecha Then
                AlertJs($"Error: La fecha no está dentro del rango del corte para el encuestador: {idEncuestador}, en el TrabajoId: {trabajoId}, y con fecha: {resFecha.ToString("dd/MM/yyyy")}")
                Exit Sub
            End If

            If Not IsNumeric(cantidad) Then
                AlertJs("La cantidad no es valida")
                Exit Sub
            End If
            If Integer.Parse(cantidad) < 1 Then
                AlertJs("La cantidad no es valida")
                Exit Sub
            End If
            If tipoActividad = ETipoActividadPlanilla.MediaJornadaDominical Or tipoActividad = ETipoActividadPlanilla.JornadaDominical Then


                If resFecha.DayOfWeek = DayOfWeek.Sunday Then
                    Continue For
                End If

                Dim dateString = resFecha.ToString("yyyy-MM-dd")

                Dim festivoStored = festivosDt.Select($"festivo = '{dateString}'")

                If festivoStored.Length = 0 Then
                    AlertJs($"Error: El registro dominical {dateString} no es valido, revisar la fecha")
                    Exit Sub
                End If
            End If
        Next





        Using bulkcopy As New SqlBulkCopy(sqlConnection)
            bulkcopy.DestinationTableName = "OP_CuantiPlanillas"

            Try
                bulkcopy.BulkCopyTimeout = 0
                bulkcopy.ColumnMappings.Add("TrabajoId", "TrabajoId")
                bulkcopy.ColumnMappings.Add("Per_NumIdentificacionEncu", "Per_NumIdentificacionEncu")
                bulkcopy.ColumnMappings.Add("Res_Ciudad", "Res_Ciudad")
                bulkcopy.ColumnMappings.Add("Res_Fecha", "Res_Fecha")
                bulkcopy.ColumnMappings.Add("Cantidad", "Cantidad")
                bulkcopy.ColumnMappings.Add("TipoActividad", "TipoActividad")
                bulkcopy.ColumnMappings.Add("SubidoPor", "SubidoPor")
                bulkcopy.ColumnMappings.Add("FechaCarga", "FechaCarga")
                bulkcopy.ColumnMappings.Add("Revisado", "Revisado")
                bulkcopy.ColumnMappings.Add("RevisadoPor", "RevisadoPor")
                bulkcopy.ColumnMappings.Add("FechaRevision", "FechaRevision")
                bulkcopy.WriteToServer(dt)
                PanelSuccess.Visible = True

            Catch ex As Exception
                If TypeOf ex Is SqlException And ex.Message.Contains("IX_OP_CuantiPlanillas_Unique_Trabajo_Per_ResFecha") Then
                    AlertJs($"Error: verificar que no existan duplicados o que ya esten registrados")
                Else
                    AlertJs($"Error: ocurrio un error al agregar los datos, comuniquese con el administrador")
                End If
                If File.Exists(tempFilePath) Then
                    File.Delete(tempFilePath)
                End If
                Exit Sub
            End Try

        End Using






        If File.Exists(tempFilePath) Then
            File.Delete(tempFilePath)
        End If



    End Sub

    Public Function ValidateHeaders(dt As DataTable, expectedHeaders As List(Of String)) As Boolean
        For Each header As String In expectedHeaders
            If Not dt.Columns.Contains(header) Then
                Return False
            End If
        Next
        Return True
    End Function
    Sub AlertJs(ByVal message As String)
        ClientScript.RegisterStartupScript(Me.GetType(), "AlertScript", "alert('" & message & "');", True)
    End Sub

    Function ValidarValoresTipoActividad(dt As DataTable) As Boolean
        For Each row As DataRow In dt.Rows
            Dim valor As Int32 = row("TipoActividad")
            If Not [Enum].IsDefined(GetType(ETipoActividadPlanilla), valor) Then
                Return False ' Un valor no está en el enum, retorna falso
            End If
        Next
        Return True ' Todos los valores están en el enum, retorna verdadero
    End Function
    Public Enum ETipoActividadPlanilla
        EncuestaNormal = 1
        EncuestaFiltro = 10
        EncuestaFiltroNSE_1to2 = 11
        EncuestaFiltroNSE_3to4 = 12
        EncuestaFiltroNSE_5to6 = 13
        Jornada = 20
        MediaJornada = 21
        JornadaDominical = 22
        MediaJornadaDominical = 23
    End Enum
End Class