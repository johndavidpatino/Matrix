<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="SGC_AccionesMejora.aspx.vb" Inherits="WebMatrix.SGC_AccionesMejora" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/js/libs/Highcharts-Gantt-11.2.0/code/highcharts.js"></script>
    <script src="../Scripts/js/libs/Highcharts-Gantt-11.2.0/code/modules/accessibility.js"></script>
    <script src="../Scripts/js/Components/Lit/index.js?v2" type="module" defer></script>
    <script src="../Scripts/js/acciones-mejora/acciones-mejora.js" type="module" defer></script>
    <link href="../css/reportes-calidad/styles.css" rel="stylesheet" />
    <link href="../Scripts/js/reportes/notifications/notifications.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server">SGC Calidad</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Acciones mejora</li>
    <li>
        <a href="SGC_AccionesMejora.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-calendar"></i>
            Acciones de Mejora
        </a>
    </li>
    <li class="app-sidebar__heading">Auditorias Internas</li>
    <li>
        <a href="../AuditoriasInternas/Auditor.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-clipboard-list"></i>
            Auditorias Internas
        </a>
    </li>
    <li class="app-sidebar__heading">Reportes
    </li>
    <li>
        <a href="/RP_Reportes/Calidad/IndicadoresRegistroObservaciones.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-address-card"></i>
            Registro de Observaciones
        </a>
    </li>
    <li>
        <a href="/RP_Reportes/Calidad/IndicadoresCumplimientoTareas.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-clipboard-list"></i>
            Cumplimiento de tareas
        </a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Title" runat="server">
    <mx-typo tag="h2" type="title">Acciones de Mejora</mx-typo>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <div id="notificationsContainer" class="notifications-container"></div>
    <main class="report-container">
        <section class="mx-section">
            <header class="mx-modal-form-header">
                <mx-typo tag="h3" type="subtitle">Registro de Acciones de Mejora</mx-typo>
                <mx-button id="btnNuevaAccion">Nueva</mx-button>
            </header>
            <mx-table id="accionesMejoraTable"></mx-table>
        </section>
    </main>
    <div id="notification-container"></div>
</asp:Content>
