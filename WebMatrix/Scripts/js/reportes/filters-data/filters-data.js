//1 = Total, 3 = Unidad
export const filtersGroup = {
    "1": 3,
    "8": 1,
    "12": 3,
    "13": 1,
    "78": 1,
    "16": 3,
    "20": 1,
    "76": 1,
    "23": 3,
    "65": 3,
    "66": 1
}



export const unidades = [
    { idUnidad: 1,NombreUnidad: "Connect"},
    { idUnidad: 2,NombreUnidad: "MSU"},
    { idUnidad: 3,NombreUnidad: "CEX"},
    { idUnidad: 4,NombreUnidad: "Public Affairs"},
    { idUnidad: 5,NombreUnidad: "Media"},
    { idUnidad: 6,NombreUnidad: "IUU"},
    { idUnidad: 7,NombreUnidad: "Gerencia OP Connect"},
    { idUnidad: 8,NombreUnidad: "Gerencia OP Marketing"},
    { idUnidad: 9,NombreUnidad: "Gerencia OP LO-PA-MD"},
    { idUnidad: 10,NombreUnidad: "Gerencia Telefonico"},
    { idUnidad: 11,NombreUnidad: "Gerencia Tecnica"},
    { idUnidad: 12,NombreUnidad: "Estadistica"},
    { idUnidad: 13,NombreUnidad: "Talento Humano"},
    { idUnidad: 14,NombreUnidad: "Compras y Outsourcing"},
    { idUnidad: 15,NombreUnidad: "Tecnologia"},
    { idUnidad: 16,NombreUnidad: "Gestión de Calidad"},
    { idUnidad: 17,NombreUnidad: "Finanzas"},
    { idUnidad: 18,NombreUnidad: "Gerencia General"},
    { idUnidad: 19,NombreUnidad: "Campo Cuantitativo"},
    { idUnidad: 20,NombreUnidad: "Gerencia Cualitativa"},
    { idUnidad: 21,NombreUnidad: "Gerencia Mystery Shopper"},
    { idUnidad: 22,NombreUnidad: "Gerencia Online"},
    { idUnidad: 23,NombreUnidad: "P&G - MK"},
    { idUnidad: 24,NombreUnidad: "Ecuador OPS"},
    { idUnidad: 25,NombreUnidad: "Ecuador"},
    { idUnidad: 26,NombreUnidad: "Operations"},
    { idUnidad: 27,NombreUnidad: "Gerencia OP Especializados"},
    { idUnidad: 28,NombreUnidad: "Administracion"},
    { idUnidad: 29,NombreUnidad: "BHT"},
    { idUnidad: 30,NombreUnidad: "Innovation"},
    { idUnidad: 31,NombreUnidad: "Creative Excellence"},
    { idUnidad: 32,NombreUnidad: "Mystery Shopping"},
    { idUnidad: 33,NombreUnidad: "HealthCare"},
    { idUnidad: 34,NombreUnidad: "Corporate Reputation"},
    { idUnidad: 35,NombreUnidad: "Observer"},
    { idUnidad: 36,NombreUnidad: "Strategy 3"},
    { idUnidad: 37,NombreUnidad: "IIS RAES"},
];

export const tasks = [
    { IdTarea: 1, NombreTarea: "Instrumentos" },
    { IdTarea: 8, NombreTarea: "Codificación" },
    { IdTarea: 12, NombreTarea: "PDC" },
    { IdTarea: 13, NombreTarea: "Procesamiento Total" },
    { IdTarea: 78, NombreTarea: "Procesamiento - Control Interno" },
    { IdTarea: 16, NombreTarea: "Elaboración Informe" },
    { IdTarea: 20, NombreTarea: "Scripting" },
    { IdTarea: 76, NombreTarea: "Scripting - Control Interno" },
    { IdTarea: 23, NombreTarea: "Estadistica-Metodología" },
    { IdTarea: 65, NombreTarea: "Evaluación variables de control Proyectos" },
    { IdTarea: 66, NombreTarea: "Evaluación variables de control OMP" },
    { IdTarea: 93, NombreTarea: "Data Visualization - Plan Graficación" },
    { IdTarea: 94, NombreTarea: "Data Visualization - Informes" }
]

export const instruments = [
    { IdInstrumento: 9, NombreInstrumento: "Cuestionario" },
    { IdInstrumento: 10, NombreInstrumento: "Instructivo" },
    { IdInstrumento: 11, NombreInstrumento: "Metodología" },
    { IdInstrumento: 12, NombreInstrumento: "Tarjetas" },
    { IdInstrumento: 13, NombreInstrumento: "Circular" },
    { IdInstrumento: 70, NombreInstrumento: "Tracking-Archivo de cambios" },
    { IdInstrumento: 80, NombreInstrumento: "Guion de verificación" }
]


export const indicadores = {
    REGISTRO_OBSERVACIONES: "registro de observaciones",
    CUMPLIMIENTO_TAREAS: "cumplimiento de tareas"
}


export const tasksOptions = [
    { value: 1, label: "Instrumentos" },
    { value: 8, label: "Codificación" },
    { value: 12, label: "PDC" },
    { value: 13, label: "Procesamiento Total" },
    { value: 78, label: "Procesamiento - Control Interno" },
    { value: 16, label: "Elaboración Informe" },
    { value: 20, label: "Scripting" },
    { value: 76, label: "Scripting - Control Interno" },
    { value: 23, label: "Estadistica-Metodología" },
    { value: 65, label: "Evaluación variables de control Proyectos" },
    { value: 66, label: "Evaluación variables de control OMP" }
]

export const instrumentsOptions = [
    { value: 9, label: "Cuestionario" },
    { value: 10, label: "Instructivo" },
    { value: 11, label: "Metodología" },
    { value: 12, label: "Tarjetas" },
    { value: 13, label: "Circular" },
    { value: 70, label: "Tracking-Archivo de cambios" },
    { value: 80, label: "Guion de verificación" }
]

export const trimestreOptions = [
    { value: "Ene-Mar", label: "Ene-Mar" },
    { value: "Abr-Jun", label: "Abr-Jun" },
    { value: "Jul-Sep", label: "Jul-Sep" },
    { value: "Oct-Dic", label: "Oct-Dic" }
]

export const groupOptions = [
    { value: 1, label: "Total" },
    { value: 3, label: "Unidad" },
    { value: "PersonaAsignada", label: "Persona Asignada" }
]

export const groupOptionsCumplimientoTareas = [
    { value: 1, label: "Proceso" },
    { value: 3, label: "Tarea" },
    { value: "PersonaAsignada", label: "Persona Asignada" }
]