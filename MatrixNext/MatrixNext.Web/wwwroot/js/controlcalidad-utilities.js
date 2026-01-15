/**
 * Utilidades AJAX para Control de Calidad
 * Módulo: PY_ControlCalidad
 */

// Configuración de momentjs para formato local
moment.locale('es', {
    months: 'enero_febrero_marzo_abril_mayo_junio_julio_agosto_septiembre_octubre_noviembre_diciembre'.split('_'),
    monthsShort: 'ene_feb_mar_abr_may_jun_jul_ago_sep_oct_nov_dic'.split('_'),
    weekdays: 'domingo_lunes_martes_miércoles_jueves_viernes_sábado'.split('_'),
    weekdaysShort: 'dom_lun_mar_mié_jue_vie_sab'.split('_'),
    weekdaysMin: 'do_lu_ma_mi_ju_vi_sa'.split('_'),
    longDateFormat: {
        LT: 'HH:mm',
        L: 'DD/MM/YYYY',
        LL: 'D [de] MMMM [de] YYYY',
        LLL: 'D [de] MMMM [de] YYYY HH:mm',
        LLLL: 'dddd, D [de] MMMM [de] YYYY HH:mm'
    }
});

/**
 * Carga dinámicamente trabajos para un selector
 */
function cargarTrabajosSelector(selectId) {
    $.ajax({
        url: '/api/py/trabajos/activos',
        type: 'GET',
        success: function (response) {
            if (response.success) {
                var select = $('#' + selectId);
                select.empty();
                select.append('<option value="">-- Seleccione un trabajo --</option>');
                
                $.each(response.data, function (idx, trabajo) {
                    select.append('<option value="' + trabajo.id + '">' + trabajo.nombre + '</option>');
                });
            }
        },
        error: function () {
            console.error('Error cargando trabajos');
        }
    });
}

/**
 * Carga dinámicamente personas para un selector
 */
function cargarPersonasSelector(selectId) {
    $.ajax({
        url: '/api/th/personas/activas',
        type: 'GET',
        success: function (response) {
            if (response.success) {
                var select = $('#' + selectId);
                select.empty();
                select.append('<option value="">-- Seleccione una persona --</option>');
                
                $.each(response.data, function (idx, persona) {
                    select.append('<option value="' + persona.id + '">' + persona.nombres + ' ' + persona.apellidos + '</option>');
                });
            }
        },
        error: function () {
            console.error('Error cargando personas');
        }
    });
}

/**
 * Formatea una fecha a DD/MM/YYYY
 */
function formatearFecha(fecha) {
    if (!fecha) return '';
    return moment(fecha).format('DD/MM/YYYY');
}

/**
 * Formatea una fecha con hora a DD/MM/YYYY HH:mm
 */
function formatearFechaHora(fecha) {
    if (!fecha) return '';
    return moment(fecha).format('DD/MM/YYYY HH:mm');
}

/**
 * Obtiene la clase de badge según la calificación
 */
function obtenerClaseBadgeCalificacion(calificacion) {
    if (calificacion >= 80) return 'badge-success';
    if (calificacion >= 60) return 'badge-warning';
    return 'badge-danger';
}

/**
 * Valida que un formulario tenga datos antes de guardar
 */
function validarFormulario(formSelector) {
    var form = $(formSelector);
    if (!form.length) {
        console.error('Formulario no encontrado: ' + formSelector);
        return false;
    }
    
    return form.valid();
}

/**
 * Genera HTML para las respuestas en formato de tabla
 */
function generarTablaRespuestas(respuestas) {
    var html = '<table class="table table-striped table-sm">' +
        '<thead>' +
        '<tr>' +
        '<th>Pregunta</th>' +
        '<th>Respuesta</th>' +
        '<th>Calificación</th>' +
        '</tr>' +
        '</thead>' +
        '<tbody>';
    
    $.each(respuestas, function (idx, respuesta) {
        var badgeClass = obtenerClaseBadgeCalificacion(respuesta.calificacion);
        html += '<tr>' +
            '<td>' + (respuesta.preguntaTexto || '') + '</td>' +
            '<td>' + (respuesta.respuesta || '') + '</td>' +
            '<td><span class="badge ' + badgeClass + '">' + respuesta.calificacion + '%</span></td>' +
            '</tr>';
    });
    
    html += '</tbody></table>';
    return html;
}

/**
 * Muestra un modal de confirmación antes de eliminar
 */
function confirmarEliminar(mensaje, callbackSi, callbackNo) {
    if (confirm(mensaje || '¿Está seguro de que desea eliminar este registro?')) {
        if (typeof callbackSi === 'function') {
            callbackSi();
        }
    } else {
        if (typeof callbackNo === 'function') {
            callbackNo();
        }
    }
}

/**
 * Exporta datos de un DataTable a CSV
 */
function exportarTablaCSV(tableSelector, nombreArchivo) {
    var table = $(tableSelector).DataTable();
    var csv = '';
    var headers = [];
    
    // Obtener encabezados
    table.columns().header().to$().each(function () {
        headers.push($(this).text());
    });
    
    csv += headers.join(',') + '\n';
    
    // Obtener datos
    table.rows({ search: 'applied' }).data().each(function (row) {
        var rowData = [];
        $.each(row, function (idx, cell) {
            rowData.push('"' + (cell || '').replace(/"/g, '""') + '"');
        });
        csv += rowData.join(',') + '\n';
    });
    
    // Descargar
    var elemento = document.createElement('a');
    elemento.setAttribute('href', 'data:text/csv;charset=utf-8,' + encodeURIComponent(csv));
    elemento.setAttribute('download', nombreArchivo || 'reporte.csv');
    elemento.style.display = 'none';
    document.body.appendChild(elemento);
    elemento.click();
    document.body.removeChild(elemento);
}

/**
 * Calcula el promedio de un array de números
 */
function calcularPromedio(numeros) {
    if (!numeros || numeros.length === 0) return 0;
    var suma = numeros.reduce(function (a, b) { return a + b; }, 0);
    return Math.round((suma / numeros.length) * 100) / 100;
}

/**
 * Añade una fila a una tabla HTML
 */
function agregarFilaTabla(tableSelector, datosFila) {
    var table = $(tableSelector).DataTable();
    table.row.add(datosFila).draw();
}

/**
 * Limpia los datos de un formulario
 */
function limpiarFormulario(formSelector) {
    $(formSelector)[0].reset();
    $(formSelector).find('.is-invalid').removeClass('is-invalid');
    $(formSelector).find('.invalid-feedback').remove();
}

console.log('✓ Utilities de Control de Calidad cargadas');
