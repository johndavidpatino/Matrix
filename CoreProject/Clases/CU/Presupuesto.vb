
'Imports CoreProject.CU_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Presupuesto
#Region "Variables Globales"
    Private oMatrixContext As CU_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New CU_Entities
    End Sub
#End Region
#Region "Actualizar"
    Public Sub ActualizarParNumJobBookEnIQ(ByVal JobBook As String, ByVal IdPropuesta As Int64, ByVal Alternativa As Int32)
        oMatrixContext.IQ_UpdateParNumJobBook(JobBook, IdPropuesta, Alternativa)
    End Sub
#End Region

#Region "Obtener"
    Function obtenerPresupuestosXGrupoUnidadXGerenteCuentas(ByVal grupoUnidad As Int32?, ByVal idGerenteCuentas As Int64?, ByVal idPropuesta As Int64?) As List(Of CU_Presupuestos)
        Return (From Brief In oMatrixContext.CU_Brief
                Join Propuesta In oMatrixContext.CU_Propuestas
                On Brief.Id Equals Propuesta.Brief
                Join Presupuesto In oMatrixContext.CU_Presupuestos
                On Presupuesto.PropuestaId Equals Propuesta.Id
                Join gerenteCuentas In oMatrixContext.US_UsuariosUnidades2
                On Brief.GerenteCuentas Equals gerenteCuentas.UsuarioId
                Join Unidades In oMatrixContext.US_Unidades2
                On Unidades.id Equals gerenteCuentas.UnidadId
                Join GrupoUnidades In oMatrixContext.US_GrupoUnidad2
                On GrupoUnidades.US_Unidades.id Equals Unidades.GrupoUnidadId
                Where (Not grupoUnidad.HasValue Or (grupoUnidad.HasValue And Unidades.GrupoUnidadId = grupoUnidad.Value)) And (Not idGerenteCuentas.HasValue Or (idGerenteCuentas.HasValue And gerenteCuentas.UsuarioId = idGerenteCuentas)) And (Not idPropuesta.HasValue Or (idPropuesta.HasValue And Propuesta.Id = idPropuesta)) Select Presupuesto).Distinct().ToList
    End Function

    Function obtenerPresupuestosXGrupoUnidadXGerenteCuentasParaRev(ByVal grupoUnidad As Int32?, ByVal idGerenteCuentas As Int64?, ByVal idPropuesta As Int64?) As List(Of CU_Presupuestos)
        Return (From Brief In oMatrixContext.CU_Brief
                Join Propuesta In oMatrixContext.CU_Propuestas
                On Brief.Id Equals Propuesta.Brief
                Join Presupuesto In oMatrixContext.CU_Presupuestos
                On Presupuesto.PropuestaId Equals Propuesta.Id
                Join gerenteCuentas In oMatrixContext.US_UsuariosUnidades2
                On Brief.GerenteCuentas Equals gerenteCuentas.UsuarioId
                Join Unidades In oMatrixContext.US_Unidades2
                On Unidades.id Equals gerenteCuentas.UnidadId
                Join GrupoUnidades In oMatrixContext.US_GrupoUnidad2
                On GrupoUnidades.US_Unidades.id Equals Unidades.GrupoUnidadId
                Where (Not grupoUnidad.HasValue Or (grupoUnidad.HasValue And Unidades.GrupoUnidadId = grupoUnidad.Value)) And (Not idGerenteCuentas.HasValue Or (idGerenteCuentas.HasValue And gerenteCuentas.UsuarioId = idGerenteCuentas)) And (Not idPropuesta.HasValue Or (idPropuesta.HasValue And Propuesta.Id = idPropuesta)) And Presupuesto.ParaRevisar = True Select Presupuesto).Distinct().ToList
    End Function

    Function ObtenerPresupuestosParaRevisar(ByVal GerenteOperacionesId As Int64, Revisado As Boolean, TituloPropuesta As String, IdPropuesta As Int64?, IdTrabajo As Int64?, jobbook As String) As List(Of CU_PresupuestosRevisionPorGerenteOperaciones_Result)
        Return oMatrixContext.CU_PresupuestosRevisionPorGerenteOperaciones(GerenteOperacionesId, Revisado, TituloPropuesta, IdPropuesta, IdTrabajo, jobbook).ToList
    End Function

    Function ObtenerPresupuestosParaEnviarRevisarxIdPropuesta(ByVal idPropuesta As Int64)
        Return oMatrixContext.CU_Presupuestos.Where(Function(x) x.ParaRevisar = False And x.PropuestaId = idPropuesta).ToList
    End Function

    Function obtenerPresupuestosXGrupoUnidadXGerenteCuentasAprobacionDirectores(ByVal grupoUnidad As Int32?, ByVal idGerenteCuentas As Int64?, ByVal nombrePropuesta As String) As List(Of CU_Presupuestos)
        Return (From Brief In oMatrixContext.CU_Brief
                Join Propuesta In oMatrixContext.CU_Propuestas
                On Brief.Id Equals Propuesta.Brief
                Join Presupuesto In oMatrixContext.CU_Presupuestos
                On Presupuesto.PropuestaId Equals Propuesta.Id
                Join gerenteCuentas In oMatrixContext.US_UsuariosUnidades2
                On Brief.GerenteCuentas Equals gerenteCuentas.UsuarioId
                Join Unidades In oMatrixContext.US_Unidades2
                On Unidades.id Equals gerenteCuentas.UnidadId
                Join GrupoUnidades In oMatrixContext.US_GrupoUnidad2
                On GrupoUnidades.US_Unidades.id Equals Unidades.GrupoUnidadId
                Where (Not grupoUnidad.HasValue Or (grupoUnidad.HasValue And Unidades.GrupoUnidadId = grupoUnidad.Value)) And (Not idGerenteCuentas.HasValue Or (idGerenteCuentas.HasValue And gerenteCuentas.UsuarioId = idGerenteCuentas)) And (String.IsNullOrEmpty(nombrePropuesta) Or (Propuesta.Titulo.Contains(nombrePropuesta)) And Presupuesto.Aprobado = True) Select Presupuesto).ToList
    End Function

    Public Function ObtenerPresupuestosAsignadosXEstudio(ByVal EstudioId As Int64) As List(Of CU_Presupuesto_Get_Result)
        Try
            Dim oResult As List(Of CU_Presupuesto_Get_Result)
            oResult = CU_PresupuestoXEstudio_Get(EstudioId, Nothing, Nothing)
            Return oResult
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Public Function DevolverxIdPropuesta(ByVal pPropuestaId As Int64, ByVal pEstudioId As Int64?) As IEnumerable
        Try
            Dim x = (From a In (From y In oMatrixContext.CU_Presupuestos
                    Group Join i In (From z In oMatrixContext.CU_Estudios_Presupuestos Where (Not pEstudioId.HasValue OrElse z.EstudioId = pEstudioId))
                    On y.Id Equals i.PresupuestoId
                    Into PresupuestoAsignados = Group
                    Where y.PropuestaId = pPropuestaId
                    ).ToList()
                    Select a.y.Id, a.y.Nombre, a.y.Alternativa, a.y.PropuestaId, a.y.Valor, a.y.Muestra, a.y.ProductoId, a.y.GrossMargin, a.y.UsadoPropuesta, a.y.Aprobado, a.y.JobBook, a.y.EstadoId, Asignado = If(a.PresupuestoAsignados.Count > 0, True, False)).ToList
            Return x

            Return oMatrixContext.CU_Presupuestos.Where(Function(y) y.PropuestaId = pPropuestaId).ToList()

        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxIdPropuestaAprobados(ByVal pPropuestaId As Int64, ByVal pEstudioId As Int64?) As IEnumerable
        Try
            Dim x = (From a In (From y In oMatrixContext.CU_Presupuestos
                    Group Join i In (From z In oMatrixContext.CU_Estudios_Presupuestos Where (Not pEstudioId.HasValue OrElse z.EstudioId = pEstudioId))
                    On y.Id Equals i.PresupuestoId
                    Into PresupuestoAsignados = Group
                    Where y.PropuestaId = pPropuestaId And y.ParaRevisar = True And y.Aprobado = True
                    ).ToList()
                    Select a.y.Id, a.y.Nombre, a.y.Alternativa, a.y.PropuestaId, a.y.Valor, a.y.Muestra, a.y.ProductoId, a.y.GrossMargin, a.y.UsadoPropuesta, a.y.Aprobado, a.y.JobBook, a.y.EstadoId, Asignado = If(a.PresupuestoAsignados.Count > 0, True, False)).ToList
            Return x

            Return oMatrixContext.CU_Presupuestos.Where(Function(y) y.PropuestaId = pPropuestaId).ToList()

        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverxIdPropuestaSinRevision(ByVal pPropuestaId As Int64, ByVal pEstudioId As Int64?) As IEnumerable
        Try
            Dim x = (From a In (From y In oMatrixContext.CU_Presupuestos
                    Group Join i In (From z In oMatrixContext.CU_Estudios_Presupuestos Where (Not pEstudioId.HasValue OrElse z.EstudioId = pEstudioId))
                    On y.Id Equals i.PresupuestoId
                    Into PresupuestoAsignados = Group
                    Where y.PropuestaId = pPropuestaId
                    ).ToList()
                    Select a.y.Id, a.y.Nombre, a.y.Alternativa, a.y.PropuestaId, a.y.Valor, a.y.Muestra, a.y.ProductoId, a.y.GrossMargin, a.y.UsadoPropuesta, a.y.Aprobado, a.y.JobBook, a.y.EstadoId, Asignado = If(a.PresupuestoAsignados.Count > 0, True, False)).ToList
            Return x

            Return oMatrixContext.CU_Presupuestos.Where(Function(y) y.PropuestaId = pPropuestaId And (y.Aprobado = False Or y.Aprobado Is Nothing) And y.ParaRevisar = False Or y.ParaRevisar Is Nothing).ToList()

        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Public Function DevolverPropuestasParaRevisionPresupuestosGO(ByVal GerenteOperacionesID As Int64, ByVal Revisado As Boolean) As List(Of CU_PropuestasRevisionPorGerenteOperaciones_Result)
        Return oMatrixContext.CU_PropuestasRevisionPorGerenteOperaciones(GerenteOperacionesID, Revisado).ToList
    End Function

    Public Function DevolverPropuestasParaRevisionPresupuestos() As IEnumerable
        Dim x = (From list In oMatrixContext.CU_Propuestas
                 From listp In oMatrixContext.CU_Presupuestos
                 Where list.Id = listp.PropuestaId And listp.ParaRevisar = True And listp.Aprobado = True
                 Select list.Id, list.Titulo).Distinct
        Return x.ToList
    End Function

    Public Function DevolverxIdPropuestaParaRevision(ByVal pPropuestaId As Int64) As List(Of CU_Presupuestos)
        Return oMatrixContext.CU_Presupuestos.Where(Function(x) x.PropuestaId = pPropuestaId).ToList
    End Function

    Public Function obtenerXId(ByVal ID As Int64) As CU_Presupuesto_Get_Result
        Return CU_Presupuesto_Get(ID, Nothing).FirstOrDefault
    End Function

    Public Function ObtenerMuestraParaAnuncioAprobacion(ByVal ID As Int64) As List(Of CU_MuestraAnuncioAprobacion_Result)
        Return oMatrixContext.CU_MuestraAnuncioAprobacion(ID).ToList
    End Function

    Public Function CU_Presupuesto_Get(ByVal ID As Int64?, ByVal propuestaId As Int64?) As List(Of CU_Presupuesto_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_Presupuesto_Get_Result) = oMatrixContext.CU_Presupuesto_Get(ID, propuestaId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function
    Private Function CU_PresupuestoXEstudio_Get(ByVal EstudioId As Int64?, ByVal presupuestoId As Int64?, ByVal propuestaId As Int64?) As List(Of CU_Presupuesto_Get_Result)
        Try
            Dim oResult As ObjectResult(Of CU_Presupuesto_Get_Result) = oMatrixContext.CU_Estudios_Presupuestos_Asignados_Get(EstudioId, presupuestoId, propuestaId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Function obtenerMetodologiaTecnicaXAlternativasPropuesta(ByVal idPropuesta As Int64, ByVal alternativas As List(Of Integer)) As List(Of CU_Propuesta_Presupuestos_TecnicaMetodologia_Result)
        Return oMatrixContext.CU_Propuesta_Presupuestos_TecnicaMetodologia(idPropuesta, String.Join(",", alternativas)).ToList
    End Function

    Function Cu_PresupestoGetByPropAndAlt(ByVal idpropuesta As Int64, alternativa As Int32) As CU_Presupuestos
        If oMatrixContext.CU_Presupuestos.Where(Function(x) x.PropuestaId = idpropuesta And x.Alternativa = alternativa).ToList.Count > 0 Then
            Return oMatrixContext.CU_Presupuestos.First(Function(x) x.PropuestaId = idpropuesta And x.Alternativa = alternativa)
        Else
            Return New CU_Presupuestos
        End If
    End Function
#End Region
#Region "Grabar"
    Sub editarJobBook(ByVal id As Int64, ByVal jobBook As String)
        oMatrixContext.CU_Presupuestos_JobBook_Edit(id, jobBook)
    End Sub
    Sub editarEnvio(ByVal id As Int64, ByVal envio As Boolean)
        Dim Ent = oMatrixContext.CU_Presupuestos.Where(Function(x) x.Id = id).FirstOrDefault
        Ent.ParaRevisar = True
        oMatrixContext.SaveChanges()
    End Sub
#End Region
End Class
