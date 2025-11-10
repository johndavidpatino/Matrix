Imports CoreProject
Imports WebMatrix.Util
Public Class Evaluacion_Facturas_Operaciones
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Private _idOrden As Int64?
    Public Property idOrden() As Int64?
        Get
            Return _idOrden
        End Get
        Set(ByVal value As Int64?)
            _idOrden = value
        End Set
    End Property
    Private _idFactura As Int64?
    Public Property idFactura() As Int64?
        Get
            Return _idFactura
        End Get
        Set(ByVal value As Int64?)
            _idFactura = value
        End Set
    End Property


#End Region


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim daContratista As New Contratista
        Dim daTrabajo As New Trabajo


        If Not (Request.QueryString("IdOrden") Is Nothing) Then
            idOrden = If(Int64.TryParse(Request.QueryString("IdOrden"), New Int64), CType(Request.QueryString("IdOrden"), Int64?), CType(Nothing, Int64?))
        End If

        'HACK Se coloca este codigo debido a un error cometido, se debe quitar aproximadamente el 01/03/2016
        If Not (Request.QueryString("IdFActura") Is Nothing) Then
            idOrden = If(Int64.TryParse(Request.QueryString("IdFActura"), New Int64), CType(Request.QueryString("IdFActura"), Int64?), CType(Nothing, Int64?))
        End If


        Dim oPI As New ProcesosInternos
        Dim lstFacturas = oPI.OrdenesdeServicioGet(Nothing, idOrden, Nothing)

        If lstFacturas.Count < 1 Then
            pnlAvisoFactura.Visible = True
            pnlEvaluacion.Visible = False
            pnlGracias.Visible = False
        Else
            Dim oContratista = daContratista.ObtenerContratista(lstFacturas(0).ContratistaId)
            Dim oTrabajo = daTrabajo.DevolverxID(lstFacturas(0).TrabajoId)
            idFactura = oPI.RecepcionCuentasGet(Nothing, idOrden).FirstOrDefault.Id

            If oPI.obtenerCalificacionesEvaluacion(idOrden).Count > 0 Then
                pnlAvisoFactura.Visible = True
                pnlEvaluacion.Visible = False
                pnlGracias.Visible = False
            Else
                pnlAvisoFactura.Visible = False
                pnlEvaluacion.Visible = True
                pnlGracias.Visible = False
                lblNombreProveedor.Text = oContratista.Nombre
                lblNombreTrabajo.Text = oTrabajo.NombreTrabajo
            End If
        End If

    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub btnEnviar_Click(sender As Object, e As EventArgs) Handles btnEnviar.Click
        GuardarCalificacion(1, Me.RBP1.SelectedValue, Nothing)
        If txtP1.Text <> "" Then
            GuardarCalificacion(2, Nothing, txtP1.Text)
        End If
        GuardarCalificacion(3, Me.RBP2.SelectedValue, Nothing)
        If txtP2.Text <> "" Then
            GuardarCalificacion(4, Nothing, txtP2.Text)
        End If
        GuardarCalificacion(5, Me.RBP3.SelectedValue, Nothing)
        If txtP3.Text <> "" Then
            GuardarCalificacion(6, Nothing, txtP3.Text)
        End If
        GuardarCalificacion(7, Me.RBP4.SelectedValue, Nothing)
        If txtP4.Text <> "" Then
            GuardarCalificacion(8, Nothing, txtP4.Text)
        End If
        GuardarCalificacion(9, Me.RBP5.SelectedValue, Nothing)
        If txtP5.Text <> "" Then
            GuardarCalificacion(10, Nothing, txtP5.Text)
        End If
        GuardarCalificacion(11, Me.RBP6.SelectedValue, Nothing)
        If txtP6.Text <> "" Then
            GuardarCalificacion(12, Nothing, txtP6.Text)
        End If
        If txtP7.Text <> "" Then
            GuardarCalificacion(13, Nothing, txtP7.Text)
        End If
        limpiar()
        pnlEvaluacion.Visible = False
        pnlAvisoFactura.Visible = False
        pnlGracias.Visible = True
    End Sub

    Sub GuardarCalificacion(ByVal item As Integer, ByVal calificacion As Integer?, abierta As String)
        Dim o As New ProcesosInternos
        Dim e As New CO_EvaluacionProveedoresFacturaOP
        e.IdFactura = idFactura
        e.Usuario = hfSolicitante.Value
        e.Fecha = Date.UtcNow.AddHours(-5)
        e.Item = item
        e.Calificacion = calificacion
        e.Abierta = abierta
        e.Usuario = Session("IDUsuario")
        o.GuardarCalificacionEvaluacionProveedor(e)
    End Sub
    Sub limpiar()
        Me.RBP1.ClearSelection()
        Me.txtP1.Text = ""
        Me.txtP1.Visible = False
        Me.RBP2.ClearSelection()
        Me.txtP2.Text = ""
        Me.txtP2.Visible = False
        Me.RBP3.ClearSelection()
        Me.txtP3.Text = ""
        Me.txtP3.Visible = False
        Me.RBP4.ClearSelection()
        Me.txtP4.Text = ""
        Me.txtP4.Visible = False
        Me.RBP5.ClearSelection()
        Me.txtP5.Text = ""
        Me.txtP5.Visible = False
        Me.RBP6.ClearSelection()
        Me.txtP6.Text = ""
        Me.txtP6.Visible = False
    End Sub

    Private Sub RBP1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP1.SelectedIndexChanged
        If RBP1.SelectedValue > 0 AndAlso RBP1.SelectedValue < 8 Then
            pnlTxtP1.Visible = True
        Else
            pnlTxtP1.Visible = False
            txtP1.Text = ""
        End If
    End Sub
    Private Sub RBP2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP2.SelectedIndexChanged
        If RBP2.SelectedValue > 0 AndAlso RBP2.SelectedValue < 8 Then
            pnlTxtP2.Visible = True
        Else
            pnlTxtP2.Visible = False
            txtP2.Text = ""
        End If
    End Sub
    Private Sub RBP3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP3.SelectedIndexChanged
        If RBP3.SelectedValue > 0 AndAlso RBP3.SelectedValue < 8 Then
            pnlTxtP3.Visible = True
        Else
            pnlTxtP3.Visible = False
            txtP3.Text = ""
        End If
    End Sub
    Private Sub RBP4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP4.SelectedIndexChanged
        If RBP4.SelectedValue > 0 AndAlso RBP4.SelectedValue < 8 Then
            pnlTxtP4.Visible = True
        Else
            pnlTxtP4.Visible = False
            txtP4.Text = ""
        End If
    End Sub
    Private Sub RBP5_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP5.SelectedIndexChanged
        If RBP5.SelectedValue > 0 AndAlso RBP5.SelectedValue < 8 Then
            pnlTxtP5.Visible = True
        Else
            pnlTxtP5.Visible = False
            txtP5.Text = ""
        End If
    End Sub
    Private Sub RBP6_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RBP6.SelectedIndexChanged
        If RBP6.SelectedValue > 0 AndAlso RBP6.SelectedValue < 8 Then
            pnlTxtP6.Visible = True
        Else
            pnlTxtP6.Visible = False
            txtP6.Text = ""
        End If
    End Sub
End Class