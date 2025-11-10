Imports CoreProject
Imports WebMatrix.Util
Imports System.IO
Imports System.Web.Hosting
Imports System.Net
Imports CoreProject.CIEntities
Imports CoreProject.OP
Imports System.Threading.Tasks

Public Class AjustesCostosMystery
    Inherits System.Web.UI.Page

#Region "Propiedades"

    Private _IdPropuesta As Int64
    Public Property IdPropuesta() As Int64
        Get
            Return _IdPropuesta
        End Get
        Set(ByVal value As Int64)
            _IdPropuesta = value
        End Set
    End Property

    Private _idUsuario As Int64
    Public Property idUsuario() As Int64
        Get
            Return _idUsuario
        End Get
        Set(ByVal value As Int64)
            _idUsuario = value
        End Set
    End Property

    Private _alternativa As Int64
    Public Property alternativa() As Int64
        Get
            Return _alternativa
        End Get
        Set(ByVal value As Int64)
            _alternativa = value
        End Set
    End Property


#End Region

#Region "Metodos"

    Sub limpiar()
        ddlActividades.ClearSelection()
        txtValorAct.Text = ""
        txtGMOPS.Text = ""
        txtSubContInternas.Text = ""
        txtCostoOperacion.Text = ""
        txtSubContExternas.Text = ""
        txtVrVenta.Text = ""
        txtPorcentajeGM.Text = ""
    End Sub

#End Region


