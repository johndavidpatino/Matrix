<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral2.master" CodeBehind="Calendario.aspx.vb" Inherits="WebMatrix.Calendario" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="https://code.highcharts.com/gantt/highcharts-gantt.js"></script>
    <script src="https://code.highcharts.com/stock/modules/stock.js"></script>
    <script src="https://code.highcharts.com/gantt/modules/exporting.js"></script>

    <script>        
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
                    text: 'Listado de tareas'
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

        .widthAuto {
            width: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Calendario Coordinadores
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Menu" runat="server">
    <ul class="mi-menu">
        <li>
            <a href="#">GERENCIA OP</a>
            <ul>
                <li><a title="Asignación de coordinación de estudios" href="../RE_GT/AsignacionCOE.aspx">AsignarOMP</a></li>
                <li><a title="Asignación JBI a Proyectos" href="../RE_GT/AsignacionJBI.aspx">AsignarJBI</a></li>
                <li><a title="Revisar Presupuestos" href="../CU_Cuentas/RevisionPresupuestos.aspx">RevisarPresupuestos</a></li>
                <li><a title="AjustarCostos" href="../CAP/PresupuestosAprobados.aspx?opt=2">AjustarCostos</a></li>
                <li><a title="Trabajos por gerencia" href="../RP_Reportes/TrabajosPorGerencia.aspx">Seguimiento</a></li>
                <li><a title="Cambios de JobBook Interno" href="../RE_GT/CambiosJBI.aspx">Cambios JBI</a></li>
            </ul>
        </li>
        <li>
            <a href="#">PLANEACIÓN</a>
            <ul>
                <li><a title="Planeación Operaciones General" href="../RP_Reportes/PlaneacionGeneralOperaciones.aspx">Planeación Encuestas y Encuestadores</a></li>
                <li><a title="Listado propuestas" href="../RP_Reportes/ListadoPropuestasSeguimiento.aspx" target="_blank">Seguimiento de Propuestas</a></li>
                <li><a title="Listado anuncios de aprobación" href="../RP_Reportes/ListadoEstudiosSeguimiento.aspx" target="_blank">Seguimiento de Anuncios</a></li>
                <li><a title="Trabajos atrasados" href="../RP_Reportes/TrabajosConAtraso.aspx" target="_blank">Trabajos atrasados</a></li>
                <li><a title="Listado encuestadores - Ficha Encuestador" href="../RP_Reportes/ListadoEncuestadores.aspx" target="_blank">Ficha de Encuestador</a></li>
                <li><a title="Tiempos de revisión de presupuestos" href="../RP_Reportes/InformeTiemposRevisionPresupuestos.aspx" target="_blank">Tiempos revisión</a></li>

            </ul>
        </li>
        <li>
            <a href="#">COORDINACIÓN</a>
            <ul>
                <li><a title="Asignación de coordinación de estudios" href="AsignacionCoordinador.aspx?TipoTecnicaid=1">Asignar Coordinador de Campo</a></li>
                <li><a title="Personal PST-Contratistas" href="../TH_TalentoHumano/PrestacionServicios-CT.aspx" target="_blank">Personal PST-Contratistas</a></li>
            </ul>
        </li>
        <li>
            <a href="#">CAMPO</a>
            <ul>
                <li><a title="Encuestadores y supervisores sin producción" href="../RP_Reportes/PersonalSinProduccion.aspx">Personas sin producción</a></li>
            </ul>
        </li>
        <li>
            <a href="#">OP CUANTI</a>
            <ul>
                <li>
                    <a href="#">OMP</a>
                    <ul>
                        <li><a title="Trabajos" href="../op_cuantitativo/Trabajos.aspx" runat="server" id="liTrabajosCuanti">Trabajos</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">Coordinador Campo</a>
                    <ul>
                        <li><a title="Trabajos" href="../op_cuantitativo/TrabajosCoordinador.aspx">Trabajos</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">Call Center</a>
                    <ul>
                        <li><a title="Trabajos" href="../OP_CUANTITATIVO/TrabajosCallCenter.aspx">Trabajos</a></li>
                        <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=14&RolId=49&URLRetorno=18" target="_blank">Gestionar tareas</a></li>
                        <li><a title="Cargar productividad" href="ImportarDatos.aspx">Cargar productividad</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">Scripting</a>
                    <ul>
                        <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=11&RolId=28&URLRetorno=5" target="_blank">Gestionar tareas</a></li>
                        <li><a title="Trafico tareas" href="../CORE/ListaTareas-Trafico.aspx?Permiso=112&ProcesoId=10" target="_blank">Trafico tareas</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">Mystery</a>
                    <ul>
                        <li><a title="Cargar productividad" href="../OP_CUANTITATIVO/ImportarDatos.aspx">Cargar productividad</a></li>
                    </ul>
                </li>
                <li>
                    <a href="#">RMC</a>
                    <ul>
                        <li><a title="Cargar productividad" href="../OP_Cuantitativo/ImportarDatos.aspx">Cargar productividad</a></li>
                        <li><a title="Tráfico y seguimiento de encuestas y recursos" href="../OP_Cuantitativo/TraficoEncuestas.aspx?UnidadId=38">Tráfico y Recursos</a></li>
                    </ul>
                </li>
            </ul>
        </li>
        <li>
            <a href="#">OP CUALI</a>
            <ul>
                <li><a href="../OP_Cualitativo/TrabajosCoordinador.aspx">Trabajos</a></li>
                <li><a href="../RE_GT/AsignacionCampo.aspx">Asignar Coordinador</a></li>
                <li><a href="../OP_Cualitativo/Calendario.aspx">Calendario Coordinador</a></li>
            </ul>
        </li>
        <li>
            <a href="../Home/Default.aspx">INICIO</a>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Calendario
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
    <br />
    <br />
    <div class="row">
        <div class="col-md-12">
            <asp:UpdatePanel runat="server" ID="upDatos">
                <ContentTemplate>
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="ddlCoordinador">Coordinador</label>
                            <asp:DropDownList runat="server" ID="ddlCoordinador" CssClass="form-control widthAuto" AutoPostBack="true">
                            </asp:DropDownList>
                            <asp:HiddenField ID="hfGerente" Value="0" runat="server" />
                            <asp:HiddenField ID="hfCoordinador" Value="0" runat="server" />
                            <asp:HiddenField ID="hfIdTrabajo" Value="0" runat="server" />
                        </div>
                    </div>
                    <div class="form-horizontal" id="EstudiosGerente" runat="server" visible="false">
                        <asp:GridView ID="gvTrabajos" runat="server" Width="90%" AutoGenerateColumns="False" PageSize="5"
                            CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="id" AllowPaging="true" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="id" HeaderText="Id Tarea" />
                                <asp:BoundField DataField="ProyectoId" HeaderText="Id Proyecto" />
                                <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre Trabajo" />
                                <asp:BoundField DataField="FechaTentativaInicioCampo" HeaderText="Fecha Inicio Campo" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="FechaTentativaFinalizacion" HeaderText="Fecha Fin Campo" DataFormatString="{0:dd/MM/yyyy}" />
                                <asp:BoundField DataField="NombreProyecto" HeaderText="Nombre Proyecto" />
                                <asp:TemplateField HeaderText="Cronograma" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgCronograma" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Ver" ImageUrl="~/Images/calendar_24.png" Text="Ver Cronograma" ToolTip="Ver Cronograma" />
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
                                                    Enabled='<%# IIf(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIf(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvTrabajos.PageIndex + 1%>-<%= gvTrabajos.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIf((gvTrabajos.PageIndex + 1) = gvTrabajos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIf((gvTrabajos.PageIndex + 1) = gvTrabajos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                    <asp:Panel ID="pnlCronograma" runat="server" Visible="false">
                        <p>Cronograma</p>
                        <asp:Button Text="Tabla" ID="li_Tabla_Cronograma" runat="server" CssClass="button ml-20" />
                        <asp:Button Text="Gantt" ID="li_Gantt_Cronograma" runat="server" CssClass="button ml-20" />
                        <div id="tabs" class="divButton">
                            <div id="Tabla_Cronograma" runat="server" visible="true" class="divButton">                                
                                <asp:GridView ID="gvCronograma" runat="server" DataKeyNames="Id,TareaId,UsuarioId,Retraso,RolEstima,RolEjecuta" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
                                    EmptyDataText="No se encuentran tareas para este trabajo">
                                    <SelectedRowStyle CssClass="SelectedRow" />
                                    <AlternatingRowStyle CssClass="odd" />
                                    <Columns>
                                        <asp:BoundField DataField="Tarea" HeaderText="Tarea" />
                                        <asp:BoundField DataField="FIniP" HeaderText="F Inicio Planeada" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="FFinP" HeaderText="F Fin Planeada" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="FIniR" HeaderText="F Inicio Real" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="FFinR" HeaderText="F Fin Real" DataFormatString="{0:d}" />
                                        <asp:BoundField DataField="ObservacionesPlaneacion" HeaderText="Observaciones P." />
                                        <asp:BoundField DataField="ObservacionesEjecucion" HeaderText="Observaciones E." />
                                        <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                        <asp:BoundField DataField="Responsable" HeaderText="Usuario Asignado" />
                                        <%--<asp:TemplateField HeaderText="" ShowHeader="False" Visible="false">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Info" ImageUrl="~/Images/cliente.jpg" Text="Info" OnClientClick="MostrarUsuariosAsignados()"
                                            ToolTip="Asignar Usuario" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
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
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
    </div>
</asp:Content>
