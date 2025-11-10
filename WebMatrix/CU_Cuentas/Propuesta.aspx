<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="Propuesta.aspx.vb" Inherits="WebMatrix.PropuestaForm" %>

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
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%= txtJobBook.ClientId %>").mask("99-999999");
            $("#<%= txtFechaEnvio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaEnvio.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });
            $("#<%= txtFechaAprobacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaAprobacion.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });
            $("#<%= txtFechaInicioCampo.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicioCampo.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
                beforeShow: function () {
                    setTimeout(function () {
                        $('.ui-datepicker').css('z-index', 99999999999999);
                    }, 0);
                }
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Opciones</li>
    <li>
        <a href="Default.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-search"></i>
            Buscar o crear
        </a>
    </li>
    <li>
        <a href="Frame.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-book-reader"></i>
            Brief / Frame
        </a>
    </li>
    <li>
        <a href="Propuesta.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-file-alt"></i>
            Información de la Propuesta
        </a>
    </li>
    <li>
        <a href="Presupuesto.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-calculator"></i>
            Presupuestos
        </a>
    </li>
    <li>
        <a href="Estudio.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-tag"></i>
            Estudios aprobados
        </a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Title" runat="server">
    Información de la Propuesta
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
    <asp:HiddenField ID="hfPropuesta" runat="server" />
    Información general relacionada con propuesta. Esta información es muy importante ya que es tenida en cuenta para planeación y pasos posteriores en caso de ser aprobada
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <div class="main-card mb-3 card">
        <div class="card-body">
            <div>
                <div class="form-row">
                    <div class="input-group col-md-4 mb-3" id="ModalDatePicker" data-tooltip="El jobbook se crea en Symphony. Registre aquí el número de Job antes de agregar los presupuestos">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">JobBook</button>
                        </div>
                        <asp:TextBox ID="txtJobBook" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-4 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Estado</button>
                        </div>
                        <asp:DropDownList ID="ddlestadopropuesta" runat="server" CssClass="form-control">
                            <asp:ListItem Value="1" Text="Creada"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Enviada"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Vendida"></asp:ListItem>
                            <asp:ListItem Value="4" Text="Cancelada"></asp:ListItem>
                            <asp:ListItem Value="5" Text="Cerrada"></asp:ListItem>
                            <asp:ListItem Value="6" Text="Perdida"></asp:ListItem>
                            <asp:ListItem Value="7" Text="Error"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="input-group col-md-4 mb-3" data-tooltip="Defina la probabilidad de aprobación de esta propuesta">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Probabilidad</button>
                        </div>
                        <asp:DropDownList ID="ddlprobabilidadaprob" runat="server" CssClass="form-control">
                            <asp:ListItem Value="75" Text="Alta"></asp:ListItem>
                            <asp:ListItem Value="50" Text="Media"></asp:ListItem>
                            <asp:ListItem Value="25" Text="Baja"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="input-group col-md-4 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Fecha Envío</button>
                        </div>
                        <asp:TextBox ID="txtFechaEnvio" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-4 mb-3" data-tooltip="Esta es la fecha en que se aprobó o se canceló la propuesta">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">F Aprob / No Aprob</button>
                        </div>
                        <asp:TextBox ID="txtFechaAprobacion" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-4 mb-3">
                        <div class="input-group-prepend" data-tooltip="Escriba la fecha en que estima que iniciaría el campo. Esta información es importante para planeación">
                            <button class="btn btn-secondary">F Inicio Campo</button>
                        </div>
                        <asp:TextBox ID="txtFechaInicioCampo" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-4 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Plazo de pago (días)</button>
                        </div>
                        <asp:TextBox ID="txtPlazoPago" Text="30" CssClass="form-control" TextMode="Number" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-4 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Anticipo (%)</button>
                        </div>
                        <asp:TextBox ID="txtAnticipo" Text="70" CssClass="form-control" TextMode="Number" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-4 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Saldo (%)%</button>
                        </div>
                        <asp:TextBox ID="txtSaldo" Text="30" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-12 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Requerimientos De Habeas Data/ Confidencialidad/ Privacidad Y Seguridad De La Información</button>
                        </div>
                        <asp:TextBox ID="txtHabeasData" Text="De acuerdo a la ley colombiana" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>

                </div>
                <asp:UpdatePanel ID="UPanelButtons" runat="server">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbModalWarning" runat="server"></asp:LinkButton>
                <asp:Button runat="server" ID="btnSave" class="btn btn-primary" Text="Guardar Cambios" Visible="false" OnClick="btnSave_Click"></asp:Button>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderWarning" CancelControlID="btnCloseAlert" PopupControlID="pnlMessageInfo" TargetControlID="lkbModalWarning" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
                        </asp:ModalPopupExtender>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>

        </div>
    </div>
    <div class="main-card mb-3 card">
        <div class="card-body">
            <asp:Button runat="server" ID="btnLoadFiles" CssClass="btn btn-secondary" Visible="false" Text="Ver / Cargar Archivos" OnClick="LoadFiles_Click" />
            <asp:Panel ID="pnlLoadFiles" runat="server" Visible="false">
                <div>
                    <uclf:LoadFiles ID="UCFiles" runat="server" />
                </div>
            </asp:Panel>
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
