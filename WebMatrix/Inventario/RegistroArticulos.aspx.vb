Imports WebMatrix.Util
Imports CoreProject
Imports CoreProject.Datos.ClsPermisosUsuarios
Imports ClosedXML.Excel
Imports CoreProject.OP
Imports CoreProject.INV
Imports System.IO

Public Class RegistroArticulos
    Inherits System.Web.UI.Page


#Region "Eventos del Control"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            CargarPerifericos()
            CargarListaArticulos()
            EstadosArticulos()
            CargarBU()
            cargarCuentasContablesDropDown()
            CargarIdPerifericos()
            CargarSedes()
            CargarIdSedes()
            IdEstadosArticulos()
            CargarListaPapeleria()
        End If

        lblEstado.Visible = False
        ddlEstado.Visible = False

        Dim smanager As ScriptManager = ScriptManager.GetCurrent(Me.Page)
        smanager.RegisterPostBackControl(Me.btnExportar)

    End Sub

    Sub AlertJS(ByVal mensaje As String)
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "AlertScript", "alert('" & mensaje & "');", True)
    End Sub

    Private Sub gvArticulos_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvArticulos.PageIndexChanging
        gvArticulos.PageIndex = e.NewPageIndex
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvArticulos_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvArticulos.RowCommand
        Dim lstRegistroArticulos As List(Of INV_RegistroArticulos_Get_Result)
        If e.CommandName = "Actualizar" Then
            lstRegistroArticulos = ObtenerRegistroArticulos(gvArticulos.DataKeys(e.CommandArgument).Value)
            ddlTipoArticulo.SelectedValue = lstRegistroArticulos(0).IdTipoArticulo

            CargarArticulos()
            ddlArticulo.SelectedValue = lstRegistroArticulos(0).IdArticulo

            CargarFormulario()
            txtFechaCompra.Text = lstRegistroArticulos(0).FechaCompra

            If lstRegistroArticulos(0).IdProductoPapeleria IsNot Nothing Then
                ddlPapeleria.SelectedValue = lstRegistroArticulos(0).IdProductoPapeleria
            End If

            If lstRegistroArticulos(0).IdCentroCosto IsNot Nothing Then
                ddlCentroCosto.SelectedValue = lstRegistroArticulos(0).IdCentroCosto
            End If

            If lstRegistroArticulos(0).IdBU IsNot Nothing Then
                ddlBU.SelectedValue = lstRegistroArticulos(0).IdBU
            End If

            If lstRegistroArticulos(0).JobBook IsNot Nothing Then
                hfIdTrabajo.Value = lstRegistroArticulos(0).JobBook
            End If

            If lstRegistroArticulos(0).JobBookCodigo IsNot Nothing Then
                txtJBIJBE.Text = lstRegistroArticulos(0).JobBookCodigo
            End If

            If lstRegistroArticulos(0).JobBookNombre IsNot Nothing Then
                txtNombreJBIJBE.Text = lstRegistroArticulos(0).JobBookNombre
            End If

            If lstRegistroArticulos(0).IdCuentaContable IsNot Nothing Then
                cargarCuentasContablesDropDown()
                ddlCuentasContables.SelectedValue = lstRegistroArticulos(0).IdCuentaContable
            End If

            If lstRegistroArticulos(0).Valor IsNot Nothing Then
                txtValorUnitario.Text = lstRegistroArticulos(0).Valor
            End If

            If lstRegistroArticulos(0).Cantidad IsNot Nothing Then
                txtCantidad.Text = lstRegistroArticulos(0).Cantidad
            End If

            If lstRegistroArticulos(0).IdEstado IsNot Nothing Then
                ddlEstado.SelectedValue = lstRegistroArticulos(0).IdEstado
            End If

            txtDescripcion.Text = lstRegistroArticulos(0).Descripcion

            If lstRegistroArticulos(0).NumeroPV IsNot Nothing Then
                txtNumeroPV.Text = lstRegistroArticulos(0).NumeroPV
            End If

            If lstRegistroArticulos(0).ProveedorId IsNot Nothing Then
                txtProveedor.Text = lstRegistroArticulos(0).ProveedorId
            End If

            If lstRegistroArticulos(0).Symphony IsNot Nothing Then
                txtSymphony.Text = lstRegistroArticulos(0).Symphony
            End If

            If lstRegistroArticulos(0).IdFisico IsNot Nothing Then
                txtIdFisico.Text = lstRegistroArticulos(0).IdFisico
            End If

            If lstRegistroArticulos(0).IdSede IsNot Nothing Then
                ddlSede.SelectedValue = lstRegistroArticulos(0).IdSede
            End If

            If lstRegistroArticulos(0).IdTipoComputador IsNot Nothing Then
                ddlTipoComputador.SelectedValue = lstRegistroArticulos(0).IdTipoComputador
            End If

            If lstRegistroArticulos(0).IdPertenecePC IsNot Nothing Then
                ddlPertenecePC.SelectedValue = lstRegistroArticulos(0).IdPertenecePC
            End If

            If lstRegistroArticulos(0).FechaFinRenta IsNot Nothing Then
                txtFechaFin.Text = lstRegistroArticulos(0).FechaFinRenta
            End If

            If lstRegistroArticulos(0).IdTipoPeriferico IsNot Nothing Then
                CargarPerifericos()
                ddlTipoPeriferico.SelectedValue = lstRegistroArticulos(0).IdTipoPeriferico
            End If

            txtMarca.Text = lstRegistroArticulos(0).Marca
            txtModelo.Text = lstRegistroArticulos(0).Modelo
            txtProcesador.Text = lstRegistroArticulos(0).Procesador
            txtMemoria.Text = lstRegistroArticulos(0).Memoria
            txtAlmacenamiento.Text = lstRegistroArticulos(0).Almacenamiento
            txtSistemaOperativo.Text = lstRegistroArticulos(0).SistemaOperativo
            txtSerial.Text = lstRegistroArticulos(0).Serial
            txtNombreEquipo.Text = lstRegistroArticulos(0).NombreEquipo
            txtOffice.Text = lstRegistroArticulos(0).Office
            txtProgramas.Text = lstRegistroArticulos(0).Programas
            txtTipoServidor.Text = lstRegistroArticulos(0).TipoServidor
            txtRaid.Text = lstRegistroArticulos(0).Raid

            If lstRegistroArticulos(0).IdTablet IsNot Nothing Then
                txtIdTablet.Text = lstRegistroArticulos(0).IdTablet
            End If

            If lstRegistroArticulos(0).IdSTG IsNot Nothing Then
                txtIdSTG.Text = lstRegistroArticulos(0).IdSTG
            End If

            txtTamanoPantalla.Text = lstRegistroArticulos(0).TamanoPantalla

            If lstRegistroArticulos(0).Chip IsNot Nothing Then
                txtChip.Text = lstRegistroArticulos(0).Chip
            End If

            If lstRegistroArticulos(0).IMEI IsNot Nothing Then
                txtIMEI.Text = lstRegistroArticulos(0).IMEI
            End If

            If lstRegistroArticulos(0).IdPertenece IsNot Nothing Then
                ddlPertenece.SelectedValue = lstRegistroArticulos(0).IdPertenece
            End If

            If lstRegistroArticulos(0).IdOperador IsNot Nothing Then
                ddlOperador.SelectedValue = lstRegistroArticulos(0).IdOperador
            End If

            If lstRegistroArticulos(0).NumeroCelular IsNot Nothing Then
                txtNumCelular.Text = lstRegistroArticulos(0).NumeroCelular
            End If

            If lstRegistroArticulos(0).CantidadMinutos IsNot Nothing Then
                txtMinutos.Text = lstRegistroArticulos(0).CantidadMinutos
            End If

            If lstRegistroArticulos(0).IdTipoProducto IsNot Nothing Then
                ddlTipoProducto.SelectedValue = lstRegistroArticulos(0).IdTipoProducto
            End If

            txtProducto.Text = lstRegistroArticulos(0).Producto

            If lstRegistroArticulos(0).IdTipoObsequio IsNot Nothing Then
                ddlTipoObsequio.SelectedValue = lstRegistroArticulos(0).IdTipoObsequio
            End If

            If lstRegistroArticulos(0).IdTipoBono IsNot Nothing Then
                ddlTipoBono.SelectedValue = lstRegistroArticulos(0).IdTipoBono
            End If

            lblIdActualizar.Text = lstRegistroArticulos(0).Id
            lblActualizar.Visible = True
            lblIdActualizar.Visible = True

            If (lstRegistroArticulos(0).IdTipoArticulo = 1) Then
                lblEstado.Visible = True
                ddlEstado.Visible = True
            Else
                lblEstado.Visible = False
                ddlEstado.Visible = False
                ddlEstado.SelectedValue = 1
            End If

            If (lstRegistroArticulos(0).IdTipoArticulo = 1 And lstRegistroArticulos(0).IdArticulo <> 4) Or (lstRegistroArticulos(0).IdTipoArticulo = 2 And lstRegistroArticulos(0).IdArticulo <> 9) Then
                lblCentroCosto.Visible = True
                ddlCentroCosto.Visible = True

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
                    lblNombreJBIJBE.Visible = True
                    txtNombreJBIJBE.Visible = True
                    lblBU.Visible = False
                    ddlBU.Visible = False
                    ddlBU.ClearSelection()
                End If

                lblCuentasContables.Visible = True
                ddlCuentasContables.Visible = True
                btnCuentasContables.Visible = True
            Else
                lblCentroCosto.Visible = False
                ddlCentroCosto.Visible = False
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

            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End If

    End Sub

    Private Sub gvProveedores_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvProveedores.RowCommand
        If e.CommandName = "Seleccionar" Then

            hfProveedor.Value = Me.gvProveedores.DataKeys(CInt(e.CommandArgument))("Identificacion")
            Me.txtProveedor.Text = Server.HtmlDecode(Me.gvProveedores.Rows(CInt(e.CommandArgument)).Cells(1).Text.ToString)
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
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

    Public Sub CargarColumnas()
        If ddlIdArticulo.SelectedValue = 1 Then
            gvArticulos.Columns(7).Visible = True  'CentroCosto
            gvArticulos.Columns(8).Visible = True  'BU
            gvArticulos.Columns(9).Visible = False  'IdTrabajo
            gvArticulos.Columns(10).Visible = False  'JobBookCodigo
            gvArticulos.Columns(11).Visible = False  'JobBookNombre
            gvArticulos.Columns(12).Visible = True  'NumeroCuentaContable
            gvArticulos.Columns(13).Visible = True  'CuentaContable
            gvArticulos.Columns(14).Visible = True 'ValorUnitario
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = True  'Symphony
            gvArticulos.Columns(18).Visible = True  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = True  'TipoComputador
            gvArticulos.Columns(21).Visible = True  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = True  'Procesador
            gvArticulos.Columns(26).Visible = True  'Memoria
            gvArticulos.Columns(27).Visible = True  'Almacenamiento
            gvArticulos.Columns(28).Visible = True  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = True  'NombreEquipo
            gvArticulos.Columns(31).Visible = True  'Office
            gvArticulos.Columns(32).Visible = True  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
            gvArticulos.Columns(48).Visible = False  'FechaFinRenta
            gvArticulos.Columns(49).Visible = False  'ProductoPapeleria
        End If

        If ddlIdArticulo.SelectedValue = 1 And ddlIdPertenecePC.SelectedValue = 2 Then
            gvArticulos.Columns(48).Visible = True  'FechaFinRenta
        End If

        If ddlIdArticulo.SelectedValue = 2 Then
            gvArticulos.Columns(7).Visible = True  'CentroCosto
            gvArticulos.Columns(8).Visible = True  'BU
            gvArticulos.Columns(9).Visible = False  'IdTrabajo
            gvArticulos.Columns(10).Visible = False  'JobBookCodigo
            gvArticulos.Columns(11).Visible = False  'JobBookNombre
            gvArticulos.Columns(12).Visible = True  'NumeroCuentaContable
            gvArticulos.Columns(13).Visible = True  'CuentaContable
            gvArticulos.Columns(14).Visible = True 'ValorUnitario
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = True  'Symphony
            gvArticulos.Columns(18).Visible = True  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = True  'Procesador
            gvArticulos.Columns(26).Visible = True  'Memoria
            gvArticulos.Columns(27).Visible = True  'Almacenamiento
            gvArticulos.Columns(28).Visible = True  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = True  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = True  'TipoServidor
            gvArticulos.Columns(34).Visible = True  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
            gvArticulos.Columns(48).Visible = False  'FechaFinRenta
            gvArticulos.Columns(49).Visible = False  'ProductoPapeleria
        End If

        If ddlIdArticulo.SelectedValue = 3 Then
            gvArticulos.Columns(7).Visible = True  'CentroCosto
            gvArticulos.Columns(8).Visible = True  'BU
            gvArticulos.Columns(9).Visible = False  'IdTrabajo
            gvArticulos.Columns(10).Visible = False  'JobBookCodigo
            gvArticulos.Columns(11).Visible = False  'JobBookNombre
            gvArticulos.Columns(12).Visible = True  'NumeroCuentaContable
            gvArticulos.Columns(13).Visible = True  'CuentaContable
            gvArticulos.Columns(14).Visible = True 'ValorUnitario
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = True  'Symphony
            gvArticulos.Columns(18).Visible = True  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = True  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
            gvArticulos.Columns(48).Visible = False  'FechaFinRenta
            gvArticulos.Columns(49).Visible = False  'ProductoPapeleria
        End If

        If ddlIdArticulo.SelectedValue = 4 Then
            gvArticulos.Columns(7).Visible = False  'CentroCosto
            gvArticulos.Columns(8).Visible = False  'BU
            gvArticulos.Columns(9).Visible = False  'IdTrabajo
            gvArticulos.Columns(10).Visible = False  'JobBookCodigo
            gvArticulos.Columns(11).Visible = False  'JobBookNombre
            gvArticulos.Columns(12).Visible = False  'NumeroCuentaContable
            gvArticulos.Columns(13).Visible = False  'CuentaContable
            gvArticulos.Columns(14).Visible = True 'ValorUnitario
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = True  'Procesador
            gvArticulos.Columns(26).Visible = True  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = True  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = True  'IdTablet
            gvArticulos.Columns(36).Visible = True  'IdSTG
            gvArticulos.Columns(37).Visible = True  'TamanoPantalla
            gvArticulos.Columns(38).Visible = True  'Chip
            gvArticulos.Columns(39).Visible = True  'IMEI
            gvArticulos.Columns(40).Visible = True  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
            gvArticulos.Columns(48).Visible = False  'FechaFinRenta
            gvArticulos.Columns(49).Visible = False  'ProductoPapeleria
        End If

        If ddlIdArticulo.SelectedValue = 5 Then
            gvArticulos.Columns(7).Visible = True  'CentroCosto
            gvArticulos.Columns(8).Visible = True  'BU
            gvArticulos.Columns(9).Visible = False  'IdTrabajo
            gvArticulos.Columns(10).Visible = False  'JobBookCodigo
            gvArticulos.Columns(11).Visible = False  'JobBookNombre
            gvArticulos.Columns(12).Visible = True  'NumeroCuentaContable
            gvArticulos.Columns(13).Visible = True  'CuentaContable
            gvArticulos.Columns(14).Visible = True 'ValorUnitario
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = True  'Operador
            gvArticulos.Columns(42).Visible = True  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
            gvArticulos.Columns(48).Visible = False  'FechaFinRenta
            gvArticulos.Columns(49).Visible = False  'ProductoPapeleria
        End If

        If ddlIdArticulo.SelectedValue = 6 Then
            gvArticulos.Columns(7).Visible = True  'CentroCosto
            gvArticulos.Columns(8).Visible = True  'BU
            gvArticulos.Columns(9).Visible = False  'IdTrabajo
            gvArticulos.Columns(10).Visible = False  'JobBookCodigo
            gvArticulos.Columns(11).Visible = False  'JobBookNombre
            gvArticulos.Columns(12).Visible = True  'NumeroCuentaContable
            gvArticulos.Columns(13).Visible = True  'CuentaContable
            gvArticulos.Columns(14).Visible = True 'ValorUnitario
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = False  'Marca
            gvArticulos.Columns(24).Visible = False  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = True  'Operador
            gvArticulos.Columns(42).Visible = True  'NumeroCelular
            gvArticulos.Columns(43).Visible = True  'CantidadMinutos
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
            gvArticulos.Columns(48).Visible = False  'FechaFinRenta
            gvArticulos.Columns(49).Visible = False  'ProductoPapeleria
        End If

        If ddlIdArticulo.SelectedValue = 7 Then
            gvArticulos.Columns(7).Visible = False  'CentroCosto
            gvArticulos.Columns(8).Visible = False  'BU
            gvArticulos.Columns(9).Visible = False  'IdTrabajo
            gvArticulos.Columns(10).Visible = True  'JobBookCodigo
            gvArticulos.Columns(11).Visible = True  'JobBookNombre
            gvArticulos.Columns(12).Visible = False  'NumeroCuentaContable
            gvArticulos.Columns(13).Visible = False  'CuentaContable
            gvArticulos.Columns(14).Visible = True 'ValorUnitario
            gvArticulos.Columns(15).Visible = False  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = False  'Marca
            gvArticulos.Columns(24).Visible = False  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
            gvArticulos.Columns(44).Visible = True  'TipoProducto
            gvArticulos.Columns(45).Visible = True  'Producto
            gvArticulos.Columns(46).Visible = True  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
            gvArticulos.Columns(48).Visible = False  'FechaFinRenta
            gvArticulos.Columns(49).Visible = False  'ProductoPapeleria
        End If

        If ddlIdArticulo.SelectedValue = 8 Then
            gvArticulos.Columns(7).Visible = True  'CentroCosto
            gvArticulos.Columns(8).Visible = False  'BU
            gvArticulos.Columns(9).Visible = True  'IdTrabajo
            gvArticulos.Columns(10).Visible = True  'JobBookCodigo
            gvArticulos.Columns(11).Visible = True  'JobBookNombre
            gvArticulos.Columns(12).Visible = True  'NumeroCuentaContable
            gvArticulos.Columns(13).Visible = True  'CuentaContable
            gvArticulos.Columns(14).Visible = False 'ValorUnitario
            gvArticulos.Columns(15).Visible = False  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = False  'Marca
            gvArticulos.Columns(24).Visible = False  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = True  'TipoBono
            gvArticulos.Columns(48).Visible = False  'FechaFinRenta
            gvArticulos.Columns(49).Visible = False  'ProductoPapeleria
        End If

        If ddlIdArticulo.SelectedValue = 9 Then
            gvArticulos.Columns(7).Visible = False  'CentroCosto
            gvArticulos.Columns(8).Visible = False  'BU
            gvArticulos.Columns(9).Visible = False  'IdTrabajo
            gvArticulos.Columns(10).Visible = False  'JobBookCodigo
            gvArticulos.Columns(11).Visible = False  'JobBookNombre
            gvArticulos.Columns(12).Visible = False  'NumeroCuentaContable
            gvArticulos.Columns(13).Visible = False  'CuentaContable
            gvArticulos.Columns(14).Visible = True 'ValorUnitario
            gvArticulos.Columns(15).Visible = False  'Estado
            gvArticulos.Columns(17).Visible = False  'Symphony
            gvArticulos.Columns(18).Visible = False  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = False  'Marca
            gvArticulos.Columns(24).Visible = False  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = False  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
            gvArticulos.Columns(48).Visible = False  'FechaFinRenta
            gvArticulos.Columns(49).Visible = False  'ProductoPapeleria
        End If


        If ddlIdArticulo.SelectedValue = 10 Then
            gvArticulos.Columns(7).Visible = True  'CentroCosto
            gvArticulos.Columns(8).Visible = True  'BU
            gvArticulos.Columns(9).Visible = False  'IdTrabajo
            gvArticulos.Columns(10).Visible = False  'JobBookCodigo
            gvArticulos.Columns(11).Visible = False  'JobBookNombre
            gvArticulos.Columns(12).Visible = True  'NumeroCuentaContable
            gvArticulos.Columns(13).Visible = True  'CuentaContable
            gvArticulos.Columns(14).Visible = True 'ValorUnitario
            gvArticulos.Columns(15).Visible = True  'Estado
            gvArticulos.Columns(17).Visible = True  'Symphony
            gvArticulos.Columns(18).Visible = True  'IdFisico
            gvArticulos.Columns(19).Visible = True  'Sede
            gvArticulos.Columns(20).Visible = False  'TipoComputador
            gvArticulos.Columns(21).Visible = False  'PertenecePC
            gvArticulos.Columns(22).Visible = False  'TipoPeriferico
            gvArticulos.Columns(23).Visible = True  'Marca
            gvArticulos.Columns(24).Visible = True  'Modelo
            gvArticulos.Columns(25).Visible = False  'Procesador
            gvArticulos.Columns(26).Visible = False  'Memoria
            gvArticulos.Columns(27).Visible = False  'Almacenamiento
            gvArticulos.Columns(28).Visible = False  'SistemaOperativo
            gvArticulos.Columns(29).Visible = True  'Serial
            gvArticulos.Columns(30).Visible = False  'NombreEquipo
            gvArticulos.Columns(31).Visible = False  'Office
            gvArticulos.Columns(32).Visible = False  'Programas
            gvArticulos.Columns(33).Visible = False  'TipoServidor
            gvArticulos.Columns(34).Visible = False  'Raid
            gvArticulos.Columns(35).Visible = False  'IdTablet
            gvArticulos.Columns(36).Visible = False  'IdSTG
            gvArticulos.Columns(37).Visible = False  'TamanoPantalla
            gvArticulos.Columns(38).Visible = False  'Chip
            gvArticulos.Columns(39).Visible = False  'IMEI
            gvArticulos.Columns(40).Visible = False  'Pertenece
            gvArticulos.Columns(41).Visible = False  'Operador
            gvArticulos.Columns(42).Visible = False  'NumeroCelular
            gvArticulos.Columns(43).Visible = False  'CantidadMinutos
            gvArticulos.Columns(44).Visible = False  'TipoProducto
            gvArticulos.Columns(45).Visible = False  'Producto
            gvArticulos.Columns(46).Visible = False  'TipoObsequio
            gvArticulos.Columns(47).Visible = False  'TipoBono
            gvArticulos.Columns(48).Visible = False  'FechaFinRenta
            gvArticulos.Columns(49).Visible = False  'ProductoPapeleria
        End If

		If ddlIdArticulo.SelectedValue = 11 Then
			gvArticulos.Columns(7).Visible = False  'CentroCosto
			gvArticulos.Columns(8).Visible = False  'BU
			gvArticulos.Columns(9).Visible = False  'IdTrabajo
			gvArticulos.Columns(10).Visible = False  'JobBookCodigo
			gvArticulos.Columns(11).Visible = False  'JobBookNombre
			gvArticulos.Columns(12).Visible = False  'NumeroCuentaContable
			gvArticulos.Columns(13).Visible = False  'CuentaContable
			gvArticulos.Columns(14).Visible = True 'ValorUnitario
			gvArticulos.Columns(15).Visible = False  'Estado
			gvArticulos.Columns(17).Visible = False  'Symphony
			gvArticulos.Columns(18).Visible = False  'IdFisico
			gvArticulos.Columns(19).Visible = True  'Sede
			gvArticulos.Columns(20).Visible = False  'TipoComputador
			gvArticulos.Columns(21).Visible = False  'PertenecePC
			gvArticulos.Columns(22).Visible = False  'TipoPeriferico
			gvArticulos.Columns(23).Visible = False  'Marca
			gvArticulos.Columns(24).Visible = False  'Modelo
			gvArticulos.Columns(25).Visible = False  'Procesador
			gvArticulos.Columns(26).Visible = False  'Memoria
			gvArticulos.Columns(27).Visible = False  'Almacenamiento
			gvArticulos.Columns(28).Visible = False  'SistemaOperativo
			gvArticulos.Columns(29).Visible = False  'Serial
			gvArticulos.Columns(30).Visible = False  'NombreEquipo
			gvArticulos.Columns(31).Visible = False  'Office
			gvArticulos.Columns(32).Visible = False  'Programas
			gvArticulos.Columns(33).Visible = False  'TipoServidor
			gvArticulos.Columns(34).Visible = False  'Raid
			gvArticulos.Columns(35).Visible = False  'IdTablet
			gvArticulos.Columns(36).Visible = False  'IdSTG
			gvArticulos.Columns(37).Visible = False  'TamanoPantalla
			gvArticulos.Columns(38).Visible = False  'Chip
			gvArticulos.Columns(39).Visible = False  'IMEI
			gvArticulos.Columns(40).Visible = False  'Pertenece
			gvArticulos.Columns(41).Visible = False  'Operador
			gvArticulos.Columns(42).Visible = False  'NumeroCelular
			gvArticulos.Columns(43).Visible = False  'CantidadMinutos
			gvArticulos.Columns(44).Visible = False  'TipoProducto
			gvArticulos.Columns(45).Visible = False  'Producto
			gvArticulos.Columns(46).Visible = False  'TipoObsequio
			gvArticulos.Columns(47).Visible = False  'TipoBono
			gvArticulos.Columns(48).Visible = False  'FechaFinRenta
			gvArticulos.Columns(49).Visible = True  'ProductoPapeleria
		End If

		Try
			gvArticulos.Columns(31).HeaderText = "Serial Windows"
		Catch ex As Exception

		End Try

	End Sub

    Function ObtenerRegistroArticulos(ByVal Id As Int64?) As List(Of INV_RegistroArticulos_Get_Result)
        Dim lstRegistroArticulos As New List(Of INV_RegistroArticulos_Get_Result)
        Dim RecordArticulos As New Inventario
        lstRegistroArticulos = RecordArticulos.obtenerRegistroArticulosxId(Id)
        Return lstRegistroArticulos
    End Function

    Function obtenerRegistrosArticulos() As List(Of INV_RegistroArticulos_Get_Result)
        Dim oBusqueda As New Inventario

        Dim Articulo As Int64? = Nothing
        Dim TipoComputador As Int64? = Nothing
        Dim PertenecePC As Int16? = Nothing
        Dim TipoPeriferico As Int64? = Nothing
        Dim TipoProducto As Int64? = Nothing
        Dim Estado As Int64? = Nothing
        Dim Sede As Int64? = Nothing
        Dim TodosCampos As String = Nothing

        If Not ddlIdArticulo.SelectedValue = -1 Then Articulo = ddlIdArticulo.SelectedValue
        If Not ddlIdTipoComputador.SelectedValue = -1 Then TipoComputador = ddlIdTipoComputador.SelectedValue
        If Not ddlIdPertenecePC.SelectedValue = -1 Then PertenecePC = ddlIdPertenecePC.SelectedValue
        If Not ddlIdPeriferico.SelectedValue = -1 Then TipoPeriferico = ddlIdPeriferico.SelectedValue
        If Not ddlIdTipoProducto.SelectedValue = -1 Then TipoProducto = ddlIdTipoProducto.SelectedValue
        If Not ddlIdEstado.SelectedValue = -1 Then Estado = ddlIdEstado.SelectedValue
        If Not ddlIdSede.SelectedValue = -1 Then Sede = ddlIdSede.SelectedValue
        If Not txtTodosCampos.Text = "" Then TodosCampos = txtTodosCampos.Text

		Return oBusqueda.obtenerRegistroArticulos(Nothing, Nothing, Articulo, TipoComputador, PertenecePC, TipoPeriferico, TipoProducto, Estado, Sede, Nothing, Nothing, Nothing, Nothing, TodosCampos)
	End Function
