<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MBO_.master" CodeBehind="Default.aspx.vb" Inherits="WebMatrix._DefaultMBO" %>
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
    <div class="name-menu"><a href="#">MBO  Gerencial</a></div>
    <div class="icon-menu"><img src="../images/iconos-secciones/comunicacion.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Opcion 1" href="#"></a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="MBO Gerencia" href="AOTGerencia.aspx">AOT</a></li>
        <li><a title="Presupuestos" href="../CU_Cuentas/AutorizacionPresupuestosDirectores.aspx">Autorizar Cambios GM</a></li>
        <li><a title="Matrix gestion" href="MatrixGestion.aspx">Gestion en Matrix</a></li>
        <li><a title="Estado propuestas" href="PropuestasEstadoTotal.aspx">Propuestas sin actualizar</a></li>
        <li><a title="Propuestas sin Trabajo" href="PropuestasSinTrabajo.aspx">Propuestas por entregar a Operaciones</a></li>
        <li><a title="Listado anuncios de aprobación" href="../RP_Reportes/ListadoEstudiosSeguimiento.aspx">Seguimiento de Anuncios</a></li>
        <li><a>&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element1-->

<div class="menu-element2"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu"><a href="#">Direccion de Operaciones</a></div>
    <div class="icon-menu"><img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="Planeación Operaciones General" href="../RP_Reportes/PlaneacionOperaciones.aspx">Planeación General</a></li>
        <li><a title="Planeación Campo" href="../RP_Reportes/PlaneacionCampo.aspx">Planeación Campo</a></li>
        <li><a title="Planeación basada en propuestas" href="../RP_Reportes/PlaneacionPropuestas.aspx">Planeación Propuestas</a></li>
        <li><a title="Informe anulación" href="../RP_Reportes/InformeAnulacion.aspx">Informe de anulación</a></li>
        <li><a title="Tiempos de revisión de presupuestos" href="../RP_Reportes/InformeTiemposRevisionPresupuestos.aspx">Tiempos revisión presupuestos</a></li>
        <li><a title="Registro Producto No Conforme" href="../MBO/ProductoNoConformeRegistrar.aspx">Registro Producto No Conforme</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Trabajos atrasados" href="../RP_Reportes/TrabajosConAtraso.aspx">Trabajos atrasados</a></li>
        <li><a title="Seguimiento" href="../RP_Reportes/TrabajosPorGerencia.aspx">Seguimiento</a></li>
        <li><a title="Tráfico de Encuestas" href="../RP_Reportes/TraficoAreasGeneral.aspx">Tráfico</a></li>        
        <li><a title="Producción campo" href="CampoProduccion.aspx">Producción Campo</a></li>
        <li><a title="Calidad campo" href="CampoCalidadTotal.aspx">Calidad Campo</a></li>
        <li><a title="Listado encuestadores - Ficha Encuestador" href="../TH_TalentoHumano/ListadoEncuestadores.aspx">Ficha de Encuestador</a></li>
        <li><a title="Top 10 de Encuestadores (Anulación, Errores y VIP)" href="../RP_Reportes/Top10Encuestadores.aspx">Top 10 Encuestadores</a></li>
        <li><a>&nbsp;</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element2-->

<div class="menu-element3"><!-- espacio para elementos en el pie de la aplicación-->
    <div class="name-menu"><a href="#">Director de Unidad</a></div>
    <div class="icon-menu"><img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea"></div>
    <div class="submenu">
        <div class="linea1">
        <ul>
        <li><a title="AOT" href="../MBO/AOTDireccion.aspx">AOT</a></li>
        <li><a title="Registro Producto No Conforme" href="../MBO/ProductoNoConformeRegistrar.aspx">Registro Producto No Conforme</a></li>
        <li><a href="../RP_Reportes/ListadoPlaneacionUnidades.aspx" target="_blank">Planeación OPS</a></li>
        </ul>
        </div>
        <div class="linea2">
        <ul>
        <li><a title="Cambios en presupuestos" href="../CU_Cuentas/AutorizacionPresupuestosDirectores.aspx">Cambiar Presupuestos</a></li>
        <li><a title="Cambios en presupuestos" href="../RP_Reportes/ListadoBrief.aspx">Viabilidad briefs</a></li>
        <li><a title="Listado propuestas" href="../RP_Reportes/ListadoPropuestasSeguimiento.aspx">Seguimiento Propuestas</a></li>
        <li><a title="Estado propuestas" href="PropuestasEstadoUnidad.aspx">Propuestas sin actualizar</a></li>
        <li><a title="Propuestas sin Trabajo" href="PropuestasSinTrabajo.aspx">Propuestas por entregar a Operaciones</a></li>
        <li><a title="Tiempos de revisión de presupuestos" href="../RP_Reportes/InformeTiemposRevisionPresupuestos.aspx">Tiempos revisión Presupuestos</a></li>
        <li><a title="Cambiar Gerente de Cuentas" href="../CU_Cuentas/CambiarGerenteCuentasBriefs.aspx">Cambiar Gerente Cuentas</a></li>
        </ul>
        </div> 
    </div>
</div><!-- menu-element3-->
 
       </nav>
       
       </div>
       
       </div>
       <div class="next"><a href="#anterior"  title="Anterior"></a></div>
</asp:Content>
