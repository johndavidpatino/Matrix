<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MBO_F.master" CodeBehind="CampoCalidadCiudad.aspx.vb" Inherits="WebMatrix.CampoCalidadCiudad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../FusionCharts/FusionCharts.js"> </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
    <li><a title="MBO Gerencia" href="CampoCalidadTotal.aspx">Calidad Total</a></li>
    <li><a title="MBO Gerencia" href="GerenciaAOT.aspx">MBO Gerencia</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Calidad campo ciudades</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <table style="width:100%; height: 99%;">
    <tr>
        <asp:Literal ID="GraficaCiudadesHistorico" runat="server"></asp:Literal>
    </tr>
    <tr>
        <td style="width:25%; height: 99%;">
            <asp:GridView ID="GVCiudades" runat="server" AllowSorting="True" 
                                            CellPadding="4" Font-Size="Medium" 
                                            ForeColor="#333333" GridLines="None" style="font-size: small" 
                                            HorizontalAlign="Left" Width="249px"><AlternatingRowStyle BackColor="White" /> <EditRowStyle BackColor="#2461BF" />
                                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#EFF3FB" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            </asp:GridView>
        </td>
         <td style="width:75%; height: 99%;">
            <asp:Literal ID="GraficaCiudadesTotal" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td style="width:25%; height: 99%;">
            <asp:GridView ID="GVCiudadesEmp" runat="server" AllowSorting="True" 
                                            CellPadding="4" 
                                            EnableSortingAndPagingCallbacks="True" 
                Font-Size="Medium" ForeColor="#333333" 
                                            GridLines="None" style="font-size: small" 
                Width="244px"><AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                            <EditRowStyle BackColor="#999999" /><FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                                    </asp:GridView>
        </td>
        <td style="width:75%; height: 99%;">
            <asp:Literal ID="GraficaCiudadesEmp" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td style="width:25%; height: 99%;">
            <asp:GridView ID="GVCiudadesCon" runat="server" AllowSorting="True" 
                                        CellPadding="4" Font-Size="Medium" 
                                        ForeColor="#333333" GridLines="None" style="font-size: small"><AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                                        <EditRowStyle BackColor="#999999" /><FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" /><RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            </asp:GridView>    
        </td>
        <td style="width:75%; height: 99%;">
            <asp:Literal ID="GraficaCiudadesCon" runat="server"></asp:Literal>
        </td>
    </tr>
</table>

</asp:Content>
