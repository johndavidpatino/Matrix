<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/FI_.master" CodeBehind="Recepcion-Facturas-Antiguo.aspx.vb" Inherits="WebMatrix.FI_Recepcion_Facturas_Antiguo" %>

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
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Section" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel ID="upTarea" runat="server">
        <ContentTemplate>
            <div style="width: 100%">
                <div id="container">
                    <h2><a>Recepción de </a>Facturas</h2>
                    <asp:HiddenField ID="hfId" runat="server" Value="0" />
                    <asp:HiddenField ID="hfEstado" runat="server" Value="0" />
                    <asp:HiddenField ID="hfProveedor" runat="server" Value="0" />
                    <asp:HiddenField ID="hfTipoOrden" runat="server" Value="0" />
                    <asp:HiddenField ID="hfSolicitante" runat="server" Value="0" />
                    <asp:HiddenField ID="hfTipoBusqueda" runat="server" Value="0" />
                    <asp:HiddenField ID="hfProveedorSearch" runat="server" Value="0" />
                    <asp:HiddenField ID="hfSolicitanteSearch" runat="server" Value="0" />
                    <div id="menu-form">

                        <nav>
           <ul>
                <li><a><asp:LinkButton ID="LinkButton1" runat="server">Ordenes de Servicio</asp:LinkButton></a></li>
                <li><a><asp:LinkButton ID="LinkButton2" runat="server">Ordenes de Compra</asp:LinkButton></a></li>
               <li><a><asp:LinkButton ID="LinkButton3" runat="server">Requisiciones de servicio</asp:LinkButton></a></li>
            </ul>
           </nav>
                        <asp:Label ID="txtTitulo" runat="server"></asp:Label>
                    </div>
                    <div style="min-height: 400px">
                        <asp:Panel ID="pnlTareasOrdenes" runat="server" Visible="false">
                            <div id="campo-formulario1">
                                <a>
                                    <asp:LinkButton ID="lbMenu2" runat="server">Nueva radicación</asp:LinkButton></a>
                                <br />
                                <a>
                                    <asp:LinkButton ID="lbMenu3" runat="server">Aprobaciones</asp:LinkButton></a>
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
                                            <asp:BoundField DataField="SubTotal" HeaderText="Total" />
                                            <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                            <asp:TemplateField HeaderText="Recibir" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="Grabar" ImageUrl="~/Images/Select_16.png" Text="Grabar" ToolTip="Tareas" />
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
                                    <asp:Button ID="btnAprobar" runat="server" Text="Aprobar" />
                                    <asp:Button ID="btnNoAprobar" runat="server" Text="No Aprobar" />
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
                            <asp:Panel ID="pnlOrden" runat="server" Visible="false" Enabled="false">
                                <asp:HiddenField ID="hfIdWFid" runat="server" Value="0" />
                                <div id="campo-formulario2">
                                    <a>Detalle de la orden</a>
                                    <label>No. Orden</label>
                                    <asp:TextBox ID="txtNoOrden" ReadOnly="true" runat="server"></asp:TextBox>
                                    <label>Proveedor</label>
                                    <asp:TextBox ID="txtProveedor" runat="server" ReadOnly="true"></asp:TextBox><br />
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
                                    <label>Beneficiario Compra</label>
                                    <asp:TextBox ID="txtBeneficiario" runat="server"></asp:TextBox>
                                    <label>Tipo</label>
                                    <asp:DropDownList ID="ddlTipo" runat="server" AutoPostBack="true">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="JBE - JobBookExterno" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="JBI - JbBookInterno" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Centro de Costo" Value="3"></asp:ListItem>
                                    </asp:DropDownList>
                                     <label id="lblCentroCostos" runat="server" visible="false">Centro de Costos</label>
                                    <asp:DropDownList ID="ddlCentroCostos" runat="server" Enabled="false" Visible="false"></asp:DropDownList>
                                     <label id="lblJBIJBE" runat="server" visible="false">Codigo JBIJBE</label>
                                    <asp:TextBox ID="txtJBIJBE" runat="server" Visible="false" ReadOnly="true" ></asp:TextBox>
                                    <label id="lblNombreJBIJBE" runat="server" visible="false">Nombre JBIJBE</label>
                                    <asp:TextBox ID="txtNombreJBIJBE" runat="server" Visible="false" ReadOnly="true"></asp:TextBox>
                                    <label>Solicitante</label>
                                    <asp:TextBox ID="txtSolicitante" runat="server" ReadOnly="true"></asp:TextBox><br />
                                    <asp:Label ID="lblSubtotal" runat="server"></asp:Label>
                                    <asp:Label ID="lblVrTotal" runat="server"></asp:Label>
                                    <div class="actions"></div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlDetalleOrden" runat="server" Visible="false">
                                <div id="campo-formulario3">
                                    <asp:Panel ID="pnlDetalleOrdenes" runat="server">
                                        <asp:HiddenField ID="hfDetalleId" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfIdFactura" runat="server" Value="0" />
                                        <a>Facturas radicadas</a>
                                        <br />
                                        <label>No. Radicado</label>
                                        <asp:TextBox ID="txtConsecutivo" runat="server"></asp:TextBox>
                                        <label>No. Factura</label>
                                        <asp:TextBox ID="txtNoFactura" runat="server"></asp:TextBox>
                                        <label>Cantidad</label>
                                        <asp:TextBox ID="txtCantidad" runat="server"></asp:TextBox>
                                        <label>Valor Unitario</label>
                                        <asp:TextBox ID="txtValorUnitario" runat="server"></asp:TextBox>
                                        <label>Subtotal</label>
                                        <asp:TextBox ID="txtSubTotal" runat="server"></asp:TextBox>
                                        <label>Valor Total</label>
                                        <asp:TextBox ID="txtValorTotal" runat="server"></asp:TextBox>
                                        <label>Observaciones</label>
                                        <asp:TextBox ID="txtObservaciones" runat="server"></asp:TextBox>
                                        <label>Tipo de documento</label>
                                        <asp:DropDownList ID="ddlTipoDocRadicado" runat="server">
                                            <asp:ListItem Text="--Seleccione--" Value="-1" Selected="True"></asp:ListItem>
                                            <asp:ListItem Text="Factura" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="Cuenta de cobro" Value="2"></asp:ListItem>
                                        </asp:DropDownList>
                                        <div class="actions"></div>
                                        <asp:Button ID="btnAdd" runat="server" Text="Agregar" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancelar" />
                                        <div class="actions"></div>
                                        <asp:GridView ID="gvDetalle" runat="server" Width="100%" AutoGenerateColumns="False"
                                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                            DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen documentos para esta tarea">
                                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                                            <SelectedRowStyle CssClass="SelectedRow" />
                                            <AlternatingRowStyle CssClass="odd" />
                                            <Columns>
                                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" />
                                                <asp:BoundField DataField="Consecutivo" HeaderText="No. Radicado" />
                                                <asp:BoundField DataField="NoFactura" HeaderText="No. Factura" />
                                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                                <asp:BoundField DataField="ValorTotal" HeaderText="Vr Total" />
                                                <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtnActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar" ToolTip="Actualizar" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Borrar" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtnBorrar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            CommandName="Borrar" ImageUrl="~/Images/delete_16.png" Text="Borrar" ToolTip="Borrar"/>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Anular" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgBtnAnular" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            CommandName="Anular" ImageUrl="~/Images/Select_16.png" Text="Borrar" ToolTip="Anular"/>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Escáner" ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                            CommandName="Load" ImageUrl="~/Images/list_16.png" Text="Subir Escáner" ToolTip="Subir imagen de factura" OnClientClick="MostrarLoadFiles()" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </div>
                            </asp:Panel>

                             <asp:Panel ID="pnlAnulacion" runat="server" Visible="false">
                                <div class="actions">
                                    <label id="lblAnular" runat="server">Motivos y Persona que Anula</label>
                                     <asp:TextBox ID="txtUsAnula" runat="server" placeholder="Solicitante" ReadOnly="true"></asp:TextBox>
                                    <asp:Button ID="btnUsAnula" Text="..." runat="server" Width="25px" OnClientClick="MostrarSolicitantes()" />
                                    <br />
                                    <asp:TextBox ID="txtAnular" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnAnular" runat="server" Text="Anular"></asp:button>
                                    &nbsp;
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar"></asp:button>
                                </div>
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

                <asp:GridView ID="gvProveedores" runat="server" Width="100%" AutoGenerateColumns="False"
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
        <asp:UpdatePanel ID="upScanner" runat="server" >
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
