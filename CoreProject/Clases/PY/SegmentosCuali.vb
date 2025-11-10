
'Imports CoreProject.PY_Cuali_Model

<Serializable()>
Public Class SegmentosCuali
#Region "Variables Globales"
    Private oMatrixContext As PY_CualiEntities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New PY_CualiEntities
    End Sub
#End Region

#Region "Obtener"
    Public Function ObtenerSegmentoXId(ByVal id As Int64) As PY_SegmentosCuali
        Return oMatrixContext.PY_SegmentosCuali.Where(Function(x) x.id = id).FirstOrDefault
    End Function

    Public Function ObtenerAyudasCuali() As List(Of PY_AyudasCuali)
        Return oMatrixContext.PY_AyudasCuali.ToList
    End Function

    Public Function ObtenerTipoReclutamiento() As List(Of PY_TipoReclutamientoCuali)
        Return oMatrixContext.PY_TipoReclutamientoCuali.ToList
    End Function

    Public Function ObtenerAyudasRequeridasCuali(ByVal TrabajoId As Int64, ByVal AyudaId As Int32) As PY_AyudasRequeridasCuali
        Return oMatrixContext.PY_AyudasRequeridasCuali.Where(Function(x) x.TrabajoId = TrabajoId And x.TipoAyuda = AyudaId).FirstOrDefault
    End Function

    Public Function ObtenerAyudasRequeridasCualiList(ByVal TrabajoId As Int64) As List(Of PY_AyudasRequeridasCuali)
        Return oMatrixContext.PY_AyudasRequeridasCuali.Where(Function(x) x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerReclutamientoRequeridoCuali(ByVal TrabajoId As Int64, ByVal TipoReclutamiento As Int32) As PY_ReclutamientoRequeridoCuali
        Return oMatrixContext.PY_ReclutamientoRequeridoCuali.Where(Function(x) x.TrabajoId = TrabajoId And x.TipoReclutamiento = TipoReclutamiento).FirstOrDefault
    End Function

    Public Function ObtenerReclutamientoRequeridoCualiList(ByVal TrabajoId As Int64) As List(Of PY_ReclutamientoRequeridoCuali)
        Return oMatrixContext.PY_ReclutamientoRequeridoCuali.Where(Function(x) x.TrabajoId = TrabajoId).ToList
    End Function

    Public Function ObtenerLugaresCuali() As List(Of PY_LugaresCuali)
        Return oMatrixContext.PY_LugaresCuali.ToList
    End Function

    Public Function ObtenerNombreLugarCuali(ByVal idLugar As Int16) As String
        Return oMatrixContext.PY_LugaresCuali.Where(Function(x) x.id = idLugar).FirstOrDefault.Lugar
    End Function

    Public Function ObtenerSegmentosList(ByVal TrabajoID As Int64) As List(Of PY_SegmentosCuali_Get_Result)
        Return oMatrixContext.PY_SegmentosCuali_Get(TrabajoID).ToList
    End Function

    Public Function Duplicar(ByVal idSegmento As Int64) As Decimal
        Return oMatrixContext.PY_SegmentosCualiDuplicar(idSegmento)(0).Value
    End Function

    Public Function ObtenerEspecifacionesCuali(ByVal IDTrabajo As Int64) As PY_EspecifTecTrabajoCuali
        If oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo).ToList.Count = 0 Then
            Return New PY_EspecifTecTrabajoCuali
        Else
            Return oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo).First
        End If
    End Function

    Public Function ObtenerEspecifacionesCualiLast(ByVal IDTrabajo As Int64) As PY_EspecifTecTrabajoCuali
        If oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo).ToList.Count = 0 Then
            Return New PY_EspecifTecTrabajoCuali
        Else
            Dim EspTecCuali = oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo).OrderByDescending(Function(x) x.NoVersion).First
            Return EspTecCuali
        End If
    End Function
    Public Function ObtenerEspecifacionesCualiList(ByVal IDTrabajo As Int64) As List(Of PY_EspecifTecTrabajoCuali)
        If oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo).ToList.Count = 0 Then
            Return New List(Of PY_EspecifTecTrabajoCuali)
        Else
            Dim EspTecCuali = oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo).OrderByDescending(Function(x) x.NoVersion).ToList
            Return EspTecCuali
        End If
    End Function

    Public Function ObtenerEspecifacionesCualiListAsc(ByVal IDTrabajo As Int64) As List(Of PY_EspecifTecTrabajoCuali)
        If oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo).ToList.Count = 0 Then
            Return New List(Of PY_EspecifTecTrabajoCuali)
        Else
            Dim EspTecCuali = oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo).ToList
            Return EspTecCuali
        End If
    End Function


    Function ObtenerEspecifacionesContar(ByVal IDTrabajo As Int64) As Integer
        Return oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo).ToList.Count
    End Function

    Function ObtenerEspecifacionesCualixIdxTr(ByVal IDTrabajo As Int64, ByVal Id As Int64) As List(Of PY_EspecifTecTrabajoCuali)
        Try
            If oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo And x.id = Id).ToList.Count = 0 Then
                Return New List(Of PY_EspecifTecTrabajoCuali)
            Else
                Return oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.TrabajoId = IDTrabajo And x.id = Id).OrderByDescending(Function(x) x.id).ToList
            End If
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Function ObtenerEspecifacionesCualixId(ByVal Id As Int64) As PY_EspecifTecTrabajoCuali
        Try
            If oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.id = Id).ToList.Count = 0 Then
                Return New PY_EspecifTecTrabajoCuali
            Else
                Return oMatrixContext.PY_EspecifTecTrabajoCuali.Where(Function(x) x.id = Id).FirstOrDefault
            End If
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
    Public Function GuardarInfoEspecificacionesCuali(ByRef Ent As PY_EspecifTecTrabajoCuali) As Int64
        oMatrixContext.PY_EspecifTecTrabajoCuali.Add(Ent)
        oMatrixContext.SaveChanges()
        Return Ent.id
    End Function

    Public Function GuardarSegmento(ByRef Ent As PY_SegmentosCuali) As Int64
        If Ent.id = 0 Then
            oMatrixContext.PY_SegmentosCuali.Add(Ent)
            oMatrixContext.SaveChanges()
            Return Ent.id
        Else
            Dim e As New PY_SegmentosCuali
            e = ObtenerSegmentoXId(Ent.id)
            e.id = Ent.id
            e.TrabajoId = Ent.TrabajoId
            e.Descripcion = Ent.Descripcion
            e.Cantidad = Ent.Cantidad
            e.ObservacionesMetodologia = Ent.ObservacionesMetodologia
            e.MetodoReclutamiento = Ent.MetodoReclutamiento
            e.NoReclutadoras = Ent.NoReclutadoras
            e.OtrasAyudas = Ent.OtrasAyudas
            e.FechaInicio = Ent.FechaInicio
            e.FechaFin = Ent.FechaFin
            e.Departamento = Ent.Departamento
            e.Municipio = Ent.Municipio
            e.TipoLugar = Ent.TipoLugar
            e.EspecificacionesLugar = Ent.EspecificacionesLugar
            e.NSE = Ent.NSE
            e.Edades = Ent.Edades
            e.Genero = Ent.Genero
            e.OtrasCaracteristicas = Ent.OtrasCaracteristicas
            e.ExclusionesYRestricciones = Ent.ExclusionesYRestricciones
            e.NoPersonas = Ent.NoPersonas
            e.GastosDeViaje = Ent.GastosDeViaje
            e.Incentivos = Ent.Incentivos
            e.Alimentacion = Ent.Alimentacion
            e.Transcripcion = Ent.Transcripcion
            e.Traduccion = Ent.Traduccion
            e.Video = Ent.Video
            e.Audios = Ent.Audios
            e.Filtros = Ent.Filtros
            e.FlashReport = Ent.FlashReport
            e.OtrosEntregables = Ent.OtrosEntregables
            e.ObservacionesGenerales = Ent.ObservacionesGenerales
            oMatrixContext.SaveChanges()
            Return e.id
        End If
    End Function

    Sub GuardarAyudas(ByVal TrabajoId As Int64, ByVal AyudaId As Int32, incluido As Boolean)
        Dim e As New PY_AyudasRequeridasCuali
        If incluido = False Then
            e = ObtenerAyudasRequeridasCuali(TrabajoId, AyudaId)
            If Not e Is Nothing Then
                oMatrixContext.PY_AyudasRequeridasCuali.Remove(e)
                oMatrixContext.SaveChanges()
            End If
        Else
            e = ObtenerAyudasRequeridasCuali(TrabajoId, AyudaId)
            If e Is Nothing Then
                e = New PY_AyudasRequeridasCuali
                e.TrabajoId = TrabajoId
                e.TipoAyuda = AyudaId
                oMatrixContext.PY_AyudasRequeridasCuali.Add(e)
                oMatrixContext.SaveChanges()
            End If
        End If
    End Sub

    Sub GuardarTipoReclutamiento(ByVal TrabajoId As Int64, ByVal ReclutamientoId As Int32, incluido As Boolean)
        Dim e As New PY_ReclutamientoRequeridoCuali
        If incluido = False Then
            e = ObtenerReclutamientoRequeridoCuali(TrabajoId, ReclutamientoId)
            If Not e Is Nothing Then
                oMatrixContext.PY_ReclutamientoRequeridoCuali.Remove(e)
                oMatrixContext.SaveChanges()
            End If
        Else
            e = ObtenerReclutamientoRequeridoCuali(TrabajoId, ReclutamientoId)
            If e Is Nothing Then
                e = New PY_ReclutamientoRequeridoCuali
                e.TrabajoId = TrabajoId
                e.TipoReclutamiento = ReclutamientoId
                oMatrixContext.PY_ReclutamientoRequeridoCuali.Add(e)
                oMatrixContext.SaveChanges()
            End If
        End If
    End Sub


#End Region
End Class
