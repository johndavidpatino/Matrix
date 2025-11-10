<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRRHH.master" CodeBehind="HojasVida.aspx.vb" Inherits="WebMatrix.HojasVida" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
	<script src="../Scripts/noUiSlider/nouislider.min.js"></script>
	<link href="../Content/noUiSlider/css/nouislider.min.css" rel="stylesheet">
	<link href="../Content/noUiSlider/css/nouislider.pips.css" rel="stylesheet">
	<link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
	<style>
		input[type="file"] {
			display: none;
		}

		.custom-file-upload {
			border: 1px solid #ccc;
			display: inline-block;
			padding: 6px 12px;
			cursor: pointer;
			align-self: baseline;
		}

			.custom-file-upload:hover {
				background-color: rgb(0,173,168);
			}

		.containerCardsPersons {
			background-color: #f5f5f5;
			margin: 10px;
		}

		.cards {
			display: flex;
			flex-wrap: wrap;
			justify-content: center;
			//align-items: flex-start;
			align-self: baseline;
			width: 980px;
		}

			.cards .cardPerson {
				margin-left: 5px;
				margin-right: 5px;
				cursor: pointer;
				transition: all 1s ease;
				width: 180px;
				top: 80px;
				margin-bottom: 10px;
				border: dotted 1px rgb(54,78,162);
			}

				.cards .cardPerson img {
					width: 120px;
				}

				.cards .cardPerson i {
					color: rgb(0,173,168);
				}

				.cards .cardPerson:hover {
					box-shadow: 0 5px 5px rgba(106,106,106.1,0.4), 0 -5px 5px rgba(106,106,106.1,0.4), 5px 0 5px rgba(106,106,106.1,0.4), -5px 0 5px rgba(106,106,106.1,0.4);
				}

				.cards .cardPerson label {
					float: none !important;
					width: auto !important;
					text-align: left !important;
					cursor: pointer;
					word-wrap: break-word;
				}

					.cards .cardPerson label i {
						font-size: 14px;
						margin-right: 2px;
					}

				.cards .cardPerson.open {
					height: calc(100vh - 120px);
					z-index: 1000000;
					position: absolute;
					left: 1000px;
				}

					.cards .cardPerson.open a {
						visibility: visible;
					}

		.person {
			width: 980px;
			background-color: #f5f5f5;
			margin: 10px;
		}

			.person label {
				float: none !important;
				width: auto !important;
				text-align: left !important;
			}

			.person button {
				float: none !important;
			}

		.showReturnToPersons {
			display: none;
		}

		.returnToPersons {
			width: 24px;
		}

			.returnToPersons:hover {
				box-shadow: 0 5px 5px rgba(106,106,106.1,0.4), 0 -5px 5px rgba(106,106,106.1,0.4), 5px 0 5px rgba(106,106,106.1,0.4), -5px 0 5px rgba(106,106,106.1,0.4);
			}

			.returnToPersons i {
				cursor: pointer;
				background-color: rgb(228,228,228);
				color: rgb(0,173,168);
			}

		.scroller {
			display: inline-flex;
			overflow: hidden;
			width: 1000px;
			background-color: white;
		}

		.interview {
			margin-left: 20px;
			margin-top: 10px;
		}

			.interview input {
				margin-bottom: 0px !important;
				margin-left: 0px !important;
				float: none !important;
			}

			.interview label {
				margin-left: 0px !important;
				float: none !important;
			}

			.interview textarea {
				min-width: 300px;
			}

		.interviewIssue {
			position: relative;
			padding-left: 20px;
		}

			.interviewIssue .issue {
				padding: 5px;
				background-color: #ecf0f1;
				border-radius: 8px;
				-webkit-border-radius: 8px;
				margin-bottom: 3px;
				padding-top: 5px;
				padding-right: 3px;
				min-width: 200px;
			}

			.interviewIssue .arrow {
				position: absolute;
				left: 10px;
				top: 10px;
				display: block;
				height: 0;
				width: 0;
				border-top: 20px solid transparent;
				border-bottom: 20px solid transparent;
				border-right: 20px solid #ecf0f1;
			}

			.interviewIssue .iconDeleteInterview {
				cursor: pointer;
				display: block;
				width: fit-content;
				float: right
			}

				.interviewIssue .iconDeleteInterview:hover {
					box-shadow: 0 5px 5px rgba(106,106,106.1,0.4), 0 -5px 5px rgba(106,106,106.1,0.4), 5px 0 5px rgba(106,106,106.1,0.4), -5px 0 5px rgba(106,106,106.1,0.4);
				}

		.keywords {
			margin-left: 20px;
		}

		.containerKeywords {
			margin-top: 10px;
			display: flex;
			flex-wrap: wrap;
		}

			.containerKeywords .keyword {
				border-radius: 8px;
				-webkit-border-radius: 8px;
				background-color: rgb(0,173,168);
				padding: 6px !important;
				margin-left: 3px;
				margin-bottom: 3px;
			}

				.containerKeywords .keyword label {
					margin-left: 0 !important;
					padding-top: 0px !important;
				}

				.containerKeywords .keyword .iconDeleteKeyword {
					cursor: pointer;
					display: block;
					width: fit-content;
					float: right;
					font-size: 10px;
				}

					.containerKeywords .keyword .iconDeleteKeyword:hover {
						box-shadow: 0 5px 5px rgba(106,106,106.1,0.4), 0 -5px 5px rgba(106,106,106.1,0.4), 5px 0 5px rgba(106,106,106.1,0.4), -5px 0 5px rgba(106,106,106.1,0.4);
					}

		.formKeywords {
			margin-top: 10px;
		}

			.formKeywords input {
				margin-right: 10px !important;
			}

		#stylized input:required:invalid {
			border-color: red;
		}

		#stylized label {
			margin-left: 10px;
		}

		.person input:required:valid {
			border-color: #0f0;
		}



		.search {
			position: absolute;
			width: 10%;
		}

			.search .searchIcon {
				cursor: pointer;
			}

				.search .searchIcon:hover {
					box-shadow: 0 5px 5px rgba(106,106,106.1,0.4), 0 -5px 5px rgba(106,106,106.1,0.4), 5px 0 5px rgba(106,106,106.1,0.4), -5px 0 5px rgba(106,106,106.1,0.4);
				}

				.search .searchIcon.hideIcon {
					display: none;
				}


			.search .searchClose {
				cursor: pointer;
			}

				.search .searchClose:hover {
					box-shadow: 0 5px 5px rgba(106,106,106.1,0.4), 0 -5px 5px rgba(106,106,106.1,0.4), 5px 0 5px rgba(106,106,106.1,0.4), -5px 0 5px rgba(106,106,106.1,0.4);
				}

			.search .searchForm {
				transition: all ease-out 0.3s;
				position: absolute;
				left: 0px;
				background-color: rgba(175, 175, 175, 1)
			}

				.search .searchForm.hideSearchForm {
					opacity: -1;
					left: -200px;
				}

				.search .searchForm.showSearchForm {
					opacity: 1;
					left: 0px;
				}

				.search .searchForm label {
					float: none !important;
					width: auto !important;
					text-align: left !important;
				}

				.search .searchForm > [input, select] {
					margin-left: 0px !important;
				}

				.search .searchForm h4 {
					text-shadow: unset !important;
					width: fit-content;
					display: inline-block;
				}

				.search .searchForm i {
					float: right;
					display: inline-block;
				}

		.filters .maximizeSearchIcon, .filters .minimizeSearchIcon {
			cursor: pointer;
			background-color: rgb(228,228,228);
			color: rgb(0,173,168);
		}

			.filters .maximizeSearchIcon:hover, .filters .minimizeSearchIcon:hover {
				box-shadow: 0 5px 5px rgba(106,106,106.1,0.4), 0 -5px 5px rgba(106,106,106.1,0.4), 5px 0 5px rgba(106,106,106.1,0.4), -5px 0 5px rgba(106,106,106.1,0.4);
			}

		.formFilters label {
			float: none !important;
			width: auto !important;
			text-align: left !important;
		}

		.formFilters > [input, select] {
			margin-left: 0px !important;
		}

		.namesPerson {
			font-size: 30px !important;
			margin: 10px;
			color: rgb(0,173,168);
		}

		.interviewsKeyWords {
			margin-top: 10px;
			display: flex;
			justify-content: space-around;
		}

		#filterAnosExperiencia {
			margin-left: 15px;
			margin-right: 20px;
			margin-top: 5px;
		}

		.noUi-horizontal {
			height: 14px;
		}

			.noUi-horizontal .noUi-handle {
				top: -4px;
				width: 24px;
				height: 20px;
			}

		.filters {
			transition: all ease 1s;
			height: 30px;
			overflow-y: hidden;
			margin-bottom: 10px;
		}

			.filters.maximize {
				height: 300px;
			}

			.filters .minimizeSearch {
				bottom: 1px;
			}

		.formJobExperiencie input {
			margin-bottom: 0px !important;
			margin-left: 0px !important;
			float: none !important;
		}

		.formJobExperiencie label {
			margin-left: 0px !important;
			float: none !important;
		}

		.jobsExperiences {
			margin-left: 20px;
		}

		.experiencies {
			position: relative;
		}

			.experiencies .jobExperiencie {
				padding: 5px;
				background-color: #ecf0f1;
				border-radius: 8px;
				-webkit-border-radius: 8px;
				margin-bottom: 3px;
				padding-top: 5px;
				padding-right: 3px;
				min-width: 200px;
			}

				.experiencies .jobExperiencie .iconDeleteJobExperiencie {
					cursor: pointer;
					display: block;
					width: fit-content;
					float: right
				}

					.experiencies .jobExperiencie .iconDeleteJobExperiencie:hover {
						box-shadow: 0 5px 5px rgba(106,106,106.1,0.4), 0 -5px 5px rgba(106,106,106.1,0.4), 5px 0 5px rgba(106,106,106.1,0.4), -5px 0 5px rgba(106,106,106.1,0.4);
					}
	</style>
	<script>
		var intervalIdRight;
		var intervalIdLeft;
		var scrollLeftNow = 0;
		var scrollRightNow = 1000;
		var idPersonSelected;

		(function () {
			var originalFetch = fetch;
			fetch = function () {
				return originalFetch.apply(this, arguments).then(function (data) {
					if (data.status == 401)
						window.location.href = "/ReturnUrl=" + window.location.pathname;
					return data;
				});
			};
		})();


		function animateScroll(parentToScroll) {
			if (scrollLeftNow >= 1000) {
				clearInterval(intervalIdLeft);
				scrollLeftNow = 0;
			}
			else {
				scrollLeftNow += 100;
				parentToScroll.scrollLeft = scrollLeftNow;
			}
		}
		function animateScrollLeft(parentToScroll) {
			if (scrollRightNow <= 0) {
				scrollRightNow = 1000;
				clearInterval(intervalIdRight);
			}
			else {
				scrollRightNow -= 100;
				parentToScroll.scrollLeft = scrollRightNow;
			}
		}

		async function showPerson(element) {
			let person;
			let interviews;
			intervalIdLeft = setInterval(function () { animateScroll(element.parentElement.parentElement.parentElement.parentElement) }, 25)
			document.getElementsByClassName('returnToPersons')[0].classList.toggle('showReturnToPersons');
			idPersonSelected = element.getElementsByTagName('input').length > 0 ? element.getElementsByTagName('input')[0].value : null;
			//hideFilterForm();
			//hideSearchIcon();
			clearFormPerson();
			clearInterviews();
			clearKeywords();
			clearExperienciasLaborales();

			if (idPersonSelected) {
				person = await getPerson(idPersonSelected);
				interviews = await getEntrevistas(idPersonSelected);
				jobsExperiencies = await getExperienciasLaborales(idPersonSelected);

				loadPerson(person);
				drawEntrevistas(interviews);
				drawNamesPerson(person.Nombres, person.Apellidos);
				drawExperienciasLaborales(jobsExperiencies);
			}
		}
		function returnToPersons() {
			let element = document.getElementsByClassName('scroller')[0];
			let persons;
			intervalIdRight = setInterval(function () { animateScrollLeft(element) }, 25)
			document.getElementsByClassName('returnToPersons')[0].classList.toggle('showReturnToPersons');
			//showSearchIcon();
			clearPersons();
			getHojasVida();
		}
		function loadImage(input) {
			let img = document.querySelector('.person .left img');
			let url = URL.createObjectURL(input.files[0]);
			img.src = url;
		}
		async function save() {

			let formData = new Object();

			if (isValidForm()) {
				formData.idPersonSelected = idPersonSelected;
				formData.tipoIdentificacion = document.getElementById('tipoIdentificacion').value == "" ? null : document.getElementById('tipoIdentificacion').value;
				formData.identificacion = document.getElementById('identificacion').value == "" ? null : document.getElementById('identificacion').value;
				formData.nombres = document.getElementById('nombres').value;
				formData.apellidos = document.getElementById('apellidos').value;
				formData.edad = document.getElementById('edad').value == "" ? null : document.getElementById('edad').value;
				formData.nivelIngles = document.getElementById('nivelIngles').value == "" ? null : document.getElementById('nivelIngles').value;
				formData.numeroCelular = document.getElementById('numeroCelular').value == "" ? null : document.getElementById('numeroCelular').value;
				formData.correo = document.getElementById('correo').value == "" ? null : document.getElementById('correo').value;
				formData.fechaEntrevista = document.getElementById('fechaEntrevista').value == "" ? null : document.getElementById('fechaEntrevista').value;
				formData.observacion = document.getElementById('observacion').value == "" ? null : document.getElementById('observacion').value;
				formData.anosExperiencia = document.getElementById('anosExperiencia').value == "" ? null : document.getElementById('anosExperiencia').value;
				formData.ciudadResidencia = document.getElementById('ciudadResidencia').value == "" ? null : document.getElementById('ciudadResidencia').value;
				formData.nivelEducativo = document.getElementById('nivelEducativo').value == "" ? null : document.getElementById('nivelEducativo').value;
				formData.profesion = document.getElementById('profesion').value == "" ? null : document.getElementById('profesion').value;

				fetch('HojasVida.aspx/savePerson', {
					method: 'POST',
					body: JSON.stringify(formData),
					headers: {
						'Content-Type': 'application/json'
					}
				})
					.then(result => result.json())
					.then(async data => {
						let personId = data.d;
						idPersonSelected = personId;
						clearInterviews();
						clearKeywords();
						clearFormInterview();
						clearFormKeywords();
						let person = await getPerson(personId);
						let interviews = await getEntrevistas(personId);
						loadPerson(person);
						drawEntrevistas(interviews);
					});
			}
		}
		async function addKeyword() {

			let formData = new Object();

			if (isValidForm()) {
				formData.idPersonSelected = idPersonSelected;
				formData.keyword = document.getElementById('keyword').value;

				fetch('HojasVida.aspx/addKeyword', {
					method: 'POST',
					body: JSON.stringify(formData),
					headers: {
						'Content-Type': 'application/json'
					}
				})
					.then(result => result.json())
					.then(async data => {
						let person;
						person = await getPerson(idPersonSelected);
						clearFormKeywords();
						clearKeywords();
						drawKeywords(person.keywords.split(','));
					});
			}
		}
		async function addEntrevista() {

			let formData = new Object();

			if (isValidForm()) {
				formData.idPersonSelected = idPersonSelected;
				formData.fechaEntrevista = document.getElementById('fechaEntrevista').value;
				formData.observacion = document.getElementById('observacion').value;

				fetch('HojasVida.aspx/addEntrevista', {
					method: 'POST',
					body: JSON.stringify(formData),
					headers: {
						'Content-Type': 'application/json'
					}
				})
					.then(result => result.json())
					.then(async data => {
						let interviews;
						interviews = await getEntrevistas(idPersonSelected);
						clearFormInterview();
						clearInterviews();
						drawEntrevistas(interviews);
					});
			}
		}
		async function addExperienciaLaboral() {

			let formData = new Object();

			formData.idPersonSelected = idPersonSelected;
			formData.empresa = document.getElementById('empresaExperienciaLaboral').value;
			formData.duracionAnos = document.getElementById('duracionAnosExperienciaLaboral').value;

			fetch('HojasVida.aspx/addExperienciaLaboral', {
				method: 'POST',
				body: JSON.stringify(formData),
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(async data => {
					let experienciasLaborales;

					clearFormExperienciaLaboral();
					clearExperienciasLaborales();

					experienciasLaborales = await getExperienciasLaborales(idPersonSelected);
					drawExperienciasLaborales(experienciasLaborales);
				});
		}

		function imageToText(inputFile) {
			const temporaryFileReader = new FileReader();

			return new Promise((resolve, reject) => {
				temporaryFileReader.onerror = () => {
					temporaryFileReader.abort();
					reject(new DOMException("Problem parsing input file."));
				};

				temporaryFileReader.onload = () => {
					resolve(temporaryFileReader.result.replace('data:image/png;base64,', '').replace('data:image/jpeg;base64,', ''));
				};
				temporaryFileReader.readAsDataURL(inputFile);
			});
		}
		function isValidForm() {
			let form = document.getElementsByTagName('form')[0];
			return form.checkValidity();
		}
		function showFilterForm() {
			document.getElementsByClassName('searchForm')[0].classList.remove('hideSearchForm');
			document.getElementsByClassName('searchForm')[0].classList.toggle('showSearchForm');
			hideSearchIcon();
		}
		function hideFilterForm() {
			document.getElementsByClassName('searchForm')[0].classList.remove('showSearchForm');
			document.getElementsByClassName('searchForm')[0].classList.add('hideSearchForm');
			showSearchIcon();
		}
		function hideSearchIcon() {
			document.getElementsByClassName('searchIcon')[0].classList.add('hideIcon');
		}
		function showSearchIcon() {
			document.getElementsByClassName('searchIcon')[0].classList.remove('hideIcon');
		}
		function maximizeSearch() {
			document.getElementsByClassName('filters')[0].classList.add('maximize');
		}
		function minimizeSearch() {
			document.getElementsByClassName('filters')[0].classList.remove('maximize');
		}

		function clearPersons() {
			let cardsPersons = Array.from(document.getElementsByClassName('cardPerson'));
			cardsPersons.splice(0, 1);
			cardsPersons.forEach(element => element.remove());
		}
		function clearInterviews() {
			let containerInterviews = document.getElementsByClassName('interviewsIssues')[0];
			containerInterviews.innerHTML = '';
		}
		function clearKeywords() {
			let containerKeywords = document.getElementsByClassName('containerKeywords')[0];
			containerKeywords.innerHTML = '';
		}
		function clearExperienciasLaborales() {
			let containerExperienciasLaborales = document.getElementsByClassName('experiencies')[0];
			containerExperienciasLaborales.innerHTML = '';
		}
		function clearFormPerson() {
			let elements = Array.from(document.querySelectorAll('.person input,.person select,.person textarea'));
			elements.forEach((element) => {
				element.value = '';
			});
			document.getElementsByClassName('namesPerson')[0].innerHTML = '';
		}
		function clearFormInterview() {
			let elements = Array.from(document.querySelectorAll('.interview input,.interview select,.interview textarea'));
			elements.forEach((element) => {
				element.value = '';
			});
		}
		function clearFormKeywords() {
			let elements = Array.from(document.querySelectorAll('.formKeywords input'));
			elements.forEach((element) => {
				element.value = '';
			});
		}
		function clearFormExperienciaLaboral() {
			let elements = Array.from(document.querySelectorAll('.formJobExperiencie input'));
			elements.forEach((element) => {
				element.value = '';
			});
		}


		function loadPerson(person) {

			let keywords;

			document.getElementById('tipoIdentificacion').value = person.TipoIdentificacion;
			document.getElementById('identificacion').value = person.Identificacion;
			document.getElementById('nombres').value = person.Nombres;
			document.getElementById('apellidos').value = person.Apellidos;
			document.getElementById('edad').value = person.Edad;
			document.getElementById('anosExperiencia').value = person.AnosExperiencia;
			document.getElementById('nivelIngles').value = person.NivelIngles;
			document.getElementById('numeroCelular').value = person.NumeroCelular;
			document.getElementById('correo').value = person.Correo;
			document.getElementById('ciudadResidencia').value = person.CiudadResidencia;
			document.getElementById('nivelEducativo').value = person.NivelEducativo;
			document.getElementById('profesion').value = person.Profesion;


			if (person.keywords) {
				keywords = person.keywords.split(',');
				drawKeywords(keywords);
			}
		}

		function deleteInterview(element) {
			let formData = Object();

			formData.id = element.getElementsByTagName('input')[0].value;

			fetch('HojasVida.aspx/eliminarEntrevista', {
				method: 'POST',
				body: JSON.stringify(formData),
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(async data => {
					clearInterviews();
					let interviews = await getEntrevistas(idPersonSelected);
					drawEntrevistas(interviews);
				});
		}
		function deleteKeyword(element) {
			let formData = Object();

			formData.hojasVidaId = idPersonSelected;
			formData.keyword = element.getElementsByTagName('label')[0].innerHTML.trim();

			fetch('HojasVida.aspx/eliminarKeyword', {
				method: 'POST',
				body: JSON.stringify(formData),
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(async data => {
					clearKeywords();
					let keywords;
					let person = await getPerson(idPersonSelected);
					keywords = person.keywords.split(',');
					drawKeywords(keywords);
				});
		}
		function deleteExperienciaLaboral(element) {
			let formData = Object();

			formData.id = element.getElementsByTagName('input')[0].value;

			fetch('HojasVida.aspx/eliminarExperienciaLaboral', {
				method: 'POST',
				body: JSON.stringify(formData),
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(async data => {
					clearExperienciasLaborales();
					let experienciasLaborales = await getExperienciasLaborales(idPersonSelected);
					drawExperienciasLaborales(experienciasLaborales);
				});
		}


		function drawPersons(persons) {
			let cards = document.getElementsByClassName('cards')[0];
			persons.forEach((element, index) => {
				let rawHtml = `<div class="cardPerson">
					<label>${element.Nombres}</label>
					<label>${element.Apellidos}</label>
					<label>${element.Edad} Años</label>
					<label><i class="material-icons icon" title="Correo">mail</i>${element.Correo}</label>
					<label><i class="material-icons icon" title="Número celular">phone_iphone</i>${element.NumeroCelular}</label>
					<label><i class="material-icons icon" title="Años experiencia">trending_up</i>${(element.AnosExperiencia == null ? 'No registra' : element.AnosExperiencia) + ' años experiencia'} </label>
					<label><i class="material-icons icon" title="Fecha creación">calendar_today</i>${element.fechaCreacion != null ? new Date(parseInt(element.fechaCreacion.substr(6))).toLocaleDateString() : 'Sin fecha creación'}</label>
					<input type="hidden" value="${element.id}" />
				</div>`;
				cards.insertAdjacentHTML("beforeend", rawHtml);
				let persons = document.getElementsByClassName('cardPerson');
				persons[persons.length - 1].onclick = function () { showPerson(this) };
			});
		}
		function drawKeywords(keywords) {
			let containerKeywords = document.getElementsByClassName('containerKeywords')[0];
			keywords.forEach((element => {
				if (element.trim() != '') {
					let rawHtml = `<div class="keyword">
									<i class="material-icons iconDeleteKeyword" style="display:block;width:fit-content;float:right" title="Eliminar">close</i>
									<label>
										${element}
									</label>
								</div>
								`;
					containerKeywords.insertAdjacentHTML("beforeend", rawHtml);

					keywords = Array.from(document.getElementsByClassName('iconDeleteKeyword'));

					keywords.forEach((element, index) => element.onclick = function () { deleteKeyword(element.parentElement) });
				}

			}));
		}
		function drawNamesPerson(nombres, apellidos) {
			let personName = document.getElementsByClassName('namesPerson')[0];
			personName.innerHTML = apellidos + ' ' + nombres;
		}
		function drawEntrevistas(entrevistas) {
			let containerKeywords = document.getElementsByClassName('interviewsIssues')[0];
			let interviewIssues;
			entrevistas.forEach((element => {
				let rawHtml = `<div class="interviewIssue">
									<div class="issue">
										<i class="material-icons iconDeleteInterview" title="Eliminar">close</i>
										<label>${new Date(parseInt(element.fechaEntrevista.substr(6))).toLocaleDateString()}</label>
										<label>${element.observacion}</label>
									</div>
									<div class="arrow"></div>
									<input type="hidden" value="${element.id}" />
								</div>`;
				containerKeywords.insertAdjacentHTML("beforeend", rawHtml);
			}));

			interviewIssues = Array.from(document.querySelectorAll('.issue i'));

			interviewIssues.forEach((element, index) => element.onclick = function () { deleteInterview(element.parentElement.parentElement) });
		}
		function drawExperienciasLaborales(experienciasLaborales) {
			let containerExperiencias = document.getElementsByClassName('experiencies')[0];
			let experiencias;
			experienciasLaborales.forEach((element => {
				let rawHtml = `<div class="jobExperiencie">
								<i class="material-icons iconDeleteJobExperiencie" title="Eliminar">close</i>
								<label>Empresa: ${element.empresa}</label>
								<label>Duración(Años): ${element.duracionAnos}</label>
								<input type="hidden" value="${element.id}" />
								</div>`;
				containerExperiencias.insertAdjacentHTML("beforeend", rawHtml);
			}));

			experiencias = Array.from(document.querySelectorAll('.jobExperiencie i'));

			experiencias.forEach((element, index) => element.onclick = function () { deleteExperienciaLaboral(element.parentElement) });
		}

		function getHojasVida() {

			clearPersons();

			let formData = new Object();
			formData.id = null;
			formData.nombres = document.getElementById('filterNombres').value;
			formData.apellidos = document.getElementById('filterApellidos').value;
			formData.nivelIngles = document.getElementById('filterNivelIngles').value;
			formData.keywords = document.getElementById('filterKeywords').value;
			formData.anosExperienciaInicio = parseInt(document.getElementById('filterAnosExperienciaInicio').value, 10);
			formData.anosExperienciaFin = parseInt(document.getElementById('filterAnosExperienciaFin').value, 10);
			formData.nivelEducativo = document.getElementById('filterNivelEducativo').value;
			formData.ciudadResidencia = document.getElementById('filterCiudadResidencia').value;
			formData.tieneEntrevista = document.getElementById('filterTieneEntrevista').value;
			formData.profesion = document.getElementById('filterProfesion').value;


			fetch('HojasVida.aspx/obtenerHojasVida', {
				method: 'POST',
				body: JSON.stringify(formData),
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => drawPersons(data.d));
		}
		function getEntrevistas(idPerson) {
			let formData = new Object();

			formData.hojaVidaId = idPerson;

			return fetch('HojasVida.aspx/obtenerEntrevistasHojaVida', {
				method: 'POST',
				body: JSON.stringify(formData),
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d });
		}
		function getExperienciasLaborales(idPerson) {
			let formData = new Object();

			formData.hojaVidaId = idPerson;

			return fetch('HojasVida.aspx/obtenerExperienciasLaboralesPorHojaVida', {
				method: 'POST',
				body: JSON.stringify(formData),
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d });
		}
		function getPerson(idPerson) {
			let formData = new Object();

			formData.id = idPerson;

			return fetch('HojasVida.aspx/obtenerHojasVidaPorId', {
				method: 'POST',
				body: JSON.stringify(formData),
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d[0] });
		}
		function getNivelesEducativos() {

			return fetch('HojasVida.aspx/getNivelesEducativos', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d });
		}
		function getCiudades() {

			return fetch('HojasVida.aspx/getCiudades', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d });
		}
		function getProfesiones() {

			return fetch('HojasVida.aspx/getProfesiones', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json'
				}
			})
				.then(result => result.json())
				.then(data => { return data.d });
		}

		function fillSelectNivelesEducativos(nivelesEducativos) {
			let select = document.getElementById('nivelEducativo');

			nivelesEducativos.forEach((element, index) => {
				let option = document.createElement('option');
				option.value = element.id;
				option.text = element.NivelEducativo;
				select.add(option);
			});

		}
		function fillSelectCiudades(ciudades) {
			let select = document.getElementById('ciudadResidencia');

			ciudades.forEach((element, index) => {
				let option = document.createElement('option');
				option.value = element.id;
				option.text = element.Ciudad;
				select.add(option);
			});

		}
		function fillSelectProfesiones(profesiones) {
			let select = document.getElementById('profesion');

			profesiones.forEach((element, index) => {
				let option = document.createElement('option');
				option.value = element.id;
				option.text = element.profesion;
				select.add(option);
			});

		}
		function fillSelectFilterNivelesEducativos(nivelesEducativos) {
			let select = document.getElementById('filterNivelEducativo');

			nivelesEducativos.forEach((element, index) => {
				let option = document.createElement('option');
				option.value = element.id;
				option.text = element.NivelEducativo;
				select.add(option);
			});

		}
		function fillSelectFilterCiudadesResidencia(ciudades) {
			let select = document.getElementById('filterCiudadResidencia');

			ciudades.forEach((element, index) => {
				let option = document.createElement('option');
				option.value = element.id;
				option.text = element.Ciudad;
				select.add(option);
			});

		}
		function fillSelectFilterProfesiones(profesiones) {
			let select = document.getElementById('filterProfesion');

			profesiones.forEach((element, index) => {
				let option = document.createElement('option');
				option.value = element.id;
				option.text = element.profesion;
				select.add(option);
			});

		}


		function setupFilterControlAnosExperiencia() {
			let slider = document.getElementById('filterAnosExperiencia');
			let inputStart = document.getElementById('filterAnosExperienciaInicio');
			let inputEnd = document.getElementById('filterAnosExperienciaFin');
			let inputsStartEnd = [inputStart, inputEnd];
			noUiSlider.create(slider, {
				connect: true,
				tooltips: true,
				start: [4, 10],
				range: {
					'min': 0,
					'max': 20
				},
				step: 1
			});
			slider.noUiSlider.on('update', function (values, handle) {
				inputsStartEnd[handle].value = values[handle];
				document.getElementsByClassName('filterAnosExperienciaRango')[0].innerHTML = values.join(' - ');
			});

		}

		async function setupForms() {
			let nivelesEducativos;
			let ciudades;
			let profesiones;

			nivelesEducativos = await getNivelesEducativos();
			fillSelectNivelesEducativos(nivelesEducativos);
			ciudades = await getCiudades();
			fillSelectCiudades(ciudades);
			profesiones = await getProfesiones();
			fillSelectProfesiones(profesiones);

			fillSelectFilterNivelesEducativos(nivelesEducativos);
			fillSelectFilterCiudadesResidencia(ciudades);
			fillSelectFilterProfesiones(profesiones);
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
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Subtitulo" runat="server">
	Hojas de vida
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
	<div class="returnToPersons showReturnToPersons">
		<i class="material-icons">keyboard_backspace</i>
	</div>
	<%--<div class="search">
		<i class="material-icons searchIcon">search</i>
		<div class="searchForm hideSearchForm">
			<h4>Filtros</h4>
			<i class="material-icons searchClose" style="text-align: right">close</i>
			<div>
				<label>Nombres</label>
				<input type="text" id="filterNombres" />
			</div>
			<div>
				<label>Apellidos</label>
				<input type="text" id="filterApellidos" />
			</div>
			<div>
				<label>Nivel ingles</label>
				<select id="filterNivelIngles">
					<option value=""></option>
					<option value="1">Basico</option>
					<option value="2">Intermedio</option>
					<option value="3">Alto</option>
				</select>
			</div>
			<div>
				<label>Keywords</label>
				<input type="text" id="filterKeywords" />
			</div>
			<div>
				<label>Años experiencia</label>
				<div id="filterAnosExperiencia"></div>
				<input type="hidden" id="filterAnosExperienciaInicio" />
				<input type="hidden" id="filterAnosExperienciaFin" />
				<label class="filterAnosExperienciaRango"></label>
			</div>
			<div>
				<label>Nivel educativo</label>
				<select id="filterNivelEducativo">
					<option value=""></option>
				</select>
			</div>
			<div>
				<label>Ciudad residencia</label>
				<select id="filterCiudadResidencia">
					<option value=""></option>
				</select>
			</div>
		</div>
	</div>--%>
	<div class="scroller">
		<div style="width: 2400px; display: inline-flex;">
			<div class="containerCardsPersons">
				<div class="filters" style="width: 980px;">
					<div>
						<i class="material-icons maximizeSearchIcon" title="Filtros">details</i>
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
							<label>Nivel ingles</label>
							<select id="filterNivelIngles">
								<option value=""></option>
								<option value="1">Basico</option>
								<option value="2">Intermedio</option>
								<option value="3">Alto</option>
							</select>
						</div>
						<div>
							<label>Keywords</label>
							<input type="text" id="filterKeywords" />
						</div>
						<div style="width: 200px;">
							<label>Años experiencia</label>
							<div id="filterAnosExperiencia"></div>
							<input type="hidden" id="filterAnosExperienciaInicio" />
							<input type="hidden" id="filterAnosExperienciaFin" />
							<label class="filterAnosExperienciaRango"></label>
						</div>
						<div>
							<label>Nivel educativo</label>
							<select id="filterNivelEducativo">
								<option value=""></option>
							</select>
						</div>
						<div>
							<label>Ciudad residencia</label>
							<select id="filterCiudadResidencia">
								<option value=""></option>
							</select>
						</div>
						<div>
							<label>Profesión</label>
							<select id="filterProfesion">
								<option value=""></option>
							</select>
						</div>
						<div>
							<label>Tiene entrevista</label>
							<select id="filterTieneEntrevista">
								<option value=""></option>
								<option value="true">Si</option>
								<option value="false">No</option>
							</select>
						</div>
						<div>
							<button id="btnSearch">Buscar</button>
						</div>
					</div>
					<div style="text-align: right">
						<i class="material-icons minimizeSearchIcon">publish</i>
					</div>
				</div>
				<div class="cards">
					<div class="cardPerson">
						<i class="material-icons" style="font-size: 120px">person_add</i>
					</div>
				</div>
			</div>
			<div class="person">
				<div class="formPerson">
					<div>
						<label class="namesPerson" style="margin-left: 10px;">
						</label>
					</div>
					<div style="display: flex; flex-wrap: wrap; align-content: space-between;">
						<div>
							<label for="tipoIdentificacion">Tipo identificación</label>
							<select id="tipoIdentificacion">
								<option value=""></option>
								<option value="1">Cedula ciudadania</option>
								<option value="2">Tarjeta de identidad</option>
								<option value="3">Cedula de extranjeria</option>
							</select>
						</div>
						<div>
							<label for="identificacion">Identificación</label>
							<input type="text" id="identificacion" />
						</div>
						<div>
							<label for="nombres">Nombres</label>
							<input type="text" required id="nombres" />
						</div>
						<div>
							<label for="apellidos">Apellidos</label>
							<input type="text" required id="apellidos" />
						</div>
						<div>
							<label for="edad">Edad</label>
							<input type="number" id="edad" />
						</div>
						<div>
							<label for="nivelIngles">Nivel Ingles</label>
							<select id="nivelIngles">
								<option value=""></option>
								<option value="1">Basico</option>
								<option value="2">Intermedio</option>
								<option value="3">Alto</option>
							</select>
						</div>
						<div>
							<label for="numeroCelular">Número celular</label>
							<input type="number" id="numeroCelular" />
						</div>
						<div>
							<label for="correo">Correo</label>
							<input type="email" required id="correo" />
						</div>
						<div>
							<label for="anosExperiencia">Experiencia(Años):</label>
							<input type="number" id="anosExperiencia" />
						</div>
						<div>
							<label for="ciudadResidencia">Ciudad residencia</label>
							<select id="ciudadResidencia">
								<option value=""></option>
							</select>
						</div>
						<div>
							<label for="nivelEducativo">Nivel educativo mas alto</label>
							<select id="nivelEducativo">
								<option value=""></option>
							</select>
						</div>
						<div>
							<label for="profesion">Profesión</label>
							<select id="profesion">
								<option value=""></option>
							</select>
						</div>
					</div>
					<div style="margin-left: 10px;">
						<button id="btnAdd">Actualizar!</button>
					</div>
				</div>
				<div class="interviewsKeyWords">
					<div class="interviews">
						<h3>Entrevistas</h3>
						<div class="interviewsIssues">
						</div>
						<div class="interview">
							<div>
								<label for="fechaEntrevista">Fecha Entrevista</label>
								<input type="date" id="fechaEntrevista" />
							</div>
							<div>
								<label for="observacion">Observación</label>
								<textarea rows="5" cols="10" id="observacion"></textarea>
							</div>
							<div>
								<button id="btnAddEntrevista">Adicionar!</button>
							</div>
						</div>
					</div>
					<div class="jobsExperiences">
						<h3>Experiencia laboral</h3>
						<div class="experiencies">
						</div>
						<div class="formJobExperiencie">
							<div>
								<label for="empresaExperienciaLaboral">Empresa</label>
								<input type="text" id="empresaExperienciaLaboral" />
							</div>
							<div>
								<label for="duracionAnosExperienciaLaboral">Duración(Años)</label>
								<input type="number" id="duracionAnosExperienciaLaboral" step="0.0" />
							</div>
							<div>
								<button id="btnAddExperienciaLaboral">Adicionar!</button>
							</div>
						</div>
					</div>
					<div class="keywords">
						<h3>Keywords</h3>
						<div class="containerKeywords">
						</div>
						<div class="formKeywords">
							<input type="text" id="keyword" placeholder="Keyword" />
							<button id="btnAddKeywords">Adicionar!</button>
						</div>
					</div>
				</div>
			</div>
		</div>
	</div>
	<script>
		var elements = Array.from(document.getElementsByClassName('cardPerson'));
		elements.forEach((element, index) => element.onclick = function () { showPerson(element) });
		document.getElementsByClassName('returnToPersons')[0].onclick = returnToPersons;
		document.getElementById('btnAdd').onclick = () => { save(); return false; };
		document.getElementById('btnAddKeywords').onclick = () => { addKeyword(); return false; };
		document.getElementById('btnAddEntrevista').onclick = () => { addEntrevista(); return false; };
		document.getElementById('btnAddExperienciaLaboral').onclick = () => { addExperienciaLaboral(); return false; };
		document.getElementById('btnSearch').onclick = () => { getHojasVida(); return false; };
		//document.getElementsByClassName('searchIcon')[0].onclick = showFilterForm;
		//document.getElementsByClassName('searchClose')[0].onclick = hideFilterForm;
		document.querySelectorAll('.searchForm select').forEach(element => { element.onchange = getHojasVida });
		document.querySelectorAll('.searchForm input').forEach(element => { element.onkeyup = getHojasVida });

		document.getElementsByClassName('maximizeSearchIcon')[0].onclick = maximizeSearch;
		document.getElementsByClassName('minimizeSearchIcon')[0].onclick = minimizeSearch;

		setupFilterControlAnosExperiencia();
		getHojasVida();
		setupForms();
	</script>
</asp:Content>
