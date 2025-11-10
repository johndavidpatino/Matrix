<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="DesvinculacionesEmpleadosGestionRRHH.aspx.vb" Inherits="WebMatrix.DesvinculacionesEmpleadosGestionRRHH" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH_HeadPage" ContentPlaceHolderID="head" runat="server">
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <link href="../Scripts/js/Components/CardInfoEmpleadoDesvinculacion/CardInfoEmpleadoDesvinculacion.css" rel="stylesheet">
    <link href="../Scripts/js/Components/Paginator/Paginator.css" rel="stylesheet">
    <link href="../Scripts/js/Components/ContenedorEmpleadosDesvinculacionEstatus/ContenedorEmpleadosDesvinculacionEstatus.css" rel="stylesheet">
    <link href="../Scripts/js/Components/SearchBox/SearchBox.css" rel="stylesheet">
    <link href="../Scripts/js/Pages/TH_TalentoHumano/DesvinculacionesEmpleadosGestionRRHH.css" rel="stylesheet">
    <link href="../Scripts/js/Components/Table/Table.css" rel="stylesheet">
    <link href="../Scripts/js/Components/ModalDialog/ModalDialog.css" rel="stylesheet">
    <link href="../Scripts/js/Components/FormDesvinculacionEmpleado/FormDesvinculacionEmpleado.css" rel="stylesheet">
    <script src="../Scripts/Chosen/chosen.jquery.js" type="text/javascript"></script>
    <link href="../Scripts/Chosen/chosen.min.css" rel="stylesheet">
    <link href="../Scripts/js/Components/Loader/Loader.css" rel="stylesheet">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPH_TitleSection" runat="server">
    |::...  Matrix  ...::|
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Title" runat="server">
    Desvinculaciones
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="CPH_Subtitle" runat="server">
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="CPH_SideBar" runat="server">
    <li class="app-sidebar__heading">Opciones</li>
    <li>
        <a href="DesvinculacionesEmpleadosGestionRRHH.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-search"></i>
            Desvinculaciones
        </a>
    </li>
</asp:Content>

<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">

    <div class="ContainerPage" id="ContainerPage">
    </div>
    <script type="module" defer="defer">
        import { DesvinculacionesEmpleadosGestionRRHH } from "../Scripts/js/Pages/TH_TalentoHumano/DesvinculacionesEmpleadosGestionRRHH.js"

        let page = new DesvinculacionesEmpleadosGestionRRHH();

    </script>
</asp:Content>
