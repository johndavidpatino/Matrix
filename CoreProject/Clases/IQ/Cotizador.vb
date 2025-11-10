Imports System.Transactions
Imports System.Data.Entity.Core

Namespace Cotizador

    Public Class General
#Region "Variables Globales"
        Private _IQ_Entities As IQ_MODEL
#End Region
#Region "Constructores"
        'Constructor public 
        Public Sub New()
            _IQ_Entities = New IQ_MODEL
        End Sub
#End Region

#Region "GET"

		Public Function GetSolicitudesCambiosGM(ByVal idPropuesta As Int64?, Alternativa As Integer?, AprobadoCFO As Boolean?, AprobadoOPS As Boolean?) As List(Of IQ_SolicitudesCambiosGM_Get_Result)
			Return _IQ_Entities.IQ_SolicitudesCambiosGM_Get(idPropuesta, Alternativa, AprobadoCFO, AprobadoOPS).ToList
		End Function
		Public Function GetAllPresupuestosByAlternativa(ByVal idPropuesta As Int64, idAlternativa As Integer) As List(Of IQ_Parametros)
            Return _IQ_Entities.IQ_Parametros.Where(Function(x) x.IdPropuesta = idPropuesta And x.ParAlternativa = idAlternativa).ToList
        End Function
        Public Function GetGeneralByAlternativa(ByVal idPropuesta As Int64, ByVal idAlternativa As Integer) As IQ_DatosGeneralesPresupuesto
            Return (From d In _IQ_Entities.IQ_DatosGeneralesPresupuesto Where d.IdPropuesta = idPropuesta And d.ParAlternativa = idAlternativa Select d).First()
        End Function

		Public Function GetPresupuestosByTecnica(ByVal idPropuesta As Int64, ByVal IdAlternativa As Integer, ByVal Tecnica As Integer?) As List(Of IQ_ObtenerNacionales_Result)
			Return (From c In _IQ_Entities.IQ_ObtenerNacionales(idPropuesta, Tecnica, IdAlternativa)).ToList()
		End Function

		Public Function GetMetodologiasByTecnica(ByVal Tecnica As Integer) As List(Of IQ_ObtenerMetodologias_Result)
            Return _IQ_Entities.IQ_ObtenerMetodologias(Tecnica).ToList()
        End Function

        Public Function GetFasesByTecnica(ByVal Tecnica As Integer) As List(Of IQ_ObtenerFases_Result)
            Return _IQ_Entities.IQ_ObtenerFases(Tecnica).ToList()
        End Function

        Public Function GetTipoMuestraByMetodologia(ByVal metodologia As Integer) As List(Of IQ_ObtenerOpcionesMuestra_Result)
            Return _IQ_Entities.IQ_ObtenerOpcionesMuestra(metodologia).ToList()
            'lst.Insert(0, New IQ_ObtenerOpcionesMuestra_Result With {.IdIdentificador = "0", .DescIdentMuestra = "Seleccione..."})
        End Function

		Public Function GetPresupuesto(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As IQ_Parametros
			Dim p = (From p1 In _IQ_Entities.IQ_Parametros Where p1.IdPropuesta = idPropuesta And p1.ParAlternativa = Alternativa And p1.MetCodigo = Metodologia And p1.ParNacional = Fase Select p1).First()
			Return p
		End Function

		Public Function GetProcesos(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of IQ_ProcesosPresupuesto)
            Dim LstProc As List(Of IQ_ProcesosPresupuesto)
            LstProc = (From proc1 In _IQ_Entities.IQ_ProcesosPresupuesto Where proc1.IdPropuesta = idPropuesta And proc1.ParAlternativa = Alternativa And proc1.MetCodigo = Metodologia And proc1.ParNacional = Fase Select proc1).ToList()
            Return LstProc
        End Function

        Public Function GetAnalisisEstadisticos(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of IQ_ObtenerAnalisisEstadisticos_Result)
            Return _IQ_Entities.IQ_ObtenerAnalisisEstadisticos(idPropuesta, Alternativa, Fase, Metodologia).ToList
        End Function

        Public Function GetActividadesSubcontratadas(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of IQ_ObtenerActividades_Result)
            Return _IQ_Entities.IQ_ObtenerActividades(idPropuesta, Alternativa, Fase, Metodologia).ToList()
        End Function
        Public Function GetHorasProfesionales(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer?, Fase As Integer?) As List(Of IQ_ObtenerHorasProfesionales_Result)
            Return _IQ_Entities.IQ_ObtenerHorasProfesionales(idPropuesta, Alternativa, Metodologia, Fase).ToList()
        End Function
        Public Function GetHorasProfesionalesByAlternativa(ByVal idPropuesta As Int64, Alternativa As Integer) As List(Of IQ_ObtenerHorasProfesionalesXAlternativa_Result)
            Return _IQ_Entities.IQ_ObtenerHorasProfesionalesXAlternativa(idPropuesta, Alternativa).ToList()
        End Function
        Public Function GetListadoCiudadesGeneral(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of IQ_ObtenerCiudadesMuestraGeneral_Result)
            Return _IQ_Entities.IQ_ObtenerCiudadesMuestraGeneral(idPropuesta, Alternativa, Fase, Metodologia).ToList()
        End Function
        Public Function GetUltimaAlternativa(ByVal IdPropuesta As Int64) As Integer
            Try
                Dim ultima As Integer = 0
                If _IQ_Entities.IQ_DatosGeneralesPresupuesto.Where(Function(x) x.IdPropuesta = IdPropuesta).ToList.Count > 0 Then
                    ultima = (From p In _IQ_Entities.IQ_DatosGeneralesPresupuesto Where p.IdPropuesta = IdPropuesta Select CType(p.ParAlternativa, Integer?)).Max()
                End If
                Return ultima
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Function GetExistsPresupuesto(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As Boolean
            If _IQ_Entities.IQ_Parametros.Where(Function(x) x.IdPropuesta = idPropuesta And x.ParAlternativa = Alternativa And x.MetCodigo = Metodologia And x.ParNacional = Fase).ToList.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function GetCalculoProductividad(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, ByVal Tecnica As Integer) As Decimal
            If _IQ_Entities.IQ_Muestra.Where(Function(x) x.IdPropuesta = idPropuesta And x.ParAlternativa = Alternativa And x.MetCodigo = Metodologia And x.ParNacional = Fase).ToList.Count > 0 Then
                Dim output As New Objects.ObjectParameter("Productividad", GetType(Decimal))
                If Tecnica = 100 Then _IQ_Entities.IQ_CARACARAProductividad(idPropuesta, Alternativa, Metodologia, Fase, output)
                If Tecnica = 200 Then _IQ_Entities.IQ_CATIProductividad(idPropuesta, Alternativa, Metodologia, Fase)
                If Tecnica = 300 Then _IQ_Entities.IQ_ONLINEProductividad(idPropuesta, Alternativa, Metodologia, Fase, output)
                If Tecnica = 200 Then
                    Return _IQ_Entities.IQ_Parametros.Where(Function(x) x.IdPropuesta = idPropuesta And x.ParAlternativa = Alternativa And x.MetCodigo = Metodologia And x.ParNacional = Fase).FirstOrDefault.ParProductividad
                Else
                    Return CDec(output.Value)
                End If
            Else
                Return 1
            End If
        End Function
        Private Function GetCostoDirecto(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, ByVal Tecnica As Integer) As Decimal
            If Tecnica = 100 Then
                Dim outputF2F As New Objects.ObjectParameter("CostoDirecto", GetType(Decimal))
                _IQ_Entities.IQ_CARACARACostosDirectos(idPropuesta, Alternativa, Metodologia, Fase, outputF2F)
                Return CDec(outputF2F.Value)
            End If
            If Tecnica = 200 Then
                Dim outputCATI As New Objects.ObjectParameter("CATICostoDirecto", GetType(Decimal))
                _IQ_Entities.IQ_CATICostosDirectos(idPropuesta, Alternativa, Metodologia, Fase, outputCATI)
                Return CDec(outputCATI.Value)
            End If
            If Tecnica = 300 Then
                Dim outputOnLine As New Objects.ObjectParameter("CostoDirecto", GetType(Decimal))
                _IQ_Entities.IQ_ONLINECostosDirectos(idPropuesta, Alternativa, Metodologia, Fase, outputOnLine)
                Return CDec(outputOnLine.Value)
            End If
            Return 0
        End Function

        Public Function GetActividadesNoAplicanGM(ByVal p As IQ_Parametros) As Decimal
            Dim ValorRestar As Decimal
            Dim V = (From c In _IQ_Entities.IQ_CostoActividades Join a In _IQ_Entities.IQ_Actividades On c.ActCodigo Equals a.ID Where a.AplicaGM = False And c.IdPropuesta = p.IdPropuesta And c.ParAlternativa = p.ParAlternativa And c.ParNacional = p.ParNacional And c.MetCodigo = p.MetCodigo Select c.CaCosto).ToArray()
            ValorRestar = If(V.Sum() = Nothing, 0, V.Sum)
            Return ValorRestar
        End Function

        Public Function GetMuestraF2F(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of IQ_ObtenerMuestraF2F_Result)
            Return _IQ_Entities.IQ_ObtenerMuestraF2F(idPropuesta, Alternativa, Metodologia, Fase).ToList
        End Function
        Public Function GetMuestraCati(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of IQ_ObtenerMuestraCatiShow_Result)
            Return _IQ_Entities.IQ_ObtenerMuestraCatiShow(idPropuesta, Alternativa, Metodologia, Fase).ToList
        End Function
        Public Function GetMuestraOnline(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of IQ_ObtenerMuestraOnLineShow_Result)
            Return _IQ_Entities.IQ_ObtenerMuestraOnLineShow(idPropuesta, Alternativa, Metodologia, Fase).ToList
        End Function
        Public Function GetTotalMuestra(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As Integer?
            Try
                Return (From t In _IQ_Entities.IQ_Muestra Where t.IdPropuesta = idPropuesta And t.ParAlternativa = Alternativa And t.MetCodigo = Metodologia And t.ParNacional = Fase And Not (t.MuIdentificador = 1) Select t.MuCantidad).Sum()
            Catch ex As Exception
                Return 0
            End Try
        End Function
        Public Function GetCostosJBI(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, GMU As Decimal, GMO As Decimal, simular As Boolean) As List(Of IQ_CostosJobBookInternoTotales_Result)
            Return _IQ_Entities.IQ_CostosJobBookInternoTotales(idPropuesta, Alternativa, Metodologia, Fase, GMU, GMO, simular).ToList()
        End Function

        Public Function GetCostosJBE(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, GMU As Decimal, GMO As Decimal, simular As Boolean) As List(Of IQ_CostosJobBookExternoTotales_Result)
            Return _IQ_Entities.IQ_CostosJobBookExternoTotales(idPropuesta, Alternativa, Metodologia, Fase, GMU, GMO, simular).ToList()
        End Function

        Public Function GetSimularGM(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, ByVal ValorVenta As Decimal, ByVal Simular As Boolean) As Decimal
            Return _IQ_Entities.IQ_AjustarGM(idPropuesta, Alternativa, Metodologia, Fase, ValorVenta, Simular).First()
        End Function

        Public Function GetSimularVenta(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, ByVal GM_Unidad As Decimal, ByVal GM_Operaciones As Decimal, ByVal Simular As Boolean) As Decimal
            Return _IQ_Entities.IQ_AjustarVenta(idPropuesta, Alternativa, Metodologia, Fase, GM_Unidad, GM_Operaciones, Simular).First()
        End Function

        Public Function GetParametrosGenerales(ByVal PGrCodigo As Integer) As Double
            Return (From t In _IQ_Entities.IQ_ParametrosGenerales Where t.PGrCodigo = PGrCodigo Select t.PGrValor1).First()
        End Function

        Public Function GetValorTotalXalternativa(ByVal idPropuesta As Int64, Alternativa As Integer) As Decimal
            Return (From o In _IQ_Entities.IQ_Parametros Where o.IdPropuesta = idPropuesta And o.ParAlternativa = Alternativa Select o.ParValorVenta).Sum()
        End Function

        Public Function GetDatosPropuesta(ByVal idPropuesta As Long) As IQ_ObtenerDatosPropuesta_Result
            Return _IQ_Entities.IQ_ObtenerDatosPropuesta(idPropuesta).First()
        End Function

        Public Function GetValidarUsuarioAutorizado(ByVal Unit As Integer, ByVal User As Decimal) As Boolean
            'Validamos si el usuario existe y ademas esta autorizadoa modificar dentro de la unidad respectiva .
            If (_IQ_Entities.IQ_AutorizadosModificacionGM.Any(Function(a) a.IdUsuario = User And a.Unidad = Unit)) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function GetPresupuestosModificarGM(ByVal alternativa As Integer, ByVal propuesta As Long) As List(Of IQ_Parametros)
            Dim lst As List(Of IQ_Parametros)
            lst = (From p In _IQ_Entities.IQ_Parametros Where p.IdPropuesta = propuesta And p.ParAlternativa = alternativa Select p).ToList()
            Return lst
        End Function

        Public Function GetTiposPresupuestoXCuPresupuesto(ByVal idPresupuesto As Int64) As IQ_TipoPresupuestosXCuPresupuesto_Result
            Return _IQ_Entities.IQ_TipoPresupuestosXCuPresupuesto(idPresupuesto).First
        End Function

        Public Function GetCostosJobBookExterno(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As DataSet
            Try
                Dim cnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ConnectionString
                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_CostosJobBookExterno", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = idPropuesta
                    command.Parameters.Add("ParAlternativa", SqlDbType.Int).Value = Alternativa
                    command.Parameters.Add("ParNacional", SqlDbType.Int).Value = Fase
                    command.Parameters.Add("MetCodigo", SqlDbType.Int).Value = Metodologia
                    Using da As New SqlClient.SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetCostosJobBookExternoObserver(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As DataSet
            Try
                Dim cnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ConnectionString
                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_CostosJobBookExternoObserver", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = idPropuesta
                    command.Parameters.Add("ParAlternativa", SqlDbType.Int).Value = Alternativa
                    command.Parameters.Add("ParNacional", SqlDbType.Int).Value = Fase
                    command.Parameters.Add("MetCodigo", SqlDbType.Int).Value = Metodologia
                    Using da As New SqlClient.SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetCostosJobBookInterno(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As DataSet
            Try
                Dim cnString As String = System.Configuration.ConfigurationManager.ConnectionStrings("MatrixConnectionString").ConnectionString
                Dim SQLcon As SqlClient.SqlConnection = New SqlClient.SqlConnection(cnString)
                Dim Ds As New DataSet()
                Using command As New SqlClient.SqlCommand("IQ_CostosJobBookInterno", SQLcon)
                    command.CommandType = CommandType.StoredProcedure
                    command.Connection.Open()
                    command.Parameters.Add("IdPropuesta", SqlDbType.Int).Value = idPropuesta
                    command.Parameters.Add("ParAlternativa", SqlDbType.Int).Value = Alternativa
                    command.Parameters.Add("ParNacional", SqlDbType.Int).Value = Fase
                    command.Parameters.Add("MetCodigo", SqlDbType.Int).Value = Metodologia
                    Using da As New SqlClient.SqlDataAdapter(command)
                        da.Fill(Ds)
                    End Using
                End Using

                Return Ds
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function GetCostos(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, ByVal TipoDetalle As Integer) As List(Of IQ_ObtenerActControlCostos_Result)
            Return _IQ_Entities.IQ_ObtenerActControlCostos(idPropuesta, Alternativa, Fase, Metodologia, TipoDetalle).ToList()
        End Function

        Public Function GetViaticos(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer) As List(Of IQ_ObtenerViaticosPresupuesto_Result)
            Return _IQ_Entities.IQ_ObtenerViaticosPresupuesto(idPropuesta, Alternativa, Metodologia, Fase).ToList()
        End Function

        Public Function ExistsAlternativaMarkedToExport(ByVal Propuesta As Int32, ByVal Alternativa As Int32) As Boolean
            If _IQ_Entities.IQ_AlternativasToExportIQuote.Where(Function(x) x.idPropuesta = Propuesta And x.Alternativa = Alternativa And x.FechaExportacion Is Nothing).ToList.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function
        Public Function ExistsAlternativaMarkedToExportPrevious(ByVal Propuesta As Int32, ByVal Alternativa As Int32) As Boolean
            If _IQ_Entities.IQ_AlternativasToExportIQuote.Where(Function(x) x.idPropuesta = Propuesta And x.Alternativa = Alternativa And x.FechaExportacion IsNot Nothing).ToList.Count > 0 Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function GetPyG(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer?, Fase As Integer?) As List(Of IQ_CalculoPyG_Result)
            Return _IQ_Entities.IQ_CalculoPyG(idPropuesta, Alternativa, Metodologia, Fase).ToList
        End Function

        Public Function GetCalculoDiasCampo(ByVal idPropuesta As Int64, Alternativa As Integer) As Integer
            Dim Dias As Integer? = _IQ_Entities.IQ_CalculoGeneralDiasCampo(idPropuesta, Alternativa)(0)
            If Dias Is Nothing Then
                Return 10
            Else
                Return Dias
            End If
        End Function

        Public Function GetSimulador(ByVal idPropuesta As Int64?, Alternativa As Integer?, Metodologia As Integer?, Fase As Integer?, TipoCalculo As Integer, VrVenta As Decimal?, GM As Decimal?, OP As Decimal?, GMOps As Decimal?) As IQ_Simulador_Result
            Return _IQ_Entities.IQ_Simulador(idPropuesta, Alternativa, Fase, Metodologia, TipoCalculo, VrVenta, GM, OP, GMOps).FirstOrDefault()
        End Function


        Public Function GetSolicitudesSimulador(ByVal Id As Int64?, ByVal idPropuesta As Int64?, Alternativa As Integer?, Metodologia As Integer?, Fase As Integer?, Aprobado As Boolean?, SL As Integer?, FSIni As Date?, FSEnd As Date?, FAIni As Date?, FAEnd As Date?, SolicitadoPor As Long?) As List(Of IQ_SolicitudesSimulador_Result)
            Return _IQ_Entities.IQ_SolicitudesSimulador(Id, idPropuesta, Alternativa, Fase, Metodologia, Aprobado, SL, FSIni, FSEnd, FAIni, FAEnd, SolicitadoPor).ToList
        End Function

        Public Function GetSolicitudSimulador(ByVal id As Int64?) As IQ_SolicitudesAjustesPresupuesto
            Return _IQ_Entities.IQ_SolicitudesAjustesPresupuesto.Where(Function(x) x.id = id).FirstOrDefault
        End Function
#End Region

#Region "PUT"

        Public Sub PutSolicitud(ByRef ent As IQ_SolicitudesAjustesPresupuesto)
            If ent.id = 0 Then
                _IQ_Entities.IQ_SolicitudesAjustesPresupuesto.Add(ent)
            End If
            _IQ_Entities.SaveChanges()
        End Sub
        Public Sub PutDatosGenerales(ByVal par As IQ_DatosGeneralesPresupuesto)
            Try
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

        Public Sub PutDuplicar(ByVal IdPropuesta As Int64, Alternativa As Int32)
            _IQ_Entities.IQ_DuplicarAlternativa(IdPropuesta, Alternativa)
        End Sub

        Public Sub PutDuplicarToPropuesta(ByVal IdPropuesta As Int64, Alternativa As Int32, ByVal NewPropuesta As Int64)
            _IQ_Entities.IQ_DuplicarAlternativaToPropuesta(IdPropuesta, Alternativa, NewPropuesta)
        End Sub

        Public Sub PutSaveParametros(ByRef Parametros As IQ_Parametros, ByVal NewPresupuesto As Boolean)
            If NewPresupuesto = True Then
                _IQ_Entities.IQ_Parametros.Add(Parametros)
            End If
            _IQ_Entities.SaveChanges()
        End Sub


        Public Sub PutValorVenta(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, ByVal Tecnica As Integer, ByVal Unidad As Integer)
            Dim IQP = GetPresupuesto(idPropuesta, Alternativa, Metodologia, Fase)
            PutCostoGasto(idPropuesta, Alternativa, Metodologia, Fase, Tecnica, IQP)
            Dim output As New Objects.ObjectParameter("ValorVenta", GetType(Decimal))
            _IQ_Entities.IQ_ValorVenta(idPropuesta, Alternativa, Metodologia, Fase, Unidad, IQP.ParActSubGasto, IQP.ParCostoDirecto, output)
            IQP.ParValorVenta = CDec(output.Value)
            IQP.ParGrossMargin = ((IQP.ParValorVenta) - (IQP.ParCostoDirecto + ((IQP.ParActSubGasto)))) / (IQP.ParValorVenta)
            PutSaveParametros(IQP, False)
        End Sub

        Public Sub PutOP(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, ByVal OP As Double?)
            Dim IQP = GetPresupuesto(idPropuesta, Alternativa, Metodologia, Fase)
            IQP.OP = OP
            PutSaveParametros(IQP, False)
        End Sub

        Private Sub PutCostoGasto(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, ByVal Tecnica As Integer, ByRef IQP As IQ_Parametros)
            Dim ActCosto As New Objects.ObjectParameter("ValorActividadCosto", GetType(Decimal))
            Dim ActGasto As New Objects.ObjectParameter("ValorActividadGasto", GetType(Decimal))
            _IQ_Entities.IQ_ActividadesSubcontratadas(idPropuesta, Alternativa, Metodologia, Fase, ActCosto, ActGasto)
            IQP.ParActSubCosto = CDec(ActCosto.Value)
            IQP.ParActSubGasto = CDec(ActGasto.Value)
            IQP.ParCostoDirecto = GetCostoDirecto(idPropuesta, Alternativa, Metodologia, Fase, Tecnica)
        End Sub

        Public Sub PutPreguntas(ByVal Preguntas As IQ_Preguntas)
            'Insertamos los totales de preguntas digitadas
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
        End Sub

        Public Sub PutProcesos(ByVal LstProcesos As List(Of IQ_ProcesosPresupuesto), ByVal PAR As IQ_Parametros)

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

        End Sub

        Public Sub PutMuestra(ByVal m1 As IQ_Muestra_1)
            If m1.MuIdentificador = 1 Then
                _IQ_Entities.IQ_DistribuirMuestra_WORK(m1.CiuCodigo, m1.MuCantidad, m1.IdPropuesta, m1.ParNacional, m1.MetCodigo, m1.ParAlternativa)
            End If
            If Not m1.MuIdentificador = 60 And Not m1.MuIdentificador = 70 And Not m1.MuIdentificador = 1 Then
                _IQ_Entities.IQ_InsertarMuestra_WORK(m1.CiuCodigo, m1.MuCantidad, m1.MuIdentificador, m1.IdPropuesta, m1.ParNacional, m1.MetCodigo, m1.ParAlternativa)
            Else
                If _IQ_Entities.IQ_Muestra.Any(Function(m2) m2.IdPropuesta = m1.IdPropuesta And m2.ParAlternativa = m1.ParAlternativa And m2.MetCodigo = m1.MetCodigo And m2.MuIdentificador = m1.MuIdentificador And m2.ParNacional = m1.ParNacional) Then
                    Dim m4 = _IQ_Entities.IQ_Muestra.First(Function(m3) m3.IdPropuesta = m1.IdPropuesta And m3.ParAlternativa = m1.ParAlternativa And m3.MetCodigo = m1.MetCodigo And m3.MuIdentificador = m1.MuIdentificador And m3.ParNacional = m1.ParNacional)
                    m4.MuCantidad = m1.MuCantidad
                    _IQ_Entities.SaveChanges()
                Else
                    If Not m1.MuIdentificador = 1 Then _IQ_Entities.IQ_Muestra.Add(m1)
                    _IQ_Entities.SaveChanges()
                End If
            End If
        End Sub

		Public Sub PutSolicitudGM(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer, VrVenta As Decimal?, GMUnidad As Decimal?, GMOPS As Decimal?, Usuario As Int64)
			_IQ_Entities.IQ_SolicitarCambioGM(idPropuesta, Alternativa, Metodologia, Fase, GMUnidad, GMOPS, VrVenta, Usuario)
		End Sub

		Public Sub PutActividadesSubcontratadas(ByVal Lstact As List(Of IQ_CostoActividades))
            For Each act In Lstact
                If _IQ_Entities.IQ_CostoActividades.Any(Function(a) a.IdPropuesta = act.IdPropuesta And a.ParAlternativa = act.ParAlternativa And a.MetCodigo = act.MetCodigo And a.ActCodigo = act.ActCodigo And a.ParNacional = act.ParNacional) Then
                    'SI EXISTE Y EL  NUEVO VALOR ES IGUAL A 0 SE DEBE ELIMINAR DE LA BASE 
                    If act.CaCosto = 0 Then
                        Dim ActDel = _IQ_Entities.IQ_CostoActividades.First(Function(b) b.IdPropuesta = act.IdPropuesta And b.ParAlternativa = act.ParAlternativa And b.MetCodigo = act.MetCodigo And b.ActCodigo = act.ActCodigo And b.ParNacional = act.ParNacional)
                        _IQ_Entities.IQ_CostoActividades.Remove(ActDel)
                    Else
                        Dim ActOrig = _IQ_Entities.IQ_CostoActividades.First(Function(b) b.IdPropuesta = act.IdPropuesta And b.ParAlternativa = act.ParAlternativa And b.MetCodigo = act.MetCodigo And b.ActCodigo = act.ActCodigo And b.ParNacional = act.ParNacional)
                        ActOrig.CaCosto = act.CaCosto
                        ActOrig.CaUnidades = act.CaUnidades
                        ActOrig.CaDescripcionUnidades = act.CaDescripcionUnidades
                    End If
                    _IQ_Entities.SaveChanges()
                Else
                    If act.CaCosto > 0 Then
                        _IQ_Entities.IQ_CostoActividades.Add(act)
                        _IQ_Entities.SaveChanges()
                    End If
                End If
            Next
        End Sub


        Public Sub PutModelosEstadistica(ByVal Lstact As List(Of IQ_AnalisisEstadisticaPresupuesto))
            For Each act In Lstact
                If _IQ_Entities.IQ_AnalisisEstadisticaPresupuesto.Any(Function(a) a.IdPropuesta = act.IdPropuesta And a.ParAlternativa = act.ParAlternativa And a.MetCodigo = act.MetCodigo And a.IdAnalisis = act.IdAnalisis And a.ParNacional = act.ParNacional) Then
                    'SI EXISTE Y EL  NUEVO VALOR ES IGUAL A 0 SE DEBE ELIMINAR DE LA BASE 
                    If act.Cantidad = 0 Then
                        Dim ActDel = _IQ_Entities.IQ_AnalisisEstadisticaPresupuesto.First(Function(b) b.IdPropuesta = act.IdPropuesta And b.ParAlternativa = act.ParAlternativa And b.MetCodigo = act.MetCodigo And b.IdAnalisis = act.IdAnalisis And b.ParNacional = act.ParNacional)
                        _IQ_Entities.IQ_AnalisisEstadisticaPresupuesto.Remove(ActDel)
                    Else
                        Dim ActOrig = _IQ_Entities.IQ_AnalisisEstadisticaPresupuesto.First(Function(b) b.IdPropuesta = act.IdPropuesta And b.ParAlternativa = act.ParAlternativa And b.MetCodigo = act.MetCodigo And b.IdAnalisis = act.IdAnalisis And b.ParNacional = act.ParNacional)
                        ActOrig.Cantidad = act.Cantidad
                        ActOrig.VrTotal = act.VrTotal
                    End If
                    _IQ_Entities.SaveChanges()
                Else
                    If act.Cantidad > 0 Then
                        _IQ_Entities.IQ_AnalisisEstadisticaPresupuesto.Add(act)
                        _IQ_Entities.SaveChanges()
                    End If
                End If
            Next
        End Sub

        Public Sub PutHorasProfesionales(ByVal Lstact As List(Of IQ_HorasProfesionales))
            For Each hor In Lstact
                If _IQ_Entities.IQ_HorasProfesionales.Any(Function(a) a.IdPropuesta = hor.IdPropuesta And a.ParAlternativa = hor.ParAlternativa And a.MetCodigo = hor.MetCodigo And a.CodCargo = hor.CodCargo And a.ParNacional = hor.ParNacional) Then
                    'SI EXISTE Y EL  NUEVO VALOR ES IGUAL A 0 SE DEBE ELIMINAR DE LA BASE 
                    If hor.Horas = 0 Then
                        Dim ActDel = _IQ_Entities.IQ_HorasProfesionales.First(Function(b) b.IdPropuesta = hor.IdPropuesta And b.ParAlternativa = hor.ParAlternativa And b.MetCodigo = hor.MetCodigo And b.CodCargo = hor.CodCargo And b.ParNacional = hor.ParNacional)
                        _IQ_Entities.IQ_HorasProfesionales.Remove(ActDel)
                    Else
                        Dim ActOrig = _IQ_Entities.IQ_HorasProfesionales.First(Function(b) b.IdPropuesta = hor.IdPropuesta And b.ParAlternativa = hor.ParAlternativa And b.MetCodigo = hor.MetCodigo And b.CodCargo = hor.CodCargo And b.ParNacional = hor.ParNacional)
                        ActOrig.PreField = hor.PreField
                        ActOrig.FieldWork = hor.FieldWork
                        ActOrig.DPandCoding = hor.DPandCoding
                        ActOrig.Analysis = hor.Analysis
                        ActOrig.PM = hor.PM
                        ActOrig.Meetings = hor.Meetings
                        ActOrig.Presentation = hor.Presentation
                        ActOrig.ClientTravel = hor.ClientTravel
                        ActOrig.Horas = hor.Horas
                    End If
                    _IQ_Entities.SaveChanges()
                Else
                    If hor.Horas > 0 Then
                        _IQ_Entities.IQ_HorasProfesionales.Add(hor)
                        _IQ_Entities.SaveChanges()
                    End If
                End If
            Next
        End Sub


        Public Sub PutCalcularGrossMarginXAlternativa(ByVal lst As List(Of IQ_Parametros), ByVal NuevoGM As Decimal, ByVal TipoCalculo As Integer, ByVal ValorVenta As Decimal, ByVal GM_UNI As Decimal, ByVal GM_OPE As Decimal)
            Using scope As New TransactionScope()

                For Each o In lst
                    PUTModificarGrossMargin(NuevoGM, o, TipoCalculo, ValorVenta, GM_UNI, GM_OPE)
                Next
                scope.Complete()
            End Using
        End Sub

        Public Sub PUTModificarGrossMargin(ByVal GM As Double, ByVal Par As IQ_Parametros, ByVal TipoCalculo As Integer, ByVal ValorVenta As Decimal, ByVal GM_UNI As Decimal, ByVal GM_OPE As Decimal)
            '1. Modificamos el GM 
            '2.Ajustamos el valor de venta
            '3.Ajustamos el costo directo 
            Dim retorno As Decimal
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

        End Sub

        Public Sub PUTCalculoHorasProfesionales(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer?, Fase As Integer?)
            _IQ_Entities.IQ_CalculoProfessionalTime(idPropuesta, Alternativa, Metodologia, Fase)
        End Sub

        Public Sub PUTCalculoHorasProfesionalesVenta(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer?, Fase As Integer?, ValorVenta As Double)
            _IQ_Entities.IQ_CalculoProfessionalTime_ValorVenta(idPropuesta, Alternativa, Metodologia, Fase, ValorVenta)
        End Sub

        Public Sub MarkAlternativaToExportToIQuote(ByVal Propuesta As Int32, ByVal Alternativa As Int32)
            _IQ_Entities.IQ_AlternativasToExportIQuote.Add(New IQ_AlternativasToExportIQuote With {.idPropuesta = Propuesta, .Alternativa = Alternativa, .Fecha = Now})
            _IQ_Entities.SaveChanges()
        End Sub

#End Region

#Region "DEL"

        Public Sub DELPresupuesto(ByVal idPropuesta As Int64, Alternativa As Integer, Metodologia As Integer, Fase As Integer)
            _IQ_Entities.IQ_BorrarPresupuesto(idPropuesta, Alternativa, Fase, Metodologia)
        End Sub

        Public Sub DELMuestra(ByVal vM As IQ_Muestra_1)
            Dim oM As IQ_Muestra_1
            If vM.CiuCodigo = 0 Then
                oM = _IQ_Entities.IQ_Muestra.First(Function(x) x.IdPropuesta = vM.IdPropuesta And x.ParAlternativa = vM.ParAlternativa And x.MetCodigo = vM.MetCodigo And x.ParNacional = vM.ParNacional And x.MuIdentificador = vM.MuIdentificador)
                _IQ_Entities.IQ_Muestra.Remove(oM)
                _IQ_Entities.SaveChanges()
            Else
                _IQ_Entities.IQ_BorrarMuestra(vM.IdPropuesta, vM.ParAlternativa, vM.ParNacional, vM.MetCodigo, vM.CiuCodigo)
            End If
        End Sub
#End Region


    End Class


End Namespace
