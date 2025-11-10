<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ObservacionesIPS.aspx.vb" Inherits="WebMatrix.ObservacionesIPSMail" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="hfidtrabajo" runat="server" />
            <asp:HiddenField ID="hfidtarea" runat="server" />
            <asp:Label ID="lblAsunto" Text="" runat="server"></asp:Label>
            <asp:Panel ID="pnlBody" runat="server" Width="90%">
                <div style="font-size: 12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400; color: #333333;">
                    <p style="margin: 0 0 0 0; padding: 0 0 0 0;">
                        <asp:Label ID="lblBodyIPS" runat="server"></asp:Label>
                        <asp:Label ID="lblTrabajo" runat="server"></asp:Label>
                    </p>
                   
                    <br />
                    <p style="margin: 0 0 0 0; padding: 0 0 0 0;">
                        Para ver el Registro de Observación
                        <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink>
                    </p>
                </div>
            </asp:Panel>
        </div>
    </form>
</body>
</html>
