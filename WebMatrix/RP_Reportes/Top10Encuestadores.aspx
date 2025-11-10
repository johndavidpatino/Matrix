<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/TH_F.master"
    CodeBehind="Top10Encuestadores.aspx.vb" Inherits="WebMatrix.REP_Top10Encuestadores" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Top 10 Encuestadores
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
    
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                 
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="form_left">
                            <a>Meses a consultar</a>
                            <asp:TextBox ID="txtMeses" runat="server" Text="1"></asp:TextBox>
                        </div>
                        <div class="form_left">
                            <a>Ciudades</a><asp:DropDownList ID="ddlCiudades" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="--Ver todas--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Bogotá" Value="11001"></asp:ListItem>
                            <asp:ListItem Text="Medellín" Value="5001"></asp:ListItem>
                            <asp:ListItem Text="Cali" Value="76001"></asp:ListItem>
                            <asp:ListItem Text="Barranquilla" Value="8001"></asp:ListItem>
                            <asp:ListItem Text="Otras ciudades" Value="900001"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form_left">
                            <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
                        </div>
                        <div class="actions">
                        <h4>Top 10 Indice de Anulación</h4>
                        <asp:GridView ID="gvAnulacion" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="IdEncuestador" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="IdEncuestador" HeaderText="CC" />
                        <asp:BoundField DataField="Encuestador" HeaderText="Encuestador" />
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo de encuestador" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                        <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha Ingreso" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="Realizadas" HeaderText="Realizadas" />
                        <asp:BoundField DataField="Anuladas" HeaderText="Anuladas" />
                        <asp:BoundField DataField="Errores" HeaderText="Errores" />
                        <asp:BoundField DataField="Indice" HeaderText="Indice" DataFormatString="{0:P2}" />
                        <asp:TemplateField HeaderText="Ver Ficha" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                    ToolTip="Ir a la ficha del encuestador" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                                
                        <h4>Top 10 Indice de Errores</h4>
                        <asp:GridView ID="gvErrores" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="IdEncuestador" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="IdEncuestador" HeaderText="CC" />
                        <asp:BoundField DataField="Encuestador" HeaderText="Encuestador" />
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo de encuestador" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                        <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha Ingreso" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="Realizadas" HeaderText="Realizadas" />
                        <asp:BoundField DataField="Anuladas" HeaderText="Anuladas" />
                        <asp:BoundField DataField="Errores" HeaderText="Errores" />
                        <asp:BoundField DataField="Indice" HeaderText="Indice" DataFormatString="{0:P2}" />
                        <asp:TemplateField HeaderText="Ver Ficha" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                    ToolTip="Ir a la ficha del encuestador" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                                <br />
                        <h4>Top 10 VIP</h4>
                        <asp:GridView ID="gvVIP" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="IdEncuestador" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="IdEncuestador" HeaderText="CC" />
                        <asp:BoundField DataField="Encuestador" HeaderText="Encuestador" />
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo de encuestador" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                        <asp:BoundField DataField="FechaIngreso" HeaderText="Fecha Ingreso" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="Realizadas" HeaderText="Realizadas" />
                        <asp:BoundField DataField="Anuladas" HeaderText="Anuladas" />
                        <asp:BoundField DataField="Errores" HeaderText="Errores" />
                        <asp:BoundField DataField="Indice" HeaderText="Indice" DataFormatString="{0:P2}" Visible="false" />
                        <asp:TemplateField HeaderText="Ver Ficha" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                    ToolTip="Ir a la ficha del encuestador" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                                <br /><br />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
