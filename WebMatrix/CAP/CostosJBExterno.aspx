<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="CostosJBExterno.aspx.vb" Inherits="WebMatrix.CostosJBExterno" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Styles/TabsStyles.css" rel="stylesheet" type="text/css" />
          <style type="text/css">
                  
        table { text-align : center
        }
        tr {text-align : left}
        th {text-align : left}
        td{text-align : left       
      }
              .style1
              {
                  font-size: large;
                  color: #FFFFFF;
                  font-family: Verdana;
              }
              
              
                .RightAlign
        {
            text-align: Right;
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
                        <td>
                            &nbsp;</td>
                        <td style="text-align: right">
                            <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="True" 
                                Font-Size="Medium">Volver</asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                            <table style="width:100%;" __designer:mapid="3">
                                <tr __designer:mapid="4">
                                    <td style="text-align: center" __designer:mapid="5" class="style1">
                                        <strong>JOB BOOK EXTERNO</strong></td>
                                </tr>
                                <tr __designer:mapid="4">
                                    <td style="text-align: right" __designer:mapid="5">
                                        <asp:ImageButton runat="server" ImageUrl="~/Images/excel.jpg" CssClass="style3" 
                                            Height="36px" Width="39px" ID="ExpToExcel"></asp:ImageButton>

                                    </td>
                                </tr>
                                <tr __designer:mapid="7">
                                    <td __designer:mapid="8">
                                        <asp:GridView runat="server" EmptyDataText="No existen datos" 
                                            CssClass="displayTable" Width="100%" ID="gvJBExterno" ShowFooter="True">
                                            <FooterStyle Font-Bold="True" />
                                        </asp:GridView>

                                    </td>
                                </tr>
                                <tr __designer:mapid="c">
                                    <td __designer:mapid="d">
                                        &nbsp;</td>
                                </tr>
                            </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>
