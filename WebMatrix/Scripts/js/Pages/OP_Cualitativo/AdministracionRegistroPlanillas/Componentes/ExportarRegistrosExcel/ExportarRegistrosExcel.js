export { ExportarRegistrosExcel }

import { Loader } from "/Scripts/js/Components/Loader/Loader.js";
import { ModalDialog } from "/Scripts/js/Components/ModalDialog/ModalDialog.js";
import { RegistroPlanillasCualitativoService } from "/Scripts/js/Services/RegistroPlanillasCualitativoService.js"

class ExportarRegistrosExcel {
    #Container
    #ContainerName
    #Fields
    #ModalWindow
    #FiltroFechaInicio
    #FiltroFechaFin
    #FiltroTipoPlantilla
    #FiltroBtnGeneraExcel
    #ModalWindowName
    #btnGeneraExcelName

    constructor(containerId, containerIdName) {
        this.#Container = containerId
        this.#ContainerName = containerIdName
        this.#btnGeneraExcelName = "btnGenerarExcel"
        this.#ModalWindowName = "modalWindowExport"
        this.#Initialize()
    }

    #Initialize() {
        this.#CreateModalWindow();

        let modalConfiguration = {
            title: "Exportar registros",
            bodyElementId: this.#ModalWindowName,
            widthPixels: "",
            heightPixels: "",
            actionButtons: [],
        }

