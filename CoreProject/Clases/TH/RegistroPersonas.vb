
'Imports CoreProject.TH_Model
'Imports CoreProject

<Serializable()>
Public Class RegistroPersonas
#Region "Variables Globales"
	Private oMatrixContext As TH_Entities
#End Region
#Region "Constructores"
	'Constructor public 
	Public Sub New()
		oMatrixContext = New TH_Entities
	End Sub
#End Region
#Region "Obtener"
	Public Function TipoIdentificacionList() As List(Of TH_TipoIdentificacion)
		Return oMatrixContext.TH_TipoIdentificacion.ToList
	End Function
	Public Function SexoList() As List(Of TH_Sexo)
		Return oMatrixContext.TH_Sexo.ToList
	End Function
	Public Function SexoXNombre(ByVal nombre As String) As TH_Sexo
		Return oMatrixContext.TH_Sexo.Where(Function(x) x.Sexo = nombre).FirstOrDefault
	End Function
	Public Function SexoXId(ByVal id As Int32) As TH_Sexo
		Return oMatrixContext.TH_Sexo.Where(Function(x) x.Id = id).FirstOrDefault
	End Function
	Public Function EstadoCivilList() As List(Of TH_EstadoCivil)
		Return oMatrixContext.TH_EstadoCivil.ToList
	End Function
	Public Function EstadoCivilXNombre(ByVal nombre As String) As TH_EstadoCivil
		Return oMatrixContext.TH_EstadoCivil.Where(Function(x) x.EstadoCivil = nombre).FirstOrDefault
	End Function
	Public Function EstadoCivilXid(ByVal id As Int32) As TH_EstadoCivil
		Return oMatrixContext.TH_EstadoCivil.Where(Function(x) x.Id = id).FirstOrDefault
	End Function
	Public Function NivelEducativoList() As List(Of TH_NivelesEducativos)
		Return oMatrixContext.TH_NivelesEducativos.ToList
	End Function
	Public Function NivelEducativoXId(ByVal id As Int32) As TH_NivelesEducativos
		Return oMatrixContext.TH_NivelesEducativos.Where(Function(x) x.id = id).FirstOrDefault
	End Function
	Public Function NivelEducativoXNombre(ByVal nombre As String) As TH_NivelesEducativos
		Return oMatrixContext.TH_NivelesEducativos.Where(Function(x) x.NivelEducativo = nombre).FirstOrDefault
	End Function
	Public Function GruposSanguineosList() As List(Of TH_GruposSanguineos)
		Return oMatrixContext.TH_GruposSanguineos.ToList
	End Function
	Public Function CiudadesList() As List(Of TH_Ciudades)
		Return oMatrixContext.TH_Ciudades.ToList
	End Function
	Public Function EmpresasList() As List(Of TH_Empresas)
		Return oMatrixContext.TH_Empresas.ToList
	End Function
	Public Function EstadosActualesList() As List(Of TH_EstadosActuales)
		Return oMatrixContext.TH_EstadosActuales.ToList
	End Function
	Public Function TipoContratoList() As List(Of TH_TipoContratacion2)
		Return oMatrixContext.TH_TipoContratacion2.ToList
	End Function
	Public Function TipoContratoListXGrupo(ByVal Grupo As Int16) As List(Of TH_TipoContratacion2)
		Return oMatrixContext.TH_TipoContratacion2.Where(Function(x) x.Grupo = Grupo).ToList
	End Function

	Public Function TipoSalarioList() As List(Of TH_TiposSalario)
		Return oMatrixContext.TH_TiposSalario.ToList
	End Function
	Public Function FormasSalarioList() As List(Of TH_FormasSalario)
		Return oMatrixContext.TH_FormasSalario.ToList
	End Function
	Public Function BancosList() As List(Of TH_Bancos)
		Return oMatrixContext.TH_Bancos.ToList
	End Function
	Public Function TiposCuentaList() As List(Of TH_TiposCuentaBancaria)
		Return oMatrixContext.TH_TiposCuentaBancaria.ToList
	End Function
	Public Function CargosList() As List(Of TH_Cargos2)
		Return oMatrixContext.TH_Cargos2.OrderBy(Function(x) x.Cargo).ToList
	End Function
	Public Function FondosPensionesList() As List(Of TH_FondosPensiones)
		Return oMatrixContext.TH_FondosPensiones.ToList
	End Function
	Public Function FondosCesantiasList() As List(Of TH_FondosCesantias)
		Return oMatrixContext.TH_FondosCesantias.ToList
	End Function
	Public Function EPSList() As List(Of TH_EPS)
		'Return oMatrixContext.TH_EPS.ToList
		Return oMatrixContext.TH_EPS.OrderBy(Function(x) x.EPS).ToList
	End Function
	Public Function CajaCompensacionList() As List(Of TH_CajasCompensacion)
		Return oMatrixContext.TH_CajasCompensacion.ToList
	End Function
	Public Function ARLList() As List(Of TH_ARL)
		Return oMatrixContext.TH_ARL.ToList
	End Function
	Public Function BUList() As List(Of TH_BU)
		Return oMatrixContext.TH_BU.ToList
	End Function
	Public Function AreaList(ByVal Bu As Int32?) As List(Of TH_Area)
		Return oMatrixContext.TH_Area.Where(Function(x) x.BU_Id = Bu).ToList
	End Function
	Public Function ListadoPersonas() As List(Of TH_Personas2)
		Return oMatrixContext.TH_Personas.ToList

	End Function
	Public Function ListadoPersonasActivas() As List(Of TH_Personas2)
		Return oMatrixContext.TH_Personas.Where(Function(x) x.Activo = True).ToList
	End Function
	Public Function ObtenerPersona(ByVal id As Int64) As TH_Personas2
		Return oMatrixContext.TH_Personas.Where(Function(x) x.id = id).FirstOrDefault
	End Function
	Public Function SedesList() As List(Of TH_Sedes)
		Return oMatrixContext.TH_Sedes.ToList
	End Function
	Public Function TH_PersonasGet(ByVal Id As Int64?, ByVal Nombre As String) As List(Of TH_PersonasGET_Result)
		Return oMatrixContext.TH_PersonasGET(Id, Nombre).ToList
	End Function
	Public Function ContratistasList() As List(Of TH_Contratistas)
		'Return oMatrixContext.TH_Contratistas.Where(Function(x) x.Activo = True).ToList
		Return oMatrixContext.TH_Contratistas.OrderBy(Function(x) x.Nombre).ToList
	End Function
	Public Function TH_PersonasContratistas(ByVal id As Int64?, ByVal Nombre As String) As List(Of TH_PersonasXTipoContratoGET_Result)
		Return oMatrixContext.TH_PersonasXTipoContratoGET(id, Nombre).ToList
	End Function
	Public Function ObtenerEncuestador(ByVal id As Int64) As OP_Encuestadores4
		Return oMatrixContext.OP_Encuestadores.Where(Function(x) x.id = id).FirstOrDefault
	End Function

	Public Function ListadoPersonasGet(ByVal Cedula As Int64?, ByVal nombre As String) As List(Of TH_PersonasGET_Result)
		Return oMatrixContext.TH_PersonasGET(Cedula, nombre).ToList
	End Function

	Public Function Reporte_PST_CT() As List(Of TH_Reporte_PST_CT_Result)
		Return oMatrixContext.TH_Reporte_PST_CT.ToList
	End Function

	Public Function ClasificacionList() As List(Of TH_ContratistasClasificacion)
		Return oMatrixContext.TH_ContratistasClasificacion.ToList
	End Function

