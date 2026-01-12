/**
 * Módulo de Filtros Avanzados para OP_Cualitativo
 * Soporta: Autocomplete, Date ranges, Multi-select
 */

const OpAdvancedFiltersModule = (() => {
    'use strict';

    const API_BASE = '/api/OP/filters';

    // ========== AUTOCOMPLETE SETUP ==========
    const setupAutocomplete = (inputElementId, apiEndpoint, onSelect) => {
        const inputElement = document.getElementById(inputElementId);
        if (!inputElement) return;

        let currentResults = [];

        inputElement.addEventListener('input', async (e) => {
            const searchText = e.target.value.trim();

            if (searchText.length < 2) {
                clearAutocompleteDropdown(inputElementId);
                return;
            }

            try {
                const response = await fetch(`${API_BASE}/${apiEndpoint}?search=${encodeURIComponent(searchText)}`);
                const result = await response.json();

                if (result.success) {
                    currentResults = result.data;
                    showAutocompleteDropdown(inputElementId, result.data, onSelect);
                }
            } catch (error) {
                console.error('Autocomplete error:', error);
            }
        });

        // Cerrar dropdown al perder foco
        inputElement.addEventListener('blur', () => {
            setTimeout(() => clearAutocompleteDropdown(inputElementId), 200);
        });
    };

    const showAutocompleteDropdown = (inputElementId, items, onSelect) => {
        let dropdown = document.getElementById(`${inputElementId}-dropdown`);

        if (!dropdown) {
            dropdown = document.createElement('div');
            dropdown.id = `${inputElementId}-dropdown`;
            dropdown.className = 'autocomplete-dropdown position-absolute bg-white border rounded shadow-sm';
            dropdown.style.cssText = 'width: 100%; max-height: 300px; overflow-y: auto; z-index: 1000;';
            
            const inputElement = document.getElementById(inputElementId);
            inputElement.parentElement.style.position = 'relative';
            inputElement.parentElement.appendChild(dropdown);
        }

        dropdown.innerHTML = items.map((item, index) => `
            <div class="autocomplete-item p-2 border-bottom cursor-pointer" 
                 data-index="${index}" 
                 style="cursor: pointer;">
                <strong>${item.codigo || item.nombre}</strong>
                <small class="d-block text-muted">${item.descripcion || item.email || ''}</small>
            </div>
        `).join('');

        dropdown.style.display = 'block';

        dropdown.querySelectorAll('.autocomplete-item').forEach(item => {
            item.addEventListener('click', () => {
                const index = parseInt(item.dataset.index);
                onSelect(items[index]);
                clearAutocompleteDropdown(inputElementId);
            });
        });
    };

    const clearAutocompleteDropdown = (inputElementId) => {
        const dropdown = document.getElementById(`${inputElementId}-dropdown`);
        if (dropdown) {
            dropdown.style.display = 'none';
        }
    };

    // ========== DATE RANGE FILTER ==========
    const setupDateRangeFilter = async (
        fechaDesdeInputId,
        fechaHastaInputId,
        estadoSelectId,
        tableBodyId,
        apiEndpoint,
        renderFunction) => {
        
        const applyButton = document.createElement('button');
        applyButton.className = 'btn btn-sm btn-primary';
        applyButton.textContent = 'Aplicar Filtro por Fechas';
        applyButton.addEventListener('click', async () => {
            await applyDateRangeFilter(
                fechaDesdeInputId,
                fechaHastaInputId,
                estadoSelectId,
                apiEndpoint,
                tableBodyId,
                renderFunction
            );
        });

        return applyButton;
    };

    const applyDateRangeFilter = async (
        fechaDesdeInputId,
        fechaHastaInputId,
        estadoSelectId,
        apiEndpoint,
        tableBodyId,
        renderFunction) => {
        
        const fechaDesde = document.getElementById(fechaDesdeInputId)?.value;
        const fechaHasta = document.getElementById(fechaHastaInputId)?.value;
        const estado = document.getElementById(estadoSelectId)?.value;

        if (!fechaDesde || !fechaHasta) {
            alert('Por favor selecciona ambas fechas');
            return;
        }

        try {
            const params = new URLSearchParams({
                fechaDesde,
                fechaHasta,
                ...(estado && { estado })
            });

            const response = await fetch(`${API_BASE}/${apiEndpoint}?${params}`);
            const result = await response.json();

            if (result.success) {
                const tbody = document.getElementById(tableBodyId);
                tbody.innerHTML = '';
                
                if (result.data.data.length === 0) {
                    tbody.innerHTML = '<tr><td colspan="100%" class="text-center text-muted">No hay resultados</td></tr>';
                    return;
                }

                result.data.data.forEach(item => {
                    const row = renderFunction(item);
                    tbody.appendChild(row);
                });
            } else {
                alert('Error al aplicar filtro');
            }
        } catch (error) {
            console.error('Error applying date range filter:', error);
            alert('Error al aplicar filtro');
        }
    };

    // ========== MULTI-SELECT FILTER ==========
    const setupMultiSelectFilter = async (
        selectElementId,
        tableBodyId,
        apiEndpoint,
        renderFunction) => {
        
        const selectElement = document.getElementById(selectElementId);
        if (!selectElement) return;

        // Cargar estados disponibles
        try {
            const response = await fetch(`${API_BASE}/estados`);
            const result = await response.json();

            if (result.success) {
                selectElement.innerHTML = result.data.map(estado => `
                    <option value="${estado.codigo}">
                        ${estado.nombre} (${estado.cantidad})
                    </option>
                `).join('');

                // Permitir multi-select con Ctrl+Click
                selectElement.setAttribute('multiple', 'multiple');
                selectElement.style.height = '120px';
            }
        } catch (error) {
            console.error('Error loading states:', error);
        }
    };

    const applyMultiSelectFilter = async (
        selectElementId,
        tableBodyId,
        apiEndpoint,
        renderFunction,
        fechaDesdeInputId = null,
        fechaHastaInputId = null) => {
        
        const selectElement = document.getElementById(selectElementId);
        const selectedOptions = Array.from(selectElement.selectedOptions).map(opt => opt.value);

        if (selectedOptions.length === 0) {
            alert('Por favor selecciona al menos un estado');
            return;
        }

        try {
            const payload = {
                estados: selectedOptions,
                pageNumber: 1,
                pageSize: 50
            };

            // Agregar fechas si están disponibles
            if (fechaDesdeInputId && fechaHastaInputId) {
                const fechaDesde = document.getElementById(fechaDesdeInputId)?.value;
                const fechaHasta = document.getElementById(fechaHastaInputId)?.value;
                if (fechaDesde) payload.fechaDesde = fechaDesde;
                if (fechaHasta) payload.fechaHasta = fechaHasta;
            }

            const response = await fetch(`${API_BASE}/${apiEndpoint}`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(payload)
            });

            const result = await response.json();

            if (result.success) {
                const tbody = document.getElementById(tableBodyId);
                tbody.innerHTML = '';
                
                if (result.data.data.length === 0) {
                    tbody.innerHTML = '<tr><td colspan="100%" class="text-center text-muted">No hay resultados</td></tr>';
                    return;
                }

                result.data.data.forEach(item => {
                    const row = renderFunction(item);
                    tbody.appendChild(row);
                });

                // Mostrar información de paginación
                console.log(`Mostrando ${result.data.data.length} de ${result.data.totalRecords} registros`);
            } else {
                alert('Error al aplicar filtro');
            }
        } catch (error) {
            console.error('Error applying multi-select filter:', error);
            alert('Error al aplicar filtro');
        }
    };

    // ========== API PÚBLICA ==========
    return {
        setupAutocomplete,
        setupDateRangeFilter,
        applyDateRangeFilter,
        setupMultiSelectFilter,
        applyMultiSelectFilter
    };
})();
