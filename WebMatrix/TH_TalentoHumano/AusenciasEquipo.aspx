<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="AusenciasEquipo.aspx.vb" Inherits="WebMatrix.AusenciasEquipo" %>


<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
	<script src="../Scripts/js/libs/Highcharts-Gantt-11.2.0/code/highcharts-gantt.js"></script>
	<script src="../Scripts/js/libs/Highcharts-Gantt-11.2.0/code/modules/accessibility.js"></script>
	<script src="../Scripts/js/libs/Highcharts-Gantt-11.2.0/code/modules/pattern-fill.js"></script>
	<script src="../Scripts/js/ausencias-equipo/gantt.js" defer  type="module"></script>
    <%--<script src="../Scripts/js/ausencias-equipo/listados-personas.js" defer  type="module"></script>--%>
	<link href="../css/ausencias-equipo/ausencias-equipo.css" rel="stylesheet"/>
    
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    |::...  Matrix  ...::|
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Title" runat="server">
    Ausencias del equipo
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Solicitudes</li>
    <li>
        <a href="SolicitudAusencia.aspx?Request=New" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-calendar-plus"></i>
            Nueva solicitud
                            </a>
    </li>
    <li>
        <a href="SolicitudAusencia.aspx?Request=Pending" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-list"></i>
            Vacaciones y beneficios pendientes
                            </a>
    </li>
    <li>
        <a href="SolicitudAusencia.aspx?Request=History" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-sun"></i>
            Historial
                            </a>
    </li>
    <li>
        <a href="SolicitudAusencia.aspx?Request=Approvals" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-bell"></i>
            Solicitudes por aprobar
                            </a>
    </li>
    <li class="app-sidebar__heading">Gestión área</li>
    <li>
        <a href="AusenciasEquipo.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-calendar"></i>
            Ausencias del equipo
                            </a>
    </li>

</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
	<main class="ausencias-equipo-container">
        <header class="ausencias-header">
            <div class="ausencias-filtros">
                <input class="ausencias-date" type="date" id="startDateInput" placeholder="Desde la fecha"/>
                <input class="ausencias-date" type="date" id="endDateInput" placeholder="Hasta la fecha"/>
                <button type="button" id="bntFilterDate" class="btn-ausencias btn-ausencias-icon">
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 -960 960 960" ><path d="M440-160q-17 0-28.5-11.5T400-200v-240L168-736q-15-20-4.5-42t36.5-22h560q26 0 36.5 22t-4.5 42L560-440v240q0 17-11.5 28.5T520-160h-80Zm40-308 198-252H282l198 252Zm0 0Z" fill="currentColor"/></svg>
                    <span>Filtrar</span></button>
            </div>
<%--            <div class="ausencias-agregar-wrapper">
                <button type="button" id="bntAgregarPersona" class="btn-ausencias btn-ausencias-variant btn-ausencias-icon">
                    
                    <span>Administrar equipo</span></button>
                
            </div>--%>
        </header>
        <section id="modalAusencias" class="ausencias-modal hidden">
            <header class="ausencias-modal-header">
                <h2 class="asencias-modal-title">Agregar personas al equipo</h2>
                <div class="ausencias-modal-header-actions">
                    <div class="ausencias-search-persona">

                        <input class="ausencias-input-text" id="inputBuscarPersonas" type="text" placeholder="Buscar por nombre..."/>

                        <button type="button" id="btnBuscarPersonas" class="btn-ausencias btn">Buscar</button>
                    </div>
                    <button type="button" id="bntOcultarPersonas" class="btn-ausencias btn-ausencias-icon btn-ausencias-variant">
                        <svg xmlns="http://www.w3.org/2000/svg"  viewBox="0 -960 960 960"><path d="m256-200-56-56 224-224-224-224 56-56 224 224 224-224 56 56-224 224 224 224-56 56-224-224-224 224Z" fill="currentColor"/></svg>
                    </button>
                </div>
            </header>
            <article  class="ausencias-modal-card hidden">
                <div class="ausencias-listas">
                    <div class="ausencias-listas-inner">
                        <div id="listaPersonasIn" class="ausencias-lista-personas">
                        </div>
                        <div id="listaPersonasOut" class="ausencias-lista-personas">
                            <label class="lista-persona">
                                <span>Carlos Yeric Fonseca Rios</span>
                                <button class="btn-ausencias btn-ausencias-icon">
                                    <span class="material-symbols-outlined">
                                        add
                                    </span>
                                </button>
                            </label>
                             <label class="lista-persona">
                                 <span>Carlos Yeric Fonseca Rios</span>
                                 <button class="btn-ausencias btn-ausencias-icon">
                                     <span class="material-symbols-outlined">
                                         add
                                     </span>
                                 </button>
                             </label>
                             <label class="lista-persona">
                                 <span>Carlos Yeric Fonseca Rios</span>
                                 <button class="btn-ausencias btn-ausencias-icon">
                                     <span class="material-symbols-outlined">
                                         add
                                     </span>
                                 </button>
                             </label>
                        </div>
                    </div>
<%--                    <button type="button" id="bntGuardarPersonas" class="btn-ausencias">
                        <span>Guardar</span>
                    </button>--%>
                </div>
            </article>
        </section>
		<div id="ganttContainer" class="gantt-container">
            <div>
                <span class="beneficio-vacaciones beneficio-label">
                    Vacaciones
                </span>
                <span class="beneficio-plus beneficio-label">
                    Dias Plus (V+)
                </span>
                <span class="beneficio-balance beneficio-label">
                    Dias de balance
                </span>
            </div>

            <div id="ausenciasEquipoCards" class="asuencias-equipo-cards">
                
            </div>
			<div id="ganttChart"></div>
<%--			<button type="button" class="btn-ausencias btn-aprobar" disabled id="btnAprobar">Aprobar</button>
			<button type="button" class="btn-ausencias btn-anular" disabled id="btnRechazar">Rechazar</button>
            <button type="button" class="btn-ausencias btn-anular" disabled id="btnAnular">Anular</button>--%>
		</div>
        
	</main>
</asp:Content>