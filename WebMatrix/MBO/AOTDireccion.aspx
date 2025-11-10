<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MBO_F.master" CodeBehind="AOTDireccion.aspx.vb" Inherits="WebMatrix.AOTDireccion" %>

<%@ Register assembly="BusyBoxDotNet" namespace="BusyBoxDotNet" tagprefix="busyboxdotnet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../FusionCharts/FusionCharts.js"> </script>
    <busyboxdotnet:BusyBox ID="BusyBox1" runat="server" Showbusybox="OnPostBackOnly" ShowTimeout="10000" Text="Leyendo datos" Title="Por favor espere..." />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>AOT - AL MES DE : </a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:DropDownList ID="Meses" runat="server" AutoPostBack="True">
    </asp:DropDownList>&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" Width="98px" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <table style="width:100%; height: 20%;">
        <tr>
            <td style="width:50%; height: 25%;">
                <asp:Literal ID="AOTMetaTotal" runat="server"></asp:Literal>
            </td>
            <td style="width:50%; height: 25%;">
                <asp:Literal ID="AOTBudgetTotal" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="width:50%; height: 25%;">
                <asp:Literal ID="AOTMetaMes" runat="server"></asp:Literal>
            </td>
            <td style="width:50%; height: 25%;">
                <asp:Literal ID="AOTBudgetMes" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
      
    <table style="width:100%; height: 50%;">
    <tr>
        <td style="width:30%; height: 100%;">
            <asp:Literal  ID="AOTUnidad" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 100%;">
            <asp:Literal  ID="AOTUnidadMes" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 50%;">
            <asp:Literal  ID="AcquisitionUnidad" runat="server"></asp:Literal>
            <br />
            <asp:Literal  ID="AchievementUnidad" runat="server"></asp:Literal>
        </td>
        <td>
        </td>
     </tr>
    
    
    <tr>
        <td>
            <asp:GridView ID="GVAOTPorUnidadMes" runat="server" CellPadding="4" Font-Size="Medium" 
                            ForeColor="#333333" GridLines="None" style="font-size: small" 
                            HorizontalAlign="Left" PageSize="4"><AlternatingRowStyle BackColor="White" /> 
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            </asp:GridView>
            <asp:GridView ID="GVAOTPorUnidad" runat="server" CellPadding="4" Font-Size="Medium" 
                            ForeColor="#333333" GridLines="None" style="font-size: small" 
                            HorizontalAlign="Left" PageSize="4"><AlternatingRowStyle BackColor="White" /> 
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            </asp:GridView>
             <asp:GridView ID="GVAOTAcquisition" runat="server" CellPadding="4" Font-Size="Medium" 
                            ForeColor="#333333" GridLines="None" style="font-size: small" 
                            HorizontalAlign="Left" PageSize="4"><AlternatingRowStyle BackColor="White" /> 
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            </asp:GridView>
           
        </td>
    </tr>
    </table>

</asp:Content>