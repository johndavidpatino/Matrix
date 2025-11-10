<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRRHH.master" CodeBehind="EmpleadosAdmin.aspx.vb" Inherits="WebMatrix.Empleados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <link href="../css/TH_Empleados.css" rel="stylesheet">
    <script src="../Scripts/returnLoginFetchUnauthorized.js" type="text/javascript"></script>
    <script src="../Scripts/imageToTextBase64FromInputFile.js" type="text/javascript"></script>
    <script src="../Scripts/loadImageFromInputFileToTagImage.js" type="text/javascript"></script>

    <script>
        var intervalIdRight;
        var intervalIdLeft;
        var scrollLeftNow = 0;
        var idPersonSelected;

        function animateScroll(maxleft) {
            if (scrollLeftNow >= maxleft) {
                clearInterval(intervalIdLeft);
            }
            else {
                scrollLeftNow += 100;
                parentToScroll.scrollLeft = scrollLeftNow;
            }
        }
        function animateScrollLeft() {
            if (scrollLeftNow <= 0) {
                clearInterval(intervalIdRight);
            }
            else {
                scrollLeftNow -= 100;
                parentToScroll.scrollLeft = scrollLeftNow;
            }
        }

        function returnToPersons() {
            let element = document.getElementsByClassName('scroller')[0];
            let persons;
            intervalIdRight = setInterval(function () { animateScrollLeft(element) }, 25)
            document.getElementsByClassName('returnToPersons')[0].classList.toggle('showReturnToPersons');
            clearPersons();
            getEmpleados();
        }
        function loadImage(input) {
            let img = document.querySelector('.containerForms img');
            loadImageFromInputFileToTagImage(input, img);
        }

        function getEmpleados() {

            clearPersons();

            let formData = new Object();
            formData.id = document.getElementById('filterIdentificacion').value == "" ? null : document.getElementById('filterIdentificacion').value;
            formData.nombres = document.getElementById('filterNombres').value == "" ? null : document.getElementById('filterNombres').value;
            formData.apellidos = document.getElementById('filterApellidos').value == "" ? null : formData.apellidos = document.getElementById('filterApellidos').value;
            formData.activo = document.getElementById('filterActivo').checked;
            formData.areaServiceLine = document.getElementById('filterAreaServiceLine').value == "" ? null : document.getElementById('filterAreaServiceLine').value;
            formData.cargo = document.getElementById('filterCargo').value == "" ? null : document.getElementById('filterCargo').value;
            formData.sede = document.getElementById('filterSede').value == "" ? null : document.getElementById('filterSede').value;


            fetch('EmpleadosAdmin.aspx/getEmpleados', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => drawEmpleados(data.d));
        }
        function getEmpleado(identificacion) {
            let formData = new Object();

            formData.identificacion = identificacion;

            return fetch('EmpleadosAdmin.aspx/getEmpleadoPorIdentificacion', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getExperienciasLaboralesPorIdentificacion(identificacion) {
            let formData = new Object();

            formData.identificacion = identificacion;

            return fetch('EmpleadosAdmin.aspx/getExperienciasLaboralesPorIdentificacion', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getEducacion(identificacion) {
            let formData = new Object();

            formData.identificacion = identificacion;

            return fetch('EmpleadosAdmin.aspx/getEducacionPorIdentificacion', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getHijos(identificacion) {
            let formData = new Object();

            formData.identificacion = identificacion;

            return fetch('EmpleadosAdmin.aspx/getHijosPorIdentificacion', {
                method: 'POST',
                body: JSON.stringify(formData),
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
        function getPromociones(identificacion) {
            let formData = new Object();

            formData.identificacion = identificacion;

            return fetch('EmpleadosAdmin.aspx/getPromocionesPorIdentificacion', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getSalarios(identificacion) {
            let formData = new Object();

            formData.identificacion = identificacion;

            return fetch('EmpleadosAdmin.aspx/getSalariosPorIdentificacion', {
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
											<i class="material-icons" title="Fecha de cumpleaños">cake</i>
											<span> ${element.FechaNacimiento != null ? formatMillisecondsToColombiaDate(element.FechaNacimiento) : ""}</label>
										</div>
										<div>
											<i class="material-icons" title="Correo Ipsos">mail</i>
											<span> ${element.correoIpsos}</label>
										</div>
										<div>
											<i class="material-icons" title="Número de contacto">phone_iphone</i>
											<span> ${element.Celular}</span>
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
										<div>
											<i class="material-icons" title="% diligenciamiento">battery_charging_full</i>
											<span> ${element.PorcentajeDiligenciamiento}%</span>
										</div>
									</div>
									<div>
										<button type="button" data-active=${element.Activo} data-Apellidos="${element.Apellidos}" data-Nombres="${element.Nombres}" data-id=${element.id}>${element.Activo ? `Desativar` : `Activar`}</button>
									</div>
									<input type="hidden" value="${element.id}" />
								</div>`;
                containerCardsPerson.insertAdjacentHTML("beforeend", rawHtml);
            });
            var elements = Array.from(document.querySelectorAll('.cardPerson .info'));
            var elementsButtonsActiveInactive = Array.from(document.querySelectorAll('.cardPerson button'));
            elements.forEach((element, index) => element.onclick = function () { showPerson(element.parentElement) });
            elementsButtonsActiveInactive.forEach((element, index) => element.onclick = function () { showActiveInactivePerson(element) });
        }
        function drawExperienciasLaborales(experienciasLaborales) {
            let containerExperienciaLaboral = document.getElementsByClassName('containerExperienciaLaboral')[0];

            experienciasLaborales.forEach((element, index) => {
                let rawHtml = `<div class="experienciaLaboral">
								<input type="hidden" value="${element.Id}"></input>
								<i class="material-icons iconDeleteExperienciaLaboral" title="Borrar experiencia">close</i>
								<div>
									<span>${element.empresa}</span>
								</div>
								<div>
									<span>${element.cargo}</span>
								</div>
								<div>
									<span>${formatMillisecondsToColombiaDate(element.fechaInicio)}</span>
								</div>
								<div>
									<span>${formatMillisecondsToColombiaDate(element.fechaFin)}</span>
								</div>
								${element.esInvestigacion ?
                        `<div>
									<span>Empresa investigación</span>
								</div>`
                        :
                        ``}
							</div>`;
                containerExperienciaLaboral.insertAdjacentHTML("beforeend", rawHtml);
                iconDeleteExperienciaLaboral = Array.from(document.getElementsByClassName('iconDeleteExperienciaLaboral'));
                iconDeleteExperienciaLaboral.forEach((element, index) => element.onclick = function () { deleteExperienciaLaboral(element.parentElement) });
            });
        }
        function drawEducacion(formaciones) {
            let containerEducacion = document.getElementsByClassName('containerEducacion')[0];
            formaciones.forEach((element, index) => {
                let rawHtml = `<div class="educacion">
									<i class="material-icons iconDeleteEducacion" title="Borrar educación">close</i>
									<div>
										<span>Titulo:</span>
										<span>${element.titulo}</span>
									</div>
									<div>
										<span>Institución:</span>
										<span>${element.institucion}</span>
									</div>
									<div>
										<span>Modalidad:</span>
										<span>${element.modalidadTxt}</span>
									</div>
									<div>
										<span>Tipo:</span>
										<span>${element.tipoTxt}</span>
									</div>
									<div>
										<span>País:</span>
										<span>${element.pais}</span>
									</div>
									<div>
										<span>Ciudad:</span>
										<span>${element.ciudad}</span>
									</div>
									<div>
										<span>Fecha Inicio:</span>
										<span>${formatMillisecondsToColombiaDate(element.fechaInicio)}</span>
									</div>
									<div>
										<span>Fecha Fin:</span>
										<span>${element.fechaFin != null ? formatMillisecondsToColombiaDate(element.fechaFin) : ""}</span>
									</div>
									<div>
										<span>Estado:</span>
										<span>${element.estadoTxt}</span>
									</div>
									<input type="hidden" value="${element.Id}" />
								</div>`;
                containerEducacion.insertAdjacentHTML("beforeend", rawHtml);
                iconDeleteEducacion = Array.from(document.getElementsByClassName('iconDeleteEducacion'));
                iconDeleteEducacion.forEach((element, index) => element.onclick = function () { deleteEducacion(element.parentElement) });
            });
        }
        function drawHijos(hijos) {
            let containerHijos = document.getElementsByClassName('containerHijos')[0];
            hijos.forEach((element, index) => {
                let rawHtml = `<div class="hijo">
									<i class="material-icons iconDeleteHijo" title="Borrar hijo">close</i>
									<div>
										<span>Nombres:</span>
										<span>${element.nombres}</span>
									</div>
									<div>
										<span>Apellidos:</span>
										<span>${element.apellidos}</span>
									</div>
									<div>
										<span>Genero:</span>
										<span>${element.generoTxt}</span>
									</div>
									<div>
										<span>Fecha nacimiento:</span>
										<span>${element.fechaNacimiento != null ? formatMillisecondsToColombiaDate(element.fechaNacimiento) : ""}</span>
									</div>
									<div>
										<span>Edad:</span>
										<span>${calculateAgeFromBirthdate(getDateFromMilliseconds(element.fechaNacimiento))}</span>
									</div>
									<input type="hidden" value="${element.id}" />
								</div>`;
                containerHijos.insertAdjacentHTML("beforeend", rawHtml);
                iconDeleteHijo = Array.from(document.getElementsByClassName('iconDeleteHijo'));
                iconDeleteHijo.forEach((element, index) => element.onclick = function () { deleteHijo(element.parentElement) });
            });
        }
        function drawContactosEmergencia(contactosEmergencia) {
            let containerContactosEmergencia = document.getElementsByClassName('containerContactosEmergencia')[0];
            contactosEmergencia.forEach((element, index) => {
                let rawHtml = `<div class="contactoEmergencia">
									<i class="material-icons iconDeleteContactoEmergencia" title="Borrar contacto de emergencia">close</i>
									<div>
										<span>Nombres:</span>
										<span>${element.nombres}</span>
									</div>
									<div>
										<span>Apellidos:</span>
										<span>${element.apellidos}</span>
									</div>
									<div>
										<span>Parentesco:</span>
										<span>${element.parentescoTxt}</span>
									</div>
									<div>
										<span>Telefono fijo:</span>
										<span>${element.telefonoFijo}</span>
									</div>
									<div>
										<span>Telefono celular:</span>
										<span>${element.telefonoCelular}</span>
									</div>
									<input type="hidden" value="${element.id}" />
								</div>`;
                containerContactosEmergencia.insertAdjacentHTML("beforeend", rawHtml);
                iconDeleteContactoEmergencia = Array.from(document.getElementsByClassName('iconDeleteContactoEmergencia'));
                iconDeleteContactoEmergencia.forEach((element, index) => element.onclick = function () { deleteContactoEmergencia(element.parentElement) });
            });
        }
        function drawPromociones(promociones) {
            let containerPromociones = document.getElementsByClassName('containerPromociones')[0];
            promociones.forEach((element, index) => {
                let rawHtml = `<div class="promocion">
									<i class="material-icons iconDeletePromocion" title="Borrar promoción">close</i>
									<div>
										<span>Fecha:</span>
										<span>${formatMillisecondsToColombiaDate(element.fechaPromocion)}</span>
									</div>
									<div>
										<span>Banda:</span>
										<span>${element.bandaTxt}</span>
									</div>
									<div>
										<span>Level:</span>
										<span>${element.levelTxt}</span>
									</div >
									<div>
										<span>Service line / Area:</span>
										<span>${element.areaTxt}</span>
									</div >
									<div>
										<span>Cargo:</span>
										<span>${element.cargoTxt}</span>
									</div >
									<input type="hidden" value="${element.id}" />
								</div>`;
                containerPromociones.insertAdjacentHTML("beforeend", rawHtml);
                iconDeletePromocion = Array.from(document.getElementsByClassName('iconDeletePromocion'));
                iconDeletePromocion.forEach((element, index) => element.onclick = function () { deletePromocion(element.parentElement) });
            });
        }
        function drawSalarios(salarios) {
            let containerSalarios = document.getElementsByClassName('containerSalarios')[0];
            salarios.forEach((element, index) => {
                let rawHtml = `<div class="salario">
									<i class="material-icons iconDeleteSalario" title="Borrar salario">close</i>
									<div>
										<span>Fecha aplicación:</span>
										<span>${formatMillisecondsToColombiaDate(element.fechaAplicacion)}</span>
									</div>
									<div>
										<span>Motivo cambio:</span>
										<span>${element.motivoCambioTxt}</span>
									</div>
									<div>
										<span>Tipo:</span>
										<span>${element.tipoSalarioTxt}</span>
									</div>
									<div>
										<span>Salario:</span>
										<span>${formatNumberToColombiaCurrency(element.salario)}</span>
									</div>
									<input type="hidden" value="${element.id}" />
								</div > `;
                containerSalarios.insertAdjacentHTML("beforeend", rawHtml);
                iconDeleteSalario = Array.from(document.getElementsByClassName('iconDeleteSalario'));
                iconDeleteSalario.forEach((element, index) => element.onclick = function () { deleteSalario(element.parentElement) });
            });
        }

        async function showPerson(element) {
            intervalIdLeft = setInterval(function () { animateScroll(1000) }, 25)
            document.getElementsByClassName('returnToPersons')[0].classList.toggle('showReturnToPersons');
            window.scrollTo({ top: 0 });
            idPersonSelected = element.getElementsByTagName('input').length > 0 ? element.getElementsByTagName('input')[0].value : null;
            clearFormPerson();
            clearExperienciasLaborales();
            clearFormEducacion();
            clearEducacion();
            clearFormHijo();
            clearHijos();
            clearFormContactoEmergencia();
            clearContactosEmergencia();
            clearFormPromocion();
            clearPromociones();
            clearSalarios();
            clearFormSalario();

            if (idPersonSelected) {
                let empleado;
                let experienciasLaborales;
                let formaciones;
                let hijos;
                let contactosEmergencia;
                let promociones;
                let salarios;

                empleado = await getEmpleado(idPersonSelected);
                loadEmpleado(empleado);
                experienciasLaborales = await getExperienciasLaboralesPorIdentificacion(idPersonSelected);
                drawExperienciasLaborales(experienciasLaborales);
                formaciones = await getEducacion(idPersonSelected);
                drawEducacion(formaciones);
                hijos = await getHijos(idPersonSelected);
                drawHijos(hijos);
                contactosEmergencia = await getContactosEmergencia(idPersonSelected);
                drawContactosEmergencia(contactosEmergencia);
                promociones = await getPromociones(idPersonSelected);
                drawPromociones(promociones);
                salarios = await getSalarios(idPersonSelected);
                drawSalarios(salarios);
            }
        }
        function showActiveInactivePerson(element) {
            window.scrollTo({ top: 0 });
            intervalIdLeft = setInterval(function () { animateScroll(3000) }, 25)
            document.getElementsByClassName('returnToPersons')[0].classList.toggle('showReturnToPersons');
            document.getElementsByClassName('namesPerson')[1].innerHTML = element.dataset.apellidos + ', ' + element.dataset.nombres;
            idPersonSelected = element.dataset.id;

            if (element.dataset.active == "true")
                showRetirarPersona();
            else
                showReintegrarPersona();
        }

        function loadEmpleado(empleado) {
            let birthdate;
            birthdate = empleado.FechaNacimiento != null ? getDateFromMilliseconds(empleado.FechaNacimiento) : null;

            document.getElementsByClassName('namesPerson')[0].innerHTML = empleado.Apellidos + ', ' + empleado.Nombres;

            document.querySelector('.containerForms img').src = empleado.urlFoto != null ? empleado.urlFoto : '../Images/sin-foto.jpg'

            document.getElementById('tipoIdentificacion').value = empleado.TipoId;
            document.getElementById('identificacion').value = empleado.id;
            document.getElementById('nombres').value = empleado.Nombres;
            document.getElementById('apellidos').value = empleado.Apellidos;
            document.getElementById('nombrePreferido').value = empleado.nombrePreferido;
            document.getElementById('fechaNacimiento').value = formatDateToyyyyMMdd(birthdate);;
            document.getElementById('edad').value = calculateAgeFromBirthdate(birthdate);
            document.getElementById('genero').value = empleado.Sexo;
            document.getElementById('estadoCivil').value = empleado.EstadoCivil;
            document.getElementById('grupoSanguineo').value = empleado.GrupoSanguineo;
            document.getElementById('nacionalidad').value = empleado.Nacionalidad;

            document.getElementById('employeedId').value = empleado.IdIStaff;
            document.getElementById('jefeInmediato').value = empleado.JefeInmediato;
            document.getElementById('sede').value = empleado.Sede;
            document.getElementById('correoIpsos').value = empleado.correoIpsos;
            document.getElementById('centroCosto').value = empleado.centroCostoId;
            document.getElementById('fechaIngresoIpsos').value = empleado.FechaIngreso != null ? formatMillisecondsToyyyyMMdd(empleado.FechaIngreso) : null;;
            document.getElementById('tipoContrato').value = empleado.TipoContratacion;
            document.getElementById('tiempoContrato').value = empleado.tiempoContratoId;
            document.getElementById('jobFunction').value = empleado.jobFunctionId;
            document.getElementById('observaciones').value = empleado.observaciones;

            document.getElementById('empresa').value = empleado.Empresa;
            mostrarTiempoContrato();


            document.getElementById('banco').value = empleado.Banco;
            document.getElementById('tipoCuenta').value = empleado.TipoCuenta;
            document.getElementById('numeroCuenta').value = empleado.CuentaBanco;
            document.getElementById('eps').value = empleado.EPS;
            document.getElementById('fondoPension').value = empleado.FondoPensiones;
            document.getElementById('fondoCesantias').value = empleado.FondoCesantias;
            document.getElementById('cajaCompensacion').value = empleado.CajaCompensacion;
            document.getElementById('arl').value = empleado.ARL;

            document.getElementById('ciudad').value = empleado.CiudadId;
            document.getElementById('direccion').value = empleado.Direccion;
            document.getElementById('barrio').value = empleado.BarrioResidencia;
            document.getElementById('localidad').value = empleado.localidadId;
            document.getElementById('nse').value = empleado.nseId;
            document.getElementById('telefonoFijo').value = empleado.Telefono1;
            document.getElementById('telefonoCelular').value = empleado.Celular;
            document.getElementById('emailPersonal').value = empleado.EmailPersonal;
            document.getElementById('tallaCamiseta').value = empleado.TallaCamisetaId;
            document.getElementById('municipioNacimiento').value = empleado.municipioNacimientoDivipolaId;
            fillSelectDepartamentoNacimiento(document.getElementById('municipioNacimiento').value)

            mostrarLocalidad();

            document.getElementById('nivelIngles').value = empleado.nivelInglesId;
        }
        function OnChangeDdlMunicipioNacimiento(e) {
            let element = e.target;
            let value = element.value;

            fillSelectDepartamentoNacimiento(value)
        }

        function deleteExperienciaLaboral(element) {
            let formData = Object();

            formData.identificacion = element.getElementsByTagName('input')[0].value;

            fetch('EmpleadosAdmin.aspx/deleteExperienciaLaboral', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(async data => {
                    clearExperienciasLaborales();
                    let experienciasLaborales = await getExperienciasLaboralesPorIdentificacion(idPersonSelected);
                    drawExperienciasLaborales(experienciasLaborales);
                    alert('Eliminado correctamente');
                });
        }
        function deleteEducacion(element) {
            let formData = Object();

            formData.identificacion = element.getElementsByTagName('input')[0].value;

            fetch('EmpleadosAdmin.aspx/deleteEducacion', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(async data => {
                    let formaciones;
                    clearEducacion();
                    formaciones = await getEducacion(idPersonSelected);
                    drawEducacion(formaciones);
                    clearFormEducacion();
                    alert('Eliminado correctamente');
                });
        }
        function deleteHijo(element) {
            let formData = Object();

            formData.id = element.getElementsByTagName('input')[0].value;

            fetch('EmpleadosAdmin.aspx/deleteHijo', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(async data => {
                    let hijos;
                    clearHijos();
                    hijos = await getHijos(idPersonSelected);
                    drawHijos(hijos);
                    clearFormHijo();
                    alert('Eliminado correctamente');
                });
        }
        function deleteContactoEmergencia(element) {
            let formData = Object();

            formData.id = element.getElementsByTagName('input')[0].value;

            fetch('EmpleadosAdmin.aspx/deleteContactoEmergencia', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(async data => {
                    let contactosEmergencia;
                    clearContactosEmergencia();
                    contactosEmergencia = await getContactosEmergencia(idPersonSelected);
                    drawContactosEmergencia(contactosEmergencia);
                    clearFormContactoEmergencia();
                    alert('Eliminado correctamente');
                });
        }
        function deletePromocion(element) {
            let formData = Object();

            formData.id = element.getElementsByTagName('input')[0].value;

            fetch('EmpleadosAdmin.aspx/deletePromocion', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(async data => {
                    let promociones;
                    clearPromociones();
                    promociones = await getPromociones(idPersonSelected);
                    drawPromociones(promociones);
                    clearFormPromocion();
                    alert('Eliminado correctamente');
                });
        }
        function deleteSalario(element) {
            let formData = Object();

            formData.id = element.getElementsByTagName('input')[0].value;

            fetch('EmpleadosAdmin.aspx/deleteSalario', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(async data => {
                    let salarios;
                    let empleado;

                    clearSalarios();
                    salarios = await getSalarios(idPersonSelected);
                    drawSalarios(salarios);
                    clearFormSalario();

                    empleado = await getEmpleado(idPersonSelected);
                    loadEmpleado(empleado);

                    alert('Eliminado correctamente');
                });
        }

        function addExperienciaLaboral() {
            if (isValidForm('experienciaLaboralNueva')) {
                let formData = new Object();
                formData.identificacion = idPersonSelected;
                formData.empresa = document.getElementById('nombreEmpresa').value;
                formData.fechaInicio = document.getElementById('FechaInicioExperiencia').value;
                formData.fechaFin = document.getElementById('FechaFinExperiencia').value;
                formData.cargo = document.getElementById('experienciaLaboralCargo').value;
                formData.esInvestigacion = document.getElementById('esInvestigacion').checked;
                fetch('EmpleadosAdmin.aspx/addExperienciaLaboral', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(result => result.json())
                    .then(async data => {
                        let experienciasLaborales;
                        clearExperienciasLaborales();
                        experienciasLaborales = await getExperienciasLaboralesPorIdentificacion(idPersonSelected);
                        drawExperienciasLaborales(experienciasLaborales);
                        drawExperienciaLaboralIpsosUsuarioActivo();
                        clearFormExperienciaLaboral();
                        alert('Adicionado correctamente');
                    });
            }
        }
        function addEducacion() {
            if (isValidForm('formEducacionNueva')) {
                let formData = new Object();
                formData.identificacion = idPersonSelected;
                formData.tipo = document.getElementById('tipoEducacion').value;
                formData.titulo = document.getElementById('tituloEducacion').value;
                formData.institucion = document.getElementById('institucionEducacion').value;
                formData.pais = document.getElementById('paisEducacion').value;
                formData.ciudad = document.getElementById('ciudadEducacion').value;
                formData.fechaInicio = document.getElementById('fechaInicioEducacion').value;
                formData.fechaFin = document.getElementById('fechaFinEducacion').value;
                formData.modalidad = document.getElementById('modalidadEducacion').value;
                formData.estado = document.getElementById('estadoEducacion').value;

                fetch('EmpleadosAdmin.aspx/addEducacion', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(result => result.json())
                    .then(async data => {
                        let formaciones;
                        clearEducacion();
                        formaciones = await getEducacion(idPersonSelected);
                        drawEducacion(formaciones);
                        clearFormEducacion();
                        alert('Adicionado correctamente');
                    });
            }
        }
        function addHijo() {
            if (isValidForm('formHijoNuevo')) {
                let formData = new Object();
                formData.personaId = idPersonSelected;
                formData.nombres = document.getElementById('nombresHijo').value;
                formData.apellidos = document.getElementById('apellidosHijo').value;
                formData.genero = document.getElementById('generoHijo').value;
                formData.fechaNacimiento = document.getElementById('fechaNacimientoHijo').value;

                fetch('EmpleadosAdmin.aspx/addHijo', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(result => result.json())
                    .then(async data => {
                        let hijos;
                        clearHijos();
                        hijos = await getHijos(idPersonSelected);
                        drawHijos(hijos);
                        clearFormHijo();
                        alert('Adicionado correctamente');
                    });
            }
        }
        function addContactoEmergencia() {
            if (isValidForm('formContactoEmergenciaNuevo')) {
                let formData = new Object();

                formData.personaId = idPersonSelected;
                formData.nombres = document.getElementById('nombresContactoEmergencia').value;
                formData.apellidos = document.getElementById('apellidosContactoEmergencia').value;
                formData.parentesco = document.getElementById('parentescoContactoEmergencia').value;
                formData.telefonoFijo = document.getElementById('telefonoFijoContactoEmergencia').value;
                formData.telefonoCelular = document.getElementById('telefonoCelularContactoEmergencia').value;

                fetch('EmpleadosAdmin.aspx/addContactoEmergencia', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(result => result.json())
                    .then(async data => {
                        let contactosEmergencia;
                        clearContactosEmergencia();
                        contactosEmergencia = await getContactosEmergencia(idPersonSelected);
                        drawContactosEmergencia(contactosEmergencia);
                        clearFormContactoEmergencia();
                        alert('Adicionado correctamente');
                    });
            }
        }
        function addPromocion() {

            if (isValidForm('formPromocionNueva')) {
                let formData = new Object();

                formData.personaId = idPersonSelected;
                formData.fechaPromocion = document.getElementById('fechaPromocion').value;
                formData.nuevaBandaId = document.getElementById('nuevaBandaPromocion').value;
                formData.nuevaAreaId = document.getElementById('nuevaAreaPromocion').value;
                formData.nuevoCargoId = document.getElementById('nuevoCargoPromocion').value;
                formData.nuevoLevelId = document.getElementById('nuevoLevelPromocion').value;

                fetch('EmpleadosAdmin.aspx/addPromocion', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(result => result.json())
                    .then(async data => {
                        let promociones;
                        clearPromociones();
                        promociones = await getPromociones(idPersonSelected);
                        drawPromociones(promociones);
                        clearFormPromocion();
                        alert('Adicionado correctamente');
                    });
            }
        }
        function addSalario() {
            if (isValidForm('formSalarioNuevo')) {
                let formData = new Object();
                formData.personaId = idPersonSelected;
                formData.fechaAplicacion = document.getElementById('fechaAplicacionSalario').value;
                formData.motivoCambio = document.getElementById('motivoCambioSalario').value;
                formData.salario = document.getElementById('nuevoSalario').value;
                formData.tipo = document.getElementById('nuevoTipoSalario').value;

                fetch('EmpleadosAdmin.aspx/addSalario', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(result => result.json())
                    .then(async data => {
                        let salarios;
                        clearSalarios();
                        salarios = await getSalarios(idPersonSelected);
                        drawSalarios(salarios);
                        clearFormSalario();
                        alert('Adicionado correctamente');
                    });
            }
        }

        function retirarPersona() {
            if (isValidForm('formRetirarPersona')) {
                let formData = new Object();
                formData.identificacion = idPersonSelected;
                formData.fechaRetiro = document.getElementById('fechaRetiro').value;
                formData.observacion = document.getElementById('observacionRetiro').value;

                fetch('EmpleadosAdmin.aspx/retirarEmpleado', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(result => result.json())
                    .then(async data => {
                        /*let salarios;
                        clearSalarios();
                        salarios = await getSalarios(idPersonSelected);
                        drawSalarios(salarios);
                        clearFormSalario();*/
                        alert('Adicionado correctamente');
                    });
            }
        }
        function reintegrarPersona() {
            if (isValidForm('formReintegrarPersona')) {
                let formData = new Object();
                formData.identificacion = idPersonSelected;
                formData.fechaReintegro = document.getElementById('fechaReintegro').value;

                fetch('EmpleadosAdmin.aspx/reintegrarEmpleado', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(result => result.json())
                    .then(async data => {
                        /*let salarios;
                        clearSalarios();
                        salarios = await getSalarios(idPersonSelected);
                        drawSalarios(salarios);
                        clearFormSalario();*/
                        alert('Adicionado correctamente');
                    });
            }
        }

        async function updateDatosGenerales() {

            if (isValidForm('formDatosGenerales')) {
                let formData = new Object();
                let inputFoto = document.querySelector('.custom-file-upload input');

                formData.tipoId = document.getElementById('tipoIdentificacion').value;
                formData.id = idPersonSelected == null ? document.getElementById('identificacion').value : idPersonSelected;
                formData.esNuevo = idPersonSelected == null ? true : false;
                formData.nombres = document.getElementById('nombres').value;
                formData.apellidos = document.getElementById('apellidos').value;
                formData.nombrePreferido = document.getElementById('nombrePreferido').value;
                formData.fechaNacimiento = document.getElementById('fechaNacimiento').value;
                formData.sexo = document.getElementById('genero').value;
                formData.estadoCivil = document.getElementById('estadoCivil').value;
                formData.grupoSanguineo = document.getElementById('grupoSanguineo').value;
                formData.nacionalidad = document.getElementById('nacionalidad').value;
                formData.fotoBase64 = inputFoto.files.length == 0 ? null : await imageToTextBase64FromInputFile(inputFoto.files[0]);
                fetch('EmpleadosAdmin.aspx/updateDatosGenerales', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(data => {
                        if (data.status == 200) {
                            idPersonSelected = idPersonSelected == null ? document.getElementById('identificacion').value : idPersonSelected;
                            alert('Actualizado con exito');
                        }
                    });
            }
        }
        async function updateDatosLaborales() {

            validateEmployeedIdRequired();

            if (isValidForm('formDatosLaborales')) {

                let formData = new Object();

                formData.id = idPersonSelected;
                formData.idIStaff = document.getElementById('employeedId').value;
                formData.jefeInmediato = document.getElementById('jefeInmediato').value;
                formData.sede = document.getElementById('sede').value;
                formData.correoIpsos = document.getElementById('correoIpsos').value;
                formData.fechaIngreso = document.getElementById('fechaIngresoIpsos').value;
                formData.centroCosto = document.getElementById('centroCosto').value;
                formData.tipoContratoId = document.getElementById('tipoContrato').value;
                formData.tiempoContratoId = document.getElementById('tiempoContrato').value;
                formData.empresa = document.getElementById('empresa').value;
                formData.jobFunctionId = document.getElementById('jobFunction').value;
                formData.observaciones = document.getElementById('observaciones').value;

                fetch('EmpleadosAdmin.aspx/updateDatosLaborales', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(data => {
                        if (data.status == 200)
                            alert('Actualizado con exito');
                    });
            }
        }
        async function updateDatosPersonales() {
            if (isValidForm('formDatosPersonales')) {
                let formData = new Object();

                formData.id = idPersonSelected;
                formData.ciudadId = document.getElementById('ciudad').value;
                formData.direccion = document.getElementById('direccion').value;
                formData.barrio = document.getElementById('barrio').value;
                formData.nseId = document.getElementById('nse').value;
                formData.telefonoFijo = document.getElementById('telefonoFijo').value;
                formData.telefonoCelular = document.getElementById('telefonoCelular').value;
                formData.emailPersonal = document.getElementById('emailPersonal').value;
                formData.localidad = document.getElementById('localidad').value != "" ? document.getElementById('localidad').value : null;
                formData.municipioNacimientoId = document.getElementById('municipioNacimiento').value
                formData.tallaCamisetaId = document.getElementById('tallaCamiseta').value


                fetch('EmpleadosAdmin.aspx/updateDatosPersonales', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(data => {
                        if (data.status == 200)
                            alert('Actualizado con exito');
                    });
            }
        }
        async function updateNomina() {
            if (isValidForm('formNomina')) {
                let formData = new Object();

                formData.id = idPersonSelected;
                formData.bancoId = document.getElementById('banco').value;
                formData.tipoCuentaId = document.getElementById('tipoCuenta').value;
                formData.numeroCuenta = document.getElementById('numeroCuenta').value;
                formData.EPSId = document.getElementById('eps').value;
                formData.fondoPensionesId = document.getElementById('fondoPension').value;
                formData.fondoCesantiasId = document.getElementById('fondoCesantias').value;
                formData.cajaCompensacionId = document.getElementById('cajaCompensacion').value;
                formData.ARLId = document.getElementById('arl').value;

                fetch('EmpleadosAdmin.aspx/updateNomina', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(data => {
                        if (data.status == 200)
                            alert('Actualizado con exito');
                    });
            }
        }
        async function updateNivelIngles() {
            if (isValidForm('formIngles')) {
                let formData = new Object();

                formData.id = idPersonSelected;
                formData.nivelInglesId = document.getElementById('nivelIngles').value;


                fetch('EmpleadosAdmin.aspx/updateNivelIngles', {
                    method: 'POST',
                    body: JSON.stringify(formData),
                    headers: {
                        'Content-Type': 'application/json'
                    }
                })
                    .then(data => {
                        if (data.status == 200)
                            alert('Actualizado con exito');
                    });
            }
        }

        function clearExperienciasLaborales() {
            let containerExperienciaLaboral = document.getElementsByClassName('containerExperienciaLaboral')[0];
            containerExperienciaLaboral.innerHTML = '';
        }
        function clearEducacion() {
            let containerEducacion = document.getElementsByClassName('containerEducacion')[0];
            containerEducacion.innerHTML = '';
        }
        function clearPersons() {
            let cardsPersons = Array.from(document.getElementsByClassName('cardPerson'));
            cardsPersons.splice(0, 1);
            cardsPersons.forEach(element => element.remove());
        }
        function clearHijos() {
            let containerHijos = document.getElementsByClassName('containerHijos')[0];
            containerHijos.innerHTML = '';
        }
        function clearContactosEmergencia() {
            let containerContactosEmergencia = document.getElementsByClassName('containerContactosEmergencia')[0];
            containerContactosEmergencia.innerHTML = '';
        }
        function clearPromociones() {
            let containerPromociones = document.getElementsByClassName('containerPromociones')[0];
            containerPromociones.innerHTML = '';
        }
        function clearSalarios() {
            let containerSalarios = document.getElementsByClassName('containerSalarios')[0];
            containerSalarios.innerHTML = '';
        }

        function clearFormPerson() {
            let elements = Array.from(document.querySelectorAll('.person input,.person select,.person textarea'));
            elements.forEach((element) => {
                element.value = '';
            });
            document.getElementsByClassName('namesPerson')[0].innerHTML = '';
            document.querySelector('.person img').src = "../Images/sin-foto.jpg";
        }
        function clearFormExperienciaLaboral() {
            let elements = Array.from(document.querySelectorAll('.experienciaLaboralNueva input,.experienciaLaboralNueva select,.experienciaLaboralNueva textarea'));
            elements.forEach((element) => {
                element.value = '';
            });
        }
        function clearFormEducacion() {
            let elements = Array.from(document.querySelectorAll('.formEducacionNueva input,.formEducacionNueva select,.formEducacionNueva textarea'));
            elements.forEach((element) => {
                element.value = '';
            });
        }
        function clearFormHijo() {
            let elements = Array.from(document.querySelectorAll('.formHijoNuevo input,.formHijoNuevo select,.formHijoNuevo textarea'));
            elements.forEach((element) => {
                element.value = '';
            });
        }
        function clearFormContactoEmergencia() {
            let elements = Array.from(document.querySelectorAll('.formContactoEmergenciaNuevo input,.formContactoEmergenciaNuevo select,.formContactoEmergenciaNuevo textarea'));
            elements.forEach((element) => {
                element.value = '';
            });
        }
        function clearFormPromocion() {
            let elements = Array.from(document.querySelectorAll('.formPromocionNueva input,.formPromocionNueva select,.formPromocionNueva textarea'));
            elements.forEach((element) => {
                element.value = '';
            });
        }
        function clearFormSalario() {
            let elements = Array.from(document.querySelectorAll('.formSalarioNuevo input,.formSalarioNuevo select,.formSalarioNuevo textarea'));
            elements.forEach((element) => {
                element.value = '';
            });
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
        function getGruposSanguineos() {
            return fetch('EmpleadosAdmin.aspx/getGruposSanguineos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getEstadosCiviles() {
            return fetch('EmpleadosAdmin.aspx/getEstadosCiviles', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getJefesInmediatos() {
            return fetch('EmpleadosAdmin.aspx/getJefesInmediatos', {
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
        function getBandas() {
            return fetch('EmpleadosAdmin.aspx/getBandas', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getLevels() {
            return fetch('EmpleadosAdmin.aspx/getLevels', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiposEducacion() {
            return fetch('EmpleadosAdmin.aspx/getTiposEducacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getModalidadesEducacion() {
            return fetch('EmpleadosAdmin.aspx/getModalidadesEducacion', {
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
        function getCentrosCosto() {
            return fetch('EmpleadosAdmin.aspx/getCentrosCosto', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getEstadosEducacion() {
            return fetch('EmpleadosAdmin.aspx/getEstadosEducacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getParentescos() {
            return fetch('EmpleadosAdmin.aspx/getParentescos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getMotivosCambioSalario() {
            return fetch('EmpleadosAdmin.aspx/getMotivosCambioSalario', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiposContratacion() {
            return fetch('EmpleadosAdmin.aspx/getTiposContratacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiposIdentificacion() {
            return fetch('EmpleadosAdmin.aspx/getTiposIdentificacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiemposContrato() {
            return fetch('EmpleadosAdmin.aspx/getTiemposContrato', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiposSalario() {
            return fetch('EmpleadosAdmin.aspx/getTiposSalario', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getBancos() {
            return fetch('EmpleadosAdmin.aspx/getBancos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiposCuenta() {
            return fetch('EmpleadosAdmin.aspx/getTiposCuentaBancaria', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getEPS() {
            return fetch('EmpleadosAdmin.aspx/getEPS', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getFondosPensiones() {
            return fetch('EmpleadosAdmin.aspx/getFondosPensiones', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getFondosCesantias() {
            return fetch('EmpleadosAdmin.aspx/getFondosCesantias', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getCajasCompensacion() {
            return fetch('EmpleadosAdmin.aspx/getCajasCompensacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getARL() {
            return fetch('EmpleadosAdmin.aspx/getARL', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getCiudades() {
            return fetch('EmpleadosAdmin.aspx/getCiudades', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getNSE() {
            return fetch('EmpleadosAdmin.aspx/getNSE', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getLocalidades() {
            return fetch('EmpleadosAdmin.aspx/getLocalidades', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getMunicipios() {
            return fetch('EmpleadosAdmin.aspx/MunicipiosDivipola', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTallasCasmiseta() {
            return fetch('EmpleadosAdmin.aspx/TallasCamiseta', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getNivelesIdioma() {
            return fetch('EmpleadosAdmin.aspx/getNivelesIdioma', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getEmpresas() {
            return fetch('EmpleadosAdmin.aspx/getEmpresas', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getJobFunctions() {
            return fetch('EmpleadosAdmin.aspx/getJobFunctions', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }

        function fillSelectTiposIdentificacion(tiposIdentificacion) {
            let select = document.getElementById('tipoIdentificacion');

            tiposIdentificacion.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.Id;
                option.text = element.TipoIdentificacion;
                select.add(option);
            });

        }
        function fillSelectAreasServicesLines(areasServicesLines) {
            let select = document.getElementById('areaServiceLine');

            areasServicesLines.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.Area;
                select.add(option);
            });

        }
        function fillSelectAreasServicesLinesPromocion(areasServicesLines) {
            let select = document.getElementById('nuevaAreaPromocion');

            areasServicesLines.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.Area;
                select.add(option);
            });

        }
        function fillSelectGruposSanguineos(gruposSanguineos) {
            let select = document.getElementById('grupoSanguineo');

            gruposSanguineos.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.GrupoSanguineo;
                select.add(option);
            });

        }
        function fillSelectCargos(cargos) {
            let select = document.getElementById('cargo');

            cargos.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.Cargo;
                select.add(option);
            });

        }
        function fillSelectCargosPromocion(cargos) {
            let select = document.getElementById('nuevoCargoPromocion');

            cargos.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.Cargo;
                select.add(option);
            });

        }
        function fillSelectEstadosCiviles(estadosCiviles) {
            let select = document.getElementById('estadoCivil');

            estadosCiviles.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.EstadoCivil;
                select.add(option);
            });

        }
        function fillSelectJefesInmediatos(jefesInmediatos) {
            let select = document.getElementById('jefeInmediato');

            jefesInmediatos.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.Item1;
                option.text = element.Item2;
                select.add(option);
            });

        }
        function fillSelectSedes(sedes) {
            let select = document.getElementById('sede');

            sedes.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.Sede;
                select.add(option);
            });

        }
        function fillSelectBandas(bandas) {
            let select = document.getElementById('banda');

            bandas.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.banda;
                select.add(option);
            });

        }
        function fillSelectBandasPromocion(bandas) {
            let select = document.getElementById('nuevaBandaPromocion');

            bandas.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.banda;
                select.add(option);
            });

        }
        function fillSelectNuevoLevels(levels) {
            let select = document.getElementById('nuevoLevelPromocion');

            levels.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.level;
                select.add(option);
            });

        }
        function fillSelectTiposEducacion(tiposEducacion) {
            let select = document.getElementById('tipoEducacion');

            tiposEducacion.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.tipo;
                select.add(option);
            });

        }
        function fillSelectBUITalent(BUITalent) {
            let select = document.getElementById('centroCosto');

            BUITalent.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = `${element.id} - ${element.CentroDeCosto} `;
                select.add(option);
            });
        }
        function fillSelectModalidadesEducacion(modalidadesEducacion) {
            let select = document.getElementById('modalidadEducacion');

            modalidadesEducacion.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.modalidad;
                select.add(option);
            });

        }
        function fillSelectEstadosEducacion(estadosEducacion) {
            let select = document.getElementById('estadoEducacion');

            estadosEducacion.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.Id;
                option.text = element.EstadoEducacion;
                select.add(option);
            });

        }
        function fillSelectParentescos(parentescos) {
            let select = document.getElementById('parentescoContactoEmergencia');

            parentescos.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.parentesco;
                select.add(option);
            });

        }
        function fillSelectMotivosCambioSalario(motivosCambioSalario) {
            let select = document.getElementById('motivoCambioSalario');

            motivosCambioSalario.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.motivo;
                select.add(option);
            });

        }
        function fillSelectTiposContrato(tiposContratacion) {
            let select = document.getElementById('tipoContrato');

            tiposContratacion.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.Tipo;
                select.add(option);
            });

        }
        function fillSelectTiemposContrato(tiemposContrato) {
            let select = document.getElementById('tiempoContrato');

            tiemposContrato.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.tiempoContrato;
                select.add(option);
            });

        }
        function fillSelectTiposSalario(tiposSalario) {
            let select = document.getElementById('tipoSalario');

            tiposSalario.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.TipoSalario;
                select.add(option);
            });

        }
        function fillSelectNuevoTiposSalario(tiposSalario) {
            let select = document.getElementById('nuevoTipoSalario');

            tiposSalario.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.TipoSalario;
                select.add(option);
            });

        }
        function fillSelectBancos(bancos) {
            let select = document.getElementById('banco');

            bancos.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = `${element.CodigoITalent} - ${element.Banco}`;
                select.add(option);
            });

        }
        function fillSelectTiposCuenta(tiposCuenta) {
            let select = document.getElementById('tipoCuenta');

            tiposCuenta.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.TipoCuenta;
                select.add(option);
            });

        }
        function fillSelectEPS(EPS) {
            let select = document.getElementById('eps');

            EPS.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.EPS;
                select.add(option);
            });

        }
        function fillSelectFondosPensiones(fondosPensiones) {
            let select = document.getElementById('fondoPension');

            fondosPensiones.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.FondoPensiones;
                select.add(option);
            });

        }
        function fillSelectFondosCesantias(fondosCesantias) {
            let select = document.getElementById('fondoCesantias');

            fondosCesantias.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.FondoCesantias;
                select.add(option);
            });

        }
        function fillSelectCajasCompensacion(cajasCompensacion) {
            let select = document.getElementById('cajaCompensacion');

            cajasCompensacion.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.CajaCompensacion;
                select.add(option);
            });

        }
        function fillSelectARL(ARL) {
            let select = document.getElementById('arl');

            ARL.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.ARL;
                select.add(option);
            });

        }
        function fillSelectCiudades(ciudades) {
            let select = document.getElementById('ciudad');

            ciudades.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.Ciudad;
                select.add(option);
            });

        }
        function fillSelectNSE(nse) {
            let select = document.getElementById('nse');

            nse.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.nse;
                select.add(option);
            });

        }
        function fillSelectLocalidades(localidades) {
            let select = document.getElementById('localidad');

            localidades.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.localidad;
                select.add(option);
            });

        }
        function fillSelectMunicipiosNacimiento(municipios) {
            let select = document.getElementById('municipioNacimiento');

            municipios.forEach((element, index) => {
                let option = document.createElement('option');
                option.dataset.departamentoid = element.DivDeptoMunicipio;
                option.dataset.departamentonombre = element.DivDeptoNombre;
                option.value = element.DivMunicipio;
                option.text = element.DivMuniNombre;
                select.add(option);
            });
        }
        function fillSelectDepartamentoNacimiento(municipioNacimientoId) {
            let ddlMunicipioNacimiento = document.getElementById('municipioNacimiento');
            let ddlDepartamentoNacimiento = document.getElementById('departamentoNacimiento');

            let option = Array.from(ddlMunicipioNacimiento.options).find(x => x.value == municipioNacimientoId);

            ddlDepartamentoNacimiento.remove(0)

            let optionDepartamento = document.createElement('option');
            optionDepartamento.value = option.dataset.departamentoid;
            optionDepartamento.text = option.dataset.departamentonombre;
            ddlDepartamentoNacimiento.add(optionDepartamento);
        }
        function fillSelectTallasCamiseta(tallasCamiseta) {
            let select = document.getElementById('tallaCamiseta');

            tallasCamiseta.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.TallaCamiseta;
                select.add(option);
            });
        }
        function fillSelectNivelesIdioma(nivelesIdioma) {
            let select = document.getElementById('nivelIngles');

            nivelesIdioma.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.nivel;
                select.add(option);
            });

        }
        function fillSelectEmpresa(empresas) {
            let select = document.getElementById('empresa');

            empresas.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.Empresa;
                select.add(option);
            });

        }
        function fillSelectJobFunctions(jobFunctions) {
            let select = document.getElementById('jobFunction');

            jobFunctions.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.jobFunction;
                select.add(option);
            });

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

        function fillInputEdad() {
            let edad;
            let fechaNacimiento;

            fechaNacimiento = new Date(document.getElementById('fechaNacimiento').value);

            edad = calculateAgeFromBirthdate(fechaNacimiento);

            document.getElementById('edad').value = edad;
        }

        function mostrarTiempoContrato() {
            const TIPO_CONTRATO_FIJO = 1;
            const TIPO_CONTRATO_APRENDIZAJE = 8;

            let tipoContrato;

            tipoContrato = document.getElementById('tipoContrato').value;

            if (tipoContrato == TIPO_CONTRATO_FIJO || tipoContrato == TIPO_CONTRATO_APRENDIZAJE) {
                document.getElementById('tiempoContrato').parentElement.style.display = 'inline';
            }
            else {
                document.getElementById('tiempoContrato').value = "";
                document.getElementById('tiempoContrato').parentElement.style.display = 'none';
            }
        }

        function mostrarLocalidad() {
            const COD_CIUDAD_BOGOTA = 11001;

            let ciudad;
            let localidad;

            ciudad = document.getElementById('ciudad').value;
            localidad = document.getElementById('localidad');

            if (ciudad == COD_CIUDAD_BOGOTA) {
                localidad.parentElement.style.display = 'inline';
            }
            else {
                localidad.value = "";
                localidad.parentElement.style.display = 'none';
            }
        }

        async function setupForms() {
            let tiposIdentificacion;
            let areasServiceLines;
            let gruposSanguineos;
            let cargos;
            let estadosCiviles;
            let jefesInmediatos;
            let sedes;
            let modalidadesEducacion;
            let tiposEducacion;
            let BUITalent;
            let estadosEducacion;
            let parentescos;
            let motivosCambioSalario;
            let tiposContrato;
            let tiemposContrato;
            let tiposSalario;
            let bancos;
            let tiposCuenta;
            let EPS;
            let fondosPensiones;
            let fondosCesantias;
            let cajasCompensacion;
            let ARL;
            let ciudades;
            let NSE;
            let localidades;
            let nivelesIdioma;
            let empresas;
            let levels;
            let jobFunctions;
            let municipios;
            let tallasCamiseta;

            tiposIdentificacion = await getTiposIdentificacion();
            fillSelectTiposIdentificacion(tiposIdentificacion);
            areasServiceLines = await getAreasServicesLine();
            fillSelectAreasServicesLinesPromocion(areasServiceLines);
            gruposSanguineos = await getGruposSanguineos();
            fillSelectGruposSanguineos(gruposSanguineos);
            estadosCiviles = await getEstadosCiviles();
            fillSelectEstadosCiviles(estadosCiviles);
            jefesInmediatos = await getJefesInmediatos();
            fillSelectJefesInmediatos(jefesInmediatos);
            sedes = await getSedes();
            fillSelectSedes(sedes);
            bandas = await getBandas();
            fillSelectBandasPromocion(bandas);
            levels = await getLevels();
            fillSelectNuevoLevels(levels);
            modalidadesEducacion = await getModalidadesEducacion();
            fillSelectModalidadesEducacion(modalidadesEducacion);
            tiposEducacion = await getTiposEducacion();
            fillSelectTiposEducacion(tiposEducacion);
            cargos = await getCargos();
            fillSelectCargosPromocion(cargos);
            BUITalent = await getCentrosCosto();
            fillSelectBUITalent(BUITalent);
            estadosEducacion = await getEstadosEducacion();
            fillSelectEstadosEducacion(estadosEducacion);
            parentescos = await getParentescos();
            fillSelectParentescos(parentescos);
            motivosCambioSalario = await getMotivosCambioSalario();
            fillSelectMotivosCambioSalario(motivosCambioSalario);
            tiposContrato = await getTiposContratacion();
            fillSelectTiposContrato(tiposContrato);
            tiemposContrato = await getTiemposContrato();
            fillSelectTiemposContrato(tiemposContrato);
            tiposSalario = await getTiposSalario();
            fillSelectNuevoTiposSalario(tiposSalario);
            bancos = await getBancos();
            fillSelectBancos(bancos);
            tiposCuenta = await getTiposCuenta();
            fillSelectTiposCuenta(tiposCuenta);
            EPS = await getEPS();
            fillSelectEPS(EPS);
            fondosPensiones = await getFondosPensiones();
            fillSelectFondosPensiones(fondosPensiones);
            fondosCesantias = await getFondosCesantias();
            fillSelectFondosCesantias(fondosCesantias);
            cajasCompensacion = await getCajasCompensacion();
            fillSelectCajasCompensacion(cajasCompensacion);
            ARL = await getARL();
            fillSelectARL(ARL);
            ciudades = await getCiudades();
            fillSelectCiudades(ciudades);
            NSE = await getNSE();
            fillSelectNSE(NSE);
            localidades = await getLocalidades();
            fillSelectLocalidades(localidades);
            nivelesIdioma = await getNivelesIdioma();
            fillSelectNivelesIdioma(nivelesIdioma);
            empresas = await getEmpresas();
            fillSelectEmpresa(empresas);
            jobFunctions = await getJobFunctions();
            fillSelectJobFunctions(jobFunctions);
            municipios = await getMunicipios();
            fillSelectMunicipiosNacimiento(municipios);
            tallasCamiseta = await getTallasCasmiseta();
            fillSelectTallasCamiseta(tallasCamiseta);

            fillSelectFilterAreasServicesLines(areasServiceLines);
            fillSelectFilterCargos(cargos);
            fillSelectFilterSedes(sedes);
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

        function maximizeSearch() {
            document.getElementsByClassName('filters')[0].classList.add('maximize');
        }
        function minimizeSearch() {
            document.getElementsByClassName('filters')[0].classList.remove('maximize');
        }

        function isValidForm(formClassName) {
            let isValid = true;
            let containerForm = document.getElementsByClassName(formClassName)[0];

            let fields = document.querySelectorAll(`.${formClassName} input, .${formClassName} select, .${formClassName} textarea`);

            fields.forEach((element, index) => {
                if (!element.validity.valid && elementIsVisible(element)) {
                    element.classList.add('invalid');
                    isValid = false;
                }
                else
                    element.classList.remove('invalid');
            });

            return isValid;
        }
        function elementIsVisible(element) {
            return !(element.offsetParent === null);
        }

        function enableButton(button) {
            button.disabled = false;
        }
        function disableButton(button) {
            button.disabled = true;
        }

        function showRetirarPersona() {
            let containerRetirarPersona = document.getElementsByClassName("retirarPersona")[0];
            let containerReintegrarPersona = document.getElementsByClassName("reintegrarPersona")[0];

            containerRetirarPersona.style.display = "";
            containerReintegrarPersona.style.display = "none";

        }
        function showReintegrarPersona() {
            let containerRetirarPersona = document.getElementsByClassName("retirarPersona")[0];
            let containerReintegrarPersona = document.getElementsByClassName("reintegrarPersona")[0];

            containerRetirarPersona.style.display = "none";
            containerReintegrarPersona.style.display = "";
        }

        function validateEmployeedIdRequired() {
            const COD_EMPRESA_IPSOS = 1;

            let employeedId = document.getElementById('employeedId').value;
            let empresa = document.getElementById('empresa').value;

            if (empresa == COD_EMPRESA_IPSOS && employeedId == "")
                employeedId.setCustomValidity = "*";
            else
                employeedId.setCustomValidity = "";
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
    Administración de la información de los empleados
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
    <div class="returnToPersons showReturnToPersons">
        <i class="material-icons">keyboard_backspace</i>
    </div>
    <div class="scroller">
        <div style="width: 3000px; display: inline-flex;">
            <div class="containerCardsPerson">
                <div class="filters maximize" style="width: 980px;">
                    <div>
                        <i class="material-icons maximizeSearchIcon" title="Filtros">details</i>
                        <h4>Filtros:</h4>
                    </div>
                    <div class="formFilters" style="display: flex; flex-wrap: wrap; justify-content: space-evenly; align-items: center;">
                        <div>
                            <label>Activo</label>
                            <input type="checkbox" id="filterActivo" checked />
                        </div>
                        <div>
                            <label>Identificacion</label>
                            <input type="text" id="filterIdentificacion" />
                        </div>
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
                            <button id="btnSearch">Buscar</button>
                        </div>
                    </div>
                    <div style="text-align: right">
                        <i class="material-icons minimizeSearchIcon">publish</i>
                    </div>
                </div>
                <div class="cardPerson">
                    <div>
                        <i class="material-icons" style="font-size: 120px">person_add</i>
                    </div>
                    <div>
                        <label>Nuevo</label>
                        <label>candidato!!</label>
                    </div>
                </div>
            </div>
            <div class="menuLeft">
                <a name="Inicio"><i class="material-icons" title="Datos generales">book</i></a>
                <div class="verticalLine"></div>
                <a href="#DatosLaborales"><i class="material-icons" title="Datos laborales">business</i></a>
                <div class="verticalLine"></div>
                <a href="#Salarios"><i class="material-icons" title="Salarios">attach_money</i></a>
                <div class="verticalLine"></div>
                <a href="#HistoricoPosiciones"><i class="material-icons" title="Historico posiciones">trending_up</i></a>
                <div class="verticalLine"></div>
                <a href="#Nomina"><i class="material-icons" title="Nomina">payment</i></a>
                <div class="verticalLine"></div>
                <a href="#Ingles"><i class="material-icons" title="Ingles">record_voice_over</i></a>
                <div class="verticalLine"></div>
                <a href="#Hijos"><i class="material-icons" title="Hijos">child_care</i></a>
                <div class="verticalLine"></div>
                <a href="#ContactoEmergencia"><i class="material-icons" title="Contacto emergencia">directions_run</i></a>
                <div class="verticalLine"></div>
                <a href="#DatosPersonales"><i class="material-icons" title="Datos personales">person</i></a>
                <div class="verticalLine"></div>
                <a href="#Experiencia"><i class="material-icons" title="Experiencia laboral">call_merge</i></a>
                <div class="verticalLine"></div>
                <a href="#Educacion"><i class="material-icons" title="Educación">school</i></a>
                <div class="verticalLine"></div>
            </div>
            <div class="person">
                <div class="containerForms">
                    <div class="form">
                        <label class="namesPerson"></label>
                        <h1>
                            <i class="material-icons" title="Datos generales" style="font-size: 20px;">book</i>
                            Datos generales
                        </h1>
                        <div style="display: flex; flex-wrap: wrap;">
                            <img src="../Images/sin-foto.jpg" alt="Foto" style="width: 104px" />
                            <label class="custom-file-upload" style="margin-left: 10px;">
                                <input type="file" id="foto" />
                                <i class="material-icons">cloud_upload</i>Cargar imagen
                            </label>
                        </div>
                        <div style="display: flex; flex-wrap: wrap; align-content: space-between;" class="formDatosGenerales">
                            <div class="field">
                                <label for="tipoIdentificacion">Tipo identificación</label>
                                <select id="tipoIdentificacion" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="identificacion">Identificación</label>
                                <input type="text" id="identificacion" required />
                            </div>
                            <div class="field">
                                <label for="nombres">Nombres</label>
                                <input type="text" id="nombres" required />
                            </div>
                            <div class="field">
                                <label for="apellidos">Apellidos</label>
                                <input type="text" id="apellidos" required />
                            </div>
                            <div class="field">
                                <label for="nombrePreferido">Nombre preferido</label>
                                <input type="text" id="nombrePreferido" required />
                            </div>
                            <div class="field">
                                <label for="fechaNacimiento">Fecha Nacimiento</label>
                                <input type="date" id="fechaNacimiento" required />
                            </div>
                            <div class="field">
                                <label for="edad">Edad</label>
                                <input type="number" id="edad" disabled required />
                            </div>
                            <div class="field">
                                <label for="genero">Genero</label>
                                <select id="genero" required>
                                    <option value=""></option>
                                    <option value="1">Masculino</option>
                                    <option value="2">Femenino</option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="estadoCivil">Estado civil</label>
                                <select id="estadoCivil" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="grupoSanguineo">Grupo sanguineo</label>
                                <select id="grupoSanguineo" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="nacionalidad">Nacionalidad</label>
                                <input type="text" id="nacionalidad" required />
                            </div>
                            <div class="field add">
                                <button title="Adicionar / Actualizar Datos generales" id="updateDatosGenerales" type="button"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <h1>
                            <i class="material-icons" title="Datos laborales">business</i>
                            Datos laborales
							<a href="#Inicio" name="DatosLaborales"><i class="material-icons" title="Arriba">arrow_upward</i></a>
                        </h1>
                        <div style="display: flex; flex-wrap: wrap;" class="formDatosLaborales">
                            <div class="field">
                                <label for="employeedId">Employeed Id</label>
                                <input type="text" id="employeedId" required />
                            </div>
                            <div class="field">
                                <label for="centroCosto">BU Name Italent</label>
                                <select id="centroCosto" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="jobFunction">Job Function</label>
                                <select id="jobFunction" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="jefeInmediato">Jefe:</label>
                                <select id="jefeInmediato" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="sede">Sede:</label>
                                <select id="sede" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="correoIpsos">Correo Ipsos:</label>
                                <input type="email" id="correoIpsos" />
                            </div>
                            <div class="field">
                                <label for="fechaIngresoIpsos">Fecha Ingreso Ipsos:</label>
                                <input type="date" id="fechaIngresoIpsos" required />
                            </div>
                            <div class="field">
                                <label for="tipoContrato">Tipo de contrato:</label>
                                <select id="tipoContrato" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="tiempoContrato">Tiempo de contrato:</label>
                                <select id="tiempoContrato" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="empresa">Empresa:</label>
                                <select id="empresa" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="observaciones">Observaciones</label>
                                <textarea id="observaciones" rows="4" cols="50"> </textarea>
                            </div>
                            <div class="field add">
                                <button title="Actualziar datos laborales" id="updateDatosLaborales" type="button"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <h1>
                            <i class="material-icons" title="Salarios">attach_money</i>
                            Salarios
							<a href="#Inicio" name="Salarios"><i class="material-icons" title="Arriba">arrow_upward</i></a>
                        </h1>
                        <div class="containerSalarios">
                        </div>
                        <h4>Nuevo salario</h4>
                        <div style="display: flex; flex-wrap: wrap;" class="formSalarioNuevo">
                            <div class="field">
                                <label for="fechaAplicacionSalario">Fecha aplicación:</label>
                                <input type="date" value="" id="fechaAplicacionSalario" required />
                            </div>
                            <div class="field">
                                <label for="motivoCambioSalario">Motivo cambio:</label>
                                <select id="motivoCambioSalario" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="nuevoTipoSalario">Tipo:</label>
                                <select id="nuevoTipoSalario" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="nuevoSalario">Salario:</label>
                                <input type="number" value="" id="nuevoSalario" required />
                            </div>
                            <div class="field add">
                                <button title="Adicionar salario" id="addSalario" type="button"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <h1>
                            <i class="material-icons" title="Historico de posiciones">trending_up</i>
                            Historico de posiciones
							<a href="#Inicio" name="HistoricoPosiciones"><i class="material-icons" title="Arriba">arrow_upward</i></a>
                        </h1>
                        <div class="containerPromociones">
                        </div>
                        <h4>Agregar posición</h4>
                        <div style="display: flex; flex-wrap: wrap;" class="formPromocionNueva">
                            <div class="field">
                                <label for="fechaPromocion">Fecha:</label>
                                <input type="date" value="" id="fechaPromocion" required />
                            </div>
                            <div class="field">
                                <label for="nuevaBandaPromocion">Banda:</label>
                                <select id="nuevaBandaPromocion" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="nuevoLevelPromocion">Level:</label>
                                <select id="nuevoLevelPromocion" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="nuevaAreaPromocion">Service line / Area:</label>
                                <select id="nuevaAreaPromocion" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="nuevoCargoPromocion">Cargo:</label>
                                <select id="nuevoCargoPromocion" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field add">
                                <button title="Adicionar promoción" id="addPromocion" type="button"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <h1>
                            <i class="material-icons" title="Nomina">payment</i>
                            Nomina
							<a name="Nomina" href="#Inicio"><i class="material-icons" title="Arriba">arrow_upward</i></a>
                        </h1>
                        <div style="display: flex; flex-wrap: wrap;" class="formNomina">
                            <div class="field">
                                <label for="banco">Banco:</label>
                                <select id="banco" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="tipoCuenta">Tipo de cuenta:</label>
                                <select id="tipoCuenta" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="numeroCuenta">Número de cuenta:</label>
                                <input type="number" id="numeroCuenta" required />
                            </div>
                            <div class="field">
                                <label for="eps">EPS:</label>
                                <select id="eps" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="fondoPension">Fondo de pensiones:</label>
                                <select id="fondoPension" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="fondoCesantias">Fondo de cesantias:</label>
                                <select id="fondoCesantias" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="cajaCompensacion">Caja de compensación:</label>
                                <select id="cajaCompensacion" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="arl">ARL:</label>
                                <select id="arl" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field add">
                                <button title="Adicionar salario" id="updateNomina" type="button"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <h1>
                            <a name="Nomina" href="#Ingles"><i class="material-icons" title="Ingles">record_voice_over</i></a>
                            Ingles
                        </h1>
                        <div class="formIngles" style="display: flex; flex-wrap: wrap;">
                            <div class="field">
                                <label for="nivelIngles">Nivel de ingles:</label>
                                <select id="nivelIngles" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field add">
                                <button title="Actualizar nivel de ingles" id="updateNivelIngles" type="button"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <h1>
                            <i class="material-icons" title="Hijos">child_care</i>
                            Hijos
							<a href="#Inicio" name="Hijos"><i class="material-icons" title="Arriba">arrow_upward</i></a>
                        </h1>
                        <div class="containerHijos">
                        </div>
                        <h4>Agregar hijo</h4>
                        <div style="display: flex; flex-wrap: wrap;" class="formHijoNuevo">
                            <div class="field">
                                <label for="nombresHijo">Nombres:</label>
                                <input type="text" value="" id="nombresHijo" required />
                            </div>
                            <div class="field">
                                <label for="apellidosHijo">Apellidos:</label>
                                <input type="text" value="" id="apellidosHijo" required />
                            </div>
                            <div class="field">
                                <label for="generoHijo">Genero:</label>
                                <select id="generoHijo" required>
                                    <option value=""></option>
                                    <option value="1">Masculino</option>
                                    <option value="2">Femenino</option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="fechaNacimientoHijo">Fecha nacimiento:</label>
                                <input type="date" value="" id="fechaNacimientoHijo" required />
                            </div>
                            <div class="field add">
                                <button title="Adicionar hijo" id="addHijo" type="button"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <h1>
                            <i class="material-icons" title="Contactos de emergencia">directions_run</i>
                            Contactos de emergencia
							<a href="#Inicio" name="ContactoEmergencia"><i class="material-icons" title="Arriba">arrow_upward</i></a>
                        </h1>
                        <div class="containerContactosEmergencia">
                        </div>
                        <h4>Agregar contacto de emergencia</h4>
                        <div style="display: flex; flex-wrap: wrap;" class="formContactoEmergenciaNuevo">
                            <div class="field">
                                <label for="nombresContactoEmergencia">Nombres:</label>
                                <input type="text" value="" id="nombresContactoEmergencia" required />
                            </div>
                            <div class="field">
                                <label for="apellidosContactoEmergencia">Apellidos:</label>
                                <input type="text" value="" id="apellidosContactoEmergencia" required />
                            </div>
                            <div class="field">
                                <label for="parentescoContactoEmergencia">Parentesco:</label>
                                <select id="parentescoContactoEmergencia" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="telefonoFijoContactoEmergencia">Telefono fijo:</label>
                                <input type="number" value="" id="telefonoFijoContactoEmergencia" />
                            </div>
                            <div class="field">
                                <label for="telefonoCelularContactoEmergencia">Telefono celular:</label>
                                <input type="number" value="" id="telefonoCelularContactoEmergencia" required />
                            </div>
                            <div class="field add">
                                <button title="Adicionar contacto de emergencia" id="addContactoEmergencia" type="button"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <h1>
                            <a href="#Inicio" name="DatosPersonales"><i class="material-icons" title="Datos personales" style="font-size: 20px;">person</i></a>
                            Datos personales
                        </h1>
                        <div style="display: flex; flex-wrap: wrap; align-content: space-between;" class="formDatosPersonales">
                            <div class="field">
                                <label for="municipioNacimiento">Ciudad / municipio nacimiento</label>
                                <select id="municipioNacimiento" required class="chosen-select">
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="departamentoNacimiento">Departamento nacimiento</label>
                                <select id="departamentoNacimiento" required disabled>
                                </select>
                            </div>
                            <div class="field">
                                <label for="ciudad">Ciudad residencia</label>
                                <select id="ciudad" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="direccion">Dirección</label>
                                <input type="text" id="direccion" required />
                            </div>
                            <div class="field">
                                <label for="barrio">Barrio / Zona</label>
                                <input type="text" id="barrio" required />
                            </div>
                            <div class="field">
                                <label for="localidad">Localidad</label>
                                <select id="localidad" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="nse">NSE</label>
                                <select id="nse" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="telefonoFijo">Telefono fijo</label>
                                <input type="number" id="telefonoFijo" />
                            </div>
                            <div class="field">
                                <label for="telefonoCelular">Telefono celular</label>
                                <input type="number" id="telefonoCelular" required />
                            </div>
                            <div class="field">
                                <label for="emailPersonal">Email personal</label>
                                <input type="email" id="emailPersonal" required />
                            </div>
                            <div class="field">
                                <label for="tallaCamiseta">Talla camiseta</label>
                                <select id="tallaCamiseta" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field add">
                                <button title="Adicionar / Actualizar datos personales" id="updateDatosPersonales" type="button"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <h1>
                            <a href="#Inicio" name="Experiencia"><i class="material-icons" title="Experiencia laboral">call_merge</i></a>
                            Experiencia laboral
							<a href="#Inicio"><i class="material-icons" title="Arriba">arrow_upward</i></a>
                        </h1>
                        <div class="containerExperienciaLaboral">
                        </div>
                        <div>
                            <h4>Nueva experiencia</h4>
                            <div class="experienciaLaboralNueva">
                                <div class="field">
                                    <label for="nombreEmpresa">Nombre empresa:</label>
                                    <input type="text" id="nombreEmpresa" required />
                                </div>
                                <div class="field">
                                    <label for="experienciaLaboralCargo">Cargo:</label>
                                    <input type="text" id="experienciaLaboralCargo" required />
                                </div>
                                <div class="field">
                                    <label for="FechaInicio">Fecha Inicio:</label>
                                    <input type="date" id="FechaInicioExperiencia" required />
                                </div>
                                <div class="field">
                                    <label for="FechaFin">Fecha Fin:</label>
                                    <input type="date" id="FechaFinExperiencia" required />
                                </div>
                                <div class="field">
                                    <label for="esInvestigacion">Empresa investigación:</label>
                                    <input type="checkbox" id="esInvestigacion" />
                                </div>
                                <div class="field add">
                                    <button title="Adicionar experiencia" id="addExperiencia" type="button"><i class="material-icons">playlist_add</i></button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form">
                        <h1>
                            <a href="#Inicio" name="Educacion"><i class="material-icons" title="Educación">school</i></a>
                            Educación
							<a href="#Inicio"><i class="material-icons" title="Arriba">arrow_upward</i></a>
                        </h1>
                        <div class="containerEducacion">
                        </div>
                        <h4>Nueva formación</h4>
                        <div style="display: flex; flex-wrap: wrap;" class="formEducacionNueva">
                            <div class="field">
                                <label for="tipo">Tipo:</label>
                                <select id="tipoEducacion" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="modalidad">Modalidad:</label>
                                <select id="modalidadEducacion" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field">
                                <label for="titulo">Titulo:</label>
                                <input type="text" value="" id="tituloEducacion" required />
                            </div>
                            <div class="field">
                                <label for="institucion">Institución:</label>
                                <input type="text" id="institucionEducacion" required />
                            </div>
                            <div class="field">
                                <label for="pais">Pais:</label>
                                <input type="text" id="paisEducacion" required />
                            </div>
                            <div class="field">
                                <label for="ciudad">Ciudad:</label>
                                <input type="text" id="ciudadEducacion" required />
                            </div>
                            <div class="field">
                                <label for="fechaInicioEducacion">Fecha inicio:</label>
                                <input type="date" id="fechaInicioEducacion" required />
                            </div>
                            <div class="field">
                                <label for="fechaFinEducacion">Fecha fin:</label>
                                <input type="date" id="fechaFinEducacion" />
                            </div>
                            <div class="field">
                                <label for="estadoEducacion">Estado:</label>
                                <select id="estadoEducacion" required>
                                    <option value=""></option>
                                </select>
                            </div>
                            <div class="field add">
                                <button title="Adicionar educación" id="addEducacion" type="button"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="inactivePerson">
                <label class="namesPerson"></label>
                <div class="retirarPersona">
                    <h4>Retirar persona</h4>
                    <div class="form">
                        <div class="formRetirarPersona" style="display: flex; flex-wrap: wrap; align-content: space-between;">
                            <div class="field">
                                <label for="fechaRetiro">Fecha retiro</label>
                                <input type="date" id="fechaRetiro" required />
                            </div>
                            <div class="field">
                                <label for="observacionRetiro">Observación de retiro</label>
                                <textarea id="observacionRetiro" cols="50" rows="5" required></textarea>
                            </div>
                            <div class="field" style="align-content: center;">
                                <button id="addRetiro" type="button">Retirar</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="reintegrarPersona">
                    <h4>Reintegrar persona</h4>
                    <div class="form">
                        <div class="formReintegrarPersona" style="display: flex; flex-wrap: wrap; align-content: space-between; align-items: center;">
                            <div class="field">
                                <label for="fechaReintegro">Fecha reintegro</label>
                                <input type="date" id="fechaReintegro" required />
                            </div>
                            <div class="field">
                                <button id="addReintegro" type="button">Reintegro</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>

        var parentToScroll = document.getElementsByClassName("scroller")[0];

        document.getElementsByClassName('returnToPersons')[0].onclick = returnToPersons;
        document.querySelector('.custom-file-upload input').onchange = function () { loadImage(this) };

        document.getElementsByClassName('cardPerson')[0].onclick = function () { showPerson(this) };

        document.getElementById('updateDatosGenerales').onclick = async function () { disableButton(this); await updateDatosGenerales(); enableButton(this); return false; };
        document.getElementById('updateDatosLaborales').onclick = async function () { disableButton(this); await updateDatosLaborales(); enableButton(this); return false; };
        document.getElementById('updateDatosPersonales').onclick = async function () { disableButton(this); await updateDatosPersonales(); enableButton(this); return false; };
        document.getElementById('updateNomina').onclick = async function () { disableButton(this); await updateNomina(); enableButton(this); return false; };
        document.getElementById('updateNivelIngles').onclick = async function () { disableButton(this); await updateNivelIngles(); enableButton(this); return false; };

        document.getElementById('addExperiencia').onclick = async function () { disableButton(this); await addExperienciaLaboral(); enableButton(this); return false; };
        document.getElementById('addEducacion').onclick = async function () { disableButton(this); await addEducacion(); enableButton(this); return false; };
        document.getElementById('addHijo').onclick = async function () { disableButton(this); await addHijo(); enableButton(this); return false; };
        document.getElementById('addContactoEmergencia').onclick = async function () { disableButton(this); await addContactoEmergencia(); enableButton(this); return false; };
        document.getElementById('addPromocion').onclick = async function () { disableButton(this); await addPromocion(); enableButton(this); return false; };
        document.getElementById('addSalario').onclick = async function () { disableButton(this); await addSalario(); enableButton(this); return false; };

        document.getElementById('addRetiro').onclick = async function () { disableButton(this); await retirarPersona(); enableButton(this); return false; };
        document.getElementById('addReintegro').onclick = async function () { disableButton(this); await reintegrarPersona(); enableButton(this); return false; };

        document.getElementById('tipoContrato').onchange = async function () { mostrarTiempoContrato(); return false; };
        document.getElementById('ciudad').onchange = async function () { mostrarLocalidad(); return false; };

        document.getElementById('fechaNacimiento').onchange = fillInputEdad;
        document.getElementById('municipioNacimiento').onchange = OnChangeDdlMunicipioNacimiento;

        document.getElementsByClassName('maximizeSearchIcon')[0].onclick = maximizeSearch;
        document.getElementsByClassName('minimizeSearchIcon')[0].onclick = minimizeSearch;

        document.getElementById('btnSearch').onclick = function () { getEmpleados(); return false; };

        setupForms();
    </script>
</asp:Content>
