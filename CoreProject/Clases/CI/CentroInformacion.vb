Imports System.Data.Entity.Core.Objects

Imports System.Data.SqlClient

Namespace OP
    Public Class CentroInformacion

#Region "Variables Globales"
        Private oMatrixContext As CIEntities
#End Region

#Region "Enumerador"
        Enum eGrupoUnidad
            Loyalty = 3
            ASI = 1
            Opinion = 4
            Marketing = 2
        End Enum
        Enum eTipoGrupounidad
            Comercial = 1
            Operativa = 2
        End Enum
#End Region

#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New CIEntities
        End Sub
#End Region

#Region "Obtener"

        Function obtenerdetallesxtodoscampos(ByVal TodosCampos As String) As List(Of CI_DetalleAlmacenamiento_Get_Result)
            Return oMatrixContext.CI_DetalleAlmacenamiento_Get(Nothing, Nothing, Nothing, TodosCampos).ToList
        End Function

        Function obtenerdetallesxid(ByVal Id As Int64?) As List(Of CI_DetalleAlmacenamiento_Get_Result)
            Return oMatrixContext.CI_DetalleAlmacenamiento_Get(Id, Nothing, Nothing, Nothing).ToList
        End Function

        Function obtenerdetallesxidtrabajo(ByVal IdTrabajo As Int64?) As List(Of CI_DetalleAlmacenamiento_Get_Result)
            Return oMatrixContext.CI_DetalleAlmacenamiento_Get(Nothing, Nothing, IdTrabajo, Nothing).ToList
        End Function

        Function obtenerdetallesxidmaestro(ByVal IdMaestro As Int64?) As List(Of CI_DetalleAlmacenamiento_Get_Result)
            Return oMatrixContext.CI_DetalleAlmacenamiento_Get(Nothing, IdMaestro, Nothing, Nothing).ToList
        End Function

        Function obtenerdetallestodos() As List(Of CI_DetalleAlmacenamiento_Get_Result)
            Return oMatrixContext.CI_DetalleAlmacenamiento_Get(Nothing, Nothing, Nothing, Nothing).ToList
        End Function

        Function obtenermaestroxid(ByVal Id As Int64) As List(Of CI_MaestroAlmacenamiento_Get_Result)
            Return oMatrixContext.CI_MaestroAlmacenamiento_Get(Id).ToList
        End Function

        Function obtenermaestrotodos() As List(Of CI_MaestroAlmacenamiento_Get_Result)
            Return oMatrixContext.CI_MaestroAlmacenamiento_Get(Nothing).ToList
        End Function

        Function obtenersolicitudtodas() As List(Of CI_SolicitudMedios_Get_Result)
            Return oMatrixContext.CI_SolicitudMedios_Get(Nothing, Nothing, Nothing).ToList
        End Function

        Function obtenersolicitudxid(ByVal Id As Int64?) As CI_SolicitudMedios_Get_Result
            Return oMatrixContext.CI_SolicitudMedios_Get(Id, Nothing, Nothing).FirstOrDefault
        End Function

        Function obtenersolicitudxidmedio(ByVal IdMedio As Int64?) As List(Of CI_SolicitudMedios_Get_Result)
            Return oMatrixContext.CI_SolicitudMedios_Get(Nothing, IdMedio, Nothing).ToList
        End Function

        Function obtenersolicitudxtodoscampos(ByVal TodosCampos As String) As List(Of CI_SolicitudMedios_Get_Result)
            Return oMatrixContext.CI_SolicitudMedios_Get(Nothing, Nothing, TodosCampos).ToList
        End Function

        Function obtenerdocumentoscierre(ByVal IdTrabajo As Int64, ByVal rolResponsableCierre As Int64?) As List(Of CI_DocumentosCierre_Get_Result)
            Return oMatrixContext.CI_DocumentosCierre_Get(IdTrabajo, rolResponsableCierre).ToList
        End Function

        Function obtenerdocumentosrecuperacion(ByVal IdTrabajo As Int64) As List(Of CI_DocumentosRecuperacion_Get_Result)
            Return oMatrixContext.CI_DocumentosRecuperacion_Get(IdTrabajo).ToList
        End Function

        Function obtenerCarpetasRaiz(ByVal unidad As Integer?, tipoGrupoUnidad As eTipoGrupounidad?) As IList(Of CI_CarpetasRedRaiz_Get_Result)
            Return oMatrixContext.CI_CarpetasRedRaiz_Get(unidad, tipoGrupoUnidad).ToList
        End Function
        Function obtenerCarpetas(ByVal unidad As Integer?, tipoGrupoUnidad As eTipoGrupounidad?) As IList(Of CI_CarpetasRed_Get_Result)
            Return oMatrixContext.CI_CarpetasRed_Get(unidad, tipoGrupoUnidad).ToList
        End Function
        Function busquedaXVariosCampos(ByVal variosCampos As String) As List(Of CI_Busqueda_Get_Result)
            Return oMatrixContext.CI_Busqueda_Get(variosCampos).ToList
        End Function
#End Region

#Region "Guardar"

        Public Function GuardarDetalleAlmacenamiento(IdMaestro As Int64?, ByVal IdTrabajo As Int64?, ByVal Contiene As String, ByVal Observacion As String) As Decimal
            Return oMatrixContext.CI_DetalleAlmacenamiento_Add(IdMaestro, IdTrabajo, Contiene, Observacion).FirstOrDefault
        End Function

        Public Sub ActualizarDetalleAlmacenamiento(id As Int64?, IdMaestro As Int64?, ByVal IdTrabajo As Int64?, ByVal Contiene As String, ByVal Observacion As String)
            oMatrixContext.CI_DetalleAlmacenamiento_Edit(id, IdMaestro, IdTrabajo, Contiene, Observacion)
        End Sub

        Public Function GuardarMaestroAlmacenamiento(UsuarioId As Int64?, ByVal Observacion As String, ByVal TipoAlmacenamiento As String) As Decimal
            Return oMatrixContext.CI_MaestroAlmacenamiento_Add(UsuarioId, Observacion, TipoAlmacenamiento).FirstOrDefault
        End Function

        Public Function GuardarSolicitudMedios(IdMedio As Int64?, ByVal IdUsuarioSolicita As Int64?) As Decimal
            Return oMatrixContext.CI_SolicitudMedios_Add(IdMedio, IdUsuarioSolicita).FirstOrDefault
        End Function

        Public Function GuardarLogCierres(ByVal IdTrabajo As Int64?, ByVal IdUsuarioCierre As Int64?, ByVal Observacion As String, CierreForzado As Boolean?) As Decimal
            Return oMatrixContext.CI_LogCierres_Add(IdTrabajo, IdUsuarioCierre, Observacion, CierreForzado).FirstOrDefault
        End Function

        Function guardarLogCreacionCarpetasRed(trabajo As Long?, carpetaRed As Integer?, creada As Boolean?, descripcion As String, usuario As Long?, fecha As Date?) As Int64
            Return oMatrixContext.CI_LogCreacionCarpetasRed_Add(trabajo, carpetaRed, creada, descripcion, usuario, fecha).FirstOrDefault
        End Function

#End Region

    End Class
End Namespace