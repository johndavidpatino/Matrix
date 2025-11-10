
''Imports CoreProject.OP_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class FichaEntrevistas
#Region "Variables Globales"
    Private oMatrixContext As OP_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New OP_Entities
    End Sub
#End Region
#Region "Obtener"
    Public Function DevolverTodos() As List(Of OP_FichaEntrevistas_Get_Result)
        Try
            Return OP_FichaEntrevistas_Get(Nothing, Nothing)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxTrabajoID(ByVal TrabajoID As Int64?) As List(Of OP_FichaEntrevistas_Get_Result)
        Try
            Return OP_FichaEntrevistas_Get(Nothing, TrabajoID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxID(ByVal ID As Int64) As OP_FichaEntrevistas_Get_Result
        Try
            Dim oResult As List(Of OP_FichaEntrevistas_Get_Result)
            oResult = OP_FichaEntrevistas_Get(ID, Nothing)
            Return oResult.Item(0)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function OP_FichaEntrevistas_Get(ByVal ID As Int64, ByVal TrabajoID As Int64) As List(Of OP_FichaEntrevistas_Get_Result)
        Try
            Dim oResult As ObjectResult(Of OP_FichaEntrevistas_Get_Result) = oMatrixContext.OP_FichaEntrevistas_Get(ID, TrabajoID)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
#Region "Guardar"
    Public Function Guardar(ByVal ID As Int64?, ByVal TrabajoId As Int64?, ByVal CantidadRequerida As Short?, ByVal FlashReport As Boolean?, ByVal DescripcionIncentivos As String,
                            ByVal IncentivoEconomico As Boolean?, ByVal PresupuestoIncentivo As Double?, ByVal RegalosCliente As Boolean?, ByVal CompraIpsos As Boolean?,
                             ByVal Presupuesto As Double?, ByVal CircuitoCerrado As Boolean?, ByVal FilmacionFija As Boolean?, ByVal CamaraFotografica As Boolean?,
                            ByVal Tv_DVD As Boolean?, ByVal FilmacionActiva As Boolean?, ByVal VideoBeam As Boolean?,
                            ByVal EntregaFiltrosReclutamiento As Boolean?, ByVal EntregaFiltrosAsistente As Boolean?, ByVal EntregaCartaInvitacion As Boolean?, ByVal EntregaFaxConfirmacion As Boolean?,
                            ByVal ListadosCliente As Boolean?, ByVal CallCenter As Boolean?, ByVal SacaCita As Boolean?, ByVal FlashReportEscrito As Boolean?, ByVal FlashReportVerbal As Boolean?,
                            ByVal Transcripcion As Boolean?, ByVal Grabacion As Boolean?,
                            ByVal GrupoObjetivo As String, ByVal CaracteristicasEspeciales As String, ByVal Comentarios As String, ByVal MetodoAceptableReclutamiento As String, ByVal ExclusionesYRestriccionesEspecificas As String, ByVal RecursosPropiedadCliente As String, ByVal Observaciones As String) As Decimal
        Try
            Dim FichaEntrevistaID As Decimal = 0

            If ID > 0 Then
                oMatrixContext.OP_FichaEntrevistas_Edit(ID, TrabajoId, CantidadRequerida, FlashReport, DescripcionIncentivos, IncentivoEconomico, PresupuestoIncentivo, RegalosCliente, CompraIpsos, Presupuesto, CircuitoCerrado, FilmacionFija, CamaraFotografica,
                             Tv_DVD, FilmacionActiva, VideoBeam, EntregaFiltrosReclutamiento, EntregaFiltrosAsistente, EntregaCartaInvitacion, EntregaFaxConfirmacion, ListadosCliente, CallCenter, SacaCita, FlashReportEscrito, FlashReportVerbal, Transcripcion, Grabacion,
                             GrupoObjetivo, CaracteristicasEspeciales, Comentarios, MetodoAceptableReclutamiento, ExclusionesYRestriccionesEspecificas, RecursosPropiedadCliente, Observaciones)
                FichaEntrevistaID = ID
            Else
                Dim oResult As ObjectResult(Of Decimal?)
                oResult = oMatrixContext.OP_FichaEntrevistas_Add(TrabajoId, CantidadRequerida, FlashReport, DescripcionIncentivos, IncentivoEconomico, PresupuestoIncentivo, RegalosCliente, CompraIpsos, Presupuesto, CircuitoCerrado, FilmacionFija, CamaraFotografica,
                             Tv_DVD, FilmacionActiva, VideoBeam, EntregaFiltrosReclutamiento, EntregaFiltrosAsistente, EntregaCartaInvitacion, EntregaFaxConfirmacion, ListadosCliente, CallCenter, SacaCita, FlashReportEscrito, FlashReportVerbal, Transcripcion, Grabacion,
                             GrupoObjetivo, CaracteristicasEspeciales, Comentarios, MetodoAceptableReclutamiento, ExclusionesYRestriccionesEspecificas, RecursosPropiedadCliente, Observaciones)
                FichaEntrevistaID = Decimal.Parse(oResult.ToList().Item(0))
            End If

            Return FichaEntrevistaID
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
#Region "Eliminar"
    Public Function Eliminar(ByVal ID As Int64) As Integer
        Try
            Return oMatrixContext.OP_FichaEntrevistas_Del(ID)
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
#End Region
End Class
