Public Class Campo
#Region "Variables globales"
    Private oMatrixContext As RP_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New RP_Entities
    End Sub
#End Region
#Region "Obtener"
    Function obtenerReporteEjecucionCampo(ByVal fechaInicio As Date, ByVal fechaFin As Date, ByVal codigosGerencias As String) As Data.DataTable
        Dim pFechaInicio As New SqlClient.SqlParameter("@FechaIni", fechaInicio)
        Dim pFechaFin As New SqlClient.SqlParameter("@FechaFin", fechaFin)
        Dim pCodGerencias As New SqlClient.SqlParameter("@codGerencias", If(codigosGerencias.Trim = "", System.DBNull.Value, codigosGerencias.Trim))
        pCodGerencias.SqlDbType = SqlDbType.VarChar
        Dim parametros() As SqlClient.SqlParameter = New SqlClient.SqlParameter() {pFechaInicio, pFechaFin, pCodGerencias}
        Dim dt As New DataTable

        Using con = oMatrixContext.Database.Connection
            con.Open()
            Using cmd = con.CreateCommand
                cmd.CommandText = "REP_AsignacionEjecucionCampo"
                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddRange(parametros)
                Using reader = cmd.ExecuteReader
                    dt.Load(reader)
                End Using
            End Using
        End Using

        Return dt
    End Function

    Sub LlenadoNewTable()
        oMatrixContext.Llenado_NEWTABLE()
    End Sub
    Function obtenerReporteTabletsAgrupado(ByVal fechaInicio As Date, ByVal fechaFin As Date) As List(Of REP_InformeTablets_Agrupado_Result)
        oMatrixContext.Database.CommandTimeout = 60
        Return oMatrixContext.REP_InformeTablets_Agrupado(fechaInicio, fechaFin).ToList
    End Function

    Function obtenerReporteTabletsDiario(ByVal fechaInicio As Date, ByVal fechaFin As Date) As List(Of REP_InformeTablets_Diario_Result)
        oMatrixContext.Database.CommandTimeout = 60
        Return oMatrixContext.REP_InformeTablets_Diario(fechaInicio, fechaFin).ToList
    End Function
#End Region
End Class