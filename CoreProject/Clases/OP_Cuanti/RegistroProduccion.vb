
'Imports CoreProject.OP_Cuanti_Model

<Serializable()>
Public Class RecordProduccion
#Region "Variables Globales"
    Private oMatrixContext As OP_Cuanti
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Cuanti
    End Sub
#End Region
#Region "Obtener"
    Public Function ObtenerUnidades(identificacion As Int64?) As List(Of OP_UnidadesProduccionGet_Result)
        Return oMatrixContext.OP_UnidadesProduccionGet(identificacion).ToList
    End Function

    Public Function MatrizActividades(ByVal Unidadid As Int32?, Actividad As Int32?, SubActividad As Int32?, ByVal activa As Boolean?) As List(Of OP_ActividadesProduccionGet_Result)
        Return oMatrixContext.OP_ActividadesProduccionGet(Unidadid, Actividad, SubActividad, activa).ToList
    End Function

    Public Function JBE_JBI(ByVal tipo As Int32) As List(Of OP_JBE_JBI_CC_Get_Result)
        Return oMatrixContext.OP_JBE_JBI_CC_Get(tipo, Nothing).ToList
    End Function

    Public Function JBE_JBI_Busqueda(ByVal tipo As Int32, ByVal busqueda As String) As List(Of OP_JBE_JBI_CC_Get_Result)
        Return oMatrixContext.OP_JBE_JBI_CC_Get(tipo, busqueda).ToList
    End Function

    Public Function REP_InformeConsolidadoEjecucion(ByVal fechaInicio As Date, ByVal fechaFin As Date, ByVal areaId As Int64) As List(Of REP_InformeConsolidadoEjecucion_Result)
        Return oMatrixContext.REP_InformeConsolidadoEjecucion(fechaInicio, fechaFin, areaId).ToList
    End Function

#End Region
    Public Sub grabar(actividad As Nullable(Of Integer), subActividad As Nullable(Of Integer), unidad As Nullable(Of Integer), trabajoId As Nullable(Of Integer), estudioId As Nullable(Of Integer), fecha As Nullable(Of Date), horaInicio As Nullable(Of System.TimeSpan), horaFin As Nullable(Of System.TimeSpan), cantidad As Nullable(Of Integer), observacion As String, estado As Nullable(Of Integer), validadoPor As Nullable(Of Long), personaId As Nullable(Of Long), ByVal esReproceso As Nullable(Of Boolean), ByVal cantidadEfectivas As Nullable(Of Integer), tipoReproceso As Nullable(Of Byte), tipoAplicativoProceso As Nullable(Of Byte), cantVarsScript As Nullable(Of Integer), cantVarsExport As Nullable(Of Integer))
        oMatrixContext.OP_Produccion_Add(actividad, subActividad, unidad, trabajoId, estudioId, fecha, horaInicio, horaFin, cantidad, observacion, estado, validadoPor, personaId, esReproceso, cantidadEfectivas, tipoReproceso, tipoAplicativoProceso, cantVarsScript, cantVarsExport)
    End Sub
    Public Sub actualizar(id As Integer?, actividad As Nullable(Of Integer), subActividad As Nullable(Of Integer), unidad As Nullable(Of Integer), trabajoId As Nullable(Of Integer), estudioId As Nullable(Of Integer), fecha As Nullable(Of Date), horaInicio As Nullable(Of System.TimeSpan), horaFin As Nullable(Of System.TimeSpan), cantidad As Nullable(Of Integer), observacion As String, estado As Nullable(Of Integer), validadoPor As Nullable(Of Long), personaId As Nullable(Of Long), ByVal esReproceso As Nullable(Of Boolean), ByVal cantidadEfectivas As Nullable(Of Integer), tipoReproceso As Nullable(Of Byte), tipoAplicativoProceso As Nullable(Of Byte), cantVarsScript As Nullable(Of Integer), cantVarsExport As Nullable(Of Integer))
        oMatrixContext.OP_Produccion_Edit(id, actividad, subActividad, unidad, trabajoId, estudioId, fecha, horaInicio, horaFin, cantidad, observacion, estado, validadoPor, personaId, esReproceso, cantidadEfectivas, tipoReproceso, tipoAplicativoProceso, cantVarsScript, cantVarsExport)
    End Sub


    Public Function obtener(ByVal fechaInicio As Nullable(Of Date), ByVal fechaFin As Nullable(Of Date), ByVal personaId As Nullable(Of Int64), ByVal id As Integer?, ByVal unidad As Integer?) As List(Of OP_Produccion_Get_Result)
        Return oMatrixContext.OP_Produccion_Get(fechaInicio, fechaFin, personaId, id, unidad).ToList()
    End Function

    Public Function obtenerListaActividades(id As Nullable(Of Integer)) As OP_ListaActividades_Get_Result
        Return oMatrixContext.OP_ListaActividades_Get(id).FirstOrDefault
    End Function
End Class
