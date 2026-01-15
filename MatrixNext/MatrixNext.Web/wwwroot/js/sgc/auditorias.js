// auditorias.js - Gestión de Auditorías Internas SGC
let currentPage = 1;
const pageSize = 10;
let catalogos = {
    estados: [],
    normativas: [],
    tiposAuditoria: [],
    tiposHallazgo: [],
    usuarios: []
};

// ========== INICIALIZACIÓN ==========
document.addEventListener('DOMContentLoaded', function() {
    cargarCatalogos();
    cargarAuditorias();
    
    // Event listeners
    document.getElementById('btnFiltrar').addEventListener('click', () => {
        currentPage = 1;
        cargarAuditorias();
    });
    
    document.getElementById('btnLimpiarFiltros').addEventListener('click', limpiarFiltros);
    document.getElementById('btnNuevaAuditoria').addEventListener('click', abrirModalNueva);
    document.getElementById('formAuditoria').addEventListener('submit', guardarAuditoria);
    document.getElementById('formInforme').addEventListener('submit', guardarInforme);
    document.getElementById('btnAgregarAuditado').addEventListener('click', agregarCampoAuditado);
    document.getElementById('btnAgregarHallazgo').addEventListener('click', agregarCampoHallazgo);
});

// ========== CATÁLOGOS ==========
async function cargarCatalogos() {
    try {
        // Cargar todos los catálogos en paralelo
        const [estados, normativas, tiposAuditoria, tiposHallazgo] = await Promise.all([
            fetch('/api/sgc/auditorias/catalogos/estados').then(r => r.json()),
            fetch('/api/sgc/auditorias/catalogos/normativas').then(r => r.json()),
            fetch('/api/sgc/auditorias/catalogos/tipos-auditoria').then(r => r.json()),
            fetch('/api/sgc/auditorias/catalogos/tipos-hallazgo').then(r => r.json())
        ]);

        if (estados.success) {
            catalogos.estados = estados.data;
            llenarSelectEstados();
        }

        if (normativas.success) {
            catalogos.normativas = normativas.data;
            llenarSelectNormativas();
        }

        if (tiposAuditoria.success) {
            catalogos.tiposAuditoria = tiposAuditoria.data;
            llenarSelectTiposAuditoria();
        }

        if (tiposHallazgo.success) {
            catalogos.tiposHallazgo = tiposHallazgo.data;
        }
    } catch (error) {
        console.error('Error cargando catálogos:', error);
        mostrarToast('Error al cargar catálogos', 'error');
    }
}

function llenarSelectEstados() {
    const select = document.getElementById('filtroEstado');
    select.innerHTML = '<option value="">Todos los estados</option>';
    catalogos.estados.forEach(estado => {
        select.innerHTML += `<option value="${estado.EstadoId}">${estado.NombreEstado}</option>`;
    });
}

function llenarSelectNormativas() {
    const select = document.getElementById('normativaId');
    select.innerHTML = '<option value="">Seleccione...</option>';
    catalogos.normativas.forEach(normativa => {
        select.innerHTML += `<option value="${normativa.NormativaId}">${normativa.NombreNormativa}</option>`;
    });
}

function llenarSelectTiposAuditoria() {
    const select = document.getElementById('tipoAuditoriaId');
    select.innerHTML = '<option value="">Seleccione...</option>';
    catalogos.tiposAuditoria.forEach(tipo => {
        select.innerHTML += `<option value="${tipo.TipoAuditoriaId}">${tipo.NombreTipoAuditoria}</option>`;
    });
}

// ========== CARGAR AUDITORÍAS ==========
async function cargarAuditorias() {
    const loadingSpinner = document.getElementById('loadingSpinner');
    const tablaAuditorias = document.getElementById('tablaAuditorias');
    
    loadingSpinner.style.display = 'block';
    tablaAuditorias.style.display = 'none';

    try {
        const estadoId = document.getElementById('filtroEstado').value;
        const anoAuditoria = document.getElementById('filtroAno').value;

        let url = `/api/sgc/auditorias?pageSize=${pageSize}&pageIndex=${currentPage}`;
        if (estadoId) url += `&estadoId=${estadoId}`;
        if (anoAuditoria) url += `&anoAuditoria=${anoAuditoria}`;

        const response = await fetch(url);
        const result = await response.json();

        if (result.success) {
            renderizarTabla(result.data);
        } else {
            mostrarToast(result.message, 'error');
        }
    } catch (error) {
        console.error('Error cargando auditorías:', error);
        mostrarToast('Error al cargar auditorías', 'error');
    } finally {
        loadingSpinner.style.display = 'none';
        tablaAuditorias.style.display = 'block';
    }
}

