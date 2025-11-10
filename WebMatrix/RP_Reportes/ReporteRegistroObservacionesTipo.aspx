<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterREP.master" CodeBehind="ReporteRegistroObservacionesTipo.aspx.vb" Inherits="WebMatrix.ReporteRegistroObservacionesTipo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>

    <link rel="stylesheet" href="https://blacklabel.github.io/grouped_categories/css/styles.css" type="text/css" />
    <script src="https://code.highcharts.com/10.0/highcharts.js" type="text/javascript" language="javascript"></script>
    <script src="https://blacklabel.github.io/grouped_categories/grouped-categories.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#accordion1').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                fillSpace: true,
                collapsible: true,
                heightStyle: "fill",
                active: false
            });

            $('#accordion0').accordion({
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
        .divFiltro {
            border: none;
            margin: 0px;
            padding: 0px;
            width: 33%;
            float: left;
        }

        .lblFiltro {
            float: left !important;
            width: 80px !important;
            margin: 5px !important;
        }

        .cajaFiltro {
            border: none; /*1px #000000 solid;*/
            padding: 0px;
        }

        .btnFiltro {
            float: right !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Indicadores
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Registros de Observaciones por Tipo
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
    <br />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Content" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#tabs').tabs();

            $('#accordion1').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                fillSpace: true,
                collapsible: true,
                heightStyle: "fill",
                active: true
            });

            $('#accordion0').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                fillSpace: true,
                collapsible: true,
                heightStyle: "fill",
                active: true
            });
        }
    </script>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div class="col-md-12 cajaFiltro">
                <div class="divFiltro">
                    <asp:Label ID="lblAno" Text="Año" runat="server" AssociatedControlID="ddlAno" CssClass="lblFiltro"></asp:Label>
                    <asp:DropDownList ID="ddlAno" runat="server">
                    </asp:DropDownList>
                </div>
                <div class="divFiltro">
                    <asp:Label ID="lblMes" Text="Mes" runat="server" AssociatedControlID="ddlMes" CssClass="lblFiltro"></asp:Label>
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
                <div class="divFiltro">
                    <asp:Label ID="lblTipo" Text="Tipo" runat="server" AssociatedControlID="ddlTipos" CssClass="lblFiltro"></asp:Label>
                    <asp:DropDownList ID="ddlTipos" runat="server">
                        <asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
                        <asp:ListItem Text="Adicional" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Cambio" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Error" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Sugerencia" Value="4"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divFiltro">
                    <asp:Label ID="lblTareas" Text="Tareas" runat="server" AssociatedControlID="ddlTareas" CssClass="lblFiltro"></asp:Label>
                    <asp:DropDownList ID="ddlTareas" runat="server" AutoPostBack="true">
                        <asp:ListItem Text="Total" Value=""></asp:ListItem>
                        <asp:ListItem Text="Instrumentos" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Codificación" Value="8"></asp:ListItem>
                        <asp:ListItem Text="PDC" Value="12"></asp:ListItem>
                        <asp:ListItem Text="Procesamiento Total" Value="13"></asp:ListItem>
                        <asp:ListItem Text="Procesamiento - Control Interno" Value="78"></asp:ListItem>
                        <asp:ListItem Text="Elaboración Informe" Value="16"></asp:ListItem>
                        <asp:ListItem Text="Scripting" Value="20"></asp:ListItem>
                        <asp:ListItem Text="Scripting - Control Interno" Value="76"></asp:ListItem>
                        <asp:ListItem Text="Estadistica-Metodología" Value="23"></asp:ListItem>
                        <asp:ListItem Text="Evaluación variables de control Proyectos" Value="65"></asp:ListItem>
                        <asp:ListItem Text="Evaluación variables de control OMP" Value="66"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="divFiltro">
                    <asp:Label ID="lblInstrumento" Text="Instrumentos" runat="server" AssociatedControlID="ddlInstrumentos" Visible="false" CssClass="lblFiltro"></asp:Label>
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
                <div class="divFiltro">
                    <asp:Label ID="lblVerPor" Text="Ver por:" runat="server" AssociatedControlID="ddlVerPor" CssClass="lblFiltro"></asp:Label>
                    <asp:DropDownList ID="ddlVerPor" runat="server">
                        <asp:ListItem Text="Tarea" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Tipo" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Persona" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-12 cajaFiltro">
                <asp:Button ID="btnAcualizar" runat="server" Text="Actualizar" CssClass="btnFiltro" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <div style="clear: both;"></div>

    <%--    <asp:UpdatePanel runat="server" ID="updGrafica" UpdateMode="Conditional">
        <ContentTemplate>
            <div id="contenedorGrafica" runat="server">
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />--%>
    <div style="margin-top: 30px">
        <div id="accordion0">
            <h2>Datos Agrupados</h2>
            <div style="max-width: 100%; min-height: 300px;">
                <asp:UpdatePanel runat="server" ID="updDatos" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="Key" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Key" HeaderText="Filtro" />
                                <asp:BoundField DataField="Mes" HeaderText="Mes" />
                                <asp:BoundField DataField="KeyMes" HeaderText="Conteo Mes" />
                                <asp:BoundField DataField="TotalKey" HeaderText="Total" />
                                <asp:BoundField DataField="Porcentaje" HeaderText="Porcentaje" DataFormatString="{0:P0}" />
                            </Columns>
                        </asp:GridView> 
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <br />
        <div id="accordion1">
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
        </div>
    </div>
</asp:Content>
