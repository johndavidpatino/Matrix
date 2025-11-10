<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CoordinadorAsignado.aspx.vb" Inherits="WebMatrix.CoordinadorAsignado" %>

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
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Ha sido asignado el coordinador de campo <asp:Label ID="lblCoordinadorCampo" runat="server"></asp:Label> al trabajo: <asp:Label ID="lblTrabajo" runat="server"></asp:Label></p><br /> en la ciudad <asp:Label ID="lblCiudad" runat="server"></asp:Label>
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para ver el listado de trabajos asignados ingrese directamente a Matrix <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink> (solo coordinadores de campo)</p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
