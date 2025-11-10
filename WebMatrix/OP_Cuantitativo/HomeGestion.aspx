<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/OPQ_.master"
    CodeBehind="HomeGestion.aspx.vb" Inherits="WebMatrix._HomeGestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/slider.css" media="screen" />
    <script type="text/javascript" src="../Scripts/slider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Section" runat="server">
    <div class="prev">
        <a href="#anterior" title="Anterior"></a>
    </div>
    <div id="slider">
        <div class="slidesContainer">
            <nav class="slide">
       

<div class="menu-element1"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Crítica</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/mensaje.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Cargar Errores" href="../MBO/CargarErrores.aspx">Cargar errores</a></li>
        <li><a title="Consultar Errores" href="../MBO/CampoErroresUnEstudio.aspx">Consultar errores</a></li>
        <li><a title="Calidad encuestadores" href="../MBO/CampoEncuestadores.aspx">Calidad encuestadores</a></li>
        <li><a title="Exportado de errores de campo" href="../RP_Reportes/ErroresDecampo.aspx">Errores de Campo</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Tráfico y seguimiento de encuestas y recursos" href="../OP_Cuantitativo/TraficoEncuestas.aspx?UnidadId=28">Tráfico y Recursos</a></li>
        <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=5&RolId=22&URLRetorno=7">Gestionar tareas</a></li>
        <li><a title="Tabular Estudios" href="../RE_GT/SeleccionarPreguntasTabular.aspx">TabularEstudios</a></li>
        <li><a href="#">&nbsp;</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element1-->

<div class="menu-element2"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Verificación</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/compras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Tráfico y seguimiento de encuestas y recursos" href="../OP_Cuantitativo/TraficoEncuestas.aspx?UnidadId=20">Tráfico y Recursos</a></li>
        <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=6&RolId=23&URLRetorno=8">Gestionar tareas</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element2-->

<div class="menu-element3"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Captura</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/tv.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Tráfico y seguimiento de encuestas y recursos" href="../OP_Cuantitativo/TraficoEncuestas.aspx?UnidadId=21">Tráfico y Recursos</a></li>
        <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=7&RolId=24&URLRetorno=9">Gestionar tareas</a></li>
        <li><a title="" href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element3-->
 
 <div class="menu-element4"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Codificación</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/mensaje.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=8&RolId=25&URLRetorno=10">Gestionar tareas</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element4-->

<div class="menu-element5"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">DataCleaning</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/compras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=9&RolId=27&URLRetorno=11">Gestionar tareas</a></li>
        <li><a title="Trafico tareas" href="../CORE/ListaTareas-Trafico.aspx?Permiso=110&ProcesoId=9">Trafico tareas</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element5-->

<div class="menu-element6"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Procesamiento</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/tv.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Gestión y Ejecución de Tareas" href="../RE_GT/TraficoTareas.aspx?UnidadId=10&RolId=26&URLRetorno=12">Gestionar tareas</a></li>
                <li><a title="Trafico tareas" href="../CORE/ListaTareas-Trafico.aspx?Permiso=112&ProcesoId=10">Trafico tareas</a></li>
        <li><a title="" href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element6-->
       </nav>
        </div>
    </div>
    <div class="next">
        <a href="#anterior" title="Anterior"></a>
    </div>
</asp:Content>
