<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NuevoProyectoCoordinacion.aspx.vb" Inherits="WebMatrix.NuevoProyectoCoordinacionMail" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Nuevo Proyecto para Asignación" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Ha sido creado un nuevo proyecto para asignación de Gerente de Proyectos con nombre: <asp:Label ID="lblProyecto" runat="server"></asp:Label></p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para realizar asignación <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink> :</p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
