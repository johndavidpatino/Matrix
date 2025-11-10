<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterReportes.master" CodeBehind="PrestacionServicios-CT.aspx.vb" Inherits="WebMatrix.PrestacionServicios_CT" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
	<link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
	<script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
	<script type="text/javascript">
		function loadPlugins() {

			$.validator.addMethod('selectNone',
				function (value, element) {
					return (this.optional(element) == true) || ((value != -1) && (value != ''));
				}, "*Requerido");
			$.validator.addClassRules("mySpecificClass", { selectNone: true });

			$("#<%= txtFechaExpedicion.ClientId %>").mask("99/99/9999");
					$("#<%= txtFechaExpedicion.ClientId %>").datepicker({
						dateFormat: 'dd/mm/yy',
						changeMonth: true,
						changeYear: true,
						yearRange: '-70:+0',
						dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
						monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
						monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
					});

					$("#<%= txtFechaNacimiento.ClientId %>").mask("99/99/9999");
					$("#<%= txtFechaNacimiento.ClientId %>").datepicker({
						dateFormat: 'dd/mm/yy',
						changeMonth: true,
						changeYear: true,
						yearRange: '-70:+0',
						dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
						monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
						monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
					});

					$("#<%= txtFechaIngreso.ClientId %>").mask("99/99/9999");
					$("#<%= txtFechaIngreso.ClientId %>").datepicker({
						dateFormat: 'dd/mm/yy',
						changeMonth: true,
						changeYear: true,
						yearRange: '-20:+0',
						dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
						monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
						monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
					});

					$("#<%= txtFechaUltimoAscenso.ClientId %>").mask("99/99/9999");
					$("#<%= txtFechaUltimoAscenso.ClientId %>").datepicker({
						dateFormat: 'dd/mm/yy',
						changeMonth: true,
						changeYear: true,
						yearRange: '-20:+0',
						dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
						monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
						monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
					});

					$("#<%= txtFechaVencimientoContrato.ClientId %>").mask("99/99/9999");
					$("#<%= txtFechaVencimientoContrato.ClientId %>").datepicker({
						dateFormat: 'dd/mm/yy',
						changeMonth: true,
						changeYear: true,
						yearRange: '-20:+0',
						dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
						monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
						monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
					});

					$("#<%= txtfechabloqueosgt.ClientID%>").mask("99/99/9999");
					$("#<%= txtfechabloqueosgt.ClientID%>").datepicker({
						dateFormat: 'dd/mm/yy',
						changeMonth: true,
						changeYear: true,
						yearRange: '-20:+0',
						dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
						monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
						monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
					});


					$("#<%= txtfechacapacitacion.ClientID%>").mask("99/99/9999");
					$("#<%= txtfechacapacitacion.ClientID%>").datepicker({
						dateFormat: 'dd/mm/yy',
						changeMonth: true,
						changeYear: true,
						yearRange: '-20:+0',
						dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
						monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
						monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
					});

					$("#<%= txtfechaevaluacion.ClientID%>").mask("99/99/9999");
					$("#<%= txtfechaevaluacion.ClientID%>").datepicker({
						dateFormat: 'dd/mm/yy',
						changeMonth: true,
						changeYear: true,
						yearRange: '-20:+0',
						dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
						monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
						monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
					});

					$("#<%= txtFechaRetiro.ClientID%>").mask("99/99/9999");
					$("#<%= txtFechaRetiro.ClientID%>").datepicker({
						dateFormat: 'dd/mm/yy',
						changeMonth: true,
						changeYear: true,
						yearRange: '-20:+0',
						dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
						monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
						monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
					});

					$("#<%= txtfechaentrega.ClientID%>").mask("99/99/9999");
					$("#<%= txtfechaentrega.ClientID%>").datepicker({
						dateFormat: 'dd/mm/yy',
						changeMonth: true,
						changeYear: true,
						yearRange: '-20:+0',
						dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
						monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
						monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
					});

					$("#<%= txtfechavencimiento.ClientID%>").mask("99/99/9999");
					$("#<%= txtfechavencimiento.ClientID%>").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				yearRange: '-20:+0',
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});

			$('.accordion').accordion({
				change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
				header: "h3",
				autoHeight: true
			});

			$(".toolTipFunction").tipTip({
				maxWidth: "auto",
				activation: "focus",
				defaultPosition: "bottom"
			});

			validationForm();
						
		}

		$(document).ready(function () {
			loadPlugins();
		});
		
	</script>
	<style>
		#stylized label {
			width: 120px ;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
	<a>Gestión de Personal PST-CONTRATISTAS</a>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
	<a>Formulario para contratación y creación de personas</a>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
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
			<strong style="float: left;" class="lblTitleInfo">Info: </strong>
			<br />
			<label style="float: left; display: block; width: auto;" id="lblTextInfo" class="lblTextInfo">
			</label>
		</div>
	</div>
	<div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
		<div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
		<div style="float: left; margin-left: 10px; margin-top: 5px;">
			<span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
			<strong style="float: left;" class="lblTitleInfo">Error: </strong>
			<br />
			<label style="float: left; display: block; width: auto;" id="lbltextError" class="lblTextInfo">
			</label>
		</div>
	</div>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:HiddenField ID="hfNuevo" runat="server" Value="0" /> 
			<asp:Panel runat="server" ID="accordion">
				<div id="accordion0" class="accordion">
					<h3>Listado de personas</h3>
					<asp:Panel runat="server">
						<div class="actions">
							<div>
								<label>Búsqueda por</label>
								<asp:TextBox ID="txtCedulaBuscar" runat="server" placeholder="Cedula"></asp:TextBox>
								<asp:TextBox ID="txtNombreBuscar" runat="server" placeholder="Nombre"></asp:TextBox>
								<asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
								<asp:Button ID="BtnExportar" runat="server" Text="Exportar" />
							</div>
							<div class="spacer">
								<asp:Button ID="btnNuevo" runat="server" Text="Nuevo ingreso" />
							</div>
						</div>
						<asp:GridView ID="gvPersonas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
							AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
							DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
							<PagerStyle CssClass="headerfooter ui-toolbar" />
							<SelectedRowStyle CssClass="SelectedRow" />
							<AlternatingRowStyle CssClass="odd" />
							<Columns>
								<asp:BoundField DataField="id" HeaderText="Identificacion" />
								<asp:BoundField DataField="Nombres" HeaderText="Nombres" />
								<asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
								<asp:BoundField DataField="FechaIngreso" HeaderText="FechaIngreso" DataFormatString="{0:dd/MM/yyyy}" />
								<asp:BoundField DataField="FechaRetiro" HeaderText="FechaRetiro" DataFormatString="{0:dd/MM/yyyy}" />
								<asp:BoundField DataField="TipoContratacion" HeaderText="TipoContratacion" />
								<asp:BoundField DataField="Contratista" HeaderText="Contratista" />
								<asp:BoundField DataField="EstadoActual" HeaderText="EstadoActual" />
								<asp:BoundField DataField="Activo" HeaderText="Activo" />
								<asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
									<ItemTemplate>
										<asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
											CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
											ToolTip="Actualizar" />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>
							</Columns>
							<PagerTemplate>
								<div class="pagingButtons">
									<table>
										<tr>
											<td>
												<asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
													Enabled='<%# IIf(gvPersonas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
											</td>
											<td>
												<asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
													Enabled='<%# IIf(gvPersonas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
											</td>
											<td>
												<span class="pagingLinks">[<%= gvPersonas.PageIndex + 1%>-<%= gvPersonas.PageCount%>]</span>
											</td>
											<td>
												<asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
													Enabled='<%# IIf((gvPersonas.PageIndex + 1) = gvPersonas.PageCount, "false", "true") %>'
													SkinID="paging">Siguiente &gt;</asp:LinkButton>
											</td>
											<td>
												<asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
													Enabled='<%# IIf((gvPersonas.PageIndex + 1) = gvPersonas.PageCount, "false", "true") %>'
													SkinID="paging">Ultimo »</asp:LinkButton>
											</td>
										</tr>
									</table>
								</div>
							</PagerTemplate>
						</asp:GridView>
					</asp:Panel>
				</div>
				<div class="spacer"></div>
				<div id="accordion1" class="accordion">
					<h3>Datos básicos</h3>
					<asp:Panel runat="server">
						<div class="form_left">

							<label>
								Fecha Ingreso</label>
							<asp:TextBox ID="txtFechaIngreso" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>


							<label>
								Identificación</label>
							<asp:HiddenField ID="hfID" runat="server" />
							<asp:TextBox ID="txtCedula" runat="server" AutoPostBack="true"></asp:TextBox>
							<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
								TargetControlID="txtCedula">
							</asp:FilteredTextBoxExtender>


							<label>
								Tipo identificación</label>
							<asp:DropDownList ID="ddlTipoIdentificacion" runat="server"></asp:DropDownList>


							<label>
								Lugar Expedicion</label>
							<asp:TextBox ID="txtLugarExpedicion" MaxLength="100" runat="server"></asp:TextBox>


							<label>
								Fecha expedición</label>
							<asp:TextBox ID="txtFechaExpedicion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>


							<label>
								Estado actual</label>
							<asp:DropDownList ID="ddlEstadoActual" runat="server"></asp:DropDownList>


						</div>
						<div>

							<label>
								Apellidos</label>
							<asp:TextBox ID="txtApellidos" MaxLength="100" runat="server"></asp:TextBox>


							<label>
								Nombres</label>
							<asp:TextBox ID="txtNombres" MaxLength="100" runat="server"></asp:TextBox>


							<label>
								Nacionalidad</label>
							<asp:TextBox ID="txtNacionalidad" MaxLength="50" runat="server"></asp:TextBox>


							<label>
								Sexo</label>
							<asp:DropDownList ID="ddlSexo" runat="server"></asp:DropDownList>


							<label>
								Fecha Nacimiento</label>
							<asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>



							<label>
								Fecha retiro</label>
							<asp:TextBox ID="txtFechaRetiro" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>

						</div>
						<div class="form_left">

							<label>
								Estado Civil</label>
							<asp:DropDownList ID="ddlEstadoCivil" runat="server"></asp:DropDownList>


							<label>
								Nivel educativo</label>
							<asp:DropDownList ID="ddlNivelEducativo" runat="server"></asp:DropDownList>


							<label>
								Profesión</label>
							<asp:TextBox ID="txtProfesion" MaxLength="250" runat="server"></asp:TextBox>


							<label>
								Grupo Sanguíneo</label>
							<asp:DropDownList ID="ddlGrupoSanguineo" runat="server"></asp:DropDownList>



							<label>
								Tipo Contrato</label>
							<asp:DropDownList ID="ddlTipoContrato" runat="server" AutoPostBack="true"></asp:DropDownList>


							<label>Contratista</label>
							<asp:DropDownList ID="ddlContratista" runat="server"></asp:DropDownList>


							<label>
								Motivo retiro</label>
							<asp:TextBox ID="txtMotivoRetiro" runat="server"></asp:TextBox>


						</div>
					</asp:Panel>
				</div>
				<div class="spacer"></div>
				<div id="accordion2" class="accordion">
					<h3>Datos de contacto</h3>
					<asp:Panel runat="server">
						<div class="spacer"></div>
						<div class="block">
							<div class="form_left">

								<label>
									Ciudad Residencia</label>
								<asp:DropDownList ID="ddlCiudadResidencia" runat="server"></asp:DropDownList>


								<label>
									Barrio</label>
								<asp:TextBox ID="txtBarrio" MaxLength="100" runat="server"></asp:TextBox>


								<label>
									Direccion</label>
								<asp:TextBox ID="txtDireccion" MaxLength="250" runat="server"></asp:TextBox>

							</div>
							<div class="form_left">

								<label>
									Teléfono 1</label>
								<asp:TextBox ID="txtTelefono1" MaxLength="50" runat="server"></asp:TextBox>


								<label>
									Teléfono 2</label>
								<asp:TextBox ID="txtTelefono2" MaxLength="50" runat="server"></asp:TextBox>


								<label>
									Celular</label>
								<asp:TextBox ID="txtCelular" MaxLength="50" runat="server"></asp:TextBox>

							</div>
							<div class="form_left">

								<label>
									Email personal</label>
								<asp:TextBox ID="txtEmailPersonal" MaxLength="250" runat="server"></asp:TextBox>

							</div>
						</div>
					</asp:Panel>
				</div>
				<div class="spacer"></div>
				<div id="accordion3" class="accordion">
					<h3>Registro</h3>
					<asp:Panel runat="server">
						<div class="spacer"></div>
						<div class="block">
							<div class="form_left">

								<label>
									Fecha Capacitacion General</label>
								<asp:TextBox ID="txtfechacapacitacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>



								<label>
									Fecha Evaluacion 1er dia campo</label>
								<asp:TextBox ID="txtfechaevaluacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>



								<label>
									Usuario STG</label>
								<asp:TextBox ID="txtusuariosgt" runat="server"></asp:TextBox>



								<label>
									Usuario Bloquea STG</label>
								<asp:TextBox ID="txtusuariobloquea" runat="server"></asp:TextBox>


								<label>
									Nuevo Carnet</label>
								<asp:TextBox ID="txtfechaentrega" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>

								<br />
								<div class="spacer"></div>

								<asp:GridView ID="GvCarnet" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
									CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
									DataKeyNames="PersonaId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
									<PagerStyle CssClass="headerfooter ui-toolbar" />
									<SelectedRowStyle CssClass="SelectedRow" />
									<AlternatingRowStyle CssClass="odd" />
									<Columns>
										<asp:BoundField DataField="Personaid" HeaderText="Identificacion" />
										<asp:BoundField DataField="FechaEntrega" HeaderText="FechaEntrega" DataFormatString="{0:dd/MM/yyyy}" />
										<asp:BoundField DataField="FechaVencimiento" HeaderText="FechaVencimiento" DataFormatString="{0:dd/MM/yyyy}" />
										<asp:BoundField DataField="NombreEntrega" HeaderText="Nombre Entrego" />
									</Columns>

								</asp:GridView>


							</div>

							<br />
							<div class="spacer"></div>

							<div class="form_left">

								<label>
									Calificacion</label>
								<asp:TextBox ID="txtcalificacioncap" runat="server"></asp:TextBox>



								<label>
									Calificacion</label>
								<asp:TextBox ID="txtcapacitaciocampo" runat="server"></asp:TextBox>



								<label>
									Clave</label>
								<asp:TextBox ID="txtclave" runat="server"></asp:TextBox>



								<label>
									Fecha Vencimiento</label>
								<asp:TextBox ID="txtfechavencimiento" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>



							</div>

							<div class="form_left">

								<label>
									Nombre Capacitador</label>
								<asp:TextBox ID="txtnombrecapacitador" runat="server"></asp:TextBox>



								<label>
									Nombre Evaluador</label>
								<asp:TextBox ID="txtnombreevaluador" runat="server"></asp:TextBox>



								<label>
									Fecha Bloqueo STG</label>
								<asp:TextBox ID="txtfechabloqueosgt" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>



								<label>
									Persona Entrega</label>
								<asp:TextBox ID="txtnombreentrega" runat="server"></asp:TextBox>



							</div>
						</div>
					</asp:Panel>
				</div>
				<div class="spacer"></div>
				<div id="accordion4" class="accordion">
					<h3>Datos de contratación</h3>
					<asp:Panel runat="server">
						<div class="spacer"></div>
						<div class="block">
							<div class="form_left">



								<label>
									Empresa</label>
								<asp:DropDownList ID="ddlEmpresa" runat="server"></asp:DropDownList>


								<label>
									Fecha Vencimiento Contrato</label>
								<asp:TextBox ID="txtFechaVencimientoContrato" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>


								<label>
									Tipo Salario</label>
								<asp:DropDownList ID="ddlTipoSalario" runat="server"></asp:DropDownList>


								<label>
									Forma Salario</label>
								<asp:DropDownList ID="ddlFormaSalario" runat="server"></asp:DropDownList>


							</div>
							<div class="form_left">





								<label>
									Salario</label>
								<asp:TextBox ID="txtSalario" runat="server"></asp:TextBox>
								<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
									TargetControlID="txtSalario">
								</asp:FilteredTextBoxExtender>



								<label>
									Último Salario</label>
								<asp:TextBox ID="txtUltimoSalario" runat="server"></asp:TextBox>


								<label>
									Fecha Último Ascenso</label>
								<asp:TextBox ID="txtFechaUltimoAscenso" runat="server"></asp:TextBox>



								<label>
									No. de Cuenta</label>
								<asp:TextBox ID="txtCuentaBanco" runat="server"></asp:TextBox>

							</div>

							<div class="form_left">


								<label>
									Banco</label>
								<asp:DropDownList ID="ddlBanco" runat="server"></asp:DropDownList>


								<label>
									Tipo Cuenta</label>
								<asp:DropDownList ID="ddlTipoCuenta" runat="server"></asp:DropDownList>



								<label>
									Último cargo</label>
								<asp:DropDownList ID="ddlUltimoCargo" runat="server"></asp:DropDownList>


								<label>
									Fondo de Pensiones</label>
								<asp:DropDownList ID="ddlFondoPensiones" runat="server"></asp:DropDownList>


								<label>
									Fondo de Cesantías</label>
								<asp:DropDownList ID="ddlFondoCesantias" runat="server"></asp:DropDownList>


								<label>
									EPS</label>
								<asp:DropDownList ID="ddlEPS" runat="server"></asp:DropDownList>


								<label>
									Caja de Compensación</label>
								<asp:DropDownList ID="ddlCajaCompensacion" runat="server"></asp:DropDownList>


								<label>
									ARL</label>
								<asp:DropDownList ID="ddlARL" runat="server"></asp:DropDownList>

							</div>
						</div>
					</asp:Panel>
				</div>
				<div class="spacer"></div>
				<div id="accordion5" class="accordion">
					<h3>Datos de posición</h3>
					<asp:Panel runat="server">
						<div class="spacer"></div>
						<div class="block">
							<div class="form_left">

								<label>
									BU</label>
								<asp:DropDownList ID="ddlBU" runat="server" AutoPostBack="true"></asp:DropDownList>


								<label>
									Area</label>
								<asp:DropDownList ID="ddlArea" runat="server"></asp:DropDownList>


								<label>
									Sede</label>
								<asp:DropDownList ID="ddlSede" runat="server"></asp:DropDownList>


								<label>
									Cargo</label>
								<asp:DropDownList ID="ddlCargo" runat="server" AutoPostBack="true"></asp:DropDownList>



								<label>
									Jefe inmediato</label>
								<asp:DropDownList ID="ddlJefeInmediato" runat="server"></asp:DropDownList>

							</div>
							<div style="clear: both"></div>
							<div>
								<div>
									<label style="margin-left: 10px">
										Tipo encuestador:
									</label>
								</div>
								<div style="display: inline-block">
									<div>
										<asp:CheckBox ID="ckbCampo" runat="server" Text="Campo" TextAlign="left" />
									</div>
									<div>
										<asp:CheckBox ID="ckbtraking" runat="server" Text="Tracking" TextAlign="left" />
									</div>
									<div>
										<asp:CheckBox ID="ckbespecializado" runat="server" Text="Especializado" TextAlign="left" />
									</div>
									<div>
										<asp:CheckBox ID="ckbbilingue" runat="server" Text="Bilingue" TextAlign="left" />
									</div>
									<div>
										<asp:CheckBox ID="ckbtelefonico" runat="server" Text="Telefonico" TextAlign="left" />
									</div>
									<div>
										<asp:CheckBox ID="ckbMystery" runat="server" Text="Mystery" TextAlign="left" />
									</div>
								</div>
								<asp:DropDownList ID="ddlTipoEncuestador" runat="server" CssClass="mySpecificClass dropdowntext"
									Enabled="False" Visible="false">
								</asp:DropDownList>
							</div>
							<div style="clear: both"></div>
							<div class="form_left">

								<label>
									Nivel</label>
								<asp:DropDownList ID="ddlNivelBI" runat="server">
									<asp:ListItem Text="0" Value="0"></asp:ListItem>
									<asp:ListItem Text="1" Value="1"></asp:ListItem>
									<asp:ListItem Text="2" Value="2"></asp:ListItem>
									<asp:ListItem Text="3" Value="3"></asp:ListItem>
									<asp:ListItem Text="4" Value="4"></asp:ListItem>
									<asp:ListItem Text="5" Value="5"></asp:ListItem>
									<asp:ListItem Text="6" Value="6"></asp:ListItem>
									<asp:ListItem Text="7" Value="7"></asp:ListItem>
									<asp:ListItem Text="8" Value="8"></asp:ListItem>
									<asp:ListItem Text="9" Value="9"></asp:ListItem>
									<asp:ListItem Text="10" Value="10"></asp:ListItem>
								</asp:DropDownList>


								<label>HeadCount</label>
								<asp:CheckBox ID="chbHeadCount" runat="server" />

							</div>
							<div class="form_left" style="visibility: hidden">

								<label>¿Acceso a Matrix?</label>
								<asp:CheckBox ID="chbAccesoMatrix" runat="server" />


								<label>Usuario Matrix</label>
								<asp:TextBox ID="txtUsuarioMatrix" runat="server"></asp:TextBox>


								<label>Correo corporativo</label>
								<asp:TextBox ID="txtCorreoCorporativo" runat="server"></asp:TextBox>

							</div>
							<div class="actions">
								<asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
								<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
								<asp:Button ID="btnCargar" runat="server" Text="Cargar Documentos" />
							</div>
						</div>
					</asp:Panel>
				</div>
				<div class="spacer"></div>
			</asp:Panel>
		</ContentTemplate>
	</asp:UpdatePanel>
	<script type="text/javascript">
		var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
		pageReqManger.add_initializeRequest(InitializeRequest);
		pageReqManger.add_endRequest(EndRequest);
	</script>
</asp:Content>
