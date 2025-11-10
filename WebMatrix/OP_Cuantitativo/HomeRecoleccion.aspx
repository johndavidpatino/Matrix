<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_.master" CodeBehind="HomeRecoleccion.aspx.vb" Inherits="WebMatrix._HomeRecoleccion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/slider.css" media="screen" />
    <script type="text/javascript" src="../Scripts/slider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Section" runat="server">
    <div class="prev"><a href="#anterior" title="Anterior"></a></div>
    <div id="slider">
        <div class="slidesContainer">
            <nav class="slide">
      
<div class="menu-element1"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">OMP</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/mensaje.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Trabajos" href="Trabajos.aspx">Trabajos</a></li>
        <li><a title="Consultar Errores" href="../MBO/CampoErroresUnEstudio.aspx">Consultar errores</a></li>
            <li><a title="Calidad encuestadores" href="../MBO/CampoEncuestadores.aspx">Calidad encuestadores</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element1-->

<div class="menu-element2"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Coordinador de Campo</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/tv.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Trabajos" href="TrabajosCoordinador.aspx">Trabajos</a></li>
        <%--<li><a title="Inclusión de encuestadores y supervisores" href="../TH_TalentoHumano/Contratacion.aspx?coordinador=asjflasjfklsaf9a09f809fjflas0">Creación de personal</a></li>--%>
        <li><a title="Consultar Errores" href="../MBO/CampoErroresUnEstudio.aspx">Consultar errores</a></li>
        <li><a title="Calidad encuestadores" href="../MBO/CampoEncuestadores.aspx">Calidad encuestadores</a></li>
        <li><a title="" href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element2-->

<div class="menu-element6"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Call Center</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
            <li><a title="Trabajos" href="TrabajosCallCenter.aspx">Trabajos</a></li>    
            <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=14&RolId=49&URLRetorno=18">Gestionar tareas</a></li>
            <li><a title="Cargar productividad" href="ImportarDatos.aspx">Cargar productividad</a></li>    
            <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element6--> 

<div class="menu-element3"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Scripting</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
                <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=11&RolId=28&URLRetorno=5">Gestionar tareas</a></li>
                <li><a title="Trafico tareas" href="../CORE/ListaTareas-Trafico.aspx?Permiso=112&ProcesoId=10">Trafico tareas</a></li>
                <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element3--> 

<div class="menu-element4"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Mystery</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
                <li><a title="Cargar productividad" href="ImportarDatos.aspx">Cargar productividad</a></li>
                <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element4--> 

<div class="menu-element5"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">RMC</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
                <li><a title="Tráfico y seguimiento de encuestas y recursos" href="../OP_Cuantitativo/TraficoEncuestas.aspx?UnidadId=38">Tráfico y Recursos</a></li>
            <li><a title="Cargar productividad" href="ImportarDatos.aspx">Cargar productividad</a></li>
                <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element5--> 
       </nav>

        </div>

    </div>
    <div class="next"><a href="#anterior" title="Anterior"></a></div>
</asp:Content>
