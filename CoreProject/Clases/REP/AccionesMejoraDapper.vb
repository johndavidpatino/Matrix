Imports System.Configuration
Imports System.Data.SqlClient
Imports Dapper

Public Class AccionesMejoraDapper
#Region "Variables Globales"
    Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString

#End Region
    Public Function AddAccionMejora(params As AccionesMejora_Add_Request) As Result(Of Integer)
        Dim clientResult As New Result(Of Integer)(False, "", 0)

        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Try
                Dim result = db.Query(Of Integer)(
                    sql:="ACM_AccionMejora_Add",
                    param:=params,
                    commandType:=CommandType.StoredProcedure
                )

                clientResult.Message = "Accion de mejora agregada"
                clientResult.Success = True
                clientResult.Data = result.FirstOrDefault()
            Catch ex As Exception
                clientResult.Message = ex.Message
                clientResult.Success = False
            End Try
            Return clientResult
        End Using
    End Function

    Public Function UpdateAccionMejora(params As AccionesMejora_Edit_Request) As Result
        Dim clientResult As New Result

        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Try
                db.Execute(
                    sql:="ACM_AccionesMejora_Edit",
                    param:=params,
                    commandType:=CommandType.StoredProcedure
                )
                clientResult.Message = "Accion de mejora actualizada"
                clientResult.Success = True
            Catch ex As Exception
                clientResult.Message = ex.Message
                clientResult.Success = False
            End Try
        End Using
        Return clientResult
    End Function

    Public Function DeleteAccionMejora(AccionMejoraId As Integer) As Result
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "UPDATE ACM_AccionesMejora SET IsDeleted = 1 WHERE AccionMejoraId = @AccionMejoraId"
            Try
                db.Execute(sql, param:=New With {.AccionMejoraId = AccionMejoraId})
                Return New Result(True, "Accion de mejora eliminada")
            Catch ex As Exception
                Return New Result(False, ex.Message)
            End Try
        End Using
    End Function
    Public Function GetTiposFuente() As Result(Of List(Of TiposFuente_Get_Response))
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "SELECT TipoFuenteId, TipoFuenteNombre FROM ACM_TiposFuente"
            Try
                Dim result = db.Query(Of TiposFuente_Get_Response)(sql).ToList()
                Return New Result(Of List(Of TiposFuente_Get_Response))(True, "Tipos de fuente obtenidos", result)
            Catch ex As Exception
                Return New Result(Of List(Of TiposFuente_Get_Response))(False, ex.Message, New List(Of TiposFuente_Get_Response))
            End Try
        End Using
    End Function

    Public Function GetAccionMejoraProcesos() As Result(Of List(Of AccionMejoraProcesos_Get_Response))
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "SELECT ProcesoId, NombreProceso FROM ACM_Procesos"
            Try
                Dim result = db.Query(Of AccionMejoraProcesos_Get_Response)(sql).ToList()
                Return New Result(Of List(Of AccionMejoraProcesos_Get_Response))(True, "Procesos obtenidos", result)
            Catch ex As Exception
                Return New Result(Of List(Of AccionMejoraProcesos_Get_Response))(False, ex.Message, New List(Of AccionMejoraProcesos_Get_Response))
            End Try
        End Using
    End Function

    Public Function GetUsuarios() As Result(Of List(Of AccionesMejoraUsuario_Get_Response))
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "
                SELECT 
                    US.id As UsuarioId, 
                    Usuario, 
                    PE.Nombres + ' ' + PE.Apellidos As Nombre,
                    CA.id CargoId,
                    CA.Cargo Cargo,
	                REPLACE(PE.urlFoto,'..','') Avatar
                FROM US_Usuarios US
                LEFT JOIN TH_Personas PE ON PE.id = US.id
                LEFT JOIN TH_Cargos CA ON CA.id = PE.Cargo
                WHERE US.Activo = 1 AND PE.Activo = 1
            "
            Try
                Dim result = db.Query(Of AccionesMejoraUsuario_Get_Response)(sql).ToList()
                Return New Result(Of List(Of AccionesMejoraUsuario_Get_Response))(True, "Usuarios obtenidos", result)
            Catch ex As Exception
                Return New Result(Of List(Of AccionesMejoraUsuario_Get_Response))(False, ex.Message, New List(Of AccionesMejoraUsuario_Get_Response))
            End Try
        End Using
    End Function

    Public Function AddCausas(accionMejoraId As Integer, causas As List(Of Causas_Add_Request)) As Result
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "INSERT INTO ACM_Causas (AccionMejoraId, DescripcionCausa) VALUES (@AccionMejoraId, @DescripcionCausa)"
            Try
                For Each causa In causas
                    If causa.AccionMejoraId = 0 Then causa.AccionMejoraId = accionMejoraId
                Next
                db.Execute(sql, param:=causas)
                Return New Result(True, "Causas agregadas")
            Catch ex As Exception
                Return New Result(False, ex.Message)
            End Try
        End Using
    End Function

    Public Function AddFuente(params As AccionesMejora_Fuentes_Add_Request) As Result
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "INSERT INTO ACM_AccionesMejora_Fuentes (AccionMejoraId, TipoFuenteId, FuenteId) VALUES (@AccionMejoraId, @TipoFuenteId, @FuenteId)"
            Try
                db.Execute(sql, param:=params)
                Return New Result(True, "Fuente agregada")
            Catch ex As Exception
                Return New Result(False, ex.Message)
            End Try
        End Using
    End Function

    Public Function GetAccionesMejora(params As AccionesMejora_Get_Request) As Result(Of List(Of AccionesMejora_Get_Response))
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "
            SELECT
                AM.AccionMejoraId,
                AM.DescripcionAccion,
                CONVERT(varchar(250),AM.FechaIncidente) FechaIncidente,
                AM.UsuarioReporta,
                AM.ProcesoId,
                AM.UsuarioResponsable,
                AM.Descripcion,
                AM.Correccion,
                AF.TipoFuenteId,
                AF.FuenteId
            FROM ACM_AccionesMejora AM
            LEFT JOIN ACM_AccionesMejora_Fuentes AF ON AM.AccionMejoraId = AF.AccionMejoraId
            WHERE 
                (@AccionMejoraId IS NULL OR AM.AccionMejoraId = @AccionMejoraId)
                AND (@FechaIncidente IS NULL OR FechaIncidente = @FechaIncidente)
                AND (@UsuarioReporta IS NULL OR UsuarioReporta = @UsuarioReporta)
                AND (@ProcesoId IS NULL OR ProcesoId = @ProcesoId)
                AND (@UsuarioResponsable IS NULL OR UsuarioResponsable = @UsuarioResponsable)
                AND AM.IsDeleted = 0
            "
            Try
                Dim result = db.Query(Of AccionesMejora_Get_Response)(sql, param:=params).ToList()
                Return New Result(Of List(Of AccionesMejora_Get_Response))(True, "Acciones de mejora obtenidas", result)
            Catch ex As Exception
                Return New Result(Of List(Of AccionesMejora_Get_Response))(False, ex.Message, New List(Of AccionesMejora_Get_Response))
            End Try
        End Using
    End Function

    Public Function AddPlanesAccion(accionMejoraId As Integer, planes As List(Of PlanesAccion_Add_Request)) As Result
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "INSERT INTO ACM_PlanesAccion (AccionMejoraId, DescripcionPlan, FechaPlaneado, FechaEjecutado, EficaciaPlan, FechaRevision) 
            VALUES (@AccionMejoraId, @DescripcionPlan, @FechaPlaneado, @FechaEjecutado, @EficaciaPlan, @FechaRevision)"
            Try
                For Each plan In planes
                    If plan.AccionMejoraId = 0 Then plan.AccionMejoraId = accionMejoraId
                Next
                db.Execute(sql, param:=planes)
                Return New Result(True, "Planes de accion agregados")
            Catch ex As Exception
                Return New Result(False, ex.Message)
            End Try
        End Using
    End Function

    Public Function GetCausasByAccionMejoraId(accionMejoraId As Integer) As Result(Of List(Of Causas_Get_Response))
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "SELECT CausaId, AccionMejoraId, DescripcionCausa FROM ACM_Causas WHERE AccionMejoraId = @AccionMejoraId"
            Try
                Dim result = db.Query(Of Causas_Get_Response)(sql, param:=New With {.AccionMejoraId = accionMejoraId}).ToList()
                Return New Result(Of List(Of Causas_Get_Response))(True, "Causas obtenidas", result)
            Catch ex As Exception
                Return New Result(Of List(Of Causas_Get_Response))(False, ex.Message, New List(Of Causas_Get_Response))
            End Try
        End Using
    End Function

    Public Function GetPlanesAccionByAccionMejoraId(accionMejoraId As Integer) As Result(Of List(Of PlanesAccion_Get_Response))
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "SELECT PlanAccionId, AccionMejoraId, DescripcionPlan, CONVERT(varchar(250),FechaPlaneado) FechaPlaneado, CONVERT(varchar(250),FechaEjecutado) FechaEjecutado, EficaciaPlan, CONVERT(varchar(250),FechaRevision) FechaRevision FROM ACM_PlanesAccion WHERE AccionMejoraId = @AccionMejoraId"
            Try
                Dim result = db.Query(Of PlanesAccion_Get_Response)(sql, param:=New With {.AccionMejoraId = accionMejoraId}).ToList()
                Return New Result(Of List(Of PlanesAccion_Get_Response))(True, "Planes de accion obtenidos", result)
            Catch ex As Exception
                Return New Result(Of List(Of PlanesAccion_Get_Response))(False, ex.Message, New List(Of PlanesAccion_Get_Response))
            End Try
        End Using
    End Function
    Public Function EditCausas(causas As List(Of Causas_Edit_Request)) As Result
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "UPDATE ACM_Causas SET DescripcionCausa = @DescripcionCausa WHERE CausaId = @CausaId"
            Try
                db.Execute(sql, param:=causas)
                Return New Result(True, "Causas actualizadas")
            Catch ex As Exception
                Return New Result(False, ex.Message)
            End Try
        End Using
    End Function

    Public Function EditPlanesAccion(planes As List(Of PlanesAccion_Edit_Request)) As Result
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "UPDATE ACM_PlanesAccion SET DescripcionPlan = @DescripcionPlan, FechaPlaneado = @FechaPlaneado, FechaEjecutado = @FechaEjecutado, EficaciaPlan = @EficaciaPlan, FechaRevision = @FechaRevision WHERE PlanAccionId = @PlanAccionId"
            Try
                db.Execute(sql, param:=planes)
                Return New Result(True, "Planes de accion actualizados")
            Catch ex As Exception
                Return New Result(False, ex.Message)
            End Try
        End Using
    End Function

    Public Function DeleteCausas(causas As List(Of Integer)) As Result
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "DELETE FROM ACM_Causas WHERE CausaId = @CausaId"
            Try
                Dim causasToDelete = causas.Select(Function(x) New With {.CausaId = x}).ToList()
                db.Execute(sql, param:=causasToDelete)
                Return New Result(True, "Causas eliminadas")
            Catch ex As Exception
                Return New Result(False, ex.Message)
            End Try
        End Using
    End Function

    Public Function DeletePlanesAccion(planes As List(Of Integer)) As Result
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "DELETE FROM ACM_PlanesAccion WHERE PlanAccionId = @PlanAccionId"
            Try
                Dim planesToDelete = planes.Select(Function(x) New With {.PlanAccionId = x}).ToList()
                db.Execute(sql, param:=planesToDelete)
                Return New Result(True, "Planes de accion eliminados")
            Catch ex As Exception
                Return New Result(False, ex.Message)
            End Try
        End Using
    End Function

    Public Function GetAuditoriasInterna() As Result(Of List( Of AuditoriaInterna_Get_Response))
        Using db As IDbConnection = New SqlConnection(sqlConnectionString)
            Dim sql = "
            SELECT
                Id,
                AuditorId,
                AreaAuditada,
                ProcesoAuditado,
                CONVERT(varchar(250),FechaLimiteAuditoria) FechaLimiteAuditoria,
                CONVERT(varchar(250),FechaRegistro) FechaRegistro,
                UsuarioRegistraId,
                SGC_AI_EstadoId
            FROM SGC_AI_Auditorias"
            Try
                Dim result = db.Query(Of AuditoriaInterna_Get_Response)(sql).ToList()
                Return New Result(Of List(Of AuditoriaInterna_Get_Response))(True, "Auditorias internas obtenidas", result)
            Catch ex As Exception
                Return New Result(Of List(Of AuditoriaInterna_Get_Response))(False, ex.Message, New List(Of AuditoriaInterna_Get_Response))
            End Try
        End Using
    End Function
