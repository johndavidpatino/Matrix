<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Tarea.aspx.vb" Inherits="WebMatrix.Tarea1" %>

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
                <asp:Panel ID="pnlDescripcionConUsuariosANotificar" runat="server">
                    La tarea ha cambiado a estado:<asp:Label ID="lblEstado" runat="server" Font-Bold="true"></asp:Label>,
                    con la observación:<asp:Label ID="lblObservacion" runat="server" Font-Bold="true"></asp:Label>
                </asp:Panel>
                <asp:Panel ID="pnlDescripcionSinUsuariosANotificar" runat="server">
                    Tarea finaliza con exito - Pero, no se pudo notificar a las tareas siguientes, que estan
                    a la espera de que esta tarea finalice, debido a que esas tareas no tienen usuario
                    asignado aún, por favor pongase en contacto con los coordinadores que requieren
                    saber que esta tarea finalizo con exito.
                </asp:Panel>
                <br />
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
