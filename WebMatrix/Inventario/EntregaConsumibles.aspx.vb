Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports ClosedXML.Excel
Imports CoreProject.OP
Imports CoreProject.INV
Imports System.IO

Public Class EntregaConsumibles
    Inherits System.Web.UI.Page

#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarArticulosConsumibles()
            CargarEstadosConsumibles()
            CargarBU()
            CargarTipoMovimiento()
            cargarCuentasContablesDropDown()
            CargarTipoMovimiento()
            CargarSedes()
            CargarCiudades()
        End If

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)
        smanager.RegisterPostBackControl(Me.btnExportarSC)

    End Sub


    Private Sub gvArticulos_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvArticulos.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If CType(e.Row.DataItem, INV_RegistroArticulos_Get_Result).IdEstado = 1 Then
                DirectCast(e.Row.FindControl("imgBtnAsignar"), ImageButton).Visible = True
            Else
                DirectCast(e.Row.FindControl("imgBtnAsignar"), ImageButton).Visible = False
            End If
        End If

    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub gvArticulos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvArticulos.PageIndexChanging
        gvArticulos.PageIndex = e.NewPageIndex
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvStock_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvStock.PageIndexChanging
        gvStock.PageIndex = e.NewPageIndex
        CargarGridStock()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvArticulos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvArticulos.RowCommand
        Dim lstRegistroArticulos As List(Of INV_RegistroArticulos_Get_Result)

        If e.CommandName = "Asignar" Then

            lstRegistroArticulos = ObtenerRegistroArticulos(gvArticulos.DataKeys(e.CommandArgument)("id"))
            hfIdArticulo.Value = gvArticulos.DataKeys(e.CommandArgument)("IdArticulo")

            lblIdAsignar.Text = lstRegistroArticulos(0).Id
            lblAsignar.Visible = True
            lblIdAsignar.Visible = True
            lblArticulo.Visible = True

            If hfIdArticulo.Value = 7 Or hfIdArticulo.Value = 9 Then
                lblValor.InnerText = "Cantidad"
            Else
                lblValor.InnerText = "Valor"
            End If

            If lstRegistroArticulos(0).IdArticulo = 7 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo & ": " & lstRegistroArticulos(0).Producto
            ElseIf lstRegistroArticulos(0).IdArticulo = 8 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo & lstRegistroArticulos(0).TipoBono
            ElseIf lstRegistroArticulos(0).IdArticulo = 9 Then
                lblArticulo.Text = lstRegistroArticulos(0).Articulo
            End If

            Dim lstStockConsumibles As List(Of INV_StockConsumibles_Get_Result)
            lstStockConsumibles = ObtenerStockConsumibles(lblIdAsignar.Text)

            If lstStockConsumibles.Count > 0 AndAlso lstStockConsumibles(0).Disponible IsNot Nothing Then
                lblIdDisponible.Text = lstStockConsumibles(0).Disponible
            Else
                lblIdDisponible.Text = "Este Artículo no se encuentra en el Stock"
            End If

            lblDisponible.Visible = True
            lblIdDisponible.Visible = True

            If lstStockConsumibles.Count > 0 AndAlso lstRegistroArticulos(0).IdArticulo = 7 Then
                ddlCentroCosto.SelectedValue = lstStockConsumibles(0).IdCentroCosto
                ddlCentroCosto.Enabled = False

                If ddlCentroCosto.SelectedValue = 3 Then
                    lblBU.Visible = True
                    ddlBU.Visible = True
                    ddlBU.SelectedValue = lstStockConsumibles(0).IdBU
                    ddlBU.Enabled = False
                    lblJBIJBE.Visible = False
                    txtJBIJBE.Visible = False
                    txtJBIJBE.Text = ""
                    lblNombreJBIJBE.Visible = False
                    txtNombreJBIJBE.Visible = False
                    txtNombreJBIJBE.Text = ""
                    hfIdTrabajo.Value = Nothing
                Else
                    lblJBIJBE.Visible = True
                    txtJBIJBE.Visible = True
                    txtJBIJBE.Text = lstStockConsumibles(0).JobBookCodigo
                    txtJBIJBE.Enabled = False
                    lblNombreJBIJBE.Visible = True
                    txtNombreJBIJBE.Visible = True
                    txtNombreJBIJBE.Text = lstStockConsumibles(0).JobBookNombre
                    txtNombreJBIJBE.Enabled = False
                    hfIdTrabajo.Value = lstStockConsumibles(0).JobBook
                    lblBU.Visible = False
                    ddlBU.Visible = False
                    ddlBU.ClearSelection()
                End If

                cargarCuentasContablesDropDown()
                ddlCuentasContables.SelectedValue = lstStockConsumibles(0).IdCuentaContable
                btnCuentasContables.Visible = False

            ElseIf lstRegistroArticulos(0).IdArticulo = 8 Then
                lblNumeroEntrega.Visible = True
                txtNumeroEntrega.Visible = True
                txtNumeroEntrega.Text = lstRegistroArticulos(0).Id
                txtNumeroEntrega.Enabled = False

                ddlCentroCosto.SelectedValue = lstRegistroArticulos(0).IdCentroCosto
                ddlCentroCosto.Enabled = False

                If ddlCentroCosto.SelectedValue = 3 Then
                    lblBU.Visible = True
                    ddlBU.Visible = True
                    ddlBU.SelectedValue = lstRegistroArticulos(0).IdBU
                    ddlBU.Enabled = False
                    lblJBIJBE.Visible = False
                    txtJBIJBE.Visible = False
                    txtJBIJBE.Text = ""
                    lblNombreJBIJBE.Visible = False
                    txtNombreJBIJBE.Visible = False
                    txtNombreJBIJBE.Text = ""
                    hfIdTrabajo.Value = Nothing
                Else
                    lblJBIJBE.Visible = True
                    txtJBIJBE.Visible = True
                    txtJBIJBE.Text = lstRegistroArticulos(0).JobBookCodigo
                    txtJBIJBE.Enabled = False
                    lblNombreJBIJBE.Visible = True
                    txtNombreJBIJBE.Visible = True
                    txtNombreJBIJBE.Text = lstRegistroArticulos(0).JobBookNombre
                    txtNombreJBIJBE.Enabled = False
                    If lstRegistroArticulos(0).JobBook Is Nothing Then
                        hfIdTrabajo.Value = Nothing
                    Else
                        hfIdTrabajo.Value = lstRegistroArticulos(0).JobBook
                    End If
                    lblBU.Visible = False
                    ddlBU.Visible = False
                    ddlBU.ClearSelection()
                End If

                cargarCuentasContablesDropDown()
                ddlCuentasContables.SelectedValue = lstRegistroArticulos(0).IdCuentaContable
                btnCuentasContables.Visible = False

            Else
                lblNumeroEntrega.Visible = False
                txtNumeroEntrega.Visible = False
                txtNumeroEntrega.Text = ""
                ddlCentroCosto.Enabled = True
                ddlCentroCosto.ClearSelection()
                lblBU.Visible = False
                ddlBU.Visible = False
                ddlBU.ClearSelection()
                ddlBU.Enabled = True
                lblJBIJBE.Visible = False
                txtJBIJBE.Visible = False
                txtJBIJBE.Text = ""
                txtJBIJBE.Enabled = True
                lblNombreJBIJBE.Visible = False
                txtNombreJBIJBE.Visible = False
                txtNombreJBIJBE.Text = ""
                txtNombreJBIJBE.Enabled = True
                hfIdTrabajo.Value = Nothing
                ddlCuentasContables.ClearSelection()
                btnCuentasContables.Visible = True

            End If

            If lstRegistroArticulos(0).IdArticulo = 7 Then
                lblEstado.Visible = True
                ddlEstado.Visible = True
            Else
                lblEstado.Visible = False
                ddlEstado.Visible = False
            End If

            If lstRegistroArticulos(0).IdArticulo = 9 Then
                lblVale.Visible = True
                txtVale.Visible = True
            Else
                lblVale.Visible = False
                txtVale.Visible = False
                txtVale.Text = ""
            End If
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If

        If e.CommandName = "Stock" Then
            lstRegistroArticulos = ObtenerRegistroArticulos(gvArticulos.DataKeys(e.CommandArgument)("id"))

            lblIdStock.Text = lstRegistroArticulos(0).Id
            lblIdStock.Visible = True
            lblStock.Visible = True
            lblConsumible.Visible = True

            If lstRegistroArticulos(0).IdArticulo = 7 Then
                lblConsumible.Text = lstRegistroArticulos(0).Articulo & "-" & lstRegistroArticulos(0).TipoProducto & "-" & lstRegistroArticulos(0).Producto
            ElseIf lstRegistroArticulos(0).IdArticulo = 8 Then
                lblConsumible.Text = lstRegistroArticulos(0).Articulo & "-" & lstRegistroArticulos(0).TipoBono
            ElseIf lstRegistroArticulos(0).IdArticulo = 9 Then
                lblConsumible.Text = lstRegistroArticulos(0).Articulo
            End If

            If lstRegistroArticulos(0).IdArticulo = 7 AndAlso lstRegistroArticulos(0).IdTipoProducto = 1 Then
                gvStock.Columns(5).Visible = True  'TipoObsequio
            Else
                gvStock.Columns(5).Visible = False  'TipoObsequio

            End If

            If lstRegistroArticulos(0).IdArticulo = 7 Then
                gvStock.Columns(6).Visible = True  'EstadoProducto
            Else
                gvStock.Columns(6).Visible = False  'EstadoProducto
            End If

            If lstRegistroArticulos(0).IdArticulo = 9 Then
                gvStock.Columns(8).Visible = True  'NumeroVale
            Else
                gvStock.Columns(8).Visible = False  'NumeroVale
            End If

            CargarGridStock()

            ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
        End If

    End Sub

    Private Sub gvUsuarios_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvUsuarios.RowCommand
        If e.CommandName = "Seleccionar" Then
            txtUsuario.Text = Server.HtmlDecode(gvUsuarios.Rows(e.CommandArgument).Cells(0).Text)
            hfIdUsuario.Value = Me.gvUsuarios.DataKeys(CInt(e.CommandArgument))("Id")

            If Server.HtmlDecode(gvUsuarios.Rows(e.CommandArgument).Cells(2).Text) = "Contratista" Then
                hfIdCargo.Value = 2
            Else
                hfIdCargo.Value = 1
            End If
            txtCargo.Text = Server.HtmlDecode(gvUsuarios.Rows(e.CommandArgument).Cells(2).Text)

            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub

