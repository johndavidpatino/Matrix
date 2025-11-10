Imports ClosedXML.Excel
Imports CoreProject
Imports WebMatrix.Util

Public Class RegistroObservacionesConsolidado
    Inherits System.Web.UI.Page

#Region "Eventos"

    Private Sub RegistroObservacionesConsolidado_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ddlAno.SelectedValue = Date.Now.Year
            ddlMes.SelectedValue = Date.Now.Month
            Dim tareasid = ""
            If Request.QueryString("un") IsNot Nothing Then
                Dim unidadTareas = Request.QueryString("un").ToString
                Select Case unidadTareas
                    Case "ES"
                        ddlTareas.Items.Clear()
                        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadistica - Diseño Muestral", "80"))
                        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadistica - Seleccion de IDMs", "51"))
                        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadística - Aprobación ponderación", "48"))
                        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadística - Proceso especiales", "37"))
                        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadística - Metodología", "23"))
                        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadística - Ponderación", "22"))
                        ddlTareas.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
                    Case Else
                        cargarTareas()
                End Select
            Else
                cargarTareas()
            End If
        End If
    End Sub

    Private Sub btnAcualizar_Click(sender As Object, e As EventArgs) Handles btnAcualizar.Click
        Dim ano = If(ddlAno.SelectedValue = "-1", Short.Parse(Date.Now.Year), Short.Parse(ddlAno.SelectedValue))
        Dim mes = If(ddlMes.SelectedValue = "-1", Nothing, Short.Parse(ddlMes.SelectedValue))
        Dim idUnidad = If(ddlUnidades.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlUnidades.SelectedValue))
        Dim idInstrumento = If(ddlInstrumentos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlInstrumentos.SelectedValue))
        Dim tareasid = ""

        If Request.QueryString("un") IsNot Nothing Then
            Dim unidadTareas = Request.QueryString("un").ToString.ToUpper
            If ddlTareas.SelectedValue = "-1" Then
                Select Case unidadTareas
                    Case "ES"
                        tareasid = "22,23,37,48,51,80"
                    Case "OP"

                    Case Else
                        tareasid = If(ddlTareas.SelectedValue <> "-1", ddlTareas.SelectedValue.ToString, "")
                End Select
            Else
                If ddlTareas.SelectedValue <> "-1" Then
                    tareasid = If(ddlTareas.SelectedValue <> "-1", ddlTareas.SelectedValue.ToString, "")
                End If
            End If
        Else
            If ddlTareas.SelectedValue <> "-1" Then
                tareasid = If(ddlTareas.SelectedValue <> "-1", ddlTareas.SelectedValue.ToString, "")
            End If
        End If

        If tareasid = "" Then
            ShowNotification("Debe seleccionar una Tarea para continuar", ShowNotifications.ErrorNotification)
            Exit Sub
        End If
        Session("tareasid") = tareasid

        Dim da As New RegistroObservaciones
        Dim RegistroObservaciones As List(Of REP_RegistroObservaciones_Consolidado_Result)
        RegistroObservaciones = da.obtenerRegistroObservacionesConsolidado(tareasid, idInstrumento, Nothing, ano, mes, idUnidad)

        If RegistroObservaciones.Count < 1 Then
            limpiar()
            ShowNotification("No se encontraron datos con el filtro indicado", ShowNotifications.ErrorNotification)
            Exit Sub
        Else
            If ddlTareas.SelectedValue <> 1 Then
                gvDatos.Columns(1).Visible = False
            End If
            gvDatos.DataSource = RegistroObservaciones
            gvDatos.DataBind()

            Dim listUsuarios = RegistroObservaciones.Select(Function(x) x.Usuario_Tarea).Distinct.ToList


            lblUsuario.Visible = True
            ddlUsuario.Visible = True
            ddlUsuario.DataSource = listUsuarios
            ddlUsuario.DataBind()
            ddlUsuario.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))

        End If

    End Sub

    Private Sub ddlTareas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTareas.SelectedIndexChanged
        limpiar()
        If ddlTareas.SelectedItem.Text = "Instrumentos" Then
            ddlInstrumentos.Visible = True
            lblInstrumento.Visible = True
        Else
            ddlInstrumentos.Visible = False
            lblInstrumento.Visible = False
            ddlInstrumentos.ClearSelection()
        End If
    End Sub
    Private Sub ddlUnidades_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUnidades.SelectedIndexChanged
        limpiar()
    End Sub

    'Private Sub btnExcelDetalle_Click(sender As Object, e As EventArgs) Handles btnExcelDetalle.Click
    '    Dim da As New RegistroObservaciones
    '    Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
    '    Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
    '    Dim idInstrumento = If(ddlInstrumentos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlInstrumentos.SelectedValue))
    '    Dim idTarea = If(ddlTareas.SelectedValue = "", DirectCast(Nothing, Short?), Short.Parse(ddlTareas.SelectedValue))
    '    Dim listaErrores As New List(Of REP_ErroresRegistroObservacionesDetalle_Result)


    '    Dim wb As New XLWorkbook

    '    Dim titulosProduccion As String = "HiloId;TrabajoId;Tarea;TareaId;DocumentoId;Documento;Ano;Mes;OrdenCarga;Usuario;GrupoUnidad;workFlowId;workFlow;Instrumento"

    '    Dim ws = wb.Worksheets.Add("DetErroresRegistroObservaciones")
    '    insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

    '    ws.Cell(2, 1).InsertData(listaErrores)

    '    exportarExcel(wb, "Detalles de Errores Registro Observaciones")
    'End Sub 
