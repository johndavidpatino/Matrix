<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master" CodeBehind="PlaneacionOperaciones.aspx.vb" Inherits="WebMatrix.PlaneacionOperaciones" %>
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
    <a>Planeacion General de Operaciones</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    <a>Formulario que realiza los cálculos requeridos para planear cantidades y cargas</a>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" id="accordion">
                <div id="accordion00">
                    <h3>
                                Menú de Planeación
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                                <label>Seleccione el proceso que desea ver</label>
                                <div class="actions">
                                    <asp:button ID="Button1" runat="server" Text="Campo" />
                                    <asp:button ID="Button2" runat="server" Text="Scripting" />
                                    <asp:button ID="Button3" runat="server" Text="Critica" />
                                    <asp:button ID="Button4" runat="server" Text="Verificación" />
                                    <asp:button ID="Button5" runat="server" Text="Captura" />
                                    <asp:button ID="Button6" runat="server" Text="Codificación" />
                                    <asp:button ID="Button7" runat="server" Text="Procesamiento" />
                                </div>
                            </div>
                    </div>
                </div>
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Campo<asp:HiddenField ID="hfidTrabajo" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
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
                                    <asp:LinkButton ID="LinkButton1" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
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
                                    <asp:LinkButton ID="LinkButton3" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton4" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                    </div>
                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Scripting
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                                <asp:Chart ID="ChartScriptingTrabajos" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Trabajos" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField3" runat="server" />
                                    <asp:LinkButton ID="LinkButton5" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton6" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                                <br />
                                <asp:Chart ID="ChartScriptingPersonas" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Personas" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField4" runat="server" />
                                    <asp:LinkButton ID="LinkButton7" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton8" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                            </div>
                    </div>
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Crítica
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                                <asp:Chart ID="ChartCriticaEncuestas" runat="server" Palette="Fire" 
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
                                    <asp:HiddenField ID="HiddenField5" runat="server" />
                                    <asp:LinkButton ID="LinkButton9" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton10" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                                <br />
                                <asp:Chart ID="ChartCriticaPersonas" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Personas" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField6" runat="server" />
                                    <asp:LinkButton ID="LinkButton11" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton12" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                            </div>
                    </div>
                </div>
                <div id="accordion3">
                    <h3>
                        <a href="#">
                            <label>
                                Verificación
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                                <asp:Chart ID="ChartVerificacionHoras" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Horas" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField7" runat="server" />
                                    <asp:LinkButton ID="LinkButton13" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton14" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                                <br />
                                <asp:Chart ID="ChartVerificacionPersonas" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Personas" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField8" runat="server" />
                                    <asp:LinkButton ID="LinkButton15" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton16" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                            </div>
                    </div>
                </div>
                <div id="accordion4">
                    <h3>
                        <a href="#">
                            <label>
                                Captura
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                                <asp:Chart ID="ChartCapturaPreguntas" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Preguntas" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField9" runat="server" />
                                    <asp:LinkButton ID="LinkButton17" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton18" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                                <br />
                                <asp:Chart ID="ChartCapturaPersonas" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Personas" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField10" runat="server" />
                                    <asp:LinkButton ID="LinkButton19" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton20" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                            </div>
                    </div>
                </div>
                <div id="accordion5">
                    <h3>
                        <a href="#">
                            <label>
                                Codificación
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                                <asp:Chart ID="ChartCodificacionPreguntas" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Preguntas" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField11" runat="server" />
                                    <asp:LinkButton ID="LinkButton21" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton22" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                                <br />
                                <asp:Chart ID="ChartCodificacionPersonas" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Personas" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField12" runat="server" />
                                    <asp:LinkButton ID="LinkButton23" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton24" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                            </div>
                    </div>
                </div>
                <div id="accordion6">
                    <h3>
                        <a href="#">
                            <label>
                                Procesamiento
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                                <asp:Chart ID="ChartProcesamientoTrabajos" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Trabajos" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField13" runat="server" />
                                    <asp:LinkButton ID="LinkButton25" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton26" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                                <br />
                                <asp:Chart ID="ChartProcesamientoPersonas" runat="server" Palette="Fire" 
                                    BackColor="243, 223, 193" Width="1024px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2px"
                                    BorderColor="#B54001">
                                    <Legends>
                                        <asp:Legend Name="Legend1" Alignment="Center" Docking="Bottom">
                                        </asp:Legend>
                                    </Legends>
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Cantidad de Personas" Name="Title1" ForeColor="26, 59, 105">
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
                                    <asp:HiddenField ID="HiddenField14" runat="server" />
                                    <asp:LinkButton ID="LinkButton27" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton28" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                            </div>
                    </div>
                </div>                                
            </asp:Panel>
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
                <asp:HiddenField ID="hfFactorDatos" runat="server" />
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
                    SelectCommand="OP_PL_PivotDatosGenerales" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hfFactorDatos" Name="FACTORA" PropertyName="Value" 
                            Type="Double" />
                        <asp:ControlParameter ControlID="hfGraficaDatos" Name="GRAFICA" 
                            PropertyName="Value" Type="Int32" />
                    </SelectParameters>
                </asp:SqlDataSource>
                    <div class="form_rigth">
                        <asp:Button ID="btnExport" runat="server" Text="Exportar a Excel" OnClientClick="$('#dialog-factores').dialog('close');" />
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