#End Region
#Region "Guardar"
	Sub GuardarPersona(ByRef e As TH_Personas2, Optional ByVal Nuevo As Boolean = True)
		If Nuevo = True Then
			oMatrixContext.TH_Personas.Add(e)
		End If
		oMatrixContext.SaveChanges()
	End Sub
	Sub GuardarTipoEncuestador(ByVal idEncuestador As Int64, TipoEncuestador As Int32)
		If oMatrixContext.OP_Encuestadores.Where(Function(x) x.id = idEncuestador).ToList.Count = 0 Then
			oMatrixContext.OP_Encuestadores_Add(idEncuestador, TipoEncuestador)
		Else
			Dim ent As New OP_Encuestadores4
			ent = oMatrixContext.OP_Encuestadores.Where(Function(x) x.id = idEncuestador).FirstOrDefault
			ent.TipoId = TipoEncuestador
			oMatrixContext.SaveChanges()
		End If
	End Sub
	Sub EliminarEncuestador(ByVal idEncuestador As Int64)
		oMatrixContext.OP_Encuestadores_Del(idEncuestador)
	End Sub
	Sub actualizar(th_Persona As TH_Personas2, idAnterior As Nullable(Of Long), idNuevo As Nullable(Of Long))
		oMatrixContext.TH_Personas_Edit(idAnterior, idNuevo, th_Persona.TipoId, th_Persona.LugarExpedicion, th_Persona.FechaExpedicion, th_Persona.Nombres, th_Persona.Apellidos, th_Persona.Nacionalidad, th_Persona.Sexo, th_Persona.GrupoSanguineo, th_Persona.EstadoCivil, th_Persona.FechaNacimiento, th_Persona.CiudadId, th_Persona.BarrioResidencia, th_Persona.Direccion, th_Persona.Telefono1, th_Persona.Telefono2, th_Persona.Celular, th_Persona.EmailPersonal, th_Persona.NivelEducativo, th_Persona.Profesion, th_Persona.BU, th_Persona.Area, th_Persona.Sede, th_Persona.Cargo, th_Persona.JefeInmediato, th_Persona.FechaIngreso, th_Persona.TipoContratacion, th_Persona.Empresa, th_Persona.TipoSalario, th_Persona.FormaSalario, th_Persona.SalarioActual, th_Persona.UltimoSalario, th_Persona.FechaUltimoAscenso, th_Persona.UltimoCargo, th_Persona.EstadoActual, th_Persona.FechaVencimientoContrato, th_Persona.FechaRetiro, th_Persona.MotivoRetiro, th_Persona.Banco, th_Persona.TipoCuenta, th_Persona.CuentaBanco, th_Persona.FondoPensiones, th_Persona.FondoCesantias, th_Persona.EPS, th_Persona.CajaCompensacion, th_Persona.ARL, th_Persona.Activo, th_Persona.Contratista, th_Persona.NivelBI, th_Persona.HeadCount)
	End Sub



#End Region
End Class

