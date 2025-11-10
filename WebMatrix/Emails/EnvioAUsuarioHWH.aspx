<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EnvioAUsuarioHWH.aspx.vb" Inherits="WebMatrix.EnvioAUsuarioHWH" %>


<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Confirmación de Easy Work a Usario" runat="server"></asp:Label>
        <asp:Label ID="lblHWHId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Su Easy Work solicitado para el día <asp:Label Text="" ID="lblFecha" runat="server" /> fue <b><asp:Label Text="" ID="lblEstado" runat="server" /></b> por <asp:Label Text="" ID="lblUsuario" runat="server" />.</p><br />
                <%--<p style="margin:0 0 0 0;padding:0 0 0 0;">Para Aprobar/Rechazar <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink></p>--%>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
