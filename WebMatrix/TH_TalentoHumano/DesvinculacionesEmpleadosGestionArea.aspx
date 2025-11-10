<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MPNewMatrix.Master" CodeBehind="DesvinculacionesEmpleadosGestionArea.aspx.vb" Inherits="WebMatrix.DesvinculacionesEmpleadosGestionArea" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH_HeadPage" ContentPlaceHolderID="head" runat="server">
    <link href="../Scripts/js/Components/Table/Table.css" rel="stylesheet">
    <link href="../Scripts/js/Components/Loader/Loader.css" rel="stylesheet">
    <link href="../Scripts/js/Components/ModalDialog/ModalDialog.css" rel="stylesheet">
    <link href="../Scripts/js/Pages/TH_TalentoHumano/DesvinculacionesEmpleadosGestionArea.css" rel="stylesheet">
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
        <a href="DesvinculacionesEmpleadosGestionArea.aspx" class="nav-link">
            <i class="metismenu-icon fa fa-search"></i>
            Desvinculaciones
        </a>
    </li>
</asp:Content>


<asp:Content ID="Content7" ContentPlaceHolderID="CPH_Content" runat="server">

    <div id="ContainerPage">
        <div id="ProcesosPendientesEvaluacion">
            <h3>Procesos pendientes evaluación</h3>
            <div id="ProcesosDesvinculacionEnCurso">
            </div>

        </div>
        <div id="EvaluacionesRealizadas">
            <h3>Evaluaciones realizadas</h3>
            <div id="ProcesosDesvinculacionRealizados">
            </div>

        </div>
    </div>
    <div id="ModalContentEvaluacion" style="display: none;">
        <div id="containerDatosEmpleado">
            <h5>Datos del empleado</h5>
            <div id="tblDatosEmpleado">
            </div>
        </div>
        <div id="containerItemsEvaluar">
            <h5>Items a evaluar</h5>
            <div id="tblItemsAreaEvaluar"></div>
        </div>
        <div id="GroupObservaciones">
            <label for="txtObservacionesEvaluacion">Observaciones</label>
            <textarea id="txtObservacionesEvaluacion" required></textarea>
        </div>
        <input type="button" id="btnGuardarEvaluacion" value="Aprobar!"></input>
    </div>
    <script type="module" defer="defer">
        import { DesvinculacionesEmpleadosGestionArea } from "../Scripts/js/Pages/TH_TalentoHumano/DesvinculacionesEmpleadosGestionArea.js"

        let page = new DesvinculacionesEmpleadosGestionArea();

    </script>
</asp:Content>
