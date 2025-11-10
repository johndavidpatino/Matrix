<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PresupuestosParaRevisar.aspx.vb" Inherits="WebMatrix.PresupuestosParaRevisarMail" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Presupuestos para revisión" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Han sido enviados presupuestos para revisar de la propuesta: <asp:Label ID="lblTitulo" runat="server"></asp:Label></p><br />
                Metodologías:
                <br />
                <asp:Label ID="lblMetodologia" runat="server"></asp:Label>
                <br />
                <br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">El gerente de cuentas es: <asp:Label ID="lblGerenteCuentas" runat="server"></asp:Label></p><br /><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para ir a la pantalla de revisión <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink> :</p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
