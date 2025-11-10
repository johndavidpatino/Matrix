Imports System.Transactions

Imports System.Configuration
Imports System.IO

Imports System.Data.Entity.Core
Imports System.Data.SqlClient
Imports System.Data.OleDb

Namespace IQ
    Public Class CaraCara
        Dim _IQ_Entities As IQ_MODEL

        Public Function ObtenerDepartamentos(ByVal ciuPrincipales As Boolean, ByVal nacional As Boolean) As List(Of IQ_ObtenerDepartamentos_Result)
            Try
                _IQ_Entities = New IQ_MODEL
                Dim lstDepto As List(Of IQ_ObtenerDepartamentos_Result)
                lstDepto = _IQ_Entities.IQ_ObtenerDepartamentos(ciuPrincipales, nacional).ToList()
                lstDepto.Insert(0, New IQ_ObtenerDepartamentos_Result With {.DivDepto = "0", .DivDeptoNombre = "Seleccione..."})

                Return lstDepto
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerCiudades(ByVal Departamento As Integer, ByVal ciuPrincipal As Boolean) As List(Of IQ_ObtenerCiudades_Result)
            Try
                _IQ_Entities = New IQ_MODEL
                Dim lstCiudades As List(Of IQ_ObtenerCiudades_Result)
                lstCiudades = _IQ_Entities.IQ_ObtenerCiudades(Departamento, ciuPrincipal).ToList()
                lstCiudades.Insert(0, New IQ_ObtenerCiudades_Result With {.DivMunicipio = "0", .DivMuniNombre = "Seleccione..."})
                Return lstCiudades
            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Public Function ObtenerCaraCara(ByVal par As IQ_Parametros) As IQ_Parametros
            Try
                _IQ_Entities = New IQ_MODEL

                Dim Cara As IQ_Parametros
                Cara = (From p In _IQ_Entities.IQ_Parametros Where p.IdPropuesta = par.IdPropuesta And p.ParAlternativa = par.ParAlternativa And p.MetCodigo = par.MetCodigo And p.ParNacional = par.ParNacional Select p).First()

                Return Cara

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        'Public Sub InsertarPresupuesto(ByVal par As IQ_Parametros)
        '    Try
        '        Using _IQ_Entities = New IQ_MODEL
        '            Using scope As New TransactionScope()

        '                Insertar(par)
        '                scope.Complete()
        '            End Using

        '        End Using



        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'End Sub

        Public Sub Insertar(ByVal parametros As IQ_Parametros)
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
                    ' P1.ParFechaCreacion = parametros.ParFechaCreacion
                    P1.ParValorDolar = parametros.ParValorDolar
                    P1.ParAprobado = parametros.ParAprobado
                    P1.ParFechaAprobacion = parametros.ParFechaAprobacion
                    P1.ParPresupuestoEnUso = parametros.ParPresupuestoEnUso
                    P1.ParUsuarioTieneUso = parametros.Usuario
                    P1.ParFactorAjustado = parametros.ParFactorAjustado
                    P1.ParNumJobBook = parametros.ParNumJobBook
                    P1.ParNProcesosDC = parametros.ParNProcesosDC
                    P1.ParNProcesosBases = parametros.ParNProcesosBases
                    P1.ParNProcesosTablas = parametros.ParNProcesosTablas
                    P1.ParNProcesosTopLines = parametros.ParNProcesosTopLines
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
                    P1.ParProbabilistico = parametros.ParProbabilistico
                    'PARAMETROS LOCALIZACION CENTRAL
                    P1.ParPorcentajeIntercep = parametros.ParPorcentajeIntercep
                    P1.ParPorcentajeRecluta = parametros.ParPorcentajeRecluta
                    P1.ParUnidadesProducto = parametros.ParUnidadesProducto
                    P1.ParValorUnitarioProd = parametros.ParValorUnitarioProd
                    P1.ParTipoCLT = parametros.ParTipoCLT
                    P1.ParAlquilerEquipos = parametros.ParAlquilerEquipos
                    P1.ParApoyoLogistico = parametros.ParApoyoLogistico
                    P1.ParAccesoInternet = parametros.ParAccesoInternet
                    P1.ParObservaciones = parametros.ParObservaciones
                    P1.ParUsaTablet = parametros.ParUsaTablet
                    P1.ParUsaPapel = parametros.ParUsaPapel
                    P1.ParDispPropio = parametros.ParDispPropio
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


        Public Function ObtenerCostoDirecto(ByVal par As IQ_Parametros) As Decimal

            Try
                _IQ_Entities = New IQ_MODEL
                Dim output As New Objects.ObjectParameter("CostoDirecto", GetType(Decimal))

                _IQ_Entities.IQ_CARACARACostosDirectos(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional, output)
                Return CDec(output.Value)

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function CalcularPoductividad(ByVal par As IQ_Parametros) As Decimal
            Try
                _IQ_Entities = New IQ_MODEL
                Dim output As New Objects.ObjectParameter("Productividad", GetType(Decimal))
                _IQ_Entities.IQ_CARACARAProductividad(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional, output)
                Return output.Value

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



        Public Function ExisteCaraCara(ByVal par As IQ_Parametros) As Boolean

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


        Public Function ExisteCaraCaraNacional(ByVal par As IQ_Parametros) As Boolean

            Try
                _IQ_Entities = New IQ_MODEL
                Dim Existe As Boolean = False
                If _IQ_Entities.IQ_Parametros.Any(Function(o) o.IdPropuesta = par.IdPropuesta And o.ParAlternativa = par.ParAlternativa And o.TecCodigo = par.TecCodigo And o.ParNacional = True) Then
                    Existe = True


                End If

                Return Existe
            Catch ex As Exception
                Throw ex
            End Try


        End Function


        Public Function ExisteCaraCaraInternacional(ByVal par As IQ_Parametros) As Boolean

            Try
                _IQ_Entities = New IQ_MODEL
                Dim Existe As Boolean = False
                If _IQ_Entities.IQ_Parametros.Any(Function(o) o.IdPropuesta = par.IdPropuesta And o.ParAlternativa = par.ParAlternativa And o.TecCodigo = par.TecCodigo And o.ParNacional = False) Then
                    Existe = True
                End If

                Return Existe
            Catch ex As Exception
                Throw ex
            End Try


        End Function



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


        Public Sub InsertarPresupuesto(ByVal par As IQ_Parametros, ByVal lstMuestra As List(Of IQ_Muestra_1), ByVal preg As IQ_Preguntas, ByVal lstProc As List(Of IQ_ProcesosPresupuesto))
            Try
                Using _IQ_Entities = New IQ_MODEL
                    Using scope As New TransactionScope()

                        Insertar(par)
                        InsertarPreguntas(preg)
                        InsertarMuestra(lstMuestra)
                        InsertarProcesos(lstProc, par)
                        scope.Complete()
                    End Using

                End Using

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function obtenerCiudadesMuestra(ByVal par As IQ_Parametros) As List(Of IQ_ObtenerMuestraXCiudad_Result)

            Try
                _IQ_Entities = New IQ_MODEL
                Dim lstCiuMuestra As List(Of IQ_ObtenerMuestraXCiudad_Result)

                lstCiuMuestra = _IQ_Entities.IQ_ObtenerMuestraXCiudad(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional).ToList()
                Return lstCiuMuestra

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function obtenerCiudadesMuestrACC(ByVal par As IQ_Parametros) As List(Of IQ_ObtenerMuestraXCiudadCaraCara_Result)

            Try
                _IQ_Entities = New IQ_MODEL
                Dim lstCiuMuestra As List(Of IQ_ObtenerMuestraXCiudadCaraCara_Result)

                lstCiuMuestra = _IQ_Entities.IQ_ObtenerMuestraXCiudadCaraCara(par.IdPropuesta, par.ParAlternativa, par.MetCodigo, par.ParNacional).ToList()
                Return lstCiuMuestra

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Sub BorrarCiudadMuestra(ByVal m As IQ_Muestra_1)
            Try

                _IQ_Entities = New IQ_MODEL

                'Dim mu As IQ_Muestra
                'mu = _IQ_Entities.IQ_Muestra.First(Function(m3) m3.IdPropuesta = m.IdPropuesta And m3.ParAlternativa = m.ParAlternativa And m3.MetCodigo = m.MetCodigo And m3.ParNacional = m.ParNacional And m3.CiuCodigo = m.CiuCodigo)
                '_IQ_Entities.IQ_Muestra.Remove(mu)
                '_IQ_Entities.SaveChanges()
                _IQ_Entities.IQ_BorrarMuestra(m.IdPropuesta, m.ParAlternativa, m.ParNacional, m.MetCodigo, m.CiuCodigo)


            Catch ex As Exception
                Throw ex
            End Try


        End Sub

        Public Function TotalizarMuestra(ByVal p As IQ_Parametros) As Integer
            Try

                _IQ_Entities = New IQ_MODEL
                Dim total As Integer
                Dim Query = (From t In _IQ_Entities.IQ_Muestra Where t.IdPropuesta = p.IdPropuesta And t.ParAlternativa = p.ParAlternativa And t.MetCodigo = p.MetCodigo And t.ParNacional = p.ParNacional Select t.MuCantidad).Sum(Function(x) CType(x, Integer?))

                If Query Is Nothing Then
                    total = 0
                Else
                    total = CInt(Query)
                End If
                Return total
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function ObtenerPrueba() As Decimal
            Try
                _IQ_Entities = New IQ_MODEL
                Dim salida As New Objects.ObjectParameter("salida", GetType(Decimal))
                Dim s As Decimal
                s = _IQ_Entities.IQ_PruebaValores(salida).First()
                Return salida.Value

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

        Public Function ObtenerMuestra(ByVal p As IQ_Parametros) As DataSet
            Try
                Dim cnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ConnectionString

                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_ObtenerMuestraXCiudadCaraCara", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.BigInt).Value = p.IdPropuesta
                    command.Parameters.Add("ParAlternativa", SqlDbType.Int).Value = p.ParAlternativa
                    command.Parameters.Add("ParNacional", SqlDbType.Int).Value = p.ParNacional
                    command.Parameters.Add("MetCodigo", SqlDbType.Int).Value = p.MetCodigo
                    Using da As New SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Sub DistribuirMuestra(ByVal m As IQ_Muestra_1)
            Try
                _IQ_Entities = New IQ_MODEL
                _IQ_Entities.IQ_DistribuirMuestra(m.CiuCodigo, m.MuCantidad, m.IdPropuesta, m.ParNacional, m.MetCodigo, m.ParAlternativa, m.DeptCodigo)

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function ExisteMuestra(ByVal par As IQ_Parametros) As Boolean

            Try
                _IQ_Entities = New IQ_MODEL
                Dim Existe As Boolean = False
                If _IQ_Entities.IQ_Muestra.Any(Function(o) o.IdPropuesta = par.IdPropuesta And o.ParAlternativa = par.ParAlternativa And o.MetCodigo = par.MetCodigo And o.ParNacional = par.ParNacional) Then
                    Existe = True

                End If

                Return Existe
            Catch ex As Exception
                Throw ex
            End Try


        End Function

