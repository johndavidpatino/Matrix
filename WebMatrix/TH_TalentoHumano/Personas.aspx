<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRRHH.master" CodeBehind="Personas.aspx.vb" Inherits="WebMatrix.PersonasF" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
	<link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
	<script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
	<script type="text/javascript">
		function loadPlugins() {

			$("#<%= txtFechaExpedicion.ClientId %>").mask("99/99/9999");
			$("#<%= txtFechaExpedicion.ClientId %>").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});

			$("#<%= txtFechaNacimiento.ClientId %>").mask("99/99/9999");
			$("#<%= txtFechaNacimiento.ClientId %>").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});

			$("#<%= txtFechaIngreso.ClientId %>").mask("99/99/9999");
			$("#<%= txtFechaIngreso.ClientId %>").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});

			$("#<%= txtFechaUltimoAscenso.ClientId %>").mask("99/99/9999");
			$("#<%= txtFechaUltimoAscenso.ClientId %>").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});

			$("#<%= txtFechaVencimientoContrato.ClientId %>").mask("99/99/9999");
			$("#<%= txtFechaVencimientoContrato.ClientId %>").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});


			$('#accordion').accordion({
				change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
				header: "h3",
				autoHeight: false
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Menu" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_BreadCumbs" runat="server">
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
</asp:Content>
<asp:Content ID="Content8" ContentPlaceHolderID="CPH_Titulo" runat="server">
	<a>Gestión de Personal</a>
</asp:Content>
<asp:Content ID="Content9" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
</asp:Content>
<asp:Content ID="Content10" ContentPlaceHolderID="CPH_Content" runat="server">
	<a>Formulario para contratación y creación de personas</a>
	<div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
		<div id="notificationHide">
			<img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
				onclick="runEffect('info');" style="cursor: pointer;" />
			<img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
				title="Ultima notificacion de error" style="cursor: pointer;" />
		</div>
	</div>
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
			<asp:HiddenField ID="hfNuevo" runat="server" Value="0" />
			<div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
				<div class="form_right" onclick="runEffect('info');" style="cursor: pointer;">
					x
				</div>
				<p>
					<span class="ui-icon ui-icon-info"></span><strong>Info: </strong>
					<label id="lblTextInfo">
					</label>
				</p>
			</div>
			<div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
				<div class="form_right" onclick="runEffect('error');" style="cursor: pointer;">
					x
				</div>
				<p>
					<span class="ui-icon ui-icon-alert"></span><strong>Error: </strong>
					<label id="lbltextError">
					</label>
				</p>
			</div>
			<div id="accordion">
				<div id="accordion0">
					<h3><a href="#">Listado de personas</a></h3>
					<div class="block">
						<div class="actions">
							<div class="form_left">
								<label>Búsqueda por</label>
								<asp:TextBox ID="txtCedulaBuscar" runat="server" placeholder="Cedula"></asp:TextBox>
								<asp:TextBox ID="txtNombreBuscar" runat="server" placeholder="Nombre"></asp:TextBox>
								<asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
							</div>
							<div class="form_left">
								<asp:Button ID="btnNuevo" runat="server" Text="Nuevo ingreso" />
							</div>
						</div>
						<asp:GridView ID="gvPersonas" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
							CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
							DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
							<PagerStyle CssClass="headerfooter ui-toolbar" />
							<SelectedRowStyle CssClass="SelectedRow" />
							<AlternatingRowStyle CssClass="odd" />
							<Columns>
								<asp:BoundField DataField="id" HeaderText="Identificacion" />
								<asp:BoundField DataField="Nombres" HeaderText="Nombres" />
								<asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
								<asp:BoundField DataField="FechaIngreso" HeaderText="FechaIngreso" DataFormatString="{0:dd/MM/yyyy}" />
								<asp:BoundField DataField="FechaNacimiento" HeaderText="FechaNacimiento" DataFormatString="{0:dd/MM/yyyy}" />
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
													Enabled='<%# IIF(gvPersonas.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
											</td>
											<td>
												<asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
													Enabled='<%# IIF(gvPersonas.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
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
					</div>
				</div>
				<div id="accordion1">
					<h3>
						<a href="#">Datos básicos
						</a>
					</h3>
					<div style="display: flex; flex-flow: wrap;">
						<div>
							<label>
								Identificación</label>
							<asp:HiddenField ID="hfID" runat="server" />
							<asp:TextBox ID="txtCedula" runat="server"></asp:TextBox>
							<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
								TargetControlID="txtCedula">
							</asp:FilteredTextBoxExtender>
						</div>
						<div>
							<label>
								Tipo identificación</label>
							<asp:DropDownList ID="ddlTipoIdentificacion" runat="server"></asp:DropDownList>
						</div>
						<div>
							<label>
								Lugar Expedicion</label>
							<asp:TextBox ID="txtLugarExpedicion" MaxLength="100" runat="server"></asp:TextBox>
						</div>
						<div>
							<label>
								Fecha expedición</label>
							<asp:TextBox ID="txtFechaExpedicion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
						</div>
						<div>
							<label>
								Apellidos</label>
							<asp:TextBox ID="txtApellidos" MaxLength="100" runat="server"></asp:TextBox>
						</div>
						<div>
							<label>
								Nombres</label>
							<asp:TextBox ID="txtNombres" MaxLength="100" runat="server"></asp:TextBox>
						</div>
						<div>
							<label>
								Nacionalidad</label>
							<asp:TextBox ID="txtNacionalidad" MaxLength="50" runat="server"></asp:TextBox>
						</div>
						<div>
							<label>
								Sexo</label>
							<asp:DropDownList ID="ddlSexo" runat="server"></asp:DropDownList>
						</div>
						<div>
							<label>
								Fecha Nacimiento</label>
							<asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
						</div>
						<div>
							<label>
								Estado Civil</label>
							<asp:DropDownList ID="ddlEstadoCivil" runat="server"></asp:DropDownList>
						</div>
						<div>
							<label>
								Nivel educativo</label>
							<asp:DropDownList ID="ddlNivelEducativo" runat="server"></asp:DropDownList>
						</div>
						<div>
							<label>
								Profesión</label>
							<asp:TextBox ID="txtProfesion" MaxLength="250" runat="server"></asp:TextBox>
						</div>
						<div>
							<label>
								Grupo Sanguíneo</label>
							<asp:DropDownList ID="ddlGrupoSanguineo" runat="server"></asp:DropDownList>
						</div>
					</div>
					<div id="accordion2">
						<h3>
							<a href="#">Datos de contacto</a>
						</h3>
						<div style="display: flex; flex-flow: wrap;">
							<div>
								<label>
									Ciudad Residencia</label>
								<asp:DropDownList ID="ddlCiudadResidencia" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									Barrio</label>
								<asp:TextBox ID="txtBarrio" MaxLength="100" runat="server"></asp:TextBox>
							</div>
							<div>
								<label>
									Direccion</label>
								<asp:TextBox ID="txtDireccion" MaxLength="250" runat="server"></asp:TextBox>
							</div>
							<div>
								<label>
									Teléfono 1</label>
								<asp:TextBox ID="txtTelefono1" MaxLength="50" runat="server"></asp:TextBox>
							</div>
							<div>
								<label>
									Teléfono 2</label>
								<asp:TextBox ID="txtTelefono2" MaxLength="50" runat="server"></asp:TextBox>
							</div>
							<div>
								<label>
									Celular</label>
								<asp:TextBox ID="txtCelular" MaxLength="50" runat="server"></asp:TextBox>
							</div>
							<div>
								<label>
									Email personal</label>
								<asp:TextBox ID="txtEmailPersonal" MaxLength="250" runat="server"></asp:TextBox>
							</div>
						</div>
					</div>
					<div id="accordion3">
						<h3>
							<a href="#">Datos de contratación</a>
						</h3>
						<div style="display: flex; flex-flow: wrap;">
							<div>
								<label>
									Estado actual</label>
								<asp:DropDownList ID="ddlEstadoActual" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									Fecha Ingreso</label>
								<asp:TextBox ID="txtFechaIngreso" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
							</div>
							<div>
								<label>
									Empresa</label>
								<asp:DropDownList ID="ddlEmpresa" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									Fecha Vencimiento Contrato</label>
								<asp:TextBox ID="txtFechaVencimientoContrato" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
							</div>
							<div>
								<label>
									Fecha retiro</label>
								<asp:TextBox ID="txtFechaRetiro" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
							</div>
							<div>
								<label>
									Motivo retiro</label>
								<asp:TextBox ID="txtMotivoRetiro" runat="server"></asp:TextBox>
							</div>
							<div>
								<label>
									Tipo Contrato</label>
								<asp:DropDownList ID="ddlTipoContrato" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									Tipo Salario</label>
								<asp:DropDownList ID="ddlTipoSalario" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									Forma Salario</label>
								<asp:DropDownList ID="ddlFormaSalario" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									Salario</label>
								<asp:TextBox ID="txtSalario" runat="server"></asp:TextBox>
								<asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
									TargetControlID="txtSalario">
								</asp:FilteredTextBoxExtender>
							</div>
							<div>
								<label>
									Banco</label>
								<asp:DropDownList ID="ddlBanco" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									Tipo Cuenta</label>
								<asp:DropDownList ID="ddlTipoCuenta" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									No. de Cuenta</label>
								<asp:TextBox ID="txtCuentaBanco" runat="server"></asp:TextBox>
							</div>

							<div>
								<label>
									Último Salario</label>
								<asp:TextBox ID="txtUltimoSalario" runat="server"></asp:TextBox>
							</div>
							<div>
								<label>
									Fecha Último Ascenso</label>
								<asp:TextBox ID="txtFechaUltimoAscenso" runat="server"></asp:TextBox>
							</div>
							<div>
								<label>
									Último cargo</label>
								<asp:DropDownList ID="ddlUltimoCargo" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									Fondo de Pensiones</label>
								<asp:DropDownList ID="ddlFondoPensiones" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									Fondo de Cesantías</label>
								<asp:DropDownList ID="ddlFondoCesantias" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									EPS</label>
								<asp:DropDownList ID="ddlEPS" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									Caja de Compensación</label>
								<asp:DropDownList ID="ddlCajaCompensacion" runat="server"></asp:DropDownList>
							</div>
							<div>
								<label>
									ARL</label>
								<asp:DropDownList ID="ddlARL" runat="server"></asp:DropDownList>
							</div>
						</div>
					</div>
					<div id="accordion4">
						<h3>
							<a href="#">Datos de posición</a>
						</h3>
						<div class="block">
							<div style="display: flex; flex-flow: wrap;">
								<div>
									<label>
										BU</label>
									<asp:DropDownList ID="ddlBU" runat="server" AutoPostBack="true"></asp:DropDownList>
								</div>
								<div>
									<label>
										Area</label>
									<asp:DropDownList ID="ddlArea" runat="server"></asp:DropDownList>
								</div>
								<div>
									<label>
										Sede</label>
									<asp:DropDownList ID="ddlSede" runat="server"></asp:DropDownList>
								</div>
								<div>
									<label>
										Jefe inmediato</label>
									<asp:DropDownList ID="ddlJefeInmediato" runat="server"></asp:DropDownList>
								</div>
								<div>
									<label>
										Cargo</label>
									<asp:DropDownList ID="ddlCargo" runat="server" AutoPostBack="true"></asp:DropDownList>
								</div>
								<div>
									<label>
										Tipo encuestador:
									</label>
									<asp:DropDownList ID="ddlTipoEncuestador" runat="server" CssClass="mySpecificClass dropdowntext"
										Enabled="False">
									</asp:DropDownList>
								</div>
								<div>
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
								</div>
								<div>
									<label>HeadCount</label>
									<asp:CheckBox ID="chbHeadCount" runat="server" />
								</div>
								<div>
									<label>¿Acceso a Matrix?</label>
									<asp:CheckBox ID="chbAccesoMatrix" runat="server" />
								</div>
								<div>
									<label>Usuario Matrix</label>
									<asp:TextBox ID="txtUsuarioMatrix" runat="server"></asp:TextBox>
								</div>
								<div>
									<label>Correo corporativo</label>
									<asp:TextBox ID="txtCorreoCorporativo" runat="server"></asp:TextBox>
								</div>
								<div class="actions">
									<asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
									<asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
								</div>
							</div>
						</div>
					</div>
					<div id="accordion5">
						<h3>
							<a href="#">Datos familiares</a>
						</h3>
					</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<script type="text/javascript">
		var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
		pageReqManger.add_initializeRequest(InitializeRequest);
		pageReqManger.add_endRequest(EndRequest);
	</script>
</asp:Content>
