Imports CoreProject
Imports WebMatrix.Util
Imports System.IO
Imports System.Resources
Imports System.Globalization
Imports System.Threading
Imports System.Data.OleDb
Imports System.Data
Imports System.Data.SqlClient

Public Class Gestion_Traza_Facturas
    Inherits System.Web.UI.Page

#Region "Enumerados"
    Enum eEstadoFactura
        Radicada = 0
        EnAprobacion = 1
        EnTesoreria = 4
    End Enum
#End Region

    Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Page.Form.Attributes.Add("enctype", "multipart/form-data")
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'hfIdTrabajo.Value = 374
        'hfRolEjecuta.Value = 7
        'hfRolEstima.Value = 6
        'hfUnidadEjecuta.Value = UnidadesCore.Proyectos
        If Not IsPostBack Then

        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnLoadFile)
        smanager.RegisterPostBackControl(Me.btnDownloadFile)

    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub gvDatos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDatos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.DataItem, FI_FacturasRadicadasGET_Result).Estado = 7 Then
                DirectCast(e.Row.FindControl("CBSel"), CheckBox).Visible = False
                e.Row.BackColor = System.Drawing.Color.Red
            Else
                DirectCast(e.Row.FindControl("CBSel"), CheckBox).Visible = True
            End If

            If CType(e.Row.DataItem, FI_FacturasRadicadasGET_Result).Estado = 0 And CType(e.Row.DataItem, FI_FacturasRadicadasGET_Result).Escaneada = False Then
                DirectCast(e.Row.FindControl("CBSel"), CheckBox).Visible = False
            Else
                DirectCast(e.Row.FindControl("CBSel"), CheckBox).Visible = True
            End If

        End If
    End Sub

    Private Sub gvDatos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvDatos.PageIndexChanging

        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub gvDatos_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvDatos.RowCommand
        If e.CommandName = "Load" Then
            Dim nombreFactura As String = ""
            Dim o As New FI.Facturas
            Dim ent = o.ObtenerFactura(Int64.Parse(Me.gvDatos.DataKeys(CInt(e.CommandArgument)).Value))
            Dim path As String = Server.MapPath("~/Facturas/")
            nombreFactura = ent.id & ".pdf"

            Dim urlFija As String
            urlFija = "~/Facturas/"
            urlFija = Server.MapPath(urlFija & "\")

            If System.IO.File.Exists(urlFija & "\" & nombreFactura) Then
                ShowWindows("../Facturas/" & nombreFactura)
                'Response.Redirect("..\Facturas\" & nombreFactura, False)
            Else
                AlertJS("No se ha guardado una Factura escaneada para este item!")
                gvDatos.Focus()
                Exit Sub
            End If

        End If
    End Sub

    Private Sub btnFiltrarCronograma_Click(sender As Object, e As System.EventArgs) Handles btnFiltrarCronograma.Click
        If hfEstado.Value = "-1" Then
            AlertJS("Seleccione primero qué facturas desea procesar en la opción en el menú superior")
            Exit Sub
        End If
        CargarGrid()
    End Sub

    Sub CargarGrid()
        Dim o As New FI.Facturas
        Dim numRadicado As Int64?
        Dim fechai As Date?
        Dim fechaf As Date?
        Dim Rechazada As Int64?
        Me.gvDatos.Visible = True
        numRadicado = If(String.IsNullOrEmpty(txtNumRadicado.Text), CType(Nothing, Int64?), CType(txtNumRadicado.Text, Int64?))
        fechai = If(String.IsNullOrEmpty(txtFInicioI.Text), CType(Nothing, Date?), CType(txtFInicioI.Text, Date?))
        fechaf = If(String.IsNullOrEmpty(txtFFinP.Text), CType(Nothing, Date?), CType(txtFFinP.Text, Date?))
        If hfEstado.Value = 0 Then
            Rechazada = 7
        Else
            Rechazada = hfEstado.Value
        End If

        Me.gvDatos.DataSource = o.FacturasRadicadasPorEstado(Nothing, hfEstado.Value, fechai, fechaf, Nothing, Nothing, numRadicado, Rechazada)
        Me.gvDatos.DataBind()

        If Me.gvDatos.Rows.Count = 0 Then
            Me.btnEnviar.Visible = False
        Else
            Me.btnEnviar.Visible = True
        End If
    End Sub
    Private Sub lkb1_Click(sender As Object, e As EventArgs) Handles lkb1.Click
        hfEstado.Value = 0
        Me.pnlCronograma.Visible = True
        Me.btnEnviar.Text = "Enviar a aprobación"
        Me.lblTitle.Text = "Facturas para envío a aprobación"
        Me.lblNumRadicado.Visible = True
        Me.txtNumRadicado.Visible = True
        Me.lblFechaIni.Visible = True
        Me.txtFInicioI.Visible = True
        Me.lblFechaFin.Visible = True
        Me.txtFFinP.Visible = True
        Me.btnFiltrarCronograma.Visible = True
        btnEnviar.Visible = False
        Me.gvDatos.Visible = False
        Me.FileUpData.Visible = False
        Me.btnLoadFile.Visible = False
        Me.btnDownloadFile.Visible = False
        Me.lblFileIncorrect.Visible = False
        Me.gvErrores.Visible = False
        Me.lblmsgError.Visible = False
    End Sub
    Private Sub lkb2_Click(sender As Object, e As EventArgs) Handles lkb2.Click
        hfEstado.Value = 2
        Me.pnlCronograma.Visible = True
        Me.btnEnviar.Text = "Enviar a contabilidad"
        Me.lblTitle.Text = "Facturas para envío a Contabilidad"
        Me.lblNumRadicado.Visible = True
        Me.txtNumRadicado.Visible = True
        Me.lblFechaIni.Visible = True
        Me.txtFInicioI.Visible = True
        Me.lblFechaFin.Visible = True
        Me.txtFFinP.Visible = True
        Me.btnFiltrarCronograma.Visible = True
        btnEnviar.Visible = False
        Me.gvDatos.Visible = False
        Me.FileUpData.Visible = False
        Me.btnLoadFile.Visible = False
        Me.btnDownloadFile.Visible = False
        Me.lblFileIncorrect.Visible = False
        Me.gvErrores.Visible = False
        Me.lblmsgError.Visible = False
    End Sub
    Private Sub lkb3_Click(sender As Object, e As EventArgs) Handles lkb3.Click
        hfEstado.Value = 3
        Me.pnlCronograma.Visible = True
        Me.btnEnviar.Text = "Enviar a tesorería"
        Me.lblTitle.Text = "Facturas para envío a Tesorería"
        Me.lblNumRadicado.Visible = True
        Me.txtNumRadicado.Visible = True
        Me.lblFechaIni.Visible = True
        Me.txtFInicioI.Visible = True
        Me.lblFechaFin.Visible = True
        Me.txtFFinP.Visible = True
        Me.btnFiltrarCronograma.Visible = True
        btnEnviar.Visible = False
        Me.gvDatos.Visible = False
        Me.FileUpData.Visible = False
        Me.btnLoadFile.Visible = False
        Me.btnDownloadFile.Visible = False
        Me.lblFileIncorrect.Visible = False
        Me.gvErrores.Visible = False
        Me.lblmsgError.Visible = False
    End Sub
    Private Sub lkb4_Click(sender As Object, e As EventArgs) Handles lkb4.Click
        hfEstado.Value = 4
        Me.btnEnviar.Text = "Marcar como pagada"
        Me.lblTitle.Text = "Facturas para marcar como pagadas"
        Me.lblNumRadicado.Visible = False
        Me.txtNumRadicado.Visible = False
        Me.lblFechaIni.Visible = False
        Me.txtFInicioI.Visible = False
        Me.lblFechaFin.Visible = False
        Me.txtFFinP.Visible = False
        Me.btnFiltrarCronograma.Visible = False
        btnEnviar.Visible = False
        Me.gvDatos.Visible = False
        Me.FileUpData.Visible = True
        Me.btnLoadFile.Visible = True
        Me.btnDownloadFile.Visible = True
        Me.lblFileIncorrect.Visible = False
        Me.gvErrores.Visible = False
        Me.lblmsgError.Visible = False
    End Sub

    Private Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click
        Dim o As New FI.Facturas
        Dim oEnviarCorreo As New EnviarCorreo
        For Each row As GridViewRow In gvDatos.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If DirectCast(Me.gvDatos.Rows(row.RowIndex).FindControl("CBSel"), CheckBox).Checked = True Then
                    Dim info = o.ObtenerFactura(Me.gvDatos.DataKeys(row.RowIndex).Value)
                    info.Estado = CInt(hfEstado.Value) + 1
                    o.GuardarFactura(info)
                    Dim e2 As New FI_LogAprobacionFacturas
                    e2.Estado = CInt(hfEstado.Value) + 1
                    e2.Fecha = Date.UtcNow.AddHours(-5)
                    e2.IdFactura = Me.gvDatos.DataKeys(row.RowIndex).Value
                    e2.Usuario = Session("IDUsuario").ToString
                    o.GuardarLogFactura(e2)
                    If e2.Estado = eEstadoFactura.EnAprobacion Then
                        oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/FacturaRadicada.aspx?IdFactura=" & Me.gvDatos.DataKeys(row.RowIndex).Value)
                        oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EvaluarProveedorFacturas.aspx?IdFactura=" & Me.gvDatos.DataKeys(row.RowIndex).Value)
                    End If
                End If
            End If
        Next
        CargarGrid()
    End Sub

    Protected Sub btnLoadFile_Click(sender As Object, e As EventArgs) Handles btnLoadFile.Click
        lblFileIncorrect.Visible = False
        'Verifica que hayan seleccionado una ruta correcta
        If FileUpData.HasFile Then
            'La carpeta donde voy a almacenar el archivo
            Dim path As String = Server.MapPath("~/Files/")
            Dim fileload As New System.IO.FileInfo(FileUpData.FileName)
            'Verifica que las extensiones sean las permitidas, dependiendo de la extensión llama la función
            If fileload.Extension = ".xls" Then
                FileUpData.SaveAs(path & "FacturasPagadas.xls")
                hfTypeFile.Value = 0
                ReadExcel(0, path & "FacturasPagadas.xls")
            ElseIf fileload.Extension = ".xlsx" Then
                FileUpData.SaveAs(path & "FacturasPagadas.xlsx")
                hfTypeFile.Value = 1
                ReadExcel(1, path & "FacturasPagadas.xlsx")
            Else
                lblFileIncorrect.Visible = True
                Exit Sub
            End If

            'CargarDetalle()
        End If
    End Sub

    'Procedimiento que lee el archivo de Excel
    Sub ReadExcel(ByVal typefile As Int16, ByVal path As String)
        Dim connstr As String = ""
        'Dependiendo del tipo de archivo configura la cadena de conexión
        If typefile = 0 Then
            connstr = "Provider=Microsoft.Jet.OLEDB.4.0;" & "Data Source=" & path & ";" & "Extended Properties='Excel 8.0'"
        ElseIf typefile = 1 Then
            connstr = "Provider=Microsoft.ACE.OLEDB.12.0;" & "Data Source=" & path & ";" & "Extended Properties='Excel 12.0 Xml;HDR=Yes;IMEX=1;'"
        End If

        'El objeto System.IO.FileInfo guarda todas las propiedades del archivo, nombre, extensión, tamaño, etc.
        LoadRecords("FacturasPagadas".Replace("$", "").Replace("'", ""), connstr, New System.IO.FileInfo(path & "FacturasPagadas.xls"))

        'Gestion la cadena de conexión
        Using cnn As New OleDbConnection(connstr)
            'Abre la conexión
            cnn.Open()

            'Crea un datatable que lee y lista las hojas del archivo
            Dim tables As DataTable = cnn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, New Object() {Nothing, Nothing, Nothing, "TABLE"})

            cnn.Close()
        End Using


    End Sub

    Sub LoadRecords(ByVal NameSheet As String, ByVal connstr As String, fileloadinfo As System.IO.FileInfo)

        Dim o As New FI.Facturas

        'La instrucción SQL para leer los datos de la hoja. 
        Dim sqlcmd As String = "SELECT * FROM [" & NameSheet & "$A1:D250] WHERE Not NoRadicado IS NULL"
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
            AlertJS("Error: No sé encontro ningún registro")
            FileUpData.Focus()
            Exit Sub
        End If

        'Columnas que debe contener la plantilla
        Dim MiVectorColumnas(4) As String
        MiVectorColumnas(0) = "NoRadicado"
        MiVectorColumnas(1) = "Ano"
        MiVectorColumnas(2) = "FechaPagada"
        MiVectorColumnas(3) = "ValorPagado"

        'Verificar las columnas en el excel

        If dt.Columns.Count < 3 Then
            AlertJS("Error: Las columnas del archivo no coinciden con la plantilla, por favor revise!")
            FileUpData.Focus()
            Exit Sub
        End If

        Dim MiError As Integer
        MiError = 0

        For i = 0 To 3
            If dt.Columns.Count > i And dt.Columns(i).ColumnName <> MiVectorColumnas(i) Then MiError = MiError + 1
        Next

        If MiError > 0 Then
            AlertJS("Error: Las columnas del archivo no coinciden con la plantilla, por favor revise!")
            FileUpData.Focus()
            Exit Sub
        End If


        Dim numerosRepetidos = (From x In dt.Rows
                                Group By consecutivo = x(0), ano = x(1), llave = x(0) & "-" & x(1) Into Group
                                Where Group.Count > 1 AndAlso Int64.TryParse(consecutivo, New Int64) = True AndAlso Int64.TryParse(ano, New Int64) = True
                                Select llave).ToList
        '


        Dim numerosRadicoNoNumerico = (From x As DataRow In dt.Rows Where Int64.TryParse(x(0), New Int64) = False
                                       Select x(0) & "-" & x(1)).ToList

        Dim valoresPagadosNoNumericos = (From x As DataRow In dt.Rows Where Double.TryParse(x(3), New Int64) = False
                                         Select x(0) & "-" & x(1)).ToList


        Dim valoresBusqueda = String.Join(";", (From x As DataRow In dt.Rows
                                                Select x(0) & "-" & x(1)).ToArray)

        Dim resultado = o.obtenerFacturaLlaveCompuesta(valoresBusqueda)

        Dim registrosDiferentesAEstadoEnTesoreria = (From x In resultado Where x.Estado <> eEstadoFactura.EnTesoreria Select x.Consecutivo).ToList

        Dim noEncontrados = (From x In resultado Where x.Encontrado = False
                             Select x.item).ToList

        If numerosRepetidos.Count = 0 AndAlso noEncontrados.Count = 0 AndAlso numerosRadicoNoNumerico.Count = 0 AndAlso valoresPagadosNoNumericos.Count = 0 AndAlso registrosDiferentesAEstadoEnTesoreria.Count = 0 Then
            gvErrores.Visible = False
            Me.lblmsgError.Visible = True
            Me.lblmsgError.InnerText = "Archivo cargado correctamente!"
            Dim resultado2 = (From x In resultado
                              Join y As DataRow In dt.Rows On CStr(x.Consecutivo) Equals CStr(y(0)) And CStr(x.Ano) Equals CStr(y(1))
                              Select New FI_FacturasRadicadas With {.id = x.id, .IdOrden = x.IdOrden, .Tipo = x.Tipo, .TipoDocumento = x.TipoDocumento, .Consecutivo = x.Consecutivo,
                                                     .NoFactura = x.NoFactura, .Cantidad = x.Cantidad, .VrUnitario = x.VrUnitario, .Subtotal = x.Subtotal,
                                                     .ValorTotal = x.ValorTotal, .Observaciones = x.Observaciones, .Fecha = x.Fecha, .Usuario = x.Usuario,
                                                     .Estado = 5, .ValorPagado = y(3)}).ToList

            Dim resultado3 = (From x In resultado
                              Join y As DataRow In dt.Rows On CStr(x.Consecutivo) Equals CStr(y(0)) And CStr(x.Ano) Equals CStr(y(1))).ToList


            For Each x In resultado2
                o.GuardarFactura(x)
                Dim w = resultado3.Where(Function(z) z.x.id = x.id).FirstOrDefault
                Dim e2 As New FI_LogAprobacionFacturas
                e2.Estado = 5
                e2.Fecha = w.y(2)
                e2.IdFactura = x.id
                e2.Usuario = Session("IDUsuario").ToString
                o.GuardarLogFactura(e2)
            Next

        Else
            gvErrores.Visible = True
            Me.lblmsgError.Visible = True
            Me.lblmsgError.InnerText = "Archivo Incorrecto!, por favor corrija los errores y vuelva a cargar el archivo"

            Dim lstErrores = numerosRepetidos.Select(Function(x) New LstErrores With {.llave = x, .descripcion = "Se encuentra repetido en el archivo que esta intentando cargar"}).ToList
            Dim lstErrores1 = numerosRadicoNoNumerico.Select(Function(x) New LstErrores With {.llave = x, .descripcion = "El campo NoRadicado no es un valor correcto"}).ToList
            Dim lstErrores2 = valoresPagadosNoNumericos.Select(Function(x) New LstErrores With {.llave = x, .descripcion = "El campo Valor Pagado no es un valor correcto"}).ToList
            Dim lstErrores3 = noEncontrados.Select(Function(x) New LstErrores With {.llave = x, .descripcion = "La llave NoRadicado-Año no se encontró en Matrix"}).ToList
            Dim lstErrores4 = registrosDiferentesAEstadoEnTesoreria.Select(Function(x) New LstErrores With {.llave = x, .descripcion = "El registro se encuentra en un estado diferente a en tesoreria"}).ToList

            lstErrores.AddRange(lstErrores1)
            lstErrores.AddRange(lstErrores2)
            lstErrores.AddRange(lstErrores3)
            lstErrores.AddRange(lstErrores4)

            Me.gvErrores.DataSource = lstErrores
            Me.gvErrores.DataBind()

        End If

    End Sub

    Protected Sub btnDownloadFile_Click(sender As Object, e As EventArgs) Handles btnDownloadFile.Click
        descargarArchivos()
    End Sub

    Sub descargarArchivos()
        Dim urlFija As String
        urlFija = "\ArchivosCargados\Facturas"
        'HACK Deuda tecnica Aquí se deberia colocar la ruta raíz en un parametro en la base de datos

        urlFija = Server.MapPath(urlFija & "\" & "FacturasPagadas.xlsx")

        Dim path As New IO.FileInfo(urlFija)

        Response.Clear()
        Response.AddHeader("Content-Disposition", "attachment; filename=" & path.Name)
        Response.AddHeader("Content-Length", path.Length.ToString())
        Response.ContentType = "application/octet-stream"
        Response.WriteFile(urlFija)
        Response.End()

    End Sub


End Class

Class LstErrores
    Private _llave As String
    Public Property llave() As String
        Get
            Return _llave
        End Get
        Set(ByVal value As String)
            _llave = value
        End Set
    End Property

    Private _descripcion As String
    Public Property descripcion() As String
        Get
            Return _descripcion
        End Get
        Set(ByVal value As String)
            _descripcion = value
        End Set
    End Property


End Class

