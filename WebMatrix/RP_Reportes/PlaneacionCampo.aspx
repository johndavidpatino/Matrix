<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP_F.master" CodeBehind="PlaneacionCampo.aspx.vb" Inherits="WebMatrix.PlaneacionCampo" %>
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
    <a>Planeacion de Campo</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    <a>Formulario que realiza los cálculos requeridos para planear cantidades y cargas en campo</a>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <div id="accordion">
                <div id="accordion00">
                    <h3>
                        <a href="#">
                            <label>
                                Menú de Planeación
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div style="text-align: center">
                                <label>Seleccione el tipo de filtro que desea hacer</label>
                                <div class="actions">
                                    <asp:button ID="Button1" runat="server" Text="Gerencia de Operaciones" />
                                    <asp:button ID="Button2" runat="server" Text="Ciudad" />
                                    <asp:button ID="Button3" runat="server" Text="Metodología" />
                                    <asp:button ID="Button4" runat="server" Text="Unidad" />
                                </div>
                            </div>
                    </div>
                </div>
                <div id="accordion0">
                    <h3>
                        <a href="#">
                            <label>
                                Gerencia de Operaciones<asp:HiddenField ID="hfidTrabajo" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                    <a>Gerencias </a><asp:DropDownList ID="ddlGerencias" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                        <div style="text-align: center">
                        
                                <asp:Chart ID="ChartEncuestasGerencia" runat="server" Palette="Fire" 
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
                                <asp:Chart ID="ChartEncuestadoresGerencia" runat="server" Palette="Fire" 
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
                                Ciudad
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <a>Ciudades</a><asp:DropDownList ID="ddlCiudades" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="Bogotá" Value="11001"></asp:ListItem>
                            <asp:ListItem Text="Medellín" Value="5001"></asp:ListItem>
                            <asp:ListItem Text="Cali" Value="76001"></asp:ListItem>
                            <asp:ListItem Text="Barranquilla" Value="8001"></asp:ListItem>
                            <asp:ListItem Text="Bucaramanga" Value="68001"></asp:ListItem>
                            <asp:ListItem Text="Cartagena" Value="13001"></asp:ListItem>
                            <asp:ListItem Text="Manizales" Value="17001"></asp:ListItem>
                            <asp:ListItem Text="Pereira" Value="66001"></asp:ListItem>
                            <asp:ListItem Text="Ibagué" Value="73001"></asp:ListItem>
                            <asp:ListItem Text="Otras ciudades" Value="900001"></asp:ListItem>
                                        </asp:DropDownList>
                        <div style="text-align: center">
                                <asp:Chart ID="ChartCiudadEncuestas" runat="server" Palette="Fire" 
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
                                    <asp:HiddenField ID="HiddenField3" runat="server" />
                                    <asp:LinkButton ID="LinkButton5" runat="server" OnClientClick="MostrarFactoresDialog()">Ver / Ajustar factor de variación</asp:LinkButton>
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:LinkButton ID="LinkButton6" runat="server" OnClientClick="MostrarDatosDialog()">Ver / Exportar datos</asp:LinkButton>
                                </div>
                                <br />
                                <asp:Chart ID="ChartCiudadEncuestadores" runat="server" Palette="Fire" 
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
                                Metodología
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                    <a>Metodologías </a><asp:DropDownList ID="ddlMetodologias" runat="server" AutoPostBack="true">
                                        </asp:DropDownList>
                        <div style="text-align: center">
                                <asp:Chart ID="ChartMetodologiaEncuestas" runat="server" Palette="Fire" 
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
                                <asp:Chart ID="ChartMetodologiaEncuestadores" runat="server" Palette="Fire" 
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
                                Unidad comercial
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                    <a>Unidades </a><asp:DropDownList ID="ddlUnidades" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                            <asp:ListItem Text="ASI" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Marketing" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Loyalty" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Opinión" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Media" Value="5"></asp:ListItem>
                                        </asp:DropDownList>
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
                <asp:HiddenField ID="hfFiltroDatos" runat="server" />
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
                    SelectCommand="OP_PL_PivotDatosCampo" SelectCommandType="StoredProcedure">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hfFactorDatos" Name="FACTORA" PropertyName="Value" 
                            Type="Double" />
                        <asp:ControlParameter ControlID="hfGraficaDatos" Name="GRAFICA" 
                            PropertyName="Value" Type="Int32" />
                        <asp:ControlParameter ControlID="hfFiltroDatos" Name="FILTRO" 
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
