

'Imports CoreProject.OP_Model
Imports System.Data.Entity.Core.Objects
Imports System.Data.SqlClient

Namespace OP
    Public Class SupervisionCampoTelefonico
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

        Public Function AgregarSupervisionCampoTelefonico(TrabajoId As Integer, NoEstudio As Integer, Supervisor As Integer, Operador As Integer, FechaSupervision As DateTime, CRI01 As Boolean, CRI02 As Boolean, CRI03 As Boolean, CRI04 As Boolean, CRI05 As Boolean, CRI06 As Boolean, CRI07 As Boolean, CRI08 As Boolean, CRI09 As Boolean, CRI10 As Boolean, CRI11 As Boolean, CRI12 As Boolean, CRI13 As Boolean, COM01 As Integer, COM02 As Integer, COM03 As Integer, COM04 As Integer, ACC01 As Integer, ACC02 As Integer, ACC03 As Integer, ACC04 As Integer, Observacion As String) As Integer
            Try
                Try
                    Return OPContext.OP_SupervisionCampoTelefonico_Add(TrabajoId, NoEstudio, Supervisor, Operador, FechaSupervision, CRI01, CRI02, CRI03, CRI04, CRI05, CRI06, CRI07, CRI08, CRI09, CRI10, CRI11, CRI12, CRI13, COM01, COM02, COM03, COM04, ACC01, ACC02, ACC03, ACC04, Observacion)
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

        'Public Function ObtenerGeneralidades() As List(Of f_Generalidades_Get_Result)
        '    Try
        '        Try
        '            Dim List As ObjectResult(Of f_Generalidades_Get_Result) = OPContext.f_Generalidades_Get
        '            Return List.ToList()
        '        Catch ex As SqlException
        '            Throw ex
        '        End Try
        '    Catch ex As Exception
        '        If IsNothing(ex.InnerException) Then
        '            Throw ex
        '        Else
        '            Throw ex.InnerException
        '        End If
        '    End Try
        'End Function

        Public Function ObtenerMaxNoEstudio(ByVal trabajoId As Integer) As Integer
            Try
                Try
                    Dim List As ObjectResult(Of OP_SupervisionNoEstudioMax_Get_Result) = OPContext.OP_SupervisionNoEstudioMax_Get(trabajoId)
                    Return List.ToList(0).NoEstudio
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