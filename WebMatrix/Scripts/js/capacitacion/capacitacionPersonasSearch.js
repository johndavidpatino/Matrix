import { getPersonsCanParticipate } from "./capacitacionesServices.js"

const inputPersonName = document.getElementById("inputPersonsSearchNombre")
const inputPersonId = document.getElementById("inputPersonsSearchId")
const inputPersonContratista = document.getElementById("inputPersonsSearchContratista")
const btnPersonsSearch = document.getElementById("btnPersonsSeatch")
const capacitacionPersonsTable = document.getElementById('mxPersonsTable')



capacitacionPersonsTable.columns = [
	{ name: 'Identificación', property: 'id' },
	{ name: 'Nombre', property: 'name' },
	{ name: 'Id Contratista', property: 'contractorId' },
	{ name: 'Contratista', property: 'contractorName' },
]


capacitacionPersonsTable.options.actions.add = true


let searchParameters = {
    Identificacion: null,
	Nombre: "",
	ContratistaId: null,
	NombreContratista: "",
	CapacitacionId: null,
	SonParticipantes: 0,
	Page: 1,
	PageSize: 100,
}

document.addEventListener("changeCapacitacionId", event =>{
    searchParameters.CapacitacionId = event.detail.capacitacionId
})

btnPersonsSearch.addEventListener("click", async function(event){
    event.preventDefault()    

    if(!btnPersonsSearch.classList.contains("ip-loading")){
        btnPersonsSearch.classList.add("ip-loading")
    }

    searchParameters.Nombre = inputPersonName.value
    searchParameters.NombreContratista = inputPersonContratista.value
    searchParameters.Identificacion = inputPersonId.value === "" ? null:parseInt(inputPersonId.value)
    
    const persons = await getPersonsCanParticipate(searchParameters)



    const newEntries = persons.map(item => {
        return {			
            id: item.Identificacion,
            name: item.Nombres +  " " + item.Apellidos,
            contractorId: item.IdContratista,
            contractorName: item.NombreContratista,		
        }
    })

    capacitacionPersonsTable.setEntries(newEntries)

    btnPersonsSearch.classList.remove("ip-loading")
    
})