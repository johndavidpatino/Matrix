<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="Contactos.aspx.vb" Inherits="WebMatrix.Contactos" %>

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


            });


            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });


        }

        function redireccion() {
            var idcliente = $('#<%= hfidcliente.ClientID %>').val();
            document.location.href = 'Contactos.aspx?idcliente=' + idcliente;

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
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
                                Lista de Contactos</label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="tittleOption">
                           
                                <div class="form_left">
                                    <label> CLIENTE:
                                    <asp:LinkButton ID="lnkCliente" runat="server" ForeColor="Black"></label></asp:LinkButton>
                                </div>
                            
                        </div>
                            <div class="actions">
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Nombre</label>
                                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:HiddenField ID="hfidcliente" runat="server" />
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar"  />
                                <asp:Button ID="btnNuevo" runat="server" Text="Nuevo"  />
                            </fieldset>
                        </div>
                        <div class="actions">
                        </div>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Id" AllowPaging="True" 
                            EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                                <asp:BoundField DataField="Celular" HeaderText="Celular" />
                                <asp:BoundField DataField="Email" HeaderText="Email" />
                                <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
                                <asp:TemplateField HeaderText="Modificar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Modificar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                            ToolTip="Modificar" />
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
                                                <span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-
                                                    <%= gvDatos.PageCount%>]</span>
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
                                Información del Contacto</label>
                        </a>
                    </h3>
                    <div class="block">
                        <fieldset class="validationGroup">
                            <div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Nombre</label>
                                        <asp:HiddenField ID="hfidcontacto" runat="server" />
                                        <asp:TextBox ID="txtNombre" runat="server" CssClass="required textEntry"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Teléfono</label>
                                        <asp:TextBox ID="txtTelefono" runat="server"  CssClass="required text textEntry"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Celular</label>
                                        <asp:TextBox ID="txtCelular" runat="server" CssClass="textEntry"></asp:TextBox>
                                    </fieldset>
                                </div>
                                  <div class="actions">
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Email</label>
                                        <asp:TextBox ID="txtEmail" runat="server"  CssClass="required email text textEntry"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Cargo</label>
                                        <asp:TextBox ID="txtCargo" runat="server" CssClass="textEntry"></asp:TextBox>
                                    </fieldset>
                                </div>
                                <div class="form_left">
                                    <fieldset>
                                        <label>
                                            Activo</label>
                                        <asp:CheckBox ID="chkActivo" runat="server" Checked="True" />
                                    </fieldset>
                                </div>
                                <div class="actions">
                                </div>
                                <div class="actions">
                                    <div class="form_right">
                                        <fieldset>
                                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CommandName="Guardar"  />
                                            &nbsp;
                                            <input id="Button1" type="button" class="button" value="Cancelar" 
                                                style="font-size: 11px;" onclick="redireccion();" />
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
