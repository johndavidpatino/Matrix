# API Reference - TH Empleados Module

## Base URLs
- **Empleados:** `/TH/Empleados`
- **Reportes:** `/TH/EmpleadosReportes`
- **Catálogos:** `/TH/Catalogos`
- **Desvinculaciones:** `/TH/Desvinculaciones` ✨ NUEVO

---

## 📋 Empleados Controller

### Búsqueda y Consulta

#### POST /TH/Empleados/Search
Busca empleados con filtros
```json
Request Body:
{
  "identificacion": "string",
  "nombres": "string",
  "apellidos": "string",
  "activo": true,
  "areaServiceLineId": 0,
  "cargoId": 0,
  "sedeId": 0
}

Response:
{
  "success": true,
  "data": [...]
}
```

#### GET /TH/Empleados/{identificacion}
Obtiene empleado por identificación
```json
Response:
{
  "success": true,
  "data": { empleado }
}
```

---

### Actualización de Datos Maestros ✨ NUEVO

#### PUT /TH/Empleados/DatosGenerales
Crear/actualizar datos generales del empleado
```json
Request Body:
{
  "personaId": 0,
  "tipoIdentificacion": "CC",
  "identificacion": "12345678",
  "primerNombre": "Juan",
  "segundoNombre": "Carlos",
  "primerApellido": "Pérez",
  "segundoApellido": "García",
  "fechaNacimiento": "1990-01-15",
  "genero": "M",
  "paisNacimiento": "Colombia",
  "grupoSanguineoId": 1,
  "estadoCivilId": 1,
  "telefonoCelular": "3001234567",
  "direccion": "Calle 123 #45-67",
  "fotoBase64": "data:image/jpeg;base64,...",
  "rutaFoto": "/fotos/empleados/guid.jpg",
  "esNuevo": true
}

Response:
{
  "success": true,
  "message": "Empleado creado exitosamente"
}
```

#### PUT /TH/Empleados/DatosLaborales
Actualizar datos laborales
```json
Request Body:
{
  "personaId": 123,
  "idIStaff": 456,
  "jefeInmediato": 789,
  "sede": "Bogotá",
  "cargo": 10,
  "area": 5,
  "banda": 3,
  "level": 2,
  "tipoContrato": 1,
  "fechaIngreso": "2020-01-15",
  "correoIpsos": "juan.perez@ipsos.com",
  "saldoVacaciones": 15,
  "diasVacaciones": 15,
  "salarioActual": 5000000,
  "salarioAnterior": 4500000,
  "esAcumulador": false
}

Response:
{
  "success": true,
  "message": "Datos laborales actualizados exitosamente"
}
```

#### PUT /TH/Empleados/DatosPersonales
Actualizar datos personales
```json
Request Body:
{
  "personaId": 123,
  "direccion": "Calle 123 #45-67",
  "telefono": "6012345678",
  "telefonoCelular": "3001234567",
  "emailPersonal": "juan@gmail.com",
  "nse": 3,
  "tallaCamiseta": 2,
  "numeroHijos": 2,
  "personasACargo": 3,
  "licenciaConduccion": "B1",
  "estadoCivil": 1,
  "grupoSanguineo": 2
}

Response:
{
  "success": true,
  "message": "Datos personales actualizados exitosamente"
}
```

#### PUT /TH/Empleados/Nomina
Actualizar información de nómina
```json
Request Body:
{
  "personaId": 123,
  "banco": 1,
  "tipoCuenta": 1,
  "numeroCuenta": "123456789",
  "eps": 2,
  "fondoPensiones": 3,
  "fondoCesantias": 4,
  "cajaCompensacion": 5,
  "arl": 1,
  "salario": 5000000
}

Response:
{
  "success": true,
  "message": "Información de nómina actualizada exitosamente"
}
```

#### PUT /TH/Empleados/NivelIngles
Actualizar nivel de inglés
```json
Request Body:
{
  "personaId": 123,
  "nivelInglesId": 3
}

Response:
{
  "success": true,
  "message": "Nivel de inglés actualizado exitosamente"
}
```

---

### Experiencia Laboral

#### GET /TH/Empleados/{identificacion}/experiencia
Obtiene experiencias laborales

#### POST /TH/Empleados/{identificacion}/experiencia
Agrega experiencia laboral
```json
Request Body:
{
  "empresa": "Empresa XYZ",
  "fechaInicio": "2015-01-01",
  "fechaFin": "2019-12-31",
  "cargo": "Analista",
  "esInvestigacion": true
}
```

