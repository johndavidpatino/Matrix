
'Imports CoreProject.PY_Model
Imports System.Data.Entity.Core.Objects

<Serializable()>
Public Class Proyecto
#Region "Variables Globales"
    Private oMatrixContext As PY_Entities
#End Region
#Region "Constructores"
    'Constructor public 
    Public Sub New()
        oMatrixContext = New PY_Entities
    End Sub
#End Region
#Region "Obtener"

    Function obtenerProyectosSinJBI(ByVal Unidad As Int64?) As List(Of PY_ReporteProyectosSinJBI_Result)
        Return oMatrixContext.PY_ReporteProyectosSinJBI(Unidad).ToList
    End Function

    Function obtenerUnidadesProyectosSinJBI() As List(Of PY_Unidades_ProyectosSinJBI_Get_Result)
        Return oMatrixContext.PY_Unidades_ProyectosSinJBI_Get().ToList
    End Function

    Function obtenerTodos() As List(Of PY_Proyectos_Get_Result)
        Return PY_Proyecto_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing)
    End Function

    Function obtenerXId(ByVal id As Long) As PY_Proyectos_Get_Result
        Return PY_Proyecto_Get(id, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
    End Function

    Function obtenerTodosCampos(ByVal todosCampos As String) As List(Of PY_Proyectos_Get_Result)
        Return oMatrixContext.PY_Proyectos_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, todosCampos, Nothing).ToList
    End Function

    Function obtenerXGerenteProyectos(ByVal gerenteProyectos As Int64) As List(Of PY_Proyectos_Get_Result)
        Return PY_Proyecto_Get(Nothing, Nothing, Nothing, Nothing, gerenteProyectos, Nothing, Nothing, Nothing, Nothing)
    End Function

    Function obtenerXGerenteCuentas(ByVal gerenteCuentas As Int64) As List(Of PY_Proyectos_Get_Result)
        Return PY_Proyecto_Get(Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, gerenteCuentas)
    End Function

    Function obtener(ByVal gerenteProyectos As Int64, ByVal todosCampos As String) As List(Of PY_Proyectos_Get_Result)
        Return PY_Proyecto_Get(Nothing, Nothing, Nothing, Nothing, gerenteProyectos, Nothing, Nothing, todosCampos, Nothing)
    End Function

    Function obtenerXIdXGerenteProyecto(ByVal proyectoId As Int64, ByVal gerenteProyecto As Integer) As List(Of PY_Proyectos_Get_Result)
        Return PY_Proyecto_Get(proyectoId, Nothing, Nothing, Nothing, gerenteProyecto, Nothing, Nothing, Nothing, Nothing)
    End Function

    Function obtenerXAsignarGerenteProyecto(ByVal Unidad As Int64) As List(Of PY_Proyectos_Get_Result)
        Return PY_Proyecto_Get_XAsignar(Unidad)
    End Function

    Function obtenerXReAsignarGerenteProyecto(ByVal Unidad As Int64, ByVal Nombre As String) As List(Of PY_Proyectos_Get_Result)
        Return PY_Proyecto_Get_XReAsignar(Unidad, Nombre)
    End Function

    Function obtenerXJobBook(ByVal JobBook As String) As PY_Proyectos_Get_Result
        Return PY_Proyecto_Get(Nothing, JobBook, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing).FirstOrDefault
    End Function
    Private Function PY_Proyecto_Get(ByVal ID As Int64?, ByVal JobBook As String, ByVal Nombre As String, ByVal UnidadId As Int32?, ByVal GerenteProyectos As Int64?, ByVal EstudioId As Int64?, ByVal TipoProyectoId As Int16?, ByVal todosCampos As String, ByVal gerenteCuentas As Int64?) As List(Of PY_Proyectos_Get_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Proyectos_Get_Result) = oMatrixContext.PY_Proyectos_Get(ID, JobBook, Nombre, UnidadId, GerenteProyectos, EstudioId, TipoProyectoId, todosCampos, gerenteCuentas)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Private Function PY_Proyecto_Get_XAsignar(ByVal UnidadId As Int32) As List(Of PY_Proyectos_Get_Result)
        Try
            Dim oResult As ObjectResult(Of PY_Proyectos_Get_Result) = oMatrixContext.PY_Proyectos_Get_XAsignar(UnidadId)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

	Private Function PY_Proyecto_Get_XReAsignar(ByVal UnidadId As Int32, ByVal Nombre As String) As List(Of PY_Proyectos_Get_Result)
		Try
			Dim oResult As ObjectResult(Of PY_Proyectos_Get_Result) = oMatrixContext.PY_Proyectos_Get_XREAsignar(UnidadId, Nombre)
			Return oResult.ToList()
		Catch ex As Exception
			If (ex.InnerException Is Nothing) Then
				Throw ex
			Else
				Throw ex.InnerException
			End If
		End Try
	End Function

    Function ObtenerXEstudio(ByVal idEstudio As Int64) As PY_Proyectos
        Return oMatrixContext.PY_Proyectos.First(Function(x) x.EstudioId = idEstudio)
    End Function

    Function ObtenerListadoXEstudio(ByVal idEstudio As Int64) As List(Of PY_Proyectos_Get_Result)
        Return oMatrixContext.PY_Proyectos_Get(Nothing, Nothing, Nothing, Nothing, Nothing, idEstudio, Nothing, Nothing, Nothing).ToList
    End Function
    Function ObtenerProyecto(ByVal id As Int64) As PY_Proyectos
        Return oMatrixContext.PY_Proyectos.Where(Function(x) x.id = id).FirstOrDefault
    End Function

    Function ObtenerProyectoXJob(ByVal id As String) As PY_Proyectos
        Return oMatrixContext.PY_Proyectos.Where(Function(x) x.JobBook = id).FirstOrDefault
    End Function

    Function ObtenerVariableControlxTrabajo(ByVal id As Int64) As List(Of PY_Variables_Control)
        Return oMatrixContext.PY_Variables_Control.Where(Function(x) x.idTrabajo = id).ToList
    End Function

    Function ObtenerVariableControlxTrabajoxMod(ByVal id As Int64, ByVal tipo As String) As List(Of PY_Variables_Control)
        Return oMatrixContext.PY_Variables_Control.Where(Function(x) x.idTrabajo = id And x.tipoEvaluado = tipo).ToList
    End Function

    Function ObtenerEspecifaciones(ByVal IDTrabajo As Int64) As PY_EspecifTecTrabajo
        If oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.TrabajoId = IDTrabajo).ToList.Count = 0 Then
            Return New PY_EspecifTecTrabajo
        Else
            Return oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.TrabajoId = IDTrabajo).First
        End If
    End Function

    Function ObtenerEspecifacionesLast(ByVal IDTrabajo As Int64) As PY_EspecifTecTrabajo
        If oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.TrabajoId = IDTrabajo).ToList.Count = 0 Then
            Return New PY_EspecifTecTrabajo
        Else
            Return oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.TrabajoId = IDTrabajo).ToList().Last
        End If
    End Function

    Function ObtenerEspecifacionesxVersion(ByVal IDTrabajo As Int64, ByVal version As Int64) As PY_EspecifTecTrabajo
        If oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.TrabajoId = IDTrabajo And x.NoVersion = version).ToList.Count = 0 Then
            Return New PY_EspecifTecTrabajo
        Else
            Return oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.TrabajoId = IDTrabajo And x.NoVersion = version).ToList().Last
        End If
    End Function

    Function ObtenerEspecifacionesList(ByVal IDTrabajo As Int64) As List(Of PY_EspecifTecTrabajo)
        If oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.TrabajoId = IDTrabajo).ToList.Count = 0 Then
            Return New List(Of PY_EspecifTecTrabajo)
        Else
            Return oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.TrabajoId = IDTrabajo).ToList()
        End If
    End Function

    Function ObtenerEspecifacionesContar(ByVal IDTrabajo As Int64) As Integer
        Return oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.TrabajoId = IDTrabajo).ToList.Count
    End Function

    Function ObtenerEspecifacionesId(ByVal id As Int64) As List(Of PY_EspecifTecTrabajo)
        Return oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.id = id).ToList()
    End Function

    Function ObtenerVerEspecifTecTr(ByVal IDTrabajo As Int64) As List(Of PY_ObtenerVerEspecifTecTr_Result)
        Try
            Dim oResult As ObjectResult(Of PY_ObtenerVerEspecifTecTr_Result) = oMatrixContext.PY_ObtenerVerEspecifTecTr(IDTrabajo)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Function ObtenerVerEspecifCuentaCuali(ByVal IdProyecto As Int64) As List(Of PY_EspCuentasCuali)
        Try
            If oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IdProyecto).ToList.Count = 0 Then
                Return New List(Of PY_EspCuentasCuali)
            Else
                Return oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IdProyecto).OrderByDescending(Function(x) x.NoVersion).ToList
            End If
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Function ObtenerEspecifCuentaCualixIdxPr(ByVal IDProyecto As Int64, ByVal Id As Int64) As List(Of PY_EspCuentasCuali)
        Try
            If oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto And x.id = Id).ToList.Count = 0 Then
                Return New List(Of PY_EspCuentasCuali)
            Else
                Return oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto And x.id = Id).OrderByDescending(Function(x) x.id).ToList
            End If
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Function ObtenerEspecifCuentaCualixId(ByVal Id As Int64) As PY_EspCuentasCuali
        Try
            If oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.id = Id).ToList.Count = 0 Then
                Return New PY_EspCuentasCuali
            Else
                Return oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.id = Id).FirstOrDefault
            End If
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Function ObtenerEspecifCuentaCualiContar(ByVal IDProyecto As Int64) As Integer
        Return oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto).ToList.Count
    End Function

    Function ObtenerEspecifCuentaCualiList(ByVal IDProyecto As Int64) As List(Of PY_EspCuentasCuali)
        If oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto).ToList.Count = 0 Then
            Return New List(Of PY_EspCuentasCuali)
        Else
            Return oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto).ToList
        End If
    End Function

    Function ObtenerEspecifXIdxTr(ByVal IDTrabajo As Int64, ByVal Id As Int64) As List(Of PY_ObtenerEspecifXIdxTr_Result)
        Try
            Dim oResult As ObjectResult(Of PY_ObtenerEspecifXIdxTr_Result) = oMatrixContext.PY_ObtenerEspecifXIdxTr(IDTrabajo, Id)
            Return oResult.ToList()
        Catch ex As Exception
            If (ex.InnerException Is Nothing) Then
                Throw ex
            Else
                Throw ex.InnerException
            End If
        End Try
    End Function

    Function obtenerEspCuentasCuali(ByVal IDProyecto As Int64) As PY_EspCuentasCuali
        If oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto).ToList.Count = 0 Then
            Return New PY_EspCuentasCuali
        Else
            Return oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto).ToList.Last
        End If
    End Function

    Function obtenerEspCuentasCuanti(ByVal IDProyecto As Int64) As PY_EspCuentasCuanti
        If oMatrixContext.PY_EspCuentasCuanti.Where(Function(x) x.ProyectoId = IDProyecto).ToList.Count = 0 Then
            Return New PY_EspCuentasCuanti
        Else
            Return oMatrixContext.PY_EspCuentasCuanti.Where(Function(x) x.ProyectoId = IDProyecto).ToList.Last
        End If
    End Function

    Function obtenerEspCuentasCualixVersion(ByVal IDProyecto As Int64, ByVal Version As Int64) As PY_EspCuentasCuali
        If oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto And x.NoVersion = Version).ToList.Count = 0 Then
            Return New PY_EspCuentasCuali
        Else
            Return oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto And x.NoVersion = Version).ToList.Last
        End If
    End Function
    Function obtenerEspCuentasCualixID(ByVal IDProyecto As Int64, ByVal id As Int64) As PY_EspCuentasCuali
        If oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto And x.id = id).ToList.Count = 0 Then
            Return New PY_EspCuentasCuali
        Else
            Return oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto And x.id = id).ToList.Last
        End If
    End Function

    Function obtenerEspCuentasCuantixVersion(ByVal IDProyecto As Int64, ByVal Version As Int64) As PY_EspCuentasCuanti
        If oMatrixContext.PY_EspCuentasCuanti.Where(Function(x) x.ProyectoId = IDProyecto And x.NoVersion = Version).ToList.Count = 0 Then
            Return New PY_EspCuentasCuanti
        Else
            Return oMatrixContext.PY_EspCuentasCuanti.Where(Function(x) x.ProyectoId = IDProyecto And x.NoVersion = Version).ToList.Last
        End If
    End Function

    Function obtenerEspCuentasCuantixID(ByVal IDProyecto As Int64, ByVal id As Int64) As PY_EspCuentasCuanti
        If oMatrixContext.PY_EspCuentasCuanti.Where(Function(x) x.ProyectoId = IDProyecto And x.id = id).ToList.Count = 0 Then
            Return New PY_EspCuentasCuanti
        Else
            Return oMatrixContext.PY_EspCuentasCuanti.Where(Function(x) x.ProyectoId = IDProyecto And x.id = id).ToList.Last
        End If
    End Function

    Function obtenerEspCuentasCualiList(ByVal IDProyecto As Int64, ByVal orderDesc As Long?) As List(Of PY_EspCuentasCuali)
        If oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto).ToList.Count = 0 Then
            Return New List(Of PY_EspCuentasCuali)
        Else
            If orderDesc = 1 Then
                Return oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto).OrderByDescending(Function(x) x.NoVersion).ToList()
            Else
                Return oMatrixContext.PY_EspCuentasCuali.Where(Function(x) x.ProyectoId = IDProyecto).ToList()
            End If
        End If
    End Function

    Function obtenerEspCuentasCuantiList(ByVal IDProyecto As Int64, ByVal orderDesc As Long?) As List(Of PY_EspCuentasCuanti)
        If oMatrixContext.PY_EspCuentasCuanti.Where(Function(x) x.ProyectoId = IDProyecto).ToList.Count = 0 Then
            Return New List(Of PY_EspCuentasCuanti)
        Else
            If orderDesc = 1 Then
                Return oMatrixContext.PY_EspCuentasCuanti.Where(Function(x) x.ProyectoId = IDProyecto).OrderByDescending(Function(x) x.NoVersion).ToList()
            Else
                Return oMatrixContext.PY_EspCuentasCuanti.Where(Function(x) x.ProyectoId = IDProyecto).ToList()
            End If
        End If
    End Function


