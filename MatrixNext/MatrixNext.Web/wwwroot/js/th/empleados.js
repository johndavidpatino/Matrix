document.addEventListener('DOMContentLoaded', function () {
    if (document.getElementById('empleadoApp')) initEmpleadoApp();
});

let catalogosCache = null;
let modalEditarInstance = null;
let toastInstance = null;

function initEmpleadoApp() {
    loadCatalogos();

    // Bootstrap modal instance
    const modalEl = document.getElementById('modalEditar');
    if (modalEl) {
        modalEditarInstance = new bootstrap.Modal(modalEl);
    }

    // Bootstrap toast instance
    const toastEl = document.getElementById('empleadoToast');
    if (toastEl) {
        toastInstance = new bootstrap.Toast(toastEl);
    }

    document.getElementById('btnSearch').addEventListener('click', buscarEmpleados);
    document.getElementById('FotoFile').addEventListener('click', handleFotoFile);
    document.getElementById('btnSaveEmpleado').addEventListener('click', guardarEmpleado);
}

function showLoader(show) {
    const loader = document.getElementById('loader');
    if (loader) {
        loader.style.display = show ? 'block' : 'none';
    }
}

function showToast(msg) {
    const bodyEl = document.getElementById('empleadoToastBody');
    if (bodyEl && toastInstance) {
        bodyEl.textContent = msg;
        toastInstance.show();
    }
}

async function loadCatalogos() {
    try {
        const res = await fetch('/TH/Catalogos/Todos');
        if (!res.ok) return;
        const json = await res.json();
        if (json.success) {
            catalogosCache = json.data;
        }
    } catch (e) {
        console.warn('No se pudieron cargar catálogos', e);
    }
}

async function buscarEmpleados() {
    const payload = {
        Identificacion: document.getElementById('filterIdentificacion').value || null,
        Nombre: document.getElementById('filterNombre').value || null
    };

    const res = await fetch('/TH/Empleados/Search', {
        method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload)
    });
    const j = await res.json();
    if (!j.success) { showToast(j.message || 'Error en búsqueda'); return; }
    renderCards(j.data || []);
}

function renderCards(items) {
    const container = document.getElementById('cardsContainer');
    container.innerHTML = '';
    items.forEach(it => {
        const col = document.createElement('div');
        col.className = 'col-xl-4 col-md-6';
        col.innerHTML = `<div class="card custom-card">
            <div class="card-body">
                <div class="d-flex align-items-center">
                    <div class="avatar avatar-lg me-2">
                        <img src="${it.UrlFoto || '/images/sin-foto.jpg'}" alt="Foto" class="rounded-circle">
                    </div>
                    <div class="flex-fill">
                        <h6 class="mb-0">${it.PrimerNombre || ''} ${it.PrimerApellido || ''}</h6>
                        <span class="text-muted fs-12">${it.Identificacion || ''}</span>
                    </div>
                </div>
                <div class="mt-2">
                    <button data-id="${it.Id}" class="btnEdit btn btn-sm btn-primary-light">Editar</button>
                </div>
            </div>
        </div>`;
        container.appendChild(col);
        col.querySelector('.btnEdit').addEventListener('click', () => abrirModalEditar(it));
    });
}

function abrirModalEditar(item) {
    document.getElementById('PersonaId').value = item.Id || '';
    document.getElementById('TipoIdentificacion').value = item.TipoIdentificacion || '';
    document.getElementById('Identificacion').value = item.Identificacion || '';
    document.getElementById('PrimerNombre').value = item.PrimerNombre || '';
    document.getElementById('PrimerApellido').value = item.PrimerApellido || '';
    document.getElementById('FotoPreview').src = item.UrlFoto || '/images/sin-foto.jpg';
    if (modalEditarInstance) {
        modalEditarInstance.show();
    }
}

function handleFotoFile(ev) {
    const file = ev.target.files[0];
    if (!file) return;
    if (!/^image\/(jpeg|png)$/.test(file.type)) { alert('Solo JPG/PNG permitidos'); return; }
    if (file.size > 2 * 1024 * 1024) { alert('La imagen excede 2MB'); return; }
    const reader = new FileReader();
    reader.onload = function (e) {
        const dataUrl = e.target.result;
        document.getElementById('FotoPreview').src = dataUrl;
        document.getElementById('FotoBase64').value = dataUrl; // incluir data URI
    };
    reader.readAsDataURL(file);
}

async function guardarEmpleado() {
    const dto = {
        PersonaId: parseInt(document.getElementById('PersonaId').value || '0'),
        TipoIdentificacion: document.getElementById('TipoIdentificacion').value,
        Identificacion: document.getElementById('Identificacion').value,
        PrimerNombre: document.getElementById('PrimerNombre').value,
        PrimerApellido: document.getElementById('PrimerApellido').value,
        FotoBase64: document.getElementById('FotoBase64').value
    };

    // Client-side validations
    if (!dto.TipoIdentificacion || !dto.Identificacion || !dto.PrimerNombre || !dto.PrimerApellido) { 
        showToast('Complete los campos requeridos'); 
        return; 
    }

    try {
        showLoader(true);
        const res = await fetch('/TH/Empleados/DatosGenerales', { 
            method: 'PUT', 
            headers: { 'Content-Type': 'application/json' }, 
            body: JSON.stringify(dto) 
        });
        const j = await res.json();
        if (j.success) { 
            showToast('Guardado exitoso'); 
            if (modalEditarInstance) {
                modalEditarInstance.hide();
            }
            buscarEmpleados(); 
        }
        else { 
            showToast(j.message || 'Error al guardar'); 
        }
    } catch (e) { 
        showToast('Error de red'); 
    }
    finally { 
        showLoader(false); 
    }
}
