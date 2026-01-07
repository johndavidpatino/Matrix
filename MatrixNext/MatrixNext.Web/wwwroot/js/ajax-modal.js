// Lightweight helpers for AJAX modals + toast + grid refresh
(function(){
  let appModal, appToast;
  document.addEventListener('DOMContentLoaded', () => {
    const modalEl = document.getElementById('appModal');
    if (modalEl) { appModal = new bootstrap.Modal(modalEl); }
    const toastEl = document.getElementById('appToast');
    if (toastEl) { appToast = new bootstrap.Toast(toastEl); }

    // Delegate clicks on elements with [data-modal-url]
    document.body.addEventListener('click', async (ev) => {
      const t = ev.target.closest('[data-modal-url]');
      if (!t) return;
      ev.preventDefault();
      const url = t.getAttribute('data-modal-url');
      const title = t.getAttribute('data-modal-title') || '';
      await openModal(url, title);
    });

    // Handle submit on forms inside modal with [data-ajax]
    document.body.addEventListener('submit', async (ev) => {
      const form = ev.target;
      if (!(form instanceof HTMLFormElement)) return;
      if (!form.matches('#appModalBody form[data-ajax="true"]')) return;
      ev.preventDefault();
      await submitModalForm(form);
    });
  });

  async function openModal(url, title){
    const body = document.getElementById('appModalBody');
    const titleEl = document.getElementById('appModalTitle');
    if (!body) return;
    body.innerHTML = '<div class="text-center p-4"><div class="spinner-border"></div></div>';
    if (titleEl) titleEl.textContent = title;
    try {
      const html = await fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(r => r.text());
      body.innerHTML = html;
      appModal && appModal.show();
    } catch {
      showToast('No se pudo cargar el formulario');
    }
  }

  async function submitModalForm(form){
    const action = form.getAttribute('action') || window.location.href;
    const method = (form.getAttribute('method') || 'post').toUpperCase();
    const data = new FormData(form);
    try {
      const res = await fetch(action, {
        method,
        headers: { 'X-Requested-With': 'XMLHttpRequest' },
        body: data
      });
      const ct = res.headers.get('content-type') || '';
      if (ct.includes('application/json')) {
        const json = await res.json();
        if (json.success) {
          appModal && appModal.hide();
          showToast(json.message || 'Guardado');
          // Optional grid refresh
          const container = document.querySelector('[data-grid-url]');
          if (container) { await reloadGrid(container); }
        } else {
          showToast(json.message || 'Error');
        }
      } else {
        // Validation failed: server returns partial HTML
        const html = await res.text();
        const body = document.getElementById('appModalBody');
        if (body) body.innerHTML = html;
      }
    } catch {
      showToast('Error de red');
    }
  }

  async function reloadGrid(container){
    try {
      const url = container.getAttribute('data-grid-url');
      const html = await fetch(url, { headers: { 'X-Requested-With': 'XMLHttpRequest' } }).then(r => r.text());
      container.innerHTML = html;
    } catch {
      // ignore
    }
  }

  function showToast(msg){
    const body = document.getElementById('appToastBody');
    if (body) body.textContent = msg;
    appToast && appToast.show();
  }
})();
