import { Table } from "../../Components/Table/Table.js"
import { Loader } from "../../Components/Loader/Loader.js"
import { ModalDialog } from "../../Components/ModalDialog/ModalDialog.js"

import { DesvinculacionesEmpleadosService } from "../../Services/DesvinculacionesEmpleadosService.js"

export class DesvinculacionesEmpleadosGestionArea {
    #ElementTablaProcesosDesvinculacionEnCurso;
    #ElementTablaProcesosDesvinculacionRealizados;
    #ElementObjProcesosDesvinculacionEnCurso;
    #ElementModalContentEvaluacion;
    #ElementModalContentEvaluacionId;
    #ElementModalContentObjTablaItemsAVerificar
    #ElementModalContentContainerTablaItemsAVerificar
    #ElementModalContetObjTablaInformacionEmpleado
    #ElementModalContentContainerInformacionEmpleado
    #ElementBtnGuardarEvaluacion
    #ElementTxtComentarios
    #DatosEmpleadoEnEvaluacionActual
    #ElementObjModalEvaluacion
    constructor() {
        this.#Initialize()
    }

    async #Initialize() {
        this.#ElementTablaProcesosDesvinculacionEnCurso = document.getElementById("ProcesosDesvinculacionEnCurso")
        this.#ElementTablaProcesosDesvinculacionRealizados = document.getElementById("ProcesosDesvinculacionRealizados")
        this.#ElementObjProcesosDesvinculacionEnCurso = null
        this.#ElementModalContentEvaluacionId = "ModalContentEvaluacion"
        this.#ElementModalContentEvaluacion = document.getElementById(this.#ElementModalContentEvaluacionId)
        this.#ElementModalContentContainerTablaItemsAVerificar = document.getElementById("tblItemsAreaEvaluar")
        this.#ElementModalContentContainerInformacionEmpleado = document.getElementById("tblDatosEmpleado")
        this.#ElementBtnGuardarEvaluacion = document.getElementById("btnGuardarEvaluacion")
        this.#ElementBtnGuardarEvaluacion.onclick = this.#OnClickButtonGuardarEvaluacion.bind(this)
        this.#ElementTxtComentarios = document.getElementById("txtObservacionesEvaluacion")
        this.#ElementObjModalEvaluacion = null
        await this.#LoadTableProcesosDesvinculacionEnCurso({ pageSize: 10, pageIndex: 1 })
        await this.#LoadTableProcesosDesvinculacionRealizados({ pageSize: 10, pageIndex: 1 })
    }

    async #LoadTableProcesosDesvinculacionEnCurso({ pageSize, pageIndex }) {
        let loader = new Loader()
        loader.show()
        let data = await DesvinculacionesEmpleadosService.ProcesosDesvinculacionPendientesPorEvaluarUsuarioActual()
        loader.close()
        const columnsConfiguration = [
            { sourcename: "DesvinculacionEmpleadoId", show: false, nameToShow: "" },
            { sourcename: "EmpleadoId", show: false, nameToShow: "" },
            { sourcename: "AreaId", show: false, nameToShow: "" },
            { sourcename: "NombreEmpleadoCompleto", show: true, nameToShow: "Nombre Empleado" },
            { sourcename: "FechaRegistro", show: true, nameToShow: "Fecha registro desvinculación" },
            { sourcename: "NombreArea", show: true, nameToShow: "Area a evaluar" },
        ];

        let tableConfiguration = {
            containerElement: this.#ElementTablaProcesosDesvinculacionEnCurso,
            data: data,
            columsConfiguration: columnsConfiguration,
            pageSize: pageSize,
            actionButtons: [
                {
                    name: "Gestionar",
                    text: "Gestionar",
                }
            ],
            totalRows: data.length,
            pageIndex: pageIndex,
            pagination: false,
        }

        this.#ElementObjProcesosDesvinculacionEnCurso = new Table(tableConfiguration)
        this.#ElementObjProcesosDesvinculacionEnCurso.addEventListenerChangePage((e) => this.#LoadTableProcesosDesvinculacionEnCurso({ pageSize, pageIndex: e.currentPage }))
        this.#ElementObjProcesosDesvinculacionEnCurso.addEventListenerActionButtonClick((e) => this.#OnClickButtonTablaProcesosDesvinculacionEnCurso(e));
    }

    async #LoadModalContentItemsAVerificar({ pageSize, pageIndex, AreaId }) {
        let data = await DesvinculacionesEmpleadosService.ItemsAVerificarPor({ AreaId: AreaId })

        const columnsConfiguration = [
            { sourcename: "Id", show: false, nameToShow: "" },
            { sourcename: "AreaId", show: false, nameToShow: "" },
            { sourcename: "Descripcion", show: true, nameToShow: "Descripción" },
        ];

        let tableConfiguration = {
            containerElement: this.#ElementModalContentContainerTablaItemsAVerificar,
            data: data,
            columsConfiguration: columnsConfiguration,
            pageSize: pageSize,
            actionButtons: [],
            totalRows: data.length,
            pageIndex: pageIndex,
            pagination: false,
            showColumnsNames: false
        }

        this.#ElementModalContentObjTablaItemsAVerificar = new Table(tableConfiguration)
        this.#ElementModalContentObjTablaItemsAVerificar.addEventListenerChangePage((e) => this.#LoadModalContentItemsAVerificar({ pageSize, pageIndex: e.currentPage }))
    }

    async #LoadProcesoDesvinculacionInformacionEmpleado(DesvinculacionEmpleadoId) {
        let employeeInfo = await DesvinculacionesEmpleadosService.InformacionEmpleadoPor({ DesvinculacionEmpleadoId: DesvinculacionEmpleadoId })
        let employeeInfoMapped = []

        //console.log(employeeInfo)
        //if (employeeInfo === null) return;


        employeeInfoMapped.push({ "Property": "EmpleadoId", "Value": employeeInfo.EmpleadoId })
        employeeInfoMapped.push({ "Property": "Nombre empleado", "Value": employeeInfo.NombreEmpleadoCompleto })
        employeeInfoMapped.push({ "Property": "Fecha retiro", "Value": employeeInfo.FechaRetiro })
        employeeInfoMapped.push({ "Property": "Cargo", "Value": employeeInfo.Cargo })

        const columnsConfiguration = [
            { sourcename: "Property", show: true, nameToShow: "-" },
            { sourcename: "Value", show: true, nameToShow: "-" },
        ];

        let tableConfiguration = {
            containerElement: this.#ElementModalContentContainerInformacionEmpleado,
            data: employeeInfoMapped,
            columsConfiguration: columnsConfiguration,
            pageSize: 10,
            actionButtons: [],
            totalRows: employeeInfoMapped.length,
            pageIndex: 1,
            pagination: false,
            showColumnsNames: false
        }

        this.#ElementModalContetObjTablaInformacionEmpleado = new Table(tableConfiguration)
    }

    async #OnClickButtonTablaProcesosDesvinculacionEnCurso(e) {
        let loader = new Loader()
        this.#DatosEmpleadoEnEvaluacionActual = e.dataItem
        loader.show()
        await this.#LoadProcesoDesvinculacionInformacionEmpleado(e.dataItem.DesvinculacionEmpleadoId)
        await this.#LoadModalContentItemsAVerificar({ pageSize: 10, pageIndex: 1, AreaId: e.dataItem.AreaId })
        loader.close()

        this.#ElementModalContentEvaluacion.style.display = "grid"

        let modalConfiguration = {
            title: "Evaluación de items para desviculanción!",
            bodyElementId: this.#ElementModalContentEvaluacionId,
            widthPixels: "600",
            heightPixels: "400",
            actionButtons: [],
        }
        this.#ElementObjModalEvaluacion = new ModalDialog(modalConfiguration);
        this.#ElementObjModalEvaluacion.show();
    }

    async #OnClickButtonGuardarEvaluacion() {
        let loader = new Loader()
        if (this.#ElementTxtComentarios.value.trim() == "") {
            this.#ElementTxtComentarios.classList.add("invalid")
            return
        }
        else {
            this.#ElementTxtComentarios.classList.remove("invalid")
        }

        let Evaluacion = {
            DesvinculacionEmpleadoId: this.#DatosEmpleadoEnEvaluacionActual.DesvinculacionEmpleadoId,
            AreaId: this.#DatosEmpleadoEnEvaluacionActual.AreaId,
            Comentarios: this.#ElementTxtComentarios.value
        }

        loader.show()
        this.#ElementObjModalEvaluacion.close()
        await DesvinculacionesEmpleadosService.GuardarEvaluacion({ Evaluacion })
        await this.#LoadTableProcesosDesvinculacionEnCurso({ pageSize: 10, pageIndex: 1 })
        await this.#LoadTableProcesosDesvinculacionRealizados({ pageSize: 10, pageIndex: 1 })
        loader.close()

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
    async #LoadTableProcesosDesvinculacionRealizados({ pageSize, pageIndex }) {
        let loader = new Loader()
        loader.show()
        let data = await DesvinculacionesEmpleadosService.EvaluacionesRealizadasPorUsuarioActual()
        loader.close()
        const columnsConfiguration = [
            { sourcename: "DesvinculacionEmpleadoId", show: false, nameToShow: "" },
            { sourcename: "EmpleadoId", show: false, nameToShow: "" },
            { sourcename: "NombreEmpleadoCompleto", show: true, nameToShow: "Nombre Empleado" },
            { sourcename: "FechaDiligenciamiento", show: true, nameToShow: "Fecha evaluación" },
            { sourcename: "NombreArea", show: true, nameToShow: "Area evaluada" },
            { sourcename: "Comentarios", show: true, nameToShow: "Comentarios" },
        ];

        let tableConfiguration = {
            containerElement: this.#ElementTablaProcesosDesvinculacionRealizados,
            data: data,
            columsConfiguration: columnsConfiguration,
            pageSize: pageSize,
            actionButtons: [],
            totalRows: data.length,
            pageIndex: pageIndex,
            pagination: false,
        }

        let evaluacionesRealizadsa = new Table(tableConfiguration)
    }
}