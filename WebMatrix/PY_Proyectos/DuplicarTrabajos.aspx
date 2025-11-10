<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/PY_F.master"
    CodeBehind="DuplicarTrabajos.aspx.vb" Inherits="WebMatrix.DuplicarTrabajos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $("#<%= txtFechaTentativaInicioCampo.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaTentativaInicioCampo.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaTentativaFinalizacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaTentativaFinalizacion.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
<div id="info" class="information ui-corner-all ui-state-highlight" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer;">
            x
        </div>
        <p>
            <span class="ui-icon ui-icon-info"></span><strong>Info: </strong>
            <label id="lblTextInfo">
            </label>
        </p>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('error');" style="cursor: pointer;">
            x
        </div>
        <p>
            <span class="ui-icon ui-icon-alert"></span><strong>Error: </strong>
            <label id="lbltextError">
            </label>
        </p>
    </div>
    <div id="accordion">
        <div id="accordion0">
            <h3>
                <a href="#">
                    <label>
                        Duplicar Trabajo
                        <asp:HiddenField ID="hfidTrabajo" runat="server" />
                        <asp:HiddenField ID="hfidProyecto" runat="server" />
                    </label>
                </a>
            </h3>
              <asp:Panel ID="pnlNewFecha" runat="server" Visible="true">
                                
                                <p style="color:White;">Información del Trabajo</p>
                                <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Nombre:
                                            </label>
                                            <asp:TextBox ID="txtNombreTrabajo" runat="server" Width="270px"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Fecha tentativa inicio de campo:
                                            </label>
                                            <asp:TextBox ID="txtFechaTentativaInicioCampo" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Fecha tentativa finalización trabajo:
                                            </label>
                                            <asp:TextBox ID="txtFechaTentativaFinalizacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                                        </fieldset>
                                    </div>
                                    <div class="form_left">
                                        <fieldset>
                                            <label>
                                                Número de medición:
                                            </label>
                                            <asp:TextBox ID="txtNoMedicion" runat="server"></asp:TextBox>
                                        </fieldset>
                                    </div>
                           
         
            <div class="form_left">
                <fieldset>
                    <asp:CheckBox ID="CbkDocumentos" runat="server" Text="Documentos" TextAlign="Left" />
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <asp:CheckBox ID="CbkEspecificaciones" runat="server" Text="Especificaciones"
                        TextAlign="Left" />
                </fieldset>
            </div>
            <div class="form_left">
                <fieldset>
                    <asp:CheckBox ID="CbkMes" runat="server" Text="Sumar un mes"
                        TextAlign="Left" AutoPostBack ="true"  />
                </fieldset>
            </div>
       
        <div class="actions">
            <div class="form_left">
                <fieldset>
                    <asp:Button ID="btnDuplicar" runat="server" Text="Duplicar" />
                    <asp:Button ID="btnVolver" runat="server" Text="Volver" />
                    
                </fieldset>
            </div>
        </div>
    </div>
     </div>
     </asp:Panel>
</asp:Content>
