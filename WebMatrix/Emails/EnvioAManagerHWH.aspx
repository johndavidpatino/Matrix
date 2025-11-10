<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EnvioAManagerHWH.aspx.vb" Inherits="WebMatrix.EnvioAManagerHWH" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Envío de Easy Work a Manager Para Aprobación" runat="server"></asp:Label>
        <asp:Label ID="lblHWHId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">El Usuario <asp:Label Text="" ID="lblUsuario" runat="server" /> ha solicitado el día <asp:Label Text="" ID="lblFecha" runat="server" /> para  hacer Easy Work.</p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para Aprobar/Rechazar <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink></p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
