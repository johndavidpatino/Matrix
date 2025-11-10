<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MBO_F.master" CodeBehind="GerenciaAOT.aspx.vb" Inherits="WebMatrix.GerenciaAOT" %>

<%@ Register assembly="BusyBoxDotNet" namespace="BusyBoxDotNet" tagprefix="busyboxdotnet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../FusionCharts/FusionCharts.js"> </script>
    <busyboxdotnet:BusyBox ID="BusyBox1" runat="server" Showbusybox="OnPostBackOnly" ShowTimeout="10000" Text="Leyendo datos" Title="Por favor espere..." />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
     <li><a title="Operaciones Produccion" href="CampoProduccion.aspx">Operaciones Produccion</a></li>
     <li><a title="Operaciones Calidad" href="CampoCalidadTotal.aspx">Operaciones Calidad</a></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a> Análisis AOT</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <table style="width:100%; height: 20%;">
        <tr>
            <td style="width:50%; height: 50%;">
                <asp:Literal ID="AOTMetaTotal" runat="server"></asp:Literal>
            </td>
            <td style="width:50%; height: 50%;">
                <asp:Literal ID="AOTBudgetTotal" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style="width:50%; height: 50%;">
                <asp:Literal ID="AOTMetaMes" runat="server"></asp:Literal>
            </td>
            <td style="width:50%; height: 50%;">
                <asp:Literal ID="AOTBudgetMes" runat="server"></asp:Literal>
            </td>
        </tr>
    </table>
    
    <table style="width:100%; height: 20%;">
    <tr>
        <td style="width:30%; height: 100%;">
            <asp:Literal  ID="AOTASI" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 100%;">
            <asp:Literal  ID="AOTASIMes" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 50%;">
            <asp:Literal  ID="AcquisitionASI" runat="server"></asp:Literal>
            <br />
            <asp:Literal  ID="AchievementASI" runat="server"></asp:Literal>
        </td>
        <td>
        </td>
     </tr>
    <tr>
        <td style="width:30%; height: 100%;">
            <asp:Literal  ID="AOTLO" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 100%;">
            <asp:Literal  ID="AOTLOMes" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 50%;">
            <asp:Literal  ID="AcquisitionLO" runat="server"></asp:Literal>
            <br />
            <asp:Literal  ID="AchievementLO" runat="server"></asp:Literal>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td style="width:30%; height: 100%;">
            <asp:Literal  ID="AOTMD" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 100%;">
            <asp:Literal  ID="AOTMDMes" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 50%;">
            <asp:Literal  ID="AcquisitionMD" runat="server"></asp:Literal>
            <br />
            <asp:Literal  ID="AchievementMD" runat="server"></asp:Literal>
        </td>
        <td>
        </td>
    </tr>
    
    <tr>
        <td style="width:30%; height: 100%;">
            <asp:Literal ID="AOTMK" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 100%;">
            <asp:Literal ID="AOTMKMes" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 50%;">
            <asp:Literal  ID="AcquisitionMK" runat="server"></asp:Literal>
            <br />
            <asp:Literal  ID="AchievementMK" runat="server"></asp:Literal>
        </td>
        <td>
        </td>
    </tr>

    <tr>
        <td style="width:30%; height: 100%;">
            <asp:Literal ID="AOTPA" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 100%;">
            <asp:Literal ID="AOTPAMes" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 50%;">
            <asp:Literal  ID="AcquisitionPA" runat="server"></asp:Literal>
            <br />
            <asp:Literal  ID="AchievementPA" runat="server"></asp:Literal>
        </td>
        <td>
        </td>
    </tr>
    <tr>
        <td style="width:30%; height: 100%;">
            <asp:Literal ID="AOTPG" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 100%;">
            <asp:Literal ID="AOTPGMes" runat="server"></asp:Literal>
        </td>
        <td style="width:30%; height: 50%;">
            <asp:Literal  ID="AcquisitionPG" runat="server"></asp:Literal>
            <br />
            <asp:Literal  ID="AchievementPG" runat="server"></asp:Literal>
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