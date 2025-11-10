<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterProyectos.master"
    CodeBehind="Trabajos.aspx.vb" Inherits="WebMatrix.Trabajos1" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../css/jquery-ui.css" rel="stylesheet">
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

            validationForm();

        }

        $(document).ready(function () {
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

        $(function () {
            $("#gvReporte").dialog({
                autoOpen: false,
                show: {
                    effect: "blind",
                    duration: 1000
                },
                hide: {
                    effect: "explode",
                    duration: 1000
                }
            });

            $("#btnCerrarTrabajo").click(function () {
                $("#gvReporte").dialog("open");
            });
        });

    </script>
    <style type="text/css">
        .lblTextInfo {
            display: block;
            font-weight: lighter;
            text-align: right;
            padding-top: 5px;
            width: 150px;
            float: left;
            font-family: 'Roboto', sans-serif;
            font-size: 13px;
        }

        .lblTitleInfo {
            font-family: 'Roboto', sans-serif;
            font-size: 13px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="CPH_OpcionesMenu" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="CPH_Titulo" runat="server">
    Trabajos
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="CPH_ComentFormulario" runat="server">
    Este espacio es uno de los más importantes para los Gerentes de Proyectos. Asegúrese de completar todos los campos,
    incluyendo las especificaciones para Operaciones.
    <br />
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
    
    Proyecto:
    <asp:Label ID="lblProyecto" runat="server"></asp:Label>
    <br />
    <asp:LinkButton ID="lnkProyecto" runat="server" Text="Volver"></asp:LinkButton>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <div>
                <asp:Panel runat="server" ID="accordion0">
                    <h3 style="float: left; text-align: left;">
                        <a>Consulta de Trabajos
                        </a>
                    </h3>
                    <div class="spacer"></div>
                    <asp:TextBox ID="txtID" placeholder="ID Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                    &nbsp;
                                <asp:TextBox ID="txtJobBook" placeholder="JobBook" runat="server" CssClass="textEntry"></asp:TextBox>
                    &nbsp;
                                <asp:TextBox ID="txtNombreBuscar" placeholder="Nombre Trabajo" runat="server" CssClass="textEntry"></asp:TextBox>
                    <label>Estado</label>
                    <asp:DropDownList ID="ddlEstado" runat="server">
                        <asp:ListItem Value="-1" Text="--Ver todos--"></asp:ListItem>
                        <asp:ListItem Value="1" Text="Activo"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Cerrado en OP"></asp:ListItem>
                        <asp:ListItem Value="10" Text="Cerrado"></asp:ListItem>
                        <asp:ListItem Value="11" Text="Anulado"></asp:ListItem>
                        <asp:ListItem Value="12" Text="Cerrado en Proyectos"></asp:ListItem>
                        <asp:ListItem Value="13" Text="En proceso de cierre Proyectos"></asp:ListItem>
                        <asp:ListItem Value="15" Text="En proceso de cierre Operaciones"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />

                    <asp:GridView ID="gvTrabajos" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                        AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                        DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="id" HeaderText="id" />
                            <asp:BoundField DataField="JobBook" HeaderText="JBI" />
                            <asp:BoundField DataField="NombreTrabajo" HeaderText="Nombre" />
                            <asp:BoundField DataField="Metodologia" HeaderText="Metodología" />
                            <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                            <asp:BoundField DataField="CoeAsignado" HeaderText="OMP" />
                            <asp:BoundField DataField="FechaTentativaInicioCampo" HeaderText="Fecha Tentativa Inicio Campo"
                                DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FechaTentativaFinalizacion" HeaderText="Fecha Tentativa Finalizacion"
                                DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="EstadoTrabajo" HeaderText="Estado" />
                            <asp:TemplateField HeaderText="Tareas" ShowHeader="False" Visible="false">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgTareas" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Tareas" ImageUrl="~/Images/Select_16.png" Text="Seleccionar" ToolTip="Tareas" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Abrir" ShowHeader="False">
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
                                        CommandName="Avance" ImageUrl="~/Images/find_16.png" Text="Actualizar" ToolTip="Ir a Avance de Campo" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <%--<asp:TemplateField HeaderText="Seguimiento" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgIrProject" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                        CommandName="Project" ImageUrl="~/Images/calendar_24.png" Text="Actualizar" ToolTip="Ir a Gantt de Planeación y Ejecución" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>--%>
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
                <asp:Panel runat="server" ID="accordion1" Visible="false">
                    <h3 style="float: left; text-align: left;">
                        <a>Información del trabajo
                        </a>
                    </h3>
                    <div class="spacer"></div>
                    <asp:HiddenField ID="hfIdTrabajo" Value="0" runat="server" />
                    <asp:HiddenField ID="hfIdProyecto" runat="server" />
                    <asp:HiddenField ID="hfIdPresupuesto" Value="0" runat="server" />
                    <asp:HiddenField ID="hfIdPropuesta" runat="server" />
                    <asp:HiddenField ID="hfIdAlternativa" runat="server" />
                    <asp:HiddenField ID="hfIdMetodologia" runat="server" />
                    <asp:HiddenField ID="hfCodigoMetodologia" runat="server" />
                    <asp:HiddenField ID="hfIdFase" runat="server" />
                    <asp:HiddenField ID="hfNoMediciones" runat="server" />
                    <asp:HiddenField ID="hfJobBook" runat="server" />
                    <asp:HiddenField ID="hfDiasCampo" runat="server" />
                    <asp:HiddenField ID="hfInternacional" runat="server" />
                    <asp:HiddenField ID="hfActualizar" runat="server" Value="0" />
                    <asp:Panel ID="pnlNewTrabajo" runat="server" Visible="false">
                        <div style="text-align: right"><a>Paso 1 de 3 <a style="font-style: italic;">(Empecemos)</a></div>
                        <p>Seleccione la opción que desea usar para el presupuesto</p>
                        <asp:GridView ID="gvOpcionesTrabajo" runat="server" EmptyDataText="No se encuentran opciones. Posiblemente no ha sido asignado el JBI. Comuníquese con Cuentas y/o Gerentes de Operaciones"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="IdPropuesta,ParAlternativa,MetCodigo,ParNacional,JoBBook,NumeroMediciones,IdMetodologia,DiasCampo" AllowPaging="False" AutoGenerateColumns="false">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="JoBBook" HeaderText="JoBBook" />
                                <asp:BoundField DataField="NombrePresupuesto" HeaderText="Nombre" />
                                <asp:BoundField DataField="Metodologia" HeaderText="Metodología" />
                                <asp:BoundField DataField="Fase" HeaderText="Fase" />
                                <asp:BoundField DataField="NumeroMediciones" HeaderText="Mediciones" />
                                <asp:BoundField DataField="Muestra" HeaderText="Muestra" />
                                <asp:TemplateField HeaderText="Usar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgUsarOpcion" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Usar" ImageUrl="~/Images/select_16.png" Text="Actualizar" ToolTip="Usar esta opción" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>

                        <asp:Button ID="btnCancelStep1" runat="server" Text="Cancelar" />
                        <div class="spacer"></div>
                    </asp:Panel>
                    <asp:Panel ID="pnlNewMuestra" runat="server" Visible="false">
                        <div style="text-align: right"><a>Paso 2 de 3 <a style="font-style: italic;">(Configuremos)</a></div>
                        <p>Configure la muestra del trabajo</p>
                        <asp:GridView ID="gvMuestraNew" runat="server" EmptyDataText="No se encuentran opciones. Posiblemente no ha sido asignado el JBI. Comuníquese con Cuentas y/o Gerentes de Operaciones"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="CiudadId" AllowPaging="False" AutoGenerateColumns="false">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:TemplateField HeaderText="Muestra" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMuestra" runat="server" Text='<%#  DataBinder.Eval(Container, "DataItem.Muestra") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Quitar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgUsarOpcion" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                            CommandName="Quitar" ImageUrl="~/Images/delete_16.png" Text="Actualizar" ToolTip="Usar esta opción" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <h4>Agregar más ciudades</h4>

                        <label>Departamento</label>
                        <asp:DropDownList ID="ddlDepartamentoNew" runat="server" AutoPostBack="true"></asp:DropDownList>
                        <label>Ciudad a agregar</label>
                        <asp:DropDownList ID="ddlCiudadNew" runat="server" AppendDataBoundItems="true">
                            <asp:ListItem Text="" Value="-1" />
                        </asp:DropDownList>
                        <label>Muestra</label>
                        <asp:TextBox ID="txtMuestraNew" runat="server"></asp:TextBox>
                        <div class="spacer"></div>
                        <asp:Button ID="btnAddMuestraNew" runat="server" Text="Agregar" />
                        <div class="spacer"></div>
                        <asp:Button ID="btnContinuarStep2" runat="server" Text="Continuar" />
                        <asp:Button ID="btnCancelStep2" runat="server" Text="Cancelar" />
                        <div class="spacer"></div>
                    </asp:Panel>
                    <asp:Panel ID="pnlNewFecha" runat="server" Visible="false">
                        <div style="text-align: right"><a>Paso 3 de 3 <a style="font-style: italic;">(Ya casi)</a></div>
                        <p>Complete las fechas, el nombre y la medición</p>
                        <div class="spacer"></div>
                        <label>
                            Nombre:</label>
                        <asp:TextBox ID="txtNombreTrabajo" runat="server"></asp:TextBox>

                        <label>
                            Número de medición:
                        </label>
                        <asp:TextBox ID="txtNoMedicion" runat="server" TextMode="Number"></asp:TextBox>
                        <div class="spacer"></div>

                        <label>
                            Fecha tentativa inicio de campo:
                        </label>
                        <asp:TextBox ID="txtFechaTentativaInicioCampo" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                        <label>
                            Fecha tentativa finalización trabajo:
                        </label>
                        <asp:TextBox ID="txtFechaTentativaFinalizacion" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>

                        <div class="clear"></div>
                        <asp:Panel ID="pnlCOE" runat="server" Visible="false">
                            <label>
                                OMP:
                            </label>
                            <asp:DropDownList ID="ddlCOE" runat="server"></asp:DropDownList>
                        </asp:Panel>
                        <div class="spacer"></div>
                        <asp:Button ID="btnContinuarStep3" runat="server" Text="Crear trabajo" />
                        <asp:Button ID="btnCancelStep3" runat="server" Text="Cancelar" />
                        <asp:Button ID="btnCancelCambioInfo" Visible="false" runat="server" Text="Cancelar" />
                        <div class="spacer"></div>
                    </asp:Panel>
                    <asp:Panel ID="PnlMuestra" runat="server" Visible="false">
                        <asp:GridView ID="gvMuestra" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                            AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                            DataKeyNames="idMuestra" AllowPaging="False" EmptyDataText="No existen registros para mostrar">
                            <PagerStyle CssClass="headerfooter ui-toolbar" />
                            <SelectedRowStyle CssClass="SelectedRow" />
                            <AlternatingRowStyle CssClass="odd" />
                            <Columns>
                                <asp:BoundField DataField="Departamento" HeaderText="Departamento" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:TemplateField HeaderText="Muestra" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtMuestra" runat="server" Text='<%#  DataBinder.Eval(Container, "DataItem.Cantidad") %>'></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbUpdateMuestra" runat="server" CausesValidation="False" CommandName="Actualizar"
                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/select_16.png"
                                            OnClientClick="return confirm('Esta seguro de actualizar este registro ?');" Text="Seleccionar" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imbRemoveMuestra" runat="server" CausesValidation="False" CommandName="Eliminar"
                                            CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png"
                                            OnClientClick="return confirm('Esta seguro de eliminar este registro ?');" Text="Seleccionar" />
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
                                                    Enabled='<%# IIf(gvMuestra.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                                    Enabled='<%# IIf(gvMuestra.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                            </td>
                                            <td>
                                                <span class="pagingLinks">[<%= gvMuestra.PageIndex + 1%>-<%= gvMuestra.PageCount%>]</span>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                    Enabled='<%# IIf((gvMuestra.PageIndex + 1) = gvMuestra.PageCount, "false", "true") %>'
                                                    SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                    Enabled='<%# IIf((gvMuestra.PageIndex + 1) = gvMuestra.PageCount, "false", "true") %>'
                                                    SkinID="paging">Ultimo »</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </PagerTemplate>
                        </asp:GridView>
                        <label>
                            Departamento</label>
                        <asp:DropDownList ID="ddlDepartamento" runat="server" AutoPostBack="True">
                        </asp:DropDownList>
                        <label>
                            Ciudad</label>
                        <asp:DropDownList ID="ddlCiudad" runat="server">
                        </asp:DropDownList>
                        <label>
                            Cantidad</label>
                        <asp:TextBox ID="tbCantidad" runat="server"></asp:TextBox>
                        <asp:Button ID="btnAddMuestra" runat="server" Text="Agregar" />
                        <div class="spacer"></div>
                    </asp:Panel>
                    <asp:Panel ID="pnlAnulacion" runat="server" Visible="false">
                        <p>Está seguro de esta opción? No podrá deshacer la anulación.</p>
                        <label>Si desea, puede agregar una observación</label>
                        <asp:TextBox ID="txtObservacionesAnulacion" runat="server"></asp:TextBox>
                        <div class="spacer"></div>
                        <asp:Button ID="btnConfirmarAnulacion" runat="server" Text="Confirmar" />
                        <asp:Button ID="btnCancelarAnulacion" runat="server" Text="Cancelar" />
                        <div class="spacer"></div>
                    </asp:Panel>
                    <asp:Panel ID="pnlCierre" runat="server" Visible="false">
                        <asp:Label ID="lblInfoCierre" Text="Está seguro de esta opción? No podrá deshacer el cierre" Visible="false" runat="server" />
                        <asp:Label ID="lblreporte" Text="REPORTE DOCUMENTOS DE CIERRE" Font-Bold="true" Visible="false" runat="server" />
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
                        <div class="spacer"></div>
                    </asp:Panel>
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
                        <div class="spacer"></div>
                        <asp:Button ID="btnMuestra" runat="server" Text="Ver / Ocultar Muestra" />
                        <asp:Button ID="btnCambiarInfo" runat="server" Text="Cambiar Información" />
                        <asp:Button ID="btnPreEntrega" runat="server" Text="PreEntrega" Visible="false" />
                        <asp:Button ID="btnFichaCuanti" runat="server" Text="Instructivo General" Visible="false" />
                        <asp:Button ID="btnInstructivoGeneral" runat="server" Text="Especificaciones Técnicas" />
                        <asp:Button ID="btnCircular" runat="server" Text="Nueva circular" />
                        <asp:Button ID="btnSegmentos" runat="server" Text="Segmentos" />
                        <asp:Button ID="btnDocumentos" runat="server" Text="Brief de Cuentas a Proyectos" Visible="false" />
                        <asp:Button ID="btnEstadoTareas" runat="server" Text="Módulo tareas" />
                        <asp:Button ID="BtnDuplicar" runat="server" Text="Duplicar Trabajo" />
                        <asp:Button ID="btnROCuestionario" runat="server" Text="RO Cuestionario" Visible="false" />
                        <asp:Button ID="btnROInstructivo" runat="server" Text="RO Instructivo" Visible="false" />
                        <asp:Button ID="btnROMaterialAyuda" runat="server" Text="RO Material Ayuda" Visible="false" />
                        <asp:Button ID="btnROMetodologia" runat="server" Text="RO Metodología" Visible="false" />
                        <asp:Button ID="btnAnularTrabajo" runat="server" Text="Anular Trabajo" />
                        <asp:Button ID="btnCerrarTrabajo" runat="server" Text="Cerrar Trabajo" OnClientClick="return confirm('Realmente desea cerrar el trabajo');" />
                        <asp:Button ID="btnReporteCierre" runat="server" Text="Reporte Cierre" Visible="false" />
                        <asp:Button ID="btnVerInfoGeneral" runat="server" Text="Ver Información General" />
                        <asp:Button id="btnVariablesControl" runat="server" Text="Variables de Control" />
                    </asp:Panel>
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
