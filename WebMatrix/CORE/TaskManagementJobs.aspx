<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="TaskManagementJobs.aspx.vb" Inherits="WebMatrix.TaskManagementJobs" %>

<%@ Register Assembly="DevExpress.Web.v22.2, Version=22.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/AppUsersControls/UC_LoadFiles.ascx" TagName="LoadFiles" TagPrefix="uclf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.tools.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/css/theme.light.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui.min.css" rel="stylesheet">
        <script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <script src="../Scripts/blockUIOnAllAjaxRequests.js" type="text/javascript"></script>
    <script src="../Scripts/jquery.blockUI.js" type="text/javascript"></script>
    <link href="../css/jquery-ui.css" rel="stylesheet">

    <script src="https://code.highcharts.com/gantt/highcharts-gantt.js"></script>
    <script src="https://code.highcharts.com/stock/modules/stock.js"></script>
    <script src="https://code.highcharts.com/gantt/modules/exporting.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerIni);
            bindPickerIni();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerFin);
            bindPickerFin();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest($.unblockUI);
            $('#DevolucionTarea').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Devolucion Tarea",
                    width: "600px"
                });


            $('#DevolucionTarea').parent().appendTo("form");

            $('#CargaArchivos').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Carga archivos",
                    width: "600px"
                });


            $('#CargaArchivos').parent().appendTo("form");

            $('#UsuariosAsignados').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Usuarios Asignados",
                    width: "600px",
                    closeOnEscape: true,
                    open: function (type, data) {
                        $(this).parent().appendTo("form");

                    }
                });

            $('#EstimacionFechas').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Estimar Fechas",
                    width: "800px",
                    minHeight: "600px",
                    closeOnEscape: true,
                    open: function (type, data) {
                        $(this).parent().appendTo("form");

                    }
                });

            $('#mensaje').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    buttons: {
                        Cerrar: function () {
                            $(this).addClass("button");
                            $(this).dialog("close");
                        }
                    }
                });


        });

        function UpdateInfo() {
            var daysTotal = deEnd.GetRangeDayCount();
            tbInfo.SetText(daysTotal !== -1 ? daysTotal + ' days' : '');
        }

        function abrirMensaje(mensaje) {
            $('#lblMensaje').text(mensaje);
            $('#mensaje').dialog("open");
        }

        function AbrirDevolucion() {
            $('#DevolucionTarea').dialog("open");
        }

        function AbrirCargaArchivos() {
            $('#CargaArchivos').dialog("open");
        }

        function MostrarUsuariosAsignados() {
            $('#UsuariosAsignados').dialog("open");
        }

        function MostrarEstimacionFechas() {
            $('#EstimacionFechas').dialog("open");
        }

        function OcultarUsuariosAsignados() {
            $('#UsuariosAsignados').dialog("close");
        }

        function OcultarEstimacionFechas() {
            $('#EstimacionFechas').dialog("close");
        }
        function bindPickerIni() {
            $("input[type=text][id*=txtFInicio]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function bindPickerFin() {
            $("input[type=text][id*=txtFFin]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }
        function bloquearUI() {
            $.blockUI({ message: "Procesando ...." });
        }
        function mostrarMensaje(mensaje) {
            $('#mensaje').dialog("open");
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
                name: 'Cronograma de Tareas',
                id: 'cronograma_tareas'
            }];
            data.series.forEach(function (item) {
                var s = {
                    name: item.name,
                    id: item.id,
                    parent: item.parent,
                    dependency: item.dependency,
                    start: new Date(modificarMesFecha(item.fstart + " 00:00:00")).getTime(),
                    end: new Date(modificarMesFecha(item.fend + " 23:59:59")).getTime(),
                    //completed: {
                    //    amount: (item.porcentaje * 100) + '%' || null
                    //},
                    owner: item.owner || 'Sin Asignar'
                };
                datos.push(s);
            });
            var ListaSerie = ([{
                name: 'Cronograma',
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
                            value: point.name,
                            style: 'font-weight: bold;'
                        }, {
                            title: 'Inicio',
                            value: dateFormat(format, point.start)
                        }, {
                            visible: !options.milestone,
                            title: 'Final',
                            value: dateFormat(format, point.end)
                            //}, {
                            //    title: 'Completada',
                            //    value: status
                        }, {
                            title: 'Asignada',
                            value: options.owner || 'Sin Asignar'
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
                    text: 'Cronograma de Tareas'
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
                }
            });
            var chart = Highcharts.ganttChart('containerGantt', options);
        }
        $(function () {
            $("#tabs").tabs();
        });
    </script>
    <style>
        .button {
            float: left !important;
            padding: 0 20px !important;
            height: 30px !important;
            font-size: 12px !important;
            color: #fff !important;
            outline: none !important;
            background-color: #00ada8 !important;
            border: none !important;
            transition: all 0.2s ease !important;
            border-radius: 3px !important;
        }

        .ml-20 {
            margin: auto 10px;
        }

        .divButton {
            margin-top: 15px;
        }

        .divButton2 {
            margin-top: 3px;
        }

        #containerGantt {
            min-width: 100vh;
            margin: 1em auto;
        }

        .scrolling-container {
            overflow-x: auto;
            -webkit-overflow-scrolling: touch;
        }

        .lblTextInfo {
            display: block;
            font-weight: lighter;
            text-align: right;
            padding-top: 5px;
            width: 150px;
            float: left;
            font-family: 'Roboto', sans-serif;
            font-size: 13px;
        }

        .lblTitleInfo {
            font-family: 'Roboto', sans-serif;
            font-size: 13px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Opciones</li>
    <%--    <li>
        <a href="../RE_GT/HomeRecoleccion.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-arrow-circle-left"></i>
            Regresar a
            <br />
            Recolección de Datos
        </a>
    </li>--%>
    <li>
        <a class="nav-link nav-link-white" runat="server" id="MenuOption1" onserverclick="MenuCall"><i class="metismenu-icon fa fa-tasks"></i>
            Todas las tareas</a>
    </li>
    <li>
        <a class="nav-link nav-link-white" runat="server" id="MenuOption2" onserverclick="MenuCall"><i class="metismenu-icon fa fa-star"></i>
            Mis tareas</a>
    </li>
    <li>
        <a class="nav-link nav-link-white" runat="server" id="MenuOption3" onserverclick="MenuCall"><i class="metismenu-icon fa fa-map"></i>
            Realizar estimación</a>
    </li>
    <li>
        <a class="nav-link nav-link-white" runat="server" id="MenuOption4" onserverclick="MenuCall"><i class="metismenu-icon fa fa-tags"></i>
            Asignar tareas</a>
    </li>
    <li>
        <a class="nav-link nav-link-white" runat="server" id="MenuOption5" onserverclick="MenuCall"><i class="metismenu-icon fa fa-calendar"></i>
            Ver cronograma</a>
    </li>
    <li>
        <a class="nav-link nav-link-white" runat="server" id="MenuOption6" onserverclick="MenuCall"><i class="metismenu-icon fa fa-folder-open"></i>
            Documentos</a>
    </li>
    <li>
        <a class="nav-link nav-link-white" runat="server" id="MenuOption7" onserverclick="MenuCall"><i class="metismenu-icon fa fa-arrow-circle-left"></i>
            Regresar</a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Title" runat="server">
    Gestión de Tareas
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
    <asp:Label ID="txtNombreTrabajo" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
<asp:UpdatePanel ID="UPanelGeneral" runat="server">
                    <ContentTemplate>
    <asp:HiddenField ID="hfIdTrabajo" runat="server" Value="0" />
    <asp:HiddenField ID="hfInternacional" runat="server" Value="0" />
    <asp:HiddenField ID="hfRolEstima" runat="server" />
    <asp:HiddenField ID="hfRolEjecuta" runat="server" Value="0" />
    <asp:HiddenField ID="hfUnidadEjecuta" runat="server" Value="0" />
    <asp:HiddenField ID="hfUsuarioAsignado" runat="server" Value="0" />
    <asp:HiddenField ID="hfUrlRetorno" runat="server" Value="0" />
    <asp:HiddenField ID="hfEstadoId" runat="server" />

        <asp:Panel ID="pnlTareasXTrabajo" runat="server" Visible="false"  CssClass="row" >
                <div class="col-lg-4">
                    <div class="main-card mb-3 card">
                        <div class="card-body">
                            <h5 class="card-title">Tareas</h5>
            <p class="card-subtitle"></p>
                        <asp:GridView ID="gvTareasList" runat="server" DataKeyNames="Id,UsuarioId,Retraso,RolEstima,RolEjecuta" CssClass="mb-0 table table-hover table-bordered" AutoGenerateColumns="false"
                                EmptyDataText="No se encuentran tareas para este trabajo">
                                <Columns>
                                    <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                                    <asp:TemplateField HeaderText="" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Info" ImageUrl="~/Images/info_16.png" Text="Info"
                                                ToolTip="Ir al detalle de la tarea" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            </div>
                    </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="main-card mb-3 card">
                        <div class="card-body">
                        <asp:Panel ID="pnlDetalleTarea" runat="server" Visible="false">
                            <asp:HiddenField ID="hfIdWFid" runat="server" Value="0" />
                            <asp:HiddenField ID="hfIdTareaRechazo" runat="server" Value="0" />
                                <asp:Panel ID="pnlDetalle" runat="server" Visible="false">
                                    <p>
                                        Tarea: <a style="font-weight: bold">
                                            <asp:Label ID="lblTarea" runat="server"></asp:Label></a>
                                    </p>
                                    <p>
                                        Estado: <a style="font-weight: bold">
                                            <asp:Label ID="lblEstado" runat="server"></asp:Label></a>
                                    </p>
                                    <p>
                                        Responsable: <a style="font-weight: bold">
                                            <asp:Label ID="lblResponsable" runat="server"></asp:Label></a>
                                    </p>
                                    <p>Fechas:</p>
                                    <table style="width: 100%" class="mb-0 table table-dark">
                                        <tr>
                                            <td colspan="2">Planeación</td>
                                            <td colspan="2">Ejecución</td>
                                        </tr>
                                        <tr>
                                            <td>Inicio</td>
                                            <td>Fin</td>
                                            <td>Inicio</td>
                                            <td>Fin</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblInicioP" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblFinP" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblInicioR" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblFinR" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                    <br />
                                    <a>Tareas anteriores</a>
                                    <br />
                                    <asp:GridView ID="gvTareasAnteriores" runat="server" DataKeyNames="Id,UsuarioId,Retraso,RolEstima,RolEjecuta" CssClass="mb-0 table table-hover table-bordered" AutoGenerateColumns="false"
                                        EmptyDataText="No se encuentran tareas dependientes anteriores">
                                        <Columns>
                                            <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                                            <asp:TemplateField HeaderText="" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="Devolver" ImageUrl="~/Images/delete_16.png" Text="Actualizar"
                                                        OnClientClick=<%# IIF(Eval("ESTADOID") = 5, "AbrirDevolucion()", "abrirMensaje('Esta tarea no puede ser devuelta porque aún no se ha finalizado!');return false") %>
                                                        ToolTip="Rechazar o devolver la tarea" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <p>Observaciones</p>
                                    <div style="overflow: scroll">
                                        <asp:GridView ID="gvObservaciones" runat="server" CssClass="table table-bordered table-hover" AutoGenerateColumns="true"
                                            EmptyDataText="No hay observaciones registradas" AllowPaging="true" PageSize="2">
                                        </asp:GridView>
                                    </div>
                                    <br />
                                    <asp:Label runat="server" Visible="false" ID="lblObservacion">¿Tienes alguna observación? Registrala aquí</asp:Label>
                                    <asp:TextBox ID="txtObservacionesEjecucion" runat="server" Visible="false" placeholder="Agregue aquí las observaciones" Width="100%"></asp:TextBox>
                                    <br />
                                    <br />
                                    <asp:Button ID="btnGestionTarea" runat="server" CssClass="btn btn-secondary" Visible="false" Text="Iniciar/Finalizar tarea" />
                                </asp:Panel>
                                <asp:Panel ID="PnlDetalleNoData" runat="server" Visible="true">
                                    <p>Seleccione una tarea en el listado de la izquierda para conocer el detalle</p>
                                </asp:Panel>
                                <br />
                                <asp:Panel ID="PnlIncosistencias" runat="server" Visible="false">

                                    <asp:Button ID="btnInconsistencias" CssClass="btn btn-secondary" Text="Registrar Observaciones" runat="server" Width="214px" />
                                    <br />
                                    <br />
                                    <asp:Button ID="btnRevInconsistencias" CssClass="btn btn-secondary" Text="Revisar Observaciones" runat="server" Width="214px" />

                                </asp:Panel>
                                <asp:Panel ID="PnlIncosistenciasCuali" runat="server" Visible="false">

                                    <asp:Button ID="btnInconsistenciasCuali" CssClass="btn btn-secondary" Text="Registrar Observaciones" runat="server" Width="214px" />
                                    <br />
                                    <br />
                                    <asp:Button ID="btnRevInconsistenciasCuali" CssClass="btn btn-secondary" Text="Revisar Observaciones" runat="server" Width="214px" />

                                </asp:Panel>
                        </asp:Panel>
                            </div>
                            </div>
                    </div>
                    <div class="col-lg-4">
                        <div class="main-card mb-3 card">
                        <div class="card-body">
<asp:Panel ID="pnlDocumentos" runat="server" Visible="true">
                                <asp:Panel ID="pnlDetalleDocumentos" runat="server" Visible="false">
                                    <asp:HiddenField ID="hfTareaId" runat="server" />
                                    <asp:HiddenField ID="hfDocumentoId" runat="server" />
                                    <a>Documentos requeridos</a>
                                    <asp:GridView ID="gvArchivosEntregables" runat="server" Width="100%" CssClass="mb-0 table table-striped" AutoGenerateColumns="False"
                                        DataKeyNames="TareaId, IdDocumento" AllowPaging="False" EmptyDataText="No existen documentos para esta tarea">
                                        <Columns>
                                            <asp:BoundField DataField="Documento" HeaderText="Documento" />
                                            <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                            <asp:TemplateField HeaderText="Subir" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                        CommandName="Archivos" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="AbrirCargaArchivos()" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                                <asp:Panel ID="pnlNoDataDocs" runat="server" Visible="true">
                                    <p>Seleccione una tarea en el listado de la izquierda para conocer el detalle</p>
                                </asp:Panel>
                        </asp:Panel>
                            </div>
                        </div>
                    </div>
            </asp:Panel>
        <asp:Panel ID="pnlEstimacionXTrabajo" runat="server" Visible="false" CssClass="main-card mb-3 card">
            <div class="card-body">
            <h5 class="card-title">Estimación de tareas</h5>
            <p class="card-subtitle">Espacio para definir las fechas para cada tarea</p>
                                            <asp:HiddenField ID="hfWfiDEstimacion" runat="server" />
                <asp:HiddenField ID="hfRowIdEstimacion" runat="server" />
                        <asp:HiddenField ID="hfTareaIdEstimacion" runat="server" />
                        <asp:GridView ID="gvEstimacion" runat="server" DataKeyNames="Id,TareaId,Tarea,UsuarioId,Retraso,RolEstima,RolEjecuta" CssClass="mb-0 table table-hover table-bordered" AutoGenerateColumns="false"
                            EmptyDataText="No se encuentran tareas para este trabajo">
                            <Columns>
                                <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                                <asp:TemplateField HeaderText="Inicio" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFInicio" runat="server" Text='<%#  Eval("FIniP", "{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Fin" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtFFin" runat="server" Text='<%#  Eval("FFinP", "{0:dd/MM/yyyy}") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Observaciones" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtObservacionesPlaneacion" runat="server" Text='<%#  Eval("ObservacionesPlaneacion") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="No Aplica" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chbAplica" runat="server" Checked='<%# IIF(Eval("ESTADOID")=6,True,False) %>'
                                            AutoPostBack="true" OnCheckedChanged="chkVerificar_CheckedChanged" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Dates" ImageUrl="~/Images/cliente.jpg" Text="Info" OnClientClick="MostrarEstimacionFechas()"
                                            ToolTip="Estimación de fechas" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:Button ID="btnGuardarEstimacion" runat="server" Text="Guardar Cambios" CssClass="btn btn-primary"  OnClientClick="bloquearUI()" />
            </div>
            </asp:Panel>
        <asp:Panel ID="pnlAsignacionXTrabajo" runat="server" Visible="false" CssClass="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Asignación de tareas</h5>
            <p class="card-subtitle">Espacio para asignar los responsables de cada tarea</p>
                    <asp:HiddenField ID="hfWfIdAsignacion" runat="server" />
                        <asp:HiddenField ID="hfWfTareaAsignacion" runat="server" />
                        <asp:GridView ID="gvAsignacion" runat="server" DataKeyNames="Id,TareaId,UsuarioId,Retraso,RolEstima,RolEjecuta,FIniP,FFinP" CssClass="mb-0 table table-hover table-bordered"  AutoGenerateColumns="false"
                            EmptyDataText="No se encuentran tareas para este trabajo">
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                                <asp:BoundField DataField="FIniP" HeaderText="F Inicio Planeada" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="FFinP" HeaderText="F Fin Planeada" DataFormatString="{0:d}" />
                                <asp:BoundField DataField="Responsable" HeaderText="Usuario Asignado" />
                                <asp:TemplateField HeaderText="" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Info" ImageUrl="~/Images/cliente.jpg" Text="Info" OnClientClick="MostrarUsuariosAsignados()"
                                            ToolTip="Asignar Usuario" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
        </div>
            </asp:Panel>
        <asp:Panel ID="pnlCronograma" runat="server" Visible="false" CssClass="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Cronograma</h5>
            <p class="card-subtitle">Cronograma de todas las tareas del trabajo junto con las fechas establecidas</p>

                    <asp:Button Text="Tabla" ID="li_Tabla_Cronograma" runat="server" CssClass="btn btn-secondary" />
                        <asp:Button Text="Gantt" ID="li_Gantt_Cronograma" runat="server" CssClass="btn btn-secondary" />
                        <br />
                        <div id="tabs" class="divButton">
                            <div id="Tabla_Cronograma" runat="server" visible="true" class="divButton">
                                <asp:GridView ID="gvCronograma" runat="server" DataKeyNames="Id,TareaId,UsuarioId,Retraso,RolEstima,RolEjecuta" CssClass="mb-0 table table-hover table-bordered" AutoGenerateColumns="false"
                                    EmptyDataText="No se encuentran tareas para este trabajo">
                                    <Columns>
                                        <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                                        <asp:BoundField DataField="FIniP" HeaderText="F Inicio Planeada" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="FFinP" HeaderText="F Fin Planeada" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="FIniR" HeaderText="F Inicio Real" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="FFinR" HeaderText="F Fin Real" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="ObservacionesPlaneacion" HeaderText="Observaciones Planeación" />
                                        <asp:BoundField DataField="ObservacionesEjecucion" HeaderText="Observaciones Ejecución" />
                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                        <asp:BoundField DataField="Responsable" HeaderText="Usuario Asignado" />
                                        <asp:TemplateField HeaderText="" ShowHeader="False" Visible="false">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                    CommandName="Info" ImageUrl="~/Images/cliente.jpg" Text="Info" OnClientClick="MostrarUsuariosAsignados()"
                                                    ToolTip="Asignar Usuario" />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <div class="divButton" style="display: block;">
                                    <asp:Button ID="btnDescarga" Text="Descargar" CssClass="button" runat="server" />
                                </div>
                            </div>
                            <div id="Gantt_Cronograma" runat="server" visible="false" class="divButton">
                                <div class="divButton">
                                    <asp:Table runat="server" ID="GanttCronograma"></asp:Table>
                                    <div class="scrolling-container">
                                        <div id="containerGantt" style="border: 1px solid #000000"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
        </div>
            </asp:Panel>
        <asp:Panel ID="pnlListadoDocumentos" runat="server" Visible="false" CssClass="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Listado de documentos</h5>
            <p class="card-subtitle">Espacio para ver y descargar todos los documentos relacionados del trabajo</p>
                    <asp:HiddenField ID="hfIdDocDescarga" runat="server" Value="0" />
                        <asp:Panel ID="pnlListadoDocsTotal" runat="server" Visible="false">
                            <asp:GridView ID="gvTareasXDocumentos" runat="server" CssClass="mb-0 table table-hover table-bordered"  AutoGenerateColumns="False"
                                DataKeyNames="IdDocumento" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <Columns>
                                    <asp:BoundField DataField="Documento" HeaderText="Documento" />
                                    <asp:BoundField DataField="Codigo" HeaderText="Codigo" />
                                    <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                    <asp:TemplateField HeaderText="Archivos" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Archivos" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Panel ID="PnlListadoDocsDescargar" runat="server" Visible="false">
                            <asp:GridView ID="gvDocumentosCargados" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                                AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="IdDocumentoRepositorio" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="Version" HeaderText="Version" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha cargue" />
                                    <asp:BoundField DataField="Comentarios" HeaderText="Comentarios" />
                                    <asp:TemplateField HeaderText="Descargar" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Descargar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar"
                                                ToolTip="Tareas" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Button ID="btnVolverDescarga" runat="server" Text="Volver" />
                        </asp:Panel>
        </div>
            </asp:Panel>

                        </ContentTemplate>
    </asp:UpdatePanel>
        <asp:UpdatePanel ID="UPanelButtons" runat="server">
            <ContentTemplate>
                <asp:LinkButton ID="lkbModalWarning" runat="server"></asp:LinkButton>
                <asp:LinkButton ID="lbkLoadFiles" runat="server"></asp:LinkButton>
                <asp:ModalPopupExtender ID="ModalPopupExtenderWarning" CancelControlID="btnCloseAlert" PopupControlID="pnlMessageInfo" TargetControlID="lkbModalWarning" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
                </asp:ModalPopupExtender>
                <asp:Button runat="server" ID="btnNew" class="btn btn-primary" Text="Crear Nuevo" Visible="false"></asp:Button>
                <asp:Button runat="server" ID="btnSave" class="btn btn-primary" Text="Guardar Cambios" Visible="false"></asp:Button>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnNew" />
                <asp:PostBackTrigger ControlID="btnSave" />
            </Triggers>
        </asp:UpdatePanel>

    <asp:Panel ID="pnlMessageInfo" runat="server">
        <asp:UpdatePanel ID="UPanelMessage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-sm show">
                        <div class="modal-content">
                            <div class="modal-header">
                                <p class="modal-title" id="exampleModalLabel">
                                    <asp:Label ID="lblTitleWarning" runat="server"></asp:Label>
                                </p>
                                <asp:Button ID="btnCloseAlert" runat="server" class="icon" data-dismiss="modal" aria-label="Close" Text="x"></asp:Button>
                            </div>
                            <div class="modal-body">
                                <div class="main-card mb-3 card">
                                    <div class="card-body">
                                        <asp:Panel ID="pnlMsgTextWarning" runat="server" Visible="false" class="alert alert-warning fade show" role="alert">
                                            <h6>
                                                <asp:Label ID="lblMsgTextWarning" runat="server"></asp:Label></h6>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlMsgTextError" runat="server" Visible="false" class="alert alert-danger fade show" role="alert">
                                            <h6>
                                                <asp:Label ID="lblMsgTextError" runat="server"></asp:Label></h6>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlMsgTextInfo" runat="server" Visible="false" class="alert alert-info fade show" role="alert">
                                            <h6>
                                                <asp:Label ID="lblMsgTextInfo" runat="server"></asp:Label></h6>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <div id="mensaje">
        <label id="lblMensaje"></label>
    </div>
    <div id="DevolucionTarea">
        <div>
            <fieldset>
                <label>
                    Observaciones
                </label>
                <asp:TextBox ID="txtObservacionDevolucion" runat="server" TextMode="MultiLine"></asp:TextBox>
            </fieldset>
        </div>
        <div class="spacer">
            <div class="form_right">
                <fieldset>
                    <asp:Button ID="btnGrabar" runat="server" Text="Grabar" />
                </fieldset>
            </div>
        </div>
    </div>
    <div id="CargaArchivos" style="min-height: 350px;">
        <fieldset class="validationGroup" style="min-height: 350px;">
            <div>
                <div class="form_left">
                    <fieldset>
                        <label>
                            Nombre:
                        </label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="required text textEntry"></asp:TextBox>
                    </fieldset>
                </div>
                <div class="form_left">
                    <fieldset>
                        <label>
                            Comentarios:
                        </label>
                        <asp:TextBox ID="txtComentarios" runat="server" CssClass="required text textEntry"></asp:TextBox>
                    </fieldset>
                </div>
                <div class="form_left">
                    <fieldset>
                        <label>
                            Ubicación Archivo:
                        </label>
                        <asp:FileUpload ID="ufArchivo" runat="server" Text="CargarArchivo" CssClass="required text textEntry" />
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="ufArchivo"
                            ErrorMessage="*" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                    </fieldset>
                </div>
                <div class="spacer">
                    <div class="form_right">
                        <fieldset>
                            <asp:Button ID="btnCargarArchivos" runat="server" Text="Subir Archivo" CssClass="button" />
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="button" />
                        </fieldset>
                    </div>
                </div>
            </div>
        </fieldset>
    </div>
    <div id="UsuariosAsignados">
        <asp:UpdatePanel ID="upUsuariosAsignados" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <fieldset class="validationGroup">
                    <div class="form_left" style="display: block;">
                        <label style="display: block;">
                            Usuarios disponibles para asignar</label>
                        <asp:DropDownList ID="ddlUsuariosDisponibles" runat="server" CssClass="mySpecificClass dropdowntext form-control" Width="400px">
                        </asp:DropDownList>
                        <br />
                        <br />
                        <div class="divButton2">
                            <asp:Button ID="btnAdicionarUsuario" runat="server" Text="Asignar" CssClass="button m-10" OnClientClick="$('#UsuariosAsignados').dialog('close');" />
                        </div>
                    </div>
                </fieldset>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="EstimacionFechas">

        <asp:UpdatePanel ID="upEstimacionFechas" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <fieldset class="validationGroup">
                    <div class="form_left" style="display: block; all: initial;">
                        <dx:ASPxFormLayout ID="flDateRangePicker" runat="server" RequiredMarkDisplayMode="None" CssClass="indent" UseDefaultPaddings="false" Theme="MaterialCompact">
                            <SettingsItemCaptions Location="Top"></SettingsItemCaptions>
                            <SettingsAdaptivity AdaptivityMode="SingleColumnWindowLimit" SwitchToSingleColumnAtWindowInnerWidth="600" />
                            <Items>
                                <dx:LayoutGroup ColCount="2" GroupBoxDecoration="none" UseDefaultPaddings="false">
                                    <Items>
                                        <dx:LayoutItem Caption="Start Date">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxDateEdit ID="deStart" ClientInstanceName="deStart" runat="server" Theme="Aqua">
                                                        <ClientSideEvents DateChanged="UpdateInfo"></ClientSideEvents>
                                                        <CalendarProperties>
                                                            <FastNavProperties DisplayMode="Inline" />
                                                        </CalendarProperties>
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField IsRequired="True" ErrorText="Start date is required"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="End Date">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxDateEdit ID="deEnd" ClientInstanceName="deEnd" runat="server" Theme="Aqua">
                                                        <CalendarProperties>
                                                            <FastNavProperties DisplayMode="Inline" />
                                                        </CalendarProperties>
                                                        <DateRangeSettings StartDateEditID="deStart"></DateRangeSettings>
                                                        <ClientSideEvents DateChanged="UpdateInfo"></ClientSideEvents>
                                                        <ValidationSettings Display="Dynamic" SetFocusOnError="True" CausesValidation="True" ErrorDisplayMode="ImageWithTooltip">
                                                            <RequiredField IsRequired="True" ErrorText="End date is required"></RequiredField>
                                                        </ValidationSettings>
                                                    </dx:ASPxDateEdit>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                        <dx:LayoutItem Caption="Duration">
                                            <LayoutItemNestedControlCollection>
                                                <dx:LayoutItemNestedControlContainer runat="server">
                                                    <dx:ASPxTextBox ID="tbInfo" ClientInstanceName="tbInfo" runat="server" ReadOnly="True">
                                                    </dx:ASPxTextBox>
                                                </dx:LayoutItemNestedControlContainer>
                                            </LayoutItemNestedControlCollection>
                                        </dx:LayoutItem>
                                    </Items>
                                </dx:LayoutGroup>
                            </Items>
                        </dx:ASPxFormLayout>
                        <div class="divButton2">
                            <asp:Button ID="btnEstimarOk" runat="server" Text="Listo!" CssClass="button m-10" OnClick="btnEstimarOk_Click" OnClientClick="$('#EstimacionFechas').dialog('close');" />
                        </div>
                    </div>
                </fieldset>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
