<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterFormatos.master" CodeBehind="QuejasReclamosProveedores.aspx.vb" Inherits="WebMatrix.QuejasReclamosProveedores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Styles/formatos.css" rel="stylesheet" />
    <link href="../Styles/forms.css" rel="stylesheet" />
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script>
        $(document).ready(function () {
            $('#BusquedaProveedores').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Proveedores disponibles",
                    width: "600px"
                });

            $('#BusquedaProveedores').parent().appendTo("form");

            $('#BusquedaCC').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Centros de Costos",
                    width: "600px"
                });

            $('#BusquedaCC').parent().appendTo("form");

            loadPlugins();
        });

        function loadPlugins() {

            validationForm();

            $("#<%= txtRespuestaFechaImp.ClientId %>").mask("99/99/9999");
            $("#<%= txtRespuestaFechaImp.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        };

        function MostrarProveedores() {
            $('#BusquedaProveedores').dialog("open");
        }

        function CerrarProveedores() {
            $('#BusquedaProveedores').dialog("close");
        }

        function MostrarCC() {
            $('#BusquedaCC').dialog("open");
        }

        function CerrarCC() {
            $('#BusquedaCC').dialog("close");
        }
    </script>
    <style>
        .pos-center{
            margin:0px auto !important;
            float:none !important;
            text-align:center !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_ContentTitulo" runat="server">
    QUEJAS Y RECLAMOS DE PROVEEDORES
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ContentSubtitulo" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:Button Text="Nueva Queja/Reclamo" ID="btnNuevo" runat="server" />
    <asp:Button Text="Ver mis Formatos Realizados" ID="btnListado" runat="server" Visible="true" />
    <div class="spacer"></div>
    <asp:UpdatePanel runat="server" ID="upNuevoQuejasProveedor">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnNuevo" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnListado" EventName="Click" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlListado" Visible="true">
                <div style="width: 100%; height: 300px; margin: 0px auto; border: 1px solid #000000; border-radius: 5px; overflow-y: scroll;">
                    <asp:GridView ID="gvQuejasProveedores" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="NombreProveedor" HeaderText="Proveedor" />
                            <asp:BoundField DataField="Contacto" HeaderText="Contacto" />
                            <asp:BoundField DataField="TipoEstudioN" HeaderText="Tipo" />
                            <asp:BoundField DataField="NoEstudio" HeaderText="Estudio" />
                            <asp:BoundField DataField="FechaImplementacion" HeaderText="Fecha Implementación" />
                            <asp:BoundField DataField="Usuario1" HeaderText="Usuario" />
                            <asp:TemplateField HeaderText="Satisfaccion" ItemStyle-CssClass="pos-center">
                                <ItemTemplate>
                                    <asp:Image ID="imgSatisfaccion" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" 
                                        ImageUrl='<%# If(Eval("Satisfaccion").ToString() = "1", "~/Images/sad.png", If(Eval("Satisfaccion").ToString() = "2", "~/Images/confused.png", "~/Images/happy.png")) %>'
                                        Text='<%# If(Eval("Satisfaccion").ToString() = "1", "Mala", If(Eval("Satisfaccion").ToString() = "2", "Regular", "Buena")) %>'
                                        ToolTip='<%# If(Eval("Satisfaccion").ToString() = "1", "Mala", If(Eval("Satisfaccion").ToString() = "2", "Regular", "Buena")) %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Seleccionar" ItemStyle-CssClass="pos-center" ControlStyle-CssClass="pos-center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Seleccionar" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <asp:Panel runat="server" ID="pnlNuevo" Visible="false">
                <div class="spacer"></div>
                <h3 style="float: left; text-align: left; font-size: 22px;">Datos Generales</h3>
                <br />
                <br />
                <div style="width: 100%; clear: both;">
                    <div style="width: 99%; float: left; padding: 15px; margin-top: 5px;" class="pnl-data pnl-border-data">
                        <div style="float: left; width: 10%;">
                            <p style="font-size: 18px;">Proveedor</p>
                        </div>
                        <asp:HiddenField ID="hfId" runat="server" Value="0" />
                        <div style="float: left; width: 50%;">
                            <asp:UpdatePanel ID="upProveedor" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                <ContentTemplate>
                                    <asp:TextBox ID="txtProveedor" runat="server" ReadOnly="true" CssClass="txt-data"></asp:TextBox>
                                    <asp:Button ID="btnProveedorBusqueda" CssClass="btn-Open-Modal txt-data" Text="..." runat="server" OnClientClick="MostrarProveedores()" />
                                    <label style="text-align: left; width: auto; font-size: 18px;" class="lblNombres" id="lblNombreProveedor" runat="server"></label>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div style="float: left; width: 5%;">
                            <p style="font-size: 18px;">Contacto</p>
                        </div>
                        <div style="float: left; width: 25%; margin-left: 15px;">
                            <asp:TextBox ID="txtContacto" runat="server" CssClass="txt-data"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 99%; float: left; padding: 15px;" class="pnl-data pnl-border-data">
                        <asp:UpdatePanel runat="server" ID="upCentroCosto" UpdateMode="Conditional" ChildrenAsTriggers="true">
                            <ContentTemplate>
                                <div style="float: left; width: 10%;">
                                    <p style="font-size: 18px;">Estudio</p>
                                </div>
                                <div style="float: left; width: 20%; border: none;">
                                    <asp:DropDownList runat="server" CssClass="txt-data" ID="ddlTipo" AutoPostBack="true">
                                    </asp:DropDownList>
                                </div>
                                <div style="float: left;" id="divCentroCostos" runat="server" visible="false">
                                    <asp:TextBox ID="txtCC" runat="server" ReadOnly="true" CssClass="txt-data"></asp:TextBox>
                                    <asp:Button ID="btnCCBusqueda" CssClass="btn-Open-Modal txt-data" Text="..." runat="server" OnClientClick="MostrarCC()" />
                                    <label style="font-size: 18px; width: 50%; text-align: left; margin-left: 10px; border: none;" id="txtNombreCCBusqueda" runat="server"></label>
                                    <label style="font-size: 18px; float: right; margin-top: -30px; width: 100%;" id="txtNombreCC" runat="server"></label>
                                </div>
                                <div style="float: left;" id="divJob" runat="server" visible="false">
                                    <label style="font-size: 18px;">Código JBIJBE</label>
                                    <asp:TextBox ID="txtJobBusqueda" runat="server" CssClass="txt-data lblNombres" AutoPostBack="true"></asp:TextBox>
                                    <label style="font-size: 18px;">Nombre JBIJBE</label>
                                    <asp:TextBox ID="txtNombreJobBusqueda" runat="server" CssClass="txt-data lblNombres"></asp:TextBox>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div style="width: 99%; float: left; padding: 15px;" class="pnl-data pnl-border-data">
                        <div style="float: left; width: 20%;">
                            <p style="font-size: 18px;">Nombre Gerente Proyecto</p>
                        </div>
                        <div style="float: left; width: 30%; border: none;">
                            <asp:TextBox ID="txtGerenteProyecto" runat="server" CssClass="txt-data lblNombres" Width="100%"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <br />
                <div style="width: 100%; clear: both;">
                    <div style="width: 99%; float: left; padding: 15px; margin-top: 25px;" class="pnl-data pnl-border-data">
                        <p style="font-size: 18px; text-align: left;">Descripción de la Queja o Reclamo</p>
                        <br />
                        <asp:TextBox ID="txtDescripcionQueja" TextMode="MultiLine" CssClass="textareaWhite txt-data" Text="" runat="server" Rows="6" Width="100%"></asp:TextBox>
                        <br />
                        <p style="font-size: 18px; text-align: left;">Acciones Requeridas Para La Mejora</p>
                        <br />
                        <asp:TextBox ID="txtAccionesRequeridas" TextMode="MultiLine" CssClass="textareaWhite txt-data" Text="" runat="server" Rows="6" Width="100%"></asp:TextBox>
                        <br />
                        <p style="font-size: 18px; text-align: left;">Respuesta y Plan de Acción</p>
                        <br />
                        <div class="pos-izq" style="width: 100%; padding: 10px;">
                            <div style="float: left; width: 50%;">
                                <label style="font-size: 16px; text-align: left;">Causa Raíz</label>
                                <asp:TextBox ID="txtRespuestaCausaRaiz" TextMode="MultiLine" CssClass="textareaWhite pos-izq txt-data" Text="" runat="server" Rows="6" Width="95%"></asp:TextBox>
                            </div>
                            <div style="float: right; width: 50%;">
                                <label style="font-size: 16px; text-align: left;">Plan de acción</label>
                                <asp:TextBox ID="txtRespuestaPlanAccion" TextMode="MultiLine" CssClass="textareaWhite pos-izq txt-data" Text="" runat="server" Rows="6" Width="95%"></asp:TextBox>
                            </div>
                            <br />
                            <div style="float: left; width: 50%;">
                                <label style="font-size: 16px; width: auto; text-align: left;">Responsable</label>
                                <br />
                                <asp:TextBox ID="txtRespuestaResponsable" CssClass="textareaWhite pos-izq txt-data" Text="" runat="server" Width="90%"></asp:TextBox>
                            </div>
                            <div style="float: right; width: 50%;">
                                <label style="font-size: 16px; width: auto; text-align: left;">Fecha de Implementación</label>
                                <br />
                                <asp:TextBox ID="txtRespuestaFechaImp" CssClass="textareaWhite pos-izq txt-data" Text="" runat="server" Width="90%"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <br />
                        <br />
                        <p style="font-size: 18px; text-align: left;">Satisfacción con las acciones implementadas, originadas por la queja o reclamo</p>
                        <br />
                        <asp:DropDownList ID="ddlSatisfaccion" runat="server" CssClass="txt-data" AppendDataBoundItems="true">
                            <asp:ListItem Text="--Seleccione una --" />
                            <asp:ListItem Text="Buena" Value="3" />
                            <asp:ListItem Text="Regular" Value="2" />
                            <asp:ListItem Text="Mala" Value="1" />
                        </asp:DropDownList>
                        <br />
                        <br />
                        <br />
                        <p style="font-size: 18px; text-align: left;">Observaciones</p>
                        <br />
                        <asp:TextBox ID="txtObservaciones" TextMode="MultiLine" CssClass="textareaWhite txt-data" Text="" runat="server" Rows="6" Width="100%"></asp:TextBox>
                        <br />
                        <br />
                        <asp:Button ID="btnGuardar" Text="Guardar" runat="server" />
                    </div>
                </div>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="BusquedaProveedores">
        <asp:UpdatePanel ID="UPanelProveedores" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div style="width: 100%;">
                    <asp:TextBox ID="txtNitProveedor" runat="server" placeholder="NIT o CC" CssClass="inputModal"></asp:TextBox>
                    <asp:TextBox ID="txtNombreProveedor" runat="server" placeholder="Razón Social o Nombre" CssClass="inputModal"></asp:TextBox>
                    <asp:Button ID="btnBuscarProveedor" CssClass="pos-der btnModal" runat="server" Text="Buscar" />
                </div>
                <br />
                <div style="width: 100%; height: 300px; margin: 0px auto; border: 1px solid #000000; border-radius: 5px; overflow-y: scroll;">
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
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="pos-center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Seleccionar" OnClientClick="CerrarProveedores()" />
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
    <div id="BusquedaCC">
        <asp:UpdatePanel ID="UPanel1" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div style="width: 100%;">
                    <asp:TextBox ID="txtCCBusqueda" runat="server" placeholder="Valor" CssClass="inputModal"></asp:TextBox>
                    <asp:Button ID="btnBuscarCC" CssClass="pos-der btnModal" runat="server" Text="Buscar" />
                </div>
                <br />
                <div style="width: 100%; height: 300px; margin: 0px auto; border: 1px solid #000000; border-radius: 5px; overflow-y: scroll;">
                    <asp:GridView ID="gvCC" runat="server" Width="100%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Id" HeaderText="id" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="pos-center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Seleccionar" OnClientClick="CerrarCC()" />
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
