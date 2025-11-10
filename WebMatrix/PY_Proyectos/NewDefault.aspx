<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MP_Proyectos.master" CodeBehind="NewDefault.aspx.vb" Inherits="WebMatrix._Default1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Title" runat="server">
    Listado de Proyectos
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Actions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <div class="main-card mb-3 card">
        <div class="card-body">
            <h5 class="card-title">Proyectos</h5>
            <p class="card-subtitle">Listado de Proyectos que han sido asignados</p>
            <div>
                <asp:UpdatePanel ID="UPanelSearch" runat="server">
                    <ContentTemplate>
                        <asp:LinkButton ID="lkbModals" runat="server"></asp:LinkButton>
                        <asp:ModalPopupExtender ID="ModalPopupExtenderClonar" CancelControlID="btnCancelClone" PopupControlID="pnlDuplicar" TargetControlID="lkbModals" runat="server" Enabled="True" BackgroundCssClass="modal-backdrop fade show">
                        </asp:ModalPopupExtender>
                        <asp:GridView ID="gvDataProyectos" runat="server" AutoGenerateColumns="false" DataKeyNames="Id, EstudioId" CssClass="mb-0 table table-hover" BorderStyle="NotSet" BorderWidth="0" EmptyDataText="No hay coincidencias en la búsqueda">
                            <Columns>
                                <asp:TemplateField HeaderText="Abrir" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbSelect" runat="server" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Info" ToolTip="Abrir este JobBook"><i class="metismenu-icon fa fa-folder-open"></i></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                            <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                            <asp:BoundField DataField="TipoProyecto" HeaderText="Tipo proyecto" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </div>
</asp:Content>
