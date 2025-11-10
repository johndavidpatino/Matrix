<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGestionTratamiento.master"
    CodeBehind="InformeAnulacion.aspx.vb" Inherits="WebMatrix.REP_InformeAnuladas" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
        });

        function loadPlugins() {

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $("#<%= txtFechaInicio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaTerminacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaTerminacion.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Informe de Anulación
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div>
                <div id="accordion0">
                    <h3>
                                Indicadores Generales<asp:HiddenField ID="hfidTrabajo" runat="server" />
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <label>Seleccione la fecha Inicio</label>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <label>Seleccione la fecha Fin</label>
                            <asp:TextBox ID="txtFechaTerminacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <asp:Button ID="btnConsultar" runat="server" Text="Consultar" />
                            <br /><br />
                            <div style="text-align: center">
                                <asp:Chart ID="ChartEstado" runat="server" Palette="Excel" BackColor="#F3DFC1" Width="700px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2"
                                    BorderColor="181, 64, 1">
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="% de Anulación" Name="Title1" ForeColor="26, 59, 105">
                                        </asp:Title>
                                    </Titles>
                                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                    <Series>
                                        <asp:Series Name="Series1">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White"
                                            BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                            <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                WallWidth="0" IsClustered="False" />
                                            <AxisY LineColor="64, 64, 64, 64" LabelAutoFitMaxFontSize="8" Minimum="0">
                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                <LabelStyle Format="0%" Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            </AxisY>
                                            <AxisX LineColor="64, 64, 64, 64" LabelAutoFitMaxFontSize="8">
                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsEndLabelVisible="False" />
                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                            </AxisX>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                                <br />
                            </div>
                        </div>
                    </div>
                    <%--items--%>
                </div>
                <div id="accordion1">
                    <h3>
                                Detalles
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <asp:GridView ID="gvInformeAnulacion" runat="server" Width="70%" AutoGenerateColumns="False" DataKeyNames="ID"
                                AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                    <asp:BoundField DataField="Realizadas" HeaderText="Realizadas" />
                                    <asp:BoundField DataField="Anuladas" HeaderText="Anuladas" />
                                    <asp:BoundField DataField="PorcAnulacion" HeaderText="Porcentaje Anulación" DataFormatString="{0:P2}" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="accordion3">
                    <h3>
                        <a href="#">
                            <label>
                                Exportado
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <div style="text-align:center"><asp:Button ID="btnExportar" runat="server" Text="Generar exportado" /></div>
        <br />
        <asp:GridView ID="gvExportado" runat="server" AutoGenerateColumns="False" Visible="False"
            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px" 
            CellPadding="3" DataSourceID="SqlDsExportado" 
            GridLines="Vertical">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" 
                    SortExpression="TrabajoId" />
                <asp:BoundField DataField="Nombre Trabajo" HeaderText="Nombre Trabajo" 
                    SortExpression="Nombre Trabajo" ReadOnly="True" />
                <asp:BoundField DataField="FechaReporte" HeaderText="FechaReporte" 
                    SortExpression="FechaReporte" />
                <asp:BoundField DataField="No Encuesta" 
                    HeaderText="No Encuesta" 
                    SortExpression="No Encuesta" />
                <asp:BoundField DataField="Ciudad" 
                    HeaderText="Ciudad" 
                    SortExpression="Ciudad" />
                <asp:BoundField DataField="CiudadReclasificada" HeaderText="CiudadReclasificada" 
                    SortExpression="CiudadReclasificada" />
                <asp:BoundField DataField="CCEncuestador" HeaderText="CCEncuestador" 
                    SortExpression="CCEncuestador" />
                <asp:BoundField DataField="ENCUESTADOR" HeaderText="ENCUESTADOR" 
                    SortExpression="ENCUESTADOR" ReadOnly="True" />
                <asp:BoundField DataField="TIPOENCUESTADOR" HeaderText="TIPOENCUESTADOR" 
                    SortExpression="TIPOENCUESTADOR" />
                <asp:BoundField DataField="CCSUPERVISOR" HeaderText="CCSUPERVISOR" 
                    SortExpression="CCSUPERVISOR" />
                <asp:BoundField DataField="SUPERVISOR" HeaderText="SUPERVISOR" 
                    SortExpression="SUPERVISOR" ReadOnly="True" />
                <asp:BoundField DataField="COE" HeaderText="OMP" 
                    SortExpression="COE" ReadOnly="True" />
                <asp:BoundField DataField="GERENCIAOPERACIONES" HeaderText="Gerencia Operaciones" 
                    SortExpression="GERENCIAOPERACIONES" ReadOnly="True" />
                <asp:BoundField DataField="UnidadAnula" HeaderText="Unidad Anula" 
                    SortExpression="UnidadAnula" ReadOnly="True" />
                <asp:BoundField DataField="PersonaAnula" HeaderText="Persona Anula" 
                    SortExpression="PersonaAnula" ReadOnly="True" />
            </Columns>
            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
            <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
            <SortedAscendingCellStyle BackColor="#F1F1F1" />
            <SortedAscendingHeaderStyle BackColor="#0000A9" />
            <SortedDescendingCellStyle BackColor="#CAC9C9" />
            <SortedDescendingHeaderStyle BackColor="#000065" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDsExportado" runat="server" 
            ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
            
            SelectCommand="REP_InformeAnuladasExportado" SelectCommandType="StoredProcedure">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtFechaInicio" Name="FechaI" 
                    PropertyName="Text" DbType="Date" />
                <asp:ControlParameter ControlID="txtFechaTerminacion" DbType="Date" 
                    Name="FechaF" PropertyName="Text" />
            </SelectParameters>
        </asp:SqlDataSource>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
