<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="BriefEstadisticaCambio.aspx.vb" Inherits="WebMatrix.BriefEstadisticaMailCambio" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Nuevo Brief Estadística" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Ha sido modificado el brief de diseño muestral por parte del área de cuentas:</p><br />
                <strong>Gerente: </strong><asp:Label ID="lblSolicitante" Text="" runat="server"></asp:Label><br /><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para ver el brief <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink> :</p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
