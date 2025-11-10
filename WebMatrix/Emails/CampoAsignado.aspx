<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CampoAsignado.aspx.vb" Inherits="WebMatrix.CampoAsignado" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: OMP Asignado" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Se asignó a <asp:Label ID="lblCOE" runat="server"></asp:Label> al trabajo <b><asp:Label ID="lblTrabajo" runat="server"></asp:Label></b></p><br />
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
