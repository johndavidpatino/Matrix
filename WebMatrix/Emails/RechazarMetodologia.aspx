<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RechazarMetodologia.aspx.vb" Inherits="WebMatrix.RechazarMetodologia" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Nueva Metodologia de Campo" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">La metodología de campo para el trabajo <asp:Label id="lblTrabajo" runat="server"></asp:Label>, ha sido rechazada:</p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Rechazada por: <asp:Label id="lblRechazado" Font-Bold="true" runat="server"></asp:Label></p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Razón: <br /><asp:Label id="lblRazon" runat="server"></asp:Label></p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
