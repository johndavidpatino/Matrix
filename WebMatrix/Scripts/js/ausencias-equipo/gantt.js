
import { colorSchemes } from './colors.js';
import { GetGanttOptions } from './high-options.js';
import { orderEstado } from './orders.js';
import { GetAusenciasEquipo, GetAusenciasSubordinados, GetBeneficiosPendientes, GetCurrentUserId, GetFormatDate } from './services.js';
const startDateInput = document.getElementById('startDateInput')
const endDateInput = document.getElementById('endDateInput')
const bntFilterDate = document.getElementById('bntFilterDate')
const ganttContainer = document.getElementById('ganttContainer')
const btnAnular = document.getElementById('btnAnular')
const btnAprobar = document.getElementById('btnAprobar')
const ausenciasEquipoCards = document.getElementById('ausenciasEquipoCards')
const {active : activeColors, inactive : inactiveColors} = colorSchemes


const day = 24 * 36e5
const today = Math.floor(Date.now() / day) * day;

let currentGantt = null

const userId = await GetCurrentUserId()

const loader = '<div class="loader-wrapper" ><span class="loader"></span></div>'
const loadingTemplate = '<div class="loader-wrapper" ><span class="loader" style="--size:40px; --border: 4px"></span></div>'

startDateInput.value = GetFormatDate(today  - (day * 60))
endDateInput.value = GetFormatDate(today  + (day * 60))


bntFilterDate.addEventListener('click',async(event)=>{
    event.preventDefault()
    bntFilterDate.insertAdjacentHTML('afterbegin',loader)
    bntFilterDate.disabled = true
    if(endDateInput.value == '' || startDateInput.value == '') return

    try{
        await UpdateAusenciasEquipo()
    }
    catch(error){
        console.error(error)
    }
    finally {
        bntFilterDate.querySelector('.loader-wrapper').remove()
        bntFilterDate.disabled = false
    }

})

const GetTeamCard = function({
    avatar = '',
    name = '',
    vacaciones = 0,
    plus = 0,
    balance = 0
}) {
    const userAvatar = avatar ?? '../Images/sin-foto.jpg'
    return `
        <article class="team-card">
            <img class="team-card-avatar" src="${userAvatar}">
            <span class="team-card-name">${name}</span>
            <div class="team-card-beneficios-wrapper">
                <span>Pendientes:</span>
                <ul class="team-card-beneficios">
                    <li class="beneficio-vacaciones beneficio-label">${vacaciones}</li>
                    <li class="beneficio-plus beneficio-label">${plus}</li>
                    <li class="beneficio-balance beneficio-label">${balance}</li>
                </ul>
            </div>
        </article>
    `
}

