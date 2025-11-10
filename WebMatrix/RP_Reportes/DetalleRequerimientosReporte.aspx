<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="DetalleRequerimientosReporte.aspx.vb" Inherits="WebMatrix.DetalleRequerimientosReporte" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        function loadPlugins() {
        }

        $(document).ready(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPicker);
            bindPicker();


            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $('#PresupuestosAsignadosXEstudio').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Presupuestos asignados",
                    width: "600px",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });

        });

        function bindPicker() {
            $(".input-fecha").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function MostrarPresupuestosAsignadosXEstudio() {
            $('#PresupuestosAsignadosXEstudio').dialog("open");
        }

        function ActualizarPresupuestosAsignados(rowIndex, checked) {

            if (checked == true) {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() + ";" + rowIndex + ";");
            }
            else {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val().replace(";" + rowIndex + ";", ""));
            }
        }

        $(function () {
            $("#gvReporte").dialog({
                autoOpen: false,
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                }
            });

            $("#btnCerrarTrabajo").click(function () {
                $("#gvReporte").dialog("open");
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu" style="float: right;">
        <li>
            <a href="../RP_Reportes/DefaultMenu.aspx">REPORTES  </a>
        </li>
        <li>
            <a href="../Home/Default.aspx">INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Radicación y Aprobación
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Detalle de Radicación y Aprobación
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
    Detalle de Radicación y Aprobación de Ordenes de Servicio, de Compra y Requerimientos de Servicio
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
    <style>
        #stylized label {
            margin: 0;
            color: #1c1c1c;
            font-family: 'Roboto', sans-serif;
            font-size: 13px;
            width: 100%;
            text-align: left;
        }

        #stylized input, #stylized select {
            margin: 0px;
            width: 100%;
            padding: 0px;
        }

        #stylized select {
            padding: 6px 12px;
            height: 28px;
        }

        #stylized input[type=submit] {
            font-size: 12px;
        }
    </style>
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
    <asp:UpdatePanel ID="upBuscar" runat="server">
        <ContentTemplate>
            <div id="filtros" class="row" style="margin-top: 15px;">
                <div class="col-md-12">
                    <div class="panel panel-default">
                        <div class="panel-heading">
                            <h3 style="padding: 0px;">Filtro de Ordenes/Requerimientos</h3>
                        </div>
                        <div class="panel-body">
                            <div class="row"></div>
                            <div class="form-group">
                                <div class="col-md-3">
                                    <label for="txtFechaInicio">ID Trabajo:</label>
                                    <div class="col-sm-12" style="padding-left: 0px;">
                                        <asp:TextBox runat="server" ID="txtIdTrabajo" CssClass="form-control" />
                                    </div>
                                </div>
                                <div class="col-md-6">
                                    <div class="col-md-3" style="margin-top: 20px;">
                                        <asp:Button ID="btnBuscar" Text="Buscar" runat="server" CssClass="btn btn-primary" />
                                        <asp:HiddenField ID="hfNumRequerimiento" runat="server" Value="0" />
                                    </div>
                                    <div class="col-md-3 col-md-offset-1" style="margin-top: 20px;">
                                        <asp:Button ID="btnExportar" Text="Exportar" runat="server" CssClass="btn btn-primary" Visible="false" />
                                    </div>
                                </div>
                            </div>
                            <div class="row"></div>
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upOrdenesFacturas" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row" style="margin-top: 15px;">
                <div class="col-md-12" style="overflow-x: scroll;">
                    <asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="true" HeaderStyle-CssClass="cabeceraTabla"
                        CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="" />
                        <PagerTemplate>
                            <div class="pagingButtons">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                Enabled='<%# IIf(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                Enabled='<%# IIf(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td>
                                            <span class="pagingLinks">[<asp:Label ID="lblPaginaActual" runat="server"></asp:Label>-<asp:Label ID="lblCantidadPaginas" runat="server"></asp:Label>]</span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true") %>'
                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                Enabled='<%# IIf((gvDatos.PageIndex + 1) = gvDatos.PageCount, "false", "true") %>'
                                                SkinID="paging">Ultimo »</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>--%>
</asp:Content>
