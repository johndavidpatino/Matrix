<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/ES_.master" CodeBehind="Default.aspx.vb" Inherits="WebMatrix._Default2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/slider.css" media="screen" />
    <script type="text/javascript" src="../Scripts/slider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Section" runat="server">
    <div class="prev"><a href="#anterior"  title="Anterior"></a></div>
       <div id="slider">
       <div class="slidesContainer">
       <nav class="slide">
       
<div class="menu-element1"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Brief</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Brief's Pendiente de Respuesta" href="BriefDisenoDeMuestra.aspx?pendientes=1">Pendientes de respuesta</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Ver todos los brief's de diseño de muestra" href="BriefDisenoDeMuestra.aspx">Ver todos</a></li>
        <li><a>&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element1-->

<div class="menu-element2"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Diseños Muestrales</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/mensaje.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Ver Diseños Muestrales" href="DisenoDeMuestra.aspx">Ver</a></li>
        <li><a>&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element2-->

<div class="menu-element3"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Tareas</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/compras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Realizar Gestión de Tareas de la Unidad" href="../RE_GT/TraficoTareas.aspx?UnidadId=13&RolId=33&URLRetorno=15">Gestionar Tareas</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a>&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element3-->


<div class="menu-element4"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Metodología de Campo</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Trabajos pendientes de metodología" href="MetodologiaDeCampo.aspx?pendientes=yes">Metodologías Pendientes</a></li>
            <li><a title="Todos los trabajos" href="MetodologiaDeCampo.aspx">Todos los trabajos</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a>&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element4-->

       
       </nav>
       
       </div>
       
       </div>
       <div class="next"><a href="#anterior"  title="Anterior"></a></div>
</asp:Content>