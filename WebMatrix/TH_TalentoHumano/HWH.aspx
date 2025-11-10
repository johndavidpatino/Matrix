<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="HWH.aspx.vb" Inherits="WebMatrix.HWH" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" rel="stylesheet" integrity="sha384-wvfXpqpZZVQGK6TAh5PVlGOfQNHSoD2xbE+QkPxCAFlNEevoEH3Sl0sibVcOQVnN" crossorigin="anonymous">

    <script src="https://code.highcharts.com/gantt/highcharts-gantt.js"></script>
    <script src="https://code.highcharts.com/stock/modules/stock.js"></script>
    <script src="https://code.highcharts.com/gantt/modules/exporting.js"></script>
    <script>
        function loadPlugins() {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            var hoy = new Date();
            var fechaMin = null;
            var fechaMax = null;
            fechaMin = new Date();

            if (hoy.getMonth() == 10) {
                var fechaMax = new Date(hoy.getFullYear() + 2, 0, 0);
            } else {
                fechaMax = new Date(hoy.getFullYear(), hoy.getMonth() + 2, 0);
            }

            function convert(str) {
                var date = new Date(str),
                    mnth = ("0" + (date.getMonth() + 1)).slice(-2),
                    day = ("0" + date.getDate()).slice(-2);
                return [day, mnth, date.getFullYear()].join("/");
            }

            var festivos = ["4/18/2019", "4/19/2019", "5/1/2019", "6/3/2019", "6/24/2019", "7/1/2019", "7/20/2019", "8/7/2019", "8/19/2019", "10/14/2019", "11/4/2019", "11/11/2019", "12/8/2019", "12/25/2019"];
            
            $("#CPH_Content_txtFecha").mask("99/99/9999");
            $("#CPH_Content_txtFecha").datepicker({
                dateFormat: 'dd/mm/yy',
                minDate: convert(fechaMin.toString()),
                maxDate: convert(fechaMax.toString()),
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                beforeShowDay: function (date) {
                    show = true;
                    if (date.getDay() == 0 || date.getDay() == 6) { show = false; }//No Fines de Semana
                    for (var i = 0; i < festivos.length; i++) {
                        if (new Date(festivos[i]).toString() == date.toString()) { show = false; }//No Festivos
                    }
                    var display = [show, '', (show) ? '' : 'No Fines de Semana ni Festivos'];//With Fancy hover tooltip!
                    return display;
                }
            });


            $(".FechaSelect").mask("99/99/9999");
            $(".FechaSelect").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                beforeShowDay: function (date) {
                    show = true;
                    if (date.getDay() == 0 || date.getDay() == 6) { show = false; }//No Fines de Semana
                    for (var i = 0; i < festivos.length; i++) {
                        if (new Date(festivos[i]).toString() == date.toString()) { show = false; }//No Festivos
                    }
                    var display = [show, '', (show) ? '' : 'No Fines de Semana ni Festivos'];//With Fancy hover tooltip!
                    return display;
                }
            });
        }

        $(document).ready(function () {
            loadPlugins();
        });

        function activarHistoricoHWH() {
            $('#accordion').accordion({
                active: 1
            });
        }

        function BusquedaHWH() {
            $('#accordion').accordion({
                active: 2
            });
        }
        function modificarMesFecha(fecha) {
            var initial = fecha.split(/\//);
            return ([initial[1], initial[0], initial[2]].join('/'));
        }
        function cargarGantt(data) {
            var today = new Date(),
                day = 1000 * 60 * 60 * 24,
                // Utility functions
                dateFormat = Highcharts.dateFormat,
                defined = Highcharts.defined,
                isObject = Highcharts.isObject,
                reduce = Highcharts.reduce;

            // Set to 00:00:00:000 today
            today.setUTCHours(0);
            today.setUTCMinutes(0);
            today.setUTCSeconds(0);
            today.setUTCMilliseconds(0);
            today = today.getTime();

            var datos = [{
                name: 'Listado de TeleTrabajo',
                id: 'Listado_TeleTrabajo'
            }];
            data.series.forEach(function (item) {
                var s = {
                    name: item.name,
                    descripcion: item.descripcion,
                    id: item.id,
                    start: new Date(modificarMesFecha(item.fstart + " 00:00:00")).getTime(),
                    end: new Date(modificarMesFecha(item.fend + " 23:59:59")).getTime(),
                    //completed: {
                    //    amount: (item.porcentaje * 100) + '%' || null
                    //},
                    owner: item.owner || 'Sin Asignar',
                    estado: item.estado
                };
                datos.push(s);
            });
            var ListaSerie = ([{
                name: 'Easy Work',
                data: datos
            }]);

            var options = {
                series: ListaSerie,
                yAxis: {
                    visible: false,
                    uniqueNames: true
                },
                tooltip: {
                    pointFormatter: function () {
                        var point = this,
                            format = '%e. %b',
                            options = point.options,
                            completed = options.completed,
                            amount = isObject(completed) ? completed.amount : completed,
                            status = ((amount || 0) * 100) + '%',
                            lines;

                        lines = [{
                            title: 'Descripción',
                            value: point.descripcion,
                            style: 'font-weight: bold;'
                        }, {
                            title: 'Día Solicitado',
                            value: dateFormat(format, point.start)
                            //}, {
                            //    visible: !options.milestone,
                            //    title: 'Final',
                            //    value: dateFormat(format, point.end)
                            //}, {
                            //    title: 'Completada',
                            //    value: status
                        }, {
                            title: 'Solicitada',
                            value: options.name || 'Sin Asignar'
                        }, {
                            title: 'Estado',
                            value: options.estado
                        }];

                        return reduce(lines, function (str, line) {
                            var s = '',
                                style = (
                                    defined(line.style) ? line.style : 'font-size: 0.8em;'
                                );
                            if (line.visible !== false) {
                                s = (
                                    '<span style="' + style + '">' +
                                    (defined(line.title) ? line.title + ': ' : '') +
                                    (defined(line.value) ? line.value : '') +
                                    '</span><br/>'
                                );
                            }
                            return str + s;
                        }, '');
                    }
                },
                title: {
                    text: 'Listado de Easy Work'
                },
                plotOptions: {
                    series: {
                        dataLabels: {
                            enabled: true,
                            format: '{point.name}',
                            style: {
                                fontSize: "12px",
                                fontWeight: "bold",
                                cursor: "default",
                                color: "#000000",
                                textShadow: "none"
                            }
                        },
                        allowPointSelect: true
                    }
                },
                xAxis: {
                    currentDateIndicator: true,
                    min: new Date(modificarMesFecha(data.FechaIni)).getTime() - 1 * day,
                    max: new Date(modificarMesFecha(data.FechaFin)).getTime() + 1 * day,
                },

                navigator: {
                    enabled: true,
                    liveRedraw: true,
                    series: {
                        type: 'gantt',
                        pointPlacement: 0.5,
                        pointPadding: 0.25
                    },
                    yAxis: {
                        min: 0,
                        max: 3,
                        reversed: true,
                        categories: []
                    }
                },
                scrollbar: {
                    enabled: true
                },
                rangeSelector: {
                    enabled: true,
                    selected: 0
                },
            };

            Highcharts.setOptions({
                lang: {
                    loading: 'Cargando...',
                    months: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                    weekdays: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
                    shortMonths: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                    shortWeekdays: ['D', 'L', 'M', 'M', 'J', 'V', 'S'],
                    exportButtonTitle: "Exportar",
                    printButtonTitle: "Importar",
                    rangeSelectorFrom: "Desde",
                    rangeSelectorTo: "Hasta",
                    rangeSelectorZoom: "Período",
                    downloadPNG: 'Descargar imagen PNG',
                    downloadJPEG: 'Descargar imagen JPEG',
                    downloadPDF: 'Descargar imagen PDF',
                    downloadSVG: 'Descargar imagen SVG',
                    printChart: 'Imprimir',
                    resetZoom: 'Reiniciar zoom',
                    resetZoomTitle: 'Reiniciar zoom',
                    thousandsSep: ",",
                    decimalPoint: '.'
                },
                colors: ['#0d233a', '#8bbc21', '#910000', '#1aadce', '#492970', '#f28f43', '#77a1e5', '#c42525', '#2f7ed8', '#a6c96a']
            });
            var chart = Highcharts.ganttChart('containerGantt', options);
        }

    </script>
    <style>
        .transparent {
            background-color: transparent !important;
            float: none !important;
        }

        .ui-state-hover a, .ui-state-hover a:hover,
        .ui-state-hover a:link a:link, .ui-state-hover a:link {
            color: #ffffff;
        }

        #containerGantt {
            min-width: 100vh;
            margin: 1em auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Easy Work
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu" style="float: right;">
        <li>
            <a href="../Home/Default.aspx">INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
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

    <div class="col-md-12">
        <div id="accordion">
            <div id="accordion0">
                <h3><a href="#">Agregar Easy Work</a></h3>
                <div class="block">
                    <asp:UpdatePanel ID="pnlUsuario" runat="server">
                        <ContentTemplate>
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label for="txtUsuario">Documento</label>
                                    <asp:TextBox runat="server" TextMode="Number" class="form-control" ID="txtUsuario" AutoPostBack="true"></asp:TextBox>
                                    <label style="margin: 5px 0 0 15px; width: auto;" runat="server" id="lblUsuario"></label>
                                    <asp:HiddenField runat="server" ID="hfUsuario" />
                                </div>
                                <asp:Panel runat="server" ID="formFecha" Enabled="false">
                                    <div class="form-group">
                                        <label for="txtFecha">Fecha</label>
                                        <asp:TextBox runat="server" class="form-control" ID="txtFecha" />
                                    </div>
                                </asp:Panel>
                                <div class="form-group">
                                    <label for="txtObservaciones" style="margin-right: 10px;">Observaciones</label>
                                    <asp:TextBox runat="server" class="form-control" ID="txtObservaciones" TextMode="MultiLine" Width="50%" />
                                </div>
                                <asp:Button Text="Guardar" ID="btnEnviar" runat="server" CssClass="btn btn-default" OnClientClick="activarHistoricoHWH();" />
                                <asp:Button Text="Limpiar" ID="btnLimpiar" runat="server" CssClass="btn btn-default" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="accordion1">
                <h3><a href="#">Listado Easy Work</a></h3>
                <div class="block">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="form-horizontal">
                                    <div class="form-group">
                                        <label for="txtUsuarioBuscar">Documento</label>
                                        <asp:TextBox runat="server" TextMode="Number" class="form-control" ID="txtUsuarioBuscar" AutoPostBack="true"></asp:TextBox>
                                        <label style="margin: 5px 0 0 15px; width: auto;" runat="server" id="lblUsuarioBuscar"></label>
                                    </div>
                                    <div class="form-group" runat="server" id="divFechaListado" visible="false">
                                        <label for="txtFechaIniListado">Fecha Inicio</label>
                                        <asp:TextBox runat="server" CssClass="form-control izq FechaSelect" ID="txtFechaIniListado" />
                                        <label for="txtFechaFinListado">Fecha Fin</label>
                                        <asp:TextBox runat="server" CssClass="form-control izq FechaSelect" ID="txtFechaFinListado" />
                                    </div>
                                    <asp:Button Text="Buscar" ID="btnBuscar" runat="server" CssClass="btn btn-default" />
                                    <asp:Button Text="Limpiar" ID="btnLimpiarBuscar" runat="server" CssClass="btn btn-default" />
                                </div>
                            </div>
                            <div class="row">
                                <asp:GridView ID="gvTeleTrabajo" runat="server" Width="90%" AutoGenerateColumns="False"
                                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                    DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Fecha_Programada" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                        <asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
                                        <asp:BoundField DataField="Usuario_Gestion" HeaderText="Usuario Gestión" />
                                        <asp:BoundField DataField="Fecha_Gestion" HeaderText="Fecha Gestión" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="Observaciones_Gestion" HeaderText="Observaciones de Gestión" />
                                        <asp:TemplateField HeaderText="Gestionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgGestionar" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Gestionar" ImageUrl="~/Images/Select_16.png" Text="Gestionar HWH" ToolTip="Gestionar HWH" OnClientClick="BusquedaHWH();" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerTemplate>
                                        <div class="pagingButtons">
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                            Enabled='<%# IIf(gvTeleTrabajo.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                            Enabled='<%# IIf(gvTeleTrabajo.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <span class="pagingLinks">[<%= gvTeleTrabajo.PageIndex + 1%>-<%= gvTeleTrabajo.PageCount%>]</span>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                            Enabled='<%# IIf((gvTeleTrabajo.PageIndex + 1) = gvTeleTrabajo.PageCount, "false", "true") %>'
                                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                            Enabled='<%# IIf((gvTeleTrabajo.PageIndex + 1) = gvTeleTrabajo.PageCount, "false", "true") %>'
                                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </PagerTemplate>
                                </asp:GridView>
                            </div>
                            <div class="row scrolling-container">
                                <div id="containerGantt" style="border: 1px solid #000000"></div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <div id="accordion2">
                <h3><a href="#">Editar Easy Work</a></h3>
                <div class="block">
                    <asp:UpdatePanel ID="pnlGestionar" runat="server">
                        <ContentTemplate>
                            <div class="row">
                                <div class="form-horizontal">
                                    <asp:HiddenField ID="hfId" runat="server" />
                                    <asp:HiddenField ID="hfEstado" runat="server" />
                                    <asp:Panel runat="server" ID="formDocumentoEdit" Enabled="false">
                                        <div class="form-group">
                                            <label for="txtUsuarioEdit">Documento</label>
                                            <asp:TextBox runat="server" class="form-control" ID="txtUsuarioEdit"></asp:TextBox>
                                            <label style="margin: 5px 0 0 15px; width: auto;" runat="server" id="lblUsuarioEdit"></label>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="formEstadoEdit" Enabled="false">
                                        <div class="form-group">
                                            <label for="txtEstadoEdit">Estado</label>
                                            <asp:TextBox runat="server" class="form-control" ID="txtEstadoEdit"></asp:TextBox>
                                        </div>
                                    </asp:Panel>
                                    <asp:Panel runat="server" ID="formFechaEdit" Enabled="false">
                                        <div class="form-group">
                                            <label for="txtFechaEdit">Fecha</label>
                                            <asp:TextBox runat="server" class="form-control" ID="txtFechaEdit"></asp:TextBox>
                                        </div>
                                    </asp:Panel>
                                    <div class="form-group">
                                        <label for="txtObservacionesEdit" style="margin-right: 10px;">Observaciones</label>
                                        <asp:TextBox runat="server" class="form-control" ID="txtObservacionesEdit" TextMode="MultiLine" Width="50%" />
                                    </div>
                                    <div style="float: right;">
                                        <button class="btn btn-primary" visible="false" style="margin: 0px 5px;" id="btnAnular" runat="server" onclick="activarHistoricoHWH();"><i class="fa fa-ban"></i>Anular</button>
                                        <button class="btn btn-primary" style="margin: 0px 5px;" id="btnLimpiarEdit" runat="server" onclick="activarHistoricoHWH();">Limpiar</button>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>

</asp:Content>
