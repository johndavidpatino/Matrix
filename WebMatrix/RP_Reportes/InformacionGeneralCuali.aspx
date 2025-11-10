<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral.Master" CodeBehind="InformacionGeneralCuali.aspx.vb" Inherits="WebMatrix.InformacionGeneralCuali" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            $('#tabs').tabs();

            $('#accordionFrame').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEsquema').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionPropuesta').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEspecificaciones').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEspecificacionesTecnicas').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionMetodologia').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEspecificacionesAdicionales').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEsquemaAnalisis').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#BusquedaVersionesEC').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Versiones de Especificaciones Cuentas a Proyectos",
                    width: "600px"
                });

            $('#BusquedaVersionesEC').parent().appendTo("form");

            $('#BusquedaVersionesET').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Versiones de Especificaciones Técnicas del Trabajo",
                    width: "600px"
                });

            $('#BusquedaVersionesET').parent().appendTo("form");
        });

        function MostrarVersionesEC(e) {
            var a = $(e).attr('id');
            var text = $("#" + a).text();
            if (text == "Sin Versiones") {
                alert("No hay versiones disponibles");
            } else {
                $('#BusquedaVersionesEC').dialog("open");
            }
        }

        function MostrarVersionesET(e) {
            var a = $(e).attr('id');
            var text = $("#" + a).text();
            if (text == "Sin Versiones") {
                alert("No hay versiones disponibles");
            } else {
                $('#BusquedaVersionesET').dialog("open");
            }
        }

        function MostrarVersionesEA(e) {
            var a = $(e).attr('id');
            var text = $("#" + a).text();
            if (text == "Sin Versiones") {
                alert("No hay versiones disponibles");
            } else {
                $('#BusquedaVersionesEA').dialog("open");
            }
        }
    </script>
    <style>
        .ui-widget-header {
            background-color: #3b9f9a;
            border: 1px solid #3b9f9a;
        }

        .text-center {
            margin: 0px auto;
            text-align: center;
        }

        .cambioVersion1 {
            background-color: #ea7b7e;
            color: white;
            border: 1px solid #000 !important;
        }

        .cambioVersion {
            /*background-color: #52bb69;*/
            background-color: transparent;
            color: black;
            border: 1px solid #000 !important;
        }

        .lblScroll {
            overflow-x: scroll;
            overflow-y: scroll;
            border: 1px solid;
        }

        .lblScrolllbl {
            overflow-x: scroll;
            padding: 10px;
            border: 1px solid;
        }

        .versionIgual {
            background-color: transparent;
            color: black;
            border: 1px solid;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
    Matrix
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Información General del Trabajo
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
    Aquí se encuentra toda la información correspondiente al Proyecto o Trabajo seleccionado
    <br />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Content" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#tabs').tabs();

            $('#accordionFrame').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEsquema').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionPropuesta').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEspecificaciones').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });

            $('#accordionEsquemaAnalisis').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h2",
                autoHeight: true,
                collapsible: true,
                active: false
            });
        }
    </script>
    <style>
        #stylized.leftAuto {
            text-align: initial;
        }

        #stylized label {
            text-align: left;
            margin-left: 10px;
            width: auto;
        }

        #stylized input[type=radio] {
            margin: 10px;
        }

        #stylized input[type=checkbox] {
            margin-top: 7px;
        }
    </style>
    <asp:HiddenField ID="hfTrabajoID" runat="server" />
    <asp:HiddenField ID="hfProyectoID" runat="server" />
    <asp:HiddenField ID="hfversion" runat="server" />
    <asp:HiddenField ID="hfVolver" runat="server" />

    <div id="accordionFrame">
        <h2>Información del frame</h2>
        <asp:Panel ID="pnlFrame" runat="server">
            <div class="spacer"></div>
            <div id="tabs">
                <ul>
                    <li><a href="#tabs-1">Objetivos de Negocio</a></li>
                    <li><a href="#tabs-2">Decisiones</a></li>
                    <li><a href="#tabs-3">Competencia</a></li>
                    <li><a href="#tabs-4">Metodología</a></li>
                    <li><a href="#tabs-5">Datos de Diseño</a></li>
                    <li><a href="#tabs-6">Tiempos y Presupuesto</a></li>
                    <li><a href="#tabs-7">Entregables</a></li>
                </ul>
                <div id="tabs-1">
                    <div style="width: 100%; clear: both">
                        <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                            <p>¿Qué necesidad de negocios está tratando de resolver?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtO1" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Cuáles son sus objetivos estratégicos para este estudio?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtO2" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Hay algún factor importante a considerar para el diseño del estudio?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtO3" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿El problema planteado está relacionado a alguna área en específico de la empresa?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtO4" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Por qué está haciendo este estudio?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtO5" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Qué hipótesis tiene sobre el tema (s) está estudiando?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtO6" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Cuáles son los indicadores clave de rendimiento requerido?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtO7" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                </div>
                <div id="tabs-2">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Qué decisiones va a tomar como resultado de este estudio?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtD1" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Quién va a utilizar esta investigación y qué van a hacer con él?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtD2" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Hay alguna política de su empresa /reglas que se tienen que considerar ir, sin cambio en el precio, empaquetado, etc. protocolo global?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtD3" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                </div>
                <div id="tabs-3">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Cuáles son los desafíos que enfrenta su  negocio en el mercado? ¿Cómo es el desempeño de la industria?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtC1" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Quiénes son sus competidores? ¿Necesita alguna evaluación comparativa del estudio?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtC2" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Cuál es el posicionamiento de su marca y cuáles son las diferencias vs. su competencia?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtC3" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Qué actividades recientes hay en el mercado que podrían haber influido en el mercado?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtC4" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Dónde ve su oportunidad y las amenazas en su caso?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtC5" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>

                </div>
                <div id="tabs-4">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Necesita comparar esta investigación contra alguna investigación previa?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtM1" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Cuál fue el diseño de esa investigación previa?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtM2" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Cuándo se realizó?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtM3" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                </div>
                <div id="tabs-5">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿En dónde tiene pensado hacer el levantamiento?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI1" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Cuál es la definición de su target?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI2" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Cómo segmenta su mercado? Por productos, por usuario, por zonas geográficas?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI3" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Tiene algún requisito de tamaño de la muestra?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI4" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Espera que la muestra debe sea representativa del mercado?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI5" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Tiene alguna metodología específica en mente, por ejemplo, cualitativa o cuantitativa?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI6" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Hay alguna información clave que necesite este en el reporte final?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI7" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                </div>
                <div id="tabs-6">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Para cuándo necesita los resultados. ¿Hay alguna limitante de tiempo?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI8" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Para cuándo necesita la propuesta?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI9" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>¿Cuándo cree que se tomará para tomar una decisión para comisionar el proyecto?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI10" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Con la idea de dimensionar el alcance del estudio para usted, podría compartir con nosotros cuál es el presupuesto que ha destinado a este estudio?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI11" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                </div>
                <div id="tabs-7">
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Por último, sólo quiero ver si usted tiene algún requerimiento especial en cuanto a formatos de entrega?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI12" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Topline de  resultados?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI13" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Presentación en ppt?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI14" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Transcripciones?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI15" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Presentaciones verbales?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI16" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Workshop?</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI17" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                    <div style="width: 100%; clear: both">
                        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                            <p>Otros, especifique:</p>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:Label ID="txtDI18" runat="server" Width="100%"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
    <br />
    <div id="accordionPropuesta">
        <h2>Información de la Propuesta</h2>
        <asp:Panel ID="pnlPropuesta" runat="server">
            <asp:Button ID="btnDescargarPropuesta" runat="server" Text="Descargar propuesta" Visible="false" />
            <div class="spacer"></div>
            <p>Información General</p>
            <asp:GridView ID="gvPropuestaInfoGeneral" runat="server" Width="100%" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="NoPropuesta" HeaderText="No Prop" />
                    <asp:BoundField DataField="Alternativa" HeaderText="Alt" />
                    <asp:BoundField DataField="Fase" HeaderText="Fase" />
                    <asp:BoundField DataField="Metodologia" HeaderText="Metod" />
                    <asp:BoundField DataField="GrupoObjetivo" HeaderText="Grupo Objetivo" />
                    <asp:BoundField DataField="Productividad" HeaderText="Productividad" />
                    <asp:BoundField DataField="Duracion" HeaderText="Duración" />
                    <asp:BoundField DataField="DiasCampo" HeaderText="D Campo" />
                    <asp:BoundField DataField="RequestHabeasData" HeaderText="Habeas Data" />
                </Columns>
            </asp:GridView>
            <p>Información de Muestra</p>
            <asp:GridView ID="gvPropuestaMuestra" runat="server" Width="100%" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Fase" HeaderText="Fase" />
                    <asp:BoundField DataField="Metodologia" HeaderText="Metod" />
                    <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                    <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                </Columns>
            </asp:GridView>
            <p>Información de Preguntas</p>
            <asp:GridView ID="gvPropuestaPreguntas" runat="server" Width="100%" AutoGenerateColumns="false">
                <Columns>
                    <asp:BoundField DataField="Fase" HeaderText="Fase" />
                    <asp:BoundField DataField="Metodologia" HeaderText="Metod" />
                    <asp:BoundField DataField="Abiertas" HeaderText="Abiertas" />
                    <asp:BoundField DataField="AbiertasMultiples" HeaderText="Abiertas Multiples" />
                    <asp:BoundField DataField="Cerradas" HeaderText="Cerradas" />
                    <asp:BoundField DataField="CerradasMultiples" HeaderText="Cerradas Multiples" />
                    <asp:BoundField DataField="Demograficos" HeaderText="Demograficos" />
                    <asp:BoundField DataField="Otras" HeaderText="Otras" />
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </div>
    <br />
    <div id="accordionEspecificaciones" style="padding: 0,0,0,0; margin: 0,0,0,0">
        <h2>Especificaciones de Cuentas a Proyectos</h2>
        <asp:Panel ID="pnlBriefCuentasProyectosCuali" runat="server" Visible="true">
            <form id="formCuentasProyectosCuali">
                <a href="#" id="lblVersionEspC" style="text-decoration: none; float: right" onclick="MostrarVersionesEC(this)" runat="server">Sin Versiones</a>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Observaciones Generales</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtObservacionesCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Técnica</p>
                    </div>
                    <div style="width: 80%; float: left; padding: 2px 2px 2px 2px">
                        <asp:RadioButtonList runat="server" ID="chbBCPTecnicaCuali" Width="100%" RepeatDirection="Horizontal" Enabled="false">
                            <asp:ListItem Value="1" Text="Entrevista"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Sesiones de grupo/Talleres"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Inmersiones"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Estudios online"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Otro"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div style="width: 18%; float: left; padding: 2px 2px 2px 2px; margin-top: -5px;">
                        <label>Otra Técnica</label>
                        <asp:TextBox ID="otraTecnica" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Incentivos</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPIncentivosEspCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Base de Datos</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPBDDEspCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Productos o Conceptos</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPProductoEspCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Método de reclutamiento</p>
                    </div>
                    <div style="width: 80%; float: left; padding: 2px 2px 2px 2px">
                        <asp:RadioButtonList runat="server" ID="chbBCPReclutamientoCuali" Width="100%" RepeatDirection="Horizontal" Enabled="false">
                            <asp:ListItem Value="1" Text="Base de datos"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Convencional"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Referidos"></asp:ListItem>
                            <asp:ListItem Value="4" Text="En frío"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Reclutamiento</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPEspReclutamientoCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both; margin-top: 5px;">
                    <asp:CheckBox ID="chbBCPEspProductoCuali" CssClass="leftAuto" TextAlign="Left" runat="server" Text="Especificaciones de Producto" Enabled="false" />
                </div>
                <div style="width: 98%; clear: both; margin-top: 5px;">
                    <asp:CheckBox ID="chbBCPMaterialEvalCuali" CssClass="leftAuto" TextAlign="Left" runat="server" Text="Materiales a Evaluar" Enabled="false" />
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Observaciones sobre el Producto</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPObsProductoCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
            </form>
        </asp:Panel>
    </div>
    <div id="accordionMetodologia" style="padding: 0,0,0,0; margin: 0,0,0,0; display: none;">
        <h2>Metodología de Campo</h2>
        <asp:Panel ID="pnlMetodologia" runat="server">
            <asp:Panel ID="pnlMetodologia1" runat="server">
                <a href="#" id="lblNumVersionMetodologia" style="text-decoration: none; float: right" onclick="MostrarVersionesET(this)" runat="server">Sin Versiones</a>

                <asp:Panel ID="pnlobjetivos" runat="server">
                    <label>
                        Grupo Objetivo</label>
                    <asp:Label ID="txtObjetivos" Width="100%" runat="server" />
                    <div class="spacer"></div>
                </asp:Panel>
                <asp:Panel ID="pnlmercado" runat="server">
                    <label>
                        Mercado, Cubrimiento geográfico</label>
                    <asp:Label ID="txtMercado" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlmarco" runat="server">
                    <label>
                        Marco muestral</label>
                    <asp:Label ID="txtMarcoMuestral" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnltecnica" runat="server">
                    <label>
                        Técnica</label>
                    <asp:Label ID="txtTecnica" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnldiseno" runat="server">
                    <label>
                        Diseño Muestral</label>
                    <asp:Label ID="txtDiseno" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlinstrucciones" runat="server">
                    <label>
                        Instrucciones para la recolección</label>
                    <asp:Label ID="txtInstrucciones" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnldistribucion" runat="server">
                    <label>
                        Distribución de la muestra</label>
                    <asp:Label ID="txtDistribucion" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlnivelconfianza" runat="server">
                    <label>
                        Nivel de confianza</label>
                    <asp:Label ID="txtNivelConfianza" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlmargenerror" runat="server">
                    <label>
                        Margen de Error Esperado</label>
                    <asp:Label ID="txtMargenError" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnldesagregacion" runat="server">
                    <label>
                        Desagregación básica de los resultados</label>
                    <asp:Label ID="txtDesagregacion" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlfuente" runat="server">
                    <label>
                        Fuente para la elaboración de la distribución muestral</label>
                    <asp:Label ID="txtFuente" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlVariables" runat="server">
                    <label>
                        Variables básicas de ponderación</label>
                    <asp:Label ID="txtVariables" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnltasa" runat="server">
                    <label>
                        Tasa de Respuesta</label>
                    <asp:Label ID="txtTasa" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlprocedimiento" runat="server">
                    <label>
                        Procedimiento de Imputación</label>
                    <asp:Label ID="txtprocedimiento" Width="100%" runat="server" />
                    <div class="spacer">
                    </div>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>
    </div>
    <br />
    <div id="accordionEspecificacionesTecnicas" style="padding: 0,0,0,0; margin: 0,0,0,0">
        <h2>Especificaciones Técnicas del Trabajo</h2>
        <asp:Panel ID="pnlEspecificacionesTecnicas" runat="server">
            <a href="#" id="lblNumVersionEspecificacion" style="text-decoration: none; float: right;" onclick="MostrarVersionesET(this)" runat="server">Sin Versiones</a>
            <br />
            <div class="spacer"></div>
            <h3 style="float: left; text-align: left;">
                <a>Capítulo I Especificaciones para campo
                </a>
            </h3>
            <br />
            <div class="spacer"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Moderador</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtModerador" runat="server" Width="50%" TextMode="MultiLine" Height="26px" CssClass="no-height" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div class="spacer"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Especificaciones para Campo</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="lblEspecificacionesCampo" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Material de Entrevista y Apoyo</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtMaterialApoyo" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Incidencias</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtIncidencias" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Auditoría</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtAuditoriaCampo" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo II Variables de Control
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Seguridad y confidencialidad de la información</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtVCSeguridad" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Forma de obtención de los entrevistados</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtVCObtencion" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Grupo objetivo</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="lblVCGrupoObjetivo" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Aplicación de Instrumentos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtVCAplicacionInstrumentos" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Distribución de Cuotas</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="lblVCDistribucionCuotas" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Cumplimiento de Metodología y otras instrucciones</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtVCMetodologia" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
    </div>
    <br />
    <div id="accordionEspecificacionesAdicionales" style="padding: 0,0,0,0; margin: 0,0,0,0">
        <h2>Especificaciones Adicionales a Operaciones </h2>
        <asp:Panel ID="Panel5" runat="server">
            <a href="#" id="lblNumVersionEspecificacionAdicionales" style="text-decoration: none; float: right;" onclick="MostrarVersionesET(this)" runat="server">Sin Versiones</a>
            <br />
            <div class="spacer"></div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo I Incentivos a utilizar
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Incentivos Económicos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:RadioButtonList ID="rblIncentivos" runat="server" Width="30%" BackColor="Transparent" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Value="1">Si</asp:ListItem>
                        <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Presupuesto Incentivos Económicos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtPresupuestoIncentivo" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Distribución Incentivos Económicos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtDistribucionIncentivo" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Regalos Cliente</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:RadioButtonList ID="rblRegaloClientes" runat="server" Width="30%" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Value="1">Si</asp:ListItem>
                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Compra Ipsos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:RadioButtonList ID="rblCompraIpsos" runat="server" Width="30%" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Value="1">Si</asp:ListItem>
                        <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Presupuesto Compra Ipsos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtPresupuestoCompra" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Distribución Compra Ipsos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtDistribucionCompra" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo II Ayudas Adicionales
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Incentivos Económicos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:CheckBoxList ID="chbAyudas" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" Enabled="false">
                    </asp:CheckBoxList>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Método Aceptable de Reclutamiento</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:CheckBoxList ID="chbReclutamiento" runat="server" RepeatDirection="Horizontal" Enabled="false">
                    </asp:CheckBoxList>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Observaciones, Exclusiones y Restricciones Específicas</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtExclusionesyRestricciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Recursos Propiedad del Cliente</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtRecursosPropiedadesCliente" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Requerimientos De Habeas Data/ Confidencialidad/ Privacidad Y Seguridad De La Información</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtHabeasData" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo III Especificaciones generales
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Otras especificaciones</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="lblOtrasEspecificaciones" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                </div>
            </div>
        </asp:Panel>
    </div>
    <br />
    <div id="accordionEsquemaAnalisis" style="padding: 0,0,0,0; margin: 0,0,0,0">
        <h2>Información Esquema de Análisis</h2>
        <asp:Panel ID="pnlEsquemaAnalisis" runat="server">
            <br />
            <div class="spacer"></div>
            <h3 style="float: left; text-align: left;">
                <a>Esquema de Análisis</a>
            </h3>
            <br />
            <div class="spacer"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                    <p>Cruces requeridos para el informe.</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtA1" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Comparativos (años anteriores u otros informes) </p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtA2" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Orden de la presentación y su contenido (Capítulos) -  Vs. Cubrimiento de objetivos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtA3" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Cómo se quiere la presentación de los datos (decimales, enteros o con o sin símbolo de porcentaje)</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtA4" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Graficación sugerida (círculos, líneas, columnas, etc y por pregunta o por bloques o tipos de preguntas)</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtA5" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Diseño (Definición de logos, colores, preguntas, soporte de matrices, lineamientos especiales del cliente ejemplo plantilla, etc), Complementos a los datos, por ejemplo: información secundaria, información del cliente, Orden de los datos históricos, de izquierda a derecha o de derecha a izquierda, etc </p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtA6" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Formato de gráficas para presentar los análisis estadísticos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtA7" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>¿Ponderación, entregas adicionales, traducción?</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtA8" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
    </div>
    <br />

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Button Text="Exportar a Word" CssClass="align-rigth" ID="btnExportarEspecificacionesWord" runat="server" Visible="false" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnRegresar" Text="Regresar" runat="server" />
    <div id="BusquedaVersionesEC">
        <asp:UpdatePanel ID="UPanelVersionesEC" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="actions"></div>
                <div style="width: 100%;">
                    <asp:Panel ID="pnlVersionesEC" runat="server" Visible="true">
                        <div style="overflow-y: scroll; width: 100%; height: 600px; margin-left: auto; margin-right: auto">
                            <asp:GridView ID="gvVersionesEC" runat="server" Width="95%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="id,NoVersion" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="Codigo" Visible="false" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="NoVersion" HeaderText="Versión" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Ver" ImageUrl="~/Images/application_view_detail.png" Text="Ver" ToolTip="Ver" OnClientClick="return true;" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comparar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos2" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" OnClientClick="return true;"
                                                CommandName="Comparar" ImageUrl="~/Images/comparar_32.png" Text="Comparar con versión anterior" ToolTip="Comparar con versión anterior" Width="16px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlDetalleVerEC" runat="server" Visible="false" Width="100%">
                        <div style="height: 500px; overflow-y: scroll;">
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones Generales</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObservacionesVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Técnica</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPTecnicaVer" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Otra Técnica</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="otraTecnicaVer" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Incentivos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPIncentivosEspVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Base de Datos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPBDDEspVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Productos o Conceptos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPProductoEspVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Método de reclutamiento</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPReclutamientoVer" runat="server" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Reclutamiento</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPEspReclutamientoVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 98%; clear: both; margin-top: 5px;">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Producto</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="lblBCPEspProductoVer" Text="" runat="server" Width="100%" Font-Size="12px" Height="30px" />
                                </div>
                            </div>
                            <div style="width: 98%; clear: both; margin-top: 5px;">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Materiales a Evaluar</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="lblBCPMaterialEvalVer" Text="" runat="server" Width="100%" Font-Size="12px" Height="30px" />
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones sobre el Producto</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObsProductoVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVersEC" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                    <asp:Panel ID="pnlCompararEC" runat="server" Width="100%" Visible="false">
                        <asp:Label Text="" runat="server" ID="lblErrorVersionEC" ForeColor="Red" />
                        <div style="height: 500px; overflow-y: scroll;">
                            <div style="width: 95%; clear: both">
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label runat="server" ID="lblVersionA" ForeColor="Red"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label runat="server" ID="lblVersionB" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones Generales</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObservacionesCam1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObservacionesCam2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Técnica</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPTecnicaV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPTecnicaV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Otra Técnica</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="otraTecnicaV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="otraTecnicaV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Incentivos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPIncentivosEspCam1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPIncentivosEspCam2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Base de Datos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPBDDEspCam1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPBDDEspCam2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Productos o Conceptos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPProductoEspCam1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPProductoEspCam2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Método de reclutamiento</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPReclutamientoV1" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbBCPReclutamientoV2" runat="server" Width="100%" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Reclutamiento</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPEspReclutamientoCam1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPEspReclutamientoCam2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones de Producto</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="lblBCPEspProductoCam1" runat="server" Width="100%" TextMode="MultiLine" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="lblBCPEspProductoCam2" runat="server" Width="100%" TextMode="MultiLine" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Materiales a Evaluar</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="lblBCPMaterialEvalCam1" runat="server" Width="100%" TextMode="MultiLine" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="lblBCPMaterialEvalCam2" runat="server" Width="100%" TextMode="MultiLine" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones sobre el Producto</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObsProductoCam1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtBCPObsProductoCam2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVersEC2" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="BusquedaVersionesET">
        <asp:UpdatePanel ID="UPanelVersionesET" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="actions"></div>
                <div style="width: 100%;">
                    <asp:Panel ID="pnlVersionesET" runat="server" Visible="true">
                        <div style="overflow-y: scroll; width: 100%; height: 600px; margin-left: auto; margin-right: auto">
                            <asp:GridView ID="gvVersionesET" runat="server" Width="95%" AutoGenerateColumns="False"
                                CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                                DataKeyNames="id,NoVersion" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                                <Columns>
                                    <asp:BoundField DataField="id" HeaderText="Codigo" Visible="false" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="NoVersion" HeaderText="Versión" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Fecha" HeaderText="Fecha" ItemStyle-CssClass="text-center" />
                                    <asp:BoundField DataField="Usuario" HeaderText="Usuario" ItemStyle-CssClass="text-center" />
                                    <asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                                CommandName="Ver" ImageUrl="~/Images/application_view_detail.png" Text="Ver" ToolTip="Ver" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Comparar" ShowHeader="False" ItemStyle-CssClass="text-center">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgArchivos2" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" OnClientClick="return true;"
                                                CommandName="Comparar" ImageUrl="~/Images/comparar_32.png" Text="Comparar con versión anterior" ToolTip="Comparar con versión anterior" Width="16px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnlDetalleVerET" runat="server" Visible="false" Width="100%">
                        <div style="height: 500px; overflow-y: scroll;">
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Moderador</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtModeradorETVer" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones para Campo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="lblEspecificacionesCampoETVer" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Material de Entrevista y Apoyo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMaterialApoyoETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Incidencias</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtIncidenciasETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Auditoría</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtAuditoriaCampoETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Seguridad y confidencialidad de la información</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCSeguridadETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Forma de obtención de los entrevistados</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCObtencionETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Grupo objetivo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="lblVCGrupoObjetivoETVer" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Aplicación de Instrumentos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCAplicacionInstrumentosETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Distribución de Cuotas</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="lblVCDistribucionCuotasETVer" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Cumplimiento de Metodología y otras instrucciones</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCMetodologiaETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Incentivos Económicos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="rblIncentivosETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Presupuesto Incentivos Económicos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtPresupuestoIncentivoETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Distribución Incentivos Económicos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDistribucionIncentivoETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Regalos Cliente</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="rblRegaloClientesETVer" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Compra Ipsos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="rblCompraIpsosETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Presupuesto Compra Ipsos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtPresupuestoCompraETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Distribución Compra Ipsos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDistribucionCompraETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <!--<div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Incentivos Económicos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbAyudasETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Método Aceptable de Reclutamiento</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="chbReclutamientoETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>-->
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones, Exclusiones y Restricciones Específicas</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtExclusionesyRestriccionesETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Recursos Propiedad del Cliente</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtRecursosPropiedadesClienteETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Requerimientos De Habeas Data/ Confidencialidad/ Privacidad Y Seguridad De La Información</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtHabeasDataETVer" runat="server" Width="100%" Font-Size="12px" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Otras especificaciones</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="lblOtrasEspecificacionesETVer" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVersET" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                    <asp:Panel ID="pnlCompararET" runat="server" Width="100%" Visible="false">
                        <asp:Label Text="" runat="server" ID="lblErrorVersionET" ForeColor="Red" />
                        <div style="height: 500px; overflow-y: scroll;">
                            <div style="width: 95%; clear: both">
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label runat="server" ID="lblVersionC" ForeColor="Red"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label runat="server" ID="lblVersionD" ForeColor="Red"></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Moderador</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtModerador1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtModerador2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both;">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones para Campo</p>
                                </div>
                                <asp:Panel runat="server" ID="Panel1" Width="100%">
                                    <asp:Label ID="lblEspecificacionesCampo1" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                                <br />
                                <asp:Panel runat="server" ID="Panel2" Width="100%">
                                    <asp:Label ID="lblEspecificacionesCampo2" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Material de Entrevista y Apoyo</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMaterialApoyo1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMaterialApoyo2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Incidencias</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtIncidencias1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtIncidencias2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Auditoría</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtAuditoriaCampo1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtAuditoriaCampo2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Seguridad y confidencialidad de la información</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCSeguridad1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCSeguridad2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Forma de obtención de los entrevistados</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCObtencion1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCObtencion2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both;">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Grupo objetivo</p>
                                </div>
                                <asp:Panel runat="server" ID="Panel3" Width="100%">
                                    <asp:Label ID="lblVCGrupoObjetivo1" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                                <br />
                                <asp:Panel runat="server" ID="Panel4" Width="100%">
                                    <asp:Label ID="lblVCGrupoObjetivo2" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Aplicación de Instrumentos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCAplicacionInstrumentos1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCAplicacionInstrumentos2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both;">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Distribución de Cuotas</p>
                                </div>
                                <asp:Panel runat="server" ID="Panel7" Width="100%">
                                    <asp:Label ID="lblVCDistribucionCuotas1" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                                <br />
                                <asp:Panel runat="server" ID="Panel8" Width="100%">
                                    <asp:Label ID="lblVCDistribucionCuotas2" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Cumplimiento de Metodología y otras instrucciones</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCMetodologia1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVCMetodologia2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Incentivos Económicos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="rblIncentivos1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="rblIncentivos2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Presupuesto Incentivos Económicos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtPresupuestoIncentivo1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtPresupuestoIncentivo2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Distribución Incentivos Económicos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDistribucionIncentivo1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDistribucionIncentivo2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Regalos Cliente</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="rblRegaloClientes1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="rblRegaloClientes2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Compra Ipsos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="rblCompraIpsos1" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="rblCompraIpsos2" runat="server" Width="100%" Font-Size="12px" Height="30px" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Presupuesto Compra Ipsos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtPresupuestoCompra1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtPresupuestoCompra2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Distribución Compra Ipsos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDistribucionCompra1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDistribucionCompra2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Observaciones, Exclusiones y Restricciones Específicas</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtExclusionesyRestricciones1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtExclusionesyRestricciones2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Recursos Propiedad del Cliente</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtRecursosPropiedadesCliente1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtRecursosPropiedadesCliente2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Requerimientos De Habeas Data/ Confidencialidad/ Privacidad Y Seguridad De La Información</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtHabeasData1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtHabeasData2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both;">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Otras especificaciones</p>
                                </div>
                                <asp:Panel runat="server" ID="pnlOtrasEspecificaciones1" Width="100%">
                                    <asp:Label ID="lblOtrasEspecificaciones1" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                                <br />
                                <asp:Panel runat="server" ID="pnlOtrasEspecificaciones2" Width="100%">
                                    <asp:Label ID="lblOtrasEspecificaciones2" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVersET2" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
