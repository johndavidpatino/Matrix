<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterREP.master" CodeBehind="ReportesVariablesControl.aspx.vb" Inherits="WebMatrix.ReportesVariablesControl" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
	<script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
	<script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
	<script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>

	<link rel="stylesheet" href="https://blacklabel.github.io/grouped_categories/css/styles.css" type="text/css" />
	<script src="https://code.highcharts.com/10.0/highcharts.js" type="text/javascript" language="javascript"></script>
	<script src="https://blacklabel.github.io/grouped_categories/grouped-categories.js" type="text/javascript" language="javascript"></script>
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
	Indicadores de variables de control
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
	Indicadores que muestran información acerca de las evaluaciones realizadas a las variables de control
    <br />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Content" runat="server">
	<script type="text/javascript">
		Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endReq);
		function endReq(sender, args) {
			$('#tabs').tabs();

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

	<asp:Label ID="lblAno" Text="Año" runat="server" AssociatedControlID="ddlAno"></asp:Label>
	<asp:DropDownList ID="ddlAno" runat="server">
	</asp:DropDownList>
	<asp:Label ID="lblMes" Text="Mes" runat="server" AssociatedControlID="ddlMes"></asp:Label>
	<asp:DropDownList ID="ddlMes" runat="server">
		<asp:ListItem Text="--Seleccione--" Value="-1"></asp:ListItem>
		<asp:ListItem Text="Enero" Value="1"></asp:ListItem>
		<asp:ListItem Text="Febrero" Value="2"></asp:ListItem>
		<asp:ListItem Text="Marzo" Value="3"></asp:ListItem>
		<asp:ListItem Text="Abril" Value="4"></asp:ListItem>
		<asp:ListItem Text="Mayo" Value="5"></asp:ListItem>
		<asp:ListItem Text="Junio" Value="6"></asp:ListItem>
		<asp:ListItem Text="Julio" Value="7"></asp:ListItem>
		<asp:ListItem Text="Agosto" Value="8"></asp:ListItem>
		<asp:ListItem Text="Septiemnbre" Value="9"></asp:ListItem>
		<asp:ListItem Text="Octubre" Value="10"></asp:ListItem>
		<asp:ListItem Text="Noviembre" Value="11"></asp:ListItem>
		<asp:ListItem Text="Diciembre" Value="12"></asp:ListItem>
	</asp:DropDownList>
	<asp:Label ID="lblEmpleadosConEvaluacion" Text="Evaluados" runat="server" AssociatedControlID="ddlEmpleadosConEvaluacion"></asp:Label>
	<asp:DropDownList ID="ddlEmpleadosConEvaluacion" runat="server">
	</asp:DropDownList>
	<asp:UpdatePanel runat="server" ID="upActualizar" UpdateMode="Conditional">
		<ContentTemplate>
			<asp:Button ID="btnAcualizar" runat="server" Text="Actualizar" />
		</ContentTemplate>
	</asp:UpdatePanel>
	<div style="clear: both;"></div>

	<br />
	<div style="margin-top: 30px">
		<div id="accordion1">
			<h2>Variables de control</h2>
			<div style="max-width: 100%; min-height: 300px;">
				<asp:Button Text="Exportar a Excel" runat="server" ID="btnExportarExcelVariablesControl" />
				<br />
				<asp:UpdatePanel runat="server" ID="upVariablesControl" UpdateMode="Conditional">
					<ContentTemplate>
						<asp:GridView ID="gvVariablesControl" runat="server" AutoGenerateColumns="true"></asp:GridView>
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>
		</div>

		<br />
		<div id="accordion2">
			<h2>Variables de control por mes</h2>
			<div style="max-width: 100%; min-height: 500px; max-height: 500px;">
				<asp:Button Text="Exportar a Excel" runat="server" ID="btnExportarExcelVariablesControlPorMes" />
				<br />
				<asp:UpdatePanel runat="server" ID="upVariablesControlPorMes" UpdateMode="Conditional">
					<ContentTemplate>
						<asp:GridView ID="gvVariablesControlPorMes" runat="server" AutoGenerateColumns="true"></asp:GridView>
					</ContentTemplate>
				</asp:UpdatePanel>
			</div>
		</div>

	</div>
</asp:Content>
