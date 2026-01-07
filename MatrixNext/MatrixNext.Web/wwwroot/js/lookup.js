// Lightweight AJAX lookup for selects with data-lookup-url
(function(){
  document.addEventListener('DOMContentLoaded', () => {
    // Wire lookups on modal open
    document.getElementById('appModal')?.addEventListener('shown.bs.modal', initLookups);
    
    function initLookups() {
      document.querySelectorAll('.lookup-trabajo, .lookup-tarea').forEach(select => {
        if (select.dataset.lookupInit) return; // Already initialized
        select.dataset.lookupInit = 'true';
        
        const url = select.dataset.lookupUrl;
        if (!url) return;
        
        // Simple search on input (type-ahead)
        let timeout;
        select.addEventListener('input', function(ev) {
          clearTimeout(timeout);
          const q = ev.target.value || '';
          timeout = setTimeout(() => loadOptions(select, url, q), 300);
        });
        
        // Focus: load initial options
        select.addEventListener('focus', function() {
          if (select.options.length <= 1) {
            loadOptions(select, url, '');
          }
        });
      });
    }
    
    async function loadOptions(select, url, q) {
      try {
        const res = await fetch(`${url}?q=${encodeURIComponent(q)}&limit=20`);
        const items = await res.json();
        
        const currentVal = select.value;
        select.innerHTML = '<option value="">-- Seleccionar --</option>';
        
        items.forEach(item => {
          const opt = document.createElement('option');
          opt.value = item.id;
          opt.textContent = item.text;
          if (item.id == currentVal) opt.selected = true;
          select.appendChild(opt);
        });
      } catch {
        // Silent fail; keep current options
      }
    }
  });
})();