        this.#ModalWindow = new ModalDialog(modalConfiguration);
    }

    #CreateModalWindow() {
        this.#ModalWindow = document.createElement("div")
        this.#ModalWindow.id = this.#ModalWindowName

        let divColumna = document.createElement("div")
        divColumna.className = "columna"

        this.#FiltroFechaInicio = document.createElement("input")
        this.#FiltroFechaInicio.type = "text"
        this.#FiltroFechaInicio.id = "fechaInicioExportar"
        this.#FiltroFechaInicio.name = "fechaInicioExportar"
        this.#FiltroFechaInicio.className = "dateTimePicker"
        this.#FiltroFechaInicio.value = this.#getDate();

        let labelFechaInicio = document.createElement("label");
        labelFechaInicio.htmlFor = "fechaInicioExportar";
        labelFechaInicio.textContent = "Fecha Inicio";

        let divInputContainerFinicio = document.createElement("div")
        divInputContainerFinicio.className = "form-group"

        divInputContainerFinicio.appendChild(labelFechaInicio)
        divInputContainerFinicio.appendChild(this.#FiltroFechaInicio)
        divColumna.appendChild(divInputContainerFinicio)

        this.#FiltroFechaFin = document.createElement("input")
        this.#FiltroFechaFin.type = "text"
        this.#FiltroFechaFin.id = "fechaFinExportar"
        this.#FiltroFechaFin.name = "fechaFinExportar"
        this.#FiltroFechaFin.className = "dateTimePicker"
        this.#FiltroFechaFin.value = this.#getDate();

        let labelFechaFin = document.createElement("label");
        labelFechaFin.htmlFor = "fechaInicioExportar";
        labelFechaFin.textContent = "Fecha Final";

        let divInputContainerFfinal = document.createElement("div")
        divInputContainerFfinal.className = "form-group"

        divInputContainerFfinal.appendChild(labelFechaFin)
        divInputContainerFfinal.appendChild(this.#FiltroFechaFin)
        divColumna.appendChild(divInputContainerFfinal)

        this.#FiltroTipoPlantilla = document.createElement("select")
        this.#FiltroTipoPlantilla.id = "tipoPlantilla"
        this.#FiltroTipoPlantilla.name = "tipoPlantilla"

        let optionModeracion = document.createElement("option")
        optionModeracion.value = "Moderacion"
        optionModeracion.text = "Moderacion"

        let optionInformes = document.createElement("option")
        optionInformes.value = "Informes"
        optionInformes.text = "Informes"

        this.#FiltroTipoPlantilla.appendChild(optionModeracion)
        this.#FiltroTipoPlantilla.appendChild(optionInformes)

        let labelTipoPlantilla = document.createElement("label");
        labelTipoPlantilla.htmlFor = "tipoPlantilla";
        labelTipoPlantilla.textContent = "Tipo de Plantilla";

        let divInputContainerTipoPlantilla = document.createElement("div")
        divInputContainerTipoPlantilla.className = "form-group"

        divInputContainerTipoPlantilla.appendChild(labelTipoPlantilla)
        divInputContainerTipoPlantilla.appendChild(this.#FiltroTipoPlantilla)
        divColumna.appendChild(divInputContainerTipoPlantilla)

        this.#FiltroBtnGeneraExcel = document.createElement("input")
        this.#FiltroBtnGeneraExcel.value = "Exportar"
        this.#FiltroBtnGeneraExcel.id = "btnGeneraExcel"
        this.#FiltroBtnGeneraExcel.name = "btnGeneraExcel"
        this.#FiltroBtnGeneraExcel.type = "button"

        let labelBtnGenerarExcel = document.createElement("label");
        labelBtnGenerarExcel.htmlFor = this.#btnGeneraExcelName;
        labelBtnGenerarExcel.textContent = "";

        let divInputContainerBtnGeneraExcel = document.createElement("div")
        divInputContainerBtnGeneraExcel.className = "form-group"

        divInputContainerBtnGeneraExcel.appendChild(labelBtnGenerarExcel)
        divInputContainerBtnGeneraExcel.appendChild(this.#FiltroBtnGeneraExcel)
        divColumna.appendChild(divInputContainerBtnGeneraExcel)

        this.#ModalWindow.appendChild(divColumna)
        document.body.appendChild(this.#ModalWindow)

        this.#InitializeModalEvents()

    }

    ShowModalFilters() {
        this.#ModalWindow.show()
    }

    #InitializeModalEvents() {
        this.#FiltroBtnGeneraExcel.addEventListener("click", this.#GenerateExcel.bind(this))
        $("#modalWindowExport #fechaInicioExportar").datepicker({
            dateFormat: 'dd/mm/yy',
            showAnim: 'fadeIn',
            beforeShow: function (input, inst) {
                let idDatePicker = document.getElementById("ui-datepicker-div");
                let modalWindow = document.getElementById("modalWindowExport");
                modalWindow.appendChild(idDatePicker)
            }
        });

        $("#modalWindowExport #fechaFinExportar").datepicker({
            dateFormat: 'dd/mm/yy',
            showAnim: 'fadeIn',
            beforeShow: function (input, inst) {
                let idDatePicker = document.getElementById("ui-datepicker-div");
                let modalWindow = document.getElementById("modalWindowExport");
                modalWindow.appendChild(idDatePicker)
            }
        });
    }

    async #GenerateExcel() {
        let loader = new Loader();
        let fechaInicio = this.#FiltroFechaInicio.value
        let fechaFinal = this.#FiltroFechaFin.value
        let tipoPlanilla = this.#FiltroTipoPlantilla.value

        loader.show()
        let responseFile = await RegistroPlanillasCualitativoService.ExportExcelPlanillasBy({ fechaInicio: fechaInicio, fechaFinal: fechaFinal, tipoPlanilla: tipoPlanilla });
        var blob = new Blob([this.#Base64ToBytes(responseFile)], { type: 'application/octetstream' });
        var link = document.createElement("a");

        link.href = window.URL.createObjectURL(blob);
        link.download = "Planillas.xlsx";
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);

        this.#ModalWindow.close()
        loader.close()
    }

    #getDate() {
        const date = new Date();
        const day = String(date.getDate()).padStart(2, '0');
        const month = String(date.getMonth() + 1).padStart(2, '0');
        const year = date.getFullYear();

        return `${day}/${month}/${year}`;
    }
    #Base64ToBytes(base64) {
        var s = window.atob(base64);
        var bytes = new Uint8Array(s.length);
        for (var i = 0; i < s.length; i++) {
            bytes[i] = s.charCodeAt(i);
        }
        return bytes;
    };


}

