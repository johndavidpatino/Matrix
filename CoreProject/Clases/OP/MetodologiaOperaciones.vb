
'Imports CoreProject.OP_Model

<Serializable()>
Public Class MetodologiaOperaciones
#Region "Enumerados"
    Enum EMetodologiasOperaciones
        Especializada = 4
        MysteryShopper = 5
    End Enum
#End Region

#Region "Variables Globales"
    Private oMatrixContext As OP_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Entities
    End Sub
#End Region
#Region "Obtener"

    Function obtenerTodos() As List(Of OP_Metodologias_Get_Result)
        Return OP_Metodologias_Get(Nothing, Nothing, Nothing, Nothing, Nothing)
    End Function

    Function obtenerMetodologiasTipoPy(ByVal cualitativo As Boolean) As List(Of OP_Metodologias2)
        If cualitativo = True Then
            Return oMatrixContext.OP_Metodologias.Where(Function(x) x.MetGrupoUnidad = 20).ToList
        Else
            Return oMatrixContext.OP_Metodologias.Where(Function(x) Not (x.MetGrupoUnidad = 20)).ToList
        End If
    End Function


    Function obtenerXId(ByVal id As Integer) As OP_Metodologias_Get_Result
        Return OP_Metodologias_Get(id, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
    End Function

    Function obtenerXCod(ByVal Cod As Integer) As OP_Metodologias_Get_Result
        Return OP_Metodologias_Get(Nothing, Nothing, Cod, Nothing, Nothing).FirstOrDefault
    End Function

    Private Function OP_Metodologias_Get(ByVal id As Integer?, ByVal idTecnica As Integer?, ByVal metCodigo As Integer?, ByVal metNombre As String, ByVal metGrupoUnidad As Integer?) As List(Of OP_Metodologias_Get_Result)
        Return oMatrixContext.OP_Metodologias_Get(id, idTecnica, metCodigo, metNombre, metGrupoUnidad).ToList
    End Function
    Function ObtenerFichaMetodologiaxId(ByVal Metodologia As Int32) As OP_MetodologiaTipoFicha
        Return oMatrixContext.OP_MetodologiaTipoFicha.Where(Function(x) x.MetodologiaId = Metodologia).FirstOrDefault()
    End Function

    Function obtenerMetodologiasCodTec(ByVal CodigoTecnica As Int32) As List(Of OP_Metodologias2)
        Return oMatrixContext.OP_Metodologias.Where(Function(x) x.TecCodigo = CodigoTecnica).ToList
    End Function

    Function obtenerMetodologiasCuanti() As List(Of OP_Metodologias2)
        Return oMatrixContext.OP_Metodologias.Where(Function(x) Not (x.MetGrupoUnidad = 20)).ToList
    End Function

#End Region
End Class
