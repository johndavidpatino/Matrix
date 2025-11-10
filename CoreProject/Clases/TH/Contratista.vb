
'Imports CoreProject.TH_Model
'Imports CoreProject
Public Class Contratista
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
    Public Function ObtenerContratistas(ByVal Identificacion As Int64?, ByVal Nombre As String, ByVal activo As Boolean?) As List(Of TH_ContratistasGet_Result)
        Return oMatrixContext.TH_ContratistasGet(Identificacion, Nombre, activo).ToList
    End Function
    Public Function ObtenerContratista(ByVal id As Int64) As TH_Contratistas
        Return oMatrixContext.TH_Contratistas.Where(Function(x) x.Identificacion = id).FirstOrDefault
    End Function
    Public Function ExisteContratista(ByVal id As Int64) As Boolean
        If oMatrixContext.TH_Contratistas.Where(Function(x) x.Identificacion = id).ToList.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function ObtenerServicios(ByVal id As Int64?) As List(Of TH_ServicioContratistaGet_Result)
        Return oMatrixContext.TH_ServicioContratistaGet(id).ToList
    End Function

    Public Function ObtenerEstados() As List(Of TH_ContratistasEstadosGet_Result)
        Return oMatrixContext.TH_ContratistasEstadosGet().ToList
    End Function

    Public Function ObtenerServiciosContratista(ByVal IdentificacionId As Int64) As List(Of TH_ContratistasDetalleServiciosGet_Result)
        Return oMatrixContext.TH_ContratistasDetalleServiciosGet(IdentificacionId).ToList
    End Function

    Public Function ObtenerCuentasContablesXServicio(ByVal idServicio As Int64?) As List(Of TH_ServiciosContratistasCuentasContables_Get_Result)
        Return oMatrixContext.TH_ServiciosContratistasCuentasContables_Get(idServicio).ToList
    End Function

    Public Function LogContratistasGet(ByVal ContratistaId As Int64?, ByVal Nombre As String) As List(Of TH_LogContratistaGet_Result)
        Return oMatrixContext.TH_LogContratistaGet(ContratistaId, Nombre).ToList
    End Function

    Public Function ObtenerClasificacion(ByVal clasificacion As Int64?) As TH_ContratistasClasificacion
        Return oMatrixContext.TH_ContratistasClasificacion.Where(Function(x) x.Id = clasificacion).FirstOrDefault
    End Function

#End Region

#Region "Guardar"
    Sub Guardar(ByVal Identificacion As Int64, ByVal Nombre As String, ByVal Direccion As String, ByVal Email As String, ByVal Activo As Int16, ByVal CiudadId As Int64, ByVal NumeroSymphony As Int64, ByVal ServicioId As Int64, ByVal DescripcionCuenta As String, ByVal Telefono As String, ByVal FechaRegistro As DateTime, ByVal Estado As Int64, ByVal Solicitud As String, ByVal Aprobado As String, ByVal Observaciones As String, ByVal Clasificacion As Int32)
        oMatrixContext.TH_ContratistasAdd(Identificacion, Nombre, Direccion, Email, Activo, CiudadId, NumeroSymphony, ServicioId, DescripcionCuenta, Telefono, FechaRegistro, Estado, Solicitud, Aprobado, Observaciones, Clasificacion)
    End Sub

    Sub GuardarServiciosContratista(ByVal identificacionId As Int64, ByVal ServicioId As Int64, ByVal NombreServicio As String, ByVal Estado As Boolean)
        oMatrixContext.TH_ContratistasDetalleServiciosAdd(identificacionId, ServicioId, NombreServicio, Estado)
    End Sub

    Sub LogPersonasAddContratistas(ByVal ContratistaId As Int64, ByVal UsuarioId As Int64)
        oMatrixContext.TH_LogPersonasContratistasAdd(ContratistaId, UsuarioId)
    End Sub
    Sub LogContratistasAdd(ByVal ContratistaId As Int64, ByVal Observacion As String, ByVal UsarioId As Int64)
        oMatrixContext.TH_LogContratistasAdd(ContratistaId, Observacion, UsarioId)
    End Sub

#End Region

#Region "Actualizar"
    Sub Actualizar(ByVal Identificacion As Int64, ByVal Nombre As String, ByVal Direccion As String, ByVal Email As String, ByVal Activo As Int16, ByVal CiudadId As Int64, ByVal NumeroSymphony As Int64, ByVal ServicioId As Int64, ByVal DescripcionCuenta As String, ByVal Telefono As String, ByVal FechaRegistro As DateTime, ByVal Estado As Int64, ByVal Solicitud As String, ByVal Aprobado As String, ByVal Observaciones As String, ByVal Clasificacion As Int32)
        oMatrixContext.TH_ContratistaUpdate(Identificacion, Nombre, Direccion, Email, Activo, CiudadId, NumeroSymphony, ServicioId, DescripcionCuenta, Telefono, FechaRegistro, Estado, Solicitud, Aprobado, Observaciones, Clasificacion)
    End Sub

    Sub ActualizarEstado(ByVal Identificacion As Int64, ByVal Estado As Int16)
        oMatrixContext.TH_ContratistaActualizarEstado(Identificacion, Estado)
    End Sub

    Sub ActualizarEstadoServicioContratista(ByVal Id As Int16, ByVal Estado As Boolean)
        oMatrixContext.TH_ContratistasDetalleServiciosUpdate(Id, Estado)
    End Sub
    Sub ActualizarEstadoPersonasContratista(ByVal ContratistaId As Int64)
        oMatrixContext.TH_PersonasContratistaUpdateEstado(ContratistaId)
    End Sub
#End Region

End Class
