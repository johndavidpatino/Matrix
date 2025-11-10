<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/INVENTARIO_F.master"
    CodeBehind="EntregaConsumibles.aspx.vb" Inherits="WebMatrix.EntregaConsumibles" %>

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

            $("#<%= txtFecha.ClientID%>").mask("99/99/9999");
            $("#<%= txtFecha.ClientID%>").datepicker({
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
    <a> ARTICULOS CONSUMIBLES</a>
    
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
                                    <asp:DropDownList ID="ddlIdArticulo" runat="server" AutoPostBack="true"></asp:DropDownList>
                                    <label id="lblIdTipoProducto" runat="server" visible="false">Tipo de Producto</label>
                                    <asp:DropDownList ID="ddlIdTipoProducto" runat="server" Visible="false">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Obsequio" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Prueba Producto" Value="2"></asp:ListItem>
                                    </asp:DropDownList>
                                    <label>Sede</label>
                                    <asp:DropDownList ID="ddlIdSede" runat="server"></asp:DropDownList>
                                    &nbsp;&nbsp;
                                    <asp:TextBox ID="txtBusqueda" runat="server" placeholder="Busqueda" Width="240px"></asp:TextBox>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar"/>
                                    &nbsp;&nbsp;
                                    <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                                    </div>
                            </div>
                        </fieldset>
                        <fieldset>
                        <div class="form_right">
                        <asp:GridView ID="gvArticulos" runat="server" Width="100%" AutoGenerateColumns="false" PageSize="50"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="id,IdArticulo" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:TemplateField ShowHeader="false" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblid" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Id" HeaderText="Id" />
                                <asp:BoundField DataField="TipoArticulo" HeaderText="Tipo Articulo" Visible="false"/>
                                <asp:BoundField DataField="Articulo" HeaderText="Articulo" />
                                <asp:BoundField DataField="FechaCompra" HeaderText="Fecha Compra" Visible="false"/>
                                <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" Visible="false"/>
                                <asp:BoundField DataField="UsuarioRegistra" HeaderText="Usuario Registra" Visible="false"/>
                                <asp:BoundField DataField="CentroCosto" HeaderText="Centro de Costo" Visible="false"/>
                                <asp:BoundField DataField="BU" HeaderText="BU" Visible="false"/>
                                <asp:BoundField DataField="JobBook" HeaderText="JobBook" Visible="false"/>
                                <asp:BoundField DataField="JobBookCodigo" HeaderText="JobBookCodigo" Visible="false"/>
                                <asp:BoundField DataField="JobBookNombre" HeaderText="JobBookNombre" Visible="false"/>
                                <asp:BoundField DataField="NumeroCuentaContable" HeaderText="Numero Cuenta Contable" Visible="false"/>
                                <asp:BoundField DataField="CuentaContable" HeaderText="Cuenta Contable" Visible="false"/>
                                <asp:BoundField DataField="Valor" HeaderText="Valor" Visible="false"/>
                                <asp:BoundField DataField="Estado" HeaderText="Estado" Visible="false" />
                                <asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
                                <asp:BoundField DataField="Symphony" HeaderText="Symphony" Visible="false"/>
                                <asp:BoundField DataField="IdFisico" HeaderText="IdFisico" Visible="false"/>
                                <asp:BoundField DataField="Sede" HeaderText="Sede"/>
                                <asp:BoundField DataField="TipoComputador" HeaderText="TipoComputador" Visible="false"/>
                                <asp:BoundField DataField="PertenecePC" HeaderText="Pertenece PC" Visible="false"/>
                                <asp:BoundField DataField="TipoPeriferico" HeaderText="TipoPeriferico" Visible="false"/>
                                <asp:BoundField DataField="Marca" HeaderText="Marca" Visible="false"/>
                                <asp:BoundField DataField="Modelo" HeaderText="Modelo" Visible="false"/>
                                <asp:BoundField DataField="Procesador" HeaderText="Procesador" Visible="false"/>
                                <asp:BoundField DataField="Memoria" HeaderText="Memoria" Visible="false"/>
                                <asp:BoundField DataField="Almacenamiento" HeaderText="Almacenamiento" Visible="false"/>
                                <asp:BoundField DataField="SistemaOperativo" HeaderText="SistemaOperativo" Visible="false"/>
                                <asp:BoundField DataField="Serial" HeaderText="Serial" Visible="false"/>
                                <asp:BoundField DataField="NombreEquipo" HeaderText="Nombre Equipo" Visible="false"/>
                                <asp:BoundField DataField="Office" HeaderText="Office" Visible="false"/>
                                <asp:BoundField DataField="Programas" HeaderText="Programas" Visible="false"/>
                                <asp:BoundField DataField="TipoServidor" HeaderText="TipoServidor" Visible="false"/>
                                <asp:BoundField DataField="Raid" HeaderText="Raid" Visible="false"/>
                                <asp:BoundField DataField="IdTablet" HeaderText="IdTablet" Visible="false"/>
                                <asp:BoundField DataField="IdSTG" HeaderText="IdSTG" Visible="false"/>
                                <asp:BoundField DataField="TamanoPantalla" HeaderText="Tamaño Pantalla" Visible="false"/>
                                <asp:BoundField DataField="Chip" HeaderText="Chip" Visible="false"/>
                                <asp:BoundField DataField="IMEI" HeaderText="IMEI" Visible="false"/>
                                <asp:BoundField DataField="Pertenece" HeaderText="Pertenece Tablet" Visible="false"/>
                                <asp:BoundField DataField="Operador" HeaderText="Operador" Visible="false"/>
                                <asp:BoundField DataField="NumeroCelular" HeaderText="Numero Celular" Visible="false"/>
                                <asp:BoundField DataField="CantidadMinutos" HeaderText="Cantidad Minutos" Visible="false"/>
                                <asp:BoundField DataField="TipoProducto" HeaderText="Tipo Producto" Visible="false"/>
                                <asp:BoundField DataField="Producto" HeaderText="Producto" Visible="false"/>
                                <asp:BoundField DataField="TipoObsequio" HeaderText="Tipo Obsequio" Visible="false"/>
                                <asp:BoundField DataField="TipoBono" HeaderText="Tipo Bono" Visible="false"/>
                                <asp:BoundField DataField="Asignado" HeaderText="Asignado" Visible="false"/>
                                <asp:BoundField DataField="UsuarioAsignado" HeaderText="Usuario Asignado" Visible="false"/>
                                <asp:BoundField DataField="FechaAsignacion" HeaderText="Fecha Asignacion" Visible="false"/>
                                <asp:BoundField DataField="ObservacionAsignacion" HeaderText="Observacion Asignacion" Visible="false"/>
                                <asp:TemplateField HeaderText="Entregar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnAsignar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Asignar" ImageUrl="~/Images/Select_16.png" Text="Asignar"
                                            ToolTip="Asignar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Movimientos" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnStock" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Stock" ImageUrl="~/Images/Select_16.png" Text="Stock"
                                            ToolTip="Stock" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                            </Columns>                        

                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvArticulos.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvArticulos.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td><span class="pagingLinks">[<%= gvArticulos.PageIndex + 1%>-<%= gvArticulos.PageCount%>]</span> </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvArticulos.PageIndex + 1) = gvArticulos.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvArticulos.PageIndex + 1) = gvArticulos.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
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
                <div id="accordion1">
                    <h3>  <a href="#">
                        <label>
                           ENTREGAR UN ARTICULO CONSUMIBLE
                        </label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <asp:HiddenField ID="hfIdArticulo" runat="server" Value="0" />
                                <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
                                <asp:HiddenField ID="hfIdTrabajo" runat="server" Value="0" />
                                <asp:HiddenField ID="hfIdCargo" runat="server" Value="0" />
                                <asp:label ID="lblAsignar" runat="server" Text="Artículo:" Visible="false" ></asp:label>
                                <asp:label ID="lblIdAsignar" runat="server" Visible="false" ></asp:label>
                                &nbsp;
                                <asp:label ID="lblArticulo" runat="server" Visible="false" ></asp:label>
                            </div>
                            <div style="margin-top: 0px">
                            <label id="lblNumeroEntrega" runat="server" visible="false">Número Entrega</label>
                            <asp:TextBox ID="txtNumeroEntrega" runat="server" CssClass="textEntry" Visible="false" Enabled="false"></asp:TextBox>
                            
                            <label>Fecha</label>
                            <asp:TextBox ID="txtFecha" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <label>Tipo de Movimiento</label>
                            <asp:DropDownList ID="ddlTipo" runat="server" AutoPostBack="true"></asp:DropDownList>
                            &nbsp;
                            <asp:label ID="lblDisponible" runat="server" Text="Disponible:" Visible="false" ></asp:label>
                            &nbsp;<asp:label ID="lblIdDisponible" runat="server" Visible="false" ></asp:label>
                            <label id="lblVale" runat="server" visible="false">Número Vale Taxi</label>
                            <asp:TextBox ID="txtVale" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <label id="lblEstado" runat="server" visible="false">Estado Producto</label>
                            <asp:DropDownList ID="ddlEstado" runat="server" Visible="false"></asp:DropDownList>
                            <label id="lblCentroCosto" runat="server">Centro de Costo</label>
                            <asp:DropDownList ID="ddlCentroCosto" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="JBE - JobBookExterno" Value="1"></asp:ListItem>
                            <asp:ListItem Text="JBI - JobBookInterno" Value="2"></asp:ListItem>    
                            <asp:ListItem Text="BU - Business Unit" Value="3"></asp:ListItem>    
                            </asp:DropDownList>
                            <label id="lblBU" runat="server" visible="false">Unidad de Negocio</label>
                            <asp:DropDownList ID="ddlBU" runat="server" Visible="false"></asp:DropDownList>
                            <label id="lblJBIJBE" runat="server" visible="false">Codigo JBIJBE</label>
                            <asp:TextBox ID="txtJBIJBE" runat="server" Visible="false" AutoPostBack="true" Width="242px"></asp:TextBox>
                            <label id="lblNombreJBIJBE" runat="server" visible="false">Nombre JBIJBE</label>
                            <asp:TextBox ID="txtNombreJBIJBE" runat="server" Visible="false" Width="242px"></asp:TextBox>
                            <label id="lblCuentasContables" runat="server">Cuenta Contable</label>
                            <asp:DropDownList ID="ddlCuentasContables" runat="server" Enabled="false"></asp:DropDownList>
                            <asp:Button ID="btnCuentasContables" Text="..." runat="server" Width="25px" OnClientClick="MostrarCuentasContables()" /><br />
                            <label>Ciudad</label>
                            <asp:DropDownList ID="ddlCiudad" runat="server"></asp:DropDownList> 
                            <label id="lblValor" runat="server"></label>
                            <asp:TextBox ID="txtValor" runat="server" CssClass="textEntry" ></asp:TextBox>
                            <label>Usuario Asignado</label>
                            <asp:TextBox ID="txtUsuario" runat="server" ReadOnly="true" Width="242px"></asp:TextBox>
                            <asp:Button ID="btnSearchUsuario" Text="..." runat="server" OnClientClick="MostrarUsuarios()" /><br /> 
                            <label>Cargo</label>
                            <asp:TextBox ID="txtCargo" runat="server" ReadOnly="true" Width="244px"></asp:TextBox>
                            <label>Observación</label>
                            <asp:TextBox ID="txtObservacion" runat="server" CssClass="textEntry" Width="480px"></asp:TextBox>
                                <br />
                            <asp:Button ID="btnGuardar" runat="server" CssClass="causesValidation" Text="Guardar" />
                            &nbsp;
                           <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                       </div>
                      </fieldset>
                    </div>
                 </div>
                <div id="accordion2">
                    <h3><a href="#">
                        <label>
                        MOVIMIENTOS DEL STOCK
                        </label>
                        </a></h3>
                    <div class="block">
                        <fieldset>
                        <div>
                            <asp:label ID="lblStock" runat="server" Text="Artículo:" Visible="false" ></asp:label>
                            <asp:label ID="lblIdStock" runat="server" Visible="false" ></asp:label>
                            &nbsp;
                            <asp:label ID="lblConsumible" runat="server" Visible="false" ></asp:label>
                            <br />
                            <asp:Button ID="btnExportarSC" runat="server" Text="Exportar" />
                            <br /><br />
                        </div>
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
                                <asp:BoundField DataField="IdConsumible" HeaderText="Id" />
                                <asp:BoundField DataField="Articulo" HeaderText="Articulo" visible="false"/>
                                <asp:BoundField DataField="TipoProducto" HeaderText="Tipo Producto" visible="false"/>
                                <asp:BoundField DataField="Producto" HeaderText="Producto" visible="false"/>
                                <asp:BoundField DataField="TipoObsequio" HeaderText="Tipo Obsequio" Visible="false"/>
                                <asp:BoundField DataField="EstadoProducto" HeaderText="Estado" Visible="false"/>
                                <asp:BoundField DataField="TipoBono" HeaderText="Tipo Bono" visible="false"/>
                                <asp:BoundField DataField="NumeroVale" HeaderText="Numero Vale" Visible="false"/>
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha"/>
                                <asp:BoundField DataField="BU" HeaderText="BU" />
                                <asp:BoundField DataField="JobBookCodigo" HeaderText="JobBook" />
                                <asp:BoundField DataField="JobBookNombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad"/>
                                <asp:BoundField DataField="UsuarioAsignado" HeaderText="Usuario Asignado"/>
                                <asp:BoundField DataField="TipoMovimiento" HeaderText="Tipo Movimiento"/>
                                <asp:BoundField DataField="Valor" HeaderText="Entregado" />
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
