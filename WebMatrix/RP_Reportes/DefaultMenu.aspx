<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/RP.master" CodeBehind="DefaultMenu.aspx.vb" Inherits="WebMatrix.DefaultMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../Styles/slider.css" media="screen" />
    <script type="text/javascript" src="../Scripts/slider.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Section" runat="server">
    <div class="prev"><a href="#anterior" title="Anterior"></a></div>
    <div id="slider">
        <div class="slidesContainer">
            <div class="slide">
                <div class="menu-element1">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">Cuentas</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/ejecutivo.png" width="65" height="65" alt="polea">
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                            <ul>
                                <li><a></a></li>
                            </ul>
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a href="ListadoPropuestasSeguimientoCCT.aspx">Propuestas por CCT</a></li>
                                <li><a href="TrabajosPorCCT.aspx">Trabajos por CCT</a></li>
                                <li><a href="EstudiosXEntregarCCT.aspx">Estudios sin entregar</a></li>
                                <li><a href="../MBO/PropuestasSinTrabajo.aspx">Propuestas sin trabajo</a></li>
                                <li><a></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element1-->

                <div class="menu-element2">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">Proyectos</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea">
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                            <ul>
                                <li><a title="" href="#"></a></li>
                            </ul>
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a title="Reporte de Proyectos sin JBI" href="ReporteProyectosSinJBI.aspx">Proyectos sin JBI</a></li>
                                <li><a title="" href="#">&nbsp;</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element2-->

                <div class="menu-element3">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">Operaciones</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/polea.png" width="65" height="65" alt="polea">
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                            <ul>
                                <li><a></a></li>
                            </ul>
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a title="Ver menú de reportes de operaciones" href="MenuOperacionesREP.aspx">Ver reportes</a></li>
                                <li><a title="Ver menú de reportes de Requerimientos de Servicio" href="DetalleRequerimientosReporte.aspx">Detalle Requerimiento de Servicio</a></li>
                                <li><a title="" href="#">&nbsp;</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element3-->

                <div class="menu-element4">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">Talento Humano</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/comunicacion.png" width="65" height="65" alt="polea">
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                            <ul>
                                <li><a title="" href="#"></a></li>
                            </ul>
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a title="" href="#"></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element4-->

                <div class="menu-element5">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">Financiero</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea">
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                            <ul>
                                <li><a title="Opcion 1" href="#"></a></li>
                            </ul>
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a href="ReporteActividades.aspx">Presupuesto y ejecucion por actividad</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element5-->

                <div class="menu-element6">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">Calidad</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/tv.png" width="65" height="65" alt="polea">
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a title="Errores Registro Observaciones" href="../RP_Reportes/ReportesIndicadoresRegistroObservaciones.aspx">Errores Registro Observaciones</a></li>
                                <li><a title="Registro Observaciones Por Tipo" href="../RP_Reportes/ReporteRegistroObservacionesTipo.aspx">Registro Observaciones Por Tipo</a></li>
                                <li><a title="Indicador de Cuestionario" href="../RP_Reportes/ReportesCumplimientoTareas.aspx">Cumplimiento de tareas</a></li>
                                <li><a href="../RP_Reportes/ReporteInconsistencias.aspx">Reporte de Observaciones</a></li>                                
                                <li><a href="../RP_Reportes/IndicadoresCalidad.aspx">Indicadores Calidad</a></li>
                                <li><a href="ReporteTablets.aspx">Reporte de Tablets</a></li>
                                <li><a href="PlaneacionPorUnidad.aspx">Planeación Tareas Cuentas y Proyectos</a></li>
                                <li><a href="PlaneacionPorUnidad.aspx">Planeación Tareas Operaciones</a></li>
                                <li><a href="ReporteEvaluacionProveedores.aspx">Evaluación de Proveedores</a></li> 
                                <li><a href="#"></a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element6-->
            </div>
        </div>
    </div>
    <div class="next"><a href="#anterior" title="Anterior"></a></div>
</asp:Content>