function renderizarTabla(auditorias) {
    const tbody = document.getElementById('bodyAuditorias');
    
    if (!auditorias || auditorias.length === 0) {
        tbody.innerHTML = `
            <tr>
                <td colspan="8" class="text-center text-muted py-4">
                    <i class="fas fa-inbox fa-3x mb-3 d-block"></i>
                    No se encontraron auditorías
                </td>
            </tr>`;
        return;
    }

    tbody.innerHTML = auditorias.map(auditoria => `
        <tr>
            <td>${auditoria.AuditoriaId}</td>
            <td>${auditoria.NombreNormativa || '-'}</td>
            <td>${auditoria.NombreTipoAuditoria || '-'}</td>
            <td>${formatearFecha(auditoria.FechaLimite)}</td>
            <td>${auditoria.NombreAuditor || '-'}</td>
            <td>${auditoria.AnoAuditoria}</td>
            <td>
                <span class="badge ${getBadgeClass(auditoria.EstadoId)}">
                    ${auditoria.NombreEstado || 'Pendiente'}
                </span>
            </td>
            <td>
                <div class="btn-group btn-group-sm" role="group">
                    <button class="btn btn-info" onclick="verDetalle(${auditoria.AuditoriaId})" title="Ver detalle">
                        <i class="fas fa-eye"></i>
                    </button>
                    ${auditoria.EstadoId === 20 || auditoria.EstadoId === 30 ? `
                        <button class="btn btn-success" onclick="abrirModalInforme(${auditoria.AuditoriaId})" title="Diligenciar informe">
                            <i class="fas fa-file-alt"></i>
                        </button>
                    ` : ''}
                    ${auditoria.EstadoId === 30 ? `
                        <button class="btn btn-warning" onclick="cambiarEstado(${auditoria.AuditoriaId}, 40)" title="Aprobar">
                            <i class="fas fa-check"></i>
                        </button>
                    ` : ''}
                </div>
            </td>
        </tr>
    `).join('');
}

function getBadgeClass(estadoId) {
    switch(estadoId) {
        case 20: return 'bg-secondary';  // Creada
        case 30: return 'bg-warning';     // Diligenciada
        case 40: return 'bg-success';     // Aprobada
        case 50: return 'bg-info';        // Cerrada
        default: return 'bg-secondary';
    }
}

// ========== GUARDAR AUDITORÍA ==========
async function guardarAuditoria(event) {
    event.preventDefault();
    
    const form = event.target;
    if (!form.checkValidity()) {
        event.stopPropagation();
        form.classList.add('was-validated');
        return;
    }

    // Validar al menos un auditado
    const auditados = obtenerAuditados();
    if (auditados.length === 0) {
        mostrarToast('Debe agregar al menos un auditado', 'error');
        return;
    }

    const btnGuardar = document.getElementById('btnGuardarAuditoria');
    btnGuardar.disabled = true;
    btnGuardar.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';

    try {
        const data = {
            NormativaId: parseInt(document.getElementById('normativaId').value),
            TipoAuditoriaId: parseInt(document.getElementById('tipoAuditoriaId').value),
            FechaLimite: document.getElementById('fechaLimite').value,
            AuditorId: parseInt(document.getElementById('auditorId').value),
            AnoAuditoria: parseInt(document.getElementById('anoAuditoria').value),
            Auditados: auditados
        };

        const response = await fetch('/api/sgc/auditorias', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (result.success) {
            mostrarToast(result.message, 'success');
            bootstrap.Modal.getInstance(document.getElementById('modalAuditoria')).hide();
            form.reset();
            form.classList.remove('was-validated');
            document.getElementById('listaAuditados').innerHTML = '';
            cargarAuditorias();
        } else {
            mostrarToast(result.message, 'error');
        }
    } catch (error) {
        console.error('Error guardando auditoría:', error);
        mostrarToast('Error al guardar la auditoría', 'error');
    } finally {
        btnGuardar.disabled = false;
        btnGuardar.innerHTML = '<i class="fas fa-save me-2"></i>Guardar';
    }
}

// ========== INFORME AUDITOR ==========
async function abrirModalInforme(auditoriaId) {
    document.getElementById('informeAuditoriaId').value = auditoriaId;
    document.getElementById('listaHallazgos').innerHTML = '';
    
    const modal = new bootstrap.Modal(document.getElementById('modalInformeAuditor'));
    modal.show();
}

async function guardarInforme(event) {
    event.preventDefault();
    
    const form = event.target;
    if (!form.checkValidity()) {
        event.stopPropagation();
        form.classList.add('was-validated');
        return;
    }

    // Validar al menos un hallazgo
    const hallazgos = obtenerHallazgos();
    if (hallazgos.length === 0) {
        mostrarToast('Debe agregar al menos un hallazgo', 'error');
        return;
    }

    const btnGuardar = document.getElementById('btnGuardarInforme');
    btnGuardar.disabled = true;
    btnGuardar.innerHTML = '<span class="spinner-border spinner-border-sm me-2"></span>Guardando...';

    try {
        const auditoriaId = parseInt(document.getElementById('informeAuditoriaId').value);
        const data = {
            AuditoriaId: auditoriaId,
            Conclusiones: document.getElementById('conclusiones').value,
            Hallazgos: hallazgos
        };

        const response = await fetch(`/api/sgc/auditorias/${auditoriaId}/informe`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data)
        });

        const result = await response.json();

        if (result.success) {
            mostrarToast(result.message, 'success');
            bootstrap.Modal.getInstance(document.getElementById('modalInformeAuditor')).hide();
            form.reset();
            form.classList.remove('was-validated');
            cargarAuditorias();
        } else {
            mostrarToast(result.message, 'error');
        }
    } catch (error) {
        console.error('Error guardando informe:', error);
        mostrarToast('Error al guardar el informe', 'error');
    } finally {
        btnGuardar.disabled = false;
        btnGuardar.innerHTML = '<i class="fas fa-check me-2"></i>Guardar Informe';
    }
}

