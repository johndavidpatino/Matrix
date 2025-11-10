<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="SGC_Calidad_Home.aspx.vb" Inherits="WebMatrix.SGC_Calidad_Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/js/Components/Lit/index.js" type="module"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server">SGC</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Acciones mejora</li>
    <li>
        <a href="./AccionesMejora/SGC_AccionesMejora.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-calendar"></i>
            Acciones mejora
        </a>
    </li>
    <li class="app-sidebar__heading">Auditorias Internas</li>
    <li>
        <a href="./AuditoriasInternas/Auditor.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-clipboard-list"></i>
            Auditorias internas
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
    Home
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <div id="notificationsContainer" class="notifications-container"></div>
    <main class="SGC-AI-Auditor">
        <section>
            <article>
            </article>
        </section>
    </main>
</asp:Content>
