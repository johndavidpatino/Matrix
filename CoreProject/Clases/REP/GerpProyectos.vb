
'Imports CoreProject.RPMatrixModel

Namespace Reportes
    <Serializable()>
    Public Class GerpProyectos
#Region "Variables Globales"
        Private oMatrixContext As RP_Entities
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            oMatrixContext = New RP_Entities
        End Sub
#End Region

#Region "Obtener"

        Public Function ObtenerTrabajosPorGrupoBU(ByVal GerenciaId As Int64) As List(Of REP_TrabajosxGrupoUnidadBU_Result)
            Return oMatrixContext.REP_TrabajosxGrupoUnidadBU(GerenciaId).ToList
        End Function

        Public Function obtenertareas(ByVal TareaId As Int64?, ByVal idtrabajo As Int64?, ByVal RolEstima As Int64?, ByVal UnidadEjecuta As Int64?, ByVal EstadoNo As Int64?, ByVal UsuarioId As Int64?, ByVal Estado As Int64?, ByVal Finip As Date?, ByVal FFnip As Date?, ByVal Finir As Date?, ByVal Ffnir As Date?, ByVal Unidad As Int64?, ByVal GrupoUnidad As Int64?) As List(Of CT_TareasIndicadores_Result)
            Return oMatrixContext.CT_TareasIndicadores(TareaId, idtrabajo, RolEstima, UnidadEjecuta, EstadoNo, UsuarioId, Estado, Finip, FFnip, Finir, Ffnir, Unidad, GrupoUnidad).ToList
            'Return oMatrixContext.CT_TareasIndicadores(TareaId, idtrabajo, RolEstima, UnidadEjecuta, EstadoNo, UsuarioId, Estado, Nothing, Nothing, Nothing, Nothing).ToList
        End Function
        Public Function ObtenerTrabajosPorUsuario(ByVal Usuario As Int64) As List(Of Rep_TrabajosXUsuario_Result)
            Return oMatrixContext.Rep_TrabajosXUsuario(Usuario).ToList
        End Function

#End Region


    End Class


End Namespace
