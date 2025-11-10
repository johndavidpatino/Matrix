<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EnvioDefinicionAusencia.aspx.vb" Inherits="WebMatrix.EnvioDefinicionAusencia" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Respuesta a Solicitud de Ausencia" runat="server"></asp:Label>
        <asp:Label ID="lblHWHId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:14px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Confirmamos que la solicitud de <b><asp:Label ID="lblTipoAusencia" runat="server"></asp:Label></b> desde <asp:Label ID="lblFini" runat="server"></asp:Label> hasta <asp:Label ID="lblFFin" runat="server"></asp:Label> fue <asp:Label ID="lblStatus" runat="server"></asp:Label> .</p><br />
                <br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;"><asp:Label ID="lblMensajeAdicional" runat="server" Text=""></asp:Label></p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
