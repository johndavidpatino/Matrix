<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master"
    CodeBehind="Trabajos.aspx.vb" Inherits="WebMatrix.TrabajosCuali" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript">

        function loadPlugins() {

            $.validator.addMethod('selectNone',
                function (value, element) {
                    return this.optional(element) ||
                        (value != -1);
                }, "*Requerido");
            $.validator.addClassRules("mySpecificClass", { selectNone: true });

            $.validator.addMethod('selectNone2',
                function (value, element) {
                    return this.optional(element) ||
                        ($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() != "");
                }, "*Debe asignar por lo menos un presupuesto");
            $.validator.addClassRules("mySpecificClass2", { selectNone2: true });

            $("#<%= txtFechaInicio.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaTerminacion.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaTerminacion.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
            validationForm();

        }

        $(document).ready(function () {
            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });



            $('#PresupuestosAsignadosXEstudio').dialog(
                {
                    modal: true,
                    autoOpen: false,
                    title: "Presupuestos asignados",
                    width: "600px",
                    buttons: {
                        Ok: function () {
                            $(this).dialog("close");
                        }
                    }
                });

            loadPlugins();
        });
        function MostrarPresupuestosAsignadosXEstudio() {
            $('#PresupuestosAsignadosXEstudio').dialog("open");
        }

        function ActualizarPresupuestosAsignados(rowIndex, checked) {

            if (checked == true) {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val() + ";" + rowIndex + ";");
            }
            else {
                $('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val($('#CPH_Section_CPH_Section_CPH_ContentForm_hfIndicesFilasPresupuestosAsignados').val().replace(";" + rowIndex + ";", ""));
            }
        }



    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="CPH_ContentForm" runat="server">
    <div id="Notification-Error" title="Notificaciones" onclick="ShowInfoNotifications();">
        <div id="notificationHide">
            <img alt="Ver ultima notificacion" src="../Images/info_16.png" id="Img2" title="Ultima notificacion de informacion"
                onclick="runEffect('info');" style="cursor: pointer;" />
            <img alt="Ver ultima notificacion" src="../Images/error_16.png" id="Img3" onclick="runEffect('error');"
                title="Ultima notificacion de error" style="cursor: pointer;" />
        </div>
    </div>
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
    <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver"></asp:LinkButton>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div>
                <asp:Panel runat="server" ID="accordion0">
                    <h3>Trabajos
                    </h3>
                    <div class="spacer"></div>
                    <label>
                        Nombre Trabajo</label>
                    <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                    <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                        AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" />
                            <asp:BoundField DataField="JobBook" HeaderText="JobBook" />
                            <asp:BoundField DataField="NombreTrabajo" HeaderText="NombreTrabajo" />
                            <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                            <asp:BoundField DataField="FechaTentativaInicioCampo" HeaderText="Fecha Tentativa Inicio Campo"
                                DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FechaTentativaFinalizacion" HeaderText="Fecha Tentativa Finalizacion Trabajo"
                                DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="NombreMetodologia" HeaderText="Metodología" />
                            <asp:BoundField DataField="GerenteCuenta" HeaderText="Gerente de Cuenta" />
                            <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                            <asp:TemplateField HeaderText="Gestionar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Actualizar" ImageUrl="~/Images/Select_16.png" Text="Actualizar"
                                        ToolTip="Actualizar" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <PagerTemplate>
                            <div class="pagingButtons">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument="First" CommandName="Page"
                                                Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                Enabled='<%# IIF(gvTrabajos.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                        </td>
                                        <td>
                                            <span class="pagingLinks">[<%= gvTrabajos.PageIndex + 1%>-<%= gvTrabajos.PageCount%>]</span>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                Enabled='<%# IIf((gvTrabajos.PageIndex + 1) = gvTrabajos.PageCount, "false", "true") %>'
                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                Enabled='<%# IIf((gvTrabajos.PageIndex + 1) = gvTrabajos.PageCount, "false", "true") %>'
                                                SkinID="paging">Ultimo »</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </PagerTemplate>
                    </asp:GridView>
                    <div class="spacer"></div>
                </asp:Panel>
                <asp:Panel runat="server" Visible="false" ID="accordion1">
                    <h3>Información del trabajo
                    </h3>
                    <div class="spacer"></div>
                    <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                    <asp:HiddenField ID="hfidConfigTrabajo" runat="server" />
                    <div>
                        <asp:Panel ID="pnlDatos" runat="server">
                            <label>
                                Fecha de Inicio</label>
                            <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <label>
                                Fecha de Finalización</label>
                            <asp:TextBox ID="txtFechaTerminacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            <label>
                                Tipo de Recolección</label>
                            <asp:DropDownList ID="ddlTipoRecoleccion" runat="server">
                            </asp:DropDownList>
                            <div class="spacer"></div>
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
                            <asp:Button ID="btnSegmentos" runat="server" Text="Segmentos" Visible="false" />
                            <asp:Button ID="btnFicha" runat="server" Text="Especificaciones del Trabajo" />
                            <asp:Button ID="btnEntrevistas" runat="server" Text="Entrevistas" Visible="false" />
                            <asp:Button ID="btnSesiones" runat="server" Text="Sesiones" Visible="false" />
                            <asp:Button ID="btnInHome" runat="server" Text="In Home" Visible="false" />
                            <asp:Button ID="btnFiltroReclutamiento" runat="server" Text="Filtro Reclutamiento" />
                            <asp:Button ID="btnFiltroAsistencia" runat="server" Text="Filtro Asistencia" />
                            <asp:Button ID="btnEstadoTareas" runat="server" Text="Módulo tareas" />
                            <asp:Button ID="btnVariablesControl" runat="server" Text="Variables de Control" />
                        </asp:Panel>
                    </div>
                </asp:Panel>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
