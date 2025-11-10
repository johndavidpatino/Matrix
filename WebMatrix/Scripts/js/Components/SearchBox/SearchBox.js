export { SearchBox }

class SearchBox {
    #ContainerElementId;
    #CallbackClickButtonSearch;
    #ElementTextSearch;
    constructor({ ContainerElementId, CallbackClickButtonSearch }) {
        this.#ContainerElementId = ContainerElementId;
        this.#CallbackClickButtonSearch = CallbackClickButtonSearch;
        this.#Initialize();
    }
    #Initialize() {
        let containerExternal = document.getElementById(this.#ContainerElementId);
        let container = document.createElement("div");
        let textSearch = document.createElement("input");
        let btn = document.createElement("button");
        let img = document.createElement("img");

        textSearch.type = "text";
        btn.type = "button";
        img.src = "/images/Search_16x16.png";

        container.classList.add("SearchBox");

        btn.appendChild(img);
        container.appendChild(textSearch);
        container.appendChild(btn);
        containerExternal.appendChild(container);

        btn.onclick = this.#triggerEventClickButtonSearch.bind(this);
        this.#ElementTextSearch = textSearch;

    }
    #triggerEventClickButtonSearch(event) {
        this.#CallbackClickButtonSearch({
            textToSearch: this.#ElementTextSearch.value.trim()
        })
    }
    get CurrentText() {
        return this.#ElementTextSearch.value.trim();
    }
}
