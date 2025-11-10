'Imports CoreProject.CU_Model
'Imports CoreProject.IQ_ENTITIES

Namespace IQ

    Public Class IquoteGeneral
        Dim _IQ_Entities As IQ_MODEL
        Public Sub DuplicarAlternativa(ByVal p As IQ_Parametros)
            Try
                _IQ_Entities = New IQ_MODEL()
                _IQ_Entities.IQ_DuplicarAlternativa(p.IdPropuesta, p.ParAlternativa)
            Catch ex As Exception

            End Try

        End Sub

        Public Function ObtenerTasaCambioDia() As Double

            _IQ_Entities = New IQ_MODEL()
            Dim tasa As Double
            tasa = (From t In _IQ_Entities.IQ_ParametrosGenerales Where t.PGrCodigo = 523 Select t.PGrValor2).First()
            Return tasa

        End Function

        Public Function ObtenerParametrosGenerales(ByVal PGrCodigo As Integer) As Double

            _IQ_Entities = New IQ_MODEL()
            Dim tasa As Double
            tasa = (From t In _IQ_Entities.IQ_ParametrosGenerales Where t.PGrCodigo = PGrCodigo Select t.PGrValor2).First()
            Return tasa

        End Function

        Public Function ObtenerCiudadesMuestra() As List(Of IQ_ObtenerCiudadesMuestra_Result)
            Try
                _IQ_Entities = New IQ_MODEL()
                Return _IQ_Entities.IQ_ObtenerCiudadesMuestra().ToList()
            Catch ex As Exception
                Return Nothing
            End Try


        End Function


        Public Function MuestraInfoTrabajo(ByVal IdPropuesta As Int64, Alternativa As Int32, Metodologia As Int32, Fase As Int32) As List(Of IQ_MuestraInfoTrabajo_Result)
            _IQ_Entities = New IQ_MODEL()
            Return _IQ_Entities.IQ_MuestraInfoTrabajo(IdPropuesta, Alternativa, Metodologia, Fase).ToList
        End Function

        Public Function ExisteAlternativaAprobada(ByVal par As IQ_Parametros) As Boolean
            Dim IQ_Entitties As New IQ_MODEL
            Dim x = (From p In IQ_Entitties.IQ_Parametros Where p.IdPropuesta = par.IdPropuesta And p.ParAprobado = True Select p).Count()
            If CInt(x) > 0 Then
                Dim y = (From p In IQ_Entitties.IQ_Parametros Where p.IdPropuesta = par.IdPropuesta And p.ParAlternativa = par.ParAlternativa And p.ParAprobado = True Select p).Count()
                If y > 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return True
            End If

        End Function


        Public Function ObtenerValorTotalXalternativa(ByVal p As IQ_Parametros) As Decimal
            Dim total As Decimal
            _IQ_Entities = New IQ_MODEL()
            total = (From o In _IQ_Entities.IQ_Parametros Where o.IdPropuesta = p.IdPropuesta And o.ParAlternativa = p.ParAlternativa Select o.ParValorVenta).Sum()
            Return total
        End Function


        Public Function ObtenerValorLimiteAprobacionGM() As Decimal
            Dim valor As Decimal
            _IQ_Entities = New IQ_MODEL()
            valor = (From t In _IQ_Entities.IQ_ParametrosGenerales Where t.PGrGrupo = 15 And t.PGrCodigo = 1501 Select t.PGrValor1).First()
            Return valor

        End Function

        Public Function ObtenerSalarioMinimoActual() As Decimal
            Dim valor As Decimal
            _IQ_Entities = New IQ_MODEL()
            valor = (From t In _IQ_Entities.IQ_ParametrosGenerales Where t.PGrGrupo = 1 And t.PGrCodigo = 101 Select t.PGrValor2).First()
            Return valor

        End Function

        Function obtenerValorTransporteEncuestadorPST() As Double
            Return ObtenerParametrosGenerales(2002)
        End Function
        Function obtenerValorTransporteSupervisorPST() As Double
            Return ObtenerParametrosGenerales(2003)
        End Function
    End Class





End Namespace
