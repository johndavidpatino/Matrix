Imports System.IO
Imports WebMatrix.Util

Public Class MatrizEstimacionGeneral
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarMatriz()
        End If
    End Sub

#End Region

#Region "Matriz"
    Sub CargarMatriz()
        Dim o As New CoreProject.Reportes.AvanceCampo
        Me.gvMatriz.DataSource = o.ObtenerMatrizCumplimiento
        Me.gvMatriz.DataBind()
        FormatoGridview()
    End Sub

    Sub FormatoGridview()
        For Each e As GridViewRow In gvMatriz.Rows
            If e.RowType = DataControlRowType.DataRow Then
                e.Cells(1).BackColor = Drawing.Color.LightGray
                e.Cells(2).BackColor = Drawing.Color.LightGray
                e.Cells(3).BackColor = Drawing.Color.LightGray
                e.Cells(4).BackColor = Drawing.Color.LightGray
                e.Cells(5).BackColor = Drawing.Color.LightGray

                e.Cells(11).BackColor = Drawing.Color.LightGray
                e.Cells(12).BackColor = Drawing.Color.LightGray
                e.Cells(13).BackColor = Drawing.Color.LightGray
                e.Cells(14).BackColor = Drawing.Color.LightGray
                e.Cells(15).BackColor = Drawing.Color.LightGray

                e.Cells(21).BackColor = Drawing.Color.LightGray
                e.Cells(22).BackColor = Drawing.Color.LightGray
                e.Cells(23).BackColor = Drawing.Color.LightGray
                e.Cells(24).BackColor = Drawing.Color.LightGray
                e.Cells(25).BackColor = Drawing.Color.LightGray
            End If
        Next
    End Sub

    Private Sub gvMatriz_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMatriz.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim gvr As New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal)
            Dim thc As New TableHeaderCell()
            thc.RowSpan = 2
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 5
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "CAMPO"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 5
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "RMC"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 5
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "CRÍTICA"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 5
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "VERIFICACIÓN"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 5
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "CAPTURA"
            gvr.Cells.Add(thc)

            Me.gvMatriz.Controls(0).Controls.AddAt(0, gvr)


            ' SEGUNDA FILA DE DATOS
            gvr = New GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Normal)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "Planeado"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "Ejecutado"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 1
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = ""
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "Planeado"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "Ejecutado"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 1
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = ""
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "Planeado"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "Ejecutado"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 1
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = ""
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "Planeado"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "Ejecutado"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 1
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = ""
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "RecapturePerformance"
            thc.Text = "Planeado"
            gvr.Cells.Add(thc)


            thc = New TableHeaderCell()
            thc.ColumnSpan = 2
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = "Ejecutado"
            gvr.Cells.Add(thc)

            thc = New TableHeaderCell()
            thc.ColumnSpan = 1
            'thc.CssClass = "PurchaseRefiSplit"
            thc.Text = ""
            gvr.Cells.Add(thc)

            Me.gvMatriz.Controls(0).Controls.AddAt(1, gvr)

            'formato

            'If e.Row.RowType = DataControlRowType.DataRow Then
            '    e.Row.Cells(1).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(2).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(3).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(4).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(5).BackColor = Drawing.Color.LightBlue

            '    e.Row.Cells(11).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(12).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(13).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(14).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(15).BackColor = Drawing.Color.LightBlue

            '    e.Row.Cells(21).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(22).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(23).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(24).BackColor = Drawing.Color.LightBlue
            '    e.Row.Cells(25).BackColor = Drawing.Color.LightBlue
            'End If
        End If
    End Sub

    Private Sub gvMatriz_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMatriz.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For i As Integer = 5 To 25 Step 5
                If IsNumeric(e.Row.Cells(i).Text) Then
                    If CDbl(e.Row.Cells(i).Text) > 0 Then
                        e.Row.Cells(i).BackColor = Drawing.Color.LightYellow
                        e.Row.Cells(i).BorderColor = Drawing.Color.Red
                        e.Row.Cells(i).Font.Bold = True
                        e.Row.Cells(i).ForeColor = Drawing.Color.Red
                    End If
                End If
            Next
        End If

    End Sub

#End Region

End Class