Imports CoreProject
Imports WebMatrix.Util
Public Class ListadoCuentasRecibidas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ListadoCuentasCobro(Nothing, Nothing)
            Estados()
        End If
    End Sub
    Private Sub GvListadoCuentas_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvListadoCuentas.PageIndexChanging
        GvListadoCuentas.PageIndex = e.NewPageIndex
        ListadoCuentasCobro(Nothing, Nothing)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub GvListadoCuentas_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GvListadoCuentas.RowCommand
        If e.CommandName = "Detalle" Then
            hfidorden.Value = Int64.Parse(Me.GvListadoCuentas.DataKeys(CInt(e.CommandArgument))("OrdenId"))
            ListadoCuentasCobroXId(Nothing, hfidorden.Value)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "Eliminar" Then
            EliminarRadicado(Int64.Parse(Me.GvListadoCuentas.DataKeys(CInt(e.CommandArgument))("OrdenId")))
            ShowNotification("Registro Eliminado", ShowNotifications.InfoNotification)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            ListadoCuentasCobro(Nothing, Nothing)
        End If

    End Sub

    Sub EliminarRadicado(ByVal Consecutivo As Int64)
        Dim op As New ProcesosInternos
        op.EliminarRadicadoCuenta(Consecutivo)
    End Sub

    Sub ListadoCuentasCobro(ByVal ContratistaId As Int64?, ByVal OrdenId As Int64?)
        Dim Op As New ProcesosInternos
        Me.GvListadoCuentas.DataSource = Op.CuentasRadicadasGet(ContratistaId, OrdenId).ToList
        Me.GvListadoCuentas.DataBind()
    End Sub
    Sub ListadoCuentasCobroXId(ByVal ContratistaId As Int64?, ByVal OrdenId As Int64?)
        Dim Op As New ProcesosInternos
        Me.GvDetalleCuenta.DataSource = Op.CuentasRadicadasGet(ContratistaId, OrdenId).ToList
        Me.GvDetalleCuenta.DataBind()
    End Sub

    Sub Estados()
        Dim op As New ProcesosInternos
        ddlestado.DataSource = op.EstadosCuentasRecibidas()
        ddlestado.DataValueField = "id"
        ddlestado.DataTextField = "Estado"
        ddlestado.DataBind()
    End Sub
    Sub ActualizarEstado(ByVal Estadoid As Int64, ByVal Consecutivo As Int64)
        Dim op As New ProcesosInternos
        op.ActualizarEstadoCuentarecibida(Estadoid, Consecutivo)

    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        ActualizarEstado(ddlestado.SelectedValue, hfidorden.Value)
        ShowNotification("Estado Actualizado", ShowNotifications.InfoNotification)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If txtOrdenBuscar.Text = "" And txtIdentificacionBuscar.Text = "" Then
            ListadoCuentasCobro(Nothing, Nothing)
        ElseIf txtOrdenBuscar.Text = "" Then
            ListadoCuentasCobro(txtIdentificacionBuscar.Text, Nothing)
        ElseIf txtIdentificacionBuscar.Text = "" Then
            ListadoCuentasCobro(Nothing, txtOrdenBuscar.Text)

        End If
    End Sub
End Class