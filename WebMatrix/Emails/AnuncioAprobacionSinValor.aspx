<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="AnuncioAprobacionSinValor.aspx.vb" Inherits="WebMatrix.Emails_AnuncioAprobacionSinValor" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Anuncio de Aprobación" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Ha sido aprobado el estudio en referencia y estas son las principales características:</p>
                
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
                        <td>Cliente</td>
                        <td><asp:Label ID="lblCliente" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Nombre del estudio</td>
                        <td><asp:Label ID="lblNombreEstudio" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color: #555;">
                        <td>JobBook</td>
                        <td><asp:Label ID="lblNumeroEstudio" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Fecha Inicio</td>
                        <td><asp:Label ID="lblFechaInicio" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Fecha Inicio Campo</td>
                        <td><asp:Label ID="lblFechaInicioCampo" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color: #555;">
                        <td>Fecha Fin</td>
                        <td><asp:Label ID="lblFechaFin" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="background-color:Navy; color:White; text-align:center;" colspan="2">Información de la propuesta</td>
                    </tr>
                    <tr style="color: #555;">
                        <td>Situación</td>
                        <td><asp:Label ID="lblSituacion" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Complicación</td>
                        <td><asp:Label ID="lblComplicación" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color: #555;">
                        <td>Pregunta o Reto de negocios</td>
                        <td><asp:Label ID="lblQuestion" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color: #555;">
                        <td>Evidencia o Recomendaciones</td>
                        <td><asp:Label ID="lblRecomendaciones" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Fecha aprobación</td>
                        <td><asp:Label ID="lblFechaAprobacion" runat="server"></asp:Label></td>
                    </tr>
                     <tr style="color: #555;">
                        <td>Gerente de Cuentas</td>
                        <td><asp:Label ID="lblGerenteCuentas" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color:#049D9C;">
                        <td>Documento Soporte</td>
                        <td><asp:Label ID="lblDocumentoSoporte" runat="server"></asp:Label></td>
                    </tr>
                    <tr style="color: #555;">
                        <td>Tiempo Retención (años)</td>
                        <td><asp:Label ID="lblTiempoRetencion" runat="server"></asp:Label></td>
                    </tr>
                </table>
                <p style="background-color:Navy; color:White; text-align:center;">Muestra asociada</p>
                <%--<asp:GridView ID="gvDatos" runat="server" Width="100%" BackColor="White" 
                    BorderColor="#DEDFDE" BorderStyle="None" BorderWidth="1px" CellPadding="4" 
                    ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" 
                    DataKeyNames="Id" DataSourceID="EntityDatos">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" ReadOnly="True" 
                            SortExpression="Id" />
                        <asp:BoundField DataField="PropuestaId" HeaderText="PropuestaId" 
                            SortExpression="PropuestaId" />
                        <asp:BoundField DataField="Valor" HeaderText="Valor" SortExpression="Valor" />
                        <asp:BoundField DataField="Muestra" HeaderText="Muestra" 
                            SortExpression="Muestra" />
                        <asp:BoundField DataField="ProductoId" HeaderText="ProductoId" 
                            SortExpression="ProductoId" />
                        <asp:BoundField DataField="GrossMargin" HeaderText="GrossMargin" 
                            SortExpression="GrossMargin" />
                        <asp:CheckBoxField DataField="UsadoPropuesta" HeaderText="UsadoPropuesta" 
                            SortExpression="UsadoPropuesta" />
                        <asp:BoundField DataField="NoCAP" HeaderText="NoCAP" SortExpression="NoCAP" />
                        <asp:BoundField DataField="JobBook" HeaderText="JobBook" 
                            SortExpression="JobBook" />
                        <asp:BoundField DataField="EstadoId" HeaderText="EstadoId" 
                            SortExpression="EstadoId" />
                    </Columns>
                    <FooterStyle BackColor="#CCCC99" />
                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                    <RowStyle BackColor="#F7F7DE" />
                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    <SortedAscendingCellStyle BackColor="#FBFBF2" />
                    <SortedAscendingHeaderStyle BackColor="#848384" />
                    <SortedDescendingCellStyle BackColor="#EAEAD3" />
                    <SortedDescendingHeaderStyle BackColor="#575357" />
                </asp:GridView>--%>
            </div>
        </asp:Panel>
        <asp:GridView ID="gvPresupuestosAsignadosXEstudio" runat="server" Width="100%" AutoGenerateColumns="False"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    AllowPaging="False" 
                EmptyDataText="No existen registros para mostrar" CellPadding="4" 
                ForeColor="#333333" GridLines="None">
                    <EditRowStyle BackColor="#7C6F57" />
                    <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
                    <PagerStyle CssClass="headerfooter ui-toolbar" BackColor="#666666" 
                        ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#E3EAEB" />
                    <SelectedRowStyle CssClass="SelectedRow" BackColor="#C5BBAF" Font-Bold="True" 
                        ForeColor="#333333" />
                    <AlternatingRowStyle CssClass="odd" BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="Metodologia" HeaderText="Metodologia" />
                        <asp:BoundField DataField="Ciudad" HeaderText="Ciudad"  />
                        <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                    </Columns>
                    <SortedAscendingCellStyle BackColor="#F8FAFA" />
                    <SortedAscendingHeaderStyle BackColor="#246B61" />
                    <SortedDescendingCellStyle BackColor="#D4DFE1" />
                    <SortedDescendingHeaderStyle BackColor="#15524A" />
                </asp:GridView>
    </div>
    </form>
</body>
</html>
