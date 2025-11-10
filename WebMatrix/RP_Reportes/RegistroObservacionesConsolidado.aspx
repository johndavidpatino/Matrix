<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="RegistroObservacionesConsolidado.aspx.vb" Inherits="WebMatrix.RegistroObservacionesConsolidado" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>

    <link rel="stylesheet" href="https://blacklabel.github.io/grouped_categories/css/styles.css" type="text/css" />
    <script src="https://code.highcharts.com/10.0/highcharts.js" type="text/javascript" language="javascript"></script>
    <script src="https://blacklabel.github.io/grouped_categories/grouped-categories.js" type="text/javascript" language="javascript"></script>
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
    <style>
        .label-buscar {
            padding-top: 11px !important;
            width: 120px !important;
        }

        .panelResultado {
            margin-top: 30px;
            border: none; /*1px #000 solid;*/
            display: block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Indicadores
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Consolidado - Registro de Observaciones
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu" style="float: right;">
        <li>
            <a href="../Home/Default.aspx">IR AL INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
    En este reporte se puede visualizar el consolidado del registro de Observaciones de las tareas.
    <br />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Content" runat="server">
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
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div style="width: 100%; margin: 25px 0; display: block;">
                <div style="width: 32%; float: left; display: block;">
                    <asp:Label ID="lblAno" CssClass="label-buscar" Text="Año" runat="server" AssociatedControlID="ddlAno"></asp:Label>
                    <asp:DropDownList ID="ddlAno" runat="server">
                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                        <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="width: 32%; float: left; display: block;">
                    <asp:Label ID="lblMes" CssClass="label-buscar" Text="Mes" runat="server" AssociatedControlID="ddlMes"></asp:Label>
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
                        <asp:ListItem Text="Septiemnbre" Value="9"></asp:ListItem>
                        <asp:ListItem Text="Octubre" Value="10"></asp:ListItem>
                        <asp:ListItem Text="Noviembre" Value="11"></asp:ListItem>
                        <asp:ListItem Text="Diciembre" Value="12"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="width: 32%; float: left; display: block;">
                    <asp:Label ID="lblUnidad" CssClass="label-buscar" Text="Unidad que Registró" runat="server" AssociatedControlID="ddlUnidades"></asp:Label>
                    <asp:DropDownList ID="ddlUnidades" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="-1">---Seleccione---</asp:ListItem>
                        <asp:ListItem Value="14">Public Affairs</asp:ListItem>
                        <asp:ListItem Value="15">CEX - Customer Experience</asp:ListItem>
                        <asp:ListItem Value="42">Marketing CST3 CG</asp:ListItem>
                        <asp:ListItem Value="43">Marketing CST4 JS</asp:ListItem>
                        <asp:ListItem Value="70">MSU - Market Strategic & Understanding- AG</asp:ListItem>
                        <asp:ListItem Value="71">BHT - Brand Health Tracking</asp:ListItem>
                        <asp:ListItem Value="72">INN - Innovation</asp:ListItem>
                        <asp:ListItem Value="73">CRE - Creative Excellence</asp:ListItem>
                        <asp:ListItem Value="74">MYS - Mystery Shopping</asp:ListItem>
                        <asp:ListItem Value="75">HEC - Healthcare</asp:ListItem>
                        <asp:ListItem Value="77">OBS - Observer</asp:ListItem>
                        <asp:ListItem Value="78">MSU - Market Strategic & Understanding- CG</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="width: 32%; float: left; display: block;">
                    <asp:Label ID="lblTareas" Text="Tareas" CssClass="label-buscar" runat="server" AssociatedControlID="ddlTareas"></asp:Label>
                    <asp:DropDownList ID="ddlTareas" runat="server" AutoPostBack="true">
                        <asp:ListItem Value="-1">---Seleccione---</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="width: 32%; float: left; display: block;">
                    <asp:Label ID="lblInstrumento" Text="Instrumentos" CssClass="label-buscar" runat="server" AssociatedControlID="ddlInstrumentos" Visible="false"></asp:Label>
                    <asp:DropDownList ID="ddlInstrumentos" runat="server" Visible="false">
                        <asp:ListItem Value="-1">---Seleccione---</asp:ListItem>
                        <asp:ListItem Value="9">Cuestionario</asp:ListItem>
                        <asp:ListItem Value="10">Instructivo</asp:ListItem>
                        <asp:ListItem Value="11">Metodología</asp:ListItem>
                        <asp:ListItem Value="12">Tarjetas</asp:ListItem>
                        <asp:ListItem Value="13">Circular</asp:ListItem>
                        <asp:ListItem Value="70">Tracking-Archivo de cambios</asp:ListItem>
                        <asp:ListItem Value="80">Guion de verificación</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="width: 32%; float: left; display: block;">
                    <asp:Label ID="lblUsuario" Text="Usuario" CssClass="label-buscar" runat="server" AssociatedControlID="ddlUsuario" Visible="false"></asp:Label>
                    <asp:DropDownList ID="ddlUsuario" runat="server" Visible="false" AutoPostBack="true">
                        <asp:ListItem Value="-1">---Seleccione---</asp:ListItem> 
                    </asp:DropDownList>
                </div>
                <div style="float: right; display: block;">
                    <asp:Button ID="btnAcualizar" runat="server" Text="Actualizar" />
                </div>
            </div>
            <div class="spacer"></div>
            <asp:Panel runat="server" CssClass="panelResultado">
                <div style="width: 98%; max-height: 300px; overflow-y: auto; margin: 2px 5px;">
                    <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="false" DataKeyNames="Tarea"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        AllowPaging="true" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                            <asp:BoundField DataField="Documento" HeaderText="Documento" />
                            <asp:BoundField DataField="Usuario_Observacion" HeaderText="Usuario Observacion" />
                            <asp:BoundField DataField="Usuario_Tarea" HeaderText="Usuario Tarea" />
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                            <asp:BoundField DataField="Anio" HeaderText="Año" />
                            <asp:BoundField DataField="Mes" HeaderText="Mes" />
                            <asp:BoundField DataField="Adicional" HeaderText="Adicional" />
                            <asp:BoundField DataField="Cambio" HeaderText="Cambio" />
                            <asp:BoundField DataField="Error" HeaderText="Error" />
                            <asp:BoundField DataField="Sugerencia" HeaderText="Sugerencia" />
                            <asp:BoundField DataField="Total" HeaderText="Total" />

                            <%--<asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarProveedores()" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>--%>
                        </Columns>
                    </asp:GridView>
                </div>
            </asp:Panel>
            <%--        
        <div id="accordionData">
            <h2>Datos Agrupados</h2>
            <div style="max-width: 100%; min-height: 300px;">
                <asp:UpdatePanel runat="server" ID="updDatos" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvDatos" runat="server" AutoGenerateColumns="true"></asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>

        <br />
        <div id="accordionDataDet">
            <h2>Detalles</h2>
            <div style="max-width: 100%; min-height: 500px; max-height: 500px;">
                <asp:Button Text="Exportar a Excel" runat="server" ID="btnExcelDetalle" />
                <br />
                <br />
                <br />
                <asp:UpdatePanel runat="server" ID="updDatosDetalle" UpdateMode="Conditional">
                    <ContentTemplate>
                        <label style="text-align: left; width: auto;">Usuario: </label>
                        <asp:DropDownList runat="server" ID="ddlUsuario" AutoPostBack="true">
                        </asp:DropDownList>
                        <br />
                        <asp:GridView ID="gvDatosDetalle" runat="server" AutoGenerateColumns="true"></asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
