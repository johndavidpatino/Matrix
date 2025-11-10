<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="IndicadoresCumplimientoTareas.aspx.vb" Inherits="WebMatrix._IndicadoresCumplimientoTareas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/js/libs/Highcharts-Gantt-11.2.0/code/highcharts.js"></script>
    <script src="../Scripts/js/libs/Highcharts-Gantt-11.2.0/code/modules/accessibility.js"></script>
    <script src="../Scripts/js/Components/Lit/index.js" type="module"></script>
    <script src="../Scripts/js/reportes/reporte-cumplimiento-tareas-filters.js" type="module" defer></script>
    <script src="../Scripts/js/reportes/analisis-indicador-tareas.js?v1" type="module" defer></script>
    <link href="../css/reportes-calidad/styles.css" rel="stylesheet" />
    <link href="../Scripts/js/reportes/notifications/notifications.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server">Reportes</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Acciones mejora</li>
    <li>
        <a href="/SGC_Calidad/AccionesMejora/SGC_AccionesMejora.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-calendar"></i>
            Acciones de Mejora
        </a>
    </li>
    <li class="app-sidebar__heading">Auditorias Internas</li>
    <li>
        <a href="/SGC_Calidad/AuditoriasInternas/Auditor.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-clipboard-list"></i>
            Auditorias Internas
        </a>
    </li>
    <li class="app-sidebar__heading">Reportes</li>
    <li>
        <a href="IndicadoresRegistroObservaciones.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-address-card"></i>
            Registro de Observaciones
        </a>
    </li>
    <li>
        <a href="IndicadoresCumplimientoTareas.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-clipboard-list"></i>
            Cumplimiento de tareas
        </a>
    </li>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Title" runat="server">
    Cumplimiento de Tareas
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <div id="notificationsContainer" class="notifications-container"></div>
    <main class="report-container">
        <section id="filtersForm" class="filters-section">
            <h3 class="filters-title">Filtros</h3>
            <div class="filters-container">
                <mx-select-number id="yearFilter">
                    <span>Año</span>
                </mx-select-number>
                <mx-select id="unidadSelector" emptyoptionlabel="---Seleccione---" emptyoptionvalue="">
                    <span>Unidad / Area</span>
                </mx-select>
                <mx-select id="processFilter" hasemptyoption emptyoptionlabel="---Seleccione---" emptyoptionvalue="">
                    <span>Proceso</span>
                </mx-select>
                <mx-select id="taskFilter" hasemptyoption emptyoptionlabel="---Seleccione---" emptyoptionvalue="">
                    <span>Tarea</span>
                </mx-select>
                <mx-select id="groupFilter">
                    <span>Ver por</span>
                </mx-select>
            </div>
            <div class="filters-buttons">
                <mx-button id="filterSubmitBtn">Consultar</mx-button>
            </div>
        </section>
        <article class="chart-card mx-hidden" id="chartContainer">
            <div id="mainChart"></div>
            <div class="mx-buttons-group">
                <mx-button id="btnExportAgrupadosExcel" variant="outline">Datos Agrupados a Excel</mx-button>
                <mx-button id="btnExportDetallesExcel" variant="outline">Detalles a Excel</mx-button>
                <mx-button id="btnExportAgrupadosCOEExcel" variant="outline">Datos Agrupados COE a Excel</mx-button>
                <mx-button id="btnExportDetallesCOEExcel" variant="outline">Detalles COE a Excel</mx-button>
            </div>
        </article>
        <section id="analisisSection" class="analisis-section">
            <h3 class="filters-title">Registro de Análisis</h3>
            <p>Seleccione el trimestre y el área para registrar un análisis del indicador</p>
            <div class="analisis-controls">
                <div class="controls-group">
                    <mx-select id="quarterSelector" hasemptyoption emptyoptionlabel="---Seleccione---" emptyoptionvalue="">Trimestre</mx-select>
                </div>
                <mx-text-area id="analisisText"><span>Analisis del indicador</span></mx-text-area>
            </div>
            <div class="analisis-buttons">
                <mx-button id="analisisSubmitBtn">Guardar</mx-button>
            </div>
        </section>
        <section class="analisis-section">
            <h3 class="filters-title">Análisis registrados</h3>
            <div class="analisis-filters">
                <mx-select id="trimesterFilter" hasemptyoption emptyoptionlabel="Todos los trimestres" emptyoptionvalue="">
                    <span>Todos los trimestres</span>
                </mx-select>
            </div>
            <div id="analisisContainer" class="analisis-container">
                <mx-table id="analisisTableLit"></mx-table>
            </div>
        </section>
        <section class="links-section">
            <a class="mx-link" href="ReportesCumplimientoTareas.aspx">Ver version anterior</a>
        </section>
    </main>
</asp:Content>
