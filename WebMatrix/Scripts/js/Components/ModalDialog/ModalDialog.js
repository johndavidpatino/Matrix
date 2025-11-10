export { ModalDialog }

class ModalDialog {
    #element
    #title
    #bodyElementId
    #width
    #height
    #bodyElement
    #actionButtons
    #callbackActionButtonClick
    #data
    #bodyText
    constructor({ title, bodyElementId, widthPixels, heightPixels, actionButtons, data, bodyText, bodyElement }) {
        this.#title = title
        this.#bodyElementId = bodyElementId
        this.#width = widthPixels
        this.#height = heightPixels
        this.#actionButtons = actionButtons
        this.#data = data
        this.#bodyText = bodyText
        this.#bodyElement = bodyElement

    }
    show() {

        if (this.#element != null) {
            this.#element.showModal()
            return;
        }

        let elem = document.createElement('dialog')
        let container = document.createElement('div')
        let containerHeader = document.createElement('div')
        let title = document.createElement('div')
        let body = document.createElement('div')
        let buttons = document.createElement('div')
        let buttonClose = document.createElement('button')
        let bodyElement

        if (this.#bodyElementId) {
            bodyElement = document.getElementById(this.#bodyElementId)
            bodyElement.classList.remove('hide')
        }
        else if (this.#bodyText) {
            bodyElement = document.createElement('div')
            //bodyElement.classList.add('text')
            bodyElement.innerHTML = this.#bodyText
        }
        else {
            bodyElement = this.#bodyElement
        }

        console.log(bodyElement)
        this.#bodyElement = bodyElement
        body.appendChild(bodyElement)

        elem.style.width = this.#width + 'px'
        elem.style.height = this.#height + 'px'

        containerHeader.appendChild(title)
        containerHeader.appendChild(buttonClose)

        container.appendChild(containerHeader)
        container.appendChild(body)

        elem.classList.add("ModalMessage")

        container.classList.add("Container")
        containerHeader.classList.add("containerHeader")
        body.classList.add("body")
        title.classList.add("title")
        buttons.classList.add("buttons")

        title.innerHTML = this.#title
        buttonClose.innerHTML = "X"
        buttonClose.addEventListener('click', () => {
            elem.close()
        })

        if (this.#actionButtons.length == 0) {
            container.style.gridTemplateRows = "40px 1fr"
        }

        for (var i = 0; i < this.#actionButtons.length; i++) {
            const actionButton = this.#actionButtons[i]
            const btnActionButton = document.createElement("button")
            btnActionButton.innerHTML = actionButton.text
            btnActionButton.setAttribute("data-actionbuttonname", actionButton.name)
            btnActionButton.onclick = this.#triggerEventActionButtonClick.bind(this)
            buttons.appendChild(btnActionButton)
        }

        this.#element = elem


        elem.appendChild(container)
        document.body.appendChild(elem)


        elem.showModal()

    }
    addEventListenerActionButtonClick(callback) {
        this.#callbackActionButtonClick = callback
    }
    #triggerEventActionButtonClick(event) {
        const actionButtonName = event.target.getAttribute("data-actionbuttonname")
        this.#callbackActionButtonClick({
            actiionButtonName: actionButtonName,
            data: this.#data
        })
    }
    close() {
        this.#bodyElement.classList.add('hide')
        this.#element.close()
    }
}