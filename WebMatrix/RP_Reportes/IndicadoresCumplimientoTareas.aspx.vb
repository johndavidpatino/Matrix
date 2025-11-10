Imports CoreProject
Imports CoreProject.AusenciasEquipo
Imports System.Data.Entity.Core.Objects
Imports System.Net
Imports System.Web.Script.Services

Public Class _IndicadoresCumplimientoTareas
    Inherits System.Web.UI.Page
    Shared userId As Long
    Shared oMatrixContext As CORE_Entities
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
    Shared Function IndicadorCumplimientoTareas(request As IndicadoresCumplimientoTareas_Get_Request)
        Dim datos = analisisIndicadoresRepository.GetIndicadoresCumplimientoTareas(request)
        Return datos
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function Procesos()
        Return analisisIndicadoresRepository.GetCoreUnidades()
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function Tareas(idUnidad As Long)
        Return analisisIndicadoresRepository.GetCoreTareas(idUnidad)
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function TodasTareas()
        Return analisisIndicadoresRepository.GetAllCoreTareas()
    End Function


    <Services.WebMethod()>
    <ScriptMethod()>
    Shared Function CumplimientoTareasAgrupadoExcel(request As IndicadoresCumplimientoTareas_Get_Request) As String
        Try
            Dim datos = analisisIndicadoresRepository.GetIndicadoresCumplimientoTareas(request)
            Dim nombresColumnas As IEnumerable(Of String) = {"Año", "Mes", "Grupo", "Porcentaje", "Cumplidos", "Planeados"}
            Dim excel As New Utilidades.ResponseExcel
            Dim excelBase64 = excel.CreateExcelToBase64("Cumplimiento Tareas", nombresColumnas, datos)
            Return excelBase64
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod()>
    Shared Function CumplimientoTareasDetallesExcel(request As IndicadoresCumplimientoTareasDetalle_Get_Request) As String
        Try
            Dim datos = analisisIndicadoresRepository.GetCumplimientoTareasDetalle(request)

            Dim nombresColumnas As IEnumerable(Of String) = {"Código", "Hilo", "Tarea", "Proyecto", "Unidad", "JobBook", "Nombre Trabajo", "Usuario", "Cumplimiento", "Año", "Mes", "Inicio Planeación", "Inicio Ejecución", "Fin Planeación", "Fin Ejecución"}
            Dim excel As New Utilidades.ResponseExcel
            Dim excelBase64 = excel.CreateExcelToBase64("Cumplimiento Tareas Detalles", nombresColumnas, datos)
            Return excelBase64
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function CumplimientoTareasCOEExcel(request As IndicadoresCumplimientoTareas_COE_Get_Request) As String
        Try
            Dim datos = analisisIndicadoresRepository.GetCumplimientoTareasCOE(request)

            Dim nombresColumnas As IEnumerable(Of String) = {"Año", "Mes", "Grupo", "Cumplidos", "Frecuencia", "Planeados", "Base", "Porcentaje"}
            Dim excel As New Utilidades.ResponseExcel
            Dim excelBase64 = excel.CreateExcelToBase64("Tareas COE", nombresColumnas, datos)
            Return excelBase64

        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

    <Services.WebMethod()>
    <ScriptMethod()>
    Shared Function CumplimientoTareasCOEDetallesExcel(request As IndicadoresCumplimientoTareasDetalle_COE_Get_Request) As String
        Try
            Dim datos = analisisIndicadoresRepository.GetCumplimientoTareasCOEDetalle(request)
            Dim nombresColumnas As IEnumerable(Of String) = {"Código", "Hilo", "Tarea", "Proyecto", "Unidad", "JobBook", "Nombre Trabajo", "Usuario", "Cumplimiento", "Año", "Mes", "Inicio Planeación", "Inicio Ejecución", "Fin Planeación", "Fin Ejecución"}
            Dim excel As New Utilidades.ResponseExcel
            Dim excelBase64 = excel.CreateExcelToBase64("Tareas COE Detalles", nombresColumnas, datos)
            Return excelBase64
        Catch ex As Exception
            HttpContext.Current.Response.StatusCode = HttpStatusCode.InternalServerError
            Return Nothing
        End Try
    End Function

End Class