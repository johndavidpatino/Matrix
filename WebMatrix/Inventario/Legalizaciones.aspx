<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/INVENTARIO_F.master"
    CodeBehind="Legalizaciones.aspx.vb" Inherits="WebMatrix.Legalizaciones" %>

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
    <a>LEGALIZACIONES DE ARTÍCULOS CONSUMIBLES</a>
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
                        LISTA DE ARTÍCULOS CONSUMIBLES
                        </label>
                        </a></h3>
                    <div class="block">
                        <fieldset>
                                <div class="actions">
                                    
                                    <div class="form_rigth">
                                        <asp:HiddenField ID="hfIdUsuario" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfIdUsuarioReg" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfIdConsumible" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfIdArticulo" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfCentroCosto" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfBU" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfJobBook" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfJobBookCodigo" runat="server" Value="0" />
                                        <asp:HiddenField ID="hfJobBookNombre" runat="server" Value="0" />
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
                                        <label>Artículo</label>
                                        <asp:DropDownList ID="ddlIdArticulo" runat="server">
                                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                                        <asp:ListItem Text="Obsequios" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="Bonos" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="Vale Taxi" Value="9"></asp:ListItem>
                                        </asp:DropDownList>
                                        <label>Usuario Asignado</label>
                                        <asp:TextBox ID="txtIdUsuario" runat="server" Width="201px"></asp:TextBox>
                                        <label>Id Consumible</label>
                                        <asp:TextBox ID="txtIdConsumible" runat="server" Width="201px"></asp:TextBox>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnBuscar" runat="server" Text="Buscar" Height="22px"/>
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                                        <br />
                                    </div>
                            </div>
                        </fieldset>
                        <fieldset>
                        <div class="form_right">
                           <asp:GridView ID="gvStock" runat="server" Width="100%" AutoGenerateColumns="false" PageSize="50"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="IdConsumible,IdArticulo,UsuarioRegistra,UsuarioAsignado,IdCentroCosto,JobBook,IdBU,Pendiente" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="IdConsumible" HeaderText="IdConsumible" />
                                <asp:BoundField DataField="Articulo" HeaderText="Articulo"/>
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario"/>
                                <asp:BoundField DataField="CentroCosto" HeaderText="Centro Costo"/>
                                <asp:BoundField DataField="JobBookCodigo" HeaderText="JobBookCodigo"/>
                                <asp:BoundField DataField="JobBookNombre" HeaderText="JobBookNombre"/>
                                <asp:BoundField DataField="BU" HeaderText="BU"/>
                                <asp:BoundField DataField="TotalEntregado" HeaderText="Total Entregado"/>
                                <asp:BoundField DataField="TotalFirmas" HeaderText="Total Firmas"/>
                                <asp:BoundField DataField="TotalDevoluciones" HeaderText="Total Devoluciones"/>
                                <asp:BoundField DataField="TotalNotasCredito" HeaderText="Total Notas Crédito"/>
                                <asp:BoundField DataField="TotalDescuentoNomina" HeaderText="Total Descuento Nómina"/>
                                <asp:BoundField DataField="TotalLegalizado" HeaderText="Total Legalizado"/>
                                <asp:BoundField DataField="Pendiente" HeaderText="Pendiente"/>
                                <asp:BoundField DataField="Estado" HeaderText="Estado"/>
                                <asp:TemplateField HeaderText="Legalizar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnLegalizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Legalizar" ImageUrl="~/Images/Select_16.png" Text="Legalizar"
                                            ToolTip="Legalizar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Detalle" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnDetalle" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Detalle" ImageUrl="~/Images/Select_16.png" Text="Detalle"
                                            ToolTip="Detalle" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>           
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

                <div id="accordion1">
                    <h3>  <a href="#">
                        <label>
                           PROCESO DE LEGALIZACIÓN
                        </label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset>
                            <div>
                                <asp:label ID="lblActualizar" runat="server" Text="Se esta actualizando el Id:" Visible="false" ></asp:label>
                                <asp:label ID="lblIdActualizar" runat="server" Visible="false" ></asp:label>
                                &nbsp;
                                <asp:label ID="lblLegalizar" runat="server" Text="Se esta legalizando el Id:" Visible="false" ></asp:label>
                                <asp:label ID="lblIdLegalizar" runat="server" Visible="false" ></asp:label>
                                &nbsp;
                                <asp:label ID="lblArticulo" runat="server" Visible="false" ></asp:label>
                                <br />
                                <asp:label ID="lblUsuario" runat="server" Text="Usuario Asignado:" Visible="false" ></asp:label>
                                &nbsp;
                                <asp:label ID="lblNombreUsuario" runat="server" Visible="false" ></asp:label>
                                <br />
                                <asp:label ID="lblCentroCosto" runat="server" Visible="false" ></asp:label>
                                &nbsp;
                                <asp:label ID="lblBUJBI" runat="server" Visible="false" ></asp:label>
                             </div>
                        <div>
                            <asp:label id="lblVale" runat="server" visible="false">Número Vale</asp:label>
                            &nbsp;<asp:label id="lblIdVale" runat="server" visible="false"></asp:label>
                            <label id="lblRadicado" runat="server" visible="false">Número Radicado</label>
                            <asp:TextBox ID="txtRadicado" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <label>Fecha Legalización</label>
                            <asp:TextBox ID="txtFecha" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <label>Nombre Responsable</label>
                            <asp:TextBox ID="txtNombre" runat="server" CssClass="textEntry" ReadOnly="true" Width="240px"></asp:TextBox>
                            <label id="lblUnidades" runat="server" visible="false">Unidades</label>
                            <asp:TextBox ID="txtUnidades" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <label id="lblValorCarrera" runat="server" visible="false">Valor Carrera</label>
                            <asp:TextBox ID="txtValorCarrera" runat="server" CssClass="textEntry" Visible="false"></asp:TextBox>
                            <label id="lblfirmas" runat="server" visible="false">Firmas</label>
                            <asp:TextBox ID="txtFirmas" runat="server" CssClass="textEntry" Width="152px" visible="false"></asp:TextBox>
                            <label id="lblDevoluciones" runat="server" visible="false">Devoluciones</label>
                            <asp:TextBox ID="txtDevoluciones" runat="server" CssClass="textEntry" Width="152px" visible="false"></asp:TextBox>
                            <label id="lblNotasCredito" runat="server" visible="false">Notas Crédito</label>
                            <asp:TextBox ID="txtNotasCredito" runat="server" CssClass="textEntry" Width="152px" visible="false"></asp:TextBox>
                            <label id="lblDescuentoNomina" runat="server" visible="false">Descuentos Nómina</label>
                            <asp:TextBox ID="txtDescuentoNomina" runat="server" CssClass="textEntry" Width="152px" visible="false"></asp:TextBox>        
                            <label id="lbllegalizado" runat="server" visible="false">Legalizado</label>
                            <asp:TextBox ID="txtLegalizado" runat="server" CssClass="textEntry" Width="152px" Visible="false"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:label id="lblPendiente" runat="server" visible="false">Valor Pendiente:</asp:label>
                            &nbsp;<asp:label id="lblIdPendiente" runat="server" visible="false"></asp:label>
                            <label>Observaciones</label>
                            <asp:TextBox ID="txtObservaciones" runat="server" CssClass="textEntry" Width="480px"></asp:TextBox>
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
                        DETALLE DE LA LEGALIZACIÓN
                        </label>
                        </a></h3>
                    <div class="block">
                        <fieldset>
                            <div>
                            <asp:Button ID="btnExportarSC" runat="server" Text="Exportar" />
                            <br /><br />
                        </div>
                        <div class="form_right">
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
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha"
                                    DataFormatString="{0:dd/MM/yyyy}"/>
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario"/>
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
                                <asp:BoundField DataField="Verificado" HeaderText="Verificado" Visible="false"/>
                                <asp:BoundField DataField="FechaVerificacion" HeaderText="Fecha Verificacion"
                                    DataFormatString="{0:dd/MM/yyyy}" Visible="false"/>
                                <asp:BoundField DataField="UsuarioVerifica" HeaderText="Usuario Verifica" Visible="false"/>
                                <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                            ToolTip="Actualizar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgBtnEliminar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Eliminar" ImageUrl="~/Images/delete_16.png" Text="Eliminar"
                                            ToolTip="Eliminar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Verificar">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkVerificar" runat="server" Checked='<%# IIf(Eval("IdVerificado") = False, "false", "true")%>'
                                        AutoPostBack="true" OnCheckedChanged="chkVerificar_CheckedChanged" />
                                </ItemTemplate>
                                </asp:TemplateField>           
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
    
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
