<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Tarea.aspx.vb" Inherits="WebMatrix.Tarea1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Tarea: @1"
            runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size: 12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;
                color: #333333;">
                <p style="margin: 0 0 0 0; padding: 0 0 0 0;">
                    JobBook:<asp:Label ID="lblJobBook" runat="server"></asp:Label>
                    <br />
                    Trabajo:<asp:Label ID="lblTrabajoId" runat="server"></asp:Label> - <asp:Label ID="lblTrabajoNombre" runat="server"></asp:Label>
                    <br />
                    Tarea:<asp:Label ID="lblTareaNombre" runat="server"></asp:Label>
                    <br />
                    <br />
                    La tarea ha cambiado a estado:<asp:Label ID="lblEstado" runat="server"></asp:Label>,
                    con la observación:<asp:Label ID="lblObservacion" runat="server"></asp:Label>
                </p>
                <br />
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
