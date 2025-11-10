<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TareaNotificacionFecha.aspx.vb" Inherits="WebMatrix.TareaNotificacionFecha" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Tarea: @1" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size: 12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;
                color: #333333;">
                <p style="margin: 0 0 0 0; padding: 0 0 0 0;">
                    JobBook:<asp:Label ID="lblJobBook" runat="server"></asp:Label>
                    <br />
                    Trabajo:<asp:Label ID="lblTrabajoId" runat="server"></asp:Label>
                    -
                    <asp:Label ID="lblTrabajoNombre" runat="server"></asp:Label>
                    <br />
                    Tarea:<asp:Label ID="lblTareaNombre" runat="server"></asp:Label>
                    <br />
                    <br />
                </p>
                    La planificación de la tarea ha tenido una actualización de fechas.<br />
                    Fecha de Inicio <asp:Label ID="lblInicio" runat="server" Font-Bold="true"></asp:Label> - Fecha fin <asp:Label ID="lblFin" runat="server" Font-Bold="true"></asp:Label>
                    <br />
                    <br />
                <br />
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
