<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterGeneral.Master" CodeBehind="InformacionGeneral.aspx.vb" Inherits="WebMatrix.InformacionGeneral" %>

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

            $('#BusquedaVersionesE').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Versiones de Especificaciones Técnicas del Trabajo",
                    width: "600px"
                });

            $('#BusquedaVersionesE').parent().appendTo("form");

            $('#BusquedaVersionesM').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Versiones de Metodología de Campo",
                    width: "600px"
                });

            $('#BusquedaVersionesM').parent().appendTo("form");

            $('#AprobarMetodologia').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Aprobar Metodología",
                    width: "600px"
                });

            $('#AprobarMetodologia').parent().appendTo("form");
        });

        function MostrarVersionesE(e) {
            var a = $(e).attr('id');
            var text = $("#" + a).text();
            if (text == "Sin Versiones") {
                alert("No hay versiones disponibles");
            } else {
                $('#BusquedaVersionesE').dialog("open");
            }
        }

        function MostrarVersionesM(e) {
            var a = $(e).attr('id');
            var text = $("#" + a).text();
            if (text == "Sin Versiones") {
                alert("No hay versiones disponibles");
            } else {
                $('#BusquedaVersionesM').dialog("open");
            }
        }

        function abrirAprobarMetodologia() {
            $('#AprobarMetodologia').dialog("open");
        }
        function cerrarAprobarMetodologia() {
            $('#AprobarMetodologia').dialog("close");
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

        .btn {
            float: left;
            padding: 0 20px;
            height: 30px;
            font-size: 12px !important;
            color: #fff;
            outline: none;
            background-color: #048e87;
            border: none;
            transition: all 0.2s ease;
            border-radius: 5px;
            margin: 0px 5px;
        }

        .btn-danger {
            color: #fff;
            background-color: #dc3545;
            border-color: #dc3545;
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
    <asp:HiddenField ID="hfVolver" runat="server" />
    <asp:HiddenField ID="hfTrabajoId" runat="server" />
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
            <asp:Button ID="btnDescargarPropuesta" runat="server" Text="Descargar propuesta" />
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
                <asp:BoundField DataField="ParIncidencia" HeaderText="Incidencia" />
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
            <p>Información de procesos</p>
        <asp:Literal ID="literalProcesos" runat="server"></asp:Literal>
            <asp:GridView ID="gvNoProcesos" runat="server" Width="100%" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="DescFase" HeaderText="Fase" />
                <asp:BoundField DataField="Metodologia" HeaderText="Metod" />
                <asp:BoundField DataField="ParNProcesosDC" HeaderText="# DataClean" />
                <asp:BoundField DataField="ParNProcesosTopLines" HeaderText="# TopLines" />
                <asp:BoundField DataField="ParNProcesosTablas" HeaderText="# Tablas" />
                <asp:BoundField DataField="ParNProcesosBases" HeaderText="# Bases" />
                <asp:CheckBoxField DataField="DPUnificacion" HeaderText="Unificación" />
                <asp:CheckBoxField DataField="DPTransformacion" HeaderText="Transformación" />
            </Columns>
        </asp:GridView>
            <p>Análisis Estadística Cotizados</p>
            <asp:GridView ID="gvProcesosEstadistica" runat="server" Width="100%" AutoGenerateColumns="false" EmptyDataText="No hay análisis cotizados para esta propuesta">
            <Columns>
                <asp:BoundField DataField="DescFase" HeaderText="Fase" />
                <asp:BoundField DataField="Metodologia" HeaderText="Metodología" />
                <asp:BoundField DataField="Categoria" HeaderText="Categoria" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="AnalisisServicio" HeaderText="Analisis/Servicio" ItemStyle-Wrap="true" />
                <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
            </Columns>
        </asp:GridView>
        <asp:Panel ID="pnlCotizacionOPS" runat="server" Visible="false">
            <p>Observaciones del presupuesto</p>
            <asp:Label ID="lblObservacionesPresupuesto" runat="server"></asp:Label>
            <p>Detalle de Costos de operaciones</p>
            <asp:GridView ID="gvDetallesOperaciones" runat="server"
                                                            AutoGenerateColumns="False" CssClass="displayTable" ShowFooter="True"
                                                            Width="100%">
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderText="ID" />
                                                                <asp:BoundField DataField="ActNombre" HeaderText="ACTIVIDAD" />
                                                                <asp:BoundField DataField="PRESUPUESTADO" DataFormatString="{0:C0}"
                                                                    HeaderText="PRESUPUESTADO" />
                                                                <asp:BoundField DataField="AUTORIZADO" DataFormatString="{0:C0}"
                                                                    HeaderText="AUTORIZADO" Visible="False" />
                                                                <asp:BoundField DataField="PRESUVSAUTORIZADO" DataFormatString="{0:C0}"
                                                                    HeaderText="PRESUP VS AUTO" Visible="False" />
                                                                <asp:BoundField DataField="PORCENTAJE1" DataFormatString="{0:N}" HeaderText="%"
                                                                    Visible="False" />
                                                                <asp:BoundField DataField="PRODUCCION" DataFormatString="{0:C0}"
                                                                    HeaderText="PRODUCCION" Visible="False" />
                                                                <asp:BoundField DataField="PRESUVSPROD" DataFormatString="{0:C0}"
                                                                    HeaderText="PRESUP VS PROD" Visible="False" />
                                                                <asp:BoundField DataField="PORCENTAJE3" DataFormatString="{0:C0}"
                                                                    HeaderText="%" Visible="False" />
                                                                <asp:BoundField DataField="PRESUVSEJECUTADO" DataFormatString="{0:C0}"
                                                                    HeaderText="PRESUP VS EJEC" Visible="False" />
                                                                <asp:BoundField DataField="PORCENTAJE2" DataFormatString="{0:N}" HeaderText="%"
                                                                    Visible="False" />
                                                                <asp:BoundField DataField="UNIDADES" DataFormatString="{0:N0}"
                                                                    HeaderText="UNIDADES" />
                                                                <asp:BoundField DataField="DESC_UNIDADES" HeaderText="DESCRIPCION" />
                                                                <asp:BoundField DataField="HORAS" HeaderText="HORAS" />
                                                            </Columns>
                                                            <FooterStyle Font-Bold="True" />
                                                        </asp:GridView>
        </asp:Panel>
        </asp:Panel>
    </div>
    <br />
    <div id="accordionEspecificaciones" style="padding: 0,0,0,0; margin: 0,0,0,0">
        <h2>Especificaciones de Cuentas a Proyectos</h2>
        <asp:Panel ID="pnlBriefCuentasProyectos" runat="server">
            <asp:Panel ID="pnlCuanti" runat="server">
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p runat="server" id="lblVersionEspCuanti" style="font-weight: bold; font-size: large;"></p>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Observaciones Generales</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPObservaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Tipo de medición</p>
                    </div>
                    <div style="width: 70%; float: left; padding: 2px 2px 2px 2px;">
                        <asp:RadioButtonList runat="server" ID="chbBCPMedicion" Width="100%" RepeatDirection="Horizontal" Enabled="false">
                            <asp:ListItem Value="1" Text="Medición Puntual"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Medición Multifases"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Tracking Puntuales"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Tracking Continuo"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div style="width: 22%; float: left; padding: 2px 2px 2px 2px; margin-top: -10px;">
                        <label>No. Olas</label>
                        <asp:TextBox ID="txtBCPOlas" runat="server" Width="100%" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both; margin-top: 5px;">
                    <asp:CheckBox ID="chbBCPPilotos" CssClass="leftAuto" TextAlign="Left" runat="server" Text="Pilotos" Enabled="false" />
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Pilotos</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPPilotosEspecificaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Incentivos</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPIncentivosEspecificaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Base de Datos</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPBDDEspecificaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Productos o Conceptos</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPProductoEspecificaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px" ReadOnly="true"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 100%; clear: both">
                    <div style="width: 33%; float: left;">
                        <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                            <label style="text-align: left; width: auto;">Fecha BDD</label>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtBCPFechaBDD" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <div style="width: 33%; float: left;">
                        <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                            <label style="text-align: left; width: auto;">Fecha Productos o Conceptos</label>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtBCPFechaConceptos" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <div style="width: 33%; float: left;">
                        <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                            <label style="text-align: left; width: auto;">Fecha Cuestionario</label>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtBCPFechaCuestionario" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <div style="width: 33%; float: left;">
                        <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                            <label style="text-align: left; width: auto;">Fecha Inicio de Campo</label>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtBCPFechaInicioCampo" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <div style="width: 33%; float: left;">
                        <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                            <label style="text-align: left; width: auto;">Fecha Informe a Cuentas</label>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtBCPFechaInformeCuentas" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>

                    <div style="width: 33%; float: left;">
                        <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                            <label style="text-align: left; width: auto;">Fecha Informe a Cliente</label>
                        </div>
                        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                            <asp:TextBox runat="server" ID="txtBCPFechaInformeCliente" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlCuali" runat="server">
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p runat="server" id="lblVersionEspCuali" style="font-weight: bold; font-size: large;"></p>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Observaciones Generales</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtObservacionesCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Técnica</p>
                    </div>
                    <div style="width: 80%; float: left; padding: 2px 2px 2px 2px">
                        <asp:RadioButtonList runat="server" ID="chbBCPTecnicaCuali" Width="100%" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Text="Entrevista"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Sesiones de grupo/Talleres"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Inmersiones"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Estudios online"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Otro"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div style="width: 18%; float: left; padding: 2px 2px 2px 2px; margin-top: -5px;">
                        <label>Otro</label>
                        <asp:TextBox ID="otraTecnica" runat="server" Width="100%"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Incentivos</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPIncentivosEspCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Base de Datos</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPBDDEspCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Especificaciones de Productos o Conceptos</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPProductoEspCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Método de reclutamiento</p>
                    </div>
                    <div style="width: 80%; float: left; padding: 2px 2px 2px 2px">
                        <asp:RadioButtonList runat="server" ID="chbBCPReclutamientoCuali" Width="100%" RepeatDirection="Horizontal">
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
                        <asp:TextBox ID="txtBCPEspReclutamientoCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                    </div>
                </div>
                <div style="width: 98%; clear: both; margin-top: 5px;">
                    <asp:CheckBox ID="chbBCPEspProductoCuali" CssClass="leftAuto" TextAlign="Left" runat="server" Text="Especificaciones de Producto" />
                </div>
                <div style="width: 98%; clear: both; margin-top: 5px;">
                    <asp:CheckBox ID="chbBCPMaterialEvalCuali" CssClass="leftAuto" TextAlign="Left" runat="server" Text="Materiales a Evaluar" />
                </div>
                <div style="width: 98%; clear: both">
                    <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                        <p>Observaciones sobre el Producto</p>
                    </div>
                    <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                        <asp:TextBox ID="txtBCPObsProductoCuali" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                    </div>
                </div>
            </asp:Panel>
        </asp:Panel>
    </div>
    <br />
    <asp:HiddenField runat="server" ID="hfMetodologiaId" Value="0" />
    <div id="accordionMetodologia" style="padding: 0,0,0,0; margin: 0,0,0,0">
        <h2>Metodología de Campo</h2>
        <asp:Panel ID="pnlMetodologia" runat="server">
            <asp:Panel ID="pnlMetodologia1" runat="server">

                <a href="#" id="lblNumVersionMetodologia" style="text-decoration: none; float: right" onclick="MostrarVersionesM(this)" runat="server">Sin Versiones</a>

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
                <br />
                <asp:Panel runat="server" ID="pnlModalAprobar">
                    <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <asp:Button Text="Aprobar Metodología" runat="server" ID="btnAprobarMetodologiaModal" OnClientClick="abrirAprobarMetodologia()" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </asp:Panel>
        </asp:Panel>

    </div>
    <br />
    <div id="accordionEspecificacionesTecnicas" style="padding: 0,0,0,0; margin: 0,0,0,0">
        <h2>Especificaciones Técnicas del Trabajo</h2>

        <asp:Panel ID="pnlEspecificacionesTecnicas" runat="server">

            <a href="#" id="lblNumVersionEspecificacion" style="text-decoration: none; float: right" onclick="MostrarVersionesE(this)" runat="server">Sin Versiones</a>

            <h3 style="float: left; text-align: left;">
                <a>Capítulo I Especificaciones para campo
                </a>
            </h3>
            <br />
            <div class="spacer"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Especificaciones para Campo</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtEspecificacionesCampo" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Material de Entrevista y Apoyo</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtMaterialApoyo" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Incidencias</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtIncidencias" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Pilotos de Campo</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtPilotos" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo II Especificaciones para control calidad (Auditoría de Campo, critica, verificación)
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Auditoría de Campo</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtAuditoriaCampo" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Verificación (se verifica el 10%; si el cliente externo solicita un porcentaje diferente, especifíquelo)</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtVerificacion" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Crítica</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtCritica" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Pilotos de Campo (Calidad)</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtPilotosCalidad" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo III Especificaciones para codificación
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Codificación (se verifica la codificación al 5%; si el cliente externo solicita un porcentaje diferente, especifíquelo)</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtCodificacion" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>

            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo IV Especificaciones de procesamiento
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Procesamiento</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtProcesamiento" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo V Variables de Control
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Seguridad y confidencialidad de la información</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtVCSeguridad" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Forma de obtención de los entrevistados</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtVCObtencion" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Grupo objetivo</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtVCGrupoObjetivo" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Aplicación de Instrumentos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtVCAplicacionInstrumentos" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Distribución de Cuotas</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px; min-height=30px;">
                    <asp:Label ID="txtVCDistribucionCuotas" runat="server" Width="100%" TextMode="MultiLine"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Cumplimiento de Metodología y otras instrucciones</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtVCMetodologia" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo VI Especificaciones Estadística
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Estadística</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtEstadistica" runat="server" />
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo VII Especificaciones generales
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Otras especificaciones</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtOtrasEspecificaciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
                </div>
            </div>
        </asp:Panel>

    </div>
    <br />
    <div id="accordionEsquema">
        <h2>Información del Esquema de Análisis</h2>
        <asp:Panel runat="server" ID="pnlEsquema">
            <div style="width: 100%; clear: both">
                <div style="width: 100%; float: left; clear: both; padding: 2px 2px 2px 2px">
                    <p>Cruces requeridos para el informe.</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtA1" runat="server" Width="100%"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Comparativos (años anteriores u otros informes) </p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtA2" runat="server" Width="100%"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Orden de la presentación y su contenido (Capítulos) -  Vs. Cubrimiento de objetivos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtA3" runat="server" Width="100%"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Cómo se quiere la presentación de los datos (decimales, esteros o con o sin símbolo de porcentaje</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtA4" runat="server" Width="100%"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Graficación sugerida (círculos, líneas, columnas, etc y por pregunta o por bloques o tipos de preguntas)</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtA5" runat="server" Width="100%"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Diseño (Definición de logos, colores, preguntas, soporte de matrices, lineamientos especiales del cliente ejemplo plantilla, etc), Complementos a los datos, por ejemplo: información secundaria, información del cliente, Orden de los datos históricos, de izquierda a derecha o de derecha a izquierda, etc </p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtA6" runat="server" Width="100%"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Formato de gráficas para presentar los análisis estadísticos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtA7" runat="server" Width="100%"></asp:Label>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>¿Ponderación, entregas adicionales, traducción</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:Label ID="txtA8" runat="server" Width="100%"></asp:Label>
                </div>
            </div>
        </asp:Panel>
    </div>
    <br />
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <asp:Button Text="Exportar a Word" CssClass="align-rigth" ID="btnExportarEspecificacionesWord" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Button ID="btnRegresar" Text="Regresar" runat="server" />
    <div id="BusquedaVersionesM">
        <asp:UpdatePanel ID="UPanelVersionesM" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="actions"></div>
                <div style="width: 100%;">
                    <asp:Panel ID="pnlVersionesM" runat="server" Visible="true">
                        <div style="overflow-y: scroll; width: 100%; height: 600px; margin-left: auto; margin-right: auto">
                            <asp:GridView ID="gvVersionesM" runat="server" Width="95%" AutoGenerateColumns="False"
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
                    <asp:Panel ID="pnlDetalleVerM" runat="server" Visible="false" Width="100%">
                        <div style="height: 500px; overflow-y: scroll;">
                            <h3 style="float: left; text-align: left;">
                                <a>Metodología de Campo</a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Grupo Objetivo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtGrupoObjetivoMVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Mercado, Cubrimiento geográfico</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMercadoCubrimientoVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Marco muestral</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMarcoMuestralVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Técnica</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtTecnicaVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Diseño Muestral</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDisenoMuestralVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Instrucciones para la recolección</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtInstruccionesRecoleccionVer" runat="server" Width="100%" TextMode="MultiLine" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Distribución de la muestra</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtDistribucionMuestraMVer" runat="server" Width="100%" TextMode="MultiLine" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Nivel de confianza</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtNivelConfianzaVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Margen de Error Esperado</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMargenErrorEsperadoVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Desagregación básica de los resultados</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDesagregacionVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fuente para la elaboración de la distribución muestral</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtFuenteDistribucionVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Variables básicas de ponderación</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVariablePonderacionVer" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Tasa de Respuesta</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtTasaRespuesta" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Procedimiento de Imputación</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtProcedimientoImputacion" runat="server" Width="100%" TextMode="MultiLine" Height="100px" Font-Size="Small"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVersM" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                    <asp:Panel ID="pnlCompararM" runat="server" Width="100%" Visible="false">
                        <asp:Label Text="" runat="server" ID="lblErrorVersionM" ForeColor="Red" />
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
                            <h3 style="float: left; text-align: left;">
                                <a>Metodología de Campo</a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Grupo Objetivo</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtGruObjM1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtGruObjM2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Mercado, Cubrimiento geográfico</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMerCub1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMerCub2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Marco muestral</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMarMues1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMarMues2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Técnica</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtTec1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtTec2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Diseño Muestral</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDisMues1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDisMues2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both;">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Instrucciones para la recolección</p>
                                </div>
                                <asp:Panel runat="server" ID="divInstRec1" Width="100%">
                                    <asp:Label ID="txtInstRec1" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                                <br />
                                <asp:Panel runat="server" ID="divInstRec2" Width="100%">
                                    <asp:Label ID="txtInstRec2" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Distribución de la muestra</p>
                                </div>
                                <asp:Panel runat="server" ID="divDistriMues1" Width="100%">
                                    <asp:Label ID="txtDistriMues1" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                                <br />
                                <asp:Panel runat="server" ID="divDistriMues2" Width="100%">
                                    <asp:Label ID="txtDistriMues2" runat="server" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </asp:Panel>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Nivel de confianza</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtNiv1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtNiv2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Margen de Error Esperado</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMarErr1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtMarErr2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Desagregación básica de los resultados</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDesBas1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtDesBas2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Fuente para la elaboración de la distribución muestral</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtFue1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtFue2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Variables básicas de ponderación</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVar1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtVar2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Tasa de Respuesta</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtTas1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtTas2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Procedimiento de Imputación</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtProImp1" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:TextBox ID="txtProImp2" runat="server" Width="100%" TextMode="MultiLine" Height="150px" Font-Size="Small" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVersM2" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="AprobarMetodologia">
        <asp:UpdatePanel runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="actions"></div>
                <div style="height: 150px;">
                    <br />
                    <asp:TextBox runat="server" ID="txtObsAprobarMetodologia" TextMode="MultiLine" Height="100px" Width="100%" />
                    <br />
                    <br />
                    <div style="float: right;">
                        <asp:Button Text="Aprobar" ID="btnAprobarMetodologia" runat="server" CssClass="btn" />
                        <asp:Button Text="Rechazar" ID="btnRechazarMetodologia" runat="server" CssClass="btn btn-danger" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div id="BusquedaVersionesE">
        <asp:UpdatePanel ID="UPanelVersionesE" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
            <ContentTemplate>
                <div class="actions"></div>
                <div style="width: 100%;">
                    <asp:Panel ID="pnlVersionesE" runat="server" Visible="true">
                        <div style="overflow-y: scroll; width: 100%; height: 600px; margin-left: auto; margin-right: auto">
                            <asp:GridView ID="gvVersionesE" runat="server" Width="95%" AutoGenerateColumns="False"
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
                    <asp:Panel ID="pnlDetalleVerE" runat="server" Visible="false" Width="100%">
                        <div style="height: 500px; overflow-y: scroll;">
                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo I Especificaciones para campo</a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones para Campo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtEspecificacionesCampoVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Material de Entrevista y Apoyo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtMaterialApoyoVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Incidencias</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtIncidenciasVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Pilotos de Campo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtPilotoCampoVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>

                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo II Especificaciones para control calidad</a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Auditoría de Campo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtAuditoriaVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Verificación</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtVerificacionVer" Width="100%" runat="server" Font-Size="Small" />
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Crítica</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtCriticaVer" Width="100%" runat="server" Font-Size="Small" />
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Pilotos de Campo (Calidad)</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtPilotoCampoCalidadVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>

                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo III Especificaciones para codificación
                                </a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Codificación</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtCodificacionVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>

                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo IV Especificaciones de procesamiento
                                </a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Procesamiento</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtProcesamientoVer" Width="100%" runat="server" Font-Size="Small" />
                                </div>
                            </div>

                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo V Variables de Control</a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Seguridad y confidencialidad de la información</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtSeguridadVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Forma de obtención de los entrevistados</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtObtencionEntrevistadosVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Grupo objetivo</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtGrupoObjetivoVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Aplicación de Instrumentos</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtAplicacionInstrumentosVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Distribución de Cuotas</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtDistribucionVer" Width="100%" runat="server" Font-Size="Small" />
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Cumplimiento de Metodología y otras instrucciones</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtCumplimientoMetodologiaVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo VI Especificaciones Estadística
                                </a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Estadística</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtEstadisticaVer" Width="100%" runat="server" Font-Size="Small" />
                                </div>
                            </div>
                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo VII Especificaciones generales
                                </a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Otras especificaciones</p>
                                </div>
                                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtOtrasEspecificacionesVer" runat="server" Width="100%" Font-Size="Small"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVersE" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                    <asp:Panel ID="pnlCompararE" runat="server" Width="100%" Visible="false">
                        <asp:Label Text="" runat="server" ID="lblErrorVersionE" ForeColor="Red" />
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
                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo I Especificaciones para campo</a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Especificaciones para Campo</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtEspCam1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtEspCam2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Material de Entrevista y Apoyo</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtMatApo1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtMatApo2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Incidencias</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtInc1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtInc2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Pilotos de Campo</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtpilCam1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtpilCam2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>

                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo II Especificaciones para control calidad</a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Auditoría de Campo</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtAudCam1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtAudCam2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Verificación</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtVer1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <br />
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtVer2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Crítica</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtcri1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <br />
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtcri2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Pilotos de Campo (Calidad)</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtpilCal1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtpilCal2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>

                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo III Especificaciones para codificación
                                </a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Codificación</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtCod1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtCod2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>

                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo IV Especificaciones de procesamiento
                                </a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Procesamiento</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtPro1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <br />
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtPro2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>

                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo V Variables de Control</a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Seguridad y confidencialidad de la información</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtSegcon1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtSegcon2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Forma de obtención de los entrevistados</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtObt1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtObt2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Grupo objetivo</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtgruObj1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtgruObj2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Aplicación de Instrumentos</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtInst1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtInst2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Distribución de Cuotas</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtDis1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <br />
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtDis2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <br />
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Cumplimiento de Metodología y otras instrucciones</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtCumMet1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtCumMet2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo VI Especificaciones Estadística
                                </a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Estadística</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtEst1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <br />
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtEst2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                            <br />
                            <h3 style="float: left; text-align: left;">
                                <a>Capítulo VII Especificaciones generales
                                </a>
                            </h3>
                            <div class="spacer"></div>
                            <div style="width: 95%; clear: both">
                                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                                    <p>Otras especificaciones</p>
                                </div>
                                <div style="width: 48%; float: left; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtOtrEsp1" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                                <div style="width: 48%; float: right; padding: 2px 2px 2px 2px">
                                    <asp:Label ID="txtOtrEsp2" runat="server" Width="95%" Font-Size="Small" ReadOnly="true"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <br />
                        <a href="#" style="font-size: 12px;" id="volverListadoVersE2" runat="server">Volver al Listado de Versiones</a>
                    </asp:Panel>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
