Imports ClosedXML.Excel
Imports CoreProject
Imports CoreProject.AusenciasEquipo
Imports DocumentFormat.OpenXml.Wordprocessing
Imports System.Data.Entity.Core.Objects
Imports System.Net
Imports System.Web.Script.Services

Public Class _IndicadoresRegistroObservaciones
    Inherits System.Web.UI.Page
    Shared userId As Long
    Shared analisisIndicadoresRepository As New AnalisisIndicadoresDapper()
    Public Enum ETiposAgrupacion
        Total = 1
        PersonaAsignada = 2
        Unidad = 3
    End Enum
    Public Enum ETiposAgrupacionTipo
        Tarea = 1
        Tipo = 2
        Persona = 3
    End Enum
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        userId = HttpContext.Current.Session("IDUsuario")

    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function ErroresRegistroObservaciones(idTarea As Short?, mes As Short?, ano As Short?, agruparPor As ETiposAgrupacion, idInstrumento As Short?) As List(Of REP_ErroresRegistroObservaciones_Result)

        Dim oMatrixContext = New RP_Entities
        Dim datos As List(Of REP_ErroresRegistroObservaciones_Result)

        datos = oMatrixContext.REP_ErroresRegistroObservaciones(idTarea, mes, ano, [Enum].GetName(agruparPor.GetType, agruparPor), idInstrumento).ToList()

        Return datos

    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function UserInfo() As ObjectResult(Of US_UsuariosGrupoUnidad_Get_Result)
        Dim usuarios = New US_Entities
        Dim currentUserId = HttpContext.Current.Session("IDUsuario")

        Dim user = usuarios.US_UsuariosGrupoUnidad_Get(CLng(currentUserId))

        Return user
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function AddAnalisis(request As AnalisisInicadores_Add_Request)
        Dim result = analisisIndicadoresRepository.AddAnalisis(request)

        If result.Success Then
            HttpContext.Current.Response.StatusCode = 200
        Else
            HttpContext.Current.Response.StatusCode = 400
        End If

        Return result
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function GetAnalisis(request As AnalisisIndicadores_Get_Request)
        Return analisisIndicadoresRepository.GetAnalisis(request)
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function DeleteAnalisis(idAnalisis As Long) As Result
        Dim result = analisisIndicadoresRepository.DeleteAnalisis(idAnalisis)

        If result.Success Then
            HttpContext.Current.Response.StatusCode = 200
        Else
            HttpContext.Current.Response.StatusCode = 400
        End If

        Return result
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function UpdateAnalisis(idAnalisis As Long, content As String) As Result
        Return analisisIndicadoresRepository.UpdateAnalisisContent(idAnalisis, content)
    End Function

    <Services.WebMethod()>
    <ScriptMethod()>
    Public Shared Function Detalles(request As IndicadoresRegistroObservacionesDetalle_Get_Request) As String
        Try


            Dim response As HttpResponse = HttpContext.Current.Response
            'request.usuario = HttpContext.Current.Session("IDUsuario")

            Dim data = analisisIndicadoresRepository.GetRegistroObservacionDetalle(request)

            '"JOBBOOK;TRABAJO ID;TAREA;DOCUMENTO;AÑO;MES;USUARIO;GRUPOUNIDAD;TIPO DE OBSERVACIÓN"
            Dim nombresColumnas As IEnumerable(Of String) = {"JOBBOOK", "TRABAJO ID", "TAREA", "DOCUMENTO", "AÑO", "MES", "USUARIO", "GRUPOUNIDAD", "TIPO DE OBSERVACIÓN"}
            Dim fechaFormato As String = "dd/MM/yyyy"
            Dim nombreArchivo As String = "Detalles de Errores Registro Observaciones"

            Dim excel As New Utilidades.ResponseExcel


            Dim excelBase64 = excel.CreateExcelToBase64("Registro Observaciones Detalles", nombresColumnas, data)
            Return excelBase64
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod()>
    Public Shared Function ErroresRegistroObservacionesExcel(idTarea As Short?, mes As Short?, ano As Short?, agruparPor As ETiposAgrupacion, idInstrumento As Short?) As String
        Try
            Dim response As HttpResponse = HttpContext.Current.Response
            'request.usuario = HttpContext.Current.Session("IDUsuario")

            Dim oMatrixContext = New RP_Entities
            Dim datos As List(Of REP_ErroresRegistroObservaciones_Result)

            datos = oMatrixContext.REP_ErroresRegistroObservaciones(idTarea, mes, ano, [Enum].GetName(agruparPor.GetType, agruparPor), idInstrumento).ToList()

            Dim nombresColumnas As IEnumerable(Of String) = {"Grupo", "Año", "Mes", "Cant. Entregados", "Cant. Con Error", "Porcentaje"}

            Dim excel As New Utilidades.ResponseExcel

            Dim excelBase64 = excel.CreateExcelToBase64("Errores Registro Observaciones", nombresColumnas, datos)

            Return excelBase64
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function


    Public Shared Sub insertarNombreColumnasExcel(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String, ByVal fila As Integer)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(fila, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub
End Class