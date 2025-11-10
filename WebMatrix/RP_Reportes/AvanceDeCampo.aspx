<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterReportes.master"
    CodeBehind="AvanceDeCampo.aspx.vb" Inherits="WebMatrix.AvanceDeCampo" %>

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
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    <a>Informe de Avance de Campo</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <asp:UpdatePanel ID="upDatos" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" id="accordion">
                <div>
                    <h3 style="float: left; text-align: left;">
                                Avance General<asp:HiddenField ID="hfidTrabajo" runat="server" />
                    </h3>
                    <div>
                            <div style="text-align: center">
                                <asp:Chart ID="ChartEstado" runat="server" Palette="Excel" BackColor="#F3DFC1" Width="412px"
                                    Height="296px" BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2"
                                    BorderColor="181, 64, 1">
                                    <Titles>
                                        <asp:Title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3"
                                            Text="Avance de Campo" Name="Title1" ForeColor="26, 59, 105">
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
                                <asp:Label ID="lblVariacion" runat="server" Visible="false" ></asp:Label>
                                <asp:Label ID="lblNoData" runat="server" Visible="false" >No se han realizado ninguna estimación de producción aún, por lo cual no es posible mostrar datos. Por favor comuníquese con el OMP</asp:Label>
                            </div>
                    </div>
                    <%--items--%>
                </div>
                <div id="accordion1">
                    <h3>
                        <a href="#">
                            <label>
                                Avance Por Ciudad
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <asp:GridView ID="gvAvanceCiudad" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="MPIO" HeaderText="Ciudad" />
                                    <asp:BoundField DataField="Realizadas" HeaderText="Realizadas" />
                                    <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                    <asp:BoundField DataField="Avance" HeaderText="Avance" DataFormatString="{0:P2}" />
                                    <asp:BoundField DataField="Variacion" HeaderText="Variación" DataFormatString="{0:P2}" Visible="false" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="accordion2">
                    <h3>
                        <a href="#">
                            <label>
                                Avance de Cuotas por Ciudad
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <p>
                                Seleccione la variable por la que desea ver el avance&nbsp;&nbsp;
                                <asp:DropDownList ID="ddVariables" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDsAvanceCuotasPreg"
                                    DataTextField="Pr_Nombre" DataValueField="Pr_Id" AutoPostBack="True">
                                    <asp:ListItem Text="" Value="0" Selected="True"></asp:ListItem>
                                </asp:DropDownList>
                            </p>
                            <asp:GridView ID="gvAvanceCuotas" runat="server" Width="100%" AutoGenerateColumns="True" DataSourceID="SqlDsAvanceCuotas"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen datos para cruzar o bien no se encuentran almacenadas las etiquetas para esta pregunta">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDsAvanceCuotasPreg" runat="server" ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>"
                                SelectCommand="SELECT Pr_Id, Pr_Nombre FROM GestionCampo.dbo.DescripcionCamposPreguntas INNER JOIN GestionCampo.dbo.Preguntas ON Preguntas.DCP_Id=DescripcionCamposPreguntas.DCP_Id WHERE Preguntas.DCP_Id&gt;6 AND E_Id=@CAP">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hfidTrabajo" Name="CAP" PropertyName="Value" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            <asp:SqlDataSource ID="SqlDsAvanceCuotas" runat="server" ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>"
                                SelectCommand="REP_AvancePorVariables" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hfidTrabajo" Name="TrabajoId" PropertyName="Value" />
                                    <asp:ControlParameter ControlID="ddVariables" Name="IDPregunta" PropertyName="SelectedValue"
                                        Type="Decimal" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </div>
                <div id="accordion3">
                    <h3>
                        <a href="#">
                            <label>
                                Avance de Cuotas Cruzado por Variables
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <table style="width: 70%; margin: 0 auto; border-width: thin; border-style: solid;">
                                <tr>
                                    <td>
                                    </td>
                                    <td style="border-width: thin; border-style: dashed;">
                                        <p>
                                            Seleccione la variable de columna&nbsp;&nbsp;
                                            <asp:DropDownList ID="ddColumna" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDsAvanceCuotasPreg"
                                                DataTextField="Pr_Nombre" DataValueField="Pr_Id" AutoPostBack="True">
                                                <asp:ListItem Text="" Value="0" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </p>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-width: thin; border-style: dashed;">
                                        <p>
                                            Seleccione la variable de fila&nbsp;&nbsp;
                                            <asp:DropDownList ID="ddFila" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDsAvanceCuotasPreg"
                                                DataTextField="Pr_Nombre" DataValueField="Pr_Id" AutoPostBack="True">
                                                <asp:ListItem Text="" Value="0" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </p>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                            <asp:GridView ID="gvAvanceCuotasCruce" runat="server" Width="100%" AutoGenerateColumns="True" DataSourceID="SqlDsAvanceCruce"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                            </asp:GridView>
                            <asp:SqlDataSource ID="SqlDsAvanceCruce" runat="server" ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>"
                                SelectCommand="REP_AvancePorVariablesFilas" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hfidTrabajo" Name="TrabajoId" PropertyName="Value" />
                                    <asp:ControlParameter ControlID="ddColumna" Name="IDPregunta" PropertyName="SelectedValue"
                                        Type="Decimal" />
                                    <asp:ControlParameter ControlID="ddFila" Name="IDPregunta2" PropertyName="SelectedValue"
                                        Type="Decimal" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                        </div>
                    </div>
                </div>
                <div id="accordion4">
                    <h3>
                        <a href="#">
                            <label>
                                Avance de Áreas
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <asp:GridView ID="gvAvanceAreas" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                    <asp:BoundField DataField="RMC" HeaderText="RMC" />
                                    <asp:BoundField DataField="Critica" HeaderText="Critica" />
                                    <asp:BoundField DataField="Verificacion" HeaderText="Verificacion" />
                                    <asp:BoundField DataField="Captura" HeaderText="Captura" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="accordion5">
                    <h3>
                        <a href="#">
                            <label>
                                Remanentes en áreas
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <asp:GridView ID="gvRemanentes" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="RMC" HeaderText="RMC" />
                                    <asp:BoundField DataField="Critica" HeaderText="Critica" />
                                    <asp:BoundField DataField="Verificacion" HeaderText="Verificacion" />
                                    <asp:BoundField DataField="Captura" HeaderText="Captura" />
                                    <asp:BoundField DataField="AnuladasCritica" HeaderText="Anuladas Crítica" />
                                    <asp:BoundField DataField="AnuladasVerificacion" HeaderText="Anuladas Verificación" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="accordion6">
                    <h3>
                        <a href="#">
                            <label>
                                Encuestas Anuladas
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <asp:GridView ID="gvAnuladas" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen encuestas anuladas">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="NumeroEncuesta" HeaderText="No." />
                                    <asp:BoundField DataField="Observacion" HeaderText="Observación o Razón" />
                                    <asp:BoundField DataField="Encuestador" HeaderText="Encuestador" />
                                    <asp:BoundField DataField="Supervisor" HeaderText="Supervisor" />
                                    <asp:BoundField DataField="FechaEncuesta" HeaderText="Fecha Encuesta" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="AnuladoPor" HeaderText="Anulado Por" />
                                    <asp:BoundField DataField="FechaAnulacion" HeaderText="Fecha y Hora" />
                                    <asp:BoundField DataField="Unidad" HeaderText="Detectada en" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="accordion7">
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
            GridLines="Vertical" DataKeyNames="Res_Id">
            <AlternatingRowStyle BackColor="#DCDCDC" />
            <Columns>
                <asp:BoundField DataField="Res_Id" HeaderText="Res_Id" InsertVisible="False" 
                    ReadOnly="True" SortExpression="Res_Id" />
                <asp:BoundField DataField="E_Id" HeaderText="E_Id" SortExpression="E_Id" />
                <asp:BoundField DataField="Res_Numero" HeaderText="Res_Numero" 
                    SortExpression="Res_Numero" />
                <asp:BoundField DataField="Per_NumIdentificacionEncu" 
                    HeaderText="Per_NumIdentificacionEncu" 
                    SortExpression="Per_NumIdentificacionEncu" />
                <asp:BoundField DataField="Per_NumIdentificacionSup" 
                    HeaderText="Per_NumIdentificacionSup" 
                    SortExpression="Per_NumIdentificacionSup" />
                <asp:BoundField DataField="Res_IDM" HeaderText="Res_IDM" 
                    SortExpression="Res_IDM" />
                <asp:BoundField DataField="Res_Ciudad" HeaderText="Res_Ciudad" 
                    SortExpression="Res_Ciudad" />
                <asp:BoundField DataField="Res_Fecha" HeaderText="Res_Fecha" DataFormatString="{0:d}" 
                    SortExpression="Res_Fecha" />
                <asp:BoundField DataField="Res_Libre1" HeaderText="Res_Libre1" 
                    SortExpression="Res_Libre1" />
                <asp:BoundField DataField="Res_Libre2" HeaderText="Res_Libre2" 
                    SortExpression="Res_Libre2" />
                <asp:BoundField DataField="Res_Libre3" HeaderText="Res_Libre3" 
                    SortExpression="Res_Libre3" />
                <asp:BoundField DataField="Res_Libre4" HeaderText="Res_Libre4" 
                    SortExpression="Res_Libre4" />
                <asp:BoundField DataField="Res_Libre5" HeaderText="Res_Libre5" 
                    SortExpression="Res_Libre5" />
                <asp:BoundField DataField="Res_Libre6" HeaderText="Res_Libre6" 
                    SortExpression="Res_Libre6" />
                <asp:BoundField DataField="Res_Libre7" HeaderText="Res_Libre7" 
                    SortExpression="Res_Libre7" />
                <asp:BoundField DataField="Res_Libre8" HeaderText="Res_Libre8" 
                    SortExpression="Res_Libre8" />
                <asp:BoundField DataField="Res_Libre9" HeaderText="Res_Libre9" 
                    SortExpression="Res_Libre9" />
                <asp:BoundField DataField="Res_Libre10" HeaderText="Res_Libre10" 
                    SortExpression="Res_Libre10" />
                 <asp:BoundField DataField="Res_Libre11" HeaderText="Res_Libre11" 
                    SortExpression="Res_Libre11" />
                <asp:BoundField DataField="Res_Libre12" HeaderText="Res_Libre12" 
                    SortExpression="Res_Libre12" />
                <asp:BoundField DataField="Res_Libre13" HeaderText="Res_Libre13" 
                    SortExpression="Res_Libre13" />
                <asp:BoundField DataField="Res_Libre14" HeaderText="Res_Libre14" 
                    SortExpression="Res_Libre14" />
                <asp:BoundField DataField="Res_Libre15" HeaderText="Res_Libre15" 
                    SortExpression="Res_Libre15" />
                <asp:BoundField DataField="Res_Libre16" HeaderText="Res_Libre16" 
                    SortExpression="Res_Libre16" />
                <asp:BoundField DataField="Res_Libre17" HeaderText="Res_Libre17" 
                    SortExpression="Res_Libre17" />
                <asp:BoundField DataField="Res_Libre18" HeaderText="Res_Libre18" 
                    SortExpression="Res_Libre18" />
                <asp:BoundField DataField="Res_Libre19" HeaderText="Res_Libre19" SortExpression="Res_Libre19" />
                <asp:BoundField DataField="Res_Libre20" HeaderText="Res_Libre20" 
                    SortExpression="Res_Libre20" />
                 <asp:BoundField DataField="Res_Libre21" HeaderText="Res_Libre21" 
                    SortExpression="Res_Libre21" />
                <asp:BoundField DataField="Res_Libre22" HeaderText="Res_Libre22" 
                    SortExpression="Res_Libre22" />
                <asp:BoundField DataField="Res_Libre23" HeaderText="Res_Libre23" 
                    SortExpression="Res_Libre23" />
                <asp:BoundField DataField="Res_Libre24" HeaderText="Res_Libre24" 
                    SortExpression="Res_Libre24" />
                <asp:BoundField DataField="Res_Libre25" HeaderText="Res_Libre25" 
                    SortExpression="Res_Libre25" />
                <asp:BoundField DataField="Res_Libre26" HeaderText="Res_Libre26" 
                    SortExpression="Res_Libre26" />
                <asp:BoundField DataField="Res_Libre27" HeaderText="Res_Libre27" 
                    SortExpression="Res_Libre27" />
                <asp:BoundField DataField="Res_Libre28" HeaderText="Res_Libre28" 
                    SortExpression="Res_Libre28" />
                <asp:BoundField DataField="Res_Libre29" HeaderText="Res_Libre29" SortExpression="Res_Libre29" />
                <asp:BoundField DataField="Res_Libre30" HeaderText="Res_Libre30" 
                    SortExpression="Res_Libre30" />
                <asp:BoundField DataField="Usuario" HeaderText="Usuario" 
                    SortExpression="Usuario" />
                <asp:BoundField DataField="Fecha" HeaderText="Fecha" SortExpression="Fecha" DataFormatString="{0:d}" />
                <asp:BoundField DataField="Anulada" HeaderText="Anulada" ReadOnly="True" 
                    SortExpression="Anulada" />
                <asp:BoundField DataField="Obs_Anulacion" HeaderText="Obs_Anulacion" 
                    SortExpression="Obs_Anulacion" />
                <asp:BoundField DataField="Per_anulo" HeaderText="Per_anulo" 
                    SortExpression="Per_anulo" />
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
            
            SelectCommand="SELECT Res_Id, E_Id, Res_Numero, Per_NumIdentificacionEncu, Per_NumIdentificacionSup, Res_IDM, Res_Ciudad, Res_Fecha, Res_Libre1, Res_Libre2, Res_Libre3, Res_Libre4, Res_Libre5, Res_Libre6, Res_Libre7, Res_Libre8, Res_Libre9, Res_Libre10, Res_Libre11, Res_Libre12, Res_Libre13, Res_Libre14, Res_Libre15, Res_Libre16, Res_Libre17, Res_Libre18, Res_Libre19, Res_Libre20, Res_Libre21, Res_Libre22, Res_Libre23, Res_Libre24, Res_Libre25, Res_Libre26, Res_Libre27, Res_Libre28, Res_Libre29, Res_Libre30, Usuario, Fecha, Anulada=CASE WHEN Anulada='TRUE' THEN 1 ELSE 0 END, Obs_Anulacion, Per_anulo FROM GestionCampo.dbo.Respuestas WHERE (E_Id = @CAP)">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfidTrabajo" Name="CAP" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>
                        </div>
                    </div>
                </div>
                <div id="accordion8">
                    <h3>
                        <a href="#">
                            <label>
                                Matriz<asp:HiddenField ID="HiddenField1" runat="server" />
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <asp:GridView ID="gvMatriz" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable2" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="CAMPOP" HeaderText="&#8494;" />
                                    <asp:BoundField DataField="CAMPOP_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="CAMPOE" HeaderText="#"  />
                                    <asp:BoundField DataField="CAMPOE_S" HeaderText="&sum;"  />
                                    <asp:BoundField DataField="CAMPO_DIF" HeaderText="< >" />
                                    <asp:BoundField DataField="RMCP" HeaderText="&#8494;" />
                                    <asp:BoundField DataField="RMCP_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="RMCE" HeaderText="#" />
                                    <asp:BoundField DataField="RMCE_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="RMC_DIF" HeaderText="< >" />
                                    <asp:BoundField DataField="CRITICAP" HeaderText="&#8494;" />
                                    <asp:BoundField DataField="CRITICAP_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="CRITICAE" HeaderText="#" />
                                    <asp:BoundField DataField="CRITICAE_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="CRITICA_DIF" HeaderText="< >" />
                                    <asp:BoundField DataField="VERIFICACIONP" HeaderText="&#8494;" />
                                    <asp:BoundField DataField="VERIFICACIONP_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="VERIFICACIONE" HeaderText="#" />
                                    <asp:BoundField DataField="VERIFICACIONE_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="VERIFICACION_DIF" HeaderText="< >" />
                                    <asp:BoundField DataField="CAPTURAP" HeaderText="&#8494;" />
                                    <asp:BoundField DataField="CAPTURAP_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="CAPTURAE" HeaderText="#" />
                                    <asp:BoundField DataField="CAPTURAE_S" HeaderText="&sum;" />
                                    <asp:BoundField DataField="CAPTURA_DIF" HeaderText="< >" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="accordion9">
                    <h3>
                        <a href="#">
                            <label>
                                TraficoAreas
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <div class="actions">
                            <asp:DropDownList ID="ddlAreasTrafico" runat="server" AutoPostBack="true">
                                <asp:ListItem Text="-- Seleccione un área --" Value="0"></asp:ListItem>
                                <asp:ListItem Text="RMC" Value="38"></asp:ListItem>
                                <asp:ListItem Text="Crítica" Value="28"></asp:ListItem>
                                <asp:ListItem Text="Verificación" Value="20"></asp:ListItem>
                                <asp:ListItem Text="Captura" Value="21"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:GridView ID="gvTraficoAreas" runat="server" Width="100%" AutoGenerateColumns="False"
                                CssClass="displayTable2" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="FECHA" HeaderText="Fecha" DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="RECIBIDO" HeaderText="Recibido" />
                                    <asp:BoundField DataField="ACUMRECIBIDO" HeaderText="Acumulado Recibido" />
                                    <asp:BoundField DataField="ENVIADO" HeaderText="Enviado"  />
                                    <asp:BoundField DataField="ACUMENVIADO" HeaderText="Acumulado Enviado"  />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <div id="accordion10">
                    <h3>
                        <a href="#">
                            <label>
                                Encuestadores participantes en el estudio
                            </label>
                        </a>
                    </h3>
                    <div class="block">
                        <asp:GridView ID="gvEncuestadores" runat="server" AutoGenerateColumns="False" CellSpacing="4" CssClass="displayTable2" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar" DataKeyNames="Per_NumIdentificacionEncu"
            CellPadding="4" DataSourceID="SqlDsEncuestadores" ForeColor="#333333" 
            GridLines="None">
            <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
            <Columns>
                <asp:BoundField DataField="Per_NumIdentificacionEncu" HeaderText="CC" 
                    SortExpression="Per_NumIdentificacionEncu" />
                <asp:BoundField DataField="Per_Nombre" HeaderText="Nombre" 
                    SortExpression="Per_Nombre" />
                <asp:TemplateField HeaderText="Ver Ficha" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                    ToolTip="Ir a la ficha del encuestador" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
            </Columns>
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
        <asp:SqlDataSource ID="SqlDsEncuestadores" runat="server"
        ConnectionString="<%$ ConnectionStrings:MatrixConnectionString %>" 
            
            SelectCommand="SELECT DISTINCT R.Per_NumIdentificacionEncu, P.Per_Nombre from GestionCampo..Respuestas R inner join GestionCampo..Personas P on r.Per_NumIdentificacionEncu = P.Per_NumIdentificacion WHERE (E_Id = @CAP)">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfidTrabajo" Name="CAP" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>
                    </div>
                </div>
                
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
