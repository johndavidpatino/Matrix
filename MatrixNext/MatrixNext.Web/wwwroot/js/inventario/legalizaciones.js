// ============================================================
// LEGALIZACIONES - INVENTARIO (INV)
// Gestión de legalizaciones con cálculo automático de pendiente
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
        verificado: $('#filtroVerificado').val() || null,
        fechaDesde: $('#filtroFechaDesde').val() || null,
        fechaHasta: $('#filtroFechaHasta').val() || null
    };

    $.get('/INV/Legalizaciones/Index', filtros, function (html) {
        $('#gridContainer').html(html);
    }).fail(function () {
        mostrarError('Error al cargar las legalizaciones');
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

    $(document).on('submit', '#formLegalizacion', function (e) {
        e.preventDefault();
        guardarLegalizacion();
    });

    // Calcular pendiente en tiempo real
    $(document).on('input', '#txtValorLegalizado, .componente-legalizacion', function () {
        calcularPendiente();
    });

    $(document).on('click', '[data-verificar-legalizacion]', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        verificarLegalizacion(id);
    });

    $(document).on('click', '[data-delete-legalizacion]', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        eliminarLegalizacion(id);
    });
}

function abrirModal(url) {
    $.get(url, function (html) {
        $('#modalContainer').html(html);
        $('#modalLegalizacion').modal('show');
        
        if ($.fn.select2) {
            $('.select2').select2({
                theme: 'bootstrap4',
                dropdownParent: $('#modalLegalizacion')
            });
        }
        
        // Calcular pendiente inicial
        calcularPendiente();
    }).fail(function () {
        mostrarError('Error al cargar el formulario');
    });
}

// ============================================================
// CÁLCULO AUTOMÁTICO DE PENDIENTE
// ============================================================
function calcularPendiente() {
    const valorLegalizado = parseFloat($('#txtValorLegalizado').val()) || 0;
    const firmas = parseFloat($('#Firmas').val()) || 0;
    const devoluciones = parseFloat($('#Devoluciones').val()) || 0;
    const notasCredito = parseFloat($('#NotasCredito').val()) || 0;
    const descuentoNomina = parseFloat($('#DescuentoNomina').val()) || 0;
    
    // Pendiente = Valor - (Firmas + Devoluciones + NotasCredito + DescuentoNomina)
    const suma = firmas + devoluciones + notasCredito + descuentoNomina;
    const pendiente = valorLegalizado - suma;
    
    // Actualizar display
    $('#txtPendiente').text(formatearMoneda(pendiente));
    
    // Cambiar color según valor
    if (pendiente > 0) {
        $('#txtPendiente').removeClass('text-success').addClass('text-danger');
    } else if (pendiente === 0) {
        $('#txtPendiente').removeClass('text-danger').addClass('text-success');
    } else {
        $('#txtPendiente').removeClass('text-success text-danger').addClass('text-warning');
    }
    
    return pendiente;
}

function formatearMoneda(valor) {
    return new Intl.NumberFormat('es-CO', {
        style: 'currency',
        currency: 'COP',
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    }).format(valor);
}

// ============================================================
// GUARDAR LEGALIZACIÓN
// ============================================================
function guardarLegalizacion() {
    const form = $('#formLegalizacion');
    
    if (!form.valid()) {
        mostrarAdvertencia('Complete todos los campos obligatorios');
        return;
    }

    const pendiente = calcularPendiente();
    
    // Advertir si hay pendiente
    if (pendiente !== 0) {
        Swal.fire({
            title: 'Advertencia',
            html: `Hay un valor pendiente de <strong>${formatearMoneda(pendiente)}</strong>.<br/>¿Desea continuar?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sí, guardar',
            cancelButtonText: 'Revisar'
        }).then((result) => {
            if (result.isConfirmed) {
                enviarLegalizacion();
            }
        });
    } else {
        enviarLegalizacion();
    }
}

function enviarLegalizacion() {
    const form = $('#formLegalizacion');
    const data = form.serializeObject();
    const url = form.attr('action');
    const method = data.IdLegalizacion > 0 ? 'PUT' : 'POST';

    $.ajax({
        url: url,
        type: method,
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: function () {
            bloquearBoton('#formLegalizacion button[type="submit"]', true);
        },
        success: function (response) {
            if (response.success) {
                mostrarExito(response.message);
                $('#modalLegalizacion').modal('hide');
                cargarGrid();
            } else {
                mostrarError(response.message);
            }
        },
        error: function (xhr) {
            mostrarError(xhr.responseJSON?.message || 'Error al guardar la legalización');
        },
        complete: function () {
            bloquearBoton('#formLegalizacion button[type="submit"]', false);
        }
    });
}

// ============================================================
// VERIFICAR LEGALIZACIÓN
// ============================================================
function verificarLegalizacion(id) {
    Swal.fire({
        title: '¿Verificar legalización?',
        text: 'Una vez verificada, no se podrá modificar',
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#28a745',
        confirmButtonText: 'Sí, verificar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/INV/Legalizaciones/${id}/verificar`,
                type: 'POST',
                success: function (response) {
                    if (response.success) {
                        mostrarExito(response.message);
                        cargarGrid();
                    } else {
                        mostrarError(response.message);
                    }
                },
                error: function (xhr) {
                    mostrarError(xhr.responseJSON?.message || 'Error al verificar la legalización');
                }
            });
        }
    });
}

// ============================================================
// ELIMINAR LEGALIZACIÓN
// ============================================================
function eliminarLegalizacion(id) {
    Swal.fire({
        title: '¿Está seguro?',
        text: 'Esta acción no se puede revertir',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/INV/Legalizaciones/${id}`,
                type: 'DELETE',
                success: function (response) {
                    if (response.success) {
                        mostrarExito(response.message);
                        cargarGrid();
                    } else {
                        mostrarError(response.message);
                    }
                },
                error: function (xhr) {
                    mostrarError(xhr.responseJSON?.message || 'Error al eliminar la legalización');
                }
            });
        }
    });
}

// ============================================================
// UTILIDADES
// ============================================================
$.fn.serializeObject = function () {
    const obj = {};
    const arr = this.serializeArray();
    
    $.each(arr, function () {
        if (obj[this.name]) {
            if (!obj[this.name].push) {
                obj[this.name] = [obj[this.name]];
            }
            obj[this.name].push(this.value || '');
        } else {
            obj[this.name] = this.value || '';
        }
    });
    
    return obj;
};

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
