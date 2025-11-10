<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DesvinculacionEmpleadoFinProceso.aspx.vb" Inherits="WebMatrix.DesvinculacionEmpleadoFinProceso" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Proceso de desvinculación evaluación areas finalizado" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">La evaluación de las areas para el proceso de desvinculación del empleado @NombreEmpleadoADesvincular, ha finalizado.</p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
