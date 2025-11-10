Imports CoreProject.GestionCampo
Public Class SeleccionarPreguntasTabular
    Inherits System.Web.UI.Page
    Dim gc As New CoreProject.GestionCampo.GC_Tabulacion
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            CargarEstudios()
        End If
    End Sub
    Private Sub CargarEstudios()
        lstEstudios.DataSource = gc.ObtenerEstudios().Tables(0)
        lstEstudios.DataTextField = "E_Descripcion"
        lstEstudios.DataValueField = "E_Id"
        lstEstudios.DataBind()
        Dim li As New ListItem() With {.Text = "Seleccione...", .Value = "0"}

        lstEstudios.Items.Insert(0, li)


    End Sub

    Protected Sub btnTabular_Click(sender As Object, e As EventArgs) Handles btnTabular.Click
        If lstEstudios.SelectedIndex > 0 Then
            Dim DSD As DataSet
            DSD = gc.ObtenerRespuestasTabular(CDec(lstEstudios.SelectedValue))
            Session("TABULAR_DATA") = DSD
            Response.Redirect("TabularEstudios.aspx?ESTUDIO=" & lstEstudios.SelectedValue)
        End If
    End Sub

    Protected Sub lstEstudios_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstEstudios.SelectedIndexChanged
        If lstEstudios.SelectedIndex > 0 Then
            CargarNombresPreguntas()
            CargarPregTabularEstudio()
            UpdatePanel2.Update()
        End If
    End Sub

    Private Sub CargarNombresPreguntas()
        lstPreguntas.DataSource = gc.ObtenerNombresPreguntas(CDec(lstEstudios.SelectedValue)).Tables(0)
        lstPreguntas.DataTextField = "Pr_Texto"
        lstPreguntas.DataValueField = "Pr_Id"
        lstPreguntas.DataBind()
        Dim li As New ListItem() With {.Text = "Seleccione...", .Value = "0"}
        lstPreguntas.Items.Insert(0, li)

    End Sub

    Private Sub CargarPregTabularEstudio()
        gvPregTab.DataSource = gc.ObtenerPreguntasTabularEstudio(CDec(lstEstudios.SelectedValue))
        gvPregTab.DataBind()

    End Sub

    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click

        If lstEstudios.SelectedIndex > 0 And lstPreguntas.SelectedIndex > 0 Then
            gc.InsertarPreguntasTabular(CDec(lstEstudios.SelectedValue), CDec(lstPreguntas.SelectedValue))
            CargarPregTabularEstudio()
            UpdatePanel2.Update()
        End If

    End Sub

    Protected Sub gvPregTab_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPregTab.RowCommand

        Select Case e.CommandName
            Case "DEL"
                Dim Est, Preg As Decimal
                Est = CDec(gvPregTab.Rows(e.CommandArgument).Cells(0).Text)
                Preg = CDec(gvPregTab.Rows(e.CommandArgument).Cells(1).Text)
                gc.BorrarPreguntaTabular(Est, Preg)
                UpdatePanel2.Update()

        End Select




    End Sub
End Class