<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master"
    CodeBehind="RevisionPresupuestos.aspx.vb" Inherits="WebMatrix.RevisionPresupuestos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script type="text/javascript">
        function loadPlugins() {
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Revisión de presupuestos
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    Pruebe escribiendo uno o más criterios para ver el listado de presupuestos pendientes de aprobación
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
                <asp:TextBox ID="txtNoPropuestaBuscar" runat="server" placeholder="No Propuesta"></asp:TextBox>
                <asp:TextBox ID="txtNombreBuscar" runat="server" placeholder="Nombre Propuesta"></asp:TextBox>
                <asp:TextBox ID="txtJobBook" runat="server" placeholder="JobBook"></asp:TextBox>
                <asp:TextBox ID="txtIdTrabajo" runat="server" placeholder="ID Trabajo"></asp:TextBox><br />
                <asp:CheckBox ID="chbRevisados" runat="server" Text="Ver presupuestos ya revisados" />
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                <div class="spacer"></div>
                <asp:GridView ID="gvPresupuestos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="Id" Visible="false" />
                        <asp:BoundField DataField="Titulo" HeaderText="Nombre Propuesta" />
                        <asp:BoundField DataField="PropuestaId" HeaderText="No. Propuesta" />
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre Presupuesto" />
                        <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C0}" />
                        <asp:BoundField DataField="GrossMargin" HeaderText="GrossMargin" DataFormatString="{0:P2}" Visible="false" />
                        <asp:BoundField DataField="Alternativa" HeaderText="Alternativa" />
                        <asp:BoundField DataField="Probabilidad" HeaderText="Probabilidad" />
                        <asp:BoundField DataField="GerenteCuentas" HeaderText="Gerente Cuentas" />
                        <asp:BoundField DataField="Fecha" HeaderText="Enviado" />
                        <asp:TemplateField HeaderText="Revisar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrRevisar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Revisar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Editar presupuestos" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nueva Versión" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrRevisarNEWV" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="ReviewNew" ImageUrl="~/Images/Select_16.png" Text="Nueva versión" ToolTip="Revisar presupuestos en la nueva versión" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>

</asp:Content>
