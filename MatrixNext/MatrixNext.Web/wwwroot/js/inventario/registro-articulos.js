// ============================================================
// REGISTRO DE ARTÍCULOS - INVENTARIO (INV)
// Gestión CRUD de artículos con tabs dinámicos por tipo
// ============================================================

$(document).ready(function () {
    inicializarGrid();
    configurarEventos();
});

// ============================================================
// INICIALIZACIÓN GRID
// ============================================================
function inicializarGrid() {
    // Configurar filtros
    $('#btnFiltrar').on('click', function () {
        cargarGrid();
    });

    // Enter en filtros
    $('.filtros input').on('keypress', function (e) {
        if (e.which === 13) {
            cargarGrid();
        }
    });

    // Cargar grid inicial
    cargarGrid();
}

function cargarGrid() {
    const filtros = {
        texto: $('#filtroTexto').val(),
        idTipoArticulo: $('#filtroTipoArticulo').val() || null,
        marca: $('#filtroMarca').val() || null,
        serial: $('#filtroSerial').val() || null,
        asignado: $('#filtroAsignado').is(':checked') ? true : null
    };

    $.get('/INV/RegistroArticulos/Obtener', filtros, function (html) {
        $('#gridContainer').html(html);
    }).fail(function () {
        mostrarError('Error al cargar el listado de artículos');
    });
}

// ============================================================
// EVENTOS MODALES
// ============================================================
function configurarEventos() {
    // Abrir modal crear
    $(document).on('click', '[data-ajax-modal]', function (e) {
        e.preventDefault();
        const url = $(this).data('url');
        abrirModal(url);
    });

    // Submit formulario
    $(document).on('submit', '#formArticulo', function (e) {
        e.preventDefault();
        guardarArticulo();
    });

    // Cambio de tipo de artículo
    $(document).on('change', '#IdTipoArticulo', function () {
        mostrarCamposSegunTipo();
    });

    // Validación de campos numéricos
    $(document).on('blur', '#ValorUnitario', function () {
        const valor = parseFloat($(this).val()) || 0;
        if (valor < 0) {
            $(this).val(0);
            mostrarAdvertencia('El valor debe ser mayor o igual a 0');
        }
    });
}

function abrirModal(url) {
    $.get(url, function (html) {
        $('#modalContainer').html(html);
        $('#modalArticulo').modal('show');
        
        // Inicializar tabs después de abrir modal
        mostrarCamposSegunTipo();
        
        // Configurar select2 si existe
        if ($.fn.select2) {
            $('.select2').select2({
                theme: 'bootstrap4',
                dropdownParent: $('#modalArticulo')
            });
        }
    }).fail(function () {
        mostrarError('Error al cargar el formulario');
    });
}

// ============================================================
// LÓGICA DE TABS DINÁMICOS
// ============================================================
function mostrarCamposSegunTipo() {
    const idTipo = parseInt($('#IdTipoArticulo').val()) || 0;
    
    // Ocultar todos los tabs
    $('.tab-content').hide();
    
    // Limpiar validaciones required de todos los tabs
    $('.tab-content input, .tab-content select, .tab-content textarea').prop('required', false);
    
    // Mostrar tab correspondiente y activar validaciones
    switch (idTipo) {
        case 1: // Computadores
            $('#tabComputadores').show();
            $('#Procesador, #RAM, #DiscoDuro, #SistemaOperativo').prop('required', true);
            break;
        case 2: // Tablets
            $('#tabTablets').show();
            $('#MarcaTablet, #ModeloTablet, #SistemaOperativoTablet').prop('required', true);
            break;
        case 3: // Celulares
            $('#tabCelulares').show();
            $('#IMEI, #Chip, #Operador, #SistemaOperativoCelular').prop('required', true);
            break;
        case 4: // Consumibles
            $('#tabConsumibles').show();
            $('#CantidadConsumible, #UnidadMedidaConsumible').prop('required', true);
            break;
        case 5: // Periféricos
            $('#tabPerifericos').show();
            $('#TipoPeriferico').prop('required', true);
            break;
        case 6: // Papelería
            $('#tabPapeleria').show();
            $('#CantidadPapeleria, #UnidadMedidaPapeleria').prop('required', true);
            break;
        case 7: // Mobiliario
            $('#tabMobiliario').show();
            $('#DescripcionMobiliario').prop('required', true);
            break;
        case 8: // Otros
            $('#tabOtros').show();
            $('#DescripcionOtros').prop('required', true);
            break;
    }
}

// ============================================================
// GUARDAR ARTÍCULO
// ============================================================
function guardarArticulo() {
    const form = $('#formArticulo');
    
    if (!form.valid()) {
        mostrarAdvertencia('Complete todos los campos obligatorios');
        return;
    }

    const data = form.serializeObject();
    const url = form.attr('action');
    const method = data.IdArticulo > 0 ? 'PUT' : 'POST';

    $.ajax({
        url: url,
        type: method,
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: function () {
            bloquearBoton('#btnGuardarArticulo', true);
        },
        success: function (response) {
            if (response.success) {
                mostrarExito(response.message);
                $('#modalArticulo').modal('hide');
                cargarGrid();
            } else {
                mostrarError(response.message);
            }
        },
        error: function (xhr) {
            mostrarError(xhr.responseJSON?.message || 'Error al guardar el artículo');
        },
        complete: function () {
            bloquearBoton('#btnGuardarArticulo', false);
        }
    });
}

// ============================================================
// VER DETALLES
// ============================================================
function verDetallesArticulo(id) {
    const url = `/INV/RegistroArticulos/Details/${id}`;
    
    $.get(url, function (html) {
        $('#modalContainer').html(html);
        $('#modalDetalles').modal('show');
    }).fail(function () {
        mostrarError('Error al cargar los detalles del artículo');
    });
}

// ============================================================
// ELIMINAR ARTÍCULO
// ============================================================
function eliminarArticulo(id) {
    Swal.fire({
        title: '¿Está seguro?',
        text: 'Esta acción no se puede revertir',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sí, eliminar',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: `/INV/RegistroArticulos/${id}`,
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
                    mostrarError(xhr.responseJSON?.message || 'Error al eliminar el artículo');
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
