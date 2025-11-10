<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master" CodeBehind="ImportarDatos.aspx.vb" Inherits="WebMatrix.ImportarDatos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
      <div style ="width:100%">
            <br />
            <asp:Label ID="Label1" runat="server" Text="<h2><center>IMPORTADOR CATI A RMC</center></h2>"></asp:Label>
            <asp:Label ID="Label2" runat="server" Text="<h3>IMPORTAR DATOS DESDE EXCEL</h3>"></asp:Label>
                <br />
            <asp:HiddenField ID="hfTypeFile" runat="server" />

            <asp:Panel ID="pnlLoadFile" runat="server" Visible="true">
                <img src="../Images/excel.jpg" alt="Descargar plantilla de cargue" style="height: 74px; width: 64px" />
                <asp:HyperLink ID="hlPlantillaArchivo" runat="server" NavigateUrl="~/Files/Plantilla-CargueProduccion-V3.xlsx" Text="Descargar plantilla de cargue"></asp:HyperLink>
                <br />
                <br />
                <asp:Label ID="Label3" runat="server" Text="<b>1. Seleccionar Archivo de Excel</b>"></asp:Label>
                <br />
                <br />
                <asp:FileUpload ID="FileUpData" runat="server" />
                <br />
                <br />
                <asp:Button ID="btnLoadFile" runat="server" Text="Abrir Archivo" />
                <br />
                <br />
                <asp:Label ID="lblFileIncorrect" runat="server" Text="Archivo Incorrecto, por favor asegurese que es un archivo excel" Visible="False"></asp:Label>

                <asp:Panel ID="pnlSheets" runat="server" Visible="False">
                    <asp:Label ID="Label5" runat="server" Text="<b>2. Seleccionar Hoja que contiene los datos a subir</b>"></asp:Label>
                    <br />
                    <br />
                    <asp:ListBox ID="ListBoxSheets" runat="server"></asp:ListBox>
                    <br />
                    <br />
                    <asp:Button ID="btnLoadData" runat="server" Text="Verificar Estructura de los Datos" />
                    <br />
                    <br />
                </asp:Panel>
            </asp:Panel>

            <asp:Panel ID="pnlSummaryLoad" runat="server" Visible="true">
                <asp:Label ID="Label4" runat="server" Text="<b>3. Detalles de la hoja de Excel seleccionada</b>"></asp:Label>
                <br />
                <br />
                <asp:Label ID="Label11" runat="server" Text="Cantidad de Registros:"></asp:Label>
                <asp:Label ID="lblRecordsSum" runat="server"></asp:Label>
                <br />
                <br />
                <label>
                    <asp:Label ID="Label4x" runat="server" Text="Estado:"></asp:Label></label>
                <label>
                    <asp:Label ID="lblEstado" runat="server"></asp:Label></label>
                <br />
                <br />
                <asp:Label ID="Label6" runat="server" Text="<b>4. Subir Datos al Servidor</b>"></asp:Label>
                <br />
                <br />
                <asp:Button ID="btnCargarData" runat="server" Text="Subir Datos" />
                <div class="spacer"></div>
            </asp:Panel>

            <asp:Panel ID="pnlLoadComplete" runat="server" Visible="False">
                <asp:Label ID="Label12" runat="server" Text="<b>Subida de Datos Completa</b>"></asp:Label>
                <br />
                <br />
                <asp:Label ID="Label7" runat="server" Text="<b>5. Verificar Datos Subidos y Generar Reporte de Errores </b>"></asp:Label>
                <br />
                <br />
                <asp:Button ID="btnFinalizar" runat="server" Text="Finalizar y Generar Reporte" />
                <br />
                <br />
            </asp:Panel>

            <asp:Panel ID="pnlResumen" runat="server" Visible="False">
                <asp:Label ID="Label8" runat="server" Text="<b>6. Resumen</b>"></asp:Label>
                <br />
                <br />
                <asp:Label ID="Label10" runat="server" Text="<b><center>Registros Nuevos Insertados</center></b>"></asp:Label>
                <br />
                <div>
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="true"
                    CellSpacing="4" 
                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    CellPadding="4" ForeColor="#333333" GridLines="None">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </div>
                <br />
                <br />
                <asp:Label ID="Label13" runat="server" Text="<b><center>Registros Nuevos No Insertados - Con Errores </center></b>"></asp:Label>
                <br />
                <div style="margin-left: auto; margin-right: auto; width:100% ">
                    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="true"
                    CellSpacing="4" 
                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    CellPadding="4" ForeColor="#333333" GridLines="None">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </div>
                <br />
                <br />
                <asp:Label ID="Label14" runat="server" Text="<b><center>Registros No insertados - Duplicados</center></b>"></asp:Label>
                <br />
                <div style="margin-left: auto; margin-right: auto; width:100% ">
                    <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="true"
                    CellSpacing="4" 
                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    CellPadding="4" ForeColor="#333333" GridLines="None">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </div>
                <br />
                <br />
                <asp:Label ID="Label9" runat="server" Text="<b>7. Reporte de Inconsistencias</b>"></asp:Label>
                <br />
                <br />
                <div style="margin-left: auto; margin-right: auto; width:100% ">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true" 
                    CellSpacing="4" 
                    AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    CellPadding="4" ForeColor="#333333" GridLines="None">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                    </asp:GridView>
                </div>
                <br />
                <br />
           </asp:Panel>
        </div>
</asp:Content>


