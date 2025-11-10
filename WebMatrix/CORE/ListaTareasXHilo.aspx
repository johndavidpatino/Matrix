<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="ListaTareasXHilo.aspx.vb" Inherits="WebMatrix.ListaTareasXHilo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

            $('#Observaciones').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Observaciones",
                width: "600px"
            });

        });
        function mostrarObservaciones() {
            $('#Observaciones').dialog("open");
        }F
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Lista de tareas</a>
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
    <div class="actions">
        <div class="form_rigth">
            <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver" />
        </div>
    </div>
    <asp:UpdatePanel ID="upTareas" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvTareasEnCurso" runat="server" Width="100%" AutoGenerateColumns="False"
                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                DataKeyNames="Id, TareaId, HiloId" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                <PagerStyle CssClass="headerfooter ui-toolbar" />
                <SelectedRowStyle CssClass="SelectedRow" />
                <AlternatingRowStyle CssClass="odd" />
                <Columns>
                    <asp:BoundField DataField="Tarea" HeaderText="Nombre" />
                    <asp:BoundField DataField="FIniP" HeaderText="FechaInicioPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FFinP" HeaderText="FechaFinPlaneada" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FIniR" HeaderText="FechaInicioReal" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="FFinR" HeaderText="FechaFinReal" DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="CantidadUsuariosAsignados" HeaderText="CantidadUsuariosAsignados" />
                    <asp:BoundField DataField="Estado" HeaderText="Estado" />
                    <asp:TemplateField HeaderText="Observaciones">
                        <ItemTemplate>
                            <asp:ImageButton ID="imgVerObservaciones" runat="server" CausesValidation="False"
                                CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Observaciones"
                                ImageUrl="~/Images/Select_16.png" Text="Observaciones" ToolTip="Ver observaciones"
                                OnClientClick="mostrarObservaciones()" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <PagerTemplate>
                    <div class="pagingButtons">
                        <table>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                        Enabled='<%# IIF(gvTareasEnCurso.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                        Enabled='<%# IIF(gvTareasEnCurso.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                </td>
                                <td>
                                    <span class="pagingLinks">[<%= gvTareasEnCurso.PageIndex + 1%>-<%= gvTareasEnCurso.PageCount%>]</span>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                        Enabled='<%# IIF((gvTareasEnCurso.PageIndex +1) = gvTareasEnCurso.PageCount, "false", "true") %>'
                                        SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                        Enabled='<%# IIF((gvTareasEnCurso.PageIndex +1) = gvTareasEnCurso.PageCount, "false", "true") %>'
                                        SkinID="paging">Ultimo »</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                </PagerTemplate>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="Observaciones">
        <asp:UpdatePanel ID="upObservaciones" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hfWorkFlowId" runat="server" />
                <asp:GridView ID="gvObservaciones" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="FechaRegistro" HeaderText="FechaRegistro" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                        <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                        <asp:BoundField DataField="Cargo" HeaderText="Cargo" />
                    </Columns>
                    <PagerTemplate>
                        <div class="pagingButtons">
                            <table>
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                            Enabled='<%# IIF(gvObservaciones.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvObservaciones.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvObservaciones.PageIndex + 1%>-<%= gvObservaciones.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvObservaciones.PageIndex +1) = gvObservaciones.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvObservaciones.PageIndex +1) = gvObservaciones.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
