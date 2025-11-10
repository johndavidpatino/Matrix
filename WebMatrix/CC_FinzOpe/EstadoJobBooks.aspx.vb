Imports WebMatrix
Imports CoreProject
Imports WebMatrix.Util
Imports System.IO

Public Class EstadoJobBooks
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
        Dim op As New ProcesosInternos
        Dim datos() As String
        datos = txtTexto.Text.Split(Chr(10))
        For i As Integer = 0 To datos.Length - 1
            If datos(i) = "" Then
                Exit For
            End If
            Dim data() As String
            data = datos(i).Split(Chr(9))
            op.ActualizarJobBooks(data(0), data(1))
        Next
        ShowNotification("Cambios realizados", ShowNotifications.InfoNotification)
        ActivateAccordion(0)
    End Sub

End Class