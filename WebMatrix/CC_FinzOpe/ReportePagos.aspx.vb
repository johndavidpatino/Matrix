Imports WebMatrix.Util
Imports CoreProject
Public Class ReportePagos
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub


    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If txttrabajo.Text = "" And txtjob.Text = "" Then
            ShowNotification("Ingrese id de Trabajo o Job a buscar", ShowNotifications.InfoNotification)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        ElseIf txttrabajo.Text = "" Then
            ReportePagos(Nothing, txtjob.Text)
            Muestra(Nothing, txtjob.Text)
        ElseIf txtjob.Text = "" Then
            ReportePagos(txttrabajo.Text, Nothing)
            Muestra(txttrabajo.Text, Nothing)
        End If
    End Sub

    Sub ReportePagos(ByVal TrabajoId As Int64?, ByVal Job As String)
        Dim op As New ProcesosInternos
        GvDetallePagos.DataSource = op.ResumenPagos(TrabajoId, Job)
        GvDetallePagos.DataBind()
        GvCampo.DataSource = op.ResumenPagos(TrabajoId, Job)
        GvCampo.DataBind()
        GvTotales.DataSource = op.ReportePagosConsolidado(TrabajoId, Job)
        GvTotales.DataBind()
        Informaciontrabajo(TrabajoId, Job)
        TotalProduccion()
    End Sub

    Sub Muestra(ByVal TrabajoId As Int64?, ByVal Job As String)
        Dim op As New ProcesosInternos
        lblmuestra.Text = op.ObtenerMuestra(TrabajoId, Job)
    End Sub

    Sub Informaciontrabajo(ByVal TrabajoId As Int64?, ByVal Job As String)
        Dim op As New ProcesosInternos
        Dim Res As New List(Of CC_InformacionTrabajos_Result)
        Res = op.InformacionTrabajos(TrabajoId, Job)
        Dim pres As New List(Of CC_ActividadesPresupuestadas_Result)
        Dim Campo As New List(Of CC_ActividadesPresupuestadasCampo_Result)

        For i = 0 To Res.Count - 1
            pres = op.ActividadesPresupuestadas(Res(i).IdPropuesta, Res(i).Alternativa, Res(i).Fase, Res(i).MetCodigo)
            Campo = op.ActividadesPresupuestadasCampo(Res(i).IdPropuesta, Res(i).Alternativa, Res(i).Fase, Res(i).MetCodigo)
        Next
        GvActividades.DataSource = pres.ToList
        GvActividades.DataBind()
        GvActividadesCampo.DataSource = Campo.ToList
        GvActividadesCampo.DataBind()
        TotalPresupuetado()
    End Sub

    Sub TotalProduccion()
        Dim totalpro As Double = 0
        For i = 0 To Me.GvDetallePagos.Rows.Count - 1
            totalpro = GvDetallePagos.Rows(i).Cells(5).Text + totalpro
        Next
        lblTotalPagado.Text = FormatCurrency(totalpro, 0)
    End Sub

    Sub TotalPresupuetado()
        Dim totalpres As Double = 0
        For i = 0 To Me.GvActividades.Rows.Count - 1
            If IsNumeric(GvActividades.Rows(i).Cells(2).Text) Then
                totalpres = GvActividades.Rows(i).Cells(2).Text + totalpres
            End If
        Next
        lblPresupuestado.Text = FormatCurrency(totalpres, 0)
    End Sub

    Private Sub GvActividades_DataBound(sender As Object, e As EventArgs) Handles GvActividades.DataBound
        Dim TotalPres As Decimal = 0
        Dim Totalauto As Decimal = 0

        For Each r As GridViewRow In GvActividades.Rows
            If r.RowType = DataControlRowType.DataRow Then
                If IsNumeric(r.Cells(2).Text) Then
                    TotalPres += Convert.ToDecimal(CDbl(r.Cells(2).Text))

                End If
                If IsNumeric(r.Cells(3).Text) Then
                    Totalauto += Convert.ToDecimal(CDbl(r.Cells(3).Text))
                End If
            End If
        Next
        GvActividades.FooterRow.Cells(1).Text = "Total:"
        GvActividades.FooterRow.Cells(2).Text = FormatCurrency(TotalPres, 0)
        GvActividades.FooterRow.Cells(3).Text = FormatCurrency(Totalauto, 0)
    End Sub

End Class