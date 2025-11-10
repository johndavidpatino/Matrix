<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="GD_SeguimientoPNC.aspx.vb" Inherits="WebMatrix.GD_SeguimientoPNC" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
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
    <asp:UpdatePanel ID="upPNC" runat="server">
        <ContentTemplate>
            <div class="block">
                <div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Estado:
                            </label>
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="mySpecificClass dropdowntext">
                                <asp:ListItem Text="Seleccione" Value="-1"></asp:ListItem>
                                <asp:ListItem Text="Cerrado" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No tiene causas" Value="2"></asp:ListItem>
                                <asp:ListItem Text="No tiene acciones" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Gestionado " Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="actions">
                        <div class="form_right">
                            <fieldset>
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </fieldset>
                            <fieldset>
                                <asp:Button ID="btnExportar" runat="server" Text="Exportar" />
                            </fieldset>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upGVLista" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:GridView ID="gvLista" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="NombreEstudio" HeaderText="NombreEstudio" />
                    <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                    <asp:BoundField DataField="FechaReclamo" HeaderText="FechaReclamo" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="Reporta" HeaderText="Reporta" />
                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" />

                    <asp:BoundField DataField="FuenteReclamo" HeaderText="FuenteReclamo" />
                    <asp:BoundField DataField="Categoria" HeaderText="Categoria" />
                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                    <%--<asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />--%>
                    <asp:BoundField DataField="CantidadCausas" HeaderText="CantidadCausas" />
                    <asp:BoundField DataField="CantidadAcciones" HeaderText="CantidadAcciones" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                </Columns>
                <PagerTemplate>
                    <div class="pagingButtons">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                        SkinID="Paging">« Primero</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                        SkinID="paging">&lt; Anterior</asp:LinkButton>
                                </td>
                                <td>
                                    <span class="pagingLinks">[<asp:Label ID="lblPaginaActual" runat="server"></asp:Label>-<asp:Label ID="lblCantidadPaginas" runat="server"></asp:Label>]</span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
