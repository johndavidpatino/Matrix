import { getParticipantsInTraining } from "./capacitacionesServices.js"

let capacitacionId = null
let participantsContaienr = document.getElementById("registerParticipantsModule")

participantsContaienr.setVisible(false)

const listenInterval = setInterval(()=>{
    const hiddenCapacitacionId = document.getElementById("CPH_Section_CPH_Section_CPH_ContentForm_hfidCapacitacion")
    if(hiddenCapacitacionId.value !== "" && capacitacionId != hiddenCapacitacionId.value){
        capacitacionId = hiddenCapacitacionId.value
        participantsContaienr.setVisible(true)
        document.dispatchEvent(
            new CustomEvent("changeCapacitacionId", 
            {
                detail: {
                    capacitacionId: capacitacionId
                }
            }
        ))
    }
},1000)

const capacitacionParticipantesTable = document.getElementById("mxCapacitacionTable")


capacitacionParticipantesTable.columns = [
    { name: 'Identificación', property: 'id' },
	{ name: 'Participante', property: 'name' },
	{ name: 'Eficacia Obtenida', property: 'eficacia', type:"input:number" },
	{ name: 'Aprobó', property: 'aprobo',type:"input:checkbox" },
]

capacitacionParticipantesTable.options.actions.edit = true
capacitacionParticipantesTable.options.actions.remove = true
capacitacionParticipantesTable.propNameInModal = 'name'

document.addEventListener("changeCapacitacionId",async function(event){
    await LoadParticipants(event.detail.capacitacionId)
})

export async function LoadParticipants(id) {
    let searchParameters = {
        CapacitacionId: id,
    }

    const participants = await getParticipantsInTraining(searchParameters)


    const newEntries = participants.map(item => {
        return {
            participantId: item.CapacitacionParticipanteId,
            capacitacionId: item.CapacitacionId,
            id: item.Identificacion,
            name: item.Nombres + " " + item.Apellidos,
            eficacia: item.Eficacia,
            aprobo: item.Aprobo,
        }
    })

    capacitacionParticipantesTable.setEntries(newEntries)
    
}
