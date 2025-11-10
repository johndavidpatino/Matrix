
'Imports CoreProject.IQ_ENTITIES

Imports System.Data.SqlClient
'
Namespace IQ

    Public Class ControlCostos
        Dim op As New IQ_MODEL
        Dim _IQ_Entities As IQ_MODEL
        Dim cnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ConnectionString

        Public Function ObtenerCostos(ByVal par As IQ_Parametros, ByVal TipoDetalle As Integer) As List(Of IQ_ObtenerActControlCostos_Result)

            Try
                _IQ_Entities = New IQ_MODEL

                Dim C As List(Of IQ_ObtenerActControlCostos_Result)
                C = _IQ_Entities.IQ_ObtenerActControlCostos(par.IdPropuesta, par.ParAlternativa, par.ParNacional, par.MetCodigo, TipoDetalle).ToList()
                Return C

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function ObtenerCostosAutorizados(ByVal par As IQ_Parametros) As List(Of IQ_ObtenerControlCostosAutorizados_Result)

            Try
                _IQ_Entities = New IQ_MODEL

                ' Dim C As List(Of IQ_ObtenerControlCostosAutorizados_Result)
                Dim C = _IQ_Entities.IQ_ObtenerControlCostosAutorizados(par.IdPropuesta, par.ParAlternativa, par.ParNacional, par.MetCodigo).ToList()
                Return C

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function ObtenerDatosPresupuesto(ByVal P As IQ_Parametros) As IQ_ObtenerDescPresupuesto_Result

            _IQ_Entities = New IQ_MODEL()

            Return (From d In _IQ_Entities.IQ_ObtenerDescPresupuesto(P.IdPropuesta, P.ParAlternativa, P.ParNacional, P.MetCodigo)).First()

        End Function

        Public Function ObtenerParametros(ByVal idPropuesta As Int64, ByVal parAlternativa As Int32, ByVal parNacional As Int32, ByVal metCodigo As Int32) As List(Of IQ_ObtenerDescPresupuesto_Result)

            _IQ_Entities = New IQ_MODEL()

            Return _IQ_Entities.IQ_ObtenerDescPresupuesto(idPropuesta, parAlternativa, parNacional, metCodigo).ToList()

        End Function

        Public Function ObtenerActividadesCostosAutorizados() As List(Of IQ_Actividades)


            _IQ_Entities = New IQ_MODEL
            Dim Lst As List(Of IQ_Actividades)
            Lst = (From a In _IQ_Entities.IQ_Actividades Where (a.ActTipoValor = "SUBINT" Or a.ActTipoValor = "SUBEXT" Or a.ActTipoValor = "INT" Or a.ActTipoValor = "EXT") And (a.ActAAI <> 7777 And a.ActAAI <> 8888 And a.ActAAI <> 9999) Select a).OrderBy(Function(x) x.ActNombre).ToList()
            Lst.Insert(0, New IQ_Actividades With {.ID = 0, .ActNombre = "Seleccione..."})
            Return Lst


        End Function
        Public Function ObtenerActividadesPresupAprobados(p As IQ_Parametros) As List(Of ActividadDTO)

            _IQ_Entities = New IQ_MODEL
            Dim Lst As List(Of ActividadDTO)
            Lst = (From a In _IQ_Entities.IQ_Actividades _
                   Where (a.ActTipoValor = "SUBINT" Or a.ActTipoValor = "INT" Or a.ActTipoValor = "SUBEXT") _
                    Select New ActividadDTO With {.ActID = a.ID, .ActNombre = a.ActNombre & "(" & a.ActTipoValor & ")"}).OrderBy(Function(x) x.ActNombre).ToList()
            Lst.Insert(0, New ActividadDTO With {.ActID = 0, .ActNombre = "Seleccione..."})
            Return Lst



        End Function


        Public Function ObtenerValorActividad(p As IQ_Parametros, act As IQ_Actividades) As Decimal

            _IQ_Entities = New IQ_MODEL
            Dim valor = (From ca In _IQ_Entities.IQ_CostoActividades Where (ca.ActCodigo = act.ID) And (ca.IdPropuesta = p.IdPropuesta And _
                                                                                         ca.MetCodigo = p.MetCodigo And _
                                                                                         ca.ParAlternativa = p.ParAlternativa And _
                                                                                         ca.ParNacional = p.ParNacional) Select ca.CaCosto).ToList()
            If valor.Count = 0 Then
                Return 0
            Else
                Return CDec(valor(0))
            End If

        End Function

        Public Function ObtenerAjustesCostosOperaciones(p As IQ_Parametros, NuevoValor As Decimal, porcentaje As Decimal, Afecta As Boolean, act As Integer, PorAfectaOperaciones As Decimal) As IQ_AjusteCostoOperaciones_Result
            _IQ_Entities = New IQ_MODEL

            Dim ajuste As IQ_AjusteCostoOperaciones_Result

            ajuste = _IQ_Entities.IQ_AjusteCostoOperaciones(p.IdPropuesta, p.ParAlternativa, p.MetCodigo, p.ParNacional, act, NuevoValor, porcentaje, Afecta, PorAfectaOperaciones).First()

            Return ajuste
        End Function

        Public Function ObtenerValoresActualesAjustar(p As IQ_Parametros, act As Integer) As IQ_AjusteCostoOperacionesObtenerDatos_Result
            _IQ_Entities = New IQ_MODEL

            Dim ajuste As IQ_AjusteCostoOperacionesObtenerDatos_Result

            ajuste = _IQ_Entities.IQ_AjusteCostoOperacionesObtenerDatos(p.IdPropuesta, p.ParAlternativa, p.MetCodigo, p.ParNacional, act).First()

            Return ajuste
        End Function


        Public Sub InsertarCostosAutorizados(ByVal a As IQ_ControlCostos)
            Try
                _IQ_Entities = New IQ_MODEL

                If (_IQ_Entities.IQ_ControlCostos.Any(Function(p) p.IdPropuesta = a.IdPropuesta And p.ParAlternativa = a.ParAlternativa And p.MetCodigo = a.MetCodigo And p.ParNacional = a.ParNacional And p.ID = a.ID And p.Consecutivo = a.Consecutivo)) Then

                    Dim A1 As IQ_ControlCostos
                    A1 = _IQ_Entities.IQ_ControlCostos.First(Function(p) p.IdPropuesta = a.IdPropuesta And p.ParAlternativa = a.ParAlternativa And p.MetCodigo = a.MetCodigo And p.ParNacional = a.ParNacional And p.ID = a.ID And p.Consecutivo = a.Consecutivo)
                    A1.ValorAutorizado = a.ValorAutorizado
                    A1.Fecha = a.Fecha
                    A1.Usuario = a.Usuario
                    A1.Observacion = a.Observacion

                    _IQ_Entities.SaveChanges()
                Else
                    _IQ_Entities.IQ_ControlCostos.Add(a)
                    _IQ_Entities.SaveChanges()
                End If

            Catch ex As Exception
                Throw ex
            End Try



        End Sub


        Public Function ObtenerPresupuestos(ByVal Unidad As Integer, ByVal JB As String, ByVal Tecnica As Integer, ByVal Metodologia As Integer) As DataSet
            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_ObtenerPresupuestosControlCostos", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("Unidad", SqlDbType.Int).Value = Unidad
                    command.Parameters.Add("JobBook", SqlDbType.VarChar).Value = If(JB = "", Nothing, JB)
                    command.Parameters.Add("Tecnica", SqlDbType.Int).Value = Tecnica
                    command.Parameters.Add("Metodologia", SqlDbType.Int).Value = Metodologia
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


        Public Function ObtenerUnidades() As List(Of IQ_ObtenerUnidades_Result)

            _IQ_Entities = New IQ_MODEL
            Dim lst As List(Of IQ_ObtenerUnidades_Result)
            lst = _IQ_Entities.IQ_ObtenerUnidades().ToList()
            lst.Insert(0, New IQ_ObtenerUnidades_Result With {.CodContable = 0, .GrupoUnidad = "Seleccione..."})
            Return lst

        End Function

        Public Function ObtenerTecnicas() As List(Of IQ_ObtenerTecnicas_Result)
            _IQ_Entities = New IQ_MODEL
            Dim lst As List(Of IQ_ObtenerTecnicas_Result)
            lst = _IQ_Entities.IQ_ObtenerTecnicas().ToList()
            lst.Insert(0, New IQ_ObtenerTecnicas_Result With {.TecCodigo = 0, .TecNombre = "Seleccione..."})
            Return lst
        End Function
        Public Function ObtenerTecnicasCuali() As List(Of IQ_ObtenerTecnicas_Result)
            _IQ_Entities = New IQ_MODEL
            Dim lst As List(Of IQ_ObtenerTecnicas_Result)
            lst = _IQ_Entities.IQ_ObtenerTecnicas().Where(Function(o) o.TecCodigo < 600).ToList()
            lst.Insert(0, New IQ_ObtenerTecnicas_Result With {.TecCodigo = 0, .TecNombre = "Seleccione..."})
            Return lst
        End Function

        Public Function ObtenerMetodolgias(ByVal tec As Integer) As List(Of IQ_ObtenerMetodologias_Result)
            _IQ_Entities = New IQ_MODEL
            Dim lst As List(Of IQ_ObtenerMetodologias_Result)
            lst = _IQ_Entities.IQ_ObtenerMetodologias(tec).ToList()
            lst.Insert(0, New IQ_ObtenerMetodologias_Result With {.MetCodigo = 0, .MetNombre = "Seleccione..."})
            Return lst
        End Function

        Public Function ObtenerPreguntasPresupuesto(ByVal p As IQ_Parametros) As IQ_Preguntas
            Try

                _IQ_Entities = New IQ_MODEL

                Dim D = (From preg In _IQ_Entities.IQ_Preguntas Where preg.IdPropuesta = p.IdPropuesta And preg.ParAlternativa = p.ParAlternativa And preg.ParNacional = p.ParNacional And preg.MetCodigo = p.MetCodigo Select preg).FirstOrDefault()
                If D IsNot Nothing Then

                    Return D
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Function ObtenerDetalleAutorizadosXActividad(ByVal C As IQ_ControlCostos) As List(Of IQ_ControlCostos)
            Try

                _IQ_Entities = New IQ_MODEL
                Dim A As List(Of IQ_ControlCostos)
                A = (From o In _IQ_Entities.IQ_ControlCostos Where o.IdPropuesta = C.IdPropuesta And o.ParAlternativa = C.ParAlternativa And o.MetCodigo = C.MetCodigo And o.ParNacional = C.ParNacional And o.ID = C.ID Select o).ToList()
                Return A

            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Sub BorrarDetalleAutoizadosXactividad(ByVal C As IQ_ControlCostos)
            Try

                _IQ_Entities = New IQ_MODEL
                Dim o = _IQ_Entities.IQ_ControlCostos.First(Function(b) b.IdPropuesta = C.IdPropuesta And b.ParAlternativa = C.ParAlternativa And b.MetCodigo = C.MetCodigo And b.ParNacional = C.ParNacional And b.Consecutivo = C.Consecutivo And b.ID = C.ID)
                _IQ_Entities.IQ_ControlCostos.Remove(o)
                _IQ_Entities.SaveChanges()

            Catch ex As Exception

            End Try

        End Sub

        Public Function ObtenerConsecutivoDetalle(ByVal C As IQ_ControlCostos) As Integer
            Try

                _IQ_Entities = New IQ_MODEL
                Dim Conse As Nullable(Of Integer)
                If (_IQ_Entities.IQ_ControlCostos.Any(Function(b) b.IdPropuesta = C.IdPropuesta And b.ParAlternativa = C.ParAlternativa And b.MetCodigo = C.MetCodigo And b.ParNacional = C.ParNacional And b.ID = C.ID)) Then
                    Conse = (From o In _IQ_Entities.IQ_ControlCostos Where o.IdPropuesta = C.IdPropuesta And o.ParAlternativa = C.ParAlternativa And o.MetCodigo = C.MetCodigo And o.ParNacional = C.ParNacional And o.ID = C.ID Select o.Consecutivo).Max() + 1
                Else
                    Conse = 1
                End If



                Return Conse


            Catch ex As Exception
                Return Nothing
            End Try

        End Function


        Public Function ObtenerObservacionesPresupuesto(ByVal p As IQ_Parametros) As ObservacionesPresupuestos

            _IQ_Entities = New IQ_MODEL
            Dim result = (From O In _IQ_Entities.IQ_Parametros
                          Join D In _IQ_Entities.IQ_DatosGeneralesPresupuesto On O.ParAlternativa Equals D.ParAlternativa And O.IdPropuesta Equals D.IdPropuesta
                          Where O.IdPropuesta = p.IdPropuesta And O.ParAlternativa = p.ParAlternativa And O.ParNacional = p.ParNacional And O.MetCodigo = p.MetCodigo
                          Select New ObservacionesPresupuestos With {.Observaciones_Generales = D.Observaciones, .Observaciones_Presupuesto = O.ParObservaciones}).First()

            Return result

        End Function

        Public Function ObtenerDatosGeneralesPresupuesto(ByVal IdPropuesta As Int64, ByVal Alternativa As Int32) As IQ_DatosGeneralesPresupuesto
            Return op.IQ_DatosGeneralesPresupuesto.Where(Function(x) x.IdPropuesta = IdPropuesta And x.ParAlternativa = Alternativa).FirstOrDefault
        End Function

        Public Function ObtenerViaticos(ByVal p As IQ_Parametros) As List(Of IQ_ObtenerViaticosPresupuesto_Result)
            _IQ_Entities = New IQ_MODEL
            Return _IQ_Entities.IQ_ObtenerViaticosPresupuesto(p.IdPropuesta, p.ParAlternativa, p.MetCodigo, p.ParNacional).ToList()


        End Function

        Public Function ExisteRolUsuario(ByVal usuario As Decimal, ByVal rol As Integer) As Boolean
            Dim existe As Boolean
            _IQ_Entities = New IQ_MODEL
            existe = False

            If (Convert.ToInt32(_IQ_Entities.IQ_ObtenerRolUsuario(usuario, rol).First()) = 0) Then
                existe = False
            Else
                existe = True
            End If

            Return existe
        End Function


        Public Function ObtenerPresupuestosAprobados(ByVal IdPropuesta As Long, ByVal IdTrabajo As Long, ByVal JB As String, ByVal tecCodigo As Integer) As DataSet
            Try

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_ObtenerPresupuestosAprobados", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Parameters.Add("IdPropuesta", SqlDbType.BigInt).Value = If(IdPropuesta = 0, Nothing, IdPropuesta)
                    command.Parameters.Add("IdTrabajo", SqlDbType.BigInt).Value = If(IdTrabajo = 0, Nothing, IdTrabajo)
                    command.Parameters.Add("JobBook", SqlDbType.VarChar).Value = If(JB = "", Nothing, JB)
                    command.Parameters.Add("TecCodigo", SqlDbType.Int).Value = If(tecCodigo = 0, Nothing, tecCodigo)
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

        Public Function ObtenerCantidadPreguntas(ByVal p As IQ_Parametros) As Integer
            _IQ_Entities = New IQ_MODEL
            Dim obj = (From o In _IQ_Entities.IQ_Preguntas Where o.IdPropuesta = p.IdPropuesta And o.ParAlternativa = p.ParAlternativa And o.MetCodigo = p.MetCodigo And o.ParNacional = p.ParNacional Select (o.PregAbiertas + o.PregAbiertasMultiples + o.PregCerradas + o.PregCerradasMultiples + o.PregDemograficos + o.PregOtras))
            If obj.Count = 0 Then

                Return 0
            Else
                Return CInt(obj.First())
            End If

        End Function

    End Class


End Namespace

