
Imports System.Transactions
'Imports CoreProject.IQ_ENTITIES
Imports System.Data.Entity.Core

Namespace IQ
    Public Class sesionesGrupo

        Dim _IQ_Entities As IQ_MODEL

        Public Sub insertar(ByVal parametros As IQ_Parametros)
            Try
                _IQ_Entities = New IQ_MODEL

                If (_IQ_Entities.IQ_Parametros.Any(Function(p) p.IdPropuesta = parametros.IdPropuesta And p.ParAlternativa = parametros.ParAlternativa And p.MetCodigo = parametros.MetCodigo And p.ParNacional = parametros.ParNacional)) Then

                    Dim P1 As IQ_Parametros
                    P1 = _IQ_Entities.IQ_Parametros.First(Function(p) p.IdPropuesta = parametros.IdPropuesta And p.ParAlternativa = parametros.ParAlternativa And p.MetCodigo = parametros.MetCodigo And p.ParNacional = parametros.ParNacional)
                    P1.TipoProyecto = parametros.TipoProyecto
                    P1.ParNomPresupuesto = parametros.ParNomPresupuesto
                    P1.Pr_ProductCode = parametros.Pr_ProductCode
                    P1.TecCodigo = parametros.TecCodigo
                    P1.ParTotalPreguntas = parametros.ParTotalPreguntas
                    P1.ParPaginasEncuesta = parametros.ParPaginasEncuesta
                    P1.ParHorasEntrevista = parametros.ParHorasEntrevista
                    P1.ParNumAsistentesSesion = parametros.ParNumAsistentesSesion
                    P1.ParEncuestadoresPunto = parametros.ParEncuestadoresPunto
                    P1.ParProductividad = parametros.ParProductividad
                    'P1.ParProductividadOriginal = parametros.ParProductividadOriginal
                    P1.ParContactosNoEfectivos = parametros.ParContactosNoEfectivos
                    P1.ParContactosNoEfectivosOriginales = parametros.ParContactosNoEfectivosOriginales
                    P1.ParTiempoEncuesta = parametros.ParTiempoEncuesta
                    'P1.Usuario = parametros.Usuario
                    'P1.ParFechaCreacion = parametros.ParFechaCreacion
                    P1.ParValorDolar = parametros.ParValorDolar
                    P1.ParAprobado = parametros.ParAprobado
                    P1.ParFechaAprobacion = parametros.ParFechaAprobacion
                    P1.ParPresupuestoEnUso = parametros.ParPresupuestoEnUso
                    P1.ParUsuarioTieneUso = parametros.Usuario
                    P1.ParFactorAjustado = parametros.ParFactorAjustado
                    P1.ParNumJobBook = parametros.ParNumJobBook
                    P1.ParNProcesosDC = parametros.ParNProcesosDC
                    P1.ParNProcesosTopLines = parametros.ParNProcesosTopLines
                    P1.ParNProcesosTablas = parametros.ParNProcesosTablas
                    P1.ParNProcesosBases = parametros.ParNProcesosBases
                    P1.ParGrupoObjetivo = parametros.ParGrupoObjetivo
                    P1.ParIncidencia = parametros.ParIncidencia
                    P1.ParDiasEncuestador = parametros.ParDiasEncuestador
                    P1.ParDiasSupervisor = parametros.ParDiasSupervisor
                    P1.ParDiasCoordinador = parametros.ParDiasCoordinador
                    'P1.ParUnidad = parametros.ParUnidad
                    'P1.ParGrossMargin = parametros.ParGrossMargin
                    'P1.ParValorVenta = parametros.ParValorVenta
                    'P1.ParCostoDirecto = parametros.ParCostoDirecto
                    'P1.ParActSubCosto = parametros.ParActSubCosto
                    P1.Pr_Offeringcode = parametros.Pr_Offeringcode
                    'P1.ParActSubGasto = parametros.ParActSubGasto
                    P1.ParUsaLista = parametros.ParUsaLista
                    P1.ParObservaciones = parametros.ParObservaciones
                    P1.ParSubcontratar = parametros.ParSubcontratar
                    P1.ParPorcentajeSub = parametros.ParPorcentajeSub
                    P1.ParAñoSiguiente = parametros.ParAñoSiguiente

                    _IQ_Entities.SaveChanges()
                Else
                    _IQ_Entities.IQ_Parametros.Add(parametros)
                    _IQ_Entities.SaveChanges()
                End If

            Catch ex As Exception
                Throw ex
            End Try

        End Sub



        Public Function ObtenerSesiones(ByVal par As IQ_Parametros) As IQ_Parametros
            Try
                _IQ_Entities = New IQ_MODEL

                Dim P1 As IQ_Parametros
                P1 = (From p In _IQ_Entities.IQ_Parametros Where p.IdPropuesta = par.IdPropuesta And p.ParAlternativa = par.ParAlternativa And p.MetCodigo = par.MetCodigo And p.ParNacional = par.ParNacional Select p).First()

                Return P1

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub InsertarPresupuesto(ByVal par As IQ_Parametros, ByVal lstMuestra As List(Of IQ_Muestra_1), ByVal lstProc As List(Of IQ_ProcesosPresupuesto), ByVal lstOp As List(Of IQ_OpcionesAplicadas), ByVal lsth As List(Of IQ_HorasProfesionales))
            Try
                Using _IQ_Entities = New IQ_MODEL
                    Using scope As New TransactionScope()

                        insertar(par)
                        InsertarMuestra(lstMuestra)
                        InsertarProcesos(lstProc, par)
                        InsertarOpciones(lstOp)
                        InsertarCargos(lsth)

                        scope.Complete()
                    End Using

                End Using



            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function ExistePresupuesto(ByVal par As IQ_Parametros) As Boolean

            Try
                _IQ_Entities = New IQ_MODEL
                Dim Existe As Boolean = False
                If _IQ_Entities.IQ_Parametros.Any(Function(o) o.IdPropuesta = par.IdPropuesta And o.ParAlternativa = par.ParAlternativa And o.TecCodigo = par.TecCodigo) Then
                    Existe = True


                End If

                Return Existe
            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Public Sub InsertarProcesos(ByVal LstProcesos As List(Of IQ_ProcesosPresupuesto), ByVal par As IQ_Parametros)
            'Insertamos los procesos que aplican para la tecnica 
            Try
                Using _IQ_Entities = New IQ_MODEL
                    Using scope As New TransactionScope()

                        _IQ_Entities.IQ_BorrarProcesos(par.IdPropuesta, par.ParAlternativa, par.ParNacional, par.MetCodigo)

                        For Each proc In LstProcesos

                            If _IQ_Entities.IQ_ProcesosPresupuesto.Any(Function(Proc1) Proc1.IdPropuesta = proc.IdPropuesta And Proc1.ParAlternativa = proc.ParAlternativa And Proc1.MetCodigo = proc.MetCodigo And Proc1.ProcCodigo = proc.ProcCodigo And Proc1.ParNacional = proc.ParNacional) Then

                                Dim Proc2 = _IQ_Entities.IQ_ProcesosPresupuesto.First(Function(proc3) proc3.IdPropuesta = proc.IdPropuesta And proc3.ParAlternativa = proc.ParAlternativa And proc3.MetCodigo = proc.MetCodigo And proc3.ProcCodigo = proc.ProcCodigo And proc3.ParNacional = proc.ParNacional)

                                Proc2.Porcentaje = proc.Porcentaje

                                _IQ_Entities.SaveChanges()

                            Else

                                _IQ_Entities.IQ_ProcesosPresupuesto.Add(proc)
                                _IQ_Entities.SaveChanges()
                            End If

                        Next

                        scope.Complete()
                    End Using

                End Using

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub InsertarMuestra(ByVal LstMuestra As List(Of IQ_Muestra_1))
            'Insertamos los valores de muestra 
            Try
                _IQ_Entities = New IQ_MODEL

                For Each m1 In LstMuestra
                    If _IQ_Entities.IQ_Muestra.Any(Function(m2) m2.IdPropuesta = m1.IdPropuesta And m2.ParAlternativa = m1.ParAlternativa And m2.MetCodigo = m1.MetCodigo And m2.MuIdentificador = m1.MuIdentificador And m2.ParNacional = m1.ParNacional And m2.CiuCodigo = m1.CiuCodigo) Then
                        Dim m4 = _IQ_Entities.IQ_Muestra.First(Function(m3) m3.IdPropuesta = m1.IdPropuesta And m3.ParAlternativa = m1.ParAlternativa And m3.MetCodigo = m1.MetCodigo And m3.MuIdentificador = m1.MuIdentificador And m3.ParNacional = m1.ParNacional And m3.CiuCodigo = m1.CiuCodigo)

                        m4.MuCantidad = m1.MuCantidad
                        m4.MuIdentificador = m1.MuIdentificador
                        m4.DeptCodigo = m1.DeptCodigo
                        m4.CiuCodigo = m1.CiuCodigo

                        _IQ_Entities.SaveChanges()
                    Else
                        _IQ_Entities.IQ_Muestra.Add(m1)
                        _IQ_Entities.SaveChanges()

                    End If


                Next

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function ObtenerOpciones(ByVal Tecnica As Integer) As List(Of IQ_OpcionesTecnicas)
            Try
                _IQ_Entities = New IQ_MODEL
                Dim lst As List(Of IQ_OpcionesTecnicas)
                lst = (From o In _IQ_Entities.IQ_OpcionesTecnicas Where o.TecCodigo = Tecnica Select o).ToList()

                Return lst

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Public Sub InsertarOpciones(ByVal lstOp As List(Of IQ_OpcionesAplicadas))
            Try
                _IQ_Entities = New IQ_MODEL

                For Each OPc In lstOp

                    If _IQ_Entities.IQ_OpcionesAplicadas.Any(Function(o) OPc.IdPropuesta = o.IdPropuesta And OPc.ParAlternativa = o.ParAlternativa And OPc.MetCodigo = o.MetCodigo And OPc.IdOpcion = o.IdOpcion And OPc.ParNacional = o.ParNacional And OPc.TecCodigo = o.TecCodigo) Then

                        Dim Opcion = _IQ_Entities.IQ_OpcionesAplicadas.First(Function(o3) o3.IdPropuesta = OPc.IdPropuesta And o3.ParAlternativa = OPc.ParAlternativa And o3.MetCodigo = OPc.MetCodigo And o3.ParNacional = OPc.ParNacional And o3.IdOpcion = OPc.IdOpcion And o3.TecCodigo = OPc.TecCodigo)
                        Opcion.Aplica = OPc.Aplica
                        _IQ_Entities.SaveChanges()

                    Else

                        _IQ_Entities.IQ_OpcionesAplicadas.Add(OPc)
                        _IQ_Entities.SaveChanges()
                    End If

                Next

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function ExisteOpcion(ByVal lst As List(Of IQ_OpcionesAplicadas), ByVal op As Integer)
            Try
                Dim existe As Boolean = False

                If lst.Any(Function(p) p.IdOpcion = op And p.Aplica = True) Then
                    existe = True
                End If


                Return existe
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Function ObtenerCostoDirecto(ByVal par As IQ_Parametros) As Decimal

            Try
                _IQ_Entities = New IQ_MODEL
                Dim output As New Objects.ObjectParameter("SESIONESCostoDirecto", GetType(Decimal))

                _IQ_Entities.IQ_SESIONESCostosDirectos(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional, output)
                Return CDec(output.Value)

            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function ValorVenta(ByVal par As IQ_Parametros) As Decimal

            Try
                _IQ_Entities = New IQ_MODEL
                Dim output As New Objects.ObjectParameter("ValorVenta", GetType(Decimal))
                _IQ_Entities.IQ_ValorVenta(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional, par.ParUnidad, par.ParActSubGasto, (par.ParCostoDirecto), output)
                Return CDec(output.Value)

            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Function ValorVentaCualitativo(ByVal par As IQ_Parametros) As Decimal

            Try
                _IQ_Entities = New IQ_MODEL
                Dim output As New Objects.ObjectParameter("ValorVenta", GetType(Decimal))
                _IQ_Entities.IQ_ValorVentaCualitativo(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional, par.ParUnidad, par.ParActSubGasto, (par.ParCostoDirecto), output)
                Return CDec(output.Value)

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub calcularActSubcontratadas(ByRef p As IQ_Parametros)
            Try
                _IQ_Entities = New IQ_MODEL
                Dim ActCosto As New Objects.ObjectParameter("ValorActividadCosto", GetType(Decimal))
                Dim ActGasto As New Objects.ObjectParameter("ValorActividadGasto", GetType(Decimal))

                _IQ_Entities.IQ_ActividadesSubcontratadas(p.IdPropuesta, p.ParAlternativa, p.MetCodigo, p.ParNacional, ActCosto, ActGasto)
                p.ParActSubCosto = CDec(ActCosto.Value)
                p.ParActSubGasto = CDec(ActGasto.Value)

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub InsertarGrossMargin(ByVal par As IQ_Parametros)

            Try
                _IQ_Entities = New IQ_MODEL

                Dim P1 As IQ_Parametros
                P1 = _IQ_Entities.IQ_Parametros.First(Function(p) p.IdPropuesta = par.IdPropuesta And p.ParAlternativa = par.ParAlternativa And p.MetCodigo = par.MetCodigo And p.ParNacional = par.ParNacional)
                P1.ParGrossMargin = par.ParGrossMargin
                P1.ParValorVenta = par.ParValorVenta

                _IQ_Entities.SaveChanges()



            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function ObtenerNumAsistentes(ByVal metodologia As Integer) As Integer
            Try
                _IQ_Entities = New IQ_MODEL
                Dim NumAsistentes As Integer

                NumAsistentes = _IQ_Entities.IQ_ObtenerNumAsistentes(metodologia).First()
                Return NumAsistentes

            Catch ex As Exception
                Throw ex
            End Try

        End Function



        Public Function TotalizarMuestra(ByVal p As IQ_Parametros) As Integer
            Try

                _IQ_Entities = New IQ_MODEL
                Dim total As Integer
                total = (From t In _IQ_Entities.IQ_Muestra Where t.IdPropuesta = p.IdPropuesta And t.ParAlternativa = p.ParAlternativa And t.MetCodigo = p.MetCodigo And t.ParNacional = p.ParNacional Select t.MuCantidad).Sum()
                Return total
            Catch ex As Exception
                Throw ex
            End Try



        End Function



        Public Function ObtenerCargos(ByVal P As IQ_Parametros, ByVal FirstTime As Integer) As List(Of IQ_ObtenerCargos_Result)
            _IQ_Entities = New IQ_MODEL
            Dim lst As List(Of IQ_ObtenerCargos_Result)
            If FirstTime = 1 Then
                lst = (_IQ_Entities.IQ_ObtenerCargos(P.IdPropuesta, P.ParAlternativa, 0, 0)).ToList()
            Else
                lst = (_IQ_Entities.IQ_ObtenerCargos(P.IdPropuesta, P.ParAlternativa, P.MetCodigo, P.ParNacional)).ToList()
            End If

            Return lst
        End Function

        Public Sub InsertarCargos(ByVal lst As List(Of IQ_HorasProfesionales))
            Try
                _IQ_Entities = New IQ_MODEL

                For Each h In lst

                    If _IQ_Entities.IQ_HorasProfesionales.Any(Function(o) h.IdPropuesta = o.IdPropuesta And h.ParAlternativa = o.ParAlternativa And h.MetCodigo = o.MetCodigo And h.ParNacional = o.ParNacional And h.CodCargo = o.CodCargo) Then

                        Dim Cargos = _IQ_Entities.IQ_HorasProfesionales.First(Function(o3) o3.IdPropuesta = h.IdPropuesta And o3.ParAlternativa = h.ParAlternativa And o3.MetCodigo = h.MetCodigo And o3.ParNacional = h.ParNacional And h.CodCargo = o3.CodCargo)
                        Cargos.Horas = h.Horas
                        _IQ_Entities.SaveChanges()

                    Else
                        _IQ_Entities.IQ_HorasProfesionales.Add(h)
                        _IQ_Entities.SaveChanges()
                    End If

                Next

            Catch ex As Exception
                Throw ex
            End Try

        End Sub
    End Class




End Namespace
