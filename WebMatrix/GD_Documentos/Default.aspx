<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/GD_.master" CodeBehind="Default.aspx.vb" Inherits="WebMatrix._Default3" %>

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
                    <div class="name-menu">Actas</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/notas.png" width="65" height="65" alt="polea" />
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                            <ul>
                                <li><a title="Opcion 1" href="#">Pendientes de propuesta</a></li>
                            </ul>
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a title="Opcion 1" href="../SG_Actas/Actas.aspx">Crear</a></li>
                                <li><a title="Opcion 2" href="../SG_Actas/Seguimiento.aspx">Hacer seguimiento</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element1-->

                <div class="menu-element2">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">Retroalimentación</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/mensaje.png" width="65" height="65" alt="polea" />
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                            <ul>
                                <li><a title="Opcion 1" href=""></a></li>
                            </ul>
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a title="Responder a las retroalimentaciones de los usuarios" href="../US_Usuarios/SeguimientoFeedback.aspx">Seguimiento y respuestas</a></li>
                                <li><a href="#">&nbsp;</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element2-->

                <div class="menu-element3">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">MBO</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea">
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a title="Indicadores reportados manualmente" href="../MBO/IndicesManualesCuentas.aspx">Cuentas y Proyectos</a></li>
                                <li><a href="#">&nbsp;</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element2-->

                <div class="menu-element4">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">Seguimiento</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/cifras.png" width="65" height="65" alt="polea" />
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                            <ul>
                                <li><a title="Opcion 1" href=""></a></li>
                            </ul>
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a title="Seguimiento tareas" href="../CORE/ListaTareas-Trafico.aspx?Permiso=29">Seguimiento tareas</a></li>
                                <li><a title="Configuración tareas" href="../CORE/Configuracion_Tareas.aspx">Configuración tareas</a></li>
                                <li><a title="Configuración tareas X hilo" href="../CORE/ConfiguracionTareasXHilo.aspx">Configuración tareas X hilo</a></li>
                                <li><a title="Registro Producto No Conforme" href="ProductoNoConformeRegistrar.aspx">Registro Producto No Conforme</a></li>
                                <li><a title="Seguimiento PNC" href="GD_SeguimientoPNC.aspx">Seguimiento PNC</a></li>
                                <li><a href="#">&nbsp;</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- menu-element2-->

                <div class="menu-element6">
                    <!-- espacio para elementos en el pie de la aplicación-->
                    <div class="name-menu">Reportes Calidad</div>
                    <div class="icon-menu">
                        <img src="../images/iconos-secciones/tv.png" width="65" height="65" alt="polea">
                    </div>
                    <div class="submenu">
                        <div class="linea1">
                            <ul>
                                <li><a title="Opcion 1" href=""></a></li>
                            </ul>
                        </div>
                        <div class="linea2">
                            <ul>
                                <li><a title="Errores Registro Observaciones" href="/RP_Reportes/Calidad/IndicadoresRegistroObservaciones.aspx">Errores Registro Observaciones</a></li>
                                <li><a title="Registro Observaciones Por Tipo" href="../RP_Reportes/ReporteRegistroObservacionesTipo.aspx">Registro Observaciones Por Tipo</a></li>
                                <li><a title="Indicador de Cuestionario" href="/RP_Reportes/Calidad/IndicadoresCumplimientoTareas.aspx">Cumplimiento de tareas</a></li>
                                <li><a href="../RP_Reportes/ReporteInconsistencias.aspx">Reporte de Observaciones</a></li>                                
                                <li><a href="../RP_Reportes/IndicadoresCalidad.aspx">Indicadores Calidad</a></li>
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
