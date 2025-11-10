export const getPersonsCanParticipate = async function( params = {
    Identificacion: null,
	Nombre: "",
	ContratistaId: null,
	NombreContratista: "",
	CapacitacionId: 13677,
	SonParticipantes: 0,
	Page: 1,
	PageSize: 100,
}) 
{



    const response = await fetch(`Capacitacion.aspx/getCapacitacionPersonas`,{
        	method: 'POST',
            body: JSON.stringify(params),
        	headers: {
        		'Content-Type': 'application/json'
        	}
        })

    if(response.ok){
        const jsonResponse = await response.json()
        const persons = jsonResponse.d

        return persons
    }

    return []
}

export const getParticipantsInTraining = async function(params = {
	CapacitacionId: null,
}) {
    const response = await fetch(`Capacitacion.aspx/getCapacitacionParticipantes`, {
        method: 'POST',
        body: JSON.stringify(params),
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if (response.ok) {
        const jsonResponse = await response.json()
        const participants = jsonResponse.d
        return participants
    }
    
    return []
}

export const removeParticipantFromTraining = async function(params = {
    ParticipantId: null
}) {
	const response = await fetch(`Capacitacion.aspx/removeCapacitacionParticipant`,{
		method: 'POST',
		body: JSON.stringify(params),
		headers: {
			'Content-Type': 'application/json'
		}
	})

	if(response.ok){
		return true
	}

    return false
}

export const addPersonToTraining = async function(params = {
    CapacitacionId: null,
    Participante: null,
    Eficacia: null,
    OportunidadMejora: "",
    Aprobo: false
})
{
    const response = await fetch(`Capacitacion.aspx/addCapacitacionPerson`,{
		method: 'POST',
		body: JSON.stringify(params),
		headers: {
			'Content-Type': 'application/json'
		}
	})

	if(response.ok){
		return true
	}
    return false
}

export const updatePersonToTraining = async function(params = {
    CapacitacionParticipanteId: null,
    CapacitacionId: null,
    Participante: null,
    Eficacia: null,
    OportunidadMejora: "",
    Aprobo: false
})
{
    const response = await fetch(`Capacitacion.aspx/updateCapacitacionParticipant`,{
		method: 'POST',
		body: JSON.stringify(params),
		headers: {
			'Content-Type': 'application/json'
		}
	})

	if(response.ok){
		return true
	}
    return false
}