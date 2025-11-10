

'Imports CoreProject.GD_Model

Public Class RepositorioDocumentos
#Region "Enumerados"
    Enum TiposContenedores
        General = 1
        Tareas = 2
    End Enum
#End Region
#Region "Variables Globales"
    Private oMatrixContext As GD_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New GD_Entities
    End Sub
#End Region
#Region "Obtener"
    Function obtenerDocumentosXIdContenedorXIdDocumento(ByVal ContenedorId As Int64, ByVal DocumentoId As Int64) As List(Of GD_RepositorioDocumentos)
        Return GD_RepositorioDocumentos_Get(Nothing, Nothing, Nothing, DocumentoId, Nothing, Nothing, Nothing, Nothing, ContenedorId)
    End Function

    Function obtenerDocumentosXId(ByVal Id As Int64) As GD_RepositorioDocumentos
        Return GD_RepositorioDocumentos_Get(Id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
    End Function

    Private Function GD_RepositorioDocumentos_Get(ByVal id As Int64?, ByVal nombre As String, ByVal url As String, ByVal documentoId As Int64?, ByVal version As Decimal?, ByVal fecha As DateTime?, ByVal comentarios As String, ByVal usuarioId As Int64?, ByVal idContenedor As Int64?) As List(Of GD_RepositorioDocumentos)
        Return oMatrixContext.GD_RepositorioDocumentos_Get(id, nombre, url, documentoId, version, fecha, comentarios, usuarioId, idContenedor).ToList
    End Function
    Function GD_RepositorioDocumentos_GetXtrabajo(ByVal id As Int64?, ByVal nombre As String, ByVal url As String, ByVal documentoId As Int64?, ByVal version As Decimal?, ByVal fecha As DateTime?, ByVal comentarios As String, ByVal usuarioId As Int64?, ByVal idContenedor As Int64?) As List(Of GD_RepositorioDocumentos)
        Return oMatrixContext.GD_RepositorioDocumentos_Get(id, nombre, url, documentoId, version, fecha, comentarios, usuarioId, idContenedor).ToList
    End Function
    Function obtenerDocumentos(id As Long?, nombre As String, url As String, documentoId As Long?, version As Double?, fecha As Date?, comentarios As String, usuarioId As Long?, idContenedor As Long?, esRecuperacion As Boolean?) As List(Of GD_RepositorioDocumentos_GetXTrabajo_Result)
        Return oMatrixContext.GD_RepositorioDocumentos_GetXTrabajo(id, nombre, url, documentoId, version, fecha, comentarios, usuarioId, idContenedor, esRecuperacion).ToList
    End Function
#End Region
#Region "Grabar"
    Function Grabar(ByVal nombre As String, ByVal url As String, ByVal documentoId As Int64, ByVal version As Decimal?, ByVal fecha As DateTime, ByVal comentarios As String, ByVal usuarioId As Int64, ByVal idContenedor As Int64) As Decimal
        Return oMatrixContext.GD_GD_RepositorioDocumentos_Add(nombre, url, documentoId, version, fecha, comentarios, usuarioId, idContenedor).FirstOrDefault
    End Function
#End Region
End Class
