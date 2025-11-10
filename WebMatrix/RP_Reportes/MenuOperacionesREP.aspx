<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP.master" CodeBehind="MenuOperacionesREP.aspx.vb" Inherits="WebMatrix.MenuOperacionesREP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/slider.css" media="screen" />
    <script type="text/javascript" src="../Scripts/slider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Section" runat="server">
    <div class="prev"><a href="#anterior" title="Anterior"></a></div>
    <div id="slider">
        <div class="slidesContainer">
            <nav class="slide">

<div class="menu-element4"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">General</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/mundo.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Planeación General" href="PlaneacionOperaciones.aspx">Planeación estimada general</a></li>
        <li><a title="Matriz General" href="MatrizEstimacionGeneral.aspx">Matriz Estimación General</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Tráfico General de Operaciones" href="TraficoGeneralOperaciones.aspx">Tráfico general de operaciones</a></li>
        <li><a title="" href="#"></a></li>
        </ul>
            <ul>
        <li><a title="Reporte producción" href="RP_RegistroProduccionOP.aspx">Reporte producción</a></li>
        <li><a title="" href="#"></a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element4--> 

<div class="menu-element1"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Campo</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Consultar Errores" href="../MBO/CampoErroresUnEstudio.aspx">Consultar errores</a></li>
        <li><a title="Calidad encuestadores" href="../MBO/CampoEncuestadores.aspx">Calidad encuestadores</a></li>

        </ul>
        </div>
        <div class="linea2">
        <ul>
            <li><a title="Top 10 de Encuestadores (Anulación, Errores y VIP)" href="Top10Encuestadores.aspx">Top 10 Encuestadores</a></li>
            <li><a title="Ficha de Encuestador" href="../TH_TalentoHumano/ListadoEncuestadores.aspx">Ir a listado de encuestadores</a></li>
            <li><a title="Producción de Campo" href="ProduccionCampoPorFecha.aspx">Producción de campo</a></li>
            <li><a title="Planeación" href="PlaneacionGeneralOperaciones.aspx">Planeación Encuestas y Encuestadores</a></li>
            <li><a title="Asignación y Ejecución" href="AsignacionYEjecucionCampo.aspx">Asignación y Ejecución de Campo</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element1-->

<div class="menu-element2"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Crítica y Verificación</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a></a></li>
        <li><a title="Informe de anulación" href="InformeAnulacion.aspx">Informe de anulación</a></li>
        <li><a title="Tráfico de Encuestas" href="TraficoAreasGeneral.aspx">Tráfico de Encuestas</a></li>
        <li><a title="Exportado de errores de campo" href="../RP_Reportes/ErroresDecampo.aspx">Errores de Campo</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element2-->

<div class="menu-element3"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Procesos internos</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/polea.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="" href="PlaneacionEstudiosPorSalir.aspx">Trabajos planeados</a></li>
        <li><a title="" href="ReporteListadoTrabajos.aspx">Listado de Trabajos</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element3-->
<div style="visibility:hidden; display:none;">
<div class="menu-element4"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Talento Humano</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/comunicacion.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="" href="#"></a></li>
        <li><a title="" href="#"></a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element4--> 

<div class="menu-element5"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Financiero</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Ver listado de proyectos" href="Proyectos.aspx">Mis proyectos</a></li>
        <li><a title="" href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element5-->        

    <div class="menu-element6"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Calidad</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/tv.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Radicación de Peticiones, Quejas y Reclamos" href="FormPQR.aspx">PQR</a></li>
        <li><a title="" href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element6-->
</div>
       </nav>

        </div>

    </div>
    <div class="next"><a href="#anterior" title="Anterior"></a></div>
</asp:Content>
