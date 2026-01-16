// MatrixNext.Web/Services/RE_GT/RecoleccionDatosService.cs

using MatrixNext.Core.DTOs.RE_GT;
using Microsoft.Extensions.Logging;

namespace MatrixNext.Web.Services.RE_GT
{
    /// <summary>
    /// Servicio para manejo de menús y navegación de Recolección y Gestión de Datos
    /// Sprint 17 - Fase 3
    /// </summary>
    public class RecoleccionDatosService : IRecoleccionDatosService
    {
        private readonly ILogger<RecoleccionDatosService> _logger;

        public RecoleccionDatosService(ILogger<RecoleccionDatosService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Obtiene menú de Recolección de Datos con dos secciones:
        /// 1. Gerencia de Operaciones
        /// 2. Subdirección Operativa
        /// </summary>
        public async Task<RecoleccionDatosMenuDto> ObtenerMenuRecoleccionAsync()
        {
            try
            {
                _logger.LogInformation("[RecoleccionDatos] Obteniendo menú");

                var menu = new RecoleccionDatosMenuDto();

                // Sección 1: Gerencia de Operaciones
                var seccionGerencia = new MenuSeccionDto
                {
                    Nombre = "Gerencia de Operaciones",
                    Descripcion = "Operaciones y gestión general",
                    IconoCss = "fas fa-cogs",
                    Orden = 1,
                    Items = new List<MenuItemDto>
                    {
                        new MenuItemDto { Id = 1, Titulo = "Asignar OMP", Url = "/RE_GT/Recoleccion/AsignacionCOE", Descripcion = "Asignación de coordinación de estudios", IconoCss = "fas fa-user-tie", Orden = 1, Seccion = "Gerencia" },
                        new MenuItemDto { Id = 2, Titulo = "Asignar JBI", Url = "/RE_GT/Recoleccion/AsignacionJBI", Descripcion = "Asignación de JobBook Interno a proyectos", IconoCss = "fas fa-book", Orden = 2, Seccion = "Gerencia" },
                        new MenuItemDto { Id = 3, Titulo = "Revisar Presupuestos", Url = "/CAP/RevisionPresupuestos", Descripcion = "Revisión de presupuestos", IconoCss = "fas fa-file-invoice-dollar", Orden = 3, Seccion = "Gerencia", Habilitado = true },
                        new MenuItemDto { Id = 4, Titulo = "Ajustar Costos", Url = "/CAP/PresupuestosAprobados?opt=2", Descripcion = "Ajuste de costos", IconoCss = "fas fa-sliders-h", Orden = 4, Seccion = "Gerencia", Habilitado = true },
                        new MenuItemDto { Id = 5, Titulo = "Trabajos Atrasados", Url = "/RP_Reportes/TrabajosConAtraso", Descripcion = "Trabajos con retraso", IconoCss = "fas fa-clock", Orden = 5, Seccion = "Gerencia" },
                        new MenuItemDto { Id = 6, Titulo = "Seguimiento", Url = "/RP_Reportes/TrabajosPorGerencia", Descripcion = "Seguimiento de trabajos", IconoCss = "fas fa-eye", Orden = 6, Seccion = "Gerencia" },
                        new MenuItemDto { Id = 7, Titulo = "Planeación Tráfico", Url = "/RP_Reportes/PlaneacionOperaciones", Descripcion = "Planeación de operaciones", IconoCss = "fas fa-project-diagram", Orden = 7, Seccion = "Gerencia" },
                        new MenuItemDto { Id = 8, Titulo = "Producción", Url = "/MBO/CampoProduccion", Descripcion = "Reporte de producción campo", IconoCss = "fas fa-chart-line", Orden = 8, Seccion = "Gerencia" },
                        new MenuItemDto { Id = 9, Titulo = "Calidad Campo", Url = "/MBO/CampoCalidadTotal", Descripcion = "Métricas de calidad en campo", IconoCss = "fas fa-star", Orden = 9, Seccion = "Gerencia" },
                        new MenuItemDto { Id = 10, Titulo = "Ficha Encuestador", Url = "/TH/ListadoEncuestadores", Descripcion = "Gestión de encuestadores", IconoCss = "fas fa-user-check", Orden = 10, Seccion = "Gerencia" },
                        new MenuItemDto { Id = 11, Titulo = "Tiempos Revisión", Url = "/RP_Reportes/InformeTiemposRevisionPresupuestos", Descripcion = "Tiempos de revisión", IconoCss = "fas fa-hourglass-end", Orden = 11, Seccion = "Gerencia" },
                        new MenuItemDto { Id = 12, Titulo = "Cambios JBI", Url = "/RE_GT/Recoleccion/CambiosJBI", Descripcion = "Gestión de cambios en JBI", IconoCss = "fas fa-exchange-alt", Orden = 12, Seccion = "Gerencia" }
                    }
                };

                // Sección 2: Subdirección Operativa
                var seccionSubdir = new MenuSeccionDto
                {
                    Nombre = "Subdirección Operativa",
                    Descripcion = "Planeación operativa",
                    IconoCss = "fas fa-layer-group",
                    Orden = 2,
                    Items = new List<MenuItemDto>
                    {
                        new MenuItemDto { Id = 20, Titulo = "Planeación General", Url = "/RP_Reportes/PlaneacionOperaciones", Descripcion = "Planeación operaciones general", IconoCss = "fas fa-map-signs", Orden = 1, Seccion = "Subdir" },
                        new MenuItemDto { Id = 21, Titulo = "Planeación Campo", Url = "/RP_Reportes/PlaneacionCampo", Descripcion = "Planeación estimada campo", IconoCss = "fas fa-map", Orden = 2, Seccion = "Subdir" },
                        new MenuItemDto { Id = 22, Titulo = "Planeación Propuestas", Url = "/RP_Reportes/PlaneacionPropuestas", Descripcion = "Planeación de propuestas", IconoCss = "fas fa-handshake", Orden = 3, Seccion = "Subdir" },
                        new MenuItemDto { Id = 23, Titulo = "Planeación Estudios", Url = "/RP_Reportes/PlaneacionEstudios", Descripcion = "Planeación de estudios", IconoCss = "fas fa-graduation-cap", Orden = 4, Seccion = "Subdir" }
                    }
                };

                menu.Secciones.Add(seccionGerencia);
                menu.Secciones.Add(seccionSubdir);

                _logger.LogInformation("[RecoleccionDatos] Menú obtenido: {Secciones} secciones, {Items} items", menu.Secciones.Count, menu.Secciones.Sum(s => s.Items.Count));

                return await Task.FromResult(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[RecoleccionDatos] Error al obtener menú");
                return new RecoleccionDatosMenuDto { TieneAcceso = false };
            }
        }

        /// <summary>
        /// Obtiene menú de Gestión y Tratamiento con 4 secciones:
        /// 1. Operaciones Cualitativas
        /// 2. Operaciones Cuantitativas
        /// 3. Subdirección de Calidad
        /// 4. Subdirección Tratamiento
        /// </summary>
        public async Task<GestionTratamientoDatosMenuDto> ObtenerMenuGestionTratamientoAsync()
        {
            try
            {
                _logger.LogInformation("[GestionTratamiento] Obteniendo menú");

                var menu = new GestionTratamientoDatosMenuDto();

                // Sección 1: Operaciones Cualitativas
                var seccionCuali = new MenuSeccionDto
                {
                    Nombre = "Operaciones Cualitativas",
                    Descripcion = "Estudios de naturaleza cualitativa",
                    IconoCss = "fas fa-comments",
                    Orden = 1,
                    Items = new List<MenuItemDto>
                    {
                        new MenuItemDto { Id = 1, Titulo = "Ir a Operaciones Cualitativas", Url = "/OP_Cualitativo/Home/Gestion", Descripcion = "Acceso a módulo cualitativo", IconoCss = "fas fa-sign-in-alt", Orden = 1, Seccion = "Cuali" }
                    }
                };

                // Sección 2: Operaciones Cuantitativas
                var seccionCuanti = new MenuSeccionDto
                {
                    Nombre = "Operaciones Cuantitativas",
                    Descripcion = "Estudios de naturaleza cuantitativa",
                    IconoCss = "fas fa-chart-bar",
                    Orden = 2,
                    Items = new List<MenuItemDto>
                    {
                        new MenuItemDto { Id = 2, Titulo = "Ir a Operaciones Cuantitativas", Url = "/OP_Cuantitativo/Home/Gestion", Descripcion = "Acceso a módulo cuantitativo", IconoCss = "fas fa-sign-in-alt", Orden = 1, Seccion = "Cuanti" }
                    }
                };

                // Sección 3: Subdirección de Calidad
                var seccionCalidad = new MenuSeccionDto
                {
                    Nombre = "Subdirección de Calidad",
                    Descripcion = "Gestión de calidad operacional",
                    IconoCss = "fas fa-check-double",
                    Orden = 3,
                    Items = new List<MenuItemDto>
                    {
                        new MenuItemDto { Id = 3, Titulo = "Informe Anulación", Url = "/RP_Reportes/InformeAnulacion", Descripcion = "Informe de anulaciones", IconoCss = "fas fa-ban", Orden = 1, Seccion = "Calidad" },
                        new MenuItemDto { Id = 4, Titulo = "Desanulación", Url = "/OP_Cuantitativo/ConsultaTrabajos", Descripcion = "Desanulación de encuestas", IconoCss = "fas fa-undo", Orden = 2, Seccion = "Calidad" },
                        new MenuItemDto { Id = 5, Titulo = "Errores Campo", Url = "/RP_Reportes/ErroresDecampo", Descripcion = "Errores detectados en campo", IconoCss = "fas fa-exclamation-triangle", Orden = 3, Seccion = "Calidad" },
                        new MenuItemDto { Id = 6, Titulo = "Tráfico Encuestas", Url = "/RP_Reportes/TraficoAreasGeneral", Descripcion = "Tráfico general de encuestas", IconoCss = "fas fa-traffic-light", Orden = 4, Seccion = "Calidad" },
                        new MenuItemDto { Id = 7, Titulo = "Planeación", Url = "/RP_Reportes/PlaneacionOperaciones", Descripcion = "Planeación operativa", IconoCss = "fas fa-calendar-alt", Orden = 5, Seccion = "Calidad" },
                        new MenuItemDto { Id = 8, Titulo = "Seguimiento Tareas", Url = "/CORE/WorkFlow/TraficoTareas", Descripcion = "Tráfico de tareas y seguimiento", IconoCss = "fas fa-tasks", Orden = 6, Seccion = "Calidad" },
                        new MenuItemDto { Id = 9, Titulo = "Presupuestos", Url = "/CAP/PresupuestosAprobados?opt=1", Descripcion = "Gestión de presupuestos", IconoCss = "fas fa-file-invoice-dollar", Orden = 7, Seccion = "Calidad" },
                        new MenuItemDto { Id = 10, Titulo = "Simulador Costos", Url = "/CAP/SimuladorCostosOperaciones", Descripcion = "Simulador de costos", IconoCss = "fas fa-calculator", Orden = 8, Seccion = "Calidad" }
                    }
                };

                // Sección 4: Subdirección Tratamiento
                var seccionTratamiento = new MenuSeccionDto
                {
                    Nombre = "Subdirección Tratamiento",
                    Descripcion = "Gestión de tratamiento de datos",
                    IconoCss = "fas fa-flask",
                    Orden = 4,
                    Items = new List<MenuItemDto>
                    {
                        new MenuItemDto { Id = 11, Titulo = "Planeación", Url = "/RP_Reportes/PlaneacionOperaciones", Descripcion = "Planeación operativa", IconoCss = "fas fa-calendar-alt", Orden = 1, Seccion = "Tratamiento" },
                        new MenuItemDto { Id = 12, Titulo = "Presupuestos", Url = "/CAP/PresupuestosAprobados?opt=1", Descripcion = "Gestión de presupuestos", IconoCss = "fas fa-file-invoice-dollar", Orden = 2, Seccion = "Tratamiento" },
                        new MenuItemDto { Id = 13, Titulo = "Simulador Costos", Url = "/CAP/SimuladorCostosOperaciones", Descripcion = "Simulador de costos", IconoCss = "fas fa-calculator", Orden = 3, Seccion = "Tratamiento" }
                    }
                };

                menu.Secciones.Add(seccionCuali);
                menu.Secciones.Add(seccionCuanti);
                menu.Secciones.Add(seccionCalidad);
                menu.Secciones.Add(seccionTratamiento);

                _logger.LogInformation("[GestionTratamiento] Menú obtenido: {Secciones} secciones, {Items} items", menu.Secciones.Count, menu.Secciones.Sum(s => s.Items.Count));

                return await Task.FromResult(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[GestionTratamiento] Error al obtener menú");
                return new GestionTratamientoDatosMenuDto { TieneAcceso = false };
            }
        }
    }
}
