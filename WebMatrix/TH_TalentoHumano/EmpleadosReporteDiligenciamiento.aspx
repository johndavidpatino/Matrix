<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRRHH.master" CodeBehind="EmpleadosReporteDiligenciamiento.aspx.vb" Inherits="WebMatrix.EmpleadosReporteDiligenciamiento" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
	<link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
	<link href="../css/TH_Empleados.css" rel="stylesheet">
	<script src="../Scripts/returnLoginFetchUnauthorized.js" type="text/javascript"></script>
	<style>
		tableRegistrosDiligenciamiento {
			height: 1000px;
			overflow-y: auto;
		}

		th {
			position: sticky;
			top: 0;
		}
	</style>
	<script>

		function loadImage(input) {
			let img = document.querySelector('.containerForms img');
			loadImageFromInputFileToTagImage(input, img);
		}

		function drawReport(registrosDiligenciamiento) {
			let tableRegistrosDiligenciamiento = document.getElementsByClassName('tableRegistrosDiligenciamiento')[0];
			let rawHtmlTableHeader = '';
			let rawHtmlTableBody = '';

			rawHtmlTableHeader = `<table>
										<thead>
											<tr>
												<th>Identificación</th>
												<th>Nombres y Apellidos</th>
												<th>ServiceLine / Area</th>
												<th>Correo Ipsos</th>
												<th>Experiencia Laboral</th>
												<th>Educación</th>
												<th>Contactos Emergencia</th>
												<th>Historico Posiciones</th>
												<th>Salarios</th>
												<th>Datos Laborales</th>
												<th>Datos Personales</th>
												<th>Ingles</th>
												<th>Nomina</th>
												<th>% Diligenciamiento</th>
											</tr>
										</thead>
										<tbody>
									`;

			registrosDiligenciamiento.forEach((element, index) => {
				rawHtmlTableBody += `
								<tr>
									<td>
										${element.personaId}
									</td>
									<td>
										${element.Nombres + ' ' + element.Apellidos}
									</td>
									<td>
										${element.AreaTxt == null ? `` : element.AreaTxt}
									</td>
									<td>
										${element.correoIpsos == null ? `` : element.correoIpsos}
									</td>
									<td>
										${element.ExperienciaLaboral == true ? `&#128267` : `&#128308`}
									</td>
									<td>
										${element.Educacion == true ? `&#128267` : `&#128308`}
									</td>
									<td>
										${element.ContactoEmergencia == true ? `&#128267` : `&#128308`}
									</td>
									<td>
										${element.HistoricoPosiciones == true ? `&#128267` : `&#128308`}
									</td>
									<td>
										${element.Salarios == true ? `&#128267` : `&#128308`}
									</td>
									<td>
										${element.DatosLaborales == true ? `&#128267` : `&#128308`}
									</td>
									<td>
										${element.DatosPersonales == true ? `&#128267` : `&#128308`}
									</td>
									<td>
										${element.Ingles == true ? `&#128267` : `&#128308`}
									</td>
									<td>
										${element.Nomina == true ? `&#128267` : `&#128308`}
									</td>
									<td>
										${element.PorcentajeDiligenciamiento}%
									</td>
								</tr>
								`;
			});
			rawHtmlTableBody += `</tbody></table>`;
			tableRegistrosDiligenciamiento.insertAdjacentHTML("beforeend", rawHtmlTableHeader + rawHtmlTableBody);
		}
		function getReporteDiligenciamiento() {
			return fetch('EmpleadosReporteDiligenciamiento.aspx/getReporteDiligenciamiento', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d });
		}

		function formatDateToColombiaDate(dateToFormat) {
			return dateToFormat.format('dd/MM/yyyy');
		}
		function formatDateToyyyyMMdd(dateToFormat) {
			return dateToFormat.format('yyyy-MM-dd');
		}
		function calculateAgeFromBirthdate(birthdate) {
			let yearBirthday = birthdate.getFullYear();
			let yearNow = new Date().getFullYear();

			return yearNow - yearBirthday;

		}
		function getDateFromMilliseconds(milliseconds) {
			return new Date(parseInt(milliseconds.substr(6)));
		}
		function formatNumberToColombiaCurrency(number) {
			var currency = new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP' });
			return currency.format(number);
		}
		function formatMillisecondsToColombiaDate(milliseconds) {
			let date = getDateFromMilliseconds(milliseconds);
			return formatDateToColombiaDate(date);
		}
		function formatMillisecondsToyyyyMMdd(milliseconds) {
			let date = getDateFromMilliseconds(milliseconds);
			return formatDateToyyyyMMdd(date);
		}

	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Menu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_BreadCumbs" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_TituloGeneral" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Titulo" runat="server">
	Empleados
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
	Reporte de actualización de información de los empleados
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
	<div class="tableRegistrosDiligenciamiento">
	</div>
	<script>
		(
			async () => {
				let registrosDiligenciamiento;
				registrosDiligenciamiento = await getReporteDiligenciamiento();
				drawReport(registrosDiligenciamiento);
			}
		)();
	</script>
</asp:Content>