#Region "Cargue excel"

        Public Function CargarMuestraDesdeExcel(ByVal par As IQ_Parametros, ByVal ruta As String) As List(Of IQ_Muestra_1)
            'Cargamos los datos del archivo excel a una lista tipo muestra 
            Try
                Dim lstCargue As New List(Of IQ_Muestra_1)
                Dim m As IQ_Muestra_1
                If (File.Exists(ruta)) Then
                    Dim i As Integer
                    Dim adapter As OleDbDataAdapter
                    Dim selectConnection As OleDbConnection
                    selectConnection = New OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0; Data Source='" + ruta + "';Extended Properties='Excel 12.0;HDR=YES; IMEX=1'")
                    adapter = New OleDbDataAdapter("select * from [MUESTRA$]", selectConnection)
                    Dim dataSet As New DataSet()
                    adapter.Fill(dataSet)
                    If dataSet.Tables(0).Rows.Count > 0 Then
                        For i = 0 To dataSet.Tables(0).Rows.Count - 1
                            m = New IQ_Muestra_1()
                            m.IdPropuesta = par.IdPropuesta
                            m.ParAlternativa = par.ParAlternativa
                            m.ParNacional = par.ParNacional
                            m.MetCodigo = par.MetCodigo
                            m.CiuCodigo = CInt(dataSet.Tables(0).Rows(i).ItemArray(1).ToString())
                            m.DeptCodigo = CInt(dataSet.Tables(0).Rows(i).ItemArray(0).ToString())
                            m.MuCantidad = CInt(dataSet.Tables(0).Rows(i).ItemArray(3).ToString())
                            m.MuIdentificador = CInt(dataSet.Tables(0).Rows(i).ItemArray(2).ToString())
                            lstCargue.Add(m)
                        Next

                    Else
                        Throw New Exception("No existen registros para cargar!")
                    End If

                Else
                    Throw New Exception("La ruta no es correcta !")
                End If

                Return lstCargue

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Public Function ValidarExistenciaCiudades(ByVal lstCargue As List(Of IQ_Muestra_1)) As Boolean
            'Validamos que las ciudades del archivo correspondan  con las existentes en la tabla IQ_Ciudades
            'en caso de no corresponder se debe generar alerta al usuario, partimos del hecho que el archivo ya ha sido validado previamente en estadistica y las ciudades deberian 
            'corresponder con las existentes, para tal proposito se habilito la lista completa de ciudades existentes en la divipola
            Try
                Dim i As Integer
                Dim valida As Boolean = True
                i = 1
                _IQ_Entities = New IQ_MODEL

                'Dim c1 = From mu In lstCargue Where Not ((From o In _IQ_Entities.IQ_Ciudades Select o.CiuCiudad).Contains(mu.CiuCodigo)) Select mu


                For Each r In lstCargue
                    Dim c = From ciu In _IQ_Entities.IQ_Ciudades Where ciu.CiuCiudad = r.CiuCodigo Select ciu

                    If c.Count = 0 Then
                        valida = False
                        Throw New Exception("La ciudad relacionada en la linea " & i + 1 & " no existe !!")
                    End If

                    i = i + 1
                Next

                Return valida
            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Public Function ValidarIdentificadorDistribucion(ByVal lstCargue As List(Of IQ_Muestra_1)) As Boolean
            'Validamos que los items de distribucion  correspondan a los items  de la metodologia 
            'identificamos la metdologia y validamos segun esta si los codiogs ocrresponden 
            Dim Identificador As Boolean = True
            Dim i As Integer
            i = 1
            _IQ_Entities = New IQ_MODEL

            For Each r In lstCargue
                Dim id = From d In _IQ_Entities.IQ_OpcionesMuestra Where d.MetCodigo = r.MetCodigo And d.IdIdentificador = r.MuIdentificador Select d

                If id.Count = 0 Then
                    Identificador = False
                    Throw New Exception("El codigo de distribucion de la muestra  de la linea  " & i + 1 & " no correponde con la metodologia !!")
                End If
                i = i + 1
            Next


            Return Identificador

        End Function

