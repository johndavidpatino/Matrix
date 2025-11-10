Imports CoreProject
Public Class UC_Producto
    Inherits System.Web.UI.UserControl
    Dim _P As IQ.UCPreguntas
    Public Event CargarPreguntas As System.EventHandler
    Public tecnica As String
    Public unidad As Integer
    Dim CC As New IQ.ControlCostos()

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            cargarOfertas()
            CerradasReal.Text = "0"
            CerradasMultReal.Text = "0"
            AbiertasReal.Text = "0"
            AbiertasMultReal.Text = "0"
            OtrosReal.Text = "0"
            DemoReal.Text = "0"

            CargarUnidades()


        End If



    End Sub

    Private Sub cargarOfertas()
        Try
            _P = New IQ.UCPreguntas()
            Dim lstProds As List(Of ObtenerOfertas_Result)

            lstProds = _P.ObtenerOferta(unidad)
            LstOferta.DataSource = lstProds
            LstOferta.DataTextField = "Pr_OfferingDescription"
            LstOferta.DataValueField = "Pr_Offeringcode"
            LstOferta.DataBind()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub cargarOfertasHist()
        Try
            _P = New IQ.UCPreguntas()
            Dim lstProds As List(Of ObtenerOfertas_Result)

            lstProds = _P.ObtenerOferta(CInt(lstUnidad_hist.SelectedValue))
            lstOferta_Hist.DataSource = lstProds
            lstOferta_Hist.DataTextField = "Pr_OfferingDescription"
            lstOferta_Hist.DataValueField = "Pr_Offeringcode"
            lstOferta_Hist.DataBind()

        Catch ex As Exception

        End Try
    End Sub


    Public Sub cargarProductos()
        _P = New IQ.UCPreguntas()
        Dim Oferta As String = LstOferta.SelectedValue
        Dim lstProds = _P.ObtenerProductosPorUnidad(Oferta)
        lstProducto.DataSource = lstProds
        lstProducto.DataTextField = "Pr_ProductDescription"
        lstProducto.DataValueField = "Pr_ProductCode"
        lstProducto.DataBind()

    End Sub

    Public Sub cargarProductosHist()
        _P = New IQ.UCPreguntas()
        Dim Oferta As String = lstOferta_Hist.SelectedValue
        Dim lstProds = _P.ObtenerProductosPorUnidad(Oferta)
        lstProducto_Hist.DataSource = lstProds
        lstProducto_Hist.DataTextField = "Pr_ProductDescription"
        lstProducto_Hist.DataValueField = "Pr_ProductCode"
        lstProducto_Hist.DataBind()

    End Sub


    Private Sub CargarUnidades()
        lstUnidad_hist.DataSource = CC.ObtenerUnidades()
        lstUnidad_hist.DataTextField = "GrupoUnidad"
        lstUnidad_hist.DataValueField = "CodContable"
        lstUnidad_hist.DataBind()
    End Sub
    Protected Sub lstOferta_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles LstOferta.SelectedIndexChanged

        If LstOferta.SelectedIndex > 0 Then
            cargarProductos()
        End If

    End Sub

    Public Sub lstProducto_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstProducto.SelectedIndexChanged
        'Dim UcPreguntas As UC_Preguntas1
        If lstProducto.SelectedIndex > 0 Then
            Dim Prod As New IQ_Productos
            _P = New IQ.UCPreguntas()
            Prod.Pr_ProductCode = lstProducto.SelectedValue
            Prod = _P.ObtenerPreguntasProducto(Prod)

            CerradasReal.Text = "0"
            CerradasMultReal.Text = "0"
            AbiertasReal.Text = "0"
            AbiertasMultReal.Text = "0"
            OtrosReal.Text = "0"

            CerradasProp.Text = Prod.Pr_PregCerradas.ToString()
            CerradasMultProp.Text = Prod.Pr_PregCerradasMult.ToString()
            AbiertasProp.Text = Prod.Pr_PregAbiertas.ToString()
            AbiertasMultProp.Text = Prod.Pr_PregAbiertasMult.ToString()
            OtrosProp.Text = Prod.Pr_PregOtros.ToString()
            DemoProp.Text = Prod.Pr_Demograficos.ToString()



            DemoReal.Text = Prod.Pr_Demograficos.ToString()


        End If

    End Sub


    Public Sub CargarPreguntasPropuestas()
        If lstProducto.SelectedIndex > 0 Then
            'Dim UcPreguntas As UC_Preguntas1
            Dim Prod As New IQ_Productos
            _P = New IQ.UCPreguntas()
            Prod.Pr_ProductCode = lstProducto.SelectedValue
            Prod = _P.ObtenerPreguntasProducto(Prod)

            CerradasProp.Text = Prod.Pr_PregCerradas.ToString()
            CerradasMultProp.Text = Prod.Pr_PregCerradasMult.ToString()
            AbiertasProp.Text = Prod.Pr_PregAbiertas.ToString()
            AbiertasMultProp.Text = Prod.Pr_PregAbiertasMult.ToString()
            OtrosProp.Text = Prod.Pr_PregOtros.ToString()
            DemoProp.Text = Prod.Pr_Demograficos.ToString()


        End If
    End Sub

    Protected Sub CerradasReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CerradasReal.TextChanged
        _P = New IQ.UCPreguntas
        Me.txtDuracion.Text = _P.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        CerradasReal.Text = CerradasReal.Text
        CerradasMultReal.Focus()
    End Sub

    Protected Sub CerradasMultReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CerradasMultReal.TextChanged
        _P = New IQ.UCPreguntas
        Me.txtDuracion.Text = _P.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        CerradasMultReal.Text = CerradasMultReal.Text
        AbiertasReal.Focus()
    End Sub

    Protected Sub AbiertasReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles AbiertasReal.TextChanged
        _P = New IQ.UCPreguntas
        Me.txtDuracion.Text = _P.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        AbiertasReal.Text = AbiertasReal.Text
        AbiertasMultReal.Focus()
    End Sub

    Protected Sub AbiertasMultReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles AbiertasMultReal.TextChanged
        _P = New IQ.UCPreguntas
        Me.txtDuracion.Text = _P.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        AbiertasMultReal.Text = AbiertasMultReal.Text
        OtrosReal.Focus()
    End Sub

    Protected Sub OtrosReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles OtrosReal.TextChanged
        _P = New IQ.UCPreguntas
        Me.txtDuracion.Text = _P.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        OtrosReal.Text = OtrosReal.Text
        DemoReal.Focus()
    End Sub

    Protected Sub DemoReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DemoReal.TextChanged
        _P = New IQ.UCPreguntas
        Me.txtDuracion.Text = _P.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        DemoReal.Text = DemoReal.Text
    End Sub

    Protected Sub btnHitorial_Click(sender As Object, e As EventArgs) Handles btnHitorial.Click

    End Sub

    Protected Sub btnBuscarHist_Click(sender As Object, e As EventArgs) Handles btnBuscarHist.Click
        CargarHistorialPreguntas()
        btnHitorial_ModalPopupExtender.Show()

    End Sub

    Private Sub CargarHistorialPreguntas()
        Dim jb, Prod, Nom As String
        Dim Unidad As Integer
        _P = New IQ.UCPreguntas()
        jb = If(txtJobbook_Hist.Text.Trim = String.Empty, Nothing, txtJobbook_Hist.Text)
        Prod = If((lstProducto_Hist.SelectedIndex = 0 Or lstProducto_Hist.Items.Count = 0), Nothing, lstProducto_Hist.SelectedValue)
        Nom = If(txtNombres_Hist.Text.Trim = String.Empty, Nothing, txtNombres_Hist.Text)
        Unidad = If(lstUnidad_hist.SelectedIndex = 0, Nothing, CInt(lstUnidad_hist.SelectedValue))
        gvHistPreg.DataSource = _P.ObtenerPreguntasHistoricos(jb, Unidad, Prod, Nom)
        gvHistPreg.DataBind()
    End Sub

    Protected Sub lstOferta_Hist_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstOferta_Hist.SelectedIndexChanged

        If lstOferta_Hist.SelectedIndex > 0 Then
            cargarProductosHist()
            btnHitorial_ModalPopupExtender.Show()
        End If
    End Sub

    Protected Sub lstUnidad_hist_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstUnidad_hist.SelectedIndexChanged
        cargarOfertasHist()
        btnHitorial_ModalPopupExtender.Show()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click

        btnHitorial_ModalPopupExtender.Hide()
    End Sub

    Protected Sub gvHistPreg_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvHistPreg.PageIndexChanging
        gvHistPreg.PageIndex = e.NewPageIndex
        CargarHistorialPreguntas()
        btnHitorial_ModalPopupExtender.Show()
    End Sub

    Protected Sub gvHistPreg_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvHistPreg.RowCommand
        Select Case e.CommandName
            Case "SEL"
                CerradasReal.Text = gvHistPreg.Rows(e.CommandArgument).Cells(1).Text
                CerradasMultReal.Text = gvHistPreg.Rows(e.CommandArgument).Cells(2).Text
                AbiertasReal.Text = gvHistPreg.Rows(e.CommandArgument).Cells(3).Text
                AbiertasMultReal.Text = gvHistPreg.Rows(e.CommandArgument).Cells(4).Text
                OtrosReal.Text = gvHistPreg.Rows(e.CommandArgument).Cells(5).Text
                DemoReal.Text = gvHistPreg.Rows(e.CommandArgument).Cells(6).Text
                _P = New IQ.UCPreguntas
                Me.txtDuracion.Text = _P.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
                CerradasReal.Text = CerradasReal.Text
                CerradasMultReal.Focus()
                lstProducto.SelectedIndex = 0
                lstUnidad_hist.SelectedIndex = 0
                lstOferta_Hist.SelectedIndex = 0
                txtJobbook_Hist.Text = ""
                txtNombres_Hist.Text = ""

                gvHistPreg.DataSource = Nothing
                gvHistPreg.DataBind()

        End Select
    End Sub
End Class