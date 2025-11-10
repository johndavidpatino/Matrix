Imports CoreProject

Public Class RMC
    Inherits System.Web.UI.Page

#Region "Funciones y Métodos"""
    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub
    Public Function ObtenerUnidad() As List(Of UnidadCombo_Result)
        Dim Data As New SG.Actas
        Try
            Return Data.ObtenerUnidad
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Eventos del Control"
    Protected Sub txtCantidadElegir_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCantidadElegir.TextChanged
        Try
            If CInt(txtCuenta.Text) < CInt(txtCantidadElegir.Text) Then
                txtCantidadElegir.Text = 0
            End If
        Catch ex As Exception
            txtCantidadElegir.Text = 0
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Dim OP As New OP.RMC
        Session("IdUsuario") = Session("IDUsuario").ToString
        Try
            OP.AgregarTraficoEncuestasRMC(txtTrabajoId.Text, txtRCiudad.Text, txtCantidadElegir.Text, Session("IdUsuario"), ddlUnidad.SelectedValue, ddlUnidadRecibe.SelectedValue, DateTime.Now, txtObservaciones.Text)
            lblResult.Text = "RMC Generada"
            btnGuardar.Enabled = False
        Catch ex As Exception
            lblResult.Text = ex.Message
        End Try
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("TrabajoId") IsNot Nothing Then
                Dim TrabajoId As Int64 = Int64.Parse(Request.QueryString("TrabajoId").ToString)
                txtTrabajoId.Text = TrabajoId
                txtRCiudad.Text = Request.QueryString("Ciudad")
                txtCuenta.Text = Request.QueryString("Cuenta")
                CargarCombo(ddlUnidad, "id", "Unidad", ObtenerUnidad())
                CargarCombo(ddlUnidadRecibe, "id", "Unidad", ObtenerUnidad())
                ddlUnidad.SelectedValue = 38
                ddlUnidadRecibe.SelectedValue = 26
            Else
                Response.Redirect("TrabajosProyectos.aspx")
            End If
        End If
    End Sub
#End Region
End Class