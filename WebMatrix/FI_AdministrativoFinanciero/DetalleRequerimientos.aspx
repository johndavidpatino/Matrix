<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="DetalleRequerimientos.aspx.vb" Inherits="WebMatrix.DetalleRadicacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function loadPlugins() {
            $("#<%= txtOrdenFechaInicio.ClientID%>").mask("99/99/9999");
            $("#<%= txtOrdenFechaInicio.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtOrdenFechaFin.ClientID%>").mask("99/99/9999");
            $("#<%= txtOrdenFechaFin.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
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

            //validationForm();
        }

        $(document).ready(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPicker);
            bindPicker();


            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $('#BusquedaProveedores').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Proveedores disponibles",
                    width: "600px"
                });

            $('#BusquedaProveedores').parent().appendTo("form");

            $('#BusquedaCentroCosto').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Centro de Costo",
                    width: "600px"
                });

            $('#BusquedaCentroCosto').parent().appendTo("form");

            $('#PresupuestosAsignadosXEstudio').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Presupuestos asignados",
                    width: "600px",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });

            loadPlugins();
        });

        function bindPicker() {
            $(".input-fecha").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function MostrarProveedores() {
            $('#BusquedaProveedores').dialog("open");
        }

        function CerrarProveedores() {
            $('#BusquedaProveedores').dialog("close");
        }

        function MostrarCentroCosto() {
            $('#BusquedaCentroCosto').dialog("open");
        }

        function CerrarCentroCosto() {
            $('#BusquedaCentroCosto').dialog("close");
        }

        function MostrarPresupuestosAsignadosXEstudio() {
            $('#PresupuestosAsignadosXEstudio').dialog("open");
        }

        function ActualizarPresupuestosAsignados(rowIndex, checked) {

            if (checked == true) {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() + ";" + rowIndex + ";");
            }
            else {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val().replace(";" + rowIndex + ";", ""));
            }
        }

        $(function () {
            $("#gvReporte").dialog({
                autoOpen: false,
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                }
            });

            $("#btnCerrarTrabajo").click(function () {
                $("#gvReporte").dialog("open");
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu" style="float: right;">
        <li>
            <a href="../FI_AdministrativoFinanciero/Default-Compras.aspx">COMPRAS</a>
        </li>
        <li>
            <a href="../Home/Default.aspx">INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Radicación y Aprobación
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Detalle de Radicación y Aprobación
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
    Detalle de Radicación y Aprobación de Ordenes de Servicio, de Compra y Requerimientos de Servicio
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
    <style>
        #stylized label {
            margin: 0;
            color: #1c1c1c;
            font-family: 'Roboto', sans-serif;
            font-size: 13px;
            width: 100%;
            text-align: left;
        }

        #stylized input, #stylized select {
            margin: 0px;
            width: 100%;
            padding: 0px;
        }

        #stylized select {
            padding: 6px 12px;
            height: 28px;
        }

        #stylized input[type=submit] {
            font-size: 12px;
        }
    </style>
    <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
    <div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer;">
            x
        </div>
        <p>
            <span class="ui-icon ui-icon-info"></span><strong>Info: </strong>
            <label id="lblTextInfo">
            </label>
        </p>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('error');" style="cursor: pointer;">
            x
        </div>
        <p>
            <span class="ui-icon ui-icon-alert"></span><strong>Error: </strong>
            <label id="lbltextError">
            </label>
        </p>
    </div>
    <asp:UpdatePanel ID="upBuscar" runat="server">
        <ContentTemplate>
            <div id="filtros" class="row" style="margin-top: 15px;">
                <div class="col-md-6">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 style="padding: 0px;">Filtro de Ordenes/Requerimientos</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="col-md-5">
                                    <label for="txtIdOrden">Id Orden:</label>
                                    <div class="col-sm-12" style="padding-left: 0px;">
                                        <asp:TextBox runat="server" ID="txtIdOrden" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <label for="idTipoOrden">Tipo Orden:</label>
                                    <div class="col-sm-12" style="padding-left: 0px;">
                                        <asp:DropDownList ID="ddlTipoOrden" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Seleccione" Value="-1" />
                                            <%-- <asp:ListItem Text="Orden de Servicio" Value="1" />
                                            <asp:ListItem Text="Orden de Compra" Value="2" />--%>
                                            <asp:ListItem Text="Requerimiento de Servicio" Value="3" />
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="row"></div>
                            <div class="form-group">
                                <div class="col-md-5">
                                    <label for="txtProveedor">Proveedor</label>
                                    <div class="col-sm-12" style="padding-left: 0px;">
                                        <asp:TextBox runat="server" ID="txtProveedor" CssClass="form-control" ReadOnly="true" />
                                        <asp:HiddenField ID="hfProveedor" runat="server" Value="0" />
                                    </div>
                                </div>
                                <div class="col-md-5" style="padding-top: 20px;">
                                    <div class="row"></div>
                                    <div class="col-md-1" style="padding-left: 0px;">
                                        <asp:Button ID="btnSearchProveedor" Text="..." runat="server" Width="25px" CssClass="btn btn-examinar" OnClientClick="MostrarProveedores(); return false;" />
                                    </div>
                                    <div class="col-md-7">
                                        <div class="row"></div>
                                        <asp:Button ID="btnLimpiar" Text="Limpiar Proveedor" runat="server" CssClass="btn btn-primary" />
                                    </div>
                                </div>
                            </div>
                            <div class="row"></div>
                            <div class="form-group">
                                <div class="col-md-5">
                                    <label for="txtFechaInicio">ID Trabajo:</label>
                                    <div class="col-sm-12" style="padding-left: 0px;">
                                        <asp:TextBox runat="server" ID="txtIdTrabajo" CssClass="form-control" />
                                    </div>
                                </div>
                            </div>
                            <div class="row"></div>
                            <div class="form-group">
                                <div class="col-md-5">
                                    <label for="txtFechaInicio">Fecha Inicio:</label>
                                    <div class="col-sm-12" style="padding-left: 0px;">
                                        <asp:TextBox runat="server" ID="txtOrdenFechaInicio" CssClass="form-control input-fecha" />
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <label for="txtFechaFin">Fecha Fin:</label>
                                    <div class="col-sm-12" style="padding-left: 0px;">
                                        <asp:TextBox runat="server" ID="txtOrdenFechaFin" CssClass="form-control input-fecha" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-md-5" style="margin: 0 10px;">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 style="padding: 0px;">Filtro de Facturas</h3>
                        </div>
                        <div class="panel-body">
                            <div class="form-group">
                                <div class="col-md-5">
                                    <label for="txtIdFactura">Factura Consecutivo:</label>
                                    <div class="col-sm-12" style="padding-left: 0px;">
                                        <asp:TextBox runat="server" ID="txtFacturaConsecutivo" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="row"></div>
                                <div class="form-group">
                                    <div class="col-md-5">
                                        <label for="txtFechaInicioFac">Fecha Inicio:</label>
                                        <div class="col-sm-12" style="padding-left: 0px;">
                                            <asp:TextBox runat="server" ID="txtFacturaFechaInicio" CssClass="form-control input-fecha" />
                                        </div>
                                    </div>
                                    <div class="col-md-5">
                                        <label for="txtFechaFinFac">Fecha Fin:</label>
                                        <div class="col-sm-12" style="padding-left: 0px;">
                                            <asp:TextBox runat="server" ID="txtFacturaFechaFin" CssClass="form-control input-fecha" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="panel panel-default">
                        <div class="panel-body">
                            <div class="col-md-3">
                                <asp:Button ID="btnBuscar" Text="Buscar" runat="server" CssClass="btn btn-primary" />
                                <asp:HiddenField ID="hfNumRequerimiento" runat="server" Value="0" />
                            </div>
                            <div class="col-md-3 col-md-offset-1">
                                <asp:Button ID="btnExportar" Text="Exportar" runat="server" CssClass="btn btn-primary" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upOrdenesFacturas" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row" style="margin-top: 15px;">
                <div class="col-md-12" style="overflow-x: scroll;">
                    <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="true" HeaderStyle-CssClass="cabeceraTabla"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="" />
                        <PagerTemplate>
                            <div class="pagingButtons">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td>
                                            <span class="pagingLinks">[<asp:Label ID="lblPaginaActual" runat="server"></asp:Label>-<asp:Label ID="lblCantidadPaginas" runat="server"></asp:Label>]</span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true") %>'
                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true") %>'
                                                SkinID="paging">Ultimo »</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div id="BusquedaProveedores">
        <asp:UpdatePanel ID="UPanelProveedores" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtNitProveedor" runat="server" placeholder="NIT o CC"></asp:TextBox>
                <asp:TextBox ID="txtNombreProveedor" runat="server" placeholder="Razón Social o Nombre"></asp:TextBox>
                <asp:Button ID="btnBuscarProveedor" runat="server" Text="Buscar" />
                <div class="actions"></div>
                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvProveedores" runat="server" Width="90%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Identificacion" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                            <asp:BoundField DataField="Nombre" HeaderText="RazonSocial" />
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarProveedores()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="BusquedaCentroCosto">
        <asp:UpdatePanel ID="UPanelCentroCosto" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtNumeroCuenta" runat="server" placeholder="Número Centro"></asp:TextBox>
                <asp:TextBox ID="txtDescripcionCuenta" runat="server" placeholder="Descripción"></asp:TextBox>
                <asp:Button ID="btnBuscarCentroCosto" runat="server" Text="Buscar" />
                <div class="actions"></div>
                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvCentroCosto" runat="server" Width="80%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarCentroCosto()" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <%--<script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>--%>
</asp:Content>
