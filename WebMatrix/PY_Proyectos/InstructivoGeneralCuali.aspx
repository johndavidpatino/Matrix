<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterProyectos.master" CodeBehind="InstructivoGeneralCuali.aspx.vb" Inherits="WebMatrix.InstructivoGeneralCuali" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link rel="stylesheet" href="../css/non-estilos.css" />
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Especificaciones Técnicas del Trabajo
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    Este apartado permite definir las especificaciones para las áreas involucradas en la ejecución de cada trabajo.
    Asegúrese de completar toda la información de forma detallada para garantizar la calidad en los entregables
    <br />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <script type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
        function endReq(sender, args) {
            $('#tabs').tabs();
        }

        function formatearHtmlenTexto() {
            var form = "CPH_Content_CPH_ContentForm_";
            quitarllaves(form + 'txtEspecificacionesCampo');
            quitarllaves(form + 'txtMaterialApoyo');
            quitarllaves(form + 'txtIncidencias');
            quitarllaves(form + 'txtAuditoriaCampo');
            quitarllaves(form + 'txtVCSeguridad');
            quitarllaves(form + 'txtVCObtencion');
            quitarllaves(form + 'txtVCGrupoObjetivo');
            quitarllaves(form + 'txtVCAplicacionInstrumentos');
            quitarllaves(form + 'txtVCMetodologia');
            quitarllaves(form + 'txtPresupuestoIncentivo');
            quitarllaves(form + 'txtDistribucionIncentivo');
            quitarllaves(form + 'txtPresupuestoCompra');
            quitarllaves(form + 'txtDistribucionCompra');
            quitarllaves(form + 'txtExclusionesyRestricciones');
            quitarllaves(form + 'txtRecursosPropiedadesCliente');
            quitarllaves(form + 'txtHabeasData');
            quitarllaves(form + 'txtOtrasEspecificaciones');
            return false;
        }

        function quitarllaves(obj) {
            var objeto = $('#' + obj).val();
            objeto = objeto.replace(/</g, '');
            objeto = objeto.replace(/>/g, '');
            $('#' + obj).val(objeto);
        }
    </script>
    <style>
        .no-height {
            max-height: 26px;
            min-height: 26px;
            max-width: 50%;
            min-width: 50%;
        }

        .divNuevaLinea {
            width: 100%;
            float: left;
        }

        .div3Form {
            width: 33%;
            float: left;
        }

        .div2Form {
            width: 48%;
            float: left;
        }

        #stylized label {
            text-align: left;
            margin: 0px;
            margin-left: 5px;
            width: 100px;
        }

        #stylized input[type="radio"] {
            width: auto;
            float: left;
            padding: 0px;
            margin: 5px 0 0 10px;
        }

        .text70 {
            width: 70% !important;
        }

        .textFull {
            width: 100% !important;
        }

        .wAuto {
            width: auto;
        }

        .textTabla {
            margin: 0px 5px !important;
        }
    </style>
    <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
                <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
                <div style="float: left; margin-left: 10px; margin-top: 5px;">
                    <span class="ui-icon ui-icon-info" style="float: left; margin-top: 0px;"></span>
                    <strong style="float: left;">Info: </strong>
                    <br />
                    <label style="float: left; display: block; width: auto;" id="lblTextInfo">
                    </label>
                </div>
            </div>
            <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
                <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
                <div style="float: left; margin-left: 10px; margin-top: 5px;">
                    <span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
                    <strong style="float: left;">Error: </strong>
                    <br />
                    <label style="float: left; display: block; width: auto;" id="lbltextError">
                    </label>
                </div>
            </div>
            <asp:HiddenField ID="hfTrabajoID" runat="server" />
            <asp:HiddenField ID="hfversion" runat="server" />
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p id="pVersion" runat="server" style="font-size: large; font-weight: bold;"></p>
                    <br />
                    <br />
                </div>
            </div>
            <br />
            <br />
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
                    <asp:TextBox ID="txtModerador" runat="server" Width="50%" TextMode="MultiLine" Height="26px" CssClass="no-height"></asp:TextBox>
                </div>
            </div>
            <div class="spacer"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Especificaciones para Campo</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <cc1:Editor ID="txtEspecificacionesCampo" Width="100%" Height="50px" runat="server" NoUnicode="true" />
                    <asp:Label ID="lblEspecificacionesCampo" Width="100%" runat="server" Visible="false" />
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Material de Entrevista y Apoyo</p>
                    <%--<a style="font-style: italic;">Especificar los materiales adicionales que se utilizarán en el proyecto Ej: Tarjetas, naipes, videos, dummies, producto, entre otros</a>--%>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtMaterialApoyo" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Incidencias</p>
                    <%--<a style="font-style: italic;">Especifique si se requiere manejo de incidencias y su tratamiento</a>--%>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtIncidencias" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Auditoría</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtAuditoria" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo II Variables de Control
                </a>
            </h3>
            <div style="clear: both"></div>
            <%--<a style="font-style: italic;">Especifique a continuación las variables de control requeridas</a>--%>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Seguridad y confidencialidad de la información</p>
                    <%--<a style="font-style: italic;">Verificando la información contenida en: 
•	Procesos de seguridad y confidencialidad de la información en la intranet de calidad.
•	Introducción de cuestionario
                    </a>--%>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtVCSeguridad" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Forma de obtención de los entrevistados</p>
                    <%--<a style="font-style: italic;">Verificando la información contenida en la Metodología del estudio en: 
