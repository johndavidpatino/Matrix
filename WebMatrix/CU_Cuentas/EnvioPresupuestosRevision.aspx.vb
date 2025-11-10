Imports CoreProject
Imports WebMatrix.Util
Public Class EnvioPresupuestosRevision
    Inherits System.Web.UI.Page

#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("PropuestaId") IsNot Nothing Then
                hfidPropuesta.Value = Request.QueryString("PropuestaId")
                cargarPresupuestos(hfidPropuesta.Value)
            End If
        End If
    End Sub
#End Region
#Region "Metodos"
    Sub cargarPresupuestos(ByVal propuestaId As Int64)
        Dim o As New Presupuesto
        gvPresupuestos.DataSource = o.ObtenerPresupuestosParaEnviarRevisarxIdPropuesta(propuestaId)
        gvPresupuestos.DataBind()
    End Sub

    Sub EnviarEmail(propuestaId As Int64, alternativas As List(Of Integer))
        Dim oEnviarCorreo As New EnviarCorreo
        Try
            oEnviarCorreo.enviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/PresupuestosParaRevisar.aspx?propuestaId=" & propuestaId.ToString & "&alternativas=" & String.Join(",", alternativas))
        Catch ex As Exception
        End Try
    End Sub

#End Region

    Protected Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click
        Dim o As New Presupuesto
        Dim log As New LogEjecucion
        Dim flag As Boolean = False
        Dim lstAlternativas As New List(Of Integer)
        For Each row As GridViewRow In Me.gvPresupuestos.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If DirectCast(row.FindControl("chbEnviar"), CheckBox).Checked = True Then
                    flag = True
                    o.editarEnvio(Me.gvPresupuestos.DataKeys(row.RowIndex)("Id"), True)
                    log.Guardar(36, Me.gvPresupuestos.DataKeys(row.RowIndex)("Id"), Now(), Session("IDUsuario"), 6)
                    lstAlternativas.Add(gvPresupuestos.DataKeys(row.RowIndex)("Alternativa"))
                End If
            End If
        Next
        If flag = True Then
            EnviarEmail(hfidPropuesta.Value, lstAlternativas)
            cargarPresupuestos(hfidPropuesta.Value)
            ShowNotification("Presupuestos enviados", ShowNotifications.InfoNotification)
        Else
            ShowNotification("No seleccionó ningún presupuesto", ShowNotifications.ErrorNotification)
        End If
        
    End Sub

    Protected Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("Propuestas.aspx")
    End Sub
End Class