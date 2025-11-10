<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NotificacionPNCAcciones.aspx.vb" Inherits="WebMatrix.Emails_NotificacionPNCAcciones" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Nueva Accion PNC Trabajo" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Ha sido registrada una nueva acción a realizar con las siguientes características:</p>
                
                <table border="1" style="color: #555; width:100%; border-style:solid; 
	font:12px/15px Arial, Helvetica, sans-serif;
    border:1px solid #d3d3d3;
    background:#fefefe;
    margin:5% auto 0;/*Propiedad de centrado, borrar para dejarlo normal*/
    -moz-border-radius:5px;
    -webkit-border-radius:5px;
	-ms-border-radius:5px;
    border-radius:5px;
    -moz-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);
    -webkit-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);
	-ms-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);
	-o-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);">
                    <tr style="color: #555;">
                        <td>Descripción PNC</td>
                        <td><asp:Label ID="lblDescripcion" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color: #555;">
                        <td>Tipo de Acción</td>
                        <td><asp:Label ID="lblTipoAccion" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Acción a ejecutar</td>
                        <td><asp:Label ID="lblAccion" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Fecha planeada</td>
                        <td><asp:Label ID="lblFechaPlaneada" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color: #555;">
                        <td>Responsable Acción</td>
                        <td><asp:Label ID="lblResponsableAccion" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Responsable Seguimiento</td>
                        <td><asp:Label ID="lblResponsableSeguimiento" runat="server"></asp:Label></td>
                    </tr>
                </table>
            </div>
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                    <p style="margin:0 0 0 0;padding:0 0 0 0;">Para consultar <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink> :</p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
