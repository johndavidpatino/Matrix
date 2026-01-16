// ============================================================
// ASIGNACIONES - INVENTARIO (INV)
// Gestión de asignación y devolución de activos fijos
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

    $('.filtros input, .filtros select').on('keypress', function (e) {
        if (e.which === 13) {
            cargarGrid();
        }
    });

    cargarGrid();
}

function cargarGrid() {
    const filtros = {
        idActivoFijo: $('#filtroActivoFijo').val() || null,
        idUsuarioAsignado: $('#filtroUsuario').val() || null,
        fechaDesde: $('#filtroFechaDesde').val() || null,
        fechaHasta: $('#filtroFechaHasta').val() || null,
        page: 1
    };

    $.get('/INV/Asignaciones/Index', filtros, function (html) {
        $('#gridContainer').html(html);
    }).fail(function () {
        mostrarError('Error al cargar las asignaciones');
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

    $(document).on('submit', '#formAsignacion', function (e) {
        e.preventDefault();
        crearAsignacion();
    });

    $(document).on('click', '[data-devolver-asignacion]', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        devolverAsignacion(id);
    });
}

function abrirModal(url) {
    $.get(url, function (html) {
        $('#modalContainer').html(html);
        $('#modalAsignacion').modal('show');
        
        if ($.fn.select2) {
            $('.select2').select2({
                theme: 'bootstrap4',
                dropdownParent: $('#modalAsignacion')
            });
        }
    }).fail(function () {
        mostrarError('Error al cargar el formulario');
    });
}

// ============================================================
// CREAR ASIGNACIÓN
// ============================================================
function crearAsignacion() {
    const form = $('#formAsignacion');
    
    if (!form.valid()) {
        mostrarAdvertencia('Complete todos los campos obligatorios');
        return;
    }

    const data = {
        IdActivoFijo: $('#IdActivoFijo').val(),
        IdUsuarioAsignado: $('#IdUsuarioAsignado').val(),
        FechaAsignacion: $('#FechaAsignacion').val(),
        Observaciones: $('#Observaciones').val()
    };

    $.ajax({
        url: '/INV/Asignaciones/CrearAsignacion',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: function () {
            bloquearBoton('#btnGuardarAsignacion', true);
        },
        success: function (response) {
            if (response.success) {
                mostrarExito(response.message);
                $('#modalAsignacion').modal('hide');
                cargarGrid();
            } else {
                mostrarError(response.message);
            }
        },
        error: function (xhr) {
            mostrarError(xhr.responseJSON?.message || 'Error al crear la asignación');
        },
        complete: function () {
            bloquearBoton('#btnGuardarAsignacion', false);
        }
    });
}

// ============================================================
// DEVOLVER ASIGNACIÓN
// ============================================================
function devolverAsignacion(id) {
    Swal.fire({
        title: 'Registrar Devolución',
        html: `
            <div class="form-group text-left">
                <label class="font-weight-bold">Observaciones de la devolución:</label>
                <textarea id="observacionesDevolucion" 
                          class="form-control" 
                          rows="4" 
                          placeholder="Describa el estado del equipo y las condiciones de devolución..."></textarea>
            </div>
        `,
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#28a745',
        cancelButtonColor: '#6c757d',
        confirmButtonText: '<i class="fas fa-check"></i> Registrar Devolución',
        cancelButtonText: 'Cancelar',
        preConfirm: () => {
            const observaciones = $('#observacionesDevolucion').val();
            if (!observaciones || observaciones.trim().length < 10) {
                Swal.showValidationMessage('Debe ingresar observaciones (mínimo 10 caracteres)');
                return false;
            }
            return { observaciones: observaciones };
        }
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/INV/Asignaciones/${id}/devolver`,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ observaciones: result.value.observaciones }),
                success: function (response) {
                    if (response.success) {
                        mostrarExito(response.message);
                        cargarGrid();
                    } else {
                        mostrarError(response.message);
                    }
                },
                error: function (xhr) {
                    mostrarError(xhr.responseJSON?.message || 'Error al registrar la devolución');
                }
            });
        }
    });
}

// ============================================================
// UTILIDADES
// ============================================================
function bloquearBoton(selector, bloquear) {
    if (bloquear) {
        $(selector).prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i> Procesando...');
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
