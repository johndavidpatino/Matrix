Imports System.Data.Objects

<Serializable()>
Public Class CampoCualitativo
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
    Public Function ObtenerCampoCualiXId(ByVal id As Int64) As OP_CampoCuali
        Return oMatrixContext.OP_CampoCuali.Where(Function(x) x.id = id).FirstOrDefault
    End Function
#End Region
#Region "Guardar"
    Public Function GuardarSegmento(ByRef Ent As OP_CampoCuali) As Int64
        If Ent.id = 0 Then
            oMatrixContext.AddObject("OP_CampoCuali", Ent)
            oMatrixContext.SaveChanges()
            Return Ent.id
        Else
            Dim e As New OP_CampoCuali
            e = ObtenerCampoCualiXId(Ent.id)
            e.id = Ent.id
            e.SegmentoId = Ent.SegmentoId
            e.Moderador = Ent.Moderador
            e.PlaneacionPor = Ent.PlaneacionPor
            e.Lugar = Ent.Lugar
            e.PersonaContaco = Ent.PersonaContaco
            e.DatosContacto = Ent.DatosContacto
            e.Direccion = Ent.Direccion
            e.Fecha = Ent.Fecha
            e.Hora = Ent.Hora
            e.ObservacionesPrevias = Ent.ObservacionesPrevias
            e.FechaReal = Ent.FechaReal
            e.HoraReal = Ent.HoraReal
            e.Asistentes = Ent.Asistentes
            e.AsistentesNoCumplen = Ent.AsistentesNoCumplen
            e.ObservacionesEjecucion = Ent.ObservacionesEjecucion
            e.EjecucionPor = Ent.EjecucionPor
            e.Caida = Ent.Caida
            e.Cancelada = Ent.Cancelada
            oMatrixContext.SaveChanges()
            Return e.id
        End If
    End Function
#End Region
End Class
