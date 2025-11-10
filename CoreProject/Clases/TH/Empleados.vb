Public Class Empleados
    Public Enum ETipoSalario
        Fijo = 1
        Variable = 2
        Integral = 4
    End Enum

    Private _Context As TH_Entities
    Sub New()
        _Context = New TH_Entities()
    End Sub
    Function obtener(id As Int64?, nombres As String, apellidos As String, activo As Boolean?, areaServiceLine As UShort?, cargo As Byte?, sede As Byte?) As List(Of TH_Empleados_Get_Result)
        Return _Context.TH_Empleados_Get(id, nombres, apellidos, activo, areaServiceLine, cargo, sede).ToList
    End Function
    Function obtenerPorIdentificacion(identificacion As Int64) As TH_Empleados_Get_Result
        Return _Context.TH_Empleados_Get(identificacion, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
    End Function
    Function grabarDatosGenerales(id As Long, tipoId As UShort, nombres As String, apellidos As String, nombrePreferido As String, fechaNacimiento As Date, sexo As String, estadoCivil As UShort, grupoSanguineo As UShort, nacionalidad As String, urlFoto As String, fechaCreacion As Date, usuarioRegistro As Long, fechaUltimaActualizacion As Date) As Decimal
        Return _Context.TH_Empleados_DatosGenerales_Add(id, tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, urlFoto, fechaCreacion, usuarioRegistro, fechaUltimaActualizacion).FirstOrDefault
    End Function
    Sub actualizarDatosLaborales(id As Long, idIStaff As Long, jefeInmediato As Long, sede As UShort, correoIpsos As String, fechaingreso As Date, centroCostoId As Long, tipoContratoId As Byte, tiempoContratoId As Byte?, empresa As Byte, jobFunctionId As Byte, observaciones As String)
        _Context.TH_Empleados_DatosLaborales_Edit(id, idIStaff, jefeInmediato, sede, correoIpsos, fechaingreso, centroCostoId, tipoContratoId, tiempoContratoId, empresa, jobFunctionId, observaciones)
    End Sub
    Function obtenerPromocionesPorPersonaId(personaId As Long) As List(Of TH_Promociones_Get_Result)
        Return _Context.TH_Promociones_Get(personaId).ToList
    End Function
    Sub eliminarPromocion(id As Long)
        _Context.TH_Promociones_Del(id)
    End Sub
    Function agregarPromocion(personaId As Long, nuevaAreaId As Byte, nuevaBandaId As Byte, nuevoCargoId As Short, fechaPromocion As Date, nuevoLevelId As Short) As Long
        Return _Context.TH_Promociones_Add(personaId, nuevaAreaId, nuevaBandaId, nuevoCargoId, nuevoLevelId, fechaPromocion)
    End Function
    Function obtenerMotivosCambioSalario() As List(Of TH_MotivosCambioSalario_Get_Result)
        Return _Context.TH_MotivosCambioSalario_Get.ToList
    End Function
    Sub eliminarSalario(id As Long)
        _Context.TH_Salarios_Del(id)
    End Sub
    Function agregarSalario(personaId As Long, fechaAplicacion As Date, motivoCambioId As Byte?, salario As Decimal, tipo As Byte?) As Long
        Return _Context.TH_Salarios_Add(personaId, fechaAplicacion, motivoCambioId, tipo, salario)
    End Function
    Function obtenerSalariosPorPersonaId(personaId As Long) As List(Of TH_Salarios_Get_Result)
        Return _Context.TH_Salarios_Get(personaId).ToList
    End Function
    Function obtenerTiemposContrato() As List(Of TH_TiemposContrato_Get_Result)
        Return _Context.TH_TiemposContrato_Get.ToList
    End Function
    Function obtenerTiposSalario() As List(Of TH_TiposSalario_Get_Result)
        Return _Context.TH_TiposSalario_Get.ToList
    End Function
    Sub actualizarNomina(id As Long, bancoId As Byte, tipoCuentaId As Byte, numeroCuenta As String, fondoPensionesId As Byte, fondoCesantiasId As Byte, EPSId As Byte, cajaCompensacionId As Byte, ARLId As Byte)
        _Context.TH_Empleados_Nomina_Edit(id, bancoId, tipoCuentaId, numeroCuenta, fondoPensionesId, fondoCesantiasId, EPSId, cajaCompensacionId, ARLId)
    End Sub
    Sub actualizarDatosPersonales(id As Long, ciudadId As Long, direccion As String, nseId As UShort, telefonoFijo As Long?, telefonoCelular As Long, emailPersonal As String, barrio As String, localidad As Byte, municipioNacimientoId As Integer, tallaCamisetaId As UShort)
        _Context.TH_Empleados_DatosPersonales_Edit(id, ciudadId, direccion, nseId, telefonoFijo, telefonoCelular, emailPersonal, barrio, localidad, municipioNacimientoId, tallaCamisetaId)
    End Sub
    Function obtenerNSE() As List(Of NSE_Get_Result)
        Return _Context.NSE_Get.ToList
    End Function
    Sub actualizarDatosGenerales(id As Long, tipoId As Byte, nombres As String, apellidos As String, nombrePreferido As String, fechaNacimiento As Date, sexo As String, estadoCivil As Integer, grupoSanguineo As Integer, nacionalidad As String, urlFoto As String, fechaUltimaActualizacion As Date)
        _Context.TH_Empleados_DatosGenerales_Edit(id, tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, urlFoto, fechaUltimaActualizacion)
    End Sub
    Function obtenerLocalidades() As List(Of TH_Localidades_Get_Result)
        Return _Context.TH_Localidades_Get().ToList
    End Function
    Function obtenerNivelesIdioma() As List(Of TH_NivelesIdiomas_Get_Result)
        Return _Context.TH_NivelesIdiomas_Get().ToList
    End Function
    Sub actualizarNivelIngles(personaId As Long, nivelInglesId As UShort)
        _Context.TH_Empleados_NivelIngles_Edit(personaId, nivelInglesId)
    End Sub
    Function obtenerEmpresas() As List(Of TH_Empresas_Get_Result)
        Return _Context.TH_Empresas_Get.ToList
    End Function
    Sub actualizarSalario(personaId As Long, salario As Decimal, tipo As Byte?)
        _Context.TH_Empleados_DatosLaborales_ActualizarSalario(personaId, salario, tipo)
    End Sub
    Function obtenerEmpleadosPorMesCumpleaños(mes As UShort) As List(Of TH_Personas2)
        Return _Context.TH_Personas.Where(Function(x) x.FechaNacimiento.Value.Month = mes AndAlso x.Activo = True AndAlso x.TipoContratacion <> 4 AndAlso x.TipoContratacion <> 7)
    End Function
    Function obtenerReporteDiligenciamiento() As List(Of TH_Empleados_EstadoDiligenciamientoDatos_Get_Result)
        Return _Context.TH_Empleados_EstadoDiligenciamientoDatos_Get(Nothing).ToList
    End Function
    Function obtenerJobFunctions() As List(Of TH_JobFunctions_Get_Result)
        Return _Context.TH_JobFunctions_Get.ToList
    End Function
    Sub retirar(personaId As Long, observacion As String, fechaRetiro As Date, usuarioRegistro As Long, fechaRegistro As DateTime)
        _Context.TH_Empleados_Retirar(personaId, observacion, fechaRetiro, usuarioRegistro, fechaRegistro)
    End Sub
    Sub reintegrar(personaId As Long, fechaReintegro As Date)
        _Context.TH_Empleados_Reintegrar(personaId, fechaReintegro)
    End Sub
    Function obtenerReporteInformacionEmpleados() As List(Of TH_Empleados_Reporte_Info_Result)
        Return _Context.TH_Empleados_Reporte_Info.ToList
    End Function
    Function obtenerDatosEmergencia(nombres As String, apellidos As String, serviceLine As Short?, cargo As Short?, sede As Byte?) As List(Of TH_Empleados_DatosEmergencia_Get_Result)
        Return _Context.TH_Empleados_DatosEmergencia_Get(nombres, apellidos, serviceLine, cargo, sede).ToList
    End Function
    Function obtenerReporteHijosEmpleados() As List(Of TH_Hijos_Get_Result)
        Return _Context.TH_Hijos_Get(Nothing).ToList()
    End Function
    Function obtenerReporteHijosEmpleadosReport() As List(Of TH_Hijos_Report_Result)
        Return _Context.TH_Hijos_Report(Nothing).ToList()
    End Function
End Class