#Region "Eventos"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim daCentro As New Trabajo

        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(97, UsuarioID) = False Then
            Response.Redirect("../CU_Cuentas/Propuestas.aspx")
        End If

        idUsuario = Session("IdUsuario")

        If Request.QueryString("IdPropuesta") IsNot Nothing Then
            IdPropuesta = Int64.Parse(Request.QueryString("IdPropuesta").ToString)
            hfIdPropuesta.Value = IdPropuesta
        End If

        If Request.QueryString("Alternativa") IsNot Nothing Then
            alternativa = Int64.Parse(Request.QueryString("Alternativa").ToString)
            hfIdAlternativa.Value = alternativa
        End If

        If Not IsPostBack Then
            lnkProyecto.PostBackUrl = "../CU_Cuentas/Propuestas.aspx"
            CargarActividades()
            CargarCostosActividades()
            CargarGridActividades()
        End If
    End Sub

    Protected Sub Guardar()
        Try
            Dim IQ As New IQ_MODEL
            Dim entCostosActividades As New List(Of IQ_ObtenerCostoActividades_Result)
            entCostosActividades = Session("CostosActividades")

            For i As Integer = 0 To entCostosActividades.Count - 1
                Dim entActividades As New IQ_CostoActividades
                entActividades.IdPropuesta = 6534
                entActividades.ParAlternativa = 4
                entActividades.MetCodigo = 140
                entActividades.ActCodigo = entCostosActividades.Item(i).ActCodigo
                entActividades.CaCosto = entCostosActividades.Item(i).CaCosto
                entActividades.CaUnidades = 1
                entActividades.CaDescripcionUnidades = "Valor"
                entActividades.ParNacional = 1
            Next
            AjustarCostosMystery()
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            ShowNotification("Registro guardado correctamente", ShowNotifications.InfoNotification)
        Catch ex As Exception
            ShowNotification("Ha ocurrido un error al intentar ingresar el registro - " & ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End Try
    End Sub

    Sub AjustarCostosMystery()
        Dim iqent As New IQ.Consultas
        iqent.AjustarCostosMysteryShopper(6534, 4, 140, 1, txtGMOPS.Text, txtSubContInternas.Text, txtCostoOperacion.Text, txtSubContExternas.Text, txtVrVenta.Text, txtPorcentajeGM.Text)
    End Sub

    Sub CargarActividades()
        Dim iqent As New IQ.Consultas
        Me.ddlActividades.DataSource = iqent.ActividadesList
        Me.ddlActividades.DataValueField = "ID"
        Me.ddlActividades.DataTextField = "ActNombre"
        Me.ddlActividades.DataBind()
        Me.ddlActividades.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarCostosActividades()
        Dim o As New IQ.Consultas
        Dim entCostosActividades As List(Of IQ_ObtenerCostoActividades_Result)
        entCostosActividades = o.ObtenerCostosActividades(6534, 4, 140, 1)
        Session("CostosActividades") = entCostosActividades
    End Sub

    Sub CargarGridActividades()
        Me.gvActividades.DataSource = Session("CostosActividades")
        Me.gvActividades.DataBind()
    End Sub

    Private Sub gvActividades_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvActividades.RowCommand
        If e.CommandName = "Quitar" Then
            Dim idActividad As Int32 = Int32.Parse(Me.gvActividades.DataKeys(CInt(e.CommandArgument))("ActCodigo"))
            Dim entCostosActividades As List(Of IQ_ObtenerCostoActividades_Result)
            Dim itmEnc As New IQ_ObtenerCostoActividades_Result
            entCostosActividades = Session("CostosActividades")
            For Each itm As IQ_ObtenerCostoActividades_Result In entCostosActividades
                If itm.ActCodigo = idActividad Then
                    itmEnc = itm
                End If
            Next
            entCostosActividades.Remove(itmEnc)
            Session("CostosActividades") = entCostosActividades
            CargarGridActividades()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnAddActividad_Click(sender As Object, e As System.EventArgs) Handles btnAddActividad.Click
        If ddlActividades.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar la Actividad que desea agregar!", ShowNotifications.ErrorNotification)
            ddlActividades.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        If txtValorAct.Text = "" Then
            ShowNotification("Debe ingresar un valor para la Actividad que desea agregar!", ShowNotifications.ErrorNotification)
            txtValorAct.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Dim entCostosActividades As List(Of IQ_ObtenerCostoActividades_Result)
        entCostosActividades = Session("CostosActividades")
        Dim flag As Boolean = False
        For Each itm As IQ_ObtenerCostoActividades_Result In entCostosActividades
            If itm.ActCodigo = ddlActividades.SelectedValue Then
                flag = True
            End If
        Next
        If flag = True Then
            ShowNotification("Esta Actividad ya está adicionada en los Costos", ShowNotifications.ErrorNotification)
            ddlActividades.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If
        Dim itemActividad As New IQ_ObtenerCostoActividades_Result
        itemActividad.ActCodigo = ddlActividades.SelectedValue
        itemActividad.Actividad = ddlActividades.SelectedItem.Text
        itemActividad.CaCosto = txtValorAct.Text
        entCostosActividades.Add(itemActividad)
        Session("CostosActividades") = entCostosActividades
        CargarGridActividades()
        ddlActividades.ClearSelection()
        txtValorAct.Text = ""
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnGuardar_Click(sender As Object, e As System.EventArgs) Handles btnGuardar.Click
        For Each row As GridViewRow In gvActividades.Rows
            If Not (IsNumeric(DirectCast(row.FindControl("txtValor"), TextBox).Text)) Then
                ShowNotification("El Valor de la Actividad debe ser numérico y no puede haber vacíos", ShowNotifications.ErrorNotification)
                ActivateAccordion(1, EffectActivateAccordion.NoEffect)
                Exit Sub
            End If
        Next
        Dim entCostosActividades As New List(Of IQ_ObtenerCostoActividades_Result)
        For Each row As GridViewRow In gvActividades.Rows
            entCostosActividades.Add(New IQ_ObtenerCostoActividades_Result With {.ActCodigo = gvActividades.DataKeys(row.RowIndex).Value, .Actividad = gvActividades.Rows(row.RowIndex).Cells(0).Text, .CaCosto = CInt(DirectCast(row.FindControl("txtValor"), TextBox).Text)})
        Next
        Session("CostosActividades") = entCostosActividades

        Guardar()
        limpiar()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        limpiar()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub


#End Region

End Class