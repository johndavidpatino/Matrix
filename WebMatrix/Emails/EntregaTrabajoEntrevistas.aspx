<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EntregaTrabajoEntrevistas.aspx.vb"
    Inherits="WebMatrix.EntregaTrabajoEntrevistas" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Especificaciones del Trabajo" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size: 12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;
                color: #333333;">
                <p style="margin: 0 0 0 0; padding: 0 0 0 0;">
                    Se han creado las especificaciones para el siguiente trabajo:</p>
                <table border="1" style="color: #555; width: 100%; border-style: solid; font: 12px/15px Arial, Helvetica, sans-serif;
                    border: 1px solid #d3d3d3; background: #fefefe; margin: 5% auto 0; /*propiedad de centrado, borrar para dejarlo normal*/
    -moz-border-radius: 5px; -webkit-border-radius: 5px; -ms-border-radius: 5px; border-radius: 5px;
                    -moz-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2); -webkit-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);
                    -ms-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2); -o-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);">
                    <tr style="color: #049D9C;">
                        <td>
                            Nombre del Trabajo
                        </td>
                        <td>
                            <asp:Label ID="lblNombreTrabajo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            JobBook del Proyecto
                        </td>
                        <td>
                            <asp:Label ID="lblJobBook" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            ID del trabajo
                        </td>
                        <td>
                            <asp:Label ID="lblIdTrabajo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Gerente de Proyectos
                        </td>
                        <td>
                            <asp:Label ID="lblGerenteProyectos" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Fecha Inicio
                        </td>
                        <td>
                            <asp:Label ID="lblFechaInicio" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Fecha Fin
                        </td>
                        <td>
                            <asp:Label ID="lblFechaFin" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Metodología
                        </td>
                        <td>
                            <asp:Label ID="lblMetodologia" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Unidad
                        </td>
                        <td>
                            <asp:Label ID="lblUnidad" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: Navy; color: White; text-align: center;" colspan="2">
                            Información de la propuesta
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Muestra
                        </td>
                        <td>
                            <asp:Label ID="lblMuestra" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Backups
                        </td>
                        <td>
                            <asp:Label ID="lblBackups" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <%--<tr>
                        <td style="background-color: Navy; color: White; text-align: center;" colspan="2">
                            Incentivos, Regalos y Especificaciones
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Incentivo Económico
                        </td>
                        <td>
                            <asp:Label ID="lblIncentivoEconomico" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Presupuesto Incentivo Económico
                        </td>
                        <td>
                            <asp:Label ID="lblPresupuestoIncentivo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Distribucion Incentivo Económico
                        </td>
                        <td>
                            <asp:Label ID="lblDistribucionIncentivo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Regalos Cliente
                        </td>
                        <td>
                            <asp:Label ID="lblRegalosCliente" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: ##555;">
                        <td>
                            Compra Ipsos
                        </td>
                        <td>
                            <asp:Label ID="lblCompraIpsos" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Presupuesto Compra Ipsos
                        </td>
                        <td>
                            <asp:Label ID="lblPresupuestoCompra" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Distribución Presupuesto Compra Ipsos
                        </td>
                        <td>
                            <asp:Label ID="lblDistribucionCompra" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Observaciones, Exclusiones y Restricciones Específicas
                        </td>
                        <td>
                            <asp:Label ID="lblExclusionesRestricciones" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Recursos Propiedad del Cliente
                        </td>
                        <td>
                            <asp:Label ID="lblRecursosPropiedadCliente" runat="server"></asp:Label>
                        </td>
                    </tr>--%>
                  </table>
                 <br />

                <p style="margin:0 0 0 0;padding:0 0 0 0;">Para ver toda la información del Trabajo <asp:HyperLink ID="hplLink" runat="server" Text="haga clic aquí"></asp:HyperLink></p>

            </div>
        </asp:Panel>
        
    </div>
    </form>
</body>
</html>
