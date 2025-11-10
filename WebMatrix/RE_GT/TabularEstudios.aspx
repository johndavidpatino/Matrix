<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RD_F.master" CodeBehind="TabularEstudios.aspx.vb" Inherits="WebMatrix.TabularEstudios"  EnableViewState ="false" %>
<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v22.2, Version=22.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <style type="text/css">
        .auto-style1 {
            text-align: left;
            height: 19px;
        }
        .auto-style2 {
            text-align: left;
            width: 936px;
            height: 19px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <table style="width:100%;">
        <tr>
            <td>
                <table style="width:100%;">
                    <tr>
                        <td class="auto-style1"></td>
                        <td class="auto-style2">
                            </td>
                        <td class="auto-style1">
                            <asp:LinkButton ID="btnVolver" runat="server" Font-Size="Medium">Volver</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                
                    <asp:Panel ID="Panel1" runat="server" EnableViewState="False" ScrollBars="Horizontal">
                    </asp:Panel>
                
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
