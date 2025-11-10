Imports ClosedXML.Excel
Imports CoreProject
Imports WebMatrix.Util

Public Class HWH_RH
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If (Not IsPostBack) Then
            Dim permisos As New Datos.ClsPermisosUsuarios
            Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

            If permisos.VerificarPermisoUsuario(151, UsuarioID) = False Then
                Response.Redirect("../Home/Default.aspx")
            End If

            CargarJefes()
        End If
    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If txtFechaIni.Text <> "" And txtFechaIni.Text <> "" Then
            cargarListadoHWH()
        Else
            ShowNotification("Las fechas son obligatorias para la búsqueda de HWH", ShowNotifications.InfoNotificationLong)
        End If
    End Sub

    Sub CargarJefes()
        Dim teletrabajo As New TeleTrabajoC
        Dim Jefes = teletrabajo.BuscarJefes()
        ddlJefe.DataSource = Jefes
        ddlJefe.DataTextField = "Nombre"
        ddlJefe.DataValueField = "Id"
        ddlJefe.DataBind()
        ddlJefe.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub cargarListadoHWH()
        Dim Jefe = If(ddlJefe.SelectedValue = "-1", Nothing, ddlJefe.SelectedValue)
        Dim estado = If(ddlEstados.SelectedValue = "-1", Nothing, ddlEstados.SelectedValue)
        Dim fechaIni = If(txtFechaIni.Text = "", Nothing, txtFechaIni.Text)
        Dim fechaFin = If(txtFechaFin.Text = "", Nothing, txtFechaFin.Text)

        Dim teletrabajo As New TeleTrabajoC
        Dim hwh = teletrabajo.BuscarXjefeDirectoXEstadoXFechas(Jefe, estado, fechaIni, fechaFin)

        If hwh.Count > 0 Then
            gvTeleTrabajo.DataSource = hwh
            gvTeleTrabajo.DataBind()
            btnDescargar.Visible = True

            pnlTeleTrabajo.Visible = True
            Dim dataJefe = Nothing
            If estado = 0 Then
                If Jefe = 0 Then
                    dataJefe = teletrabajo.BuscarTeleTrabajosJefeXJefe(fechaIni, fechaFin, Nothing, Nothing)
                Else
                    dataJefe = teletrabajo.BuscarTeleTrabajosJefeXJefe(fechaIni, fechaFin, Jefe, Nothing)
                End If
            Else
                If Jefe = 0 Then
                    dataJefe = teletrabajo.BuscarTeleTrabajosJefeXJefe(fechaIni, fechaFin, Nothing, estado)
                Else
                    dataJefe = teletrabajo.BuscarTeleTrabajosJefeXJefe(fechaIni, fechaFin, Jefe, estado)
                End If
            End If
            Dim teleTrabajoJefe = AdaptarTeletrabajoAGantt(dataJefe)

            If teleTrabajoJefe.Count > 0 Then
                Dim dataGantt = CrearTablaGantt(teleTrabajoJefe)
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "cargarGantt", "cargarGantt(" + SerializarAJSON(dataGantt) + ");", True)
            End If
        Else
            gvTeleTrabajo.DataSource = Nothing
            gvTeleTrabajo.DataBind()
            btnDescargar.Visible = False

            pnlTeleTrabajo.Visible = False
            ShowNotification("No se encontraron días de Easy Work", ShowNotifications.InfoNotification)
        End If
    End Sub

    Private Sub limpiar()
        ddlJefe.SelectedValue = -1
        ddlEstados.SelectedValue = 1
        btnDescargar.Visible = False

        gvTeleTrabajo.DataSource = Nothing
        gvTeleTrabajo.DataBind()
        pnlTeleTrabajo.Visible = False
    End Sub

    Private Sub btnLimpiarBuscar_Click(sender As Object, e As EventArgs) Handles btnLimpiarBuscar.Click
        limpiar()
    End Sub

    Private Sub btnDescargar_Click(sender As Object, e As EventArgs) Handles btnDescargar.Click
        exportarExcel()
    End Sub

    Sub exportarExcel()
        Dim wb As New XLWorkbook

        Dim nombre = "Easy Work"
        Dim titulosProduccion As String = "#;Id Usuario;Nombre Usuario;Area Persona;Fecha Programada;Observaciones;Cod Estado;Estado;Id Usuario Gestión;Usuario Gestion;Fecha Gestión;Observaciones Gestión;Fecha Creación"

        Dim ws = wb.Worksheets.Add(nombre)
        insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

        Dim Jefe = If(ddlJefe.SelectedValue = "-1", Nothing, ddlJefe.SelectedValue)
        Dim estado = If(ddlEstados.SelectedValue = "-1", Nothing, ddlEstados.SelectedValue)
        Dim fechaIni = If(txtFechaIni.Text = "", Nothing, txtFechaIni.Text)
        Dim fechaFin = If(txtFechaFin.Text = "", Nothing, txtFechaFin.Text)

        Dim teletrabajo As New TeleTrabajoC
        Dim hwh = teletrabajo.BuscarXjefeDirectoXEstadoXFechas(Jefe, estado, fechaIni, fechaFin)

        ws.Cell(2, 1).InsertData(hwh)

        exportarExcelxls(wb, nombre)
    End Sub
    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
    Private Sub exportarExcelxls(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=" & name.Replace(" ", "_") & ".xlsx")

        Using memoryStream = New System.IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Private Sub gvTeleTrabajo_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvTeleTrabajo.PageIndexChanging
        gvTeleTrabajo.PageIndex = e.NewPageIndex
        cargarListadoHWH()
    End Sub

    Public Function AdaptarTeletrabajoAGantt(data As List(Of TH_TeleTrabajoJefeXJefe_Result)) As List(Of TH_TeleTrabajoAllJefe)
        Dim TeleTrabajoAllJefe As New List(Of TH_TeleTrabajoAllJefe)

        For Each item As TH_TeleTrabajoJefeXJefe_Result In data
            Dim itemAllJefe As New TH_TeleTrabajoAllJefe
            itemAllJefe.id = item.id
            itemAllJefe.Usuario = item.Usuario
            itemAllJefe.Nombre = item.NombreUsuario
            itemAllJefe.Descripcion = item.Observaciones
            itemAllJefe.Estado = item.NombreEstado
            itemAllJefe.FechaInicio = Convert.ToDateTime(item.Fecha + " 00:00:00").ToString
            itemAllJefe.FechaFinal = Convert.ToDateTime(item.Fecha + " 23:59:59").ToString


            TeleTrabajoAllJefe.Add(itemAllJefe)
        Next

        Return TeleTrabajoAllJefe
    End Function
    Public Function CrearTablaGantt(data As List(Of TH_TeleTrabajoAllJefe)) As Gantt
        Dim Cronograma = New Gantt()
        Dim FechaInicial = Nothing
        Dim FechaFinal = Nothing
        Dim ListaSerie = New List(Of serie)
        Dim dependency = Nothing
        Dim c = 0
        For Each row As TH_TeleTrabajoAllJefe In data
            c += 1
            If FechaInicial Is Nothing Then
                FechaInicial = row.FechaInicio
            Else
                Dim FechaMenor = Convert.ToDateTime(FechaInicial)
                Dim FechaActual = Convert.ToDateTime(row.FechaInicio)
                If (Not (row.FechaInicio Is Nothing)) Then
                    If FechaActual.Date < FechaMenor.Date Then
                        FechaInicial = FechaActual.ToString("dd/MM/yyyy")
                    End If
                End If
            End If

            If FechaFinal Is Nothing Then
                FechaFinal = row.FechaFinal
            Else
                Dim FechaMayor = Convert.ToDateTime(FechaFinal)
                Dim FechaActual = Convert.ToDateTime(row.FechaFinal)
                If (Not (row.FechaFinal Is Nothing)) Then
                    If FechaActual.Date > FechaMayor.Date Then
                        FechaFinal = FechaActual.ToString("dd/MM/yyyy")
                    End If
                End If
            End If

            Dim S As New serie()
            S.name = row.Nombre
            S.id = row.id
            S.parent = "Listado_TeleTrabajo"
            S.fstart = Format(Convert.ToDateTime(row.FechaInicio), "dd/MM/yyyy")
            S.fend = Format(Convert.ToDateTime(row.FechaFinal), "dd/MM/yyyy")
            S.owner = row.Usuario
            S.estado = row.Estado
            S.descripcion = row.Descripcion
            If (Not (S.fstart Is Nothing)) Then
                ListaSerie.Add(S)
            End If
        Next

        Cronograma.FechaIni = Format(Convert.ToDateTime(FechaInicial), "dd/MM/yyyy")
        Cronograma.FechaFin = Format(Convert.ToDateTime(FechaFinal), "dd/MM/yyyy")
        Cronograma.series = ListaSerie

        Return Cronograma
    End Function
    Shared Function SerializarAJSON(Of T)(ByVal objeto As T) As String
        Dim JSON As System.Web.Script.Serialization.JavaScriptSerializer = New System.Web.Script.Serialization.JavaScriptSerializer
        Return JSON.Serialize(objeto)
    End Function

#Region "Objetos Adicionales"
    Public Class TH_TeleTrabajoAllJefe
        Public Property id As String
        Public Property Usuario As String
        Public Property Nombre As String
        Public Property Descripcion As String
        Public Property Estado As String
        Public Property FechaInicio As String
        Public Property FechaFinal As String

    End Class

    Public Class Gantt
        Private _FechaIni As String
        Private _FechaFin As String
        Private _series As List(Of serie)
        Public Property FechaIni() As String
            Get
                Return _FechaIni
            End Get
            Set(ByVal value As String)
                _FechaIni = value
            End Set
        End Property
        Public Property FechaFin() As String
            Get
                Return _FechaFin
            End Get
            Set(ByVal value As String)
                _FechaFin = value
            End Set
        End Property
        Public Property series() As List(Of serie)
            Get
                Return _series
            End Get
            Set(ByVal value As List(Of serie))
                _series = value
            End Set
        End Property
    End Class

    Public Class serie
        Private _name As String
        Private _id As String
        Private _parent As String
        Private _fstart As String
        Private _fend As String
        Private _owner As String
        Private _estado As String
        Private _descripcion As String

        Public Property name() As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
            End Set
        End Property
        Public Property id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
            End Set
        End Property
        Public Property parent() As String
            Get
                Return _parent
            End Get
            Set(ByVal value As String)
                _parent = value
            End Set
        End Property
        Public Property fstart() As String
            Get
                Return _fstart
            End Get
            Set(ByVal value As String)
                _fstart = value
            End Set
        End Property
        Public Property fend() As String
            Get
                Return _fend
            End Get
            Set(ByVal value As String)
                _fend = value
            End Set
        End Property
        Public Property owner() As String
            Get
                Return _owner
            End Get
            Set(ByVal value As String)
                _owner = value
            End Set
        End Property

        Public Property estado() As String
            Get
                Return _estado
            End Get
            Set(ByVal value As String)
                _estado = value
            End Set
        End Property

        Public Property descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property
    End Class
#End Region
End Class