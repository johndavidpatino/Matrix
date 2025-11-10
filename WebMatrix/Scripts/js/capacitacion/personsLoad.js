import { addPersonToTraining, removeParticipantFromTraining, updatePersonToTraining } from "./capacitacionesServices.js"
import { LoadParticipants } from "./capacitacionParticipantesGet.js"

const personsTable = document.getElementById('mxPersonsTable')
const capacitacionTable = document.getElementById('mxCapacitacionTable')

let capacitacionId = null

document.addEventListener("changeCapacitacionId", event =>{
    capacitacionId = event.detail.capacitacionId
})

personsTable.addEventListener('addEntry', async event =>{
	event.preventDefault()
    let {detail} = event
	let addRequest = {
		CapacitacionId: capacitacionId,
		Participante: detail.id,
		Eficacia: null,
		OportunidadMejora: "",
		Aprobo: false
	}

	
	const isAdded = await addPersonToTraining(addRequest)

	if(isAdded){
		personsTable.removeEntry(detail.rowId)
		LoadParticipants(capacitacionId)
	}
})

capacitacionTable.addEventListener('removeEntry', async event =>{
	event.preventDefault()
    let {detail} = event
	let removeRequest = {
		ParticipantId: detail.participantId,
	}

	const isRemoved = await removeParticipantFromTraining(removeRequest)
	if(isRemoved){
		personsTable.removeEntry(detail.rowId)
		LoadParticipants(capacitacionId)
	}
})

capacitacionTable.addEventListener('updateEntry', async event => {
	event.preventDefault()
	let {detail} = event
	let updateRequest = {
		CapacitacionParticipanteId: detail.participantId,
		CapacitacionId: detail.capacitacionId,
		Participante: detail.id,
		Eficacia: detail.eficacia,
		OportunidadMejora: "",
		Aprobo: detail.aprobo
	}

	const isUpdated = await updatePersonToTraining(updateRequest)
	detail.CallbackUpdated(isUpdated)
	
})