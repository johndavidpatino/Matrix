const defaultHighChartOpt = {
    chart: {
        type: 'spline'
    },
    credits: {
        enabled: false
    },
    title: {
        text: '',
        align: 'left'
    },

    subtitle: {
        text: '',
        align: 'left'
    },


    yAxis: {
        title: {
            text: ''
        },
        min: 0,
        max: 100
    },

    xAxis: {
        accessibility: {
            rangeDescription: ''
        }
    },


    plotOptions: {
        series: {
            dataLabels: {
                enabled: true
            }
        }
    },

    series: [],

    responsive: {
        rules: [{
            condition: {
                maxWidth: 500
            },
            chartOptions: {
                legend: {
                    layout: 'horizontal',
                    align: 'center',
                    verticalAlign: 'bottom'
                }
            }
        }]
    }

}

const monthsOrder = {
    'January': 1,
    'February': 2,
    'March': 3,
    'April': 4,
    'May': 5,
    'June': 6,
    'July': 7,
    'August': 8,
    'September': 9,
    'October': 10,
    'November': 11,
    'December': 12,
}
export class Chart {
    elementId
    highChartRef
    options = {}
    constructor(elementId, options) {
        this.elementId = elementId
        this.options = options
    }
    RenderChart(otherOptions = {}) {
        this.highChartRef = Highcharts.chart(this.elementId,
            { ...defaultHighChartOpt, ...this.options, ...otherOptions }
        )
    }

    // Nuevo método para actualizar el gráfico
    UpdateChart(categories, series) {
        if (this.highChartRef) {
            this.highChartRef.xAxis[0].setCategories(categories, false);
            this.highChartRef.series.forEach((s, index) => {
                s.setData(series[index].data, false);
            });
            this.highChartRef.redraw();
        }
    }
}

let lineChart = new Chart('mainChart', { title: { text: '' } })

document.addEventListener('changeFilters', async (event) => {
    let data = event.detail.data
 
    if(!data) return
    let categories = [...new Set(data.map(item => item.Mes))]
    let legends = [...new Set(data.map(item => item.Grupo))]

    categories = categories.sort((a, b) => {
        return monthsOrder[a] - monthsOrder[b]
    })

    let series = legends.map(legend => {
        return {
            name: legend,
            data: data
                .filter(item => item.Grupo === legend)
                .map(value => value.Porcentaje),
            marker: {
                symbol: 'circle'
            },
        }
    })

    // Comprueba si hay datos para mostrar
    if (categories.length > 0 && series.length > 0) {
        lineChart.RenderChart({
            xAxis: {
                categories: categories
            },
            series: series
        })
    } else {
        // Si no hay datos, muestra un mensaje o limpia el gráfico
        lineChart.RenderChart({
            title: {
                text: 'No hay datos para mostrar'
            }
        });
    }
})