#### DELETE /TH/Empleados/experiencia/{id}
Elimina experiencia laboral

---

### Educación

#### GET /TH/Empleados/{identificacion}/educacion
Obtiene educación

#### POST /TH/Empleados/{identificacion}/educacion
Agrega educación
```json
Request Body:
{
  "nivelEducativo": "Profesional",
  "institucion": "Universidad Nacional",
  "titulo": "Ingeniero de Sistemas",
  "fechaInicio": "2010-01-01",
  "fechaFin": "2014-12-31",
  "graduado": true
}
```

#### DELETE /TH/Empleados/educacion/{id}
Elimina educación

---

### Hijos

#### GET /TH/Empleados/{identificacion}/hijos
Obtiene hijos

#### POST /TH/Empleados/{identificacion}/hijos
Agrega hijo
```json
Request Body:
{
  "nombres": "María",
  "apellidos": "Pérez",
  "fechaNacimiento": "2015-03-20",
  "genero": "F"
}
```

#### DELETE /TH/Empleados/hijos/{id}
Elimina hijo

---

### Contactos de Emergencia

#### GET /TH/Empleados/{identificacion}/contactos
Obtiene contactos de emergencia

#### POST /TH/Empleados/{identificacion}/contactos
Agrega contacto de emergencia
```json
Request Body:
{
  "nombres": "Ana",
  "apellidos": "García",
  "parentesco": "Esposa",
  "telefono": "3009876543",
  "direccion": "Calle 456 #78-90"
}
```

#### DELETE /TH/Empleados/contactos/{id}
Elimina contacto de emergencia

---

### Promociones

#### GET /TH/Empleados/{identificacion}/promociones
Obtiene promociones

#### POST /TH/Empleados/{identificacion}/promociones
Agrega promoción
```json
Request Body:
{
  "nuevaAreaId": 5,
  "nuevaBandaId": 4,
  "nuevoCargoId": 15,
  "nuevoLevelId": 3,
  "fechaPromocion": "2023-06-01"
}
```

---

### Salarios

#### GET /TH/Empleados/{identificacion}/salarios
Obtiene salarios

#### POST /TH/Empleados/{identificacion}/salarios
Agrega salario
```json
Request Body:
{
  "fechaAplicacion": "2023-01-01",
  "motivoCambio": 1,
  "salario": 6000000,
  "tipo": 1
}
```

---

### Retiro y Reintegro

#### POST /TH/Empleados/{identificacion}/retiro
Retira empleado
```json
Request Body:
{
  "fechaRetiro": "2023-12-31",
  "observacion": "Renuncia voluntaria"
}
```

#### POST /TH/Empleados/{identificacion}/reintegro
Reintegra empleado
```json
Request Body:
{
  "fechaReintegro": "2024-02-01"
}
```

---

## 📊 Reportes Controller

### POST /TH/EmpleadosReportes/Export
Exporta reportes a Excel
```json
Request Body:
{
  "tipoReporte": "InformacionGeneral"
}

Tipos válidos:
- "InformacionGeneral"
- "Hijos"
- "Educacion"
- "ExperienciaLaboral"
- "ContactosEmergencia"

Response: Excel file download
```

### GET /TH/EmpleadosReportes/EstadoDiligenciamiento
Obtiene estado de diligenciamiento de empleados

---

## 🗂️ Catálogos Controller ✨ NUEVO

### GET /TH/Catalogos/AreasServiceLines
Obtiene áreas/service lines
```json
Response:
{
  "success": true,
  "data": [
    {
      "id": 1,
      "nombre": "Analytics & Data Science",
      "activo": true
    },
    ...
  ]
}
```

### GET /TH/Catalogos/GruposSanguineos
Obtiene grupos sanguíneos
```json
Response:
{
  "success": true,
  "data": [
    {
      "id": 1,
      "descripcion": "O+",
      "activo": true
    },
    ...
  ]
}
```

### GET /TH/Catalogos/Cargos
Obtiene cargos
```json
Response:
{
  "success": true,
  "data": [
    {
      "id": 1,
      "descripcion": "Analista Junior",
      "activo": true
    },
    ...
  ]
}
```

### GET /TH/Catalogos/EstadosCiviles
Obtiene estados civiles
```json
Response:
{
  "success": true,
  "data": [
    {
      "id": 1,
      "descripcion": "Soltero",
      "activo": true
    },
    ...
  ]
}
```

