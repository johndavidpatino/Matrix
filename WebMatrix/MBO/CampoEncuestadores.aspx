<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MBO_F.master" CodeBehind="CampoEncuestadores.aspx.vb" Inherits="WebMatrix.CampoEncuestadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../FusionCharts/FusionCharts.js"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
<asp:RadioButtonList ID="Tipos" runat="server" 
        RepeatDirection="Horizontal" style="display:inline"  Height="16px" Width="523px" AutoPostBack="True">
        <asp:ListItem Value="2">Empleados</asp:ListItem>
        <asp:ListItem Value="7">Contratistas</asp:ListItem>
        <asp:ListItem Value="9">Todos</asp:ListItem>
    </asp:RadioButtonList>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <table style="width:100%; height: 99%;">
   
    <tr>
        <td style="width:75%; height: 99%;">
            <asp:GridView ID="GVEncuestadores" datakeynames="Cedula" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="20"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                                PagerStyle-CssClass="headerfooter ui-toolbar" AllowSorting="True"  
                                AllowPaging="True" EmptyDataText="No existen registros para mostrar" SelectedRowStyle-CssClass="SelectedRow"
                                DataSourceID="CampoErroresEncuestadores">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <AlternatingRowStyle CssClass="odd" />
                    
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="cedula" HeaderText="cedula" SortExpression="cedula" />
                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" SortExpression="Nombre" ItemStyle-HorizontalAlign="left" HtmlEncode="False"> 
                        <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    </asp:BoundField>
                    <asp:BoundField DataField="Tipo" HeaderText="Tipo" SortExpression="Tipo" />
                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" SortExpression="Ciudad" />
                    <asp:BoundField DataField="Encuestas" HeaderText="Encuestas" ReadOnly="True" SortExpression="Encuestas" />
                    <asp:BoundField DataField="Errores" HeaderText="Errores" ReadOnly="True" SortExpression="Errores" HtmlEncode="false" NullDisplayText="0" />
                    
                    <asp:TemplateField HeaderText="Indice" SortExpression="True">
                        <ItemTemplate>
                            <asp:TextBox ID="Indice1" runat="server" Width="24"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                
                </Columns>

                <HeaderStyle ForeColor="Red" />
                    <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(GVEncuestadores.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(GVEncuestadores.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= GVEncuestadores.PageIndex + 1%>-
                                                    <%= GVEncuestadores.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((GVEncuestadores.PageIndex +1) = GVEncuestadores.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((GVEncuestadores.PageIndex +1) = GVEncuestadores.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
            </asp:GridView>
            <asp:SqlDataSource ID="CampoErroresEncuestadores" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                SelectCommand="MBO_OPCampoEncuestadores" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="Tipo" SessionField="WTipo" Type="Int32" />
                    <asp:SessionParameter Name="WAño" SessionField="WAño" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td>
         <td style="width:25%; height: 99%;"> 
            <asp:GridView ID="GVEncuestadorMeses" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="12"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                                PagerStyle-CssClass="headerfooter ui-toolbar" AllowSorting="True"  
                                AllowPaging="True" EmptyDataText="No existen registros para mostrar" 
                                DataSourceID="CampoErroresEncuestador">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <AlternatingRowStyle CssClass="odd" />
                 
                <Columns>
                   
                    <asp:BoundField DataField="Mes" HeaderText="Mes" SortExpression="Mes" />
                    <asp:BoundField DataField="Encuestas" HeaderText="Encuestas" ReadOnly="True" SortExpression="Encuestas" />
                    <asp:BoundField DataField="Errores" HeaderText="Errores" ReadOnly="True" SortExpression="Errores" />
                    
                    <asp:TemplateField HeaderText="Indice" SortExpression="True">
                        <ItemTemplate>
                            <asp:TextBox ID="Indice2" runat="server" Width="24"/>
                        </ItemTemplate>
                    </asp:TemplateField>

                </Columns>

                <HeaderStyle ForeColor="Red" />
                    <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(GVEncuestadorMeses.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(GVEncuestadorMeses.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= GVEncuestadorMeses.PageIndex + 1%>-
                                                    <%= GVEncuestadorMeses.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((GVEncuestadorMeses.PageIndex +1) = GVEncuestadorMeses.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((GVEncuestadorMeses.PageIndex +1) = GVEncuestadorMeses.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
            </asp:GridView>
            <asp:SqlDataSource ID="CampoErroresEncuestador" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                SelectCommand="MBO_OPCampoMuestraErroresMesEncuestador" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="Encuestador" SessionField="WEncuestador" Type="Int32" />
                    <asp:SessionParameter Name="Año" SessionField="WAño" Type="Int32" />
                </SelectParameters>
            </asp:SqlDataSource>

       </td>
       
    </tr>
   
</table>

</asp:Content>
