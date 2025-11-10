Imports System.Resources
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient
Imports CoreProject

Public Class ImportarDatos
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(135, UsuarioID) = False Then
            Response.Redirect("../RE_GT/HomeRecoleccion.aspx")
        End If
        Me.pnlSheets.Visible = False
        Me.pnlSummaryLoad.Visible = False
        Me.pnlLoadComplete.Visible = False
        Me.pnlResumen.Visible = False

    End Sub



    Protected Sub btnLoadFile_Click(sender As Object, e As EventArgs) Handles btnLoadFile.Click
        lblFileIncorrect.Visible = False
        'Verifica que hayan seleccionado una ruta correcta
        If FileUpData.HasFile Then
            'La carpeta donde voy a almacer el archivo
            Dim path As String = Server.MapPath("~/Files/")
            Dim fileload As New System.IO.FileInfo(FileUpData.FileName)
            'Verifica que las extensiones sean las permitidas, dependiendo de la extensión llama la función
            If fileload.Extension = ".xls" Then
                FileUpData.SaveAs(path & "Data.xls")
                hfTypeFile.Value = 0
                ReadExcel(0, path & "Data.xls")
            ElseIf fileload.Extension = ".xlsx" Then
                FileUpData.SaveAs(path & "Data.xlsx")
                hfTypeFile.Value = 1
                ReadExcel(1, path & "Data.xlsx")
            Else
                lblFileIncorrect.Visible = True
                Exit Sub
            End If

        End If
    End Sub

    'Procedimiento que lee el archivo de Excel
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
            pnlSheets.Visible = True

            'Recorre el datable Tables y agrega al listbox los nombres de las hojas
            ListBoxSheets.Items.Clear()
            For Each row As DataRow In tables.Rows
                ListBoxSheets.Items.Add(row.Item("Table_Name").ToString.Replace("$", "").Replace("'", ""))
            Next
            cnn.Close()
        End Using
    End Sub


    ' Revisa la información en el excel
    Protected Sub btnLoadData_Click(sender As Object, e As EventArgs) Handles btnLoadData.Click

        'En general configura la cadena de conexión nuevamente y envía como parámetro el nombre de la hoja seleccionada
        Dim connstr As String = ""
        Dim path As String = Server.MapPath("~/Files/")
        If hfTypeFile.Value = 0 Then
            connstr = "Provider=Microsoft.Jet.OLEDB.4.0;" & "Data Source=" & path & "Data.xls" & ";" & "Extended Properties='Excel 8.0'"
        ElseIf hfTypeFile.Value = 1 Then
            connstr = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & path & "Data.xlsx" & ";" & "Extended Properties='Excel 12.0'"
        End If
        'El objeto System.IO.FileInfo guarda todas las propiedades del archivo, nombre, extensión, tamaño, etc.
        LoadRecords(ListBoxSheets.SelectedValue.Replace("$", "").Replace("'", ""), connstr, New System.IO.FileInfo(path & "Data.xls"))

    End Sub


    Sub LoadRecords(ByVal NameSheet As String, ByVal connstr As String, fileloadinfo As System.IO.FileInfo)
        Me.pnlLoadFile.Visible = False
        'La instrucción SQL para leer los datos de la hoja. 
        Dim sqlcmd As String = "SELECT * FROM [" & NameSheet & "$A1:AS11]"
        Dim dt As DataTable = New DataTable
        'Abre la cadena de conexión que fue enviada como parámetro
        Using conn As OleDbConnection = New OleDbConnection(connstr)
            'Ejecuta un dataaapter para abrir la base y ejecutar el comando
            Using da As OleDbDataAdapter = New OleDbDataAdapter(sqlcmd, conn)
                'Llena el objeto dt que es un datatable con la información de la hoja
                da.Fill(dt)
            End Using
        End Using
        'Verifica que efectivamente traiga datos
        If dt.Rows.Count = 0 Then
            AlertJs("Error: No sé encontro ningún registro")
            Me.pnlLoadFile.Visible = True
            Exit Sub
        Else
            lblRecordsSum.Text = dt.Rows.Count
        End If

        'Columnas que debe contener la plantilla
        Dim MiVectorColumnas(8) As String
        MiVectorColumnas(0) = "TrabajoId"
        MiVectorColumnas(1) = "Res_Numero"
        MiVectorColumnas(2) = "Per_NumIdentificacionEncu"
        MiVectorColumnas(3) = "Per_NumIdentificacionSup"
        MiVectorColumnas(4) = "Res_IDM"
        MiVectorColumnas(5) = "Res_Ciudad"
        MiVectorColumnas(6) = "Res_Fecha"
        MiVectorColumnas(7) = "TipoSupervision"
        MiVectorColumnas(8) = "TipoActividad"

        'Verificar las columnas en el excel

        If dt.Columns.Count < 9 OrElse dt.Columns.Count > 9 Then
            AlertJs("Error: Las columnas del archivo no coinciden con la plantilla, por favor revise!")
            Me.pnlLoadFile.Visible = True
            Exit Sub
        End If

        Dim MiError As Integer
        MiError = 0

        For i = 0 To 8
            If dt.Columns.Count > i And dt.Columns(i).ColumnName <> MiVectorColumnas(i) Then MiError = MiError + 1
        Next


        Dim todosLosValoresValidos As Boolean = ValidarValoresTipoActividad(dt)

        'Se valida que los datos para TipoActividad concuerden con los permitidos
        If todosLosValoresValidos = False Then
            AlertJs("Error: Algunos de los datos en la columna [TipoActividad], no estan de acuerdo a los valores permitidos!")
            Me.pnlLoadFile.Visible = True
            Exit Sub
        End If

        If MiError > 0 Then
            AlertJs("Error: Las columnas del archivo no coinciden con la plantilla, por favor revise!")
            Me.pnlLoadFile.Visible = True
            Exit Sub
        Else
            lblEstado.Text = "La estructura de los datos en la hoja de excel ha sido verificada"
        End If

        'Ocultar o Mostra paneles
        pnlLoadFile.Visible = True
        pnlSheets.Visible = True
        pnlSummaryLoad.Visible = True
    End Sub

    Protected Sub btnCargarData_Click(sender As Object, e As EventArgs) Handles btnCargarData.Click

        'En general configura la cadena de conexión nuevamente y envía como parámetro el nombre de la hoja seleccionada
        Dim connstr As String = ""
        Dim path As String = Server.MapPath("~/Files/")
        If hfTypeFile.Value = 0 Then
            connstr = "Provider=Microsoft.Jet.OLEDB.4.0;" & "Data Source=" & path & "Data.xls" & ";" & "Extended Properties='Excel 8.0'"
        ElseIf hfTypeFile.Value = 1 Then
            connstr = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & path & "Data.xlsx" & ";" & "Extended Properties='Excel 12.0'"
        End If

        Me.pnlLoadFile.Visible = False
        'La instrucción SQL para leer los datos de la hoja. 
        Dim sqlcmd As String = "SELECT * FROM [" & ListBoxSheets.SelectedValue.Replace("$", "").Replace("'", "") & "$A1:AS65536]"
        Dim dt As DataTable = New DataTable
        'Abre la cadena de conexión que fue enviada como parámetro
        Using conn As OleDbConnection = New OleDbConnection(connstr)
            'Ejecuta un dataaapter para abrir la base y ejecutar el comando
            Using da As OleDbDataAdapter = New OleDbDataAdapter(sqlcmd, conn)
                'Llena el objeto dt que es un datatable con la información de la hoja
                da.Fill(dt)
            End Using
        End Using

        ' Ejecutar SP para dejar en blanco las tablas RespuestasCatiRMCtmp y RespuestasCatiRMCInconsistencias
        EjecutarProcedimientoAlmacenado("CatiRMC_BorrarDatosRespuestasCatiRMCtmp")

        'Bulkcopy
        CopiarToSql(dt, "RespuestasCatiRMCtmp")

        Me.pnlSummaryLoad.Visible = False
        Me.pnlLoadComplete.Visible = True
        Me.pnlResumen.Visible = False

    End Sub

    'Realiza el bulk copy
    Sub CopiarToSql(ByRef MyDataTable As DataTable, ByVal TablaDestino As String)
        Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("GestionCampoConnectionString").ToString
        'Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.AppSettings.Get("GestionCampoConnectionString")
        Dim sqlConnection As SqlClient.SqlConnection = New SqlClient.SqlConnection(sqlConnectionString)
        sqlConnection.Open()
        Using bulkcopy As New SqlBulkCopy(sqlConnection)
            bulkcopy.DestinationTableName = TablaDestino ' "dbo.RespuestasCatiRMCtmp"
            Try
                bulkcopy.BulkCopyTimeout = 0
                bulkcopy.ColumnMappings.Add("TrabajoId", "E_Id")
                bulkcopy.ColumnMappings.Add("Res_Numero", "Res_Numero")
                bulkcopy.ColumnMappings.Add("Per_NumIdentificacionEncu", "Per_NumIdentificacionEncu")
                bulkcopy.ColumnMappings.Add("Per_NumIdentificacionSup", "Per_NumIdentificacionSup")
                bulkcopy.ColumnMappings.Add("Res_IDM", "Res_IDM")
                bulkcopy.ColumnMappings.Add("Res_Ciudad", "Res_Ciudad")
                bulkcopy.ColumnMappings.Add("Res_Fecha", "Res_Fecha")
                bulkcopy.ColumnMappings.Add("TipoSupervision", "TipoSupervision")
                'Se modifica el IdTable en el archivo de entrada por TipoActividad, pero se conserva en base de datos el mismo nombre IdTablet
                bulkcopy.ColumnMappings.Add("TipoActividad", "idTablet")
                bulkcopy.WriteToServer(MyDataTable)
                'AlertJs("Datos Insers")
            Catch ex As Exception
                AlertJs("Error: " & ex.Message.ToString)
            End Try
        End Using
        sqlConnection.Close()
    End Sub

    ' Revisar Datos importados ejecutando SP y generar informe de errores
    Private Sub btnFinalizar_Click(sender As Object, e As EventArgs) Handles btnFinalizar.Click

        ' Ejecutar Procedimiento Almacenado para validar Datos, marcar los validos y generar incosistencias
        EjecutarProcedimientoAlmacenado("CatiRMC_ValidarDatosRespuestasCatiRMCtmp")

        ' Cargar en un grilla el resumen de los datos importados
        GridView2.DataSource = ObtenerDatosdeProcedimientoAlmacenado("CatiRMC_ReportarResumenValidasNuevas")
        GridView2.DataBind()
        GridView3.DataSource = ObtenerDatosdeProcedimientoAlmacenado("CatiRMC_ReportarResumenNoValidasNuevas")
        GridView3.DataBind()
        GridView4.DataSource = ObtenerDatosdeProcedimientoAlmacenado("CatiRMC_ReportarResumenDuplicadas")
        GridView4.DataBind()
        ' Cargar en un grilla el resumen de los datos importados con error
        GridView1.DataSource = ObtenerDatosdeProcedimientoAlmacenado("CatiRMC_ReportarInconsistencias")
        GridView1.DataBind()


        ' Insertar registros válidos en la tabla Respuestas
        EjecutarProcedimientoAlmacenadoParametro("CatiRMC_InsertarDatosEnRespuestas", Session("IDUsuario").ToString)

        Me.pnlSummaryLoad.Visible = False
        Me.pnlLoadComplete.Visible = False
        Me.pnlResumen.Visible = True

    End Sub


    Sub AlertJs(ByVal message As String)
        ClientScript.RegisterStartupScript(Me.GetType(), "AlertScript", "alert('" & message & "');", True)
    End Sub

    Sub EjecutarProcedimientoAlmacenado(ByVal NomProcedimiento As String)
        Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("GestionCampoConnectionString").ToString
        'Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.AppSettings.Get("GestionCampoConnectionString")
        Using sqlConnection As New SqlClient.SqlConnection(sqlConnectionString)
            sqlConnection.Open()
            Using SqlCommand As New SqlClient.SqlCommand(NomProcedimiento, sqlConnection)
                SqlCommand.CommandType = CommandType.StoredProcedure
                SqlCommand.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Function ObtenerDatosdeProcedimientoAlmacenado(ByVal NomProcedimiento As String) As DataTable
        Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("GestionCampoConnectionString").ToString
        ' Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.AppSettings.Get("GestionCampoConnectionString")
        Dim MiDataTable As New DataTable
        Using sqlConnection As New SqlClient.SqlConnection(sqlConnectionString)
            sqlConnection.Open()
            Using SqlCommand As New SqlClient.SqlCommand(NomProcedimiento, sqlConnection)
                SqlCommand.CommandType = CommandType.StoredProcedure
                Dim MiDataAdaptor As New SqlDataAdapter(SqlCommand)
                MiDataAdaptor.Fill(MiDataTable)
            End Using
        End Using
        Return MiDataTable
    End Function

    Sub EjecutarProcedimientoAlmacenadoParametro(ByVal NomProcedimiento As String, ByVal Usuario_Id As Long)
        Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.ConnectionStrings("GestionCampoConnectionString").ToString
        ' Dim sqlConnectionString As String = System.Configuration.ConfigurationManager.AppSettings.Get("GestionCampoConnectionString")
        Using sqlConnection As New SqlClient.SqlConnection(sqlConnectionString)
            sqlConnection.Open()
            Using SqlCommand As New SqlClient.SqlCommand(NomProcedimiento, sqlConnection)
                SqlCommand.CommandType = CommandType.StoredProcedure
                SqlCommand.Parameters.AddWithValue("@Usuario_Id", Usuario_Id)
                SqlCommand.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Function ValidarValoresTipoActividad(dt As DataTable) As Boolean
        For Each row As DataRow In dt.Rows
            Dim valor As Int32 = row("TipoActividad")
            If Not [Enum].IsDefined(GetType(ETipoActividad), valor) Then
                Return False ' Un valor no está en el enum, retorna falso
            End If
        Next
        Return True ' Todos los valores están en el enum, retorna verdadero
    End Function

    Public Enum ETipoActividad
        EncuestaNormal = 1
        EncuestaFiltro = 10
        Jornada = 20
        MediaJornada = 21
        JornadaDominical = 22
        MediaJornadaDominical = 23
    End Enum
End Class