#End Region
#Region "Grabar"
    Function Grabar(ByVal jobBook As String, ByVal nombre As String, ByVal unidadId As Integer, ByVal gerenteProyectos As Long?, ByVal estudioId As Long, ByVal tipoProyectoId As Integer, ByVal id As Long?, A1 As String, A2 As String, A3 As String, A4 As String, A5 As String, A6 As String, A7 As String) As Decimal
		If id.HasValue Then
			oMatrixContext.PY_Proyectos_Edit(jobBook, nombre, unidadId, gerenteProyectos, estudioId, tipoProyectoId, id, A1, A2, A3, A4, A5, A6, A7)
			Return id
		Else
			Return oMatrixContext.PY_Proyecto_Add(jobBook, nombre, unidadId, gerenteProyectos, estudioId, tipoProyectoId, A1, A2, A3, A4, A5, A6, A7).FirstOrDefault
		End If
	End Function

	Sub ActualizarGerente(ByVal gerentepy As Long, id As Long)
        oMatrixContext.PY_Proyectos_EditGerentePY(id, gerentepy)
    End Sub


    Public Sub AsignarJBE_ProyectosCuali(ByVal IdProyecto As Int64)
		oMatrixContext.IQ_AsignacionJBE_ProyectosCuali(IdProyecto)
	End Sub

    Sub GuardarProyecto(ByRef ent As PY_Proyectos)
        If ent.id = 0 Then
            oMatrixContext.PY_Proyectos.Add(ent)
        End If
        oMatrixContext.SaveChanges()
    End Sub

    Sub GuardarInfoEspecificaciones(ByRef ent As PY_EspecifTecTrabajo)
        Dim idTrabajo As Int64 = ent.TrabajoId
        'If oMatrixContext.PY_EspecifTecTrabajo.Where(Function(x) x.TrabajoId = idTrabajo).ToList.Count = 0 Then
        oMatrixContext.PY_EspecifTecTrabajo.Add(ent)
        'End If
        oMatrixContext.SaveChanges()
    End Sub

    Sub GuardarEspecificacionesCuentasCualitativa(ByRef esp As PY_EspCuentasCuali)
        oMatrixContext.PY_EspCuentasCuali.Add(esp)
        oMatrixContext.SaveChanges()
    End Sub
    Sub GuardarEspecificacionesCuentasCuantitativo(ByRef esp As PY_EspCuentasCuanti)
        oMatrixContext.PY_EspCuentasCuanti.Add(esp)
        oMatrixContext.SaveChanges()
    End Sub

    Sub guardarVariableControl(ByRef variable As PY_Variables_Control)
        oMatrixContext.PY_Variables_Control.Add(variable)
        oMatrixContext.SaveChanges()
    End Sub

#End Region
End Class
