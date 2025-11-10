Imports CoreProject

Public Class Captura

    Inherits System.Web.UI.Page
    Dim OP As New OP.RMC
    Dim OPC As New OP.Critica
    Dim OPV As New OP.Verificacion
    Private _id As Integer
    Private _cant As Integer

#Region "Propiedades"
    Public Property IdRegistro As Integer
        Get
            Return ViewState("_id")
        End Get
        Set(ByVal value As Integer)
            ViewState("_id") = value
        End Set
    End Property
    Public Property Cantidad As Integer
        Get
            Return ViewState("_cant")
        End Get
        Set(ByVal value As Integer)
            ViewState("_cant") = value
        End Set
    End Property
#End Region

#Region "Funciones y Métodos"
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
    Public Function ObtenerTE(ByVal TrabajoId) As List(Of OP_TraficoEncuesta_GetCritica_Result)
        Try
            Return OPC.ObtenerTraficoEncuestasCriticaXTrabajo(TrabajoId, 21)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Session("TrabajoId") = 1
            gvDatos.DataSource = ObtenerTE(CInt(Session("TrabajoId")))
            gvDatos.DataBind()
            CargarCombo(ddlUnidadRecibe, "id", "Unidad", ObtenerUnidad())
            ddlUnidadRecibe.SelectedValue = 21
        End If
    End Sub
    Protected Sub gvDatos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvDatos.RowCommand
        Try
            Select Case e.CommandName
                Case "Editar"
                    IdRegistro = gvDatos.DataKeys(CInt(e.CommandArgument))("Id")
                    Cantidad = gvDatos.DataKeys(CInt(e.CommandArgument))("Cantidad")
                    datos.Visible = True
                    lista.Visible = False
            End Select
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            If chkDevolucion.Checked = True Then
                Session("IdUsuario") = 80243742
                OPV.EditarTraficoEncuestasVerificacion(IdRegistro, Cantidad - CInt(txtCantidadElegir.Text), Session("IdUsuario"), 26, ddlUnidad.SelectedValue, DateTime.Now, txtObservaciones.Content, chkDevolucion.Checked, txtMotivoDevolucion.Content)
                Session("TrabajoId") = 1
            Else
                Session("IdUsuario") = 80243742
                OPC.EditarTraficoEncuestasCritica(IdRegistro, Cantidad - CInt(txtCantidadElegir.Text), Session("IdUsuario"), ddlUnidadRecibe.SelectedValue, ddlUnidad.SelectedValue, DateTime.Now, txtObservaciones.Content)
                Session("TrabajoId") = 1
            End If
            gvDatos.DataSource = ObtenerTE(CInt(Session("TrabajoId")))
            gvDatos.DataBind()
            datos.Visible = False
            lista.Visible = True
        Catch ex As Exception
            lblResult.Text = ex.Message
        End Try
    End Sub
    Protected Sub txtCantidadElegir_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCantidadElegir.TextChanged
        If Cantidad < CInt(txtCantidadElegir.Text) Then
            lblResult.Text = "la cantidad no puede ser mayor a la enviada"
            btnGuardar.Enabled = False
        Else
            btnGuardar.Enabled = True
            lblResult.Text = ""
        End If
    End Sub
    Protected Sub chkDevolucion_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkDevolucion.CheckedChanged
        If chkDevolucion.Checked = True Then
            DvMotivoDev.Visible = True
        Else
            DvMotivoDev.Visible = False
        End If
    End Sub
#End Region
End Class