<%@ Page Title="" Language="vb" AutoEventWireup="false" EnableEventValidation="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="Default.aspx.vb" Inherits="WebMatrix._DefaultCuentas" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function CheckIncludePresupuestos() {
            var checkbox = document.getElementById("CPH_Content_chbIncluirPresupuestos")
            if (check.checked = true) {
                document.getElementById("divPresupuestos").style.visibility = 'visible';
            }
            if (check.checked != true) {
                document.getElementById("divPresupuestos").style.visibility = 'hidden';
            }
        };
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
    Búsqueda y creación de JobBook
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Búsqueda</h5>
            <p class="card-subtitle">Diligencie los campos por los cuales desea buscar</p>
            <div>
                <div class="form-row">
                    <div class="input-group col-md-4 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Criterio</button>
                        </div>
                        <asp:RadioButtonList ID="rbSearch" runat="server" CssClass="form-check-inline form-control form-control-sm" CellPadding="10" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1" Text="Mis jobs" Selected="True"></asp:ListItem>
                            <asp:ListItem Value="2" Text="Los de mi unidad"></asp:ListItem>
                            <asp:ListItem Value="3" Text="Todos"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="input-group col-md-3 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">Título</button>
                        </div>
                        <asp:TextBox ID="txtTituloSearch" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-3 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">JobBook</button>
                        </div>
                        <asp:TextBox ID="txtJobBookSearch" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="input-group col-md-2 mb-3">
                        <div class="input-group-prepend">
                            <button class="btn btn-secondary">No Propuesta</button>
                        </div>
                        <asp:TextBox ID="txtIdPropuestaSearch" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                </div>
                <asp:Button runat="server" ID="btnSearch" class="btn btn-primary" Text="Buscar" OnClick="btnSearch_Click"></asp:Button>
                <asp:Button runat="server" ID="btnNew" class="btn btn-primary" Text="Crear Nuevo" OnClick="btnNew_Click"></asp:Button>
            </div>

        </div>
    </div>
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Resultados</h5>
            <p class="card-subtitle">Resultados de búsqueda</p>
            <div>
                <asp:UpdatePanel ID="UPanelSearch" runat="server">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbModals" runat="server"></asp:LinkButton>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderClonar" CancelControlID="btnCancelClone" PopupControlID="pnlDuplicar" TargetControlID="lkbModals" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
                        </asp:ModalPopupExtender>
                        <asp:GridView ID="gvDataSearch" runat="server" AutoGenerateColumns="false" DataKeyNames="IdBrief,IdPropuesta,IdEstudio" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay coincidencias en la búsqueda" OnRowCommand="gvDataSearch_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Abrir" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSelect" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Info" ToolTip="Abrir este JobBook"><i class="metismenu-icon fa fa-folder-open"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Clonar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbDuplicar" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Duplicate" ToolTip="Duplicar JobBook"><i class="metismenu-icon fa fa-clone"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="IdPropuesta" HeaderText="Propuesta" />
                                <asp:BoundField DataField="Cliente" HeaderText="Cliente" />
                                <asp:BoundField DataField="Titulo" HeaderText="Nombre" />
                                <asp:BoundField DataField="MarcaCategoria" HeaderText="Marca o Categoría" HeaderStyle-Wrap="true" />
                                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                                <asp:BoundField DataField="GerenteCuentas" HeaderText="Gerente Cuentas" HeaderStyle-Wrap="true" />
                                <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
    <asp:Panel ID="pnlDuplicar" runat="server">
        <asp:UpdatePanel ID="UPanelClonar" runat="server">
            <ContentTemplate>
                <div class="bd-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-sm show">
                        <div class="modal-content">
                            <div class="modal-body">
                                <div class="main-card mb-3 card">
                                    <div class="card-body">
                                        <div class="form-row">
                                            <div class="col-md-12">
                                                <asp:HiddenField ID="hfBriefToDuplicar" runat="server" />
                                                <div class="position-relative form-group">
                                                    <label class="">Opciones de duplicación</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-md-9">
                                                <div class="position-relative form-group">
                                                    <label for="txtNombre" class="">Nuevo nombre del JobBook</label><asp:TextBox runat="server" ID="txtNuevoNombre" class="form-control" />
                                                </div>
                                            </div>
                                            <div class="col-md-3">
                                                <div class="position-relative form-group">
                                                    <label for="ddlUnidades" class="">Unidad</label><asp:DropDownList ID="ddlUnidades" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnOkClone" type="button" class="btn btn-secondary" runat="server" OnClientClick="return confirm('¿Está seguro de clonar este Frame?')" OnClick="btnOkClone_Click" Text="Duplicar"></asp:Button>
                                <asp:Button ID="btnCancelClone" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
