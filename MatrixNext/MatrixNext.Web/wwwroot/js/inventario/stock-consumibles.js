// ============================================================
// STOCK CONSUMIBLES - INVENTARIO (INV)
// Gestión de entradas/salidas con validación de stock
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
        idConsumible: $('#filtroConsumible').val() || null,
        tipoMovimiento: $('#filtroTipoMovimiento').val() || null,
        fechaDesde: $('#filtroFechaDesde').val() || null,
        fechaHasta: $('#filtroFechaHasta').val() || null
    };

    $.get('/INV/StockConsumibles/Index', filtros, function (html) {
        $('#gridContainer').html(html);
    }).fail(function () {
        mostrarError('Error al cargar los movimientos de stock');
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

    $(document).on('submit', '#formMovimiento', function (e) {
        e.preventDefault();
        registrarMovimiento();
    });

    $(document).on('change', '#IdConsumible', function () {
        verificarStockDisponible();
    });

    $(document).on('change', 'input[name="TipoMovimiento"]', function () {
        validarStockSalida();
    });

    $(document).on('blur', '#Total', function () {
        validarStockSalida();
    });
}

function abrirModal(url) {
    $.get(url, function (html) {
        $('#modalContainer').html(html);
        $('#modalMovimiento').modal('show');
        
        if ($.fn.select2) {
            $('.select2').select2({
                theme: 'bootstrap4',
                dropdownParent: $('#modalMovimiento')
            });
        }
    }).fail(function () {
        mostrarError('Error al cargar el formulario');
    });
}

// ============================================================
// VALIDACIÓN DE STOCK
// ============================================================
function verificarStockDisponible() {
    const idConsumible = $('#IdConsumible').val();
    
    if (!idConsumible) {
        $('#stockDisponibleContainer').hide();
        return;
    }

    $.get(`/INV/StockConsumibles/ObtenerStockDisponible/${idConsumible}`, function (data) {
        if (data.success) {
            $('#stockDisponible').text(data.disponible);
            $('#stockDisponibleContainer').show();
            validarStockSalida();
        }
    }).fail(function () {
        mostrarAdvertencia('No se pudo verificar el stock disponible');
    });
}

function validarStockSalida() {
    const tipoMovimiento = parseInt($('input[name="TipoMovimiento"]:checked').val());
    const cantidad = parseFloat($('#Total').val()) || 0;
    const disponible = parseFloat($('#stockDisponible').text()) || 0;
    
    $('#alertaStock').hide();
    
    // Solo validar en salidas
    if (tipoMovimiento === 2) { // Salida
        if (cantidad > disponible) {
            $('#alertaStock')
                .html(`<i class="fas fa-exclamation-triangle"></i> Stock insuficiente. Disponible: ${disponible}`)
                .show();
            $('#btnGuardarMovimiento').prop('disabled', true);
            return false;
        }
    }
    
    $('#btnGuardarMovimiento').prop('disabled', false);
    return true;
}

// ============================================================
// REGISTRAR MOVIMIENTO
// ============================================================
function registrarMovimiento() {
    const form = $('#formMovimiento');
    
    if (!form.valid()) {
        mostrarAdvertencia('Complete todos los campos obligatorios');
        return;
    }

    if (!validarStockSalida()) {
        mostrarError('Stock insuficiente para registrar la salida');
        return;
    }

    const data = {
        IdConsumible: $('#IdConsumible').val(),
        TipoMovimiento: parseInt($('input[name="TipoMovimiento"]:checked').val()),
        Total: parseFloat($('#Total').val()),
        IdResponsable: $('#IdResponsable').val(),
        Fecha: $('#Fecha').val(),
        Observaciones: $('#Observaciones').val()
    };

    $.ajax({
        url: '/INV/StockConsumibles/RegistrarMovimiento',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: function () {
            bloquearBoton('#btnGuardarMovimiento', true);
        },
        success: function (response) {
            if (response.success) {
                mostrarExito(response.message);
                $('#modalMovimiento').modal('hide');
                cargarGrid();
            } else {
                mostrarError(response.message);
            }
        },
        error: function (xhr) {
            mostrarError(xhr.responseJSON?.message || 'Error al registrar el movimiento');
        },
        complete: function () {
            bloquearBoton('#btnGuardarMovimiento', false);
        }
    });
}

// ============================================================
// LEGALIZAR MOVIMIENTO
// ============================================================
function crearLegalizacion(idMovimiento) {
    window.location.href = `/INV/Legalizaciones/Create?idMovimiento=${idMovimiento}`;
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
