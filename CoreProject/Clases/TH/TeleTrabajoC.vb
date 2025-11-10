Public Class TeleTrabajoC
#Region "Variables Globales"
    Private oMatrixContext As TH_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New TH_Entities
    End Sub
#End Region

#Region "Obtener"
    Public Function BuscarXId(ByVal id As Long?) As TH_TeletrabajoGet_Result
        Return oMatrixContext.TH_TeletrabajoGet(id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).First()
    End Function
    Public Function BuscarXUsuario(ByRef usuario As String) As List(Of TH_TeletrabajoGet_Result)
        Return oMatrixContext.TH_TeletrabajoGet(Nothing, usuario, Nothing, Nothing, Nothing, Nothing, Nothing).ToList()
    End Function
    Public Function BuscarXUsuarioXFecha(ByRef usuario As String, ByVal fechaIni As String, ByVal fechaFin As String) As List(Of TH_TeletrabajoGet_Result)
        Dim fIni As DateTime = Convert.ToDateTime(fechaIni + " 00:00:00")
        Dim fFin As DateTime = Convert.ToDateTime(fechaFin + " 23:59:59")
        Return oMatrixContext.TH_TeletrabajoGet(Nothing, usuario, Nothing, Nothing, Nothing, fIni, fFin).ToList()
    End Function
    Public Function BuscarXjefeDirectoXEstado(ByVal jefeDirecto As Long?, ByVal estado As Long?) As List(Of TH_TeletrabajoGet_Result)
        estado = If(estado = 0, Nothing, estado)
        Return oMatrixContext.TH_TeletrabajoGet(Nothing, Nothing, Nothing, jefeDirecto, estado, Nothing, Nothing).ToList()
    End Function
    Public Function BuscarXjefeDirectoXEstadoXFechas(ByVal jefeDirecto As Int64?, ByVal estado As Long?, ByVal fechaIni As String, ByVal fechaFin As String) As List(Of TH_TeletrabajoGet_Result)
        estado = If(estado = 0, Nothing, estado)
        Dim fIni As DateTime = Convert.ToDateTime(fechaIni + " 00:00:00")
        Dim fFin As DateTime = Convert.ToDateTime(fechaFin + " 23:59:59")

        Return oMatrixContext.TH_TeletrabajoGet(Nothing, Nothing, Nothing, jefeDirecto, estado, fIni, fFin).ToList()
    End Function
    Public Function BuscarAreasXId(ByRef id As Long?) As List(Of TH_Area_JefeArea_Get_Result)
        Return oMatrixContext.TH_Area_JefeArea_Get(id).ToList
    End Function
    Public Function BuscarJefes() As List(Of TH_Jefes_Get_Result)
        Return oMatrixContext.TH_Jefes_Get().ToList
    End Function
    Public Function BuscarJefesArea() As List(Of TH_HWH_AprobacionManager_Get_Result)
        Return oMatrixContext.TH_HWH_AprobacionManager_Get().ToList
    End Function
    Public Function BuscarTeleTrabajosJefeXId(ByVal fechaIni As String, ByVal fechaFin As String, ByRef id As Long?) As List(Of TH_TeleTrabajoJefeXId_Result)
        Dim fIni As DateTime = Convert.ToDateTime(fechaIni + " 00:00:00")
        Dim fFin As DateTime = Convert.ToDateTime(fechaFin + " 23:59:59")
        Return oMatrixContext.TH_TeleTrabajoJefeXId(fIni, fFin, id).ToList
    End Function

    Public Function BuscarTeleTrabajosJefeXJefe(ByVal fechaIni As String, ByVal fechaFin As String, ByRef jefeDirecto As Long?, ByRef estado As Long?) As List(Of TH_TeleTrabajoJefeXJefe_Result)
        Dim fIni As DateTime = Convert.ToDateTime(fechaIni + " 00:00:00")
        Dim fFin As DateTime = Convert.ToDateTime(fechaFin + " 23:59:59")
        estado = If(estado = 0, Nothing, estado)
        Return oMatrixContext.TH_TeleTrabajoJefeXJefe(fIni, fFin, jefeDirecto, estado).ToList
    End Function

#End Region

#Region "Guardar"
    Sub Guardar(ByRef e As TH_Teletrabajo)
        oMatrixContext.TH_Teletrabajo.Add(e)
        oMatrixContext.SaveChanges()

        oMatrixContext.TH_LogTeleTrabajoAdd(e.id, e.Estado, e.Observaciones, e.Usuario)
    End Sub

    Sub LogGuardar(ByRef e As TH_LogTeleTrabajo)
        oMatrixContext.TH_LogTeleTrabajo.Add(e)
        oMatrixContext.SaveChanges()
    End Sub
#End Region

#Region "Actualizar"
    Sub ActualizarGestion(ByVal id As Long, ByVal estado As Long, ByVal UsuarioGestion As Long, ByVal ObservacionesGestion As String)
        oMatrixContext.TH_TeletrabajoUpdate(id, estado, UsuarioGestion, ObservacionesGestion)
        oMatrixContext.TH_LogTeleTrabajoAdd(id, estado, ObservacionesGestion, UsuarioGestion)
    End Sub
#End Region
End Class