### GET /TH/Catalogos/Todos ⚡ OPTIMIZADO
Obtiene TODOS los catálogos en un solo request
```json
Response:
{
  "success": true,
  "data": {
    "areas": [...],
    "gruposSanguineos": [...],
    "cargos": [...],
    "estadosCiviles": [...],
    "bancos": [...],
    "tiposCuenta": [...],
    "eps": [...],
    "fondosPensiones": [...],
    "fondosCesantias": [...],
    "cajasCompensacion": [...],
    "arls": [...],
    "nivelesIngles": [...],
    "sedes": [...],
    "tiposContrato": [...],
    "nse": [...],
    "tallasCamiseta": [...],
    "bandas": [...],
    "levels": [...]
  }
}
```

**💡 Tip:** Usar `/Todos` para carga inicial de formularios en lugar de 18 requests individuales.

---

## � Desvinculaciones Controller ✨ NUEVO

### POST /TH/Desvinculaciones/Buscar
Busca procesos de desvinculación con paginación
```json
Request Body:
{
  "pageSize": 10,
  "pageIndex": 0,
  "textoBuscado": "Juan"
}

Response:
{
  "success": true,
  "data": {
    "totalRegistros": 45,
    "paginaActual": 0,
    "tamañoPagina": 10,
    "totalPaginas": 5,
    "desvinculaciones": [
      {
        "id": 1,
        "empleadoId": 123,
        "nombreCompleto": "Juan Pérez",
        "identificacion": "12345678",
        "cargo": "Analista",
        "area": "Analytics",
        "fechaRetiro": "2026-01-31",
        "fechaSolicitud": "2026-01-03",
        "motivoDesvinculacion": "Renuncia voluntaria",
        "estado": "En proceso",
        "responsableId": 45,
        "responsableNombre": "María García",
        "totalEvaluaciones": 5,
        "evaluacionesCompletadas": 3,
        "porcentajeAvance": 60.0
      },
      ...
    ]
  }
}
```

### GET /TH/Desvinculaciones/EmpleadosActivos
Obtiene empleados activos disponibles para desvinculación
```json
Response:
{
  "success": true,
  "data": [
    {
      "id": 123,
      "personaId": 456,
      "identificacion": "12345678",
      "nombreCompleto": "Juan Pérez",
      "cargo": "Analista",
      "area": "Analytics",
      "sede": "Bogotá",
      "fechaIngreso": "2020-01-15",
      "correoIpsos": "juan.perez@ipsos.com"
    },
    ...
  ]
}
```

### POST /TH/Desvinculaciones/Iniciar
Inicia proceso de desvinculación de empleado
```json
Request Body:
{
  "empleadoId": 123,
  "fechaRetiro": "2026-01-31",
  "motivosDesvinculacion": "Renuncia voluntaria por motivos personales"
}

Response:
{
  "success": true,
  "message": "Proceso de desvinculación iniciado exitosamente",
  "desvinculacionId": 15
}
```

### GET /TH/Desvinculaciones/{desvinculacionId}/Evaluaciones
Obtiene evaluaciones de áreas para una desvinculación
```json
Response:
{
  "success": true,
  "data": [
    {
      "id": 1,
      "desvinculacionEmpleadoId": 15,
      "nombreArea": "TI",
      "comentarios": "Equipos devueltos, accesos revocados",
      "nombreEvaluador": "Carlos López",
      "fechaDiligenciamiento": "2026-01-05T14:30:00",
      "completada": true,
      "estado": "Aprobado"
    },
    {
      "id": 2,
      "desvinculacionEmpleadoId": 15,
      "nombreArea": "RRHH",
      "comentarios": null,
      "nombreEvaluador": null,
      "fechaDiligenciamiento": null,
      "completada": false,
      "estado": "Pendiente"
    },
    ...
  ]
}
```

### GET /TH/Desvinculaciones/{desvinculacionId}/PDF
Genera PDF de formato de desvinculación
```json
Response:
{
  "success": true,
  "pdfBase64": "JVBERi0xLjQKJeLjz9MKNCAwIG9iago8P..." 
}

Nota: El PDF está en base64, decodificar para descargar
```

---

## �🔐 Autenticación

Todos los endpoints requieren autenticación: `[Authorize]`

Headers requeridos:
```
Authorization: Bearer {token}
Content-Type: application/json
```

---

## ⚠️ Manejo de Errores

Todas las respuestas siguen el patrón:

