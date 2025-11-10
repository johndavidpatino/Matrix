Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data.SqlTypes
Imports Dapper

Public Class AnalisisIndicadoresDapper
#Region "Variables Globales"
	Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString

#End Region
	Public Function AddAnalisis(params As AnalisisInicadores_Add_Request) As Result
		Dim clientResult As New Result
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)

			Dim trimestre = Nothing

			If params.Trimestre <> Nothing Then
				trimestre = params.Trimestre
			End If


			Dim existsRequet As New AnalisisIndicadores_Get_Request With {
				.IdInstrumento = params.IdInstrumento,
				.Indicador = params.Indicador,
				.UsuarioRegistra = params.UsuarioRegistra,
				.IdProceso = params.IdProceso,
				.Periodo = params.Periodo,
				.IdUnidad = params.IdUnidad,
				.IdTarea = params.IdTarea,
				.Trimestre = trimestre
			}

			Dim existResult = GetAnalisis(existsRequet)

			If existResult.Items.Count > 0 Then
				clientResult.Message = "Ya existe un análisis con los mismos parámetros."
				clientResult.Success = False
				Return clientResult
			End If

			Dim result = db.Query(Of String)(
				sql:="REP_AnalisisIndicadores_Add",
				param:=params,
				commandType:=CommandType.StoredProcedure
				)

			clientResult.Message = "Análisis registrado exitosamente."
			clientResult.Success = True

			Return clientResult
		End Using
	End Function

	Public Function GetAnalisis(params As AnalisisIndicadores_Get_Request) As PagedResult(Of AnalisisIndicadores_Get_Result)
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.QueryMultiple(
				sql:="REP_AnalisisIndicadores_Get",
				param:=params,
				commandType:=CommandType.StoredProcedure
				)

			Dim items = result.Read(Of AnalisisIndicadores_Get_Result).ToList()

			Dim totalCount = result.ReadFirst(Of Integer)()

			Dim pagedData As New PagedResult(Of AnalisisIndicadores_Get_Result)(items, params.PageNumber, params.PageSize, totalCount)

			Return pagedData
		End Using
	End Function

	Public Function DeleteAnalisis(idAnalisis As Long) As Result
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)

			Dim result As New Result

			Try
				Dim sql2 = "DELETE FROM REP_AnalisisIndicadoresFiltros WHERE IdAnalisis = @IdAnalisis"
				Dim sql = "DELETE FROM REP_AnalisisIndicadores WHERE IdAnalisis = @IdAnalisis"
				db.Execute(sql2, New With {.IdAnalisis = idAnalisis})
				db.Execute(sql, New With {.IdAnalisis = idAnalisis})
				result.Message = "Análisis eliminado exitosamente."
				result.Success = True
			Catch ex As Exception
				result.Message = ex.Message
				result.Success = False
			End Try
			Return result
		End Using
	End Function

	Public Function GetCoreUnidades() As List(Of CoreUnidad_Get_Result)
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim sql = "SELECT id, Unidad, MultiUnidad FROM CORE_Unidades"
			Dim result = db.Query(Of CoreUnidad_Get_Result)(sql)
			Return result.ToList()
		End Using
	End Function

	Public Function GetCoreTareas(idUnidad As Integer) As List(Of CORE_TareasXUnidadEjecuta_Result)
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of CORE_TareasXUnidadEjecuta_Result)(
				sql:="CORE_TareasXUnidadEjecuta",
				param:=New With {.unidadEjecuta = idUnidad},
				commandType:=CommandType.StoredProcedure
				)
			Return result.ToList()
		End Using
	End Function

	Public Function GetAllCoreTareas() As List(Of Tareas_Get_Result)
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim sql = "SELECT id,Tarea FROM CORE_Tareas"

			Dim result = db.Query(Of Tareas_Get_Result)(sql)

			Return result.ToList()
		End Using
	End Function

	Public Function GetIndicadoresCumplimientoTareas(params As IndicadoresCumplimientoTareas_Get_Request) As List(Of REP_IndicadoresCumplimientoTareas_Result)
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of REP_IndicadoresCumplimientoTareas_Result)(
				sql:="REP_IndicadoresCumplimientoTareas",
				param:=params,
				commandType:=CommandType.StoredProcedure
				)
			Return result.ToList()
		End Using
	End Function

	Public Function UpdateAnalisisContent(idAnalisis As Long, content As String) As Result
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim sql = "UPDATE REP_AnalisisIndicadores SET Contenido = @Content WHERE IdAnalisis = @IdAnalisis"
			db.Execute(sql, New With {.Content = content, .IdAnalisis = idAnalisis})
			Return New Result With {.Message = "Análisis actualizado exitosamente.", .Success = True}
		End Using
	End Function

	Public Function GetRegistroObservacionDetalle(params As IndicadoresRegistroObservacionesDetalle_Get_Request) As List(Of REP_ErroresRegistroObservacionesDetalle_Result)
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of REP_ErroresRegistroObservacionesDetalle_Result)(
			sql:="REP_ErroresRegistroObservacionesDetalle",
			param:=params,
			commandType:=CommandType.StoredProcedure
			)
			Return result.ToList()
		End Using
	End Function

	Public Function GetCumplimientoTareasDetalle(params As IndicadoresCumplimientoTareasDetalle_Get_Request) As List(Of IndicadoresCumplimientoTareasDetalle_Get_Result)
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of IndicadoresCumplimientoTareasDetalle_Get_Result)(
			sql:="REP_IndicadoresCumplimientoTareasDetalle",
			param:=params,
			commandType:=CommandType.StoredProcedure
			)
			Return result.ToList()
		End Using
	End Function

	Public Function GetCumplimientoTareasCOE(params As IndicadoresCumplimientoTareas_COE_Get_Request) As List(Of IndicadoresCumplimientoTareas_COE_Get_Result)
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of IndicadoresCumplimientoTareas_COE_Get_Result)(
			sql:="REP_IndicadoresCumplimientoTareas_COE",
			param:=params,
			commandType:=CommandType.StoredProcedure
			)
			Return result.ToList()
		End Using
	End Function

	Public Function GetCumplimientoTareasCOEDetalle(params As IndicadoresCumplimientoTareasDetalle_COE_Get_Request) As List(Of IndicadoresCumplimientoTareasDetalle_COE_Get_Result)
		Using db As IDbConnection = New SqlConnection(sqlConnectionString)
			Dim result = db.Query(Of IndicadoresCumplimientoTareasDetalle_COE_Get_Result)(
			sql:="REP_IndicadoresCumplimientoTareasDetalle_COE",
			param:=params,
			commandType:=CommandType.StoredProcedure
			)
			Return result.ToList()
		End Using
	End Function
