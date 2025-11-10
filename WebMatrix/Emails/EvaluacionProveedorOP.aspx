<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EvaluacionProveedorOP.aspx.vb" Inherits="WebMatrix.EvaluacionProveedorOP" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Evaluación de proveedores" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size: 12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;
                color: #333333;">
                <p style="margin: 0 0 0 0; padding: 0 0 0 0;">
                    Proveedor:<asp:Label ID="lblProveedor" runat="server"></asp:Label>
                    <br />
                    Trabajo:<asp:Label ID="lblTrabajoId" runat="server"></asp:Label>
                    -
                    <asp:Label ID="lblTrabajoNombre" runat="server"></asp:Label>
                    <br />
                </p>
                <asp:Panel ID="pnlInvitacion" runat="server">
                   <p>Lo invitamos a participar en la encuesta de evaluación de proveedores de Ipsos, la encuesta tomara menos de 2 minutos de su tiempo, la información que proporcione será usada solo como parte de nuestro programa de mejora continua, participando en esta encuesta usted acepta que la información que proporcione pueda ser compartida con el personal de Ipsos involucrado en el Sistema de Gestión de Calidad, quien la mantendrá bajo estricta confidencialidad</p>
                </asp:Panel>
                <asp:Panel ID="pnlURL" runat="server">
                    <asp:HyperLink ID="lnkRuta" runat="server" Text="Click Aqui"></asp:HyperLink>
                </asp:Panel>
                <br />
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
