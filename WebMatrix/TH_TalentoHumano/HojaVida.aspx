<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRRHH.master"
	CodeBehind="HojaVida.aspx.vb" Inherits="WebMatrix.HojaVida" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
	TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
	<script type="text/javascript">
		$(document).ready(function () {
			loadPlugins();
		});

		function fuFotoPersona() {
			__doPostBack('fuFotoPersona', '');
			return (true);
		}

		function loadPlugins() {

			validationForm();

			$("#<%= calFechaNacimiento.ClientId %>").mask("99/99/9999");
			$("#<%= calFechaNacimiento.ClientId %>").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});

			$("#<%= calInicioEd.ClientId %>").mask("99/99/9999");
			$("#<%= calInicioEd.ClientId %>").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});

			$("#<%= calFinalizacionEd.ClientId %>").mask("99/99/9999");
			$("#<%= calFinalizacionEd.ClientId %>").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});

			$("#<%= calInicioExp.ClientId %>").mask("99/99/9999");
			$("#<%= calInicioExp.ClientId %>").datepicker({
				dateFormat: 'dd/mm/yy',
				changeMonth: true,
				changeYear: true,
				dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
				monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
				monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
			});

			$("#<%= calFinalizacionExp.ClientId %>").mask("99/99/9999");
			$("#<%= calFinalizacionExp.ClientId %>").datepicker({
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
				autoHeight: false,
				collapsible: true
			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Menu" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
	Tus datos
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Content" runat="server">
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
	<asp:UpdatePanel runat="server" ID="upDatos">
		<ContentTemplate>
			<div id="accordion">
				<div id="accordion0">
					<h3>
						<a href="#">Datos generales
						</a>
					</h3>
					<div class="block">
						<fieldset class="validationgroup">
							<div>
								<div class="actions">
									<asp:Image ID="imgPersona" runat="server" Height="104px" ImageUrl="~/Images/sin-foto.jpg"
										Width="104px" />
									<label>
										Subir foto:</label>
									<asp:FileUpload ID="fuFotoPersona" runat="server" Text="CargarArchivo" CssClass="required text textEntry" />
								</div>
								<div style="display: flex; flex-flow: wrap;">
									<div>
										<label>
											Tipo de documento:</label>
										<asp:DropDownList ID="ddTipoDocumento" runat="server">
										</asp:DropDownList>
									</div>
									<div>
										<label>
											No Documento:</label>
										<asp:TextBox ID="txtNoDocumento" Enabled="false" runat="server"></asp:TextBox>
									</div>
									<div>
										<label>
											Fecha de nacimiento:</label>
										<asp:TextBox ID="calFechaNacimiento" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
									</div>
									<div>
										<label>
											Nombres:</label>
										<asp:TextBox ID="txtNombres" runat="server"></asp:TextBox>
									</div>
									<div>
										<label>
											Apellidos:</label>
										<asp:TextBox ID="txtApellidos" runat="server"></asp:TextBox>
									</div>
									<div>
										<label>
											Email:</label>
										<asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
									</div>
									<div>
										<label>
											Genero:</label>
										<asp:DropDownList ID="ddlGenero" runat="server">
										</asp:DropDownList>
									</div>
									<div>
										<label>
											Estado civil:</label>
										<asp:DropDownList ID="ddlEstadoCivil" runat="server">
										</asp:DropDownList>
									</div>
									<div>
										<label>
											Pais de nacimiento:</label>
										<asp:DropDownList ID="ddlPaisNacimiento" runat="server" AutoPostBack="True">
										</asp:DropDownList>
									</div>
								</div>
								<div class="actions">
									<div class="form_right">
										<asp:Button ID="btnSave1" runat="server" Text="Guardar y continuar" />
									</div>
								</div>
							</div>
						</fieldset>
					</div>
					<%--items--%>
				</div>
				<div id="accordion1">
					<h3>
						<a href="#">Datos de Contacto
						</a>
					</h3>
					<div class="block">
						<fieldset class="validationgroup">
							<div style="display: flex; flex-flow: wrap;">
								<div>
									<label>
										País de residencia:</label>
									<asp:DropDownList ID="ddlPaisResidencia" runat="server" AutoPostBack="true">
									</asp:DropDownList>
								</div>
								<div>
									<label>
										Departamento de residencia:</label>
									<asp:DropDownList ID="ddlDepartamentoResidencia" runat="server" AutoPostBack="true">
									</asp:DropDownList>
								</div>
								<div>
									<label>
										Ciudad de residencia:</label>
									<asp:DropDownList ID="ddlCiudadResidencia" runat="server">
									</asp:DropDownList>
								</div>
								<div>
									<label>
										Dirección:</label>
									<asp:TextBox ID="txtDireccion" runat="server"></asp:TextBox>
								</div>
								<div>
									<label>
										Telefono:</label>
									<asp:TextBox ID="txtTelefono" runat="server"></asp:TextBox>
								</div>
								<div>
									<label>
										Celular:</label>
									<asp:TextBox ID="txtCelular" runat="server"></asp:TextBox>
								</div>
								<div>
									<label>
										Telefono Oficina:</label>
									<asp:TextBox ID="txtTelefonoOficina" runat="server"></asp:TextBox>
								</div>
								<div>
									<label>
										Extensión:</label>
									<asp:TextBox ID="txtExtension" runat="server"></asp:TextBox>
								</div>
								<div>
									<label>
										Profesión:</label>
									<asp:TextBox ID="txtProfesion" runat="server"></asp:TextBox>
									<asp:AutoCompleteExtender ID="AutoCompleteExtender1" TargetControlID="txtProfesion"
										runat="server" ServiceMethod="GetCompletionList" UseContextKey="True" />
								</div>
							</div>
							<div class="actions">
								<asp:Button ID="btnSave2" runat="server" Text="Guardar y continuar" />
							</div>
						</fieldset>
					</div>
				</div>
				<div id="accordion2">
					<h3>
						<a href="#">Educación
						</a>
					</h3>
					<div class="block">
						<fieldset>
							<div class="actions" id="DvEducacion" runat="server" visible="true">
								<div style="display: flex; flex-wrap: wrap;">
									<div>
										<label>
											Nivel de estudio:</label>
										<asp:DropDownList ID="ddlNivelEstudioEd" runat="server">
										</asp:DropDownList>
									</div>
									<div>
										<label>
											Titulo:</label>
										<asp:TextBox ID="txtTituloEd" runat="server"></asp:TextBox>
									</div>
									<div>
										<label>
											Institución</label>
										<asp:TextBox ID="txtInstitucionEd" runat="server"></asp:TextBox>
									</div>
									<div>
										<label>
											País</label>
										<asp:DropDownList ID="ddlPaisEd" runat="server">
										</asp:DropDownList>
									</div>
									<div>
										<label>
											Departamento</label>
										<asp:DropDownList ID="ddlDepartamentoEd" runat="server" AutoPostBack="True">
										</asp:DropDownList>
									</div>
									<div>
										<label>
											Ciudad</label>
										<asp:DropDownList ID="ddlCiudadEd" runat="server">
										</asp:DropDownList>
									</div>
									<div>
										<label>
											Inicio</label>
										<asp:TextBox ID="calInicioEd" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
									</div>
									<div>
										<label>
											Finalización</label>
										<asp:TextBox ID="calFinalizacionEd" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
									</div>
									<div>
										<label>
											Estado:</label>
										<asp:DropDownList ID="ddlEstadoEd" runat="server">
										</asp:DropDownList>
									</div>
									<div class="actions">
										<div class="form_left">
											<asp:Button ID="btnAddEducacion" runat="server" Text="Agregar" />
										</div>
									</div>
								</div>
								<label style="background-color: cadetblue; width: 100%; text-align: left; color: white;">
									Historico
								</label>
								<asp:GridView ID="gvEducacion" runat="server" AlternatingRowStyle-CssClass="odd"
									AutoGenerateColumns="false" CssClass="displayTable" DataKeyNames="Id,HojaVidaId,NivelEstudioId,NivelEstudio,Titulo,Institucion,PaisId,CiudadId,Inicio,Finalizacion,EstadoEducacion"
									EmptyDataText="No existen registros para mostrar" PagerStyle-CssClass="headerfooter ui-toolbar">
									<PagerStyle CssClass="headerfooter ui-toolbar" />
									<SelectedRowStyle CssClass="SelectedRow" />
									<AlternatingRowStyle CssClass="odd" />
									<Columns>
										<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
										<asp:BoundField DataField="NivelEstudio" HeaderText="Nivel de estudio" SortExpression="NivelEstudio" />
										<asp:BoundField DataField="Titulo" HeaderText="Titulo" SortExpression="Titulo" />
										<asp:BoundField DataField="Institucion" HeaderText="Institución" SortExpression="Institucion" />
										<asp:BoundField DataField="EstadoEducacion" HeaderText="Estado" SortExpression="EstadoEducacion" />
										<asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
											<ItemTemplate>
												<asp:ImageButton ID="imgEducacion" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
													CommandName="eliminar" ImageUrl="~/Images/delete_16.png" Text="eliminar" ToolTip="Eliminar"
													OnClientClick="return confirm('Esta seguro que desea eliminar este estudio?');" />
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Center" />
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Editar" ShowHeader="False">
											<ItemTemplate>
												<asp:ImageButton ID="imgEdtEducacion" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
													CommandName="Editar" ImageUrl="~/Images/Select_16.png" Text="Editar" ToolTip="Editar" />
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Center" />
										</asp:TemplateField>
									</Columns>
								</asp:GridView>
							</div>
							<div class="actions">
								<asp:Button ID="btnSave3" runat="server" Text="Continuar" />
							</div>
						</fieldset>
					</div>
				</div>
				<div id="accordion3">
					<h3>
						<a href="#">Idiomas
						</a>
					</h3>
					<div class="block">
						<fieldset>
							<div>
								<div class="form_left">
									<label>
										Idioma nativo:</label>
									<asp:DropDownList ID="ddlIdioma" runat="server">
									</asp:DropDownList>
								</div>
								<div class="actions" id="DvIdioma" runat="server" visible="true" style="display: flex; flex-wrap: wrap;">
									<div>
										<label>
											Dominio:</label>
										<asp:DropDownList ID="ddlDominioIdm" runat="server">
										</asp:DropDownList>
									</div>
									<div>
										<label>
											Idioma:</label>
										<asp:DropDownList ID="ddlIdiomaIdm" runat="server">
										</asp:DropDownList>
									</div>
									<div>
										<label>
											Lugar:</label>
										<asp:TextBox ID="txtLugarIdm" runat="server"></asp:TextBox>
									</div>
									<div>
										<asp:Button ID="btnAddIdioma" runat="server" Text="Agregar" />
									</div>
								</div>
								<asp:GridView ID="gvIdiomas" runat="server" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
									CssClass="displayTable" DataKeyNames="Id,DominioId,Dominio,Lugar,IdiomaId,Idioma"
									EmptyDataText="No existen registros para mostrar" PagerStyle-CssClass="headerfooter ui-toolbar">
									<PagerStyle CssClass="headerfooter ui-toolbar" />
									<SelectedRowStyle CssClass="SelectedRow" />
									<AlternatingRowStyle CssClass="odd" />
									<Columns>
										<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
										<asp:BoundField DataField="Lugar" HeaderText="Lugar" SortExpression="Lugar" />
										<asp:BoundField DataField="Idioma" HeaderText="Idioma" SortExpression="Idioma" />
										<asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
											<ItemTemplate>
												<asp:ImageButton ID="imgIdioma" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
													CommandName="eliminar" ImageUrl="~/Images/delete_16.png" Text="eliminar" ToolTip="Eliminar"
													OnClientClick="return confirm('Esta seguro que desea eliminar este estudio?');" />
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Center" />
										</asp:TemplateField>
										<asp:TemplateField HeaderText="Editar" ShowHeader="False">
											<ItemTemplate>
												<asp:ImageButton ID="imgEdtIdioma" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
													CommandName="Editar" ImageUrl="~/Images/Select_16.png" Text="Editar" ToolTip="Editar" />
											</ItemTemplate>
											<ItemStyle HorizontalAlign="Center" />
										</asp:TemplateField>
									</Columns>
								</asp:GridView>
							</div>
							<div class="actions">
								<asp:Button ID="btnSave4" runat="server" Text="Guardar y Continuar" />
							</div>
						</fieldset>
					</div>
				</div>
				<div id="accordion4">
					<h3>
						<a href="#">Experiencia
						</a>
					</h3>
					<div class="block">
						<fieldset>
							<div class="actions" id="dvExperiencia" runat="server" visible="true" style="display: flex; flex-wrap: wrap;">
								<div>
									<label>
										Empresa:</label>
									<asp:TextBox ID="txtEmpresaExp" runat="server"></asp:TextBox>
								</div>
								<div>
									<label>
										Telefono:</label>
									<asp:TextBox ID="txtTelefonoExp" runat="server"></asp:TextBox>
								</div>
								<div>
									<label>
										Actualmente:</label>
									<asp:CheckBox ID="chkActualmenteExp" runat="server" />
								</div>
								<div>
									<label>
										Inicio:</label>
									<asp:TextBox ID="calInicioExp" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
								</div>
								<div>
									<label>
										Finalización:</label>
									<asp:TextBox ID="calFinalizacionExp" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
								</div>
								<div>
									<label>
										Cargo:</label>
									<asp:DropDownList ID="ddlCargoExp" runat="server">
									</asp:DropDownList>
								</div>
								<div>
									<label>
										Nivel cargo:</label>
									<asp:DropDownList ID="ddlNivelCargoExp" runat="server">
									</asp:DropDownList>
								</div>
								<div>
									<label>
										País:</label>
									<asp:DropDownList ID="ddlPaisExp" runat="server" AutoPostBack="True">
									</asp:DropDownList>
								</div>
								<div>
									<label>
										Departamento:</label>
									<asp:DropDownList ID="ddlDepartamentoExp" runat="server" AutoPostBack="True">
									</asp:DropDownList>
								</div>
								<div>
									<label>
										Ciudad:</label>
									<asp:DropDownList ID="ddlCiudadExp" runat="server">
									</asp:DropDownList>
								</div>
								<div>
									<label>
										Dirección:</label>
									<asp:TextBox ID="txtDireccionExp" runat="server"></asp:TextBox>
								</div>
								<div class="actions">
									<asp:Button ID="btnAddExperiencia" runat="server" Text="Agregar" />
								</div>
							</div>
						</fieldset>
						<asp:GridView ID="gvExperiencia" runat="server" AlternatingRowStyle-CssClass="odd"
							AutoGenerateColumns="false" CssClass="displayTable" DataKeyNames="Id,Empresa,Telefono,Inicio,Finalizacion,Actualmente,CargoId,Cargo,NivelCargoId,NivelCargo,PaisId,CiudadId,Direccion"
							EmptyDataText="No existen registros para mostrar" PagerStyle-CssClass="headerfooter ui-toolbar">
							<PagerStyle CssClass="headerfooter ui-toolbar" />
							<SelectedRowStyle CssClass="SelectedRow" />
							<AlternatingRowStyle CssClass="odd" />
							<Columns>
								<asp:BoundField DataField="Id" HeaderText="Id" SortExpression="Id" Visible="false" />
								<asp:BoundField DataField="Empresa" HeaderText="Empresa" SortExpression="Empresa" />
								<asp:BoundField DataField="Inicio" HeaderText="Inicio" SortExpression="Inicio" />
								<asp:BoundField DataField="Finalizacion" HeaderText="Finalizacion" SortExpression="Finalizacion" />
								<asp:BoundField DataField="Cargo" HeaderText="Cargo" SortExpression="Cargo" />
								<asp:BoundField DataField="Direccion" HeaderText="Dirección" SortExpression="Direccion" />
								<asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
									<ItemTemplate>
										<asp:ImageButton ID="imgExp" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
											CommandName="eliminar" ImageUrl="~/Images/delete_16.png" Text="eliminar" ToolTip="Eliminar"
											OnClientClick="return confirm('Esta seguro que desea eliminar esta experiencia?');" />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>
								<asp:TemplateField HeaderText="Editar" ShowHeader="False">
									<ItemTemplate>
										<asp:ImageButton ID="imgEdtExp" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
											CommandName="Editar" ImageUrl="~/Images/Select_16.png" Text="Editar" ToolTip="Editar" />
									</ItemTemplate>
									<ItemStyle HorizontalAlign="Center" />
								</asp:TemplateField>
							</Columns>
						</asp:GridView>
						<div class="actions">
							<asp:Button ID="btnSave5" runat="server" Text="Continuar" />
						</div>
					</div>
				</div>
				<div id="accordion5">
					<h3>
						<a href="#">Otra información
						</a>
					</h3>
					<div class="block">
						<fieldset>
							<div style="display: flex; flex-wrap: wrap;">
								<div>
									<label>
										Años de experiencia:</label>
									<asp:DropDownList ID="ddlAnoExperiencia" runat="server">
									</asp:DropDownList>
								</div>
								<div style="visibility: hidden">
									<label>
										Trabaja actualmente:</label>
									<asp:CheckBox ID="chkTrabaja" runat="server" Visible="false" />
								</div>
								<div>
									<label>
										Posibilidades para viajar:</label>
									<asp:CheckBox ID="chkViajar" runat="server" />
								</div>
							</div>
							<div class="actions">
								<div>
									<asp:Button ID="btnSave6" runat="server" Text="Guardar y continuar" />
								</div>
							</div>
						</fieldset>
					</div>
				</div>
				<div id="accordion6">
					<h3>
						<a href="#">Hoja de Vida
						</a>
					</h3>
					<div class="block">
						<fieldset>
							<div class="actions">
								<label>
									Perfil profesional</label>
								<asp:TextBox ID="txtPerfilProfesional" Width="100%" TextMode="MultiLine" Height="200px"
									runat="server" />
								<br />
							</div>
							<div class="actions">
								<label>
									Texto de la hoja de vida</label>
								<cc1:Editor ID="txtTextoHV" NoUnicode="true" Width="100%" Height="200px" runat="server" />
								<br />
							</div>
							<div class="actions" style="visibility: hidden;">
								<label style="visibility: hidden;">
									Texto de la hoja de vida (Ingles)</label>
								<asp:TextBox ID="txtTextoHVIngles" Width="100%" Visible="false" Height="100px" runat="server" />
								<br />
							</div>
							<div class="actions">
								<asp:Button ID="btnSave7" runat="server" Text="Guardar y continuar" />
							</div>
						</fieldset>
					</div>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<script type="text/javascript">
		var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
		pageReqManger.add_initializeRequest(InitializeRequest);
		pageReqManger.add_endRequest(EndRequest);
	</script>
</asp:Content>
