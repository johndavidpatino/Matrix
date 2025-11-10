<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/FI_.master" CodeBehind="Gestion-Ordenes.aspx.vb" Inherits="WebMatrix.FI_Gestion_Ordenes" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/Site.css" type="text/css" />
    <link rel="stylesheet" href="../Styles/Formulario.css" type="text/css" />
    <script type="text/javascript">
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

            $('#BusquedaJBEJBICC').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "JBEJBICC",
                    width: "600px"
                });

            $('#BusquedaJBEJBICC').parent().appendTo("form");

            $('#BusquedaCuentasContables').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "JBEJBICC",
                    width: "600px"
                });

            $('#BusquedaCuentasContables').parent().appendTo("form");

            $('#BusquedaObservacionesAprobacion').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "ObservacionesAprobacion",
                    width: "600px"
                });

            $('#BusquedaObservacionesAprobacion').parent().appendTo("form");

            $('#anularOrden').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Anular Orden",
                    width: "600px"
                });

            $('#anularOrden').parent().appendTo("form");



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

        function MostrarSolicitantes(tipo) {
            $('#CPH_Section_CPH_Section_hfTipoAprobacion').val(tipo);
            $('#BusquedaSolicitantes').dialog("open");
        }

        function CerrarSolicitantes() {
            $('#BusquedaSolicitantes').dialog("close");
        }
        function MostrarCentrosCostos() {
            $('#BusquedaJBEJBICC').dialog("open");
        }
        function MostrarCuentasContables() {
            $('#BusquedaCuentasContables').dialog("open");
        }
        function CerrarJBEJBICC() {
            $('#BusquedaJBEJBICC').dialog("close");
        }
        function CerrarCuentasContables() {
            $('#BusquedaCuentasContables').dialog("close");
        }

        function MostrarObservacionesAprobacion() {
            $('#BusquedaObservacionesAprobacion').dialog("open");
        }
        function CerrarObservacionesAprobacion() {
            $('#BusquedaObservacionesAprobacion').dialog("close");
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

        function MostrarAnularOrden(tipo) {
            $('#anularOrden').dialog("open");
        }

        function CerrarAnularOrden() {
            $('#anularOrden').dialog("close");
        }
    </script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Section" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ToolkitScriptManager>
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
    <asp:UpdatePanel ID="upTarea" runat="server">
        <ContentTemplate>
            <div style="width: 100%">
                <div id="container">
                    <h2><a>Gestión de </a>Ordenes</h2>
                    <asp:HiddenField ID="hfId" runat="server" Value="0" />
                    <asp:HiddenField ID="hfEstado" runat="server" Value="0" />
                    <asp:HiddenField ID="hfProveedor" runat="server" Value="0" />
                    <asp:HiddenField ID="hfTipoOrden" runat="server" Value="0" />
                    <asp:HiddenField ID="hfSolicitante" runat="server" Value="0" />
                    <asp:HiddenField ID="hfTipoBusqueda" runat="server" Value="0" />
                    <asp:HiddenField ID="hfProveedorSearch" runat="server" Value="0" />
                    <asp:HiddenField ID="hfSolicitanteSearch" runat="server" Value="0" />
                    <asp:HiddenField ID="hfCentroCosto" runat="server" Value="0" />
                    <asp:HiddenField ID="hfDuplicar" runat="server" Value="0" />
                    <asp:HiddenField ID="hfIdAnterior" runat="server" Value="0" />
                    <asp:HiddenField ID="hfEvaluador" runat="server" Value="0" />

                    <div id="menu-form">

                        <nav>
           <ul>
                <li><a><asp:LinkButton ID="LinkButton1" runat="server">Ordenes de Servicio</asp:LinkButton></a></li>
                <li><a><asp:LinkButton ID="LinkButton2" runat="server">Ordenes de Compra</asp:LinkButton></a></li>
            </ul>
           </nav>
                        <asp:Label ID="txtTitulo" runat="server"></asp:Label>
                    </div>
                    <div style="min-height: 400px">
                        <asp:Panel ID="pnlTareasOrdenes" runat="server" Visible="false">
                            <div id="campo-formulario1">
                                <a>
                                    <asp:LinkButton ID="lbMenu1" runat="server">Nueva Orden</asp:LinkButton></a>
                                <br />
                                <a>
                                    <asp:LinkButton ID="lbMenu2" runat="server">Buscar</asp:LinkButton></a>
                                <br />
                                <a>
                                    <asp:LinkButton ID="lbMenu3" runat="server">Aprobaciones</asp:LinkButton></a>
                                <br />
                                <a>
                                    <asp:LinkButton ID="lbMenu4" runat="server" OnClientClick="MostrarObservacionesAprobacion()">Generar PDF</asp:LinkButton></a>
                                <br />
                                <div class="actions"></div>
                                <asp:Panel ID="pnlBuscar" runat="server" Visible="false">
                                    <asp:TextBox ID="txtOrdenSearch" runat="server" placeholder="No. Orden"></asp:TextBox>
                                    <asp:TextBox ID="txtFechaSearch" runat="server" placeholder="Fecha"></asp:TextBox>
                                    <asp:TextBox ID="txtJobBookSearch" runat="server" placeholder="JobBook"></asp:TextBox><br />
                                    <asp:TextBox ID="txtProveedorBusqueda" placeholder="Proveedor" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:Button ID="btnProveedorBusqueda" Text="..." runat="server" Width="25px" OnClientClick="MostrarProveedores()" /><br />

                                    <asp:TextBox ID="txtSolicitanteBusqueda" runat="server" placeholder="Solicitante" ReadOnly="true"></asp:TextBox>
                                    <asp:Button ID="btnSolicitanteBusqueda" Text="..." runat="server" Width="25px" OnClientClick="MostrarSolicitantes()" />
                                    <br />
                                    <label>Centro de Costos</label>
                                    <asp:DropDownList ID="ddlCentroDeCostosSearch" runat="server"></asp:DropDownList>
                                    <asp:CheckBox ID="chbMisOrdenes" Text="Ver solo mis órdenes" runat="server" />
                                    <asp:Button ID="btnSearchOk" runat="server" Text="Buscar" />
                                    <asp:Button ID="btnSearchCancel" runat="server" Text="Cancelar" />
                                    <div class="actions"></div>

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
                                            <asp:BoundField DataField="SubTotal" HeaderText="Total" DataFormatString="{0:C0}" />
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarProveedores()" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                </asp:Panel>
                                <asp:Panel ID="pnlAprobaciones" runat="server" Visible="false">
                                    <asp:TextBox ID="txtComentarios" Width="150px" runat="server" placeholder="Agregue sus comentarios aquí" TextMode="MultiLine"></asp:TextBox>
                                    <asp:Button ID="btnEnviarAprobacion" runat="server" Text="Enviar a Aprobación" />
                                    <div style="width: 100%; overflow-x: scroll">
                                        <asp:GridView ID="gvAprobaciones" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                            DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                                            <SelectedRowStyle CssClass="SelectedRow" />
                                            <AlternatingRowStyle CssClass="odd" />
                                            <Columns>
                                                <asp:CheckBoxField DataField="Aprobado" HeaderText="Aprobado" />
                                                <asp:BoundField DataField="FechaAprobacion" HeaderText="Fecha" />
                                                <asp:BoundField DataField="Usuario" HeaderText="Aprobó" />
                                                <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </asp:Panel>
                            </div>
                            <asp:Panel ID="pnlOrden" runat="server" Visible="false">
                                <asp:HiddenField ID="hfIdWFid" runat="server" Value="0" />
                                <div id="campo-formulario2">
                                    <asp:Label ID="lblAnulada" runat="server" Text="Esta orden esta anulada por lo que no podra modificarla" ForeColor="Red" Visible="false"></asp:Label>
                                    <label>No. Orden</label>
                                    <asp:TextBox ID="txtNoOrden" ReadOnly="true" runat="server"></asp:TextBox>
                                    <label>Proveedor</label>
                                    <asp:TextBox ID="txtProveedor" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:Button ID="btnSearchProveedor" Text="..." runat="server" Width="25px" OnClientClick="MostrarProveedores()" /><br />
                                    <label>Fecha</label>
                                    <asp:TextBox ID="txtFecha" runat="server"></asp:TextBox>
                                    <label>Fecha Entrega</label>
                                    <asp:TextBox ID="txtFechaEntrega" runat="server"></asp:TextBox>
                                    <label>Departamento</label>
                                    <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="true"></asp:DropDownList>
                                    <label>Ciudad</label>
                                    <asp:DropDownList ID="ddlCiudad" runat="server"></asp:DropDownList>
                                    <label>Forma Pago</label>
                                    <asp:TextBox ID="txtFormaPago" runat="server"></asp:TextBox>
                                    <label>Evalua Proveedor</label>
                                    <asp:TextBox ID="txtBeneficiario" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:Button ID="btnBeneficiario" Text="..." runat="server" OnClientClick="MostrarSolicitantes('EvaluaProveedor')" /><br />
                                    <label>Tipo</label>
                                    <asp:DropDownList ID="ddlTipo" runat="server" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <label id="lblCentroCostos" runat="server" visible="false">Centro de Costos</label>
                                    <asp:DropDownList ID="ddlCentroCostos" runat="server" Enabled="false" Visible="false"></asp:DropDownList>
                                    <label id="lblJBIJBE" runat="server" visible="false">Codigo JBIJBE</label>
                                    <asp:TextBox ID="txtJBIJBE" runat="server" Visible="false" AutoPostBack="true"></asp:TextBox>
                                    <label id="lblNombreJBIJBE" runat="server" visible="false">Nombre JBIJBE</label>
                                    <asp:TextBox ID="txtNombreJBIJBE" runat="server" Visible="false"></asp:TextBox>
                                    <asp:Button ID="btnSearchCentroCostos" Text="..." runat="server" Width="25px" OnClientClick="MostrarCentrosCostos()" Visible="false" /><br />
                                    <label id="lblApruebaOrden" runat="server" visible="false">Aprueba la orden:</label>
                                    <asp:DropDownList ID="ddlApruebaOrden" runat="server" Visible="false"></asp:DropDownList>
                                    <label>Aprueba Factura</label>
                                    <asp:HiddenField ID="hfTipoAprobacion" runat="server" />
                                    <asp:TextBox ID="txtApruebaFactura" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:Button ID="btnApruebaFactura" Text="..." runat="server" OnClientClick="MostrarSolicitantes('ApruebaFactura')" /><br />
                                    <asp:DropDownList ID="ddlApruebaFactura" runat="server" Visible="false"></asp:DropDownList>
                                    <label>Genérica (Marque el Check si es una Órden que se puede agregar a más de una Factura)</label>
                                    <asp:CheckBox ID="chbGenerica" runat="server" />
                                    <br />
                                    <br />
                                    <asp:Button ID="btnSave" runat="server" Text="Guardar" />
                                    <asp:Button ID="btnduplicar" runat="server" Text="Duplicar" Visible="true" />
                                    <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
                                    <asp:Button ID="btnAnular" runat="server" Text="Anular" OnClientClick="MostrarAnularOrden();return false;" />
                                    <br />
                                    <asp:Button ID="btnCancelEnvio" runat="server" Text="Cancelar Envío Aprobación" Width="190px" />
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlDetalleOrden" runat="server" Visible="false">
                                <div id="campo-formulario3">
                                    <asp:Panel ID="pnlDetalleOrdenes" runat="server">
                                        <asp:HiddenField ID="hfDetalleId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfTypeFile" runat="server" />
                                        <a>Detalle de la orden</a>
                                        <br />
                                        <label>Fecha</label>
                                        <asp:TextBox ID="txtFechaDetalle" runat="server"></asp:TextBox>
                                        <label>Descripción</label>
                                        <asp:TextBox ID="txtDescripcionDetalle" runat="server"></asp:TextBox>
                                        <label>Cantidad</label>
                                        <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>
                                        <label>Valor Unitario</label>
                                        <asp:TextBox ID="txtValorUnitario" runat="server"></asp:TextBox>
                                        <label>Cuenta Contable</label>
                                        <asp:DropDownList ID="ddlCuentasContables" runat="server" Enabled="false"></asp:DropDownList>
                                        <asp:Button ID="btnCuentasContables" Text="..." runat="server" Width="25px" OnClientClick="MostrarCuentasContables()" /><br />
                                        <br />
                                        <asp:FileUpload ID="FileUpData" runat="server" />
                                        <br />
                                        <br />
                                        <asp:Button ID="btnLoadFile" runat="server" Text="Cargar Archivo" />
                                        &nbsp;&nbsp;
                                       
                                        <br />
                                        <br />
                                        <asp:Label ID="lblFileIncorrect" runat="server" Text="Archivo Incorrecto, por favor asegurese que es un archivo excel" Visible="False"></asp:Label>
                                        <br />
                                        <div class="actions"></div>
                                        <asp:Button ID="btnAdd" runat="server" Text="Agregar" />
                                        <asp:Button ID="btnCancelDetalle" runat="server" Text="Cancelar" />

                                        <div class="actions"></div>
                                        <asp:GridView ID="gvDetalle" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                            DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                                            <SelectedRowStyle CssClass="SelectedRow" />
                                            <AlternatingRowStyle CssClass="odd" />
                                            <Columns>
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="Descripcion" HeaderText="Descripción" />
                                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                <asp:BoundField DataField="VrUnitario" HeaderText="Vr Unitario" DataFormatString="{0:c0}" />
                                                <asp:BoundField DataField="NombreCuenta" HeaderText="Cuenta Contable" />
                                                <asp:TemplateField HeaderText="Borrar" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            CommandName="Borrar" ImageUrl="~/Images/delete_16.png" Text="Borrar" ToolTip="Borrar" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                        <asp:Label ID="lblSubtotal" runat="server"></asp:Label>
                                    </asp:Panel>

                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlAprobacion" runat="server" Visible="false">
                            </asp:Panel>
                        </asp:Panel>
                        <div class="actions"></div>
                        <asp:Panel ID="pnlEstimacionXTrabajo" runat="server" Visible="false">
                            <p>Estimación de tareas</p>
                            <asp:HiddenField ID="hfWfiDEstimacion" runat="server" />
                            <asp:HiddenField ID="hfTareaIdEstimacion" runat="server" />
                            <asp:GridView ID="gvEstimacion" runat="server" DataKeyNames="Id,TareaId,UsuarioId,Retraso,RolEstima,RolEjecuta" CssClass="displayTable" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
                                EmptyDataText="No se encuentran tareas para este trabajo">
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                                    <asp:TemplateField HeaderText="Inicio" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFInicio" runat="server" Text='<%#  Eval("FIniP", "{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Fin" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtFFin" runat="server" Text='<%#  Eval("FFinP", "{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Observaciones" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtObservacionesPlaneacion" runat="server" Text='<%#  Eval("ObservacionesPlaneacion") %>'></asp:TextBox>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="No Aplica" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chbAplica" runat="server" Checked='<%# IIF(Eval("ESTADOID")=6,True,False) %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnGuardarEstimacion" runat="server" Text="Guardar Cambios" />
                        </asp:Panel>
                    </div>
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
    <div id="BusquedaSolicitantes">
        <asp:UpdatePanel ID="UPanelSolicitantes" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtCedulaSolicitante" runat="server" placeholder="Cedula"></asp:TextBox>
                <asp:TextBox ID="txtNombreSolicitante" runat="server" placeholder="Nombre"></asp:TextBox>
                <asp:Button ID="btnBuscarSolicitante" runat="server" Text="Buscar" />
                <div class="actions"></div>
                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvSolicitantes" runat="server" Width="90%" AutoGenerateColumns="False"
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
                </div>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="BusquedaJBEJBICC">
        <asp:UpdatePanel ID="upJBEJBICC" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtJBEJBICC" runat="server" placeholder="Valor busqueda"></asp:TextBox>
                <asp:Button ID="btnBuscarJBEJBICC" runat="server" Text="Buscar" />
                <div class="actions"></div>

                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvJBEJBICC" runat="server" Width="80%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarJBEJBICC()" />
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
    <div id="BusquedaCuentasContables">
        <asp:UpdatePanel ID="upCuentasContables" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtNumeroCuenta" runat="server" placeholder="Número cuenta"></asp:TextBox>
                <asp:TextBox ID="txtDescripcion" runat="server" placeholder="Descripción"></asp:TextBox>
                <asp:Button ID="btnBuscarCuentaContable" runat="server" Text="Buscar" />
                <div class="actions"></div>

                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvCuentasContables" runat="server" Width="80%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="NumeroCuenta" HeaderText="NumeroCuenta" />
                            <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarCuentasContables()" />
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

    <div id="BusquedaObservacionesAprobacion">
        <asp:UpdatePanel ID="upObservacionesAprobacion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>

                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvObservacionesAprobacion" runat="server" Width="80%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="id" AllowPaging="False" EmptyDataText="No hay ninguna Observación registrada para esta Orden">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="IdOrden" HeaderText="Id Orden" />
                            <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                            <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:Button ID="btnGenerarPDF" runat="server" Text="Generar PDF" OnClientClick="CerrarObservacionesAprobacion()" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div id="anularOrden">
        <asp:UpdatePanel ID="upAnularOrden" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:Label ID="lblObserAnulacion" Text="Observación de anulación" AssociatedControlID="txtObservacionAnulacion" runat="server"></asp:Label>
                <asp:TextBox ID="txtObservacionAnulacion" runat="server" placeholder="Observación de anulación"></asp:TextBox>
                <asp:Button ID="btnAnularOrden" runat="server" Text="Guardar" OnClientClick="CerrarAnularOrden()" />
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
