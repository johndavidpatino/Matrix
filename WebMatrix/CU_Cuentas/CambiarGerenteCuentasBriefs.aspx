<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/CU_F.master"
	CodeBehind="CambiarGerenteCuentasBriefs.aspx.vb" Inherits="WebMatrix.CambiarGerenteCuentasBriefs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
	TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
	<link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
	<script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
	<script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
	<script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>

	<script type="text/javascript">
		$(document).ready(function () {

			$('#GerenteCuentasAsignar').dialog(
			{
				modal: true,
				autoOpen: false,
				title: "Seleccione el Gerente de Cuentas",
				width: "600px",
				closeOnEscape: true,
				open: function (type, data) {
					$(this).parent().appendTo("form");

				}
			});


			loadPlugins();
		});

		function MostrarGerentesCuentas() {
			$('#GerenteCuentasAsignar').dialog("open");
		}

		function loadPlugins() {

			validationForm();
			
			$('#accordion').accordion({
				change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
				header: "h3",
				autoHeight: false
			});
		}



		
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
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
	<asp:UpdatePanel ID="UpdatePanel1" runat="server">
		<ContentTemplate>
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
					<h3>
						<a href="#">
							<label>
								Lista de Briefs</label>
						</a>
					</h3>
					<asp:HiddenField ID="hfidbrief" runat="server" value="0" />
					<asp:HiddenField ID="hfidGrupoUnidad" runat="server" Value="0" />
                    <asp:HiddenField ID="hfGerenteAnterior" runat="server" Value="0" />
					<div class="block">
						<div class="form_left">
							<fieldset>
								<label>
									Palabra a Buscar</label>
								<asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
								<asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
							</fieldset>
						</div>
						
						<div class="actions">
						</div>
						<asp:GridView ID="gvDatos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
							CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
							DataKeyNames="Id,GerenteCuentas" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
							<PagerStyle CssClass="headerfooter ui-toolbar" />
							<SelectedRowStyle CssClass="SelectedRow" />
							<AlternatingRowStyle CssClass="odd" />
							<Columns>
								<asp:BoundField DataField="Id" HeaderText="No. Brief" />
								<asp:BoundField DataField="Titulo" HeaderText="Titulo" />
								<asp:BoundField DataField="RazonSocial" HeaderText="Cliente" />
								<asp:BoundField DataField="Nombre" HeaderText="Contacto" />
								<asp:BoundField DataField="NombreBrief" HeaderText="Tipo Brief" />
								<asp:BoundField DataField="NombreGerenteCuentas" HeaderText="Gerente Cuentas" />
								<asp:TemplateField HeaderText="Cambiar Gerente" ShowHeader="False">
									<ItemTemplate>
										<asp:ImageButton ID="ImageButton1" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
											CommandName="Modificar" ImageUrl="~/Images/cliente.jpg" Text="Seleccionar"
											ToolTip="Cambiar Gerente" OnClientClick="MostrarGerentesCuentas()" />
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
													Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
											</td>
											<td>
												<asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
													Enabled='<%# IIF(gvDatos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
											</td>
											<td>
												<span class="pagingLinks">[<%= gvDatos.PageIndex + 1%>-
													<%= gvDatos.PageCount%>]</span>
											</td>
											<td>
												<asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
													Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
													SkinID="paging">Siguiente &gt;</asp:LinkButton>
											</td>
											<td>
												<asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
													Enabled='<%# IIF((gvDatos.PageIndex +1) = gvDatos.PageCount, "false", "true") %>'
													SkinID="paging">Ultimo »</asp:LinkButton>
											</td>
										</tr>
									</table>
								</div>
							</PagerTemplate>
						</asp:GridView>
					</div>
				</div>
			</div>
		</ContentTemplate>
	</asp:UpdatePanel>
	<div id="GerenteCuentasAsignar">
		<asp:UpdatePanel ID="upGerenteCuentasAsignar" runat="server" ChildrenAsTriggers="false"
			UpdateMode="Conditional">
			<ContentTemplate>
				<div class="form_left">
					<label>Seleccione el Gerente de Cuentas a asignar</label>
					<asp:DropDownList ID="ddlGerente" runat="server"></asp:DropDownList>
					<asp:Button ID="btnUpdate" runat="server" Text="Asignar Gerente"  OnClientClick="$('#GerenteCuentasAsignar').dialog('close');" />
				</div>
			</ContentTemplate>
		</asp:UpdatePanel>
	</div>
	<script type="text/javascript">
		var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
		pageReqManger.add_initializeRequest(InitializeRequest);
		pageReqManger.add_endRequest(EndRequest);
	</script>
</asp:Content>
