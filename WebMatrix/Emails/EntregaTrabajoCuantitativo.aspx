<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="EntregaTrabajoCuantitativo.aspx.vb"
    Inherits="WebMatrix.EntregaTrabajoCuantitativo" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Label ID="lblAsunto" Text="Especificaciones Técnicas del Trabajo" runat="server"></asp:Label>
        <asp:Label ID="lblEstudioId" Text="" runat="server"></asp:Label>
        <asp:Panel ID="pnlBody" runat="server" Width="90%">
            <div style="font-size: 12px; font-family: 'Metrophobic', Arial, serif; font-weight: 400;
                color: #333333;">
                <p style="margin: 0 0 0 0; padding: 0 0 0 0;">
                    Se han creado las especificaciones técnicas para el siguiente trabajo:</p>
                <asp:Panel ID="pnlEspecificacionesTecnicas" runat="server">
                        <h3 style="float: left; text-align: left;">
        <a>Capítulo I Especificaciones para campo
        </a>
    </h3>
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
    <h3 style="float: left; text-align: left; margin-top:20px; margin-bottom:0px; padding-bottom:0px;">
        <a>Capítulo II Especificaciones para control calidad (Auditoría de Campo, critica, verificación)
        </a>
    </h3>
    <div style="clear:both"></div>
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
            <p>Verificación</p>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Label ID="txtVerificacion" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
        </div>
    </div>
    <div style="width: 100%; clear: both">
        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
            <p>Crítica</p>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Label ID="txtCritica" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
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
    <h3 style="float: left; text-align: left; margin-top:20px; margin-bottom:0px; padding-bottom:0px;">
        <a>Capítulo III Especificaciones para codificación
        </a>
    </h3>
    <div style="clear:both"></div>
    <div style="width: 100%; clear: both">
        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
            <p>Codificación</p>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Label ID="txtCodificacion" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
        </div>
    </div>

    <h3 style="float: left; text-align: left; margin-top:20px; margin-bottom:0px; padding-bottom:0px;">
        <a>Capítulo IV Especificaciones de procesamiento
        </a>
    </h3>
    <div style="clear:both"></div>
    <div style="width: 100%; clear: both">
        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
            <p>Procesamiento</p>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Label ID="txtProcesamiento" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
        </div>
    </div>
    <h3 style="float: left; text-align: left; margin-top:20px; margin-bottom:0px; padding-bottom:0px;">
        <a>Capítulo V Variables de Control
        </a>
    </h3>
    <div style="clear:both"></div>
    <a style="font-style: italic;">Especifique a continuación las variables de control requeridas</a>
    <div style="width: 100%; clear: both">
        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
            <p>Seguridad y confidencialidad de la información</p>
            <a style="font-style: italic;">Verificando la información contenida en: 
•	Procesos de seguridad y confidencialidad de la información en la intranet de calidad.
•	Introducción de cuestionario
</a>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Label ID="txtVCSeguridad" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
        </div>
    </div>
    <div style="width: 100%; clear: both">
        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
            <p>Forma de obtención de los entrevistados</p>
            <a style="font-style: italic;">Verificando la información contenida en la Metodología del estudio en: 
•	Técnica 
•	Diseño muestral 
•	Instrucciones para la recolección 
</a>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Label ID="txtVCObtencion" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
        </div>
    </div>
    <div style="width: 100%; clear: both">
        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
            <p>Grupo objetivo</p>
            <a style="font-style: italic;">Verificando el Cumplimiento de la técnica de recolección de acuerdo con las características del estudio:

•	Hogares, Interceptación, CLT con reclutamiento previo, Probabilístico, Multitécnica, Lista de contactos por barrido y Lista de contactos por cita previa
Metodología del estudio en: 
•	Grupo objetivo 
•	Mercado: Cubrimiento geográfico 
</a>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Label ID="txtVCGrupoObjetivo" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
        </div>
    </div>
    <div style="width: 100%; clear: both">
        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
            <p>Aplicación de Instrumentos</p>
            <a style="font-style: italic;">La correcta aplicación y uso de:
•	Cuestionarios
•	Consentimientos
•	Instructivo
•	Material de apoyo (Tarjetas, naipes, ayudas audiovisuales)
•	Prueba de producto
•	Etc.
</a>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Label ID="txtVCAplicacionInstrumentos" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
        </div>
    </div>
    <div style="width: 100%; clear: both">
        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
            <p>Distribución de Cuotas</p>
            <a style="font-style: italic;">Se valida controlando: 
•	Demográficos: NSE, edad, género, ciudad
•	Celdas: por producto, concepto, empaque
•	Cuotas por marca, frecuencias de consumo u otros comportamientos de los entrevistados relevantes para el estudio.
</a>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Label ID="txtVCDistribucionCuotas" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
        </div>
    </div>
    <div style="width: 100%; clear: both">
        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
            <p>Cumplimiento de Metodología y otras instrucciones</p>
            <a style="font-style: italic;">Se verifica y controla a través de los Instructivos del estudio donde están específicos los requerimientos:
•	Manejo de Protocolos, Toma de fotos durante la entrevista con instrucciones claras, Condiciones de geo cercas, Reporte de incidencias, especificando formato del reporte de estas, Manejo de consentimientos y Reporte de eventos adversos
Los consentimientos informados deben aplicar para todas las encuestas en caso de no ser firmado por el entrevistado no realizar la encuesta
</a>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Label ID="txtVCMetodologia" runat="server" Width="100%" TextMode="MultiLine" Height="30px"></asp:Label>
        </div>
    </div>
    <h3 style="float: left; text-align: left; margin-top:20px; margin-bottom:0px; padding-bottom:0px;">
        <a>Capítulo VI Especificaciones Estadística
        </a>
    </h3>
    <div style="clear:both"></div>
    <div style="width: 100%; clear: both">
        <div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
            <p>Estadística</p>
        </div>
        <div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
            <asp:Literal ID="txtEstadistica" runat="server" />
        </div>
    </div>
    <h3 style="float: left; text-align: left; margin-top:20px; margin-bottom:0px; padding-bottom:0px;">
        <a>Capítulo VII Especificaciones generales
        </a>
    </h3>
    <div style="clear:both"></div>
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
        </asp:Panel>
        
    </div>
    </form>
</body>
</html>
