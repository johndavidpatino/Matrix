#element;

function Loadershow() {
	let elem = document.createElement('div');
	let loader = document.createElement('div');
	let buttonClose = document.createElement('button');

	elem.classList.add("Loader");
	loader.classList.add("Spinner");
	elem.appendChild(loader);
	document.body.appendChild(elem);
	this.#element = elem;
}

function Loaderclose() {
	this.#element.remove();
}

export { Loadershow, Loaderclose };