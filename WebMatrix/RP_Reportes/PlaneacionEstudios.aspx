<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master" CodeBehind="PlaneacionEstudios.aspx.vb" Inherits="WebMatrix.PlaneacionEstudios" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            loadPlugins();
            $('#dialog-factores').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Factores de variación y ajuste",
                width: "400px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });

            $('#dialog-datos').dialog(
            {
                modal: true,
                autoOpen: false,
                title: "Exportación de datos",
                width: "600px",
                closeOnEscape: true,
                open: function (type, data) {
                    $(this).parent().appendTo("form");
                }
            });
        });

        function loadPlugins() {

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

        }

        function MostrarFactoresDialog() {
            $('#dialog-factores').dialog("open");
        }

        function MostrarDatosDialog() {
            $('#dialog-datos').dialog("open");
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Planeacion Estudios sin Entregar</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    <a>Formulario que realiza los cálculos requeridos para planear cantidades y cargas</a>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Estudios sin entregar<asp:HiddenField ID="hfidTrabajo" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        Gerencias OP</label>
                                    <asp:DropDownList ID="ddlGerencias" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                                <fieldset>
                                    <label>
                                        Unidades</label>
                                    <asp:DropDownList ID="ddlUnidades" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                            </div>
                            <div class="form_left">
                                <fieldset>
                                    <label>
                                        Metodología</label>
                                    <asp:DropDownList ID="ddlMetodologia" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                                <fieldset>
                                    <label>
                                        Ciudad</label>
                                    <asp:DropDownList ID="ddlCiudad" runat="server">
                                    </asp:DropDownList>
                                </fieldset>
                            </div>
                            <div class="form_left">
                            <fieldset>
                                <label> </label>
                            </fieldset>
                            <asp:Button ID="btnBuscar" runat="server" Text="Mostrar información" />
                        </div>
                            <div class="actions"></div>
                                <asp:Chart ID="ChartEncuestas" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Encuestas" Name="Title1" ForeColor="26, 59, 105">
                                        </asp:Title>
                                    </Titles>
                                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                    <Series>
                                        <asp:Series Name="Series1" ChartType="Line" Legend="Legend1">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White"
                                            BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                            <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                WallWidth="0" IsClustered="False" />
                                            <AxisY LineColor="64, 64, 64, 64" LabelAutoFitMaxFontSize="8">
                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            </AxisY>
                                            <AxisX LineColor="64, 64, 64, 64" LabelAutoFitMaxFontSize="8" Interval="1">
                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsEndLabelVisible="False" />
                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                            </AxisX>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                                <div style="text-align:center">
                                    <asp:HiddenField ID="HiddenField1" runat="server" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton2" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                                <br />
                                <asp:Chart ID="ChartEncuestadores" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Encuestadores" Name="Title1" ForeColor="26, 59, 105">
                                        </asp:Title>
                                    </Titles>
                                    <BorderSkin SkinStyle="Emboss"></BorderSkin>
                                    <Series>
                                        <asp:Series Name="Series1" ChartType="Line" Legend="Legend1">
                                        </asp:Series>
                                    </Series>
                                    <ChartAreas>
                                        <asp:ChartArea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="White"
                                            BackColor="OldLace" ShadowColor="Transparent" BackGradientStyle="TopBottom">
                                            <Area3DStyle Rotation="10" Perspective="10" Inclination="15" IsRightAngleAxes="False"
                                                WallWidth="0" IsClustered="False" />
                                            <AxisY LineColor="64, 64, 64, 64" LabelAutoFitMaxFontSize="8">
                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" />
                                            </AxisY>
                                            <AxisX LineColor="64, 64, 64, 64" LabelAutoFitMaxFontSize="8" Interval="1">
                                                <LabelStyle Font="Trebuchet MS, 8.25pt, style=Bold" IsEndLabelVisible="False" />
                                                <MajorGrid LineColor="64, 64, 64, 64" />
                                            </AxisX>
                                        </asp:ChartArea>
                                    </ChartAreas>
                                </asp:Chart>
                            </div>
                            <div style="text-align:center">
                                    <asp:HiddenField ID="HiddenField2" runat="server" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton4" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="dialog-factores">
        <asp:UpdatePanel ID="upanelFactor" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <div class="form_left">
                    <label>Factor de variación calculado</label>
                    <asp:Label ID="lblFactorVariacion" Text="" runat="server"></asp:Label>
                </div>
                <div class="form_left">
                    <label>Especifique el factor de ajuste sin signos</label>
                    <asp:TextBox ID="tbFactorAjuste" runat="server"></asp:TextBox>
                </div>
                <div class="actions"></div>
                <asp:HiddenField ID="hfFactorGrafica" runat="server" />
                    <div class="form_rigth">
                        <asp:Button ID="btnUpdate" runat="server" Text="Especificar Ajuste" OnClientClick="$('#dialog-factores').dialog('close');" />
                    </div>
                    <div class="actions"></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="dialog-datos">
        <asp:UpdatePanel ID="UPanelExport" runat="server" ChildrenAsTriggers="false"
            UpdateMode="Conditional">
            <ContentTemplate>
                <asp:HiddenField ID="hfGraficaDatos" runat="server" />
                    <asp:GridView ID="gvExport" runat="server" Width="100%"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                    PagerStyle-CssClass="headerfooter ui-toolbar" 
                    EmptyDataText="No existen registros para mostrar" DataSourceID="SqlDsExport">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                        </asp:GridView>
                <asp:SqlDataSource ID="SqlDsExport" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                    SelectCommand="OP_PL_PivotEstudiosSinEntregar" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hfGraficaDatos" Name="GRAFICA" 
                            PropertyName="Value" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlGerencias" Name="GERENCIA" 
                            PropertyName="SelectedValue" Type="Int64" />
                        <asp:ControlParameter ControlID="ddlUnidades" Name="UNIDAD" 
                            PropertyName="SelectedValue" Type="Int64" />
                        <asp:ControlParameter ControlID="ddlMetodologia" Name="METODOLOGIA" 
                            PropertyName="SelectedValue" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlCiudad" Name="CIUDAD" 
                            PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:GridView ID="gvExportCantidades" runat="server" Width="100%"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                    PagerStyle-CssClass="headerfooter ui-toolbar" 
                    EmptyDataText="No existen registros para mostrar" DataSourceID="SqlDsExportCantidades">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                        </asp:GridView>
                <asp:SqlDataSource ID="SqlDsExportCantidades" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                    SelectCommand="OP_PL_EstudiosSinEntregarEncuestas_Datos" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hfGraficaDatos" Name="GRAFICA" 
                            PropertyName="Value" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlGerencias" Name="GERENCIA" 
                            PropertyName="SelectedValue" Type="Int64" />
                        <asp:ControlParameter ControlID="ddlUnidades" Name="UNIDAD" 
                            PropertyName="SelectedValue" Type="Int64" />
                        <asp:ControlParameter ControlID="ddlMetodologia" Name="METODOLOGIA" 
                            PropertyName="SelectedValue" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlCiudad" Name="CIUDAD" 
                            PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                <asp:GridView ID="gvExportRecursos" runat="server" Width="100%"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" 
                    PagerStyle-CssClass="headerfooter ui-toolbar" 
                    EmptyDataText="No existen registros para mostrar" DataSourceID="SqlDsExportRecursos">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                        </asp:GridView>
                <asp:SqlDataSource ID="SqlDsExportRecursos" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
                    SelectCommand="OP_PL_EstudiosSinEntregarRecursos_Datos" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hfGraficaDatos" Name="GRAFICA" 
                            PropertyName="Value" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlGerencias" Name="GERENCIA" 
                            PropertyName="SelectedValue" Type="Int64" />
                        <asp:ControlParameter ControlID="ddlUnidades" Name="UNIDAD" 
                            PropertyName="SelectedValue" Type="Int64" />
                        <asp:ControlParameter ControlID="ddlMetodologia" Name="METODOLOGIA" 
                            PropertyName="SelectedValue" Type="Int32" />
                        <asp:ControlParameter ControlID="ddlCiudad" Name="CIUDAD" 
                            PropertyName="SelectedValue" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                    <div class="form_rigth">
                        <asp:Button ID="btnExport" runat="server" Text="Exportar proyección a Excel" OnClientClick="$('#dialog-factores').dialog('close');" />
                        <asp:Button ID="btnExportC" runat="server" Text="Exportar matriz de datos" OnClientClick="$('#dialog-factores').dialog('close');" />
                        <asp:Button ID="btnExportR" runat="server" Visible="false" Text="Exportar datos recursos a Excel" OnClientClick="$('#dialog-factores').dialog('close');" />
                    </div>
                    <div class="actions"></div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>    
</asp:Content>
