<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master" CodeBehind="ReporteFacturasRadicadas.aspx.vb" Inherits="WebMatrix.ReporteFacturasRadicadas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>

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
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
    <%-- <p>
        OK<br />
    </p>--%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
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
            <div style="width: 45%; float: left; border: double">
                <fieldset class="validationGroup">
                    <legend>Filtros de factura</legend>
                    <fieldset>
                        <label>
                            Factura consecutivo
                        </label>
                        <asp:TextBox ID="txtFacturaConsecutivo" runat="server" TextMode="Number"> </asp:TextBox>
                    </fieldset>
                    <fieldset>
                        <label>Proveedor</label>
                        <asp:HiddenField ID="hfProveedor" runat="server" Value="0" />
                        <asp:TextBox ID="txtProveedor" runat="server" ReadOnly="true" Width="250px"></asp:TextBox>
                        <asp:Button ID="btnSearchProveedor" Text="..." runat="server" Width="25px" OnClientClick="MostrarProveedores(); return false;" />
                        &nbsp; 
                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar Proveedor" /><br />
                    </fieldset>
                    <fieldset>
                        <label>Centro de Costo</label>
                        <asp:HiddenField ID="hfCentroCosto" runat="server" Value="0" />
                        <asp:TextBox ID="txtCentroCosto" runat="server" ReadOnly="true" Width="250px"></asp:TextBox>
                        <asp:Button ID="btnSearchCentroCosto" Text="..." runat="server" Width="25px" OnClientClick="MostrarCentroCosto(); return false;" />
                        &nbsp;
                        <asp:Button ID="btnLimpiarCC" runat="server" Text="Limpiar Centro de Costo" /><br />
                    </fieldset>
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
                </fieldset>

                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                <asp:ImageButton ID="btnImgExportarInforme0" runat="server" ImageUrl="~/Images/excel.jpg" Height="5%" Width="5%" Visible="false" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upOrdenesFacturas" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id Factura" />
                    <asp:BoundField DataField="NumeroRadicado" HeaderText="Número Radicado" />
                    <asp:BoundField DataField="NumeroFactura" HeaderText="Número Factura" />
                    <asp:BoundField DataField="IdOrden" HeaderText="Id Orden" />
                    <asp:BoundField DataField="CC" HeaderText="Centro de Costo" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:BoundField DataField="CuentaContable" HeaderText="Cuenta Contable" />
                    <asp:BoundField DataField="ObservacionesAnulacion" HeaderText="Observación Anulación" />
                    <asp:BoundField DataField="ValorFactura" HeaderText="Valor Factura" />
                    <asp:BoundField DataField="IdentificacionProveedor" HeaderText="Identificacion Proveedor" />
                    <asp:BoundField DataField="NombreProveedor" HeaderText="Nombre Proveedor" />
                    <asp:BoundField DataField="FechaRadicadoFactura" HeaderText="Fecha Radicado Factura"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="UsuarioRadica" HeaderText="Usuario Radica" />
                    <asp:BoundField DataField="FacturaRecibida" HeaderText="Fecha Recibida"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Factura_EnviadaAAprobacion" HeaderText="Fecha Enviada a Aprobación"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Factura_Aprobada" HeaderText="Fecha Aprobada"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Factura_Contabilidad" HeaderText="Fecha Enviada a Contabilidad"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Factura_Tesoreria" HeaderText="Fecha Enviada a Tesorería"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Factura_Pagada" HeaderText="Fecha Pagada"
                        DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="ValorPagado" HeaderText="Valor Pagado"
                        DataFormatString="{0:C0}" />
                    <asp:BoundField DataField="Factura_Anulada" HeaderText="Fecha Anulada"
                        DataFormatString="{0:dd/MM/yyyy}" />
                </Columns>
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
                                        Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                        Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
            </asp:GridView>
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

    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
