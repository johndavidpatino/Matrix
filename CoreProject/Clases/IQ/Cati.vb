Imports System.Transactions
Imports System.Data.Entity.Core

Namespace IQ

    Public Class Cati
        Dim _IQ_Entities As IQ_MODEL


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
                    'P1.ParTiempoEncuesta = parametros.ParTiempoEncuesta
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

        Public Function ObtenerCuantitativos(ByVal par As IQ_Parametros) As List(Of IQ_Cuantitativos_Result)
            Try

                _IQ_Entities = New IQ_MODEL
                Dim lstCuantitativos As List(Of IQ_Cuantitativos_Result)
                lstCuantitativos = (From c In _IQ_Entities.IQ_Cuantitativos(par.IdPropuesta, par.ParAlternativa, par.MetCodigo)).ToList()
                Return lstCuantitativos

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub CalcularProductividad(ByVal par As IQ_Parametros)
            Try
                _IQ_Entities = New IQ_MODEL

                _IQ_Entities.IQ_CATIProductividad(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional)

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function ObtenerCostoDirecto(ByVal par As IQ_Parametros) As Decimal

            Try
                _IQ_Entities = New IQ_MODEL
                Dim output As New Objects.ObjectParameter("CATICostoDirecto", GetType(Decimal))


                _IQ_Entities.IQ_CATICostosDirectos(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional, output)
                Return CDec(output.Value)

            Catch ex As Exception
                Throw ex
            End Try



        End Function

        Public Function ValorVenta(ByVal par As IQ_Parametros) As Decimal

            Try
                _IQ_Entities = New IQ_MODEL
                Dim output As New Objects.ObjectParameter("ValorVenta", GetType(Decimal))
                _IQ_Entities.IQ_ValorVenta(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional, par.ParUnidad, (par.ParCostoDirecto + par.ParActSubCosto), par.ParActSubGasto, output)
                Return CDec(output.Value)

            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Function OnbtenerMetodologias(ByVal Tecnica As Integer) As List(Of IQ_ObtenerMetodologias_Result)
            Try
                _IQ_Entities = New IQ_MODEL
                Dim lstMetodologia As List(Of IQ_ObtenerMetodologias_Result)
                lstMetodologia = _IQ_Entities.IQ_ObtenerMetodologias(Tecnica).ToList()
                lstMetodologia.Insert(0, New IQ_ObtenerMetodologias_Result With {.MetCodigo = "0", .MetNombre = "Seleccione..."})
                Return lstMetodologia

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerAlternativa(ByVal par As IQ_Parametros) As Integer

            Try
                Dim alternativa As Integer
                _IQ_Entities = New IQ_MODEL
                alternativa = CInt(_IQ_Entities.IQ_ObtenerAlternativa(par.IdPropuesta).ToString())
                Return alternativa
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

        Public Function ObtenerParametros(ByVal Par As IQ_Parametros) As IQ_Parametros
            Try
                _IQ_Entities = New IQ_MODEL
                Dim p = (From p1 In _IQ_Entities.IQ_Parametros Where p1.IdPropuesta = Par.IdPropuesta And p1.ParAlternativa = Par.ParAlternativa And p1.MetCodigo = Par.MetCodigo And p1.ParNacional = Par.ParNacional Select p1).First()
                Return p

            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Function ObtenerPreguntas(ByVal par As IQ_Parametros) As IQ_Preguntas
            Try
                _IQ_Entities = New IQ_MODEL
                Dim p = (From preg In _IQ_Entities.IQ_Preguntas Where preg.IdPropuesta = par.IdPropuesta And preg.ParAlternativa = par.ParAlternativa And preg.MetCodigo = par.MetCodigo And preg.ParNacional = par.ParNacional Select preg).First()
                Return p

            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Function ObtenerProcesos(ByVal par As IQ_Parametros) As List(Of IQ_ProcesosPresupuesto)
            Try

                _IQ_Entities = New IQ_MODEL
                Dim LstProc As List(Of IQ_ProcesosPresupuesto)
                LstProc = (From proc1 In _IQ_Entities.IQ_ProcesosPresupuesto Where proc1.IdPropuesta = par.IdPropuesta And proc1.ParAlternativa = par.ParAlternativa And proc1.MetCodigo = par.MetCodigo And proc1.ParNacional = par.ParNacional Select proc1).ToList()

                Return LstProc
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ExisteCati(ByVal par As IQ_Parametros) As Boolean

            Try
                _IQ_Entities = New IQ_MODEL
                Dim Existe As Boolean = False
                'NO VA LA OPCION  NACIONAL PUES ACA SOLO SE VALIDA A NIVEL DE TECNICA 
                If _IQ_Entities.IQ_Parametros.Any(Function(o) o.IdPropuesta = par.IdPropuesta And o.ParAlternativa = par.ParAlternativa And o.MetCodigo = par.MetCodigo And o.ParNacional = par.ParNacional) Then
                    Existe = True

                End If

                Return Existe
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function ExisteCatiNacional(ByVal par As IQ_Parametros) As Boolean

            Try
                _IQ_Entities = New IQ_MODEL
                Dim Existe As Boolean = False
                'NO VA LA OPCION  NACIONAL PUES ACA SOLO SE VALIDA A NIVEL DE TECNICA 
                If _IQ_Entities.IQ_Parametros.Any(Function(o) o.IdPropuesta = par.IdPropuesta And o.ParAlternativa = par.ParAlternativa And o.TecCodigo = par.TecCodigo And o.ParNacional = True) Then
                    Existe = True

                End If

                Return Existe
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function ExisteCatiInternacional(ByVal par As IQ_Parametros) As Boolean

            Try
                _IQ_Entities = New IQ_MODEL
                Dim Existe As Boolean = False
                'NO VA LA OPCION  NACIONAL PUES ACA SOLO SE VALIDA A NIVEL DE TECNICA 
                If _IQ_Entities.IQ_Parametros.Any(Function(o) o.IdPropuesta = par.IdPropuesta And o.ParAlternativa = par.ParAlternativa And o.TecCodigo = par.TecCodigo And o.ParNacional = False) Then
                    Existe = True

                End If

                Return Existe
            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Public Function ObtenerMetodologia(ByVal par As IQ_Parametros) As Integer
            Try
                Dim Metodologia As Integer
                _IQ_Entities = New IQ_MODEL

                Metodologia = (From m In _IQ_Entities.IQ_Parametros Where m.IdPropuesta = par.IdPropuesta And m.ParAlternativa = par.ParAlternativa And m.TecCodigo = par.TecCodigo Select m.MetCodigo).First()

                Return Metodologia
            Catch ex As Exception
                Throw ex
            End Try
        End Function

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

        Public Sub ModificarGrossMargin(ByVal GM As Double, ByVal Par As IQ_Parametros, ByVal TipoCalculo As Integer, ByVal ValorVenta As Decimal, ByVal GM_UNI As Decimal, ByVal GM_OPE As Decimal)
            Try
                '1. Modificamos el GM 
                '2.Ajustamos el valor de venta
                '3.Ajustamos el costo directo 
                Dim retorno As Decimal
                Using _IQ_Entities = New IQ_MODEL
                    Using scope As New TransactionScope()

                        If TipoCalculo = 1 Then
                            retorno = _IQ_Entities.IQ_AjustarGM(Par.IdPropuesta, Par.ParAlternativa, Par.MetCodigo, Par.ParNacional, ValorVenta, 0).First()
                        ElseIf TipoCalculo = 2 Then
                            retorno = _IQ_Entities.IQ_AjustarVenta(Par.IdPropuesta, Par.ParAlternativa, Par.MetCodigo, Par.ParNacional, GM_UNI, GM_OPE, 0).First()
                        End If


                        ''1. le restamos a 100 el nuevo porcentaje 
                        'Par.ParGrossMargin = (100 - GM) / 100
                        'Dim ValorSumarVenta As Decimal
                        'Dim ValorRestar As Decimal
                        'ValorRestar = ActividadesNoAplicanGM(Par)
                        'ValorSumarVenta = (ValorRestar * 1.1000000000000001)

                        'Par.ParValorVenta = ((((Par.ParCostoDirecto + Par.ParActSubCosto + Par.ParActSubGasto) - ValorRestar) * (1)) / Par.ParGrossMargin) + ValorSumarVenta
                        'Par.ParGrossMargin = GM / 100

                        'InsertarGrossMargin(Par)

                        scope.Complete()
                    End Using

                End Using



            Catch ex As Exception
                Throw ex
            End Try

        End Sub

        Public Function ObtenerPresupuestosModificarGM(ByVal alternativa As Integer, ByVal propuesta As Long) As List(Of IQ_Parametros)
            Try

                _IQ_Entities = New IQ_MODEL

                Dim lst As List(Of IQ_Parametros)
                lst = (From p In _IQ_Entities.IQ_Parametros Where p.IdPropuesta = propuesta And p.ParAlternativa = alternativa Select p).ToList()
                Return lst

            Catch ex As Exception
                Throw ex
            End Try
        End Function

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

        Public Function ObtenerNacionales(ByVal par As IQ_Parametros, ByVal Tecnica As Integer) As List(Of IQ_ObtenerNacionales_Result)
            Try

                _IQ_Entities = New IQ_MODEL
                Dim lstNacionales As List(Of IQ_ObtenerNacionales_Result)
                lstNacionales = (From c In _IQ_Entities.IQ_ObtenerNacionales(par.IdPropuesta, Tecnica, par.ParAlternativa)).ToList()
                Return lstNacionales

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerDatosPropuesta(ByVal idPropuesta As Long) As IQ_ObtenerDatosPropuesta_Result

            Try
                Dim datos As IQ_ObtenerDatosPropuesta_Result
                _IQ_Entities = New IQ_MODEL
                datos = _IQ_Entities.IQ_ObtenerDatosPropuesta(idPropuesta).First()


                Return datos
            Catch ex As Exception
                Throw ex
            End Try
        End Function



        Public Function ExisteAlternativa(ByVal Parametros As IQ_Parametros) As Boolean
            Try

                _IQ_Entities = New IQ_MODEL
                Dim existe As Boolean = False
                If (_IQ_Entities.IQ_DatosGeneralesPresupuesto.Any(Function(p) p.IdPropuesta = Parametros.IdPropuesta And p.ParAlternativa = Parametros.ParAlternativa)) Then
                    existe = True
                End If

                Return existe
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function UltimaAlternativa(ByVal Parametros As IQ_Parametros) As Integer
            Try

                _IQ_Entities = New IQ_MODEL
                Dim ultima As Integer = 0
                ultima = (From p In _IQ_Entities.IQ_DatosGeneralesPresupuesto Where p.IdPropuesta = Parametros.IdPropuesta Select CType(p.ParAlternativa, Integer?)).Max()

                Return ultima
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Sub ActualizarRevision(ByVal par As IQ_Parametros)
            Try

                _IQ_Entities = New IQ_MODEL
                Dim P1 As IQ_Parametros
                'Obtenemos el presupuesto y lo marcamos como revisado
                P1 = _IQ_Entities.IQ_Parametros.First(Function(p) p.IdPropuesta = par.IdPropuesta And p.ParAlternativa = par.ParAlternativa And p.ParNacional = par.ParNacional And p.MetCodigo = par.MetCodigo)
                P1.ParRevisado = par.ParRevisado
                P1.ParRevisadoPor = par.ParRevisadoPor
                P1.ParFechaRevision = par.ParFechaRevision
                _IQ_Entities.SaveChanges()


            Catch ex As Exception

            End Try
        End Sub


        Public Function ObtenerDatosGenerales(ByVal par As IQ_DatosGeneralesPresupuesto) As IQ_DatosGeneralesPresupuesto
            Try
                _IQ_Entities = New IQ_MODEL

                If (_IQ_Entities.IQ_DatosGeneralesPresupuesto.Any(Function(DATOS) DATOS.IdPropuesta = par.IdPropuesta And DATOS.ParAlternativa = par.ParAlternativa)) Then
                    Return (From d In _IQ_Entities.IQ_DatosGeneralesPresupuesto Where d.IdPropuesta = par.IdPropuesta And d.ParAlternativa = par.ParAlternativa Select d).First()
                Else
                    Return Nothing
                End If



            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub InsertarDatosGenerales(ByVal par As IQ_DatosGeneralesPresupuesto)
            Try
                _IQ_Entities = New IQ_MODEL
                If (_IQ_Entities.IQ_DatosGeneralesPresupuesto.Any(Function(DATOS) DATOS.IdPropuesta = par.IdPropuesta And DATOS.ParAlternativa = par.ParAlternativa)) Then
                    Dim P1 As IQ_DatosGeneralesPresupuesto
                    P1 = _IQ_Entities.IQ_DatosGeneralesPresupuesto.First(Function(p) p.IdPropuesta = par.IdPropuesta And p.ParAlternativa = par.ParAlternativa)
                    P1.Observaciones = par.Observaciones
                    P1.Descripcion = par.Descripcion
                    P1.DiasCampo = par.DiasCampo
                    P1.DiasDiseno = par.DiasDiseno
                    P1.DiasInformes = par.DiasInformes
                    P1.DiasProcesamiento = par.DiasProcesamiento
                    P1.NumeroMediciones = par.NumeroMediciones
                    P1.MesesMediciones = par.MesesMediciones
                    P1.TipoPresupuesto = par.TipoPresupuesto
                    _IQ_Entities.SaveChanges()
                Else
                    _IQ_Entities.IQ_DatosGeneralesPresupuesto.Add(par)
                    _IQ_Entities.SaveChanges()
                End If

            Catch ex As Exception
                Throw ex
            End Try


        End Sub


        Public Function ValidarRevisiones(ByVal par As IQ_Parametros) As Boolean

            Try

                _IQ_Entities = New IQ_MODEL
                Dim CantTecnicas As Integer = 0
                Dim CantRevisiones As Integer = 0
                'Cantidad de presupuestos creados por alternativa 
                CantTecnicas = (From t In _IQ_Entities.IQ_Parametros Where t.IdPropuesta = par.IdPropuesta And t.ParAlternativa = par.ParAlternativa Select t.IdPropuesta).Count()
                'cantidad de presupuestos aprobados
                CantRevisiones = (From t In _IQ_Entities.IQ_Parametros Where t.IdPropuesta = par.IdPropuesta And t.ParAlternativa = par.ParAlternativa And t.ParRevisado = True Select t.IdPropuesta).Count()

                If CantRevisiones = CantTecnicas Then
                    Return True
                Else
                    Return False
                End If


            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Function ObtenerFases(ByVal TEC As Integer) As List(Of IQ_ObtenerFases_Result)
            Try
                _IQ_Entities = New IQ_MODEL

                Dim lst As List(Of IQ_ObtenerFases_Result)
                lst = _IQ_Entities.IQ_ObtenerFases(TEC).ToList()
                lst.Insert(0, New IQ_ObtenerFases_Result With {.IdFase = "0", .DescFase = "Seleccione..."})
                Return lst


            Catch ex As Exception
                Throw ex
            End Try
        End Function


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

        Public Sub CalcularGrossMarginXAlternativa(ByVal lst As List(Of IQ_Parametros), ByVal NuevoGM As Decimal, ByVal TipoCalculo As Integer, ByVal ValorVenta As Decimal, ByVal GM_UNI As Decimal, ByVal GM_OPE As Decimal)
            Try

                Using _IQ_Entities = New IQ_MODEL
                    Using scope As New TransactionScope()

                        For Each o In lst
                            ModificarGrossMargin(NuevoGM, o, TipoCalculo, ValorVenta, GM_UNI, GM_OPE)
                        Next
                        scope.Complete()
                    End Using

                End Using

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub BorrarPresupuesto(ByVal p As IQ_Parametros)
            Try


                Using _IQ_Entities = New IQ_MODEL
                    Using scope As New TransactionScope()

                        _IQ_Entities.IQ_BorrarPresupuesto(p.IdPropuesta, p.ParAlternativa, p.ParNacional, p.MetCodigo)

                        scope.Complete()
                    End Using

                End Using



            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function ObtenerOpcionesMuestra(ByVal metodologia As Integer) As List(Of IQ_ObtenerOpcionesMuestra_Result)

            Try
                _IQ_Entities = New IQ_MODEL
                Dim lst As List(Of IQ_ObtenerOpcionesMuestra_Result)
                lst = _IQ_Entities.IQ_ObtenerOpcionesMuestra(metodologia).ToList()
                lst.Insert(0, New IQ_ObtenerOpcionesMuestra_Result With {.IdIdentificador = "0", .DescIdentMuestra = "Seleccione..."})
                Return lst
            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Public Function ObtenerMuestra(ByVal par As IQ_Parametros) As List(Of IQ_ObtenerMuestraCati_Result)
            Try

                _IQ_Entities = New IQ_MODEL
                Dim lst As List(Of IQ_ObtenerMuestraCati_Result)
                lst = _IQ_Entities.IQ_ObtenerMuestraCati(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional).ToList()
                Return lst
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Sub ActualizarGrossMargin(ByVal par As IQ_Parametros)

            Try
                _IQ_Entities = New IQ_MODEL

                Dim P1 As IQ_Parametros
                P1 = _IQ_Entities.IQ_Parametros.First(Function(p) p.IdPropuesta = par.IdPropuesta And p.ParAlternativa = par.ParAlternativa And p.MetCodigo = par.MetCodigo And p.ParNacional = par.ParNacional)
                P1.ParGrossMargin = par.ParGrossMargin

                _IQ_Entities.SaveChanges()



            Catch ex As Exception
                Throw ex
            End Try
        End Sub


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

        Public Sub BorrarMuestra(ByVal m As IQ_Muestra_1)
            Try

                _IQ_Entities = New IQ_MODEL

                Dim mu As IQ_Muestra_1
                mu = _IQ_Entities.IQ_Muestra.First(Function(m3) m3.IdPropuesta = m.IdPropuesta And m3.ParAlternativa = m.ParAlternativa And m3.MetCodigo = m.MetCodigo And m3.ParNacional = m.ParNacional And m3.MuIdentificador = m.MuIdentificador)
                _IQ_Entities.IQ_Muestra.Remove(mu)
                _IQ_Entities.SaveChanges()
            Catch ex As Exception

            End Try
        End Sub

        Public Function PresupuestoRevisado(ByVal p As IQ_Parametros) As Boolean
            Try
                _IQ_Entities = New IQ_MODEL
                Dim revisado As Boolean
                revisado = (From r In _IQ_Entities.IQ_Parametros Where r.IdPropuesta = p.IdPropuesta And r.ParAlternativa = p.ParAlternativa And r.ParNacional = p.ParNacional And r.MetCodigo = p.MetCodigo Select rev = If(r.ParRevisado Is Nothing, False, r.ParRevisado)).First()
                Return revisado
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function PresupuestoAprobado(ByVal p As IQ_Parametros) As Boolean
            Try
                _IQ_Entities = New IQ_MODEL
                Dim aprobado As Boolean
                aprobado = (From r In _IQ_Entities.IQ_Parametros Where r.IdPropuesta = p.IdPropuesta And r.ParAlternativa = p.ParAlternativa And r.ParNacional = p.ParNacional And r.MetCodigo = p.MetCodigo Select ap = If(r.ParAprobado Is Nothing, False, r.ParAprobado)).First()
                Return aprobado
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ActividadesNoAplicanGM(ByVal p As IQ_Parametros) As Decimal
            'Obtenemos el valor de restar  del costo de las actividades subcontratadas
            _IQ_Entities = New IQ_MODEL
            Dim ValorRestar As Decimal
            Dim V = (From c In _IQ_Entities.IQ_CostoActividades Join a In _IQ_Entities.IQ_Actividades On c.ActCodigo Equals a.ID Where a.AplicaGM = False And c.IdPropuesta = p.IdPropuesta And c.ParAlternativa = p.ParAlternativa And c.ParNacional = p.ParNacional And c.MetCodigo = p.MetCodigo Select c.CaCosto).ToArray()
            ValorRestar = If(V.Sum() = Nothing, 0, V.Sum)
            Return ValorRestar
        End Function

        Public Function ObtenerValorFinanciacion(ByVal p As IQ_Parametros) As Decimal
            _IQ_Entities = New IQ_MODEL
            Return _IQ_Entities.IQ_ObtenerValorFinanciacion(p.IdPropuesta, p.ParAlternativa).First()

        End Function

        Public Function ObtenerPresupuestosxPropuesta(ByVal idPropuesta As Int64, ByVal parAlternativa As Int32, ByVal metCodigo As Int32, ByVal parNacional As Int32) As IQ_ObtenerPresupuestosxPropuesta_Result
            _IQ_Entities = New IQ_MODEL()
            Return _IQ_Entities.IQ_ObtenerPresupuestosxPropuesta(idPropuesta, parAlternativa, metCodigo, parNacional).FirstOrDefault
        End Function

    End Class


End Namespace
