<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PresupuestosRevisados.aspx.vb" Inherits="WebMatrix.PresupuestosRevisados" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Presupuestos revisados" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Han sido revisados los presupuestos de la propuesta: <asp:Label ID="lblTitulo" runat="server"></asp:Label></p><br />
                

                <table border="1" style="color: #555; width: 100%; border-style: solid; font: 12px/15px Arial, Helvetica, sans-serif;
                    border: 1px solid #d3d3d3; background: #fefefe; margin: 5% auto 0; /*propiedad de centrado, borrar para dejarlo normal*/
    -moz-border-radius: 5px; -webkit-border-radius: 5px; -ms-border-radius: 5px; border-radius: 5px;
                    -moz-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2); -webkit-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);
                    -ms-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2); -o-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);">
                    <tr style="color: #049D9C;">
                        <td>
                            Fase
                        </td>
                        <td>
                            <asp:Label ID="lblFase" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Metodología
                        </td>
                        <td>
                            <asp:Label ID="lblMetodología" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                    <td>
                        Venta Operaciones
                    </td>
                    <td>
                        <asp:Label ID="lblVentaOPS" runat="server"></asp:Label>
                    </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Valor Venta
                        </td>
                        <td>
                            <asp:Label ID="lblValorVenta" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Gross Margin
                        </td>
                        <td>
                            <asp:Label ID="lblGross" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>

                <br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para ir a la pantalla de propuestas <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink> :</p>
            </div>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
