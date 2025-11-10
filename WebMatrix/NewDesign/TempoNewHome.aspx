<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="TempoNewHome.aspx.vb" Inherits="WebMatrix.TempoNewHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    |::...  Matrix  ...::|
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_Title" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Comercial y Proyectos</li>
    <li>
        <a href="../CU_Cuentas/Default.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-briefcase"></i>
            Cuentas
                            </a>
    </li>
    <li>
        <a href="../PY_Proyectos/Home.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-map-marked-alt"></i>
            Proyectos
                            </a>
    </li>
    <li class="app-sidebar__heading">Operaciones</li>
    <li>
        <a href="../RE_GT/HomeRecoleccion.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-mobile"></i>
            Recolección de Datos
                            </a>
    </li>
    <li>
        <a href="../RE_GT/HomeGestionTratamiento.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-database"></i>
            Tratamiento de Datos
                            </a>
    </li>
    <li>
        <a href="../ES_Estadistica/Home.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-diagnoses"></i>
            Estadística
                            </a>
    </li>
    <li class="app-sidebar__heading">Administrativo</li>
    <li>
        <a href="../FI_AdministrativoFinanciero/Default.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-file-invoice"></i>
            Administrativo y Finanzas
                            </a>
    </li>
    <li>
        <a href="../FI_AdministrativoFinanciero/Default-Compras.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-search-dollar"></i>
            Compras y Outsourcing
                            </a>
    </li>
    <li class="app-sidebar__heading">Soporte</li>
    <li>
        <a href="javascript:void(0);" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-user-friends"></i>
            Recursos Humanos
                            </a>
    </li>
    <li>
        <a href="javascript:void(0);" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-rss"></i>
            Tecnología
                            </a>
    </li>
    <li class="app-sidebar__heading">Calidad</li>
    <li><a href="../MBO/Default.aspx" class="nav-link nav-link-white">
        <i class="metismenu-icon fa fa-signal"></i>
        Proceso Gerencial
                            </a></li>
    <li>
        <a href="../GD_Documentos/Default.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-signature"></i>
            Gestión de Calidad
                            </a>
    </li>
    <li>
        <a href="../RP_Reportes/Home.aspx" class="nav-link nav-link-white">
            <i class="metismenu-icon fa fa-list"></i>
            Reportes
                            </a>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Content" runat="server">
    <div class="row">
        <div class="col-md-6 col-xl-4">
            <div class="card mb-3 widget-content bg-midnight-bloom">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="../CU_Cuentas/Default.aspx" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-briefcase"></i>
                            Cuentas
                            </a>
                    </h5>
                </div>
            </div>
            <div class="card mb-3 widget-content bg-arielle-smile">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="../PY_Proyectos/Home.aspx" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-map-marked-alt"></i>
                            Proyectos
                            </a>
                    </h5>
                </div>
            </div>
            <div class="card mb-3 widget-content bg-grow-early">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="../RE_GT/HomeRecoleccion.aspx" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-mobile"></i>
                            Recolección de Datos
                            </a>
                    </h5>
                </div>
            </div>
            <div class="card mb-3 widget-content bg-premium-dark">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="../RE_GT/HomeGestionTratamiento.aspx" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-database"></i>
                            Tratamiento de Datos
                            </a>
                    </h5>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-xl-4">
            <div class="card mb-3 widget-content bg-midnight-bloom">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="../MBO/Default.aspx" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-signal"></i>
                            Proceso Gerencial
                            </a>
                    </h5>
                </div>
            </div>
            <div class="card mb-3 widget-content bg-arielle-smile">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="../GD_Documentos/Default.aspx" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-signature"></i>
                            Gestión de Calidad
                            </a>
                    </h5>
                </div>
            </div>
            <div class="card mb-3 widget-content bg-grow-early">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="../FI_AdministrativoFinanciero/Default.aspx" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-file-invoice"></i>
                            Administrativo y Finanzas
                            </a>
                    </h5>
                </div>
            </div>
            <div class="card mb-3 widget-content bg-premium-dark">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="../FI_AdministrativoFinanciero/Default-Compras.aspx" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-search-dollar"></i>
                            Compras y Outsourcing
                            </a>
                    </h5>
                </div>
            </div>
        </div>
        <div class="col-md-6 col-xl-4">
            <div class="card mb-3 widget-content bg-midnight-bloom">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="../ES_Estadistica/Home.aspx" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-diagnoses"></i>
                            Estadística
                            </a>
                    </h5>
                </div>
            </div>
            <div class="card mb-3 widget-content bg-arielle-smile">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="javascript:void(0);" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-user-friends"></i>
                            Recursos Humanos
                            </a>
                    </h5>
                </div>
            </div>
            <div class="card mb-3 widget-content bg-grow-early">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="javascript:void(0);" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-rss"></i>
                            Tecnología
                            </a>
                    </h5>
                </div>
            </div>
            <div class="card mb-3 widget-content bg-premium-dark">
                <div class="widget-content-wrapper text-white">
                    <h5 class="card-title">
                        <a href="../RP_Reportes/Home.aspx" class="nav-link nav-link-white">
                            <i class="nav-link-icon fa fa-list"></i>
                            Reportes
                            </a>
                    </h5>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

