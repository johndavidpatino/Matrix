<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/NewSiteWithReturnInit.master" CodeBehind="SolicitudAusenciaIncapacidades.aspx.vb" Inherits="WebMatrix.TH_SolicitudAusenciaIncapacidades" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH_HeadPage" ContentPlaceHolderID="CPH_Head" runat="server">
	<%--<link rel="stylesheet" href="../Styles/NewSite.css" type="text/css" />--%>
	<link rel="stylesheet" href="../Styles/Newformulario.css" type="text/css" />
	<script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
	<script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
	<link href="../css/Bootstrap/css/bootstrap.css" rel="stylesheet" />
	<script type="text/javascript">
		$(document).ready(function () {

			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerIni);
			bindPickerIni();

			Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerFin);
			bindPickerFin();

			$('#DevolucionTarea').parent().appendTo("form");

			$('#BusquedaProveedores').dialog(
				{
					modal: true,
					autoOpen: false,
					title: "Proveedores disponibles",
					width: "600px"
				});

			$('#BusquedaProveedores').parent().appendTo("form");

			$('#BusquedaSolicitantes').dialog(
				{
					modal: true,
					autoOpen: false,
					title: "Solicitantes",
					width: "600px"
				});

			$('#BusquedaSolicitantes').parent().appendTo("form");

			$('#BusquedaJBEJBICC').dialog(
				{
					modal: true,
					autoOpen: false,
					title: "JBEJBICC",
					width: "600px"
				});

			$('#BusquedaJBEJBICC').parent().appendTo("form");

			$('#BusquedaCuentasContables').dialog(
				{
					modal: true,
					autoOpen: false,
					title: "Búsqueda JBEJBICC",
					width: "600px"
				});

			$('#BusquedaCuentasContables').parent().appendTo("form");

			$('#BusquedaObservacionesAprobacion').dialog(
				{
					modal: true,
					autoOpen: false,
					title: "Observaciones de Aprobación",
					width: "600px"
				});

			$('#BusquedaObservacionesAprobacion').parent().appendTo("form");

			$('#anularOrden').dialog(
				{
					modal: true,
					autoOpen: false,
					title: "Anular Orden",
					width: "600px"
				});

			$('#anularOrden').parent().appendTo("form");



			$('#CargaArchivos').dialog({
				modal: true,
				autoOpen: false,
				title: "Carga archivos",
				width: "600px"
			});

		});

		function MostrarProveedores() {
			$('#BusquedaProveedores').dialog("open");
		}

		function CerrarProveedores() {
			$('#BusquedaProveedores').dialog("close");
		}

		function MostrarSolicitantes(tipo) {
			$('#CPH_Content_hfTipoAprobacion').val(tipo);
			$('#BusquedaSolicitantes').dialog("open");
		}

		function CerrarSolicitantes() {
			$('#BusquedaSolicitantes').dialog("close");
		}
		function MostrarCentrosCostos() {
			$('#BusquedaJBEJBICC').dialog("open");
		}
		function MostrarCuentasContables() {
			$('#BusquedaCuentasContables').dialog("open");
		}
		function CerrarJBEJBICC() {
			$('#BusquedaJBEJBICC').dialog("close");
		}
		function CerrarCuentasContables() {
			$('#BusquedaCuentasContables').dialog("close");
		}

		function MostrarObservacionesAprobacion() {
			$('#BusquedaObservacionesAprobacion').dialog("open");
		}
		function CerrarObservacionesAprobacion() {
			$('#BusquedaObservacionesAprobacion').dialog("close");
		}

		function bindPickerIni() {
			$("input[type=text][id*=txtFecha]").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});
		}

		function bindPickerFin() {
			$("input[type=text][id*=txtFechaEntrega]").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});
		}

		function bindPickerFin() {
			$("input[type=text][id*=txtFechaSolicitud]").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});
		}

		function MostrarAnularOrden(tipo) {
			$('#anularOrden').dialog("open");
		}

		function CerrarAnularOrden() {
			$('#anularOrden').dialog("close");
		}

		function bloquearUI() {
			$.blockUI({ message: "Procesando ...." });
		}
	</script>
	<style>
		.transparent {
			background-color: transparent !important;
			float: none !important;
		}

		.btn {
			float: left !important;
			padding: 0 20px !important;
			height: 30px !important;
			font-size: 12px !important;
			color: #fff !important;
			outline: none !important;
			background-color: #00ada8 !important;
			border: none !important;
			transition: all 0.2s ease !important;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
	Vacaciones y Gestión de Ausencias
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Menu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
	<div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
		<div id="notificationHide">
			<img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
				onclick="runEffect('info');" style="cursor: pointer;" />
			<img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
				title="Ultima notificacion de error" style="cursor: pointer;" />
		</div>
	</div>
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
	<asp:UpdatePanel ID="CPH_Content" runat="server">
		<ContentTemplate>
			<div class="col-md-12">
				<asp:HiddenField ID="hfId" runat="server" Value="0" />
				<asp:HiddenField ID="hfEstado" runat="server" Value="0" />
				<asp:HiddenField ID="hfProveedor" runat="server" Value="0" />
				<asp:HiddenField ID="hfTipoOrden" runat="server" Value="0" />
				<asp:HiddenField ID="hfSolicitante" runat="server" Value="0" />
				<asp:HiddenField ID="hfTipoBusqueda" runat="server" Value="0" />
				<asp:HiddenField ID="hfProveedorSearch" runat="server" Value="0" />
				<asp:HiddenField ID="hfSolicitanteSearch" runat="server" Value="0" />
				<asp:HiddenField ID="hfCentroCosto" runat="server" Value="0" />
				<asp:HiddenField ID="hfDuplicar" runat="server" Value="0" />
				<asp:HiddenField ID="hfIdAnterior" runat="server" Value="0" />
				<asp:HiddenField ID="hfEvaluador" runat="server" Value="0" />
				<asp:HiddenField ID="hfTipoAprobacion" runat="server" />

				<div class="col-md-12">
					<div class="panel panel-primary" id="pnlTitulo" runat="server" style="border: none;">
						<div class="panel-heading text-left">
							Gestionar
						</div>
						<div class="panel-body" style="background-image: url(../images/fnd-section.png); background-position: right; background-repeat: no-repeat;">

							<div class="col-md-2" style="margin: 0 -10px;">
								<%--<a href="#" class="nav-tabs-dropdown btn btn-block btn-primary">Tabs</a>--%>
								<ul id="nav-tabs-wrapper" class="nav nav-tabs nav-pills nav-stacked well">
									<li id="liMenu1" runat="server">
										<a href="#vtab1" data-toggle="tab" style="display: none;">
											<asp:LinkButton ID="lbMenuNew" runat="server">Nueva Solicitud</asp:LinkButton>
										</a>
									</li>
									<li id="liMenu2" runat="server">
										<a href="#vtab2" data-toggle="tab" style="display: none;">
											<asp:LinkButton ID="lbMenuPendientes" runat="server">Vacaciones y beneficios pendientes</asp:LinkButton>
										</a>
									</li>
									<li id="liMenu3" runat="server">
										<a href="#vtab3" data-toggle="tab" style="display: none;">
											<asp:LinkButton ID="lbMenuHistorial" runat="server">Historial</asp:LinkButton>
										</a>
									</li>
									<li id="liMenu4" runat="server">
										<a href="#vtab4" data-toggle="tab" style="display: none;">
											<asp:LinkButton ID="lbMenuAprobaciones" runat="server">Solicitudes por Aprobar</asp:LinkButton>
										</a>
									</li>
								</ul>
							</div>

							<div class="col-md-9">
								<asp:Panel ID="pnlNuevaSolicitud" runat="server" Visible="false">
									<div class="campo-formulario col-md-12">
										<div style="display: flex; flex-wrap: wrap;">
											<div>
												<label for="ddlTipoSolicitud">Tipo solicitud</label>
												<asp:DropDownList ID="ddlTipoSolicitud" CssClass="form-control select-form" runat="server" AutoPostBack="true"></asp:DropDownList>
											</div>
											<div class="spacer"></div>
											<div>
												<label for="txtFechaInicioSolicitud">Fecha Inicial</label>
												<asp:TextBox ID="txtFechaInicioSolicitud" CssClass="form-control" runat="server" AutoPostBack="true"></asp:TextBox>
											</div>
											<div>
												<label for="txtFechaFinSolicitud">Fecha Fecha Final</label>
												<asp:TextBox ID="txtFechaFinSolicitud" CssClass="form-control" runat="server" AutoPostBack="true"></asp:TextBox>
											</div>
											<div>
												<label for="txtDiasCalendario">Dias calendario</label>
												<asp:TextBox ID="txtDiasCalendario" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
											</div>
											<div>
												<label for="txtDiasLaborales">Dias laborales</label>
												<asp:TextBox ID="txtDiasLaborales" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
											</div>
											<div>
												<label for="txtObservaciones">Comentarios</label>
												<asp:TextBox ID="txtObservaciones" CssClass="form-control" runat="server"></asp:TextBox>
											</div>
											<div>
												<label for="ddlAprobador">Aprobador</label>
												<asp:DropDownList ID="ddlAprobador" CssClass="form-control select-form" runat="server"></asp:DropDownList>
											</div>
										</div>
										<div class="row">
											<div class="form-group" style="margin-top: 10px;">
												<asp:Label ID="lblAvisoIncapacidad" runat="server" Font-Bold="true" Text="Tenga presente que debe entregar la incapacidad original a Recursos Humanos. " Visible="false" />
												<asp:Label ID="lblAvisoIncapacidad3dias" runat="server" Font-Bold="true" Text="Es necesario que agregue también la historia clínica o epicrisis." Visible="false" />
												<asp:Button ID="btnSubmitAusencia" CssClass="col-md-3" runat="server" Text="Radicar" />
											</div>
										</div>
									</div>
									<asp:Panel ID="pnlIncapacidad" runat="server" Visible="false">
										<div class="campo-formulario col-md-12">
											<div style="display: flex; flex-wrap: wrap;">
												<div>
													<label for="ddlIncapacidadEntidad">Entidad consulta</label>
													<asp:DropDownList ID="ddlIncapacidadEntidad" CssClass="form-control select-form" runat="server">
														<asp:ListItem Value="0" Text="--Seleccione--"></asp:ListItem>
														<asp:ListItem Value="1" Text="EPS"></asp:ListItem>
														<asp:ListItem Value="2" Text="ARL"></asp:ListItem>
														<asp:ListItem Value="3" Text="Prepagada"></asp:ListItem>
													</asp:DropDownList>
												</div>
												<div>
													<label for="txtIncapacidadIPS">IPS Prestadora</label>
													<asp:TextBox ID="txtIncapacidadIPS" MaxLength="100" CssClass="form-control" runat="server"></asp:TextBox>
												</div>
												<div>
													<label for="txtIncapacidadNoRegistroMedico">No. Registro Médico</label>
													<asp:TextBox ID="txtIncapacidadNoRegistroMedico" MaxLength="20" ReadOnly="false" CssClass="form-control" runat="server"></asp:TextBox>
												</div>
												<div>
													<label for="ddlIncapacidadTipo">Tipo Incapacidad</label>
													<asp:DropDownList ID="ddlIncapacidadTipo" CssClass="form-control select-form" runat="server">
														<asp:ListItem Value="1" Text="No Aplica"></asp:ListItem>
														<asp:ListItem Value="2" Text="Inicial"></asp:ListItem>
														<asp:ListItem Value="3" Text="Prórroga"></asp:ListItem>
													</asp:DropDownList>
												</div>
												<div>
													<label for="ddlIncapacidadClaseAusencia">Clase de Ausencia</label>
													<asp:DropDownList ID="ddlIncapacidadClaseAusencia" CssClass="form-control select-form" runat="server" AutoPostBack="true">
														<asp:ListItem Value="0" Text="--Seleccione--"></asp:ListItem>
														<asp:ListItem Value="1" Text="Enfermedad General"></asp:ListItem>
														<asp:ListItem Value="2" Text="Enfermedad Laboral"></asp:ListItem>
														<asp:ListItem Value="3" Text="Accidente de Trabajo"></asp:ListItem>
														<asp:ListItem Value="4" Text="Accidente de Tránsito"></asp:ListItem>
													</asp:DropDownList>
												</div>
												<div>
													<label for="ddlIncapacidadSOAT">SOAT</label>
													<asp:DropDownList ID="ddlIncapacidadSOAT" CssClass="form-control select-form" runat="server" Enabled="false">
														<asp:ListItem Value="1" Text="No Aplica"></asp:ListItem>
														<asp:ListItem Value="2" Text="SI"></asp:ListItem>
														<asp:ListItem Value="3" Text="NO"></asp:ListItem>
													</asp:DropDownList>
												</div>
												<div>
													<label for="txtFechaAccidenteTrabajo">Fecha Accidente de Trabajo</label>
													<asp:TextBox ID="txtFechaAccidenteTrabajo" CssClass="form-control" runat="server" Enabled="false"></asp:TextBox>
												</div>
												<div>
													<label for="txtIncapacidadObservaciones">Comentarios Incapacidad</label>
													<asp:TextBox ID="txtIncapacidadObservaciones" CssClass="form-control" runat="server"></asp:TextBox>
												</div>
												<div class="spacer"></div>
												<div>
													<label for="ddlDXAsociado">CIE</label>
													<asp:TextBox ID="txtCIE" runat="server" AutoPostBack="true" AutoCompleteType="Disabled"></asp:TextBox>
												</div>
												<div>
													<label for="ddlDX">Categoria</label>
													<asp:Label ID="lblDX" runat="server" Text=""></asp:Label>
												</div>
											</div>
											<div class="row">
												<div class="form-group" style="margin-top: 50px;">
													<asp:Button ID="btnIncapacidadSubmit" CssClass="col-md-3" runat="server" Text="Radicar" />
												</div>
											</div>
										</div>
									</asp:Panel>
								</asp:Panel>
								<asp:Panel ID="pnlBeneficiosPendientes" runat="server" Visible="false">
									<asp:GridView ID="gvBeneficiosPendientes" runat="server" AutoGenerateColumns="false" Width="100%"
										AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar">
										<PagerStyle CssClass="headerfooter ui-toolbar" />
										<SelectedRowStyle CssClass="SelectedRow" />
										<AlternatingRowStyle CssClass="odd" />
										<Columns>
											<asp:BoundField DataField="Beneficio" HeaderText="Beneficio" />
											<asp:BoundField DataField="Dias" HeaderText="No. Días Pendientes" />
										</Columns>
									</asp:GridView>
								</asp:Panel>
								<asp:Panel ID="pnlHistorialAusencia" runat="server" Visible="false">
									<asp:GridView ID="gvHistorialAusencia" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="ID"
										AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar">
										<PagerStyle CssClass="headerfooter ui-toolbar" />
										<SelectedRowStyle CssClass="SelectedRow" />
										<AlternatingRowStyle CssClass="odd" />
										<Columns>
											<asp:BoundField DataField="FINICIO" HeaderText="Inicio" DataFormatString="{0:d}" />
											<asp:BoundField DataField="FFIN" HeaderText="Fin" DataFormatString="{0:d}" />
											<asp:BoundField DataField="DIASCALENDARIO" HeaderText="Dias Cal" />
											<asp:BoundField DataField="DIASLABORALES" HeaderText="Dias Lab" />
											<asp:BoundField DataField="TIPO" HeaderText="Tipo" />
											<asp:BoundField DataField="ESTADO" HeaderText="Estado" />
											<asp:BoundField DataField="OBSERVACIONESSOLICITUD" HeaderText="Observaciones Solicitud" />
											<asp:BoundField DataField="OBSERVACIONESAPROBACION" HeaderText="Observaciones Aprobación / Rechazo" />
											<asp:TemplateField HeaderText="Anular" ShowHeader="False">
												<ItemTemplate>
													<asp:ImageButton ID="Anular" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
														CommandName="Anular" ImageUrl="~/Images/Delete_16.png" Text="Anular" ToolTip="Anular la Solicitud" />
												</ItemTemplate>
												<ItemStyle HorizontalAlign="Center" />
											</asp:TemplateField>
										</Columns>
									</asp:GridView>
								</asp:Panel>
								<asp:Panel ID="pnlAprobaciones" runat="server" Visible="false">
									<asp:GridView ID="gvAprobacionesPendientes" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="ID"
										AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar" EmptyDataText="No hay aprobaciones pendientes">
										<PagerStyle CssClass="headerfooter ui-toolbar" />
										<SelectedRowStyle CssClass="SelectedRow" />
										<AlternatingRowStyle CssClass="odd" />
										<Columns>
											<asp:BoundField DataField="EMPLEADO" HeaderText="Empleado" />
											<asp:BoundField DataField="FINICIO" HeaderText="F Inicio" DataFormatString="{0:d}" />
											<asp:BoundField DataField="FFIN" HeaderText="F Fin" DataFormatString="{0:d}" />
											<asp:BoundField DataField="DIASLABORALES" HeaderText="Dias Lab" />
											<asp:BoundField DataField="TIPO" HeaderText="Tipo" />
											<asp:BoundField DataField="OBSERVACIONESSOLICITUD" HeaderText="Observaciones Solicitud" />
											<asp:TemplateField HeaderText="Aprobar" ShowHeader="False">
												<ItemTemplate>
													<asp:ImageButton ID="Aprobar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
														CommandName="Aprobar" ImageUrl="~/Images/Select_16.png" Text="Aprobar" ToolTip="Aprobar Solicitud" />
												</ItemTemplate>
												<ItemStyle HorizontalAlign="Center" />
											</asp:TemplateField>
											<asp:TemplateField HeaderText="Rechazar" ShowHeader="False">
												<ItemTemplate>
													<asp:ImageButton ID="Rechazar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
														CommandName="Rechazar" ImageUrl="~/Images/Delete_16.png" Text="Rechazar" ToolTip="Rechazar Solicitud" />
												</ItemTemplate>
												<ItemStyle HorizontalAlign="Center" />
											</asp:TemplateField>
										</Columns>
									</asp:GridView>
								</asp:Panel>
							</div>
						</div>
					</div>
				</div>

			</div>
		</ContentTemplate>
	</asp:UpdatePanel>

	<div id="BusquedaProveedores" class="modalMatrix">
		<asp:UpdatePanel ID="UPanelProveedores" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
			<ContentTemplate>
				<div class="row form-horizontal modaldiv">
					<div class="col-md-5">
						<asp:TextBox ID="txtNitProveedor" runat="server" placeholder="NIT o CC" BorderWidth="1px"></asp:TextBox>
					</div>
					<div class="col-md-5">
						<asp:TextBox ID="txtNombreProveedor" runat="server" placeholder="Razón Social o Nombre" BorderWidth="1px"></asp:TextBox>
					</div>
					<div class="col-md-2">
						<asp:Button ID="btnBuscarProveedor" runat="server" Text="Buscar" CssClass="btn btn-primary" />
					</div>
				</div>
				<div class="col-md-12" style="margin: 2px;">
					<div style="width: 100%; height: 300px; overflow-y: auto;">
						<asp:GridView ID="gvProveedores" runat="server" AutoGenerateColumns="False"
							CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
							DataKeyNames="Identificacion" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
							<PagerStyle CssClass="headerfooter ui-toolbar" />
							<SelectedRowStyle CssClass="SelectedRow" />
							<AlternatingRowStyle CssClass="odd" />
							<Columns>
								<asp:BoundField DataField="Identificacion" HeaderText="Identificacion" />
								<asp:BoundField DataField="Nombre" HeaderText="RazonSocial" />
								<asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
								<asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
									<ItemTemplate>
										<asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
											CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarProveedores()" />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>
							</Columns>
						</asp:GridView>
					</div>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	<div id="BusquedaSolicitantes" class="modalMatrix">
		<asp:UpdatePanel ID="UPanelSolicitantes" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
			<ContentTemplate>
				<div class="row form-horizontal modaldiv">
					<div class="col-md-5">
						<asp:TextBox ID="txtCedulaSolicitante" runat="server" placeholder="Cedula" BorderWidth="1px"></asp:TextBox>
					</div>
					<div class="col-md-5">
						<asp:TextBox ID="txtNombreSolicitante" runat="server" placeholder="Nombre" BorderWidth="1px"></asp:TextBox>
					</div>
					<div class="col-md-2">
						<asp:Button ID="btnBuscarSolicitante" runat="server" Text="Buscar" CssClass="btn btn-primary" />
					</div>
				</div>
				<div class="col-md-12" style="margin: 2px;">
					<div style="width: 100%; height: 300px; overflow-y: auto;">
						<asp:GridView ID="gvSolicitantes" runat="server" AutoGenerateColumns="False"
							CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
							DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
							<PagerStyle CssClass="headerfooter ui-toolbar" />
							<SelectedRowStyle CssClass="SelectedRow" />
							<AlternatingRowStyle CssClass="odd" />
							<Columns>
								<asp:BoundField DataField="Nombres" HeaderText="Nombres" />
								<asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
								<asp:BoundField DataField="Cargo" HeaderText="Cargo" />
								<asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
								<asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
									<ItemTemplate>
										<asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
											CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarSolicitantes()" />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>
							</Columns>
						</asp:GridView>
					</div>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	<div id="BusquedaJBEJBICC">
		<asp:UpdatePanel ID="upJBEJBICC" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
			<ContentTemplate>
				<asp:TextBox ID="txtJBEJBICC" runat="server" placeholder="Valor busqueda"></asp:TextBox>
				<asp:Button ID="btnBuscarJBEJBICC" runat="server" Text="Buscar" />
				<div class="actions"></div>

				<div style="overflow: scroll; width: 500px; height: 300px; margin-left: auto; margin-right: auto">
					<asp:GridView ID="gvJBEJBICC" runat="server" Width="80%" AutoGenerateColumns="False"
						CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
						DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
						<PagerStyle CssClass="headerfooter ui-toolbar" />
						<SelectedRowStyle CssClass="SelectedRow" />
						<AlternatingRowStyle CssClass="odd" />
						<Columns>
							<asp:BoundField DataField="Nombre" HeaderText="Nombre" />
							<asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
								<ItemTemplate>
									<asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
										CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarJBEJBICC()" />
								</ItemTemplate>
								<ItemStyle HorizontalAlign="Center" />
							</asp:TemplateField>
						</Columns>
					</asp:GridView>
				</div>
				<br />
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	<div id="BusquedaCuentasContables" class="modalMatrix">
		<asp:UpdatePanel ID="upCuentasContables" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
			<ContentTemplate>
				<div class="row form-horizontal modaldiv">
					<div class="col-md-5">
						<asp:TextBox ID="txtNumeroCuenta" runat="server" placeholder="Número cuenta" BorderWidth="1px"></asp:TextBox>
					</div>
					<div class="col-md-5">
						<asp:TextBox ID="txtDescripcion" runat="server" placeholder="Descripción" BorderWidth="1px"></asp:TextBox>
					</div>
					<div class="col-md-2">
						<asp:Button ID="btnBuscarCuentaContable" runat="server" Text="Buscar" CssClass="btn btn-primary" />
					</div>
				</div>
				<div class="col-md-12" style="margin: 2px;">
					<div style="width: 100%; height: 300px; overflow-y: auto;">
						<asp:GridView ID="gvCuentasContables" runat="server" AutoGenerateColumns="False"
							CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
							DataKeyNames="id" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
							<PagerStyle CssClass="headerfooter ui-toolbar" />
							<SelectedRowStyle CssClass="SelectedRow" />
							<AlternatingRowStyle CssClass="odd" />
							<Columns>
								<asp:BoundField DataField="NumeroCuenta" HeaderText="NumeroCuenta" />
								<asp:BoundField DataField="Descripcion" HeaderText="Descripcion" />
								<asp:TemplateField HeaderText="Seleccionar" ShowHeader="False" ItemStyle-CssClass="text-center transparent">
									<ItemTemplate>
										<asp:ImageButton ID="imgArchivos" runat="server" CausesValidation="False" CssClass="transparent" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
											CommandName="Seleccionar" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" OnClientClick="CerrarCuentasContables()" />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>
							</Columns>
						</asp:GridView>
					</div>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	<div id="BusquedaObservacionesAprobacion">
		<asp:UpdatePanel ID="upObservacionesAprobacion" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
			<ContentTemplate>
				<div style="margin: 30px 2px 2px; width: 95%;">
					<asp:Panel runat="server" ID="pnlObservacionesAprobacion">
						<div style="width: 100%; max-height: 300px; min-height: 100px; overflow-y: auto;">
							<asp:GridView ID="gvObservacionesAprobacion" runat="server" AutoGenerateColumns="False"
								CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
								DataKeyNames="id" AllowPaging="False" EmptyDataText="No hay ninguna Observación registrada para esta Orden">
								<PagerStyle CssClass="headerfooter ui-toolbar" />
								<SelectedRowStyle CssClass="SelectedRow" />
								<AlternatingRowStyle CssClass="odd" />
								<Columns>
									<asp:BoundField DataField="IdOrden" HeaderText="Id Orden" />
									<asp:BoundField DataField="Usuario" HeaderText="Usuario" />
									<asp:BoundField DataField="Observaciones" HeaderText="Observaciones" />
								</Columns>
							</asp:GridView>
						</div>
					</asp:Panel>
				</div>
				<div class="col-md-11" style="margin: 0px auto;">
					<div class="col-md-3">
						<asp:Button ID="btnGenerarPDF" CssClass="btn btn-primary" runat="server" Text="Generar PDF Orden" OnClientClick="CerrarObservacionesAprobacion()" />
					</div>
					<div class="col-md-2">
						<asp:Button ID="btnNotaCredito" CssClass="btn btn-primary" runat="server" Text="Nota Crédito" OnClientClick="CerrarObservacionesAprobacion()" />
					</div>
					<div class="col-md-4">
						<asp:Button ID="btnDocEquivalente" CssClass="btn btn-primary" runat="server" Text="Documento Equivalente" OnClientClick="CerrarObservacionesAprobacion()" />
					</div>
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	<div id="anularOrden">
		<asp:UpdatePanel ID="upAnularOrden" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
			<ContentTemplate>
				<div class="form-group">
					<asp:Label Text="Observación de Anulación" ID="lblObserAnulacion" runat="server" />
					<br />
					<asp:TextBox ID="txtObservacionAnulacion" runat="server" placeholder="Observación de anulación" Width="100%" TextMode="MultiLine" Height="150px" BorderColor="#13b0a8" BorderStyle="Solid" BorderWidth="1px"></asp:TextBox>
					<br />
					<div class="col-md-1 col-md-offset-10">
						<asp:Button ID="btnAnularOrden" runat="server" CssClass="btn btn-primary" Text="Guardar" OnClientClick="CerrarAnularOrden()" />
					</div>
				</div>
				<div class="actions"></div>
				<br />
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>

</asp:Content>
