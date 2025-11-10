Imports System.Data.OleDb
Imports System.IO
Imports CoreProject
Imports WebMatrix.Util
Imports System.Data.Common
Imports System.Data
Imports System.Data.SqlClient
Imports Microsoft.Practices.EnterpriseLibrary.Data
'Imports CoreProject.CC_FinanzasOp
Imports System.Resources
Imports System.Globalization
Imports System.Threading
Imports ClosedXML
Imports ClosedXML.Excel


Public Class CC_ExportarProduccionIDs
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(132, UsuarioID) = False Then
            Response.Redirect("../FI_AdministrativoFinanciero/Default.aspx")
        End If
        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExport)
    End Sub
    Sub AlertJs(ByVal message As String)
        ClientScript.RegisterStartupScript(Me.GetType(), "AlertScript", "alert('" & message & "');", True)
    End Sub


    Sub Exportar()
        Dim excel As New List(Of Array)
        Dim Titulos As String = "IDPRODUCCION;CC_PERSONA;NOMBRE_PERSONA;TRABAJOID;JBI;NOMBRETRABAJO;CANTIDAD;VRUNITARIO;TOTAL;FECHA;CIUDADID;CIUDAD;DIASTRABAJADOS;PRESUPUESTOID;TIPOCONTRATACION;FECHALIQUIDACION;NOMBREPRESUPUESTO;VRPROVISIONBONO"
        Dim DynamicColNames() As String
        Dim lstCambios As List(Of CC_ProduccionExportPorIDs_Result)
        Dim workbook = New XLWorkbook()
        Dim worksheet = workbook.Worksheets.Add("ExportadoProduccion")

        DynamicColNames = Titulos.Split(CChar(";"))
        excel.Add(DynamicColNames)
        Dim o As New ProcesosInternos
        lstCambios = o.ObtenerProduccionPorIDs(txtIDInicial.Text, txtIDFinal.Text)
        For Each x In lstCambios
            excel.Add((x.ID_PRODUCCION & "|" & x.CC_PERSONA & "|" & x.NOM_PERSONA & "|" & x.TrabajoId & "|" & x.JobBook & "|" & x.NombreTrabajo & "|" & x.CANTIDAD & "|" & x.VrUnitario & "|" & x.Total & "|" & x.Fecha & "|" & x.CiudadId & "|" & x.CIUDAD & "|" & x.DiasTrabajados & "|" & x.PresupuestoId & "|" & x.TIPOCONTRATACION & "|" & x.FechaLiquidacion & "|" & x.NOMBREPRESUPUESTO & "|" & x.VrProvisionBono).Split(CChar("|")).ToArray())
        Next
        worksheet.Cell("A1").Value = excel
        Try
            worksheet.Range("J2:J" & lstCambios.Count + 1).DataType = XLCellValues.DateTime
        Catch ex As Exception
        End Try
        Try
            worksheet.Range("P2:P" & lstCambios.Count + 1).DataType = XLCellValues.DateTime
        Catch ex As Exception
        End Try
        Try
            worksheet.Range("A2:B" & lstCambios.Count + 1).DataType = XLCellValues.Number
        Catch ex As Exception
        End Try
        Try
            worksheet.Range("D2:D" & lstCambios.Count + 1).DataType = XLCellValues.Number
        Catch ex As Exception
        End Try
        Try
            worksheet.Range("G2:I" & lstCambios.Count + 1).DataType = XLCellValues.Number
        Catch ex As Exception
        End Try
        Try
            worksheet.Range("L2:L" & lstCambios.Count + 1).DataType = XLCellValues.Number
        Catch ex As Exception
        End Try
        Try
            worksheet.Range("M2:O" & lstCambios.Count + 1).DataType = XLCellValues.Number
        Catch ex As Exception
        End Try
        Try
            worksheet.Range("R2:R" & lstCambios.Count + 1).DataType = XLCellValues.Number
        Catch ex As Exception
        End Try
        worksheet.Columns("A", "S").AdjustToContents(1, 2)
        'worksheet.Columns("X", "AD").AdjustToContents(1, 2)
        'worksheet.Columns("AE", "AG").AdjustToContents(1, 2)

        Crearexcel(workbook, "ReporteProduccion_ID_" & txtIDInicial.Text & "_HASTA_ID_" & txtIDFinal.Text)
    End Sub

    Private Sub Crearexcel(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)

            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If Not (IsNumeric(txtIDInicial.Text) And IsNumeric(txtIDFinal.Text)) Then
            AlertJs("Debe escribir los IDS antes de continuar")
            Exit Sub
        End If
        Exportar()
    End Sub
End Class