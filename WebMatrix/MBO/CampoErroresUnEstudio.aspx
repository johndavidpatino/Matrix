<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MBO_F.master" CodeBehind="CampoErroresUnEstudio.aspx.vb" Inherits="WebMatrix.CampoErroresUnEstudio" %>


<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Trabajo a consultar : </a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:DropDownList ID="Trabajos" runat="server" AutoPostBack="True">
    </asp:DropDownList>&nbsp;&nbsp;&nbsp;
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
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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

                <asp:GridView ID="DGErrores" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="15"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                                PagerStyle-CssClass="headerfooter ui-toolbar" AllowSorting="True"  
                                AllowPaging="True" EmptyDataText="No existen registros para mostrar" 
                                DataSourceID="GestionCampo">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" SortExpression="Ciudad" />
                        <asp:BoundField DataField="Encuestador" HeaderText="Encuestador" SortExpression="Encuestador" />
                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                        <asp:BoundField DataField="Supervisor" HeaderText="Supervisor" SortExpression="Supervisor" />
                        <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" ReadOnly="True" />
                        <asp:BoundField DataField="Encuesta" HeaderText="Encuesta" SortExpression="Encuesta" />
                        <asp:BoundField DataField="P1" HeaderText="P1" SortExpression="P1" />
                        <asp:BoundField DataField="D1" HeaderText="D1" SortExpression="D1" />
                        <asp:BoundField DataField="P2" HeaderText="P2" SortExpression="P2" />
                        <asp:BoundField DataField="D2" HeaderText="D2" SortExpression="D2" />
                        <asp:BoundField DataField="P3" HeaderText="P3" SortExpression="P3" />
                        <asp:BoundField DataField="D3" HeaderText="D3" SortExpression="D3" />
                        <asp:BoundField DataField="P4" HeaderText="P4" SortExpression="P4" />
                        <asp:BoundField DataField="D4" HeaderText="D4" SortExpression="D4" />
                        <asp:BoundField DataField="P5" HeaderText="P5" SortExpression="P5" />
                        <asp:BoundField DataField="D5" HeaderText="D5" SortExpression="D5" />
                        <asp:BoundField DataField="P6" HeaderText="P6" SortExpression="P6" />
                        <asp:BoundField DataField="D6" HeaderText="D6" SortExpression="D6" />
                        <asp:BoundField DataField="P7" HeaderText="P7" SortExpression="P7" />
                        <asp:BoundField DataField="D7" HeaderText="D7" SortExpression="D7" />
                        <asp:BoundField DataField="AccionCampo" HeaderText="AccionCampo" SortExpression="AccionCampo" />
                        <asp:BoundField DataField="AccionCritica" HeaderText="AccionCritica" SortExpression="AccionCritica" />
                        <asp:BoundField DataField="FechaRecepcion" HeaderText="FechaRecepcion" SortExpression="FechaRecepcion" />
                    </Columns>
                    <HeaderStyle ForeColor="Red" />
                    <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(DGErrores.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(DGErrores.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= DGErrores.PageIndex + 1%>-
                                                    <%= DGErrores.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((DGErrores.PageIndex +1) = DGErrores.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((DGErrores.PageIndex +1) = DGErrores.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                </asp:GridView>
           
            <asp:SqlDataSource ID="GestionCampo" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                SelectCommand="MBO_OPCampoErroresUnEstudio" SelectCommandType="StoredProcedure">
                <SelectParameters>
                   <asp:SessionParameter Name = "Trabajo" SessionField = "WEstudio" Type = "Decimal" />
                </SelectParameters>
            </asp:SqlDataSource>
           
       </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

