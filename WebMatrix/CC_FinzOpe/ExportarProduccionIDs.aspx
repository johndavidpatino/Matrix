<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CAP_F.master" CodeBehind="ExportarProduccionIDs.aspx.vb" Inherits="WebMatrix.CC_ExportarProduccionIDs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">

    <asp:HiddenField ID="hfTypeFile" runat="server" />
    <div class="form_left">
        <table style="width: 100%;">
            <tr>
                <td width="20%">
                    <label>ID Inicial</label>
                    <asp:TextBox ID="txtIDInicial" runat="server" ></asp:TextBox>
                </td>
                <td width="20%">
                    <label>ID Final</label>
                    <asp:TextBox ID="txtIDFinal" runat="server" ></asp:TextBox>
                </td>
                <td width="60%">
                    
                </td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td width="50%">
                    <asp:Button ID="btnExport" runat="server" Text="Exportar" />
                </td>
                <td width="50%">
                    
                    
                </td>
            </tr>
        </table>

        
    </div>

</asp:Content>