End Class

Public Class AccionesMejora_Add_Request
    Property DescripcionAccion As String
    Property FechaIncidente As DateTimeOffset
    Property UsuarioReporta As Long
    Property ProcesoId As Integer
    Property UsuarioResponsable As Long
    Property Descripcion As String
    Property Correccion As String
    Property FuenteNoConformidadId As Integer

    Property FuenteId As Integer?
End Class

Public Class AccionesMejora_Edit_Request
    Property AccionMejoraId As Integer
    Property DescripcionAccion As String = Nothing
    Property FechaIncidente As DateTimeOffset?
    Property UsuarioReporta As Long?
    Property ProcesoId As Integer?
    Property UsuarioResponsable As Long?
    Property Descripcion As String = Nothing
    Property Correccion As String = Nothing
    Property FuenteNoConformidadId As Integer?
End Class
Public Class AccionesMejora_Get_Request
    Property AccionMejoraId As Integer?
    Property FechaIncidente As DateTimeOffset?
    Property UsuarioReporta As Long?
    Property ProcesoId As Integer?
    Property UsuarioResponsable As Long?
End Class
Public Class AccionesMejora_Get_Response
    Property AccionMejoraId As Integer
    Property DescripcionAccion As String
    Property FechaIncidente As String
    Property UsuarioReporta As Long
    Property ProcesoId As Integer
    Property UsuarioResponsable As Long
    Property Descripcion As String
    Property Correccion As String
    Property TipoFuenteId As Integer
    Property FuenteId As Integer
