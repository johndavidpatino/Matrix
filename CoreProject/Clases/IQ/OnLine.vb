Imports System.Transactions
''Imports CoreProject.IQ_ENTITIES
Imports System.Data.Entity.Core

Namespace IQ
    Public Class OnLine
        Dim _IQ_Entities As IQ_MODEL


        Public Function ObtenerProcesosXTecnica(ByVal Tecnica As Integer) As List(Of IQ_Procesos)
            Try
                _IQ_Entities = New IQ_MODEL

                Dim lst As List(Of IQ_Procesos)
                lst = (From p In _IQ_Entities.IQ_Procesos Join pt In _IQ_Entities.IQ_TecnicaProcesos On p.ProcCodigo Equals pt.ProcCodigo Where pt.TecCodigo = Tecnica Select p).ToList()
                Return lst


            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Sub InsertarParametros(ByVal parametros As IQ_Parametros)
            'Insertamos sobre la tabla de parametros

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
                    P1.ParObservaciones = parametros.ParObservaciones
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

        Public Sub InsertarPreguntas(ByVal Preguntas As IQ_Preguntas)
            'Insertamos los totales de preguntas digitadas
            Try
                _IQ_Entities = New IQ_MODEL

                If _IQ_Entities.IQ_Preguntas.Any(Function(P) P.IdPropuesta = Preguntas.IdPropuesta And P.ParAlternativa = Preguntas.ParAlternativa And P.MetCodigo = Preguntas.MetCodigo And P.ParNacional = Preguntas.ParNacional) Then
                    Dim Preg As IQ_Preguntas
                    Preg = _IQ_Entities.IQ_Preguntas.First(Function(Preg1) Preg1.IdPropuesta = Preguntas.IdPropuesta And Preg1.ParAlternativa = Preguntas.ParAlternativa And Preg1.MetCodigo = Preguntas.MetCodigo And Preg1.ParNacional = Preguntas.ParNacional)
                    Preg.PregAbiertas = Preguntas.PregAbiertas
                    Preg.PregAbiertasMultiples = Preguntas.PregAbiertasMultiples
                    Preg.PregCerradas = Preguntas.PregCerradas
                    Preg.PregCerradasMultiples = Preguntas.PregCerradasMultiples
                    Preg.PregOtras = Preguntas.PregOtras
                    Preg.PregDemograficos = Preguntas.PregDemograficos
                    _IQ_Entities.SaveChanges()


                Else
                    _IQ_Entities.IQ_Preguntas.Add(Preguntas)
                    _IQ_Entities.SaveChanges()
                End If

            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Public Sub InsertarProcesos(ByVal LstProcesos As List(Of IQ_ProcesosPresupuesto), ByVal PAR As IQ_Parametros)
            'Insertamos los procesos que aplican para la tecnica 
            Try



                Using _IQ_Entities = New IQ_MODEL
                    Using scope As New TransactionScope()

                        _IQ_Entities.IQ_BorrarProcesos(PAR.IdPropuesta, PAR.ParAlternativa, PAR.ParNacional, PAR.MetCodigo)

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
                    If _IQ_Entities.IQ_Muestra.Any(Function(m2) m2.IdPropuesta = m1.IdPropuesta And m2.ParAlternativa = m1.ParAlternativa And m2.MetCodigo = m1.MetCodigo And m2.MuIdentificador = m1.MuIdentificador And m2.ParNacional = m1.ParNacional) Then
                        Dim m4 = _IQ_Entities.IQ_Muestra.First(Function(m3) m3.IdPropuesta = m1.IdPropuesta And m3.ParAlternativa = m1.ParAlternativa And m3.MetCodigo = m1.MetCodigo And m3.MuIdentificador = m1.MuIdentificador And m3.ParNacional = m1.ParNacional)

                        m4.MuCantidad = m1.MuCantidad


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

        Public Sub InsertarPresupuesto(ByVal par As IQ_Parametros, ByVal lstMuestra As List(Of IQ_Muestra_1), ByVal lstProc As List(Of IQ_ProcesosPresupuesto), ByVal Preg As IQ_Preguntas)
            Try
                Using _IQ_Entities = New IQ_MODEL
                    Using scope As New TransactionScope()

                        InsertarParametros(par)
                        InsertarMuestra(lstMuestra)
                        InsertarProcesos(lstProc, par)
                        InsertarPreguntas(Preg)
                        scope.Complete()
                    End Using

                End Using



            Catch ex As Exception
                Throw ex
            End Try
        End Sub


        Public Function ExisteProceso(ByVal lst As List(Of IQ_ProcesosPresupuesto), ByVal proc As Integer)
            Try
                Dim existe As Boolean = False
                'Dim lst As List(Of IQ_ProcesosPresupuesto)
                'lst = (From p In _IQ_Entities.IQ_ProcesosPresupuesto Where p.ProcCodigo = proc).ToList()
                If lst.Any(Function(p) p.ProcCodigo = proc) Then
                    existe = True
                End If


                Return existe
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerMuestra(ByVal par As IQ_Parametros) As List(Of IQ_ObtenerMuestraOnLine_Result)
            Try
                _IQ_Entities = New IQ_MODEL
                Dim lst As List(Of IQ_ObtenerMuestraOnLine_Result)
                lst = _IQ_Entities.IQ_ObtenerMuestraOnLine(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional).ToList()
                Return lst

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerPresupuesto(ByVal Par As IQ_Parametros) As IQ_Parametros
            Try
                _IQ_Entities = New IQ_MODEL
                Dim p = (From p1 In _IQ_Entities.IQ_Parametros Where p1.IdPropuesta = Par.IdPropuesta And p1.ParAlternativa = Par.ParAlternativa And p1.MetCodigo = Par.MetCodigo And p1.ParNacional = Par.ParNacional Select p1).First()
                Return p

            Catch ex As Exception
                Throw ex
            End Try
        End Function




        Public Function ObtenerCostoDirecto(ByVal par As IQ_Parametros) As Decimal

            Try
                _IQ_Entities = New IQ_MODEL
                Dim output As New Objects.ObjectParameter("CostoDirecto", GetType(Decimal))


                _IQ_Entities.IQ_ONLINECostosDirectos(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional, output)
                Return CDec(output.Value)

            Catch ex As Exception
                Throw ex
            End Try



        End Function

        Public Function CalcularPoductividad(ByVal par As IQ_Parametros) As Decimal
            Try
                _IQ_Entities = New IQ_MODEL
                Dim output As New Objects.ObjectParameter("Productividad", GetType(Decimal))
                _IQ_Entities.IQ_ONLINEProductividad(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional, output)
                Return output.Value

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub ActualizarCalculos(ByVal par As IQ_Parametros)

            Try
                _IQ_Entities = New IQ_MODEL

                Dim P1 As IQ_Parametros
                P1 = _IQ_Entities.IQ_Parametros.First(Function(p) p.IdPropuesta = par.IdPropuesta And p.ParAlternativa = par.ParAlternativa And p.MetCodigo = par.MetCodigo And p.ParNacional = par.ParNacional)
                P1.ParGrossMargin = par.ParGrossMargin
                P1.ParValorVenta = par.ParValorVenta
                P1.ParProductividad = par.ParProductividad

                _IQ_Entities.SaveChanges()



            Catch ex As Exception
                Throw ex
            End Try
        End Sub

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

    End Class
End Namespace

