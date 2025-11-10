<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
    CodeBehind="Configuracion_Tareas_Documentos.aspx.vb" Inherits="WebMatrix.Configuracion_Tareas_Documentos" %>

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
    Nombre tarea:<asp:Label ID="lblNombreTarea" runat="server"></asp:Label>
    <br />
    Tipo de documento:<asp:Label ID="lblTipoDocumento" runat="server"></asp:Label>
    <br />
    <asp:LinkButton ID="lnkVolver" runat="server" Text="Volver a tarea"></asp:LinkButton>
    <asp:HiddenField ID="hfIdTarea" runat="server" />
    <div id="accordion">
        <div id="accordion0">
            <h3>
                <a href="#">
                    <label>
                        Asignados
                    </label>
                </a>
            </h3>
            <div class="block">
                <asp:GridView ID="gvArchivosAsignados" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="IdDocumento, Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Documento" HeaderText="Documento" />
                        <asp:BoundField DataField="Controlado" HeaderText="Controlado" />
                        <asp:BoundField DataField="Activo" HeaderText="Activo" />
                        <asp:BoundField DataField="Codigo" HeaderText="Codigo" />
                        <asp:BoundField DataField="IdProceso" HeaderText="IdProceso" />
                        <asp:BoundField DataField="Responsable" HeaderText="Responsable" />
                        <asp:BoundField DataField="URL" HeaderText="URL" />
                        <asp:BoundField DataField="EsOpcional" HeaderText="EsOpcional" />
                        <asp:TemplateField HeaderText="Quitar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgQuitar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Quitar" ImageUrl="~/Images/Delete_16.png" Text="Quitar" ToolTip="Quitar" />
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
                                            Enabled='<%# IIF(gvArchivosAsignados.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvArchivosAsignados.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvArchivosAsignados.PageIndex + 1%>-<%= gvArchivosAsignados.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvArchivosAsignados.PageIndex +1) = gvArchivosAsignados.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvArchivosAsignados.PageIndex +1) = gvArchivosAsignados.PageCount, "false", "true") %>'
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
                <asp:HiddenField ID="hfIdDocumentoNoAsignado" runat="server" />
                <asp:GridView ID="gvArchivosNoAsignados" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="IdDocumento" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Documento" HeaderText="Documento" />
                        <asp:BoundField DataField="Controlado" HeaderText="Controlado" />
                        <asp:BoundField DataField="Activo" HeaderText="Activo" />
                        <asp:BoundField DataField="Codigo" HeaderText="Codigo" />
                        <asp:BoundField DataField="IdProceso" HeaderText="IdProceso" />
                        <asp:BoundField DataField="Responsable" HeaderText="Responsable" />
                        <asp:BoundField DataField="URL" HeaderText="URL" />
                        <asp:TemplateField HeaderText="Adicionar" ShowHeader="False">
                            <ItemTemplate>
                                <img id="imgAdicionar" src="/Images/Select_16.png" alt="Adicionar" title="Adicionar"
                                    onclick='<%#"AsignarNuevoDocumento(" & Eval("IdDocumento") & ")" %>' />
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
                                            Enabled='<%# IIF(gvArchivosNoAsignados.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvArchivosNoAsignados.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvArchivosNoAsignados.PageIndex + 1%>-<%= gvArchivosNoAsignados.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvArchivosNoAsignados.PageIndex +1) = gvArchivosNoAsignados.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvArchivosNoAsignados.PageIndex +1) = gvArchivosNoAsignados.PageCount, "false", "true") %>'
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
    <div id="AsignarNuevoDocumento">
        <fieldset class="validationGroup">
            <div class="block">
                <div class="form_left">
                    <fieldset>
                        <label>
                            Es opcional:
                        </label>
                        <asp:DropDownList ID="ddlEsOpcional" runat="server" CssClass="mySpecificClass dropdowntext">
                            <asp:ListItem Value="-1" Text="--Seleccione--"></asp:ListItem>
                            <asp:ListItem Value="1" Text="Si">
                            </asp:ListItem>
                            <asp:ListItem Value="0" Text="No">
                            </asp:ListItem>
                        </asp:DropDownList>
                    </fieldset>
                </div>
                <div class="actions">
                    <div class="form_right">
                        <fieldset>
                            <asp:Button ID="btnGrabar" runat="server" Text="Guardar" />
                        </fieldset>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
</asp:Content>
