<%@ Page Language="vb" MasterPageFile="~/MasterPage/MBO_F.master"  AutoEventWireup="false" CodeBehind="PropuestasEstadoTotal.aspx.vb" Inherits="WebMatrix.PropuestasEstadoTotal" %>

<%@ Register assembly="BusyBoxDotNet" namespace="BusyBoxDotNet" tagprefix="busyboxdotnet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript" src="../FusionCharts/FusionCharts.js"> </script>
    <busyboxdotnet:BusyBox ID="BusyBox1" runat="server" Showbusybox="OnPostBackOnly" ShowTimeout="10000" Text="Leyendo datos" Title="Por favor espere..." />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <table style="width:100%; height: 100%;">
        <tr>
            <td style="width:50%; height: 50%;">
                <asp:Literal ID="CreadasEnviadas" runat="server"></asp:Literal>
            </td>
            <td></td>
            <td style="width:50%; height: 50%;">
                <asp:Literal ID="AltaProbabilidad" runat="server"></asp:Literal>
            </td>
        </tr>
        <tr>
            <td style= "justify">
                <asp:Literal ID="CreadasEnviadasGC" runat="server"></asp:Literal>
            </td>
        </tr>
        
    </table>
</asp:Content>