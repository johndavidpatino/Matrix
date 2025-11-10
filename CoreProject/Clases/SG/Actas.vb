

'Imports CoreProject.SG_Model
Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

Namespace SG
    Public Class Actas

        Private _sgcontext As SG_Entities

        Public Property SGContext As SG_Entities
            Get
                If Me._sgcontext Is Nothing Then
                    Me._sgcontext = New SG_Entities()
                End If
                Return Me._sgcontext
            End Get
            Set(value As SG_Entities)
                Me._sgcontext = value
            End Set
        End Property

        Public Function GuardarActa(Denominacion As String, NoActa As Integer, TipoId As Integer, UnidadId As Integer, Secretario As Integer, Lider As Integer, SeguimientoCompromisos As String, SeguimientoAcciones As String, TemasTratados As String, CompromisosConclusiones As String, Activa As Boolean, Form As Integer, FormId As Integer, Publica As Boolean) As Integer
            Try
                Try
                    Dim Res As ObjectResult(Of SG_Acta_Add_Result)
                    Res = SGContext.SG_Acta_Add(Denominacion, NoActa, TipoId, UnidadId, Secretario, Lider, SeguimientoCompromisos, SeguimientoAcciones, TemasTratados, CompromisosConclusiones, Activa, Form, FormId, Publica, Date.Now)
                    'devuelvo el ultimo id ingresado en la tabla de Actas
                    Return Res(0).id
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

        Public Function EditarActa(id As Integer, Denominacion As String, NoActa As Integer, TipoId As Integer, UnidadId As Integer, Secretario As Integer, Lider As Integer, SeguimientoCompromisos As String, SeguimientoAcciones As String, TemasTratados As String, CompromisosConclusiones As String, Activa As Boolean, Form As Integer, FormId As Integer, Publica As Boolean) As Integer
            Try
                Try
                    Return SGContext.SG_Acta_Edit(id, Denominacion, NoActa, TipoId, UnidadId, Secretario, Lider, SeguimientoCompromisos, SeguimientoAcciones, TemasTratados, CompromisosConclusiones, Activa, Form, FormId, Publica)
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

        Public Function EliminarActa(ByVal id As Integer) As Integer
            Try
                Try
                    Return SGContext.SG_Acta_Del(id)
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

        Public Function ObtenerActas() As List(Of SG_Acta_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_Acta_Result) = SGContext.SG_Acta_Get
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

        Public Function ObtenerActas(ByVal PqrId As Integer) As List(Of SG_Acta_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_Acta_Result) = SGContext.SG_Acta_GetPQR(PqrId)
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

        Public Function ConsultarActasXNoActa(ByVal NoActa As Integer) As List(Of SG_Acta_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_Acta_Result) = SGContext.SG_Acta_Get_NoActa(NoActa)
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

        Public Function ConsultarActasXTipoActa(ByVal TipoActa As Integer) As List(Of SG_Acta_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_Acta_Result) = SGContext.SG_Acta_Get_TipoActa(TipoActa)
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

        Public Function ConsultarActasXUnidad(ByVal UnidadId As Integer) As List(Of SG_Acta_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_Acta_Result) = SGContext.SG_Acta_Get_Unidad(UnidadId)
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

        Public Function ObtenerTipoActa() As List(Of TipoActaCombo_Result)
            Try
                Try
                    Dim List As ObjectResult(Of TipoActaCombo_Result) = SGContext.SG_Acta_Get_TipoActa_combo()
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

        'poner en clase unidad
        Public Function ObtenerUnidad() As List(Of UnidadCombo_Result)
            Try
                Try
                    Dim List As ObjectResult(Of UnidadCombo_Result) = SGContext.US_Unidad_Get_Combo
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

        Public Function UltimoNoActa(ByVal TipoId As Integer) As Integer
            Try
                Try
                    Dim Res As ObjectResult(Of UltimoNoActa_Result)
                    Res = SGContext.SG_Acta_UltimoNoActa(TipoId)
                    Return Res(0).MaxId + 1
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

        Public Function ObtenerEstadoActas() As List(Of SG_ActasTareasEstado_Result)
            Try
                Try
                    Dim List As ObjectResult(Of SG_ActasTareasEstado_Result) = SGContext.SG_ActasTareasEstado_Get()
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

    End Class

End Namespace