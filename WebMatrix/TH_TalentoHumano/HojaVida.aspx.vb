Imports CoreProject
Imports System.Globalization
Imports System.IO
Imports WebMatrix.Util


Public Class HojaVida
    Inherits System.Web.UI.Page
    Private _codigo As Integer
    Private _foto As String
    Private _identificacion As String
    Private _idioma_id As String
    Private _experiencia_id As String
    Private _educacion_id As String

    Public Property Foto As String
        Get
            Return ViewState("_foto")
        End Get
        Set(value As String)
            ViewState("_foto") = value
        End Set
    End Property

    Public Property Codigo As Integer
        Get
            Return ViewState("_codigo")
        End Get
        Set(value As Integer)
            ViewState("_codigo") = value
        End Set
    End Property

    Public Property IdiomaId As Integer
        Get
            Return ViewState("_idioma_id")
        End Get
        Set(value As Integer)
            ViewState("_idioma_id") = value
        End Set
    End Property

    Public Property ExperienciaId As Integer
        Get
            Return ViewState("_experiencia_id")
        End Get
        Set(value As Integer)
            ViewState("_experiencia_id") = value
        End Set
    End Property

    Public Property EducacionId As Integer
        Get
            Return ViewState("_educacion_id")
        End Get
        Set(value As Integer)
            ViewState("_educacion_id") = value
        End Set
    End Property

    Public Property Identificacion As Integer
        Get
            Return ViewState("_identificacion")
        End Get
        Set(value As Integer)
            ViewState("_identificacion") = value
        End Set
    End Property

    Dim TH As New TH.HojaVida

    Public Function ObtenerEstadoActas(ByVal Identificacion As String) As List(Of TH_HojaVida_Get_Result)
        Try
            Return TH.ObtenerHojaVidaIdentificacion(Identificacion)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerCombo(ByVal Opcion As String, Optional IdPadre As Integer = 0) As List(Of Combo)
        Dim Data As New TH.HojaVida
        Try
            Return Data.CombosHojaVida(Opcion, IdPadre)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function ObtenerComboPais() As List(Of Combo32)
        Dim Data As New TH.HojaVida
        Try
            Return Data.ComboPais
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function CargarExperienciaLaboral(ByVal HVId As Integer) As List(Of TH_ExperienciaLaboral_Get_Result)
        Dim Data As New TH.ExperienciaLaboral
        Dim list As List(Of TH_ExperienciaLaboral_Get_Result)
        list = Data.ObtenerExperienciaLaboralHVID(HVId)
        Return list
    End Function

    Public Function CargarIdiomas(ByVal HVId As Integer) As List(Of TH_Idiomas_Get_Result)
        Dim Data As New TH.Idiomas
        Dim list As List(Of TH_Idiomas_Get_Result)
        list = Data.ObtenerIdiomasHVID(HVId)
        Return list
    End Function

    Public Function CargarEducacion(ByVal HVId As Integer) As List(Of TH_Educacion_Get_Result)
        Dim Data As New TH.Educacion
        Dim list As List(Of TH_Educacion_Get_Result)
		'list = Data.ObtenerEducacionHVID(HVId)
		Return list
    End Function

    Sub CargarCombo(ByVal cmb As DropDownList, ByVal dvf As String, ByVal dtf As String, ByVal ds As Object)
        cmb.DataValueField = dvf
        cmb.DataTextField = dtf
        cmb.DataSource = ds
        cmb.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.Form("__eventtarget") = "fuFotoPersona" Then
            If fuFotoPersona.HasFile Then
                UploadFile()
            End If
        End If
        If Not Page.IsPostBack Then
            CargarDepartamentos()
            CargarCombo(ddlEstadoCivil, "Id", "Nombre", ObtenerCombo("EstadoCivil"))
            CargarCombo(ddlGenero, "Id", "Nombre", ObtenerCombo("Sexo"))
            CargarCombo(ddTipoDocumento, "Id", "Nombre", ObtenerCombo("TipoIdentificacion"))
            CargarCombo(ddlAnoExperiencia, "Id", "Nombre", ObtenerCombo("TiempoExperiencia"))
            CargarCombo(ddlIdioma, "Id", "Nombre", ObtenerCombo("Idiomas"))
            CargarCombo(ddlPaisNacimiento, "Id", "Nombre", ObtenerComboPais)
            CargarCombo(ddlPaisResidencia, "Id", "Nombre", ObtenerComboPais)
            'CargarCombo(ddlCiudadResidencia, "Id", "Nombre", ObtenerCombo("Ciudad", ddlPaisNacimiento.SelectedValue))
            ddlIdioma.SelectedValue = 48
            'CARGA DE HIJOS
            CargarCombo(ddlPaisEd, "Id", "Nombre", ObtenerComboPais)
            'CargarCombo(ddlCiudadEd, "Id", "Nombre", ObtenerCombo("Ciudad", ddlPaisEd.SelectedValue))
            CargarCombo(ddlPaisExp, "Id", "Nombre", ObtenerComboPais)
            'CargarCombo(ddlCiudadExp, "Id", "Nombre", ObtenerCombo("Ciudad", ddlPaisExp.SelectedValue))
            CargarCombo(ddlIdiomaIdm, "Id", "Nombre", ObtenerCombo("Idiomas"))
            CargarCombo(ddlDominioIdm, "Id", "Nombre", ObtenerCombo("DominioIdiomas"))
            CargarCombo(ddlNivelEstudioEd, "Id", "Nombre", ObtenerCombo("NivelEstudio"))
            CargarCombo(ddlEstadoEd, "Id", "Nombre", ObtenerCombo("EstadoEducacion"))
            CargarCombo(ddlCargoExp, "Id", "Nombre", ObtenerCombo("Cargo"))
            'CargarCombo(ddlCargoActual, "Id", "Nombre", ObtenerCombo("Cargo"))
            CargarCombo(ddlNivelCargoExp, "Id", "Nombre", ObtenerCombo("NivelCargo"))
            DvEducacion.Visible = False
            dvExperiencia.Visible = False
            DvIdioma.Visible = False
            If Request.QueryString("HojaVidaId") IsNot Nothing Then
                'If Session("IDUsuario") IsNot Nothing Then
                Me.Codigo = Request.QueryString("HojaVidaId").ToString
                imgPersona.ImageUrl = "../Images/Fotos/" & Request.QueryString("HojaVidaId") & ".jpg"
                Me.txtNoDocumento.Text = Me.Codigo
                'Me.Identificacion = Request.QueryString("HojaVidaId")
                Me.Identificacion = Request.QueryString("HojaVidaId").ToString
                btnAddEducacion.Enabled = False
                btnAddExperiencia.Enabled = False
                btnAddIdioma.Enabled = False
                btnSave1.Enabled = False
                btnSave2.Enabled = False
                btnSave3.Enabled = False
                btnSave4.Enabled = False
                btnSave5.Enabled = False
                btnSave6.Enabled = False
                btnSave7.Enabled = False
                Me.gvEducacion.Columns(5).Visible = False
                Me.gvEducacion.Columns(6).Visible = False
                Me.gvIdiomas.Columns(3).Visible = False
                Me.gvIdiomas.Columns(4).Visible = False
                Me.gvExperiencia.Columns(5).Visible = False
                Me.gvExperiencia.Columns(6).Visible = False
            Else
                Me.Codigo = Session("IDUsuario").ToString
                imgPersona.ImageUrl = "../Images/Fotos/" & Me.Codigo & ".jpg"
                Me.txtNoDocumento.Text = Me.Codigo
                'Me.Identificacion = Request.QueryString("HojaVidaId")
                Me.Identificacion = Session("IDUsuario").ToString
            End If
            CargarHV()
        End If
    End Sub

    Sub CargarHV()
        Dim List As List(Of TH_HojaVida_Get_Result) = ObtenerEstadoActas(Me.Identificacion)
        For Each dr In List
            Me.Codigo = dr.Id
            txtNombres.Text = dr.Nombres
            txtApellidos.Text = dr.Apellidos
            txtCelular.Text = dr.Celular
            txtDireccion.Text = dr.Direccion
            txtEmail.Text = dr.Correo
            txtExtension.Text = dr.Extension
            txtNoDocumento.Text = dr.NumeroIdentificacion
            txtPerfilProfesional.Text = dr.PerfilProfesional
            txtProfesion.Text = dr.Profesion
            txtTelefono.Text = dr.Telefono
            txtTelefonoOficina.Text = dr.TelefonoOficina
            txtTextoHV.Content = dr.Texto
            txtTextoHVIngles.Text = dr.Texto_ingles
            Dim oth As New TH.HojaVida
            Try
                ddlAnoExperiencia.SelectedValue = dr.TiempoExperienciaId
            Catch ex As Exception
            End Try
            Try
                ddlPaisNacimiento.SelectedValue = dr.PaisNacimientoId
            Catch ex As Exception
            End Try
            Try
                If dr.CiudadResidenciaId IsNot Nothing Then
                    Dim dpto As Integer = oth.GetDpto(dr.CiudadResidenciaId).DivDeptoMunicipio
                    ddlDepartamentoResidencia.SelectedValue = dpto.ToString
                    CargarCiudades()
                    ddlCiudadResidencia.SelectedValue = dr.CiudadResidenciaId
                End If
            Catch ex As Exception
            End Try
            Try
                ddlPaisResidencia.SelectedValue = dr.PaisResidenciaId
            Catch ex As Exception
            End Try
            Try
                ddlEstadoCivil.SelectedValue = dr.EstadoCivilId
            Catch ex As Exception
            End Try
            Try
                ddlGenero.SelectedValue = dr.SexoId
            Catch ex As Exception
            End Try
            Try
                ddlIdioma.SelectedValue = dr.IdiomaId
            Catch ex As Exception
            End Try

            'ddlCargoActual.SelectedValue = dr.CargoActualId
            'Foto = dr.Foto
            'imgPersona.ImageUrl = Foto
            Dim fn = CDate(dr.FechaNacimiento)
            'calFechaNacimiento.Text = fn.Month & "/" & fn.Day & "/" & fn.Year
            calFechaNacimiento.Text = fn
            'chkTrabaja.Checked = dr.Trabaja
            Try
                chkViajar.Checked = dr.PosibilidadViajar
            Catch ex As Exception

            End Try
        Next
        'cargar campos
        gvEducacion.DataSource = CargarEducacion(Me.Codigo)
        gvExperiencia.DataSource = CargarExperienciaLaboral(Me.Codigo)
        gvIdiomas.DataSource = CargarIdiomas(Me.Codigo)
        gvEducacion.DataBind()
        gvExperiencia.DataBind()
        gvIdiomas.DataBind()
        DvEducacion.Visible = True
        dvExperiencia.Visible = True
        DvIdioma.Visible = True
    End Sub
    Sub CargarDepartamentos()
        Dim oCoordCampo As New CoordinacionCampo
        Dim list = (From ldep In oCoordCampo.ObtenerDepartamentos()
                  Select iddep = ldep.DivDeptoMunicipio, nomdep = ldep.DivDeptoNombre).Distinct.ToList
        ddlDepartamentoResidencia.DataSource = list
        ddlDepartamentoResidencia.DataValueField = "iddep"
        ddlDepartamentoResidencia.DataTextField = "nomdep"
        ddlDepartamentoResidencia.DataBind()
        ddlDepartamentoEd.DataSource = list
        ddlDepartamentoEd.DataValueField = "iddep"
        ddlDepartamentoEd.DataTextField = "nomdep"
        ddlDepartamentoEd.DataBind()
        ddlDepartamentoExp.DataSource = list
        ddlDepartamentoExp.DataValueField = "iddep"
        ddlDepartamentoExp.DataTextField = "nomdep"
        ddlDepartamentoExp.DataBind()
    End Sub
    Sub CargarCiudades()
        Dim oCoordCampo As New CoordinacionCampo
        Dim listciudades = (From ldep In oCoordCampo.ObtenerCiudades(ddlDepartamentoResidencia.SelectedValue)
                  Select idmuni = ldep.DivMunicipio, nommuni = ldep.DivMuniNombre)
        ddlCiudadResidencia.DataSource = listciudades
        ddlCiudadResidencia.DataValueField = "idmuni"
        ddlCiudadResidencia.DataTextField = "nommuni"
        ddlCiudadResidencia.DataBind()
    End Sub
    Sub CargarCiudadesEd()
        Dim oCoordCampo As New CoordinacionCampo
        Dim listciudades = (From ldep In oCoordCampo.ObtenerCiudades(ddlDepartamentoEd.SelectedValue)
                  Select idmuni = ldep.DivMunicipio, nommuni = ldep.DivMuniNombre)
        ddlCiudadEd.DataSource = listciudades
        ddlCiudadEd.DataValueField = "idmuni"
        ddlCiudadEd.DataTextField = "nommuni"
        ddlCiudadEd.DataBind()
    End Sub
    Sub CargarCiudadesExp()
        Dim oCoordCampo As New CoordinacionCampo
        Dim listciudades = (From ldep In oCoordCampo.ObtenerCiudades(ddlDepartamentoExp.SelectedValue)
                  Select idmuni = ldep.DivMunicipio, nommuni = ldep.DivMuniNombre)
        ddlCiudadExp.DataSource = listciudades
        ddlCiudadExp.DataValueField = "idmuni"
        ddlCiudadExp.DataTextField = "nommuni"
        ddlCiudadExp.DataBind()
    End Sub
    Protected Sub ddlPaisNacimiento_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPaisNacimiento.SelectedIndexChanged

    End Sub

    Function Validar1() As Boolean
        Dim Val = True
        If txtNombres.Text = String.Empty Then
            ShowNotification("Escriba su nombre", ShowNotifications.ErrorNotification)
            Val = False
        End If
        If txtEmail.Text = String.Empty Then
            ShowNotification("Escriba su correo electronico", ShowNotifications.ErrorNotification)
            Val = False
        End If
        If txtApellidos.Text = String.Empty Then
            ShowNotification("Escriba su apellido", ShowNotifications.ErrorNotification)
            Val = False
        End If
        If calFechaNacimiento.Text = String.Empty Then
            ShowNotification("Escriba la fecha de nacimiento", ShowNotifications.ErrorNotification)
            Val = False
        End If
        Return Val
    End Function

    Function Validar2() As Boolean
        Dim Val = True
        If txtProfesion.Text = String.Empty Then
            ShowNotification("Escriba su profesión", ShowNotifications.ErrorNotification)
            Val = False
        End If
        If txtDireccion.Text = String.Empty Then
            ShowNotification("Escriba la dirección", ShowNotifications.ErrorNotification)
            Val = False
        End If
        If ddlCiudadResidencia.SelectedIndex = -1 Then
            ShowNotification("Seleccione primero la ciudad", ShowNotifications.ErrorNotification)
            Val = False
        End If
        Return Val
    End Function

    Sub LimpiarEducacion()
        txtTituloEd.Text = String.Empty
        txtInstitucionEd.Text = String.Empty
        calInicioEd.Text = String.Empty
        calFinalizacionEd.Text = String.Empty
    End Sub

    Function ValidarEducacion() As Boolean
        Dim Val = True
        If txtTituloEd.Text = String.Empty Then
            ShowNotification("Escriba el titulo", ShowNotifications.ErrorNotification)
            Val = False
        End If
        If txtInstitucionEd.Text = String.Empty Then
            ShowNotification("Escriba la institución", ShowNotifications.ErrorNotification)
            Val = False
        End If
        If calInicioEd.Text = String.Empty Then
            ShowNotification("Escriba la fecha de inicio", ShowNotifications.ErrorNotification)
            Val = False
        End If
        If calFinalizacionEd.Text = String.Empty Then
            ShowNotification("Escriba la fecha de finalización", ShowNotifications.ErrorNotification)
            Val = False
        End If
        Return Val
    End Function

    Sub LimpiarExperiencia()
        txtEmpresaExp.Text = String.Empty
        calInicioExp.Text = String.Empty
        calFinalizacionExp.Text = String.Empty
    End Sub

    Function ValidarExperiencia() As Boolean
        Dim Val = True
        If txtEmpresaExp.Text = String.Empty Then
            ShowNotification("Escriba el nombre de la empresa", ShowNotifications.ErrorNotification)
            Val = False
        End If
        If calInicioExp.Text = String.Empty Then
            ShowNotification("Escriba la fecha de inicio", ShowNotifications.ErrorNotification)
            Val = False
        End If
        If calFinalizacionExp.Text = String.Empty And chkActualmenteExp.Checked = False Then
            ShowNotification("Escriba la fecha de finalización", ShowNotifications.ErrorNotification)
            Val = False
        End If
        Return Val
    End Function

    Sub LimpiarIdiomas()
        txtLugarIdm.Text = String.Empty
    End Sub

    Function ValidarIdiomas() As Boolean
        Dim Val = True
        If txtLugarIdm.Text = String.Empty Then
            ShowNotification("Escriba el lugar donde estudio el idioma adicional", ShowNotifications.ErrorNotification)
            Val = False
        End If
        Return Val
    End Function

    'Protected Sub imgUpload_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgUpload.Click
    '    Dim sExt As String = String.Empty
    '    Dim sName As String = String.Empty

    '    If fuFotoPersona.HasFile Then
    '        'sName = fuFotoPersona.FileName.Split(".")(0) & Date.Now.Year & Date.Now.Month & Date.Now.Day & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & "." & fuFotoPersona.FileName.Split(".")(1)
    '        sName = Me.Codigo
    '        sExt = Path.GetExtension(sName)
    '        fuFotoPersona.SaveAs(MapPath("~/Images/Fotos/" & sName))
    '        imgPersona.ImageUrl = "/Images/Fotos/" & sName
    '        Foto = "/Images/Fotos/" & sName
    '    Else
    '        'lblResult.Text = "Seleccione el archivo que desea subir."
    '    End If
    'End Sub

    Sub UploadFile()
        Dim sExt As String = String.Empty
        Dim sName As String = String.Empty

        If fuFotoPersona.HasFile Then
            'sName = fuFotoPersona.FileName.Split(".")(0) & Date.Now.Year & Date.Now.Month & Date.Now.Day & Date.Now.Hour & Date.Now.Minute & Date.Now.Second & "." & fuFotoPersona.FileName.Split(".")(1)
            sName = Me.Codigo
            sExt = Path.GetExtension(sName)
            fuFotoPersona.SaveAs(MapPath("~/Images/Fotos/" & sName & ".jpg"))
            imgPersona.ImageUrl = "/Images/Fotos/" & sName & ".jpg"
            Foto = "/Images/Fotos/" & sName & ".jpg"
        Else
            'lblResult.Text = "Seleccione el archivo que desea subir."
        End If
    End Sub

    Protected Sub ddlPaisResidencia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPaisResidencia.SelectedIndexChanged
        'CargarCombo(ddlCiudadResidencia, "Id", "Nombre", ObtenerCombo("Ciudad", sender.SelectedValue))
        'CargarDepartamentos()
    End Sub

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        ' Create array of movies
        Dim Data As New TH.HojaVida
        Dim loaddata() As String = Data.Profesiones.ToArray
        Dim movies() As String = loaddata
        count = loaddata.Count
        ' Return matching movies
        Return (
            From m In movies
            Where m.StartsWith(prefixText, StringComparison.CurrentCultureIgnoreCase)
            Select m).Take(count).ToArray()
    End Function

    Protected Sub gvExperiencia_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvExperiencia.RowCommand
        Dim EL As New TH.ExperienciaLaboral
        Try
            Select Case e.CommandName
                Case "eliminar"
                    ExperienciaId = Int32.Parse(gvExperiencia.DataKeys(CInt(e.CommandArgument))("Id"))
                    EL.EliminarExperienciaLaboral(ExperienciaId)
                    gvExperiencia.DataSource = CargarExperienciaLaboral(Me.Identificacion)
                    gvExperiencia.DataBind()
                Case "Editar"
                    ExperienciaId = Int32.Parse(gvExperiencia.DataKeys(CInt(e.CommandArgument))("Id"))
                    Dim fi = CDate(gvExperiencia.DataKeys(CInt(e.CommandArgument))("Inicio"))
                    Dim ff = CDate(gvExperiencia.DataKeys(CInt(e.CommandArgument))("Finalizacion"))
                    calInicioExp.Text = fi.Month & "/" & fi.Day & "/" & fi.Year
                    calFinalizacionExp.Text = ff.Month & "/" & ff.Day & "/" & ff.Year
                    txtEmpresaExp.Text = gvExperiencia.DataKeys(CInt(e.CommandArgument))("Empresa")
                    txtTelefonoExp.Text = gvExperiencia.DataKeys(CInt(e.CommandArgument))("Telefono")
                    chkActualmenteExp.Checked = gvExperiencia.DataKeys(CInt(e.CommandArgument))("Actualmente")
                    ddlCargoExp.SelectedValue = gvExperiencia.DataKeys(CInt(e.CommandArgument))("CargoId")
                    'ddlCargoActual.SelectedValue = gvExperiencia.DataKeys(CInt(e.CommandArgument))("CargoActualId")
                    ddlNivelCargoExp.SelectedValue = gvExperiencia.DataKeys(CInt(e.CommandArgument))("NivelCargoId")
                    ddlPaisExp.SelectedValue = gvExperiencia.DataKeys(CInt(e.CommandArgument))("PaisId")
                    ddlCiudadExp.SelectedValue = gvExperiencia.DataKeys(CInt(e.CommandArgument))("CiudadId")
                    txtDireccionExp.Text = gvExperiencia.DataKeys(CInt(e.CommandArgument))("Direccion")
                    btnAddExperiencia.Text = "Editar"
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        ActivateAccordion(4, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub gvIdiomas_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvIdiomas.RowCommand
        Dim IDM As New TH.Idiomas
        Try
            Select Case e.CommandName
                Case "eliminar"
                    IdiomaId = Int32.Parse(gvIdiomas.DataKeys(CInt(e.CommandArgument))("Id"))
                    IDM.EliminarIdioma(IdiomaId)
                    gvIdiomas.DataSource = CargarIdiomas(Me.Identificacion)
                    gvIdiomas.DataBind()
                Case "Editar"
                    IdiomaId = Int32.Parse(gvIdiomas.DataKeys(CInt(e.CommandArgument))("Id"))
                    ddlDominioIdm.SelectedValue = gvIdiomas.DataKeys(CInt(e.CommandArgument))("DominioId")
                    txtLugarIdm.Text = gvIdiomas.DataKeys(CInt(e.CommandArgument))("Lugar")
                    ddlIdiomaIdm.SelectedValue = gvIdiomas.DataKeys(CInt(e.CommandArgument))("IdiomaId")
                    btnAddIdioma.Text = "Editar"
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub gvEducacion_RowCommand(sender As Object, e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvEducacion.RowCommand
        Dim IDM As New TH.Educacion
        Try
            Select Case e.CommandName
                Case "eliminar"
                    EducacionId = Int32.Parse(gvEducacion.DataKeys(CInt(e.CommandArgument))("Id"))
                    IDM.EliminarEducacion(EducacionId)
                    gvEducacion.DataSource = CargarIdiomas(Me.Identificacion)
                    gvEducacion.DataBind()
                Case "Editar"
                    EducacionId = Int32.Parse(gvEducacion.DataKeys(CInt(e.CommandArgument))("Id"))
                    ddlNivelEstudioEd.SelectedValue = gvEducacion.DataKeys(CInt(e.CommandArgument))("NivelEstudioId")
                    txtTituloEd.Text = gvEducacion.DataKeys(CInt(e.CommandArgument))("Titulo")
                    txtInstitucionEd.Text = gvEducacion.DataKeys(CInt(e.CommandArgument))("Institucion")
                    ddlPaisEd.SelectedValue = gvEducacion.DataKeys(CInt(e.CommandArgument))("PaisId")
                    ddlCiudadEd.SelectedValue = gvEducacion.DataKeys(CInt(e.CommandArgument))("CiudadId")
                    Dim fi = CDate(gvEducacion.DataKeys(CInt(e.CommandArgument))("Inicio"))
                    Dim ff = CDate(gvEducacion.DataKeys(CInt(e.CommandArgument))("Finalizacion"))
                    calInicioEd.Text = fi.Month & "/" & fi.Day & "/" & fi.Year
                    calFinalizacionEd.Text = ff.Month & "/" & ff.Day & "/" & ff.Year
                    ddlEstadoEd.SelectedValue = gvEducacion.DataKeys(CInt(e.CommandArgument))("EstadoEducacionId")
                    btnAddEducacion.Text = "Editar"
            End Select
        Catch ex As Exception
            Throw ex
        End Try
        ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub ddlPaisEd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPaisEd.SelectedIndexChanged
        CargarCombo(ddlCiudadEd, "Id", "Nombre", ObtenerCombo("Ciudad", sender.SelectedValue))
    End Sub

    Protected Sub ddlPaisExp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPaisExp.SelectedIndexChanged
        CargarCombo(ddlCiudadExp, "Id", "Nombre", ObtenerCombo("Ciudad", sender.SelectedValue))
    End Sub

    Protected Sub btnAddEducacion_Click(sender As Object, e As EventArgs) Handles btnAddEducacion.Click
        Dim Data As New TH.Educacion
        If ValidarEducacion() Then
            If btnAddEducacion.Text = "Agregar" Then
				'Data.AgregarEducacion(Me.Codigo, ddlNivelEstudioEd.SelectedValue, txtTituloEd.Text, txtInstitucionEd.Text, ddlPaisEd.SelectedValue, ddlCiudadEd.SelectedValue, DateTime.ParseExact(calInicioEd.Text, "M/d/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(calFinalizacionEd.Text, "M/d/yyyy", CultureInfo.InvariantCulture), ddlEstadoEd.SelectedValue)
			End If
            If btnAddEducacion.Text = "Editar" Then
                Data.EditarEducacion(Me.EducacionId, Me.Codigo, ddlNivelEstudioEd.SelectedValue, txtTituloEd.Text, txtInstitucionEd.Text, ddlPaisEd.SelectedValue, ddlCiudadEd.SelectedValue, DateTime.ParseExact(calInicioEd.Text, "M/d/yyyy", CultureInfo.InvariantCulture), DateTime.ParseExact(calFinalizacionEd.Text, "M/d/yyyy", CultureInfo.InvariantCulture), ddlEstadoEd.SelectedValue)
            End If
            gvEducacion.DataSource = CargarEducacion(Me.Codigo)
            gvEducacion.DataBind()
            btnAddEducacion.Text = "Agregar"
            LimpiarEducacion()
        End If
        ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnAddIdioma_Click(sender As Object, e As EventArgs) Handles btnAddIdioma.Click
        Dim Data As New TH.Idiomas
        If ValidarIdiomas() Then
            If btnAddIdioma.Text = "Agregar" Then
                Data.AgregarIdiomas(Me.Codigo, ddlDominioIdm.SelectedValue, txtLugarIdm.Text, ddlIdiomaIdm.SelectedValue)
            End If
            If btnAddIdioma.Text = "Editar" Then
                Data.EditarIdiomas(Me.IdiomaId, Me.Codigo, ddlDominioIdm.SelectedValue, txtLugarIdm.Text, ddlIdiomaIdm.SelectedValue)
            End If
            gvIdiomas.DataSource = CargarIdiomas(Me.Codigo)
            gvIdiomas.DataBind()
            btnAddIdioma.Text = "Agregar"
            LimpiarIdiomas()
        End If
        ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
    End Sub

	'Protected Sub btnAddExperiencia_Click(sender As Object, e As EventArgs) Handles btnAddExperiencia.Click
	'Dim Data As New TH.ExperienciaLaboral
	'If ValidarExperiencia() Then
	'Dim flag As Boolean = False
	'If Not (calFinalizacionExp.Text = "") Then
	'flag = True
	'Else
	'flag = False
	'End If
	'If btnAddExperiencia.Text = "Agregar" Then
	'If flag = True Then
	'Data.AgregarExperienciaLaboral(Me.Codigo, txtEmpresaExp.Text, txtTelefonoExp.Text, CDate(calInicioExp.Text), CDate(calFinalizacionExp.Text), chkActualmenteExp.Checked, ddlCargoExp.SelectedValue, ddlNivelCargoExp.SelectedValue, ddlPaisExp.SelectedValue, ddlCiudadExp.SelectedValue, txtDireccionExp.Text)
	'Else
	'Data.AgregarExperienciaLaboral(Me.Codigo, txtEmpresaExp.Text, txtTelefonoExp.Text, CDate(calInicioExp.Text), Nothing, chkActualmenteExp.Checked, ddlCargoExp.SelectedValue, ddlNivelCargoExp.SelectedValue, ddlPaisExp.SelectedValue, ddlCiudadExp.SelectedValue, txtDireccionExp.Text)
	'End If

	'End If
	'If btnAddExperiencia.Text = "Editar" Then
	'If flag = True Then
	'				Data.EditarExperienciaLaboral(Me.ExperienciaId, Me.Codigo, txtEmpresaExp.Text, txtTelefonoExp.Text, CDate(calInicioExp.Text), CDate(calFinalizacionExp.Text), chkActualmenteExp.Checked, ddlCargoExp.SelectedValue, ddlNivelCargoExp.SelectedValue, ddlPaisExp.SelectedValue, ddlCiudadExp.SelectedValue, txtDireccionExp.Text)
	'Else
	'				Data.EditarExperienciaLaboral(Me.ExperienciaId, Me.Codigo, txtEmpresaExp.Text, txtTelefonoExp.Text, CDate(calInicioExp.Text), Nothing, chkActualmenteExp.Checked, ddlCargoExp.SelectedValue, ddlNivelCargoExp.SelectedValue, ddlPaisExp.SelectedValue, ddlCiudadExp.SelectedValue, txtDireccionExp.Text)
	'End If
	'End If
	'gvExperiencia.DataSource = CargarExperienciaLaboral(Me.Codigo)
	'gvExperiencia.DataBind()
	'btnAddExperiencia.Text = "Agregar"
	'LimpiarExperiencia()
	'End If
	'ActivateAccordion(4, EffectActivateAccordion.SlideEffect)
	'End Sub

	Public Sub cargarPaises()
        Try
            Dim oAuxiliar As New Auxiliares
            Dim listaPaises = (From lpaises In oAuxiliar.DevolverPaises()
                               Select id = lpaises.PaiPais, pais = lpaises.PaiNombre).ToList().OrderBy(Function(p) p.pais)
            ddlPaisResidencia.DataSource = listaPaises
            ddlPaisResidencia.DataTextField = "pais"
            ddlPaisResidencia.DataValueField = "id"
            ddlPaisResidencia.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    'Public Sub CargarDepartamentos(ByVal idPais As Int32)
    '    Try
    '        Dim oAuxiliar As New Auxiliares
    '        Dim listaDepartamentos = (From ldpto In oAuxiliar.DevolverDepartamentos(idPais)
    '                                  Select id = ldpto.DivDepto, dpto = ldpto.DivDeptoNombre).ToList().OrderBy(Function(d) d.dpto)
    '        ddlDepartamentoResidencia.DataSource = listaDepartamentos
    '        ddlDepartamentoResidencia.DataValueField = "id"
    '        ddlDepartamentoResidencia.DataTextField = "dpto"
    '        ddlDepartamentoResidencia.DataBind()

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    'Public Sub cargarciudades(ByVal iddpto As Int32)
    '    Try
    '        Dim oAuxiliar As New Auxiliares
    '        Dim listaCiudades = (From lciudad In oAuxiliar.DevolverCiudades(iddpto)
    '                             Select id = lciudad.DivDeptoMunicipio, idciudad = lciudad.DivMunicipio, ciudad = lciudad.DivMuniNombre).ToList().OrderBy(Function(c) c.ciudad)
    '        ddlCiudadResidencia.DataSource = listaCiudades
    '        ddlCiudadResidencia.DataValueField = "id"
    '        ddlCiudadResidencia.DataTextField = "ciudad"
    '        ddlCiudadResidencia.DataBind()

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    Protected Sub ddlDepartamentoResidencia_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDepartamentoResidencia.SelectedIndexChanged
        CargarCiudades()
        ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub btnSave1_Click(sender As Object, e As EventArgs) Handles btnSave1.Click
        If Validar1() = True Then
            If Foto = String.Empty Then Foto = "/Images/sin-foto.jpg"
            Me.Identificacion = Session("IDUsuario").ToString
            Dim Ent As New TH_HojaVida
            Ent.TipoIdentificacionId = Me.ddTipoDocumento.SelectedValue
            Ent.Id = Session("IDUsuario").ToString
            Ent.NumeroIdentificacion = Ent.Id
            Ent.FechaNacimiento = calFechaNacimiento.Text
            Ent.Nombres = txtNombres.Text
            Ent.Apellidos = txtApellidos.Text
            Ent.Correo = txtEmail.Text
            Ent.SexoId = ddlGenero.SelectedValue
            Ent.EstadoCivilId = ddlEstadoCivil.SelectedValue
            Ent.PaisNacimientoId = ddlPaisNacimiento.SelectedValue
            Dim oth As New TH.HojaVida
            oth.HojaVidaSave(Ent)
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
            log(Me.txtNoDocumento.Text, 2)
        Else
            ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

    Protected Sub btnSave2_Click(sender As Object, e As EventArgs) Handles btnSave2.Click
        If Validar2() = True Then
            Dim oth As New TH.HojaVida
            Dim Ent As New TH_HojaVida
            Ent.Id = Session("IDUsuario").ToString
            Ent = oth.HojaVidaGetxId(Ent.Id)
            Ent.PaisResidenciaId = ddlPaisResidencia.SelectedValue
            Ent.CiudadResidenciaId = ddlCiudadResidencia.SelectedValue
            Ent.Direccion = txtDireccion.Text
            Ent.Telefono = txtTelefono.Text
            Ent.Celular = txtCelular.Text
            Ent.TelefonoOficina = txtTelefonoOficina.Text
            Ent.Extension = txtExtension.Text
            Ent.Profesion = txtProfesion.Text

            oth.HojaVidaSave(Ent)
            ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
            log(Me.txtNoDocumento.Text, 2)
        Else
            ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
        End If
    End Sub

    Protected Sub btnSave3_Click(sender As Object, e As EventArgs) Handles btnSave3.Click
        ActivateAccordion(3, EffectActivateAccordion.SlideEffect)
        log(Me.txtNoDocumento.Text, 2)
    End Sub

    Protected Sub btnSave4_Click(sender As Object, e As EventArgs) Handles btnSave4.Click
        Dim oth As New TH.HojaVida
        Dim Ent As New TH_HojaVida
        Ent.Id = Session("IDUsuario").ToString
        Ent = oth.HojaVidaGetxId(Ent.Id)
        Ent.IdiomaId = ddlIdioma.SelectedValue
        oth.HojaVidaSave(Ent)
        ActivateAccordion(4, EffectActivateAccordion.SlideEffect)
        log(Me.txtNoDocumento.Text, 2)
    End Sub

    Protected Sub btnSave5_Click(sender As Object, e As EventArgs) Handles btnSave5.Click
        ActivateAccordion(5, EffectActivateAccordion.SlideEffect)
        log(Me.txtNoDocumento.Text, 2)
    End Sub

    Protected Sub btnSave6_Click(sender As Object, e As EventArgs) Handles btnSave6.Click
        Dim oth As New TH.HojaVida
        Dim Ent As New TH_HojaVida
        Ent.Id = Session("IDUsuario").ToString
        Ent = oth.HojaVidaGetxId(Ent.Id)
        Ent.TiempoExperienciaId = ddlAnoExperiencia.SelectedValue
        Ent.PosibilidadViajar = chkViajar.Checked
        oth.HojaVidaSave(Ent)
        ActivateAccordion(6, EffectActivateAccordion.SlideEffect)
        log(Me.txtNoDocumento.Text, 2)
    End Sub

    Protected Sub btnSave7_Click(sender As Object, e As EventArgs) Handles btnSave7.Click
        Dim oth As New TH.HojaVida
        Dim Ent As New TH_HojaVida
        Ent.Id = Session("IDUsuario").ToString
        Ent = oth.HojaVidaGetxId(Ent.Id)
        Ent.PerfilProfesional = txtPerfilProfesional.Text
        Ent.Texto = txtTextoHV.Content
        oth.HojaVidaSave(Ent)
        ActivateAccordion(0, EffectActivateAccordion.SlideEffect)
        log(Me.txtNoDocumento.Text, 2)
    End Sub

    Protected Sub ddlDepartamentoEd_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDepartamentoEd.SelectedIndexChanged
        CargarCiudadesEd()
        ActivateAccordion(2, EffectActivateAccordion.SlideEffect)
    End Sub

    Protected Sub ddlDepartamentoExp_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDepartamentoExp.SelectedIndexChanged
        CargarCiudadesExp()
        ActivateAccordion(4, EffectActivateAccordion.SlideEffect)
    End Sub

    Private Sub fuFotoPersona_Init(sender As Object, e As System.EventArgs) Handles fuFotoPersona.Init
        fuFotoPersona.Attributes.Add("onchange", "fuFotoPersona()")
    End Sub
    Public Sub log(ByVal iddoc As Int64?, ByVal idaccion As Int16)
        Try
            Dim log As New LogEjecucion
            log.Guardar(21, iddoc, Now(), Session("IDUsuario"), idaccion)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class