•	Técnica 
•	Diseño muestral 
•	Instrucciones para la recolección 
                    </a>--%>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtVCObtencion" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Grupo objetivo</p>
                    <%--<a style="font-style: italic;">Verificando el Cumplimiento de la técnica de recolección de acuerdo con las características del estudio:

•	Hogares, Interceptación, CLT con reclutamiento previo, Probabilístico, Multitécnica, Lista de contactos por barrido y Lista de contactos por cita previa
Metodología del estudio en: 
•	Grupo objetivo 
•	Mercado: Cubrimiento geográfico 
                    </a>--%>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">                  
                    <cc1:Editor ID="txtVCGrupoObjetivo" Width="100%" Height="50px" runat="server" NoUnicode="true" />
                    <asp:Label ID="lblVCGrupoObjetivo" Width="100%" runat="server" Visible="false" />
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Aplicación de Instrumentos</p>
                    <%--<a style="font-style: italic;">La correcta aplicación y uso de:
•	Cuestionarios
•	Consentimientos
•	Instructivo
•	Material de apoyo (Tarjetas, naipes, ayudas audiovisuales)
•	Prueba de producto
•	Etc.
                    </a>--%>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtVCAplicacionInstrumentos" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Distribución de Cuotas</p>
                    <%--<a style="font-style: italic;">Se valida controlando: 
•	Demográficos: NSE, edad, género, ciudad
•	Celdas: por producto, concepto, empaque
•	Cuotas por marca, frecuencias de consumo u otros comportamientos de los entrevistados relevantes para el estudio.
                    </a>--%>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <cc1:Editor ID="txtVCDistribucionCuotas" Width="100%" Height="50px" runat="server" NoUnicode="true" />
                    <asp:Label ID="lblVCDistribucionCuotas" Width="100%" runat="server" Visible="false" />
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Cumplimiento de Metodología y otras instrucciones</p>
                    <%--<a style="font-style: italic;">Se verifica y controla a través de los Instructivos del estudio donde están específicos los requerimientos:
•	Manejo de Protocolos, Toma de fotos durante la entrevista con instrucciones claras, Condiciones de geo cercas, Reporte de incidencias, especificando formato del reporte de estas, Manejo de consentimientos y Reporte de eventos adversos
Los consentimientos informados deben aplicar para todas las encuestas en caso de no ser firmado por el entrevistado no realizar la encuesta
                    </a>--%>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtVCMetodologia" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo III Incentivos a utilizar
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Incentivos Económicos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:RadioButtonList ID="rblIncentivos" runat="server" Width="30%" BackColor="Transparent" RepeatDirection="Horizontal" AutoPostBack="True">
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
                    <asp:TextBox ID="txtPresupuestoIncentivo" runat="server" Width="100%" TextMode="MultiLine" Height="30px" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Distribución Incentivos Económicos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtDistribucionIncentivo" runat="server" Width="100%" TextMode="MultiLine" Height="30px" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Regalos Cliente</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:RadioButtonList ID="rblRegaloClientes" runat="server" Width="30%" RepeatDirection="Horizontal">
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
                    <asp:RadioButtonList ID="rblCompraIpsos" runat="server" Width="30%" RepeatDirection="Horizontal" AutoPostBack="True">
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
                    <asp:TextBox ID="txtPresupuestoCompra" runat="server" Width="100%" TextMode="MultiLine" Height="30px" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Distribución Compra Ipsos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtDistribucionCompra" runat="server" Width="100%" TextMode="MultiLine" Height="30px" Enabled="false"></asp:TextBox>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo IV Ayudas Adicionales
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Incentivos Económicos</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:CheckBoxList ID="chbAyudas" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                    </asp:CheckBoxList>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Método Aceptable de Reclutamiento</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:CheckBoxList ID="chbReclutamiento" runat="server" RepeatDirection="Horizontal">
                    </asp:CheckBoxList>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Observaciones, Exclusiones y Restricciones Específicas</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtExclusionesyRestricciones" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Recursos Propiedad del Cliente</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtRecursosPropiedadesCliente" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                </div>
            </div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Requerimientos De Habeas Data/ Confidencialidad/ Privacidad Y Seguridad De La Información</p>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
                    <asp:TextBox ID="txtHabeasData" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:TextBox>
                </div>
            </div>
            <h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
                <a>Capítulo V Especificaciones generales
                </a>
            </h3>
            <div style="clear: both"></div>
            <div style="width: 100%; clear: both">
                <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
                    <p>Otras especificaciones</p>
                    <%--<a style="font-style: italic;">Este campo es si se quiere adicionar algún tema que no se encuentre en capítulos anteriores</a>--%>
                </div>
                <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">                    
                    <cc1:Editor ID="txtOtrasEspecificaciones" Width="100%" Height="50px" runat="server" NoUnicode="true" />
                    <asp:Label ID="lblOtrasEspecificaciones" Width="100%" runat="server" Visible="false" />
                </div>
            </div>
            <asp:Button ID="btnGuardar" Text="Guardar" runat="server" OnClientClick="formatearHtmlenTexto();" />
            <asp:Button ID="btnRegresar" Text="Regresar" runat="server" />
            <script type="text/javascript">
                var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
                pageReqManger.add_initializeRequest(InitializeRequest);
                pageReqManger.add_endRequest(EndRequest);
            </script>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
