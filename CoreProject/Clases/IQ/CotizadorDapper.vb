Imports Dapper
Imports System.Configuration
Imports System.Data
Imports System.Data.SqlClient

Namespace Cotizador

    Public Class GeneralDapper
#Region "Variables Globales"
        Dim sqlConnectionString = ConfigurationManager.ConnectionStrings("MatrixConnectionString").ToString
#End Region

        Public Sub PUT_EjecutarComando(ByVal sqlcommand As String)
            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                db.Execute(sqlcommand)
            End Using
        End Sub

        Public Function GetProcesos(ByVal idTrabajo As Int64) As List(Of Class_ProcesosIQuote)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Return db.Query(Of Class_ProcesosIQuote)("select IdFase, PP.ProcCodigo, DescFase + ' - ' + MET.MetNombre DescFase, ProcDescripcion from IQ_ProcesosPresupuesto PP inner join IQ_Procesos P on P.ProcCodigo=pp.ProcCodigo 
	                    inner join IQ_Fases F on F.IdFase=PP.ParNacional
                        inner join OP_Metodologias MET on MET.MetCodigo=PP.MetCodigo
                        inner join PY_Trabajo T on T.IdPropuesta=PP.IdPropuesta and T.Alternativa=PP.ParAlternativa
                    where T.id=" & idTrabajo.ToString & "
                    ORDER BY PP.ParNacional, PP.MetCodigo, PP.ProcCodigo").ToList()
            End Using
        End Function

        Public Function GetNoProcesos(ByVal idTrabajo As Int64) As List(Of Class_NoProcesosIQuote)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Return db.Query(Of Class_NoProcesosIQuote)("select IdFase, DescFase, PP.MetCodigo, MET.MetNombre Metodologia, ParNProcesosDC, ParNProcesosTopLines, ParNProcesosTablas, ParNProcesosBases, DPUnificacion, DPTransformacion
	                    from IQ_Parametros PP inner join IQ_Fases F on F.IdFase=PP.ParNacional
                        inner join OP_Metodologias MET on MET.MetCodigo=PP.MetCodigo
                        inner join PY_Trabajo T on T.IdPropuesta=PP.IdPropuesta and T.Alternativa=PP.ParAlternativa
                    where T.id=" & idTrabajo.ToString & "
                    ORDER BY PP.ParNacional, PP.MetCodigo").ToList()
            End Using
        End Function

        Public Function GetProcesosEstadistica(ByVal idTrabajo As Int64) As List(Of Class_ProcesosEstadistica)

            Using db As IDbConnection = New SqlConnection(sqlConnectionString)
                Return db.Query(Of Class_ProcesosEstadistica)("select F.IdFase, F.DescFase, MET.MetNombre Metodologia, TE.Categoria, TE.AnalisisServicio, PP.Cantidad from IQ_AnalisisEstadisticaPresupuesto PP
	                    inner join IQ_TarifasEstadistica TE ON TE.IdAnalisis=PP.IdAnalisis
                        inner join IQ_Fases F on F.IdFase=PP.ParNacional
                        inner join OP_Metodologias MET on MET.MetCodigo=PP.MetCodigo
                        inner join PY_Trabajo T on T.IdPropuesta=PP.IdPropuesta and T.Alternativa=PP.ParAlternativa
                    where T.id=" & idTrabajo.ToString & "
                    ORDER BY PP.ParNacional, PP.MetCodigo, TE.Orden").ToList()
            End Using
        End Function

        Public Class Class_ProcesosIQuote
            Property idFase As Integer
            Property ProcCodigo As Integer
            Property DescFase As String
            Property ProcDescripcion As String

        End Class


        Public Class Class_NoProcesosIQuote
            Property idFase As Integer
            Property DescFase As String
            Property MetCodigo As Integer
            Property Metodologia As String
            Property ParNProcesosDC As Integer
            Property ParNProcesosTopLines As Integer
            Property ParNProcesosTablas As Integer
            Property ParNProcesosBases As Integer
            Property DPUnificacion As Boolean
            Property DPTransformacion As Boolean

        End Class

        Public Class Class_ProcesosEstadistica
            Property idFase As Integer
            Property DescFase As String
            Property Metodologia As String
            Property Categoria As String
            Property AnalisisServicio As String
            Property Cantidad As Integer

        End Class
    End Class

End Namespace
