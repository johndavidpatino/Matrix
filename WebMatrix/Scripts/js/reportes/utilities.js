export const RenderSelectOptions = function (
    { element, items, defaultSelected, keyLabel, keyValue } = { items:[], keyLabel:'label', keyValue:'value' }) {
    for (let i = 0; i < items.length; i++) {
        let item = items[i]
        let itemDOM = document.createElement('option')

        itemDOM.setAttribute('value', item[keyValue])
        itemDOM.innerHTML = item[keyLabel]

        if (i == defaultSelected) {
            itemDOM.selected = true
        }

        element.appendChild(itemDOM)
    }
}