// ========== UTILIDADES ==========
function agregarCampoAuditado() {
    const container = document.getElementById('listaAuditados');
    const index = container.children.length;
    
    const div = document.createElement('div');
    div.className = 'row mb-2 auditado-item';
    div.innerHTML = `
        <div class="col-md-10">
            <select class="form-select form-select-sm" name="auditados[]" required>
                <option value="">Seleccione un auditado...</option>
            </select>
        </div>
        <div class="col-md-2">
            <button type="button" class="btn btn-sm btn-danger w-100" onclick="this.parentElement.parentElement.remove()">
                <i class="fas fa-trash"></i>
            </button>
        </div>
    `;
    container.appendChild(div);
}

function agregarCampoHallazgo() {
    const container = document.getElementById('listaHallazgos');
    const index = container.children.length;
    
    const div = document.createElement('div');
    div.className = 'card mb-3 hallazgo-item';
    div.innerHTML = `
        <div class="card-body">
            <div class="row g-2">
                <div class="col-md-6">
                    <label class="form-label form-label-sm">Tipo de Hallazgo</label>
                    <select class="form-select form-select-sm" name="tipoHallazgo[]" required>
                        <option value="">Seleccione...</option>
                        ${catalogos.tiposHallazgo.map(tipo => 
                            `<option value="${tipo.TipoHallazgoId}">${tipo.NombreTipoHallazgo}</option>`
                        ).join('')}
                    </select>
                </div>
                <div class="col-md-12">
                    <label class="form-label form-label-sm">Descripción del Hallazgo</label>
                    <textarea class="form-control form-control-sm" name="descripcionHallazgo[]" rows="2" required></textarea>
                </div>
                <div class="col-md-12 text-end">
                    <button type="button" class="btn btn-sm btn-danger" onclick="this.closest('.hallazgo-item').remove()">
                        <i class="fas fa-trash"></i> Eliminar
                    </button>
                </div>
            </div>
        </div>
    `;
    container.appendChild(div);
}

function obtenerAuditados() {
    const selects = document.querySelectorAll('[name="auditados[]"]');
    return Array.from(selects)
        .map(select => parseInt(select.value))
        .filter(value => !isNaN(value) && value > 0);
}

function obtenerHallazgos() {
    const items = document.querySelectorAll('.hallazgo-item');
    return Array.from(items).map(item => ({
        TipoHallazgoId: parseInt(item.querySelector('[name="tipoHallazgo[]"]').value),
        Descripcion: item.querySelector('[name="descripcionHallazgo[]"]').value
    }));
}

function abrirModalNueva() {
    document.getElementById('formAuditoria').reset();
    document.getElementById('formAuditoria').classList.remove('was-validated');
    document.getElementById('listaAuditados').innerHTML = '';
    document.getElementById('modalAuditoriaLabel').textContent = 'Nueva Auditoría Interna';
}

function limpiarFiltros() {
    document.getElementById('filtroEstado').value = '';
    document.getElementById('filtroAno').value = '';
    currentPage = 1;
    cargarAuditorias();
}

async function cambiarEstado(auditoriaId, nuevoEstadoId) {
    if (!confirm('¿Está seguro de cambiar el estado de esta auditoría?')) {
        return;
    }

    try {
        const response = await fetch(`/api/sgc/auditorias/${auditoriaId}/estado`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ NuevoEstadoId: nuevoEstadoId })
        });

        const result = await response.json();

        if (result.success) {
            mostrarToast(result.message, 'success');
            cargarAuditorias();
        } else {
            mostrarToast(result.message, 'error');
        }
    } catch (error) {
        console.error('Error cambiando estado:', error);
        mostrarToast('Error al cambiar el estado', 'error');
    }
}

async function verDetalle(auditoriaId) {
    try {
        const response = await fetch(`/api/sgc/auditorias/${auditoriaId}`);
        const result = await response.json();

        if (result.success) {
            // TODO: Mostrar modal con detalles completos
            console.log('Detalle de auditoría:', result.data);
            mostrarToast('Funcionalidad de detalle en desarrollo', 'info');
        } else {
            mostrarToast(result.message, 'error');
        }
    } catch (error) {
        console.error('Error obteniendo detalle:', error);
        mostrarToast('Error al obtener el detalle', 'error');
    }
}

function formatearFecha(fecha) {
    if (!fecha) return '-';
    const date = new Date(fecha);
    return date.toLocaleDateString('es-CO');
}

function mostrarToast(mensaje, tipo = 'info') {
    // TODO: Implementar toast notifications (Bootstrap Toast o library)
    console.log(`[${tipo.toUpperCase()}] ${mensaje}`);
    alert(mensaje);
}
