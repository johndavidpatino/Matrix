<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="IndicadoresCalidad.aspx.vb" Inherits="WebMatrix.IndicadoresCalidad" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type="text/javascript">
        $(document).ready(function () {
            $('#accordionDataDet').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                fillSpace: true,
                collapsible: true,
                heightStyle: "fill",
                active: false
            });

            $('#accordionData').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                fillSpace: true,
                collapsible: true,
                heightStyle: "fill",
                active: false
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Indicadores
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu" style="float: right;">
        <li>
            <a href="../Home/Default.aspx">IR AL INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Titulo" runat="server">
    REPORTES DE CALIDAD
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
    Indicador de Esquema de Análisis - Porcentaje de Diligenciamiento del Brief - Envío de Propuestas 48 Horas (Brief vs Propuesta)
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
    <div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-info" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Info: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lblTextInfo">
            </label>
        </div>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Error: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lbltextError">
            </label>
        </div>
    </div>

    <asp:Label ID="lblReporte" Text="Reporte" runat="server" AssociatedControlID="ddlReporte" CssClass="lblEncabezado"></asp:Label>
    <asp:DropDownList ID="ddlReporte" runat="server" Width="350" AutoPostBack="true">
        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
        <asp:ListItem Text="Indicador de Esquema de Análisis" Value="1"></asp:ListItem>
        <asp:ListItem Text="Porcentaje de Diligenciamiento del Brief" Value="2"></asp:ListItem>
        <asp:ListItem Text="Envío de Propuestas 48 Horas (Brief vs Propuesta)" Value="3"></asp:ListItem>
    </asp:DropDownList>
    <div class="row"></div>
    <asp:Label ID="lblAno" Text="Año" runat="server" AssociatedControlID="ddlAno" CssClass="lblEncabezado"></asp:Label>
    <asp:DropDownList ID="ddlAno" runat="server">
    </asp:DropDownList>
    <asp:Label ID="lblMes" Text="Mes" runat="server" AssociatedControlID="ddlMes" CssClass="lblEncabezado"></asp:Label>
    <asp:DropDownList ID="ddlMes" runat="server">
        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
        <asp:ListItem Text="Enero" Value="1"></asp:ListItem>
        <asp:ListItem Text="Febrero" Value="2"></asp:ListItem>
        <asp:ListItem Text="Marzo" Value="3"></asp:ListItem>
        <asp:ListItem Text="Abril" Value="4"></asp:ListItem>
        <asp:ListItem Text="Mayo" Value="5"></asp:ListItem>
        <asp:ListItem Text="Junio" Value="6"></asp:ListItem>
        <asp:ListItem Text="Julio" Value="7"></asp:ListItem>
        <asp:ListItem Text="Agosto" Value="8"></asp:ListItem>
        <asp:ListItem Text="Septiembre" Value="9"></asp:ListItem>
        <asp:ListItem Text="Octubre" Value="10"></asp:ListItem>
        <asp:ListItem Text="Noviembre" Value="11"></asp:ListItem>
        <asp:ListItem Text="Diciembre" Value="12"></asp:ListItem>
    </asp:DropDownList>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Label ID="lblEstados" Text="Estados" runat="server" AssociatedControlID="lblEstados" CssClass="lblEncabezado"></asp:Label>
            <asp:DropDownList ID="ddlEstados" runat="server">
            </asp:DropDownList>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddlReporte" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
    <div class="row"></div>
    <div style="float: right;">
        <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" />
    </div>
    <div class="row"></div>
    <asp:Panel runat="server" ID="pnlResultados" Visible="false">
        <div id="accordionData">
            <h2>Datos Agrupados</h2>
            <div style="max-width: 100%; min-height: 300px;">
                <asp:UpdatePanel runat="server" ID="updDatos" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="true" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <br />
        <div id="accordionDataDet">
            <h2>Detalles</h2>
            <div style="max-width: 100%; min-height: 500px; max-height: 500px;">
                <asp:Button ID="btnDescargar" Text="Exportar a Excel" runat="server" Visible="false" />
                <br />
                <br />
                <br />
                <asp:UpdatePanel runat="server" ID="updDatosDetalle" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label style="text-align: left; width: auto;">Gerente de Cuentas: </label>
                        <asp:DropDownList runat="server" ID="ddlUsuario" AutoPostBack="true">
                        </asp:DropDownList>
                        <br />
                        <asp:GridView ID="gvDatosDetalle" runat="server" AutoGenerateColumns="true" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
