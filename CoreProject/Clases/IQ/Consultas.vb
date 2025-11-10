
Imports System.Data.SqlClient

Namespace IQ


    Public Class Consultas
        Dim _IQ_Entities As IQ_MODEL
        Dim cnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ConnectionString

        Public Function ObtenerCostosJobBookExterno(ByVal P As IQ_Parametros) As DataSet
            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_CostosJobBookExterno", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = P.IdPropuesta
                    command.Parameters.Add("ParAlternativa", SqlDbType.Int).Value = P.ParAlternativa
                    command.Parameters.Add("ParNacional", SqlDbType.Int).Value = P.ParNacional
                    command.Parameters.Add("MetCodigo", SqlDbType.Int).Value = P.MetCodigo

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerCostosJobBookExternoObserver(ByVal P As IQ_Parametros) As DataSet
            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_CostosJobBookExternoObserver", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = P.IdPropuesta
                    command.Parameters.Add("ParAlternativa", SqlDbType.Int).Value = P.ParAlternativa
                    command.Parameters.Add("ParNacional", SqlDbType.Int).Value = P.ParNacional
                    command.Parameters.Add("MetCodigo", SqlDbType.Int).Value = P.MetCodigo

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerCostosJobBookInterno(ByVal P As IQ_Parametros) As DataSet
            Try


                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_CostosJobBookInterno", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = P.IdPropuesta
                    command.Parameters.Add("ParAlternativa", SqlDbType.Int).Value = P.ParAlternativa
                    command.Parameters.Add("ParNacional", SqlDbType.Int).Value = P.ParNacional
                    command.Parameters.Add("MetCodigo", SqlDbType.Int).Value = P.MetCodigo

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerCostoOperacion(ByVal P As IQ_Parametros) As Decimal
            Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
            Dim Costo As Decimal = 0.0
            Using command As New SqlClient.SqlCommand("IQ_CostoOperacionJBI", SQLcon)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.Open()
                command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = P.IdPropuesta
                command.Parameters.Add("ParAlternativa", SqlDbType.Int).Value = P.ParAlternativa
                command.Parameters.Add("ParNacional", SqlDbType.Int).Value = P.ParNacional
                command.Parameters.Add("MetCodigo", SqlDbType.Int).Value = P.MetCodigo

                Costo = Convert.ToDecimal(command.ExecuteScalar())

            End Using

            Return Costo
        End Function

        Public Function ObtenerProfesionalFeesExterno(ByVal P As IQ_Parametros) As DataSet
            Try


                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_ProfesionalFeesExterno", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = P.IdPropuesta
                    command.Parameters.Add("Alternativa", SqlDbType.Int).Value = P.ParAlternativa

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerProfesionalFeesInterno(ByVal P As IQ_Parametros) As DataSet
            Try


                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_ProfesionalFeesInterno", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = P.IdPropuesta
                    command.Parameters.Add("Alternativa", SqlDbType.Int).Value = P.ParAlternativa

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerProfesionalTimesExtreno(ByVal P As IQ_Parametros) As DataSet
            Try


                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_ProfesionalTimeHorasExterno", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = P.IdPropuesta
                    command.Parameters.Add("Alternativa", SqlDbType.Int).Value = P.ParAlternativa

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerProfesionalTimesInterno(ByVal P As IQ_Parametros) As DataSet
            Try


                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_ProfesionalTimeHorasInterno", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = P.IdPropuesta
                    command.Parameters.Add("Alternativa", SqlDbType.Int).Value = P.ParAlternativa

                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerAutorizadoCambioGrossMargin(ByVal Unidad As Int32) As Int64
            Dim iqent As New IQ_MODEL
            Return iqent.IQ_UsuarioAutorizadoModificacionGM(Unidad).First
            'Dim info = _IQ_Entities.IQ_AutorizadosModificacionGM.Where(Function(x) x.Unidad = Unidad).FirstOrDefault
            'Return info.IdUsuario
        End Function

        Public Function ObtenerAutorizadosCambioGrossMargin(ByVal Unidad As Int32) As List(Of Long?)
            Dim iqent As New IQ_MODEL
            Return iqent.IQ_UsuarioAutorizadoModificacionGM(Unidad).ToList
            'Dim info = _IQ_Entities.IQ_AutorizadosModificacionGM.Where(Function(x) x.Unidad = Unidad).FirstOrDefault
            'Return info.IdUsuario
        End Function

        Public Function ObtenerAutorizadosCambioGrossMarginTable(ByVal Unidad As Int32, ByVal Valor As Long) As List(Of IQ_AutorizadosModificacionGM)
            Dim iqent As New IQ_MODEL
            Return iqent.IQ_AutorizadosModificacionGM.Where(Function(x) x.Unidad = Unidad And Valor >= x.MontoMinimo).ToList()
            'Dim info = _IQ_Entities.IQ_AutorizadosModificacionGM.Where(Function(x) x.Unidad = Unidad).FirstOrDefault
            'Return info.IdUsuario
        End Function

        Public Sub CambiarJBI(ByVal idTrabajo As Int64?, ByVal fase As Int32?, ByVal jobBook As String)
            Dim iqent As New IQ_MODEL
            iqent.IQ_CambiosJobBook_WORK(idTrabajo, fase, jobBook)
        End Sub

        Public Function FasesList() As List(Of IQ_Fases)
            Dim iqent As New IQ_MODEL
            Return iqent.IQ_Fases.ToList
        End Function

        Public Function GuardarLogCambiosJBI(ByVal IdTrabajo As Int64?, ByVal JBI_Anterior As String, ByVal JBI_Nuevo As String, ByVal Usuario As Int64?) As Decimal
            Dim iqent As New IQ_MODEL
            Return iqent.IQ_LogCambiosJBI_Add(IdTrabajo, JBI_Anterior, JBI_Nuevo, Usuario).FirstOrDefault
        End Function

        Public Function ActividadesList() As List(Of IQ_ActividadesGet_Result)
            Dim iqent As New IQ_MODEL
            Return iqent.IQ_ActividadesGet().ToList
        End Function

        Public Function ObtenerCostosActividades(ByVal idPropuesta As Int64, ByVal parAlternativa As Int32, ByVal metCodigo As Int32, ByVal parNacional As Int32) As List(Of IQ_ObtenerCostoActividades_Result)
            Dim iqent As New IQ_MODEL
            Return iqent.IQ_ObtenerCostoActividades(idPropuesta, parAlternativa, metCodigo, parNacional).ToList()
        End Function

        Public Sub AjustarCostosMysteryShopper(ByVal idPropuesta As Int64, ByVal parAlternativa As Int32, ByVal metCodigo As Int32, ByVal parNacional As Int32, ByVal gmOPS As Decimal?, ByVal subInternas As Decimal?, ByVal ventaOPS As Decimal?, ByVal subExternas As Decimal?, ByVal valorVenta As Decimal?, ByVal porcentajeGM As Double?)
            Dim iqent As New IQ_MODEL
            iqent.AJ_AjusteCostosMysteryShopper_Matrix(idPropuesta, parAlternativa, metCodigo, parNacional, gmOPS, subInternas, ventaOPS, subExternas, valorVenta, porcentajeGM)
        End Sub

		Public Function ObtenerParametros(idPropuesta As Int64, parAlternativa As Int32, metCodigo As Int32, parNacional As Int32) As List(Of IQ_Parametros)

			_IQ_Entities = New IQ_MODEL

			Dim parametros = _IQ_Entities.IQ_Parametros.Where(Function(x) x.IdPropuesta = idPropuesta AndAlso x.ParAlternativa = parAlternativa AndAlso x.MetCodigo = metCodigo AndAlso x.ParNacional = parNacional).ToList()

			Return parametros
		End Function

	End Class



End Namespace

