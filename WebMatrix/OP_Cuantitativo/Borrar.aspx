<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Borrar.aspx.vb" Inherits="WebMatrix.Borrar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtid" runat="server"></asp:TextBox>
    <asp:TextBox ID="txttrabajoId" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtobservacion" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtpregunta" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtdescripcion" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtrespuesta" runat="server"></asp:TextBox>

        <asp:Button ID="btnBuscar" runat="server" Text="buscar" />
       <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
    </div>
    </form>
</body>
</html>
