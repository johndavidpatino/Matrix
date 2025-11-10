<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ListadoCuentasRecibidas.aspx.vb" Inherits="WebMatrix.ListadoCuentasRecibidas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
     <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
        function loadPlugins() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            validationForm();

        }
        $(document).ready(function () {
            loadPlugins();
        });
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
    <asp:UpdatePanel ID="UpPanel100" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hfNuevo" runat="server" Value="0" />
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
                <div id="accordion0">
                    <h3><a href="#">Cuentas de Cobro Radicadas</a></h3>
                    <div class="block">
                        <div class="actions">
                            <div class="form_left">
                                <label>Búsqueda por</label>
                                <asp:HiddenField ID="hfidorden" runat="server" />
                                <asp:TextBox ID="txtIdentificacionBuscar" runat="server" placeholder="Identificacion"></asp:TextBox>
                                <asp:TextBox ID="txtOrdenBuscar" runat="server" placeholder="OrdenId"></asp:TextBox>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </div>
                            <div class="form_left">
                                <fieldset >
                                
                                    </fieldset>
                            </div>
                        </div>
                        <asp:GridView ID="GvListadoCuentas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="OrdenId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="OrdenId" HeaderText="OrdenID" />
                                <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="TipoDocumento" HeaderText="TipoDocumento" />
                                <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
                                <asp:BoundField DataField="Consecutivo" HeaderText="Consecutivo" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="VrUnitario" HeaderText="VrUnitario" />
                                <asp:BoundField DataField="VrTotal" HeaderText="VrTotal"/>
                                <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                
                                
                                <asp:TemplateField HeaderText="Detalle" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgDetalle" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Detalle" ImageUrl="~/Images/Select_16.png" Text="Detalle"
                                            ToolTip="Detalle" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                  <ItemTemplate>
                                        <asp:ImageButton ID="ImgEliminar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Eliminar" ImageUrl="~/Images/delete_12.png" Text="Eliminar"
                                            ToolTip="<>Eliminar" />
                                    </ItemTemplate>
                                      </asp:TemplateField>
                            </Columns>
                            <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIf(GvListadoCuentas.PageIndex = 0, "false", "true")%>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIf(GvListadoCuentas.PageIndex = 0, "false", "true")%>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= GvListadoCuentas.PageIndex + 1%>-<%= GvListadoCuentas.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIf((GvListadoCuentas.PageIndex + 1) = GvListadoCuentas.PageCount, "false", "true")%>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIf((GvListadoCuentas.PageIndex + 1) = GvListadoCuentas.PageCount, "false", "true")%>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                    <div id="accordion1">
                        <h3>
                            <a href="#">Detalle Cuenta de Cobro
                            </a>
                        </h3>
                        <div class="block">
                           <fieldset>
                                    <label>
                                       Orden de Servicio</label>
                                    
                                </fieldset>

                       
                    <asp:GridView ID="GvDetalleCuenta" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="OrdenId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="OrdenId" HeaderText="OrdenID" />
                                <asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="TipoDocumento" HeaderText="TipoDocumento" />
                                <asp:BoundField DataField="Consecutivo" HeaderText="Consecutivo" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                <asp:BoundField DataField="VrUnitario" HeaderText="VrUnitario" />
                                <asp:BoundField DataField="VrTotal" HeaderText="VrTotal"/>
                                <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                
                            </Columns> 
                            </asp:GridView> 
                            
                        </fieldset>
                          <div class="actions">
                              <label >Seleccione Estado</label>
                              <asp:DropDownList ID="ddlestado" runat ="server" ></asp:DropDownList>
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
                              
                                
                            </div>
                        </div>
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
