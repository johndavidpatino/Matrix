// MatrixNext.Data/DTOs/CORE/TareasPorUnidadDto.cs

using System;

namespace MatrixNext.Data.DTOs.CORE
{
    /// <summary>
    /// DTO para listado de tareas en TraficoTareas (por unidad OP)
    /// Mapea resultado de SP WorkFlow.obtenerTrabajosWorkFlow
    /// Sprint 17 - RE_GT TraficoTareas UI consolidada
    /// </summary>
    public class TareasPorUnidadDto
    {
        /// <summary>ID de la tarea (WorkFlow.Id)</summary>
        public long IdWorkFlow { get; set; }

        /// <summary>ID del trabajo (Trabajo.Id)</summary>
        public long IdTrabajo { get; set; }

        /// <summary>JobBook del trabajo</summary>
        public string JobBook { get; set; } = string.Empty;

        /// <summary>Nombre/descripción del trabajo</summary>
        public string NombreTrabajo { get; set; } = string.Empty;

        /// <summary>Tamaño de muestra</summary>
        public int Muestra { get; set; }

        /// <summary>Tipo de metodología (Cuantitativa, Cualitativa, etc)</summary>
        public string NombreMetodologia { get; set; } = string.Empty;

        /// <summary>Centro de operaciones (CoE)</summary>
        public string NombreCOE { get; set; } = string.Empty;

        /// <summary>Unidad OP (Crítica, Verificación, Captura, Codificación, etc)</summary>
        public string NombreUnidad { get; set; } = string.Empty;

        /// <summary>ID de unidad (5-14)</summary>
        public int IdUnidad { get; set; }

        /// <summary>Estado de la tarea (Creada, EnProgreso, Completada, Anulada)</summary>
        public string Estado { get; set; } = "Creada";

        /// <summary>Prioridad (1=Normal, 2=Alta, 3=Baja)</summary>
        public int Prioridad { get; set; } = 1;

        /// <summary>Fecha de vencimiento</summary>
        public DateTime? FechaVencimiento { get; set; }

        /// <summary>Cantidad de usuarios asignados a esta tarea</summary>
        public int UsuariosAsignados { get; set; }

        /// <summary>Indicador: ¿Es proyecto cualitativo? (oculta btnFichaCuanti)</summary>
        public bool EsProyectoCualitativo { get; set; }

        /// <summary>Display humanizado del estado</summary>
        public string EstadoDisplay => Estado switch
        {
            "Creada" => "Creada",
            "EnProgreso" => "En Progreso",
            "Completada" => "Completada",
            "Anulada" => "Anulada",
            _ => Estado
        };

        /// <summary>Display humanizado de prioridad</summary>
        public string PrioridadDisplay => Prioridad switch
        {
            1 => "Normal",
            2 => "Alta",
            3 => "Baja",
            _ => "Desconocida"
        };

        /// <summary>Clase CSS para badge de prioridad</summary>
        public string PrioridadCssClass => Prioridad switch
        {
            2 => "danger",    // Alta
            1 => "secondary", // Normal
            3 => "success",   // Baja
            _ => "light"
        };

        /// <summary>Clase CSS para badge de estado</summary>
        public string EstadoCssClass => Estado switch
        {
            "Creada" => "secondary",
            "EnProgreso" => "warning",
            "Completada" => "success",
            "Anulada" => "danger",
            _ => "light"
        };

        /// <summary>¿Está vencida o próxima a vencer?</summary>
        public bool EsUrgente
        {
            get
            {
                if (FechaVencimiento == null) return false;
                var diasAlerta = 3;
                var diasHastaVencer = (FechaVencimiento.Value - DateTime.Now).Days;
                return diasHastaVencer <= diasAlerta && diasHastaVencer >= 0;
            }
        }

        /// <summary>¿Está vencida?</summary>
        public bool EsVencida => FechaVencimiento.HasValue && FechaVencimiento.Value < DateTime.Now;
    }
}
