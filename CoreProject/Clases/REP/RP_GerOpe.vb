
'Imports CoreProject.RPMatrixModel

Namespace Reportes
    <Serializable()>
    Public Class RP_GerOpe
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
        Public Function ObtenerEstudiosAtraso(ByVal GerenciaId As Int64) As List(Of REP_EstudiosAtrasadosxGerencia_Result)
            Return oMatrixContext.REP_EstudiosAtrasadosxGerencia(GerenciaId, Nothing, Nothing, Nothing).ToList
        End Function
        Public Function ObtenerTrabajosPorGerencia(ByVal GerenciaId As Int64, ByVal id As Int64, ByVal job As String, ByVal trabajo As String) As List(Of REP_TrabajosxGerencia_Result)
            Return oMatrixContext.REP_TrabajosxGerencia(GerenciaId, Nothing, Nothing, Nothing).ToList
        End Function
        Public Function ObtenerTrabajosPorGerencia_filtrados(ByVal GerenciaId As Int64, ByVal id As Int64, ByVal job As String, ByVal trabajo As String) As List(Of REP_TrabajosxGerencia_Result)
            Return oMatrixContext.REP_TrabajosxGerencia(GerenciaId, id, job, trabajo).ToList
        End Function
        Public Function ObtenerEstudiosAtraso_Filtrado(ByVal GerenciaId As Int64, ByVal id As Int64, ByVal job As String, ByVal trabajo As String) As List(Of REP_EstudiosAtrasadosxGerencia_Result)
            Return oMatrixContext.REP_EstudiosAtrasadosxGerencia(GerenciaId, id, job, trabajo).ToList
        End Function

        Public Function ObtenerFichaEncuestadorDatos(ByVal MesesConsulta As Int32, ByVal IdEncuestador As Int64) As REP_FichaEncuestador_Datos_Result
            Return oMatrixContext.REP_FichaEncuestador_Datos(MesesConsulta, IdEncuestador).FirstOrDefault
        End Function
        Public Function ObtenerFichaEncuestadorMetodologias(ByVal MesesConsulta As Int32, ByVal IdEncuestador As Int64) As List(Of REP_FichaEncuestador_EncuestasMetodologia_Result)
            Return oMatrixContext.REP_FichaEncuestador_EncuestasMetodologia(MesesConsulta, IdEncuestador).ToList
        End Function
        Public Function ObtenerFichaEncuestadorTrabajos(ByVal MesesConsulta As Int32, ByVal IdEncuestador As Int64) As List(Of REP_FichaEncuestador_TrabajosParticipado_Result)
            Return oMatrixContext.REP_FichaEncuestador_TrabajosParticipado(MesesConsulta, IdEncuestador).ToList
        End Function
        Public Function ObtenerListadoPropuestasSeguimiento(ByVal Gerencia As Int64?, ByVal Estado As Int32?, ByVal Probabilidad As Int32?, ByVal Unidad As Int32?, ByVal GrupoUnidad As Int64?, ByVal GerenteCuentas As Int64?) As List(Of REP_ListadoPropuestasSeguimiento_Result)
            Return oMatrixContext.REP_ListadoPropuestasSeguimiento(Estado, Probabilidad, Unidad, GerenteCuentas, GrupoUnidad, Gerencia).ToList
        End Function
        Public Function ObtenerUsuariosXUnidadXRol(ByVal unidad As Integer, ByVal Rol As Integer) As List(Of US_Usuarios_Get_xUnidadxRolNombre_Result)
            Return oMatrixContext.US_Usuarios_Get_xUnidadxRolNombre(Rol, unidad).ToList
        End Function

        Public Function ObtenerListadoEstudiosSeguimiento(ByVal Unidad As Int32?, ByVal GrupoUnidad As Int64?, ByVal GerenteCuentas As Int64?, ByVal GerenciaOP As Int64?) As List(Of REP_ListadoEstudiosSeguimiento_Result)
            Return oMatrixContext.REP_ListadoEstudiosSeguimiento(Unidad, GerenteCuentas, GrupoUnidad, GerenciaOP).ToList
        End Function
        Public Function ListadoDiezCiudadesPrincipales() As List(Of REP_CiudadesAgrupacionDiezPrincipales_Result)
            Return oMatrixContext.REP_CiudadesAgrupacionDiezPrincipales.ToList
        End Function

        Public Function TiemposRevisionPresupuestosListado(ByVal FIni As Date, FFin As Date, GerenciaOperaciones As Int32?) As List(Of REP_TiemposRevisionPresupuestosListado_Result)
            Return oMatrixContext.REP_TiemposRevisionPresupuestosListado(FIni, FFin, GerenciaOperaciones).ToList
        End Function
        Public Function TiemposRevisionPresupuestosResumen(ByVal FIni As Date, FFin As Date, GerenciaOperaciones As Int32?) As List(Of REP_TiemposRevisionPresupuestosResumen_Result)
            Return oMatrixContext.REP_TiemposRevisionPresupuestosResumen(FIni, FFin, GerenciaOperaciones).ToList
        End Function
#End Region

#Region "Actualizar"
        Public Sub ActualizarPropuestaSeguimiento(ByVal PropuestaId As Int64, ByVal FechaInicioCampo As Date, ByVal ProbabilidadAprobacion As Int32)
            oMatrixContext.CU_Propuestas_Edit_Seguimiento(PropuestaId, ProbabilidadAprobacion, FechaInicioCampo)
        End Sub
        Public Sub ActualizarEstudioSeguimiento(ByVal EstudioId As Int64, ByVal FechaInicioCampo As Date)
            oMatrixContext.CU_Estudios_Edit_Seguimiento(EstudioId, FechaInicioCampo)
        End Sub
#End Region

    End Class


End Namespace