End Class

Public Class AnalisisInicadores_Add_Request
	Property UsuarioRegistra As Long
	Property FechaRegistro As DateTimeOffset
	Property Contenido As String
	Property Periodo As Integer
	Property Trimestre As String
	Property Indicador As String
	Property AgruparPor As String
	Property IdInstrumento As Integer?
	Property IdTarea As Integer?

	Property IdProceso As Integer?
	Property IdUnidad As Integer?
End Class

Public Class AnalisisIndicadores_Get_Request
	Property Indicador As String
	Property IdUnidad As Integer?
	Property Periodo As Integer?
	Property UsuarioRegistra As Integer?
	Property IdTarea As Integer?
	Property IdInstrumento As Integer?
	Property IdProceso As Integer?
	Property Trimestre As String = Nothing

	Property PageNumber As Integer = 1
	Property PageSize As Integer = 20
End Class

Public Class AnalisisIndicadores_Get_Result
	Property IdAnalisis As Long
	Property UsuarioRegistra As String
	Property FechaRegistro As String
	Property Contenido As String
	Property Periodo As Integer
	Property Trimestre As String
	Property Indicador As String
	Property IdUnidad As Integer
	Property IdTarea As Integer?
	Property IdInstrumento As Integer?
	Property IdProceso As Integer?
End Class

Public Class Result
	Public Property Success As Boolean
	Public Property Message As String

	Public Sub New()
		Me.Success = False
		Me.Message = String.Empty
	End Sub

	Public Sub New(success As Boolean, message As String)
		Me.Success = success
		Me.Message = message
	End Sub

End Class

Public Class Result(Of T)
	Inherits Result
	Public Property Data As T
	Public Sub New(success As Boolean, message As String, data As T)
		Me.Success = success
		Me.Message = message
		Me.Data = data
	End Sub
