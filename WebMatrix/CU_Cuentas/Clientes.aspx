<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="Clientes.aspx.vb" Inherits="WebMatrix.Clientes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="Control" TagName="Usuarios" Src="~/UsersControl/UsrCtrl_Usuarios.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {
            validationForm();
            $('#<%= btnGuardar.ClientID %>').click(function (evt) {
                var idpais = $('#<%= ddlpais.ClientID %>').val();
                var iddpto = $('#<%= ddldepartamento.ClientID %>').val();
                var idciudad = $('#<%= ddlCiudad.ClientID %>').val();
                var idtipocliente = $('#<%= ddlTipoCliente.ClientID %>').val();
                var idsector = $('#<%= ddlSector.ClientID %>').val();

                if (idpais == '-1') {
                    validarSelect('<%= ddlpais.ClientID %>', "Debe seleccionar un pais");
                }

                if (iddpto == '-1') {
                    validarSelect('<%= ddldepartamento.ClientID %>', "Debe seleccionar un departamento");
                }

                if (idciudad == '-1') {
                    validarSelect('<%= ddlCiudad.ClientID %>', "Debe seleccionar una ciudad");
                }

                if (idtipocliente == '-1') {
                    validarSelect('<%= ddlTipoCliente.ClientID %>', "Debe seleccionar un tipo de cliente");
                }

                if (idsector == '-1') {
                    validarSelect('<%= ddlSector.ClientID %>', "Debe seleccionar un sector");
                }

            });


            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });


        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Clientes</a>
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
            <div id="accordion">
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Lista de Clientes</label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Razón Social
                                    <asp:TextBox ID="txtBuscar" runat="server"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo"  /></label>
                                
                            </fieldset>
                        </div>
                        <div class="actions">
                        </div>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                            PagerStyle-CssClass="headerfooter ui-toolbar" DataKeyNames="Id" 
                            AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Nit" HeaderText="Nit" />
                                <asp:BoundField DataField="RazonSocial" HeaderText="Razon Social" />
                                <asp:BoundField DataField="GrupoEconomico" HeaderText="Grupo Economico" />
                                <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                <asp:BoundField DataField="dpto" HeaderText="Departamento" />
                                <asp:BoundField DataField="ciudad" HeaderText="Ciudad" />
                                <asp:TemplateField HeaderText="Modificar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Modificar" ImageUrl="~/Images/Select_16.png" 
                                            Text="Seleccionar" ToolTip="Modificar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Contactos" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Contactos" ImageUrl="~/Images/Chat-icon.png" 
                                            Text="Seleccionar" ToolTip="Contactos" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
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
                                                <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>- <%= gvDatos.PageCount%>]</span>
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
                    </div>
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Información del Cliente</label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Nit</label>
                                        <asp:HiddenField ID="hfidcliente" runat="server" />
                                        <asp:TextBox ID="txtNit" runat="server" CssClass="required number textEntry"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="fteTxtJobBook" runat="server" FilterType="Numbers"
                                                            TargetControlID="txtNit">
                                                        </asp:FilteredTextBoxExtender>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Dirección</label>
                                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            País</label>
                                        <asp:DropDownList ID="ddlpais" runat="server" AutoPostBack="True" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Tipo Cliente</label>
                                        <asp:DropDownList ID="ddlTipoCliente" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Anticipo (%)</label>
                                        <asp:TextBox ID="txtAnticipo" Text="70" runat="server" CssClass="required number textEntry"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                                            TargetControlID="txtAnticipo">
                                                        </asp:FilteredTextBoxExtender>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Razón Social</label>
                                        <asp:TextBox ID="TxtRazonSocial" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Teléfono</label>
                                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="textEntry"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Departamento</label>
                                        <asp:DropDownList ID="ddldepartamento" runat="server" AutoPostBack="True" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Sector</label>
                                        <asp:DropDownList ID="ddlSector" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Saldo (%)</label>
                                        <asp:TextBox ID="txtSaldo" runat="server" Text="30" CssClass="required number textEntry"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                                            TargetControlID="txtSaldo">
                                                        </asp:FilteredTextBoxExtender>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Grupo Económico</label>
                                        <asp:TextBox ID="txtGrupoEconomico" runat="server" CssClass="textEntry"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Alias</label>
                                        <asp:TextBox ID="txtAlias" runat="server" CssClass="required text textEntry"></asp:TextBox>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Ciudad</label>
                                        <asp:DropDownList ID="ddlCiudad" runat="server" CssClass="dropdowntext">
                                        </asp:DropDownList>
                                    </fieldset>
                                    <fieldset>
                                        <label>
                                            Plazo de pago (días)</label>
                                        <asp:TextBox ID="txtPlazo" runat="server" Text="30" CssClass="required number textEntry"></asp:TextBox>
                                        <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                                            TargetControlID="txtPlazo">
                                                        </asp:FilteredTextBoxExtender>
                                    </fieldset>
                                </div>
                                <div class="actions">
                                </div>
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar"  />
                                            &nbsp;
                                            <input id="Button1" type="button" class="button" value="Cancelar" class="button" 
                                                onclick="location.href='Clientes.aspx';" />
                                        </fieldset>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
                <div id="accordion3">
                    <h3>
                        <a href="#">
                            <label>
                                Detalles del registro</label>
                        </a>
                    </h3>
                    <div class="block">
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
