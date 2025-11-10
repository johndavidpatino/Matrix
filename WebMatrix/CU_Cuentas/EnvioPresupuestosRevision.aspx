<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterCuentas.master"
    CodeBehind="EnvioPresupuestosRevision.aspx.vb" Inherits="WebMatrix.EnvioPresupuestosRevision" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../Scripts/blockUIOnAllAjaxRequests.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Envío presupuestos para aprobación
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    Seleccione los presupuestos que desea que sean revisados por Operaciones
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
    <div id="Presupuestos">
        <asp:UpdatePanel ID="upPresupuestos" runat="server">
            <ContentTemplate>
                    <asp:HiddenField ID="hfidPropuesta" runat="server" />
                <asp:GridView ID="gvPresupuestos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id,Alternativa" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Valor" HeaderText="Valor" />
                        <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                        <asp:BoundField DataField="GrossMargin" HeaderText="GrossMargin" />
                        <asp:BoundField DataField="Alternativa" HeaderText="Alternativa" />
                        <asp:BoundField DataField="Aprobado" HeaderText="Aprobado" Visible="false" />
                        <asp:TemplateField HeaderText="Enviar a revisar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:CheckBox ID="chbEnviar" runat="server" Text="" />
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
                                            Enabled='<%# IIF(gvPresupuestos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvPresupuestos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvPresupuestos.PageIndex + 1%>-
                                            <%= gvPresupuestos.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvPresupuestos.PageIndex +1) = gvPresupuestos.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvPresupuestos.PageIndex +1) = gvPresupuestos.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
                <div class="spacer"></div>
                                            <asp:Button ID="btnEnviar" runat="server" Text="Enviar a revisión" />
                                            <asp:Button ID="btnVolver" runat="server" Text="Volver" />
                                
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
        <script type="text/javascript">
            var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
            pageReqManger.add_initializeRequest(InitializeRequest);
            pageReqManger.add_endRequest(EndRequest);
    </script>

</asp:Content>