End Class
Public Class CoreUnidad_Get_Result
	Property id As Integer
	Property Unidad As String
	Property MultiUnidad As Short
End Class


Public Class IndicadoresCumplimientoTareas_Get_Request
	Property mes As Short?
	Property ano As Short
	Property agruparPor As String
	Property idTarea As Short
	Property proceso As Short
	Property grupoUnidad As Short?
End Class

Public Class IndicadoresRegistroObservacionesDetalle_Get_Request
	Property mes As Short?
	Property ano As Short
	Property agruparPor As String
	Property idTarea As Short
	Property idInstrumento As Short
	Property usuario As String
End Class
Public Class Tareas_Get_Result
	Property id As Integer
	Property Tarea As String
End Class

Public Class IndicadoresCumplimientoTareasDetalle_Get_Request
	Property mes As Short?
	Property ano As Short
	Property agruparPor As String
	Property idTarea As Short?
	Property proceso As Short?
	Property grupoUnidad As Short?
	Property cumple As Short = Nothing
	Property usuario As String = Nothing
End Class

Public Class IndicadoresCumplimientoTareasDetalle_Get_Result
   Public Property id As Long
   Public Property HiloId As Nullable(Of Long)
   Public Property Tarea As String
   Public Property ProyectoId As Nullable(Of Long)
   Public Property Unidad As String
   Public Property JobBook As String
   Public Property NombreTrabajo As String
   Public Property Usuario As String
   Public Property Cumplimiento As String
   Public Property Ano As Nullable(Of Integer)
   Public Property Mes As Nullable(Of Integer)
   Public Property FIniP As Nullable(Of Date)
   Public Property FIniR As Nullable(Of Date)
   Public Property FFinP As Nullable(Of Date)
   Public Property FFinR As Nullable(Of Date)
End Class

Public Class IndicadoresCumplimientoTareas_COE_Get_Request
	Property mes As Short?
	Property ano As Short
	Property agruparPor As String
	Property idTarea As Short?
	Property proceso As Short?
	Property grupoUnidad As Short?
End Class

Public Class IndicadoresCumplimientoTareas_COE_Get_Result
    Public Property Ano As Nullable(Of Short)
    Public Property Mes As String
    Public Property Grupo As String
    Public Property Cumplidos As Nullable(Of Short)
    Public Property Frecuencia As Nullable(Of Short)
    Public Property Planeados As Nullable(Of Short)
    Public Property Base As Nullable(Of Short)
    Public Property Porcentaje As Nullable(Of Byte)
End Class

Public Class IndicadoresCumplimientoTareasDetalle_COE_Get_Request
	Property mes As Short?
	Property ano As Short
	Property agruparPor As String
	Property idTarea As Short?
	Property proceso As Short?
	Property grupoUnidad As Short?
	Property cumple As Short?
	Property usuario As String = Nothing
End Class

Public Class IndicadoresCumplimientoTareasDetalle_COE_Get_Result
    Public Property Ano As Nullable(Of Integer)
    Public Property Mes As Nullable(Of Integer)
    Public Property id As Long
    Public Property HiloId As Nullable(Of Long)
    Public Property Tarea As String
    Public Property ProyectoId As Nullable(Of Long)
    Public Property Unidad As String
    Public Property JobBook As String
    Public Property NombreTrabajo As String
    Public Property Usuario As String
    Public Property Cumplimiento As String
    Public Property FIniP As Nullable(Of Date)
    Public Property FIniR As Nullable(Of Date)
    Public Property FFinP As Nullable(Of Date)
    Public Property FFinR As Nullable(Of Date)
End Class

Public Class PagedResult(Of T)
	Property Items As List(Of T)
	Property PageNumber As Integer
	Property PageSize As Integer
	Property TotalCount As Integer

	Property NextPage As Integer?
	Property PreviousPage As Integer?

	Public Sub New(data As List(Of T), page As Integer, size As Integer, count As Integer)
		Items = data
		PageNumber = page
		PageSize = size
		TotalCount = count
		NextPage = Nothing
		PreviousPage = Nothing
		If (page * PageSize) < TotalCount Then
			NextPage = page + 1
		End If
		If page > 1 Then
			PreviousPage = page - 1
		End If
	End Sub
End Class