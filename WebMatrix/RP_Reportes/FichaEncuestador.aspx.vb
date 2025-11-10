Imports CoreProject
Imports WebMatrix.Util
Public Class FichaEncuestador
	Inherits System.Web.UI.Page
	Private _Identificacion As Int64
	Public Property Identificacion() As Int64
		Get
			Return _Identificacion
		End Get
		Set(ByVal value As Int64)
			_Identificacion = value
		End Set
	End Property

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		If Not IsPostBack Then
			Dim permisos As New Datos.ClsPermisosUsuarios
			Dim UsuarioID As Int64 = Int64.Parse(Session("IDUsuario").ToString())

			If Not Request.QueryString("Identificacion") Is Nothing Then
				Int64.TryParse(Request.QueryString("Identificacion"), Identificacion)
				hfPersona.Value = Identificacion
				cargarDatosEncuestador(Identificacion)
				CargarFichaEncuestador(Identificacion)
			End If
		End If
	End Sub
#Region "Metodos"
	Sub cargarDatosEncuestador(ByVal id As Int64)
		Dim oPersonas As New Personas
		Dim oeTh_Persona As TH_Personas_Get_Result


		oeTh_Persona = oPersonas.DevolverxID(id)

		lblIdentificacion.Text = oeTh_Persona.id
		lblNombres.Text = oeTh_Persona.Nombres
		lblApellidos.Text = oeTh_Persona.Apellidos
		lblCargos.Text = oeTh_Persona.CargoTexto
		If oeTh_Persona.Cargo = Cargos.TiposCargos.Encuestador Then
			lblTipoEncuestador.Text = oPersonas.ObtenerTipoEncuestador(id).OP_TipoEncuestador.Tipo
		End If
		lblCiudad.Text = oeTh_Persona.DivMuniNombre
		lblTipoContratacion.Text = oeTh_Persona.TipoContratacionTexto
		lblFechaIngreso.Text = oeTh_Persona.FechaIngreso
		lblFechaNacimiento.Text = oeTh_Persona.FechaNacimiento
		chkActivo.Checked = oeTh_Persona.Activo
		Try
			chkVetado.Checked = oPersonas.OPEncuestadoresXid(id).Vetado
		Catch ex As Exception
		End Try
		lblRazon.Text = oPersonas.OPEncuestadoresXid(id).RazonVeto

	End Sub

	Sub CargarFichaEncuestador(ByVal id As Int64)
		Dim oRep As New Reportes.RP_GerOpe
		Dim info = oRep.ObtenerFichaEncuestadorDatos(1, id)
		Me.lblRealizadas1.Text = info.ENCUESTAS
		Me.lblAnuladas1.Text = info.ANULADAS
		Me.lblError1.Text = info.ERRORES
		Me.lblRevisadas1.Text = info.REVISADAS
		Me.gvEncuestasPorMetodologia1.DataSource = oRep.ObtenerFichaEncuestadorMetodologias(1, id)
		Me.gvEncuestasPorMetodologia1.DataBind()
		Me.gvTrabajos1.DataSource = oRep.ObtenerFichaEncuestadorTrabajos(1, id)
		Me.gvTrabajos1.DataBind()

		Dim info3 = oRep.ObtenerFichaEncuestadorDatos(3, id)
		Me.lblRealizadas3.Text = info3.ENCUESTAS
		Me.lblAnuladas3.Text = info3.ANULADAS
		Me.lblError3.Text = info3.ERRORES
		Me.lblRevisadas3.Text = info3.REVISADAS
		Me.gvEncuestasPorMetodologia3.DataSource = oRep.ObtenerFichaEncuestadorMetodologias(3, id)
		Me.gvEncuestasPorMetodologia3.DataBind()
		Me.gvTrabajos3.DataSource = oRep.ObtenerFichaEncuestadorTrabajos(3, id)
		Me.gvTrabajos3.DataBind()

		Dim info6 = oRep.ObtenerFichaEncuestadorDatos(6, id)
		Me.lblRealizadas6.Text = info6.ENCUESTAS
		Me.lblAnuladas6.Text = info6.ANULADAS
		Me.lblError6.Text = info6.ERRORES
		Me.lblRevisadas6.Text = info6.REVISADAS
		Me.gvEncuestasPorMetodologia6.DataSource = oRep.ObtenerFichaEncuestadorMetodologias(6, id)
		Me.gvEncuestasPorMetodologia6.DataBind()
		Me.gvTrabajos6.DataSource = oRep.ObtenerFichaEncuestadorTrabajos(6, id)
		Me.gvTrabajos6.DataBind()
	End Sub
#End Region

	Protected Sub btnVetar_Click(sender As Object, e As EventArgs) Handles btnVetar.Click
		Dim o As New Personas
		o.VetarEncuestador(hfPersona.Value, Session("IDUsuario").ToString, txtRazonVeto.Text)
		ActivateAccordion(1, EffectActivateAccordion.SlideEffect)
		cargarDatosEncuestador(hfPersona.Value)
	End Sub
End Class