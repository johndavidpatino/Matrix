export { FormDesvinculacionEmpleado }

import { EmpleadosService } from "../../Services/EmpleadosService.js";
import { DesvinculacionesEmpleadosService } from "../../Services/DesvinculacionesEmpleadosService.js";
import { Loader } from "../Loader/Loader.js";
import { ModalDialog } from "../ModalDialog/ModalDialog.js";

class FormDesvinculacionEmpleado {
    #ContainerElementId;
    #ElementDdlEmpleados;
    #ElementTxtFechaDesvinculacion;
    #ElementTxtMotivosDesvinculacion;
    #CallbackInitProcess;
    constructor({ ContainerElementId, CallbackInitProcess }) {
        this.#ContainerElementId = ContainerElementId;
        this.#CallbackInitProcess = CallbackInitProcess;
        this.#Initialize();
    }
    async #Initialize() {
        let containerExternal = document.getElementById(this.#ContainerElementId);
        let container = document.createElement("div");
        let header = document.createElement("header");
        let section = document.createElement("section");
        let containerFechaDesvinculacion = document.createElement("div");
        let containerMotivosDesvinculacion = document.createElement("div");
        let containerDdlEmpleados = document.createElement("div");
        let containerBtnIniciarProceso = document.createElement("div");
        let title = document.createElement("h4");
        let labelFechaDesvinculacion = document.createElement("label");
        let labelMotivosDesvinculacion = document.createElement("label");
        let labelDdlEmpleados = document.createElement("Empleados");
        let ddlEmpleados = document.createElement("select")
        let btnIniciarProceso = document.createElement("button")

        let txtFechaDesvinculacion = document.createElement("input");
        let txtMotivosDesvinculacion = document.createElement("textarea");

        section.classList.add("FormDesvinculacion");

        ddlEmpleados.setAttribute("id", "ddlEmpleados");
        ddlEmpleados.dataset.placeholder = "--Seleccione--";
        ddlEmpleados.required = true;
        this.#ElementDdlEmpleados = ddlEmpleados;
        await this.#FillDdlEmpleados(ddlEmpleados);

        title.innerHTML = "Desvincular empleado";
        txtMotivosDesvinculacion.setAttribute('rows', 4);
        txtMotivosDesvinculacion.setAttribute('cols', 50);
        txtMotivosDesvinculacion.required = true;
        this.#ElementTxtMotivosDesvinculacion = txtMotivosDesvinculacion;

        txtFechaDesvinculacion.setAttribute('type', "date");
        txtFechaDesvinculacion.required = true;
        this.#ElementTxtFechaDesvinculacion = txtFechaDesvinculacion;
        labelFechaDesvinculacion.innerHTML = "Fecha desvinculación:";
        labelMotivosDesvinculacion.innerHTML = "Motivos desvinculación";
        labelDdlEmpleados.innerHTML = "Empleados";
        btnIniciarProceso.innerHTML = "Iniciar proceso";

        containerDdlEmpleados.classList.add("FormGroupField");
        containerFechaDesvinculacion.classList.add("FormGroupField");
        containerMotivosDesvinculacion.classList.add("FormGroupField");
        containerBtnIniciarProceso.classList.add("FormGroupField");

        containerDdlEmpleados.appendChild(labelDdlEmpleados);
        containerDdlEmpleados.appendChild(ddlEmpleados);
        containerFechaDesvinculacion.appendChild(labelFechaDesvinculacion);
        containerFechaDesvinculacion.appendChild(txtFechaDesvinculacion);
        containerMotivosDesvinculacion.appendChild(labelMotivosDesvinculacion);
        containerMotivosDesvinculacion.appendChild(txtMotivosDesvinculacion);
        containerBtnIniciarProceso.appendChild(btnIniciarProceso);

        section.appendChild(containerDdlEmpleados);
        section.appendChild(containerFechaDesvinculacion);
        section.appendChild(containerMotivosDesvinculacion);
        section.appendChild(containerBtnIniciarProceso);

        header.appendChild(title);

        container.appendChild(header);
        container.appendChild(section);

        containerExternal.appendChild(container);

        btnIniciarProceso.onclick = this.#TriggerEventClickBtnInitProcess.bind(this);

        $(ddlEmpleados).chosen();
    }
    #IsValidForm(formClassName) {
        let isValid = true;
        let containerForm = document.getElementsByClassName(formClassName)[0];

        let fields = document.querySelectorAll(`.${formClassName} input, .${formClassName} select, .${formClassName} textarea`);

        fields.forEach((element, index) => {
            let elementChosen;
            if (element.tagName == "SELECT") {
                elementChosen = document.getElementById(element.id + "_chosen");
            }

            if (!element.validity.valid && (elementChosen || this.#ElementIsVisible(element))) {
                if (elementChosen) {
                    elementChosen.classList.add('invalid');
                    console.log("Entro a chosen");
                }
                else {
                    element.classList.add('invalid');
                }
                isValid = false;
            }
            else {
                if (elementChosen) {
                    elementChosen.classList.remove('invalid');
                }
                else {
                    element.classList.remove('invalid');
                }

            }
        });

        return isValid;
    }
    #ElementIsVisible(element) {
        return !(element.offsetParent === null);
    }
    async #TriggerEventClickBtnInitProcess(event) {
        event.preventDefault();
        if (this.#IsValidForm("FormDesvinculacion")) {
            let loader = new Loader();
            loader.show();
            let result = await DesvinculacionesEmpleadosService.IniciarProcesoDesvinculacion({
                empleadoId: this.#ElementDdlEmpleados.value,
                fechaRetiro: this.#ElementTxtFechaDesvinculacion.value,
                motivosDesvinculacion: this.#ElementTxtMotivosDesvinculacion.value
            });
            loader.close();
            this.#ClearForm();
            this.#CallbackInitProcess({
                Status: true
            });
            let modalConfiguration = {
                title: "Información!",
                widthPixels: "400",
                heightPixels: "200",
                actionButtons: [],
                bodyText: "Registro almacenado con exito!!"
            }

            let modalProcessoExitoso = new ModalDialog(modalConfiguration);
            modalProcessoExitoso.show()
        }
        return false;
    }
    async  #FillDdlEmpleados() {
        this.#ElementDdlEmpleados.innerHTML = "";

        let loader = new Loader();
        loader.show();
        let empleadosActivos = await EmpleadosService.EmpleadosActivos();
        loader.close();
        var opt = document.createElement('option');
        opt.value = "";
        opt.innerHTML = "--Seleccione--";
        this.#ElementDdlEmpleados.appendChild(opt);
        empleadosActivos.forEach(element => {
            opt = document.createElement('option');
            opt.value = element.Id;
            opt.innerHTML = element.Id + " - " + element.NombreCompleto;
            this.#ElementDdlEmpleados.appendChild(opt);
        });

        if ($(this.#ElementDdlEmpleados).data("chosen")) {
            $(this.#ElementDdlEmpleados).trigger("chosen:updated");
        }
        
    }
    #ClearForm() {
        this.#FillDdlEmpleados();
        this.#ElementTxtFechaDesvinculacion.value = "";
        this.#ElementTxtMotivosDesvinculacion.value = "";
    }
}