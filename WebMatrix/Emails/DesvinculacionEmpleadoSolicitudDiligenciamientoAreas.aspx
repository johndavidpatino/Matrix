<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DesvinculacionEmpleadoSolicitudDiligenciamientoAreas.aspx.vb" Inherits="WebMatrix.DesvinculacionEmpleadoNotificacionArea" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Nuevo proceso de desvinculación" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Se ha iniciado el proceso de desvinculacion de @NombreEmpleadoADesvincular , es necesario que diligencies el check list  de tu area ,para saber si exixte alguna novedad en el proceso.</p>
                <a href="https://colombia.intranetipsos.latam/Matrix/TH_TalentoHumano/DesvinculacionesEmpleadosGestionArea.aspx">Ingrese aquí</a>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
