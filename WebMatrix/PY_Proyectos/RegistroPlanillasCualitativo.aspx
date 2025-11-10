<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterProyectos.Master" CodeBehind="RegistroPlanillasCualitativo.aspx.vb" Inherits="WebMatrix.PlanillaModeracion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH_HeadPage" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <link href="../Scripts/js/Components/Paginator/Paginator.css" rel="stylesheet">
    <link href="../Scripts/js/Components/SearchBox/SearchBox.css" rel="stylesheet">
    <link href="../Scripts/js/Components/Table/Table.css" rel="stylesheet">
    <link href="../Scripts/js/Components/ModalDialog/ModalDialog.css" rel="stylesheet">
    <link href="../Scripts/js/Components/FormValidator/FormValidator.css" rel="stylesheet">
    <script src="../Scripts/Chosen/chosen.jquery.js" type="text/javascript"></script>
    <link href="../Scripts/Chosen/chosen.min.css" rel="stylesheet">
    <link href="../Scripts/js/Components/Loader/Loader.css" rel="stylesheet">
    <link href="../Scripts/js/Pages/PY_Proyectos/RegistroPlanillasCualitativo/RegistroPlanillasCualitativo.css" rel="stylesheet">
    <link rel="stylesheet" href="../Scripts/js/Components/TimePicker/jquery.timepicker.min.css">
    <script src="../Scripts/js/Components/TimePicker/jquery.timepicker.min.js"></script>
    <script type="module" defer="defer">
        
        $(function () {
            $("#fecha").datepicker({
                dateFormat: 'dd/mm/yy',
                showAnim: 'fadeIn'
            });
            $("#fechaEntregaInformes").datepicker({
                dateFormat: 'dd/mm/yy',
                showAnim: 'fadeIn'
            });
            $("#hora").timepicker({
                timeFormat: 'h:mm p',
                interval: 30,
                minTime: '06',
                maxTime: '11:30pm',
                defaultTime: '06',
                startTime: '06:00',
                dynamic: false,
                dropdown: true,
                scrollbar: true
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Cuali - Registro planillas 
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <div id="Moderacion" style="border-bottom: 1px solid #808080">
        <div class="columna">
            <div class="form-group">
                    <label for="tipoPlantilla">Selecciona tipo de plantilla</label>
                    <select id="tipoPlantilla" name="tipoPlantilla">
                        <option value="">Selecciona...</option>
                        <option value="Moderacion">Moderacion</option>
                        <option value="Informes">Informes</option>
                    </select>
                </div>
        </div>
    </div>
    
    <div id="FormsContainer"></div>
    <div id="TablesContainer"></div>
    
    <script type="module" defer="defer">
        import { PlanillaModeracion } from "/Scripts/js/Pages/PY_Proyectos/RegistroPlanillasCualitativo/RegistroPlanillasCualitativo.js"
        let page = new PlanillaModeracion();

    </script>
</asp:Content>
