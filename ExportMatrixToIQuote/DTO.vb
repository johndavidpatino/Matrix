Public Class DTO
    Partial Public Class IQ_Parametros
        Public Property IdPropuesta As Long
        Public Property ParAlternativa As Integer
        Public Property MetCodigo As Integer
        Public Property ParNacional As Integer
        Public Property TipoProyecto As Nullable(Of Integer)
        Public Property ParNomPresupuesto As String
        Public Property Pr_ProductCode As String
        Public Property TecCodigo As Nullable(Of Integer)
        Public Property ParTotalPreguntas As Nullable(Of Integer)
        Public Property ParPaginasEncuesta As Nullable(Of Integer)
        Public Property ParHorasEntrevista As Nullable(Of Integer)
        Public Property ParNumAsistentesSesion As Nullable(Of Integer)
        Public Property ParEncuestadoresPunto As Nullable(Of Integer)
        Public Property ParProductividad As Nullable(Of Double)
        Public Property ParProductividadOriginal As Nullable(Of Double)
        Public Property ParContactosNoEfectivos As Nullable(Of Integer)
        Public Property ParContactosNoEfectivosOriginales As Nullable(Of Integer)
        Public Property ParTiempoEncuesta As Nullable(Of Integer)
        Public Property Usuario As Nullable(Of Decimal)
        Public Property ParFechaCreacion As Nullable(Of Date)
        Public Property ParValorDolar As Nullable(Of Decimal)
        Public Property ParAprobado As Nullable(Of Boolean)
        Public Property ParFechaAprobacion As Nullable(Of Date)
        Public Property ParPresupuestoEnUso As Nullable(Of Boolean)
        Public Property ParUsuarioTieneUso As Nullable(Of Decimal)
        Public Property ParFactorAjustado As Nullable(Of Integer)
        Public Property ParNumJobBook As String
        Public Property ParNProcesosDC As Nullable(Of Integer)
        Public Property ParNProcesosTopLines As Nullable(Of Integer)
        Public Property ParNProcesosTablas As Nullable(Of Integer)
        Public Property ParNProcesosBases As Nullable(Of Integer)
        Public Property ParGrupoObjetivo As String
        Public Property ParIncidencia As Nullable(Of Integer)
        Public Property ParDiasEncuestador As Nullable(Of Integer)
        Public Property ParDiasSupervisor As Nullable(Of Integer)
        Public Property ParDiasCoordinador As Nullable(Of Integer)
        Public Property ParUnidad As Nullable(Of Integer)
        Public Property ParGrossMargin As Nullable(Of Double)
        Public Property ParValorVenta As Nullable(Of Decimal)
        Public Property ParCostoDirecto As Nullable(Of Decimal)
        Public Property ParActSubGasto As Nullable(Of Decimal)
        Public Property Pr_Offeringcode As String
        Public Property ParActSubCosto As Nullable(Of Decimal)
        Public Property ParUsaLista As Nullable(Of Boolean)
        Public Property ParRevisado As Nullable(Of Boolean)
        Public Property ParRevisadoPor As Nullable(Of Decimal)
        Public Property ParFechaRevision As Nullable(Of Date)
        Public Property ParProbabilistico As Nullable(Of Boolean)
        Public Property ParDicultadTargetCualitativo As String
        Public Property ParViaticosReclutamiento As Nullable(Of Boolean)
        Public Property ParViaticosModeracion As Nullable(Of Boolean)
        Public Property ParViaticosInforme As Nullable(Of Boolean)
        Public Property ParEditaVideo As Nullable(Of Boolean)
        Public Property ParTransmiteInternet As Nullable(Of Boolean)
        Public Property ParQAP As Nullable(Of Boolean)
        Public Property ParPorcentajeIntercep As Nullable(Of Integer)
        Public Property ParPorcentajeRecluta As Nullable(Of Integer)
        Public Property ParUnidadesProducto As Nullable(Of Integer)
        Public Property ParValorUnitarioProd As Nullable(Of Decimal)
        Public Property ParTipoCLT As Nullable(Of Integer)
        Public Property ParAlquilerEquipos As Nullable(Of Decimal)
        Public Property ParApoyoLogistico As Nullable(Of Boolean)
        Public Property ParAccesoInternet As Nullable(Of Boolean)
        Public Property ParObservaciones As String
        Public Property ParSubcontratar As Nullable(Of Boolean)
        Public Property ParPorcentajeSub As Nullable(Of Integer)
        Public Property ParUsaTablet As Nullable(Of Boolean)
        Public Property ParUsaPapel As Nullable(Of Boolean)
        Public Property ParDispPropio As Nullable(Of Boolean)
        Public Property ParAñoSiguiente As Nullable(Of Boolean)
        Public Property TipoPresupuesto As Nullable(Of Byte)
        Public Property Complejidad As Nullable(Of Byte)
        Public Property F2FVirtual As Nullable(Of Boolean)
        Public Property ComplejidadCodificacion As Nullable(Of Byte)
        Public Property DPTransformacion As Nullable(Of Boolean)
        Public Property DPUnificacion As Nullable(Of Boolean)
        Public Property DPComplejidad As Nullable(Of Byte)
        Public Property DPPonderacion As Nullable(Of Byte)
        Public Property DPInInterna As Nullable(Of Boolean)
        Public Property DPInCliente As Nullable(Of Boolean)
        Public Property DPInPanel As Nullable(Of Boolean)
        Public Property DPInExterno As Nullable(Of Boolean)
        Public Property DPInGMU As Nullable(Of Boolean)
        Public Property DPInOtro As Nullable(Of Boolean)
        Public Property DPOutCliente As Nullable(Of Boolean)
        Public Property DPOutWebDelivery As Nullable(Of Boolean)
        Public Property DPOutExterno As Nullable(Of Boolean)
        Public Property DPOutGMU As Nullable(Of Boolean)
        Public Property DPOutOtro As Nullable(Of Boolean)
        Public Property PTApoyosPunto As Nullable(Of Byte)
        Public Property PTCompra As Nullable(Of Boolean)
        Public Property PTNeutralizador As Nullable(Of Boolean)
        Public Property PTTipoProducto As Nullable(Of Byte)
        Public Property PTLotes As Nullable(Of Byte)
        Public Property PTVisitas As Nullable(Of Byte)
        Public Property PTCeldas As Nullable(Of Byte)
        Public Property PTProductosEvaluar As Nullable(Of Byte)
        Public Property DPComplejidadCuestionario As Nullable(Of Byte)
        Public Property NoIQuote As String

    End Class
    Partial Public Class IQ_DatosGeneralesPresupuesto
        Public Property IdPropuesta As Long
        Public Property ParAlternativa As Integer
        Public Property Descripcion As String
        Public Property Observaciones As String
        Public Property DiasCampo As Integer
        Public Property DiasDiseno As Nullable(Of Integer)
        Public Property DiasProcesamiento As Nullable(Of Integer)
        Public Property DiasInformes As Nullable(Of Integer)
        Public Property Anticipo As Nullable(Of Integer)
        Public Property Saldo As Nullable(Of Integer)
        Public Property Plazo As Nullable(Of Integer)
        Public Property TasaCambio As Nullable(Of Single)
        Public Property NumeroMediciones As Nullable(Of Integer)
        Public Property MesesMediciones As Nullable(Of Integer)
        Public Property TipoPresupuesto As Nullable(Of Byte)
        Public Property NoIQuote As String

    End Class
    Partial Public Class IQ_ProcesosPresupuesto
        Public Property IdPropuesta As Long
        Public Property ParAlternativa As Integer
        Public Property MetCodigo As Integer
        Public Property ProcCodigo As Integer
        Public Property Porcentaje As Nullable(Of Double)
        Public Property ParNacional As Integer

    End Class
    Partial Public Class IQ_Preguntas
        Public Property IdPropuesta As Long
        Public Property ParAlternativa As Integer
        Public Property MetCodigo As Integer
        Public Property PregCerradas As Integer
        Public Property PregCerradasMultiples As Integer
        Public Property PregAbiertas As Integer
        Public Property PregAbiertasMultiples As Integer
        Public Property PregOtras As Integer
        Public Property PregDemograficos As Integer
        Public Property ParNacional As Integer

    End Class
    Partial Public Class IQ_ObtenerHorasProfesionalesXAlternativa_Result
        Public Property PGrCodigo As Integer
        Public Property PGrDescripcion As String
        Public Property PreField As Nullable(Of Integer)
        Public Property FieldWork As Nullable(Of Integer)
        Public Property DPandCoding As Nullable(Of Integer)
        Public Property Analysis As Nullable(Of Integer)
        Public Property PM As Nullable(Of Integer)
        Public Property Meetings As Nullable(Of Integer)
        Public Property Presentation As Nullable(Of Integer)
        Public Property ClientTravel As Nullable(Of Integer)
        Public Property TotalHoras As Nullable(Of Integer)

    End Class
    Partial Public Class IQ_ObtenerActividades_Result
        Public Property ASID As Integer
        Public Property ACTIVIDAD As String
        Public Property VALOR As Decimal
        Public Property TIPO As String
        Public Property ID As Integer

    End Class

    Partial Public Class IQ_COSTOACTIVIDADES_GET_TO_IQUOTE_Result
        Public Property iQuoteName As String
        Public Property TypeIQuote As String
        Public Property OrderIQuote As Integer
        Public Property Valor As Int64

    End Class

    Partial Public Class IQ_AlternativasMarkedToIquote_Result
        Public Property ID As Int64
        Public Property Propuesta As Int32
        Public Property Alternativa As Int32
        Public Property JobBook As String
        Public Property Titulo As String
        Public Property Cliente As String
        Public Property Fecha As DateTime
    End Class

    Partial Public Class IQ_CostosOPS_ToIQuote_Result
        Public Property id As Integer
        Public Property Proceso As String
        Public Property Valor As Int64
        Public Property VentaOPS As Int64
        Public Property CostoOPS As Int64
        Public Property Unidades As Decimal?
    End Class

End Class