#End Region

        Public Function ObtenerTipoServicio() As List(Of IQ_TipoServicio)
            Try
                _IQ_Entities = New IQ_MODEL
                Dim lst As List(Of IQ_TipoServicio)
                lst = (From o In _IQ_Entities.IQ_TipoServicio Select o).ToList()

                lst.Insert(0, New IQ_TipoServicio With {.TS_Id = "0", .TS_Descripcion = "Seleccione..."})
                Return lst
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerEvidencia() As List(Of IQ_Evidencia)
            Try
                _IQ_Entities = New IQ_MODEL
                Dim lst As List(Of IQ_Evidencia)
                lst = (From o In _IQ_Entities.IQ_Evidencia Select o).ToList()

                lst.Insert(0, New IQ_Evidencia With {.EV_Id = "0", .EV_Descripcion = "Seleccione..."})
                Return lst
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerTipoEvidencia() As List(Of IQ_TipoEvidencia)
            Try
                _IQ_Entities = New IQ_MODEL
                Dim lst As List(Of IQ_TipoEvidencia)
                lst = (From o In _IQ_Entities.IQ_TipoEvidencia Select o).ToList()

                lst.Insert(0, New IQ_TipoEvidencia With {.TE_Id = "0", .TE_Descripcion = "Seleccione..."})
                Return lst
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerCantidadContactos() As List(Of IQ_CantidadContactos)
            Try
                _IQ_Entities = New IQ_MODEL
                Dim lst As List(Of IQ_CantidadContactos)
                lst = (From o In _IQ_Entities.IQ_CantidadContactos Select o).ToList()

                lst.Insert(0, New IQ_CantidadContactos With {.CC_Id = "0", .CC_Cantidad = "-1"})
                Return lst
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function ObtenerValorVistaMistery(ByVal V As IQ_TarifasMistery) As Decimal

            _IQ_Entities = New IQ_MODEL
            Dim Result As Decimal

            Dim Valor = (From o In _IQ_Entities.IQ_TarifasMistery Where o.TS_Id = V.TS_Id And o.TE_Id = V.TE_Id And o.EV_Id = V.EV_Id And o.CC_Id = V.CC_Id Select o.VM_Valor).FirstOrDefault().ToString

            If Valor IsNot Nothing Then
                Result = CType(Valor, Decimal)
            Else
                Result = 0
            End If


            Return Result


        End Function

        Public Function ObtenerTiempoCritica(ByVal TE As IQ_TipoEvidencia) As Integer
            _IQ_Entities = New IQ_MODEL

            Dim valor = (From t In _IQ_Entities.IQ_TipoEvidencia Where t.TE_Id = TE.TE_Id Select CInt(t.TE_TiempoCritica)).First()

            Return CInt(valor)

        End Function

        Public Function ObtenerIdCombinacion(ByVal TA As IQ_TarifasMistery) As Integer
            _IQ_Entities = New IQ_MODEL

            Dim Id = (From i In _IQ_Entities.IQ_TarifasMistery Where i.TS_Id = TA.TS_Id And i.TE_Id = TA.TE_Id And i.EV_Id = TA.EV_Id And i.CC_Id = TA.CC_Id Select i.VM_Id).First()

            Return CInt(Id)
        End Function




        Public Function ExisteCombinacion(ByVal V As IQ_TarifasMistery) As Boolean

            _IQ_Entities = New IQ_MODEL
            Dim result As Boolean

            Dim Valor = (From o In _IQ_Entities.IQ_TarifasMistery Where o.TS_Id = V.TS_Id And o.TE_Id = V.TE_Id And o.EV_Id = V.EV_Id And o.CC_Id = V.CC_Id Select o).FirstOrDefault()

            If Valor IsNot Nothing Then
                result = True
            Else
                result = False
            End If


            Return result
        End Function

        Public Function ObtenerIdValorVisitaMistery(ByVal v As IQ_TarifasMistery) As IQ_TarifasMistery
            Try

                _IQ_Entities = New IQ_MODEL
                Return (From o In _IQ_Entities.IQ_TarifasMistery Where o.TS_Id = v.TS_Id And o.TE_Id = v.TE_Id And o.EV_Id = v.EV_Id And o.CC_Id = v.CC_Id Select o).First()
            Catch ex As Exception
                Throw ex
            End Try


        End Function

        Public Sub InsertarTarifaMistery(ByVal tar As IQ_ValorVisitaMistery)
            _IQ_Entities = New IQ_MODEL

            If (_IQ_Entities.IQ_ValorVisitaMistery.Any(Function(t) t.VM_Id = tar.VM_Id And t.IdPropuesta = tar.IdPropuesta And t.ParAlternativa = tar.ParAlternativa And t.ParNacional = tar.ParNacional And t.MetCodigo = tar.MetCodigo)) Then
                Dim T1 As IQ_ValorVisitaMistery
                T1 = _IQ_Entities.IQ_ValorVisitaMistery.First(Function(t) t.VM_Id = tar.VM_Id And t.IdPropuesta = tar.IdPropuesta And t.ParAlternativa = tar.ParAlternativa And t.ParNacional = tar.ParNacional And t.MetCodigo = tar.MetCodigo)
                T1.TM_Valor = tar.TM_Valor
                T1.TE_TiempoCritica = tar.TE_TiempoCritica
                _IQ_Entities.SaveChanges()

            Else
                _IQ_Entities.IQ_ValorVisitaMistery.Add(tar)
                _IQ_Entities.SaveChanges()

            End If

        End Sub


        Public Sub InsertarTiempoCritica(ByVal p As IQ_ValorVisitaMistery)

            _IQ_Entities = New IQ_MODEL
            Dim T1 As IQ_ValorVisitaMistery
            T1 = _IQ_Entities.IQ_ValorVisitaMistery.First(Function(t) t.IdPropuesta = p.IdPropuesta And t.ParAlternativa = p.ParAlternativa And t.ParNacional = p.ParNacional And t.MetCodigo = p.MetCodigo And t.VM_Id = p.VM_Id)
            T1.TE_TiempoCritica = p.TE_TiempoCritica
            _IQ_Entities.SaveChanges()


        End Sub


        Public Function ObtenerValorMistery(ByVal p As IQ_Parametros) As List(Of IQ_ObtenerValorMistery_Result)
            _IQ_Entities = New IQ_MODEL
            Return _IQ_Entities.IQ_ObtenerValorMistery(p.IdPropuesta, p.ParAlternativa, p.ParNacional, p.MetCodigo).ToList()

        End Function

        Public Sub BorrarValorMistery(ByVal TM_Id As Decimal)
            _IQ_Entities = New IQ_MODEL
            _IQ_Entities.IQ_BorrarValorMistery(TM_Id)
        End Sub
    End Class
End Namespace

