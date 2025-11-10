<%@ Page Language="vb" MasterPageFile="~/MasterPage/MBO_F.master" AutoEventWireup="false" CodeBehind="PropuestasSinTrabajo.aspx.vb" Inherits="WebMatrix.PropuestasSinTrabajo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../FusionCharts/FusionCharts.js"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
<a>Propuestas sin entregar a Operaciones </a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
 <table style="width:100%; height: 99%;">
   
    <tr>
        <td style="width:40%; height: 99%;">
            <asp:GridView ID="GVPropuestasPorUnidad" datakeynames="Unidad" runat="server" Width="50%" AutoGenerateColumns="False" PageSize="10"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                                PagerStyle-CssClass="headerfooter ui-toolbar" AllowSorting="True"  
                                AllowPaging="True" EmptyDataText="No existen registros para mostrar" 
                                DataSourceID="PropuestasPorUnidad">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <AlternatingRowStyle CssClass="odd" />
                    <SelectedRowStyle BackColor="#FFFF00" Font-Bold="True" />
                <Columns>
                    <asp:CommandField ShowSelectButton="True" />
                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" SortExpression="Unidad" />
                    <asp:BoundField DataField="Propuestas" HeaderText="Propuestas" SortExpression="Propuestas" ItemStyle-HorizontalAlign="left" HtmlEncode="False" />
                    <asp:BoundField DataField="VrPresupuesto" DataFormatString={0:N0} HeaderText="VrPresupuesto" SortExpression="VrPresupuesto" ItemStyle-HorizontalAlign="left" HtmlEncode="False" />
                </Columns>
                <HeaderStyle ForeColor="Red" />
            </asp:GridView>

            <asp:SqlDataSource ID="PropuestasPorUnidad" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                SelectCommand="MBO_PropuestasAprobadasSinTrabajoPorUnidad" SelectCommandType="StoredProcedure">
            </asp:SqlDataSource>
        </td>

        <td style="width:45%; height: 99%;">
            <asp:GridView ID="GVPropuestasMetodologia" runat="server" Width="50%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                                PagerStyle-CssClass="headerfooter ui-toolbar" AllowSorting="True"  
                                AllowPaging="True" EmptyDataText="No existen registros para mostrar" 
                                DataSourceID="PropuestasPorMetodologia">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <AlternatingRowStyle CssClass="odd" />
                    <SelectedRowStyle BackColor="#FFFF00" Font-Bold="True" />
                <Columns>
                    <asp:BoundField DataField="Metodologia" HeaderText="Metodologia"  ItemStyle-Width="50" SortExpression="Metodologia" />
                    <asp:BoundField DataField="Encuestas" DataFormatString={0:N0} HeaderText="Encuestas" SortExpression="Encuestas" ItemStyle-HorizontalAlign="left" ReadOnly="True" />
                    <asp:BoundField DataField="VrPresupuesto" DataFormatString={0:N0} HeaderText="VrPresupuesto" SortExpression="VrPresupuesto" ItemStyle-HorizontalAlign="left" ReadOnly="True" />
                </Columns>
                <HeaderStyle ForeColor="Red" />
            </asp:GridView>

            <asp:SqlDataSource ID="PropuestasPorMetodologia" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                SelectCommand="MBO_PropuestasAprobadasSinTrabajoUnidadMetodo" SelectCommandType="StoredProcedure">
                 <SelectParameters>
                    <asp:SessionParameter Name="UnaUnidad" SessionField="WUnaUnidad" Type="Int32" />
                    <asp:SessionParameter Name="UnidadMetodo" SessionField="WUnidadMetodo" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>
        </td>
        <td style="width:10%; height: 99%;">
            <asp:Button ID="btTotales" runat="server" Height="108px" Text="VER TOTALES" Width="139px" />
        </td>
    </tr>
 </table>

 <table style="width:100%; height: 99%;">
    <tr>
         <td style="width:100%; height: 99%;"> 
            <asp:GridView ID="GVPropuestasRelacion" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="12"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                                PagerStyle-CssClass="headerfooter ui-toolbar" AllowSorting="True"  
                                AllowPaging="True" EmptyDataText="No existen registros para mostrar" 
                                DataSourceID="PropuestasRelacion">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <AlternatingRowStyle CssClass="odd" />
                 
                <Columns>
                    <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" SortExpression="Id" />
                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" ReadOnly="True" SortExpression="JobBook" />
                    <asp:BoundField DataField="Titulo" HeaderText="Titulo" SortExpression="Titulo" />
                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" ReadOnly="True" SortExpression="Unidad" />
                    <asp:BoundField DataField="FechaAprobacion" DataFormatString={0:dd-M-yyyy} HeaderText="FechaAprobacion"  SortExpression="FechaAprobacion" />
                    <asp:BoundField DataField="FechaInicioCampo" DataFormatString={0:dd-M-yyyy} HeaderText="FechaInicioCampo" ReadOnly="True" SortExpression="FechaInicioCampo" />
                    <asp:BoundField DataField="Nombres" HeaderText="Nombres" ReadOnly="True" SortExpression="Nombres" />
                    <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" SortExpression="Apellidos" />
                    <asp:BoundField DataField="Encuestas" DataFormatString={0:N0} HeaderText="Encuestas" ReadOnly="True" SortExpression="Encuestas" />
                    <asp:BoundField DataField="VrPresupuesto" DataFormatString={0:N0} HeaderText="VrPresupuesto" ReadOnly="True" SortExpression="VrPresupuesto" />
                    
                </Columns>

                <HeaderStyle ForeColor="Red" />
                    <PagerTemplate>
                                <div class="pagingButtons">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                    Enabled='<%# IIF(GVPropuestasRelacion.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIF(GVPropuestasRelacion.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= GVPropuestasRelacion.PageIndex + 1%>-
                                                    <%= GVPropuestasRelacion.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIF((GVPropuestasRelacion.PageIndex +1) = GVPropuestasRelacion.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((GVPropuestasRelacion.PageIndex +1) = GVPropuestasRelacion.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
            </asp:GridView>
            <asp:SqlDataSource ID="PropuestasRelacion" runat="server" 
                ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                SelectCommand="MBO_PropuestasAprobadasSinTrabajo" SelectCommandType="StoredProcedure">
                <SelectParameters>
                    <asp:SessionParameter Name="Unidad" SessionField="WUnidad" Type="String" />
                </SelectParameters>
            </asp:SqlDataSource>

       </td>
       
    </tr>
   
</table>

</asp:Content>

