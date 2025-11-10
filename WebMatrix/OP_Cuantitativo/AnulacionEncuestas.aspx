<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_F.master"
    CodeBehind="AnulacionEncuestas.aspx.vb" Inherits="WebMatrix.AnulacionEncuestas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">
         $(document).ready(function () {
             loadPlugins();
         });
        function loadPlugins() {
            validationForm();

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
    <a>Anulación de Encuestas</a>
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
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <asp:Label ID="txtNombreTrabajo" runat="server"></asp:Label>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Anular Encuestas
                                <asp:HiddenField ID="hfidTrabajo" runat="server" />
                                <asp:HiddenField ID="hfidUnidad" runat="server" />
                            </label>
                        </a>
                    </h3>

    <div class="block">
        <fieldset class="validationGroup">
            <div>
                <div class="form_left">
                    <fieldset>
                        <label>
                            Número de encuesta:
                        </label>
                        <asp:TextBox ID="txtEncuesta" runat="server" CssClass="required text textEntry"></asp:TextBox>
                        <asp:FilteredTextBoxExtender ID="ftTxtEncuesta" runat="server" TargetControlID="txtEncuesta" FilterType="Numbers"></asp:FilteredTextBoxExtender>
                    </fieldset> 
                </div>
                <div class="form_left">
                    <fieldset>
                        <label>
                            Observación:
                        </label>
                        <asp:TextBox ID="txtObservacion" runat="server" CssClass="required text textEntry"></asp:TextBox>
                    </fieldset>
                </div>
                <div class="actions">
                <div class="form_left">
                    <fieldset>
                        <asp:Button ID="btnAnular" runat="server" Text="Anular" 
                            OnClientClick="return confirm('Esta seguro de anular esta encuesta?');"  />
                        <asp:Button ID="btnVolver" runat="server" Text="Volver" />
                    </fieldset>
                </div>
                </div>
            </div>
        </fieldset>
    </div>
    </div>
    <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Encuestas Anuladas
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <asp:GridView ID="gvAnuladas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="NumeroEncuesta" HeaderText="No. Encuesta" />
                                <asp:BoundField DataField="Observacion" HeaderText="Observacion" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
                                <asp:BoundField DataField="Usuario" HeaderText="Anulada Por" />
                                <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImageButton2" runat="server" CausesValidation="False" CommandName="Eliminar"
                                            CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png"
                                            OnClientClick="return confirm('Esta seguro de eliminar este registro ?');" Text="Seleccionar" />
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
                                                    Enabled='<%# IIF(gvAnuladas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(gvAnuladas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvAnuladas.PageIndex + 1%>-<%= gvAnuladas.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((gvAnuladas.PageIndex +1) = gvAnuladas.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvAnuladas.PageIndex +1) = gvAnuladas.PageCount, "false", "true") %>'
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
    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
