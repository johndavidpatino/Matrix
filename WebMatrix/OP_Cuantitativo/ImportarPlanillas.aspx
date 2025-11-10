<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPPresupuestosInternos.master" CodeBehind="ImportarPlanillas.aspx.vb" Inherits="WebMatrix.ImportarPlanillas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="TitleSection" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="SideBar" runat="server">
    <li class="app-sidebar__heading">Opciones</li>
    <li>
    <a href="../op_cuantitativo/TrabajosCoordinador.aspx" class="nav-link text-primary">
        <i class="metismenu-icon fa fa-search"></i>
        Ir a Trabajos
    </a>
</li>
<li>
    <a href="../op_cuantitativo/ImportarPlanillas.aspx" class="nav-link text-secondary">
        <i class="metismenu-icon fa fa-table"></i>
        Importar planillas
    </a>
</li>
<li>
    <a href="../op_cuantitativo/PlanillasCargadas.aspx" class="nav-link text-success">
        <i class="metismenu-icon fa fa-table"></i>
        Planillas Cargadas
    </a>
</li>
<li>
    <a href="../op_cuantitativo/RevisionProductividadCoordinador.aspx" class="nav-link text-secondary">
        <i class="metismenu-icon fa fa-list"></i>
        Revisar Productividad
    </a>
</li>
<li>
    <a href="../op_cuantitativo/ProductividadRevisadaCoordinador.aspx" class="nav-link text-success">
        <i class="metismenu-icon fa fa-list"></i>
        Productividad Revisada
    </a>
</li>
<li>
    <a href="../RE_GT/HomeRecoleccion.aspx" class="nav-link text-primary">
        <i class="metismenu-icon fa fa-bookmark"></i>
        Ir al menu
    </a>
</li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Title" runat="server">
    Importar Planillas
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="SubTitle" runat="server">
    Importación de planillas de productividad
</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="Actions" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Content" runat="server">
    <div style="width: 100%">
        <asp:Panel ID="pnlLoadFile" runat="server" Visible="true">
            <div class="planillas-wrapper">
                <div class="planillas-image">
                    <img src="../Images/excel.jpg" alt="Descargar plantilla de cargue" />
                    <asp:HyperLink ID="hlPlantillaArchivo" runat="server" NavigateUrl="~/Files/Plantilla-CarguePlanillas-V2.xlsx" Text="Descargar plantilla de cargue"></asp:HyperLink>
                </div>
                <div class="planillas-controlls">
                    <asp:FileUpload ID="FileUpData" runat="server" />
                    <asp:Button ID="btnUploadFile" runat="server" Text="Enviar Archivo" />
                </div>
            </div>
            <script type="text/javascript">

                var fileInput = document.getElementById('<%= FileUpData.ClientID %>');
                fileInput.setAttribute("accept", ".xls,.xlsx")

            </script>
        </asp:Panel>
        <asp:Panel ID="PanelSuccess" runat="server" Visible="false">
            <div class="planillas-success-message">
                <div class="svg-icon">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-6">
                        <path stroke-linecap="round" stroke-linejoin="round" d="m4.5 12.75 6 6 9-13.5" />
                    </svg>
                </div>

                <p>Los datos han sido cargados</p>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
