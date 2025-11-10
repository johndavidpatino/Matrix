<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EvaluarProveedorFacturas.aspx.vb" Inherits="WebMatrix.EvaluarProveedorFacturas" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblAsunto" Text="Evaluación Proveedor de Factura Radicada" runat="server"></asp:Label>
            <asp:Panel ID="pnlBody" runat="server" Width="90%">
                <div style="font-size: 12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400; color: #333333;">
                    <p style="margin: 0 0 0 0; padding: 0 0 0 0;">
                        Id Factura:<asp:Label ID="lblIdFactura" runat="server"></asp:Label>
                        <br />
                        No. Radicado:<asp:Label ID="lblNoRadicado" runat="server"></asp:Label>
                        <br />
                        NIT_CC:<asp:Label ID="lblNIT" runat="server"></asp:Label>
                        <br />
                        Proveedor:<asp:Label ID="lblProveedor" runat="server"></asp:Label>
                        <br />
                        Valor Factura:<asp:Label ID="lblValor" runat="server"></asp:Label>
                    </p>
                    <asp:Panel ID="pnlInvitacion" runat="server">
                        <p>Se ha radicado una factura, se requiere que evalúe al proveedor para continuar con el proceso. <asp:HyperLink ID="hplLink" runat="server" Text="Haga clic aquí"></asp:HyperLink></p> 
                    </asp:Panel>
                    <br />
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
