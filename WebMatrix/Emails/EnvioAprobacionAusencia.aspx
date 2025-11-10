<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EnvioAprobacionAusencia.aspx.vb" Inherits="WebMatrix.EnvioAprobacionAusencia" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Alguien necesita tu aprobación para una Solicitud de Ausencia" runat="server"></asp:Label>
        <asp:Label ID="lblHWHId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:14px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Te contamos que el Usuario <b><asp:Label Text="" ID="lblUsuario" runat="server" /></b> ha registrado una solicitud de <b><asp:Label ID="lblTipoAusencia" runat="server"></asp:Label></b> desde <asp:Label ID="lblFini" runat="server"></asp:Label> hasta <asp:Label ID="lblFFin" runat="server"></asp:Label> .</p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para Aprobar o Rechazar <asp:HyperLink ID="hplLink" runat="server" Text="haz clic aquí"></asp:HyperLink></p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
