Namespace ExternalServices.BIServices
    Public Class Result(Of T)
        Public Property IsSuccess As Boolean
        Public Property ErrorResult As ErrorResult
        Public Property Data As T
    End Class
End Namespace