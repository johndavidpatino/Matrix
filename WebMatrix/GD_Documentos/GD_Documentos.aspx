<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/GD_F.master"
    CodeBehind="GD_Documentos.aspx.vb" Inherits="WebMatrix.GD_Documentos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });
            validationForm();
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
    <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver"></asp:LinkButton>
    <div id="accordion">
        <div id="accordion0">
            <h3>
                <a href="#">
                    <label>
                        Documentos cargados
                    </label>
                </a>
            </h3>
            <div class="block">
                <asp:GridView ID="gvDocumentosCargados" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="IdDocumentoRepositorio" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Version" HeaderText="Version" Visible="false" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha cargue" />
                        <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" />
                        <asp:TemplateField HeaderText="Descargar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Descargar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                    ToolTip="Tareas" />
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
                                            Enabled='<%# IIF(gvDocumentosCargados.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvDocumentosCargados.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvDocumentosCargados.PageIndex + 1%>-<%= gvDocumentosCargados.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvDocumentosCargados.PageIndex +1) = gvDocumentosCargados.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvDocumentosCargados.PageIndex +1) = gvDocumentosCargados.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </div>
        </div>
        <div id="accordion1" runat="server">
            <h3>
                <a href="#">
                    <label>
                        Cargar nuevo documento
                    </label>
                </a>
            </h3>
            <div class="block">
                <fieldset class="validationGroup">
                    <div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Nombre:
                                </label>
                                <asp:TextBox ID="txtNombre" runat="server" CssClass="required text textEntry"></asp:TextBox>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Comentarios:
                                </label>
                                <asp:TextBox ID="txtComentarios" runat="server" CssClass="required text textEntry"></asp:TextBox>
                            </fieldset>
                        </div>
                        <div class="form_left">
                            <fieldset>
                                <label>
                                    Ubicación Archivo:
                                </label>
                                <asp:FileUpload ID="ufArchivo" runat="server" Text="CargarArchivo" CssClass="required text textEntry" />
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="ufArchivo"
                                    ErrorMessage="*" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                            </fieldset>
                        </div>
                        <div class="actions">
                            <div class="form_right">
                                <fieldset>
                                    <asp:Button ID="btnGrabar" runat="server" Text="Guardar" CssClass="mySpecificClass2 causesValidation buttonText buttonSave corner-all" />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="causesValidation buttonText buttonSave corner-all" />
                                </fieldset>
                            </div>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
