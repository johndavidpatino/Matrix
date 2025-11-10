<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterREP.master" CodeBehind="ReportesIndicadoresCronogramaTareas.aspx.vb" Inherits="WebMatrix.ReportesIndicadoresCronogramaTareas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
	<script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
	<script type="text/javascript">
		$(document).ready(function () {
			$('#accordion1').accordion({
				change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
				header: "h2",
				fillSpace: true,
				collapsible: true,
				heightStyle: "fill",
				active: false
			});
			$('#accordion2').accordion({
				change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
				header: "h2",
				fillSpace: true,
				collapsible: true,
				heightStyle: "fill",
				active: false
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
	Indicadores
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
	Indicadores de cronograma de tareas
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Content" runat="server">
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
		function endReq(sender, args) {

			$('#accordion1').accordion({
				change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
				header: "h2",
				fillSpace: true,
				collapsible: true,
				heightStyle: "fill",
				active: true
			});
			$('#accordion2').accordion({
				change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
				header: "h2",
				fillSpace: true,
				collapsible: true,
				heightStyle: "fill",
				active: true
			});
		}
	</script>

	<div style="margin-top: 30px">
		<div id="accordion2">
			<h2>Explicación indicadores</h2>
			<div style="max-width: 100%; min-height: 300px;">
				<strong>Oportunidad Diligencia Planeacion:</strong> Este indicador muestra que tan oportuno fue el diligenciamiento del cronograma, se considera oportuno si se dilgencia dentro de los dos días posteriores a la entrega del proyecto!
		<br />
				<strong>Diligencia Planeacion:</strong> Este indicador muestra en que porcentaje se encuentra diligenciada la planeación
		<br />
				<strong>Ejecución cumple con planeación:</strong> Este indicador muestra en que porcentaje se cumplio lo planeado vs lo ejecutado
		<br />
				<strong>Diligencia Ejecución:</strong> Este indicador muestra el porcentaje de diligenciamieto de la ejecución de las tareas
			</div>
		</div>
		<div id="accordion1">
			<h2>Indicadores por trabajo y tarea</h2>
			<div style="max-width: 100%; min-height: 300px;">
				<asp:Button Text="Exportar a Excel" runat="server" ID="btnExportarExcelVariablesControl" />
				<label>Trabajos con tarea entrega de proyecto</label>
				<asp:DropDownList ID="ddlTrabajosConTareaEntregaProyecto" runat="server"></asp:DropDownList>
				<asp:UpdatePanel ID="upCargarIndicadoresTrabajo" runat="server">
					<ContentTemplate>
						<asp:Button ID="btnCargarIndicadoresTrabajo" runat="server" Text="Mostrar" />
					</ContentTemplate>
				</asp:UpdatePanel>
				<asp:UpdatePanel runat="server" ID="upIndicadoresPorTrabajo" UpdateMode="Conditional">
					<ContentTemplate>
						<asp:GridView ID="gvIndicadoresPorTrabajo" runat="server" AutoGenerateColumns="false">
							<AlternatingRowStyle CssClass="odd" />
							<Columns>
								<asp:BoundField DataField="TrabajoId" HeaderText="TrabajoId" />
								<asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
								<asp:BoundField DataField="Estima" HeaderText="Rol Estima" />
								<asp:BoundField DataField="ResponsableEstimacion" HeaderText="ResponsableEstimacion" />
								<asp:BoundField DataField="Tarea" HeaderText="Tarea" />
								<asp:BoundField DataField="FechaEntregaProyectos" HeaderText="FechaEntregaProyectos" DataFormatString="{0:d}" />
								<asp:BoundField DataField="FechaRegistroPlaneacion" HeaderText="FechaRegistroPlaneacion" DataFormatString="{0:d}" />
								<asp:BoundField DataField="FechaFinPlaneada" HeaderText="FechaFinPlaneada" DataFormatString="{0:d}" />
								<asp:BoundField DataField="FechaFinEjecucion" HeaderText="FechaFinEjecucion" DataFormatString="{0:d}" />
								<asp:BoundField DataField="OportunidadDiligenciaPlaneacion" HeaderText="OportunidadDiligenciaPlaneacion" />
								<asp:BoundField DataField="DiligenciaPlaneacion" HeaderText="DiligenciaPlaneacion" />
								<asp:BoundField DataField="EjecucionCumpleConPlaneacion" HeaderText="EjecucionCumpleConPlaneacion" />
								<asp:BoundField DataField="DiligenciaEjecucion" HeaderText="DiligenciaEjecucion" />
							</Columns>
						</asp:GridView>
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>
		</div>
	</div>
</asp:Content>
