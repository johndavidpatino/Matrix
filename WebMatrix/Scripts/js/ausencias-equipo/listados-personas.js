import { AddSubordinado, GetAusenciasSubordinados, GetAusenciassPersonas, GetCurrentUserId, RemoveSubordinado } from "./services.js"

const modalAusencias = document.getElementById("modalAusencias")
const bntAgregarPersona = document.getElementById("bntAgregarPersona")
const bntOcultarPersonas = document.getElementById("bntOcultarPersonas")

const listaPersonasIn = document.getElementById("listaPersonasIn")
const listaPersonasOut = document.getElementById("listaPersonasOut")

const inputBuscarPersonas = document.getElementById("inputBuscarPersonas")
const btnBuscarPersonas = document.getElementById("btnBuscarPersonas")

const loadingTemplate = '<div class="ausencias-loading-inner"><div class="ausencias-loading"><div></div>'

let isFirstLoad = true

//TODO: Sacar en otro archivo
const GetPersonaCard = function({
    name = '',
    avatar = undefined,
    id = '',
    type = 'in' // in or out
} = {}) {

    const userAvatar = avatar ?? '../Images/sin-foto.jpg'

    return `
    <article class="lista-persona">
        <img class="lista-persona-avatar" src="${userAvatar}" />
        <span class="lista-persona-name">${name}</span>
        <button type="button" class="btn-ausencias btn-ausencias-icon ${type === 'in'?'btn-ausencias-red':''}" data-rowid="${id}" data-action="${type === 'in'?'remove':'add'}">
            ${type === 'in'
            ?'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 -960 960 960"><path d="M280-120q-33 0-56.5-23.5T200-200v-520h-40v-80h200v-40h240v40h200v80h-40v520q0 33-23.5 56.5T680-120H280Zm400-600H280v520h400v-520ZM360-280h80v-360h-80v360Zm160 0h80v-360h-80v360ZM280-720v520-520Z" fill="currentColor"/></svg>'
            :'<svg xmlns="http://www.w3.org/2000/svg" viewBox="0 -960 960 960"><path d="M440-440H200v-80h240v-240h80v240h240v80H520v240h-80v-240Z" fill="currentColor"/></svg>'}
        </button>
    </article>
    `
}

const jefeId = await GetCurrentUserId()

bntAgregarPersona.addEventListener('click', ()=>{
    if(modalAusencias.classList.contains('hidden')){
        modalAusencias.classList.remove('hidden')
    }
})

bntOcultarPersonas.addEventListener('click', ()=>{
    if(!modalAusencias.classList.contains('hidden')){
        modalAusencias.classList.add('hidden')
    }
})


btnBuscarPersonas.addEventListener('click', async ()=>{
    await UpdatePersonasOut({search:inputBuscarPersonas.value})
})




async function HandlerAction(event) {

    const target = event.currentTarget
    const action = target.dataset.action
    if (!action) return

    const rowid = target.dataset.rowid

    if (action === 'add') {
        const newPerson = await AddSubordinado({ jefeId: jefeId, empleadoId: Number(rowid) })
        if (newPerson) {
            await Promise.all([
                UpdatePersonasIn(),
                UpdatePersonasOut({search:inputBuscarPersonas.value})]
            )
        }
    }
    if (action === 'remove') {
        const isRemoved = await RemoveSubordinado({ subordinadoId: Number(rowid) })
        if (isRemoved) {
            await Promise.all([
                UpdatePersonasIn(),
                UpdatePersonasOut({search:inputBuscarPersonas.value})]
            )
        }
    }
    
}



async function UpdatePersonasIn(){
    listaPersonasIn.innerHTML = loadingTemplate
    const subordinados = await GetAusenciasSubordinados({jefeId:jefeId})
    let templateIn = ''
    for (const subordinado of subordinados) {
        templateIn += GetPersonaCard({name: subordinado.Nombre,id: subordinado.Id, avatar:subordinado.Avatar, type:'in'})
    }
    listaPersonasIn.innerHTML = templateIn
    listaPersonasIn.querySelectorAll('.btn-ausencias').forEach(btn => {
        btn.addEventListener('click', HandlerAction)
    })
}



async function UpdatePersonasOut({search= ''} = {}){
    listaPersonasOut.innerHTML = loadingTemplate
    const personas = await GetAusenciassPersonas({jefeId:jefeId,search:search})
    let templateOut= ''
    for (const persona of personas) {
        templateOut += GetPersonaCard({name: persona.Nombre,id: persona.Id, avatar:persona.Avatar,type:'out'})
    }
    if(!isFirstLoad){
        document.dispatchEvent(new CustomEvent('changeTeam'))
    }
    listaPersonasOut.innerHTML = templateOut

    listaPersonasOut.querySelectorAll('.btn-ausencias').forEach(btn => {
        btn.addEventListener('click', HandlerAction)
    })
}

    
await Promise.all([
    UpdatePersonasIn(),
    UpdatePersonasOut()]
)











