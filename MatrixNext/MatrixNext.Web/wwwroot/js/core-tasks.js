(() => {
  const apiBase = '/api/Core';

  async function fetchUserId() {
    // Try to read from a claim endpoint or rely on server to infer
    // For simplicity, leave idUsuario empty to call an endpoint that uses server-side user
    return null;
  }

  async function fetchTasks(page = 1, size = 20) {
    const userId = await fetchUserId();
    const url = userId
      ? `${apiBase}/tareas/usuario/${userId}?pageNumber=${page}&pageSize=${size}`
      : `${apiBase}/tareas/usuario/1?pageNumber=${page}&pageSize=${size}`; // fallback
    const res = await fetch(url, { headers: { 'Accept': 'application/json' } });
    if (!res.ok) throw new Error('Error al obtener tareas');
    const json = await res.json();
    return json.data || json.Data || { items: [], totalItems: 0 };
  }

  function renderTasks(data) {
    const tbody = document.querySelector('#tablaTareas tbody');
    tbody.innerHTML = '';
    const items = data.items || data.Items || [];
    for (const t of items) {
      const tr = document.createElement('tr');
      tr.innerHTML = `
        <td>${t.id ?? t.Id}</td>
        <td>${t.idTrabajo ?? t.IdTrabajo}</td>
        <td>${t.idTarea ?? t.IdTarea}</td>
        <td><span class="badge bg-secondary">${t.estado ?? t.Estado}</span></td>
        <td>${t.prioridad ?? t.Prioridad}</td>
        <td>${t.fechaVencimiento ? new Date(t.fechaVencimiento).toLocaleDateString() : (t.FechaVencimiento ? new Date(t.FechaVencimiento).toLocaleDateString() : '')}</td>
        <td>${t.observaciones ?? t.Observaciones ?? ''}</td>
        <td>
          <button class="btn btn-sm btn-success" data-action="complete" data-id="${t.id ?? t.Id}">Cerrar</button>
          <button class="btn btn-sm btn-danger ms-1" data-action="cancel" data-id="${t.id ?? t.Id}">Anular</button>
          <button class="btn btn-sm btn-primary ms-1" data-action="assign" data-id="${t.id ?? t.Id}">Asignar</button>
          <button class="btn btn-sm btn-warning ms-1" data-action="escalate" data-id="${t.id ?? t.Id}">Escalar</button>
        </td>
      `;
      tbody.appendChild(tr);
    }
  }

  async function createTask(payload) {
    const res = await fetch(`${apiBase}/tareas/crear`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
      body: JSON.stringify(payload)
    });
    if (!res.ok) throw new Error('Error al crear tarea');
    return res.json();
  }

  async function completeTask(id) {
    const res = await fetch(`${apiBase}/tareas/${id}/cerrar`, {
      method: 'POST',
      headers: { 'Accept': 'application/json' }
    });
    return res.ok;
  }

  async function cancelTask(id) {
    const res = await fetch(`${apiBase}/tareas/${id}/anular`, {
      method: 'DELETE',
      headers: { 'Accept': 'application/json' }
    });
    return res.ok;
  }

  async function assignTask(id, payload) {
    const res = await fetch(`${apiBase}/tareas/${id}/asignar`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
      body: JSON.stringify(payload)
    });
    return res.ok;
  }

  async function escalateTask(id, payload) {
    const res = await fetch(`${apiBase}/tareas/${id}/escalar`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json', 'Accept': 'application/json' },
      body: JSON.stringify(payload)
    });
    return res.ok;
  }

  function bindEvents() {
    const btnCrear = document.getElementById('btnCrear');
    const crearModalEl = document.getElementById('crearTareaModal');
    const crearModal = new bootstrap.Modal(crearModalEl);
    btnCrear.addEventListener('click', () => crearModal.show());
    document.getElementById('btnGuardarCrear').addEventListener('click', async () => {
      const form = document.getElementById('formCrearTarea');
      const payload = {
        IdTrabajo: Number(form.IdTrabajo.value),
        IdTarea: Number(form.IdTarea.value),
        IdTipoHilo: Number(form.IdTipoHilo.value),
        Prioridad: Number(form.Prioridad.value),
        FechaVencimiento: form.FechaVencimiento.value || null,
        Observaciones: form.Observaciones.value || null
      };
      try {
        await createTask(payload);
        crearModal.hide();
        const data = await fetchTasks();
        renderTasks(data);
      } catch (e) {
        console.error(e);
        alert('No se pudo crear la tarea');
      }
    });

    const asignarModalEl = document.getElementById('asignarTareaModal');
    const asignarModal = new bootstrap.Modal(asignarModalEl);
    const escalarModalEl = document.getElementById('escalarTareaModal');
    const escalarModal = new bootstrap.Modal(escalarModalEl);
    let currentTaskId = null;

    document.querySelector('#tablaTareas tbody').addEventListener('click', async (ev) => {
      const btn = ev.target.closest('button[data-action]');
      if (!btn) return;
      const id = btn.getAttribute('data-id');
      const action = btn.getAttribute('data-action');
      try {
        if (action === 'complete') await completeTask(id);
        if (action === 'cancel') await cancelTask(id);
        if (action === 'assign') {
          currentTaskId = id;
          asignarModal.show();
          return;
        }
        if (action === 'escalate') {
          currentTaskId = id;
          escalarModal.show();
          return;
        }
        const data = await fetchTasks();
        renderTasks(data);
      } catch (e) {
        console.error(e);
        alert('Acción no completada');
      }
    });

    document.getElementById('btnGuardarAsignar').addEventListener('click', async () => {
      const form = document.getElementById('formAsignarTarea');
      const ids = (form.IdUsuarios.value || '')
        .split(',')
        .map(s => s.trim())
        .filter(Boolean)
        .map(Number)
        .filter(n => !isNaN(n));
      const payload = {
        IdUsuarios: ids,
        Rol: form.Rol.value || 'Ejecutor',
        Comentario: form.Comentario.value || null
      };
      try {
        if (!currentTaskId) throw new Error('Tarea no seleccionada');
        const ok = await assignTask(currentTaskId, payload);
        if (!ok) throw new Error('Error al asignar');
        asignarModal.hide();
        const data = await fetchTasks();
        renderTasks(data);
      } catch (e) {
        console.error(e);
        alert('No se pudo asignar la tarea');
      }
    });

    document.getElementById('btnGuardarEscalar').addEventListener('click', async () => {
      const form = document.getElementById('formEscalarTarea');
      const payload = {
        IdUsuarioDestino: Number(form.IdUsuarioDestino.value),
        Motivo: form.Motivo.value,
        Comentario: form.Comentario.value || null
      };
      try {
        if (!currentTaskId) throw new Error('Tarea no seleccionada');
        const ok = await escalateTask(currentTaskId, payload);
        if (!ok) throw new Error('Error al escalar');
        escalarModal.hide();
        const data = await fetchTasks();
        renderTasks(data);
      } catch (e) {
        console.error(e);
        alert('No se pudo escalar la tarea');
      }
    });
  }

  (async function init() {
    try {
      bindEvents();
      const data = await fetchTasks();
      renderTasks(data);
    } catch (e) {
      console.error(e);
    }
  })();
})();
