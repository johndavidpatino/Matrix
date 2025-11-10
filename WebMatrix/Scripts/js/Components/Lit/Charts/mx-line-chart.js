import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js'

class MxLineChart extends LitElement {
    static properties = {
        elementId: { type: String },
        title: { type: String },
        data: { type: Array },
    };

    constructor() {
        super();
        this.elementId = null;
        this.title = '';
        this.data = [];
        this.highChartRef = null;
    }

    updated() {
        this.renderChart();
    }

    renderChart(otherOptions = {}) {
       const elementChart = this._elementChart
       if(!elementChart) return
        this.highChartRef = Highcharts.chart(elementChart, {
            ...defaultHighChartOpt,
            ...{ title: { text: this.title } },
            ...otherOptions
        });
    }

    get _elementChart(){
        return this.renderRoot.querySelector(`#${this.elementId}`)
    }
    updateChart() {
        const categories = [...new Set(this.data.map(item => item.Mes))].sort((a, b) => monthsOrder[a] - monthsOrder[b]);
        const legends = [...new Set(this.data.map(item => item.Grupo))];

        const series = legends.map(legend => ({
            name: legend,
            data: this.data
                .filter(item => item.Grupo === legend)
                .map(value => value.Porcentaje),
            marker: {
                symbol: 'circle'
            },
        }));

        if (categories.length > 0 && series.length > 0) {
            if (this.highChartRef) {
                this.highChartRef.xAxis[0].setCategories(categories, false);
                this.highChartRef.series.forEach((s, index) => {
                    s.setData(series[index].data, false);
                });
                this.highChartRef.redraw();
            } else {
                this.renderChart({
                    xAxis: { categories },
                    series
                });
            }
        } else {
            this.renderChart({
                title: { text: 'No hay datos para mostrar' }
            });
        }
    }
    setData(data){
        this.data = data
        this.updateChart()
    }
    render() {
        return html`
            <div id="${this.elementId}" class="mx-line-chart"></div>
        `;
    }

    static styles = css`
        .mx-line-chart {
            width: 100%;
            height: 100%;
        }
    `;

}

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
};

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
};

customElements.define('mx-line-chart', MxLineChart);