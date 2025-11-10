
'Imports CoreProject.OP_Cuanti_Model
<Serializable()>
Public Class PlaneacionProduccion
#Region "Variables Globales"
    Private oMatrixContext As OP_Cuanti
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Cuanti
    End Sub
#End Region

#Region "Obtener"
    Public Function ObtenerEstimacionxIdList(ByVal EstimacionId As Int64) As List(Of OP_EstimacionProduccion)
        Return oMatrixContext.OP_EstimacionProduccion.Where(Function(x) x.EstimacionId = EstimacionId).ToList
    End Function

    Public Function ObtenerEstimacionxTrabajo(ByVal EstimacionId As Int64) As OP_EstimacionProduccion
        Return oMatrixContext.OP_EstimacionProduccion.Where(Function(x) x.id = EstimacionId).FirstOrDefault
    End Function

    Public Sub GuardarEstimacion(ByVal id As Int64, cantidad As Int64)
        Dim E1 = ObtenerEstimacionxTrabajo(id)
        If Not (E1.Cantidad = cantidad) Then
            E1.Cantidad = cantidad
            oMatrixContext.SaveChanges()
        End If
    End Sub

    Public Function ObtenerEstimacionCiudadxTrabajoList(ByVal TrabajoId As Int64) As List(Of OP_EstimacionesProduccionCiudad)
        Return oMatrixContext.OP_EstimacionesProduccionCiudad.Where(Function(x) x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerEstimacionCiudadxTrabajo(ByVal EstimacionId As Int64) As OP_EstimacionesProduccionCiudad
        Return oMatrixContext.OP_EstimacionesProduccionCiudad.Where(Function(x) x.id = EstimacionId).FirstOrDefault
    End Function

    Public Sub AgregarEstimacionAutomatica(ByVal TrabajoId As Int64, UsuarioId As Int64, lunes As Boolean, martes As Boolean, miercoles As Boolean, jueves As Boolean, viernes As Boolean, sabado As Boolean, domingo As Boolean, festivos As Boolean)
        oMatrixContext.OP_PlaneaccionProduccionAutomatica(TrabajoId, UsuarioId, lunes, martes, miercoles, jueves, viernes, sabado, domingo, festivos)
    End Sub

    Public Sub ActualizarFechasOP_Muestra(ByVal TrabajoId As Int64, FechaI As Date, FechaF As Date)
        oMatrixContext.OP_MuestraTrabajosUpdateFechas(TrabajoId, FechaI, FechaF)
    End Sub

    Public Sub AgregarEstimacionManual(ByVal TrabajoId As Int64, UsuarioId As Int64, lunes As Boolean, martes As Boolean, miercoles As Boolean, jueves As Boolean, viernes As Boolean, sabado As Boolean, domingo As Boolean, festivos As Boolean, estimacionid As Int64)
        oMatrixContext.OP_PlaneaccionProduccionManual(TrabajoId, UsuarioId, lunes, martes, miercoles, jueves, viernes, sabado, domingo, festivos, estimacionid)
    End Sub

    Public Function AgregarEstimacionCiudad(ByVal TrabajoId As Int64, UsuarioId As Int64, lunes As Boolean, martes As Boolean, miercoles As Boolean, jueves As Boolean, viernes As Boolean, sabado As Boolean, domingo As Boolean, festivos As Boolean, Observaciones As String, Ciudad As Int32) As OP_EstimacionesProduccionCiudad
        Dim E As New OP_EstimacionesProduccionCiudad
        E.TrabajoId = TrabajoId
        E.FechaEstimacion = Now
        E.CiudadId = Ciudad
        E.UsuarioEstimacion = UsuarioId
        E.Observaciones = Observaciones
        E.Bloqueada = True
        E.Activa = False
        oMatrixContext.OP_EstimacionesProduccionCiudad.Add(E)
        oMatrixContext.SaveChanges()
        AgregarEstimacionManual(TrabajoId, UsuarioId, lunes, martes, miercoles, jueves, viernes, sabado, domingo, festivos, E.id)
        Return E
    End Function

    Public Sub ActivarEstimacion(ByVal EstimacionId As Int64)
        oMatrixContext.OP_Planeacion_ActivarEstimacion(EstimacionId)
    End Sub

    Public Sub ActualizarFechasCiudad(ByVal IdMuestra As Int64, lunes As Boolean, martes As Boolean, miercoles As Boolean, jueves As Boolean, viernes As Boolean, sabado As Boolean, domingo As Boolean, festivos As Boolean)
        oMatrixContext.OP_AjusteProduccionAutoCiudad(IdMuestra, lunes, martes, miercoles, jueves, viernes, sabado, domingo, festivos)
    End Sub
    Public Function ObtenerEstimacionOPTrafico(ByVal TrabajoId As Int64) As OP_ProduccionEstimadaOPTrafico
        Return oMatrixContext.OP_ProduccionEstimadaOPTrafico.Where(Function(x) x.id = TrabajoId).FirstOrDefault
    End Function
    Public Sub GuardarEstimacionInicialOPTrafico(ByVal TrabajoId As Int64)
        Dim Ent As New OP_ProduccionEstimadaOPTrafico
        Ent.id = TrabajoId
        Ent.RMC = 300
        Ent.Critica = 50
        Ent.Verificacion = 20
        Ent.Captura = 80
        oMatrixContext.OP_ProduccionEstimadaOPTrafico.Add(Ent)
        oMatrixContext.SaveChanges()
    End Sub
    Public Sub GuardarEstimacionOpTrafico(ByVal Entidad As OP_ProduccionEstimadaOPTrafico)
        Dim Ent As New OP_ProduccionEstimadaOPTrafico
        Ent = ObtenerEstimacionOPTrafico(Entidad.id)
        Ent.RMC = Entidad.RMC
        Ent.Critica = Entidad.Critica
        Ent.Verificacion = Entidad.Verificacion
        Ent.Captura = Entidad.Captura
        oMatrixContext.SaveChanges()
    End Sub
#End Region
End Class
