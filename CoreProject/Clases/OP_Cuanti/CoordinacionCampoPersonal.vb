
''Imports CoreProject.OP_Cuanti2_Model

<Serializable()>
Public Class CoordinacionCampoPersonal
#Region "Variables Globales"
    Private oMatrixContext As OP_Cuanti2
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Cuanti2
    End Sub
#End Region
#Region "Obtener"
    Public Function ObtenerPersonalAsignado(ByVal TrabajoId As Int64, CargoId As Int64?, Ciudad As Int32?) As List(Of OP_PersonasAsignacion_Result)
        Return oMatrixContext.OP_PersonasAsignadasTrabajo_Get(TrabajoId, CargoId, Ciudad).ToList
    End Function
    Public Function ObtenerPersonalSinAsignar(ByVal TrabajoId As Int64, CargoId As Int64?, Ciudad As Int32?) As List(Of OP_PersonasAsignacion_Result)
        Return oMatrixContext.OP_PersonasCapacitadasSinAsignadarTrabajo_Get(TrabajoId, CargoId, Ciudad).ToList
    End Function
    Public Function ObtenerPersonalAsignadoList(ByVal TrabajoId As Int64) As List(Of OP_PersonasAsignadasTrabajo)
        Return oMatrixContext.OP_PersonasAsignadasTrabajo.Where(Function(x) x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerPersonalAsignadoXCiudad(ByVal TrabajoId As Int64, CiudadId As Int32) As List(Of OP_PersonasAsignadasTrabajo)
        Dim lPersonal = (From lasignado In ObtenerPersonalAsignadoList(TrabajoId)
                         From lthper In oMatrixContext.TH_Personas
                         Where lasignado.Persona = lthper.id
                         Select lasignado)
        Return lPersonal.ToList
    End Function
    Public Function ObtenerPersonalAsignadoXCargo(ByVal TrabajoId As Int64, CargoId As Int32) As List(Of OP_PersonasAsignadasTrabajo)
        Dim lPersonal = (From lasignado In ObtenerPersonalAsignadoList(TrabajoId)
                         From lthper In oMatrixContext.TH_Personas
                         Where lasignado.Persona = lthper.id And lthper.Cargo = CargoId
                         Select lasignado)
        Return lPersonal.ToList
    End Function
    Public Function ObtenerListadoEncuestadores() As List(Of OP_Encuestadores)
        Return oMatrixContext.OP_Encuestadores.Where(Function(x) x.TH_Personas.Activo = True).ToList()
    End Function
    Public Function ObtenerPersonalAsignadoXId(ByVal Id As Int64) As OP_PersonasAsignadasTrabajo
        Return oMatrixContext.OP_PersonasAsignadasTrabajo.Where(Function(x) x.id = Id).FirstOrDefault
    End Function
    Public Function ObtenerPersonalAsignadoXPersonaYTrabajo(ByVal Cedula As Int64, ByVal Trabajo As Int64) As OP_PersonasAsignadasTrabajo
        Return oMatrixContext.OP_PersonasAsignadasTrabajo.Where(Function(x) x.Persona = Cedula AndAlso x.TrabajoId = Trabajo).FirstOrDefault
    End Function
    Public Function ObtenerPersonalCapacitadoNoAsignadoList(ByVal TrabajoId As Int64, ByVal Ciudad As Int32, ByVal Cargo As Int64) As List(Of TH_Personas)
        Dim LPersonal = (From lper In oMatrixContext.TH_Capacitaciones
                       From lperp In oMatrixContext.TH_CapacitacionesParticipantes
                       Where lper.TrabajoId = TrabajoId And lper.id = lperp.CapacitacionId
                       Select lperp.Participante)
        Dim LAsignado = (From lper In ObtenerPersonalAsignadoList(TrabajoId)
                         Select lper.Persona)
        'Dim lpersonas As New List(Of TH_Personas)
        Dim lpersonas = (From lthper In oMatrixContext.TH_Personas.ToList
                     Where LPersonal.Contains(lthper.id) And Not (LAsignado.Contains(lthper.id)) And lthper.Activo = True
                     Select lthper)
        Return lpersonas.Where(Function(x) x.CiudadId = Ciudad And x.Cargo = Cargo).ToList
    End Function

    Public Function ObtenerPersonalCapacitadoNoAsignadoXCargoList(ByVal TrabajoId As Int64, ByVal Cargo As Int64) As List(Of TH_Personas)
        Dim LPersonal = (From lper In oMatrixContext.TH_Capacitaciones
                       From lperp In oMatrixContext.TH_CapacitacionesParticipantes
                       Where lper.TrabajoId = TrabajoId And lper.id = lperp.CapacitacionId
                       Select lperp.Participante)
        Dim LAsignado = (From lper In ObtenerPersonalAsignadoList(TrabajoId)
                         Select lper.Persona)
        'Dim lpersonas As New List(Of TH_Personas)
        Dim lpersonas = (From lthper In oMatrixContext.TH_Personas.ToList
                     Where LPersonal.Contains(lthper.id) And Not (LAsignado.Contains(lthper.id)) And lthper.Activo = True
                     Select lthper)
        Return lpersonas.Where(Function(x) x.Cargo = Cargo).ToList
    End Function

    Public Function ListadoPersonasAsignadas(ByVal TrabajoId As Int64) As List(Of OP_ListadoPersonasAsignadasTrabajo_Result)
        Return oMatrixContext.OP_ListadoPersonasAsignadasTrabajo(TrabajoId).ToList
    End Function

    Public Function ListadoPersonalSinProduccion(ByVal Fecha As Date, Cargo As Int64) As List(Of OP_PersonalSinProduccion_Result)
        Return oMatrixContext.OP_PersonalSinProduccion(Fecha, Cargo).ToList
    End Function
#End Region

#Region "Guardar"
    Public Sub GuardarPersonalAsignado(ByVal Entidad As OP_PersonasAsignadasTrabajo)
        If oMatrixContext.OP_PersonasAsignadasTrabajo.Where(Function(x) x.id = Entidad.id).ToList.Count = 0 Then
            oMatrixContext.OP_PersonasAsignadasTrabajo.Add(Entidad)
            oMatrixContext.SaveChanges()
        Else
            Dim E1 As New OP_PersonasAsignadasTrabajo
            E1 = ObtenerPersonalAsignadoXId(Entidad.id)
            E1.TrabajoId = Entidad.TrabajoId
            E1.Persona = Entidad.Persona
            oMatrixContext.SaveChanges()
        End If

    End Sub

#End Region

#Region "Eliminar"
    Public Sub EliminarPersonalAsignado(ByVal cedula As Int64, ByVal trabajo As Int64)
        Dim e As New OP_PersonasAsignadasTrabajo
        e = ObtenerPersonalAsignadoXPersonaYTrabajo(cedula, trabajo)
        oMatrixContext.OP_PersonasAsignadasTrabajo.Remove(e)
        oMatrixContext.SaveChanges()
    End Sub
#End Region
End Class
