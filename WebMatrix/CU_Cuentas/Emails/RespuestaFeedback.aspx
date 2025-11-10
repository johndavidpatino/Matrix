<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RespuestaFeedback.aspx.vb" Inherits="WebMatrix.RespuestaFeedbackMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Respuesta a su retroalimentación" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Se ha respondido a su retroalimentación dada el día <asp:Label ID="lblFecha" runat="server"> la cual decía: </asp:Label> <asp:Label ID="lblMensaje" runat="server"></asp:Label></p><br /><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">La respuesta es:</p>
                <p><asp:Label ID="lblRespuesta" runat="server"></asp:Label></p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
