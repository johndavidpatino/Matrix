'Imports CoreProject.CU_Model
'Imports CoreProject.IQ_ENTITIES

Namespace IQ

    Public Class Transacciones
        Dim _IQ_Entities As IQ_MODEL

        Public Function PresupuestosPorAlternativa(ByVal p As Int32, a As Int32) As List(Of IQ_PresupuestosPorPropuesta_Result)
            _IQ_Entities = New IQ_MODEL()
            Return _IQ_Entities.IQ_PresupuestosPorPropuesta(p, a, Nothing, Nothing).ToList
        End Function

        Public Sub DuplicarAlternativaToPropuesta(ByVal p As Int32, ByVal Alternativa As Int32, newp As Int32)
            Try
                _IQ_Entities = New IQ_MODEL()
                _IQ_Entities.IQ_DuplicarAlternativaToPropuesta(p, Alternativa, newp)
            Catch ex As Exception

            End Try
        End Sub

        Public Sub CopiarPresupuesto(ByVal p As Int32, alt As Int32, metcodigo As Int32, fase As Int32, ByVal altd As Int32, ByVal fased As Int32)
            _IQ_Entities = New IQ_MODEL()
            _IQ_Entities.IQ_CopiarUnPresupuestoConFase(p, alt, fase, metcodigo, p, altd, fased)
        End Sub

        Public Function AlternativasDisponibles(ByVal propuesta As Int64) As List(Of Integer?)
            _IQ_Entities = New IQ_MODEL()
            Return _IQ_Entities.IQ_AlternativasPorPropuesta(propuesta, Nothing, Nothing, Nothing).ToList
        End Function

        Public Function FasesDisponibles(ByVal propuesta As Int64, ByVal alternativa As Int32, ByVal metcodigo As Integer) As List(Of IQ_FasesDisponiblesPorAlternativa_Result)
            _IQ_Entities = New IQ_MODEL()
            Return _IQ_Entities.IQ_FasesDisponiblesPorAlternativa(propuesta, alternativa, Nothing, metcodigo).ToList
        End Function


    End Class





End Namespace