Highcharts.setOptions({
    lang: {
        weekdays:['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        months:['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre',	'Diciembre'],
        shortMonths: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
    }
})


let minDate = Date.parse(startDateInput.value)
let maxDate = Date.parse(endDateInput.value)

async function GetTranformedSeries(){

    minDate = Date.parse(startDateInput.value)
    maxDate = Date.parse(endDateInput.value)



    let ausencias = await GetAusenciasEquipo({jefeId:userId, fFin: maxDate, fInicio: minDate})
    let personasEquipo = await GetAusenciasSubordinados({jefeId:userId})



    
    const getDateUTCFromString = (text,delimiter="/",hora = 0)=>{
    
        const partes =  text.split(delimiter);
        var dia = parseInt(partes[0], 10);
        var mes = parseInt(partes[1], 10) - 1; // Restar 1 porque los meses en JavaScript van de 0 a 11
        var anio = parseInt(partes[2], 10);
    
        return Date.UTC(anio, mes, dia,hora);
    }
    

    
    const empleados = [...new Set(personasEquipo.map(persona => persona.idEmpleado))]
    
    let beneficios = []
    const beneficiosPromises = empleados.map(async(id)=>{
        let beneficiosResult = await GetBeneficiosPendientes(id)
        beneficiosResult['idEmpleado'] = id
        beneficios.push(beneficiosResult)
    })


    try {
        await Promise.all(beneficiosPromises)
    } catch (error) {
        console.error(error)
    }

    let cardsTemplate = ''
    personasEquipo.forEach(persona =>{
        let personaBeneficios = beneficios.find(b => b.idEmpleado === persona.idEmpleado)
        cardsTemplate += GetTeamCard({
            name: persona.Nombre,
            avatar: persona.Avatar,
            vacaciones: personaBeneficios.find(b => b.Beneficio === 'Vacaciones').dias,
            plus: personaBeneficios.find(b => b.Beneficio === 'Dias Plus (V+)').dias,
            balance: personaBeneficios.find(b => b.Beneficio === 'Dia de balance semestre actual').dias,
        })
    })
    ausenciasEquipoCards.innerHTML = cardsTemplate


    const asuenciasMap = ausencias.map(item => {
        item.FFin = getDateUTCFromString(item.FFin,'/',24);
        item.FInicio = getDateUTCFromString(item.FInicio,'/',0);
        let itemBeneficios = beneficios.find(b => b.idEmpleado === item.idEmpleado)
        item['vacaciones'] = itemBeneficios.find(b => b.Beneficio === 'Vacaciones').dias
        item['diasPlus'] = itemBeneficios.find(b => b.Beneficio === 'Dias Plus (V+)').dias
        item['diasBalance'] = itemBeneficios.find(b => b.Beneficio === 'Dia de balance semestre actual').dias
        return item
    })


    
    const groupData = {}
    
    
    for (const empleado of empleados) {
    
        const ausenciasEmpleado = asuenciasMap.filter(ausencia => ausencia.idEmpleado === empleado)
    
    
        if(!groupData.hasOwnProperty(empleado)){
            const newEmpleado = personasEquipo.find(persona => persona.idEmpleado === empleado)
    
            groupData[empleado] = {
                nombre: newEmpleado.Nombre,
                idEmpleado: newEmpleado.idEmpleado,
                ausencias: []
            }
        }
    
        const groupAusencias = {}
        const groupAusenciasArray = []
    
        for (const ausencia of ausenciasEmpleado) {
            const groupKey = ausencia.EstadoId
            if(!groupAusencias.hasOwnProperty(groupKey)){
                groupAusencias[groupKey] = []
            }
            groupAusencias[groupKey].push(ausencia)
        }
    
        for (const group in groupAusencias) {
            groupAusenciasArray.push(groupAusencias[group])
        }
    
        groupData[empleado].ausencias = groupAusenciasArray
    }
    
    let groupDataArray = []
    
    for(let key in groupData){
        groupDataArray.push(groupData[key])
    }
    const series = []
    
    let yIndex = 0
    
    series.push({
        name: 'Ausencias',
        data: []
    })
    let names = []
    
    for (let empleadoIdx = 0; empleadoIdx < empleados.length; empleadoIdx++) {
        const empleado = groupDataArray.find(item => item.idEmpleado === empleados[empleadoIdx]);
        if(empleado === undefined) continue
        
        for (let ausenciaIdx = 0; ausenciaIdx < empleado.ausencias.length; ausenciaIdx++) {
            const ausenciasGroup = empleado.ausencias[ausenciaIdx].sort((a,b)=>{
                return  orderEstado[b.Estado] - orderEstado[a.Estado]
            })
    
            for (let index = 0; index < ausenciasGroup.length; index++) {
                const ausencia = ausenciasGroup[index];
    
                if(names.includes(ausencia.Nombre)){
                    ausencia.Nombre = ''
                    ausencia.vacaciones = ''
                    ausencia.diasBalance = ''
                    ausencia.diasPlus = ''
                }
                else {
                    names.push(ausencia.Nombre)
                }
               
                series[0].data.push({
                    name: ausencia.Tipo,
                    start: ausencia.FInicio,
                    end: ausencia.FFin,
                    color: ausencia.EstadoId === 20?activeColors[ausencia.TipoId]:'rgba(0, 0, 0, 0.28)',
                    owner: empleado.nombre,
                    estado: ausencia.Estado,
                    assignee: ausencia.Nombre,
                    assigneeId: ausencia.idEmpleado,
                    taskId:ausencia.SolicitudId,
                    vacaciones: ausencia.vacaciones,
                    diasBalance: ausencia.diasBalance,
                    diasPlus: ausencia.diasPlus,
                    y:yIndex
                })
            }
            yIndex++
        }
    }
    return series
}

const UpdateAusenciasEquipo = async function(){

    try {
        ganttContainer.insertAdjacentHTML('afterbegin',loadingTemplate)
        const series = await GetTranformedSeries()
        minDate = Date.parse(startDateInput.value)
        maxDate = Date.parse(endDateInput.value)
        currentGantt.series[0].setData(series[0].data,true);
    } catch (error) {
        console.error(error)
    }
    finally {
        ganttContainer.querySelector('.loader-wrapper').remove()
    }

    // Highcharts.addEvent(Highcharts.Axis, 'foundExtremes', e => {
    //     if (e.target.options.custom && e.target.options.custom.weekendPlotBands) {
    //         const axis = e.target,
    //             chart = axis.chart,
    //             day = 24 * 36e5,
    //             isWeekend = t => /[06]/.test(chart.time.dateFormat('%w', t)),
    //             plotBands = [];
    
    //         let inWeekend = false;
    
    //         for (
    //             let x = Math.floor(axis.min / day) * day;
    //             x <= Math.ceil(axis.max / day) * day;
    //             x += day
    //         ) {
    //             const last = plotBands.at(-1);
    //             if (isWeekend(x) && !inWeekend) {
    //                 plotBands.push({
    //                     from: x,
    //                     color: {
    //                         pattern: {
    //                             path: 'M 0 10 L 10 0 M -1 1 L 1 -1 M 9 11 L 11 9',
    //                             width: 10,
    //                             height: 10,
    //                             color: 'rgba(128,128,128,0.15)'
    //                         }
    //                     }
    //                 });
    //                 inWeekend = true;
    //             }
                
    //             if (!isWeekend(x) && inWeekend && last) {
    //                 last.to = x;
    //                 inWeekend = false;
    //             }
    //         }
    //         axis.options.plotBands = plotBands;
    //     }
    // });    
}

try {
    ganttContainer.insertAdjacentHTML('afterbegin',loadingTemplate)
    const series = await GetTranformedSeries()
    currentGantt = Highcharts.ganttChart('ganttChart', GetGanttOptions({minDate:minDate,maxDate:maxDate,series:series}));
 
} catch (error) {
    console.error(error)
}
finally{
    ganttContainer.querySelector('.loader-wrapper').remove()
}



document.addEventListener('changeTeam', async () => {
    await UpdateAusenciasEquipo()
})

   


