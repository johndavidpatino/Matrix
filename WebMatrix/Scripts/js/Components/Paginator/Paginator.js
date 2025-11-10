export { Paginator }

class Paginator {
    #containerElementId;
    #callbackChangePage;
    #currentPage = 1;
    #totalRows;
    #pageSize;
    #controlShowCurrentPage;
    constructor({ ContainerElementId, CallbackChangePage, TotalRows, PageSize }) {
        if (TotalRows == undefined || TotalRows == null) {
            throw new Error("The parameter TotalRows should be a valid value!");
        }
        if (PageSize == undefined || PageSize == 0 || PageSize == null) {
            throw new Error("The parameter PageSize should be a valid value!");
        }
        if (document.getElementById(ContainerElementId) == null) {
            throw new Error("The element ContainerElementId doesn't exist in the DOM!");
        }

        if (typeof CallbackChangePage !== "function") {
            throw new Error("The parameter CallbackChangePage isn't a function!");
        }
        if (typeof CallbackChangePage !== "function") {
            throw new Error("The parameter CallbackChangePage isn't a function!");
        }
        this.#containerElementId = ContainerElementId;
        this.#callbackChangePage = CallbackChangePage;
        this.#totalRows = TotalRows;
        this.#pageSize = PageSize;

        if (TotalRows > 0) {
            this.#Initialize();
        }
    }
    #Initialize() {
        let container = document.getElementById(this.#containerElementId);
        let paginator = this.#BuildPagination();
        container.appendChild(paginator);
    }
    #BuildPagination() {
        const container = document.createElement("div");
        const containerControls = document.createElement("div");
        const previousPage = document.createElement("input");
        const nextPage = document.createElement("input");
        const currentPage = document.createElement("div");
        const firstPage = document.createElement("input");
        const lastPage = document.createElement("input");

        this.#controlShowCurrentPage = currentPage;

        container.classList.add("pagination");
        currentPage.classList.add("currentPage");

        previousPage.type = "button";
        nextPage.type = "button";
        firstPage.type = "button";
        lastPage.type = "button";

        firstPage.value = "<<";
        firstPage.setAttribute("data-paginationname", "first");
        previousPage.value = "<";
        previousPage.setAttribute("data-paginationname", "previous");
        nextPage.value = ">";
        nextPage.setAttribute("data-paginationname", "next");
        lastPage.value = ">>";
        lastPage.setAttribute("data-paginationname", "last");
        currentPage.innerHTML = `${this.#currentPage} of ${this.#TotalPages()}`;

        firstPage.onclick = this.#triggerEventClickChangePage.bind(this);
        previousPage.onclick = this.#triggerEventClickChangePage.bind(this);
        nextPage.onclick = this.#triggerEventClickChangePage.bind(this);
        lastPage.onclick = this.#triggerEventClickChangePage.bind(this);

        containerControls.appendChild(firstPage);
        containerControls.appendChild(previousPage);
        containerControls.appendChild(currentPage);
        containerControls.appendChild(nextPage);
        containerControls.appendChild(lastPage);

        containerControls.classList.add("paginationControls");
        container.appendChild(containerControls);

        return container;
    }
    #triggerEventClickChangePage(event) {
        let controlName = event.target.getAttribute("data-paginationname");
        let newPage = this.#currentPage;

        if (controlName == "first") {
            if (this.#currentPage > 1) {
                newPage = 1;
            }
        }
        else if (controlName == "previous") {
            if (this.#currentPage > 1) {
                newPage = this.#currentPage - 1;
            }
        }
        else if (controlName == "next") {
            if (this.#currentPage < this.#TotalPages()) {
                newPage = this.#currentPage + 1;
            }
        }
        else if (controlName == "last") {
            if (this.#currentPage < this.#TotalPages()) {
                newPage = this.#TotalPages();
            }
        }

        if (this.#currentPage != newPage) {
            this.#currentPage = newPage;
            this.#controlShowCurrentPage.innerHTML = `${this.#currentPage} of ${this.#TotalPages()}`;
            this.#callbackChangePage({
                currentPage: this.#currentPage
            })
        }
    }
    #TotalPages() {
        return Math.ceil(this.#totalRows / this.#pageSize);
    }
    get CurrentPage() {
        return this.#currentPage;
    }
}