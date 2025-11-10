<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SolicitudPresupuesto.aspx.vb" Inherits="WebMatrix.Emails_SolicitudPresupuesto" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Solicitud Presupuesto" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Se ha hecho una solicitud de presupuesto para el trabajo <asp:Label ID="lblTrabajo" runat="server"></asp:Label></p><br />
            </div>
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
            <br />
            <p style="margin:0 0 0 0;padding:0 0 0 0;">OMP: <asp:Label ID="lblcoe" runat="server"></asp:Label></p><br />
            </div>

              <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
            <br />
            <p style="margin:0 0 0 0;padding:0 0 0 0;">Observación<asp:Label ID="lblObservacion" runat="server"></asp:Label></p><br />
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
