Public Class UC_Header_Presupuesto
	Inherits System.Web.UI.UserControl

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarIncidencia(ddlIncidencia)
        End If
    End Sub

    Private Sub CargarIncidencia(ByVal lst As DropDownList)
        Dim i As Integer
        Dim li As ListItem
        For i = 0 To 100 Step 1
            If i = 0 Then
                li = New ListItem With {.Text = "Seleccione...", .Value = "0"}

                lst.Items.Add(li)
            Else
                li = New ListItem With {.Text = i.ToString() & "%", .Value = i.ToString()}

                lst.Items.Add(li)
            End If
        Next

    End Sub

    Sub ClearControls()
        txtGrupoObjetivo.Text = String.Empty
        txtCerradas.Text = 0
        txtCerradasMultiples.Text = 0
        txtAbiertas.Text = 0
        txtAbiertasMultiples.Text = 0
        txtOtros.Text = 0
        txtDemograficos.Text = 15
        ddlIncidencia.SelectedIndex = 0
        txtProductividad.Text = 0
        chbProbabilistico.Checked = False
        chbF2fVirtual.Checked = False

        chbProcessCampo.Checked = False
        chbProcessVerificacion.Checked = False
        chbProcessCritica.Checked = False
        chbProcessCodificacion.Checked = False
        chbProcessDataClean.Checked = False
        chbProcessTopLines.Checked = False
        chbProcessProceso.Checked = False
        chbProcessArchivos.Checked = False
        chbProcessScripting.Checked = False
        ddlComplejidadCodificación.SelectedIndex = 1
        ddlComplejidadCuestionario.SelectedIndex = 1

        txtProcesosDataClean.Text = 0
        txtProcesosToplines.Text = 0
        txtProcesosTablas.Text = 0
        txtProcesosArchivos.Text = 0
        chbDPTransformacion.Checked = False
        chbDPUnificacion.Checked = False
        ddlComplejidadProcesamiento.SelectedIndex = 1
        ddlPonderacion.SelectedIndex = 0
        chbDPInInterna.Checked = False
        chbDPInCliente.Checked = False
        chbDPInPanel.Checked = False
        chbDPInExterno.Checked = False
        chbDPInGMU.Checked = False
        chbDPInOtro.Checked = False
        chbDPOutCliente.Checked = False
        chbDPOutWebDelivery.Checked = False
        chbDPOutExterno.Checked = False
        chbDPOutGMU.Checked = False
        chbDPOutOtro.Checked = False

        txtPorcInterceptacion.Text = 0
        txtPorcReclutamiento.Text = 0
        txtEncuestadoresPunto.Text = 0
        txtApoyosLogisticos.Text = 0
        chbPTRequierecompra.Checked = False
        chbPTNeutralizador.Checked = False
        ddlTipoProducto.SelectedIndex = 0
        txtNumLotes.Text = 0
        txtNumUnidadesLote.Text = 0
        txtValorProducto.Text = 0
        txtVisitasRequeridas.Text = 0
        txtCeldasEvaluadas.Text = 0
        txtNumeroProductosPersona.Text = 0
        chbPTAccesoInternet.Checked = False
        ddlTipoCLT.SelectedIndex = 0
        txtValorAlquilerEquipos.Text = 0

        btnPruebaProducto.Enabled = False
    End Sub

    Protected Sub btnOKDP_Click1(sender As Object, e As EventArgs)

        If Not (IsNumeric(txtProcesosDataClean.Text)) Then txtProcesosDataClean.Text = 0
        If Not (IsNumeric(txtProcesosToplines.Text)) Then txtProcesosToplines.Text = 0
        If Not (IsNumeric(txtProcesosTablas.Text)) Then txtProcesosTablas.Text = 0
        If Not (IsNumeric(txtProcesosArchivos.Text)) Then txtProcesosArchivos.Text = 0
        If Integer.Parse(txtProcesosDataClean.Text) > 0 Then chbProcessDataClean.Checked = True
        If Integer.Parse(txtProcesosToplines.Text) > 0 Then chbProcessTopLines.Checked = True
        If Integer.Parse(txtProcesosTablas.Text) > 0 Then chbProcessProceso.Checked = True
		If Integer.Parse(txtProcesosArchivos.Text) > 0 Then chbProcessArchivos.Checked = True
		If chbDPUnificacion.Checked = True Then chbProcessArchivos.Checked = True
		If chbDPTransformacion.Checked = True Then chbProcessArchivos.Checked = True
		If Integer.Parse(txtProcesosDataClean.Text) = 0 Then chbProcessDataClean.Checked = False
        If Integer.Parse(txtProcesosToplines.Text) = 0 Then chbProcessTopLines.Checked = False
        If Integer.Parse(txtProcesosTablas.Text) = 0 Then chbProcessProceso.Checked = False
		If Integer.Parse(txtProcesosArchivos.Text) = 0 And chbDPUnificacion.Checked = False And chbDPTransformacion.Checked = False Then chbProcessArchivos.Checked = False
	End Sub

    Public Function ActividadesSubcontratadas(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of CoreProject.IQ_CostoActividades)
        Dim lstAct As New List(Of CoreProject.IQ_CostoActividades)
        For Each row As GridViewRow In gvActividadesSubcontratadas.Rows
            If IsNumeric(DirectCast(row.Cells(2).FindControl("txtValorAct"), TextBox).Text) Then
                'If CDec(DirectCast(row.Cells(2).FindControl("txtValorAct"), TextBox).Text) > 0 Then
                Dim Act As New CoreProject.IQ_CostoActividades
                    Act.IdPropuesta = idPropuesta
                    Act.ParAlternativa = Alternativa
                    Act.MetCodigo = Metodologia
                    Act.ParNacional = Fase
                    Act.CaCosto = CDec(DirectCast(row.Cells(2).FindControl("txtValorAct"), TextBox).Text)
                    Act.CaUnidades = 1
                    Act.CaDescripcionUnidades = ""
                    Act.ActCodigo = gvActividadesSubcontratadas.DataKeys(row.RowIndex).Value
                    lstAct.Add(Act)
                'End If
            End If
        Next
        Return lstAct
    End Function


    Public Function AnalisisEstadisticos(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of CoreProject.IQ_AnalisisEstadisticaPresupuesto)
        Dim lstAct As New List(Of CoreProject.IQ_AnalisisEstadisticaPresupuesto)
        For Each row As GridViewRow In gvAnalisisEstadisticos.Rows
            If IsNumeric(DirectCast(row.Cells(3).FindControl("txtCantidad"), TextBox).Text) Then
                If CInt(DirectCast(row.Cells(3).FindControl("txtCantidad"), TextBox).Text) > 0 Then
                    Dim Act As New CoreProject.IQ_AnalisisEstadisticaPresupuesto
                    Act.IdPropuesta = idPropuesta
                    Act.ParAlternativa = Alternativa
                    Act.MetCodigo = Metodologia
                    Act.ParNacional = Fase
                    Act.Cantidad = CInt(DirectCast(row.Cells(2).FindControl("txtCantidad"), TextBox).Text)
                    'Act.VrTotal = CDec(row.Cells(3).Text) * CDec(Act.Cantidad)
                    Act.IdAnalisis = gvAnalisisEstadisticos.DataKeys(row.RowIndex).Value
                    lstAct.Add(Act)
                End If
            End If
        Next
        Return lstAct
    End Function

    Public Function HorasProfesionales(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of CoreProject.IQ_HorasProfesionales)
        Dim lstHor As New List(Of CoreProject.IQ_HorasProfesionales)
        For Each row As GridViewRow In gvProfessionalTime.Rows
            Dim Hor As New CoreProject.IQ_HorasProfesionales
            Hor.CodCargo = gvProfessionalTime.DataKeys(row.RowIndex).Value
            Hor.IdPropuesta = idPropuesta
            Hor.ParAlternativa = Alternativa
            Hor.MetCodigo = Metodologia
            Hor.ParNacional = Fase
            Dim TotalHoras As Integer = 0

            If IsNumeric(DirectCast(row.Cells(1).FindControl("txtPreField"), TextBox).Text) And Not DirectCast(row.Cells(1).FindControl("txtPreField"), TextBox).Text = "" Then
                Hor.PreField = DirectCast(row.Cells(1).FindControl("txtPreField"), TextBox).Text
                TotalHoras += Hor.PreField
            End If

            If IsNumeric(DirectCast(row.Cells(2).FindControl("txtFieldWork"), TextBox).Text) And Not DirectCast(row.Cells(2).FindControl("txtFieldWork"), TextBox).Text = "" Then
                Hor.FieldWork = DirectCast(row.Cells(2).FindControl("txtFieldWork"), TextBox).Text
                TotalHoras += Hor.FieldWork
            End If

            If IsNumeric(DirectCast(row.Cells(3).FindControl("txtDPandCoding"), TextBox).Text) And Not DirectCast(row.Cells(3).FindControl("txtDPandCoding"), TextBox).Text = "" Then
                Hor.DPandCoding = DirectCast(row.Cells(3).FindControl("txtDPandCoding"), TextBox).Text
                TotalHoras += Hor.DPandCoding
            End If

            If IsNumeric(DirectCast(row.Cells(4).FindControl("txtAnalysis"), TextBox).Text) And Not DirectCast(row.Cells(4).FindControl("txtAnalysis"), TextBox).Text = "" Then
                Hor.Analysis = DirectCast(row.Cells(4).FindControl("txtAnalysis"), TextBox).Text
                TotalHoras += Hor.Analysis
            End If

            If IsNumeric(DirectCast(row.Cells(5).FindControl("txtPM"), TextBox).Text) And Not DirectCast(row.Cells(5).FindControl("txtPM"), TextBox).Text = "" Then
                Hor.PM = DirectCast(row.Cells(5).FindControl("txtPM"), TextBox).Text
                TotalHoras += Hor.PM
            End If

            If IsNumeric(DirectCast(row.Cells(6).FindControl("txtMeetings"), TextBox).Text) And Not DirectCast(row.Cells(6).FindControl("txtMeetings"), TextBox).Text = "" Then
                Hor.Meetings = DirectCast(row.Cells(6).FindControl("txtMeetings"), TextBox).Text
                TotalHoras += Hor.Meetings
            End If

            If IsNumeric(DirectCast(row.Cells(7).FindControl("txtPresentation"), TextBox).Text) And Not DirectCast(row.Cells(7).FindControl("txtPresentation"), TextBox).Text = "" Then
                Hor.Presentation = DirectCast(row.Cells(7).FindControl("txtPresentation"), TextBox).Text
                TotalHoras += Hor.Presentation
            End If

            If IsNumeric(DirectCast(row.Cells(8).FindControl("txtClientTravel"), TextBox).Text) And Not DirectCast(row.Cells(8).FindControl("txtClientTravel"), TextBox).Text = "" Then
                Hor.ClientTravel = DirectCast(row.Cells(8).FindControl("txtClientTravel"), TextBox).Text
                TotalHoras += Hor.ClientTravel
            End If
            Hor.Horas = TotalHoras
            If TotalHoras > 0 Then
                lstHor.Add(Hor)
            End If

        Next
        Return lstHor
    End Function

End Class