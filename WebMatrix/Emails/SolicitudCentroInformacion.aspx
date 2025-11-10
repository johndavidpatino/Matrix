<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SolicitudCentroInformacion.aspx.vb" Inherits="WebMatrix.SolicitudCentroInformacionMail" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Solicitud de Medio - Centro de Información" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Número de Solicitud: <asp:Label ID="lblSolicitud" runat="server"></asp:Label></p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Se ha hecho la solicitud del Medio número: <asp:Label ID="lblMedio" runat="server"></asp:Label></p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">La Persona que lo solicita es: <asp:Label ID="lblUsuarioSolicita" runat="server"></asp:Label></p><br /><br />
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
