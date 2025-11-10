Imports System.Configuration
Imports System.Data.SqlClient
Imports Dapper
Imports Ipsos.Apis.SisConn

Namespace PlanillaModeracionDapper

    Public Class SGC_AuditoriasInternasDapper
#Region "Variables Globales"
        Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString

#End Region

        Public Function Add(auditoria As SGC_AuditoriaInterna) As Integer
            Dim params = New With {
                    .AuditorId = auditoria.AuditorId,
                    .AreaAuditada = auditoria.AreaAuditada,
                    .ProcesoAuditado = auditoria.ProcesoAuditado,
                    .FechaLimiteAuditoria = auditoria.FechaLimiteAuditoria,
                    .FechaRegistro = auditoria.FechaRegistro,
                    .TiposAuditoria = String.Join(",", auditoria.TiposAuditoria),
                    .NormativasAAuditar = String.Join(",", auditoria.NormativasAAuditar),
                    .UsuarioRegistraId = auditoria.UsuarioRegistraId
                    }
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.ExecuteScalar(Of Integer)(sql:="SGC_AuditoriasInternas_Add", commandType:=CommandType.StoredProcedure, param:=params)
                Return result
            End Using
        End Function
        Public Function By(estadoId As Short?, auditorId As Long?, anoAuditoria As Integer?, auditadoId As Integer?, pageIndex As Integer, pageSize As Integer) As IList(Of SGC_AuditoriaInternaEntity)
            Dim params = New With {
                    .AuditorId = auditorId,
                    .EstadoId = estadoId,
                    .AnoAuditoria = anoAuditoria,
                    .AuditadoId = auditadoId,
                    .pageSize = pageSize,
                    .pageIndex = pageIndex
                    }
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of SGC_AuditoriaInternaEntity)(sql:="SGC_AI_AuditoriasBy", commandType:=CommandType.StoredProcedure, param:=params)
                Return result
            End Using
        End Function
        Public Function InformeAuditor(auditoriaId As Integer, fechaAuditoria As Date, fortalezas As String, auditados As IEnumerable(Of Long), hallazgos As IEnumerable(Of SGC_AI_Hallazgo), archivoInformeAuditoriaNombre As String, archivoInformeAuditoriaId As String, archivoInformeAuditoriaTamanoBytes As Integer, usuarioRegistra As Long, fechaRegistro As DateTime)
            Dim params = New With {
                    .AuditoriaId = auditoriaId,
                    .FechaAuditoria = fechaAuditoria,
                    .Auditados = String.Join(",", auditados),
                    .Fortalezas = fortalezas,
                    .ArchivoInformeAuditoriaNombre = archivoInformeAuditoriaNombre,
                    .ArchivoInformeAuditoriaId = archivoInformeAuditoriaId,
                    .ArchivoInformeAuditoriaTamanoBytes = archivoInformeAuditoriaTamanoBytes,
                    .Hallazgos = MapHallazgos(hallazgos),
                    .UsuarioRegistra = usuarioRegistra,
                    .FechaRegistro = fechaRegistro
                    }
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Execute(sql:="SGC_AI_AuditoriaInforme_Add", commandType:=CommandType.StoredProcedure, param:=params)
                Return result
            End Using
        End Function
        Public Function InformeAuditorBy(auditoriaId As Integer) As SGC_AI_Auditorias_InformeAuditorEntity
            Dim params = New With {
                    .AuditoriaId = auditoriaId
                    }
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.QueryFirstOrDefault(Of SGC_AI_Auditorias_InformeAuditorEntity)(sql:="SGC_AI_Auditorias_InformeAuditorByAuditoriaId", commandType:=CommandType.StoredProcedure, param:=params)
                Return result
            End Using
        End Function
        Public Function InformeAuditor_HallazgosBy(auditoriaId As Integer) As List(Of SGC_AI_HallazgoResult)
            Dim params = New With {
                    .AuditoriaId = auditoriaId
                    }
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of SGC_AI_HallazgoResult)(sql:="SGC_AI_Auditorias_InformeAuditor_HallazgosByAuditoriaId", commandType:=CommandType.StoredProcedure, param:=params)
                Return result
            End Using
        End Function
        Public Function InformeAuditor_AuditadosBy(auditoriaId As Integer) As List(Of SGC_AI_AuditadoResult)
            Dim params = New With {
                    .AuditoriaId = auditoriaId
                    }
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Dim result = db.Query(Of SGC_AI_AuditadoResult)(sql:="SGC_AI_Auditorias_InformeAuditor_AuditadosByAuditoriaId", commandType:=CommandType.StoredProcedure, param:=params)
                Return result
            End Using
        End Function
        Private Function MapHallazgos(hallazgos As IList(Of SGC_AI_Hallazgo)) As String
            Dim xml As String

            xml = "<Hallazgos>"

            For Each item As SGC_AI_Hallazgo In hallazgos
                xml += "<Hallazgo>"
                xml += "<Hallazgo>"
                xml += item.Hallazgo
                xml += "</Hallazgo>"
                xml += "<TipoId>"
                xml += item.TipoId.ToString()
                xml += "</TipoId>"
                xml += "</Hallazgo>"
            Next

            xml += "</Hallazgos>"

            Return xml

        End Function
        Public Class SGC_AI_Auditorias_InformeAuditorEntity
            Property Id As Integer
            Property SGC_AI_AuditoriaId As Integer
            Property FechaAuditoria As Date
            Property Fortalezas As String
            Property ArchivoInformeAuditoriaNombre As String
            Property ArchivoInformeAuditoriaId As String
            Property ArchivoInformeAuditoriaTamanoBytes As Integer
            Property FechaRegistro As DateTime
            Property UsuarioRegistra As Long

        End Class
        Public Class SGC_AI_AuditadoResult
            Property Id As Integer
            Property AuditadoId As Integer
            Property SGC_AI_AuditoriaId As Integer
            Property Nombres As String
            Property Apellidos As String
            Property activo As Boolean

        End Class
        Public Class SGC_AI_HallazgoResult
            Property Id As Integer
            Property SGC_AI_AuditoriaId As Integer
            Property Hallazgo As String
            Property SGC_AI_TipoHallazgoId As Integer
            Property TipoHallazgo As String
        End Class
        Public Class SGC_AI_Hallazgo
            Property TipoId As Short
            Property Hallazgo As String
        End Class
        Public Class SGC_AuditoriaInterna
            Property AuditorId As Long
            Property AreaAuditada As String
            Property ProcesoAuditado As String
            Property FechaLimiteAuditoria As Date
            Property FechaRegistro As DateTime
            Property UsuarioRegistraId As Long
            Property NormativasAAuditar As List(Of Short)
            Property TiposAuditoria As List(Of Short)
        End Class
        Public Class SGC_AuditoriaInternaEntity
            Property Id As Integer
            Property AuditorId As Long
            Property AreaAuditada As String
            Property ProcesoAuditado As String
            Property FechaLimiteAuditoria As Date
            Property FechaRegistro As DateTime
            Property UsuarioRegistraId As Long
            Property SGC_AI_EstadoId As Short
            Property SGC_AI_EstadoAuditoria As String
            Property RowNum As Integer
            Property TotalRows As Integer
            Property SGC_NormativasAAuditar As String
            Property SGC_AI_Tipos As String
            Property NombreAuditor As String
            Public ReadOnly Property NormativasAuditar As List(Of SGC_NormativaEntity)
                Get
                    If (String.IsNullOrEmpty(SGC_NormativasAAuditar)) Then
                        Return Enumerable.Empty(Of SGC_NormativaEntity).ToList()
                    End If

                    Dim normativas = New List(Of SGC_NormativaEntity)
                    Dim vNormas = SGC_NormativasAAuditar.Split("|")


                    For Each item As String In vNormas
                        Dim vNorma = item.Split(";")
                        normativas.Add(
                            New SGC_NormativaEntity With {
                            .Id = vNorma(0),
                            .Estandar = vNorma(1)
                            })
                    Next

                    Return normativas
                End Get
            End Property
            Public ReadOnly Property TiposAuditoria As List(Of SGC_AI_TipoEntity)
                Get
                    If (String.IsNullOrEmpty(SGC_AI_Tipos)) Then
                        Return Enumerable.Empty(Of SGC_AI_TipoEntity).ToList()
                    End If

                    Dim tipos = New List(Of SGC_AI_TipoEntity)
                    Dim vTipos = SGC_AI_Tipos.Split("|")

                    For Each item As String In vTipos
                        Dim vTipo = item.Split(";")
                        tipos.Add(
                            New SGC_AI_TipoEntity With {
                            .Id = vTipo(0),
                            .TipoAuditoria = vTipo(1)
                            })
                    Next

                    Return tipos
                End Get
            End Property
        End Class
        Public Class SGC_NormativaEntity
            Property Id As Short
            Property Estandar As String
        End Class
        Public Class SGC_AI_TipoEntity
            Property Id As Short
            Property TipoAuditoria As String
        End Class
    End Class
End Namespace
