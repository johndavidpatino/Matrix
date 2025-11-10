'Imports CoreProject.IQ_ENTITIES

Namespace IQ

    Public Class GrossMargin

        Dim _IQ_Entities As IQ_MODEL

        Public Function ValidarUsuarioAutorizado(ByVal Unit As Integer, ByVal User As Decimal) As Boolean
            'Validamos si el usuario existe y ademas esta autorizadoa modificar dentro de la unidad respectiva .
            _IQ_Entities = New IQ_MODEL
            If (_IQ_Entities.IQ_AutorizadosModificacionGM.Any(Function(a) a.IdUsuario = User And a.Unidad = Unit)) Then
                Return True
            Else
                Return False

            End If

        End Function
        Public Function SimularGM(ByVal P As IQ_Parametros, ByVal ValorVenta As Decimal, ByVal Simular As Boolean) As Decimal
            _IQ_Entities = New IQ_MODEL
            Dim resultado As Decimal
            resultado = _IQ_Entities.IQ_AjustarGM(P.IdPropuesta, P.ParAlternativa, P.MetCodigo, P.ParNacional, ValorVenta, Simular).First()
            Return resultado
        End Function

        Public Function SimularVenta(ByVal P As IQ_Parametros, ByVal GM_Unidad As Decimal, ByVal GM_Operaciones As Decimal, ByVal Simular As Boolean) As Decimal
            _IQ_Entities = New IQ_MODEL
            Dim resultado As Decimal
            resultado = _IQ_Entities.IQ_AjustarVenta(P.IdPropuesta, P.ParAlternativa, P.MetCodigo, P.ParNacional, GM_Unidad, GM_Operaciones, Simular).First()
            Return resultado
            'Dim valor As Decimal
            'valor = _IQ_Entities.IQ_AjustarVenta(P.IdPropuesta, P.ParAlternativa, P.MetCodigo, P.ParNacional, GM_Unidad, GM_Operaciones, Simular).First()
            'Return valor
        End Function

        Public Function ObtenerCostosJBI(p As IQ_Parametros, GMU As Decimal, GMO As Decimal, simular As Boolean) As List(Of IQ_CostosJobBookInternoTotales_Result)
            _IQ_Entities = New IQ_MODEL
            Return _IQ_Entities.IQ_CostosJobBookInternoTotales(p.IdPropuesta, p.ParAlternativa, p.MetCodigo, p.ParNacional, GMU, GMO, simular).ToList()
        End Function

        Public Function ObtenerCostosJBE(p As IQ_Parametros, GMU As Decimal, GMO As Decimal, simular As Boolean) As List(Of IQ_CostosJobBookExternoTotales_Result)
            _IQ_Entities = New IQ_MODEL
            Return _IQ_Entities.IQ_CostosJobBookExternoTotales(p.IdPropuesta, p.ParAlternativa, p.MetCodigo, p.ParNacional, GMU, GMO, simular).ToList()
        End Function
    End Class
End Namespace