#End Region


#Region "Grillas"

    Public Sub CargarGrid()
        Dim oListadoArticulos As New Inventario
        gvArticulos.DataSource = oListadoArticulos.obtenerRegistroArticulosxTodos
        gvArticulos.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Public Sub CargarGridStock()
        gvStock.DataSource = obtenerArticulosConsumibles()
        gvStock.DataBind()
        ActivateAccordion(2, EffectActivateAccordion.NoEffect)
    End Sub

    Sub CargarGridPersonas()
        Dim o As New Personas
        Dim daContratistas As New Contratista
        Dim cedula As Int64? = Nothing
        Dim nombre As String = Nothing

        If IsNumeric(txtCedulaUsuario.Text) Then cedula = txtCedulaUsuario.Text
        If Not txtNombreUsuario.Text = "" Then nombre = txtNombreUsuario.Text

        Dim lstPersonas = o.ObtenerPersonasxCCNombre(cedula, nombre)
        Dim lstContratistas = daContratistas.ObtenerContratistas(cedula, nombre, True)

        Dim un = (From x In lstPersonas
                    Select Nombres = x.Nombres & " " & x.Apellidos, Id = x.id, Ciudad = x.Ciudad, Cargo = x.Cargo
                    ).Union(
                    From y In lstContratistas
                    Select Nombres = y.Nombre, Id = y.Identificacion, Ciudad = y.Ciudad, Cargo = "Contratista"
                    ).ToList


        Me.gvUsuarios.DataSource = un
        Me.gvUsuarios.DataBind()
    End Sub

    Sub Guardar()

        If Not (IsDate(txtFecha.Text)) Then
            ShowNotification("Escriba la fecha de la asignación", ShowNotifications.ErrorNotification)
            txtFecha.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlTipo.SelectedValue = "-1" Then
            ShowNotification("Seleccione el tipo de Movimiento", ShowNotifications.ErrorNotification)
            ddlTipo.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If


        If ddlEstado.Visible = True And ddlEstado.SelectedValue = "-1" Then
            ShowNotification("Seleccione el Estado del Artículo", ShowNotifications.ErrorNotification)
            ddlEstado.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCentroCosto.Visible = True And ddlCentroCosto.SelectedValue = "-1" Then
            ShowNotification("Seleccione el Centro de Costo y luego el JB o la BU", ShowNotifications.ErrorNotification)
            ddlCentroCosto.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCentroCosto.SelectedValue = 3 AndAlso ddlBU.SelectedValue = "-1" Then
            ShowNotification("Seleccione la Unidad de Negocio", ShowNotifications.ErrorNotification)
            ddlBU.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If (ddlCentroCosto.SelectedValue = 1 Or ddlCentroCosto.SelectedValue = 2) AndAlso String.IsNullOrEmpty(txtJBIJBE.Text) Then
            ShowNotification("Debe indicar un codigo de JobBook", ShowNotifications.ErrorNotification)
            txtJBIJBE.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If (ddlCentroCosto.SelectedValue = 1 Or ddlCentroCosto.SelectedValue = 2) AndAlso String.IsNullOrEmpty(txtNombreJBIJBE.Text) Then
            ShowNotification("Debe indicar un nombre de JobBook", ShowNotifications.ErrorNotification)
            txtNombreJBIJBE.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCuentasContables.Visible = True And ddlCuentasContables.SelectedValue = -1 Then
            ShowNotification("Debe seleccionar una cuenta contable", ShowNotifications.ErrorNotification)
            ddlCuentasContables.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCiudad.SelectedValue = "-1" Then
            ShowNotification("Debe seleccionar una Ciudad", ShowNotifications.ErrorNotification)
            ddlCiudad.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If txtValor.Visible = True And String.IsNullOrEmpty(txtValor.Text) Then
            ShowNotification("Debe ingresar el Valor", ShowNotifications.ErrorNotification)
            txtValor.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If

        If hfIdUsuario.Value = 0 Then
            ShowNotification("Debe seleccionar el Usuario", ShowNotifications.ErrorNotification)
            btnSearchUsuario.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Dim oGuardar As New Inventario

        Dim lstRegistroArticulos As List(Of INV_RegistroArticulos_Get_Result)
        lstRegistroArticulos = ObtenerRegistroArticulos(lblIdAsignar.Text)

        Dim lstStockMovimiento As List(Of INV_StockConsumibles_Get_Result)
        lstStockMovimiento = ObtenerStockxMovimiento(lblIdAsignar.Text, 1)

        Dim NumeroVale As Int64?
        NumeroVale = If(txtVale.Text = "", CType(Nothing, Int64?), CType(txtVale.Text, Int64?))

        Dim UsuarioRegistra As Int64?
        UsuarioRegistra = CType(Session("IDUsuario").ToString, Int64?)

        Dim Estado As Int16? = Nothing
        If Not ddlEstado.SelectedValue = -1 Then Estado = ddlEstado.SelectedValue

        Dim CentroCosto As Int16? = Nothing
        If Not ddlCentroCosto.SelectedValue = -1 Then CentroCosto = ddlCentroCosto.SelectedValue

        Dim BU As Int32? = Nothing
        If Not ddlBU.SelectedValue = -1 Then BU = ddlBU.SelectedValue

        Dim IdTrabajo As Int64? = Nothing
        If ddlCentroCosto.SelectedValue = 1 Or ddlCentroCosto.SelectedValue = 2 Then IdTrabajo = If(String.IsNullOrEmpty(hfIdTrabajo.Value), CType(Nothing, Int64?), CType(hfIdTrabajo.Value, Int64?))

        Dim JobBookCodigo As String = Nothing
        If Not txtJBIJBE.Text = "" Then JobBookCodigo = txtJBIJBE.Text

        Dim JobBookNombre As String = Nothing
        If Not txtNombreJBIJBE.Text = "" Then JobBookNombre = txtNombreJBIJBE.Text

        Dim CuentaContable As Int64? = Nothing
        If Not ddlCuentasContables.SelectedValue = -1 Then CuentaContable = ddlCuentasContables.SelectedValue

        Dim Valor As Int64? = Nothing
        If Not txtValor.Text = "" Then Valor = txtValor.Text

        Dim Total As Int64? = Nothing
        If ddlTipo.SelectedValue = 1 And lstStockMovimiento.Count > 0 Then
            Total = lstStockMovimiento(0).Total + Valor
        ElseIf lstStockMovimiento.Count > 0 Then
            Total = lstStockMovimiento(0).Total
        Else
            Total = Valor
        End If
        
        Dim lstStockConsumibles As List(Of INV_StockConsumibles_Get_Result)
        lstStockConsumibles = ObtenerStockConsumibles(lstRegistroArticulos(0).Id)
        Dim Id As Int64? = Nothing
        If lstStockConsumibles.Count > 0 Then Id = lstStockConsumibles(0).Id

        Dim Disponible As Int64?
        If ddlTipo.SelectedValue = 1 And lstStockConsumibles.Count > 0 Then
            Disponible = lstStockConsumibles(0).Disponible + Valor
        End If

        If ddlTipo.SelectedValue = 1 And lstStockConsumibles.Count = 0 Then
            Disponible = Valor
        End If

        If ddlTipo.SelectedValue = 2 And lstStockConsumibles.Count > 0 Then
            Disponible = lstStockConsumibles(0).Disponible - Valor
        End If

        If Disponible < 0 Then
            AlertJS("No se puede asignar la cantidad porque no está Disponible en el Stock")
            txtValor.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlTipo.SelectedValue = 2 And lstStockConsumibles.Count = 0 Then
            AlertJS("No se puede asignar la cantidad porque no está Disponible en el Stock")
            txtValor.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlTipo.SelectedValue = 3 And lstStockConsumibles.Count > 0 Then
            Disponible = lstStockConsumibles(0).Disponible + Valor
        End If

        If ddlTipo.SelectedValue = 3 And lstStockConsumibles.Count = 0 Then
            AlertJS("No se puede realizar una devolución porque el artículo no está Disponible en el Stock")
            txtValor.Focus()
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        oGuardar.GuardarStockConsumibles(lblIdAsignar.Text, NumeroVale, txtFecha.Text, UsuarioRegistra, ddlTipo.SelectedValue, Estado, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, CuentaContable, ddlCiudad.SelectedValue, Valor, Total, Disponible, hfIdUsuario.Value, hfIdCargo.Value, txtObservacion.Text)

        ShowNotification("La Asignación se ha hecho correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)

        limpiar()

    End Sub

    Public Sub CargarColumnas()

        'Productos
        If ddlIdArticulo.SelectedValue = 7 Then
            gvArticulos.Columns(7).Visible = False  'CentroCosto
            gvArticulos.Columns(8).Visible = False  'BU
            gvArticulos.Columns(10).Visible = False  'JobBookCodigo
            gvArticulos.Columns(11).Visible = False  'JobBookNombre
            gvArticulos.Columns(44).Visible = True  'TipoProducto
            gvArticulos.Columns(45).Visible = True  'Producto
            gvArticulos.Columns(46).Visible = True  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
        End If

        'Bonos
        If ddlIdArticulo.SelectedValue = 8 Then
            gvArticulos.Columns(7).Visible = True  'CentroCosto
            gvArticulos.Columns(8).Visible = True  'BU
            gvArticulos.Columns(10).Visible = True  'JobBookCodigo
            gvArticulos.Columns(11).Visible = True  'JobBookNombre
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = True  'TipoBono
        End If

        'Vale Taxi
        If ddlIdArticulo.SelectedValue = 9 Then
            gvArticulos.Columns(7).Visible = False  'CentroCosto
            gvArticulos.Columns(8).Visible = False  'BU
            gvArticulos.Columns(10).Visible = False  'JobBookCodigo
            gvArticulos.Columns(11).Visible = False  'JobBookNombre
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
        End If

    End Sub

    Function ObtenerRegistroArticulos(ByVal Id As Int64?) As List(Of INV_RegistroArticulos_Get_Result)
        Dim lstRegistroArticulos As New List(Of INV_RegistroArticulos_Get_Result)
        Dim RecordArticulos As New Inventario
        lstRegistroArticulos = RecordArticulos.obtenerRegistroArticulosxId(Id)
        Return lstRegistroArticulos
    End Function

    Function ObtenerStockConsumibles(ByVal IdConsumible As Int64?) As List(Of INV_StockConsumibles_Get_Result)
        Dim lstStockConsumibles As New List(Of INV_StockConsumibles_Get_Result)
        Dim RecordStock As New Inventario
        lstStockConsumibles = RecordStock.ObtenerStockConsumiblesxIdConsumible(IdConsumible)
        Return lstStockConsumibles
    End Function

    Function ObtenerStockxUsuario(ByVal IdConsumible As Int64?, ByVal IdUsuario As Int64?) As List(Of INV_StockConsumibles_Get_Result)
        Dim lstStockConsumibles As New List(Of INV_StockConsumibles_Get_Result)
        Dim RecordStock As New Inventario
        lstStockConsumibles = RecordStock.ObtenerStockConsumiblesxIdusuario(IdConsumible, IdUsuario)
        Return lstStockConsumibles
    End Function

    Function ObtenerStockxMovimiento(ByVal IdConsumible As Int64?, ByVal TipoMovimiento As Int16?) As List(Of INV_StockConsumibles_Get_Result)
        Dim lstStockConsumibles As New List(Of INV_StockConsumibles_Get_Result)
        Dim RecordStock As New Inventario
        lstStockConsumibles = RecordStock.ObtenerStockConsumiblesxTipoMovimiento(IdConsumible, TipoMovimiento)
        Return lstStockConsumibles
    End Function

    Function obtenerRegistrosArticulos() As List(Of INV_RegistroArticulos_Get_Result)
        Dim oBusqueda As New Inventario
        Dim Articulo As Int64? = Nothing
        Dim TipoProducto As Int64? = Nothing
        Dim Sede As Int64? = Nothing
        Dim IdUsuarioAsignado As Int64? = Nothing
        Dim UsuarioAsignado As String = Nothing
        Dim TodosCampos As String = Nothing

        If Not ddlIdArticulo.SelectedValue = -1 Then Articulo = ddlIdArticulo.SelectedValue
        If Not ddlIdTipoProducto.SelectedValue = -1 Then TipoProducto = ddlIdTipoProducto.SelectedValue
        If Not ddlIdSede.SelectedValue = -1 Then Sede = ddlIdSede.SelectedValue
        If Not txtBusqueda.Text = "" Then TodosCampos = txtBusqueda.Text

		Return oBusqueda.obtenerRegistroArticulos(Nothing, 2, Articulo, Nothing, Nothing, Nothing, TipoProducto, Nothing, Sede, IdUsuarioAsignado, UsuarioAsignado, Nothing, Nothing, TodosCampos)
	End Function

    Function obtenerArticulosConsumibles() As List(Of INV_StockConsumibles_Get_Result)
        Dim oBusqueda As New Inventario
        Dim IdConsumible As Int64? = Nothing

        If Not lblIdStock.Text = "" Then IdConsumible = lblIdStock.Text

        Return oBusqueda.ObtenerStockConsumiblesxIdConsumible(IdConsumible)
    End Function

    Function obtenerCuentasContables(ByVal numeroCuenta As String, ByVal descripcion As String) As List(Of CC_CuentasContablesGet_Result)
        Dim o As New ProcesosInternos
        Dim tipo As Int16
        If String.IsNullOrEmpty(numeroCuenta) Then
            numeroCuenta = Nothing
        End If
        If String.IsNullOrEmpty(descripcion) Then
            descripcion = Nothing
        End If

        If ddlCentroCosto.SelectedValue = 3 Then
            tipo = 1
        ElseIf (ddlCentroCosto.SelectedValue = 1 Or ddlCentroCosto.SelectedValue = 2) Then
            tipo = 2
        End If

        Return o.CuentasContablesGet(numeroCuenta, descripcion, Nothing, tipo)
    End Function

#End Region

#Region "DDL"

    Sub CargarArticulosConsumibles()
        Dim oArticulos As New Inventario
        Dim oUsuario As New US.UsuariosUnidades
        Dim Usuario = oUsuario.obtenerUnidadesXUsuario(Session("IDUsuario").ToString, True, Nothing)

        For Each oGrupoUnidad In Usuario
            Dim GrupoUnidad As Int32? = oGrupoUnidad.GrupoUnidadId
            Dim Busqueda = oArticulos.obtenerArticulosxTipoArticulo(2, GrupoUnidad)
            If Busqueda.Count > 0 Then
                Me.ddlIdArticulo.DataSource = oArticulos.obtenerArticulosxTipoArticulo(2, GrupoUnidad)
                Exit For
            Else
                Me.ddlIdArticulo.Items.Clear()
            End If
        Next

        Me.ddlIdArticulo.DataValueField = "Id"
        Me.ddlIdArticulo.DataTextField = "Articulo"
        Me.ddlIdArticulo.DataBind()
        Me.ddlIdArticulo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarEstadosConsumibles()
        Dim oEstados As New Inventario
        Me.ddlEstado.DataSource = oEstados.ObtenerEstadosConsumibles
        Me.ddlEstado.DataValueField = "Id"
        Me.ddlEstado.DataTextField = "Estado"
        Me.ddlEstado.DataBind()
        Me.ddlEstado.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarBU()
        Dim oBU As New Inventario
        Me.ddlBU.DataSource = oBU.obtenerBU
        Me.ddlBU.DataValueField = "Id"
        Me.ddlBU.DataTextField = "Nombre"
        Me.ddlBU.DataBind()
        Me.ddlBU.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarCiudades()
        Dim oCiudad As New CoreProject.RegistroPersonas
        Me.ddlCiudad.DataSource = oCiudad.CiudadesList
        Me.ddlCiudad.DataValueField = "id"
        Me.ddlCiudad.DataTextField = "Ciudad"
        Me.ddlCiudad.DataBind()
        Me.ddlCiudad.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarTipoMovimiento()
        Dim oTipo As New Inventario
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

        If permisos.VerificarPermisoUsuario(138, UsuarioID) = True And permisos.VerificarPermisoUsuario(140, UsuarioID) = False Then
            Me.ddlTipo.DataSource = oTipo.obtenerTipoMovimiento(138)
        ElseIf permisos.VerificarPermisoUsuario(138, UsuarioID) = False And permisos.VerificarPermisoUsuario(140, UsuarioID) = True Then
            Me.ddlTipo.DataSource = oTipo.obtenerTipoMovimiento(140)
        ElseIf permisos.VerificarPermisoUsuario(138, UsuarioID) = True And permisos.VerificarPermisoUsuario(140, UsuarioID) = True Then
            Me.ddlTipo.DataSource = oTipo.obtenerTipoMovimiento(Nothing)
        End If

        Me.ddlTipo.DataValueField = "Id"
        Me.ddlTipo.DataTextField = "Movimiento"
        Me.ddlTipo.DataBind()
        Me.ddlTipo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarSedes()
        Dim oSedes As New Inventario
        Me.ddlIdSede.DataSource = oSedes.obtenerSedes
        Me.ddlIdSede.DataValueField = "Id"
        Me.ddlIdSede.DataTextField = "Sede"
        Me.ddlIdSede.DataBind()
        Me.ddlIdSede.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub cargarCuentasContablesDropDown()
        ddlCuentasContables.DataSource = obtenerCuentasContables(Nothing, Nothing)
        ddlCuentasContables.DataValueField = "id"
        ddlCuentasContables.DataTextField = "descripcion"
        ddlCuentasContables.DataBind()
        ddlCuentasContables.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

#End Region

#Region "Formulario"

    Function obtenerCentroCosto(ByVal tipo As Byte, ByVal valorBusqueda As String) As List(Of FI_JBE_JBI_CC_Get_Result)
        Dim o As New FI.Ordenes
        Return o.ObtenerJBE_JBI_CC(tipo, valorBusqueda)
    End Function


    Sub limpiar()
        Me.txtNumeroEntrega.Text = ""
        Me.txtVale.Text = ""
        Me.txtFecha.Text = ""
        ddlTipo.ClearSelection()
        ddlEstado.ClearSelection()
        ddlCentroCosto.ClearSelection()
        ddlBU.ClearSelection()
        Me.txtJBIJBE.Text = ""
        Me.txtNombreJBIJBE.Text = ""
        ddlCuentasContables.ClearSelection()
        ddlCiudad.ClearSelection()
        Me.txtValor.Text = ""
        Me.txtUsuario.Text = ""
        Me.txtCargo.Text = ""
        Me.txtObservacion.Text = ""
        hfIdArticulo.Value = "0"
        hfIdTrabajo.Value = "0"
        hfIdUsuario.Value = "0"
        hfIdCargo.Value = "0"

        Me.txtCedulaUsuario.Text = ""
        Me.txtNombreUsuario.Text = ""
        Me.txtNumeroCuenta.Text = ""
        Me.txtDescripcion.Text = ""

        lblIdAsignar.Text = ""
        lblArticulo.Text = ""
        lblIdDisponible.Text = ""
        lblIdAsignar.Visible = False
        lblAsignar.Visible = False
        lblArticulo.Visible = False
        lblDisponible.Visible = False
        lblIdDisponible.Visible = False

        ddlIdArticulo.ClearSelection()
        ddlIdTipoProducto.ClearSelection()
        ddlIdSede.ClearSelection()
        txtBusqueda.Text = ""
        gvArticulos.DataSource = Nothing
        gvArticulos.DataBind()

    End Sub

#End Region

#Region "Eventos"

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Guardar()
    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscarUsuario_Click(sender As Object, e As EventArgs) Handles btnBuscarUsuario.Click
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        CargarGridPersonas()
        UPanelUsuarios.Update()
    End Sub

    Private Sub gvCuentasContables_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCuentasContables.RowCommand
        If e.CommandName = "Seleccionar" Then
            ddlCuentasContables.SelectedValue = Me.gvCuentasContables.DataKeys(CInt(e.CommandArgument))("id")
            ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        End If
    End Sub

    Protected Sub btnBuscarCuentaContable_Click(sender As Object, e As EventArgs) Handles btnBuscarCuentaContable.Click
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
        gvCuentasContables.DataSource = obtenerCuentasContables(txtDescripcion.Text, txtNumeroCuenta.Text)
        gvCuentasContables.DataBind()
    End Sub

    Protected Sub btnCuentasContables_Click(sender As Object, e As EventArgs) Handles btnCuentasContables.Click
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnSearchUsuario_Click(sender As Object, e As EventArgs) Handles btnSearchUsuario.Click
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlTipo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipo.SelectedIndexChanged
        If ddlTipo.SelectedValue = 1 And hfIdArticulo.Value = 9 Then
            lblVale.Visible = False
            txtVale.Visible = False
            txtVale.Text = ""
            lblCentroCosto.Visible = False
            ddlCentroCosto.Visible = False
            ddlCentroCosto.ClearSelection()
            lblBU.Visible = False
            ddlBU.Visible = False
            ddlBU.ClearSelection()
            lblJBIJBE.Visible = False
            txtJBIJBE.Visible = False
            txtJBIJBE.Text = ""
            lblNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Text = ""
            lblCuentasContables.Visible = False
            ddlCuentasContables.Visible = False
            ddlCuentasContables.ClearSelection()
            btnCuentasContables.Visible = False
            
        ElseIf ddlTipo.SelectedValue = 2 And hfIdArticulo.Value = 9 Then
            lblVale.Visible = True
            txtVale.Visible = True
            lblCentroCosto.Visible = True
            ddlCentroCosto.Visible = True
            lblCuentasContables.Visible = True
            ddlCuentasContables.Visible = True
            btnCuentasContables.Visible = True
            
        ElseIf ddlTipo.SelectedValue = 3 And hfIdArticulo.Value = 9 Then
            lblVale.Visible = True
            txtVale.Visible = True
            lblCentroCosto.Visible = False
            ddlCentroCosto.Visible = False
            ddlCentroCosto.ClearSelection()
            lblBU.Visible = False
            ddlBU.Visible = False
            ddlBU.ClearSelection()
            lblJBIJBE.Visible = False
            txtJBIJBE.Visible = False
            txtJBIJBE.Text = ""
            lblNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Text = ""
            lblCuentasContables.Visible = False
            ddlCuentasContables.Visible = False
            ddlCuentasContables.ClearSelection()
            btnCuentasContables.Visible = False
            
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlCentroCosto_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlCentroCosto.SelectedIndexChanged
        hfIdTrabajo.Value = "0"
        If ddlCentroCosto.SelectedValue = 3 Then
            lblBU.Visible = True
            ddlBU.Visible = True
            lblJBIJBE.Visible = False
            txtJBIJBE.Visible = False
            txtJBIJBE.Text = ""
            lblNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Visible = False
            txtNombreJBIJBE.Text = ""
        Else
            lblJBIJBE.Visible = True
            txtJBIJBE.Visible = True
            txtJBIJBE.Text = ""
            lblNombreJBIJBE.Visible = True
            txtNombreJBIJBE.Visible = True
            txtNombreJBIJBE.Text = ""
            lblBU.Visible = False
            ddlBU.Visible = False
            ddlBU.ClearSelection()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub txtJBIJBE_TextChanged(sender As Object, e As EventArgs) Handles txtJBIJBE.TextChanged
        Dim daProyecto As New Proyecto
        Dim daTrabajo As New Trabajo
        Dim oP As PY_Proyectos_Get_Result
        Dim oT As PY_Trabajo0
        Select Case ddlCentroCosto.SelectedValue
            Case 1
                oP = daProyecto.obtenerXJobBook(txtJBIJBE.Text)
                If Not ((oP Is Nothing)) Then
                    txtNombreJBIJBE.Text = oP.Nombre
                    hfIdTrabajo.Value = oP.id
                Else
                    txtNombreJBIJBE.Text = ""
                    hfIdTrabajo.Value = ""
                End If

            Case 2
                oT = daTrabajo.ObtenerTrabajoXJob(txtJBIJBE.Text)
                If Not ((oT Is Nothing)) Then
                    txtNombreJBIJBE.Text = oT.NombreTrabajo
                    hfIdTrabajo.Value = oT.id
                Else
                    txtNombreJBIJBE.Text = ""
                    hfIdTrabajo.Value = ""
                End If
        End Select
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlIdArticulo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlIdArticulo.SelectedIndexChanged

        If ddlIdArticulo.SelectedValue = 7 Then
            lblIdTipoProducto.Visible = True
            ddlIdTipoProducto.Visible = True
        Else
            lblIdTipoProducto.Visible = False
            ddlIdTipoProducto.Visible = False
            ddlIdTipoProducto.ClearSelection()
        End If
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)

    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        gvArticulos.Visible = True
        'Actualiza los datos del gridview
        gvArticulos.AllowPaging = False
        gvArticulos.DataBind()

        'Crea variables en memoria para almacenar el contenido del gridview
        Dim sb As StringBuilder = New StringBuilder()
        Dim sw As StringWriter = New StringWriter(sb)
        Dim htw As HtmlTextWriter = New HtmlTextWriter(sw)
        'Crea una nueva página html y un form dentro de ella
        Dim pagina As Page = New Page
        Dim form = New HtmlForm
        'Cambie el estado del gridview para que no guarde los controles de vista
        gvArticulos.EnableViewState = False
        'Quita la validación de la página 
        pagina.EnableEventValidation = False
        pagina.DesignerInitialize()
        'Agrega el form creado
        pagina.Controls.Add(form)
        'Agrega el gridview al form
        form.Controls.Add(gvArticulos)
        'Hace un render para pasar los valores de la página al htmltextwriter que contendrá los datos del gridview
        pagina.RenderControl(htw)
        'Hace un response para descargar el control
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.AddHeader("Content-Disposition", "attachment;filename=Articulos_Consumibles.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvArticulos.Visible = False

    End Sub

    Protected Sub btnExportarSC_Click(sender As Object, e As EventArgs) Handles btnExportarSC.Click
        exportarExcelSC()
    End Sub

#Region "Exportar a Excel Listado Stock"
    Sub exportarExcelSC()
        Dim wb As New XLWorkbook
        Dim oStock As List(Of INV_StockConsumibles_Get_Result)
        Dim titulosStock As String = "IdConsumible;Articulo;TipoProducto;Producto;TipoBono;Fecha;UsuarioRegistra;TipoObsequio;EstadoProducto;CentroCosto;BU;JobBook;JobBookCodigo;JobBookNombre;NumeroCuentaContable;CuentaContable;Ciudad;NumeroVale;TipoMovimiento;Valor;Total;Disponible;Observaciones;UsuarioAsignado;TipoCargo"
        oStock = obtenerArticulosConsumibles()
        Dim oExportar = (From x In oStock
                        Select x.IdConsumible, x.Articulo, x.TipoProducto, x.Producto, x.TipoBono, x.Fecha, x.UsuarioRegistra, x.TipoObsequio, x.EstadoProducto, x.CentroCosto, x.BU, x.JobBook, x.JobBookCodigo, x.JobBookNombre,
                        x.NumeroCuentaContable, x.CuentaContable, x.Ciudad, x.NumeroVale, x.TipoMovimiento, x.Valor, x.Total, x.Disponible, x.Observaciones, x.UsuarioAsignado, x.TipoCargo).ToList

        Dim ws = wb.Worksheets.Add("Stock")
        insertarNombreColumnasExcelSC(ws, titulosStock.Split(";"))
        ws.Cell(2, 1).InsertData(oExportar)
        exportarExcelSC(wb, "Stock")
    End Sub

    Sub insertarNombreColumnasExcelSC(ByVal hoja As IXLWorksheet, ByVal nombresColumnas() As String)
        For columna = 0 To nombresColumnas.Count - 1
            hoja.Cell(1, columna + 1).Value = nombresColumnas(columna)
        Next
    End Sub

    Private Sub exportarExcelSC(ByVal workbook As XLWorkbook, ByVal name As String)
        Response.Clear()

        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        Response.AddHeader("content-disposition", "attachment;filename=""Articulos_" & name & ".xlsx""")

        Using memoryStream = New IO.MemoryStream()
            workbook.SaveAs(memoryStream)
            memoryStream.WriteTo(Response.OutputStream)
        End Using
        Response.End()
    End Sub

#End Region

    Private Sub _AlmacenamientoDisco_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(138, UsuarioID) = False AndAlso permisos.VerificarPermisoUsuario(140, UsuarioID) = False Then
            Response.Redirect("../Inventario/RegistroArticulos.aspx")
        End If
    End Sub

#End Region



  
   
End Class