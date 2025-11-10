<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/TH_.master" CodeBehind="Default.aspx.vb" Inherits="WebMatrix._Default5" %>
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
    <div class="name-menu">Capacitaciones</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Ver capacitaciones realizadas" href="Capacitacion.aspx">Ver Capacitaciones</a></li>
        <li><a title="Registro Producto No Conforme" href="../MBO/ProductoNoConformeRegistrar.aspx">Registro Producto No Conforme</a></li>
        <li><a title="Opcion 2" href="#"></a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element1-->

<div class="menu-element2"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Hojas de vida</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/mensaje.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Listado de hojas de vida" href="ListadoHojasDeVida.aspx">Listas hojas de vida</a></li>
        <li><a></a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element2-->

<div class="menu-element3"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Usuarios</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/idea.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href=""></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Gestión de usuarios, sus unidades y sus permisos" href="../US_Usuarios/Usuarios.aspx">Gestión de Usuarios</a></li>
        <li><a title="Unidades en el sistema" href="../US_Usuarios/TipoGrupoUnidad.aspx">Unidades</a></li>
        <li><a title="Permisos del sistema" href="../US_Usuarios/GruposPermisos.aspx">Permisos</a></li>
        <li><a title="Roles del sistema" href="../US_Usuarios/Roles.aspx">Roles</a></li>
        <li><a title="" href="">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element3-->

<div class="menu-element4"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Contratación</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/cine.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href=""></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Módulo de Contratación" href="Contratacion.aspx">Contratistas</a></li>
        <li><a title="Módulo de Personal" href="Personas.aspx">Empleados y temporales</a></li>
        <li><a title="Reporte general en Excel" href="../RP_Reportes/ListadoGeneralMatrix.aspx">Exportar Reporte</a></li>
        <li><a title="Encuestadores y supervisores sin producción" href="../RP_Reportes/PersonalSinProduccion.aspx">Personas sin producción</a></li>
        <li><a title="PST-Contratistas" href="PrestacionServicios-CT.aspx">PST-Contratistas</a></li>
        <li><a title="Reporte Cambios Contrataciones" href="ReporteCambiosContratacion.aspx">Reportes Cambios Contrataciones</a></li>
        <li><a title="" href="">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element4-->

<div class="menu-element5"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Ficha de encuestador</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href=""></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Ficha de Encuestador" href="ListadoEncuestadores.aspx">Ir a listado de encuestadores</a></li>
        <li><a title="" href="">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element5-->

       
       </nav>
       
       </div>
       
       </div>
       <div class="next"><a href="#anterior"  title="Anterior"></a></div>
</asp:Content>
