import { ContenedorEmpleadosDesvinculacionEstatus } from "../../Components/ContenedorEmpleadosDesvinculacionEstatus/ContenedorEmpleadosDesvinculacionEstatus.js"
import { FormDesvinculacionEmpleado } from "../../Components/FormDesvinculacionEmpleado/FormDesvinculacionEmpleado.js"
export class DesvinculacionesEmpleadosGestionRRHH {
    #objDesvinculaciones
    constructor() {
        this.#Initialize()
    }

    async #Initialize() {
        let elementContainer = document.getElementById("ContainerPage")
        let elementContainerFormDesvinculacionEmpleado = document.createElement("div");
        let elementContainerDesvinculaciones = document.createElement("div");

        elementContainerDesvinculaciones.setAttribute("id", "ContainerDesvinculaciones");
        elementContainerFormDesvinculacionEmpleado.setAttribute("id", "ContainerFormDesvinculacionEmpleado");

        elementContainer.appendChild(elementContainerFormDesvinculacionEmpleado);
        elementContainer.appendChild(elementContainerDesvinculaciones);

        this.#objDesvinculaciones = new ContenedorEmpleadosDesvinculacionEstatus({ ContainerElementId: "ContainerDesvinculaciones" });
        let containerFormDesvinculacionEmpleado = new FormDesvinculacionEmpleado({ ContainerElementId: "ContainerFormDesvinculacionEmpleado", CallbackInitProcess: this.#OnClickIniciarProceso.bind(this) });
    }
    async #OnClickIniciarProceso(e) {
        await this.#objDesvinculaciones.UpdateCards();
    }
}