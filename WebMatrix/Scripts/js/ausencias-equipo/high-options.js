const day = 24 * 36e5
const today = Math.floor(Date.now() / day) * day;



export const GetGanttOptions = ({
    minDate = '',
    maxDate = '',
    series = []
} = {}) => {
    let options =     {
        chart: {
            plotBackgroundColor: 'rgba(128,128,128,0.02)',
            plotBorderColor: 'rgba(128,128,128,0.1)',
            plotBorderWidth: 1
        },
    
        scrollbar: {
            enabled: true
        },
        navigator: {
            enabled: true,
            // liveRedraw: true,
            // series: {
            //     type: 'gantt',
            //     pointPlacement: 0.5,
            //     pointPadding: 0.5,
            //     accessibility: {
            //         enabled: false
            //     }
            // },
            xAxis: {
                zoomEnabled:false
            },
            type:'datetime',
            // yAxis: {
            //     reversed: true,
            // }
        },
        // rangeSelector: {
        //     enabled: true,
        //     selected: 0
        // },
        plotOptions: {
            series: {
                borderRadius: '50%',
                connectors: {
                    dashStyle: 'ShortDot',
                    lineWidth: 2,
                    radius: 5,
                    startMarker: {
                        enabled: false
                    }
                },
                groupPadding: 0,
                dataLabels: [{
                    enabled: true,
                    align: 'left',
                    format: '{point.name}',
                    padding: 10,
                    style: {
                        fontWeight: 'normal',
                        textOutline: 'none'
                    }
                }, {
                    enabled: true,
                    align: 'right',
                    format: '{#if point.completed}{(multiply point.completed.amount 100):.0f}%{/if}',
                    padding: 10,
                    style: {
                        fontWeight: 'normal',
                        textOutline: 'none',
                        opacity: 0.6
                    }
                }],
                allowPointSelect:true,
                point: {
                    events: {
                        select: activeButtons,
                        unselect: disableButtons,
                    }
                }
            }
        },
    
        series: series,
    
        tooltip: {
            pointFormat: '<span style="font-weight: bold">{point.name}</span><br>' +
                '<span>{point.estado}</span><br>' +
                '{point.start:%e %b}' +
                '{#unless point.milestone} → {point.end:%e %b}{/unless}' +
                '<br>' +
                '{#if point.completed}' +
                'Completed: {multiply point.completed.amount 100}%<br>' +
                '{/if}' +
                'Empleado: {#if point.owner}{point.owner}{else}unassigned{/if}'
        },
        title: {
            text: ''
        },

        xAxis: {
            currentDateIndicator: {
                color: '#2caffe',
                dashStyle: 'ShortDot',
                width: 4,
                label: {
                    format: ''
                }
            },
            dateTimeLabelFormats: {
                day: '%e<br><span style="opacity: 0.5; font-size: 0.7em">%a</span>',
                year: {main:'%Y'},
                week: { list: ['Semana %W', 'S%W'] },
            },
            borderWidth: 0,
            grid: {
                borderWidth: 0,
                enabled: true,
            },
            gridLineWidth: 0,
            // min: minDate,
            // max: maxDate,
            // custom: {
            //     today,
            //     weekendPlotBands: true
            // },
            type:'datetime',
        },
        yAxis: {
            type: 'category',
            grid: {
                enabled: true,
                // borderColor: 'rgba(0,0,0,0.3)',
                borderWidth: 0,
                columns: [{
                    title: {
                        text: 'Persona',
                        style: {
                            color:'#444444',
                            fontSize: 16,
                            align: 'left'
                        }
                    },
                    labels: {
                        format: '<span class="gantt-row-title gantt-persona">{point.assignee}</span>',
                        useHTML:true,
                        align: 'left'
                    }
                }, {
                    title: {
                        text: 'Beneficios',
                        style: {
                            color:'#444444',
                            fontSize: 16
                        }
                    },
                    labels: {
                        format: `<span class="gantt-row-beneficio beneficio-vacaciones gantt-estado">{point.vacaciones}</span>
                        <span class="gantt-row-beneficio beneficio-plus gantt-estado">{point.diasPlus}</span>
                        <span class="gantt-row-beneficio beneficio-balance gantt-estado">{point.diasBalance}</span>`,
                        useHTML:true
                    }
                },
                {
                    title: {
                        text: 'Estado',
                        style: {
                            color:'#444444',
                            fontSize: 16
                        }
                        
                    },
                    labels: {
                        format: '<span class="gantt-row-title gantt-estado">{point.estado}</span>',
                        useHTML:true
                    }
                }]
            },
            labels: {
                // align:'left',
                style: {
                    fontSize: 12,
                }
            },
            staticScale: 28
        },
        accessibility: {
            keyboardNavigation: {
                seriesNavigation: {
                    mode: 'serialize'
                }
            },
            // point: {
            //     descriptionFormatter: function (point) {
            //         const completedValue = point.completed ?
            //             point.completed.amount || point.completed : null,
            //             completed = completedValue ?
            //                 ' Task ' + Math.round(completedValue * 1000) / 10 + '% completed.' :
            //                 '',
            //             dependency = point.dependency &&
            //                 point.series.chart.get(point.dependency).name,
            //             dependsOn = dependency ? ' Depends on ' + dependency + '.' : '';
    
            //         return Highcharts.format(
            //             point.milestone ?
            //                 '{point.yCategory}. Milestone at {point.x:%Y-%m-%d}. Owner: {point.owner}.{dependsOn}' :
            //                 '{point.yCategory}.{completed} Start {point.x:%Y-%m-%d}, end {point.x2:%Y-%m-%d}. Owner: {point.owner}.{dependsOn}',
            //             { point, completed, dependsOn }
            //         );
            //     }
            // }
        },
        lang: {
            accessibility: {
                axis: {
                    xAxisDescriptionPlural: 'The chart has a two-part X axis showing time in both week numbers and days.'
                }
            }
        }
    };


    return options

}

function activeButtons(event){
    const estado = event.target.estado
    if(estado !== 'Aprobado'){
        btnAprobar.disabled = false
    }
    if(estado === 'Aprobado'){
        btnAnular.disabled = false
    }
    if(estado === 'Solicitado') {
        btnRechazar.disabled = false
    }
}

function disableButtons(event){
    btnAprobar.disabled = true
    btnAnular.disabled = true
    btnRechazar.disabled = true
}