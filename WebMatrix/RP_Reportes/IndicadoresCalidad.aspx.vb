Imports ClosedXML.Excel
Imports CoreProject
Imports WebMatrix.Util

Public Class IndicadoresCalidad
    Inherits System.Web.UI.Page

    Enum enumReporte
        Esquema_Analisis = 1
        Diligenciamiento_Brief = 2
        Envio_Propuestas_48Horas = 3
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			llenarDdlAnos()
		End If
	End Sub

    Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        If ddlReporte.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar un Reporte antes de continuar", ShowNotifications.InfoNotification)
            Exit Sub
        End If
        If ddlAno.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar un Año antes de continuar", ShowNotifications.InfoNotification)
            Exit Sub
        End If
        Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
        Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
		Dim estado = If((ddlEstados.SelectedValue = "" Or ddlEstados.SelectedValue = "0" Or ddlEstados.SelectedValue = "-1"), DirectCast(Nothing, Short?), Short.Parse(ddlEstados.SelectedValue))
		Dim reporte = ddlReporte.SelectedValue

        Select Case reporte
            Case enumReporte.Esquema_Analisis
                EsquemaAnalisisReporte(ano, mes, estado, "")
            Case enumReporte.Diligenciamiento_Brief
                DiligenciamientoBrief(ano, mes, "")
            Case enumReporte.Envio_Propuestas_48Horas
                EnvioPropuestas48Horas(ano, mes, estado, "")
            Case Else
                ShowNotification("Debe seleccionar un Reporte antes de continuar", ShowNotifications.InfoNotification)
                Exit Sub
        End Select
    End Sub

    Sub EsquemaAnalisisReporte(ano As Short?, mes As Short?, estado As Short?, usuario As String)
        Dim DiligenciamientoEsquemaAnalisis As List(Of REP_Diligenciamiento_Esquema_Analisis_Result)
        Dim da As New ReportesTareas

        DiligenciamientoEsquemaAnalisis = da.ObtenerDiligenciamientoEsquemaAnalisis(ano, mes, estado, usuario)

        If DiligenciamientoEsquemaAnalisis.Count > 0 Then
            gvDatosDetalle.DataSource = DiligenciamientoEsquemaAnalisis
            gvDatosDetalle.DataBind()
            Dim ListaUsuario As List(Of String) = ((From s In DiligenciamientoEsquemaAnalisis Select s.GerenteCuentas Order By GerenteCuentas).Distinct()).ToList()
            Session.Add("Diligenciamiento_Esquema_Analisis", DiligenciamientoEsquemaAnalisis)

            Dim grupoCumplimientos = (From x In DiligenciamientoEsquemaAnalisis
                                      Group x By Key1 = New With {Key x.GerenteCuentas, Key x.MesPDC, Key x.AñoPDC} Into Group
                                      Select New With {
                             .gerente = Key1.GerenteCuentas,
                             .mes = Key1.MesPDC,
                             .ano = Key1.AñoPDC,
                             .Base = Group.Count(),
                             .cumplen = Group.Where(Function(x) x.TieneEsquemaAnalisis = "Sí").ToList().Count(),
                             .porcentaje = CSng(Group.Where(Function(x) x.TieneEsquemaAnalisis = "Sí").ToList().Count()) / CSng(Group.Count())
                             }).ToList()

            Dim ListAnalisisData = New List(Of AnalisisData)

            For Each cumplimiento In grupoCumplimientos
                Dim analisis = New AnalisisData()
                analisis.Gerente = cumplimiento.gerente
                analisis.Año = cumplimiento.ano
                analisis.Mes = cumplimiento.mes
                analisis.Base = cumplimiento.Base
                analisis.Cumplimiento = cumplimiento.cumplen
                analisis.Porcentaje = cumplimiento.porcentaje.ToString("0.0%")

                ListAnalisisData.Add(analisis)
            Next

            gvDatos.DataSource = ListAnalisisData
            gvDatos.DataBind()

            ddlUsuario.Items.Clear()
            ddlUsuario.DataSource = ListaUsuario
            ddlUsuario.DataBind()
            ddlUsuario.Items.Insert(0, New ListItem("--Todos--", ""))

            pnlResultados.Visible = True
            btnDescargar.Visible = True
        Else
            ddlUsuario.DataSource = Nothing
            ddlUsuario.DataBind()

            pnlResultados.Visible = False
            btnDescargar.Visible = False
        End If
    End Sub

    Sub DiligenciamientoBrief(ano As Short?, mes As Short?, usuarioB As String)
        Dim DiligenciamientoBrief As List(Of REP_Porcentaje_Diligenciamiento_Brief_Result)
        Dim da As New ReportesTareas

        DiligenciamientoBrief = da.ObtenerPorcentajeDiligenciamientoBrief(ano, mes, usuarioB)

        If DiligenciamientoBrief.Count > 0 Then
            Dim ListBrief = New List(Of REP_Porcentaje_Diligenciamiento_Brief_Result2)
            For Each itemBrief In DiligenciamientoBrief
                Dim item = New REP_Porcentaje_Diligenciamiento_Brief_Result2()
                item.IdBrief = itemBrief.IdBrief
                item.PorcentajeDiligenciamiento = (Convert.ToDouble(itemBrief.PorcentajeDiligenciamiento) / 100).ToString("0.0%")
                item.FechaCreacionBrief = itemBrief.FechaCreacionBrief
                item.Año = itemBrief.Año
                item.Mes = itemBrief.Mes
                item.Usuario = itemBrief.Usuario

                ListBrief.Add(item)
            Next

            gvDatosDetalle.DataSource = ListBrief
            gvDatosDetalle.DataBind()
            Dim ListaUsuario As List(Of String) = ((From s In DiligenciamientoBrief Select s.Usuario Order By Usuario).Distinct()).ToList()
            Session.Add("Diligenciamiento_Brief", ListBrief)

            Dim grupoPorcentaje = (From x In DiligenciamientoBrief
                                   Group x By Key1 = New With {Key x.Usuario, Key x.Mes, Key x.Año} Into Group
                                   Select New With {
                                       .gerente = Key1.Usuario,
                                       .mes = Key1.Mes,
                                       .ano = Key1.Año,
                                       .base = Group.Count(),
                                       .porcentaje = CSng(Group.Sum(Function(x) x.PorcentajeDiligenciamiento) / Group.Count())
                                       }).ToList()

            Dim ListBriefData = New List(Of BriefData)

            For Each porcentaje In grupoPorcentaje
                Dim brief = New BriefData()
                brief.Gerente = porcentaje.gerente
                brief.Ano = porcentaje.ano
                brief.Mes = porcentaje.mes
                brief.Base = porcentaje.base
                brief.Porcentaje = (porcentaje.porcentaje / 100).ToString("0.0%")

                ListBriefData.Add(brief)
            Next

            gvDatos.DataSource = ListBriefData
            gvDatos.DataBind()


            ddlUsuario.Items.Clear()
            ddlUsuario.DataSource = ListaUsuario
            ddlUsuario.DataBind()
            ddlUsuario.Items.Insert(0, New ListItem("--Todos--", ""))

            pnlResultados.Visible = True
            btnDescargar.Visible = True
        Else
            ddlUsuario.DataSource = Nothing
            ddlUsuario.DataBind()

            pnlResultados.Visible = False
            btnDescargar.Visible = False
        End If
    End Sub

    Sub EnvioPropuestas48Horas(ano As Short?, mes As Short?, estado As Short?, usuario As String)
        Dim Propuestas48Horas As List(Of REP_Envio_Propuestas_48Horas_Result)
        Dim da As New ReportesTareas

        Propuestas48Horas = da.ObtenerEnvioPropuestas48Horas(ano, mes, estado, usuario)

        If Propuestas48Horas.Count > 0 Then
            gvDatosDetalle.DataSource = Propuestas48Horas
            gvDatosDetalle.DataBind()
            Dim ListaUsuario As List(Of String) = ((From s In Propuestas48Horas Select s.GerenteCuentas Order By GerenteCuentas).Distinct()).ToList()
            Session.Add("Envio_Propuestas_48Horas", Propuestas48Horas)

            Dim grupoPropuestas = (From x In Propuestas48Horas
                                   Group x By Key1 = New With {Key x.GerenteCuentas, Key x.MesCreacionBrief, Key x.AnoCreacionBrief} Into Group
                                   Select New With {
                                       .gerente = Key1.GerenteCuentas,
                                       .mes = Key1.MesCreacionBrief,
                                       .ano = Key1.AnoCreacionBrief,
                                       .Base = Group.Count(),
                                       .cumplen = Group.Where(Function(x) x.CumpleEnvio48Horas = "Sí").ToList().Count(),
                                       .porcentaje = Group.Where(Function(x) x.CumpleEnvio48Horas = "Sí").ToList().Count() / Group.Count()
                              }).ToList()

            Dim ListPropuestaData = New List(Of PropuestaData)

            For Each propuestaItem In grupoPropuestas
                Dim propuesta = New PropuestaData()
                propuesta.Gerente = propuestaItem.gerente
                propuesta.Año = propuestaItem.ano
                propuesta.Mes = propuestaItem.mes
                propuesta.Base = propuestaItem.Base
                propuesta.Cumplimiento = propuestaItem.cumplen
                propuesta.Porcentaje = propuestaItem.porcentaje.ToString("0.0%")

                ListPropuestaData.Add(propuesta)
            Next

            gvDatos.DataSource = ListPropuestaData
            gvDatos.DataBind()

            ddlUsuario.Items.Clear()
            ddlUsuario.DataSource = ListaUsuario
            ddlUsuario.DataBind()
            ddlUsuario.Items.Insert(0, New ListItem("--Todos--", ""))

            pnlResultados.Visible = True
            btnDescargar.Visible = True
        Else
            ddlUsuario.DataSource = Nothing
            ddlUsuario.DataBind()

            pnlResultados.Visible = False
            btnDescargar.Visible = False
        End If
    End Sub

    Private Sub btnDescargar_Click(sender As Object, e As EventArgs) Handles btnDescargar.Click
        exportarExcel()
    End Sub

    Sub exportarExcel()
        Dim wb As New XLWorkbook
        Dim ano = If(ddlAno.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlAno.SelectedValue))
        Dim mes = If(ddlMes.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlMes.SelectedValue))
        Dim estado = If(ddlEstados.SelectedValue = "-1", DirectCast(Nothing, Short?), Short.Parse(ddlEstados.SelectedValue))
        Dim usuario = If(ddlUsuario.SelectedValue = "-1", "", ddlUsuario.SelectedValue)
        Dim da As New ReportesTareas

        Dim nombre = ""
        Select Case ddlReporte.SelectedValue
            Case enumReporte.Esquema_Analisis
                nombre = "Indicador Esquema Análisis"
                Dim titulosProduccion As String = "Id;Gerente de Cuentas;Fecha Planeación PDC;Año;Mes;Esquema de Análisis;Coe;Id Trabajo;Nombre Trabajo;JOBBOOK;Estado Tarea"

                Dim ws = wb.Worksheets.Add(nombre)
                insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

                Dim DiligenciamientoEsquemaAnalisis = da.ObtenerDiligenciamientoEsquemaAnalisis(ano, mes, estado, usuario)
                ws.Cell(2, 1).InsertData(DiligenciamientoEsquemaAnalisis)

            Case enumReporte.Diligenciamiento_Brief
                nombre = "Diligenciamiento del Brief"
                Dim titulosProduccion As String = "IdBrief;PorcentajeDiligenciamiento;FechaCreacionBrief;Año;Mes;Usuario"

                Dim ws = wb.Worksheets.Add(nombre)
                insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

                Dim Propuestas48Horas = da.ObtenerPorcentajeDiligenciamientoBrief(ano, mes, usuario)
                Dim Propuestas48Horas2 = New List(Of REP_Porcentaje_Diligenciamiento_Brief_Result2)
                For Each itemBrief In Propuestas48Horas
                    Dim item = New REP_Porcentaje_Diligenciamiento_Brief_Result2()
                    item.IdBrief = itemBrief.IdBrief
                    item.PorcentajeDiligenciamiento = (Convert.ToDouble(itemBrief.PorcentajeDiligenciamiento) / 100).ToString("0.0%")
                    item.FechaCreacionBrief = itemBrief.FechaCreacionBrief
                    item.Año = itemBrief.Año
                    item.Mes = itemBrief.Mes
                    item.Usuario = itemBrief.Usuario

                    Propuestas48Horas2.Add(item)
                Next
                ws.Cell(2, 1).InsertData(Propuestas48Horas2)

            Case enumReporte.Envio_Propuestas_48Horas
                nombre = "Envío Propuestas 48 Horas"
                Dim titulosProduccion As String = "Id;Gerente de Cuentas;Fecha Planeación PDC;Año;Mes;Esquema de Análisis;Coe;Id Trabajo;Nombre Trabajo;JOBBOOK;Estado Tarea"

                Dim ws = wb.Worksheets.Add(nombre)
                insertarNombreColumnasExcel(ws, titulosProduccion.Split(";"), 1)

                Dim Propuestas48Horas = da.ObtenerEnvioPropuestas48Horas(ano, mes, estado, usuario)
                ws.Cell(2, 1).InsertData(Propuestas48Horas)

            Case Else
                ShowNotification("Debe seleccionar un Reporte antes de continuar", ShowNotifications.InfoNotification)
                Exit Sub
        End Select

        exportarExcel(wb, nombre)
    End Sub

    Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub

    Private Sub exportarExcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=" & name.Replace(" ", "_") & ".xlsx")

        Using memoryStream = New System.IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Private Sub ddlUsuario_TextChanged(sender As Object, e As EventArgs) Handles ddlUsuario.TextChanged
        Dim GerenteCuentas = ddlUsuario.SelectedValue.ToString
        Select Case ddlReporte.SelectedValue
            Case enumReporte.Esquema_Analisis
                Dim Detalle As List(Of REP_Diligenciamiento_Esquema_Analisis_Result)
                Detalle = Session("Diligenciamiento_Esquema_Analisis")
                If GerenteCuentas = "" Then
                    gvDatosDetalle.DataSource = Detalle
                Else
                    gvDatosDetalle.DataSource = Detalle.Where(Function(x) x.GerenteCuentas = GerenteCuentas).Distinct().ToList()
                End If
            Case enumReporte.Diligenciamiento_Brief
                Dim Detalle As List(Of REP_Porcentaje_Diligenciamiento_Brief_Result2)
                Detalle = Session("Diligenciamiento_Brief")
                If GerenteCuentas = "" Then
                    gvDatosDetalle.DataSource = Detalle
                Else
                    gvDatosDetalle.DataSource = Detalle.Where(Function(x) x.Usuario = GerenteCuentas).Distinct().ToList()
                End If
            Case enumReporte.Envio_Propuestas_48Horas
                Dim Detalle As List(Of REP_Envio_Propuestas_48Horas_Result)
                Detalle = Session("Envio_Propuestas_48Horas")
                If GerenteCuentas = "" Then
                    gvDatosDetalle.DataSource = Detalle
                Else
                    gvDatosDetalle.DataSource = Detalle.Where(Function(x) x.GerenteCuentas = GerenteCuentas).Distinct().ToList()
                End If
            Case Else
                gvDatosDetalle.DataSource = Nothing
        End Select

        gvDatosDetalle.DataBind()
        updDatosDetalle.Update()
    End Sub

    Private Sub ddlReporte_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlReporte.SelectedIndexChanged
        Select Case ddlReporte.SelectedValue
            Case enumReporte.Esquema_Analisis
                lblEstados.Visible = True
                ddlEstados.Visible = True

                Dim oWorkFlowEstados As New WorkFlowEstados
                Dim listaEstados = oWorkFlowEstados.obtenerListado().ToList()
                Dim EstadoAll = New CORE_WorkflowEstados()
                EstadoAll.Estado = "--Seleccione--"
                EstadoAll.id = Nothing
                listaEstados.Insert(0, EstadoAll)

                ddlEstados.DataSource = listaEstados
                ddlEstados.DataValueField = "Id"
                ddlEstados.DataTextField = "Estado"
            Case enumReporte.Diligenciamiento_Brief
                lblEstados.Visible = False
                ddlEstados.Visible = False
                ddlEstados.DataSource = Nothing
            Case enumReporte.Envio_Propuestas_48Horas
                lblEstados.Visible = True
                ddlEstados.Visible = True

                Dim oAuxiliares As New Auxiliares
                Dim listaEstados = oAuxiliares.DevolverEstadoPropuesta().ToList()
                Dim EstadoAll = New CU_EstadoPropuesta_Get_Result()
                EstadoAll.Estado = "--Seleccione--"
                EstadoAll.id = Nothing
                listaEstados.Insert(0, EstadoAll)

                ddlEstados.DataSource = listaEstados
                ddlEstados.DataValueField = "Id"
                ddlEstados.DataTextField = "Estado"
            Case Else
                lblEstados.Visible = False
                ddlEstados.Visible = False
                ddlEstados.DataSource = Nothing
        End Select
        ddlEstados.DataBind()
    End Sub

    Public Class AnalisisData
        Public Property Gerente As String
        Public Property Año As String
        Public Property Mes As Integer
        Public Property Base As Integer
        Public Property Cumplimiento As Integer
        Public Property Porcentaje As String
    End Class

    Public Class BriefData
        Public Property Gerente As String
        Public Property Ano As Integer
        Public Property Mes As Integer
        Public Property Base As Integer
        Public Property Porcentaje As String
    End Class

    Public Class PropuestaData
        Public Property Gerente As String
        Public Property Año As String
        Public Property Mes As Integer
        Public Property Base As Integer
        Public Property Cumplimiento As Integer
        Public Property Porcentaje As String
    End Class

	Public Class REP_Porcentaje_Diligenciamiento_Brief_Result2
		Public Property IdBrief As Long
		Public Property PorcentajeDiligenciamiento As String
		Public Property FechaCreacionBrief As Nullable(Of Date)
		Public Property Año As Nullable(Of Integer)
		Public Property Mes As Nullable(Of Integer)
		Public Property Usuario As String

	End Class
	Private Sub llenarDdlAnos()
		Dim anoInicial As Integer
		Dim anoFinal As Integer

		anoFinal = Date.Now.Year

		For anoInicial = 2018 To anoFinal
			ddlAno.Items.Insert(0, New ListItem With {.Text = anoInicial, .Value = anoInicial})
		Next

	End Sub
End Class