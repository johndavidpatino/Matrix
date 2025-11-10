Imports System.IO
Imports System.Web.Script.Services
Imports CoreProject

Public Class EmpleadoUpdate
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getEmpleado() As CoreProject.TH_Empleados_Get_Result
        Dim empleados As New CoreProject.Empleados
        Dim context = HttpContext.Current
        If context.Session("IDUsuario") Is Nothing Then
            context.Response.Clear()
            context.Response.StatusCode = 401
            context.Response.End()
        Else
            Return empleados.obtenerPorIdentificacion(context.Session("IDUsuario"))
        End If

    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getExperienciasLaborales() As List(Of CoreProject.TH_ExperienciaLaboral_Get_Result)
        Dim experienciaLaboral As New CoreProject.TH.ExperienciaLaboral
        Dim context = HttpContext.Current
        Return experienciaLaboral.getByPersonaId(context.Session("IDUsuario"))
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getEducacion() As List(Of CoreProject.TH_Educacion_Get_Result)
        Dim educacion As New CoreProject.TH.Educacion
        Dim context = HttpContext.Current
        Return educacion.ObtenerEducacionPorIdentificacion(context.Session("IDUsuario"))
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getHijos() As List(Of CoreProject.TH_Hijos_Get_Result)
        Dim personas As New CoreProject.Personas
        Dim context = HttpContext.Current
        Return personas.obtenerHijosPorPersonaId(context.Session("IDUsuario"))
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getContactosEmergencia() As List(Of CoreProject.TH_ContactosEmergencia_Get_Result)
        Dim personas As New CoreProject.Personas
        Dim context = HttpContext.Current
        Return personas.obtenerContactosEmergenciaPorPersonaId(context.Session("IDUsuario"))
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getPromociones() As List(Of CoreProject.TH_Promociones_Get_Result)
        Dim empleados As New CoreProject.Empleados
        Dim context = HttpContext.Current
        Return empleados.obtenerPromocionesPorPersonaId(context.Session("IDUsuario"))
    End Function
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Function getSalarios() As List(Of CoreProject.TH_Salarios_Get_Result)
        Dim empleados As New CoreProject.Empleados
        Dim context = HttpContext.Current
        Return empleados.obtenerSalariosPorPersonaId(context.Session("IDUsuario"))
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
    Shared Sub addExperienciaLaboral(empresa As String, fechaInicio As Date, fechaFin As Date, cargo As String, esInvestigacion As Boolean)
        Dim experienciaLaboral As New CoreProject.TH.ExperienciaLaboral
        Dim context = HttpContext.Current
        experienciaLaboral.add(context.Session("IDUsuario"), empresa, fechaInicio, fechaFin, cargo, esInvestigacion)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub addEducacion(tipo As UShort, titulo As String, institucion As String, pais As String, ciudad As String, fechaInicio As Date, fechaFin As Date?, modalidad As UShort, estado As UShort)
        Dim educacion As New CoreProject.TH.Educacion
        Dim context = HttpContext.Current
        educacion.AgregarEducacion(context.Session("IDUsuario"), tipo, titulo, institucion, pais, ciudad, fechaInicio, fechaFin, modalidad, estado)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub addHijo(nombres As String, apellidos As String, genero As Byte, fechaNacimiento As Date)
        Dim hijos As New CoreProject.Personas
        Dim context = HttpContext.Current
        hijos.agregarHijo(context.Session("IDUsuario"), nombres, apellidos, genero, fechaNacimiento)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub addContactoEmergencia(nombres As String, apellidos As String, parentesco As Byte, telefonoFijo As Long?, telefonoCelular As Long?)
        Dim contactosEmergencia As New CoreProject.Personas
        Dim context = HttpContext.Current
        contactosEmergencia.agregarContactoEmergencia(context.Session("IDUsuario"), nombres, apellidos, parentesco, telefonoFijo, telefonoCelular)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub addSalario(personaId As Int64, fechaAplicacion As Date, motivoCambio As UShort?, salario As Decimal, tipo As UShort?)
        Dim empleados As New CoreProject.Empleados
        empleados.agregarSalario(personaId, fechaAplicacion, motivoCambio, salario, tipo)
    End Sub


    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub updateDatosGenerales(tipoId As UShort, nombres As String, apellidos As String, nombrePreferido As String, fechaNacimiento As Date, sexo As String, estadoCivil As UShort, grupoSanguineo As UShort, nacionalidad As String, fotoBase64 As String)
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

        empleados.actualizarDatosGenerales(context.Session("IDUsuario"), tipoId, nombres, apellidos, nombrePreferido, fechaNacimiento, sexo, estadoCivil, grupoSanguineo, nacionalidad, relativeImagePath, DateTime.Now)
    End Sub
    <Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Shared Sub updateDatosPersonales(ciudadId As Long, direccion As String, nseId As UShort, telefonoFijo As Long?, telefonoCelular As Long, emailPersonal As String, barrio As String, localidad As String, municipioNacimientoId As Integer, tallaCamisetaId As UShort)
        Dim empleados As New CoreProject.Empleados
        Dim context = HttpContext.Current

        empleados.actualizarDatosPersonales(context.Session("IDUsuario"), ciudadId, direccion, nseId, telefonoFijo, telefonoCelular, emailPersonal, barrio, localidad, municipioNacimientoId, tallaCamisetaId)
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

End Class