const CatalogosModule = (() => {
    const init = async () => {
        await loadAllCatalogos();
    };

    const loadAllCatalogos = async () => {
        const endpoints = [
            { api: '/api/empleados/areas', tbody: 'areasTableBody', cols: (d) => `<tr><td>${d.id}</td><td>${d.nombre}</td></tr>` },
            { api: '/api/empleados/cargos', tbody: 'cargosTableBody', cols: (d) => `<tr><td>${d.id}</td><td>${d.nombre}</td><td>${d.descripcion || ''}</td></tr>` },
            { api: '/api/empleados/bandas', tbody: 'bandasTableBody', cols: (d) => `<tr><td>${d.id}</td><td>${d.nombre}</td><td>${d.salarioBase || ''}</td></tr>` },
            { api: '/api/empleados/estadosciviles', tbody: 'estadosTableBody', cols: (d) => `<tr><td>${d.id}</td><td>${d.nombre}</td></tr>` },
            { api: '/api/empleados/grupossanguineos', tbody: 'gruposTableBody', cols: (d) => `<tr><td>${d.id}</td><td>${d.tipo}</td></tr>` },
            { api: '/api/empleados/sedes', tbody: 'sedesTableBody', cols: (d) => `<tr><td>${d.id}</td><td>${d.nombre}</td><td>${d.ciudad || ''}</td></tr>` },
            { api: '/api/empleados/tiposcontrato', tbody: 'tiposTableBody', cols: (d) => `<tr><td>${d.id}</td><td>${d.nombre}</td></tr>` },
            { api: '/api/empleados/tiemposcontrato', tbody: 'tiemposTableBody', cols: (d) => `<tr><td>${d.id}</td><td>${d.nombre}</td><td>${d.meses || ''}</td></tr>` },
            { api: '/api/empleados/empresas', tbody: 'empresasTableBody', cols: (d) => `<tr><td>${d.id}</td><td>${d.nombre}</td><td>${d.nit || ''}</td></tr>` }
        ];

        for (const endpoint of endpoints) {
            await loadCatalogo(endpoint.api, endpoint.tbody, endpoint.cols);
        }
    };

    const loadCatalogo = async (api, tbodyId, colsRenderer) => {
        try {
            const response = await apiCall('GET', api, null);
            const tbody = document.getElementById(tbodyId);
            if (response.success && response.data && response.data.length > 0) {
                tbody.innerHTML = response.data.map(colsRenderer).join('');
            } else {
                tbody.innerHTML = '<tr><td colspan="3" class="text-center py-2 text-muted">Sin datos</td></tr>';
            }
        } catch (error) {
            console.error(`Error loading ${api}:`, error);
            showToast(`Error cargando catálogo`, 'error');
        }
    };

    const showToast = (message, type = 'error') => {
        const messageId = type === 'error' ? 'errorToastMessage' : 'errorToastMessage';
        document.getElementById(messageId).textContent = message;
        const toast = new bootstrap.Toast(document.getElementById('errorToast'));
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
        init
    };
})();
