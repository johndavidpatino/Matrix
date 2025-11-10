
Namespace IQ
    Public Class ActividadDTO
        Private _ActNombre As String
        Public Property ActNombre() As String
            Get
                Return _ActNombre
            End Get
            Set(value As String)
                _ActNombre = value
            End Set
        End Property

        Private _ActID As Integer
        Public Property ActID() As Integer
            Get
                Return _ActID
            End Get
            Set(value As Integer)
                _ActID = value
            End Set
        End Property
    End Class
End Namespace

