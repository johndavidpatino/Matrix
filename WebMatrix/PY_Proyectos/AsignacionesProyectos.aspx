<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterProyectos.master" CodeBehind="AsignacionesProyectos.aspx.vb" Inherits="WebMatrix.AsignacionesProyectos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Title" runat="server">
    Asignación de Gerente de Proyectos
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitle" runat="server">
    Utilice este formulario para asignar un gerente de proyectos de su unidad
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Actions" runat="server">
    <div id="divActions" class="btn-shadow mr-3 btn btn-dark">
        <asp:Button ID="btnAsignacion" runat="server" CssClass="btn btn-primary" Visible="true" Text="Proyectos sin asignar" OnClick="btnAsignacion_Click" />
        <asp:Button ID="btnReasignacion" runat="server" CssClass="btn btn-primary" Visible="true" Text="Cambiar gerente de Proyectos" OnClick="btnReasignacion_Click" />
    </div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <div class="main-card mb-3 card">
        <div class="card-body">
            <div class="form-row">

                <div class="input-group col-md-4 mb-3">
                    <div class="input-group-prepend">
                        <button class="btn btn-secondary">Unidad</button>
                    </div>
                    <asp:DropDownList ID="ddlUnidades" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlUnidades_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

            </div>
            <h5 class="card-title">Proyectos</h5>
            <p class="card-subtitle">Listado de Proyectos que han sido asignados</p>
        </div>
    </div>
    <asp:UpdatePanel ID="UPanelSearch" runat="server">
        <ContentTemplate>
            <asp:LinkButton ID="lkbModalGP" runat="server"></asp:LinkButton>
            <asp:ModalPopupExtender ID="ModalPopupExtenderGP" CancelControlID="btnCancelClone" PopupControlID="pnlGP" TargetControlID="lkbModalGP" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
            </asp:ModalPopupExtender>
            <asp:Panel ID="pnlAsignacion" runat="server">
                <div class="main-card mb-3 card">
                    <div class="card-body">
                        <div>

                            <asp:GridView ID="gvDataProyectos" runat="server" AutoGenerateColumns="false" DataKeyNames="Id, EstudioId, UnidadId" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay proyectos pendientes por asignar" OnRowCommand="gvDataProyectos_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Asignar Gerente" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbAsignarGP" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Asignar" ToolTip="Asignar Gerente de Proyectos"><i class="metismenu-icon fa fa-user-circle"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                    <asp:BoundField DataField="GP_Nombres" HeaderText="Gerente Cuentas" />
                                    <asp:BoundField DataField="EstudioId" HeaderText="Estudio" />
                                    <asp:BoundField DataField="TipoProyecto" HeaderText="Tipos proyectos" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlReasignacion" runat="server" Visible="false">
                                <div class="main-card mb-3 card">
                    <div class="card-body">
                        <div>

                            <asp:GridView ID="gvDataProyectosReasignar" runat="server" AutoGenerateColumns="false" DataKeyNames="Id, EstudioId, UnidadId" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay proyectos pendientes por asignar" OnRowCommand="gvDataProyectosReasignar_RowCommand">
                                <Columns>
                                    <asp:TemplateField HeaderText="Cambiar gerente" ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbAsignarGP" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Asignar" ToolTip="Cambiar Gerente de Proyectos"><i class="metismenu-icon fa fa-user-circle"></i></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                                    <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                    <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                    <asp:BoundField DataField="GP_Nombres" HeaderText="Gerente Proyectos" />
                                    <asp:BoundField DataField="EstudioId" HeaderText="Estudio" />
                                    <asp:BoundField DataField="TipoProyecto" HeaderText="Tipos proyectos" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel ID="pnlGP" runat="server">
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
                                                <asp:HiddenField ID="hfIDProyecto" runat="server" />
                                                <asp:HiddenField ID="hfIdUnidad" runat="server" />
                                                <div class="position-relative form-group">
                                                    <label class="">Asignación de Gerente</label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-row">
                                            <div class="col-md-12">
                                                <div class="position-relative form-group">
                                                    <label for="ddlUsuarios" class="">Unidad</label><asp:DropDownList ID="ddlUsuarios" runat="server" CssClass="form-control">
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnOk" type="button" class="btn btn-secondary" runat="server" OnClientClick="return confirm('¿Está seguro de asignar este gerente de proyectos?')" Text="Asignar Gerente" OnClick="btnOk_Click"></asp:Button>
                                <asp:Button ID="btnCancelClone" type="button" class="btn btn-secondary" runat="server" Text="Cancelar"></asp:Button>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
