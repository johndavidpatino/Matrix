const DesvinculacionesModule = (() => {
    let currentPage = 1;
    const pageSize = 10;
    let allDesvinculaciones = [];

    const init = () => {
        loadDesvinculaciones();
    };

    const loadDesvinculaciones = async () => {
        const textoBuscado = document.getElementById('filtroText')?.value || '';
        await apiCall('GET', `/api/desvinculaciones?pageSize=${pageSize}&pageIndex=${currentPage}&textoBuscado=${encodeURIComponent(textoBuscado)}`, null)
            .then(response => {
                if (response.success && response.data) {
                    allDesvinculaciones = response.data;
                    renderTable();
                    updatePaginationControls();
                } else {
                    showToast('Error cargando desvinculaciones', 'error');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showToast('Error al cargar las desvinculaciones', 'error');
            });
    };

    const renderTable = () => {
        const tbody = document.getElementById('desvinculacionesTableBody');
        if (!allDesvinculaciones || allDesvinculaciones.length === 0) {
            tbody.innerHTML = '<tr><td colspan="6" class="text-center py-4 text-muted">No hay desvinculaciones registradas</td></tr>';
            return;
        }

        tbody.innerHTML = allDesvinculaciones.map(d => `
            <tr>
                <td>${d.id || ''}</td>
                <td>${d.nombreEmpleado || ''}</td>
                <td>${d.identificacion || ''}</td>
                <td>${d.fechaRetiro ? new Date(d.fechaRetiro).toLocaleDateString() : ''}</td>
                <td>
                    <span class="badge ${d.estado === 'Completada' ? 'bg-success' : 'bg-warning'}">
                        ${d.estado || 'Pendiente'}
                    </span>
                </td>
                <td class="text-center">
                    <button type="button" class="btn btn-sm btn-info" onclick="DesvinculacionesModule.viewEvaluaciones(${d.id})" title="Ver Evaluaciones">
                        <i class="bi bi-file-text"></i>
                    </button>
                    <button type="button" class="btn btn-sm btn-danger" onclick="DesvinculacionesModule.deleteDesvinculacion(${d.id})" title="Eliminar">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            </tr>
        `).join('');
    };

    const updatePaginationControls = () => {
        document.getElementById('pageIndicator').innerHTML = `<span class="page-link">Página ${currentPage}</span>`;
        document.getElementById('prevPageBtn').classList.toggle('disabled', currentPage === 1);
        document.getElementById('nextPageBtn').classList.toggle('disabled', allDesvinculaciones.length < pageSize);
    };

    const nextPage = () => {
        if (allDesvinculaciones.length >= pageSize) {
            currentPage++;
            loadDesvinculaciones();
        }
    };

    const previousPage = () => {
        if (currentPage > 1) {
            currentPage--;
            loadDesvinculaciones();
        }
    };

    const saveDesvinculacion = async () => {
        const form = document.getElementById('desvinculacionForm');
        if (!form.checkValidity()) {
            form.classList.add('was-validated');
            return;
        }

        const data = {
            empleadoId: parseInt(document.getElementById('empleadoId').value),
            fechaRetiro: document.getElementById('fechaRetiro').value,
            observacion: document.getElementById('observacion').value
        };

        await apiCall('POST', '/api/desvinculaciones', data)
            .then(response => {
                if (response.success) {
                    showToast('Desvinculación registrada correctamente', 'success');
                    bootstrap.Modal.getInstance(document.getElementById('desvinculacionModal')).hide();
                    currentPage = 1;
                    loadDesvinculaciones();
                } else {
                    showToast(response.message || 'Error al registrar desvinculación', 'error');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showToast('Error al registrar desvinculación', 'error');
            });
    };

    const deleteDesvinculacion = async (id) => {
        if (!confirm('¿Está seguro de que desea eliminar esta desvinculación?')) return;

        await apiCall('DELETE', `/api/desvinculaciones/${id}`, null)
            .then(response => {
                if (response.success) {
                    showToast('Desvinculación eliminada correctamente', 'success');
                    loadDesvinculaciones();
                } else {
                    showToast(response.message || 'Error al eliminar desvinculación', 'error');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showToast('Error al eliminar desvinculación', 'error');
            });
    };

    const viewEvaluaciones = async (desvinculacionId) => {
        await apiCall('GET', `/api/desvinculaciones/${desvinculacionId}/evaluaciones`, null)
            .then(response => {
                if (response.success && response.data) {
                    alert('Evaluaciones:\n' + JSON.stringify(response.data, null, 2));
                } else {
                    showToast('Error cargando evaluaciones', 'error');
                }
            })
            .catch(error => {
                console.error('Error:', error);
                showToast('Error al cargar evaluaciones', 'error');
            });
    };

    const applyFilters = () => {
        currentPage = 1;
        loadDesvinculaciones();
    };

    const clearFilters = () => {
        document.getElementById('filtroText').value = '';
        currentPage = 1;
        loadDesvinculaciones();
    };

    const resetForm = () => {
        document.getElementById('desvinculacionForm').reset();
        document.getElementById('desvinculacionForm').classList.remove('was-validated');
        document.getElementById('desvinculacionModalLabel').textContent = 'Nueva Desvinculación';
    };

    const showToast = (message, type = 'success') => {
        const toastId = type === 'success' ? 'successToast' : 'errorToast';
        const messageId = type === 'success' ? 'successToastMessage' : 'errorToastMessage';
        document.getElementById(messageId).textContent = message;
        const toast = new bootstrap.Toast(document.getElementById(toastId));
        toast.show();
    };

    const apiCall = async (method, endpoint, data) => {
        const options = {
            method: method,
            headers: { 'Content-Type': 'application/json' }
        };
        if (data) options.body = JSON.stringify(data);
        const response = await fetch(endpoint, options);
        return response.json();
    };

    return {
        init,
        nextPage,
        previousPage,
        loadDesvinculaciones,
        saveDesvinculacion,
        deleteDesvinculacion,
        viewEvaluaciones,
        applyFilters,
        clearFilters,
        resetForm
    };
})();
