<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/INVENTARIO_F.master"
    CodeBehind="ReporteLegalizaciones.aspx.vb" Inherits="WebMatrix.ReporteLegalizaciones" %>

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

            $("#<%= txtfechainicio.ClientID%>").mask("99/99/9999");
            $("#<%= txtfechainicio.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtfechafin.ClientID%>").mask("99/99/9999");
            $("#<%= txtfechafin.ClientID%>").datepicker({
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
    <a>REPORTE DE LEGALIZACIONES</a>
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
                    <h3><a href="#">Reporte de Legalizaciones</a></h3>
                           
            <div class="block">
               <div class="form_left">
                    <fieldset>
                        <label>Fecha Inicio</label>
                        <asp:TextBox ID="txtfechainicio" runat="server" CssClass="textEntry"></asp:TextBox>
                        <label>Usuario Asignado</label>
                        <asp:TextBox ID="txtIdUsuario" runat="server" Width="201px"></asp:TextBox>
                        <label>Artículo</label>
                        <asp:DropDownList ID="ddlIdArticulo" runat="server">
                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Obsequios" Value="7"></asp:ListItem>
                        <asp:ListItem Text="Bonos" Value="8"></asp:ListItem>
                        <asp:ListItem Text="Vale Taxi" Value="9"></asp:ListItem>
                        </asp:DropDownList>
                        <label>Busqueda</label>
                        <asp:TextBox ID="txtTodosCampos" runat="server" CssClass="textEntry" Width="185px"></asp:TextBox>
                    </fieldset>
                </div>
                <div class="form_left">
                    <fieldset>
                        <label>Fecha Fin</label>
                        <asp:TextBox ID="txtfechafin" runat="server" CssClass="textEntry"></asp:TextBox>
                        <label>Centro Costo</label>
                        <asp:DropDownList ID="ddlCentroCosto" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="JBE - JobBookExterno" Value="1"></asp:ListItem>
                        <asp:ListItem Text="JBI - JobBookInterno" Value="2"></asp:ListItem>    
                        <asp:ListItem Text="BU - Business Unit" Value="3"></asp:ListItem>    
                        </asp:DropDownList>
                        <label id="lblBU" runat="server" visible="false">Unidad de Negocio</label>
                        <asp:DropDownList ID="ddlBU" runat="server" visible="false"></asp:DropDownList>
                        <label id="lblJBIJBE" runat="server" visible="false">Codigo JBIJBE</label>
                        <asp:TextBox ID="txtJBIJBE" runat="server"  Width="200px" AutoPostBack="true" visible="false"></asp:TextBox>
                        <label id="lblNombreJBIJBE" runat="server" visible="false">Nombre JBIJBE</label>
                        <asp:TextBox ID="txtNombreJBIJBE" runat="server"  Width="200px" visible="false"></asp:TextBox>
                    </fieldset>
                </div>
                <div class="actions">
                   <div class="form_rigth">
                      <fieldset>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Height="22px"/>
                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                        <br />
                      </fieldset>
                    </div>
                </div>
                <div class="form_right">
                    <fieldset>
                    <asp:HiddenField ID="hfJobBook" runat="server" Value="0" />
                    <asp:GridView ID="gvLegalizaciones" runat="server" Width="100%" AutoGenerateColumns="false" PageSize="50"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id,IdConsumible,IdArticulo,IdTipoLegalizacion,UsuarioRegistra,UsuarioResponsable,IdCentroCosto,JobBook,JobBookCodigo,IdBU,Pendiente,IdLegalizado" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
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
                        <asp:BoundField DataField="IdConsumible" HeaderText="IdConsumible" />
                        <asp:BoundField DataField="Articulo" HeaderText="Articulo"/>
                        <asp:BoundField DataField="TipoLegalizacion" HeaderText="Tipo Legalizacion"/>
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="UsuarioAsignado" HeaderText="Usuario Asignado"/>
                        <asp:BoundField DataField="JobBookCodigo" HeaderText="JobBook"/>
                        <asp:BoundField DataField="JobBookNombre" HeaderText="JobBook Nombre"/>
                        <asp:BoundField DataField="Unidades" HeaderText="Unidades" Visible="false"/>
                        <asp:BoundField DataField="ValorCarrera" HeaderText="Valor Carrera" Visible="false"/>
                        <asp:BoundField DataField="ValorEntregado" HeaderText="Valor Entregado"/>
                        <asp:BoundField DataField="Firmas" HeaderText="Firmas" Visible="false"/>
                        <asp:BoundField DataField="Devoluciones" HeaderText="Devoluciones" Visible="false"/>
                        <asp:BoundField DataField="NotasCredito" HeaderText="Notas Credito" Visible="false"/>
                        <asp:BoundField DataField="DescuentoNomina" HeaderText="Descuento Nomina" Visible="false"/>
                        <asp:BoundField DataField="ValorLegalizado" HeaderText="Valor Legalizado"/>
                        <asp:BoundField DataField="Pendiente" HeaderText="Pendiente" />
                        <asp:BoundField DataField="Observaciones" HeaderText="Observaciones"/>
                        <asp:BoundField DataField="Legalizado" HeaderText="Legalizado"/>
                    </Columns>                        

                    <PagerTemplate>
                        <div class="pagingButtons">
                            <table>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page" Enabled='<%# IIf(gvLegalizaciones.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page" Enabled='<%# IIf(gvLegalizaciones.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td><span class="pagingLinks">[<%= gvLegalizaciones.PageIndex + 1%>-<%= gvLegalizaciones.PageCount%>]</span> </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page" Enabled='<%# IIf((gvLegalizaciones.PageIndex + 1) = gvLegalizaciones.PageCount, "false", "true")%>' SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page" Enabled='<%# IIf((gvLegalizaciones.PageIndex + 1) = gvLegalizaciones.PageCount, "false", "true")%>' SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
                </fieldset>
                </div>
                

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
    
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
