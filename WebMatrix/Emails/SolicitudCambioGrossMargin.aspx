<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SolicitudCambioGrossMargin.aspx.vb" Inherits="WebMatrix.SolicitudCambioGrossMargin" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Solicitud Cambio de GROSS MARGIN en Presupuesto" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Se está solicitando un cambio de Gross Margin para la propuesta <asp:Label id="lblPropuesta" runat="server"></asp:Label> a un Gross Margin de <asp:Label id="lblNewGM" runat="server"></asp:Label></p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para realizar el cambio dentro de Matrix <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink> :</p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
