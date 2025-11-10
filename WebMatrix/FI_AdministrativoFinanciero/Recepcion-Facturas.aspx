<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/FI_.master" CodeBehind="Recepcion-Facturas.aspx.vb" Inherits="WebMatrix.FI_Recepcion_Facturas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/Site.css" type="text/css" />
    <link rel="stylesheet" href="../Styles/Formulario.css" type="text/css" />
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ui-tabs-vertical {
            width: 66em;
        }

            .ui-tabs-vertical .ui-tabs-nav {
                padding: .2em .1em .2em .2em;
                float: left;
                width: 12em;
            }

                .ui-tabs-vertical .ui-tabs-nav li {
                    clear: left;
                    width: 100%;
                    border-bottom-width: 1px !important;
                    border-right-width: 0 !important;
                    margin: 0 -1px .2em 0;
                }

                    .ui-tabs-vertical .ui-tabs-nav li a {
                        display: block;
                    }

                    .ui-tabs-vertical .ui-tabs-nav li.ui-tabs-active {
                        padding-bottom: 0;
                        padding-right: .1em;
                        border-right-width: 1px;
                    }

            .ui-tabs-vertical .ui-tabs-panel {
                padding: 1em;
                float: right;
                width: 50em;
            }

        .text-center {
            text-align: center;
        }
    </style>
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">

        function loadPlugins() {


            $.validator.addMethod('selectNone',
                function (value, element) {
                    return this.optional(element) ||
                        (value != -1);
                }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });

            $.validator.addMethod('selectNone2',
                function (value, element) {
                    return this.optional(element) ||
                        ($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() != "");
                }, "*Debe asignar por lo menos un presupuesto");
            $.validator.addClassRules("mySpecificClass2", { selectNone2: true });

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $('#infoBrief').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Brief",
                    width: "800px"
                });

            $('#tabs').tabs().addClass("ui-tabs-vertical ui-helper-clearfix");

            $('#VisualizarDocumento').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Visualizar Órden",
                    width: "1000px"

                });

            $("#<%= txtFacturaFechaInicio.ClientID%>").mask("99/99/9999");
            $("#<%= txtFacturaFechaInicio.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFacturaFechaFin.ClientID%>").mask("99/99/9999");
            $("#<%= txtFacturaFechaFin.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            validationForm();

        }

        $(document).ready(function () {
            loadPlugins();
        });

        function cargarOrden(id) {
            var tipoOrden = $('#CPH_Section_CPH_Section_ddlTipoOrden').val();
            var orden = "";
            if (tipoOrden == 1) { orden = 'SERVICIO'; }
            else if (tipoOrden == 2) { orden = 'COMPRA'; }
            else { orden = 'REQUERIMIENTO'; }

            $('#ifDocumento').attr('src', '/Files/ORDEN' + orden + '-' + id + '.pdf');
            $('#CPH_Section_CPH_Section_pnlAprobaciones').css('visibility', 'visible');
            $('#VisualizarDocumento').css('visibility', 'visible');
            $("#VisualizarDocumento").dialog("option", "width", 1000);
            $("#VisualizarDocumento").dialog("option", "height", 800);
            $('#VisualizarDocumento').dialog("open");
        }

        $(document).ready(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerIni);
            bindPickerIni();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerFin);
            bindPickerFin();

            $('#DevolucionTarea').parent().appendTo("form");

            $('#BusquedaProveedores').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Proveedores disponibles",
                    width: "600px"
                });

            $('#BusquedaProveedores').parent().appendTo("form");

            $('#BusquedaSolicitantes').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Solicitantes",
                    width: "600px"
                });

            $('#BusquedaSolicitantes').parent().appendTo("form");


            $('#LoadFiles').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Cargar Factura",
                    width: "600px"

                });

            $('#LoadFiles').parent().appendTo("form");
        });

        function MostrarProveedores() {
            $('#BusquedaProveedores').dialog("open");
        }

        function CerrarProveedores() {
            $('#BusquedaProveedores').dialog("close");
        }

        function MostrarSolicitantes() {
            $('#BusquedaSolicitantes').dialog("open");
        }

        function CerrarSolicitantes() {
            $('#BusquedaSolicitantes').dialog("close");
        }

        function MostrarLoadFiles() {
            $('#LoadFiles').dialog("open");
        }

        function CerrarLoadFiles() {
            $('#LoadFiles').dialog("close");
        }

        function bindPickerIni() {
            $("input[type=text][id*=txtFecha]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function bindPickerFin() {
            $("input[type=text][id*=txtFechaEntrega]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }
    </script>

    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Section" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upTarea" runat="server">
        <ContentTemplate>
            <div>
                <h2><a>Recepción de </a>Facturas</h2>
                <asp:HiddenField ID="hfIdFactura" runat="server" Value="0" />
                <asp:HiddenField ID="hfIdOrden" runat="server" Value="0" />
                <asp:HiddenField ID="hfTipoOrden" runat="server" Value="0" />
                <asp:HiddenField ID="hfTipoOrdenRad" runat="server" Value="0" />
                <asp:HiddenField ID="hfUsuarioAnula" runat="server" Value="0" />
                <asp:HiddenField ID="hfFacturaEscaneda" runat="server" Value="0" />
            </div>

            <div id="tabs">
                <ul>
                    <li><a href="#tabs-1">Radicar</a></li>
                    <li><a href="#tabs-2">Consultar</a></li>
                </ul>
                <div id="tabs-1" style="overflow-y: auto">
                    <asp:Panel ID="pnlRadicar" runat="server">
                        <div class="block">
                            <fieldset class="validationGroup">
                                <div>
                                    <h3><a>Radicar Facturas</a></h3>
                                    <br />
                                    <label>No. Radicado</label>
                                    <asp:TextBox ID="txtConsecutivo" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                    <label>No. Factura</label>
                                    <asp:TextBox ID="txtNoFactura" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                    <label>Cantidad</label>
                                    <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>
                                    <label>Valor Unitario</label>
                                    <asp:TextBox ID="txtValorUnitario" runat="server"></asp:TextBox>
                                    <label>Subtotal</label>
                                    <asp:TextBox ID="txtSubTotal" runat="server"></asp:TextBox>
                                    <label>Valor Total</label>
                                    <asp:TextBox ID="txtValorTotal" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                    <label>Observaciones</label>
                                    <asp:TextBox ID="txtObservaciones" runat="server"></asp:TextBox>
                                    <label>Tipo de documento</label>
                                    <asp:DropDownList ID="ddlTipoDocRadicado" runat="server" CssClass="mySpecificClass">
                                        <asp:ListItem Text="--Seleccione--" Value="-1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Factura" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Cuenta de cobro" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div>
                                    <h3><a>Agregar Ordenes</a></h3>
                                    <br />
                                    <label>No. Orden</label>
                                    <asp:TextBox ID="txtNoOrden" runat="server"></asp:TextBox>
                                    <label>Tipo Orden</label>
                                    <asp:DropDownList ID="ddlTipoOrden" runat="server">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Orden de Servicio" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Orden de Compra" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Requerimiento de Servicio" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;&nbsp;
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Height="22px" />
                                    &nbsp;
                                <asp:Button ID="btnAgregar" runat="server" Text="Agregar" Visible="false" Height="22px" />
                                </div>
                                <div>
                                    <asp:GridView ID="gvOrdenes" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="Id,IdOrden,TipoOrdenId" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="IdOrden" HeaderText="IdOrden" />
                                            <asp:BoundField DataField="TipoOrden" HeaderText="TipoOrden" />
                                            <asp:BoundField DataField="ValorOrden" HeaderText="Valor" />
                                            <asp:TemplateField HeaderText="Quitar" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="Quitar" ImageUrl="~/Images/delete_16.png" Text="Quitar" ToolTip="Quitar" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:GridView ID="gvOrdenesReq" runat="server" Width="100%" AutoGenerateColumns="False" Visible="false"
                                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                        DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                                        <SelectedRowStyle CssClass="SelectedRow" />
                                        <AlternatingRowStyle CssClass="odd" />
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="Id" HeaderStyle-Width="40px" />
                                            <asp:BoundField DataField="IdOrden" HeaderText="Id Orden" HeaderStyle-Width="70px" />                                            
                                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                            <asp:TemplateField HeaderText="Cantidad" HeaderStyle-Width="70px" SortExpression="Cantidad">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtCantidad" Width="70px" runat="server" Text='<%# Bind("Cantidad") %>' BorderColor="#aaaaaa" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Valor Unitario" HeaderStyle-Width="100px" SortExpression="VrUnitario">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtVrUnitario" runat="server" Width="100px" Text='<%# Bind("VrUnitario") %>' BorderColor="#aaaaaa" BorderStyle="Solid" BorderWidth="1"></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Quitar" HeaderStyle-Width="50px" ShowHeader="False" ItemStyle-CssClass="text-center">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDelete" runat="server" CausesValidation="False" CssClass="text-center" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="Quitar" ImageUrl="~/Images/delete_16.png" Text="Quitar" ToolTip="Quitar" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <asp:Label ID="lblTextTotal" runat="server" Visible="false">Total Órdenes: </asp:Label>
                                    <asp:Label ID="lblTotal" runat="server" Visible="false"></asp:Label>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnAdd" runat="server" Text="Radicar Factura" Height="22px" Visible="false" CssClass="causesValidation" OnClientClick="return confirm('Esta seguro de radicar esta factura ?');" />
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnLimpiar" runat="server" Text="Cancelar" Visible="false" />
                                </div>
                            </fieldset>
                        </div>
                    </asp:Panel>
                </div>
                <div id="tabs-2">
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <div class="actions">
                                    <h3><a>Consultar Facturas</a></h3>
                                    <br />
                                    <div class="form_rigth">
                                        <div>
                                            <label>No. Radicado</label>
                                            <asp:TextBox ID="txtSearchConsecutivo" runat="server"></asp:TextBox>
                                            <label>Tipo de documento</label>
                                            <asp:DropDownList ID="ddlSearchTipoDoc" runat="server">
                                                <asp:ListItem Text="--Seleccione--" Value="-1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Factura" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Cuenta de cobro" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                            <label>Escaneada</label>
                                            <asp:DropDownList ID="ddlSearchEscaneada" runat="server">
                                                <asp:ListItem Text="--Seleccione--" Value="-1" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="Si" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div>
                                            <label>Facturas creadas entre las fechas:</label>
                                            <div style="float: left; margin-right: 5px">
                                                <fieldset>
                                                    <label>
                                                        Fecha Inicio
                                                    </label>
                                                    <asp:TextBox ID="txtFacturaFechaInicio" runat="server"> </asp:TextBox>
                                                </fieldset>
                                            </div>
                                            <div style="float: left">
                                                <fieldset>
                                                    <label>
                                                        Fecha Fin
                                                    </label>
                                                    <asp:TextBox ID="txtFacturaFechaFin" runat="server"> </asp:TextBox>
                                                </fieldset>
                                            </div>
                                        </div>
                                        <br />
                                        <div>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnSearch" runat="server" Text="Buscar" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
                                        </div>
                                        <div class="actions"></div>
                                        <asp:GridView ID="gvDetalle" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="10"
                                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                            DataKeyNames="id" AllowPaging="True" EmptyDataText="No existen documentos para esta tarea">
                                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                                            <SelectedRowStyle CssClass="SelectedRow" />
                                            <AlternatingRowStyle CssClass="odd" />
                                            <Columns>
                                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="Consecutivo" HeaderText="No. Radicado" />
                                                <asp:BoundField DataField="NoFactura" HeaderText="No. Factura" />
                                                <asp:BoundField DataField="ValorTotal" HeaderText="Vr Total" />
                                                <asp:BoundField DataField="Escaneada" HeaderText="Factura Cargada" />
                                                <asp:TemplateField HeaderText="Editar" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtnActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar" ToolTip="Actualizar" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Borrar" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtnBorrar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            CommandName="Borrar" ImageUrl="~/Images/delete_16.png" Text="Borrar" ToolTip="Borrar" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Anular" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtnAnular" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            CommandName="Anular" ImageUrl="~/Images/Select_16.png" Text="Borrar" ToolTip="Anular" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Cargar Factura" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            CommandName="Load" ImageUrl="~/Images/list_16.png" Text="Subir Escáner" ToolTip="Subir imagen de factura" OnClientClick="MostrarLoadFiles()" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <PagerTemplate>
                                                <div class="pagingButtons">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                                    Enabled='<%# IIf(gvDetalle.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                                    Enabled='<%# IIf(gvDetalle.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <span class="pagingLinks">[<%= gvDetalle.PageIndex + 1%>-<%= gvDetalle.PageCount%>]</span>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                                    Enabled='<%# IIf((gvDetalle.PageIndex + 1) = gvDetalle.PageCount, "false", "true") %>'
                                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                                    Enabled='<%# IIf((gvDetalle.PageIndex + 1) = gvDetalle.PageCount, "false", "true") %>'
                                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </PagerTemplate>
                                        </asp:GridView>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>

            <asp:Panel ID="pnlAnulacion" runat="server" Visible="false">
                <div class="actions">
                    <label id="lblAnular" runat="server">Motivos y Persona que Anula</label>
                    <asp:TextBox ID="txtUsAnula" runat="server" placeholder="Solicitante" ReadOnly="true"></asp:TextBox>
                    <asp:Button ID="btnUsAnula" Text="..." runat="server" Width="25px" OnClientClick="MostrarSolicitantes()" />
                    <br />
                    <asp:TextBox ID="txtAnular" runat="server" TextMode="MultiLine"></asp:TextBox>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btnAnular" runat="server" Text="Anular"></asp:Button>
                    &nbsp;
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"></asp:Button>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="VisualizarDocumento" style="width: 100%; height: 500px; visibility: hidden">
        <iframe id="ifDocumento" style="width: 100%; height: 100%"></iframe>
    </div>

    <div id="BusquedaSolicitantes">
        <asp:UpdatePanel ID="UPanelSolicitantes" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtCedulaSolicitante" runat="server" placeholder="Cedula"></asp:TextBox>
                <asp:TextBox ID="txtNombreSolicitante" runat="server" placeholder="Nombre"></asp:TextBox>
                <asp:Button ID="btnBuscarSolicitante" runat="server" Text="Buscar" />
                <div class="actions"></div>

                <asp:GridView ID="gvSolicitantes" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                        <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarSolicitantes()" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="LoadFiles">
        <asp:UpdatePanel ID="upScanner" runat="server">
            <ContentTemplate>
                <asp:FileUpload ID="fileup" runat="server" />
                <asp:Button ID="btnLoadFile" runat="server" Text="Cargar archivo" />
                <asp:Button ID="btnViewFile" runat="server" Text="Ver archivo" />
                <div class="actions"></div>
                <br />
            </ContentTemplate>

        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
