Public Class ObservacionesPresupuestos
    Public Sub New()
    End Sub
    Private _Observaciones_Presupuesto As [String]
    Public Property Observaciones_Presupuesto() As [String]
        Get
            Return _Observaciones_Presupuesto
        End Get
        Set(ByVal value As [String])
            _Observaciones_Presupuesto = value
        End Set
    End Property

    Private _Observaciones_Generales As [String]
    Public Property Observaciones_Generales() As String
        Get
            Return _Observaciones_Generales
        End Get
        Set(ByVal value As String)
            _Observaciones_Generales = value
        End Set
    End Property


End Class
