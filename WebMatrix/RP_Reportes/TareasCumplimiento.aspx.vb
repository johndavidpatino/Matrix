Imports CoreProject
Imports WebMatrix.Util
Public Class TareasCumplimiento
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            cargarunidades()
            cargargerenciasoperaciones()
            cargarCoreUnidades()

        End If

        Me.IndCumplimiento.Visible = False
        Me.IndOportundad.Visible = False
        Me.LblIndCum.Visible = False
        Me.lblIndOpor.Visible = False
        Me.lblCuenta.Visible = False
        Me.LblTextCuenta.Visible = False

        ActivateAccordion(1, EffectActivateAccordion.NoEffect)

    End Sub

    Protected Sub btnConsultar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnConsultar.Click
        Dim TareaId As Int64?, idtrabajo As Int64?, RolEstima As Int64?, UnidadEjecuta As Int64?, EstadoNo As Int64?, UsuarioId As Int64?, Estado As Int64?, Finip As Date?, Ffinp As Date?, Finir As Date?, Ffinr As Date?, Unidad As Int64?, GrupoUnidad As Int64?

        If ddlUnidad.SelectedValue = -1 Or ddlUnidad.SelectedValue = "" Then
            Unidad = Nothing
        Else
            Unidad = ddlUnidad.SelectedValue
        End If

        If ddlGerenteProyectos.SelectedValue = "-1" Or ddlGerenteProyectos.SelectedValue = "" Then
            UsuarioId = Nothing
        Else
            UsuarioId = ddlGerenteProyectos.SelectedValue
        End If

        If ddlGerenciasOp.SelectedValue = "-1" Or ddlGerenciasOp.SelectedValue = "" Then
            GrupoUnidad = Nothing
        Else
            GrupoUnidad = ddlGerenciasOp.SelectedValue
        End If

        If ddlGerenciaEjecuta.SelectedValue = "-1" Or ddlGerenciaEjecuta.SelectedValue = "" Then
            UnidadEjecuta = Nothing
        Else
            UnidadEjecuta = ddlGerenciaEjecuta.SelectedValue
        End If

        If (ddlFechas.SelectedValue = "-1" Or ddlFechas.SelectedValue = "") Then
            Finip = Nothing
            Ffinp = Nothing
            Finir = Nothing
            Ffinr = Nothing

        Else
            If ddlFechas.SelectedValue = 1 And txtFechaFinalizacion.Text <> "" And txtFechaInicio.Text <> "" Then
                Finip = txtFechaInicio.Text
                Ffinp = txtFechaFinalizacion.Text
                Finir = Nothing
                Ffinr = Nothing
            ElseIf ddlFechas.SelectedValue = 2 And txtFechaFinalizacion.Text <> "" And txtFechaInicio.Text <> "" Then
                Finip = Nothing
                Ffinp = Nothing
                Finir = txtFechaInicio.Text
                Ffinr = txtFechaFinalizacion.Text
            End If
        End If

        cargartareas(TareaId, idtrabajo, RolEstima, UnidadEjecuta, EstadoNo, UsuarioId, Estado, Finip, Ffinp, Finir, Ffinr, Unidad, GrupoUnidad)
        Me.IndCumplimiento.Visible = True
        Me.IndOportundad.Visible = True
        Me.LblIndCum.Visible = True
        Me.lblIndOpor.Visible = True
        Me.lblCuenta.Visible = True
        Me.LblTextCuenta.Visible = True
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub cargartareas(ByVal TareaId As Int64?, ByVal idtrabajo As Int64?, ByVal RolEstima As Int64?, ByVal UnidadEjecuta As Int64?, ByVal EstadoNo As Int64?, ByVal UsuarioId As Int64?, ByVal Estado As Int64?, ByVal Finip As Date?, ByVal FFnip As Date?, ByVal Finir As Date?, ByVal Ffnir As Date?, ByVal Unidad As Int64?, ByVal GrupoUnidad As Int64?)
        Dim oRep_fil As New Reportes.GerpProyectos
        gvTareas.DataSource = oRep_fil.obtenertareas(TareaId, idtrabajo, RolEstima, UnidadEjecuta, EstadoNo, UsuarioId, Estado, Finip, FFnip, Finir, Ffnir, Unidad, GrupoUnidad)
        gvTareas.DataBind()

        If gvTareas.Rows.Count >= 1 Then
            indicadorCumplimiento()
            indicadoroportunidad()
            lblCuenta.Text = gvTareas.Rows.Count
        End If

        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub cargarunidades()
        Dim oUnidades As New CoreProject.US.Unidades
        ddlUnidad.DataSource = oUnidades.ObtenerUnidadesxUsuario(Session("IDUsuario").ToString)
        ddlUnidad.DataTextField = "Unidad"
        ddlUnidad.DataValueField = "id"
        ddlUnidad.DataBind()
        ddlUnidad.Items.Insert(0, New ListItem With {.Text = "--Ver Todo--", .Value = -1})
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub cargargerentes(ByVal Unidad As Int64)
        Try
            Dim oUsuarios As New US.Usuarios

            Dim listapersonas = (From lpersona In oUsuarios.UsuariosxUnidadXrol(Unidad, ListaRoles.GerenteProyectos)
                                 Select Id = lpersona.id,
                                 Nombre = lpersona.Apellidos & " " & lpersona.Nombres).OrderBy(Function(p) p.Nombre)
            ddlGerenteProyectos.DataSource = listapersonas.ToList()
            ddlGerenteProyectos.DataValueField = "Id"
            ddlGerenteProyectos.DataTextField = "Nombre"
            ddlGerenteProyectos.DataBind()
            ddlGerenteProyectos.Items.Insert(0, New ListItem With {.Text = "--Ver Todos--", .Value = -1})
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Sub cargargerenciasoperaciones()
        Dim oGruposUnidad As New US.GrupoUnidad
        ddlGerenciasOp.DataSource = oGruposUnidad.ObtenerGrupoUnidadCombo(2)
        ddlGerenciasOp.DataValueField = "id"
        ddlGerenciasOp.DataTextField = "GrupoUnidad"
        ddlGerenciasOp.DataBind()
        ddlGerenciasOp.Items.Insert(0, New ListItem With {.Text = "--Ver Todas--", .Value = -1})
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)

    End Sub

    Sub cargartrabajos(ByVal geretenid As Int64)
        Dim oRep_fil As New Reportes.GerpProyectos
        ddlTrabajo.DataSource = oRep_fil.ObtenerTrabajosPorUsuario(geretenid)
        ddlTrabajo.DataValueField = "Id"
        ddlTrabajo.DataTextField = "NombreTrabajo"
        ddlTrabajo.DataBind()
        ddlTrabajo.Items.Insert(0, New ListItem With {.Text = "--Ver Todos--", .Value = -1})
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Sub indicadorCumplimiento()
        Dim inc As Integer
        Dim cumpl As Integer
        Dim Cont As Integer
        Dim i As Integer
        Dim INDCUMP As Double
        For i = 0 To gvTareas.Rows.Count - 1
            If IsNumeric(gvTareas.Rows(i).Cells(20).Text) = True Then
                If CInt(gvTareas.Rows(i).Cells(20).Text) = 0 Then
                    inc = CInt(gvTareas.Rows(i).Cells(20).Text) + inc
                ElseIf CInt(gvTareas.Rows(i).Cells(20).Text) = 1 Then
                    cumpl = CInt(gvTareas.Rows(i).Cells(20).Text) + cumpl
                End If
            End If
            Cont = Cont + 1
        Next

        If ((cumpl) / Cont) < 0 Then
            IndCumplimiento.Text = "%"
        Else
            INDCUMP = ((cumpl) / Cont) * 100
            IndCumplimiento.Text = Math.Round(INDCUMP, 1) & "%"
        End If
        inc = 0
        Cont = 0
        cumpl = 0

    End Sub

    Sub indicadoroportunidad()
        Dim inc As Integer
        Dim cumpl As Integer
        Dim Cont As Integer
        Dim i As Integer
        Dim INDOPT As Double
        For i = 0 To gvTareas.Rows.Count - 1
            If gvTareas.Rows(i).Cells(21).Text <> gvTareas.Rows(i).Cells(22).Text Then
                inc = inc + 1
            ElseIf gvTareas.Rows(i).Cells(21).Text >= gvTareas.Rows(i).Cells(22).Text Then
                cumpl = cumpl + 1
            End If
            Cont = Cont + 1
        Next
        If ((cumpl) / Cont) <= 0 Then
            IndOportundad.Text = "0%"
        Else
            INDOPT = ((cumpl / Cont) * 100)
            IndOportundad.Text = Math.Round(INDOPT, 1) & "%"
        End If

        inc = 0
        Cont = 0
        cumpl = 0
    End Sub

    Sub cargarCoreUnidades()
        Dim CoreUnidad As New CoreProject.Unidades
        ddlGerenciaEjecuta.DataSource = CoreUnidad.ObtenerUnidadesCore()
        ddlGerenciaEjecuta.DataValueField = "Id"
        ddlGerenciaEjecuta.DataTextField = "Unidad"
        ddlGerenciaEjecuta.DataBind()
        ddlGerenciaEjecuta.Items.Insert(0, New ListItem With {.Text = "--Ver Todos--", .Value = -1})
        ActivateAccordion(1, EffectActivateAccordion.NoEffect)
    End Sub

    Private Sub ddlUnidad_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlUnidad.SelectedIndexChanged
        cargargerentes(ddlUnidad.SelectedValue)
    End Sub


    Private Sub ddlGerenteProyectos_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlGerenteProyectos.SelectedIndexChanged
        cargartrabajos(ddlGerenteProyectos.SelectedValue)
    End Sub


End Class