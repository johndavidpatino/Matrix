<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/INVENTARIO_F.master"
    CodeBehind="ReporteRemanente.aspx.vb" Inherits="WebMatrix.ReporteRemanente" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
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

            validationForm();

        }

        $(document).ready(function () {
            loadPlugins();

            $('#BusquedaUsuarios').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Usuarios",
                width: "600px"
            });

            $('#BusquedaUsuarios').parent().appendTo("form");

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
                title: "Cuentas Contables",
                width: "600px"
            });

            $('#BusquedaCuentasContables').parent().appendTo("form");

           $('#CargaArchivos').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Carga archivos",
                width: "600px"
            });
        });

        
        function MostrarUsuarios() {
            $('#BusquedaUsuarios').dialog("open");
        }

        function CerrarUsuarios() {
            $('#BusquedaUsuarios').dialog("close");
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
        

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a> REMANENTE ARTICULOS CONSUMIBLES</a>
    
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
    <asp:LinkButton ID="lbtnVolver" Text ="Volver" href="../Inventario/RegistroArticulos.aspx" runat="server"></asp:LinkButton>
    <br />
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            
            <div id="accordion">

                <div id="accordion0">
                    <h3><a href="#">
                        <label>
                        BUSQUEDA DE ARTICULOS CONSUMIBLES
                        </label>
                        </a></h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                           <div class="actions">
                                <div class="form_rigth">
                                    <label>Artículo</label>
                                    <asp:DropDownList ID="ddlIdArticulo" runat="server" AutoPostBack="true">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Productos" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="Bonos" Value="8"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label id="lblIdTipoProducto" runat="server" visible="false">Tipo de Producto</label>
                                    <asp:DropDownList ID="ddlIdTipoProducto" runat="server" Visible="false">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Obsequio" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Prueba Producto" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label>JobBook</label>
                                    <asp:TextBox ID="txtBusqueda" runat="server" Width="240px"></asp:TextBox>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar"/>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                                    </div>
                            </div>
                        </fieldset>
                        <fieldset>
                        <div class="form_right">
                             <asp:GridView ID="gvStock" runat="server" Width="100%" AutoGenerateColumns="false" PageSize="50"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:TemplateField ShowHeader="false" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="IdConsumible" HeaderText="Id Consumible" />
                                <asp:BoundField DataField="Articulo" HeaderText="Articulo" visible="false"/>
                                <asp:BoundField DataField="TipoProducto" HeaderText="Tipo Producto" visible="false"/>
                                <asp:BoundField DataField="Producto" HeaderText="Producto" visible="false"/>
                                <asp:BoundField DataField="TipoObsequio" HeaderText="Tipo Obsequio" Visible="false"/>
                                <asp:BoundField DataField="EstadoProducto" HeaderText="Estado" Visible="false"/>
                                <asp:BoundField DataField="TipoBono" HeaderText="Tipo Bono" visible="false"/>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="JobBookCodigo" HeaderText="JobBook" />
                                <asp:BoundField DataField="JobBookNombre" HeaderText="JobBookNombre" />
                                <asp:BoundField DataField="Total" HeaderText="Total" />
                                <asp:BoundField DataField="Disponible" HeaderText="Disponible"/>
                            </Columns>                        
                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvStock.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvStock.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td><span class="pagingLinks">[<%= gvStock.PageIndex + 1%>-<%= gvStock.PageCount%>]</span> </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvStock.PageIndex + 1) = gvStock.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvStock.PageIndex + 1) = gvStock.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                        </div>
                    </fieldset>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="BusquedaUsuarios">
        <asp:UpdatePanel ID="UPanelUsuarios" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <asp:TextBox ID="txtCedulaUsuario" runat="server" placeholder="Cedula"></asp:TextBox>
                <asp:TextBox ID="txtNombreUsuario" runat="server" placeholder="Nombre"></asp:TextBox>
                <asp:Button ID="btnBuscarUsuario" runat="server" Text="Buscar" />
                <div class="actions"></div>
                <div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
                    <asp:GridView ID="gvUsuarios" runat="server" Width="90%" AutoGenerateColumns="False"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                            <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                            <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarUsuarios()" />
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
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
