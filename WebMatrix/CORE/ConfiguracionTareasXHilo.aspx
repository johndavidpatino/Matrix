<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="ConfiguracionTareasXHilo.aspx.vb" Inherits="WebMatrix.ConfiguracionTareasXHilo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">

        function loadPlugins() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $('#AsignarNuevoDocumento').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Grabar",
                width: "600px",
                buttons: {
                    Ok: function () {
                        $(this).dialog("close");
                    }
                }
            });
            $('#AsignarNuevoDocumento').parent().appendTo("form");
        }

        $(document).ready(function () {
            loadPlugins();
        });

        function AsignarNuevoDocumento(idDocumento) {
            $('#AsignarNuevoDocumento').dialog("open");
            $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIdDocumentoNoAsignado').val(idDocumento);
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
    <asp:Label ID="lblTiposHilos" runat="server" Text="Tipos de hilo"></asp:Label>
    <asp:DropDownList ID="ddlTiposHilos" runat="server" AutoPostBack="True">
    </asp:DropDownList>
    <div id="accordion">
        <div id="accordion0">
            <h3>
                <a href="#">
                    <label>
                        Asignadas
                    </label>
                </a>
            </h3>
            <div class="block">
                <asp:GridView ID="gvTareasAsignadas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                        <asp:BoundField DataField="NoEmpiezaAntesDe" HeaderText="NoEmpiezaAntesDe" />
                        <asp:BoundField DataField="NoTerminaAntesDe" HeaderText="NoTerminaAntesDe" />
                        <asp:BoundField DataField="TiempoPromedioDias" HeaderText="TiempoPromedioDias" />
                        <asp:BoundField DataField="RequiereEstimacion" HeaderText="RequiereEstimacion" />
                        <asp:BoundField DataField="TextoRolEstima" HeaderText="RolEstima" />
                        <asp:BoundField DataField="TextoUnidadEjecuta" HeaderText="UnidadEjecuta" />
                        <asp:BoundField DataField="UnidadRecibe" HeaderText="UnidadRecibe" />
                        <asp:BoundField DataField="TextoRolEjecuta" HeaderText="RolEjecuta" />
                        <asp:BoundField DataField="Visible" HeaderText="Visible" />
                        <asp:TemplateField HeaderText="Quitar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgQuitar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Quitar" ImageUrl="~/Images/Delete_16.png" Text="Quitar" ToolTip="Quitar" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Tareas Previas" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgTareasPrevias" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="TareasPrevias" ImageUrl="~/Images/Select_16.png" Text="TareasPrevias" ToolTip="TareasPrevias" />
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
                                            Enabled='<%# IIF(gvTareasAsignadas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvTareasAsignadas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvTareasAsignadas.PageIndex + 1%>-<%= gvTareasAsignadas.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvTareasAsignadas.PageIndex +1) = gvTareasAsignadas.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvTareasAsignadas.PageIndex +1) = gvTareasAsignadas.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </div>
        </div>
        <div id="accordion1">
            <h3>
                <a href="#">
                    <label>
                        Sin Asignar
                    </label>
                </a>
            </h3>
            <div class="block">
                <asp:GridView ID="gvTareasNoAsignadas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                        <asp:BoundField DataField="NoEmpiezaAntesDe" HeaderText="NoEmpiezaAntesDe" />
                        <asp:BoundField DataField="NoTerminaAntesDe" HeaderText="NoTerminaAntesDe" />
                        <asp:BoundField DataField="TiempoPromedioDias" HeaderText="TiempoPromedioDias" />
                        <asp:BoundField DataField="RequiereEstimacion" HeaderText="RequiereEstimacion" />
                        <asp:BoundField DataField="TextoRolEstima" HeaderText="RolEstima" />
                        <asp:BoundField DataField="TextoUnidadEjecuta" HeaderText="UnidadEjecuta" />
                        <asp:BoundField DataField="UnidadRecibe" HeaderText="UnidadRecibe" />
                        <asp:BoundField DataField="TextoRolEjecuta" HeaderText="RolEjecuta" />
                        <asp:BoundField DataField="Visible" HeaderText="Visible" />
                        <asp:TemplateField HeaderText="Adicionar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgAdicionar" ImageUrl="/Images/Select_16.png" ToolTip="Adicionar"
                                    runat="server" AlternateText="Adicionar" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Adicionar" />
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
                                            Enabled='<%# IIF(gvTareasNoAsignadas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvTareasNoAsignadas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvTareasNoAsignadas.PageIndex + 1%>-<%= gvTareasNoAsignadas.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvTareasNoAsignadas.PageIndex +1) = gvTareasNoAsignadas.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvTareasNoAsignadas.PageIndex +1) = gvTareasNoAsignadas.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
