<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NotificacionPNC.aspx.vb" Inherits="WebMatrix.Emails_NotificacionPNC" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <label>Se ha asignado un nuevo producto no conforme, para ver el detalle, por favor </label><asp:HyperLink ID="hLink" runat="server" Text="Click aquí"></asp:HyperLink>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
