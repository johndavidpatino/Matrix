import { Table } from "/Scripts/js/Components/Table/Table.js"
import { Loader } from "/Scripts/js/Components/Loader/Loader.js"
import { FormValidator } from "/Scripts/js/Components/FormValidator/FormValidator.js"

import { RegistroPlanillasCualitativoService } from "/Scripts/js/Services/RegistroPlanillasCualitativoService.js"

export { PlanillaModeracion }

class PlanillaModeracion {
    #FormsContainerId
    #FormsContainer
    #TablesContainer
    #TipoPlantilla
    #TecnicaDdl
    #ModeradorDdl
    #SearchJobInput
    #SearchJobResults
    #SearchJobUnselectBtn
    #JobSelectedContainer
    #btnSave
    #TablePlanilla
    #CuentasUUDdl
    #TipoTecnica
    #FechaInput
    #Muestra
    #Observaciones
    #JobSelectedContent
    #SearchJobBtn
    constructor() {
        this.#FormsContainerId = "FormsContainer"
        this.#FormsContainer = document.getElementById(this.#FormsContainerId)
        this.#TablesContainer = document.getElementById("TablesContainer")
        this.#TipoPlantilla = document.getElementById("tipoPlantilla")
        this.#Initialize();

    }

    #Initialize() {
        this.#TipoPlantilla.addEventListener("change", this.#ShowForm.bind(this))
    }

    #ShowForm() {
        let formToShow = this.#TipoPlantilla.value;
        if (formToShow == 'Moderacion') this.#InitializeFormModeracion();
        else if (formToShow == 'Informes') this.#InitializeFormInformes();
    }

    #InitializeFormModeracion() {

        this.#LoadFormModeracion()
        this.#LoadTableModeracion()

        this.#SearchJobUnselectBtn = document.getElementById("btnUnselect")
        this.#JobSelectedContainer = document.getElementById("jobSelectedContainer")
        this.#JobSelectedContent = document.getElementById("jobSelectedContent")
        this.#SearchJobInput = document.getElementById("job");
        this.#SearchJobResults = document.getElementById("searchJobResults");
        this.#SearchJobBtn = document.getElementById("btnSearchJob");

        this.#TecnicaDdl = document.getElementById("tecnica");
        this.#ModeradorDdl = document.getElementById("moderador");
        this.#btnSave = document.getElementById("btnSave");

        this.#TablePlanilla = document.getElementById("tablePlanilla")
        this.#CuentasUUDdl = document.getElementById("cuentasUU")
        this.#FechaInput = document.getElementById("fecha")
        this.#TipoTecnica = 'Moderacion'


        this.#btnSave.addEventListener("click", this.#SaveModeracion.bind(this))
        this.#SearchJobUnselectBtn.addEventListener("click", this.#UnselectJob.bind(this))
        this.#SearchJobBtn.addEventListener("click", this.#SearchJob.bind(this))

        this.#FechaInput.value = this.#GetCurrentDate();
        this.#FillDdlTecnica()
        this.#FillDdlModerador()
        this.#FillCuentasUU()
        this.#FillTableRegistros(10, 1, this.#TipoTecnica, null)
    }

    #InitializeFormInformes() {

        this.#LoadFormInformes()
        this.#LoadTableInformes()

        this.#SearchJobUnselectBtn = document.getElementById("btnUnselect")
        this.#JobSelectedContainer = document.getElementById("jobSelectedContainer")
        this.#JobSelectedContent = document.getElementById("jobSelectedContent")
        this.#SearchJobInput = document.getElementById("jobInformes");
        this.#SearchJobResults = document.getElementById("searchJobResultsInformes");
        this.#SearchJobBtn = document.getElementById("btnSearchJobInformes");

        this.#FechaInput = document.getElementById("fechaEntregaInformes")

        this.#TecnicaDdl = document.getElementById("tecnicaInformes");
        this.#Muestra = document.getElementById("muestraInformes");
        this.#CuentasUUDdl = document.getElementById("cuentasUUInformes")
        this.#ModeradorDdl = document.getElementById("analistaInformes");
        this.#Observaciones = document.getElementById("observacionesInformes");

        this.#btnSave = document.getElementById("btnSaveInformes");
        this.#TablePlanilla = document.getElementById("tablePlanillaInformes")
        this.#TipoTecnica = 'Informes'

        this.#btnSave.addEventListener("click", this.#SaveInformes.bind(this))
        this.#SearchJobBtn.addEventListener("click", this.#SearchJob.bind(this))
        this.#SearchJobUnselectBtn.addEventListener("click", this.#UnselectJob.bind(this))

        this.#FechaInput.value = this.#GetCurrentDate();
        this.#FillDdlTecnica()
        this.#FillCuentasUU()
        this.#FillDdlModerador()
        this.#FillTableRegistros(10, 1, this.#TipoTecnica, null)
    }

    async #FillDdlTecnica() {
        let loader = new Loader()
        loader.show()
        let TipoTecnica = this.#TipoTecnica;
        let data = await RegistroPlanillasCualitativoService.GetTecnicas({ TipoTecnica: TipoTecnica })
        loader.close()

        if (data.length > 0) {
            this.#TecnicaDdl.innerHTML = '<option value="">Selecciona opcion</option>';
            for (var i = 0; i < data.length; i++) {
                var option = document.createElement('option');
                option.value = data[i].Id;
                option.text = data[i].TecnicaNombre;
                option.setAttribute("puntos", data[i].Puntos)
                this.#TecnicaDdl.appendChild(option);
            }
        }

    }

    async #FillDdlModerador() {
        let loader = new Loader()
        loader.show()
        let data = await RegistroPlanillasCualitativoService.GetModeradores()
        loader.close()

        if (data.length > 0) {
            this.#ModeradorDdl.innerHTML = '<option value="">Selecciona opcion</option>';
            for (var i = 0; i < data.length; i++) {
                var option = document.createElement('option');
                option.value = data[i].Id;
                option.text = data[i].NombreModerador;
                this.#ModeradorDdl.appendChild(option);
            }
        }

    }

    async #FillCuentasUU() {
        let loader = new Loader()
        loader.show()
        let data = await RegistroPlanillasCualitativoService.GetGerentesCuentasUU()
        loader.close()

        if (data.length > 0) {
            this.#CuentasUUDdl.innerHTML = '<option value="">Selecciona opcion</option>';
            for (var i = 0; i < data.length; i++) {
                var option = document.createElement('option');
                option.value = data[i].Id;
                option.text = data[i].NombreModerador;
                this.#CuentasUUDdl.appendChild(option);
            }
        }

    }

    async #SearchJob(e) {
        e.preventDefault();
        const termToSearch = this.#SearchJobInput.value.toLowerCase();
        this.#SearchJobResults.innerHTML = '';
        let loader = new Loader()
        loader.show()
        const filteredData = await RegistroPlanillasCualitativoService.GetJobsBy({ termToSearch: termToSearch })

        filteredData.forEach(item => {
            const div = document.createElement('div');
            div.classList.add('result-item');
            div.textContent = `${item.JobId} - ${item.DescJob}`;
            div.addEventListener('click', this.#SetSearchJobValues.bind(this, item));
            this.#SearchJobResults.appendChild(div);
        });

        this.#SearchJobResults.style.display = 'block';
        loader.close()
    }

    #SetSearchJobValues(item) {
        let jobId = item.JobId;
        let jobName = item.DescJob;
        let serviceLineName = item.ServiceLineName;
        this.#SearchJobInput.value = jobId + '|' + jobName + '|' + serviceLineName;
        this.#JobSelectedContent.setAttribute('data-placeholder', jobId + ' - ' + jobName)
        this.#SearchJobResults.style.display = 'none';
        this.#JobSelectedContainer.style.display = "block"
        this.#SearchJobInput.style.display = 'none'

        let nextElementInput = this.#SearchJobInput.nextElementSibling;
        if (nextElementInput && nextElementInput.classList.contains('error-message')) {
            nextElementInput.remove();
        }

    }

    #UnselectJob(e) {
        this.#SearchJobInput.value = '';
        this.#JobSelectedContent.setAttribute('data-placeholder', '')
        this.#JobSelectedContainer.style.display = "none"
        this.#SearchJobInput.style.display = 'block'
        this.#SearchJobInput.focus()
    }

    async #SaveModeracion(e) {
        e.preventDefault();
        let loader = new Loader()
        let validator = new FormValidator();
        let jobInput = this.#SearchJobInput.value;
        let jobArray = jobInput.split("|")
        let IdJob = jobArray[0];
        let jobDesc = jobArray[1];
        let serviceLineName = jobArray[2];
        let fecha = this.#FechaInput.value;
        let hora = document.getElementById("hora").value;
        let tecnica = this.#TecnicaDdl.value
        let tiempo = document.getElementById("tiempo").value;
        let moderador = this.#ModeradorDdl.value
        let rol = document.getElementById("rol").value;
        let Observaciones = document.getElementById("observaciones").value
        let IdCuentasUU = this.#CuentasUUDdl.value


        validator.Initialize(this.#FormsContainerId);
        if (validator.IsFormValid()) {
            loader.show()
            let data = await RegistroPlanillasCualitativoService.SaveModeracion(
                {
                    IdJob: IdJob,
                    jobDesc: jobDesc,
                    fecha: fecha,
                    hora: hora,
                    tecnica: tecnica,
                    tiempo: tiempo,
                    moderador: moderador,
                    rol: rol,
                    Observaciones: Observaciones,
                    IdCuentasUU: IdCuentasUU,
                    ServiceLineName: serviceLineName
                })
            loader.close()
            this.#ClearForm();
            this.#FillTableRegistros(10, 1, this.#TipoTecnica, null)
        }

        return false;
    }

    async #SaveInformes(e) {
        e.preventDefault();
        let loader = new Loader()
        let validator = new FormValidator();
        let jobInput = this.#SearchJobInput.value;
        let jobArray = jobInput.split("|")
        let IdJob = jobArray[0];
        let jobDesc = jobArray[1];
        let fecha = this.#FechaInput.value;
        let tecnica = this.#TecnicaDdl.value
        let muestra = this.#Muestra.value;
        let IdCuentasUU = this.#CuentasUUDdl.value
        let analista = this.#ModeradorDdl.value
        let serviceLineName = jobArray[2];
        let Observaciones = this.#Observaciones.value

        validator.Initialize(this.#FormsContainerId);
        if (validator.IsFormValid()) {
            loader.show()
            let data = await RegistroPlanillasCualitativoService.SaveInformes({ IdJob: IdJob, jobDesc: jobDesc, fecha: fecha, tecnica: tecnica, muestra: muestra, IdCuentasUU: IdCuentasUU, analista: analista, Observaciones: Observaciones, ServiceLineName: serviceLineName })
            loader.close()
            this.#ClearFormInformes();
            this.#FillTableRegistros(10, 1, this.#TipoTecnica, null)
        }

        return false;
    }

    #ClearFormInformes() {
        this.#UnselectJob();
        this.#FechaInput.value = '';
        this.#TecnicaDdl.value = '';
        this.#Muestra.value = '';
        this.#CuentasUUDdl.value = '';
        this.#ModeradorDdl.value = '';
        this.#Observaciones.value = '';
    }

    #ClearForm() {
        this.#UnselectJob();
        this.#FechaInput.value = this.#GetCurrentDate();
        document.getElementById("hora").value = "";
        this.#TecnicaDdl.value = ""
        document.getElementById("tiempo").value = "";
        this.#ModeradorDdl.value = ""
        this.#CuentasUUDdl.value = ""
        document.getElementById("observaciones").value = ""
    }

    async #FillTableRegistros(pageSize, pageIndex, filtroPlanilla, idEstado) {
        let loader = new Loader()
        loader.show()
        let data = await RegistroPlanillasCualitativoService.GetPlanillas({ pageSize: pageSize, pageIndex: pageIndex, filtroPlanilla, idEstado })
        loader.close()
        const columnsConfiguration = [
            { sourcename: "IdRegistro", show: false, nameToShow: "" },
            { sourcename: "TipoRegistro", show: true, nameToShow: "Tipo Registro" },
            { sourcename: "FechaRegistro", show: true, nameToShow: "Fecha de Registro" },
            { sourcename: "UsuarioRegistro", show: true, nameToShow: "Usuario Registro" },
            { sourcename: "IdJob", show: true, nameToShow: "Job ID" },
            { sourcename: "JobDesc", show: true, nameToShow: "Job Descripcion" },
            { sourcename: "FechaActividad", show: true, nameToShow: "Fecha de Actividad" },
            { sourcename: "Tecnica", show: true, nameToShow: "Tecnica" },
            { sourcename: "EstadoPlanilla", show: true, nameToShow: "Status" },
        ];

        let tableConfiguration = {
            containerElement: this.#TablePlanilla,
            data: data,
            columsConfiguration: columnsConfiguration,
            pageSize: pageSize,
            actionButtons: [],
            totalRows: data[0] ? data[0].TotalRows : 0,
            pageIndex: pageIndex,
            pagination: true,
        }

        let table = new Table(tableConfiguration)
        table.addEventListenerChangePage(async (e) => await this.#FillTableRegistros(pageSize, e.currentPage, filtroPlanilla, idEstado))
    }

    #GetCurrentDate() {
        var today = new Date();
        var day = String(today.getDate()).padStart(2, '0');
        var month = String(today.getMonth() + 1).padStart(2, '0'); // Enero es 0!
        var year = today.getFullYear();

        return `${day}/${month}/${year}`;
    }

    #LoadFormModeracion() {
        this.#FormsContainer.innerHTML = `<div id="Moderacion">
            <div class="columna">
                <div class="form-group">
                    <label for="job">Job (Teclea numero o nombre y <span style="color: red;">teclea enter</span> para buscar)</label>
                    <div class="jobInputContainer">
                        <input type="text" id="job" name="job" class="required">
                        <button id="btnSearchJob">Buscar</button>
                    </div>
                    <div id="searchJobResults"></div>
                    <div class="input-like-container" id="jobSelectedContainer">
                        <div class="input-like" data-placeholder="." id="jobSelectedContent"></div>
                        <span class="clear-button" id="btnUnselect">&times;</span>
                    </div>
                </div>
                <div style="width:100%; display:flex;">
                    <div class="form-group" style="width:49%; margin-right:1%">
                        <label for="fecha">Fecha</label>
                        <input type="text" id="fecha" name="fecha" class="required" />
                    </div>
                    <div class="form-group" style="width:49%">
                        <label for="hora">Hora</label>
                        <input type="text" id="hora" name="hora" class="required" />
                    </div>
                </div>
                <div style="width:100%; display:flex;">
                    <div class="form-group" style="width:49%; margin-right:1%">
                        <label for="tecnica">Tecnica</label>
                        <select id="tecnica" name="tecnica" class="required"></select>
                    </div>
                    <div class="form-group" style="width:49%">
                        <label for="tiempo">Tiempo (Horas)</label>
                        <input type="text" id="tiempo" name="tiempo" class="number required" />
                    </div>
                </div>
                <div style="width:100%; display:flex;">
                    <div class="form-group" style="width:49%;  margin-right:1%">
                        <label for="cuentasUU">Cuentas UU</label>
                        <select id="cuentasUU" name="cuentasUU" class="required"></select>
                    </div>
                    <div class="form-group" style="width:49%">
                        <label for="moderador">Moderador</label>
                        <select id="moderador" name="moderador" class="required"></select>
                    </div>
                </div>
                <div class="form-group" style="width:49%">
                    <label for="rol">Rol /Principal/Apoyo</label>
                    <select id="rol" name="rol" class="required">
                        <option value="">Selecciona</option>
                        <option value="Principal">Principal</option>
                        <option value="Apoyo">Apoyo</option>
                    </select>
                </div>
            </div>
            <div class="columna">
                <div class="form-group">
                    <label for="observaciones">Observaciones</label>
                    <textarea id="observaciones" name="observaciones" rows="10" class="required"></textarea>
                </div>
                <div class="form-group" style="width:49%">
                    <label for="btnSave">&nbsp;</label>
                    <input type="button" name="btnSave" id="btnSave" class="btn" value="Guardar" />
                </div>
            </div>
        </div>`
        $('#fecha').datepicker({
            dateFormat: 'dd/mm/yy',
            showAnim: 'fadeIn'
        });
        $("#hora").timepicker({
            timeFormat: 'h:mm p',
            interval: 30,
            minTime: '06',
            maxTime: '11:30pm',
            defaultTime: '06',
            startTime: '06:00',
            dynamic: false,
            dropdown: true,
            scrollbar: true
        });

    }

    #LoadTableModeracion() {
        this.#TablesContainer.innerHTML = `<h1 class="titulo">Historico Registro de Moderaciones</h1>
                                          <div id="tablePlanilla"></div>`
    }

    #LoadFormInformes() {
        this.#FormsContainer.innerHTML = `<div id="Moderacion">
            <div class="columna">
                
                    <div class="form-group">
                        <label for="jobInformes">Job (Teclea numero o nombre y <span style="color: red;">teclea enter</span> para buscar)</label>
                        <div class="jobInputContainer">
                            <input type="text" id="jobInformes" name="jobInformes" class="required">
                            <button id="btnSearchJobInformes">Buscar</button>
                        </div>
                            <div id="searchJobResultsInformes"></div>
                            <div class="input-like-container" id="jobSelectedContainer">
                                <div class="input-like" data-placeholder="." id="jobSelectedContent"></div>
                                <span class="clear-button" id="btnUnselect">&times;</span>
                            </div>
                    </div>
                <div style="width:100%; display:flex;">
                    <div class="form-group" style="width:49%; margin-right:1%">
                        <label for="fechaEntregaInformes">Fecha de entrega</label>
                        <input type="text" id="fechaEntregaInformes" name="fechaEntregaInformes" class="required" />
                    </div>
                    <div class="form-group" style="width:49%">
                        <label for="tecnicaInformes">Tecnica</label>
                        <select id="tecnicaInformes" name="tecnicaInformes" class="required"></select>
                    </div>
                </div>
                <div style="width:100%; display:flex;">
                    <div class="form-group" style="width:49%; margin-right:1%">
                        <label for="cuentasUUInformes">Cuentas UU</label>
                        <select id="cuentasUUInformes" name="cuentasUUInformes" class="required"></select>
                    </div>
                    <div class="form-group" style="width:49%;">
                        <label for="analistaInformes">Analista</label>
                        <select id="analistaInformes" name="analistaInformes" class="required"></select>
                    </div>
                </div>
                
                <div style="width:100%; display:flex;">
                    <div class="form-group" style="width:49%; margin-right:1%">
                        <label for="muestraInformes">Muestra</label>
                        <input type="text" id="muestraInformes" name="muestraInformes" class="required number" />
                    </div>
                </div>
            </div>
            <div class="columna">
                               
                <div class="form-group">
                    <label for="observacionesInformes">Observaciones</label>
                    <textarea id="observacionesInformes" name="observacionesInformes" rows="10" class="required"></textarea>
                </div>
                <div class="form-group" style="width:49%">
                    <label for="btnSaveInformes">&nbsp;</label>
                    <input type="button" name="btnSaveInformes" id="btnSaveInformes" class="btn" value="Guardar" />
                </div>
            </div>
        </div>`
        $('#fechaEntregaInformes').datepicker({
            dateFormat: 'dd/mm/yy',
            showAnim: 'fadeIn'
        });
    }

    #LoadTableInformes() {
        this.#TablesContainer.innerHTML = `<h1 class="titulo">Historico Registro de Informes</h1>
                                            <div id="tablePlanillaInformes"></div>`
    }


}