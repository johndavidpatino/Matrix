<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/MasterProyectos.master"
    CodeBehind="TrabajosCualitativos.aspx.vb" Inherits="WebMatrix.TrabajosCualitativos" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../Scripts/css/tipTip.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/js/libs/jquery.tipTip.minified.js" type="text/javascript"></script>
    <script src="../Scripts/js/libs/jquery.timeentry.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Scripts/jquery.blockUI.js"></script>
    <script type="text/javascript" src="../Scripts/blockUIOnAllAjaxRequests.js"></script>
    <script type="text/javascript">

        function bindPickerIni() {
            $("input[type=text][id*=<%= txtFechaTentativaInicioCampo.ClientId %>]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function bindPickerFin() {
            $("input[type=text][id*=<%= txtFechaTentativaFinalizacion.ClientId %>]").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });
        }

        function loadPlugins() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerIni);
            bindPickerIni();

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindPickerFin);
            bindPickerFin();

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

            <%--$("#<%= txtFechaInicio.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaInicio.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaFin.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaFin.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            });--%>

            <%--$('#<%= txtHoraSesion.ClientId %>').timeEntry({ show24Hours: true, spinnerImage: '', defaultTime: '00:00:00', showSeconds: true });

            $("#<%= txtFechaSesion.ClientID%>").mask("99/99/9999");
            $("#<%= txtFechaSesion.ClientID%>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            });--%>

            $('#accordion').accordion({
                change: function (event, ui) { $('html, body').animate({ scrollTop: 0 }, 'slow'); },
                header: "h3",
                autoHeight: false
            });

            $(".toolTipFunction").tipTip({
                maxWidth: "auto",
                activation: "focus",
                defaultPosition: "bottom"
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
    <style>
        .divNuevaLinea {
            width: 100%;
            float: left;
        }

        .div3Form {
            width: 33%;
            float: left;
        }

        .div2Form {
            width: 48%;
            float: left;
        }

        #stylized label {
            text-align: left;
            margin: 0px;
            margin-left: 5px;
            width: 100px;
        }

        #stylized input[type="radio"] {
            width: auto;
            float: left;
            padding: 0px;
            margin: 5px 0 0 10px;
        }

        .text70 {
            width: 70% !important;
        }

        .textFull {
            width: 100% !important;
        }

        .wAuto {
            width: auto;
        }

        .textTabla {
            margin: 0px 5px !important;
        }

        .m-0 {
            margin: 0px !important;
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
    <div style="margin: 10px 0 20px;">
        <asp:LinkButton ID="lnkProyecto" runat="server" Text="Volver"></asp:LinkButton>
    </div>
    <asp:UpdatePanel runat="server" ID="upDatos">
        <ContentTemplate>
            <asp:Panel ID="accordion0" runat="server">
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
                        <%--<asp:TemplateField HeaderText="Avance" ShowHeader="False">
                            <ItemTemplate>
                                <asp:ImageButton ID="imgIrAvance" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>"
                                    CommandName="Avance" ImageUrl="~/Images/find_16.png" Text="Actualizar" ToolTip="Ir a Avance de Campo" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Seguimiento" ShowHeader="False">
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
                <asp:HiddenField ID="hfIdSesion" Value="0" runat="server" />
                <asp:HiddenField ID="hfFechaMinMuestra" Value="" runat="server" />
                <asp:HiddenField ID="hfFechaMaxMuestra" Value="" runat="server" />
                <asp:HiddenField ID="hfNombreProyecto" Value="" runat="server" />

                <asp:Panel ID="pnlNewTrabajo" runat="server" Visible="false">
                    <div style="text-align: right"><a>Paso 1 de 3 <a style="font-style: italic;">(Empecemos)</a></div>
                    <h4>Presupuesto</h4>
                    <p>Seleccione la opción que desea usar para el presupuesto</p>
                    <br />
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
                    <br />
                    <div class="spacer">
                        <asp:Button ID="btnCancelStep1" runat="server" Text="Cancelar" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlNewMuestra" runat="server" Visible="false">
                    <div style="text-align: right"><a>Paso 2 de 3 <a style="font-style: italic;">(Configuremos)</a></div>
                    <h4>Distribución Muestral</h4>
                    <p>Configure la Distribución Muestral del trabajo</p>
                    <br />
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
                                    <asp:TextBox ID="txtMuestra" runat="server" Text='<%#  DataBinder.Eval(Container, "DataItem.Muestra") %>' CssClass="m-0"></asp:TextBox>
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
                    <asp:Panel ID="pnlPropuesta" runat="server" Visible="true">
                        <div class="spacer"></div>
                        <asp:GridView ID="gvPropuestaMuestra" runat="server" Width="100%" AutoGenerateColumns="false">
                            <Columns>
                                <asp:BoundField DataField="Fase" HeaderText="Fase" />
                                <asp:BoundField DataField="Metodologia" HeaderText="Metod" />
                                <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                                <asp:BoundField DataField="Muestra" HeaderText="Cantidad" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <h4 style="font-size: 14px; font-weight: 500;">Agregar Muestra</h4>
                    <asp:Panel ID="pnlDiseñoMuestral" runat="server">
                        <div class="spacer"></div>
                        <div class="divNuevaLinea">
                            <div class="div3Form">
                                <label style="width: 100px">Muestra</label>
                                <asp:TextBox ID="txtMuestraNew" CssClass="textEntry" runat="server"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server" FilterType="Numbers"
                                    TargetControlID="txtMuestraNew">
                                </asp:FilteredTextBoxExtender>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="[sólo números]"
                                    ControlToValidate="txtMuestraNew" Type="Double" Operator="DataTypeCheck"
                                    ValidationGroup="Guardar"></asp:CompareValidator>
                            </div>
                            <div class="div3Form">
                                <label>Departamento</label>
                                <asp:DropDownList ID="ddlDepartamentoNew" runat="server" AutoPostBack="true"></asp:DropDownList>
                            </div>
                            <div class="div3Form">
                                <label style="width: 100px">Ciudad</label>
                                <asp:DropDownList ID="ddlCiudadNew" runat="server" AppendDataBoundItems="true">
                                    <asp:ListItem Text="" Value="-1" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="spacer"></div>
                        <%--<div class="divNuevaLinea">
                            <div class="div3Form">
                                <label>Número de Backup</label>
                                <asp:TextBox ID="txtBackup" runat="server" CssClass="textEntry"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server" FilterType="Numbers"
                                    TargetControlID="txtBackup">
                                </asp:FilteredTextBoxExtender>
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="[sólo números]"
                                    ControlToValidate="txtBackup" Type="Double" Operator="DataTypeCheck"
                                    ValidationGroup="Guardar"></asp:CompareValidator>
                            </div>
                            <div class="div3Form">
                                <label style="width: 100px">Fecha Inicio</label>
                                <asp:TextBox ID="txtFechaInicio" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </div>
                            <div class="div3Form">
                                <label style="width: 100px">Fecha Fin</label>
                                <asp:TextBox ID="txtFechaFin" runat="server" CssClass="bgCalendar textCalendarStyle"></asp:TextBox>
                            </div>
                        </div>--%>
                        <div class="divNuevaLinea" style="display: none;">
                            <div class="div3Form">
                                <label style="width: 100px">Moderador</label>
                                <asp:DropDownList ID="ddlModerador" runat="server" CssClass="dropdowntext"></asp:DropDownList>
                            </div>
                        </div>
                    </asp:Panel>

                    <div class="spacer" />
                    <asp:Button ID="btnAddMuestraNew" runat="server" Text="Agregar" />

                    <asp:GridView ID="gvDistribucion" Visible="false" runat="server" AllowPaging="False" AlternatingRowStyle-CssClass="odd" AutoGenerateColumns="false"
                        DataKeyNames="CiudadId" EmptyDataText="No se encuentran opciones. Posiblemente no ha sido asignado el JBI. Comuníquese con Cuentas y/o Gerentes de Operaciones" PagerStyle-CssClass="headerfooter ui-toolbar">
                        <PagerStyle CssClass="headerfooter ui-toolbar" />
                        <SelectedRowStyle CssClass="SelectedRow" />
                        <AlternatingRowStyle CssClass="odd" />
                        <Columns>
                            <asp:BoundField DataField="Ciudad" HeaderText="Ciudad" />
                            <asp:BoundField DataField="CiudadId" HeaderText="CiudadId" Visible="false" />
                            <asp:TemplateField HeaderText="Cantidad" ShowHeader="False" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMuestra" runat="server" CssClass="text70 textTabla" Text='<%#  DataBinder.Eval(Container, "DataItem.Cantidad") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Número Backup" ShowHeader="False" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtNumeroBackup" runat="server" CssClass="text70 textTabla" Text='<%#  DataBinder.Eval(Container, "DataItem.NumeroBackup") %>'></asp:TextBox>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="FechaInicio" HeaderText="Inicio" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="FechaFin" HeaderText="Fin" DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:TemplateField HeaderText="Quitar" ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgUsarOpcion" runat="server" CausesValidation="False" CommandArgument="<%# CType(Container, GridViewRow).RowIndex %>" CommandName="Quitar" ImageUrl="~/Images/delete_16.png" Text="Actualizar" ToolTip="Usar esta opción" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <div class="spacer">
                        <asp:Button ID="btnContinuarStep2" runat="server" Text="Continuar" />
                        <asp:Button ID="btnCancelStep2" runat="server" Text="Cancelar" />
                        <asp:Button ID="btnAtrasStep2" runat="server" Text="Atrás" />
                    </div>
                </asp:Panel>

                <%--<asp:Panel ID="pnlNewAyudas" runat="server" Visible="false">
                    <div style="text-align: right"><a>Paso 3 de 4 <a style="font-style: italic;">(Incentivos y Ayudas Adicionales)</a></div>
                    <h4>Backups Necesarios</h4>
                    <asp:Panel ID="pnlBackupNecesarios" runat="server" Visible="true">
                        <div class="spacer"></div>
                        <div class="divNuevaLinea">
                            <div class="div2Form">
                                <label>Backups</label>
                                <asp:TextBox ID="txtBackups" runat="server" CssClass="textEntry"></asp:TextBox>
                            </div>
                        </div>
                        <div class="divNuevaLinea">
                            <div class="div2Form">
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" FilterType="Numbers"
                                    TargetControlID="txtBackups">
                                </asp:FilteredTextBoxExtender>
                            </div>
                            <div class="div2Form">
                                <asp:CompareValidator ID="cvbackups" runat="server" ErrorMessage="[sólo números]"
                                    ControlToValidate="txtBackups" Type="Double" Operator="DataTypeCheck"
                                    ValidationGroup="Guardar"></asp:CompareValidator>
                            </div>
                        </div>
                    </asp:Panel>
                    <br />
                    <h4>Incentivos a utilizar</h4>
                    <asp:Panel ID="pnlIncentivos" runat="server" Visible="true">
                        <div class="spacer"></div>
                        <div class="divNuevaLinea" style="margin-bottom: -15px !important;">
                            <div class="div2Form">
                                <label style="width: 150px;">Incentivos Económicos</label>
                                <asp:RadioButtonList ID="rblIncentivos" runat="server" Width="50%" BackColor="Transparent" RepeatDirection="Horizontal" AutoPostBack="True">
                                    <asp:ListItem Value="1">Si</asp:ListItem>
                                    <asp:ListItem Selected="True" Value="0">No</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="div2Form">
                                <label style="width: 70px; float: left;">Presupuesto</label>
                                <asp:TextBox ID="txtPresupuestoIncentivo" runat="server" CssClass="textEntry text70" Enabled="false"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="fteTxtJobBook" runat="server" FilterType="Numbers"
                                    TargetControlID="txtPresupuestoIncentivo">
                                </asp:FilteredTextBoxExtender>
                                <asp:CompareValidator ID="cvpresupuestoincentivo" runat="server" ErrorMessage="[sólo números]"
                                    ControlToValidate="txtPresupuestoIncentivo" Type="Double" Operator="DataTypeCheck"
                                    ValidationGroup="Guardar"></asp:CompareValidator>
                            </div>
                        </div>
                        <div class="divNuevaLinea">
                            <label style="width: 100%;">Distribución Incentivos Económicos:</label>
                            <div class="textFull">
                                <asp:TextBox ID="txtDistribucionIncentivo" runat="server" CssClass="textMultiline" Width="100%"
                                    Height="100px" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                        <div class="spacer"></div>
                        <div class="divNuevaLinea" style="margin-bottom: -15px !important;">
                            <div class="div3Form">
                                <label>Regalos Cliente</label>
                                <asp:RadioButtonList ID="rblRegaloClientes" runat="server" CssClass="wAuto" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1">Si</asp:ListItem>
                                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="div3Form">
                                <label>Compra Ipsos</label>
                                <asp:RadioButtonList ID="rblCompraIpsos" runat="server" CssClass="wAuto" RepeatDirection="Horizontal" AutoPostBack="True">
                                    <asp:ListItem Value="1">Si</asp:ListItem>
                                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="div3Form">
                                <label style="width: 70px; float: left;">Presupuesto</label>
                                <asp:TextBox ID="txtPresupuesto" runat="server" CssClass="textEntry text70" Enabled="false"></asp:TextBox>
                                <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" FilterType="Numbers"
                                    TargetControlID="txtPresupuesto">
                                </asp:FilteredTextBoxExtender>
                                <asp:CompareValidator ID="cvPresupuesto" runat="server" ErrorMessage="[sólo números]"
                                    ControlToValidate="txtPresupuesto" Type="Double" Operator="DataTypeCheck" ValidationGroup="Guardar"></asp:CompareValidator>
                            </div>
                        </div>

                        <div class="divNuevaLinea">
                            <label style="width: 100%;">Distribución Compra Ipsos:</label>
                            <div class="textFull">
                                <asp:TextBox ID="txtDistribucionCompra" runat="server" CssClass="textMultiline" Width="100%"
                                    Height="100px" TextMode="MultiLine" Enabled="false"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>

                    <h4>Ayudas Adicionales</h4>
                    <asp:Panel ID="pnlAyudaAdicionales" runat="server" Visible="true">
                        <div class="spacer"></div>
                        <div class="divNuevaLinea" style="margin-bottom: -15px !important;">
                            <label style="width: 150px;">Incentivos Económicos</label>
                            <asp:CheckBoxList ID="chbAyudas" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                            </asp:CheckBoxList>
                        </div>
                        <br />
                        <div class="divNuevaLinea" style="margin-top: 30px !important;">
                            <label style="width: 100%;">Método Aceptable de Reclutamiento</label>
                            <asp:CheckBoxList ID="chbReclutamiento" runat="server" RepeatDirection="Horizontal">
                            </asp:CheckBoxList>
                        </div>
                        <br />
                        <div class="divNuevaLinea">
                            <label style="width: 100%;">Observaciones, Exclusiones y Restricciones Específicas</label>
                            <div class="textFull">
                                <asp:TextBox ID="txtExclusionesyRestricciones" runat="server" CssClass="textMultiline" Width="100%"
                                    Height="100px" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="divNuevaLinea">
                            <label style="width: 100%;">Recursos Propiedad del Cliente</label>
                            <div class="textFull">
                                <asp:TextBox ID="txtRecursosPropiedadesCliente" runat="server" CssClass="textMultiline" Width="100%"
                                    Height="100px" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="divNuevaLinea">
                            <label style="width: 100%;">Requerimientos De Habeas Data/ Confidencialidad/ Privacidad Y Seguridad De La Información</label>
                            <div class="textFull">
                                <asp:TextBox ID="txtHabeasData" runat="server" CssClass="textMultiline" Width="100%" Height="100px" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="divNuevaLinea">
                            <label style="width: 100%;">Variables de control</label>
                            <div class="textFull">
                                <asp:TextBox ID="txtVariablesControl" runat="server" CssClass="textMultiline" Width="100%" Height="100px" TextMode="MultiLine"></asp:TextBox>
                            </div>
                        </div>
                    </asp:Panel>
                    <div class="spacer">
                        <asp:Button ID="btnContinuarStep3" runat="server" Text="Continuar" />
                        <asp:Button ID="btnCancelStep3" runat="server" Text="Cancelar" />

                    </div>
                </asp:Panel>--%>

                <asp:Panel ID="pnlNewFecha" runat="server" Visible="false">
                    <div style="text-align: right"><a>Paso 3 de 3 <a style="font-style: italic;">(Ya casi)</a></div>
                    <h4>Complete las fechas, el nombre y la medición</h4>
                    <div class="spacer"></div>
                    <div class="divNuevaLinea" style="">
                        <div class="div3Form">
                            <label>Nombre</label>
                            <asp:TextBox ID="txtNombreTrabajo" runat="server"></asp:TextBox>
                        </div>
                        <div class="div3Form">
                            <label>Fecha Inicio de Campo</label>
                            <asp:TextBox ID="txtFechaTentativaInicioCampo" runat="server" CssClass="bgCalendar textCalendarStyle" AutoPostBack="true"></asp:TextBox>
                        </div>
                        <div class="div3Form">
                            <label>Fecha Cierre de Campo</label>
                            <asp:TextBox ID="txtFechaTentativaFinalizacion" runat="server" CssClass="bgCalendar textCalendarStyle" AutoPostBack="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="divNuevaLinea" style="display: none;">
                        <div class="div2Form">
                            <label>Número de medición</label>
                            <asp:TextBox ID="txtNoMedicion" runat="server" Text="1"></asp:TextBox>
                        </div>
                    </div>

                    <div class="spacer"></div>
                    <asp:Panel ID="pnlCOE" runat="server" Visible="false">
                        <label>
                            OMP:
                        </label>
                        <asp:DropDownList ID="ddlCOE" runat="server"></asp:DropDownList>
                    </asp:Panel>
                    <div class="spacer">
                        <asp:Button ID="btnContinuarStep4" runat="server" Text="Crear trabajo" />
                        <asp:Button ID="btnCancelStep4" runat="server" Text="Cancelar" />
                        <asp:Button ID="btnAtrasStep4" runat="server" Text="Atrás" />
                        <asp:Button ID="btnCancelCambioInfo" Visible="false" runat="server" Text="Cancelar" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="PnlMuestra" runat="server" Visible="false">
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
                    &nbsp;
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
                                                        CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/select_16.png"
                                                        OnClientClick="return confirm('Esta seguro de actualizar este registro ?');" Text="Seleccionar" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Eliminar" ShowHeader="False">
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imbRemoveMuestra" runat="server" CausesValidation="False" CommandName="Eliminar"
                                                        CommandArgument="<%# Ctype(Container, GridViewRow).RowIndex %>" ImageUrl="~/Images/delete_16.png"
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
                                                                Enabled='<%# IIF(gvMuestra.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <span class="pagingLinks">[<%= gvMuestra.PageIndex + 1%>-<%= gvMuestra.PageCount%>]</span>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                                                Enabled='<%# IIF((gvMuestra.PageIndex +1) = gvMuestra.PageCount, "false", "true") %>'
                                                                SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                                                Enabled='<%# IIF((gvMuestra.PageIndex +1) = gvMuestra.PageCount, "false", "true") %>'
                                                                SkinID="paging">Ultimo »</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </PagerTemplate>
                                    </asp:GridView>

                </asp:Panel>
                <asp:Panel ID="pnlAnulacion" runat="server" Visible="false">
                    <p>Está seguro de esta opción? No podrá deshacer la anulación.</p>
                    <label>Si desea, puede agregar una observación</label>
                    <asp:TextBox ID="txtObservacionesAnulacion" runat="server"></asp:TextBox>
                    <div class="actions"></div>
                    <asp:Button ID="btnConfirmarAnulacion" runat="server" Text="Confirmar" />
                    <asp:Button ID="btnCancelarAnulacion" runat="server" Text="Cancelar" />
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
                </asp:Panel>
                <asp:Panel ID="pnlInfoTrabajo" runat="server" Visible="false">
                    <div style="width: 20%; float: left; display: block;">
                        <label style="width: auto; margin-left: 0px;">
                            Nombre Trabajo:
                        </labe>
                        <br />
                            <div style="margin-left: -10px;">
                                <asp:TextBox ID="txtNombre" runat="server" ReadOnly="true"></asp:TextBox>
                            </div>
                    </div>
                    <div style="width: 20%; float: left; display: block;">
                        <label style="width: auto; margin-left: 0px;">
                            Metodología:
                        </label>
                        <br />
                        <div style="margin-left: -10px;">
                            <asp:TextBox ID="txtMetodologia" runat="server" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div style="width: 20%; float: left; display: block;">
                        <label style="width: auto; margin-left: 0px;">
                            Muestra:
                        </label>
                        <br />
                        <div style="margin-left: -10px;">
                            <asp:TextBox ID="txtMuestra" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="fteTxtMuestra" runat="server" FilterType="Numbers"
                                TargetControlID="txtMuestra">
                            </asp:FilteredTextBoxExtender>
                        </div>
                    </div>
                    <div style="width: 20%; float: left; display: block;">
                        <label style="width: auto; margin-left: 0px;">
                            Medicion:
                        </label>
                        <br />
                        <div style="margin-left: -10px;">
                            <asp:TextBox ID="txtMedicion" runat="server" ReadOnly="true"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Numbers"
                                TargetControlID="txtMedicion">
                            </asp:FilteredTextBoxExtender>
                        </div>
                    </div>
                    <div style="width: 20%; float: left; display: block;">
                        <label style="width: auto; margin-left: 0px;">
                            Estado del Trabajo:
                        </label>
                        <br />
                        <asp:TextBox ID="lblEstadoTrabajo" runat="server" ReadOnly="true"></asp:TextBox>
                    </div>
                    <br />
                    <br />

                    <div class="spacer" />
                    <asp:Button ID="btnMuestra" runat="server" Text="Ver / Ocultar Muestra" Visible="false" />
                    <asp:Button ID="btnCambiarInfo" runat="server" Text="Cambiar Información" Visible="false" />
                    <asp:Button ID="btnPreEntrega" runat="server" Text="PreEntrega" Visible="false" />
                    <asp:Button ID="btnFicha" runat="server" Text="Especificaciones Técnicas" Visible="true" />
                    <asp:Button ID="btnCircular" runat="server" Text="Nueva circular" Visible="false" />
                    <asp:Button ID="btnSegmentos" runat="server" Text="Segmentos" Visible="false" />
                    <asp:Button ID="btnEntrevistas" runat="server" Text="Entrevistas" Visible="false" />
                    <asp:Button ID="btnSesiones" runat="server" Text="Sesiones" Visible="false" />
                    <asp:Button ID="btnInHome" runat="server" Text="In Home" Visible="false" />
                    <asp:Button ID="btnFiltroReclutamiento" runat="server" Text="Filtro Reclutamiento" Visible="false" />
                    <asp:Button ID="btnFiltroAsistencia" runat="server" Text="Filtro Asistencia" Visible="false" />
                    <asp:Button ID="btnDocumentos" runat="server" Text="Brief de Cuentas a Proyectos" Visible="false" />
                    <asp:Button ID="btnEstadoTareas" runat="server" Text="Módulo tareas" Visible="true" />
                    <asp:Button ID="BtnDuplicar" runat="server" Text="Duplicar Trabajo" Visible="false" />
                    <asp:Button ID="btnROCuestionario" runat="server" Text="RO Cuestionario" Visible="false" />
                    <asp:Button ID="btnROInstructivo" runat="server" Text="RO Instructivo" Visible="false" />
                    <asp:Button ID="btnROMaterialAyuda" runat="server" Text="RO Material Ayuda" Visible="false" />
                    <asp:Button ID="btnROMetodologia" runat="server" Text="RO Metodología" Visible="false" />
                    <asp:Button ID="btnAnularTrabajo" runat="server" Text="Anular Trabajo" Visible="true" />
                    <asp:Button ID="btnCerrarTrabajo" runat="server" Text="Cerrar Trabajo" Visible="true" />
                    <asp:Button ID="btnReporteCierre" runat="server" Text="Reporte Cierre" Visible="false" />
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
