import { CardInfoEmpleadoDesvinculacion } from "../CardInfoEmpleadoDesvinculacion/CardInfoEmpleadoDesvinculacion.js"
import { Paginator } from "../Paginator/Paginator.js"
import { SearchBox } from "../SearchBox/SearchBox.js"
import { DesvinculacionesEmpleadosService } from "../../Services/DesvinculacionesEmpleadosService.js"
import { Table } from "../Table/Table.js"
import { ModalDialog } from "../ModalDialog/ModalDialog.js"
import { Loader } from "../Loader/Loader.js"

export { ContenedorEmpleadosDesvinculacionEstatus }

class ContenedorEmpleadosDesvinculacionEstatus {
    #containerElementId;
    #elementContentCards;
    #elementContentPaginator;
    #pageSize = 10;
    #objPaginator;
    #objSearcher;
    #currentTextToSearch = "";
    constructor({ ContainerElementId }) {
        this.#containerElementId = ContainerElementId;
        this.#Initialize();
    }
    async #Initialize() {
        let elementContainer = document.getElementById(this.#containerElementId);
        let container = document.createElement("div");
        let pagination = document.createElement("div");
        let search = document.createElement("div");
        let content = document.createElement("div");
        let header = document.createElement("header");
        let segment = document.createElement("segment");
        let title = document.createElement("h4");

        let loader = new Loader();
        loader.show();
        let desvinculacionesEmpleadosEstatus = await DesvinculacionesEmpleadosService.DesvinculacionesEmpleadosEstatus(
            {
                pageSize: this.#pageSize,
                pageIndex: 1,
                textoBuscado: ''
            });
        loader.close();

        container.classList.add("ContenedorEmpleadosDesvinculacionEstatus");

        title.innerHTML = "Desvinculaciones";

        header.appendChild(title);

        container.appendChild(header);
        container.appendChild(segment);

        segment.appendChild(search);
        segment.appendChild(content);
        segment.appendChild(pagination);
        content.setAttribute("id", "ContentCardsEmpleadosDesvinculacionEstatus")
        content.classList.add("ContentCards");
        pagination.setAttribute("id", "PaginationCardsEmpleadosDesvinculacionEstatus")
        pagination.classList.add("Pagination");
        search.setAttribute("id", "SearchCardsEmpleadosDesvinculacionEstatus")
        search.classList.add("Search");

        this.#elementContentCards = content;
        this.#elementContentPaginator = pagination;

        elementContainer.appendChild(container);

        this.#objSearcher = new SearchBox({
            ContainerElementId: "SearchCardsEmpleadosDesvinculacionEstatus",
            CallbackClickButtonSearch: this.#OnClickButtonSearch.bind(this)
        });

        this.#DrawPaginator(desvinculacionesEmpleadosEstatus);
        this.#DrawCards(desvinculacionesEmpleadosEstatus);

    }
    async #OnChangePaginatorPage(e) {
        let loader = new Loader();
        loader.show();
        let desvinculacionesEmpleadosEstatus = await DesvinculacionesEmpleadosService.DesvinculacionesEmpleadosEstatus(
            {
                pageSize: this.#pageSize,
                pageIndex: this.#currentTextToSearch != this.#objSearcher.CurrentText ? 1 : e.currentPage,
                textoBuscado: this.#objSearcher.CurrentText
            });
        loader.close();
        this.#DrawCards(desvinculacionesEmpleadosEstatus);
        if (this.#currentTextToSearch != this.#objSearcher.CurrentText) {
            this.#DrawPaginator(desvinculacionesEmpleadosEstatus);
            this.#currentTextToSearch = this.#objSearcher.CurrentText
        }
    }
    async #OnClickButtonSearch(e) {
        let loader = new Loader();
        loader.show();
        let desvinculacionesEmpleadosEstatus = await DesvinculacionesEmpleadosService.DesvinculacionesEmpleadosEstatus(
            {
                pageSize: this.#pageSize,
                pageIndex: this.#currentTextToSearch != this.#objSearcher.CurrentText ? 1 : this.#objPaginator.CurrentPage,
                textoBuscado: e.textToSearch
            });
        loader.close();
        this.#DrawCards(desvinculacionesEmpleadosEstatus);
        this.#DrawPaginator(desvinculacionesEmpleadosEstatus);
        this.#currentTextToSearch = e.textToSearch
    }
    async #DrawCards(desvinculacionesEmpleadosEstatus) {

        this.#elementContentCards.innerHTML = '';

        desvinculacionesEmpleadosEstatus.forEach(x => {
            let card = new CardInfoEmpleadoDesvinculacion({
                ContainerElementId: "ContentCardsEmpleadosDesvinculacionEstatus",
                DesvinculacionEmpleadoId: x.DesvinculacionEmpleadoId,
                EmpleadoId: x.EmpleadoId,
                UrlImagenEmpleado: x.urlFoto ?? "https://controlio.net/i/svg/brand-figure.svg",
                NombreEmpleado: x.NombreEmpleadoCompleto,
                PorcentageAvanceDesvinculacion: x.PorcentajeAvanceDesvinculacion,
                CallbackClickBtnEvaluaciones: this.#OnClickCardBtnEvaluaciones.bind(this),
                CallbackClickBtnPDF: this.#OnClickCardBtnPDF.bind(this)
            });
        });
    }
    #DrawPaginator(desvinculacionesEmpleadosEstatus) {
        this.#elementContentPaginator.innerHTML = '';
        this.#objPaginator = new Paginator({
            ContainerElementId: "PaginationCardsEmpleadosDesvinculacionEstatus",
            CallbackChangePage: this.#OnChangePaginatorPage.bind(this),
            TotalRows: desvinculacionesEmpleadosEstatus && desvinculacionesEmpleadosEstatus.length > 0 ? desvinculacionesEmpleadosEstatus[0].CantidadTotalFilas : 0,
            PageSize: this.#pageSize
        });
    }
    async UpdateCards() {
        let loader = new Loader();
        loader.show();
        let desvinculacionesEmpleadosEstatus = await DesvinculacionesEmpleadosService.DesvinculacionesEmpleadosEstatus(
            {
                pageSize: this.#pageSize,
                pageIndex: 1,
                textoBuscado: this.#objSearcher.CurrentText
            });
        loader.close();
        this.#DrawCards(desvinculacionesEmpleadosEstatus);
        this.#DrawPaginator(desvinculacionesEmpleadosEstatus);
    }
    async #OnClickCardBtnEvaluaciones(e) {

        let containerTable = document.createElement("div");

        let data = await DesvinculacionesEmpleadosService.DesvinculacionEmpleadosEstatusEvaluaciones({
            desvinculacionEmpleadoId: e.desvinculacionEmpleadoId
        });

        const columnsConfiguration = [
            { sourcename: "NombreArea", show: true, nameToShow: "Area" },
            { sourcename: "NombreEvaluadorCompleto", show: true, nameToShow: "Nombre Evaluador" },
            { sourcename: "NombresEvaluadores", show: true, nameToShow: "Evaluadores habilitados" },
            { sourcename: "FechaDiligenciamiento", show: true, nameToShow: "Fecha Evaluación" },
            { sourcename: "Comentarios", show: true, nameToShow: "Comentarios" },
            { sourcename: "Estado", show: true, nameToShow: "Estado" },
        ];

        let tableConfiguration = {
            containerElement: containerTable,
            data: data,
            columsConfiguration: columnsConfiguration,
            pageSize: 10,
            actionButtons: [],
            totalRows: data.length,
            pageIndex: 1,
            pagination: false,
            showColumnsNames: true
        }

        let evaluaciones = new Table(tableConfiguration);

        let modalConfiguration = {
            title: "Evaluaciones!",
            bodyElementId: null,
            widthPixels: "600",
            heightPixels: "400",
            actionButtons: [],
            bodyText: containerTable.innerHTML
        }
        let modaldialog = new ModalDialog(modalConfiguration);
        modaldialog.show();
    }
    async #OnClickCardBtnPDF(e) {
        let loader = new Loader();
        loader.show();
        let pdfBase64 = await DesvinculacionesEmpleadosService.DownloadPDFFormat({ desvinculacionEmpleadoId: e.desvinculacionEmpleadoId })
        let pdfBytes = this.#ConvertBase64ToBytes(pdfBase64);
        var pdfBlob = new Blob([pdfBytes], { type: 'application/pdf' });

        let iFrame = document.createElement("iframe");
        iFrame.setAttribute("src", window.URL.createObjectURL(pdfBlob))
        iFrame.setAttribute("width", "100%")
        console.log(iFrame.innerHTML)

        let modalConfiguration = {
            title: "Formato diligenciado!",
            bodyElementId: null,
            widthPixels: "600",
            heightPixels: "400",
            actionButtons: [],
            bodyText: "",
            bodyElement: iFrame
        }
        let modaldialog = new ModalDialog(modalConfiguration);
        loader.close();
        modaldialog.show();

    }
    #ConvertBase64ToBytes(base64String) {
        var byteCharacters = atob(base64String);
        var byteNumbers = new Array(byteCharacters.length);
        for (var i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        var byteArray = new Uint8Array(byteNumbers);
        return byteArray;
    }
}
