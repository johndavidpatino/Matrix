<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="GerenteProyectosAsignado.aspx.vb" Inherits="WebMatrix.GerenteProyectosMail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Gerente de Proyectos Asignado" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Ha sido asignado el gerente de proyectos <asp:Label ID="lblGerenteProyectos" runat="server"></asp:Label> al proyecto: <asp:Label ID="lblProyecto" runat="server"></asp:Label></p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para ver el listado de proyectos asignados <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink> (solo gerentes de proyectos)</p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
