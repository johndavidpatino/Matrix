Imports System.IO
Imports System.Web.Script.Services
Imports CoreProject

Public Class Empleados
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub
    <Services.WebMethod()>
    Shared Function save(idPersonSelected As Int64?, tipoIdentificacion As Int16?, identificacion As String, foto As String, nombres As String, apellidos As String, edad As UShort?, experiencia As UShort?, nivelIngles As UShort?, numeroCelular As Int64?, correo As String, fechaEntrevista As Date?, observacion As String, keywords As String, ciudadResidencia As UShort?) As Decimal
        Dim hojasVida As New CoreProject.HojasVida()
        Dim personId As Decimal
        Dim relativeImagePath As String = Nothing

        If (Not String.IsNullOrEmpty(foto)) Then
            Dim imageGuid = Guid.NewGuid()
            Dim bytes = getArrayBytesFromStringFormatBase64(foto)
            Dim physicalImagePath = getPhysicalImagePath(imageGuid.ToString())
            relativeImagePath = getRelativeImagePath(imageGuid.ToString())
            saveImage(bytes, physicalImagePath)
        End If

        If (idPersonSelected.HasValue = False) Then
            'personId = hojasVida.agregar(tipoIdentificacion, identificacion, nombres, apellidos, relativeImagePath, edad, experiencia, nivelIngles, numeroCelular, correo, keywords)
        Else
            'hojasVida.actualizarHojasVida(idPersonSelected.Value, tipoIdentificacion, identificacion, nombres, apellidos, relativeImagePath, edad, experiencia, nivelIngles, numeroCelular, correo, keywords)
        End If

        If idPersonSelected.HasValue Then
            personId = idPersonSelected.Value
        End If

        If fechaEntrevista.HasValue Then
            hojasVida.agregarEntrevista(personId, fechaEntrevista, observacion)
        End If

        Return personId

    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getEmpleados(id As Int64?, nombres As String, apellidos As String, activo As Boolean?, areaServiceLine As UShort?, cargo As Byte?, sede As Byte?) As List(Of CoreProject.TH_Empleados_Get_Result)
        Dim empleados As New CoreProject.Empleados
        Return empleados.obtener(id, nombres, apellidos, activo, areaServiceLine, cargo, sede)
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getEmpleadoPorIdentificacion(identificacion As Int64) As CoreProject.TH_Empleados_Get_Result
        Dim empleados As New CoreProject.Empleados
        Return empleados.obtenerPorIdentificacion(identificacion)
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getExperienciasLaboralesPorIdentificacion(identificacion As Int64) As List(Of CoreProject.TH_ExperienciaLaboral_Get_Result)
        Dim experienciaLaboral As New CoreProject.TH.ExperienciaLaboral
        Return experienciaLaboral.getByPersonaId(identificacion)
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getEducacionPorIdentificacion(identificacion As Int64) As List(Of CoreProject.TH_Educacion_Get_Result)
        Dim educacion As New CoreProject.TH.Educacion
        Return educacion.ObtenerEducacionPorIdentificacion(identificacion)
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getHijosPorIdentificacion(identificacion As Long) As List(Of CoreProject.TH_Hijos_Get_Result)
        Dim personas As New CoreProject.Personas
        Return personas.obtenerHijosPorPersonaId(identificacion)
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getContactosEmergenciaPorIdentificacion(identificacion As Long) As List(Of CoreProject.TH_ContactosEmergencia_Get_Result)
        Dim personas As New CoreProject.Personas
        Return personas.obtenerContactosEmergenciaPorPersonaId(identificacion)
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getPromocionesPorIdentificacion(identificacion As Long) As List(Of CoreProject.TH_Promociones_Get_Result)
        Dim empleados As New CoreProject.Empleados
        Return empleados.obtenerPromocionesPorPersonaId(identificacion)
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getSalariosPorIdentificacion(identificacion As Long) As List(Of CoreProject.TH_Salarios_Get_Result)
        Dim empleados As New CoreProject.Empleados
        Return empleados.obtenerSalariosPorPersonaId(identificacion)
    End Function

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub deleteExperienciaLaboral(identificacion As Int64)
        Dim experienciaLaboral As New CoreProject.TH.ExperienciaLaboral
        experienciaLaboral.deleteById(identificacion)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub deleteEducacion(identificacion As Int64)
        Dim educacion As New CoreProject.TH.Educacion
        educacion.EliminarEducacion(identificacion)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub deleteHijo(id As Int64)
        Dim hijos As New CoreProject.Personas
        hijos.eliminarHijoPorId(id)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub deleteContactoEmergencia(id As Int64)
        Dim contactosEmergencia As New CoreProject.Personas
        contactosEmergencia.eliminarContactoEmergenciaPorId(id)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub deletePromocion(id As Int64)
        Dim empleados As New CoreProject.Empleados
        empleados.eliminarPromocion(id)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub deleteSalario(id As Int64)
        Dim empleados As New CoreProject.Empleados
        empleados.eliminarSalario(id)
    End Sub

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub retirarEmpleado(identificacion As Long, fechaRetiro As Date, observacion As String)
        Dim empleados As New CoreProject.Empleados
        Dim context = HttpContext.Current
        empleados.retirar(identificacion, observacion, fechaRetiro, context.Session("IDUsuario"), DateTime.Now)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub reintegrarEmpleado(identificacion As Long, fechaReintegro As Date)
        Dim empleados As New CoreProject.Empleados
        Dim context = HttpContext.Current
        empleados.reintegrar(identificacion, fechaReintegro)
    End Sub

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub addExperienciaLaboral(identificacion As Int64, empresa As String, fechaInicio As Date, fechaFin As Date, cargo As String, esInvestigacion As Boolean)
        Dim experienciaLaboral As New CoreProject.TH.ExperienciaLaboral
        experienciaLaboral.add(identificacion, empresa, fechaInicio, fechaFin, cargo, esInvestigacion)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub addEducacion(identificacion As Int64, tipo As UShort, titulo As String, institucion As String, pais As String, ciudad As String, fechaInicio As Date, fechaFin As Date?, modalidad As UShort, estado As UShort)
        Dim educacion As New CoreProject.TH.Educacion
        educacion.AgregarEducacion(identificacion, tipo, titulo, institucion, pais, ciudad, fechaInicio, fechaFin, modalidad, estado)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub addHijo(personaId As Int64, nombres As String, apellidos As String, genero As Byte, fechaNacimiento As Date)
        Dim hijos As New CoreProject.Personas
        hijos.agregarHijo(personaId, nombres, apellidos, genero, fechaNacimiento)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub addContactoEmergencia(personaId As Int64, nombres As String, apellidos As String, parentesco As Byte, telefonoFijo As Long?, telefonoCelular As Long?)
        Dim contactosEmergencia As New CoreProject.Personas
        contactosEmergencia.agregarContactoEmergencia(personaId, nombres, apellidos, parentesco, telefonoFijo, telefonoCelular)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub addPromocion(personaId As Int64, nuevaAreaId As Byte, nuevaBandaId As Byte, nuevoCargoId As Short, nuevoLevelId As Byte, fechaPromocion As Date)
        Dim empleados As New CoreProject.Empleados
        empleados.agregarPromocion(personaId, nuevaAreaId, nuevaBandaId, nuevoCargoId, fechaPromocion, nuevoLevelId)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub addSalario(personaId As Int64, fechaAplicacion As Date, motivoCambio As UShort?, salario As Decimal, tipo As UShort?)
        Dim empleados As New CoreProject.Empleados
        Dim empleado As New CoreProject.TH_Empleados_Get_Result

        empleado = empleados.obtenerPorIdentificacion(personaId)

        empleados.agregarSalario(personaId, fechaAplicacion, motivoCambio, salario, tipo)
    End Sub


    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub updateDatosGenerales(esNuevo As Boolean, id As Long, tipoId As UShort, nombres As String, apellidos As String, nombrePreferido As String, fechaNacimiento As Date, sexo As String, estadoCivil As UShort, grupoSanguineo As UShort, nacionalidad As String, fotoBase64 As String)
        Dim empleados As New CoreProject.Empleados
        Dim context = HttpContext.Current
        Dim relativeImagePath As String = Nothing

        If (Not String.IsNullOrEmpty(fotoBase64)) Then
            Dim imageGuid = Guid.NewGuid()
            Dim bytes = getArrayBytesFromStringFormatBase64(fotoBase64)
            Dim physicalImagePath = getPhysicalImagePath(imageGuid.ToString())
            relativeImagePath = getRelativeImagePath(imageGuid.ToString())
            saveImage(bytes, physicalImagePath)
        End If

        If esNuevo = True Then
            empleados.grabarDatosGenerales(id, tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, relativeImagePath, DateTime.UtcNow.AddHours(-5), context.Session("IDUsuario"), DateTime.Now)
        Else
            empleados.actualizarDatosGenerales(id, tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, relativeImagePath, DateTime.Now)
        End If
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub updateDatosLaborales(id As Long, idIStaff As Long, jefeInmediato As Long, sede As UShort, correoIpsos As String, fechaIngreso As Date, centroCosto As Long, tipoContratoId As Byte, tiempoContratoId As Byte?, empresa As Byte, jobFunctionId As Byte, observaciones As String)
        Dim empleados As New CoreProject.Empleados

        empleados.actualizarDatosLaborales(id, idIStaff, jefeInmediato, sede, correoIpsos, fechaIngreso, centroCosto, tipoContratoId, tiempoContratoId, empresa, jobFunctionId, observaciones)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub updateDatosPersonales(id As Long, ciudadId As Long, direccion As String, nseId As UShort, telefonoFijo As Long?, telefonoCelular As Long, emailPersonal As String, barrio As String, localidad As String, municipioNacimientoId As Integer, tallaCamisetaId As UShort)
        Dim empleados As New CoreProject.Empleados

        empleados.actualizarDatosPersonales(id, ciudadId, direccion, nseId, telefonoFijo, telefonoCelular, emailPersonal, barrio, localidad, municipioNacimientoId, tallaCamisetaId)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub updateNomina(id As Long, bancoId As Byte, tipoCuentaId As Byte, numeroCuenta As String, fondoPensionesId As Byte, fondoCesantiasId As Byte, EPSId As Byte, cajaCompensacionId As Byte, ARLId As Byte)
        Dim empleados As New CoreProject.Empleados

        empleados.actualizarNomina(id, bancoId, tipoCuentaId, numeroCuenta, fondoPensionesId, fondoCesantiasId, EPSId, cajaCompensacionId, ARLId)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub updateNivelIngles(id As Long, nivelInglesId As UShort)
        Dim empleados As New CoreProject.Empleados

        empleados.actualizarNivelIngles(id, nivelInglesId)
    End Sub

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getAreasServiceLines() As List(Of CoreProject.TH_Area_Get_Result)
        Dim areasServicesLines As New CoreProject.Personas
        Return areasServicesLines.obtenerAreasServicesLines()
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getGruposSanguineos() As List(Of CoreProject.TH_GruposSanguineos)
        Dim gruposSanguineos As New CoreProject.RegistroPersonas
        Return gruposSanguineos.GruposSanguineosList()
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getCargos() As List(Of CoreProject.TH_Cargos_Get_Result)
        Dim cargos As New CoreProject.Cargos
        Return cargos.DevolverTodos
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getEstadosCiviles() As List(Of CoreProject.TH_EstadosCiviles)
        Dim TH As New CoreProject.TH_Entities
        Return TH.TH_EstadosCiviles.ToList()
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getBandas() As List(Of CoreProject.TH_Bandas_Get_Result)
        Dim persona As New CoreProject.Personas
        Return persona.obtenerBandas
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getLevels() As List(Of CoreProject.TH_Levels_Get_Result)
        Dim personas As New CoreProject.Personas
        Return personas.obtenerLevels
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getJefesInmediatos() As List(Of (identificacion As Long, nombres As String))
        Dim personas As New CoreProject.Personas
        Return personas.personasActivas().Select(Function(x) (x.id, x.Nombres & " " & x.Apellidos)).ToList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getSedes() As List(Of CoreProject.TH_Sedes)
        Dim personas As New CoreProject.RegistroPersonas
        Return personas.SedesList()
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getTiposEducacion() As List(Of CoreProject.TH_TiposEducacion_Get_Result)
        Dim educacion As New CoreProject.TH.Educacion
        Return educacion.obtenerTiposEducacion
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getModalidadesEducacion() As List(Of CoreProject.TH_ModalidadesEducacion_Get_Result)
        Dim educacion As New CoreProject.TH.Educacion
        Return educacion.obtenerModalidadesEducacion
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getCentrosCosto() As List(Of CoreProject.FI_CentroCosto_Get_Result)
        Dim centrosCosto As New CoreProject.FI.Ordenes
        Return centrosCosto.obtenerCentroCosto
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getEstadosEducacion() As List(Of CoreProject.TH_EstadoEducacion_Get_Result)
        Dim educacion As New CoreProject.TH.Educacion
        Return educacion.obtenerEstadosEducacion
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getParentescos() As List(Of CoreProject.TH_Parentescos_Get_Result)
        Dim parentescos As New CoreProject.Personas
        Return parentescos.obtenerParentescos.ToList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getMotivosCambioSalario() As List(Of CoreProject.TH_MotivosCambioSalario_Get_Result)
        Dim motivosCambioSalario As New CoreProject.Empleados
        Return motivosCambioSalario.obtenerMotivosCambioSalario.ToList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getTiposContratacion() As List(Of CoreProject.TH_TiposContratacion_Get_Result)
        Dim tiposContratacion As New CoreProject.TH.TipoContratacion
        Return tiposContratacion.obtener.ToList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getTiposIdentificacion() As List(Of CoreProject.TH_TipoIdentificacion_Get_Result)
        Dim personas As New CoreProject.Personas
        Return personas.obtenerTiposIdentificacion
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getTiemposContrato() As List(Of CoreProject.TH_TiemposContrato_Get_Result)
        Dim empleado As New CoreProject.Empleados
        Return empleado.obtenerTiemposContrato
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getTiposSalario() As List(Of CoreProject.TH_TiposSalario_Get_Result)
        Dim empleado As New CoreProject.Empleados
        Return empleado.obtenerTiposSalario
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getBancos() As List(Of CoreProject.TH_Bancos)
        Dim registroPersonas As New CoreProject.RegistroPersonas
        Return registroPersonas.BancosList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getTiposCuentaBancaria() As List(Of CoreProject.TH_TiposCuentaBancaria)
        Dim registroPersonas As New CoreProject.RegistroPersonas
        Return registroPersonas.TiposCuentaList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getEPS() As List(Of CoreProject.TH_EPS)
        Dim registroPersonas As New CoreProject.RegistroPersonas
        Return registroPersonas.EPSList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getFondosPensiones() As List(Of CoreProject.TH_FondosPensiones)
        Dim registroPersonas As New CoreProject.RegistroPersonas
        Return registroPersonas.FondosPensionesList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getFondosCesantias() As List(Of CoreProject.TH_FondosCesantias)
        Dim registroPersonas As New CoreProject.RegistroPersonas
        Return registroPersonas.FondosCesantiasList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getCajasCompensacion() As List(Of CoreProject.TH_CajasCompensacion)
        Dim registroPersonas As New CoreProject.RegistroPersonas
        Return registroPersonas.CajaCompensacionList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getARL() As List(Of CoreProject.TH_ARL)
        Dim registroPersonas As New CoreProject.RegistroPersonas
        Return registroPersonas.ARLList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getCiudades() As List(Of CoreProject.TH_Ciudades)
        Dim registroPersonas As New CoreProject.RegistroPersonas
        Return registroPersonas.CiudadesList
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getNSE() As List(Of CoreProject.NSE_Get_Result)
        Dim empleados As New CoreProject.Empleados
        Return empleados.obtenerNSE
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getLocalidades() As List(Of CoreProject.TH_Localidades_Get_Result)
        Dim empleados As New CoreProject.Empleados
        Return empleados.obtenerLocalidades
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getNivelesIdioma() As List(Of CoreProject.TH_NivelesIdiomas_Get_Result)
        Dim empleados As New CoreProject.Empleados
        Return empleados.obtenerNivelesIdioma
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getEmpresas() As List(Of CoreProject.TH_Empresas_Get_Result)
        Dim empleados As New CoreProject.Empleados
        Return empleados.obtenerEmpresas
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getJobFunctions() As List(Of CoreProject.TH_JobFunctions_Get_Result)
        Dim empleados As New CoreProject.Empleados
        Return empleados.obtenerJobFunctions
    End Function

    Shared Sub saveImage(bytes() As Byte, physicalImagePath As String)
        File.WriteAllBytes(physicalImagePath, bytes)
    End Sub
    Shared Function getArrayBytesFromStringFormatBase64(stringimageFormatBase64 As String) As Byte()
        Dim bytes() As Byte = Convert.FromBase64String(stringimageFormatBase64)
        Return bytes
    End Function
    Shared Function getPhysicalImagePath(guid As String) As String
        Return HttpRuntime.AppDomainAppPath & "Images\Fotos\Empleados\" & guid & ".jpg"
    End Function
    Shared Function getRelativeImagePath(guid As String) As String
        Return "../Images/Fotos/Empleados/" & guid & ".jpg"
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function MunicipiosDivipola() As List(Of Divipola_Get_Result)
        Dim DivipolaRepository = New Divipola()
        Return DivipolaRepository.obtenerXPais(1)
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function TallasCamiseta() As List(Of TH_TallasCamiseta)
        Dim PersonasRepository = New Personas
        Return PersonasRepository.obtenerTallasCamiseta()
    End Function

    Private Sub Empleados_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Dim permisos As New Datos.ClsPermisosUsuarios
        Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

        If permisos.VerificarPermisoUsuario(154, UsuarioID) = False Then
            Response.Redirect("../Home/Default.aspx")
        End If

    End Sub
End Class