/**
 * Generic CRUD module for nested resources (Experiencias, Educación, Hijos, etc)
 * Usage: NestedResourcesModule.init('experiencias', '/api/empleados/{id}/experiencias', { title: 'Experiencias Laborales' })
 */
const NestedResourcesModule = (() => {
    let config = {};
    let currentParentId = null;
    let allItems = [];

    const init = (resourceName, apiEndpoint, options = {}) => {
        config = {
            resourceName,
            apiEndpoint,
            title: options.title || resourceName,
            parentIdParam: options.parentIdParam || 'personaId',
            ...options
        };
    };

    const loadItems = async (parentId) => {
        currentParentId = parentId;
        const endpoint = config.apiEndpoint.replace('{id}', parentId);
        
        try {
            const response = await apiCall('GET', endpoint, null);
            if (response.success && response.data) {
                allItems = response.data;
                renderTable();
            } else {
                showToast(`Error cargando ${config.title}`, 'error');
            }
        } catch (error) {
            console.error('Error:', error);
            showToast(`Error al cargar ${config.title}`, 'error');
        }
    };

    const renderTable = () => {
        const tbody = document.getElementById(`${config.resourceName}TableBody`);
        if (!allItems || allItems.length === 0) {
            tbody.innerHTML = `<tr><td colspan="5" class="text-center py-4 text-muted">No hay ${config.title.toLowerCase()} registradas</td></tr>`;
            return;
        }

        tbody.innerHTML = allItems.map((item, idx) => {
            const cells = Object.values(item).slice(1, 5).map(v => `<td>${v || ''}</td>`).join('');
            return `<tr>
                <td>${idx + 1}</td>
                ${cells}
                <td class="text-center">
                    <button class="btn btn-sm btn-danger" onclick="NestedResourcesModule.deleteItem(${item.id})">
                        <i class="bi bi-trash"></i>
                    </button>
                </td>
            </tr>`;
        }).join('');
    };

    const saveItem = async () => {
        const form = document.getElementById(`${config.resourceName}Form`);
        if (!form.checkValidity()) {
            form.classList.add('was-validated');
            return;
        }

        const formData = new FormData(form);
        const data = Object.fromEntries(formData);
        data[config.parentIdParam] = currentParentId;

        const endpoint = config.apiEndpoint.replace('{id}', currentParentId);
        
        try {
            const response = await apiCall('POST', endpoint, data);
            if (response.success) {
                showToast(`${config.title} guardado correctamente`, 'success');
                form.reset();
                form.classList.remove('was-validated');
                loadItems(currentParentId);
                const modal = bootstrap.Modal.getInstance(document.getElementById(`${config.resourceName}Modal`));
                if (modal) modal.hide();
            } else {
                showToast(response.message || `Error al guardar ${config.title}`, 'error');
            }
        } catch (error) {
            console.error('Error:', error);
            showToast(`Error al guardar ${config.title}`, 'error');
        }
    };

    const deleteItem = async (itemId) => {
        if (!confirm(`¿Está seguro de que desea eliminar este ${config.title.toLowerCase()}?`)) return;

        const endpoint = `${config.apiEndpoint.replace('{id}', currentParentId)}/${itemId}`;
        
        try {
            const response = await apiCall('DELETE', endpoint, null);
            if (response.success) {
                showToast(`${config.title} eliminado correctamente`, 'success');
                loadItems(currentParentId);
            } else {
                showToast(response.message || `Error al eliminar ${config.title}`, 'error');
            }
        } catch (error) {
            console.error('Error:', error);
            showToast(`Error al eliminar ${config.title}`, 'error');
        }
    };

    const resetForm = () => {
        const form = document.getElementById(`${config.resourceName}Form`);
        if (form) {
            form.reset();
            form.classList.remove('was-validated');
        }
    };

    const showToast = (message, type = 'success') => {
        const toastId = type === 'success' ? 'successToast' : 'errorToast';
        const messageId = type === 'success' ? 'successToastMessage' : 'errorToastMessage';
        const elem = document.getElementById(messageId);
        if (elem) {
            elem.textContent = message;
            const toast = new bootstrap.Toast(document.getElementById(toastId));
            toast.show();
        }
    };

    const apiCall = async (method, endpoint, data) => {
        const options = {
            method: method,
            headers: { 'Content-Type': 'application/json' }
        };
        if (data && typeof data === 'object' && !(data instanceof FormData)) {
            options.body = JSON.stringify(data);
        } else if (data instanceof FormData) {
            options.body = data;
            delete options.headers['Content-Type'];
        }
        const response = await fetch(endpoint, options);
        return response.json();
    };

    return {
        init,
        loadItems,
        saveItem,
        deleteItem,
        resetForm
    };
})();
