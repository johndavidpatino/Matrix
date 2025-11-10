Public Class Borrar
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        Dim o As New CoreProject.OP.IPSClass
        Dim info As CoreProject.OP_IPS_Revision
        info = o.TraerIPSRevisionRegistroTabla(txtid.Text)
        txtdescripcion.Text = info.DescripcionObservacion
        txtobservacion.Text = info.Observacion
        txtpregunta.Text = info.Pregunta
        txtrespuesta.Text = info.RespuestaProgramador
        txttrabajoId.Text = info.TrabajoId
    End Sub


    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim o As New CoreProject.OP.IPSClass
        'o.GuardarRegistroRevision(txttrabajoId.Text, txtpregunta.Text, txtobservacion.Text, txtdescripcion.Text, txtrespuesta.Text)
        Dim ent As New CoreProject.OP_IPS_Revision
        If IsNumeric(txtid.Text) Then
            ent = o.TraerIPSRevisionRegistroTabla(txtid.Text)
        End If
        ent.TrabajoId = txttrabajoId.Text
        ent.DescripcionObservacion = txtdescripcion.Text
        ent.Observacion = txtobservacion.Text
        o.GuardarRegistroEntidad(ent)
        txtid.Text = ent.id
    End Sub
End Class