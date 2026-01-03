document.addEventListener('DOMContentLoaded', function () {
    if (document.getElementById('desvApp')) initDesvApp();
});

let modalIniciarInstance = null;
let modalEvaluacionesInstance = null;
let toastDesvInstance = null;

function initDesvApp() {
    // Bootstrap modals
    const modalIniciar = document.getElementById('modalIniciar');
    if (modalIniciar) {
        modalIniciarInstance = new bootstrap.Modal(modalIniciar);
    }

    const modalEval = document.getElementById('modalEvaluaciones');
    if (modalEval) {
        modalEvaluacionesInstance = new bootstrap.Modal(modalEval);
    }

    // Bootstrap toast
    const toastEl = document.getElementById('desvToast');
    if (toastEl) {
        toastDesvInstance = new bootstrap.Toast(toastEl);
    }

    document.getElementById('btnBuscarDesv').addEventListener('click', buscarDesv);
    document.getElementById('btnConfirmIniciar').addEventListener('click', iniciarDesvinculacion);
}

function showLoader(show) {
    const loader = document.getElementById('loader');
    if (loader) {
        loader.style.display = show ? 'block' : 'none';
    }
}

function showToastDesv(msg) {
    const bodyEl = document.getElementById('desvToastBody');
    if (bodyEl && toastDesvInstance) {
        bodyEl.textContent = msg;
        toastDesvInstance.show();
    }
}

async function buscarDesv() {
    const payload = { pageSize: 10, pageIndex: 0, textoBuscado: document.getElementById('searchTexto').value || '' };
    const res = await fetch('/TH/Desvinculaciones/Buscar', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(payload) });
    const j = await res.json();
    if (!j.success) { showToastDesv(j.message || 'Error'); return; }
    renderDesv(j.data?.Desvinculaciones || []);
}

function renderDesv(items) {
    const tbody = document.querySelector('#tblDesv tbody');
    tbody.innerHTML = '';
    items.forEach(it => {
        const tr = document.createElement('tr');
        tr.innerHTML = `<td>${it.Id}</td><td>${it.EmpleadoNombre}</td><td><span class="badge bg-info">${it.Estatus}</span></td><td>${it.FechaRegistro}</td><td>
            <button data-id="${it.Id}" class="btnPdf btn btn-sm btn-primary-light"><i class="ri-file-pdf-line"></i> PDF</button>
            <button data-id="${it.Id}" class="btnEval btn btn-sm btn-info-light"><i class="ri-list-check"></i> Evaluaciones</button>
        </td>`;
        tbody.appendChild(tr);
        tr.querySelector('.btnPdf').addEventListener('click', () => descargarPdf(it.Id));
        tr.querySelector('.btnEval').addEventListener('click', () => verEvaluaciones(it.Id));
    });
}

function abrirModalIniciar(item) {
    document.getElementById('inEmpleadoId').value = item.EmpleadoId || '';
    document.getElementById('inFechaRetiro').value = '';
    document.getElementById('inMotivos').value = '';
    if (modalIniciarInstance) {
        modalIniciarInstance.show();
    }
}

async function iniciarDesvinculacion() {
    const dto = {
        EmpleadoId: parseInt(document.getElementById('inEmpleadoId').value || '0'),
        FechaRetiro: document.getElementById('inFechaRetiro').value,
        MotivosDesvinculacion: document.getElementById('inMotivos').value
    };
    const res = await fetch('/TH/Desvinculaciones/Iniciar', { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(dto) });
    const j = await res.json();
    if (j.success) { 
        showToastDesv('Proceso iniciado'); 
        if (modalIniciarInstance) {
            modalIniciarInstance.hide();
        }
        buscarDesv(); 
    }
    else { showToastDesv(j.message || 'Error'); }
}

async function descargarPdf(id) {
    const res = await fetch(`/TH/Desvinculaciones/${id}/PDF`);
    if (!res.ok) { alert('Error generando PDF'); return; }
    const blob = await res.blob();
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url; a.download = `desvinculacion_${id}.pdf`; document.body.appendChild(a); a.click(); a.remove();
}

async function verEvaluaciones(id){
    try {
        showLoader(true);
        const res = await fetch(`/TH/Desvinculaciones/${id}/Evaluaciones`);
        const j = await res.json();
        if (!j.success) { 
            showToastDesv(j.message || 'Error'); 
            return; 
        }
        renderEvaluaciones(j.data || []);
        if (modalEvaluacionesInstance) {
            modalEvaluacionesInstance.show();
        }
    } catch (e) { 
        showToastDesv('Error de red'); 
    }
    finally { 
        showLoader(false); 
    }
}

function renderEvaluaciones(items) {
    const c = document.getElementById('evaluacionesContent'); 
    c.innerHTML = '';
    if (!items || items.length == 0) { 
        c.innerHTML = '<div class="alert alert-info">No hay evaluaciones.</div>'; 
        return; 
    }
    const accordion = document.createElement('div');
    accordion.className = 'accordion';
    accordion.id = 'evalAccordion';
    items.forEach((it, idx) => {
        const itemHtml = `<div class="accordion-item">
            <h2 class="accordion-header">
                <button class="accordion-button collapsed" type="button" data-bs-toggle="collapse" data-bs-target="#collapse${idx}">
                    ${it.NombreArea} - <span class="badge bg-${it.Estado === 'Completado' ? 'success' : 'warning'} ms-2">${it.Estado || ''}</span>
                </button>
            </h2>
            <div id="collapse${idx}" class="accordion-collapse collapse" data-bs-parent="#evalAccordion">
                <div class="accordion-body">
                    <p><strong>Comentarios:</strong></p>
                    <p>${it.Comentarios || ''}</p>
                    <p><strong>Evaluador:</strong> ${it.NombreEvaluadorCompleto || ''}</p>
                    <p><strong>Fecha:</strong> ${it.FechaDiligenciamiento || ''}</p>
                </div>
            </div>
        </div>`;
        accordion.innerHTML += itemHtml;
    });
    c.appendChild(accordion);
}

function closeEvaluaciones() { 
    if (modalEvaluacionesInstance) {
        modalEvaluacionesInstance.hide();
    }
}

function showOverlayDesv(show) { 
    // No-op: usar loader global 
}

function showToastDesv(msg, timeout = 3000) { 
    const bodyEl = document.getElementById('desvToastBody');
    if (bodyEl && toastDesvInstance) {
        bodyEl.textContent = msg;
        toastDesvInstance.show();
    }
}
