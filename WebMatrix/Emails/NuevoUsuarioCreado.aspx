<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NuevoUsuarioCreado.aspx.vb" Inherits="WebMatrix.NuevoUsuarioCreado" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Bienvenido a Matrix!" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Bienvenido(a): <asp:Label ID="lblNombreCompleto" runat="server"></asp:Label>&nbsp; a nuestra nueva realidad.</p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Estos son sus datos para ingresar al sistema:</p>
                <br />
                <p><b>Usuario: </b> <asp:Label ID="lblUsuario" runat="server"></asp:Label></p>
                <p><b>Password: </b> 123456</p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Recuerde que su primer paso al ingresar al sistema  es cambiar este password  por una clave que sea privada.</p>
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para cualquier novedad  que necesite reportar, por favor utilizar la opción de Matrix denominada <i>Mi retroalimentación a Matrix</i>.</p>
                <p>El link para ingreso al sistema es: <a href="https://colombia.intranetipsos.latam/Matrix/Default.aspx">https://colombia.intranetipsos.latam/Matrix/Default.aspx</a></p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>

