<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRRHH.master" CodeBehind="EnCasoEmergencia.aspx.vb" Inherits="WebMatrix.EnCasoEmergencia" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
	<link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
	<link href="../css/TH_Empleados.css" rel="stylesheet">
	<script src="../Scripts/returnLoginFetchUnauthorized.js" type="text/javascript"></script>

	<style>
		.containerSegments {
			display: flex;
		}
		.cardInfoHealthProvider{
			width:200px
		}
	</style>

	<script>
		function maximizeMinimizeSearch() {
			document.getElementsByClassName('filters')[0].classList.toggle('maximize');
		}
		function loadImage(input) {
			let img = document.querySelector('.containerForms img');
			loadImageFromInputFileToTagImage(input, img);
		}

		function getEmpleados() {

			clearPersons();

			let formData = new Object();
			formData.nombres = document.getElementById('filterNombres').value == "" ? null : document.getElementById('filterNombres').value;
			formData.apellidos = document.getElementById('filterApellidos').value == "" ? null : formData.apellidos = document.getElementById('filterApellidos').value;
			formData.areaServiceLine = document.getElementById('filterAreaServiceLine').value == "" ? null : document.getElementById('filterAreaServiceLine').value;
			formData.cargo = document.getElementById('filterCargo').value == "" ? null : document.getElementById('filterCargo').value;
			formData.sede = document.getElementById('filterSede').value == "" ? null : document.getElementById('filterSede').value;

			fetch('EnCasoEmergencia.aspx/getDatosEmergenciaEmpleados', {
				method: 'POST',
				body: JSON.stringify(formData),
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => drawEmpleados(data.d));
		}
		function getAreasServicesLine() {
			return fetch('EmpleadosAdmin.aspx/getAreasServiceLines', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d });
		}
		function getSedes() {
			return fetch('EmpleadosAdmin.aspx/getSedes', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d });
		}
		function getCargos() {
			return fetch('EmpleadosAdmin.aspx/getCargos', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d });
		}
		function getContactosEmergencia(identificacion) {
			let formData = new Object();

			formData.identificacion = identificacion;

			return fetch('EmpleadosAdmin.aspx/getContactosEmergenciaPorIdentificacion', {
				method: 'POST',
				body: JSON.stringify(formData),
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d });
		}

		function drawEmpleados(persons) {
			let containerCardsPerson = document.getElementsByClassName('containerCardsPerson')[0];
			persons.forEach((element, index) => {
				let rawHtml = `<div class="cardPerson">
									<div>
										<img src="${element.urlFoto ? element.urlFoto : '../Images/sin-foto.jpg'}" alt="" />
									</div>
									<div class="info">
										<div>
											<label>${element.Nombres}</label>
										</div>
										<div>
											<label>${element.Apellidos}</label>
										</div>
										<div>
											<i class="material-icons" title="Grupo sanguineo">local_hospital</i>
											<span> ${element.GrupoSanguineoTxt}</span>
										</div>
										<div>
											<i class="material-icons" title="Sede">person_pin_circle</i>
											<span> ${element.SedeTxt}</span>
										</div>
										<div>
											<i class="material-icons" title="Service line / Area">my_location</i>
											<span> ${element.AreaTxt}</span>
										</div>
									</div>
								</div>`;
				containerCardsPerson.insertAdjacentHTML("beforeend", rawHtml);
			});
		}
		function getDateFromMilliseconds(milliseconds) {
			return new Date(parseInt(milliseconds.substr(6)));
		}
		function formatMillisecondsToColombiaDate(milliseconds) {
			let date = getDateFromMilliseconds(milliseconds);
			return formatDateToColombiaDate(date);
		}
		function formatDateToColombiaDate(dateToFormat) {
			return dateToFormat.format('dd/MM/yyyy');
		}

		function clearPersons() {
			let cardsPersons = Array.from(document.getElementsByClassName('cardPerson'));
			cardsPersons.forEach(element => element.remove());
		}

		function fillSelectFilterAreasServicesLines(areasServicesLines) {
			let select = document.getElementById('filterAreaServiceLine');

			areasServicesLines.forEach((element, index) => {
				let option = document.createElement('option');
				option.value = element.id;
				option.text = element.Area;
				select.add(option);
			});

		}
		function fillSelectFilterCargos(cargos) {
			let select = document.getElementById('filterCargo');

			cargos.forEach((element, index) => {
				let option = document.createElement('option');
				option.value = element.id;
				option.text = element.Cargo;
				select.add(option);
			});

		}
		function fillSelectFilterSedes(sedes) {
			let select = document.getElementById('filterSede');

			sedes.forEach((element, index) => {
				let option = document.createElement('option');
				option.value = element.id;
				option.text = element.Sede;
				select.add(option);
			});

		}
		async function setupForms() {
			let areasServiceLines;
			let cargos;
			let sedes;

			areasServiceLines = await getAreasServicesLine();
			sedes = await getSedes();
			cargos = await getCargos();

			fillSelectFilterAreasServicesLines(areasServiceLines);
			fillSelectFilterCargos(cargos);
			fillSelectFilterSedes(sedes);
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
	En caso de emergencia
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
	Información para casos de emergencia
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
	<div class="containerSegments">
		<div class="infoGeneralEmergency">
			<h3>Información entidades</h3>
			<div class="cardInfoHealthProvider">
				Emermedica
				3077087 Opción 1
				Nit Ipsos Colombia: 890.319.494
			</div>
		</div>
		<div class="infoEmployees">
			<div class="filters maximize">
				<div>
					<i class="material-icons maximizeMinimizeIcon" title="Filtros">details</i>
					<h4>Filtros:</h4>
				</div>
				<div class="formFilters" style="display: flex; flex-wrap: wrap; justify-content: space-evenly; align-items: center;">
					<div>
						<label>Nombres</label>
						<input type="text" id="filterNombres" />
					</div>
					<div>
						<label>Apellidos</label>
						<input type="text" id="filterApellidos" />
					</div>
					<div>
						<label>Service line / Area</label>
						<select id="filterAreaServiceLine">
							<option value=""></option>
						</select>
					</div>
					<div>
						<label>Cargo</label>
						<select id="filterCargo">
							<option value=""></option>
						</select>
					</div>
					<div>
						<label>Sede</label>
						<select id="filterSede">
							<option value=""></option>
						</select>
					</div>
					<div>
						<button id="btnSearch" type="button">Buscar</button>
					</div>
				</div>
			</div>
			<div class="containerCardsPerson">
			</div>
		</div>
		<div class="brigadists">
			<h3>Brigadistas</h3>
			Prueba 
			Prueba 
			Prueba Prueba Prueba Prueba Prueba Prueba Prueba Prueba Prueba Prueba Prueba Prueba Prueba 
		</div>
	</div>
	<script>

		document.getElementsByClassName('maximizeMinimizeIcon')[0].onclick = maximizeMinimizeSearch;

		document.getElementById('btnSearch').onclick = function () { getEmpleados(); return false; };

		setupForms();

	</script>
</asp:Content>