#End Region

#Region "DDL"
    Sub CargarArticulos()
        Dim oArticulos As New Inventario
        Dim oUsuario As New US.UsuariosUnidades
        Dim Usuario = oUsuario.obtenerUnidadesXUsuario(Session("IDUsuario").ToString, True, Nothing)

        For Each oGrupoUnidad In Usuario
            Dim GrupoUnidad As Int32? = oGrupoUnidad.GrupoUnidadId
            Dim Busqueda = oArticulos.obtenerArticulosxTipoArticulo(ddlTipoArticulo.SelectedValue, GrupoUnidad)
            If Busqueda.Count > 0 Then
                Me.ddlArticulo.DataSource = oArticulos.obtenerArticulosxTipoArticulo(ddlTipoArticulo.SelectedValue, GrupoUnidad)
                Exit For
            Else
                Me.ddlArticulo.Items.Clear()
            End If
        Next

        Me.ddlArticulo.DataValueField = "Id"
        Me.ddlArticulo.DataTextField = "Articulo"
        Me.ddlArticulo.DataBind()
        Me.ddlArticulo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})

    End Sub

    Sub CargarListaArticulos()
        Dim oArticulos As New Inventario
        Me.ddlIdArticulo.DataSource = oArticulos.obtenerArticulosTodos
        Me.ddlIdArticulo.DataValueField = "Id"
        Me.ddlIdArticulo.DataTextField = "Articulo"
        Me.ddlIdArticulo.DataBind()
        Me.ddlIdArticulo.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarListaPapeleria()
        Dim oArticulos As New Inventario
        Me.ddlPapeleria.DataSource = oArticulos.obtenerListaPapeleria
        Me.ddlPapeleria.DataValueField = "Id"
        Me.ddlPapeleria.DataTextField = "Producto"
        Me.ddlPapeleria.DataBind()
        Me.ddlPapeleria.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarPerifericos()
        Dim oPerifericos As New Inventario
        Me.ddlTipoPeriferico.DataSource = oPerifericos.obtenerDispositivosPerifericos
        Me.ddlTipoPeriferico.DataValueField = "Id"
        Me.ddlTipoPeriferico.DataTextField = "Periferico"
        Me.ddlTipoPeriferico.DataBind()
        Me.ddlTipoPeriferico.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarIdPerifericos()
        Dim oPerifericos As New Inventario
        Me.ddlIdPeriferico.DataSource = oPerifericos.obtenerDispositivosPerifericos
        Me.ddlIdPeriferico.DataValueField = "Id"
        Me.ddlIdPeriferico.DataTextField = "Periferico"
        Me.ddlIdPeriferico.DataBind()
        Me.ddlIdPeriferico.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub EstadosArticulos()
        Dim oEstados As New Inventario
        Me.ddlEstado.DataSource = oEstados.obtenerEstadosArticulos
        Me.ddlEstado.DataValueField = "Id"
        Me.ddlEstado.DataTextField = "Estado"
        Me.ddlEstado.DataBind()
        Me.ddlEstado.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub IdEstadosArticulos()
        Dim oEstados As New Inventario
        Me.ddlIdEstado.DataSource = oEstados.obtenerEstadosArticulos
        Me.ddlIdEstado.DataValueField = "Id"
        Me.ddlIdEstado.DataTextField = "Estado"
        Me.ddlIdEstado.DataBind()
        Me.ddlIdEstado.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarBU()
        Dim oBU As New Inventario
        Me.ddlBU.DataSource = oBU.obtenerBU
        Me.ddlBU.DataValueField = "Id"
        Me.ddlBU.DataTextField = "Nombre"
        Me.ddlBU.DataBind()
        Me.ddlBU.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub cargarCuentasContablesDropDown()
        ddlCuentasContables.DataSource = obtenerCuentasContables(Nothing, Nothing)
        ddlCuentasContables.DataValueField = "id"
        ddlCuentasContables.DataTextField = "descripcion"
        ddlCuentasContables.DataBind()
        ddlCuentasContables.Items.Insert(0, New System.Web.UI.WebControls.ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarSedes()
        Dim oSedes As New Inventario
        Me.ddlSede.DataSource = oSedes.obtenerSedes
        Me.ddlSede.DataValueField = "Id"
        Me.ddlSede.DataTextField = "Sede"
        Me.ddlSede.DataBind()
        Me.ddlSede.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

    Sub CargarIdSedes()
        Dim oSedes As New Inventario
        Me.ddlIdSede.DataSource = oSedes.obtenerSedes
        Me.ddlIdSede.DataValueField = "Id"
        Me.ddlIdSede.DataTextField = "Sede"
        Me.ddlIdSede.DataBind()
        Me.ddlIdSede.Items.Insert(0, New ListItem With {.Value = "-1", .Text = "--Seleccione--"})
    End Sub

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

#Region "Formulario"

    Function obtenerCentroCosto(ByVal tipo As Byte, ByVal valorBusqueda As String) As List(Of FI_JBE_JBI_CC_Get_Result)
        Dim o As New FI.Ordenes
        Return o.ObtenerJBE_JBI_CC(tipo, valorBusqueda)
    End Function

    Sub CargarFormulario()
        If ddlArticulo.SelectedValue = 1 Then
            lblCantidad.Visible = False
            txtCantidad.Visible = False
            lblValorUnitario.Visible = True
            txtValorUnitario.Visible = True
            lblCentroCosto.Visible = True
            ddlCentroCosto.Visible = True
            lblCuentasContables.Visible = True
            ddlCuentasContables.Visible = True
            btnCuentasContables.Visible = True
            lblNumeroPV.Visible = True
            txtNumeroPV.Visible = True
            lblProveedor.Visible = True
            txtProveedor.Visible = True
            btnSearchProveedor.Visible = True
            lblSymphony.Visible = True
            txtSymphony.Visible = True
            lblIdFisico.Visible = True
            txtIdFisico.Visible = True
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = True
            ddlTipoComputador.Visible = True
            lblPertenecePC.Visible = True
            ddlPertenecePC.Visible = True
            lblTipoPeriferico.Visible = False
            ddlTipoPeriferico.Visible = False
            ddlTipoPeriferico.ClearSelection()
            lblMarca.Visible = True
            txtMarca.Visible = True
            lblModelo.Visible = True
            txtModelo.Visible = True
            lblProcesador.Visible = True
            txtProcesador.Visible = True
            lblMemoria.Visible = True
            txtMemoria.Visible = True
            lblAlmacenamiento.Visible = True
            txtAlmacenamiento.Visible = True
            lblSistemaOperativo.Visible = True
            txtSistemaOperativo.Visible = True
            lblSerial.Visible = True
            txtSerial.Visible = True
            lblNombreEquipo.Visible = True
            txtNombreEquipo.Visible = True
            lblOffice.Visible = True
            txtOffice.Visible = True
            lblProgramas.Visible = True
            txtProgramas.Visible = True
            lblTipoServidor.Visible = False
            txtTipoServidor.Visible = False
            txtTipoServidor.Text = ""
            lblRaid.Visible = False
            txtRaid.Visible = False
            txtRaid.Text = ""
            lblIdTablet.Visible = False
            txtIdTablet.Visible = False
            txtIdTablet.Text = ""
            lblIdSTG.Visible = False
            txtIdSTG.Visible = False
            txtIdSTG.Text = ""
            lblTamanoPantalla.Visible = False
            txtTamanoPantalla.Visible = False
            txtTamanoPantalla.Text = ""
            lblChip.Visible = False
            txtChip.Visible = False
            txtChip.Text = ""
            lblIMEI.Visible = False
            txtIMEI.Visible = False
            txtIMEI.Text = ""
            lblPertenece.Visible = False
            ddlPertenece.Visible = False
            ddlPertenece.ClearSelection()
            lblOperador.Visible = False
            ddlOperador.Visible = False
            ddlOperador.ClearSelection()
            lblNumCelular.Visible = False
            txtNumCelular.Visible = False
            txtNumCelular.Text = ""
            lblMinutos.Visible = False
            txtMinutos.Visible = False
            txtMinutos.Text = ""
            lblTipoProducto.Visible = False
            ddlTipoProducto.Visible = False
            ddlTipoProducto.ClearSelection()
            lblProducto.Visible = False
            txtProducto.Visible = False
            txtProducto.Text = ""
            lblTipoObsequio.Visible = False
            ddlTipoObsequio.Visible = False
            ddlTipoObsequio.ClearSelection()
            lblTipoBono.Visible = False
            ddlTipoBono.Visible = False
            ddlTipoBono.ClearSelection()
            lblPapeleria.Visible = False
            ddlPapeleria.Visible = False
            ddlPapeleria.ClearSelection()

        End If

        If ddlArticulo.SelectedValue = 2 Then
            lblCantidad.Visible = False
            txtCantidad.Visible = False
            lblValorUnitario.Visible = True
            txtValorUnitario.Visible = True
            lblCentroCosto.Visible = True
            ddlCentroCosto.Visible = True
            lblCuentasContables.Visible = True
            ddlCuentasContables.Visible = True
            btnCuentasContables.Visible = True
            lblNumeroPV.Visible = True
            txtNumeroPV.Visible = True
            lblProveedor.Visible = True
            txtProveedor.Visible = True
            btnSearchProveedor.Visible = True
            lblSymphony.Visible = True
            txtSymphony.Visible = True
            lblIdFisico.Visible = True
            txtIdFisico.Visible = True
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = False
            ddlTipoComputador.Visible = False
            ddlTipoComputador.ClearSelection()
            lblPertenecePC.Visible = False
            ddlPertenecePC.Visible = False
            ddlPertenecePC.ClearSelection()
            lblTipoPeriferico.Visible = False
            ddlTipoPeriferico.Visible = False
            ddlTipoPeriferico.ClearSelection()
            lblMarca.Visible = True
            txtMarca.Visible = True
            lblModelo.Visible = True
            txtModelo.Visible = True
            lblProcesador.Visible = True
            txtProcesador.Visible = True
            lblMemoria.Visible = True
            txtMemoria.Visible = True
            lblAlmacenamiento.Visible = True
            txtAlmacenamiento.Visible = True
            lblSistemaOperativo.Visible = True
            txtSistemaOperativo.Visible = True
            lblSerial.Visible = True
            txtSerial.Visible = True
            lblNombreEquipo.Visible = True
            txtNombreEquipo.Visible = True
            lblOffice.Visible = False
            txtOffice.Visible = False
            txtOffice.Text = ""
            lblProgramas.Visible = False
            txtProgramas.Visible = False
            txtProgramas.Text = ""
            lblTipoServidor.Visible = True
            txtTipoServidor.Visible = True
            lblRaid.Visible = True
            txtRaid.Visible = True
            lblIdTablet.Visible = False
            txtIdTablet.Visible = False
            txtIdTablet.Text = ""
            lblIdSTG.Visible = False
            txtIdSTG.Visible = False
            txtIdSTG.Text = ""
            lblTamanoPantalla.Visible = False
            txtTamanoPantalla.Visible = False
            txtTamanoPantalla.Text = ""
            lblChip.Visible = False
            txtChip.Visible = False
            txtChip.Text = ""
            lblIMEI.Visible = False
            txtIMEI.Visible = False
            txtIMEI.Text = ""
            lblPertenece.Visible = False
            ddlPertenece.Visible = False
            ddlPertenece.ClearSelection()
            lblOperador.Visible = False
            ddlOperador.Visible = False
            ddlOperador.ClearSelection()
            lblNumCelular.Visible = False
            txtNumCelular.Visible = False
            txtNumCelular.Text = ""
            lblMinutos.Visible = False
            txtMinutos.Visible = False
            txtMinutos.Text = ""
            lblTipoProducto.Visible = False
            ddlTipoProducto.Visible = False
            ddlTipoProducto.ClearSelection()
            lblProducto.Visible = False
            txtProducto.Visible = False
            txtProducto.Text = ""
            lblTipoObsequio.Visible = False
            ddlTipoObsequio.Visible = False
            ddlTipoObsequio.ClearSelection()
            lblTipoBono.Visible = False
            ddlTipoBono.Visible = False
            ddlTipoBono.ClearSelection()
            lblPapeleria.Visible = False
            ddlPapeleria.Visible = False
            ddlPapeleria.ClearSelection()

        End If

        If ddlArticulo.SelectedValue = 3 Then
            lblCantidad.Visible = False
            txtCantidad.Visible = False
            lblValorUnitario.Visible = True
            txtValorUnitario.Visible = True
            lblCentroCosto.Visible = True
            ddlCentroCosto.Visible = True
            lblCuentasContables.Visible = True
            ddlCuentasContables.Visible = True
            btnCuentasContables.Visible = True
            lblNumeroPV.Visible = True
            txtNumeroPV.Visible = True
            lblProveedor.Visible = True
            txtProveedor.Visible = True
            btnSearchProveedor.Visible = True
            lblSymphony.Visible = True
            txtSymphony.Visible = True
            lblIdFisico.Visible = True
            txtIdFisico.Visible = True
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = False
            ddlTipoComputador.Visible = False
            ddlTipoComputador.ClearSelection()
            lblPertenecePC.Visible = False
            ddlPertenecePC.Visible = False
            ddlPertenecePC.ClearSelection()
            lblTipoPeriferico.Visible = True
            ddlTipoPeriferico.Visible = True
            lblMarca.Visible = True
            txtMarca.Visible = True
            lblModelo.Visible = True
            txtModelo.Visible = True
            lblProcesador.Visible = False
            txtProcesador.Visible = False
            txtProcesador.Text = ""
            lblMemoria.Visible = False
            txtMemoria.Visible = False
            txtMemoria.Text = ""
            lblAlmacenamiento.Visible = False
            txtAlmacenamiento.Visible = False
            txtAlmacenamiento.Text = ""
            lblSistemaOperativo.Visible = False
            txtSistemaOperativo.Visible = False
            txtSistemaOperativo.Text = ""
            lblSerial.Visible = True
            txtSerial.Visible = True
            lblNombreEquipo.Visible = False
            txtNombreEquipo.Visible = False
            txtNombreEquipo.Text = ""
            lblOffice.Visible = False
            txtOffice.Visible = False
            txtOffice.Text = ""
            lblProgramas.Visible = False
            txtProgramas.Visible = False
            txtProgramas.Text = ""
            lblTipoServidor.Visible = False
            txtTipoServidor.Visible = False
            txtTipoServidor.Text = ""
            lblRaid.Visible = False
            txtRaid.Visible = False
            txtRaid.Text = ""
            lblIdTablet.Visible = False
            txtIdTablet.Visible = False
            txtIdTablet.Text = ""
            lblIdSTG.Visible = False
            txtIdSTG.Visible = False
            txtIdSTG.Text = ""
            lblTamanoPantalla.Visible = False
            txtTamanoPantalla.Visible = False
            txtTamanoPantalla.Text = ""
            lblChip.Visible = False
            txtChip.Visible = False
            txtChip.Text = ""
            lblIMEI.Visible = False
            txtIMEI.Visible = False
            txtIMEI.Text = ""
            lblPertenece.Visible = False
            ddlPertenece.Visible = False
            ddlPertenece.ClearSelection()
            lblOperador.Visible = False
            ddlOperador.Visible = False
            ddlOperador.ClearSelection()
            lblNumCelular.Visible = False
            txtNumCelular.Visible = False
            txtNumCelular.Text = ""
            lblMinutos.Visible = False
            txtMinutos.Visible = False
            txtMinutos.Text = ""
            lblTipoProducto.Visible = False
            ddlTipoProducto.Visible = False
            ddlTipoProducto.ClearSelection()
            lblProducto.Visible = False
            txtProducto.Visible = False
            txtProducto.Text = ""
            lblTipoObsequio.Visible = False
            ddlTipoObsequio.Visible = False
            ddlTipoObsequio.ClearSelection()
            lblTipoBono.Visible = False
            ddlTipoBono.Visible = False
            ddlTipoBono.ClearSelection()
            lblPapeleria.Visible = False
            ddlPapeleria.Visible = False
            ddlPapeleria.ClearSelection()

        End If

        If ddlArticulo.SelectedValue = 4 Then
            lblCantidad.Visible = False
            txtCantidad.Visible = False
            lblValorUnitario.Visible = True
            txtValorUnitario.Visible = True
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
            lblNumeroPV.Visible = False
            txtNumeroPV.Visible = False
            lblProveedor.Visible = False
            txtProveedor.Visible = False
            btnSearchProveedor.Visible = False
            lblSymphony.Visible = False
            txtSymphony.Visible = False
            txtSymphony.Text = ""
            lblIdFisico.Visible = False
            txtIdFisico.Visible = False
            txtIdFisico.Text = ""
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = False
            ddlTipoComputador.Visible = False
            ddlTipoComputador.ClearSelection()
            lblPertenecePC.Visible = False
            ddlPertenecePC.Visible = False
            ddlPertenecePC.ClearSelection()
            lblTipoPeriferico.Visible = False
            ddlTipoPeriferico.Visible = False
            ddlTipoPeriferico.ClearSelection()
            lblMarca.Visible = True
            txtMarca.Visible = True
            lblModelo.Visible = True
            txtModelo.Visible = True
            lblProcesador.Visible = True
            txtProcesador.Visible = True
            lblMemoria.Visible = True
            txtMemoria.Visible = True
            lblAlmacenamiento.Visible = False
            txtAlmacenamiento.Visible = False
            txtAlmacenamiento.Text = ""
            lblSistemaOperativo.Visible = True
            txtSistemaOperativo.Visible = True
            lblSerial.Visible = False
            txtSerial.Visible = False
            txtSerial.Text = ""
            lblNombreEquipo.Visible = False
            txtNombreEquipo.Visible = False
            txtNombreEquipo.Text = ""
            lblOffice.Visible = False
            txtOffice.Visible = False
            txtOffice.Text = ""
            lblProgramas.Visible = False
            txtProgramas.Visible = False
            txtProgramas.Text = ""
            lblTipoServidor.Visible = False
            txtTipoServidor.Visible = False
            txtTipoServidor.Text = ""
            lblRaid.Visible = False
            txtRaid.Visible = False
            txtRaid.Text = ""
            lblIdTablet.Visible = True
            txtIdTablet.Visible = True
            lblIdSTG.Visible = True
            txtIdSTG.Visible = True
            lblTamanoPantalla.Visible = True
            txtTamanoPantalla.Visible = True
            lblChip.Visible = True
            txtChip.Visible = True
            lblIMEI.Visible = True
            txtIMEI.Visible = True
            lblPertenece.Visible = True
            ddlPertenece.Visible = True
            lblOperador.Visible = False
            ddlOperador.Visible = False
            ddlOperador.ClearSelection()
            lblNumCelular.Visible = False
            txtNumCelular.Visible = False
            txtNumCelular.Text = ""
            lblMinutos.Visible = False
            txtMinutos.Visible = False
            txtMinutos.Text = ""
            lblTipoProducto.Visible = False
            ddlTipoProducto.Visible = False
            ddlTipoProducto.ClearSelection()
            lblProducto.Visible = False
            txtProducto.Visible = False
            txtProducto.Text = ""
            lblTipoObsequio.Visible = False
            ddlTipoObsequio.Visible = False
            ddlTipoObsequio.ClearSelection()
            lblTipoBono.Visible = False
            ddlTipoBono.Visible = False
            ddlTipoBono.ClearSelection()
            lblPapeleria.Visible = False
            ddlPapeleria.Visible = False
            ddlPapeleria.ClearSelection()

        End If

        If ddlArticulo.SelectedValue = 5 Then
            lblCantidad.Visible = False
            txtCantidad.Visible = False
            lblValorUnitario.Visible = True
            txtValorUnitario.Visible = True
            lblCentroCosto.Visible = True
            ddlCentroCosto.Visible = True
            lblCuentasContables.Visible = True
            ddlCuentasContables.Visible = True
            btnCuentasContables.Visible = True
            lblNumeroPV.Visible = False
            txtNumeroPV.Visible = False
            lblProveedor.Visible = False
            txtProveedor.Visible = False
            btnSearchProveedor.Visible = False
            lblSymphony.Visible = False
            txtSymphony.Visible = False
            txtSymphony.Text = ""
            lblIdFisico.Visible = False
            txtIdFisico.Visible = False
            txtIdFisico.Text = ""
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = False
            ddlTipoComputador.Visible = False
            ddlTipoComputador.ClearSelection()
            lblPertenecePC.Visible = False
            ddlPertenecePC.Visible = False
            ddlPertenecePC.ClearSelection()
            lblTipoPeriferico.Visible = False
            ddlTipoPeriferico.Visible = False
            ddlTipoPeriferico.ClearSelection()
            lblMarca.Visible = True
            txtMarca.Visible = True
            lblModelo.Visible = True
            txtModelo.Visible = True
            lblProcesador.Visible = False
            txtProcesador.Visible = False
            txtProcesador.Text = ""
            lblMemoria.Visible = False
            txtMemoria.Visible = False
            txtMemoria.Text = ""
            lblAlmacenamiento.Visible = False
            txtAlmacenamiento.Visible = False
            txtAlmacenamiento.Text = ""
            lblSistemaOperativo.Visible = False
            txtSistemaOperativo.Visible = False
            txtSistemaOperativo.Text = ""
            lblSerial.Visible = False
            txtSerial.Visible = False
            txtSerial.Text = ""
            lblNombreEquipo.Visible = False
            txtNombreEquipo.Visible = False
            txtNombreEquipo.Text = ""
            lblOffice.Visible = False
            txtOffice.Visible = False
            txtOffice.Text = ""
            lblProgramas.Visible = False
            txtProgramas.Visible = False
            txtProgramas.Text = ""
            lblTipoServidor.Visible = False
            txtTipoServidor.Visible = False
            txtTipoServidor.Text = ""
            lblRaid.Visible = False
            txtRaid.Visible = False
            txtRaid.Text = ""
            lblIdTablet.Visible = False
            txtIdTablet.Visible = False
            txtIdTablet.Text = ""
            lblIdSTG.Visible = False
            txtIdSTG.Visible = False
            txtIdSTG.Text = ""
            lblTamanoPantalla.Visible = False
            txtTamanoPantalla.Visible = False
            txtTamanoPantalla.Text = ""
            lblChip.Visible = False
            txtChip.Visible = False
            txtChip.Text = ""
            lblIMEI.Visible = True
            txtIMEI.Visible = True
            lblPertenece.Visible = False
            ddlPertenece.Visible = False
            ddlPertenece.ClearSelection()
            lblOperador.Visible = True
            ddlOperador.Visible = True
            lblNumCelular.Visible = True
            txtNumCelular.Visible = True
            lblMinutos.Visible = False
            txtMinutos.Visible = False
            txtMinutos.Text = ""
            lblTipoProducto.Visible = False
            ddlTipoProducto.Visible = False
            ddlTipoProducto.ClearSelection()
            lblProducto.Visible = False
            txtProducto.Visible = False
            txtProducto.Text = ""
            lblTipoObsequio.Visible = False
            ddlTipoObsequio.Visible = False
            ddlTipoObsequio.ClearSelection()
            lblTipoBono.Visible = False
            ddlTipoBono.Visible = False
            ddlTipoBono.ClearSelection()
            lblPapeleria.Visible = False
            ddlPapeleria.Visible = False
            ddlPapeleria.ClearSelection()

        End If

        If ddlArticulo.SelectedValue = 6 Then
            lblCantidad.Visible = False
            txtCantidad.Visible = False
            lblValorUnitario.Visible = True
            txtValorUnitario.Visible = True
            lblCentroCosto.Visible = True
            ddlCentroCosto.Visible = True
            lblCuentasContables.Visible = True
            ddlCuentasContables.Visible = True
            btnCuentasContables.Visible = True
            lblNumeroPV.Visible = False
            txtNumeroPV.Visible = False
            lblProveedor.Visible = False
            txtProveedor.Visible = False
            btnSearchProveedor.Visible = False
            lblSymphony.Visible = False
            txtSymphony.Visible = False
            txtSymphony.Text = ""
            lblIdFisico.Visible = False
            txtIdFisico.Visible = False
            txtIdFisico.Text = ""
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = False
            ddlTipoComputador.Visible = False
            ddlTipoComputador.ClearSelection()
            lblPertenecePC.Visible = False
            ddlPertenecePC.Visible = False
            ddlPertenecePC.ClearSelection()
            lblTipoPeriferico.Visible = False
            ddlTipoPeriferico.Visible = False
            ddlTipoPeriferico.ClearSelection()
            lblMarca.Visible = False
            txtMarca.Visible = False
            txtMarca.Text = ""
            lblModelo.Visible = False
            txtModelo.Visible = False
            txtModelo.Text = ""
            lblProcesador.Visible = False
            txtProcesador.Visible = False
            txtProcesador.Text = ""
            lblMemoria.Visible = False
            txtMemoria.Visible = False
            txtMemoria.Text = ""
            lblAlmacenamiento.Visible = False
            txtAlmacenamiento.Visible = False
            txtAlmacenamiento.Text = ""
            lblSistemaOperativo.Visible = False
            txtSistemaOperativo.Visible = False
            txtSistemaOperativo.Text = ""
            lblSerial.Visible = False
            txtSerial.Visible = False
            txtSerial.Text = ""
            lblNombreEquipo.Visible = False
            txtNombreEquipo.Visible = False
            txtNombreEquipo.Text = ""
            lblOffice.Visible = False
            txtOffice.Visible = False
            txtOffice.Text = ""
            lblProgramas.Visible = False
            txtProgramas.Visible = False
            txtProgramas.Text = ""
            lblTipoServidor.Visible = False
            txtTipoServidor.Visible = False
            txtTipoServidor.Text = ""
            lblRaid.Visible = False
            txtRaid.Visible = False
            txtRaid.Text = ""
            lblIdTablet.Visible = False
            txtIdTablet.Visible = False
            txtIdTablet.Text = ""
            lblIdSTG.Visible = False
            txtIdSTG.Visible = False
            txtIdSTG.Text = ""
            lblTamanoPantalla.Visible = False
            txtTamanoPantalla.Visible = False
            txtTamanoPantalla.Text = ""
            lblChip.Visible = False
            txtChip.Visible = False
            txtChip.Text = ""
            lblIMEI.Visible = False
            txtIMEI.Visible = False
            txtIMEI.Text = ""
            lblPertenece.Visible = False
            ddlPertenece.Visible = False
            ddlPertenece.ClearSelection()
            lblOperador.Visible = True
            ddlOperador.Visible = True
            lblNumCelular.Visible = True
            txtNumCelular.Visible = True
            lblMinutos.Visible = True
            txtMinutos.Visible = True
            lblTipoProducto.Visible = False
            ddlTipoProducto.Visible = False
            ddlTipoProducto.ClearSelection()
            lblProducto.Visible = False
            txtProducto.Visible = False
            txtProducto.Text = ""
            lblTipoObsequio.Visible = False
            ddlTipoObsequio.Visible = False
            ddlTipoObsequio.ClearSelection()
            lblTipoBono.Visible = False
            ddlTipoBono.Visible = False
            ddlTipoBono.ClearSelection()
            lblPapeleria.Visible = False
            ddlPapeleria.Visible = False
            ddlPapeleria.ClearSelection()

        End If

        If ddlArticulo.SelectedValue = 7 Then
            lblCantidad.Visible = True
            txtCantidad.Visible = True
            lblValorUnitario.Visible = True
            txtValorUnitario.Visible = True
            lblCentroCosto.Visible = True
            ddlCentroCosto.Visible = True
            lblCuentasContables.Visible = True
            ddlCuentasContables.Visible = True
            btnCuentasContables.Visible = True
            lblNumeroPV.Visible = False
            txtNumeroPV.Visible = False
            lblProveedor.Visible = False
            txtProveedor.Visible = False
            btnSearchProveedor.Visible = False
            lblSymphony.Visible = False
            txtSymphony.Visible = False
            txtSymphony.Text = ""
            lblIdFisico.Visible = False
            txtIdFisico.Visible = False
            txtIdFisico.Text = ""
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = False
            ddlTipoComputador.Visible = False
            ddlTipoComputador.ClearSelection()
            lblPertenecePC.Visible = False
            ddlPertenecePC.Visible = False
            ddlPertenecePC.ClearSelection()
            lblTipoPeriferico.Visible = False
            ddlTipoPeriferico.Visible = False
            ddlTipoPeriferico.ClearSelection()
            lblMarca.Visible = False
            txtMarca.Visible = False
            txtMarca.Text = ""
            lblModelo.Visible = False
            txtModelo.Visible = False
            txtModelo.Text = ""
            lblProcesador.Visible = False
            txtProcesador.Visible = False
            txtProcesador.Text = ""
            lblMemoria.Visible = False
            txtMemoria.Visible = False
            txtMemoria.Text = ""
            lblAlmacenamiento.Visible = False
            txtAlmacenamiento.Visible = False
            txtAlmacenamiento.Text = ""
            lblSistemaOperativo.Visible = False
            txtSistemaOperativo.Visible = False
            txtSistemaOperativo.Text = ""
            lblSerial.Visible = False
            txtSerial.Visible = False
            txtSerial.Text = ""
            lblNombreEquipo.Visible = False
            txtNombreEquipo.Visible = False
            txtNombreEquipo.Text = ""
            lblOffice.Visible = False
            txtOffice.Visible = False
            txtOffice.Text = ""
            lblProgramas.Visible = False
            txtProgramas.Visible = False
            txtProgramas.Text = ""
            lblTipoServidor.Visible = False
            txtTipoServidor.Visible = False
            txtTipoServidor.Text = ""
            lblRaid.Visible = False
            txtRaid.Visible = False
            txtRaid.Text = ""
            lblIdTablet.Visible = False
            txtIdTablet.Visible = False
            txtIdTablet.Text = ""
            lblIdSTG.Visible = False
            txtIdSTG.Visible = False
            txtIdSTG.Text = ""
            lblTamanoPantalla.Visible = False
            txtTamanoPantalla.Visible = False
            txtTamanoPantalla.Text = ""
            lblChip.Visible = False
            txtChip.Visible = False
            txtChip.Text = ""
            lblIMEI.Visible = False
            txtIMEI.Visible = False
            txtIMEI.Text = ""
            lblPertenece.Visible = False
            ddlPertenece.Visible = False
            ddlPertenece.ClearSelection()
            lblOperador.Visible = False
            ddlOperador.Visible = False
            ddlOperador.ClearSelection()
            lblNumCelular.Visible = False
            txtNumCelular.Visible = False
            txtNumCelular.Text = ""
            lblMinutos.Visible = False
            txtMinutos.Visible = False
            txtMinutos.Text = ""
            lblTipoProducto.Visible = True
            ddlTipoProducto.Visible = True
            lblProducto.Visible = True
            txtProducto.Visible = True
            lblTipoObsequio.Visible = True
            ddlTipoObsequio.Visible = True
            lblTipoBono.Visible = False
            ddlTipoBono.Visible = False
            ddlTipoBono.ClearSelection()
            lblPapeleria.Visible = False
            ddlPapeleria.Visible = False
            ddlPapeleria.ClearSelection()

        End If

        If ddlArticulo.SelectedValue = 8 Then
            lblCantidad.Visible = False
            txtCantidad.Visible = False
            lblValorUnitario.Visible = False
            txtValorUnitario.Visible = False
            txtValorUnitario.Text = ""
            lblCentroCosto.Visible = True
            ddlCentroCosto.Visible = True
            lblCuentasContables.Visible = True
            ddlCuentasContables.Visible = True
            btnCuentasContables.Visible = True
            lblNumeroPV.Visible = False
            txtNumeroPV.Visible = False
            lblProveedor.Visible = False
            txtProveedor.Visible = False
            btnSearchProveedor.Visible = False
            lblSymphony.Visible = False
            txtSymphony.Visible = False
            txtSymphony.Text = ""
            lblIdFisico.Visible = False
            txtIdFisico.Visible = False
            txtIdFisico.Text = ""
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = False
            ddlTipoComputador.Visible = False
            ddlTipoComputador.ClearSelection()
            lblPertenecePC.Visible = False
            ddlPertenecePC.Visible = False
            ddlPertenecePC.ClearSelection()
            lblTipoPeriferico.Visible = False
            ddlTipoPeriferico.Visible = False
            ddlTipoPeriferico.ClearSelection()
            lblMarca.Visible = False
            txtMarca.Visible = False
            txtMarca.Text = ""
            lblModelo.Visible = False
            txtModelo.Visible = False
            txtModelo.Text = ""
            lblProcesador.Visible = False
            txtProcesador.Visible = False
            txtProcesador.Text = ""
            lblMemoria.Visible = False
            txtMemoria.Visible = False
            txtMemoria.Text = ""
            lblAlmacenamiento.Visible = False
            txtAlmacenamiento.Visible = False
            txtAlmacenamiento.Text = ""
            lblSistemaOperativo.Visible = False
            txtSistemaOperativo.Visible = False
            txtSistemaOperativo.Text = ""
            lblSerial.Visible = False
            txtSerial.Visible = False
            txtSerial.Text = ""
            lblNombreEquipo.Visible = False
            txtNombreEquipo.Visible = False
            txtNombreEquipo.Text = ""
            lblOffice.Visible = False
            txtOffice.Visible = False
            txtOffice.Text = ""
            lblProgramas.Visible = False
            txtProgramas.Visible = False
            txtProgramas.Text = ""
            lblTipoServidor.Visible = False
            txtTipoServidor.Visible = False
            txtTipoServidor.Text = ""
            lblRaid.Visible = False
            txtRaid.Visible = False
            txtRaid.Text = ""
            lblIdTablet.Visible = False
            txtIdTablet.Visible = False
            txtIdTablet.Text = ""
            lblIdSTG.Visible = False
            txtIdSTG.Visible = False
            txtIdSTG.Text = ""
            lblTamanoPantalla.Visible = False
            txtTamanoPantalla.Visible = False
            txtTamanoPantalla.Text = ""
            lblChip.Visible = False
            txtChip.Visible = False
            txtChip.Text = ""
            lblIMEI.Visible = False
            txtIMEI.Visible = False
            txtIMEI.Text = ""
            lblPertenece.Visible = False
            ddlPertenece.Visible = False
            ddlPertenece.ClearSelection()
            lblOperador.Visible = False
            ddlOperador.Visible = False
            ddlOperador.ClearSelection()
            lblNumCelular.Visible = False
            txtNumCelular.Visible = False
            txtNumCelular.Text = ""
            lblMinutos.Visible = False
            txtMinutos.Visible = False
            txtMinutos.Text = ""
            lblTipoProducto.Visible = False
            ddlTipoProducto.Visible = False
            ddlTipoProducto.ClearSelection()
            lblProducto.Visible = False
            txtProducto.Visible = False
            txtProducto.Text = ""
            lblTipoObsequio.Visible = False
            ddlTipoObsequio.Visible = False
            ddlTipoObsequio.ClearSelection()
            lblTipoBono.Visible = True
            ddlTipoBono.Visible = True
            lblPapeleria.Visible = False
            ddlPapeleria.Visible = False
            ddlPapeleria.ClearSelection()

        End If

        If ddlArticulo.SelectedValue = 9 Then
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
            lblCantidad.Visible = False
            txtCantidad.Visible = False
            lblValorUnitario.Visible = False
            txtValorUnitario.Visible = False
            txtValorUnitario.Text = ""
            lblNumeroPV.Visible = False
            txtNumeroPV.Visible = False
            lblProveedor.Visible = False
            txtProveedor.Visible = False
            btnSearchProveedor.Visible = False
            lblSymphony.Visible = False
            txtSymphony.Visible = False
            txtSymphony.Text = ""
            lblIdFisico.Visible = False
            txtIdFisico.Visible = False
            txtIdFisico.Text = ""
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = False
            ddlTipoComputador.Visible = False
            ddlTipoComputador.ClearSelection()
            lblPertenecePC.Visible = False
            ddlPertenecePC.Visible = False
            ddlPertenecePC.ClearSelection()
            lblTipoPeriferico.Visible = False
            ddlTipoPeriferico.Visible = False
            ddlTipoPeriferico.ClearSelection()
            lblMarca.Visible = False
            txtMarca.Visible = False
            txtMarca.Text = ""
            lblModelo.Visible = False
            txtModelo.Visible = False
            txtModelo.Text = ""
            lblProcesador.Visible = False
            txtProcesador.Visible = False
            txtProcesador.Text = ""
            lblMemoria.Visible = False
            txtMemoria.Visible = False
            txtMemoria.Text = ""
            lblAlmacenamiento.Visible = False
            txtAlmacenamiento.Visible = False
            txtAlmacenamiento.Text = ""
            lblSistemaOperativo.Visible = False
            txtSistemaOperativo.Visible = False
            txtSistemaOperativo.Text = ""
            lblSerial.Visible = False
            txtSerial.Visible = False
            txtSerial.Text = ""
            lblNombreEquipo.Visible = False
            txtNombreEquipo.Visible = False
            txtNombreEquipo.Text = ""
            lblOffice.Visible = False
            txtOffice.Visible = False
            txtOffice.Text = ""
            lblProgramas.Visible = False
            txtProgramas.Visible = False
            txtProgramas.Text = ""
            lblTipoServidor.Visible = False
            txtTipoServidor.Visible = False
            txtTipoServidor.Text = ""
            lblRaid.Visible = False
            txtRaid.Visible = False
            txtRaid.Text = ""
            lblIdTablet.Visible = False
            txtIdTablet.Visible = False
            txtIdTablet.Text = ""
            lblIdSTG.Visible = False
            txtIdSTG.Visible = False
            txtIdSTG.Text = ""
            lblTamanoPantalla.Visible = False
            txtTamanoPantalla.Visible = False
            txtTamanoPantalla.Text = ""
            lblChip.Visible = False
            txtChip.Visible = False
            txtChip.Text = ""
            lblIMEI.Visible = False
            txtIMEI.Visible = False
            txtIMEI.Text = ""
            lblPertenece.Visible = False
            ddlPertenece.Visible = False
            ddlPertenece.ClearSelection()
            lblOperador.Visible = False
            ddlOperador.Visible = False
            ddlOperador.ClearSelection()
            lblNumCelular.Visible = False
            txtNumCelular.Visible = False
            txtNumCelular.Text = ""
            lblMinutos.Visible = False
            txtMinutos.Visible = False
            txtMinutos.Text = ""
            lblTipoProducto.Visible = False
            ddlTipoProducto.Visible = False
            ddlTipoProducto.ClearSelection()
            lblProducto.Visible = False
            txtProducto.Visible = False
            txtProducto.Text = ""
            lblTipoObsequio.Visible = False
            ddlTipoObsequio.Visible = False
            ddlTipoObsequio.ClearSelection()
            lblTipoBono.Visible = False
            ddlTipoBono.Visible = False
            ddlTipoBono.ClearSelection()
            lblPapeleria.Visible = False
            ddlPapeleria.Visible = False
            ddlPapeleria.ClearSelection()

        End If

        If ddlArticulo.SelectedValue = 10 Then
            lblCantidad.Visible = False
            txtCantidad.Visible = False
            lblValorUnitario.Visible = True
            txtValorUnitario.Visible = True
            lblNumeroPV.Visible = True
            txtNumeroPV.Visible = True
            lblProveedor.Visible = True
            txtProveedor.Visible = True
            btnSearchProveedor.Visible = True
            lblSymphony.Visible = True
            txtSymphony.Visible = True
            lblIdFisico.Visible = True
            txtIdFisico.Visible = True
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = False
            ddlTipoComputador.Visible = False
            ddlTipoComputador.ClearSelection()
            lblPertenecePC.Visible = False
            ddlPertenecePC.Visible = False
            ddlPertenecePC.ClearSelection()
            lblTipoPeriferico.Visible = False
            ddlTipoPeriferico.Visible = False
            lblMarca.Visible = True
            txtMarca.Visible = True
            lblModelo.Visible = True
            txtModelo.Visible = True
            lblProcesador.Visible = False
            txtProcesador.Visible = False
            txtProcesador.Text = ""
            lblMemoria.Visible = False
            txtMemoria.Visible = False
            txtMemoria.Text = ""
            lblAlmacenamiento.Visible = False
            txtAlmacenamiento.Visible = False
            txtAlmacenamiento.Text = ""
            lblSistemaOperativo.Visible = False
            txtSistemaOperativo.Visible = False
            txtSistemaOperativo.Text = ""
            lblSerial.Visible = True
            txtSerial.Visible = True
            lblNombreEquipo.Visible = False
            txtNombreEquipo.Visible = False
            txtNombreEquipo.Text = ""
            lblOffice.Visible = False
            txtOffice.Visible = False
            txtOffice.Text = ""
            lblProgramas.Visible = False
            txtProgramas.Visible = False
            txtProgramas.Text = ""
            lblTipoServidor.Visible = False
            txtTipoServidor.Visible = False
            txtTipoServidor.Text = ""
            lblRaid.Visible = False
            txtRaid.Visible = False
            txtRaid.Text = ""
            lblIdTablet.Visible = False
            txtIdTablet.Visible = False
            txtIdTablet.Text = ""
            lblIdSTG.Visible = False
            txtIdSTG.Visible = False
            txtIdSTG.Text = ""
            lblTamanoPantalla.Visible = False
            txtTamanoPantalla.Visible = False
            txtTamanoPantalla.Text = ""
            lblChip.Visible = False
            txtChip.Visible = False
            txtChip.Text = ""
            lblIMEI.Visible = False
            txtIMEI.Visible = False
            txtIMEI.Text = ""
            lblPertenece.Visible = False
            ddlPertenece.Visible = False
            ddlPertenece.ClearSelection()
            lblOperador.Visible = False
            ddlOperador.Visible = False
            ddlOperador.ClearSelection()
            lblNumCelular.Visible = False
            txtNumCelular.Visible = False
            txtNumCelular.Text = ""
            lblMinutos.Visible = False
            txtMinutos.Visible = False
            txtMinutos.Text = ""
            lblTipoProducto.Visible = False
            ddlTipoProducto.Visible = False
            ddlTipoProducto.ClearSelection()
            lblProducto.Visible = False
            txtProducto.Visible = False
            txtProducto.Text = ""
            lblTipoObsequio.Visible = False
            ddlTipoObsequio.Visible = False
            ddlTipoObsequio.ClearSelection()
            lblTipoBono.Visible = False
            ddlTipoBono.Visible = False
            ddlTipoBono.ClearSelection()
            lblPapeleria.Visible = False
            ddlPapeleria.Visible = False
            ddlPapeleria.ClearSelection()

        End If

        If ddlArticulo.SelectedValue = 11 Then
            lblCantidad.Visible = True
            txtCantidad.Visible = True
            lblValorUnitario.Visible = True
            txtValorUnitario.Visible = True
            lblCentroCosto.Visible = True
            ddlCentroCosto.Visible = True
            lblCuentasContables.Visible = True
            ddlCuentasContables.Visible = True
            btnCuentasContables.Visible = True
            lblNumeroPV.Visible = False
            txtNumeroPV.Visible = False
            lblProveedor.Visible = False
            txtProveedor.Visible = False
            btnSearchProveedor.Visible = False
            lblSymphony.Visible = False
            txtSymphony.Visible = False
            txtSymphony.Text = ""
            lblIdFisico.Visible = False
            txtIdFisico.Visible = False
            txtIdFisico.Text = ""
            lblSede.Visible = True
            ddlSede.Visible = True
            lblTipoComputador.Visible = False
            ddlTipoComputador.Visible = False
            ddlTipoComputador.ClearSelection()
            lblPertenecePC.Visible = False
            ddlPertenecePC.Visible = False
            ddlPertenecePC.ClearSelection()
            lblTipoPeriferico.Visible = False
            ddlTipoPeriferico.Visible = False
            ddlTipoPeriferico.ClearSelection()
            lblMarca.Visible = False
            txtMarca.Visible = False
            txtMarca.Text = ""
            lblModelo.Visible = False
            txtModelo.Visible = False
            txtModelo.Text = ""
            lblProcesador.Visible = False
            txtProcesador.Visible = False
            txtProcesador.Text = ""
            lblMemoria.Visible = False
            txtMemoria.Visible = False
            txtMemoria.Text = ""
            lblAlmacenamiento.Visible = False
            txtAlmacenamiento.Visible = False
            txtAlmacenamiento.Text = ""
            lblSistemaOperativo.Visible = False
            txtSistemaOperativo.Visible = False
            txtSistemaOperativo.Text = ""
            lblSerial.Visible = False
            txtSerial.Visible = False
            txtSerial.Text = ""
            lblNombreEquipo.Visible = False
            txtNombreEquipo.Visible = False
            txtNombreEquipo.Text = ""
            lblOffice.Visible = False
            txtOffice.Visible = False
            txtOffice.Text = ""
            lblProgramas.Visible = False
            txtProgramas.Visible = False
            txtProgramas.Text = ""
            lblTipoServidor.Visible = False
            txtTipoServidor.Visible = False
            txtTipoServidor.Text = ""
            lblRaid.Visible = False
            txtRaid.Visible = False
            txtRaid.Text = ""
            lblIdTablet.Visible = False
            txtIdTablet.Visible = False
            txtIdTablet.Text = ""
            lblIdSTG.Visible = False
            txtIdSTG.Visible = False
            txtIdSTG.Text = ""
            lblTamanoPantalla.Visible = False
            txtTamanoPantalla.Visible = False
            txtTamanoPantalla.Text = ""
            lblChip.Visible = False
            txtChip.Visible = False
            txtChip.Text = ""
            lblIMEI.Visible = False
            txtIMEI.Visible = False
            txtIMEI.Text = ""
            lblPertenece.Visible = False
            ddlPertenece.Visible = False
            ddlPertenece.ClearSelection()
            lblOperador.Visible = False
            ddlOperador.Visible = False
            ddlOperador.ClearSelection()
            lblNumCelular.Visible = False
            txtNumCelular.Visible = False
            txtNumCelular.Text = ""
            lblMinutos.Visible = False
            txtMinutos.Visible = False
            txtMinutos.Text = ""
            lblTipoProducto.Visible = False
            ddlTipoProducto.Visible = False
            lblProducto.Visible = False
            txtProducto.Visible = False
            txtProducto.Text = ""
            lblTipoObsequio.Visible = False
            ddlTipoObsequio.Visible = False
            lblTipoBono.Visible = False
            ddlTipoBono.Visible = False
            ddlTipoBono.ClearSelection()
            lblPapeleria.Visible = True
            ddlPapeleria.Visible = True

        End If

    End Sub

    Sub limpiar()
        Me.ddlTipoArticulo.SelectedValue = "-1"
        Me.ddlArticulo.SelectedValue = "-1"
        Me.txtFechaCompra.Text = ""
        Me.ddlCentroCosto.SelectedValue = "-1"
        Me.ddlBU.SelectedValue = "-1"
        hfIdTrabajo.Value = "0"
        Me.txtJBIJBE.Text = ""
        Me.txtNombreJBIJBE.Text = ""
        ddlCuentasContables.ClearSelection()
        Me.txtNumeroCuenta.Text = ""
        Me.txtDescripcionCC.Text = ""
        Me.txtValorUnitario.Text = ""
        Me.txtCantidad.Text = ""
        Me.ddlEstado.SelectedValue = "-1"
        Me.txtDescripcion.Text = ""
        Me.txtNumeroPV.Text = ""
        Me.txtProveedor.Text = ""
        Me.txtNitProveedor.Text = ""
        Me.txtNombreProveedor.Text = ""
        Me.txtSymphony.Text = ""
        Me.txtIdFisico.Text = ""
        Me.ddlSede.SelectedValue = "-1"
        Me.ddlTipoComputador.SelectedValue = "-1"
        Me.ddlPertenecePC.SelectedValue = "-1"
        Me.txtFechaFin.Text = ""
        Me.ddlTipoPeriferico.SelectedValue = "-1"
        Me.txtMarca.Text = ""
        Me.txtModelo.Text = ""
        Me.txtProcesador.Text = ""
        Me.txtMemoria.Text = ""
        Me.txtAlmacenamiento.Text = ""
        Me.txtSistemaOperativo.Text = ""
        Me.txtSerial.Text = ""
        Me.txtNombreEquipo.Text = ""
        Me.txtOffice.Text = ""
        Me.txtProgramas.Text = ""
        Me.txtTipoServidor.Text = ""
        Me.txtRaid.Text = ""
        Me.txtIdTablet.Text = ""
        Me.txtIdSTG.Text = ""
        Me.txtTamanoPantalla.Text = ""
        Me.txtChip.Text = ""
        Me.txtIMEI.Text = ""
        Me.ddlPertenece.SelectedValue = "-1"
        Me.ddlOperador.SelectedValue = "-1"
        Me.txtNumCelular.Text = ""
        Me.txtMinutos.Text = ""
        Me.ddlTipoProducto.SelectedValue = "-1"
        Me.txtProducto.Text = ""
        Me.ddlTipoObsequio.ClearSelection()
        Me.ddlTipoBono.SelectedValue = "-1"
        Me.ddlPapeleria.ClearSelection()


        lblCentroCosto.Visible = False
        ddlCentroCosto.Visible = False
        lblBU.Visible = False
        ddlBU.Visible = False
        lblJBIJBE.Visible = False
        txtJBIJBE.Visible = False
        lblNombreJBIJBE.Visible = False
        txtNombreJBIJBE.Visible = False
        lblCuentasContables.Visible = False
        ddlCuentasContables.Visible = False
        btnCuentasContables.Visible = False
        lblValorUnitario.Visible = False
        txtValorUnitario.Visible = False
        lblCantidad.Visible = False
        txtCantidad.Visible = False
        lblNumeroPV.Visible = False
        txtNumeroPV.Visible = False
        lblProveedor.Visible = False
        txtProveedor.Visible = False
        btnSearchProveedor.Visible = False
        lblSymphony.Visible = False
        txtSymphony.Visible = False
        lblIdFisico.Visible = False
        txtIdFisico.Visible = False
        lblTipoComputador.Visible = False
        ddlTipoComputador.Visible = False
        lblPertenecePC.Visible = False
        ddlPertenecePC.Visible = False
        lblTipoPeriferico.Visible = False
        ddlTipoPeriferico.Visible = False
        lblMarca.Visible = False
        txtMarca.Visible = False
        lblModelo.Visible = False
        txtModelo.Visible = False
        lblProcesador.Visible = False
        txtProcesador.Visible = False
        lblMemoria.Visible = False
        txtMemoria.Visible = False
        lblAlmacenamiento.Visible = False
        txtAlmacenamiento.Visible = False
        lblSistemaOperativo.Visible = False
        txtSistemaOperativo.Visible = False
        lblSerial.Visible = False
        txtSerial.Visible = False
        lblNombreEquipo.Visible = False
        txtNombreEquipo.Visible = False
        lblOffice.Visible = False
        txtOffice.Visible = False
        lblProgramas.Visible = False
        txtProgramas.Visible = False
        lblTipoServidor.Visible = False
        txtTipoServidor.Visible = False
        lblRaid.Visible = False
        txtRaid.Visible = False
        lblIdTablet.Visible = False
        txtIdTablet.Visible = False
        lblIdSTG.Visible = False
        txtIdSTG.Visible = False
        lblTamanoPantalla.Visible = False
        txtTamanoPantalla.Visible = False
        lblChip.Visible = False
        txtChip.Visible = False
        lblIMEI.Visible = False
        txtIMEI.Visible = False
        lblPertenece.Visible = False
        ddlPertenece.Visible = False
        lblOperador.Visible = False
        ddlOperador.Visible = False
        lblNumCelular.Visible = False
        txtNumCelular.Visible = False
        lblMinutos.Visible = False
        txtMinutos.Visible = False
        lblTipoProducto.Visible = False
        ddlTipoProducto.Visible = False
        lblProducto.Visible = False
        txtProducto.Visible = False
        lblTipoObsequio.Visible = False
        ddlTipoObsequio.Visible = False
        lblTipoBono.Visible = False
        ddlTipoBono.Visible = False
        lblPapeleria.Visible = False
        ddlPapeleria.Visible = False

        lblIdActualizar.Text = ""
        lblIdActualizar.Visible = False
        lblActualizar.Visible = False
    End Sub

#End Region

    Protected Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click

        If Not (IsDate(txtFechaCompra.Text)) Then
            ShowNotification("Escriba la fecha de la compra", ShowNotifications.ErrorNotification)
            txtFechaCompra.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCentroCosto.Visible = True And ddlCentroCosto.SelectedValue = "-1" Then
            ShowNotification("Seleccione el Centro de Costo y luego el JB o la BU", ShowNotifications.ErrorNotification)
            ddlCentroCosto.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCentroCosto.SelectedValue = 3 AndAlso ddlBU.SelectedValue = "-1" Then
            ShowNotification("Seleccione la Unidad de Negocio", ShowNotifications.ErrorNotification)
            ddlBU.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If (ddlCentroCosto.SelectedValue = 1 Or ddlCentroCosto.SelectedValue = 2) AndAlso String.IsNullOrEmpty(txtJBIJBE.Text) Then
            ShowNotification("Debe indicar un codigo de JobBook", ShowNotifications.ErrorNotification)
            txtJBIJBE.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If (ddlCentroCosto.SelectedValue = 1 Or ddlCentroCosto.SelectedValue = 2) AndAlso String.IsNullOrEmpty(txtNombreJBIJBE.Text) Then
            ShowNotification("Debe indicar un nombre de JobBook", ShowNotifications.ErrorNotification)
            txtNombreJBIJBE.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        If ddlCuentasContables.Visible = True And ddlCuentasContables.SelectedValue = -1 Then
            ShowNotification("Debe seleccionar una cuenta contable", ShowNotifications.ErrorNotification)
            ddlCuentasContables.Focus()
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
            Exit Sub
        End If

        Dim oGuardar As New Inventario

        Dim otxtUsuario As New TextBox With {.Text = Session("IDUsuario").ToString}

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

        Dim ValorUnitario As Int64? = Nothing
        If Not txtValorUnitario.Text = "" Then ValorUnitario = txtValorUnitario.Text

        Dim Cantidad As Int64? = Nothing
        If Not txtCantidad.Text = "" Then Cantidad = txtCantidad.Text

        Dim Estado As Int64?
        If ddlTipoArticulo.SelectedValue = 1 Then
            Estado = If(ddlEstado.SelectedValue = -1, CType(Nothing, Int64?), CType(ddlEstado.SelectedValue, Int64?))
        Else
            Estado = 1
        End If

        Dim Descripcion As String
        Descripcion = If(txtDescripcion.Text = "", CType(Nothing, String), CType(txtDescripcion.Text, String))

        Dim NumeroPV As Int64? = Nothing
        NumeroPV = If(txtNumeroPV.Text = "", CType(Nothing, Int64?), CType(txtNumeroPV.Text, Int64?))

        Dim ProveedorId As Int64? = Nothing
        ProveedorId = If(hfProveedor.Value = 0, CType(Nothing, Int64?), CType(hfProveedor.Value, Int64?))

        Dim Symphony As String
        Symphony = If(txtSymphony.Text = "", CType(Nothing, String), CType(txtSymphony.Text, String))

        Dim IdFisico As Int64?
        IdFisico = If(txtIdFisico.Text = "", CType(Nothing, Int64?), CType(txtIdFisico.Text, Int64?))

        Dim Sede As Int64?
        Sede = If(ddlSede.SelectedValue = -1, CType(Nothing, Int64?), CType(ddlSede.SelectedValue, Int64?))

        Dim TipoComputador As Int64?
        TipoComputador = If(ddlTipoComputador.SelectedValue = -1, CType(Nothing, Int64?), CType(ddlTipoComputador.SelectedValue, Int64?))

        Dim PertenecePC As Int16?
        PertenecePC = If(ddlPertenecePC.SelectedValue = -1, CType(Nothing, Int16?), CType(ddlPertenecePC.SelectedValue, Int16?))

        Dim TipoPeriferico As Int64?
        TipoPeriferico = If(ddlTipoPeriferico.SelectedValue = -1, CType(Nothing, Int64?), CType(ddlTipoPeriferico.SelectedValue, Int64?))

        Dim Marca As String
        Marca = If(txtMarca.Text = "", CType(Nothing, String), CType(txtMarca.Text, String))

        Dim Modelo As String
        Modelo = If(txtModelo.Text = "", CType(Nothing, String), CType(txtModelo.Text, String))

        Dim Procesador As String
        Procesador = If(txtProcesador.Text = "", CType(Nothing, String), CType(txtProcesador.Text, String))

        Dim Memoria As String
        Memoria = If(txtMemoria.Text = "", CType(Nothing, String), CType(txtMemoria.Text, String))

        Dim Almacenamiento As String
        Almacenamiento = If(txtAlmacenamiento.Text = "", CType(Nothing, String), CType(txtAlmacenamiento.Text, String))

        Dim SistemaOperativo As String
        SistemaOperativo = If(txtSistemaOperativo.Text = "", CType(Nothing, String), CType(txtSistemaOperativo.Text, String))

        Dim Serial As String
        Serial = If(txtSerial.Text = "", CType(Nothing, String), CType(txtSerial.Text, String))

        Dim NombreEquipo As String
        NombreEquipo = If(txtNombreEquipo.Text = "", CType(Nothing, String), CType(txtNombreEquipo.Text, String))

        Dim Office As String
        Office = If(txtOffice.Text = "", CType(Nothing, String), CType(txtOffice.Text, String))

        Dim Programas As String
        Programas = If(txtProgramas.Text = "", CType(Nothing, String), CType(txtProgramas.Text, String))

        Dim TipoServidor As String
        TipoServidor = If(txtTipoServidor.Text = "", CType(Nothing, String), CType(txtTipoServidor.Text, String))

        Dim Raid As String
        Raid = If(txtRaid.Text = "", CType(Nothing, String), CType(txtRaid.Text, String))

        Dim IdTablet As Int64?
        IdTablet = If(txtIdTablet.Text = "", CType(Nothing, Int64?), CType(txtIdTablet.Text, Int64?))

        Dim IdSTG As Int64?
        IdSTG = If(txtIdSTG.Text = "", CType(Nothing, Int64?), CType(txtIdSTG.Text, Int64?))

        Dim TamanoPantalla As String
        TamanoPantalla = If(txtTamanoPantalla.Text = "", CType(Nothing, String), CType(txtTamanoPantalla.Text, String))

        Dim Chip As Int64?
        Chip = If(txtChip.Text = "", CType(Nothing, Int64?), CType(txtChip.Text, Int64?))

        Dim IMEI As Int64?
        IMEI = If(txtIMEI.Text = "", CType(Nothing, Int64?), CType(txtIMEI.Text, Int64?))

        Dim Pertenece As Int64?
        Pertenece = If(ddlPertenece.SelectedValue = -1, CType(Nothing, Int64?), CType(ddlPertenece.SelectedValue, Int64?))

        Dim Operador As Int64?
        Operador = If(ddlOperador.SelectedValue = -1, CType(Nothing, Int64?), CType(ddlOperador.SelectedValue, Int64?))

        Dim NumeroCelular As Int64?
        NumeroCelular = If(txtNumCelular.Text = "", CType(Nothing, Int64?), CType(txtNumCelular.Text, Int64?))

        Dim CantidadMinutos As Integer?
        CantidadMinutos = If(txtMinutos.Text = "", CType(Nothing, Integer?), CType(txtMinutos.Text, Integer?))

        Dim TipoProducto As Int64?
        TipoProducto = If(ddlTipoProducto.SelectedValue = -1, CType(Nothing, Int64?), CType(ddlTipoProducto.SelectedValue, Int64?))

        Dim Producto As String
        Producto = If(txtProducto.Text = "", CType(Nothing, String), CType(txtProducto.Text, String))

        Dim TipoObsequio As Int16?
        TipoObsequio = If(ddlTipoObsequio.SelectedValue = -1, CType(Nothing, Int16?), CType(ddlTipoObsequio.SelectedValue, Int16?))

        Dim TipoBono As Int64?
        TipoBono = If(ddlTipoBono.SelectedValue = -1, CType(Nothing, Int64?), CType(ddlTipoBono.SelectedValue, Int64?))

        Dim ProductoPapeleria As Int64?
        ProductoPapeleria = If(ddlPapeleria.SelectedValue = -1, CType(Nothing, Int64?), CType(ddlPapeleria.SelectedValue, Int64?))

        Dim FechaFinRenta As DateTime?
        FechaFinRenta = If(txtFechaFin.Text = "", CType(Nothing, DateTime?), CType(txtFechaFin.Text, DateTime?))

        If String.IsNullOrEmpty(lblIdActualizar.Text) Then
            Dim Id As Decimal = oGuardar.GuardarRegistroArticulos(ddlTipoArticulo.SelectedValue, ddlArticulo.SelectedValue, txtFechaCompra.Text, otxtUsuario.Text, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, CuentaContable, ValorUnitario, 1, Descripcion, Symphony, IdFisico, Sede, TipoComputador, PertenecePC, TipoPeriferico, Marca, Modelo, Procesador, Memoria, Almacenamiento, SistemaOperativo, Serial, NombreEquipo, Office, Programas, TipoServidor, Raid, IdTablet, IdSTG, TamanoPantalla, Chip, IMEI, Pertenece, Operador, NumeroCelular, CantidadMinutos, TipoProducto, Producto, TipoObsequio, TipoBono, False, FechaFinRenta, NumeroPV, ProveedorId, Cantidad, ProductoPapeleria)

            If ddlArticulo.SelectedValue = 7 Then
                Dim UsuarioRegistra As Int64 = CType(Session("IDUsuario").ToString, Int64?)

                oGuardar.GuardarStockConsumibles(Id, Nothing, txtFechaCompra.Text, UsuarioRegistra, 1, Nothing, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, CuentaContable, Sede, Cantidad, Cantidad, Cantidad, UsuarioRegistra, 1, "Entrada de Producto")
            End If
        Else
            oGuardar.ActualizarRegistroArticulos(lblIdActualizar.Text, ddlTipoArticulo.SelectedValue, ddlArticulo.SelectedValue, txtFechaCompra.Text, otxtUsuario.Text, CentroCosto, BU, IdTrabajo, JobBookCodigo, JobBookNombre, CuentaContable, ValorUnitario, Estado, Descripcion, Symphony, IdFisico, Sede, TipoComputador, PertenecePC, TipoPeriferico, Marca, Modelo, Procesador, Memoria, Almacenamiento, SistemaOperativo, Serial, NombreEquipo, Office, Programas, TipoServidor, Raid, IdTablet, IdSTG, TamanoPantalla, Chip, IMEI, Pertenece, Operador, NumeroCelular, CantidadMinutos, TipoProducto, Producto, TipoObsequio, TipoBono, Nothing, FechaFinRenta, NumeroPV, ProveedorId, Cantidad, ProductoPapeleria)
        End If

        ShowNotification("El Artículo ha sido guardado correctamente", ShowNotifications.InfoNotification)
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)

        limpiar()

    End Sub

    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        limpiar()
        gvArticulos.DataSource = Nothing
        gvArticulos.DataBind()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnBuscar_Click(sender As Object, e As EventArgs) Handles btnBuscar.Click
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub btnBuscarProveedor_Click(sender As Object, e As EventArgs) Handles btnBuscarProveedor.Click
        Dim o As New FI.Ordenes
        Dim identificacion As Int64? = Nothing
        Dim proveedor As String = Nothing
        If IsNumeric(Me.txtNitProveedor.Text) Then identificacion = Me.txtNitProveedor.Text
        If Not Me.txtNombreProveedor.Text = "" Then proveedor = Me.txtNombreProveedor.Text
        Me.gvProveedores.DataSource = o.ObtenerContratistas(identificacion, proveedor, True)
        Me.gvProveedores.DataBind()
        Me.UPanelProveedores.Update()
    End Sub

    Protected Sub btnCuentasContables_Click(sender As Object, e As EventArgs) Handles btnCuentasContables.Click
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub btnSearchProveedor_Click(sender As Object, e As EventArgs) Handles btnSearchProveedor.Click
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlTipoArticulo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlTipoArticulo.SelectedIndexChanged
        CargarArticulos()

        If ddlTipoArticulo.SelectedValue = 1 Then
            lblCentroCosto.Visible = True
            ddlCentroCosto.Visible = True
            lblCuentasContables.Visible = True
            ddlCuentasContables.Visible = True
            btnCuentasContables.Visible = True
        Else
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

        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlArticulo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlArticulo.SelectedIndexChanged
        CargarFormulario()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlPertenecePC_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPertenecePC.SelectedIndexChanged
        If ddlPertenecePC.SelectedValue = 2 Then
            lblFechaFin.Visible = True
            txtFechaFin.Visible = True
        Else
            lblFechaFin.Visible = False
            txtFechaFin.Visible = False
            txtFechaFin.Text = ""
        End If
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub gvCuentasContables_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvCuentasContables.RowCommand
        If e.CommandName = "Seleccionar" Then
            ddlCuentasContables.SelectedValue = Me.gvCuentasContables.DataKeys(CInt(e.CommandArgument))("id")
            ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        End If
    End Sub

    Protected Sub btnBuscarCuentaContable_Click(sender As Object, e As EventArgs) Handles btnBuscarCuentaContable.Click
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
        gvCuentasContables.DataSource = obtenerCuentasContables(txtDescripcionCC.Text, txtNumeroCuenta.Text)
        gvCuentasContables.DataBind()
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
        cargarCuentasContablesDropDown()
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
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
        ActivateAccordion(0, EffectActivateAccordion.NoEffect)
    End Sub

    Protected Sub ddlIdArticulo_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlIdArticulo.SelectedIndexChanged
        If ddlIdArticulo.SelectedValue = 1 Then
            lblIdTipoComputador.Visible = True
            ddlIdTipoComputador.Visible = True
            lblIdPertenecePC.Visible = True
            ddlIdPertenecePC.Visible = True
        Else
            lblIdTipoComputador.Visible = False
            ddlIdTipoComputador.Visible = False
            ddlIdTipoComputador.ClearSelection()
            lblIdPertenecePC.Visible = False
            ddlIdPertenecePC.Visible = False
            ddlIdPertenecePC.ClearSelection()
        End If

        If ddlIdArticulo.SelectedValue = 3 Then
            lblIdPeriferico.Visible = True
            ddlIdPeriferico.Visible = True
        Else
            lblIdPeriferico.Visible = False
            ddlIdPeriferico.Visible = False
            ddlIdPeriferico.ClearSelection()
        End If

        If ddlIdArticulo.SelectedValue = 7 Then
            lblIdTipoProducto.Visible = True
            ddlIdTipoProducto.Visible = True
        Else
            lblIdTipoProducto.Visible = False
            ddlIdTipoProducto.Visible = False
            ddlIdTipoProducto.ClearSelection()
        End If
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)

    End Sub

    Protected Sub btnExportar_Click(sender As Object, e As EventArgs) Handles btnExportar.Click
        CargarColumnas()
        gvArticulos.DataSource = obtenerRegistrosArticulos()
        gvArticulos.DataBind()
        gvArticulos.Visible = True
        'Actualiza los datos del gridview
        gvArticulos.AllowPaging = False
        gvArticulos.DataBind()
        gvArticulos.Columns(gvArticulos.Columns.Count - 1).Visible = False

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
        Response.AddHeader("Content-Disposition", "attachment;filename=Registro_Articulos.xls")
        Response.Charset = "UTF-8"
        Response.ContentEncoding = Encoding.Default
        Response.Write(sb.ToString())
        Response.End()
        'Incluya esta línea si el gridview está oculto
        gvArticulos.Columns(gvArticulos.Columns.Count - 1).Visible = True
        gvArticulos.Visible = False
    End Sub

    Private Sub _AlmacenamientoDisco_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New CoreProject.Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())
        If permisos.VerificarPermisoUsuario(138, UsuarioID) = False AndAlso permisos.VerificarPermisoUsuario(140, UsuarioID) = False AndAlso permisos.VerificarPermisoUsuario(141, UsuarioID) = False AndAlso permisos.VerificarPermisoUsuario(142, UsuarioID) = False Then
            Response.Redirect("../Home.aspx")
        End If
    End Sub

   
    
End Class