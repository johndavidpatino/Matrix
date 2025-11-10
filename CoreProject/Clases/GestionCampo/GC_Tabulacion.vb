Imports System.Data.SqlClient


Namespace GestionCampo
    Public Class GC_Tabulacion
        Dim cnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("GestionCampoConnectionString").ConnectionString

        Public Function ObtenerPreguntasTabular(Estudio As Decimal) As DataSet

            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("ObtenerPreguntasTabular", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.CommandTimeout = 0
                    command.Parameters.Add("E_Id", SqlDbType.Int).Value = Estudio
                    command.Connection.Open()

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerRespuestasTabular(Estudio As Decimal) As DataSet

            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("ObtenerRespuestaTabular", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("E_Id", SqlDbType.Decimal).Value = Estudio
                    command.Connection.Open()

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Function ObtenerEstudios() As DataSet

            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("ObtenerListaEstudiosTablet", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerNombresPreguntas(Estudio As Decimal) As DataSet

            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("ObtenerNombresPreguntasTabular", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("E_Id", SqlDbType.Decimal).Value = Estudio
                    command.Connection.Open()

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Function ObtenerPreguntasTabularEstudio(Estudio As Decimal) As DataSet

            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("ObtenerPreguntasTabularEstudio", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("E_Id", SqlDbType.Decimal).Value = Estudio
                    command.Connection.Open()

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Sub InsertarPreguntasTabular(Estudio As Decimal, Pregnta As Integer)

            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("InsertarPreguntasTabular", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("E_Id", SqlDbType.Decimal).Value = Estudio
                    command.Parameters.Add("Pr_Id", SqlDbType.Decimal).Value = Pregnta
                    command.Connection.Open()

                    command.ExecuteNonQuery()
                End Using


            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub BorrarPreguntaTabular(Estudio As Decimal, Pregnta As Integer)

            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("BorrarPreguntaTabular", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("E_Id", SqlDbType.Decimal).Value = Estudio
                    command.Parameters.Add("Pr_Id", SqlDbType.Decimal).Value = Pregnta
                    command.Connection.Open()

                    command.ExecuteNonQuery()
                End Using


            Catch ex As Exception
                Throw ex
            End Try
        End Sub

    End Class

End Namespace

