<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RD_.master" CodeBehind="RecoleccionDeDatos.aspx.vb" Inherits="WebMatrix._RecoleccionDeDatos" %>
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
    <div class="name-menu"><a href="#">Gerencia de Operaciones</a></div>
    <div class="icon-menu"><img src="../images/iconos-secciones/ejecutivo.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="" href="">&nbsp;&nbsp;&nbsp;</a></li>
        <li><a title="Asignación de coordinación de estudios" href="AsignacionCOE.aspx">AsignarOMP</a></li>
        <li><a title="Asignación JBI a Proyectos" href="AsignacionJBI.aspx">AsignarJBI</a></li>
        <li><a title="Revisar Presupuestos" href="../CU_Cuentas/RevisionPresupuestos.aspx">RevisarPresupuestos</a></li>
        <li><a title="AjustarCostos" href="../CAP/PresupuestosAprobados.aspx?opt=2">AjustarCostos</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Trabajos atrasados" href="../RP_Reportes/TrabajosConAtraso.aspx">Trabajos atrasados</a></li>
        <li><a title="Trabajos por gerencia" href="../RP_Reportes/TrabajosPorGerencia.aspx">Seguimiento</a></li>
        <li><a title="Planeación General de Operaciones" href="../RP_Reportes/PlaneacionOperaciones.aspx">Planeación Tráfico</a></li>
        <li><a title="Informe produccion" href="../MBO/CampoProduccion.aspx">Produccion</a></li>
        <li><a title="Calidad campo" href="../MBO/CampoCalidadTotal.aspx">Calidad campo</a></li>
        <li><a title="Listado encuestadores - Ficha Encuestador" href="../TH_TalentoHumano/ListadoEncuestadores.aspx">Ficha de Encuestador</a></li>
         <li><a title="Tiempos de revisión de presupuestos" href="../RP_Reportes/InformeTiemposRevisionPresupuestos.aspx">Tiempos revisión</a></li>
        <li><a title="Cambios de JobBook Interno" href="../RE_GT/CambiosJBI.aspx">Cambios JBI</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element1-->

<div class="menu-element3"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu">Subdirección Operativa</div>
    <div class="icon-menu"><img src="../images/iconos-secciones/movil.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Planeación Operaciones General" href="../RP_Reportes/PlaneacionOperaciones.aspx">Planeación Estimada General</a></li>
        <li><a title="Planeación Campo" href="../RP_Reportes/PlaneacionCampo.aspx">Planeación Estimada Campo</a></li>
        <li><a title="Planeación Propuestas" href="../RP_Reportes/PlaneacionPropuestas.aspx">Planeación Propuestas</a></li>
        <li><a title="Planeación Propuestas" href="../RP_Reportes/PlaneacionEstudios.aspx">Planeación Estudios</a></li>
        <li><a title="Planeación Propuestas" href="../RP_Reportes/PlaneacionPropuestasYEstudios.aspx">Planeación Total</a></li>
        <li><a title="Listado propuestas" href="../RP_Reportes/ListadoPropuestasSeguimiento.aspx">Seguimiento de Propuestas</a></li>
        <li><a title="Listado anuncios de aprobación" href="../RP_Reportes/ListadoEstudiosSeguimiento.aspx">Seguimiento de Anuncios</a></li>
        </ul>
        </div>
        <div class="linea2">
            <ul>
            <li><a title="Trabajos atrasados" href="../RP_Reportes/TrabajosConAtraso.aspx">Trabajos atrasados</a></li>
        <li><a title="Seguimiento" href="../RP_Reportes/TrabajosPorGerencia.aspx">Seguimiento</a></li>
        <li><a title="Tráfico" href="../RP_Reportes/TraficoAreasGeneral.aspx">Tráfico Encuestas</a></li>
            </ul>
        </div> 
    </div>
</div><!-- menu-element3-->

<div class="menu-element2"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu"><a href="#">Operaciones Cualitativas</a></div>
    <div class="icon-menu"><img src="../images/iconos-secciones/comunicacion.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Operaciones Cualitativas" href="../OP_Cualitativo/HomeRecoleccion.aspx">Ir a Operaciones Cualitativas</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="" href=""></a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element4-->

<div class="menu-element5"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu"><a href="#">Coordinación de Campo</a></div>
    <div class="icon-menu"><img src="../images/iconos-secciones/compras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Asignación de coordinación de estudios" href="AsignacionCoordinador.aspx?TipoTecnicaid=1">Asignar Coordinador de Campo</a></li>
        <li><a title="Personal PST-Contratistas" href="../TH_TalentoHumano/PrestacionServicios-CT.aspx">Personal PST-Contratistas</a></li>
        <li><a title="Consultar Errores" href="../MBO/CampoErroresUnEstudio.aspx">Consultar errores</a></li>
        <li><a title="Calidad encuestadores" href="../MBO/CampoEncuestadores.aspx">Calidad encuestadores</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element2-->
 
<div class="menu-element4"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu"><a href="#">Gerencia de Campo</a></div>
    <div class="icon-menu"><img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        </div>
        <div class="linea2">
        <ul>
            <li><a title="Planeación" href="../RP_Reportes/PlaneacionCampo.aspx">Planeación Campo</a></li>
            <li><a title="Top 10 de Encuestadores (Anulación, Errores y VIP)" href="../RP_Reportes/Top10Encuestadores.aspx">Top 10 Encuestadores</a></li>
            <li><a title="Producción de Campo" href="../RP_Reportes/ProduccionCampoPorFecha.aspx">Producción de campo</a></li>
            <li><a title="Consultar Errores" href="../MBO/CampoErroresUnEstudio.aspx">Consultar errores</a></li>
            <li><a title="Calidad encuestadores" href="../MBO/CampoEncuestadores.aspx">Calidad encuestadores</a></li>
            <li><a title="Encuestadores y supervisores sin producción" href="../RP_Reportes/PersonalSinProduccion.aspx">Personas sin producción</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element7-->

<div class="menu-element6"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu"><a href="#">Operaciones Cuantitativas</a></div>
    <div class="icon-menu"><img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Operaciones Cuantitativas" href="../OP_Cuantitativo/HomeRecoleccion.aspx">Ir a Operaciones Cuantitativas</a></li>
        <li><a href="#">&nbsp;</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        </ul>
        </div> 
    </div>
    <br />
</div><!-- menu-element3-->
       </nav>
       
       </div>
       
       </div>
       <div class="next"><a href="#anterior"  title="Anterior"></a></div>
</asp:Content>
