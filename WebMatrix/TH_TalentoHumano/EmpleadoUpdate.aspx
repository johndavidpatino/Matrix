<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/NewSiteWithReturnInit.master" CodeBehind="EmpleadoUpdate.aspx.vb" Inherits="WebMatrix.EmpleadoUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <link href="../css/TH_Empleados.css" rel="stylesheet">
    <script src="../Scripts/returnLoginFetchUnauthorized.js" type="text/javascript"></script>
    <script src="../Scripts/imageToTextBase64FromInputFile.js" type="text/javascript"></script>
    <script src="../Scripts/loadImageFromInputFileToTagImage.js" type="text/javascript"></script>
    <link href="../Scripts/Chosen/chosen.min.css" rel="stylesheet">
    <script src="../Scripts/Chosen/chosen.jquery.js" type="text/javascript"></script>

    <script>
        var intervalIdRight;
        var intervalIdLeft;
        var scrollLeftNow = 0;
        var scrollRightNow = 1000;
        var idPersonSelected;

        function loadImage(input) {
            let img = document.querySelector('.containerForms img');
            loadImageFromInputFileToTagImage(input, img);
        }
        function getEmpleado() {

            return fetch('EmpleadoUpdate.aspx/getEmpleado', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getExperienciasLaborales() {

            return fetch('EmpleadoUpdate.aspx/getExperienciasLaborales', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getEducacion() {

            return fetch('EmpleadoUpdate.aspx/getEducacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getHijos() {

            return fetch('EmpleadoUpdate.aspx/getHijos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getContactosEmergencia() {

            return fetch('EmpleadoUpdate.aspx/getContactosEmergencia', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
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

        async function loadEmpleado() {
            let birthdate;
            let empleado;
            let experienciasLaborales;
            let formaciones;
            let hijos;
            let contactosEmergencia;

            empleado = await getEmpleado();
            experienciasLaborales = await getExperienciasLaborales();
            formaciones = await getEducacion();
            hijos = await getHijos();
            contactosEmergencia = await getContactosEmergencia();

            birthdate = empleado.FechaNacimiento != null ? getDateFromMilliseconds(empleado.FechaNacimiento) : null;

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

            drawExperienciasLaborales(experienciasLaborales);
            drawEducacion(formaciones);
            drawHijos(hijos);
            drawContactosEmergencia(contactosEmergencia);

            SetupChosenComponents();

        }

        function OnChangeDdlMunicipioNacimiento(e) {
            let element = e.target;
            let value = element.value;

            fillSelectDepartamentoNacimiento(value)
        }

        function deleteExperienciaLaboral(element) {
            let formData = Object();

            formData.identificacion = element.getElementsByTagName('input')[0].value;

            fetch('EmpleadoUpdate.aspx/deleteExperienciaLaboral', {
                method: 'POST',
                body: JSON.stringify(formData),
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(async data => {
                    clearExperienciasLaborales();
                    let experienciasLaborales = await getExperienciasLaborales();
                    drawExperienciasLaborales(experienciasLaborales);
                    alert('Eliminado correctamente');
                });
        }
        function deleteEducacion(element) {
            let formData = Object();

            formData.identificacion = element.getElementsByTagName('input')[0].value;

            fetch('EmpleadoUpdate.aspx/deleteEducacion', {
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
                    formaciones = await getEducacion();
                    drawEducacion(formaciones);
                    clearFormEducacion();
                    alert('Eliminado correctamente');
                });
        }
        function deleteHijo(element) {
            let formData = Object();

            formData.id = element.getElementsByTagName('input')[0].value;

            fetch('EmpleadoUpdate.aspx/deleteHijo', {
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
                    hijos = await getHijos();
                    drawHijos(hijos);
                    clearFormHijo();
                    alert('Eliminado correctamente');
                });
        }
        function deleteContactoEmergencia(element) {
            let formData = Object();

            formData.id = element.getElementsByTagName('input')[0].value;

            fetch('EmpleadoUpdate.aspx/deleteContactoEmergencia', {
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
                    contactosEmergencia = await getContactosEmergencia();
                    drawContactosEmergencia(contactosEmergencia);
                    clearFormContactoEmergencia();
                    alert('Eliminado correctamente');
                });
        }

        function addExperienciaLaboral() {
            if (isValidForm('experienciaLaboralNueva')) {
                let button = document.getElementById('addExperiencia');
                button.disabled = true;
                let formData = new Object();

                formData.empresa = document.getElementById('nombreEmpresa').value;
                formData.fechaInicio = document.getElementById('FechaInicioExperiencia').value;
                formData.fechaFin = document.getElementById('FechaFinExperiencia').value;
                formData.cargo = document.getElementById('experienciaLaboralCargo').value;
                formData.esInvestigacion = document.getElementById('esInvestigacion').checked;
                fetch('EmpleadoUpdate.aspx/addExperienciaLaboral', {
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
                        experienciasLaborales = await getExperienciasLaborales(idPersonSelected);
                        drawExperienciasLaborales(experienciasLaborales);
                        clearFormExperienciaLaboral();
                        alert('Adicionado correctamente');
                        button.disabled = false;
                    });
            }
        }
        function addEducacion() {
            if (isValidForm('formEducacionNueva')) {
                let formData = new Object();
                let button = document.getElementById('addEducacion');
                button.disabled = true;
                formData.tipo = document.getElementById('tipoEducacion').value;
                formData.titulo = document.getElementById('tituloEducacion').value;
                formData.institucion = document.getElementById('institucionEducacion').value;
                formData.pais = document.getElementById('paisEducacion').value;
                formData.ciudad = document.getElementById('ciudadEducacion').value;
                formData.fechaInicio = document.getElementById('fechaInicioEducacion').value;
                formData.fechaFin = document.getElementById('fechaFinEducacion').value;
                formData.modalidad = document.getElementById('modalidadEducacion').value;
                formData.estado = document.getElementById('estadoEducacion').value;

                fetch('EmpleadoUpdate.aspx/addEducacion', {
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
                        button.disabled = false;
                    });
            }
        }
        function addHijo() {
            if (isValidForm('formHijoNuevo')) {
                let button = document.getElementById('addHijo');
                button.disabled = true;
                let formData = new Object();

                formData.nombres = document.getElementById('nombresHijo').value;
                formData.apellidos = document.getElementById('apellidosHijo').value;
                formData.genero = document.getElementById('generoHijo').value;
                formData.fechaNacimiento = document.getElementById('fechaNacimientoHijo').value;

                fetch('EmpleadoUpdate.aspx/addHijo', {
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
                        button.disabled = false;
                    });
            }
        }
        function addContactoEmergencia() {
            if (isValidForm('formContactoEmergenciaNuevo')) {
                let formData = new Object();
                let button = document.getElementById('addContactoEmergencia');
                button.disabled = true;

                formData.nombres = document.getElementById('nombresContactoEmergencia').value;
                formData.apellidos = document.getElementById('apellidosContactoEmergencia').value;
                formData.parentesco = document.getElementById('parentescoContactoEmergencia').value;
                formData.telefonoFijo = document.getElementById('telefonoFijoContactoEmergencia').value;
                formData.telefonoCelular = document.getElementById('telefonoCelularContactoEmergencia').value;

                fetch('EmpleadoUpdate.aspx/addContactoEmergencia', {
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
                        button.disabled = false;
                    });
            }
        }

        async function updateDatosGenerales() {

            if (isValidForm('formDatosGenerales')) {
                let formData = new Object();
                let inputFoto = document.querySelector('.custom-file-upload input');

                formData.tipoId = document.getElementById('tipoIdentificacion').value;
                formData.nombres = document.getElementById('nombres').value;
                formData.apellidos = document.getElementById('apellidos').value;
                formData.nombrePreferido = document.getElementById('nombrePreferido').value;
                formData.fechaNacimiento = document.getElementById('fechaNacimiento').value;
                formData.sexo = document.getElementById('genero').value;
                formData.estadoCivil = document.getElementById('estadoCivil').value;
                formData.grupoSanguineo = document.getElementById('grupoSanguineo').value;
                formData.nacionalidad = document.getElementById('nacionalidad').value;
                formData.fotoBase64 = inputFoto.files.length == 0 ? null : await imageToTextBase64FromInputFile(inputFoto.files[0]);
                fetch('EmpleadoUpdate.aspx/updateDatosGenerales', {
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


                fetch('EmpleadoUpdate.aspx/updateDatosPersonales', {
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

        function clearFormPerson() {
            let elements = Array.from(document.querySelectorAll('.person input,.person select,.person textarea'));
            elements.forEach((element) => {
                element.value = '';
            });
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

        function getAreasServicesLine() {
            return fetch('EmpleadoUpdate.aspx/getAreasServiceLines', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getGruposSanguineos() {
            return fetch('EmpleadoUpdate.aspx/getGruposSanguineos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getEstadosCiviles() {
            return fetch('EmpleadoUpdate.aspx/getEstadosCiviles', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getJefesInmediatos() {
            return fetch('EmpleadoUpdate.aspx/getJefesInmediatos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getSedes() {
            return fetch('EmpleadoUpdate.aspx/getSedes', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getBandas() {
            return fetch('EmpleadoUpdate.aspx/getBandas', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getLevels() {
            return fetch('EmpleadoUpdate.aspx/getLevels', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiposEducacion() {
            return fetch('EmpleadoUpdate.aspx/getTiposEducacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getModalidadesEducacion() {
            return fetch('EmpleadoUpdate.aspx/getModalidadesEducacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getCargos() {
            return fetch('EmpleadoUpdate.aspx/getCargos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getCentrosCosto() {
            return fetch('EmpleadoUpdate.aspx/getCentrosCosto', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getEstadosEducacion() {
            return fetch('EmpleadoUpdate.aspx/getEstadosEducacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getParentescos() {
            return fetch('EmpleadoUpdate.aspx/getParentescos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getMotivosCambioSalario() {
            return fetch('EmpleadoUpdate.aspx/getMotivosCambioSalario', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiposContratacion() {
            return fetch('EmpleadoUpdate.aspx/getTiposContratacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiposIdentificacion() {
            return fetch('EmpleadoUpdate.aspx/getTiposIdentificacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiemposContrato() {
            return fetch('EmpleadoUpdate.aspx/getTiemposContrato', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiposSalario() {
            return fetch('EmpleadoUpdate.aspx/getTiposSalario', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getBancos() {
            return fetch('EmpleadoUpdate.aspx/getBancos', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getTiposCuenta() {
            return fetch('EmpleadoUpdate.aspx/getTiposCuentaBancaria', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getEPS() {
            return fetch('EmpleadoUpdate.aspx/getEPS', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getFondosPensiones() {
            return fetch('EmpleadoUpdate.aspx/getFondosPensiones', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getFondosCesantias() {
            return fetch('EmpleadoUpdate.aspx/getFondosCesantias', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getCajasCompensacion() {
            return fetch('EmpleadoUpdate.aspx/getCajasCompensacion', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getARL() {
            return fetch('EmpleadoUpdate.aspx/getARL', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getCiudades() {
            return fetch('EmpleadoUpdate.aspx/getCiudades', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            })
                .then(result => result.json())
                .then(data => { return data.d });
        }
        function getNSE() {
            return fetch('EmpleadoUpdate.aspx/getNSE', {
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
        function fillSelectLevels(levels) {
            let select = document.getElementById('level');

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
                option.text = `${element.idelement} - ${element.CentroDeCosto}`;
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
        function fillSelectBancos(bancos) {
            let select = document.getElementById('banco');

            bancos.forEach((element, index) => {
                let option = document.createElement('option');
                option.value = element.id;
                option.text = element.Banco;
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

        function fillInputEdad() {
            let edad;
            let fechaNacimiento;

            fechaNacimiento = new Date(document.getElementById('fechaNacimiento').value);

            edad = calculateAgeFromBirthdate(fechaNacimiento);

            document.getElementById('edad').value = edad;
        }

        async function setupForms() {
            let tiposIdentificacion;
            let gruposSanguineos;
            let estadosCiviles;
            let modalidadesEducacion;
            let tiposEducacion;
            let estadosEducacion;
            let parentescos;
            let ciudades;
            let NSE;
            let localidades;
            let municipios;
            let tallasCamiseta;

            tiposIdentificacion = await getTiposIdentificacion();
            fillSelectTiposIdentificacion(tiposIdentificacion);
            gruposSanguineos = await getGruposSanguineos();
            fillSelectGruposSanguineos(gruposSanguineos);
            estadosCiviles = await getEstadosCiviles();
            fillSelectEstadosCiviles(estadosCiviles);
            modalidadesEducacion = await getModalidadesEducacion();
            fillSelectModalidadesEducacion(modalidadesEducacion);
            tiposEducacion = await getTiposEducacion();
            fillSelectTiposEducacion(tiposEducacion);
            estadosEducacion = await getEstadosEducacion();
            fillSelectEstadosEducacion(estadosEducacion);
            parentescos = await getParentescos();
            fillSelectParentescos(parentescos);
            ciudades = await getCiudades();
            fillSelectCiudades(ciudades);
            NSE = await getNSE();
            fillSelectNSE(NSE);
            localidades = await getLocalidades();
            fillSelectLocalidades(localidades);
            municipios = await getMunicipios();
            fillSelectMunicipiosNacimiento(municipios);
            tallasCamiseta = await getTallasCasmiseta();
            fillSelectTallasCamiseta(tallasCamiseta);

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

        function isValidForm(formClassName) {
            let isValid = true;
            let containerForm = document.getElementsByClassName(formClassName)[0];

            let fields = document.querySelectorAll(`.${formClassName} input, .${formClassName} select`);

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
        function SetupChosenComponents() {
            $(".chosen-select").chosen();
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
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
    <div class="returnToPersons showReturnToPersons">
        <i class="material-icons">keyboard_backspace</i>
    </div>
    <div class="scroller">
        <div class="menuLeft">
            <a name="Inicio"><i class="material-icons" title="Datos generales">book</i></a>
            <div class="verticalLine"></div>
            <a href="#DatosPersonales"><i class="material-icons" title="Datos personales">person</i></a>
            <div class="verticalLine"></div>
            <a href="#Experiencia"><i class="material-icons" title="Experiencia laboral">call_merge</i></a>
            <div class="verticalLine"></div>
            <a href="#Educacion"><i class="material-icons" title="Educación">school</i></a>
            <div class="verticalLine"></div>
            <a href="#Hijos"><i class="material-icons" title="Hijos">child_care</i></a>
            <div class="verticalLine"></div>
            <a href="#ContactosEmergencia"><i class="material-icons" title="Contactos Emergencia">directions_run</i></a>
            <div class="verticalLine"></div>
        </div>
        <div class="person">
            <div class="containerForms">
                <div class="form">
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
                            <select id="tipoIdentificacion" required disabled>
                                <option value=""></option>
                            </select>
                        </div>
                        <div class="field">
                            <label for="identificacion">Identificación</label>
                            <input type="text" id="identificacion" required disabled />
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
                            <button title="Adicionar / Actualizar Datos generales" id="updateDatosGenerales"><i class="material-icons">playlist_add</i></button>
                        </div>
                    </div>
                </div>
                <div class="form">
                    <h1>
                        <i class="material-icons" title="Datos personales" style="font-size: 20px;">person</i>
                        Datos personales
						<a href="#Inicio" name="DatosPersonales"><i class="material-icons" title="Arriba">arrow_upward</i></a>
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
                            <button title="Adicionar / Actualizar datos personales" id="updateDatosPersonales"><i class="material-icons">playlist_add</i></button>
                        </div>
                    </div>
                </div>
                <div class="form">
                    <h1>
                        <i class="material-icons" title="Experiencia laboral">call_merge</i>
                        Experiencia laboral
							<a href="#Inicio" name="Experiencia"><i class="material-icons" title="Arriba">arrow_upward</i></a>
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
                                <button title="Adicionar experiencia" id="addExperiencia"><i class="material-icons">playlist_add</i></button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form">
                    <h1>
                        <i class="material-icons" title="Educación">school</i>
                        Educación
							<a href="#Inicio" name="Educacion"><i class="material-icons" title="Arriba">arrow_upward</i></a>
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
                            <button title="Adicionar educación" id="addEducacion"><i class="material-icons">playlist_add</i></button>
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
                            <button title="Adicionar hijo" id="addHijo"><i class="material-icons">playlist_add</i></button>
                        </div>
                    </div>
                </div>
                <div class="form">
                    <h1>
                        <i class="material-icons" title="Contactos de emergencia">directions_run</i>
                        Contactos de emergencia
							<a href="#Inicio" name="ContactosEmergencia"><i class="material-icons" title="Arriba">arrow_upward</i></a>
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
                            <button title="Adicionar contacto de emergencia" id="addContactoEmergencia"><i class="material-icons">playlist_add</i></button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        document.querySelector('.custom-file-upload input').onchange = function () { loadImage(this) };

        document.getElementById('updateDatosGenerales').onclick = () => { updateDatosGenerales(); return false; };
        document.getElementById('updateDatosPersonales').onclick = () => { updateDatosPersonales(); return false; };

        document.getElementById('addExperiencia').onclick = () => { addExperienciaLaboral(); return false; };
        document.getElementById('addEducacion').onclick = () => { addEducacion(); return false; };
        document.getElementById('addHijo').onclick = () => { addHijo(); return false; };
        document.getElementById('addContactoEmergencia').onclick = () => { addContactoEmergencia(); return false; };

        document.getElementById('fechaNacimiento').onchange = fillInputEdad;

        document.getElementById('ciudad').onchange = () => { mostrarLocalidad(); return false; };

        document.getElementById('municipioNacimiento').onchange = OnChangeDdlMunicipioNacimiento;

        (async () => { await setupForms(); loadEmpleado(); })();


    </script>
</asp:Content>
