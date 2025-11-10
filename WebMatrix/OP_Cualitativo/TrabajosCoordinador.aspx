<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master"
    CodeBehind="TrabajosCoordinador.aspx.vb" Inherits="WebMatrix.TrabajosCoordinadorC" %>

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
    <style>
        .text-clear {
            display: initial;
            font-weight: normal;
        }

            .text-clear label {
                display: initial;
                font-weight: normal;
                float: left;
                margin-right: 10px;
            }

            .text-clear input[type=checkbox] {
                height: 26px !important;
                margin: 0 auto !important;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Trabajos
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
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-info" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Info: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lblTextInfo">
            </label>
        </div>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;">Error: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lbltextError">
            </label>
        </div>
    </div>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <asp:LinkButton ID="lbtnVolver" runat="server" Text="Volver"></asp:LinkButton>
            <br />
            <asp:Panel runat="server" ID="accordion1">
                <div class="spacer"></div>
                <label>
                    Nombre Trabajo</label>
                <asp:TextBox ID="txtBuscar" runat="server" CssClass="textEntry"></asp:TextBox>
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                <div runat="server" id="divPropios" style="margin-top: 5px;">
                    <asp:CheckBox CssClass="text-clear" Text="Solo Propios:  " ID="chbPropios" runat="server" />
                </div>
                <div class="spacer"></div>
                <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False"
                    PageSize="25" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
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
                        <asp:BoundField DataField="NombreCoe" HeaderText="Campo Asignado" />
                        <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                        <asp:TemplateField HeaderText="Gestionar" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrGestionar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
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
            </asp:Panel>
            <asp:Panel runat="server" ID="accordion2" Visible="false">
                <h3>Opciones del trabajo
                </h3>
                <div class="spacer"></div>
                <asp:HiddenField ID="hfIdTrabajo" runat="server" />                
                <asp:HiddenField ID="hfIdProyecto" runat="server" />
                <asp:HiddenField ID="hfidCiudadId" runat="server" />
                <asp:Panel runat="server" ID="pnlCiudad">
                    <label>
                        Ciudades asignadas</label>
                    <div class="spacer"></div>
                    <asp:GridView ID="gvCiudades" runat="server" Width="100%" AutoGenerateColumns="False"
                        AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Id,CiudadId" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                            <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                            <asp:TemplateField HeaderText="Asignar Equipos" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgAsignarEquipos" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Asignar" ImageUrl="~/Images/cliente.jpg" Text="Asignar" ToolTip="Asignar Equipo de Trabajo" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Enviar Encuestas" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgEnviarEncuestas" runat="server" CausesValidation="False"
                                        CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Enviar"
                                        ImageUrl="~/Images/Select_16.png" Text="Actualizar" ToolTip="Enviar Encuestas a Bogotá" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                </asp:Panel>
                <asp:Panel ID="pnlDatos" runat="server">
                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" Visible="false" />
                    <asp:Button ID="btnSegmentos" runat="server" Text="Segmentos" Visible="false" />
                    <asp:Button ID="btnEspecificaciones" runat="server" Text="Especificaciones del Trabajo" Visible="false" />
                    <asp:Button ID="btnProgramacionCampo" runat="server" Text="Programación Campo" />
                    <%--<asp:Button ID="btnTranscripciones" runat="server" Text="Control de Transcripciones"  />--%>
                    <asp:Button ID="btnAudios" runat="server" Text="Cargar Audios" Visible="false" />
                    <asp:Button ID="btnFiltroReclutamiento" runat="server" Text="Filtro Reclutamiento" Visible="false" />
                    <asp:Button ID="btnFiltroAsistencia" runat="server" Text="Filtro Asistencia" Visible="false" />
                    <asp:Button ID="btnLoadTranscripciones" runat="server" Text="Cargar Transcripciones" Visible="false" />
                    <asp:Button ID="btnListadoDocumentos" runat="server" Text="Listado de documentos" />
                    <asp:Button ID="btnEstadoTareas" runat="server" Text="Módulo tareas" />
                    <asp:Button ID="btnAnular" runat="server" Text="Anular Trabajo" Visible="false" />
                    <asp:Button ID="btnVerInfoGeneral" runat="server" Text="Ver Información General" Visible="true" />
                    <asp:Button ID="btnVariablesControl" runat="server" Text="Evaluación Variables de Control" Width="210px" />
                </asp:Panel>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        var pageReqManger = Sys.WebForms.PageRequestManager.getInstance();
        pageReqManger.add_initializeRequest(InitializeRequest);
        pageReqManger.add_endRequest(EndRequest);
    </script>
</asp:Content>