End Class
Public Class Causas_Add_Request
    Property AccionMejoraId As Integer
    Property DescripcionCausa As String
End Class

Public Class Causas_Edit_Request
    Property CausaId As Integer?
    Property DescripcionCausa As String
End Class

Public Class Causas_Get_Response
    Property CausaId As Integer
    Property AccionMejoraId As Integer
    Property DescripcionCausa As String
End Class

Public Class TiposFuente_Get_Response
    Property TipoFuenteId As Integer
    Property TipoFuenteNombre As String
End Class

Public Class AccionMejoraProcesos_Get_Response
    Property ProcesoId As Integer
    Property NombreProceso As String
End Class


Public Class AccionesMejoraUsuario_Get_Response
    Property UsuarioId As Integer
    Property Usuario As String
    Property Nombre As String
    Property CargoId As Integer
    Property Cargo As String
    Property Avatar As String
End Class

Public Class AccionesMejora_Fuentes_Add_Request
    Property AccionMejoraId As Integer
    Property TipoFuenteId As Integer
    Property FuenteId As Integer?
End Class

Public Class AccionesMejora_Fuentes_Update_Request
    Property AccionMejoraId As Integer
    Property TipoFuenteId As Integer?
    Property FuenteId As Integer?
End Class

Public Class PlanesAccion_Add_Request
    Property AccionMejoraId As Integer
    Property DescripcionPlan As String
    Property FechaPlaneado As DateTimeOffset
    Property FechaEjecutado As DateTimeOffset?
    Property EficaciaPlan As String = Nothing
    Property FechaRevision As DateTimeOffset?
End Class

Public Class PlanesAccion_Get_Response
    Property PlanAccionId As Integer
    Property AccionMejoraId As Integer
    Property DescripcionPlan As String
    Property FechaPlaneado As String
    Property FechaEjecutado As String
    Property EficaciaPlan As String
    Property FechaRevision As String
End Class

Public Class PlanesAccion_Edit_Request
    Property PlanAccionId As Integer?
    Property DescripcionPlan As String
    Property FechaPlaneado As DateTimeOffset
    Property FechaEjecutado As DateTimeOffset?
    Property EficaciaPlan As String = Nothing
    Property FechaRevision As DateTimeOffset?
End Class

Public Class AuditoriaInterna_Get_Response
    Property Id As Integer
    Property AuditorId As Integer
    Property AreaAuditada As String
    Property ProcesoAuditado As String
    Property FechaLimiteAuditoria As String
    Property FechaRegistro As String
    Property UsuarioRegistraId As Integer
    Property SGC_AI_EstadoId As Integer
End Class