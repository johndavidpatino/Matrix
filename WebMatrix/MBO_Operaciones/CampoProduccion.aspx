<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MBO_F.master" CodeBehind="CampoProduccion.aspx.vb" Inherits="WebMatrix.CampoProduccion" %>

<%@ Register assembly="BusyBoxDotNet" namespace="BusyBoxDotNet" tagprefix="busyboxdotnet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../FusionCharts/FusionCharts.js"> </script>
    <busyboxdotnet:BusyBox ID="BusyBox1" runat="server" Showbusybox="OnPostBackOnly" ShowTimeout="10000" Text="Leyendo datos" Title="Por favor espere..." />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
    <li><a title="MBO Gerencia" href="../MBO_Gerencial/GerenciaAOT.aspx">MBO Gerencia</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Operaciones campo produccion</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <table style="width:100%; height: 99%;">
    <tr>
        <td style="width:50%; height: 99%;">
            <asp:Literal ID="EncAHoyTotal" runat="server"></asp:Literal>
        </td>
        <td style="width:50%; height: 99%;">
            <asp:Literal ID="EncAHoyCATI" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td style="width:20%; height: 99%;">
            <asp:Literal ID="EncAHoyCARACARAHI" runat="server"></asp:Literal>
        </td>
        <td style="width:20%; height: 99%;">
            <asp:Literal ID="EncAHoyCARACARAEP" runat="server"></asp:Literal>
        </td>
         <td style="width:20%; height: 99%;">
            <asp:Literal ID="EncAHoyCARACARATK" runat="server"></asp:Literal>
        </td>
         <td style="width:20%; height: 99%;">
            <asp:Literal ID="EncAHoyCARACARALC" runat="server"></asp:Literal>
        </td>
         <td style="width:20%; height: 99%;">
            <asp:Literal ID="EncAHoyCARACARAOT" runat="server"></asp:Literal>
        </td>
    </tr>
    <tr>
        <td >
            <asp:GridView ID="GVCampoTotalEncuestas" runat="server" CellPadding="4" Font-Size="Medium" 
                            ForeColor="#333333" GridLines="None" style="font-size: small" 
                            HorizontalAlign="Left" PageSize="4"><AlternatingRowStyle BackColor="White" /> 
                            <EditRowStyle BackColor="#2461BF" />
                            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#EFF3FB" /><SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            </asp:GridView>
            <asp:GridView ID="GVTotalEncuestasAprobadas" runat="server" CellPadding="4" Font-Size="Medium" 
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
