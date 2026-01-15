// acciones-mejora.js - Gestión de Acciones de Mejora SGC
let currentPage = 1;
const pageSize = 10;
let catalogos = {
    procesos: [],
    usuarios: [],
    fuentesNoConformidad: [],
    fuentesEspecificas: {}
};

// ========== INICIALIZACIÓN ==========
document.addEventListener('DOMContentLoaded', function() {
    cargarCatalogos();
    cargarAcciones();
    verificarPlanesVencidos();
    
    // Event listeners
    document.getElementById('btnFiltrar').addEventListener('click', () => {
        currentPage = 1;
        cargarAcciones();
    });
    
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
    document.getElementById('btnNuevaAccion').addEventListener('click', abrirModalNueva);
    document.getElementById('formAccionMejora').addEventListener('submit', guardarAccion);
    
    // Cambio de fuente no conformidad → cargar fuentes específicas
    document.getElementById('fuenteNoConformidadId').addEventListener('change', async function() {
        const fuenteId = this.value;
        const selectFuente = document.getElementById('fuenteId');
        
        if (!fuenteId) {
            selectFuente.disabled = true;
            selectFuente.innerHTML = '<option value="">Seleccione primero fuente no conformidad...</option>';
            return;
        }

        selectFuente.disabled = false;
        await cargarFuentesEspecificas(fuenteId);
    });
});

// ========== CATÁLOGOS ==========
async function cargarCatalogos() {
    try {
        const [procesos, fuentesNC] = await Promise.all([
            fetch('/api/sgc/acciones-mejora/catalogos/procesos').then(r => r.json()),
            fetch('/api/sgc/acciones-mejora/catalogos/fuentes-no-conformidad').then(r => r.json())
        ]);

        if (procesos.success) {
            catalogos.procesos = procesos.data;
            llenarSelectProcesos();
        }

        if (fuentesNC.success) {
            catalogos.fuentesNoConformidad = fuentesNC.data;
            llenarSelectFuentesNoConformidad();
        }
    } catch (error) {
        console.error('Error cargando catálogos:', error);
        mostrarToast('Error al cargar catálogos', 'error');
    }
}

function llenarSelectProcesos() {
    const selectFiltro = document.getElementById('filtroProceso');
    const selectForm = document.getElementById('procesoId');
    
    const options = catalogos.procesos.map(p => 
        `<option value="${p.ProcesoId}">${p.NombreProceso}</option>`
    ).join('');

    selectFiltro.innerHTML = '<option value="">Todos los procesos</option>' + options;
    selectForm.innerHTML = '<option value="">Seleccione...</option>' + options;
}

function llenarSelectFuentesNoConformidad() {
    const select = document.getElementById('fuenteNoConformidadId');
    select.innerHTML = '<option value="">Seleccione...</option>' +
        catalogos.fuentesNoConformidad.map(f => 
            `<option value="${f.FuenteNoConformidadId}">${f.NombreFuente}</option>`
        ).join('');
}

async function cargarFuentesEspecificas(fuenteNoConformidadId) {
    try {
        if (catalogos.fuentesEspecificas[fuenteNoConformidadId]) {
            // Ya está cacheado
            llenarSelectFuentesEspecificas(catalogos.fuentesEspecificas[fuenteNoConformidadId]);
            return;
        }

        const response = await fetch(`/api/sgc/acciones-mejora/catalogos/fuentes/${fuenteNoConformidadId}`);
        const result = await response.json();

        if (result.success) {
            catalogos.fuentesEspecificas[fuenteNoConformidadId] = result.data;
            llenarSelectFuentesEspecificas(result.data);
        }
    } catch (error) {
        console.error('Error cargando fuentes específicas:', error);
    }
}

function llenarSelectFuentesEspecificas(fuentes) {
    const select = document.getElementById('fuenteId');
    select.innerHTML = '<option value="">Seleccione...</option>' +
        fuentes.map(f => 
            `<option value="${f.FuenteId}">${f.NombreFuente}</option>`
        ).join('');
}