#End Region

#Region "Funciones y Métodos"
    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub

    Sub cargarTareas()
        ddlTareas.Items.Clear()
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Evaluación variables de control COE", "66"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Evaluación variables de control Proyectos", "65"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadistica - Diseño Muestral", "80"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadistica - Seleccion de IDMs", "51"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadística - Aprobación ponderación", "48"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadística - Proceso especiales", "37"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadística - Metodología", "23"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Estadística - Ponderación", "22"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Scripting - Control Interno", "76"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Scripting", "20"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Elaboración Informe", "16"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Procesamiento - Control Interno", "78"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Procesamiento Total", "13"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("PDC", "12"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Codificación", "8"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("Instrumentos", "1"))
        ddlTareas.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
    End Sub


    Sub limpiar()
        lblUsuario.Visible = False
        ddlUsuario.Visible = False
        ddlUsuario.DataSource = Nothing
        ddlUsuario.DataBind()
        ddlUsuario.Items.Insert(0, New WebControls.ListItem("--Seleccione--", "-1"))
        gvDatos.DataSource = Nothing
        gvDatos.DataBind()
    End Sub

    Private Sub ddlUsuario_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlUsuario.SelectedIndexChanged
        Dim oUsuarios As New US.Usuarios
        Dim usuarioId = oUsuarios.DevolverIdXNombreUsuario(ddlUsuario.SelectedValue)
        
        Dim tareasid = Session("tareasid").ToString
        Dim ano = If(ddlAno.SelectedValue = "-1", Short.Parse(Date.Now.Year), Short.Parse(ddlAno.SelectedValue))
        Dim mes = If(ddlMes.SelectedValue = "-1", Nothing, Short.Parse(ddlMes.SelectedValue))
        Dim idUnidad = If(ddlUnidades.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlUnidades.SelectedValue))
        Dim idInstrumento = If(ddlInstrumentos.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlInstrumentos.SelectedValue))
        
        Dim da As New RegistroObservaciones
        Dim RegistroObservaciones As List(Of REP_RegistroObservaciones_Consolidado_Result)
        RegistroObservaciones = da.obtenerRegistroObservacionesConsolidado(tareasid, idInstrumento, usuarioId, ano, mes, idUnidad)

        If ddlTareas.SelectedValue <> 1 Then
            gvDatos.Columns(1).Visible = False
        End If
        gvDatos.DataSource = RegistroObservaciones
        gvDatos.DataBind()

    End Sub
#End Region
End Class