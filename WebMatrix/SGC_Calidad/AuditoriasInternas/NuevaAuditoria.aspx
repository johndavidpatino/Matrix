<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="NuevaAuditoria.aspx.vb" Inherits="WebMatrix.NuevaAuditoria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/js/Components/Lit/index.js" type="module"></script>
    <script src="../Scripts/js/Pages/SGC_Calidad/AuditoriasInternas/NuevaAuditoria/NuevaAuditoria.js"  type="module" defer></script>
    <link href="../Scripts/js/reportes/notifications/notifications.css" rel="stylesheet"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server">Reportes</asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Reportes</li>
    <li>
        <a href="IndicadoresRegistroObservaciones.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-address-card"></i>
            Registro de Observaciones
        </a>
    </li>
    <li>
        <a href="IndicadoresCumplimientoTareas.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-book-reader"></i>
            Cumplimiento de tareas
        </a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Title" runat="server">
    Nueva auditoria
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>


<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <div id="notificationsContainer" class="notifications-container"></div>
    <main class="SGC-AI-NuevaAuditoria">
        <section id="nuevaAsuditoriaForm" class="">
            <header>

            </header>
            <article>
                <mx-sgc-ai-nueva-auditoria>

                </mx-sgc-ai-nueva-auditoria>
            </article>
        </section>
    </main>
</asp:Content>