Imports CoreProject
Imports WebMatrix.Util
Imports System.Threading.Tasks

Public Class RecepcionCuentasdeCobro
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(132, UsuarioID) = False Then
            Response.Redirect("../FI_AdministrativoFinanciero/Default.aspx")
        End If
        If Not IsPostBack Then
            ObtenerOrdenes(Nothing, Nothing)
            CargarTipoDocumento()
            CargarRadicados(Nothing)
        End If
    End Sub

    Public Function Obtenertrabajo(ByVal TrabajoId As String)
        Dim oTrabajo As New Trabajo
        Dim oeTrabajo = oTrabajo.ObtenerTrabajoXJob(TrabajoId)
        Return oeTrabajo
    End Function
    Public Function ObtenerContratista(ByVal Id As Int64)
        Dim oContratista As New Contratista
        Dim info As New TH_Contratistas
        info = oContratista.ObtenerContratista(Id)
        Return info
    End Function

    Sub ObtenerOrdenes(ByVal Id As Int64?, ByVal Trabajoid As Int64?)
        Dim op As New ProcesosInternos
        GvOrdenes.DataSource = op.OrdenesdeServicioGet(Trabajoid, Id, Nothing)
        GvOrdenes.DataBind()
    End Sub
    Sub RecepcionCuentaadd(ByVal Consecutivo As Int64, ByVal Cantidad As Double, ByVal VrUnitario As Double, ByVal VrTotal As Double, ByVal Observacion As String, ByVal TipoDocumento As Int64, ByVal OrdenId As Int64, ByVal UsuarioId As Int64)
        Dim op As New ProcesosInternos
        Dim oEnviarCorreo As New EnviarCorreo
        Dim idFactura As Decimal?
        Try
            idFactura = op.RecepcionCuentadeCobroadd(Consecutivo, Cantidad, VrUnitario, VrTotal, Observacion, TipoDocumento, OrdenId, UsuarioId)
        Catch ex As Exception
            ShowNotification(ex.Message, ShowNotifications.ErrorNotification)
            ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
        End Try
    End Sub
    Sub CargarTipoDocumento()
        Dim op As New ProcesosInternos
        ddltipodocumento.DataSource = op.TipoDocumentoGet()
        ddltipodocumento.DataValueField = "id"
        ddltipodocumento.DataTextField = "TipoDocumento"
        ddltipodocumento.DataBind()
        ddltipodocumento.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarRadicados(ByVal Consecutivo As Int64?)
        Dim op As New ProcesosInternos
        GvRadicados.DataSource = op.RecepcionCuentasGet(Consecutivo, Nothing)
        GvRadicados.DataBind()
    End Sub

    Private Sub GvOrdenes_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GvOrdenes.RowCommand
        Dim Orden As New List(Of CC_OrdenesdeServicioGet_Result)
        Dim RecepcionCuentas As New List(Of CC_RecepcionCuentasdeCobroGet_Result)
        Dim op As New ProcesosInternos
        If e.CommandName = "Grabar" Then
            hfidOrden.Value = Int64.Parse(Me.GvOrdenes.DataKeys(CInt(e.CommandArgument))("Id"))
            RecepcionCuentas = op.RecepcionCuentasGet(Nothing, hfidOrden.Value)
            Orden = op.OrdenesdeServicioGet(Nothing, hfidOrden.Value, Nothing)

            Dim o = op.OrdenesdeServicioGet(Nothing, hfidOrden.Value, Nothing).FirstOrDefault

            If Orden.Count > 0 And o.PorcentajeAnticipo > 0 Then
                If RecepcionCuentas.Count > 0 Then
                    Dim Cantidad As Double = 0
                    For x = 0 To RecepcionCuentas.Count - 1
                        If RecepcionCuentas.Item(x).Estado <> "Anulada" Then
                            Cantidad = Cantidad + 1
                        End If
                    Next

                    If Cantidad = 2 Then
                        ShowNotification("Ya ingreso Cuenta para esa Orden!!!", ShowNotifications.InfoNotification)
                        Exit Sub
                    Else
                        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                        btnGuardar.Text = "Guardar"
                        txtconsecutivo.Focus()
                    End If
                Else
                    If op.OrdenesdeServicioGet(Nothing, hfidOrden.Value, Nothing).FirstOrDefault.Estado = ProcesosInternos.EEstadosOrdenes.Anular Then
                        ShowNotification("Esta orden se encuentra anulada, por lo cual no se pueden asociar cuentas de cobro a ella", ShowNotifications.InfoNotification)
                        Exit Sub
                    Else
                        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                        btnGuardar.Text = "Guardar"
                        txtconsecutivo.Focus()
                    End If

                End If

            ElseIf Orden.Count > 0 And (o.PorcentajeAnticipo = 0 Or o.PorcentajeAnticipo Is Nothing) Then
                If RecepcionCuentas.Count > 0 Then
                    If RecepcionCuentas.Item(0).Estado <> "Anulada" Then
                        ShowNotification("Ya ingreso Cuenta para esa Orden!!!", ShowNotifications.InfoNotification)
                        Exit Sub
                    Else
                        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                        btnGuardar.Text = "Guardar"
                        txtconsecutivo.Focus()
                    End If
                Else
                    If op.OrdenesdeServicioGet(Nothing, hfidOrden.Value, Nothing).FirstOrDefault.Estado = ProcesosInternos.EEstadosOrdenes.Anular Then
                        ShowNotification("Esta orden se encuentra anulada, por lo cual no se pueden asociar cuentas de cobro a ella", ShowNotifications.InfoNotification)
                        Exit Sub
                    Else
                        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
                        btnGuardar.Text = "Guardar"
                        txtconsecutivo.Focus()
                    End If
                End If
            End If

        End If
    End Sub

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Dim op As New ProcesosInternos
        Dim oEnviarCorreo As New EnviarCorreo
        If btnGuardar.Text = "Guardar" Then
            If op.RecepcionCuentasGet(txtconsecutivo.Text, Nothing).Count > 0 Then
                ShowNotification("Consecutivo ya Registrado!!!", ShowNotifications.InfoNotification)
                Exit Sub
            End If

            For i = 0 To GvDetalle.Rows.Count - 1
                RecepcionCuentaadd(GvDetalle.Rows(i).Cells(0).Text, Replace(GvDetalle.Rows(i).Cells(1).Text, ".", ","), Replace(GvDetalle.Rows(i).Cells(2).Text, ".", ","), Replace(GvDetalle.Rows(i).Cells(3).Text, ".", ","), GvDetalle.Rows(i).Cells(4).Text, GvDetalle.Rows(i).Cells(5).Text, hfidOrden.Value, Session("IDUsuario"))
            Next
            oEnviarCorreo.AsyncEnviarCorreo(WebMatrix.Util.obtenerUrlRaiz() & "/Emails/EvaluacionProveedorOP.aspx?IdOrden=" & hfidOrden.Value)
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
            ShowNotification("Registro Guardado", ShowNotifications.InfoNotification)
            Limpiar()
            txtconsecutivo.Text = String.Empty
            CargarRadicados(Nothing)
        ElseIf btnGuardar.Text = "Actualizar" Then
            ActualizarRadicado(HfId.Value)
            CargarRadicados(Nothing)
            ShowNotification("Radicado Actualizado", ShowNotifications.InfoNotification)
            ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

    Protected Sub btnAgregar_Click(sender As Object, e As EventArgs) Handles btnAgregar.Click
        AdicionarGv()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        txtcantidad.Text = String.Empty
        txtvrunitario.Text = String.Empty
        txttotal.Text = String.Empty
        txtobservacion.Text = String.Empty

    End Sub
    Sub Limpiar()
        txtcantidad.Text = String.Empty
        txtvrunitario.Text = String.Empty
        txttotal.Text = String.Empty
        txtobservacion.Text = String.Empty
        GvDetalle.DataSource = Nothing
        GvDetalle.DataBind()
    End Sub


    Sub AdicionarGv()
        Dim miDataTable As New DataTable
        Dim dRow As DataRow
        Dim InfoContr As New TH_Contratistas
        Dim Cont As New CoreProject.Contratista


        miDataTable.Columns.Add("Consecutivo")
        miDataTable.Columns.Add("Cantidad")
        miDataTable.Columns.Add("VrUnitario")
        miDataTable.Columns.Add("VrTotal")
        miDataTable.Columns.Add("Observacion")
        miDataTable.Columns.Add("TipoDocumento")


        For i = 0 To GvDetalle.Rows.Count - 1
            dRow = miDataTable.NewRow()
            Dim row As GridViewRow = GvDetalle.Rows(i)
            dRow("Consecutivo") = row.Cells(0).Text
            dRow("Cantidad") = row.Cells(1).Text
            dRow("VrUnitario") = row.Cells(2).Text
            dRow("VrTotal") = row.Cells(3).Text
            dRow("Observacion") = row.Cells(4).Text
            dRow("TipoDocumento") = row.Cells(5).Text


            miDataTable.Rows.Add(dRow)
        Next

        dRow = miDataTable.NewRow()
        dRow("Consecutivo") = txtconsecutivo.Text
        dRow("Cantidad") = txtcantidad.Text
        dRow("VrUnitario") = txtvrunitario.Text
        dRow("VrTotal") = txttotal.Text
        dRow("Observacion") = txtobservacion.Text
        dRow("TipoDocumento") = ddltipodocumento.SelectedValue


        miDataTable.Rows.Add(dRow)
        GvDetalle.DataSource = miDataTable
        GvDetalle.DataBind()
    End Sub
    Private Sub GvOrdenes_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GvOrdenes.PageIndexChanging
        GvOrdenes.PageIndex = e.NewPageIndex
        ObtenerOrdenes(Nothing, Nothing)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        If txtbuscar.Text = "" Then
            ObtenerOrdenes(Nothing, Nothing)
        Else
            ObtenerOrdenes(txtbuscar.Text, Nothing)
        End If

    End Sub

    Protected Sub btbuscar_Click(sender As Object, e As EventArgs) Handles btbuscar.Click
        If TxtConBus.Text = "" Then
            ShowNotification("Ingrese Numero de Consecutivo", ShowNotifications.InfoNotification)
            ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
        Else
            CargarRadicados(TxtConBus.Text)
            ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
        End If
    End Sub


    Private Sub GvRadicados_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GvRadicados.PageIndexChanging
        GvRadicados.PageIndex = e.NewPageIndex
        CargarRadicados(Nothing)
        ActivateAccordion(2, EffectActivateAccordion.SlideEffect)

    End Sub

    Private Sub GvRadicados_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles GvRadicados.RowCommand
        If e.CommandName = "Eliminar" Then
            EliminarRadicado(Int64.Parse(Me.GvRadicados.DataKeys(CInt(e.CommandArgument))("Id")))
            ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
        ElseIf e.CommandName = "Actualizar" Then
            HfId.Value = (Int64.Parse(Me.GvRadicados.DataKeys(CInt(e.CommandArgument))("Id")))
            CargarDatosActualizar(Int64.Parse(Me.GvRadicados.DataKeys(CInt(e.CommandArgument))("Id")))
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

    Sub EliminarRadicado(ByVal Id As Int16)
        Dim op As New ProcesosInternos
        op.EliminarRadicadoCuenta(Id)
        ShowNotification("Radicado Elimado", ShowNotifications.InfoNotification)
        CargarRadicados(Nothing)
    End Sub

    Sub CargarDatosActualizar(ByVal Id As Int64)
        Dim Op As New ProcesosInternos
        Dim Radicado As List(Of CC_RecepcionCuentasdeCobroGetXId_Result)
        Radicado = Op.RecepcionCuentasGetXId(Id)
        txtconsecutivo.Text = Radicado.Item(0).Consecutivo
        txtcantidad.Text = Radicado.Item(0).Cantidad
        txtvrunitario.Text = Radicado.Item(0).VrUnitario
        txttotal.Text = Radicado.Item(0).VrTotal
        txtobservacion.Text = Radicado.Item(0).Observacion
        ddltipodocumento.SelectedValue = Radicado.Item(0).TipoDocumentoId
        btnGuardar.Text = "Actualizar"
    End Sub

    Sub ActualizarRadicado(ByVal Id As Int16)
        Dim op As New ProcesosInternos
        op.ActualizarRadicado(txtconsecutivo.Text, txtcantidad.Text, txtvrunitario.Text, txttotal.Text, txtobservacion.Text, Id)

    End Sub

End Class