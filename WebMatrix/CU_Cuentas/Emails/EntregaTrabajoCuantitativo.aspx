<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EntregaTrabajoCuantitativo.aspx.vb"
    Inherits="WebMatrix.EntregaTrabajoCuantitativo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Entrega" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size: 12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;
                color: #333333;">
                <p style="margin: 0 0 0 0; padding: 0 0 0 0;">
                    Se hace Entrega del siguiente trabajo:</p>
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
                            JobBook
                        </td>
                        <td>
                            <asp:Label ID="lblJobBook" runat="server"></asp:Label>
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
                    <tr>
                        <td style="background-color: Navy; color: White; text-align: center;" colspan="2">
                            Especificaciones
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Grupo Objetivo
                        </td>
                        <td>
                            <asp:Label ID="lblGrupoObjetivo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Cubrimiento geográfico
                        </td>
                        <td>
                            <asp:Label ID="lblCubrimientoGeografico" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Marco muestral
                        </td>
                        <td>
                            <asp:Label ID="lblMarcoMuestral" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Distribución muestra
                        </td>
                        <td>
                            <asp:Label ID="lblDistribucionMuestra" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Cuotas
                        </td>
                        <td>
                            <asp:Label ID="lblCuotas" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Nivel desagregación resultados
                        </td>
                        <td>
                            <asp:Label ID="lblDesagregacionResultados" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Ponderacion
                        </td>
                        <td>
                            <asp:Label ID="lblPonderacion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Requerimientos especiales
                        </td>
                        <td>
                            <asp:Label ID="lblRequerimientosEspeciales" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Otras Observaciones
                        </td>
                        <td>
                            <asp:Label ID="lblOtrasObservaciones" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Incentivo Económico
                        </td>
                        <td>
                            <asp:Label ID="lblIncentivoEconimico" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Presupuesto Incentivo
                        </td>
                        <td>
                            <asp:Label ID="lblPresupuestoIncentivo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Regalos cliente
                        </td>
                        <td>
                            <asp:Label ID="lblRegalosCliente" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Compra Ipsos
                        </td>
                        <td>
                            <asp:Label ID="lblCompraIpsos" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Presupuesto regalos
                        </td>
                        <td>
                            <asp:Label ID="lblPresupuesto" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </asp:Panel>
        
    </div>
    </form>
</body>
</html>
