export { Table }
import { Checkbox } from '../Checkbox/Checkbox.js'

class Table {
    #configuration
    #callbackActionButtonClick
    #callbackChangeSelectRow
    #callbackChangePage
    #rowsChecks = []
    #currentIndexRowSelected
    #currentRealIndexRowSelected
    #currentPage = 1
    #controlShowCurrentPage
    constructor({ allowSelectRows, containerElement, data, columsConfiguration, pageSize, actionButtons, totalRows, pageIndex, selectRowIndex, pagination, showColumnsNames }) {
        this.#configuration = {
            allowSelectRows,
            containerElement,
            data,
            columsConfiguration,
            pageSize,
            actionButtons,
            totalRows,
            pageIndex,
            selectRowIndex,
            pagination: pagination ?? true,
            showColumnsNames: showColumnsNames ?? true
        }
        this.#currentPage = pageIndex
        this.#currentRealIndexRowSelected = selectRowIndex
        this.#currentIndexRowSelected = selectRowIndex ? this.#RowIndexFromRowIndexReal(selectRowIndex) : undefined
        this.#draw()
    }
    #draw() {
        let elementtoRemove = this.#configuration.containerElement.querySelector(".containerTable")
        if (elementtoRemove)
            this.#configuration.containerElement.removeChild(elementtoRemove)

        const container = document.createElement("div")
        const containerBody = document.createElement("div")
        const tbl = document.createElement("table");
        const tblHead = document.createElement("thead");
        const tblHeadRow = document.createElement("tr")
        const tblBody = document.createElement("tbody");

        container.classList.add("containerTable")
        containerBody.classList.add("containerTableBody")
        tbl.classList.add("table")

        let columnsToShow = this.#configuration.columsConfiguration.filter(column => column.show)

        if (this.#configuration.allowSelectRows) {
            const cell = document.createElement("td")
            tblHeadRow.appendChild(cell)
        }

        if (this.#configuration.showColumnsNames) {
            for (var columnIndex = 0; columnIndex < columnsToShow.length; columnIndex++) {
                let column = columnsToShow[columnIndex]
                if (column.show) {
                    const cell = document.createElement("td")
                    const cellText = document.createTextNode(column.nameToShow ?? column.sourcename)
                    cell.appendChild(cellText)
                    tblHeadRow.appendChild(cell)
                }
            }
        }

        for (var indexActionButton = 0; indexActionButton < this.#configuration.actionButtons.length; indexActionButton++) {
            const actionButton = this.#configuration.actionButtons[indexActionButton]
            const cell = document.createElement("td")
            tblHeadRow.appendChild(cell)
        }

        tblHead.appendChild(tblHeadRow)

        if (this.#configuration.data) {
            for (var rowIndex = 0; rowIndex < this.#configuration.data.length; rowIndex++) {
                const row = document.createElement("tr")

                if (this.#configuration.allowSelectRows) {
                    const cell = document.createElement("td")
                    const checked = this.#currentRealIndexRowSelected == this.#RowIndexRealFromRowIndex(rowIndex)
                    const checkbox = new Checkbox({ containerElement: cell, data: { rowIndex, page: this.#currentPage }, allowUnselect: false, checked })
                    checkbox.addEventListenerChange(this.#triggerEventChangeSelectRow.bind(this))
                    row.appendChild(cell)
                    this.#rowsChecks.push(checkbox)
                }

                for (var columnIndex = 0; columnIndex < columnsToShow.length; columnIndex++) {
                    let column = columnsToShow[columnIndex]
                    const cell = document.createElement("td")
                    let value = this.#configuration.data[rowIndex][column.sourcename]
                    if (value && value.length > 5 && value.substring(0, 5) == "/Date")
                        cell.innerText = this.#FormatMillisecondsToyyyyMMdd(value)
                    else
                        cell.innerText = this.#configuration.data[rowIndex][column.sourcename]
                    row.appendChild(cell)
                }

                for (var indexActionButton = 0; indexActionButton < this.#configuration.actionButtons.length; indexActionButton++) {
                    const actionButton = this.#configuration.actionButtons[indexActionButton]
                    const cell = document.createElement("td")
                    const btnActionButton = document.createElement("input")
                    btnActionButton.type = "button"
                    btnActionButton.value = actionButton.text
                    btnActionButton.setAttribute("data-rowindex", rowIndex)
                    btnActionButton.setAttribute("data-actionbuttonname", actionButton.name)
                    btnActionButton.onclick = this.#triggerEventActionButtonClick.bind(this)
                    cell.appendChild(btnActionButton)
                    row.appendChild(cell)
                }

                tblBody.appendChild(row);
            }
        }

        tbl.appendChild(tblHead)
        tbl.appendChild(tblBody)
        containerBody.appendChild(tbl)
        container.appendChild(containerBody)

        if (this.#configuration.pagination && this.#configuration.data && this.#configuration.data.length > 0) {
            container.appendChild(this.#DrawPagination())
        }
        else {
            container.style.gridTemplateRows = "1fr"
            tbl.style.marginBottom = "0"
        }


        this.#configuration.containerElement.appendChild(container)
    }
    addEventListenerActionButtonClick(callback) {
        this.#callbackActionButtonClick = callback
    }
    addEventListenerChangeSelectRow(callback) {
        this.#callbackChangeSelectRow = callback
    }
    addEventListenerChangePage(callback) {
        this.#callbackChangePage = callback
    }
    #triggerEventActionButtonClick(event) {
        const rowIndex = event.target.getAttribute("data-rowindex")
        const actionButtonName = event.target.getAttribute("data-actionbuttonname")
        this.#callbackActionButtonClick({
            actiionButtonName: actionButtonName,
            dataItem: this.#configuration.data[rowIndex]
        })
    }
    #triggerEventChangeSelectRow(event) {
        if (this.#currentIndexRowSelected != undefined) {
            if (this.#CurrentSelectedRowIsThisPage()) {
                this.#rowsChecks[this.#currentIndexRowSelected].Uncheck()
            }
        }
        this.#currentIndexRowSelected = event.data.rowIndex
        this.#currentRealIndexRowSelected = this.#RowIndexRealFromRowIndex(event.data.rowIndex)

        this.#callbackChangeSelectRow({
            isSelected: event.isSelected,
            dataItem: this.#configuration.data[event.data.rowIndex]
        })
    }
    #DrawPagination() {
        const container = document.createElement("div")
        const containerControls = document.createElement("div")
        const previousPage = document.createElement("input")
        const nextPage = document.createElement("input")
        const currentPage = document.createElement("div")
        const firstPage = document.createElement("input")
        const lastPage = document.createElement("input")

        this.#controlShowCurrentPage = currentPage

        container.classList.add("pagination")
        currentPage.classList.add("currentPage")

        previousPage.type = "button"
        nextPage.type = "button"
        firstPage.type = "button"
        lastPage.type = "button"

        firstPage.value = "<<"
        firstPage.setAttribute("data-paginationname", "first")
        previousPage.value = "<"
        previousPage.setAttribute("data-paginationname", "previous")
        nextPage.value = ">"
        nextPage.setAttribute("data-paginationname", "next")
        lastPage.value = ">>"
        lastPage.setAttribute("data-paginationname", "last")
        currentPage.innerHTML = `${this.#currentPage} of ${this.#TotalPages()}`

        firstPage.onclick = this.#triggerEventClickChangePage.bind(this)
        previousPage.onclick = this.#triggerEventClickChangePage.bind(this)
        nextPage.onclick = this.#triggerEventClickChangePage.bind(this)
        lastPage.onclick = this.#triggerEventClickChangePage.bind(this)

        containerControls.appendChild(firstPage)
        containerControls.appendChild(previousPage)
        containerControls.appendChild(currentPage)
        containerControls.appendChild(nextPage)
        containerControls.appendChild(lastPage)

        containerControls.classList.add("paginationControls")
        container.appendChild(containerControls)

        return container
    }
    #TotalPages() {
        return Math.ceil(this.#configuration.totalRows / this.#configuration.pageSize)
    }
    #triggerEventClickChangePage(event) {
        let controlName = event.target.getAttribute("data-paginationname")
        let newPage = this.#currentPage

        if (controlName == "first") {
            if (this.#currentPage > 1) {
                newPage = 1
            }
        }
        else if (controlName == "previous") {
            if (this.#currentPage > 1) {
                newPage = this.#currentPage - 1
            }
        }
        else if (controlName == "next") {
            if (this.#currentPage < this.#TotalPages()) {
                newPage = this.#currentPage + 1
            }
        }
        else if (controlName == "last") {
            if (this.#currentPage < this.#TotalPages()) {
                newPage = this.#TotalPages()
            }
        }

        if (this.#currentPage != newPage) {
            this.#currentPage = newPage
            this.#controlShowCurrentPage.innerHTML = `${this.#currentPage} of ${this.#TotalPages()}`
            this.#callbackChangePage({
                currentPage: this.#currentPage,
                currentSelectRow: this.#currentRealIndexRowSelected
            })
        }
    }
    #RowIndexRealFromRowIndex(rowIndex) {
        return (this.#configuration.pageSize * this.#configuration.pageIndex) - this.#configuration.pageSize + rowIndex
    }
    #RowIndexFromRowIndexReal(rowIndexReal) {
        return rowIndexReal % this.#configuration.pageSize
    }
    #CurrentSelectedRowIsThisPage() {
        let startRowPage = this.#currentPage * this.#configuration.pageSize - this.#configuration.pageSize
        let endRowPage = this.#currentPage * this.#configuration.pageSize - 1

        return this.#currentRealIndexRowSelected >= startRowPage && this.#currentRealIndexRowSelected <= endRowPage

    }
    #DateFromMilliseconds(milliseconds) {
        return new Date(parseInt(milliseconds.substr(6)));
    }
    #FormatMillisecondsToyyyyMMdd(milliseconds) {
        let date = this.#DateFromMilliseconds(milliseconds);
        return this.#FormatDateToyyyyMMdd(date);
    }
    #FormatDateToyyyyMMdd(dateToFormat) {
        return dateToFormat.format('yyyy-MM-dd HH:mm:ss');
    }
}