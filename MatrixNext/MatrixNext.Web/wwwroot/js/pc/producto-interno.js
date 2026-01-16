/**
 * producto-interno.js
 * Funciones para gestión de productos internos
 */

// Abrir modal para crear producto
function abrirModalCrear() {
    $.ajax({
        url: '/PC/ProductoInterno/Create',
        type: 'GET',
        success: function (html) {
            $('#modalContent').html(html);
            $('#modalProducto').modal('show');
            
            // Cargar catálogos después de abrir modal
            cargarProyectos();
            cargarUnidades();
            cargarTiposMovimiento();
            
            // Configurar submit AJAX
            configurarFormularioAjax('#formProducto');
        },
        error: function () {
            mostrarMensaje('Error al abrir el formulario', 'danger');
        }
    });
}

// Editar producto existente
function editarProducto(id) {
    $.ajax({
        url: '/PC/ProductoInterno/Edit/' + id,
        type: 'GET',
        success: function (html) {
            $('#modalContent').html(html);
            $('#modalProducto').modal('show');
            
            // Cargar catálogos
            cargarProyectos();
            cargarUnidades();
            cargarTiposMovimiento();
            
            // Configurar submit AJAX
            configurarFormularioAjax('#formProducto');
        },
        error: function () {
            mostrarMensaje('Error al cargar el producto', 'danger');
        }
    });
}

// Ver detalles del producto
function verDetalles(id) {
    $.ajax({
        url: '/PC/ProductoInterno/Details/' + id,
        type: 'GET',
        success: function (html) {
            $('#modalContent').html(html);
            $('#modalProducto').modal('show');
        },
        error: function () {
            mostrarMensaje('Error al cargar los detalles', 'danger');
        }
    });
}

// Recibir producto
function recibirProducto(id) {
    $.ajax({
        url: '/PC/ProductoInterno/Recibir/' + id,
        type: 'GET',
        success: function (html) {
            $('#modalContent').html(html);
            $('#modalProducto').modal('show');
            
            // Configurar submit AJAX
            configurarFormularioAjax('#formRecibir');
        },
        error: function () {
            mostrarMensaje('Error al abrir el formulario de recepción', 'danger');
        }
    });
}

// Eliminar producto
function eliminarProducto(id) {
    if (!confirm('¿Está seguro de eliminar este producto?\n\nEsta acción no se puede deshacer.')) {
        return;
    }

    $.ajax({
        url: '/PC/ProductoInterno/Delete/' + id,
        type: 'POST',
        data: {
            __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
        },
        success: function (response) {
            if (response.success) {
                mostrarMensaje(response.message, 'success');
                // Recargar página después de 1 segundo
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                mostrarMensaje(response.message, 'danger');
            }
        },
        error: function () {
            mostrarMensaje('Error al eliminar el producto', 'danger');
        }
    });
}

// Configurar formulario para submit AJAX
function configurarFormularioAjax(formSelector) {
    $(formSelector).on('submit', function (e) {
        e.preventDefault();

        var form = $(this);
        var url = form.attr('action');

        $.ajax({
            url: url,
            type: 'POST',
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    $('#modalProducto').modal('hide');
                    mostrarMensaje(response.message, 'success');
                    
                    // Recargar página después de 1 segundo
                    setTimeout(function () {
                        location.reload();
                    }, 1000);
                } else {
                    // Si hay errores de validación, mostrar mensaje
                    mostrarMensaje(response.message || 'Error al guardar el producto', 'danger');
                }
            },
            error: function (xhr) {
                if (xhr.status === 400) {
                    // Errores de validación - recargar el formulario con errores
                    $('#modalContent').html(xhr.responseText);
                    configurarFormularioAjax(formSelector);
                } else {
                    mostrarMensaje('Error al procesar la solicitud', 'danger');
                }
            }
        });
    });
}

// Cargar proyectos en dropdown
function cargarProyectos() {
    // TODO: Implementar carga desde API cuando esté disponible
    // Por ahora, valores hardcoded para testing
    var selectProyecto = $('#selectProyecto');
    if (selectProyecto.length && selectProyecto.children('option').length === 1) {
        // Agregar opciones de ejemplo
        selectProyecto.append('<option value="1">Proyecto Demo 1</option>');
        selectProyecto.append('<option value="2">Proyecto Demo 2</option>');
    }
}

// Cargar unidades en dropdowns
function cargarUnidades() {
    // TODO: Implementar carga desde API cuando esté disponible
    var selectUnidadEnvia = $('#selectUnidadEnvia');
    var selectUnidadRecibe = $('#selectUnidadRecibe');
    
    if (selectUnidadEnvia.length && selectUnidadEnvia.children('option').length === 1) {
        // Agregar opciones de ejemplo
        var unidades = [
            { id: 1, nombre: 'Unidad 1 - Bogotá' },
            { id: 2, nombre: 'Unidad 2 - Medellín' },
            { id: 3, nombre: 'Unidad 3 - Cali' }
        ];

        unidades.forEach(function (unidad) {
            selectUnidadEnvia.append('<option value="' + unidad.id + '">' + unidad.nombre + '</option>');
            selectUnidadRecibe.append('<option value="' + unidad.id + '">' + unidad.nombre + '</option>');
        });
    }
}

// Cargar tipos de movimiento
function cargarTiposMovimiento() {
    // TODO: Implementar carga desde API cuando esté disponible
    var selectTipo = $('#selectTipo');
    
    if (selectTipo.length && selectTipo.children('option').length === 1) {
        // Agregar opciones de ejemplo
        selectTipo.append('<option value="1">Envío Regular</option>');
        selectTipo.append('<option value="2">Transferencia</option>');
        selectTipo.append('<option value="3">Devolución</option>');
    }
}

// Mostrar mensaje toast
function mostrarMensaje(mensaje, tipo) {
    // Si existe un contenedor de alertas, usarlo
    var alertHtml = '<div class="alert alert-' + tipo + ' alert-dismissible fade show" role="alert">' +
        mensaje +
        '<button type="button" class="close" data-dismiss="alert"><span>&times;</span></button>' +
        '</div>';

    // Insertar al inicio del container
    $('.container-fluid').prepend(alertHtml);

    // Auto-cerrar después de 5 segundos
    setTimeout(function () {
        $('.alert').fadeOut('slow', function () {
            $(this).remove();
        });
    }, 5000);
}

// Inicializar DataTables si está disponible
$(document).ready(function () {
    if ($.fn.DataTable && $('#tablaProductos').length) {
        $('#tablaProductos').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json'
            },
            pageLength: 25,
            order: [[5, 'desc']], // Ordenar por fecha de envío descendente
            columnDefs: [
                { orderable: false, targets: 8 } // Columna de acciones no ordenable
            ]
        });
    }
});
