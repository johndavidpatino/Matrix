<%@ Page Title="..: MATRIX :.. Cargue de Descuentos SS" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="CargueDescuentosSS.aspx.vb" Inherits="WebMatrix.CargueDescuentosSSForm" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>

<%@ Register Src="~/AppUsersControls/UC_Header_Presupuesto.ascx" TagName="HeaderPresupuesto" TagPrefix="uch" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    <asp:Label ID="lblInfo" runat="server"></asp:Label>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <asp:Panel ID="pnlSideBar" runat="server">
        <li class="app-sidebar__heading">Opciones</li>
        <li>
            <a href="../FI_AdministrativoFinanciero/Default.aspx" class="nav-link">
                <i class="metismenu-icon fa fa-arrow-left"></i>
                Regresar
            </a>
        </li>
    </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Title" runat="server">
    Cargue de descuentos
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_Subtitle" runat="server">
    Cargue de descuentos de seguridad social
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Actions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_Content" runat="server">
    <div class="main-card mb-3 card">
        <div class="card-body">
            <asp:Panel ID="pnlExcelMuestra" runat="server">
                <asp:UpdatePanel ID="UPanelExcelMuestra" runat="server">
                    <ContentTemplate>
                        <div class="bd-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                            <div class="modal-dialog modal-lg show">
                                <div class="modal-content">
                                    <div class="modal-body">
                                        <div class="main-card mb-3 card">
                                            <div class="card-body">
                                                <h5 class="card-title">Importar descuentos de seguridad social desde Excel</h5>
                                                <div class="form-inline">
                                                    <label>Descargue el formato en Excel desde este icono para cargar los datos. No cambie la estructura ni inserte o elimine filas</label>
                                                    <a style="margin-left: 10px;" href="../Files/FormatoCargueDescuentosSS.xlsx" target="_blank">
                                                        <img src="../Images/excel.jpg" alt="Excel de importar" style="width: 36px; height: 36px" /></a>
                                                </div>
                                                <div>
                                                    <div class="form-inline">
                                                        <div class="mb-2 mr-sm-2 mb-sm-0 position-relative form-group">
                                                            <label for="FUploadExcelMuestra" class="mr-sm-2">Seleccione el archivo a subir</label>
                                                            <asp:FileUpload ID="FUploadExcel" runat="server" CssClass="form-control-min-select form-control" />
                                                        </div>
                                                        <br />
                                                        <div class="form-inline">
                                                            <asp:Button ID="btnLoadDataExcel" runat="server" Text="Importar la información" CssClass="btn btn-primary" OnClick="btnLoadDataExcel_Click" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                    <div class="modal-footer">
                                        <asp:Label ID="lblMessages" ForeColor="Red" Visible="false" runat="server" Text=""></asp:Label>
                                        <asp:Label ID="lblMessageSuccess" ForeColor="Green" Visible="false" runat="server" Text=""></asp:Label>
                                    </div>
                                </div>
                            </div>
                            <span>Texto al final de content template</span>
                        </div>
                        
                    </ContentTemplate>  
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btnLoadDataExcel" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>

    </div>

</asp:Content>
