Imports System.Transactions

Imports System.Configuration
Imports System.IO

'Imports CoreProject.IQ_ENTITIES
Namespace IQ
    Public Class SimulacionCostos
        Dim _IQ_Entities As IQ_MODEL
        Public Sub New()
            _IQ_Entities = New IQ_MODEL
        End Sub
        Public Function ObtenerCostosOperacionesSimulados(Muestra As Integer, Productividad As Decimal, cerradas As Integer, CerradasMult As Integer, Abiertas As Integer, AbiertasMult As Integer, otras As Integer, Demogracficas As Integer, PorcentajeVerifi As Decimal) As IQ_SimuladorCostosOperaciones_Result

            Return _IQ_Entities.IQ_SimuladorCostosOperaciones(Muestra, Productividad, cerradas, CerradasMult, Abiertas, AbiertasMult, otras, Demogracficas, PorcentajeVerifi).First()
        End Function
    End Class
End Namespace

