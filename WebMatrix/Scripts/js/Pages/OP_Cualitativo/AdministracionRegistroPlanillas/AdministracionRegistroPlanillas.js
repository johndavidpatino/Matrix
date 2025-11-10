import { Table } from "/Scripts/js/Components/Table/Table.js"
import { Loader } from "/Scripts/js/Components/Loader/Loader.js"
import { ModalDialog } from "/Scripts/js/Components/ModalDialog/ModalDialog.js"
import { FormValidator } from "/Scripts/js/Components/FormValidator/FormValidator.js"

import { RegistroPlanillasCualitativoService } from "/Scripts/js/Services/RegistroPlanillasCualitativoService.js"
import { ExportarRegistrosExcel } from "./Componentes/ExportarRegistrosExcel/ExportarRegistrosExcel.js"

export { AdministracionRegistroPlanillas }

class AdministracionRegistroPlanillas {
    #TablesContainer
    #btnSave
    #ModalWindow
    #ModalWindowInformes
    #ModalContainer
    #ModalContainerId
    #TablePlanilla
    #TipoPlantilla
    #BtnFiltro
    #BtnExcel
    #BtnExcelName
    #ExportarExcelObj
    #ID_ESTADO_APROBACION_ENESPERA = 1
    #ID_ESTADO_APROBACION_APROBADO = 2
    #ID_ESTADO_APROBACION_NOAPROBADO = 3
    constructor() {
        this.#TablesContainer = document.getElementById("TablesContainer")
        this.#TipoPlantilla = document.getElementById("tipoPlantilla")
        this.#ModalContainerId = "ModalContainer"
        this.#ModalContainer = document.getElementById(this.#ModalContainerId)
        this.#BtnFiltro = document.getElementById("btnFiltro")
        this.#BtnExcelName = "btnExcel"
        this.#BtnExcel = document.getElementById(this.#BtnExcelName)
        this.#Initialize();
    }

