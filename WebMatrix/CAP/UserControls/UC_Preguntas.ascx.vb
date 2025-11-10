
Imports CoreProject

Public Class UC_Preguntas1
    Inherits System.Web.UI.UserControl
    Dim ucpreg As IQ.UCPreguntas

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            CerradasReal.Text = "0"
            CerradasMultReal.Text = "0"
            AbiertasReal.Text = "0"
            AbiertasMultReal.Text = "0"
            OtrosReal.Text = "0"
            DemoReal.Text = "0"
        End If

    End Sub


    Protected Sub CerradasReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CerradasReal.TextChanged
        ucpreg = New IQ.UCPreguntas
        Me.txtDuracion.Text = ucpreg.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        CerradasPresup.Text = CerradasReal.Text
        CerradasMultReal.Focus()
    End Sub

    Protected Sub CerradasMultReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles CerradasMultReal.TextChanged
        ucpreg = New IQ.UCPreguntas
        Me.txtDuracion.Text = ucpreg.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        CerradasMultPresup.Text = CerradasMultReal.Text
        AbiertasReal.Focus()
    End Sub

    Protected Sub AbiertasReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles AbiertasReal.TextChanged
        ucpreg = New IQ.UCPreguntas
        Me.txtDuracion.Text = ucpreg.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        AbiertasPresup.Text = AbiertasReal.Text
        AbiertasMultReal.Focus()
    End Sub

    Protected Sub AbiertasMultReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles AbiertasMultReal.TextChanged
        ucpreg = New IQ.UCPreguntas
        Me.txtDuracion.Text = ucpreg.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        AbiertasMultPresup.Text = AbiertasMultReal.Text
        OtrosReal.Focus()
    End Sub

    Protected Sub OtrosReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles OtrosReal.TextChanged
        ucpreg = New IQ.UCPreguntas
        Me.txtDuracion.Text = ucpreg.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        OtrosPresup.Text = OtrosReal.Text
        DemoReal.Focus()
    End Sub

    Protected Sub DemoReal_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles DemoReal.TextChanged
        ucpreg = New IQ.UCPreguntas
        Me.txtDuracion.Text = ucpreg.CalcularTiempo(CInt(CerradasReal.Text), CInt(CerradasMultReal.Text), CInt(AbiertasReal.Text), CInt(AbiertasMultReal.Text), CInt(OtrosReal.Text), CInt(DemoReal.Text))
        DemoPresup.Text = DemoReal.Text
    End Sub
End Class

