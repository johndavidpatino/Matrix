Imports CoreProject

Public Class SimuladorCostosOperaciones
    Inherits System.Web.UI.Page
    Dim _Simulacion As New IQ.SimulacionCostos
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnCalcular_Click(sender As Object, e As EventArgs) Handles btnCalcular.Click
        Dim CostosSimuladosOper As IQ_SimuladorCostosOperaciones_Result
        Dim Muestra, Cerradas, CerradasMult, Abiertas, AbiertasMult, OtrasCuales, Demograficas As Integer
        Dim Productividad, PorcentajeVerificacion As Decimal
        Muestra = If(txtMuestra.Text = String.Empty, 0, CInt(txtMuestra.Text))
        Cerradas = If(txtCerradas.Text = String.Empty, 0, CInt(txtCerradas.Text))
        CerradasMult = If(txtCerradasMult.Text = String.Empty, 0, CInt(txtCerradasMult.Text))
        Abiertas = If(txtAbiertas.Text = String.Empty, 0, CInt(txtAbiertas.Text))
        AbiertasMult = If(txtAbiertasMult.Text = String.Empty, 0, CInt(txtAbiertasMult.Text))
        OtrasCuales = If(txtOtrosCuales.Text = String.Empty, 0, CInt(txtOtrosCuales.Text))
        Demograficas = If(txtDemograficos.Text = String.Empty, 0, CInt(txtDemograficos.Text))
        Productividad = If(txtProductividad.Text = String.Empty, 0, CDec(txtProductividad.Text))
        PorcentajeVerificacion = If(txtPorcVerificacion.Text = String.Empty, 0, CDec(txtPorcVerificacion.Text))

        CostosSimuladosOper = _Simulacion.ObtenerCostosOperacionesSimulados(Muestra, Productividad, Cerradas, CerradasMult, Abiertas, AbiertasMult, OtrasCuales, Demograficas, PorcentajeVerificacion)

        txtCampo.Text = CDec(CostosSimuladosOper.VrCostoCampo).ToString("C")
        txtCritica.Text = CDec(CostosSimuladosOper.VrCostoCritica).ToString("C")
        txtVerificion.Text = CDec(CostosSimuladosOper.VrCostoVerificacion).ToString("C")
        txtCodificacion.Text = CDec(CostosSimuladosOper.VrCostoCodificacion).ToString("C")
        txtScripting.Text = CDec(CostosSimuladosOper.VrCostoScripting).ToString("C")
        txtCaptura.Text = CDec(CostosSimuladosOper.VrCostoCaptura).ToString("C")
        txtDataCleaning.Text = CDec(CostosSimuladosOper.VrCostoDataClean).ToString("C")
        txtProcesamiento.Text = CDec(CostosSimuladosOper.VrCostoProcesamiento).ToString("C")
        txtGeneracionArchivos.Text = CDec(CostosSimuladosOper.VrCostoArchivos).ToString("C")
        upResultados.Update()
    End Sub
End Class