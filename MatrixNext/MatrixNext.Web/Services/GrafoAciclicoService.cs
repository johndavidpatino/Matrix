namespace MatrixNext.Web.Services
{
    /// <summary>
    /// Validador de grafos acíclicos (para precedencias CORE)
    /// Ref: MATRIZ_PERMISOS_ROLES.md § 5.2 (algoritmo DFS)
    /// Ref: MAPA_DEPENDENCIAS_PY_CORE.md § 2 (ciclos sospechosos)
    /// </summary>
    public class GrafoAciclicoService
    {
        private readonly ILogger<GrafoAciclicoService> _logger;

        public GrafoAciclicoService(ILogger<GrafoAciclicoService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Valida que no existan ciclos en las precedencias de tareas
        /// Usa algoritmo DFS (Depth-First Search)
        /// Retorna true si el grafo es acíclico (sin ciclos)
        /// </summary>
        public bool ValidarNoCiclos<T>(
            List<T> items,
            Func<T, long> getId,
            Func<T, long?> getIdPrevia
        )
        {
            try
            {
                var grafo = new Dictionary<long, List<long>>();

                // Construir grafo: id → lista de ids que dependen de ella
                foreach (var item in items)
                {
                    var id = getId(item);
                    var idPrevia = getIdPrevia(item);

                    if (!grafo.ContainsKey(id))
                        grafo[id] = new List<long>();

                    if (idPrevia.HasValue)
                        grafo[id].Add(idPrevia.Value);
                }

                // Detectar ciclos usando DFS
                var visitados = new HashSet<long>();
                var recursionStack = new HashSet<long>();

                foreach (var nodo in grafo.Keys)
                {
                    if (!visitados.Contains(nodo))
                    {
                        if (DetectarCiclo(nodo, grafo, visitados, recursionStack))
                        {
                            _logger.LogWarning($"Ciclo detectado en nodo {nodo}");
                            return false; // Ciclo encontrado
                        }
                    }
                }

                _logger.LogInformation("Validación de ciclos completada: Sin ciclos encontrados");
                return true; // Sin ciclos
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validando ciclos");
                return false;
            }
        }

        /// <summary>
        /// Algoritmo DFS recursivo para detectar ciclos
        /// </summary>
        private bool DetectarCiclo(
            long nodo,
            Dictionary<long, List<long>> grafo,
            HashSet<long> visitados,
            HashSet<long> recursionStack
        )
        {
            visitados.Add(nodo);
            recursionStack.Add(nodo);

            if (grafo.ContainsKey(nodo))
            {
                var vecinos = grafo[nodo];
                foreach (var vecino in vecinos)
                {
                    if (!visitados.Contains(vecino))
                    {
                        if (DetectarCiclo(vecino, grafo, visitados, recursionStack))
                            return true;
                    }
                    else if (recursionStack.Contains(vecino))
                    {
                        // Ciclo detectado: vecino ya está en la pila de recursión
                        _logger.LogWarning($"Ciclo detectado: {nodo} → {vecino}");
                        return true;
                    }
                }
            }

            recursionStack.Remove(nodo);
            return false;
        }

        /// <summary>
        /// Valida si una transición de estado es permitida según las precedencias
        /// Retorna true si todas las tareas previas están completadas
        /// </summary>
        public bool PermiteTransicion(
            long idTarea,
            List<(long IdTarea, long? IdTareaPreviaRequerida, string EstadoTareaPrevia)> precedencias
        )
        {
            try
            {
                var treasPrevias = precedencias
                    .Where(p => p.IdTarea == idTarea && p.IdTareaPreviaRequerida.HasValue)
                    .ToList();

                foreach (var tarea in treasPrevias)
                {
                    // Si tarea previa no está completada, no permite transición
                    if (tarea.EstadoTareaPrevia != "Completada")
                    {
                        _logger.LogWarning($"Transición bloqueada: Tarea {idTarea} tiene previa no completada");
                        return false;
                    }
                }

                _logger.LogInformation($"Transición permitida para tarea {idTarea}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error validando transición para tarea {idTarea}");
                return false;
            }
        }

        /// <summary>
        /// Obtiene todas las tareas que deben completarse antes de una tarea específica
        /// </summary>
        public List<long> ObtenerTareasPrevias(
            long idTarea,
            List<(long IdTarea, long? IdTareaPreviaRequerida)> precedencias
        )
        {
            var resultado = new List<long>();
            var visitados = new HashSet<long>();

            void RecorrerPrevias(long id)
            {
                if (visitados.Contains(id))
                    return;

                visitados.Add(id);

                var previas = precedencias
                    .Where(p => p.IdTarea == id && p.IdTareaPreviaRequerida.HasValue)
                    .Select(p => p.IdTareaPreviaRequerida!.Value)
                    .ToList();

                foreach (var previa in previas)
                {
                    resultado.Add(previa);
                    RecorrerPrevias(previa);
                }
            }

            RecorrerPrevias(idTarea);
            return resultado.Distinct().ToList();
        }
    }
}
