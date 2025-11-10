<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="SolicitudAusencia.aspx.vb" Inherits="WebMatrix.TH_SolicitudAusencia" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH_HeadPage" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.tools.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/css/theme.light.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui.min.css" rel="stylesheet">
        

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    |::...  Matrix  ...::|
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Title" runat="server">
    Vacaciones y Gestión de Ausencias
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Solicitudes</li>
    <li>
        <a href="SolicitudAusencia.aspx?Request=New" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-calendar-plus"></i>
            Nueva solicitud
        </a>
    </li>
    <li>
        <a href="SolicitudAusencia.aspx?Request=Pending" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-list"></i>
            Vacaciones y beneficios pendientes
        </a>
    </li>
    <li>
        <a href="SolicitudAusencia.aspx?Request=History" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-sun"></i>
            Historial
        </a>
    </li>
    <li>
        <a href="SolicitudAusencia.aspx?Request=Approvals" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-bell"></i>
            Solicitudes por aprobar
        </a>
    </li>
    <li class="app-sidebar__heading">Gestión área</li>
    <li>
        <a href="AusenciasEquipo.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-calendar"></i>
            Ausencias del equipo
        </a>
    </li>

</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">
    <div class="col-md-12">
        <asp:HiddenField ID="hfId" runat="server" Value="0" />
        <asp:HiddenField ID="hfEstado" runat="server" Value="0" />
        <asp:HiddenField ID="hfProveedor" runat="server" Value="0" />
        <asp:HiddenField ID="hfTipoOrden" runat="server" Value="0" />
        <asp:HiddenField ID="hfSolicitante" runat="server" Value="0" />
        <asp:HiddenField ID="hfTipoBusqueda" runat="server" Value="0" />
        <asp:HiddenField ID="hfProveedorSearch" runat="server" Value="0" />
        <asp:HiddenField ID="hfSolicitanteSearch" runat="server" Value="0" />
        <asp:HiddenField ID="hfCentroCosto" runat="server" Value="0" />
        <asp:HiddenField ID="hfDuplicar" runat="server" Value="0" />
        <asp:HiddenField ID="hfIdAnterior" runat="server" Value="0" />
        <asp:HiddenField ID="hfEvaluador" runat="server" Value="0" />
        <asp:HiddenField ID="hfTipoAprobacion" runat="server" />


    </div>
    <asp:Panel ID="pnlNuevaSolicitud" runat="server" Visible="false">
        <div class="main-card mb-3 card">
            <asp:UpdatePanel ID="UpanelPresupuesto" runat="server">
    <ContentTemplate>
            <div class="card-body">
                <h5 class="card-title">Nueva solicitud</h5>
                <p class="card-subtitle">Diligencie aquí sus solicitudes de ausencia</p>
                <div>
                    <div class="form-row">
                        <div class="input-group col-md-3 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Tipo de solicitud</button>
                            </div>
                            <asp:DropDownList ID="ddlTipoSolicitud" CssClass="form-control select-form" runat="server" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="input-group col-md-4 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Fecha Inicial</button>
                            </div>
                            <asp:TextBox ID="txtFechaInicioSolicitud" AutoCompleteType="Disabled" autocomplete="off" CssClass="form-control" runat="server" AutoPostBack="true" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-4 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Fecha Final</button>
                            </div>
                            <asp:TextBox ID="txtFechaFinSolicitud" AutoCompleteType="Disabled" autocomplete="off" CssClass="form-control" runat="server" AutoPostBack="true" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-2 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Días calendario</button>
                            </div>
                            <asp:TextBox ID="txtDiasCalendario" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-2 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Días hábiles</button>
                            </div>
                            <asp:TextBox ID="txtDiasLaborales" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-8 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Comentarios</button>
                            </div>
                            <asp:TextBox ID="txtObservaciones" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="input-group col-md-4 mb-3">
                            <div class="input-group-prepend">
                                <button class="btn btn-secondary">Aprobador</button>
                            </div>
                            <asp:DropDownList ID="ddlAprobador" CssClass="form-control select-form" runat="server"></asp:DropDownList>
                        </div>
                        <div class="form-row center">
                            <div class="form-group" style="margin-top: 10px;">
                                <asp:Label ID="lblAvisoIncapacidad" runat="server" Font-Bold="true" Text="Tenga presente que debe entregar la incapacidad original a Recursos Humanos. " Visible="false" />
                                <asp:Label ID="lblAvisoIncapacidad3dias" runat="server" Font-Bold="true" Text="Es necesario que agregue también la historia clínica o epicrisis." Visible="false" />

                            </div>
                        </div>
                        <div>
                            <asp:Button ID="btnSubmitAusencia" CssClass="btn btn-primary" runat="server" Text="Radicar" />
                        </div>
                    </div>
                    <asp:Panel ID="pnlIncapacidad" runat="server" Visible="false">
                        <asp:Panel ID="pnlRHIncapacidadEmpleado" runat="server" Visible="false">
                            <div style="display: flex; flex-wrap: wrap;">
                                <div>
                                    <label for="ddlEmpleadoIncapacidad">Nombre Empleado</label>
                                    <asp:DropDownList ID="ddlEmpleadoIncapacidad" CssClass="form-control select-form" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </asp:Panel>
                        <div class="form-row">
                            <div class="input-group col-md-4 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Entidad consulta</button>
                                </div>
                                <asp:DropDownList ID="ddlIncapacidadEntidad" CssClass="form-control select-form" runat="server">
                                    <asp:ListItem Value="0" Text="--Seleccione--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="EPS"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="ARL"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Prepagada"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="input-group col-md-4 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">IPS Prestadora</button>
                                </div>
                                <asp:TextBox ID="txtIncapacidadIPS" MaxLength="100" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-4 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">No. Registro medico</button>
                                </div>
                                <asp:TextBox ID="txtIncapacidadNoRegistroMedico" MaxLength="20" ReadOnly="false" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-3 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Tipo Incapacidad</button>
                                </div>
                                <asp:DropDownList ID="ddlIncapacidadTipo" CssClass="form-control select-form" runat="server">
                                    <asp:ListItem Value="1" Text="No Aplica"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Inicial"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Prórroga"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="input-group col-md-3 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Clase de Ausencia</button>
                                </div>
                                <asp:DropDownList ID="ddlIncapacidadClaseAusencia" CssClass="form-control select-form" runat="server" AutoPostBack="true">
                                    <asp:ListItem Value="0" Text="--Seleccione--"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Enfermedad General"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Enfermedad Laboral"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Accidente de Trabajo"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Accidente de Tránsito"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="input-group col-md-3 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">SOAT</button>
                                </div>
                                <asp:DropDownList ID="ddlIncapacidadSOAT" CssClass="form-control select-form" runat="server" Enabled="false">
                                    <asp:ListItem Value="1" Text="No Aplica"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="SI"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="NO"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="input-group col-md-3 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Fecha Accidente de Trabajo</button>
                                </div>
                                <asp:TextBox ID="txtFechaAccidenteTrabajo" CssClass="form-control" runat="server" Enabled="false"  TextMode="Date"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-6 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Comentarios Incapacidad</button>
                                </div>
                                <asp:TextBox ID="txtIncapacidadObservaciones" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-3 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">CIE</button>
                                </div>
                                <asp:TextBox ID="txtCIE" runat="server" AutoPostBack="true" AutoCompleteType="Disabled"></asp:TextBox>
                            </div>
                            <div class="input-group col-md-3 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Categoría</button>
                                </div>
                                <asp:Label ID="lblDX" runat="server" Text=""></asp:Label>
                            </div>
                        </div>

                        <div>
                            <asp:Button ID="btnIncapacidadSubmit" class="btn btn-primary" runat="server" Text="Radicar" />

                        </div>

                    </asp:Panel>
                </div>

            </div>
        </ContentTemplate>
                </asp:UpdatePanel>
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlBeneficiosPendientes" runat="server" Visible="false">
        <div class="main-card mb-3 card">
    <div class="card-body">
        <h5 class="card-title">Beneficios Pendientes</h5>
        <p class="card-subtitle">Estos son todos los beneficios que están pendientes por disfrutar. Están sujetos a validación por el área de nómina</p>
        <asp:GridView ID="gvBeneficiosPendientes" runat="server" AutoGenerateColumns="false" Width="100%"
            CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0">
            <Columns>
                <asp:BoundField DataField="Beneficio" HeaderText="Beneficio" />
                <asp:BoundField DataField="Dias" HeaderText="No. Días Pendientes" />
            </Columns>
        </asp:GridView>
        </div>
            </div>
    </asp:Panel>
    <asp:Panel ID="pnlHistorialAusencia" runat="server" Visible="false">
        <div class="main-card mb-3 card">
    <div class="card-body">
        <h5 class="card-title">Historial</h5>
        <p class="card-subtitle">Estos son los beneficios que han sido solicitados</p>
        <asp:GridView ID="gvHistorialAusencia" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="ID"
            CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0">
            <Columns>
                <asp:BoundField DataField="FINICIO" HeaderText="Inicio" DataFormatString="{0:d}" />
                <asp:BoundField DataField="FFIN" HeaderText="Fin" DataFormatString="{0:d}" />
                <asp:BoundField DataField="DIASCALENDARIO" HeaderText="Dias Cal" />
                <asp:BoundField DataField="DIASLABORALES" HeaderText="Dias Lab" />
                <asp:BoundField DataField="TIPO" HeaderText="Tipo" />
                <asp:BoundField DataField="ESTADO" HeaderText="Estado" />
                <asp:BoundField DataField="OBSERVACIONESSOLICITUD" HeaderText="Observaciones Solicitud" />
                <asp:BoundField DataField="OBSERVACIONESAPROBACION" HeaderText="Observaciones Aprobación / Rechazo" />
                <asp:TemplateField HeaderText="Anular" ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="Anular" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                            CommandName="Anular" ImageUrl="~/Images/Delete_16.png" Text="Anular" ToolTip="Anular la Solicitud" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
            </div>
    </asp:Panel>
    <asp:Panel ID="pnlAprobaciones" runat="server" Visible="false">
        <div class="main-card mb-3 card">
    <div class="card-body">
        <h5 class="card-title">Aprobaciones</h5>
        <p class="card-subtitle">Si tiene equipo a cargo, aquí aparecerán las aprobaciones que requieren su aprobación</p>
        <asp:GridView ID="gvAprobacionesPendientes" runat="server" AutoGenerateColumns="false" Width="100%" DataKeyNames="ID"
            CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay aprobaciones pendientes">
            <Columns>
                <asp:BoundField DataField="EMPLEADO" HeaderText="Empleado" />
                <asp:BoundField DataField="FINICIO" HeaderText="F Inicio" DataFormatString="{0:d}" />
                <asp:BoundField DataField="FFIN" HeaderText="F Fin" DataFormatString="{0:d}" />
                <asp:BoundField DataField="DIASLABORALES" HeaderText="Dias Lab" />
                <asp:BoundField DataField="TIPO" HeaderText="Tipo" />
                <asp:BoundField DataField="OBSERVACIONESSOLICITUD" HeaderText="Observaciones Solicitud" />
                <asp:TemplateField HeaderText="Aprobar" ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="Aprobar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                            CommandName="Aprobar" ImageUrl="~/Images/Select_16.png" Text="Aprobar" ToolTip="Aprobar Solicitud" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rechazar" ShowHeader="False">
                    <ItemTemplate>
                        <asp:ImageButton ID="Rechazar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                            CommandName="Rechazar" ImageUrl="~/Images/Delete_16.png" Text="Rechazar" ToolTip="Rechazar Solicitud" />
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        </div>
            </div>
    </asp:Panel>
</asp:Content>
