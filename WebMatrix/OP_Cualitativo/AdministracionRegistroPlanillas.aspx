<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.Master" CodeBehind="AdministracionRegistroPlanillas.aspx.vb" Inherits="WebMatrix.PlanillaModeracion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="CPH_HeadPage" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="https://fonts.googleapis.com/icon?family=Material+Icons" rel="stylesheet">
    <link href="../Scripts/js/Pages/OP_Cualitativo/AdministracionRegistroPlanillas/Componentes/ExportarRegistrosExcel/ExportarRegistrosExcel.css" rel="stylesheet">
    <link href="../Scripts/js/Pages/OP_Cualitativo/AdministracionRegistroPlanillas/AdministracionRegistroPlanillas.css" rel="stylesheet">
    <link href="../Scripts/js/Components/Paginator/Paginator.css" rel="stylesheet">
    <link href="../Scripts/js/Components/SearchBox/SearchBox.css" rel="stylesheet">
    <link href="../Scripts/js/Components/Table/Table.css" rel="stylesheet">
    <link href="../Scripts/js/Components/ModalDialog/ModalDialog.css" rel="stylesheet">
    <link href="../Scripts/js/Components/Loader/Loader.css" rel="stylesheet">
    <link href="../Scripts/js/Components/FormValidator/FormValidator.css" rel="stylesheet">
    <link rel="stylesheet" href="../Scripts/js/Components/TimePicker/jquery.timepicker.min.css">
    <script src="../Scripts/js/Components/TimePicker/jquery.timepicker.min.js"></script>

    <script>
        $(function () {
            $("#fecha").datepicker({
                dateFormat: 'dd/mm/yy',
                defaultDate: new Date(),
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

            $("#fechaInicioExportar").datepicker({
                dateFormat: 'dd/mm/yy',
                showAnim: 'fadeIn',
                beforeShow: function (input, inst) {
                    var datepickerDiv = $('#ui-datepicker-div');
                    datepickerDiv.css('z-index', 1060);
                }
            });

            $("#fechaFinExportar").datepicker({
                dateFormat: 'dd/mm/yy',
                showAnim: 'fadeIn'
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Cuali - Administración registro planillas
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <div id="Moderacion" style="border-bottom: 1px solid #808080; padding-bottom: 10px;">
        <div style="width: 50%; display: flex; column-gap: 10px;">
            <div class="columna">
                <div class="form-group">
                    <label for="tipoPlantilla">Selecciona tipo de plantilla</label>
                    <select id="tipoPlantilla" name="tipoPlantilla">
                        <option value="">Selecciona</option>
                        <option value="Moderacion">Moderacion</option>
                        <option value="Informes">Informes</option>
                    </select>
                </div>
            </div>
            <div class="columna">
                <div class="form-group">
                    <label for="statusRegistro">Selecciona Status</label>
                    <select id="statusRegistro" name="statusRegistro">
                        <option value="">Selecciona</option>
                        <option value="2">Aprobada</option>
                        <option value="3">Rechazada</option>
                    </select>
                </div>
            </div>
        </div>

        <div style="width: 50%; display: flex; gap: 10px; align-items: end;">
            <div class="form-group" style="width: 30%">
                <input type="button" id="btnFiltro" name="btnFiltro" value="Filtrar" class="btn" />
            </div>
            <div class="form-group" style="width: 30%;">
                <a href="#" id="btnExcel">Exportar a Excel</a>
            </div>
        </div>

    </div>

    <div id="TablesContainer"></div>
    <div id="ModalContainer"></div>
    <script type="module" defer="defer">
        import { AdministracionRegistroPlanillas } from "/Scripts/js/Pages/OP_Cualitativo/AdministracionRegistroPlanillas/AdministracionRegistroPlanillas.js"
        let page = new AdministracionRegistroPlanillas();

    </script>
</asp:Content>
