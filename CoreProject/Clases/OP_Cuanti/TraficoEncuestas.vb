

'Imports CoreProject.OP_Model
Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

Namespace OP
    Public Class TraficoEncuestas
        Private _OPcontext As OP_Entities

        Public Property OPContext As OP_Entities
            Get
                If Me._OPcontext Is Nothing Then
                    Me._OPcontext = New OP_Entities()
                End If
                Return Me._OPcontext
            End Get
            Set(value As OP_Entities)
                Me._OPcontext = value
            End Set
        End Property

        Public Function ObtenerTraficoEncuestasXTrabajo(ByVal TrabajoID As Integer) As List(Of OP_TraficoEncuestasCiudad_Result)
            Try
                Try
                    Dim List As ObjectResult(Of OP_TraficoEncuestasCiudad_Result) = OPContext.OP_TraficoEncuestasCiudad(TrabajoID)
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerEncuestasDisponibles(ByVal TrabajoId As Int64, UnidadEnvia As Int64) As List(Of OP_TraficoArhivos_GetDisponibleEnvio_Result)
            Try
                Try
                    Dim List As ObjectResult(Of OP_TraficoArhivos_GetDisponibleEnvio_Result) = OPContext.OP_TraficoArhivos_GetDisponibleEnvio(TrabajoId, UnidadEnvia)
                    Return List.ToList
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerEncuestasDisponiblesDevol(ByVal TrabajoId As Int64, UnidadEnvia As Int64) As List(Of OP_TraficoArhivos_GetDisponibleDevolucion_Result)
            Try
                Try
                    Dim List As ObjectResult(Of OP_TraficoArhivos_GetDisponibleDevolucion_Result) = OPContext.OP_TraficoArhivos_GetDisponibleDevolucion(TrabajoId, UnidadEnvia)
                    Return List.ToList
                Catch ex As Exception
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function
        Public Function ExistsTRaficoEnCuestas(ByVal id As Int64) As Boolean
            If OPContext.OP_TraficoEncuestas.Where(Function(x) x.id = id).ToList.Count = 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Function ObtenerTraficoEncuestaXId(ByVal id As Int64) As OP_TraficoEncuestas
            Return OPContext.OP_TraficoEncuestas.Where(Function(x) x.id = id).FirstOrDefault
        End Function

        Public Function ObtenerEnviosXUnidadEnviaYTrabajo(ByVal TrabajoId As Int64, Unidad As Int32) As List(Of OP_TraficoEncuestas_Get_Result)
            Return OPContext.OP_TraficoEncuestas_Get(TrabajoId, Unidad, Nothing).ToList
        End Function
        Public Function ObtenerEnviosXUnidadRecibeYTrabajo(ByVal TrabajoId As Int64, Unidad As Int32) As List(Of OP_TraficoEncuestas_Get_Result)
            Return OPContext.OP_TraficoEncuestas_Get(TrabajoId, Nothing, Unidad).ToList
        End Function
        Public Function ObtenerEnviosXUnidadYTrabajo(ByVal TrabajoId As Int64, UnidadEnvia As Int32, UnidadRecibe As Int32) As List(Of OP_TraficoEncuestas_Get_Result)
            Return OPContext.OP_TraficoEncuestas_Get(TrabajoId, UnidadEnvia, UnidadRecibe).ToList
        End Function

        Public Function ObtenerEnviosListadoGet(ByVal TrabajoId As Int64, UnidadEnvia As Int32, UnidadRecibe As Int32) As List(Of OP_TraficoEncuestas_ListadoGet_Result)
            Return OPContext.OP_TraficoEncuestas_ListadoGet(UnidadEnvia, UnidadRecibe, TrabajoId).ToList
        End Function
        Public Function ObtenerMuestraEnviadaCiudadRMC(ByVal TrabajoId As Int64) As List(Of OP_TraficoEncuestasMuestraCiudadesRMC_Result)
            Return OPContext.OP_TraficoEncuestasMuestraCiudadesRMC(TrabajoId).ToList
        End Function
#Region "Guardar"
        Public Sub GuardarTraficoEnvio(ByVal Entidad As OP_TraficoEncuestas)
            If ExistsTRaficoEnCuestas(Entidad.id) = True Then
                Dim Ent As New OP_TraficoEncuestas
                Ent = ObtenerTraficoEncuestaXId(Entidad.id)
                Ent.UsuarioRecibe = Entidad.UsuarioRecibe
                Ent.FechaRecibo = Entidad.FechaRecibo
                Ent.ObservacionesRecibo = Entidad.ObservacionesRecibo
                Ent.Cantidad = Entidad.Cantidad
                OPContext.SaveChanges()
            Else
                OPContext.OP_TraficoEncuestas.Add(Entidad)
                OPContext.SaveChanges()
            End If
        End Sub
#End Region
#Region "Eliminar"
        Public Sub BorrarEnvio(ByVal idEnvio As Int64)
            OPContext.OP_TraficoEncuestasBorrarEnvio(idEnvio)
        End Sub
#End Region
    End Class
End Namespace