<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/FI_.master" CodeBehind="Gestion-Ordenes-Aprobacion.aspx.vb" Inherits="WebMatrix.FI_Gestion_Ordenes_Aprobacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/Site.css" type="text/css" />
    <link rel="stylesheet" href="../Styles/Formulario.css" type="text/css" />
    <script type="text/javascript">

        function loadPlugins() {
            $("#VisualizarDocumento").resizable();
        }

        $(document).ready(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerIni);
            bindPickerIni();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerFin);
            bindPickerFin();


            loadPlugins();

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


            $('#CargaArchivos').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Carga archivos",
                    width: "600px"
                });


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
        function cargarOrden(id) {
            var hfTipoOrden = $('#CPH_Section_CPH_Section_hfTipoOrden').val();
            var tipoOrden = "SERVICIO";
            switch (hfTipoOrden) {
                case "1":
                    tipoOrden = "SERVICIO";
                    break;
                case "2":
                    tipoOrden = "COMPRA";
                    break;
                case "3":
                    tipoOrden = "REQUERIMIENTO";
                    break;
            }

            $('#ifDocumento').attr('src', '/Files/ORDEN' + tipoOrden + '-' + id + '.pdf');
            $('#CPH_Section_CPH_Section_pnlAprobaciones').css('visibility', 'visible');
            $('#VisualizarDocumento').css('visibility', 'visible');
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
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Section" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ToolkitScriptManager>
    <div style="height: 100%">
        <asp:UpdatePanel ID="upTarea" runat="server">
            <ContentTemplate>
                <div style="width: 100%">
                    <h2><a>Gestión de Aprobacion de </a>Ordenes</h2>
                    <asp:HiddenField ID="hfId" runat="server" Value="0" />
                    <asp:HiddenField ID="hfTipoOrden" runat="server" Value="0" />
                    <div id="menu-form">
                        <ul>
                            <li>
                                <asp:LinkButton ID="LinkButton1" runat="server">Ordenes de Servicio</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="LinkButton2" runat="server">Ordenes de Compra</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="LinkButton3" runat="server">Ordenes de Requerimiento</asp:LinkButton>
                            </li>
                            <li>
                                <asp:LinkButton ID="LinkButton7" runat="server">Regresar</asp:LinkButton>
                            </li>
                        </ul>
                        <asp:Label ID="txtTitulo" runat="server"></asp:Label>
                    </div>
                </div>
                <asp:GridView ID="gvOrdenes" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="No." />
                        <asp:BoundField DataField="Proveedor" HeaderText="Proveedor" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="CargadoA" HeaderText="CC" />
                        <asp:BoundField DataField="SubTotal" HeaderText="Total"
                            DataFormatString="{0:C0}" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                        <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Orden" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Panel ID="pnlAprobaciones" runat="server" Style="visibility: hidden">
                    <br />
                    <div id="aprobaciones">
                        <asp:Label ID="lblEstadoAutorizaciones" runat="server" Text="Estado de aprobaciones"></asp:Label>
                        <asp:GridView ID="gvAprobaciones" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                <asp:BoundField DataField="FechaAprobacion" HeaderText="Fecha" />
                                <asp:BoundField DataField="Aprobado" HeaderText="Aprobado" />
                                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                            </Columns>
                        </asp:GridView>
                    </div>
                    <br />
                    <asp:Label ID="lblAprobacionActual" runat="server" Text="Aprobación actual"></asp:Label>
                    <div>
                        <asp:TextBox ID="txtComentarios" Width="150px" runat="server" placeholder="Agregue sus comentarios aquí" TextMode="MultiLine"></asp:TextBox>
                        <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" OnClientClick="$('#CPH_Section_CPH_Section_pnlAprobaciones').css('visibility', 'hidden');" />
                        <asp:Button ID="btnNoAprobar" runat="server" Text="No Aprobar" />
                    </div>
                </asp:Panel>
                <div id="VisualizarDocumento" style="width: 100%; height: 500px; visibility: hidden">
                    <iframe id="ifDocumento" style="width: 100%; height: 100%"></iframe>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="BusquedaSolicitantes">
        <asp:UpdatePanel ID="UPanelSolicitantes" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtCedulaSolicitante" runat="server" placeholder="Cedula"></asp:TextBox>
                <asp:TextBox ID="txtNombreSolicitante" runat="server" placeholder="Nombre"></asp:TextBox>
                <asp:Button ID="btnBuscarSolicitante" runat="server" Text="Buscar" />
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
