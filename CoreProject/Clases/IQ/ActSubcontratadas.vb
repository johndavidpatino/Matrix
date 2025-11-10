'Imports CoreProject.IQ_ENTITIES

Namespace IQ


    Public Class ActSubcontratadas
        Dim _IQ_Entities As IQ_MODEL


        Public Function ObtenerActividadesXMetodologia(ByVal Metodologia As Integer) As List(Of IQ_Actividades)

            Try
                _IQ_Entities = New IQ_MODEL()
                Dim lstAct As List(Of IQ_Actividades)
                lstAct = (From a In _IQ_Entities.IQ_Actividades Where a.ActSubcontratada = True And a.ActTipoCalculo = "M" Select a).ToList()
                lstAct.Insert(0, New IQ_Actividades With {.ID = "0", .ActNombre = "Seleccione"})
                Return (lstAct)
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Sub InsertarCostoActividades(ByVal Lstact As List(Of IQ_CostoActividades))

            Try
                _IQ_Entities = New IQ_MODEL()

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



            Catch ex As Exception
                Throw ex
            End Try
        End Sub


        Public Function ObtenerMetodologiaSegunTecnica(ByVal par As IQ_Parametros) As Integer
            Try
                _IQ_Entities = New IQ_MODEL()
                Dim Metodologia As Integer

                Metodologia = (From m In _IQ_Entities.IQ_Parametros Where m.IdPropuesta = par.IdPropuesta And m.ParAlternativa = par.ParAlternativa And m.TecCodigo = par.TecCodigo Select m.MetCodigo).First()


                Return Metodologia

            Catch ex As Exception
                Throw ex
            End Try

        End Function


        Public Function ObtenerActividadesSubcontratadas(ByVal par As IQ_Parametros) As List(Of IQ_ObtenerActividades_Result)
            Try
                Dim lst As List(Of IQ_ObtenerActividades_Result)
                _IQ_Entities = New IQ_MODEL()
                lst = _IQ_Entities.IQ_ObtenerActividades(par.IdPropuesta, par.ParAlternativa, par.ParNacional, par.MetCodigo).ToList()
                Return lst
            Catch ex As Exception
                Throw ex
            End Try


        End Function



        Public Function TotalizarActSub(ByVal par As IQ_Parametros) As Decimal

            _IQ_Entities = New IQ_MODEL
            Dim total As Decimal
            Dim q = (From a In _IQ_Entities.IQ_CostoActividades Join c In _IQ_Entities.IQ_Actividades On a.ActCodigo Equals c.ID Where a.IdPropuesta = par.IdPropuesta And a.ParAlternativa = par.ParAlternativa And a.ParNacional = par.ParNacional And a.MetCodigo = par.MetCodigo And c.ActSubcontratada = True Select a.CaCosto).ToArray()
            total = If(q.Sum() = Nothing, 0, q.Sum)
            Return total
        End Function

    End Class
End Namespace

