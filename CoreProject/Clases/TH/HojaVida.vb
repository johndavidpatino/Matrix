Imports System.Data.Entity.Core.Objects

'Imports CoreProject.TH_Model
'Imports CoreProject
'Imports CoreProject.OP_Cuanti_Model
Imports System.Data.SqlClient

Namespace TH
    Public Class HojaVida

        Private _THcontext As TH_Entities
        Private _DVcontext As C_Divipola

        Public Property THContext As TH_Entities
            Get
                If Me._THcontext Is Nothing Then
                    Me._THcontext = New TH_Entities()
                End If
                Return Me._THcontext
            End Get
            Set(value As TH_Entities)
                Me._THcontext = value
            End Set
        End Property

        Public Property DVContext As C_Divipola
            Get
                If Me._DVcontext Is Nothing Then
                    Me._DVcontext = New C_Divipola
                End If
                Return Me._DVcontext
            End Get
            Set(value As C_Divipola)
                Me._DVcontext = value
            End Set
        End Property

        Public Function GuardarHojaVida(TipoIdentificacionId As Integer, NumeroIdentificacion As String, FechaNacimiento As Date, Nombres As String, Apellidos As String, Correo As String, SexoId As Integer, EstadoCivilId As Integer, PaisNacimientoId As Integer, PaisResidenciaId As Integer, CiudadResidenciaId As Integer, Direccion As String, Telefono As String, Celular As String, TelefonoOficina As String, Extension As Integer, Profesion As String, Foto As String, PerfilProfesional As String, TiempoExperienciaId As Integer, AspiracionSalarialId As Integer, Trabaja As Boolean, PosibilidadViajar As Boolean, IdiomaId As Integer, Texto As String, Texto_ingles As String, CargoActual As Integer) As Integer

            Try
                Try
                    Dim Res As ObjectResult(Of TH_HojaVida_Add_Result)
                    Res = THContext.TH_HojaVida_Add(TipoIdentificacionId, NumeroIdentificacion, FechaNacimiento, Nombres, Apellidos, Correo, SexoId, EstadoCivilId, PaisNacimientoId, PaisResidenciaId, CiudadResidenciaId, Direccion, Telefono, Celular, TelefonoOficina, Extension, Profesion, Foto, PerfilProfesional, TiempoExperienciaId, AspiracionSalarialId, Trabaja, PosibilidadViajar, IdiomaId, Texto, Texto_ingles, CargoActual)
                    Return Res(0).Id
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function EditarHojaVida(Id As Integer, TipoIdentificacionId As Integer, NumeroIdentificacion As String, FechaNacimiento As Date, Nombres As String, Apellidos As String, Correo As String, SexoId As Integer, EstadoCivilId As Integer, PaisNacimientoId As Integer, PaisResidenciaId As Integer, CiudadResidenciaId As Integer, Direccion As String, Telefono As String, Celular As String, TelefonoOficina As String, Extension As Integer, Profesion As String, Foto As String, PerfilProfesional As String, TiempoExperienciaId As Integer, AspiracionSalarialId As Integer, Trabaja As Boolean, PosibilidadViajar As Boolean, IdiomaId As Integer, Texto As String, Texto_ingles As String, cargoActual As Integer) As Integer
            Try
                Try
                    Return THContext.TH_HojaVida_Edit(Id, TipoIdentificacionId, NumeroIdentificacion, FechaNacimiento, Nombres, Apellidos, Correo, SexoId, EstadoCivilId, PaisNacimientoId, PaisResidenciaId, CiudadResidenciaId, Direccion, Telefono, Celular, TelefonoOficina, Extension, Profesion, Foto, PerfilProfesional, TiempoExperienciaId, AspiracionSalarialId, Trabaja, PosibilidadViajar, IdiomaId, Texto, Texto_ingles, cargoActual)(0).Id
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function EliminarHojaVida(ByVal id As Integer) As Integer
            Try
                Try
                    Return THContext.TH_HojaVida_Del(id)
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerHojaVida() As List(Of TH_HojaVida_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of TH_HojaVida_Get_Result) = THContext.TH_HojaVida_Get
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ObtenerHojaVidaIdentificacion(ByVal Identificacion As String) As List(Of TH_HojaVida_Get_Result)
            Try
                Try
                    Dim List As ObjectResult(Of TH_HojaVida_Get_Result) = THContext.TH_HojaVida_GetIdentificacion(Identificacion)
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function CombosHojaVida(ByVal Tabla As String, Optional IdPadre As Integer = 0) As List(Of Combo)
            Dim List As ObjectResult(Of Combo)
            Try
                Try
                    Select Case Tabla
                        Case "AspiracionSalarial"
                            List = THContext.TH_AspiracionSalarial_Get
                        Case "EstadoCivil"
                            List = THContext.TH_EstadoCivil_Get
                        Case "Sexo"
                            List = THContext.TH_Sexo_Get
                        Case "TiempoExperiencia"
                            List = THContext.TH_TiempoExperiencia_Get
                        Case "TipoIdentificacion"
							'List = THContext.TH_TipoIdentificacion_Get
						Case "Ciudad"
                            List = THContext.f_Ciudades_Get(IdPadre)
                        Case "Idiomas"
                            List = THContext.f_Idiomas_Get()
                        Case "DominioIdiomas"
                            List = THContext.TH_DominioIdioma_Get()
                        Case "Cargo"
                            List = THContext.TH_Cargo_Get()
                        Case "NivelCargo"
                            List = THContext.TH_NivelCargo_Get()
                        Case "NivelEstudio"
                            List = THContext.TH_NivelEstudio_Get()
                        Case "EstadoEducacion"
							'List = THContext.TH_EstadoEducacion_Get()
						Case Else
                            List = Nothing
                    End Select
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function ComboPais() As List(Of Combo32)
            Dim List As ObjectResult(Of Combo32)
            Try
                Try
                    List = THContext.f_Paises_Get
                    Return List.ToList()
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function Profesiones() As List(Of String)
            Dim arr As New List(Of String)
            Try
                Try
                    Dim List As ObjectResult(Of TH_Profesiones_Get_Result) = THContext.TH_Profesiones_Get
                    For Each dr As TH_Profesiones_Get_Result In List
                        arr.Add(dr.Nombre)
                    Next
                    Return arr
                Catch ex As SqlException
                    Throw ex
                End Try
            Catch ex As Exception
                If IsNothing(ex.InnerException) Then
                    Throw ex
                Else
                    Throw ex.InnerException
                End If
            End Try
        End Function

        Public Function HojaVidaGetxId(ByVal id As Int64) As TH_HojaVida
            Return THContext.TH_HojaVida.Where(Function(x) x.Id = id).FirstOrDefault
        End Function

        Public Sub HojaVidaSave(ByVal HVEnt As TH_HojaVida)
            If THContext.TH_HojaVida.Where(Function(x) x.Id = HVEnt.Id).ToList.Count = 0 Then
                THContext.TH_HojaVida.Add(HVEnt)
                THContext.SaveChanges()
            Else
                Dim Ent As New TH_HojaVida
                Ent = HojaVidaGetxId(HVEnt.Id)
                Ent.Apellidos = HVEnt.Apellidos
                Ent.AspiracionSalarialId = HVEnt.AspiracionSalarialId
                Ent.CargoActualId = HVEnt.CargoActualId
                Ent.Celular = HVEnt.Celular
                Ent.CiudadResidenciaId = HVEnt.CiudadResidenciaId
                Ent.Correo = HVEnt.Correo
                Ent.Direccion = HVEnt.Direccion
                Ent.EstadoCivilId = HVEnt.EstadoCivilId
                Ent.Extension = HVEnt.Extension
                Ent.FechaNacimiento = HVEnt.FechaNacimiento
                Ent.Foto = HVEnt.Foto
                Ent.IdiomaId = HVEnt.IdiomaId
                Ent.Nombres = HVEnt.Nombres
                Ent.NumeroIdentificacion = HVEnt.NumeroIdentificacion
                Ent.PaisNacimientoId = HVEnt.PaisNacimientoId
                Ent.PaisResidenciaId = HVEnt.PaisResidenciaId
                Ent.PerfilProfesional = HVEnt.PerfilProfesional
                Ent.PosibilidadViajar = HVEnt.PosibilidadViajar
                Ent.Profesion = HVEnt.Profesion
                Ent.SexoId = HVEnt.SexoId
                Ent.Telefono = HVEnt.Telefono
                Ent.TelefonoOficina = HVEnt.TelefonoOficina
                Ent.Texto = HVEnt.Texto
                Ent.Texto_ingles = HVEnt.Texto_ingles
                Ent.TiempoExperienciaId = HVEnt.TiempoExperienciaId
                Ent.TipoIdentificacionId = HVEnt.TipoIdentificacionId
                Ent.Trabaja = HVEnt.Trabaja

                THContext.SaveChanges()
            End If
        End Sub

        Public Function GetDpto(ByVal Ciudad As Int32) As Divipola_Get_Dpto_Result
            Return THContext.Divipola_Get_Dpto(Ciudad).First
        End Function
    End Class
End Namespace