**Éxito:**
```json
{
  "success": true,
  "message": "Operación exitosa",
  "data": {...}  // opcional
}
```

**Error:**
```json
{
  "success": false,
  "message": "Descripción del error"
}
```

**Códigos HTTP:**
- 200 OK: Operación exitosa
- 400 Bad Request: Datos inválidos
- 401 Unauthorized: No autenticado
- 500 Internal Server Error: Error del servidor

---

## 📝 Validaciones

### Datos Generales
- ✅ Tipo identificación requerido
- ✅ Identificación requerida
- ✅ Primer nombre requerido
- ✅ Primer apellido requerido
- ✅ Edad mínima: 18 años

### Datos Laborales
- ✅ Correo Ipsos formato válido
- ✅ Fecha ingreso no > 30 días futuro

### Datos Personales
- ✅ Email personal formato válido

### Foto
- ⚠️ Base64 válido
- ⚠️ Formato sugerido: JPEG/PNG
- ⚠️ Tamaño máximo: 2MB (por implementar)

---

## 🧪 Ejemplos de Uso

### Crear un nuevo empleado
```javascript
// 1. Obtener catálogos
const response = await fetch('/TH/Catalogos/Todos');
const { data: catalogos } = await response.json();

// 2. Crear empleado
await fetch('/TH/Empleados/DatosGenerales', {
  method: 'PUT',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    personaId: 0,
    tipoIdentificacion: 'CC',
    identificacion: '12345678',
    primerNombre: 'Juan',
    primerApellido: 'Pérez',
    grupoSanguineoId: catalogos.gruposSanguineos[0].id,
    estadoCivilId: catalogos.estadosCiviles[0].id,
    esNuevo: true
  })
});
```

### Actualizar datos laborales
```javascript
await fetch('/TH/Empleados/DatosLaborales', {
  method: 'PUT',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    personaId: 123,
    cargo: 10,
    area: 5,
    correoIpsos: 'juan.perez@ipsos.com'
  })
});
```

### Cargar foto
```javascript
// Convertir archivo a base64
const fileInput = document.getElementById('fotoInput');
const file = fileInput.files[0];
const reader = new FileReader();

reader.onload = async (e) => {
  const base64 = e.target.result;
  
  await fetch('/TH/Empleados/DatosGenerales', {
    method: 'PUT',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({
      personaId: 123,
      fotoBase64: base64,
      esNuevo: false
    })
  });
};

reader.readAsDataURL(file);
```

### Iniciar desvinculación
```javascript
// 1. Obtener empleados activos
const responseEmpleados = await fetch('/TH/Desvinculaciones/EmpleadosActivos');
const { data: empleados } = await responseEmpleados.json();

// 2. Iniciar proceso
await fetch('/TH/Desvinculaciones/Iniciar', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify({
    empleadoId: 123,
    fechaRetiro: '2026-01-31',
    motivosDesvinculacion: 'Renuncia voluntaria'
  })
});
```

### Buscar desvinculaciones con paginación
```javascript
const filtros = {
  pageSize: 10,
  pageIndex: 0,
  textoBuscado: 'Juan'
};

const response = await fetch('/TH/Desvinculaciones/Buscar', {
  method: 'POST',
  headers: { 'Content-Type': 'application/json' },
  body: JSON.stringify(filtros)
});

const { data } = await response.json();
console.log(`Total: ${data.totalRegistros}, Página ${data.paginaActual + 1} de ${data.totalPaginas}`);
```

### Descargar PDF de desvinculación
```javascript
const response = await fetch(`/TH/Desvinculaciones/${desvinculacionId}/PDF`);
const { pdfBase64 } = await response.json();

// Decodificar y descargar
const blob = base64ToBlob(pdfBase64, 'application/pdf');
const url = URL.createObjectURL(blob);
const a = document.createElement('a');
a.href = url;
a.download = `desvinculacion_${desvinculacionId}.pdf`;
a.click();

function base64ToBlob(base64, type) {
  const binStr = atob(base64);
  const len = binStr.length;
  const arr = new Uint8Array(len);
  for (let i = 0; i < len; i++) {
    arr[i] = binStr.charCodeAt(i);
  }
  return new Blob([arr], { type: type });
}
```

---

## 📚 Referencias

- **Documentación completa:** `ANALISIS_TH_EMPLEADOS.md`
- **Cambios recientes:** `CAMBIOS_TH_EMPLEADOS_20260103.md`
- **Directrices:** `DIRECTRICES_MIGRACION.md`
