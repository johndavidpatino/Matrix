<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EncuestaAnuladaNotificacion.aspx.vb" Inherits="WebMatrix.Emails_EncuestaAnuladaNotificacion" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="" runat="server"></asp:Label>
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
                    <br />
                </p>
                    Ha sido anulada una encuesta del trabajo en mención.<br /><br />
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
                        <td>OMP</td>
                        <td><asp:Label ID="lblCOE" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Coordinador</td>
                        <td><asp:Label ID="lblCoordinador" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color: #555;">
                        <td>Anulado por</td>
                        <td><asp:Label ID="lblAnulador" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Unidad que anula</td>
                        <td><asp:Label ID="lblUnidad" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="background-color:Navy; color:White; text-align:center;" colspan="2">Información de la encuesta anulada</td>
                    </tr>
                    <tr style="color: #555;">
                        <td>Número de encuesta</td>
                        <td><asp:Label ID="lblNumero" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Razón de anulación</td>
                        <td><asp:Label ID="lblRazon" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color: #555;">
                        <td>Ciudad</td>
                        <td><asp:Label ID="lblCiudad" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <br />
                    <p>Importante: este correo está en prueba aún; si presenta alguna inconsistencia por favor comuníquese con el equipo de Matrix. Gracias.</p>
                <br />
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
