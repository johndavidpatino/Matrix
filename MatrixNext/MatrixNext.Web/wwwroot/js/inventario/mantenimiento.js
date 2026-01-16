// ============================================================
// MANTENIMIENTO DE EQUIPOS - INVENTARIO (INV)
// Gestión de historial de mantenimientos por activo
// ============================================================

$(document).ready(function () {
    inicializarGrid();
    configurarEventos();
});

// ============================================================
// INICIALIZACIÓN
// ============================================================
function inicializarGrid() {
    $('#btnFiltrar').on('click', function () {
        cargarGrid();
    });

    cargarGrid();
}

function cargarGrid() {
    const filtros = {
        texto: $('#filtroTexto').val() || null,
        fechaDesde: $('#filtroFechaDesde').val() || null,
        fechaHasta: $('#filtroFechaHasta').val() || null
    };

    $.get('/INV/MantenimientoEquipos/Index', filtros, function (html) {
        $('#gridContainer').html(html);
    }).fail(function () {
        mostrarError('Error al cargar los mantenimientos');
    });
}

// ============================================================
// EVENTOS
// ============================================================
function configurarEventos() {
    $(document).on('click', '[data-ajax-modal]', function (e) {
        e.preventDefault();
        const url = $(this).data('url');
        abrirModal(url);
    });

    $(document).on('submit', '#formMantenimiento', function (e) {
        e.preventDefault();
        guardarMantenimiento();
    });

    $(document).on('click', '[data-ver-historial]', function (e) {
        e.preventDefault();
        const idActivo = $(this).data('id-activo');
        verHistorial(idActivo);
    });

    // Validar fecha no sea futura
    $(document).on('change', '#Fecha', function () {
        const fecha = new Date($(this).val());
        const hoy = new Date();
        hoy.setHours(0, 0, 0, 0);
        
        if (fecha > hoy) {
            mostrarAdvertencia('La fecha de mantenimiento no puede ser futura');
            $(this).val('');
        }
    });

    // Validar longitud mínima de observaciones
    $(document).on('blur', '#Observaciones', function () {
        const texto = $(this).val().trim();
        if (texto.length > 0 && texto.length < 10) {
            mostrarAdvertencia('Las observaciones deben tener al menos 10 caracteres');
        }
    });
}

function abrirModal(url) {
    $.get(url, function (html) {
        $('#modalContainer').html(html);
        $('#modalMantenimiento').modal('show');
        
        if ($.fn.select2) {
            $('.select2').select2({
                theme: 'bootstrap4',
                dropdownParent: $('#modalMantenimiento'),
                width: '100%'
            });
        }
    }).fail(function () {
        mostrarError('Error al cargar el formulario');
    });
}

// ============================================================
// GUARDAR MANTENIMIENTO
// ============================================================
function guardarMantenimiento() {
    const form = $('#formMantenimiento');
    
    if (!form.valid()) {
        mostrarAdvertencia('Complete todos los campos obligatorios');
        return;
    }

    const observaciones = $('#Observaciones').val().trim();
    if (observaciones.length < 10) {
        mostrarAdvertencia('Las observaciones deben tener al menos 10 caracteres');
        return;
    }

    const data = {
        IdMantenimiento: $('#IdMantenimiento').val() || 0,
        IdActivoFijo: $('#IdActivoFijo').val(),
        Fecha: $('#Fecha').val(),
        Observaciones: observaciones
    };

    const url = form.attr('action');
    const method = data.IdMantenimiento > 0 ? 'PUT' : 'POST';

    $.ajax({
        url: url,
        type: method,
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: function () {
            bloquearBoton('#formMantenimiento button[type="submit"]', true);
        },
        success: function (response) {
            if (response.success) {
                mostrarExito(response.message);
                $('#modalMantenimiento').modal('hide');
                cargarGrid();
            } else {
                mostrarError(response.message);
            }
        },
        error: function (xhr) {
            mostrarError(xhr.responseJSON?.message || 'Error al guardar el mantenimiento');
        },
        complete: function () {
            bloquearBoton('#formMantenimiento button[type="submit"]', false);
        }
    });
}

// ============================================================
// VER HISTORIAL POR ACTIVO
// ============================================================
function verHistorial(idActivo) {
    $.get(`/INV/MantenimientoEquipos/ObtenerPorActivo/${idActivo}`, function (html) {
        Swal.fire({
            title: 'Historial de Mantenimientos',
            html: html,
            width: '800px',
            showCloseButton: true,
            showConfirmButton: false,
            customClass: {
                popup: 'swal-historial-mantenimiento'
            }
        });
    }).fail(function () {
        mostrarError('Error al cargar el historial');
    });
}

// ============================================================
// UTILIDADES
// ============================================================
function bloquearBoton(selector, bloquear) {
    if (bloquear) {
        $(selector).prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i> Guardando...');
    } else {
        $(selector).prop('disabled', false).html('<i class="fas fa-save"></i> Guardar');
    }
}

function mostrarExito(mensaje) {
    toastr.success(mensaje);
}

function mostrarError(mensaje) {
    toastr.error(mensaje);
}

function mostrarAdvertencia(mensaje) {
    toastr.warning(mensaje);
}
