<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="iFieldConfiguration.aspx.vb" Inherits="WebMatrix.iFieldConfiguration" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v22.2, Version=22.2.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/AppUsersControls/UC_LoadFiles.ascx" TagName="LoadFiles" TagPrefix="uclf" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/jquery-1.6.2.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.tools.min.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.maskedinput-1.3.min.js" type="text/javascript"></script>
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <link href="../Scripts/css/theme.light.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery-ui.min.css" rel="stylesheet">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Opciones</li>
    <li>
        <a href="../RE_GT/HomeRecoleccion.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-arrow-circle-left"></i>
            Regresar a <br />Recolección de Datos
        </a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Title" runat="server">
    Configuración de Sincronización con IField
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
    <asp:HiddenField ID="hfBrief" runat="server" Value="0" />
    Es importante realizar la configuración de los proyectos para poder realizar la sincronización de datos entre iField y Matrix de forma automatizada
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">

    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Búsqueda</h5>
            <p class="card-subtitle">Diligencie los campos por los cuales desea buscar</p>
            <div>
                <div class="form-row">
                    <div class="input-group col-md-5 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Criterio</button>
                        </div>
                        <asp:RadioButtonList ID="rbSearch" runat="server" CssClass="form-check-inline form-control form-control-sm" AutoPostBack="true" CellPadding="10" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Text="Proyectos activos" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Pendientes de configurar"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Proyectos pasados"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="input-group col-md-7 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Proyecto de iField</button>
                        </div>
                        <asp:DropDownList ID="ddlProyectosIField" runat="server" AutoPostBack="true" class="form-control-select form-control" OnSelectedIndexChanged="ddlProyectosIField_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="input-group col-md-2 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Número de trabajo</button>
                        </div>
                        <asp:TextBox ID="txtNumTrabajo" runat="server" CssClass="form-control" Enabled="true" AutoPostBack="true" OnTextChanged="txtJobBookSearch_TextChanged"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-6 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Nombre Trabajo</button>
                        </div>
                        <asp:Label ID="lblNombreTrabajo" runat="server" CssClass="form-control"></asp:Label>
                    </div>
                    <div class="input-group col-md-4 mb-3">
                        <div class="input-group-prepend">
                        </div>
                        <asp:Button runat="server" ID="btnConfirm" class="btn btn-primary" Text="Confirmar ID de Trabajo" Visible="false" OnClick="btnConfirm_Click"></asp:Button>
                    </div>
                </div>

            </div>
            <asp:UpdatePanel ID="UPanelButtons" runat="server">
                <ContentTemplate>
                    <asp:LinkButton ID="lkbModalWarning" runat="server"></asp:LinkButton>
                    <asp:LinkButton ID="lbkLoadFiles" runat="server"></asp:LinkButton>
                    <asp:ModalPopupExtender ID="ModalPopupExtenderWarning" CancelControlID="btnCloseAlert" PopupControlID="pnlMessageInfo" TargetControlID="lkbModalWarning" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
                    </asp:ModalPopupExtender>
                    <asp:Button runat="server" ID="btnNew" class="btn btn-primary" Text="Crear Nuevo" Visible="false"></asp:Button>
                    <asp:Button runat="server" ID="btnSave" class="btn btn-primary" Text="Guardar Cambios" Visible="false"></asp:Button>
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnNew" />
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="main-card mb-3 card">
        <div class="card-body">
            <p class="card-subtitle">Parámetros del proyecto seleccionado</p>
            <h5 class="card-title">Cargar nuevos usuarios</h5>
            <div class="form-row">
                <div class="input-group col-md-8 mb-3">
                                <div class="input-group-prepend">
                                    <button class="btn btn-secondary">Pegue aquí los datos</button>
                                </div>
                                <asp:TextBox ID="txtNewConfig" CssClass="form-control" TextMode="MultiLine" runat="server"></asp:TextBox>
                            </div>
                <div class="input-group col-md-4 mb-3">
                        <div class="input-group-prepend">
                        </div>
                        <asp:Button runat="server" ID="btnAddConfiguration" class="btn btn-primary" Text="Cargar" OnClick="btnAddConfiguration_Click" ></asp:Button>
                    </div>
            </div>
            <br />
            <h5 class="card-title">Usuarios cargados</h5>
            <div>
                <asp:UpdatePanel ID="UPanelSearch" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvDataConfiguracion" runat="server" AutoGenerateColumns="false" DataKeyNames="id" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay encuestadores configurados aún" OnRowCommand="gvDataConfiguracion_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="UsuarioIfield" HeaderText="Login iField" />
                                <asp:BoundField DataField="CCEncuestador" HeaderText="Encuestador" />
                                <asp:BoundField DataField="CCSupervisor" HeaderText="Supervisor" />
                                <asp:BoundField DataField="Usuario" HeaderText="Usuario" />
                                <asp:BoundField DataField="FechaConfig" HeaderText="Fecha" />
                                <asp:TemplateField HeaderText="Quitar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSelect" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Info" ToolTip="Remover este usuario"><i class="metismenu-icon fa fa-trash"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <br />

            <h5 class="card-title">Encuestas pendientes por sincronización</h5>
            <div>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:GridView ID="gvPendientes" runat="server" AutoGenerateColumns="false" DataKeyNames="NumEncuesta" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay encuestas pendientes por sincronizar" OnRowCommand="gvDataConfiguracion_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="NumEncuesta" HeaderText="No. Encuesta" />
                                <asp:BoundField DataField="Encuestador" HeaderText="Login Encuestador" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:BoundField DataField="FechaEncuesta" HeaderText="Fecha" />
                                <asp:BoundField DataField="FechaSync" HeaderText="Fecha Sincronización" />
                            </Columns>
                        </asp:GridView>

                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
    <asp:Panel ID="pnlMessageInfo" runat="server">
        <asp:UpdatePanel ID="UPanelMessage" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-sm show">
                        <div class="modal-content">
                            <div class="modal-header">
                                <p class="modal-title" id="exampleModalLabel">
                                    <asp:Label ID="lblTitleWarning" runat="server"></asp:Label>
                                </p>
                                <asp:Button ID="btnCloseAlert" runat="server" class="icon" data-dismiss="modal" aria-label="Close" Text="x"></asp:Button>
                            </div>
                            <div class="modal-body">
                                <div class="main-card mb-3 card">
                                    <div class="card-body">
                                        <asp:Panel ID="pnlMsgTextWarning" runat="server" Visible="false" class="alert alert-warning fade show" role="alert">
                                            <h6>
                                                <asp:Label ID="lblMsgTextWarning" runat="server"></asp:Label></h6>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlMsgTextError" runat="server" Visible="false" class="alert alert-danger fade show" role="alert">
                                            <h6>
                                                <asp:Label ID="lblMsgTextError" runat="server"></asp:Label></h6>
                                        </asp:Panel>
                                        <asp:Panel ID="pnlMsgTextInfo" runat="server" Visible="false" class="alert alert-info fade show" role="alert">
                                            <h6>
                                                <asp:Label ID="lblMsgTextInfo" runat="server"></asp:Label></h6>
                                        </asp:Panel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
