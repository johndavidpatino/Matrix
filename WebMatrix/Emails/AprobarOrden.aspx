<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AprobarOrden.aspx.vb" Inherits="WebMatrix.AprobarOrden" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblAsunto" Text="Aprobación de orden de " runat="server"></asp:Label>
            <asp:Label ID="lblTipo" runat="server"></asp:Label>
            <asp:Panel ID="pnlBody" runat="server" Width="90%">
                <div style="font-size: 12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400; color: #333333;">
                    <p style="margin: 0 0 0 0; padding: 0 0 0 0;">
                        <asp:Label ID="lblIdJBIJBECC" runat="server"></asp:Label>
                        -
                    <asp:Label ID="lblJBIJBECCNombre" runat="server"></asp:Label>
                        <br />
                        Nombre Proveedor:<asp:Label ID="lblNombreProveedor" runat="server"></asp:Label>
                    </p>
                    <asp:Panel ID="pnlIntro" runat="server">
                        Se ha registrado una orden, la cual requiere de su aprobación, por un valor de 
                        <b><asp:Label ID="lblValor" runat="server"></asp:Label></b>
                    </asp:Panel>
                    <asp:Panel ID="pnlURL" runat="server">
                        <asp:HyperLink ID="lnkRuta" runat="server" Text="Click Aqui" ></asp:HyperLink>
                    </asp:Panel>
                    <br />
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
