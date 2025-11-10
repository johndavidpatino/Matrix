
Public Class Tareas_Documentos
#Region "Variables Globales"
    Private oMatrixContext As CORE_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CORE_Entities
    End Sub
#End Region
#Region "Obtener"
    Function obtenerDocumentosXTarea(ByVal tareaId As Int64?, ByVal tipoDocumentoTareaId As Int16, ByVal idConteneedor As Int64) As List(Of CORE_DocumentosRequeridosXTarea_Get_Result)
        Return oMatrixContext.CORE_DocumentosRequeridosXTarea_Get(tareaId, tipoDocumentoTareaId, idConteneedor).ToList
    End Function
    Function obtenerDocumentosXHilo(ByVal idContenedor As Int64) As List(Of CORE_DocumentosXHilo_Get_Result)
        Return oMatrixContext.CORE_DocumentosXHilo_Get(idContenedor).ToList
    End Function
    Function obtenerConfiguracionDocumentosXTarea(ByVal tareaId As Int64?, ByVal tipoDocumentoTareaId As Int16, ByVal Asignado As Boolean) As List(Of CORE_Configuracion_DocumentosXTarea_Get_Result)
        Return oMatrixContext.CORE_Configuracion_DocumentosXTarea_Get(tareaId, tipoDocumentoTareaId, Asignado).ToList
    End Function
#End Region
#Region "Grabar"
    Sub grabar(ByVal tareaId As Int64, ByVal documentoId As Int64, ByVal tipoDocumento As Int16, ByVal esOpcional As Boolean)
        Dim oCORE_Tareas_Documentos As New CORE_Tareas_Documentos
        oCORE_Tareas_Documentos.TareaId = tareaId
        oCORE_Tareas_Documentos.TipoDocumentoTareaId = tipoDocumento
        oCORE_Tareas_Documentos.DocumentoId = documentoId
        oCORE_Tareas_Documentos.EsOpcional = esOpcional
        oMatrixContext.CORE_Tareas_Documentos.Add(oCORE_Tareas_Documentos)
        oMatrixContext.SaveChanges()
    End Sub
#End Region
#Region "Eliminar"
    Sub Eliminar(ByVal Id As Int64)
        Dim oCORE_Tareas_Documentos As New CORE_Tareas_Documentos
        oCORE_Tareas_Documentos = oMatrixContext.CORE_Tareas_Documentos.Where(Function(x) x.id = Id).FirstOrDefault()
        oMatrixContext.CORE_Tareas_Documentos.Remove(oCORE_Tareas_Documentos)
        oMatrixContext.SaveChanges()
    End Sub
#End Region
End Class
