<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage/TH_F.master"
    CodeBehind="Contratacion.aspx.vb" Inherits="WebMatrix.Contratacion" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="CPH_Head" runat="server">
    <link href="../css/jquery-ui.css" rel="stylesheet">
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

            $("#<%= txtFechaIngreso.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaIngreso.ClientId %>").datepicker({
                dateFormat: 'dd/mm/yy',
                changeMonth: true,
                changeYear: true,
                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'],
                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic']
            });

            $("#<%= txtFechaNacimiento.ClientId %>").mask("99/99/9999");
            $("#<%= txtFechaNacimiento.ClientId %>").datepicker({
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

            loadPlugins();
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

        input[type=text] {
            color: #666;
            border: 1px solid #666;
        }

        input[type=submit] {
            font-size: 12px;
        }

        select {
            color: #666;
            border: 1px solid #666;
        }
    </style>
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
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-info" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;" class="lblTitleInfo">Info: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lblTextInfo" class="lblTextInfo">
            </label>
        </div>
    </div>
    <div id="error" class="error_message ui-state-error ui-corner-all" style="display: none;">
        <div class="form_right" onclick="runEffect('info');" style="cursor: pointer; float: right; margin-right: 10px;">x</div>
        <div style="float: left; margin-left: 10px; margin-top: 5px;">
            <span class="ui-icon ui-icon-alert" style="float: left; margin-top: 0px;"></span>
            <strong style="float: left;" class="lblTitleInfo">Error: </strong>
            <br />
            <label style="float: left; display: block; width: auto;" id="lbltextError" class="lblTextInfo">
            </label>
        </div>
    </div>
    <div id="accordion">
        <div id="accordion1">
            <h3>
                <a href="#">
                    <label>
                        Personal:<asp:HiddenField ID="hfCoordinador" Value="1" runat="server" />
                    </label>
                </a>
            </h3>
            <div class="block">
                <div class="form_left">
                    <label>Búsqueda por</label>
                    <asp:TextBox ID="txtCedulaBuscar" runat="server" placeholder="Cedula"></asp:TextBox>
                    <asp:TextBox ID="txtNombreBuscar" runat="server" placeholder="Nombre"></asp:TextBox>
                    <asp:Button ID="btnBuscar" runat="server" Text="Buscar" />
                    <asp:Button ID="btnNuevo" runat="server" Text="Nuevo" />
                </div>
                <br />
                <asp:GridView ID="gvContratacion" runat="server" Width="100%" AutoGenerateColumns="False" PageSize="25"
                    CssClass="displayTable" AlternatingRowStyle-CssClass="odd" PagerStyle-CssClass="headerfooter ui-toolbar"
                    DataKeyNames="Id" AllowPaging="True" EmptyDataText="No existen registros para mostrar">
                    <PagerStyle CssClass="headerfooter ui-toolbar" />
                    <SelectedRowStyle CssClass="SelectedRow" />
                    <AlternatingRowStyle CssClass="odd" />
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="Identificacion" />
                        <asp:BoundField DataField="Nombres" HeaderText="Nombres" />
                        <asp:BoundField DataField="Apellidos" HeaderText="Apellidos" />
                        <asp:BoundField DataField="FechaIngreso" HeaderText="FechaIngreso" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="Activo" HeaderText="Activo" />
                        <asp:BoundField DataField="Contratista" HeaderText="Contratista" />
                        <asp:TemplateField HeaderText="Actualizar" ShowHeader="False">
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
                                            Enabled='<%# IIF(gvContratacion.PageIndex = 0, "false", "true") %>' SkinID="Paging">« Primero</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument="Prev" CommandName="Page"
                                            Enabled='<%# IIF(gvContratacion.PageIndex = 0, "false", "true") %>' SkinID="paging">&lt; Anterior</asp:LinkButton>
                                    </td>
                                    <td>
                                        <span class="pagingLinks">[<%= gvContratacion.PageIndex + 1%>-<%= gvContratacion.PageCount%>]</span>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton3" runat="server" CommandArgument="Next" CommandName="Page"
                                            Enabled='<%# IIF((gvContratacion.PageIndex +1) = gvContratacion.PageCount, "false", "true") %>'
                                            SkinID="paging">Siguiente &gt;</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="LinkButton4" runat="server" CommandArgument="Last" CommandName="Page"
                                            Enabled='<%# IIF((gvContratacion.PageIndex +1) = gvContratacion.PageCount, "false", "true") %>'
                                            SkinID="paging">Ultimo »</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </PagerTemplate>
                </asp:GridView>
            </div>
        </div>
        <div id="accordion0">
            <h3>
                <a href="#">
                    <label>
                        Información:
                    </label>
                </a>
            </h3>
            <div class="block">
                <fieldset class="validationGroup">
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Identificación:
                            </label>
                            <asp:HiddenField ID="hfId" runat="server" />
                            <asp:TextBox ID="txtIdentificacion" runat="server" CssClass="required text textEntry"></asp:TextBox>
                            <asp:FilteredTextBoxExtender ID="fteTxtIdentificacion" runat="server" FilterType="Numbers"
                                TargetControlID="txtIdentificacion">
                            </asp:FilteredTextBoxExtender>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Nombres:
                            </label>
                            <asp:TextBox ID="txtNombres" runat="server" CssClass="required text textEntry"></asp:TextBox>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Apellidos:
                            </label>
                            <asp:TextBox ID="txtApellidos" runat="server" CssClass="required text textEntry"></asp:TextBox>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Cargo:
                            </label>
                            <asp:DropDownList ID="ddlCargos" runat="server" CssClass="mySpecificClass dropdowntext"
                                AutoPostBack="True">
                            </asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Tipo encuestador:
                            </label>
                            <asp:DropDownList ID="ddlTipoEncuestador" runat="server" CssClass="mySpecificClass dropdowntext"
                                Enabled="False">
                            </asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Ciudad:
                            </label>
                            <asp:DropDownList ID="ddlCiudad" runat="server" CssClass="mySpecificClass dropdowntext">
                            </asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Tipo contratación:
                            </label>
                            <asp:DropDownList ID="ddlTipoContratacion" runat="server" CssClass="mySpecificClass dropdowntext">
                                <asp:ListItem Text="Contratista" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Fecha ingreso:
                            </label>
                            <asp:TextBox ID="txtFechaIngreso" runat="server" CssClass="required bgCalendar textCalendarStyle"></asp:TextBox>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Fecha nacimiento:
                            </label>
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="required bgCalendar textCalendarStyle"></asp:TextBox>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>Contratista</label>
                            <asp:DropDownList ID="ddlContratista" runat="server"></asp:DropDownList>
                        </fieldset>
                    </div>
                    <div class="form_left">
                        <fieldset>
                            <label>
                                Activo:
                            </label>
                            <asp:CheckBox ID="chkActivo" runat="server" Checked="true" />
                        </fieldset>
                    </div>
                    <div class="actions">
                        <div class="form_right">
                            <fieldset>
                                <asp:Button ID="btnGrabar" runat="server" Text="Guardar" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" />
                                <asp:Button ID="btnCrearUsuario" runat="server" Text="Crear usuario" Visible="false" />
                            </fieldset>
                        </div>
                    </div>
                </fieldset>
            </div>
        </div>
    </div>
</asp:Content>
