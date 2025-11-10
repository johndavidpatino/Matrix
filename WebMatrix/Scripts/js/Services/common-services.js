let usersResponse = null
export const GetUsers = async function(){
    if(usersResponse != null) return Promise.resolve(usersResponse)
    const response = await fetch('/SGC_Calidad/AccionesMejora/SGC_AccionesMejora.aspx/Users',{
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    })
    usersResponse = await response.json()
    return usersResponse
}

