<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="ListaTrabajosTareas.aspx.vb" Inherits="WebMatrix.ListaTrabajosTareas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Lista de resumen trabajos - estados de tareas </a>
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
    <asp:LinkButton ID="lbtnVolver" runat="server">Volver</asp:LinkButton>
    <asp:GridView ID="gvTrabajosTareas" runat="server" Width="100%" AutoGenerateColumns="False"
        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
        DataKeyNames="Id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
        <PagerStyle CssClass="headerfooter ui-toolbar" />
        <SelectedRowStyle CssClass="SelectedRow" />
        <AlternatingRowStyle CssClass="odd" />
        <Columns>
            <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
            <asp:BoundField DataField="Creada" HeaderText="Creada" />
            <asp:BoundField DataField="EnCurso" HeaderText="EnCurso" />
            <asp:BoundField DataField="Asignada" HeaderText="Asignada" />
            <asp:BoundField DataField="Devuelta" HeaderText="Devuelta" />
            <asp:BoundField DataField="Finalizada" HeaderText="Finalizada" />
            <asp:BoundField DataField="NoAplica" HeaderText="NoAplica" />
            <asp:TemplateField HeaderText="Ver" ShowHeader="False">
                <ItemTemplate>
                    <asp:ImageButton ID="imgIrVer" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                        CommandName="Ver" ImageUrl="~/Images/Select_16.png" Text="Ver" ToolTip="Ver" />
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
                                Enabled='<%# IIF(gvTrabajosTareas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                Enabled='<%# IIF(gvTrabajosTareas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                        </td>
                        <td>
                            <span class="pagingLinks">[<%= gvTrabajosTareas.PageIndex + 1%>-<%= gvTrabajosTareas.PageCount%>]</span>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                Enabled='<%# IIF((gvTrabajosTareas.PageIndex +1) = gvTrabajosTareas.PageCount, "false", "true") %>'
                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                        </td>
                        <td>
                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                Enabled='<%# IIF((gvTrabajosTareas.PageIndex +1) = gvTrabajosTareas.PageCount, "false", "true") %>'
                                SkinID="paging">Ultimo »</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </div>
        </PagerTemplate>
    </asp:GridView>
</asp:Content>
