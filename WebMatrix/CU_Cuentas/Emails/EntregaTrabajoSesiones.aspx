<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EntregaTrabajoSesiones.aspx.vb"
    Inherits="WebMatrix.EntregaTrabajoSesiones" %>

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
                    <tr style="color: #555;">
                        <td>
                            Grupo Objetivo
                        </td>
                        <td>
                            <asp:Label ID="lblGrupoObjetivo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Características Especiales
                        </td>
                        <td>
                            <asp:Label ID="lblCaracteristicasEspeciales" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Comentarios
                        </td>
                        <td>
                            <asp:Label ID="lblComentarios" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            MetodoAceptableReclutamiento
                        </td>
                        <td>
                            <asp:Label ID="lblMetodoAceptableReclutamiento" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Exclusiones y Restricciones Específicas
                        </td>
                        <td>
                            <asp:Label ID="lblExclusionesYRestriccionesEspecificas" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Recursos Propiedad del Cliente
                        </td>
                        <td>
                            <asp:Label ID="lblRecursosPropiedadCliente" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Observaciones
                        </td>
                        <td>
                            <asp:Label ID="lblObservaciones" runat="server"></asp:Label>
                        </td>
                    </tr>
                                        <tr style="color: #049D9C;">
                        <td>
                            Sesiones Requeridas
                        </td>
                        <td>
                            <asp:Label ID="lblCantidadRequerida" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Asistentes por sesión requeridos
                        </td>
                        <td>
                            <asp:Label ID="lblAsistentesRequeridos" runat="server"></asp:Label>
                        </td>
                    </tr>
                    </table>
                    <table  border="1" style="color: #555; width: 100%; border-style: solid; font: 12px/15px Arial, Helvetica, sans-serif;
                    border: 1px solid #d3d3d3; background: #fefefe; margin: 5% auto 0; /*propiedad de centrado, borrar para dejarlo normal*/
    -moz-border-radius: 5px; -webkit-border-radius: 5px; -ms-border-radius: 5px; border-radius: 5px;
                    -moz-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2); -webkit-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);
                    -ms-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2); -o-box-shadow: 0 0 4px rgba(0, 0, 0, 0.2);">
                    <tr>
                        <td style="background-color: Navy; color: White; text-align: center;" colspan="4">
                            Requerimientos
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Soporte de análisis
                        </td>
                        <td>
                            <asp:Label ID="lblSoporteAnalisis" runat="server"></asp:Label>
                        </td>
                        <td>
                            Quién brinda el soporte
                        </td>
                        <td>
                            <asp:Label ID="lblSoporteAdicional" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Soporte crítica
                        </td>
                        <td>
                            <asp:Label ID="lblSoporteCritica" runat="server"></asp:Label>
                        </td>
                        <td>
                            Apoyo logístico
                        </td>
                        <td>
                            <asp:Label ID="lblApoyoLogistico" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            FlashReport
                        </td>
                        <td>
                            <asp:Label ID="lblFlashReport" runat="server"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                        </tr>
                        <tr>
                        <td style="background-color: Navy; color: White; text-align: center;" colspan="4">
                            Incentivos y Regalos
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Incentivo Económico
                        </td>
                        <td>
                            <asp:Label ID="lblIncentivoEconomico" runat="server"></asp:Label>
                        </td>
                        <td>
                            Presupuesto Incentivo
                        </td>
                        <td>
                            <asp:Label ID="lblPresupuestoIncentivo" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">

                        <td>
                            Descripción Incentivo
                        </td>
                        <td>
                            <asp:Label ID="lblDescripcionIncentivo" runat="server"></asp:Label>
                        </td>
                        <td>
                            Regalos Clientes
                        </td>
                        <td>
                            <asp:Label ID="lblRegalosCliente" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Regalos Compra Ipsos
                        </td>
                        <td>
                            <asp:Label ID="lblCompraIpsos" runat="server"></asp:Label>
                        </td>
                        <td>
                            Presupuesto Regalos
                        </td>
                        <td>
                            <asp:Label ID="lblPresupuesto" runat="server"></asp:Label>
                        </td>
                        </tr>
                        <tr>
                        <td style="background-color: Navy; color: White; text-align: center;" colspan="4">
                            Equipos requeridos
                        </td>
                    </tr>
                    <tr style="color: #555;">

                        <td>
                            Circuito Cerrado
                        </td>
                        <td>
                            <asp:Label ID="lblCircuitoCerrado" runat="server"></asp:Label>
                        </td>
                        <td>
                            Filmación Fija
                        </td>
                        <td>
                            <asp:Label ID="lblFilmacionFija" runat="server"></asp:Label>
                        </td>
                        </tr>
                        <tr style="color: #049D9C;">

                        <td>
                            Cámara Fotográfica
                        </td>
                        <td>
                            <asp:Label ID="lblCamaraFotografica" runat="server"></asp:Label>
                        </td>
                        <td>
                            Televisor / DVD
                        </td>
                        <td>
                            <asp:Label ID="lblTV_DVD" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">

                        <td>
                            Filmación Activa
                        </td>
                        <td>
                            <asp:Label ID="lblFilmacionActiva" runat="server"></asp:Label>
                        </td>
                        <td>
                            Video Beam / Laptop
                        </td>
                        <td>
                            <asp:Label ID="lblVideoBeam_Laptop" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: Navy; color: White; text-align: center;" colspan="4">
                            Documentos entregados
                        </td>
                    </tr>
                    <tr style="color: #049D9C;">
                        <td>
                            Entrega Filtro Asistentes
                        </td>
                        <td>
                            <asp:Label ID="lblEntregaFiltroAsistentes" runat="server"></asp:Label>
                        </td>
                        <td>
                            Entrega de carta de invitación
                        </td>
                        <td>
                            <asp:Label ID="lblEntregaCartaInvitacion" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr style="color: #555;">
                        <td>
                            Entrega de Fax de Confirmación
                        </td>
                        <td>
                            <asp:Label ID="lblEntregaFaxConfirmacion" runat="server"></asp:Label>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>

            </div>
        </asp:Panel>
        
    </div>
    </form>
</body>
</html>
