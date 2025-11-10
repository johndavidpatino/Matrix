Imports CoreProject

Public Class MasterRecoleccion
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

	Private Sub btnActualizar_Click(sender As Object, e As EventArgs) Handles btnActualizar.Click
		Dim op As New OP_CuantiDapper
		Dim inicioCorteFecha = New DateTime(Now.Year, DateAdd(DateInterval.Month, -1, Now).Month, 16)
		If Now.Month = 1 Then inicioCorteFecha = DateAdd(DateInterval.Year, -1, inicioCorteFecha)
		Dim finCorteFecha = New DateTime(Now.Year, Now.Month, 15)
		op.CuantiProduccionUpdate(inicioCorteFecha, finCorteFecha, Session("IDUsuario").ToString)
	End Sub



	'Private Sub liTrabajosCuanti_ServerClick(sender As Object, e As EventArgs) Handles liTrabajosCuanti.ServerClick
	'    Session.Remove("TrabajoId")
	'    Response.Redirect("~/OP_Cuantitativo/Trabajos.aspx")
	'End Sub

	'Private Sub liTrabajosCuali_ServerClick(sender As Object, e As EventArgs) Handles liTrabajosCuali.ServerClick
	'    Session.Remove("TrabajoId")
	'    Response.Redirect("~/OP_Cualitativo/Trabajos.aspx")
	'End Sub
End Class