    #Initialize() {
        this.#BtnFiltro.addEventListener("click", this.#FilterTable.bind(this))
        this.#BtnExcel.addEventListener("click", this.#ExportExcel.bind(this))
        this.#LoadTableGral();
    }

    #ExportExcel() {
        if (this.#ExportarExcelObj == null) {
            console.log("Entro aquí")
            this.#ExportarExcelObj = new ExportarRegistrosExcel(this.#BtnExcel, this.#BtnExcelName)
        }
        this.#ExportarExcelObj.ShowModalFilters()
    }

    #FilterTable() {
        let tipoPlanilla = document.getElementById("tipoPlantilla").value
        let statusPlanilla = document.getElementById("statusRegistro").value
        this.#FillTableRegistros(10, 1, tipoPlanilla, statusPlanilla)
    }

    #LoadTableGral() {
        let tipoPlanilla = document.getElementById("tipoPlantilla").value
        let statusPlanilla = document.getElementById("statusRegistro").value

        this.#TablesContainer.innerHTML = `<h1 class="titulo">Historico de Registros</h1>
                                          <div id="tablePlanilla"></div>`
        this.#TablePlanilla = document.getElementById("tablePlanilla")
        this.#FillTableRegistros(10, 1, tipoPlanilla, statusPlanilla)
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
            //{ sourcename: "Observaciones", show: false, nameToShow: "Obs" },
        ];

        let tableConfiguration = {
            containerElement: this.#TablePlanilla,
            data: data,
            columsConfiguration: columnsConfiguration,
            pageSize: pageSize,
            actionButtons: [{ name: "Evaluar", text: "Ver/Evaluar" }],
            totalRows: data[0] ? data[0].TotalRows : 0,
            pageIndex: pageIndex,
            pagination: true,
        }

        let table = new Table(tableConfiguration)
        table.addEventListenerChangePage(async (e) => await this.#FillTableRegistros(pageSize, e.currentPage, filtroPlanilla, idEstado))
        table.addEventListenerActionButtonClick((e) => this.#OnClickButtonTablePlanillaModeracion(e))
    }

    #OnClickButtonTablePlanillaModeracion(item) {
        let dataItem = item.dataItem
        if (dataItem.TipoRegistro == "Moderacion") this.#OpenModalEvaluarModeracion(dataItem)
        if (dataItem.TipoRegistro == "Informes") this.#OpenModalEvaluarInformes(dataItem)
    }

    async #OpenModalEvaluarModeracion(item) {
        let loader = new Loader()
        loader.show()
        let idPlanilla = item.IdRegistro
        let data = await RegistroPlanillasCualitativoService.GetPlanillaModeracionById({ idPlanilla: idPlanilla })
        let observacionesPlanillaInput = "";
        let buttonsPlanilla = "";
        let biSL = data.BI_WBSL;
        let biStatus = data.BI_Status;
        let biDinero = data.BI_3320_Moderacion_DineroDisponible;
        let biStatusInput = `<b>Estado BI:</b> ${biStatus} <input type="hidden" id="bi_status" name="bi_status" value="${biStatus}" />`
        let biDineroInput = `<b>Dinero BI:</b> ${biDinero} <input type="hidden" id="bi_dinero" name="bi_dinero" value="${biDinero}" />`
        let defaultMessageHtml = ""
        let statusPlanilla = data.IdEstado;

        if (statusPlanilla != this.#ID_ESTADO_APROBACION_ENESPERA) {
            observacionesPlanillaInput = data.ObservacionesAprobacion;
        } else {
            observacionesPlanillaInput = '<textarea class="required" id="observacionesModeracion" name="observacionesModeracion" style="width: 100%;" rows="10"></textarea>';
            buttonsPlanilla = `
                    <div class="form-group">
                        <label for="btnSaveStatus">&nbsp;</label>
                        <button name="btnApproveStatusModeracion" id="btnApproveStatusModeracion" class="btn" style="width: 45% !important">Aprobar</button>
                        <label for="btnSaveStatus">&nbsp;</label>
                        <button name="btnDennyStatusModeracion" id="btnDennyStatusModeracion" class="btn" style="background: red; width: 45% !important">Rechazar</button>
                    </div>`

            if (biDinero.length <= 0) {
                let biDefaultMessage = data.BI_DefaultMessage;
                biStatusInput = `<label for="bi_status"><b>Bi Status</b></label>
                             <input class="required" type="text" id="bi_status" id="bi_status" style="width:100%" />`
                biDineroInput = `<label for="bi_dinero"><b>Bi Dinero</b></label>
                             <input class="required" type="text" id="bi_dinero" id="bi_dinero" style="width:100%"  />`
                defaultMessageHtml = `<div class="infoElement">
                                    <b style="color:red;">${biDefaultMessage}</b>
                                </div>`
            }
        }

        loader.close()

        this.#ModalContainer.innerHTML = `<div id="ModeracionModal">
            <div class="columna">
                <div id="jobDetailInfo">
                    <div class="infoElement">
                        <b>Fecha de Registro:</b> ${data.fecha}
                        <input type="hidden" id="idPlantillaModeracion" name="idPlantillaModeracion" value="${data.Id}" />
                        <input type="hidden" id="idJob" name="idJob" value="${data.IdJob}" />
                    </div>
                    <div class="infoElement">
                        <b>Usuario Registra:</b> ${data.moderador}
                    </div>
                    <div class="infoElement">
                        <b>JobBook:</b>  ${data.IdJob}
                    </div>
                    <div class="infoElement">
                        <b>Nombre Proyecto:</b>  ${data.jobDesc}
                    </div>
                    <div class="infoElement">
                        <b>SL:</b> ${biSL}
                    </div>
                    <div class="infoElement">
                        ${biStatusInput}
                    </div>
                    <div class="infoElement">
                        ${biDineroInput}
                    </div>
                    ${defaultMessageHtml}
                    <div class="infoElement">
                        <b>Cuentas UU:</b> ${data.cuentasUU}
                    </div>
                    <div class="infoElement">
                        <b>Moderador:</b>   ${data.moderador}
                    </div>
                    <div class="infoElement">
                        <b>Tiempo:</b>   ${data.tiempo}
                    </div>
                    <div class="infoElement">
                        <b>Puntos:</b>   ${data.puntos}
                    </div>
                    <div class="infoElement">
                        <b>Rol:</b>   ${data.rol}
                    </div>
                    <div class="infoElement">
                        <b>Observaciones:</b>   ${data.Observaciones}
                    </div>
              </div>
            </div>
            <div class="columna">
                <div class="form-group">
                    <label for="tiempo">Observaciones</label>
                    ${observacionesPlanillaInput}
                </div>
                ${buttonsPlanilla}
            </div>
        </div>`

        this.#ModalContainer.style.display = 'block'


        let modalConfiguration = {
            title: "Evaluación Planilla Moderación",
            bodyElementId: this.#ModalContainerId,
            widthPixels: "800",
            heightPixels: "600",
            actionButtons: [],
        }
        this.#ModalWindow = new ModalDialog(modalConfiguration);
        if (statusPlanilla == this.#ID_ESTADO_APROBACION_ENESPERA) this.#ModalWindow.addEventListenerActionButtonClick((e) => this.#ConfigureEventsModalModeracion(e));
        this.#ModalWindow.show();

        if (statusPlanilla == this.#ID_ESTADO_APROBACION_ENESPERA) this.#ConfigureEventsModalModeracion()

    }

    #ConfigureEventsModalModeracion() {
        document.getElementById("btnApproveStatusModeracion").addEventListener("click", this.#ChangeStatusModeracion.bind(this, "Aprobar"))
        document.getElementById("btnDennyStatusModeracion").addEventListener("click", this.#ChangeStatusModeracion.bind(this, "Rechazar"))
    }

    async #ChangeStatusModeracion(statusPlanilla) {
        let loader = new Loader()
        let validator = new FormValidator();

        validator.Initialize("ModeracionModal");
        if (validator.IsFormValid()) {
            let idPlanilla = document.getElementById("idPlantillaModeracion").value;
            let observaciones = document.getElementById("observacionesModeracion").value;
            let idJob = document.getElementById("idJob").value;
            let biStatus = document.getElementById("bi_status").value;
            let biDinero = document.getElementById("bi_dinero").value;
            loader.show()
            let idEstado = statusPlanilla == "Aprobar" ? this.#ID_ESTADO_APROBACION_APROBADO : this.#ID_ESTADO_APROBACION_NOAPROBADO
            let data = await RegistroPlanillasCualitativoService.SaveStatusAprobacionModeracion({ idPlanilla: idPlanilla, idJob: idJob, idEstado: idEstado, observaciones: observaciones, biStatus: biStatus, biDinero: biDinero })
            loader.close()
            let filtroPlanilla = document.getElementById("tipoPlantilla").value
            let filtroStatus = document.getElementById("statusRegistro").value
            this.#FillTableRegistros(10, 1, filtroPlanilla, filtroStatus)
            this.#ModalWindow.close()
        }
    }

    async #OpenModalEvaluarInformes(item) {
        let loader = new Loader()
        loader.show()
        let idPlanilla = item.IdRegistro
        let data = await RegistroPlanillasCualitativoService.GetPlanillaInformesById({ idPlanilla: idPlanilla })
        loader.close()
        let observacionesPlanillaInput = "";
        let buttonsPlanilla = "";
        let biSL = data.BI_WBSL;
        let biStatus = data.BI_Status;
        let biDinero = data.BI_3320_Moderacion_DineroDisponible;
        let biStatusInput = `<b>Estado BI:</b> ${biStatus} <input type="hidden" id="bi_status" name="bi_status" value="${biStatus}" />`
        let biDineroInput = `<b>Dinero BI:</b> ${biDinero} <input type="hidden" id="bi_dinero" name="bi_dinero" value="${biDinero}" />`
        let defaultMessageHtml = ""
        let statusPlanilla = data.IdEstado;

        if (statusPlanilla != this.#ID_ESTADO_APROBACION_ENESPERA) {
            observacionesPlanillaInput = data.ObservacionesAprobacion;
        }
        else {
            if (biDinero.length <= 0) {
                let biDefaultMessage = data.BI_DefaultMessage;
                biStatusInput = `<label for="bi_status"><b>Bi Status</b></label>
                             <input type="text" id="bi_status" id="bi_status" style="width:100%" class="required"/> ${data.BI_Status}`
                biDineroInput = `<label for="bi_dinero"><b>Bi Dinero</b></label>
                             <input type="text" id="bi_dinero" id="bi_dinero" style="width:100%" class="required"  />`
                defaultMessageHtml = `<div class="infoElement">
                                    <b style="color:red;">${biDefaultMessage}</b>
                                </div>`
            }


            observacionesPlanillaInput = '<textarea id="observacionesInformes" name="observacionesInformes" style="width: 100%;" rows="10" class="required"></textarea>';
            buttonsPlanilla = `
                    <div class="form-group">
                        <label for="btnSaveStatus">&nbsp;</label>
                        <button name="btnApproveStatusInformes" id="btnApproveStatusInformes" class="btn" style="width: 45% !important">Aprobar</button>
                        <label for="btnSaveStatus">&nbsp;</label>
                        <button name="btnDenyStatusInformes" id="btnDenyStatusInformes" class="btn" style="background: red; width: 45% !important">Rechazar</button>
                    </div>`
        }

        this.#ModalContainer.innerHTML = `<div id="ModeracionModal">
            <div class="columna">
                <div id="jobDetailInfo">
                    <div class="infoElement">
                        <b>Fecha de Registro:</b> ${data.fecha}
                        <input type="hidden" id="idPlantillaInformes" name="idPlantillaInformes" value="${data.Id}" />
                        <input type="hidden" id="idJobInformes" name="idJobInformes" value="${data.IdJob}" />
                    </div>
                    <div class="infoElement">
                        <b>JobBook:</b>  ${data.IdJob}
                    </div>
                    <div class="infoElement">
                        <b>Nombre Proyecto:</b>  ${data.jobDesc}
                    </div>
                    <div class="infoElement">
                        <b>SL:</b> ${biSL}
                    </div>
                    <div class="infoElement">
                        ${biStatusInput}
                    </div>
                    <div class="infoElement">
                        ${biDineroInput}
                    </div>
                    ${defaultMessageHtml}
                    <div class="infoElement">
                        <b>Cuentas UU:</b> ${data.cuentasUU}
                    </div>
                    <div class="infoElement">
                        <b>Tecnica:</b>   ${data.tecnica}
                    </div>
                    <div class="infoElement">
                        <b>Muestra:</b>   ${data.muestra}
                    </div>
                    <div class="infoElement">
                        <b>Analista:</b>   ${data.analista}
                    </div>
                    <div class="infoElement">
                        <b>Observaciones:</b>   ${data.Observaciones}
                    </div>
              </div>
            </div>
            <div class="columna">
                <div class="form-group">
                    <label for="tiempo">Observaciones</label>
                    ${observacionesPlanillaInput}        
                </div>
                ${buttonsPlanilla}
            </div>
        </div>`

        this.#ModalContainer.style.display = 'block'


        let modalConfiguration = {
            title: "Evaluación Planilla Informes",
            bodyElementId: this.#ModalContainerId,
            widthPixels: "800",
            heightPixels: "600",
            actionButtons: [],
        }
        this.#ModalWindowInformes = new ModalDialog(modalConfiguration);
        if (statusPlanilla == this.#ID_ESTADO_APROBACION_ENESPERA) this.#ModalWindowInformes.addEventListenerActionButtonClick((e) => this.#ConfigureEventsModalInformes(e));
        this.#ModalWindowInformes.show();

        if (statusPlanilla == this.#ID_ESTADO_APROBACION_ENESPERA) this.#ConfigureEventsModalInformes()
    }

    #ConfigureEventsModalInformes() {
        document.getElementById("btnApproveStatusInformes").addEventListener("click", this.#ChangeStatusInformes.bind(this, "Aprobar"))
        document.getElementById("btnDenyStatusInformes").addEventListener("click", this.#ChangeStatusInformes.bind(this, "Rechazar"))
    }

    async #ChangeStatusInformes(statusPlanilla) {
        let loader = new Loader()
        let validator = new FormValidator();
        validator.Initialize("ModeracionModal");
        if (validator.IsFormValid()) {
            let idPlanilla = document.getElementById("idPlantillaInformes").value;
            let observaciones = document.getElementById("observacionesInformes").value;
            let idJob = document.getElementById("idJobInformes").value;
            let biStatus = document.getElementById("bi_status").value;
            let biDinero = document.getElementById("bi_dinero").value;
            let idEstado = statusPlanilla == "Aprobar" ? this.#ID_ESTADO_APROBACION_APROBADO : this.#ID_ESTADO_APROBACION_NOAPROBADO
            loader.show()
            let data = await RegistroPlanillasCualitativoService.SaveStatusAprobacionInformes({ idPlanilla: idPlanilla, idJob: idJob, idEstado: idEstado, observaciones: observaciones, biStatus: biStatus, biDinero: biDinero })
            loader.close()

            let filtroPlanilla = document.getElementById("tipoPlantilla").value
            let filtroStatus = document.getElementById("statusRegistro").value
            this.#FillTableRegistros(10, 1, filtroPlanilla, filtroStatus)
            this.#ModalWindowInformes.close()
        }



    }


}