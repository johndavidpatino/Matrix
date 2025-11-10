<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterRecoleccion.master"
    CodeBehind="Trabajos.aspx.vb" Inherits="WebMatrix.TrabajosCOE" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Trabajos Cuantitativos
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
                    <h3 style="float: left; text-align: left;">
                        <a>
                            <label>
                                Consulta
                            </label>
                        </a>
                    </h3>
                    <div>
                        <div>
                            <asp:TextBox ID="txtID" placeholder="ID Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                            &nbsp;
                                <asp:TextBox ID="txtJobBook" placeholder="JobBook" runat="server" CssClass="textEntry"></asp:TextBox>
                            &nbsp;
                                <asp:TextBox ID="txtNombreBuscar" placeholder="Nombre Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                            <label>Estado</label>
                            <asp:DropDownList ID="ddlEstado" runat="server" AutoPostBack="true">
                                <asp:ListItem Value="-1" Text="--Ver todos--"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Activo" Selected="True"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Cerrado en OP"></asp:ListItem>
                                <asp:ListItem Value="10" Text="Cerrado"></asp:ListItem>
                                <asp:ListItem Value="11" Text="Anulado"></asp:ListItem>
                                <asp:ListItem Value="12" Text="Cerrado en Proyectos"></asp:ListItem>
                                <asp:ListItem Value="13" Text="En proceso de cierre Proyectos"></asp:ListItem>
                                <asp:ListItem Value="15" Text="En proceso de cierre Operaciones"></asp:ListItem>
                            </asp:DropDownList>
                            <div class="spacer">
                                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                            </div>

                        </div>
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
                                <asp:BoundField DataField="Metodologia" HeaderText="Metodología" />
                                <asp:BoundField DataField="GerenteProyectos" HeaderText="Gerente Proyectos" />
                                <asp:BoundField DataField="NombreUnidad" HeaderText="Unidad" />
                                <asp:BoundField DataField="EstadoTrabajo" HeaderText="Estado" />
                                <asp:TemplateField HeaderText="Gestionar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrActualizar" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Actualizar" ImageUrl="~/Images/list_16_.png" Text="Actualizar"
                                            ToolTip="Actualizar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Avance" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Avance" ImageUrl="~/Images/find_16.png" Text="Actualizar"
                                            ToolTip="Ir a Avance de Campo" />
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
                                                    Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIF((gvTrabajos.PageIndex +1) = gvTrabajos.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" Visible="false" ID="accordion1">
                    <h3 style="float: left; text-align: left;">
                        <a>
                            <label>
                                Información del trabajo
                            </label>
                        </a>
                    </h3>
                    <div class="spacer"></div>
                    <div>
                        <asp:HiddenField ID="hfIdTrabajo" runat="server" />
                        <asp:HiddenField ID="hfidConfigTrabajo" runat="server" />
                        <asp:Panel ID="pnlInfoTrabajo" runat="server" Visible="false">
                            <asp:Label ID="lblEstadoTrabajo" runat="server"></asp:Label>
                            <div class="spacer"></div>
                            <label>
                                Nombre Trabajo:
                            </label>
                            <asp:TextBox ID="txtNombre" runat="server" ReadOnly="true"></asp:TextBox>
                            <label>
                                Metodología:
                            </label>
                            <asp:TextBox ID="txtMetodologia" runat="server" ReadOnly="true"></asp:TextBox>
                            <label>
                                Muestra:
                            </label>
                            <asp:TextBox ID="txtMuestra" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="fteTxtMuestra" runat="server" FilterType="Numbers"
                                TargetControlID="txtMuestra">
                            </asp:FilteredTextBoxExtender>
                            <label>
                                Medicion:
                            </label>
                            <asp:TextBox ID="txtMedicion" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                TargetControlID="txtMedicion">
                            </asp:FilteredTextBoxExtender>
                            <label>
                                Tipo de Recolección</label>
                            <asp:DropDownList ID="ddlTipoRecoleccion" runat="server">
                            </asp:DropDownList>

                            <asp:Panel ID="pnlDatos" runat="server">
                                <div class="spacer"></div>

                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" />
                                <asp:Button ID="btnMuestra" runat="server" Text="Distribución de muestra" />
                                <asp:Button ID="btnEstimacionAuto" runat="server" Text="Estimación de Producción"
                                    OnClientClick="return confirm('Aún no hay estimación de producción, desea agregarla de manera automática?')" />
                                <asp:Button ID="btnEstimaciones" runat="server" Text="Ver Estimación de Producción" />
                                <asp:Button ID="btnEspecificaciones" runat="server" Text="Ver información general" />
                                <asp:Button ID="btnCapacitaciones" runat="server" Text="Capacitaciones" Visible="false" />
                                <asp:Button ID="btnEstadoTareas" runat="server" Text="Módulo tareas" />
                                <asp:Button ID="btnPresupuestos" runat="server" Text="Solicitud Presupuesto" />
                                <asp:Button ID="btnROCuestionario" runat="server" Text="RO Cuestionario" Visible="false" />
                                <asp:Button ID="btnROInstructivo" runat="server" Text="RO Instructivo" Visible="false" />
                                <asp:Button ID="btnROMaterialAyuda" runat="server" Text="RO Material Ayuda" Visible="false" />
                                <asp:Button ID="btnROMetodologia" runat="server" Text="RO Metodología" Visible="false" />
                                <asp:Button ID="btnCargar" runat="server" Text="ImportarDatos" Visible="false" />
                                <asp:Button ID="btnCerrarTrabajo" runat="server" Text="Cerrar Trabajo" OnClientClick="return confirm('Realmente desea cerrar el trabajo');" />
                                <asp:Button ID="btnReporteCierre" runat="server" Text="Reporte Cierre" Visible="false" />
                                <asp:Button ID="btnVariablesControl" runat="server" Text="Variables de Control" />
                                <div class="spacer"></div>
                                <asp:Button ID="btnAbrirOtroTrabajo" runat="server" Text="Abrir Otro Trabajo" />
                            </asp:Panel>
                        </asp:Panel>

                        <asp:Panel ID="pnlCierre" runat="server" Visible="false">
                            <asp:Label ID="lblInfoCierre" Text="Está seguro de esta opción? No podrá deshacer el cierre. Hágalo únicamente si ya tiene confirmación de cierre por parte del OMP en operaciones y si ya está seguro de haber cerrado la medición" Visible="false" runat="server" />
                            <div class="spacer" />
                            <asp:Label ID="lblreporte" Text="REPORTE DOCUMENTOS DE CIERRE" ForeColor="White" Font-Bold="true" Visible="false" runat="server" />
                            <asp:GridView ID="gvReporte" runat="server" AutoGenerateColumns="False" Width="100%" PageSize="25" Visible="false" DataKeyNames="Id"
                                AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar">
                                <Columns>
                                    <asp:TemplateField ShowHeader="false" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblid" runat="server" Text='<%# Eval("Id")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="IdTrabajo" HeaderText="IdTrabajo" SortExpression="IdTrabajo" />
                                    <asp:BoundField DataField="Documento" HeaderText="Documento" SortExpression="Documento" />
                                    <asp:BoundField DataField="Encontrado" HeaderText="Encontrado" ReadOnly="True" SortExpression="Encontrado" />
                                    <asp:BoundField DataField="FechaEscaneo" HeaderText="FechaEscaneo" SortExpression="FechaEscaneo" />
                                    <asp:BoundField DataField="Responsable" HeaderText="Responsable" SortExpression="Responsable" />
                                    <asp:TemplateField HeaderText="Observación">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtobservacion" runat="server" Width="200px" CssClass="required text textEntry"
                                                BorderColor="Silver" BorderStyle="Solid" TextMode="MultiLine" Text='<%# If(Not (Eval("Observacion") Is Nothing), Eval("Observacion"), "")%>'></asp:TextBox>
                                            <asp:Label ID="lblObservacion" runat="server" Width="200px" Text='<%# Eval("Observacion")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <PagerStyle CssClass="headerfooter ui-toolbar" />
                                <SelectedRowStyle CssClass="SelectedRow" />
                                <AlternatingRowStyle CssClass="odd" />
                            </asp:GridView>
                            <asp:Label ID="lblforzar" Text="Hay algunos Documentos que no se encontraron durante el Escaneo, ¿Desea forzar el Cierre de todas maneras?" Visible="false" runat="server" />
                            <br />
                            <asp:Label ID="lblobservaciones" Text="Registre Observaciones" Font-Bold="true" Visible="false" runat="server" />
                            <br />
                            <asp:TextBox ID="txtObservacionesCierre" runat="server" CssClass="required text textEntry" Width="329px" BorderColor="Silver" BorderStyle="Solid" TextMode="MultiLine" Visible="false"></asp:TextBox>
                            <div class="spacer"></div>
                            <asp:Button ID="btnConfirmarCierre" runat="server" Text="Confirmar Cierre" CssClass="causesValidation" Visible="false" />
                            <asp:Button ID="btnForzarCierre" runat="server" Text="Forzar Cierre" CssClass="causesValidation" Visible="false" />
                            <asp:Button ID="btnCancelarCierre" runat="server" Text="Cancelar" Visible="false" />
                            <asp:Button ID="btnActualizarCierre" runat="server" Text="Escanear de Nuevo" Visible="false" />
                            </fieldset>
                        </asp:Panel>

                        <asp:Panel ID="pnlEstimacion" runat="server" Visible="false">
                            <label>
                                ¿Festivos?</label>
                            <asp:CheckBox ID="chbFestivosExcluir" Text="Excluir Festivos" runat="server" />
                            <label>
                                Días a estimar</label>
                            <asp:CheckBoxList ID="chbDias" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Lunes" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Martes" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Miércoles" Value="4"></asp:ListItem>
                                <asp:ListItem Text="Jueves" Value="5"></asp:ListItem>
                                <asp:ListItem Text="Viernes" Value="6"></asp:ListItem>
                                <asp:ListItem Text="Sábado" Value="7"></asp:ListItem>
                                <asp:ListItem Text="Domingo" Value="1"></asp:ListItem>
                            </asp:CheckBoxList>
                            <div class="spacer"></div>
                            <asp:Button ID="btnGenerarPlaneacion" runat="server" Text="Generar planeación" />
                            &nbsp;
                                                <asp:Button ID="btnCancelGeneracionPlaneacion" runat="server" Text="Cancelar" />
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
