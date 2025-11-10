export { Checkbox }

class Checkbox {
	#element
	#elementSelect
	#containerElement
	#callbackChange
	#isSelected = false
	#data
	#allowUnselect
	constructor({ containerElement, data, allowUnselect, checked }) {
		this.#containerElement = containerElement
		this.#data = data
		this.#allowUnselect = allowUnselect
		this.#isSelected = checked
		this.#draw()
	}
	#draw() {
		const container = document.createElement("div")
		const element = document.createElement("label")
		const elementCheckmark = document.createElement("div")
		const elementSelect = document.createElement("div")

		container.onclick = this.#triggerEventChange.bind(this)

		element.classList.add("CBContainer")
		elementCheckmark.classList.add("checkmark")

		if (!this.#isSelected)
			elementSelect.classList.add("unselect")
		else
			elementSelect.classList.add("select")

		elementCheckmark.appendChild(elementSelect)
		element.appendChild(elementCheckmark)
		container.appendChild(element)
		this.#containerElement.appendChild(container)
		this.#element = container
		this.#elementSelect = elementSelect
	}
	get element() {
		return this.#element
	}
	addEventListenerChange(callback) {
		this.#callbackChange = callback
	}
	#triggerEventChange() {
		if (!this.#allowUnselect && this.#isSelected)
			return

		if (this.#isSelected)
			this.#UpdateState(false)
		else
			this.#UpdateState(true)

		this.#callbackChange({
			data: this.#data,
			isSelected: this.#isSelected
		})
	}
	Check() {
		this.#isSelected = true
		this.#UpdateState(true)
	}
	Uncheck() {
		this.#isSelected = false
		this.#UpdateState(false)
	}
	#UpdateState(check) {
		if (check) {
			this.#elementSelect.classList.add("select")
			this.#elementSelect.classList.remove("unselect")
			this.#isSelected = true
		}
		else {
			this.#elementSelect.classList.remove("select")
			this.#elementSelect.classList.add("unselect")
			this.#isSelected = false
		}
	}
	get data() {
		return this.#data
	}
}