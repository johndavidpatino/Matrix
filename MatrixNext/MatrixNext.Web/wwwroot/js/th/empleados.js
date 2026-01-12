// Empleados Module - AJAX CRUD
const EmpleadosModule = (function () {
    const API_BASE = '/api/th/empleados';
    const PAGE_SIZE = 10;
    let currentPage = 1;
    let currentFilters = {};
    let totalPages = 1;

    // Toast helper
    const showToast = (message, type = 'info') => {
        const toast = document.getElementById('empleadoToast');
        const toastBody = document.getElementById('empleadoToastBody');
        
        // Remove previous classes
        toast.classList.remove('bg-success', 'bg-danger', 'bg-warning', 'bg-info');
        
        // Add class based on type
        if (type === 'success') toast.classList.add('bg-success', 'text-white');
        else if (type === 'error') toast.classList.add('bg-danger', 'text-white');
        else if (type === 'warning') toast.classList.add('bg-warning');
        
        toastBody.textContent = message;
        const bsToast = new bootstrap.Toast(toast);
        bsToast.show();
    };

    // API Helper
    const apiCall = async (method, endpoint, data = null) => {
        try {
            const options = {
                method: method,
                headers: {
                    'Content-Type': 'application/json',
                    'X-Requested-With': 'XMLHttpRequest'
                }
            };

            if (data) {
                options.body = JSON.stringify(data);
            }

            const response = await fetch(API_BASE + endpoint, options);
            const result = await response.json();

            if (!response.ok) {
                throw new Error(result.message || `HTTP ${response.status}`);
            }

            return result;
        } catch (error) {
            console.error('API Error:', error);
            throw error;
        }
    };

    // Load empleados with pagination
    const loadEmpleados = async (page = 1) => {
        try {
            currentPage = page;
            const tbody = document.getElementById('empleadosList');
            
            // Show loading
            tbody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center text-muted py-4">
                        <div class="spinner-border spinner-border-sm" role="status">
                            <span class="visually-hidden">Cargando...</span>
                        </div>
                        Cargando empleados...
                    </td>
                </tr>
            `;

            // Build query params
            const params = new URLSearchParams({
                pageNumber: page,
                pageSize: PAGE_SIZE,
                ...(currentFilters.identificacion && { identificacion: currentFilters.identificacion }),
                ...(currentFilters.nombre && { nombre: currentFilters.nombre }),
                ...(currentFilters.estado !== undefined && { estado: currentFilters.estado })
            });

            const result = await apiCall('GET', `?${params}`);

            if (result.success && result.data) {
                renderEmpleadosTable(result.data);
                
                // Update pagination info
                const totalRecords = result.totalRecords || result.data.length;
                totalPages = Math.ceil(totalRecords / PAGE_SIZE);
                
                document.getElementById('totalRegistros').textContent = totalRecords;
                document.getElementById('pageInfo').textContent = `Página ${page} de ${totalPages}`;
                
                // Update pagination buttons
                document.getElementById('btnPrevPage').disabled = page <= 1;
                document.getElementById('btnNextPage').disabled = page >= totalPages;
            } else {
                tbody.innerHTML = `
                    <tr>
                        <td colspan="6" class="text-center text-muted">
                            No se encontraron empleados
                        </td>
                    </tr>
                `;
            }
        } catch (error) {
            showToast('Error al cargar empleados: ' + error.message, 'error');
            console.error('Load error:', error);
        }
    };

    // Render empleados table
    const renderEmpleadosTable = (empleados) => {
        const tbody = document.getElementById('empleadosList');
        
        if (!empleados || empleados.length === 0) {
            tbody.innerHTML = `
                <tr>
                    <td colspan="6" class="text-center text-muted">
                        No se encontraron empleados
                    </td>
                </tr>
            `;
            return;
        }

        tbody.innerHTML = empleados.map(emp => `
            <tr>
                <td>${emp.identificacion || '-'}</td>
                <td>
                    <strong>${emp.primerNombre || ''} ${emp.segundoNombre || ''} ${emp.primerApellido || ''} ${emp.segundoApellido || ''}</strong>
                </td>
                <td>${emp.emailCorporativo || '-'}</td>
                <td>${emp.telefonoNumero || '-'}</td>
                <td>
                    <span class="badge ${emp.estado ? 'bg-success' : 'bg-secondary'}">
                        ${emp.estado ? 'Activo' : 'Inactivo'}
                    </span>
                </td>
                <td>
                    <button class="btn btn-sm btn-info" onclick="EmpleadosModule.editEmpleado(${emp.personaId})" title="Editar">
                        <i class="ri-edit-line"></i>
                    </button>
                    <button class="btn btn-sm btn-danger" onclick="EmpleadosModule.deleteEmpleado(${emp.personaId})" title="Eliminar">
                        <i class="ri-delete-bin-line"></i>
                    </button>
                </td>
            </tr>
        `).join('');
    };

    // Get empleado for edit
    const editEmpleado = async (id) => {
        try {
            const result = await apiCall('GET', `/${id}`);
            
            if (result.success && result.data) {
                const emp = result.data;
                
                // Populate form
                document.getElementById('empleadoId').value = emp.personaId || '';
                document.getElementById('tipoIdentificacion').value = emp.tipoIdentificacion || '';
                document.getElementById('identificacion').value = emp.identificacion || '';
                document.getElementById('primerNombre').value = emp.primerNombre || '';
                document.getElementById('segundoNombre').value = emp.segundoNombre || '';
                document.getElementById('primerApellido').value = emp.primerApellido || '';
                document.getElementById('segundoApellido').value = emp.segundoApellido || '';
                document.getElementById('emailCorporativo').value = emp.emailCorporativo || '';
                document.getElementById('telefonoNumero').value = emp.telefonoNumero || '';
                document.getElementById('celularNumero').value = emp.celularNumero || '';
                document.getElementById('activo').checked = emp.estado || false;
                
                // Update modal title
                document.getElementById('modalEmpleadoTitle').textContent = `Editar Empleado: ${emp.primerNombre} ${emp.primerApellido}`;
                
                // Show modal
                const modal = new bootstrap.Modal(document.getElementById('modalEmpleado'));
                modal.show();
            }
        } catch (error) {
            showToast('Error al cargar empleado: ' + error.message, 'error');
        }
    };

    // Delete empleado
    const deleteEmpleado = async (id) => {
        if (!confirm('¿Está seguro de que desea eliminar este empleado?')) {
            return;
        }

        try {
            const result = await apiCall('DELETE', `/${id}`);
            
            if (result.success) {
                showToast('Empleado eliminado correctamente', 'success');
                loadEmpleados(currentPage);
            } else {
                showToast(result.message || 'Error al eliminar empleado', 'error');
            }
        } catch (error) {
            showToast('Error: ' + error.message, 'error');
        }
    };

    // Save empleado (create or update)
    const saveEmpleado = async (e) => {
        e.preventDefault();

        // Validate form
        const form = document.getElementById('empleadoForm');
        if (!form.checkValidity() === false) {
            e.stopPropagation();
            form.classList.add('was-validated');
            return;
        }

        try {
            const id = document.getElementById('empleadoId').value;
            const data = {
                tipoIdentificacion: document.getElementById('tipoIdentificacion').value,
                identificacion: document.getElementById('identificacion').value,
                primerNombre: document.getElementById('primerNombre').value,
                segundoNombre: document.getElementById('segundoNombre').value,
                primerApellido: document.getElementById('primerApellido').value,
                segundoApellido: document.getElementById('segundoApellido').value,
                emailCorporativo: document.getElementById('emailCorporativo').value,
                telefonoNumero: document.getElementById('telefonoNumero').value || null,
                celularNumero: document.getElementById('celular').value || null,
                estado: document.getElementById('activo').checked
            };

            const method = id ? 'PUT' : 'POST';
            const endpoint = id ? `/${id}` : '';
            
            const result = await apiCall(method, endpoint, data);

            if (result.success) {
                showToast(`Empleado ${id ? 'actualizado' : 'creado'} correctamente`, 'success');
                
                // Close modal
                const modal = bootstrap.Modal.getInstance(document.getElementById('modalEmpleado'));
                modal.hide();
                
                // Reset form
                resetForm();
                
                // Reload list
                loadEmpleados(1);
            } else {
                showToast(result.message || 'Error al guardar', 'error');
            }
        } catch (error) {
            showToast('Error: ' + error.message, 'error');
        }
    };

    // Reset form
    const resetForm = () => {
        const form = document.getElementById('empleadoForm');
        form.reset();
        form.classList.remove('was-validated');
        document.getElementById('empleadoId').value = '';
        document.getElementById('modalEmpleadoTitle').textContent = 'Nuevo Empleado';
    };

    // Apply filters
    const applyFilters = () => {
        currentFilters = {
            identificacion: document.getElementById('filterIdentificacion').value || undefined,
            nombre: document.getElementById('filterNombre').value || undefined,
            estado: document.getElementById('filterEstado').value ? document.getElementById('filterEstado').value === 'true' : undefined
        };
        
        loadEmpleados(1);
    };

    // Clear filters
    const clearFilters = () => {
        document.getElementById('filterIdentificacion').value = '';
        document.getElementById('filterNombre').value = '';
        document.getElementById('filterEstado').value = '';
        currentFilters = {};
        loadEmpleados(1);
    };

    // Initialize
    const init = () => {
        // Button events
        document.getElementById('btnSearch').addEventListener('click', applyFilters);
        document.getElementById('btnLimpiar').addEventListener('click', clearFilters);
        document.getElementById('btnNuevoEmpleado').addEventListener('click', () => {
            resetForm();
            const modal = new bootstrap.Modal(document.getElementById('modalEmpleado'));
            modal.show();
        });

        // Pagination
        document.getElementById('btnPrevPage').addEventListener('click', () => {
            if (currentPage > 1) loadEmpleados(currentPage - 1);
        });
        document.getElementById('btnNextPage').addEventListener('click', () => {
            if (currentPage < totalPages) loadEmpleados(currentPage + 1);
        });

        // Form submit
        document.getElementById('empleadoForm').addEventListener('submit', saveEmpleado);

        // Filter inputs - allow Enter key
        document.getElementById('filterIdentificacion').addEventListener('keypress', (e) => {
            if (e.key === 'Enter') applyFilters();
        });
        document.getElementById('filterNombre').addEventListener('keypress', (e) => {
            if (e.key === 'Enter') applyFilters();
        });

        // Load initial data
        loadEmpleados(1);
    };

    // Public API
    return {
        init,
        loadEmpleados,
        editEmpleado,
        deleteEmpleado,
        applyFilters,
        clearFilters,
        showToast
    };
})();

// Initialize on DOM ready
document.addEventListener('DOMContentLoaded', EmpleadosModule.init);
