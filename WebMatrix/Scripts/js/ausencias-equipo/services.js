
export function GetFormatDate(date) {
    var fechaUTC = new Date(date)// Año, mes (0-11), día

    var anio = fechaUTC.getUTCFullYear();
    var mes = ('0' + (fechaUTC.getUTCMonth() + 1)).slice(-2); // Suma 1 al mes porque los meses van de 0 a 11
    var dia = ('0' + fechaUTC.getUTCDate()).slice(-2);

    return anio + '-' + mes + '-' + dia;
}




export const GetAusenciasEquipo = async function( {
    jefeId = 80243742,
    fInicio = '2023-05-01',
    fFin = '2024-03-01'
} = {}) 
{


    let newFInicio = GetFormatDate(fInicio)
    let newFFin = GetFormatDate(fFin)

    let params = {
        jefeId: jefeId,
        fInicio: newFInicio,
        fFin: newFFin
    }

    const response = await fetch(`AusenciasEquipo.aspx/getAusenciasEquipo`,{
        	method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(params)
        })

    if(response.ok){
        const jsonResponse = await response.json()
        const ausencias = jsonResponse.d.map(item => {
            item.FInicio = item.FInicio.replace(' 12:00:00 a. m.','')
            item.FFin = item.FFin.replace(' 12:00:00 a. m.','')
            return item
        })

        return ausencias
    }

    return []
}

export const GetBeneficiosPendientes = async function (idEmpleado) {
    const response = await fetch(`AusenciasEquipo.aspx/getBeneficiosPendientes`,{
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                idempleado: idEmpleado
            })
        })

    if(response.ok){
        const jsonResponse = await response.json()
        const beneficios = jsonResponse.d
        return beneficios
    }

    return []
}

export const GetCurrentUserId = async function(){
    
    const result = await fetch('AusenciasEquipo.aspx/getCurrentUserId',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
    })

    if(result.ok){
        const jsonResponse = await result.json()
        return jsonResponse.d
    }
    return null 
}

export const GetAusenciasSubordinados = async function({jefeId}){
    const result = await fetch('AusenciasEquipo.aspx/getAusenciasSubordinados',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            jefeId
        })
    })

    if(result.ok){
        const jsonResponse = await result.json()
        return jsonResponse.d
    }
    return [] 
}

export const GetAusenciassPersonas = async function({jefeId = null,search = ''} = {} ){
    if(!jefeId) return []
    const result = await fetch('AusenciasEquipo.aspx/getAusenciasPersonas',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            jefeId,
            search,
        })
    })

    if(result.ok){
        const jsonResponse = await result.json()
        return jsonResponse.d
    }
    return [] 
}

export const AddSubordinado = async function({jefeId = null, empleadoId = null} = {}) {
    if(!empleadoId || !jefeId) return
    const result = await fetch('AusenciasEquipo.aspx/addAusenciasSubordinado',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            jefeId,
            empleadoId,
        })
    })
    if(result.ok){
        const jsonResponse = await result.json()
        return jsonResponse.d
    }
    return null
}

export const RemoveSubordinado = async function({subordinadoId = null} = {}) {
    if(!subordinadoId) return
    const result = await fetch('AusenciasEquipo.aspx/removeAusenciasSubordinado',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            subordinadoId,
        })
    })
    if(result.ok){
        return true
    }
    return false
}