// ========== CARGAR ACCIONES ==========
async function cargarAcciones() {
    const loadingSpinner = document.getElementById('loadingSpinner');
    const tablaAcciones = document.getElementById('tablaAcciones');
    
    loadingSpinner.style.display = 'block';
    tablaAcciones.style.display = 'none';

    try {
        const procesoId = document.getElementById('filtroProceso').value;
        const responsableId = document.getElementById('filtroResponsable').value;

        let url = `/api/sgc/acciones-mejora?pageSize=${pageSize}&pageIndex=${currentPage}`;
        if (procesoId) url += `&procesoId=${procesoId}`;
        if (responsableId) url += `&usuarioResponsable=${responsableId}`;

        const response = await fetch(url);
        const result = await response.json();

        if (result.success) {
            renderizarTabla(result.data);
        } else {
            mostrarToast(result.message, 'error');
        }
    } catch (error) {
        console.error('Error cargando acciones:', error);
        mostrarToast('Error al cargar acciones de mejora', 'error');
    } finally {
        loadingSpinner.style.display = 'none';
        tablaAcciones.style.display = 'block';
    }
}

function renderizarTabla(acciones) {
    const tbody = document.getElementById('bodyAcciones');
    
    if (!acciones || acciones.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="7" class="text-center text-muted py-4">
                    <i class="fas fa-inbox fa-3x mb-3 d-block"></i>
                    No se encontraron acciones de mejora
                </td>
            </tr>`;
        return;
    }

    tbody.innerHTML = acciones.map(accion => `
        <tr>
            <td>${accion.AccionMejoraId}</td>
            <td>
                <div class="text-truncate" style="max-width: 300px;" title="${accion.DescripcionAccion}">
                    ${accion.DescripcionAccion || '-'}
                </div>
            </td>
            <td>${accion.NombreProceso || '-'}</td>
            <td>${accion.NombreUsuarioResponsable || '-'}</td>
            <td>${formatearFecha(accion.FechaIncidente)}</td>
            <td>
                <small class="text-muted">
                    ${accion.NombreFuenteNoConformidad || '-'}
                </small>
            </td>
            <td>
                <div class="btn-group btn-group-sm" role="group">
                    <button class="btn btn-info" onclick="verDetalles(${accion.AccionMejoraId})" title="Ver detalles">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-warning" onclick="editarAccion(${accion.AccionMejoraId})" title="Editar">
                        <i class="fas fa-edit"></i>
                    </button>
                    <button class="btn btn-danger" onclick="eliminarAccion(${accion.AccionMejoraId})" title="Eliminar">
                        <i class="fas fa-trash"></i>
                    </button>
                </div>
            </td>
        </tr>
    `).join('');
}

// ========== GUARDAR ACCIÓN ==========
async function guardarAccion(event) {
    event.preventDefault();
    
    const form = event.target;
    if (!form.checkValidity()) {
        event.stopPropagation();
        form.classList.add('was-validated');
        return;
    }

    const btnGuardar = document.getElementById('btnGuardarAccion');
    btnGuardar.disabled = true;
    btnGuardar.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';

    try {
        const accionMejoraId = document.getElementById('accionMejoraId').value;
        const isEdit = accionMejoraId && accionMejoraId !== '';

        const data = {
            DescripcionAccion: document.getElementById('descripcionAccion').value,
            FechaIncidente: document.getElementById('fechaIncidente').value,
            UsuarioReporta: parseInt(document.getElementById('usuarioReporta').value),
            ProcesoId: parseInt(document.getElementById('procesoId').value),
            UsuarioResponsable: parseInt(document.getElementById('usuarioResponsable').value),
            Descripcion: document.getElementById('descripcion').value || null,
            Correccion: document.getElementById('correccion').value || null,
            FuenteNoConformidadId: parseInt(document.getElementById('fuenteNoConformidadId').value) || null,
            FuenteId: parseInt(document.getElementById('fuenteId').value) || null
        };

        if (isEdit) {
            data.AccionMejoraId = parseInt(accionMejoraId);
        }

        const url = isEdit 
            ? `/api/sgc/acciones-mejora/${accionMejoraId}` 
            : '/api/sgc/acciones-mejora';
        
        const method = isEdit ? 'PUT' : 'POST';

        const response = await fetch(url, {
            method: method,
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (result.success) {
            mostrarToast(result.message, 'success');
            bootstrap.Modal.getInstance(document.getElementById('modalAccionMejora')).hide();
            form.reset();
            form.classList.remove('was-validated');
            cargarAcciones();
        } else {
            mostrarToast(result.message, 'error');
        }
    } catch (error) {
        console.error('Error guardando acción:', error);
        mostrarToast('Error al guardar la acción de mejora', 'error');
    } finally {
        btnGuardar.disabled = false;
        btnGuardar.innerHTML = '<i class="fas fa-save me-2"></i>Guardar';
    }
}

// ========== EDITAR ==========
async function editarAccion(accionMejoraId) {
    try {
        const response = await fetch(`/api/sgc/acciones-mejora/${accionMejoraId}`);
        const result = await response.json();

        if (result.success) {
            const accion = result.data;
            
            document.getElementById('accionMejoraId').value = accion.AccionMejoraId;
            document.getElementById('descripcionAccion').value = accion.DescripcionAccion;
            document.getElementById('fechaIncidente').value = accion.FechaIncidente.split('T')[0];
            document.getElementById('usuarioReporta').value = accion.UsuarioReporta;
            document.getElementById('procesoId').value = accion.ProcesoId;
            document.getElementById('usuarioResponsable').value = accion.UsuarioResponsable;
            document.getElementById('descripcion').value = accion.Descripcion || '';
            document.getElementById('correccion').value = accion.Correccion || '';
            
            if (accion.FuenteNoConformidadId) {
                document.getElementById('fuenteNoConformidadId').value = accion.FuenteNoConformidadId;
                await cargarFuentesEspecificas(accion.FuenteNoConformidadId);
                if (accion.FuenteId) {
                    document.getElementById('fuenteId').value = accion.FuenteId;
                }
            }

            document.getElementById('modalAccionLabel').textContent = 'Editar Acción de Mejora';
            
            const modal = new bootstrap.Modal(document.getElementById('modalAccionMejora'));
            modal.show();
        } else {
            mostrarToast(result.message, 'error');
        }
    } catch (error) {
        console.error('Error cargando acción:', error);
        mostrarToast('Error al cargar la acción', 'error');
    }
}

// ========== ELIMINAR ==========
async function eliminarAccion(accionMejoraId) {
    if (!confirm('¿Está seguro de eliminar esta acción de mejora?')) {
        return;
    }

    try {
        const response = await fetch(`/api/sgc/acciones-mejora/${accionMejoraId}`, {
            method: 'DELETE'
        });

        const result = await response.json();

        if (result.success) {
            mostrarToast(result.message, 'success');
            cargarAcciones();
        } else {
            mostrarToast(result.message, 'error');
        }
    } catch (error) {
        console.error('Error eliminando acción:', error);
        mostrarToast('Error al eliminar la acción', 'error');
    }
}

// ========== VER DETALLES ==========
async function verDetalles(accionMejoraId) {
    try {
        const response = await fetch(`/api/sgc/acciones-mejora/${accionMejoraId}`);
        const result = await response.json();

        if (result.success) {
            const accion = result.data;
            renderizarDetalles(accion);
            
            const modal = new bootstrap.Modal(document.getElementById('modalDetalles'));
            modal.show();
        } else {
            mostrarToast(result.message, 'error');
        }
    } catch (error) {
        console.error('Error cargando detalles:', error);
        mostrarToast('Error al cargar los detalles', 'error');
    }
}

function renderizarDetalles(accion) {
    const infoGeneral = document.getElementById('infoGeneral');
    infoGeneral.innerHTML = `
        <dl class="row">
            <dt class="col-sm-4">Descripción Acción:</dt>
            <dd class="col-sm-8">${accion.DescripcionAccion}</dd>
            
            <dt class="col-sm-4">Proceso:</dt>
            <dd class="col-sm-8">${accion.NombreProceso || '-'}</dd>
            
            <dt class="col-sm-4">Responsable:</dt>
            <dd class="col-sm-8">${accion.NombreUsuarioResponsable || '-'}</dd>
            
            <dt class="col-sm-4">Fecha Incidente:</dt>
            <dd class="col-sm-8">${formatearFecha(accion.FechaIncidente)}</dd>
            
            <dt class="col-sm-4">Usuario Reporta:</dt>
            <dd class="col-sm-8">${accion.NombreUsuarioReporta || '-'}</dd>
            
            ${accion.Descripcion ? `
                <dt class="col-sm-4">Descripción Detallada:</dt>
                <dd class="col-sm-8">${accion.Descripcion}</dd>
            ` : ''}
            
            ${accion.Correccion ? `
                <dt class="col-sm-4">Corrección:</dt>
                <dd class="col-sm-8">${accion.Correccion}</dd>
            ` : ''}
        </dl>
    `;

    // Renderizar causas
    renderizarCausas(accion.Causas || []);
    
    // Renderizar planes de acción
    renderizarPlanes(accion.PlanesAccion || []);
}

function renderizarCausas(causas) {
    const container = document.getElementById('listaCausas');
    
    if (!causas || causas.length === 0) {
        container.innerHTML = '<p class="text-muted">No se han agregado causas raíz</p>';
        return;
    }

    container.innerHTML = causas.map((causa, index) => `
        <div class="card mb-2">
            <div class="card-body">
                <h6 class="card-subtitle mb-2 text-muted">Causa ${index + 1}</h6>
                <p class="card-text">${causa.Descripcion}</p>
            </div>
        </div>
    `).join('');
}

function renderizarPlanes(planes) {
    const container = document.getElementById('listaPlanes');
    
    if (!planes || planes.length === 0) {
        container.innerHTML = '<p class="text-muted">No se han agregado planes de acción</p>';
        return;
    }

    container.innerHTML = planes.map((plan, index) => `
        <div class="card mb-3 ${estaVencido(plan.FechaPlaneado) ? 'border-warning' : ''}">
            <div class="card-body">
                <div class="d-flex justify-content-between align-items-start">
                    <h6 class="card-subtitle mb-2">Plan ${index + 1}</h6>
                    ${estaVencido(plan.FechaPlaneado) ? 
                        '<span class="badge bg-warning">Vencido</span>' : ''}
                </div>
                <p class="card-text mb-2">${plan.Descripcion}</p>
                <div class="row">
                    <div class="col-md-6">
                        <small class="text-muted">
                            <i class="fas fa-calendar me-1"></i>
                            Planeado: ${formatearFecha(plan.FechaPlaneado)}
                        </small>
                    </div>
                    ${plan.FechaVerificacion ? `
                        <div class="col-md-6">
                            <small class="text-success">
                                <i class="fas fa-check me-1"></i>
                                Verificado: ${formatearFecha(plan.FechaVerificacion)}
                            </small>
                        </div>
                    ` : ''}
                </div>
                ${plan.ObservacionVerificacion ? `
                    <small class="text-muted d-block mt-2">
                        <strong>Observación:</strong> ${plan.ObservacionVerificacion}
                    </small>
                ` : ''}
            </div>
        </div>
    `).join('');
}

// ========== PLANES VENCIDOS ==========
async function verificarPlanesVencidos() {
    try {
        const response = await fetch('/api/sgc/acciones-mejora/planes-accion/vencidos');
        const result = await response.json();

        if (result.success && result.data && result.data.length > 0) {
            document.getElementById('countPlanesVencidos').textContent = result.data.length;
            document.getElementById('alertaPlanesVencidos').style.display = 'block';
        }
    } catch (error) {
        console.error('Error verificando planes vencidos:', error);
    }
}

// ========== UTILIDADES ==========
function abrirModalNueva() {
    document.getElementById('formAccionMejora').reset();
    document.getElementById('formAccionMejora').classList.remove('was-validated');
    document.getElementById('accionMejoraId').value = '';
    document.getElementById('modalAccionLabel').textContent = 'Nueva Acción de Mejora';
    document.getElementById('fuenteId').disabled = true;
}

function limpiarFiltros() {
    document.getElementById('filtroProceso').value = '';
    document.getElementById('filtroResponsable').value = '';
    currentPage = 1;
    cargarAcciones();
}

function formatearFecha(fecha) {
    if (!fecha) return '-';
    const date = new Date(fecha);
    return date.toLocaleDateString('es-CO');
}

function estaVencido(fecha) {
    if (!fecha) return false;
    return new Date(fecha) < new Date();
}

function mostrarToast(mensaje, tipo = 'info') {
    // TODO: Implementar toast notifications (Bootstrap Toast o library)
    console.log(`[${tipo.toUpperCase()}] ${mensaje}`);
    alert(mensaje);
}
