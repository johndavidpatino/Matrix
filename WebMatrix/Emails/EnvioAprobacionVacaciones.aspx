<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EnvioAprobacionVacaciones.aspx.vb" Inherits="WebMatrix.EnvioAprobacionVacaciones" %>

<!DOCTYPE html >

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Matrix: Llegaron las buenas noticias: Aprobación de Vacaciones" runat="server"></asp:Label>
        <asp:Label ID="lblHWHId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size:14px; font-family: 'Metrophobic', Calibri, serif; font-weight: 400;color:#333333;">
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Confirmamos que la solicitud de <b><asp:Label ID="lblTipoAusencia" runat="server"></asp:Label></b> desde  <asp:Label ID="lblFini" runat="server"></asp:Label> hasta <asp:Label ID="lblFFin" runat="server"></asp:Label> fue aprobada. </p><br />
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Te esperamos de regreso con toda tu energía el día <b><asp:Label ID="lblDiaRegreso" runat="server"></asp:Label></p><br />
                <p>No olvides dejar reportados los días en iTime antes de irte.</p>
                <p style="margin:0 0 0 0;padding:0 0 0 0;">Estos son los detalles de la causación de las vacaciones, para que sepas cuántos días te quedan del periodo que tomaste.</p>
                <br />
                
                <br /><br /><br />
                
            </div>
        </asp:Panel>
        <asp:GridView ID="gvPeriodos" runat="server" Width="100%" AutoGenerateColumns="False"
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
                        <asp:BoundField DataField="FIniPeriodo" HeaderText="Fecha Inicial" DataFormatString="{0:d}" />
                        <asp:BoundField DataField="FFinPeriodo" HeaderText="Fecha Final"  DataFormatString="{0:d}" />
                        <asp:BoundField DataField="DiasDisfrutados" HeaderText="Dias a Disfrutar" />
                        <asp:BoundField DataField="DiasPendientes" HeaderText="Dias Pendientes del Periodo" />
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
