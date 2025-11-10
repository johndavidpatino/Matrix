<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterProyectos.master" CodeBehind="InstructivoGeneral.aspx.vb" Inherits="WebMatrix.InstructivoGeneral" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
	TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
	<link rel="stylesheet" href="../css/non-estilos.css" />
	<script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
	<script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>

	<style>
		#CPH_Content_CPH_ContentForm_ctl00 label {
			margin: 20px 10px 5px;
			width: 100%;
		}
	</style>
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
	<script>
		function formatearHtmlenTexto() {
			var form = "CPH_Content_CPH_ContentForm_";
			quitarllaves(form + 'txtEspecificacionesCampo');
			quitarllaves(form + 'txtMaterialApoyo');
			quitarllaves(form + 'txtIncidencias');
			quitarllaves(form + 'txtPilotos');
			quitarllaves(form + 'txtAuditoriaCampo');
			quitarllaves(form + 'txtVerificacion');
			quitarllaves(form + 'txtCritica');
			quitarllaves(form + 'txtPilotosCalidad');
			quitarllaves(form + 'txtCodificacion');
			quitarllaves(form + 'txtProcesamiento');
			quitarllaves(form + 'txtVCSeguridad');
			quitarllaves(form + 'txtVCObtencion');
			quitarllaves(form + 'txtVCGrupoObjetivo');
			quitarllaves(form + 'txtVCAplicacionInstrumentos');
			quitarllaves(form + 'txtVC_DistribucionCuotas');
			quitarllaves(form + 'txtVCMetodologia');
			quitarllaves(form + 'txtEstadistica');
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
	<asp:HiddenField ID="hfTrabajoID" runat="server" />
	<asp:HiddenField ID="hfversion" runat="server" />
	<asp:UpdatePanel runat="server">
		<ContentTemplate>
			<div style="width: 100%; clear: both">
				<div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
					<asp:Button ID="btnDuplicar" Text="Nueva Versión" runat="server" Visible="false" />
					<br />
					<br />
					<br />
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
			<div class="spacer"></div>
			<div style="width: 100%; clear: both">
				<div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
					<p>Especificaciones para Campo</p>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtEspecificacionesCampo" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblEspecificacionesCampo" Width="100%" runat="server" Visible="false" />
				</div>
			</div>
			<div style="width: 100%; clear: both">
				<div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
					<p>Material de Entrevista y Apoyo</p>
					<a style="font-style: italic;">Especificar los materiales adicionales que se utilizarán en el proyecto Ej: Tarjetas, naipes, videos, dummies, producto, entre otros</a>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtMaterialApoyo" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblMaterialApoyo" Width="100%" runat="server" Visible="false" />
				</div>
			</div>
			<div style="width: 100%; clear: both">
				<div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
					<p>Incidencias</p>
					<a style="font-style: italic;">Especifique si se requiere manejo de incidencias y su tratamiento</a>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtIncidencias" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblIncidencias" Width="100%" runat="server" Visible="false" />
				</div>
			</div>
			<div style="width: 100%; clear: both">
				<div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
					<p>Pilotos de Campo</p>
					<a style="font-style: italic;">Especifique si se requieren pilotos y el manejo que se le dará en campo</a>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtPilotos" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblPilotos" Width="100%" runat="server" Visible="false" />
				</div>
			</div>
			<h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
				<a>Capítulo II Especificaciones para control calidad (Auditoría de Campo, critica, verificación)
				</a>
			</h3>
			<div style="clear: both"></div>
			<a style="font-style: italic;">Las validaciones y monitorización establecida por IPSOS tienen un mínimo del 20% de acuerdo con lo establecido en el guion de verificación, siguiendo los lineamientos de la norma ISO 20252; para critica aplica el 100% de las preguntas abiertas u otros cuales. En caso de que el estudio requiera un porcentaje superior al estándar debe ser comunicado y cotizado con el gerente de Operaciones.</a>
			<div style="width: 100%; clear: both">
				<div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
					<p>Auditoría de Campo</p>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtAuditoriaCampo" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblAuditoriaCampo" Width="100%" runat="server" Visible="false" />
				</div>
			</div>
			<div style="width: 100%; clear: both">
				<div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
					<p>Verificación (se verifica el 10%; si el cliente externo solicita un porcentaje diferente, especifíquelo)</p>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtVerificacion" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblVerificacion" Width="100%" runat="server" Visible="false" />
				</div>
			</div>
			<div style="width: 100%; clear: both">
				<div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
					<p>Crítica</p>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtCritica" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblCritica" Width="100%" runat="server" Visible="false" />
				</div>
			</div>
			<div style="width: 100%; clear: both">
				<div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
					<p>Pilotos de Campo</p>
					<a style="font-style: italic;">Especifique si se requieren pilotos y el manejo que se le dará en Control Calidad</a>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtPilotosCalidad" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblPilotosCalidad" Width="100%" runat="server" Visible="false" />
				</div>
			</div>
			<h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
				<a>Capítulo III Especificaciones para codificación
				</a>
			</h3>
			<div style="clear: both"></div>
			<a style="font-style: italic;">Informar cuando la aprobación va a ser realizada por otro País, se debe especificar: Qué País la va a realizar, Quién va a ser la persona a cargo de realizar la aprobación e idioma.
			Establecer parciales de aprobación (Si se requiere por tamaño de muestra) con el 50%, 80% y 100%, con el objetivo de que la aprobación final sea en ho-ras (Máximo 5).</a>
			<div style="width: 100%; clear: both">
				<div style="width: 99%; float: left; padding: 2px 2px 2px 2px">
					<p>Codificación (se verifica la codificación al 5%; si el cliente externo solicita un porcentaje diferente, especifíquelo)</p>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtCodificacion" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblCodificacion" Width="100%" runat="server" Visible="false" />
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
					<a style="font-style: italic;">Si hay algún requisito adicional al PDC específiquelo aquí</a>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtProcesamiento" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblProcesamiento" Width="100%" runat="server" Visible="false" />
				</div>
			</div>
			<h3 style="float: left; text-align: left; margin-top: 20px; margin-bottom: 0px; padding-bottom: 0px;">
				<a>Capítulo V Variables de Control
				</a>
			</h3>
			<div style="clear: both"></div>
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
					<cc1:Editor ID="txtVCSeguridad" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblVCSeguridad" Width="100%" runat="server" Visible="false" />
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
					<cc1:Editor ID="txtVCObtencion" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblVCObtencion" Width="100%" runat="server" Visible="false" />
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
					<cc1:Editor ID="txtVCGrupoObjetivo" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblVCGrupoObjetivo" Width="100%" runat="server" Visible="false" />
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
					<cc1:Editor ID="txtVCAplicacionInstrumentos" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblVCAplicacionInstrumentos" Width="100%" runat="server" Visible="false" />
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
					<cc1:Editor ID="txtVC_DistribucionCuotas" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblVC_DistribucionCuotas" Width="100%" runat="server" Visible="false" />
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
					<cc1:Editor ID="txtVCMetodologia" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblVCMetodologia" Width="100%" runat="server" Visible="false" />
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
					<cc1:Editor ID="txtEstadistica" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblEstadistica" Width="100%" runat="server" Visible="false" />
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
					<a style="font-style: italic;">Este campo es si se quiere adicionar algún tema que no se encuentre en capítulos anteriores</a>
				</div>
				<div style="width: 100%; float: left; padding: 2px 2px 2px 2px">
					<cc1:Editor ID="txtOtrasEspecificaciones" NoUnicode="true" Width="100%" Height="50px" runat="server" />
					<asp:Label ID="lblOtrasEspecificaciones" Width="100%" runat="server" Visible="false" />
				</div>
			</div>
			<asp:Button ID="btnGuardar" Text="Guardar" runat="server" OnClientClick="formatearHtmlenTexto();" />
			<asp:Button ID="btnRegresar" Text="Regresar" runat="server" />
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
