export { CardInfoEmpleadoDesvinculacion }

class CardInfoEmpleadoDesvinculacion {
    #ContainerElementId
    #DesvinculacionEmpleadoId
    #EmpleadoId
    #UrlImagenEmpleado
    #NombreEmpleado
    #PorcentageAvanceDesvinculacion
    #CallbackClickBtnEvaluaciones
    #CallbackClickBtnPDF
    constructor({ ContainerElementId, DesvinculacionEmpleadoId, EmpleadoId, UrlImagenEmpleado, NombreEmpleado, PorcentageAvanceDesvinculacion, CallbackClickBtnEvaluaciones, CallbackClickBtnPDF }) {
        this.#ContainerElementId = ContainerElementId;
        this.#DesvinculacionEmpleadoId = DesvinculacionEmpleadoId;
        this.#EmpleadoId = EmpleadoId;
        this.#UrlImagenEmpleado = UrlImagenEmpleado;
        this.#NombreEmpleado = NombreEmpleado;
        this.#PorcentageAvanceDesvinculacion = PorcentageAvanceDesvinculacion
        this.#CallbackClickBtnEvaluaciones = CallbackClickBtnEvaluaciones
        this.#CallbackClickBtnPDF = CallbackClickBtnPDF

        this.#Initialize();
    }

    #Initialize() {
        let container = document.getElementById(this.#ContainerElementId)
        let card = document.createElement("div");
        let imgContainer = document.createElement("div");
        let info = document.createElement("div");
        let img = document.createElement("img");


        let empleadoIdLbl = document.createElement("p");
        let empleadoId = document.createElement("p");
        let nombreEmpleadoLbl = document.createElement("p");
        let nombreEmpleado = document.createElement("p");
        let porcentageAvanceDesvinculacionLbl = document.createElement("p");
        let porcentageAvanceDesvinculacion = document.createElement("p");

        empleadoIdLbl.innerHTML = "ID:";
        nombreEmpleadoLbl.innerHTML = "Nombre:";
        porcentageAvanceDesvinculacionLbl.innerHTML = "% Avance";

        empleadoId.innerHTML = this.#EmpleadoId;
        nombreEmpleado.innerHTML = this.#NombreEmpleado;
        nombreEmpleado.setAttribute('title', this.#NombreEmpleado);
        porcentageAvanceDesvinculacion.innerHTML = this.#PorcentageAvanceDesvinculacion;
        img.src = this.#UrlImagenEmpleado

        info.classList.add("EmpleadoInfo")
        imgContainer.classList.add("imgContainer")
        imgContainer.appendChild(img)

        info.appendChild(empleadoIdLbl)
        info.appendChild(empleadoId)
        info.appendChild(nombreEmpleadoLbl)
        info.appendChild(nombreEmpleado)
        info.appendChild(porcentageAvanceDesvinculacionLbl)
        info.appendChild(porcentageAvanceDesvinculacion)
        
        this.#DrawViewEvaluacionesButton(info)
        this.#DrawPDFButton(info)

        card.appendChild(imgContainer)
        card.appendChild(info)

        card.classList.add("CardInfoEmpleadoDesvinculacion")

        container.appendChild(card)
    }

    #DrawViewEvaluacionesButton(container, des) {
        let btnEvaluaciones = document.createElement("button")
        let iconEvaluaciones = document.createElement("i")

        btnEvaluaciones.appendChild(iconEvaluaciones)
        btnEvaluaciones.classList.add("button")
        iconEvaluaciones.classList.add("material-icons")
        btnEvaluaciones.setAttribute("title", "Evaluaciones")
        btnEvaluaciones.dataset.desvinculacionEmpleadoId = this.#DesvinculacionEmpleadoId
        iconEvaluaciones.dataset.desvinculacionEmpleadoId = this.#DesvinculacionEmpleadoId
        iconEvaluaciones.innerHTML = "manage_search"

        btnEvaluaciones.onclick = this.#triggerEventClickBtnEvaluaciones.bind(this)

        container.appendChild(btnEvaluaciones)



    }

    #DrawPDFButton(container) {
        if (this.#PorcentageAvanceDesvinculacion === 100) {
            let btnPDF = document.createElement("button")
            let iconPDF = document.createElement("i")
            btnPDF.classList.add("button")
            iconPDF.classList.add("material-icons")
            btnPDF.setAttribute("title", "PDF")
            btnPDF.dataset.desvinculacionEmpleadoId = this.#DesvinculacionEmpleadoId
            iconPDF.dataset.desvinculacionEmpleadoId = this.#DesvinculacionEmpleadoId
            iconPDF.innerHTML = "picture_as_pdf"

            btnPDF.onclick = this.#triggerEventClickBtnPDF.bind(this)

            btnPDF.appendChild(iconPDF)
            container.appendChild(btnPDF)
        }
    }
    #triggerEventClickBtnEvaluaciones(e) {
        e.preventDefault()

        this.#CallbackClickBtnEvaluaciones({
            desvinculacionEmpleadoId: e.target.dataset.desvinculacionEmpleadoId
        })

        return false;
    }
    #triggerEventClickBtnPDF(e) {
        e.preventDefault()

        this.#CallbackClickBtnPDF({
            desvinculacionEmpleadoId: e.target.dataset.desvinculacionEmpleadoId
        })

        return false;
    }
}